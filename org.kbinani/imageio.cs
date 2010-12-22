/*
 * imageio.cs
 * Copyright © 2009-2010 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if !JAVA
using org.kbinani.java.awt.image;
using org.kbinani.java.io;

namespace org.kbinani.javax.imageio {

    public class ImageIO {
        public static bool write( BufferedImage im, string formatName, File output ) {
            System.Drawing.Imaging.ImageFormat fmt = System.Drawing.Imaging.ImageFormat.Bmp;
            switch ( formatName ) {
                case "BMP":
                case "bmp":
                    fmt = System.Drawing.Imaging.ImageFormat.Bmp;
                    break;
                case "jpg":
                case "JPG":
                case "jpeg":
                case "JPEG":
                    fmt = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;
                case "png":
                case "PNG":
                    fmt = System.Drawing.Imaging.ImageFormat.Png;
                    break;
                case "GIF":
                case "gif":
                    fmt = System.Drawing.Imaging.ImageFormat.Gif;
                    break;
                default:
                    return false;
            }
            System.IO.FileStream fs = null;
            bool ret = false;
            try {
#if DEBUG
                PortUtil.println( "ImageIO#write; output.getPath()=" + output.getPath() );
#endif
                fs = new System.IO.FileStream( output.getPath(), System.IO.FileMode.Create, System.IO.FileAccess.Write );
                im.m_image.Save( fs, fmt );
                ret = true;
            } catch ( System.Exception ex ) {
                ret = false;
                Logger.write( typeof( ImageIO ) + ".write; ex=" + ex );
                PortUtil.println( typeof( ImageIO ) + "#write; ex=" + ex );
            } finally {
                if ( fs != null ) {
                    try {
                        fs.Close();
                    } catch { }
                }
            }
            return ret;
        }
    }

}
#endif
