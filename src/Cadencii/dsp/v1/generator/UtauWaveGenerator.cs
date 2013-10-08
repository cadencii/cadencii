/*
 * UtauWaveGenerator.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using cadencii.media;
using cadencii.vsq;
using cadencii;
using cadencii.java.awt;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.utau;

namespace cadencii
{

    /// <summary>
    /// UTAUの合成器(または互換合成器)を用いて波形を合成する波形生成器
    /// </summary>
    public class UtauWaveGenerator : WaveUnit, WaveGenerator
    {
        public const string FILEBASE = "temp.wav";
        private const int MAX_CACHE = 512;
        private const int BUFLEN = 1024;
        private const int VERSION = 0;
        private static SortedDictionary<string, ValuePair<string, Double>> mCache = new SortedDictionary<string, ValuePair<string, Double>>();
        private const int BASE_TEMPO = 120;

        private List<RenderQueue> mResamplerQueue = new List<RenderQueue>();
        private double[] mLeft;
        private double[] mRight;

        private VsqFileEx mVsq;
        private string mResampler;
        private string mWavtool;
        private string mTempDir;
        //private bool mAbortRequired = false;
        private bool mRunning = false;

        private long mTotalSamples;
        private WaveReceiver mReceiver = null;
        private long mTotalAppend = 0;
        private int mTrack;
        private double[] mBufferL = new double[BUFLEN];
        private double[] mBufferR = new double[BUFLEN];
        private double mTrimRemainSeconds = 0.0;
        private int mSampleRate;
        /// <summary>
        /// whdから読み込んだサンプリングレート．
        /// 波形処理ラインのサンプリングレートと違う可能性がある
        /// </summary>
        private int mThisSampleRate = 44100;
        private RateConvertContext mContext = null;
        private WorkerState mState;
        private bool mUseWideCharacterWorkaround = false;
        // 作成したジャンクションのリスト
        private List<string> mJunctions = new List<string>();
#if DEBUG
        /// <summary>
        /// ログを出さない設定の時true
        /// </summary>
        private bool mIsQuiet = false;

        private bool mUseShortTemp = false;
        private bool mUseShortResampler = false;
        private bool mUseShortWavtool = false;
        private bool mUseShortVoicebank = false;
#endif

#if DEBUG
        public void setDebugCondition(bool use_short_temp, bool use_short_resampler, bool use_short_wavtool, bool use_short_voicebank)
        {
            mUseShortTemp = use_short_temp;
            mUseShortResampler = use_short_resampler;
            mUseShortWavtool = use_short_wavtool;
            mUseShortVoicebank = use_short_voicebank;
        }

        public void setQuiet(bool value)
        {
            mIsQuiet = value;
        }
#endif

        public int getSampleRate()
        {
            return mSampleRate;
        }

        public bool isRunning()
        {
            return mRunning;
        }

        public long getTotalSamples()
        {
            return mTotalSamples;
        }

        public double getProgress()
        {
            if (mTotalSamples <= 0) {
                return 0.0;
            } else {
                return mTotalAppend / (double)mTotalSamples;
            }
        }

        /*public void stop()
        {
            if ( mRunning ) {
                mAbortRequired = true;
                while ( mRunning ) {
                    Thread.Sleep( 100 );
                }
            }
        }*/

        public override void setConfig(string parameter)
        {
            // do nothing
        }

        public override int getVersion()
        {
            return VERSION;
        }

        /// <summary>
        /// 初期化メソッド．
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="track"></param>
        /// <param name="start_clock"></param>
        /// <param name="end_clock"></param>
        public void init(VsqFileEx vsq, int track, int start_clock, int end_clock, int sample_rate)
        {
#if DEBUG
            setQuiet(true);
#endif
            mTrack = track;
            int resampler_index = VsqFileEx.getTrackResamplerUsed(vsq.Track[track]);
            int resampler_count = mConfig.getResamplerCount();
            if (resampler_count <= resampler_index) {
                resampler_index = resampler_count - 1;
            }
            if (resampler_index < 0) {
                resampler_index = 0;
            }
            mUseWideCharacterWorkaround = mConfig.UseWideCharacterWorkaround;
            mResampler = mConfig.getResamplerAt(resampler_index);
            mWavtool = mConfig.PathWavtool;
#if DEBUG
            sout.println("UtauWaveGenerator#init; mResampler=" + mResampler + "; exists=" + System.IO.File.Exists(mResampler));
            sout.println("UtauWaveGenerator#init; mWavtool=" + mWavtool + "; exists=" + System.IO.File.Exists(mWavtool));
#endif
            mSampleRate = sample_rate;
            string id = AppManager.getID();
            mTempDir = Path.Combine(AppManager.getCadenciiTempDir(), id);
            if (mUseWideCharacterWorkaround) {
                string junction_path = System.IO.Path.Combine(getSystemRoot(), "cadencii_" + id + "_temp");
                if (!Directory.Exists(junction_path)) {
                    cadencii.helper.Utils.MountPointCreate(junction_path, mTempDir);
                    mJunctions.Add(junction_path);
                }
                mTempDir = junction_path;
            }
#if DEBUG
            sout.println("UtauWaveGenerator#init; mTempDir=" + mTempDir + "; exists=" + Directory.Exists(mTempDir));
#endif
            mVsq = (VsqFileEx)vsq.clone();
            mVsq.updateTotalClocks();

            if (end_clock < vsq.TotalClocks) {
                // 末尾の部分は不要なので削除
                mVsq.removePart(end_clock, mVsq.TotalClocks + 480);
            }

            double trim_sec = 0.0;
            if (start_clock > 0) {
                // 途中からの合成が指示された場合
                // 0clockからstart_clockまでを削除する
                // もしstart_clock位置に音符があれば，その音符の先頭から合成し，trim_secを適切に設定する

                // まず，start_clockに音符があるかどうかを調べる
                // 音符があれば，trim_endに適切な値を代入
                VsqTrack vsq_track = mVsq.Track[track];
                int c = vsq_track.getEventCount();
                int trim_end = start_clock;
                for (int i = 0; i < c; i++) {
                    VsqEvent itemi = vsq_track.getEvent(i);
                    if (itemi.ID.type != VsqIDType.Anote) {
                        continue;
                    }
                    if (itemi.Clock <= start_clock && start_clock < itemi.Clock + itemi.ID.getLength()) {
                        trim_end = itemi.Clock;
                        break;
                    }
                }

                if (trim_end == start_clock) {
                    trim_sec = 0.0;
                } else {
                    trim_sec = mVsq.getSecFromClock(start_clock) - mVsq.getSecFromClock(trim_end);
                }

                // 必要ならトリムを実行
                if (0 < trim_end) {
                    mVsq.removePart(0, trim_end);
                }
            }
            mVsq.adjustClockToMatchWith(BASE_TEMPO);
            mVsq.updateTotalClocks();

            mTrimRemainSeconds = trim_sec;
        }

        private static string getSystemRoot()
        {
            string system = System.Environment.GetFolderPath(Environment.SpecialFolder.System);
            return System.IO.Path.GetPathRoot(system);
        }

        /*private static String getShortPathName( String path )
        {
#if DEBUG
            String before = path;
#endif
            if ( path == null ) {
                return "";
            }
            if ( path == "" ) {
                return path;
            }
            if ( fsys.isFileExists( path ) || fsys.isDirectoryExists( path ) ) {
                const int LEN = 260;
                System.Text.StringBuilder sb_path = new System.Text.StringBuilder( LEN );
                if ( win32.GetShortPathNameW( path, sb_path, LEN ) != 0 ) {
                    path = sb_path.ToString();
                }
            }
#if DEBUG
            sout.println( "UtauWaveGenerator#getShortPathName; before=" + before + "; after=" + path );
#endif
            return path;
        }*/

        public void setReceiver(WaveReceiver r)
        {
            if (mReceiver != null) {
                mReceiver.end();
            }
            mReceiver = r;
        }

        public long getPosition()
        {
            return mTotalAppend;
        }

        public static void clearCache()
        {
            foreach (var key in mCache.Keys) {
                ValuePair<string, Double> value = mCache[key];
                string file = value.getKey();
                try {
                    PortUtil.deleteFile(file);
                } catch (Exception ex) {
                    serr.println("UtauWaveGenerator#clearCache; ex=" + ex);
                    Logger.write("UtauWaveGenerator::clearCache; ex=" + ex + "\n");
                }
            }
            mCache.Clear();
        }

        public void begin(long total_samples, WorkerState state)
        {
            mState = state;
            mTotalSamples = total_samples;
#if MAKEBAT_SP
            StreamWriter bat = null;
            StreamWriter log = null;
#endif
#if DEBUG
            System.IO.StreamWriter sw = new System.IO.StreamWriter("UtauWaveGenerator.begin(long).log");
            System.IO.StreamWriter sw2 = new System.IO.StreamWriter("UtauWaveGenerator.begin(long).notes.log");
#endif
            try {
                double sample_length = mVsq.getSecFromClock(mVsq.TotalClocks) * mSampleRate;
                //mAbortRequired = false;
                mRunning = true;
                if (!Directory.Exists(mTempDir)) {
                    PortUtil.createDirectory(mTempDir);
                }

#if MAKEBAT_SP
                log = new StreamWriter( Path.Combine( m_temp_dir, "UtauWaveGenerator.log" ), false, Encoding.GetEncoding( "Shift_JIS" ) );
#endif
                // 原音設定を読み込み
                VsqTrack target = mVsq.Track[mTrack];

                string file = Path.Combine(mTempDir, FILEBASE);
                if (System.IO.File.Exists(file)) {
                    PortUtil.deleteFile(file);
                }
                string file_whd = Path.Combine(mTempDir, FILEBASE + ".whd");
                if (System.IO.File.Exists(file_whd)) {
                    PortUtil.deleteFile(file_whd);
                }
                string file_dat = Path.Combine(mTempDir, FILEBASE + ".dat");
                if (System.IO.File.Exists(file_dat)) {
                    PortUtil.deleteFile(file_dat);
                }
#if DEBUG
                debugWriteLine("UtauWaveGenerator#run; temp_dir=" + mTempDir);
                debugWriteLine("UtauWaveGenerator#run; file_whd=" + file_whd);
                debugWriteLine("UtauWaveGenerator#run; file_dat=" + file_dat);
#endif

                int count = -1;
                double sec_end = 0;
                double sec_end_old = 0;
                int program_change = 0;
                mResamplerQueue.Clear();
#if DEBUG
                double error_sum = 0.0;
#endif

                // 前後の音符の先行発音やオーバーラップやらを取得したいので、一度リストに格納する
                List<VsqEvent> events = new List<VsqEvent>();
                foreach (var itemi in target.getNoteEventIterator()) {
                    events.Add(itemi);
#if DEBUG
                    sw2.WriteLine(itemi.Clock + "\t" + itemi.ID.Note * 100);
                    sw2.WriteLine((itemi.Clock + itemi.ID.getLength()) + "\t" + itemi.ID.Note * 100);
#endif
                }

#if MAKEBAT_SP
                log.WriteLine( "making resampler queue..." );
#endif
                int events_count = events.Count;
                for (int k = 0; k < events_count; k++) {
                    VsqEvent item = events[k];
#if MAKEBAT_SP
                    log.Write( "    #" + k + "; clock=" + item.Clock );
#endif
                    VsqEvent singer_event = target.getSingerEventAt(item.Clock);
                    if (singer_event == null) {
                        program_change = 0;
                    } else {
                        program_change = singer_event.ID.IconHandle.Program;
                    }
                    string singer_raw = "";
                    string singer = "";
                    if (0 <= program_change && program_change < mConfig.UtauSingers.Count) {
                        singer_raw = mConfig.UtauSingers[program_change].VOICEIDSTR;
                        singer = singer_raw;
                        if (mUseWideCharacterWorkaround) {
                            string junction = Path.Combine(getSystemRoot(), "cadencii_" + AppManager.getID() + "_singer_" + program_change);
                            if (!Directory.Exists(junction)) {
                                cadencii.helper.Utils.MountPointCreate(junction, singer_raw);
                                mJunctions.Add(junction);
                            }
                            singer = junction;
                        }
                    }
#if DEBUG
                    sout.println("UtauWaveGenerator#begin; singer=" + singer + "; singer_raw=" + singer_raw);
#endif
#if MAKEBAT_SP
                    log.Write( "; pc=" + program_change );
#endif
                    if (state.isCancelRequested()) {
                        exitBegin();
                        return;
                    }
                    count++;
                    double sec_start = mVsq.getSecFromClock(item.Clock);
                    double sec_start_act = sec_start - item.UstEvent.getPreUtterance() / 1000.0;
                    sec_end_old = sec_end;
                    sec_end = mVsq.getSecFromClock(item.Clock + item.ID.getLength());
                    double sec_end_act = sec_end;
                    VsqEvent item_next = null;
                    if (k + 1 < events_count) {
                        item_next = events[k + 1];
                    }
                    if (item_next != null) {
                        double sec_start_act_next =
                            mVsq.getSecFromClock(item_next.Clock) - item_next.UstEvent.getPreUtterance() / 1000.0
                            + item_next.UstEvent.getVoiceOverlap() / 1000.0;
                        if (sec_start_act_next < sec_end_act) {
                            sec_end_act = sec_start_act_next;
                        }
                    }
                    //float t_temp = (float)(item.ID.getLength() / (sec_end - sec_start) / 8.0);
                    if ((count == 0 && sec_start > 0.0) || (sec_start > sec_end_old)) {
                        // 最初の音符，
                        double sec_start2 = sec_end_old;
                        double sec_end2 = sec_start;
                        // t_temp2がBASE_TEMPOから大きく外れないように
                        int draft_length = (int)((sec_end2 - sec_start2) * 8.0 * BASE_TEMPO);
                        //float t_temp2 = (float)(draft_length / (sec_end2 - sec_start2) / 8.0);
                        //String str_t_temp2 = PortUtil.formatDecimal( "0.00", t_temp2 );
                        //double act_t_temp2 = PortUtil.parseDouble( str_t_temp2 );
#if DEBUG
                        //error_sum += (draft_length / (act_t_temp2 * 8.0)) - (sec_end2 - sec_start2);
#endif
                        RenderQueue rq = new RenderQueue();
                        //rq.WavtoolArgPrefix = "\"" + file + "\" \"" + fsys.combine( singer, "R.wav" ) + "\" 0 " + draft_length + "@" + BASE_TEMPO;
                        rq.WavtoolArgPrefix.Clear();
                        rq.WavtoolArgPrefix.Add("\"" + file + "\"");
                        rq.WavtoolArgPrefix.Add("\"" + Path.Combine(singer, "R.wav") + "\"");
                        rq.WavtoolArgPrefix.Add("0");
                        rq.WavtoolArgPrefix.Add(draft_length + "@" + BASE_TEMPO);
                        //rq.WavtoolArgSuffix = " 0 0";
                        rq.WavtoolArgSuffix.Clear();
                        rq.WavtoolArgSuffix.Add("0");
                        rq.WavtoolArgSuffix.Add("0");
                        rq.Oto = new OtoArgs();
                        rq.FileName = "";
                        rq.secStart = sec_start2;
                        rq.ResamplerFinished = true;
                        mResamplerQueue.Add(rq);
                        count++;
                    }
                    string lyric = item.ID.LyricHandle.L0.Phrase;
                    string note = NoteStringFromNoteNumber(item.ID.Note);
                    int millisec = (int)((sec_end_act - sec_start_act) * 1000) + 50;

                    OtoArgs oa = new OtoArgs();
                    if (AppManager.mUtauVoiceDB.ContainsKey(singer_raw)) {
                        UtauVoiceDB db = AppManager.mUtauVoiceDB[singer_raw];
                        oa = db.attachFileNameFromLyric(lyric, item.ID.Note);
                    }
#if MAKEBAT_SP
                    log.Write( "; lyric=" + lyric + "; fileName=" + oa.fileName );
#endif
                    oa.msPreUtterance = item.UstEvent.getPreUtterance();
                    oa.msOverlap = item.UstEvent.getVoiceOverlap();
#if DEBUG
                    debugWriteLine("UtauWaveGenerator#run; oa.fileName=" + oa.fileName);
                    debugWriteLine("UtauWaveGenerator#run; lyric=" + lyric);
#endif
                    RenderQueue rq2 = new RenderQueue();
                    string wavPath = "";
                    if (oa.fileName != null && oa.fileName.Length > 0) {
                        wavPath = Path.Combine(singer, oa.fileName);
                    } else {
                        wavPath = Path.Combine(singer, lyric + ".wav");
                    }
#if DEBUG
                    debugWriteLine("UtauWaveGenerator#run; wavPath=" + wavPath);
#endif
                    string[] resampler_arg_prefix = new string[] { "\"" + wavPath + "\"" };
                    string[] resampler_arg_suffix = new string[]{
                        "\"" + note + "\"",
                        "100",
                        "\"" + item.UstEvent.Flags + "\"",
                        oa.msOffset + "",
                        millisec + "",
                        oa.msConsonant + "",
                        oa.msBlank + "",
                        item.UstEvent.getIntensity() + "",
                        item.UstEvent.getModuration() + "" };

                    // ピッチを取得
                    List<string> pitch = new List<string>();
                    bool allzero = true;
                    int delta_clock = 5;  //ピッチを取得するクロック間隔
                    int tempo = BASE_TEMPO;
                    double delta_sec = delta_clock / (8.0 * tempo); //ピッチを取得する時間間隔

                    // sec_start_act～sec_end_actまでの，item.ID.Note基準のピッチベンドを取得
                    // ただしdelta_sec秒間隔で
                    double sec = mVsq.getSecFromClock(item.Clock) - (item.UstEvent.getPreUtterance() + item.UstEvent.getStartPoint()) / 1000.0;
                    int indx = 0;
                    int base_note = item.ID.Note;
                    double sec_vibstart = mVsq.getSecFromClock(item.Clock + item.ID.VibratoDelay);
                    int totalcount = 0;

                    VibratoPointIteratorBySec vibitr = null;
                    if (item.ID.VibratoHandle != null) {
                        vibitr = new VibratoPointIteratorBySec(mVsq,
                                                               item.ID.VibratoHandle.getRateBP(),
                                                               item.ID.VibratoHandle.getStartRate(),
                                                               item.ID.VibratoHandle.getDepthBP(),
                                                               item.ID.VibratoHandle.getStartDepth(),
                                                               item.Clock + item.ID.VibratoDelay,
                                                               item.ID.getLength() - item.ID.VibratoDelay,
                                                               (float)delta_sec);
                    }

#if DEBUG
                    string logname =
                        Path.Combine(mTempDir, k + "_" + PortUtil.getFileNameWithoutExtension(wavPath) + "_" + note + ".log");
                    System.IO.StreamWriter sw3 = new System.IO.StreamWriter(logname);
                    int prevx = 0;
                    float max = -100;
                    float min = 12800;
#endif
                    while (sec <= sec_end) {
                        // clockでの音符の音の高さを調べる
                        // ピッチベンドを調べたい時刻
                        int clock = (int)mVsq.getClockFromSec(sec);
                        // dst_noteに，clockでの，音符のノートナンバー(あれば．なければ元の音符と同じ値)
                        int dst_note = base_note;
                        if (k > 0) {
                            VsqEvent prev = events[k - 1];
                            dst_note = base_note;
                        }
                        for (int i = indx; i < events_count; i++) {
                            VsqEvent itemi = events[i];
                            if (clock < itemi.Clock) {
                                continue;
                            }
                            int itemi_length = itemi.ID.getLength();
                            if (itemi.Clock <= clock && clock < itemi.Clock + itemi_length) {
                                dst_note = itemi.ID.Note;
                                indx = i;
                                break;
                            }
                        }

                        // PIT, PBSによるピッチベンドを加味
                        double pvalue = (dst_note - base_note) * 100.0 + target.getPitchAt(clock);

                        // ビブラートがあれば，ビブラートによるピッチベンドを加味
                        if (sec_vibstart <= sec && vibitr != null && vibitr.hasNext()) {
                            PointD pd = vibitr.next();
                            pvalue += pd.getY() * 100.0;
                        }

                        // リストに入れる
                        if (totalcount == 0) {
                            pitch.Add(PortUtil.formatDecimal("0.00", pvalue) + "Q" + tempo);
                        } else {
                            pitch.Add(PortUtil.formatDecimal("0.00", pvalue));
                        }
                        totalcount++;
#if DEBUG
                        float ty = (float)pvalue + base_note * 100;
                        max = Math.Max(max, ty);
                        min = Math.Min(min, ty);
                        prevx = clock;
                        sw3.WriteLine(clock + "\t" + ty);
                        sw.WriteLine(clock + "\t" + pvalue + "\t" + dst_note + "\t" + base_note + "\t" + target.getPitchAt(clock));
#endif
                        if (pvalue != 0.0) {
                            allzero = false;
                        }

                        // 次
                        sec += delta_sec;
                    }
#if DEBUG
                    int delta = 20;
                    sw3.WriteLine(prevx + "\t" + (min - delta));
                    sw3.WriteLine((item.Clock + item.ID.getLength()) + "\t" + (min - delta));
                    sw3.WriteLine((item.Clock + item.ID.getLength()) + "\t" + (max + delta));
                    sw3.WriteLine((item.Clock + item.ID.getLength()) + "\t" + (min - delta));
                    sw3.WriteLine(item.Clock + "\t" + (min - delta));
                    sw3.WriteLine(item.Clock + "\t" + (max + delta));
                    sw3.Close();
#endif

                    //4_あ_C#4_550.wav
                    //String md5_src = "";
                    rq2.hashSource = "";
                    foreach (string s in resampler_arg_prefix) {
                        rq2.hashSource += s + " ";
                    }
                    foreach (string s in resampler_arg_suffix) {
                        rq2.hashSource += s + " ";
                    }
                    foreach (string s in pitch) {
                        rq2.hashSource += s + " ";
                    }
                    rq2.hashSource += mResampler;
                    //#if DEBUG
                    //                    String filename =
                    //                        fsys.combine( mTempDir, k + "_" + PortUtil.getFileNameWithoutExtension( wavPath ) + "_" + note + ".wav" );
                    //#else
                    string filename =
                        Path.Combine(mTempDir, PortUtil.getMD5FromString(mCache.Count + rq2.hashSource) + ".wav");
                    //#endif

                    rq2.appendArgRange(resampler_arg_prefix);
                    rq2.appendArg("\"" + filename + "\"");
                    rq2.appendArgRange(resampler_arg_suffix);
                    if (!allzero) {
                        rq2.appendArgRange(pitch.ToArray());
                    }

                    bool exist_in_cache = mCache.ContainsKey(rq2.hashSource);
                    if (!exist_in_cache) {
                        if (mCache.Count + 1 >= MAX_CACHE) {
                            double old = PortUtil.getCurrentTime();
                            string delfile = "";
                            string delkey = "";
                            foreach (var key in mCache.Keys) {
                                ValuePair<string, Double> value = mCache[key];
                                if (old < value.getValue()) {
                                    old = value.getValue();
                                    delfile = value.getKey();
                                    delkey = key;
                                }
                            }
                            try {
                                PortUtil.deleteFile(delfile);
                            } catch (Exception ex) {
                                serr.println("UtauWaveGenerator#begin; ex=" + ex);
                                Logger.write("UtauWaveGenerator#begin(long): ex=" + ex + "\n");
                            }
                            mCache.Remove(delkey);
                        }
                        //mCache.put( search_key, new ValuePair<String, Double>( filename, PortUtil.getCurrentTime() ) );
                        //->ここ，実際の合成が終わったタイミングで追加するようにする
                    } else {
                        filename = mCache[rq2.hashSource].getKey();
                    }

                    string str_t_temp = PortUtil.formatDecimal("0.00", BASE_TEMPO);
#if DEBUG
                    double act_t_temp = double.Parse(str_t_temp);
                    error_sum += (item.ID.getLength() / (8.0 * act_t_temp)) - (sec_end - sec_start);
                    Logger.write("UtauWaveGenerator#begin; error_sum=" + error_sum + "\n");
#endif
                    //rq2.WavtoolArgPrefix = "\"" + file + "\" \"" + filename + "\" " + item.UstEvent.getStartPoint() + " " + item.ID.getLength() + "@" + str_t_temp;
                    rq2.WavtoolArgPrefix.Clear();
                    rq2.WavtoolArgPrefix.Add("\"" + file + "\"");
                    rq2.WavtoolArgPrefix.Add("\"" + filename + "\"");
                    rq2.WavtoolArgPrefix.Add("" + item.UstEvent.getStartPoint());
                    rq2.WavtoolArgPrefix.Add("" + item.ID.getLength() + "@" + str_t_temp);
                    UstEnvelope env = item.UstEvent.getEnvelope();
                    if (env == null) {
                        env = new UstEnvelope();
                    }
                    //rq2.WavtoolArgSuffix = " " + env.p1 + " " + env.p2 + " " + env.p3 + " " + env.v1 + " " + env.v2 + " " + env.v3 + " " + env.v4;
                    //rq2.WavtoolArgSuffix += " " + oa.msOverlap + " " + env.p4 + " " + env.p5 + " " + env.v5;
                    rq2.WavtoolArgSuffix.Clear();
                    rq2.WavtoolArgSuffix.Add("" + env.p1);
                    rq2.WavtoolArgSuffix.Add("" + env.p2);
                    rq2.WavtoolArgSuffix.Add("" + env.p3);
                    rq2.WavtoolArgSuffix.Add("" + env.v1);
                    rq2.WavtoolArgSuffix.Add("" + env.v2);
                    rq2.WavtoolArgSuffix.Add("" + env.v3);
                    rq2.WavtoolArgSuffix.Add("" + env.v4);
                    rq2.WavtoolArgSuffix.Add("" + oa.msOverlap);
                    rq2.WavtoolArgSuffix.Add("" + env.p4);
                    rq2.WavtoolArgSuffix.Add("" + env.p5);
                    rq2.WavtoolArgSuffix.Add("" + env.v5);
                    rq2.Oto = oa;
                    rq2.FileName = filename;
                    rq2.secStart = sec_start_act;
                    rq2.ResamplerFinished = exist_in_cache;
                    mResamplerQueue.Add(rq2);
#if MAKEBAT_SP
                    log.WriteLine();
#endif
                }
#if MAKEBAT_SP
                log.WriteLine( "...done" );
#endif

                int num_queues = mResamplerQueue.Count;
                int processed_sample = 0; //WaveIncomingで受け渡した波形の合計サンプル数
                int channel = 0; // .whdに記録されたチャンネル数
                int byte_per_sample = 0;
                // 引き続き、wavtoolを呼ぶ作業に移行
                bool first = true;
                //int trim_remain = (int)( trimMillisec / 1000.0 * VSTiProxy.SAMPLE_RATE); //先頭から省かなければならないサンプル数の残り
                VsqBPList dyn_curve = mVsq.Track[mTrack].getCurve("dyn");
#if MAKEBAT_SP
                bat = new StreamWriter( Path.Combine( m_temp_dir, "utau.bat" ), false, Encoding.GetEncoding( "Shift_JIS" ) );
#endif
                for (int i = 0; i < num_queues; i++) {
                    RenderQueue rq = mResamplerQueue[i];
                    if (!rq.ResamplerFinished) {
#if MAKEBAT_SP
                        bat.WriteLine( "\"" + mResampler + "\" " + rq.getResamplerArgString() );
#endif

                        Process process = null;
                        try {
                            process = new Process();
                            process.StartInfo.FileName = "\"" + mResampler + "\"";
                            process.StartInfo.Arguments = rq.getResamplerArgString();
#if DEBUG
                            sout.println("UtauWaveGenerator#begin; FileName=" + process.StartInfo.FileName);
                            sout.println("UtauWaveGenerator#begin; Arguments=" + process.StartInfo.Arguments);
#endif
                            process.StartInfo.WorkingDirectory = mTempDir;
                            process.StartInfo.CreateNoWindow = true;
                            process.StartInfo.UseShellExecute = false;
                            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                            process.Start();
                            process.WaitForExit();

                            // 合成が済んだのでキャッシュに登録する
                            mCache[rq.hashSource] = new ValuePair<string, Double>(rq.FileName, PortUtil.getCurrentTime());
                        } catch (Exception ex) {
                            Logger.write(typeof(UtauWaveGenerator) + ".begin; ex=" + ex + "\n");
#if DEBUG
                            sout.println(typeof(UtauWaveGenerator) + ".begin; ex=" + ex);
#endif
                        } finally {
                            if (process != null) {
                                process.Dispose();
                            }
                        }
                    }
                    if (state.isCancelRequested()) {
                        break;
                    }

                    // wavtoolを起動
                    double sec_fin; // 今回のwavtool起動によってレンダリングが完了したサンプル長さ
                    RenderQueue p = mResamplerQueue[i];
                    OtoArgs oa_next;
                    if (i + 1 < num_queues) {
                        oa_next = mResamplerQueue[i + 1].Oto;
                    } else {
                        oa_next = new OtoArgs();
                    }

                    // この後のwavtool呼び出しで，どこまで波形が確定するか？
                    // 安全のために，wavtoolでくっつける音符の先頭位置までが確定するだろう，ということにする
                    sec_fin = p.secStart;
                    if (i + 1 == num_queues) {
                        // 最後の音符だった場合は，最後まで読み取ることにする
                        sec_fin = mTotalSamples / (double)mSampleRate;
                    }
#if DEBUG
                    debugWriteLine("UtauWaveGenerator#run; sec_fin=" + sec_fin);
#endif
                    float mten = p.Oto.msPreUtterance + oa_next.msOverlap - oa_next.msPreUtterance;
                    //String arg_wavtool = p.WavtoolArgPrefix + (mten >= 0 ? ("+" + mten) : ("-" + (-mten))) + p.WavtoolArgSuffix;
                    List<string> arg_wavtool = new List<string>();
                    int size = p.WavtoolArgPrefix.Count;
                    for (int j = 0; j < size; j++) {
                        string s = p.WavtoolArgPrefix[j];
                        if (j == size - 1) {
                            s += (mten >= 0 ? ("+" + mten) : ("-" + (-mten)));
                        }
                        arg_wavtool.Add(s);
                    }
                    size = p.WavtoolArgSuffix.Count;
                    for (int j = 0; j < size; j++) {
                        arg_wavtool.Add(p.WavtoolArgSuffix[j]);
                    }
#if MAKEBAT_SP
                    bat.WriteLine( "\"" + m_wavtool + "\" " + arg_wavtool );
#endif
                    processWavtool(arg_wavtool, file, mTempDir, mWavtool);

                    // できたwavを読み取ってWaveIncomingイベントを発生させる
                    int sample_end = (int)(sec_fin * mSampleRate);
#if DEBUG
                    debugWriteLine("UtauWaveGenerator#run; sample_end=" + sample_end);
#endif
                    // whdを読みに行く
                    if (first) {
                        Stream whd = null;
                        // このファイルのサンプリングレート．ヘッダで読み込むけど初期値はコレにしとく
                        mThisSampleRate = 44100;
                        try {
                            whd = new FileStream(file_whd, FileMode.Open, FileAccess.Read);
                            #region whdを読みに行く
                            whd.Seek(0, SeekOrigin.Begin);
                            // RIFF
                            byte[] buf = new byte[4];
                            int gcount = whd.Read(buf, 0, 4);
                            if (buf[0] != 'R' || buf[1] != 'I' || buf[2] != 'F' || buf[3] != 'F') {
#if DEBUG
                                debugWriteLine("UtauWaveGenerator#run; whd header error");
#endif
                                continue;
                            }
                            // ファイルサイズ
                            whd.Read(buf, 0, 4);
                            // WAVE
                            whd.Read(buf, 0, 4);
                            if (buf[0] != 'W' || buf[1] != 'A' || buf[2] != 'V' || buf[3] != 'E') {
#if DEBUG
                                debugWriteLine("UtauWaveGenerator#run; whd header error");
#endif
                                continue;
                            }
                            // fmt 
                            whd.Read(buf, 0, 4);
                            if (buf[0] != 'f' || buf[1] != 'm' || buf[2] != 't' || buf[3] != ' ') {
#if DEBUG
                                debugWriteLine("UtauWaveGenerator#run; whd header error");
#endif
                                continue;
                            }
                            // fmt チャンクのサイズ
                            whd.Read(buf, 0, 4);
                            long loc_end_of_fmt = whd.Position; //fmtチャンクの終了位置．ここは一定値でない可能性があるので読込み
                            loc_end_of_fmt += buf[0] | buf[1] << 8 | buf[2] << 16 | buf[3] << 24;
                            // format ID
                            whd.Read(buf, 0, 2);
                            int id = buf[0] | buf[1] << 8;
                            if (id != 0x0001) { //0x0001はリニアPCM
                                continue;
                            }
                            // チャンネル数
                            whd.Read(buf, 0, 2);
                            channel = buf[1] << 8 | buf[0];
                            // サンプリングレート
                            whd.Read(buf, 0, 4);
                            mThisSampleRate = PortUtil.make_int32_le(buf);//.__BBBBBBBBBAAAAAAAAAAAAAAAAAAAAAAAARRRRRRRR__stderr buf[0] | buf[1] << 8 | buf[2] << 16 | buf[3] << 24;
#if DEBUG
                            debugWriteLine("UtauWaveGenerator#begin; mThisSampleRate=" + mThisSampleRate);
#endif
                            // データ速度
                            whd.Read(buf, 0, 4);
                            // ブロックサイズ
                            whd.Read(buf, 0, 2);
                            // 1チャンネル、1サンプルあたりのビット数
                            whd.Read(buf, 0, 2);
                            int bit_per_sample = buf[1] << 8 | buf[0];
                            byte_per_sample = bit_per_sample / 8;
                            whd.Seek(loc_end_of_fmt, SeekOrigin.Begin);
                            // data
                            whd.Read(buf, 0, 4);
                            if (buf[0] != 'd' || buf[1] != 'a' || buf[2] != 't' || buf[3] != 'a') {
#if DEBUG
                                debugWriteLine("UtauWaveGenerator#run; whd header error");
#endif
                                continue;
                            }
                            // size of data chunk
                            whd.Read(buf, 0, 4);
                            //int size = buf[3] << 24 | buf[2] << 16 | buf[1] << 8 | buf[0];
                            //int total_samples = size / (channel * byte_per_sample);
                            #endregion
                        } catch (Exception ex) {
                            serr.println("UtauWaveGenerator#begin; ex=" + ex);
                            Logger.write("UtauWaveGenerator::begin(long); ex=" + ex + "\n");
                        } finally {
                            if (whd != null) {
                                try {
                                    whd.Close();
                                } catch (Exception ex2) {
                                    serr.println("UtauWaveGenerator#begin; ex2=" + ex2);
                                    Logger.write("UtauWaveGenerator::begin(long); ex=" + ex2 + "\n");
                                }
                            }
                        }
                        first = false;
                    }

                    // datを読みに行く
                    int sampleFrames = sample_end - processed_sample;
#if DEBUG
                    debugWriteLine("UtauWaveGenerator#run; sampleFrames=" + sampleFrames + "; channel=" + channel + "; byte_per_sample=" + byte_per_sample);
#endif
                    if (channel > 0 && byte_per_sample > 0 && sampleFrames > 0) {
                        int length = (sampleFrames > mSampleRate ? mSampleRate : sampleFrames);
                        int remain = sampleFrames;
                        mLeft = new double[length];
                        mRight = new double[length];
                        double k_inv64 = 1.0 / 64.0;
                        double k_inv128 = 1.0 / 128.0;
                        double k_inv32768 = 1.0 / 32768.0;
                        int buflen = 1024;
                        byte[] wavbuf = new byte[buflen];
                        int pos = 0;
                        Stream dat = null;
                        try {
                            dat = new FileStream(file_dat, FileMode.Open, FileAccess.Read);
                            dat.Seek(processed_sample * channel * byte_per_sample, SeekOrigin.Begin);
                            double sec_start = processed_sample / (double)mSampleRate;
                            double sec_per_sa = 1.0 / (double)mSampleRate;
                            ByRef<int> index = new ByRef<int>(0);
                            #region チャンネル数／ビット深度ごとの読み取り操作
                            if (byte_per_sample == 1) {
                                if (channel == 1) {
                                    while (remain > 0) {
                                        if (state.isCancelRequested()) {
                                            break;
                                        }
                                        int len = dat.Read(wavbuf, 0, buflen);
                                        if (len <= 0) {
                                            break;
                                        }
                                        int c = 0;
                                        while (len > 0 && remain > 0) {
                                            if (state.isCancelRequested()) {
                                                break;
                                            }
                                            len -= 1;
                                            remain--;
                                            double gtime_dyn = sec_start + pos * sec_per_sa;
                                            int clock = (int)mVsq.getClockFromSec(gtime_dyn);
                                            int dyn = dyn_curve.getValue(clock, index);
                                            double amp = dyn * k_inv64;
                                            double v = ((0xff & wavbuf[c]) - 128) * k_inv128 * amp;
                                            c++;
                                            mLeft[pos] = v;
                                            mRight[pos] = v;
                                            pos++;
                                            if (pos >= length) {
                                                waveIncoming(mLeft, mRight, mLeft.Length, mThisSampleRate);
                                                pos = 0;
                                            }
                                        }
                                    }
                                } else {
                                    while (remain > 0) {
                                        if (state.isCancelRequested()) {
                                            break;
                                        }
                                        int len = dat.Read(wavbuf, 0, buflen);
                                        if (len <= 0) {
                                            break;
                                        }
                                        int c = 0;
                                        while (len > 0 && remain > 0) {
                                            if (state.isCancelRequested()) {
                                                break;
                                            }
                                            len -= 2;
                                            remain--;
                                            double gtime_dyn = sec_start + pos * sec_per_sa;
                                            int clock = (int)mVsq.getClockFromSec(gtime_dyn);
                                            int dyn = dyn_curve.getValue(clock, index);
                                            double amp = dyn * k_inv64;
                                            double vl = ((0xff & wavbuf[c]) - 128) * k_inv128 * amp;
                                            double vr = ((0xff & wavbuf[c + 1]) - 128) * k_inv128 * amp;
                                            mLeft[pos] = vl;
                                            mRight[pos] = vr;
                                            c += 2;
                                            pos++;
                                            if (pos >= length) {
                                                waveIncoming(mLeft, mRight, mLeft.Length, mThisSampleRate);
                                                pos = 0;
                                            }
                                        }
                                    }
                                }
                            } else if (byte_per_sample == 2) {
                                if (channel == 1) {
                                    while (remain > 0) {
                                        if (state.isCancelRequested()) {
                                            break;
                                        }
                                        int len = dat.Read(wavbuf, 0, buflen);
                                        if (len <= 0) {
                                            break;
                                        }
                                        int c = 0;
                                        while (len > 0 && remain > 0) {
                                            if (state.isCancelRequested()) {
                                                break;
                                            }
                                            len -= 2;
                                            remain--;
                                            double gtime_dyn = sec_start + pos * sec_per_sa;
                                            int clock = (int)mVsq.getClockFromSec(gtime_dyn);
                                            int dyn = dyn_curve.getValue(clock, index);
                                            double amp = dyn * k_inv64;
                                            double v = ((short)(PortUtil.make_int16_le(wavbuf, c))) * k_inv32768 * amp;
                                            mLeft[pos] = v;
                                            mRight[pos] = v;
                                            c += 2;
                                            pos++;
                                            if (pos >= length) {
                                                waveIncoming(mLeft, mRight, mLeft.Length, mThisSampleRate);
                                                pos = 0;
                                            }
                                        }
                                    }
                                } else {
                                    while (remain > 0) {
                                        if (state.isCancelRequested()) {
                                            break;
                                        }
                                        int len = dat.Read(wavbuf, 0, buflen);
                                        if (len <= 0) {
                                            break;
                                        }
                                        int c = 0;
                                        while (len > 0 && remain > 0) {
                                            if (state.isCancelRequested()) {
                                                break;
                                            }
                                            len -= 4;
                                            remain--;
                                            double gtime_dyn = sec_start + pos * sec_per_sa;
                                            int clock = (int)mVsq.getClockFromSec(gtime_dyn);
                                            int dyn = dyn_curve.getValue(clock, index);
                                            double amp = dyn * k_inv64;
                                            double vl = ((short)(PortUtil.make_int16_le(wavbuf, c))) * k_inv32768 * amp;
                                            double vr = ((short)(PortUtil.make_int16_le(wavbuf, c + 2))) * k_inv32768 * amp;
                                            mLeft[pos] = vl;
                                            mRight[pos] = vr;
                                            c += 4;
                                            pos++;
                                            if (pos >= length) {
                                                waveIncoming(mLeft, mRight, mLeft.Length, mThisSampleRate);
                                                pos = 0;
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        } catch (Exception ex) {
                            serr.println("UtauWaveGenerator#run; ex=" + ex);
                            Logger.write("UtauWaveGenerator::begin(long); ex=" + ex + "\n");
                        } finally {
                            if (dat != null) {
                                try {
                                    dat.Close();
                                } catch (Exception ex2) {
                                    serr.println("UtauWaveGenerator#run; ex2=" + ex2);
                                    Logger.write(typeof(UtauWaveGenerator) + "::begin(long); ex=" + ex2 + "\n");
                                }
                                dat = null;
                            }
                        }

                        if (state.isCancelRequested()) {
                            //mAbortRequired = false;
                            exitBegin();
                            return;
                        }
#if DEBUG
                        debugWriteLine("UtauWaveGenerator#run; calling WaveIncoming...");
#endif
                        if (pos > 0) {
                            waveIncoming(mLeft, mRight, pos, mThisSampleRate);
                        }
                        mLeft = null;
                        mRight = null;
                        GC.Collect();
#if DEBUG
                        debugWriteLine("UtauWaveGenerator#run; ...done(calling WaveIncoming)");
#endif
                        processed_sample += (sampleFrames - remain);
                    }
                }

#if MAKEBAT_SP
                bat.Close();
                bat = null;
#endif

                int tremain = (int)(mTotalSamples - mTotalAppend);
#if DEBUG
                debugWriteLine("UtauWaveGenerator#run; tremain=" + tremain);
#endif
                for (int i = 0; i < BUFLEN; i++) {
                    mBufferL[i] = 0.0;
                    mBufferR[i] = 0.0;
                }
                while (tremain > 0 && !state.isCancelRequested()) {
                    int amount = (tremain > BUFLEN) ? BUFLEN : tremain;
                    waveIncoming(mBufferL, mBufferR, amount, mThisSampleRate);
                    tremain -= amount;
                }
            } catch (Exception ex) {
                serr.println("UtauWaveGenerator.begin; ex=" + ex);
                Logger.write(typeof(UtauWaveGenerator) + ".begin; ex=" + ex + "\n");
            } finally {
#if MAKEBAT_SP
                if ( bat != null ) {
                    bat.WriteLine( "copy \"" + m_temp_dir + "\\temp.wav.whd\" /b + \"" + m_temp_dir + "\\temp.wav.dat\" /b \"" + m_temp_dir + "\\temp.wav\" /b" );
                    bat.Close();
                    bat = null;
                }
                if ( log != null ) {
                    log.Close();
                }
#endif
#if DEBUG
                sw.Close();
                sw2.Close();
#endif
                exitBegin();
                state.reportComplete();
            }
        }

        private void debugWriteLine(string value)
        {
#if DEBUG
            if (!mIsQuiet) {
                sout.println(value);
            }
#endif
        }

        /// <summary>
        /// beginメソッドを抜けるときの共通処理を行います
        /// </summary>
        private void exitBegin()
        {
            mRunning = false;
            //mAbortRequired = false;
            mReceiver.end();

            // ジャンクションを消す
            foreach (string junction in mJunctions) {
                PortUtil.deleteDirectory(junction);
            }
        }

        private void processWavtool(List<string> arg, string filebase, string temp_dir, string wavtool)
        {
            Process process = null;
            try {
                process = new Process();
                process.StartInfo.FileName = "\"" + wavtool + "\"";
                string argument = "";
                int size = arg.Count;
                for (int i = 0; i < size; i++) {
                    argument += arg[i] + (i == size - 1 ? "" : " ");
                }
                /*if ( __a != arg ) {
                    serr.println( "UtauWaveGenerator#processWavtool; warning; (__a != arg);" );
                    serr.println( "  __a=" + __a );
                    serr.println( "  arg=" + arg );
                }*/
                process.StartInfo.Arguments = argument;
                process.StartInfo.WorkingDirectory = temp_dir;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
#if DEBUG
                sout.println("UtauWaveGenerator#processWavTool; .FileName=" + process.StartInfo.FileName);
                sout.println("UtauWaveGenerator#processWavtool; .Arguments=" + process.StartInfo.Arguments);
#endif
                process.Start();
                process.WaitForExit();
            } catch (Exception ex) {
                serr.println(typeof(UtauWaveGenerator) + ".processWavtool; ex=" + ex);
                Logger.write(typeof(UtauWaveGenerator) + ".processWavtool; ex=" + ex + "\n");
            } finally {
                if (process != null) {
                    process.Dispose();
                }
            }
        }

        private void waveIncoming(double[] l, double[] r, int length, int sample_rate)
        {
            int offset = 0;
            int mTrimRemain = (int)(mTrimRemainSeconds * sample_rate);
            if (mTrimRemain > 0) {
                if (length <= mTrimRemain) {
                    mTrimRemainSeconds -= (length / sample_rate);
                    // mTrimRemain -= length;
                    return;
                } else {
                    mTrimRemainSeconds = 0.0;
                    //mTrimRemain = 0;
                    offset += length;// -= mTrimRemain;
                }
            }
            int remain = length - offset;

            if (mContext == null) {
                try {
                    mContext = new RateConvertContext(sample_rate, mSampleRate);
                } catch (Exception ex) {
                    mContext = null;
                }
            } else {
                if (mContext.getSampleRateFrom() != sample_rate ||
                     mContext.getSampleRateTo() != mSampleRate) {
                    mContext.dispose();
                    mContext = null;
                    try {
                        mContext = new RateConvertContext(sample_rate, mSampleRate);
                    } catch (Exception ex) {
                        mContext = null;
                    }
                }
            }
            if (mContext == null) {
                mTotalAppend += length;
                mState.reportProgress(mTotalAppend);
                return;
            }

            while (remain > 0) {
                int amount = (remain > BUFLEN) ? BUFLEN : remain;
                for (int i = 0; i < amount; i++) {
                    mBufferL[i] = l[i + offset];
                    mBufferR[i] = r[i + offset];
                }
                while (RateConvertContext.convert(mContext, mBufferL, mBufferR, amount)) {
                    mReceiver.push(mContext.bufferLeft, mContext.bufferRight, mContext.length);
                    mTotalAppend += mContext.length;
                    mState.reportProgress(mTotalAppend);
                }
                remain -= amount;
                offset += amount;
            }
        }

        private static string NoteStringFromNoteNumber(int note_number)
        {
            int odd = note_number % 12;
            string head = (new string[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" })[odd];
            return head + (note_number / 12 - 1);
        }
    }

}
