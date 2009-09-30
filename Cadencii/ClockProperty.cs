/*
 * ClockProperty.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.ComponentModel;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    [TypeConverter( typeof( ClockPropertyConverter ) )]
    public class ClockProperty {
        private CalculatableString m_clock = new CalculatableString();

        private CalculatableString m_measure;
        private CalculatableString m_beat;
        private CalculatableString m_gate;
        private int m_num;
        private int m_den;

        public ClockProperty()
            : this( 0, 0, 0 ) {
        }

        public ClockProperty( int measure, int beat, int gate ) {
            m_measure = new CalculatableString();
            m_beat = new CalculatableString();
            m_gate = new CalculatableString();
            int clock = 0;
            if ( AppManager.getVsqFile() != null ) {
                int premeasure = AppManager.getVsqFile().getPreMeasure();
                int clock_at_measure = AppManager.getVsqFile().getClockFromBarCount( measure + premeasure - 1 );
                int den, num;
                AppManager.getVsqFile().getTimesigAt( clock_at_measure, out num, out den );
                clock = clock_at_measure + 480 * 4 / den * (beat - 1) + gate;
            } else {
                clock = measure * 480 * 4 + (beat - 1) * 480 + gate;
            }
            Clock = new CalculatableString( clock );
        }

        public override boolean Equals( object obj ) {
            if ( obj is ClockProperty ) {
                if ( m_clock.getIntValue() == ((ClockProperty)obj).m_clock.getIntValue() ) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return base.Equals( obj );
            }
        }

        public CalculatableString Measure {
            get {
                return m_measure;
            }
            set {
                int clock = m_clock.getIntValue();
                if ( m_den <= 0 || m_num <= 0 ) {
                    if ( AppManager.getVsqFile() != null ) {
                        AppManager.getVsqFile().getTimesigAt( clock, out m_num, out m_den );
                    } else {
                        m_num = 4;
                        m_den = 4;
                    }
                }
                int dif = value.getIntValue() - m_measure.getIntValue();
                int step = 480 * 4 / m_den;
                int draft_clock = clock + dif * m_num * step;
                Clock = new CalculatableString( draft_clock );
            }
        }

        public CalculatableString Beat {
            get {
                return m_beat;
            }
            set {
                int clock = m_clock.getIntValue();
                if ( m_den <= 0 || m_num <= 0 ) {
                    if ( AppManager.getVsqFile() != null ) {
                        AppManager.getVsqFile().getTimesigAt( clock, out m_num, out m_den );
                    } else {
                        m_num = 4;
                        m_den = 4;
                    }
                }
                int dif = value.getIntValue() - m_beat.getIntValue();
                int step = 480 * 4 / m_den;
                int draft_clock = clock + dif * step;
                Clock = new CalculatableString( draft_clock );
            }
        }

        public CalculatableString Gate {
            get {
                return m_gate;
            }
            set {
                int clock = m_clock.getIntValue();
                int dif = value.getIntValue() - m_gate.getIntValue();
                int draft_clock = clock + dif;
                Clock = new CalculatableString( draft_clock );
            }
        }

        public CalculatableString Clock {
            get {
                return m_clock;
            }
            set {
                m_clock.str = value.str;
                int clock = m_clock.getIntValue();
                if ( AppManager.getVsqFile() != null ) {
                    int premeasure = AppManager.getVsqFile().getPreMeasure();
                    m_measure.str = "" + (AppManager.getVsqFile().getBarCountFromClock( clock ) - premeasure + 1);
                    int clock_bartop = AppManager.getVsqFile().getClockFromBarCount( m_measure.getIntValue() + premeasure - 1 );
                    AppManager.getVsqFile().getTimesigAt( clock, out m_num, out m_den );
                    int dif = clock - clock_bartop;
                    int step = 480 * 4 / m_den;
                    m_beat.str = "" + (dif / step + 1);
                    m_gate.str = "" + (dif - (m_beat.getIntValue() - 1) * step);
                }
            }
        }
    }

}
