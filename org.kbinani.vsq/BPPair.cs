/*
 * BPPair.cs
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
#elif __cplusplus
namespace org{ namespace kbinani{ namespace vsq{
#else
using System;

namespace org.kbinani.vsq {
#endif

    /// <summary>
    /// ゲートタイムと、何らかのパラメータ値とのペアを表します。主にVsqBPListで使用します。
    /// </summary>
#if JAVA
    public class BPPair implements Comparable<BPPair>, Serializable{
#elif __cplusplus
    class BPPair{
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
#if __cplusplus
        };
#else
        }
#endif

#if JAVA
#elif __cplusplus
#else
        public int CompareTo( BPPair item ) {
            return compareTo( item );
        }
#endif

        /// <summary>
        /// 指定されたゲートタイムとパラメータ値を使って、新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="clock_"></param>
        /// <param name="value_"></param>
        public BPPair( int clock_, int value_ ) {
            Clock = clock_;
            Value = value_;
#if __cplusplus
        };
#else
        }
#endif
    };

#if JAVA
#elif __cplusplus
} } }
#else
}
#endif
