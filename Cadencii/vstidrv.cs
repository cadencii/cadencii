#if ENABLE_VOCALOID
/*
 * VSTiProxy.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Runtime.InteropServices;
using bocoree;
using bocoree.util;
using VstSdk;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
    using VstInt32 = Int32;
    using VstIntPtr = Int32;

    public struct TempoInfo {
        /// <summary>
        /// テンポが変更される時刻を表すクロック数
        /// </summary>
        public int Clock;
        /// <summary>
        /// テンポ
        /// </summary>
        public int Tempo;
        /// <summary>
        /// テンポが変更される時刻
        /// </summary>
        public double TotalSec;
    }

    public struct MIDI_EVENT : IComparable<MIDI_EVENT> {
        public uint clock;
        public uint dwDataSize;
        public byte dwOffset;
        public byte[] pMidiEvent;

        public int CompareTo( MIDI_EVENT item ) {
            return (int)clock - (int)item.clock;
        }
    }

    public unsafe class vstidrv {
        private delegate IntPtr PVSTMAIN( [MarshalAs( UnmanagedType.FunctionPtr )]audioMasterCallback audioMaster );

        private PVSTMAIN s_main_delegate;
        private audioMasterCallback s_audio_master;

        const int TRUE = 1;
        const int FALSE = 0;

        const int TIME_FORMAT = 480;
        const int DEF_TEMPO = 500000;           // デフォルトのテンポ．
        static int g_block_size;              // 波形バッファのサイズ。
        static int g_sample_rate;             // サンプリングレート。VOCALOID2 VSTiは限られたサンプリングレートしか受け付けない。たいてい44100Hzにする

        static vstidrv s_instance;
        /// <summary>
        /// 読込んだdllから作成したVOCALOID2の本体。VOCALOID2への操作はs_aeffect->dispatcherで行う
        /// </summary>
        AEffect s_aeffect;
        Vector<Vector<MIDI_EVENT>> s_track_events;
        /// <summary>
        /// 読込んだdllのハンドル
        /// </summary>
        IntPtr g_dllHandle;
        Vector<MIDI_EVENT> g_pEvents;
        int g_pCurrentEvent;
        /// <summary>
        /// s_track_events[0]のmidiイベントを受信済みかどうかを表すフラグ
        /// </summary>
        boolean g_midiPrepared0;
        /// <summary>
        /// s_track_events[1]のmidiイベントを受信済みかどうかを表すフラグ
        /// </summary>
        boolean g_midiPrepared1;
        int g_tcCurrent;
        int g_tcPrevious;
        int g_saProcessed;
        int g_saTotalSamples;
        Vector<TempoInfo> g_tempoList;
        int g_numTempoList;
        boolean g_cancelRequired;
        double g_progress;


        /// <summary>
        /// 指定したタイムコードにおける，曲頭から測った時間を調べる
        /// </summary>
        private double msec_from_clock( int timeCode ) {
            double ret = 0.0;
            int index = -1;
            int c = g_tempoList.size();
            for ( int i = 0; i < c; i++ ) {
                if ( timeCode <= g_tempoList.get( i ).Clock ) {
                    break;
                }
                index = i;
            }
            if ( index >= 0 ) {
                TempoInfo item = g_tempoList.get( index );
                ret = item.TotalSec + (timeCode - item.Clock) * (double)item.Tempo / (1000.0 * TIME_FORMAT);
            } else {
                ret = timeCode * (double)DEF_TEMPO / (1000.0 * TIME_FORMAT);
            }
            return ret;
        }

        void exit_start_rendering() {
            s_aeffect.Dispatch( ref s_aeffect, AEffectOpcodes.effMainsChanged, 0, 0, (void*)0, 0 );
        }

        Boare.Lib.Media.FirstBufferWrittenCallback s_first_buffer_written_callback;

        public vstidrv() {
            s_instance = this;
        }

        public static vstidrv GetInstance() {
            if ( s_instance == null ) {
                s_instance = new vstidrv();
            }
            return s_instance;
        }

        public event WaveIncomingEventHandler WaveIncoming;
        public event EventHandler RenderingFinished;

        public void SetFirstBufferWrittenCallback( Boare.Lib.Media.FirstBufferWrittenCallback handler ) {
            s_first_buffer_written_callback = handler;
        }

        public static void InvokeFirstBufferWrittenEvent() {
            s_instance.s_first_buffer_written_callback();
        }

        public boolean Init( char[] dll_path, int block_size, int sample_rate ) {
#if DEBUG
            bocoree.debug.push_log( "vstidrv.Init" );
#endif
            g_pEvents = new Vector<MIDI_EVENT>();
            g_midiPrepared0 = false;
            g_midiPrepared1 = false;
            g_tcCurrent = 0;
            g_tcPrevious = 0;
            g_saProcessed = 0;
            g_saTotalSamples = 0;
            g_tempoList = new Vector<TempoInfo>();
            g_numTempoList = 0;
            g_cancelRequired = false;
            g_progress = 0.0;
            try {

                g_block_size = block_size;
                g_sample_rate = sample_rate;
                String str = new String( dll_path );
                IntPtr dll_handle = win32.LoadLibraryExW( str, IntPtr.Zero, win32.LOAD_WITH_ALTERED_SEARCH_PATH );
                System.Threading.Thread.Sleep( 250 );
#if TEST
                bocoree.debug.push_log( "    dll_handle=0x" + Convert.ToString( dll_handle.ToInt32(), 16 ) );
#endif
                g_dllHandle = dll_handle;

                s_main_delegate = (PVSTMAIN)Marshal.GetDelegateForFunctionPointer( win32.GetProcAddress( g_dllHandle, "main" ), 
                                                                                   typeof( PVSTMAIN ) );
                System.Threading.Thread.Sleep( 250 );
                if ( s_main_delegate == null ) {
                    return false;
                }

                s_audio_master = new audioMasterCallback( AudioMaster );
                System.Threading.Thread.Sleep( 250 );

                IntPtr ptr_aeffect = IntPtr.Zero;
                try {
                    ptr_aeffect = s_main_delegate( s_audio_master );
                } catch ( Exception ex ){
#if TEST
                    AppManager.debugWriteLine( "    vstidrv.Init; invoking s_main_delegate; ex=" + ex );
#endif
                }
#if TEST
                //bocoree.debug.push_log( "    s_aeffect=0x" + Convert.ToString( (int)(&s_aeffect), 16 ) );
#endif
                if ( ptr_aeffect == IntPtr.Zero ) {
                    return false;
                }
                s_aeffect = (AEffect)Marshal.PtrToStructure( ptr_aeffect, typeof( AEffect ) );
                s_aeffect.Dispatch( ref s_aeffect, AEffectOpcodes.effOpen, 0, 0, (void*)0, 0 );
                s_aeffect.Dispatch( ref s_aeffect, AEffectOpcodes.effSetSampleRate, 0, 0, (void*)0, (float)g_sample_rate );
                s_aeffect.Dispatch( ref s_aeffect, AEffectOpcodes.effSetBlockSize, 0, g_block_size, (void*)0, 0 );

                s_track_events = new Vector<Vector<MIDI_EVENT>>();
                s_track_events.add( new Vector<MIDI_EVENT>() );
                s_track_events.add( new Vector<MIDI_EVENT>() );
            } catch ( Exception ex ) {
#if TEST
                PortUtil.println( "vstidrv+Init" );
                PortUtil.println( "    ex=" + ex );
#endif
                return false;
            }
            return true;
        }

        public int SendEvent( byte[] src, int[] deltaFrames/*, int numEvents*/, int targetTrack ) {
            int count;
            int numEvents = deltaFrames.Length;
            if ( targetTrack == 0 ) {
                if ( g_tempoList == null ) {
                    g_tempoList = new Vector<TempoInfo>();
                } else {
                    g_tempoList.clear();
                }
                if ( numEvents <= 0 ) {
                    g_numTempoList = 1;
                    TempoInfo ti = new TempoInfo();
                    ti.Clock = 0;
                    ti.Tempo = DEF_TEMPO;
                    ti.TotalSec = 0.0;
                    g_tempoList.add( ti );
                } else {
                    if ( deltaFrames[0] == 0 ) {
                        g_numTempoList = numEvents;
                    } else {
                        g_numTempoList = numEvents + 1;
                        TempoInfo ti = new TempoInfo();
                        ti.Clock = 0;
                        ti.Tempo = DEF_TEMPO;
                        ti.TotalSec = 0.0;
                        g_tempoList.add( ti );
                    }
                    int prev_tempo = DEF_TEMPO;
                    int prev_clock = 0;
                    double total = 0.0;
                    count = -3;
                    for ( int i = 0; i < numEvents; i++ ) {
                        count += 3;
                        int tempo = (int)(src[count + 2] | (src[count + 1] << 8) | (src[count] << 16));
                        total += (deltaFrames[i] - prev_clock) * (double)prev_tempo / (1000.0 * TIME_FORMAT);
                        TempoInfo ti = new TempoInfo();
                        ti.Clock = deltaFrames[i];
                        ti.Tempo = tempo;
                        ti.TotalSec = total;
                        g_tempoList.add( ti );
                        prev_tempo = tempo;
                        prev_clock = deltaFrames[i];
                    }
                }
            }

            // 与えられたイベント情報をs_track_eventsに収納
            count = -3;
            int pPrev = 0;
            s_track_events.get( targetTrack ).clear();
            for ( int i = 0; i < numEvents; i++ ) {
                count += 3;
                MIDI_EVENT pEvent = new MIDI_EVENT();
                //pEvent = &(new MIDI_EVENT());
                //pEvent->pNext = NULL;
                pEvent.clock = (uint)deltaFrames[i];
                pEvent.dwOffset = 0;
                if ( targetTrack == 0 ) {
                    pEvent.dwDataSize = 6;
                    pEvent.pMidiEvent = new byte[6];
                    pEvent.pMidiEvent[0] = 0xff;
                    pEvent.pMidiEvent[1] = 0x51;
                    pEvent.pMidiEvent[2] = 0x03;
                    pEvent.pMidiEvent[3] = src[count];
                    pEvent.pMidiEvent[4] = src[count + 1];
                    pEvent.pMidiEvent[5] = src[count + 2];
                } else {
                    pEvent.dwDataSize = 4;
                    pEvent.pMidiEvent = new byte[4];
                    pEvent.pMidiEvent[0] = src[count];
                    pEvent.pMidiEvent[1] = src[count + 1];
                    pEvent.pMidiEvent[2] = src[count + 2];
                    pEvent.pMidiEvent[3] = 0x00;
                }
                s_track_events.get( targetTrack ).add( pEvent );
            }

            return TRUE;
        }

        public int StartRendering( long total_samples, boolean mode_infinite ) {
#if TEST
            bocoree.debug.push_log( "vstidrv.StartRendering" );
#endif
            g_cancelRequired = false;
            g_progress = 0.0;

            Vector<MIDI_EVENT> lpEvents = merge_events( s_track_events.get( 0 ), s_track_events.get( 1 ) );
            int current_count = -1;
            MIDI_EVENT current = new MIDI_EVENT();// = lpEvents;

            IntPtr ptr_left_ch = Marshal.AllocHGlobal( sizeof( float ) * g_sample_rate );
            IntPtr ptr_right_ch = Marshal.AllocHGlobal( sizeof( float ) * g_sample_rate );
            float* left_ch = (float*)ptr_left_ch.ToPointer();
            float* right_ch = (float*)ptr_right_ch.ToPointer();
            IntPtr ptr_outbuffer = Marshal.AllocHGlobal( sizeof( float* ) * 2 );
            float** out_buffer = (float**)ptr_outbuffer.ToPointer();
            out_buffer[0] = left_ch;
            out_buffer[1] = right_ch;

#if TEST
            bocoree.debug.push_log( "    calling initial dispatch..." );
#endif
            s_aeffect.Dispatch( ref s_aeffect, AEffectOpcodes.effSetSampleRate, 0, 0, (void*)0, (float)g_sample_rate );//dispatch_VST_command(effSetSampleRate, 0, 0, 0, kSampleRate);
            s_aeffect.Dispatch( ref s_aeffect, AEffectOpcodes.effMainsChanged, 0, 1, (void*)0, 0 );// dispatch_VST_command(effMainsChanged, 0, 1, 0, 0);
            // ここではブロックサイズ＝サンプリングレートということにする
            s_aeffect.Dispatch( ref s_aeffect, AEffectOpcodes.effSetBlockSize, 0, g_sample_rate, (void*)0, 0 );// dispatch_VST_command(effSetBlockSize, 0, sampleFrames, 0, 0);
#if TEST
            bocoree.debug.push_log( "    ...done" );
#endif

            int delay = 0;
            int duration = 0;
            int dwNow = 0;
            int dwPrev = 0;
            int dwDelta;
            int dwDelay = 0;
            int dwDeltaDelay = 0;

            int addr_msb = 0, addr_lsb = 0;
            int data_msb = 0, data_lsb = 0;

            int total_processed = 0;
#if TEST
            bocoree.debug.push_log( "    getting dwDelay..." );
#endif
            dwDelay = 0;
            Vector<MIDI_EVENT> list = s_track_events.get( 1 );
            int list_size = list.size();
            for ( int i = 0; i < list_size; i++ ) {
                MIDI_EVENT work = list.get( i );
                if ( (work.pMidiEvent[0] & 0xf0) == 0xb0 ) {
                    switch ( work.pMidiEvent[1] ) {
                        case 0x63:
                            addr_msb = work.pMidiEvent[2];
                            addr_lsb = 0;
                            break;
                        case 0x62:
                            addr_lsb = work.pMidiEvent[2];
                            break;
                        case 0x06:
                            data_msb = work.pMidiEvent[2];
                            break;
                        case 0x26:
                            data_lsb = work.pMidiEvent[2];
                            if ( addr_msb == 0x50 && addr_lsb == 0x01 ) {
                                dwDelay = (data_msb & 0xff) << 7 | (data_lsb & 0x7f);
                            }
                            break;
                    }
                }
                if ( dwDelay > 0 ) {
                    break;
                }
            }
#if TEST
            bocoree.debug.push_log( "    ...done; dwDelay=" + dwDelay );
#endif

            while ( true ) {
#if TEST
                bocoree.debug.push_log( "g_cancelRequired=" + g_cancelRequired );
#endif
                if ( g_cancelRequired ) {
                    Marshal.FreeHGlobal( ptr_left_ch );
                    Marshal.FreeHGlobal( ptr_right_ch );
                    Marshal.FreeHGlobal( ptr_outbuffer );
                    lpEvents.clear();
                    exit_start_rendering();
                    return FALSE;
                }
#if TEST
                bocoree.debug.push_log( "-----------------------------------------------------------------------" );
#endif
                //MIDI_EVENT pProcessEvent = current;
                int process_event_count = current_count;
                int nEvents = 0;

#if TEST
                bocoree.debug.push_log( "lpEvents.Count=" + lpEvents.size() );
#endif
                if ( current_count < 0 ) {
                    current_count = 0;
                    current = lpEvents.get( current_count );
                    process_event_count = current_count;
                }
                while ( current.clock == dwNow ) {
                    // durationを取得
                    if ( (current.pMidiEvent[0] & 0xf0) == 0xb0 ) {
                        switch ( current.pMidiEvent[1] ) {
                            case 0x63:
                                addr_msb = current.pMidiEvent[2];
                                addr_lsb = 0;
                                break;
                            case 0x62:
                                addr_lsb = current.pMidiEvent[2];
                                break;
                            case 0x06:
                                data_msb = current.pMidiEvent[2];
                                break;
                            case 0x26:
                                data_lsb = current.pMidiEvent[2];
                                // Note Duration in millisec
                                if ( addr_msb == 0x50 && addr_lsb == 0x4 ) {
                                    duration = data_msb << 7 | data_lsb;
#if TEST
                                    bocoree.debug.push_log( "duration=" + duration );
#endif
                                }
                                break;
                        }
                    }

                    nEvents++;
                    if ( current_count + 1 < lpEvents.size() ) {
                        current_count++;
                        current = lpEvents.get( current_count );
                    } else {
                        break;
                    }
                }

                if ( current_count + 1 >= lpEvents.size() ) {
                    break;
                }

#if TEST
                bocoree.debug.push_log( "nEvents=" + nEvents );
#endif
                double msNow = msec_from_clock( dwNow );
                dwDelta = (int)(msNow / 1000.0 * g_sample_rate) - total_processed;
#if TEST
                bocoree.debug.push_log( "dwNow=" + dwNow );
                bocoree.debug.push_log( "dwPrev=" + dwPrev );
                bocoree.debug.push_log( "dwDelta=" + dwDelta );
#endif
                IntPtr ptr_pvst_events = Marshal.AllocHGlobal( sizeof( VstEvent ) + nEvents * sizeof( VstEvent* ) );
                VstEvents* pVSTEvents = (VstEvents*)ptr_pvst_events.ToPointer();
                pVSTEvents->numEvents = 0;
                pVSTEvents->reserved = (VstIntPtr)0;

                for ( int i = 0; i < nEvents; i++ ) {
                    MIDI_EVENT pProcessEvent = lpEvents.get( process_event_count );
                    byte event_code = pProcessEvent.pMidiEvent[0];
                    VstEvent* pVSTEvent = (VstEvent*)0;
                    VstMidiEvent* pMidiEvent;

                    switch ( event_code ) {
                        case 0xff:
                        case 0xf0:
                        case 0xf7:
                            break;
                        default:
                            pMidiEvent = (VstMidiEvent*)Marshal.AllocHGlobal( (int)(sizeof( VstMidiEvent ) + pProcessEvent.dwDataSize * sizeof( byte )) ).ToPointer();
                            pMidiEvent->byteSize = sizeof( VstMidiEvent );
                            pMidiEvent->deltaFrames = dwDelta;
                            pMidiEvent->detune = 0;
                            pMidiEvent->flags = 1;
                            pMidiEvent->noteLength = 0;
                            pMidiEvent->noteOffset = 0;
                            pMidiEvent->noteOffVelocity = 0;
                            pMidiEvent->reserved1 = 0;
                            pMidiEvent->reserved2 = 0;
                            pMidiEvent->type = VstEventTypes.kVstMidiType;
                            for ( int j = 0; j < pProcessEvent.dwDataSize; j++ ) {
                                pMidiEvent->midiData[j] = pProcessEvent.pMidiEvent[pProcessEvent.dwOffset + j];
                            }
                            pVSTEvents->events[pVSTEvents->numEvents++] = (int)(VstEvent*)pMidiEvent;
                            break;
                    }
                    process_event_count++;
                    //pProcessEvent = lpEvents[process_event_count];
                }
#if TEST
                bocoree.debug.push_log( "calling Dispatch with effProcessEvents..." );
#endif
                s_aeffect.Dispatch( ref s_aeffect, AEffectXOpcodes.effProcessEvents, 0, 0, pVSTEvents, 0 );
#if TEST
                bocoree.debug.push_log( "...done" );
#endif

                while ( dwDelta > 0 ) {
                    if ( g_cancelRequired ) {
                        Marshal.FreeHGlobal( ptr_left_ch );
                        Marshal.FreeHGlobal( ptr_right_ch );
                        Marshal.FreeHGlobal( ptr_outbuffer );
                        lpEvents.clear();
                        exit_start_rendering();
                        return FALSE;
                    }
                    int dwFrames = dwDelta > g_sample_rate ? g_sample_rate : dwDelta;
#if TEST
                    bocoree.debug.push_log( "calling ProcessReplacing..." );
#endif
                    s_aeffect.ProcessReplacing( ref s_aeffect, (float**)0, out_buffer, dwFrames );
#if TEST
                    bocoree.debug.push_log( "...done" );
#endif

                    int iOffset = dwDelay - dwDeltaDelay;
                    if ( iOffset > (int)dwFrames ) {
                        iOffset = (int)dwFrames;
                    }

                    if ( iOffset == 0 ) {
                        double[] send_data_l = new double[dwFrames];
                        double[] send_data_r = new double[dwFrames];
                        for ( int i = 0; i < (int)dwFrames; i++ ) {
                            send_data_l[i] = out_buffer[0][i];
                            send_data_r[i] = out_buffer[1][i];
                        }
                        WaveIncoming( send_data_l, send_data_r );
                    } else {
                        dwDeltaDelay += iOffset;
                    }
                    dwDelta -= dwFrames;
                    total_processed += dwFrames;
                }

                Marshal.FreeHGlobal( ptr_pvst_events );

                dwPrev = dwNow;
                dwNow = (int)current.clock;
                g_progress = total_processed / (double)total_samples * 100.0;
            }

            double msLast = msec_from_clock( dwNow );
            dwDelta = (int)(g_sample_rate * ((double)duration + (double)delay) / 1000.0 + dwDeltaDelay);
            if ( total_samples - total_processed > dwDelta ) {
                dwDelta = (int)total_samples - total_processed;
            }
            while ( dwDelta > 0 ) {
                if ( g_cancelRequired ) {
                    lpEvents.clear();
                    exit_start_rendering();
                    return FALSE;
                }
                int dwFrames = dwDelta > g_sample_rate ? g_sample_rate : dwDelta;
#if TEST
                bocoree.debug.push_log( "calling ProcessReplacing..." );
#endif
                s_aeffect.ProcessReplacing( ref s_aeffect, (float**)0, out_buffer, dwFrames );
#if TEST
                bocoree.debug.push_log( "...done" );
#endif

                double[] send_data_l = new double[dwFrames];
                double[] send_data_r = new double[dwFrames];
                for ( int i = 0; i < (int)dwFrames; i++ ) {
                    send_data_l[i] = out_buffer[0][i];
                    send_data_r[i] = out_buffer[1][i];
                }
                WaveIncoming( send_data_l, send_data_r );
                send_data_l = null;
                send_data_r = null;

                dwDelta -= dwFrames;
                total_processed += dwFrames;
            }

#if TEST
            PortUtil.println( "vstidrv::StartRendering; total_processed=" + total_processed );
#endif

            if ( mode_infinite ) {
                double[] silence_l = new double[g_block_size];
                double[] silence_r = new double[g_block_size];
                while ( !g_cancelRequired ) {
                    /*s_aeffect.ProcessReplacing( ref s_aeffect, (float**)0, out_buffer, g_block_size );
                    for ( int i = 0; i < g_block_size; i++ ) {
                        silence_l[i] = out_buffer[0][i];
                        silence_r[i] = out_buffer[1][i];
                    }*/
                    WaveIncoming( silence_l, silence_r );
                }
                silence_l = null;
                silence_r = null;
            }

            s_aeffect.Dispatch( ref s_aeffect, AEffectOpcodes.effMainsChanged, 0, 0, (void*)0, 0 );
            lpEvents.clear();
            RenderingFinished( this, null );

            return 1;
        }

        public void AbortRendering() {
            g_cancelRequired = true;
        }

        public double GetProgress() {
            return g_progress;
        }

        public void Terminate() {
            try {
                s_aeffect.Dispatch( ref s_aeffect, AEffectOpcodes.effClose, 0, 0, (void*)0, 0.0f );
            } catch {
            }
        }

        private Vector<MIDI_EVENT> merge_events( Vector<MIDI_EVENT> x0, Vector<MIDI_EVENT> y0 ) {
            Vector<MIDI_EVENT> ret = new Vector<MIDI_EVENT>();
            for ( int i = 0; i < x0.size(); i++ ) {
                ret.add( x0.get( i ) );
            }
            for ( int i = 0; i < y0.size(); i++ ) {
                ret.add( y0.get( i ) );
            }
            boolean changed = true;
            while ( changed ) {
                changed = false;
                for ( int i = 0; i < ret.size() - 1; i++ ) {
                    if ( ret.get( i ).CompareTo( ret.get( i + 1 ) ) > 0 ) {
                        MIDI_EVENT m = ret.get( i );
                        ret.set( i, ret.get( i + 1 ) );
                        ret.set( i + 1, m );
                        changed = true;
                    }
                }

            }
            return ret;
        }

        private VstIntPtr AudioMaster( AEffect* effect, VstInt32 opcode, VstInt32 index, VstIntPtr value, void* ptr, float opt ) {
            VstIntPtr result = 0;

            switch ( opcode ) {
                case AudioMasterOpcodes.audioMasterVersion:
                    result = Constants.kVstVersion;
                    break;
            }
            return result;
        }
    }

}
#endif
