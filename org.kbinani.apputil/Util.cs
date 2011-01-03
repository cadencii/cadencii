/*
 * Misc.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of org.kbinani.apputil.
 *
 * org.kbinani.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.apputil;

import java.awt.*;
import javax.swing.*;
import java.awt.image.*;
import org.kbinani.*;
#else
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using org.kbinani;

namespace org.kbinani.apputil {
    using java = org.kbinani.java;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class Util{
#else
    public static partial class Util {
#endif
        public static readonly String PANGRAM = "cozy lummox gives smart squid who asks for job pen. 01234567890 THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS.";
        /// <summary>
        /// このクラスのメソッド'applyFontRecurse', 'applyToolStripFontRecurse', 'applyContextMenuFontRecurse'の呼び出しを有効とするかどうか。
        /// デフォルトではtrue
        /// </summary>
        public static boolean isApplyFontRecurseEnabled = true;

#if JAVA
        public static void applyContextMenuFontRecurse( MenuElement item, Font font ){
            if ( !isApplyFontRecurseEnabled ) {
                return;
            }
            applyToolStripFontRecurse( item, font );
        }
#else
        public static void applyContextMenuFontRecurse( ContextMenuStrip item, org.kbinani.java.awt.Font font ) {
            if ( !isApplyFontRecurseEnabled ) {
                return;
            }
            item.Font = font.font;
            foreach ( ToolStripItem tsi in item.Items ) {
                applyToolStripFontRecurse( tsi, font );
            }
        }
#endif

#if JAVA
        public static void applyToolStripFontRecurse( MenuElement item, Font font ){
            if ( !isApplyFontRecurseEnabled ) {
                return;
            }
            if( item instanceof Component ){
                ((Component)item).setFont( font );
            }
            for( MenuElement element : item.getSubElements() ){
                applyToolStripFontRecurse( element, font );
            }
        }
#else
        public static void applyToolStripFontRecurse( ToolStripItem item, org.kbinani.java.awt.Font font ) {
            if ( !isApplyFontRecurseEnabled ) {
                return;
            }
            item.Font = font.font;
            if ( item is ToolStripMenuItem ) {
                ToolStripMenuItem tsmi = (ToolStripMenuItem)item;
                foreach ( ToolStripItem tsi in tsmi.DropDownItems ) {
                    applyToolStripFontRecurse( tsi, font );
                }
            } else if ( item is ToolStripDropDownItem ) {
                ToolStripDropDownItem tsdd = (ToolStripDropDownItem)item;
                foreach ( ToolStripItem tsi in tsdd.DropDownItems ) {
                    applyToolStripFontRecurse( tsi, font );
                }
            }
        }
#endif

        /// <summary>
        /// 指定したフォントを描画するとき、描画指定したy座標と、描かれる文字の中心線のズレを調べます
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public static int getStringDrawOffset( java.awt.Font font ) {
            int ret = 0;
            java.awt.Dimension size = measureString( PANGRAM, font );
            if ( size.height <= 0 ) {
                return 0;
            }
            java.awt.image.BufferedImage b = null;
            java.awt.Graphics2D g = null;
#if JAVA
            java.awt.image.BufferedImage b2 = null;
#else
            BitmapEx b2 = null;
#endif
            try {
                int string_desty = size.height * 2; // 文字列が書き込まれるy座標
                int w = size.width * 4;
                int h = size.height * 4;
                b = new java.awt.image.BufferedImage( w, h, java.awt.image.BufferedImage.TYPE_INT_BGR );
                g = b.createGraphics();
                g.setColor( java.awt.Color.white );
                g.fillRect( 0, 0, w, h );
                g.setFont( font );
                g.setColor( java.awt.Color.black );
                g.drawString( PANGRAM, size.width, string_desty );

#if JAVA
                b2 = b;
#else
                b2 = new BitmapEx( b.m_image );
#endif
                // 上端に最初に現れる色つきピクセルを探す
                int firsty = 0;
                boolean found = false;
                for ( int y = 0; y < h; y++ ) {
                    for ( int x = 0; x < w; x++ ) {
#if JAVA
                        int ic = b2.getRGB( x, y );
                        Color c = new Color( ic );
#else
                        java.awt.Color c = new org.kbinani.java.awt.Color( b2.GetPixel( x, y ) );
#endif
                        if ( c.getRed() != 255 || c.getGreen() != 255 || c.getBlue() != 255 ) {
                            found = true;
                            firsty = y;
                            break;
                        }
                    }
                    if ( found ) {
                        break;
                    }
                }

                // 下端
                int endy = h - 1;
                found = false;
                for ( int y = h - 1; y >= 0; y-- ) {
                    for ( int x = 0; x < w; x++ ) {
#if JAVA
                        int ic = b2.getRGB( x, y );
                        Color c = new Color( ic );
#else
                        java.awt.Color c = new org.kbinani.java.awt.Color( b2.GetPixel( x, y ) );
#endif
                        if ( c.getRed() != 255 || c.getGreen() != 255 || c.getBlue() != 255 ) {
                            found = true;
                            endy = y;
                            break;
                        }
                    }
                    if ( found ) {
                        break;
                    }
                }

                int center = (firsty + endy) / 2;
                ret = center - string_desty;
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "Util#getStringDrawOffset; ex=" + ex );
            } finally {
#if JAVA
#else
                if ( b != null && b.m_image != null ) {
                    b.m_image.Dispose();
                }
                if ( g != null ) {
                    g.nativeGraphics.Dispose();
                }
                if ( b2 != null && b2 != null ) {
                    b2.Dispose();
                }
#endif
            }
            return ret;
        }

        /// <summary>
        /// 指定した言語コードの表す言語が、右から左へ記述する言語かどうかを調べます
        /// </summary>
        /// <param name="language_code"></param>
        /// <returns></returns>
        public static boolean isRightToLeftLanguage( String language_code ) {
            language_code = language_code.ToLower();
            if ( language_code.Equals( "ar" ) ||
                 language_code.Equals( "he" ) ||
                 language_code.Equals( "iw" ) ||
                 language_code.Equals( "fa" ) ||
                 language_code.Equals( "ur" ) ) {
                return true;
            } else {
                return false;
            }
        }

#if !JAVA
        /// <summary>
        /// 指定したディレクトリに作成可能な、一時ファイル名を取得します
        /// </summary>
        /// <param name="directory">ディレクトリ</param>
        /// <returns></returns>
        public static string GetTempFileNameIn( string directory ) {
            for ( uint i = uint.MinValue; i <= uint.MaxValue; i++ ) {
                string file = Path.Combine( directory, "temp" + i );
                if ( !File.Exists( file ) ) {
                    return file;
                }
            }
            return "";
        }
#endif

#if !JAVA
        /// <summary>
        /// 指定したディレクトリに作成可能な、一時ファイル名を取得します
        /// </summary>
        /// <param name="directory">ディレクトリ</param>
        /// <param name="extention">拡張子（ex. ".txt"）</param>
        /// <returns></returns>
        public static string GetTempFileNameIn( string directory, string extention ){
            for ( uint i = uint.MinValue; i <= uint.MaxValue; i++ ) {
                string file = Path.Combine( directory, "temp" + i + extention );
                if ( !File.Exists( file ) ) {
                    return file;
                }
            }
            return "";
        }
#endif

#if !JAVA
        /// <summary>
        /// 指定した画像ファイルから新しいBitmapオブジェクトを作成します
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Bitmap BitmapFromStream( string file ) {
            if ( !File.Exists( file ) ) {
                return null;
            }
            FileStream fs = new FileStream( file, FileMode.Open );
            Bitmap ret = new Bitmap( fs );
            fs.Close();
            return ret;
        }
#endif

#if !JAVA
        /// <summary>
        /// 指定した画像ファイルから新しいImageオブジェクトを作成します
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Image ImageFromStream( string file ) {
            if ( !File.Exists( file ) ) {
                return null;
            }
            FileStream fs = new FileStream( file, FileMode.Open );
            Image ret = Image.FromStream( fs );
            fs.Close();
            return ret;
        }
#endif

#if !JAVA
        /// <summary>
        /// ImageFormatから，デフォルトの拡張子を取得します
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetExtensionFromImageFormat( ImageFormat format ) {
            switch ( format.ToString().ToLower() ) {
                case "bmp":
                    return ".bmp";
                case "emf":
                    return ".emf";
                case "gif":
                    return ".gif";
                case "jpeg":
                    return ".jpg";
                case "png":
                    return ".png";
                case "tiff":
                    return ".tiff";
                case "wmf":
                    return ".wmf";
                default:
                    return "";

            }
        }
#endif

#if !JAVA
        /// <summary>
        /// System.Drawimg.Imaging.ImageFormatで使用可能なフォーマットの一覧を取得します
        /// </summary>
        /// <returns></returns>
        public static ImageFormat[] GetImageFormats() {
#if DEBUG
            Console.WriteLine( "GetImageFormats()" );
#endif
            PropertyInfo[] properties = typeof( System.Drawing.Imaging.ImageFormat ).GetProperties();
            List<ImageFormat> ret = new List<ImageFormat>();
            foreach ( PropertyInfo pi in properties ) {
                if ( pi.PropertyType.Equals( typeof( System.Drawing.Imaging.ImageFormat ) ) ) {
                    ImageFormat ifmt = (System.Drawing.Imaging.ImageFormat)pi.GetValue( null, null );
#if DEBUG
                    Console.WriteLine( ifmt.ToString() );
#endif
                    ret.Add( ifmt );
                }
            }
            return ret.ToArray();
        }
#endif

#if !JAVA
        public static void RgbToHsv( int r, int g, int b, out double h, out double s, out double v ) {
            RgbToHsv( r / 255.0, g / 255.0, b / 255.0, out h, out s, out v );
        }

        public static void RgbToHsv( double r, double g, double b, out double h, out double s, out double v ) {
            double tmph, imax, imin;
            const double sqrt3 = 1.7320508075688772935274463415059;
            imax = Math.Max( r, Math.Max( g, b ) );
            imin = Math.Min( r, Math.Min( g, b ) );
            if ( imax == 0.0 ) {
                h = 0;
                s = 0;
                v = 0;
                return;
            } else if ( imax == imin ) {
                tmph = 0;
            } else {
                if ( r == imax ) {
                    tmph = 60.0 * (g - b) / (imax - imin);
                } else if ( g == imax ) {
                    tmph = 60.0 * (b - r) / (imax - imin) + 120.0;
                } else {
                    tmph = 60.0 * (r - g) / (imax - imin) + 240.0;
                }
            }
            while ( tmph < 0.0 ) {
                tmph = tmph + 360.0;
            }
            while ( tmph >= 360.0 ) {
                tmph = tmph - 360.0;
            }
            h = tmph / 360.0;
            s = (imax - imin) / imax;
            v = imax;
        }

        public static Color HsvToColor( double h, double s, double v ) {
            double dr, dg, db;
            HsvToRgb( h, s, v, out dr, out dg, out db );
            return Color.FromArgb( (int)(dr * 255), (int)(dg * 255), (int)(db * 255) );
        }

        public static void HsvToRgb( double h, double s, double v, out byte r, out byte g, out byte b ) {
            double dr, dg, db;
            HsvToRgb( h, s, v, out dr, out dg, out db );
            r = (byte)(dr * 255);
            g = (byte)(dg * 255);
            b = (byte)(db * 255);
        }

        public static void HsvToRgb( double h, double s, double v, out double r, out double g, out double b ) {
            double f, p, q, t, hh;
            int hi;
            r = g = b = 0.0;
            if ( s == 0 ) {
                r = v;
                g = v;
                b = v;
            } else {
                hh = h * 360.0;
                hi = (int)(hh / 60.0) % 6;
                f = hh / 60.0 - (double)(hi);
                p = v * (1.0 - s);
                q = v * (1.0 - f * s);
                t = v * (1.0 - (1.0 - f) * s);
                switch ( hi ) {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;
                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;
                    case 2:
                        r = p;
                        g = v;
                        b = t;
                        break;
                    case 3:
                        r = p;
                        g = q;
                        b = v;
                        break;
                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = p;
                        b = q;
                        break;
                }
            }
        }
#endif

        /// <summary>
        /// 指定された文字列を指定されたフォントで描画したときのサイズを計測します。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
#if JAVA
        public static Dimension measureString( String text, Font font ){
            BufferedImage dumy = new BufferedImage( 1, 1, BufferedImage.TYPE_INT_BGR );
            Graphics2D g = dumy.createGraphics();
            g.setFont( font );
            FontMetrics fm = g.getFontMetrics();
            Dimension ret = new Dimension( fm.stringWidth( text ), fm.getHeight() );
            g = null;
            dumy = null;
            return ret;
        }
#else
        public static java.awt.Dimension measureString( string text, java.awt.Font font ) {
            using ( Bitmap dumy = new Bitmap( 1, 1 ) )
            using ( Graphics g = Graphics.FromImage( dumy ) ) {
                SizeF tmp = g.MeasureString( text, font.font );
                return new java.awt.Dimension( (int)tmp.Width, (int)tmp.Height );
            }
        }
#endif

        /// <summary>
        /// 指定したコントロールと、その子コントロールのフォントを再帰的に変更します
        /// </summary>
        /// <param name="c"></param>
        /// <param name="font"></param>
#if JAVA
        public static void applyFontRecurse( Component c, Font font ){
#else
        public static void applyFontRecurse( Control c, java.awt.Font font ) {
#endif
            if ( !isApplyFontRecurseEnabled ) {
                return;
            }
#if JAVA
            c.setFont( font );
            if( c instanceof Container ){
                Container container = (Container)c;
                int count = container.getComponentCount();
                for( int i = 0; i < count; i++ ){
                    Component component = container.getComponent( i );
                    applyFontRecurse( component, font );
                }
            }
#else
            c.Font = font.font;
            for ( int i = 0; i < c.Controls.Count; i++ ) {
                applyFontRecurse( c.Controls[i], font );
            }
#endif
        }

#if !JAVA
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start1"></param>
        /// <param name="end1"></param>
        /// <param name="start2"></param>
        /// <param name="end2"></param>
        /// <returns></returns>
        public static bool IsOverwrapped( double start1, double end1, double start2, double end2 ) {
            if ( start2 <= start1 && start1 < end2 ) {
                return true;
            } else if ( start2 < end1 && end1 < end2 ) {
                return true;
            } else {
                if ( start1 <= start2 && start2 < end1 ) {
                    return true;
                } else if ( start1 < end2 && end2 < end1 ) {
                    return true;
                } else {
                    return false;
                }
            }
        }
#endif
    }

#if !JAVA
}
#endif
