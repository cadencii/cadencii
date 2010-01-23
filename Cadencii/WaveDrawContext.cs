/*
 * WaveDrawContext.cs
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
#if JAVA
package org.kbinani.cadencii;

import java.awt.*;
import org.kbinani.*;
import org.kbinani.media.*;
#else
using System;
using org.kbinani.media;
using org.kbinani;
using org.kbinani.java.awt;
using org.kbinani.java.util;
using org.kbinani.vsq;

namespace org.kbinani.cadencii {
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
        /// <summary>
        /// 第indexSpec[i]からindexSpec[i + 1]サンプルについては，1サンプルあたりのクロック数がclockPerSample[i]であることを表すのに使う．
        /// </summary>
        private int[] indexSpec;
        /// <summary>
        /// 1サンプルあたりのクロック数
        /// </summary>
        private float[] clockPerSample;

        public WaveDrawContext( String file ) {
            load( file );
        }

        public WaveDrawContext() {
            m_wave = new byte[0];
            m_length = 0.0f;
        }

        public void load( String file ) {
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
                int count = (int)wr.getTotalSamples();
                for ( int i = 0; i < count; i++ ) {
                    double b = wr.getDouble( (int)i );
                    m_wave[i] = (byte)((b + 1.0) * 0.5 * 127.0);
                }
            } catch ( Exception ex ) {
            } finally {
                if ( wr != null ) {
                    try {
                        wr.dispose();
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

        public void draw( Graphics2D g, 
                          Color pen,
                          Rectangle rect,
                          int clock_start,
                          int clock_end, 
                          TempoVector tempo_table, 
                          float pixel_per_clock ) {
            if ( m_wave.Length == 0 ) {
                return;
            }
#if DEBUG
            PortUtil.println( "WaveDrawContext#draw; gatetime-base" );
#endif
            double secStart = tempo_table.getSecFromClock( clock_start );
            double secEnd = tempo_table.getSecFromClock( clock_end );
            int sStart0 = (int)(secStart * m_sample_rate) - 1;
            int sEnd = (int)(secEnd * m_sample_rate) + 1;

            int count = tempo_table.size();
            int sStart = 0;
            double cStart = 0.0;
            float order_y = rect.height / 127.0f;
            int ox = rect.x;
            int oy = rect.y + rect.height;
            byte last = m_wave[0];
            int lastx = ox;
            int lasty = oy - (int)(last * order_y);
            int BUFLEN = 1024;
#if JAVA
            int[] xPoints = new int[BUFLEN];
            int[] yPoints = new int[BUFLEN];
#else
            System.Drawing.Point[] points = new System.Drawing.Point[BUFLEN];
#endif
            int pos = 0;
#if JAVA
            xPoints[pos] = lastx;
            yPoints[pos] = lasty;
#else
            points[pos] = new System.Drawing.Point( lastx, lasty );
#endif
            pos++;
            int xmax = rect.x + rect.width;
            for ( int i = 0; i <= count; i++ ) {
                TempoTableEntry entry = null;
                if ( i < count ) {
                    entry = tempo_table.get( i );
                } else {
                    entry = (TempoTableEntry)tempo_table.get( i - 1 ).clone();
                    entry.Clock = clock_end;
                    entry.Time = tempo_table.getSecFromClock( clock_end );
                }
                int sThisEnd = (int)(entry.Time * m_sample_rate);
                double cEnd = tempo_table.getClockFromSec( entry.Time );
                
                // startからendまでを描画する(必要なら!)
                if ( sThisEnd < sStart0 ) {
                    continue;
                }
                if ( sStart < sEnd ) {
                    //break;
                }

                // 
                int xoffset = (int)(cStart * pixel_per_clock) - AppManager.startToDrawX + AppManager.keyOffset;
                double sec_per_clock = entry.Tempo * 1e-6 / 480.0;
                int j0 = sStart;
                if ( j0 < 0 ) {
                    j0 = 0;
                }
                int j1 = sThisEnd;
                if ( m_wave.Length < j1 ) {
                    j1 = m_wave.Length;
                }
                boolean breakRequired = false;
                for ( int j = j0; j < j1; j++ ) {
                    byte v = m_wave[j];
                    if ( v == last ) {
                        continue;
                    }
                    double secDelta = (j - sStart) / (double)m_sample_rate;
                    double c = secDelta / sec_per_clock;
                    int x = xoffset + (int)(c * pixel_per_clock);
                    if ( xmax < x ) {
                        breakRequired = true;
                        break;
                    }
                    int y = oy - (int)(v * order_y);
#if JAVA
                    xPoints[pos] = x;
                    yPoints[pos] = y;
#else
                    points[pos].X = x;
                    points[pos].Y = y;
#endif
                    pos++;
                    if ( BUFLEN <= pos ) {
#if JAVA
                        g.drawPolyline( xPoints, yPoints, BUFLEN );
                        xPoints[0] = xPoints[BUFLEN - 1];
                        yPoints[0] = yPoints[BUFLEN - 1];
#else
                        g.nativeGraphics.DrawLines( g.stroke.pen, points );
                        points[0] = points[BUFLEN - 1];
#endif
                        pos = 1;
                    }
                    lastx = x;
                    lasty = y;
                    last = v;
                }
                sStart = sEnd;
                cStart = cEnd;
                if ( breakRequired ) {
                    break;
                }
            }

            if ( pos > 2 ) {
#if JAVA
                g.drawPolyline( xPoints, yPoints, pos );
#else
                Array.Resize( ref points, pos );
                g.nativeGraphics.DrawLines( g.stroke.pen, points );
#endif
            }
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