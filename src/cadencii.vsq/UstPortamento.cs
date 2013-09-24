/*
 * UstPortamento.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii.vsq;

import java.util.*;
import java.io.*;
import cadencii.*;
import cadencii.xml.*;

#else
using System;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;

namespace cadencii.vsq
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class UstPortamento implements Cloneable, Serializable
#else
    [Serializable]
    public class UstPortamento : ICloneable
#endif
    {
#if JAVA
        @XmlGenericType( UstPortamentoPoint.class )
#endif
        public Vector<UstPortamentoPoint> Points = new Vector<UstPortamentoPoint>();
        public int Start;
        /// <summary>
        /// PBSの末尾のセミコロンの後ろについている整数
        /// </summary>
        private int mUnknownInt;
        /// <summary>
        /// mUnknownIntが設定されているかどうか
        /// </summary>
        private boolean mIsUnknownIntSpecified = false;

        /// <summary>
        /// このクラスの指定した名前のプロパティをXMLシリアライズする際に使用する
        /// 要素名を取得します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String getXmlElementName( String name )
        {
            return name;
        }

        public void print( ITextWriter sw )
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
                pby += Points.get( i ).Value + ",";
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
            sw.write( "PBS=" + Start + (mIsUnknownIntSpecified ? (";" + mUnknownInt) : "") );
            sw.newLine();
            if ( Points.Count >= 2 ) {
                sw.write( "PBY=" + pby );
                sw.newLine();
                sw.write( "PBM=" + pbm );
                sw.newLine();
            }
        }

        public Object clone()
        {
            UstPortamento ret = new UstPortamento();
            int count = Points.size();
            for ( int i = 0; i < count; i++ ) {
                ret.Points.add( Points.get( i ) );
            }
            ret.Start = Start;
            ret.mIsUnknownIntSpecified = mIsUnknownIntSpecified;
            ret.mUnknownInt = mUnknownInt;
            return ret;
        }

#if !JAVA
        public object Clone()
        {
            return clone();
        }
#endif

        /*
        PBW=50,50,46,48,56,50,50,50,50
        PBS=-87
        PBY=-15.9,-20,-31.5,-26.6
        PBM=,s,r,j,s,s,s,s,s
        */
        public void parseLine( String line )
        {
            line = line.ToLower();
            String[] spl = PortUtil.splitString( line, '=' );
            if ( spl.Length == 0 ) {
                return;
            }
            String[] values = PortUtil.splitString( spl[1], ',' );
            if ( line.StartsWith( "pbs=" ) ) {
                String v = values[0];
                int indx = values[0].IndexOf( ";", 0 );
                if ( indx >= 0 ) {
                    v = values[0].Substring( 0, indx );
                    if ( values[0].Length > indx + 1 ) {
                        String unknown = values[0].Substring( indx + 1 );
                        mIsUnknownIntSpecified = true;
                        mUnknownInt = int.Parse( unknown );
                    }
                }
                Start = int.Parse( v );
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
                    if ( values[i].Length <= 0 ) {
                        continue;
                    }
                    if ( i >= Points.size() ) {
                        Points.add( new UstPortamentoPoint() );
                    }
                    UstPortamentoPoint up = Points.get( i );
                    up.Value = (float)double.Parse( values[i] );
                    Points.set( i, up );
                }
            } else if ( line.StartsWith( "pbm=" ) ) {
                for ( int i = 0; i < values.Length; i++ ) {
                    if ( i >= Points.size() ) {
                        Points.add( new UstPortamentoPoint() );
                    }
                    UstPortamentoPoint up = Points.get( i );
                    String search = values[i].ToLower();
                    if ( search == "s" ) {
                        up.Type = UstPortamentoType.Linear;
                    } else if ( search == "r" ) {
                        up.Type = UstPortamentoType.R;
                    } else if ( search == "j" ) {
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
