/*
 * Range.cs
 * Copyright (c) 2009 kbinani
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
using System;

namespace org.kbinani.cadencii {

    public struct Range {
        private int m_start;
        private int m_end;

        public Range( int start, int end ) {
            m_start = Math.Min( start, end );
            m_end = Math.Max( start, end );
        }

        public int getStart() {
            return m_start;
        }

        public void setStart( int value ) {
            if ( m_end < value ) {
                m_start = m_end;
                m_end = value;
            } else {
                m_start = value;
            }
        }

        public int getEnd() {
            return m_end;
        }

        public void setEnd( int value ) {
            if ( value < m_start ) {
                m_end = m_start;
                m_start = value;
            } else {
                m_end = value;
            }
        }
    }

}
