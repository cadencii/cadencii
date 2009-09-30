/*
 * WaveDrawContext.cs
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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

using Boare.Lib.Media;
using bocoree;

namespace Boare.Cadencii {

    public class WaveDrawContext : IDisposable {
        private byte[] m_wave;
        private int m_sample_rate = 44100;
        private String m_name;
        public UtauFreq Freq;
        private float m_length;

        public WaveDrawContext( String file ) {
            if ( !File.Exists( file ) ) {
                m_wave = new byte[0];
                m_length = 0.0f;
                return;
            }

            using ( Wave wr = new Wave( file ) ) {
                m_wave = new byte[wr.TotalSamples];
                m_sample_rate = (int)wr.SampleRate;
                m_length = wr.TotalSamples / (float)wr.SampleRate;
#if DEBUG
                Console.WriteLine( "WaveDrawContext..ctor(String); m_length=" + m_length );
#endif
                int count = (int)wr.TotalSamples;
                for ( int i = 0; i < count; i++ ) {
                    double b = wr.Get( (int)i );
                    m_wave[i] = (byte)((b + 1.0) * 0.5 * 127.0);
                }
            }
            if ( m_wave == null ) {
                m_wave = new byte[0];
                m_sample_rate = 44100;
                m_length = 0.0f;
            }
        }

        public String Name {
            get {
                return m_name;
            }
            set {
                m_name = value;
            }
        }

        public float Length {
            get {
                return m_length;
            }
        }

        public void Dispose() {
            m_wave = null;
            GC.Collect();
        }

        public unsafe void Draw( Graphics g, Pen pen, Rectangle rect, float sec_start, float sec_end ) {
            int start0 = (int)(sec_start * m_sample_rate) - 1;
            int end = (int)(sec_end * m_sample_rate) + 1;

            int width = rect.Width;
            int height = rect.Height;
            int ox = rect.X;
            int oy = rect.Y + height;
            float order_y = rect.Height / 127.0f;
            float order_x = rect.Width / (float)(sec_end - sec_start) / (float)m_sample_rate;

            int start = start0;
            if ( start < 0 ) {
                start = 0;
            }
            if ( m_wave.Length < end ) {
                end = m_wave.Length - 1;
            }

            byte last = 0x0;
            if ( m_wave == null || (m_wave != null && m_wave.Length <= 0) ) {
                return;
            }
            last = m_wave[0];
            int lastx = ox;
            int lasty = oy - (int)(last * order_y);
            bool drawn = false;
            fixed ( byte* pb = &m_wave[0] ) {
                for ( int i = start + 1; i <= end; i++ ) {
                    byte v = pb[i];
                    if ( v != last ) {
                        drawn = true;
                        int x = ox + (int)((i - start0) * order_x);
                        int y = oy - (int)(v * order_y);
                        g.DrawLine( pen, lastx, lasty, x, lasty );
                        g.DrawLine( pen, x, lasty, x, y );
                        lastx = x;
                        lasty = y;
                        last = v;
                    }
                }
            }
            if ( !drawn ) {
                g.DrawLine( pen, rect.X, lasty, rect.X + rect.Width, lasty );
            }
        }
    }

}
