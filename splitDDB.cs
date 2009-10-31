using System;
using System.IO;

class splitDDB{
    static void Main( string[] args ){
        uint fcc = makefcc( "FRM2" );
        Console.WriteLine( string.Format( "{0:X}", fcc ) );
        if( args.Length <= 0 ){
            return;
        }
        string file = args[0];
        using( FileStream fs = new FileStream( file, FileMode.Open, FileAccess.Read ) ){
            byte[] header = new byte[]{ 0x46, 0x52, 0x4d, 0x32 };
            byte[] size = new byte[4];
            int BUFLEN = 512;
            byte[] buf = new byte[BUFLEN];

            int count = 0;
            while( true ){
                Console.WriteLine( "-----------------------------" );
                long pos = fs.Position;
                Console.WriteLine( "pos=" + pos );
                int len = fs.Read( header, 0, 4 );
                if( len < 4 ){
                    break;
                }
                uint headerfcc = BitConverter.ToUInt32( header, 0 );
                Console.WriteLine( "header=0x" + string.Format( "{0:X}", headerfcc ) );
                if( headerfcc == makefcc( "FRM2" ) || 
                    headerfcc == makefcc( "SND " ) ){
                    // FRM2/SND  チャンク
                    len = fs.Read( size, 0, 4 );
                    if( len < 4 ){
                        break;
                    }
                    int s = BitConverter.ToInt32( size, 0 );
                    Console.WriteLine( "size=" + s );
                    string datafile = "";
                    count++;
                    if( headerfcc == makefcc( "FRM2" ) ){
                        datafile = count + ".frm2";
                        Console.WriteLine( "frm2#" + count );
                    }else if( headerfcc == makefcc( "SND " ) ){
                        datafile = count + ".snd";
                        Console.WriteLine( "snd #" + count );
                    }

                    using( FileStream fsout = new FileStream( datafile, FileMode.Create, FileAccess.Write ) ){
                        fsout.Write( header, 0, 4 );
                        fsout.Write( size, 0, 4 );
                        int totalwrite = 8;
                        while( totalwrite < s ){
                            int remain = (s - totalwrite > BUFLEN) ? BUFLEN : s - totalwrite;
                            int len2 = fs.Read( buf, 0, remain );
                            if( len2 <= 0 ){
                                break;
                            }
                            fsout.Write( buf, 0, len2 );
                            totalwrite += len2;
                        }
                        fs.Seek( pos + s, SeekOrigin.Begin );
                    }
                }else{
                    break;
                }
            }
        }
    }
    
    static uint makefcc( string s ){
        byte[] cha = new byte[4];
        for( int i = 0; i < 4; i++ ){
            if( s.Length >= i + 1 ){
                cha[i] = (byte)s[i];
            }else{
                cha[i] = 0;
            }
        }
        return BitConverter.ToUInt32( cha, 0 );
    }
}