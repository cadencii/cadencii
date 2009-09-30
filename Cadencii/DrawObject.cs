/*
 * DrawObject.cs
 * Copyright (c) 2008-2009 kbinani
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
using System.Drawing;

using Boare.Lib.Vsq;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    /// <summary>
    /// 画面に描画するアイテムを表します
    /// </summary>
    class DrawObject : IComparable<DrawObject> {
        public Rectangle pxRectangle;
        public String Text;
        public int Accent;
        public int InternalID;
        /// <summary>
        /// 音符の先頭から，ビブラート開始位置までの長さ(単位：ピクセル)
        /// </summary>
        public int pxVibratoDelay;
        /// <summary>
        /// このアイテムが他のアイテムと再生時にオーバーラップするかどうかを表すフラグ
        /// </summary>
        public boolean Overwrapped;
        public boolean SymbolProtected;
        public VibratoBPList VibRate;
        public VibratoBPList VibDepth;
        public int VibStartRate;
        public int VibStartDepth;
        public int Note;
        public UstEnvelope UstEnvelope;
        /// <summary>
        /// 音符の長さ（クロック）
        /// </summary>
        public int Length;

        public DrawObject( Rectangle rect, 
                           String text,
                           int accent,
                           int internal_id,
                           int vibrato_delay,
                           boolean overwrapped, 
                           boolean symbol_protected,
                           VibratoBPList vib_rate,
                           VibratoBPList vib_depth,
                           int vib_start_rate,
                           int vib_start_depth,
                           int note,
                           UstEnvelope ust_envelope,
                           int length ) {
            pxRectangle = rect;
            Text = text;
            Accent = accent;
            InternalID = internal_id;
            pxVibratoDelay = vibrato_delay;
            Overwrapped = overwrapped;
            SymbolProtected = symbol_protected;
            VibRate = vib_rate;
            VibDepth = vib_depth;
            VibStartRate = vib_start_rate;
            VibStartDepth = vib_start_depth;
            Note = note;
            UstEnvelope = ust_envelope;
            Length = length;
        }

        public int CompareTo( DrawObject item ) {
            return pxRectangle.X - item.pxRectangle.X;
        }
    }

}
