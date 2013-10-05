/*
 * MouseTracer.cs
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
using System.Collections.Generic;
using cadencii.java.awt;
using cadencii.java.util;

namespace cadencii
{

    /// <summary>
    /// コントロールカーブの編集時などに，マウスの軌跡をトレースする処理をカプセル化？する
    /// </summary>
    public class MouseTracer
    {
        class MouseTracerIterator : IEnumerable<Point>, IEnumerator<Point>
        {
            private MouseTracer mTracer;
            private int mIndex;

            public MouseTracerIterator(MouseTracer tracer)
            {
                mTracer = tracer;
                Reset();
            }

            public IEnumerator<Point> GetEnumerator() { return this; }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return this; }

            public Point Current
            {
                get
                {
                    int x = mIndex + mTracer.mXAt0;
                    int y = mTracer.mTrace[mIndex];
                    return new Point(x, y);
                }
            }

            object System.Collections.IEnumerator.Current { get { return Current; } }

            public void Reset()
            {
                mIndex = -1;
            }

            public bool MoveNext()
            {
                ++mIndex;
                return mIndex < mTracer.mSize;
            }

            public void Dispose() { }
        }

        /// <summary>
        /// マウスのトレース。配列の添え字が1進むと、1ピクセル右側
        /// </summary>
        private int[] mTrace = null;
        /// <summary>
        /// mTrace[0]が表してるx座標
        /// </summary>
        private int mXAt0;
        /// <summary>
        /// mTrace[m_size - 1]までが有効だということを表す
        /// </summary>
        private int mSize = 0;
        /// <summary>
        /// マウスのトレース時、前回リストに追加されたx座標の値
        /// </summary>
        private int mMouseTraceLastX;
        /// <summary>
        /// マウスのトレース時、前回リストに追加されたy座標の値
        /// </summary>
        private int mMouseTraceLastY;

        /// <summary>
        /// 軌跡の点を順に返す反復子を取得します．単純にデータ点を返すのではなく，x+1ごとの補間も含めた点が返される点に注意
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Point> iterator()
        {
            return new MouseTracerIterator(this);
        }

        /// <summary>
        /// 軌跡に点を追加します
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void append(int x, int y)
        {
            if (mSize <= 0) {
                appendFirst(x, y);
                return;
            }

            if (x == mMouseTraceLastX) {
                mTrace[x - mXAt0] = y;
                mMouseTraceLastY = y;
                return;
            }

            if (x < mXAt0) {
                // 一番最初に登録されている座標よりさらに左側(x小)の登録が要求された場合
                // 今もっているデータをdxずらす必要がある(必要な配列のサイズはdx増加する)
                int dx = mXAt0 - x;
                ensureLength(mSize + dx);
                mSize += dx;
                // ずらすよ
                for (int i = mSize - 1; i >= dx; i--) {
                    mTrace[i] = mTrace[i - dx];
                }
                mXAt0 = x;
            } else if (mXAt0 + mSize <= x) {
                mSize = x - mXAt0 + 1;
                ensureLength(mSize);
            }

            int d = x - mMouseTraceLastX;
            if (d == 1 || d == -1) {
                // 1個しかずれてないんだったら、傾きとか計算しなくても良いよ
                mTrace[x - mXAt0] = y;
            } else {
                // 点を登録する処理
                int startx = mMouseTraceLastX;
                int starty = mMouseTraceLastY;
                int endx = x;
                int endy = y;
                if (endx < startx) {
                    int b = endx;
                    endx = startx;
                    startx = b;
                    b = endy;
                    endy = starty;
                    starty = b;
                }

                if (endy == starty) {
                    // yが変化していないなら，傾きを計算しなくてもいい
                    for (int px = startx; px <= endx; px++) {
                        mTrace[px - mXAt0] = starty;
                    }
                } else {
                    // 傾き
                    double a = (endy - starty) / (double)(endx - startx);
                    // 1pxづつ計算
                    for (int px = startx; px <= endx; px++) {
                        int i = px - mXAt0;
                        int v = (int)(starty + a * (px - startx));
                        mTrace[i] = v;
                    }
                }
            }

            mMouseTraceLastX = x;
            mMouseTraceLastY = y;
        }

        /// <summary>
        /// 現在保持されているデータの個数を取得します
        /// </summary>
        /// <returns></returns>
        public int size()
        {
            return mSize;
        }

        /// <summary>
        /// 現在保持されている軌跡を破棄します
        /// </summary>
        public void clear()
        {
            mSize = 0;
        }

        /// <summary>
        /// 現在保持している軌跡を破棄し，新しい点を追加します．
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void appendFirst(int x, int y)
        {
            ensureLength(1);
            mSize = 1;
            mTrace[0] = y;
            mXAt0 = x;
            mMouseTraceLastX = x;
            mMouseTraceLastY = y;
        }

        /// <summary>
        /// 現在登録されている軌跡の左端のX座標を調べます
        /// </summary>
        /// <returns></returns>
        public int firstKey()
        {
            return mXAt0;
        }

        /// <summary>
        /// 現在登録されている軌跡の右端のX座標を調べます
        /// </summary>
        /// <returns></returns>
        public int lastKey()
        {
            return mXAt0 + mSize - 1;
        }

        /// <summary>
        /// mTraceの長さが指定された長さ以上に変更します
        /// </summary>
        /// <param name="new_length"></param>
        private void ensureLength(int new_length)
        {
            if (new_length <= 0) {
                return;
            }
            if (mTrace == null) {
                mTrace = new int[new_length];
            } else {
                if (mTrace.Length < new_length) {
                    Array.Resize(ref mTrace, new_length);
                }
            }
        }
    }

}
