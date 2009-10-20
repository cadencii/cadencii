/*
 * VibratoBPList.cs
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

import java.util.*;

public class VibratoBPList implements Cloneable {
    private Vector<VibratoBPPair> m_list;

    public VibratoBPList() {
        m_list = new Vector<VibratoBPPair>();
    }

    public VibratoBPList( float[] x, int[] y ){
        if ( x == null || y == null ){
            m_list = new Vector<VibratoBPPair>();
            return;
        }
        int len = Math.min( x.length, y.length );
        m_list = new Vector<VibratoBPPair>( len );
        for ( int i = 0; i < len; i++ ) {
            m_list.add( new VibratoBPPair( x[i], y[i] ) );
        }
        Comparator<VibratoBPPair> comp = new Comparator<VibratoBPPair>(){
            public int compare( VibratoBPPair obj1, VibratoBPPair obj2 ){
                VibratoBPPair v1 = (VibratoBPPair)obj1;
                VibratoBPPair v2 = (VibratoBPPair)obj2;
                float dif = v1.x - v2.x;
                if( dif == 0.0 ){
                    return 0;
                }else if( dif > 0.0 ){
                    return 1;
                }else{
                    return -1;
                }
            }
        };
        Collections.sort( m_list, comp );
    }

    public static boolean isXmlIgnored( String name ){
        if( name.equals( "Element" ) ){
            return true;
        }
        return false;
    }

    public int getValue( float x, int default_value ) {
        int c = m_list.size();
        if ( c <= 0 ) {
            return default_value;
        }
        int index = -1;
        for ( int i = 0; i < c; i++ ) {
            if ( x < m_list.get( i ).x ) {
                break;
            }
            index = i;
        }
        if ( index == -1 ) {
            return default_value;
        } else {
            return m_list.get( index ).y;
        }
    }

    public Object clone() {
        VibratoBPList ret = new VibratoBPList();
        int c = m_list.size();
        for ( int i = 0; i < c; i++ ) {
            VibratoBPPair v = m_list.get( i );
            ret.m_list.add( new VibratoBPPair( v.x, v.y ) );
        }
        return ret;
    }

    public int getCount() {
        return m_list.size();
    }

    public VibratoBPPair getElement( int index ) {
        return m_list.get( index );
    }

    public void setElement( int index, VibratoBPPair value ) {
        m_list.set( index, value );
    }

    /// <summary>
    /// XMLシリアライズ用
    /// </summary>
    public String getData(){
        String ret = "";
        int c = m_list.size();
        for ( int i = 0; i < c; i++ ) {
            VibratoBPPair v = m_list.get( i );
            ret += (i == 0 ? "" : ",") + v.x + "=" + v.y;
        }
        return ret;
    }

    public void setData( String value ){
        m_list.clear();
        String[] spl = value.split( "," );
        for ( int i = 0; i < spl.length; i++ ) {
            String[] spl2 = spl[i].split( "=");
            if ( spl2.length < 2 ) {
                continue;
            }
            try{
                float k = Float.parseFloat( spl2[0] );
                int v = Integer.parseInt( spl2[1] );
                m_list.add( new VibratoBPPair( k, v ) );
            }catch( Exception ex ){
            }
        }
    }
}
