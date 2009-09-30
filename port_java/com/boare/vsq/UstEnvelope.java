/*
 * UstEnvelope.java
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

public class UstEnvelope implements Cloneable {
    public int p1 = 0;
    public int p2 = 5;
    public int p3 = 35;
    public int v1 = 0;
    public int v2 = 100;
    public int v3 = 100;
    public int v4 = 0;
    public String separator = "";
    public int p4 = 0;
    public int p5 = 0;
    public int v5 = 100;

    public UstEnvelope() {
    }

    public UstEnvelope( String line ) {
        if ( line.toLowerCase().startsWith( "envelope=" ) ) {
            String[] spl = line.split( "=" );
            spl = spl[1].split( "," );
            if ( spl.length < 7 ) {
                return;
            }
            separator = "";
            p1 = Integer.parseInt( spl[0] );
            p2 = Integer.parseInt( spl[1] );
            p3 = Integer.parseInt( spl[2] );
            v1 = Integer.parseInt( spl[3] );
            v2 = Integer.parseInt( spl[4] );
            v3 = Integer.parseInt( spl[5] );
            v4 = Integer.parseInt( spl[6] );
            if ( spl.length == 11 ) {
                separator = "%";
                p4 = Integer.parseInt( spl[8] );
                p5 = Integer.parseInt( spl[9] );
                v5 = Integer.parseInt( spl[10] );
            }
        }
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "separator" ) ){
            return "Separator";
        }
        return name;
    }

    public Object clone() {
        return new UstEnvelope( toString() );
    }

    public String toString() {
        String ret = "Envelope=" + p1 + "," + p2 + "," + p3 + "," + v1 + "," + v2 + "," + v3 + "," + v4;
        if ( separator.equals( "%" ) ) {
            ret += ",%," + p4 + "," + p5 + "," + v5;
        }
        return ret;
    }

    public int getCount() {
        if ( separator.equals( "%" ) ) {
            return 5;
        } else {
            return 4;
        }
    }
}
