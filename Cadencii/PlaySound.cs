/*
 * PlaySound.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.Cadencii;

import javax.sound.sampled.*;
#else
using System;
using System.Runtime.InteropServices;

namespace Boare.Cadencii {
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
        private static bool s_initialized = false;
        private static int s_sample_rate = 44100;
#endif

#if JAVA
        static{
            m_buffer = new byte[UNIT_BUFFER * 4];
        }
#else
        [DllImport( "PlaySound" )]
        private static extern void SoundInit( int block_size, int sample_rate );

        [DllImport( "PlaySound" )]
        private static extern unsafe void SoundAppend( double* left, double* right, int length );

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
#endif

        public static void waitForExit() {
#if JAVA
            if( m_line == null ){
                return;
            }
            m_line.drain();
#else
            try {
                SoundWaitForExit();
            } catch {
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
            if ( s_initialized ) {
                return;
            }
            s_sample_rate = sample_rate;
            try {
                SoundInit( VSTiProxy.BLOCK_SIZE, sample_rate );
                SoundSetResolution( VSTiProxy.BLOCK_SIZE );
                s_initialized = true;
            } catch ( Exception ex ) {
                bocoree.debug.push_log( "PlaySound.Init; ex=" + ex );
            }
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
                unsafe {
                    fixed ( double* pl = &left[0] )
                    fixed ( double* pr = &right[0] ) {
                        SoundAppend( pl, pr, length );
                    }
                }
            } catch ( Exception ex ) {
                bocoree.debug.push_log( "PlaySound#Append; ex=" + ex );
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
#if DEBUG
                bocoree.debug.push_log( "PlaySound.GetPosition; ex=" + ex );
#endif
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
                m_line = null;
            }
#else
            try {
                SoundReset();
            } catch ( Exception ex ) {
#if DEBUG
                bocoree.debug.push_log( "PlaySound.Reset; ex=" + ex );
#endif
            }
#endif
        }
    }

#if !JAVA
}
#endif
