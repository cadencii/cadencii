/*
 * BPPair.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.io.*;
#else
using System;

namespace Boare.Lib.Vsq {
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
