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

                TreeMap<Integer, Vector<MidiEvent>> list = generateMidiEvent( vsq, renderingTrack, lastClock, item.Clock + item.ID.getLength() );
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
                    Vector<MidiEvent> append = list.get( clock );
                    // 歌手変更をまず処理してしまう
                    for ( Iterator itr3 = append.iterator(); itr3.hasNext(); ) {
                        MidiEvent foo = (MidiEvent)itr3.next();
                        if ( foo.firstByte == 0xff ) {
                            driver.setParameter( driver.phontParameterIndex, foo.data[0] + 0.01f );
                            itr3.remove();
                        }
                    }
                    // 歌手変更以外のイベントを送信
                    driver.send( append.toArray( new MidiEvent[] { } ) );
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
        private static TreeMap<Integer, Vector<MidiEvent>> generateMidiEvent( VsqFileEx vsq, int track, int clock_start, int clock_end ) {
            TreeMap<Integer, Vector<MidiEvent>> list = new TreeMap<Integer,Vector<MidiEvent>>();
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
                    if ( list.containsKey( item.Clock ) ) {
                        list.get( item.Clock ).add( singer );
                    } else {
                        list.put( item.Clock, Arrays.asList( new MidiEvent[] { singer } ) );
                    }
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
                        if ( list.containsKey( item.Clock ) ) {
                            list.get( item.Clock ).addAll( add );
                        } else {
                            list.put( item.Clock, add );
                        }
                    }

                    // noteoff MIDIイベントを作成
                    MidiEvent noteoff = new MidiEvent();
                    noteoff.firstByte = (byte)0x80;
                    noteoff.data = new byte[] { (byte)item.ID.Note, 0x40 };
                    int endclock = item.Clock + item.ID.getLength();
                    if ( list.containsKey( endclock ) ) {
                        list.get( endclock ).add( noteoff );
                    } else {
                        list.put( endclock, Arrays.asList( new MidiEvent[] { noteoff } ) );
                    }
                } else if ( clock_end < item.Clock ) {
                    break;
                }
            }

            // pitch bend sensitivity
            VsqBPList pbs = t.getCurve( "pbs" );
            if ( pbs != null ) {
                int keycount = pbs.size();
                for ( int i = 0; i < keycount; i++ ) {
                    int clock = pbs.getKeyClock( i );
                    if ( clock_start <= clock && clock <= clock_end ) {
                        int value = (int)(pbs.getElementA( i ) * 127.0 / 13.0);
#if DEBUG
                        PortUtil.println( "AquesToneRenderingRunner#generateMidiEvent; pbs=" + value );
#endif
                        MidiEvent pbs0 = new MidiEvent();
                        pbs0.firstByte = 0xB0;
                        pbs0.data = new byte[] { 0x64, 0x00 };
                        MidiEvent pbs1 = new MidiEvent();
                        pbs1.firstByte = 0xB0;
                        pbs1.data = new byte[] { 0x65, 0x00 };
                        MidiEvent pbs2 = new MidiEvent();
                        pbs2.firstByte = 0xB0;
                        pbs2.data = new byte[] { 0x06, (byte)value };
                        Vector<MidiEvent> add = Arrays.asList( new MidiEvent[] { pbs0, pbs1, pbs2 } );
                        if ( list.containsKey( clock ) ) {
                            list.get( clock ).addAll( add );
                        } else {
                            list.put( clock, add );
                        }
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
                        if ( list.containsKey( clock ) ) {
                            list.get( clock ).addAll( add );
                        } else {
                            list.put( clock, add );
                        }
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

        public override bool isRendering() {
            return m_rendering;
        }

        public override double computeRemainingSeconds() {
            return 0.0;
        }

        public override void abortRendering() {
            m_abort_required = true;
            while ( m_rendering ) {
#if JAVA
                Thread.sleep( 0 );
#else
                System.Windows.Forms.Application.DoEvents();
#endif
            }
            int count = readers.size();
            for ( int i = 0; i < count; i++ ) {
                try {
                    readers.get( i ).close();
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "AquesToneRenderingRunner#abortRendering; ex=" + ex );
                }
                readers.set( i, null );
            }
            readers.clear();
        }

        public override double getProgress() {
            return 0.0;
        }
    }

}
#endif
