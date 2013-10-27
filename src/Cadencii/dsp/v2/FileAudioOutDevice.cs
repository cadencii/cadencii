/*
 * MonitorAudioOutDevice.cs
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
using cadencii.media;

namespace cadencii.dsp.v2
{
    class FileAudioOutDevice : AudioOutDevice
    {
        private const int CHANNELS = 2;
        private const int BIT_PER_SAMPLE = 16;
        private const int BUFFER_LENGTH = 4096;

        private WaveWriter writer_;
        private float[] left_;
        private float[] right_;

        public FileAudioOutDevice(string file_path, int sample_rate)
        {
            writer_ = new WaveWriter(file_path, CHANNELS, BIT_PER_SAMPLE, sample_rate);
            left_ = new float[BUFFER_LENGTH];
            right_ = new float[BUFFER_LENGTH];
        }

        public void push(BusBuffer buffer, int samples)
        {
            int remain = samples;
            int offset = 0;
            while (remain > 0) {
                int amount = Math.Min(BUFFER_LENGTH, remain);

                const int LEFT = 0;
                const int RIGHT = 1;

                for (int i = 0; i < amount; ++i) {
                    int index = i + offset;
                    left_[i] = buffer[LEFT, index];
                    right_[i] = buffer[RIGHT, index];
                }

                writer_.append(left_, right_, amount);

                remain -= amount;
                offset += amount;
            }
        }

        public void stop()
        {
            writer_.close();
            writer_ = null;
        }
    }
}
