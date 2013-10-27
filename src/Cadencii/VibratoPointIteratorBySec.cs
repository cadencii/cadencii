/*
 * VibratoPointIteratorBySec.cs
 * Copyright © 2010-2011 kbinani
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
using cadencii.vsq;
using cadencii.java.util;

namespace cadencii
{

    /// <summary>
    /// ビブラート用のデータ点のリストを取得します。返却されるリストは、{秒, ビブラートの振幅(ノートナンバー単位)}の値ペアとなっています
    /// </summary>
    public class VibratoPointIteratorBySec
    {
        VsqFile vsq;
        VibratoBPList rate;
        int start_rate;
        VibratoBPList depth;
        int start_depth;
        int clock_start;
        int clock_width;
        float sec_resolution;

        double sec0;
        double sec1;
        int count = 0;
        double phase = 0.0;
        double amplitude;
        float period;
        float omega;
        double sec;
        float fadewidth;
        int i;
        bool first = true;

        // Rate値から周期を高速に求めるためのキャッシュ
        private static float[] mVibratoPeriod = null;

        /// <summary>
        /// VibratoRate値からビブラートの周期を求めます。単位は秒
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static float getPeriodFromRate(int rate)
        {
            if (mVibratoPeriod == null) {
                // キャッシュが初期化されていない場合は初期化
                mVibratoPeriod = new float[128];
                for (int r = 0; r < 128; r++) {
                    mVibratoPeriod[r] = (float)Math.Exp(5.24 - 1.07e-2 * r) * 2.0f / 1000.0f;
                }
            }
            if (rate < 0 || 128 <= rate) {
                // 範囲外のRate値の場合はいちいち計算。ほんとはだめだけどー
                return (float)Math.Exp(5.24 - 1.07e-2 * rate) * 2.0f / 1000.0f;
            } else {
                // 範囲内のRate値の場合はキャッシュの値を返すだけ
                return mVibratoPeriod[rate];
            }
        }

        public PointD next()
        {
            if (first) {
                i = 0;
                first = false;
                return new PointD(sec0, 0);
            } else {
                i++;
                if (i < count) {
                    double t_sec = sec0 + sec_resolution * i;
                    double clock = vsq.getClockFromSec(t_sec);
                    if (sec0 <= t_sec && t_sec <= sec0 + fadewidth) {
                        amplitude *= (float)(t_sec - sec0) / fadewidth;
                    }
                    if (sec1 - fadewidth <= t_sec && t_sec <= sec1) {
                        amplitude *= (float)(sec1 - t_sec) / fadewidth;
                    }
                    phase += omega * (t_sec - sec);
                    PointD ret = new PointD(t_sec, amplitude * Math.Sin(phase));
                    float v = (float)(clock - clock_start) / (float)clock_width;
                    int r = rate.getValue(v, start_rate);
                    int d = depth.getValue(v, start_depth);
                    amplitude = d * 2.5f / 127.0f / 2.0f;
                    period = getPeriodFromRate(r);
                    omega = (float)(2.0 * Math.PI / period);
                    sec = t_sec;
                    return ret;
                } else {
                    return new PointD();
                }
            }
        }

        public bool hasNext()
        {
            if (first) {
                return true;
            } else {
                return (i + 1 < count);
            }
        }

        public void remove()
        {
        }

        public VibratoPointIteratorBySec(VsqFile vsq,
                                         VibratoBPList rate,
                                         int start_rate,
                                         VibratoBPList depth,
                                         int start_depth,
                                         int clock_start,
                                         int clock_width,
                                         float sec_resolution)
        {
            this.vsq = vsq;
            this.rate = rate;
            this.start_rate = start_rate;
            this.depth = depth;
            this.start_depth = start_depth;
            this.clock_start = clock_start;
            this.clock_width = clock_width;
            this.sec_resolution = sec_resolution;

            sec0 = vsq.getSecFromClock(clock_start);
            sec1 = vsq.getSecFromClock(clock_start + clock_width);
            count = (int)((sec1 - sec0) / sec_resolution);
            phase = 0;
            start_rate = rate.getValue(0.0f, start_rate);
            start_depth = depth.getValue(0.0f, start_depth);
            amplitude = start_depth * 2.5f / 127.0f / 2.0f; // ビブラートの振幅。
            period = getPeriodFromRate(start_rate); //ビブラートの周期、秒
            omega = (float)(2.0 * Math.PI / period); // 角速度(rad/sec)
            sec = sec0;
            fadewidth = (float)(sec1 - sec0) * 0.2f;
        }
    }

}
