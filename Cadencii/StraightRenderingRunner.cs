/*
 * StraightRenderingRunner.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;

using Boare.Lib.Media;
using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    public class StraightRenderingRunner : RenderingRunner {
        public static String STRAIGHT_SYNTH = "straightdrv.exe";
        private const int MAX_CACHE = 512;
        private static TreeMap<String, DateTime> s_cache = new TreeMap<String, DateTime>();
        const int TEMPO = 120;
        
        boolean m_rendering = false;
        Vector<StraightRenderingQueue> m_queue;
        Vector<SingerConfig> m_singer_config_sys;
        double m_progress_percent = 0.0;
        int m_rendering_track = 1;
        boolean m_reflect_amp_to_wave = false;
        WaveWriter m_wave_writer = null;
        Vector<WaveReader> m_reader;
        boolean m_direct_play = false;
        double m_wave_read_offset_seconds;
        long m_total_append = 0;
        boolean m_abort_required = false;
        boolean m_mode_infinite;
        double m_trim_msec;
        int m_sample_rate;

        int m_trim_remain = 0;
        TreeMap<String, UtauVoiceDB> m_voicedb_configs = new TreeMap<String, UtauVoiceDB>();
        long m_vsq_length_samples;
        DateTime m_started_date;
        /// <summary>
        /// 現在の処理速度．progress%/sec
        /// </summary>
        double m_running_rate;

        public StraightRenderingRunner( VsqFileEx vsq,
                                        int track,
                                        Vector<SingerConfig> singer_config_sys,
                                        int sample_rate,
                                        int trim_msec,
                                        boolean mode_infinite,
                                        WaveWriter wave_writer,
                                        double wave_read_offset_seconds,
                                        Vector<WaveReader> wave_reader,
                                        boolean direct_play,
                                        boolean reflect_amp_to_wave ) {
            m_queue = new Vector<StraightRenderingQueue>();
            m_singer_config_sys = singer_config_sys;
            m_rendering_track = track;
            m_reflect_amp_to_wave = reflect_amp_to_wave;
            m_wave_writer = wave_writer;
            if ( wave_reader == null ) {
                m_reader = new Vector<WaveReader>();
            } else {
                m_reader = wave_reader;
            }
            m_direct_play = direct_play;
            m_wave_read_offset_seconds = wave_read_offset_seconds;
            m_mode_infinite = mode_infinite;
            m_trim_msec = trim_msec;
            m_sample_rate = sample_rate;
            int midi_tempo = 60000000 / TEMPO;
            VsqFileEx work = (VsqFileEx)vsq.clone();
            VsqFile tempo = new VsqFile( "Miku", 1, 4, 4, midi_tempo );
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
                StraightRenderingQueue q = m_queue.get( m_queue.size() - 1);
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

            string singer = singer_event.ID.IconHandle.IDS;
            int num_singers = m_singer_config_sys.size();
            string singer_path = "";
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
            string oto_ini = Path.Combine( Path.Combine( singer_path, "analyzed" ), "oto.ini" );
            if ( !File.Exists( oto_ini ) ) {
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
                double current_sec_end = vsq.getSecFromClock( current.Clock + current.ID.Length );
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
        private void appendQueueCor( VsqFileEx vsq, int track, Vector<VsqEvent> list, int sample_rate, string oto_ini ) {
            if ( list.size() <= 0 ) {
                return;
            }
            
            const int OFFSET = 1920;
            CurveType[] CURVE = new CurveType[]{
                    CurveType.PIT,
                    CurveType.PBS,
                    CurveType.DYN,
                    CurveType.BRE,
                    CurveType.GEN, };

            VsqTrack vsq_track = (VsqTrack)vsq.Track.get( track ).clone();
            VsqEvent ve0 = list.get( 0 );
            int first_clock = ve0.Clock;
            int last_clock = ve0.Clock + ve0.ID.Length;
            if ( list.size() > 1 ) {
                VsqEvent ve9 = list.get( list.size() - 1 );
                last_clock = ve9.Clock + ve9.ID.Length;
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
                tlast_clock = item.Clock + item.ID.Length;
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

        private void prepareMetaText( StreamWriter writer, VsqTrack vsq_track, String oto_ini, int end_clock ) {
            CurveType[] CURVE = new CurveType[]{
                    CurveType.PIT,
                    CurveType.PBS,
                    CurveType.DYN,
                    CurveType.BRE,
                    CurveType.GEN, };
            // メモリーストリームに出力
            writer.WriteLine( "[Tempo]" );
            writer.WriteLine( TEMPO.ToString() );
            writer.WriteLine( "[oto.ini]" );
            writer.WriteLine( oto_ini );
            Vector<VsqHandle> handles = vsq_track.MetaText.writeEventList( writer, end_clock );
            Vector<String> print_targets = new Vector<String>( new String[]{ "Length",
                                                                             "Note#",
                                                                             "Dynamics",
                                                                             "DEMdecGainRate",
                                                                             "DEMaccent",
                                                                             "PreUtterance",
                                                                             "VoiceOverlap" } );
            for ( Iterator itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                item.write( writer, print_targets );
            }
            int count = handles.size();
            for ( int i = 0; i < count; i++ ) {
                handles.get( i ).write( writer, false );
            }
            count = CURVE.Length;
            for ( int i = 0; i < count; i++ ) {
                CurveType curve = CURVE[i];
                VsqBPList src = vsq_track.getCurve( curve.getName() );
                if ( src == null ) {
                    continue;
                }
                string name = "";
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
                } else {
                    continue;
                }
                src.print( writer, 0, name );
            }
        }

        public double getElapsedSeconds() {
            return DateTime.Now.Subtract( m_started_date ).TotalSeconds;
        }

        public void run() {
            m_started_date = DateTime.Now;
            const int BUF_LEN = 1024;
            m_rendering = true;
            m_abort_required = false;
            String straight_synth = Path.Combine( Application.StartupPath, STRAIGHT_SYNTH );
            if ( !File.Exists( straight_synth ) ) {
#if DEBUG
                Console.WriteLine( "StraightRendeingRunner#run; \"" + straight_synth + "\" does not exists" );
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
            Console.WriteLine( "StraightRenderingRunner#run; total_samples=" + total_samples );
#endif

            m_trim_remain = (int)(m_trim_msec / 1000.0 * m_sample_rate); //先頭から省かなければならないサンプル数の残り
            long max_next_wave_start = m_vsq_length_samples;

            if ( m_queue.size() > 0 ) {
                StraightRenderingQueue queue = m_queue.get( 0 );
                if ( queue.startFrame > 0 ) {
                    double[] silence = new double[BUF_LEN];
                    int remain = queue.startFrame;
                    while ( remain > 0 ) {
                        int len = (remain > BUF_LEN) ? BUF_LEN : remain;
                        if ( len == BUF_LEN ) {
                            WaveIncoming( silence, silence );
                        } else {
                            double[] t_silence = new double[remain];
                            WaveIncoming( t_silence, t_silence );
                        }
                        remain -= len;
                    }
                }
            }

            double[] cached_data_l = null;
            double[] cached_data_r = null;
            double processed_samples = 0.0;
#if DEBUG
            StreamWriter log = new StreamWriter( Path.Combine( Application.StartupPath, "StraightRenderingRunner.log" ) );
            log.AutoFlush = true;
            log.WriteLine( "#startFrame\tstartFrame+wave_length\tnext_wave_start\tchache length" );
#endif
            for ( int i = 0; i < count; i++ ){
                if ( m_abort_required ) {
                    m_rendering = false;
                    m_abort_required = false;
#if DEBUG
                    log.Close();
#endif
                    return;
                }
                StraightRenderingQueue queue = m_queue.get( i );
                String tmp_dir = AppManager.getTempWaveDir();

                String tmp_file = Path.Combine( tmp_dir, "tmp.usq" );
                String hash = "";
                using ( StreamWriter sw = new StreamWriter( tmp_file, false, Encoding.GetEncoding( "Shift_JIS" ) ) ) {
                    prepareMetaText( sw, queue.track, queue.oto_ini, queue.endClock );
                }
                using ( FileStream fs = new FileStream( tmp_file, FileMode.Open, FileAccess.Read ) ) {
                    hash = misc.getmd5( fs );
                }
                try {
                    File.Copy( tmp_file, Path.Combine( tmp_dir, hash + ".usq" ) );
                    File.Delete( tmp_file );
                } catch {
                }
                tmp_file = Path.Combine( tmp_dir, hash );
                if ( !s_cache.containsKey( hash ) || !File.Exists( tmp_file + ".wav" ) ) {
                    using ( Process process = new Process() ) {
                        process.StartInfo.FileName = straight_synth;
                        process.StartInfo.Arguments = "\"" + tmp_file + ".usq\" \"" + tmp_file + ".wav\"";
                        process.StartInfo.WorkingDirectory = Application.StartupPath;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
#if DEBUG
                        //process.StartInfo.RedirectStandardError = true;
                        //process.StartInfo.RedirectStandardOutput = true;
#endif
                        process.Start();
#if DEBUG
                        //String stdout = process.StandardOutput.ReadToEnd();
                        //String stderr = process.StandardError.ReadToEnd();
                        //Console.WriteLine( "StraightRenderingRunner#run; stdout=" + stdout );
                        //Console.WriteLine( "StraightRenderingRunner#run; stderr=" + stderr );
#endif
                        process.WaitForExit();
                    }
                    /*straightdrv.init( queue.metatext, queue.oto_ini );
                    WaveWriter ww = new WaveWriter( tmp_file + ".wav", WaveChannel.Monoral, 16, 44100 );
                    ww.Append( straightdrv.synthesize() );
                    ww.Close();
                    straightdrv.uninitialize();*/

                    try {
                        File.Delete( tmp_file + ".usq" );
                    } catch {
                    }

                    if ( s_cache.size() > MAX_CACHE ) {
                        // キャッシュの許容個数を超えたので、古いものを削除
                        boolean first = true;
                        DateTime old_date = DateTime.Now;
                        String old_key = "";
                        for ( Iterator itr = s_cache.keySet().iterator(); itr.hasNext(); ) {
                            String key = (String)itr.next();
                            DateTime time = s_cache.get( key );
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
                            File.Delete( Path.Combine( tmp_dir, old_key + ".wav" ) );
                        } catch ( Exception ex ) {
                        }
                    }
                    s_cache.put( hash, DateTime.Now );
                }

                long next_wave_start = max_next_wave_start;
                if ( i + 1 < count ) {
                    StraightRenderingQueue next_queue = m_queue.get( i + 1 );
                    next_wave_start = next_queue.startFrame;
                }

                WaveReader wr = null;
                try {
                    if ( File.Exists( tmp_file + ".wav" ) ) {
                        wr = new WaveReader( tmp_file + ".wav" );
                    }
                } catch {
                    wr = null;
                }
                int wave_samples = 0;
                if ( wr != null ) wave_samples = wr.TotalSamples;
                int overlapped = 0;
