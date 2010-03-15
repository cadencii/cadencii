using System;
using System.IO;
using System.Collections.Generic;
using org.kbinani;
using org.kbinani.vsq;

namespace CheckSinger {

    class Program {
        static void Main( string[] args ) {
            VocaloSysUtil.init();

            using ( StreamWriter sw = new StreamWriter( "result.txt" ) ) {
                string editor2 = VocaloSysUtil.getEditorPath( SynthesizerType.VOCALOID2 );
                sw.WriteLine( new string( '#', 72 ) );
                sw.WriteLine( "VOCALOID2" );
                SingerConfigSys sys2 = VocaloSysUtil.getSingerConfigSys( SynthesizerType.VOCALOID2 );
                write( sw, editor2, sys2 );

                sw.WriteLine( new string( '#', 72 ) );
                sw.WriteLine( "VOCALOID1" );
                string editor1 = VocaloSysUtil.getEditorPath( SynthesizerType.VOCALOID1 );
                SingerConfigSys sys1 = VocaloSysUtil.getSingerConfigSys( SynthesizerType.VOCALOID1 );
                write( sw, editor1, sys1 );
            }
        }

        static void write( StreamWriter sw, string editor_path, SingerConfigSys sys ) {
            if ( editor_path == "" ) {
                println( sw, "editor_path is null" );
                return;
            }
            string dir_editor = Path.GetDirectoryName( editor_path );
            string voicedbdir = Path.Combine( dir_editor, "voicedbdir" );
            Dictionary<string, string> map = new Dictionary<string, string>(); // BHXXXXXXXXXXと、仮のIDをマッピング

            // voice.mapがあるかどうか
            string path_voicemap = Path.Combine( voicedbdir, "voice.map" );
            if ( File.Exists( path_voicemap ) ) {
                println( sw, "reading 'voice.map'..." );
                using ( FileStream fs = new FileStream( path_voicemap, FileMode.Open, FileAccess.Read ) ) {
                    byte[] dat = new byte[8];
                    fs.Seek( 0x10, SeekOrigin.Begin );
                    String header = "";
                    fs.Read( dat, 0, 8 );
                    for ( int i = 0; i < 8; i++ ) {
                        header += new string( (char)dat[i], 1 );
                    }
                    fs.Read( dat, 0, 8 );
                    for ( int i = 0; i < 8; i++ ) {
                        header += new string( (char)dat[i], 1 );
                    }
                    println( sw, "header=" + header );

                    fs.Seek( 0x20, SeekOrigin.Begin );
                    for ( int i = 0; i < SingerConfigSys.MAX_SINGERS; i++ ) {
                        fs.Read( dat, 0, 8 );
                        long value = PortUtil.make_int64_le( dat );
                        if ( value != 0 ) {
                            println( sw, "i=" + i + "; value=" + value );
                        }
                    }
                }
            }

            // vvoice*.vvdをリストアップ
            println( sw, new string( '-', 72 ) );
            println( sw, "checking 'vvoice*.vvd',,," );
            foreach ( string vvd in Directory.GetFiles( voicedbdir, "*.vvd" ) ) {
                string name = Path.GetFileName( vvd );
                println( sw, "vvd=" + name );
                SingerConfig sc = SingerConfig.fromVvd( vvd, 0, 0 );
                if ( map.ContainsKey( sc.VOICEIDSTR ) ) {
                    sc.VOICEIDSTR = map[sc.VOICEIDSTR];
                } else {
                    String newmap = "DUMY_MAP_ID#" + map.Count;
                    map.Add( sc.VOICEIDSTR, newmap );
                    sc.VOICEIDSTR = newmap;
                }
                println( sw, sc.toString() );
            }

            // インストールされた歌手のリスト
            println( sw, new string( '-', 72 ) );
            println( sw, "installed singers" );
            if ( sys != null ) {
                foreach ( SingerConfig sc in sys.getInstalledSingers() ) {
                    string vvd = sc.VvdPath;
                    println( sw, "vvd=" + Path.GetFileName( vvd ) );
                    if ( map.ContainsKey( sc.VOICEIDSTR ) ) {
                        sc.VOICEIDSTR = map[sc.VOICEIDSTR];
                    } else {
                        String newmap = "DUMY_MAP_ID#" + map.Count;
                        map.Add( sc.VOICEIDSTR, newmap );
                        sc.VOICEIDSTR = newmap;
                    }
                    println( sw, sc.toString() );
                }
            } else {
                println( sw, "sys is null" );
            }
        }

        static void println( StreamWriter sw, string message ) {
            sw.WriteLine( message );
            Console.WriteLine( message );
        }

        static void print( StreamWriter sw, string message ) {
            sw.Write( message );
            Console.Write( message );
        }
    }
}

