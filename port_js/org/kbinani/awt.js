/*
 * awt.js
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.java == undefined ) org.kbinani.java = {};
if( org.kbinani.java.awt == undefined ) org.kbinani.java.awt = {};

/*    public class Icon {
        public System.Drawing.Image image;
    }

    public class ImageIcon : Icon {
        public ImageIcon( System.Drawing.Image image ) {
            this.image = image;
        }

        public ImageIcon( Image image ) {
            if ( image != null ) {
                this.image = image.image;
            }
        }
    }

    public class Image{
        public System.Drawing.Image image;

        public int getWidth( object observer ) {
            return image.Width;
        }

        public int getHeight( object observer ) {
            return image.Height;
        }
    }

    public class Cursor {
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

        public Cursor( int type ) {
            m_type = type;
            if ( m_type == CROSSHAIR_CURSOR ) {
                cursor = System.Windows.Forms.Cursors.Cross;
            } else if ( m_type == HAND_CURSOR ) {
                cursor = System.Windows.Forms.Cursors.Hand;
            } else if ( m_type == TEXT_CURSOR ) {
                cursor = System.Windows.Forms.Cursors.IBeam;
            } else if ( m_type == E_RESIZE_CURSOR ){
                cursor = System.Windows.Forms.Cursors.PanEast;
            } else if ( m_type == NE_RESIZE_CURSOR ){
                cursor = System.Windows.Forms.Cursors.PanNE;
            } else if ( m_type == N_RESIZE_CURSOR ){
                cursor = System.Windows.Forms.Cursors.PanNorth;
            } else if ( m_type == NW_RESIZE_CURSOR ) {
                cursor = System.Windows.Forms.Cursors.PanNW;
            } else if ( m_type == SE_RESIZE_CURSOR ){
                cursor = System.Windows.Forms.Cursors.PanSE;
            } else if ( m_type == S_RESIZE_CURSOR ){
                cursor = System.Windows.Forms.Cursors.PanSouth;
            } else if ( m_type == SW_RESIZE_CURSOR ){
                cursor = System.Windows.Forms.Cursors.PanSW;
            } else if( m_type == W_RESIZE_CURSOR ){
                cursor = System.Windows.Forms.Cursors.PanWest;
            } else if ( m_type == MOVE_CURSOR ){
                cursor = System.Windows.Forms.Cursors.SizeAll;
            }
        }

        public int getType() {
            return m_type;
        }
    }*/

org.kbinani.java.awt.Graphics = function(){
    this.nativeGraphics = null;
    this.color = "rgba(0,0,0,0)";
    if( arguments.length == 1 ){
        this._init_1( arguments[0] );
    }
};

