/*
 * WaveView.cs
 * Copyright (C) 2009 kbinani
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

import org.kbinani.windows.forms.*;

import java.awt.*;
import java.awt.image.*;
import org.kbinani.*;
import org.kbinani.media.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Windows.Forms;
using org.kbinani.media;
using org.kbinani;
using org.kbinani.java.awt;
using org.kbinani.java.awt.image;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using BEventArgs = System.EventArgs;
#endif

#if JAVA
    public class WaveView extends BPanel {
#else
    public class WaveView : BPanel {
#endif
        private float m_sample_rate = 44100;
        private float[] m_wave = new float[0];
#if !JAVA
        private PictureBox m_pict;
#endif
        private BufferedImage m_bmp;
        private int m_last_w = 0;
        private int m_last_h = 0;
        private int m_shiftx = 0;
        private int skip = 100;
        private float m_last_scalex;
        private int m_last_stdx;
        private Dimension m_last_size;

        public WaveView() {
#if JAVA
            super();
            //m_pict = new BPictureBox();
#else
            this.SetStyle( ControlStyles.DoubleBuffer, true );
            this.SetStyle( ControlStyles.UserPaint, true );
            m_pict = new PictureBox();
            this.Controls.Add( m_pict );
            m_pict.Dock = DockStyle.Fill;
            this.Resize += new EventHandler( WaveView_Resize );
            this.Paint += new PaintEventHandler( WaveView_Paint );
            m_pict.Paint += new PaintEventHandler( m_pict_Paint );
#endif
        }

#if !JAVA
        public void repaint() {
            this.Refresh();
        }
#endif

        /// <summary>
        /// 現在の波形画像をリセットします
        /// </summary>
        public void clear() {
            if ( m_bmp != null ) {
                try {
                    Graphics2D g = m_bmp.createGraphics();
                    g.setColor( Color.white );
                    g.clearRect( 0, 0, m_bmp.getWidth(), m_bmp.getHeight() );
                } catch ( Exception ex ) {
                }
            }
        }

#if JAVA
        public void paint( Graphics g1 ){
            Graphics2D g = (Graphics2D)g1;
            if( m_bmp != null ){
                g.drawImage( m_bmp, 0, 0, m_bmp.getWidth(), m_bmp.getHeight(), this );
            }
        }
#else
        void WaveView_Paint( object sender, PaintEventArgs e ) {
            m_pict.Invalidate();
        }

        private void m_pict_Paint( object sender, PaintEventArgs e ) {
            if ( m_bmp != null ) {
                e.Graphics.DrawImage( m_bmp.m_image, m_shiftx, 0, m_bmp.getWidth(), m_bmp.getHeight() );
            }
        }
#endif

        private void WaveView_Resize( Object sender, BEventArgs e ) {
            if ( getWidth() != 0 && getHeight() != 0 ) {
                if ( getWidth() != m_last_w || getHeight() != m_last_h ) {
                    draw();
                }
                m_last_w = getWidth();
                m_last_h = getHeight();
            }
#if !JAVA
            m_pict.Invalidate();
#endif
        }

        public void loadWave( String file ){
            WaveReader reader = null;
            try {
                reader = new WaveReader( file );
                m_wave = new float[reader.getTotalSamples() / skip];
                for ( int i = 0; i < reader.getTotalSamples() / skip; i++ ) {
                    ByRef<float[]> left = new ByRef<float[]>();
                    ByRef<float[]> right = new ByRef<float[]>();
                    reader.read( i * skip, 1, left, right );
                    m_wave[i] = 0.5f * (left.value[0] + right.value[0]);
                }
            } catch ( Exception ex ) {
            } finally {
                try {
                    reader.close();
                } catch ( Exception ex2 ) {
                }
            }
            m_last_scalex = -1;
            draw();
        }

        public void draw(){
            if ( getWidth() <= 0 || getHeight() <= 0 ) {
                return;
            }

            // 前回の描画ステータスと同じなら描画する必要なし
            Dimension size = getSize();
            if ( size.width == m_last_size.width && size.height == m_last_size.height && AppManager.startToDrawX == m_last_stdx && AppManager.scaleX == m_last_scalex ) {
                return;
            }
            m_last_size = getSize();
            m_last_stdx = AppManager.startToDrawX;
            m_last_scalex = AppManager.scaleX;

#if !JAVA
            m_pict.Image = null;
            if ( m_bmp != null && m_bmp.m_image != null ) {
                m_bmp.m_image.Dispose();
            }
#endif
            m_bmp = new BufferedImage( getWidth(), getHeight(), BufferedImage.TYPE_INT_BGR );

            int stdx = (int)(AppManager.startToDrawX / AppManager.scaleX);//            +(int)(AppManager._KEY_LENGTH / AppManager.ScaleX);
            double stdx_sec = AppManager.getVsqFile().getSecFromClock( stdx );
            int etdx = (int)(stdx + getWidth() / AppManager.scaleX); // end to draw x
            double etdx_sec = AppManager.getVsqFile().getSecFromClock( etdx );
            int sample_start = (int)(stdx_sec * m_sample_rate / (float)skip);
            int sample_end = (int)(etdx_sec * m_sample_rate / (float)skip);
            if ( sample_start < 0 ) sample_start = 0;
            Graphics2D g = null;
            try {
                g.setColor( new Color( 50, 50, 50 ) );
                g = m_bmp.createGraphics();
                g.clearRect( 0, 0, m_bmp.getWidth(), m_bmp.getHeight() );
                int centre = getHeight() / 2;
                Point last = new Point( 0, centre );
                for ( int i = sample_start; i < sample_end && i < m_wave.Length; i++ ) {
                    float sec = i * skip / m_sample_rate;
                    int x = (int)((AppManager.getVsqFile().getClockFromSec( sec ) - stdx) * AppManager.scaleX);
                    int y = centre - (int)(m_wave[i] * centre);
                    Point p = new Point( x, y );
                    g.drawLine( last.x, last.y, p.x, p.y );
                    last = p;
                }
            } catch ( Exception ex ) {
            } finally {
            }
#if !JAVA
            m_pict.Image = m_bmp.m_image;
#endif
            invalidate();
        }
    }

#if !JAVA
}
#endif
