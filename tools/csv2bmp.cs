using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

class csv2bmp{
    public static void Main( string[] args )
    {
        byte[,] r = null;
        byte[,] g = null;
        byte[,] b = null;
        byte[,] a = null;
        string current = "";
        string file = "";
        foreach( string s in args ){
            if( s.StartsWith( "-" ) ){
                current = s;
            }else{
                if( current == "-a" ){
                    a = load_csv( s );
                }else if( current == "-r" ){
                    r = load_csv( s );
                }else if( current == "-g" ){
                    g = load_csv( s );
                }else if( current == "-b" ){
                    b = load_csv( s );
                }else if( current == "-o" ){
                    file = s;
                }
                current = "";
            }
        }
        if( file == "" ){
            Console.WriteLine( "error; output file not specified" );
            return;
        }
        int w = 0;
        int h = 0;
        int rx = (r != null) ? r.GetLength( 0 ) : 0;
        int ry = (r != null) ? r.GetLength( 1 ) : 0;
        int gx = (g != null) ? g.GetLength( 0 ) : 0;
        int gy = (g != null) ? g.GetLength( 1 ) : 0;
        int bx = (b != null) ? b.GetLength( 0 ) : 0;
        int by = (b != null) ? b.GetLength( 1 ) : 0;
        int ax = (a != null) ? a.GetLength( 0 ) : 0;
        int ay = (a != null) ? a.GetLength( 1 ) : 0;
        if( r != null ){
            w = Math.Max( w, rx );
            h = Math.Max( h, ry );
        }
        if( g != null ){
            w = Math.Max( w, gx );
            h = Math.Max( h, gy );
        }
        if( b != null ){
            w = Math.Max( w, bx );
            h = Math.Max( h, by );
        }
        if( a != null ){
            w = Math.Max( w, ax );
            h = Math.Max( h, ay );
        }
        if( w <= 0 || h <= 0 ){
            Console.WriteLine( "error; width=" + w + "; height=" + h );
            return;
        }
        Bitmap bmp = new Bitmap( w, h, PixelFormat.Format32bppArgb );
        for( int j = 0; j < h; j++ ){
            for( int i = 0; i < w; i++ ){
                byte red = 0;
                if( r != null && i < rx && j < ry ){
                    red = r[i, j];
                }
                byte green = 0;
                if( g != null && i < gx && j < gy ){
                    green = g[i, j];
                }
                byte blue = 0;
                if( b != null && i < bx && j < by ){
                    blue = b[i, j];
                }
                byte alpha = 0;
                if( a != null && i < ax && j < ay ){
                    alpha = a[i, j];
                }
                Color c = Color.FromArgb( alpha, red, green, blue );
                bmp.SetPixel( i, j, c );
            }
        }
        bmp.Save( file, ImageFormat.Png );
    }

    public static byte[,] load_csv( string file )
    {
        int lines = 0;
        int max_colum = 0;
        List<byte[]> t = new List<byte[]>();
        using( StreamReader sr = new StreamReader( file ) ){
            string line = "";
            while( (line = sr.ReadLine()) != null ){
                string[] spl = line.Split( ',' );
                byte[] l = new byte[spl.Length];
                for( int i = 0; i < l.Length; i++ ){
                    l[i] = byte.Parse( spl[i] );
                }
                t.Add( l );
                max_colum = Math.Max( max_colum, spl.Length );
                lines++;
            }
        }
        byte[,] ret = new byte[max_colum, lines];
        for( int j = 0; j < lines; j++ ){
            byte[] src = t[j];
            for( int i = 0; i < src.Length; i++ ){
                ret[i, j] = src[i];
            }
        }
        return ret;
    }
}
