/*
 * Lyric.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;

using cadencii;

namespace cadencii.vsq
{

    /// <summary>
    /// VsqHandleに格納される歌詞の情報を扱うクラス。
    /// </summary>
    [Serializable]
    public class Lyric
    {
        /// <summary>
        /// この歌詞のフレーズ
        /// </summary>
        public string Phrase;
        private List<string> mPhoneticSymbols;
        public float UnknownFloat;
        private List<int> mConsonantAdjustments;
        public bool PhoneticSymbolProtected;

        /// <summary>
        /// このクラスの指定した名前のプロパティをXMLシリアライズする際に使用する
        /// 要素名を取得します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string getXmlElementName(string name)
        {
            return name;
        }

        /// <summary>
        /// このオブジェクトのインスタンスと、指定されたアイテムが同じかどうかを調べます。
        /// 音声合成したときに影響のある範囲のフィールドしか比較されません。
        /// たとえば、PhoneticSymbolProtectedがthisとitemで違っていても、他が同一であればtrueが返る。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool equalsForSynth(Lyric item)
        {
            if (this.PhoneticSymbolProtected != item.PhoneticSymbolProtected) return false;
            if (this.getPhoneticSymbol() != item.getPhoneticSymbol()) return false;
            if (this.getConsonantAdjustment() != item.getConsonantAdjustment()) return false;
            return true;
        }

        /// <summary>
        /// このオブジェクトのインスタンスと、指定されたオブジェクトが同じかどうかを調べます。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool equals(Lyric item)
        {
            if (!equalsForSynth(item)) return false;
            if (this.Phrase != item.Phrase) return false;
            if (this.UnknownFloat != item.UnknownFloat) return false;
            return true;
        }

        /// <summary>
        /// Consonant Adjustmentの文字列形式を取得します。
        /// </summary>
        /// <returns></returns>
        public string getConsonantAdjustment()
        {
            string ret = "";
            List<int> arr = getConsonantAdjustmentList();
            int size = arr.Count;

            for (int i = 0; i < size; i++) {
                int v = arr[i];
                ret += (i == 0 ? "" : " ") + v;
            }
            return ret;
        }

        /// <summary>
        /// Consonant Adjustmentを文字列形式で設定します。
        /// </summary>
        /// <param name="value"></param>
        public void setConsonantAdjustment(string value)
        {
            List<string> spl = new List<string>();
            spl = new List<string>(value.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries));

            int size = spl.Count;
            List<int> arr = new List<int>();
            for (int i = 0; i < size; i++) {
                int v = 64;
                try {
                    v = int.Parse(spl[i]);
                } catch (Exception ex) {
                    serr.println("Lyric#setCosonantAdjustment; ex=" + ex);
                }
                arr.Add(v);
            }
            setConsonantAdjustmentList(arr);
        }

        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public string ConsonantAdjustment
        {
            get
            {
                return getConsonantAdjustment();
            }
            set
            {
                setConsonantAdjustment(value);
            }
        }

        /// <summary>
        /// Consonant Adjustmentを、整数配列で取得します。
        /// </summary>
        /// <returns></returns>
        public List<int> getConsonantAdjustmentList()
        {
            if (mConsonantAdjustments != null) {
                return mConsonantAdjustments;
            }
            if (mPhoneticSymbols == null) {
                mConsonantAdjustments = new List<int>();
                return mConsonantAdjustments;
            }

            mConsonantAdjustments.Clear();
            for (int i = 0; i < mPhoneticSymbols.Count; i++) {
                int v = VsqPhoneticSymbol.isConsonant(mPhoneticSymbols[i]) ? 64 : 0;
                mConsonantAdjustments.Add(v);
            }
            return mConsonantAdjustments;
        }

        /// <summary>
        /// Consonant Adjustmentを、整数配列形式で設定します。
        /// </summary>
        /// <param name="value"></param>
        public void setConsonantAdjustmentList(List<int> value)
        {
            if (value == null) {
                return;
            }
            mConsonantAdjustments.Clear();
            for (int i = 0; i < value.Count; i++) {
                int v = value[i];
                mConsonantAdjustments.Add(v);
            }
        }

        /// <summary>
        /// このオブジェクトの簡易コピーを取得します。
        /// </summary>
        /// <returns>このインスタンスの簡易コピー</returns>
        public Object clone()
        {
            Lyric result = new Lyric();
            result.Phrase = this.Phrase;
            result.mPhoneticSymbols = new List<string>();
            for (int i = 0; i < mPhoneticSymbols.Count; i++) {
                result.mPhoneticSymbols.Add(mPhoneticSymbols[i]);
            }
            result.UnknownFloat = this.UnknownFloat;
            if (mConsonantAdjustments != null) {
                result.mConsonantAdjustments = new List<int>();
                for (int i = 0; i < mConsonantAdjustments.Count; i++) {
                    result.mConsonantAdjustments.Add(mConsonantAdjustments[i]);
                }
            }
            result.PhoneticSymbolProtected = PhoneticSymbolProtected;
            return result;
        }

        public Object Clone()
        {
            return clone();
        }

        public Lyric()
        {
            UnknownFloat = 1.0f;
            mConsonantAdjustments = new List<int>();
            mPhoneticSymbols = new List<string>();
        }

        /// <summary>
        /// 歌詞、発音記号を指定したコンストラクタ
        /// </summary>
        /// <param name="phrase">歌詞</param>
        /// <param name="phonetic_symbol">発音記号</param>
        public Lyric(string phrase, string phonetic_symbol)
        {
            Phrase = phrase;
            setPhoneticSymbol(phonetic_symbol);
            UnknownFloat = 1.0f;
        }

        /// <summary>
        /// 文字列(ex."a","a",0.0000,0.0)からのコンストラクタ
        /// </summary>
        /// <param name="line"></param>
        public Lyric(string line)
        {
            int len = PortUtil.getStringLength(line);
            if (len == 0) {
                Phrase = "a";
                setPhoneticSymbol("a");
                UnknownFloat = 1.0f;
                PhoneticSymbolProtected = false;
                setConsonantAdjustment("0");
                return;
            }
            int indx = -1;
            int dquote_count = 0;
            string work = "";
            string consonant_adjustment = "";
            for (int i = 0; i < len; i++) {
                char c = line[i];
                if (c == ',') {
                    if (dquote_count % 2 == 0) {
                        // ,の左側に偶数個の"がある場合→,は区切り文字
                        indx++;
                        if (indx == 0) {
                            // Phrase
                            work = work.Replace("\"\"", "\"");  // "は""として保存される
                            if (work.StartsWith("\"") && work.EndsWith("\"")) {
                                int l = work.Length;
                                if (l > 2) {
                                    Phrase = work.Substring(1, l - 2);
                                } else {
                                    Phrase = "a";
                                }
                            } else {
                                Phrase = work;
                            }
                            work = "";
                        } else if (indx == 1) {
                            // symbols
                            string symbols = "";
                            if (work.StartsWith("\"") && work.EndsWith("\"")) {
                                int l = PortUtil.getStringLength(work);
                                if (l > 2) {
                                    symbols = work.Substring(1, l - 2);
                                } else {
                                    symbols = "a";
                                }
                            } else {
                                symbols = work;
                            }
                            setPhoneticSymbol(symbols);
                            work = "";
                        } else if (indx == 2) {
                            // UnknownFloat
                            UnknownFloat = (float)double.Parse(work);
                            work = "";
                        } else {
                            if (indx - 3 < mPhoneticSymbols.Count) {
                                // consonant adjustment
                                if (indx - 3 == 0) {
                                    consonant_adjustment += work;
                                } else {
                                    consonant_adjustment += "," + work;
                                }
                            } else {
                                // protected
                                PhoneticSymbolProtected = (work == "1");
                            }
                            work = "";
                        }
                    } else {
                        // ,の左側に奇数個の"がある場合→,は歌詞等の一部
                        work += "" + c;
                    }
                } else {
                    work += "" + c;
                    if (c == '"') {
                        dquote_count++;
                    }
                }
            }
            setConsonantAdjustment(consonant_adjustment);
        }

        /// <summary>
        /// この歌詞の発音記号を取得します。
        /// </summary>
        public string getPhoneticSymbol()
        {
            List<string> symbol = getPhoneticSymbolList();
            string ret = "";
            for (int i = 0; i < symbol.Count; i++) {
                ret += (i == 0 ? "" : " ") + symbol[i];
            }
            return ret;
        }

        /// <summary>
        /// この歌詞の発音記号を設定します。
        /// </summary>
        public void setPhoneticSymbol(string value)
        {
            string s = value.Replace("  ", " ");

            // 古い発音記号を保持しておく
            List<string> old_symbol = null;
            if (mPhoneticSymbols != null) {
                int count = mPhoneticSymbols.Count;
                old_symbol = new List<string>();
                for (int i = 0; i < count; i++) {
                    old_symbol.Add(mPhoneticSymbols[i]);
                }
            }

            // 古いconsonant adjustmentを保持しておく
            List<int> old_adjustment = null;
            if (mConsonantAdjustments != null) {
                old_adjustment = new List<int>();
                int count = mConsonantAdjustments.Count;
                for (int i = 0; i < count; i++) {
                    old_adjustment.Add(mConsonantAdjustments[i]);
                }
            }

            string[] spl = PortUtil.splitString(s, new char[] { ' ' }, 16, true);
            if (mPhoneticSymbols == null) {
                mPhoneticSymbols = new List<string>();
            }
            int size = spl.Length;
            mPhoneticSymbols.Clear();
            for (int i = 0; i < size; i++) {
                mPhoneticSymbols.Add(spl[i]);
            }
            for (int i = 0; i < mPhoneticSymbols.Count; i++) {
                mPhoneticSymbols[i] = mPhoneticSymbols[i].Replace("\\" + "\\", "\\");
            }

            // consonant adjustmentを更新
            if (mConsonantAdjustments == null ||
                (mConsonantAdjustments != null && mConsonantAdjustments.Count != mPhoneticSymbols.Count)) {
                mConsonantAdjustments = new List<int>();
                for (int i = 0; i < mPhoneticSymbols.Count; i++) {
                    mConsonantAdjustments.Add(0);
                }
            }

            // 古い発音記号と同じなら、古い値を使う
            if (old_symbol != null) {
                for (int i = 0; i < mPhoneticSymbols.Count; i++) {
                    if (i >= old_symbol.Count) {
                        break;
                    }
                    string s0 = mPhoneticSymbols[i];
                    string s1 = old_symbol[i];
                    bool use_old_value = (old_symbol != null && i < old_symbol.Count) &&
                                            (s0 == s1) &&
                                            (old_adjustment != null && i < old_adjustment.Count);
                    if (use_old_value) {
                        mConsonantAdjustments[i] = VsqPhoneticSymbol.isConsonant(mPhoneticSymbols[i]) ? old_adjustment[i] : 0;
                    } else {
                        mConsonantAdjustments[i] = VsqPhoneticSymbol.isConsonant(mPhoneticSymbols[i]) ? 64 : 0;
                    }
                }
            }
        }

        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public string PhoneticSymbol
        {
            get
            {
                return getPhoneticSymbol();
            }
            set
            {
                setPhoneticSymbol(value);
            }
        }

        public List<string> getPhoneticSymbolList()
        {
            if (mPhoneticSymbols == null) {
                mPhoneticSymbols = new List<string>();
            }
            return mPhoneticSymbols;
        }

        /// <summary>
        /// このインスタンスを文字列に変換します
        /// </summary>
        /// <param name="add_quatation_mark">クォーテーションマークを付けるかどうか</param>
        /// <returns>変換後の文字列</returns>
        public string toString(bool add_quatation_mark)
        {
            string quot = (add_quatation_mark ? "\"" : "");
            string result;
            string phrase = (this.Phrase == null) ? "" : this.Phrase.Replace("\"", "\"\"");
            result = quot + phrase + quot + ",";
            List<string> symbol = getPhoneticSymbolList();
            string strSymbol = getPhoneticSymbol();
            if (!add_quatation_mark) {
                if (strSymbol == null || (strSymbol != null && strSymbol == "")) {
                    strSymbol = "u:";
                }
            }
            result += quot + strSymbol + quot + "," + PortUtil.formatDecimal("0.000000", UnknownFloat);
            result = result.Replace("\\" + "\\", "\\");
            if (mConsonantAdjustments == null) {
                mConsonantAdjustments = new List<int>();
                for (int i = 0; i < symbol.Count; i++) {
                    mConsonantAdjustments.Add(VsqPhoneticSymbol.isConsonant(symbol[i]) ? 64 : 0);
                }
            }
            for (int i = 0; i < mConsonantAdjustments.Count; i++) {
                result += "," + mConsonantAdjustments[i];
            }
            if (PhoneticSymbolProtected) {
                result += ",1";
            } else {
                result += ",0";
            }
            return result;
        }

        public override string ToString()
        {
            return toString(true);
        }
    }

}
