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
#if JAVA
package org.kbinani.cadencii;

import java.awt.*;
import org.kbinani.*;
import org.kbinani.media.*;
#else
using System;
using Boare.Lib.Media;
using bocoree;
using bocoree.java.awt;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
#endif

#if JAVA
    public class WaveDrawContext{
#else
    public class WaveDrawContext : IDisposable {
#endif
        private byte[] m_wave;
        private int m_sample_rate = 44100;
        private String m_name;
        public UtauFreq Freq;
        private float m_length;

        public WaveDrawContext( String file ) {
            if ( !PortUtil.isFileExists( file ) ) {
                m_wave = new byte[0];
                m_length = 0.0f;
                return;
            }

            Wave wr = null;
            try {
                wr = new Wave( file );
                m_wave = new byte[(int)wr.getTotalSamples()];
                m_sample_rate = (int)wr.getSampleRate();
                m_length = wr.getTotalSamples() / (float)wr.getSampleRate();
#if DEBUG
                //PortUtil.println( "WaveDrawContext..ctor(String); m_length=" + m_length );
#endif
                int count = (int)wr.getTotalSamples();
                for ( int i = 0; i < count; i++ ) {
                    double b = wr.getDouble( (int)i );
                    m_wave[i] = (byte)((b + 1.0) * 0.5 * 127.0);
                }
            } catch ( Exception ex ) {
            } finally {
                if ( wr != null ) {
                    try {
#if !JAVA
                        wr.Dispose();
#endif
                    } catch ( Exception ex2 ) {
                    }
                }
            }
            if ( m_wave == null ) {
                m_wave = new byte[0];
                m_sample_rate = 44100;
                m_length = 0.0f;
            }
        }

        public String getName() {
            return m_name;
        }

        public void setName( String value ) {
            m_name = value;
        }

        public float getLength() {
            return m_length;
        }

        public void Dispose() {
            m_wave = null;
#if JAVA
            System.gc();
#else
            GC.Collect();
#endif
        }

        public void draw( Graphics2D g, Color pen, Rectangle rect, float sec_start, float sec_end ) {
            int start0 = (int)(sec_start * m_sample_rate) - 1;
            int end = (int)(sec_end * m_sample_rate) + 1;

            int width = rect.width;
            int height = rect.height;
            int ox = rect.x;
            int oy = rect.y + height;
            float order_y = rect.height / 127.0f;
            float order_x = rect.width / (float)(sec_end - sec_start) / (float)m_sample_rate;

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
            boolean drawn = false;
            g.setColor( pen );
            for ( int i = start + 1; i <= end; i++ ) {
                byte v = m_wave[i];
                if ( v != last ) {
                    drawn = true;
                    int x = ox + (int)((i - start0) * order_x);
                    int y = oy - (int)(v * order_y);
                    g.drawLine( lastx, lasty, x, lasty );
                    g.drawLine( x, lasty, x, y );
                    lastx = x;
                    lasty = y;
                    last = v;
                }
            }
            if ( !drawn ) {
                g.drawLine( rect.x, lasty, rect.x + rect.width, lasty );
            }
        }
    }

#if !JAVA
}
#endif