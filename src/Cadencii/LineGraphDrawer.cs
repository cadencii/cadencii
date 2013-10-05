/*
 * LineGraphDrawer.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Drawing;

namespace cadencii
{

    /// <summary>
    /// 折れ線グラフを効率よく描く描画プラクシー。データ点描画の有無、グラフの塗りつぶしの有無を選べる
    /// </summary>
    public class LineGraphDrawer
    {
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
        /// <summary>
        /// データ点を描かないモード
        /// </summary>
        public const int DOTMODE_NO = 0;
        /// <summary>
        /// データ点を常に描くモード
        /// </summary>
        public const int DOTMODE_ALWAYS = 1;
        /// <summary>
        /// データ点がマウスと近い場合にのみ描くモード
        /// </summary>
        public const int DOTMODE_NEAR = 2;

        private const int BUFLEN = 1024;
        /// <summary>
        /// マウスに「近い」と判定する距離（ピクセル単位）
        /// </summary>
        private const float NEAR_THRESHOLD = 200f;
        /// <summary>
        /// <see cref="NEAR_THRESHOLD"/>の逆数
        /// </summary>
        private const float INV_NEAR_THRESHOLD = 1f / NEAR_THRESHOLD;

        /// <summary>
        /// X軸の描画位置
        /// </summary>
        private int mBaseLineY;
        /// <summary>
        /// データ点のバッファ
        /// </summary>
        private System.Drawing.Point[] mPoints;
        /// <summary>
        /// 自動flushを行う時のmIndexの値。
        /// mIndex + 1 >= mMaxPointsの時、自動でflushが行われる
        /// </summary>
        private int mMaxPoints;
        /// <summary>
        /// グラフのタイプ。<see cref="TYPE_LINEAR"/>または<see cref="TYPE_CIRCLE"/>のどちらか
        /// </summary>
        private int mGraphType;
        /// <summary>
        /// flushの後、一度もappendされていない時にtrue。
        /// </summary>
        private bool mFirst = true;
        /// <summary>
        /// flushの後、最初にappendされてきたデータ点のx座標の値
        /// </summary>
        private int mFirstX;
        /// <summary>
        /// 描画に使用するグラフィックス
        /// </summary>
        private System.Drawing.Graphics mGraphics;
        /// <summary>
        /// グラフの線とX軸との間隙を塗りつぶすかどうか
        /// </summary>
        private bool mFill = true;
        /// <summary>
        /// グラフの線とX軸との間隙の塗りつぶしに使用する色
        /// </summary>
        private java.awt.Color mFillColor = new java.awt.Color(255, 0, 0);
        /// <summary>
        /// グラフの線とX軸との間隙の塗りつぶしに使用するブラシ
        /// </summary>
        private System.Drawing.SolidBrush mFillBrush = null;
        /// <summary>
        /// データ点を描画するかどうか
        /// </summary>
        private int mDot = 1;
        /// <summary>
        /// データ点の描画サイズ。
        /// mDotTypeがDOT_CIRCLEの場合は半径、DOT_RECTの場合は一辺の長さの半分の値を指定します
        /// </summary>
        private int mDotSize = 2;
        /// <summary>
        /// データ点の描画サイズ。mDotSizeに従属で、mDotSize変更時に自動的に計算される
        /// </summary>
        private int mDotWidth = 2 * 2 + 1;
        /// <summary>
        /// データ点の描画タイプ。
        /// </summary>
        private int mDotType;
        /// <summary>
        /// データ点の描画色
        /// </summary>
        private java.awt.Color mDotColor = new java.awt.Color(0, 0, 0);
        /// <summary>
        /// データ点の塗りつぶしに使用するブラシ
        /// </summary>
        private System.Drawing.SolidBrush mDotBrush = null;
        /// <summary>
        /// 線の描画色
        /// </summary>
        private java.awt.Color mLineColor = new java.awt.Color(0, 0, 0);
        /// <summary>
        /// 線を描画するかどうか
        /// </summary>
        private bool mLine = true;
        /// <summary>
        /// 線の描画幅
        /// </summary>
        private int mLineWidth = 1;
        /// <summary>
        /// 線の描画に使用するペン
        /// </summary>
        private System.Drawing.Pen mLinePen = null;
        /// <summary>
        /// 次のappendでデータ点の座標を代入するmPointsのインデックス
        /// </summary>
        private int mIndex;
        /// <summary>
        /// 前回appendされてきたデータ点のX座標
        /// </summary>
        private int mLastX;
        /// <summary>
        /// 前回appendされてきたデータ点のY座標
        /// </summary>
        private int mLastY;
        /// <summary>
        /// 現在のマウスのX座標
        /// </summary>
        private int mMouseX;

        /// <summary>
        /// コンストラクタ。グラフのタイプを指定します
        /// </summary>
        /// <param name="graph_type">グラフのタイプを指定する整数値。<see cref="TYPE_LINEAR"/>または<see cref="TYPE_STEP"/>を指定する</param>
        public LineGraphDrawer(int graph_type)
        {
            // データ点のバッファを確保
            mPoints = new Point[BUFLEN];

            // グラフのタイプを特定
            if (graph_type == TYPE_LINEAR) {
                mGraphType = TYPE_LINEAR;
            } else if (graph_type == TYPE_STEP) {
                mGraphType = TYPE_STEP;
            } else {
                mGraphType = TYPE_LINEAR;
            }

            // 自動flushを起こすときのデータ点の個数を設定
            if (mGraphType == TYPE_LINEAR) {
                mMaxPoints = BUFLEN - 1;
            } else if (mGraphType == TYPE_STEP) {
                mMaxPoints = BUFLEN;
            }

#if DEBUG
            sout.println("LineGraphDrawer#mMaxPoints=" + mMaxPoints);
#endif
            mFirst = true;
        }

        #region public methods
        /// <summary>
        /// グラフの線の幅を設定します．単位はピクセルです
        /// </summary>
        /// <param name="value"></param>
        public void setLineWidth(int value)
        {
            if (value <= 0) {
                value = 1;
            }
            mLineWidth = value;
        }

        /// <summary>
        /// グラフの線の幅を取得します．単位はピクセルです
        /// </summary>
        public int getLineWidth()
        {
            return mLineWidth;
        }

        /// <summary>
        /// マウスのX座標を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setMouseX(int value)
        {
            mMouseX = value;
        }

        /// <summary>
        /// グラフの線の描画色を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setLineColor(java.awt.Color value)
        {
            mLineColor = value;
        }

        /// <summary>
        /// グラフの線の描画色を取得します
        /// </summary>
        /// <returns></returns>
        public java.awt.Color getLineColor()
        {
            return mLineColor;
        }

        /// <summary>
        /// 線を描画するかどうかを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setDrawLine(bool value)
        {
            mLine = value;
        }

        /// <summary>
        /// 線を描画するかどうかを取得します
        /// </summary>
        /// <returns></returns>
        public bool isDrawLine()
        {
            return mLine;
        }

        /// <summary>
        /// データ点の描画色を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setDotColor(java.awt.Color value)
        {
            mDotColor = value;
        }

        /// <summary>
        /// データ点の描画色を取得します
        /// </summary>
        /// <returns></returns>
        public java.awt.Color getDotColor()
        {
            return mDotColor;
        }

        /// <summary>
        /// グラフの線とX軸の間を塗りつぶすかどうかを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setFill(bool value)
        {
            mFill = value;
        }

        /// <summary>
        /// グラフの線とX軸の間を塗りつぶすかどうかを取得します
        /// </summary>
        /// <returns></returns>
        public bool isFill()
        {
            return mFill;
        }

        /// <summary>
        /// グラフの線とX軸の間を塗りつぶす場合の色を設定します
        /// </summary>
        /// <param name="c"></param>
        public void setFillColor(java.awt.Color c)
        {
            mFillColor = c;
        }

        /// <summary>
        /// グラフの線とX軸の間を塗りつぶす場合の色を取得します
        /// </summary>
        /// <returns></returns>
        public java.awt.Color getFillColor()
        {
            return mFillColor;
        }

        /// <summary>
        /// データ点の描画サイズを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setDotSize(int value)
        {
            mDotSize = value;
            if (mDotSize <= 0) {
                mDot = DOTMODE_ALWAYS;
                mDotSize = 0;
            }
            mDotWidth = 2 * mDotSize + 1;
        }

        /// <summary>
        /// データ点の描画サイズを取得します
        /// </summary>
        /// <returns></returns>
        public int getDotSize()
        {
            return mDotSize;
        }

        /// <summary>
        /// データ点の描画タイプを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setDotType(int value)
        {
            if (value == DOT_CIRCLE) {
                mDotType = DOT_CIRCLE;
            } else if (value == DOT_RECT) {
                mDotType = DOT_RECT;
            } else {
                mDotType = DOT_RECT;
            }
        }

        /// <summary>
        /// データ点の描画タイプを取得します
        /// </summary>
        /// <returns></returns>
        public int getDotType()
        {
            return mDotType;
        }

        /// <summary>
        /// データ点の描画モードを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setDotMode(int value)
        {
            if (value == DOTMODE_ALWAYS) {
                mDot = DOTMODE_ALWAYS;
            } else if (value == DOTMODE_NEAR) {
                mDot = DOTMODE_NEAR;
            } else if (value == DOTMODE_NO) {
                mDot = DOTMODE_NO;
            }
        }

        /// <summary>
        /// データ点の描画モードを取得します
        /// </summary>
        /// <returns></returns>
        public int getDotMode()
        {
            return mDot;
        }

        /// <summary>
        /// データ点を追加します。必要があれば、flushメソッドが自動で呼ばれます
        /// </summary>
        /// <param name="x">データ点のX座標</param>
        /// <param name="y">データ点のY座標</param>
        public void append(int x, int y)
        {
            if (mGraphType == TYPE_LINEAR) {
                // 直線で結ぶ場合
                setPointData(mIndex, x, y);
                mIndex++;
            } else if (mGraphType == TYPE_STEP) {
                // ステップ状に結ぶ場合
                if (mFirst) {
                    mFirst = false;
                    mFirstX = x;
                } else {
                    setPointData(mIndex, x, mLastY);
                    mIndex++;
                }
                setPointData(mIndex, x, y);
                mIndex++;
            }
            mLastX = x;
            mLastY = y;
            if (mIndex + 1 >= mMaxPoints) {
                flush();
            }
        }

        /// <summary>
        /// 現在のデータ点のバッファの描画を行います
        /// </summary>
        public void flush()
        {
            if (mIndex < 2) {
                // データ点が少ないのでflushする意味なし
                return;
            }
            if (mGraphType == TYPE_LINEAR) {
                // 塗りつぶし
                if (mFill) {
                    setPointData(mIndex, mLastX, mBaseLineY);
                    setPointData(mIndex + 1, mFirstX, mBaseLineY);

                    mGraphics.FillPolygon(getFillBrush(), mPoints);
                }

                // 線を描く
                if (mLine) {
                    for (int i = mIndex - 1; i < BUFLEN; i++) {
                        setPointData(i, mLastX, mLastY);
                    }
                    mGraphics.DrawLines(getLinePen(), mPoints);
                }

                // ドットを描く
                if (mDot != DOTMODE_NO) {
                    // ここでは第mIndex - 1番目のドットまでを描いて、mIndex - 1番目のデータは第0番目にコピーし，次のflushで描く
                    for (int i = 0; i < mIndex - 1; i++) {
                        System.Drawing.Point p = mPoints[i];
                        if (mDotType == DOT_CIRCLE) {
                            mGraphics.FillEllipse(getDotBrush(), p.X - mDotSize, p.Y - mDotSize, mDotWidth, mDotWidth);
                        } else if (mDotType == DOT_RECT) {
                            mGraphics.FillRectangle(getDotBrush(), p.X - mDotSize, p.Y - mDotSize, mDotWidth, mDotWidth);
                        }
                    }
                }

                // 次の描画に備える
                setPointData(0, mLastX, mLastY);
                mFirstX = mLastX;
                mIndex = 1;
            } else if (mGraphType == TYPE_STEP) {
                if (mFill) {
                    // 塗りつぶし用の枠を追加
                    setPointData(mIndex - 1, mLastX, mBaseLineY);
                    setPointData(mIndex, mFirstX, mBaseLineY);
                    for (int i = mIndex + 1; i < BUFLEN; i++) {
                        setPointData(i, mFirstX, mBaseLineY);
                    }

                    // 塗りつぶし
                    mGraphics.FillPolygon(getFillBrush(), mPoints);
                }

                if (mLine) {
                    // 線を描く
                    for (int i = mIndex - 1; i < BUFLEN; i++) {
                        setPointData(i, mLastX, mLastY);
                    }
                    mGraphics.DrawLines(getLinePen(), mPoints);
                }

                if (mDot != DOTMODE_NO) {
                    // データ点を描く
                    SolidBrush brs = getDotBrush();
                    Color c = brs.Color;
                    for (int i = 0; i < mIndex; i += 2) {
                        Point p = mPoints[i];
                        int px = p.X;
                        int alpha = (mDot == DOTMODE_NEAR) ? getAlpha(px) : 255;
                        if (alpha <= 0) {
                            continue;
                        }
                        brs.Color = Color.FromArgb(alpha, c);
                        if (mDotType == DOT_CIRCLE) {
                            mGraphics.FillEllipse(brs, p.X - mDotSize, p.Y - mDotSize, mDotWidth, mDotWidth);
                        } else {
                            mGraphics.FillRectangle(brs, p.X - mDotSize, p.Y - mDotSize, mDotWidth, mDotWidth);
                        }
                    }
                    brs.Color = Color.FromArgb(255, c);
                }

                // 次の描画に備える
                setPointData(0, mLastX, mLastY);
                mFirstX = mLastX;
                mIndex = 1;
            }
        }

        /// <summary>
        /// データ点のバッファをクリアします
        /// </summary>
        public void clear()
        {
            mFirst = true;
            mIndex = 0;
        }

        public void setGraphics(System.Drawing.Graphics g)
        {
            mGraphics = g;
        }

        /// <summary>
        /// 描画に使用するグラフィックスを指定します
        /// </summary>
        /// <param name="g"></param>
        public void setGraphics(java.awt.Graphics g)
        {
            mGraphics = g.nativeGraphics;
        }

        /// <summary>
        /// X軸の描画位置を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setBaseLineY(int value)
        {
            mBaseLineY = value;
        }

        /// <summary>
        /// X軸の描画位置を取得します
        /// </summary>
        /// <returns></returns>
        public int getBaseLineY()
        {
            return mBaseLineY;
        }
        #endregion

        #region private methods
        private int getAlpha(int x)
        {
            double dx = Math.Abs(x - mMouseX) * INV_NEAR_THRESHOLD;
            if (dx > NEAR_THRESHOLD) {
                return 0;
            }
            //double sigma = 0.3;
            //int alpha = (int)(255.0 * Math.Exp( -(x * x) / (2.0 * sigma * sigma) ));
            //↑の近似式
            int ret = (int)((((-1293.6 * dx + 3022.5) * dx - 2020.4) * dx + 34.77) * dx + 255.0);
            if (ret < 0) {
                ret = 0;
            } else if (255 < ret) {
                ret = 255;
            }
            return ret;
        }

        private SolidBrush getFillBrush()
        {
            if (mFillBrush == null) {
                mFillBrush = new SolidBrush(mFillColor.color);
            }
            mFillBrush.Color = mFillColor.color;
            return mFillBrush;
        }

        private SolidBrush getDotBrush()
        {
            if (mDotBrush == null) {
                mDotBrush = new SolidBrush(mDotColor.color);
            }
            mDotBrush.Color = mDotColor.color;
            return mDotBrush;
        }

        private Pen getLinePen()
        {
            if (mLinePen == null) {
                mLinePen = new Pen(mLineColor.color);
            }
            mLinePen.Color = mLineColor.color;
            mLinePen.Width = mLineWidth;
            return mLinePen;
        }

        private void setPointData(int index, int x, int y)
        {
            mPoints[index].X = x;
            mPoints[index].Y = y;
        }
        #endregion

    }

}