#if DEBUG
                log.WriteLine( queue.startFrame + "\t" + (queue.startFrame + wave_samples) + "\t" + next_wave_start + "\t" + (cached_data_l == null ? 0 : cached_data_l.Length) );
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
                    Console.WriteLine( "StraightRenderingRunner#run; cache is null" );
#endif
                    // キャッシュが残っていない場合
                    int remain = wave_samples;
                    long pos = 0;
                    while ( remain > 0 ) {
                        int len = (remain > BUF_LEN) ? BUF_LEN : remain;
                        double[] left, right;
                        if ( wr != null ) {
                            wr.Read( pos, len, out left, out right );
                        } else {
                            left = new double[len];
                            right = new double[len];
                        }
                        WaveIncoming( left, right );
                        pos += len;
                        remain -= len;
                    }
                    int rendererd_length = 0;
                    if ( wr != null ) rendererd_length = wr.TotalSamples;
                    if ( wave_samples < rendererd_length ) {
                        // 次のキューのためにデータを残す
                        if ( wr != null ) wr.Read( pos, overlapped, out cached_data_l, out cached_data_r );
                    } else if ( i + 1 < count ) {
                        // 次のキューのためにデータを残す必要がない場合で、かつ、最後のキューでない場合。
                        // キュー間の無音部分を0で埋める
                        int silence_samples = (int)(next_wave_start - (queue.startFrame + rendererd_length));
                        double[] silence = new double[silence_samples];
                        WaveIncoming( silence, silence );
                    }
                } else {
#if DEBUG
                    Console.WriteLine( "StraightRenderingRunner#run; cache is NOT null" );
#endif
                    // キャッシュが残っている場合
                    int rendered_length = 0;
                    if ( wr != null ) rendered_length = wr.TotalSamples;
                    if ( rendered_length < cached_data_l.Length ) {
                        if ( next_wave_start < queue.startFrame + rendered_length ) {
#if DEBUG
                            Console.WriteLine( "StraightRenderingRunner#run; (i) or (ii)" );
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
                                double[] left, right;
                                wr.Read( 0, rendered_length, out left, out right );
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
                                WaveIncoming( buf_l, buf_r );
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
                            Console.WriteLine( "StraightRenderingRunner#run; (iii)" );
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
                                double[] left, right;
                                wr.Read( 0, rendered_length, out left, out right );
                                for ( int j = 0; j < left.Length; j++ ) {
                                    cached_data_l[j] += left[j];
                                    cached_data_r[j] += right[j];
                                }
                                WaveIncoming( cached_data_l, cached_data_r );
                                int silence_len = (int)(next_wave_start - (queue.startFrame + cached_data_l.Length));
                                cached_data_l = null;
                                cached_data_r = null;
                                left = new double[silence_len];
                                right = new double[silence_len];
                                WaveIncoming( left, right );
                            } catch ( Exception ex ) {
                                AppManager.debugWriteLine( "StraightRenderingRunner#run; (C); ex=" + ex );
                            }
                        }
                    } else {
                        if ( next_wave_start < queue.startFrame + cached_data_l.Length ) {
#if DEBUG
                            Console.WriteLine( "StraightRenderingRunner#run; (iv)" );
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
                                double[] left, right;
                                wr.Read( 0, cached_data_l.Length, out left, out right );
                                for ( int j = 0; j < cached_data_l.Length; j++ ) {
                                    cached_data_l[j] += left[j];
                                    cached_data_r[j] += right[j];
                                }
#if DEBUG
                                Console.WriteLine( "    next_wave_start=" + next_wave_start + "; queue.startFrame=" + queue.startFrame );
#endif
                                int append_len = (int)(next_wave_start - queue.startFrame);
#if DEBUG
                                Console.WriteLine( "    append_len=" + append_len );
#endif
                                double[] buf_l = new double[append_len];
                                double[] buf_r = new double[append_len];
                                for ( int j = 0; j < append_len; j++ ) {
                                    buf_l[j] = cached_data_l[j];
                                    buf_r[j] = cached_data_r[j];
                                }
                                WaveIncoming( buf_l, buf_r );
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
                                wr.Read( old_cache_len, rendered_length - old_cache_len, out buf_l, out buf_r );
                                for ( int j = 0; j < buf_l.Length; j++ ) {
                                    cached_data_l[j + (old_cache_len - append_len)] = buf_l[j];
                                    cached_data_r[j + (old_cache_len - append_len)] = buf_r[j];
                                }
                            } catch ( Exception ex ) {
                                AppManager.debugWriteLine( "StraightRenderingRunner#run; (D); ex=" + ex );
                            }
                        } else if ( next_wave_start < queue.startFrame + rendered_length ) {
#if DEBUG
                            Console.WriteLine( "StraightRenderingRunner#run; (v)" );
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
                                double[] left, right;
                                wr.Read( 0, cached_data_l.Length, out left, out right );
                                for ( int j = 0; j < cached_data_l.Length; j++ ) {
                                    cached_data_l[j] += left[j];
                                    cached_data_r[j] += right[j];
                                }
                                WaveIncoming( cached_data_l, cached_data_r );
                                int append_len = (int)(next_wave_start - (queue.startFrame + cached_data_l.Length));
                                wr.Read( cached_data_l.Length, append_len, out left, out right );
                                WaveIncoming( left, right );
                                int new_cache_len = (int)(queue.startFrame + rendered_length - next_wave_start);
                                int old_cache_len = cached_data_l.Length;
                                wr.Read( old_cache_len + append_len, new_cache_len, out cached_data_l, out cached_data_r );
                            } catch ( Exception ex ) {
                                AppManager.debugWriteLine( "StraightRenderingRunner#run; (E); ex=" + ex );
                            }
                        } else {
#if DEBUG
                            Console.WriteLine( "StraightRenderingRunner#run; (vi)" );
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
                                double[] left, right;
                                wr.Read( 0, cached_data_l.Length, out left, out right );
                                for ( int j = 0; j < cached_data_l.Length; j++ ) {
                                    cached_data_l[j] += left[j];
                                    cached_data_r[j] += right[j];
                                }
                                WaveIncoming( cached_data_l, cached_data_r );
                                wr.Read( cached_data_l.Length, rendered_length - cached_data_l.Length, out left, out right );
                                WaveIncoming( left, right );
                                cached_data_l = null;
                                cached_data_r = null;
                                int silence_len = (int)(next_wave_start - (queue.startFrame + rendered_length));
                                left = new double[silence_len];
                                right = new double[silence_len];
                                WaveIncoming( left, right );
                            } catch ( Exception ex ) {
                                AppManager.debugWriteLine( "StraightRenderingRunner#run; (F); ex=" + ex );
                            }
                        }
                    }
                }

                if ( wr != null ) {
                    wr.Close();
                    wr = null;
                }

                processed_samples += queue.abstractFrameLength;
                m_progress_percent = processed_samples / total_samples * 100.0;
                double elapsed = DateTime.Now.Subtract( m_started_date ).TotalSeconds;
                m_running_rate = m_progress_percent / elapsed;
            }
