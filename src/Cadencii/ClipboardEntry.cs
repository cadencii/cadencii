/*
 * ClipboardEntry.cs
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
#if JAVA
package cadencii;

import java.io.*;
import java.util.*;
import cadencii.*;
import cadencii.vsq.*;
#else
using System;
using cadencii.vsq;
using cadencii.java.util;

namespace cadencii {

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
