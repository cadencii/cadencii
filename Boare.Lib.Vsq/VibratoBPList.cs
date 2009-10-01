/*
 * VibratoBPList.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;

using bocoree;

namespace Boare.Lib.Vsq {

    [Serializable]
    public class VibratoBPList : ICloneable {
        private Vector<VibratoBPPair> m_list;

        public VibratoBPList() {
            m_list = new Vector<VibratoBPPair>();
        }

        public VibratoBPList( float[] x, int[] y ){
            if ( x == null ){
                throw new ArgumentNullException( "x" );
            }
            if ( y == null ){
                throw new ArgumentNullException( "y" );
            }
            int len = Math.Min( x.Length, y.Length );
            m_list = new Vector<VibratoBPPair>( len );
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
            for ( int i = 0; i < m_list.size(); i++ ) {
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

        public object Clone() {
            VibratoBPList ret = new VibratoBPList();
            for ( int i = 0; i < m_list.size(); i++ ) {
                ret.m_list.add( new VibratoBPPair( m_list.get( i ).X, m_list.get( i ).Y ) );
            }
            return ret;
        }

        public int getCount() {
            return m_list.size();
        }

        public VibratoBPPair getElement( int index ) {
            return m_list.get( index );
        }

        public void setElement( int index, VibratoBPPair value ) {
            m_list.set( index, value );
        }

        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public String Data {
            get {
                String ret = "";
                for ( int i = 0; i < m_list.size(); i++ ) {
                    ret += (i == 0 ? "" : ",") + m_list.get( i ).X + "=" + m_list.get( i ).Y;
                }
                return ret;
            }
            set {
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
    }

}