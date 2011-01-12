/*
 * VibratoBPList.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.util.*;
import java.io.*;
import org.kbinani.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.util;

namespace org.kbinani.vsq
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class VibratoBPList implements Cloneable, Serializable {
#else
    [Serializable]
    public class VibratoBPList : ICloneable
    {
#endif
        private Vector<VibratoBPPair> m_list;

        public VibratoBPList()
        {
            m_list = new Vector<VibratoBPPair>();
        }

        public VibratoBPList( String strNum, String strBPX, String strBPY )
        {
            int num = 0;
            try {
                num = PortUtil.parseInt( strNum );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "org.kbinani.vsq.VibratoBPList#.ctor; ex=" + ex );
                num = 0;
            }
            String[] bpx = PortUtil.splitString( strBPX, ',' );
            String[] bpy = PortUtil.splitString( strBPY, ',' );
            int actNum = Math.Min( num, Math.Min( bpx.Length, bpy.Length ) );
            if ( actNum > 0 ) {
                float[] x = new float[actNum];
                int[] y = new int[actNum];
                for ( int i = 0; i < actNum; i++ ) {
                    try {
                        x[i] = PortUtil.parseFloat( bpx[i] );
                        y[i] = PortUtil.parseInt( bpy[i] );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                    }
                }

                int len = Math.Min( x.Length, y.Length );
                m_list = new Vector<VibratoBPPair>( len );
                for ( int i = 0; i < len; i++ ) {
                    m_list.add( new VibratoBPPair( x[i], y[i] ) );
                }
                Collections.sort( m_list );
            } else {
                m_list = new Vector<VibratoBPPair>();
            }
        }

        public VibratoBPList( float[] x, int[] y )
#if JAVA
            throws NullPointerException
#endif
        {
            if ( x == null ) {
#if JAVA
                throw new NullPointerException( "x" );
#else
                throw new ArgumentNullException( "x" );
#endif
            }
            if ( y == null ) {
#if JAVA
                throw new NullPointerException( "y" );
#else
                throw new ArgumentNullException( "y" );
#endif
            }
            int len = Math.Min( x.Length, y.Length );
            m_list = new Vector<VibratoBPPair>( len );
            for ( int i = 0; i < len; i++ ) {
                m_list.add( new VibratoBPPair( x[i], y[i] ) );
            }
            Collections.sort( m_list );
        }

        /// <summary>
        /// このインスタンスと，指定したVibratoBPListのインスタンスが等しいかどうかを調べます
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public boolean equals( VibratoBPList item )
        {
            if ( item == null ) {
                return false;
            }
            int size = this.m_list.size();
            if ( size != item.m_list.size() ) {
                return false;
            }
            for ( int i = 0; i < size; i++ ) {
                VibratoBPPair p0 = this.m_list.get( i );
                VibratoBPPair p1 = item.m_list.get( i );
                if ( p0.X != p1.X ) {
                    return false;
                }
                if ( p0.Y != p1.Y ) {
                    return false;
                }
            }
            return true;
        }

        public int getValue( float x, int default_value )
        {
            if ( m_list.size() <= 0 ) {
                return default_value;
            }
            int index = -1;
            int size = m_list.size();
            for ( int i = 0; i < size; i++ ) {
                if ( x < m_list.get( i ).X ) {
                    break;
                }
                index = i;
            }
            if ( index == -1 ) {
                return default_value;
            } else {
                return m_list.get( index ).Y;
            }
        }

        public Object clone()
        {
            VibratoBPList ret = new VibratoBPList();
            for ( int i = 0; i < m_list.size(); i++ ) {
                ret.m_list.add( new VibratoBPPair( m_list.get( i ).X, m_list.get( i ).Y ) );
            }
            return ret;
        }

#if !JAVA
        public object Clone()
        {
            return clone();
        }
#endif

        public int getCount()
        {
            return m_list.size();
        }

        public VibratoBPPair getElement( int index )
        {
            return m_list.get( index );
        }

        public void setElement( int index, VibratoBPPair value )
        {
            m_list.set( index, value );
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public String Data
        {
            get
            {
                return getData();
            }
            set
            {
                setData( value );
            }
        }
#endif

        public String getData()
        {
            String ret = "";
            for ( int i = 0; i < m_list.size(); i++ ) {
                ret += (i == 0 ? "" : ",") + m_list.get( i ).X + "=" + m_list.get( i ).Y;
            }
            return ret;
        }

        public void setData( String value )
        {
            m_list.clear();
            String[] spl = PortUtil.splitString( value, ',' );
            for ( int i = 0; i < spl.Length; i++ ) {
                String[] spl2 = PortUtil.splitString( spl[i], '=' );
                if ( spl2.Length < 2 ) {
                    continue;
                }
                m_list.add( new VibratoBPPair( PortUtil.parseFloat( spl2[0] ), PortUtil.parseInt( spl2[1] ) ) );
            }
        }
    }

#if !JAVA
}
#endif
