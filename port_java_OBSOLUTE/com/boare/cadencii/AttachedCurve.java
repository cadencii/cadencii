/*
 * AttachedCurve.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.cadencii;

import java.util.*;

public class AttachedCurve implements Cloneable {
    public Vector<BezierCurves> curves = new Vector<BezierCurves>();

    public static String getXmlElementName( String name ){
        if( name.equals( "curves" ) ){
            return "Curves";
        }
        return name;
    }

    public static String getGenericTypeName( String name ){
        if( name.equals( "curves" ) ){
            return "com.boare.cadencii.BezierCurves";
        }
        return "";
    }

    public void add( BezierCurves item ) {
        curves.add( item );
    }

    public void removeElementAt( int index ) {
        curves.removeElementAt( index );
    }

    public void insert( int position, BezierCurves attached_curve ) {
        curves.insertElementAt( attached_curve, position );
    }

    public Object clone() {
        AttachedCurve ret = new AttachedCurve();
        ret.curves.clear();
        for ( int i = 0; i < curves.size(); i++ ) {
            ret.curves.add( (BezierCurves)curves.get( i ).clone() );
        }
        return ret;
    }

    public BezierCurves get( int index ){
        return curves.get( index );
    }

    public void set( int index, BezierCurves value ){
        curves.set( index, value );
    }
}
