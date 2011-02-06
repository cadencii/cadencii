/**
 * waveout.cpp
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
#include <io.h>
#include <fcntl.h>
#include "waveout.h"

int main( int argc, char *argv[] )
{
    int ret = _setmode( _fileno( stdin ), _O_BINARY );
    if( ret == -1 ){
        return -1;
    }
    WaveOut wo;
    wo.init();
    wo.prepare( 44100 );
    int c;
    const int BUFLEN = 10240;
    const int CH = 2; // channel
    const int BPS = 2; // bytes per sample
    const int TLEN = BUFLEN / CH / BPS;
    unsigned char buf[BUFLEN];
    int indx = 0;
    double left[TLEN];
    double right[TLEN];
    double i32768 = 1.0 / 32768.0;
    while( (c = getchar()) != EOF ){
        buf[indx] = (unsigned char)c;
        indx++;
        if( indx >= BUFLEN ){
            indx = 0;
            for( int i = 0; i < TLEN; i++ ){
                left[i] = (short)(buf[indx] << 8 | buf[indx + 1]) * i32768;
                right[i] = (short)(buf[indx + 2] << 8 | buf[indx + 3]) * i32768;
                indx += 4;
            }
            wo.append( left, right, TLEN );
            indx = 0;
        }
    }
    return 0;
}

WaveOut::WaveOut(){
#ifdef WAVEOUT_OS_WIN
    mWaveOut = NULL;
    mBufferIndex = 0; // 次のデータを書き込むバッファの番号
    mBufferLoc = 0; // 次のデータを書き込む位置
    mBlockSize = 4410; // ブロックサイズ
#elif defined( WAVEOUT_OS_MAC )
    mMutex = (pthread_mutex_t *)malloc( sizeof( pthread_mutex_t ) );
    memset( mMutex, 0, sizeof( pthread_mutex_t ) );
    pthread_mutex_init( mMutex, NULL );
#elif defined( WAVEOUT_OS_OTHER )
    mFileDescripter = 0;
    mBlockSize = 44100;
    mSampleRate = 44100;
    mIsInitialized = 0;
    mBufferLength = 44100;
#endif
}

WaveOut::~WaveOut(){
}

#ifdef WAVEOUT_OS_OTHER
/// <summary>
/// /dev/dsp を以下の様に設定する。
/// </summary>
int WaveOut::setupDsp( int fd ){
    int fmt = AFMT_S16_LE;
    int channel = 2;

    /* サウンドフォーマットの設定 */
    if( ioctl( fd, SNDCTL_DSP_SETFMT, &fmt ) == -1 ){
        perror( "ioctl( SOUND_PCM_SETFMT )" );
        return -1;
    }

    /* チャンネル数の設定 */
    if( ioctl( fd, SNDCTL_DSP_CHANNELS, &channel ) == -1 ){
        perror( "iotcl( SOUND_PCM_WRITE_CHANNELS )" );
        return -1;
    }

    /* サンプリング周波数の設定 */
    if( ioctl( fd, SNDCTL_DSP_SPEED, &mSampleRate ) == -1 ){
        perror( "iotcl( SOUND_PCM_WRITE_RATE )" );
        return -1;
    }

    return 0;
}
#endif

