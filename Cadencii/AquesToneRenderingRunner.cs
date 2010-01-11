#if ENABLE_AQUESTONE
/*
 * AquesToneRenderingRunner.cs
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
using System;
using org.kbinani;
using org.kbinani.java.awt;
using org.kbinani.java.util;
using org.kbinani.vsq;
using org.kbinani.media;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using Integer = System.Int32;
    using Float = System.Single;

#if JAVA
    public class AquesToneRenderingRunner extends RenderingRunner {
#else
    public class AquesToneRenderingRunner : RenderingRunner {
#endif
        private AquesToneDriver driver = null;
        private String tempDir;
        private boolean modeInfinite;
        private VsqFileEx vsq = null;

        /// <summary>
        /// ドライバのパラメータの変更要求
        /// </summary>
        private class ParameterEvent {
            public int index;
            public float value;
        }

        private class MidiEventQueue {
            public Vector<MidiEvent> noteoff;
            //public ParameterEvent singer;
            public Vector<MidiEvent> noteon;
            public Vector<MidiEvent> pit;
            //public ParameterEvent pbs;
            public Vector<ParameterEvent> param;
        }

        public AquesToneRenderingRunner(
            AquesToneDriver driver,
            VsqFileEx vsq,
            int track,
            String temp_dir,
            int sample_rate,
            int trim_msec,
            long total_samples,
            boolean mode_infinite,
            WaveWriter wave_writer,
            double wave_read_offset_seconds,
            Vector<WaveReader> readers,
            boolean direct_play,
            boolean reflect_amp_to_wave 
        )
#if JAVA
        {
#else
            :
#endif
            base( track, reflect_amp_to_wave, wave_writer, wave_read_offset_seconds, readers, direct_play, trim_msec, total_samples, sample_rate )
#if JAVA
            ;
#else
        {
#endif
            this.vsq = vsq;
            this.driver = driver;
            tempDir = temp_dir;
            modeInfinite = mode_infinite;
        }

        public override void run() {
            m_rendering = true;
            m_abort_required = false;

            VsqTrack track = vsq.Track.get( renderingTrack );
            int BUFLEN = sampleRate / 10;
            double[] left = new double[BUFLEN];
            double[] right = new double[BUFLEN];
            long saProcessed = 0; // これまでに合成したサンプル数
            int saRemain = 0;
            int lastClock = 0; // 最後に処理されたゲートタイム

#if DEBUG
            /* // この部分はテスト用
            // ノートオンとpitを同時に送ってみる->NG
            // noteonののち、pitを送ってみる->NG
            // pitののち、noteonを送ってみる->NG
            MidiEvent pit0 = new MidiEvent();
            pit0.firstByte = 0xE0;
            int value = (0x3fff & (0 + 0x2000));
            byte msb = (byte)(value >> 7);
            byte lsb = (byte)(value - (msb << 7));
            pit0.data = new byte[] { lsb, msb };
            MidiEvent noteon = new MidiEvent();
            noteon.firstByte = 0x90;
            noteon.data = new byte[] { 0x40, 0x7f };
            driver.send( new MidiEvent[] { pit0, noteon } );
            for ( int i = 0; i < 20; i++ ) {
                driver.process( left, right );
                waveIncoming( left, right );
            }
            MidiEvent pit = new MidiEvent();
            pit.firstByte = 0xE0;
            value = (0x3fff & (8191 + 0x2000));
            msb = (byte)(value >> 7);
            lsb = (byte)(value - (msb << 7));
            pit.data = new byte[] { lsb, msb };
            MidiEvent noteoff = new MidiEvent();
            noteoff.firstByte = 0x80;
            noteoff.data = new byte[] { 0x40, 0x7f };
            driver.send( new MidiEvent[] { noteoff } );
            driver.send( new MidiEvent[] { noteon } );
            driver.send( new MidiEvent[] { pit } );
            for ( int i = 0; i < 20; i++ ) {
                driver.process( left, right );
                waveIncoming( left, right );
            }
            m_rendering = false;
            return;*/
