#if ENABLE_AQUESTONE
/*
 * AquesToneSynthesizerBase.cs
 * Copyright © 2010-2013 kbinani
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
using System.Windows.Forms;
using cadencii.dsp.v2.generator;
using cadencii.vsq;

namespace cadencii.synthesizer
{
    public abstract class AquesToneSynthesizerBase : ISingingSynthesizer
    {
        class Session
        {
            public VsqFile sequence_ = null;
            public int track_index_;
            public int sample_rate_;
            public float[] left_ = new float[BUFLEN];
            public float[] right_ = new float[BUFLEN];
        }

        public event RenderCallback Rendered;

        private const int VERSION = 0;
        private const int BUFLEN = 1024;
        private readonly Form main_window_;
        private Session session_;
        private object session_mutex_ = new object();

        protected abstract EventQueueSequence generateMidiEvent(VsqFile vsq, int track, int clock_start, int clock_end);

        protected abstract AquesToneDriverBase getDriver();

        protected AquesToneSynthesizerBase(Form main_window)
        {
            main_window_ = main_window;
        }

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        /// <param name="parameter"></param>
        public void beginSession(VsqFile vsq, int track, int sample_rate)
        {
            lock (session_mutex_) {
                session_ = new Session();
                getDriver().setSampleRate(sample_rate);
                session_.track_index_ = track;
                session_.sample_rate_ = sample_rate;

                session_.sequence_ = (VsqFile)vsq.clone();
                session_.sequence_.updateTotalClocks();

                session_.sequence_.updateTotalClocks();
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
            var mDriver = getDriver();
            if (mDriver == null) {
                return;
            }

            if (!mDriver.loaded) {
                return;
            }

            VsqTrack track = session_.sequence_.Track[session_.track_index_];
            int BUFLEN = session_.sample_rate_ / 10;
            float[] left = new float[BUFLEN];
            float[] right = new float[BUFLEN];
            long saProcessed = 0;
            int saRemain = 0;
            int lastClock = 0; // 最後に処理されたゲートタイム

            // 最初にダミーの音を鳴らす
            // (最初に入るノイズを回避するためと、前回途中で再生停止した場合に無音から始まるようにするため)
            mDriver.resetAllParameters();
            mDriver.process(left, right, BUFLEN);
            MidiEvent f_noteon = new MidiEvent();
            f_noteon.firstByte = 0x90;
            f_noteon.data = new int[] { 0x40, 0x40 };
            f_noteon.clock = 0;
            mDriver.send(new MidiEvent[] { f_noteon });
            mDriver.process(left, right, BUFLEN);
            MidiEvent f_noteoff = new MidiEvent();
            f_noteoff.firstByte = 0x80;
            f_noteoff.data = new int[] { 0x40, 0x7F };
            mDriver.send(new MidiEvent[] { f_noteoff });
            for (int i = 0; i < 3; i++) {
                mDriver.process(left, right, BUFLEN);
            }

            // レンダリング開始位置での、パラメータの値をセットしておく
            foreach (var item in track.getNoteEventIterator()) {
                long saNoteStart = (long)(session_.sequence_.getSecFromClock(item.Clock) * session_.sample_rate_);
                long saNoteEnd = (long)(session_.sequence_.getSecFromClock(item.Clock + item.ID.getLength()) * session_.sample_rate_);

                EventQueueSequence list = generateMidiEvent(session_.sequence_, session_.track_index_, lastClock, item.Clock + item.ID.getLength());
                lastClock = item.Clock + item.ID.Length + 1;
                foreach (var clock in list.keyIterator()) {
                    // まず直前までの分を合成
                    long saStart = (long)(session_.sequence_.getSecFromClock(clock) * session_.sample_rate_);
                    saRemain = (int)(saStart - saProcessed);
                    while (saRemain > 0) {
                        int len = saRemain > BUFLEN ? BUFLEN : saRemain;
                        mDriver.process(left, right, len);
                        if (!dispatchRenderedEvent(left, right, len)) {
                            return;
                        }
                        saRemain -= len;
                        saProcessed += len;
                    }

                    // MIDiイベントを送信
                    MidiEventQueue queue = list.get(clock);
                    // まずnoteoff
                    bool noteoff_send = false;
                    if (queue.noteoff.Count > 0) {
                        mDriver.send(queue.noteoff.ToArray());
                        noteoff_send = true;
                    }
                    // parameterの変更
                    if (queue.param.Count > 0) {
                        foreach (var pe in queue.param) {
                            mDriver.setParameter(pe.index, pe.value);
                        }
                    }
                    // ついでnoteon
                    if (queue.noteon.Count > 0) {
                        // 同ゲートタイムにピッチベンドも指定されている場合、同時に送信しないと反映されないようだ！
                        if (queue.pit.Count > 0) {
                            queue.noteon.AddRange(queue.pit);
                            queue.pit.Clear();
                        }
                        mDriver.send(queue.noteon.ToArray());
                    }
                    // PIT
                    if (queue.pit.Count > 0 && !noteoff_send) {
                        mDriver.send(queue.pit.ToArray());
                    }
                    if (mDriver.getUi(main_window_) != null) {
                        mDriver.getUi(main_window_).invalidateUi();
                    }
                }
            }

            while (true) {
                int len = saRemain > BUFLEN ? BUFLEN : saRemain;
                mDriver.process(left, right, len);
                if (!dispatchRenderedEvent(left, right, len)) {
                    return;
                }
            }
        }

        private bool dispatchRenderedEvent(float[] l, float[] r, int length)
        {
            int remain = length;
            while (remain > 0) {
                int amount = (remain > BUFLEN) ? BUFLEN : remain;
                for (int i = 0; i < amount; i++) {
                    session_.left_[i] = l[i];
                    session_.right_[i] = r[i];
                }
                if (!Rendered(session_.left_, session_.right_, amount)) {
                    return false;
                }
                remain -= amount;
            }
            return true;
        }
    }
}
#endif
