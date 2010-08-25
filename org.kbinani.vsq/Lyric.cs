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
    public class Lyric implements Serializable {
#else
    [Serializable]
    public class Lyric {
#endif
        /// <summary>
        /// この歌詞のフレーズ
        /// </summary>
        public String Phrase;
        private String[] m_phonetic_symbol;
        public float UnknownFloat = 1.0f;
        private int[] m_consonant_adjustment;
        public boolean PhoneticSymbolProtected;

        /// <summary>
        /// このクラスの指定した名前のプロパティをXMLシリアライズする際に使用する
        /// 要素名を取得します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String getXmlElementName( String name ) {
            return name;
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティを，XMLシリアライズ時に無視するかどうかを表す
        /// ブール値を返します．デフォルトの実装では戻り値は全てfalseです．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static boolean isXmlIgnored( String name ) {
            if ( name == null ) {
                return true;
            }
            if ( name.Equals( "ConsonantAdjustmentList" ) ) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティが総称型引数を用いる型である場合に，
        /// その型の限定名を返します．それ以外の場合は空文字を返します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String getGenericTypeName( String name ) {
            return "";
        }

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
            UnknownFloat = 1.0f;
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

            // 古い発音記号を保持しておく
            String[] old_symbol = null;
            if ( m_phonetic_symbol != null ) {
                old_symbol = new String[m_phonetic_symbol.Length];
                for ( int i = 0; i < m_phonetic_symbol.Length; i++ ) {
                    old_symbol[i] = m_phonetic_symbol[i];
                }
            }

            // 古いconsonant adjustmentを保持しておく
            int[] old_adjustment = null;
            if ( m_consonant_adjustment != null ) {
                old_adjustment = new int[m_consonant_adjustment.Length];
                for ( int i = 0; i < m_consonant_adjustment.Length; i++ ) {
                    old_adjustment[i] = m_consonant_adjustment[i];
                }
            }

            m_phonetic_symbol = PortUtil.splitString( s, new char[] { ' ' }, 16, true );
            for ( int i = 0; i < m_phonetic_symbol.Length; i++ ) {
                m_phonetic_symbol[i] = m_phonetic_symbol[i].Replace( "\\" + "\\", "\\" );
            }

            // consonant adjustmentを更新
            if( m_consonant_adjustment == null ||
                (m_consonant_adjustment != null && m_consonant_adjustment.Length != m_phonetic_symbol.Length) ){
                m_consonant_adjustment = new int[m_phonetic_symbol.Length];
            }

            // 古い発音記号と同じなら、古い値を使う
            for ( int i = 0; i < m_phonetic_symbol.Length; i++ ) {
                boolean use_old_value = (old_symbol != null && i < old_symbol.Length) &&
                                        (m_phonetic_symbol[i].Equals( old_symbol[i])) &&
                                        (old_adjustment != null && i < old_adjustment.Length);
                if( use_old_value ){
                    m_consonant_adjustment[i] = VsqPhoneticSymbol.isConsonant( m_phonetic_symbol[i] ) ? old_adjustment[i] : 0;
                } else {
                    m_consonant_adjustment[i] = VsqPhoneticSymbol.isConsonant( m_phonetic_symbol[i] ) ? 64 : 0;
                }
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
            if ( this.m_phonetic_symbol == null ) {
                this.m_phonetic_symbol = new String[0];
            }
            String[] ret = new String[m_phonetic_symbol.Length];
            for ( int i = 0; i < m_phonetic_symbol.Length; i++ ) {
                ret[i] = m_phonetic_symbol[i];
            }
            return ret;
        }

        /// <summary>
        /// 文字列(ex."a","a",0.0000,0.0)からのコンストラクタ
        /// </summary>
        /// <param name="line"></param>
        public Lyric( String line ) {
            int len = PortUtil.getStringLength( line );
            if ( len == 0 ) {
                Phrase = "a";
                setPhoneticSymbol( "a" );
                UnknownFloat = 1.0f;
                PhoneticSymbolProtected = false;
                setConsonantAdjustment( "0" );
                return;
            }
            int indx = -1;
            int dquote_count = 0;
            String work = "";
            String consonant_adjustment = "";
            for ( int i = 0; i < len; i++ ) {
#if JAVA
                char c = line.charAt( i );
#else
                char c = line[i];
#endif
                if ( c == ',' ) {
                    if ( dquote_count % 2 == 0 ) {
                        // ,の左側に偶数個の"がある場合→,は区切り文字
                        indx++;
                        if ( indx == 0 ) {
                            // Phrase
                            work = work.Replace( "\"\"", "\"" );  // "は""として保存される
                            if ( work.StartsWith( "\"" ) && work.EndsWith( "\"" ) ) {
                                int l = PortUtil.getStringLength( work );
                                if ( l > 2 ) {
                                    Phrase = work.Substring( 1, l - 2 );
                                } else {
                                    Phrase = "a";
                                }
                            } else {
                                Phrase = work;
                            }
                            work = "";
                        } else if ( indx == 1 ) {
                            // symbols
                            String symbols = "";
                            if ( work.StartsWith( "\"" ) && work.EndsWith( "\"" ) ) {
                                int l = PortUtil.getStringLength( work );
                                if ( l > 2 ) {
                                    symbols = work.Substring( 1, l - 2 );
                                } else {
                                    symbols = "a";
                                }
                            } else {
                                symbols = work;
                            }
                            setPhoneticSymbol( symbols );
                            work = "";
                        } else if ( indx == 2 ) {
                            // UnknownFloat
                            UnknownFloat = PortUtil.parseFloat( work );
                            work = "";
                        } else {
                            if ( indx - 3 < m_phonetic_symbol.Length ) {
                                // consonant adjustment
                                if ( indx - 3 == 0 ) {
                                    consonant_adjustment += work;
                                } else {
                                    consonant_adjustment += "," + work;
                                }
                            } else {
                                // protected
                                PhoneticSymbolProtected = work.Equals( "1" );
                            }
                            work = "";
                        }
                    } else {
                        // ,の左側に奇数個の"がある場合→,は歌詞等の一部
                        work += "" + c;
                    }
                } else {
                    work += "" + c;
                    if ( c == '"' ) {
                        dquote_count++;
                    }
                }
            }
            setConsonantAdjustment( consonant_adjustment );
        }

        /// <summary>
        /// このインスタンスを文字列に変換します
        /// </summary>
        /// <param name="add_quatation_mark">クォーテーションマークを付けるかどうか</param>
        /// <returns>変換後の文字列</returns>
        public String toString( boolean add_quatation_mark ) {
            String quot = (add_quatation_mark ? "\"" : "");
            String result;
            String phrase = (this.Phrase == null) ? "" : this.Phrase.Replace( "\"", "\"\"" );
            result = quot + phrase + quot + ",";
            String[] symbol = getPhoneticSymbolList();
            String strSymbol = getPhoneticSymbol();
            if( !add_quatation_mark ){
                if( strSymbol == null || (strSymbol != null && strSymbol.Equals( "" ) ) ){
                    strSymbol = "u:";
                }
            }
            result += quot + strSymbol + quot + "," + PortUtil.formatDecimal( "0.000000", UnknownFloat );
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
            return toString( true );
        }
#endif
    }

#if !JAVA
}
#endif
