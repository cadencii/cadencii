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
using System;
using System.Collections.Generic;
using cadencii.vsq;
using cadencii.java.util;

namespace cadencii
{

    [Serializable]
    public class ClipboardEntry
    {
        public List<VsqEvent> events;
        /// <summary>
        /// コピーorカットで複製されたテンポ
        /// </summary>
        public List<TempoTableEntry> tempo;
        /// <summary>
        /// コピーorカットで複製された拍子
        /// </summary>
        public List<TimeSigTableEntry> timesig;
        /// <summary>
        /// コピーorカットで複製されたカーブ
        /// </summary>
        public SortedDictionary<CurveType, VsqBPList> points;
        /// <summary>
        /// コピーorカットで複製されたベジエ曲線
        /// </summary>
        public SortedDictionary<CurveType, List<BezierChain>> beziers;
        /// <summary>
        /// コピーの開始位置。貼付け時に、この値をもとにクロックのシフトを行う
        /// </summary>
        public int copyStartedClock;
    }

}
