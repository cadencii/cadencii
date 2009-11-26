/*
 * awt.image.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of bocoree.
 *
 * bocoree is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * bocoree is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if !JAVA
namespace bocoree.java.awt.image {

    public class BufferedImage/* : bocoree.awt.Image*/{
        public static int TYPE_INT_BGR = 0;
        public static int TYPE_INT_RGB = 1;
        public static int TYPE_INT_ARGB = 2;

        public System.Drawing.Bitmap m_image;

        public BufferedImage( int width, int height, int imageType ) {
            System.Drawing.Imaging.PixelFormat format = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
            if ( imageType == TYPE_INT_ARGB ) {
                format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            }
            m_image = new System.Drawing.Bitmap( width, height, format );
        }

        public Graphics2D createGraphics() {
            return new Graphics2D( System.Drawing.Graphics.FromImage( m_image ) );
        }

        public int getHeight( object observer ) {
            return m_image.Height;
        }

        public int getWidth( object observer ) {
            return m_image.Width;
        }

        public int getWidth() {
            return m_image.Width;
        }

        public int getHeight() {
            return m_image.Height;
        }
    }

}
#endif
