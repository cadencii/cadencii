/*
 * awt.cs
 * Copyright © 2009-2011 kbinani
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
using cadencii.java.awt;
using cadencii.java.awt.geom;

namespace cadencii.java.awt
{

    public class Image
    {
        public System.Drawing.Image image;

        public int getWidth(object observer)
        {
            return image.Width;
        }

        public int getHeight(object observer)
        {
            return image.Height;
        }
    }

    public class Cursor
    {
        public const int CROSSHAIR_CURSOR = 1;
        public const int CUSTOM_CURSOR = -1;
        public const int DEFAULT_CURSOR = 0;
        public const int E_RESIZE_CURSOR = 11;
        public const int HAND_CURSOR = 12;
        public const int MOVE_CURSOR = 13;
        public const int N_RESIZE_CURSOR = 8;
        public const int NE_RESIZE_CURSOR = 7;
        public const int NW_RESIZE_CURSOR = 6;
        public const int S_RESIZE_CURSOR = 9;
        public const int SE_RESIZE_CURSOR = 5;
        public const int SW_RESIZE_CURSOR = 4;
        public const int TEXT_CURSOR = 2;
        public const int W_RESIZE_CURSOR = 10;
        public const int WAIT_CURSOR = 3;

        private int m_type = DEFAULT_CURSOR;
        public System.Windows.Forms.Cursor cursor = System.Windows.Forms.Cursors.Default;

        public Cursor(int type)
        {
            m_type = type;
            if (m_type == CROSSHAIR_CURSOR) {
                cursor = System.Windows.Forms.Cursors.Cross;
            } else if (m_type == HAND_CURSOR) {
                cursor = System.Windows.Forms.Cursors.Hand;
            } else if (m_type == TEXT_CURSOR) {
                cursor = System.Windows.Forms.Cursors.IBeam;
            } else if (m_type == E_RESIZE_CURSOR) {
                cursor = System.Windows.Forms.Cursors.PanEast;
            } else if (m_type == NE_RESIZE_CURSOR) {
                cursor = System.Windows.Forms.Cursors.PanNE;
            } else if (m_type == N_RESIZE_CURSOR) {
                cursor = System.Windows.Forms.Cursors.PanNorth;
            } else if (m_type == NW_RESIZE_CURSOR) {
                cursor = System.Windows.Forms.Cursors.PanNW;
            } else if (m_type == SE_RESIZE_CURSOR) {
                cursor = System.Windows.Forms.Cursors.PanSE;
            } else if (m_type == S_RESIZE_CURSOR) {
                cursor = System.Windows.Forms.Cursors.PanSouth;
            } else if (m_type == SW_RESIZE_CURSOR) {
                cursor = System.Windows.Forms.Cursors.PanSW;
            } else if (m_type == W_RESIZE_CURSOR) {
                cursor = System.Windows.Forms.Cursors.PanWest;
            } else if (m_type == MOVE_CURSOR) {
                cursor = System.Windows.Forms.Cursors.SizeAll;
            }
        }

        public int getType()
        {
            return m_type;
        }
    }

    public class Graphics
    {
        public System.Drawing.Graphics nativeGraphics;
        public Color color = Color.black;
        public BasicStroke stroke = new BasicStroke();
        public System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        public System.Drawing.Font m_font = new System.Drawing.Font("Arial", 10);

        public Graphics(System.Drawing.Graphics g)
        {
            nativeGraphics = g;
        }

        public void clearRect(int x, int y, int width, int height)
        {
            nativeGraphics.FillRectangle(System.Drawing.Brushes.White, x, y, width, height);
        }

        public void drawLine(int x1, int y1, int x2, int y2)
        {
            nativeGraphics.DrawLine(stroke.pen, x1, y1, x2, y2);
        }

        public void drawRect(int x, int y, int width, int height)
        {
            nativeGraphics.DrawRectangle(stroke.pen, x, y, width, height);
        }

        public void fillRect(int x, int y, int width, int height)
        {
            nativeGraphics.FillRectangle(brush, x, y, width, height);
        }

        public void drawOval(int x, int y, int width, int height)
        {
            nativeGraphics.DrawEllipse(stroke.pen, x, y, width, height);
        }

        public void fillOval(int x, int y, int width, int height)
        {
            nativeGraphics.FillEllipse(brush, x, y, width, height);
        }

        public void setColor(Color c)
        {
            color = c;
            stroke.pen.Color = c.color;
            brush.Color = c.color;
        }

        public Color getColor()
        {
            return color;
        }

        public void setFont(Font font)
        {
            m_font = font.font;
        }

        public void drawString(string str, float x, float y)
        {
            nativeGraphics.DrawString(str, m_font, brush, x, y);
        }

        public void drawPolygon(Polygon p)
        {
            drawPolygon(p.xpoints, p.ypoints, p.npoints);
        }

        public void drawPolygon(int[] xPoints, int[] yPoints, int nPoints)
        {
            System.Drawing.Point[] points = new System.Drawing.Point[nPoints];
            for (int i = 0; i < nPoints; i++) {
                points[i] = new System.Drawing.Point(xPoints[i], yPoints[i]);
            }
            nativeGraphics.DrawPolygon(stroke.pen, points);
        }

        public void drawPolyline(int[] xPoints, int[] yPoints, int nPoints)
        {
            System.Drawing.Point[] points = new System.Drawing.Point[nPoints];
            for (int i = 0; i < nPoints; i++) {
                points[i] = new System.Drawing.Point(xPoints[i], yPoints[i]);
            }
            nativeGraphics.DrawLines(stroke.pen, points);
        }

        public void fillPolygon(Polygon p)
        {
            fillPolygon(p.xpoints, p.ypoints, p.npoints);
        }

        public void fillPolygon(int[] xPoints, int[] yPoints, int nPoints)
        {
            System.Drawing.Point[] points = new System.Drawing.Point[nPoints];
            for (int i = 0; i < nPoints; i++) {
                points[i] = new System.Drawing.Point(xPoints[i], yPoints[i]);
            }
            nativeGraphics.FillPolygon(brush, points);
        }

        public Shape getClip()
        {
            Area ret = new Area();
            ret.region = nativeGraphics.Clip;
            return ret;
        }

        public void setClip(int x, int y, int width, int height)
        {
            nativeGraphics.SetClip(new System.Drawing.Rectangle(x, y, width, height));
        }

        public void setClip(Shape clip)
        {
            if (clip == null) {
                nativeGraphics.Clip = new System.Drawing.Region();
            } else if (clip is Area) {
                nativeGraphics.Clip = ((Area)clip).region;
            } else if (clip is Rectangle) {
                Rectangle rc = (Rectangle)clip;
                nativeGraphics.Clip = new System.Drawing.Region(new System.Drawing.Rectangle(rc.x, rc.y, rc.width, rc.height));
            } else {
                serr.println(
                    "fixme: org.kbinani.java.awt.Graphics#setClip; argument type of clip is not supported for '" +
                    clip.GetType() + "'.");
            }
        }

        public void clipRect(int x, int y, int width, int height)
        {
            nativeGraphics.Clip = new System.Drawing.Region(new System.Drawing.Rectangle(x, y, width, height));
        }

        public void drawImage(System.Drawing.Image img, int x, int y, object obs)
        {
            nativeGraphics.DrawImage(img, new System.Drawing.Point(x, y));
        }

        public void drawImage(cadencii.java.awt.Image img, int x, int y, object obs)
        {
            if (img == null) {
                return;
            }
            drawImage(img.image, x, y, obs);
        }
    }

    public class Graphics2D : Graphics
    {
        public Graphics2D(System.Drawing.Graphics g)
            : base(g)
        {
        }

        public void setStroke(Stroke stroke)
        {
            if (stroke is BasicStroke) {
                BasicStroke bstroke = (BasicStroke)stroke;
                this.stroke.pen = bstroke.pen;
                this.stroke.pen.Color = color.color;
            }
        }

        public Stroke getStroke()
        {
            return stroke;
        }

        public void translate(int tx, int ty)
        {
            nativeGraphics.TranslateTransform(tx, ty);
        }

        public void translate(double tx, double ty)
        {
            nativeGraphics.TranslateTransform((float)tx, (float)ty);
        }

        public void fill(Shape s)
        {
            if (s == null) {
                return;
            }

            if (s is Area) {
                Area a = (Area)s;
                if (a.region != null) {
                    nativeGraphics.FillRegion(brush, a.region);
                }
            } else if (s is Rectangle) {
                Rectangle rc = (Rectangle)s;
                nativeGraphics.FillRectangle(brush, rc.x, rc.y, rc.width, rc.height);
            } else {
                serr.println(
                    "fixme; org.kbinani.java.awt.Graphics2D#fill; type of argument s is not supported for '" +
                    s.GetType() + "'.");
            }
        }
    }

    /*public interface Image{
        int getHeight( object observer );
        int getWidth( object observer );
    }*/

    [Serializable]
    public struct Color
    {
        /// <summary>
        /// 黒を表します。
        /// </summary>
        public static Color black = new Color(System.Drawing.Color.Black);
        /// <summary>
        /// 黒を表します。
        /// </summary>
        public static Color BLACK = new Color(System.Drawing.Color.Black);
        /// <summary>
        /// 青を表します。
        /// </summary>
        public static Color blue = new Color(System.Drawing.Color.Blue);
        /// <summary>
        /// 青を表します。
        /// </summary>
        public static Color BLUE = new Color(System.Drawing.Color.Blue);
        /// <summary>
        /// シアンを表します。
        /// </summary>
        public static Color cyan = new Color(System.Drawing.Color.Cyan);
        /// <summary>
        /// シアンを表します。
        /// </summary>
        public static Color CYAN = new Color(System.Drawing.Color.Cyan);
        /// <summary>
        /// ダークグレイを表します。
        /// </summary>
        public static Color DARK_GRAY = new Color(System.Drawing.Color.DarkGray);
        /// <summary>
        /// ダークグレイを表します。
        /// </summary>
        public static Color darkGray = new Color(System.Drawing.Color.DarkGray);
        /// <summary>
        /// グレイを表します。
        /// </summary>
        public static Color gray = new Color(System.Drawing.Color.Gray);
        /// <summary>
        /// グレイを表します。
        /// </summary>
        public static Color GRAY = new Color(System.Drawing.Color.Gray);
        /// <summary>
        /// 緑を表します。
        /// </summary>
        public static Color green = new Color(System.Drawing.Color.Green);
        /// <summary>
        /// 緑を表します。
        /// </summary>
        public static Color GREEN = new Color(System.Drawing.Color.Green);
        /// <summary>
        /// ライトグレイを表します。
        /// </summary>
        public static Color LIGHT_GRAY = new Color(System.Drawing.Color.LightGray);
        /// <summary>
        /// ライトグレイを表します。
        /// </summary>
        public static Color lightGray = new Color(System.Drawing.Color.LightGray);
        /// <summary>
        /// マゼンタを表します。
        /// </summary>
        public static Color magenta = new Color(System.Drawing.Color.Magenta);
        /// <summary>
        /// マゼンタを表します。
        /// </summary>
        public static Color MAGENTA = new Color(System.Drawing.Color.Magenta);
        /// <summary>
        /// オレンジを表します。
        /// </summary>
        public static Color orange = new Color(System.Drawing.Color.Orange);
        /// <summary>
        /// オレンジを表します。
        /// </summary>
        public static Color ORANGE = new Color(System.Drawing.Color.Orange);
        /// <summary>
        /// ピンクを表します。
        /// </summary>
        public static Color pink = new Color(System.Drawing.Color.Pink);
        /// <summary>
        /// ピンクを表します。
        /// </summary>
        public static Color PINK = new Color(System.Drawing.Color.Pink);
        /// <summary>
        /// 赤を表します。
        /// </summary>
        public static Color red = new Color(System.Drawing.Color.Red);
        /// <summary>
        /// 赤を表します。
        /// </summary>
        public static Color RED = new Color(System.Drawing.Color.Red);
        /// <summary>
        /// 白を表します。
        /// </summary>
        public static Color white = new Color(System.Drawing.Color.White);
        /// <summary>
        /// 白を表します。
        /// </summary>
        public static Color WHITE = new Color(System.Drawing.Color.White);
        /// <summary>
        /// 黄を表します。
        /// </summary>
        public static Color yellow = new Color(System.Drawing.Color.Yellow);
        /// <summary>
        /// 黄を表します。 
        /// </summary>
        public static Color YELLOW = new Color(System.Drawing.Color.Yellow);

        public System.Drawing.Color color;

        public Color(System.Drawing.Color value)
        {
            color = value;
        }

        public Color(int r, int g, int b)
        {
            color = System.Drawing.Color.FromArgb(r, g, b);
        }

        public Color(int r, int g, int b, int a)
        {
            color = System.Drawing.Color.FromArgb(a, r, g, b);
        }

        public int getRed()
        {
            return color.R;
        }

        public int getGreen()
        {
            return color.G;
        }

        public int getBlue()
        {
            return color.B;
        }
    }

    [Serializable]
    public struct Rectangle : Shape
    {
        public int height;
        public int width;
        public int x;
        public int y;

        public Rectangle(int width_, int height_)
        {
            x = 0;
            y = 0;
            width = width_;
            height = height_;
        }

        public Rectangle(int x_, int y_, int width_, int height_)
        {
            x = x_;
            y = y_;
            width = width_;
            height = height_;
        }

        public Rectangle(Rectangle r)
        {
            x = r.x;
            y = r.y;
            width = r.width;
            height = r.height;
        }

        public override string ToString()
        {
            return "{x=" + x + ", y=" + y + ", width=" + width + ", height=" + height + "}";
        }
    }

    [Serializable]
    public struct Point
    {
        public int x;
        public int y;

        public Point(int x_, int y_)
        {
            x = x_;
            y = y_;
        }

        public Point(Point p)
        {
            x = p.x;
            y = p.y;
        }
    }

    public class Font
    {
        public const int PLAIN = 0;
        public const int ITALIC = 2;
        public const int BOLD = 1;
        public System.Drawing.Font font;

        public Font(System.Drawing.Font value)
        {
            font = value;
        }

        public Font(string name, int style, int size)
        {
            System.Drawing.FontStyle fstyle = System.Drawing.FontStyle.Regular;
            if (style >= Font.BOLD) {
                fstyle = fstyle | System.Drawing.FontStyle.Bold;
            }
            if (style >= Font.ITALIC) {
                fstyle = fstyle | System.Drawing.FontStyle.Italic;
            }
            font = new System.Drawing.Font(name, size, fstyle);
        }

        public string getName()
        {
            return font.Name;
        }

        public int getSize()
        {
            return (int)font.SizeInPoints;
        }

        public float getSize2D()
        {
            return font.SizeInPoints;
        }
    }

    public interface Stroke
    {
    }

    public class BasicStroke : Stroke
    {
        public const int CAP_BUTT = 0;
        public const int CAP_ROUND = 1;
        public const int CAP_SQUARE = 2;
        public const int JOIN_BEVEL = 2;
        public const int JOIN_MITER = 0;
        public const int JOIN_ROUND = 1;
        public System.Drawing.Pen pen;

        public BasicStroke()
        {
            pen = new System.Drawing.Pen(System.Drawing.Color.Black);
        }

        public BasicStroke(float width)
            : this(width, 0, 0, 10.0f)
        {
        }

        public BasicStroke(float width, int cap, int join)
            : this(width, cap, join, 10.0f)
        {
        }

        public BasicStroke(float width, int cap, int join, float miterlimit)
        {
            pen = new System.Drawing.Pen(System.Drawing.Color.Black, width);
            System.Drawing.Drawing2D.LineCap linecap = System.Drawing.Drawing2D.LineCap.Flat;
            if (cap == 1) {
                linecap = System.Drawing.Drawing2D.LineCap.Round;
            } else if (cap == 2) {
                linecap = System.Drawing.Drawing2D.LineCap.Square;
            }
            pen.StartCap = linecap;
            pen.EndCap = linecap;
            System.Drawing.Drawing2D.LineJoin linejoin = System.Drawing.Drawing2D.LineJoin.Miter;
            if (join == 1) {
                linejoin = System.Drawing.Drawing2D.LineJoin.Round;
            } else if (join == 2) {
                linejoin = System.Drawing.Drawing2D.LineJoin.Bevel;
            }
            pen.LineJoin = linejoin;
            pen.MiterLimit = miterlimit;
        }

        public BasicStroke(float width, int cap, int join, float miterlimit, float[] dash, float dash_phase)
            : this(width, cap, join, miterlimit)
        {
            pen.DashPattern = dash;
            pen.DashOffset = dash_phase;
        }
    }

    public class Polygon
    {
        /// <summary>
        /// 点の総数です。
        /// </summary>
        public int npoints;
        /// <summary>
        /// X 座標の配列です。
        /// </summary>
        public int[] xpoints;
        /// <summary>
        /// Y 座標の配列です。
        /// </summary>
        public int[] ypoints;

        public Polygon()
        {
            npoints = 0;
            xpoints = new int[0];
            ypoints = new int[0];
        }

        public Polygon(int[] xpoints_, int[] ypoints_, int npoints_)
        {
            npoints = npoints_;
            xpoints = xpoints_;
            ypoints = ypoints_;
        }
    }

    public interface Shape
    {
    }

    public struct Dimension
    {
        public int height;
        public int width;

        public Dimension(int width_, int height_)
        {
            width = width_;
            height = height_;
        }
    }

    public class Frame : System.Windows.Forms.Form
    {
    }

}

