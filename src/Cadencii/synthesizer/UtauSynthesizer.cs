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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using cadencii.dsp.v2.generator;
using cadencii.media;
using cadencii.utau;
using cadencii.vsq;

namespace cadencii.synthesizer
{

    /// <summary>
    /// UTAUの合成器(または互換合成器)を用いて波形を合成する波形生成器
    /// </summary>
    public class UtauSynthesizer : ISingingSynthesizer
    {
        class Session : IDisposable
        {
            public List<RenderQueue> resampler_queue_ = new List<RenderQueue>();
            public float[] left_;
            public float[] right_;
            public VsqFile sequence_;
            public string resampler_;
            public string temp_dir_;
            public int track_index_;
            public float[] buffer_left_ = new float[BUFLEN];
            public float[] buffer_right_ = new float[BUFLEN];
            public int sample_rate_;
            /// <summary>
            /// whdから読み込んだサンプリングレート．
            /// 波形処理ラインのサンプリングレートと違う可能性がある
            /// </summary>
            public int this_sample_rate_ = 44100;
            // 作成したジャンクションのリスト
            public List<string> junctions_ = new List<string>();

            public void Dispose()
            {
                // ジャンクションを消す
                foreach (var junction in junctions_) {
                    PortUtil.deleteDirectory(junction);
                }
            }
        }

        public event RenderCallback Rendered;

        public const string FILEBASE = "temp.wav";
        private const int MAX_CACHE = 512;
        private const int BUFLEN = 1024;
        private const int VERSION = 0;
        private static SortedDictionary<string, ValuePair<string, Double>> cache_ = new SortedDictionary<string, ValuePair<string, Double>>();
        private const int BASE_TEMPO = 120;

        private Session session_;
        private object session_mutex_ = new object();

        private IEnumerable<string> resamplers_;
        private string wavtool_path_;
        private List<SingerConfig> singer_configs_;
        private bool use_wide_character_workaround_ = false;

        public UtauSynthesizer(IEnumerable<string> resamplers, string wavtool_path, List<SingerConfig> singer_configs, bool use_widecharacter_workaround)
        {
            resamplers_ = resamplers;
            wavtool_path_ = wavtool_path;
            singer_configs_ = singer_configs;
            use_wide_character_workaround_ = use_widecharacter_workaround;
        }

        public void beginSession(VsqFile sequence, int track_index, int sample_rate)
        {
            lock (session_mutex_) {
                session_ = new Session();
                session_.track_index_ = track_index;
                int resampler_index = VsqFileEx.getTrackResamplerUsed(sequence.Track[track_index]);
                session_.resampler_ = resamplers_.ElementAtOrDefault(resampler_index);
                session_.sample_rate_ = sample_rate;
                string id = AppManager.getID();
                session_.temp_dir_ = Path.Combine(AppManager.getCadenciiTempDir(), id);
                if (use_wide_character_workaround_) {
                    string junction_path = System.IO.Path.Combine(getSystemRoot(), "cadencii_" + id + "_temp");
                    if (!Directory.Exists(junction_path)) {
                        cadencii.helper.Utils.MountPointCreate(junction_path, session_.temp_dir_);
                        session_.junctions_.Add(junction_path);
                    }
                    session_.temp_dir_ = junction_path;
                }
                session_.sequence_ = (VsqFile)sequence.clone();
                session_.sequence_.updateTotalClocks();

                session_.sequence_.adjustClockToMatchWith(BASE_TEMPO);
                session_.sequence_.updateTotalClocks();
            }
        }

        public void endSession()
        {
            lock (session_mutex_) {
                if (session_ != null) {
                    session_.Dispose();
                }
                session_ = null;
            }
        }

        private static string getSystemRoot()
        {
            string system = System.Environment.GetFolderPath(Environment.SpecialFolder.System);
            return System.IO.Path.GetPathRoot(system);
        }

