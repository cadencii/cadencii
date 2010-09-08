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
using System;
using org.kbinani.java.util;
using org.kbinani.media;

namespace org.kbinani.cadencii.draft {
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
        private int _version = 0;

        /// <summary>
        /// 初期化メソッド．
        /// (デフォルトでは改行文字区切りで)String path, int channel, int bit_per_sample, int sample_rate
        /// </summary>
        /// <param name="parameter"></param>
        public void init( String parameter ) {
            if( parameter == null ){
                return;
            }
            if ( parameter.Length < 2 ){
                return;
            }
            char sep = parameter[0];
            parameter = parameter.Substring( 1 );
            String[] spl = PortUtil.splitString( parameter, sep );
            String path = spl[0];
            int channel = PortUtil.parseInt( spl[1] );
            int bit_per_sample = PortUtil.parseInt( spl[2] );
            int sample_rate = PortUtil.parseInt( spl[3] );
            WaveWriter ww = new WaveWriter( path, channel, bit_per_sample, sample_rate );
            _adapter = new WaveRateConvertAdapter( ww, VSTiProxy.SAMPLE_RATE );
        }

        public int getVersion() {
            return _version;
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
