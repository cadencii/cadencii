/*
 * WaveView.cs
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
        private WaveDrawContext[] drawer = new WaveDrawContext[16];

        public WaveView()
            : base() {
            this.SetStyle( ControlStyles.DoubleBuffer, true );
            this.SetStyle( ControlStyles.UserPaint, true );
            this.DoubleBuffered = true;
        }

        private void paint( Graphics2D g ) {
            int width = getWidth();
            Rectangle rc = new Rectangle( 0, 0, width, getHeight() );
            if ( AppManager.skipDrawingWaveformWhenPlaying && AppManager.isPlaying() ) {
                PortUtil.drawStringEx( g, 
                                       "(hidden for performance)", 
                                       AppManager.baseFont10,
                                       rc, 
                                       PortUtil.STRING_ALIGN_CENTER,
                                       PortUtil.STRING_ALIGN_CENTER );
                return;
            }
            int selected = AppManager.getSelected();
            WaveDrawContext context = drawer[selected - 1];
            if ( context == null ) {
                return;
            }
            context.draw( g,
                          Color.black,
                          rc,
                          AppManager.clockFromXCoord( AppManager.keyWidth ),
                          AppManager.clockFromXCoord( AppManager.keyWidth + width ),
                          AppManager.getVsqFile().TempoTable,
                          AppManager.scaleX );
        }

        public void unloadAll() {
            for ( int i = 0; i < drawer.Length; i++ ) {
                WaveDrawContext context = drawer[i];
                if ( context == null ) {
                    continue;
                }
                context.unload();
            }
        }

        public void reloadPartial( int track, String file, double sec_from, double sec_to ) {
            if ( track < 0 || drawer.Length <= track ) {
                return;
            }
            if ( drawer[track] == null ) {
                drawer[track] = new WaveDrawContext();
                drawer[track].load( file );
            } else {
                drawer[track].reloadPartial( file, sec_from, sec_to );
            }
        }

        public void load( int track, String wave_path ) {
            if ( track < 0 || drawer.Length <= track ) {
                return;
            }
            if ( drawer[track] == null ) {
                drawer[track] = new WaveDrawContext();
            }
            drawer[track].load( wave_path );
        }

        protected override void OnPaint( PaintEventArgs e ) {
            base.OnPaint( e );
            paint( new Graphics2D( e.Graphics ) );
        }
    }

    public class WaveView_ : BPanel {
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
        private int skip = 1;
        private float m_last_scalex;
        private int m_last_stdx;
        private Dimension m_last_size;

        public WaveView_() {
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
            int width = getWidth();
            int height = getHeight();
            if ( width != 0 && height != 0 ) {
                if ( width != m_last_w || height != m_last_h ) {
                    draw();
                }
                m_last_w = width;
                m_last_h = height;
            }
#if !JAVA
            m_pict.Invalidate();
#endif
        }

        public void loadWave( String file ){
            WaveReader reader = null;
            try {
                reader = new WaveReader( file );
                int totalSamples = reader.getTotalSamples();
                m_wave = new float[totalSamples / skip];
                for ( int i = 0; i < totalSamples / skip; i++ ) {
                    ByRef<float[]> left = new ByRef<float[]>();
                    ByRef<float[]> right = new ByRef<float[]>();
                    reader.read( i * skip, 1, left, right );
                    m_wave[i] = 0.5f * (left.value[0] + right.value[0]);
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "WaveView#loadWave; ex=" + ex );
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

            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq == null ) {
                return;
            }
            int stdx = (int)(AppManager.startToDrawX / AppManager.scaleX);//            +(int)(AppManager._KEY_LENGTH / AppManager.ScaleX);
            double stdx_sec = vsq.getSecFromClock( stdx );
            int etdx = (int)(stdx + getWidth() / AppManager.scaleX); // end to draw x
            double etdx_sec = vsq.getSecFromClock( etdx );
            int sample_start = (int)(stdx_sec * m_sample_rate / (float)skip);
            int sample_end = (int)(etdx_sec * m_sample_rate / (float)skip);
            if ( sample_start < 0 ) sample_start = 0;
            Graphics2D g = null;
            try {
                g = m_bmp.createGraphics();
                g.setColor( new Color( 50, 50, 50 ) );
                g.clearRect( 0, 0, m_bmp.getWidth(), m_bmp.getHeight() );
                int centre = getHeight() / 2;
                Point last = new Point( 0, centre );
                for ( int i = sample_start; i < sample_end && i < m_wave.Length; i++ ) {
                    float sec = i * skip / m_sample_rate;
                    int x = (int)((vsq.getClockFromSec( sec ) - stdx) * AppManager.scaleX);
                    int y = centre - (int)(m_wave[i] * centre);
                    Point p = new Point( x, y );
                    g.drawLine( last.x, last.y, p.x, p.y );
                    last = p;
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "WaveView#draw; ex=" + ex );
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
