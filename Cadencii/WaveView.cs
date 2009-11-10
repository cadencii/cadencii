/*
 * WaveView.cs
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
using System.Drawing;
using System.Windows.Forms;
using Boare.Lib.Media;

namespace Boare.Cadencii {

    public class WaveView : UserControl {
        private float m_sample_rate = 44100;
        private float[] m_wave = new float[0];
        private PictureBox m_pict;
        private Bitmap m_bmp;
        private int m_last_w = 0;
        private int m_last_h = 0;
        private int m_shiftx = 0;
        private int skip = 100;
        private float m_last_scalex;
        private int m_last_stdx;
        private Size m_last_size;

        public WaveView() {
            this.SetStyle( ControlStyles.DoubleBuffer, true );
            this.SetStyle( ControlStyles.UserPaint, true );
            m_pict = new PictureBox();
            this.Controls.Add( m_pict );
            m_pict.Dock = DockStyle.Fill;
            this.Resize += new EventHandler( WaveView_Resize );
            this.Paint += new PaintEventHandler( WaveView_Paint );
            m_pict.Paint += new PaintEventHandler( m_pict_Paint );
        }

        /// <summary>
        /// 現在の波形画像をリセットします
        /// </summary>
        public void Clear() {
            if ( m_bmp != null ) {
                using ( Graphics g = Graphics.FromImage( m_bmp ) ) {
                    g.Clear( Color.Transparent );
                }
            }
        }

        void WaveView_Paint( object sender, PaintEventArgs e ) {
            m_pict.Invalidate();
        }

        private void m_pict_Paint( object sender, PaintEventArgs e ) {
            if ( m_bmp != null ) {
                e.Graphics.DrawImage( m_bmp, m_shiftx, 0, m_bmp.Width, m_bmp.Height );
            }
        }

        private void WaveView_Resize( object sender, EventArgs e ) {
            if ( this.Width != 0 && this.Height != 0 ) {
                if ( this.Width != m_last_w || this.Height != m_last_h ) {
                    Draw();
                }
                m_last_w = this.Width;
                m_last_h = this.Height;
            }
            m_pict.Invalidate();
        }

        public void LoadWave( String file ){
            using ( WaveReader reader = new WaveReader( file ) ) {
                m_wave = new float[reader.getTotalSamples() / skip];
                for ( int i = 0; i < reader.getTotalSamples() / skip; i++ ) {
                    float[] left, right;
                    reader.read( i * skip, 1, out left, out right );
                    m_wave[i] = 0.5f * (left[0] + right[0]);
                }
            }
            m_last_scalex = -1;
            Draw();
        }

        public void Draw(){
            if ( this.Width <= 0 || this.Height <= 0 ) {
                return;
            }

            // 前回の描画ステータスと同じなら描画する必要なし
            if ( this.Size == m_last_size && AppManager.startToDrawX == m_last_stdx && AppManager.scaleX == m_last_scalex ) {
                return;
            }
            m_last_size = this.Size;
            m_last_stdx = AppManager.startToDrawX;
            m_last_scalex = AppManager.scaleX;

            m_pict.Image = null;
            if ( m_bmp != null ) {
                m_bmp.Dispose();
            }
            m_bmp = new Bitmap( this.Width, this.Height );

            int stdx = (int)(AppManager.startToDrawX / AppManager.scaleX);//            +(int)(AppManager._KEY_LENGTH / AppManager.ScaleX);
            double stdx_sec = AppManager.getVsqFile().getSecFromClock( stdx );
            int etdx = (int)(stdx + Width / AppManager.scaleX); // end to draw x
            double etdx_sec = AppManager.getVsqFile().getSecFromClock( etdx );
            int sample_start = (int)(stdx_sec * m_sample_rate / (float)skip);
            int sample_end = (int)(etdx_sec * m_sample_rate / (float)skip);
            if ( sample_start < 0 ) sample_start = 0;
            using ( Pen pen = new Pen( Color.FromArgb( 50, 50, 50 ) ) )
            using ( Graphics g = Graphics.FromImage( m_bmp ) ) {
                g.Clear( Color.Transparent );
                int centre = this.Height / 2;
                Point last = new Point( 0, centre );
                for ( int i = sample_start; i < sample_end && i < m_wave.Length; i++ ) {
                    float sec = i * skip / m_sample_rate;
                    int x = (int)((AppManager.getVsqFile().getClockFromSec( sec ) - stdx) * AppManager.scaleX);
                    int y = centre - (int)(m_wave[i] * centre);
                    Point p = new Point( x, y );
                    g.DrawLine( pen, last, p );
                    last = p;
                }
            }
            m_pict.Image = m_bmp;
            this.Invalidate();
        }
    }
    
}
