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

import java.awt.*;
import java.util.*;
import org.kbinani.*;
import org.kbinani.media.*;
#else
using System;
using org.kbinani.java.awt;
using org.kbinani.java.util;
using org.kbinani.media;

namespace org.kbinani.cadencii
{
#endif

#if JAVA
    public class FileWaveSender extends WaveUnit implements WaveSender
#else
    public class FileWaveSender : WaveUnit, WaveSender
#endif
    {
#if DEBUG
        private static int mNumInstance = 0;
#endif
        private const int BUFLEN = 1024;
        private double[] mBufferL = new double[BUFLEN];
        private double[] mBufferR = new double[BUFLEN];
        private WaveRateConverter mConverter = null;
        private long mPosition = 0;
        private int mVersion = 0;
        private WaveReader mReader = null;
        private Object mSyncRoot = new Object();

        public FileWaveSender( WaveReader reader )
        {
            mReader = reader;
#if DEBUG
            mNumInstance++;
#endif
        }

        public override int getVersion()
        {
            return mVersion;
        }

        public override void setConfig( String parameter )
        {
            // do nothing
        }

        public void setSender( WaveSender s )
        {
            // do nothing
        }

        public void pull( double[] l, double[] r, int length )
        {
            lock ( mSyncRoot ) {
                if ( mConverter == null ) {
                    int rate = mRoot.getSampleRate();
                    mConverter = new WaveRateConverter( mReader, rate );
                }
                try {
                    mConverter.read( mPosition, length, l, r );
                    mPosition += length;
                } catch ( Exception ex ) {
                    serr.println( "FileWaveSender#pull; ex=" + ex );
                    Logger.write( typeof( FileWaveSender ) + ".pull; ex=" + ex + "\n" );
                }
            }
        }

        public void end()
        {
#if DEBUG
            mNumInstance--;
            sout.println( "FileWaveSender#end; mNumInstance=" + mNumInstance );
#endif
            lock ( mSyncRoot ) {
                try {
                    mConverter.close();
                } catch ( Exception ex ) {
                    sout.println( "FileWaveSender#end; ex=" + ex );
                    Logger.write( typeof( FileWaveSender ) + ".end; ex=" + ex + "\n" );
                }
            }
        }
    }

#if !JAVA
}
#endif
