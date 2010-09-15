#if ENABLE_VOCALOID
/*
 * VocaloidRenderingRunner.cs
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
using System.Windows.Forms;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.media;
using org.kbinani.vsq;

namespace org.kbinani.cadencii {
    using boolean = Boolean;

    public class VocaloidRenderingRunner : RenderingRunner, IWaveIncoming {
        public NrpnData[] nrpn;
        public TempoVector tempo;
        public boolean mode_infinite;
        public VocaloidDriver driver;

        private double m_started_date;
        /// <summary>
        /// プリセンドタイム（ミリ秒）。NRPNが作成されたときに指定したのと同じ値である必要がある。
        /// </summary>
        private int msPresend = 0;

        public VocaloidRenderingRunner( NrpnData[] nrpn_,
                                        TempoVector tempo_,
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
                                        int sample_rate,
                                        int ms_presend )
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
#if DEBUG
            PortUtil.println( "VocaloidRenderingRunner#.ctor; reader_.size()=" + reader_.size() );
#endif
            msPresend = ms_presend;
            nrpn = nrpn_;
            tempo = tempo_;
            mode_infinite = mode_infinite_;
            driver = driver_;
            float first_tempo = 125.0f;
            if ( tempo.size() > 0 ) {
                first_tempo = (float)(60e6 / (double)tempo[0].Tempo);
            }
            int errorSamples = VSTiProxy.getErrorSamples( first_tempo );
            int trim_remain = errorSamples + (int)(trim_msec_ / 1000.0 * VSTiProxy.SAMPLE_RATE);
            m_trim_remain = trim_remain;
            totalSamples = total_samples_ + errorSamples;
        }

        public void waveIncomingImpl( double[] t_L, double[] t_R, int length ) {
            waveIncoming( t_L, t_R, length );
        }

        public override double getElapsedSeconds() {
            return PortUtil.getCurrentTime() - m_started_date;
        }

        public override double computeRemainingSeconds() {
            double elapsed = getElapsedSeconds();
            double running_rate = 1;
            if ( driver != null && driver != null ) {
                try {
                    running_rate = driver.getProgress() / elapsed;
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
#if DEBUG
            PortUtil.println( "VocaloidRenderingRunner#abortRendering; enter" );
#endif
            if ( driver != null && driver.isRendering() ) {
                try {
                    driver.abortRendering();
                } catch( Exception ex ) {
                    PortUtil.stderr.println( "VocaloidRenderingRunner#run; ex=" + ex );
                }
                while ( driver.isRendering() ) {
                    System.Windows.Forms.Application.DoEvents();
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
            return driver.getProgress();
        }

        public override void run() {
#if DEBUG
            PortUtil.println( "VocaloidRenderingRunner#run; enter" );
#endif
            m_started_date = PortUtil.getCurrentTime();
            m_abort_required = false;
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
            if ( driver.isRendering() ) {
                driver.abortRendering();
                while ( driver.isRendering() && !m_abort_required ) {
#if JAVA
                    Thread.sleep( 0 );
#else
                    System.Windows.Forms.Application.DoEvents();
#endif
                }
            }

            // 古いイベントをクリア
            driver.clearSendEvents();

            int tempo_count = tempo.size();
            float first_tempo = 125.0f;
            if ( tempo.size() > 0 ) {
                first_tempo = (float)(60e6 / (double)tempo[0].Tempo);
            }
            byte[] masterEventsSrc = new byte[tempo_count * 3];
            int[] masterClocksSrc = new int[tempo_count];
            int count = -3;
            for ( int i = 0; i < tempo.size(); i++ ) {
                count += 3;
                TempoTableEntry itemi = tempo.get( i );
                masterClocksSrc[i] = itemi.Clock;
                byte b0 = (byte)(itemi.Tempo >> 16);
                uint u0 = (uint)(itemi.Tempo - (b0 << 16));
                byte b1 = (byte)(u0 >> 8);
                byte b2 = (byte)(u0 - (u0 << 8));
                masterEventsSrc[count] = b0;
                masterEventsSrc[count + 1] = b1;
                masterEventsSrc[count + 2] = b2;
            }
            driver.sendEvent( masterEventsSrc, masterClocksSrc, 0 );
            //driver.setTempoTable( tempo );

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

            driver.sendEvent( bodyEventsSrc, bodyClocksSrc, 1 );

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

            driver.startRendering( totalSamples + m_trim_remain + (int)(msPresend / 1000.0 * sampleRate), mode_infinite, sampleRate , this );
            /*while ( driver.isRendering() ) {
                Application.DoEvents();
            }*/
            m_rendering = false;
            if ( directPlay ) {
                PlaySound.waitForExit();
            }
#if DEBUG
            PortUtil.println( "VocaloidRenderingRunner#run; done; m_total_append=" + m_total_append );
#endif
        }
    }

}
#endif
