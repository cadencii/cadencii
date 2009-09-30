/*
 * VsqPhoneticSymbol.java
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

public class VsqPhoneticSymbol {
    private static String[] _SYMBOL_VOWEL_JP = new String[]{
        "a",
        "i",
        "M",
        "e",
        "o",
    };
    private static String[] _SYMBOL_CONSONANT_JP = new String[]{
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
        "N\\",
    };
    private static String[] _SYMBOL_EN = new String[]{
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
    };

    public static boolean isConsonant( String symbol ) {
        for ( int i = 0; i < _SYMBOL_CONSONANT_JP.length; i++ ){
            String s = _SYMBOL_CONSONANT_JP[i];
            if ( s.equals( symbol ) ) {
                return true;
            }
        }
        return false;
    }

    public static boolean isValidSymbol( String symbol ) {
        for ( int i = 0; i < _SYMBOL_VOWEL_JP.length; i++ ){
            String s = _SYMBOL_VOWEL_JP[i];
            if ( s.equals( symbol ) ) {
                return true;
            }
        }
        for ( int i = 0; i < _SYMBOL_CONSONANT_JP.length; i++ ){
            String s = _SYMBOL_CONSONANT_JP[i];
            if ( s.equals( symbol ) ) {
                return true;
            }
        }
        for ( int i = 0; i < _SYMBOL_EN.length; i++ ){
            String s = _SYMBOL_EN[i];
            if ( s.equals( symbol ) ) {
                return true;
            }
        }
        return false;
    }
}
