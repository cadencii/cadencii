using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

class bmp2csv{
    public static void Main( string[] args )
    {
        string file = args[0];
        Bitmap bmp = null;
        FileStream fs = null;
        try{
            fs = new FileStream( file, FileMode.Open, FileAccess.Read );
            bmp = new Bitmap( fs );
        }catch( Exception ex ){
            bmp = null;
        }finally{
            if( fs != null ){
                fs.Close();
            }
        }
        if( bmp == null ){
            Console.WriteLine( "error; failed loadin image '" + file + "'" );
            return;
        }
        
        string dir = Path.GetDirectoryName( file );
        string name = Path.GetFileNameWithoutExtension( file );
        int w = bmp.Width;
        int h = bmp.Height;
        using( StreamWriter swr = new StreamWriter( Path.Combine( dir, name + "_R.csv" ) ) )
        using( StreamWriter swg = new StreamWriter( Path.Combine( dir, name + "_G.csv" ) ) )
        using( StreamWriter swb = new StreamWriter( Path.Combine( dir, name + "_B.csv" ) ) )
        using( StreamWriter swa = new StreamWriter( Path.Combine( dir, name + "_A.csv" ) ) ){
            for( int j = 0; j < h; j++ ){
                for( int i = 0; i < w; i++ ){
                    Color c = bmp.GetPixel( i, j );
                    swr.Write( c.R + (i == w - 1 ? "" : ",") );
                    swg.Write( c.G + (i == w - 1 ? "" : ",") );
                    swb.Write( c.B + (i == w - 1 ? "" : ",") );
                    swa.Write( c.A + (i == w - 1 ? "" : ",") );
                }
                swr.WriteLine();
                swg.WriteLine();
                swb.WriteLine();
                swa.WriteLine();
            }
        }
    }
}
