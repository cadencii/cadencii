/*
 * RingBuffer.cs
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
namespace cadencii.dsp.v2
{
    class RingBuffer<T>
    {
        private readonly int capacity_;
        private readonly int mask_;
        private T[] buffer_;
        private int top_;
        private int bottom_;

        public RingBuffer(int capacity_order)
        {
            capacity_ = 1 << capacity_order;
            buffer_ = new T[capacity_];
            top_ = 0;
            bottom_ = 0;
            mask_ = capacity_ - 1;
        }

        public int Count { get { return bottom_ - top_; } }

        public int Capacity { get { return capacity_ - Count; } }

        public void push(T value)
        {
            buffer_[bottom_ & mask_] = value;
            ++bottom_;
            if (top_ > capacity_ && bottom_ > capacity_) {
                top_ -= capacity_;
                bottom_ -= capacity_;
            }
        }

        public T pull()
        {
            T result = buffer_[top_ & mask_];
            ++top_;
            return result;
        }
    }
}
