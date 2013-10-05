/*
 * wingdi.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Runtime.InteropServices;
using System.IO;

namespace cadencii
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPINFOHEADER
    {
        public UInt32 biSize;
        public Int32 biWidth;
        public Int32 biHeight;
        public Int16 biPlanes;
        public Int16 biBitCount;
        public UInt32 biCompression;
        public UInt32 biSizeImage;
        public Int32 biXPelsPerMeter;
        public Int32 biYPelsPerMeter;
        public UInt32 biClrUsed;
        public UInt32 biClrImportant;

        public static BITMAPINFOHEADER Read(Stream stream)
        {
            BITMAPINFOHEADER bifh = new BITMAPINFOHEADER();
            byte[] buf = new byte[4];
            bifh.biSize = readUInt32(stream);
            bifh.biWidth = readInt32(stream);
            bifh.biHeight = readInt32(stream);
            bifh.biPlanes = readInt16(stream);
            bifh.biBitCount = readInt16(stream);
            bifh.biCompression = readUInt32(stream);
            bifh.biSizeImage = readUInt32(stream);
            bifh.biXPelsPerMeter = readInt32(stream);
            bifh.biYPelsPerMeter = readInt32(stream);
            bifh.biClrUsed = readUInt32(stream);
            bifh.biClrImportant = readUInt32(stream);
            return bifh;
        }

        private static uint readUInt32(Stream fs)
        {
            byte[] buf = new byte[4];
            fs.Read(buf, 0, 4);
            return BitConverter.ToUInt32(buf, 0);
        }

        private static int readInt32(Stream fs)
        {
            byte[] buf = new byte[4];
            fs.Read(buf, 0, 4);
            return BitConverter.ToInt32(buf, 0);
        }

        private static ushort readUInt16(Stream fs)
        {
            byte[] buf = new byte[2];
            fs.Read(buf, 0, 2);
            return BitConverter.ToUInt16(buf, 0);
        }

        private static short readInt16(Stream fs)
        {
            byte[] buf = new byte[2];
            fs.Read(buf, 0, 2);
            return BitConverter.ToInt16(buf, 0);
        }

        public override string ToString()
        {
            return "{biSize=" + biSize + ", biWidth=" + biWidth + ", biHeight=" + biHeight + ", biPlanes=" + biPlanes + ", biBitCount=" + biBitCount +
                   ", biCompression=" + biCompression + ", biSizeImage=" + biSizeImage + ", biXPelsPerMeter=" + biXPelsPerMeter + ", biYPelsPerMeter=" + biYPelsPerMeter +
                   ", biClrUsed=" + biClrUsed + ", biClrImportant=" + biClrImportant + "}";
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(biSize);
            bw.Write((uint)biWidth);
            bw.Write((uint)biHeight);
            bw.Write((ushort)biPlanes);
            bw.Write((ushort)biBitCount);
            bw.Write((uint)biCompression);
            bw.Write((uint)biSizeImage);
            bw.Write((uint)biXPelsPerMeter);
            bw.Write((uint)biYPelsPerMeter);
            bw.Write((uint)biClrUsed);
            bw.Write((uint)biClrImportant);
        }
        public void Write(Stream s)
        {
            byte[] b;
            bool bigendian = !BitConverter.IsLittleEndian;

            b = BitConverter.GetBytes(biSize);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(biWidth);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(biHeight);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(biPlanes);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 2);

            b = BitConverter.GetBytes(biBitCount);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 2);

            b = BitConverter.GetBytes(biCompression);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(biSizeImage);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(biXPelsPerMeter);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(biYPelsPerMeter);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(biClrUsed);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);

            b = BitConverter.GetBytes(biClrImportant);
            if (bigendian) {
                Array.Reverse(b);
            }
            s.Write(b, 0, 4);
        }
    }

}