        public static void clearCache()
        {
            foreach (var key in cache_.Keys) {
                ValuePair<string, Double> value = cache_[key];
                string file = value.getKey();
                try {
                    PortUtil.deleteFile(file);
                } catch (Exception ex) {
                    serr.println("UtauWaveGenerator#clearCache; ex=" + ex);
                    Logger.write("UtauWaveGenerator::clearCache; ex=" + ex + "\n");
                }
            }
            cache_.Clear();
        }

        public void start()
        {
            try {
                double sample_length = session_.sequence_.getSecFromClock(session_.sequence_.TotalClocks) * session_.sample_rate_;
                if (!Directory.Exists(session_.temp_dir_)) {
                    PortUtil.createDirectory(session_.temp_dir_);
                }

                // 原音設定を読み込み
                VsqTrack target = session_.sequence_.Track[session_.track_index_];

                string file = Path.Combine(session_.temp_dir_, FILEBASE);
                if (System.IO.File.Exists(file)) {
                    PortUtil.deleteFile(file);
                }
                string file_whd = Path.Combine(session_.temp_dir_, FILEBASE + ".whd");
                if (System.IO.File.Exists(file_whd)) {
                    PortUtil.deleteFile(file_whd);
                }
                string file_dat = Path.Combine(session_.temp_dir_, FILEBASE + ".dat");
                if (System.IO.File.Exists(file_dat)) {
                    PortUtil.deleteFile(file_dat);
                }

                int count = -1;
                double sec_end = 0;
                double sec_end_old = 0;
                int program_change = 0;
                session_.resampler_queue_.Clear();

                // 前後の音符の先行発音やオーバーラップやらを取得したいので、一度リストに格納する
                List<VsqEvent> events = new List<VsqEvent>();
                foreach (var itemi in target.getNoteEventIterator()) {
                    events.Add(itemi);
                }

                int events_count = events.Count;
                for (int k = 0; k < events_count; k++) {
                    VsqEvent item = events[k];
                    VsqEvent singer_event = target.getSingerEventAt(item.Clock);
                    if (singer_event == null) {
                        program_change = 0;
                    } else {
                        program_change = singer_event.ID.IconHandle.Program;
                    }
                    string singer_raw = "";
                    string singer = "";
                    if (0 <= program_change && program_change < singer_configs_.Count) {
                        singer_raw = singer_configs_[program_change].VOICEIDSTR;
                        singer = singer_raw;
                        if (use_wide_character_workaround_) {
                            string junction = Path.Combine(getSystemRoot(), "cadencii_" + AppManager.getID() + "_singer_" + program_change);
                            if (!Directory.Exists(junction)) {
                                cadencii.helper.Utils.MountPointCreate(junction, singer_raw);
                                session_.junctions_.Add(junction);
                            }
                            singer = junction;
                        }
                    }
                    count++;
                    double sec_start = session_.sequence_.getSecFromClock(item.Clock);
                    double sec_start_act = sec_start - item.UstEvent.getPreUtterance() / 1000.0;
                    sec_end_old = sec_end;
                    sec_end = session_.sequence_.getSecFromClock(item.Clock + item.ID.getLength());
                    double sec_end_act = sec_end;
                    VsqEvent item_next = null;
                    if (k + 1 < events_count) {
                        item_next = events[k + 1];
                    }
                    if (item_next != null) {
                        double sec_start_act_next =
                            session_.sequence_.getSecFromClock(item_next.Clock) - item_next.UstEvent.getPreUtterance() / 1000.0
                            + item_next.UstEvent.getVoiceOverlap() / 1000.0;
                        if (sec_start_act_next < sec_end_act) {
                            sec_end_act = sec_start_act_next;
                        }
                    }
                    if ((count == 0 && sec_start > 0.0) || (sec_start > sec_end_old)) {
                        // 最初の音符，
                        double sec_start2 = sec_end_old;
                        double sec_end2 = sec_start;
                        // t_temp2がBASE_TEMPOから大きく外れないように
                        int draft_length = (int)((sec_end2 - sec_start2) * 8.0 * BASE_TEMPO);
                        RenderQueue rq = new RenderQueue();
                        rq.WavtoolArgPrefix.Clear();
                        rq.WavtoolArgPrefix.Add("\"" + file + "\"");
                        rq.WavtoolArgPrefix.Add("\"" + Path.Combine(singer, "R.wav") + "\"");
                        rq.WavtoolArgPrefix.Add("0");
                        rq.WavtoolArgPrefix.Add(draft_length + "@" + BASE_TEMPO);
                        rq.WavtoolArgSuffix.Clear();
                        rq.WavtoolArgSuffix.Add("0");
                        rq.WavtoolArgSuffix.Add("0");
                        rq.Oto = new OtoArgs();
                        rq.FileName = "";
                        rq.secStart = sec_start2;
                        rq.ResamplerFinished = true;
                        session_.resampler_queue_.Add(rq);
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
                    oa.msPreUtterance = item.UstEvent.getPreUtterance();
                    oa.msOverlap = item.UstEvent.getVoiceOverlap();

                    RenderQueue rq2 = new RenderQueue();
                    string wavPath = "";
                    if (oa.fileName != null && oa.fileName.Length > 0) {
                        wavPath = Path.Combine(singer, oa.fileName);
                    } else {
                        wavPath = Path.Combine(singer, lyric + ".wav");
                    }
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
                    double sec = session_.sequence_.getSecFromClock(item.Clock) - (item.UstEvent.getPreUtterance() + item.UstEvent.getStartPoint()) / 1000.0;
                    int indx = 0;
                    int base_note = item.ID.Note;
                    double sec_vibstart = session_.sequence_.getSecFromClock(item.Clock + item.ID.VibratoDelay);
                    int totalcount = 0;

                    VibratoPointIteratorBySec vibitr = null;
                    if (item.ID.VibratoHandle != null) {
                        vibitr = new VibratoPointIteratorBySec(session_.sequence_,
                                                               item.ID.VibratoHandle.getRateBP(),
                                                               item.ID.VibratoHandle.getStartRate(),
                                                               item.ID.VibratoHandle.getDepthBP(),
                                                               item.ID.VibratoHandle.getStartDepth(),
                                                               item.Clock + item.ID.VibratoDelay,
                                                               item.ID.getLength() - item.ID.VibratoDelay,
                                                               (float)delta_sec);
                    }

                    while (sec <= sec_end) {
                        // clockでの音符の音の高さを調べる
                        // ピッチベンドを調べたい時刻
                        int clock = (int)session_.sequence_.getClockFromSec(sec);
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
                        if (pvalue != 0.0) {
                            allzero = false;
                        }

                        // 次
                        sec += delta_sec;
                    }

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
                    rq2.hashSource += session_.resampler_;
                    string filename =
                        Path.Combine(session_.temp_dir_, PortUtil.getMD5FromString(cache_.Count + rq2.hashSource) + ".wav");

                    rq2.appendArgRange(resampler_arg_prefix);
                    rq2.appendArg("\"" + filename + "\"");
                    rq2.appendArgRange(resampler_arg_suffix);
                    if (!allzero) {
                        rq2.appendArgRange(pitch.ToArray());
                    }

                    bool exist_in_cache = cache_.ContainsKey(rq2.hashSource);
                    if (!exist_in_cache) {
                        if (cache_.Count + 1 >= MAX_CACHE) {
                            double old = PortUtil.getCurrentTime();
                            string delfile = "";
                            string delkey = "";
                            foreach (var key in cache_.Keys) {
                                ValuePair<string, Double> value = cache_[key];
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
                            cache_.Remove(delkey);
                        }
                        //mCache.put( search_key, new ValuePair<String, Double>( filename, PortUtil.getCurrentTime() ) );
                        //->ここ，実際の合成が終わったタイミングで追加するようにする
                    } else {
                        filename = cache_[rq2.hashSource].getKey();
                    }

                    string str_t_temp = PortUtil.formatDecimal("0.00", BASE_TEMPO);
                    rq2.WavtoolArgPrefix.Clear();
                    rq2.WavtoolArgPrefix.Add("\"" + file + "\"");
                    rq2.WavtoolArgPrefix.Add("\"" + filename + "\"");
                    rq2.WavtoolArgPrefix.Add("" + item.UstEvent.getStartPoint());
                    rq2.WavtoolArgPrefix.Add("" + item.ID.getLength() + "@" + str_t_temp);
                    UstEnvelope env = item.UstEvent.getEnvelope();
                    if (env == null) {
                        env = new UstEnvelope();
                    }
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
                    session_.resampler_queue_.Add(rq2);
                }

                int num_queues = session_.resampler_queue_.Count;
                int processed_sample = 0; //WaveIncomingで受け渡した波形の合計サンプル数
                int channel = 0; // .whdに記録されたチャンネル数
                int byte_per_sample = 0;
                // 引き続き、wavtoolを呼ぶ作業に移行
                bool first = true;
                //int trim_remain = (int)( trimMillisec / 1000.0 * VSTiProxy.SAMPLE_RATE); //先頭から省かなければならないサンプル数の残り
                VsqBPList dyn_curve = session_.sequence_.Track[session_.track_index_].getCurve("dyn");
                for (int i = 0; i < num_queues; i++) {
                    RenderQueue rq = session_.resampler_queue_[i];
                    if (!rq.ResamplerFinished) {

                        using (var process = new Process()) {
                            process.StartInfo.FileName = "\"" + session_.resampler_ + "\"";
                            process.StartInfo.Arguments = rq.getResamplerArgString();
                            process.StartInfo.WorkingDirectory = session_.temp_dir_;
                            process.StartInfo.CreateNoWindow = true;
                            process.StartInfo.UseShellExecute = false;
                            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                            process.Start();
                            process.WaitForExit();

                            // 合成が済んだのでキャッシュに登録する
                            cache_[rq.hashSource] = new ValuePair<string, Double>(rq.FileName, PortUtil.getCurrentTime());
                        }
                    }

                    // wavtoolを起動
                    double sec_fin; // 今回のwavtool起動によってレンダリングが完了したサンプル長さ
                    RenderQueue p = session_.resampler_queue_[i];
                    OtoArgs oa_next;
                    if (i + 1 < num_queues) {
                        oa_next = session_.resampler_queue_[i + 1].Oto;
                    } else {
                        oa_next = new OtoArgs();
                    }

                    // この後のwavtool呼び出しで，どこまで波形が確定するか？
                    // 安全のために，wavtoolでくっつける音符の先頭位置までが確定するだろう，ということにする
                    sec_fin = p.secStart;
                    float mten = p.Oto.msPreUtterance + oa_next.msOverlap - oa_next.msPreUtterance;
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
                    processWavtool(arg_wavtool, file, session_.temp_dir_, wavtool_path_);

                    // できたwavを読み取ってWaveIncomingイベントを発生させる
                    int sample_end = (int)(sec_fin * session_.sample_rate_);
                    // whdを読みに行く
                    if (first) {
                        // このファイルのサンプリングレート．ヘッダで読み込むけど初期値はコレにしとく
                        session_.this_sample_rate_ = 44100;
                        using (Stream whd = new FileStream(file_whd, FileMode.Open, FileAccess.Read)) {
                            #region whdを読みに行く
                            whd.Seek(0, SeekOrigin.Begin);
                            // RIFF
                            byte[] buf = new byte[4];
                            int gcount = whd.Read(buf, 0, 4);
                            if (buf[0] != 'R' || buf[1] != 'I' || buf[2] != 'F' || buf[3] != 'F') {
                                continue;
                            }
                            // ファイルサイズ
                            whd.Read(buf, 0, 4);
                            // WAVE
                            whd.Read(buf, 0, 4);
                            if (buf[0] != 'W' || buf[1] != 'A' || buf[2] != 'V' || buf[3] != 'E') {
                                continue;
                            }
                            // fmt 
                            whd.Read(buf, 0, 4);
                            if (buf[0] != 'f' || buf[1] != 'm' || buf[2] != 't' || buf[3] != ' ') {
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
                            session_.this_sample_rate_ = PortUtil.make_int32_le(buf);
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
                                continue;
                            }
                            // size of data chunk
                            whd.Read(buf, 0, 4);
                            //int size = buf[3] << 24 | buf[2] << 16 | buf[1] << 8 | buf[0];
                            //int total_samples = size / (channel * byte_per_sample);
                            #endregion
                        }
                        first = false;
                    }

                    // datを読みに行く
                    int sampleFrames = sample_end - processed_sample;
                    if (channel > 0 && byte_per_sample > 0 && sampleFrames > 0) {
                        int length = (sampleFrames > session_.sample_rate_ ? session_.sample_rate_ : sampleFrames);
                        int remain = sampleFrames;
                        session_.left_ = new float[length];
                        session_.right_ = new float[length];
                        double k_inv64 = 1.0 / 64.0;
                        double k_inv128 = 1.0 / 128.0;
                        double k_inv32768 = 1.0 / 32768.0;
                        int buflen = 1024;
                        byte[] wavbuf = new byte[buflen];
                        int pos = 0;
                        using (Stream dat = new FileStream(file_dat, FileMode.Open, FileAccess.Read)) {
                            dat.Seek(processed_sample * channel * byte_per_sample, SeekOrigin.Begin);
                            double sec_start = processed_sample / (double)session_.sample_rate_;
                            double sec_per_sa = 1.0 / (double)session_.sample_rate_;
                            ByRef<int> index = new ByRef<int>(0);
                            #region チャンネル数／ビット深度ごとの読み取り操作
                            if (byte_per_sample == 1) {
                                if (channel == 1) {
                                    while (remain > 0) {
                                        int len = dat.Read(wavbuf, 0, buflen);
                                        if (len <= 0) {
                                            break;
                                        }
                                        int c = 0;
                                        while (len > 0 && remain > 0) {
                                            len -= 1;
                                            remain--;
                                            double gtime_dyn = sec_start + pos * sec_per_sa;
                                            int clock = (int)session_.sequence_.getClockFromSec(gtime_dyn);
                                            int dyn = dyn_curve.getValue(clock, index);
                                            double amp = dyn * k_inv64;
                                            double v = ((0xff & wavbuf[c]) - 128) * k_inv128 * amp;
                                            c++;
                                            session_.left_[pos] = (float)v;
                                            session_.right_[pos] = (float)v;
                                            pos++;
                                            if (pos >= length) {
                                                if (!Rendered(session_.left_, session_.right_, session_.left_.Length)) {
                                                    return;
                                                }
                                                pos = 0;
                                            }
                                        }
                                    }
                                } else {
                                    while (remain > 0) {
                                        int len = dat.Read(wavbuf, 0, buflen);
                                        if (len <= 0) {
                                            break;
                                        }
                                        int c = 0;
                                        while (len > 0 && remain > 0) {
                                            len -= 2;
                                            remain--;
                                            double gtime_dyn = sec_start + pos * sec_per_sa;
                                            int clock = (int)session_.sequence_.getClockFromSec(gtime_dyn);
                                            int dyn = dyn_curve.getValue(clock, index);
                                            double amp = dyn * k_inv64;
                                            double vl = ((0xff & wavbuf[c]) - 128) * k_inv128 * amp;
                                            double vr = ((0xff & wavbuf[c + 1]) - 128) * k_inv128 * amp;
                                            session_.left_[pos] = (float)vl;
                                            session_.right_[pos] = (float)vr;
                                            c += 2;
                                            pos++;
                                            if (pos >= length) {
                                                if (!Rendered(session_.left_, session_.right_, session_.left_.Length)) {
                                                    return;
                                                }
                                                pos = 0;
                                            }
                                        }
                                    }
                                }
                            } else if (byte_per_sample == 2) {
                                if (channel == 1) {
                                    while (remain > 0) {
                                        int len = dat.Read(wavbuf, 0, buflen);
                                        if (len <= 0) {
                                            break;
                                        }
                                        int c = 0;
                                        while (len > 0 && remain > 0) {
                                            len -= 2;
                                            remain--;
                                            double gtime_dyn = sec_start + pos * sec_per_sa;
                                            int clock = (int)session_.sequence_.getClockFromSec(gtime_dyn);
                                            int dyn = dyn_curve.getValue(clock, index);
                                            double amp = dyn * k_inv64;
                                            double v = ((short)(PortUtil.make_int16_le(wavbuf, c))) * k_inv32768 * amp;
                                            session_.left_[pos] = (float)v;
                                            session_.right_[pos] = (float)v;
                                            c += 2;
                                            pos++;
                                            if (pos >= length) {
                                                if (!Rendered(session_.left_, session_.right_, session_.left_.Length)) {
                                                    return;
                                                }
                                                pos = 0;
                                            }
                                        }
                                    }
                                } else {
                                    while (remain > 0) {
                                        int len = dat.Read(wavbuf, 0, buflen);
                                        if (len <= 0) {
                                            break;
                                        }
                                        int c = 0;
                                        while (len > 0 && remain > 0) {
                                            len -= 4;
                                            remain--;
                                            double gtime_dyn = sec_start + pos * sec_per_sa;
                                            int clock = (int)session_.sequence_.getClockFromSec(gtime_dyn);
                                            int dyn = dyn_curve.getValue(clock, index);
                                            double amp = dyn * k_inv64;
                                            double vl = ((short)(PortUtil.make_int16_le(wavbuf, c))) * k_inv32768 * amp;
                                            double vr = ((short)(PortUtil.make_int16_le(wavbuf, c + 2))) * k_inv32768 * amp;
                                            session_.left_[pos] = (float)vl;
                                            session_.right_[pos] = (float)vr;
                                            c += 4;
                                            pos++;
                                            if (pos >= length) {
                                                if (!Rendered(session_.left_, session_.right_, session_.left_.Length)) {
                                                    return;
                                                }
                                                pos = 0;
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }

                        if (pos > 0) {
                            if (!Rendered(session_.left_, session_.right_, pos)) {
                                return;
                            }
                        }
                        session_.left_ = null;
                        session_.right_ = null;
                        GC.Collect();
                        processed_sample += (sampleFrames - remain);
                    }
                }

                for (int i = 0; i < BUFLEN; i++) {
                    session_.buffer_left_[i] = 0;
                    session_.buffer_right_[i] = 0;
                }
                while (Rendered(session_.buffer_left_, session_.buffer_right_, BUFLEN)) ;
            } catch (Exception ex) {
                serr.println("UtauWaveGenerator.begin; ex=" + ex);
                Logger.write(typeof(UtauWaveGenerator) + ".begin; ex=" + ex + "\n");
            }
        }

        private void processWavtool(List<string> arg, string filebase, string temp_dir, string wavtool)
        {
            using (var process = new Process()) {
                process.StartInfo.FileName = "\"" + wavtool + "\"";
                string argument = "";
                int size = arg.Count;
                for (int i = 0; i < size; i++) {
                    argument += arg[i] + (i == size - 1 ? "" : " ");
                }
                process.StartInfo.Arguments = argument;
                process.StartInfo.WorkingDirectory = temp_dir;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
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
