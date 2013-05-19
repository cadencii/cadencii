/*
 * SyllableMatcher.cs
 * Copyright © 2013 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;
using System.Text;
using cadencii.java.util;

namespace cadencii
{
    /// <summary>
    /// AquesTone2 で利用できる音素を、歌詞文字列から検索するクラス
    /// </summary>
    public class SyllableMatcher
    {
        /// <summary>
        /// カタカナの音素と、AquesTone2 の音素を紐付けるマップ
        /// </summary>
        private static TreeMap<String, String> map;

        static SyllableMatcher()
        {
            map = new TreeMap<String, String>() {
                { "ニャン", "nyan" }, { "ビャ", "bya" },  { "ビェ", "bye" },  { "ビョ", "byo" },  { "ビュ", "byu" },
                { "チ", "chi" }, { "チャ", "cya" }, { "チェ", "cye" }, { "チョ", "cyo" }, { "チュ", "cyu" },
                { "ディ", "dhi" }, { "デュ", "dhu" }, { "ドゥ", "dwu" }, { "ギャ", "gya" }, { "ギェ", "gye" },
                { "ギョ", "gyo" }, { "ギュ", "gyu" }, { "ヒャ", "hya" }, { "ヒェ", "hye" }, { "ヒョ", "hyo" },
                { "ヒュ", "hyu" }, { "キャ", "kya" }, { "キェ", "kye" }, { "キョ", "kyo" }, { "キュ", "kyu" },
                { "ミャ", "mya" }, { "ミェ", "mye" }, { "ミョ", "myo" }, { "ミュ", "myu" }, { "ニャ", "nya" },
                { "ニェ", "nye" }, { "ニョ", "nyo" }, { "ニュ", "nyu" }, { "リャ", "rya" }, { "リェ", "rye" },
                { "リョ", "ryo" }, { "リュ", "ryu" }, { "スィ", "swi" }, { "シャ", "sya" }, { "シェ", "sye" },
                { "ショ", "syo" }, { "シュ", "syu" }, { "ティ", "thi" }, { "テュ", "thu" }, { "ツァ", "tsa" },
                { "ツェ", "tse" }, { "ツィ", "tsi" }, { "ツォ", "tso" }, { "トゥ", "twu" }, { "ズィ", "zwi" },
                { "バ", "ba" }, { "ベ", "be" }, { "ビ", "bi" }, { "ボ", "bo" }, { "ブ", "bu" }, { "ダ", "da" },
                { "デ", "de" }, { "ド", "do" }, { "ファ", "fa" }, { "フェ", "fe" }, { "フィ", "fi" }, { "フォ", "fo" },
                { "フ", "fu" }, { "ガ", "ga" }, { "ゲ", "ge" }, { "ギ", "gi" }, { "ゴ", "go" }, { "グ", "gu" },
                { "ハ", "ha" }, { "ヘ", "he" }, { "ヒ", "hi" }, { "ホ", "ho" }, { "ジャ", "ja" }, { "ジェ", "je" },
                { "ジ", "ji" }, { "ジョ", "jo" }, { "ジュ", "ju" }, { "カ", "ka" }, { "ケ", "ke" }, { "キ", "ki" },
                { "コ", "ko" }, { "ク", "ku" }, { "マ", "ma" }, { "メ", "me" }, { "ミ", "mi" }, { "モ", "mo" },
                { "ム", "mu" }, { "ナ", "na" }, { "ネ", "ne" }, { "ニ", "ni" }, { "ノ", "no" }, { "ヌ", "nu" },
                { "パ", "pa" }, { "ペ", "pe" }, { "ピ", "pi" }, { "ポ", "po" }, { "プ", "pu" }, { "ラ", "ra" },
                { "レ", "re" }, { "リ", "ri" }, { "ロ", "ro" }, { "ル", "ru" }, { "サ", "sa" }, { "セ", "se" },
                { "シ", "si" }, { "ソ", "so" }, { "ス", "su" }, { "タ", "ta" }, { "テ", "te" }, { "ト", "to" },
                { "ツ", "tu" }, { "ヴァ", "va" }, { "ヴェ", "ve" }, { "ヴィ", "vi" }, { "ヴォ", "vo" }, { "ヴ", "vu" },
                { "ワ", "wa" }, { "ヱ", "we" }, { "ヰ", "wi" }, { "ヲ", "wo" }, { "ヤ", "ya" }, { "イェ", "ye" },
                { "ヨ", "yo" }, { "ユ", "yu" }, { "ザ", "za" }, { "ゼ", "ze" }, { "ゾ", "zo" }, { "ズ", "zu" },
                { "ア", "a" }, { "エ", "e" }, { "イ", "i" }, { "ン", "n" }, { "オ", "o" }, { "ウ", "u" },
            };
        }

        /// <summary>
        /// 歌詞文字列から、音素を検索する。見つからなければ空文字列を返す
        /// </summary>
        /// <param name="phrase">歌詞文字列</param>
        /// <returns>AquesTone2 の音素</returns>
        public String find( String phrase )
        {
            String hiragana = KanaDeRomanization.Attach( phrase );
            String kana = KanaDeRomanization.hiragana2katakana( hiragana );
            if ( map.containsKey( kana ) ) {
                return map.get( kana );
            } else {
                return "";
            }
        }
    }
}
