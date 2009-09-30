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
using System;
using System.Collections.Generic;

using bocoree;

namespace Boare.Lib.Vsq{

    public class UstTrack : ICloneable {
        public object Tag;
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
            return new ListIterator<UstEvent>( m_events );
        }

        public object Clone() {
            UstTrack ret = new UstTrack();
            for ( int i = 0; i < m_events.size(); i++ ) {
                ret.m_events.set( i, (UstEvent)m_events.get( i ).Clone() );
            }
            return ret;
        }
    }

}
