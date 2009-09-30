#include <fcntl.h>
#include <limits.h>
#include "soundcard.h"
#include <math.h>
#include <stdio.h>
#include <sys/ioctl.h>
#include <unistd.h>
#include <stdlib.h>

#define MAKELONG(a, b) ((long)(((unsigned short)(a)) | ((unsigned long)((unsigned short)(b))) << 16))

#ifdef __cplusplus
extern "C" {
#endif
void SoundInit( int block_size, int sample_rate );
void SoundAppend( double *left, double *right, int length );
void SoundReset();
double SoundGetPosition();
int SoundIsBusy();
void SoundWaitForExit();

static int setup_dsp( int fd );

static int fd = 0;
static int g_block_size = 44100;
static int g_sample_rate = 44100;
static int g_initialized = 0;
static int g_buffer_length = 44100;
static unsigned long *g_buffer;

void SoundWaitForExit(){
    if( fd == 0 ){
        return;
    }
}

double SoundGetPosition(){
    if( fd == 0 ){
        return -1;
    }
    
    oss_count_t ptr;
    if( ioctl( fd, SNDCTL_DSP_CURRENT_OPTR, &ptr ) == -1 ){
        return -1;
    }
    double time = (ptr.samples + ptr.fifo_samples) / (double)g_sample_rate;
    return time;
}

void SoundReset(){
    if( fd == 0 ){
        return;
    }
    ioctl( fd, SNDCTL_DSP_RESET, 0 );
}

void SoundAppend( double *left, double *right, int length ){
    if( fd == 0 ){
        return;
    }

    int remain = length;
    int i = 0;
    int pos = 0;
    while( remain > 0 ){
        unsigned long v = MAKELONG( (unsigned short)(right[i] * 32768.0), (unsigned short)(left[i] * 32768.0) );
        g_buffer[pos] = v;
        pos++;
        i++;
        remain--;
        if( pos >= g_buffer_length ){
            //ioctl( fd, SNDCTL_DSP_SYNC, 0 );
            write( fd, g_buffer, g_buffer_length * sizeof( unsigned long ) );
            pos = 0;
        }
    }

    if( pos > 0 ){
        write( fd, g_buffer, pos );
    }
}

void SoundInit( int block_size, int sample_rate ){
    g_block_size = block_size;
    g_sample_rate = sample_rate;
    int i;
    if ( ( fd = open( "/dev/dsp", O_WRONLY ) ) == -1 ) {
        perror( "open()" );
        return;
    }

    // /dev/dsp の設定
    if( setup_dsp( fd ) != 0 ){
        fprintf( stderr, "Setup /dev/dsp failed.\n" );
        close( fd );
        return;
    }

    // バッファを用意
    g_buffer = (unsigned long *)calloc( (size_t)g_buffer_length, sizeof( unsigned long ) );
    g_initialized = 1;
}


/// <summary>
/// /dev/dsp を以下の様に設定する。
/// </summary>
static int setup_dsp( int fd ){
    int fmt = AFMT_S16_LE;
    int channel = 2;

    /* サウンドフォーマットの設定 */
    if ( ioctl( fd, SNDCTL_DSP_SETFMT, &fmt ) == -1 ) {
        perror( "ioctl( SOUND_PCM_SETFMT )" );
        return -1;
    }

    /* チャンネル数の設定 */
    if ( ioctl( fd, SNDCTL_DSP_CHANNELS, &channel ) == -1 ) {
        perror( "iotcl( SOUND_PCM_WRITE_CHANNELS )" );
        return -1;
    }

    /* サンプリング周波数の設定 */
    if ( ioctl( fd, SNDCTL_DSP_SPEED, &g_sample_rate ) == -1 ) {
        perror( "iotcl( SOUND_PCM_WRITE_RATE )" );
        return -1;
    }

    return 0;
}
#ifdef __cplusplus
} // extern "C"
#endif

