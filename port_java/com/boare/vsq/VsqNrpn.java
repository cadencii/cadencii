/*
 * VsqNrpn.java
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of com.boare.vsq.
 *
 * com.boare.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.vsq;

import java.util.*;

public class VsqNrpn implements Comparable<VsqNrpn> {
    public int clock;
    public int nrpn;
    public int dataMsb;
    public int dataLsb;
    public boolean dataLsbSpecified;
    public boolean msbOmitRequired = false;
    private Vector<VsqNrpn> m_list;

    public VsqNrpn( int clock_, int nrpn_, int data_msb ) {
        clock = clock_;
        nrpn = nrpn_;
        dataMsb = data_msb;
        dataLsb = 0x0;
        dataLsbSpecified = false;
        m_list = new Vector<VsqNrpn>();
    }

    public VsqNrpn( int clock_, int nrpn_, int data_msb, int data_lsb ) {
        clock = clock_;
        nrpn = nrpn_;
        dataMsb = data_msb;
        dataLsb = data_lsb;
        dataLsbSpecified = true;
        m_list = new Vector<VsqNrpn>();
    }

    public VsqNrpn[] expand() {
        Vector<VsqNrpn> ret = new Vector<VsqNrpn>();
        if ( dataLsbSpecified ) {
            ret.add( new VsqNrpn( clock, nrpn, dataMsb, dataLsb ) );
        } else {
            ret.add( new VsqNrpn( clock, nrpn, dataMsb ) );
        }
        int c = m_list.size();
        for ( int i = 0; i < c; i++ ) {
            ret.addAll( Arrays.asList( m_list.get( i ).expand() ) );
        }
        return ret.toArray( new VsqNrpn[]{} );
    }

    public static Vector<VsqNrpn> sort( Vector<VsqNrpn> list ) {
        Vector<VsqNrpn> ret = new Vector<VsqNrpn>();
        Collections.sort( list );
        if ( list.size() >= 2 ) {
            Vector<VsqNrpn> work = new Vector<VsqNrpn>(); //workには、clockが同じNRPNだけが入る
            int last_clock = list.get( 0 ).clock;
            work.add( list.get( 0 ) );
            int c = list.size();
            for ( int i = 1; i < c; i++ ) {
                if ( list.get( i ).clock == last_clock ) {
                    work.add( list.get( i ) );
                } else {
                    // まずworkを並べ替え
                    last_clock = list.get( i ).clock;
                    boolean changed = true;
                    while ( changed ) {
                        changed = false;
                        int c2 = work.size();
                        for ( int j = 0; j < c2 - 1; j++ ) {
                            byte nrpn_msb0 = (byte)((work.get( j ).nrpn >> 8) & 0xff);
                            byte nrpn_msb1 = (byte)((work.get( j + 1 ).nrpn >> 8) & 0xff);
                            if ( nrpn_msb1 > nrpn_msb0 ) {
                                VsqNrpn buf = work.get( j );
                                work.set( j, work.get( j + 1 ) );
                                work.set( j + 1, buf );
                                changed = true;
                            }
                        }
                    }
                    int c2 = work.size();
                    for ( int j = 0; j < c2; j++ ) {
                        ret.add( work.get( j ) );
                    }
                    work.clear();
                    work.add( list.get( i ) );
                }
            }
            for ( int j = 0; j < work.size(); j++ ) {
                ret.add( work.get( j ) );
            }
        } else {
            int list_size = list.size();
            for ( int i = 0; i < list_size; i++ ) {
                ret.add( list.get( i ) );
            }
        }
        return ret;
    }

    public static VsqNrpn[] merge( VsqNrpn[] src1, VsqNrpn[] src2 ) {
        Vector<VsqNrpn> ret = new Vector<VsqNrpn>();
        for ( int i = 0; i < src1.length; i++ ) {
            ret.add( src1[i] );
        }
        for ( int i = 0; i < src2.length; i++ ) {
            ret.add( src2[i] );
        }
        Collections.sort( ret );
        return ret.toArray( new VsqNrpn[]{} );
    }

    public static NrpnData[] convert( VsqNrpn[] source ) {
        int nrpn = source[0].nrpn;
        int msb = nrpn >>> 8;
        int lsb = nrpn - (nrpn << 8);
        Vector<NrpnData> ret = new Vector<NrpnData>();
        ret.add( new NrpnData( source[0].clock, 0x63, msb ) );
        ret.add( new NrpnData( source[0].clock, 0x62, lsb ) );
        ret.add( new NrpnData( source[0].clock, 0x06, source[0].dataMsb ) );
        if ( source[0].dataLsbSpecified ) {
            ret.add( new NrpnData( source[0].clock, 0x26, source[0].dataLsb ) );
        }
        int last_msb = msb;
        for ( int i = 1; i < source.length; i++ ) {
            VsqNrpn item = source[i];
            int tnrpn = item.nrpn;
            msb = tnrpn >>> 8;
            lsb = tnrpn - (tnrpn << 8);
            if ( item.msbOmitRequired ) {
                ret.add( new NrpnData( item.clock, 0x62, lsb ) );
                ret.add( new NrpnData( item.clock, 0x06, item.dataMsb ) );
                if ( item.dataLsbSpecified ) {
                    ret.add( new NrpnData( item.clock, 0x26, item.dataLsb ) );
                }
            } else {
                ret.add( new NrpnData( item.clock, 0x63, msb ) );
                ret.add( new NrpnData( item.clock, 0x62, lsb ) );
                ret.add( new NrpnData( item.clock, 0x06, item.dataMsb ) );
                if ( item.dataLsbSpecified ) {
                    ret.add( new NrpnData( item.clock, 0x26, item.dataLsb ) );
                }
            }
        }
        return ret.toArray( new NrpnData[]{} );
    }

    public int compareTo( VsqNrpn item ) {
        return clock - item.clock;
    }

    public void append( int nrpn, int data_msb ) {
        m_list.add( new VsqNrpn( clock, nrpn, data_msb ) );
    }

    public void append( int nrpn, int data_msb, int data_lsb ) {
        m_list.add( new VsqNrpn( clock, nrpn, data_msb, data_lsb ) );
    }
    
    public void append( int nrpn, int data_msb, boolean msb_omit_required ) {
        VsqNrpn v = new VsqNrpn( clock, nrpn, data_msb );
        v.msbOmitRequired = msb_omit_required;
        m_list.add( v );
    }

    public void append( int nrpn, int data_msb, int data_lsb, boolean msb_omit_required ) {
        VsqNrpn v = new VsqNrpn( clock, nrpn, data_msb, data_lsb );
        v.msbOmitRequired = msb_omit_required;
        m_list.add( v );
    }
}
