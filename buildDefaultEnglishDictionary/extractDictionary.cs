using System;
using System.IO;
using System.Collections.Generic;
using org.kbinani;
using org.kbinani.vsq;
using org.kbinani.java.util;

class ExtractDictionary{
	public static void Main( string[] args ){
		if( args.Length != 3 ){
			Console.WriteLine( "extractDictionary [directory(ex. PRIMA)] [extension(ex. .vsq)] [default_symbol(ex. u:)]" );
			return;
		}
		string dir = args[0];
		string ext = args[1];
		string defSymbol = args[2];
		Dictionary<string, string> dict = new Dictionary<string, string>();

		// preload existing dictionary
		string dictFile = dir + "\\dictionary.txt";
		if( !Directory.Exists( dir ) ){
			Directory.CreateDirectory( dir );
		}
		if( File.Exists( dictFile ) ){
			using( StreamReader sr = new StreamReader( dictFile ) ){
				string line = "";
				while( (line = sr.ReadLine()) != null ){
					string[] spl = line.Split( '\t' );
					dict.Add( spl[0], spl[1] );
				}
			}
		}

		DirectoryInfo di = new DirectoryInfo( dir );
		foreach( FileInfo fi in di.GetFiles( "*" + ext ) ){
			VsqFile vsq = new VsqFile( fi.FullName, "Shift_JIS" );
			for( Iterator itr = vsq.Track.get( 1 ).getNoteEventIterator(); itr.hasNext(); ){
				VsqEvent item = (VsqEvent)itr.next();
				string symbol = item.ID.LyricHandle.L0.getPhoneticSymbol();
				if( symbol != defSymbol ){
					string phrase = item.ID.LyricHandle.L0.Phrase;
					if( !dict.ContainsKey( phrase ) ){
						dict.Add( phrase, symbol );
					}
				}
			}
		}
		
		using( StreamWriter sw = new StreamWriter( dictFile ) ){
			List<string> list = new List<string>();
			foreach( string key in dict.Keys ){
				list.Add( key );
			}
			list.Sort();
			int num = list.Count;
			for( int i = 0; i < num; i++ ){
				string key = list[i];
				sw.WriteLine( key + "\t" + dict[key] );
			}
		}
	}
}
