/*
 * IconHandle.cs
 * Copyright (C) 2008-2009 kbinani
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

#if JAVA
    public class IconHandle implements Cloneable, Serializable{
#else
    [Serializable]
    public class IconHandle : ICloneable{
#endif
        public String Caption = "";
        public String IconID = "";
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
