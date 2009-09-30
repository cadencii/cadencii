/*
 * vstidrv3.h
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
#ifndef __vstidrv3_h__
#define __vstidrv3_h__
#include "stdafx.h"
#include "winmmhelp.h"
//#include "waveplay.h"
#include "pluginterfaces/vst2.x/aeffectx.h"

#ifdef __cplusplus_cli
using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
using namespace System::Windows::Forms;
using namespace System::IO;
#endif

using namespace std;

struct TempoInfo{
public:
    int Clock;          // テンポが変更される時刻を表すクロック数
    int Tempo;          // テンポ
    double TotalSec;    // テンポが変更される時刻
};

typedef AEffect* (*PVSTMAIN)( audioMasterCallback audioMaster );
VstIntPtr AudioMaster( AEffect* effect, VstInt32 opcode, VstInt32 index, VstIntPtr value, void* ptr, float opt );
void CALLBACK waveOutProc( HWAVEOUT hwo, UINT uMsg, DWORD dwInstance, DWORD dwParam1, DWORD dwParam2 );
void first_buffer_written_callback();

#ifndef __cplusplus_cli
typedef void (*WaveIncomingCallback)( double *L, double *R, int length );
typedef void (*FirstBufferWrittenCallback)();
typedef void (*RenderingFinishedCallback)();
extern "C"{
    void vstidrv_setFirstBufferWrittenCallback( FirstBufferWrittenCallback proc );
    void vstidrv_setWaveIncomingCallback( WaveIncomingCallback proc );
    void vstidrv_setRenderingFinishedCallback( RenderingFinishedCallback proc );
    void vstidrv_InvokeFirstBufferWrittenEvent();
    bool vstidrv_Init( char *dll_path, int block_size, int sample_rate );
    int  vstidrv_SendEvent( unsigned char *src, int *deltaFrames, int numEvents, int targetTrack );
    int vstidrv_StartRendering(
        __int64 total_samples, 
        double amplify_left, 
        double amplify_right, 
        int error_samples, 
        bool event_enabled,
        bool direct_play_enabled,
        char **files,
        int num_files,
        double wave_read_offset_seconds,
        bool mode_infinite );
    void vstidrv_AbortRendering();
    double vstidrv_GetProgress();        
    float vstidrv_GetPlayTime();
    void vstidrv_WaveOutReset();
    void vstidrv_Terminate();
    int vstidrv_JoyInit();
    bool vstidrv_JoyIsJoyAttatched( int index );
    bool vstidrv_JoyGetStatus( int index, unsigned char *buttons, int *pov );
    int vstidrv_JoyGetNumButtons( int index );
    void vstidrv_JoyReset();
    int vstidrv_JoyGetNumJoyDev();
}
#endif

struct MIDI_EVENT{
public:
    unsigned int clock;
	MIDI_EVENT *pNext;
	unsigned int dwDataSize;
	unsigned char dwOffset;
    unsigned char* pMidiEvent;
};

namespace Boare{ namespace Cadencii{

#ifdef __cplusplus_cli
    public ref class vstidrv : public vstildr{
#else
    class vstidrv{
#endif
    private:
#ifdef __cplusplus_cli
        static vstidrv ^s_instance;
#else
        static vstidrv *s_instance;
#endif
        __PFX_MEMBER__ AEffect      *s_aeffect;                 // 読込んだdllから作成したVOCALOID2の本体。VOCALOID2への操作はs_aeffect->dispatcherで行う
        __PFX_MEMBER__ MIDI_EVENT  **s_track_events;            // 受信したmidiイベントのリスト。s_track_events[0]はvsqのマスタートラックのtempo情報のみを格納
                                                        // s_track_events[1]はレンダリングしたいトラックのmidiを格納。SendMidiで更新する
        __PFX_MEMBER__ HMODULE       g_dllHandle;               // 読込んだdllのハンドル
        __PFX_MEMBER__ MIDI_EVENT   *g_pEvents;                 // s_track_events[0]とs_track_events[1]を合成した物。SendMidiで、s_track_events[0]と
                                                        // s_track_events[1]の両方がそろった時点でmerge_eventsで合成する。
        __PFX_MEMBER__ MIDI_EVENT   *g_pCurrentEvent;           // g_pEventsの中の、現在読込んでいるMIDI_EVENTへのポインタ
        __PFX_MEMBER__ bool          g_midiPrepared0;   // s_track_events[0]のmidiイベントを受信済みかどうかを表すフラグ
        __PFX_MEMBER__ bool          g_midiPrepared1;   // s_track_events[1]のmidiイベントを受信済みかどうかを表すフラグ
        __PFX_MEMBER__ int           g_tcCurrent;
        __PFX_MEMBER__ int           g_tcPrevious;
        __PFX_MEMBER__ int           g_saProcessed;
        __PFX_MEMBER__ int           g_saTotalSamples;
        __PFX_MEMBER__ TempoInfo    *g_tempoList;
        __PFX_MEMBER__ int           g_numTempoList;
        __PFX_MEMBER__ bool          g_cancelRequired;
        __PFX_MEMBER__ double        g_progress;

#ifdef __cplusplus_cli
        String       ^s_plugin_version;
#else
        static string        s_plugin_version;
        static WaveIncomingCallback s_wave_incoming_callback;
        static RenderingFinishedCallback s_rendering_finished_callback;
#endif

        __PFX_MEMBER__ int free_events( MIDI_EVENT* pEvent );
        __PFX_MEMBER__ MIDI_EVENT* merge_events( MIDI_EVENT* x0,MIDI_EVENT* y0 );
        __PFX_MEMBER__ MIDI_EVENT* copy_event( MIDI_EVENT* x );
        __PFX_MEMBER__ MIDI_EVENT* clone_event( MIDI_EVENT* pEvent );
        /// <summary>
        /// 指定したタイムコードにおける，曲頭から測った時間を調べる
        /// </summary>
        __PFX_MEMBER__ double totalMilliSec_from_timeCode( int timeCode );

#ifdef __cplusplus_cli
        String ^GetVersion();
#else
        static string GetVersion();
#endif
        __PFX_MEMBER__ void exit_start_rendering();

#ifdef __cplusplus_cli
        Boare::Lib::Media::FirstBufferWrittenCallback ^s_first_buffer_written_callback;
#else
        static FirstBufferWrittenCallback s_first_buffer_written_callback;
#endif

    public:
        vstidrv();
#ifdef __cplusplus_cli
        static vstidrv ^GetInstance();
#endif

#ifdef __cplusplus_cli
        void Main( array<String ^> ^arg );
#endif

#ifdef __cplusplus_cli
        virtual event WaveIncomingEventHandler ^WaveIncoming;
        virtual event RenderingFinishedEventHandler ^RenderingFinished;
        //virtual void SetFirstBufferWrittenCallback( Boare::Lib::Media::FirstBufferWrittenCallback ^handler );
#else
        static void SetFirstBufferWrittenCallback( FirstBufferWrittenCallback proc );
        static void SetWaveIncomingCallback( WaveIncomingCallback proc );
        static void SetRenderingFinishedCallback( RenderingFinishedCallback proc );
#endif

        static void InvokeFirstBufferWrittenEvent();
#ifdef __cplusplus_cli
        virtual bool Init( array<System::Char> ^dll_path, int block_size, int sample_rate );
#else
        static bool Init( char *dll_path, int block_size, int sample_rate );
#endif

        __PFX_INTERFACE__ int SendEvent( array<System::Byte> ^src, array<int> ^deltaFrames, int targetTrack );

#ifdef __cplusplus_cli
        virtual int StartRendering(
            __int64 total_samples, 
            double amplify_left, 
            double amplify_right );
#else
        static int StartRendering(
            __int64 total_samples, 
            double amplify_left, 
            double amplify_right, 
            int error_samples, 
            bool event_enabled,
            bool direct_play_enabled,
			char **files,
			int num_files,
            double wave_read_offset_seconds,
            bool mode_infinite );
#endif
        __PFX_INTERFACE__ void AbortRendering();
        __PFX_INTERFACE__ double GetProgress();
        __PFX_INTERFACE__ void Terminate();
    };

} }

#endif // __vstidrv3_h__
