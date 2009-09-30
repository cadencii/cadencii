/*
 * LyricHandle.java
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

public class LyricHandle implements Cloneable {
    public Lyric L0;
    public int index;

    public LyricHandle() {
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "index" ) ){
            return "Index";
        }
        return name;
    }

    /// <summary>
    /// type = Lyric用のhandleのコンストラクタ
    /// </summary>
    /// <param name="phrase">歌詞</param>
    /// <param name="phonetic_symbol">発音記号</param>
    public LyricHandle( String phrase, String phonetic_symbol ) {
        L0 = new Lyric( phrase, phonetic_symbol );
    }

    public Object clone() {
        LyricHandle ret = new LyricHandle();
        ret.index = index;
        ret.L0 = (Lyric)L0.clone();
        return ret;
    }

    public VsqHandle castToVsqHandle() {
        VsqHandle ret = new VsqHandle();
        ret.m_type = VsqHandleType.Lyric;
        ret.L0 = (Lyric)L0.clone();
        ret.index = index;
        return ret;
    }
}
