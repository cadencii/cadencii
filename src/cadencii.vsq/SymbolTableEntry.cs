/*
 * SymbolTableEntry.cs
 * Copyright © 2010-2011 kbinani
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

namespace cadencii.vsq
{

    /// <summary>
    /// SymbolTableの要素を表すクラス。
    /// </summary>
    public class SymbolTableEntry : ICloneable
    {
        /// <summary>
        /// 単語（英語の場合、分節位置にハイフンが入る）
        /// </summary>
        public string Word = "";
        /// <summary>
        /// 発音記号列．タブ記号を含む形式
        /// </summary>
        private string m_raw_symbol = "";
        /// <summary>
        /// 発音記号列．タブ記号を含まない形式
        /// </summary>
        private string m_symbol = "";

        public SymbolTableEntry(string word, string symbol)
        {
            Word = word;
            if (Word == null) {
                Word = "";
            }
            m_raw_symbol = symbol;
            if (m_raw_symbol == null) {
                m_raw_symbol = "";
            }
            m_symbol = m_raw_symbol.Replace('\t', ' ');
        }

        /// <summary>
        /// 発音記号列を取得します．発音記号列は空白' 'で区切られています．
        /// 英単語の分節の分割位置を知るには，このメソッドの代わりにgetRawSymbolメソッドを呼び出し，
        /// タブ記号の位置を調べてください．
        /// </summary>
        /// <returns></returns>
        public string getSymbol()
        {
            return m_symbol;
        }

        /// <summary>
        /// 発音記号列を取得します．発音記号列は空白' 'またはタブ'\t'で区切られています．
        /// タブによる区切りは英単語の分節の分割位置を表し，
        /// 空白による区切りは分節中に複数の発音記号がある場合の区切りを表します．
        /// </summary>
        /// <returns></returns>
        public string getRawSymbol()
        {
            return m_raw_symbol;
        }

        /// <summary>
        /// 発音記号列を取得します．発音記号列は空白' 'またはタブ'\t'で区切られています．
        /// タブによる区切りは英単語の分節の分割位置を表し，
        /// 空白による区切りは分節中に複数の発音記号がある場合の区切りを表します．
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public string Symbol
        {
            get
            {
                return getSymbol();
            }
        }

        /// <summary>
        /// このオブジェクトのSymbolフィールドのタブ文字を空白に置き換えた文字列を取得します．
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public string getParsedSymbol()
        {
            return getSymbol();
        }

        /// <summary>
        /// このオブジェクトのディープ・コピーを取得します
        /// </summary>
        /// <returns></returns>
        public Object clone()
        {
            return new SymbolTableEntry(Word, m_raw_symbol);
        }

        /// <summary>
        /// このオブジェクトのディープ・コピーを取得します
        /// </summary>
        /// <returns></returns>
        public Object Clone()
        {
            return clone();
        }
    }

}