void WaveOut::unprepare(){
#ifdef WAVEOUT_OS_WIN
    if( NULL == mWaveOut ){
        return;
    }

    EnterCriticalSection( &mLocker );
    for( int i = 0; i < WAVEOUT_NUM_BUF; i++ ){
        waveOutUnprepareHeader( mWaveOut,
                                &(mWaveHeaders[i]),
                                sizeof( WAVEHDR ) );
        free( mWaveHeaders[i].lpData );
    }
    waveOutClose( mWaveOut );
    mWaveOut = NULL;
    LeaveCriticalSection( &mLocker );
#elif defined( WAVEOUT_OS_MAC )
    // 音声ファイル再生後の掃除
    AudioQueueDispose(
        mQueue,
        true
    );

    free( mPacketDescs );

    for( int i = 0; i < WAVEOUT_NUM_BUF; i++ ){
        if( mIncomingWavesL[i] ){
            free( mIncomingWavesL[i] );
        }
        if( mIncomingWavesR[i] ){
            free( mIncomingWavesR[i] );
        }
    }
    mIncomingBufferUsed = 0;
    mIncomingBufferLoc = 0;
    mOutgoingBufferUsed = 0;
    mOutgoingBufferLoc = 0;
#elif defined( WAVEOUT_OS_OTHER )
    if( NULL == mFileDescripter ){
        return;
    }
    close( mFileDescripter );
#endif
}

void WaveOut::init() {
#ifdef WAVEOUT_OS_WIN
    InitializeCriticalSection( &mLocker );
#endif
}

void WaveOut::kill() {
    exit();
#ifdef WAVEOUT_OS_WIN
    DeleteCriticalSection( &mLocker );
#elif defined( WAVEOUT_OS_MAC )
    pthread_mutex_destroy( mMutex );
    free( mMutex );
#endif
}

double WaveOut::getPosition(){
#ifdef WAVEOUT_OS_WIN
    if( NULL == mWaveOut ){
        return -1.0;
    }

    MMTIME mmt;
    mmt.wType = TIME_MS;
    waveOutGetPosition( mWaveOut, &mmt, sizeof( MMTIME ) );
    float ms = 0.0f;
    switch( mmt.wType ){
        case TIME_MS:{
            return mmt.u.ms * 0.001;
        }
        case TIME_SAMPLES:{
            return (double)mmt.u.sample / (double)mWaveFormat.nSamplesPerSec;
        }
        case TIME_BYTES:{
            return (double)mmt.u.cb / (double)mWaveFormat.nAvgBytesPerSec;
        }
        default:{
            return -1.0;
        }
    }
    return 0.0;
#elif defined( WAVEOUT_OS_OTHER )
    if( mFileDescripter == 0 ){
        return -1;
    }

#if defined( SNDCTL_DSP_CURRENT_OPTR )
    oss_count_t ptr;
    if( ioctl( mFileDescripter, SNDCTL_DSP_CURRENT_OPTR, &ptr ) == -1 ){
        return -1;
    }
    double time = (ptr.samples + ptr.fifo_samples) / (double)mSampleRate;
    return time;
#else
    count_info ptr;
    if( ioctl( mFileDescripter, SNDCTL_DSP_GETOPTR, &ptr ) == -1 ){
        return -1;
    }
    double rate = 16.0 * 2.0 / 8.0 * mSampleRate;
    double time = ptr.bytes / rate;
    return time;
#endif
#endif
}

void WaveOut::waitForExit(){
#ifdef WAVEOUT_OS_WIN
    if( NULL == mWaveOut ){
        return;
    }

    EnterCriticalSection( &mLocker );
    // buffer_indexがNUM_BUF未満なら、まだ1つもwaveOutWriteしていないので、書き込む
    if( mBufferIndex < WAVEOUT_NUM_BUF ){
        for( int i = 0; i < mBufferIndex; i++ ){
            if( mAbortRequired ) break;
            mIsWaveDone[i] = false;
            waveOutWrite( mWaveOut, &(mWaveHeaders[i]), sizeof( WAVEHDR ) );
        }
    }

    // まだ書き込んでないバッファがある場合、残りを書き込む
    if( mBufferLoc != 0 ){
        int act_buffer_index = mBufferIndex % WAVEOUT_NUM_BUF;

        // バッファが使用中の場合、使用終了となるのを待ち受ける
        while( !mIsWaveDone[act_buffer_index] ){
            if( mAbortRequired ) break;
            Sleep( 0 );
        }

        if( !mAbortRequired ){
            // 後半部分を0で埋める
            for( int i = mBufferLoc; i < mBlockSizeUsed; i++ ){
                mWaves[act_buffer_index][i] = MAKELONG( 0, 0 );
            }

            mBufferLoc = 0;
            mBufferIndex++;

            mIsWaveDone[act_buffer_index] = false;
            waveOutWrite( mWaveOut, &mWaveHeaders[act_buffer_index], sizeof( WAVEHDR ) );
        }
    }

    // WAVEOUT_NUM_BUF個のバッファすべてがwave_doneとなるのを待つ。
    while( !mAbortRequired ){
        bool all_done = true;
        for( int i = 0; i < WAVEOUT_NUM_BUF; i++ ){
            if( !mIsWaveDone[i] ){
                all_done = false;
                break;
            }
        }
        if( all_done ){
            break;
        }
    }
    LeaveCriticalSection( &mLocker );

    // リセット処理
    exit();
#elif defined( WAVEOUT_OS_OTHER )
    if( mFileDescripter == 0 ){
        return;
    }
#endif
}

