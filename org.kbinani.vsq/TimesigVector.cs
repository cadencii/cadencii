/*
 * TimesigVector.cs
 * Copyright © 2011 kbinani
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
import java.util.*;
import org.kbinani.*;

#else

using System;
using org.kbinani.java.util;

namespace org.kbinani.vsq
{
    using Integer = System.Int32;
#endif

#if JAVA
    public class TimesigVector extends Vector<TimeSigTableEntry> implements Serializable
#else
    [Serializable]
    public class TimesigVector : Vector<TimeSigTableEntry>
#endif
    {
        /// <summary>
        /// TimeSigTableの[*].Clockの部分を更新します
        /// </summary>
        public void updateTimesigInfo()
        {
#if DEBUG
            sout.println( "TimesigVector#updateTimesigInfo; before:" );
            for ( int i = 0; i < size(); i++ ) {
                sout.println( "    " + get( i ).Clock + " " + get( i ).Numerator + "/" + get( i ).Denominator );
            }
#endif
            if ( get( 0 ).Clock != 0 ) {
                return;
            }
            get( 0 ).Clock = 0;
            Collections.sort( this );
            int count = size();
            for ( int j = 1; j < count; j++ ) {
                TimeSigTableEntry item = get( j - 1 );
                int numerator = item.Numerator;
                int denominator = item.Denominator;
                int clock = item.Clock;
                int bar_count = item.BarCount;
                int dif = 480 * 4 / denominator * numerator;//1小節が何クロックか？
                clock += (get( j ).BarCount - bar_count) * dif;
                get( j ).Clock = clock;
            }
#if DEBUG
            sout.println( "TimesigVector#updateTimesigInfo; after:" );
            for ( int i = 0; i < size(); i++ ) {
                sout.println( "    " + get( i ).Clock + " " + get( i ).Numerator + "/" + get( i ).Denominator );
            }
#endif
        }

        public Timesig getTimesigAt( int clock )
        {
            Timesig ret = new Timesig();
            ret.numerator = 4;
            ret.denominator = 4;
            int index = 0;
            int c = size();
            for ( int i = c - 1; i >= 0; i-- ) {
                index = i;
                if ( get( i ).Clock <= clock ) {
                    break;
                }
            }
            ret.numerator = get( index ).Numerator;
            ret.denominator = get( index ).Denominator;
            return ret;
        }

        public Timesig getTimesigAt( int clock, ByRef<Integer> bar_count )
        {
            int index = 0;
            int c = size();
            for ( int i = c - 1; i >= 0; i-- ) {
                index = i;
                if ( get( i ).Clock <= clock ) {
                    break;
                }
            }
            TimeSigTableEntry item = get( index );
            Timesig ret = new Timesig();
            ret.numerator = item.Numerator;
            ret.denominator = item.Denominator;
            int diff = clock - item.Clock;
            int clock_per_bar = 480 * 4 / ret.denominator * ret.numerator;
            bar_count.value = item.BarCount + diff / clock_per_bar;
            return ret;
        }

        /// <summary>
        /// 指定した小節の開始クロックを調べます。ここで使用する小節数は、プリメジャーを考慮しない。即ち、曲頭の小節が0である。
        /// </summary>
        /// <param name="bar_count"></param>
        /// <returns></returns>
        public int getClockFromBarCount( int bar_count )
        {
            int index = 0;
            int c = size();
            for ( int i = c - 1; i >= 0; i-- ) {
                index = i;
                if ( get( i ).BarCount <= bar_count ) {
                    break;
                }
            }
            TimeSigTableEntry item = get( index );
            int numerator = item.Numerator;
            int denominator = item.Denominator;
            int init_clock = item.Clock;
            int init_bar_count = item.BarCount;
            int clock_per_bar = numerator * 480 * 4 / denominator;
            return init_clock + (bar_count - init_bar_count) * clock_per_bar;
        }

        /// <summary>
        /// 指定したクロックが、曲頭から何小節目に属しているかを調べます。ここで使用する小節数は、プリメジャーを考慮しない。即ち、曲頭の小節が0である。
        /// </summary>
        /// <param name="clock"></param>
        /// <returns></returns>
        public int getBarCountFromClock( int clock )
        {
            int index = 0;
            int c = size();
            for ( int i = c - 1; i >= 0; i-- ) {
                index = i;
                if ( get( i ).Clock <= clock ) {
                    break;
                }
            }
            int bar_count = 0;
            if ( index >= 0 ) {
                TimeSigTableEntry item = get( index );
                int last_clock = item.Clock;
                int t_bar_count = item.BarCount;
                int numerator = item.Numerator;
                int denominator = item.Denominator;
                int clock_per_bar = numerator * 480 * 4 / denominator;
                bar_count = t_bar_count + (clock - last_clock) / clock_per_bar;
            }
            return bar_count;
        }
    }

#if !JAVA
}
#endif
