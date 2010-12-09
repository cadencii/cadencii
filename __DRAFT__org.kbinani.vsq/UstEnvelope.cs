/*
 * UstEnvelope.cs
 * Copyright (C) 2009-2010 kbinani
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
import org.kbinani.*;
#else
using System;

namespace org.kbinani.vsq {
#endif

#if JAVA
    public class UstEnvelope implements Cloneable, Serializable {
#else
    [Serializable]
    public class UstEnvelope : ICloneable {
#endif
        public int p1 = 0;
        public int p2 = 5;
        public int p3 = 35;
        public int v1 = 0;
        public int v2 = 100;
        public int v3 = 100;
        public int v4 = 0;
        //public String Separator = "";
        public int p4 = 0;
        public int p5 = 0;
        public int v5 = 100;

        public UstEnvelope() {
        }

        public UstEnvelope( String line ) {
            if ( line.ToLower().StartsWith( "envelope=" ) ) {
                String[] spl = PortUtil.splitString( line, '=' );
                spl = PortUtil.splitString( spl[1], ',' );
                if ( spl.Length < 7 ) {
                    return;
                }
                //Separator = "";
                p1 = PortUtil.parseInt( spl[0] );
                p2 = PortUtil.parseInt( spl[1] );
                p3 = PortUtil.parseInt( spl[2] );
                v1 = PortUtil.parseInt( spl[3] );
                v2 = PortUtil.parseInt( spl[4] );
                v3 = PortUtil.parseInt( spl[5] );
                v4 = PortUtil.parseInt( spl[6] );
                if ( spl.Length == 11 ) {
                    //Separator = "%";
                    p4 = PortUtil.parseInt( spl[8] );
                    p5 = PortUtil.parseInt( spl[9] );
                    v5 = PortUtil.parseInt( spl[10] );
                }
            }
        }

        public Object clone() {
            return new UstEnvelope( toString() );
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

#if !JAVA
        public override string ToString() {
            return toString();
        }
#endif

        public String toString() {
            String ret = "Envelope=" + p1 + "," + p2 + "," + p3 + "," + v1 + "," + v2 + "," + v3 + "," + v4;
            ret += ",%," + p4 + "," + p5 + "," + v5;
            return ret;
        }

        public int getCount() {
            //if ( Separator == "%" ) {
            return 5;
            //} else {
            //return 4;
            //}
        }
    }

#if !JAVA
}
#endif
