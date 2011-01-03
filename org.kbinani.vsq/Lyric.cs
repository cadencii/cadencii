/*
 * Lyric.cs
 * Copyright © 2008-2011 kbinani
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
#elif __cplusplus
#else
using System;
using System.Collections.Generic;
#endif

#if !JAVA
namespace org {
    namespace kbinani {
        namespace vsq {
#endif

            /// <summary>
            /// VsqHandleに格納される歌詞の情報を扱うクラス。
            /// </summary>
#if JAVA
            public class Lyric implements Serializable {
#elif __cplusplus
            class Lyric {
#else
            [Serializable]
            public class Lyric {
#endif
                /// <summary>
                /// この歌詞のフレーズ
                /// </summary>
                public string Phrase;
                private List<string> m_phonetic_symbol;
                public float UnknownFloat = 1.0f;
                private List<int> m_consonant_adjustment;
                public bool PhoneticSymbolProtected;

                /// <summary>
                /// このクラスの指定した名前のプロパティをXMLシリアライズする際に使用する
                /// 要素名を取得します．
                /// </summary>
                /// <param name="name"></param>
                /// <returns></returns>
                public static string getXmlElementName( string name ) {
                    return name;
                }

                /// <summary>
                /// このクラスの指定した名前のプロパティを，XMLシリアライズ時に無視するかどうかを表す
                /// ブール値を返します．デフォルトの実装では戻り値は全てfalseです．
                /// </summary>
                /// <param name="name"></param>
                /// <returns></returns>
                public static bool isXmlIgnored( string name ) {
                    return VsqUtility.compare( name, "ConsonantAdjustmentList" );
                }

                /// <summary>
                /// このクラスの指定した名前のプロパティが総称型引数を用いる型である場合に，
                /// その型の限定名を返します．それ以外の場合は空文字を返します．
                /// </summary>
                /// <param name="name"></param>
                /// <returns></returns>
                public static string getGenericTypeName( string name ) {
                    return "";
                }

                /// <summary>
                /// このオブジェクトのインスタンスと、指定されたアイテムが同じかどうかを調べます。
                /// 音声合成したときに影響のある範囲のフィールドしか比較されません。
                /// たとえば、PhoneticSymbolProtectedがthisとitemで違っていても、他が同一であればtrueが返る。
                /// </summary>
                /// <param name="item"></param>
                /// <returns></returns>
                public bool equalsForSynth( Lyric item ) {
                    if ( this.PhoneticSymbolProtected != item.PhoneticSymbolProtected ) return false;
                    if ( !VsqUtility.compare( this.getPhoneticSymbol(), item.getPhoneticSymbol() ) ) return false;
                    if ( !VsqUtility.compare( this.getConsonantAdjustment(), item.getConsonantAdjustment() ) ) return false;
                    return true;
                }

                /// <summary>
                /// このオブジェクトのインスタンスと、指定されたオブジェクトが同じかどうかを調べます。
                /// </summary>
                /// <param name="item"></param>
                /// <returns></returns>
                public bool equals( Lyric item ) {
                    if ( !equalsForSynth( item ) ) return false;
                    if ( !VsqUtility.compare( this.Phrase, item.Phrase ) ) return false;
                    if ( this.UnknownFloat != item.UnknownFloat ) return false;
                    return true;
                }

                /// <summary>
                /// Consonant Adjustmentの文字列形式を取得します。
                /// </summary>
                /// <returns></returns>
                public string getConsonantAdjustment() {
                    string ret = "";
                    List<int> arr = getConsonantAdjustmentList();
                    int size = arr.Count;

                    for ( int i = 0; i < size; i++ ) {
#if JAVA
                        int v = arr.get( i );
#else
                        int v = arr[i];
#endif
                        ret += (i == 0 ? "" : " ") + v;
                    }
                    return ret;
                }

                /// <summary>
                /// Consonant Adjustmentを文字列形式で設定します。
                /// </summary>
                /// <param name="value"></param>
                public void setConsonantAdjustment( string value ) {
#if __cplusplus
                    vector<string> tokenizer;
                    tokenizer.push_back( " " );
                    tokenizer.push_back( "," );
                    vector<string> spl = PortUtil.splitString( value, tokenizer, true );
                    int size = spl.size();
#else
                    string[] spl = PortUtil.splitString( value, new char[] { ' ', ',' }, true );
#if JAVA
                    int size = spl.length;
#else
                    int size = spl.Length;
#endif
#endif
                    List<int> arr = new List<int>();
                    for ( int i = 0; i < spl.Length; i++ ) {
                        int v = 64;
                        try {
                            v = PortUtil.parseInt( spl[i] );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "Lyric#setCosonantAdjustment; ex=" + ex );
                        }
                        arr.Add( v );
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
                public List<int> getConsonantAdjustmentList() {
#if !__cplusplus
                    if ( m_consonant_adjustment != null ) {
                        return m_consonant_adjustment;
                    }
                    if ( m_phonetic_symbol == null ) {
                        m_consonant_adjustment = new List<int>();
                        return m_consonant_adjustment;
                    }
#endif

                    m_consonant_adjustment.Clear();
                    for ( int i = 0; i < m_phonetic_symbol.Count; i++ ) {
                        int v = VsqPhoneticSymbol.isConsonant( m_phonetic_symbol[i] ) ? 64 : 0;
#if JAVA
                        m_consonant_adjustment.add( v );
#elif __cplusplus
                        m_consonant_adjustment.push_back( v );
#else
                        m_consonant_adjustment.Add( v );
#endif
                    }
                    return m_consonant_adjustment;
                }

                /// <summary>
                /// Consonant Adjustmentを、整数配列形式で設定します。
                /// </summary>
                /// <param name="value"></param>
                public void setConsonantAdjustmentList( List<int> value ) {
#if !__cplusplus
                    if ( value == null ) {
                        return;
                    }
#endif
                    m_consonant_adjustment.Clear();
                    for ( int i = 0; i < value.Count; i++ ) {
#if JAVA
                        int v = value.get( i );
#else
                        int v = value[i];
#endif
#if JAVA
                        m_consonant_adjustment.add( v );
#elif __cplusplus
                        m_consonant_adjustment.push_back( v );
#else
                        m_consonant_adjustment.Add( v );
#endif
                    }
                }

#if !__cplusplus
                /// <summary>
                /// このオブジェクトの簡易コピーを取得します。
                /// </summary>
                /// <returns>このインスタンスの簡易コピー</returns>
                public Object clone() {
                    Lyric result = new Lyric();
                    result.Phrase = this.Phrase;
                    result.m_phonetic_symbol = new List<string>();
                    for ( int i = 0; i < m_phonetic_symbol.Count; i++ ) {
                        result.m_phonetic_symbol.Add( m_phonetic_symbol[i] );
                    }
                    result.UnknownFloat = this.UnknownFloat;
                    if ( m_consonant_adjustment != null ) {
                        result.m_consonant_adjustment = new List<int>();
                        for ( int i = 0; i < m_consonant_adjustment.Count; i++ ) {
                            result.m_consonant_adjustment.Add( m_consonant_adjustment[i] );
                        }
                    }
                    result.PhoneticSymbolProtected = PhoneticSymbolProtected;
                    return result;
                }
#endif

#if JAVA
#elif __cplusplus
#else
                public Object Clone() {
                    return clone();
                }
#endif

                /// <summary>
                /// 歌詞、発音記号を指定したコンストラクタ
                /// </summary>
                /// <param name="phrase">歌詞</param>
                /// <param name="phonetic_symbol">発音記号</param>
                public Lyric( string phrase, string phonetic_symbol ) {
                    Phrase = phrase;
                    setPhoneticSymbol( phonetic_symbol );
                    UnknownFloat = 1.0f;
                }

                public Lyric() {
                    this.m_consonant_adjustment = new List<int>();
                    this.m_phonetic_symbol = new List<string>();
                }

                /// <summary>
                /// この歌詞の発音記号を取得します。
                /// </summary>
                public string getPhoneticSymbol() {
                    List<string> symbol = getPhoneticSymbolList();
                    string ret = "";
                    for ( int i = 0; i < symbol.Count; i++ ) {
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
                        old_symbol = new String[m_phonetic_symbol.Count];
                        for ( int i = 0; i < m_phonetic_symbol.Count; i++ ) {
                            old_symbol[i] = m_phonetic_symbol[i];
                        }
                    }

                    // 古いconsonant adjustmentを保持しておく
                    int[] old_adjustment = null;
                    if ( m_consonant_adjustment != null ) {
                        old_adjustment = new int[m_consonant_adjustment.Count];
                        for ( int i = 0; i < m_consonant_adjustment.Count; i++ ) {
                            old_adjustment[i] = m_consonant_adjustment[i];
                        }
                    }

#if __cplusplus
                    vector<string> spl = PortUtil.splitString( s, new char[] { ' ' }, 16, true );
                    int size = spl.size();
#else
                    String[] spl = PortUtil.splitString( s, new char[] { ' ' }, 16, true );
                    if ( m_phonetic_symbol == null ) {
                        m_phonetic_symbol = new List<string>();
                    }
                    int size = spl.Length;
#endif
                    m_phonetic_symbol.Clear();
                    for ( int i = 0; i < size; i++ ) {
                        m_phonetic_symbol.Add( spl[i] );
                    }
                    for ( int i = 0; i < m_phonetic_symbol.Count; i++ ) {
                        m_phonetic_symbol[i] = m_phonetic_symbol[i].Replace( "\\" + "\\", "\\" );
                    }

                    // consonant adjustmentを更新
                    if ( m_consonant_adjustment == null ||
                        (m_consonant_adjustment != null && m_consonant_adjustment.Count != m_phonetic_symbol.Count) ) {
                        m_consonant_adjustment = new List<int>();
                        for ( int i = 0; i < m_phonetic_symbol.Count; i++ ) {
                            m_consonant_adjustment.Add( 0 );
                        }
                    }

                    // 古い発音記号と同じなら、古い値を使う
                    for ( int i = 0; i < m_phonetic_symbol.Count; i++ ) {
                        bool use_old_value = (old_symbol != null && i < old_symbol.Length) &&
                                                (m_phonetic_symbol[i].Equals( old_symbol[i] )) &&
                                                (old_adjustment != null && i < old_adjustment.Length);
                        if ( use_old_value ) {
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

                public List<string> getPhoneticSymbolList() {
#if !__cplusplus
                    if ( m_phonetic_symbol == null ) {
                        m_phonetic_symbol = new List<string>();
                    }
#endif
                    return m_phonetic_symbol;
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
                                    if ( indx - 3 < m_phonetic_symbol.Count ) {
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
                public String toString( bool add_quatation_mark ) {
                    String quot = (add_quatation_mark ? "\"" : "");
                    String result;
                    String phrase = (this.Phrase == null) ? "" : this.Phrase.Replace( "\"", "\"\"" );
                    result = quot + phrase + quot + ",";
                    List<string> symbol = getPhoneticSymbolList();
                    String strSymbol = getPhoneticSymbol();
                    if ( !add_quatation_mark ) {
                        if ( strSymbol == null || (strSymbol != null && strSymbol.Equals( "" )) ) {
                            strSymbol = "u:";
                        }
                    }
                    result += quot + strSymbol + quot + "," + PortUtil.formatDecimal( "0.000000", UnknownFloat );
                    result = result.Replace( "\\" + "\\", "\\" );
                    if ( m_consonant_adjustment == null ) {
                        m_consonant_adjustment = new List<int>();
                        for ( int i = 0; i < symbol.Count; i++ ) {
                            m_consonant_adjustment.Add( VsqPhoneticSymbol.isConsonant( symbol[i] ) ? 64 : 0 );
                        }
                    }
                    for ( int i = 0; i < m_consonant_adjustment.Count; i++ ) {
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
    }
}
#endif
