/*
 * Separator.cs
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
#else
using System;
using org.kbinani.java.util;

namespace org.kbinani.cadencii {
#endif

    /// <summary>
    /// 信号を分岐する装置
    /// </summary>
    public class Separator : WaveReceiver {
        const int _BUFLEN = 1024;
        private Vector<WaveReceiver> _receivers = new Vector<WaveReceiver>();
        private double[] _buff_l = new double[_BUFLEN];
        private double[] _buff_r = new double[_BUFLEN];
        private int _version = 0;

        public int getVersion() {
            return _version;
        }

        public void init( String parameter ) {
            // do nothing
        }

        public void setReceiver( WaveReceiver receiver ) {
            if ( receiver == null ) {
                return;
            }
            if ( !_receivers.contains( receiver ) ) {
                _receivers.add( receiver );
            }
        }

        public void end() {
            foreach ( WaveReceiver r in _receivers ) {
                r.end();
            }
        }

        public void addReceiver( WaveReceiver receiver ) {
            if ( receiver == null ) {
                return;
            }
            if ( !_receivers.contains( receiver ) ) {
                _receivers.add( receiver );
            }
        }

        public void push( double[] l, double[] r, int length ) {
            if ( _receivers.size() <= 0 ) {
                return;
            }

            int remain = length;
            int offset = 0;
            while ( remain > 0 ) {
                int amount = remain > _BUFLEN ? _BUFLEN : remain;
                for ( int i = 0; i < amount; i++ ) {
                    _buff_l[i] = l[i + offset];
                    _buff_r[i] = r[i + offset];
                }
                foreach ( WaveReceiver rc in _receivers ) {
                    rc.push( _buff_l, _buff_r, amount );
                }
                offset += amount;
                remain -= amount;
            }
        }
    }

#if !JAVA
}
#endif
