/*
 * IconHandle.cs
 * Copyright (C) 2008-2010 kbinani
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
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// 歌手設定を表します。
    /// </summary>
#if JAVA
    public class IconHandle implements Cloneable, Serializable{
#else
    [Serializable]
    public class IconHandle : ICloneable{
#endif
        /// <summary>
        /// キャプション。
        /// </summary>
        public String Caption = "";
        /// <summary>
        /// この歌手設定を一意に識別するためのIDです。
        /// </summary>
        public String IconID = "";
        /// <summary>
        /// ユーザ・フレンドリー名。
        /// このフィールドの値は、他の歌手設定のユーザ・フレンドリー名と重複する場合があります。
        /// </summary>
        public String IDS = "";
        public int Index;
        public int Length;
        public int Original;
        public int Program;
        public int Language;

        public IconHandle() {
        }

        public int getLength() {
            return Length;
        }

        public void setLength( int value ) {
            Length = value;
        }

        public boolean equals( IconHandle item ) {
            if ( item == null ) {
                return false;
            } else {
                return IconID.Equals( item.IconID );
            }
        }

        public Object clone() {
            IconHandle ret = new IconHandle();
            ret.Caption = Caption;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Index = Index;
            ret.Language = Language;
            ret.setLength( Length );
            ret.Original = Original;
            ret.Program = Program;
            return ret;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.Singer;
            ret.Caption = Caption;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Index = Index;
            ret.Language = Language;
            ret.setLength( Length );
            ret.Program = Program;
            return ret;
        }
    }

#if !JAVA
}
#endif
