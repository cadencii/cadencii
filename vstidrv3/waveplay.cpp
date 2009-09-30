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
#include "waveplay.h"

int          waveplay::s_block_size  = 44100;
int          waveplay::s_sample_rate = 44100;
HWAVEOUT     waveplay::s_hwave_out;
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
wavereader    *waveplay::s_wave_reader;
int            waveplay::s_num_wave_reader;
float        **waveplay::s_another_wave_l;
float        **waveplay::s_another_wave_r;
__int64        waveplay::s_wave_read_offset_samples;
float         *waveplay::s_wave_buffer_l;
float         *waveplay::s_wave_buffer_r;

bool waveplay::change_block_size( int block_size ){
    if ( s_playing ){
        return false;
    }
    if( block_size <= 0 ){
        return false;
    }

    for( int k = 0; k < _NUM_BUF; k++ ){
        if( s_wave[k] ) delete [] s_wave[k];
        s_wave[k] = (unsigned long*)calloc( sizeof( unsigned long ), block_size );
        s_wave_header[k].lpData = (char*)s_wave[k];
        s_wave_header[k].dwBufferLength = sizeof( unsigned long ) * block_size;
    }

    // s_wave_buffer_l, s_wave_buffer_rは、NULLならばon_your_markで初期化されるので、開放だけやっておけばOK
    if( s_wave_buffer_l ) delete [] s_wave_buffer_l;
    if( s_wave_buffer_r ) delete [] s_wave_buffer_r;
    // s_another_wave_l, s_another_wave_rは、on_your_markで全自動で初期化されるので特に操作の必要なし
    s_block_size = block_size;
    return true;
}

