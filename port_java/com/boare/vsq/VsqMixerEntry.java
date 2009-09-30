/*
 * VsqMixerEntry.java
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

/// <summary>
/// VsqMixerのSlave要素に格納される各エントリ
/// </summary>
public class VsqMixerEntry implements Cloneable {
    public int feder;
    public int panpot;
    public int mute;
    public int solo;

    public static String getXmlElementName( String name ){
        if( name.equals( "feder" ) ){
            return "Feder";
        }else if( name.equals( "panpot" ) ){
            return "Panpot";
        }else if( name.equals( "mute" ) ){
            return "Mute";
        }else if( name.equals( "solo" ) ){
            return "Solo";
        }
        return name;
    }

    public Object clone() {
        return new VsqMixerEntry( feder, panpot, mute, solo );
    }

    /// <summary>
    /// 各パラメータを指定したコンストラクタ
    /// </summary>
    /// <param name="feder">Feder値</param>
    /// <param name="panpot">Panpot値</param>
    /// <param name="mute">Mute値</param>
    /// <param name="solo">Solo値</param>
    public VsqMixerEntry( int feder_, int panpot_, int mute_, int solo_ ) {
        this.feder = feder_;
        this.panpot = panpot_;
        this.mute = mute_;
        this.solo = solo_;
    }

    public VsqMixerEntry(){
        this( 0, 0, 0, 0 );
    }
}
