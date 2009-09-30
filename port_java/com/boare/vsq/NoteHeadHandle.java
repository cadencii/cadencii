/*
 * NoteHeadHandle.java
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

public class NoteHeadHandle implements Cloneable {
    public int index;
    public String iconID;
    public String IDS;
    public int original;
    public String caption;
    public int length;
    public int duration;
    public int depth;

    public NoteHeadHandle() {
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "index" ) ){
            return "Index";
        }else if( name.equals( "iconID" ) ){
            return "IconID";
        }else if( name.equals( "original" ) ){
            return "Original";
        }else if( name.equals( "caption" ) ){
            return "Caption";
        }else if( name.equals( "length" ) ){
            return "Length";
        }else if( name.equals( "duration" ) ){
            return "Duration";
        }else if( name.equals( "depth" ) ){
            return "Depth";
        }
        return name;
    }

    public Object clone() {
        NoteHeadHandle result = new NoteHeadHandle();
        result.index = index;
        result.iconID = iconID;
        result.IDS = this.IDS;
        result.original = this.original;
        result.caption = this.caption;
        result.length = this.length;
        result.duration = duration;
        result.depth = depth;
        return result;
    }

    public VsqHandle castToVsqHandle() {
        VsqHandle ret = new VsqHandle();
        ret.m_type = VsqHandleType.NoteHeadHandle;
        ret.index = index;
        ret.iconID = iconID;
        ret.IDS = IDS;
        ret.original = original;
        ret.caption = caption;
        ret.length = length;
        ret.duration = duration;
        ret.depth = depth;
        return ret;
    }
}
