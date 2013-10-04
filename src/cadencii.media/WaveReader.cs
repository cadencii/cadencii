/*
 * WaveReader.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.media.
 *
 * cadencii.media is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.media is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii.media;

import java.io.*;
import java.util.*;
import cadencii.*;
#else
using System;
using cadencii;
using cadencii.java.io;

namespace cadencii.media {
#endif

#if JAVA
    public class WaveReader {
#else
    public class WaveReader : IDisposable {
#endif
        private int m_channel;
        private int m_byte_per_sample;
        private bool m_opened;
        private RandomAccessFile m_stream;
        private int m_total_samples;
        private double m_amplify_left = 1.0;
        private double m_amplify_right = 1.0;
        /// <summary>
        ///  ファイル先頭から，dataチャンクまでのオフセット
        /// </summary>
        private int m_header_offset = 0x2e;
        private Object m_tag = null;
        private double m_offset_seconds = 0.0;
        private int m_sample_per_sec;
        private String m_file = "";

        /*
         * 192000
         * 96000
         * 88200
         * 48000
         * 44100
         * 32000
         * 24000
         * 22050
         * 16000
         * 12000
         * 11025
         * 8000
         */

        public WaveReader() {
            m_opened = false;
        }

        public WaveReader( String file ) 
#if JAVA
            throws IOException, FileNotFoundException
#endif
 {
            bool ret = open( file );
            m_file = file;
        }

        public String getFilePath() {
            return m_file;
        }

        public int getSampleRate() {
            return m_sample_per_sec;
        }

        public double getOffsetSeconds() {
            return m_offset_seconds;
        }

        public void setOffsetSeconds( double value ) {
            m_offset_seconds = value;
        }

        public Object getTag() {
            return m_tag;
        }

        public void setTag( Object value ) {
            m_tag = value;
        }

        public double getAmplifyLeft() {
            return m_amplify_left;
        }

        public void setAmplifyLeft( double value ) {
            m_amplify_left = value;
        }

        public double getAmplifyRight() {
            return m_amplify_right;
        }

        public void setAmplifyRight( double value ) {
            m_amplify_right = value;
        }

#if !JAVA
        public void Dispose() {
            close();
        }
#endif

        public bool open( String file )
#if JAVA
            throws IOException, FileNotFoundException
#endif
        {
#if DEBUG
            sout.println( "WaveReader#open; file=" + file );
#endif
            if ( m_opened ) {
                m_stream.close();
            }
            m_stream = new RandomAccessFile( file, "r" );

            // RIFF
            byte[] buf = new byte[4];
            m_stream.read( buf, 0, 4 );
            if ( buf[0] != 'R' || buf[1] != 'I' || buf[2] != 'F' || buf[3] != 'F' ) {
                m_stream.close();
#if DEBUG
                serr.println( "WaveReader#open; header error(RIFF)" );
#endif
                return false;
            }

            // ファイルサイズ - 8最後に記入
            m_stream.read( buf, 0, 4 );

            // WAVE
            m_stream.read( buf, 0, 4 );
            if ( buf[0] != 'W' || buf[1] != 'A' || buf[2] != 'V' || buf[3] != 'E' ) {
                m_stream.close();
#if DEBUG
                serr.println( "WaveReader#open; header error(WAVE)" );
#endif
                return false;
            }

            // fmt 
            m_stream.read( buf, 0, 4 );
            if ( buf[0] != 'f' || buf[1] != 'm' || buf[2] != 't' || buf[3] != ' ' ) {
                m_stream.close();
#if DEBUG
                serr.println( "WaveReader#open; header error(fmt )" );
#endif
                return false;
            }

            // fmt チャンクのサイズ
            m_stream.read( buf, 0, 4 );
            int chunksize = (int)PortUtil.make_uint32_le( buf );
            long fmt_chunk_end_location = m_stream.getFilePointer() + chunksize;

            // format ID
            m_stream.read( buf, 0, 2 );

            // チャンネル数
            m_stream.read( buf, 0, 2 );
            m_channel = buf[1] << 8 | buf[0];
#if DEBUG
            sout.println( "WaveReader#open; m_channel=" + m_channel );
#endif

            // サンプリングレート
            m_stream.read( buf, 0, 4 );
            m_sample_per_sec = (int)PortUtil.make_uint32_le( buf );
#if DEBUG
            sout.println( "WaveReader#open; m_sample_per_sec=" + m_sample_per_sec );
#endif

            // データ速度
            m_stream.read( buf, 0, 4 );

            // ブロックサイズ
            m_stream.read( buf, 0, 2 );

            // サンプルあたりのビット数
            m_stream.read( buf, 0, 2 );
            int bit_per_sample = buf[1] << 8 | buf[0];
            m_byte_per_sample = bit_per_sample / 8;
#if DEBUG
            sout.println( "WaveReader#open; m_byte_per_sample=" + m_byte_per_sample );
#endif

            // 拡張部分
            m_stream.seek( fmt_chunk_end_location );
            //m_stream.Read( buf, 0, 2 );

            // data
            m_stream.read( buf, 0, 4 );
            if ( buf[0] != 'd' || buf[1] != 'a' || buf[2] != 't' || buf[3] != 'a' ) {
                m_stream.close();
#if DEBUG
                serr.println( "WaveReader#open; header error (data)" );
#endif
                return false;
            }

            // size of data chunk
            m_stream.read( buf, 0, 4 );
            int size = (int)PortUtil.make_uint32_le( buf );
            m_total_samples = size / (m_channel * m_byte_per_sample);
#if DEBUG
            sout.println( "WaveReader#open; m_total_samples=" + m_total_samples + "; total sec=" + (m_total_samples / (double)m_sample_per_sec) );
#endif

            m_opened = true;
            m_header_offset = (int)m_stream.getFilePointer();
            return true;
        }

        public int getTotalSamples() {
            return m_total_samples;
        }

        public void read( long start, int length, double[] left, double[] right ) 
