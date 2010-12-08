/*
 * CircuitView.cs
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

import java.util.*;
import java.awt.*;
import javax.swing.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.java.awt;
using org.kbinani.java.util;
using org.kbinani.javax.swing;
using org.kbinani.windows.forms;

//TODO: draftが格上げになったら消すこと
using org.kbinani.cadencii.draft;

namespace org.kbinani.cadencii {
    using BEventArgs = System.EventArgs;
    using BMouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// シンセサイザ等の接続回路を表示、編集するためのコンポーネント
    /// </summary>
#if JAVA
    public class CircuitView extends BPictureBox {
#else
    public class CircuitView : BPictureBox {
#endif
        /// <summary>
        /// ポートを表す丸印の半径
        /// </summary>
        private const int PORT_RADIUS = 3;

        /// <summary>
        /// 回路
        /// </summary>
        private Circuit mCircuit = null;
        /// <summary>
        /// グラフィックス
        /// </summary>
        private Graphics mGraphics = null;
        /// <summary>
        /// 現在移動中のユニットの番号
        /// </summary>
        private int mMovingUnitIndex = -1;
        /// <summary>
        /// 移動開始時の、装置の左位置とマウスX位置の差
        /// </summary>
        private int mMovingOffsetX = 0;
        /// <summary>
        /// 移動開始時の、装置の上位置とマウスY位置の差
        /// </summary>
        private int mMovingOffsetY = 0;
        /// <summary>
        /// デフォルトのストローク
        /// </summary>
        private BasicStroke mStrokeDefault = null;
        /// <summary>
        /// 幅が2ピクセルのストローク
        /// </summary>
        private BasicStroke mStroke2px = null;

        public CircuitView() {
            MouseDown += new System.Windows.Forms.MouseEventHandler( handleMouseDown );
            MouseUp += new System.Windows.Forms.MouseEventHandler( handleMouseUp );
            MouseMove += new System.Windows.Forms.MouseEventHandler( handleMouseMove );
        }

        #region event handlers
        public void handleMouseDown( Object sender, BMouseEventArgs e ) {
            Point p = new Point( e.X, e.Y );
            //TODO: マウス位置にあるポートを調べる

            // マウス位置にある装置を調べる
            for ( int i = 0; i < mCircuit.mUnits.size(); i++ ) {
                Rectangle rc = getUnitBounds( i );
                if ( Utility.isInRect( p, rc ) ) {
                    mMovingUnitIndex = i;
                    mMovingOffsetX = p.x - rc.x;
                    mMovingOffsetY = p.y - rc.y;
                    return;
                }
            }

            mMovingUnitIndex = -1;
        }

        public void handleMouseUp( Object sender, BMouseEventArgs e ) {
            if ( mMovingUnitIndex < 0 ) {
                return;
            }
            handleMouseMove( sender, e );
            mMovingUnitIndex = -1;
        }

        public void handleMouseMove( Object sender, BMouseEventArgs e ) {
            if ( mMovingUnitIndex < 0 ) {
                return;
            }
            Point p = new Point( e.X, e.Y );
            Point newloc = new Point( e.X - mMovingOffsetX, e.Y - mMovingOffsetY );
            mCircuit.mConfig.DrawPosition.set( mMovingUnitIndex, newloc );
            invalidate();
        }
        #endregion

        #region public methods
        /// <summary>
        /// 描画ループ
        /// </summary>
        /// <param name="g"></param>
        public void paint( Graphics g ) {
            int diam = PORT_RADIUS * 2 + 1;
            int num = mCircuit.mUnits.size();

            int[] port_usage_in = new int[num];
            int[] port_usage_out = new int[num];
            // 装置間の接続を描く
            g.setColor( Color.black );
            g.setStroke( getStroke2px() );
            for ( int i = 0; i < num; i++ ) {
                for ( int j = 0; j < num; j++ ) {
                    boolean connected = mCircuit.isConnected( i, j );
                    if ( !connected ) continue;

                    Rectangle rc_i = getUnitBounds( i );
                    int ports_out_i = mCircuit.mNumPortsOut.get( i );
                    int delta_h_out_i = rc_i.height / ports_out_i;

                    Rectangle rc_j = getUnitBounds( j );
                    int ports_in_j = mCircuit.mNumPortsIn.get( j );
                    int delta_h_in_j = rc_j.height / ports_in_j;

                    int x_i = rc_i.x + rc_i.width;
                    int y_i = rc_i.y + delta_h_out_i / 2 + delta_h_out_i * port_usage_out[i];

                    int x_j = rc_j.x;
                    int y_j = rc_j.y + delta_h_in_j / 2 + delta_h_in_j * port_usage_in[j];

                    g.drawLine( x_i, y_i, x_j, y_j );

                    port_usage_out[i] += 1;
                    port_usage_in[j] += 1;
                }
            }

            // 装置を描く
            for ( int i = 0; i < num; i++ ) {
                WaveUnit unit = mCircuit.mUnits.get( i );
                Rectangle rc = getUnitBounds( i );
                unit.paintTo( g, rc.x, rc.y, rc.width, rc.height );
            }

            // 入出力ポートの丸印を描く
            g.setStroke( getStrokeDefault() );
            for ( int i = 0; i < num; i++ ) {
                WaveUnit unit = mCircuit.mUnits.get( i );
                Rectangle rc = getUnitBounds( i );
                int ports_in = mCircuit.mNumPortsIn.get( i );
                int ports_out = mCircuit.mNumPortsOut.get( i );

                // 入力ポートの丸印を描く
                if ( ports_in > 0 ) {
                    int delta_h = rc.height / ports_in;
                    for ( int k = 0; k < ports_in; k++ ) {
                        int x = rc.x - PORT_RADIUS;
                        int y = rc.y + delta_h / 2 + k * delta_h - PORT_RADIUS;
                        g.setColor( Color.white );
                        g.fillOval( x, y, diam, diam );
                        g.setColor( Color.black );
                        g.drawOval( x, y, diam, diam );
                    }
                }

                // 出力ポートの丸印を描く
                if ( ports_out > 0 ) {
                    int delta_h = rc.height / ports_out;
                    for ( int k = 0; k < ports_out; k++ ) {
                        int x = rc.x + rc.width - PORT_RADIUS;
                        int y = rc.y + delta_h / 2 + k * delta_h - PORT_RADIUS;
                        g.setColor( Color.white );
                        g.fillOval( x, y, diam, diam );
                        g.setColor( Color.black );
                        g.drawOval( x, y, diam, diam );
                    }
                }
            }
        }

        /// <summary>
        /// このコンポーネントで表示・編集する回路を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setCircuit( Circuit value ) {
            mCircuit = value;
        }
        #endregion

        #region private methods
        /// <summary>
        /// デフォルトのストロークを取得します
        /// </summary>
        /// <returns></returns>
        private BasicStroke getStrokeDefault() {
            if ( mStrokeDefault == null ) {
                mStrokeDefault = new BasicStroke();
            }
            return mStrokeDefault;
        }

        /// <summary>
        /// 幅が2ピクセルのストロークを取得します
        /// </summary>
        /// <returns></returns>
        private BasicStroke getStroke2px() {
            if ( mStroke2px == null ) {
                mStroke2px = new BasicStroke( 2f );
            }
            return mStroke2px;
        }

        /// <summary>
        /// 指定した番号の装置の、画面上での表示位置・サイズを計算します
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Rectangle getUnitBounds( int index ) {
            Point pos = mCircuit.mConfig.DrawPosition.get( index );
            int ports_in = mCircuit.mNumPortsIn.get( index );
            int ports_out = mCircuit.mNumPortsOut.get( index );
            int width = WaveUnit.BASE_WIDTH;
            int height = WaveUnit.BASE_HEIGHT_PER_PORTS * ((ports_out > ports_in ? ports_out : ports_in) + 1);
            return new Rectangle( pos.x, pos.y, width, height );
        }
        #endregion

#if !JAVA
        protected override void OnPaint( System.Windows.Forms.PaintEventArgs pevent ) {
            base.OnPaint( pevent );
            if ( mGraphics == null ) {
                mGraphics = new Graphics( pevent.Graphics );
            } else {
                mGraphics.nativeGraphics = pevent.Graphics;
            }
            paint( mGraphics );
        }
#endif

    }

#if !JAVA
}
#endif