#if DEBUG
            log.Close();
#endif

            if ( m_mode_infinite ) {
                double[] silence_l = new double[44100];
                double[] silence_r = new double[44100];
                while ( !m_abort_required ) {
                    WaveIncoming( silence_l, silence_r );
                }
                silence_l = null;
                silence_r = null;
            }

            m_abort_required = false;
            m_rendering = false;
        }

        public double computeRemainingSeconds() {
            double elapsed = DateTime.Now.Subtract( m_started_date ).TotalSeconds;
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
                    File.Delete( Path.Combine( tmp_dir, key + ".wav" ) );
                } catch {
                }
            }
            s_cache.clear();
        }

        public double getProgress() {
            return m_progress_percent;
        }

        public boolean isRendering() {
            return m_rendering;
        }

        public void abortRendering() {
            if ( m_rendering ) {
                m_abort_required = true;
                while ( m_abort_required ) {
                    Application.DoEvents();
                }
            }
            m_rendering = false;
            m_abort_required = false;
            int count = m_reader.size();
            for ( int i = 0; i < count; i++ ) {
                m_reader.get( i ).Close();
                m_reader.set( i, null );
            }
            m_reader.clear();
        }

        private void WaveIncoming( double[] L, double[] R ) {
            if ( !m_rendering ) {
                return;
            }

            int length = L.Length;
            int i_start = 0;
            if ( m_trim_remain > 0 ) {
                if ( L.Length <= m_trim_remain ) {
                    m_trim_remain -= L.Length;
                    return;
                } else {
                    i_start = L.Length - m_trim_remain;
                    m_trim_remain = 0;
                }
            }

            AmplifyCoefficient amplify = AppManager.getAmplifyCoeffNormalTrack( m_rendering_track );
            if ( m_reflect_amp_to_wave ) {
                for ( int i = i_start; i < length; i++ ) {
                    if ( i % 100 == 0 ) {
                        amplify = AppManager.getAmplifyCoeffNormalTrack( m_rendering_track );
                    }
                    L[i] = L[i] * amplify.left;
                    R[i] = R[i] * amplify.right;
                }
                if ( m_wave_writer != null ) {
                    m_wave_writer.Append( L, R );
                }
            } else {
                if ( m_wave_writer != null ) {
                    m_wave_writer.Append( L, R );
                }
                for ( int i = i_start; i < length; i++ ) {
                    if ( i % 100 == 0 ) {
                        amplify = AppManager.getAmplifyCoeffNormalTrack( m_rendering_track );
                    }
                    L[i] = L[i] * amplify.left;
                    R[i] = R[i] * amplify.right;
                }
            }
            long start = m_total_append + (long)(m_wave_read_offset_seconds * VSTiProxy.SAMPLE_RATE);
            int count = m_reader.size();
            for ( int i = 0; i < count; i++ ) {
                WaveReader wr = m_reader.get( i );
                amplify.left = 1.0;
                amplify.right = 1.0;
                if ( wr.Tag != null && wr.Tag is int ) {
                    int track = (int)wr.Tag;
                    if ( 0 < track ) {
                        amplify = AppManager.getAmplifyCoeffNormalTrack( track );
                    } else if ( 0 > track ) {
                        amplify = AppManager.getAmplifyCoeffBgm( -track - 1 );
                    }
                }
                double[] reader_r;
                double[] reader_l;
                wr.Read( start, length, out reader_l, out reader_r );
                for ( int j = i_start; j < length; j++ ) {
                    L[j] += reader_l[j] * amplify.left;
                    R[j] += reader_r[j] * amplify.right;
                }
                reader_l = null;
                reader_r = null;
            }
            if ( m_direct_play ) {
                if ( i_start == 0 ) {
                    PlaySound.Append( L, R, L.Length );
                } else {
                    for ( int i = i_start; i < length; i++ ) {
                        L[i - i_start] = L[i];
                        R[i - i_start] = R[i];
                    }
                    PlaySound.Append( L, R, length - i_start );
                }
            }
            m_total_append += (length - i_start);
        }
    }

}
