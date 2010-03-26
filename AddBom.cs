using System;
using System.IO;
using System.Text;

class AddBom{
    private static string s_target_path = "";

    public static void Main( string[] args ){
        string current_parse = "";
        if( args.Length <= 0 ){
            Console.WriteLine( "AddBom" );
            Console.WriteLine( "Copyright (C) 2010 kbinani, All Rights Reserved" );
            Console.WriteLine( "Usage:" );
            Console.WriteLine( "    AddBom -t [search path]" );
            return;
        }

        for( int i = 0; i < args.Length; i++ ){
            if( args[i].StartsWith( "-" ) ){
                current_parse = args[i];
            }else{
                if( current_parse == "-t" ){
                    s_target_path = args[i];
                }else{
                }
                current_parse = "";
            }
        }

        if( s_target_path == "" ){
            return;
        }

        DirectoryInfo di = new DirectoryInfo( s_target_path );
        string tmp = Path.GetTempFileName();
        foreach( FileInfo fi in di.GetFiles( "*.cs" ) ){
            string file = fi.FullName;
            using( StreamReader sr = new StreamReader( file ) )
            using( StreamWriter sw = new StreamWriter( tmp, false, new UTF8Encoding( true ) ) ){
                string line = "";
                while( (line = sr.ReadLine()) != null ){
                    sw.WriteLine( line );
                }
            }
            
            File.Delete( file );
            File.Move( tmp, file );
        }
    }
}
