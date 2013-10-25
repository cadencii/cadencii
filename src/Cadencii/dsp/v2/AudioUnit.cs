/*
 * AudioUnit.cs
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
using System.Diagnostics;
using System.Linq;

namespace cadencii.dsp.v2
{
    public abstract class AudioUnit
    {
        protected internal AudioUnit parent_;
        private readonly BusBuffer buffer_;
        private long current_render_sample_;
        private bool stop_required_ = false;
        private Mixer mixer_ = null;

        internal AudioUnit(int channel, int buffer_size = 4096)
        {
            buffer_ = new BusBuffer(channel, buffer_size);
        }

        public abstract void render(BusBuffer dest, int samples);

        public int Channel { get { return buffer_.Channel; } }
        public int BufferSize { get { return buffer_.Length; } }
        protected long CurrentRenderSample { get { return current_render_sample_; } }

        internal void setParent(AudioUnit unit)
        {
            parent_ = unit;
        }

        internal void pullReplacing(BusBuffer buffer, int offset, int length)
        {
            Debug.Assert(0 <= offset && offset + length <= buffer.Length);

            if (parent_ == null) {
                buffer.zeroFill(offset, length);
            }

            int current_offset = offset;
            int remain = length;
            while (remain > 0) {
                int amount = Math.Min(buffer_.Length, remain);
                
                if (parent_ != null) {
                    parent_.pullReplacing(buffer, offset, amount);
                }

                buffer_.zeroFill(0, amount);
                render(buffer_, amount);
                current_render_sample_ += amount;

                buffer.mixFrom(buffer_, 0, current_offset, amount);
                
                remain -= amount;
                current_offset += amount;
            }
        }

        internal virtual void terminate()
        {
            if (parent_ != null) {
                parent_.terminate();
            }
            stop_required_ = true;
        }

        protected bool stopRequired()
        {
            return stop_required_;
        }

        public AudioUnit connect(AudioUnit parent_unit)
        {
            setParent(parent_unit);
            return this;
        }

        public AudioUnit connect(params AudioUnit[] parent_units)
        {
            if (mixer_ != null) {
                mixer_.terminate();
            }
            mixer_ = new Mixer(Channel, BufferSize);
            parent_units.ToList().ForEach((_1) => mixer_.addParent(_1));
            setParent(mixer_);
            return this;
        }
    }
}
