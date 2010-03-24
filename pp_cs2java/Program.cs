using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using org.kbinani;
using org.kbinani.windows.forms;
using org.kbinani.java.awt.event_;
using org.kbinani.javax.swing;

class pp_cs2java {
    static String s_base_dir = "";     // 出力先
    static String s_target_dir = "";   // ファイルの検索開始位置
    static string s_target_file = "";
    static bool s_recurse = false;
    static String s_encoding = "UTF-8";
    static bool s_ignore_empty = true; // プリプロセッサを通すと中身が空になるファイルを無視する場合はtrue
    static bool s_ignore_unknown_package = false; // package句が見つからなかったファイルを無視する場合true
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
        {" virtual ", " " },
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
        {"Math.Sin(", "Math.sin("},
        {"Math.Cos(", "Math.cos("},
        {"Math.Tan(", "Math.tan("},
        {"Math.Sqrt(", "Math.sqrt("},
        {"Math.Asin(", "Math.asin("},
        {"Math.Acos(", "Math.acos("},
        {"Math.Atan2(", "Math.atan2("},
        {"Math.Atan(", "Math.atan("},
        {".ToLower()", ".toLowerCase()"},
        {".ToUpper()", ".toUpperCase()" },
        {".IndexOf(", ".indexOf("},
        {" : ICloneable", " implements Cloneable" },
        {" : Iterator", " implements Iterator" },
        {".LastIndexOf(", ".lastIndexOf(" },
        {"base", "super"},
        {" EventArgs", " BEventArgs"},
        {" MouseEventArgs", " BMouseEventArgs"},
        {" KeyEventArgs", " BKeyEventArgs"},
        {" CancelEventArgs", " BCancelEventArgs"},
        {" DoWorkEventArgs", " BDoWorkEventArgs"},
        {" PaintEventArgs", " BPaintEventArgs"},
        {" PreviewKeyDownEventArgs", " BPreviewKeyDownEventArgs"},
        {" FormClosedEventArgs", " BFormClosedEventArgs"},
        {" FormClosingEventArgs", " BFormClosingEventArgs"},
        {" PaintEventArgs", " BPaintEventArgs"},
        {" Type ", " Class "},
    };

    static void printUsage() {
        Console.WriteLine( "pp_cs2java" );
        Console.WriteLine( "Copyright (C) kbinani, All Rights Reserved" );
        Console.WriteLine( "Usage:" );
        Console.WriteLine( "    pp_cs2java -t [search path] -b [output path] {options}" );
        Console.WriteLine( "    pp_cs2java -i [file] -b [output path] {options}" );
        Console.WriteLine( "Options:" );
        Console.WriteLine( "    -r                     enable recursive search" );
        Console.WriteLine( "    -e                     disable ignoring empty file" );
        Console.WriteLine( "    -c                     enable comment parse" );
        Console.WriteLine( "    -D[name]               define preprocessor directive" );
        Console.WriteLine( "    -t [path]              set search directory path" );
        Console.WriteLine( "    -i [path]              set target file" );
        Console.WriteLine( "    -b [path]              set output directory path" );
        Console.WriteLine( "    -s [number]            increase indent [number] column(s)" );
        Console.WriteLine( "                           (decrease if minus)" );
        Console.WriteLine( "    -encoding [enc. name]  set text file encoding" );
        Console.WriteLine( "    -m [path]              set path of source code for debug" );
        Console.WriteLine( "    -u                     enable ignoring unknown package" );
        Console.WriteLine( "    -h,-help               print this help" );
    }

    static void Main( string[] args ) {
        String current_parse = "";
        bool print_usage = false;
        if ( args.Length <= 0 ) {
            print_usage = true;
        }
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
                } else if ( current_parse == "-u" ) {
                    s_ignore_unknown_package = true;
                    current_parse = "";
                } else if ( current_parse.StartsWith( "-D" ) ) {
                    String def = current_parse.Substring( 2 );
                    if ( !s_defines.Contains( def ) ) {
                        s_defines.Add( def );
                    }
                    current_parse = "";
                } else if ( current_parse == "-h" || current_parse == "-help" ) {
                    print_usage = true;
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
                    current_parse = "";
                } else if ( current_parse == "-i" ) {
                    s_target_file = args[i];
                    current_parse = "";
                }
            }
        }

        if ( print_usage ) {
            printUsage();
        }
        if ( args.Length <= 0 ) {
            return;
        }

        if ( s_target_file == "" && s_target_dir == "" ) {
            Console.WriteLine( "error; target file or path has not specified" );
            return;
        }

        if ( s_base_dir == "" ) {
            Console.WriteLine( "error; output path has not specified" );
            return;
        }

        if ( s_target_dir != "" ) {
            if ( s_recurse ) {
                preprocessRecurse( s_target_dir );
            } else {
                preprocess( s_target_dir );
            }
        }
        if ( s_target_file != "" ) {
            preprocessCor( s_target_file );
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
                    //Console.WriteLine( "include_path=" + include_path );
                    //Console.WriteLine( "File.Exists(include_path)=" + File.Exists( include_path ) );
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
                   // Console.WriteLine( "include-section; section_name=" + section_name );
#endif
                    string p = spl[1];
                    string include_path = Path.Combine( Path.GetDirectoryName( path ), p );
#if DEBUG
                   // Console.WriteLine( "include_path=" + include_path );
                    //Console.WriteLine( "File.Exists(include_path)=" + File.Exists( include_path ) );
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
            //Console.WriteLine( "path=" + path );
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
                            print_this_line = print_this_line && (s_defines.Contains( search ) || local_defines.Contains( search ));
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
                    int index_typeof = line.IndexOf( "typeof" );
                    while( index_typeof >= 0 ){
                        int bra = line.IndexOf( "(", index_typeof );
                        int cket = line.IndexOf( ")", index_typeof );
                        string prefix = line.Substring( 0, index_typeof );
                        string suffix = line.Substring( cket + 1 );
                        string typename = line.Substring( bra + 1, cket - bra - 1 ).Trim();
                        string javaclass = typename + ".class";
                        switch( typename ){
                            case "int":
                                javaclass = "Integer.TYPE";
                                break;
                            case "float":
                                javaclass = "Float.TYPE";
                                break;
                            case "double":
                                javaclass = "Double.TYPE";
                                break;
                            case "void":
                                javaclass = "Void.TYPE";
                                break;
                            case "bool":
                            case "boolean":
                                javaclass = "Boolean.TYPE";
                                break;
                            case "byte":
                                javaclass = "Byte.TYPE";
                                break;
                        }
                        line = prefix + javaclass + " " + suffix;
                        index_typeof = line.IndexOf( "typeof" );
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

#if DEBUG
		//Console.WriteLine( "pp_cs2java#preprocessCor; package=" + package );
#endif
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
        //Console.WriteLine( "pp_cs2java#preprocessCor; out_path=" + out_path );
#endif

        if ( File.Exists( out_path ) ) {
            File.Delete( out_path );
        }
        if ( !s_ignore_empty || (s_ignore_empty && lines > 0) ) {
            if ( package != "" || !s_ignore_unknown_package ) {
                string class_name = Path.GetFileNameWithoutExtension( path );
#if DEBUG
                //Console.WriteLine( "pp_cs2java#preprocessCor; class_name=" + class_name + "; lines=" + lines );
#endif
                s_classes.Add( class_name );
                File.Copy( tmp, out_path );
            }
        }
        File.Delete( tmp );
    }

}
