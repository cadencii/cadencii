/*
 * UtauFreq.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;

namespace Boare.Cadencii {

    public class UtauFreq {
        public String Header;
        public int SampleInterval;
        public double AverageFrequency;
        public int NumPoints;
        public double[] Frequency;
        public double[] Volume;

        private UtauFreq() {
        }

        public static UtauFreq FromWav( String file ) {
            throw new NotImplementedException();
            return null;
        }

        /// <summary>
        /// *.frqファイルからのコンストラクタ
        /// </summary>
        /// <param name="file"></param>
        public static UtauFreq FromFrq( String file ){
            UtauFreq ret = new UtauFreq();
            using ( FileStream fs = new FileStream( file, FileMode.Open, FileAccess.Read ) ) {
                byte[] buf0 = new byte[8];
                fs.Read( buf0, 0, 8 );
                char[] ch8 = new char[8];
                for ( int i = 0; i < 8; i++ ) {
                    ch8[i] = (char)buf0[i];
                }
                ret.Header = new String( ch8 );

                fs.Read( buf0, 0, 4 );
                ret.SampleInterval = BitConverter.ToInt32( buf0, 0 );

                fs.Read( buf0, 0, 8 );
                ret.AverageFrequency = BitConverter.ToDouble( buf0, 0 );

                for ( int i = 0; i < 4; i++ ) {
                    int len2 = fs.Read( buf0, 0, 4 );
                    int i1 = BitConverter.ToInt32( buf0, 0 );
                }
                fs.Read( buf0, 0, 4 );
                ret.NumPoints = BitConverter.ToInt32( buf0, 0 );
                ret.Frequency = new double[ret.NumPoints];
                ret.Volume = new double[ret.NumPoints];
                byte[] buf = new byte[16];
                int len = fs.Read( buf, 0, 16 );
                int index = 0;
                while ( len > 0 ) {
                    double d1 = BitConverter.ToDouble( buf, 0 );
                    double d2 = BitConverter.ToDouble( buf, 8 );
                    ret.Frequency[index] = d1;
                    ret.Volume[index] = d2;
                    len = fs.Read( buf, 0, 16 );
                    index++;
                }
            }
            return ret;
        }

        public void Write( Stream fs ) {
            byte[] buf0 = new byte[8];
            char[] ch8 = Header.ToCharArray();
            for ( int i = 0; i < 8; i++ ) {
                if ( i >= ch8.Length ) {
                    buf0[i] = 0x0;
                } else {
                    buf0[i] = (byte)ch8[i];
                }
            }
            fs.Write( buf0, 0, 8 );

            buf0 = BitConverter.GetBytes( SampleInterval );
            fs.Write( buf0, 0, 4 );

            buf0 = BitConverter.GetBytes( AverageFrequency );
            fs.Write( buf0, 0, 8 );

            for ( int i = 0; i < 4; i++ ) {
                buf0 = BitConverter.GetBytes( (int)0 );
                fs.Write( buf0, 0, 4 );
            }
            buf0 = BitConverter.GetBytes( NumPoints );
            fs.Write( buf0, 0, 4 );
            for ( int i = 0; i < NumPoints; i++ ) {
                buf0 = BitConverter.GetBytes( Frequency[i] );
                fs.Write( buf0, 0, 8 );
                buf0 = BitConverter.GetBytes( Volume[i] );
                fs.Write( buf0, 0, 8 );
            }
        }
    }

}
