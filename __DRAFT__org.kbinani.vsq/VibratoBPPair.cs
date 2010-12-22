/*
 * VibratoBPPair.cs
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
#else
using System;

namespace org.kbinani.vsq {
#endif

#if JAVA
    public class VibratoBPPair implements Comparable<VibratoBPPair>, Serializable {
#else
    [Serializable]
    public struct VibratoBPPair : IComparable<VibratoBPPair> {
#endif
        public float X;
        public int Y;

        public VibratoBPPair( float x, int y ) {
            X = x;
            Y = y;
        }

        public int compareTo( VibratoBPPair item ) {
            float v = X - item.X;
            if ( v > 0.0f ) {
                return 1;
            } else if ( v < 0.0f ) {
                return -1;
            }
            return 0;
        }

#if !JAVA
        public int CompareTo( VibratoBPPair item ) {
            return compareTo( item );
        }
#endif
    }

#if !JAVA
}
#endif
