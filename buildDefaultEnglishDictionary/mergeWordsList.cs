using System;
using System.IO;
using System.Collections.Generic;

class MergeWordsList{
	static List<string> dict = new List<string>();
	
	public static void Main( string[] args ){
		string file1 = args[0];
		string file2 = args[1];
		string fileout = args[2];
		mergeFrom( file1 );
		mergeFrom( file2 );
		using( StreamWriter sw = new StreamWriter( fileout ) ){
			dict.Sort();
			int num = dict.Count;
            string last_line = "";
			for( int i = 0; i < num; i++ ){
                string s = dict[i];
                if( s == last_line ){
                    continue;
                }
				sw.WriteLine( s );
                last_line = s;
			}
		}
	}

	private static void mergeFrom( string file ){
		using( StreamReader sr = new StreamReader( file ) ){
			string line = "";
			while( (line = sr.ReadLine()) != null ){
				dict.Add( line );
			}
		}
	}
}
