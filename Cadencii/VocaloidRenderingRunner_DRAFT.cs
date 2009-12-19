#if ENABLE_VOCALOID
/*
 * VocaloRenderingRunner.cs
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
using System.Collections.Generic;
using System.Windows.Forms;
using org.kbinani.vsq;
using org.kbinani.media;
using bocoree;
using bocoree.java.util;
using bocoree.java.io;

namespace org.kbinani.cadencii {
    using boolean = Boolean;

    public class VocaloidRenderingRunner_DRAFT : RenderingRunner_DRAFT {
        public String renderer;
        public NrpnData[] nrpn;
        public TempoTableEntry[] tempo;
        //public double amplify_left;
        //public double amplify_right;
        public int trim_msec;
        public long total_samples;
        public double wave_read_offset_seconds;
        public boolean mode_infinite;
        public VocaloidDriver driver;
        public boolean direct_play;
        public WaveWriter wave_writer;

        //private Vector<WaveReader> m_reader = new Vector<WaveReader>();
        //private Object m_locker;
        private int m_trim_remain;
        //private boolean m_rendering;
        //private long m_total_append = 0;
        //private int m_rendering_track = 1;
        /// <summary>
        /// wave_writerに，Feder値を反映させた波形を出力するかどうか．
        /// menuFileExportWaveで出力した場合はtrue，そのほかの場合はキャッシュに書き込むので，falseにしておく．
        /// </summary>
        //private bool m_reflect_amp_to_wave = false;
        private double m_started_date;

        public VocaloidRenderingRunner_DRAFT( String renderer_,
                                NrpnData[] nrpn_,
                                TempoTableEntry[] tempo_,
                                //double amplify_left_,
                                //double amplify_right_,
                                int trim_msec_,
                                long total_samples_,
                                double wave_read_offset_seconds_,
                                boolean mode_infinite_,
                                VocaloidDriver driver_,
                                boolean direct_play_,
                                WaveWriter wave_writer_,
                                Vector<WaveReader> reader_,
                                int rendering_track,
                                boolean reflect_amp_to_wave ) : base( rendering_track, reflect_amp_to_wave, wave_writer_, wave_read_offset_seconds_, reader_, direct_play_ ) {
            m_locker = new Object();
            renderer = renderer_;
            nrpn = nrpn_;
            tempo = tempo_;
            //amplify_left = amplify_left_;
            //amplify_right = amplify_right_;
            trim_msec = trim_msec_;
            total_samples = total_samples_;
            wave_read_offset_seconds = wave_read_offset_seconds_;
            mode_infinite = mode_infinite_;
            driver = driver_;
            direct_play = direct_play_;
            wave_writer = wave_writer_;
            m_reader = reader_;
            m_rendering_track = rendering_track;
            m_reflect_amp_to_wave = reflect_amp_to_wave;
        }

        public override double getElapsedSeconds() {
            return PortUtil.getCurrentTime() - m_started_date;
        }

        public override double computeRemainingSeconds() {
            double elapsed = getElapsedSeconds();
            double running_rate = 1;
            if ( driver != null && driver != null ) {
                try {
                    running_rate = driver.GetProgress() / elapsed;
                } catch ( Exception ex ) {
                }
            }
            double estimated = 100.0 / running_rate;
            double draft = estimated - elapsed;
            if ( draft < 0 ) {
                draft = 0;
            }
            return draft;
        }

        public override void abortRendering() {
            if ( driver != null && driver != null ) {
                try {
                    driver.AbortRendering();
                } catch {
                }
            }
            m_rendering = false;
            if ( m_reader != null && m_reader.size() > 0 ) {
                for ( int i = 0; i < m_reader.size(); i++ ) {
                    m_reader.get( i ).close();
                    m_reader.set( i, null );
                }
                m_reader.clear();
            }
        }

        public override double getProgress() {
            return driver.GetProgress();
        }

        public override boolean isRendering() {
            return m_rendering;
        }

        public override void run() {
#if DEBUG
            PortUtil.println( "VocaloRenderingRunner#run" );
#endif
            m_started_date = PortUtil.getCurrentTime();
            if ( driver == null ) {
#if DEBUG
                PortUtil.println( "VocaloRenderingRunner#run; error: driver is null" );
#endif
                return;
            }
            if ( !driver.loaded ) {
#if DEBUG
                PortUtil.println( "VocaloRenderingRunner#run; error: driver not loaded" );
#endif
                return;
            }
            int tempo_count = tempo.Length;
            float first_tempo = 125.0f;
            if ( tempo.Length > 0 ) {
                first_tempo = (float)(60e6 / (double)tempo[0].Tempo);
            }
            byte[] masterEventsSrc = new byte[tempo_count * 3];
            int[] masterClocksSrc = new int[tempo_count];
            int count = -3;
            for ( int i = 0; i < tempo.Length; i++ ) {
                count += 3;
                masterClocksSrc[i] = tempo[i].Clock;
                byte b0 = (byte)(tempo[i].Tempo >> 16);
                uint u0 = (uint)(tempo[i].Tempo - (b0 << 16));
                byte b1 = (byte)(u0 >> 8);
                byte b2 = (byte)(u0 - (u0 << 8));
                masterEventsSrc[count] = b0;
                masterEventsSrc[count + 1] = b1;
                masterEventsSrc[count + 2] = b2;
            }
            driver.SendEvent( masterEventsSrc, masterClocksSrc, 0 );

            int numEvents = nrpn.Length;
            byte[] bodyEventsSrc = new byte[numEvents * 3];
            int[] bodyClocksSrc = new int[numEvents];
            count = -3;
            int last_clock = 0;
            for ( int i = 0; i < numEvents; i++ ) {
                count += 3;
                bodyEventsSrc[count] = 0xb0;
                bodyEventsSrc[count + 1] = nrpn[i].getParameter();
                bodyEventsSrc[count + 2] = nrpn[i].Value;
                bodyClocksSrc[i] = nrpn[i].getClock();
                last_clock = nrpn[i].getClock();
            }

            int index = tempo_count - 1;
            for ( int i = tempo_count - 1; i >= 0; i-- ) {
                if ( tempo[i].Clock < last_clock ) {
                    index = i;
                    break;
                }
            }
            int last_tempo = tempo[index].Tempo;
            int trim_remain = VSTiProxy.getErrorSamples( first_tempo ) + (int)(trim_msec / 1000.0 * VSTiProxy.SAMPLE_RATE);
            m_trim_remain = trim_remain;

            driver.SendEvent( bodyEventsSrc, bodyClocksSrc, 1 );

            m_rendering = true;
            if ( wave_writer != null ) {
                if ( m_trim_remain < 0 ) {
                    double[] d = new double[-m_trim_remain];
                    for ( int i = 0; i < -m_trim_remain; i++ ) {
                        d[i] = 0.0;
                    }
                    wave_writer.append( d, d );
                    m_trim_remain = 0;
                }
            }

            driver.WaveIncoming += waveIncoming;
            driver.RenderingFinished += vstidrv_RenderingFinished;
            driver.StartRendering( total_samples, mode_infinite );
            while ( m_rendering ) {
                Application.DoEvents();
            }
            m_rendering = false;
            driver.WaveIncoming -= waveIncoming;
            driver.RenderingFinished -= vstidrv_RenderingFinished;
            if ( direct_play ) {
                PlaySound.waitForExit();
            }
        }

        private void vstidrv_RenderingFinished( Object sendre, EventArgs e ) {
            m_rendering = false;
        }
    }

}
#endif
