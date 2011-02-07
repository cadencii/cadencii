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
import org.kbinani.*;

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

        /// <summary>
        /// 指定したindex値を持つイベントを検索します
        /// </summary>
        /// <param name="index">検索するindex値</param>
        /// <returns>見つからなかったらnullを返す</returns>
        public UstEvent findEventFromIndex( int index )
        {
            foreach ( UstEvent ue in m_events ) {
                if ( ue.Index == index ) {
                    return ue;
                }
            }
            return null;
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
            int c = vec.size( m_events );
            for ( int i = 0; i < c; i++ ) {
                vec.add( ret.m_events, (UstEvent)vec.get( m_events, i ).clone() );
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
