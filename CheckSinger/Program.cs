using System;
using System.IO;
using System.Collections.Generic;
using cadencii;
using cadencii.vsq;
using cadencii.java.io;
using cadencii.java.util;

namespace CheckSinger {
    using boolean = System.Boolean;

    class Program {
        private static TreeMap<String, String> _map_id = new TreeMap<String, String>(); // BHXXXXXXXXXXと、仮のIDをマッピング
        // BHXX***等のキーを隠すかどうか。
        private static boolean _hidekeys = true;

        public static void Main( string[] args ) {
#if DEBUG
            _hidekeys = true;
#endif
            BufferedWriter sw = null;
            try {
                sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( "result.txt" ), "Shift_JIS" ) );
                VocaloSysUtil.init();
                string editor2 = VocaloSysUtil.getEditorPath( SynthesizerType.VOCALOID2 );
                sw.write( new string( '#', 72 ) );
                sw.newLine();
                sw.write( "VOCALOID2" );
                sw.newLine();
                SingerConfigSys sys2 = VocaloSysUtil.getSingerConfigSys( SynthesizerType.VOCALOID2 );
                write( sw, editor2, sys2 );

                sw.write( new string( '#', 72 ) );
                sw.newLine();
                sw.write( "VOCALOID1" );
                sw.newLine();
                string editor1 = VocaloSysUtil.getEditorPath( SynthesizerType.VOCALOID1 );
                SingerConfigSys sys1 = VocaloSysUtil.getSingerConfigSys( SynthesizerType.VOCALOID1 );
                write( sw, editor1, sys1 );
            } catch ( Exception ex ) {
            } finally {
                if ( sw != null ) {
                    try {
                        sw.close();
                    } catch ( Exception ex ) {
                    }
                }
            }

            if ( _hidekeys ) {
                // 一度全体を読み込み、隠す必要のあるID文字列を抽出
                BufferedReader br2 = null;
                try {
                    br2 = new BufferedReader( new InputStreamReader( new FileInputStream( "result.txt" ), "Shift_JIS" ) );
                    String line = "";
                    String[] headers = new String[]{
                        "HKLM\\SOFTWARE\\VOCALOID\\APPLICATION\\",
                        "HKLM\\SOFTWARE\\VOCALOID\\DATABASE\\EXPRESSION\\",
                        "HKLM\\SOFTWARE\\VOCALOID2\\APPLICATION\\",
                        "HKLM\\SOFTWARE\\VOCALOID2\\DATABASE\\EXPRESSION\\",
                        "HKLM\\SOFTWARE\\VOCALOID\\SKIN\\",
                        "HKLM\\SOFTWARE\\VOCALOID2\\DATABASE\\VOICE\\",
                        "HKLM\\SOFTWARE\\VOCALOID\\DATABASE\\VOICE\\", };

                    while ( (line = br2.readLine()) != null ) {
                        String[] spl = PortUtil.splitString( line, '\t' );
                        if ( spl.Length < 3 ) {
                            continue;
                        }
                        String s = spl[0];

                        // ID(?)
                        foreach( String h in headers ){
                            if( !s.StartsWith( h ) ){
                                continue;
                            }
                            String keys = "";
                            try {
                                keys = s.Substring( PortUtil.getStringLength( h ) + 17, 4 );
                            } catch ( Exception ex ) {
                                continue;
                            }
#if DEBUG
                            sout.println( "Main; keys=" + keys );
#endif
                            if( !keys.Equals( "KEYS" ) ){
                                continue;
                            }
                            String key = s.Substring( PortUtil.getStringLength( h ), 16 );
#if DEBUG
                            sout.println( "Main; key=" + key );
#endif
                            addIdMap( key );
                        }

                        // handle(?)
                        if ( spl[1] == "TIME" || spl[1] == "STANDARD" ) {
                            if( isId( spl[2] ) ){
                                addIdMap( spl[2] );
                            }
                        } else if ( spl[1] == "default" ) {
                            if ( tryParseUInt128( spl[2] ) ) {
                                addHandleMap( spl[2] );
                            }
                        } else {
                            if( isId( spl[1] ) && tryParseUInt128( spl[2] ) ){
                                addIdMap( spl[1] );
                                addHandleMap( spl[2] );
                            }
                        }
                    }
                } catch ( Exception ex ) {
                    serr.println( "Main; ex=" + ex );
                } finally {
                    if ( br2 != null ) {
                        try {
                            br2.close();
                        } catch ( Exception ex2 ) {
                            serr.println( "Main; ex2=" + ex2 );
                        }
                    }
                }
                
                String tmp = PortUtil.createTempFile();
                PortUtil.deleteFile( tmp );
                PortUtil.copyFile( "result.txt", tmp );
                PortUtil.deleteFile( "result.txt" );
                BufferedReader br = null;
                BufferedWriter bw = null;

                try {
                    br = new BufferedReader( new InputStreamReader( new FileInputStream( tmp ), "Shift_JIS" ) );
                    bw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( "result.txt" ), "Shift_JIS" ) );
                    String line = "";
                    while ( (line = br.readLine()) != null ) {
                        foreach ( String from in _map_id.Keys ) {
                            String to = _map_id[from];
                            line = line.Replace( from, to );
                        }
                        bw.write( line );
                        bw.newLine();
                    }
                } catch ( Exception ex ) {
                } finally {
                    if ( br != null ) {
                        try {
                            br.close();
                        } catch ( Exception ex2 ) {
                        }
                        PortUtil.deleteFile( tmp );
                    }
                    if ( bw != null ) {
                        try {
                            bw.close();
                        } catch ( Exception ex2 ) {
                        }
                    }
                }
            }
