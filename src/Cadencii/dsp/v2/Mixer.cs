/*
 * Mixer.cs
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
using System.Diagnostics;

namespace cadencii.dsp.v2
{
    sealed class Mixer : AudioUnit
    {
        List<AudioUnit> parents_ = new List<AudioUnit>();
        BusBuffer buffer_;

        public Mixer(int channel, int buffer_size = 4096)
            : base(channel, buffer_size)
        {
            buffer_ = new BusBuffer(channel, buffer_size);
        }

        public void addParent(AudioUnit parent)
        {
            Debug.Assert(parent != null);
            parents_.Add(parent);
        }

        public override void render(BusBuffer dest, int samples)
        {
            int remain = samples;
            int offset = 0;
            while (remain > 0) {
                int amount = Math.Min(buffer_.Length, remain);
                parents_.ForEach((unit) => {
                    unit.pullReplacing(buffer_, 0, amount);
                    dest.mixFrom(buffer_, 0, offset, amount);
                });
                remain -= amount;
                offset += amount;
            }
        }

        internal override void terminate()
        {
            base.terminate();
            parents_.ForEach(_ => _.terminate());
        }
    }
}
