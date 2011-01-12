/*
 * FileWaveReceiver.cs
 * Copyright © 2010-2011 kbinani
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
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.media;

namespace org.kbinani.cadencii
{
#endif

#if JAVA
    public class FileWaveReceiver implements WaveReceiver {
#else
    public class FileWaveReceiver : WaveUnit, WaveReceiver
    {
#endif
        private const int BUFLEN = 1024;
        private WaveWriter mAdapter = null;
        private double[] mBufferL = new double[BUFLEN];
        private double[] mBufferR = new double[BUFLEN];
        private double[] mBuffer2L = new double[BUFLEN];
        private double[] mBuffer2R = new double[BUFLEN];
        private WaveReceiver mReceiver = null;
        private int mVersion = 0;
        private String mPath;
        private int mChannel;
        private int mBitPerSample;
        private Object mSyncRoot = new Object();

        public FileWaveReceiver( String path, int channel, int bit_per_sample )
        {
            mPath = path;
            mChannel = channel;
            mBitPerSample = bit_per_sample;
            //WaveWriter ww = new WaveWriter( path, channel, bit_per_sample, sample_rate );
            //mAdapter = new WaveRateConvertAdapter( ww, VSTiDllManager.SAMPLE_RATE );
        }

        public override void setGlobalConfig( EditorConfig config )
        {
            // do nothing
        }

        public override void setConfig( String parameter )
        {
            // do nothing
        }

        /// <summary>
        /// 初期化メソッド．
        /// </summary>
        /// <param name="parameter"></param>
        public void init( String parameter )
        {
        }

        public override int getVersion()
        {
            return mVersion;
        }

        public void end()
        {
            lock ( mSyncRoot ) {
#if DEBUG
                if ( mAdapter == null ) {
                    PortUtil.println( "FileWaveReceiver#end; warning; 'end' when mAdapter is null" );
                }
#endif
                if ( mAdapter != null ) {
                    mAdapter.close();
                }
            }
        }

        public void push( double[] l, double[] r, int length )
        {
            lock ( mSyncRoot ) {
                if ( mAdapter == null ) {
                    int sample_rate = mRoot.getSampleRate();
#if DEBUG
                    PortUtil.println( "FileWaveReceiver#push; sample_rate=" + sample_rate );
#endif
                    mAdapter = new WaveWriter( mPath, mChannel, mBitPerSample, sample_rate );
                }
                mAdapter.append( l, r, length );
                if ( mReceiver != null ) {
                    mReceiver.push( l, r, length );
                }
            }
        }

        public void setReceiver( WaveReceiver r )
        {
            if ( mReceiver != null ) {
                mReceiver.end();
            }
            mReceiver = r;
        }
    }

#if !JAVA
}
#endif
