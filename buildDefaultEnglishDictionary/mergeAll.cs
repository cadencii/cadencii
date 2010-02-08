using System;
using System.IO;
using System.Collections.Generic;

class MergeAll{
    public static void Main( string[] args ){
        if( args.Length != 2 ){
            Console.WriteLine( "mergeAll [directory] [result_file]" );
            return;
        }

        string dir = args[0];
        string file = args[1];

        DirectoryInfo di = new DirectoryInfo( dir );
        List<string> list = new List<string>();
        foreach( FileInfo fi in di.GetFiles( "*.*" ) ){
            if( fi.FullName.EndsWith( ".exe" ) ){
                continue;
            }
            Console.WriteLine( fi.FullName );
            using( StreamReader sr = new StreamReader( fi.FullName ) ){
                string line = "";
                while( (line = sr.ReadLine()) != null ){
                    string word = line.ToLower();
                    list.Add( word );
                }
            }
        }
        
        list.Sort();
        using( StreamWriter sw = new StreamWriter( file ) ){
            int num = list.Count;
            string last_line = "";
            for( int i = 0; i < num; i++ ){
                string s = list[i];
                if( last_line == s ){
                    continue;
                }
                sw.WriteLine( s );
                last_line = s;
            }
        }
    }
}
