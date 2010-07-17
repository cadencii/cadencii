/*
 * PassiveWaveSenderDriver.cs
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

    public class PassiveWaveSenderDriver : ActiveWaveSender {
        private const int _BUFLEN = 1024;
        private PassiveWaveSender _wave_sender = null;
        private double[] _buffer_l = new double[_BUFLEN];
        private double[] _buffer_r = new double[_BUFLEN];
        private Vector<WaveReceiver> _wave_receivers = new Vector<WaveReceiver>();

        public PassiveWaveSenderDriver( PassiveWaveSender wave_sender ) {
            _wave_sender = wave_sender;
        }

        public void addReceiver( WaveReceiver r ) {
            if ( r == null ) {
                return;
            }
            if ( !_wave_receivers.contains( r ) ) {
                _wave_receivers.add( r );
            }
        }

        public void removeReceiver( WaveReceiver r ) {
            if ( r == null ) {
                return;
            }
            if ( _wave_receivers.contains( r ) ) {
                _wave_receivers.remove( r );
            }
        }

        public void clearReceiver() {
            _wave_receivers.clear();
        }

        public void begin( long length ) {
            long remain = length;
            while ( remain > 0 ) {
                int amount = (remain > _BUFLEN) ? _BUFLEN : (int)remain;
                _wave_sender.pull( _buffer_l, _buffer_r, amount );
                foreach ( WaveReceiver r in _wave_receivers ) {
                    r.push( _buffer_l, _buffer_r, amount );
                }
                remain -= amount;
            }
            foreach ( WaveReceiver r in _wave_receivers ) {
                r.end();
            }
        }
    }

#if !JAVA
}
#endif