org.kbinani.java.awt.Graphics.prototype = {
    _init_1 : function( g ){
        this.nativeGraphics = g;
    },

/*        public System.Drawing.Graphics nativeGraphics;
    public Color color = Color.black;
    public BasicStroke stroke = new BasicStroke();
    public System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush( System.Drawing.Color.Black );
    public System.Drawing.Font m_font = new System.Drawing.Font( "Arial", 10 );*/

    clearRect : function( x, y, width, height ) {
        var old_fillstyle = nativeGraphics.fillStyle;
        this.nativeGraphics.fillStyle = "rgba(0,0,0,0)";
        this.nativeGraphics.clearRect( x, y, width, height );
        this.nativeGraphics.fillStyle = old_fillstyle;
    },

    drawLine : function( x1, y1, x2, y2 ) {
        this.nativeGraphics.beginPath();
        this.nativeGraphics.moveTo( x1, y1 );
        this.nativeGraphics.lineTo( x2, y2 );
        this.nativeGraphics.stroke();
    },

    drawRect : function( x, y, width, height ) {
        this.nativeGraphics.strokeRect( x, y, width, height );
    },

    fillRect : function( x, y, width, height ) {
        this.nativeGraphics.fillRect( x, y, width, height );
    },

    /*public void drawOval( int x, int y, int width, int height ) {
        nativeGraphics.DrawEllipse( stroke.pen, x, y, width, height );
    }

    public void fillOval( int x, int y, int width, int height ) {
        nativeGraphics.FillRectangle( brush, x, y, width, height );
    }*/

    /**
     * @param c [Color]
     */
    setColor : function( c ) {
        this.nativeGraphics.fillStyle = c.toString();
        this.nativeGraphics.strokeStyle = c.toString();
    },

    /*public Color getColor() {
        return color;
    }*/

    /*public void setFont( Font font ) {
        m_font = font.font;
    }*/

    drawString : function( str, x, y ) {
        this.nativeGraphics.fillText( str, x, y );
    },

    /*public void drawPolygon( Polygon p ) {
        drawPolygon( p.xpoints, p.ypoints, p.npoints );
    }

    public void drawPolygon( int[] xPoints, int[] yPoints, int nPoints ) {
        System.Drawing.Point[] points = new System.Drawing.Point[nPoints];
        for ( int i = 0; i < nPoints; i++ ) {
            points[i] = new System.Drawing.Point( xPoints[i], yPoints[i] );
        }
        nativeGraphics.DrawPolygon( stroke.pen, points );
    }

    public void drawPolyline( int[] xPoints, int[] yPoints, int nPoints ) {
        System.Drawing.Point[] points = new System.Drawing.Point[nPoints];
        for ( int i = 0; i < nPoints; i++ ) {
            points[i] = new System.Drawing.Point( xPoints[i], yPoints[i] );
        }
        nativeGraphics.DrawLines( stroke.pen, points );
    }

    public void fillPolygon( Polygon p ) {
        fillPolygon( p.xpoints, p.ypoints, p.npoints );
    }

    public void fillPolygon( int[] xPoints, int[] yPoints, int nPoints ) {
        System.Drawing.Point[] points = new System.Drawing.Point[nPoints];
        for ( int i = 0; i < nPoints; i++ ) {
            points[i] = new System.Drawing.Point( xPoints[i], yPoints[i] );
        }
        nativeGraphics.FillPolygon( brush, points );
    }

    public void setStroke( Stroke stroke ) {
        if ( stroke is BasicStroke ) {
            BasicStroke bstroke = (BasicStroke)stroke;
            this.stroke.pen = bstroke.pen;
            this.stroke.pen.Color = color.color;
        }
    }

    public Stroke getStroke() {
        return stroke;
    }

    public Shape getClip() {
        Area ret = new Area();
        ret.region = nativeGraphics.Clip;
        return ret;
    }

    public void setClip( Shape clip ) {
        if ( clip == null ) {
            nativeGraphics.Clip = new System.Drawing.Region();
        } else if ( clip is Area ) {
            nativeGraphics.Clip = ((Area)clip).region;
        } else if ( clip is Rectangle ) {
            Rectangle rc = (Rectangle)clip;
            nativeGraphics.Clip = new System.Drawing.Region( new System.Drawing.Rectangle( rc.x, rc.y, rc.width, rc.height ) );
        } else {
            PortUtil.stderr.println( 
                "fixme: org.kbinani.java.awt.Graphics#setClip; argument type of clip is not supported for '" +
                clip.GetType() + "'." );
        }
    }

    public void clipRect( int x, int y, int width, int height ) {
        nativeGraphics.Clip = new System.Drawing.Region( new System.Drawing.Rectangle( x, y, width, height ) );
    }

    public void drawImage( org.kbinani.java.awt.image.BufferedImage img, int x, int y, object obs ) {
        if ( img is org.kbinani.java.awt.image.BufferedImage ) {
            nativeGraphics.DrawImage( ((org.kbinani.java.awt.image.BufferedImage)img).m_image, new System.Drawing.Point( x, y ) );
        }
    }

    public void drawImage( org.kbinani.java.awt.Image img, int x, int y, object obs ) {
        if ( img == null ) {
            return;
        }
        nativeGraphics.DrawImage( img.image, new System.Drawing.Point( x, y ) );
    }*/
};

