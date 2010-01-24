/*
 * StraightRenderingRunner.cs
 * Copyright (C) 2009-2010 kbinani
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

import java.io.*;
import java.util.*;
import org.kbinani.*;
import org.kbinani.media.*;
import org.kbinani.vsq.*;
#else
using System;
using System.Diagnostics;
using org.kbinani.media;
using org.kbinani.vsq;
using org.kbinani;
using org.kbinani.java.io;
using org.kbinani.java.util;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

#if JAVA
    public class StraightRenderingRunner extends RenderingRunner{
#else
    public class StraightRenderingRunner : RenderingRunner {
#endif
        public static String STRAIGHT_SYNTH = "vConnect.exe";
        private const int MAX_CACHE = 512;
        private static TreeMap<String, Double> s_cache = new TreeMap<String, Double>();
        const int TEMPO = 120;

        Vector<StraightRenderingQueue> m_queue;
        Vector<SingerConfig> m_singer_config_sys;
        double m_progress_percent = 0.0;
        boolean m_mode_infinite;

        TreeMap<String, UtauVoiceDB> m_voicedb_configs = new TreeMap<String, UtauVoiceDB>();
        long m_vsq_length_samples;
        double m_started_date;
        /// <summary>
        /// 現在の処理速度．progress%/sec
        /// </summary>
        double m_running_rate;

        public StraightRenderingRunner( VsqFileEx vsq,
                                        int track,
                                        Vector<SingerConfig> singer_config_sys,
                                        int sample_rate,
                                        int trim_msec,
                                        long total_samples,
                                        boolean mode_infinite,
                                        WaveWriter wave_writer,
                                        double wave_read_offset_seconds,
                                        Vector<WaveReader> wave_reader,
                                        boolean direct_play,
                                        boolean reflect_amp_to_wave )
#if JAVA
        {
#else
            :
#endif
            base( track, reflect_amp_to_wave, wave_writer, wave_read_offset_seconds, wave_reader, direct_play, trim_msec, total_samples, sample_rate )
#if JAVA
            ;
#else
        {
#endif
            m_locker = new Object();
            m_queue = new Vector<StraightRenderingQueue>();
            m_singer_config_sys = singer_config_sys;
            m_mode_infinite = mode_infinite;
            int midi_tempo = 60000000 / TEMPO;
            VsqFileEx work = (VsqFileEx)vsq.clone();
            TempoVector tempo = new TempoVector();
            tempo.clear();
            tempo.add( new TempoTableEntry( 0, midi_tempo, 0.0 ) );
            tempo.updateTempoInfo();
            work.adjustClockToMatchWith( tempo );
            // テンポテーブルをクリア
            work.TempoTable.clear();
            work.TempoTable.add( new TempoTableEntry( 0, midi_tempo, 0.0 ) );
            work.updateTempoInfo();
            VsqTrack vsq_track = work.Track.get( track );
            Vector<VsqEvent> events = new Vector<VsqEvent>(); // 順次取得はめんどくさいので，一度eventsに格納してから処理しよう
            int count = vsq_track.getEventCount();
            VsqEvent current_singer_event = null;

            for ( int i = 0; i < count; i++ ) {
                VsqEvent item = vsq_track.getEvent( i );
                if ( item.ID.type == VsqIDType.Singer ) {
                    if ( events.size() > 0 && current_singer_event != null ) {
                        // eventsに格納されたノートイベントについて，StraightRenderingQueueを順次作成し，登録
                        appendQueue( work, track, events, current_singer_event, sample_rate );
                        events.clear();
                    }
                    current_singer_event = item;
                } else if ( item.ID.type == VsqIDType.Anote ) {
                    events.add( item );
                }
            }
            if ( events.size() > 0 && current_singer_event != null ) {
                appendQueue( work, track, events, current_singer_event, sample_rate );
            }
            if ( m_queue.size() > 0 ) {
                StraightRenderingQueue q = m_queue.get( m_queue.size() - 1 );
                m_vsq_length_samples = q.startFrame + q.abstractFrameLength;
            }
        }

        private void appendQueue( VsqFileEx vsq, int track, Vector<VsqEvent> events, VsqEvent singer_event, int sample_rate ) {
            int count = events.size();
            if ( count <= 0 ) {
                return;
            }
            VsqEvent current = events.get( 0 );
            VsqEvent next = null;

            String singer = singer_event.ID.IconHandle.IDS;
            int num_singers = m_singer_config_sys.size();
            String singer_path = "";
            for ( int i = 0; i < num_singers; i++ ) {
                SingerConfig sc = m_singer_config_sys.get( i );
                if ( sc.VOICENAME.Equals( singer ) ) {
                    singer_path = sc.VOICEIDSTR;
                    break;
                }
            }
            // 歌手のパスが取得できないので離脱
            if ( singer_path.Equals( "" ) ) {
                return;
            }
            String oto_ini = PortUtil.combinePath( PortUtil.combinePath( singer_path, "analyzed" ), "oto.ini" );
            if ( !PortUtil.isFileExists( oto_ini ) ) {
                // STRAIGHT合成用のoto.iniが存在しないので離脱
                return;
            }

            // 原音設定を取得
            UtauVoiceDB voicedb = null;
            if ( m_voicedb_configs.containsKey( oto_ini ) ) {
                voicedb = m_voicedb_configs.get( oto_ini );
            } else {
                voicedb = new UtauVoiceDB( oto_ini );
                m_voicedb_configs.put( oto_ini, voicedb );
            }

            // eventsのなかから、stfが存在しないものを削除
            for ( int i = count - 1; i >= 0; i-- ) {
                VsqEvent item = events.get( i );
                String search = item.ID.LyricHandle.L0.Phrase;
                OtoArgs oa = voicedb.attachFileNameFromLyric( search );
                if ( oa.fileName == null || (oa.fileName != null && oa.fileName.Equals( "" )) ) {
                    events.removeElementAt( i );
                }
            }

            Vector<VsqEvent> list = new Vector<VsqEvent>();

            count = events.size();
            for ( int i = 1; i < count + 1; i++ ) {
                if ( i == count ) {
                    next = null;
                } else {
                    next = events.get( i );
                }

                double current_sec_start = vsq.getSecFromClock( current.Clock ) - current.UstEvent.PreUtterance / 1000.0;
                double current_sec_end = vsq.getSecFromClock( current.Clock + current.ID.getLength() );
                double next_sec_start = double.MaxValue;
                if ( next != null ) {
                    // 次の音符の開始位置
                    next_sec_start = vsq.getSecFromClock( next.Clock ) - current.UstEvent.PreUtterance / 1000.0 + current.UstEvent.VoiceOverlap / 1000.0;
                    if ( next_sec_start < current_sec_end ) {
                        // 先行発音によって，現在取り扱っている音符「current」の終了時刻がずれる.
                        current_sec_end = next_sec_start;
                    }
                }

                list.add( current );
                // 前の音符との間隔が100ms以下なら，連続していると判断
                if ( next_sec_start - current_sec_end > 0.1 && list.size() > 0 ) {
                    appendQueueCor( vsq, track, list, sample_rate, oto_ini );
                    list.clear();
                }

                // 処理後
                current = next;
            }

            if ( list.size() > 0 ) {
                appendQueueCor( vsq, track, list, sample_rate, oto_ini );
            }
        }

        /// <summary>
        /// 連続した音符を元に，StraightRenderingQueueを作成
        /// </summary>
        /// <param name="list"></param>
        private void appendQueueCor( VsqFileEx vsq, int track, Vector<VsqEvent> list, int sample_rate, String oto_ini ) {
            if ( list.size() <= 0 ) {
                return;
            }

            int OFFSET = 1920;
            CurveType[] CURVE = new CurveType[]{
                    CurveType.PIT,
                    CurveType.PBS,
                    CurveType.DYN,
                    CurveType.BRE,
                    CurveType.GEN, };

            VsqTrack vsq_track = (VsqTrack)vsq.Track.get( track ).clone();
            VsqEvent ve0 = list.get( 0 );
            int first_clock = ve0.Clock;
            int last_clock = ve0.Clock + ve0.ID.getLength();
            if ( list.size() > 1 ) {
                VsqEvent ve9 = list.get( list.size() - 1 );
                last_clock = ve9.Clock + ve9.ID.getLength();
            }
            double start_sec = vsq.getSecFromClock( first_clock ); // 最初の音符の，曲頭からの時間
            int clock_shift = OFFSET - first_clock; // 最初の音符が，ダミー・トラックのOFFSET clockから始まるようシフトする．

            // listの内容を転写
            vsq_track.MetaText.Events.clear();
            int count = list.size();
            for ( int i = 0; i < count; i++ ) {
                VsqEvent ev = (VsqEvent)list.get( i ).clone();
                ev.Clock = ev.Clock + clock_shift;
                vsq_track.MetaText.Events.add( ev );
            }

            // コントロールカーブのクロックをシフトする
            count = CURVE.Length;
            for ( int i = 0; i < count; i++ ) {
                CurveType curve = CURVE[i];
                VsqBPList work = vsq_track.getCurve( curve.getName() );
                if ( work == null ) {
                    continue;
                }
                VsqBPList src = (VsqBPList)work.clone();
                int value_at_first_clock = work.getValue( first_clock );
                work.clear();
                work.add( 0, value_at_first_clock );
                int num_points = src.size();
                for ( int j = 0; j < num_points; j++ ) {
                    int clock = src.getKeyClock( j );
                    if ( 0 <= clock + clock_shift && clock + clock_shift <= last_clock + clock_shift + 1920 ) { // 4拍分の余裕を持って・・・
                        int value = src.getElementA( j );
                        work.add( clock + clock_shift, value );
                    }
                }
            }

            // 最後のクロックがいくつかを調べる
            int tlast_clock = 0;
            for ( Iterator itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                tlast_clock = item.Clock + item.ID.getLength();
            }
            double abstract_sec = tlast_clock / (8.0 * TEMPO);

            StraightRenderingQueue queue = new StraightRenderingQueue();
            // レンダリング結果の何秒後に音符が始まるか？
            queue.startFrame = (int)((start_sec - OFFSET / (8.0 * TEMPO)) * sample_rate);
            queue.oto_ini = oto_ini;
            queue.abstractFrameLength = (long)(abstract_sec * sample_rate);
            queue.endClock = last_clock + clock_shift + 1920;
            queue.track = vsq_track;
            m_queue.add( queue );
        }

        private void prepareMetaText( BufferedWriter writer, VsqTrack vsq_track, String oto_ini, int end_clock ) 
#if JAVA
            throws IOException
#endif
        {
            CurveType[] CURVE = new CurveType[]{
                    CurveType.PIT,
                    CurveType.PBS,
                    CurveType.DYN,
                    CurveType.BRE,
                    CurveType.GEN,
                    CurveType.CLE,
                    CurveType.BRI, };
            // メモリーストリームに出力
            writer.write( "[Tempo]" );
            writer.newLine();
            writer.write( TEMPO + "" );
            writer.newLine();
            writer.write( "[oto.ini]" );
            writer.newLine();
            writer.write( oto_ini );
            writer.newLine();
            Vector<VsqHandle> handles = vsq_track.MetaText.writeEventList( writer, end_clock );
            Vector<String> print_targets = new Vector<String>( Arrays.asList(
                                                               new String[]{ "Length",
                                                                             "Note#",
                                                                             "Dynamics",
                                                                             "DEMdecGainRate",
                                                                             "DEMaccent",
                                                                             "PreUtterance",
                                                                             "VoiceOverlap",
                                                                             "PMBendDepth",
                                                                             "PMBendLength" } ) );
            for ( Iterator itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                item.write( writer, print_targets );
            }
            int count = handles.size();
            for ( int i = 0; i < count; i++ ) {
                handles.get( i ).write( writer );
            }
            count = CURVE.Length;
            for ( int i = 0; i < count; i++ ) {
                CurveType curve = CURVE[i];
                VsqBPList src = vsq_track.getCurve( curve.getName() );
                if ( src == null ) {
                    continue;
                }
                String name = "";
                if ( curve.equals( CurveType.PIT ) ) {
                    name = "[PitchBendBPList]";
                } else if ( curve.equals( CurveType.PBS ) ) {
                    name = "[PitchBendSensBPList]";
                } else if ( curve.equals( CurveType.DYN ) ) {
                    name = "[DynamicsBPList]";
                } else if ( curve.equals( CurveType.BRE ) ) {
                    name = "[EpRResidualBPList]";
                } else if ( curve.equals( CurveType.GEN ) ) {
                    name = "[GenderFactorBPList]";
                } else if ( curve.equals( CurveType.BRI ) ) {
                    name = "[EpRESlopeBPList]";
                } else if ( curve.equals( CurveType.CLE ) ) {
                    name = "[EpRESlopeDepthBPList]";
                } else {
                    continue;
                }
                src.print( writer, 0, name );
            }
        }

        public override double getElapsedSeconds() {
            return PortUtil.getCurrentTime() - m_started_date;
        }

        public override void run() {
            m_started_date = PortUtil.getCurrentTime();
            int BUF_LEN = 1024;
            m_rendering = true;
            m_abort_required = false;
            String straight_synth = PortUtil.combinePath( PortUtil.getApplicationStartupPath(), STRAIGHT_SYNTH );
            if ( !PortUtil.isFileExists( straight_synth ) ) {
#if DEBUG
                PortUtil.println( "StraightRendeingRunner#run; \"" + straight_synth + "\" does not exists" );
#endif
                m_rendering = false;
                return;
            }
            int count = m_queue.size();

            // 合計でレンダリングしなければならないサンプル数を計算しておく
            double total_samples = 0;
            for ( int i = 0; i < count; i++ ) {
                total_samples += m_queue.get( i ).abstractFrameLength;
            }
#if DEBUG
            PortUtil.println( "StraightRenderingRunner#run; total_samples=" + total_samples );
#endif

            m_trim_remain = (int)(trimMillisec / 1000.0 * sampleRate); //先頭から省かなければならないサンプル数の残り
#if DEBUG
            PortUtil.println( "StraightRenderingRunner#run; m_trim_remain=" + m_trim_remain );
#endif
            long max_next_wave_start = m_vsq_length_samples;

            if ( m_queue.size() > 0 ) {
                StraightRenderingQueue queue = m_queue.get( 0 );
                if ( queue.startFrame > 0 ) {
                    double[] silence_l = new double[BUF_LEN];
                    double[] silence_r = new double[BUF_LEN];
                    int remain = queue.startFrame;
                    while ( remain > 0 ) {
                        int len = (remain > BUF_LEN) ? BUF_LEN : remain;
                        if ( len == BUF_LEN ) {
                            for ( int i = 0; i < BUF_LEN; i++ ) {
                                silence_l[i] = 0.0;
                                silence_r[i] = 0.0;
                            }
                            waveIncoming( silence_l, silence_r );
                        } else {
                            double[] t_silence_l = new double[remain];
                            double[] t_silence_r = new double[remain];
                            waveIncoming( t_silence_l, t_silence_r );
                        }
                        remain -= len;
                    }
                }
            }

            double[] cached_data_l = null;
            double[] cached_data_r = null;
            double processed_samples = 0.0;
#if DEBUG
            BufferedWriter log = new BufferedWriter( new FileWriter( PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "StraightRenderingRunner.log" ) ) );
            log.write( "#startFrame\tstartFrame+wave_length\tnext_wave_start\tchache length" );
            log.newLine();
#endif
            for ( int i = 0; i < count; i++ ) {
                if ( m_abort_required ) {
                    m_rendering = false;
                    m_abort_required = false;
#if DEBUG
                    log.close();
#endif
                    return;
                }
                StraightRenderingQueue queue = m_queue.get( i );
                String tmp_dir = AppManager.getTempWaveDir();

                String tmp_file = PortUtil.combinePath( tmp_dir, "tmp.usq" );
                String hash = "";
                BufferedWriter sw = null;
                try {
                    sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( tmp_file ), "Shift_JIS" ) );
                    prepareMetaText( sw, queue.track, queue.oto_ini, queue.endClock );
                } catch ( Exception ex ) {
#if DEBUG
                    PortUtil.println( "StraightRenderingRunner#run; ex=" + ex );
#endif
                } finally {
                    if ( sw != null ) {
                        try {
                            sw.close();
                        } catch ( Exception ex2 ) {
#if DEBUG
                            PortUtil.println( "StraightRenderingRunner#run; ex2=" + ex2 );
#endif
                        }
                    }
                }
                try {
                    hash = PortUtil.getMD5( tmp_file ).Replace( "_", "" );
                } catch ( Exception ex ) {
                }
                try {
                    PortUtil.copyFile( tmp_file, PortUtil.combinePath( tmp_dir, hash + ".usq" ) );
                    PortUtil.deleteFile( tmp_file );
                } catch ( Exception ex ) {
                }
                tmp_file = PortUtil.combinePath( tmp_dir, hash );
                if ( !s_cache.containsKey( hash ) || !PortUtil.isFileExists( tmp_file + ".wav" ) ) {
#if JAVA
                    String[] args = new String[]{ straight_synth, "\"" + tmp_file + ".usq\"", "\"" + tmp_file + ".wav\"" };
                    ProcessBuilder pb = new ProcessBuilder( args );
                    try{
                        Process process = pb.start();
                        process.waitFor();
                    }catch( Exception ex ){
                        System.err.println( "StraightRenderingRunner#run; ex=" + ex );
                    }
#else
                    using ( Process process = new Process() ) {
                        process.StartInfo.FileName = straight_synth;
                        process.StartInfo.Arguments = "\"" + tmp_file + ".usq\" \"" + tmp_file + ".wav\"";
                        process.StartInfo.WorkingDirectory = PortUtil.getApplicationStartupPath();
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        process.Start();
                        process.WaitForExit();
                    }
#endif

#if !DEBUG
                    try {
                        PortUtil.deleteFile( tmp_file + ".usq" );
                    } catch( Exception ex ){
                    }
#endif

                    if ( s_cache.size() > MAX_CACHE ) {
                        // キャッシュの許容個数を超えたので、古いものを削除
                        boolean first = true;
                        double old_date = PortUtil.getCurrentTime();
                        String old_key = "";
                        for ( Iterator itr = s_cache.keySet().iterator(); itr.hasNext(); ) {
                            String key = (String)itr.next();
                            double time = s_cache.get( key );
                            if ( first ) {
                                old_date = time;
                                old_key = key;
                            } else {
                                if ( old_date > time ) {
                                    old_date = time;
                                    old_key = key;
                                }
                            }
                        }
                        s_cache.remove( old_key );
                        try {
                            PortUtil.deleteFile( PortUtil.combinePath( tmp_dir, old_key + ".wav" ) );
                        } catch ( Exception ex ) {
                        }
                    }
                    s_cache.put( hash, PortUtil.getCurrentTime() );
                }

                long next_wave_start = max_next_wave_start;
                if ( i + 1 < count ) {
                    StraightRenderingQueue next_queue = m_queue.get( i + 1 );
                    next_wave_start = next_queue.startFrame;
                }

                WaveReader wr = null;
                try {
                    if ( PortUtil.isFileExists( tmp_file + ".wav" ) ) {
                        wr = new WaveReader( tmp_file + ".wav" );
                    }
                } catch( Exception ex ) {
                    wr = null;
                }
                try {
                    int wave_samples = 0;
                    if ( wr != null ) wave_samples = wr.getTotalSamples();
                    int overlapped = 0;
#if DEBUG
                    log.write( queue.startFrame + "\t" + (queue.startFrame + wave_samples) + "\t" + next_wave_start + "\t" + (cached_data_l == null ? 0 : cached_data_l.Length) );
                    log.newLine();
#endif
                    if ( next_wave_start <= queue.startFrame + wave_samples ) {
                        // 次のキューの開始位置が、このキューの終了位置よりも早い場合
                        // オーバーラップしているサンプル数
                        overlapped = (int)(queue.startFrame + wave_samples - next_wave_start);
                        wave_samples = (int)(next_wave_start - queue.startFrame); //ここまでしか読み取らない
                    } else {
                        //chached_data_l = null;
                        //chached_data_r = null;
                    }

                    if ( cached_data_l == null || cached_data_r == null ) {
#if DEBUG
                        PortUtil.println( "StraightRenderingRunner#run; cache is null" );
#endif
                        // キャッシュが残っていない場合
                        int remain = wave_samples;
                        long pos = 0;
                        double[] left = null, right = null;
                        while ( remain > 0 ) {
                            if ( m_abort_required ) {
                                m_rendering = false;
#if DEBUG
                                log.close();
#endif
                                return;
                            }
                            int len = (remain > BUF_LEN) ? BUF_LEN : remain;
                            if ( left == null || right == null ) {
                                left = new double[len];
                                right = new double[len];
                            } else {
                                if ( left.Length != len || right.Length != len ) {
                                    left = null;
                                    right = null;
                                    left = new double[len];
                                    right = new double[len];
                                }
                            }
                            if ( wr != null ) {
                                wr.read( pos, len, left, right );
                            }
                            waveIncoming( left, right );
                            pos += len;
                            remain -= len;
                        }
                        left = null;
                        right = null;

                        int rendererd_length = 0;
                        if ( wr != null ) rendererd_length = wr.getTotalSamples();
                        if ( wave_samples < rendererd_length ) {
                            // 次のキューのためにデータを残す
                            if ( wr != null ) {
                                if ( cached_data_l == null ) {
                                    cached_data_l = new double[overlapped];
                                } else if ( cached_data_l.Length != overlapped ) {
                                    cached_data_l = null;
                                    cached_data_l = new double[overlapped];
                                }
                                if ( cached_data_r == null ) {
                                    cached_data_r = new double[overlapped];
                                } else if ( cached_data_r.Length != overlapped ) {
                                    cached_data_r = null;
                                    cached_data_r = new double[overlapped];
                                }
                                wr.read( pos, overlapped, cached_data_l, cached_data_r );
                            }
                        } else if ( i + 1 < count ) {
                            // 次のキューのためにデータを残す必要がない場合で、かつ、最後のキューでない場合。
                            // キュー間の無音部分を0で埋める
                            int silence_samples = (int)(next_wave_start - (queue.startFrame + rendererd_length));
                            double[] silence = new double[silence_samples];
                            waveIncoming( silence, silence );
                        }
                    } else {
#if DEBUG
                        PortUtil.println( "StraightRenderingRunner#run; cache is NOT null" );
#endif
                        // キャッシュが残っている場合
                        int rendered_length = 0;
                        if ( wr != null ) rendered_length = wr.getTotalSamples();
                        if ( rendered_length < cached_data_l.Length ) {
                            if ( next_wave_start < queue.startFrame + rendered_length ) {
#if DEBUG
                                PortUtil.println( "StraightRenderingRunner#run; (i) or (ii)" );
#endif
                                // PATTERN A
                                //  ----[*****************************]----------------->  cache
                                //  ----[*********************]------------------------->  renderd result
                                //  -----------------[******************************...->  next rendering queue (not rendered yet)
                                //                  ||
                                //                  \/
                                //  ----[***********]----------------------------------->  append
                                //  -----------------[********][******]----------------->  new cache
                                //  
                                //                         OR
                                // PATTERN B
                                //  ----[*****************************]----------------->   cache
                                //  ----[***************]------------------------------->   rendered result
                                //  ----------------------------[*******************...->   next rendering queue (not rendered yet)
                                //                  ||
                                //                  \/
                                //  ----[***************][*****]------------------------>   append
                                //  ----------------------------[*****]----------------->   new chache
                                //  
                                try {
                                    double[] left = new double[rendered_length];
                                    double[] right = new double[rendered_length];
                                    wr.read( 0, rendered_length, left, right );
                                    for ( int j = 0; j < left.Length; j++ ) {
                                        cached_data_l[j] += left[j];
                                        cached_data_r[j] += right[j];
                                    }
                                    int append_len = (int)(next_wave_start - queue.startFrame);
                                    double[] buf_l = new double[append_len];
                                    double[] buf_r = new double[append_len];
                                    for ( int j = 0; j < append_len; j++ ) {
                                        buf_l[j] = cached_data_l[j];
                                        buf_r[j] = cached_data_r[j];
                                    }
                                    waveIncoming( buf_l, buf_r );
                                    buf_l = null;
                                    buf_r = null;
                                    buf_l = cached_data_l;
                                    buf_r = cached_data_r;
                                    int new_cache_len = (int)((queue.startFrame + buf_l.Length) - next_wave_start);
                                    cached_data_l = new double[new_cache_len];
                                    cached_data_r = new double[new_cache_len];
                                    for ( int j = 0; j < new_cache_len; j++ ) {
                                        cached_data_l[j] = buf_l[j + append_len];
                                        cached_data_r[j] = buf_r[j + append_len];
                                    }
                                } catch ( Exception ex ) {
                                    AppManager.debugWriteLine( "StraightRenderingRunner#run; (A),(B); ex=" + ex );
                                }
                            } else {
#if DEBUG
                                PortUtil.println( "StraightRenderingRunner#run; (iii)" );
#endif
                                // PATTERN C
                                //  ----[*****************************]----------------->   cache
                                //  ----[***************]------------------------------->   rendered result
                                //  -----------------------------------------[******...->   next rendering queue (not rendered yet)
                                //                  ||
                                //                  \/
                                //  ----[*****************************]----------------->   append
                                //  ---------------------------------------------------->   new chache -> NULL!!
                                //  -----------------------------------[****]----------->   append#2 (silence)
                                //  
                                try {
                                    double[] left = new double[rendered_length];
                                    double[] right = new double[rendered_length];
                                    wr.read( 0, rendered_length, left, right );
                                    for ( int j = 0; j < left.Length; j++ ) {
                                        cached_data_l[j] += left[j];
                                        cached_data_r[j] += right[j];
                                    }
                                    waveIncoming( cached_data_l, cached_data_r );
                                    int silence_len = (int)(next_wave_start - (queue.startFrame + cached_data_l.Length));
                                    cached_data_l = null;
                                    cached_data_r = null;
                                    left = new double[silence_len];
                                    right = new double[silence_len];
                                    waveIncoming( left, right );
                                } catch ( Exception ex ) {
                                    AppManager.debugWriteLine( "StraightRenderingRunner#run; (C); ex=" + ex );
                                }
                            }
                        } else {
                            if ( next_wave_start < queue.startFrame + cached_data_l.Length ) {
#if DEBUG
                                PortUtil.println( "StraightRenderingRunner#run; (iv)" );
#endif
                                // PATTERN D
                                //  ----[*************]--------------------------------->  cache
                                //  ----[*********************]------------------------->  renderd result
                                //  ------------[***********************************...->  next rendering queue (not rendered yet)
                                //                  ||
                                //                  \/
                                //  ----[******]---------------------------------------->  append
                                //  ------------[*****][******]------------------------->  new cache
                                //  
                                try {
                                    double[] left = new double[cached_data_l.Length];
                                    double[] right = new double[cached_data_l.Length];
                                    wr.read( 0, cached_data_l.Length, left, right );
                                    for ( int j = 0; j < cached_data_l.Length; j++ ) {
                                        cached_data_l[j] += left[j];
                                        cached_data_r[j] += right[j];
                                    }
#if DEBUG
                                    PortUtil.println( "    next_wave_start=" + next_wave_start + "; queue.startFrame=" + queue.startFrame );
#endif
                                    int append_len = (int)(next_wave_start - queue.startFrame);
#if DEBUG
                                    PortUtil.println( "    append_len=" + append_len );
#endif
                                    double[] buf_l = new double[append_len];
                                    double[] buf_r = new double[append_len];
                                    for ( int j = 0; j < append_len; j++ ) {
                                        buf_l[j] = cached_data_l[j];
                                        buf_r[j] = cached_data_r[j];
                                    }
                                    waveIncoming( buf_l, buf_r );
                                    buf_l = cached_data_l;
                                    buf_r = cached_data_r;
                                    int new_cache_len = (int)((queue.startFrame + rendered_length) - next_wave_start);
                                    cached_data_l = new double[new_cache_len];
                                    cached_data_r = new double[new_cache_len];
                                    int old_cache_len = buf_l.Length;
                                    for ( int j = append_len; j < old_cache_len; j++ ) {
                                        cached_data_l[j - append_len] = buf_l[j];
                                        cached_data_r[j - append_len] = buf_r[j];
                                    }
                                    int tlen = rendered_length - old_cache_len;
                                    buf_l = new double[tlen];
                                    buf_r = new double[tlen];
                                    wr.read( old_cache_len, rendered_length - old_cache_len, buf_l, buf_r );
                                    for ( int j = 0; j < buf_l.Length; j++ ) {
                                        cached_data_l[j + (old_cache_len - append_len)] = buf_l[j];
                                        cached_data_r[j + (old_cache_len - append_len)] = buf_r[j];
                                    }
                                } catch ( Exception ex ) {
                                    AppManager.debugWriteLine( "StraightRenderingRunner#run; (D); ex=" + ex );
                                }
                            } else if ( next_wave_start < queue.startFrame + rendered_length ) {
#if DEBUG
                                PortUtil.println( "StraightRenderingRunner#run; (v)" );
#endif
                                // PATTERN E
                                //  ----[*************]--------------------------------->  cache
                                //  ----[*********************]------------------------->  renderd result
                                //  ----------------------[*************************...->  next rendering queue (not rendered yet)
                                //                  ||
                                //                  \/
                                //  ----[*************][*]------------------------------>  append
                                //  ----------------------[***]------------------------->  new cache
                                //  
                                try {
                                    double[] left = new double[cached_data_l.Length];
                                    double[] right = new double[cached_data_l.Length];
                                    wr.read( 0, cached_data_l.Length, left, right );
                                    for ( int j = 0; j < cached_data_l.Length; j++ ) {
                                        cached_data_l[j] += left[j];
                                        cached_data_r[j] += right[j];
                                    }
                                    waveIncoming( cached_data_l, cached_data_r );
                                    int append_len = (int)(next_wave_start - (queue.startFrame + cached_data_l.Length));
                                    left = null;
                                    right = null;
                                    left = new double[append_len];
                                    right = new double[append_len];
                                    wr.read( cached_data_l.Length, append_len, left, right );
                                    waveIncoming( left, right );
                                    int new_cache_len = (int)(queue.startFrame + rendered_length - next_wave_start);
                                    int old_cache_len = cached_data_l.Length;
                                    cached_data_l = null;
                                    cached_data_r = null;
                                    cached_data_l = new double[new_cache_len];
                                    cached_data_r = new double[new_cache_len];
                                    wr.read( old_cache_len + append_len, new_cache_len, cached_data_l, cached_data_r );
                                } catch ( Exception ex ) {
                                    AppManager.debugWriteLine( "StraightRenderingRunner#run; (E); ex=" + ex );
                                }
                            } else {
#if DEBUG
                                PortUtil.println( "StraightRenderingRunner#run; (vi)" );
#endif
                                // PATTERN F
                                //  ----[*************]--------------------------------->  cache
                                //  ----[*********************]------------------------->  renderd result
                                //  --------------------------------[***************...->  next rendering queue (not rendered yet)
                                //                  ||
                                //                  \/
                                //  ----[*************][******]------------------------->  append
                                //  ---------------------------------------------------->  new cache -> NULL!!
                                //  ---------------------------[***]-------------------->  append#2 (silence)
                                //  
                                try {
                                    double[] left = new double[cached_data_l.Length];
                                    double[] right = new double[cached_data_l.Length];
                                    wr.read( 0, cached_data_l.Length, left, right );
                                    for ( int j = 0; j < cached_data_l.Length; j++ ) {
                                        cached_data_l[j] += left[j];
                                        cached_data_r[j] += right[j];
                                    }
                                    waveIncoming( cached_data_l, cached_data_r );
                                    left = null;
                                    right = null;
                                    int tlen = rendered_length - cached_data_l.Length;
                                    left = new double[tlen];
                                    right = new double[tlen];
                                    wr.read( cached_data_l.Length, rendered_length - cached_data_l.Length, left, right );
                                    waveIncoming( left, right );
                                    cached_data_l = null;
                                    cached_data_r = null;
                                    int silence_len = (int)(next_wave_start - (queue.startFrame + rendered_length));
                                    left = new double[silence_len];
                                    right = new double[silence_len];
                                    waveIncoming( left, right );
                                } catch ( Exception ex ) {
                                    AppManager.debugWriteLine( "StraightRenderingRunner#run; (F); ex=" + ex );
                                }
                            }
                        }
                    }
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "StraightRenderingRunner#run; ex=" + ex );
                } finally {
                    if ( wr != null ) {
                        try {
                            wr.close();
                        } catch ( Exception ex2 ) {
                            PortUtil.stderr.println( "StraightRenderingRunner#run; ex2=" + ex2 );
                        }
                        wr = null;
                    }
                }

                processed_samples += queue.abstractFrameLength;
                m_progress_percent = processed_samples / total_samples * 100.0;
                double elapsed = PortUtil.getCurrentTime() - m_started_date;
                m_running_rate = m_progress_percent / elapsed;
            }
#if DEBUG
            PortUtil.println( "StraightRenderingRunner#m_mode_infinite=" + m_mode_infinite );
            log.newLine();
            log.close();
#endif

            if ( m_mode_infinite ) {
                double[] silence_l = new double[44100];
                double[] silence_r = new double[44100];
                while ( !m_abort_required ) {
                    waveIncoming( silence_l, silence_r );
                }
                silence_l = null;
                silence_r = null;
            }

            m_abort_required = false;
            m_rendering = false;
            if ( directPlay ) {
                PlaySound.waitForExit();
            }
        }

        public override double computeRemainingSeconds() {
            double elapsed = PortUtil.getCurrentTime() - m_started_date;
            double estimated_total_time = 100.0 / m_running_rate;
            double draft = estimated_total_time - elapsed;
            if ( draft < 0 ) {
                draft = 0.0;
            }
            return draft;
        }

        public static void clearCache() {
            String tmp_dir = AppManager.getTempWaveDir();
            for ( Iterator itr = s_cache.keySet().iterator(); itr.hasNext(); ) {
                String key = (String)itr.next();
                try {
                    PortUtil.deleteFile( PortUtil.combinePath( tmp_dir, key + ".wav" ) );
                } catch( Exception ex ) {
                }
            }
            s_cache.clear();
        }

        public override double getProgress() {
            return m_progress_percent;
        }
    }

#if !JAVA
}
#endif
