/*
 * TimeSigTableEntry.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.io.*;
#else
using System;
using org.kbinani;

namespace org.kbinani.vsq
{
#endif

#if JAVA
    public class TimeSigTableEntry implements Comparable<TimeSigTableEntry>, Cloneable, Serializable
#else
    [Serializable]
    public class TimeSigTableEntry : IComparable<TimeSigTableEntry>, ICloneable
#endif
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
            int bar_count )
        {
            Clock = clock;
            Numerator = numerator;
            Denominator = denominator;
            BarCount = bar_count;
        }

        public TimeSigTableEntry()
        {
        }

        public override String ToString()
        {
            return "{Clock=" + Clock + ", Numerator=" + Numerator + ", Denominator=" + Denominator + ", BarCount=" + BarCount + "}";
        }

        public Object clone()
        {
            return new TimeSigTableEntry( Clock, Numerator, Denominator, BarCount );
        }

#if !JAVA
        public object Clone()
        {
            return clone();
        }
#endif

        public int compareTo( TimeSigTableEntry item )
        {
            return this.BarCount - item.BarCount;
        }

#if !JAVA
        public int CompareTo( TimeSigTableEntry item )
        {
            return compareTo( item );
        }
#endif
    }

#if !JAVA
}
#endif
