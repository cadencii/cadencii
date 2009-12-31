/*
 * DrawObject.cs
 * Copyright (C) 2008-2010 kbinani
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

import org.kbinani.vsq.*;
import java.awt.*;
#else
using System;
using org.kbinani.vsq;
using org.kbinani.java.awt;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 画面に描画するアイテムを表します
    /// </summary>
#if JAVA
    public class DrawObject implements Comparable<DrawObject>{
#else
    public class DrawObject : IComparable<DrawObject> {
#endif
        public Rectangle pxRectangle;
        public String text;
        public int accent;
        public int internalID;
        /// <summary>
        /// 音符の先頭から，ビブラート開始位置までの長さ(単位：ピクセル)
        /// </summary>
        public int pxVibratoDelay;
        /// <summary>
        /// このアイテムが他のアイテムと再生時にオーバーラップするかどうかを表すフラグ
        /// </summary>
        public boolean overlappe;
        public boolean symbolProtected;
        public VibratoBPList vibRate;
        public VibratoBPList vibDepth;
        public int vibStartRate;
        public int vibStartDepth;
        public int note;
        public UstEnvelope ustEnvelope;
        /// <summary>
        /// 音符の長さ（クロック）
        /// </summary>
        public int length;
        public DrawObjectType type;

        public DrawObject( DrawObjectType type,
                           Rectangle rect, 
                           String text_,
                           int accent_,
                           int internal_id,
                           int vibrato_delay,
                           boolean overwrapped, 
                           boolean symbol_protected,
                           VibratoBPList vib_rate,
                           VibratoBPList vib_depth,
                           int vib_start_rate,
                           int vib_start_depth,
                           int note_,
                           UstEnvelope ust_envelope,
                           int length_ ) {
            this.type = type;
            pxRectangle = rect;
            text = text_;
            accent = accent_;
            internalID = internal_id;
            pxVibratoDelay = vibrato_delay;
            overlappe = overwrapped;
            symbolProtected = symbol_protected;
            vibRate = vib_rate;
            vibDepth = vib_depth;
            vibStartRate = vib_start_rate;
            vibStartDepth = vib_start_depth;
            note = note_;
            ustEnvelope = ust_envelope;
            length = length_;
        }

        public int compareTo( DrawObject item ) {
            return pxRectangle.x - item.pxRectangle.x;
        }

#if !JAVA
        public int CompareTo( DrawObject item ){
            return compareTo( item );
        }
#endif
    }

#if !JAVA
}
#endif
