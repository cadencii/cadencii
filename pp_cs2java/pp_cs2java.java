/*
 * pp_cs2java
 * Copyright © 2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
import java.util.*;
import java.io.*;
import java.nio.channels.FileChannel;

class pp_cs2java {
    static String s_base_dir = "";     // 出力先
    static String s_target_dir = "";   // ファイルの検索開始位置
    static String s_target_file = "";
    static String s_target_file_out = "";
    static boolean s_recurse = false;
    static String s_encoding = "UTF-8";
    static boolean s_ignore_empty = true; // プリプロセッサを通すと中身が空になるファイルを無視する場合はtrue
    static boolean s_ignore_unknown_package = false; // package句が見つからなかったファイルを無視する場合true
    static Stack<Directives> s_current_dirctive = new Stack<Directives>(); // 現在のプリプロセッサディレクティブ．いれこになっている場合についても対応
    static int s_shift_indent = 0; // インデント解除する桁数
    static boolean s_parse_comment = false;
    static Vector<String> s_packages = new Vector<String>();
    static Vector<String> s_classes = new Vector<String>();
    static String s_main_class_path = "";
    static Vector<String> s_defines = new Vector<String>();
    static String s_logfile = ""; // ログファイルの名前
    static boolean s_logfile_overwrite = false; // ログファイルを上書きするかどうか(trueなら上書きする、falseなら末尾に追加)
    static Vector<String> s_included = new Vector<String>(); // インクルードされたファイルのリスト
    static String[][] REPLACE = new String[0][2];
    static ReplaceMode s_mode = ReplaceMode.NONE;
    static final String[][] REPLACE_JAVA = new String[][]{
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
    static String[][] REPLACE_CPP = new String[][]{
        {"public ", "public: "},
        {"private ", "private: "},
        {"vec.", "vec::"},
        {"dic.", "dic::"},
        {"sout.", "sout::"},
        {"serr.", "serr::"},
        {"conv.", "conv::"},
        {"fsys.", "fsys::"},
        {"str.", "str::"},
        {"List<", "vector<"},
        {"this.", "this->"},
    };
    //private static Regex reg_eventhandler = new Regex( @"(?<pre>.*?)(?<instance>\w*)[.]*(?<event>\w*)\s*(?<operator>[\+\-]\=)\s*new\s*(?<handler>\w*)EventHandler\s*\(\s*(?<method>.*)\s*\)" );

    enum ReplaceMode
    {
        NONE,
        JAVA,
        CPP,
    }

    static void printUsage() {
        System.out.println( "pp_cs2java" );
        System.out.println( "Copyright (C) 2011 kbinani, All Rights Reserved" );
        System.out.println( "Usage:" );
        System.out.println( "    pp_cs2java -t [search path] -b [output path] {options}" );
        System.out.println( "    pp_cs2java -i [in file] -o [out file] {options}" );
        System.out.println( "Options:" );
        System.out.println( "    -r                     enable recursive search" );
        System.out.println( "    -e                     do not ignore empty file" );
        System.out.println( "    -c                     enable comment parse" );
        System.out.println( "    -D[name]               define preprocessor directive" );
        System.out.println( "    -t [path]              set search directory path" );
        System.out.println( "    -i [path]              set target file" );
        System.out.println( "    -o [path]              name of output file" );
        System.out.println( "    -b [path]              set output directory path" );
        System.out.println( "    -s [number]            increase indent [number] column(s)" );
        System.out.println( "                           (decrease if minus)" );
        System.out.println( "    -encoding [enc. name]  set text file encoding" );
        System.out.println( "    -m [path]              set path of source code for debug" );
        System.out.println( "    -u                     ignoring unknown package" );
        System.out.println( "    -l [path]              set path of \"import\" log-file" );
        System.out.println( "    -ly                    overwrite \"import\" log-file" );
        System.out.println( "    -h,-help               print this help" );
        System.out.println( "    --replace-X            enable replace words list" );
        System.out.println( "                           X: java or cpp" );
    }

    public static void main( String[] args ) {
        //string _debug_line = "int second = s.IndexOf( '\"', first_end + 1 );";
        /*WordReplaceContext ct = new WordReplaceContext();
        string _debug_ret = replaceText( _debug_line, ct );
        System.out.println( _debug_line );
        System.out.println( _debug_ret );
        return;*/
        String current_parse = "";
        boolean print_usage = false;
        if ( args.length <= 0 ) {
            print_usage = true;
        }
        for ( int i = 0; i < args.length; i++ ) {
            if ( args[i].startsWith( "-" ) && !str.compare( current_parse, "-s" ) ) {
                current_parse = args[i];
                if ( str.compare( current_parse, "-r" ) ) {
                    s_recurse = true;
                    current_parse = "";
                } else if ( str.compare( current_parse, "-e" ) ) {
                    s_ignore_empty = false;
                    current_parse = "";
                } else if ( str.compare( current_parse, "-c" ) ) {
                    s_parse_comment = true;
                    current_parse = "";
                } else if ( str.compare( current_parse, "-u" ) ) {
                    s_ignore_unknown_package = true;
                    current_parse = "";
                } else if ( current_parse.startsWith( "-D" ) ) {
                    String def = current_parse.substring( 2 );
                    if ( !s_defines.contains( def ) ) {
                        s_defines.add( def );
                    }
                    current_parse = "";
                } else if ( str.compare( current_parse, "-h" ) || str.compare( current_parse, "-help" ) ) {
                    print_usage = true;
                } else if ( str.compare( current_parse, "-ly" ) ) {
                    s_logfile_overwrite = true;
                } else if ( current_parse.startsWith( "--replace-" ) ) {
                    String type = current_parse.substring( "--replace-".length() );
                    if ( str.compare( type, "java" ) ) {
                        REPLACE = REPLACE_JAVA;
                        s_mode = ReplaceMode.JAVA;
                    } else if ( str.compare( type, "cpp" ) ) {
                        REPLACE = REPLACE_CPP;
                        s_mode = ReplaceMode.CPP;
                    }
                }
            } else {
                if ( str.compare( current_parse, "-t" ) ) {
                    s_target_dir = args[i];
                    current_parse = "";
                } else if ( str.compare( current_parse, "-b" ) ) {
                    s_base_dir = args[i];
                    current_parse = "";
                } else if ( str.compare( current_parse, "-encoding" ) ) {
                    s_encoding = args[i];
                    current_parse = "";
                } else if ( str.compare( current_parse, "-s" ) ) {
                    s_shift_indent = Integer.parseInt( args[i] );
                    current_parse = "";
                } else if ( str.compare( current_parse, "-m" ) ) {
                    s_main_class_path = args[i];
                    current_parse = "";
                } else if ( str.compare( current_parse, "-i" ) ) {
                    s_target_file = args[i];
                    current_parse = "";
                } else if ( str.compare( current_parse, "-o" ) ) {
                    s_target_file_out = args[i];
                    current_parse = "";
                } else if ( str.compare( current_parse, "-l" ) ) {
                    s_logfile = args[i];
                    current_parse = "";
                }
            }
        }

        if ( !str.compare( s_target_dir, "" ) && !str.compare( s_target_file, "" ) ) {
            System.out.println( "error; confliction in command line arguments. -i and -t option can't co-exists" );
            return;
        }

        if ( print_usage ) {
            printUsage();
        }
        if ( args.length <= 0 ) {
            return;
        }

        if ( str.compare( s_target_file, "" ) && str.compare( s_target_dir, "" ) ) {
            System.out.println( "error; target file or path has not specified" );
            return;
        }

        if ( !str.compare( s_target_dir, "" ) ) {
            if ( str.compare( s_base_dir, "" ) ) {
                System.out.println( "error; output path has not specified" );
                return;
            }

            if ( s_recurse ) {
                preprocessRecurse( s_target_dir );
            } else {
                preprocess( s_target_dir );
            }
        }
        if ( !str.compare( s_target_file, "" ) ) {
        	try{
        		preprocessCor( s_target_file );
        	}catch( Exception ex ){
        		System.err.println( "pp_cs2java#main; ex=" + ex );
        		ex.printStackTrace();
        	}
        }

        if ( !str.compare( s_main_class_path, "" ) ) {
        	BufferedWriter sw = null;
        	try{
        		sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( s_main_class_path ) ) );
                for ( String pkg : s_packages ) {
                    sw.write( "import " + pkg + ".*;" );
                    sw.newLine();
                }
                sw.write( "class " + util.getFileNameWithoutExtension( s_main_class_path ) + "{" );
                sw.newLine();
                sw.write( "    public static void main( String[] args ){" );
                sw.newLine();
                int count = 0;
                for( String cls : s_classes ) {
                    count++;
                    sw.write( "        " + cls + " a" + count + ";" );
                    sw.newLine();
                }
                sw.write( "    }" );
                sw.newLine();
                sw.write( "}" );
                sw.newLine();
            }catch( Exception ex ){
        		System.err.println( "pp_cs2java#main; ex=" + ex );
        		ex.printStackTrace();
            }finally{
            	if( sw != null ){
            		try{
            			sw.close();
            		}catch( Exception ex2 ){
                		System.err.println( "pp_cs2java#main; ex2=" + ex2 );
                		ex2.printStackTrace();
            		}
            	}
            }
        }

        if ( !str.compare( s_logfile, "" ) ) {
            BufferedWriter sw = null;
            try {
            	boolean append = s_logfile_overwrite;
            	if( !util.isFileExists( s_logfile ) ){
            		append = false;
            	}
                sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( s_logfile, append ) ) );
                for ( String s : s_included ) {
                    sw.write( s );
                    sw.newLine();
                }
            } catch ( Exception ex ) {
        		System.err.println( "pp_cs2java#main; ex=" + ex );
        		ex.printStackTrace();
            } finally {
                if ( sw != null ) {
                    try {
                        sw.close();
                    } catch ( Exception ex2 ) {
                		System.err.println( "pp_cs2java#main; ex2=" + ex2 );
                		ex2.printStackTrace();
                    }
                }
            }
        }
    }

    static void bailOut( Exception ex ){
    	ex.printStackTrace();
    	System.exit( 1 );
    }
    
    static void preprocess( String path ){
    	for ( String fi : util.listFiles( path, ".cs" ) ) {//.listFiles()) Directory.GetFiles( path, "*.cs" ) ) {
    		try{
    			preprocessCor( fi );
    		}catch( Exception ex ){
        		System.err.println( "pp_cs2java#preprocess; ex=" + ex );
        		bailOut( ex );
    		}
        }
    }

    static void preprocessRecurse( String dir ) {
        preprocess( dir );
        File f = new File( dir );
        for ( File subdir : f.listFiles() ) {
        	if ( subdir.isDirectory() ){
        		preprocessRecurse( subdir.getAbsolutePath() );
        	}
        }
    }

    static void preprocessCor( String path )
    	throws IOException
    {
        String t = (new File( path )).getName();
        if( t.startsWith( "." ) ){
            return;
        }
        String tmp = File.createTempFile( "pp_cs2java", ".txt" ).getAbsolutePath();// Path.GetTempFileName();
        String str_package = "";
        int lines = 0;
        //Encoding enc = Encoding.Default;
        Vector<String> local_defines = new Vector<String>();
        /*if ( s_encoding == "UTF-8" ) {
            enc = new UTF8Encoding( false );
        } else {
            enc = Encoding.GetEncoding( s_encoding );
        }*/
        String enc = s_encoding;

        String tmp2 = File.createTempFile( "pp_cs2java", ".txt" ).getAbsolutePath();
        String indent = "";
        if ( -s_shift_indent > 0 ) {
        	for( int i = 0; i < -s_shift_indent; i++ ){
        		indent +=  " ";
        	}
        }
        //DateTime lastwritetime = File.GetLastWriteTime( path );
        BufferedWriter sw = null;
        BufferedReader sr = null;
        try{
        	sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( tmp2 ), s_encoding ) );
        	sr = new BufferedReader( new InputStreamReader( new BOMSkipFileInputStream( path ), s_encoding ) );
            String line = "";
            while ( (line = sr.readLine()) != null ) {
                String linetrim = line.trim();
                if ( linetrim.startsWith( "//INCLUDE " ) ) {
                    String p = linetrim.substring( 10 );
                    String include_path = util.combine( util.getDirectoryName( path ), p );
                    if ( !s_included.contains( p ) ) {
                        s_included.add( p );
                    }
                    if ( (new File( include_path )).exists() ) {
                    	BufferedReader sr_include = null;
                        try{
                        	sr_include = new BufferedReader( new InputStreamReader( new BOMSkipFileInputStream( include_path ), s_encoding ) );
                            String line2 = "";
                            while ( (line2 = sr_include.readLine()) != null ) {
                                sw.write( indent + line2 );
                                sw.newLine();
                            }
                        }catch( Exception ex ){
                    		System.err.println( "pp_cs2java#preprocessCor; ex=" + ex );
                    		bailOut( ex );
                        }finally{
                        	if ( sr_include != null ){
                        		try{
                        			sr_include.close();
                        		}catch( Exception ex2 ){
                            		System.err.println( "pp_cs2java#preprocessCor; ex2=" + ex2 );
                            		bailOut( ex2 );
                        		}
                        	}
                        }
                    }
                } else if ( linetrim.startsWith( "//INCLUDE-SECTION " ) ) {
                    String s = linetrim.substring( 18 );
                    String[] spl = s.split( " " );
                    String section_name = spl[0];
                    String p = spl[1];
                    String include_path = util.combine( util.getDirectoryName( path ), p );
                    if ( !s_included.contains( p ) ) {
                        s_included.add( p );
                    }
                    if ( (new File( include_path )).exists() ) {
                    	BufferedReader sr_include = null;
                    	try{
                    		sr_include = new BufferedReader( new InputStreamReader( new BOMSkipFileInputStream( include_path ), s_encoding ) );
                            String line2 = "";
                            boolean section_begin = false;
                            while ( (line2 = sr_include.readLine()) != null ) {
                                if ( section_begin ) {
                                    String strim = line2.trim();
                                    if ( strim.startsWith( "//SECTION-END-" ) ) {
                                        String name = strim.substring( 14 );
                                        if ( str.compare( name, section_name ) ) {
                                            section_begin = false;
                                            break;
                                        }
                                    } else {
                                        sw.write( indent + line2 );
                                        sw.newLine();
                                    }
                                } else {
                                    String strim = line2.trim();
                                    if ( strim.startsWith( "//SECTION-BEGIN-" ) ) {
                                        String name = strim.substring( 16 );
                                        if ( str.compare( name, section_name ) ) {
                                            section_begin = true;
                                        }
                                    }
                                }
                            }
                        }catch( Exception ex ){
                    		System.err.println( "pp_cs2java#preprocessCor; ex=" + ex );
                    		bailOut( ex );
                        }finally{
                        	if ( sr_include != null ){
                        		try{
                        			sr_include.close();
                        		}catch( Exception ex2 ){
                            		System.err.println( "pp_cs2java#preprocessCor; ex2=" + ex2 );
                            		bailOut( ex2 );
                        		}
                        	}
                        }
                    }
                } else {
                    sw.write( line );
                    sw.newLine();
                }
            }
        }catch( Exception ex ){
    		System.err.println( "pp_cs2java#preprocessCor; ex=" + ex );
    		bailOut( ex );
        }finally{
        	if( sw != null ){
        		try{
        			sw.close();
        		}catch( Exception ex2 ){
            		System.err.println( "pp_cs2java#preprocessCor; ex2=" + ex2 );
            		bailOut( ex2 );
        		}
        	}
        	if( sr != null ){
        		try{
        			sr.close();
        		}catch( Exception ex2 ){
            		System.err.println( "pp_cs2java#preprocessCor; ex2=" + ex2 );
            		bailOut( ex2 );
        		}
        	}
        }

        sw = null;
        sr = null;
        try{
        	sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( tmp ), s_encoding ) );
        	sr = new BufferedReader( new InputStreamReader( new BOMSkipFileInputStream( tmp2 ), s_encoding ) );
            String line = "";
            int line_num = 0;
            boolean comment_mode = false;
            String comment_indent = "";
            WordReplaceContext context = new WordReplaceContext();
            while ( (line = sr.readLine()) != null ) {
                String linetrim = line.trim();
                line_num++;
                if ( line.startsWith( "#" ) ) {
                    String trim = line.replace( " ", "" );
                    if ( trim.startsWith( "#if!" ) ) {
                        String name = trim.substring( 4 );
                        Directives ds = new Directives();
                        DirectiveUnit d = new DirectiveUnit( name, true );
                        ds.push( d );
                        s_current_dirctive.push( ds );
                    } else if ( trim.startsWith( "#if" ) ) {
                        String name = trim.substring( 3 );
                        Directives ds = new Directives();
                        DirectiveUnit d = new DirectiveUnit( name, false );
                        ds.push( d );
                        s_current_dirctive.push( ds );
                    } else if ( trim.startsWith( "#else" ) || trim.startsWith( "#elif" ) ) {
                        if ( s_current_dirctive.size() > 0 ) {
                            // 現在設定されているディレクティブを取得
                            Directives current = s_current_dirctive.pop();

                            // boolを反転する
                            // まず現在のを配列に取り出して
                            Vector<DirectiveUnit> cache = new Vector<DirectiveUnit>();
                            int c = current.size();
                            for ( int i = 0; i < c; i++ ) {
                                cache.add( current.pop() );
                            }
                            // 否定して格納
                            for ( int i = c - 1; i >= 0; i-- ) {
                                DirectiveUnit d = cache.get( i );
                                if ( i == 0 ) {
                                    d.not = !d.not;
                                }
                                current.push( d );
                            }

                            // elifの場合，さらに追加を行う
                            if ( trim.startsWith( "#elif" ) ) {
                                String name = trim.substring( 5 );
                                boolean not = false;
                                if ( trim.startsWith( "#elif!" ) ) {
                                    name = trim.substring( 6 );
                                    not = true;
                                }
                                DirectiveUnit d = new DirectiveUnit( name, not );
                                current.push( d );
                            }

                            // 格納
                            s_current_dirctive.push( current );
                        }
                    } else if ( trim.startsWith( "#endif" ) ) {
                        if ( s_current_dirctive.size() > 0 ) {
                            s_current_dirctive.pop();
                        }
                    } else if ( trim.startsWith( "#define" ) ) {
                        String direct = trim.replace( " ", "" );
                        direct = direct.substring( 7 );
                        local_defines.add( direct );
                    }
                    continue;
                }
                if ( linetrim.startsWith( "package" ) ) {
                    String trim = linetrim.substring( 7 ).replace( " ", "" );
                    int index = trim.indexOf( "//" );
                    if ( index >= 1 ) {
                        trim = trim.substring( 0, index );
                    }
                    str_package = trim.replace( ";", "" );
                    if ( !s_packages.contains( str_package ) ) {
                        s_packages.add( str_package );
                    }
                }

                boolean print_this_line = true;// s_current_dirctive.Count <= 0;

                // ディレクティブの定義状態を調べる
                // 現在のディレクティブを全て取り出す
                Vector<DirectiveUnit> defs = new Vector<DirectiveUnit>();                 // 定義されているべきディレクティブ
                Vector<DirectiveUnit> defs_not = new Vector<DirectiveUnit>();             // 定義されているとだめなディレクティブ
                for ( Directives ds : s_current_dirctive ) {
                    for ( DirectiveUnit d : ds ) {
                        (d.not ? defs_not : defs).add( d );
                    }
                }
                // defsのアイテムについて，s_definesとlocal_definesに全て入ってるかどうかチェック
                for ( DirectiveUnit d : defs ) {
                    if ( (!s_defines.contains( d.name )) && (!local_defines.contains( d.name )) ) {
                        print_this_line = false;
                        break;
                    }
                }
                // defs_notのアイテム全てについて，s_definesまたはlocal_definesに入っていないことをチェック
                if ( print_this_line ) {
                    for ( DirectiveUnit d : defs_not ) {
                        if ( s_defines.contains( d.name ) || local_defines.contains( d.name ) ) {
                            print_this_line = false;
                            break;
                        }
                    }
                }

                if ( print_this_line ) {
                    line = replaceText( line, context );
                    int index_typeof = line.indexOf( "typeof" );
                    while ( index_typeof >= 0 ) {
                        int bra = line.indexOf( "(", index_typeof );
                        int cket = line.indexOf( ")", index_typeof );
                        if ( bra < 0 || cket < 0 ) {
                            break;
                        }
                        String prefix = line.substring( 0, index_typeof );
                        String suffix = line.substring( cket + 1 );
                        String typename = line.substring( bra + 1, cket ).trim();
                        String javaclass = typename + ".class";
                        if( str.compare( typename, "int" ) ){
                            javaclass = "Integer.TYPE";
                        }else if( str.compare( typename, "float" ) ){
                            javaclass = "Float.TYPE";
                        }else if( str.compare( typename, "double" ) ){
                            javaclass = "Double.TYPE";
                        }else if( str.compare( typename, "void" ) ){
                            javaclass = "Void.TYPE";
                        }else if( str.compare( typename, "bool" ) || str.compare( typename, "boolean" ) ){
                            javaclass = "Boolean.TYPE";
                        }else if( str.compare( typename, "byte" ) ){
                            javaclass = "Byte.TYPE";
                        }
                        line = prefix + javaclass + " " + suffix;
                        index_typeof = line.indexOf( "typeof" );
                    }

                    // foreachの処理
                    int index_foreach = line.indexOf( "foreach" );
                    if ( index_foreach >= 0 ) {
                        int index_in = line.indexOf( " in " );
                        if ( index_in >= 0 ) {
                            line = line.substring( 0, index_foreach ) + "for" + line.substring( index_foreach + 7, index_in ) + " : " + line.substring( index_in + 4 );
                        }
                    }

                    // イベントハンドラの処理
                    int indx_event_handler = line.indexOf( "EventHandler" );
                    boolean success = true;
                    if( indx_event_handler < 0 ){
                    	success = false;
                    }
                    
                	// "EventHandler"の直前にある"new "を検出
                    int indx_new = -1;
                    if( success ){
                    	indx_new = line.lastIndexOf( "new ", indx_event_handler );
                    	if( indx_new < 0 ){
                    		success = false;
                    	}
                    }
                    
                    // イベントハンドラの追加なら1，削除なら-1，演算子が見つからなかったら0
                    int operator_mode = 0;
                    int indx_operator = -1;
                    if( success ){
                    	success = false;
                    	indx_operator = line.lastIndexOf( "+=", indx_new );
                    	if( indx_operator >= 0 ){
                    		operator_mode = 1;
                    		success = true;
                    	}else{
                    		indx_operator = line.lastIndexOf( "-=", indx_new );
                    		if( indx_operator >= 0 ){
                    			operator_mode = -1;
                    			success = true;
                    		}
                    	}
                    }
                    
                    // +=, -=演算子の直前にあるドットを検出
                    int indx_dot = -1;
                    if( success ){
                    	indx_dot = line.lastIndexOf( ".", indx_operator );
                    	if( indx_dot < 0 ){
                    		success = false;
                    	}
                    }
                    
                    // "EventHandler"の直後にある"("を検出
                    int indx_bla = -1;
                    if( success ){
                    	indx_bla = line.indexOf( "(", indx_event_handler );
                    	if( indx_bla < 0 ){
                    		success = false;
                    	}
                    }
                    
                    // 行の最後にある")"を検出
                    int indx_cket = -1;
                    if( success ){
                    	indx_cket = line.lastIndexOf( ")" );
                    	if( indx_cket < 0 ){
                    		success = false;
                    	}
                    }
                    
                    // 上記のインデクスるは，下記の順番で並んでないといけない
                    // indx_dot < indx_operator < indx_new < indx_event_handler < indx_bla < indx_cket
                    if( success ){
                    	success = (indx_dot < indx_operator) && (indx_operator < indx_new) && (indx_new < indx_event_handler) && (indx_event_handler < indx_bla) && (indx_bla < indx_cket);
                    }
                    
                    // すべてのマッチが成功した！
                    if( success ){
                    	String pre_instance = line.substring( 0, indx_dot );
                    	String ev = line.substring( indx_dot + 1, indx_operator );
                    	String handler = line.substring( indx_new + "new ".length(), indx_bla );
                    	String method = line.substring( indx_bla + 1, indx_cket );
                    	ev = ev.trim();
                    	ev = ev.substring( 0, 1 ).toLowerCase() + ev.substring( 1 ) + "Event";
                    	handler = handler.trim();
                    	method = method.trim();
                    	line = pre_instance + "." + ev + (operator_mode > 0 ? ".add( " : ".remove( ") + "new " + handler + "( this, \"" + method + "\" ) );";
                    }

                    if ( s_shift_indent < 0 ) {
                        String search = "";
                        for( int i = 0; i < -s_shift_indent; i++ ){
                        	search += " ";//new String( ' ', -s_shift_indent );
                        }
                        if ( line.startsWith( search ) ) {
                            line = line.substring( -s_shift_indent );
                        }
                    } else if ( s_shift_indent > 0 ) {
                    	String s = "";
                    	for( int i = 0; i < s_shift_indent; i++ ){
                    		s += " ";
                    	}
                        line = s + line;
                    }

                    // コメント処理
                    if ( s_parse_comment ) {
                        boolean draft_comment_mode;
                        int ind = line.indexOf( "///" );
                        if ( ind >= 0 ) {
                            if ( line.trim().startsWith( "///" ) ) {
                                draft_comment_mode = true;
                            } else {
                                draft_comment_mode = false;
                            }
                        } else {
                            draft_comment_mode = false;
                        }
                        if ( comment_mode ) {
                            if ( draft_comment_mode ) {
                                if ( line.indexOf( "</summary>" ) >= 0 ) {
                                    comment_indent = line.substring( 0, ind );
                                    comment_mode = draft_comment_mode;
                                    continue;
                                }
                                comment_indent = line.substring( 0, ind );
                                line = comment_indent + " *" + parseParamComment( line.substring( ind + 3 ) );
                            } else {
                                // コメントモードが終了したとき
                                sw.write( comment_indent + " */" );
                                sw.newLine();
                            }
                        } else {
                            if ( draft_comment_mode ) {
                                if ( line.indexOf( "<summary>" ) >= 0 ) {
                                    comment_indent = line.substring( 0, ind );
                                    line = comment_indent + "/**";
                                } else if ( line.indexOf( "</summary>" ) >= 0 ) {
                                    comment_indent = line.substring( 0, ind );
                                    comment_mode = draft_comment_mode;
                                    continue;
                                } else {
                                    comment_indent = line.substring( 0, ind );
                                    line = comment_indent + " * " + parseParamComment( line.substring( ind + 3 ) );
                                }
                            }
                        }
                        comment_mode = draft_comment_mode;
                    }
                    if ( !line.contains( "#region" ) && !line.contains( "#endregion" ) && !line.contains( "[Serializable]" ) ) {
                        sw.write( line );
                        sw.newLine();
                        if ( !str.compare( line, "" ) ) {
                            lines++;
                        }
                    }
                }
            }
        }catch( Exception ex ){
    		System.err.println( "pp_cs2java#preprocessCor; ex=" + ex );
    		bailOut( ex );
        }finally{
        	if( sw != null ){
        		try{
        			sw.close();
        		}catch( Exception ex2 ){
            		System.err.println( "pp_cs2java#preprocessCor; ex2=" + ex2 );
            		bailOut( ex2 );
        		}
        	}
        	if( sr != null ){
        		try{
        			sr.close();
        		}catch( Exception ex2 ){
            		System.err.println( "pp_cs2java#preprocessCor; ex2=" + ex2 );
            		bailOut( ex2 );
        		}
        	}
        }

        String out_path = "";
        if ( str.compare( s_target_file_out, "" ) ) {
            if ( str.compare( str_package, "" ) ) {
                out_path = util.combine( s_base_dir, util.getFileNameWithoutExtension( path ) + ".java" );
            } else {
                String[] spl = str_package.split( "." );
                if ( !(new File( s_base_dir )).exists() ) {
                    util.createDirectory( s_base_dir );
                }
                for ( int i = 0; i < spl.length; i++ ) {
                    String dir = s_base_dir;
                    for ( int j = 0; j <= i; j++ ) {
                        dir = util.combine( dir, spl[j] );
                    }
                    if ( !(new File( dir )).exists() ) {
                        util.createDirectory( dir );
                    }
                }
                out_path = s_base_dir;
                for ( int i = 0; i < spl.length; i++ ) {
                    out_path = util.combine( out_path, spl[i] );
                }
                out_path = util.combine( out_path, util.getFileNameWithoutExtension( path ) + ".java" );
            }
        } else {
            out_path = s_target_file_out;
        }

        if ( (new File( out_path )).exists() ) {
            (new File( out_path )).delete();
        }
        if ( !s_ignore_empty || (s_ignore_empty && lines > 0) ) {
            if ( !str.compare( str_package, "" ) || !s_ignore_unknown_package ) {
                String class_name = util.getFileNameWithoutExtension( path );
                s_classes.add( class_name );
                util.copyFile( tmp, out_path );
            }
        }
        (new File( tmp )).delete();
    }

    /// <summary>
    /// s_current_directiveの状態を1行で出力します(最後の改行無し)
    /// </summary>
    private static void printCurrentDirectives()
    {
        boolean first0 = true;
        for ( Directives d : s_current_dirctive/*.ToArray()*/ ) {
            System.out.print( (first0 ? "" : "," ) + "[" );
            first0 = false;
            boolean first = true;
            for ( DirectiveUnit du : d/*.ToArray()*/ ) {
                System.out.print( (first ? "" : ",") + (du.not ? "!" : "") + du.name );
                first = false;
            }
            System.out.print( "]" );
        }
    }

    /// <summary>
    /// &lt;param name="foo"&gt;comment&lt;param/&gt;
    /// のようなコメントを
    /// @param foo comment
    /// みたいに整形する
    /// </summary>
    private static String parseParamComment( String line ) {
    	String bla1 = "<param name=\"";
    	int indx_pre = line.indexOf( bla1 );
    	boolean mode_return = false;
    	if( indx_pre < 0 ){
    		bla1 = "<returns";
    		indx_pre = line.indexOf( bla1 );
    		if( indx_pre < 0 ){
    			return line;
    		}else{
    			mode_return = true;
    		}
    	}
    	int indx_cket1 = -1;
    	String cket1 = "";
    	if( mode_return ){
    		cket1 = ">";
    	}else{
    		cket1 = "\">";
    	}
		indx_cket1 = line.indexOf( cket1, indx_pre );
    	if( indx_cket1 < 0 ){
    		return line;
    	}
    	int indx_bla2 = line.lastIndexOf( "</" );
    	if( indx_bla2 < 0 ){
    		return line;
    	}
    	int indx_cket2 = line.indexOf( ">", indx_bla2 );
    	if( indx_cket2 < 0 ){
    		return line;
    	}
    	String pre = line.substring( 0, indx_pre );
    	String param = "";
    	if( !mode_return ){
    		param = line.substring( indx_pre + bla1.length(), indx_cket1 );
    	}
    	String message = line.substring( indx_cket1 + cket1.length(), indx_bla2 );
    	if( mode_return ){
    		return pre + "@return " + message;
    	}else{
    		return pre + "@param " + param + " " + message;
    	}
    }

    private static String replaceText_OLD( String line, WordReplaceContext context ) {
        for ( int i = 0; i < REPLACE.length/*.GetLength( 0 )*/; i++ ) {
            line = line.replace( REPLACE[i][0], REPLACE[i][1] );
        }
        return line;
    }

    private static boolean[] checkStringLiteralAndComment( String line, WordReplaceContext context ) {
        // 文字を無視するかどうかを表す
        boolean[] status = new boolean[line.length()];
        for ( int i = 0; i < status.length; i++ ) {
            status[i] = false;
        }

        // /**/によるコメントアウトを検出
        boolean line_comment_started = false; // //による行コメントが開始されているかどうか
        for ( int i = 0; i < line.length(); i++ ) {
            char c = line.charAt( i );
            if ( line_comment_started ) {
                status[i] = true;
                continue;
            }
            if ( c == '/' ) {
                if ( context.isStringLiteralStarted ) {
                    status[i] = true;
                } else {
                    if ( context.isCommentStarted ) {
                        if ( i > 0 && line.charAt( i - 1 ) == '*' ) {
                            status[i - 1] = true;
                            status[i] = true;
                            context.isCommentStarted = false;
                        }
                    } else {
                        if ( i > 0 && line.charAt( i - 1 ) == '/' ) {
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
                        if ( i > 0 && line.charAt( i - 1 ) == '/' ) {
                            status[i - 1] = true;
                            status[i] = true;
                            context.isCommentStarted = true;
                        }
                    }
                }
            } else if ( c == '"' ) {
                if ( context.isStringLiteralStarted ) {
                    if ( i > 0 && line.charAt( i - 1 ) == '\\' ) {
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
        System.out.println( line );
        System.out.println( d );*/
        return status;
    }

    private static String replaceText( String line, WordReplaceContext context ) {
        // 置換する文字列を検索
        for ( int i = 0; i < REPLACE.length/*.GetLength( 0 )*/; i++ ) {
            String search = REPLACE[i][0];
            String replace = REPLACE[i][1];
            boolean changed = true;
            int start = 0;
            int indx = line.indexOf( search, start );
            while ( changed || indx >= 0 ) {
                changed = false;
                WordReplaceContext ct = (WordReplaceContext)context.clone();
                boolean[] status = checkStringLiteralAndComment( line, ct );
                //System.out.println( "replaceText_DRAFT; search=" + search + "; indx=" + indx );
                if ( indx >= 0 ) {
                    boolean replace_ok = true;
                    for ( int j = indx; j < indx + search.length(); j++ ) {
                        if ( status[j] ) {
                            replace_ok = false;
                            break;
                        }
                    }
                    //System.out.println( "replaceText_DRAFT; replace_ok=" + replace_ok );
                    if ( replace_ok ) {
                        line = (indx > 0 ? line.substring( 0, indx ) : "") + replace + line.substring( indx + search.length() );
                        start = indx + 1;
                        changed = true;
                    } else {
                        start++;
                    }
                }
                indx = line.indexOf( search, start );
            }
        }
        checkStringLiteralAndComment( line, context );
        context.isStringLiteralStarted = false;
        return line;
    }
}

class str
{
	public static boolean compare( String a, String b )
	{
        if ( a == null || b == null ) {
            return false;
        }
        return a.equals( b );
	}
}

class util
{
	private static String mSeparator;
	
	public static String separator()
	{
		mSeparator = File.separator;
		return mSeparator;
	}
	
	public static String combine( String path1, String path2 )
	{
        separator();
        if ( path1.endsWith( mSeparator ) ) {
            path1 = path1.substring( 0, path1.length() - 1 );
        }
        if ( path2.startsWith( mSeparator ) ) {
            path2 = path2.substring( 1 );
        }
        return path1 + mSeparator + path2;
	}

	public static String getFileNameWithoutExtension( String path )
	{
        String file = getFileName( path );
        int index = file.lastIndexOf( "." );
        if( index > 0 ){
            file = file.substring( 0, index );
        }
        return file;
	}

	public static String getFileName( String path )
	{
        File f = new File( path );
        return f.getName();
	}

    public static String getDirectoryName( String path ) 
    {
        File f = new File( path );
        return f.getParent();
    }

    public static boolean isDirectoryExists( String path ) {
        File f = new File( path );
        if( f.exists() ){
            if( f.isFile() ){
                return false;
            }else{
                return true;
            }
        }else{
            return false;
        }
    }

    public static boolean isFileExists( String path ) {
        File f = new File( path );
        if( f.exists() ){
            if( f.isFile() ){
                return true;
            }else{
                return false;
            }
        }else{
            return false;
        }
    }

    public static void createDirectory( String path ) {
        File f = new File( path );
        f.mkdir();
    }


    public static String[] listFiles( String directory, String extension ) {
        File f = new File( directory );
        File[] list = f.listFiles();
        if( list == null ){
            return new String[]{};
        }
        Vector<String> ret = new Vector<String>();
        for( int i = 0; i < list.length; i++ ){
            File t = list[i];
            if( !t.isDirectory() ){
                String name = t.getName();
                if( name.endsWith( extension ) ){
                    ret.add( name );
                }
            }
        }
        return ret.toArray( new String[]{} );
    }

    public static void copyFile( String file1, String file2 )
                throws FileNotFoundException, IOException
    {
		FileChannel sourceChannel = new FileInputStream( new File( file1 ) ).getChannel();
		FileChannel destinationChannel = new FileOutputStream( new File( file2 ) ).getChannel();
		sourceChannel.transferTo( 0, sourceChannel.size(), destinationChannel );
		sourceChannel.close();
		destinationChannel.close();
    }
}

class BOMSkipFileInputStream extends InputStream
{
	private FileInputStream mStream = null;

	public BOMSkipFileInputStream( String path )
		throws IOException
	{
		mStream = new FileInputStream( path );
		// 最初の3バイトを読み込むEF BB BF
		int b0 = mStream.read();
		int b1 = mStream.read();
		int b2 = mStream.read();
		if( b0 != 0xEF || b1 != 0xBB || b2 != 0xBF ){
			mStream.close();
			mStream = new FileInputStream( path );
		}
	}
	
	@Override
	public int read()
		throws IOException
	{
		return mStream.read();
	}	
}

class WordReplaceContext {
    // "/*"によるコメントの途中かどうか
    public boolean isCommentStarted = false;
    // 文字列が開始されたかどうか
    public boolean isStringLiteralStarted = false;

    public Object clone() {
        WordReplaceContext ret = new WordReplaceContext();
        ret.isCommentStarted = this.isCommentStarted;
        ret.isStringLiteralStarted = this.isStringLiteralStarted;
        return ret;
    }
}

/// <summary>
/// ifからendifまでの間における読み込みコンテキストで，設定されているプリプロセッサ・ディレクティブを表現します
/// </summary>
class Directives extends Stack<DirectiveUnit> {
}

/// <summary>
/// 1個のプリプロセッサ・ディレクティブを表現します
/// </summary>
class DirectiveUnit {
    /// <summary>
    /// ディレクティブの名前
    /// </summary>
    public String name;
    /// <summary>
    /// 否定かどうか
    /// </summary>
    public boolean not;

    /// <summary>
    /// パラメータを指定したコンストラクタ
    /// </summary>
    /// <param name="name"></param>
    /// <param name="not"></param>
    public DirectiveUnit( String name, boolean not ) {
        this.name = name;
        this.not = not;
    }
}
