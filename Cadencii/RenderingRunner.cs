/*
 * RenderingRunner.cs
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
#if JAVA
package org.kbinani.cadencii;

import java.util.*;
import org.kbinani.*;
import org.kbinani.media.*;
#else
using System;
using org.kbinani.java.util;
using org.kbinani.media;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

#if JAVA
    public abstract class RenderingRunner implements Runnable {
#else
    public abstract class RenderingRunner : Runnable {
#endif
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
        protected Vector<WaveRateConverter> readers;
        protected boolean directPlay;
        protected int trimMillisec;
        protected int sampleRate;

        private const int BUFLEN = 1024;
        private double[] mBufferL = new double[BUFLEN];
        private double[] mBufferR = new double[BUFLEN];
        private double[] mBufferL2 = new double[BUFLEN];
        private double[] mBufferR2 = new double[BUFLEN];

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
            int numReaders = (readers != null) ? readers.size() : 0;
            this.readers = new Vector<WaveRateConverter>();
            for ( int i = 0; i < numReaders; i++ ) {
                this.readers.add( new WaveRateConverter( readers.get( i ), sample_rate ) );
            }
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
#if DEBUG
            PortUtil.println( "RenderingRunner#abrotRendering; enter" );
#endif
            m_abort_required = true;
            while ( m_rendering ) {
#if JAVA
                try{
                    Thread.sleep( 0 );
                }catch( InterruptedException ex ){
                    break;
                }
#else
                System.Windows.Forms.Application.DoEvents();
#endif
            }
            int count = readers.size();
            for ( int i = 0; i < count; i++ ) {
                try {
                    readers.get( i ).close();
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "RenderingRunner#abortRendering; ex=" + ex );
                }
                readers.set( i, null );
            }
            readers.clear();
        }

        protected void waveIncoming( double[] left, double[] right, int length ) {
            if ( !m_rendering ) {
                return;
            }

            if ( left == null || right == null ) {
                return;
            }

            int remain = length;
            int offset = 0;

            // トリムする分を省く
            if ( m_trim_remain > 0 ) {
                int amount = remain > m_trim_remain ? m_trim_remain : remain;
                m_trim_remain -= amount;
                offset += amount;
                remain -= amount;

                // トリム分を削りきれてない場合は終了
                if ( m_trim_remain > 0 ) {
                    return;
                }
            }

            // 
            boolean mixall = AppManager.editorConfig.WaveFileOutputFromMasterTrack;

            // BUFLEN単位で処理する
            while ( remain > 0 && !m_abort_required ) {
                // このループ内で処理するサンプル数を設定
                int amount = remain > BUFLEN ? BUFLEN : remain;
                
                // amountサンプル分をバッファにコピーしてくる
                for ( int i = 0; i < amount; i++ ) {
                    mBufferL[i] = left[offset + i];
                    mBufferR[i] = right[offset + i];
                }
                
                // 最初の増幅係数を取得
                AmplifyCoefficient amplify = AppManager.getAmplifyCoeffNormalTrack( renderingTrack );
                
                // 増幅係数を設定
                if ( reflectAmp2Wave ) {
                    // WAVEファイルに増幅係数を反映させる場合
                    // まず増幅係数をかける
                    for ( int i = 0; i < amount; i++ ) {
                        if ( m_abort_required ) return;
                        if ( i % 100 == 0 ) {
                            amplify = AppManager.getAmplifyCoeffNormalTrack( renderingTrack );
                        }
                        mBufferL[i] *= amplify.mLeft;
                        mBufferR[i] *= amplify.mRight;
                    }
                    
                    // 次にWAVEに書き込む
                    if ( !mixall && waveWriter != null ) {
                        try {
                            waveWriter.append( mBufferL, mBufferR, amount );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "RenderingRunner#waveIncoming; ex=" + ex );
                        }
                    }
                } else {
                    // WAVEファイルに増幅係数を反映させない場合
                    // まずWAVEに書き込む
                    if ( !mixall && waveWriter != null ) {
                        try {
                            waveWriter.append( mBufferL, mBufferR, amount );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "RenderingRunner#waveIncoming; ex=" + ex );
                        }
                    }
                    
                    // 次にWAVEに書き込む
                    for ( int i = 0; i < amount; i++ ) {
                        if ( m_abort_required ) return;
                        if ( i % 100 == 0 ) {
                            amplify = AppManager.getAmplifyCoeffNormalTrack( renderingTrack );
                        }
                        mBufferL[i] *= amplify.mLeft;
                        mBufferR[i] *= amplify.mRight;
                    }
                }

                // WAVEファイルからの読込み
                long start = m_total_append + (long)(waveReadOffsetSeconds * VSTiProxy.SAMPLE_RATE);
                int count = readers.size();
                // 合計count個のWAVEファイルから読込みを行う
                for ( int i = 0; i < count; i++ ) {
                    try {
                        WaveRateConverter wr = readers.get( i );
                        amplify.mLeft = 1.0;
                        amplify.mRight = 1.0;
                        Object tag = wr.getTag();
                        if ( tag == null ) {
                            continue;
                        }
                        if ( !(tag is Integer) ) {
                            continue;
                        }
                        int track = (Integer)tag;
                        if ( mixall && 0 > track ) {
                            // 全部mixするモードの時は、最初に他のトラックだけ読み込むので
                            continue;
                        }
                        if ( 0 < track ) {
                            amplify = AppManager.getAmplifyCoeffNormalTrack( track );
                        } else if ( 0 > track ) {
                            amplify = AppManager.getAmplifyCoeffBgm( -track - 1 );
                        }
                        wr.read( start, amount, mBufferL2, mBufferR2 );
                        for ( int j = 0; j < amount; j++ ) {
                            if ( m_abort_required ) return;
                            mBufferL[j] += mBufferL2[j] * amplify.mLeft;
                            mBufferR[j] += mBufferR2[j] * amplify.mRight;
                        }
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "RenderingRunner_DRAFT#waveIncoming; ex=" + ex );
                    }
                }

                // 全部ミックスする場合
                if ( mixall ) {
                    // まずWAVEに出力しておく
                    if ( waveWriter != null ) {
                        try {
                            waveWriter.append( mBufferL, mBufferR, amount );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "RenderingRunner#waveIncoming; ex=" + ex );
                        }
                    }

                    // WAVE読込み
                    for ( int i = 0; i < count; i++ ) {
                        try {
                            WaveRateConverter wr = readers.get( i );
                            Object tag = wr.getTag();
                            if ( tag == null ) {
                                continue;
                            }
                            if ( !(tag is Integer) ) {
                                continue;
                            }
                            int track = (Integer)tag;
                            if ( 0 < track ) {
                                // 全部mixするモードの時は、すでに他のトラックのはmix済みなので
                                continue;
                            }
                            amplify = AppManager.getAmplifyCoeffBgm( -track - 1 );
                            wr.read( start, amount, mBufferL2, mBufferR2 );
                            for ( int j = 0; j < amount; j++ ) {
                                if ( m_abort_required ) return;
                                mBufferL[j] += mBufferL2[j] * amplify.mLeft;
                                mBufferR[j] += mBufferR2[j] * amplify.mRight;
                            }
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "RenderingRunner#waveIncoming; ex=" + ex );
                        }
                    }
                }

                // すぐ再生する場合はプレーヤーに波形をプッシュ
                if ( directPlay ) {
                    PlaySound.append( mBufferL, mBufferR, amount );
                }
                m_total_append += amount;

                // 次のループに備える
                remain -= amount;
                offset += amount;
            }
        }
    }

#if !JAVA
}
#endif
