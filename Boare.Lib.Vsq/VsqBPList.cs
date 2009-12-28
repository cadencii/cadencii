/*
 * VsqBPList.cs
 * Copyright (C) 2008-2009 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.util.*;
import java.io.*;
import org.kbinani.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.io;
using org.kbinani.java.util;

namespace org.kbinani.vsq {
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

#if JAVA
    public class VsqBPList implements Cloneable, Serializable{
#else
    [Serializable]
    public class VsqBPList : ICloneable {
#endif
        private Vector<Integer> m_clock = new Vector<Integer>();
        private Vector<VsqBPPair> m_items = new Vector<VsqBPPair>();
        private int m_default = 0;
        private int m_maximum = 127;
        private int m_minimum = 0;
        private long m_max_id = 0;
        private String name = "";

        class KeyClockIterator : Iterator {
            private VsqBPList m_list;
            private int m_pos;

            public KeyClockIterator( VsqBPList list ) {
                m_list = list;
                m_pos = -1;
            }

            public boolean hasNext() {
                if ( m_pos + 1 < m_list.m_clock.size() ) {
                    return true;
                } else {
                    return false;
                }
            }

            public Object next() {
                m_pos++;
                return m_list.m_clock.get( m_pos );
            }

            public void remove() {
                if ( 0 <= m_pos && m_pos < m_list.m_clock.size() ) {
                    int key = m_list.m_clock.get( m_pos );
                    m_list.m_clock.removeElementAt( m_pos );
                    m_list.m_items.removeElementAt( m_pos );
                }
            }
        }

        /// <summary>
        /// コンストラクタ。デフォルト値はココで指定する。
        /// </summary>
        /// <param name="default_value"></param>
        public VsqBPList( String name, int default_value, int minimum, int maximum ) {
            this.name = name;
            m_default = default_value;
            m_maximum = maximum;
            m_minimum = minimum;
            m_max_id = 0;
        }

        public VsqBPList()
#if JAVA
        {
#else
            :
#endif
            this( "", 0, 0, 64 )
#if JAVA
            ;
#else
        {
#endif
        }

#if !JAVA
        public int Default {
            get {
                return getDefault();
            }
            set {
                setDefault( value );
            }
        }
#endif

        public String getName() {
            if ( name == null ) {
                name = "";
            }
            return name;
        }

        public void setName( String value ) {
            if ( value == null ) {
                name = "";
            } else {
                name = value;
            }
        }

#if !JAVA
        public String Name{
            get{
                return getName();
            }
            set{
                setName( value );
            }
        }
#endif

        public long getMaxID() {
            return m_max_id;
        }

        /// <summary>
        /// このBPListのデフォルト値を取得します
        /// </summary>
        public int getDefault() {
            return m_default;
        }

        public void setDefault( int value ) {
            m_default = value;
        }

        /// <summary>
        /// データ点のIDを一度クリアし，新たに番号付けを行います．
        /// IDは，Redo,Undo用コマンドが使用するため，このメソッドを呼ぶとRedo,Undo操作が破綻する．XMLからのデシリアライズ直後のみ使用するべき．
        /// </summary>
        public void renumberIDs() {
            m_max_id = 0;
            int count = m_items.size();
            for ( int i = 0; i < count; i++ ) {
                m_max_id++;
                VsqBPPair v = m_items.get( i );
                v.id = m_max_id;
                m_items.set( i, v );
            }
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public String Data {
            get {
                return getData();
            }
            set {
                setData( value );
            }
        }
#endif

        public String getData() {
            String ret = "";
            int count = -1;
            int size = m_clock.size();
            for ( int i = 0; i < size; i++ ) {
                count++;
                ret += (count == 0 ? "" : ",") + m_clock.get( i ) + "=" + m_items.get( i ).value;
            }
            return ret;
        }

        public void setData( String value ) {
            m_clock.clear();
            m_items.clear();
            m_max_id = 0;
            String[] spl = PortUtil.splitString( value, ',' );
            for ( int i = 0; i < spl.Length; i++ ) {
                String[] spl2 = PortUtil.splitString( spl[i], '=' );
                if ( spl2.Length < 2 ) {
                    continue;
                }
                try {
                    int clock = PortUtil.parseInt( spl2[0] );
                    m_clock.add( clock );
                    m_items.add( new VsqBPPair( PortUtil.parseInt( spl2[1] ), m_max_id + 1 ) );
                    m_max_id++;
                } catch ( Exception ex ) {
#if DEBUG
                    PortUtil.println( "    ex=" + ex );
                    PortUtil.println( "    i=" + i + "; spl2[0]=" + spl2[0] + "; spl2[1]=" + spl2[1] );
#endif
                }
            }
        }

        /// <summary>
        /// このVsqBPListの同一コピーを作成します
        /// </summary>
        /// <returns></returns>
        public Object clone() {
            VsqBPList res = new VsqBPList( name, m_default, m_minimum, m_maximum );
            int count = m_clock.size();
            for ( int i = 0; i < count; i++ ) {
                res.m_clock.add( m_clock.get( i ) );
#if JAVA
                res.m_items.add( (VsqBPPair)m_items.get( i ).clone() );
#else
                res.m_items.add( m_items.get( i ) );
#endif
            }
            res.m_max_id = m_max_id;
            return res;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

#if !JAVA
        public int Maximum {
            get {
                return getMaximum();
            }
            set {
                setMaximum( value );
            }
        }
#endif

        /// <summary>
        /// このリストに設定された最大値を取得します。
        /// </summary>
        public int getMaximum() {
            return m_maximum;
        }

        public void setMaximum( int value ) {
            m_maximum = value;
        }

#if !JAVA
        public int Minimum {
            get {
                return getMinimum();
            }
            set {
                setMinimum( value );
            }
        }
#endif

        /// <summary>
        /// このリストに設定された最小値を取得します
        /// </summary>
        public int getMinimum() {
            return m_minimum;
        }

        public void setMinimum( int value ) {
            m_minimum = value;
        }

        public void remove( int clock ) {
            int index = m_clock.indexOf( clock );
            removeElementAt( index );
        }

        public void removeElementAt( int index ) {
            if ( index >= 0 ) {
                m_clock.removeElementAt( index );
                m_items.removeElementAt( index );
            }
        }

        public boolean isContainsKey( int clock ) {
            return m_clock.contains( clock );
        }

        /* public int[] getKeys() {
            Vector<Integer> t = new Vector<Integer>();
            foreach( int key in m_list.Keys ){
                t.add( key );
            }
            return t.toArray( new Integer[]{} );
        }*/

        /// <summary>
        /// 時刻clockのデータを時刻new_clockに移動します。
        /// 時刻clockにデータがなければ何もしない。
        /// 時刻new_clockに既にデータがある場合、既存のデータは削除される。
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="new_clock"></param>
        public void move( int clock, int new_clock, int new_value ) {
            int index = m_clock.indexOf( clock );
            if ( index < 0 ) {
                return;
            }
            VsqBPPair item = m_items.get( index );
            m_clock.removeElementAt( index );
            m_items.removeElementAt( index );
            int index_new = m_clock.indexOf( new_clock );
            if ( index_new >= 0 ) {
                item.value = new_value;
                m_items.set( index_new, item );
                return;
            } else {
                m_clock.add( new_clock );
                Collections.sort( m_clock );
                index_new = m_clock.indexOf( new_clock );
                item.value = new_value;
                m_items.insertElementAt( item, index_new );
            }
        }

        public void clear() {
            m_clock.clear();
            m_items.clear();
        }

        public int getElement( int index ) {
            return getElementA( index );
        }

        public int getElementA( int index ) {
            return m_items.get( index ).value;
        }

        public VsqBPPair getElementB( int index ) {
            return m_items.get( index );
        }

        public int getKeyClock( int index ) {
            return m_clock.get( index );
        }

        public int findValueFromID( long id ) {
            int c = m_items.size();
            for ( int i = 0; i < c; i++ ) {
                VsqBPPair item = m_items.get( i );
                if ( item.id == id ) {
                    return item.value;
                }
            }
            return m_default;
        }

        /// <summary>
        /// 指定したid値を持つVsqBPPairを検索し、その結果を返します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VsqBPPairSearchContext findElement( long id ) {
            VsqBPPairSearchContext context = new VsqBPPairSearchContext();
            int c = m_items.size();
            for ( int i = 0; i < c; i++ ) {
                VsqBPPair item = m_items.get( i );
                if ( item.id == id ) {
                    context.clock = m_clock.get( i );
                    context.index = i;
                    context.point = item;
                    return context;
                }
            }
            context.clock = -1;
            context.index = -1;
            context.point = new VsqBPPair( m_default, -1 );
            return context;
        }

        public void setValueForID( long id, int value ) {
            int c = m_items.size();
            for ( int i = 0; i < c; i++ ) {
                VsqBPPair item = m_items.get( i );
                if ( item.id == id ) {
                    item.value = value;
                    m_items.set( i, item );
                    break;
                }
            }
        }

        public int getValue( int clock, ByRef<Integer> index ) {
            int count = m_clock.size();
            if ( count == 0 ) {
                return m_default;
            } else {
                if ( index.value < 0 ) {
                    index.value = 0;
                }
                for ( int i = index.value; i < count; i++ ) {
                    int keyclock = m_clock.get( i );
                    if ( clock < keyclock ) {
                        if ( i > 0 ) {
                            index.value = i;
                            return m_items.get( i - 1 ).value;
                        } else {
                            index.value = i;
                            return m_default;
                        }
                    }
                }
                index.value = count - 1;
                return m_items.get( count - 1 ).value;
            }
        }

        private void printCor( ITextWriter writer, int start_clock, String header )
