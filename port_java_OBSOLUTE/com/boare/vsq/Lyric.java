/*
 * Lyric.java
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

import java.io.*;
import java.text.*;

/**
 * VsqHandleに格納される歌詞の情報を扱うクラス。
 */
public class Lyric implements Cloneable, Serializable {
    /**
     * この歌詞のフレーズ。
     */
    public String phrase;
    private String[] m_phonetic_symbol;
    /**
     * 謎の値。
     */
    public float unknownFloat;
    private int[] m_consonant_adjustment;
    /**
     * この歌詞の発音記号が保護されるモードかどうかを表します。
     */
    public boolean phoneticSymbolProtected;

    public int[] getConsonantAdjustment() {
        return m_consonant_adjustment;
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "phrase" ) ){
            return "Phrase";
        }else if( name.equals( "unknownFloat" ) ){
            return "UnknownFloat";
        }else if( name.equals( "phoneticSymbolProtected" ) ){
            return "PhoneticSymbolProtected";
        }else if( name.equals( "phoneticSymbol" ) ){
            return "PhoneticSymbol";
        }
        return name;
    }

    /**
     * このオブジェクトの簡易コピーを取得します。
     *
     * @returns このインスタンスの簡易コピー
     */
    public Object clone() {
        Lyric result = new Lyric();
        result.phrase = this.phrase;
        result.m_phonetic_symbol = (String[])this.m_phonetic_symbol.clone();
        result.unknownFloat = this.unknownFloat;
        result.m_consonant_adjustment = (int[])this.m_consonant_adjustment.clone();
        result.phoneticSymbolProtected = phoneticSymbolProtected;
        return result;
    }

    /**
     * 歌詞、発音記号を指定したコンストラクタ
     *
     * @param phrase_ 歌詞
     * @param phonetic_symbol 発音記号
     */
    public Lyric( String phrase_, String phonetic_symbol ) {
        phrase = phrase_;
        setPhoneticSymbol( phonetic_symbol );
        unknownFloat = 0.000000f;
    }

    public Lyric() {
    }

    /**
     * この歌詞の発音記号を取得します。
     */
    public String getPhoneticSymbol() {
        String ret = m_phonetic_symbol[0];
        for ( int i = 1; i < m_phonetic_symbol.length; i++ ) {
            ret += " " + m_phonetic_symbol[i];
        }
        return ret;
    }

    /**
     * この歌詞の発音記号を設定します。
     */
    public void setPhoneticSymbol( String value ) {
        String s = value.replace( "  ", " " );
        m_phonetic_symbol = s.split( " ", 16 );
        for ( int i = 0; i < m_phonetic_symbol.length; i++ ) {
            m_phonetic_symbol[i] = m_phonetic_symbol[i].replace( "\\\\", "\\" );
        }
        m_consonant_adjustment = new int[m_phonetic_symbol.length];
        for ( int i = 0; i < m_phonetic_symbol.length; i++ ) {
            if ( VsqPhoneticSymbol.isConsonant( m_phonetic_symbol[i] ) ) {
                m_consonant_adjustment[i] = 64;
            } else {
                m_consonant_adjustment[i] = 0;
            }
        }
    }

    public String[] getPhoneticSymbolList() {
        String[] ret = new String[m_phonetic_symbol.length];
        for ( int i = 0; i < m_phonetic_symbol.length; i++ ) {
            ret[i] = m_phonetic_symbol[i];
        }
        return ret;
    }

    /**
     * 文字列からのコンストラクタ
     *
     * @param _line 生成元の文字列
     */
    public Lyric( String _line ){
        byte[] b = new byte[_line.length()];
        int c = _line.length();
        for ( int i = 0; i < c; i++ ) {
            b[i] = (byte)_line.charAt( i );
        }
        String s;
        try{
            s = new String( b, "SJIS" );
        }catch( UnsupportedEncodingException ex ){
            s = "";
        }
        String[] spl = s.split( "," );
        int c_length = spl.length - 3;
        if ( spl.length < 4 ) {
            phrase = "a";
            setPhoneticSymbol( "a" );
            unknownFloat = 0.0f;
            phoneticSymbolProtected = false;
        } else {
            phrase = spl[0];
            if ( phrase.startsWith( "\"" ) ) {
                phrase = phrase.substring( 1 );
            }
            if ( phrase.endsWith( "\"" ) ) {
                phrase = phrase.substring( 0, phrase.length() - 1 );
            }
            String symbols = spl[1];
            if ( symbols.startsWith( "\"" ) ) {
                symbols = symbols.substring( 1 );
            }
            if ( symbols.endsWith( "\"" ) ) {
                symbols = symbols.substring( 0, symbols.length() - 1 );
            }
            setPhoneticSymbol( symbols );
            unknownFloat = Float.parseFloat( spl[2] );
            phoneticSymbolProtected = (spl[spl.length - 1].equals( "0" )) ? false : true;
        }
    }

    /**
     * 与えられた文字列の中の2バイト文字を\x**の形式にエンコードします。
     *
     * @param item エンコード対象
     * @returns エンコードした文字列
     */
    private static char[] encode( String item ) throws UnsupportedEncodingException{
        byte[] bytea = item.getBytes( "SJIS" );
        String result = "";
        for ( int i = 0; i < bytea.length; i++ ) {
            if ( isprint( (char)bytea[i] ) ) {
                result += (char)bytea[i];
            } else {
                result += "\\x" + Integer.toHexString( bytea[i] );
            }
        }
        char[] res = result.toCharArray();
        return res;
    }

    /**
     * このインスタンスを文字列に変換します
     * 
     * @param a_encode 2バイト文字をエンコードするか否かを指定するフラグ
     * @return 変換後の文字列
     */
    public String toString( boolean a_encode ){
        String result;
        DecimalFormat df = new DecimalFormat( "0.000000" );
        if ( a_encode ) {
            String njp;
            try{
                njp = new String( encode( this.phrase ) );
            }catch( UnsupportedEncodingException ex ){
                njp = "a";
            }
            result = "\"" + njp + "\",\"" + this.getPhoneticSymbol() + "\"," + df.format( unknownFloat );
        } else {
            result = "\"";
            byte[] dat;
            try{
                dat = this.phrase.getBytes( "SJIS" );
            }catch( UnsupportedEncodingException ex ){
                dat = new byte[0];
            }
            for ( int i = 0; i < dat.length; i++ ) {
                result += (char)dat[i];
            }
            result += "\",\"" + this.getPhoneticSymbol() + "\"," + df.format( unknownFloat );
            result = result.replace( "\\\\", "\\" );
        }
        for ( int i = 0; i < m_consonant_adjustment.length; i++ ) {
            result += "," + m_consonant_adjustment[i];
        }
        if ( phoneticSymbolProtected ) {
            result += ",1";
        } else {
            result += ",0";
        }
        return result;
    }

    /**
     * 文字がプリント出力可能かどうかを判定します
     *
     * @param ch
     */
    private static boolean isprint( char ch ) {
        if ( 32 <= (int)ch && (int)ch <= 126 ) {
            return true;
        } else {
            return false;
        }
    }
}
