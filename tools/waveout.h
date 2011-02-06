/**
 * WaveOut.h
 * Copyright (C) 2010 kbinani
 *
 * This file is part of WaveOut.
 *
 * WaveOut is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * WaveOut is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#ifndef WAVEOUT_H
#define WAVEOUT_H

// プラットフォームを検出
#if defined(__APPLE__) && (defined(__GNUC__) || defined(__xlC__) || defined(__xlc__))
#  define WAVEOUT_OS_MAC
#elif !defined(SAG_COM) && (defined(WIN64) || defined(_WIN64) || defined(__WIN64__))
#  define WAVEOUT_OS_WIN
#elif !defined(SAG_COM) && (defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(__NT__))
#  define WAVEOUT_OS_WIN
#elif defined(__MWERKS__) && defined(__INTEL__)
#  define WAVEOUT_OS_WIN
#else
#  define WAVEOUT_OS_OTHER
#endif

#ifdef WAVEOUT_OS_WIN
#include <windows.h>
#pragma comment(lib, "winmm.lib")
#elif defined( WAVEOUT_OS_MAC )
#include <CoreAudio/CoreAudioTypes.h>
#include <AudioToolbox/AudioToolbox.h>
#include <AudioToolbox/AudioQueue.h>
#include <stdio.h>
#include <math.h>
#include <pthread.h>
#elif defined( WAVEOUT_OS_OTHER )
#include <fcntl.h>
#include <limits.h>
#include <sys/soundcard.h>
#include <math.h>
#include <stdio.h>
#include <sys/ioctl.h>
#include <unistd.h>
#include <stdlib.h>
#endif

#include <stdio.h>

#ifdef QT_CORE_LIB
#include <Qt>
#endif

#ifdef QT_VERSION
#define dbg qDebug
#else
#define dbg printf
#endif

#define WAVEOUT_NUM_BUF 3

#ifndef MAKELONG
#define MAKELONG(a, b) ((long)(((unsigned short)(a)) | ((unsigned long)((unsigned short)(b))) << 16))
#endif

class WaveOut{
public:
    WaveOut();
    ~WaveOut();
    void    init();
    void    prepare( int sample_rate );
    void    append( double *left, double *right, int length );
    void    exit();
    double  getPosition();
    bool    isBusy();
    void    waitForExit();
    void    setResolution( int resolution );
    void    kill();
    void    unprepare();

private:
#ifdef WAVEOUT_OS_WIN
    static void CALLBACK callback( HWAVEOUT hwo, unsigned int uMsg, unsigned long dwInstance, unsigned long dwParam1, unsigned long dwParam2 );
#elif defined( WAVEOUT_OS_MAC )
    static void callback( void *aqData, AudioQueueRef inAQ, AudioQueueBufferRef inBuffer );
    static void deriveBufferSize(
        AudioStreamBasicDescription &ASBDesc,
        UInt32                      maxPacketSize,
        Float64                     seconds,
        UInt32                      *outBufferSize,
        UInt32                      *outNumPacketsToRead
    );
#elif defined( WAVEOUT_OS_OTHER )
    // /dev/dsp を設定する。
    int setupDsp( int fd );
#endif

private:
#ifdef WAVEOUT_OS_WIN
    HWAVEOUT            mWaveOut;
    WAVEFORMATEX        mWaveFormat;
    WAVEHDR             mWaveHeaders[WAVEOUT_NUM_BUF];
    DWORD               *mWaves[WAVEOUT_NUM_BUF];
    bool                mIsWaveDone[WAVEOUT_NUM_BUF];
    int                 mBufferIndex; // 次のデータを書き込むバッファの番号
    int                 mBufferLoc; // 次のデータを書き込む位置
    CRITICAL_SECTION    mLocker;
    bool                mAbortRequired;
    int                 mBlockSize; // ブロックサイズ
    int                 mBlockSizeUsed; // prepareで初期化されたブロックサイズ
#elif defined( WAVEOUT_OS_MAC )
    AudioStreamBasicDescription     mDataFormat;
    AudioQueueRef                   mQueue;
    AudioQueueBufferRef             mBuffers[WAVEOUT_NUM_BUF];
    UInt32                          bufferByteSize;
    //SInt64                        mCurrentPacket;
    //UInt32                        mNumPacketsToRead;
    AudioStreamPacketDescription    *mPacketDescs;
    bool                            mIsRunning;
    int                             mSampleRate;
    // 排他制御用のmutex
    pthread_mutex_t                 *mMutex;
    // appendで波形を受けるためのバッファ
    double                          *mIncomingWavesL[WAVEOUT_NUM_BUF];
    double                          *mIncomingWavesR[WAVEOUT_NUM_BUF];
    // これまでいくつのIncomingバッファを使ってきたか?
    long                            mIncomingBufferUsed;
    // 現在使っているIncomingバッファの、書き込み位置
    int                             mIncomingBufferLoc;
    //これまでいくつのOutgoingバッファに書き込みをおこなったか
    long                            mOutgoingBufferUsed;
    // 現在使っているOutgoingバッファの読み込み位置
    int                             mOutgoingBufferLoc;
#elif defined( WAVEOUT_OS_OTHER )
    int mFileDescripter;// = 0;
    int mBlockSize;// = 44100;
    int mSampleRate;// = 44100;
    int mIsInitialized;// = 0;
    int mBufferLength;// = 44100;
    unsigned long *mBuffer;
#endif
};

#endif // WAVEOUT_H
