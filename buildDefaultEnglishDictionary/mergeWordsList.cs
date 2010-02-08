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
			for( int i = 0; i < num; i++ ){
				sw.WriteLine( dict[i] );
			}
		}
	}

	private static void mergeFrom( string file ){
		using( StreamReader sr = new StreamReader( file ) ){
			string line = "";
			while( (line = sr.ReadLine()) != null ){
				if( !dict.Contains( line ) ){
					dict.Add( line );
				}
			}
		}
	}
}
