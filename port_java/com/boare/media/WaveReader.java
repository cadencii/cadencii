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
package com.boare.media;

import java.io.*;

public class WaveReader{
    private int m_channel;
    private int m_byte_per_sample;
    private boolean m_opened;
    private RandomAccessFile m_stream;
    private int m_total_samples;

    public WaveReader() {
        m_opened = false;
    }

    public WaveReader( String file ) throws IOException{
        open( file );
    }

    public boolean open( String file ) throws IOException{
        if ( m_opened ) {
            m_stream.close();
        }
        m_stream = new RandomAccessFile( file, "rw" );

        // RIFF
        byte[] buf = new byte[4];
        m_stream.read( buf, 0, 4 );
        if ( (0xff & buf[0]) != (int)'R' || (0xff & buf[1]) != (int)'I' || (0xff & buf[2]) != (int)'F' || (0xff & buf[3]) != (int)'F' ) {
            m_stream.close();
            return false;
        }

        // ファイルサイズ - 8最後に記入
        m_stream.read( buf, 0, 4 );

        // WAVE
        m_stream.read( buf, 0, 4 );
        if ( (0xff & buf[0]) != (int)'W' || (0xff & buf[1]) != (int)'A' || (0xff & buf[2]) != (int)'V' || (0xff & buf[3]) != (int)'E' ) {
            m_stream.close();
            return false;
        }

        // fmt 
        m_stream.read( buf, 0, 4 );
        if ( (0xff & buf[0]) != (int)'f' || (0xff & buf[1]) != (int)'m' || (0xff & buf[2]) != (int)'t' || (0xff & buf[3]) != (int)' ' ) {
            m_stream.close();
            return false;
        }

        // fmt チャンクのサイズ
        m_stream.read( buf, 0, 4 );

        // format ID
        m_stream.read( buf, 0, 2 );

        // チャンネル数
        m_stream.read( buf, 0, 2 );
        m_channel = buf[1] << 8 | buf[0];

        // サンプリングレート
        m_stream.read( buf, 0, 4 );

        // データ速度
        m_stream.read( buf, 0, 4 );

        // ブロックサイズ
        m_stream.read( buf, 0, 2 );

        // サンプルあたりのビット数
        m_stream.read( buf, 0, 2 );
        int bit_per_sample = (0xff & buf[1]) << 8 | (0xff & buf[0]);
        m_byte_per_sample = bit_per_sample / 8;

        // 拡張部分
        m_stream.read( buf, 0, 2 );

        // data
        m_stream.read( buf, 0, 4 );
        if ( (0xff & buf[0]) != (int)'d' || (0xff & buf[1]) != (int)'a' || (0xff & buf[2]) != (int)'t' || (0xff & buf[3]) != (int)'a' ) {
            m_stream.close();
            return false;
        }

        // size of data chunk
        m_stream.read( buf, 0, 4 );
        int size = BitConverter.toInt32( buf, 0 );
        m_total_samples = size / (m_channel * m_byte_per_sample);

        m_opened = true;
        return true;
    }

    public int getTotalSamples(){
        return m_total_samples;
    }

