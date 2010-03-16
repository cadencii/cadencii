#if ENABLE_PROPERTY
/*
 * LengthProperty.cs
 * Copyright (C) 2010 kbinani
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
using System.ComponentModel;
using org.kbinani;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;

    [TypeConverter( typeof( LengthPropertyConverter ) )]
    public class LengthProperty {
        private String m_value = "0";
        private int m_int = 0;

        public LengthProperty()
            : this( 0 ) {
        }

        public LengthProperty( int value ) {
            m_int = value;
            m_value = "" + value;
        }

        public boolean equals( Object obj ) {
            if ( obj is LengthProperty ) {
                if ( m_int == ((LengthProperty)obj).m_int ) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return base.Equals( obj );
            }
        }

        public override bool Equals( object obj )
        {
            return equals( obj );
        }

        public String getStr() {
            return m_value;
        }

        public void setStr( String value ) {
            int i = m_int;
            try {
                i = (int)AppManager.eval( 0.0, value );
                String trim = value.Trim();
                if ( trim.StartsWith( "-" ) ) {
                    m_int -= i;
                } else if ( trim.StartsWith( "+" ) ) {
                    m_int += i;
                } else {
                    m_int = i;
                }
                m_value = "" + m_int;
            } catch ( Exception ex ) {
#if DEBUG
                PortUtil.println( "CalculatableString#set_str; ex=" + ex );
#endif
            }
        }

        public int getIntValue() {
            return m_int;
        }

    }

}
#endif
