/*
 * VsqBPList.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using bocoree;

namespace Boare.Lib.Vsq {

    using boolean = System.Boolean;

    /// <summary>
    /// BPListのデータ部分を取り扱うためのクラス。
    /// </summary>
    [Serializable]
    public class VsqBPList : ICloneable {
        private SortedList<int, VsqBPPair> m_list = new SortedList<int, VsqBPPair>();
        private int m_default = 0;
        private int m_maximum = 127;
        private int m_minimum = 0;
        /// <summary>
        /// このリストに設定されたidの最大値．次にデータ点が追加されたときは，個の値+1がidとして利用される．削除された場合でも減らない
        /// </summary>
        private int m_max_id = 0;

        private class KeyClockIterator : Iterator {
            private SortedList<int, VsqBPPair> m_list;
            private int m_pos;

            public KeyClockIterator( SortedList<int, VsqBPPair> list ) {
                m_list = list;
                m_pos = -1;
            }

            public boolean hasNext() {
                if ( m_pos + 1 < m_list.Keys.Count ) {
                    return true;
                } else {
                    return false;
                }
            }

            public object next() {
                m_pos++;
                return m_list.Keys[m_pos];
            }

            public void remove() {
                if ( 0 <= m_pos && m_pos < m_list.Keys.Count ) {
                    int key = m_list.Keys[m_pos];
                    m_list.Remove( key );
                }
            }
        }

        public VsqBPList()
            : this( 0, 0, 64 ) {
        }

        public int Default {
            get {
                return getDefault();
            }
            set {
                setDefault( value );
            }
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
            for ( Iterator itr = keyClockIterator(); itr.hasNext(); ) {
                VsqBPPair item = (VsqBPPair)itr.next();
                m_max_id++;
                item.id = m_max_id;
            }
        }

        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public String Data {
            get {
                String ret = "";
                int count = -1;
                foreach ( int key in m_list.Keys ) {
                    count++;
                    ret += (count == 0 ? "" : "," ) + key + "=" + m_list[key].value;
                }
                return ret;
            }
            set {
                m_list.Clear();
                m_max_id = 0;
                String[] spl = PortUtil.splitString( value, ',' );
                for ( int i = 0; i < spl.Length; i++ ) {
                    String[] spl2 = PortUtil.splitString( spl[i], '=' );
                    if ( spl2.Length < 2 ) {
                        continue;
                    }
                    try {
                        m_list.Add( PortUtil.parseInt( spl2[0] ), new VsqBPPair( PortUtil.parseInt( spl2[1] ), m_max_id + 1 ) );
                        m_max_id++;
                    } catch ( Exception ex ) {
#if DEBUG
                        Console.WriteLine( "    ex=" + ex );
                        Console.WriteLine( "    i=" + i + "; spl2[0]=" + spl2[0] + "; spl2[1]=" + spl2[1] );
#endif
                    }
                }
            }
        }

        /// <summary>
        /// このVsqBPListの同一コピーを作成します
        /// </summary>
        /// <returns></returns>
        public Object clone() {
            VsqBPList res = new VsqBPList( getDefault(), getMinimum(), getMaximum() );
            foreach ( int key in m_list.Keys ) {
                res.m_list.Add( key, m_list[key] );
            }
            res.m_max_id = m_max_id;
            return res;
        }

        public object Clone() {
            return clone();
        }

        /// <summary>
        /// コンストラクタ。デフォルト値はココで指定する。
        /// </summary>
        /// <param name="default_value"></param>
        public VsqBPList( int default_value, int minimum, int maximum ) {
            m_default = default_value;
            m_maximum = maximum;
            m_minimum = minimum;
            m_max_id = 0;
        }

        public int Maximum {
            get {
                return getMaximum();
            }
            set {
                setMaximum( value );
            }
        }

        /// <summary>
        /// このリストに設定された最大値を取得します。
        /// </summary>
        public int getMaximum() {
            return m_maximum;
        }

        public void setMaximum( int value ){
            m_maximum = value;
        }

        public int Minimum {
            get {
                return getMinimum();
            }
            set {
                setMinimum( value );
            }
        }

        /// <summary>
        /// このリストに設定された最小値を取得します
        /// </summary>
        public int getMinimum() {
            return m_minimum;
        }

        public void setMinimum( int value ) {
            m_minimum = value;
        }

        public Iterator keyClockIterator() {
            return new KeyClockIterator( m_list );
        }

        public void remove( int clock ) {
            if ( m_list.ContainsKey( clock ) ) {
                m_list.Remove( clock );
            }
        }

        public boolean isContainsKey( int clock ) {
            return m_list.ContainsKey( clock );
        }

        public int size() {
            return m_list.Count;
        }

        public int[] getKeys() {
            Vector<int> t = new Vector<int>();
            foreach( int key in m_list.Keys ){
                t.add( key );
            }
            return t.toArray( new Int32[]{} );
        }

        /// <summary>
        /// 時刻clockのデータを時刻new_clockに移動します。
        /// 時刻clockにデータがなければ何もしない。
        /// 時刻new_clockに既にデータがある場合、既存のデータは削除される。
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="new_clock"></param>
        public void move( int clock, int new_clock, int new_value ) {
            if ( !m_list.ContainsKey( clock ) ) {
                return;
            }
            VsqBPPair item = m_list[clock];
            m_list.Remove( clock );
            if ( m_list.ContainsKey( new_clock ) ) {
                m_list.Remove( new_clock );
            }
            item.value = new_value;
            m_list.Add( new_clock, item );
        }

        public void clear() {
            m_list.Clear();
        }

        /// <summary>
        /// 新しいデータ点を追加します。
        /// 戻り値に、新しいデータ点のIDを返します
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long add( int clock, int value ) {
            lock ( m_list ) {
                if ( m_list.ContainsKey( clock ) ) {
                    VsqBPPair v = m_list[clock];
                    v.value = value;
                    m_list[clock] = v;
                    return v.id;
                } else {
                    VsqBPPair v = new VsqBPPair( value, m_max_id + 1 );
                    m_max_id++;
#if DEBUG
                    //Console.WriteLine( "VsqBPList#add; m_max_id=" + m_max_id );
#endif
                    m_list.Add( clock, v );
                    return m_max_id;
                }
            }
        }

        public int getElement( int index ) {
            return getElementA( index );
        }

        public int getElementA( int index ) {
            return m_list[m_list.Keys[index]].value;
        }

        public VsqBPPair getElementB( int index ) {
            return m_list[m_list.Keys[index]];
        }

        public int getKeyClock( int index ) {
            return m_list.Keys[index];
        }

        public int findValueFromID( long id ) {
            int c = m_list.Keys.Count;
            foreach ( int key in m_list.Keys ) {
                if ( m_list[key].id == id ) {
                    return m_list[key].value;
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
            int c = m_list.Keys.Count;
            for ( int i = 0; i < c; i++ ) {
                int clock = m_list.Keys[i];
                VsqBPPair item = m_list[clock];
                if ( item.id == id ) {
                    context.clock = clock;
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
            int c = m_list.Keys.Count;
            foreach ( int key in m_list.Keys ) {
                if ( m_list[key].id == id ) {
                    VsqBPPair v = m_list[key];
                    v.value = value;
                    m_list[key] = v;
                    break;
                }
            }
        }

        public int getValue( int clock, ref int index ) {
            if ( m_list.Count == 0 ) {
                return m_default;
            } else {
                if ( index < 0 ) {
                    index = 0;
                }
                for ( int i = index ; i < m_list.Keys.Count; i++ ) {
                    int keyclock = m_list.Keys[i];
                    if ( clock < keyclock ) {
                        if ( i > 0 ) {
                            index = i;
                            return m_list[m_list.Keys[i - 1]].value;
                        } else {
                            index = i;
                            return m_default;
                        }
                    }
                }
                index = m_list.Keys.Count - 1;
                return m_list[m_list.Keys[m_list.Keys.Count - 1]].value;
            }
        }

        public int getValue( int clock ) {
            if ( m_list.Count == 0 ) {
                return m_default;
            } else {
                for ( int i = 0; i < m_list.Keys.Count; i++ ) {
                    int keyclock = m_list.Keys[i];
                    if ( clock < keyclock ) {
                        if ( i > 0 ) {
                            return m_list[m_list.Keys[i - 1]].value;
                        } else {
                            return m_default;
                        }
                    }
                }
                return m_list[m_list.Keys[m_list.Keys.Count - 1]].value;
            }
        }

        private void printCor( ITextWriter writer, int start, String header ) {
            boolean first = true;
            foreach ( int key in m_list.Keys ) {
                if ( start <= key ) {
                    if ( first ) {
                        writer.writeLine( header );
                        first = false;
                    }
                    int val = m_list[key].value;
                    writer.writeLine( key + "=" + val );
                }
            }
        }

        /// <summary>
        /// このBPListの内容をテキストファイルに書き出します
        /// </summary>
        /// <param name="writer"></param>
        public void print( StreamWriter writer, int start, String header ) {
            printCor( new WrappedStreamWriter( writer ), start, header );
        }

        /// <summary>
        /// このBPListの内容をテキストファイルに書き出します
        /// </summary>
        /// <param name="writer"></param>
        public void print( TextMemoryStream writer, int start, String header ) {
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
                int i1 = PortUtil.parseInt( spl[0] );
                int i2 = PortUtil.parseInt( spl[1] );
                VsqBPPair v = new VsqBPPair( i2, m_max_id + 1 );
                m_max_id++;
                m_list.Add( i1, v );
                if ( reader.peek() < 0 ) {
                    break;
                } else {
                    last_line = reader.readLine();
                }
            }
            return last_line;
        }
    }

}
