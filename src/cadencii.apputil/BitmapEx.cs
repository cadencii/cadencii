/*
 * BitmapEx.cs
 * Copyright © 2008-2011 kbinani
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
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace cadencii.apputil
{

    public unsafe class BitmapEx : IDisposable
    {
        private Bitmap m_base;
        private bool m_locked = false;
        private BitmapData m_bd;
        private int m_stride;
        private int m_byte_per_pixel;

        public int Width
        {
            get
            {
                return m_base.Width;
            }
        }

        public int Height
        {
            get
            {
                return m_base.Height;
            }
        }

        public void Dispose()
        {
            EndLock();
            m_base.Dispose();
        }

        public void EndLock()
        {
            if (m_locked) {
                m_base.UnlockBits(m_bd);
                m_locked = false;
            }
        }

        public Bitmap GetBitmap()
        {
            return (Bitmap)m_base.Clone();
        }

        public Color GetPixel(int x, int y)
        {
            if (!m_locked) {
                BeginLock();
            }
            int location = y * m_stride + m_byte_per_pixel * x;
            byte* dat = (byte*)m_bd.Scan0;
            byte b = dat[location];
            byte g = dat[location + 1];
            byte r = dat[location + 2];
            byte a = 255;
            if (m_base.PixelFormat == PixelFormat.Format32bppArgb) {
                a = dat[location + 3];
            }
            return Color.FromArgb(a, r, g, b);
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (!m_locked) {
                BeginLock();
            }
            int location = y * m_stride + m_byte_per_pixel * x;
            byte* dat = (byte*)m_bd.Scan0;
            dat[location] = (byte)color.B;
            dat[location + 1] = (byte)color.G;
            dat[location + 2] = (byte)color.R;
            if (m_base.PixelFormat == PixelFormat.Format32bppArgb) {
                dat[location + 3] = (byte)color.A;
            }
        }

        public void BeginLock()
        {
            if (!m_locked) {
                m_bd = m_base.LockBits(new Rectangle(0, 0, m_base.Width, m_base.Height),
                                        ImageLockMode.ReadWrite,
                                        m_base.PixelFormat);
                m_stride = m_bd.Stride;
                switch (m_base.PixelFormat) {
                    case PixelFormat.Format24bppRgb:
                    m_byte_per_pixel = 3;
                    break;
                    case PixelFormat.Format32bppArgb:
                    m_byte_per_pixel = 4;
                    break;
                    default:
                    throw new Exception("unsuported pixel format");
                }
                m_locked = true;
            }
        }

        ~BitmapEx()
        {
            m_base.Dispose();
        }

        public BitmapEx(Image original)
        {
            m_base = new Bitmap(original);
        }

        public BitmapEx(string filename)
        {
            m_base = new Bitmap(filename);
        }

        public BitmapEx(Stream stream)
        {
            m_base = new Bitmap(stream);
        }

        public BitmapEx(Image original, Size newSize)
        {
            m_base = new Bitmap(original, newSize);
        }

        public BitmapEx(int width, int height)
        {
            m_base = new Bitmap(width, height);
        }

        public BitmapEx(Stream stream, bool useIcm)
        {
            m_base = new Bitmap(stream, useIcm);
        }

        public BitmapEx(string filename, bool useIcm)
        {
            m_base = new Bitmap(filename, useIcm);
        }

        public BitmapEx(Type type, string resource)
        {
            m_base = new Bitmap(type, resource);
        }

        public BitmapEx(Image original, int width, int height)
        {
            m_base = new Bitmap(original, width, height);
        }

        public BitmapEx(int width, int height, Graphics g)
        {
            m_base = new Bitmap(width, height, g);
        }

        public BitmapEx(int width, int height, PixelFormat format)
        {
            m_base = new Bitmap(width, height, format);
        }

        public BitmapEx(int width, int height, int stride, PixelFormat format, IntPtr scan0)
        {
            m_base = new Bitmap(width, height, stride, format, scan0);
        }
    }

}
