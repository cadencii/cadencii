#if ENABLE_AQUESTONE
/*
 * AquesToneRenderingRunner.cs
 * Copyright (c) 2009 kbinani
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
using bocoree;
using bocoree.java.util;
using org.kbinani.vsq;
using org.kbinani.media;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using Integer = System.Int32;

#if JAVA
    public class AquesToneRenderingRunner extends RenderingRunner {
#else
    public class AquesToneRenderingRunner : RenderingRunner {
#endif
        private AquesToneDriver driver = null;
        private String tempDir;
        private boolean modeInfinite;
        private VsqFileEx vsq = null;

        private class MidiEventQueue {
            public Vector<MidiEvent> noteoff;
            public Vector<MidiEvent> singer;
            public Vector<MidiEvent> noteon;
            public Vector<MidiEvent> others;
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

            for ( Iterator itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                long saNoteStart = (long)(vsq.getSecFromClock( item.Clock ) * sampleRate);
                long saNoteEnd = (long)(vsq.getSecFromClock( item.Clock + item.ID.getLength() ) * sampleRate);

                TreeMap<Integer, MidiEventQueue> list = generateMidiEvent( vsq, renderingTrack, lastClock, item.Clock + item.ID.getLength() );
                lastClock = item.Clock + item.ID.Length;
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
                    if ( queue.noteoff != null ) {
                        driver.send( queue.noteoff.toArray( new MidiEvent[] { } ) );
                    }
                    // ついで歌手変更
                    if ( queue.singer != null ) {
                        for ( Iterator itr3 = queue.singer.iterator(); itr3.hasNext(); ) {
                            MidiEvent foo = (MidiEvent)itr3.next();
                            if ( foo.firstByte == 0xff ) {
                                driver.setParameter( driver.phontParameterIndex, foo.data[0] + 0.01f );
                            }
                        }
                    }
                    // ついでnoteon
                    if ( queue.noteon != null ) {
                        driver.send( queue.noteon.toArray( new MidiEvent[] { } ) );
                    }
                    // 最後にその他
                    if ( queue.others != null ) {
                        for ( Iterator itr3 = queue.others.iterator(); itr3.hasNext(); ) {
                            MidiEvent foo = (MidiEvent)itr3.next();
                            if ( foo.firstByte == 0xFE ) {
                                driver.setParameter( driver.pbsParameterIndex, foo.data[0] / 13.0f );
                                itr3.remove();
                            }
                        }
                        driver.send( queue.others.toArray( new MidiEvent[] { } ) );
                    }
                    if ( driver.pluginUi != null ) {
                        driver.pluginUi.invalidateUi();
                    }
                }
            }

            // totalSamplesに足りなかったら、追加してレンダリング
            saRemain = (int)(totalSamples - saProcessed);
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
        private static TreeMap<Integer, MidiEventQueue> generateMidiEvent( VsqFileEx vsq, int track, int clock_start, int clock_end ) {
            TreeMap<Integer, MidiEventQueue> list = new TreeMap<Integer, MidiEventQueue>();
            VsqTrack t = vsq.Track.get( track );

            // 歌手変更
            // 歌手変更はMIDIイベントではなく、VSTiに直接パラメータを送らねばならない。
            // ここでは、firstByte=0xff, data = new byte[]{ program# }のダミーMidiEventを作成する。
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
                    MidiEvent singer = new MidiEvent();
                    singer.firstByte = 0xff;
                    singer.data = new byte[] { (byte)program };
                    Vector<MidiEvent> add = Arrays.asList( new MidiEvent[] { singer } );
                    MidiEventQueue queue = null;
                    if ( list.containsKey( item.Clock ) ) {
                        queue = list.get( item.Clock );
                    } else {
                        queue = new MidiEventQueue();
                    }
                    if ( queue.singer == null ) {
                        queue.singer = new Vector<MidiEvent>();
                    }
                    queue.singer.addAll( add );
                    list.put( item.Clock, queue );
                } else if ( clock_end < item.Clock ) {
                    break;
                }
            }

            // ノートon, off
            for ( Iterator itr = t.getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( clock_start <= item.Clock && item.Clock <= clock_end ) {
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
                        // index行目に移動するコマンドを贈る
                        MidiEvent moveline = new MidiEvent();
                        moveline.firstByte = (byte)0xb0;
                        moveline.data = new byte[] { (byte)0x0a, (byte)index };
                        MidiEvent noteon = new MidiEvent();
                        noteon.firstByte = (byte)0x90;
                        noteon.data = new byte[] { (byte)item.ID.Note, (byte)item.ID.Dynamics };
                        Vector<MidiEvent> add = Arrays.asList( new MidiEvent[] { moveline, noteon } );
                        MidiEventQueue queue = null;
                        if ( list.containsKey( item.Clock ) ) {
                            queue = list.get( item.Clock );
                        }else{
                            queue = new MidiEventQueue();
                        }
                        if ( queue.noteon == null ) {
                            queue.noteon = new Vector<MidiEvent>();
                        }
                        queue.noteon.addAll( add );
                        list.put( item.Clock, queue );
                    }

                    // noteoff MIDIイベントを作成
                    MidiEvent noteoff = new MidiEvent();
                    noteoff.firstByte = (byte)0x80;
                    noteoff.data = new byte[] { (byte)item.ID.Note, 0x40 };
                    Vector<MidiEvent> a_noteoff = Arrays.asList( new MidiEvent[] { noteoff } );
                    int endclock = item.Clock + item.ID.getLength();
                    MidiEventQueue q = null;
                    if ( list.containsKey( endclock ) ) {
                        q = list.get( endclock );
                    }else{
                        q = new MidiEventQueue();
                    }
                    if ( q.noteoff == null ) {
                        q.noteoff = new Vector<MidiEvent>();
                    }
                    q.noteoff.addAll( a_noteoff );
                    list.put( endclock, q );
                } else if ( clock_end < item.Clock ) {
                    break;
                }
            }

            // pitch bend sensitivity
            // RPNで送信するのが上手くいかないので、firstByte=0xfeのダミーMidiEventを作る
            VsqBPList pbs = t.getCurve( "pbs" );
            if ( pbs != null ) {
                int keycount = pbs.size();
                for ( int i = 0; i < keycount; i++ ) {
                    int clock = pbs.getKeyClock( i );
                    if ( clock_start <= clock && clock <= clock_end ) {
                        int value = pbs.getElementA( i );
#if DEBUG
                        PortUtil.println( "AquesToneRenderingRunner#generateMidiEvent; pbs=" + value );
#endif
                        MidiEvent pbs0 = new MidiEvent();
                        pbs0.firstByte = 0xFE;
                        pbs0.data = new byte[] { (byte)value };
                        Vector<MidiEvent> add = Arrays.asList( new MidiEvent[] { pbs0 } );
                        MidiEventQueue queue = null;
                        if ( list.containsKey( clock ) ) {
                            queue = list.get( clock );
                        }else{
                            queue = new MidiEventQueue();
                        }
                        if ( queue.others == null ) {
                            queue.others = new Vector<MidiEvent>();
                        }
                        queue.others.addAll( add );
                        list.put( clock, queue );
                    } else if ( clock_end < clock ) {
                        break;
                    }
                }
            }

            // pitch bend
            VsqBPList pit = t.getCurve( "pit" );
            if ( pit != null ) {
                int keycount = pit.size();
                for ( int i = 0; i < keycount; i++ ) {
                    int clock = pit.getKeyClock( i );
                    if ( clock_start <= clock && clock <= clock_end ) {
                        int value = (0x3fff & (pit.getElementA( i ) + 0x2000));
#if DEBUG
                        PortUtil.println( "AquesToneRenderingRunner#generateMidiEvent; value=" + value );
#endif
                        byte msb = (byte)(value >> 7);
                        byte lsb = (byte)(value - (msb << 7));
                        MidiEvent pbs0 = new MidiEvent();
                        pbs0.firstByte = 0xE0;
                        pbs0.data = new byte[] { lsb, msb };
                        Vector<MidiEvent> add = Arrays.asList( new MidiEvent[] { pbs0 } );
                        MidiEventQueue queue = null;
                        if ( list.containsKey( clock ) ) {
                            queue = list.get( clock );
                        }else{
                            queue = new MidiEventQueue();
                        }
                        if ( queue.others == null ) {
                            queue.others = new Vector<MidiEvent>();
                        }
                        queue.others.addAll( add );
                        list.put( clock, queue );
                    } else if ( clock_end < clock ) {
                        break;
                    }
                }
            }

            return list;
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
