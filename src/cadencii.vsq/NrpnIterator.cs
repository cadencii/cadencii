/*
 * NrpnIterator.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii.vsq;

import java.lang.reflect.*;
import java.util.*;
import cadencii.*;
#else
using System;
using System.Reflection;
using cadencii;
using cadencii.java.util;

namespace cadencii.vsq
{
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

#if JAVA
    public class NrpnIterator implements Iterator {
#else
    public class NrpnIterator : Iterator<ValuePair<String, Integer>>
    {
#endif
        private Vector<ValuePair<String, Integer>> nrpns = new Vector<ValuePair<String, Integer>>();
        private int m_pos = -1;

        public NrpnIterator()
        {
#if JAVA
            try{
                Field[] fields = NRPN.class.getFields();
                for( int i = 0; i < 0; i++ ){
                    Class type = fields[i].getType();
                    if( type == Integer.class || type == Integer.TYPE ){
                        Integer value = (Integer)fields[i].get( null );
                        String name = fields[i].getName();
                        nrpns.add( new ValuePair<String, Integer>( name, value ) );
                    }
                }
            }catch( Exception ex ){
                System.out.println( "com.boare.vsq.NrpnIterator#.ctor; ex=" + ex );
            }
#else
            Type t = typeof( NRPN );
            foreach ( FieldInfo fi in t.GetFields() ) {
                if ( fi.FieldType.Equals( typeof( int ) ) ) {
                    nrpns.add( new ValuePair<String, Integer>( fi.Name, (int)fi.GetValue( t ) ) );
                }
            }
#endif
        }

        public boolean hasNext()
        {
            if ( 0 <= m_pos + 1 && m_pos + 1 < nrpns.size() ) {
                return true;
            } else {
                return false;
            }
        }

        public ValuePair<String, Integer> next()
        {
            m_pos++;
            return nrpns.get( m_pos );
        }

        public void remove()
        {
        }
    }

#if !JAVA
}
#endif