#if DEBUG
            Console.WriteLine( "tryParseUInt128(\"403034329006c1cf2c401d4664d3492b\")=" + tryParseUInt128( "403034329006c1cf2c401d4664d3492b" ) );
            Console.Write( "hit any key to exit..." );
            Console.Read();
#endif
        }

        static boolean isId( String s ) {
            if ( s == null ) {
                return false;
            }
            if ( s.Length != 16 ) {
                return false;
            }
            for ( int i = 0; i < 16; i++ ) {
                char c = s[i];
                if ( char.IsNumber( c ) ) {
                } else if ( char.IsLetter( c ) && char.IsUpper( c ) ) {
                } else {
                    return false;
                }
            }
            return true;
        }

        static boolean tryParseUInt64( String s ) {
            if ( s.Length < 16 ) {
                return false;
            }
            String s1 = s.Substring( 0, 16 );
            try {
                Convert.ToUInt64( s1, 16 );
            } catch ( Exception ex ) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 128bitの符号なし整数に変換できるかどうかをチェックする
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static boolean tryParseUInt128( String s ) {
            if ( s.Length < 32 ) {
                return false;
            }
            String s1 = s.Substring( 0, 16 );
            String s2 = s.Substring( 16, 16 );
            if ( tryParseUInt64( s1 ) && tryParseUInt64( s2 ) ) {
                return true;
            }
            return false;
        }

        static void addIdMap( String id ) {
            if ( !_map_id.containsKey( id ) ) {
                _map_id.put( id, "{DUMY_ID#" + _map_id.size() + "}" );
            }
        }

        static void addHandleMap( String handle ) {
            if ( !_map_id.containsKey( handle ) ) {
                _map_id.put( handle, "{DUMY_HANDLE#" + _map_id.size() + "}" );
            }
        }

        static void write( BufferedWriter sw, string editor_path, SingerConfigSys sys ) {
            if ( editor_path == "" ) {
                println( sw, "editor_path is null" );
                return;
            }
            string dir_editor = Path.GetDirectoryName( editor_path );
            string voicedbdir = Path.Combine( dir_editor, "voicedbdir" );

            // voice.mapがあるかどうか
            string path_voicemap = Path.Combine( voicedbdir, "voice.map" );
            if ( fsys.isFileExists( path_voicemap ) ) {
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
                println( sw, sc.toString() );
            }

            // インストールされた歌手のリスト
            println( sw, new string( '-', 72 ) );
            println( sw, "installed singers" );
            if ( sys != null ) {
                foreach ( SingerConfig sc in sys.getInstalledSingers() ) {
                    string vvd = sc.VvdPath;
                    println( sw, "vvd=" + Path.GetFileName( vvd ) );
                    println( sw, sc.toString() );
                }
            } else {
                println( sw, "sys is null" );
            }
        }

        static void println( BufferedWriter sw, string message ) {
            sw.write( message );
            sw.newLine();
            Console.WriteLine( message );
        }

        static void print( BufferedWriter sw, string message ) {
            sw.write( message );
            Console.Write( message );
        }
    }
}

