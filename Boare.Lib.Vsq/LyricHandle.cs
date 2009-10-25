/*
 * LyricHandle.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.io.*;
#else
using System;

namespace Boare.Lib.Vsq {
#endif

#if JAVA
    public class LyricHandle implements Cloneable, Serializable{
#else
    [Serializable]
    public class LyricHandle : ICloneable {
#endif
        public Lyric L0;
        public int Index;

        public LyricHandle() {
        }

        /// <summary>
        /// type = Lyric用のhandleのコンストラクタ
        /// </summary>
        /// <param name="phrase">歌詞</param>
        /// <param name="phonetic_symbol">発音記号</param>
        public LyricHandle( String phrase, String phonetic_symbol ) {
            L0 = new Lyric( phrase, phonetic_symbol );
        }

        public Object clone() {
            LyricHandle ret = new LyricHandle();
            ret.Index = Index;
            ret.L0 = (Lyric)L0.Clone();
            return ret;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.Lyric;
            ret.L0 = (Lyric)L0.Clone();
            ret.Index = Index;
            return ret;
        }
    }

#if !JAVA
}
#endif
