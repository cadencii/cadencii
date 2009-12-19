/*
 * RenderingRunner.cs
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
#if JAVA
package org.kbinani.cadencii;
#else
using System;
using org.kbinani.media;
using bocoree;
using bocoree.java.util;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

#if JAVA
    public interface RenderingRunner extends Runnable {
#else
    public interface RenderingRunner_OBSOLUTE : Runnable {
#endif
        //void run();
        double getProgress();
        void abortRendering();
        boolean isRendering();
        double getElapsedSeconds();
        double computeRemainingSeconds();
    }

#if !JAVA

    public abstract class RenderingRunner_DRAFT : Runnable {
        protected Object m_locker = null;
        protected boolean m_rendering = false;
        protected long m_total_samples = 0;
        protected long m_total_append = 0;
        protected int m_trim_remain = 0;
        protected boolean m_abort_required = false;

        protected int m_rendering_track = 0;
        protected boolean m_reflect_amp_to_wave;
        protected WaveWriter m_wave_writer;
        protected double m_wave_read_offset_seconds;
        protected Vector<WaveReader> m_reader;
        protected boolean m_direct_play;
        protected int m_trim_msec;
        protected int m_sample_rate;

        public abstract void run();
        public abstract double getProgress();
        public abstract void abortRendering();
        public abstract boolean isRendering();
        public abstract double getElapsedSeconds();
        public abstract double computeRemainingSeconds();

        protected RenderingRunner_DRAFT( 
            int track,
            boolean reflect_amp_to_wave,
            WaveWriter wave_writer,
            double wave_read_offset_seconds,
            Vector<WaveReader> readers,
            boolean direct_play,
            int trim_msec,
            int sample_rate
        ) {
            m_rendering_track = track;
            m_reflect_amp_to_wave = reflect_amp_to_wave;
            m_wave_writer = wave_writer;
            m_wave_read_offset_seconds = wave_read_offset_seconds;
            m_reader = readers;
            m_direct_play = direct_play;
            m_trim_msec = trim_msec;
            m_sample_rate = sample_rate;

            m_locker = new Object();
            m_rendering = false;
            m_total_samples = 0;
            m_total_append = 0;
            m_trim_remain = (int)(m_trim_msec / 1000.0 * m_sample_rate); //先頭から省かなければならないサンプル数の残り
        }

        protected void waveIncoming( double[] t_L, double[] t_R ) {
            if ( !m_rendering ) {
                return;
            }
            lock ( m_locker ) {
#if DEBUG
                PortUtil.println( "RenderingRunner_DRAFT#waveIncoming; length=" + t_L.Length );
#endif

                double[] L = t_L;
                double[] R = t_R;
                if ( m_trim_remain > 0 ) {
                    if ( L.Length <= m_trim_remain ) {
                        m_trim_remain -= L.Length;
                        return;
                    } else {
                        L = new double[t_L.Length - m_trim_remain];
                        R = new double[t_L.Length - m_trim_remain];
                        for ( int i = m_trim_remain; i < t_L.Length; i++ ) {
                            if ( m_abort_required ) return;
                            L[i - m_trim_remain] = t_L[i];
                            R[i - m_trim_remain] = t_R[i];
                        }
                        m_trim_remain = 0;
                    }
                }
                int length = L.Length;
                if ( length > m_total_samples - m_total_append ) {
                    length = (int)(m_total_samples - m_total_append);
                    if ( length <= 0 ) {
                        return;
                    }
                    double[] br = R;
                    double[] bl = L;
                    L = new double[length];
                    R = new double[length];
                    for ( int i = 0; i < length; i++ ) {
                        if ( m_abort_required ) return;
                        L[i] = bl[i];
                        R[i] = br[i];
                    }
                    br = null;
                    bl = null;
                }

                AmplifyCoefficient amplify = AppManager.getAmplifyCoeffNormalTrack( m_rendering_track );
                if ( m_reflect_amp_to_wave ) {
                    for ( int i = 0; i < length; i++ ) {
                        if ( m_abort_required ) return;
                        if ( i % 100 == 0 ) {
                            amplify = AppManager.getAmplifyCoeffNormalTrack( m_rendering_track );
                        }
                        L[i] = L[i] * amplify.left;
                        R[i] = R[i] * amplify.right;
                    }
                    if ( m_wave_writer != null ) {
                        try {
                            m_wave_writer.append( L, R );
                        } catch ( Exception ex ) {
                            PortUtil.println( "RenderingRunner_DRAFT#waveIncoming; ex=" + ex );
                        }
                    }
                } else {
                    if ( m_wave_writer != null ) {
                        try {
                            m_wave_writer.append( L, R );
                        } catch ( Exception ex ) {
                            PortUtil.println( "RenderingRunner_DRAFT#waveIncoming; ex=" + ex );
                        }
                    }
                    for ( int i = 0; i < length; i++ ) {
                        if ( m_abort_required ) return;
                        if ( i % 100 == 0 ) {
                            amplify = AppManager.getAmplifyCoeffNormalTrack( m_rendering_track );
                        }
                        L[i] = L[i] * amplify.left;
                        R[i] = R[i] * amplify.right;
                    }
                }
                long start = m_total_append + (long)(m_wave_read_offset_seconds * VSTiProxy.SAMPLE_RATE);
#if DEBUG
                PortUtil.println( "RenderingRunner_DRAFT#waveIncoming; start=" + start );
#endif
                int count = m_reader.size();
                double[] reader_r = new double[length];
                double[] reader_l = new double[length];
                for ( int i = 0; i < count; i++ ) {
                    try {
                        WaveReader wr = m_reader.get( i );
                        amplify.left = 1.0;
                        amplify.right = 1.0;
                        if ( wr.getTag() != null && wr.getTag() is Integer ) {
                            int track = (Integer)wr.getTag();
                            if ( 0 < track ) {
                                amplify = AppManager.getAmplifyCoeffNormalTrack( track );
                            } else if ( 0 > track ) {
                                amplify = AppManager.getAmplifyCoeffBgm( -track - 1 );
                            }
                        }
                        wr.read( start, length, reader_l, reader_r );
                        for ( int j = 0; j < length; j++ ) {
                            if ( m_abort_required ) return;
                            L[j] += reader_l[j] * amplify.left;
                            R[j] += reader_r[j] * amplify.right;
                        }
                    } catch ( Exception ex ) {
                        PortUtil.println( "RenderingRunner_DRAFT#waveIncoming; ex=" + ex );
                    }
                }
                reader_l = null;
                reader_r = null;
                if ( m_direct_play ) {
                    PlaySound.append( L, R, L.Length );
                }
                m_total_append += length;
                for ( int i = 0; i < t_L.Length; i++ ) {
                    t_L[i] = 0.0;
                    t_R[i] = 0.0;
                }
            }
        }
    }
}
#endif
