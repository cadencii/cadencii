/*
 * UstPortamento.java
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
import java.io.*;
import com.boare.corlib.*;

public class UstPortamento implements Cloneable {
    public Vector<UstPortamentoPoint> points = new Vector<UstPortamentoPoint>();
    public int start;

    public static String getXmlElementName( String name ){
        if( name.equals( "points" ) ){
            return "Points";
        }else if( name.equals( "start" ) ){
            return "Start";
        }
        return name;
    }

    public static String getGenericTypeName( String name ){
        if( name.equals( "points" ) ){
            return "com.boare.vsq.UstPortamentoPoint";
        }
        return "";
    }

    public void print( StreamWriter sw ) throws IOException{
        String pbw = "";
        String pby = "";
        String pbm = "";
        int c = points.size();
        for ( int i = 0; i < c; i++ ) {
            UstPortamentoPoint p = points.get( i );
            String comma = (i == 0 ? "" : ",");
            pbw += comma + p.step;
            pby += comma + p.value;
            String type = "";
            switch ( p.type ) {
                case S:
                    type = "";
                    break;
                case Linear:
                    type = "s";
                    break;
                case R:
                    type = "r";
                    break;
                case J:
                    type = "j";
                    break;
            }
            pbm += comma + type;
        }
        sw.writeLine( "PBW=" + pbw );
        sw.writeLine( "PBS=" + start );
        sw.writeLine( "PBY=" + pby );
        sw.writeLine( "PBM=" + pbm );
    }

    public Object clone() {
        UstPortamento ret = new UstPortamento();
        int c = points.size();
        for ( int i = 0; i < c; i++ ) {
            ret.points.add( points.get( i ) );
        }
        ret.start = start;
        return ret;
    }

    /*
    PBW=50,50,46,48,56,50,50,50,50
    PBS=-87
    PBY=-15.9,-20,-31.5,-26.6
    PBM=,s,r,j,s,s,s,s,s
    */
    public void parseLine( String line ) {
        line = line.toLowerCase();
        String[] spl = line.split( "=" );
        if ( spl.length == 0 ) {
            return;
        }
        String[] values = spl[1].split( "," );
        if ( line.startsWith( "pbs=" ) ) {
            start = Integer.parseInt( values[0] );
        } else if ( line.startsWith( "pbw=" ) ) {
            for ( int i = 0; i < values.length; i++ ) {
                if ( i >= points.size() ) {
                    points.add( new UstPortamentoPoint() );
                }
                points.get( i ).step = Integer.parseInt( values[i] );
            }
        } else if ( line.startsWith( "pby=" ) ) {
            for ( int i = 0; i < values.length; i++ ) {
                if ( i >= points.size() ) {
                    points.add( new UstPortamentoPoint() );
                }
                points.get( i ).value = Float.parseFloat( values[i] );
            }
        } else if ( line.startsWith( "pbm=" ) ) {
            for ( int i = 0; i < values.length; i++ ) {
                if ( i >= points.size() ) {
                    points.add( new UstPortamentoPoint() );
                }
                String s = values[i].toLowerCase();
                if( s.equals( "s" ) ){
                    points.get( i ).type = UstPortamentoType.Linear;
                }else if( s.equals( "r" ) ){
                    points.get( i ).type = UstPortamentoType.R;
                }else if( s.equals( "j" ) ){
                    points.get( i ).type = UstPortamentoType.J;
                }else{
                    points.get( i ).type = UstPortamentoType.S;
                }
            }
        } else if ( line.startsWith( "pbs=" ) ) {

        }
    }
}
