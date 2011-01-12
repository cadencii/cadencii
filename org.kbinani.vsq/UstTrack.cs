/*
 * UstTrack.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.util.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.util;

namespace org.kbinani.vsq
{
#endif

#if JAVA
    public class UstTrack implements Cloneable {
#else
    public class UstTrack : ICloneable
    {
#endif
        public Object Tag;
        private Vector<UstEvent> m_events;

        public UstTrack()
        {
            m_events = new Vector<UstEvent>();
        }

        public UstEvent getEvent( int index )
        {
            return m_events.get( index );
        }

        public void setEvent( int index, UstEvent item )
        {
            m_events.set( index, item );
        }

        public void addEvent( UstEvent item )
        {
            m_events.add( item );
        }

        public void removeEventAt( int index )
        {
            m_events.removeElementAt( index );
        }

        public int getEventCount()
        {
            return m_events.size();
        }

        public Iterator<UstEvent> getNoteEventIterator()
        {
            return m_events.iterator();
        }

        public Object clone()
        {
            UstTrack ret = new UstTrack();
            int c = m_events.size();
            for ( int i = 0; i < c; i++ ) {
                ret.m_events.set( i, (UstEvent)m_events.get( i ).clone() );
            }
            return ret;
        }

#if !JAVA
        public object Clone()
        {
            return clone();
        }
#endif
    }

#if !JAVA
}
#endif
