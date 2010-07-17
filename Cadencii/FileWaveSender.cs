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

    public class FileWaveSender : PassiveWaveSender {
        private const int _BUFLEN = 1024;
        private double[] _buffer_l = new double[_BUFLEN];
        private double[] _buffer_r = new double[_BUFLEN];
        private WaveRateConverter _converter = null;
        private long _position = 0;

        public FileWaveSender ( WaveReader reader ){
            _converter = new WaveRateConverter( reader, VSTiProxy.SAMPLE_RATE );
        }

        public void pull( double[] l, double[] r, int length ) {
            _converter.read( _position, length, l, r );
            _position += length;
        }

        public void end() {
            _converter.close();
        }
    }

#if !JAVA
}
#endif
