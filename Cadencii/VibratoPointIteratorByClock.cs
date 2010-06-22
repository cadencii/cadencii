/*
 * VibratoPointItertorByClock.cs
 * Copyright (C) 2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

import java.util.*;
import org.kbinani.vsq.*;
#else
using System;
using org.kbinani.vsq;
using org.kbinani.java.util;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// ビブラート用のデータ点のリストを取得します。返却されるリストは、{クロック, ビブラートの振幅(ノートナンバー単位)}の値ペアとなっています
    /// </summary>
#if JAVA
    public class VibratoPointIteratorByClock implements Iterator<Double> {
#else
    public class VibratoPointIteratorByClock : Iterator<Double> {
#endif
        VsqFileEx vsq;
        VibratoBPList rate;
        int start_rate;
        VibratoBPList depth;
        int start_depth;
        int clock_start;
        int clock_width;
        int clock_resolution;

        double sec0;
        double sec1;
        double phase = 0.0;
        double amplitude;
        float period;
        float omega;
        double sec;
        float fadewidth;
        int i;
        boolean first = true;

        public Double next() {
            if ( first ) {
                i = 0;
                first = false;
                return 0.0;
            } else {
                i++;
                if ( i < clock_width ) {
                    int clock = clock_start + i;
                    double t_sec = vsq.getSecFromClock( clock );
                    if ( sec0 <= t_sec && t_sec <= sec0 + fadewidth ) {
                        amplitude *= (t_sec - sec0) / fadewidth;
                    }
                    if ( sec1 - fadewidth <= t_sec && t_sec <= sec1 ) {
                        amplitude *= (sec1 - t_sec) / fadewidth;
                    }
                    phase += omega * (t_sec - sec);
                    double ret = amplitude * Math.Sin( phase );
                    float v = (float)(clock - clock_start) / (float)clock_width;
                    int r = rate.getValue( v, start_rate );
                    int d = depth.getValue( v, start_depth );
                    amplitude = d * 2.5f / 127.0f / 2.0f;
                    period = (float)Math.Exp( 5.24 - 1.07e-2 * r ) * 2.0f / 1000.0f;
                    omega = (float)(2.0 * Math.PI / period);
                    sec = t_sec;
                    return ret;
                } else {
                    return 0.0;
                }
            }
        }

        public boolean hasNext() {
            if ( first ) {
                return true;
            } else {
                return (i < clock_width);
            }
        }

        public void remove() {
        }

        public VibratoPointIteratorByClock( VsqFileEx vsq,
                                      VibratoBPList rate,
                                      int start_rate,
                                      VibratoBPList depth,
                                      int start_depth,
                                      int clock_start,
                                      int clock_width,
                                      int clock_resolution ) {
            this.vsq = vsq;
            this.rate = rate;
            this.start_rate = start_rate;
            this.depth = depth;
            this.start_depth = start_depth;
            this.clock_start = clock_start;
            this.clock_width = clock_width;
            this.clock_resolution = clock_resolution;

            sec0 = vsq.getSecFromClock( clock_start );
            sec1 = vsq.getSecFromClock( clock_start + clock_width );
            phase = 0;
            start_rate = rate.getValue( 0.0f, start_rate );
            start_depth = depth.getValue( 0.0f, start_depth );
            amplitude = start_depth * 2.5f / 127.0f / 2.0f; // ビブラートの振幅。
            period = (float)Math.Exp( 5.24 - 1.07e-2 * start_rate ) * 2.0f / 1000.0f; //ビブラートの周期、秒
            omega = (float)(2.0 * Math.PI / period); // 角速度(rad/sec)
            sec = sec0;
            fadewidth = (float)(sec1 - sec0) * 0.2f;
        }
    }

#if !JAVA
}
#endif
