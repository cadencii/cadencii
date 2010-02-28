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

namespace org.kbinani.cadencii {
    using DWORD = System.UInt32;
    using UINT = System.UInt32;
    using WORD = System.UInt16;
#endif

    public unsafe class PlaySound {
#if JAVA
        private static final int UNIT_BUFFER = 512;
        private static SourceDataLine m_line;
        private static AudioFormat m_format;
        private static DataLine.Info m_info;
        private static byte[] m_buffer;
#else
        const int NUM_BUF = 3;
        static IntPtr wave_out = IntPtr.Zero;
        static WAVEFORMATEX wave_format;
        static WAVEHDR[] wave_header = new WAVEHDR[NUM_BUF];
        static DWORD*[] wave = new DWORD*[NUM_BUF];
        static bool[] wave_done = new bool[NUM_BUF];
        static int buffer_index = 0; // 次のデータを書き込むバッファの番号
        static int buffer_loc = 0; // 次のデータを書き込む位置
        static object locker = null;
        static MemoryManager mman = null;
        static delegateWaveOutProc callback = null;
#endif

        public static void init() {
#if JAVA
            m_buffer = new byte[UNIT_BUFFER * 4];
#else
            locker = new object();
            mman = new MemoryManager();
            callback = new delegateWaveOutProc( SoundCallback );
#endif
        }

        public static void kill() {
#if JAVA
#else
            exit();
            if ( mman != null ) {
                mman.dispose();
            }
#endif
        }

        public static double getPosition() {
#if JAVA
            return m_line.getMicrosecondPosition() * 1e-6;
#else
            if ( IntPtr.Zero == wave_out ) {
                return -1.0;
            }

            MMTIME mmt = new MMTIME();
            mmt.wType = win32.TIME_MS;
            win32.waveOutGetPosition( wave_out, ref mmt, (uint)sizeof( MMTIME ) );
            float ms = 0.0f;
            switch ( mmt.wType ) {
                case win32.TIME_MS:
                    return mmt.ms * 0.001;
                    break;
                case win32.TIME_SAMPLES:
                    return (double)mmt.sample / (double)wave_format.nSamplesPerSec;
                case win32.TIME_BYTES:
                    return (double)mmt.cb / (double)wave_format.nAvgBytesPerSec;
                default:
                    return -1.0;
            }
            return 0.0;
#endif
        }

        public static void waitForExit() {
#if JAVA
            if( m_line == null ){
                return;
            }
            m_line.drain();
#else
            if ( IntPtr.Zero == wave_out ) {
                return;
            }

            lock ( locker ) {
                // buffer_indexがNUM_BUF未満なら、まだ1つもwaveOutWriteしていないので、書き込む
                if ( buffer_index < NUM_BUF ) {
                    for ( int i = 0; i < buffer_index; i++ ) {
                        wave_done[i] = false;
                        win32.waveOutWrite( wave_out, ref wave_header[i], (uint)sizeof( WAVEHDR ) );
                    }
                }

                // まだ書き込んでないバッファがある場合、残りを書き込む
                if ( buffer_loc != 0 ) {
                    int act_buffer_index = buffer_index % NUM_BUF;

                    // バッファが使用中の場合、使用終了となるのを待ち受ける
                    while ( !wave_done[act_buffer_index] ) {
                        System.Windows.Forms.Application.DoEvents();
                    }

                    // 後半部分を0で埋める
                    for ( int i = buffer_loc; i < wave_format.nSamplesPerSec; i++ ) {
                        wave[act_buffer_index][i] = (DWORD)((WORD)0 | (WORD)(0 << 16));
                    }

                    buffer_loc = 0;
                    buffer_index++;

                    wave_done[act_buffer_index] = false;
                    win32.waveOutWrite( wave_out, ref wave_header[act_buffer_index], (uint)sizeof( WAVEHDR ) );
                }

                // NUM_BUF個のバッファすべてがwave_doneとなるのを待つ。
                while ( true ) {
                    bool all_done = true;
                    for ( int i = 0; i < NUM_BUF; i++ ) {
                        if ( !wave_done[i] ) {
                            all_done = false;
                            break;
                        }
                    }
                    if ( all_done ) {
                        break;
                    }
                }
            }

            // リセット処理
            exit();
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
            if ( IntPtr.Zero == wave_out ) {
                return;
            }
            lock ( locker ) {
                int appended = 0; // 転送したデータの個数
                while ( appended < length ) {
                    // このループ内では、バッファに1個づつデータを転送する

                    // バッファが使用中の場合、使用終了となるのを待ち受ける
                    int act_buffer_index = buffer_index % NUM_BUF;
                    while ( !wave_done[act_buffer_index] ) {
                        System.Windows.Forms.Application.DoEvents();
                    }

                    int t_length = (int)wave_format.nSamplesPerSec - buffer_loc; // 転送するデータの個数
                    if ( t_length > length - appended ) {
                        t_length = length - appended;
                    }
                    for ( int i = 0; i < t_length; i++ ) {
                        wave[act_buffer_index][buffer_loc + i] = (DWORD)((WORD)(left[appended + i] * 32768.0) | ((WORD)(right[appended + i] * 32768.0) << 16));
                    }
                    appended += t_length;
                    buffer_loc += t_length;
                    if ( buffer_loc == wave_format.nSamplesPerSec ) {
                        // バッファがいっぱいになったようだ
                        buffer_index++;
                        buffer_loc = 0;
                        if ( buffer_index >= NUM_BUF ) {
                            // 最初のNUM_BUF個のバッファは、すべてのバッファに転送が終わるまで
                            // waveOutWriteしないようにしているので、ここでwaveOutWriteする。
                            if ( buffer_index == NUM_BUF ) {
                                for ( int i = 0; i < NUM_BUF; i++ ) {
                                    wave_done[i] = false;
                                    win32.waveOutWrite( wave_out, ref wave_header[i], (uint)sizeof( WAVEHDR ) );
                                }
                            } else {
                                wave_done[act_buffer_index] = false;
                                win32.waveOutWrite( wave_out, ref wave_header[act_buffer_index], (uint)sizeof( WAVEHDR ) );
                            }
                        }
                    }
                }

            }
#endif
        }

#if !JAVA
        /// <summary>
        /// コールバック関数。バッファの再生終了を検出するために使用。
        /// </summary>
        /// <param name="hwo"></param>
        /// <param name="uMsg"></param>
        /// <param name="dwInstance"></param>
        /// <param name="dwParam1"></param>
        /// <param name="dwParam2"></param>
        public static void SoundCallback(
            IntPtr hwo,
            UINT uMsg,
            DWORD dwInstance,
            DWORD dwParam1,
            DWORD dwParam2 ) {
            if ( uMsg != win32.MM_WOM_DONE ) {
                return;
            }

            for ( int i = 0; i < NUM_BUF; i++ ) {
                fixed ( WAVEHDR* ptr = &wave_header[i] ) {
                    if ( ptr != (WAVEHDR*)dwParam1 ) {
                        continue;
                    }
                }
                wave_done[i] = true;
                break;
            }
        }
#endif

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
            // デバイスを使用中の場合、使用を停止する
            if ( IntPtr.Zero != wave_out ) {
                exit();
                unprepare();
            }

