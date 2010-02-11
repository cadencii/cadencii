using System;
using System.IO;
using System.Collections.Generic;

class DiffWordsList {
    public static void Main( string[] args ) {
        if ( args.Length != 3 ) {
            Console.WriteLine( "diffWordsList [base] [diff_target] [diff_result]" );
            return;
        }
        string baseFile = args[0];
        string diffTarget = args[1];
        string diffResult = args[2];

        List<string> dict = new List<string>();
        using ( StreamReader sr = new StreamReader( baseFile ) ) {
            string line = "";
            while ( (line = sr.ReadLine()) != null ) {
                dict.Add( line );
            }
        }
        dict.Sort();
        List<string> target = new List<string>();
        using ( StreamReader sr = new StreamReader( diffTarget ) ) {
            string line = "";
            while ( (line = sr.ReadLine()) != null ) {
                target.Add( line );
            }
        }
        target.Sort();

        int start_index = 0;
        using ( StreamWriter sw = new StreamWriter( diffResult ) ) {
            int count = target.Count;
            for ( int i = 0; i < count; i++ ) {
                string s = target[i];
                int indx = dict.IndexOf( s, start_index );
                if ( indx >= 0 ) {
                    start_index = indx;
                } else {
                    sw.WriteLine( s );
                }
                //Console.Write( "\r" + i + "/" + count );
                Console.Write( "\rstart:" + start_index + "; " + i + "/" + count );
            }
        }
    }
}
