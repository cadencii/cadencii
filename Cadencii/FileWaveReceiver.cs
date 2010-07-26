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
        private WaveRateConvertAdapter _adapter = null;
        private double[] _buffer_l = new double[_BUFLEN];
        private double[] _buffer_r = new double[_BUFLEN];
        private double[] _buffer2_l = new double[_BUFLEN];
        private double[] _buffer2_r = new double[_BUFLEN];
        private WaveReceiver _receiver = null;

        public FileWaveReceiver( WaveWriter writer ) {
            _adapter = new WaveRateConvertAdapter( writer, VSTiProxy.SAMPLE_RATE );
        }

        public void end() {
            _adapter.close();
        }

        public void push( double[] l, double[] r, int length ) {
            _adapter.append( l, r, length );
            if ( _receiver != null ) {
                _receiver.push( l, r, length );
            }
        }

        public void setReceiver( WaveReceiver r ) {
            if ( r != null ) {
                r.end();
            }
            _receiver = r;
        }
    }

#if !JAVA
}
#endif