    public void read( long start, int length, double[] left, double[] right ) throws IOException{
        if ( !m_opened ) {
            return;
        }
        long loc = 0x2e + m_byte_per_sample * m_channel * start;
        m_stream.seek( loc );

        if ( m_byte_per_sample == 2 ) {
            if ( m_channel == 2 ) {
                byte[] buf = new byte[4];
                for ( int i = 0; i < length; i++ ) {
                    int ret = m_stream.read( buf, 0, 4 );
                    if ( ret < 4 ) {
                        for ( int j = i; j < length; j++ ) {
                            left[j] = 0.0f;
                            right[j] = 0.0f;
                        }
                        break;
                    }
                    short l = (short)((0xff & buf[0]) | (0xff & buf[1]) << 8);
                    short r = (short)((0xff & buf[2]) | (0xff & buf[3]) << 8);
                    left[i] = l / 32768.0f;
                    right[i] = r / 32768.0f;
                }
            } else {
                byte[] buf = new byte[2];
                for ( int i = 0; i < length; i++ ) {
                    int ret = m_stream.read( buf, 0, 2 );
                    if ( ret < 2 ) {
                        for ( int j = i; j < length; j++ ) {
                            left[j] = 0.0f;
                            right[j] = 0.0f;
                        }
                        break;
                    }
                    short l = (short)((0xff & buf[0]) | (0xff & buf[1]) << 8);
                    left[i] = l / 32768.0f;
                    right[i] = left[i];
                }
            }
        } else {
            if ( m_channel == 2 ) {
                byte[] buf = new byte[2];
                for ( int i = 0; i < length; i++ ) {
                    int ret = m_stream.read( buf, 0, 2 );
                    if ( ret < 2 ) {
                        for ( int j = i; j < length; j++ ) {
                            left[j] = 0.0f;
                            right[j] = 0.0f;
                        }
                        break;
                    }
                    left[i] = (buf[0] - 64.0f) / 64.0f;
                    right[i] = (buf[1] - 64.0f) / 64.0f;
                }
            } else {
                byte[] buf = new byte[1];
                for ( int i = 0; i < length; i++ ) {
                    int ret = m_stream.read( buf, 0, 1 );
                    if ( ret < 1 ) {
                        for ( int j = i; j < length; j++ ) {
                            left[j] = 0.0f;
                            right[j] = 0.0f;
                        }
                        break;
                    }
                    left[i] = (buf[0] - 64.0f) / 64.0f;
                    right[i] = left[i];
                }
            }
        }
    }

    public void read( long start, int length, float[] left, float[] right ) throws IOException{
        if ( !m_opened ) {
            return;
        }
        long loc = 0x2e + m_byte_per_sample * m_channel * start;
        m_stream.seek( loc );

        if ( m_byte_per_sample == 2 ) {
            if ( m_channel == 2 ) {
                byte[] buf = new byte[4];
                for ( int i = 0; i < length; i++ ) {
                    int ret = m_stream.read( buf, 0, 4 );
                    if ( ret < 4 ) {
                        for ( int j = i; j < length; j++ ) {
                            left[j] = 0.0f;
                            right[j] = 0.0f;
                        }
                        break;
                    }
                    short l = (short)((0xff & buf[0]) | (0xff & buf[1]) << 8);
                    short r = (short)((0xff & buf[2]) | (0xff & buf[3]) << 8);
                    left[i] = l / 32768.0f;
                    right[i] = r / 32768.0f;
                }
            } else {
                byte[] buf = new byte[2];
                for ( int i = 0; i < length; i++ ) {
                    int ret = m_stream.read( buf, 0, 2 );
                    if ( ret < 2 ) {
                        for ( int j = i; j < length; j++ ) {
                            left[j] = 0.0f;
                            right[j] = 0.0f;
                        }
                        break;
                    }
                    short l = (short)((0xff & buf[0]) | (0xff & buf[1]) << 8);
                    left[i] = l / 32768.0f;
                    right[i] = left[i];
                }
            }
        } else {
            if ( m_channel == 2 ) {
                byte[] buf = new byte[2];
                for ( int i = 0; i < length; i++ ) {
                    int ret = m_stream.read( buf, 0, 2 );
                    if ( ret < 2 ) {
                        for ( int j = i; j < length; j++ ) {
                            left[j] = 0.0f;
                            right[j] = 0.0f;
                        }
                        break;
                    }
                    left[i] = (buf[0] - 64.0f) / 64.0f;
                    right[i] = (buf[1] - 64.0f) / 64.0f;
                }
            } else {
                byte[] buf = new byte[1];
                for ( int i = 0; i < length; i++ ) {
                    int ret = m_stream.read( buf, 0, 1 );
                    if ( ret < 1 ) {
                        for ( int j = i; j < length; j++ ) {
                            left[j] = 0.0f;
                            right[j] = 0.0f;
                        }
                        break;
                    }
                    left[i] = (buf[0] - 64.0f) / 64.0f;
                    right[i] = left[i];
                }
            }
        }
    }

    public void close() throws IOException{
        m_opened = false;
        if ( m_stream != null ) {
            m_stream.close();
            m_stream = null;
        }
    }
}
