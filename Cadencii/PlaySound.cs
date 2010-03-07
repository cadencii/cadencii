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
using org.kbinani;

namespace org.kbinani.cadencii {
    using DWORD = System.UInt32;
    using UINT = System.UInt32;
    using WORD = System.UInt16;
#endif

    public class PlaySound {
        [DllImport( "PlaySound" )]
        private static extern void SoundInit();
        [DllImport( "PlaySound" )]
        private static extern void SoundPrepare( int sample_rate );
        [DllImport( "PlaySound" )]
        private static extern void SoundAppend( IntPtr left, IntPtr right, int length );
        [DllImport( "PlaySound" )]
        private static extern void SoundExit();
        [DllImport( "PlaySound" )]
        private static extern double SoundGetPosition();
        [DllImport( "PlaySound" )]
        private static extern bool SoundIsBusy();
        [DllImport( "PlaySound" )]
        private static extern void SoundWaitForExit();
        [DllImport( "PlaySound" )]
        private static extern void SoundSetResolution( int resolution );
        [DllImport( "PlaySound" )]
        private static extern void SoundKill();
        [DllImport( "PlaySound" )]
        private static extern void SoundUnprepare();

        public static void setResolution( int value ) {
            try {
                SoundSetResolution( value );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "PlaySound#setResolution; ex=" + ex );
            }
        }

        public static void init() {
#if JAVA
            m_buffer = new byte[UNIT_BUFFER * 4];
#else
            try {
                SoundInit();
            } catch ( Exception ex ) {
                PortUtil.println( "PlaySound#init; ex=" + ex );
            }
#endif
        }

        public static void kill() {
#if JAVA
#else
            try {
                SoundKill();
            } catch( Exception ex ){
                PortUtil.println( "PlaySound#kill; ex=" + ex );
            }
#endif
        }

        public static double getPosition() {
#if JAVA
            return m_line.getMicrosecondPosition() * 1e-6;
#else
            try {
                return SoundGetPosition();
            } catch ( Exception ex ) {
                PortUtil.println( "PlaySound#getPosition; ex=" + ex );
                return -1.0;
            }
#endif
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
                PortUtil.println( "PlaySound#waitForExit; ex=" + ex );
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
            try {
                IntPtr l = Marshal.UnsafeAddrOfPinnedArrayElement( left, 0 );
                IntPtr r = Marshal.UnsafeAddrOfPinnedArrayElement( right, 0 );
                SoundAppend( l, r, length );
            } catch ( Exception ex ) {
                PortUtil.println( "PlaySound#append; ex=" + ex );
            }
#endif
        }

        /// <summary>
        /// デバイスを初期化する
        /// </summary>
        /// <param name="sample_rate"></param>
        public static void prepare( int sample_rate ) {
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
            try {
                SoundPrepare( sample_rate );
            } catch ( Exception ex ) {
                PortUtil.println( "PlaySound#prepare; ex=" + ex );
            }
#endif
        }

        /// <summary>
        /// 再生をとめる。
        /// </summary>
        public static void exit() {
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
                SoundExit();
            } catch ( Exception ex ) {
                PortUtil.println( "PlaySound#exit; ex=" + ex );
            }
#endif
        }

        private static void unprepare() {
#if JAVA
#else
            try {
                SoundUnprepare();
            } catch ( Exception ex ) {
                PortUtil.println( "PlaySound#unprepare; ex=" + ex );
            }
        }
#endif
    }

#if !JAVA
}
#endif
