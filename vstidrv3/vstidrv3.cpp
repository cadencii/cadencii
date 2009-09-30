/*
 * vstidrv3.cpp
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
#include "vstidrv3.h"

const int SMF_FORMAT  = 1;                // vsqをStandard Midi Fileとみなした時のバージョン。現状、1に固定。
const int TIME_FORMAT = 480;              // 
const int NUM_TRACKS  = 2;                // s_track_eventsの配列サイズ。このdllでは2に固定。
const int DEF_TEMPO   = 500000;           // デフォルトのテンポ．
static int          g_block_size;              // 波形バッファのサイズ。
static int          g_sample_rate;             // サンプリングレート。VOCALOID2 VSTiは限られたサンプリングレートしか受け付けない。たいてい44100Hzにする

#ifndef __cplusplus_cli
extern "C"{
    void vstidrv_setFirstBufferWrittenCallback( FirstBufferWrittenCallback proc ){
        Boare::Cadencii::vstidrv::SetFirstBufferWrittenCallback( proc );
    }

    void vstidrv_setWaveIncomingCallback( WaveIncomingCallback proc ){
        Boare::Cadencii::vstidrv::SetWaveIncomingCallback( proc );
    }

    void vstidrv_setRenderingFinishedCallback( RenderingFinishedCallback proc ){
        Boare::Cadencii::vstidrv::SetRenderingFinishedCallback( proc );
    }

    void vstidrv_InvokeFirstBufferWrittenEvent(){
        Boare::Cadencii::vstidrv::InvokeFirstBufferWrittenEvent();
    }

    bool vstidrv_Init( char *dll_path, int block_size, int sample_rate ){
        return Boare::Cadencii::vstidrv::Init( dll_path, block_size, sample_rate );
    }

    int vstidrv_SendEvent( unsigned char *src, int *deltaFrames, int numEvents, int targetTrack ){
        return Boare::Cadencii::vstidrv::SendEvent( src, deltaFrames, numEvents, targetTrack );
    }

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
        bool mode_infinite
    ){
        return Boare::Cadencii::vstidrv::StartRendering( total_samples, 
                                                         amplify_left,
                                                         amplify_right,
                                                         error_samples,
                                                         event_enabled,
                                                         direct_play_enabled,
												         files,
												         num_files,
												         wave_read_offset_seconds,
                                                         mode_infinite );
    }

    void vstidrv_AbortRendering(){
        Boare::Cadencii::vstidrv::AbortRendering();
    }

    double vstidrv_GetProgress(){
        return Boare::Cadencii::vstidrv::GetProgress();
    }

    float vstidrv_GetPlayTime(){
        return Boare::Cadencii::vstidrv::GetPlayTime();
    }

    void vstidrv_WaveOutReset(){
        Boare::Cadencii::vstidrv::WaveOutReset();
    }

    void vstidrv_Terminate(){
        Boare::Cadencii::vstidrv::Terminate();
    }

    int vstidrv_JoyInit(){
        return Boare::Cadencii::vstidrv::JoyInit();
    }

    bool vstidrv_JoyIsJoyAttatched( int index ){
        return Boare::Cadencii::vstidrv::JoyIsJoyAttatched( index );
    }

    bool vstidrv_JoyGetStatus( int index, unsigned char *buttons, int *pov ){
        return Boare::Cadencii::vstidrv::JoyGetStatus( index, buttons, pov );
    }

    int vstidrv_JoyGetNumButtons( int index ){
        return Boare::Cadencii::vstidrv::JoyGetNumButtons( index );
    }

    void vstidrv_JoyReset(){
        Boare::Cadencii::vstidrv::JoyReset();
    }

    int vstidrv_JoyGetNumJoyDev(){
        return Boare::Cadencii::vstidrv::JoyGetNumJoyDev();
    }

    int main(){ // this is dumy
        return 110;
    }
}
#endif

namespace Boare{ namespace Cadencii{

#ifdef __cplusplus_cli
#else
    AEffect      *vstidrv::s_aeffect;                 // 読込んだdllから作成したVOCALOID2の本体。VOCALOID2への操作はs_aeffect->dispatcherで行う
    MIDI_EVENT  **vstidrv::s_track_events;            // 受信したmidiイベントのリスト。s_track_events[0]はvsqのマスタートラックのtempo情報のみを格納
    HMODULE       vstidrv::g_dllHandle;               // 読込んだdllのハンドル
    MIDI_EVENT   *vstidrv::g_pEvents;                 // s_track_events[0]とs_track_events[1]を合成した物。SendMidiで、s_track_events[0]と
    MIDI_EVENT   *vstidrv::g_pCurrentEvent;           // g_pEventsの中の、現在読込んでいるMIDI_EVENTへのポインタ
    bool          vstidrv::g_midiPrepared0;   // s_track_events[0]のmidiイベントを受信済みかどうかを表すフラグ
    bool          vstidrv::g_midiPrepared1;   // s_track_events[1]のmidiイベントを受信済みかどうかを表すフラグ
    int           vstidrv::g_tcCurrent;
    int           vstidrv::g_tcPrevious;
    int           vstidrv::g_saProcessed;
    int           vstidrv::g_saTotalSamples;
    TempoInfo    *vstidrv::g_tempoList;
    int           vstidrv::g_numTempoList;
    bool          vstidrv::g_cancelRequired;
    double        vstidrv::g_progress;
    string        vstidrv::s_plugin_version;
    FirstBufferWrittenCallback vstidrv::s_first_buffer_written_callback;
    WaveIncomingCallback       vstidrv::s_wave_incoming_callback;
    RenderingFinishedCallback  vstidrv::s_rendering_finished_callback;
    vstidrv *vstidrv::s_instance;

    void vstidrv::SetFirstBufferWrittenCallback( FirstBufferWrittenCallback proc ){
#ifdef TEST
#ifndef __cplusplus_cli
        vstidrv::PushLog( "vstidrv::setFirstBufferWrittenCallback" );
        debug::logger << "    proc=0x" << hex << (int)proc << dec << endl;
#endif
#endif
        waveplay::set_first_buffer_written_callback( proc );
    };

    void vstidrv::SetWaveIncomingCallback( WaveIncomingCallback proc ){
        s_wave_incoming_callback = proc;
    };

    void vstidrv::SetRenderingFinishedCallback( RenderingFinishedCallback proc ){
        s_rendering_finished_callback = proc;
    };
#endif

    void vstidrv::InvokeFirstBufferWrittenEvent(){
#ifdef TEST
        debug::push_log( "vstidrv::InvokeFirstBufferWrittenEvent()" );
        debug::push_log( "    (s_instance == nullptr)=" + (s_instance == nullptr) );
        if( s_instance != nullptr ){
            debug::push_log( "    (s_instance->s_first_buffer_written_callback == nullptr)=" + (s_instance->s_first_buffer_written_callback == nullptr) );
        }
#endif
        s_instance->s_first_buffer_written_callback();
    }

#ifdef __cplusplus_cli
    void vstidrv::Main( array<String ^> ^args ){

    };
#endif

    void vstidrv::Terminate(){
        if( s_aeffect ){
            s_aeffect->dispatcher( s_aeffect, effClose, 0, 0, 0, 0 );
        }
        if( s_track_events ){
            delete [] s_track_events;
        }
//        waveplay::terminate();
        //TODO: g_hwave_outの終了処理
#ifdef TEST
#ifdef __cplusplus_cli
        debug::push_log( "~vstidrv()" );
#endif
#endif
    };

    /*bool vstidrv::getIsDirectMode(){
        return s_direct_play;
    };

    void vstidrv::setIsDirectMode( bool value ){
        s_direct_play = value;
    };*/

    int vstidrv::free_events( MIDI_EVENT* pEvent ){
        MIDI_EVENT* pEventTmp;
        while( pEvent ){
            pEventTmp = pEvent->pNext;
            delete pEvent->pMidiEvent;
            delete pEvent;
            pEvent = pEventTmp;
        }
        return TRUE;
    };

    MIDI_EVENT* vstidrv::merge_events( MIDI_EVENT* x0,MIDI_EVENT* y0 ){
        MIDI_EVENT *x = copy_event( x0 );
        MIDI_EVENT *y = copy_event( y0 );
        MIDI_EVENT z;
        MIDI_EVENT *p = &z;

        while( x && y ){
            if( x->clock <= y->clock ){
                p->pNext = x;
                p = x;
                x = x->pNext;
            }else{
                p->pNext = y;
                p = y;
                y = y->pNext;
            }
        }
        
        if( x ){
            p->pNext = x;
        }else{
            p->pNext = y;
        }
        
        return z.pNext;
    };

    MIDI_EVENT* vstidrv::copy_event( MIDI_EVENT* x ){
        MIDI_EVENT z;
        MIDI_EVENT *p = &z;

        while( x ){
            p->pNext = clone_event( x );
            p = p->pNext;
            x = x->pNext;
        }
        
        return z.pNext;
    };

    MIDI_EVENT* vstidrv::clone_event( MIDI_EVENT* pEvent ){
        MIDI_EVENT* pNewEvent = new MIDI_EVENT;
        pNewEvent->clock = pEvent->clock;
        pNewEvent->dwDataSize = pEvent->dwDataSize;
        pNewEvent->dwOffset    = pEvent->dwOffset;
        pNewEvent->pMidiEvent = new unsigned char[pEvent->dwDataSize + pEvent->dwOffset];
        memcpy( pNewEvent->pMidiEvent, pEvent->pMidiEvent, pEvent->dwDataSize + pEvent->dwOffset );
        pNewEvent->pNext = NULL;
        return pNewEvent;
    };

    /// <summary>
    /// 指定したタイムコードにおける，曲頭から測った時間を調べる
    /// </summary>
    double vstidrv::totalMilliSec_from_timeCode( int timeCode ){
        double ret = 0.0;
        int index = -1;
        for( int i = 0; i < g_numTempoList; i++ ){
            if( timeCode < g_tempoList[i].Clock ){
                break;
            }
            index = i;
        }
        if( index >= 0 ){
            ret = g_tempoList[index].TotalSec + (timeCode - g_tempoList[index].Clock) * (double)g_tempoList[index].Tempo / (1000.0 * TIME_FORMAT);
        }else{
            ret = timeCode * (double)DEF_TEMPO / (1000.0 * TIME_FORMAT);
        }
        return ret;
    };

