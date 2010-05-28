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

namespace org.kbinani.cadencii{
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
