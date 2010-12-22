/*
 * SymbolTableEntry.cs
 * Copyright © 2010 kbinani
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
#if JAVA
package org.kbinani.vsq;

import java.io.*;
#else
using System;

namespace org.kbinani.vsq {
#endif

    /// <summary>
    /// SymbolTableの要素を表すクラス。
    /// </summary>
#if JAVA
    public class SymbolTableEntry implements Cloneable {
#else
    public class SymbolTableEntry : ICloneable {
#endif
        /// <summary>
        /// 単語（英語の場合、分節位置にハイフンが入る）
        /// </summary>
        public String Word = "";
        /// <summary>
        /// 発音記号列．タブ記号を含む形式
        /// </summary>
        private String m_raw_symbol = "";
        /// <summary>
        /// 発音記号列．タブ記号を含まない形式
        /// </summary>
        private String m_symbol = "";

        public SymbolTableEntry( String word, String symbol ) {
            Word = word;
            if ( Word == null ) {
                Word = "";
            }
            m_raw_symbol = symbol;
            if ( m_raw_symbol == null ) {
                m_raw_symbol = "";
            }
            m_symbol = m_raw_symbol.Replace( '\t', ' ' );
        }

        /// <summary>
        /// 発音記号列を取得します．発音記号列は空白' 'で区切られています．
        /// 英単語の分節の分割位置を知るには，このメソッドの代わりにgetRawSymbolメソッドを呼び出し，
        /// タブ記号の位置を調べてください．
        /// </summary>
        /// <returns></returns>
        public String getSymbol() {
            return m_symbol;
        }

        /// <summary>
        /// 発音記号列を取得します．発音記号列は空白' 'またはタブ'\t'で区切られています．
        /// タブによる区切りは英単語の分節の分割位置を表し，
        /// 空白による区切りは分節中に複数の発音記号がある場合の区切りを表します．
        /// </summary>
        /// <returns></returns>
        public String getRawSymbol() {
            return m_raw_symbol;
        }

#if !JAVA
        /// <summary>
        /// 発音記号列を取得します．発音記号列は空白' 'またはタブ'\t'で区切られています．
        /// タブによる区切りは英単語の分節の分割位置を表し，
        /// 空白による区切りは分節中に複数の発音記号がある場合の区切りを表します．
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public String Symbol {
            get {
                return getSymbol();
            }
        }
#endif

        /// <summary>
        /// このオブジェクトのSymbolフィールドのタブ文字を空白に置き換えた文字列を取得します．
        /// </summary>
        /// <returns></returns>
#if !JAVA
        [Obsolete]
#endif
        public String getParsedSymbol() {
            return getSymbol();
        }

        /// <summary>
        /// このオブジェクトのディープ・コピーを取得します
        /// </summary>
        /// <returns></returns>
        public Object clone() {
            return new SymbolTableEntry( Word, m_raw_symbol );
        }

        /// <summary>
        /// このオブジェクトのディープ・コピーを取得します
        /// </summary>
        /// <returns></returns>
#if !JAVA
        public Object Clone() {
            return clone();
        }
#endif
    }

#if !JAVA
}
#endif
