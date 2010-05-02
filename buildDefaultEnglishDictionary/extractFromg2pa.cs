using System;
using System.IO;
using System.Text;
class ExtractFromG2PA {
    public static void Main( string[] args ) {
        string file = args[0];

        string tfile = Path.GetTempFileName();
        using ( FileStream sw = new FileStream( tfile, FileMode.Create, FileAccess.Write ) )
        using ( FileStream fs = new FileStream( "g2pa_ENG.dll", FileMode.Open, FileAccess.Read ) ) {
            string sb = "";
            int buflen = 1024;
            byte[] buf = new byte[buflen];
            fs.Seek( 0x12c82, SeekOrigin.Begin );
            while ( true ) {
                int len = fs.Read( buf, 0, buflen );
                if ( len <= 0 ) {
                    break;
                }
                for ( int i = 0; i < len; i++ ) {
                    if ( 0x20 <= buf[i] && buf[i] <= 0x7e ){
                    }else{
                        buf[i] = 0x0A;
                    }
                }
                sw.Write( buf, 0, len );
            }
        }
        using( StreamReader tmp = new StreamReader( tfile ) )
        using( StreamWriter sw = new StreamWriter( file ) ){
            string line = "";
            while( (line = tmp.ReadLine()) != null ){
                if( line == "" ){
                    continue;
                }
                sw.WriteLine( line );
            }
        }
    }
}
