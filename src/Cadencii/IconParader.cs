/*
 * IconParader.cs
 * Copyright © 2010-2011 kbinani
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
#if JAVA
package cadencii;

import java.awt.*;
import java.awt.image.*;
import java.io.*;
import javax.imageio.*;
import cadencii.*;
import cadencii.windows.forms.*;
#else
using System;
using cadencii.java.awt;
using cadencii.java.awt.image;
using cadencii.javax.imageio;
using cadencii.windows.forms;

namespace cadencii
{
#endif

    /// <summary>
    /// 起動時のスプラッシュウィンドウに表示されるアイコンパレードの、1個のアイコンを表現します
    /// </summary>
#if JAVA
    public class IconParader extends BPictureBox
#else
    public class IconParader : System.Windows.Forms.PictureBox
#endif
    {
        const int RADIUS = 6; // 角の丸み
        const int DIAMETER = 2 * RADIUS;
        public const int ICON_WIDTH = 48;
        public const int ICON_HEIGHT = 48;

#if !JAVA
        private System.Drawing.Drawing2D.GraphicsPath graphicsPath = null;
        private System.Drawing.Region region = null;
        private System.Drawing.Region invRegion = null;
        private System.Drawing.SolidBrush brush = null;
#endif

        public IconParader()
        {
            var d = new System.Drawing.Size( ICON_WIDTH, ICON_HEIGHT );
            this.Size = d;
            this.MaximumSize = d;
            this.MinimumSize = d;
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        }

        public static Image createIconImage( String path_image, String singer_name )
        {
#if DEBUG
#if !JAVA
            sout.println( "IconParader#createIconImage; path_image=" + path_image );
#endif
#endif
            Image ret = null;
            if (System.IO.File.Exists(path_image)) {
#if JAVA
                try{
                    ret = ImageIO.read( new File( path_image ) );
                }catch( Exception ex ){
                    ret = null;
                    System.out.println( "IconParader#createIconImage; ex=" + ex );
                }
#else
                System.IO.FileStream fs = null;
                try {
                    fs = new System.IO.FileStream( path_image, System.IO.FileMode.Open, System.IO.FileAccess.Read );
                    System.Drawing.Image img = System.Drawing.Image.FromStream( fs );
                    ret = new Image();
                    ret.image = img;
                } catch ( Exception ex ) {
                    serr.println( "IconParader#createIconImage; ex=" + ex );
                } finally {
                    if ( fs != null ) {
                        try {
                            fs.Close();
                        } catch ( Exception ex2 ) {
                            serr.println( "IconParader#createIconImage; ex2=" + ex2 );
                        }
                    }
                }
#endif
            }

            if ( ret == null ) {
                // 画像ファイルが無かったか，読み込みに失敗した場合

                // 歌手名が描かれた画像をセットする
                BufferedImage bmp = new BufferedImage( ICON_WIDTH, ICON_HEIGHT, BufferedImage.TYPE_INT_RGB );
                Graphics2D g = bmp.createGraphics();
                g.clearRect( 0, 0, ICON_WIDTH, ICON_HEIGHT );
#if JAVA
                Font font = new Font( "Arial", 0, 10 );
#else
                Font font = new Font( System.Windows.Forms.SystemInformation.MenuFont );
#endif
                PortUtil.drawStringEx(
                    (Graphics)g, singer_name, font, new Rectangle( 1, 1, ICON_WIDTH - 2, ICON_HEIGHT - 2 ),
                    PortUtil.STRING_ALIGN_NEAR, PortUtil.STRING_ALIGN_NEAR );
                ret = bmp;
            }

            return ret;
        }

        public void setImage( Image img )
        {
            BufferedImage bmp = new BufferedImage( ICON_WIDTH, ICON_HEIGHT, BufferedImage.TYPE_INT_RGB );
            Graphics g = null;
            try {
                g = bmp.createGraphics();
#if !JAVA
                g.nativeGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
#endif
                if ( img != null ) {
                    int img_width = img.getWidth( null );
                    int img_height = img.getHeight( null );
                    double a = img_height / (double)img_width;
                    double aspecto = ICON_HEIGHT / (double)ICON_WIDTH;

                    int x = 0;
                    int y = 0;
                    int w = ICON_WIDTH;
                    int h = ICON_HEIGHT;
                    if ( a >= aspecto ) {
                        // アイコンより縦長
                        double act_width = ICON_WIDTH / a;
                        x = (int)((ICON_WIDTH - act_width) / 2.0);
                        w = (int)act_width;
                    } else {
                        // アイコンより横長
                        double act_height = ICON_HEIGHT * a;
                        y = (int)((ICON_HEIGHT - act_height) / 2.0);
                        h = (int)act_height;
                    }
#if JAVA
                    g.drawImage( img, x, y, w, h, null );
#else
                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle( x, y, w, h );
                    System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle( 0, 0, img_width, img_height );
                    g.nativeGraphics.DrawImage( img.image, destRect, srcRect, System.Drawing.GraphicsUnit.Pixel );
#endif
                }
#if !JAVA
                g.nativeGraphics.FillRegion( getBrush(), getInvRegion() );
                g.nativeGraphics.DrawPath( System.Drawing.Pens.DarkGray, getGraphicsPath() );
#endif
            } catch ( Exception ex ) {
#if JAVA
                System.err.println( "IconParader#setImage; ex=" + ex );
#else
                Logger.write( typeof( IconParader ) + ".setImage; ex=" + ex + "\n" );
#endif
            } finally {
#if !JAVA
                if ( g != null ) {
                    g.nativeGraphics.Dispose();
                }
#endif
            }
            base.Image = bmp.image;
        }

#if !JAVA
        /// <summary>
        /// アイコンの4隅を塗りつぶすためのブラシを取得します
        /// </summary>
        /// <returns></returns>
        private System.Drawing.SolidBrush getBrush()
        {
            if ( brush == null ) {
                brush = new System.Drawing.SolidBrush( base.BackColor );
            } else {
                if ( brush.Color != base.BackColor ) {
                    brush.Color = base.BackColor;
                }
            }
            return brush;
        }
#endif

#if !JAVA
        /// <summary>
        /// 角の丸い枠線を表すGraphicsPathを取得します
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Drawing2D.GraphicsPath getGraphicsPath()
        {
            if ( graphicsPath == null ) {
                graphicsPath = new System.Drawing.Drawing2D.GraphicsPath();
                graphicsPath.StartFigure();
                int w = base.Width - 1;
                int h = base.Height - 1;
                // 上の横線
                graphicsPath.AddLine( RADIUS, 0, w - RADIUS, 0 );
                // 右上の角
                graphicsPath.AddArc( w - DIAMETER, 0, DIAMETER, DIAMETER, 270, 90 );
                // 右の縦線
                graphicsPath.AddLine( w, RADIUS, w, h - RADIUS );
                // 右下の角
                graphicsPath.AddArc( w - DIAMETER, h - DIAMETER, DIAMETER, DIAMETER, 0, 90 );
                // 下の横線
                graphicsPath.AddLine( w - RADIUS, h, RADIUS, h );
                // 左下の角
                graphicsPath.AddArc( 0, h - DIAMETER, DIAMETER, DIAMETER, 90, 90 );
                // 左の縦線
                graphicsPath.AddLine( 0, h - RADIUS, 0, RADIUS );
                // 左上の角
                graphicsPath.AddArc( 0, 0, DIAMETER, DIAMETER, 180, 90 );
                graphicsPath.CloseFigure();
            }
            return graphicsPath;
        }
#endif

#if !JAVA
        /// <summary>
        /// 角の丸いアイコンの画像領域を取得します
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Region getRegion()
        {
            if ( region == null ) {
                region = new System.Drawing.Region( getGraphicsPath() );
            }
            return region;
        }
#endif

#if !JAVA
        /// <summary>
        /// アイコンの画像領域以外の領域(4隅)を取得します
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Region getInvRegion()
        {
            if ( invRegion == null ) {
                invRegion = new System.Drawing.Region();
                invRegion.Exclude( getGraphicsPath() );
            }
            return invRegion;
        }
#endif
    }

#if !JAVA
}
#endif
