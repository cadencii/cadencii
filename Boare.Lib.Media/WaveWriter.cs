/*
 * WaveWriter.cs
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

namespace Boare.Lib.Media
{

    public class WaveWriter : IDisposable
    {
        private Wave.WaveChannel m_channel;
        private ushort m_bit_per_sample;
        private uint m_sample_rate;
        private uint m_total_samples = 0;
        private FileStream m_stream = null;
        private string m_path = "";

        public WaveWriter( string path ) :
            this( path, Wave.WaveChannel.Stereo, 16, 44100 )
        {
        }

        public WaveWriter( string path, Wave.WaveChannel channel, ushort bit_per_sample, uint sample_rate )
        {
            m_path = path;
            m_stream = new FileStream( m_path, FileMode.Create, FileAccess.Write );
            m_channel = channel;
            m_bit_per_sample = bit_per_sample;
            m_sample_rate = sample_rate;
            m_total_samples = 0;
            WriteHeader();
        }

        /// <summary>
        /// Writes header of WAVE file
        /// </summary>
        private void WriteHeader()
        {
            // RIFF
            m_stream.WriteByte( 0x52 ); // loc=0x00
            m_stream.WriteByte( 0x49 );
            m_stream.WriteByte( 0x46 );
            m_stream.WriteByte( 0x46 );

            // ファイルサイズ - 8最後に記入
            m_stream.WriteByte( 0x00 ); // loc=0x04
            m_stream.WriteByte( 0x00 );
            m_stream.WriteByte( 0x00 );
            m_stream.WriteByte( 0x00 );

            // WAVE
            m_stream.WriteByte( 0x57 ); // loc=0x08
            m_stream.WriteByte( 0x41 );
            m_stream.WriteByte( 0x56 );
            m_stream.WriteByte( 0x45 );

            // fmt 
            m_stream.WriteByte( 0x66 ); // loc=0x0c
            m_stream.WriteByte( 0x6d );
            m_stream.WriteByte( 0x74 );
            m_stream.WriteByte( 0x20 );

            // fmt チャンクのサイズ
            m_stream.WriteByte( 0x12 ); // loc=0x10
            m_stream.WriteByte( 0x00 );
            m_stream.WriteByte( 0x00 );
            m_stream.WriteByte( 0x00 );

            // format ID
            m_stream.WriteByte( 0x01 ); // loc=0x14
            m_stream.WriteByte( 0x00 );

            // チャンネル数
            if ( m_channel == Wave.WaveChannel.Monoral )
            {
                m_stream.WriteByte( 0x01 ); // loc=0x16
                m_stream.WriteByte( 0x00 );
            }
            else
            {
                m_stream.WriteByte( 0x02 ); //loc=0x16
                m_stream.WriteByte( 0x00 );
            }

            // サンプリングレート
            byte[] buf = BitConverter.GetBytes( m_sample_rate );
            WriteByteArray( m_stream, buf, 4 ); // 0x18

            // データ速度
            int ichannel = m_channel == Wave.WaveChannel.Stereo ? 2 : 1;
            ushort block_size = (ushort)(m_bit_per_sample / 8 * ichannel);
            uint data_rate = m_sample_rate * block_size;
            buf = BitConverter.GetBytes( data_rate );
            WriteByteArray( m_stream, buf, 4 );//loc=0x1c

            // ブロックサイズ
            buf = BitConverter.GetBytes( block_size );
            WriteByteArray( m_stream, buf, 2 ); //0x20

            // サンプルあたりのビット数
            buf = BitConverter.GetBytes( m_bit_per_sample );
            WriteByteArray( m_stream, buf, 2 ); //loc=0x22

            // 拡張部分
            m_stream.WriteByte( 0x00 ); //loc=0x24
            m_stream.WriteByte( 0x00 );

            // data
            m_stream.WriteByte( 0x64 ); //loc=0x26
            m_stream.WriteByte( 0x61 );
            m_stream.WriteByte( 0x74 );
            m_stream.WriteByte( 0x61 );

            // size of data chunk
            uint size = block_size * m_total_samples;
            buf = BitConverter.GetBytes( size );
            WriteByteArray( m_stream, buf, 4 );
        }

        public void Close()
        {
            if ( m_stream != null )
            {
                // 最後にWAVEチャンクのサイズ
                uint position = (uint)m_stream.Position;
                m_stream.Seek( 4, SeekOrigin.Begin );
                byte[] buf = BitConverter.GetBytes( position - 8 );
                WriteByteArray( m_stream, buf, 4 );

                // size of data chunk
                int ichannel = m_channel == Wave.WaveChannel.Stereo ? 2 : 1;
                ushort block_size = (ushort)(m_bit_per_sample / 8 * ichannel);
                uint size = block_size * m_total_samples;
                m_stream.Seek( 42, SeekOrigin.Begin );
                buf = BitConverter.GetBytes( size );
                WriteByteArray( m_stream, buf, 4 );

                m_stream.Close();
            }
        }

        public uint SampleRate
        {
            get
            {
                return m_sample_rate;
            }
        }

        public void Dispose()
        {
            Close();
        }

        public void Append( float[] L )
        {
            int total = L.Length;
            if ( m_bit_per_sample == 8 )
            {
                if ( m_channel == Wave.WaveChannel.Monoral )
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        m_stream.WriteByte( (byte)((L[i] + 1.0f) * 127.5f) );
                    }
                }
                else
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        byte b = (byte)((L[i] + 1.0f) * 127.5f);
                        m_stream.WriteByte( b );
                        m_stream.WriteByte( b );
                    }
                }
            }
            else
            {
                byte[] buf;
                if ( m_channel == Wave.WaveChannel.Monoral )
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        buf = BitConverter.GetBytes( (short)(L[i] * 32768f) );
                        WriteByteArray( m_stream, buf, 2 );
                    }
                }
                else
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        buf = BitConverter.GetBytes( (short)(L[i] * 32768f) );
                        WriteByteArray( m_stream, buf, 2 );
                        WriteByteArray( m_stream, buf, 2 );
                    }
                }
            }
            m_total_samples += (uint)total;
        }

        public void Append( double[] L )
        {
            int total = L.Length;
            if ( m_bit_per_sample == 8 )
            {
                if ( m_channel == Wave.WaveChannel.Monoral )
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        m_stream.WriteByte( (byte)((L[i] + 1.0) * 127.5) );
                    }
                }
                else
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        byte b = (byte)((L[i] + 1.0) * 127.5);
                        m_stream.WriteByte( b );
                        m_stream.WriteByte( b );
                    }
                }
            }
            else
            {
                byte[] buf;
                if ( m_channel == Wave.WaveChannel.Monoral )
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        buf = BitConverter.GetBytes( (short)(L[i] * 32768.0) );
                        WriteByteArray( m_stream, buf, 2 );
                    }
                }
                else
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        buf = BitConverter.GetBytes( (short)(L[i] * 32768.0) );
                        WriteByteArray( m_stream, buf, 2 );
                        WriteByteArray( m_stream, buf, 2 );
                    }
                }
            }
            m_total_samples += (uint)total;
        }

        public void Append( float[] L, float[] R )
        {
            int total = Math.Min( L.Length, R.Length );
            if ( m_bit_per_sample == 8 )
            {
                if ( m_channel == Wave.WaveChannel.Monoral )
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        m_stream.WriteByte( (byte)((L[i] + R[i] + 2.0f) * 63.75f) );
                    }
                }
                else
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        m_stream.WriteByte( (byte)((L[i] + 1.0f) * 127.5f) );
                        m_stream.WriteByte( (byte)((R[i] + 1.0f) * 127.5f) );
                    }
                }
            }
            else
            {
                byte[] buf;
                if ( m_channel == Wave.WaveChannel.Monoral )
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        buf = BitConverter.GetBytes( (short)((L[i] + R[i]) * 16384f) );
                        WriteByteArray( m_stream, buf, 2 );
                    }
                }
                else
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        buf = BitConverter.GetBytes( (short)(L[i] * 32768f) );
                        WriteByteArray( m_stream, buf, 2 );
                        buf = BitConverter.GetBytes( (short)(R[i] * 32768f) );
                        WriteByteArray( m_stream, buf, 2 );
                    }
                }
            }
            m_total_samples += (uint)total;
        }

        public unsafe void Append( float* L, float* R, int length )
        {
            if ( m_bit_per_sample == 8 )
            {
                if ( m_channel == Wave.WaveChannel.Monoral )
                {
                    for ( int i = 0; i < length; i++ )
                    {
                        m_stream.WriteByte( (byte)((L[i] + R[i] + 2.0f) * 63.75f) );
                    }
                }
                else
                {
                    for ( int i = 0; i < length; i++ )
                    {
                        m_stream.WriteByte( (byte)((L[i] + 1.0f) * 127.5f) );
                        m_stream.WriteByte( (byte)((R[i] + 1.0f) * 127.5f) );
                    }
                }
            }
            else
            {
                byte[] buf;
                if ( m_channel == Wave.WaveChannel.Monoral )
                {
                    for ( int i = 0; i < length; i++ )
                    {
                        buf = BitConverter.GetBytes( (short)((L[i] + R[i]) * 16384f) );
                        WriteByteArray( m_stream, buf, 2 );
                    }
                }
                else
                {
                    for ( int i = 0; i < length; i++ )
                    {
                        buf = BitConverter.GetBytes( (short)(L[i] * 32768f) );
                        WriteByteArray( m_stream, buf, 2 );
                        buf = BitConverter.GetBytes( (short)(R[i] * 32768f) );
                        WriteByteArray( m_stream, buf, 2 );
                    }
                }
            }
            m_total_samples += (uint)length;
        }

        public void Append( double[] L, double[] R )
        {
            int total = Math.Min( L.Length, R.Length );
#if DEBUG
            Console.WriteLine( "WaveWriter#Append(double[], double[]); total=" + total );
#endif
            if ( m_bit_per_sample == 8 )
            {
                if ( m_channel == Wave.WaveChannel.Monoral )
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        m_stream.WriteByte( (byte)((L[i] + R[i] + 2.0) * 63.75) );
                    }
                }
                else
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        m_stream.WriteByte( (byte)((L[i] + 1.0) * 127.5) );
                        m_stream.WriteByte( (byte)((R[i] + 1.0) * 127.5) );
                    }
                }
            }
            else
            {
                byte[] buf;
                if ( m_channel == Wave.WaveChannel.Monoral )
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        buf = BitConverter.GetBytes( (short)((L[i] + R[i]) * 16384.0) );
                        WriteByteArray( m_stream, buf, 2 );
                    }
                }
                else
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        buf = BitConverter.GetBytes( (short)(L[i] * 32768.0) );
                        WriteByteArray( m_stream, buf, 2 );
                        buf = BitConverter.GetBytes( (short)(R[i] * 32768.0) );
                        WriteByteArray( m_stream, buf, 2 );
                    }
                }
            }
            m_total_samples += (uint)total;
        }

        public void Append( byte[] L, byte[] R )
        {
            int total = Math.Min( L.Length, R.Length );
            if ( m_bit_per_sample == 8 )
            {
                if ( m_channel == Wave.WaveChannel.Monoral )
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        m_stream.WriteByte( (byte)((L[i] + R[i]) / 2) );
                    }
                }
                else
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        m_stream.WriteByte( L[i] );
                        m_stream.WriteByte( R[i] );
                    }
                }
            }
            else
            {
                byte[] buf;
                if ( m_channel == Wave.WaveChannel.Monoral )
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        buf = BitConverter.GetBytes( (short)((L[i] + R[i]) * 128.5f - 32768f) );
                        WriteByteArray( m_stream, buf, 2 );
                    }
                }
                else
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        buf = BitConverter.GetBytes( (short)(L[i] * 257f - 32768f) );
                        WriteByteArray( m_stream, buf, 2 );
                        buf = BitConverter.GetBytes( (short)(R[i] * 257f - 32768f) );
                        WriteByteArray( m_stream, buf, 2 );
                    }
                }
            }
            m_total_samples += (uint)total;
        }

        public void Append( short[] L, short[] R )
        {
            int total = Math.Min( L.Length, R.Length );
            if ( m_bit_per_sample == 8 )
            {
                if ( m_channel == Wave.WaveChannel.Monoral )
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        m_stream.WriteByte( (byte)(((L[i] + R[i]) / 2f + 32768f) / 255f) );
                    }
                }
                else
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        m_stream.WriteByte( (byte)((L[i] + 32768f) / 255f) );
                        m_stream.WriteByte( (byte)((R[i] + 32768f) / 255f) );
                    }
                }
            }
            else
            {
                byte[] buf;
                if ( m_channel == Wave.WaveChannel.Monoral )
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        buf = BitConverter.GetBytes( (L[i] + R[i]) / 2 );
                        WriteByteArray( m_stream, buf, 2 );
                    }
                }
                else
                {
                    for ( int i = 0; i < total; i++ )
                    {
                        buf = BitConverter.GetBytes( L[i] );
                        WriteByteArray( m_stream, buf, 2 );
                        buf = BitConverter.GetBytes( R[i] );
                        WriteByteArray( m_stream, buf, 2 );
                    }
                }
            }
            m_total_samples += (uint)total;
        }

        private static void WriteByteArray( FileStream fs, byte[] dat, int limit )
        {
            fs.Write( dat, 0, (dat.Length > limit) ? limit : dat.Length );
            if ( dat.Length < limit )
            {
                for ( int i = 0; i < limit - dat.Length; i++ )
                {
                    fs.WriteByte( 0x00 );
                }
            }
        }
    }

}