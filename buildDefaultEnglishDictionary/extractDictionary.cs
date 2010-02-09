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
        List<Entry> dict = new List<Entry>();
		//Dictionary<string, string> dict = new Dictionary<string, string>();

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
                    string key = spl[0].ToLower();
                    dict.Add( new Entry( key, spl[1] ) );
				}
			}
		}

		DirectoryInfo di = new DirectoryInfo( dir );
		foreach( FileInfo fi in di.GetFiles( "*" + ext ) ){
            Console.WriteLine( fi.FullName );
			VsqFile vsq = new VsqFile( fi.FullName, "Shift_JIS" );
			for( Iterator itr = vsq.Track.get( 1 ).getNoteEventIterator(); itr.hasNext(); ){
				VsqEvent item = (VsqEvent)itr.next();
				string symbol = item.ID.LyricHandle.L0.getPhoneticSymbol();
        		string phrase = item.ID.LyricHandle.L0.Phrase.ToLower();
                Console.WriteLine( phrase + " => " + symbol );
				if( symbol != defSymbol ){
                    dict.Add( new Entry( phrase, symbol ) );
				}
			}
		}
		
		using( StreamWriter sw = new StreamWriter( dictFile ) ){
            dict.Sort();
            string lastKey = "";
            int num = dict.Count;
            for( int i = 0; i < num; i++ ){
                string key = dict[i].key;
                string value = dict[i].value;
                if( key == lastKey ){
                    continue;
                }
                sw.WriteLine( key + "\t" + value );
                lastKey = key;
            }
			/*List<string> list = new List<string>();
			foreach( string key in dict.Keys ){
				list.Add( key );
			}
			list.Sort();
			int num = list.Count;
			for( int i = 0; i < num; i++ ){
				string key = list[i];
				sw.WriteLine( key + "\t" + dict[key] );
			}*/
		}
	}
}

struct Entry : IComparable<Entry>{
    public string key;
    public string value;

    public Entry( string key_, string value_ ){
        key = key_;
        value = value_;
    }

    public int CompareTo( Entry item ){
        string thisKey = (key == null) ? "" : key;
        string itemKey = (item.key == null) ? "" : item.key;
        return thisKey.CompareTo( itemKey );
    }
}