/*    public class Graphics2D : Graphics{
        public Graphics2D( System.Drawing.Graphics g )
            : base( g ) {
        }

        public void fill( Shape s ) {
            if ( s == null ) {
                return;
            }

            if ( s is Area ) {
                Area a = (Area)s;
                if ( a.region != null ) {
                    nativeGraphics.FillRegion( brush, a.region );
                }
            } else if ( s is Rectangle ) {
                Rectangle rc = (Rectangle)s;
                nativeGraphics.FillRectangle( brush, rc.x, rc.y, rc.width, rc.height );
            } else {
                PortUtil.stderr.println(
                    "fixme; org.kbinani.java.awt.Graphics2D#fill; type of argument s is not supported for '" +
                    s.GetType() + "'." );
            }
        }
    }*/

org.kbinani.java.awt.Color = function(){
    this.r = 0;
    this.g = 0;
    this.b = 0;
    this.a = 255;
    if( arguments.length == 3 ){
        this._init_3( arguments[0], arguments[1], arguments[2] );
    }else if( arguments.length == 4 ){
        this._init_4( arguments[0], arguments[1], arguments[2], arguments[3] );
    }
};

org.kbinani.java.awt.Color.prototype = {
    clone : function(){
        return new org.kbinani.java.awt.Color( this.r, this.g, this.b, this.a );
    },

    _init_3 : function( r, g, b ) {
        this.r = r;
        this.g = g;
        this.b = b;
    },

    _init_4 : function( r, g, b, a ) {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    },

    getRed : function() {
        return this.r;
    },

    getGreen : function() {
        return this.g;
    },

    getBlue : function() {
        return this.b;
    },

    toString : function(){
        return "rgba(" + this.r + "," + this.g + "," + this.b + "," + this.a + ")";
    },
};

/**
 *  黒を表します。
 */
org.kbinani.java.awt.Color.black = new org.kbinani.java.awt.Color( 0, 0, 0 );
/**
 *  黒を表します。
 */
org.kbinani.java.awt.Color.BLACK = new org.kbinani.java.awt.Color( 0, 0, 0 );
/**
 *  青を表します。
 */
org.kbinani.java.awt.Color.blue = new org.kbinani.java.awt.Color( 0, 0, 255 );
/**
 *  青を表します。
 */
org.kbinani.java.awt.Color.BLUE = new org.kbinani.java.awt.Color( 0, 0, 255 );
/**
 *  シアンを表します。
 */
org.kbinani.java.awt.Color.cyan = new org.kbinani.java.awt.Color( 0, 255, 255 );
/**
 *  シアンを表します。
 */
org.kbinani.java.awt.Color.CYAN = new org.kbinani.java.awt.Color( 0, 255, 255 );
/**
 *  ダークグレイを表します。
 */
org.kbinani.java.awt.Color.DARK_GRAY = new org.kbinani.java.awt.Color( 169, 169, 169 );
/**
 *  ダークグレイを表します。
 */
org.kbinani.java.awt.Color.darkGray = new org.kbinani.java.awt.Color( 169, 169, 169 );
/**
 *  グレイを表します。
 */
org.kbinani.java.awt.Color.gray = new org.kbinani.java.awt.Color( 128, 128, 128 );
/**
 *  グレイを表します。
 */
org.kbinani.java.awt.Color.GRAY = new org.kbinani.java.awt.Color( 128, 128, 128 );
/**
 *  緑を表します。
 */
org.kbinani.java.awt.Color.green = new org.kbinani.java.awt.Color( 0, 255, 0 );
/**
 *  緑を表します。
 */
org.kbinani.java.awt.Color.GREEN = new org.kbinani.java.awt.Color( 0, 255, 0 );
/**
 *  ライトグレイを表します。
 */
org.kbinani.java.awt.Color.LIGHT_GRAY = new org.kbinani.java.awt.Color( 211, 211, 211 );
/**
 *  ライトグレイを表します。
 */
org.kbinani.java.awt.Color.lightGray = new org.kbinani.java.awt.Color( 211, 211, 211 );
/**
 *  マゼンタを表します。
 */
org.kbinani.java.awt.Color.magenta = new org.kbinani.java.awt.Color( 255, 0, 255 );
/**
 *  マゼンタを表します。
 */
org.kbinani.java.awt.Color.MAGENTA = new org.kbinani.java.awt.Color( 255, 0, 255 );
/**
 *  オレンジを表します。
 */
org.kbinani.java.awt.Color.orange = new org.kbinani.java.awt.Color( 255, 165, 0 );
/**
 *  オレンジを表します。
 */
