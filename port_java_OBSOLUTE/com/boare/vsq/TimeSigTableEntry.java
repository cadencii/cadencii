/*
 * TimeSigTableEntry.java
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

public class TimeSigTableEntry implements Comparable<TimeSigTableEntry>, Cloneable {
    /// <summary>
    /// クロック数
    /// </summary>
    public int clock;
    /// <summary>
    /// 拍子の分子
    /// </summary>
    public int numerator;
    /// <summary>
    /// 拍子の分母
    /// </summary>
    public int denominator;
    /// <summary>
    /// 何小節目か
    /// </summary>
    public int barCount;

    public TimeSigTableEntry(
        int clock_,
        int numerator_,
        int denominator_,
        int bar_count ) {
        clock = clock_;
        numerator = numerator_;
        denominator = denominator_;
        barCount = bar_count;
    }

    public TimeSigTableEntry() {
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "clock" ) ){
            return "Clock";
        }else if( name.equals( "numerator" ) ){
            return "Numerator";
        }else if( name.equals( "denominator" ) ){
            return "Denominator";
        }else if( name.equals( "barCount" ) ){
            return "BarCount";
        }
        return name;
    }

    public String toString() {
        return "{Clock=" + clock + ", Numerator=" + numerator + ", Denominator=" + denominator + ", BarCount=" + barCount + "}";
    }

    public Object clone() {
        return new TimeSigTableEntry( clock, numerator, denominator, barCount );
    }

    public int compareTo( TimeSigTableEntry item ) {
        return this.barCount - item.barCount;
    }
}
