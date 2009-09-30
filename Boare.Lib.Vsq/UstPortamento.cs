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
using System;
using System.Collections.Generic;
using System.IO;

using bocoree;

namespace Boare.Lib.Vsq {

    [Serializable]
    public class UstPortamento : ICloneable {
        public Vector<UstPortamentoPoint> Points = new Vector<UstPortamentoPoint>();
        public int Start;

        public void print( StreamWriter sw ) {
            String pbw = "";
            String pby = "";
            String pbm = "";
            for ( int i = 0; i < Points.size(); i++ ) {
                String comma = (i == 0 ? "" : ",");
                pbw += comma + Points.get( i ).Step;
                pby += comma + Points.get( i ).Value;
                String type = "";
                switch ( Points.get( i ).Type ) {
                    case UstPortamentoType.S:
                        type = "";
                        break;
                    case UstPortamentoType.Linear:
                        type = "s";
                        break;
                    case UstPortamentoType.R:
                        type = "r";
                        break;
                    case UstPortamentoType.J:
                        type = "j";
                        break;
                }
                pbm += comma + type;
            }
            sw.WriteLine( "PBW=" + pbw );
            sw.WriteLine( "PBS=" + Start );
            sw.WriteLine( "PBY=" + pby );
            sw.WriteLine( "PBM=" + pbm );
        }

        public object Clone() {
            UstPortamento ret = new UstPortamento();
            for ( int i = 0; i < Points.size(); i++ ) {
                ret.Points.add( Points.get( i ) );
            }
            ret.Start = Start;
            return ret;
        }

        /*
        PBW=50,50,46,48,56,50,50,50,50
        PBS=-87
        PBY=-15.9,-20,-31.5,-26.6
        PBM=,s,r,j,s,s,s,s,s
        */
        public void ParseLine( String line ) {
            line = line.ToLower();
            String[] spl = line.Split( '=' );
            if ( spl.Length == 0 ) {
                return;
            }
            String[] values = spl[1].Split( ',' );
            if ( line.StartsWith( "pbs=" ) ) {
                Start = int.Parse( values[0] );
            } else if ( line.StartsWith( "pbw=" ) ) {
                for ( int i = 0; i < values.Length; i++ ) {
                    if ( i >= Points.size() ) {
                        Points.add( new UstPortamentoPoint() );
                    }
                    UstPortamentoPoint up = Points.get( i );
                    up.Step = int.Parse( values[i] );
                    Points.set( i, up );
                }
            } else if ( line.StartsWith( "pby=" ) ) {
                for ( int i = 0; i < values.Length; i++ ) {
                    if ( i >= Points.size() ) {
                        Points.add( new UstPortamentoPoint() );
                    }
                    UstPortamentoPoint up = Points.get( i );
                    up.Value = float.Parse( values[i] );
                    Points.set( i, up );
                }
            } else if ( line.StartsWith( "pbm=" ) ) {
                for ( int i = 0; i < values.Length; i++ ) {
                    if ( i >= Points.size() ) {
                        Points.add( new UstPortamentoPoint() );
                    }
                    UstPortamentoPoint up = Points.get( i );
                    switch ( values[i].ToLower() ) {
                        case "s":
                            up.Type = UstPortamentoType.Linear;
                            break;
                        case "r":
                            up.Type = UstPortamentoType.R;
                            break;
                        case "j":
                            up.Type = UstPortamentoType.J;
                            break;
                        default:
                            up.Type = UstPortamentoType.S;
                            break;
                    }
                    Points.set( i, up );
                }
            } else if ( line.StartsWith( "pbs=" ) ) {

            }
        }
    }

    public struct UstPortamentoPoint {
        public int Step;
        public float Value;
        public UstPortamentoType Type;
    }

    public enum UstPortamentoType{
        /// <summary>
        /// S型．表記は''(空文字)
        /// </summary>
        S,
        /// <summary>
        /// 直線型．表記は's'
        /// </summary>
        Linear,
        /// <summary>
        /// R型．表記は'r'
        /// </summary>
        R,
        /// <summary>
        /// J型．表記は'j'
        /// </summary>
        J,
    }

}
