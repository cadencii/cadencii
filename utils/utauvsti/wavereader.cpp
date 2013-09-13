/*
 * wavereader.cpp
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#include "wavereader.h"

wavereader::~wavereader(){
    close();
}

void wavereader::close(){
    if( m_opened ){
        m_stream.close();
        m_opened = false;
    }
}

void wavereader::read( __int64 start, __int64 length, float *data ){
    if( !m_opened ){
        for( int i = 0; i < length; i++ ){
            data[i] = 0.0f;
        }
        return;
    }
    __int64 loc = m_loc_data + m_byte_per_sample * m_channel * start;
    m_stream.seekg( loc );

    if( m_byte_per_sample == 2 ){
        if( m_channel == 2 ){
            unsigned char buf[4];
            for( int i = 0; i < length; i++ ){
                m_stream.read( (char *)buf, 4 );
                if( m_stream.gcount() < 4 ){
                    for( int j = i; j < length; j++ ){
                        data[j] = 0.0f;
                    }
                    break;
                }
                signed short int l = (signed short int)(buf[0] | buf[1] << 8);
                signed short int r = (signed short int)(buf[2] | buf[3] << 8);
                data[i] = ((l / 32768.0f) + (r / 32768.0f)) * 0.5f;
            }
        }else{
            unsigned char buf[2];
            for( int i = 0; i < length; i++ ){
                m_stream.read( (char *)buf, 2 );
                if( m_stream.gcount() < 2 ){
                    for( int j = i; j < length; j++ ){
                        data[j] = 0.0f;
                    }
                    break;
                }
                signed short int l = (signed short int)(buf[0] | buf[1] << 8);
                data[i] = l / 32768.0f;
            }
        }
    }else{
        if( m_channel == 2 ){
            unsigned char buf[2];
            for( int i = 0; i < length; i++ ){
                m_stream.read( (char *)buf, 2 );
                if( m_stream.gcount() < 2 ){
                    for( int j = i; j < length; j++ ){
                        data[j] = 0.0f;
                    }
                    break;
                }
                data[i] = ((buf[0] - 64.0f) / 64.0f + (buf[1] - 64.0f) / 64.0f) * 0.5f;
            }
        }else{
            unsigned char buf[1];
            for( int i = 0; i < length; i++ ){
                m_stream.read( (char *)buf, 1 );
                if( m_stream.gcount() < 1 ){
                    for( int j = i; j < length; j++ ){
                        data[j] = 0.0f;
                    }
                    break;
                }
                data[i] = (buf[0] - 64.0f) / 64.0f;
            }
        }
    }
}

void wavereader::read( __int64 start, __int64 length, float *left, float *right ){
    if( !m_opened ){
        for( int i = 0; i < length; i++ ){
            left[i] = 0.0f;
            right[i] = 0.0f;
        }
        return;
    }
    __int64 loc = m_loc_data + m_byte_per_sample * m_channel * start;
    m_stream.seekg( loc );

    if( m_byte_per_sample == 2 ){
        if( m_channel == 2 ){
            unsigned char buf[4];
            for( int i = 0; i < length; i++ ){
                m_stream.read( (char *)buf, 4 );
                if( m_stream.gcount() < 4 ){
                    for( int j = i; j < length; j++ ){
                        left[j] = 0.0f;
                        right[j] = 0.0f;
                    }
                    break;
                }
                signed short int l = (signed short int)(buf[0] | buf[1] << 8);
                signed short int r = (signed short int)(buf[2] | buf[3] << 8);
                left[i] = l / 32768.0f;
                right[i] = r / 32768.0f;
            }
        }else{
            unsigned char buf[2];
            for( int i = 0; i < length; i++ ){
                m_stream.read( (char *)buf, 2 );
                if( m_stream.gcount() < 2 ){
                    for( int j = i; j < length; j++ ){
                        left[j] = 0.0f;
                        right[j] = 0.0f;
                    }
                    break;
                }
                signed short int l = (signed short int)(buf[0] | buf[1] << 8);
                left[i] = l / 32768.0f;
                right[i] = left[i];
            }
        }
    }else{
        if( m_channel == 2 ){
            unsigned char buf[2];
            for( int i = 0; i < length; i++ ){
                m_stream.read( (char *)buf, 2 );
                if( m_stream.gcount() < 2 ){
                    for( int j = i; j < length; j++ ){
                        left[j] = 0.0f;
                        right[j] = 0.0f;
                    }
                    break;
                }
                left[i] = (buf[0] - 64.0f) / 64.0f;
                right[i] = (buf[1] - 64.0f) / 64.0f;
            }
        }else{
            unsigned char buf[1];
            for( int i = 0; i < length; i++ ){
                m_stream.read( (char *)buf, 1 );
                if( m_stream.gcount() < 1 ){
                    for( int j = i; j < length; j++ ){
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

int wavereader::open( const char *file ){
    if( m_opened ){
        m_stream.close();
    }
    m_stream.open( file, ios::binary );

    // RIFF
    unsigned char buf[4];
    m_stream.read( (char *)buf, 4 * sizeof( unsigned char ) );
    if( buf[0] != 'R' || buf[1] != 'I' || buf[2] != 'F' || buf[3] != 'F' ){
        m_stream.close();
        return -1;
    }

    // ファイルサイズ - 8最後に記入
    m_stream.read( (char *)buf, 4 );

    // WAVE
    m_stream.read( (char *)buf, 4 );
    if( buf[0] != 'W' || buf[1] != 'A' || buf[2] != 'V' || buf[3] != 'E' ){
        m_stream.close();
        return -1;
    }

    // fmt 
    m_stream.read( (char *)buf, 4 );
    if( buf[0] != 'f' || buf[1] != 'm' || buf[2] != 't' || buf[3] != ' ' ){
        m_stream.close();
        return -1;
    }

    // fmt チャンクのサイズ
    m_stream.read( (char *)buf, 4 );
    int loc_fmt_end = m_stream.tellg();
    loc_fmt_end += buf[3] << 24 | buf[2] << 16 | buf[1] << 8 | buf[0];

    // format ID
    m_stream.read( (char *)buf, 2 );

    // チャンネル数
    m_stream.read( (char *)buf, 2 );
    m_channel = buf[1] << 8 | buf[0];

    // サンプリングレート
    m_stream.read( (char *)buf, 4 );

    // データ速度
    m_stream.read( (char *)buf, 4 );

    // ブロックサイズ
    m_stream.read( (char *)buf, 2 );

    // サンプルあたりのビット数
    m_stream.read( (char *)buf, 2 );
    int bit_per_sample = buf[1] << 8 | buf[0];
    m_byte_per_sample = bit_per_sample / 8;

    // 拡張部分
    m_stream.seekg( loc_fmt_end );

    // data
    m_stream.read( (char *)buf, 4 );
    if( buf[0] != 'd' || buf[1] != 'a' || buf[2] != 't' || buf[3] != 'a' ){
        m_stream.close();
        return -1;
    }
    m_loc_data = m_stream.tellg();

    // size of data chunk
    m_stream.read( (char *)buf, 4 );
    int size = buf[3] << 24 | buf[2] << 16 | buf[1] << 8 | buf[0];
    m_total_samples = size / (m_channel * m_byte_per_sample);

    m_opened = true;
    return m_total_samples;
}

wavereader::wavereader(){
    m_opened = false;
}
