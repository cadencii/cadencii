/*
 * UstPortamento.cs
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
#if JAVA
package org.kbinani.vsq;

import java.util.*;
import java.io.*;
import org.kbinani.*;
#else
using System;
using bocoree;
using bocoree.util;
using bocoree.io;

namespace Boare.Lib.Vsq {
#endif

#if JAVA
    public class UstPortamento implements Cloneable, Serializable {
#else
    [Serializable]
    public class UstPortamento : ICloneable {
#endif
        public Vector<UstPortamentoPoint> Points = new Vector<UstPortamentoPoint>();
        public int Start;

        public void print( BufferedWriter sw )
#if JAVA
            throws IOException
#endif
        {
            String pbw = "";
            String pby = "";
            String pbm = "";
            int count = Points.size();
            for ( int i = 0; i < count; i++ ) {
                String comma = (i == 0 ? "" : ",");
                pbw += comma + Points.get( i ).Step;
                pby += comma + Points.get( i ).Value;
                String type = "";
                UstPortamentoType ut = Points.get( i ).Type;
                if ( ut == UstPortamentoType.S ) {
                    type = "";
                } else if ( ut == UstPortamentoType.Linear ) {
                    type = "s";
                } else if ( ut == UstPortamentoType.R ) {
                    type = "r";
                } else if ( ut == UstPortamentoType.J ) {
                    type = "j";
                }
                pbm += comma + type;
            }
            sw.write( "PBW=" + pbw );
            sw.newLine();
            sw.write( "PBS=" + Start );
            sw.newLine();
            sw.write( "PBY=" + pby );
            sw.newLine();
            sw.write( "PBM=" + pbm );
            sw.newLine();
        }

        public Object clone() {
            UstPortamento ret = new UstPortamento();
            int count = Points.size();
            for ( int i = 0; i < count; i++ ) {
                ret.Points.add( Points.get( i ) );
            }
            ret.Start = Start;
            return ret;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

        /*
        PBW=50,50,46,48,56,50,50,50,50
        PBS=-87
        PBY=-15.9,-20,-31.5,-26.6
        PBM=,s,r,j,s,s,s,s,s
        */
        public void ParseLine( String line ) {
            line = line.ToLower();
            String[] spl = PortUtil.splitString( line, '=' );
            if ( spl.Length == 0 ) {
                return;
            }
            String[] values = PortUtil.splitString( spl[1], ',' );
            if ( line.StartsWith( "pbs=" ) ) {
                Start = PortUtil.parseInt( values[0] );
            } else if ( line.StartsWith( "pbw=" ) ) {
                for ( int i = 0; i < values.Length; i++ ) {
                    if ( i >= Points.size() ) {
                        Points.add( new UstPortamentoPoint() );
                    }
                    UstPortamentoPoint up = Points.get( i );
                    up.Step = PortUtil.parseInt( values[i] );
                    Points.set( i, up );
                }
            } else if ( line.StartsWith( "pby=" ) ) {
                for ( int i = 0; i < values.Length; i++ ) {
                    if ( i >= Points.size() ) {
                        Points.add( new UstPortamentoPoint() );
                    }
                    UstPortamentoPoint up = Points.get( i );
                    up.Value = PortUtil.parseFloat( values[i] );
                    Points.set( i, up );
                }
            } else if ( line.StartsWith( "pbm=" ) ) {
                for ( int i = 0; i < values.Length; i++ ) {
                    if ( i >= Points.size() ) {
                        Points.add( new UstPortamentoPoint() );
                    }
                    UstPortamentoPoint up = Points.get( i );
                    String search = values[i].ToLower();
                    if ( search.Equals( "s" ) ) {
                        up.Type = UstPortamentoType.Linear;
                    } else if ( search.Equals( "r" ) ) {
                        up.Type = UstPortamentoType.R;
                    } else if ( search.Equals( "j" ) ) {
                        up.Type = UstPortamentoType.J;
                    } else {
                        up.Type = UstPortamentoType.S;
                    }
                    Points.set( i, up );
                }
            } else if ( line.StartsWith( "pbs=" ) ) {

            }
        }
    }

#if !JAVA
}
#endif
