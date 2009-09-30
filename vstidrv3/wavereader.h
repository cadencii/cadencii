#ifndef __wavereader_h__
#define __wavereader_h__

#include "stdafx.h"
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
    int open( char *file );
#endif
    void read( __int64 start, __int64 length, float *left, float *right );
    void close();
private:
    int m_channel;
    int m_byte_per_sample;
    bool m_opened;
    ifstream m_stream;
    int m_total_samples;
};

#endif // __wavereader_h__
