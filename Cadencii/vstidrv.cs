#if ENABLE_VOCALOID
/*
 * VSTiProxy.cs
 * Copyright (C) 2008-2009 kbinani
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
using System.Text;
using System.Threading;
using org.kbinani;
using org.kbinani.java.util;
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

    public unsafe class MemoryManager {
        private Vector<IntPtr> list = new Vector<IntPtr>();

        public void* malloc( int bytes ) {
            IntPtr ret = Marshal.AllocHGlobal( bytes );
            list.add( ret );
            return ret.ToPointer();
        }

        public void dispose() {
            for ( Iterator itr = list.iterator(); itr.hasNext(); ) {
                IntPtr v = (IntPtr)itr.next();
                Marshal.FreeHGlobal( v );
            }
            list.clear();
        }

        ~MemoryManager() {
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
        /// <summary>
        /// パラメータの，ロード時のデフォルト値
        /// </summary>
        float[] paramDefaults = null;

        public void resetAllParameters() {
            if ( paramDefaults == null ) {
                return;
            }
            for ( int i = 0; i < paramDefaults.Length; i++ ) {
                setParameter( i, paramDefaults[i] );
            }
        }

        public virtual float getParameter( int index ) {
            return aEffect.GetParameter( ref aEffect, index );
        }

        public virtual void setParameter( int index, float value ) {
            aEffect.SetParameter( ref aEffect, index, value );
        }

        private String getStringCore( int opcode, int index, int str_capacity ) {
            byte[] arr = new byte[] { };
            IntPtr ptr = IntPtr.Zero;
            try {
                ptr = Marshal.AllocHGlobal( str_capacity );
                byte* bptr = (byte*)ptr.ToPointer();
                for ( int i = 0; i < str_capacity; i++ ) {
                    bptr[i] = 0;
                }
                aEffect.Dispatch( ref aEffect, opcode, index, 0, bptr, 0.0f );
                arr = new byte[str_capacity];
                for ( int i = 0; i < str_capacity; i++ ) {
                    arr[i] = bptr[i];
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "vstidrv#getStringCore; ex=" + ex );
            } finally {
                if ( ptr != IntPtr.Zero ) {
                    Marshal.FreeHGlobal( ptr );
                }
            }
            String ret = Encoding.ASCII.GetString( arr );
            return ret;
        }

        public String getParameterDisplay( int index ) {
            return getStringCore( AEffectOpcodes.effGetParamDisplay, index, VstStringConstants.kVstMaxParamStrLen );
        }

        public String getParameterLabel( int index ) {
            return getStringCore( AEffectOpcodes.effGetParamLabel, index, VstStringConstants.kVstMaxParamStrLen );
        }

        public String getParameterName( int index ) {
            return getStringCore( AEffectOpcodes.effGetParamName, index, VstStringConstants.kVstMaxParamStrLen );
        }

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

        public virtual void send( MidiEvent[] events ) {
            unsafe {
                MemoryManager mman = null;
                try {
                    mman = new MemoryManager();
                    int nEvents = events.Length;
                    VstEvents* pVSTEvents = (VstEvents*)mman.malloc( sizeof( VstEvent ) + nEvents * sizeof( VstEvent* ) );
                    pVSTEvents->numEvents = 0;
                    pVSTEvents->reserved = (VstIntPtr)0;

                    for ( int i = 0; i < nEvents; i++ ) {
                        MidiEvent pProcessEvent = events[i];
                        byte event_code = pProcessEvent.firstByte;
                        VstEvent* pVSTEvent = (VstEvent*)0;
                        VstMidiEvent* pMidiEvent;
                        pMidiEvent = (VstMidiEvent*)mman.malloc( (int)(sizeof( VstMidiEvent ) + (pProcessEvent.data.Length + 1) * sizeof( byte )) );
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
                    if ( mman != null ) {
                        try {
                            mman.dispose();
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
            Thread.Sleep( 100 );
            if ( dllHandle == IntPtr.Zero ) {
                return false;
            }

            mainDelegate = (PVSTMAIN)Marshal.GetDelegateForFunctionPointer( win32.GetProcAddress( dllHandle, "main" ),
                                                                            typeof( PVSTMAIN ) );
            Thread.Sleep( 100 );
            if ( mainDelegate == null ) {
                return false;
            }

            audioMaster = new audioMasterCallback( AudioMaster );
            Thread.Sleep( 100 );

            IntPtr ptr_aeffect = IntPtr.Zero;
            try {
                ptr_aeffect = mainDelegate( audioMaster );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "vstidrv#open; ex=" + ex );
                return false;
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

            // デフォルトのパラメータ値を取得
            int num = aEffect.numParams;
            paramDefaults = new float[num];
            for ( int i = 0; i < num; i++ ) {
                paramDefaults[i] = aEffect.GetParameter( ref aEffect, i );
            }

            // TODO: Editorを持っているかどうかを確認
            /*const char* plugCanDos [] =
{
	"sendVstEvents",
	"sendVstMidiEvent",
	"sendVstTimeInfo",
	"receiveVstEvents",
	"receiveVstMidiEvent",
	"receiveVstTimeInfo",
	"offline",
	"plugAsChannelInsert",
	"plugAsSend",
	"mixDryWet",
	"noRealTime",
	"multipass",
	"metapass",
	"1in1out",
	"1in2out",
	"2in1out",
	"2in2out",
	"2in4out",
	"4in2out",
	"4in4out",
	"4in8out",	// 4:2 matrix to surround bus
	"8in4out",	// surround bus to 4:2 matrix
	"8in8out"
#if VST_2_1_EXTENSIONS
	,
	"midiProgramNames",
	"conformsToWindowRules"		// mac: doesn't mess with grafport. general: may want
								// to call sizeWindow (). if you want to use sizeWindow (),
								// you must return true (1) in canDo ("conformsToWindowRules")
#endif // VST_2_1_EXTENSIONS

#if VST_2_3_EXTENSIONS
	,
	"bypass"
#endif // VST_2_3_EXTENSIONS
};
*/
            //aEffect.Dispatch( ref aEffect, AEffectXOpcodes.effCanDo,
            return true;
        }

        public virtual void close() {
            try {
                aEffect.Dispatch( ref aEffect, AEffectOpcodes.effClose, 0, 0, (void*)0, 0.0f );
                win32.FreeLibrary( dllHandle );
            } catch( Exception ex ){
                PortUtil.stderr.println( "vstidrv#close; ex=" + ex );
            }
            releaseBuffer();
            aEffect = new AEffect();
            dllHandle = IntPtr.Zero;
            mainDelegate = null;
            audioMaster = null;
        }

        ~vstidrv() {
            close();
        }
    }

}
#endif
