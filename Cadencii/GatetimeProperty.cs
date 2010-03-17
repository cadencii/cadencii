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
    [TypeConverter( typeof( GatetimePropertyConverter ) )]
    public class GatetimeProperty {
        private int m_clock;
        private string str_clock = "0";
        private string str_measure;
        private string str_beat;
        private string str_gate;

        public GatetimeProperty()
            : this( 0, 0, 0 ) {
        }

        public GatetimeProperty( int measure, int beat, int gate ) {
            str_measure = measure + "";
            str_beat = beat + "";
            str_gate = gate + "";

            m_clock = 0;
            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq == null ) {
                m_clock = measure * 480 * 4 + (beat - 1) * 480 + gate;
                return;
            }

            int premeasure = vsq.getPreMeasure();
            int clock_at_measure = vsq.getClockFromBarCount( measure + premeasure - 1 );
            Timesig timesig = vsq.getTimesigAt( clock_at_measure );
            m_clock = clock_at_measure + 480 * 4 / timesig.denominator * (beat - 1) + gate;
            str_clock = m_clock + "";
        }

        /// <summary>
        /// 現在のクロック値(m_clock)から、小節数、拍数、ゲート数(?)を計算します
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="beat"></param>
        /// <param name="gate"></param>
        /// <returns></returns>
        private Timesig getPosition( out int measure, out int beat, out int gate ) {
            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq == null ) {
                // 4/4拍子, プリメジャー2と仮定
                int i = m_clock / (480 * 4);
                int tpremeasure = 2;
                measure = i - tpremeasure + 1;
                int tdif = m_clock - i * 480 * 4;
                beat = tdif / 480 + 1;
                gate = tdif - (beat - 1) * 480;
                return new Timesig( 4, 4 );
            }

            int premeasure = vsq.getPreMeasure();
            measure = vsq.getBarCountFromClock( m_clock ) - premeasure + 1;
            int clock_bartop = vsq.getClockFromBarCount( measure + premeasure - 1 );
            Timesig timesig = vsq.getTimesigAt( m_clock );
            int den = timesig.denominator;
            int dif = m_clock - clock_bartop;
            int step = 480 * 4 / den;
            beat = dif / step + 1;
            gate = dif - (beat - 1) * step;
            return timesig;
        }

        /// <summary>
        /// 小節数、拍数、ゲート数から、クロック値を計算します
        /// </summary>
        /// <param name="measure"></param>
        /// <param name="beat"></param>
        /// <param name="gate"></param>
        /// <returns></returns>
        private int calculateClock( int measure, int beat, int gate ) {
            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq == null ) {
                int premeasure = 2;
                return ((measure + premeasure - 1) * 4 + (beat - 1)) * 480 + gate;
            } else {
                int premeasure = vsq.getPreMeasure();
                int bartopclock = vsq.getClockFromBarCount( measure + premeasure - 1 );
                Timesig timesig = vsq.getTimesigAt( bartopclock );
                return bartopclock + (beat - 1) * 480 * 4 / timesig.denominator + gate;
            }
        }

        public int getClockValue() {
            return m_clock;
        }

        public string Measure {
            get {
                return str_measure;
            }
            set {
            }
        }

        public string Beat {
            get {
                return str_beat;
            }
            set {
            }
        }

        public string Gate {
            get {
                return str_gate;
            }
            set {
                int measure, beat, gate;
                Timesig timesig = getPosition( out measure, out beat, out gate );
                gate = SelectedEventEntry.evalReceivedString( gate, value );
                int draft = calculateClock( measure, beat, gate );
                setClockCor( draft );
            }
        }

        private void setClockCor( int clock ) {
            m_clock = clock;
            str_clock = m_clock + "";

            int measure, beat, gate;
            getPosition( out measure, out beat, out gate );

            str_measure = measure + "";
            str_beat = beat + "";
            str_gate = gate + "";
        }

        public string Clock {
            get {
                return str_clock;
            }
            set {
                int old_value = m_clock;
                int draft = SelectedEventEntry.evalReceivedString( old_value, value );
                setClockCor( draft );
            }
        }
    }

}

namespace org.kbinani.cadencii.old {
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
