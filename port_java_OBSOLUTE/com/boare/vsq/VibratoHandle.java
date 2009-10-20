/*
 * VibratoHandle.java
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

public class VibratoHandle implements Cloneable {
    public int startDepth;
    public VibratoBPList depthBP;
    public int startRate;
    public VibratoBPList rateBP;
    public int index;
    public String iconID;
    public String IDS;
    public int original;
    public String caption;
    public int length;

    public VibratoHandle(){
        startRate = 64;
        startDepth = 64;
        rateBP = new VibratoBPList();
        depthBP = new VibratoBPList();
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "startDepth" ) ){
            return "StartDepth";
        }else if( name.equals( "depthBP" ) ){
            return "DepthBP";
        }else if( name.equals( "startRate" ) ){
            return "StartRate";
        }else if( name.equals( "rateBP" ) ){
            return "RateBP";
        }else if( name.equals( "index" ) ){
            return "Index";
        }else if( name.equals( "iconID" ) ){
            return "IconID";
        }else if( name.equals( "original" ) ){
            return "Original";
        }else if( name.equals( "caption" ) ){
            return "Caption";
        }else if( name.equals( "length" ) ){
            return "Length";
        }
        return name;
    }

    public Object clone() {
        VibratoHandle result = new VibratoHandle();
        result.index = index;
        result.iconID = iconID;
        result.IDS = this.IDS;
        result.original = this.original;
        result.caption = this.caption;
        result.length = this.length;
        result.startDepth = this.startDepth;
        result.depthBP = (VibratoBPList)depthBP.clone();
        result.startRate = this.startRate;
        result.rateBP = (VibratoBPList)rateBP.clone();
        return result;
    }

    public VsqHandle castToVsqHandle() {
        VsqHandle ret = new VsqHandle();
        ret.m_type = VsqHandleType.Vibrato;
        ret.index = index;
        ret.iconID = iconID;
        ret.IDS = IDS;
        ret.original = original;
        ret.caption = caption;
        ret.length = length;
        ret.startDepth = startDepth;
        ret.startRate = startRate;
        ret.depthBP = (VibratoBPList)depthBP.clone();
        ret.rateBP = (VibratoBPList)rateBP.clone();
        return ret;
    }
}
