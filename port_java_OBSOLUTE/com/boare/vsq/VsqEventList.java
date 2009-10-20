/*
 * VsqEventList.java
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

import java.util.*;

/// <summary>
/// 固有ID付きのVsqEventのリストを取り扱う
/// </summary>
public class VsqEventList {
    public Vector<VsqEvent> events;
    private Vector<Integer> m_ids;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public VsqEventList() {
        events = new Vector<VsqEvent>();
        m_ids = new Vector<Integer>();
    }

    public static String getGenericTypeName( String name ){
        if( name.equals( "events" ) ){
            return "com.boare.vsq.VsqEvent";
        }
        return "";
    }

    public static String getXmlElementName( String name ){
        if( name.equals( "events" ) ){
            return "Events";
        }
        return name;
    }

    public void sort() {
        Collections.sort( events );
        updateIDList();
    }

    public void clear() {
        events.clear();
        m_ids.clear();
    }

    public Iterator iterator() {
        updateIDList();
        return events.iterator();
    }

    public void add( VsqEvent item ) {
        updateIDList();
        int new_id = getNextId( 0 );
        item.internalID = new_id;
        events.add( item );
        m_ids.add( new_id );
        Collections.sort( events );
        int c = events.size();
        for ( int i = 0; i < c; i++ ) {
            m_ids.set( i, events.get( i ).internalID );
        }
    }

    public void removeAt( int index ) {
        updateIDList();
        events.removeElementAt( index );
        m_ids.removeElementAt( index );
    }

    private int getNextId( int next ) {
        updateIDList();
        int index = -1;
        Vector<Integer> current = new Vector<Integer>( m_ids );
        int nfound = 0;
        while ( true ) {
            index++;
            if ( !current.contains( index ) ) {
                nfound++;
                if ( nfound == next + 1 ) {
                    return index;
                } else {
                    current.add( index );
                }
            }
        }
    }

    public int getCount() {
        return events.size();
    }

    public VsqEvent getElement( int index ) {
        return events.get( index );
    }

    public void setElement( int index, VsqEvent value ) {
        events.set( index, value );
    }

    public void updateIDList() {
        if ( m_ids.size() != events.size() ) {
            m_ids.clear();
            int c = events.size();
            for ( int i = 0; i < c; i++ ) {
                m_ids.add( events.get( i ).internalID );
            }
        }else{
            int c = events.size();
            for( int i = 0; i < c; i++ ){
                m_ids.set( i, events.get( i ).internalID );
            }
        }
    }
}
