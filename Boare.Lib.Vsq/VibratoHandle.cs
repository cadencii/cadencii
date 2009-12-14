/*
 * VibratoHandle.cs
 * Copyright (c) 2009 kbinani
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
    public class VibratoHandle implements Cloneable, Serializable {
#else
    [Serializable]
    public class VibratoHandle : ICloneable {
#endif
        public int StartDepth;
        public VibratoBPList DepthBP;
        public int StartRate;
        public VibratoBPList RateBP;
        public int Index;
        public String IconID = "";
        public String IDS = "";
        public int Original;
        public String Caption = "";
        public int Length;

        public VibratoHandle() {
            StartRate = 64;
            StartDepth = 64;
            RateBP = new VibratoBPList();
            DepthBP = new VibratoBPList();
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
            VibratoHandle result = new VibratoHandle();
            result.Index = Index;
            result.IconID = IconID;
            result.IDS = this.IDS;
            result.Original = this.Original;
            result.Caption = this.Caption;
            result.setLength( Length );
            result.StartDepth = this.StartDepth;
            result.DepthBP = (VibratoBPList)DepthBP.clone();
            result.StartRate = this.StartRate;
            result.RateBP = (VibratoBPList)RateBP.clone();
            return result;
        }

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.Vibrato;
            ret.Index = Index;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Original = Original;
            ret.Caption = Caption;
            ret.setLength( Length );
            ret.StartDepth = StartDepth;
            ret.StartRate = StartRate;
            ret.DepthBP = (VibratoBPList)DepthBP.clone();
            ret.RateBP = (VibratoBPList)RateBP.clone();
            return ret;
        }
    }

#if !JAVA
}
#endif
