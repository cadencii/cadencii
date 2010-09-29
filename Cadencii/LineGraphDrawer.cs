/*
 * LineGraphDrawer.cs
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

import java.awt.*;
#else
using System;
using org.kbinani.java.awt;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 折れ線グラフを効率よく描く描画プラクシー。データ点描画の有無、グラフの塗りつぶしの有無を選べる
    /// </summary>
    public class LineGraphDrawer {
        /// <summary>
        /// 描画する際、ステップ状のグラフを描きます
        /// </summary>
        public const int TYPE_STEP = 0;
        /// <summary>
        /// 描画する際、データ点を単に結んだだけのグラフを描きます
        /// </summary>
        public const int TYPE_LINEAR = 1;
        /// <summary>
        /// データ点を四角で描きます
        /// </summary>
        public const int DOT_RECT = 0;
        /// <summary>
        /// データ点を丸で描きます
        /// </summary>
        public const int DOT_CIRCLE = 1;

        private const int BUFLEN = 512;
        
        /// <summary>
        /// X軸の描画位置
        /// </summary>
        private int mBaseLineY;
        /// <summary>
        /// データ点のバッファ
        /// </summary>
#if JAVA
        private Point[] mPoints;
#else
        private System.Drawing.Point[] mPoints;
#endif
        /// <summary>
        /// 
        /// </summary>
        private int mMaxPoints;
        private int mGraphType;
        private boolean mFirst = true;
        private int mFirstX;
#if JAVA
        private Graphics2D mGraphics;
#else
        private System.Drawing.Graphics mGraphics;
#endif
        /// <summary>
        /// グラフの線とX軸との間隙を塗りつぶすかどうか
        /// </summary>
        private boolean mFill = true;
        /// <summary>
        /// グラフの線とX軸との間隙の塗りつぶしに使用する色
        /// </summary>
        private Color mFillColor;
#if !JAVA
        private System.Drawing.SolidBrush mFillBrush = null;
#endif
        /// <summary>
        /// データ点を描画するかどうか
        /// </summary>
        private boolean mDot = true;
        /// <summary>
        /// データ点の描画サイズ。
        /// mDotTypeがDOT_CIRCLEの場合は半径、DOT_RECTの場合は一辺の長さの半分の値を指定します
        /// </summary>
        private int mDotSize = 2;
        /// <summary>
        /// データ点の描画タイプ。
        /// </summary>
        private int mDotType;
        /// <summary>
        /// データ点の描画色
        /// </summary>
        private Color mDotColor;
#if !JAVA
        private System.Drawing.SolidBrush mDotBrush = null;
#endif
        /// <summary>
        /// 線の描画色
        /// </summary>
        private Color mLineColor;
#if !JAVA
        private System.Drawing.Pen mLinePen = null;
#endif
        private int mIndex;
        private int mLastX;
        private int mLastY;

        /// <summary>
        /// コンストラクタ。グラフのタイプを指定します
        /// </summary>
        /// <param name="graph_type">グラフのタイプを指定する整数値。<see cref="TYPE_LINEAR"/>または<see cref="TYPE_STEP"/>を指定する</param>
        public LineGraphDrawer( int graph_type ) {
            // データ点のバッファを確保
#if JAVA
            mPoints = new Point[BUFLEN];
#else
            mPoints = new System.Drawing.Point[BUFLEN];
#endif

            // グラフのタイプを特定
            if ( graph_type == TYPE_LINEAR ) {
                mGraphType = TYPE_LINEAR;
            } else if ( graph_type == TYPE_STEP ) {
                mGraphType = TYPE_STEP;
            } else {
                mGraphType = TYPE_LINEAR;
            }

            // 自動flushを起こすときのデータ点の個数を設定
            if ( mGraphType == TYPE_LINEAR ) {
                mMaxPoints = BUFLEN - 2;
            } else {
                mMaxPoints = BUFLEN / 2 - 1;
            }

            mFirst = true;
        }

        #region public methods
        /// <summary>
        /// グラフの線の描画色を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setLineColor( Color value ) {
            mLineColor = value;
        }

        /// <summary>
        /// グラフの線の描画色を取得します
        /// </summary>
        /// <returns></returns>
        public Color getLineColor() {
            return mLineColor;
        }

        /// <summary>
        /// データ点の描画色を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setDotColor( Color value ) {
            mDotColor = value;
        }

        /// <summary>
        /// データ点の描画色を取得します
        /// </summary>
        /// <returns></returns>
        public Color getDotColor() {
            return mDotColor;
        }

        /// <summary>
        /// グラフの線とX軸の間を塗りつぶすかどうかを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setFill( boolean value ) {
            mFill = value;
        }

        /// <summary>
        /// グラフの線とX軸の間を塗りつぶすかどうかを取得します
        /// </summary>
        /// <returns></returns>
        public boolean isFill() {
            return mFill;
        }

        /// <summary>
        /// グラフの線とX軸の間を塗りつぶす場合の色を設定します
        /// </summary>
        /// <param name="c"></param>
        public void setFillColor( Color c ) {
            mFillColor = c;
        }

        /// <summary>
        /// グラフの線とX軸の間を塗りつぶす場合の色を取得します
        /// </summary>
        /// <returns></returns>
        public Color getFillColor() {
            return mFillColor;
        }

        /// <summary>
        /// データ点の描画サイズを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setDotSize( int value ) {
            mDotSize = value;
            if ( mDotSize <= 0 ) {
                mDot = false;
                mDotSize = 0;
            }
        }

        /// <summary>
        /// データ点の描画サイズを取得します
        /// </summary>
        /// <returns></returns>
        public int getDotSize() {
            return mDotSize;
        }

        /// <summary>
        /// データ点の描画タイプを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setDotType( int value ) {
            if ( value == DOT_CIRCLE ) {
                mDotType = DOT_CIRCLE;
            } else if ( value == DOT_RECT ) {
                mDotType = DOT_RECT;
            } else {
                mDotType = DOT_RECT;
            }
        }

        /// <summary>
        /// データ点の描画タイプを取得します
        /// </summary>
        /// <returns></returns>
        public int getDotType() {
            return mDotType;
        }

        /// <summary>
        /// データ点を描画するかどうかを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setDrawDot( boolean value ) {
            mDot = value;
        }

        /// <summary>
        /// データ点を描画するかどうかを取得します
        /// </summary>
        /// <returns></returns>
        public boolean isDrawDot() {
            return mDot;
        }

        /// <summary>
        /// データ点を追加します。必要があれば、flushメソッドが自動で呼ばれます
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void append( int x, int y ) {
            if ( mGraphType == TYPE_LINEAR ) {
                // 直線で結ぶ場合
                setPointData( mIndex, x, y );
                mIndex++;
                if ( mIndex >= mMaxPoints ) {
                    flush();
                }
            } else if ( mGraphType == TYPE_STEP ) {
                // ステップ状に結ぶ場合
                //TODO: ここがまだだだ
            }
            mLastX = x;
            mLastY = y;
        }

        /// <summary>
        /// 現在のデータ点のバッファの描画を行います
        /// </summary>
        public void flush() {
            if ( mGraphType == TYPE_LINEAR ) {
                setPointData( mIndex, x, mBaseLineY );
                setPointData( mIndex + 1, mFirstX, mBaseLineY );

                // 塗りつぶし
                if ( mFill ) {
#if JAVA
                    mGraphics.setColor( mFillColor );
                    mGraphics.fillPolygone FOOOOOOOOOOOOOOOOOO!!!!!!!!!!!!!!!
#else
                    mGraphics.FillPolygon( getFillBrush(), mPoints );
#endif
                }

                // 線を描く
                setPointData( mIndex, x, y );
                setPointData( mIndex + 1, x, y );
#if JAVA
                mGraphics.setColor( mLineColor );
                mGraphics.drawPolygone ...
#else
                mGraphics.DrawLines( getLinePen(), mPoints );
#endif

                // ドットを描く
                // ここでは第mIndex - 1番目のドットまでを描いて、mIndex - 1番目のデータは第0番目にコピーし，次のflushで描く
#if JAVA
                mGraphics.setColor( mDotColor );
#endif
                for ( int i = 0; i < mIndex - 1; i++ ) {
#if JAVA
                    Point p = mPoints[i];
#else
                    System.Drawing.Point p = mPoints[i];
#endif
                    if ( mDotType == DOT_CIRCLE ) {
#if JAVA
                        mGraphics.fillEllipse( p.x - mDotSize, p.y - mDotSize, mDotSize * 2, mDotSize * 2 );
#else
                        mGraphics.FillEllipse( getDotBrush(), p.X - mDotSize, p.Y - mDotSize, mDotSize * 2, mDotSize * 2 );
#endif
                    } else if ( mDotType == DOT_RECT ) {
#if JAVA
                        mGraphics.fillRectangle( p.x - mDotSize, p.y - mDotSize, mDotSize * 2, mDotSize * 2 );
#else
                        mGraphics.FillRectangle( getDotBrush(), p.X - mDotSize, p.Y - mDotSize, mDotSize * 2, mDotSize * 2 );
#endif
                    }
                }

                // 次の描画に備える
                setPointData( 0, mLastX, mLastY );
                mFirstX = mLastX;
                mIndex = 1;
            } else if ( mGraphType == TYPE_STEP ) {
                //TODO: この辺がまだ
            }
        }

        /// <summary>
        /// データ点のバッファをクリアします
        /// </summary>
        public void clear() {
            //TODO: ここもまだだ
        }

        /// <summary>
        /// 描画に使用するグラフィックスを指定します
        /// </summary>
        /// <param name="g"></param>
        public void setGraphics( Graphics2D g ){
#if JAVA
            mGraphics = g;
#else
            mGraphics = g.nativeGraphics;
#endif
        }

        /// <summary>
        /// X軸の描画位置を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setBaseLineY( int value ) {
            mBaseLineY = value;
        }

        /// <summary>
        /// X軸の描画位置を取得します
        /// </summary>
        /// <returns></returns>
        public int getBaseLineY() {
            return mBaseLineY;
        }
        #endregion

        #region private methods
#if !JAVA
        private System.Drawing.SolidBrush getFillBrush() {
            if ( mFillBrush == null ) {
                mFillBrush = new System.Drawing.SolidBrush( mFillColor.color );
            }
            mFillBrush.Color = mFillColor.color;
            return mFillBrush;
        }

        private System.Drawing.SolidBrush getDotBrush() {
            if ( mDotBrush == null ) {
                mDotBrush = new System.Drawing.SolidBrush( mDotColor.color );
            }
            mDotBrush.Color = mDotColor.color;
            return mDotBrush;
        }

        private System.Drawing.Pen getLinePen() {
            if ( mLinePen == null ) {
                mLinePen = new System.Drawing.Pen( mLineColor.color );
            }
            return mLinePen;
        }
#endif

        private void setPointData( int index, int x, int y ) {
#if JAVA
            Point p = mPoints[index];
            p.x = x;
            p.y = y;
#else
            System.Drawing.Point p = mPoints[index];
            p.X = x;
            p.Y = y;
#endif
            mPoints[index] = p;
        }
        #endregion

    }

#if !JAVA
}
#endif
