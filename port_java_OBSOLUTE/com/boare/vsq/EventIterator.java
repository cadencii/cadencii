/*
 * EventIterator.java
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

public class EventIterator implements Iterator{
    private VsqEventList m_list;
    private int m_pos;

    public EventIterator( VsqEventList list ) {
        m_list = list;
        m_pos = -1;
    }

    public boolean hasNext() {
        if ( 0 <= m_pos + 1 && m_pos + 1 < m_list.getCount() ) {
            return true;
        }
        return false;
    }

    public Object next() {
        m_pos++;
        return m_list.getElement( m_pos );
    }

    public void remove() {
        if ( 0 <= m_pos && m_pos < m_list.getCount() ) {
            m_list.removeAt( m_pos );
        }
    }
}
