/*
 * SymbolTableEntry.cs
 * Copyright (C) 2010 kbinani
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
        /// 発音記号列
        /// </summary>
        public String Symbol = "";

        public SymbolTableEntry( String word, String symbol ) {
            Word = word;
            if ( Word == null ) {
                Word = "";
            }
            Symbol = symbol;
            if ( Symbol == null ) {
                Symbol = "";
            }
        }

        /// <summary>
        /// このオブジェクトのディープ・コピーを取得します
        /// </summary>
        /// <returns></returns>
        public Object clone() {
            return new SymbolTableEntry( Word, Symbol );
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
