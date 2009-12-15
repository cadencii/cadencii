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
using System.Threading;
using bocoree;
using bocoree.java.util;
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
            aEffect = new AEffect();
            dllHandle = IntPtr.Zero;
            mainDelegate = null;
            audioMaster = null;
        }
    }

}
#endif
