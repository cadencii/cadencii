/*
 * WaveformZoomPanel.cs
 * Copyright © 2011 kbinani
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

import org.kbinani.windows.forms.*;

#else

using System;

using org.kbinani.java.awt;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii
{
    using BMouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using boolean = System.Boolean;
    using BPaintEventArgs = System.Windows.Forms.PaintEventArgs;
#endif

    /// <summary>
    /// 波形表示の拡大・縮小を行うためのパネルです．
    /// </summary>
#if JAVA
    class WaveformZoomPanel extends BPanel
#else
    class WaveformZoomPanel : BPanel
#endif
    {

        /// <summary>
        /// このビューのController
        /// </summary>
        private WaveformZoomController mController = null;
        /// <summary>
        /// 波形表示部の拡大ボタン上でマウスが下りた状態かどうか
        /// </summary>
        private boolean mWaveViewButtonZoomMouseDowned = false;
        /// <summary>
        /// 波形表示部のAutoMaximizeボタン上でマウスが下りた状態かどうか
        /// </summary>
        private boolean mWaveViewButtonAutoMaximizeMouseDowned = false;
        /// <summary>
        /// 波形表示部の縦軸の拡大率を自動最大化するかどうか
        /// </summary>
        private boolean mWaveViewAutoMaximize = false;
        /// <summary>
        /// 波形表示部分のズーム時に，マウスが下りた瞬間のY座標
        /// </summary>
        private int mWaveViewMouseDownedLocationY;
        /// <summary>
        /// 波形表示部の拡大ボタン上でマウスが下りた瞬間の，波形表示部の縦軸拡大率．
        /// </summary>
        private float mWaveViewInitScale;
#if !JAVA
        private Graphics2D mGraphicsPanel2 = null;
#endif

        /// <summary>
        /// Wave表示部等のボタンと他のコンポーネントの間のスペース
        /// </summary>
        const int SPACE = 4;

        public WaveformZoomPanel( WaveformZoomController controller )
        {
            mController = controller;
        }

        /// <summary>
        /// 波形表示部のズームボタンの形を取得します
        /// </summary>
        /// <returns></returns>
        private Rectangle getButtonBoundsWaveViewZoom()
        {
            int width = AppManager.keyWidth - 1;
            int height = getHeight() - 1;

            int y = SPACE + 16 + SPACE;
            return new Rectangle( SPACE, y, width - SPACE - SPACE, height - SPACE - y );
        }

#if !JAVA
        protected override void OnPaint( System.Windows.Forms.PaintEventArgs e )
        {
            base.OnPaint( e );
            if ( mGraphicsPanel2 == null )
            {
                mGraphicsPanel2 = new Graphics2D( null );
            }
            mGraphicsPanel2.nativeGraphics = e.Graphics;
            Graphics g = mGraphicsPanel2;
            paint( g );
        }

        protected override void OnMouseDown( System.Windows.Forms.MouseEventArgs e )
        {
            base.OnMouseDown( e );
            panelWaveformZoom_MouseDown( this, e );
        }

        protected override void OnMouseMove( System.Windows.Forms.MouseEventArgs e )
        {
            base.OnMouseMove( e );
            panelWaveformZoom_MouseMove( this, e );
        }

        protected override void OnMouseUp( System.Windows.Forms.MouseEventArgs e )
        {
            base.OnMouseUp( e );
            panelWaveformZoom_MouseUp( this, e );
        }
#endif

        public void panelWaveformZoom_MouseDown( Object sender, BMouseEventArgs e )
        {
            Point p = new Point( e.X, e.Y );

            int width = AppManager.keyWidth - 1;
            int height = getHeight();

            // AutoMaximizeボタン
            Rectangle rc = new Rectangle( SPACE, SPACE, width - SPACE - SPACE, 16 );
            if ( Utility.isInRect( p, rc ) )
            {
                mWaveViewButtonAutoMaximizeMouseDowned = true;
                mWaveViewButtonZoomMouseDowned = false;

                repaint();
                return;
            }

            if ( !mWaveViewAutoMaximize )
            {
                // Zoomボタン
                rc = getButtonBoundsWaveViewZoom();
                if ( Utility.isInRect( p, rc ) )
                {
                    mWaveViewMouseDownedLocationY = p.y;
                    mWaveViewButtonZoomMouseDowned = true;
                    mWaveViewButtonAutoMaximizeMouseDowned = false;
                    mWaveViewInitScale = mController.getScale();

                    repaint();
                    return;
                }
            }

            mWaveViewButtonAutoMaximizeMouseDowned = false;
            mWaveViewButtonZoomMouseDowned = false;
            repaint();
            return;
        }

        public void panelWaveformZoom_MouseMove( Object sender, BMouseEventArgs e )
        {
            if ( !mWaveViewButtonZoomMouseDowned )
            {
                return;
            }

            int height = getHeight();
            int delta = mWaveViewMouseDownedLocationY - e.Y;
            float scale = mWaveViewInitScale + delta * 3.0f / height * mWaveViewInitScale;
            mController.setScale( scale );

            mController.refreshScreen();
        }

        public void panelWaveformZoom_MouseUp( Object sender, BMouseEventArgs e )
        {
            int width = AppManager.keyWidth - 1;
            int height = getHeight();

            // AutoMaximizeボタン
            if ( Utility.isInRect( e.X, e.Y, SPACE, SPACE, width - SPACE - SPACE, 16 ) )
            {
                if ( mWaveViewButtonAutoMaximizeMouseDowned )
                {
                    mWaveViewAutoMaximize = !mWaveViewAutoMaximize;
                    mController.setAutoMaximize( mWaveViewAutoMaximize );
                }
            }

            mWaveViewButtonAutoMaximizeMouseDowned = false;
            mWaveViewButtonZoomMouseDowned = false;
#if JAVA
            refreshScreen();
#else
            repaint();
#endif
        }

        public void paint( Graphics g )
        {
            int key_width = AppManager.keyWidth;
            int width = key_width - 1;
            int height = getHeight() - 1;

            // 背景を塗る
            g.setColor( PortUtil.DarkGray );
            g.fillRect( 0, 0, width, height );

            // AutoMaximizeのチェックボックスを描く
            g.setColor( mWaveViewButtonAutoMaximizeMouseDowned ? PortUtil.Gray : PortUtil.LightGray );
            g.fillRect( SPACE, SPACE, 16, 16 );
            g.setColor( PortUtil.Gray );
            g.drawRect( SPACE, SPACE, 16, 16 );
            if ( mWaveViewAutoMaximize )
            {
                g.setColor( PortUtil.Gray );
                g.fillRect( SPACE + 3, SPACE + 3, 11, 11 );
            }
            g.setColor( Color.black );
            g.setFont( AppManager.baseFont8 );
            g.drawString(
                "Auto Maximize",
                SPACE + 16 + SPACE,
                SPACE + AppManager.baseFont8Height / 2 - AppManager.baseFont8OffsetHeight + 1 );

            // ズーム用ボタンを描く
            int zoom_button_y = SPACE + 16 + SPACE;
            int zoom_button_height = height - SPACE - zoom_button_y;
            Rectangle rc = getButtonBoundsWaveViewZoom();
            if ( !mWaveViewAutoMaximize )
            {
                g.setColor( mWaveViewButtonZoomMouseDowned ? PortUtil.Gray : PortUtil.LightGray );
                g.fillRect( rc.x, rc.y, rc.width, rc.height );
            }
            g.setColor( PortUtil.Gray );
            g.drawRect( rc.x, rc.y, rc.width, rc.height );
            g.setColor( mWaveViewAutoMaximize ? PortUtil.Gray : Color.black );
            rc.y = rc.y + 1;
            PortUtil.drawStringEx(
                g, (mWaveViewButtonZoomMouseDowned ? "↑Move Mouse↓" : "Zoom"), AppManager.baseFont9,
                rc, PortUtil.STRING_ALIGN_CENTER, PortUtil.STRING_ALIGN_CENTER );
        }
    
    }

#if JAVA

#else

}

#endif
