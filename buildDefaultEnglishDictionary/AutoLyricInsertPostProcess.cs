using System;
using System.IO;
using System.Collections.Generic;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.vsq;

class AutoLyricInsertPostProcess{
    public static void Main( string[] args ){
        int count = 0;
        List<string> list_phrase = new List<string>();
        List<string> list_symbol = new List<string>();
        while( true ){
            count++;
            string dirname = count + "";
            if( !Directory.Exists( dirname ) ){
                break;
            }
            int i = 0;
            while( true ){
                i++;
                string fname = Path.Combine( dirname, i + ".vsq" );
                if( !File.Exists( fname ) ){
                    break;
                }
                Console.Write( "\r" + fname );
                VsqFile vsq = new VsqFile( fname, "Shift_JIS" );
                VsqTrack vsq_track = vsq.Track.get( 1 );
                string cache_phrase = "";
                string cache_symbol = "";
                bool all_symbol_default = true;
                for( Iterator<VsqEvent> itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ){
                    VsqEvent item = itr.next();
                    string phrase = item.ID.LyricHandle.L0.Phrase;
                    string symbol = item.ID.LyricHandle.L0.getPhoneticSymbol();
                    if( symbol != "u:" ){
                        all_symbol_default = false;
                    }
                    if( cache_phrase == "" ){
                        cache_phrase = phrase;
                        cache_symbol = symbol;
                    }else{
                        cache_phrase += "\t" + phrase;
                        cache_symbol += "\t" + symbol;
                    }
                    if( !phrase.EndsWith( "-" ) ){
                        if( cache_phrase != "a" && cache_symbol != "u:" && !all_symbol_default ){
                            list_phrase.Add( cache_phrase );
                            list_symbol.Add( cache_symbol );
                        }
                        all_symbol_default = true;
                        cache_phrase = "";
                        cache_symbol = "";
                    }
                }
            }
        }

        bool changed = true;
        int c = list_phrase.Count;
        while( changed ){
            changed = false;
            for( int i = 0; i < c - 1; i++ ){
                string itemi = list_phrase[i];
                string itemip = list_phrase[i + 1];
                if( itemi.CompareTo( itemip ) > 0 ){
                    list_phrase[i] = itemip;
                    list_phrase[i + 1] = itemi;
                    string s = list_symbol[i];
                    list_symbol[i] = list_symbol[i + 1];
                    list_symbol[i + 1] = s;
                    changed = true;
                }
            }
        }
        string last = "";
        using( StreamWriter sw = new StreamWriter( "extracted.txt" ) ){
            for( int i = 0; i < c; i++ ){
                string s = list_phrase[i];
                if( s != last ){
                    sw.WriteLine( s + "\t\t" + list_symbol[i] );
                    last = s;
                }
            }
        }
    }
}
