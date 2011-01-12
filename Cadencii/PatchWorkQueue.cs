/*
 * PatchWorkQueue.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

#else
using System;

namespace org.kbinani.cadencii
{
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 合成の範囲やトラック番号を指示するためのクラス
    /// </summary>
    public class PatchWorkQueue
    {
        /// <summary>
        /// 合成対象のトラック番号
        /// </summary>
        public int track;
        /// <summary>
        /// 合成開始位置．単位はclock
        /// </summary>
        public int clockStart;
        /// <summary>
        /// 合成修了位置．単位はclock
        /// </summary>
        public int clockEnd;
        /// <summary>
        /// 合成結果を出力するファイル名
        /// </summary>
        public String file;
        /// <summary>
        /// トラック全体を合成する場合true，それ以外はfalse
        /// </summary>
        public boolean renderAll;
    }

#if !JAVA
}
#endif
