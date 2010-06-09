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
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.vsq == undefined ) org.kbinani.vsq = {};
if( org.kbinani.vsq.Lyric == undefined ){

    /**
     * 歌詞、発音記号を指定したコンストラクタ
     * @param phrase [String] 歌詞
     * @param phonetic_symbol [String] 発音記号
     */
    org.kbinani.vsq.Lyric = function( phrase, phonetic_symbol ){
        this.Phrase = phrase;
        this.setPhoneticSymbol( phonetic_symbol );
        this.UnknownFloat = 0.000000f;
        this.m_consonant_adjustment = new Array();
        this.PhoneticSymbolProtected = false;
    };


    org.kbinani.vsq.Lyric.prototype = {
        /**
         * このオブジェクトのインスタンスと、指定されたアイテムが同じかどうかを調べます。
         * 音声合成したときに影響のある範囲のフィールドしか比較されません。
         * たとえば、PhoneticSymbolProtectedがthisとitemで違っていても、他が同一であればtrueが返る。
         *
         * @param item [Lyric]
         * @return [bool]
         */
        equalsForSynth : function( item ) {
            if ( this.PhoneticSymbolProtected != item.PhoneticSymbolProtected ) return false;
            if ( this.getPhoneticSymbol() != item.getPhoneticSymbol() ) return false;
            if ( this.getConsonantAdjustment() != item.getConsonantAdjustment() ) return false;
            return true;
        },

        /**
         * このオブジェクトのインスタンスと、指定されたオブジェクトが同じかどうかを調べます。
         *
         * @param item [Lyric]
         * @return [bool]
         */
        equals : function( item ) {
            if ( !this.equalsForSynth( item ) ) return false;
            if ( this.Phrase != item.Phrase ) return false;
            if ( this.UnknownFloat != item.UnknownFloat ) return false;
            return true;
        },

        /**
         * Consonant Adjustmentの文字列形式を取得します。
         *
         * @return [String]
         */
        getConsonantAdjustment : function() {
            var ret = "";
            var arr = getConsonantAdjustmentList();
            for ( var i = 0; i < arr.length; i++ ) {
                ret += (i == 0 ? "" : " ") + arr[i];
            }
            return ret;
        },

        /**
         * Consonant Adjustmentを文字列形式で設定します。
         *
         * @param value [String]
         * @return [void]
         */
        setConsonantAdjustment : function( value ) {
            var spl = PortUtil.splitString( value, new char[] { ' ', ',' }, true );
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

        public String[] getPhoneticSymbolList() {
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
                UnknownFloat = 0.0f;
                PhoneticSymbolProtected = false;
                setConsonantAdjustment( "0" );
                return;
            }
            int indx = -1;
            int dquote_count = 0;
            String work = "";
            String consonant_adjustment = "";
            for ( int i = 0; i < len; i++ ) {
                char c = line.charAt( i );
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
            result = quot + this.Phrase + quot + ",";
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
    }

}