org.kbinani.java.awt.Color.ORANGE = new org.kbinani.java.awt.Color( 255, 165, 0 );
/**
 *  ピンクを表します。
 */
org.kbinani.java.awt.Color.pink = new org.kbinani.java.awt.Color( 255, 192, 203 );
/**
 *  ピンクを表します。
 */
org.kbinani.java.awt.Color.PINK = new org.kbinani.java.awt.Color( 255, 192, 203 );
/**
 *  赤を表します。
 */
org.kbinani.java.awt.Color.red = new org.kbinani.java.awt.Color( 255, 0, 0 );
/**
 *  赤を表します。
 */
org.kbinani.java.awt.Color.RED = new org.kbinani.java.awt.Color( 255, 0, 0 );
/**
 *  白を表します。
 */
org.kbinani.java.awt.Color.white = new org.kbinani.java.awt.Color( 255, 255, 255 );
/**
 *  白を表します。
 */
org.kbinani.java.awt.Color.WHITE = new org.kbinani.java.awt.Color( 255, 255, 255 );
/**
 *  黄を表します。
 */
org.kbinani.java.awt.Color.yellow = new org.kbinani.java.awt.Color( 255, 255, 0 );
/**
 *  黄を表します。 
 */
org.kbinani.java.awt.Color.YELLOW = new org.kbinani.java.awt.Color( 255, 255, 0 );

org.kbinani.java.awt.Rectangle = function(){
    this.height = 0;
    this.width = 0;
    this.x = 0;
    this.y = 0;
    if( arguments.length == 1 ){
        this._init_1( arguments[0] );
    }else if( arguments.length == 2 ){
        this._init_2( arguments[0], arguments[1] );
    }else if( arguments.length == 4 ){
        this._init_4( arguments[0], arguments[1], arguments[2], arguments[3] );
    }
};

org.kbinani.java.awt.Rectangle.prototype = {
    _init_2 : function( width_, height_ ) {
        this.x = 0;
        this.y = 0;
        this.width = width_;
        this.height = height_;
    },

    _init_4 : function( x_, y_, width_, height_ ) {
        this.x = x_;
        this.y = y_;
        this.width = width_;
        this.height = height_;
    },

    _init_1 : function( r ) {
        this.x = r.x;
        this.y = r.y;
        this.width = r.width;
        this.height = r.height;
    },

    toString : function() {
        return "{x=" + this.x + ", y=" + this.y + ", width=" + this.width + ", height=" + this.height + "}";
    },
};

org.kbinani.java.awt.Point = function(){
    this.x = 0;
    this.y = 0;
    if( arguments.length == 1 ){
        this._init_1( arguments[0] );
    }else if( arguments.length == 2 ){
        this._init_2( arguments[0], arguments[1] );
    }
};

org.kbinani.java.awt.Point.prototype = {
    _init_2 : function( x_, y_ ) {
        this.x = x_;
        this.y = y_;
    },

    _init_1 : function( p ) {
        this.x = p.x;
        this.y = p.y;
    },
};

