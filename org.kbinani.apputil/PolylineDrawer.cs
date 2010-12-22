/*
 * PolylineDrawer.cs
 * Copyright © 2010 kbinani
 *
 * This file is part of org.kbinani.apputil.
 *
 * org.kbinani.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.apputil;

import java.awt.*;
#else
using System;
using org.kbinani.java.awt;

namespace org.kbinani.apputil {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 折れ線を効率よく描画するための描画プラクシーです。
    /// あらかじめ指定された個数の座標データを保持しておき、必要になったらdrawPolyline[java], DrawLines[C#]でまとめて描画する仕組みになっています。
    /// </summary>
    public class PolylineDrawer {
        /// <summary>
        /// 保持する座標データの長さ
        /// </summary>
        int length = 1024;
#if JAVA
        int[] xPoints;
        int[] yPoints;
#else
        /// <summary>
        /// 座標データ
        /// </summary>
        System.Drawing.Point[] points;
#endif
        /// <summary>
        /// 次に書き込むべき、座標データ配列のインデックス
        /// </summary>
        int pos = 0;
        /// <summary>
        /// 描画に使用するグラフィクスオブジェクト
        /// </summary>
        Graphics2D g;

        /// <summary>
        /// 描画に使用するグラフィクスとバッファの長さを指定したコンストラクタ。
        /// </summary>
        /// <param name="g">描画に使用するグラフィクスオブジェクト。nullでもかまわない</param>
        /// <param name="buffer_length">座標バッファの個数(2より小さい値を指定した場合、替わりに「1024」が使用される)</param>
        public PolylineDrawer( Graphics2D g, int buffer_length ) {
            this.g = g;
            length = buffer_length;
            if ( length < 2 ) {
                length = 1024;
            }
#if JAVA
            xPoints = new int[length];
            yPoints = new int[length];
#else
            points = new System.Drawing.Point[length];
#endif
            pos = 0;
        }

        /// <summary>
        /// 保持している座標データをクリアします。
        /// </summary>
        public void clear() {
            pos = 0;
        }

        /// <summary>
        /// 描画に使用するグラフィクスオブジェクトを指定します。
        /// </summary>
        /// <param name="g"></param>
        public void setGraphics( Graphics2D g ) {
            this.g = g;
        }

        /// <summary>
        /// 描画に使用するグラフィクスオブジェクトを取得します。
        /// </summary>
        /// <returns></returns>
        public Graphics2D getGraphics() {
            return this.g;
        }

        /// <summary>
        /// 座標データを追加します。描画が必要になった場合は、描画も行われます。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void append( int x, int y ) {
#if JAVA
            xPoints[pos] = x;
            yPoints[pos] = y;
#else
            points[pos].X = x;
            points[pos].Y = y;
#endif
            pos++;
            if ( length <= pos ) {
                flush();
            }
        }

        /// <summary>
        /// 現在保持している座標データを、強制的に描画します。
        /// </summary>
        public void flush() {
            if ( pos <= 1 ){
                return;
            }
#if JAVA
            int lastx = xPoints[pos - 1];
            int lasty = yPoints[pos - 1];
            g.drawPolyline( xPoints, yPoints, pos );
            xPoints[0] = lastx;
            yPoints[0] = lasty;
            pos = 1;
#else
            int lastx = points[pos - 1].X;
            int lasty = points[pos - 1].Y;
            if ( pos != length ) {
                for ( int i = pos; i < length; i++ ) {
                    points[i].X = lastx;
                    points[i].Y = lasty;
                }
            }
            g.nativeGraphics.DrawLines( g.stroke.pen, points );
            points[0].X = lastx;
            points[0].Y = lasty;
            pos = 1;
#endif
        }
    }

#if !JAVA
}
#endif
