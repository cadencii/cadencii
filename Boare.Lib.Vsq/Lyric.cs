/*
* VsqMetaText/Lyric.cs
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
using System.Windows.Forms;

using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Lib.Vsq {

    using boolean = System.Boolean;

    /// <summary>
    /// VsqHandleに格納される歌詞の情報を扱うクラス。
    /// </summary>
    [Serializable]
    public class Lyric {
        /// <summary>
        /// この歌詞のフレーズ
        /// </summary>
        public String Phrase;
        private String[] m_phonetic_symbol;
        public float UnknownFloat;
        private int[] m_consonant_adjustment;
        public boolean PhoneticSymbolProtected;

        public int[] getConsonantAdjustment() {
            return m_consonant_adjustment;
        }

        /// <summary>
        /// このオブジェクトの簡易コピーを取得します。
        /// </summary>
        /// <returns>このインスタンスの簡易コピー</returns>
        public Lyric Clone() {
            Lyric result = new Lyric();
            result.Phrase = this.Phrase;
            result.m_phonetic_symbol = (String[])this.m_phonetic_symbol.Clone();
            result.UnknownFloat = this.UnknownFloat;
            result.m_consonant_adjustment = (int[])this.m_consonant_adjustment.Clone();
            result.PhoneticSymbolProtected = PhoneticSymbolProtected;
            return result;
        }

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
            String ret = m_phonetic_symbol[0];
            for ( int i = 1; i < m_phonetic_symbol.Length; i++ ) {
                ret += " " + m_phonetic_symbol[i];
            }
            return ret;
        }

        /// <summary>
        /// この歌詞の発音記号を設定します。
        /// </summary>
        public void setPhoneticSymbol( String value ) {
            String s = value.Replace( "  ", " " );
            m_phonetic_symbol = PortUtil.splitString( s, new char[]{ ' ' }, 16 );
            for ( int i = 0; i < m_phonetic_symbol.Length; i++ ) {
                m_phonetic_symbol[i] = m_phonetic_symbol[i].Replace( @"\\", @"\" );
            }
            m_consonant_adjustment = new int[m_phonetic_symbol.Length];
            for ( int i = 0; i < m_phonetic_symbol.Length; i++ ) {
                if ( VsqPhoneticSymbol.isConsonant( m_phonetic_symbol[i] ) ) {
                    m_consonant_adjustment[i] = 64;
                } else {
                    m_consonant_adjustment[i] = 0;
                }
            }
        }

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
                    Phrase = Phrase.Substring( 0, Phrase.Length - 1 );
                }
                String symbols = spl[1];
                if ( symbols.StartsWith( "\"" ) ) {
                    symbols = symbols.Substring( 1 );
                }
                if ( symbols.EndsWith( "\"" ) ) {
                    symbols = symbols.Substring( 0, symbols.Length - 1 );
                }
                setPhoneticSymbol( symbols );
                UnknownFloat = PortUtil.parseFloat( spl[2] );
                PhoneticSymbolProtected = (spl[spl.Length - 1].Equals( "0" )) ? false : true;
            }
        }

        /// <summary>
        /// 与えられた文字列の中の2バイト文字を\x**の形式にエンコードします。
        /// </summary>
        /// <param name="item">エンコード対象</param>
        /// <returns>エンコードした文字列</returns>
        public static char[] encode( String item ) {
            //Encoding sjis = Encoding.GetEncoding( 932 );
            byte[] bytea = cp932.convert( item );//            sjis.GetBytes( item );
            String result = "";
            for ( int i = 0; i < bytea.Length; i++ ) {
                if ( isprint( (char)bytea[i] ) ) {
                    result += (char)bytea[i];
                } else {
                    result += "\\x" + Convert.ToString( bytea[i], 16 );
                }
            }
            char[] res = result.ToCharArray();
            return res;
        }

        /// <summary>
        /// このインスタンスを文字列に変換します
        /// </summary>
        /// <param name="a_encode">2バイト文字をエンコードするか否かを指定するフラグ</param>
        /// <returns>変換後の文字列</returns>
        public String ToString( boolean a_encode ) {
            String result;
            if ( a_encode ) {
                String njp = new String( encode( this.Phrase ) );
                result = "\"" + njp + "\",\"" + this.getPhoneticSymbol() + "\"," + UnknownFloat.ToString( "0.000000" );
            } else {
                result = "\"";
                result += this.Phrase;
                result += "\",\"" + this.getPhoneticSymbol() + "\"," + UnknownFloat.ToString( "0.000000" );
                result = result.Replace( @"\\", @"\" );
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

        /// <summary>
        /// 文字がプリント出力可能かどうかを判定します
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private static boolean isprint( char ch ) {
            if ( 32 <= (int)ch && (int)ch <= 126 ) {
                return true;
            } else {
                return false;
            }
        }
    }

}
