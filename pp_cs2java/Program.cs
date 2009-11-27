using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using bocoree;
using bocoree.windows.forms;
using bocoree.java.awt.event_;
using bocoree.javax.swing;

class pp_cs2java {
    static String s_base_dir = "";     // 出力先
    static String s_target_dir = "";   // ファイルの検索開始位置
    static bool s_recurse = false;
    static String s_encoding = "UTF-8";
    static bool s_ignore_empty = true; // プリプロセッサを通すと中身が空になるファイルを無視する場合はtrue
    static bool s_ignore_unknown_package = true; // package句が見つからなかったファイルを無視する場合true
    static Stack<string> s_current_dirctive = new Stack<string>(); // 現在のプリプロセッサディレクティブ．いれこになっている場合についても対応
    static int s_shift_indent = 0; // インデント解除する桁数
    static bool s_parse_comment = false;
    static List<string> s_packages = new List<string>();
    static List<string> s_classes = new List<string>();
    static string s_main_class_path = "";
    static List<string> s_defines = new List<string>();
    static readonly string[,] REPLACE = new string[,]{
        {".Equals(", ".equals(" },
        {".ToString(", ".toString(" },
        {".StartsWith(", ".startsWith(" },
        {".EndsWith(", ".endsWith(" },
        {".Substring(", ".substring(" },
        {" const ", " static final " },
        {" readonly ", " final " },
        {" struct ", " class " },
        {"base.", "super." },
        {" override ", " " },
        {" is ", " instanceof "},
        {".Length", ".length" },
        {"int.MaxValue", "Integer.MAX_VALUE" },
        {"int.MinValue", "Integer.MIN_VALUE" },
        {"double.MaxValue", "Double.MAX_VALUE"},
        {"double.MinValue", "Double.MIN_VALUE"},
        {" lock", " synchronized" },
        {".Trim()", ".trim()" },
        {".Replace(", ".replace(" },
        {".ToCharArray()", ".toCharArray()" },
        {"Math.Min(", "Math.min(" },
        {"Math.Max(", "Math.max(" },
        {"Math.Log(", "Math.log(" },
        {"Math.Exp(", "Math.exp(" },
        {"Math.Ceiling(", "Math.ceil("},
        {"Math.Floor(", "Math.floor("},
        {"Math.Abs(", "Math.abs("},
        {"Math.Pow(", "Math.pow("},
        {".ToLower()", ".toLowerCase()"},
        {".ToUpper()", ".toUpperCase()" },
        {".IndexOf(", ".indexOf("},
        {" : ICloneable", " implements Cloneable" },
        {" : Iterator", " implements Iterator" },
        {".LastIndexOf(", ".lastIndexOf(" },
    };

