/*
 * VibratoBPList.cs
 * Copyright (C) 2009-2010 kbinani
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
using System.Collections.Generic;

namespace org.kbinani.vsq {
#endif

#if JAVA
    public class VibratoBPList implements Cloneable, Serializable {
#else
    [Serializable]
    public class VibratoBPList : ICloneable {
#endif
        private List<VibratoBPPair> m_list;

        public VibratoBPList() {
            m_list = new List<VibratoBPPair>();
        }

        public VibratoBPList( String strNum, String strBPX, String strBPY ) {
            int num = 0;
            try {
                num = str.toi( strNum );
            } catch ( Exception ex ) {
                serr.println( "org.kbinani.vsq.VibratoBPList#.ctor; ex=" + ex );
                num = 0;
            }
            List<string> bpx = new List<string>();
            int bpx_size = str.split( strBPX, bpx, ",", false );
            List<string> bpy = new List<string>();
            int bpy_size = str.split( strBPY, bpy, ",", false );
            int actNum = Math.Min( num, Math.Min( bpx_size, bpy_size ) );
            if ( actNum > 0 ) {
#if __cplusplus
                float *x = new float[actNum];
                int *y = new int[actNum];
#else
                float[] x = new float[actNum];
                int[] y = new int[actNum];
#endif
                for ( int i = 0; i < actNum; i++ ) {
                    try {
                        x[i] = (float)str.tof( vec.get( bpx, i ) );
                        y[i] = str.toi( vec.get( bpy, i ) );
                    } catch ( Exception ex ) {
                        serr.println( "org.kbinani.vsq.IconParameter#.ctor; ex=" + ex );
                    }
                }

#if JAVA
                m_list = new Vector<VibratoBPPair>();
#elif __cplusplus
#else
                m_list = new List<VibratoBPPair>();
#endif
                for ( int i = 0; i < actNum; i++ ) {
#if __cplusplus
                    VibratoBPPair a( x[i], y[i] );
                    vec.add( m_list, a );
#else
                    vec.add( m_list, new VibratoBPPair( x[i], y[i] ) );
#endif
                }
#if __cplusplus
                delete [] x;
                delete [] y;
#endif
                vec.sort( m_list );
            } else {
#if !__cplusplus
                m_list = new List<VibratoBPPair>();
#endif
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
            m_list = new List<VibratoBPPair>( len );
            for ( int i = 0; i < len; i++ ) {
                m_list.add( new VibratoBPPair( x[i], y[i] ) );
            }
            Collections.sort( m_list );
        }

        public int getValue( float x, int default_value ) {
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

        public Object clone() {
            VibratoBPList ret = new VibratoBPList();
            for ( int i = 0; i < m_list.size(); i++ ) {
                ret.m_list.add( new VibratoBPPair( m_list.get( i ).X, m_list.get( i ).Y ) );
            }
            return ret;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

        public int getCount() {
            return m_list.size();
        }

        public VibratoBPPair getElement( int index ) {
            return m_list.get( index );
        }

        public void setElement( int index, VibratoBPPair value ) {
            m_list.set( index, value );
        }

#if !JAVA
        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public String Data {
            get {
                return getData();
            }
            set {
                setData( value );
            }
        }
#endif

        public String getData() {
            String ret = "";
            for ( int i = 0; i < m_list.size(); i++ ) {
                ret += (i == 0 ? "" : ",") + m_list.get( i ).X + "=" + m_list.get( i ).Y;
            }
            return ret;
        }

        public void setData( String value ) {
            m_list.clear();
            String[] spl = PortUtil.splitString( value, ',' );
            for ( int i = 0; i < spl.Length; i++ ) {
                String[] spl2 = PortUtil.splitString( spl[i], '=' );
                if ( spl2.Length < 2 ) {
                    continue;
                }
                m_list.add( new VibratoBPPair( (float)str.tof( spl2[0] ), (float)str.toi( spl2[1] ) ) );
            }
        }
    }

#if !JAVA
}
#endif
