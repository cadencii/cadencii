/*
 * GeneratorUnit.cs
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
using System.Threading;

namespace cadencii.dsp.v2.generator
{
    public abstract class GeneratorUnit : AudioUnit
    {
        const int BUFFER_LENGTH_ORDER = 15;
        const int BUFFER_LENGTH = 1 << BUFFER_LENGTH_ORDER;
        const int CHANNELS = 2;

        private IWaveGenerator generator_ = null;
        private RingBuffer<float> left_ = new RingBuffer<float>(BUFFER_LENGTH_ORDER);
        private RingBuffer<float> right_ = new RingBuffer<float>(BUFFER_LENGTH_ORDER);
        private object worker_mutex_ = new object();
        private Thread worker_;
        private bool started_ = false;

        public GeneratorUnit(IWaveGenerator generator)
            : base(CHANNELS, BUFFER_LENGTH)
        {
            generator_ = generator;
        }

        protected abstract void terminated();

        private void start()
        {
            generator_.Rendered += generatorCallback;
            worker_ = new Thread(generator_.start);
            worker_.Start();
            started_ = true;
        }

        public override void render(BusBuffer dest, int samples)
        {
            lock (worker_mutex_) {
                if (!started_) {
                    start();
                }
            }

            int offset = 0;
            int remain = samples;
            while (remain > 0) {
                int amount = 0;
                lock (worker_mutex_) {
                    amount = Math.Min(remain, left_.Count);
                    if (amount == 0) {
                        Thread.Sleep(0);
                    } else {
                        for (int i = 0; i < amount; ++i) {
                            dest[0, i + offset] = left_.pull();
                            dest[1, i + offset] = right_.pull();
                        }
                    }
                }
                offset += amount;
                remain -= amount;
            }
        }

        private bool generatorCallback(float[] left, float[] right, int samples)
        {
            int offset = 0;
            int remain = samples;
            while (remain > 0) {
                int amount = 0;
                lock (worker_mutex_) {
                    if (stopRequired()) {
                        terminated();
                        return false;
                    }
                    amount = Math.Min(remain, left_.Capacity);
                    if (amount == 0) {
                        Thread.Sleep(0);
                    } else {
                        for (int i = 0; i < amount; ++i) {
                            left_.push(left[i + offset]);
                            right_.push(right[i + offset]);
                        }
                    }
                }
                offset += amount;
                remain -= amount;
            }
            return true;
        }
    }
}
