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
        private const int BUFLEN = 1024;
        private WaveRateConvertAdapter mAdapter = null;
        private double[] mBufferL = new double[BUFLEN];
        private double[] mBufferR = new double[BUFLEN];
        private double[] mBuffer2L = new double[BUFLEN];
        private double[] mBuffer2R = new double[BUFLEN];
        private WaveReceiver mReceiver = null;
        private int mVersion = 0;

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
            mAdapter = new WaveRateConvertAdapter( ww, VSTiProxy.SAMPLE_RATE );
        }

        public int getVersion() {
            return mVersion;
        }

        public void end() {
            mAdapter.close();
        }

        public void push( double[] l, double[] r, int length ) {
            mAdapter.append( l, r, length );
            if ( mReceiver != null ) {
                mReceiver.push( l, r, length );
            }
        }

        public void setReceiver( WaveReceiver r ) {
            if ( r != null ) {
                r.end();
            }
            mReceiver = r;
        }
    }

#if !JAVA
}
#endif
