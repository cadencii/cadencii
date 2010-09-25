/*
 * PictOverview.cs
 * Copyright (C) 2010 kbinani
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

using java.util.*;
using java.awt.*;
#else
using System;
using System.Threading;
using System.Windows.Forms;
using org.kbinani.java.util;
using org.kbinani.java.awt;
using org.kbinani.windows.forms;
using org.kbinani.vsq;

namespace org.kbinani.cadencii {
    using BMouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using boolean = System.Boolean;
    using BMouseButtons = System.Windows.Forms.MouseButtons;
#endif

    public class PictOverview : BPictureBox, IImageCachedComponentDrawer {
        enum OverviewMouseDownMode {
            NONE,
            LEFT,
            MIDDLE,
        }

        /// <summary>
        /// btnLeft, btnRightを押した時の、スクロール速度(px/sec)。
        /// </summary>
        const float _OVERVIEW_SCROLL_SPEED = 500.0f;
        const int _OVERVIEW_SCALE_COUNT_MAX = 7;
        const int _OVERVIEW_SCALE_COUNT_MIN = 3;

#if !JAVA
        private Graphics mGraphics;
#endif
        private ImageCachedComponentDrawer mDrawer;
        private int mOffsetX;
        private BasicStroke mStrokeDefault = null;
        private BasicStroke mStroke2px = null;
        public int mOverviewDirection = 1;
        public Thread mOverviewUpdateThread = null;
        public int mOverviewStartToDrawClockInitialValue;
        /// <summary>
        /// btnLeftまたはbtnRightが下りた時刻
        /// </summary>
        public double mOverviewBtnDowned;
        /// <summary>
        /// ミニチュア・ピアノロール画面左端でのクロック
        /// </summary>
        public int mOverviewStartToDrawClock = 0;
        /// <summary>
        /// ミニチュア・ピアノロール画面の表示倍率
        /// </summary>
        public float mOverviewPixelPerClock = 0.01f;
        /// <summary>
        /// ミニチュア・ピアノロール画面でマウスが降りている状態かどうか
        /// </summary>
        private OverviewMouseDownMode mOverviewMouseDownMode = OverviewMouseDownMode.NONE;
        /// <summary>
        /// ミニチュア・ピアノロール画面で、マウスが下りた位置のx座標
        /// </summary>
        public int mOverviewMouseDownedLocationX;
        public int mOverviewScaleCount = 5;
        /// <summary>
        /// ミニチュアピアノロールの左側の第1ボタン上でマウスが下りている状態かどうか
        /// </summary>
        private boolean mOverviewButtonLeft1MouseDowned = false;
        /// <summary>
        /// ミニチュアピアノロールの左側の第2ボタン上でマウスが下りている状態かどうか
        /// </summary>
        private boolean mOverviewButtonLeft2MouseDowned = false;
        /// <summary>
        /// ミニチュアピアノロールの右側の第1ボタン上でマウスが下りている状態かどうか
        /// </summary>
        private boolean mOverviewButtonRight1MouseDowned = false;
        /// <summary>
        /// ミニチュアピアノロールの右側の第2ボタン上でマウスが下りている状態かどうか
        /// </summary>
        private boolean mOverviewButtonRight2MouseDowned = false;
        /// <summary>
        /// ミニチュアピアノロールの拡大ボタン上でマウスが下りている状態かどうか
        /// </summary>
        private boolean mOverviewButtonZoomMouseDowned = false;
        /// <summary>
        /// ミニチュアピアノロールの縮小ボタン上でマウスが下りている状態かどうか
        /// </summary>
        private boolean mOverviewButtonMoozMouseDowned = false;
        private FormMain mMainForm = null;
        private Color mBackgroundColor = new Color( 106, 108, 108 );

        public PictOverview() {
#if !JAVA
            this.SetStyle( System.Windows.Forms.ControlStyles.DoubleBuffer, true );
            this.SetStyle( System.Windows.Forms.ControlStyles.UserPaint, true );
#endif
            mDrawer = new ImageCachedComponentDrawer( 100, FormMain._OVERVIEW_HEIGHT );
            registerEventHandlers();
        }

        public void setMainForm( FormMain form ) {
            mMainForm = form;
        }

        public void overviewStopThread() {
            if ( mOverviewUpdateThread != null ) {
                try {
#if JAVA
                    m_overview_update_thread.stop();
                    while( m_overview_update_thread.isAlive() ){
                        Thread.sleep( 0 );
                    }
#else
                    mOverviewUpdateThread.Abort();
                    while ( mOverviewUpdateThread != null && mOverviewUpdateThread.IsAlive ) {
                        System.Windows.Forms.Application.DoEvents();
                    }
#endif
                } catch ( Exception ex ) {
                    Logger.write( typeof( FormMain ) + ".overviewStopThread; ex=" + ex + "\n" );
                }
                mOverviewUpdateThread = null;
            }
        }

        public void btnLeft_MouseDown( Object sender, BMouseEventArgs e ) {
            mOverviewBtnDowned = PortUtil.getCurrentTime();
            mOverviewStartToDrawClockInitialValue = mOverviewStartToDrawClock;
            if ( mOverviewUpdateThread != null ) {
                try {
#if JAVA
                    m_overview_update_thread.stop();
                    while( m_overview_update_thread.isAlive() ){
                        Thread.sleep( 0 );
                    }
#else
                    mOverviewUpdateThread.Abort();
                    while ( mOverviewUpdateThread.IsAlive ) {
                        System.Windows.Forms.Application.DoEvents();
                    }
#endif
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "FormMain#btnLeft_MouseDown; ex=" + ex );
                    Logger.write( typeof( FormMain ) + ".btnLeft_MouseDown; ex=" + ex + "\n" );
                }
                mOverviewUpdateThread = null;
            }
            mOverviewDirection = -1;
#if JAVA
            m_overview_update_thread = new UpdateOverviewProc();
            m_overview_update_thread.start();
#else
            mOverviewUpdateThread = new Thread( new ThreadStart( this.updateOverview ) );
            mOverviewUpdateThread.Start();
#endif
        }

        public void btnLeft_MouseUp( Object sender, BMouseEventArgs e ) {
            overviewStopThread();
        }

        public void btnRight_MouseDown( Object sender, BMouseEventArgs e ) {
            mOverviewBtnDowned = PortUtil.getCurrentTime();
            mOverviewStartToDrawClockInitialValue = mOverviewStartToDrawClock;
            if ( mOverviewUpdateThread != null ) {
                try {
#if JAVA
                    while( m_overview_update_thread.isAlive() ){
                        Thread.sleep( 0 );
                    }
#else
                    while ( mOverviewUpdateThread.IsAlive ) {
                        System.Windows.Forms.Application.DoEvents();
                    }
#endif
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "FormMain#btnRight_MouseDown; ex=" + ex );
                    Logger.write( typeof( FormMain ) + ".btnRight_MouseDown; ex=" + ex + "\n" );
                }
                mOverviewUpdateThread = null;
            }
            mOverviewDirection = 1;
#if JAVA
            m_overview_update_thread = new UpdateOverviewProc();
            m_overview_update_thread.start();
#else
            mOverviewUpdateThread = new Thread( new ThreadStart( this.updateOverview ) );
            mOverviewUpdateThread.Start();
#endif
        }

        public void btnRight_MouseUp( Object sender, BMouseEventArgs e ) {
            overviewStopThread();
        }

        public void btnMooz_Click( Object sender, EventArgs e ) {
            int draft = mOverviewScaleCount - 1;
            if ( draft < _OVERVIEW_SCALE_COUNT_MIN ) {
                draft = _OVERVIEW_SCALE_COUNT_MIN;
            }
            mOverviewScaleCount = draft;
            mOverviewPixelPerClock = getOverviewScaleX( mOverviewScaleCount );
            AppManager.editorConfig.OverviewScaleCount = mOverviewScaleCount;
            updateCachedImage();
            mMainForm.refreshScreen();
        }

        public void btnZoom_Click( Object sender, EventArgs e ) {
            int draft = mOverviewScaleCount + 1;
            if ( _OVERVIEW_SCALE_COUNT_MAX < draft ) {
                draft = _OVERVIEW_SCALE_COUNT_MAX;
            }
            mOverviewScaleCount = draft;
            mOverviewPixelPerClock = getOverviewScaleX( mOverviewScaleCount );
            AppManager.editorConfig.OverviewScaleCount = mOverviewScaleCount;
            updateCachedImage();
            mMainForm.refreshScreen();
        }

        /// <summary>
        /// btnLeft1の描画位置を取得します
        /// </summary>
        /// <returns></returns>
        private Rectangle getButtonBoundsLeft1() {
            return new Rectangle( AppManager.keyWidth - 16 - 2, 1, 16, 26 );
        }

        /// <summary>
        /// btnLeft2の描画位置を取得します
        /// </summary>
        /// <returns></returns>
        private Rectangle getButtonBoundsLeft2() {
            return new Rectangle( AppManager.keyWidth - 16 - 2, 26 + 3, 16, 19 );
        }

        /// <summary>
        /// btnRight1の描画位置を取得します
        /// </summary>
        /// <returns></returns>
        private Rectangle getButtonBoundsRight1() {
            return new Rectangle( getWidth() - 16 - 2, 1, 16, 19 );
        }

        /// <summary>
        /// btnRight2の描画位置を取得します
        /// </summary>
        /// <returns></returns>
        private Rectangle getButtonBoundsRight2() {
            return new Rectangle( getWidth() - 16 - 2, 19 + 3, 16, 26 );
        }

        /// <summary>
        /// Zoomボタンの描画位置を取得します
        /// </summary>
        /// <returns></returns>
        private Rectangle getButtonBoundsZoom() {
            return new Rectangle( AppManager.keyWidth - 16 - 2 - 24, 13, 22, 23 );
        }

        /// <summary>
        /// Moozボタンの描画位置を取得します
        /// </summary>
        /// <returns></returns>
        private Rectangle getButtonBoundsMooz() {
            return new Rectangle( AppManager.keyWidth - 16 - 2 - 48, 13, 22, 23 );
        }

        public void updateCachedImage() {
            VsqFileEx vsq = AppManager.getVsqFile();
            int required_width = (int)(vsq.TotalClocks * mOverviewPixelPerClock) + mMainForm.pictPianoRoll.getWidth();
            mDrawer.setWidth( required_width );
            mDrawer.updateCache( this );
        }

#if JAVA
        public class UpdateOverviewProc extends Thread{
        public void run(){
#else
        public void updateOverview() {
#endif
            boolean д = true;
#if DEBUG
            int count = 0;
#endif
            for ( ; д; ) {
#if DEBUG
                count++;
                PortUtil.println( "FormMain#updateOverview; count=" + count );
#endif
#if JAVA
                try{
                    Thread.sleep( 100 );
                }catch( InterruptedException ex ){
                    Logger.write( FormMain.class + "; ex=" + ex + "\n" );
                    break;
                }
#else
                Thread.Sleep( 100 );
#endif
                int key_width = AppManager.keyWidth;
                double dt = PortUtil.getCurrentTime() - mOverviewBtnDowned;
                int draft = (int)(mOverviewStartToDrawClockInitialValue + mOverviewDirection * dt * _OVERVIEW_SCROLL_SPEED / mOverviewPixelPerClock);
                int clock = getOverviewClockFromXCoord( getWidth() - key_width, draft );
                if ( AppManager.getVsqFile().TotalClocks < clock ) {
                    draft = AppManager.getVsqFile().TotalClocks - (int)((getWidth() - key_width) / mOverviewPixelPerClock);
                }
                if ( draft < 0 ) {
                    draft = 0;
                }
                mOverviewStartToDrawClock = draft;
#if JAVA
                if ( this == null ) {
#else
                if ( this == null || (this != null && this.IsDisposed) ) {
#endif
                    break;
                }
#if JAVA
                repaint();
#else
                mMainForm.refreshScreen();// this.Invoke( new BEventHandler( invalidatePictOverview ) );
#endif
            }
#if JAVA
        }
#endif
        }

        public float getOverviewScaleX( int scale_count ) {
            return (float)Math.Pow( 10.0, 0.2 * scale_count - 3.0 );
        }

        /// <summary>
        /// ミニチュア・ピアノロール上のマウスの位置から、ピアノロールに設定するべきStartToDrawXの値を計算します。
        /// </summary>
        /// <param name="mouse_x"></param>
        /// <returns></returns>
        public int getOverviewStartToDrawX( int mouse_x ) {
            float clock = mouse_x / mOverviewPixelPerClock + mOverviewStartToDrawClock;
            int clock_at_left = (int)(clock - (mMainForm.pictPianoRoll.getWidth() - AppManager.keyWidth) * AppManager.getScaleXInv() / 2);
            return (int)(clock_at_left * AppManager.getScaleX());
        }

        public int getOverviewXCoordFromClock( int clock ) {
            return (int)((clock - mOverviewStartToDrawClock) * mOverviewPixelPerClock);
        }

        public int getOverviewClockFromXCoord( int x, int start_to_draw_clock ) {
            return (int)(x / mOverviewPixelPerClock) + start_to_draw_clock;
        }

        public int getOverviewClockFromXCoord( int x ) {
            return getOverviewClockFromXCoord( x, mOverviewStartToDrawClock );
        }

        private void registerEventHandlers() {
            mouseDownEvent.add( new BMouseEventHandler( this, "handleMouseDown" ) );
            mouseUpEvent.add( new BMouseEventHandler( this, "handleMouseUp" ) );
            mouseMoveEvent.add( new BMouseEventHandler( this, "handleMouseMove" ) );
            mouseDoubleClickEvent.add( new BMouseEventHandler( this, "handleMouseDoubleClick" ) );
            mouseLeaveEvent.add( new BEventHandler( this, "handleMouseLeave" ) );
            resizeEvent.add( new BEventHandler( this, "handleResize" ) );
        }

        public void handleResize( Object sender, EventArgs e ) {
            VsqFileEx vsq = AppManager.getVsqFile();
            int min_width = (int)(vsq.TotalClocks * mOverviewPixelPerClock) + getWidth();
            if ( mDrawer.getWidth() < min_width ) {
                mDrawer.setWidth( min_width );
                updateCachedImage();
            }
        }

        public void handleMouseLeave( Object sender, EventArgs e ) {
            overviewStopThread();
        }

        public void handleMouseDoubleClick( Object sender, BMouseEventArgs e ) {
            if ( AppManager.keyWidth < e.X && e.X < getWidth() - 19 ) {
                mOverviewMouseDownMode = OverviewMouseDownMode.NONE;
                int draft_stdx = getOverviewStartToDrawX( e.X - AppManager.keyWidth - AppManager.keyOffset );
                int draft = (int)(draft_stdx * AppManager.getScaleXInv());
                if ( draft < mMainForm.hScroll.getMinimum() ) {
                    draft = mMainForm.hScroll.getMinimum();
                } else if ( mMainForm.hScroll.getMaximum() < draft ) {
                    draft = mMainForm.hScroll.getMaximum();
                }
                mMainForm.hScroll.setValue( draft );
                mMainForm.refreshScreen();
            }
        }

        public void handleMouseDown( Object sender, BMouseEventArgs e ) {
            BMouseButtons btn = e.Button;
            if ( mMainForm.isMouseMiddleButtonDowned( e.Button ) ) {
                btn = BMouseButtons.Middle;
            }
            if ( btn == BMouseButtons.Middle ) {
                mOverviewMouseDownMode = OverviewMouseDownMode.MIDDLE;
                mOverviewMouseDownedLocationX = e.X;
                mOverviewStartToDrawClockInitialValue = mOverviewStartToDrawClock;
            } else if ( e.Button == BMouseButtons.Left ) {
                if ( e.X <= AppManager.keyWidth || getWidth() - 19 <= e.X ) {
                    Point mouse = new Point( e.X, e.Y );
                    if ( Utility.isInRect( mouse, getButtonBoundsLeft1() ) ) {
                        btnLeft_MouseDown( null, null );
                        mOverviewButtonLeft1MouseDowned = true;
                    } else if ( Utility.isInRect( mouse, getButtonBoundsRight1() ) ) {
                        btnLeft_MouseDown( null, null );
                        mOverviewButtonRight1MouseDowned = true;
                    } else if ( Utility.isInRect( mouse, getButtonBoundsLeft2() ) ) {
                        btnRight_MouseDown( null, null );
                        mOverviewButtonLeft2MouseDowned = true;
                    } else if ( Utility.isInRect( mouse, getButtonBoundsRight2() ) ) {
                        btnRight_MouseDown( null, null );
                        mOverviewButtonRight2MouseDowned = true;
                    } else if ( Utility.isInRect( mouse, getButtonBoundsZoom() ) ) {
                        btnZoom_Click( null, null );
                        mOverviewButtonZoomMouseDowned = true;
                    } else if ( Utility.isInRect( mouse, getButtonBoundsMooz() ) ) {
                        btnMooz_Click( null, null );
                        mOverviewButtonMoozMouseDowned = true;
                    }
                    mMainForm.refreshScreen();
                } else {
                    if ( e.Clicks == 1 ) {
                        mOverviewMouseDownMode = OverviewMouseDownMode.LEFT;
                        int draft = getOverviewStartToDrawX( e.X - AppManager.keyWidth - AppManager.keyOffset );
                        if ( draft < 0 ) {
                            draft = 0;
                        }
                        AppManager.setStartToDrawX( draft );
                        mMainForm.refreshScreen();
                        return;
                    }
                }
            }
        }

        public void handleMouseUp( Object sender, BMouseEventArgs e ) {
            Point mouse = new Point( e.X, e.Y );
            if ( Utility.isInRect( mouse, getButtonBoundsLeft1() ) ) {
                btnLeft_MouseUp( null, null );
            } else if ( Utility.isInRect( mouse, getButtonBoundsRight1() ) ) {
                btnLeft_MouseUp( null, null );
            } else if ( Utility.isInRect( mouse, getButtonBoundsLeft2() ) ) {
                btnRight_MouseUp( null, null );
            } else if ( Utility.isInRect( mouse, getButtonBoundsRight2() ) ) {
                btnRight_MouseUp( null, null );
            }
            mOverviewButtonLeft1MouseDowned = false;
            mOverviewButtonLeft2MouseDowned = false;
            mOverviewButtonRight1MouseDowned = false;
            mOverviewButtonRight2MouseDowned = false;
            mOverviewButtonZoomMouseDowned = false;
            mOverviewButtonMoozMouseDowned = false;
            if ( mOverviewMouseDownMode == OverviewMouseDownMode.LEFT ) {
                AppManager.setStartToDrawX( mMainForm.calculateStartToDrawX() );
            }
            mOverviewMouseDownMode = OverviewMouseDownMode.NONE;
            mMainForm.refreshScreen();
        }

        public void handleMouseMove( Object sender, BMouseEventArgs e ) {
            int xoffset = AppManager.keyWidth + AppManager.keyOffset;
            if ( mOverviewMouseDownMode == OverviewMouseDownMode.LEFT ) {
                int draft = getOverviewStartToDrawX( e.X - xoffset );
                if ( draft < 0 ) {
                    draft = 0;
                }
                AppManager.setStartToDrawX( draft );
                mMainForm.refreshScreen();
            } else if ( mOverviewMouseDownMode == OverviewMouseDownMode.MIDDLE ) {
                int dx = e.X - mOverviewMouseDownedLocationX;
                int draft = mOverviewStartToDrawClockInitialValue - (int)(dx / mOverviewPixelPerClock);
                int key_width = AppManager.keyWidth;
                int clock = getOverviewClockFromXCoord( getWidth() - xoffset, draft );
                if ( AppManager.getVsqFile().TotalClocks < clock ) {
                    draft = AppManager.getVsqFile().TotalClocks - (int)((getWidth() - xoffset) / mOverviewPixelPerClock);
                }
                if ( draft < 0 ) {
                    draft = 0;
                }
                mOverviewStartToDrawClock = draft;
                mMainForm.refreshScreen();
            }
        }

        private BasicStroke getStroke2px() {
            if ( mStroke2px == null ) {
                mStroke2px = new BasicStroke( 2.0f );
            }
            return mStroke2px;
        }

        private BasicStroke getStrokeDefault() {
            if ( mStrokeDefault == null ) {
                mStrokeDefault = new BasicStroke();
            }
            return mStrokeDefault;
        }

        public void paint( Graphics g1 ) {
#if JAVA
            Graphics2D g = (Graphics2D)g1;
#else
            Graphics2D g = new Graphics2D( g1.nativeGraphics );
#endif
            int doffset = (int)(mOverviewStartToDrawClock * mOverviewPixelPerClock);
            mDrawer.draw( doffset, g );

            int key_width = AppManager.keyWidth;
            int width = getWidth();
            int height = getHeight();
            int xoffset = key_width + AppManager.keyOffset;
            int current_start = AppManager.clockFromXCoord( key_width );
            int current_end = AppManager.clockFromXCoord( mMainForm.pictPianoRoll.getWidth() );
            int x_start = getOverviewXCoordFromClock( current_start );
            int x_end = getOverviewXCoordFromClock( current_end );

            // 移動中している最中に，移動開始直前の部分を影付で表示する
            int stdx = AppManager.getStartToDrawX();
            int act_start_to_draw_x = (int)(mMainForm.hScroll.getValue() * AppManager.getScaleX());
            if ( act_start_to_draw_x != stdx ) {
                int act_start_clock = AppManager.clockFromXCoord( key_width - stdx + act_start_to_draw_x );
                int act_end_clock = AppManager.clockFromXCoord( mMainForm.pictPianoRoll.getWidth() - stdx + act_start_to_draw_x );
                int act_start_x = getOverviewXCoordFromClock( act_start_clock );
                int act_end_x = getOverviewXCoordFromClock( act_end_clock );
                Rectangle rcm = new Rectangle( act_start_x, 0, act_end_x - act_start_x, height );
                g.setColor( new Color( 0, 0, 0, 100 ) );
                g.fillRect( rcm.x + xoffset, rcm.y, rcm.width, rcm.height );
            }

            // 現在の表示範囲
            Rectangle rc = new Rectangle( x_start, 0, x_end - x_start, height - 1 );
            g.setColor( new Color( 255, 255, 255, 50 ) );
            g.fillRect( rc.x + xoffset, rc.y, rc.width, rc.height );
            g.setColor( AppManager.getHilightColor() );
            g.drawRect( rc.x + xoffset, rc.y, rc.width, rc.height );

            // ソングポジション
            int px_current_clock = (int)((AppManager.getCurrentClock() - mOverviewStartToDrawClock) * mOverviewPixelPerClock);
            g.setStroke( getStroke2px() );
            g.setColor( Color.white );
            g.drawLine( px_current_clock + xoffset, 0, px_current_clock + xoffset, height );
            g.setStroke( getStrokeDefault() );

            int btn_width = 16;
            Color btn_bg = new Color( 149, 149, 149 );
            // 左側のボタン類
            g.setStroke( getStrokeDefault() );
            g.setColor( btn_bg );
            g.fillRect( 0, 0, key_width, height );
            g.setColor( AppManager.COLOR_BORDER );
            // zoomボタン
            rc = getButtonBoundsZoom();
            g.setColor( mOverviewButtonZoomMouseDowned ? Color.gray : Color.lightGray );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );
            g.setColor( AppManager.COLOR_BORDER );
            g.drawRect( rc.x, rc.y, rc.width, rc.height );
            int centerx = rc.x + rc.width / 2 + 1;
            int centery = rc.y + rc.height / 2 + 1;
            g.setColor( mOverviewButtonZoomMouseDowned ? Color.lightGray : Color.gray );
            g.setStroke( getStroke2px() );
            g.drawLine( centerx - 4, centery, centerx + 4, centery );
            g.drawLine( centerx, centery - 4, centerx, centery + 4 );
            g.setStroke( getStrokeDefault() );
            // moozボタン
            rc = getButtonBoundsMooz();
            g.setColor( mOverviewButtonMoozMouseDowned ? Color.gray : Color.lightGray );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );
            g.setColor( AppManager.COLOR_BORDER );
            g.drawRect( rc.x, rc.y, rc.width, rc.height );
            centerx = rc.x + rc.width / 2 + 1;
            centery = rc.y + rc.height / 2 + 1;
            g.setColor( mOverviewButtonMoozMouseDowned ? Color.lightGray : Color.gray );
            g.setStroke( getStroke2px() );
            g.drawLine( centerx - 4, centery, centerx + 4, centery );
            g.setStroke( getStrokeDefault() );
            // left1ボタン
            rc = getButtonBoundsLeft1();
            g.setColor( mOverviewButtonLeft1MouseDowned ? Color.gray : Color.lightGray );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );
            g.setColor( AppManager.COLOR_BORDER );
            g.drawRect( rc.x, rc.y, rc.width, rc.height );
            centerx = rc.x + rc.width / 2 + 1;
            centery = rc.y + rc.height / 2 + 1;
            g.setColor( mOverviewButtonLeft1MouseDowned ? Color.lightGray : Color.gray );
            g.drawPolyline( new int[] { centerx + 4, centerx - 4, centerx + 4 }, new int[] { centery - 4, centery, centery + 4 }, 3 );
            // left2ボタン
            rc = getButtonBoundsLeft2();
            g.setColor( mOverviewButtonLeft2MouseDowned ? Color.gray : Color.lightGray );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );
            g.setColor( AppManager.COLOR_BORDER );
            g.drawRect( rc.x, rc.y, rc.width, rc.height );
            centerx = rc.x + rc.width / 2 + 1;
            centery = rc.y + rc.height / 2 + 1;
            g.setColor( mOverviewButtonLeft2MouseDowned ? Color.lightGray : Color.gray );
            g.drawPolyline( new int[] { centerx - 4, centerx + 4, centerx - 4 }, new int[] { centery - 4, centery, centery + 4 }, 3 );

            // 右側のボタン類
            g.setColor( btn_bg );
            g.fillRect( width - btn_width - 3, 0, btn_width + 3, height );
            // right1ボタン
            rc = getButtonBoundsRight1();
            g.setColor( mOverviewButtonRight1MouseDowned ? Color.gray : Color.lightGray );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );
            g.setColor( AppManager.COLOR_BORDER );
            g.drawRect( rc.x, rc.y, rc.width, rc.height );
            centerx = rc.x + rc.width / 2 + 1;
            centery = rc.y + rc.height / 2 + 1;
            g.setColor( mOverviewButtonRight1MouseDowned ? Color.lightGray : Color.gray );
            g.drawPolyline( new int[] { centerx + 4, centerx - 4, centerx + 4 }, new int[] { centery - 4, centery, centery + 4 }, 3 );
            // right2ボタン
            rc = getButtonBoundsRight2();
            g.setColor( mOverviewButtonRight2MouseDowned ? Color.gray : Color.lightGray );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );
            g.setColor( AppManager.COLOR_BORDER );
            g.drawRect( rc.x, rc.y, rc.width, rc.height );
            centerx = rc.x + rc.width / 2 + 1;
            centery = rc.y + rc.height / 2 + 1;
            g.setColor( mOverviewButtonRight2MouseDowned ? Color.lightGray : Color.gray );
            g.drawPolyline( new int[] { centerx - 4, centerx + 4, centerx - 4 }, new int[] { centery - 4, centery, centery + 4 }, 3 );
        }

        public void draw( Graphics2D g, int width, int height ) {
            if ( mMainForm == null ) {
                return;
            }

            g.setColor( mBackgroundColor );
            g.fillRect( 0, 0, width, height );

            g.setStroke( getStroke2px() );
            g.setColor( FormMain.mColorNoteFill );
            int key_width = AppManager.keyWidth;
            int xoffset = key_width + AppManager.keyOffset;
            VsqFileEx vsq = AppManager.getVsqFile();
            int selected = AppManager.getSelected();

            int overview_dot_diam = 2;
            Vector<DrawObject> objs = AppManager.drawObjects.get( selected - 1 );

            // 平均ノートナンバーを調べる
            double sum = 0.0;
            int count = 0;
            for ( Iterator<DrawObject> itr = objs.iterator(); itr.hasNext(); ) {
                DrawObject dobj = itr.next();
                sum += dobj.mNote;
                count++;
            }
            float average_note = (float)(sum / (double)count);

            for ( Iterator<DrawObject> itr = objs.iterator(); itr.hasNext(); ) {
                DrawObject dobj = itr.next();
                int x = (int)(dobj.mClock * mOverviewPixelPerClock);
                if ( x < 0 ) {
                    continue;
                }
                if ( width - key_width < x ) {
                    break;
                }
                int y = height - (height / 2 + (int)((dobj.mNote - average_note) * overview_dot_diam));
                int length = (int)(dobj.mLength * mOverviewPixelPerClock);
                if ( length < overview_dot_diam ) {
                    length = overview_dot_diam;
                }
                g.drawLine( x + xoffset, y, x + length + xoffset, y );
            }
            g.setStroke( getStrokeDefault() );
            //}
            int current_start = AppManager.clockFromXCoord( key_width );
            int current_end = AppManager.clockFromXCoord( mMainForm.pictPianoRoll.getWidth() );
            int x_start = (int)(current_start * mOverviewPixelPerClock);
            int x_end = (int)(current_end * mOverviewPixelPerClock);

            // 小節ごとの線
            int clock_start = 0;
            int clock_end = (int)(width / mOverviewPixelPerClock);
            int premeasure = vsq.getPreMeasure();
            g.setClip( null );
            Color pen_color = new java.awt.Color( 0, 0, 0, 130 );

            int barcountx = 0;
            String barcountstr = "";
            for ( Iterator<VsqBarLineType> itr = vsq.getBarLineIterator( clock_end * 3 / 2 ); itr.hasNext(); ) {
                VsqBarLineType bar = itr.next();
                if ( bar.clock() < clock_start ) {
                    continue;
                }
                if ( width - key_width < barcountx ) {
                    break;
                }
                if ( bar.isSeparator() ) {
                    int barcount = bar.getBarCount() - premeasure + 1;
                    int x = (int)(bar.clock() * mOverviewPixelPerClock);
                    if ( (barcount % 5 == 0 && barcount > 0) || barcount == 1 ) {
                        g.setColor( pen_color );
                        g.setStroke( getStroke2px() );
                        g.drawLine( x + xoffset, 0, x + xoffset, height );

                        g.setStroke( getStrokeDefault() );
                        if ( !barcountstr.Equals( "" ) ) {
                            g.setColor( Color.white );
                            g.setFont( AppManager.baseFont9 );
                            g.drawString( barcountstr, barcountx + 1 + xoffset, 1 + AppManager.baseFont9Height / 2 - AppManager.baseFont9OffsetHeight + 1 );
                        }
                        barcountstr = barcount + "";
                        barcountx = x;
                    } else {
                        g.setColor( pen_color );
                        g.drawLine( x + xoffset, 0, x + xoffset, height );
                    }
                }
            }
            g.setClip( null );

        }

        public void setOffsetX( int value ) {
            mOffsetX = value;
        }

#if !JAVA
        protected override void OnPaint( PaintEventArgs pevent ) {
            base.OnPaint( pevent );
            if ( mGraphics == null ) {
                mGraphics = new Graphics( null );
            }
            mGraphics.nativeGraphics = pevent.Graphics;
            paint( mGraphics );
        }
#endif
    }

#if !JAVA
}
#endif
