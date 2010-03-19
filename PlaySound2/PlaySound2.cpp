#include "PlaySound2.h"

HWAVEOUT wave_out = NULL;
WAVEFORMATEX wave_format;
WAVEHDR wave_header[NUM_BUF];
DWORD *wave[NUM_BUF];
bool wave_done[NUM_BUF];
int buffer_index = 0; // 次のデータを書き込むバッファの番号
int buffer_loc = 0; // 次のデータを書き込む位置
CRITICAL_SECTION locker;
bool abort_required;
int block_size = 4410; // ブロックサイズ
int block_size_used; // SoundPrepareで初期化されたブロックサイズ

#ifdef __cplusplus
extern "C" {
#endif

void SoundUnprepare() {
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
    is_busy = false;
    LeaveCriticalSection( &locker );
}

void SoundInit() {
    InitializeCriticalSection( &locker );
}

void SoundKill() {
    SoundExit();
    DeleteCriticalSection( &locker );
}

double SoundGetPosition() {
    if ( NULL == wave_out ) {
        return -1.0;
    }

    MMTIME mmt;
    mmt.wType = TIME_MS;
    waveOutGetPosition( wave_out, &mmt, sizeof( MMTIME ) );
    float ms = 0.0f;
    switch ( mmt.wType ) {
        case TIME_MS:
            return mmt.u.ms * 0.001;
        case TIME_SAMPLES:
            return (double)mmt.u.sample / (double)wave_format.nSamplesPerSec;
        case TIME_BYTES:
            return (double)mmt.u.cb / (double)wave_format.nAvgBytesPerSec;
        default:
            return -1.0;
    }
    return 0.0;
}

void SoundWaitForExit() {
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

void SoundSetResolution( int resolution ){
    block_size = resolution;
}

void SoundAppend( double *left, double *right, int length ) {
    if ( NULL == wave_out ) {
        return;
    }
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
        for ( int i = 0; i < t_length && !abort_required; i++ ) {
            wave[act_buffer_index][buffer_loc + i] = MAKELONG( (WORD)(left[appended + i] * 32768.0), (WORD)(right[appended + i] * 32768.0) );
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

/// <summary>
/// コールバック関数。バッファの再生終了を検出するために使用。
/// </summary>
/// <param name="hwo"></param>
/// <param name="uMsg"></param>
/// <param name="dwInstance"></param>
/// <param name="dwParam1"></param>
/// <param name="dwParam2"></param>
void SoundCallback(
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
void SoundPrepare( int sample_rate ) {
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
    waveOutOpen( &wave_out,
                 WAVE_MAPPER,
                 &wave_format,
                 (DWORD_PTR)SoundCallback,
                 NULL,
                 CALLBACK_FUNCTION );

    // バッファを準備
    block_size_used = block_size;
    for ( int i = 0; i < NUM_BUF; i++ ) {
        wave[i] = (DWORD*)malloc( (int)(sizeof( DWORD ) * block_size_used) );
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
}

/// <summary>
/// 再生をとめる。
/// </summary>
void SoundExit() {
    if ( NULL != wave_out ) {
        abort_required = true;
        EnterCriticalSection( &locker );
        waveOutReset( wave_out );
        LeaveCriticalSection( &locker );
    }
}

#ifdef __cplusplus
} // extern "C"
#endif