namespace cadencii.java.awt.geom
{
    public class Area : Shape
    {
        public System.Drawing.Region region;

        public Area()
        {
            region = new System.Drawing.Region();
            region.MakeEmpty();
        }

        public Area(Shape s)
        {
            if (s == null) {
                region = new System.Drawing.Region();
            } else if (s is Area) {
                Area a = (Area)s;
                if (a.region == null) {
                    region = new System.Drawing.Region();
                } else {
                    region = (System.Drawing.Region)a.region.Clone();
                }
            } else if (s is Rectangle) {
                Rectangle rc = (Rectangle)s;
                region = new System.Drawing.Region(new System.Drawing.Rectangle(rc.x, rc.y, rc.width, rc.height));
            } else {
                serr.println(
                    "fixme: org.kbinani.java.awt.Area#.ctor(org.kbinani.java.awt.Shape); type of argument s is not supported for '" +
                    s.GetType() + "'.");
                region = new System.Drawing.Region();
            }
        }

        public void add(Area rhs)
        {
            if (rhs == null) {
                return;
            }
            if (rhs.region == null) {
                return;
            }
            if (region == null) {
                region = new System.Drawing.Region();
            }
            region.Union(rhs.region);
        }

        public void subtract(Area rhs)
        {
            if (rhs == null) {
                return;
            }
            if (rhs.region == null) {
                return;
            }
            if (region == null) {
                region = new System.Drawing.Region();
            }
            region.Exclude(rhs.region);
        }

        public Object clone()
        {
            Area ret = new Area();
            if (region == null) {
                ret.region = new System.Drawing.Region();
            } else {
                ret.region = (System.Drawing.Region)region.Clone();
            }
            return ret;
        }
    }
}
