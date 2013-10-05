/*
 * KanaDeRomanization.cs
 * Copyright © 2009-2011 kbinani
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
using cadencii;



namespace cadencii
{

    public class KanaDeRomanization
    {
        const int _MAX_MATCH = 4;

        /// <summary>
        /// ひらがなをカタカナに変換する
        /// </summary>
        /// <param name="maybe_katakana"></param>
        /// <returns></returns>
        public static string hiragana2katakana(string maybe_hiragana)
        {
            char[] arr = maybe_hiragana.ToCharArray();
            string ret = "";
            int i = -1;
            while (i + 1 < arr.Length) {
                i++;
                char transformed = hiragana2katakanaCor(arr[i]);
                if (i + 1 < arr.Length) {
                    if (arr[i + 1] == 'ﾞ' || arr[i + 1] == '゛') {
                        if (transformed == 'カ') {
                            transformed = 'ガ';
                        } else if (transformed == 'キ') {
                            transformed = 'ギ';
                        } else if (transformed == 'ク') {
                            transformed = 'グ';
                        } else if (transformed == 'ケ') {
                            transformed = 'ゲ';
                        } else if (transformed == 'コ') {
                            transformed = 'ゴ';
                        } else if (transformed == 'サ') {
                            transformed = 'ザ';
                        } else if (transformed == 'シ') {
                            transformed = 'ジ';
                        } else if (transformed == 'ス') {
                            transformed = 'ズ';
                        } else if (transformed == 'セ') {
                            transformed = 'ゼ';
                        } else if (transformed == 'ソ') {
                            transformed = 'ゾ';
                        } else if (transformed == 'タ') {
                            transformed = 'ダ';
                        } else if (transformed == 'チ') {
                            transformed = 'ヂ';
                        } else if (transformed == 'ツ') {
                            transformed = 'ヅ';
                        } else if (transformed == 'テ') {
                            transformed = 'デ';
                        } else if (transformed == 'ト') {
                            transformed = 'ド';
                        } else if (transformed == 'ハ') {
                            transformed = 'バ';
                        } else if (transformed == 'ヒ') {
                            transformed = 'ビ';
                        } else if (transformed == 'フ') {
                            transformed = 'ブ';
                        } else if (transformed == 'ヘ') {
                            transformed = 'ベ';
                        } else if (transformed == 'ホ') {
                            transformed = 'ボ';
                        } else if (transformed == 'ウ') {
                            transformed = 'ヴ';
                        }
                        i++;
                    } else if (arr[i + 1] == 'ﾟ' || arr[i + 1] == '゜') {
                        if (transformed == 'ハ') {
                            transformed = 'パ';
                        } else if (transformed == 'ヒ') {
                            transformed = 'ピ';
                        } else if (transformed == 'フ') {
                            transformed = 'プ';
                        } else if (transformed == 'ヘ') {
                            transformed = 'ペ';
                        } else if (transformed == 'ホ') {
                            transformed = 'ポ';
                        }
                        i++;
                    }
                }
                ret = ret + transformed;
            }
            return ret;
        }

        private static char hiragana2katakanaCor(char maybe_hiragana)
        {
            if (maybe_hiragana == 'あ' || maybe_hiragana == 'ｱ') {
                return 'ア';
            } else if (maybe_hiragana == 'い' || maybe_hiragana == 'ｲ') {
                return 'イ';
            } else if (maybe_hiragana == 'う' || maybe_hiragana == 'ｳ') {
                return 'ウ';
            } else if (maybe_hiragana == 'え' || maybe_hiragana == 'ｴ') {
                return 'エ';
            } else if (maybe_hiragana == 'お' || maybe_hiragana == 'ｵ') {
                return 'オ';
            } else if (maybe_hiragana == 'か' || maybe_hiragana == 'ｶ') {
                return 'カ';
            } else if (maybe_hiragana == 'き' || maybe_hiragana == 'ｷ') {
                return 'キ';
            } else if (maybe_hiragana == 'く' || maybe_hiragana == 'ｸ') {
                return 'ク';
            } else if (maybe_hiragana == 'け' || maybe_hiragana == 'ｹ') {
                return 'ケ';
            } else if (maybe_hiragana == 'こ' || maybe_hiragana == 'ｺ') {
                return 'コ';
            } else if (maybe_hiragana == 'さ' || maybe_hiragana == 'ｻ') {
                return 'サ';
            } else if (maybe_hiragana == 'し' || maybe_hiragana == 'ｼ') {
                return 'シ';
            } else if (maybe_hiragana == 'す' || maybe_hiragana == 'ｽ') {
                return 'ス';
            } else if (maybe_hiragana == 'せ' || maybe_hiragana == 'ｾ') {
                return 'セ';
            } else if (maybe_hiragana == 'そ' || maybe_hiragana == 'ｿ') {
                return 'ソ';
            } else if (maybe_hiragana == 'た' || maybe_hiragana == 'ﾀ') {
                return 'タ';
            } else if (maybe_hiragana == 'ち' || maybe_hiragana == 'ﾁ') {
                return 'チ';
            } else if (maybe_hiragana == 'つ' || maybe_hiragana == 'ﾂ') {
                return 'ツ';
            } else if (maybe_hiragana == 'て' || maybe_hiragana == 'ﾃ') {
                return 'テ';
            } else if (maybe_hiragana == 'と' || maybe_hiragana == 'ﾄ') {
                return 'ト';
            } else if (maybe_hiragana == 'な' || maybe_hiragana == 'ﾅ') {
                return 'ナ';
            } else if (maybe_hiragana == 'に' || maybe_hiragana == 'ﾆ') {
                return 'ニ';
            } else if (maybe_hiragana == 'ぬ' || maybe_hiragana == 'ﾇ') {
                return 'ヌ';
            } else if (maybe_hiragana == 'ね' || maybe_hiragana == 'ﾈ') {
                return 'ネ';
            } else if (maybe_hiragana == 'の' || maybe_hiragana == 'ﾉ') {
                return 'ノ';
            } else if (maybe_hiragana == 'は' || maybe_hiragana == 'ﾊ') {
                return 'ハ';
            } else if (maybe_hiragana == 'ひ' || maybe_hiragana == 'ﾋ') {
                return 'ヒ';
            } else if (maybe_hiragana == 'ふ' || maybe_hiragana == 'ﾌ') {
                return 'フ';
            } else if (maybe_hiragana == 'へ' || maybe_hiragana == 'ﾍ') {
                return 'ヘ';
            } else if (maybe_hiragana == 'ほ' || maybe_hiragana == 'ﾎ') {
                return 'ホ';
            } else if (maybe_hiragana == 'ま' || maybe_hiragana == 'ﾏ') {
                return 'マ';
            } else if (maybe_hiragana == 'み' || maybe_hiragana == 'ﾐ') {
                return 'ミ';
            } else if (maybe_hiragana == 'む' || maybe_hiragana == 'ﾑ') {
                return 'ム';
            } else if (maybe_hiragana == 'め' || maybe_hiragana == 'ﾒ') {
                return 'メ';
            } else if (maybe_hiragana == 'も' || maybe_hiragana == 'ﾓ') {
                return 'モ';
            } else if (maybe_hiragana == 'や' || maybe_hiragana == 'ﾔ') {
                return 'ヤ';
            } else if (maybe_hiragana == 'ゆ' || maybe_hiragana == 'ﾕ') {
                return 'ユ';
            } else if (maybe_hiragana == 'よ' || maybe_hiragana == 'ﾖ') {
                return 'ヨ';
            } else if (maybe_hiragana == 'ら' || maybe_hiragana == 'ﾗ') {
                return 'ラ';
            } else if (maybe_hiragana == 'り' || maybe_hiragana == 'ﾘ') {
                return 'リ';
            } else if (maybe_hiragana == 'る' || maybe_hiragana == 'ﾙ') {
                return 'ル';
            } else if (maybe_hiragana == 'れ' || maybe_hiragana == 'ﾚ') {
                return 'レ';
            } else if (maybe_hiragana == 'ろ' || maybe_hiragana == 'ﾛ') {
                return 'ロ';
            } else if (maybe_hiragana == 'わ' || maybe_hiragana == 'ﾜ') {
                return 'ワ';
            } else if (maybe_hiragana == 'を' || maybe_hiragana == 'ｦ') {
                return 'ヲ';
            } else if (maybe_hiragana == 'ん' || maybe_hiragana == 'ﾝ') {
                return 'ン';
            } else if (maybe_hiragana == 'が') {
                return 'ガ';
            } else if (maybe_hiragana == 'ぱ') {
                return 'パ';
            } else if (maybe_hiragana == 'ぁ' || maybe_hiragana == 'ｧ') {
                return 'ァ';
            } else if (maybe_hiragana == 'ぃ' || maybe_hiragana == 'ｨ') {
                return 'ィ';
            } else if (maybe_hiragana == 'ぅ' || maybe_hiragana == 'ｩ') {
                return 'ゥ';
            } else if (maybe_hiragana == 'ぇ' || maybe_hiragana == 'ｪ') {
                return 'ェ';
            } else if (maybe_hiragana == 'ぉ' || maybe_hiragana == 'ｫ') {
                return 'ォ';
            } else if (maybe_hiragana == 'ゃ' || maybe_hiragana == 'ｬ') {
                return 'ャ';
            } else if (maybe_hiragana == 'ゅ' || maybe_hiragana == 'ｭ') {
                return 'ュ';
            } else if (maybe_hiragana == 'ょ' || maybe_hiragana == 'ｮ') {
                return 'ョ';
            } else if (maybe_hiragana == 'っ' || maybe_hiragana == 'ｯ') {
                return 'ッ';
            } else if (maybe_hiragana == 'ゐ') {
                return 'ヰ';
            } else if (maybe_hiragana == 'ゑ') {
                return 'ヱ';
            } else if (maybe_hiragana == 'ぎ') {
                return 'ギ';
            } else if (maybe_hiragana == 'ぐ') {
                return 'グ';
            } else if (maybe_hiragana == 'げ') {
                return 'ゲ';
            } else if (maybe_hiragana == 'ご') {
                return 'ゴ';
            } else if (maybe_hiragana == 'ざ') {
                return 'ザ';
            } else if (maybe_hiragana == 'じ') {
                return 'ジ';
            } else if (maybe_hiragana == 'ず') {
                return 'ズ';
            } else if (maybe_hiragana == 'ぜ') {
                return 'ゼ';
            } else if (maybe_hiragana == 'ぞ') {
                return 'ゾ';
            } else if (maybe_hiragana == 'だ') {
                return 'ダ';
            } else if (maybe_hiragana == 'ぢ') {
                return 'ヂ';
            } else if (maybe_hiragana == 'づ') {
                return 'ヅ';
            } else if (maybe_hiragana == 'で') {
                return 'デ';
            } else if (maybe_hiragana == 'ど') {
                return 'ド';
            } else if (maybe_hiragana == 'ば') {
                return 'バ';
            } else if (maybe_hiragana == 'び') {
                return 'ビ';
            } else if (maybe_hiragana == 'ぶ') {
                return 'ブ';
            } else if (maybe_hiragana == 'べ') {
                return 'ベ';
            } else if (maybe_hiragana == 'ぼ') {
                return 'ボ';
            } else if (maybe_hiragana == 'ぴ') {
                return 'ピ';
            } else if (maybe_hiragana == 'ぷ') {
                return 'プ';
            } else if (maybe_hiragana == 'ぺ') {
                return 'ペ';
            } else if (maybe_hiragana == 'ぽ') {
                return 'ポ';
            } else if (maybe_hiragana == 'ゎ') {
                return 'ワ';
            } else if (maybe_hiragana == 'ヴ') {
                return 'ヴ';
            }
            return maybe_hiragana;
        }

        public static string Attach(string roman)
        {
            char[] arr = roman.ToCharArray();
            string ret = "";
            int index = 0;
            while (index < arr.Length) {
                // _MAX_MATCH～2文字のマッチ
                bool processed = false;
                for (int i = _MAX_MATCH; i >= 2; i--) {
                    if (index + (i - 1) < arr.Length) {
                        string s = "";
                        for (int j = 0; j < i; j++) {
                            s += "" + arr[index + j];
                        }
                        ByRef<Boolean> trailing = new ByRef<Boolean>();
                        string res = AttachCor(s, trailing);
                        if (res != s) {
                            if (!trailing.value) {
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
                if (processed) {
                    continue;
                }

                // 1文字のマッチ
                ByRef<Boolean> trailing1 = new ByRef<Boolean>();
                ret += AttachCor(arr[index] + "", trailing1);
                index++;
            }
            return ret;
        }

        private static string AttachCor(string roman, ByRef<Boolean> trailing)
        {
            string s = roman.ToLower();
            trailing.value = false;
            if (s.Equals("a")) {
                return "あ";
            } else if (s.Equals("i") ||
                        s.Equals("yi")) {
                return "い";
            } else if (s.Equals("u") ||
                        s.Equals("wu")) {
                return "う";
            } else if (s.Equals("e")) {
                return "え";
            } else if (s.Equals("o")) {
                return "お";
            } else if (s.Equals("ka") ||
                        s.Equals("ca")) {
                return "か";
            } else if (s.Equals("ki")) {
                return "き";
            } else if (s.Equals("ku") ||
                        s.Equals("cu") ||
                        s.Equals("qu")) {
                return "く";
            } else if (s.Equals("ke")) {
                return "け";
            } else if (s.Equals("ko") ||
                        s.Equals("co")) {
                return "こ";
            } else if (s.Equals("sa")) {
                return "さ";
            } else if (s.Equals("si") ||
                        s.Equals("shi") ||
                        s.Equals("ci")) {
                return "し";
            } else if (s.Equals("su")) {
                return "す";
            } else if (s.Equals("se") ||
                        s.Equals("ce")) {
                return "せ";
            } else if (s.Equals("so")) {
                return "そ";
            } else if (s.Equals("ta")) {
                return "た";
            } else if (s.Equals("chi") ||
                        s.Equals("ti")) {
                return "ち";
            } else if (s.Equals("tu") ||
                        s.Equals("tsu")) {
                return "つ";
            } else if (s.Equals("te")) {
                return "て";
            } else if (s.Equals("to")) {
                return "と";
            } else if (s.Equals("na")) {
                return "な";
            } else if (s.Equals("ni")) {
                return "に";
            } else if (s.Equals("nu")) {
                return "ぬ";
            } else if (s.Equals("ne")) {
                return "ね";
            } else if (s.Equals("no")) {
                return "の";
            } else if (s.Equals("ha")) {
                return "は";
            } else if (s.Equals("hi")) {
                return "ひ";
            } else if (s.Equals("hu") ||
                        s.Equals("fu")) {
                return "ふ";
            } else if (s.Equals("he")) {
                return "へ";
            } else if (s.Equals("ho")) {
                return "ほ";
            } else if (s.Equals("ma")) {
                return "ま";
            } else if (s.Equals("mi")) {
                return "み";
            } else if (s.Equals("mu")) {
                return "む";
            } else if (s.Equals("me")) {
                return "め";
            } else if (s.Equals("mo")) {
                return "も";
            } else if (s.Equals("ya")) {
                return "や";
            } else if (s.Equals("yu")) {
                return "ゆ";
            } else if (s.Equals("ye")) {
                return "いぇ";
            } else if (s.Equals("yo")) {
                return "よ";
            } else if (s.Equals("ra")) {
                return "ら";
            } else if (s.Equals("ri")) {
                return "り";
            } else if (s.Equals("ru")) {
                return "る";
            } else if (s.Equals("re")) {
                return "れ";
            } else if (s.Equals("ro")) {
                return "ろ";
            } else if (s.Equals("wa")) {
                return "わ";
            } else if (s.Equals("wi")) {
                return "うぃ";
            } else if (s.Equals("wyi")) {
                return "ゐ";
            } else if (s.Equals("we")) {
                return "うぇ";
            } else if (s.Equals("wye")) {
                return "ゑ";
            } else if (s.Equals("wo")) {
                return "を";
            } else if (s.Equals("nn") ||
                        s.Equals("n")) {
                return "ん";
            } else if (s.Equals("ga")) {
                return "が";
            } else if (s.Equals("gi")) {
                return "ぎ";
            } else if (s.Equals("gu")) {
                return "ぐ";
            } else if (s.Equals("ge")) {
                return "げ";
            } else if (s.Equals("go")) {
                return "ご";
            } else if (s.Equals("za")) {
                return "ざ";
            } else if (s.Equals("zi") ||
                        s.Equals("ji")) {
                return "じ";
            } else if (s.Equals("zu")) {
                return "ず";
            } else if (s.Equals("ze")) {
                return "ぜ";
            } else if (s.Equals("zo")) {
                return "ぞ";
            } else if (s.Equals("da")) {
                return "だ";
            } else if (s.Equals("di")) {
                return "ぢ";
            } else if (s.Equals("du")) {
                return "づ";
            } else if (s.Equals("de")) {
                return "で";
            } else if (s.Equals("do")) {
                return "ど";
            } else if (s.Equals("ba")) {
                return "ば";
            } else if (s.Equals("bi")) {
                return "び";
            } else if (s.Equals("bu")) {
                return "ぶ";
            } else if (s.Equals("be")) {
                return "べ";
            } else if (s.Equals("bo")) {
                return "ぼ";
            } else if (s.Equals("pa")) {
                return "ぱ";
            } else if (s.Equals("pi")) {
                return "ぴ";
            } else if (s.Equals("pu")) {
                return "ぷ";
            } else if (s.Equals("pe")) {
                return "ぺ";
            } else if (s.Equals("po")) {
                return "ぽ";
            } else if (s.Equals("sha")) {
                return "しゃ";
            } else if (s.Equals("shu")) {
                return "しゅ";
            } else if (s.Equals("sho")) {
                return "しょ";
            } else if (s.Equals("cha") ||
                        s.Equals("tya")) {
                return "ちゃ";
            } else if (s.Equals("chu") ||
                        s.Equals("tyu")) {
                return "ちゅ";
            } else if (s.Equals("cho") ||
                        s.Equals("tyo")) {
                return "ちょ";
            } else if (s.Equals("dya")) {
                return "ぢゃ";
            } else if (s.Equals("dyu")) {
                return "ぢゅ";
            } else if (s.Equals("dyo")) {
                return "ぢょ";
            } else if (s.Equals("kwa")) {
                return "くゎ";
            } else if (s.Equals("kwi")) {
                return "くぃ";
            } else if (s.Equals("kwu")) {
                return "くぅ";
            } else if (s.Equals("kwe")) {
                return "くぇ";
            } else if (s.Equals("kwo")) {
                return "くぉ";
            } else if (s.Equals("gwa")) {
                return "ぐゎ";
            } else if (s.Equals("kya")) {
                return "きゃ";
            } else if (s.Equals("kyu")) {
                return "きゅ";
            } else if (s.Equals("kyo")) {
                return "きょ";
            } else if (s.Equals("sya")) {
                return "しゃ";
            } else if (s.Equals("syu")) {
                return "しゅ";
            } else if (s.Equals("syo")) {
                return "しょ";
            } else if (s.Equals("nya")) {
                return "にゃ";
            } else if (s.Equals("nyu")) {
                return "にゅ";
            } else if (s.Equals("nyo")) {
                return "にょ";
            } else if (s.Equals("mya")) {
                return "みゃ";
            } else if (s.Equals("myu")) {
                return "みゅ";
            } else if (s.Equals("myo")) {
                return "みょ";
            } else if (s.Equals("rya")) {
                return "りゃ";
            } else if (s.Equals("ryu")) {
                return "りゅ";
            } else if (s.Equals("ryo")) {
                return "りょ";
            } else if (s.Equals("gya")) {
                return "ぎゃ";
            } else if (s.Equals("gyu")) {
                return "ぎゅ";
            } else if (s.Equals("gyo")) {
                return "ぎょ";
            } else if (s.Equals("zya") ||
                        s.Equals("ja")) {
                return "じゃ";
            } else if (s.Equals("zyu") ||
                        s.Equals("ju")) {
                return "じゅ";
            } else if (s.Equals("zyo") ||
                        s.Equals("jo")) {
                return "じょ";
            } else if (s.Equals("bya")) {
                return "びゃ";
            } else if (s.Equals("byu")) {
                return "びゅ";
            } else if (s.Equals("byo")) {
                return "びょ";
            } else if (s.Equals("pya")) {
                return "ぴゃ";
            } else if (s.Equals("pyu")) {
                return "ぴゅ";
            } else if (s.Equals("pyo")) {
                return "ぴょ";
            } else if (s.Equals("la") ||
                        s.Equals("xa")) {
                return "ぁ";
            } else if (s.Equals("li") ||
                        s.Equals("xi") ||
                        s.Equals("lyi") ||
                        s.Equals("xyi")) {
                return "ぃ";
            } else if (s.Equals("lu") ||
                        s.Equals("xu")) {
                return "ぅ";
            } else if (s.Equals("le") ||
                        s.Equals("xe") ||
                        s.Equals("lye") ||
                        s.Equals("xye")) {
                return "ぇ";
            } else if (s.Equals("lo") ||
                        s.Equals("xo")) {
                return "ぉ";
            } else if (s.Equals("lya") ||
                        s.Equals("xya")) {
                return "ゃ";
            } else if (s.Equals("lyu") ||
                        s.Equals("xyu")) {
                return "ゅ";
            } else if (s.Equals("lyo") ||
                        s.Equals("xyo")) {
                return "ょ";
            } else if (s.Equals("lwa") ||
                        s.Equals("xwa")) {
                return "ゎ";
            } else if (s.Equals("ltu") ||
                        s.Equals("xtu") ||
                        s.Equals("xtsu") ||
                        s.Equals("ltsu")) {
                return "っ";
            } else if (s.Equals("va")) {
                return "ゔぁ";
            } else if (s.Equals("vi")) {
                return "ゔぃ";
            } else if (s.Equals("vu")) {
                return "ゔ";
            } else if (s.Equals("ve")) {
                return "ゔぇ";
            } else if (s.Equals("vo")) {
                return "ゔぉ";
            } else if (s.Equals("fa")) {
                return "ふぁ";
            } else if (s.Equals("fi")) {
                return "ふぃ";
            } else if (s.Equals("fe")) {
                return "ふぇ";
            } else if (s.Equals("fo")) {
                return "ふぉ";
            } else if (s.Equals("qa")) {
                return "くぁ";
            } else if (s.Equals("qi")) {
                return "くぃ";
            } else if (s.Equals("qe")) {
                return "くぇ";
            } else if (s.Equals("qo")) {
                return "くぉ";
            } else if (s.Equals("vyu")) {
                return "ゔゅ";
            } else if (s.Equals("qq") ||
                        s.Equals("ww") ||
                        s.Equals("rr") ||
                        s.Equals("tt") ||
                        s.Equals("yy") ||
                        s.Equals("pp") ||
                        s.Equals("ss") ||
                        s.Equals("dd") ||
                        s.Equals("ff") ||
                        s.Equals("gg") ||
                        s.Equals("hh") ||
                        s.Equals("jj") ||
                        s.Equals("kk") ||
                        s.Equals("ll") ||
                        s.Equals("zz") ||
                        s.Equals("xx") ||
                        s.Equals("cc") ||
                        s.Equals("vv") ||
                        s.Equals("bb") ||
                        s.Equals("mm")) {
                trailing.value = true;
                return "っ";
            } else if (s.Equals("-")) {
                return "ー";
            } else if (s.Equals("tha")) {
                return "てゃ";
            } else if (s.Equals("thi")) {
                return "てぃ";
            } else if (s.Equals("thu")) {
                return "てゅ";
            } else if (s.Equals("the")) {
                return "てぇ";
            } else if (s.Equals("tho")) {
                return "てょ";
            } else if (s.Equals("twa")) {
                return "とぁ";
            } else if (s.Equals("twi")) {
                return "とぃ";
            } else if (s.Equals("twu")) {
                return "とぅ";
            } else if (s.Equals("twe")) {
                return "とぇ";
            } else if (s.Equals("two")) {
                return "とぉ";
            } else if (s.Equals("dha")) {
                return "でゃ";
            } else if (s.Equals("dhi")) {
                return "でぃ";
            } else if (s.Equals("dhu")) {
                return "でゅ";
            } else if (s.Equals("dhe")) {
                return "でぇ";
            } else if (s.Equals("dho")) {
                return "でょ";
            } else if (s.Equals("wha")) {
                return "うぁ";
            } else if (s.Equals("whi")) {
                return "うぃ";
            } else if (s.Equals("whu")) {
                return "う";
            } else if (s.Equals("whe")) {
                return "うぇ";
            } else if (s.Equals("who")) {
                return "うぉ";
            } else if (s.Equals("lka") ||
                        s.Equals("xka")) {
                return "ヵ";
            } else if (s.Equals("lke") ||
                        s.Equals("xke")) {
                return "ヶ";
            } else if (s.Equals("tsa")) {
                return "つぁ";
            } else if (s.Equals("tsi")) {
                return "つぃ";
            } else if (s.Equals("tse")) {
                return "つぇ";
            } else if (s.Equals("tso")) {
                return "つぉ";
            } else if (s.Equals("jya")) {
                return "じゃ";
            } else if (s.Equals("jyu")) {
                return "じゅ";
            } else if (s.Equals("jyo")) {
                return "じょ";
            } else if (s.Equals("cya")) {
                return "ちゃ";
            } else if (s.Equals("cyi")) {
                return "ちぃ";
            } else if (s.Equals("cyu")) {
                return "ちゅ";
            } else if (s.Equals("cye")) {
                return "ちぇ";
            } else if (s.Equals("cyo")) {
                return "ちょ";
            } else if (s.Equals("dwa")) {
                return "どぁ";
            } else if (s.Equals("dwi")) {
                return "どぃ";
            } else if (s.Equals("dwu")) {
                return "どぅ";
            } else if (s.Equals("dwe")) {
                return "どぇ";
            } else if (s.Equals("dwo")) {
                return "どぉ";
            } else if (s.Equals("hwa")) {
                return "ふぁ";
            } else if (s.Equals("hwi")) {
                return "ふぃ";
            } else if (s.Equals("hwu")) {
                return "ふぇ";
            } else if (s.Equals("hwo")) {
                return "ふぉ";
            } else if (s.Equals("fyu") ||
                        s.Equals("hwyu")) {
                return "ふゅ";
            }
            return roman;
        }
    }

}
