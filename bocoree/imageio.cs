/*
 * imageio.cs
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
using bocoree.java.awt.image;
using bocoree.java.io;

namespace bocoree.javax.imageio {

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
                fs = new System.IO.FileStream( output.getPath(), System.IO.FileMode.Open, System.IO.FileAccess.Write );
                im.m_image.Save( fs, fmt );
                ret = true;
            } catch {
                ret = false;
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
