/*
 * UstTrack.cs
 * Copyright (C) 2009-2010 kbinani
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
using System.Collections.Generic;

namespace org.kbinani.vsq {
#endif

#if JAVA
    public class UstTrack implements Cloneable {
#else
    public class UstTrack : ICloneable {
#endif
        public Object Tag;
        private List<UstEvent> m_events;

        public UstTrack() {
            m_events = new List<UstEvent>();
        }

        public UstEvent getEvent( int index ) {
#if JAVA
            return m_events.get( index );
#else
            return m_events[index];
#endif
        }

        public void setEvent( int index, UstEvent item ) {
#if JAVA
            m_events.set( index, item );
#else
            m_events[index] = item;
#endif
        }

        public void addEvent( UstEvent item ) {
#if JAVA
            m_events.add( item );
#elif __cplusplus
            m_events.push_back( item );
#else
            m_events.Add( item );
#endif
        }

        public void removeEventAt( int index ) {
#if JAVA
            m_events.removeElementAt( index );
#elif __cplusplus
            vector<UstEvent>::iterator it = m_events.begin();
            std::advance( it, index );
            m_events.erase( it );
#else
            m_events.RemoveAt( index );
#endif
        }

        public int getEventCount() {
#if JAVA
            return m_events.size();
#elif __cplusplus
            return m_events.size();
#else
            return m_events.Count;
#endif
        }

        public Iterator<UstEvent> getNoteEventIterator() {
            return m_events.iterator();
        }

        public Object clone() {
            UstTrack ret = new UstTrack();
            int c = m_events.size();
            for ( int i = 0; i < c; i++ ) {
                ret.m_events.set( i, (UstEvent)m_events.get( i ).clone() );
            }
            return ret;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif
    }

#if !JAVA
}
#endif
