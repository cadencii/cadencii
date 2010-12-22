/*
 * VsqNrpn.cs
 * Copyright (C) 2008-2010 kbinani
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
#else
using System;
using System.Collections.Generic;

namespace org.kbinani.vsq {
    using boolean = System.Boolean;
#endif

#if JAVA
    public class VsqNrpn implements Comparable<VsqNrpn> {
#else
    public class VsqNrpn : IComparable<VsqNrpn> {
#endif
        public int Clock;
        public int Nrpn;
        public byte DataMsb;
        public byte DataLsb;
        public boolean DataLsbSpecified;
        public boolean msbOmitRequired;
        private List<VsqNrpn> m_list;

        public VsqNrpn( int clock, int nrpn, byte data_msb ) {
            Clock = clock;
            Nrpn = nrpn;
            DataMsb = data_msb;
            DataLsb = 0x0;
            DataLsbSpecified = false;
            msbOmitRequired = false;
            m_list = new List<VsqNrpn>();
        }

        public VsqNrpn( int clock, int nrpn, byte data_msb, byte data_lsb ) {
            Clock = clock;
            Nrpn = nrpn;
            DataMsb = data_msb;
            DataLsb = data_lsb;
            DataLsbSpecified = true;
            msbOmitRequired = false;
            m_list = new List<VsqNrpn>();
        }

        public List<VsqNrpn> expand() {
            List<VsqNrpn> ret = new List<VsqNrpn>();
            if ( DataLsbSpecified ) {
                VsqNrpn v = new VsqNrpn( Clock, Nrpn, DataMsb, DataLsb );
                v.msbOmitRequired = msbOmitRequired;
                vec.add( ret, v );
            } else {
                VsqNrpn v = new VsqNrpn( Clock, Nrpn, DataMsb );
                v.msbOmitRequired = msbOmitRequired;
                vec.add( ret, v );
            }
            for ( int i = 0; i < vec.size( m_list ); i++ ) {
                List<VsqNrpn> expanded = vec.get( m_list, i ).expand();
                for ( int j = 0; j < vec.size( expanded ); j++ ) {
                    vec.add( ret, vec.get( expanded, j ) );
                }
            }
            return ret;
        }

        public static List<VsqNrpn> sort( List<VsqNrpn> list ) {
            List<VsqNrpn> ret = new List<VsqNrpn>();
            Collections.sort( list );
            if ( vec.size( list ) >= 2 ) {
                List<VsqNrpn> work = new List<VsqNrpn>(); //workには、clockが同じNRPNだけが入る
                int last_clock = vec.get( list, 0 ).Clock;
                vec.add( work, vec.get( list, 0 ) );
                for ( int i = 1; i < vec.size( list ); i++ ) {
                    if ( vec.get( list, i ).Clock == last_clock ) {
                        vec.add( work, vec.get( list, i ) );
                    } else {
                        // まずworkを並べ替え
                        last_clock = vec.get( list, i ).Clock;
                        boolean changed = true;
                        while ( changed ) {
                            changed = false;
                            for ( int j = 0; j < vec.size( work ) - 1; j++ ) {
                                byte nrpn_msb0 = (byte)((vec.get( work, j ).Nrpn >> 8) & 0xff);
                                byte nrpn_msb1 = (byte)((vec.get( work, j + 1 ).Nrpn >> 8) & 0xff);
                                if ( nrpn_msb1 > nrpn_msb0 ) {
                                    VsqNrpn buf = vec.get( work, j );
                                    vec.set( work, j, vec.get( work, j + 1 ) );
                                    vec.set( work, j + 1, buf );
                                    changed = true;
                                }
                            }
                        }
                        for ( int j = 0; j < vec.size( work ); j++ ) {
                            vec.add( ret, vec.get( work, j ) );
                        }
                        vec.clear( work );
                        vec.add( work, vec.get( list, i ) );
                    }
                }
                for ( int j = 0; j < vec.size( work ); j++ ) {
                    vec.add( ret, vec.get( work, j ) );
                }
            } else {
                for ( int i = 0; i < vec.size( list ); i++ ) {
                    vec.add( ret, vec.get( list, i ) );
                }
            }
            return ret;
        }

        public static List<VsqNrpn> merge( VsqNrpn[] src1, VsqNrpn[] src2 ) {
            List<VsqNrpn> ret = new List<VsqNrpn>();
            for ( int i = 0; i < src1.Length; i++ ) {
                vec.add( ret, src1[i] );
            }
            for ( int i = 0; i < src2.Length; i++ ) {
                vec.add( ret, src2[i] );
            }
            Collections.sort( ret );
            return ret;
        }

        public static List<NrpnData> convert( VsqNrpn[] source ) {
            int nrpn = source[0].Nrpn;
            byte msb = (byte)(nrpn >> 8);
            byte lsb = (byte)(nrpn - (nrpn << 8));
            List<NrpnData> ret = new List<NrpnData>();
            vec.add( ret, new NrpnData( source[0].Clock, (byte)0x63, msb ) );
            vec.add( ret, new NrpnData( source[0].Clock, (byte)0x62, lsb ) );
            vec.add( ret, new NrpnData( source[0].Clock, (byte)0x06, source[0].DataMsb ) );
            if ( source[0].DataLsbSpecified ) {
                vec.add( ret, new NrpnData( source[0].Clock, (byte)0x26, source[0].DataLsb ) );
            }
            for ( int i = 1; i < source.Length; i++ ) {
                VsqNrpn item = source[i];
                int tnrpn = item.Nrpn;
                msb = (byte)(tnrpn >> 8);
                lsb = (byte)(tnrpn - (tnrpn << 8));
                if ( item.msbOmitRequired ) {
                    vec.add( ret, new NrpnData( item.Clock, (byte)0x62, lsb ) );
                    vec.add( ret, new NrpnData( item.Clock, (byte)0x06, item.DataMsb ) );
                    if ( item.DataLsbSpecified ) {
                        vec.add( ret, new NrpnData( item.Clock, (byte)0x26, item.DataLsb ) );
                    }
                } else {
                    vec.add( ret, new NrpnData( item.Clock, (byte)0x63, msb ) );
                    vec.add( ret, new NrpnData( item.Clock, (byte)0x62, lsb ) );
                    vec.add( ret, new NrpnData( item.Clock, (byte)0x06, item.DataMsb ) );
                    if ( item.DataLsbSpecified ) {
                        vec.add( ret, new NrpnData( item.Clock, (byte)0x26, item.DataLsb ) );
                    }
                }
            }
            return ret;
        }

        public int compareTo( VsqNrpn item ) {
            return Clock - item.Clock;
        }

#if !JAVA
        public int CompareTo( VsqNrpn item ) {
            return compareTo( item );
        }
#endif

        public void append( int nrpn, byte data_msb ) {
            vec.add( m_list, new VsqNrpn( Clock, nrpn, data_msb ) );
        }

        public void append( int nrpn, byte data_msb, byte data_lsb ) {
            vec.add( m_list, new VsqNrpn( Clock, nrpn, data_msb, data_lsb ) );
        }

        public void append( int nrpn, byte data_msb, boolean msb_omit_required ) {
            VsqNrpn v = new VsqNrpn( Clock, nrpn, data_msb );
            v.msbOmitRequired = msb_omit_required;
            vec.add( m_list, v );
        }

        public void append( int nrpn, byte data_msb, byte data_lsb, boolean msb_omit_required ) {
            VsqNrpn v = new VsqNrpn( Clock, nrpn, data_msb, data_lsb );
            v.msbOmitRequired = msb_omit_required;
            vec.add( m_list, v );
        }
    }

#if !JAVA
}
#endif
