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
using System;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

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
        
        public override boolean Equals( object obj ) {
            if ( obj is ClockResolution ) {
                return ((ClockResolution)obj).Value == m_value;
            } else {
                return false;
            }
        }

        public int Value {
            get {
                if ( m_value == 0 ) {
                    m_value = 30;
                }
                return m_value;
            }
        }
        
        public static System.Collections.Generic.IEnumerable<ClockResolution> GetEnumerator() {
            yield return L1;
            yield return L2;
            yield return L4;
            yield return L8;
            yield return L16;
            yield return L30;
            yield return L60;
            yield return L90;
            yield return L120;
            yield return L240;
            yield return L480;
            yield return Free;
        }

        private ClockResolution( int value ) {
            m_value = value;
        }

        public override String ToString() {
            if ( m_value == 1 ) {
                return "Free";
            } else {
                return m_value.ToString();
            }
        }
    }

}
