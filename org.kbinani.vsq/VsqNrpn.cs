/*
 * VsqNrpn.cs
 * Copyright © 2008-2011 kbinani
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
import org.kbinani.*;

#else

using System;
using com.github.cadencii;
using com.github.cadencii.java.util;

namespace com.github.cadencii.vsq
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class VsqNrpn implements Comparable<VsqNrpn>
#else
    public class VsqNrpn : IComparable<VsqNrpn>
#endif
    {
        public int Clock;
        public int Nrpn;
        public byte DataMsb;
        public byte DataLsb;
        public boolean DataLsbSpecified;
        public boolean msbOmitRequired;
        private Vector<VsqNrpn> m_list;

        public VsqNrpn( int clock, int nrpn, byte data_msb )
        {
            Clock = clock;
            Nrpn = nrpn;
            DataMsb = data_msb;
            DataLsb = 0x0;
            DataLsbSpecified = false;
            msbOmitRequired = false;
            m_list = new Vector<VsqNrpn>();
        }

        public VsqNrpn( int clock, int nrpn, byte data_msb, byte data_lsb )
        {
            Clock = clock;
            Nrpn = nrpn;
            DataMsb = data_msb;
            DataLsb = data_lsb;
            DataLsbSpecified = true;
            msbOmitRequired = false;
            m_list = new Vector<VsqNrpn>();
        }

        public VsqNrpn[] expand()
        {
            Vector<VsqNrpn> ret = new Vector<VsqNrpn>();
            if ( DataLsbSpecified ) {
                VsqNrpn v = new VsqNrpn( Clock, Nrpn, DataMsb, DataLsb );
                v.msbOmitRequired = msbOmitRequired;
                ret.add( v );
            } else {
                VsqNrpn v = new VsqNrpn( Clock, Nrpn, DataMsb );
                v.msbOmitRequired = msbOmitRequired;
                ret.add( v );
            }
            for ( int i = 0; i < m_list.size(); i++ ) {
                ret.addAll( Arrays.asList( m_list.get( i ).expand() ) );
            }
            return ret.toArray( new VsqNrpn[] { } );
        }

        public static Vector<VsqNrpn> sort( Vector<VsqNrpn> list )
        {
            Vector<VsqNrpn> ret = new Vector<VsqNrpn>();
            Collections.sort( list );
            int list_size = vec.size( list );
            if ( list_size >= 2 ) {
                Vector<VsqNrpn> work = new Vector<VsqNrpn>(); //workには、clockが同じNRPNだけが入る
                int last_clock = vec.get( list, 0 ).Clock;
                work.add( vec.get( list, 0 ) );
                for ( int i = 1; i < list_size; i++ ) {
                    VsqNrpn itemi = vec.get( list, i );
                    if ( itemi.Clock == last_clock ) {
                        work.add( itemi );
                    } else {
                        // まずworkを並べ替え
                        last_clock = itemi.Clock;
                        boolean changed = true;
                        int work_size = vec.size( work );
                        while ( changed ) {
                            changed = false;
                            for ( int j = 0; j < work_size - 1; j++ ) {
                                VsqNrpn itemj = vec.get( work, j );
                                VsqNrpn itemjn = vec.get( work, j + 1 );
#if JAVA
                                int nrpn_msb0 = (itemj.Nrpn >>> 8) & 0xff;
                                int nrpn_msb1 = (itemjn.Nrpn >>> 8) & 0xff;
#else
                                int nrpn_msb0 = (itemj.Nrpn >> 8) & 0xff;
                                int nrpn_msb1 = (itemjn.Nrpn >> 8) & 0xff;
#endif
                                if ( nrpn_msb1 > nrpn_msb0 ) {
                                    VsqNrpn buf = itemj;
                                    vec.set( work, j, itemjn );
                                    vec.set( work, j + 1, buf );
                                    changed = true;
                                }
                            }
                        }
                        for ( int j = 0; j < work_size; j++ ) {
                            vec.add( ret, vec.get( work, j ) );
                        }
                        vec.clear( work );
                        vec.add( work, vec.get( list, i ) );
                    }
                }
                for ( int j = 0; j < work.size(); j++ ) {
                    vec.add( ret, vec.get( work, j ) );
                }
            } else {
                for ( int i = 0; i < list.size(); i++ ) {
                    vec.add( ret, vec.get( list, i ) );
                }
            }
            return ret;
        }

        public static VsqNrpn[] merge( VsqNrpn[] src1, VsqNrpn[] src2 )
        {
            Vector<VsqNrpn> ret = new Vector<VsqNrpn>();
            for ( int i = 0; i < src1.Length; i++ ) {
                ret.add( src1[i] );
            }
            for ( int i = 0; i < src2.Length; i++ ) {
                ret.add( src2[i] );
            }
            Collections.sort( ret );
            return ret.toArray( new VsqNrpn[] { } );
        }

        public static NrpnData[] convert( VsqNrpn[] source )
        {
            int nrpn = source[0].Nrpn;
            byte msb = (byte)(nrpn >> 8);
            byte lsb = (byte)(nrpn - (nrpn << 8));
            Vector<NrpnData> ret = new Vector<NrpnData>();
            ret.add( new NrpnData( source[0].Clock, (byte)0x63, msb ) );
            ret.add( new NrpnData( source[0].Clock, (byte)0x62, lsb ) );
            ret.add( new NrpnData( source[0].Clock, (byte)0x06, source[0].DataMsb ) );
            if ( source[0].DataLsbSpecified ) {
                ret.add( new NrpnData( source[0].Clock, (byte)0x26, source[0].DataLsb ) );
            }
            for ( int i = 1; i < source.Length; i++ ) {
                VsqNrpn item = source[i];
                int tnrpn = item.Nrpn;
                msb = (byte)(tnrpn >> 8);
                lsb = (byte)(tnrpn - (tnrpn << 8));
                if ( item.msbOmitRequired ) {
                    ret.add( new NrpnData( item.Clock, (byte)0x62, lsb ) );
                    ret.add( new NrpnData( item.Clock, (byte)0x06, item.DataMsb ) );
                    if ( item.DataLsbSpecified ) {
                        ret.add( new NrpnData( item.Clock, (byte)0x26, item.DataLsb ) );
                    }
                } else {
                    ret.add( new NrpnData( item.Clock, (byte)0x63, msb ) );
                    ret.add( new NrpnData( item.Clock, (byte)0x62, lsb ) );
                    ret.add( new NrpnData( item.Clock, (byte)0x06, item.DataMsb ) );
                    if ( item.DataLsbSpecified ) {
                        ret.add( new NrpnData( item.Clock, (byte)0x26, item.DataLsb ) );
                    }
                }
            }
            return ret.toArray( new NrpnData[] { } );
        }

        public int compareTo( VsqNrpn item )
        {
            return Clock - item.Clock;
        }

#if !JAVA
        public int CompareTo( VsqNrpn item )
        {
            return compareTo( item );
        }
#endif

        public void append( int nrpn, byte data_msb )
        {
            m_list.add( new VsqNrpn( Clock, nrpn, data_msb ) );
        }

        public void append( int nrpn, byte data_msb, byte data_lsb )
        {
            m_list.add( new VsqNrpn( Clock, nrpn, data_msb, data_lsb ) );
        }

        public void append( int nrpn, byte data_msb, boolean msb_omit_required )
        {
            VsqNrpn v = new VsqNrpn( Clock, nrpn, data_msb );
            v.msbOmitRequired = msb_omit_required;
            m_list.add( v );
        }

        public void append( int nrpn, byte data_msb, byte data_lsb, boolean msb_omit_required )
        {
            VsqNrpn v = new VsqNrpn( Clock, nrpn, data_msb, data_lsb );
            v.msbOmitRequired = msb_omit_required;
            m_list.add( v );
        }
    }

#if !JAVA
}
#endif
