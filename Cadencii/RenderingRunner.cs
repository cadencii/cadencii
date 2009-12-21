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

import java.util.*;
import org.kbinani.*;
import org.kbinani.media.*;
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
    public abstract class RenderingRunner implements Runnable {
#else
    public abstract class RenderingRunner : Runnable {
#endif
        /*private int sampleRate;
        private int trimMilliSec;
        private long totalSamples;
        private WaveWriter waveWriter;
        private double waveReadOffsetSeconds;
        private Vector<WaveReader> readers;
        private boolean directPlay;
        private boolean reflectAmp2Wave;*/
        protected Object m_locker = null;
        protected boolean m_rendering = false;
        protected long totalSamples = 0;
        /// <summary>
        /// WaveIncomingで追加されたサンプル数
        /// </summary>
        protected long m_total_append = 0;
        protected int m_trim_remain = 0;
        protected boolean m_abort_required = false;

        protected int renderingTrack = 0;
        /// <summary>
        /// wave_writerに，Feder値を反映させた波形を出力するかどうか．
        /// menuFileExportWaveで出力した場合はtrue，そのほかの場合はキャッシュに書き込むので，falseにしておく．
        /// </summary>
        protected boolean reflectAmp2Wave;
        protected WaveWriter waveWriter;
        protected double waveReadOffsetSeconds;
        protected Vector<WaveReader> readers;
        protected boolean directPlay;
        protected int trimMillisec;
        protected int sampleRate;

        public abstract void run();
        public abstract double getProgress();
        public abstract double getElapsedSeconds();
        public abstract double computeRemainingSeconds();

        protected RenderingRunner( 
            int track,
            boolean reflect_amp_to_wave,
            WaveWriter wave_writer,
            double wave_read_offset_seconds,
            Vector<WaveReader> readers,
            boolean direct_play,
            int trim_msec,
            long total_samples,
            int sample_rate
        ) {
            renderingTrack = track;
            reflectAmp2Wave = reflect_amp_to_wave;
            waveWriter = wave_writer;
            waveReadOffsetSeconds = wave_read_offset_seconds;
            this.readers = readers;
            directPlay = direct_play;
            trimMillisec = trim_msec;
            totalSamples = total_samples;
            sampleRate = sample_rate;

            m_locker = new Object();
            m_rendering = false;
            m_total_append = 0;
            m_trim_remain = (int)(trimMillisec / 1000.0 * sampleRate); //先頭から省かなければならないサンプル数の残り
        }

        public virtual boolean isRendering() {
            return m_rendering;
        }

        public virtual void abortRendering() {
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

        protected void waveIncoming( double[] t_L, double[] t_R ) {
            if ( !m_rendering ) {
                return;
            }
            lock ( m_locker ) {
#if DEBUG
                //PortUtil.println( "RenderingRunner_DRAFT#waveIncoming; length=" + t_L.Length );
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
                if ( length > totalSamples - m_total_append ) {
                    length = (int)(totalSamples - m_total_append);
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

                AmplifyCoefficient amplify = AppManager.getAmplifyCoeffNormalTrack( renderingTrack );
                if ( reflectAmp2Wave ) {
                    for ( int i = 0; i < length; i++ ) {
                        if ( m_abort_required ) return;
                        if ( i % 100 == 0 ) {
                            amplify = AppManager.getAmplifyCoeffNormalTrack( renderingTrack );
                        }
                        L[i] = L[i] * amplify.left;
                        R[i] = R[i] * amplify.right;
                    }
                    if ( waveWriter != null ) {
                        try {
                            waveWriter.append( L, R );
                        } catch ( Exception ex ) {
                            PortUtil.println( "RenderingRunner_DRAFT#waveIncoming; ex=" + ex );
                        }
                    }
                } else {
                    if ( waveWriter != null ) {
                        try {
                            waveWriter.append( L, R );
                        } catch ( Exception ex ) {
                            PortUtil.println( "RenderingRunner_DRAFT#waveIncoming; ex=" + ex );
                        }
                    }
                    for ( int i = 0; i < length; i++ ) {
                        if ( m_abort_required ) return;
                        if ( i % 100 == 0 ) {
                            amplify = AppManager.getAmplifyCoeffNormalTrack( renderingTrack );
                        }
                        L[i] = L[i] * amplify.left;
                        R[i] = R[i] * amplify.right;
                    }
                }
                long start = m_total_append + (long)(waveReadOffsetSeconds * VSTiProxy.SAMPLE_RATE);
#if DEBUG
                //PortUtil.println( "RenderingRunner_DRAFT#waveIncoming; start=" + start );
#endif
                int count = readers.size();
                double[] reader_r = new double[length];
                double[] reader_l = new double[length];
                for ( int i = 0; i < count; i++ ) {
                    try {
                        WaveReader wr = readers.get( i );
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
                if ( directPlay ) {
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

#if !JAVA
}
#endif
