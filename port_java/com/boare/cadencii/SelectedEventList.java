/*
 * SelectedEventList.cs
 * Copyright (c) 2009 kbinani
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

public class SelectedEventList{
    private Vector<SelectedEventEntry> m_list = new Vector<SelectedEventEntry>();

    public Iterator iterator(){
        return m_list.iterator();
    }

    public int getCount(){
        return m_list.size();
    }

    public int getLastSelectedID(){
        if ( m_list.size() <= 0 ) {
            return -1;
        } else {
            return m_list.get( m_list.size() - 1 ).original.internalID;
        }
    }

    public void clear() {
        m_list.clear();
    }

    public SelectedEventEntry getLastSelected(){
        if ( m_list.size() <= 0 ) {
            return null;
        } else {
            return m_list.get( m_list.size() - 1 );
        }
    }

    public void add( SelectedEventEntry ve ) {
        int c = m_list.size();
        for ( int i = 0; i < c; i++ ) {
            if ( m_list.get( i ).original.internalID == ve.original.internalID ) {
                m_list.removeElementAt( i );
                break;
            }
        }
        m_list.add( ve );
    }

    public void remove( int id ) {
        int c = m_list.size();
        for ( int i = 0; i < c; i++ ) {
            if ( m_list.get( i ).original.internalID == id ) {
                m_list.removeElementAt( i );
                return;
            }
        }
    }

    public void removeRange( Integer[] ids ) {
        List<Integer> list = Arrays.asList( ids );
        for ( Iterator itr = m_list.iterator(); itr.hasNext(); ){
            SelectedEventEntry item = (SelectedEventEntry)itr.next();
            if ( list.contains( item.original.internalID ) ) {
                itr.remove();
            }
        }
    }

    public boolean containsKey( int track, int id ) {
        int c = m_list.size();
        for ( int i = 0; i < c; i++ ) {
            SelectedEventEntry item = m_list.get( i );
            if ( item.track == track && item.original.internalID == id ) {
                return true;
            }
        }
        return false;
    }
}
