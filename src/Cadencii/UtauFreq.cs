/*
 * UtauFreq.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using cadencii;
using cadencii.java.io;



namespace cadencii
{

    public class UtauFreq
    {
        public string Header;
        public int SampleInterval;
        public double AverageFrequency;
        public int NumPoints;
        public double[] Frequency;
        public double[] Volume;

        private UtauFreq()
        {
        }

        /*public static UtauFreq FromWav( String file ) {
            throw new NotImplementedException();
            return null;
        }*/

        /// <summary>
        /// *.frqファイルからのコンストラクタ
        /// </summary>
        /// <param name="file"></param>
        public static UtauFreq FromFrq(string file)
        {
            UtauFreq ret = new UtauFreq();
            FileStream fs = null;
            try {
                fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] buf0 = new byte[8];
                fs.Read(buf0, 0, 8);
                char[] ch8 = new char[8];
                for (int i = 0; i < 8; i++) {
                    ch8[i] = (char)buf0[i];
                }
                ret.Header = new string(ch8);

                fs.Read(buf0, 0, 4);
                ret.SampleInterval = PortUtil.make_int32_le(buf0);

                fs.Read(buf0, 0, 8);
                ret.AverageFrequency = PortUtil.make_double_le(buf0);

                for (int i = 0; i < 4; i++) {
                    int len2 = fs.Read(buf0, 0, 4);
                    int i1 = PortUtil.make_int32_le(buf0);
                }
                fs.Read(buf0, 0, 4);
                ret.NumPoints = PortUtil.make_int32_le(buf0);
                ret.Frequency = new double[ret.NumPoints];
                ret.Volume = new double[ret.NumPoints];
                byte[] buf = new byte[16];
                int len = fs.Read(buf, 0, 16);
                int index = 0;
                while (len > 0) {
                    double d1 = PortUtil.make_double_le(buf);
                    for (int i = 0; i < 4; i++) {
                        buf[i] = buf[i + 4];
                    }
                    double d2 = PortUtil.make_double_le(buf);
                    ret.Frequency[index] = d1;
                    ret.Volume[index] = d2;
                    len = fs.Read(buf, 0, 16);
                    index++;
                }
            } catch (Exception ex) {
            } finally {
                if (fs != null) {
                    try {
                        fs.Close();
                    } catch (Exception ex2) {
                    }
                }
            }
            return ret;
        }

        public void Write(FileStream fs)
        {
            byte[] buf0 = new byte[8];
            char[] ch8 = Header.ToCharArray();
            for (int i = 0; i < 8; i++) {
                if (i >= ch8.Length) {
                    buf0[i] = 0x0;
                } else {
                    buf0[i] = (byte)ch8[i];
                }
            }
            fs.Write(buf0, 0, 8);

            buf0 = PortUtil.getbytes_uint32_le(SampleInterval);
            fs.Write(buf0, 0, 4);

            buf0 = PortUtil.getbytes_double_le(AverageFrequency);
            fs.Write(buf0, 0, 8);

            for (int i = 0; i < 4; i++) {
                buf0 = PortUtil.getbytes_int32_le(0);
                fs.Write(buf0, 0, 4);
            }
            buf0 = PortUtil.getbytes_int32_le(NumPoints);
            fs.Write(buf0, 0, 4);
            for (int i = 0; i < NumPoints; i++) {
                buf0 = PortUtil.getbytes_double_le(Frequency[i]);
                fs.Write(buf0, 0, 8);
                buf0 = PortUtil.getbytes_double_le(Volume[i]);
                fs.Write(buf0, 0, 8);
            }
        }
    }

}
