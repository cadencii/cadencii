/*
 * BPPair.java
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of com.boare.vsq.
 *
 * com.boare.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.vsq;

/**
 * Stores the paired value of "Clock" and integer. Mainly used in VsqBPList
 */
public class BPPair implements Comparable<BPPair> {
    public int clock;
    public int value;

    /**
     * このインスタンスと、指定したオブジェクトを比較します
     *
     * @param item
     */
    public int compareTo( BPPair item ) {
        if ( clock > item.clock ) {
            return 1;
        } else if ( clock < item.clock ) {
            return -1;
        } else {
            return 0;
        }
    }

    public BPPair( int clock_, int value_ ) {
        clock = clock_;
        value = value_;
    }
}