void WaveOut::setResolution( int resolution ){
#ifdef WAVEOUT_OS_WIN
    mBlockSize = resolution;
#endif
}

void WaveOut::append( double *left, double *right, int length ){
#ifdef WAVEOUT_OS_WIN
    if( NULL == mWaveOut ){
        return;
    }
    EnterCriticalSection( &mLocker );
    int appended = 0; // 転送したデータの個数
    while( appended < length ){
        // このループ内では、バッファに1個づつデータを転送する

        // バッファが使用中の場合、使用終了となるのを待ち受ける
        int act_buffer_index = mBufferIndex % WAVEOUT_NUM_BUF;
        while( !mIsWaveDone[act_buffer_index] && !mAbortRequired ){
            Sleep( 0 );
        }

        int t_length = mBlockSizeUsed - mBufferLoc; // 転送するデータの個数
        if( t_length > length - appended ){
            t_length = length - appended;
        }
        for( int i = 0; i < t_length && !mAbortRequired; i++ ){
            mWaves[act_buffer_index][mBufferLoc + i] =
                    MAKELONG( (WORD)(left[appended + i] * 32768.0), (WORD)(right[appended + i] * 32768.0) );
        }
        appended += t_length;
        mBufferLoc += t_length;
        if( mBufferLoc == mBlockSizeUsed ){
            // バッファがいっぱいになったようだ
            mBufferIndex++;
            mBufferLoc = 0;
            if( mBufferIndex >= WAVEOUT_NUM_BUF ){
                // 最初のNUM_BUF個のバッファは、すべてのバッファに転送が終わるまで
                // waveOutWriteしないようにしているので、ここでwaveOutWriteする。
                if( mBufferIndex == WAVEOUT_NUM_BUF ){
                    for( int i = 0; i < WAVEOUT_NUM_BUF; i++ ){
                        if( mAbortRequired ) break;
                        mIsWaveDone[i] = false;
                        waveOutWrite( mWaveOut, &mWaveHeaders[i], sizeof( WAVEHDR ) );
                    }
                }else{
                    mIsWaveDone[act_buffer_index] = false;
                    if( !mAbortRequired ){
                        waveOutWrite( mWaveOut, &mWaveHeaders[act_buffer_index], sizeof( WAVEHDR ) );
                    }
                }
            }
        }
    }
    LeaveCriticalSection( &mLocker );
#elif defined( WAVEOUT_OS_MAC )
    pthread_mutex_lock( mMutex );

    // 可能な限りバッファを転送
    // 今いるバッファはmIncomingBufferUsed、mOutgoingBuferUsedとkNumberBuffers以上の差がついてはならん
    int max_buf_num = length / mSampleRate + 1;
    if( length % mSampleRate == 0 ){
        max_buf_num--;
    }
    // mOutgoingBufferUsed + (kNumberBuffers - 1)までは書き込んでおk
    if( mIncomingBufferUsed + max_buf_num > mOutgoingBufferUsed + (WAVEOUT_NUM_BUF - 1) ){
        max_buf_num = mOutgoingBufferUsed + (WAVEOUT_NUM_BUF - 1) - mIncomingBufferUsed;
    }
    dbg( "_WaveOut::append; max_buf_num=%d", max_buf_num );
    int remain = length;
    int indx = 0;
    int buf_from = mIncomingBufferUsed;
    int buf_to = mIncomingBufferUsed + max_buf_num;
    dbg( "_WaveOut::append; buf_from=%d; buf_to=%d", buf_from, buf_to );
    for( int i = buf_from; i < buf_to; i++ ){
        int act_i = i % WAVEOUT_NUM_BUF;
        int amount = (remain > mSampleRate - mIncomingBufferLoc) ? (mSampleRate - mIncomingBufferLoc) : remain;
        for( int j = mIncomingBufferLoc; j < mIncomingBufferLoc + amount; j++ ){
            mIncomingWavesL[act_i][j] = left[indx];
            mIncomingWavesR[act_i][j] = right[indx];
            indx++;
        }
        remain -= amount;
        mIncomingBufferLoc += amount;
        if( mIncomingBufferLoc == mSampleRate ){
            mIncomingBufferLoc = 0;
            mIncomingBufferUsed++;
        }
    }

    if( !mIsRunning ){
        // 初回のappend呼び出しだったらば
        mIsRunning = true;

        // 初回のバッファを埋める
        int max_first_buf_num = length / mSampleRate + 1;
        if( length % mSampleRate == 0 ){
            max_first_buf_num--;
        }
        for( int i = 0; i < max_first_buf_num; i++ ){
            callback(
                this,
                mQueue,
                mBuffers[i]
            );
        }

        // 再生時の利得を設定
        Float32 gain = 1.0;
        AudioQueueSetParameter(
            mQueue,
            kAudioQueueParam_Volume,
            gain
        );

        // 再生を開始
        OSStatus ret = AudioQueueStart(
            mQueue,
            NULL
        );
        dbg( "::main; AudioQueueStart result=%d\n", ret );
    }

    // 埋めきらなかった分。バッファが空くのを待機
    dbg( "_WaveOut::append; remain=%d", remain );
    while( remain > 0 && mIsRunning ){
        // 待機
        while( mOutgoingBufferUsed + WAVEOUT_NUM_BUF <= mIncomingBufferUsed && mIsRunning ){
            CFRunLoopRunInMode(
                kCFRunLoopDefaultMode,
                0.25,
                false
            );
        }
        if( !mIsRunning ){
            break;
        }

        // 転送
        int act_i = mIncomingBufferUsed % WAVEOUT_NUM_BUF;
        while( remain > 0 && mIsRunning ){
            int amount = (remain > mSampleRate - mIncomingBufferLoc) ? (mSampleRate - mIncomingBufferLoc) : remain;
            for( int j = mIncomingBufferLoc; j < mIncomingBufferLoc + amount; j++ ){
                mIncomingWavesL[act_i][j] = left[indx];
                mIncomingWavesR[act_i][j] = right[indx];
                indx++;
            }
            remain -= amount;
            mIncomingBufferLoc += amount;
            if( mIncomingBufferLoc == mSampleRate ){
                mIncomingBufferLoc = 0;
                mIncomingBufferUsed++;
            }
        }
    }

    pthread_mutex_unlock( mMutex );
#elif defined( WAVEOUT_OS_OTHER )
    if( mFileDescripter == 0 ){
        return;
    }

    int remain = length;
    int i = 0;
    int pos = 0;
    while( remain > 0 ){
        unsigned long v = MAKELONG( (unsigned short)(right[i] * 32768.0), (unsigned short)(left[i] * 32768.0) );
        mBuffer[pos] = v;
        pos++;
        i++;
        remain--;
        if( pos >= mBufferLength ){
            //ioctl( fd, SNDCTL_DSP_SYNC, 0 );
            write( mFileDescripter, mBuffer, mBufferLength * sizeof( unsigned long ) );
            pos = 0;
        }
    }

    if( pos > 0 ){
        write( mFileDescripter, mBuffer, pos );
    }
#endif
}

