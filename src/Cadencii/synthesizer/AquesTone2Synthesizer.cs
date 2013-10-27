#if ENABLE_AQUESTONE
/*
 * AquesTone2Synthesizer.cs
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
using System.Windows.Forms;
using cadencii.dsp.v2.generator;
using cadencii.vsq;

namespace cadencii.synthesizer
{
    /// <summary>
    /// AquesTone2 VSTi を使って歌声合成を行うクラス
    /// </summary>
    public class AquesTone2Synthesizer : ISingingSynthesizer
    {
        public event RenderCallback Rendered;

        private AquesTone2Driver driver_;
        private int sample_rate_;

        private VsqFile sequence_;
        private int track_index_;

        public AquesTone2Synthesizer(AquesTone2Driver driver, Form main_window)
        {
            driver_ = driver;
            driver_.getUi(main_window);
        }

        public void beginSession(VsqFile sequence, int track_index, int sample_rate)
        {
            sequence_ = (VsqFile)sequence.Clone();
            sequence_.updateTotalClocks();
            sequence_.updateTotalClocks();
            sequence_.updateTempoInfo();
            sequence_.updateTimesigInfo();

            track_index_ = track_index;
            sample_rate_ = sample_rate;
        }

        public void endSession()
        {
        }

        /// <summary>
        /// イベントキューを生成する
        /// </summary>
        /// <param name="vsq"></param>
        /// <param name="trackIndex"></param>
        /// <returns></returns>
        protected EventQueueSequence generateMidiEvent(VsqFile vsq, int trackIndex)
        {
            var result = new EventQueueSequence();

            var track = vsq.Track[trackIndex];
            appendNoteEvent(track, result);

            foreach (var item in track.MetaText.Events.Events) {
                reflectNoteEventPitch(item,
                                      track.MetaText.PIT,
                                      track.MetaText.PBS,
                                      vsq.TempoTable);
            }

            appendPitchEvent(track, result);

            return result;
        }

        /// <summary>
        /// ピッチとピッチベンドセンシティビティをイベントキューに追加する
        /// </summary>
        /// <param name="track"></param>
        /// <param name="sequence"></param>
        private void appendPitchEvent(VsqTrack track, EventQueueSequence sequence)
        {
            // 実際に AquesTone2 に送信する pbs の値と、pbs カーブに入っている値とのマップ
            const int maxPitchBendSensitivity = 23;
            int[] map = new int[maxPitchBendSensitivity + 1] {
                0, 5, 15, 35, 44, 54, 64, 74, 84, 93, 103, 113,
                127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127,
            };
            var pbs = track.MetaText.PBS;
            for (int i = 0; i < pbs.size(); ++i) {
                var clock = pbs.getKeyClock(i);
                {
                    // RPN MSB = 0x00
                    var e = new MidiEvent();
                    e.firstByte = 0xB0;
                    e.data = new int[] { 0x65, 0x00 };
                    e.clock = clock;
                    sequence.get(clock).pit.Add(e);
                }
                {
                    // RPN LSB = 0x00
                    var e = new MidiEvent();
                    e.firstByte = 0xB0;
                    e.data = new int[] { 0x64, 0x00 };
                    e.clock = clock;
                    sequence.get(clock).pit.Add(e);
                }
                {
                    // RPN data MSB
                    var e = new MidiEvent();
                    e.firstByte = 0xB0;
                    int value = Math.Max(0, Math.Min(maxPitchBendSensitivity, pbs.getElementA(i)));
                    e.data = new int[] { 0x06, map[value] };
                    e.clock = clock;
                    sequence.get(clock).pit.Add(e);
                }
            }

            var pit = track.MetaText.PIT;
            for (int i = 0; i < pit.size(); ++i) {
                var clock = pit.getKeyClock(i);
                var e = new MidiEvent();
                e.firstByte = 0xE0;
                var value = pit.getElementA(i) + 8192;
                var msb = 0x7F & value;
                var lsb = 0x7F & (value >> 7);
                e.data = new int[] { msb, lsb };
                sequence.get(clock).pit.Add(e);
            }
        }

        /// <summary>
        /// 音符に付随するピッチベンドの情報を、PIT・PBS カーブに反映する
        /// </summary>
        /// <param name="item">音符</param>
        /// <param name="pitchBend">PIT カーブ</param>
        /// <param name="pitchBendSensitivity">PBS カーブ</param>
        /// <param name="tempoTable">テンポ情報</param>
        protected void reflectNoteEventPitch(VsqEvent item, VsqBPList pitchBend, VsqBPList pitchBendSensitivity, TempoVector tempoTable)
        {
            if (item.ID.type != VsqIDType.Anote) return;

            // AquesTone2 では、note on と同 clock にピッチベンドイベントを送らないと音程が反映されないので、必ずピッチイベントが送られるようにする
            pitchBend.add(item.Clock, pitchBend.getValue(item.Clock));

            if (item.ID.VibratoHandle == null) {
                return;
            }

            int startClock = item.Clock + item.ID.VibratoDelay;
            int vibratoLength = item.ID.Length - item.ID.VibratoDelay;

            var iterator = new VibratoPointIteratorByClock(tempoTable,
                                                           item.ID.VibratoHandle.RateBP, item.ID.VibratoHandle.StartRate,
                                                           item.ID.VibratoHandle.DepthBP, item.ID.VibratoHandle.StartDepth,
                                                           startClock, vibratoLength);
            var pitContext = new ByRef<int>(0);
            var pbsContext = new ByRef<int>(0);

            int pitAtEnd = pitchBend.getValue(startClock + vibratoLength);
            int pbsAtEnd = pitchBendSensitivity.getValue(startClock + vibratoLength);
            var pitBackup = (VsqBPList)pitchBend.Clone();
            var pbsBackup = (VsqBPList)pitchBendSensitivity.Clone();

            bool resetPBS = false;
            double maxNetPitchBendInCent = 0.0;
            for (int clock = startClock; clock < startClock + vibratoLength && iterator.hasNext(); ++clock) {
                double vibratoPitchBendInCent = iterator.next() * 100.0;
                int pit = pitchBend.getValue(clock, pitContext);
                int pbs = pitchBendSensitivity.getValue(clock, pbsContext);
                const double pow2_13 = 8192;
                double netPitchBendInCent = (pbs * pit / pow2_13) * 100.0 + vibratoPitchBendInCent;
                maxNetPitchBendInCent = Math.Max(maxNetPitchBendInCent, Math.Abs(netPitchBendInCent));
                int draftPitchBend = (int)Math.Round((netPitchBendInCent / 100.0) * pow2_13 / pbs);

                if (draftPitchBend < pitchBend.Minimum || pitchBend.Maximum < draftPitchBend) {
                    // pbs を変更せずにビブラートによるピッチベンドを反映しようとすると、
                    // pit が範囲を超えてしまう。
                    resetPBS = true;
                } else {
                    if (draftPitchBend != pit) {
                        pitchBend.add(clock, draftPitchBend);
                    }
                }
            }

            if (!resetPBS) {
                return;
            }

            pitchBend.Data = pitBackup.Data;

            // ピッチベンドの最大値を実現するのに必要なPBS
            int requiredPitchbendSensitivity = (int)Math.Ceiling(maxNetPitchBendInCent / 100.0);
            int pseudoMaxPitchbendSensitivity = 12; // AquesTone2 は最大 12 半音までベンドできる。
            if (requiredPitchbendSensitivity < pitchBendSensitivity.Minimum) requiredPitchbendSensitivity = pitchBendSensitivity.Minimum;
            if (pseudoMaxPitchbendSensitivity < requiredPitchbendSensitivity) requiredPitchbendSensitivity = pseudoMaxPitchbendSensitivity;

            {
                int i = 0;
                while (i < pitchBend.size()) {
                    var clock = pitchBend.getKeyClock(i);
                    if (startClock <= clock && clock < startClock + vibratoLength) {
                        pitchBend.removeElementAt(i);
                    } else {
                        ++i;
                    }
                }
            }
            {
                int i = 0;
                while (i < pitchBendSensitivity.size()) {
                    var clock = pitchBendSensitivity.getKeyClock(i);
                    if (startClock <= clock && clock < startClock + vibratoLength) {
                        pitchBendSensitivity.removeElementAt(i);
                    } else {
                        ++i;
                    }
                }
            }
            if (pitchBendSensitivity.getValue(startClock) != requiredPitchbendSensitivity) {
                pitchBendSensitivity.add(startClock, requiredPitchbendSensitivity);
            }
            pitchBend.add(startClock + vibratoLength, pitAtEnd);
            pitchBendSensitivity.add(startClock + vibratoLength, pbsAtEnd);

            iterator.rewind();
            pitContext.value = 0;
            pbsContext.value = 0;
            int lastPitchBend = pitchBend.getValue(startClock);
            for (int clock = startClock; clock < startClock + vibratoLength && iterator.hasNext(); ++clock) {
                double vibratoPitchBendInCent = iterator.next() * 100.0;
                int pit = pitBackup.getValue(clock, pitContext);
                int pbs = pbsBackup.getValue(clock, pbsContext);

                const double pow2_13 = 8192;
                double netPitchBendInCent = (pbs * pit / pow2_13) * 100.0 + vibratoPitchBendInCent;
                maxNetPitchBendInCent = Math.Max(maxNetPitchBendInCent, Math.Abs(netPitchBendInCent));
                int draftPitchBend = (int)Math.Round((netPitchBendInCent / 100.0) * pow2_13 / requiredPitchbendSensitivity);
                if (draftPitchBend < pitchBend.Minimum) draftPitchBend = pitchBend.Minimum;
                if (pitchBend.Maximum < draftPitchBend) draftPitchBend = pitchBend.Maximum;
                if (draftPitchBend != lastPitchBend) {
                    pitchBend.add(clock, draftPitchBend);
                    lastPitchBend = draftPitchBend;
                }
            }
        }

        /// <summary>
        /// 音符の note on/off のためのイベントを作成し、イベントキューに追加する
        /// </summary>
        /// <param name="track">生成元のトラック</param>
        /// <param name="result">生成したイベントの追加先</param>
        private void appendNoteEvent(VsqTrack track, EventQueueSequence result)
        {
            foreach (var item in track.MetaText.Events.Events) {
                if (item.ID.type != VsqIDType.Anote) continue;
                var note = item.ID.Note;
                {
                    var clock = item.Clock;
                    var queue = result.get(clock);
                    var noteOn = driver_.createNoteOnEvent(item.ID.Note,
                                                           item.ID.Dynamics,
                                                           item.ID.LyricHandle.L0.Phrase);
                    queue.noteon.AddRange(noteOn);
                }
                {
                    var clock = item.Clock + item.ID.Length;
                    var queue = result.get(clock);
                    var noteOff = createNoteOffEvent(clock, item.ID.Note);
                    queue.noteoff.Add(noteOff);
                }
            }
        }

        public void start()
        {
            int buffer_length = sample_rate_ / 10;
            float[] left = new float[buffer_length];
            float[] right = new float[buffer_length];

            if (driver_.getUi(null) == null) {
                throw new InvalidOperationException("plugin ui を main view のスレッドで作成した後、このメソッドを呼ばなくてはならない。");
            }

            sendAllNoteOff();

            var eventQueue = generateMidiEvent(sequence_, track_index_);
            long processed_samples = 0;
            int last_clock = 0;
            foreach (var sequence_item in eventQueue.getSequence()) {
                var clock = sequence_item.Key;
                var queue = sequence_item.Value;

                long to_sample = (long)(sequence_.getSecFromClock(clock) * sample_rate_);
                int amount = (int)(to_sample - processed_samples);
                if (!doSynthesis(amount, left, right)) {
                    return;
                }
                processed_samples += amount;

                // noteOff, noteOn の順に分けて send する方法も考えられるが、正しく動作しない。
                // このため、いったん一つの配列にまとめてから send する必要がある。
                var events = new List<MidiEvent>();
                events.AddRange(queue.pit);
                events.AddRange(queue.noteoff);
                events.AddRange(queue.noteon);
                driver_.send(events.ToArray(), clock - last_clock);
                last_clock = clock;

                //TODO: のこりのイベント送る処理
            }

            while (doSynthesis(left.Length, left, right)) ;
        }

        private void sendAllNoteOff()
        {
            List<MidiEvent> note_off_events = new List<MidiEvent>();
            for (int note = 0; note < 128; ++note) {
                note_off_events.Add(createNoteOffEvent(0, note));
            }
            driver_.send(note_off_events.ToArray());
        }

        private bool doSynthesis(int amount, float[] left, float[] right)
        {
            int buffer_length = left.Length;
            int remain = amount;
            while (0 < remain) {
                int process = Math.Min(buffer_length, remain);
                Array.Clear(left, 0, process);
                Array.Clear(right, 0, process);
                driver_.process(left, right, process);
                if (Rendered != null) {
                    if (!Rendered(left, right, process)) {
                        return false;
                    }
                }
                remain -= process;
            }
            return true;
        }

        private MidiEvent createNoteOffEvent(int clock, int note)
        {
            var result = new MidiEvent();
            result.clock = clock;
            result.firstByte = 0x80;
            result.data = new int[] { note, 0x40 };
            return result;
        }
    }
}
#endif
