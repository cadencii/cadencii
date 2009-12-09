/*
 * ClockResolution.cs
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

import java.util.*;
#else
using System;
using bocoree.java.util;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
#endif

    public struct ClockResolution {
        private int m_value;
        public static readonly ClockResolution L1 = new ClockResolution( 1 );
        public static readonly ClockResolution L2 = new ClockResolution( 2 );
        public static readonly ClockResolution L4 = new ClockResolution( 4 );
        public static readonly ClockResolution L8 = new ClockResolution( 8 );
        public static readonly ClockResolution L16 = new ClockResolution( 16 );
        public static readonly ClockResolution L30 = new ClockResolution( 30 );
        public static readonly ClockResolution L60 = new ClockResolution( 60 );
        public static readonly ClockResolution L90 = new ClockResolution( 90 );
        public static readonly ClockResolution L120 = new ClockResolution( 120 );
        public static readonly ClockResolution L240 = new ClockResolution( 240 );
        public static readonly ClockResolution L480 = new ClockResolution( 480 );
        public static readonly ClockResolution Free = new ClockResolution( 1 );

#if !JAVA
        public override boolean Equals( object obj ) {
            return equals( obj );
        }
#endif

        public boolean equals( Object obj ) {
            if ( obj is ClockResolution ) {
                return ((ClockResolution)obj).getValue() == m_value;
            } else {
                return false;
            }
        }

        public int getValue() {
            if ( m_value == 0 ) {
                m_value = 30;
            }
            return m_value;
        }

        public static Iterator iterator() {
            ClockResolution[] arr = new ClockResolution[] { L1, L2, L4, L8, L16, L30, L60, L90, L120, L240, L480, Free };
            return Arrays.asList( arr ).iterator();
        }

        private ClockResolution( int value ) {
            m_value = value;
        }

#if !JAVA
        public override string ToString() {
            return toString();
        }
#endif

        public String toString() {
            if ( m_value == 1 ) {
                return "Free";
            } else {
                return m_value + "";
            }
        }
    }

#if !JAVA
}
#endif
