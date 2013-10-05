/*
 * BTrackBar.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
//#define BENCH
using System;
using System.Windows.Forms;
using System.Reflection;

namespace cadencii.apputil
{

    /// <summary>
    /// Valueの型を変えられるTrackBar
    /// </summary>
    public class BTrackBar<T> : UserControl where T : struct, IComparable<T>
    {
        const int _MAX = 10000;
        private T m_value;
        private T m_min;
        private T m_max;
        private T m_tick_frequency;
        private MethodInfo m_parser = null;
        private ValueType m_value_type = ValueType.int_;
        private TrackBarEx m_track_bar;
        private Type m_type = typeof(int);

        private enum ValueType
        {
            sbyte_,
            byte_,
            shoft_,
            ushort_,
            int_,
            uint_,
            long_,
            ulong_,
        }

        public event EventHandler ValueChanged;

        private static void test()
        {
            System.Windows.Forms.TrackBar tb = new System.Windows.Forms.TrackBar();
            BTrackBar<int> tb2 = new BTrackBar<int>();
        }

        public BTrackBar()
        {
            InitializeComponent();
            T value_type = new T();
            if (value_type is byte) {
                m_value_type = BTrackBar<T>.ValueType.byte_;
            } else if (value_type is sbyte) {
                m_value_type = BTrackBar<T>.ValueType.sbyte_;
            } else if (value_type is short) {
                m_value_type = BTrackBar<T>.ValueType.shoft_;
            } else if (value_type is ushort) {
                m_value_type = BTrackBar<T>.ValueType.ushort_;
            } else if (value_type is int) {
                m_value_type = BTrackBar<T>.ValueType.int_;
            } else if (value_type is uint) {
                m_value_type = BTrackBar<T>.ValueType.uint_;
            } else if (value_type is long) {
                m_value_type = BTrackBar<T>.ValueType.long_;
            } else if (value_type is ulong) {
                m_value_type = BTrackBar<T>.ValueType.ulong_;
            } else {
                throw new NotSupportedException("generic type T must be byte, sbyte, short, ushort, int, uint, long or ulong");
            }
            m_type = value_type.GetType();
            m_parser = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });
            if (m_parser == null) {
                throw new ApplicationException("this error never occurs; m_type=" + m_value_type);
            }
#if BENCH
            // Benchmark1: string parser
            int _COUNT = 100000;
            MethodInfo parser_double = typeof( double ).GetMethod( "Parse", new Type[] { typeof( string ) } );
            Console.WriteLine( "parsed \"123.456\" = " + ((double)parser_double.Invoke( typeof( double ), new object[] { "123.456" } )) );
            DateTime start = DateTime.Now;
            for ( int i = 0; i < _COUNT; i++ ) {
                double v = (double)parser_double.Invoke( typeof( double ), new object[] { "123.456" } );
            }
            Console.WriteLine( "Benchmark1; " + DateTime.Now.Subtract( start ).TotalMilliseconds / (double)_COUNT + "ms" );

            // Benchmark2: BinaryFormatter
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream ms = new System.IO.MemoryStream( 64 );
            const double cdbbl = 123.456;
            bf.Serialize( ms, cdbbl );
            _COUNT = 10000;
            ms.Seek( 0, System.IO.SeekOrigin.Begin );
            Console.WriteLine( "deserilized = " + ((double)bf.Deserialize( ms )) );
            start = DateTime.Now;
            for ( int i = 0; i < _COUNT; i++ ) {
                ms.Seek( 0, System.IO.SeekOrigin.Begin );
                double v = (double)bf.Deserialize( ms );
            }
            Console.WriteLine( "Benchmark2; " + DateTime.Now.Subtract( start ).TotalMilliseconds / (double)_COUNT + "ms" );

            // Benchmark3: Convert class
            object obj = cdbbl;
            Console.WriteLine( "Converted = " + Convert.ToDouble( obj ) );
            _COUNT = 100000;
            start = DateTime.Now;
            for ( int i = 0; i < _COUNT; i++ ) {
                double v = Convert.ToDouble( obj );
            }
            Console.WriteLine( "Benchmark3; " + DateTime.Now.Subtract( start ).TotalMilliseconds / (double)_COUNT + "ms" );
#endif
        }

        public T Maximum
        {
            get
            {
                return m_max;
            }
            set
            {
                if (value.CompareTo(m_min) < 0) {
                    throw new ArgumentOutOfRangeException("Maximum");
                }
                m_max = value;
                if (m_max.CompareTo(m_value) < 0) {
                    Value = m_max;
                }
            }
        }

        public T Minimum
        {
            get
            {
                return m_min;
            }
            set
            {
                if (value.CompareTo(m_max) > 0) {
                    throw new ArgumentOutOfRangeException("Minimum");
                }
                m_min = value;
                if (m_min.CompareTo(m_value) > 0) {
                    Value = m_min;
                }
            }
        }

        public TickStyle TickStyle
        {
            get
            {
                return m_track_bar.TickStyle;
            }
            set
            {
                m_track_bar.TickStyle = value;
            }
        }

        public T TickFrequency
        {
            get
            {
                return m_tick_frequency;
            }
            set
            {
                m_tick_frequency = value;
                double max = asDouble(m_max);
                double min = asDouble(m_min);
                double stride = asDouble(m_tick_frequency);
                double rate = (max - min) / stride;
                Console.WriteLine("BTrackBar+set__TickFrequency");
                Console.WriteLine("    rate=" + rate);
                int freq = (int)(_MAX / rate);
                Console.WriteLine("    freq=" + freq);
                Console.WriteLine("    m_track_bar.Maximum=" + m_track_bar.Maximum);
                m_track_bar.TickFrequency = freq;
            }
        }

        public T Value
        {
            get
            {
                return m_value;
            }
            set
            {
                if (value.CompareTo(m_max) > 0) {
                    throw new ArgumentOutOfRangeException("Value");
                }
                if (value.CompareTo(m_min) < 0) {
                    throw new ArgumentOutOfRangeException("Value");
                }
                T old = m_value;
                m_value = value;
                if (old.CompareTo(m_value) != 0 && ValueChanged != null) {
                    ValueChanged(this, new EventArgs());
                }
            }
        }

        private double asDouble(T value)
        {
            object o = value;
            return Convert.ToDouble(o);
        }

        private T add(T value1, T value2)
        {
            object o1 = value1;
            object o2 = value2;
            switch (m_value_type) {
                case BTrackBar<T>.ValueType.sbyte_:
                sbyte sb1 = Convert.ToSByte(o1);
                sbyte sb2 = Convert.ToSByte(o2);
                object sb_r = (sb1 + sb2);
                return (T)sb_r;
                case BTrackBar<T>.ValueType.byte_:
                byte b1 = Convert.ToByte(o1);
                byte b2 = Convert.ToByte(o2);
                object b_r = (b1 + b2);
                return (T)b_r;
                case BTrackBar<T>.ValueType.shoft_:
                short s1 = Convert.ToInt16(o1);
                short s2 = Convert.ToInt16(o2);
                object s_r = (s1 + s2);
                return (T)s_r;
                case BTrackBar<T>.ValueType.ushort_:
                ushort us1 = Convert.ToUInt16(o1);
                ushort us2 = Convert.ToUInt16(o2);
                object us_r = (us1 + us2);
                return (T)us_r;
                case BTrackBar<T>.ValueType.int_:
                int i1 = Convert.ToInt32(o1);
                int i2 = Convert.ToInt32(o2);
                object i_r = (i1 + i2);
                return (T)i_r;
                case BTrackBar<T>.ValueType.uint_:
                uint ui1 = Convert.ToUInt32(o1);
                uint ui2 = Convert.ToUInt32(o2);
                object ui_r = ui1 + ui2;
                return (T)ui_r;
                case BTrackBar<T>.ValueType.long_:
                long l1 = Convert.ToInt64(o1);
                long l2 = Convert.ToInt64(o2);
                object l_r = l1 + l2;
                return (T)l_r;
                case BTrackBar<T>.ValueType.ulong_:
                ulong ul1 = Convert.ToUInt64(o1);
                ulong ul2 = Convert.ToUInt64(o2);
                object ul_r = ul1 + ul2;
                return (T)ul_r;
            }
            return new T();
        }

        private void InitializeComponent()
        {
            this.m_track_bar = new TrackBarEx();
            ((System.ComponentModel.ISupportInitialize)(this.m_track_bar)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar
            // 
            this.m_track_bar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_track_bar.Location = new System.Drawing.Point(0, 0);
            this.m_track_bar.Name = "trackBar";
            this.m_track_bar.Size = new System.Drawing.Size(286, 55);
            this.m_track_bar.TabIndex = 0;
            this.m_track_bar.Maximum = _MAX;
            this.m_track_bar.Minimum = 0;
            this.m_track_bar.TickFrequency = 1;
            this.m_track_bar.SmallChange = 1;
            this.m_track_bar.m_wheel_direction = false;
            // 
            // BTrackBar
            // 
            this.Controls.Add(this.m_track_bar);
            this.Name = "BTrackBar";
            this.Size = new System.Drawing.Size(286, 55);
            ((System.ComponentModel.ISupportInitialize)(this.m_track_bar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }

    internal class TrackBarEx : TrackBar
    {
        public bool m_wheel_direction = true;

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (m_wheel_direction) {
                base.OnMouseWheel(new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
            } else {
                base.OnMouseWheel(new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, -e.Delta));
            }
        }
    }

}
