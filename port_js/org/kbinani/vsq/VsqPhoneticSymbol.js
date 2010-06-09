/*
 * VsqPhoneticSymbol.js
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
if( org.kbinani.vsq.VsqPhoneticSymbol == undefined ){

    /// <summary>
    /// VSQで使用される発音記号の種類や有効性を判定するユーティリティ群です。
    /// </summary>
    org.kbinani.vsq.VsqPhoneticSymbol = new function(){
    };
    
    org.kbinani.vsq.VsqPhoneticSymbol._SYMBOL_VOWEL_JP = new Array(
        "a",
        "i",
        "M",
        "e",
        "o" );

    org.kbinani.vsq.VsqPhoneticSymbol._SYMBOL_CONSONANT_JP = new Array(
        "k",
        "k'",
        "g",
        "g'",
        "N",
        "N'",
        "s",
        "S",
        "z",
        "Z",
        "dz",
        "dZ",
        "t",
        "t'",
        "ts",
        "tS",
        "d",
        "d'",
        "n",
        "J",
        "h",
        "h\\",
        "C",
        "p\\",
        "p\\'",
        "b",
        "b'",
        "p",
        "p'",
        "m",
        "m'",
        "j",
        "4",
        "4'",
        "w",
        "N\\" );
    
    org.kbinani.vsq.VsqPhoneticSymbol._SYMBOL_EN = new Array(
        "@",
        "V",
        "e",
        "e",
        "I",
        "i:",
        "{",
        "O:",
        "Q",
        "U",
        "u:",
        "@r",
        "eI",
        "aI",
        "OI",
        "@U",
        "aU",
        "I@",
        "e@",
        "U@",
        "O@",
        "Q@",
        "w",
        "j",
        "b",
        "d",
        "g",
        "bh",
        "dh",
        "gh",
        "dZ",
        "v",
        "D",
        "z",
        "Z",
        "m",
        "n",
        "N",
        "r",
        "l",
        "l0",
        "p",
        "t",
        "k",
        "ph",
        "th",
        "kh",
        "tS",
        "f",
        "T",
        "s",
        "S",
        "h",
        "Sil",
        "Asp" );

    /**
     * 指定した文字列が子音を表す発音記号かどうかを判定します。
     *
     * @param symbol [String]
     * @return [bool]
     */
    org.kbinani.vsq.VsqPhoneticSymbol.isConsonant = function( symbol ) {
        for ( var i = 0; i < org.kbinani.vsq.VsqPhoneticSymbol._SYMBOL_CONSONANT_JP.length; i++ ) {
            var s = org.kbinani.vsq.VsqPhoneticSymbol._SYMBOL_CONSONANT_JP[i];
            if ( s == symbol ) {
                return true;
            }
        }
        return false;
    };

    /**
     * 指定した文字列が発音記号として有効かどうかを判定します。
     *
     * @param "symbol [String]
     * @return [bool]
     */
    org.kbinani.vsq.VsqPhoneticSymbol.isValidSymbol = function( symbol ) {
        for ( var i = 0; i < org.kbinani.vsq.VsqPhoneticSymbol._SYMBOL_VOWEL_JP.length; i++ ) {
            var s = org.kbinani.vsq.VsqPhoneticSymbol._SYMBOL_VOWEL_JP[i];
            if ( s == symbol ) {
                return true;
            }
        }
        for ( var i = 0; i < org.kbinani.vsq.VsqPhoneticSymbol._SYMBOL_CONSONANT_JP.length; i++ ) {
            var s = org.kbinani.vsq.VsqPhoneticSymbol._SYMBOL_CONSONANT_JP[i];
            if ( s == symbol ) {
                return true;
            }
        }
        for ( var i = 0; i < org.kbinani.vsq.VsqPhoneticSymbol._SYMBOL_EN.length; i++ ) {
            var s = org.kbinani.vsq.VsqPhoneticSymbol._SYMBOL_EN[i];
            if ( s == symbol ) {
                return true;
            }
        }

        // ブレスの判定
        var strlen = symbol.length;
        if ( symbol.indexOf( "br" ) == 0 && strlen > 2 ) {
            var s = symbol.substring( 2 );
            // br001とかをfalseにするためのチェック
            var num = parseInt( s, 10 );
            if ( s == ("" + num) ) {
                return true;
            }
        }
        return false;
    };

}
