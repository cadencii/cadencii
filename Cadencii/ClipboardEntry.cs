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
using System;
using System.Collections.Generic;

using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Cadencii {

    using Integer = Int32;

    [Serializable]
    public class ClipboardEntry {
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

}
