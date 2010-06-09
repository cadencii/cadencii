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
     * overload1
     * 歌詞、発音記号を指定したコンストラクタ
     * @param phrase [String] 歌詞
     * @param phonetic_symbol [String] 発音記号
     *
     * overload2
     * 文字列(ex."a","a",0.0000,0.0)からのコンストラクタ
     */
    org.kbinani.vsq.Lyric = function(){
        if( arguments.length == 1 ){
            var line = arguments[0];
            var len = line.length;
            if ( len == 0 ) {
                this.Phrase = "a";
                setPhoneticSymbol( "a" );
                this.UnknownFloat = 0.0;
                this.PhoneticSymbolProtected = false;
                setConsonantAdjustment( "0" );
                return;
            }
            var indx = -1;
            var dquote_count = 0;
            var work = "";
            var consonant_adjustment = "";
            for ( var i = 0; i < len; i++ ) {
                var c = line.charAt( i );
                if ( c == ',' ) {
                    if ( dquote_count % 2 == 0 ) {
                        // ,の左側に偶数個の"がある場合→,は区切り文字
                        indx++;
                        if ( indx == 0 ) {
                            // Phrase
                            work = work.replace( "\"\"", "\"" );  // "は""として保存される
                            if ( work.indexOf( "\"" ) === 0 && work.lastIndexOf( "\"" ) === work.length - "\"".length ) {
                                var l = work.length;
                                if ( l > 2 ) {
                                    this.Phrase = work.substring( 1, l - 2 );
                                } else {
                                    this.Phrase = "a";
                                }
                            } else {
                                Phrase = work;
                            }
                            work = "";
                        } else if ( indx == 1 ) {
                            // symbols
                            var symbols = "";
                            if ( work.indexOf( "\"" ) === 0 && work.lastIndexOf( "\"" ) === work.length - "\"".length ) {
                                var l = work.length;
                                if ( l > 2 ) {
                                    symbols = work.substring( 1, l - 2 );
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
                            this.UnknownFloat = parseFloat( work );
                            work = "";
                        } else {
                            if ( indx - 3 < this.m_phonetic_symbol.length ) {
                                // consonant adjustment
                                if ( indx - 3 == 0 ) {
                                    consonant_adjustment += work;
                                } else {
                                    consonant_adjustment += "," + work;
                                }
                            } else {
                                // protected
                                this.PhoneticSymbolProtected = work == "1";
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
        }else if( arguments.length == 2 ){
            var phrase = arguments[0];
            var phonetic_symbol = arguments[1];
            this.Phrase = phrase;
            this.setPhoneticSymbol( phonetic_symbol );
            this.UnknownFloat = 0.000000;
            this.m_consonant_adjustment = new Array();
            this.PhoneticSymbolProtected = false;
        }
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
            var spl = org.kbinani.PortUtil.splitString( value, new Array( " ", "," ), -1, true );
            var arr = new Array( spl.length );
            for ( var i = 0; i < spl.length; i++ ) {
                var v = parseInt( spl[i], 10 );
                arr[i] = v;
            }
            setConsonantAdjustmentList( arr );
        },

        /**
         * Consonant Adjustmentを、整数配列で取得します。
         *
         * @return [int[]]
         */
        getConsonantAdjustmentList : function() {
            if ( m_consonant_adjustment == null ) {
                if ( m_phonetic_symbol == null ) {
                    m_consonant_adjustment = new Array();
                } else {
                    m_consonant_adjustment = new Array( m_phonetic_symbol.length );
                    for ( var i = 0; i < m_phonetic_symbol.length; i++ ) {
                        m_consonant_adjustment[i] = org.kbinani.vsq.VsqPhoneticSymbol.isConsonant( m_phonetic_symbol[i] ) ? 64 : 0;
                    }
                }
            }
            return m_consonant_adjustment;
        },

        /**
         * Consonant Adjustmentを、整数配列形式で設定します。
         *
         * @param value [int[]]
         */
        setConsonantAdjustmentList : function( value ) {
            if ( value == null ) {
                return;
            }
            this.m_consonant_adjustment = new Array( value.length );
            for ( var i = 0; i < value.length; i++ ) {
                this.m_consonant_adjustment[i] = value[i];
            }
        },

        /**
         * このオブジェクトの簡易コピーを取得します。
         *
         * @return [object] このインスタンスの簡易コピー
         */
        clone : function() {
            var result = new org.kbinani.vsq.Lyric();
            result.Phrase = this.Phrase;
            result.m_phonetic_symbol = new Array( this.m_phonetic_symbol.length );
            for ( var i = 0; i < this.m_phonetic_symbol.length; i++ ) {
                result.m_phonetic_symbol[i] = this.m_phonetic_symbol[i];
            }
            result.UnknownFloat = this.UnknownFloat;
            if ( this.m_consonant_adjustment != null ) {
                result.m_consonant_adjustment = new Array( this.m_consonant_adjustment.length );
                for ( var i = 0; i < this.m_consonant_adjustment.length; i++ ) {
                    result.m_consonant_adjustment[i] = this.m_consonant_adjustment[i];
                }
            }
            result.PhoneticSymbolProtected = this.PhoneticSymbolProtected;
            return result;
        },

        /**
         * この歌詞の発音記号を取得します。
         *
         * @return [String]
         */
        getPhoneticSymbol : function() {
            var symbol = getPhoneticSymbolList();
            var ret = "";
            for ( var i = 0; i < symbol.length; i++ ) {
                ret += (i == 0 ? "" : " ") + symbol[i];
            }
            return ret;
        },

        /**
         * この歌詞の発音記号を設定します。
         *
         * @param valur [String]
         * @return [void]
         */
        setPhoneticSymbol : function( value ) {
            var s = value.replace( "  ", " " );
            this.m_phonetic_symbol = org.kbinani.vsq.PortUtil.splitString( s, new Array( " " ), 16, true );
            for ( var i = 0; i < this.m_phonetic_symbol.length; i++ ) {
                this.m_phonetic_symbol[i] = this.m_phonetic_symbol[i].replace( "\\" + "\\", "\\" );
            }
        },

        /**
         * @return [String[]]
         */
        getPhoneticSymbolList : function() {
            var ret = new Array( this.m_phonetic_symbol.length );
            for ( var i = 0; i < this.m_phonetic_symbol.length; i++ ) {
                ret[i] = this.m_phonetic_symbol[i];
            }
            return ret;
        },

        /**
         * このインスタンスを文字列に変換します
         *
         * @param add_quatation_mark [bool]
         * @return 変換後の文字列 [String]
         */
        toString : function( add_quatation_mark ) {
            var quot = (add_quatation_mark ? "\"" : "");
            var result;
            result = quot + this.Phrase + quot + ",";
            var symbol = getPhoneticSymbolList();
            var strSymbol = getPhoneticSymbol();
            if( !add_quatation_mark ){
                if( strSymbol == null || (strSymbol != null && strSymbol == "" ) ){
                    strSymbol = "u:";
                }
            }
            result += quot + strSymbol + quot + "," + this.UnknownFloat;
            result = result.replace( "\\" + "\\", "\\" );
            if ( this.m_consonant_adjustment == null ) {
                this.m_consonant_adjustment = new Array( symbol.length );
                for ( var i = 0; i < symbol.length; i++ ) {
                    this.m_consonant_adjustment[i] = org.kbinani.vsq.VsqPhoneticSymbol.isConsonant( symbol[i] ) ? 64 : 0;
                }
            }
            for ( var i = 0; i < this.m_consonant_adjustment.length; i++ ) {
                result += "," + this.m_consonant_adjustment[i];
            }
            if ( this.PhoneticSymbolProtected ) {
                result += ",1";
            } else {
                result += ",0";
            }
            return result;
        }
    }

}
