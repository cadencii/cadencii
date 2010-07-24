/*
 * MonitorWaveReceiver.cs
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
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// スピーカへの出力を行う波形受信器
    /// </summary>
#if JAVA
    public class MonitorWaveReceiver implements WaveReceiver {
#else
    public class MonitorWaveReceiver : WaveReceiver {
#endif
        private const int _BUFLEN = 1024;

        private static MonitorWaveReceiver _singleton = null;

        private Vector<PassiveWaveSender> _passive_wave_sender = new Vector<PassiveWaveSender>();
        private boolean _first_call = true;
        private double[] _buffer_l = new double[_BUFLEN];
        private double[] _buffer_r = new double[_BUFLEN];
        private double[] _buffer2_l = new double[_BUFLEN];
        private double[] _buffer2_r = new double[_BUFLEN];

        private MonitorWaveReceiver() {
        }

        public static MonitorWaveReceiver getInstance() {
            if ( _singleton == null ) {
                _singleton = new MonitorWaveReceiver();
            }
            _singleton.end();
            _singleton._first_call = true;
            return _singleton;
        }

        public void push( double[] l, double[] r, int length ) {
            if ( _first_call ) {
                PlaySound.init();
                PlaySound.prepare( VSTiProxy.SAMPLE_RATE );
                _first_call = false;
            }
            int remain = length;
            while( remain > 0 ){
                int amount = (remain > _BUFLEN) ? _BUFLEN : remain;
                for( int i = 0; i < amount; i++ ){
                    _buffer2_l[i] = 0.0;
                    _buffer2_r[i] = 0.0;
                }
                foreach( PassiveWaveSender s in _passive_wave_sender ){
                    s.pull( _buffer_l, _buffer_r, amount );
                    for ( int i = 0; i < amount; i++ ){
                        _buffer2_l[i] += _buffer_l[i];
                        _buffer2_r[i] += _buffer_r[i];
                    }
                }
                int offset = length - remain;
                for( int i = 0; i < amount; i++ ){
                    _buffer2_l[i] += l[i + offset];
                    _buffer2_r[i] += r[i + offset];
                }
                PlaySound.append( _buffer2_l, _buffer2_r, amount );
                remain -= amount;
            }
        }

        public void end() {
            PlaySound.exit();
        }

        public void addPassiveWaveSender( PassiveWaveSender s ) {
            if ( s == null ) {
                return;
            }
            if ( !_passive_wave_sender.contains( s ) ) {
                _passive_wave_sender.add( s );
            }
        }

        public void removePassiveWaveSender( PassiveWaveSender s ) {
            if ( s == null ) {
                return;
            }
            if ( _passive_wave_sender.contains( s ) ) {
                _passive_wave_sender.remove( s );
            }
        }

        public void clearPassiveWaveSender() {
            _passive_wave_sender.clear();
        }
    }

#if !JAVA
}
#endif
