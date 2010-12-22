#if !JAVA
/*
 * ListIterator.cs
 * Copyright © 2009-2010 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;

namespace org.kbinani.java.util {

    public class ListIterator<T> : Iterator<T> {
        private List<T> m_list;
        private int m_pos;

        public ListIterator( Vector<T> list ) {
            m_list = new List<T>();
            int c = list.size();
            for( int i = 0; i < c; i++ ){
                m_list.Add( list.get( i ) );
            }
            m_pos = -1;
        }

        public ListIterator( List<T> list ) {
            m_list = list;
            m_pos = -1;
        }

        public Boolean hasNext() {
            if ( m_list != null && 0 <= m_pos + 1 && m_pos + 1 < m_list.Count ) {
                return true;
            } else {
                return false;
            }
        }

        public T next() {
            if ( m_list == null ) {
                return default( T );
            }
            m_pos++;
            return m_list[m_pos];
        }

        public void remove() {
            m_list.RemoveAt( m_pos );
        }
    }

}
#endif
