/*
 * vocaloidrv.cpp
 * Copyright © 2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#include "vocaloidrv.h"

#ifdef TEST
FILE *vocaloidrv::flog = NULL;
#endif

vocaloidrv::~vocaloidrv()
{
	if( !mUseStdOut && mFile ){
		long pos = ftell( mFile );
		fseek( mFile, 0x4, SEEK_SET );
		unsigned int len = pos - 0x4 - 0x04;
		fwrite( &len, sizeof( unsigned int ), 1, mFile );
		fseek( mFile, 0x28, SEEK_SET );
		len = pos - 0x28 - 0x04;
		fwrite( &len, sizeof( unsigned int ), 1, mFile );
		fclose( mFile );
	}
	if( mBuffer ){
		free( mBuffer );
	}
}

bool vocaloidrv::waveIncoming( double *left, double *right, int len )
{
    int length = len;
    if( mTotalSamples <= mProcessed ){
        return true;
    }
    if( mProcessed + length >= mTotalSamples ){
        length = (int)(mTotalSamples - mProcessed);
    }
    bool ret = false;
    if( length != len ){
        ret = true;
    }
	if( mUseStdOut ){
		for( int i = 0; i < length; i++ ){
			WORD l = (WORD)(32768 * left[i]);
			WORD r = (WORD)(32768 * right[i]);
			putchar( 0xff & (l >> 8) );
			putchar( 0xff & l );
			putchar( 0xff & (r >> 8) );
			putchar( 0xff & r );
		}
	}else{
		if( !mFile ){
			mFile = fopen( "out.wav", "wb" );
			fwrite( "RIFF", 4 * sizeof( char ), 1, mFile );
			unsigned int riff_length = 0;
			fwrite( &riff_length, sizeof( unsigned int ), 1, mFile );
			fwrite( "WAVE", 4 * sizeof( char ), 1, mFile );
			fwrite( "fmt ", 4 * sizeof( char ), 1, mFile );
			// length of "fmt " chunk
			unsigned int fmt_len = 16;
			fwrite( &fmt_len, sizeof( unsigned int ), 1, mFile );
			// format id
			unsigned short format_id = 1;
			fwrite( &format_id, sizeof( unsigned short ), 1, mFile );
			// チャンネル数
			unsigned short channels = 2;
			fwrite( &channels, sizeof( unsigned short ), 1, mFile );
			// サンプリングレート
			unsigned int sample_rate = sampleRate;
			fwrite( &sample_rate, sizeof( unsigned int ), 1, mFile );
			// データ速度
			unsigned short bit_per_sample = 16;
			unsigned short block_size = channels * bit_per_sample / 8;
			unsigned int data_rate = sampleRate * block_size;
			fwrite( &data_rate, sizeof( unsigned int ), 1, mFile );
			// ブロックサイズ
			fwrite( &block_size, sizeof( unsigned short ), 1, mFile );
			// サンプルあたりのビット数
			fwrite( &bit_per_sample, sizeof( unsigned short ), 1, mFile );
			// dataチャンク
			fwrite( "data", 4 * sizeof( char ), 1, mFile );
			// dataチャンクの長さ
			unsigned int data_length = 0;
			fwrite( &data_length, sizeof( unsigned int ), 1, mFile );
		}

        if( mBufferCount < length ){
            mBuffer = (DWORD *)realloc( mBuffer, length * sizeof( DWORD ) );
            mBufferCount = length;
        }
        
		for( int i = 0; i < length; i++ ){
			WORD l = (WORD)(32768 * left[i]);
			WORD r = (WORD)(32768 * right[i]);
            mBuffer[i] = MAKELONG( r, l );
		}

        fwrite( mBuffer, length * sizeof( DWORD ), 1, mFile );
    }
    mProcessed += length;
	return ret;
}

void vocaloidrv::merge_events( vector<MidiEvent *> &x0, vector<MidiEvent *> &y0, vector<MidiEvent *> &ret )
{
    for ( int i = 0; i < x0.size(); i++ ) {
        ret.push_back( x0[i] );
    }
    for ( int i = 0; i < y0.size(); i++ ) {
        ret.push_back( y0[i] );
    }
    bool changed = true;
    while ( changed ) {
        changed = false;
        for ( int i = 0; i < ret.size() - 1; i++ ) {
            if ( ret[i]->compareTo( ret[i + 1] ) > 0 ) {
                MidiEvent *m = ret[i];
                ret[i] = ret[i + 1];
                ret[i + 1] = m;
                changed = true;
            }
        }

    }
}

/// <summary>
/// 
/// </summary>
/// <param name="total_samples"></param>
/// <param name="mode_infinite"></param>
/// <param name="sample_rate"></param>
/// <param name="runner">このドライバを駆動しているRenderingRunnerのオブジェクト</param>
/// <returns></returns>
uint64_t vocaloidrv::startRendering( uint64_t total_samples, bool mode_infinite, int sample_rate )
{
#if DEBUG
    sout.println( "VocaloidDriver#startRendering; entry; total_samples=" + total_samples + "; sample_rate=" + sample_rate );
#endif
    mIsRendering = true;
    mIsCancelRequired = false;
    mProcessed = 0;
    mTotalSamples = total_samples;
    sampleRate = sample_rate;

    vector<MidiEvent *> all_events;
    merge_events( mEvents0, mEvents1, all_events );
    int current_count = -1;
    MidiEvent *current = NULL;// = new MidiEvent();// = lpEvents;
    vector<void *> mman;

    float *left_ch;
    float *right_ch;
    float **out_buffer;

    left_ch = new float[sampleRate];// (float *)malloc( sizeof( float ) * sampleRate );
    right_ch = new float[sampleRate];// (float *)malloc( sizeof( float ) * sampleRate );
    out_buffer = new float*[2];// (float **)malloc( sizeof( float* ) * 2 );
    mman.push_back( left_ch );
    mman.push_back( right_ch );
    mman.push_back( out_buffer );
    out_buffer[0] = left_ch;
    out_buffer[1] = right_ch;

    double *buffer_l = new double[sampleRate];// (double *)malloc( sizeof( double ) * sampleRate );
    double *buffer_r = new double[sampleRate];// (double *)malloc( sizeof( double ) * sampleRate );
    mman.push_back( buffer_l );
    mman.push_back( buffer_r );

#if DEBUG
    sout.println( "VocaloidDriver#startRendering; sampleRate=" + sampleRate );
#endif
    aEffect->dispatcher( aEffect, effSetSampleRate, 0, 0, NULL, (float)sampleRate );
    aEffect->dispatcher( aEffect, effMainsChanged, 0, 1, NULL, 0 );

    // ここではブロックサイズ＝サンプリングレートということにする
    aEffect->dispatcher( aEffect, effSetBlockSize, 0, sampleRate, NULL, 0 );

    // レンダリングの途中で停止した場合，ここでProcessする部分が無音でない場合がある
    for ( int i = 0; i < 3; i++ ) {
        aEffect->processReplacing( aEffect, NULL, out_buffer, sampleRate );
    }

    int delay = 0;
    int duration = 0;
    int dwNow = 0;
    int dwPrev = 0;
    int dwDelta;
    int dwDelay = 0;
    int dwDeltaDelay = 0;

    int addr_msb = 0, addr_lsb = 0;
    int data_msb = 0, data_lsb = 0;

    uint64_t total_processed = 0;
    uint64_t total_processed2 = 0;
    dwDelay = 0;
    int list_size = mEvents1.size();
    for ( int i = 0; i < list_size; i++ ) {
        MidiEvent *work = mEvents1[i];
        if ( (work->firstByte & 0xf0) == 0xb0 ) {
            switch ( work->data[0] ) {
                case 0x63:{
                    addr_msb = work->data[1];
                    addr_lsb = 0;
                    break;
                }
                case 0x62:{
                    addr_lsb = work->data[1];
                    break;
                }
                case 0x06:{
                    data_msb = work->data[1];
                    break;
                }
                case 0x26:{
                    data_lsb = work->data[1];
                    if ( addr_msb == 0x50 && addr_lsb == 0x01 ) {
                        dwDelay = (data_msb & 0xff) << 7 | (data_lsb & 0x7f);
                    }
                    break;
                }
            }
        }
        if ( dwDelay > 0 ) {
            break;
        }
    }

    while ( !mIsCancelRequired ) {
        vector<void *> mman2;
        int process_event_count = current_count;
        int nEvents = 0;

        if ( current_count < 0 ) {
            current_count = 0;
            current = all_events[current_count];
            process_event_count = current_count;
        }
        while ( current->clock == dwNow ) {
            // durationを取得
            if ( (current->firstByte & 0xf0) == 0xb0 ) {
                switch ( current->data[0] ) {
                    case 0x63:{
                        addr_msb = current->data[1];
                        addr_lsb = 0;
                        break;
                    }
                    case 0x62:{
                        addr_lsb = current->data[1];
                        break;
                    }
                    case 0x06:{
                        data_msb = current->data[1];
                        break;
                    }
                    case 0x26:{
                        data_lsb = current->data[1];
                        // Note Duration in millisec
                        if ( addr_msb == 0x50 && addr_lsb == 0x4 ) {
                            duration = data_msb << 7 | data_lsb;
                        }
                        break;
                    }
                }
            }

            nEvents++;
            if ( current_count + 1 < all_events.size() ) {
                current_count++;
                current = all_events[current_count];
            } else {
                break;
            }
        }

        if ( current_count + 1 >= all_events.size() ) {
            break;
        }

        double msNow = msec_from_clock( dwNow );
        dwDelta = (int)((uint64_t)(msNow / 1000.0 * sampleRate) - total_processed);
        VstEvents *pVSTEvents = (VstEvents *)malloc( sizeof( VstEvent ) + nEvents * sizeof( VstEvent* ) );
        mman2.push_back( pVSTEvents );
        pVSTEvents->numEvents = 0;
        pVSTEvents->reserved = (VstIntPtr)0;

        for ( int i = 0; i < nEvents; i++ ) {
            MidiEvent *pProcessEvent = all_events[process_event_count];
            int event_code = pProcessEvent->firstByte;
            VstEvent *pVSTEvent = NULL;// (VstEvent *)0;
            VstMidiEvent *pMidiEvent = NULL;

            switch ( event_code ) {
                case 0xf0:
                case 0xf7:
                case 0xff:{
                    break;
                }
                default:{
                    pMidiEvent = (VstMidiEvent *)malloc( (int)(sizeof( VstMidiEvent ) + (pProcessEvent->dataLength + 1) * sizeof( unsigned char ) ) );
                    mman2.push_back( pMidiEvent );
                    pMidiEvent->byteSize = sizeof( VstMidiEvent );
                    pMidiEvent->deltaFrames = dwDelta;
                    pMidiEvent->detune = 0;
                    pMidiEvent->flags = 1;
                    pMidiEvent->noteLength = 0;
                    pMidiEvent->noteOffset = 0;
                    pMidiEvent->noteOffVelocity = 0;
                    pMidiEvent->reserved1 = 0;
                    pMidiEvent->reserved2 = 0;
                    pMidiEvent->type = kVstMidiType;
                    pMidiEvent->midiData[0] = (unsigned char)(0xff & pProcessEvent->firstByte);
                    for ( int j = 0; j < pProcessEvent->dataLength; j++ ) {
                        pMidiEvent->midiData[j + 1] = (unsigned char)(0xff & pProcessEvent->data[j]);
                    }
                    pVSTEvents->events[pVSTEvents->numEvents++] = (VstEvent *)pMidiEvent;
                    break;
                }
            }
            process_event_count++;
        }
        aEffect->dispatcher( aEffect, effProcessEvents, 0, 0, pVSTEvents, 0 );

        while ( dwDelta > 0 && !mIsCancelRequired ) {
            int dwFrames = dwDelta > sampleRate ? sampleRate : dwDelta;
            aEffect->processReplacing( aEffect, NULL, out_buffer, dwFrames );

            int iOffset = dwDelay - dwDeltaDelay;
            if ( iOffset > (int)dwFrames ) {
                iOffset = (int)dwFrames;
            }

            if ( iOffset == 0 ) {
                for ( int i = 0; i < (int)dwFrames; i++ ) {
                    buffer_l[i] = out_buffer[0][i];
                    buffer_r[i] = out_buffer[1][i];
                }
                total_processed2 += dwFrames;
                if ( waveIncoming( buffer_l, buffer_r, dwFrames ) ) {
                    mIsCancelRequired = true;
                }
            } else {
                dwDeltaDelay += iOffset;
            }
            dwDelta -= dwFrames;
            total_processed += dwFrames;
        }

        dwPrev = dwNow;
        dwNow = (int)current->clock;

        for( int i = 0; i < mman2.size(); i++ ){
            void *ptr = mman2[i];
            if( ptr ){
                free( ptr );
            }
        }
    }

    double msLast = msec_from_clock( dwNow );
    dwDelta = (int)(sampleRate * ((double)duration + (double)delay) / 1000.0 + dwDeltaDelay);
    if ( (int)(total_samples - total_processed2) > dwDelta ) {
        dwDelta = (int)(total_samples - total_processed2);
    }
    while ( dwDelta > 0 && !mIsCancelRequired ) {
        int dwFrames = dwDelta > sampleRate ? sampleRate : dwDelta;
        aEffect->processReplacing( aEffect, NULL, out_buffer, dwFrames );

        for ( int i = 0; i < (int)dwFrames; i++ ) {
            buffer_l[i] = out_buffer[0][i];
            buffer_r[i] = out_buffer[1][i];
        }
        total_processed2 += dwFrames;
        if ( waveIncoming( buffer_l, buffer_r, dwFrames ) ) {
            mIsCancelRequired = true;
        }

        dwDelta -= dwFrames;
        total_processed += dwFrames;
    }

    if ( mode_infinite ) {
        for ( int i = 0; i < sampleRate; i++ ) {
            buffer_l[i] = 0.0;
            buffer_r[i] = 0.0;
        }
        while ( !mIsCancelRequired ) {
            total_processed2 += sampleRate;
            if ( waveIncoming( buffer_l, buffer_r, sampleRate ) ) {
                mIsCancelRequired = true;
            }
        }
    }

    aEffect->dispatcher( aEffect, effMainsChanged, 0, 0, NULL, 0 );
    // all_eventsの中身はmEvents0, mEvents1なので，ここでfreeしなくていい
    all_events.clear();
#if DEBUG
    sout.println( "VocaloidDriver#startRendering; done; total_processed=" + total_processed + "; total_processed2=" + total_processed2 );
#endif

    for( int i = 0; i < mman.size(); i++ ){
        void *ptr = mman[i];
        if( ptr ) delete [] ptr;
    }
    mman.clear();

    mIsRendering = false;
    for ( int i = 0; i < mEvents0.size(); i++ ) {
        MidiEvent *ptr = mEvents0[i];
        if( ptr ) delete ptr;
    }
    mEvents0.clear();
    for ( int i = 0; i < mEvents1.size(); i++ ) {
        MidiEvent *ptr = mEvents1[i];
        if( ptr ) delete ptr;
    }
    mEvents1.clear();
    for( int i = 0; i < mTempoList.size(); i++ ){
        TempoInfo *ti = mTempoList[i];
        if( ti ) delete ti;
    }
    mTempoList.clear();
    mIsCancelRequired = false;

    return mProcessed;
}

bool vocaloidrv::sendEvent( unsigned char *midi_data, int *clock_data, int num_data, int targetTrack )
{
    // midi_dataがMIDIデータの本体，clock_dataがゲートタイムのリスト．
    // midi_dataのデータ個数は，num_dataの3倍になる
    int count;
    if ( targetTrack == 0 ) {
        for( int i = 0; i < mTempoList.size(); i++ ){
            TempoInfo *ti = mTempoList[i];
            if( ti ) delete ti;
        }
        mTempoList.clear();
        if ( num_data <= 0 ) {
            //g_numTempoList = 1;
            TempoInfo *ti = new TempoInfo();
            ti->Clock = 0;
            ti->Tempo = DEF_TEMPO;
            ti->TotalSec = 0.0;
            mTempoList.push_back( ti );
        } else {
            if ( clock_data[0] == 0 ) {
                //g_numTempoList = num_data;
            } else {
                //g_numTempoList = num_data + 1;
                TempoInfo *ti = new TempoInfo();
                ti->Clock = 0;
                ti->Tempo = DEF_TEMPO;
                ti->TotalSec = 0.0;
                mTempoList.push_back( ti );
            }
            int prev_tempo = DEF_TEMPO;
            int prev_clock = 0;
            double total = 0.0;
            count = -3;
            for ( int i = 0; i < num_data; i++ ) {
                count += 3;
                int tempo = (int)(midi_data[count + 2] | (midi_data[count + 1] << 8) | (midi_data[count] << 16));
                total += (clock_data[i] - prev_clock) * (double)prev_tempo / (1000.0 * TIME_FORMAT);
                TempoInfo *ti = new TempoInfo();
                ti->Clock = clock_data[i];
                ti->Tempo = tempo;
                ti->TotalSec = total;
                mTempoList.push_back( ti );
                prev_tempo = tempo;
                prev_clock = clock_data[i];
            }
        }
    }

    // 与えられたイベント情報をs_track_eventsに収納
    count = -3;
    int pPrev = 0;
    if( targetTrack == 0 ){
        for( int i = 0; i < mEvents0.size(); i++ ){
            MidiEvent *me = mEvents0[i];
            if( me ) delete me;
        }
        mEvents0.clear();
    }else{
        for( int i = 0; i < mEvents1.size(); i++ ){
            MidiEvent *me = mEvents1[i];
            if( me ) delete me;
        }
        mEvents1.clear();
    }
#if VOCALO_DRIVER_PRINT_EVENTS
    sout.println( "VocaloidDriver#SendEvent" );
    byte msb = 0x0;
    byte lsb = 0x0;
#endif
    for ( int i = 0; i < num_data; i++ ) {
        count += 3;
        MidiEvent *pEvent = new MidiEvent();
        //pEvent = &(new MIDI_EVENT());
        //pEvent->pNext = NULL;
        pEvent->clock = clock_data[i];
        //pEvent.dwOffset = 0;
        if ( targetTrack == 0 ) {
            pEvent->firstByte = 0xff;
            pEvent->data = new int[5];
            pEvent->data[0] = 0x51;
            pEvent->data[1] = 0x03;
            pEvent->data[2] = midi_data[count];
            pEvent->data[3] = midi_data[count + 1];
            pEvent->data[4] = midi_data[count + 2];
			pEvent->dataLength = 5;
        } else {
#if VOCALO_DRIVER_PRINT_EVENTS
        if ( src[count + 1] == 0x63 ) {
            msb = src[count + 2];
        } else if ( src[count + 1] == 0x62 ) {
            lsb = src[count + 2];
        } else {
            String str = (src[count + 1] == 0x06) ? ("0x" + PortUtil.toHexString( src[count + 2], 2 )) : "    ";
            str += (src[count + 1] == 0x26) ? (" 0x" + PortUtil.toHexString( src[count + 2], 2 )) : "";

            int nrpn = msb << 8 | lsb;
            sout.println( "VocaloidDriver#SendEvent; NRPN: 0x" + PortUtil.toHexString( nrpn, 4 ) + " " + str );
        }
#endif
            pEvent->firstByte = midi_data[count];
            pEvent->data = new int[3];
            pEvent->data[0] = midi_data[count + 1];
            pEvent->data[1] = midi_data[count + 2];
            pEvent->data[2] = 0x00;
			pEvent->dataLength = 3;
        }
        (targetTrack == 0 ? mEvents0 : mEvents1).push_back( pEvent );
    }

    return true;
}

bool vocaloidrv::open( int block_size, int sample_rate )
{
    bool ret = vstidrv::open( block_size, sample_rate );
#if DEBUG
    sout.println( "VocaloidDriver#open; dllHandle=0x" + PortUtil.toHexString( dllHandle.ToInt32() ).ToUpper() );
#endif
    /*for( int i = 0; i < mEventsAll.size(); i++ ){
        MidiEvent *me = mEventsAll[i];
        if( me ) delete me;
    }
    mEventsAll.clear();*/
    //g_midiPrepared0 = false;
    //g_midiPrepared1 = false;
    //g_tcCurrent = 0;
    //g_tcPrevious = 0;
    //g_saProcessed = 0;
    //g_saTotalSamples = 0;
    for( int i = 0; i < mTempoList.size(); i++ ){
        TempoInfo *ti = mTempoList[i];
        if( ti ) delete ti;
    }
    mTempoList.clear();
    //g_numTempoList = 0;
    mIsCancelRequired = false;
    for( int i = 0; i < mEvents0.size(); i++ ){
        MidiEvent *me = mEvents0[i];
        if( me ) delete me;
    }
    mEvents0.clear();
    for( int i = 0; i < mEvents1.size(); i++ ){
        MidiEvent *me = mEvents1[i];
        if( me ) delete me;
    }
    mEvents1.clear();
    return ret;
}

double vocaloidrv::msec_from_clock( int timeCode )
{
    double ret = 0.0;
    int index = -1;
    int c = mTempoList.size();
    for ( int i = 0; i < c; i++ ) {
        if ( timeCode <= mTempoList[i]->Clock ) {
            break;
        }
        index = i;
    }
    if ( index >= 0 ) {
        TempoInfo *item = mTempoList[index];
        ret = item->TotalSec + (timeCode - item->Clock) * (double)item->Tempo / (1000.0 * TIME_FORMAT);
    } else {
        ret = timeCode * (double)DEF_TEMPO / (1000.0 * TIME_FORMAT);
    }
    return ret;
}
