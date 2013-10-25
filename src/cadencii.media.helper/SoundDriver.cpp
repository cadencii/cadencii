/*
 * SoundDriver.cpp
 * Copyright (C) 2007-2011 kbinani
 *
 * This file is part of cadencii.media.helper.
 *
 * cadencii.media.helper is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.media.helper is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#include "./SoundDriver.h"

#pragma comment(lib, "winmm")

static HWAVEOUT wave_out = NULL;
static WAVEFORMATEX wave_format;
static WAVEHDR wave_header[NUM_BUF];
static DWORD *wave[NUM_BUF];
static bool wave_done[NUM_BUF];
static int buffer_index = 0; // 次のデータを書き込むバッファの番号
static int buffer_loc = 0; // 次のデータを書き込む位置
static CRITICAL_SECTION locker;
static bool abort_required;
static int block_size = 4410; // ブロックサイズ
static int block_size_used; // SoundPrepareで初期化されたブロックサイズ

typedef DWORD (GetWaveDataFunc) (void * stream, int index);

static DWORD GetWaveDataFromLRChannel(void * stream, int index)
{
    double * left = ((double **)stream)[0];
    double * right = ((double **)stream)[1];
    return MAKELONG((WORD)(left[index] * 32768.0), (WORD)(right[index] * 32768.0));
}

static DWORD GetWaveDataFromLRInterleavedStream(void * data, int index)
{
    float * stream = reinterpret_cast<float *>(data);
    int const CHANNEL = 2;
    int const LEFT = 0;
    int const RIGHT = 1;
    int const actual_index = index * CHANNEL;
    float const left = stream[actual_index + LEFT];
    float const right = stream[actual_index + RIGHT];
    return MAKELONG((WORD)(left * 32768.0), (WORD)(right * 32768.0));
}

static void SoundAppendImpl(void * stream, int length, GetWaveDataFunc * func)
{
    if ( NULL == wave_out ) {
        return;
    }
    int const CHANNELS = 2;
    int const LEFT = 0;
    int const RIGHT = 1;

    EnterCriticalSection( &locker );
    int appended = 0; // 転送したデータの個数
    while ( appended < length ) {
        // このループ内では、バッファに1個づつデータを転送する

        // バッファが使用中の場合、使用終了となるのを待ち受ける
        int act_buffer_index = buffer_index % NUM_BUF;
        while ( !wave_done[act_buffer_index] && !abort_required ) {
            Sleep( 0 );
        }

        int t_length = block_size_used - buffer_loc; // 転送するデータの個数
        if ( t_length > length - appended ) {
            t_length = length - appended;
        }
        int index = 0;
        int const stream_offset = appended * CHANNELS;
        for (int i = 0; i < t_length && !abort_required; ++i) {
            wave[act_buffer_index][buffer_loc + i] = func(stream, appended + i);
        }
        appended += t_length;
        buffer_loc += t_length;
        if ( buffer_loc == block_size_used ) {
            // バッファがいっぱいになったようだ
            buffer_index++;
            buffer_loc = 0;
            if ( buffer_index >= NUM_BUF ) {
                // 最初のNUM_BUF個のバッファは、すべてのバッファに転送が終わるまで
                // waveOutWriteしないようにしているので、ここでwaveOutWriteする。
                if ( buffer_index == NUM_BUF ) {
                    for ( int i = 0; i < NUM_BUF; i++ ) {
                        if( abort_required ) break;
                        wave_done[i] = false;
                        waveOutWrite( wave_out, &wave_header[i], sizeof( WAVEHDR ) );
                    }
                } else {
                    wave_done[act_buffer_index] = false;
                    if( !abort_required ){
                        waveOutWrite( wave_out, &wave_header[act_buffer_index], sizeof( WAVEHDR ) );
                    }
                }
            }
        }
    }
    LeaveCriticalSection( &locker );
}

CADENCII_MEDIA_HELPER_API(void, SoundUnprepare)() {
    if ( NULL == wave_out ) {
        return;
    }

    EnterCriticalSection( &locker );
    for ( int i = 0; i < NUM_BUF; i++ ) {
        waveOutUnprepareHeader( wave_out,
                                &(wave_header[i]),
                                sizeof( WAVEHDR ) );
        free( wave_header[i].lpData );
    }
    waveOutClose( wave_out );
    wave_out = NULL;
    LeaveCriticalSection( &locker );
}

CADENCII_MEDIA_HELPER_API(void, SoundInit)() {
    InitializeCriticalSection( &locker );
}

CADENCII_MEDIA_HELPER_API(void, SoundKill)() {
    SoundExit();
    DeleteCriticalSection( &locker );
}

CADENCII_MEDIA_HELPER_API(double, SoundGetPosition)() {
    if ( NULL == wave_out ) {
        return 0.0;
    }

    MMTIME mmt;
    mmt.wType = TIME_MS;
    EnterCriticalSection( &locker );
    waveOutGetPosition( wave_out, &mmt, sizeof( MMTIME ) );
    LeaveCriticalSection( &locker );
    float ms = 0.0f;
    switch ( mmt.wType ) {
        case TIME_MS:
            return mmt.u.ms * 0.001;
        case TIME_SAMPLES:
            return (double)mmt.u.sample / (double)wave_format.nSamplesPerSec;
        case TIME_BYTES:
            return (double)mmt.u.cb / (double)wave_format.nAvgBytesPerSec;
        default:
            return 0.0;
    }
    return 0.0;
}

CADENCII_MEDIA_HELPER_API(void, SoundWaitForExit)() {
    if ( NULL == wave_out ) {
        return;
    }

    EnterCriticalSection( &locker );
    // buffer_indexがNUM_BUF未満なら、まだ1つもwaveOutWriteしていないので、書き込む
    if ( buffer_index < NUM_BUF ) {
        for ( int i = 0; i < buffer_index; i++ ) {
            if( abort_required ) break;
            wave_done[i] = false;
            waveOutWrite( wave_out, &(wave_header[i]), sizeof( WAVEHDR ) );
        }
    }

    // まだ書き込んでないバッファがある場合、残りを書き込む
    if ( buffer_loc != 0 ) {
        int act_buffer_index = buffer_index % NUM_BUF;

        // バッファが使用中の場合、使用終了となるのを待ち受ける
        while ( !wave_done[act_buffer_index] ) {
            if( abort_required ) break;
            Sleep( 0 );
        }

        if( !abort_required ){
            // 後半部分を0で埋める
            for ( int i = buffer_loc; i < block_size_used; i++ ) {
                wave[act_buffer_index][i] = MAKELONG( 0, 0 );
            }

            buffer_loc = 0;
            buffer_index++;

            wave_done[act_buffer_index] = false;
            waveOutWrite( wave_out, &wave_header[act_buffer_index], sizeof( WAVEHDR ) );
        }
    }

    // NUM_BUF個のバッファすべてがwave_doneとなるのを待つ。
    while ( !abort_required ) {
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
    LeaveCriticalSection( &locker );

    // リセット処理
    SoundExit();
}

CADENCII_MEDIA_HELPER_API(void, SoundSetResolution)( int resolution ){
    block_size = resolution;
}

CADENCII_MEDIA_HELPER_API(void, SoundAppend)(double * left, double *right, int length)
{
    double * stream[2];
    stream[0] = left;
    stream[1] = right;
    SoundAppendImpl(stream, length, &GetWaveDataFromLRChannel);
}

CADENCII_MEDIA_HELPER_API(void, SoundAppendInterleaved)(float * stream, int length)
{
    SoundAppendImpl(stream, length, &GetWaveDataFromLRInterleavedStream);
}

/// <summary>
/// コールバック関数。バッファの再生終了を検出するために使用。
/// </summary>
/// <param name="hwo"></param>
/// <param name="uMsg"></param>
/// <param name="dwInstance"></param>
/// <param name="dwParam1"></param>
/// <param name="dwParam2"></param>
CADENCII_MEDIA_HELPER_EXTERN_C void CALLBACK SoundCallback(
    HWAVEOUT hwo,
    UINT uMsg,
    DWORD dwInstance,
    DWORD dwParam1,
    DWORD dwParam2 ) {
    if ( uMsg != MM_WOM_DONE ) {
        return;
    }

    for ( int i = 0; i < NUM_BUF; i++ ) {
        if ( &wave_header[i] != (WAVEHDR*)dwParam1 ) {
            continue;
        }
        wave_done[i] = true;
        break;
    }
}

/// <summary>
/// デバイスを初期化する
/// </summary>
/// <param name="sample_rate"></param>
CADENCII_MEDIA_HELPER_API(int, SoundPrepare)( int sample_rate ) {
    // デバイスを使用中の場合、使用を停止する
    if ( NULL != wave_out ) {
        SoundExit();
        SoundUnprepare();
    }

    EnterCriticalSection( &locker );
    // フォーマットを指定
    wave_format.wFormatTag = WAVE_FORMAT_PCM;
	wave_format.nChannels = 2;
    wave_format.wBitsPerSample = 16;
    wave_format.nBlockAlign
        = wave_format.nChannels * wave_format.wBitsPerSample / 8;
    wave_format.nSamplesPerSec = sample_rate;
    wave_format.nAvgBytesPerSec
        = wave_format.nSamplesPerSec * wave_format.nBlockAlign;

    // デバイスを開く
    MMRESULT ret = 
		waveOutOpen( 
			&wave_out,
            WAVE_MAPPER,
            &wave_format,
            (DWORD_PTR)SoundCallback,
            NULL,
            CALLBACK_FUNCTION );

    // バッファを準備
    block_size_used = block_size;
    for ( int i = 0; i < NUM_BUF; i++ ) {
        wave[i] = (DWORD *)malloc( (int)(sizeof( DWORD ) * block_size_used) );
        wave_header[i].lpData = (LPSTR)wave[i];
        wave_header[i].dwBufferLength = sizeof( DWORD ) * block_size_used;
        wave_header[i].dwFlags = WHDR_BEGINLOOP | WHDR_ENDLOOP;
        wave_header[i].dwLoops = 1;
        waveOutPrepareHeader( wave_out, &wave_header[i], sizeof( WAVEHDR ) );

        wave_done[i] = true;
    }

    buffer_index = 0;
    buffer_loc = 0;
    abort_required = false;

    LeaveCriticalSection( &locker );

	return (int)ret;
}

/// <summary>
/// 再生をとめる。
/// </summary>
CADENCII_MEDIA_HELPER_API(void, SoundExit)() {
    if ( NULL != wave_out ) {
        abort_required = true;
        EnterCriticalSection( &locker );
        waveOutReset( wave_out );
        LeaveCriticalSection( &locker );
    }
}