#ifdef WAVEOUT_OS_WIN
/// <summary>
/// コールバック関数。バッファの再生終了を検出するために使用。
/// </summary>
/// <param name="hwo"></param>
/// <param name="uMsg"></param>
/// <param name="dwInstance"></param>
/// <param name="dwParam1"></param>
/// <param name="dwParam2"></param>
void WaveOut::callback(
    HWAVEOUT hwo,
    UINT uMsg,
    DWORD dwInstance,
    DWORD dwParam1,
    DWORD dwParam2 ){
    if( uMsg != MM_WOM_DONE ){
        return;
    }

    WaveOut *instance = (WaveOut *)dwInstance;
    for( int i = 0; i < WAVEOUT_NUM_BUF; i++ ){
        if( &instance->mWaveHeaders[i] != (WAVEHDR *)dwParam1 ){
            continue;
        }
        instance->mIsWaveDone[i] = true;
        break;
    }
}
#endif

/// <summary>
/// デバイスを初期化する
/// </summary>
/// <param name="sample_rate"></param>
void WaveOut::prepare( int sample_rate ){
#ifdef WAVEOUT_OS_WIN
    // デバイスを使用中の場合、使用を停止する
    if( NULL != mWaveOut ){
        exit();
        unprepare();
    }

    EnterCriticalSection( &mLocker );
    // フォーマットを指定
    mWaveFormat.wFormatTag = WAVE_FORMAT_PCM;
    mWaveFormat.nChannels = 2;
    mWaveFormat.wBitsPerSample = 16;
    mWaveFormat.nBlockAlign
        = mWaveFormat.nChannels * mWaveFormat.wBitsPerSample / 8;
    mWaveFormat.nSamplesPerSec = sample_rate;
    mWaveFormat.nAvgBytesPerSec
        = mWaveFormat.nSamplesPerSec * mWaveFormat.nBlockAlign;

    // デバイスを開く
    waveOutOpen( &mWaveOut,
                 WAVE_MAPPER,
                 &mWaveFormat,
                 (DWORD_PTR)&callback,
                 (DWORD_PTR)this,
                 CALLBACK_FUNCTION );

    // バッファを準備
    mBlockSizeUsed = mBlockSize;
    for( int i = 0; i < WAVEOUT_NUM_BUF; i++ ){
        mWaves[i] = (DWORD*)malloc( (int)(sizeof( DWORD ) * mBlockSizeUsed) );
        mWaveHeaders[i].lpData = (LPSTR)mWaves[i];
        mWaveHeaders[i].dwBufferLength = sizeof( DWORD ) * mBlockSizeUsed;
        mWaveHeaders[i].dwFlags = WHDR_BEGINLOOP | WHDR_ENDLOOP;
        mWaveHeaders[i].dwLoops = 1;
        waveOutPrepareHeader( mWaveOut, &mWaveHeaders[i], sizeof( WAVEHDR ) );

        mIsWaveDone[i] = true;
    }

    mBufferIndex = 0;
    mBufferLoc = 0;
    mAbortRequired = false;

    LeaveCriticalSection( &mLocker );
#elif defined( WAVEOUT_OS_MAC )
    mSampleRate = sample_rate;

    // バッファを確保
    for( int i = 0; i < WAVEOUT_NUM_BUF; i++ ){
        mIncomingWavesL[i] = (double *)malloc( sizeof( double ) * mSampleRate );
        mIncomingWavesR[i] = (double *)malloc( sizeof( double ) * mSampleRate );
    }
    mIncomingBufferUsed = 0;
    mIncomingBufferLoc = 0;
    mOutgoingBufferUsed = 0;
    mOutgoingBufferLoc = 0;

    // ファイルの音声データ形式を設定
    mDataFormat.mBitsPerChannel = 16;
    mDataFormat.mBytesPerFrame = 4;
    mDataFormat.mBytesPerPacket = 4;
    mDataFormat.mChannelsPerFrame = 2;
    mDataFormat.mFormatFlags = 12;//kAudioFormatFlagIsBigEndian | kAudioFormatFlagIsSignedInteger;//12
    mDataFormat.mFormatID = kAudioFormatLinearPCM;
    mDataFormat.mFramesPerPacket = 1;
    mDataFormat.mReserved = 0;
    mDataFormat.mSampleRate = (double)mSampleRate;

    // 再生Audio Queueの生成
    AudioQueueNewOutput(
        &mDataFormat,
        callback,
        this,
        CFRunLoopGetCurrent(),
        kCFRunLoopCommonModes,
        0,
        &mQueue
    );

    // 再生Audio Queue Bufferの容量と読み込むパケット数を設定する
    UInt32 maxPacketSize;
    maxPacketSize = mDataFormat.mBytesPerPacket;
    dbg( "maxPacketSize=%d", maxPacketSize );

    UInt32 numPacketsToRead;
    deriveBufferSize(
        mDataFormat,
        maxPacketSize,
        0.5,
        &bufferByteSize,
        &numPacketsToRead
    );
    dbg( "aqData.bufferByteSize=%d", bufferByteSize );
    dbg( "numPacketsToRead=%d", numPacketsToRead );

    // VBRでないので、ここはNULL
    mPacketDescs = NULL;

    // 再生用のAudio Queue Bufferの確保と準備
    //mCurrentPacket = 0;
    for( int i = 0; i < WAVEOUT_NUM_BUF; i++ ){
        AudioQueueAllocateBuffer(
            mQueue,
            bufferByteSize,
            &mBuffers[i]
        );
    }
#elif defined( WAVEOUT_OS_OTHER )
    mBlockSize = sample_rate;//とりあえず block_size;
    mSampleRate = sample_rate;
    int i;
    if( (mFileDescripter = open( "/dev/dsp", O_WRONLY )) == -1 ){
        perror( "open()" );
        return;
    }

    // /dev/dsp の設定
    if( setupDsp( mFileDescripter ) != 0 ){
        fprintf( stderr, "Setup /dev/dsp failed.\n" );
        close( mFileDescripter );
        return;
    }

    // バッファを用意
    mBuffer = (unsigned long *)calloc( (size_t)mBufferLength, sizeof( unsigned long ) );
    mIsInitialized = 1;
#endif
}

