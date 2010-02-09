/*
 * VsqEventList.cs
 * Copyright (C) 2008-2010 kbinani
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

import java.io.*;
import java.util.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.util;

namespace org.kbinani.vsq {
    using Integer = System.Int32;
#endif

    /// <summary>
    /// 固有ID付きのVsqEventのリストを取り扱う
    /// </summary>
#if JAVA
    public class VsqEventList implements Serializable {
#else
    [Serializable]
    public class VsqEventList {
#endif
        public Vector<VsqEvent> Events;
        private Vector<Integer> m_ids;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VsqEventList() {
            Events = new Vector<VsqEvent>();
            m_ids = new Vector<Integer>();
        }

        public VsqEvent findFromID( int internal_id ) {
            for ( Iterator itr = Events.iterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( item.InternalID == internal_id ) {
                    return item;
                }
            }
            return null;
        }

        public void setForID( int internal_id, VsqEvent value ) {
            int c = Events.size();
            for ( int i = 0; i < c; i++ ) {
                if ( Events.get( i ).InternalID == internal_id ) {
                    Events.set( i, value );
                    break;
                }
            }
        }

        public void sort() {
            lock ( this ) {
                Collections.sort( Events );
                updateIDList();
            }
        }

        public void clear() {
            Events.clear();
            m_ids.clear();
        }

        public Iterator iterator() {
            updateIDList();
            return Events.iterator();
        }

        public void add( VsqEvent item ) {
            add( item, getNextId( 0 ) );
            Collections.sort( Events );
            int count = Events.size();
            for ( int i = 0; i < count; i++ ) {
                m_ids.set( i, Events.get( i ).InternalID );
            }
        }

        public void add( VsqEvent item, int internal_id ) {
            updateIDList();
            item.InternalID = internal_id;
            Events.add( item );
            m_ids.add( internal_id );
        }

        public void removeAt( int index ) {
            updateIDList();
            Events.removeElementAt( index );
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
            return Events.size();
        }

        public VsqEvent getElement( int index ) {
            return Events.get( index );
        }

        public void setElement( int index, VsqEvent value ) {
            value.InternalID = Events.get( index ).InternalID;
            Events.set( index, value );
        }

        public void updateIDList() {
            if ( m_ids.size() != Events.size() ) {
                m_ids.clear();
                int count = Events.size();
                for ( int i = 0; i < count; i++ ) {
                    m_ids.add( Events.get( i ).InternalID );
                }
            } else {
                int count = Events.size();
                for ( int i = 0; i < count; i++ ) {
                    m_ids.set( i, Events.get( i ).InternalID );
                }
            }
        }
    }

#if !JAVA
}
#endif
