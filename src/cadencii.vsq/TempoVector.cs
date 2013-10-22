/*
 * TempoVector.cs
 * Copyright © 2010-2011 kbinani
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

#if DEBUG
    class TestTempoVector
    {
        public void run()
        {
            TempoVector tv = new TempoVector();
            tv.Add(new TempoTableEntry(0, 500000, 0.0));
            tv.Add(new TempoTableEntry(1920, 500000, 0.0));
            tv.Add(new TempoTableEntry(3820, 500000, 0.0));
            tv.updateTempoInfo();
            for (int i = 0; i < tv.Count; i++) {
                TempoTableEntry itemi = tv[i];
                sout.println("   #" + i + "; " + itemi.Clock + "; " + itemi.Time + "; " + (60e6 / itemi.Tempo));
            }
            TempoVectorSearchContext c = new TempoVectorSearchContext();
            sout.println("getClockFromSec; time=1.0; c.mSec2ClockIndex=" + c.mSec2ClockIndex + "; c.mSec2ClockSec=" + c.mSec2ClockSec);
            double cl = tv.getClockFromSec(1.0, c);
            sout.println("cl=" + cl + "; c.mSec2ClockIndex=" + c.mSec2ClockIndex + "; c.mSec2ClockSec=" + c.mSec2ClockSec);

            sout.println("getClockFromSec; time=2.5; c.mSec2ClockIndex=" + c.mSec2ClockIndex + "; c.mSec2ClockSec=" + c.mSec2ClockSec);
            cl = tv.getClockFromSec(2.5, c);
            sout.println("cl=" + cl + "; c.mSec2ClockIndex=" + c.mSec2ClockIndex + "; c.mSec2ClockSec=" + c.mSec2ClockSec);

            sout.println("getClockFromSec; time=4.0; c.mSec2ClockIndex=" + c.mSec2ClockIndex + "; c.mSec2ClockSec=" + c.mSec2ClockSec);
            cl = tv.getClockFromSec(4.0, c);
            sout.println("cl=" + cl + "; c.mSec2ClockIndex=" + c.mSec2ClockIndex + "; c.mSec2ClockSec=" + c.mSec2ClockSec);
        }
    }
#endif

    /// <summary>
    /// テンポ情報を格納したテーブル．
    /// </summary>
    [Serializable]
    public class TempoVector : List<TempoTableEntry>, ITempoMaster
    {
        /// <summary>
        /// 4分音符1拍あたりのゲートタイム
        /// </summary>
        protected const int gatetimePerQuater = 480;
        /// <summary>
        /// デフォルトのテンポ値(4分音符1拍あたりのマイクロ秒)
        /// </summary>
        protected const int baseTempo = 500000;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TempoVector()
            : base()
        {
        }

        /// <summary>
        /// 指定した時刻におけるゲートタイムを取得します
        /// </summary>
        /// <param name="time">ゲートタイムを取得する時刻(秒)</param>
        /// <returns>ゲートタイム</returns>
        public double getClockFromSec(double time)
        {
            return getClockFromSec(time, null);
        }

        /// <summary>
        /// 指定した時刻におけるゲートタイムを取得します．
        /// このメソッドでは検索コンテキストを用い，取得したいtimeの値が順に大きくなる状況でこのメソッドの実行速度の高速化を図ります
        /// </summary>
        /// <param name="time">ゲートタイムを取得する時刻(秒)</param>
        /// <param name="context">計算を高速化するための検索コンテキスト</param>
        /// <returns>ゲートタイム</returns>
        public double getClockFromSec(double time, TempoVectorSearchContext context)
        {
            int tempo = baseTempo;
            double base_clock = 0;
            double base_time = 0.0;
            int c = Count;
            if (c == 0) {
                tempo = baseTempo;
                base_clock = 0;
                base_time = 0.0;
            } else if (c == 1) {
                TempoTableEntry t0 = this[0];
                tempo = t0.Tempo;
                base_clock = t0.Clock;
                base_time = t0.Time;
            } else {
                int i0 = 0;
                if (context != null) {
                    if (time >= context.mSec2ClockSec) {
                        // 探そうとしている時刻が前回検索時の時刻と同じかそれ以降の場合
                        i0 = context.mSec2ClockIndex;
                    } else {
                        // リセットする
                        context.mSec2ClockIndex = 0;
                    }
                    context.mSec2ClockSec = time;
                }
                TempoTableEntry prev = null;
                for (int i = i0; i < c; i++) {
                    TempoTableEntry item = this[i];
                    if (time <= item.Time) {
                        if (context != null) {
                            context.mSec2ClockIndex = i > 0 ? i - 1 : 0;
                        }
                        break;
                    }
                    prev = item;
                }
                if (prev != null) {
                    base_time = prev.Time;
                    base_clock = prev.Clock;
                    tempo = prev.Tempo;
                }
            }
            double dt = time - base_time;
            return base_clock + dt * gatetimePerQuater * 1000000.0 / (double)tempo;
        }

        /// <summary>
        /// このテーブルに登録されているテンポ変更イベントのうち、時刻に関する情報を再計算します。
        /// 新しいテンポ変更イベントを登録したり、既存のイベントを変更した場合に、都度呼び出す必要があります
        /// </summary>
        public void updateTempoInfo()
        {
            int c = Count;
            if (c == 0) {
                Add(new TempoTableEntry(0, baseTempo, 0.0));
            }
            this.Sort();
            TempoTableEntry item0 = this[0];
            if (item0.Clock != 0) {
                item0.Time = (double)baseTempo * (double)item0.Clock / (gatetimePerQuater * 1000000.0);
            } else {
                item0.Time = 0.0;
            }
            double prev_time = item0.Time;
            int prev_clock = item0.Clock;
            int prev_tempo = item0.Tempo;
            double inv_tpq_sec = 1.0 / (gatetimePerQuater * 1000000.0);
            for (int i = 1; i < c; i++) {
                TempoTableEntry itemi = this[i];
                itemi.Time = prev_time + (double)prev_tempo * (double)(itemi.Clock - prev_clock) * inv_tpq_sec;
                prev_time = itemi.Time;
                prev_tempo = itemi.Tempo;
                prev_clock = itemi.Clock;
            }
        }

        /// <summary>
        /// 指定したゲートタイムにおける時刻を取得します
        /// </summary>
        /// <param name="clock">時刻を取得するゲートタイム</param>
        /// <returns>時刻(秒)</returns>
        public double getSecFromClock(double clock)
        {
            int c = Count;
            for (int i = c - 1; i >= 0; i--) {
                TempoTableEntry item = this[i];
                if (item.Clock <= clock) {
                    double init = item.Time;
                    double dclock = clock - item.Clock;
                    double sec_per_clock1 = item.Tempo * 1e-6 / 480.0;
                    return init + dclock * sec_per_clock1;
                }
            }

            double sec_per_clock = baseTempo * 1e-6 / 480.0;
            return clock * sec_per_clock;
        }
    }

}
