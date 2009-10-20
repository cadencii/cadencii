/*
 * VibratoBPPair.java
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

import java.io.*;

public class VibratoBPPair implements Comparable<VibratoBPPair>, Serializable {
    public float x;
    public int y;

    public VibratoBPPair() {
    }

    public VibratoBPPair( float x_, int y_ ) {
        x = x_;
        y = y_;
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "x" ) ){
            return "X";
        }else if( name.equals( "y" ) ){
            return "Y";
        }
        return name;
    }

    public int compareTo( VibratoBPPair item ) {
        float v = x - item.x;
        if ( v > 0.0f ) {
            return 1;
        } else if ( v < 0.0f ) {
            return -1;
        }
        return 0;
    }
}
