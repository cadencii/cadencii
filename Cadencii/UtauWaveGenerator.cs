/*
 * UtauWaveGenerator.cs
 * Copyright (C) 2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

import java.awt.*;
import java.io.*;
import java.util.*;
import org.kbinani.*;
import org.kbinani.media.*;
import org.kbinani.vsq.*;
#else
using System;
using System.Diagnostics;
using System.Threading;
using org.kbinani.media;
using org.kbinani.vsq;
using org.kbinani;
using org.kbinani.java.awt;
using org.kbinani.java.io;
using org.kbinani.java.util;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

#if JAVA
    public class UtauWaveGenerator extends WaveUnit implements WaveGenerator {
#else
    public class UtauWaveGenerator : WaveUnit, WaveGenerator {
#endif
        public const String FILEBASE = "temp.wav";
        private const int MAX_CACHE = 512;
        private const int BUFLEN = 1024;
        private const int VERSION = 0;
        private static TreeMap<String, ValuePair<String, Double>> mCache = new TreeMap<String, ValuePair<String, Double>>();

        private Vector<RenderQueue> mResamplerQueue = new Vector<RenderQueue>();
        private double[] mLeft;
        private double[] mRight;

        VsqFileEx mVsq;
        Vector<SingerConfig> mSingerConfigSys;
        String mResampler;
        String mWavtool;
        String mTempDir;
        boolean mInvokeWithWine;
        private boolean mAbortRequired = false;
        private boolean mRunning = false;

        private long mTotalSamples;
        private WaveReceiver mReceiver = null;
        private long mTotalAppend = 0;
        private int mTrack;
        private double[] mBufferL = new double[BUFLEN];
        private double[] mBufferR = new double[BUFLEN];
        private int mTrimRemain = 0;

        public boolean isRunning() {
            return mRunning;
        }

        public long getTotalSamples() {
            return mTotalSamples;
        }

        public double getProgress() {
            if ( mTotalSamples <= 0 ) {
                return 0.0;
            }else{
                return mTotalAppend / (double)mTotalSamples;
            }
        }

        public void stop() {
            if ( mRunning ) {
                mAbortRequired = true;
                while ( mRunning ) {
                    Thread.Sleep( 100 );
                }
            }
        }

        public override void setConfig( string parameter ) {
            // do nothing
        }

        public override int getVersion() {
            return VERSION;
        }

        /// <summary>
        /// 初期化メソッド．
        /// </summary>
        /// <param name="parameter"></param>
        public void init( VsqFileEx vsq, int track, int start_clock, int end_clock ) {
            mTrack = track;
            mResampler = mConfig.PathResampler;
            mWavtool = mConfig.PathWavtool;
            mTempDir = PortUtil.combinePath( AppManager.getCadenciiTempDir(), AppManager.getID() );
            mInvokeWithWine = mConfig.InvokeUtauCoreWithWine;

            mVsq = (VsqFileEx)vsq.clone();
            mVsq.updateTotalClocks();

            if ( end_clock < vsq.TotalClocks ) {
                // 末尾の部分は不要なので削除
                mVsq.removePart( end_clock, mVsq.TotalClocks + 480 );
            }

            double trim_sec = 0.0;
            if ( start_clock > 0 ) {
                // 途中からの合成が指示された場合
                // 0clockからstart_clockまでを削除する
                // もしstart_clock位置に音符があれば，その音符の先頭から合成し，trim_secを適切に設定する

                // まず，start_clockに音符があるかどうかを調べる
                // 音符があれば，trim_endに適切な値を代入
                VsqTrack vsq_track = mVsq.Track.get( track );
                int c = vsq_track.getEventCount();
                int trim_end = start_clock;
                for ( int i = 0; i < c; i++ ) {
                    VsqEvent itemi = vsq_track.getEvent( i );
                    if ( itemi.ID.type != VsqIDType.Anote ) {
                        continue;
                    }
                    if ( itemi.Clock <= start_clock && start_clock < itemi.Clock + itemi.ID.getLength() ) {
                        trim_end = itemi.Clock;
                        break;
                    }
                }

                if ( trim_end == start_clock ) {
                    trim_sec = 0.0;
                } else {
                    trim_sec = mVsq.getSecFromClock( start_clock ) - mVsq.getSecFromClock( trim_end );
                }

                // 必要ならトリムを実行
                if ( 0 < trim_end ) {
                    mVsq.removePart( 0, trim_end );
                }
            }
            mVsq.updateTotalClocks();

            mTrimRemain = (int)(trim_sec * VSTiDllManager.SAMPLE_RATE);
        }

        public void setReceiver( WaveReceiver r ) {
            if ( mReceiver != null ) {
                mReceiver.end();
            }
            mReceiver = r;
        }

        public long getPosition() {
            return mTotalAppend;
        }

        public static void clearCache() {
            for ( Iterator<String> itr = mCache.keySet().iterator(); itr.hasNext(); ) {
                String key = itr.next();
                ValuePair<String, Double> value = mCache.get( key );
                String file = value.getKey();
                try {
                    PortUtil.deleteFile( file );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "UtauWaveGenerator#clearCache; ex=" + ex );
                    Logger.write( "UtauWaveGenerator::clearCache; ex=" + ex + "\n" );
                }
            }
            mCache.clear();
        }

        public void begin( long total_samples ) {
            mTotalSamples = total_samples;
#if MAKEBAT_SP
            StreamWriter bat = null;
            StreamWriter log = null;
#endif
            try {
                double sample_length = mVsq.getSecFromClock( mVsq.TotalClocks ) * VSTiDllManager.SAMPLE_RATE;
                mAbortRequired = false;
                mRunning = true;
                if ( !PortUtil.isDirectoryExists( mTempDir ) ) {
                    PortUtil.createDirectory( mTempDir );
                }

#if MAKEBAT_SP
                log = new StreamWriter( Path.Combine( m_temp_dir, "UtauWaveGenerator.log" ), false, Encoding.GetEncoding( "Shift_JIS" ) );
#endif
                // 原音設定を読み込み
                VsqTrack target = mVsq.Track.get( mTrack );

                String file = PortUtil.combinePath( mTempDir, FILEBASE );
                if ( PortUtil.isFileExists( file ) ) {
                    PortUtil.deleteFile( file );
                }
                String file_whd = PortUtil.combinePath( mTempDir, FILEBASE + ".whd" );
                if ( PortUtil.isFileExists( file_whd ) ) {
                    PortUtil.deleteFile( file_whd );
                }
                String file_dat = PortUtil.combinePath( mTempDir, FILEBASE + ".dat" );
                if ( PortUtil.isFileExists( file_dat ) ) {
                    PortUtil.deleteFile( file_dat );
                }
#if DEBUG
                AppManager.debugWriteLine( "UtauWaveGenerator#run; temp_dir=" + mTempDir );
                AppManager.debugWriteLine( "UtauWaveGenerator#run; file_whd=" + file_whd );
                AppManager.debugWriteLine( "UtauWaveGenerator#run; file_dat=" + file_dat );
#endif

                int count = -1;
                double sec_end = 0;
                double sec_end_old = 0;
                int program_change = 0;
                mResamplerQueue.clear();
#if DEBUG
                double error_sum = 0.0;
#endif

                // 前後の音符の先行発音やオーバーラップやらを取得したいので、一度リストに格納する
                Vector<VsqEvent> events = new Vector<VsqEvent>();
                for ( Iterator<VsqEvent> itr = target.getNoteEventIterator(); itr.hasNext(); ) {
                    events.add( itr.next() );
                }

#if MAKEBAT_SP
                log.WriteLine( "making resampler queue..." );
#endif
                int events_count = events.size();
                for ( int k = 0; k < events_count; k++ ) {
                    VsqEvent item = events.get( k );
#if MAKEBAT_SP
                    log.Write( "    #" + k + "; clock=" + item.Clock );
#endif
                    VsqEvent singer_event = target.getSingerEventAt( item.Clock );
                    if ( singer_event == null ) {
                        program_change = 0;
                    } else {
                        program_change = singer_event.ID.IconHandle.Program;
                    }
                    String singer = "";
                    if ( 0 <= program_change && program_change < mConfig.UtauSingers.size() ) {
                        singer = mConfig.UtauSingers.get( program_change ).VOICEIDSTR;
                    }
#if MAKEBAT_SP
                    log.Write( "; pc=" + program_change );
#endif
                    if ( mAbortRequired ) {
                        return;
                    }
                    count++;
                    double sec_start = mVsq.getSecFromClock( item.Clock );
                    double sec_start_act = sec_start - item.UstEvent.PreUtterance / 1000.0;
                    sec_end_old = sec_end;
                    sec_end = mVsq.getSecFromClock( item.Clock + item.ID.getLength() );
                    double sec_end_act = sec_end;
                    VsqEvent item_next = null;
                    if ( k + 1 < events_count ) {
                        item_next = events.get( k + 1 );
                    }
                    if ( item_next != null ) {
                        double sec_start_act_next = 
                            mVsq.getSecFromClock( item_next.Clock ) - item_next.UstEvent.PreUtterance / 1000.0
                            + item_next.UstEvent.VoiceOverlap / 1000.0;
                        if ( sec_start_act_next < sec_end_act ) {
                            sec_end_act = sec_start_act_next;
                        }
                    }
                    float t_temp = (float)(item.ID.getLength() / (sec_end - sec_start) / 8.0);
                    if ( (count == 0 && sec_start > 0.0) || (sec_start > sec_end_old) ) {
                        // 最初の音符，
                        double sec_start2 = sec_end_old;
                        double sec_end2 = sec_start;
                        // t_temp2が120から大きく外れないように
                        int draft_length = (int)((sec_end2 - sec_start2) * 8.0 * 120.0);
                        float t_temp2 = (float)(draft_length / (sec_end2 - sec_start2) / 8.0);
                        String str_t_temp2 = PortUtil.formatDecimal( "0.00", t_temp2 );
                        double act_t_temp2 = PortUtil.parseDouble( str_t_temp2 );
#if DEBUG
                        error_sum += (draft_length / (act_t_temp2 * 8.0)) - (sec_end2 - sec_start2);
#endif
                        RenderQueue rq = new RenderQueue();
                        rq.WavtoolArgPrefix = 
                            "\"" + file + "\" \"" + PortUtil.combinePath( singer, "R.wav" ) + "\" 0 " + draft_length + "@"
                            + str_t_temp2;
                        rq.WavtoolArgSuffix = " 0 0";
                        rq.Oto = new OtoArgs();
                        rq.FileName = "";
                        rq.secEnd = sec_end2;
                        rq.ResamplerFinished = true;
                        mResamplerQueue.add( rq );
                        count++;
                    }
                    String lyric = item.ID.LyricHandle.L0.Phrase;
                    String note = NoteStringFromNoteNumber( item.ID.Note );
                    int millisec = (int)((sec_end_act - sec_start_act) * 1000) + 50;

                    OtoArgs oa = new OtoArgs();
                    if ( AppManager.mUtauVoiceDB.containsKey( singer ) ) {
                        UtauVoiceDB db = AppManager.mUtauVoiceDB.get( singer );
                        oa = db.attachFileNameFromLyric( lyric );
                    }
#if MAKEBAT_SP
                    log.Write( "; lyric=" + lyric + "; fileName=" + oa.fileName );
#endif
                    oa.msPreUtterance = item.UstEvent.PreUtterance;
                    oa.msOverlap = item.UstEvent.VoiceOverlap;
#if DEBUG
                    PortUtil.println( "UtauWaveGenerator#run; oa.fileName=" + oa.fileName );
                    PortUtil.println( "UtauWaveGenerator#run; lyric=" + lyric );
#endif
                    RenderQueue rq2 = new RenderQueue();
                    String wavPath = "";
                    if ( PortUtil.getStringLength( oa.fileName ) > 0 ) {
                        wavPath = PortUtil.combinePath( singer, oa.fileName );
                    } else {
                        wavPath = PortUtil.combinePath( singer, lyric + ".wav" );
                    }
#if DEBUG
                    PortUtil.println( "UtauWaveGenerator#run; wavPath=" + wavPath );
#endif
                    String[] resampler_arg_prefix = new String[] { "\"" + wavPath + "\"" };
                    String[] resampler_arg_suffix = new String[]{
                        "\"" + note + "\"",
                        "100",
                        item.UstEvent.Flags,
                        "L", 
                        oa.msOffset + "",
                        millisec + "",
                        oa.msConsonant + "",
                        oa.msBlank + "",
                        "100",
                        item.UstEvent.Moduration + "" };

                    // ピッチを取得
                    Vector<String> pitch = new Vector<String>();
                    boolean allzero = true;
                    int delta_clock = 5;  //ピッチを取得するクロック間隔
                    int tempo = 120;
                    double delta_sec = delta_clock / (8.0 * tempo); //ピッチを取得する時間間隔
                    if ( item.ID.VibratoHandle == null ) {
                        int pit_count = (int)((sec_end_act - sec_start_act) / delta_sec) + 1;
                        for ( int i = 0; i < pit_count; i++ ) {
                            double gtime = sec_start_act + delta_sec * i;
                            int clock = (int)mVsq.getClockFromSec( gtime );
                            float pvalue = (float)target.getPitchAt( clock );
                            if ( pvalue != 0 ) {
                                allzero = false;
                            }
                            pitch.add( PortUtil.formatDecimal( "0.00", pvalue ) );
                            if ( i == 0 ) {
                                pitch.add( "Q" + tempo );
                            }
                        }
                    } else {
                        // ビブラートが始まるまでのピッチを取得
                        double sec_vibstart = mVsq.getSecFromClock( item.Clock + item.ID.VibratoDelay );
                        int pit_count = (int)((sec_vibstart - sec_start_act) / delta_sec);
                        int totalcount = 0;
                        for ( int i = 0; i < pit_count; i++ ) {
                            double gtime = sec_start_act + delta_sec * i;
                            int clock = (int)mVsq.getClockFromSec( gtime );
                            float pvalue = (float)target.getPitchAt( clock );
                            pitch.add( PortUtil.formatDecimal( "0.00", pvalue ) );
                            if ( totalcount == 0 ) {
                                pitch.add( "Q" + tempo );
                            }
                            totalcount++;
                        }
                        Iterator<PointD> itr = new VibratoPointIteratorBySec( 
                            mVsq,
                            item.ID.VibratoHandle.getRateBP(),
                            item.ID.VibratoHandle.getStartRate(),
                            item.ID.VibratoHandle.getDepthBP(),
                            item.ID.VibratoHandle.getStartDepth(),
                            item.Clock + item.ID.VibratoDelay,
                            item.ID.getLength() - item.ID.VibratoDelay,
                            (float)delta_sec );
                        for ( ; itr.hasNext(); ) {
                            PointD ret = itr.next();
                            float gtime = (float)ret.getX();
                            int clock = (int)mVsq.getClockFromSec( gtime );
                            float pvalue = (float)target.getPitchAt( clock );
                            pitch.add( PortUtil.formatDecimal( "0.00", pvalue + ret.getY() * 100.0f ) );
                            if ( totalcount == 0 ) {
                                pitch.add( "Q" + tempo );
                            }
                            totalcount++;
                        }
                        allzero = totalcount == 0;
                    }

                    //4_あ_C#4_550.wav
                    String md5_src = "";
                    foreach ( String s in resampler_arg_prefix ) {
                        md5_src += s + " ";
                    }
                    foreach ( String s in resampler_arg_suffix ) {
                        md5_src += s + " ";
                    }
                    foreach ( String s in pitch ) {
                        md5_src += s + " ";
                    }
                    String filename = 
                        PortUtil.combinePath( mTempDir, PortUtil.getMD5FromString( mCache.size() + md5_src ) + ".wav" );

                    rq2.appendArgRange( resampler_arg_prefix );
                    rq2.appendArg( "\"" + filename + "\"" );
                    rq2.appendArgRange( resampler_arg_suffix );
                    if ( !allzero ) {
                        rq2.appendArgRange( pitch.toArray( new String[0] ) );
                    }

                    String search_key = md5_src;
                    boolean exist_in_cache = mCache.containsKey( search_key );
                    if ( !exist_in_cache ) {
                        if ( mCache.size() + 1 >= MAX_CACHE ) {
                            double old = PortUtil.getCurrentTime();
                            String delfile = "";
                            String delkey = "";
                            for ( Iterator<String> itr = mCache.keySet().iterator(); itr.hasNext(); ) {
                                String key = itr.next();
                                ValuePair<String, Double> value = mCache.get( key );
                                if ( old < value.getValue() ) {
                                    old = value.getValue();
                                    delfile = value.getKey();
                                    delkey = key;
                                }
                            }
                            try {
                                PortUtil.deleteFile( delfile );
                            } catch ( Exception ex ) {
                                PortUtil.stderr.println( "UtauWaveGenerator#run; ex=" + ex );
                                Logger.write( "UtauWaveGenerator::begin(long): ex=" + ex + "\n" );
                            }
                            mCache.remove( delkey );
                        }
                        mCache.put( search_key, new ValuePair<String, Double>( filename, PortUtil.getCurrentTime() ) );
                    } else {
                        filename = mCache.get( search_key ).getKey();
                    }

                    String str_t_temp = PortUtil.formatDecimal( "0.00", t_temp );
#if DEBUG
                    double act_t_temp = PortUtil.parseDouble( str_t_temp );
                    error_sum += (item.ID.getLength() / (8.0 * act_t_temp)) - (sec_end - sec_start);
                    Logger.write( "UtauWaveGenerator#begin; error_sum=" + error_sum + "\n" );
#endif
                    rq2.WavtoolArgPrefix = 
                        "\"" + file + "\" \"" + filename + "\" 0 "
                        + item.ID.getLength() + "@" + str_t_temp;
                    UstEnvelope env = item.UstEvent.Envelope;
                    if ( env == null ) {
                        env = new UstEnvelope();
                    }
                    rq2.WavtoolArgSuffix = " " + env.p1 + " " + env.p2 + " " + env.p3 + " " + env.v1 + " " + env.v2 + " " + env.v3 + " " + env.v4;
                    rq2.WavtoolArgSuffix += " " + oa.msOverlap + " " + env.p4 + " " + env.p5 + " " + env.v5;
                    rq2.Oto = oa;
                    rq2.FileName = filename;
                    rq2.secEnd = sec_end;
                    rq2.ResamplerFinished = exist_in_cache;
                    mResamplerQueue.add( rq2 );
#if MAKEBAT_SP
                    log.WriteLine();
#endif
                }
#if MAKEBAT_SP
                log.WriteLine( "...done" );
#endif

                int num_queues = mResamplerQueue.size();
                int processed_sample = 0; //WaveIncomingで受け渡した波形の合計サンプル数
                int channel = 0; // .whdに記録されたチャンネル数
                int byte_per_sample = 0;
                // 引き続き、wavtoolを呼ぶ作業に移行
                boolean first = true;
                //int trim_remain = (int)( trimMillisec / 1000.0 * VSTiProxy.SAMPLE_RATE); //先頭から省かなければならないサンプル数の残り
                VsqBPList dyn_curve = mVsq.Track.get( mTrack ).getCurve( "dyn" );
#if MAKEBAT_SP
                bat = new StreamWriter( Path.Combine( m_temp_dir, "utau.bat" ), false, Encoding.GetEncoding( "Shift_JIS" ) );
#endif
                for ( int i = 0; i < num_queues; i++ ) {
                    RenderQueue rq = mResamplerQueue.get( i );
                    if ( !rq.ResamplerFinished ) {
#if MAKEBAT_SP
                        bat.WriteLine( "\"" + m_resampler + "\" " + rq.getResamplerArgString() );
#endif

#if JAVA
                        Vector<String> list = new Vector<String>();
                        if( m_invoke_with_wine ){
                            list.add( "wine" );
                        }
                        list.add( "\"" + m_resampler.replace( "\\", "\\" + "\\" ) + "\"" );
                        for( String s : rq.getResamplerArg() ){
                            s = s.replace( "\\", "\\" + "\\" );
                            list.add( s );
                        }
#if DEBUG
                        PortUtil.println( "UtauRenderingRunner#run; args=" );
                        for( String s : list ){
                            PortUtil.println( "UtauRenderingRunner#run; " + s );
                        }
#endif
                        ProcessBuilder pb = new ProcessBuilder( list );
                        Process process = pb.start();
                        boolean д = true;
                        for( ;д; ){
                            try{
                                int ecode = process.exitValue();
                            }catch( Exception ex ){
                                Logger.write( UtauWaveGenerator.class + ".begin; ex=" + ex + "\n" );
                                continue;
                            }
                            break;
                        }
                        //process.waitFor();
#else
                        Process process = null;
                        try {
                            process = new Process();
                            process.StartInfo.FileName = (mInvokeWithWine ? "wine \"" : "\"") + mResampler + "\"";
                            process.StartInfo.Arguments = rq.getResamplerArgString();
                            process.StartInfo.WorkingDirectory = mTempDir;
                            process.StartInfo.CreateNoWindow = true;
                            process.StartInfo.UseShellExecute = false;
                            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                            process.Start();
                            process.WaitForExit();
                        } catch ( Exception ex ) {
                            Logger.write( typeof( UtauWaveGenerator ) + ".begin; ex=" + ex + "\n" );
                        } finally {
                            if ( process != null ) {
                                process.Dispose();
                            }
                        }
#endif
                    }
                    if ( mAbortRequired ) {
                        break;
                    }

                    // wavtoolを起動
                    double sec_fin; // 今回のwavtool起動によってレンダリングが完了したサンプル長さ
                    RenderQueue p = mResamplerQueue.get( i ); //Phon p = s_resampler_queue[i];
                    OtoArgs oa_next;
                    if ( i + 1 < num_queues ) {
                        oa_next = mResamplerQueue.get( i + 1 ).Oto;
                    } else {
                        oa_next = new OtoArgs();
                    }
                    sec_fin = p.secEnd - oa_next.msOverlap / 1000.0;
#if DEBUG
                    AppManager.debugWriteLine( "UtauWaveGenerator#run; sec_fin=" + sec_fin );
#endif
                    float mten = p.Oto.msPreUtterance + oa_next.msOverlap - oa_next.msPreUtterance;
                    String arg_wavtool = p.WavtoolArgPrefix + (mten >= 0 ? ("+" + mten) : ("-" + (-mten))) + p.WavtoolArgSuffix;
#if MAKEBAT_SP
                    bat.WriteLine( "\"" + m_wavtool + "\" " + arg_wavtool );
#endif
                    processWavtool( arg_wavtool, file, mTempDir, mWavtool, mInvokeWithWine );

                    // できたwavを読み取ってWaveIncomingイベントを発生させる
                    int sample_end = (int)(sec_fin * VSTiDllManager.SAMPLE_RATE);
#if DEBUG
                    AppManager.debugWriteLine( "UtauWaveGenerator#run; sample_end=" + sample_end );
#endif
                    // whdを読みに行く
                    if ( first ) {
                        RandomAccessFile whd = null;
                        try {
                            whd = new RandomAccessFile( file_whd, "r" );
                            #region whdを読みに行く
                            whd.seek( 0 );
                            // RIFF
                            byte[] buf = new byte[4];
                            int gcount = whd.read( buf, 0, 4 );
                            if ( buf[0] != 'R' || buf[1] != 'I' || buf[2] != 'F' || buf[3] != 'F' ) {
#if DEBUG
                                AppManager.debugWriteLine( "UtauWaveGenerator#run; whd header error" );
#endif
                                continue;
                            }
                            // ファイルサイズ
                            whd.read( buf, 0, 4 );
                            // WAVE
                            whd.read( buf, 0, 4 );
                            if ( buf[0] != 'W' || buf[1] != 'A' || buf[2] != 'V' || buf[3] != 'E' ) {
#if DEBUG
                                AppManager.debugWriteLine( "UtauWaveGenerator#run; whd header error" );
#endif
                                continue;
                            }
                            // fmt 
                            whd.read( buf, 0, 4 );
                            if ( buf[0] != 'f' || buf[1] != 'm' || buf[2] != 't' || buf[3] != ' ' ) {
#if DEBUG
                                AppManager.debugWriteLine( "UtauWaveGenerator#run; whd header error" );
#endif
                                continue;
                            }
                            // fmt チャンクのサイズ
                            whd.read( buf, 0, 4 );
                            long loc_end_of_fmt = whd.getFilePointer(); //fmtチャンクの終了位置．ここは一定値でない可能性があるので読込み
                            loc_end_of_fmt += buf[0] | buf[1] << 8 | buf[2] << 16 | buf[3] << 24;
                            // format ID
                            whd.read( buf, 0, 2 );
                            int id = buf[0] | buf[1] << 8;
                            if ( id != 0x0001 ) { //0x0001はリニアPCM
                                continue;
                            }
                            // チャンネル数
                            whd.read( buf, 0, 2 );
                            channel = buf[1] << 8 | buf[0];
                            // サンプリングレート
                            whd.read( buf, 0, 4 );
                            int this_sample_rate = buf[0] | buf[1] << 8 | buf[2] << 16 | buf[3] << 24;
                            // データ速度
                            whd.read( buf, 0, 4 );
                            // ブロックサイズ
                            whd.read( buf, 0, 2 );
                            // 1チャンネル、1サンプルあたりのビット数
                            whd.read( buf, 0, 2 );
                            int bit_per_sample = buf[1] << 8 | buf[0];
                            byte_per_sample = bit_per_sample / 8;
                            whd.seek( loc_end_of_fmt );
                            // data
                            whd.read( buf, 0, 4 );
                            if ( buf[0] != 'd' || buf[1] != 'a' || buf[2] != 't' || buf[3] != 'a' ) {
#if DEBUG
                                AppManager.debugWriteLine( "UtauWaveGenerator#run; whd header error" );
#endif
                                continue;
                            }
                            // size of data chunk
                            whd.read( buf, 0, 4 );
                            //int size = buf[3] << 24 | buf[2] << 16 | buf[1] << 8 | buf[0];
                            //int total_samples = size / (channel * byte_per_sample);
                            #endregion
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "UtauWaveGenerator#run; ex=" + ex );
                            Logger.write( "UtauWaveGenerator::begin(long); ex=" + ex + "\n" );
                        } finally {
                            if ( whd != null ) {
                                try {
                                    whd.close();
                                } catch ( Exception ex2 ) {
                                    PortUtil.stderr.println( "UtauWaveGenerator#run; ex2=" + ex2 );
                                    Logger.write( "UtauWaveGenerator::begin(long); ex=" + ex2 + "\n" );
                                }
                            }
                        }
                        first = false;
                    }

                    // datを読みに行く
                    int sampleFrames = sample_end - processed_sample;
#if DEBUG
                    AppManager.debugWriteLine( "UtauWaveGenerator#run; sampleFrames=" + sampleFrames + "; channel=" + channel + "; byte_per_sample=" + byte_per_sample );
#endif
                    if ( channel > 0 && byte_per_sample > 0 && sampleFrames > 0 ) {
                        int length = (sampleFrames > VSTiDllManager.SAMPLE_RATE ? VSTiDllManager.SAMPLE_RATE : sampleFrames);
                        int remain = sampleFrames;
                        mLeft = new double[length];
                        mRight = new double[length];
                        float k_inv64 = 1.0f / 64.0f;
                        float k_inv32768 = 1.0f / 32768.0f;
                        int buflen = 1024;
                        byte[] wavbuf = new byte[buflen];
                        int pos = 0;
                        RandomAccessFile dat = null;
                        try {
                            dat = new RandomAccessFile( file_dat, "r" );
                            dat.seek( processed_sample * channel * byte_per_sample );
                            double sec_start = processed_sample / (double)VSTiDllManager.SAMPLE_RATE;
                            double sec_per_sa = 1.0 / (double)VSTiDllManager.SAMPLE_RATE;
                            ByRef<Integer> index = new ByRef<Integer>( 0 );
                            #region チャンネル数／ビット深度ごとの読み取り操作
                            if ( byte_per_sample == 1 ) {
                                if ( channel == 1 ) {
                                    while ( remain > 0 ) {
                                        if ( mAbortRequired ) {
                                            break;
                                        }
                                        int len = dat.read( wavbuf, 0, buflen );
                                        if ( len <= 0 ) {
                                            break;
                                        }
                                        int c = 0;
                                        while ( len > 0 && remain > 0 ) {
                                            if ( mAbortRequired ) {
                                                break;
                                            }
                                            len -= 1;
                                            remain--;
                                            double gtime_dyn = sec_start + pos * sec_per_sa;
                                            int clock = (int)mVsq.getClockFromSec( gtime_dyn );
                                            int dyn = dyn_curve.getValue( clock, index );
                                            float amp = (float)dyn * k_inv64;
                                            float v = (wavbuf[c] - 64.0f) * k_inv64 * amp;
                                            c++;
                                            mLeft[pos] = v;
                                            mRight[pos] = v;
                                            pos++;
                                            if ( pos >= length ) {
                                                waveIncoming( mLeft, mRight, mLeft.Length );
                                                pos = 0;
                                            }
                                        }
                                    }
                                } else {
                                    while ( remain > 0 ) {
                                        if ( mAbortRequired ) {
                                            break;
                                        }
                                        int len = dat.read( wavbuf, 0, buflen );
                                        if ( len <= 0 ) {
                                            break;
                                        }
                                        int c = 0;
                                        while ( len > 0 && remain > 0 ) {
                                            if ( mAbortRequired ) {
                                                break;
                                            }
                                            len -= 2;
                                            remain--;
                                            double gtime_dyn = sec_start + pos * sec_per_sa;
                                            int clock = (int)mVsq.getClockFromSec( gtime_dyn );
                                            int dyn = dyn_curve.getValue( clock, index );
                                            float amp = (float)dyn * k_inv64;
                                            float vl = (wavbuf[c] - 64.0f) * k_inv64 * amp;
                                            float vr = (wavbuf[c + 1] - 64.0f) * k_inv64 * amp;
                                            mLeft[pos] = vl;
                                            mRight[pos] = vr;
                                            c += 2;
                                            pos++;
                                            if ( pos >= length ) {
                                                waveIncoming( mLeft, mRight, mLeft.Length );
                                                pos = 0;
                                            }
                                        }
                                    }
                                }
                            } else if ( byte_per_sample == 2 ) {
                                if ( channel == 1 ) {
                                    while ( remain > 0 ) {
                                        if ( mAbortRequired ) {
                                            break;
                                        }
                                        int len = dat.read( wavbuf, 0, buflen );
                                        if ( len <= 0 ) {
                                            break;
                                        }
                                        int c = 0;
                                        while ( len > 0 && remain > 0 ) {
                                            if ( mAbortRequired ) {
                                                break;
                                            }
                                            len -= 2;
                                            remain--;
                                            double gtime_dyn = sec_start + pos * sec_per_sa;
                                            int clock = (int)mVsq.getClockFromSec( gtime_dyn );
                                            int dyn = dyn_curve.getValue( clock, index );
                                            float amp = (float)dyn * k_inv64;
                                            float v = ((short)(wavbuf[c] | wavbuf[c + 1] << 8)) * k_inv32768 * amp;
                                            mLeft[pos] = v;
                                            mRight[pos] = v;
                                            c += 2;
                                            pos++;
                                            if ( pos >= length ) {
                                                waveIncoming( mLeft, mRight, mLeft.Length );
                                                pos = 0;
                                            }
                                        }
                                    }
                                } else {
                                    while ( remain > 0 ) {
                                        if ( mAbortRequired ) {
                                            break;
                                        }
                                        int len = dat.read( wavbuf, 0, buflen );
                                        if ( len <= 0 ) {
                                            break;
                                        }
                                        int c = 0;
                                        while ( len > 0 && remain > 0 ) {
                                            if ( mAbortRequired ) {
                                                break;
                                            }
                                            len -= 4;
                                            remain--;
                                            double gtime_dyn = sec_start + pos * sec_per_sa;
                                            int clock = (int)mVsq.getClockFromSec( gtime_dyn );
                                            int dyn = dyn_curve.getValue( clock, index );
                                            float amp = (float)dyn * k_inv64;
                                            float vl = ((short)(wavbuf[c] | wavbuf[c + 1] << 8)) * k_inv32768 * amp;
                                            float vr = ((short)(wavbuf[c + 2] | wavbuf[c + 3] << 8)) * k_inv32768 * amp;
                                            mLeft[pos] = vl;
                                            mRight[pos] = vr;
                                            c += 4;
                                            pos++;
                                            if ( pos >= length ) {
                                                waveIncoming( mLeft, mRight, mLeft.Length );
                                                pos = 0;
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "UtauWaveGenerator#run; ex=" + ex );
                            Logger.write( "UtauWaveGenerator::begin(long); ex=" + ex + "\n" );
                        } finally {
                            if ( dat != null ) {
                                try {
                                    dat.close();
                                } catch ( Exception ex2 ) {
                                    PortUtil.stderr.println( "UtauWaveGenerator#run; ex2=" + ex2 );
                                    Logger.write( typeof( UtauWaveGenerator ) + "::begin(long); ex=" + ex2 + "\n" );
                                }
                                dat = null;
                            }
                        }

                        if ( mAbortRequired ) {
                            mAbortRequired = false;
                            return;
                        }
#if DEBUG
                        AppManager.debugWriteLine( "UtauWaveGenerator#run; calling WaveIncoming..." );
#endif
                        if ( pos > 0 ) {
                            waveIncoming( mLeft, mRight, pos );
                        }
                        mLeft = null;
                        mRight = null;
#if JAVA
                        System.gc();
#else
                        GC.Collect();
#endif
#if DEBUG
                        AppManager.debugWriteLine( "UtauWaveGenerator#run; ...done(calling WaveIncoming)" );
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
                PortUtil.println( "UtauWaveGenerator#run; tremain=" + tremain );
#endif
                for ( int i = 0; i < BUFLEN; i++ ) {
                    mBufferL[i] = 0.0;
                    mBufferR[i] = 0.0;
                }
                while ( tremain > 0 && !mAbortRequired ) {
                    int amount = (tremain > BUFLEN) ? BUFLEN : tremain;
                    waveIncoming( mBufferL, mBufferR, amount );
                    tremain -= amount;
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "UtauWaveGenerator.begin; ex=" + ex );
                Logger.write( typeof( UtauWaveGenerator ) + ".begin; ex=" + ex + "\n" );
#if JAVA
                ex.printStackTrace();
#endif
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
                mRunning = false;
                mAbortRequired = false;
                mReceiver.end();
            }
        }

        private static void processWavtool( String arg, String filebase, String temp_dir, String wavtool, boolean invoke_with_wine ) {
#if JAVA
            String[] args = new String[]{ (invoke_with_wine ? "wine \"" : "\"") + wavtool + "\"", arg };
            ProcessBuilder pb = new ProcessBuilder( args );
            try{
                Process process = pb.start();
                process.waitFor();
            }catch( Exception ex ){
                Logger.write( UtauWaveGenerator.class + ".processWavtool; ex=" + ex + "\n" );
            }
#else
            Process process = null;
            try {
                process = new Process();
                process.StartInfo.FileName = (invoke_with_wine ? "wine \"" : "\"") + wavtool + "\"";
                process.StartInfo.Arguments = arg;
                process.StartInfo.WorkingDirectory = temp_dir;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
#if DEBUG
                PortUtil.println( "UtauWaveGenerator#processWavTool; invoke_with_wine=" + invoke_with_wine );
                PortUtil.println( "UtauWaveGenerator#processWavTool; .FileName=" + process.StartInfo.FileName + "; .Arguments=" + process.StartInfo.Arguments );
#endif
                process.Start();
                process.WaitForExit();
            } catch ( Exception ex ) {
                Logger.write( typeof( UtauWaveGenerator ) + ".processWavtool; ex=" + ex + "\n" );
            } finally {
                if ( process != null ) {
                    process.Dispose();
                }
            }
#endif
        }

        private void waveIncoming( double[] l, double[] r, int length ) {
            int offset = 0;
            if ( mTrimRemain > 0 ) {
                if ( length <= mTrimRemain ) {
                    mTrimRemain -= length;
                    return;
                } else {
                    mTrimRemain = 0;
                    offset += length -= mTrimRemain;
                }
            }
            int remain = length - offset;
            while ( remain > 0 ) {
                int amount = (remain > BUFLEN) ? BUFLEN : remain;
                for ( int i = 0; i < amount; i++ ) {
                    mBufferL[i] = l[i + offset];
                    mBufferR[i] = r[i + offset];
                }
                mReceiver.push( mBufferL, mBufferR, amount );
                remain -= amount;
                offset += amount;
                mTotalAppend += amount;
            }
        }

        private static String NoteStringFromNoteNumber( int note_number ) {
            int odd = note_number % 12;
            String head = (new String[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" })[odd];
            return head + (note_number / 12 - 1);
        }
    }

#if !JAVA
}
#endif
