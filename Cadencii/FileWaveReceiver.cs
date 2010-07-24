/*
 * FileWaveReceiver.cs
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
import org.kbinani.media.*;
#else
using org.kbinani.java.util;
using org.kbinani.media;

namespace org.kbinani.cadencii {
#endif

#if JAVA
    public class FileWaveReceiver implements WaveReceiver {
#else
    public class FileWaveReceiver : WaveReceiver {
#endif
        private const int _BUFLEN = 1024;
        private Vector<PassiveWaveSender> _passive_wave_senders = new Vector<PassiveWaveSender>();
        private WaveRateConvertAdapter _adapter = null;
        private double[] _buffer_l = new double[_BUFLEN];
        private double[] _buffer_r = new double[_BUFLEN];
        private double[] _buffer2_l = new double[_BUFLEN];
        private double[] _buffer2_r = new double[_BUFLEN];

        public FileWaveReceiver( WaveWriter writer ) {
            _adapter = new WaveRateConvertAdapter( writer, VSTiProxy.SAMPLE_RATE );
        }

        public void end() {
            _adapter.close();
        }

        public void clearPassiveWaveSender() {
            _passive_wave_senders.clear();
        }

        public void addPassiveWaveSender( PassiveWaveSender g ) {
            if ( g == null ) {
                return;
            }
            if ( !_passive_wave_senders.contains( g ) ) {
                _passive_wave_senders.add( g );
            }
        }

        public void removePassiveWaveSender( PassiveWaveSender g ) {
            if ( g == null ) {
                return;
            }
            if ( _passive_wave_senders.contains( g ) ) {
                _passive_wave_senders.remove( g );
            }
        }

        public void push( double[] l, double[] r, int length ) {
            int remain = length;
            while ( remain > 0 ) {
                int amount = (remain > _BUFLEN) ? _BUFLEN : remain;
                for ( int i = 0; i < amount; i++ ) {
                    _buffer_l[i] = 0.0;
                    _buffer_r[i] = 0.0;
                }
                foreach ( PassiveWaveSender g in _passive_wave_senders ) {
                    g.pull( _buffer2_l, _buffer2_r, amount );
                    for ( int i = 0; i < amount; i++ ) {
                        _buffer_l[i] += _buffer2_l[i];
                        _buffer_r[i] += _buffer2_r[i];
                    }
                }
                int offset = length - remain;
                for ( int i = 0; i < amount; i++ ) {
                    _buffer2_l[i] = l[i + offset] + _buffer_l[i];
                    _buffer2_r[i] = r[i + offset] + _buffer_r[i];
                }
                _adapter.append( _buffer2_l, _buffer2_r, amount );
                remain -= amount;
            }
        }
    }

#if !JAVA
}
#endif