#if JAVA
            throws IOException
#endif
        {
            writer.writeLine( header );
            int c = m_clock.size();
            for ( int i = 0; i < c; i++ ) {
                int key = m_clock.get( i );
                if ( start_clock <= key ) {
                    int val = m_items.get( i ).value;
                    writer.writeLine( key + "=" + val );
                }
            }
        }

        /// <summary>
        /// このBPListの内容をテキストファイルに書き出します
        /// </summary>
        /// <param name="writer"></param>
        public void print( BufferedWriter writer, int start, String header )
#if JAVA
            throws IOException
#endif
        {
            printCor( new WrappedStreamWriter( writer ), start, header );
        }

        /// <summary>
        /// このBPListの内容をテキストファイルに書き出します
        /// </summary>
        /// <param name="writer"></param>
        public void print( TextMemoryStream writer, int start, String header )
#if JAVA
            throws IOException
#endif
        {
            printCor( writer, start, header );
        }

        /// <summary>
        /// テキストファイルからデータ点を読込み、現在のリストに追加します
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public String appendFromText( TextMemoryStream reader ) {
            String last_line = reader.readLine();
            while ( !last_line.StartsWith( "[" ) ) {
                String[] spl = PortUtil.splitString( last_line, new char[] { '=' } );
                int clock = PortUtil.parseInt( spl[0] );
                int value = PortUtil.parseInt( spl[1] );
                this.add( clock, value );
                if ( reader.peek() < 0 ) {
                    break;
                } else {
                    last_line = reader.readLine();
                }
            }
            return last_line;
        }

        public int size() {
            return m_clock.size();
        }

        public Iterator keyClockIterator() {
            return new KeyClockIterator( this );
        }

        public long add( int clock, int value ) {
            int index = m_clock.indexOf( clock );
            if ( index >= 0 ) {
                VsqBPPair v = m_items.get( index );
                v.value = value;
                m_items.set( index, v );
                return v.id;
            } else {
                m_clock.add( clock );
                Collections.sort( m_clock );
                index = m_clock.indexOf( clock );
                m_max_id++;
                m_items.insertElementAt( new VsqBPPair( value, m_max_id ), index );
                return m_max_id;
            }
        }

        public void addWithID( int clock, int value, long id ) {
            int index = m_clock.indexOf( clock );
            if ( index >= 0 ) {
                VsqBPPair v = m_items.get( index );
                v.value = value;
                v.id = id;
                m_items.set( index, v );
            } else {
                m_clock.add( clock );
                Collections.sort( m_clock );
                index = m_clock.indexOf( clock );
                m_items.insertElementAt( new VsqBPPair( value, id ), index );
                m_max_id = Math.Max( m_max_id, id );
            }
        }

        public void removeWithID( long id ) {
            int c = m_items.size();
            for ( int i = 0; i < c; i++ ) {
                if ( m_items.get( i ).id == id ) {
                    m_items.removeElementAt( i );
                    m_clock.removeElementAt( i );
                    break;
                }
            }
        }

        public int getValue( int clock ) {
            int index = m_clock.indexOf( clock );
            if ( index >= 0 ) {
                return m_items.get( index ).value;
            } else {
                int count = m_clock.size();
                if ( count <= 0 ) {
                    return m_default;
                } else {
                    int draft = -1;
                    for ( int i = 0; i < count; i++ ) {
                        int c = m_clock.get( i );
                        if ( clock < c ) {
                            break;
                        }
                        draft = i;
                    }
                    if ( draft < 0 ) {
                        return m_default;
                    } else {
                        return m_items.get( draft ).value;
                    }
                }
            }
        }
    }

