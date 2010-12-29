/*
 * SamplingFrequencyConverter.cs
 * Copyright © 2010 kbinani
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

import java.awt.*;
import java.util.*;
#else
using System;
using org.kbinani.media;
using org.kbinani.java.util;
using org.kbinani.java.awt;

namespace org.kbinani.cadencii
{
#endif

    /// <summary>
    /// サンプリング周波数変換器
    /// </summary>
#if JAVA
    public class SamplingFrequencyConverter extends WaveUnit implements WaveSender, WaveReceiver {
#else
    public class SamplingFrequencyConverter : WaveUnit, WaveSender, WaveReceiver, IWaveReceiver
    {
#endif
        /// <summary>
        /// 動作モードを表す
        /// </summary>
        enum Status
        {
            /// <summary>
            /// 不明
            /// </summary>
            UNKNOWN,
            /// <summary>
            /// WaveReceiverモード
            /// </summary>
            PUSH,
            /// <summary>
            /// WaveSenderモード
            /// </summary>
            PULL,
        }

        private const int _BUFLEN = 1024;

        private Status mMode = Status.UNKNOWN;
        private double[] mBufferL = new double[_BUFLEN];
        private double[] mBufferR = new double[_BUFLEN];
        private long mPosition = 0;
        private WaveReceiver mReceiver = null;
        private WaveSender mSender = null;
        private int mVersion = 0;
        private BasicStroke mStroke = null;
        private int mRateFrom = 44100;
        private int mRateTo = 44100;
        private WaveRateConvertAdapter mAdapter = null;

        public SamplingFrequencyConverter( int rate_from, int rate_to )
        {
            mRateFrom = rate_from;
            mRateTo = rate_to;
            mAdapter = new WaveRateConvertAdapter( this, mRateFrom );
        }

        #region implementation of IWaveReceiver
        public void close()
        {
        }

        public int getSampleRate()
        {
            return mRateTo;
        }

        public void append( double[] left, double[] right, int length )
        {
            if ( mMode == Status.PUSH ) {
                mReceiver.push( left, right, length );
            } else if ( mMode == Status.PULL ) {

            }
        }
        #endregion

        public override int getVersion()
        {
            return mVersion;
        }

        public override void setConfig( String parameter )
        {
            // do nothing (ı _ ı )
        }

        public void setReceiver( WaveReceiver r )
        {
            if ( mReceiver != null ) {
                mReceiver.end();
            }
            mReceiver = r;
        }

        public void setSender( WaveSender s )
        {
            if ( mSender != null ) {
                mSender.end();
            }
            mSender = s;
        }

        public long getPosition()
        {
            return mPosition;
        }

        public void end()
        {
            if ( mReceiver != null ) {
                mReceiver.end();
            }
            if ( mSender != null ) {
                mSender.end();
            }
        }

        public void push( double[] l, double[] r, int length )
        {
            if ( mMode == Status.PULL ){
                return;
            }
            if ( mMode == Status.UNKNOWN ) {
                mMode = Status.PUSH;
            }
            mAdapter.append( l, r, length );
        }

        public void pull( double[] l, double[] r, int length )
        {
            if ( mMode == Status.PUSH ) {
                return;
            }
            if ( mMode == Status.UNKNOWN ) {
                mMode = Status.PULL;
            }
        }
    }

#if !JAVA
}
#endif
