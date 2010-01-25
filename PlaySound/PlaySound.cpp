/*
 * waveplay.cpp
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
//#define TEST 1
#include "PlaySound.h"

int          waveplay::s_block_size  = 44100;
int          waveplay::s_sample_rate = 44100;
HWAVEOUT     waveplay::s_hwave_out = 0;
WAVEFORMATEX   waveplay::s_wave_formatx;
WAVEHDR        waveplay::s_wave_header[waveplay::_NUM_BUF];
unsigned long* waveplay::s_wave[waveplay::_NUM_BUF];
bool           waveplay::s_done[waveplay::_NUM_BUF];
bool           waveplay::s_abort_required = false;
int            waveplay::s_buffer_loc = 0;
int            waveplay::s_current_buffer = 0;
unsigned int   waveplay::s_processed_count = 0;
bool           waveplay::s_playing = false;
int            waveplay::s_error_samples = 0;
int            waveplay::s_last_buffer = -1;
FirstBufferWrittenCallback waveplay::s_first_buffer_written_callback;
double        *waveplay::s_wave_buffer_l;
double        *waveplay::s_wave_buffer_r;
#ifdef TEST
ofstream       waveplay::logger;
#endif

void waveplay::terminate(){
    if( s_hwave_out ){
        waveOutReset( s_hwave_out );
#ifdef TEST
#ifdef __cplusplus_cli
        System::Console::WriteLine( "waveplay::terminate; waveOutReset" );
#else
        waveplay::logger << "waveplay::terminate; waveOutReset" << endl;
#endif
#endif
        for( int k = 0; k < _NUM_BUF; k++ ){
            waveOutUnprepareHeader( s_hwave_out, &s_wave_header[k], sizeof( WAVEHDR ) );
        }
        waveOutClose( s_hwave_out );
    }
    for( int i = 0; i < _NUM_BUF; i++ ){
        if( s_wave[i] ){
            delete [] s_wave[i];
        }
    }
};

void waveplay::set_first_buffer_written_callback( FirstBufferWrittenCallback proc ){
#ifdef TEST
#ifdef __cplusplus_cli
    debug::push_log( "waveplay::set_first_buffer_written_callback" );
#else
    waveplay::logger << "waveplay::set_first_buffer_wirtten_callback" << endl;
#endif
#endif
    s_first_buffer_written_callback = proc;
};

void waveplay::reset(){
#ifdef TEST
	waveplay::logger << "waveplay::reset; s_hwave_out=" << (int)s_hwave_out << endl;
#endif
    s_playing = false;
    s_abort_required = true;
    if( 0 != s_hwave_out ){
        for( int k = 0; k < _NUM_BUF; k++ ){
            s_wave_header[k].dwUser = 1;
        }
        waveOutReset( s_hwave_out );

	    for( int k = 0; k < _NUM_BUF; k++ ){
	        waveOutUnprepareHeader( s_hwave_out, &s_wave_header[k], sizeof( WAVEHDR ) );
	        free( s_wave_header[k].lpData );
	    }
	    waveOutClose( s_hwave_out );
	    s_hwave_out = 0;
    }

	if( NULL != s_wave_buffer_l ){
		delete [] s_wave_buffer_l;
		s_wave_buffer_l = NULL;
	}
	if( NULL != s_wave_buffer_r ){
		delete [] s_wave_buffer_r;
		s_wave_buffer_r = NULL;
	}
#ifdef TEST
	waveplay::logger << "waveplay::reset; done" << endl;
#endif
}

void waveplay::append( double** data, unsigned int length, double amp_left, double amp_right ){
    unsigned int remain = length;
    unsigned int push_length = (length > s_block_size) ? s_block_size : length;
    double **a_data = new double*[2];
    a_data[0] = new double[push_length];
    a_data[1] = new double[push_length];
    int offset = 0;
    while( remain > 0 ){
        if( s_abort_required ){
            s_abort_required = false;
            break;
        }
        for( int i = 0; i < push_length; i++ ){
            a_data[0][i] = data[0][i + offset];
            a_data[1][i] = data[1][i + offset];
        }
        append_cor( a_data, push_length, amp_left, amp_right, false );
        remain -= push_length;
        offset += push_length;
        push_length = (remain > s_block_size) ? s_block_size : remain;
    }
    delete a_data[0];
    delete a_data[1];
    delete [] a_data;
}

void waveplay::flush_and_exit( double amp_left, double amp_right ){
    append_cor( (double**)0, 0, amp_left, amp_right, true );
}

void waveplay::append_cor( double** a_data, unsigned int length, double amp_left, double amp_right, bool is_last_mode ){
#ifdef TEST
    waveplay::logger << "append_cor *************************************************************" << endl;
#endif
    s_playing = true;
    int jmax = length;
    int remain = 0;
    double **data = new double*[2];
    bool cleaning_required = true;
    if( s_error_samples > 0 ){
        if( s_error_samples >= length ){
            s_error_samples -= length;
            return;
        }
        int actual_length = length - s_error_samples;
        data[0] = new double[actual_length];
        data[1] = new double[actual_length];
        for( int i = 0; i < actual_length; i++ ){
            data[0][i] = a_data[0][i + s_error_samples];
            data[1][i] = a_data[1][i + s_error_samples];
        }
        s_error_samples = 0;
        length = actual_length;
        jmax = length;
    }else{
        data[0] = new double[length];
        data[1] = new double[length];
        for( int i = 0; i < length; i++ ){
            data[0][i] = a_data[0][i];
            data[1][i] = a_data[1][i];
        }
    }

    if( length + s_buffer_loc >= s_block_size ){
        jmax = s_block_size - s_buffer_loc;
        remain = length - jmax;
    }
    double aright = amp_right;
    double aleft = amp_left;

    for( int j = 0; j < jmax; j++ ){
        s_wave_buffer_l[j + s_buffer_loc] = data[1][j];
        s_wave_buffer_r[j + s_buffer_loc] = data[0][j];
    }
    s_buffer_loc += jmax;

    if( s_buffer_loc >= s_block_size ){
        // バッファー充填完了．バッファーを転送し、waveOutWriteが書き込めるタイミングまで待機
#ifdef TEST
        logger << "append_cor; waiting(1) " << s_current_buffer << "..." << endl;
#endif
        while( true ){
        	Sleep( 0 );
            if( s_abort_required ){
                s_abort_required = false;
                goto clean_and_exit;
            }
            if( s_done[s_current_buffer] ){
                break;
            }
        }
#ifdef TEST
        logger << "append_cor; ...exit" << endl;
#endif

        s_processed_count++;
        mix( s_processed_count, aleft, aright );

        if( s_processed_count == _NUM_BUF ){
            s_done[0] = false;
            MMRESULT ret = waveOutWrite( s_hwave_out, &s_wave_header[0], sizeof( WAVEHDR ) );
            if( s_first_buffer_written_callback ){
#ifdef TEST
                logger << "append_cor; calling s_first_buffer_written_callback" << endl;
#endif
                s_first_buffer_written_callback();
            }
            for( int buffer_index = 1; buffer_index < _NUM_BUF; buffer_index++ ){
                s_done[buffer_index] = false;
                MMRESULT ret2 = waveOutWrite( s_hwave_out, &s_wave_header[buffer_index], sizeof( WAVEHDR ) );
            }
            s_current_buffer = _NUM_BUF - 1;
        }else if( s_processed_count > _NUM_BUF ){
            s_done[s_current_buffer] = false;
            MMRESULT ret3 = waveOutWrite( s_hwave_out, &s_wave_header[s_current_buffer], sizeof( WAVEHDR ) );
        }
        s_current_buffer++;
        if( s_current_buffer >= _NUM_BUF ){
            s_current_buffer = 0;
        }

        s_buffer_loc = 0;
    }

    if( remain > 0 ){
        for( int j = jmax; j < length; j++ ){
            s_wave_buffer_l[j - jmax] = data[1][j];
            s_wave_buffer_r[j - jmax] = data[0][j];
        }
        if( is_last_mode ){
            for( int j = length - jmax; j < s_block_size; j++ ){
                s_wave_buffer_l[j] = 0.0f;
                s_wave_buffer_r[j] = 0.0f;
            }
        }
        s_buffer_loc = remain;
    }

#ifdef TEST
#ifdef __cplusplus_cli
    System::Console::WriteLine( "append_cor; is_last_mode=" + is_last_mode );
#endif
#endif
    if( is_last_mode ){
        if( s_processed_count < _NUM_BUF ){
            // _NUM_BUFブロック分のデータを未だ全て受信していない場合。バッファが未だひとつも書き込まれていないので
            // 0番のブロックから順に書き込む
            s_processed_count++;
            mix( s_processed_count, aleft, aright );
            s_done[0] = false;
            waveOutWrite( s_hwave_out, &s_wave_header[0], sizeof( WAVEHDR ) );
            if( s_first_buffer_written_callback ){
#ifdef TEST
#ifdef __cplusplus_cli
                debug::push_log( "append_cor; calling s_first_buffer_written_callback" );
#else
                logger << "append_cor; calling s_first_buffer_written_callback" << endl;
#endif
#endif
                s_first_buffer_written_callback();
            }
            for( int i = 1; i < _NUM_BUF - 1; i++ ){
                s_processed_count++;
                mix( s_processed_count, aleft, aright );
                s_done[i] = false;
                waveOutWrite( s_hwave_out, &s_wave_header[i], sizeof( WAVEHDR ) );
            }
        }
        unsigned long zero = MAKELONG( 0, 0 );
        for( int j = s_buffer_loc; j < s_block_size; j++ ){
            s_wave_buffer_l[j] = 0.0f;
            s_wave_buffer_r[j] = 0.0f;
        }
#ifdef TEST
        logger << "append_cor; waiting(3) " << s_current_buffer << "..." << endl;
#endif
        while( !s_done[s_current_buffer] ){
        	Sleep( 0 );
            if( s_abort_required ){
                s_abort_required = false;
                goto clean_and_exit;
            }
        }
#ifdef TEST
        logger << "append_cor; ...exit" << endl;
#endif
        s_processed_count++;
        mix( s_processed_count, aleft, aright );
        s_done[s_current_buffer] = false;
        MMRESULT ret4 = waveOutWrite( s_hwave_out, &s_wave_header[s_current_buffer], sizeof( WAVEHDR ) );
    }
clean_and_exit:
    if( is_last_mode ){
        s_last_buffer = s_current_buffer;
    }
    if( cleaning_required ){
        delete [] data[0];
        delete [] data[1];
        delete [] data;
    }
};

void waveplay::mix( int processed_count, float amp_left, float amp_right ){
    int current_buffer = (processed_count - 1) % _NUM_BUF;
    for( int i = 0; i < s_block_size; i++ ){
        float l = s_wave_buffer_l[i] * amp_left;
        float r = s_wave_buffer_r[i] * amp_right;
        s_wave[current_buffer][i] = MAKELONG( (unsigned short)(r * 32768.0f), (unsigned short)(l * 32768.0f) );
    }
}

void waveplay::on_your_mark(){
    for( int k = 0; k < _NUM_BUF; k++ ){
        s_wave_header[k].dwUser = 0;
        s_done[k] = true;
    }
    s_abort_required = false;
    s_buffer_loc = 0;
    s_current_buffer = 0;
    s_processed_count = 0;
    s_playing = true;
    s_last_buffer = -1;

    if( NULL == s_wave_buffer_l ){
        s_wave_buffer_l = new double[s_block_size];
    }
    if( NULL == s_wave_buffer_r ){
        s_wave_buffer_r = new double[s_block_size];
    }
    for( int i = 0; i < s_block_size; i++ ){
        s_wave_buffer_l[i] = 0.0;
        s_wave_buffer_r[i] = 0.0;
    }
    unsigned long zero = MAKELONG( 0, 0 );
    for( int i = 0; i < s_block_size; i++ ){
        for( int k = 0; k < _NUM_BUF; k++ ){
            s_wave[k][i] = zero;
        }
    }
}

double waveplay::get_play_time(){
    if( s_playing ){
        MMTIME mmt;
        mmt.wType = TIME_MS;
        waveOutGetPosition( s_hwave_out, &mmt, sizeof( MMTIME ) );
        float ms = 0.0;
        switch( mmt.wType ){
            case TIME_MS:
                return mmt.u.ms * 0.001;
                break;
            case TIME_SAMPLES:
                return (double)mmt.u.sample / (double)s_wave_formatx.nSamplesPerSec;
            case TIME_BYTES:
                return (double)mmt.u.cb / (double)s_wave_formatx.nAvgBytesPerSec;
            default:
                return -1.0;
        }
        return 0.0;
    }else{
        return -1.0;
    }
}

void waveplay::init( int sample_rate ){
#ifdef TEST
	waveplay::logger << "waveplay::init; sample_rate=" << sample_rate << "; s_hwave_out=" << (int)s_hwave_out << endl;
#endif
	if( s_hwave_out != 0 ){
		reset();
	}

    s_block_size = sample_rate;
    s_sample_rate = sample_rate;

    s_wave_formatx.wFormatTag = WAVE_FORMAT_PCM;
    s_wave_formatx.nChannels = 2;
    s_wave_formatx.wBitsPerSample = 16;
    s_wave_formatx.nBlockAlign = s_wave_formatx.nChannels * s_wave_formatx.wBitsPerSample / 8;
    s_wave_formatx.nSamplesPerSec = s_sample_rate;
    s_wave_formatx.nAvgBytesPerSec = s_wave_formatx.nSamplesPerSec * s_wave_formatx.nBlockAlign;

    waveOutOpen( &s_hwave_out, WAVE_MAPPER, &s_wave_formatx, (unsigned long)wave_callback, 0, CALLBACK_FUNCTION );
    
    unsigned long zero = MAKELONG( 0, 0 );
    for( int k = 0; k < _NUM_BUF; k++ ){
        s_wave[k] = (unsigned long*)malloc( sizeof( unsigned long ) * s_block_size );
        memset( s_wave[k], zero, s_block_size * sizeof( unsigned long ) );
        s_wave_header[k].lpData = (char*)s_wave[k];
        s_wave_header[k].dwBufferLength = sizeof( unsigned long ) * s_block_size;
        s_wave_header[k].dwFlags = WHDR_BEGINLOOP | WHDR_ENDLOOP;
        s_wave_header[k].dwLoops = 1;

        waveOutPrepareHeader( s_hwave_out, &s_wave_header[k], sizeof( WAVEHDR ) );
        s_wave_header[k].dwUser = 0;
    }
}

void waveplay::abort(){
#ifdef TEST
	waveplay::logger << "waveplay::abort" << endl;
#endif
    s_abort_required = true;
    s_buffer_loc = 0;
    s_current_buffer = 0;
    s_processed_count = 0;
#ifdef TEST
	waveplay::logger << "waveplay::abort; done" << endl;
#endif
}

void CALLBACK waveplay::wave_callback( HWAVEOUT hwo, unsigned int uMsg, unsigned long dwInstance, unsigned long dwParam1, unsigned long dwParam2 ){
    if ( uMsg == MM_WOM_DONE ){
        int index_done = 0;
        for( int k = 0; k < _NUM_BUF; k++ ){
            if( (LPWAVEHDR)dwParam1 == &s_wave_header[k] ){
                index_done = k;
#ifdef TEST
#ifdef __cplusplus_cli
                System::DateTime ^now = System::DateTime::Now;
                System::String ^nowstr = now->Day + " " + now->Hour + ":" + now->Minute + ":" + (double)(now->Second + now->Millisecond / 1000.0);
                System::Console::WriteLine( "waveplay::wave_callback; now=" + nowstr + "; done=" + index_done + "; last_buffer=" + s_last_buffer + "; dwUser=" + s_wave_header[k].dwUser );
#else
                waveplay::logger << "waveplay::wave_callback; done=" << index_done << std::endl;
#endif
#endif
                s_done[index_done] = true;
                if( s_last_buffer == index_done ){
                    s_playing = false;
                }
                if( s_wave_header[k].dwUser != 0 ){
                    s_wave_header[k].dwUser = 0;
                }
                break;
            }
        }
    }
    return;
}

#ifdef __cplusplus_cli
System::String ^waveplay::util_get_errmsg( MMRESULT msg ){
    wchar_t err[260];
    mciGetErrorStringW( msg, err, 260 );
#else
string waveplay::util_get_errmsg( MMRESULT msg ){
    char err[260];
    mciGetErrorStringA( msg, err, 260 );
#endif
    int len = 260;
    for( int i = 1; i < 260; i++ ){
        if( err[i] == '\0' ){
            len = i - 1;
            break;
        }
    }
#ifdef __cplusplus_cli
    array<wchar_t> ^errstr = gcnew array<wchar_t>( len );
    for( int i = 0; i < len; i++ ){
        errstr[i] = err[i];
    }
    return gcnew System::String( errstr, 0, len );
#else
    string ret( err );
    return ret;
#endif
}

bool waveplay::is_alive(){
    return s_playing;
}

extern "C" {
	void SoundSetResolution( int resolution ){
		// do nothing
	}

    void SoundInit( int sample_rate ){
#ifdef TEST
        waveplay::logger.open( "test.log", ios::out | ios::app );
		waveplay::logger << "SoundInit" << endl;
#endif
        waveplay::init( sample_rate );
    }

    void SoundAppend( double *left, double *right, int length ){
#ifdef TEST
		waveplay::logger << "SoundAppend" << endl;
#endif
        if( !waveplay::is_alive() ){
            //waveplay::reset();
            waveplay::on_your_mark();
        }
        double *out[] = { left, right };
        waveplay::append( out, length, 1.0, 1.0 );
    }

    void SoundReset(){
#ifdef TEST
		waveplay::logger << "SoundReset" << endl;
#endif
        waveplay::abort();
        waveplay::reset();
    }

    double SoundGetPosition(){
#ifdef TEST
		waveplay::logger << "SoundGetPosition" << endl;
#endif
        if( waveplay::is_alive() ){
            return waveplay::get_play_time();
        }else{
            return 0.0;
        }
    }

    bool SoundIsBusy(){
#ifdef TEST
		waveplay::logger << "SoundIsBusy" << endl;
#endif
        return waveplay::is_alive();
    }

    void SoundWaitForExit(){
#ifdef TEST
		waveplay::logger << "SoundWaitForExit" << endl;
#endif
        if( waveplay::is_alive() ){
            waveplay::flush_and_exit( 1.0, 1.0 );
        }
        while( waveplay::is_alive() ){
            Sleep( 0 );
        }
        //waveplay::reset();
    }

    void SoundTerminate(){
#ifdef TEST
		waveplay::logger << "SoundTerminate" << endl;
#endif
        waveplay::terminate();
    }
}

int main(){
#ifdef TEST
    waveplay::logger.open( "test.log", ios::out );
#endif
    const int sample_rate = 48000;

    int f = 200;
    int len = sample_rate / f;    //波長
    double *left = new double[sample_rate];
    double *right = new double[sample_rate];
    for( int i = 0; i < sample_rate; i++ ){  //波形データ作成
        if( i % len < len / 2 ){
            left[i] = 0.2f;
            right[i] = 0.2f;
        }else{
            left[i] = -0.2f;
            right[i] = -0.2f;
        }
    }

    std::cout << "is_alive=" << (waveplay::is_alive() ? "True" : "False") << endl;

    SoundInit( sample_rate );
    for( int i = 0; i < 5; i++ ){
        SoundAppend( left, right, sample_rate );
    }
    SoundWaitForExit();
    return 0;
}
