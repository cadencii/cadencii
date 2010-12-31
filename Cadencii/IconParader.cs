/*
 * IconParader.cs
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if !JAVA
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace org.kbinani.cadencii {

    /// <summary>
    /// 起動時のスプラッシュウィンドウに表示されるアイコンパレードの、1個のアイコンを表現します
    /// </summary>
    public class IconParader : PictureBox {
        const int RADIUS = 6; // 角の丸み
        const int DIAMETER = 2 * RADIUS;
        public const int ICON_WIDTH = 48;
        public const int ICON_HEIGHT = 48;

        private GraphicsPath graphicsPath = null;
        private Region region = null;
        private Region invRegion = null;
        private SolidBrush brush = null;

        public IconParader() {
            Size s = new Size( ICON_WIDTH, ICON_HEIGHT );
            base.Size = s;
            base.MaximumSize = s;
            base.MinimumSize = s;
            base.SizeMode = PictureBoxSizeMode.Zoom;
        }

        public void setImage( Image img ) {
            Bitmap bmp = new Bitmap( ICON_WIDTH, ICON_HEIGHT );
            Graphics g = null;
            try {
                g = Graphics.FromImage( bmp );
                g.SmoothingMode = SmoothingMode.HighQuality;
                if ( img != null ) {
                    double a = img.Height / (double)img.Width;
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
                    Rectangle destRect = new Rectangle( x, y, w, h );
                    Rectangle srcRect = new Rectangle( 0, 0, img.Width, img.Height );
                    g.DrawImage( img, destRect, srcRect, GraphicsUnit.Pixel );
                }
                g.FillRegion( getBrush(), getInvRegion() );
                g.DrawPath( Pens.DarkGray, getGraphicsPath() );
            } catch ( Exception ex ) {
                Logger.write( typeof( IconParader ) + ".setImage; ex=" + ex + "\n" );
            } finally {
                if ( g != null ) {
                    g.Dispose();
                }
            }
            base.Image = bmp;
        }

        /*/// <summary>
        /// オーバーライドされます。4隅を塗りつぶし、枠線を描く処理が追加されています。
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint( PaintEventArgs pe ) {
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            base.OnPaint( pe );
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pe.Graphics.FillRegion( getBrush(), getInvRegion() );
            pe.Graphics.DrawPath( Pens.DarkGray, getGraphicsPath() );
        }*/

        /// <summary>
        /// アイコンの4隅を塗りつぶすためのブラシを取得します
        /// </summary>
        /// <returns></returns>
        private SolidBrush getBrush() {
            if ( brush == null ) {
                brush = new SolidBrush( base.BackColor );
            } else {
                if ( brush.Color != base.BackColor ) {
                    brush.Color = base.BackColor;
                }
            }
            return brush;
        }

        /// <summary>
        /// 角の丸い枠線を表すGraphicsPathを取得します
        /// </summary>
        /// <returns></returns>
        private GraphicsPath getGraphicsPath() {
            if ( graphicsPath == null ) {
                graphicsPath = new GraphicsPath();
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

        /// <summary>
        /// 角の丸いアイコンの画像領域を取得します
        /// </summary>
        /// <returns></returns>
        private Region getRegion() {
            if ( region == null ) {
                region = new Region( getGraphicsPath() );
            }
            return region;
        }

        /// <summary>
        /// アイコンの画像領域以外の領域(4隅)を取得します
        /// </summary>
        /// <returns></returns>
        private Region getInvRegion() {
            if ( invRegion == null ) {
                invRegion = new Region();
                invRegion.Exclude( getGraphicsPath() );
            }
            return invRegion;
        }
    }

}
#endif
