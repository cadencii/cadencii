#if ENABLE_VOCALOID
/*
 * VocaloidDriver.cs
 * Copyright © 2009-2011 kbinani
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
using System;
using System.Collections.Generic;
using cadencii.java.util;
using cadencii.vsq;
using VstSdk;

namespace cadencii
{
    using VstIntPtr = System.Int32;

    public unsafe class VocaloidDriver : VSTiDriverBase
    {
        const int TIME_FORMAT = 480;
        /// <summary>
        /// デフォルトのテンポ．
        /// </summary>
        const int DEF_TEMPO = 500000;

        private List<List<MidiEvent>> event_list_;
        private List<TempoInfo> tempo_list_;
        private double progress_;
        /// <summary>
        /// StartRenderingメソッドが回っている最中にtrue
        /// </summary>
        private bool is_rendering_ = false;
        private object mutex_ = new object();
        private readonly RendererKind kind_;

        public VocaloidDriver(RendererKind kind)
        {
            this.kind_ = kind;
        }

        public void clearSendEvents()
        {
            lock (mutex_) {
                for (int i = 0; i < event_list_.Count; i++) {
                    event_list_[i].Clear();
                }
            }
        }

        public override RendererKind getRendererKind()
        {
            return kind_;
        }

        /// <summary>
        /// 指定したタイムコードにおける，曲頭から測った時間を調べる
        /// </summary>
        private double msec_from_clock(int timeCode)
        {
            double ret = 0.0;
            int index = -1;
            int c = tempo_list_.Count;
            for (int i = 0; i < c; i++) {
                if (timeCode <= tempo_list_[i].Clock) {
                    break;
                }
                index = i;
            }
            if (index >= 0) {
                TempoInfo item = tempo_list_[index];
                ret = item.TotalSec + (timeCode - item.Clock) * (double)item.Tempo / (1000.0 * TIME_FORMAT);
            } else {
                ret = timeCode * (double)DEF_TEMPO / (1000.0 * TIME_FORMAT);
            }
            return ret;
        }

        public override bool open(int block_size, int sample_rate)
        {
            bool ret = base.open(block_size, sample_rate);
            tempo_list_ = new List<TempoInfo>();
            progress_ = 0.0;
            event_list_ = new List<List<MidiEvent>>();
            event_list_.Add(new List<MidiEvent>());
            event_list_.Add(new List<MidiEvent>());
            return ret;
        }

        public void sendEvent(byte[] src, int[] deltaFrames, int targetTrack)
        {
            lock (mutex_) {
                int count;
                int numEvents = deltaFrames.Length;
                if (targetTrack == 0) {
                    if (tempo_list_ == null) {
                        tempo_list_ = new List<TempoInfo>();
                    } else {
                        tempo_list_.Clear();
                    }
                    if (numEvents <= 0) {
                        TempoInfo ti = new TempoInfo();
                        ti.Clock = 0;
                        ti.Tempo = DEF_TEMPO;
                        ti.TotalSec = 0.0;
                        tempo_list_.Add(ti);
                    } else {
                        if (deltaFrames[0] != 0) {
                            TempoInfo ti = new TempoInfo();
                            ti.Clock = 0;
                            ti.Tempo = DEF_TEMPO;
                            ti.TotalSec = 0.0;
                            tempo_list_.Add(ti);
                        }
                        int prev_tempo = DEF_TEMPO;
                        int prev_clock = 0;
                        double total = 0.0;
                        count = -3;
                        for (int i = 0; i < numEvents; i++) {
                            count += 3;
                            int tempo = (int)(src[count + 2] | (src[count + 1] << 8) | (src[count] << 16));
                            total += (deltaFrames[i] - prev_clock) * (double)prev_tempo / (1000.0 * TIME_FORMAT);
                            TempoInfo ti = new TempoInfo();
                            ti.Clock = deltaFrames[i];
                            ti.Tempo = tempo;
                            ti.TotalSec = total;
                            tempo_list_.Add(ti);
                            prev_tempo = tempo;
                            prev_clock = deltaFrames[i];
                        }
                    }
                }

                // 与えられたイベント情報をs_track_eventsに収納
                count = -3;
                event_list_[targetTrack].Clear();
                for (int i = 0; i < numEvents; i++) {
                    count += 3;
                    MidiEvent pEvent = new MidiEvent();
                    pEvent.clock = (uint)deltaFrames[i];
                    if (targetTrack == 0) {
                        pEvent.firstByte = 0xff;
                        pEvent.data = new int[5];
                        pEvent.data[0] = 0x51;
                        pEvent.data[1] = 0x03;
                        pEvent.data[2] = src[count];
                        pEvent.data[3] = src[count + 1];
                        pEvent.data[4] = src[count + 2];
                    } else {
                        pEvent.firstByte = src[count];
                        pEvent.data = new int[3];
                        pEvent.data[0] = src[count + 1];
                        pEvent.data[1] = src[count + 2];
                        pEvent.data[2] = 0x00;
                    }
                    event_list_[targetTrack].Add(pEvent);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="total_samples"></param>
        /// <param name="mode_infinite"></param>
        /// <param name="sample_rate"></param>
        /// <param name="runner">このドライバを駆動しているRenderingRunnerのオブジェクト</param>
        /// <returns></returns>
        public void startRendering(long total_samples, bool mode_infinite, int sample_rate, IWaveIncoming runner, WorkerState state)
        {
            lock (mutex_) {
                is_rendering_ = true;
                progress_ = 0.0;
                sampleRate = sample_rate;

                List<MidiEvent> lpEvents = merge_events(event_list_[0], event_list_[1]);
                int current_count = -1;
                MidiEvent current = new MidiEvent();

                MemoryManager mman = null;
                float* left_ch;
                float* right_ch;
                float** out_buffer;
                try {
                    mman = new MemoryManager();
                    left_ch = (float*)mman.malloc(sizeof(float) * sampleRate).ToPointer();
                    right_ch = (float*)mman.malloc(sizeof(float) * sampleRate).ToPointer();
                    out_buffer = (float**)mman.malloc(sizeof(float*) * 2).ToPointer();
                    out_buffer[0] = left_ch;
                    out_buffer[1] = right_ch;

                    double[] buffer_l = new double[sampleRate];
                    double[] buffer_r = new double[sampleRate];

                    aEffect.Dispatch(AEffectOpcodes.effSetSampleRate, 0, 0, IntPtr.Zero, (float)sampleRate);
                    aEffect.Dispatch(AEffectOpcodes.effMainsChanged, 0, 1, IntPtr.Zero, 0);

                    // ここではブロックサイズ＝サンプリングレートということにする
                    aEffect.Dispatch(AEffectOpcodes.effSetBlockSize, 0, sampleRate, IntPtr.Zero, 0);

                    // レンダリングの途中で停止した場合，ここでProcessする部分が無音でない場合がある
                    for (int i = 0; i < 3; i++) {
                        aEffect.ProcessReplacing(IntPtr.Zero, new IntPtr(out_buffer), sampleRate);
                    }

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

                    dwDelay = 0;
                    List<MidiEvent> list = event_list_[1];
                    int list_size = list.Count;
                    for (int i = 0; i < list_size; i++) {
                        MidiEvent work = list[i];
                        if ((work.firstByte & 0xf0) == 0xb0) {
                            switch (work.data[0]) {
                                case 0x63: {
                                    addr_msb = work.data[1];
                                    addr_lsb = 0;
                                    break;
                                }
                                case 0x62: {
                                    addr_lsb = work.data[1];
                                    break;
                                }
                                case 0x06: {
                                    data_msb = work.data[1];
                                    break;
                                }
                                case 0x26: {
                                    data_lsb = work.data[1];
                                    if (addr_msb == 0x50 && addr_lsb == 0x01) {
                                        dwDelay = (data_msb & 0xff) << 7 | (data_lsb & 0x7f);
                                    }
                                    break;
                                }
                            }
                        }
                        if (dwDelay > 0) {
                            break;
                        }
                    }

                    while (!state.isCancelRequested()) {
                        int process_event_count = current_count;
                        int nEvents = 0;

                        if (current_count < 0) {
                            current_count = 0;
                            current = lpEvents[current_count];
                            process_event_count = current_count;
                        }
                        while (current.clock == dwNow) {
                            // durationを取得
                            if ((current.firstByte & 0xf0) == 0xb0) {
                                switch (current.data[0]) {
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
                                    if (addr_msb == 0x50 && addr_lsb == 0x4) {
                                        duration = data_msb << 7 | data_lsb;
                                    }
                                    break;
                                }
                            }

                            nEvents++;
                            if (current_count + 1 < lpEvents.Count) {
                                current_count++;
                                current = lpEvents[current_count];
                            } else {
                                break;
                            }
                        }

                        if (current_count + 1 >= lpEvents.Count) {
                            break;
                        }

                        double msNow = msec_from_clock(dwNow);
                        dwDelta = (int)(msNow / 1000.0 * sampleRate) - total_processed;

                        VstEvents* pVSTEvents = (VstEvents*)mman.malloc(sizeof(VstEvent) + nEvents * sizeof(VstEvent*)).ToPointer();
                        pVSTEvents->numEvents = 0;
                        pVSTEvents->reserved = (VstIntPtr)0;

                        for (int i = 0; i < nEvents; i++) {
                            MidiEvent pProcessEvent = lpEvents[process_event_count];
                            int event_code = pProcessEvent.firstByte;
                            VstEvent* pVSTEvent = (VstEvent*)0;
                            VstMidiEvent* pMidiEvent;

                            switch (event_code) {
                                case 0xf0:
                                case 0xf7:
                                case 0xff:
                                break;
                                default:
                                pMidiEvent = (VstMidiEvent*)mman.malloc((int)(sizeof(VstMidiEvent) + (pProcessEvent.data.Length + 1) * sizeof(byte))).ToPointer();
                                pMidiEvent->byteSize = sizeof(VstMidiEvent);
                                pMidiEvent->deltaFrames = dwDelta;
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
                                break;
                            }
                            process_event_count++;
                        }
                        aEffect.Dispatch(AEffectXOpcodes.effProcessEvents, 0, 0, new IntPtr(pVSTEvents), 0);

                        while (dwDelta > 0 && !state.isCancelRequested()) {
                            int dwFrames = dwDelta > sampleRate ? sampleRate : dwDelta;
                            aEffect.ProcessReplacing(IntPtr.Zero, new IntPtr(out_buffer), dwFrames);

                            int iOffset = dwDelay - dwDeltaDelay;
                            if (iOffset > (int)dwFrames) {
                                iOffset = (int)dwFrames;
                            }

                            if (iOffset == 0) {
                                for (int i = 0; i < (int)dwFrames; i++) {
                                    buffer_l[i] = out_buffer[0][i];
                                    buffer_r[i] = out_buffer[1][i];
                                }
                                total_processed2 += dwFrames;
                                runner.waveIncomingImpl(buffer_l, buffer_r, dwFrames, state);
                            } else {
                                dwDeltaDelay += iOffset;
                            }
                            dwDelta -= dwFrames;
                            total_processed += dwFrames;
                        }

                        dwPrev = dwNow;
                        dwNow = (int)current.clock;
                        progress_ = total_processed / (double)total_samples * 100.0;
                    }

                    double msLast = msec_from_clock(dwNow);
                    dwDelta = (int)(sampleRate * ((double)duration + (double)delay) / 1000.0 + dwDeltaDelay);
                    if (total_samples - total_processed2 > dwDelta) {
                        dwDelta = (int)total_samples - total_processed2;
                    }
                    while (dwDelta > 0 && !state.isCancelRequested()) {
                        int dwFrames = dwDelta > sampleRate ? sampleRate : dwDelta;
                        aEffect.ProcessReplacing(IntPtr.Zero, new IntPtr(out_buffer), dwFrames);

                        for (int i = 0; i < (int)dwFrames; i++) {
                            buffer_l[i] = out_buffer[0][i];
                            buffer_r[i] = out_buffer[1][i];
                        }
                        total_processed2 += dwFrames;
                        runner.waveIncomingImpl(buffer_l, buffer_r, dwFrames, state);

                        dwDelta -= dwFrames;
                        total_processed += dwFrames;
                    }

                    if (mode_infinite) {
                        for (int i = 0; i < sampleRate; i++) {
                            buffer_l[i] = 0.0;
                            buffer_r[i] = 0.0;
                        }
                        while (!state.isCancelRequested()) {
                            total_processed2 += sampleRate;
                            runner.waveIncomingImpl(buffer_l, buffer_r, sampleRate, state);
                        }
                    }

                    aEffect.Dispatch(AEffectOpcodes.effMainsChanged, 0, 0, IntPtr.Zero, 0);
                    lpEvents.Clear();
                } catch (Exception ex) {
                    serr.println("VocaloidDriver#startRendering; ex=" + ex);
                } finally {
                    if (mman != null) {
                        try {
                            mman.dispose();
                        } catch (Exception ex2) {
                            serr.println("VocaloidDriver#startRendering; ex2=" + ex2);
                        }
                    }
                }
                is_rendering_ = false;
                for (int i = 0; i < event_list_.Count; i++) {
                    event_list_[i].Clear();
                }
                tempo_list_.Clear();
            }
        }

        public bool isRendering()
        {
            return is_rendering_;
        }

        public double getProgress()
        {
            return progress_;
        }

        private List<MidiEvent> merge_events(List<MidiEvent> x0, List<MidiEvent> y0)
        {
            List<MidiEvent> ret = new List<MidiEvent>();
            for (int i = 0; i < x0.Count; i++) {
                ret.Add(x0[i]);
            }
            for (int i = 0; i < y0.Count; i++) {
                ret.Add(y0[i]);
            }
            bool changed = true;
            while (changed) {
                changed = false;
                for (int i = 0; i < ret.Count - 1; i++) {
                    if (ret[i].CompareTo(ret[i + 1]) > 0) {
                        MidiEvent m = ret[i];
                        ret[i] = ret[i + 1];
                        ret[i + 1] = m;
                        changed = true;
                    }
                }

            }
            return ret;
        }
    }

}
#endif // ENABLE_VOCALOID
