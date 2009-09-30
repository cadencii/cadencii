/*
 * KanaDeRomanization.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    static class KanaDeRomanization {
        public static String Attach( String roman ) {
            char[] arr = roman.ToCharArray();
            String ret = "";
            int index = 0;
            const int _MAX_MATCH = 4;
            while ( index < arr.Length ) {
                // _MAX_MATCH～2文字のマッチ
                boolean processed = false;
                for ( int i = _MAX_MATCH; i >= 2; i-- ) {
                    if ( index + (i - 1) < arr.Length ) {
                        String s = "";
                        for ( int j = 0; j < i; j++ ) {
                            s += "" + arr[index + j];
                        }
                        boolean trailing;
                        String res = AttachCor( s, out trailing );
                        if ( res != s ) {
                            if ( !trailing ) {
                                index = index + i;
                            } else {
                                index = index + i - 1;
                            }
                            ret += res;
                            processed = true;
                            break;
                        }
                    }
                }
                if ( processed ) {
                    continue;
                }

                // 1文字のマッチ
                boolean trailing1;
                ret += AttachCor( arr[index] + "", out trailing1 );
                index++;
            }
            return ret;
        }

        private static String AttachCor( String roman, out boolean trailing ) {
            String s = roman.ToLower();
            trailing = false;
            switch ( s ) {
                case "a":
                    return "あ";
                case "i":
                case "yi":
                    return "い";
                case "u":
                case "wu":
                    return "う";
                case "e":
                    return "え";
                case "o":
                    return "お";
                case "ka":
                case "ca":
                    return "か";
                case "ki":
                    return "き";
                case "ku":
                case "cu":
                case "qu":
                    return "く";
                case "ke":
                    return "け";
                case "ko":
                case "co":
                    return "こ";
                case "sa":
                    return "さ";
                case "si":
                case "shi":
                case "ci":
                    return "し";
                case "su":
                    return "す";
                case "se":
                case "ce":
                    return "せ";
                case "so":
                    return "そ";
                case "ta":
                    return "た";
                case "chi":
                case "ti":
                    return "ち";
                case "tu":
                case "tsu":
                    return "つ";
                case "te":
                    return "て";
                case "to":
                    return "と";
                case "na":
                    return "な";
                case "ni":
                    return "に";
                case "nu":
                    return "ぬ";
                case "ne":
                    return "ね";
                case "no":
                    return "の";
                case "ha":
                    return "は";
                case "hi":
                    return "ひ";
                case "hu":
                case "fu":
                    return "ふ";
                case "he":
                    return "へ";
                case "ho":
                    return "ほ";
                case "ma":
                    return "ま";
                case "mi":
                    return "み";
                case "mu":
                    return "む";
                case "me":
                    return "め";
                case "mo":
                    return "も";
                case "ya":
                    return "や";
                case "yu":
                    return "ゆ";
                case "ye":
                    return "いぇ";
                case "yo":
                    return "よ";
                case "ra":
                    return "ら";
                case "ri":
                    return "り";
                case "ru":
                    return "る";
                case "re":
                    return "れ";
                case "ro":
                    return "ろ";
                case "wa":
                    return "わ";
                case "wi":
                    return "うぃ";
                case "wyi":
                    return "ゐ";
                case "we":
                    return "うぇ";
                case "wye":
                    return "ゑ";
                case "wo":
                    return "を";
                case "nn":
                case "n":
                    return "ん";
                case "ga":
                    return "が";
                case "gi":
                    return "ぎ";
                case "gu":
                    return "ぐ";
                case "ge":
                    return "げ";
                case "go":
                    return "ご";
                case "za":
                    return "ざ";
                case "zi":
                case "ji":
                    return "じ";
                case "zu":
                    return "ず";
                case "ze":
                    return "ぜ";
                case "zo":
                    return "ぞ";
                case "da":
                    return "だ";
                case "di":
                    return "ぢ";
                case "du":
                    return "づ";
                case "de":
                    return "で";
                case "do":
                    return "ど";
                case "ba":
                    return "ば";
                case "bi":
                    return "び";
                case "bu":
                    return "ぶ";
                case "be":
                    return "べ";
                case "bo":
                    return "ぼ";
                case "pa":
                    return "ぱ";
                case "pi":
                    return "ぴ";
                case "pu":
                    return "ぷ";
                case "pe":
                    return "ぺ";
                case "po":
                    return "ぽ";
                case "sha":
                    return "しゃ";
                case "shu":
                    return "しゅ";
                case "sho":
                    return "しょ";
                case "cha":
                case "tya":
                    return "ちゃ";
                case "chu":
                case "tyu":
                    return "ちゅ";
                case "cho":
                case "tyo":
                    return "ちょ";
                case "dya":
                    return "ぢゃ";
                case "dyu":
                    return "ぢゅ";
                case "dyo":
                    return "ぢょ";
                case "kwa":
                    return "くゎ";
                case "kwi":
                    return "くぃ";
                case "kwu":
                    return "くぅ";
                case "kwe":
                    return "くぇ";
                case "kwo":
                    return "くぉ";
                case "gwa":
                    return "ぐゎ";
                case "kya":
                    return "きゃ";
                case "kyu":
                    return "きゅ";
                case "kyo":
                    return "きょ";
                case "sya":
                    return "しゃ";
                case "syu":
                    return "しゅ";
                case "syo":
                    return "しょ";
                case "nya":
                    return "にゃ";
                case "nyu":
                    return "にゅ";
                case "nyo":
                    return "にょ";
                case "mya":
                    return "みゃ";
                case "myu":
                    return "みゅ";
                case "myo":
                    return "みょ";
                case "rya":
                    return "りゃ";
                case "ryu":
                    return "りゅ";
                case "ryo":
                    return "りょ";
                case "gya":
                    return "ぎゃ";
                case "gyu":
                    return "ぎゅ";
                case "gyo":
                    return "ぎょ";
                case "zya":
                case "ja":
                    return "じゃ";
                case "zyu":
                case "ju":
                    return "じゅ";
                case "zyo":
                case "jo":
                    return "じょ";
                case "bya":
                    return "びゃ";
                case "byu":
                    return "びゅ";
                case "byo":
                    return "びょ";
                case "pya":
                    return "ぴゃ";
                case "pyu":
                    return "ぴゅ";
                case "pyo":
                    return "ぴょ";
                case "la":
                case "xa":
                    return "ぁ";
                case "li":
                case "xi":
                case "lyi":
                case "xyi":
                    return "ぃ";
                case "lu":
                case "xu":
                    return "ぅ";
                case "le":
                case "xe":
                case "lye":
                case "xye":
                    return "ぇ";
                case "lo":
                case "xo":
                    return "ぉ";
                case "lya":
                case "xya":
                    return "ゃ";
                case "lyu":
                case "xyu":
                    return "ゅ";
                case "lyo":
                case "xyo":
                    return "ょ";
                case "lwa":
                case "xwa":
                    return "ゎ";
                case "ltu":
                case "xtu":
                case "xtsu":
                case "ltsu":
                    return "っ";
                case "va":
                    return "ゔぁ";
                case "vi":
                    return "ゔぃ";
                case "vu":
                    return "ゔ";
                case "ve":
                    return "ゔぇ";
                case "vo":
                    return "ゔぉ";
                case "fa":
                    return "ふぁ";
                case "fi":
                    return "ふぃ";
                case "fe":
                    return "ふぇ";
                case "fo":
                    return "ふぉ";
                case "qa":
                    return "くぁ";
                case "qi":
                    return "くぃ";
                case "qe":
                    return "くぇ";
                case "qo":
                    return "くぉ";
                case "vyu":
                    return "ゔゅ";
                case "qq":
                case "ww":
                case "rr":
                case "tt":
                case "yy":
                case "pp":
                case "ss":
                case "dd":
                case "ff":
                case "gg":
                case "hh":
                case "jj":
                case "kk":
                case "ll":
                case "zz":
                case "xx":
                case "cc":
                case "vv":
                case "bb":
                case "mm":
                    trailing = true;
                    return "っ";
                case "-":
                    return "ー";
                case "tha":
                    return "てゃ";
                case "thi":
                    return "てぃ";
                case "thu":
                    return "てゅ";
                case "the":
                    return "てぇ";
                case "tho":
                    return "てょ";
                case "twa":
                    return "とぁ";
                case "twi":
                    return "とぃ";
                case "twu":
                    return "とぅ";
                case "twe":
                    return "とぇ";
                case "two":
                    return "とぉ";
                case "dha":
                    return "でゃ";
                case "dhi":
                    return "でぃ";
                case "dhu":
                    return "でゅ";
                case "dhe":
                    return "でぇ";
                case "dho":
                    return "でょ";
                case "wha":
                    return "うぁ";
                case "whi":
                    return "うぃ";
                case "whu":
                    return "う";
                case "whe":
                    return "うぇ";
                case "who":
                    return "うぉ";
                case "lka":
                case "xka":
                    return "ヵ";
                case "lke":
                case "xke":
                    return "ヶ";
                case "tsa":
                    return "つぁ";
                case "tsi":
                    return "つぃ";
                case "tse":
                    return "つぇ";
                case "tso":
                    return "つぉ";
                case "jya":
                    return "じゃ";
                case "jyu":
                    return "じゅ";
                case "jyo":
                    return "じょ";
                case "cya":
                    return "ちゃ";
                case "cyi":
                    return "ちぃ";
                case "cyu":
                    return "ちゅ";
                case "cye":
                    return "ちぇ";
                case "cyo":
                    return "ちょ";
                case "dwa":
                    return "どぁ";
                case "dwi":
                    return "どぃ";
                case "dwu":
                    return "どぅ";
                case "dwe":
                    return "どぇ";
                case "dwo":
                    return "どぉ";
                case "hwa":
                    return "ふぁ";
                case "hwi":
                    return "ふぃ";
                case "hwu":
                    return "ふぇ";
                case "hwo":
                    return "ふぉ";
                case "fyu":
                case "hwyu":
                    return "ふゅ";
            }
            return roman;
        }
    }

}
