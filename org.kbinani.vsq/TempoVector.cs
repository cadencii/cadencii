/*
 * TempoVector.cs
 * Copyright (C) 2010 kbinani
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
using org.kbinani.java.util;

namespace org.kbinani.vsq {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// テンポ情報を格納したテーブル．
    /// </summary>
#if JAVA
    public class TempoVector extends Vector<TempoTableEntry>, Serializable {
#else
    [Serializable]
    public class TempoVector : Vector<TempoTableEntry> {
#endif
        protected const int gatetimePerQuater = 480;
        protected const int baseTempo = 500000; 

        public TempoVector()
            : base() {
        }

        public double getClockFromSec( double time ) {
            // timeにおけるテンポを取得
            int tempo = baseTempo;
            double base_clock = 0;
            double base_time = 0f;
            int c = size();
            if ( c == 0 ) {
                tempo = baseTempo;
                base_clock = 0;
                base_time = 0f;
            } else if ( c == 1 ) {
                tempo = get( 0 ).Tempo;
                base_clock = get( 0 ).Clock;
                base_time = get( 0 ).Time;
            } else {
                for ( int i = c - 1; i >= 0; i-- ) {
                    TempoTableEntry item = get( i );
                    if ( item.Time < time ) {
                        return item.Clock + (time - item.Time) * gatetimePerQuater * 1000000.0 / item.Tempo;
                    }
                }
            }
            double dt = time - base_time;
            return base_clock + dt * gatetimePerQuater * 1000000.0 / (double)tempo;
        }

        public void updateTempoInfo() {
            int c = size();
            if ( c == 0 ) {
                add( new TempoTableEntry( 0, baseTempo, 0.0 ) );
            }
            Collections.sort( this );
            TempoTableEntry item0 = get( 0 );
            if ( item0.Clock != 0 ) {
                item0.Time = (double)baseTempo * (double)item0.Clock / (gatetimePerQuater * 1000000.0);
            } else {
                item0.Time = 0.0;
            }
            double prev_time = item0.Time;
            int prev_clock = item0.Clock;
            int prev_tempo = item0.Tempo;
            double inv_tpq_sec = 1.0 / (gatetimePerQuater * 1000000.0);
            for ( int i = 1; i < c; i++ ) {
                TempoTableEntry itemi = get( i );
                itemi.Time = prev_time + (double)prev_tempo * (double)(itemi.Clock - prev_clock) * inv_tpq_sec;
                prev_time = itemi.Time;
                prev_tempo = itemi.Tempo;
                prev_clock = itemi.Clock;
            }
        }

        public double getSecFromClock( double clock ) {
            int c = size();
            for ( int i = c - 1; i >= 0; i-- ) {
                TempoTableEntry item = get( i );
                if ( item.Clock < clock ) {
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

#if !JAVA
}
#endif
