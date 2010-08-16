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

namespace org.kbinani.cadencii {
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
        private WaveSender _wave_sender = null;
        private double[] _buffer_l = new double[_BUFLEN];
        private double[] _buffer_r = new double[_BUFLEN];
        private long _position = 0;
        private WaveReceiver _receiver = null;
        private int _version = 0;

        public override int getVersion() {
            return _version;
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
            _wave_sender = wave_sender;
        }

        public void setReceiver( WaveReceiver r ) {
            if ( _receiver != null ) {
                _receiver.end();
            }
            _receiver = r;
        }

        public long getPosition() {
            return _position;
        }

        public void begin( long length ) {
            long remain = length;
            while ( remain > 0 ) {
                int amount = (remain > _BUFLEN) ? _BUFLEN : (int)remain;
                _wave_sender.pull( _buffer_l, _buffer_r, amount );
                _receiver.push( _buffer_l, _buffer_r, amount );
                remain -= amount;
                _position += amount;
            }
            _receiver.end();
        }
    }

#if !JAVA
}
#endif
