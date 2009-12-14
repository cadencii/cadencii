#if ENABLE_PROPERTY
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
using org.kbinani.vsq;
using bocoree;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using Integer = System.Int32;

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
                Timesig timesig = AppManager.getVsqFile().getTimesigAt( clock_at_measure );
                clock = clock_at_measure + 480 * 4 / timesig.denominator * (beat - 1) + gate;
            } else {
                clock = measure * 480 * 4 + (beat - 1) * 480 + gate;
            }
            setClock( new CalculatableString( clock ) );
        }

        public boolean equals( Object obj ) {
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

        public override bool Equals( object obj ) {
            return equals( obj );
        }

        public CalculatableString getMeasure() {
            return m_measure;
        }

        public void setMeasure( CalculatableString value ) {
            int clock = m_clock.getIntValue();
            if ( m_den <= 0 || m_num <= 0 ) {
                if ( AppManager.getVsqFile() != null ) {
                    Timesig timesig = AppManager.getVsqFile().getTimesigAt( clock );
                    m_num = timesig.numerator;
                    m_den = timesig.denominator;
                } else {
                    m_num = 4;
                    m_den = 4;
                }
            }
            int dif = value.getIntValue() - m_measure.getIntValue();
            int step = 480 * 4 / m_den;
            int draft_clock = clock + dif * m_num * step;
            setClock( new CalculatableString( draft_clock ) );
        }

        public CalculatableString getBeat() {
            return m_beat;
        }

        public void setBeat( CalculatableString value ) {
            int clock = m_clock.getIntValue();
            if ( m_den <= 0 || m_num <= 0 ) {
                if ( AppManager.getVsqFile() != null ) {
                    Timesig timesig = AppManager.getVsqFile().getTimesigAt( clock );
                    m_num = timesig.numerator;
                    m_den = timesig.denominator;
                } else {
                    m_num = 4;
                    m_den = 4;
                }
            }
            int dif = value.getIntValue() - m_beat.getIntValue();
            int step = 480 * 4 / m_den;
            int draft_clock = clock + dif * step;
            setClock( new CalculatableString( draft_clock ) );
        }

        public CalculatableString getGate() {
            return m_gate;
        }

        public void setGate( CalculatableString value ) {
            int clock = m_clock.getIntValue();
            int dif = value.getIntValue() - m_gate.getIntValue();
            int draft_clock = clock + dif;
            setClock( new CalculatableString( draft_clock ) );
        }

        public CalculatableString getClock() {
            return m_clock;
        }

        public void setClock( CalculatableString value ) {
            m_clock.setStr( value.getStr() );
            int clock = m_clock.getIntValue();
            if ( AppManager.getVsqFile() != null ) {
                int premeasure = AppManager.getVsqFile().getPreMeasure();
                m_measure.setStr( "" + (AppManager.getVsqFile().getBarCountFromClock( clock ) - premeasure + 1) );
                int clock_bartop = AppManager.getVsqFile().getClockFromBarCount( m_measure.getIntValue() + premeasure - 1 );
                Timesig timesig = AppManager.getVsqFile().getTimesigAt( clock );
                m_num = timesig.numerator;
                m_den = timesig.denominator;
                int dif = clock - clock_bartop;
                int step = 480 * 4 / m_den;
                m_beat.setStr( "" + (dif / step + 1) );
                m_gate.setStr( "" + (dif - (m_beat.getIntValue() - 1) * step) );
            }
        }
    }

}
#endif
