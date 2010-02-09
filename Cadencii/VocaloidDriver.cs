#if ENABLE_VOCALOID
/*
 * VocaloidDriver.cs
 * Copyright (C) 2009-2010 kbinani
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
#if DEBUG
//#define TEST
#endif
using System;
using org.kbinani.java.util;
using org.kbinani.vsq;
using VstSdk;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using VstIntPtr = System.Int32;

    public unsafe class VocaloidDriver : vstidrv {
        const int TRUE = 1;
        const int FALSE = 0;

        const int TIME_FORMAT = 480;
        const int DEF_TEMPO = 500000;           // デフォルトのテンポ．

        static VocaloidDriver s_instance;
        Vector<Vector<MidiEvent>> s_track_events;
        Vector<MidiEvent> g_pEvents;
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
        /// StartRenderingメソッドが回っている最中にtrue
        /// </summary>
        boolean rendering = false;

        public override void close() {
            if ( rendering ) {
                g_cancelRequired = true;
                while ( rendering ) {
#if JAVA
                    Thread.sleep( 0 );
#else
                    System.Windows.Forms.Application.DoEvents();
#endif
                }
            }
            base.close();
        }

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
            aEffect.Dispatch( AEffectOpcodes.effMainsChanged, 0, 0, IntPtr.Zero, 0 );
        }

        org.kbinani.media.FirstBufferWrittenCallback s_first_buffer_written_callback;

        public VocaloidDriver() {
            s_instance = this;
        }

        //public event WaveIncomingEventHandler WaveIncoming;

        public void SetFirstBufferWrittenCallback( org.kbinani.media.FirstBufferWrittenCallback handler ) {
            s_first_buffer_written_callback = handler;
        }

        public static void InvokeFirstBufferWrittenEvent() {
            s_instance.s_first_buffer_written_callback();
        }

        public override boolean open( string dll_path, int block_size, int sample_rate ) {
            base.open( dll_path, block_size, sample_rate );
#if DEBUG
            PortUtil.println( "VocaloidDriver#open; dllHandle=0x" + PortUtil.toHexString( dllHandle.ToInt32() ).ToUpper() );
#endif
            g_pEvents = new Vector<MidiEvent>();
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
            s_track_events = new Vector<Vector<MidiEvent>>();
            s_track_events.add( new Vector<MidiEvent>() );
            s_track_events.add( new Vector<MidiEvent>() );
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
                MidiEvent pEvent = new MidiEvent();
                //pEvent = &(new MIDI_EVENT());
                //pEvent->pNext = NULL;
                pEvent.clock = (uint)deltaFrames[i];
                //pEvent.dwOffset = 0;
                if ( targetTrack == 0 ) {
                    pEvent.firstByte = 0xff;
                    pEvent.data = new byte[5];
                    pEvent.data[0] = 0x51;
                    pEvent.data[1] = 0x03;
                    pEvent.data[2] = src[count];
                    pEvent.data[3] = src[count + 1];
                    pEvent.data[4] = src[count + 2];
                } else {
                    pEvent.firstByte = src[count];
                    pEvent.data = new byte[3];
                    pEvent.data[0] = src[count + 1];
                    pEvent.data[1] = src[count + 2];
                    pEvent.data[2] = 0x00;
                }
                s_track_events.get( targetTrack ).add( pEvent );
            }

            return TRUE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="total_samples"></param>
        /// <param name="mode_infinite"></param>
        /// <param name="sample_rate"></param>
        /// <param name="runner">このドライバを駆動しているRenderingRunnerのオブジェクト</param>
        /// <returns></returns>
        public int StartRendering( long total_samples, boolean mode_infinite, int sample_rate, VocaloidRenderingRunner runner ) {
#if DEBUG
            PortUtil.println( "VocaloidDriver#StartRendering; total_samples=" + total_samples + "; sample_rate=" + sample_rate );
#endif
            rendering = true;
            g_cancelRequired = false;
            g_progress = 0.0;
            sampleRate = sample_rate;

            Vector<MidiEvent> lpEvents = merge_events( s_track_events.get( 0 ), s_track_events.get( 1 ) );
            int current_count = -1;
            MidiEvent current = new MidiEvent();// = lpEvents;

            MemoryManager mman = null;
            float* left_ch;
            float* right_ch;
            float** out_buffer;
            try {
                mman = new MemoryManager();
                left_ch = (float*)mman.malloc( sizeof( float ) * sampleRate ).ToPointer();
                right_ch = (float*)mman.malloc( sizeof( float ) * sampleRate ).ToPointer();
                out_buffer = (float**)mman.malloc( sizeof( float* ) * 2 ).ToPointer();
                out_buffer[0] = left_ch;
                out_buffer[1] = right_ch;

#if TEST
                org.kbinani.debug.push_log( "    calling initial dispatch..." );
#endif
                aEffect.Dispatch( AEffectOpcodes.effSetSampleRate, 0, 0, IntPtr.Zero, (float)sampleRate );
                aEffect.Dispatch( AEffectOpcodes.effMainsChanged, 0, 1, IntPtr.Zero, 0 );
                
                // ここではブロックサイズ＝サンプリングレートということにする
                aEffect.Dispatch( AEffectOpcodes.effSetBlockSize, 0, sampleRate, IntPtr.Zero, 0 );
                
                // レンダリングの途中で停止した場合，ここでProcessする部分が無音でない場合がある
                for ( int i = 0; i < 3; i++ ) {
                    aEffect.ProcessReplacing( IntPtr.Zero, new IntPtr( out_buffer ), sampleRate );
                }
#if TEST
                org.kbinani.debug.push_log( "    ...done" );
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
                int total_processed2 = 0;
#if TEST
                org.kbinani.debug.push_log( "    getting dwDelay..." );
#endif
                dwDelay = 0;
                Vector<MidiEvent> list = s_track_events.get( 1 );
                int list_size = list.size();
                for ( int i = 0; i < list_size; i++ ) {
                    MidiEvent work = list.get( i );
                    if ( (work.firstByte & 0xf0) == 0xb0 ) {
                        switch ( work.data[0] ) {
                            case 0x63:
                                addr_msb = work.data[1];
                                addr_lsb = 0;
                                break;
                            case 0x62:
                                addr_lsb = work.data[1];
                                break;
                            case 0x06:
                                data_msb = work.data[1];
                                break;
                            case 0x26:
                                data_lsb = work.data[1];
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
                org.kbinani.debug.push_log( "    ...done; dwDelay=" + dwDelay );
#endif

                while ( true ) {
#if TEST
                    org.kbinani.debug.push_log( "g_cancelRequired=" + g_cancelRequired );
#endif
                    if ( g_cancelRequired ) {
                        lpEvents.clear();
                        rendering = false;
                        return FALSE;
                    }
#if TEST
                    org.kbinani.debug.push_log( "-----------------------------------------------------------------------" );
#endif
                    //MIDI_EVENT pProcessEvent = current;
                    int process_event_count = current_count;
                    int nEvents = 0;

#if TEST
                    org.kbinani.debug.push_log( "lpEvents.Count=" + lpEvents.size() );
#endif
                    if ( current_count < 0 ) {
                        current_count = 0;
                        current = lpEvents.get( current_count );
                        process_event_count = current_count;
                    }
                    while ( current.clock == dwNow ) {
                        // durationを取得
                        if ( (current.firstByte & 0xf0) == 0xb0 ) {
                            switch ( current.data[0] ) {
                                case 0x63:
                                    addr_msb = current.data[1];
                                    addr_lsb = 0;
                                    break;
                                case 0x62:
                                    addr_lsb = current.data[1];
                                    break;
                                case 0x06:
                                    data_msb = current.data[1];
                                    break;
                                case 0x26:
                                    data_lsb = current.data[1];
                                    // Note Duration in millisec
                                    if ( addr_msb == 0x50 && addr_lsb == 0x4 ) {
                                        duration = data_msb << 7 | data_lsb;
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

                    double msNow = msec_from_clock( dwNow );
                    dwDelta = (int)(msNow / 1000.0 * sampleRate) - total_processed;
#if TEST
                    org.kbinani.debug.push_log( "dwNow=" + dwNow );
                    org.kbinani.debug.push_log( "dwPrev=" + dwPrev );
                    org.kbinani.debug.push_log( "dwDelta=" + dwDelta );
#endif
                    VstEvents* pVSTEvents = (VstEvents*)mman.malloc( sizeof( VstEvent ) + nEvents * sizeof( VstEvent* ) ).ToPointer();
                    pVSTEvents->numEvents = 0;
                    pVSTEvents->reserved = (VstIntPtr)0;

                    for ( int i = 0; i < nEvents; i++ ) {
                        MidiEvent pProcessEvent = lpEvents.get( process_event_count );
                        byte event_code = pProcessEvent.firstByte;
                        VstEvent* pVSTEvent = (VstEvent*)0;
                        VstMidiEvent* pMidiEvent;

                        switch ( event_code ) {
                            case 0xff:
                            case 0xf0:
                            case 0xf7:
                                break;
                            default:
                                pMidiEvent = (VstMidiEvent*)mman.malloc( (int)(sizeof( VstMidiEvent ) + (pProcessEvent.data.Length + 1) * sizeof( byte )) ).ToPointer();
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
                                pMidiEvent->midiData[0] = pProcessEvent.firstByte;
                                for ( int j = 0; j < pProcessEvent.data.Length; j++ ) {
                                    pMidiEvent->midiData[j + 1] = pProcessEvent.data[j];
                                }
                                pVSTEvents->events[pVSTEvents->numEvents++] = (int)(VstEvent*)pMidiEvent;
                                break;
                        }
                        process_event_count++;
                        //pProcessEvent = lpEvents[process_event_count];
                    }
#if TEST
                    org.kbinani.debug.push_log( "calling Dispatch with effProcessEvents..." );
#endif
                    aEffect.Dispatch( AEffectXOpcodes.effProcessEvents, 0, 0, new IntPtr( pVSTEvents ), 0 );
#if TEST
                    org.kbinani.debug.push_log( "...done" );
#endif

                    while ( dwDelta > 0 ) {
                        if ( g_cancelRequired ) {
                            lpEvents.clear();
                            rendering = false;
                            return FALSE;
                        }
                        int dwFrames = dwDelta > sampleRate ? sampleRate : dwDelta;
#if TEST
                        org.kbinani.debug.push_log( "calling ProcessReplacing..." );
#endif
                        aEffect.ProcessReplacing( IntPtr.Zero, new IntPtr( out_buffer ), dwFrames );
#if TEST
                        org.kbinani.debug.push_log( "...done" );
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
                            total_processed2 += dwFrames;
                            runner.waveIncomingImpl( send_data_l, send_data_r );
                        } else {
                            dwDeltaDelay += iOffset;
                        }
                        dwDelta -= dwFrames;
                        total_processed += dwFrames;
                    }

                    dwPrev = dwNow;
                    dwNow = (int)current.clock;
                    g_progress = total_processed / (double)total_samples * 100.0;
                }

                double msLast = msec_from_clock( dwNow );
                dwDelta = (int)(sampleRate * ((double)duration + (double)delay) / 1000.0 + dwDeltaDelay);
                if ( total_samples - total_processed2 > dwDelta ) {
                    dwDelta = (int)total_samples - total_processed2;
                }
                while ( dwDelta > 0 ) {
                    if ( g_cancelRequired ) {
#if DEBUG
                        PortUtil.println( "VocaloidDriver#StartRendering; cancel A" );
#endif
                        lpEvents.clear();
                        rendering = false;
                        return FALSE;
                    }
                    int dwFrames = dwDelta > sampleRate ? sampleRate : dwDelta;
#if TEST
                    org.kbinani.debug.push_log( "calling ProcessReplacing..." );
#endif
                    aEffect.ProcessReplacing( IntPtr.Zero, new IntPtr( out_buffer ), dwFrames );
#if TEST
                    org.kbinani.debug.push_log( "...done" );
#endif

                    double[] send_data_l = new double[dwFrames];
                    double[] send_data_r = new double[dwFrames];
                    for ( int i = 0; i < (int)dwFrames; i++ ) {
                        send_data_l[i] = out_buffer[0][i];
                        send_data_r[i] = out_buffer[1][i];
                    }
                    total_processed2 += dwFrames;
                    runner.waveIncomingImpl( send_data_l, send_data_r );
                    send_data_l = null;
                    send_data_r = null;

                    dwDelta -= dwFrames;
                    total_processed += dwFrames;
                }

#if TEST
                PortUtil.println( "vstidrv::StartRendering; total_processed=" + total_processed );
#endif

                if ( mode_infinite ) {
                    double[] silence_l = new double[blockSize];
                    double[] silence_r = new double[blockSize];
                    while ( !g_cancelRequired ) {
                        total_processed2 += blockSize;
                        runner.waveIncomingImpl( silence_l, silence_r );
                    }
                    silence_l = null;
                    silence_r = null;
                }

                aEffect.Dispatch( AEffectOpcodes.effMainsChanged, 0, 0, IntPtr.Zero, 0 );
                lpEvents.clear();
#if DEBUG
                PortUtil.println( "VocaloidDriver#StartRendering; total_processed=" + total_processed + "; total_processed2=" + total_processed2 );
#endif
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "VocaloidDriver#StartRendering; ex=" + ex );
            } finally {
                if ( mman != null ) {
                    try {
                        mman.dispose();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "VocaloidDriver#StartRendering; ex2=" + ex2 );
                    }
                }
                exit_start_rendering();
            }
            rendering = false;
            return 1;
        }

        public boolean isRendering() {
            return rendering;
        }

        public void AbortRendering() {
            g_cancelRequired = true;
        }

        public double GetProgress() {
            return g_progress;
        }

        private Vector<MidiEvent> merge_events( Vector<MidiEvent> x0, Vector<MidiEvent> y0 ) {
            Vector<MidiEvent> ret = new Vector<MidiEvent>();
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
                        MidiEvent m = ret.get( i );
                        ret.set( i, ret.get( i + 1 ) );
                        ret.set( i + 1, m );
                        changed = true;
                    }
                }

            }
            return ret;
        }
    }

}
#endif
