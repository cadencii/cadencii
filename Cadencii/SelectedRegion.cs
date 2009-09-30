/*
 * SelectedRegion.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
namespace Boare.Cadencii {

    public class SelectedRegion {
        private int m_begin;
        private int m_end;

        public void SetEnd( int value ) {
            m_end = value;
        }


        public int Start {
            get {
                if ( m_end < m_begin ) {
                    return m_end;
                } else {
                    return m_begin;
                }
            }
        }
        
        
        public int End {
            get {
                if ( m_end < m_begin ) {
                    return m_begin;
                } else {
                    return m_end;
                }
            }
        }

        
        public SelectedRegion( int begin ) {
            m_begin = begin;
        }
    }

}
