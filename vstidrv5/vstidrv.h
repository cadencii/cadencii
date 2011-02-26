/*
 * vstidrv.h
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
#pragma once

#if defined( _DEBUG ) && defined( WIN32 )
#define _CRTDBG_MAP_ALLOC
#include <stdlib.h>
#include <crtdbg.h>
#endif

#include <windows.h>
#include <stdio.h>
#include <tchar.h>
#include <string>
#include <vector>
#include <iostream>
#include "pluginterfaces/vst2.x/aeffectx.h"

using namespace std;

typedef AEffect* (*PVSTMAIN)( audioMasterCallback audioMaster );

VstIntPtr AudioMaster( AEffect* effect, VstInt32 opcode, VstInt32 index, VstIntPtr value, void* ptr, float opt );

struct TempoInfo
{
    /// <summary>
    /// テンポが変更される時刻を表すクロック数
    /// </summary>
    int Clock;
    /// <summary>
    /// テンポ
    /// </summary>
    int Tempo;
    /// <summary>
    /// テンポが変更される時刻
    /// </summary>
    double TotalSec;
};

class MidiEvent
{
public:
	long clock;
    int firstByte;
	int dataLength;
    int *data;

    int compareTo( const MidiEvent *item )
    {
        return compare( this, item );
    }

    static int compare( const MidiEvent *item0, const MidiEvent *item1 )
    {
        if ( item0->clock != item1->clock ) {
            return (int)(item0->clock - item1->clock);
        } else {
            int first_this = item0->firstByte & 0xf0;
            int first_item = item1->firstByte & 0xf0;

            if ( (first_this == 0x80 || first_this == 0x90) && (first_item == 0x80 || first_item == 0x90) ) {
                if ( item0->data != NULL && item0->dataLength >= 2 && item1->data != NULL && item1->dataLength >= 2 ) {
                    if ( first_item == 0x90 && item1->data[1] == 0 ) {
                        first_item = 0x80;
                    }
                    if ( first_this == 0x90 && item0->data[1] == 0 ) {
                        first_this = 0x80;
                    }
                    if ( item0->data[0] == item1->data[0] ) {
                        if ( first_this == 0x90 ) {
                            if ( first_item == 0x80 ) {
                                // ON -> OFF
                                return 1;
                            } else {
                                // ON -> ON
                                return 0;
                            }
                        } else {
                            if ( first_item == 0x80 ) {
                                // OFF -> OFF
                                return 0;
                            } else {
                                // OFF -> ON
                                return -1;
                            }
                        }
                    }
                }
            }
            return (int)(item0->clock - item1->clock);
        }
    }

    ~MidiEvent(){
        dataLength = 0;
        if( data ) delete[] data;
    }
};

struct midieventpred
{
    bool operator()( const MidiEvent *left, const MidiEvent *right ) const {
        // rightが順序大ならtrue
        return MidiEvent::compare( right, left ) > 0;
    }
};

class vstidrv
{

protected:
	bool loaded;// = false;
    string path;// = "";
	PVSTMAIN mainDelegate;
    /// <summary>
    /// 読込んだdllから作成したVOCALOID2の本体。VOCALOID2への操作はs_aeffect->dispatcherで行う
    /// </summary>
    AEffect *aEffect;
    //void *aEffectPointer;
    /// <summary>
    /// 読込んだdllのハンドル
    /// </summary>
    HMODULE dllHandle;
    /// <summary>
    /// 波形バッファのサイズ。
    /// </summary>
    int blockSize;
    /// <summary>
    /// サンプリングレート。VOCALOID2 VSTiは限られたサンプリングレートしか受け付けない。たいてい44100Hzにする
    /// </summary>
    int sampleRate;
    /// <summary>
    /// バッファ(bufferLeft, bufferRight)の長さ
    /// </summary>
    static const int BUFLEN = 44100;

private:
//	void *mainProcPointer;
    /// <summary>
    /// 左チャンネル用バッファ
    /// </summary>
    float *bufferLeft;// = IntPtr.Zero;
    /// <summary>
    /// 右チャンネル用バッファ
    /// </summary>
    float *bufferRight;// = IntPtr.Zero;
    /// <summary>
    /// 左右チャンネルバッファの配列(buffers={bufferLeft, bufferRight})
    /// </summary>
    float **buffers;// = IntPtr.Zero;
    /// <summary>
    /// パラメータの，ロード時のデフォルト値
    /// </summary>
    vector<float> paramDefaults;// = null;

public:
	vstidrv( string file_path )
	{
		path = file_path;
		loaded = false;
		mainDelegate = NULL;
		aEffect = NULL;
		dllHandle = NULL;
		bufferLeft = NULL;
		bufferRight = NULL;
		buffers = NULL;
	}

	bool isLoaded()
	{
		return loaded;
	}

    int getSampleRate()
    {
        return sampleRate;
    }

    void resetAllParameters()
    {
		if ( paramDefaults.size() <= 0 ) {
            return;
        }
        for ( int i = 0; i < paramDefaults.size(); i++ ) {
            setParameter( i, paramDefaults[i] );
        }
    }

    virtual float getParameter( int index )
    {
        float ret = 0.0f;
        try {
			ret = aEffect->getParameter( aEffect, index );
        } catch ( char *ex ) {
            cerr << "vstidrv#getParameter; ex=" << ex << endl;
        }
        return ret;
    }

    virtual void setParameter( int index, float value )
    {
        try {
			aEffect->setParameter( aEffect, index, value );
        } catch ( char *ex ) {
            cerr << "vstidrv#setParameter; ex=" << ex << endl;
        }
    }

    string getParameterDisplay( int index )
    {
        return getStringCore( effGetParamDisplay, index, kVstMaxParamStrLen );
    }

    string getParameterLabel( int index )
    {
        return getStringCore( effGetParamLabel, index, kVstMaxParamStrLen );
    }

    string getParameterName( int index )
    {
        return getStringCore( effGetParamName, index, kVstMaxParamStrLen );
    }

    void process( double *left, double *right, int length )
    {
        if ( left == NULL || right == NULL ) {
            return;
        }
        try {
            initBuffer();
            int remain = length;
            int offset = 0;
            float* left_ch = (float *)bufferLeft;
            float* right_ch = (float *)bufferRight;
            float** out_buffer = (float**)buffers;
            out_buffer[0] = left_ch;
            out_buffer[1] = right_ch;
            while ( remain > 0 ) {
                int proc = (remain > BUFLEN) ? BUFLEN : remain;
				aEffect->processReplacing( aEffect, NULL, out_buffer, proc );
                for ( int i = 0; i < proc; i++ ) {
                    left[i + offset] = left_ch[i];
                    right[i + offset] = right_ch[i];
                }
                remain -= proc;
                offset += proc;
            }
        } catch ( char *ex ) {
            cerr << "vstidrv#process; ex=" <<  ex << endl;
        }
    }

    virtual void send( MidiEvent *events, int nEvents )
    {
		vector<void *> mem;
        VstEvents* pVSTEvents = (VstEvents *)malloc( sizeof( VstEvent ) + nEvents * sizeof( VstEvent* ) );
		mem.push_back( pVSTEvents );
        pVSTEvents->numEvents = 0;
        pVSTEvents->reserved = (VstIntPtr)0;

        for ( int i = 0; i < nEvents; i++ ) {
            MidiEvent pProcessEvent = events[i];
            //byte event_code = (byte)pProcessEvent.firstByte;
            VstEvent* pVSTEvent = (VstEvent*)0;
            VstMidiEvent* pMidiEvent;
			pMidiEvent = (VstMidiEvent* )malloc( (int)(sizeof( VstMidiEvent ) + (pProcessEvent.dataLength + 1) * sizeof( byte )) );
			mem.push_back( pMidiEvent );
            pMidiEvent->byteSize = sizeof( VstMidiEvent );
            pMidiEvent->deltaFrames = 0;
            pMidiEvent->detune = 0;
            pMidiEvent->flags = 1;
            pMidiEvent->noteLength = 0;
            pMidiEvent->noteOffset = 0;
            pMidiEvent->noteOffVelocity = 0;
            pMidiEvent->reserved1 = 0;
            pMidiEvent->reserved2 = 0;
            pMidiEvent->type = kVstMidiType;
            pMidiEvent->midiData[0] = (byte)(0xff & pProcessEvent.firstByte);
            for ( int j = 0; j < pProcessEvent.dataLength; j++ ) {
                pMidiEvent->midiData[j + 1] = (byte)(0xff & pProcessEvent.data[j]);
            }
            pVSTEvents->events[pVSTEvents->numEvents++] = (VstEvent *)pMidiEvent;
        }
		aEffect->dispatcher( aEffect, effProcessEvents, 0, 0, pVSTEvents, 0 );
		for( int i = 0; i < mem.size(); i++ ){
			void *ptr = mem[i];
			if( ptr != NULL ){
				free( ptr );
			}
		}
    }

    virtual void setSampleRate( int sample_rate )
    {
        sampleRate = sample_rate;
		int ret1 = aEffect->dispatcher( aEffect, effSetSampleRate, 0, 0, NULL, (float)sampleRate );
		int ret2 = aEffect->dispatcher( aEffect, effSetBlockSize, 0, sampleRate, NULL, 0 );
#if DEBUG
        sout.println( "vstidrv#setSampleRate; ret1=" + ret1 + "; ret2=" + ret2 );
#endif
    }

    virtual bool open( int block_size, int sample_rate )
    {
		dllHandle = LoadLibraryExA( path.c_str(), NULL, LOAD_WITH_ALTERED_SEARCH_PATH );
        if ( dllHandle == NULL ) {
            cerr << "vstidrv#open; dllHandle is null" << endl;
            return false;
        }

        mainDelegate = (PVSTMAIN)GetProcAddress( dllHandle, "main" );
        if ( mainDelegate == NULL ) {
            cerr << "vstidrv#open; mainDelegate is null" << endl;
            return false;
        }

        aEffect = mainDelegate( AudioMaster );
        if ( aEffect == NULL ) {
            cerr << "vstidrv#open; aEffectPointer is null" << endl;
            return false;
        }
        blockSize = block_size;
        sampleRate = sample_rate;
		aEffect->dispatcher( aEffect, effOpen, 0, 0, NULL, 0 );
		int ret = aEffect->dispatcher( aEffect, effSetSampleRate, 0, 0, NULL, (float)sampleRate );
#if DEBUG
        sout.println( "vstidrv#open; dll_path=" + path + "; ret for effSetSampleRate=" + ret );
#endif

		aEffect->dispatcher( aEffect, effSetBlockSize, 0, blockSize, NULL, 0 );

        // デフォルトのパラメータ値を取得
		int num = aEffect->numParams;
		paramDefaults.clear();// = new float[num];
        for ( int i = 0; i < num; i++ ) {
			paramDefaults.push_back( aEffect->getParameter( aEffect, i ) );
        }

        return true;
    }

    ~vstidrv()
    {
        close();
    }

    virtual void close()
    {
        if ( aEffect != NULL ) {
			aEffect->dispatcher( aEffect, effClose, 0, 0, NULL, 0.0f );
        }
        if ( dllHandle != NULL ) {
			FreeLibrary( dllHandle );
        }
        aEffect = NULL;
        dllHandle = NULL;
        mainDelegate = NULL;
        releaseBuffer();
    }

private:
    string getStringCore( int opcode, int index, int str_capacity )
    {
        char *arr = new char[str_capacity + 1];
        for ( int i = 0; i < str_capacity; i++ ) {
            arr[i] = 0;
        }
        aEffect->dispatcher( aEffect, opcode, index, 0, arr, 0.0f );
        string ret = "";
		ret += arr;
        return ret;
    }

    void initBuffer()
    {
        if ( bufferLeft == NULL ) {
            bufferLeft = (float *)malloc( sizeof( float ) * BUFLEN );
        }
        if ( bufferRight == NULL ) {
            bufferRight = (float *)malloc( sizeof( float ) * BUFLEN );
        }
        if ( buffers == NULL ) {
            buffers = (float **)malloc( sizeof( float* ) * 2 );
			buffers[0] = bufferLeft;
			buffers[1] = bufferRight;
        }
    }

    void releaseBuffer()
    {
        if ( bufferLeft != NULL ) {
            free( bufferLeft );
            bufferLeft = NULL;
        }
        if ( bufferRight != NULL ) {
            free( bufferRight );
            bufferRight = NULL;
        }
        if ( buffers != NULL ) {
            free( buffers );
            buffers = NULL;
        }
    }

};
