/*
 * UstVibrato.java
 * Copyright (c) 2009 kbinani
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

public class UstVibrato implements Cloneable {
    /// <summary>
    /// 音符の長さに対する、パーセントで表したビブラートの長さ。
    /// </summary>
    public float length;
    /// <summary>
    /// ミリセカンドで表したビブラートによるピッチ振動の周期
    /// </summary>
    public float period;
    /// <summary>
    /// Centで表した、ビブラートによるピッチ振動の振幅。Peak to Peakは2*Depthとなる。
    /// </summary>
    public float depth;
    /// <summary>
    /// ビブラート長さに対する、パーセントで表したピッチ振動のフェードインの長さ。
    /// </summary>
    public float in;
    /// <summary>
    /// ビブラートの長さに対するパーセントで表したピッチ振動のフェードアウトの長さ。
    /// </summary>
    public float out;
    /// <summary>
    /// ピッチ振動開始時の位相。2PIに対するパーセントで表す。
    /// </summary>
    public float phase;
    /// <summary>
    /// ピッチ振動の中心値と、音符の本来の音の高さからのずれ。Depthに対するパーセントで表す。
    /// </summary>
    public float shift;
    public float unknown = 100;

    public UstVibrato( String line ) {
        if ( line.toLowerCase().startsWith( "vbr=" ) ) {
            String[] spl = line.split( "=" );
            spl = spl[1].split( "," );
            //VBR=65,180,70,20.0,17.6,82.8,49.8,100
            if ( spl.length >= 8 ) {
                length = Float.parseFloat( spl[0] );
                period = Float.parseFloat( spl[1] );
                depth = Float.parseFloat( spl[2] );
                in = Float.parseFloat( spl[3] );
                out = Float.parseFloat( spl[4] );
                phase = Float.parseFloat( spl[5] );
                shift = Float.parseFloat( spl[6] );
                unknown = Float.parseFloat( spl[7] );
            }
        }
    }

    public UstVibrato() {
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "length" ) ){
            return "Length";
        }else if( name.equals( "period" ) ){
            return "Period";
        }else if( name.equals( "depth" ) ){
            return "Depth";
        }else if( name.equals( "in" ) ){
            return "In";
        }else if( name.equals( "out" ) ){
            return "Out";
        }else if( name.equals( "phase" ) ){
            return "Phase";
        }else if( name.equals( "shift" ) ){
            return "Shift";
        }else if( name.equals( "unknown" ) ){
            return "Unknown";
        }
        return name;
    }

    public String toString() {
        return "VBR=" + length + "," + period + "," + depth + "," + in + "," + out + "," + phase + "," + shift + "," + unknown;
    }

    public Object clone() {
        UstVibrato ret = new UstVibrato();
        ret.length  = length;
        ret.period  = period;
        ret.depth   = depth;
        ret.in      = in;
        ret.out     = out;
        ret.phase   = phase;
        ret.shift   = shift;
        ret.unknown = unknown;
        return ret;
    }
}
