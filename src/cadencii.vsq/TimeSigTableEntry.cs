/*
 * TimeSigTableEntry.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using cadencii;

namespace cadencii.vsq
{

    [Serializable]
    public class TimeSigTableEntry : IComparable<TimeSigTableEntry>, ICloneable
    {
        /// <summary>
        /// クロック数
        /// </summary>
        public int Clock;
        /// <summary>
        /// 拍子の分子
        /// </summary>
        public int Numerator;
        /// <summary>
        /// 拍子の分母
        /// </summary>
        public int Denominator;
        /// <summary>
        /// 何小節目か
        /// </summary>
        public int BarCount;

        public TimeSigTableEntry(
            int clock,
            int numerator,
            int denominator,
            int bar_count)
        {
            Clock = clock;
            Numerator = numerator;
            Denominator = denominator;
            BarCount = bar_count;
        }

        public TimeSigTableEntry()
        {
        }

        public override string ToString()
        {
            return "{Clock=" + Clock + ", Numerator=" + Numerator + ", Denominator=" + Denominator + ", BarCount=" + BarCount + "}";
        }

        public Object clone()
        {
            return new TimeSigTableEntry(Clock, Numerator, Denominator, BarCount);
        }

        public object Clone()
        {
            return clone();
        }

        public int compareTo(TimeSigTableEntry item)
        {
            return this.BarCount - item.BarCount;
        }

        public int CompareTo(TimeSigTableEntry item)
        {
            return compareTo(item);
        }
    }

}