    static void Main( string[] args ) {
        /*Random r = new Random( DateTime.Now.Millisecond );
        for ( int k = 0; k < 10000; k++ )
        {
            short v = (short)(short.MinValue + ((double)short.MaxValue - (double)short.MinValue) * r.NextDouble());
            byte[] ret = PortUtil.getbytes_int16_le( v );
            byte[] ret_g = BitConverter.GetBytes( v );
            bool erro = false;
            for ( int i = 0; i < ret.Length; i++ )
            {
                if ( ret[i] != ret_g[i] )
                {
                    Console.WriteLine( "error; ret != ret_g" );
                    Console.Write( "ret  =" );
                    for ( int j = 0; j < ret.Length; j++ )
                    {
                        Console.Write( string.Format( "0x{0:X2} ", ret[j] ) );
                    }
                    Console.Write( "ret_g=" );
                    for ( int j = 0; j < ret_g.Length; j++ )
                    {
                        Console.Write( string.Format( "0x{0:X2} ", ret_g[j] ) );
                    }
                    erro = true;
                }
            }
            if ( erro )
            {
                break;
            }
            short v_inv = PortUtil.make_int16_le( ret );
            if ( v != v_inv )
            {
                Console.WriteLine( "error; v != v_inv; v=" + v );
            }
        }*/
        /*using ( StreamWriter sw = new StreamWriter( "getBKeysFromCode.txt" ) ) {
            foreach ( string str in Enum.GetNames( typeof( BKeys ) ) ) {
                BKeys bkey = (BKeys)Enum.Parse( typeof( BKeys ), str );
                int ibkey = (int)bkey;
                foreach ( FieldInfo fin in typeof( KeyEvent ).GetFields() ) {
                    if ( fin.FieldType == typeof( int ) ) {
                        string name = fin.Name;
                        int iname = (int)fin.GetValue( typeof( KeyEvent ) );
                        if ( iname == ibkey ) {
                            sw.WriteLine( "case KeyEvent." + name + ":" );
                            sw.WriteLine( "    return BKeys." + str + ";" );
                        }
                    }
                }
            }
        }*/

        /*Dictionary<string, int> list = new Dictionary<string, int>();
        foreach ( FieldInfo fi in typeof( bocoree.awt.event_.KeyEvent ).GetFields() ) {
            if ( fi.FieldType == typeof( int ) ) {
                string key = fi.Name;
                int value = (int)fi.GetValue( typeof( bocoree.awt.event_.KeyEvent ) );
                list.Add( key, value );
            }
        }
        using ( StreamWriter sw = new StreamWriter( "keymap.txt" ) ) {
            foreach ( string str in Enum.GetNames( typeof( bocoree.windows.forms.BKeys ) ) ) {
                bocoree.windows.forms.BKeys bkey = (bocoree.windows.forms.BKeys)Enum.Parse( typeof( bocoree.windows.forms.BKeys ), str );
                int key = (int)bkey;
                if ( list.ContainsValue( key ) ) {
                    sw.Write( key + "\t" + str + "\t" );
                    foreach ( string s in list.Keys ) {
                        int kk = list[s];
                        if ( kk == key ) {
                            sw.Write( s + "\t" );
                        }
                    }
                    sw.WriteLine();
                } else {
                    sw.WriteLine( key + "\t" + str + "\t" );
                }
            }
        }*/

        String current_parse = "";
        for ( int i = 0; i < args.Length; i++ ) {
            if ( args[i].StartsWith( "-" ) && current_parse != "-s" ) {
                current_parse = args[i];
                if ( current_parse == "-r" ) {
                    s_recurse = true;
                    current_parse = "";
                } else if ( current_parse == "-e" ) {
                    s_ignore_empty = false;
                    current_parse = "";
                } else if ( current_parse == "-c" ) {
                    s_parse_comment = true;
                    current_parse = "";
                } else if ( current_parse.StartsWith( "-D" ) ) {
                    String def = current_parse.Substring( 2 );
                    if ( !s_defines.Contains( def ) ) {
                        s_defines.Add( def );
                    }
                    current_parse = "";
                }
            } else {
                if ( current_parse == "-t" ) {
                    s_target_dir = args[i];
                    current_parse = "";
                } else if ( current_parse == "-b" ) {
                    s_base_dir = args[i];
                    current_parse = "";
                } else if ( current_parse == "-encoding" ) {
                    s_encoding = args[i];
                    current_parse = "";
                } else if ( current_parse == "-s" ) {
                    s_shift_indent = int.Parse( args[i] );
                    current_parse = "";
                } else if ( current_parse == "-m" ) {
                    s_main_class_path = args[i];
                }
            }
        }

        if ( s_base_dir == "" || s_target_dir == "" ) {
            return;
        }

        if ( s_recurse ) {
            preprocessRecurse( s_target_dir );
        } else {
            preprocess( s_target_dir );
        }

        if ( s_main_class_path != "" ) {
            using ( StreamWriter sw = new StreamWriter( s_main_class_path ) ) {
                foreach ( string pkg in s_packages ) {
                    sw.WriteLine( "import " + pkg + ".*;" );
                }
                sw.WriteLine( "class " + Path.GetFileNameWithoutExtension( s_main_class_path ) + "{" );
                sw.WriteLine( "    public static void main( String[] args ){" );
                int count = 0;
                foreach ( string cls in s_classes ) {
                    count++;
                    sw.WriteLine( "        " + cls + " a" + count + ";" );
                }
                sw.WriteLine( "    }" );
                sw.WriteLine( "}" );
            }
        }
    }

