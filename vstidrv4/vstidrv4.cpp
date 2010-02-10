#include <windows.h>
#include <stdio.h>
#include "pluginterfaces/vst2.x/aeffectx.h"

typedef AEffect* (*PVSTMAIN)( audioMasterCallback audioMaster );

AEffect *aeffect = NULL;
HANDLE mapping = NULL;

VstIntPtr AudioMaster( AEffect* effect, VstInt32 opcode, VstInt32 index, VstIntPtr value, void* ptr, float opt ){
    VstIntPtr result = 0;

    switch( opcode ){
        case audioMasterVersion:{
            result = kVstVersion;
            break;
        }
    }
    return result;
}

int main( int argc, char *argv[] ){
    if( argc < 3 ){
        return 0;
    }
    char *str_obj_name = argv[1]; // マッピングオブジェクトの名前
    char *str_vsti_dll = argv[2]; // VSTi DLLへのパス

    // VSTiの読み込み
    HMODULE module = LoadLibraryExA( str_vsti_dll, NULL, LOAD_WITH_ALTERED_SEARCH_PATH );
    if( NULL == module ){
#ifdef _DEBUG
        printf( "error; module is NULL\n" );
#endif
        return 0;
    }
#ifdef _DEBUG
    printf( "module=0x%X\n", module );
#endif
    PVSTMAIN main_entry = (PVSTMAIN)GetProcAddress( module, "main" );
    if( NULL == main_entry ){
#ifdef _DEBUG
        printf( "error; main_entry is NULL\n" );
#endif
        return 0;
    }
    aeffect = main_entry( AudioMaster );

    // マッピングオブジェクトを取得
    mapping = OpenFileMapping( FILE_MAP_ALL_ACCESS, FALSE, str_obj_name );
    if( NULL == mapping ){
#ifdef _DEBUG
        printf( "error; mapping is NULL\n" );
#endif
        return 0;
    }
    char* ptr = (char*)MapViewOfFile( mapping, FILE_MAP_ALL_ACCESS, 0, 0, 0 );
    while( 1 ){
        Sleep( 10 );
        char code = ptr[0];
        printf( "\rcode=%d", code );
    }
    UnmapViewOfFile( ptr );
    CloseHandle( mapping );
}

/*
    public class AEffectWrapper {
        public AEffect aeffect;

        private AEffectDispatcherProc dispatcherProc = null;
        private AEffectProcessProc processProc = null;
        private AEffectSetParameterProc setParameterProc = null;
        private AEffectGetParameterProc getParameterProc = null;
        private AEffectProcessProc processReplacingProc = null;
        private AEffectProcessDoubleProc processDoubleReplacingProc = null;

        /// <summary>
        /// Host to Plug-in dispatcher @see AudioEffect::dispatcher
        /// </summary>
        public VstIntPtr Dispatch( VstInt32 opcode, VstInt32 index, VstIntPtr value, IntPtr ptr, float opt ) {
            if ( dispatcherProc == null && aeffect.dispatcher != IntPtr.Zero ) {
                dispatcherProc = (AEffectDispatcherProc)Marshal.GetDelegateForFunctionPointer( aeffect.dispatcher, typeof( AEffectDispatcherProc ) );
            }
            VstIntPtr ret = 0;
            try {
                if ( dispatcherProc != null ) {
                    ret = dispatcherProc( ref aeffect, opcode, index, value, ptr, opt );
                }
            } catch ( Exception ex ) {
                Console.Error.WriteLine( "AEffectWrapper#Dispatch; ex=" + ex );
            }
            return ret;
        }

        /// <summary>
        /// deprecated Accumulating process mode is deprecated in VST 2.4! Use AEffect::processReplacing instead!
        /// </summary>
        public void __ProcessDeprecated( IntPtr inputs, IntPtr outputs, VstInt32 sampleFrames ) {
            if ( processProc == null && aeffect.__processDeprecated != IntPtr.Zero ) {
                processProc = (AEffectProcessProc)Marshal.GetDelegateForFunctionPointer( aeffect.__processDeprecated, typeof( AEffectProcessProc ) );
            }
            try {
                if ( processProc != null ) {
                    processProc( ref aeffect, inputs, outputs, sampleFrames );
                }
            } catch ( Exception ex ) {
                Console.Error.WriteLine( "AEffect#__ProcessDeprecated; ex=" + ex );
            }
        }

        /// <summary>
        /// Set new value of automatable parameter @see AudioEffect::setParameter
        /// </summary>
        public void SetParameter( VstInt32 index, float parameter ) {
            if ( setParameterProc == null && aeffect.setParameter != IntPtr.Zero ) {
                setParameterProc = (AEffectSetParameterProc)Marshal.GetDelegateForFunctionPointer( aeffect.setParameter, typeof( AEffectSetParameterProc ) );
            }
            try {
                if ( setParameterProc != null ) {
                    setParameterProc( ref aeffect, index, parameter );
                }
            } catch ( Exception ex ) {
                Console.Error.WriteLine( "AEffect#SetParameter; ex=" + ex );
            }
        }

        /// <summary>
        /// Returns current value of automatable parameter @see AudioEffect::getParameter
        /// </summary>
        public float GetParameter( VstInt32 index ) {
            if ( getParameterProc == null && aeffect.getParameter != IntPtr.Zero ) {
                getParameterProc = (AEffectGetParameterProc)Marshal.GetDelegateForFunctionPointer( aeffect.getParameter, typeof( AEffectGetParameterProc ) );
            }
            float ret = 0.0f;
            try {
                if ( getParameterProc != null ) {
                    ret = getParameterProc( ref aeffect, index );
                }
            } catch ( Exception ex ) {
                Console.Error.WriteLine( "AEffect#GetParameter; ex=" + ex );
            }
            return ret;
        }

        /// <summary>
        /// Process audio samples in replacing mode @see AudioEffect::processReplacing
        /// </summary>
        public void ProcessReplacing( IntPtr inputs, IntPtr outputs, VstInt32 sampleFrames ) {
            if ( processReplacingProc == null && aeffect.processReplacing != IntPtr.Zero ) {
                processReplacingProc = (AEffectProcessProc)Marshal.GetDelegateForFunctionPointer( aeffect.processReplacing, typeof( AEffectProcessProc ) );
            }
            try {
                if ( processReplacingProc != null ) {
                    processReplacingProc( ref aeffect, inputs, outputs, sampleFrames );
                }
            } catch ( Exception ex ) {
                Console.Error.WriteLine( "AEffect#ProcessReplacing; ex=" + ex );
            }
        }

#if VST_2_4_EXTENSIONS
        /// <summary>
        /// Process double-precision audio samples in replacing mode @see AudioEffect::processDoubleReplacing
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <param name="sampleFrames"></param>
        public void ProcessDoubleReplacing( IntPtr inputs, IntPtr outputs, VstInt32 sampleFrames ) {
            if ( processDoubleReplacingProc == null && aeffect.processDoubleReplacing != IntPtr.Zero ) {
                processDoubleReplacingProc = (AEffectProcessDoubleProc)Marshal.GetDelegateForFunctionPointer( aeffect.processDoubleReplacing, typeof( AEffectProcessDoubleProc ) );
            }
            try {
                if ( processDoubleReplacingProc != null ) {
                    processDoubleReplacingProc( ref aeffect, inputs, outputs, sampleFrames );
                }
            } catch ( Exception ex ) {
                Console.Error.WriteLine( "AEffect#ProcessDoubleReplacing; ex=" + ex );
            }
        }
#endif
    }
*/
