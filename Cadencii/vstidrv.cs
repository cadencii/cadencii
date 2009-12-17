#if ENABLE_VOCALOID
/*
 * VSTiProxy.cs
 * Copyright (c) 2008-2009 kbinani
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
using System;
using System.Runtime.InteropServices;
using System.Threading;
using bocoree;
using bocoree.java.util;
using org.kbinani.vsq;
using VstSdk;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using VstInt32 = Int32;
    using VstIntPtr = Int32;

    public delegate void WaveIncomingEventHandler( double[] L, double[] R );

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

    public class GlobalMemoryContext {
        private Vector<IntPtr> list = new Vector<IntPtr>();

        public IntPtr malloc( int bytes ) {
            IntPtr ret = Marshal.AllocHGlobal( bytes );
            list.add( ret );
            return ret;
        }

        public void dispose() {
            for ( Iterator itr = list.iterator(); itr.hasNext(); ) {
                IntPtr v = (IntPtr)itr.next();
                Marshal.FreeHGlobal( v );
            }
        }

        ~GlobalMemoryContext() {
            dispose();
        }
    }

    public unsafe class vstidrv {
        protected delegate IntPtr PVSTMAIN( [MarshalAs( UnmanagedType.FunctionPtr )]audioMasterCallback audioMaster );

        public boolean loaded = false;
        public String path = "";
        public String name = "";
        protected PVSTMAIN mainDelegate;
        protected audioMasterCallback audioMaster;
        /// <summary>
        /// 読込んだdllから作成したVOCALOID2の本体。VOCALOID2への操作はs_aeffect->dispatcherで行う
        /// </summary>
        protected AEffect aEffect;
        /// <summary>
        /// 読込んだdllのハンドル
        /// </summary>
        protected IntPtr dllHandle;
        /// <summary>
        /// 波形バッファのサイズ。
        /// </summary>
        protected int blockSize;
        /// <summary>
        /// サンプリングレート。VOCALOID2 VSTiは限られたサンプリングレートしか受け付けない。たいてい44100Hzにする
        /// </summary>
        protected int sampleRate;
        /// <summary>
        /// バッファ(bufferLeft, bufferRight)の長さ
        /// </summary>
        const int BUFLEN = 44100;
        /// <summary>
        /// 左チャンネル用バッファ
        /// </summary>
        IntPtr bufferLeft = IntPtr.Zero;
        /// <summary>
        /// 右チャンネル用バッファ
        /// </summary>
        IntPtr bufferRight = IntPtr.Zero;
        /// <summary>
        /// 左右チャンネルバッファの配列(buffers={bufferLeft, bufferRight})
        /// </summary>
        IntPtr buffers = IntPtr.Zero;

        private void initBuffer() {
            if ( bufferLeft == IntPtr.Zero ) {
                bufferLeft = Marshal.AllocHGlobal( sizeof( float ) * BUFLEN );
            }
            if ( bufferRight == IntPtr.Zero ) {
                bufferRight = Marshal.AllocHGlobal( sizeof( float ) * BUFLEN );
            }
            if ( buffers == IntPtr.Zero ) {
                buffers = Marshal.AllocHGlobal( sizeof( float* ) * 2 );
            }
        }

        private void releaseBuffer() {
            if ( bufferLeft != IntPtr.Zero ) {
                Marshal.FreeHGlobal( bufferLeft );
                bufferLeft = IntPtr.Zero;
            }
            if ( bufferRight != IntPtr.Zero ) {
                Marshal.FreeHGlobal( bufferRight );
                bufferRight = IntPtr.Zero;
            }
            if ( buffers != IntPtr.Zero ) {
                Marshal.FreeHGlobal( buffers );
                buffers = IntPtr.Zero;
            }
        }

        public void process( double[] left, double[] right ) {
            if ( left == null || right == null ){
                return;
            }
            int length = Math.Min( left.Length, right.Length );
            try {
                initBuffer();
                int remain = length;
                int offset = 0;
                float* left_ch = (float*)bufferLeft.ToPointer();
                float* right_ch = (float*)bufferRight.ToPointer();
                float** out_buffer = (float**)buffers.ToPointer();
                out_buffer[0] = left_ch;
                out_buffer[1] = right_ch;
                while ( remain > 0 ) {
                    int proc = (remain > BUFLEN) ? BUFLEN : remain;
                    aEffect.ProcessReplacing( ref aEffect, (float**)0, out_buffer, proc );
                    for ( int i = 0; i < proc; i++ ) {
                        left[i + offset] = left_ch[i];
                        right[i + offset] = right_ch[i];
                    }
                    remain -= proc;
                    offset += proc;
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "vstidrv#process; ex=" + ex );
            }
        }

        public void send( MidiEvent[] events ) {
            unsafe {
                GlobalMemoryContext mem_context = null;
                try {
                    mem_context = new GlobalMemoryContext();
                    int nEvents = events.Length;
                    IntPtr ptr_pvst_events = mem_context.malloc( sizeof( VstEvent ) + nEvents * sizeof( VstEvent* ) );
                    VstEvents* pVSTEvents = (VstEvents*)ptr_pvst_events.ToPointer();
                    pVSTEvents->numEvents = 0;
                    pVSTEvents->reserved = (VstIntPtr)0;

                    for ( int i = 0; i < nEvents; i++ ) {
                        MidiEvent pProcessEvent = events[i];
                        byte event_code = pProcessEvent.firstByte;
                        VstEvent* pVSTEvent = (VstEvent*)0;
                        VstMidiEvent* pMidiEvent;
                        pMidiEvent = (VstMidiEvent*)mem_context.malloc( (int)(sizeof( VstMidiEvent ) + (pProcessEvent.data.Length + 1) * sizeof( byte )) ).ToPointer();
                        pMidiEvent->byteSize = sizeof( VstMidiEvent );
                        pMidiEvent->deltaFrames = 0;
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
                    }
                    aEffect.Dispatch( ref aEffect, AEffectXOpcodes.effProcessEvents, 0, 0, pVSTEvents, 0 );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "vstidrv#send; ex=" + ex );
                } finally {
                    if ( mem_context != null ) {
                        try {
                            mem_context.dispose();
                        } catch ( Exception ex2 ) {
                            PortUtil.stderr.println( "vstidrv#send; ex2=" + ex2 );
                        }
                    }
                }
            }
        }

        protected virtual VstIntPtr AudioMaster( AEffect* effect, VstInt32 opcode, VstInt32 index, VstIntPtr value, void* ptr, float opt ) {
            VstIntPtr result = 0;
            switch ( opcode ) {
                case AudioMasterOpcodes.audioMasterVersion:
                    result = Constants.kVstVersion;
                    break;
            }
            return result;
        }

        public virtual bool open( string dll_path, int block_size, int sample_rate ) {
            dllHandle = win32.LoadLibraryExW( dll_path, IntPtr.Zero, win32.LOAD_WITH_ALTERED_SEARCH_PATH );
            System.Threading.Thread.Sleep( 250 );

            mainDelegate = (PVSTMAIN)Marshal.GetDelegateForFunctionPointer( win32.GetProcAddress( dllHandle, "main" ),
                                                                               typeof( PVSTMAIN ) );
            Thread.Sleep( 250 );
            if ( mainDelegate == null ) {
                return false;
            }

            audioMaster = new audioMasterCallback( AudioMaster );
            Thread.Sleep( 250 );

            IntPtr ptr_aeffect = IntPtr.Zero;
            try {
                ptr_aeffect = mainDelegate( audioMaster );
            } catch ( Exception ex ) {
            }
            if ( ptr_aeffect == IntPtr.Zero ) {
                return false;
            }
            blockSize = block_size;
            sampleRate = sample_rate;
            aEffect = (AEffect)Marshal.PtrToStructure( ptr_aeffect, typeof( AEffect ) );
            aEffect.Dispatch( ref aEffect, AEffectOpcodes.effOpen, 0, 0, (void*)0, 0 );
            aEffect.Dispatch( ref aEffect, AEffectOpcodes.effSetSampleRate, 0, 0, (void*)0, (float)sampleRate );
            aEffect.Dispatch( ref aEffect, AEffectOpcodes.effSetBlockSize, 0, blockSize, (void*)0, 0 );
            return true;
        }

        public virtual void close() {
            try {
                aEffect.Dispatch( ref aEffect, AEffectOpcodes.effClose, 0, 0, (void*)0, 0.0f );
                win32.FreeLibrary( dllHandle );
            } catch( Exception ex ){
            }
            releaseBuffer();
            aEffect = new AEffect();
            dllHandle = IntPtr.Zero;
            mainDelegate = null;
            audioMaster = null;
        }
    }

}
#endif