/// <summary>
/// 再生をとめる。
/// </summary>
void WaveOut::exit(){
#ifdef WAVEOUT_OS_WIN
    if( NULL != mWaveOut ){
        mAbortRequired = true;
        EnterCriticalSection( &mLocker );
        waveOutReset( mWaveOut );
        LeaveCriticalSection( &mLocker );
    }
#elif defined( WAVEOUT_OS_MAC )
    AudioQueueStop( mQueue, false );
    mIsRunning = false;
#elif defined( WAVEOUT_OS_OTHER )
    if( mFileDescripter == 0 ){
        return;
    }
    ioctl( mFileDescripter, SNDCTL_DSP_RESET, 0 );
#endif
}

#if defined( WAVEOUT_OS_MAC )
void WaveOut::callback(
    void                *aqData,
    AudioQueueRef       inAQ,
    AudioQueueBufferRef inBuffer
){
    dbg( "_WaveOut::callback" );
    WaveOut *instance = (WaveOut *)aqData;
    if( instance->mIsRunning == 0 ){
        return;
    }
    UInt32 numBytesReadFromFile;
    // ファイルから読まずに波形を直で与えてみる
    int bytes_per_packet = instance->mDataFormat.mBytesPerPacket;
    int channels = instance->mDataFormat.mChannelsPerFrame;
    int bits_per_channel = instance->mDataFormat.mBitsPerChannel;
    int sample_rate = instance->mSampleRate;

    // バッファを埋めるお
    // 第mOutgoingBuffer+1番目のバッファを転送する
    // もし、目的のバッファが埋まってなかったら埋まるまで待つ
    dbg( "_WaveOut::callback; mIncomingBufferUsed=%d", instance->mIncomingBufferUsed );
    dbg( "_WaveOut::callback; mOutgoingBufferUsed=%d", instance->mOutgoingBufferUsed );
    while( instance->mIncomingBufferUsed < instance->mOutgoingBufferUsed && instance->mIsRunning ){
        CFRunLoopRunInMode(
            kCFRunLoopDefaultMode,
            0.25,
            false
        );
    }
    if( !instance->mIsRunning ){
        return;
    }

    // 転送する
    int act_buf_index = instance->mOutgoingBufferUsed % WAVEOUT_NUM_BUF;
    signed short *dat = (signed short *)inBuffer->mAudioData;
    int indx = 0;
    for( int i = 0; i < sample_rate; i++ ){
        double vl = instance->mIncomingWavesL[act_buf_index][i];
        double vr = instance->mIncomingWavesR[act_buf_index][i];
        signed short bl = (signed short)(vl * 32767.0);
        signed short br = (signed short)(vr * 32767.0);
        dat[indx] = bl;
        indx++;
        dat[indx] = br;
        indx++;
    }

    // 転送が終わったので++
    instance->mOutgoingBufferUsed++;

    // 何バイト読んだことになったか
    numBytesReadFromFile = channels * bits_per_channel / 8 * sample_rate;

    if( numBytesReadFromFile > 0 ){                                     // 5
        inBuffer->mAudioDataByteSize = numBytesReadFromFile;  // 6
        AudioQueueEnqueueBuffer (
            instance->mQueue,
            inBuffer,
            0,
            instance->mPacketDescs
        );
        //instance->mCurrentPacket += numPackets;                // 7
    }else{
        AudioQueueStop(
            instance->mQueue,
            false
        );
        instance->mIsRunning = false;
    }
}
#endif // WAVEOUT_OS_MAC

