/*
 * MouseTracer.cs
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
import java.util.*;
#else
using System;
using org.kbinani.java.awt;
using org.kbinani.java.util;

namespace org.kbinani.cadencii {
    using Integer = System.Int32;
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// コントロールカーブの編集時などに，マウスの軌跡をトレースする処理をカプセル化？する
    /// </summary>
    public class MouseTracer {
#if JAVA
        class MouseTracerIterator implements Iterator<Point> {
#else
        class MouseTracerIterator : Iterator<Point> {
#endif
            private MouseTracer m_tracer;
            private int m_index;

            public MouseTracerIterator( MouseTracer tracer ) {
                m_tracer = tracer;
                m_index = -1;
            }

            public boolean hasNext() {
                return m_index + 1 < m_tracer.m_size;
            }

            public Point next() {
                m_index++;
                int x = m_index + m_tracer.m_x_at0;
                int y = m_tracer.m_trace[m_index];
                return new Point( x, y );
            }

            public void remove() {
                // do nothing
            }
        }

        /// <summary>
        /// マウスのトレース。配列の添え字が1進むと、1ピクセル右側
        /// </summary>
        private int[] m_trace = null;
        /// <summary>
        /// m_trace[0]が表してるx座標
        /// </summary>
        private int m_x_at0;
        /// <summary>
        /// m_trace[m_size - 1]までが有効だということを表す
        /// </summary>
        private int m_size = 0;
        /// <summary>
        /// マウスのトレース時、前回リストに追加されたx座標の値
        /// </summary>
        private int m_mouse_trace_last_x;
        /// <summary>
        /// マウスのトレース時、前回リストに追加されたy座標の値
        /// </summary>
        private int m_mouse_trace_last_y;

        /// <summary>
        /// 軌跡の点を順に返す反復子を取得します．単純にデータ点を返すのではなく，x+1ごとの補間も含めた点が返される点に注意
        /// </summary>
        /// <returns></returns>
        public Iterator<Point> iterator() {
            return new MouseTracerIterator( this );
        }

        /// <summary>
        /// 軌跡に点を追加します
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void append( int x, int y ) {
            if ( m_size <= 0 ) {
                appendFirst( x, y );
                return;
            }

            if( x == m_mouse_trace_last_x ){
                m_trace[x - m_x_at0] = y;
                m_mouse_trace_last_y = y;
                return;
            }

            if ( x < m_x_at0 ) {
                // 一番最初に登録されている座標よりさらに左側(x小)の登録が要求された場合
                // 今もっているデータをdxずらす必要がある(必要な配列のサイズはdx増加する)
                int dx = m_x_at0 - x;
                ensureLength( m_size + dx );
                m_size += dx;
                // ずらすよ
                for ( int i = m_size - 1; i >= dx; i-- ) {
                    m_trace[i] = m_trace[i - dx];
                }
                m_x_at0 = x;
            } else if ( m_x_at0 + m_size <= x ) {
                m_size = x - m_x_at0 + 1;
                ensureLength( m_size );
            }

            int d = x - m_mouse_trace_last_x;
            if ( d == 1 || d == -1 ) {
                // 1個しかずれてないんだったら、傾きとか計算しなくても良いよ
                m_trace[x - m_x_at0] = y;
            } else {
                // 点を登録する処理
                int startx = m_mouse_trace_last_x;
                int starty = m_mouse_trace_last_y;
                int endx = x;
                int endy = y;
                if ( endx < startx ) {
                    int b = endx;
                    endx = startx;
                    startx = b;
                    b = endy;
                    endy = starty;
                    starty = b;
                }

                if ( endy == starty ) {
                    // yが変化していないなら，傾きを計算しなくてもいい
                    for ( int px = startx; px <= endx; px++ ) {
                        m_trace[px - m_x_at0] = starty;
                    }
                } else {
                    // 傾き
                    double a = (endy - starty) / (double)(endx - startx);
                    // 1pxづつ計算
                    for ( int px = startx; px <= endx; px++ ) {
                        int i = px - m_x_at0;
                        int v = (int)(starty + a * (px - startx));
                        m_trace[i] = v;
                    }
                }
            }

            m_mouse_trace_last_x = x;
            m_mouse_trace_last_y = y;
        }

        /// <summary>
        /// 現在保持されているデータの個数を取得します
        /// </summary>
        /// <returns></returns>
        public int size() {
            return m_size;
        }

        /// <summary>
        /// 現在保持されている軌跡を破棄します
        /// </summary>
        public void clear() {
            m_size = 0;
        }

        /// <summary>
        /// 現在保持している軌跡を破棄し，新しい点を追加します．
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void appendFirst( int x, int y ) {
            ensureLength( 1 );
            m_size = 1;
            m_trace[0] = y;
            m_x_at0 = x;
            m_mouse_trace_last_x = x;
            m_mouse_trace_last_y = y;
        }

        /// <summary>
        /// 現在登録されている軌跡の左端のX座標を調べます
        /// </summary>
        /// <returns></returns>
        public int firstKey() {
            return m_x_at0;
        }

        /// <summary>
        /// 現在登録されている軌跡の右端のX座標を調べます
        /// </summary>
        /// <returns></returns>
        public int lastKey() {
            return m_x_at0 + m_size - 1;
        }

        /// <summary>
        /// m_traceの長さが指定された長さ以上に変更します
        /// </summary>
        /// <param name="new_length"></param>
        private void ensureLength( int new_length ) {
            if ( new_length <= 0 ) {
                return;
            }
            if ( m_trace == null ) {
                m_trace = new int[new_length];
            } else {
                if ( m_trace.Length < new_length ) {
#if JAVA
                    int[] newarray = new int[new_length];
                    System.arraycopy( m_trace, 0, newarray, 0, m_trace.length );
                    m_trace = null;
                    m_trace = newarray;
#else
                    Array.Resize( ref m_trace, new_length );
#endif
                }
            }
        }
    }

#if !JAVA
}
#endif
