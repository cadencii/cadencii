/*
 * VsqBPList.cs
 * Copyright (C) 2008-2010 kbinani
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
using System.Text;
using org.kbinani;
using org.kbinani.java.io;
using org.kbinani.java.util;

namespace org.kbinani.vsq.impl {
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

#if JAVA
    public class VsqBPList implements Cloneable, Serializable{
#else
    /// <summary>
    /// コントロールカーブのデータ点リスト
    /// </summary>
    [Serializable]
    public class VsqBPList : ICloneable {
#endif
        private int[] clocks = new int[INIT_BUFLEN];
        private int[] values = new int[INIT_BUFLEN];
        private long[] ids = new long[INIT_BUFLEN];
        private int length = 0; // clocks, itemsに入っているアイテムの個数
        private int defaultValue = 0;
        private int maxValue = 127;
        private int minValue = 0;
        private long maxId = 0;
        private String name = "";

        const int INIT_BUFLEN = 512;

        class KeyClockIterator : Iterator {
            private VsqBPList m_list;
            private int m_pos;

            public KeyClockIterator( VsqBPList list ) {
                m_list = list;
                m_pos = -1;
            }

            public boolean hasNext() {
                if ( m_pos + 1 < m_list.length ) {
                    return true;
                } else {
                    return false;
                }
            }

            public Object next() {
                m_pos++;
                return m_list.clocks[m_pos];
            }

            public void remove() {
                if ( 0 <= m_pos && m_pos < m_list.length ) {
                    int key = m_list.clocks[m_pos];
                    for ( int i = m_pos; i < m_list.length - 1; i++ ) {
                        m_list.clocks[i] = m_list.clocks[i + 1];
                        m_list.values[i] = m_list.values[i + 1];
                        m_list.ids[i] = m_list.ids[i + 1];
                    }
                    m_list.length = m_list.length - 1;
                }
            }
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

        /// <summary>
        /// コンストラクタ。デフォルト値はココで指定する。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="default_value"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public VsqBPList( String name, int default_value, int minimum, int maximum ) {
            this.name = name;
            defaultValue = default_value;
            maxValue = maximum;
            minValue = minimum;
            maxId = 0;
        }

        private void ensureBufferLength( int length ) {
            if ( length > clocks.Length ) {
                int newLength = length;
                if ( this.length <= 0 ) {
                    newLength = (int)(length * 1.2);
                } else {
                    int order = length / clocks.Length;
                    if ( order <= 1 ) {
                        order = 2;
                    }
                    newLength = clocks.Length * order;
                }
                Array.Resize( ref clocks, newLength );
                Array.Resize( ref values, newLength );
                Array.Resize( ref ids, newLength );
            }
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public int Default {
            get {
                return getDefault();
            }
            set {
                setDefault( value );
            }
        }
#endif

        /// <summary>
        /// このコントロールカーブの名称を取得します．
        /// </summary>
        /// <returns></returns>
        public String getName() {
            if ( name == null ) {
                name = "";
            }
            return name;
        }

        /// <summary>
        /// このコントロールカーブの名称を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setName( String value ) {
            if ( value == null ) {
                name = "";
            } else {
                name = value;
            }
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public String Name {
            get {
                return getName();
            }
            set {
                setName( value );
            }
        }
#endif

        /// <summary>
        /// このコントロールカーブのデータ点が持つIDの最大値を取得します
        /// </summary>
        /// <returns></returns>
        public long getMaxID() {
            return maxId;
        }

        /// <summary>
        /// このコントロールカーブのデフォルト値を取得します
        /// </summary>
        public int getDefault() {
            return defaultValue;
        }

        /// <summary>
        /// このコントロールカーブのデフォルト値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setDefault( int value ) {
            defaultValue = value;
        }

        /// <summary>
        /// データ点のIDを一度クリアし，新たに番号付けを行います．
        /// IDは，Redo,Undo用コマンドが使用するため，このメソッドを呼ぶとRedo,Undo操作が破綻する．XMLからのデシリアライズ直後のみ使用するべき．
        /// </summary>
        public void renumberIDs() {
            maxId = 0;
            for ( int i = 0; i < length; i++ ) {
                maxId++;
                ids[i] = maxId;
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

        /// <summary>
        /// このコントロールカーブのデータ点リストを，テキスト形式で取得します
        /// </summary>
        /// <returns></returns>
        public String getData() {
            StringBuilder ret = new StringBuilder();
            for ( int i = 0; i < length; i++ ) {
                ret.Append( (i == 0 ? "" : ",") + clocks[i] + "=" + values[i] );
            }
            return ret.ToString();
        }

        /// <summary>
        /// このコントロールカーブのデータ点を，テキスト形式のリストから再構築します
        /// </summary>
        /// <param name="value"></param>
        public void setData( String value ) {
            length = 0;
            maxId = 0;
            String[] spl = PortUtil.splitString( value, ',' );
            for ( int i = 0; i < spl.Length; i++ ) {
                String[] spl2 = PortUtil.splitString( spl[i], '=' );
                if ( spl2.Length < 2 ) {
                    continue;
                }
                try {
                    int clock = PortUtil.parseInt( spl2[0] );
                    ensureBufferLength( length + 1 );
                    clocks[length] = clock;
                    values[length] = PortUtil.parseInt( spl2[1] );
                    ids[length] = maxId + 1;
                    maxId++;
                    length++;
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "VsqBPList#setData; ex=" + ex );
#if DEBUG
                    PortUtil.println( "    i=" + i + "; spl2[0]=" + spl2[0] + "; spl2[1]=" + spl2[1] );
#endif
                }
            }
        }

        /// <summary>
        /// このコントロールカーブの同一コピーを作成します
        /// </summary>
        /// <returns></returns>
        public Object clone() {
            VsqBPList res = new VsqBPList( name, defaultValue, minValue, maxValue );
            res.ensureBufferLength( length );
            for ( int i = 0; i < length; i++ ) {
                res.clocks[i] = clocks[i];
                res.values[i] = values[i];
                res.ids[i] = ids[i];
            }
            res.length = length;
            res.maxId = maxId;
            return res;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
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
        /// このコントロールカーブの最大値を取得します。
        /// </summary>
        public int getMaximum() {
            return maxValue;
        }

        /// <summary>
        /// このコントロールカーブの最大値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setMaximum( int value ) {
            maxValue = value;
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
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
        /// このコントロールカーブの最小値を取得します
        /// </summary>
        public int getMinimum() {
            return minValue;
        }

        /// <summary>
        /// このコントロールカーブの最小値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setMinimum( int value ) {
            minValue = value;
        }

        /// <summary>
        /// 指定されたゲートタイムに設定されたデータ点を削除します
        /// </summary>
        /// <param name="clock"></param>
        public void remove( int clock ) {
            ensureBufferLength( length );
            int index = findClock( clock );
            removeElementAt( index );
        }

        /// <summary>
        /// 先頭から第index番目のデータ点を削除します
        /// </summary>
        /// <param name="index"></param>
        public void removeElementAt( int index ) {
            if ( index >= 0 ) {
                for ( int i = index; i < length - 1; i++ ) {
                    clocks[i] = clocks[i + 1];
                    values[i] = values[i + 1];
                    ids[i] = ids[i + 1];
                }
                length--;
            }
        }

        /// <summary>
        /// 指定されたゲートタイムに，データ点が設定されているかどうかを調べます
        /// </summary>
        /// <param name="clock"></param>
        /// <returns></returns>
        public boolean isContainsKey( int clock ) {
            ensureBufferLength( length );
            return (findClock( clock ) >= 0);
        }

        /// <summary>
        /// 時刻clockのデータを時刻new_clockに移動します。
        /// 時刻clockにデータがなければ何もしない。
        /// 時刻new_clockに既にデータがある場合、既存のデータは削除される。
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="new_clock"></param>
        public void move( int clock, int new_clock, int new_value ) {
            ensureBufferLength( length );
            int index = findClock( clock );
            if ( index < 0 ) {
                return;
            }
            int valueAtIndex = values[index];
            long idAtIndex = ids[index];// VsqBPPair item = items[index];
            for ( int i = index; i < length - 1; i++ ) {
                clocks[i] = clocks[i + 1];
                values[i] = values[i + 1];
                ids[i] = ids[i + 1];
            }
            length--;
            int index_new = findClock( new_clock );
            if ( index_new >= 0 ) {
                values[index_new] = new_value;// item;
                ids[index_new] = idAtIndex;
                return;
            } else {
                length++;
                ensureBufferLength( length );
                clocks[length - 1] = new_clock;
                Array.Sort( clocks, 0, length );
                index_new = findClock( new_clock );
                //item.value = new_value;
                for ( int i = length - 1; i > index_new; i-- ){
                    values[i] = values[i - 1];
                    ids[i] = ids[i - 1];
                }
                values[index_new] = new_value;// item;
                ids[index_new] = idAtIndex;
            }
        }

        public void clear() {
            length = 0;
        }

        public int getElement( int index ) {
            return getElementA( index );
        }

        public int getElementA( int index ) {
            return values[index];
        }

        public VsqBPPair getElementB( int index ) {
            return new VsqBPPair( values[index], ids[index] );
        }

        public int getKeyClock( int index ) {
            return clocks[index];
        }

        public int findValueFromID( long id ) {
            int index = findId( id );
            if ( index >= 0 ) {
                return values[index];
            } else {
                return defaultValue;
            }
        }

        /// <summary>
        /// 指定したid値を持つVsqBPPairを検索し、その結果を返します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VsqBPPairSearchContext findElement( long id ) {
            VsqBPPairSearchContext context = new VsqBPPairSearchContext();
            int index = findId( id );
            if ( index >= 0 ) {
                context.clock = clocks[index];
                context.index = index;
                context.point = new VsqBPPair( values[index], id );
                return context;
            } else {
                context.clock = -1;
                context.index = -1;
                context.point = new VsqBPPair( defaultValue, -1 );
                return context;
            }
        }

        public void setValueForID( long id, int value ) {
            int index = findId( id );
            if ( index >= 0 ) {
                values[index] = value;
            }
        }

        public int getValue( int clock, ByRef<Integer> index ) {
            if ( length == 0 ) {
                return defaultValue;
            } else {
                if ( index.value < 0 ) {
                    index.value = 0;
                }
                for ( int i = index.value; i < length; i++ ) {
                    int keyclock = clocks[i];
                    if ( clock < keyclock ) {
                        if ( i > 0 ) {
                            index.value = i;
                            return values[i - 1];
                        } else {
                            index.value = i;
                            return defaultValue;
                        }
                    }
                }
                index.value = length - 1;
                return values[length - 1];
            }
        }

        public void print( ITextWriter writer, int start_clock, String header )
#if JAVA
            throws IOException
#endif
        {
            writer.writeLine( header );
            for ( int i = 0; i < length; i++ ) {
                int key = clocks[i];
                if ( start_clock <= key ) {
                    writer.writeLine( key + "=" + values[i] );
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
            print( new WrappedStreamWriter( writer ), start, header );
        }

        /// <summary>
        /// テキストファイルからデータ点を読込み、現在のリストに追加します
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public String appendFromText( TextStream reader ) {
#if DEBUG
            PortUtil.println( "VsqBPList#appendFromText; start" );
            double started = PortUtil.getCurrentTime();
            int count = 0;
#endif
            int clock = 0;
            int value = 0;
            int minus = 1;
            int mode = 0; // 0: clockを読んでいる, 1: valueを読んでいる
            while ( reader.ready() ) {
                char ch = reader.get();
                int num = -1;
                if ( ch == '1' ) {
                    num = 1;
                } else if ( ch == '2' ) {
                    num = 2;
                } else if ( ch == '3' ) {
                    num = 3;
                } else if ( ch == '4' ) {
                    num = 4;
                } else if ( ch == '5' ) {
                    num = 5;
                } else if ( ch == '6' ) {
                    num = 6;
                } else if ( ch == '7' ) {
                    num = 7;
                } else if ( ch == '8' ) {
                    num = 8;
                } else if ( ch == '9' ) {
                    num = 9;
                } else if ( ch == '0' ) {
                    num = 0;
                } else if ( ch == '\n' ) {
                    if ( mode == 1 ) {
                        addWithoutSort( clock, value * minus );
                        mode = 0;
                        clock = 0;
                        value = 0;
                        minus = 1;
                    }
                    continue;
                } else if ( ch == '[' ) {
                    if ( mode == 1 ) {
                        addWithoutSort( clock, value * minus );
                        mode = 0;
                        clock = 0;
                        value = 0;
                        minus = 1;
                    }
                    reader.setPointer( reader.getPointer() - 1 );
                    break;
                } else if ( ch == '=' ) {
                    mode = 1;
                    continue;
                } else if ( ch == '-' ) {
                    minus = -1;
                    continue;
                }
                if( num >= 0 ) {
                    if ( mode == 0 ) {
                        clock = clock * 10 + num;
                    } else {
                        value = value * 10 + num;
                    }
                }
            }
            return reader.readLine();
        }

        public int size() {
            return length;
        }

        public Iterator keyClockIterator() {
            return new KeyClockIterator( this );
        }

        private int findClock( int clock ) {
            return Array.BinarySearch( clocks, 0, length, clock );
            //return Array.IndexOf( clocks, value, 0, length );
        }

        private int findId( long id ) {
            return Array.BinarySearch( ids, 0, length, id );
        }

        /// <summary>
        /// 並べ替え，既存の値との重複チェックを行わず，リストの末尾にデータ点を追加する
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private void addWithoutSort( int clock, int value ) {
            ensureBufferLength( length + 1 );
            clocks[length] = clock;
            maxId++;
            values[length] = value;
            ids[length] = maxId;
            length++;
        }

        public long add( int clock, int value ) {
            ensureBufferLength( length );
            int index = findClock( clock );
            if ( index >= 0 ) {
                //VsqBPPair v = items[index];
                //v.value = value;
                values[index] = value;
                return ids[index];
            } else {
                length++;
                ensureBufferLength( length );
                clocks[length - 1] = clock;
                Array.Sort( clocks, 0, length );
                index = findClock( clock );
                maxId++;
                for ( int i = length - 1; i > index; i-- ) {
                    values[i] = values[i - 1];
                    ids[i] = ids[i - 1];
                }
                values[index] = value;// new VsqBPPair( value, maxId );
                ids[index] = maxId;
                return maxId;
            }
        }

        public void addWithID( int clock, int value, long id ) {
            ensureBufferLength( length );
            int index = findClock( clock );
            if ( index >= 0 ) {
                //VsqBPPair v = items[index];
                //v.value = value;
                //v.id = id;
                values[index] = value;
                ids[index] = id;
            } else {
                length++;
                ensureBufferLength( length );
                clocks[length - 1] = clock;
                Array.Sort( clocks, 0, length );
                index = findClock( clock );
                for ( int i = length - 1; i > index; i-- ) {
                    values[i] = values[i - 1];
                    ids[i] = ids[i - 1];
                }
                values[index] = value;// new VsqBPPair( value, id );
                ids[index] = id;
                maxId = Math.Max( maxId, id );
            }
        }

        /// <summary>
        /// このコントロールカーブのデータ点の中から，指定されたIDのものを削除します
        /// </summary>
        /// <param name="id"></param>
        public void removeWithID( long id ) {
            int index = findId( id );
            if ( index >= 0 ) {
                for ( int j = index; j < length - 1; j++ ) {
                    values[j] = values[j + 1];
                    clocks[j] = clocks[j + 1];
                    ids[j] = ids[j + 1];
                }
                length--;
            }
        }

        /// <summary>
        /// ゲートタイムclockにおける，このコントロールカーブの値を取得します
        /// </summary>
        /// <param name="clock">ゲートタイム</param>
        /// <returns>ゲートタイムclockにおける，このコントロールカーブの値</returns>
        public int getValue( int clock ) {
            ensureBufferLength( length );
            int index = findClock( clock );
            if ( index >= 0 ) {
                return values[index];
            } else {
                if ( length <= 0 ) {
                    return defaultValue;
                } else {
                    int draft = -1;
                    for ( int i = 0; i < length; i++ ) {
                        int c = clocks[i];
                        if ( clock < c ) {
                            break;
                        }
                        draft = i;
                    }
                    if ( draft < 0 ) {
                        return defaultValue;
                    } else {
                        return values[draft];
                    }
                }
            }
        }
    }

#if !JAVA
}
#endif


#if !JAVA
namespace org.kbinani.vsq {
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

#if JAVA
    public class VsqBPList implements Cloneable, Serializable{
#else
    /// <summary>
    /// コントロールカーブのデータ点リスト
    /// </summary>
    [Serializable]
    public class VsqBPList : ICloneable {
#endif
        private int[] clocks;
        private VsqBPPair[] items;
        private int length = 0; // clocks, itemsに入っているアイテムの個数
        private int defaultValue = 0;
        private int maxValue = 127;
        private int minValue = 0;
        private long maxId = 0;
        private String name = "";

        const int INIT_BUFLEN = 512;

        class KeyClockIterator : Iterator {
            private VsqBPList m_list;
            private int m_pos;

            public KeyClockIterator( VsqBPList list ) {
                m_list = list;
                m_pos = -1;
            }

            public boolean hasNext() {
                if ( m_pos + 1 < m_list.length ) {
                    return true;
                } else {
                    return false;
                }
            }

            public Object next() {
                m_pos++;
                return m_list.clocks[m_pos];
            }

            public void remove() {
                if ( 0 <= m_pos && m_pos < m_list.length ) {
                    int key = m_list.clocks[m_pos];
                    for ( int i = m_pos; i < m_list.length - 1; i++ ) {
                        m_list.clocks[i] = m_list.clocks[i + 1];
                        m_list.items[i] = m_list.items[i + 1];
                    }
                    m_list.length = m_list.length - 1;
                }
            }
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

        /// <summary>
        /// コンストラクタ。デフォルト値はココで指定する。
        /// </summary>
        /// <param name="default_value"></param>
        public VsqBPList( String name, int default_value, int minimum, int maximum ) {
            this.name = name;
            defaultValue = default_value;
            maxValue = maximum;
            minValue = minimum;
            maxId = 0;
        }

        private void ensureBufferLength( int length ) {
            if ( clocks == null ) {
                clocks = new int[INIT_BUFLEN];
            }
            if ( items == null ) {
                items = new VsqBPPair[INIT_BUFLEN];
            }
            if ( length > clocks.Length ) {
                int newLength = length;
                if ( this.length <= 0 ) {
                    newLength = (int)(length * 1.2);
                } else {
                    int order = length / clocks.Length;
                    if ( order <= 1 ) {
                        order = 2;
                    }
                    newLength = clocks.Length * order;
                }
                Array.Resize( ref clocks, newLength );
                Array.Resize( ref items, newLength );
            }
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
        public String Name {
            get {
                return getName();
            }
            set {
                setName( value );
            }
        }
#endif

        public long getMaxID() {
            return maxId;
        }

        /// <summary>
        /// このBPListのデフォルト値を取得します
        /// </summary>
        public int getDefault() {
            return defaultValue;
        }

        public void setDefault( int value ) {
            defaultValue = value;
        }

        /// <summary>
        /// データ点のIDを一度クリアし，新たに番号付けを行います．
        /// IDは，Redo,Undo用コマンドが使用するため，このメソッドを呼ぶとRedo,Undo操作が破綻する．XMLからのデシリアライズ直後のみ使用するべき．
        /// </summary>
        public void renumberIDs() {
            maxId = 0;
            for ( int i = 0; i < length; i++ ) {
                maxId++;
                VsqBPPair v = items[i];
                v.id = maxId;
                items[i] = v;
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
            StringBuilder ret = new StringBuilder();
            for ( int i = 0; i < length; i++ ) {
                ret.Append( (i == 0 ? "" : ",") + clocks[i] + "=" + items[i].value );
            }
            return ret.ToString();
        }

        public void setData( String value ) {
            length = 0;
            maxId = 0;
            String[] spl = PortUtil.splitString( value, ',' );
            for ( int i = 0; i < spl.Length; i++ ) {
                String[] spl2 = PortUtil.splitString( spl[i], '=' );
                if ( spl2.Length < 2 ) {
                    continue;
                }
                try {
                    int clock = PortUtil.parseInt( spl2[0] );
                    ensureBufferLength( length + 1 );
                    clocks[length] = clock;
                    items[length] = new VsqBPPair( PortUtil.parseInt( spl2[1] ), maxId + 1 );
                    maxId++;
                    length++;
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "VsqBPList#setData; ex=" + ex );
#if DEBUG
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
            VsqBPList res = new VsqBPList( name, defaultValue, minValue, maxValue );
            res.ensureBufferLength( length );
            for ( int i = 0; i < length; i++ ) {
                res.clocks[i] = clocks[i];
#if JAVA
                res.items[i] = (VsqBPPair)items[i].clone();
#else
                res.items[i] = items[i];
#endif
            }
            res.length = length;
            res.maxId = maxId;
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
            return maxValue;
        }

        public void setMaximum( int value ) {
            maxValue = value;
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
            return minValue;
        }

        public void setMinimum( int value ) {
            minValue = value;
        }

        public void remove( int clock ) {
            ensureBufferLength( length );
            int index = find( clock );
            removeElementAt( index );
        }

        public void removeElementAt( int index ) {
            if ( index >= 0 ) {
                for ( int i = index; i < length - 1; i++ ) {
                    clocks[i] = clocks[i + 1];
                    items[i] = items[i + 1];
                }
                length--;
            }
        }

        public boolean isContainsKey( int clock ) {
            ensureBufferLength( length );
            return (find( clock ) >= 0);
        }

        /// <summary>
        /// 時刻clockのデータを時刻new_clockに移動します。
        /// 時刻clockにデータがなければ何もしない。
        /// 時刻new_clockに既にデータがある場合、既存のデータは削除される。
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="new_clock"></param>
        public void move( int clock, int new_clock, int new_value ) {
            ensureBufferLength( length );
            int index = find( clock );
            if ( index < 0 ) {
                return;
            }
            VsqBPPair item = items[index];
            for ( int i = index; i < length - 1; i++ ) {
                clocks[i] = clocks[i + 1];
                items[i] = items[i + 1];
            }
            length--;
            int index_new = find( new_clock );
            if ( index_new >= 0 ) {
                item.value = new_value;
                items[index_new] = item;
                return;
            } else {
                length++;
                ensureBufferLength( length );
                clocks[length - 1] = new_clock;
                Array.Sort( clocks, 0, length );
                index_new = find( new_clock );
                item.value = new_value;
                for ( int i = length - 1; i > index_new; i-- ) {
                    items[i] = items[i - 1];
                }
                items[index_new] = item;
            }
        }

        public void clear() {
            length = 0;
        }

        public int getElement( int index ) {
            return getElementA( index );
        }

        public int getElementA( int index ) {
            return items[index].value;
        }

        public VsqBPPair getElementB( int index ) {
            return items[index];
        }

        public int getKeyClock( int index ) {
            return clocks[index];
        }

        public int findValueFromID( long id ) {
            for ( int i = 0; i < length; i++ ) {
                VsqBPPair item = items[i];
                if ( item.id == id ) {
                    return item.value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 指定したid値を持つVsqBPPairを検索し、その結果を返します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VsqBPPairSearchContext findElement( long id ) {
            VsqBPPairSearchContext context = new VsqBPPairSearchContext();
            for ( int i = 0; i < length; i++ ) {
                VsqBPPair item = items[i];
                if ( item.id == id ) {
                    context.clock = clocks[i];
                    context.index = i;
                    context.point = item;
                    return context;
                }
            }
            context.clock = -1;
            context.index = -1;
            context.point = new VsqBPPair( defaultValue, -1 );
            return context;
        }

        public void setValueForID( long id, int value ) {
            for ( int i = 0; i < length; i++ ) {
                VsqBPPair item = items[i];
                if ( item.id == id ) {
                    item.value = value;
                    items[i] = item;
                    break;
                }
            }
        }

        public int getValue( int clock, ByRef<Integer> index ) {
            if ( length == 0 ) {
                return defaultValue;
            } else {
                if ( index.value < 0 ) {
                    index.value = 0;
                }
                for ( int i = index.value; i < length; i++ ) {
                    int keyclock = clocks[i];
                    if ( clock < keyclock ) {
                        if ( i > 0 ) {
                            index.value = i;
                            return items[i - 1].value;
                        } else {
                            index.value = i;
                            return defaultValue;
                        }
                    }
                }
                index.value = length - 1;
                return items[length - 1].value;
            }
        }

        private void printCor( ITextWriter writer, int start_clock, String header )
#if JAVA
            throws IOException
#endif
 {
            writer.writeLine( header );
            for ( int i = 0; i < length; i++ ) {
                int key = clocks[i];
                if ( start_clock <= key ) {
                    int val = items[i].value;
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
        public void print( ITextWriter writer, int start, String header )
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
        public String appendFromText( TextStream reader ) {
#if DEBUG
            PortUtil.println( "VsqBPList#appendFromText; start" );
            double started = PortUtil.getCurrentTime();
            int count = 0;
#endif
            int clock = 0;
            int value = 0;
            int minus = 1;
            int mode = 0; // 0: clockを読んでいる, 1: valueを読んでいる
            while ( reader.ready() ) {
                char ch = reader.get();
                if ( ch == '\n' ) {
                    if ( mode == 1 ) {
                        addWithoutSort( clock, value * minus );
                        mode = 0;
                        clock = 0;
                        value = 0;
                        minus = 1;
                    }
                    continue;
                }
                if ( ch == '[' ) {
                    if ( mode == 1 ) {
                        addWithoutSort( clock, value * minus );
                        mode = 0;
                        clock = 0;
                        value = 0;
                        minus = 1;
                    }
                    reader.setPointer( reader.getPointer() - 1 );
                    break;
                }
                if ( ch == '=' ) {
                    mode = 1;
                    continue;
                }
                if ( ch == '-' ) {
                    minus = -1;
                    continue;
                }
                if ( Char.IsNumber( ch ) ) {
                    int num = 0;
                    if ( ch == '1' ) {
                        num = 1;
                    } else if ( ch == '2' ) {
                        num = 2;
                    } else if ( ch == '3' ) {
                        num = 3;
                    } else if ( ch == '4' ) {
                        num = 4;
                    } else if ( ch == '5' ) {
                        num = 5;
                    } else if ( ch == '6' ) {
                        num = 6;
                    } else if ( ch == '7' ) {
                        num = 7;
                    } else if ( ch == '8' ) {
                        num = 8;
                    } else if ( ch == '9' ) {
                        num = 9;
                    }
                    if ( mode == 0 ) {
                        clock = clock * 10 + num;
                    } else {
                        value = value * 10 + num;
                    }
                }
            }
            return reader.readLine();
        }

        public int size() {
            return length;
        }

        public Iterator keyClockIterator() {
            return new KeyClockIterator( this );
        }

        private int find( int value ) {
            return Array.BinarySearch( clocks, 0, length, value );
            //return Array.IndexOf( clocks, value, 0, length );
        }

        /// <summary>
        /// 並べ替え，既存の値との重複チェックを行わず，リストの末尾にデータ点を追加する
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private void addWithoutSort( int clock, int value ) {
            ensureBufferLength( length + 1 );
            clocks[length] = clock;
            maxId++;
#if JAVA
            items[length] = new VsqBPPair( value, maxId );
#else
            items[length].value = value;
            items[length].id = maxId;
#endif
            length++;
        }

        public long add( int clock, int value ) {
            ensureBufferLength( length );
            int index = find( clock );
            if ( index >= 0 ) {
                VsqBPPair v = items[index];
                v.value = value;
                items[index] = v;
                return v.id;
            } else {
                length++;
                ensureBufferLength( length );
                clocks[length - 1] = clock;
                Array.Sort( clocks, 0, length );
                index = find( clock );
                maxId++;
                for ( int i = length - 1; i > index; i-- ) {
                    items[i] = items[i - 1];
                }
                items[index] = new VsqBPPair( value, maxId );
                return maxId;
            }
        }

        public void addWithID( int clock, int value, long id ) {
            ensureBufferLength( length );
            int index = find( clock );
            if ( index >= 0 ) {
                VsqBPPair v = items[index];
                v.value = value;
                v.id = id;
                items[index] = v;
            } else {
                length++;
                ensureBufferLength( length );
                clocks[length - 1] = clock;
                Array.Sort( clocks, 0, length );
                index = find( clock );
                for ( int i = length - 1; i > index; i-- ) {
                    items[i] = items[i - 1];
                }
                items[index] = new VsqBPPair( value, id );
                maxId = Math.Max( maxId, id );
            }
        }

        public void removeWithID( long id ) {
            for ( int i = 0; i < length; i++ ) {
                if ( items[i].id == id ) {
                    for ( int j = i; j < length - 1; j++ ) {
                        items[j] = items[j + 1];
                        clocks[j] = clocks[j + 1];
                    }
                    length--;
                    break;
                }
            }
        }

        public int getValue( int clock ) {
            ensureBufferLength( length );
            int index = find( clock );
            if ( index >= 0 ) {
                return items[index].value;
            } else {
                if ( length <= 0 ) {
                    return defaultValue;
                } else {
                    int draft = -1;
                    for ( int i = 0; i < length; i++ ) {
                        int c = clocks[i];
                        if ( clock < c ) {
                            break;
                        }
                        draft = i;
                    }
                    if ( draft < 0 ) {
                        return defaultValue;
                    } else {
                        return items[draft].value;
                    }
                }
            }
        }
    }

#if !JAVA
}
#endif

