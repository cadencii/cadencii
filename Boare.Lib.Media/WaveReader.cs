/*
 * WaveReader.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Lib.Media.
 *
 * Boare.Lib.Media is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Media is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Boare.Lib.Media {

    public class WaveReader : IDisposable{
        private int m_channel;
        private int m_byte_per_sample;
        private bool m_opened;
        private FileStream m_stream;
        private int m_total_samples;
        private double m_amplify_left = 1.0;
        private double m_amplify_right = 1.0;
        /// <summary>
        ///  ファイル先頭から，dataチャンクまでのオフセット
        /// </summary>
        private int m_header_offset = 0x2e;
        private object m_tag = null;
        private double m_offset_seconds = 0.0;
        private int m_sample_per_sec;

        public WaveReader() {
            m_opened = false;
        }

        public WaveReader( string file ) {
            bool ret = Open( file );
#if DEBUG
            Console.WriteLine( "WaveReader#.ctor; file=" + file + "; ret=" + ret );
#endif
        }

        public double OffsetSeconds {
            get {
                return m_offset_seconds;
            }
            set {
                m_offset_seconds = value;
            }
        }

        public object Tag {
            get {
                return m_tag;
            }
            set {
                m_tag = value;
            }
        }

        public double AmplifyLeft {
            get {
                return m_amplify_left;
            }
            set {
                m_amplify_left = value;
            }
        }

        public double AmplifyRight {
            get {
                return m_amplify_right;
            }
            set {
                m_amplify_right = value;
            }
        }

        public void Dispose() {
            Close();
        }

        public bool Open( string file ) {
            if ( m_opened ) {
                m_stream.Close();
            }
            m_stream = new FileStream( file, FileMode.Open, FileAccess.Read );

            // RIFF
            byte[] buf = new byte[4];
            m_stream.Read( buf, 0, 4 );
            if ( buf[0] != 'R' || buf[1] != 'I' || buf[2] != 'F' || buf[3] != 'F' ) {
                m_stream.Close();
#if DEBUG
                Console.WriteLine( "WaveReader#Open; header error(RIFF)" );
#endif
                return false;
            }

            // ファイルサイズ - 8最後に記入
            m_stream.Read( buf, 0, 4 );

            // WAVE
            m_stream.Read( buf, 0, 4 );
            if ( buf[0] != 'W' || buf[1] != 'A' || buf[2] != 'V' || buf[3] != 'E' ) {
                m_stream.Close();
#if DEBUG
                Console.WriteLine( "WaveReader#Open; header error(WAVE)" );
#endif
                return false;
            }

            // fmt 
            m_stream.Read( buf, 0, 4 );
            if ( buf[0] != 'f' || buf[1] != 'm' || buf[2] != 't' || buf[3] != ' ' ) {
                m_stream.Close();
#if DEBUG
                Console.WriteLine( "WaveReader#Open; header error(fmt )" );
#endif
                return false;
            }

            // fmt チャンクのサイズ
            m_stream.Read( buf, 0, 4 );
            int chunksize = BitConverter.ToInt32( buf, 0 );
            long fmt_chunk_end_location = m_stream.Position + chunksize;

            // format ID
            m_stream.Read( buf, 0, 2 );

            // チャンネル数
            m_stream.Read( buf, 0, 2 );
            m_channel = buf[1] << 8 | buf[0];

            // サンプリングレート
            m_stream.Read( buf, 0, 4 );
            m_sample_per_sec = BitConverter.ToInt32( buf, 0 );

            // データ速度
            m_stream.Read( buf, 0, 4 );

            // ブロックサイズ
            m_stream.Read( buf, 0, 2 );

            // サンプルあたりのビット数
            m_stream.Read( buf, 0, 2 );
            int bit_per_sample = buf[1] << 8 | buf[0];
            m_byte_per_sample = bit_per_sample / 8;

            // 拡張部分
            m_stream.Seek( fmt_chunk_end_location, SeekOrigin.Begin );
            //m_stream.Read( buf, 0, 2 );

            // data
            m_stream.Read( buf, 0, 4 );
            if ( buf[0] != 'd' || buf[1] != 'a' || buf[2] != 't' || buf[3] != 'a' ) {
                m_stream.Close();
#if DEBUG
                Console.WriteLine( "WaveReader#Open; header error (data)" );
#endif
                return false;
            }

            // size of data chunk
            m_stream.Read( buf, 0, 4 );
            int size = BitConverter.ToInt32( buf, 0 );
            m_total_samples = size / (m_channel * m_byte_per_sample);

            m_opened = true;
            m_header_offset = (int)m_stream.Position;
            return true;
        }

        public int TotalSamples {
            get {
                return m_total_samples;
            }
        }

        public void Read( long start, int length, double[] left, double[] right ) {
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
                m_stream.Seek( m_header_offset, SeekOrigin.Begin );
            } else {
                long loc = m_header_offset + m_byte_per_sample * m_channel * required_sample_start;
                m_stream.Seek( loc, SeekOrigin.Begin );
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
                        int ret = m_stream.Read( buf, 0, 4 );
                        if ( ret < 4 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        short l = (short)(buf[0] | buf[1] << 8);
                        short r = (short)(buf[2] | buf[3] << 8);
                        left[i] = l * coeff_left;
                        right[i] = r * coeff_right;
                    }
                } else {
                    byte[] buf = new byte[2];
                    double coeff_left = m_amplify_left / 32768.0;
                    for ( int i = i_start; i <= i_end; i++ ) {
                        int ret = m_stream.Read( buf, 0, 2 );
                        if ( ret < 2 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        short l = (short)(buf[0] | buf[1] << 8);
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
                        int ret = m_stream.Read( buf, 0, 2 );
                        if ( ret < 2 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        left[i] = (buf[0] - 64.0f) * coeff_left;
                        right[i] = (buf[1] - 64.0f) * coeff_right;
                    }
                } else {
                    byte[] buf = new byte[1];
                    double coeff_left = m_amplify_left / 64.0;
                    for ( int i = i_start; i <= i_end; i++ ) {
                        int ret = m_stream.Read( buf, 0, 1 );
                        if ( ret < 1 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        left[i] = (buf[0] - 64.0f) * coeff_left;
                        right[i] = left[i];
                    }
                }
            }
        }

        public void Read( long start, int length, out float[] left, out float[] right ) {
            left = new float[length];
            right = new float[length];
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
                m_stream.Seek( m_header_offset, SeekOrigin.Begin );
            } else {
                long loc = m_header_offset + m_byte_per_sample * m_channel * required_sample_start;
                m_stream.Seek( loc, SeekOrigin.Begin );
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
                        int ret = m_stream.Read( buf, 0, 4 );
                        if ( ret < 4 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        short l = (short)(buf[0] | buf[1] << 8);
                        short r = (short)(buf[2] | buf[3] << 8);
                        left[i] = l * coeff_left;
                        right[i] = r * coeff_right;
                    }
                } else {
                    byte[] buf = new byte[2];
                    float coeff_left = (float)(m_amplify_left / 32768.0f);
                    for ( int i = 0; i < length; i++ ) {
                        int ret = m_stream.Read( buf, 0, 2 );
                        if ( ret < 2 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        short l = (short)(buf[0] | buf[1] << 8);
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
                        int ret = m_stream.Read( buf, 0, 2 );
                        if ( ret < 2 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        left[i] = (buf[0] - 64.0f) * coeff_left;
                        right[i] = (buf[1] - 64.0f) * coeff_right;
                    }
                } else {
                    byte[] buf = new byte[1];
                    float coeff_left = (float)(m_amplify_left / 64.0f);
                    for ( int i = 0; i < length; i++ ) {
                        int ret = m_stream.Read( buf, 0, 1 );
                        if ( ret < 1 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        left[i] = (buf[0] - 64.0f) * coeff_left;
                        right[i] = left[i];
                    }
                }
            }
        }

        public unsafe void Read( long start, int length, ref IntPtr ptr_left, ref IntPtr ptr_right ) {
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
                m_stream.Seek( m_header_offset, SeekOrigin.Begin );
            } else {
                long loc = m_header_offset + m_byte_per_sample * m_channel * required_sample_start;
                m_stream.Seek( loc, SeekOrigin.Begin );
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
                        int ret = m_stream.Read( buf, 0, 4 );
                        if ( ret < 4 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        short l = (short)(buf[0] | buf[1] << 8);
                        short r = (short)(buf[2] | buf[3] << 8);
                        left[i] = l * coeff_left;
                        right[i] = r * coeff_right;
                    }
                } else {
                    byte[] buf = new byte[2];
                    float coeff_left = (float)(m_amplify_left / 32768.0f);
                    for ( int i = 0; i < length; i++ ) {
                        int ret = m_stream.Read( buf, 0, 2 );
                        if ( ret < 2 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        short l = (short)(buf[0] | buf[1] << 8);
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
                        int ret = m_stream.Read( buf, 0, 2 );
                        if ( ret < 2 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        left[i] = (buf[0] - 64.0f) * coeff_left;
                        right[i] = (buf[1] - 64.0f) * coeff_right;
                    }
                } else {
                    byte[] buf = new byte[1];
                    float coeff_left = (float)(m_amplify_left / 64.0f);
                    for ( int i = 0; i < length; i++ ) {
                        int ret = m_stream.Read( buf, 0, 1 );
                        if ( ret < 1 ) {
                            for ( int j = i; j < length; j++ ) {
                                left[j] = 0.0f;
                                right[j] = 0.0f;
                            }
                            break;
                        }
                        left[i] = (buf[0] - 64.0f) * coeff_left;
                        right[i] = left[i];
                    }
                }
            }
        }

        public void Close() {
            m_opened = false;
            if ( m_stream != null ) {
                m_stream.Close();
                m_stream = null;
            }
        }

        ~WaveReader() {
            Close();
        }
    }

}
