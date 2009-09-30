/*
 * IconHandle.java
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

public class IconHandle implements Cloneable {
    public String caption;
    public String iconID;
    public String IDS;
    public int index;
    public int length;
    public int original;
    public int program;
    public int language;

    public IconHandle() {
    }

    public static boolean isXmlIgnored( String name ){
        return false;
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "caption" ) ){
            return "Caption";
        }else if( name.equals( "iconID" ) ){
            return "IconID";
        }else if( name.equals( "index" ) ){
            return "Index";
        }else if( name.equals( "length" ) ){
            return "Length";
        }else if( name.equals( "original" ) ){
            return "Original";
        }else if( name.equals( "program" ) ){
            return "Program";
        }else if( name.equals( "language" ) ){
            return "Language";
        }
        return name;
    }

    public Object clone() {
        IconHandle ret = new IconHandle();
        ret.caption  = caption;
        ret.iconID   = iconID;
        ret.IDS      = IDS;
        ret.index    = index;
        ret.language = language;
        ret.length   = length;
        ret.original = original;
        ret.program  = program;
        return ret;
    }

    public VsqHandle castToVsqHandle() {
        VsqHandle ret = new VsqHandle();
        ret.m_type   = VsqHandleType.Singer;
        ret.caption  = caption;
        ret.iconID   = iconID;
        ret.IDS      = IDS;
        ret.index    = index;
        ret.language = language;
        ret.length   = length;
        ret.program  = program;
        return ret;
    }
}
