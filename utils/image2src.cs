using System;
using System.IO;

class image2src
{

    public static void Main( string[] args )
    {
        if ( args.Length <= 0 ) {
            Console.WriteLine( "error; input file was not specified" );
            return;
        }

        string file = args[0];
        if ( !File.Exists( file ) ) {
            Console.WriteLine( "error; file not found" );
            return;
        }

        string name = Path.GetFileNameWithoutExtension( file );
        Console.WriteLine( "Image " + name + "()" );
        Console.WriteLine( "{" );
        Console.WriteLine( "    try{" );
        Console.Write( "        return ImageIO.read( new ByteArrayInputStream( new byte[]{" );
        using ( FileStream fs = new FileStream( file, FileMode.Open, FileAccess.Read ) ) {
            const int BUFLEN = 1024;
            const int COLUMN = 10;
            byte[] buf = new byte[BUFLEN];
            long length = fs.Length;
            long remain = length;
            int count = COLUMN - 1;
            while ( remain > 0 ) {
                int amount = (remain > BUFLEN) ? BUFLEN : (int)remain;
                fs.Read( buf, 0, amount );
                for ( int i = 0; i < amount; i++ ) {
                    string s = "(byte)" + buf[i] + ",";
                    count++;
                    if( count != COLUMN - 1 ){
                        for ( int j = s.Length; j < "(byte)255, ".Length; j++ ) {
                            s += " ";
                        }
                    }
                    if ( count == COLUMN ) {
                        Console.WriteLine();
                        Console.Write( "            " );
                        count = 0;
                    }
                    Console.Write( s );
                }
                remain -= amount;
            }
        }
        Console.WriteLine( "} ) );" );
        Console.WriteLine( "    }catch( Exception ex ){" );
        Console.WriteLine( "        return null;" );
        Console.WriteLine( "    }" );
        Console.WriteLine( "}" );
    }

}
