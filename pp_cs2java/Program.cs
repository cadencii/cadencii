using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using org.kbinani;
using org.kbinani.java.awt.event_;
using org.kbinani.javax.swing;

class pp_cs2java {
    static String s_base_dir = "";     // 出力先
    static String s_target_dir = "";   // ファイルの検索開始位置
    static string s_target_file = "";
    static string s_target_file_out = "";
    static bool s_recurse = false;
    static String s_encoding = "UTF-8";
    static bool s_ignore_empty = true; // プリプロセッサを通すと中身が空になるファイルを無視する場合はtrue
    static bool s_ignore_unknown_package = false; // package句が見つからなかったファイルを無視する場合true
    static Stack<Directives> s_current_dirctive = new Stack<Directives>(); // 現在のプリプロセッサディレクティブ．いれこになっている場合についても対応
    static int s_shift_indent = 0; // インデント解除する桁数
    static bool s_parse_comment = false;
    static List<string> s_packages = new List<string>();
    static List<string> s_classes = new List<string>();
    static string s_main_class_path = "";
    static List<string> s_defines = new List<string>();
    static string s_logfile = ""; // ログファイルの名前
    static bool s_logfile_overwrite = false; // ログファイルを上書きするかどうか(trueなら上書きする、falseなら末尾に追加)
    static List<string> s_included = new List<string>(); // インクルードされたファイルのリスト
    static string[,] REPLACE = new string[0, 2];
    static ReplaceMode s_mode = ReplaceMode.NONE;
    static readonly string[,] REPLACE_JAVA = new string[,]{
        //{"string", "String"},
        //{" bool ", " boolean "},
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
        {" KeyPressEventArgs", " BKeyPressEventArgs"},
        {" Type ", " Class "},
        {" List<", " Vector<"},
        {".Count", ".size()"},
        {".Clear()", ".clear()"},
    };
    static string[,] REPLACE_CPP = new string[,]{
        {"public ", "public: "},
        {"private ", "private: "},
        {"vec.", "vec::"},
        {"dic.", "dic::"},
        {"sout.", "sout::"},
        {"serr.", "serr::"},
        {"conv.", "conv::"},
        {"fsys.", "fsys::"},
        {"str.", "str::"},
    };
    private static Regex reg_eventhandler = new Regex( @"(?<pre>.*?)(?<instance>\w*)[.]*(?<event>\w*)\s*(?<operator>[\+\-]\=)\s*new\s*(?<handler>\w*)EventHandler\s*\(\s*(?<method>.*)\s*\)" );

    enum ReplaceMode
    {
        NONE,
        JAVA,
        CPP,
    }

    static void printUsage() {
        Console.WriteLine( "pp_cs2java" );
        Console.WriteLine( "Copyright (C) kbinani, All Rights Reserved" );
        Console.WriteLine( "Usage:" );
        Console.WriteLine( "    pp_cs2java -t [search path] -b [output path] {options}" );
        Console.WriteLine( "    pp_cs2java -i [in file] -o [out file] {options}" );
        Console.WriteLine( "Options:" );
        Console.WriteLine( "    -r                     enable recursive search" );
        Console.WriteLine( "    -e                     do not ignore empty file" );
        Console.WriteLine( "    -c                     enable comment parse" );
        Console.WriteLine( "    -D[name]               define preprocessor directive" );
        Console.WriteLine( "    -t [path]              set search directory path" );
        Console.WriteLine( "    -i [path]              set target file" );
        Console.WriteLine( "    -o [path]              name of output file" );
        Console.WriteLine( "    -b [path]              set output directory path" );
        Console.WriteLine( "    -s [number]            increase indent [number] column(s)" );
        Console.WriteLine( "                           (decrease if minus)" );
        Console.WriteLine( "    -encoding [enc. name]  set text file encoding" );
        Console.WriteLine( "    -m [path]              set path of source code for debug" );
        Console.WriteLine( "    -u                     ignoring unknown package" );
        Console.WriteLine( "    -l [path]              set path of \"import\" log-file" );
        Console.WriteLine( "    -ly                    overwrite \"import\" log-file" );
        Console.WriteLine( "    -h,-help               print this help" );
        Console.WriteLine( "    --replace-X            enable replace words list" );
        Console.WriteLine( "                           X: java or cpp" );
    }

