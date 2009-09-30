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
package com.boare.cadencii;

import java.util.*;

public class ClockResolution{
    private int m_value;
    private String m_name;
    public static final ClockResolution L1 = new ClockResolution( 1, "1" );
    public static final ClockResolution L2 = new ClockResolution( 2, "2" );
    public static final ClockResolution L4 = new ClockResolution( 4, "4" );
    public static final ClockResolution L8 = new ClockResolution( 8, "8" );
    public static final ClockResolution L16 = new ClockResolution( 16, "16" );
    public static final ClockResolution L30 = new ClockResolution( 30, "30" );
    public static final ClockResolution L60 = new ClockResolution( 60, "60" );
    public static final ClockResolution L90 = new ClockResolution( 90, "90" );
    public static final ClockResolution L120 = new ClockResolution( 120, "120" );
    public static final ClockResolution L240 = new ClockResolution( 240, "240" );
    public static final ClockResolution L480 = new ClockResolution( 480, "480" );
    public static final ClockResolution Free = new ClockResolution( 1, "Free" );

    public boolean equals( Object obj ) {
        if ( obj instanceof ClockResolution ) {
            return ((ClockResolution)obj).getValue() == m_value && ((ClockResolution)obj).m_name == m_name;
        } else {
            return false;
        }
    }

    public int getValue(){
        if ( m_value == 0 ) {
            m_value = 30;
            m_name = "30";
        }
        return m_value;
    }

    public static Iterator iterator(){
        ClockResolution[] arr = new ClockResolution[]{ L1, L2, L4, L8, L16, L30, L60, L90, L120, L240, L480, Free };
        return Arrays.asList( arr ).iterator();
    }

    private ClockResolution( int value, String name ) {
        m_value = value;
        m_name = name;
    }

    public String toString() {
        return m_name;
    }
}
