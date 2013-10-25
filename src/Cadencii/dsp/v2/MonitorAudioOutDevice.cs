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
using cadencii.media;

namespace cadencii.dsp.v2
{
    class MonitorAudioOutDevice : AudioOutDevice
    {
        public MonitorAudioOutDevice(int sample_rate)
        {
            PlaySound.init();
            PlaySound.prepare(sample_rate);
        }

        public void push(BusBuffer buffer, int samples)
        {
            PlaySound.appendInterleaved(buffer.getRawBuffer(), samples);
        }

        public void stop()
        {
            PlaySound.waitForExit();
            PlaySound.unprepare();
        }
    }
}