            lock ( locker ) {
                // フォーマットを指定
                wave_format.wFormatTag = win32.WAVE_FORMAT_PCM;
                wave_format.nChannels = 2;
                wave_format.wBitsPerSample = 16;
                wave_format.nBlockAlign
                    = (ushort)(wave_format.nChannels * wave_format.wBitsPerSample / 8);
                wave_format.nSamplesPerSec = (uint)sample_rate;
                wave_format.nAvgBytesPerSec
                    = wave_format.nSamplesPerSec * wave_format.nBlockAlign;

                // デバイスを開く
                win32.waveOutOpen( ref wave_out,
                             win32.WAVE_MAPPER,
                             ref wave_format,
                             callback,
                             IntPtr.Zero,
                             win32.CALLBACK_FUNCTION );

                // バッファを準備
                for ( int i = 0; i < NUM_BUF; i++ ) {
                    IntPtr p = mman.malloc( (int)(sizeof( DWORD ) * wave_format.nSamplesPerSec) );
                    wave[i] = (DWORD*)p.ToPointer();
                    wave_header[i].lpData = p;
                    wave_header[i].dwBufferLength = sizeof( DWORD ) * wave_format.nSamplesPerSec;
                    wave_header[i].dwFlags = win32.WHDR_BEGINLOOP | win32.WHDR_ENDLOOP;
                    wave_header[i].dwLoops = 1;
                    win32.waveOutPrepareHeader( wave_out, ref wave_header[i], (uint)sizeof( WAVEHDR ) );

                    wave_done[i] = true;
                }

                buffer_index = 0;
                buffer_loc = 0;
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
            lock ( locker ) {
                if ( IntPtr.Zero != wave_out ) {
                    win32.waveOutReset( wave_out );
                }
            }
#endif
        }

        private static void unprepare() {
#if JAVA
#else
            if ( wave_out == IntPtr.Zero ) {
                return;
            }

            lock ( locker ) {
                for ( int i = 0; i < NUM_BUF; i++ ) {
                    win32.waveOutUnprepareHeader( wave_out,
                                                  ref wave_header[i],
                                                  (uint)sizeof( WAVEHDR ) );
                    mman.free( wave_header[i].lpData );
                }
                win32.waveOutClose( wave_out );
                wave_out = IntPtr.Zero;
            }
        }
#endif
    }

#if !JAVA
}
#endif
