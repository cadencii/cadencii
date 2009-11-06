/*
 * UstTrack.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.util.*;
#else
using System;
using bocoree;
using bocoree.util;

namespace Boare.Lib.Vsq {
#endif

#if JAVA
    public class UstTrack implements Cloneable {
#else
    public class UstTrack : ICloneable {
#endif
        public Object Tag;
        private Vector<UstEvent> m_events;

        public UstTrack() {
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

        public void removeEventAt( int index ) {
            m_events.removeElementAt( index );
        }

        public int getEventCount() {
            return m_events.size();
        }

        public Iterator getNoteEventIterator() {
#if JAVA
            return m_events.iterator();
#else
            return new ListIterator<UstEvent>( m_events );
#endif
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
