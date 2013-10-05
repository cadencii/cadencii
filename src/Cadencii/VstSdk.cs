#define VST_2_4_EXTENSIONS

using System;
using System.Runtime.InteropServices;

namespace VstSdk
{
    #region VST SDK 2.4 declarations
    using VstInt32 = System.Int32;
    using VstIntPtr = System.Int32;
    using VstInt16 = System.Int16;


    /// <summary>
    /// 定数値のコンテナクラス
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// VST SDKのバージョンを表す定数
        /// </summary>
        public const int kVstVersion = 2400;
    }

    /// <summary>
    /// String length limits (in characters excl. 0 byte)
    /// </summary>
    public static class VstStringConstants
    {
        /// <summary>
        /// used for #effGetProgramName, #effSetProgramName, #effGetProgramNameIndexed
        /// </summary>
        public const int kVstMaxProgNameLen = 24;
        /// <summary>
        /// used for #effGetParamLabel, #effGetParamDisplay, #effGetParamName
        /// </summary>
        public const int kVstMaxParamStrLen = 8;
        /// <summary>
        /// used for #effGetVendorString, #audioMasterGetVendorString
        /// </summary>
        public const int kVstMaxVendorStrLen = 64;
        /// <summary>
        /// used for #effGetProductString, #audioMasterGetProductString
        /// </summary>
        public const int kVstMaxProductStrLen = 64;
        /// <summary>
        /// used for #effGetEffectName
        /// </summary>
        public const int kVstMaxEffectNameLen = 32;
    }

    public static class AudioMasterOpcodes
    {
        /// <summary>
        /// [index]: parameter index [opt]: parameter value  @see AudioEffect::setParameterAutomated
        /// </summary>
        public const int audioMasterAutomate = 0;
        /// <summary>
        /// [return value]: Host VST version (for example 2400 for VST 2.4) @see AudioEffect::getMasterVersion
        /// </summary>
        public const int audioMasterVersion = 1;
        /// <summary>
        /// [return value]: current unique identifier on shell plug-in  @see AudioEffect::getCurrentUniqueId
        /// </summary>
        public const int audioMasterCurrentId = 2;
        /// <summary>
        /// no arguments  @see AudioEffect::masterIdle
        /// </summary>
        public const int audioMasterIdle = 3;
        /// <summary>
        /// deprecated in VST 2.4 r2
        /// </summary>
        [Obsolete]
        public const int __audioMasterPinConnectedDeprecated = 4;
    }

    /// <summary>
    /// Basic dispatcher Opcodes (Host to Plug-in)
    /// </summary>
    public static class AEffectOpcodes
    {
        /// <summary>
        /// no arguments  @see AudioEffect::open
        /// </summary>
        public const int effOpen = 0;
        /// <summary>
        /// no arguments  @see AudioEffect::close
        /// </summary>
        public const int effClose = 1;

        /// <summary>
        /// [value]: new program number  @see AudioEffect::setProgram
        /// </summary>
        public const int effSetProgram = 2;
        /// <summary>
        /// [return value]: current program number  @see AudioEffect::getProgram
        /// </summary>
        public const int effGetProgram = 3;
        /// <summary>
        /// [ptr]: char* with new program name, limited to #kVstMaxProgNameLen  @see AudioEffect::setProgramName
        /// </summary>
        public const int effSetProgramName = 4;
        /// <summary>
        /// [ptr]: char buffer for current program name, limited to #kVstMaxProgNameLen  @see AudioEffect::getProgramName
        /// </summary>
        public const int effGetProgramName = 5;

        /// <summary>
        /// [ptr]: char buffer for parameter label, limited to #kVstMaxParamStrLen  @see AudioEffect::getParameterLabel
        /// </summary>
        public const int effGetParamLabel = 6;
        /// <summary>
        /// [ptr]: char buffer for parameter display, limited to #kVstMaxParamStrLen  @see AudioEffect::getParameterDisplay
        /// </summary>
        public const int effGetParamDisplay = 7;
        /// <summary>
        /// [ptr]: char buffer for parameter name, limited to #kVstMaxParamStrLen  @see AudioEffect::getParameterName
        /// </summary>
        public const int effGetParamName = 8;

        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effGetVuDeprecated = 9;

        /// <summary>
        /// [opt]: new sample rate for audio processing  @see AudioEffect::setSampleRate
        /// </summary>
        public const int effSetSampleRate = 10;
        /// <summary>
        /// [value]: new maximum block size for audio processing  @see AudioEffect::setBlockSize
        /// </summary>
        public const int effSetBlockSize = 11;
        /// <summary>
        /// [value]: 0 means "turn off", 1 means "turn on"  @see AudioEffect::suspend @see AudioEffect::resume
        /// </summary>
        public const int effMainsChanged = 12;

        /// <summary>
        /// [ptr]: #ERect** receiving pointer to editor size  @see ERect @see AEffEditor::getRect
        /// </summary>
        public const int effEditGetRect = 13;
        /// <summary>
        /// [ptr]: system dependent Window pointer, e.g. HWND on Windows  @see AEffEditor::open
        /// </summary>
        public const int effEditOpen = 14;
        /// <summary>
        /// no arguments @see AEffEditor::close
        /// </summary>
        public const int effEditClose = 15;

        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effEditDrawDeprecated = 16;
        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effEditMouseDeprecated = 17;
        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effEditKeyDeprecated = 18;

        /// <summary>
        /// no arguments @see AEffEditor::idle
        /// </summary>
        public const int effEditIdle = 19;

        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effEditTopDeprecated = 20;
        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effEditSleepDeprecated = 21;
        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effIdentifyDeprecated = 22;

        /// <summary>
        /// [ptr]: void** for chunk data address [index]: 0 for bank, 1 for program  @see AudioEffect::getChunk
        /// </summary>
        public const int effGetChunk = 23;
        /// <summary>
        /// [ptr]: chunk data [value]: byte size [index]: 0 for bank, 1 for program  @see AudioEffect::setChunk
        /// </summary>
        public const int effSetChunk = 24;

        public const int effNumOpcodes = 25;
    }

    public static class AEffectXOpcodes
    {
        /// <summary>
        /// [ptr]: #VstEvents*  @see AudioEffectX::processEvents
        /// </summary>
        public const int effProcessEvents = 25;

        /// <summary>
        /// [index]: parameter index [return value]: 1=true, 0=false  @see AudioEffectX::canParameterBeAutomated
        /// </summary>
        public const int effCanBeAutomated = 26;
        /// <summary>
        /// [index]: parameter index [ptr]: parameter String [return value]: true for success  @see AudioEffectX::string2parameter
        /// </summary>
        public const int effString2Parameter = 27;

        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effGetNumProgramCategoriesDeprecated = 28;

        /// <summary>
        /// [index]: program index [ptr]: buffer for program name, limited to #kVstMaxProgNameLen [return value]: true for success  @see AudioEffectX::getProgramNameIndexed
        /// </summary>
        public const int effGetProgramNameIndexed = 29;

        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effCopyProgramDeprecated = 30;
        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effConnectInputDeprecated = 31;
        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effConnectOutputDeprecated = 32;

        /// <summary>
        /// [index]: input index [ptr]: #VstPinProperties* [return value]: 1 if supported  @see AudioEffectX::getInputProperties
        /// </summary>
        public const int effGetInputProperties = 33;
        /// <summary>
        /// [index]: output index [ptr]: #VstPinProperties* [return value]: 1 if supported  @see AudioEffectX::getOutputProperties
        /// </summary>
        public const int effGetOutputProperties = 34;
        /// <summary>
        /// [return value]: category  @see VstPlugCategory @see AudioEffectX::getPlugCategory
        /// </summary>
        public const int effGetPlugCategory = 35;

        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effGetCurrentPositionDeprecated = 36;
        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effGetDestinationBufferDeprecated = 37;

        /// <summary>
        /// [ptr]: #VstAudioFile array [value]: count [index]: start flag  @see AudioEffectX::offlineNotify
        /// </summary>
        public const int effOfflineNotify = 38;
        /// <summary>
        /// [ptr]: #VstOfflineTask array [value]: count  @see AudioEffectX::offlinePrepare
        /// </summary>
        public const int effOfflinePrepare = 39;
        /// <summary>
        /// [ptr]: #VstOfflineTask array [value]: count  @see AudioEffectX::offlineRun
        /// </summary>
        public const int effOfflineRun = 40;

        /// <summary>
        /// [ptr]: #VstVariableIo*  @see AudioEffectX::processVariableIo
        /// </summary>
        public const int effProcessVarIo = 41;
        /// <summary>
        /// [value]: input #VstSpeakerArrangement* [ptr]: output #VstSpeakerArrangement*  @see AudioEffectX::setSpeakerArrangement
        /// </summary>
        public const int effSetSpeakerArrangement = 42;

        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effSetBlockSizeAndSampleRateDeprecated = 43;

        /// <summary>
        /// [value]: 1 = bypass, 0 = no bypass  @see AudioEffectX::setBypass
        /// </summary>
        public const int effSetBypass = 44;
        /// <summary>
        /// [ptr]: buffer for effect name, limited to #kVstMaxEffectNameLen  @see AudioEffectX::getEffectName
        /// </summary>
        public const int effGetEffectName = 45;

        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effGetErrorTextDeprecated = 46;

        /// <summary>
        /// [ptr]: buffer for effect vendor String, limited to #kVstMaxVendorStrLen  @see AudioEffectX::getVendorString
        /// </summary>
        public const int effGetVendorString = 47;
        /// <summary>
        /// [ptr]: buffer for effect vendor String, limited to #kVstMaxProductStrLen  @see AudioEffectX::getProductString
        /// </summary>
        public const int effGetProductString = 48;
        /// <summary>
        /// [return value]: vendor-specific version  @see AudioEffectX::getVendorVersion
        /// </summary>
        public const int effGetVendorVersion = 49;
        /// <summary>
        /// no definition, vendor specific handling  @see AudioEffectX::vendorSpecific
        /// </summary>
        public const int effVendorSpecific = 50;
        /// <summary>
        /// [ptr]: "can do" String [return value]: 0: "don't know" -1: "no" 1: "yes"  @see AudioEffectX::canDo
        /// </summary>
        public const int effCanDo = 51;
        /// <summary>
        /// [return value]: tail size (for example the reverb time of a reverb plug-in); 0 is default (return 1 for 'no tail')
        /// </summary>
        public const int effGetTailSize = 52;

        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effIdleDeprecated = 53;
        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effGetIconDeprecated = 54;
        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effSetViewPositionDeprecated = 55;

        /// <summary>
        /// [index]: parameter index [ptr]: #VstParameterProperties* [return value]: 1 if supported  @see AudioEffectX::getParameterProperties
        /// </summary>
        public const int effGetParameterProperties = 56;

        /// <summary>
        /// deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effKeysRequiredDeprecated = 57;

        /// <summary>
        /// [return value]: VST version  @see AudioEffectX::getVstVersion
        /// </summary>
        public const int effGetVstVersion = 58;

        /// <summary>
        /// [index]: ASCII character [value]: virtual key [opt]: modifiers [return value]: 1 if key used  @see AEffEditor::onKeyDown
        /// </summary>
        public const int effEditKeyDown = 59;
        /// <summary>
        /// [index]: ASCII character [value]: virtual key [opt]: modifiers [return value]: 1 if key used  @see AEffEditor::onKeyUp
        /// </summary>
        public const int effEditKeyUp = 60;
        /// <summary>
        /// [value]: knob mode 0: circular, 1: circular relativ, 2: linear (CKnobMode in VSTGUI)  @see AEffEditor::setKnobMode
        /// </summary>
        public const int effSetEditKnobMode = 61;

        /// <summary>
        /// [index]: MIDI channel [ptr]: #MidiProgramName* [return value]: number of used programs, 0 if unsupported  @see AudioEffectX::getMidiProgramName
        /// </summary>
        public const int effGetMidiProgramName = 62;
        /// <summary>
        /// [index]: MIDI channel [ptr]: #MidiProgramName* [return value]: index of current program  @see AudioEffectX::getCurrentMidiProgram
        /// </summary>
        public const int effGetCurrentMidiProgram = 63;
        /// <summary>
        /// [index]: MIDI channel [ptr]: #MidiProgramCategory* [return value]: number of used categories, 0 if unsupported  @see AudioEffectX::getMidiProgramCategory
        /// </summary>
        public const int effGetMidiProgramCategory = 64;
        /// <summary>
        /// [index]: MIDI channel [return value]: 1 if the #MidiProgramName(s) or #MidiKeyName(s) have changed  @see AudioEffectX::hasMidiProgramsChanged
        /// </summary>
        public const int effHasMidiProgramsChanged = 65;
        /// <summary>
        /// [index]: MIDI channel [ptr]: #MidiKeyName* [return value]: true if supported, false otherwise  @see AudioEffectX::getMidiKeyName
        /// </summary>
        public const int effGetMidiKeyName = 66;

        /// <summary>
        /// no arguments  @see AudioEffectX::beginSetProgram
        /// </summary>
        public const int effBeginSetProgram = 67;
        /// <summary>
        /// no arguments  @see AudioEffectX::endSetProgram
        /// </summary>
        public const int effEndSetProgram = 68;
        /// <summary>
        /// [value]: input #VstSpeakerArrangement* [ptr]: output #VstSpeakerArrangement*  @see AudioEffectX::getSpeakerArrangement
        /// </summary>
        public const int effGetSpeakerArrangement = 69;
        /// <summary>
        /// [ptr]: buffer for plug-in name, limited to #kVstMaxProductStrLen [return value]: next plugin's uniqueID  @see AudioEffectX::getNextShellPlugin
        /// </summary>
        public const int effShellGetNextPlugin = 70;

        /// <summary>
        /// no arguments  @see AudioEffectX::startProcess
        /// </summary>
        public const int effStartProcess = 71;
        /// <summary>
        /// no arguments  @see AudioEffectX::stopProcess
        /// </summary>
        public const int effStopProcess = 72;
        /// <summary>
        /// [value]: number of samples to process, offline only!  @see AudioEffectX::setTotalSampleToProcess
        /// </summary>
        public const int effSetTotalSampleToProcess = 73;
        /// <summary>
        /// [value]: pan law [opt]: gain  @see VstPanLawType @see AudioEffectX::setPanLaw
        /// </summary>
        public const int effSetPanLaw = 74;

        /// <summary>
        /// [ptr]: #VstPatchChunkInfo* [return value]: -1: bank can't be loaded, 1: bank can be loaded, 0: unsupported  @see AudioEffectX::beginLoadBank
        /// </summary>
        public const int effBeginLoadBank = 75;
        /// <summary>
        /// [ptr]: #VstPatchChunkInfo* [return value]: -1: prog can't be loaded, 1: prog can be loaded, 0: unsupported  @see AudioEffectX::beginLoadProgram
        /// </summary>
        public const int effBeginLoadProgram = 76;
        /// <summary>
        /// [value]: @see VstProcessPrecision  @see AudioEffectX::setProcessPrecision
        /// </summary>
        public const int effSetProcessPrecision = 77;
        /// <summary>
        /// [return value]: number of used MIDI input channels (1-15)  @see AudioEffectX::getNumMidiInputChannels
        /// </summary>
        public const int effGetNumMidiInputChannels = 78;
        /// <summary>
        /// [return value]: number of used MIDI output channels (1-15)  @see AudioEffectX::getNumMidiOutputChannels
        /// </summary>
        public const int effGetNumMidiOutputChannels = 79;
    }

    public static class VstAEffectFlags
    {
        /// <summary>
        /// set if the plug-in provides a custom editor
        /// </summary>
        public const int effFlagsHasEditor = 1 << 0;
        /// <summary>
        /// supports replacing process mode (which should the default mode in VST 2.4)
        /// </summary>
        public const int effFlagsCanReplacing = 1 << 4;
        /// <summary>
        /// program data is handled in formatless chunks
        /// </summary>
        public const int effFlagsProgramChunks = 1 << 5;
        /// <summary>
        /// plug-in is a synth (VSTi), Host may assign mixer channels for its outputs
        /// </summary>
        public const int effFlagsIsSynth = 1 << 8;
        /// <summary>
        /// plug-in does not produce sound when input is all silence
        /// </summary>
        public const int effFlagsNoSoundInStop = 1 << 9;
#if VST_2_4_EXTENSIONS
        /// <summary>
        /// plug-in supports double precision processing
        /// </summary>
        public const int effFlagsCanDoubleReplacing = 1 << 12;
#endif
        /// <summary>
        /// \deprecated deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effFlagsHasClipDeprecated = 1 << 1;
        /// <summary>
        /// \deprecated deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effFlagsHasVuDeprecated = 1 << 2;
        /// <summary>
        /// \deprecated deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effFlagsCanMonoDeprecated = 1 << 3;
        /// <summary>
        /// \deprecated deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effFlagsExtIsAsyncDeprecated = 1 << 10;
        /// <summary>
        /// \deprecated deprecated in VST 2.4
        /// </summary>
        [Obsolete]
        public const int __effFlagsExtHasBufferDeprecated = 1 << 11;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate VstIntPtr AEffectDispatcherProc(ref AEffect effect, VstInt32 opcode, VstInt32 index, VstIntPtr value, IntPtr ptr, float opt);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void AEffectProcessProc(ref AEffect effect, IntPtr inputs, IntPtr outputs, VstInt32 sampleFrames);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void AEffectProcessDoubleProc(ref AEffect effect, IntPtr inputs, IntPtr outputs, VstInt32 sampleFrames);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void AEffectSetParameterProc(ref AEffect effect, VstInt32 index, float parameter);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate float AEffectGetParameterProc(ref AEffect effect, VstInt32 index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate VstIntPtr audioMasterCallback(ref AEffect effect, VstInt32 opcode, VstInt32 index, VstIntPtr value, IntPtr ptr, float opt);

    public class AEffectWrapper
    {
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
        public VstIntPtr Dispatch(VstInt32 opcode, VstInt32 index, VstIntPtr value, IntPtr ptr, float opt)
        {
            if (dispatcherProc == null && aeffect.dispatcher != IntPtr.Zero) {
                dispatcherProc = (AEffectDispatcherProc)Marshal.GetDelegateForFunctionPointer(aeffect.dispatcher, typeof(AEffectDispatcherProc));
            }
            VstIntPtr ret = 0;
            try {
                if (dispatcherProc != null) {
                    ret = dispatcherProc(ref aeffect, opcode, index, value, ptr, opt);
                }
            } catch (Exception ex) {
                Console.Error.WriteLine("AEffectWrapper#Dispatch; ex=" + ex);
            }
            return ret;
        }

        /// <summary>
        /// deprecated Accumulating process mode is deprecated in VST 2.4! Use AEffect::processReplacing instead!
        /// </summary>
        public void __ProcessDeprecated(IntPtr inputs, IntPtr outputs, VstInt32 sampleFrames)
        {
            if (processProc == null && aeffect.__processDeprecated != IntPtr.Zero) {
                processProc = (AEffectProcessProc)Marshal.GetDelegateForFunctionPointer(aeffect.__processDeprecated, typeof(AEffectProcessProc));
            }
            try {
                if (processProc != null) {
                    processProc(ref aeffect, inputs, outputs, sampleFrames);
                }
            } catch (Exception ex) {
                Console.Error.WriteLine("AEffect#__ProcessDeprecated; ex=" + ex);
            }
        }

        /// <summary>
        /// Set new value of automatable parameter @see AudioEffect::setParameter
        /// </summary>
        public void SetParameter(VstInt32 index, float parameter)
        {
            if (setParameterProc == null && aeffect.setParameter != IntPtr.Zero) {
                setParameterProc = (AEffectSetParameterProc)Marshal.GetDelegateForFunctionPointer(aeffect.setParameter, typeof(AEffectSetParameterProc));
            }
            try {
                if (setParameterProc != null) {
                    setParameterProc(ref aeffect, index, parameter);
                }
            } catch (Exception ex) {
                Console.Error.WriteLine("AEffect#SetParameter; ex=" + ex);
            }
        }

        /// <summary>
        /// Returns current value of automatable parameter @see AudioEffect::getParameter
        /// </summary>
        public float GetParameter(VstInt32 index)
        {
            if (getParameterProc == null && aeffect.getParameter != IntPtr.Zero) {
                getParameterProc = (AEffectGetParameterProc)Marshal.GetDelegateForFunctionPointer(aeffect.getParameter, typeof(AEffectGetParameterProc));
            }
            float ret = 0.0f;
            try {
                if (getParameterProc != null) {
                    ret = getParameterProc(ref aeffect, index);
                }
            } catch (Exception ex) {
                Console.Error.WriteLine("AEffect#GetParameter; ex=" + ex);
            }
            return ret;
        }

        /// <summary>
        /// Process audio samples in replacing mode @see AudioEffect::processReplacing
        /// </summary>
        public void ProcessReplacing(IntPtr inputs, IntPtr outputs, VstInt32 sampleFrames)
        {
            if (processReplacingProc == null && aeffect.processReplacing != IntPtr.Zero) {
                processReplacingProc = (AEffectProcessProc)Marshal.GetDelegateForFunctionPointer(aeffect.processReplacing, typeof(AEffectProcessProc));
            }
            try {
                if (processReplacingProc != null) {
                    processReplacingProc(ref aeffect, inputs, outputs, sampleFrames);
                }
            } catch (Exception ex) {
                Console.Error.WriteLine("AEffect#ProcessReplacing; ex=" + ex);
            }
        }

#if VST_2_4_EXTENSIONS
        /// <summary>
        /// Process double-precision audio samples in replacing mode @see AudioEffect::processDoubleReplacing
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <param name="sampleFrames"></param>
        public void ProcessDoubleReplacing(IntPtr inputs, IntPtr outputs, VstInt32 sampleFrames)
        {
            if (processDoubleReplacingProc == null && aeffect.processDoubleReplacing != IntPtr.Zero) {
                processDoubleReplacingProc = (AEffectProcessDoubleProc)Marshal.GetDelegateForFunctionPointer(aeffect.processDoubleReplacing, typeof(AEffectProcessDoubleProc));
            }
            try {
                if (processDoubleReplacingProc != null) {
                    processDoubleReplacingProc(ref aeffect, inputs, outputs, sampleFrames);
                }
            } catch (Exception ex) {
                Console.Error.WriteLine("AEffect#ProcessDoubleReplacing; ex=" + ex);
            }
        }
#endif
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AEffect
    {
        /// <summary>
        /// must be #kEffectMagic ('VstP')
        /// </summary>
        public VstInt32 magic;
        public IntPtr dispatcher;
        public IntPtr __processDeprecated;
        public IntPtr setParameter;
        public IntPtr getParameter;
        /// <summary>
        /// number of programs
        /// </summary>
        public VstInt32 numPrograms;
        /// <summary>
        /// all programs are assumed to have numParams parameters
        /// </summary>
        public VstInt32 numParams;
        /// <summary>
        /// number of audio inputs
        /// </summary>
        public VstInt32 numInputs;
        /// <summary>
        /// number of audio outputs
        /// </summary>
        public VstInt32 numOutputs;

        /// <summary>
        /// @see VstAEffectFlags
        /// </summary>
        public VstInt32 flags;

        /// <summary>
        /// reserved for Host, must be 0
        /// </summary>
        public VstIntPtr resvd1;
        /// <summary>
        /// reserved for Host, must be 0
        /// </summary>
        public VstIntPtr resvd2;

        /// <summary>
        /// for algorithms which need input in the first place (Group delay or latency in Samples). This value should be initialized in a resume state.
        /// </summary>
        public VstInt32 initialDelay;

        /// <summary>
        /// unused member
        /// </summary>
        [Obsolete]
        public VstInt32 __realQualitiesDeprecated;
        /// <summary>
        /// unused member
        /// </summary>
        [Obsolete]
        public VstInt32 __offQualitiesDeprecated;
        /// <summary>
        /// unused member
        /// </summary>
        [Obsolete]
        public float __ioRatioDeprecated;

        /// <summary>
        /// #AudioEffect class pointer
        /// </summary>
        public IntPtr obj;
        /// <summary>
        /// user-defined pointer
        /// </summary>
        public IntPtr user;

        /// <summary>
        /// registered unique identifier (register it at Steinberg 3rd party support Web). This is used to identify a plug-in during save+load of preset and project.
        /// </summary>
        public VstInt32 uniqueID;
        /// <summary>
        /// plug-in version (example 1100 for version 1.1.0.0)
        /// </summary>
        public VstInt32 version;

        public IntPtr processReplacing;

#if VST_2_4_EXTENSIONS
        public IntPtr processDoubleReplacing;

        /// <summary>
        /// reserved for future use (please zero)
        /// </summary>
        public byte future01;
        public byte future02;
        public byte future03;
        public byte future04;
        public byte future05;
        public byte future06;
        public byte future07;
        public byte future08;
        public byte future09;
        public byte future10;
        public byte future11;
        public byte future12;
        public byte future13;
        public byte future14;
        public byte future15;
        public byte future16;
        public byte future17;
        public byte future18;
        public byte future19;
        public byte future20;
        public byte future21;
        public byte future22;
        public byte future23;
        public byte future24;
        public byte future25;
        public byte future26;
        public byte future27;
        public byte future28;
        public byte future29;
        public byte future30;
        public byte future31;
        public byte future32;
        public byte future33;
        public byte future34;
        public byte future35;
        public byte future36;
        public byte future37;
        public byte future38;
        public byte future39;
        public byte future40;
        public byte future41;
        public byte future42;
        public byte future43;
        public byte future44;
        public byte future45;
        public byte future46;
        public byte future47;
        public byte future48;
        public byte future49;
        public byte future50;
        public byte future51;
        public byte future52;
        public byte future53;
        public byte future54;
        public byte future55;
        public byte future56;
#else
        /// <summary>
        /// reserved for future use (please zero)
        /// </summary>
        public byte future01;
        public byte future02;
        public byte future03;
        public byte future04;
        public byte future05;
        public byte future06;
        public byte future07;
        public byte future08;
        public byte future09;
        public byte future10;
        public byte future11;
        public byte future12;
        public byte future13;
        public byte future14;
        public byte future15;
        public byte future16;
        public byte future17;
        public byte future18;
        public byte future19;
        public byte future20;
        public byte future21;
        public byte future22;
        public byte future23;
        public byte future24;
        public byte future25;
        public byte future26;
        public byte future27;
        public byte future28;
        public byte future29;
        public byte future30;
        public byte future31;
        public byte future32;
        public byte future33;
        public byte future34;
        public byte future35;
        public byte future36;
        public byte future37;
        public byte future38;
        public byte future39;
        public byte future40;
        public byte future41;
        public byte future42;
        public byte future43;
        public byte future44;
        public byte future45;
        public byte future46;
        public byte future47;
        public byte future48;
        public byte future49;
        public byte future50;
        public byte future51;
        public byte future52;
        public byte future53;
        public byte future54;
        public byte future55;
        public byte future56;
        public byte future57;
        public byte future58;
        public byte future59;
        public byte future60;
#endif
    }

    /// <summary>
    /// A generic timestamped event.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct VstEvent
    {
        /// <summary>
        /// @see VstEventTypes
        /// </summary>
        public VstInt32 type;
        /// <summary>
        /// size of this event, excl. type and byteSize
        /// </summary>
        public VstInt32 byteSize;
        /// <summary>
        /// sample frames related to the current block start sample position
        /// </summary>
        public VstInt32 deltaFrames;
        /// <summary>
        /// generic flags, none defined yet
        /// </summary>
        public VstInt32 flags;
        /// <summary>
        /// data size may vary, depending on event type
        /// </summary>
        public fixed byte data[16];
    }

    public static class VstEventTypes
    {
        /// <summary>
        /// MIDI event  @see VstMidiEvent
        /// </summary>
        public const int kVstMidiType = 1;
        /// <summary>
        /// unused event type
        /// </summary>
        [Obsolete]
        public const int __kVstAudioTypeDeprecated = 2;
        /// <summary>
        /// unused event type
        /// </summary>
        [Obsolete]
        public const int __kVstVideoTypeDeprecated = 3;
        /// <summary>
        /// unused event type
        /// </summary>
        [Obsolete]
        public const int __kVstParameterTypeDeprecated = 4;
        /// <summary>
        /// unused event type
        /// </summary>
        [Obsolete]
        public const int __kVstTriggerTypeDeprecated = 5;
        /// <summary>
        /// MIDI system exclusive  @see VstMidiSysexEvent
        /// </summary>
        public const int kVstSysExType = 6;
    }

    /// <summary>
    /// A block of events for the current processed audio block.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct VstEvents
    {
        const int MAX_VST_EVENTS = 1024;
        /// <summary>
        /// number of Events in array
        /// </summary>
        public VstInt32 numEvents;
        /// <summary>
        /// zero (Reserved for future use)
        /// </summary>
        public VstIntPtr reserved;
        //[MarshalAs( UnmanagedType.ByValArray, SizeConst = MAX_VST_EVENTS )]
        /// <summary>
        /// event pointer array, variable size
        /// </summary>
        public fixed int events[MAX_VST_EVENTS];
    }

    /// <summary>
    /// MIDI Event (to be casted from VstEvent).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct VstMidiEvent
    {
        /// <summary>
        /// #kVstMidiType
        /// </summary>
        public VstInt32 type;
        /// <summary>
        /// sizeof (VstMidiEvent)
        /// </summary>
        public VstInt32 byteSize;
        /// <summary>
        /// sample frames related to the current block start sample position
        /// </summary>
        public VstInt32 deltaFrames;
        /// <summary>
        /// @see VstMidiEventFlags
        /// </summary>
        public VstInt32 flags;
        /// <summary>
        /// (in sample frames) of entire note, if available, else 0
        /// </summary>
        public VstInt32 noteLength;
        /// <summary>
        /// offset (in sample frames) into note from note start if available, else 0
        /// </summary>
        public VstInt32 noteOffset;
        /// <summary>
        /// 1 to 3 MIDI bytes; midiData[3] is reserved (zero)
        /// </summary>
        public fixed byte midiData[4];
        /// <summary>
        /// -64 to +63 cents; for scales other than 'well-tempered' ('microtuning')
        /// </summary>
        public byte detune;
        /// <summary>
        /// Note Off Velocity [0, 127]
        /// </summary>
        public byte noteOffVelocity;
        /// <summary>
        /// zero (Reserved for future use)
        /// </summary>
        public byte reserved1;
        /// <summary>
        /// zero (Reserved for future use)
        /// </summary>
        public byte reserved2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ERect
    {
        /// <summary>
        /// top coordinate
        /// </summary>
        public VstInt16 top;
        /// <summary>
        /// left coordinate
        /// </summary>
        public VstInt16 left;
        /// <summary>
        /// bottom coordinate
        /// </summary>
        public VstInt16 bottom;
        /// <summary>
        /// right coordinate
        /// </summary>
        public VstInt16 right;

        public override string ToString()
        {
            return "{top=" + top + ", left=" + left + ", bottom=" + bottom + ", right=" + right + "}";
        }
    }
    #endregion

}
