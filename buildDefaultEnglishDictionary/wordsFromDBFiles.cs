using System;
using System.IO;
using System.Collections.Generic;

class WordsFromDBFiles{
	public static void Main( string[] args ){
		DirectoryInfo di = new DirectoryInfo( "dbfiles" );
		List<string> dict = new List<string>();
		foreach( FileInfo fi in di.GetFiles( "*.*" ) ){
			using( StreamReader sr = new StreamReader( fi.FullName ) ){
				string line = "";
				while( (line = sr.ReadLine()) != null ){
					line = line.Replace( " ", "" );
					if( line.StartsWith( "{[" ) ){
						int indx = line.IndexOf( ',' );
						if( indx > 0 ){
							string word = line.Substring( 2, indx - 2 );
							if( word.IndexOf( '_' ) >= 0 ){
								continue;
							}
							string lastChar = word[word.Length - 1] + "";
							int testInteger = 0;
							while( int.TryParse( lastChar, out testInteger ) ){
								//ÅŒã‚Ì•¶š‚ğ”’l‚É•ÏŠ·‚Å‚«‚é‚Ì‚ÅA‚±‚ê‚ğÈ‚­
								word = word.Substring( 0, word.Length - 1 );
								lastChar = word[word.Length - 1] + "";
							}
							if( !dict.Contains( word ) ){
								dict.Add( word );
								//Console.WriteLine( word );
							}
						}
					}
				}
			}
		}
		dict.Sort();
		using( StreamWriter sw = new StreamWriter( "parsed.txt" ) ){
			int num = dict.Count;
			for( int i = 0; i < num; i++ ){
				sw.WriteLine( dict[i] );
			}
		}
	}
}
