using System;
using System.IO;
using System.Collections.Generic;

class DiffWordsList{
	public static void Main( string[] args ){
		if( args.Length != 3 ){
			Console.WriteLine( "diffWordsList [base] [diff_target] [diff_result]" );
			return;
		}
		string baseFile = args[0];
		string diffTarget = args[1];
		string diffResult = args[2];

		List<string> dict = new List<string>();
		using( StreamReader sr = new StreamReader( baseFile ) ){
			string line = "";
			while( (line = sr.ReadLine()) != null ){
				dict.Add( line );
			}
		}
		using( StreamWriter sw = new StreamWriter( diffResult ) )
		using( StreamReader sr = new StreamReader( diffTarget ) ){
			string line = "";
			while( (line = sr.ReadLine()) != null ){
				if( !dict.Contains( line ) ){
					sw.WriteLine( line );
				}
			}
		}
	}
}
