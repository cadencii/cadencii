/*
 * Lyric.cs
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

import java.io.*;
import org.kbinani.*;
#else
using System;
using org.kbinani;

namespace org.kbinani.vsq {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// VsqHandleに格納される歌詞の情報を扱うクラス。
    /// </summary>
#if JAVA
    public class Lyric implements Serializable{
#else
    [Serializable]
    public class Lyric {
#endif
        /// <summary>
        /// この歌詞のフレーズ
        /// </summary>
        public String Phrase;
        private String[] m_phonetic_symbol;
        public float UnknownFloat;
        private int[] m_consonant_adjustment;
        public boolean PhoneticSymbolProtected;

        /// <summary>
        /// このオブジェクトのインスタンスと、指定されたアイテムが同じかどうかを調べます。
        /// 音声合成したときに影響のある範囲のフィールドしか比較されません。
        /// たとえば、PhoneticSymbolProtectedがthisとitemで違っていても、他が同一であればtrueが返る。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public boolean equalsForSynth( Lyric item ) {
            if ( this.PhoneticSymbolProtected != item.PhoneticSymbolProtected ) return false;
            if ( !this.getPhoneticSymbol().Equals( item.getPhoneticSymbol() ) ) return false;
            if ( !this.getConsonantAdjustment().Equals( item.getConsonantAdjustment() ) ) return false;
            return true;
        }

        /// <summary>
        /// このオブジェクトのインスタンスと、指定されたオブジェクトが同じかどうかを調べます。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public boolean equals( Lyric item ) {
            if ( !equalsForSynth( item ) ) return false;
            if ( !this.Phrase.Equals( item.Phrase ) ) return false;
            if ( this.UnknownFloat != item.UnknownFloat ) return false;
            return true;
        }

        /// <summary>
        /// Consonant Adjustmentの文字列形式を取得します。
        /// </summary>
        /// <returns></returns>
        public String getConsonantAdjustment() {
            String ret = "";
            int[] arr = getConsonantAdjustmentList();
            for ( int i = 0; i < arr.Length; i++ ) {
                ret += (i == 0 ? "" : " ") + arr[i];
            }
            return ret;
        }

        /// <summary>
        /// Consonant Adjustmentを文字列形式で設定します。
        /// </summary>
        /// <param name="value"></param>
        public void setConsonantAdjustment( String value ) {
            String[] spl = PortUtil.splitString( value, new char[] { ' ', ',' }, true );
            int[] arr = new int[spl.Length];
            for ( int i = 0; i < spl.Length; i++ ) {
                int v = 64;
                try {
                    v = PortUtil.parseInt( spl[i] );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "Lyric#setCosonantAdjustment; ex=" + ex );
                }
                arr[i] = v;
            }
            setConsonantAdjustmentList( arr );
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public String ConsonantAdjustment {
            get {
                return getConsonantAdjustment();
            }
            set {
                setConsonantAdjustment( value );
            }
        }
#endif

        /// <summary>
        /// Consonant Adjustmentを、整数配列で取得します。
        /// </summary>
        /// <returns></returns>
        public int[] getConsonantAdjustmentList() {
            if ( m_consonant_adjustment == null ) {
                if ( m_phonetic_symbol == null ) {
                    m_consonant_adjustment = new int[] { };
                } else {
                    m_consonant_adjustment = new int[m_phonetic_symbol.Length];
                    for ( int i = 0; i < m_phonetic_symbol.Length; i++ ) {
                        m_consonant_adjustment[i] = VsqPhoneticSymbol.isConsonant( m_phonetic_symbol[i] ) ? 64 : 0;
                    }
                }
            }
            return m_consonant_adjustment;
        }

        /// <summary>
        /// Consonant Adjustmentを、整数配列形式で設定します。
        /// </summary>
        /// <param name="value"></param>
        public void setConsonantAdjustmentList( int[] value ) {
            if ( value == null ) {
                return;
            }
            m_consonant_adjustment = new int[value.Length];
            for ( int i = 0; i < value.Length; i++ ) {
                m_consonant_adjustment[i] = value[i];
            }
        }

        /// <summary>
        /// このオブジェクトの簡易コピーを取得します。
        /// </summary>
        /// <returns>このインスタンスの簡易コピー</returns>
        public Object clone() {
            Lyric result = new Lyric();
            result.Phrase = this.Phrase;
            result.m_phonetic_symbol = new String[m_phonetic_symbol.Length];
            for ( int i = 0; i < m_phonetic_symbol.Length; i++ ) {
                result.m_phonetic_symbol[i] = m_phonetic_symbol[i];
            }
            result.UnknownFloat = this.UnknownFloat;
            if ( m_consonant_adjustment != null ) {
                result.m_consonant_adjustment = new int[m_consonant_adjustment.Length];
                for ( int i = 0; i < m_consonant_adjustment.Length; i++ ) {
                    result.m_consonant_adjustment[i] = m_consonant_adjustment[i];
                }
            }
            result.PhoneticSymbolProtected = PhoneticSymbolProtected;
            return result;
        }

#if !JAVA
        public Object Clone() {
            return clone();
        }
#endif

        /// <summary>
        /// 歌詞、発音記号を指定したコンストラクタ
        /// </summary>
        /// <param name="phrase">歌詞</param>
        /// <param name="phonetic_symbol">発音記号</param>
        public Lyric( String phrase, String phonetic_symbol ) {
            Phrase = phrase;
            setPhoneticSymbol( phonetic_symbol );
            UnknownFloat = 0.000000f;
        }

        public Lyric() {
        }

        /// <summary>
        /// この歌詞の発音記号を取得します。
        /// </summary>
        public String getPhoneticSymbol() {
            String[] symbol = getPhoneticSymbolList();
            String ret = "";
            for ( int i = 0; i < symbol.Length; i++ ) {
                ret += (i == 0 ? "" : " ") + symbol[i];
            }
            return ret;
        }

        /// <summary>
        /// この歌詞の発音記号を設定します。
        /// </summary>
        public void setPhoneticSymbol( String value ) {
            String s = value.Replace( "  ", " " );
            m_phonetic_symbol = PortUtil.splitString( s, new char[] { ' ' }, 16, true );
            for ( int i = 0; i < m_phonetic_symbol.Length; i++ ) {
                m_phonetic_symbol[i] = m_phonetic_symbol[i].Replace( "\\" + "\\", "\\" );
            }
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public String PhoneticSymbol {
            get {
                return getPhoneticSymbol();
            }
            set {
                setPhoneticSymbol( value );
            }
        }
#endif

        public String[] getPhoneticSymbolList() {
            String[] ret = new String[m_phonetic_symbol.Length];
            for ( int i = 0; i < m_phonetic_symbol.Length; i++ ) {
                ret[i] = m_phonetic_symbol[i];
            }
            return ret;
        }

        /// <summary>
        /// 文字列からのコンストラクタ
        /// </summary>
        /// <param name="_line">生成元の文字列</param>
        public Lyric( String _line ) {
            String[] spl = PortUtil.splitString( _line, ',' );
            int c_length = spl.Length - 3;
            if ( spl.Length < 4 ) {
                Phrase = "a";
                setPhoneticSymbol( "a" );
                UnknownFloat = 0.0f;
                PhoneticSymbolProtected = false;
            } else {
                Phrase = spl[0];
                if ( Phrase.StartsWith( "\"" ) ) {
                    Phrase = Phrase.Substring( 1 );
                }
                if ( Phrase.EndsWith( "\"" ) ) {
                    Phrase = Phrase.Substring( 0, PortUtil.getStringLength( Phrase ) - 1 );
                }
                String symbols = spl[1];
                if ( symbols.StartsWith( "\"" ) ) {
                    symbols = symbols.Substring( 1 );
                }
                if ( symbols.EndsWith( "\"" ) ) {
                    symbols = symbols.Substring( 0, PortUtil.getStringLength( symbols ) - 1 );
                }
                setPhoneticSymbol( symbols );
                UnknownFloat = PortUtil.parseFloat( spl[2] );
                PhoneticSymbolProtected = (spl[spl.Length - 1].Equals( "0" )) ? false : true;
            }
        }

        /// <summary>
        /// このインスタンスを文字列に変換します
        /// </summary>
        /// <param name="a_encode">2バイト文字をエンコードするか否かを指定するフラグ</param>
        /// <returns>変換後の文字列</returns>
        public String toString() {
            String result;
            result = "\"";
            result += this.Phrase;
            String[] symbol = getPhoneticSymbolList();
            result += "\",\"" + this.getPhoneticSymbol() + "\"," + PortUtil.formatDecimal( "0.000000", UnknownFloat );
            result = result.Replace( "\\" + "\\", "\\" );
            if ( m_consonant_adjustment == null ) {
                m_consonant_adjustment = new int[symbol.Length];
                for ( int i = 0; i < symbol.Length; i++ ) {
                    m_consonant_adjustment[i] = VsqPhoneticSymbol.isConsonant( symbol[i] ) ? 64 : 0;
                }
            }
            for ( int i = 0; i < m_consonant_adjustment.Length; i++ ) {
                result += "," + m_consonant_adjustment[i];
            }
            if ( PhoneticSymbolProtected ) {
                result += ",1";
            } else {
                result += ",0";
            }
            return result;
        }

#if !JAVA
        public override string ToString() {
            return toString();
        }
#endif
    }

#if !JAVA
}
#endif
