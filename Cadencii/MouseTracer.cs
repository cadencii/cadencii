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

    public class MouseTracer {
        class MouseTracerIterator : Iterator<Point>{
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
                throw new NotSupportedException( "MouseTracer#ArrayIterator<T>#remove(void)" );
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

            if ( Math.Abs( x - m_mouse_trace_last_x ) == 1 ) {
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

                // 傾き
                double a = (endy - starty) / (double)(endx - startx);
                // 1pxづつ計算
                for ( int px = startx; px <= endx; px++ ) {
                    int i = px - m_x_at0;
                    int v = (int)(starty + a * (px - startx));
                    m_trace[i] = v;
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

        public int firstKey() {
            if ( m_size <= 0 ) {
                throw new System.Collections.Generic.KeyNotFoundException( "MouseTracer#firstKey(void)" );
            }
            return m_x_at0;
        }

        public int lastKey() {
            if ( m_size <= 0 ) {
                throw new System.Collections.Generic.KeyNotFoundException( "MouseTracer#lastKey(void)" );
            }
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
                    Array.Resize( ref m_trace, new_length );
                }
            }
        }
    }

#if !JAVA
}
#endif

#if !JAVA
namespace org.kbinani.cadencii.ols {
    using Integer = System.Int32;
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// コントロールカーブの編集時などに，マウスをトレースする処理をカプセル化？する
    /// </summary>
    public class MouseTracer {
        public class MouseTracerIterator : Iterator<Point> {
            private TreeMap<Integer, Integer> m_trace = null;
            private Iterator<Integer> m_key_iterator = null;
            /// <summary>
            /// 直前にnextで返したキー値
            /// </summary>
            private int m_key_now;
            /// <summary>
            /// この反復子が最後に返す予定のキー値
            /// </summary>
            private int m_end_key;
            /// <summary>
            /// トレースのデータ点個数
            /// </summary>
            private int m_size;

            /// <summary>
            /// m_key_iteratorから取得した最新のキー値
            /// </summary>
            private int m_next_key;
            /// <summary>
            /// m_key_iteratorから前回取得したキー値(m_next_keyの1個前)
            /// </summary>
            private int m_current_key;
            private double m_a;
            private int m_current_value;
            private int m_next_value;

            public MouseTracerIterator( TreeMap<Integer, Integer> trace ) {
                m_trace = trace;
                m_key_iterator = trace.keySet().iterator();
                m_size = trace.size();
                if ( m_size > 0 ) {
                    m_end_key = trace.lastKey();
                    m_key_now = trace.firstKey() - 1;
                    m_next_key = m_key_iterator.next();
                    m_next_value = m_trace.get( m_next_key );
                    m_current_key = m_next_key;
                    m_current_value = m_next_value;
                }
            }

            public boolean hasNext() {
                if ( m_size <= 0 ) {
                    return false;
                } else {
                    return m_key_now + 1 <= m_end_key;
                }
            }

            public Point next() {
                m_key_now++;
                if ( m_key_now == m_next_key ) {
                    int old_next_value = m_next_value;
                    Point ret = new Point( m_key_now, m_next_value );
                    if ( m_key_iterator.hasNext() ) {
                        m_current_key = m_next_key;
                        m_next_key = m_key_iterator.next();
                        m_next_value = m_trace.get( m_next_key );
                        m_current_value = old_next_value;
                        m_a = (m_next_value - m_current_value) / (double)(m_next_key - m_current_key);
                    } else {
                        m_key_now = m_end_key;
                    }
                    return ret;
                } else {
                    if ( m_current_key == m_key_now ) {
                        return new Point( m_key_now, m_trace.get( m_key_now ) );
                    } else {
                        int v = (int)(m_current_value + m_a * (m_key_now - m_current_key));
                        return new Point( m_key_now, v );
                    }
                }
            }

            public void remove() {
                throw new NotSupportedException( "MouseTraser.MouseTracerIterator.remove(void)" );
            }
        }

        /// <summary>
        /// マウスのトレース。
        /// </summary>
        private TreeMap<Integer, Integer> m_mouse_trace = new TreeMap<Integer, Integer>();
        /// <summary>
        /// マウスのトレース時、前回リストに追加されたx座標の値
        /// </summary>
        private int m_mouse_trace_last_x;
        private int m_mouse_trace_last_y;

        public boolean containsKey( int value ) {
            return m_mouse_trace.containsKey( value );
        }

        public int firstKey() {
            return m_mouse_trace.firstKey();
        }

        public int lastKey() {
            return m_mouse_trace.lastKey();
        }

        /// <summary>
        /// 現在保持している軌跡を破棄し，新しい点を追加します．
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void appendFirst( int x, int y ) {
            m_mouse_trace.clear();
            m_mouse_trace.put( x, y );
            m_mouse_trace_last_x = x;
            m_mouse_trace_last_y = y;
        }

        /// <summary>
        /// 軌跡に点を追加します
        /// </summary>
        /// <param name="p"></param>
        public void append( Point p ) {
            append( p.x, p.y );
        }

        /// <summary>
        /// 軌跡に点を追加します
        /// </summary>
        /// <param name="p"></param>
        public void append( int x, int y ) {
            Vector<Integer> removelist = new Vector<Integer>();
            if ( x < m_mouse_trace_last_x ) {
                for ( Iterator<Integer> itr = m_mouse_trace.keySet().iterator(); itr.hasNext(); ) {
                    int key = itr.next();
                    if ( x <= key && key < m_mouse_trace_last_x ) {
                        removelist.add( key );
                    }
                }
            } else if ( m_mouse_trace_last_x < x ) {
                for ( Iterator<Integer> itr = m_mouse_trace.keySet().iterator(); itr.hasNext(); ) {
                    int key = itr.next();
                    if ( m_mouse_trace_last_x < key && key <= x ) {
                        removelist.add( key );
                    }
                }
            }
            for ( int i = 0; i < removelist.size(); i++ ) {
                m_mouse_trace.remove( removelist.get( i ) );
            }
            if ( x == m_mouse_trace_last_x ) {
                m_mouse_trace.put( x, y );
                m_mouse_trace_last_y = y;
            } else {
                float a = (y - m_mouse_trace_last_y) / (float)(x - m_mouse_trace_last_x);
                float b = m_mouse_trace_last_y - a * m_mouse_trace_last_x;
                int start = Math.Min( x, m_mouse_trace_last_x );
                int end = Math.Max( x, m_mouse_trace_last_x );
                for ( int xx = start; xx <= end; xx++ ) {
                    int yy = (int)(a * xx + b);
                    m_mouse_trace.put( xx, yy );
                }
                m_mouse_trace_last_x = x;
                m_mouse_trace_last_y = y;
            }
        }

        /// <summary>
        /// 現在保持されている軌跡を破棄します
        /// </summary>
        public void clear() {
            m_mouse_trace.clear();
        }

        /// <summary>
        /// 軌跡の点を順に返す反復子を取得します．単純にデータ点を返すのではなく，x+1ごとの補間も含めた点が返される点に注意
        /// </summary>
        /// <returns></returns>
        public Iterator<Point> iterator() {
            return new MouseTracerIterator( m_mouse_trace );
        }

        /// <summary>
        /// 現在保持されているデータの個数を取得します
        /// </summary>
        /// <returns></returns>
        public int size() {
            return m_mouse_trace.size();
        }
    }

#if !JAVA
}
#endif
