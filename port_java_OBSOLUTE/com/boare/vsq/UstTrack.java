/*
 * UstTrack.java
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

public class UstTrack implements Cloneable {
    public Object tag;
    private Vector<UstEvent> m_events;
    
    public UstTrack(){
        m_events = new Vector<UstEvent>();
    }

    public UstEvent getEvent( int index ) {
        return m_events.get( index );
    }

    public void setEvent( int index, UstEvent item ) {
        m_events.set( index, item );
    }

    public void addEvent( UstEvent item ) {
        m_events.add( item );
    }

    public void removeEvent( int index ) {
        m_events.removeElementAt( index );
    }

    public int getEventCount() {
        return m_events.size();
    }

    public Iterator getNoteEventIterator() {
        return m_events.iterator();
    }

    public Object clone() {
        UstTrack ret = new UstTrack();
        int c = m_events.size();
        for ( int i = 0; i < c; i++ ) {
            ret.m_events.add( (UstEvent)m_events.get( i ).clone() );
        }
        return ret;
    }
}
