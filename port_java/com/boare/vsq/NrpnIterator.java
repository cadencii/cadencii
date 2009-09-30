/*
 * NrpnIterator.java
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
import java.lang.reflect.*;

public class NrpnIterator implements Iterator {
    private Vector<Integer> nrpns = new Vector<Integer>();
    private int m_pos = -1;

    public NrpnIterator() {
        try{
            NRPN obj = new NRPN();
            Field[] fields = obj.getClass().getDeclaredFields();
            for( int i = 0; i < fields.length; i++ ){
                Field fi = fields[i];
                if ( fi.getType() == Integer.TYPE ) {
                    nrpns.add( (Integer)fi.get( obj ) );
                }
            }
        }catch( Exception ex ){
        }
    }

    public boolean hasNext() {
        if ( 0 <= m_pos + 1 && m_pos + 1 < nrpns.size() ) {
            return true;
        } else {
            return false;
        }
    }

    public Object next() {
        m_pos++;
        return nrpns.get( m_pos );
    }

    public void remove() {
    }
}