#endif


            // 最初にダミーの音を鳴らす
            // (最初に入るノイズを回避するためと、前回途中で再生停止した場合に無音から始まるようにするため)
            driver.resetAllParameters();
            driver.process( left, right );
            MidiEvent f_noteon = new MidiEvent();
            f_noteon.firstByte = 0x90;
            f_noteon.data = new byte[] { 0x40, 0x40 };
            driver.send( new MidiEvent[] { f_noteon } );
            driver.process( left, right );
            MidiEvent f_noteoff = new MidiEvent();
            f_noteoff.firstByte = 0x80;
            f_noteoff.data = new byte[] { 0x40, 0x7F };
            driver.send( new MidiEvent[] { f_noteoff } );
            for ( int i = 0; i < 3; i++ ) {
                driver.process( left, right );
            }

            // レンダリング開始位置での、パラメータの値をセットしておく


            for ( Iterator itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                long saNoteStart = (long)(vsq.getSecFromClock( item.Clock ) * sampleRate);
                long saNoteEnd = (long)(vsq.getSecFromClock( item.Clock + item.ID.getLength() ) * sampleRate);

                TreeMap<Integer, MidiEventQueue> list = generateMidiEvent( vsq, renderingTrack, lastClock, item.Clock + item.ID.getLength() );
                lastClock = item.Clock + item.ID.Length + 1;
                for ( Iterator itr2 = list.keySet().iterator(); itr2.hasNext(); ) {
                    // まず直前までの分を合成
                    Integer clock = (Integer)itr2.next();
                    long saStart = (long)(vsq.getSecFromClock( clock ) * sampleRate);
                    saRemain = (int)(saStart - saProcessed);
                    while ( saRemain > 0 ) {
                        if ( m_abort_required ) {
                            m_rendering = false;
                            return;
                        }
                        int len = saRemain > BUFLEN ? BUFLEN : saRemain;
                        double[] bufl = null;
                        double[] bufr = null;
                        if ( len == BUFLEN ) {
                            bufl = left;
                            bufr = right;
                        } else {
                            bufl = new double[len];
                            bufr = new double[len];
                        }
                        driver.process( bufl, bufr );
                        waveIncoming( bufl, bufr );
                        saRemain -= len;
                        saProcessed += len;
                    }

                    // MIDiイベントを送信
                    MidiEventQueue queue = list.get( clock );
                    // まずnoteoff
                    boolean noteoff_send = false;
                    if ( queue.noteoff != null ) {
                        driver.send( queue.noteoff.toArray( new MidiEvent[] { } ) );
                        noteoff_send = true;
                    }
                    // parameterの変更
                    if ( queue.param != null ) {
                        for ( Iterator itr3 = queue.param.iterator(); itr3.hasNext(); ) {
                            ParameterEvent pe = (ParameterEvent)itr3.next();
                            driver.setParameter( pe.index, pe.value );
                        }
                    }
                    // ついでnoteon
                    if ( queue.noteon != null && queue.noteon.size() > 0 ) {
                        // 同ゲートタイムにピッチベンドも指定されている場合、同時に送信しないと反映されないようだ！
                        if ( queue.pit != null && queue.pit.size() > 0 ) {
                            queue.noteon.addAll( queue.pit );
                            queue.pit.clear();
                        }
                        driver.send( queue.noteon.toArray( new MidiEvent[] { } ) );
                    }
                    // PIT
                    if ( queue.pit != null && queue.pit.size() > 0 && !noteoff_send ) {
                        driver.send( queue.pit.toArray( new MidiEvent[] { } ) );
                    }
                    if ( driver.getUi() != null ) {
                        driver.getUi().invalidateUi();
                    }
                }
            }

            // totalSamplesに足りなかったら、追加してレンダリング
            saRemain = (int)(totalSamples - saProcessed);
#if DEBUG
            PortUtil.println( "AquesToneRenderingRunner#run; totalSamples=" + totalSamples + "; saProcessed=" + saProcessed + "; saRemain=" + saRemain );
#endif
            while ( saRemain > 0 ) {
                if ( m_abort_required ) {
                    m_rendering = false;
                    return;
                }
                int len = saRemain > BUFLEN ? BUFLEN : saRemain;
                double[] bufl = null;
                double[] bufr = null;
                if ( len == BUFLEN ) {
                    bufl = left;
                    bufr = right;
                } else {
                    bufl = new double[len];
                    bufr = new double[len];
                }
                driver.process( bufl, bufr );
                waveIncoming( bufl, bufr );
                saRemain -= len;
                saProcessed += len;
            }

            // modeInfiniteなら、中止要求が来るまで無音を追加
