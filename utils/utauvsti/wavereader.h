/*
 * wavereader.h
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
#ifndef __wavereader_h__
#define __wavereader_h__

#include <iostream>
#include <fstream>

using namespace std;

class wavereader{
public:
    ~wavereader();
    wavereader();
#ifdef __cplusplus_cli
    int open( wchar_t *file );
#else
    int open( const char *file );
#endif
    void read( __int64 start, __int64 length, float *left, float *right );
    void read( __int64 start, __int64 length, float *data );
    void close();
private:
    int m_channel;
    int m_byte_per_sample;
    bool m_opened;
    ifstream m_stream;
    int m_total_samples;
    int m_loc_data; //dataチャンクのデータ部分の開始位置
};

#endif // __wavereader_h__
