#if ENABLE_VOCALOID
/*
 * VocaloidRenderingRunner.cs
 * Copyright (C) 2009 kbinani
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
using System.Windows.Forms;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.media;
using org.kbinani.vsq;

namespace org.kbinani.cadencii {
    using boolean = Boolean;

    public class VocaloidRenderingRunner : RenderingRunner {
        public String renderer;
        public NrpnData[] nrpn;
        public TempoTableEntry[] tempo;
        public boolean mode_infinite;
        public VocaloidDriver driver;

        private double m_started_date;

        public VocaloidRenderingRunner( String renderer_,
                                NrpnData[] nrpn_,
                                TempoTableEntry[] tempo_,
                                int trim_msec_,
                                long total_samples_,
                                double wave_read_offset_seconds_,
                                boolean mode_infinite_,
                                VocaloidDriver driver_,
                                boolean direct_play_,
                                WaveWriter wave_writer_,
                                Vector<WaveReader> reader_,
                                int rendering_track,
                                boolean reflect_amp_to_wave,
                                int sample_rate )
#if JAVA
        {
#else
            :
#endif
            base( rendering_track, reflect_amp_to_wave, wave_writer_, wave_read_offset_seconds_, reader_, direct_play_, trim_msec_, total_samples_, sample_rate )
#if JAVA
            ;
#else
        {
#endif
            renderer = renderer_;
            nrpn = nrpn_;
            tempo = tempo_;
            mode_infinite = mode_infinite_;
            driver = driver_;

            float first_tempo = 125.0f;
            if ( tempo.Length > 0 ) {
                first_tempo = (float)(60e6 / (double)tempo[0].Tempo);
            }
            int trim_remain = VSTiProxy.getErrorSamples( first_tempo ) + (int)(trim_msec_ / 1000.0 * VSTiProxy.SAMPLE_RATE);
            m_trim_remain = trim_remain;        
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
                } catch( Exception ex ) {
                    PortUtil.stderr.println( "VocaloidRenderingRunner#run; ex=" + ex );
                }
            }
            m_rendering = false;
            if ( readers != null && readers.size() > 0 ) {
                for ( int i = 0; i < readers.size(); i++ ) {
                    readers.get( i ).close();
                    readers.set( i, null );
                }
                readers.clear();
            }
        }

        public override double getProgress() {
            return driver.GetProgress();
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
            int trim_remain = VSTiProxy.getErrorSamples( first_tempo ) + (int)(trimMillisec / 1000.0 * VSTiProxy.SAMPLE_RATE);
            m_trim_remain = trim_remain;

            driver.SendEvent( bodyEventsSrc, bodyClocksSrc, 1 );

            m_rendering = true;
            if ( waveWriter != null ) {
                if ( m_trim_remain < 0 ) {
                    double[] d = new double[-m_trim_remain];
                    for ( int i = 0; i < -m_trim_remain; i++ ) {
                        d[i] = 0.0;
                    }
                    waveWriter.append( d, d );
                    m_trim_remain = 0;
                }
            }

            driver.WaveIncoming += waveIncoming;
            driver.RenderingFinished += vstidrv_RenderingFinished;
            driver.StartRendering( totalSamples, mode_infinite );
            while ( m_rendering ) {
                Application.DoEvents();
            }
            m_rendering = false;
            driver.WaveIncoming -= waveIncoming;
            driver.RenderingFinished -= vstidrv_RenderingFinished;
            if ( directPlay ) {
                PlaySound.waitForExit();
            }
        }

        private void vstidrv_RenderingFinished( Object sendre, EventArgs e ) {
            m_rendering = false;
        }
    }

}
#endif