#if !JAVA
    /*public class VsqBPList : ICloneable {
        private System.Collections.Generic.SortedList<int, VsqBPPair> m_items = new System.Collections.Generic.SortedList<int, VsqBPPair>();
        private int m_default = 0;
        private int m_maximum = 127;
        private int m_minimum = 0;
        private long m_max_id = 0;
        private String name = "";

        public VsqBPList( String name, int default_value, int minimum, int maximum ) {
            this.name = name;
            m_default = default_value;
            m_maximum = maximum;
            m_minimum = minimum;
        }

        public int getKeyClock( int index ) {
#if JAVA
            return m_items.keySet().get( index );
#else
#endif
        }

        public int getDefault() {
            return m_default;
        }

        public Object clone() {
            VsqBPList ret = new VsqBPList( name, m_default, m_minimum, m_maximum );
            for ( Iterator itr = m_items.keySet().iterator(); itr.hasNext(); ) {
                int clock = (Integer)itr.next();
                ret.m_items.put( clock, m_items.get( clock ) );
            }
            return ret;
        }

#if !JAVA
        public Object Clone() {
            return clone();
        }
#endif

        public int size() {
            return m_items.size();
        }

        public void add( int clock, int value ) {
            if ( m_items.containsKey( clock ) ) {
                VsqBPPair v = m_items.get( clock );
                v.value = value;
                m_items.put( clock, v );
            } else {
                m_max_id++;
                m_items.put( clock, new VsqBPPair( value, m_max_id ) );
            }
        }


    }*/

}
#endif
