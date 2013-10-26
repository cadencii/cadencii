/*
 * VocaloidVstDriver.cs
 * Copyright © 2013 kbinani
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
using System.Linq;
using VstSdk;
using cadencii.dsp.v2.generator;
using cadencii.vsq;

namespace cadencii.synthesizer
{
    unsafe class VocaloidVstDriver : VSTiDriverBase, ISingingSynthesizer
    {
        public event RenderCallback Rendered;

        private readonly RendererKind kind_;
        private readonly MemoryManager allocator_ = new MemoryManager();
        private float[] left_buffer_;
        private float[] right_buffer_;
        private IEnumerable<MidiEvent> sequence_;
        private ITempoMaster tempo_master_;
        private int sample_rate_;

        public VocaloidVstDriver(RendererKind kind, string dll_path)
        {
            kind_ = kind;
            path = dll_path;
        }

        public override RendererKind getRendererKind()
        {
            return kind_;
        }

        public void beginSession(VsqFile sequence, int track_index, int sample_rate)
        {
            VsqNrpn[] vocaloid_nrpn = VsqFile.generateNRPN(sequence, track_index, 500);
            NrpnData[] midi_nrpn = VsqNrpn.convert(vocaloid_nrpn);
            sequence_ = midi_nrpn.Select(nrpn => {
                MidiEvent item = new MidiEvent();
                item.clock = nrpn.getClock();
                item.firstByte = 0xB0;
                item.data = new int[3] { nrpn.getParameter(), nrpn.Value, 0 };
                return item;
            }).ToList();
            tempo_master_ = sequence.TempoTable;
            sample_rate_ = sample_rate;

            if (!loaded) {
                base.open(sample_rate_, sample_rate_);
            }
        }

        public void endSession()
        {
            sequence_ = null;
            tempo_master_ = null;
        }

        public void start()
        {
            long clock = 0;
            long processed = 0;

            blockSize = sample_rate_;

            setSampleRate(sample_rate_);
            aEffect.Dispatch(AEffectOpcodes.effMainsChanged, 0, 1, IntPtr.Zero, 0);

            List<MidiEvent> event_buffer = new List<MidiEvent>();
            foreach (var item in sequence_) {
                if (item.clock == clock) {
                    event_buffer.Add(item);
                } else {
                    int process_samples = (int)((long)(tempo_master_.getSecFromClock(clock) * sampleRate) - processed);
                    send(event_buffer.ToArray(), process_samples);
                    while (process_samples > 0) {
                        int amount = Math.Min(process_samples, blockSize);
                        if (!dispatchProcessReplacing(amount)) {
                            return;
                        }
                        process_samples -= amount;
                        processed += amount;
                    }
                    event_buffer.Clear();
                    event_buffer.Add(item);
                    clock = item.clock;
                }
            }

            while (dispatchProcessReplacing(blockSize)) ;
        }

        private bool dispatchProcessReplacing(int amount)
        {
            if (left_buffer_ == null) {
                left_buffer_ = new float[blockSize];
            }
            if (right_buffer_ == null) {
                right_buffer_ = new float[blockSize];
            }
            process(left_buffer_, right_buffer_, amount);
            if (Rendered == null) {
                return false;
            } else {
                return Rendered(left_buffer_, right_buffer_, amount);
            }
        }
    }
}