#if DEBUG
            PortUtil.println( "AquesToneRenderingRunner#run; modeInfinite=" + modeInfinite );
#endif
            if ( modeInfinite ) {
                for ( int i = 0; i < BUFLEN; i++ ) {
                    left[i] = 0.0;
                    right[i] = 0.0;
                }
                while ( !m_abort_required ) {
                    waveIncoming( left, right );
                }
            }
            m_rendering = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="track"></param>
        /// <param name="clock_start"></param>
        /// <param name="clock_end"></param>
        /// <returns></returns>
        private TreeMap<Integer, MidiEventQueue> generateMidiEvent( VsqFileEx vsq, int track, int clock_start, int clock_end ) {
            TreeMap<Integer, MidiEventQueue> list = new TreeMap<Integer, MidiEventQueue>();
            VsqTrack t = vsq.Track.get( track );

            // 歌手変更
            for ( Iterator itr = t.getSingerEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( clock_start <= item.Clock && item.Clock <= clock_end ) {
                    if ( item.ID.IconHandle == null ) {
                        continue;
                    }
                    int program = item.ID.IconHandle.Program;
                    if ( 0 > program || program >= AquesToneDriver.SINGERS.Length ) {
                        program = 0;
                    }
                    ParameterEvent singer = new ParameterEvent();
                    singer.index = driver.phontParameterIndex;
                    singer.value = program + 0.01f;
                    if ( !list.containsKey( item.Clock ) ) {
                        list.put( item.Clock, new MidiEventQueue() );
                    }
                    MidiEventQueue queue = list.get( item.Clock );
                    if ( queue.param == null ) {
                        queue.param = new Vector<ParameterEvent>();
                    }
                    queue.param.add( singer );
                } else if ( clock_end < item.Clock ) {
                    break;
                }
            }

            // ノートon, off
            Vector<Point> pit_send = new Vector<Point>(); // PITが追加されたゲートタイム。音符先頭の分を重複して送信するのを回避するために必要。
            VsqBPList pit = t.getCurve( "pit" );
            VsqBPList pbs = t.getCurve( "pbs" );
            VsqBPList dyn = t.getCurve( "dyn" );
            VsqBPList bre = t.getCurve( "bre" );
            VsqBPList cle = t.getCurve( "cle" );
            VsqBPList por = t.getCurve( "por" );
            for ( Iterator itr = t.getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                int endclock = item.Clock + item.ID.getLength();
                boolean contains_start = clock_start <= item.Clock && item.Clock <= clock_end;
                boolean contains_end = clock_start <= endclock && endclock <= clock_end;
                if ( contains_start || contains_end ) {
                    if ( contains_start ) {
                        #region contains_start
                        // noteonのゲートタイムが，範囲に入っている
                        // noteon MIDIイベントを作成
                        String lyric = item.ID.LyricHandle.L0.Phrase;
                        String katakana = KanaDeRomanization.hiragana2katakana( KanaDeRomanization.Attach( lyric ) );
                        int index = -1;
                        for ( int i = 0; i < AquesToneDriver.PHONES.Length; i++ ) {
                            if ( katakana.Equals( AquesToneDriver.PHONES[i] ) ) {
                                index = i;
                                break;
                            }
                        }
                        if ( index >= 0 ) {
                            if ( !list.containsKey( item.Clock ) ) {
                                list.put( item.Clock, new MidiEventQueue() );
                            }
                            MidiEventQueue queue = list.get( item.Clock );
                            if ( queue.noteon == null ) {
                                queue.noteon = new Vector<MidiEvent>();
                            }

                            // index行目に移動するコマンドを贈る
                            MidiEvent moveline = new MidiEvent();
                            moveline.firstByte = (byte)0xb0;
                            moveline.data = new byte[] { (byte)0x0a, (byte)index };
                            MidiEvent noteon = new MidiEvent();
                            noteon.firstByte = (byte)0x90;
                            noteon.data = new byte[] { (byte)item.ID.Note, (byte)item.ID.Dynamics };
                            Vector<MidiEvent> add = Arrays.asList( new MidiEvent[] { moveline, noteon } );
                            queue.noteon.addAll( add );
                            pit_send.add( new Point( item.Clock, item.Clock ) );
                        }

                        /* 音符頭で設定するパラメータ */
                        // Release
                        MidiEventQueue q = null;
                        if ( !list.containsKey( item.Clock ) ) {
                            q = new MidiEventQueue();
                        } else {
                            q = list.get( item.Clock );
                        }
                        if ( q.param == null ) {
                            q.param = new Vector<ParameterEvent>();
                        }

                        String strRelease = VsqFileEx.getEventTag( item, VsqFileEx.TAGNAME_AQUESTONE_RELEASE );
                        int release = 64;
                        try {
                            release = PortUtil.parseInt( strRelease );
                        } catch ( Exception ex ) {
                            release = 64;
                        }
                        ParameterEvent pe = new ParameterEvent();
                        pe.index = driver.releaseParameterIndex;
                        pe.value = release / 127.0f;
                        q.param.add( pe );

                        // dyn
                        int dynAtStart = dyn.getValue( item.Clock );
                        ParameterEvent peDyn = new ParameterEvent();
                        peDyn.index = driver.volumeParameterIndex;
                        peDyn.value = (float)(dynAtStart - dyn.getMinimum()) / (float)(dyn.getMaximum() - dyn.getMinimum());
                        q.param.add( peDyn );

                        // bre
                        int breAtStart = bre.getValue( item.Clock );
                        ParameterEvent peBre = new ParameterEvent();
                        peBre.index = driver.haskyParameterIndex;
                        peBre.value = (float)(breAtStart - bre.getMinimum()) / (float)(bre.getMaximum() - bre.getMinimum());
                        q.param.add( peBre );

                        // cle
                        int cleAtStart = cle.getValue( item.Clock );
                        ParameterEvent peCle = new ParameterEvent();
                        peCle.index = driver.resonancParameterIndex;
                        peCle.value = (float)(cleAtStart - cle.getMinimum()) / (float)(cle.getMaximum() - cle.getMinimum());
                        q.param.add( peCle );

                        // por
                        int porAtStart = por.getValue( item.Clock );
                        ParameterEvent pePor = new ParameterEvent();
                        pePor.index = driver.portaTimeParameterIndex;
                        pePor.value = (float)(porAtStart - por.getMinimum()) / (float)(por.getMaximum() - por.getMinimum());
                        q.param.add( pePor );
                        #endregion
                    }

                    // ビブラート
                    // ビブラートが存在する場合、PBSは勝手に変更する。
                    if ( item.ID.VibratoHandle == null ) {
                        if ( contains_start ) {
                            // 音符頭のPIT, PBSを強制的に指定
                            int notehead_pit = pit.getValue( item.Clock );
                            MidiEvent pit0 = getPitMidiEvent( notehead_pit );
                            if ( !list.containsKey( item.Clock ) ) {
                                list.put( item.Clock, new MidiEventQueue() );
                            }
                            MidiEventQueue queue = list.get( item.Clock );
                            if ( queue.pit == null ) {
                                queue.pit = new Vector<MidiEvent>();
                            } else {
                                queue.pit.clear();
                            }
                            queue.pit.add( pit0 );
                            int notehead_pbs = pbs.getValue( item.Clock );
                            ParameterEvent pe = new ParameterEvent();
                            pe.index = driver.bendLblParameterIndex;
                            pe.value = notehead_pbs / 13.0f;
                            if ( queue.param == null ) {
                                queue.param = new Vector<ParameterEvent>();
                            }
                            queue.param.add( pe );
                        }
                    } else {
                        int delta_clock = 5;  //ピッチを取得するクロック間隔
                        int tempo = 120;
                        double sec_start_act = vsq.getSecFromClock( item.Clock );
                        double sec_end_act = vsq.getSecFromClock( item.Clock + item.ID.getLength() );
                        double delta_sec = delta_clock / (8.0 * tempo); //ピッチを取得する時間間隔
                        float pitmax = 0.0f;
                        int st = item.Clock;
                        if ( st < clock_start ) {
                            st = clock_start;
                        }
                        int end = item.Clock + item.ID.getLength();
                        if ( clock_end < end ){
                            end = clock_end;
                        }
                        pit_send.add( new Point( st, end ) );
                        // ビブラートが始まるまでのピッチを取得
                        double sec_vibstart = vsq.getSecFromClock( item.Clock + item.ID.VibratoDelay );
                        int pit_count = (int)((sec_vibstart - sec_start_act) / delta_sec);
                        TreeMap<Integer, Float> pit_change = new TreeMap<Integer, Float>();
                        for ( int i = 0; i < pit_count; i++ ) {
                            double gtime = sec_start_act + delta_sec * i;
                            int clock = (int)vsq.getClockFromSec( gtime );
                            float pvalue = (float)t.getPitchAt( clock );
                            pitmax = Math.Max( pitmax, Math.Abs( pvalue ) );
                            pit_change.put( clock, pvalue );
                        }
                        // ビブラート部分のピッチを取得
                        Vector<PointD> ret = FormMain.getVibratoPoints( vsq,
                                                                        item.ID.VibratoHandle.RateBP,
                                                                        item.ID.VibratoHandle.StartRate,
                                                                        item.ID.VibratoHandle.DepthBP,
                                                                        item.ID.VibratoHandle.StartDepth,
                                                                        item.Clock + item.ID.VibratoDelay,
                                                                        item.ID.getLength() - item.ID.VibratoDelay,
                                                                        (float)delta_sec );
                        int c = ret.size();
                        for ( int i = 0; i < c; i++ ) {
                            float gtime = (float)ret.get( i ).getX();
                            int clock = (int)vsq.getClockFromSec( gtime );
                            float pvalue = (float)(t.getPitchAt( clock ) + ret.get( i ).getY() * 100.0);
                            pitmax = Math.Max( pitmax, Math.Abs( pvalue ) );
                            pit_change.put( clock, pvalue );
                        }

                        // ピッチベンドの最大値を実現するのに必要なPBS
                        int required_pbs = (int)Math.Ceiling( pitmax / 100.0 );
#if DEBUG
                        PortUtil.println( "AquesToneRenderingRunner#generateMidiEvent; required_pbs=" + required_pbs );
#endif
                        if ( required_pbs > 13 ){
                            required_pbs = 13;
                        }
                        if ( !list.containsKey( item.Clock ) ) {
                            list.put( item.Clock, new MidiEventQueue() );
                        }
                        MidiEventQueue queue = list.get( item.Clock );
                        ParameterEvent pe = new ParameterEvent();
                        pe.index = driver.bendLblParameterIndex;
                        pe.value = required_pbs / 13.0f;
                        if ( queue.param == null ) {
                            queue.param = new Vector<ParameterEvent>();
                        }
                        queue.param.add( pe );

                        // PITを順次追加
                        for ( Iterator itr2 = pit_change.keySet().iterator(); itr2.hasNext(); ) {
                            Integer clock = (Integer)itr2.next();
                            if ( clock_start <= clock && clock <= clock_end ) {
                                float pvalue = pit_change.get( clock );
                                int pit_value = (int)(8192.0 / (double)required_pbs * pvalue / 100.0);
                                if ( !list.containsKey( clock ) ) {
                                    list.put( clock, new MidiEventQueue() );
                                }
                                MidiEventQueue q = list.get( clock );
                                MidiEvent me = getPitMidiEvent( pit_value );
                                if ( q.pit == null ) {
                                    q.pit = new Vector<MidiEvent>();
                                } else {
                                    q.pit.clear();
                                }
                                q.pit.add( me );
                            } else if ( clock_end < clock ) {
                                break;
                            }
                        }
                    }

                    //pit_send.add( pit_send_p );

                    // noteoff MIDIイベントを作成
                    if ( contains_end ) {
                        MidiEvent noteoff = new MidiEvent();
                        noteoff.firstByte = (byte)0x80;
                        noteoff.data = new byte[] { (byte)item.ID.Note, 0x40 }; // ここのvel
                        Vector<MidiEvent> a_noteoff = Arrays.asList( new MidiEvent[] { noteoff } );
                        if ( !list.containsKey( endclock ) ) {
                            list.put( endclock, new MidiEventQueue() );
                        }
                        MidiEventQueue q = list.get( endclock );
                        if ( q.noteoff == null ) {
                            q.noteoff = new Vector<MidiEvent>();
                        }
                        q.noteoff.addAll( a_noteoff );
                        pit_send.add( new Point( endclock, endclock ) ); // PITの送信を抑制するために必要
                    }
                }

                if ( clock_end < item.Clock ) {
                    break;
                }
            }

            // pitch bend sensitivity
            // RPNで送信するのが上手くいかないので、parameterを直接いぢる
            if ( pbs != null ) {
                int keycount = pbs.size();
                for ( int i = 0; i < keycount; i++ ) {
                    int clock = pbs.getKeyClock( i );
                    if ( clock_start <= clock && clock <= clock_end ) {
                        int value = pbs.getElementA( i );
                        ParameterEvent pbse = new ParameterEvent();
                        pbse.index = driver.bendLblParameterIndex;
                        pbse.value = value / 13.0f;
                        MidiEventQueue queue = null;
                        if ( list.containsKey( clock ) ) {
                            queue = list.get( clock );
                        }else{
                            queue = new MidiEventQueue();
                        }
                        if ( queue.param == null ) {
                            queue.param = new Vector<ParameterEvent>();
                        }
                        queue.param.add( pbse );
                        list.put( clock, queue );
                    } else if ( clock_end < clock ) {
                        break;
                    }
                }
            }

            // pitch bend
            if ( pit != null ) {
                int keycount = pit.size();
                for ( int i = 0; i < keycount; i++ ) {
                    int clock = pit.getKeyClock( i );
                    if ( clock_start <= clock && clock <= clock_end ) {
                        boolean contains = false;
                        for ( Iterator itr = pit_send.iterator(); itr.hasNext(); ) {
                            Point p = (Point)itr.next();
                            if ( p.x <= clock && clock <= p.y ) {
                                contains = true;
                                break;
                            }
                        }
                        if ( contains ) {
                            continue;
                        }
                        int value = pit.getElementA( i );
                        MidiEvent pbs0 = getPitMidiEvent( value );
                        MidiEventQueue queue = null;
                        if ( list.containsKey( clock ) ) {
                            queue = list.get( clock );
                        }else{
                            queue = new MidiEventQueue();
                        }
                        if ( queue.pit == null ) {
                            queue.pit = new Vector<MidiEvent>();
                        } else {
                            queue.pit.clear();
                        }
                        queue.pit.add( pbs0 );
                        list.put( clock, queue );
                    } else if ( clock_end < clock ) {
                        break;
                    }
                }
            }

            appendParameterEvents( list, dyn, driver.volumeParameterIndex, clock_start, clock_end );
            appendParameterEvents( list, bre, driver.haskyParameterIndex, clock_start, clock_end );
            appendParameterEvents( list, cle, driver.resonancParameterIndex, clock_start, clock_end );
            appendParameterEvents( list, por, driver.portaTimeParameterIndex, clock_start, clock_end );

            return list;
        }

        private static void appendParameterEvents( TreeMap<Integer, MidiEventQueue> list, VsqBPList cle, int parameter_index, int clock_start, int clock_end ) {
            int max = cle.getMaximum();
            int min = cle.getMinimum();
            float order = 1.0f / (float)(max - min);
            if ( cle != null ) {
                int keycount = cle.size();
                for ( int i = 0; i < keycount; i++ ) {
                    int clock = cle.getKeyClock( i );
                    if ( clock_start <= clock && clock <= clock_end ) {
                        int value = cle.getElementA( i );
                        MidiEventQueue queue = null;
                        if ( list.containsKey( clock ) ) {
                            queue = list.get( clock );
                        } else {
                            queue = new MidiEventQueue();
                        }
                        if ( queue.param == null ) {
                            queue.param = new Vector<ParameterEvent>();
                        }
                        ParameterEvent pe = new ParameterEvent();
                        pe.index = parameter_index;
                        pe.value = (value - min) * order;
                        queue.param.add( pe );
                        list.put( clock, queue );
                    } else if ( clock_end < clock ) {
                        break;
                    }
                }
            }
        }

        private static MidiEvent getPitMidiEvent( int pitch_bend ) {
            int value = (0x3fff & (pitch_bend + 0x2000));
            byte msb = (byte)(value >> 7);
            byte lsb = (byte)(value - (msb << 7));
            MidiEvent pbs0 = new MidiEvent();
            pbs0.firstByte = 0xE0;
            pbs0.data = new byte[] { lsb, msb };
            return pbs0;
        }

        public override double getElapsedSeconds() {
            return 0.0;
        }

        public override double computeRemainingSeconds() {
            return 0.0;
        }

        public override double getProgress() {
            return 0.0;
        }
    }

}
#endif
