/*
 * NoteHeadHandle.cs
 * Copyright (C) 2009 kbinani
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
#endif

#if JAVA
    public class NoteHeadHandle implements Cloneable, Serializable {
#else
    [Serializable]
    public class NoteHeadHandle : ICloneable {
#endif
        public int Index;
        public String IconID = "";
        public String IDS = "";
        public int Original;
        public String Caption = "";
        public int Length;
        public int Duration;
        public int Depth;

        public NoteHeadHandle() {
        }

        public int getLength() {
            return Length;
        }

        public void setLength( int value ) {
            Length = value;
        }

        public String getDisplayString() {
            String s = IDS;
            if ( !Caption.Equals( "" ) ) {
                s += " (" + Caption + ")";
            }
            return s;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

        public Object clone() {
            NoteHeadHandle result = new NoteHeadHandle();
            result.Index = Index;
            result.IconID = IconID;
            result.IDS = IDS;
            result.Original = Original;
            result.Caption = Caption;
            result.setLength( Length );
            result.Duration = Duration;
            result.Depth = Depth;
            return result;
        }

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.NoteHeadHandle;
            ret.Index = Index;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Original = Original;
            ret.Caption = Caption;
            ret.setLength( Length );
            ret.Duration = Duration;
            ret.Depth = Depth;
            return ret;
        }
    }

#if !JAVA
}
#endif
