/*
 * UtauFreq.cs
 * Copyright © 2009-2010 kbinani
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
#if JAVA
package org.kbinani.cadencii;

import java.io.*;
import org.kbinani.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.io;

namespace org.kbinani.cadencii {
#endif

    public class UtauFreq {
        public String Header;
        public int SampleInterval;
        public double AverageFrequency;
        public int NumPoints;
        public double[] Frequency;
        public double[] Volume;

        private UtauFreq() {
        }

        /*public static UtauFreq FromWav( String file ) {
            throw new NotImplementedException();
            return null;
        }*/

        /// <summary>
        /// *.frqファイルからのコンストラクタ
        /// </summary>
        /// <param name="file"></param>
        public static UtauFreq FromFrq( String file ){
            UtauFreq ret = new UtauFreq();
            FileInputStream fs = null;
            try {
                fs = new FileInputStream( file );
                byte[] buf0 = new byte[8];
                fs.read( buf0, 0, 8 );
                char[] ch8 = new char[8];
                for ( int i = 0; i < 8; i++ ) {
                    ch8[i] = (char)buf0[i];
                }
                ret.Header = new String( ch8 );

                fs.read( buf0, 0, 4 );
                ret.SampleInterval = PortUtil.make_int32_le( buf0 );

                fs.read( buf0, 0, 8 );
                ret.AverageFrequency = PortUtil.make_double_le( buf0 );

                for ( int i = 0; i < 4; i++ ) {
                    int len2 = fs.read( buf0, 0, 4 );
                    int i1 = PortUtil.make_int32_le( buf0 );
                }
                fs.read( buf0, 0, 4 );
                ret.NumPoints = PortUtil.make_int32_le( buf0 );
                ret.Frequency = new double[ret.NumPoints];
                ret.Volume = new double[ret.NumPoints];
                byte[] buf = new byte[16];
                int len = fs.read( buf, 0, 16 );
                int index = 0;
                while ( len > 0 ) {
                    double d1 = PortUtil.make_double_le( buf );
                    for ( int i = 0; i < 4; i++ ) {
                        buf[i] = buf[i + 4];
                    }
                    double d2 = PortUtil.make_double_le( buf );
                    ret.Frequency[index] = d1;
                    ret.Volume[index] = d2;
                    len = fs.read( buf, 0, 16 );
                    index++;
                }
            } catch ( Exception ex ) {
            } finally {
                if ( fs != null ) {
                    try {
                        fs.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
            return ret;
        }

        public void Write( FileOutputStream fs ) 
#if JAVA
            throws IOException
#endif
        {
            byte[] buf0 = new byte[8];
            char[] ch8 = Header.ToCharArray();
            for ( int i = 0; i < 8; i++ ) {
                if ( i >= ch8.Length ) {
                    buf0[i] = 0x0;
                } else {
                    buf0[i] = (byte)ch8[i];
                }
            }
            fs.write( buf0, 0, 8 );

            buf0 = PortUtil.getbytes_uint32_le( SampleInterval );
            fs.write( buf0, 0, 4 );

            buf0 = PortUtil.getbytes_double_le( AverageFrequency );
            fs.write( buf0, 0, 8 );

            for ( int i = 0; i < 4; i++ ) {
                buf0 = PortUtil.getbytes_int32_le( 0 );
                fs.write( buf0, 0, 4 );
            }
            buf0 = PortUtil.getbytes_int32_le( NumPoints );
            fs.write( buf0, 0, 4 );
            for ( int i = 0; i < NumPoints; i++ ) {
                buf0 = PortUtil.getbytes_double_le( Frequency[i] );
                fs.write( buf0, 0, 8 );
                buf0 = PortUtil.getbytes_double_le( Volume[i] );
                fs.write( buf0, 0, 8 );
            }
        }
    }

#if !JAVA
}
#endif