#ifdef __cplusplus_cli
    String ^vstidrv::GetVersion(){
#else
    string vstidrv::GetVersion(){
#endif
        if ( s_aeffect ) {
            // effGetVendorString => YAMAHA
            // effGetProductString => VOCALOID Vocal Synth
            // effGetEffectName
            char* str = new char[kVstMaxEffectNameLen];
            s_aeffect->dispatcher( s_aeffect, effGetEffectName, 0, 0, str, 0 );
#ifdef __cplusplus_cli
            array<Char> ^arr = gcnew array<Char>( kVstMaxEffectNameLen );
            int len = 0;
            for ( int i = 0; i < kVstMaxEffectNameLen; i++ ) {
                if ( str[i] == '\0' ) {
                    len = i;
                    break;
                }
                arr[i] = str[i];
            }
            delete [] str;
            return gcnew String( arr, 0, len );
#else
            string ret( str, kVstMaxEffectNameLen );
            delete [] str;
            return ret;
#endif
        } else {
            return "";
        }
    };

    void vstidrv::exit_start_rendering(){
        s_aeffect->dispatcher( s_aeffect, effMainsChanged, 0, 0, 0, 0 );
    };

    /*void vstidrv::FireFirstBufferWrittenEventHandler(){
#ifdef __cplusplus_cli
        FirstBufferWritten::raise();
#else

#endif
    };*/

#ifdef __cplusplus_cli
    bool vstidrv::Init( array<System::Char> ^dll_path, int block_size, int sample_rate ){
#else
    bool vstidrv::Init( char *str, int block_size, int sample_rate ){
#endif
#ifdef TEST
            std::cout << "vstidrv+Init" << std::endl;
            debug::push_log( "vstidrv+Init" );
#endif
        g_pEvents = NULL;
        g_midiPrepared0 = false;
        g_midiPrepared1 = false;
        g_tcCurrent = 0;
        g_tcPrevious = 0;
        g_saProcessed = 0;
        g_saTotalSamples = 0;
        g_tempoList = NULL;
        g_numTempoList = 0;
        g_cancelRequired = false;
        g_progress = 0.0;
        s_plugin_version = "";
        //s_direct_play = true;
        try{
#ifdef TEST
#ifdef __cplusplus_cli
            debug::push_log( "    sizeof(int)=" + sizeof( int ) );
#else
            debug::logger << "    sizeof(int)=" << sizeof( int ) << endl;
#endif
#endif

            g_block_size = block_size;
            g_sample_rate = sample_rate;
//            waveplay::init( g_block_size, g_sample_rate );
//            waveplay::set_first_buffer_written_callback( first_buffer_written_callback );

#ifdef __cplusplus_cli
            WCHAR *str = new WCHAR[dll_path->Length + 1];
            for( int i = 0; i < dll_path->Length; i++ ){
                str[i] = dll_path[i];
            }
            str[dll_path->Length] = '\0';
            HMODULE dll_handle = LoadLibraryExW( str, NULL, LOAD_WITH_ALTERED_SEARCH_PATH );
            Sleep( 250 );
            delete [] str;
#else
#ifdef TEST
            debug::logger << "    str=" << str << std::endl;
            debug::logger << "    calling LoadLibraryA..." << std::endl;
#endif
            HMODULE dll_handle = LoadLibraryExA( str, NULL, LOAD_WITH_ALTERED_SEARCH_PATH );
#ifdef TEST
            debug::logger << "    ...done" << std::endl;
#endif
            Sleep( 250 );
#endif
#ifdef TEST
            int hm = (int)dll_handle;
            std::cout << "    dll_handle=0x" << std::hex << hm << std::endl;
#ifdef __cplusplus_cli
            debug::push_log( "    dll_handle=0x" + Convert::ToString( hm, 16 ) );
#else
            debug::logger << "    dll_handle=0x" << hex << hm << dec << endl;
#endif
#endif
            g_dllHandle = dll_handle;

            PVSTMAIN main = (PVSTMAIN)GetProcAddress( g_dllHandle, "main" );
            Sleep( 250 );
#ifdef TEST
#ifdef __cplusplus_cli
            Console::WriteLine( "    (!main)=" + (!main) );
            debug::push_log( "    (!main)=" + (!main) );
            int addr = (int)main;
            debug::push_log( "    (int)main=0x" + Convert::ToString( addr, 16 ) );
#else
            int addr = (int)main;
            debug::logger << "    (int)main=0x" << std::hex << addr << std::endl;
#endif
#endif
            if( !main ){
                return false;
            }

            try{
                s_aeffect = main( AudioMaster );
#ifdef __cplusplus_cli
            }catch( Exception ^ex32 ){
#else
            }catch( string ex32 ){
#endif
#ifdef TEST
#ifdef __cplusplus_cli
                debug::push_log( "    ex32=" + ex32->ToString() );
#else
                debug::logger << "    ex32=" << ex32 << std::endl;
#endif
#endif
            }
#ifdef TEST
            addr = (int)s_aeffect;
#ifdef __cplusplus_cli
            debug::push_log( "    (int)s_aeffect=0x" + Convert::ToString( addr, 16 ) );
#else
            debug::logger << "    (int)s_aeffect=0x" << std::hex << addr << std::endl;       
#endif
#endif
            if( !s_aeffect ){
                return false;
            }
#ifdef TEST
            s_plugin_version = GetVersion();
            debug::push_log( "    s_plugin_vesion=" + s_plugin_version );
#endif
            s_aeffect->dispatcher( s_aeffect, effOpen, 0, 0, 0, 0 );
            s_aeffect->dispatcher( s_aeffect, effSetSampleRate, 0, 0, 0, (float)g_sample_rate );
            s_aeffect->dispatcher( s_aeffect, effSetBlockSize, 0, g_block_size, 0, 0 );
#ifdef TEST
			addr = (int)s_aeffect->processDoubleReplacing;
			debug::push_log( "    s_aeffect->processDoubleReplacing=0x" + Convert::ToString( addr, 16 ) );
#endif

            s_track_events = new MIDI_EVENT*[NUM_TRACKS];
#ifdef __cplusplus_cli
        }catch( Exception ^ex ){
            Console::WriteLine( "    ex=" + ex->ToString() );
#else
        }catch( string ex ){
            std::cout << "    ex=" << ex << std::endl;
#endif
#ifdef TEST
#ifdef __cplusplus_cli
            debug::push_log( "    ex=" + ex->ToString() );
#else
            debug::logger << "    ex=" << ex << std::endl;
#endif
#endif
            return false;
        }
        return true;
    };

    int vstidrv::SendEvent( array<System::Byte> ^src, array<int> ^deltaFrames, int targetTrack ){
        int numEvents = deltaFrames->Length;
        int count;
        if ( targetTrack == 0 ) {
            if ( g_tempoList ) {
                delete [] g_tempoList;
            }
            if( numEvents <= 0 ){
                g_numTempoList = 1;
                g_tempoList = new TempoInfo[1];
                g_tempoList[0].Clock = 0;
                g_tempoList[0].Tempo = DEF_TEMPO;
                g_tempoList[0].TotalSec = 0.0;
            } else {
                int index_offset;
                if ( deltaFrames[0] == 0 ) {
                    g_numTempoList = numEvents;
                    g_tempoList = new TempoInfo[numEvents];
                    index_offset = 0;
                } else {
                    g_numTempoList = numEvents + 1;
                    g_tempoList = new TempoInfo[numEvents + 1];
                    index_offset = 1;
                    g_tempoList[0].Clock = 0;
                    g_tempoList[0].Tempo = DEF_TEMPO;
                    g_tempoList[0].TotalSec = 0.0;
                }
                int tempo_prev = DEF_TEMPO;
                int tempo_clock = 0;
                double total = 0.0;
                count = -3;
                for ( int i = 0; i < numEvents; i++ ) {
                    count += 3;
                    int tempo = (int)(src[count + 2] | (src[count + 1] << 8) | (src[count] << 16));
                    g_tempoList[i + index_offset].Clock = deltaFrames[i];
                    g_tempoList[i + index_offset].Tempo = tempo;
                    total += (deltaFrames[i] - tempo_clock) * (double)tempo_prev / (1000.0 * TIME_FORMAT);
                    g_tempoList[i + index_offset].TotalSec = total;
                    tempo_prev = tempo;
                    tempo_clock = deltaFrames[i];
                }
            }
        }

        // 与えられたイベント情報をs_track_eventsに収納
        count = -3;
        MIDI_EVENT *pPrev = NULL;
        for ( int i = 0; i < numEvents; i++ ) {
            count += 3;
            MIDI_EVENT *pEvent;
            pEvent = NULL;
            pEvent = new MIDI_EVENT;
            pEvent->pNext = NULL;
            pEvent->clock = deltaFrames[i];
            pEvent->dwOffset = 0;
            if ( targetTrack == 0 ) {
                pEvent->dwDataSize = 6;
                pEvent->pMidiEvent = new unsigned char[6];
                pEvent->pMidiEvent[0] = 0xff;
                pEvent->pMidiEvent[1] = 0x51;
                pEvent->pMidiEvent[2] = 0x03;
                pEvent->pMidiEvent[3] = src[count];
                pEvent->pMidiEvent[4] = src[count + 1];
                pEvent->pMidiEvent[5] = src[count + 2];
            } else {
                pEvent->dwDataSize = 4;
                pEvent->pMidiEvent = new unsigned char[4];
                pEvent->pMidiEvent[0] = src[count];
                pEvent->pMidiEvent[1] = src[count + 1];
                pEvent->pMidiEvent[2] = src[count + 2];
                pEvent->pMidiEvent[3] = 0x00;
            }
            if ( pPrev ) {
                pPrev->pNext = pEvent;
            } else {
                s_track_events[targetTrack] = pEvent;
            }
            pPrev = pEvent;
        }

        return TRUE;
    };

#ifdef __cplusplus_cli
	int vstidrv::StartRendering(
        __int64 total_samples,
        double amplify_left,
        double amplify_right
#else
	int vstidrv::StartRendering(
        __int64 total_samples,
        double amplify_left,
        double amplify_right,
        int error_samples,
        bool event_enabled,
        bool direct_play_enabled,
        char **files,
		int num_files,
        double wave_read_offset_seconds,
        bool mode_infinite
#endif
    ){
#ifdef TEST
		try{
#ifdef __cplusplus_cli
        debug::push_log( "vstidrv::StartRendering" );
#else
        debug::logger << "vstidrv::StartRendering" << endl;
#endif
#endif
        g_cancelRequired = false;
        g_progress = 0.0;

        MIDI_EVENT* lpEvents = merge_events( s_track_events[0], s_track_events[1] );
        MIDI_EVENT* current = lpEvents;

#ifdef USE_DOUBLE
		double* left_ch = new double[g_sample_rate];
		double* right_ch = new double[g_sample_rate];
		double* out_buffer[] = { left_ch, right_ch };
#else
        float* left_ch = new float[g_sample_rate];
        float* right_ch = new float[g_sample_rate];
        float* out_buffer[] = { left_ch, right_ch };
#endif

        s_aeffect->dispatcher( s_aeffect, effSetSampleRate, 0, 0, 0, (float)g_sample_rate );//dispatch_VST_command(effSetSampleRate, 0, 0, 0, kSampleRate);
        s_aeffect->dispatcher( s_aeffect, effMainsChanged, 0, 1, 0, 0 );// dispatch_VST_command(effMainsChanged, 0, 1, 0, 0);
        // ここではブロックサイズ＝サンプリングレートということにする
        s_aeffect->dispatcher( s_aeffect, effSetBlockSize, 0, g_sample_rate, 0, 0 );// dispatch_VST_command(effSetBlockSize, 0, sampleFrames, 0, 0);

        int cur_buf = 0;

        int delay = 0;
        int duration = 0;
        unsigned long dwNow = 0;
        unsigned long dwPrev = 0;
        unsigned long dwDelta;
        unsigned long dwDelay = 0;
        unsigned long dwDeltaDelay = 0;

        int addr_msb, addr_lsb;
        int data_msb, data_lsb;

        int cl_start = 0;
#ifdef TEST
        for( int i = 0; i < 1; i++ ){
#ifdef __cplusplus_cli
            Console::WriteLine( "***********************************************************************" );
#else
            std::cout << "***********************************************************************" << std::endl;
#endif
            MIDI_EVENT *pDebugWork = s_track_events[i];
            while( pDebugWork ){
#ifdef __cplusplus_cli
                Console::Write( "    " );
                Console::Write( "clock=;" + pDebugWork->clock );
                for( int j = 0; j < pDebugWork->dwDataSize; j++ ){
                    Console::Write( " 0x" + Convert::ToString( pDebugWork->pMidiEvent[j], 16 ) );
                }
                pDebugWork = pDebugWork->pNext;
                Console::WriteLine();
#else
                std::cout << "    ";
                std::cout << "clock=;" << pDebugWork->clock;
                for( int j = 0; j < pDebugWork->dwDataSize; j++ ){
                    std::cout << " 0x" << std::hex << pDebugWork->pMidiEvent[j];
                }
                pDebugWork = pDebugWork->pNext;
                std::cout << std::endl;
#endif
            }
        }
#endif

        int max_wave_samples = 0;
        unsigned int total_processed = 0;

        MIDI_EVENT* pWork = s_track_events[1];
        //int eof_clock = 0;
        while( !dwDelay && pWork ){
            // Delayの取得
            if( (pWork->pMidiEvent[0] & 0xf0) == 0xb0 ){
                switch( pWork->pMidiEvent[1] ){
                    case 0x63:
                        addr_msb = pWork->pMidiEvent[2];
                        addr_lsb = 0;
                        break;
                    case 0x62:
                        addr_lsb = pWork->pMidiEvent[2];
                        break;
                    case 0x06:
                        data_msb = pWork->pMidiEvent[2];
                        break;
                    case 0x26:
                        data_lsb = pWork->pMidiEvent[2];
                        if( addr_msb == 0x50 && addr_lsb == 0x01 ){
                            delay = data_msb << 7 | data_lsb;
#ifdef TEST
#ifdef __cplusplus_cli
                            Console::WriteLine( "delay=" + delay );
#else
                            std::cout << "delay=" << delay << std::endl;
#endif
#endif
                            dwDelay = (unsigned long)(delay * (double)g_sample_rate / 1000.0);
                        }
                        break;
                }
            }
            pWork = pWork->pNext;
        }

        while( -1 ){
            if ( g_cancelRequired ) {
                delete [] left_ch;
                delete [] right_ch;
                free_events( lpEvents );
                exit_start_rendering();
                return FALSE;
            }
#ifdef TEST
#ifdef __cplusplus_cli
            Console::WriteLine( "-----------------------------------------------------------------------" );
#else
            std::cout << "-----------------------------------------------------------------------" << std::endl;
#endif
#endif
            MIDI_EVENT* pProcessEvent = current;
            int nEvents = 0;

            while( current->clock == dwNow ){
                // durationを取得
                if( (current->pMidiEvent[0] & 0xf0) == 0xb0 ){
                    switch( current->pMidiEvent[1] ){
                        case 0x63:
                            addr_msb = current->pMidiEvent[2];
                            addr_lsb = 0;
                            break;
                        case 0x62:
                            addr_lsb = current->pMidiEvent[2];
                            break;
                        case 0x06:
                            data_msb = current->pMidiEvent[2];
                            break;
                        case 0x26:
                            data_lsb = current->pMidiEvent[2];
                            // Note Duration in millisec
                            if( addr_msb == 0x50 && addr_lsb == 0x4 ){
                                duration = data_msb << 7 | data_lsb;
#ifdef TEST
#ifdef __cplusplus_cli
                                Console::WriteLine( "duration=" + duration );
#else
                                std::cout << "duration=" << duration<< std::endl;
#endif
#endif
                            }
                            break;
                    }
                }

                nEvents++;
                current = current->pNext;
                if( !current ){
                    break;
                }
            }

            if( !current ){
                break;
            }

#ifdef TEST
#ifdef __cplusplus_cli
            Console::WriteLine( "nEvents=" + nEvents );
#else
            std::cout << "nEvents=" << nEvents << std::endl;
#endif
#endif
            double msNow = totalMilliSec_from_timeCode( dwNow );
            double msPrev = totalMilliSec_from_timeCode( dwPrev );
            double dt = msNow - msPrev;
            dwDelta = (unsigned long)(dt * g_sample_rate / 1000.0);
#ifdef TEST
#ifdef __cplusplus_cli
            Console::WriteLine( "dwNow=" + dwNow );
            Console::WriteLine( "dwPrev=" + dwPrev );
            Console::WriteLine( "dt=" + dt );
            Console::WriteLine( "dwDelta=" + dwDelta );
#else
            std::cout << "dwNow=" << dwNow << std::endl;
            std::cout << "dwPrev=" << dwPrev << std::endl;
            std::cout << "dt=" << dt << std::endl;
            std::cout <<  "dwDelta=" << dwDelta << std::endl;
#endif
#endif

#ifdef TEST
            Console::Write( "malloc for pVSTEvents..." );
#endif
            VstEvents* pVSTEvents = (VstEvents*)malloc( sizeof(VstEvents) + nEvents * sizeof(VstEvent*) );
            pVSTEvents->numEvents = 0;
            pVSTEvents->reserved = (VstIntPtr)0;
#ifdef TEST
            Console::WriteLine( " ...done" );
#endif

            for( int i = 0 ; i < nEvents ; i++ ){
                unsigned char event_code = pProcessEvent->pMidiEvent[0];
                VstEvent* pVSTEvent = NULL;
                VstMidiEvent* pMidiEvent;

                switch( event_code ){
                    case 0xff:
                    case 0xf0:
                    case 0xf7:
                        break;
                    default:
#ifdef TEST
                        Console::Write( "malloc for pMidiEvent..." );
#endif
                        pMidiEvent = (VstMidiEvent*)malloc( sizeof(VstMidiEvent) + pProcessEvent->dwDataSize * sizeof( unsigned char ) );
#ifdef TEST
                        Console::WriteLine( " ...done" );
#endif
                        pMidiEvent->byteSize = sizeof(VstMidiEvent);
                        pMidiEvent->deltaFrames = dwDelta;
                        pMidiEvent->detune = 0;
                        pMidiEvent->flags = 1;
                        pMidiEvent->noteLength = 0;
                        pMidiEvent->noteOffset = 0;
                        pMidiEvent->noteOffVelocity = 0;
                        pMidiEvent->reserved1 = 0;
                        pMidiEvent->reserved2 = 0;
                        pMidiEvent->type = kVstMidiType;
                        memcpy( &pMidiEvent->midiData, &pProcessEvent->pMidiEvent[pProcessEvent->dwOffset], pProcessEvent->dwDataSize );
                        pVSTEvents->events[pVSTEvents->numEvents++] = (VstEvent*)pMidiEvent;
                        break;
                }
                pProcessEvent = pProcessEvent->pNext;
            }
#ifdef TEST
            Console::Write( "calling dispatcher with effProcessEvents..." );
#endif
            s_aeffect->dispatcher( s_aeffect, effProcessEvents, 0, 0, pVSTEvents, 0 );
#ifdef TEST
            Console::WriteLine( " ...done" );
#endif

            while( dwDelta ){
                if ( g_cancelRequired ) {
                    delete [] left_ch;
                    delete [] right_ch;
                    free_events( lpEvents );
                    exit_start_rendering();
                    return FALSE;
                }
                unsigned long dwFrames = dwDelta > (unsigned long)g_sample_rate ?  (unsigned long)g_sample_rate : dwDelta;
#ifdef TEST
                int addr = (int)out_buffer;
                Console::WriteLine( "out_buffer=0x" + Convert::ToString( addr, 16 ) );
                addr = (int)out_buffer[0];
                Console::WriteLine( "out_buffer[0]=0x" + Convert::ToString( addr, 16 ) );
                addr = (int)out_buffer[1];
                Console::WriteLine( "out_buffer[1]=0x" + Convert::ToString( addr, 16 ) );
                Console::Write( "calling processReplacing..." );
#endif
#ifdef USE_DOUBLE
				s_aeffect->processDoubleReplacing( s_aeffect, NULL, out_buffer, dwFrames );
#else
                s_aeffect->processReplacing( s_aeffect, NULL, out_buffer, dwFrames );
#endif
#ifdef TEST
                Console::WriteLine( " ...done" );
#endif

                int iOffset = dwDelay - dwDeltaDelay;
                if( iOffset > (int)dwFrames ){
                    iOffset = (int)dwFrames;
                }

                if ( !iOffset ) {
#ifdef __cplusplus_cli
                        array<Double> ^send_data_l = gcnew array<Double>( dwFrames );
                        array<Double> ^send_data_r = gcnew array<Double>( dwFrames );
#else
                        double *send_data_l = new double[dwFrames];
                        double *send_data_r = new double[dwFrames];
#endif
                        for( int i = 0; i < (int)dwFrames; i++ ){
                            send_data_l[i] = out_buffer[0][i] * amplify_left;
                            send_data_r[i] = out_buffer[1][i] * amplify_right;
                        }
#ifdef TEST
                        Console::Write( "calling WaveIncoming..." );
#endif
#ifdef __cplusplus_cli
                        WaveIncoming( send_data_l, send_data_r );
#else
                        s_wave_incoming_callback( send_data_l, send_data_r, dwFrames );
#endif
#ifdef TEST
                        Console::WriteLine( " ...done" );
#endif
                    total_processed += dwFrames;
                } else {
                    dwDeltaDelay += iOffset;
                }
                dwDelta -= dwFrames;
            }

#ifdef TEST
            Console::Write( "calling free for pVSTEvents..." );
#endif
            free( pVSTEvents );
#ifdef TEST
            Console::WriteLine( " ...done" );
#endif

            dwPrev = dwNow;
            dwNow = current->clock;
            g_progress = total_processed / (double)total_samples * 100.0;
        }

        double msLast = totalMilliSec_from_timeCode( dwNow );
        dwDelta = (unsigned long)(g_sample_rate * ((double)duration + (double)delay) / 1000.0 + dwDeltaDelay);
        while( dwDelta ){
            if ( g_cancelRequired ) {
                free_events( lpEvents );
                exit_start_rendering();
                return FALSE;
            }
            unsigned long dwFrames = dwDelta > (unsigned long)g_sample_rate ?  (unsigned long)g_sample_rate : dwDelta;
#ifdef USE_DOUBLE
            s_aeffect->processDoubleReplacing( s_aeffect, NULL, out_buffer, dwFrames );
#else
            s_aeffect->processReplacing( s_aeffect, NULL, out_buffer, dwFrames );
#endif

#ifdef __cplusplus_cli
                array<Double> ^send_data_l = gcnew array<Double>( dwFrames );
                array<Double> ^send_data_r = gcnew array<Double>( dwFrames );
#else
                double *send_data_l = new double[dwFrames];
                double *send_data_r = new double[dwFrames];
#endif
                for( int i = 0; i < (int)dwFrames; i++ ){
                    send_data_l[i] = out_buffer[0][i] * amplify_left;
                    send_data_r[i] = out_buffer[1][i] * amplify_right;
                }
#ifdef __cplusplus_cli
                WaveIncoming( send_data_l, send_data_r );
#else
                s_wave_incoming_callback( send_data_l, send_data_r, dwFrames );
#endif

            dwDelta -= dwFrames;
            total_processed += dwFrames;
        }

        s_aeffect->dispatcher( s_aeffect, effMainsChanged, 0, 0, 0, 0 );

#ifdef TEST
#ifdef __cplusplus_cli
        Console::WriteLine( "vstidrv::StartRendering; total_processed=" + total_processed );
#else
        std::cout << "vstidrv::StartRendering; total_processed=" << total_processed << std::endl;
#endif
#endif
        free_events( lpEvents );

        delete [] left_ch;
        delete [] right_ch;
#ifdef __cplusplus_cli
        RenderingFinished();
#else
        if( s_rendering_finished_callback ){
            s_rendering_finished_callback();
        }
#endif
#ifdef TEST
        }catch( System::Exception ^ex ){
            Console::WriteLine( "vstidrv::StartRendering; ex=" + ex->ToString() );
		}
#endif
        return TRUE;
    };

    void vstidrv::AbortRendering(){
        g_cancelRequired = true;
//        waveplay::abort();
    };

    double vstidrv::GetProgress(){
        return g_progress;
    };

    vstidrv::vstidrv(){
#ifdef TEST
#ifdef __cplusplus_cli
        debug::push_log( "vstidrv::vstidrv()" );
#else
        if( !debug::logger.is_open() ){
            debug::logger.open( "err_vsti3.txt", ios::out );
        }
        debug::logger << "vstidrv::vstidrv()" << endl;
#endif
#endif
        s_instance = this;
    }

#ifdef __cplusplus_cli
    vstidrv ^vstidrv::GetInstance(){
#ifdef TEST
#ifdef __cplusplus_cli
        debug::push_log( "vstidrv::GetInstance()" );
#endif
#endif
        if ( s_instance == nullptr ){
            s_instance = gcnew vstidrv();
        }
        return s_instance;
    }
#endif

} }

VstIntPtr AudioMaster( AEffect* effect, VstInt32 opcode, VstInt32 index, VstIntPtr value, void* ptr, float opt ){
    VstIntPtr result = 0;

    switch( opcode ){
        case audioMasterVersion :
            result = kVstVersion;
            break;
    }
    return result;
}

void first_buffer_written_callback(){ //vstidrv::として定義すると，clrcallとなってFirstBufferWrittenCallbackとみなされない．c++標準に書き換える場合は，vstidrv::に入れてもOKだろう．
#ifdef TEST
    debug::push_log( "first_buffer_written_callback()" );
#endif
    vstidrv::InvokeFirstBufferWrittenEvent();
}
