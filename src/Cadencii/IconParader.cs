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
using System;
using cadencii.java.awt;
using cadencii.windows.forms;



namespace cadencii
{

    /// <summary>
    /// 起動時のスプラッシュウィンドウに表示されるアイコンパレードの、1個のアイコンを表現します
    /// </summary>
    public class IconParader : System.Windows.Forms.PictureBox
    {
        const int RADIUS = 6; // 角の丸み
        const int DIAMETER = 2 * RADIUS;
        public const int ICON_WIDTH = 48;
        public const int ICON_HEIGHT = 48;

        private System.Drawing.Drawing2D.GraphicsPath graphicsPath = null;
        private System.Drawing.Region region = null;
        private System.Drawing.Region invRegion = null;
        private System.Drawing.SolidBrush brush = null;

        public IconParader()
        {
            var d = new System.Drawing.Size(ICON_WIDTH, ICON_HEIGHT);
            this.Size = d;
            this.MaximumSize = d;
            this.MinimumSize = d;
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        }

        public static Image createIconImage(string path_image, string singer_name)
        {
#if DEBUG
            sout.println("IconParader#createIconImage; path_image=" + path_image);
#endif
            Image ret = null;
            if (System.IO.File.Exists(path_image)) {
                System.IO.FileStream fs = null;
                try {
                    fs = new System.IO.FileStream(path_image, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
                    ret = new Image();
                    ret.image = img;
                } catch (Exception ex) {
                    serr.println("IconParader#createIconImage; ex=" + ex);
                } finally {
                    if (fs != null) {
                        try {
                            fs.Close();
                        } catch (Exception ex2) {
                            serr.println("IconParader#createIconImage; ex2=" + ex2);
                        }
                    }
                }
            }

            if (ret == null) {
                // 画像ファイルが無かったか，読み込みに失敗した場合

                // 歌手名が描かれた画像をセットする
                Image bmp = new Image();
                bmp.image = new System.Drawing.Bitmap(ICON_WIDTH, ICON_HEIGHT, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Graphics2D g = new Graphics2D(System.Drawing.Graphics.FromImage(bmp.image));
                g.clearRect(0, 0, ICON_WIDTH, ICON_HEIGHT);
                Font font = new Font(System.Windows.Forms.SystemInformation.MenuFont);
                PortUtil.drawStringEx(
                    (Graphics)g, singer_name, font, new Rectangle(1, 1, ICON_WIDTH - 2, ICON_HEIGHT - 2),
                    PortUtil.STRING_ALIGN_NEAR, PortUtil.STRING_ALIGN_NEAR);
                ret = bmp;
            }

            return ret;
        }

        public void setImage(Image img)
        {
            Image bmp = new Image();
            bmp.image = new System.Drawing.Bitmap(ICON_WIDTH, ICON_HEIGHT, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics g = null;
            try {
                g = new Graphics2D(System.Drawing.Graphics.FromImage(bmp.image));
                g.nativeGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                if (img != null) {
                    int img_width = img.getWidth(null);
                    int img_height = img.getHeight(null);
                    double a = img_height / (double)img_width;
                    double aspecto = ICON_HEIGHT / (double)ICON_WIDTH;

                    int x = 0;
                    int y = 0;
                    int w = ICON_WIDTH;
                    int h = ICON_HEIGHT;
                    if (a >= aspecto) {
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
                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, y, w, h);
                    System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle(0, 0, img_width, img_height);
                    g.nativeGraphics.DrawImage(img.image, destRect, srcRect, System.Drawing.GraphicsUnit.Pixel);
                }
                g.nativeGraphics.FillRegion(getBrush(), getInvRegion());
                g.nativeGraphics.DrawPath(System.Drawing.Pens.DarkGray, getGraphicsPath());
            } catch (Exception ex) {
                Logger.write(typeof(IconParader) + ".setImage; ex=" + ex + "\n");
            } finally {
                if (g != null) {
                    g.nativeGraphics.Dispose();
                }
            }
            base.Image = bmp.image;
        }

        /// <summary>
        /// アイコンの4隅を塗りつぶすためのブラシを取得します
        /// </summary>
        /// <returns></returns>
        private System.Drawing.SolidBrush getBrush()
        {
            if (brush == null) {
                brush = new System.Drawing.SolidBrush(base.BackColor);
            } else {
                if (brush.Color != base.BackColor) {
                    brush.Color = base.BackColor;
                }
            }
            return brush;
        }

        /// <summary>
        /// 角の丸い枠線を表すGraphicsPathを取得します
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Drawing2D.GraphicsPath getGraphicsPath()
        {
            if (graphicsPath == null) {
                graphicsPath = new System.Drawing.Drawing2D.GraphicsPath();
                graphicsPath.StartFigure();
                int w = base.Width - 1;
                int h = base.Height - 1;
                // 上の横線
                graphicsPath.AddLine(RADIUS, 0, w - RADIUS, 0);
                // 右上の角
                graphicsPath.AddArc(w - DIAMETER, 0, DIAMETER, DIAMETER, 270, 90);
                // 右の縦線
                graphicsPath.AddLine(w, RADIUS, w, h - RADIUS);
                // 右下の角
                graphicsPath.AddArc(w - DIAMETER, h - DIAMETER, DIAMETER, DIAMETER, 0, 90);
                // 下の横線
                graphicsPath.AddLine(w - RADIUS, h, RADIUS, h);
                // 左下の角
                graphicsPath.AddArc(0, h - DIAMETER, DIAMETER, DIAMETER, 90, 90);
                // 左の縦線
                graphicsPath.AddLine(0, h - RADIUS, 0, RADIUS);
                // 左上の角
                graphicsPath.AddArc(0, 0, DIAMETER, DIAMETER, 180, 90);
                graphicsPath.CloseFigure();
            }
            return graphicsPath;
        }

        /// <summary>
        /// 角の丸いアイコンの画像領域を取得します
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Region getRegion()
        {
            if (region == null) {
                region = new System.Drawing.Region(getGraphicsPath());
            }
            return region;
        }

        /// <summary>
        /// アイコンの画像領域以外の領域(4隅)を取得します
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Region getInvRegion()
        {
            if (invRegion == null) {
                invRegion = new System.Drawing.Region();
                invRegion.Exclude(getGraphicsPath());
            }
            return invRegion;
        }
    }

}
