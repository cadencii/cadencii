/*
 * FileWaveReceiver.cs
 * Copyright © 2010 kbinani
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

namespace org.kbinani.cadencii {
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

        public FileWaveReceiver( string path, int channel, int bit_per_sample, int sample_rate ) {
            WaveWriter ww = new WaveWriter( path, channel, bit_per_sample, sample_rate );
            mAdapter = new WaveRateConvertAdapter( ww, VSTiDllManager.SAMPLE_RATE );
        }

        public void setGlobalConfig( EditorConfig config ) {
            // do nothing
        }

        /// <summary>
        /// 初期化メソッド．
        /// </summary>
        /// <param name="parameter"></param>
        public void init( String parameter ) {
        }

        public int getVersion() {
            return mVersion;
        }

        public void end() {
#if DEBUG
            PortUtil.println( "FileWaveReceiver#end" );
#endif
            lock ( mAdapter ) {
                mAdapter.close();
            }
        }

        public void push( double[] l, double[] r, int length ) {
            lock ( mAdapter ) {
                mAdapter.append( l, r, length );
            }
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
