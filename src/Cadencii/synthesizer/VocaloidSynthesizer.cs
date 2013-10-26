/*
 * VocaloidSynthesizer.cs
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
    unsafe class VocaloidSynthesizer : VSTiDriverBase, ISingingSynthesizer
    {
        class Session
        {
            public readonly IEnumerable<MidiEvent> sequence_;
            public readonly ITempoMaster tempo_master_;
            public readonly int sample_rate_;
            public readonly float[] left_buffer_;
            public readonly float[] right_buffer_;
            public readonly int trim_samples_;
            public int trimmed_samples_;

            public Session(IEnumerable<MidiEvent> sequence, ITempoMaster tempo_master, int sample_rate, int trim_samples)
            {
                sequence_ = sequence;
                tempo_master_ = tempo_master;
                sample_rate_ = sample_rate;
                left_buffer_ = new float[sample_rate];
                right_buffer_ = new float[sample_rate];
                trim_samples_ = trim_samples;
                trimmed_samples_ = 0;
            }
        }

        public event RenderCallback Rendered;

        private readonly RendererKind kind_;
        private readonly MemoryManager allocator_ = new MemoryManager();
        private Session session_;
        private object session_mutex_ = new object();
        private readonly int presend_milli_seconds_;

        public VocaloidSynthesizer(RendererKind kind, string dll_path, int presend_milli_seconds)
        {
            kind_ = kind;
            path = dll_path;

            presend_milli_seconds_ = presend_milli_seconds;
        }

        public override RendererKind getRendererKind()
        {
            return kind_;
        }

        public void beginSession(VsqFile sequence, int track_index, int sample_rate)
        {
            lock (session_mutex_) {
                VsqNrpn[] vocaloid_nrpn = VsqFile.generateNRPN(sequence, track_index, presend_milli_seconds_);
                NrpnData[] midi_nrpn = VsqNrpn.convert(vocaloid_nrpn);
                IEnumerable<MidiEvent> event_sequence = midi_nrpn.Select(nrpn => {
                    MidiEvent item = new MidiEvent();
                    item.clock = nrpn.getClock();
                    item.firstByte = 0xB0;
                    item.data = new int[3] { nrpn.getParameter(), nrpn.Value, 0 };
                    return item;
                }).ToList();
                int trim_samples = getTrimSamples(event_sequence, sequence.TempoTable);
                session_ = new Session(event_sequence, sequence.TempoTable, sample_rate, trim_samples);

                if (!loaded) {
                    base.open(sample_rate, sample_rate);
                }
            }
        }

        public void endSession()
        {
            lock (session_mutex_) {
                session_ = null;
            }
        }

        public void start()
        {
            long clock = 0;
            long processed = 0;

            blockSize = session_.sample_rate_;

            setSampleRate(session_.sample_rate_);
            aEffect.Dispatch(AEffectOpcodes.effMainsChanged, 0, 1, IntPtr.Zero, 0);

            List<MidiEvent> event_buffer = new List<MidiEvent>();
            foreach (var item in session_.sequence_) {
                if (item.clock == clock) {
                    event_buffer.Add(item);
                } else {
                    int process_samples = (int)((long)(session_.tempo_master_.getSecFromClock(clock) * sampleRate) - processed);
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
            lock (session_mutex_) {
                if (session_ == null) {
                    return false;
                }

                process(session_.left_buffer_, session_.right_buffer_, amount);
                if (Rendered == null) {
                    return false;
                } else {
                    int trim_samples = session_.trim_samples_ - session_.trimmed_samples_;
                    if (trim_samples > amount) {
                        trim_samples = amount;
                    }
                    if (trim_samples == 0) {
                        return Rendered(session_.left_buffer_, session_.right_buffer_, amount);
                    } else {
                        session_.trimmed_samples_ += trim_samples;
                        return true;
                    }
                }
            }
        }

        private int getTrimSamples(IEnumerable<MidiEvent> sequence, TempoVector tempo_table)
        {
            float first_tempo = tempo_table.Select(entry => (float)(60e6 / (double)entry.Tempo)).FirstOrDefault();
            if (first_tempo == default(float)) {
                first_tempo = 125.0f;
            }
            return VSTiDllManager.getErrorSamples(first_tempo) + getFirstNoteDelay(sequence);
        }

        private int getFirstNoteDelay(IEnumerable<MidiEvent> sequence)
        {
            int addr_msb = 0;
            int addr_lsb = 0;
            int data_msb = 0;
            int data_lsb = 0;
            foreach (var work in sequence) {
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
                                return (data_msb & 0xff) << 7 | (data_lsb & 0x7f);
                            }
                            break;
                        }
                    }
                }
            }
            return 0;
        }
    }
}
