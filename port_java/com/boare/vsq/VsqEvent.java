/*
 * VsqEvent.java
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

/// <summary>
/// vsqファイルのメタテキスト内に記述されるイベント。
/// </summary>
public class VsqEvent implements Comparable<VsqEvent>, Cloneable {
    public String tag;
    /// <summary>
    /// 内部で使用するインスタンス固有のID
    /// </summary>
    public int internalID;
    public int clock;
    public VsqID id;
    public UstEvent ustEvent = new UstEvent();

    public static boolean isXmlIgnored( String name ){
        if( name.equals( "internalID" ) ){
            return true;
        }else if( name.equals( "tag" ) ){
            return true;
        }
        return false;
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "clock" ) ){
            return "Clock";
        }else if( name.equals( "id" ) ){
            return "ID";
        }else if( name.equals( "ustEvent" ) ){
            return "UstEvent";
        }
        return name;
    }

    /// <summary>
    /// このオブジェクトのコピーを作成します
    /// </summary>
    /// <returns></returns>
    public Object clone() {
        VsqEvent ret = new VsqEvent( clock, (VsqID)id.clone() );
        ret.internalID = internalID;
        if ( ustEvent != null ) {
            ret.ustEvent = (UstEvent)ustEvent.clone();
        }
        ret.tag = tag;
        return ret;
    }

    public int compareTo( VsqEvent item ) {
        int ret = this.clock - item.clock;
        if ( ret == 0 ) {
            if ( this.id != null && item.id != null ) {
                return (int)this.id.type.ordinal() - (int)item.id.type.ordinal();
            } else {
                return ret;
            }
        } else {
            return ret;
        }
    }

    public VsqEvent( String line ) {
        String[] spl = line.split( "=" );
        clock = Integer.parseInt( spl[0] );
        if ( spl[1].equals( "EOS" ) ) {
            id = (VsqID)VsqID.EOS.clone();
        }
    }

    public VsqEvent(){
        this( 0, new VsqID() );
    }

    public VsqEvent( int clock_, VsqID id_ ) {
        clock = clock_;
        id = id_;
        internalID = 0;
    }
}
