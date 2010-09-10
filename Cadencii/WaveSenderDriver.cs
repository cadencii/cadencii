/*
 * WaveSenderDriver.cs
 * Copyright (C) 2010 kbinani
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
#else
using org.kbinani.java.util;

namespace org.kbinani.cadencii.draft {
#endif

    /// <summary>
    /// WaveSenderをWaveGeneratorとして使うためのドライバー．
    /// WaveSenderは受動的波形生成器なので，自分では波形を作らない．
    /// </summary>
#if JAVA
    public class WaveSenderDriver extends WaveUnit implements WaveGenerator {
#else
    public class WaveSenderDriver : WaveUnit, WaveGenerator {
#endif
        private const int _BUFLEN = 1024;
        private WaveSender mWaveSender = null;
        private double[] mBufferL = new double[_BUFLEN];
        private double[] mBufferR = new double[_BUFLEN];
        private long mPosition = 0;
        private WaveReceiver mReceiver = null;
        private int mVersion = 0;

        public override int getVersion() {
            return mVersion;
        }

        public override void setConfig( string parameters ) {
            // do nothing
        }

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="track"></param>
        /// <param name="start_clock"></param>
        /// <param name="end_clock"></param>
        public void init( VsqFileEx vsq, int track, int start_clock, int end_clock ) {
            // do nothing (!!)
        }

        public void setSender( WaveSender wave_sender ) {
            mWaveSender = wave_sender;
        }

        public void setReceiver( WaveReceiver r ) {
            if ( mReceiver != null ) {
                mReceiver.end();
            }
            mReceiver = r;
        }

        public long getPosition() {
            return mPosition;
        }

        public void begin( long length ) {
            long remain = length;
            while ( remain > 0 ) {
                int amount = (remain > _BUFLEN) ? _BUFLEN : (int)remain;
                mWaveSender.pull( mBufferL, mBufferR, amount );
                mReceiver.push( mBufferL, mBufferR, amount );
                remain -= amount;
                mPosition += amount;
            }
            mReceiver.end();
        }
    }

#if !JAVA
}
#endif
