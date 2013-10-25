/*
 * AudioUnitRunner.cs
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
using System.Diagnostics;

namespace cadencii.dsp.v2
{
    public sealed class AudioUnitRunner
    {
        private AudioUnit first_unit_;
        private AudioOutDevice device_;
        private object mutex_ = new object();
        private bool stop_requested_ = false;

        public AudioUnitRunner(AudioUnit first_unit, AudioOutDevice device)
        {
            Debug.Assert(first_unit != null);
            Debug.Assert(device != null);

            first_unit_ = first_unit;
            device_ = device;
        }

        public void start()
        {
            BusBuffer buffer = new BusBuffer(first_unit_.Channel, first_unit_.BufferSize);
            while (true) {
                lock (mutex_) {
                    if (stop_requested_) {
                        break;
                    }
                }
                first_unit_.pullReplacing(buffer, 0, buffer.Length);
                device_.push(buffer, buffer.Length);
            }
        }

        public void stop()
        {
            lock (mutex_) {
                stop_requested_ = true;
            }
            device_.stop();
            first_unit_.terminate();
        }
    }
}
