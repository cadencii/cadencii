/*
 * VsqBPList.java
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of com.boare.vsq.
 *
 * com.boare.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.vsq;

import java.util.*;
import java.io.*;
import com.boare.corlib.*;

/// <summary>
/// BPListのデータ部分を取り扱うためのクラス。
/// </summary>
public class VsqBPList implements Cloneable {
    private Vector<KeyValuePair<Integer, Integer>> m_list = new Vector<KeyValuePair<Integer, Integer>>();
    public int defaultValue = 0;
    public int maximum = 127;
    public int minimum = 0;

    class KeyClockIterator implements Iterator {
        private Vector<KeyValuePair<Integer, Integer>> m_list;
        private int m_pos;

        public KeyClockIterator( Vector<KeyValuePair<Integer, Integer>> list ) {
            m_list = list;
            m_pos = -1;
        }

        public boolean hasNext() {
            if ( m_pos + 1 < m_list.size() ) {
                return true;
            } else {
                return false;
            }
        }

        public Object next() {
            m_pos++;
            return m_list.get( m_pos ).key;
        }

        public void remove() {
            if ( 0 <= m_pos && m_pos < m_list.size() ) {
                m_list.remove( m_pos );
            }
        }
    }

    public VsqBPList(){
        this( 0, 0, 64 );
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "defaultValue" ) ){
            return "DefaultValue";
        }else if( name.equals( "minimum" ) ){
            return "Minimum";
        }else if( name.equals( "maximum" ) ){
            return "Maximum";
        }
        return name;
    }

    /// <summary>
    /// XMLシリアライズ用
    /// </summary>
    public String getData(){
        String ret = "";
        int c = m_list.size();
        for( int i = 0; i < c; i++ ){
            KeyValuePair<Integer, Integer> v = m_list.get( i );
            ret += (i == 0 ? "" : "," ) + v.key + "=" + v.value;
        }
        return ret;
    }

    public void setData( String value ){
        m_list.clear();
        String[] spl = value.split( "," );
        for ( int i = 0; i < spl.length; i++ ) {
            String[] spl2 = spl[i].split( "=" );
            if ( spl2.length < 2 ) {
                continue;
            }
            try {
                int k = Integer.parseInt( spl2[0] );
                int v = Integer.parseInt( spl2[1] );
                m_list.add( new KeyValuePair<Integer, Integer>( k, v ) );
            } catch ( Exception ex ) {
            }
        }
    }

    /// <summary>
    /// このVsqBPListの同一コピーを作成します
    /// </summary>
    /// <returns></returns>
    public Object clone() {
        VsqBPList res = new VsqBPList( defaultValue, minimum, maximum );
        int c = m_list.size();
        for( int i = 0; i < c; i++ ){
            KeyValuePair<Integer, Integer> v = m_list.get( i );
            res.m_list.add( new KeyValuePair<Integer, Integer>( v.key, v.value ) );
        }
        return res;
    }

    /// <summary>
    /// コンストラクタ。デフォルト値はココで指定する。
    /// </summary>
    /// <param name="default_value"></param>
    public VsqBPList( int default_value, int minimum_, int maximum_ ) {
        defaultValue = default_value;
        maximum = maximum_;
        minimum = minimum_;
    }

    public Iterator keyClockIterator() {
        return new KeyClockIterator( m_list );
    }

    public void remove( int clock ) {
        int c = m_list.size();
        for( int i = 0; i < c; i++ ){
            int key = m_list.get( i ).key;
            if( key == clock ){
                m_list.remove( i );
            }else if( clock < key ){
                return;
            }
        }
    }

    public boolean isContainsKey( int clock ) {
        int c = m_list.size();
        for( int i = 0; i < c; i++ ){
            int key = m_list.get( i ).key;
            if( key == clock ){
                return true;
            }else if( clock < key ){
                return false;
            }
        }
        return false;
    }

    public int getCount() {
        return m_list.size();
    }

    public int[] getKeys() {
        int c = m_list.size();
        int[] res = new int[c];
        for( int i = 0; i < c; i++ ){
            res[i] = m_list.get( i ).key;
        }
        return res;
    }

    public void clear() {
        m_list.clear();
    }

    /// <summary>
    /// 新しいデータ点を追加します。
    /// </summary>
    /// <param name="clock"></param>
    /// <param name="value"></param>
    public void add( int clock, int value ) {
        int c = m_list.size();
        for( int i = 0; i < c; i++ ){
            KeyValuePair<Integer, Integer> v = m_list.get( i );
            if( v.key == clock ){
                v.value = value;
                break;
            }else if( clock < v.key ){
                m_list.insertElementAt( new KeyValuePair<Integer, Integer>( clock, value ), i );
            }
        }
    }

    public int getElement( int index ) {
        return m_list.get( index ).value;
    }

    public int getKeyClock( int index ) {
        return m_list.get( index ).key;
    }

    /*public int getValue( int clock, ref int index ) {
        if ( m_list.Count == 0 ) {
            return Default;
        } else {
            if ( index < 0 ) {
                index = 0;
            }
            for ( int i = index ; i < m_list.Keys.Count; i++ ) {
                int keyclock = m_list.Keys[i];
                if ( clock < keyclock ) {
                    if ( i > 0 ) {
                        index = i;
                        return m_list[m_list.Keys[i - 1]];
                    } else {
                        index = i;
                        return Default;
                    }
                }
            }
            index = m_list.Keys.Count - 1;
            return m_list[m_list.Keys[m_list.Keys.Count - 1]];
        }
    }*/

    public int getValue( int clock ) {
        int c = m_list.size();
        if ( c == 0 ) {
            return defaultValue;
        } else {
            for ( int i = 0; i < c; i++ ) {
                if ( clock < m_list.get( i ).key ) {
                    if ( i > 0 ) {
                        return m_list.get( i - 1 ).value;
                    } else {
                        return defaultValue;
                    }
                }
            }
            return m_list.get( c - 1 ).value;
        }
    }

    /// <summary>
    /// このBPListの内容をテキストファイルに書き出します
    /// </summary>
    /// <param name="writer"></param>
    public void print( StreamWriter writer ) throws IOException{
        boolean first = true;
        int c = m_list.size();
        for( int i = 0; i < c; i++ ){
            KeyValuePair<Integer, Integer> v = m_list.get( i );
            if ( first ) {
                writer.writeLine( v.key + "=" + v.value );
                first = false;
            } else {
                writer.writeLine( v.key + "=" + v.value );
            }
        }
    }

    /// <summary>
    /// このBPListの内容をテキストファイルに書き出します
    /// </summary>
    /// <param name="writer"></param>
    public void print( TextMemoryStream writer, int start, String header ) {
        boolean first = true;
        int c = m_list.size();
        for( int i = 0; i < c; i++ ){
            KeyValuePair<Integer, Integer> v = m_list.get( i );
            int key = v.key;
            if ( start <= key ) {
                if ( first ) {
                    writer.writeLine( header );
                    first = false;
                }
                writer.writeLine( key + "=" + v.value );
            }
        }
    }

    /// <summary>
    /// テキストファイルからデータ点を読込み、現在のリストに追加します
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public String appendFromText( TextMemoryStream reader ) {
        String last_line = reader.readLine();
        while ( !last_line.startsWith( "[" ) ) {
            String[] spl = last_line.split( "=" );
            int i1 = Integer.parseInt( spl[0] );
            int i2 = Integer.parseInt( spl[1] );
            m_list.add( new KeyValuePair<Integer, Integer>( i1, i2 ) );
            if ( reader.peek() < 0 ) {
                break;
            } else {
                last_line = reader.readLine();
            }
        }
        return last_line;
    }
}