/*    public class Font {
        public const int PLAIN = 0;
        public const int ITALIC = 2;
        public const int BOLD = 1;
        public System.Drawing.Font font;

        public Font( System.Drawing.Font value ) {
            font = value;
        }

        public Font( string name, int style, int size ) {
            System.Drawing.FontStyle fstyle = System.Drawing.FontStyle.Regular;
            if ( style >= Font.BOLD ) {
                fstyle = fstyle | System.Drawing.FontStyle.Bold;
            }
            if ( style >= Font.ITALIC ) {
                fstyle = fstyle | System.Drawing.FontStyle.Italic;
            }
            font = new System.Drawing.Font( name, size, fstyle );
        }

        public string getName() {
            return font.Name;
        }

        public int getSize() {
            return (int)font.SizeInPoints;
        }

        public float getSize2D() {
            return font.SizeInPoints;
        }
    }

    public interface Stroke {
    }

    public class BasicStroke : Stroke {
        public const int CAP_BUTT = 0;
        public const int CAP_ROUND = 1;
        public const int CAP_SQUARE = 2;
        public const int JOIN_BEVEL = 2;
        public const int JOIN_MITER = 0;
        public const int JOIN_ROUND = 1;
        public System.Drawing.Pen pen;

        public BasicStroke() {
            pen = new System.Drawing.Pen( System.Drawing.Color.Black );
        }

        public BasicStroke( float width )
            : this( width, 0, 0, 10.0f ) {
        }

        public BasicStroke( float width, int cap, int join )
            : this( width, cap, join, 10.0f ) {
        }

        public BasicStroke( float width, int cap, int join, float miterlimit ) {
            pen = new System.Drawing.Pen( System.Drawing.Color.Black, width );
            System.Drawing.Drawing2D.LineCap linecap = System.Drawing.Drawing2D.LineCap.Flat;
            if ( cap == 1 ) {
                linecap = System.Drawing.Drawing2D.LineCap.Round;
            } else if ( cap == 2 ) {
                linecap = System.Drawing.Drawing2D.LineCap.Square;
            }
            pen.StartCap = linecap;
            pen.EndCap = linecap;
            System.Drawing.Drawing2D.LineJoin linejoin = System.Drawing.Drawing2D.LineJoin.Miter;
            if ( join == 1 ) {
                linejoin = System.Drawing.Drawing2D.LineJoin.Round;
            } else if ( join == 2 ) {
                linejoin = System.Drawing.Drawing2D.LineJoin.Bevel;
            }
            pen.LineJoin = linejoin;
            pen.MiterLimit = miterlimit;
        }

        public BasicStroke( float width, int cap, int join, float miterlimit, float[] dash, float dash_phase )
            : this( width, cap, join, miterlimit ) {
            pen.DashPattern = dash;
            pen.DashOffset = dash_phase;
        }
    }

    public class Polygon {
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

        public Polygon() {
            npoints = 0;
            xpoints = new int[0];
            ypoints = new int[0];
        }

        public Polygon( int[] xpoints_, int[] ypoints_, int npoints_ ) {
            npoints = npoints_;
            xpoints = xpoints_;
            ypoints = ypoints_;
        }
    }

    public interface Shape {
    }*/

org.kbinani.java.awt.Dimension = function(){
    this.height = 0;
    this.width = 0;
    if( arguments.length == 2 ){
        this._init_2( arguments[0], arguments[1] );
    }
};

org.kbinani.java.awt.Dimension.prototype = {
    _init_2 : function( width_, height_ ) {
        this.width = width_;
        this.height = height_;
    },
};

/*namespace org.kbinani.java.awt.geom {
    public class Area : Shape {
        public System.Drawing.Region region;

        public Area() {
            region = new System.Drawing.Region();
            region.MakeEmpty();
        }

        public Area( Shape s ) {
            if ( s == null ) {
                region = new System.Drawing.Region();
            } else if ( s is Area ) {
                Area a = (Area)s;
                if ( a.region == null ) {
                    region = new System.Drawing.Region();
                } else {
                    region = (System.Drawing.Region)a.region.Clone();
                }
            } else if ( s is Rectangle ) {
                Rectangle rc = (Rectangle)s;
                region = new System.Drawing.Region( new System.Drawing.Rectangle( rc.x, rc.y, rc.width, rc.height ) );
            } else {
                PortUtil.stderr.println(
                    "fixme: org.kbinani.java.awt.Area#.ctor(org.kbinani.java.awt.Shape); type of argument s is not supported for '" +
                    s.GetType() + "'." );
                region = new System.Drawing.Region();
            }
        }

        public void add( Area rhs ) {
            if ( rhs == null ) {
                return;
            }
            if ( rhs.region == null ) {
                return;
            }
            if ( region == null ) {
                region = new System.Drawing.Region();
            }
            region.Union( rhs.region );
        }

        public void subtract( Area rhs ) {
            if ( rhs == null ) {
                return;
            }
            if ( rhs.region == null ) {
                return;
            }
            if ( region == null ) {
                region = new System.Drawing.Region();
            }
            region.Exclude( rhs.region );
        }

        public Object clone() {
            Area ret = new Area();
            if ( region == null ) {
                ret.region = new System.Drawing.Region();
            } else {
                ret.region = (System.Drawing.Region)region.Clone();
            }
            return ret;
        }
    }
}
#endif
*/
