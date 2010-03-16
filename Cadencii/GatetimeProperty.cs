#if ENABLE_PROPERTY
/*
 * GatetimeProperty.cs
 * Copyright (C) 2009-2010 kbinani
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
using System;
using System.ComponentModel;
using org.kbinani.vsq;
using org.kbinani;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
    using Integer = System.Int32;

    [TypeConverter( typeof( GatetimePropertyConverter ) )]
    public class GatetimeProperty {
        private CalculatableString m_clock = new CalculatableString();

        private CalculatableString m_measure;
        private CalculatableString m_beat;
        private CalculatableString m_gate;
        private int m_num;
        private int m_den;

        public GatetimeProperty()
            : this( 0, 0, 0 ) {
        }

        public GatetimeProperty( int measure, int beat, int gate ) {
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
            this.Clock = new CalculatableString( clock );
        }

        public boolean equals( Object obj ) {
            if ( obj is GatetimeProperty ) {
                if ( m_clock.getIntValue() == ((GatetimeProperty)obj).m_clock.getIntValue() ) {
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

        public CalculatableString Measure {
            get {
                return m_measure;
            }
            set {
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
                this.Clock = new CalculatableString( draft_clock );
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
                this.Clock = new CalculatableString( draft_clock );
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
                this.Clock = new CalculatableString( draft_clock );
            }
        }

        public CalculatableString Clock {
            get {
                return m_clock;
            }
            set {
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

}
#endif
