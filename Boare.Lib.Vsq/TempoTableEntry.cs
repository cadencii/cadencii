/*
 * TempoTableEntry.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;

using bocoree;

namespace Boare.Lib.Vsq {

    using boolean = System.Boolean;

    public class TempoTable : ICloneable {
        private struct TempoTableEntry : IComparable<TempoTableEntry> {
            public int Clock;
            public int Tempo;
            public double Time;

            public TempoTableEntry( int clock, int tempo, double time ) {
                Clock = clock;
                Tempo = tempo;
                Time = time;
            }

            public int CompareTo( TempoTableEntry item ) {
                return Clock - item.Clock;
            }
        }

        private Vector<TempoTableEntry> m_tempo_table;
        private int m_base_tempo;
        private int m_tpq;

        private TempoTable() {
        }

        public TempoTable( int base_tempo, int clock_per_quoter ) {
            m_base_tempo = base_tempo;
            m_tpq = clock_per_quoter;
            m_tempo_table = new Vector<TempoTableEntry>();
            m_tempo_table.add( new TempoTableEntry( 0, base_tempo, 0.0 ) );
        }

        public object Clone() {
            TempoTable ret = new TempoTable();
            ret.m_base_tempo = m_base_tempo;
            ret.m_tpq = m_tpq;
            ret.m_tempo_table = new Vector<TempoTableEntry>();
            for ( int i = 0; i < m_tempo_table.size(); i++ ) {
                ret.m_tempo_table.add( m_tempo_table.get( i ) );
            }
            ret.update();
            return ret;
        }

        public void add( int clock, int tempo ) {
            boolean found = false;
            for ( int i = 0; i < m_tempo_table.size(); i++ ) {
                if ( m_tempo_table.get( i ).Clock == clock ) {
                    found = true;
                    m_tempo_table.set( i, new TempoTableEntry( clock, tempo, 0.0 ) );
                    break;
                }
            }
            if ( !found ) {
                m_tempo_table.add( new TempoTableEntry( clock, tempo, 0.0 ) );
            }
            Collections.sort( m_tempo_table );
            update();
        }

        public void clear( int base_tempo ) {
            m_tempo_table.clear();
            m_tempo_table.add( new TempoTableEntry( 0, base_tempo, 0.0 ) );
        }

        private void update() {
            for ( int i = 0; i < m_tempo_table.size(); i++ ) {
                long sum = 0;
                for ( int k = 0; k < i; k++ ) {
                    sum += (m_tempo_table.get( k ).Tempo * (m_tempo_table.get( k + 1 ).Clock - m_tempo_table.get( k ).Clock));
                }
                double time = sum / (m_tpq * 1e6);
                m_tempo_table.set( i, new TempoTableEntry( m_tempo_table.get( i ).Clock, m_tempo_table.get( i ).Tempo, time ) );
            }
        }

        /// <summary>
        /// 指定した時刻における、クロックを取得します
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public double getClockFromSec( double time ) {
            // timeにおけるテンポを取得
            int tempo = m_base_tempo;
            double base_clock = 0;
            double base_time = 0f;
            if ( m_tempo_table.size() == 0 ) {
                tempo = m_base_tempo;
                base_clock = 0;
                base_time = 0f;
            } else if ( m_tempo_table.size() == 1 ) {
                tempo = m_tempo_table.get( 0 ).Tempo;
                base_clock = m_tempo_table.get( 0 ).Clock;
                base_time = m_tempo_table.get( 0 ).Time;
            } else {
                for ( int i = m_tempo_table.size() - 1; i >= 0; i-- ) {
                    if ( m_tempo_table.get( i ).Time < time ) {
                        return m_tempo_table.get( i ).Clock + (time - m_tempo_table.get( i ).Time) * m_tpq * 1000000.0 / m_tempo_table.get( i ).Tempo;
                    }
                }
            }
            double dt = time - base_time;
            return base_clock + dt * m_tpq * 1000000.0 / (double)tempo;
        }

        /// <summary>
        /// 指定したクロックにおける、clock=0からの演奏経過時間(sec)を取得します
        /// </summary>
        /// <param name="clock"></param>
        /// <returns></returns>
        public double getSecFromClock( int clock ) {
            for ( int i = m_tempo_table.size() - 1; i >= 0; i-- ) {
                if ( m_tempo_table.get( i ).Clock < clock ) {
                    double init = m_tempo_table.get( i ).Time;
                    int dclock = clock - m_tempo_table.get( i ).Clock;
                    double sec_per_clock1 = m_tempo_table.get( i ).Tempo * 1e-6 / 480.0;
                    return init + dclock * sec_per_clock1;
                }
            }
            double sec_per_clock = m_base_tempo * 1e-6 / 480.0;
            return clock * sec_per_clock;
        }

        public int getBaseTempo() {
            return m_base_tempo;
        }
    }

    [Serializable]
    public class TempoTableEntry : IComparable<TempoTableEntry>, ICloneable {
        public int Clock;
        public int Tempo;
        public double Time;

        public object Clone() {
            return new TempoTableEntry( Clock, Tempo, Time );
        }

        public TempoTableEntry( int clock, int _tempo, double _time ) {
            this.Clock = clock;
            this.Tempo = _tempo;
            this.Time = _time;
        }

        public TempoTableEntry() {
        }

        public int CompareTo( TempoTableEntry entry ) {
            return this.Clock - entry.Clock;
        }

        public boolean Equals( TempoTableEntry entry ) {
            if ( this.Clock == entry.Clock ) {
                return true;
            } else {
                return false;
            }
        }
    }

}