    static void Main( string[] args ) {
        //string _debug_line = "int second = s.IndexOf( '\"', first_end + 1 );";
        /*WordReplaceContext ct = new WordReplaceContext();
        string _debug_ret = replaceText( _debug_line, ct );
        Console.WriteLine( _debug_line );
        Console.WriteLine( _debug_ret );
        return;*/
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
                } else if ( current_parse == "-ly" ) {
                    s_logfile_overwrite = true;
                } else if ( current_parse.StartsWith( "--replace-" ) ) {
                    string type = current_parse.Substring( "--replace-".Length );
                    if ( type == "java" ) {
                        REPLACE = REPLACE_JAVA;
                        s_mode = ReplaceMode.JAVA;
                    } else if ( type == "cpp" ) {
                        REPLACE = REPLACE_CPP;
                        s_mode = ReplaceMode.CPP;
                    }
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
                } else if ( current_parse == "-o" ) {
                    s_target_file_out = args[i];
                    current_parse = "";
                } else if ( current_parse == "-l" ) {
                    s_logfile = args[i];
                    current_parse = "";
                }
            }
        }

        if ( s_target_dir != "" && s_target_file != "" ) {
            Console.WriteLine( "error; confliction in command line arguments. -i and -t option can't co-exists" );
            return;
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

        if ( s_target_dir != "" ) {
            if ( s_base_dir == "" ) {
                Console.WriteLine( "error; output path has not specified" );
                return;
            }

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

        if ( s_logfile != "" ) {
            StreamWriter sw = null;
            try {
                if ( File.Exists( s_logfile ) ) {
                    if ( s_logfile_overwrite ) {
                        sw = new StreamWriter( s_logfile );
                    } else {
                        sw = new StreamWriter( s_logfile, true );
                    }
                } else {
                    sw = new StreamWriter( s_logfile );
                }
                foreach ( string s in s_included ) {
                    sw.WriteLine( s );
                }
            } catch ( Exception ex ) {
            } finally {
                if ( sw != null ) {
                    try {
                        sw.Close();
                    } catch ( Exception ex2 ) {
                    }
                }
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
        string t = Path.GetFileName( path );
        if( t.StartsWith( "." ) ){
            return;
        }
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
        //DateTime lastwritetime = File.GetLastWriteTime( path );
        using ( StreamWriter sw = new StreamWriter( tmp2, false, Encoding.GetEncoding( s_encoding ) ) )
        using ( StreamReader sr = new StreamReader( path, Encoding.GetEncoding( s_encoding ) ) ) {
            string line = "";
            while ( (line = sr.ReadLine()) != null ) {
                string linetrim = line.Trim();
                if ( linetrim.StartsWith( "//INCLUDE " ) ) {
                    string p = linetrim.Substring( 10 );
                    string include_path = Path.Combine( Path.GetDirectoryName( path ), p );
                    if ( !s_included.Contains( p ) ) {
                        s_included.Add( p );
                    }
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
                } else if ( linetrim.StartsWith( "//INCLUDE-SECTION " ) ) {
                    string s = linetrim.Substring( 18 );
                    string[] spl = s.Split( new char[] { ' ' } );
                    string section_name = spl[0];
#if DEBUG
                    // Console.WriteLine( "include-section; section_name=" + section_name );
#endif
                    string p = spl[1];
                    string include_path = Path.Combine( Path.GetDirectoryName( path ), p );
                    if ( !s_included.Contains( p ) ) {
                        s_included.Add( p );
                    }
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
            WordReplaceContext context = new WordReplaceContext();
            while ( (line = sr.ReadLine()) != null ) {
#if DEBUG
                //Console.WriteLine( "pp_cs2java#preprocessCor; line=" + line );
#endif
                string linetrim = line.Trim();
                line_num++;
                if ( line.StartsWith( "#" ) ) {
                    String trim = line.Replace( " ", "" );
                    if ( trim.StartsWith( "#if!" ) ) {
                        string name = trim.Substring( 4 );
                        Directives ds = new Directives();
                        DirectiveUnit d = new DirectiveUnit( name, true );
                        ds.Push( d );
                        s_current_dirctive.Push( ds );
#if DEBUG
                        Console.Write( "---if!         ; " );
                        printCurrentDirectives();
                        Console.WriteLine();
#endif
                    } else if ( trim.StartsWith( "#if" ) ) {
                        string name = trim.Substring( 3 );
                        Directives ds = new Directives();
                        DirectiveUnit d = new DirectiveUnit( name, false );
                        ds.Push( d );
                        s_current_dirctive.Push( ds );
#if DEBUG
                        Console.Write( "---if          ; " );
                        printCurrentDirectives();
                        Console.WriteLine();
#endif
                    } else if ( trim.StartsWith( "#else" ) || trim.StartsWith( "#elif" ) ) {
                        if ( s_current_dirctive.Count > 0 ) {
                            // 現在設定されているディレクティブを取得
                            Directives current = s_current_dirctive.Pop();

                            // boolを反転する
                            // まず現在のを配列に取り出して
                            List<DirectiveUnit> cache = new List<DirectiveUnit>();
                            int c = current.Count;
                            for ( int i = 0; i < c; i++ ) {
                                cache.Add( current.Pop() );
                            }
                            // 否定して格納
                            for ( int i = c - 1; i >= 0; i-- ) {
                                DirectiveUnit d = cache[i];
                                if ( i == 0 ) {
                                    d.not = !d.not;
                                }
                                current.Push( d );
                            }

                            // elifの場合，さらに追加を行う
                            if ( trim.StartsWith( "#elif" ) ) {
                                string name = trim.Substring( 5 );
                                bool not = false;
                                if ( trim.StartsWith( "#elif!" ) ) {
                                    name = trim.Substring( 6 );
                                    not = true;
                                }
                                DirectiveUnit d = new DirectiveUnit( name, not );
                                current.Push( d );
                            }

                            // 格納
                            s_current_dirctive.Push( current );
                        }
#if DEBUG
                        Console.Write( "---else or elif; " );
                        printCurrentDirectives();
                        Console.WriteLine();
#endif
                    } else if ( trim.StartsWith( "#endif" ) ) {
                        if ( s_current_dirctive.Count > 0 ) {
                            s_current_dirctive.Pop();
                        }
#if DEBUG
                        Console.Write( "---endif       ; " );
                        printCurrentDirectives();
                        Console.WriteLine();
#endif
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

                bool print_this_line = true;// s_current_dirctive.Count <= 0;

                // ディレクティブの定義状態を調べる
                // 現在のディレクティブを全て取り出す
                List<DirectiveUnit> defs = new List<DirectiveUnit>();                 // 定義されているべきディレクティブ
                List<DirectiveUnit> defs_not = new List<DirectiveUnit>();             // 定義されているとだめなディレクティブ
                foreach ( Directives ds in s_current_dirctive ) {
                    foreach ( DirectiveUnit d in ds ) {
                        (d.not ? defs_not : defs).Add( d );
                    }
                }
                // defsのアイテムについて，s_definesとlocal_definesに全て入ってるかどうかチェック
                foreach ( DirectiveUnit d in defs ) {
                    if ( (!s_defines.Contains( d.name )) && (!local_defines.Contains( d.name )) ) {
                        print_this_line = false;
                        break;
                    }
                }
                // defs_notのアイテム全てについて，s_definesまたはlocal_definesに入っていないことをチェック
                if ( print_this_line ) {
                    foreach ( DirectiveUnit d in defs_not ) {
                        if ( s_defines.Contains( d.name ) || local_defines.Contains( d.name ) ) {
                            print_this_line = false;
                            break;
                        }
                    }
                }

#if DEBUG
                Console.WriteLine( line + ";" + (print_this_line ? "TRUE" : "FALSE") );
#endif

                if ( print_this_line ) {
                    line = replaceText( line, context );
                    int index_typeof = line.IndexOf( "typeof" );
                    while ( index_typeof >= 0 ) {
                        int bra = line.IndexOf( "(", index_typeof );
                        int cket = line.IndexOf( ")", index_typeof );
                        if ( bra < 0 || cket < 0 ) {
                            break;
                        }
                        string prefix = line.Substring( 0, index_typeof );
                        string suffix = line.Substring( cket + 1 );
                        string typename = line.Substring( bra + 1, cket - bra - 1 ).Trim();
                        string javaclass = typename + ".class";
                        switch ( typename ) {
                            case "int": {
                                javaclass = "Integer.TYPE";
                                break;
                            }
                            case "float": {
                                javaclass = "Float.TYPE";
                                break;
                            }
                            case "double": {
                                javaclass = "Double.TYPE";
                                break;
                            }
                            case "void": {
                                javaclass = "Void.TYPE";
                                break;
                            }
                            case "bool":
                            case "boolean": {
                                javaclass = "Boolean.TYPE";
                                break;
                            }
                            case "byte": {
                                javaclass = "Byte.TYPE";
                                break;
                            }
                        }
                        line = prefix + javaclass + " " + suffix;
                        index_typeof = line.IndexOf( "typeof" );
                    }

                    // foreachの処理
                    int index_foreach = line.IndexOf( "foreach" );
                    if ( index_foreach >= 0 ) {
                        int index_in = line.IndexOf( " in " );
                        if ( index_in >= 0 ) {
                            line = line.Substring( 0, index_foreach ) + "for" + line.Substring( index_foreach + 7, index_in - (index_foreach + 7) ) + " : " + line.Substring( index_in + 4 );
                        }
                    }

                    // イベントハンドラの処理
                    if( line.IndexOf( "EventHandler" ) > 0 ){
                        Match m = reg_eventhandler.Match( line );
                        if ( m.Success ) {
                            string pre = m.Groups["pre"].Value;
                            string instance = m.Groups["instance"].Value;
                            string ev = m.Groups["event"].Value;
                            string handler = m.Groups["handler"].Value;
                            string method = m.Groups["method"].Value;
                            string ope = m.Groups["operator"].Value;
                            if ( ope == "+=" ) {
                                ope = "add";
                            } else {
                                ope = "remove";
                            }
                            if ( ev == "" ) {
                                Console.Error.WriteLine( "error; ev is null; path=" + path + "+ line:" + line );
                            } else {
                                method = method.Trim();
                                line = pre + instance + (instance == "" ? "" : ".") + ev.Substring( 0, 1 ).ToLower() + ev.Substring( 1 ) + "Event." + ope + "( new " + handler + "EventHandler( this, \"" + method + "\" ) );";
                            }
                        }
                    }

                    // javaのときのユーティリティ関数str, vec等をナントカする
                    /* string[] sp_funcs = new string[] { "vec", "dic" };
                    bool sp_changed = true;
                    while ( sp_changed ) {
                        sp_changed = false;
                        foreach ( string func in sp_funcs ) {
                            int indx = line.IndexOf( func + "." );
                            if ( indx < 0 ) {
                                continue;
                            }
                            int indx_bla = line.IndexOf( func + ".<" );
#if DEBUG
                            PortUtil.println( "pp_cs2java#preprocessCor; 0; indx=" + indx + "; indx_bla=" + indx_bla );
#endif
                            while ( indx_bla == indx ) {
                                // 既に処理済
                                indx = line.IndexOf( func + ".", indx + 1 );
                                if ( indx < 0 ) {
                                    break;
                                }
                                indx_bla = line.IndexOf( func + ".<", indx );
                                if ( indx < 0 ) {
                                    break;
                                }
                            }
#if DEBUG
                            PortUtil.println( "pp_cs2java#preprocessCor; 1; indx=" + indx + "; indx_bla=" + indx_bla );
#endif
                            if ( indx < 0 ) {
                                // 既に処理済
                                continue;
                            }
                            // クラス名の直後に出てくる<を検出
                            indx_bla = line.IndexOf( "<", indx );
                            if ( indx_bla < 0 ) {
                                continue;
                            }
                            // その間にあるのがメソッド名
                            int indx_start = indx + (func + ".").Length;
                            int indx_end = indx_bla;
                            string method_name = line.Substring( indx_start, indx_end - indx_start );
#if DEBUG
                            PortUtil.println( "pp_cs2java#preprocessCor; line=" + line + "; method_name=" + method_name );
#endif
                            // 「>」を検出．型の指定にさらに総称型が入っていた場合を考慮
                            int indx_cket = -1;
                            int stack_count = 1;
                            for ( int i = indx_bla + 1; i < line.Length; i++ ) {
                                char c = line[i];
#if DEBUG
                                PortUtil.println( "pp_cs2java#preprocessCor; #" + i + "; c='" + new string( c, 1 ) + "'" );
#endif
                                if ( c == '>' ) {
                                    stack_count--;
                                }
                                if ( c == '<' ) {
                                    stack_count++;
                                }
                                if ( stack_count == 0 ) {
                                    indx_cket = i;
                                    break;
                                }
                            }
#if DEBUG
                            PortUtil.println( "pp_cs2java#preprocessCor; indx_bla=" + indx_bla + "; indx_cket=" + indx_cket );
#endif
                            // genericsの指定を抽出
                            if ( indx_cket <= indx_bla ) {
                                continue;
                            }
                            string generics = line.Substring( indx_bla + 1, indx_cket - indx_bla - 1 );
#if DEBUG
                            PortUtil.println( "pp_cs2java#preprocessCor; line=" + line + "; generics=" + generics );
#endif
                            // プレフィクス
                            string prefix = line.Substring( 0, indx );
                            // サフィックス
                            string suffix = line.Substring( indx_cket + 1 );
                            // 置換を実行
                            line = prefix + func + ".<" + generics + ">" + method_name + suffix;
#if DEBUG
                            PortUtil.println( "pp_cs2java#preprocessCor; after; line=" + line );
#endif
                            sp_changed = true;
                        }
                    }*/

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
                                line = comment_indent + " *" + parseParamComment( line.Substring( ind + 3 ) );
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
                                    line = comment_indent + " * " + parseParamComment( line.Substring( ind + 3 ) );
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
        if ( s_target_file_out == "" ) {
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
        } else {
            out_path = s_target_file_out;
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
                //File.SetLastWriteTime( out_path, lastwritetime );
            }
        }
        File.Delete( tmp );
    }

    /// <summary>
    /// s_current_directiveの状態を1行で出力します(最後の改行無し)
    /// </summary>
    private static void printCurrentDirectives()
    {
        bool first0 = true;
        foreach ( Directives d in s_current_dirctive.ToArray() ) {
            Console.Write( (first0 ? "" : "," ) + "[" );
            first0 = false;
            bool first = true;
            foreach ( DirectiveUnit du in d.ToArray() ) {
                Console.Write( (first ? "" : ",") + (du.not ? "!" : "") + du.name );
                first = false;
            }
            Console.Write( "]" );
        }
    }

    /// <summary>
    /// &lt;param name="foo"&gt;comment&lt;param/&gt;
    /// のようなコメントを
    /// @param foo comment
    /// みたいに整形する
    /// </summary>
    private static string parseParamComment( string line ) {
        Regex r = new Regex( @"(?<header>\s*)<param name=""(?<name>.*)"">(?<text>.*)</param>(?<footer>\s*)" );
        Match m = r.Match( line );
        string ret = line;
        if ( m.Success ) {
            string header = m.Groups["header"].Value;
            string name = m.Groups["name"].Value;
            string text = m.Groups["text"].Value;
            string footer = m.Groups["footer"].Value;
            ret = header + "@param " + name + " " + text + footer;
        }

        ValuePair<string, string>[] token = new ValuePair<string, string>[]{
            new ValuePair<string, string>( "returns", "return" ),
        };
        foreach ( ValuePair<string, string> t in token ) {
            Regex r2 = new Regex( @"(?<header>\s*)<" + t.Key + ">(?<text>.*)</" + t.Key + @">(?<footer>\s*)" );
            Match m2 = r2.Match( ret );
            if ( m2.Success ) {
                string header2 = m2.Groups["header"].Value;
                string text2 = m2.Groups["text"].Value;
                string footer2 = m2.Groups["footer"].Value;
                ret = header2 + "@" + t.Value + " " + text2 + footer2;
            }
        }

        return ret;
    }

    private static string replaceText_OLD( string line, WordReplaceContext context ) {
        for ( int i = 0; i < REPLACE.GetLength( 0 ); i++ ) {
            line = line.Replace( REPLACE[i, 0], REPLACE[i, 1] );
        }
        return line;
    }

    private static bool[] checkStringLiteralAndComment( string line, WordReplaceContext context ) {
        // 文字を無視するかどうかを表す
        bool[] status = new bool[line.Length];
        for ( int i = 0; i < status.Length; i++ ) {
            status[i] = false;
        }

        // /**/によるコメントアウトを検出
        bool line_comment_started = false; // //による行コメントが開始されているかどうか
        for ( int i = 0; i < line.Length; i++ ) {
            char c = line[i];
            if ( line_comment_started ) {
                status[i] = true;
                continue;
            }
            if ( c == '/' ) {
                if ( context.isStringLiteralStarted ) {
                    status[i] = true;
                } else {
                    if ( context.isCommentStarted ) {
                        if ( i > 0 && line[i - 1] == '*' ) {
                            status[i - 1] = true;
                            status[i] = true;
                            context.isCommentStarted = false;
                        }
                    } else {
                        if ( i > 0 && line[i - 1] == '/' ) {
                            status[i - 1] = true;
                            status[i] = true;
                            line_comment_started = true;
                        }
                    }
                }
            } else if ( c == '*' ) {
                if ( context.isStringLiteralStarted ) {
                    status[i] = true;
                } else {
                    if ( !context.isCommentStarted ) {
                        if ( i > 0 && line[i - 1] == '/' ) {
                            status[i - 1] = true;
                            status[i] = true;
                            context.isCommentStarted = true;
                        }
                    }
                }
            } else if ( c == '"' ) {
                if ( context.isStringLiteralStarted ) {
                    if ( i > 0 && line[i - 1] == '\\' ) {
                        status[i] = true;
                    } else {
                        status[i] = true;
                        context.isStringLiteralStarted = false;
                    }
                } else {
                    if ( context.isCommentStarted ) {
                        status[i] = true;
                    } else {
                        status[i] = true;
                        context.isStringLiteralStarted = true;
                    }
                }
            } else {
                if ( context.isStringLiteralStarted ||
                    context.isCommentStarted ) {
                    status[i] = true;
                }
            }
        }
        /*string d = "";
        for( int i = 0; i < status.Length; i++ ){
            d += (status[i] ? "T" : "F");
        }
        Console.WriteLine( line );
        Console.WriteLine( d );*/
        return status;
    }

    private static string replaceText( string line, WordReplaceContext context ) {
        // 置換する文字列を検索
        for ( int i = 0; i < REPLACE.GetLength( 0 ); i++ ) {
            string search = REPLACE[i, 0];
            string replace = REPLACE[i, 1];
            bool changed = true;
            int start = 0;
            int indx = line.IndexOf( search, start );
            while ( changed || indx >= 0 ) {
                changed = false;
                WordReplaceContext ct = (WordReplaceContext)context.Clone();
                bool[] status = checkStringLiteralAndComment( line, ct );
                //Console.WriteLine( "replaceText_DRAFT; search=" + search + "; indx=" + indx );
                if ( indx >= 0 ) {
                    bool replace_ok = true;
                    for ( int j = indx; j < indx + search.Length; j++ ) {
                        if ( status[j] ) {
                            replace_ok = false;
                            break;
                        }
                    }
                    //Console.WriteLine( "replaceText_DRAFT; replace_ok=" + replace_ok );
                    if ( replace_ok ) {
                        line = (indx > 0 ? line.Substring( 0, indx ) : "") + replace + line.Substring( indx + search.Length );
                        start = indx + 1;
                        changed = true;
                    } else {
                        start++;
                    }
                }
                indx = line.IndexOf( search, start );
            }
        }
        checkStringLiteralAndComment( line, context );
        context.isStringLiteralStarted = false;
        return line;
    }
}

class WordReplaceContext {
    // "/*"によるコメントの途中かどうか
    public bool isCommentStarted = false;
    // 文字列が開始されたかどうか
    public bool isStringLiteralStarted = false;

    public object Clone() {
        WordReplaceContext ret = new WordReplaceContext();
        ret.isCommentStarted = this.isCommentStarted;
        ret.isStringLiteralStarted = this.isStringLiteralStarted;
        return ret;
    }
}

/// <summary>
/// ifからendifまでの間における読み込みコンテキストで，設定されているプリプロセッサ・ディレクティブを表現します
/// </summary>
class Directives : Stack<DirectiveUnit> {
}

/// <summary>
/// 1個のプリプロセッサ・ディレクティブを表現します
/// </summary>
class DirectiveUnit {
    /// <summary>
    /// ディレクティブの名前
    /// </summary>
    public string name;
    /// <summary>
    /// 否定かどうか
    /// </summary>
    public bool not;

    /// <summary>
    /// パラメータを指定したコンストラクタ
    /// </summary>
    /// <param name="name"></param>
    /// <param name="not"></param>
    public DirectiveUnit( string name, bool not ) {
        this.name = name;
        this.not = not;
    }
}
