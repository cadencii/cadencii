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
    public class FileWaveSender extends WaveUnit implements WaveSender {
#else
    public class FileWaveSender : WaveUnit, WaveSender
    {
#endif
#if DEBUG
        private static int mNumInstance = 0;
#endif
        private const int _BUFLEN = 1024;
        private double[] _buffer_l = new double[_BUFLEN];
        private double[] _buffer_r = new double[_BUFLEN];
        private WaveRateConverter _converter = null;
        private long _position = 0;
        private int _version = 0;
        private WaveReader mReader = null;
        private object mSyncRoot = new object();

        public FileWaveSender( WaveReader reader )
        {
            mReader = reader;
#if DEBUG
            mNumInstance++;
#endif
        }

        public override int getVersion()
        {
            return _version;
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
                if ( _converter == null ) {
                    int rate = mRoot.getSampleRate();
#if DEBUG
                    PortUtil.println( "FileWaveSender#pull; mRoot.GetType()=" + mRoot.GetType() );
                    PortUtil.println( "FileWaveSender#pull; rate=" + rate );
#endif
                    _converter = new WaveRateConverter( mReader, rate );
                }
                try {
                    _converter.read( _position, length, l, r );
                    _position += length;
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "FileWaveSender#pull; ex=" + ex );
                    Logger.write( typeof( FileWaveSender ) + ".pull; ex=" + ex + "\n" );
                }
            }
        }

        public void end()
        {
#if DEBUG
            mNumInstance--;
            PortUtil.println( "FileWaveSender#end; mNumInstance=" + mNumInstance );
#endif
            lock ( mSyncRoot ) {
                try {
                    _converter.close();
                } catch ( Exception ex ) {
                    PortUtil.println( "FileWaveSender#end; ex=" + ex );
                    Logger.write( typeof( FileWaveSender ) + ".end; ex=" + ex + "\n" );
                }
            }
        }
    }

#if !JAVA
}
#endif
