/*
 * BusBuffer.cs
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
    /// <summary>
    /// float, n-channel, interleaved wave buffer.
    /// </summary>
    public class BusBuffer
    {
        /// <summary>
        /// Raw buffer data.
        /// </summary>
        protected float[] buffer_;
        private readonly int length_;
        private readonly int channel_;

        /// <summary>
        /// Initialize buffer.
        /// </summary>
        /// <param name="channel">The number of channels.</param>
        /// <param name="length">The buffer length of the buffer for each channel.</param>
        public BusBuffer(int channel, int length)
        {
            Debug.Assert(channel > 0);
            Debug.Assert(length > 0);
            length_ = length;
            channel_ = channel;
            buffer_ = new float[channel_ * length_];
        }

        /// <summary>
        /// The length of the buffer.
        /// </summary>
        public int Length { get { return length_; } }

        /// <summary>
        /// The number of channels.
        /// </summary>
        public int Channel { get { return channel_; } }

        public float this[int channel, int index]
        {
            get { return buffer_[index * channel_ + channel]; }
            set { buffer_[index * channel_ + channel] = value; }
        }

        /// <summary>
        /// Fill buffer with 0.0f.
        /// </summary>
        /// <param name="offset">The offset index to be filled.</param>
        /// <param name="length">The length to be filled.</param>
        public void zeroFill(int offset, int length)
        {
            int actual_offset = channel_ * offset;
            int actual_length = channel_ * length;
            for (int i = actual_offset; i < actual_offset + actual_length; ++i) {
                buffer_[i] = 0.0f;
            }
        }

        /// <summary>
        /// Mix wave buffer data from the source.
        /// </summary>
        /// <param name="source">Source buffer to mix.</param>
        /// <param name="source_offset">Start index of source buffer.</param>
        /// <param name="offset">Start index of this buffer.</param>
        /// <param name="length">The sample length to be mixed.</param>
        public void mixFrom(BusBuffer source, int source_offset, int offset, int length)
        {
            Debug.Assert(Channel == source.Channel);
            Debug.Assert(0 <= source_offset && source_offset + length <= source.Length);
            Debug.Assert(0 <= offset && offset + length <= Length);
            int channel = source.Channel;
            int actual_soruce_offset = channel * source_offset;
            int actual_offset = channel * offset;
            int actual_length = channel * length;
            for (int i = 0; i < actual_length; ++i) {
                buffer_[actual_offset + i] += source.buffer_[actual_soruce_offset + i];
            }
        }

        internal float[] getRawBuffer()
        {
            return buffer_;
        }
    }
}
