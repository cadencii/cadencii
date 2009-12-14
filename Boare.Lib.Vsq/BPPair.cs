/*
 * BPPair.cs
 * Copyright (c) 2008-2009 kbinani
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

    /// <summary>
    /// Stores the paired value of "Clock" and integer. Mainly used in VsqBPList
    /// </summary>
#if JAVA
    public class BPPair implements Comparable<BPPair>, Serializable{
#else
    [Serializable]
    public class BPPair : IComparable<BPPair>{
#endif
        public int Clock;
        public int Value;

        /// <summary>
        /// このインスタンスと、指定したオブジェクトを比較します
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int compareTo( BPPair item ) {
            if ( Clock > item.Clock ) {
                return 1;
            } else if ( Clock < item.Clock ) {
                return -1;
            } else {
                return 0;
            }
        }

#if !JAVA
        public int CompareTo( BPPair item ) {
            return compareTo( item );
        }
#endif

        public BPPair( int clock_, int value_ ) {
            Clock = clock_;
            Value = value_;
        }
    }

#if !JAVA
}
#endif