void waveplay::terminate(){
    if( s_hwave_out ){
        waveOutReset( s_hwave_out );
#ifdef TEST
#ifdef __cplusplus_cli
        System::Console::WriteLine( "waveplay::terminate; waveOutReset" );
#else
        cout << "waveplay::terminate; waveOutReset" << endl;
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
    debug::logger << "waveplay::set_first_buffer_wirtten_callback" << endl;
#endif
#endif
    s_first_buffer_written_callback = proc;
};

void waveplay::set_error_samples( int error_samples ){
    s_error_samples = error_samples;
};

#ifdef __cplusplus_cli
int waveplay::on_your_mark( array<System::String ^> ^files, __int64 wave_read_offset_samples ){
	int num_files = files->Length;
#else
int waveplay::on_your_mark( char **files, int num_files, __int64 wave_read_offset_samples ){
#endif
    reset();
    s_wave_read_offset_samples = wave_read_offset_samples;
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

    if( !s_wave_buffer_l ){
        s_wave_buffer_l = new float[s_block_size];
    }
    if( !s_wave_buffer_r ){
        s_wave_buffer_r = new float[s_block_size];
    }

    if( s_wave_reader ){
        for( int i = 0; i < s_num_wave_reader; i++ ){
            s_wave_reader[i].close();
        }
        delete [] s_wave_reader;
    }
    s_wave_reader = new wavereader[num_files];

    if( s_another_wave_l ){
        for( int i = 0; i < s_num_wave_reader; i++ ){
            delete [] s_another_wave_l[i];
        }
        delete [] s_another_wave_l;
    }
    if( s_another_wave_r ){
        for( int i = 0; i < s_num_wave_reader; i++ ){
            delete [] s_another_wave_r[i];
        }
        delete [] s_another_wave_r;
    }
    s_another_wave_l = new float*[num_files];
    s_another_wave_r = new float*[num_files];
    int max_samples = 0;
    for( int i = 0; i < num_files; i++ ){
        // waveファイルヘッダを読込む
#ifdef __cplusplus_cli
        int len = files[i]->Length;
        wchar_t *name = new wchar_t[len + 1];
        array<wchar_t> ^buf = files[i]->ToCharArray();
        for( int k = 0; k < len; k++ ){
            name[k] = buf[k];
        }
        name[len] = '\0';
        int samples = s_wave_reader[i].open( name );
        if( samples > max_samples ){
            max_samples = samples;
        }
        delete [] name;
#else
		s_wave_reader[i].open( files[i] );
#endif

        // バッファを用意
        s_another_wave_l[i] = new float[s_block_size];
        s_another_wave_r[i] = new float[s_block_size];
    }
    s_num_wave_reader = num_files;
    return max_samples;
};

void waveplay::reset(){
    s_playing = false;
    s_abort_required = true;
    if( s_hwave_out ){
        for( int k = 0; k < _NUM_BUF; k++ ){
            s_wave_header[k].dwUser = 1;
        }
        waveOutReset( s_hwave_out );
        unsigned long zero = MAKELONG( 0, 0 );
        for( int k = 0; k < _NUM_BUF; k++ ){
            for( int i = 0; i < s_block_size; i++ ){
                s_wave[k][i] = zero;
            }
        }
    }
    for( int i = 0; i < s_num_wave_reader; i++ ){
        s_wave_reader[i].close();
    }
};

void waveplay::append( double** data, unsigned int length, double amp_left, double amp_right ){
	float* l = new float[length];
	float* r = new float[length];
	float* out[] = { l, r };
	append( out, length, amp_left, amp_right );
	delete [] l;
	delete [] r;
}

//todo: lengthがs_block_sizeよりも大きい場合の処理。
void waveplay::append( float** data, unsigned int length, double amp_left, double amp_right ){
    append_cor( data, length, amp_left, amp_right, false );
}

void waveplay::flush_and_exit( double amp_left, double amp_right ){
    append_cor( (float**)0, 0, amp_left, amp_right, true );
}

void waveplay::append_cor( float** a_data, unsigned int length, double amp_left, double amp_right, bool is_last_mode ){
#ifdef TEST
    debug::push_log( "append_cor *************************************************************" );
#endif
    s_playing = true;
    int jmax = length;
    int remain = 0;
    float **data = new float*[2];
    bool cleaning_required = false;
    if( s_error_samples > 0 ){
        if( s_error_samples >= length ){
            s_error_samples -= length;
            return;
        }
        cleaning_required = true;
        int actual_length = length - s_error_samples;
        data[0] = new float[actual_length];
        data[1] = new float[actual_length];
        for( int i = 0; i < actual_length; i++ ){
            data[0][i] = a_data[0][i + s_error_samples];
            data[1][i] = a_data[1][i + s_error_samples];
        }
        s_error_samples = 0;
        length = actual_length;
        jmax = length;
    }else{
        data = a_data;
    }

    if( length + s_buffer_loc >= s_block_size ){
        jmax = s_block_size - s_buffer_loc;
        remain = length - jmax;
    }
    float aright = (float)amp_right;
    float aleft = (float)amp_left;

    for( int j = 0; j < jmax; j++ ){
        s_wave_buffer_l[j + s_buffer_loc] = data[1][j];
        s_wave_buffer_r[j + s_buffer_loc] = data[0][j];
    }
    s_buffer_loc += jmax;

    if( s_buffer_loc >= s_block_size ){
        // バッファー充填完了．バッファーを転送し、waveOutWriteが書き込めるタイミングまで待機
#ifdef TEST
        debug::push_log( "append_cor; waiting(1) " + s_current_buffer + "..." );
#endif
        while( true ){
            if( s_abort_required ){
                s_abort_required = false;
                goto clean_and_exit;
            }
            //if( (s_wave_header[s_current_buffer].dwFlags & WHDR_INQUEUE) != WHDR_INQUEUE ){
            if( s_done[s_current_buffer] ){
                break;
            }
        }
#ifdef TEST
        debug::push_log( "append_cor; ...exit" );
#endif

        s_processed_count++;
        mix( s_processed_count, aleft, aright );

        if( s_processed_count == _NUM_BUF ){
            s_done[0] = false;
            MMRESULT ret = waveOutWrite( s_hwave_out, &s_wave_header[0], sizeof( WAVEHDR ) );
            if( s_first_buffer_written_callback ){
#ifdef TEST
                debug::push_log( "append_cor; calling s_first_buffer_written_callback" );
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
                debug::logger << "append_cor; calling s_first_buffer_written_callback" << endl;
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
        debug::push_log( "append_cor; waiting(3) " + s_current_buffer + "..." );
#endif
        while( !s_done[s_current_buffer] ){
        //while( (s_wave_header[s_current_buffer].dwFlags & WHDR_INQUEUE) == WHDR_INQUEUE ){
            if( s_abort_required ){
                s_abort_required = false;
                goto clean_and_exit;
            }
        }
#ifdef TEST
        debug::push_log( "append_cor; ...exit" );
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
    for( int k = 0; k < s_num_wave_reader; k++ ){
        s_wave_reader[k].read( s_block_size * (processed_count - 1) + s_wave_read_offset_samples, 
                               s_block_size,
                               s_another_wave_l[k],
                               s_another_wave_r[k] );
    }
    for( int i = 0; i < s_block_size; i++ ){
        float l = s_wave_buffer_l[i] * amp_left;
        float r = s_wave_buffer_r[i] * amp_right;
        for( int k = 0; k < s_num_wave_reader; k++ ){
            l += s_another_wave_l[k][i];
            r += s_another_wave_r[k][i];
        }
        s_wave[current_buffer][i] = MAKELONG( (unsigned short)(r * 32768.0f), (unsigned short)(l * 32768.0f) );
    }
}

float waveplay::get_play_time(){
    if( s_playing ){
        MMTIME mmt;
        mmt.wType = TIME_MS;
        waveOutGetPosition( s_hwave_out, &mmt, sizeof( MMTIME ) );
        float ms = 0.0;
        switch( mmt.wType ){
            case TIME_MS:
                return mmt.u.ms * 0.001f;
                break;
            case TIME_SAMPLES:
                return (float)mmt.u.sample / (float)s_wave_formatx.nSamplesPerSec;
            case TIME_BYTES:
                return (float)mmt.u.cb / (float)s_wave_formatx.nAvgBytesPerSec;
            default:
                return -1.0f;
        }
        return 0.0f;
    }else{
        return -1.0f;
    }
};

void waveplay::init( int block_size, int sample_rate ){
    s_block_size = block_size;
    s_sample_rate = sample_rate;

    s_wave_formatx.wFormatTag = WAVE_FORMAT_PCM;
    s_wave_formatx.nChannels = 2;
    s_wave_formatx.wBitsPerSample = 16;
    s_wave_formatx.nBlockAlign = s_wave_formatx.nChannels * s_wave_formatx.wBitsPerSample / 8;
    s_wave_formatx.nSamplesPerSec = s_sample_rate;
    s_wave_formatx.nAvgBytesPerSec = s_wave_formatx.nSamplesPerSec * s_wave_formatx.nBlockAlign;

    waveOutOpen( &s_hwave_out, WAVE_MAPPER, &s_wave_formatx, (unsigned long)wave_callback, 0, CALLBACK_FUNCTION );
    
    for( int k = 0; k < _NUM_BUF; k++ ){
        s_wave[k] = (unsigned long*)calloc( sizeof( unsigned long ), s_block_size );
        s_wave_header[k].lpData = (char*)s_wave[k];
        s_wave_header[k].dwBufferLength = sizeof( unsigned long ) * s_block_size;
        s_wave_header[k].dwFlags = WHDR_BEGINLOOP | WHDR_ENDLOOP;
        s_wave_header[k].dwLoops = 1;

        waveOutPrepareHeader( s_hwave_out, &s_wave_header[k], sizeof( WAVEHDR ) );
        s_wave_header[k].dwUser = 0;
    }
};

void waveplay::abort(){
    s_abort_required = true;
    reset();
    for( int k = 0; k < _NUM_BUF; k++ ){
        if( s_wave[k] ){
            memset( s_wave[k], 0, s_block_size * sizeof( unsigned long ) );
        }
    }
    s_buffer_loc = 0;
    s_current_buffer = 0;
    s_processed_count = 0;
};

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
                std::cout << "waveplay::wave_callback; done=" << index_done << std::endl;
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
};

bool waveplay::is_alive(){
    return s_playing;
};
