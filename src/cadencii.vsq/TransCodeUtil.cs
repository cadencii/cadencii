/*
 * TransCodeUtil.cs
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
namespace cadencii.vsq
{

    public class TransCodeUtil
    {
        private static readonly byte[] MAP_L = new byte[] { 0xA, 0xB, 0x8, 0x9, 0xE, 0xF, 0xC, 0xD, 0x2, 0x3, 0x0, 0x1, 0x6, 0x7, 0x4, 0x5 };
        private static readonly byte[] MAP_R = new byte[] { 0x1, 0x0, 0x3, 0x2, 0x5, 0x4, 0x7, 0x6, 0x9, 0x8, 0xB, 0xA, 0xD, 0xC, 0xF, 0xE };

        public static void decodeBytes(byte[] dat)
        {
            for (int i = 0; i < dat.Length; i++) {
                byte M = (byte)(dat[i] >> 4);
                byte L = (byte)(dat[i] - (M << 4));
                byte newM = endecode_vvd_m(M);
                byte newL = endecode_vvd_l(L);
                dat[i] = (byte)((newM << 4) | newL);
            }
            for (int i = 0; i < dat.Length - 1; i++) {
                if (dat[i] == 0x17 && dat[i + 1] == 0x10) {
                    dat[i] = 0x0d;
                    dat[i + 1] = 0x0a;
                }
            }
        }

        static byte endecode_vvd_l(byte value)
        {
            return MAP_L[value];
        }

        static byte endecode_vvd_m(byte value)
        {
            return MAP_R[value];
        }
    }

}
