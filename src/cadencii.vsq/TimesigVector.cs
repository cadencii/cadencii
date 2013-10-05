/*
 * TimesigVector.cs
 * Copyright © 2011 kbinani
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
using System.Collections.Generic;
using cadencii.java.util;

namespace cadencii.vsq
{

    [Serializable]
    public class TimesigVector : List<TimeSigTableEntry>
    {
        /// <summary>
        /// TimeSigTableの[*].Clockの部分を更新します
        /// </summary>
        public void updateTimesigInfo()
        {
#if DEBUG
            sout.println("TimesigVector#updateTimesigInfo; before:");
            for (int i = 0; i < Count; i++) {
                sout.println("    " + this[i].Clock + " " + this[i].Numerator + "/" + this[i].Denominator);
            }
#endif
            if (this[0].Clock != 0) {
                return;
            }
            this[0].Clock = 0;
            this.Sort();
            int count = Count;
            for (int j = 1; j < count; j++) {
                TimeSigTableEntry item = this[j - 1];
                int numerator = item.Numerator;
                int denominator = item.Denominator;
                int clock = item.Clock;
                int bar_count = item.BarCount;
                int dif = 480 * 4 / denominator * numerator;//1小節が何クロックか？
                clock += (this[j].BarCount - bar_count) * dif;
                this[j].Clock = clock;
            }
#if DEBUG
            sout.println("TimesigVector#updateTimesigInfo; after:");
            for (int i = 0; i < Count; i++) {
                sout.println("    " + this[i].Clock + " " + this[i].Numerator + "/" + this[i].Denominator);
            }
#endif
        }

        public Timesig getTimesigAt(int clock)
        {
            Timesig ret = new Timesig();
            ret.numerator = 4;
            ret.denominator = 4;
            int index = 0;
            int c = Count;
            for (int i = c - 1; i >= 0; i--) {
                index = i;
                if (this[i].Clock <= clock) {
                    break;
                }
            }
            ret.numerator = this[index].Numerator;
            ret.denominator = this[index].Denominator;
            return ret;
        }

        public Timesig getTimesigAt(int clock, ByRef<int> bar_count)
        {
            int index = 0;
            int c = Count;
            for (int i = c - 1; i >= 0; i--) {
                index = i;
                if (this[i].Clock <= clock) {
                    break;
                }
            }
            TimeSigTableEntry item = this[index];
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
        public int getClockFromBarCount(int bar_count)
        {
            int index = 0;
            int c = Count;
            for (int i = c - 1; i >= 0; i--) {
                index = i;
                if (this[i].BarCount <= bar_count) {
                    break;
                }
            }
            TimeSigTableEntry item = this[index];
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
        public int getBarCountFromClock(int clock)
        {
            int index = 0;
            int c = Count;
            for (int i = c - 1; i >= 0; i--) {
                index = i;
                if (this[i].Clock <= clock) {
                    break;
                }
            }
            int bar_count = 0;
            if (index >= 0) {
                TimeSigTableEntry item = this[index];
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

}
