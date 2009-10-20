/*
 * SingerEventIterator.java
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

public class SingerEventIterator implements Iterator {
    VsqEventList m_list;
    int m_pos;

    public SingerEventIterator( VsqEventList list ) {
        m_list = list;
        m_pos = -1;
    }

    public boolean hasNext() {
        for ( int i = m_pos + 1; i < m_list.getCount(); i++ ) {
            if ( m_list.getElement( i ).id.type == VsqIDType.Singer ) {
                return true;
            }
        }
        return false;
    }

    public Object next() {
        for ( int i = m_pos + 1; i < m_list.getCount(); i++ ) {
            VsqEvent item = m_list.getElement( i );
            if ( item.id.type == VsqIDType.Singer ) {
                m_pos = i;
                return item;
            }
        }
        return null;
    }

    public void remove() {
        if ( 0 <= m_pos && m_pos < m_list.getCount() ) {
            m_list.removeAt( m_pos );
        }
    }
}
