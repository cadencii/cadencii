/*
 * SelectedRegion.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii;
#else
namespace cadencii {
#endif

    public class SelectedRegion {
        private int m_begin;
        private int m_end;

        public void setEnd( int value ) {
            m_end = value;
        }

        public int getStart() {
            if ( m_end < m_begin ) {
                return m_end;
            } else {
                return m_begin;
            }
        }

        public int getEnd() {
            if ( m_end < m_begin ) {
                return m_begin;
            } else {
                return m_end;
            }
        }
        
        public SelectedRegion( int begin ) {
            m_begin = begin;
        }
    }

#if !JAVA
}
#endif
