/*
 * ContextProperty.cs
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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Boare.Lib.AppUtil;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    class ContextProperty {
        private object m_tag;
        private Rectangle m_bounds;
        private GraphicsPath m_bound_path;
        private GraphicsPath m_titlebar_path;
        private int m_corner = 5;
        private int m_titlebar_height = 15;
        private String m_title = "";
        private Font m_titlefont;
        private int m_titlefont_yoffset = 0;
        private Point m_shift;
        private boolean m_mouse_downed = false;
        /// <summary>
        /// タイトルバーのドラッグによりウィンドウを動かすとき，最初マウスの降りた時点m_shiftの値
        /// </summary>
        private Point m_title_drag_init_shift;
        /// <summary>
        /// タイトルバーのドラッグによりウィンドウを動かすとき，最初マウスの降りた時点でのマウスの位置
        /// </summary>
        private Point m_title_drag_init;
        private int m_internal_id;
        private boolean m_x_button_downed = false;

        /// <summary>
        /// このウィンドウがが閉じようとしているとき発生します
        /// </summary>
        public event BSimpleDelegate<int> FormClosing;

        public int TitleHeight {
            get {
                return m_titlebar_height;
            }
            set {
                m_titlebar_height = value;
            }
        }

        public ContextProperty( int internal_id ) {
            m_internal_id = internal_id;
            UpdateBoundPath();
            TitleFont = new Font( "MS UI Gothic", 9 );
        }

        /// <summary>
        /// pがrcの中にあるかどうかを判定します
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rc"></param>
        /// <returns></returns>
        static boolean IsInRect( Point p, Rectangle rc ) {
            if ( rc.X <= p.X ) {
                if ( p.X <= rc.X + rc.Width ) {
                    if ( rc.Y <= p.Y ) {
                        if ( p.Y <= rc.Y + rc.Height ) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public Point Shift {
            get {
                return m_shift;
            }
            set {
                m_shift = value;
            }
        }

        public boolean MouseMove( object sender, MouseEventArgs e ) {
            if ( m_mouse_downed ) {
                m_shift = new Point( e.X - m_title_drag_init.X + m_title_drag_init_shift.X,
                                     e.Y - m_title_drag_init.Y + m_title_drag_init_shift.Y );
                return true;
            } else {
                return false;
            }
        }

        public boolean MouseDown( object sender, MouseEventArgs e ) {
            if ( IsInRect( e.Location, m_bounds ) ) {
                if ( e.Button == MouseButtons.Left ) {
                    // ×ぼたんでの左クリックかどうか
                    Rectangle rc = CloseButtonBounds;
                    rc = new Rectangle( m_bounds.X + rc.X, m_bounds.Y + rc.Y, rc.Width, rc.Height );
#if DEBUG
                    AppManager.debugWriteLine( "ContextProperty.MouseDown; rc=" + rc );
#endif
                    if ( IsInRect( e.Location, rc ) ) {
#if DEBUG
                        AppManager.debugWriteLine( "ContextProperty.MouseDown; clicked x button" );
#endif
                        m_x_button_downed = true;
                        return true;
                    }
                    m_x_button_downed = false;

                    // タイトルバーでの左クリックかどうか
                    rc = new Rectangle( m_bounds.X, m_bounds.Y, m_bounds.Width, m_titlebar_height );
                    if ( IsInRect( e.Location, rc ) ) {
                        m_mouse_downed = true;
                        m_title_drag_init = e.Location;
                        m_title_drag_init_shift = m_shift;
                        return true;
                    }
                    m_mouse_downed = false;
                }
                return true;
            } else {
                return false;
            }
        }

        public boolean MouseUp( object sender, MouseEventArgs e ) {
            if ( m_x_button_downed ) {
                Rectangle rc = CloseButtonBounds;
                rc = new Rectangle( m_bounds.X + rc.X, m_bounds.Y + rc.Y, rc.Width, rc.Height );
                if ( IsInRect( e.Location, rc ) && FormClosing != null ) {
                    m_x_button_downed = false;
                    FormClosing( m_internal_id );
                    return true;
                }
            }
            m_mouse_downed = false;
            return false;
        }

        public Rectangle CloseButtonBounds {
            get {
                return new Rectangle( m_bounds.Width - m_titlebar_height + 2, 1, m_titlebar_height - 4, m_titlebar_height - 4 );
            }
        }

        public Font TitleFont {
            get {
                return m_titlefont;
            }
            set {
                m_titlefont = value;
                m_titlefont_yoffset = Util.GetStringDrawOffset( value );
            }
        }

        public String Title {
            get {
                return m_title;
            }
            set {
                m_title = value;
            }
        }

        private void UpdateBoundPath() {
            if ( m_bound_path != null ) {
                m_bound_path.Dispose();
                m_bound_path = null;
            }
            if ( m_titlebar_path != null ) {
                m_titlebar_path.Dispose();
                m_titlebar_path = null;
            }
            int corner2 = 2 * m_corner;
            m_bound_path = new GraphicsPath();
            m_bound_path.StartFigure();
            m_bound_path.AddArc( new Rectangle( 0, 0, corner2, corner2 ), 180, 90 );
            m_bound_path.AddLine( m_corner, 0, m_bounds.Width - m_corner, 0 );
            m_bound_path.AddArc( new Rectangle( m_bounds.Width - corner2, 0, corner2, corner2 ), 270, 90 );
            m_titlebar_path = (GraphicsPath)m_bound_path.Clone();
            m_bound_path.AddLine( m_bounds.Width, m_corner, m_bounds.Width, m_bounds.Height - m_corner );
            m_bound_path.AddArc( new Rectangle( m_bounds.Width - corner2, m_bounds.Height - corner2, corner2, corner2 ), 0, 90 );
            m_bound_path.AddLine( m_bounds.Width - m_corner, m_bounds.Height, m_corner, m_bounds.Height );
            m_bound_path.AddArc( new Rectangle( 0, m_bounds.Height - corner2, corner2, corner2 ), 90, 90 );
            m_bound_path.AddLine( 0, m_bounds.Height - m_corner, 0, m_corner );
            m_titlebar_path.AddLines( new Point[]{ new Point( m_bounds.Width, m_corner ), new Point( m_bounds.Width, m_titlebar_height ),
                                                   new Point( 0, m_titlebar_height ), new Point( 0, m_corner ) } );
        }

        public void DrawTo( Graphics g, Point position ) {
            Rectangle rc = new Rectangle( position.X, position.Y, m_bounds.Width, m_bounds.Height );
            
            SmoothingMode smold = g.SmoothingMode;
            Region clipold = g.Clip.Clone();

            Matrix m = new Matrix();
            m.Translate( position.X, position.Y );
            m_bound_path.Transform( m );
            m_titlebar_path.Transform( m );
            RectangleF rc_clip = m_bound_path.GetBounds();
            g.Clip = new Region( new Rectangle( (int)rc_clip.X - 1, (int)rc_clip.Y - 1, (int)rc_clip.Width + 3, (int)rc_clip.Height + 3 ) );
            
            g.FillPath( new SolidBrush( Color.FromArgb( 127, Color.Black ) ), m_bound_path );
            g.FillPath( new SolidBrush( Color.FromArgb( 127, Color.Black ) ), m_titlebar_path );
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawPath( Pens.Black, m_bound_path );
            g.DrawString( m_title, m_titlefont, Brushes.White, new PointF( position.X + 5, position.Y + m_titlebar_height / 2 - m_titlefont_yoffset + 1 ) );

            // ×ボタン
            Rectangle rcx = CloseButtonBounds;
            rcx = new Rectangle( rcx.X + position.X, rcx.Y + position.Y, rcx.Width, rcx.Height );
            g.FillEllipse( Brushes.White, rcx );
            int shift = (int)(rcx.Width / 2.0 - (rcx.Width - 4) / 2.0 / 1.41421356);
            using ( Pen pen = new Pen( Color.Black, 2.0f ) ) {
                pen.EndCap = LineCap.Round;
                pen.StartCap = LineCap.Round;
                g.DrawLine( pen, rcx.X + shift, rcx.Y + shift, rcx.X + rcx.Width - shift, rcx.Y + rcx.Height - shift );
                g.DrawLine( pen, rcx.X + rcx.Width - shift, rcx.Y + shift, rcx.X + shift, rcx.Y + rcx.Height - shift );
            }

            CheckBoxRenderer.DrawCheckBox( g, new Point( position.X, position.Y ), System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal );

            m.Reset();
            m.Translate( -position.X, -position.Y );
            m_bound_path.Transform( m );
            m_titlebar_path.Transform( m );

            g.SmoothingMode = smold;
            g.Clip = clipold;
        }

        public int Top {
            get {
                return m_bounds.Top;
            }
            set {
                m_bounds = new Rectangle( m_bounds.X, value, m_bounds.Width, m_bounds.Height );
            }
        }

        public int Left {
            get {
                return m_bounds.Left;
            }
            set {
                m_bounds = new Rectangle( value, m_bounds.Y, m_bounds.Width, m_bounds.Height );
            }
        }

        public object Tag {
            get {
                return m_tag;
            }
            set {
                m_tag = value;
            }
        }

        public Rectangle Bounds {
            get {
                return m_bounds;
            }
            set {
                m_bounds = value;
                UpdateBoundPath();
            }
        }

        public int Width {
            get {
                return m_bounds.Width;
            }
            set {
                m_bounds = new Rectangle( m_bounds.X, m_bounds.Y, value, m_bounds.Height );
                UpdateBoundPath();
            }
        }

        public int Height {
            get {
                return m_bounds.Height;
            }
            set {
                m_bounds = new Rectangle( m_bounds.X, m_bounds.Y, m_bounds.Width, value );
                UpdateBoundPath();
            }
        }
    }

}
