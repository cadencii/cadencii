using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

class movie
{

    public static void Mljluhjbyjfain( string[] args )
    {
        using( Bitmap bmp = new Bitmap( 360, 50 ) ){
            for ( int i = 0; i < 360; i++ ) {
                Color c = colorFromPhase( i );
                for ( int j = 0; j < 50; j++ ) {
                    bmp.SetPixel( i, j, c );
                }
            }
            bmp.Save( "foo.png", ImageFormat.Png );
        }
    }

    /// <summary>
    /// 位相の値に対応する色を取得します
    /// </summary>
    private static Color colorFromPhase( double phase_degree )
    {
        double amount_of_red = (Math.Sin( phase_degree / 180.0 * Math.PI ) + 1.0) / 2.0;
        int r = (int)(255 * amount_of_red);
        int b = (int)(255 * (1.0 - amount_of_red));
        return Color.FromArgb( r, 0, b );
    }

}