    static void preprocess( String path ) {
        foreach ( String fi in Directory.GetFiles( path, "*.cs" ) ) {
            preprocessCor( fi );
        }
    }

    static void preprocessRecurse( String dir ) {
        preprocess( dir );
        foreach ( String subdir in Directory.GetDirectories( dir ) ) {
            preprocessRecurse( subdir );
        }
    }

    static void preprocessCor( String path ) {
        String tmp = Path.GetTempFileName();
        String package = "";
        int lines = 0;
        Encoding enc = Encoding.Default;
        List<string> local_defines = new List<string>();
        if ( s_encoding == "UTF-8" ) {
            enc = new UTF8Encoding( false );
        } else {
            enc = Encoding.GetEncoding( s_encoding );
        }

        string tmp2 = Path.GetTempFileName();
        string indent = "";
        if ( -s_shift_indent > 0 ) {
            indent = new string( ' ', -s_shift_indent );
        }
        using ( StreamWriter sw = new StreamWriter( tmp2, false, Encoding.GetEncoding( s_encoding ) ) )
        using ( StreamReader sr = new StreamReader( path, Encoding.GetEncoding( s_encoding ) ) ) {
            string line = "";
            while ( (line = sr.ReadLine()) != null ) {
                string linetrim = line.Trim();
                if ( linetrim.StartsWith( "//INCLUDE " ) ) {
                    string p = linetrim.Substring( 10 );
                    string include_path = Path.Combine( Path.GetDirectoryName( path ), p );
#if DEBUG
                    Console.WriteLine( "include_path=" + include_path );
                    Console.WriteLine( "File.Exists(include_path)=" + File.Exists( include_path ) );
#endif
                    if ( File.Exists( include_path ) ) {
                        using ( StreamReader sr_include = new StreamReader( include_path ) ) {
                            string line2 = "";
                            while ( (line2 = sr_include.ReadLine()) != null ) {
                                sw.WriteLine( indent + line2 );
                            }
                        }
                    }
                } else if( linetrim.StartsWith( "//INCLUDE-SECTION " ) ){
                    string s = linetrim.Substring( 18 );
                    string[] spl = s.Split( new char[] { ' ' } );
                    string section_name = spl[0];
#if DEBUG
                    Console.WriteLine( "include-section; section_name=" + section_name );
#endif
                    string p = spl[1];
                    string include_path = Path.Combine( Path.GetDirectoryName( path ), p );
#if DEBUG
                    Console.WriteLine( "include_path=" + include_path );
                    Console.WriteLine( "File.Exists(include_path)=" + File.Exists( include_path ) );
#endif
                    if ( File.Exists( include_path ) ) {
                        using ( StreamReader sr_include = new StreamReader( include_path ) ) {
                            string line2 = "";
                            bool section_begin = false;
                            while ( (line2 = sr_include.ReadLine()) != null ) {
                                if ( section_begin ) {
                                    string strim = line2.Trim();
                                    if ( strim.StartsWith( "//SECTION-END-" ) ) {
                                        string name = strim.Substring( 14 );
                                        if ( name == section_name ) {
                                            section_begin = false;
                                            break;
                                        }
                                    } else {
                                        sw.WriteLine( indent + line2 );
                                    }
                                } else {
                                    string strim = line2.Trim();
                                    if ( strim.StartsWith( "//SECTION-BEGIN-" ) ) {
                                        string name = strim.Substring( 16 );
                                        if ( name == section_name ) {
                                            section_begin = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                } else {
                    sw.WriteLine( line );
                }
            }
        }

        using ( StreamWriter sw = new StreamWriter( tmp, false, enc ) )
        using ( StreamReader sr = new StreamReader( tmp2, Encoding.GetEncoding( s_encoding ) ) ) {
#if DEBUG
            Console.WriteLine( "path=" + path );
#endif
            String line = "";
            int line_num = 0;
            bool comment_mode = false;
            String comment_indent = "";
            while ( (line = sr.ReadLine()) != null ) {
#if DEBUG
                //Console.WriteLine( "pp_cs2java#preprocessCor; line=" + line );
#endif
                string linetrim = line.Trim();
                line_num++;
                if ( line.StartsWith( "#" ) ) {
                    String trim = line.Replace( " ", "" );
                    if ( trim.StartsWith( "#if!" ) ) {
                        s_current_dirctive.Push( trim );
                    } else if ( trim.StartsWith( "#if" ) ) {
                        s_current_dirctive.Push( trim );
                    } else if ( trim.StartsWith( "#else" ) ) {
                        if ( s_current_dirctive.Count > 0 ) {
                            string current = s_current_dirctive.Pop();
#if DEBUG
                            //Console.Write( "  " + current + " -> " );
#endif
                            if ( current.StartsWith( "#if!" ) ) {
                                current = "#if" + current.Substring( 4 );
                            } else {
                                current = "#if!" + current.Substring( 3 );
                            }
#if DEBUG
                            //Console.WriteLine( current );
#endif
                            s_current_dirctive.Push( current );
                        }
                    } else if ( trim.StartsWith( "#endif" ) ) {
                        if ( s_current_dirctive.Count > 0 ) {
                            s_current_dirctive.Pop();
                        }
                    } else if ( trim.StartsWith( "#define" ) ) {
                        string direct = trim.Replace( " ", "" );
                        direct = direct.Substring( 7 );
                        local_defines.Add( direct );
                    }
                    continue;
                }
                if ( linetrim.StartsWith( "package" ) ) {
                    String trim = linetrim.Substring( 7 ).Replace( " ", "" );
                    int index = trim.IndexOf( "//" );
                    if ( index >= 1 ) {
                        trim = trim.Substring( 0, index );
                    }
                    package = trim.Replace( ";", "" );
                    if ( !s_packages.Contains( package ) ) {
                        s_packages.Add( package );
                    }
                }

                bool print_this_line = s_current_dirctive.Count <= 0;
                string dirs = "";
                bool first = true;
                foreach ( string c in s_current_dirctive ) {
                    dirs += c + " ";
                    if ( c.StartsWith( "#if!" ) ) {
                        string search = c.Substring( 4 );
                        if ( first ) {
                            print_this_line = !s_defines.Contains( search ) && !local_defines.Contains( search );
                            first = false;
                        } else {
                            print_this_line = print_this_line && (!s_defines.Contains( search ) && !local_defines.Contains( search ));
                        }
                    } else if ( c.StartsWith( "#if" ) ) {
                        string search = c.Substring( 3 );
                        if ( first ) {
                            print_this_line = s_defines.Contains( search ) || local_defines.Contains( search );
                            first = false;
                        } else {
                            print_this_line = print_this_line && s_defines.Contains( search ) || local_defines.Contains( search );
                        }
                    }
                }

#if DEBUG
                //Console.WriteLine( "dirs=" + dirs + "; line=" + line );
#endif

                if ( print_this_line ) {
                    for ( int i = 0; i < REPLACE.GetLength( 0 ); i++ ) {
                        line = line.Replace( REPLACE[i, 0], REPLACE[i, 1] );
                    }
                    int index_foreach = line.IndexOf( "foreach" );
                    if ( index_foreach >= 0 ) {
                        int index_in = line.IndexOf( " in " );
                        if ( index_in >= 0 ) {
                            line = line.Substring( 0, index_foreach ) + "for" + line.Substring( index_foreach + 7, index_in - (index_foreach + 7) ) + " : " + line.Substring( index_in + 4 );
                        }
                    }
                    if ( s_shift_indent < 0 ) {
                        string search = new string( ' ', -s_shift_indent );
                        if ( line.StartsWith( search ) ) {
                            line = line.Substring( -s_shift_indent );
                        }
                    } else if ( s_shift_indent > 0 ) {
                        line = new string( ' ', s_shift_indent ) + line;
                    }

                    // コメント処理
                    if ( s_parse_comment ) {
                        bool draft_comment_mode;
                        int ind = line.IndexOf( "///" );
                        if ( ind >= 0 ) {
                            if ( line.Trim().StartsWith( "///" ) ) {
                                draft_comment_mode = true;
                            } else {
                                draft_comment_mode = false;
                            }
                        } else {
                            draft_comment_mode = false;
                        }
                        if ( comment_mode ) {
                            if ( draft_comment_mode ) {
                                if ( line.IndexOf( "</summary>" ) >= 0 ) {
                                    comment_indent = line.Substring( 0, ind );
                                    comment_mode = draft_comment_mode;
                                    continue;
                                }
                                comment_indent = line.Substring( 0, ind );
                                line = comment_indent + " *" + line.Substring( ind + 3 );
                            } else {
                                // コメントモードが終了したとき
                                sw.WriteLine( comment_indent + " */" );
                            }
                        } else {
                            if ( draft_comment_mode ) {
                                if ( line.IndexOf( "<summary>" ) >= 0 ) {
                                    comment_indent = line.Substring( 0, ind );
                                    line = comment_indent + "/**";
                                } else if ( line.IndexOf( "</summary>" ) >= 0 ) {
                                    comment_indent = line.Substring( 0, ind );
                                    comment_mode = draft_comment_mode;
                                    continue;
                                } else {
                                    comment_indent = line.Substring( 0, ind );
                                    line = comment_indent + " * " + line.Substring( ind + 3 );
                                }
                            }
                        }
                        comment_mode = draft_comment_mode;
                    }
                    if ( !line.Contains( "#region" ) && !line.Contains( "#endregion" ) && !line.Contains( "[Serializable]" ) ) {
                        sw.WriteLine( line );
                        if ( line != "" ) {
                            lines++;
                        }
#if DEBUG
                        //Console.WriteLine( "preprocessCor; line=" + line );
#endif
                    }
                }
            }
        }

        String out_path = "";
        if ( package == "" ) {
            out_path = Path.Combine( s_base_dir, Path.GetFileNameWithoutExtension( path ) + ".java" );
        } else {
            String[] spl = package.Split( '.' );
            if ( !Directory.Exists( s_base_dir ) ) {
                Directory.CreateDirectory( s_base_dir );
            }
            for ( int i = 0; i < spl.Length; i++ ) {
                String dir = s_base_dir;
                for ( int j = 0; j <= i; j++ ) {
                    dir = Path.Combine( dir, spl[j] );
                }
                if ( !Directory.Exists( dir ) ) {
                    Directory.CreateDirectory( dir );
                }
            }
            out_path = s_base_dir;
            for ( int i = 0; i < spl.Length; i++ ) {
                out_path = Path.Combine( out_path, spl[i] );
            }
            out_path = Path.Combine( out_path, Path.GetFileNameWithoutExtension( path ) + ".java" );
        }
#if DEBUG
        Console.WriteLine( "pp_cs2java#preprocessCor; out_path=" + out_path );
#endif

        if ( File.Exists( out_path ) ) {
            File.Delete( out_path );
        }
        if ( !s_ignore_empty || (s_ignore_empty && lines > 0) ) {
            if ( package != "" || !s_ignore_unknown_package ) {
                string class_name = Path.GetFileNameWithoutExtension( path );
#if DEBUG
                Console.WriteLine( "pp_cs2java#preprocessCor; class_name=" + class_name + "; lines=" + lines );
#endif
                s_classes.Add( class_name );
                File.Copy( tmp, out_path );
            }
        }
        File.Delete( tmp );
    }
}
