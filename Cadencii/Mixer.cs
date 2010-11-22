/*
 * Mixer.cs
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

import java.awt.*;
import java.util.*;
#else
using System;
using org.kbinani.java.awt;
using org.kbinani.java.util;

namespace org.kbinani.cadencii.draft {
#endif

#if JAVA
    public class Mixer extends WaveUnit implements WaveSender, WaveReceiver {
#else
    public class Mixer : WaveUnit, WaveSender, WaveReceiver {
#endif
        private const int _BUFLEN = 1024;

        private WaveReceiver _receiver = null;
        private Vector<WaveSender> _senders = new Vector<WaveSender>();
        private double[] _buffer_l = new double[_BUFLEN];
        private double[] _buffer_r = new double[_BUFLEN];
        private double[] _buffer2_l = new double[_BUFLEN];
        private double[] _buffer2_r = new double[_BUFLEN];
        private int _version = 0;

        public override int getVersion() {
            return _version;
        }

        public override void setConfig( String parameter ) {
            // do nothing
        }

        public void push( double[] l, double[] r, int length ) {
            int remain = length;
            int offset = 0;
            while ( remain > 0 ) {
                int amount = (remain > _BUFLEN) ? _BUFLEN : remain;
                for ( int i = 0; i < _BUFLEN; i++ ) {
                    _buffer_l[i] = l[i + offset];
                    _buffer_r[i] = r[i + offset];
                }
                if ( _receiver != null ) {
                    _receiver.push( _buffer_l, _buffer_r, amount );
                }
                remain -= amount;
                offset += amount;
            }
        }

        public void pull( double[] l, double[] r, int length ) {
            int remain = length;
            int offset = 0;
            while ( remain > 0 ) {
                int amount = (remain > _BUFLEN) ? _BUFLEN : remain;
                for ( int i = 0; i < amount; i++ ) {
                    _buffer2_l[i] = 0.0;
                    _buffer2_r[i] = 0.0;
                }
                foreach ( WaveSender s in _senders ) {
                    if ( s == null ) {
                        continue;
                    }
                    s.pull( _buffer_l, _buffer_r, amount );
                    for ( int i = 0; i < amount; i++ ) {
                        _buffer2_l[i] += _buffer_l[i];
                        _buffer2_r[i] += _buffer_r[i];
                    }
                }
                if ( _receiver != null ) {
                    _receiver.push( _buffer2_l, _buffer2_r, amount );
                }
                for ( int i = 0; i < amount; i++ ) {
                    l[i + offset] = _buffer2_l[i];
                    r[i + offset] = _buffer2_r[i];
                }
                remain -= amount;
                offset += amount;
            }
        }

        public void setReceiver( WaveReceiver r ) {
            if ( _receiver != null ) {
                _receiver.end();
            }
            _receiver = r;
        }

        public void setSender( WaveSender s ) {
            addSender( s );
        }

        public void end() {
            if ( _receiver != null ) {
                _receiver.end();
            }
            foreach ( WaveSender s in _senders ) {
                if ( s != null ) {
                    s.end();
                }
            }
        }

        public void addSender( WaveSender s ) {
            if ( s == null ) {
                return;
            }
            if ( !_senders.contains( s ) ) {
                _senders.add( s );
            }
        }
    }

#if !JAVA
}
#endif
