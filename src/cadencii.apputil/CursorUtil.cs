/*
 * CursorUtil.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#define RGB24
using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;


using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using cadencii;

namespace cadencii.apputil
{

    public static class CursorUtil
    {
        public static void SaveAsIcon(Bitmap item, Stream stream, Color transp)
        {
            SaveCor(item, new Point(0, 0), stream, 1, transp);
        }

        public static void SaveAsCursor(Bitmap item, Point hotspot, Stream stream, Color transp)
        {
            SaveCor(item, hotspot, stream, 2, transp);
        }

        private static void SaveCor(Bitmap item, Point hotspot, Stream stream, ushort type, Color transp)
        {
            IconFileHeader ifh = new IconFileHeader();
            ifh.icoReserved = 0x0;
            ifh.icoResourceCount = 1;
            ifh.icoResourceType = type;
            ifh.Write(stream);
            IconInfoHeader iif = new IconInfoHeader();
            BITMAPINFOHEADER bih = new BITMAPINFOHEADER();
            iif.Width = (byte)item.Width;
            iif.Height = (byte)item.Height;
            iif.ColorCount = 0;
            iif.Reserved1 = 0;
            iif.Reserved2 = (ushort)hotspot.X;
            iif.Reserved3 = (ushort)hotspot.Y;
#if RGB24
            int linesize = ((item.Width * 24 + 31) / 32) * 4;
#else
            int linesize = ((item.Width * 32 + 31) / 32) * 4;
#endif
            int linesize_mask = ((item.Width * 1 + 31) / 32) * 4;
            int size = linesize * item.Height + linesize_mask * item.Height + 40;
#if DEBUG
            Console.WriteLine("linesize=" + linesize);
#endif
            iif.icoDIBSize = (uint)size;
            iif.icoDIBOffset = 0x16;
            iif.Write(stream);
            bih.biSize = 40;
            bih.biWidth = item.Width;
#if RGB24
            bih.biHeight = item.Height * 2;
#else
            bih.biHeight = item.Height * 2;
#endif
            bih.biPlanes = 1;
#if RGB24
            bih.biBitCount = 24;
#else
            bih.biBitCount = 32;
#endif
            bih.biCompression = 0;
            bih.biSizeImage = (uint)(linesize * item.Height);
            bih.biXPelsPerMeter = 0;//            (int)(item.HorizontalResolution / 2.54e-2);
            bih.biYPelsPerMeter = 0;//            (int)(item.VerticalResolution / 2.54e-2);
            bih.biClrUsed = 0;
            bih.biClrImportant = 0;
            bih.Write(stream);
            for (int y = item.Height - 1; y >= 0; y--) {
                int count = 0;
                for (int x = 0; x < item.Width; x++) {
                    Color c = item.GetPixel(x, y);
                    stream.WriteByte((byte)c.B);
                    stream.WriteByte((byte)c.G);
                    stream.WriteByte((byte)c.R);
#if DEBUG
                    if (c.R != transp.R || c.G != transp.G || c.B != transp.B) {
                        Console.WriteLine("color=" + c);
                    }
#endif
#if RGB24
                    count += 3;
#else
                    stream.WriteByte( (byte)c.A );
                    count += 4;
#endif
                }
                for (int i = count; i < linesize; i++) {
                    stream.WriteByte(0x0);
                }
            }

            for (int y = item.Height - 1; y >= 0; y--) {
                int count = 0;
                byte v = 0x0;
                int tcount = 0;
                for (int x = 0; x < item.Width; x++) {
                    Color c = item.GetPixel(x, y);
                    byte tr = 0x0;
                    if (c.R == transp.R && c.G == transp.G && c.B == transp.B) {
                        tr = 0x1;
                    }
                    v = (byte)((byte)(v << 1) | (byte)(tr & 0x1));
                    tcount++;
                    if (tcount == 8) {
                        stream.WriteByte(v);
                        count++;
                        tcount = 0;
                        v = 0x0;
                    }
                }
                if (0 < tcount) {
                    v = (byte)(v << (9 - tcount));
                    stream.WriteByte(v);
                    count++;
                }
                for (int i = count; i < linesize_mask; i++) {
                    stream.WriteByte(0x0);
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IconFileHeader
    {
        public ushort icoReserved;
        public ushort icoResourceType;
        public ushort icoResourceCount;

        public void Write(Stream stream)
        {
            byte[] buf;
            buf = BitConverter.GetBytes(icoReserved);
            stream.Write(buf, 0, 2);
            buf = BitConverter.GetBytes(icoResourceType);
            stream.Write(buf, 0, 2);
            buf = BitConverter.GetBytes(icoResourceCount);
            stream.Write(buf, 0, 2);
        }

        public static IconFileHeader Read(Stream fs)
        {
            IconFileHeader ifh = new IconFileHeader();
            byte[] buf = new byte[2];
            fs.Read(buf, 0, 2);
            ifh.icoReserved = BitConverter.ToUInt16(buf, 0);
            fs.Read(buf, 0, 2);
            ifh.icoResourceType = BitConverter.ToUInt16(buf, 0);
            fs.Read(buf, 0, 2);
            ifh.icoResourceCount = BitConverter.ToUInt16(buf, 0);
            return ifh;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IconInfoHeader
    {
        public byte Width;
        public byte Height;
        public byte ColorCount;
        public byte Reserved1;
        public ushort Reserved2;
        public ushort Reserved3;
        public uint icoDIBSize;
        public uint icoDIBOffset;

        public override string ToString()
        {
            return "{Width=" + Width + ", Height=" + Height + ", ColorCount=" + ColorCount + ", Reserved1=" + Reserved1 + ", Reserved2=" + Reserved2 + ", Reserved3=" + Reserved3 + ", icoDIBSize=" + icoDIBSize + ", icoDIBOffset=" + icoDIBOffset + "}";
        }

        public void Write(Stream stream)
        {
            byte[] buf;
            stream.WriteByte(Width);
            stream.WriteByte(Height);
            stream.WriteByte(ColorCount);
            stream.WriteByte(Reserved1);
            buf = BitConverter.GetBytes(Reserved2);
            stream.Write(buf, 0, 2);
            buf = BitConverter.GetBytes(Reserved3);
            stream.Write(buf, 0, 2);
            buf = BitConverter.GetBytes(icoDIBSize);
            stream.Write(buf, 0, 4);
            buf = BitConverter.GetBytes(icoDIBOffset);
            stream.Write(buf, 0, 4);
        }

        public static IconInfoHeader Read(Stream stream)
        {
            IconInfoHeader iih = new IconInfoHeader();
            iih.Width = (byte)stream.ReadByte();
            iih.Height = (byte)stream.ReadByte();
            iih.ColorCount = (byte)stream.ReadByte();
            iih.Reserved1 = (byte)stream.ReadByte();
            byte[] buf = new byte[4];
            stream.Read(buf, 0, 4);
            iih.Reserved2 = BitConverter.ToUInt16(buf, 0);
            iih.Reserved3 = BitConverter.ToUInt16(buf, 2);
            stream.Read(buf, 0, 4);
            iih.icoDIBSize = BitConverter.ToUInt32(buf, 0);
            stream.Read(buf, 0, 4);
            iih.icoDIBOffset = BitConverter.ToUInt32(buf, 0);
            return iih;
        }
    }

}
