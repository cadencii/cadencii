/*
 * PencilMode.cs
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
#if JAVA
package org.kbinani.Cadencii;
#else
using System;

namespace Boare.Cadencii
{
    using boolean = Boolean;
#endif

    public struct PencilMode
    {
        private PencilModeEnum m_mode;
        private boolean m_triplet;
        private boolean m_dot;

        public int getUnitLength()
        {
            int b = (int)m_mode;
            if ( m_triplet )
            {
                b = b * 2 / 3;
            }
            else if ( m_dot )
            {
                b = b + b / 2;
            }
            return b;
        }

        public PencilModeEnum getMode()
        {
            return m_mode;
        }

        public void setMode( PencilModeEnum value )
        {
            m_mode = value;
        }

        public boolean isTriplet()
        {
            return m_triplet;
        }

        public void setTriplet( boolean value )
        {
            m_triplet = value;
            if ( m_triplet && m_dot )
            {
                m_dot = false;
            }
        }

        public boolean isDot()
        {
            return m_dot;
        }

        public void setDot( boolean value )
        {
            m_dot = value;
            if ( m_dot && m_triplet )
            {
                m_triplet = false;
            }
        }
    }

#if !JAVA
}
#endif
