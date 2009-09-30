#include <SDL/SDL.h>
#include <SDL/SDL_audio.h>

#define FALSE 0
#define TRUE 1

#define NUM_BUF 3

#ifdef __cplusplus
extern "C"{
#endif

Uint16 *g_buffer_left[NUM_BUF];
Uint16 *g_buffer_right[NUM_BUF];

long g_processed_samples;
int g_block_size;
int g_sample_rate;
int g_byte_per_sample;
int g_buffer_writeloc;   //次にappendがきたとき書き込み始める位置
int g_buffer_readloc;    //次にCallbackがきたとき読み始める位置
int g_done[NUM_BUF];     // 各バッファがcallbackでの読み出しを完了したかどうかを表すフラグ
int g_current_wbuffer;   // 次回書き込むべきバッファのインデクス
int g_current_rbuffer;   // 次回callback読み込むべきバッファのインデックス
int g_prepared[NUM_BUF]; // 各バッファにSoundAppendでデータがパックされたかどうかを表すフラグ
int g_playing;
int g_exit_required;
int g_last_buffer;
int g_resolution;

void WaveCallback( void *unused, Uint8 *stream, int len );

void SoundInit( int block_size, int sample_rate );
void SoundAppend( double *left, double *right, int length );
void SoundReset();
double SoundGetPosition();
int SoundIsBusy();
void SoundWaitForExit();
void SoundSetResolution( int resolution );

int main( int argc, char* argv[] ){
    return 0;
}

void SoundSetResolution( int resolution ){
    g_resolution = resolution;
}

int SoundIsBusy(){
    return g_playing;
}

double SoundGetPosition(){
    if( g_playing ){
        return g_processed_samples / (double)g_sample_rate;
    }else{
        return 0.0;
    }
}

void SoundWaitForExit(){
    if( !g_playing ){
        return;
    }
    g_last_buffer = g_current_wbuffer;
    g_exit_required = TRUE;
    while( TRUE ){
        int sum = 0;
        int i;
        for( i = 0; i < NUM_BUF; i++ ){
            if( !g_done[i] ){
                sum++;
            }
        }
#ifdef _TEST
        printf( "SoundWaitForExit; sum=%d\n", sum );
#endif
        if( sum == 0 ){
            break;
        }
    }
    SoundReset();
}

void SoundReset(){
    if( g_playing ){
        SDL_PauseAudio( 0 );
        SDL_CloseAudio();
    }
    int i;
    for( i = 0; i < NUM_BUF; i++ ){
        g_done[i] = TRUE;
        g_prepared[i] = FALSE;
        int j;
        for( j = 0; j < g_block_size; j++ ){
            g_buffer_left[i][j] = 16384;
            g_buffer_right[i][j] = 16384;
        }
    }
    g_current_wbuffer = 0;
    g_current_rbuffer = 0;
    g_buffer_writeloc = 0;
    g_buffer_readloc = 0;
    g_playing = FALSE;
    g_processed_samples = 0;
    g_exit_required = FALSE;
}

void SoundAppend( double *left, double *right, int length ){
    if( !g_playing ){
        SDL_AudioSpec fmt;

        fmt.freq = g_sample_rate;
        fmt.format = AUDIO_S16;
        fmt.channels = 2;
        fmt.samples = g_resolution;
        fmt.callback = WaveCallback;
        fmt.userdata = NULL;
        g_byte_per_sample = 4;

        if( SDL_OpenAudio( &fmt, NULL ) < 0 ){
            printf( "SDL_OpenAudio; error\n" );
            return;
        }

        SDL_PauseAudio( 0 );
        g_playing = TRUE;
    }
    while( !g_done[g_current_wbuffer] ){
        // 書き込むべきバッファがcallback読み出し終了するのを待つ
#ifdef _TEST
        printf( "SoundAppend; waiting for %d (1)\n", g_current_wbuffer );
#endif
        //SDL_Delay( 50 );
    }
    int first_length = length;
    if( g_buffer_writeloc + length >= g_block_size ){
        // g_current_wbufferに全部を書ききれない場合
        first_length = g_block_size - g_buffer_writeloc;
    }
    int i;
    int pos = 0;
    for( i = 0; i < first_length; i++ ){
#ifdef _TEST
        //printf( "%f\t%f\n", left[pos], right[pos] );
#endif
        g_buffer_left[g_current_wbuffer][g_buffer_writeloc + i]  = (Uint16)((left[pos] + 0.5)  * 16384);
        g_buffer_right[g_current_wbuffer][g_buffer_writeloc + i] = (Uint16)((right[pos] + 0.5) * 16384);
        pos++;
    }
    g_buffer_writeloc += first_length;
    if( g_buffer_writeloc >= g_block_size ){
        g_prepared[g_current_wbuffer] = TRUE;
        g_done[g_current_wbuffer] = FALSE;
        g_buffer_writeloc = 0;
        g_current_wbuffer++;
        if( g_current_wbuffer >= NUM_BUF ){
            g_current_wbuffer = 0;
        }
        int remain = length - first_length;
        if( remain > 0 ){
            // 次のバッファに行く場合
            while( !g_done[g_current_wbuffer] ){
                // callback読み出しが終了するのを待機
#ifdef _TEST
                printf( "SoundAppend; waiting for %d (2)\n", g_current_wbuffer );
#endif
                //SDL_Delay( 50 );
            }
            for( i = 0; i < remain; i++ ){
#ifdef _TEST
                //printf( "%f\t%f\n", left[pos], right[pos] );
#endif
                g_buffer_left[g_current_wbuffer][g_buffer_writeloc + i]  = (Uint16)((left[pos] + 0.5)  * 16384);
                g_buffer_right[g_current_wbuffer][g_buffer_writeloc + i] = (Uint16)((right[pos] + 0.5) * 16384);
                pos++;
            }
            g_buffer_writeloc += remain;
            // length <= g_block_sizeなので，3個目のバッファに移行することはない
        }
    }
}

void SoundInit( int block_size, int sample_rate ){
    g_sample_rate = sample_rate;
    g_block_size = block_size;
    g_processed_samples = 0;
    g_buffer_writeloc = 0;
    g_buffer_readloc = 0;
    g_playing = FALSE;
    g_resolution = 4410;

    if( SDL_Init( SDL_INIT_AUDIO ) < 0 ){
        printf( "SDL_Init; error\n" );
        return;
    }

    int i;
    for( i = 0; i < NUM_BUF; i++ ){
        g_buffer_left[i] = (Uint16*)calloc( g_block_size, sizeof( Uint16 ) );
        g_buffer_right[i] = (Uint16*)calloc( g_block_size, sizeof( Uint16 ) );
    }
    SoundReset();
}

void WaveCallback( void *unused, Uint8 *stream, int len ){
    int length = len / g_byte_per_sample; //このcallbackで送信しなければならないサンプル数
#ifdef _TEST
    printf( "WaveCallback; length=%d\n", length );
#endif
    while( !(g_exit_required && g_current_rbuffer == g_last_buffer) && !g_prepared[g_current_rbuffer] ){
        // 現在のバッファが準備済みとなるのを待機
#ifdef _TEST
        printf( "WaveCallback; waiting for %d (1)\n", g_current_rbuffer );
#endif
        //SDL_Delay( 50 );
    }
    int first_length = length;
    if( g_buffer_readloc + first_length >= g_block_size ){
        first_length = g_block_size - g_buffer_readloc;
    }
    if( g_exit_required && g_current_rbuffer == g_last_buffer && g_buffer_readloc + first_length > g_buffer_writeloc ){
        first_length = g_buffer_writeloc - g_buffer_readloc;
        if( first_length < 0 ){
            first_length = 0;
        }
    }
    int pos = 0;
    int i;
    for( i = 0; i < first_length; i++ ){
        Uint16 v = g_buffer_right[g_current_rbuffer][g_buffer_readloc + i];
        stream[pos++] = v & 0xff;
        stream[pos++] = (v >> 8) & 0xff;
        v = g_buffer_left[g_current_rbuffer][g_buffer_readloc + i];
        stream[pos++] = v & 0xff;
        stream[pos++] = (v >> 8) & 0xff;
    }
    if( g_exit_required && g_current_rbuffer && g_last_buffer ){
        for( i = 0; i < NUM_BUF; i++ ){
            g_done[i] = TRUE;
        }
    }
    g_buffer_readloc += first_length;
    g_processed_samples += first_length;
    if( g_buffer_readloc >= g_block_size ){
        g_prepared[g_current_rbuffer] = FALSE;
        g_done[g_current_rbuffer] = TRUE;
        g_buffer_readloc = 0;
        g_current_rbuffer++;
        if( g_current_rbuffer >= NUM_BUF ){
            g_current_rbuffer = 0;
        }
        int remain = length - first_length;
        if( g_exit_required && g_current_rbuffer == g_last_buffer && g_buffer_readloc + remain > g_buffer_writeloc ){
            remain = g_buffer_writeloc - g_buffer_readloc;
            if( remain < 0 ){
                remain = 0;
            }
        }
        if( remain > 0 ){
            // 次のバッファに行く場合
            while( !(g_exit_required && g_current_rbuffer == g_last_buffer) && !g_prepared[g_current_rbuffer] ){
                // 書き込みが終了するのを待機
#ifdef _TEST
                printf( "WaveCallback; waiting for %d (2)\n", g_current_rbuffer );
#endif
                //SDL_Delay( 50 );
            }
            for( i = 0; i < remain; i++ ){
                Uint16 v = g_buffer_right[g_current_rbuffer][g_buffer_readloc + i];
                stream[pos++] = v & 0xff;
                stream[pos++] = (v >> 8) & 0xff;
                v = g_buffer_left[g_current_rbuffer][g_buffer_readloc + i];
                stream[pos++] = v & 0xff;
                stream[pos++] = (v >> 8) & 0xff;
            }
            g_buffer_readloc += remain;
            g_processed_samples += remain;
        }
        if( g_exit_required && g_current_rbuffer && g_last_buffer ){
            for( i = 0; i < NUM_BUF; i++ ){
                g_done[i] = TRUE;
            }
        }
    }
}
#ifdef __cplusplus
} // extern "C"
#endif

