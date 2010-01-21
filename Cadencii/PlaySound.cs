/*
 * PlaySound.cs
 * Copyright (C) 2009-2010 kbinani
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

import javax.sound.sampled.*;
#else
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif
    public class PlaySound {

#if JAVA
        private static final int UNIT_BUFFER = 512;
        private static SourceDataLine m_line;
        private static AudioFormat m_format;
        private static DataLine.Info m_info;
        private static byte[] m_buffer;
#else
        //private static bool s_initialized = false;
        private static int s_sample_rate = 44100;
#endif

#if JAVA
        static{
            m_buffer = new byte[UNIT_BUFFER * 4];
        }
#else
        [DllImport( "PlaySound" )]
        private static extern void SoundInit( int sample_rate );

        [DllImport( "PlaySound" )]
        private static extern void SoundAppend( IntPtr leftDoublePointer, IntPtr rightDoublePointer, int length );

        [DllImport( "PlaySound" )]
        private static extern void SoundWaitForExit();

        [DllImport( "PlaySound" )]
        private static extern double SoundGetPosition();

        [DllImport( "PlaySound" )]
        private static extern boolean SoundIsBusy();

        [DllImport( "PlaySound" )]
        private static extern void SoundReset();

        [DllImport( "PlaySound" )]
        private static extern void SoundSetResolution( int resolution );

        [DllImport( "PlaySound" )]
        private static extern void SoundTerminate();
#endif

        private static Object synchronizer = new Object();
        private static int capacity;
        private static int numBuffer = 2;
        private static double[][] left;
        private static double[][] right;
        private static boolean locked = false;
        private static int index = 0;
        private static int loc = 0;
        private static Thread listener = null;

        public static void terminate() {
            try {
                SoundTerminate();
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "PlaySound#terminate; ex=" + ex );
            }
        }

        private static void listenerProc() {
            while ( true ) {
                try {
                    Thread.Sleep( 100 );
                } catch ( ThreadInterruptedException ex ) {
                    PortUtil.stderr.println( "WaveBufferAdapter#listenerProc; ex=" + ex );
                    break;
                }
            }
        }

        private static void append_( double[] l, double[] r, int len ) {
            int remain = capacity - loc;
            int offset = 0;
            int appended = 0;
            //int len = Math.Min( l.Length, r.Length );
            while ( appended < len ) {
                if ( offset + remain >= len ) {
                    remain = len - offset;
                }
                // l, rのoffset -> offset + remainまでを，index[*]のloc -> loc + remainまでに転送する
                lock ( synchronizer ) {
                    for ( int i = offset; i < offset + remain; i++ ) {
                        left[index][i - offset + loc] = l[i];
                        right[index][i - offset + loc] = r[i];
                    }
                    loc += remain;
                }

                appended += remain;
                offset += remain;
                if ( loc == capacity ) {
                    loc = 0;
                    append( left[index], right[index], capacity );
                    index++;
                    if ( index == numBuffer ) {
                        index = 0;
                    }
                }
                remain = capacity - loc;
                if ( offset + remain >= len ) {
                    remain = len - offset;
                }
            }
        }

        public static void waitForExit() {
#if JAVA
            if( m_line == null ){
                return;
            }
            m_line.drain();
#else
            try {
                SoundWaitForExit();
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "PlaySound#waitForExit; ex=" + ex );
            }
#endif
        }

        public static void init( int sample_rate ) {
#if JAVA
            m_format = new AudioFormat( sample_rate, 16, 2, true, false );
            m_info = new DataLine.Info( SourceDataLine.class, m_format );
            try{
                m_line = (SourceDataLine)AudioSystem.getLine( m_info );
                m_line.open( m_format );
                m_line.start();
            }catch( Exception ex ){
                m_line = null;
            }
#else
#if DEBUG
            PortUtil.println( "PlaySound#init; sample_rate=" + sample_rate );
#endif
            s_sample_rate = sample_rate;
            try {
                SoundInit( sample_rate );
                SoundSetResolution( VSTiProxy.SAMPLE_RATE );
                //s_initialized = true;
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "PlaySound.Init; ex=" + ex );
            }
            /*capacity = capacity_samples;
            numBuffer = num_buffer;
            left = new double[numBuffer][];
            right = new double[numBuffer][];
            for ( int i = 0; i < numBuffer; i++ ) {
                left[i] = new double[capacity];
                right[i] = new double[capacity];
            }
            listener = new Thread( new ThreadStart( listenerProc ) );
            listener.IsBackground = true;
            listener.Start();*/
#endif
        }

        public static void append( double[] left, double[] right, int length ) {
#if JAVA
            if( m_line == null ){
                return;
            }
            int remain = left.length;
            int off = 0;
            while( remain > 0 ){
                int thislen = remain > UNIT_BUFFER ? UNIT_BUFFER : remain;
                int c = 0;
                for( int i = 0; i < thislen; i++ ){
                    short l = (short)(left[i + off] * 32767.0);
                    m_buffer[c] = (byte)(0xff & l);
                    m_buffer[c + 1] = (byte)(0xff & (l >>> 8));
                    short r = (short)(right[i + off] * 32767.0);
                    m_buffer[c + 2] = (byte)(0xff & r);
                    m_buffer[c + 3] = (byte)(0xff & (r >>> 8));
                    c += 4;
                }
                m_line.write( m_buffer, 0, thislen * 4 );
                off += thislen;
                remain -= thislen;
            }
#else
            if ( length <= 0 ) {
                return;
            }
            try {
                IntPtr pl = Marshal.UnsafeAddrOfPinnedArrayElement( left, 0 );
                IntPtr pr = Marshal.UnsafeAddrOfPinnedArrayElement( right, 0 );
                SoundAppend( pl, pr, length );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "PlaySound#Append; ex=" + ex );
            }
#endif
        }

        public static double getPosition() {
#if JAVA
            return m_line.getMicrosecondPosition() * 1e-6;
#else
            double ret = -1;
            try {
                ret = SoundGetPosition();
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "PlaySound.GetPosition; ex=" + ex );
            }
            return ret;
#endif

        }

        public static void reset() {
#if JAVA
            m_line.stop();
            m_line.close();
            m_info = new DataLine.Info( SourceDataLine.class, m_format );
            try{
                m_line = (SourceDataLine)AudioSystem.getLine( m_info );
                m_line.open( m_format );
                m_line.start();
            }catch( Exception ex ){
                PortUtil.stderr.println( "PlaySound#reset; ex=" + ex );
                m_line = null;
            }
#else
            try {
                SoundReset();
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "PlaySound.Reset; ex=" + ex );
            }
#endif
        }
    }

#if !JAVA
}
#endif
