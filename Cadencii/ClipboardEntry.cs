/*
 * ClipboardEntry.cs
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
#if JAVA
package org.kbinani.Cadencii;

import java.io.*;
import java.util.*;
import org.kbinani.*;
import org.kbinani.vsq.*;
#else
using System;
using Boare.Lib.Vsq;
using bocoree.java.util;

namespace Boare.Cadencii {

#endif

#if JAVA
    public class ClipboardEntry implements Serializable {
#else
    [Serializable]
    public class ClipboardEntry {
#endif
        public Vector<VsqEvent> events;
        /// <summary>
        /// コピーorカットで複製されたテンポ
        /// </summary>
        public Vector<TempoTableEntry> tempo;
        /// <summary>
        /// コピーorカットで複製された拍子
        /// </summary>
        public Vector<TimeSigTableEntry> timesig;
        /// <summary>
        /// コピーorカットで複製されたカーブ
        /// </summary>
        public TreeMap<CurveType, VsqBPList> points;
        /// <summary>
        /// コピーorカットで複製されたベジエ曲線
        /// </summary>
        public TreeMap<CurveType, Vector<BezierChain>> beziers;
        /// <summary>
        /// コピーの開始位置。貼付け時に、この値をもとにクロックのシフトを行う
        /// </summary>
        public int copyStartedClock;
    }

#if !JAVA
}
#endif
