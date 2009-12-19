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
using bocoree.java.util;
using org.kbinani.vsq;
using org.kbinani.media;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;

    public class AquesToneRenderingRunner : RenderingRunner {
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
        ) : base( track, reflect_amp_to_wave, wave_writer, wave_read_offset_seconds, readers, direct_play, trim_msec, total_samples, sample_rate ) {
            this.vsq = vsq;
            this.driver = driver;
            tempDir = temp_dir;
            modeInfinite = mode_infinite;
        }

        public override void run() {
#if DEBUG
            Console.WriteLine( "AquesToneRenderingRunner#run" );
#endif
            m_rendering = true;
            m_abort_required = false;

            VsqTrack track = vsq.Track.get( renderingTrack );
            double sec_first_note = 0.0;
            for ( Iterator itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                sec_first_note = vsq.getSecFromClock( item.Clock );
                break;
            }

            int BUFLEN = sampleRate / 10;
            double[] left = new double[BUFLEN];
            double[] right = new double[BUFLEN];
            long saProcessed = 0; // これまでに合成したサンプル数
            int saRemain = 0;

            for ( Iterator itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                long saNoteStart = (long)(vsq.getSecFromClock( item.Clock ) * sampleRate);
                long saNoteEnd = (long)(vsq.getSecFromClock( item.Clock + item.ID.getLength() ) * sampleRate);
                saRemain = (int)(saNoteStart - saProcessed);
#if DEBUG
                Console.WriteLine( "AquesToneRenderingRunner#run; before note; saRemain=" + saRemain );
#endif

                // 前回のレンダリング部分から、音符の開始位置までを合成
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

                // 音符開始MIDIイベントを送信
                String lyric = item.ID.LyricHandle.L0.Phrase;
                String katakana = KanaDeRomanization.hiragana2katakana( lyric );
#if DEBUG
                Console.WriteLine( "AquesToneRenderingRunner#run; lyric=" + lyric + "; katakana=" + katakana );
#endif
                int index = -1;
                for ( int i = 0; i < AquesToneDriver.PHONES.Length; i++ ) {
                    if ( katakana.Equals( AquesToneDriver.PHONES[i] ) ) {
                        index = i;
                        break;
                    }
                }
#if DEBUG
                Console.WriteLine( "AquesToneRenderingRunner#run; index=" + index );
#endif
                if ( index >= 0 ) {
#if DEBUG
                    Console.WriteLine( "AquesToneRenderingRunner#run; send note on" );
#endif
                    // index行目に移動するコマンドを贈る
                    MidiEvent moveline = new MidiEvent();
                    moveline.firstByte = (byte)0xb0;
                    moveline.data = new byte[] { (byte)0x0a, (byte)index };
                    //driver.send( new MidiEvent[] { moveline } );
                    // note on
                    MidiEvent sing = new MidiEvent();
                    sing.firstByte = (byte)0x90;
                    sing.data = new byte[] { (byte)item.ID.Note, (byte)item.ID.Dynamics };
                    driver.send( new MidiEvent[] { moveline, sing } );
                }

                // 音符終了位置までをレンダリング
                saRemain = (int)(saNoteEnd - saNoteStart);
#if DEBUG
                Console.WriteLine( "AquesToneRenderingRunner#run; note; saRemain=" + saRemain );
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

                // note off
                MidiEvent noteoff = new MidiEvent();
                noteoff.firstByte = (byte)0x80;
                noteoff.data = new byte[] { (byte)item.ID.Note, 0x40 };
#if DEBUG
                Console.WriteLine( "AquesToneRenderingRunner#run; send note off" );
#endif
                driver.send( new MidiEvent[] { noteoff } );
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
        }

        public override double getProgress() {
            return 0.0;
        }
    }

}
#endif