#if defined( WAVEOUT_OS_MAC )
//リスト3-7 再生用Audio Queue Bufferの大きさを求める
void WaveOut::deriveBufferSize (
    AudioStreamBasicDescription &ASBDesc,                            // 1
    UInt32                      maxPacketSize,                       // 2
    Float64                     seconds,                             // 3
    UInt32                      *outBufferSize,                      // 4
    UInt32                      *outNumPacketsToRead                 // 5
) {
    static const int maxBufferSize = 0x50000;                        // 6
    static const int minBufferSize = 0x4000;                         // 7

    dbg( "::DeriveBufferSize; ASBDesc.mFramesPerPacket=%d", ASBDesc.mFramesPerPacket );
    dbg( "::DeriveBufferSize; ASBDesc.mSampleRate=%f", ASBDesc.mSampleRate );
    if (ASBDesc.mFramesPerPacket != 0) {                             // 8
        Float64 numPacketsForTime =
            ASBDesc.mSampleRate / ASBDesc.mFramesPerPacket * seconds;
        *outBufferSize = numPacketsForTime * maxPacketSize;
    } else {                                                         // 9
        *outBufferSize =
            maxBufferSize > maxPacketSize ?
                maxBufferSize : maxPacketSize;
    }

    if (                                                             // 10
        *outBufferSize > maxBufferSize &&
        *outBufferSize > maxPacketSize
        ){
        *outBufferSize = maxBufferSize;
    }else {                                                           // 11
        if (*outBufferSize < minBufferSize){
            *outBufferSize = minBufferSize;
        }
    }

    *outNumPacketsToRead = *outBufferSize / maxPacketSize;           // 12
}
#endif // WAVEOUT_OS_MAC
