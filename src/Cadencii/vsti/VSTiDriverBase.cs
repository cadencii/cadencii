#if ENABLE_VOCALOID
/*
 * VSTiDriverBase.cs
 * Copyright © 2008-2013 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#define TEST

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using cadencii.java.awt;
using cadencii.java.util;
using cadencii.vsq;
using VstSdk;

namespace cadencii
{
    using VstInt32 = Int32;
    using VstIntPtr = Int32;


    public struct TempoInfo
    {
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

    public abstract class VSTiDriverBase
    {
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        protected delegate IntPtr PVSTMAIN([MarshalAs(UnmanagedType.FunctionPtr)]audioMasterCallback audioMaster);

        public bool loaded = false;
        public string path = "";
        /// <summary>
        /// プラグインのUI
        /// </summary>
        protected FormPluginUi ui = null;
        private bool isUiOpened = false;

        protected PVSTMAIN mainDelegate;
        private IntPtr mainProcPointer;
        protected audioMasterCallback audioMaster;
        /// <summary>
        /// 読込んだdllから作成したVOCALOID2の本体。VOCALOID2への操作はs_aeffect->dispatcherで行う
        /// </summary>
        protected AEffectWrapper aEffect;
        protected IntPtr aEffectPointer;
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
        private IntPtr bufferLeft = IntPtr.Zero;
        /// <summary>
        /// 右チャンネル用バッファ
        /// </summary>
        private IntPtr bufferRight = IntPtr.Zero;
        /// <summary>
        /// 左右チャンネルバッファの配列(buffers={bufferLeft, bufferRight})
        /// </summary>
        private IntPtr buffers = IntPtr.Zero;
        /// <summary>
        /// パラメータの，ロード時のデフォルト値
        /// </summary>
        private float[] paramDefaults = null;
        /// <summary>
        /// UIウィンドウのサイズ
        /// </summary>
        private Dimension uiWindowRect = new Dimension(373, 158);
        /* /// <summary>
        /// win32.LoadLibraryExを使うかどうか。trueならwin32.LoadLibraryExを使い、falseならutil.dllのLoadDllをつかう。既定ではtrue
        /// </summary>
        private bool useNativeDllLoader = true;*/
        protected MemoryManager memoryManager = new MemoryManager();
        private Object mSyncRoot = new Object();

        /// <summary>
        /// このドライバが担当する、合成エンジンの種類を取得する
        /// </summary>
        /// <returns>合成エンジンの種類</returns>
        public abstract RendererKind getRendererKind();

        public int getSampleRate()
        {
            return sampleRate;
        }

        public void resetAllParameters()
        {
            if (paramDefaults == null) {
                return;
            }
            for (int i = 0; i < paramDefaults.Length; i++) {
                setParameter(i, paramDefaults[i]);
            }
        }

        public virtual float getParameter(int index)
        {
            float ret = 0.0f;
            try {
                ret = aEffect.GetParameter(index);
            } catch (Exception ex) {
                serr.println("vstidrv#getParameter; ex=" + ex);
            }
            return ret;
        }

        public virtual void setParameter(int index, float value)
        {
            try {
                aEffect.SetParameter(index, value);
            } catch (Exception ex) {
                serr.println("vstidrv#setParameter; ex=" + ex);
            }
        }

        private string getStringCore(int opcode, int index, int str_capacity)
        {
            byte[] arr = new byte[str_capacity + 1];
            for (int i = 0; i < str_capacity; i++) {
                arr[i] = 0;
            }
            IntPtr ptr = IntPtr.Zero;
            try {
                unsafe {
                    fixed (byte* bptr = &arr[0]) {
                        ptr = new IntPtr(bptr);
                        aEffect.Dispatch(opcode, index, 0, ptr, 0.0f);
                    }
                }
            } catch (Exception ex) {
                serr.println("vstidrv#getStringCore; ex=" + ex);
            }
            string ret = Encoding.ASCII.GetString(arr);
            return ret;
        }

        public string getParameterDisplay(int index)
        {
            return getStringCore(AEffectOpcodes.effGetParamDisplay, index, VstStringConstants.kVstMaxParamStrLen);
        }

        public string getParameterLabel(int index)
        {
            return getStringCore(AEffectOpcodes.effGetParamLabel, index, VstStringConstants.kVstMaxParamStrLen);
        }

        public string getParameterName(int index)
        {
            return getStringCore(AEffectOpcodes.effGetParamName, index, VstStringConstants.kVstMaxParamStrLen);
        }

        private void initBuffer()
        {
            if (bufferLeft == IntPtr.Zero) {
                bufferLeft = Marshal.AllocHGlobal(sizeof(float) * BUFLEN);
            }
            if (bufferRight == IntPtr.Zero) {
                bufferRight = Marshal.AllocHGlobal(sizeof(float) * BUFLEN);
            }
            if (buffers == IntPtr.Zero) {
                unsafe {
                    buffers = Marshal.AllocHGlobal(sizeof(float*) * 2);
                }
            }
        }

        private void releaseBuffer()
        {
            if (bufferLeft != IntPtr.Zero) {
                Marshal.FreeHGlobal(bufferLeft);
                bufferLeft = IntPtr.Zero;
            }
            if (bufferRight != IntPtr.Zero) {
                Marshal.FreeHGlobal(bufferRight);
                bufferRight = IntPtr.Zero;
            }
            if (buffers != IntPtr.Zero) {
                Marshal.FreeHGlobal(buffers);
                buffers = IntPtr.Zero;
            }
        }

        public void process(double[] left, double[] right, int length)
        {
            process<double>(left, right, length, _ => _);
        }

        public void process(float[] left, float[] right, int length)
        {
            process<float>(left, right, length, _ => _);
        }

        private void process<T>(T[] left, T[] right, int length, Func<float, T> convert)
        {
            if (left == null || right == null) {
                return;
            }
            try {
                initBuffer();
                int remain = length;
                int offset = 0;
                unsafe {
                    float* left_ch = (float*)bufferLeft.ToPointer();
                    float* right_ch = (float*)bufferRight.ToPointer();
                    for (int i = 0; i < BUFLEN; ++i) {
                        left_ch[i] = 0;
                        right_ch[i] = 0;
                    }
                    float** out_buffer = (float**)buffers.ToPointer();
                    out_buffer[0] = left_ch;
                    out_buffer[1] = right_ch;
                    while (remain > 0) {
                        int proc = (remain > BUFLEN) ? BUFLEN : remain;
                        aEffect.ProcessReplacing(IntPtr.Zero, new IntPtr(out_buffer), proc);
                        for (int i = 0; i < proc; i++) {
                            left[i + offset] = convert(left_ch[i]);
                            right[i + offset] = convert(right_ch[i]);
                        }
                        remain -= proc;
                        offset += proc;
                    }
                }
            } catch (Exception ex) {
                serr.println("vstidrv#process; ex=" + ex);
            }
        }

        public virtual void send(MidiEvent[] events, int delta_clocks = 0)
        {
            if (events.Length == 0) return;
            unsafe {
                MemoryManager mman = null;
                try {
                    mman = new MemoryManager();
                    int nEvents = events.Length;
                    VstEvents* pVSTEvents = (VstEvents*)mman.malloc(sizeof(VstEvent) + nEvents * sizeof(VstEvent*)).ToPointer();
                    pVSTEvents->numEvents = 0;
                    pVSTEvents->reserved = (VstIntPtr)0;

                    for (int i = 0; i < nEvents; i++) {
                        MidiEvent pProcessEvent = events[i];
                        //byte event_code = (byte)pProcessEvent.firstByte;
                        VstEvent* pVSTEvent = (VstEvent*)0;
                        VstMidiEvent* pMidiEvent;
                        pMidiEvent = (VstMidiEvent*)mman.malloc((int)(sizeof(VstMidiEvent) + (pProcessEvent.data.Length + 1) * sizeof(byte))).ToPointer();
                        pMidiEvent->byteSize = sizeof(VstMidiEvent);
                        pMidiEvent->deltaFrames = delta_clocks;
                        pMidiEvent->detune = 0;
                        pMidiEvent->flags = 1;
                        pMidiEvent->noteLength = 0;
                        pMidiEvent->noteOffset = 0;
                        pMidiEvent->noteOffVelocity = 0;
                        pMidiEvent->reserved1 = 0;
                        pMidiEvent->reserved2 = 0;
                        pMidiEvent->type = VstEventTypes.kVstMidiType;
                        pMidiEvent->midiData[0] = (byte)(0xff & pProcessEvent.firstByte);
                        for (int j = 0; j < pProcessEvent.data.Length; j++) {
                            pMidiEvent->midiData[j + 1] = (byte)(0xff & pProcessEvent.data[j]);
                        }
                        pVSTEvents->events[pVSTEvents->numEvents++] = (int)(VstEvent*)pMidiEvent;
                    }
                    aEffect.Dispatch(AEffectXOpcodes.effProcessEvents, 0, 0, new IntPtr(pVSTEvents), 0);
                } catch (Exception ex) {
                    serr.println("vstidrv#send; ex=" + ex);
                } finally {
                    if (mman != null) {
                        try {
                            mman.dispose();
                        } catch (Exception ex2) {
                            serr.println("vstidrv#send; ex2=" + ex2);
                        }
                    }
                }
            }
        }

        protected virtual VstIntPtr AudioMaster(ref AEffect effect, VstInt32 opcode, VstInt32 index, VstIntPtr value, IntPtr ptr, float opt)
        {
            VstIntPtr result = 0;
            switch (opcode) {
                case AudioMasterOpcodes.audioMasterVersion: {
                    result = Constants.kVstVersion;
                    break;
                }
            }
            return result;
        }

        public FormPluginUi getUi(System.Windows.Forms.Form main_window)
        {
            if (ui == null) {
                if (main_window != null) {
                    // mainWindowのスレッドで、uiが作成されるようにする
                    main_window.Invoke(new Action(createPluginUi));
                }
            }
            return ui;
        }

        private void createPluginUi()
        {
            bool hasUi = (aEffect.aeffect.flags & VstAEffectFlags.effFlagsHasEditor) == VstAEffectFlags.effFlagsHasEditor;
            if (!hasUi) {
                return;
            }
            if (ui == null) {
                ui = new FormPluginUi();
            }
            if (!isUiOpened) {
                // Editorを持っているかどうかを確認
                if ((aEffect.aeffect.flags & VstAEffectFlags.effFlagsHasEditor) == VstAEffectFlags.effFlagsHasEditor) {
                    try {
                        // プラグインの名前を取得
                        string product = getStringCore(AEffectXOpcodes.effGetProductString, 0, VstStringConstants.kVstMaxProductStrLen);
                        ui.Text = product;
                        ui.Location = new System.Drawing.Point(0, 0);
                        unsafe {
                            aEffect.Dispatch(AEffectOpcodes.effEditOpen, 0, 0, ui.Handle, 0.0f);
                        }
                        //Thread.Sleep( 250 );
                        updatePluginUiRect();
                        ui.ClientSize = new System.Drawing.Size(uiWindowRect.width, uiWindowRect.height);
                        ui.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                        isUiOpened = true;
                    } catch (Exception ex) {
                        serr.println("vstidrv#open; ex=" + ex);
                        isUiOpened = false;
                    }
                }
            }
            return;
        }

        public virtual void setSampleRate(int sample_rate)
        {
            sampleRate = sample_rate;
            int ret1 = aEffect.Dispatch(AEffectOpcodes.effSetSampleRate, 0, 0, IntPtr.Zero, (float)sampleRate);
            int ret2 = aEffect.Dispatch(AEffectOpcodes.effSetBlockSize, 0, sampleRate, IntPtr.Zero, 0);
#if DEBUG
            sout.println("vstidrv#setSampleRate; ret1=" + ret1 + "; ret2=" + ret2);
#endif
        }

        public virtual bool open(int block_size, int sample_rate)
        {
            dllHandle = win32.LoadLibraryExW(path, IntPtr.Zero, win32.LOAD_WITH_ALTERED_SEARCH_PATH);
            if (dllHandle == IntPtr.Zero) {
                serr.println("vstidrv#open; dllHandle is null");
                return false;
            }

            mainProcPointer = win32.GetProcAddress(dllHandle, "main");
            mainDelegate = (PVSTMAIN)Marshal.GetDelegateForFunctionPointer(mainProcPointer,
                                                                            typeof(PVSTMAIN));
            if (mainDelegate == null) {
                serr.println("vstidrv#open; mainDelegate is null");
                return false;
            }

            audioMaster = new audioMasterCallback(AudioMaster);
            if (audioMaster == null) {
                serr.println("vstidrv#open; audioMaster is null");
                return false;
            }

            aEffectPointer = IntPtr.Zero;
            try {
                aEffectPointer = mainDelegate(audioMaster);
            } catch (Exception ex) {
                serr.println("vstidrv#open; ex=" + ex);
                return false;
            }
            if (aEffectPointer == IntPtr.Zero) {
                serr.println("vstidrv#open; aEffectPointer is null");
                return false;
            }
            blockSize = block_size;
            sampleRate = sample_rate;
            aEffect = new AEffectWrapper();
            aEffect.aeffect = (AEffect)Marshal.PtrToStructure(aEffectPointer, typeof(AEffect));
            aEffect.Dispatch(AEffectOpcodes.effOpen, 0, 0, IntPtr.Zero, 0);
            int ret = aEffect.Dispatch(AEffectOpcodes.effSetSampleRate, 0, 0, IntPtr.Zero, (float)sampleRate);
#if DEBUG
            sout.println("vstidrv#open; dll_path=" + path + "; ret for effSetSampleRate=" + ret);
#endif

            aEffect.Dispatch(AEffectOpcodes.effSetBlockSize, 0, blockSize, IntPtr.Zero, 0);

            // デフォルトのパラメータ値を取得
            int num = aEffect.aeffect.numParams;
            paramDefaults = new float[num];
            for (int i = 0; i < num; i++) {
                paramDefaults[i] = aEffect.GetParameter(i);
            }

            return true;
        }

        private void updatePluginUiRect()
        {
            if (ui != null) {
                try {
                    win32.EnumChildWindows(ui.Handle, enumChildProc, 0);
                } catch (Exception ex) {
                    serr.println("vstidrv#updatePluginUiRect; ex=" + ex);
                }
            }
        }

        private bool enumChildProc(IntPtr hwnd, int lParam)
        {
            RECT rc = new RECT();
            try {
                win32.GetWindowRect(hwnd, ref rc);
            } catch (Exception ex) {
                serr.println("vstidrv#enumChildProc; ex=" + ex);
            }
            if (ui != null) {
                ui.childWnd = hwnd;
            }
            uiWindowRect = new Dimension(rc.right - rc.left, rc.bottom - rc.top);
            return false; //最初のやつだけ検出できればおｋなので
        }

        public virtual void close()
        {
            lock (mSyncRoot) {
#if TEST
                sout.println("vstidrv#close");
#endif
                if (ui != null && !ui.IsDisposed) {
                    ui.Close();
                }
                try {
                    sout.println("vstidrv#close; (aEffect==null)=" + (aEffect == null));
                    if (aEffect != null) {
                        aEffect.Dispatch(AEffectOpcodes.effClose, 0, 0, IntPtr.Zero, 0.0f);
                    }
                    sout.println("vstidrv#close; dllHandle=" + dllHandle);
                    if (dllHandle != IntPtr.Zero) {
                        win32.FreeLibrary(dllHandle);
                    }
                    aEffect = null;
                    dllHandle = IntPtr.Zero;
                    mainDelegate = null;
                    audioMaster = null;
                } catch (Exception ex) {
                    serr.println("vstidrv#close; ex=" + ex);
                }
                releaseBuffer();
            }
        }

        ~VSTiDriverBase()
        {
            lock (mSyncRoot) {
                close();
            }
        }
    }

}
#endif // ENABLE_VOCALOID