#if JAVA
            throws IOException
#endif
        {
            for ( int i = 0; i < length; i++ ) {
                left[i] = 0.0;
                right[i] = 0.0;
            }
            //left = new double[length];
            //right = new double[length];
            if ( !m_opened ) {
                return;
            }
            int i_start = 0;
            int i_end = length - 1;
            long required_sample_start = start + (long)(m_offset_seconds * m_sample_per_sec);
            long required_sample_end = required_sample_start + length;
            // 第required_sample_startサンプルから，第required_sample_endサンプルまでの読み込みが要求された．
            if ( required_sample_start < 0 ) {
                i_start = -(int)required_sample_start + 1;
                // 0 -> i_start - 1までは0で埋める
                if ( i_start >= length ) {
                    // 全部0で埋める必要のある場合.
                    for ( int i = 0; i < length; i++ ) {
                        left[i] = 0.0;
                        right[i] = 0.0;
                    }
                    return;
                } else {
                    for ( int i = 0; i < i_start; i++ ) {
                        left[i] = 0.0;
                        right[i] = 0.0;
                    }
                }
                m_stream.seek( m_header_offset );
            } else {
                long loc = m_header_offset + m_byte_per_sample * m_channel * required_sample_start;
                m_stream.seek( loc );
            }
            if ( m_total_samples < required_sample_end ) {
                i_end = length - 1 - (int)required_sample_end + m_total_samples;
                // i_end + 1 -> length - 1までは0で埋める
                if ( i_end < 0 ) {
                    // 全部0で埋める必要のある場合
                    for ( int i = 0; i < length; i++ ) {
                        left[i] = 0.0;
                        right[i] = 0.0;
                    }
                    return;
                } else {
                    for ( int i = i_end + 1; i < length; i++ ) {
                        left[i] = 0.0;
                        right[i] = 0.0;
                    }
                }
            }

            if ( m_byte_per_sample == 2 ) {
                if ( m_channel == 2 ) {
                    byte[] buf = new byte[4];
                    double coeff_left = m_amplify_left / 32768.0;
                    double coeff_right = m_amplify_right / 32768.0;
                    for ( int i = i_start; i <= i_end; i++ ) {
                        int ret = m_stream.read( buf, 0, 4 );
                        if ( ret < 4 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        short l = PortUtil.make_int16_le( buf, 0 );
                        short r = PortUtil.make_int16_le( buf, 2 );
                        left[i] = l * coeff_left;
                        right[i] = r * coeff_right;
                    }
                } else {
                    byte[] buf = new byte[2];
                    double coeff_left = m_amplify_left / 32768.0;
                    for ( int i = i_start; i <= i_end; i++ ) {
                        int ret = m_stream.read( buf, 0, 2 );
                        if ( ret < 2 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        short l = PortUtil.make_int16_le( buf, 0 );
                        left[i] = l * coeff_left;
                        right[i] = left[i];
                    }
                }
            } else {
                if ( m_channel == 2 ) {
                    byte[] buf = new byte[2];
                    double coeff_left = m_amplify_left / 64.0;
                    double coeff_right = m_amplify_right / 64.0;
                    for ( int i = i_start; i <= i_end; i++ ) {
                        int ret = m_stream.read( buf, 0, 2 );
                        if ( ret < 2 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        left[i] = ((0xff & buf[0]) - 64.0f) * coeff_left;
                        right[i] = ((0xff & buf[1]) - 64.0f) * coeff_right;
                    }
                } else {
                    byte[] buf = new byte[1];
                    double coeff_left = m_amplify_left / 64.0;
                    for ( int i = i_start; i <= i_end; i++ ) {
                        int ret = m_stream.read( buf, 0, 1 );
                        if ( ret < 1 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        left[i] = ((0xff & buf[0]) - 64.0f) * coeff_left;
                        right[i] = left[i];
                    }
                }
            }
        }

        public void read( long start, int length, ByRef<float[]> left, ByRef<float[]> right ) 
#if JAVA
            throws IOException
#endif
        {
            left.value = new float[length];
            right.value = new float[length];
            if ( !m_opened ) {
                return;
            }
            int i_start = 0;
            int i_end = length;
            long required_sample_start = start + (long)(m_offset_seconds * m_sample_per_sec);
            long required_sample_end = required_sample_start + length;
            // 第required_sample_startサンプルから，第required_sample_endサンプルまでの読み込みが要求された．
            if ( required_sample_start < 0 ) {
                i_start = -(int)required_sample_start + 1;
                // 0 -> i_start - 1までは0で埋める
                for ( int i = 0; i < i_start; i++ ) {
                    left.value[i] = 0.0f;
                    right.value[i] = 0.0f;
                }
                m_stream.seek( m_header_offset );
            } else {
                long loc = m_header_offset + m_byte_per_sample * m_channel * required_sample_start;
                m_stream.seek( loc );
            }
            if ( m_total_samples < required_sample_end ) {
                i_end = length - 1 - (int)required_sample_end + m_total_samples;
                // i_end + 1 -> length - 1までは0で埋める
                for ( int i = i_end + 1; i < length; i++ ) {
                    left.value[i] = 0.0f;
                    right.value[i] = 0.0f;
                }
            }

            if ( m_byte_per_sample == 2 ) {
                if ( m_channel == 2 ) {
                    byte[] buf = new byte[4];
                    float coeff_left = (float)(m_amplify_left / 32768.0f);
                    float coeff_right = (float)(m_amplify_right / 32768.0f);
                    for ( int i = 0; i < length; i++ ) {
                        int ret = m_stream.read( buf, 0, 4 );
                        if ( ret < 4 ) {
                            for ( int j = i; j < length; j++ ) {
                                left.value[j] = 0.0f;
                                right.value[j] = 0.0f;
                            }
                            break;
                        }
                        short l = PortUtil.make_int16_le( buf, 0 );
                        short r = PortUtil.make_int16_le( buf, 2 );
                        left.value[i] = l * coeff_left;
                        right.value[i] = r * coeff_right;
                    }
                } else {
                    byte[] buf = new byte[2];
                    float coeff_left = (float)(m_amplify_left / 32768.0f);
                    for ( int i = 0; i < length; i++ ) {
                        int ret = m_stream.read( buf, 0, 2 );
                        if ( ret < 2 ) {
                            for ( int j = i; j < length; j++ ) {
                                left.value[j] = 0.0f;
                                right.value[j] = 0.0f;
                            }
                            break;
                        }
                        short l = PortUtil.make_int16_le( buf, 0 );
                        left.value[i] = l * coeff_left;
                        right.value[i] = left.value[i];
                    }
                }
            } else {
                if ( m_channel == 2 ) {
                    byte[] buf = new byte[2];
                    float coeff_left = (float)(m_amplify_left / 64.0f);
                    float coeff_right = (float)(m_amplify_right / 64.0f);
                    for ( int i = 0; i < length; i++ ) {
                        int ret = m_stream.read( buf, 0, 2 );
                        if ( ret < 2 ) {
                            for ( int j = i; j < length; j++ ) {
                                left.value[j] = 0.0f;
                                right.value[j] = 0.0f;
                            }
                            break;
                        }
                        left.value[i] = ((0xff & buf[0]) - 64.0f) * coeff_left;
                        right.value[i] = ((0xff & buf[1]) - 64.0f) * coeff_right;
                    }
                } else {
                    byte[] buf = new byte[1];
                    float coeff_left = (float)(m_amplify_left / 64.0f);
                    for ( int i = 0; i < length; i++ ) {
                        int ret = m_stream.read( buf, 0, 1 );
                        if ( ret < 1 ) {
                            for ( int j = i; j < length; j++ ) {
                                left.value[j] = 0.0f;
                                right.value[j] = 0.0f;
                            }
                            break;
                        }
                        left.value[i] = ((0xff & buf[0]) - 64.0f) * coeff_left;
                        right.value[i] = left.value[i];
                    }
                }
            }
        }

#if !JAVA
        public unsafe void read( long start, int length, ref IntPtr ptr_left, ref IntPtr ptr_right ) {
            float* left = (float*)ptr_left.ToPointer();
            float* right = (float*)ptr_right.ToPointer();
            if ( !m_opened ) {
                return;
            }
            int i_start = 0;
            int i_end = length;
            long required_sample_start = start + (long)(m_offset_seconds * m_sample_per_sec);
            long required_sample_end = required_sample_start + length;
            // 第required_sample_startサンプルから，第required_sample_endサンプルまでの読み込みが要求された．
            if ( required_sample_start < 0 ) {
                i_start = -(int)required_sample_start + 1;
                // 0 -> i_start - 1までは0で埋める
                for ( int i = 0; i < i_start; i++ ) {
                    left[i] = 0.0f;
                    right[i] = 0.0f;
                }
                m_stream.seek( m_header_offset );
            } else {
                long loc = m_header_offset + m_byte_per_sample * m_channel * required_sample_start;
                m_stream.seek( loc );
            }
            if ( m_total_samples < required_sample_end ) {
                i_end = length - 1 - (int)required_sample_end + m_total_samples;
                // i_end + 1 -> length - 1までは0で埋める
                for ( int i = i_end + 1; i < length; i++ ) {
                    left[i] = 0.0f;
                    right[i] = 0.0f;
                }
            }

            if ( m_byte_per_sample == 2 ) {
                if ( m_channel == 2 ) {
                    byte[] buf = new byte[4];
                    float coeff_left = (float)(m_amplify_left / 32768.0f);
                    float coeff_right = (float)(m_amplify_right / 32768.0f);
                    for ( int i = 0; i < length; i++ ) {
                        int ret = m_stream.read( buf, 0, 4 );
                        if ( ret < 4 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        short l = PortUtil.make_int16_le( buf, 0 );
                        short r = PortUtil.make_int16_le( buf, 2 );
                        left[i] = l * coeff_left;
                        right[i] = r * coeff_right;
                    }
                } else {
                    byte[] buf = new byte[2];
                    float coeff_left = (float)(m_amplify_left / 32768.0f);
                    for ( int i = 0; i < length; i++ ) {
                        int ret = m_stream.read( buf, 0, 2 );
                        if ( ret < 2 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        short l = PortUtil.make_int16_le( buf, 0 );
                        left[i] = l * coeff_left;
                        right[i] = left[i];
                    }
                }
            } else {
                if ( m_channel == 2 ) {
                    byte[] buf = new byte[2];
                    float coeff_left = (float)(m_amplify_left / 64.0f);
                    float coeff_right = (float)(m_amplify_right / 64.0f);
                    for ( int i = 0; i < length; i++ ) {
                        int ret = m_stream.read( buf, 0, 2 );
                        if ( ret < 2 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        left[i] = ((0xff & buf[0]) - 64.0f) * coeff_left;
                        right[i] = ((0xff & buf[1]) - 64.0f) * coeff_right;
                    }
                } else {
                    byte[] buf = new byte[1];
                    float coeff_left = (float)(m_amplify_left / 64.0f);
                    for ( int i = 0; i < length; i++ ) {
                        int ret = m_stream.read( buf, 0, 1 );
                        if ( ret < 1 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        left[i] = ((0xff & buf[0]) - 64.0f) * coeff_left;
                        right[i] = left[i];
                    }
                }
            }
        }
#endif

        public void close()
#if JAVA
            throws IOException
#endif
        {
#if DEBUG
            sout.println( "WaveReader#close; m_file=" + m_file );
#endif
            m_opened = false;
            if ( m_stream != null ) {
                m_stream.close();
                m_stream = null;
            }
        }

#if !JAVA
        ~WaveReader() {
            close();
        }
#endif
    }

#if !JAVA
}
#endif
