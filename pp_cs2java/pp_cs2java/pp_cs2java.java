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
package pp_cs2java;

import java.util.*;
import java.io.*;
import java.nio.channels.FileChannel;

class pp_cs2java
{
    /**
     * 出力先
     */
    static String mBaseDir = "";
    /**
     * ファイルの検索開始位置
     */
    static String mTargetDir = "";
    static String mTargetFile = "";
    static String mTargetFileOut = "";
    static boolean mRecurse = false;
    static String mEncoding = "UTF-8";
    /**
     * プリプロセッサを通すと中身が空になるファイルを無視する場合はtrue
     */
    static boolean mIgnoreEmpty = true;
    /**
     * package句が見つからなかったファイルを無視する場合true
     */
    static boolean mIgnoreUnknownPackage = false;
    /**
     * 現在のプリプロセッサディレクティブ．いれこになっている場合についても対応
     */
    static Stack<Directives> mCurrentDirctive = new Stack<Directives>();
    /**
     * インデント解除する桁数
     */
    static int mShiftIndent = 0;
    static boolean mParseComment = false;
    static Vector<String> mPackages = new Vector<String>();
    static Vector<String> mClasses = new Vector<String>();
    static String mMainClassPath = "";
    static Vector<String> mDefines = new Vector<String>();
    /**
     * ログファイルの名前
     */
    static String mLogfile = "";
    /**
     * ログファイルを上書きするかどうか(trueなら上書きする、falseなら末尾に追加)
     */
    static boolean mLogfileOverwrite = false;
    /**
     * インクルードされたファイルのリスト
     */
    static Vector<String> mIncluded = new Vector<String>();
    static String[][] REPLACE = new String[0][2];
    static ReplaceMode mMode = ReplaceMode.NONE;
    static final String[][] REPLACE_JAVA = new String[][]{
        // {"string", "String"},
        // {" bool ", " boolean "},
        { ".Equals(", ".equals(" },
        { ".ToString(", ".toString(" },
        { ".StartsWith(", ".startsWith(" },
        { ".EndsWith(", ".endsWith(" },
        { ".Substring(", ".substring(" },
        { " const ", " static final " },
        { " readonly ", " final " },
        { " struct ", " class " },
        { "base.", "super." },
        { " override ", " " },
        { " virtual ", " " },
        { " is ", " instanceof " },
        { ".Length", ".length" },
        { "int.MaxValue", "Integer.MAX_VALUE" },
        { "int.MinValue", "Integer.MIN_VALUE" },
        { "double.MaxValue", "Double.MAX_VALUE" },
        { "double.MinValue", "Double.MIN_VALUE" },
        { " lock", " synchronized" },
        { ".Trim()", ".trim()" },
        { ".Replace(", ".replace(" },
        { ".ToCharArray()", ".toCharArray()" },
        { "Math.Min(", "Math.min(" },
        { "Math.Max(", "Math.max(" },
        { "Math.Log(", "Math.log(" },
        { "Math.Exp(", "Math.exp(" },
        { "Math.Ceiling(", "Math.ceil(" },
        { "Math.Floor(", "Math.floor(" },
        { "Math.Abs(", "Math.abs(" },
        { "Math.Pow(", "Math.pow(" },
        { "Math.Sin(", "Math.sin(" },
        { "Math.Cos(", "Math.cos(" },
        { "Math.Tan(", "Math.tan(" },
        { "Math.Sqrt(", "Math.sqrt(" },
        { "Math.Asin(", "Math.asin(" },
        { "Math.Acos(", "Math.acos(" },
        { "Math.Atan2(", "Math.atan2(" },
        { "Math.Atan(", "Math.atan(" },
        { ".ToLower()", ".toLowerCase()" },
        { ".ToUpper()", ".toUpperCase()" },
        { ".IndexOf(", ".indexOf(" },
        { " : ICloneable", " implements Cloneable" },
        { " : Iterator", " implements Iterator" },
        { ".LastIndexOf(", ".lastIndexOf(" },
        { "base", "super" },
        { " EventArgs", " BEventArgs" },
        { " MouseEventArgs", " BMouseEventArgs" },
        { " KeyEventArgs", " BKeyEventArgs" },
        { " CancelEventArgs", " BCancelEventArgs" },
        { " DoWorkEventArgs", " BDoWorkEventArgs" },
        { " PaintEventArgs", " BPaintEventArgs" },
        { " PreviewKeyDownEventArgs", " BPreviewKeyDownEventArgs" },
        { " FormClosedEventArgs", " BFormClosedEventArgs" },
        { " FormClosingEventArgs", " BFormClosingEventArgs" },
        { " PaintEventArgs", " BPaintEventArgs" },
        { " KeyPressEventArgs", " BKeyPressEventArgs" },
        { " Type ", " Class " },
        { " List<", " Vector<" },
        { ".Count", ".size()" },
        { ".Clear()", ".clear()" }, };
    static String[][] REPLACE_CPP = new String[][]{
        { "public ", "public: " },
        { "private ", "private: " },
        { "vec.", "vec::" },
        { "dic.", "dic::" },
        { "sout.", "sout::" },
        { "serr.", "serr::" },
        { "conv.", "conv::" },
        { "fsys.", "fsys::" },
        { "str.", "str::" },
        { "List<", "vector<" },
        { "this.", "this->" }, };

    // private static Regex reg_eventhandler = new Regex(
    // @"(?<pre>.*?)(?<instance>\w*)[.]*(?<event>\w*)\s*(?<operator>[\+\-]\=)\s*new\s*(?<handler>\w*)EventHandler\s*\(\s*(?<method>.*)\s*\)"
    // );

    static void printUsage()
    {
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

    public static void main(
        String[] args )
    {
        // 引数を解析
        boolean print_usage = parseArguments( args );

        // -i オプションと -t オプションは共存できないので死ぬ
        if( !str.compare( mTargetDir, "" ) && !str.compare( mTargetFile, "" ) )
        {
            System.out.println( "error; confliction in command line arguments. -i and -t option can't co-exists" );
            return;
        }

        if( print_usage )
        {
            printUsage();
        }
        if( args.length <= 0 )
        {
            return;
        }

        // 読み込みファイルと，出力先のディレクトリがどちらも設定されなかった場合は何も出来ない
        if( str.compare( mTargetFile, "" ) && str.compare( mTargetDir, "" ) )
        {
            System.out.println( "error; target file or path has not specified" );
            return;
        }

        if( !str.compare( mTargetDir, "" ) )
        {
            if( str.compare( mBaseDir, "" ) )
            {
                System.out.println( "error; output path has not specified" );
                return;
            }

            if( mRecurse )
            {
                preprocessRecurse( mTargetDir );
            }
            else
            {
                preprocess( mTargetDir );
            }
        }
        if( !str.compare( mTargetFile, "" ) )
        {
            try
            {
                preprocessCor( mTargetFile );
            }
            catch( Exception ex )
            {
                System.err.println( "pp_cs2java#main; ex=" + ex );
                ex.printStackTrace();
            }
        }

        if( !str.compare( mMainClassPath, "" ) )
        {
            BufferedWriter sw = null;
            try
            {
                sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( mMainClassPath ) ) );
                for( String pkg : mPackages )
                {
                    sw.write( "import " + pkg + ".*;" );
                    sw.newLine();
                }
                sw.write( "class " + util.getFileNameWithoutExtension( mMainClassPath ) + "{" );
                sw.newLine();
                sw.write( "    public static void main( String[] args ){" );
                sw.newLine();
                int count = 0;
                for( String cls : mClasses )
                {
                    count++;
                    sw.write( "        " + cls + " a" + count + ";" );
                    sw.newLine();
                }
                sw.write( "    }" );
                sw.newLine();
                sw.write( "}" );
                sw.newLine();
            }
            catch( Exception ex )
            {
                System.err.println( "pp_cs2java#main; ex=" + ex );
                ex.printStackTrace();
            }
            finally
            {
                if( sw != null )
                {
                    try
                    {
                        sw.close();
                    }
                    catch( Exception ex2 )
                    {
                        System.err.println( "pp_cs2java#main; ex2=" + ex2 );
                        ex2.printStackTrace();
                    }
                }
            }
        }

        if( !str.compare( mLogfile, "" ) )
        {
            BufferedWriter sw = null;
            try
            {
                boolean append = mLogfileOverwrite;
                if( !util.isFileExists( mLogfile ) )
                {
                    append = false;
                }
                sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( mLogfile, append ) ) );
                for( String s : mIncluded )
                {
                    sw.write( s );
                    sw.newLine();
                }
            }
            catch( Exception ex )
            {
                System.err.println( "pp_cs2java#main; ex=" + ex );
                ex.printStackTrace();
            }
            finally
            {
                if( sw != null )
                {
                    try
                    {
                        sw.close();
                    }
                    catch( Exception ex2 )
                    {
                        System.err.println( "pp_cs2java#main; ex2=" + ex2 );
                        ex2.printStackTrace();
                    }
                }
            }
        }
    }

    /**
     * 実行時の引数を解析します．
     * 
     * @param args
     *            解析する引数．
     * @return 使い方の説明文を表示するオプションがONだった場合にtrue，それ以外はfalseを返します．
     */
    private static boolean parseArguments(
        String[] args )
    {
        String current_parse = "";
        boolean print_usage = false;
        if( args.length <= 0 )
        {
            print_usage = true;
        }
        for( int i = 0; i < args.length; i++ )
        {
            if( args[i].startsWith( "-" ) && !str.compare( current_parse, "-s" ) )
            {
                current_parse = args[i];
                if( str.compare( current_parse, "-r" ) )
                {
                    mRecurse = true;
                    current_parse = "";
                }
                else if( str.compare( current_parse, "-e" ) )
                {
                    mIgnoreEmpty = false;
                    current_parse = "";
                }
                else if( str.compare( current_parse, "-c" ) )
                {
                    mParseComment = true;
                    current_parse = "";
                }
                else if( str.compare( current_parse, "-u" ) )
                {
                    mIgnoreUnknownPackage = true;
                    current_parse = "";
                }
                else if( current_parse.startsWith( "-D" ) )
                {
                    String def = current_parse.substring( 2 );
                    if( !mDefines.contains( def ) )
                    {
                        mDefines.add( def );
                    }
                    current_parse = "";
                }
                else if( str.compare( current_parse, "-h" ) || str.compare( current_parse, "-help" ) )
                {
                    print_usage = true;
                }
                else if( str.compare( current_parse, "-ly" ) )
                {
                    mLogfileOverwrite = true;
                }
                else if( current_parse.startsWith( "--replace-" ) )
                {
                    String type = current_parse.substring( "--replace-".length() );
                    if( str.compare( type, "java" ) )
                    {
                        REPLACE = REPLACE_JAVA;
                        mMode = ReplaceMode.JAVA;
                    }
                    else if( str.compare( type, "cpp" ) )
                    {
                        REPLACE = REPLACE_CPP;
                        mMode = ReplaceMode.CPP;
                    }
                }
            }
            else
            {
                if( str.compare( current_parse, "-t" ) )
                {
                    mTargetDir = args[i];
                    current_parse = "";
                }
                else if( str.compare( current_parse, "-b" ) )
                {
                    mBaseDir = args[i];
                    current_parse = "";
                }
                else if( str.compare( current_parse, "-encoding" ) )
                {
                    mEncoding = args[i];
                    current_parse = "";
                }
                else if( str.compare( current_parse, "-s" ) )
                {
                    mShiftIndent = Integer.parseInt( args[i] );
                    current_parse = "";
                }
                else if( str.compare( current_parse, "-m" ) )
                {
                    mMainClassPath = args[i];
                    current_parse = "";
                }
                else if( str.compare( current_parse, "-i" ) )
                {
                    mTargetFile = args[i];
                    current_parse = "";
                }
                else if( str.compare( current_parse, "-o" ) )
                {
                    mTargetFileOut = args[i];
                    current_parse = "";
                }
                else if( str.compare( current_parse, "-l" ) )
                {
                    mLogfile = args[i];
                    current_parse = "";
                }
            }
        }
        return print_usage;
    }

    static void bailOut(
        Exception ex )
    {
        ex.printStackTrace();
        System.exit( 1 );
    }

    static void preprocess(
        String path )
    {
        for( String fi : util.listFiles( path, ".cs" ) )
        {
            try
            {
                preprocessCor( fi );
            }
            catch( Exception ex )
            {
                System.err.println( "pp_cs2java#preprocess; ex=" + ex );
                bailOut( ex );
            }
        }
    }

    static void preprocessRecurse(
        String dir )
    {
        preprocess( dir );
        File f = new File( dir );
        for( File subdir : f.listFiles() )
        {
            if( subdir.isDirectory() )
            {
                preprocessRecurse( subdir.getAbsolutePath() );
            }
        }
    }

    /**
     * 1個のファイルに対してプリプロセス処理を行います．
     * 
     * @param String
     *            path 処理するファイルのパス．
     */
    static void preprocessCor_new(
        String path ) throws IOException
    {
        // ファイル名が.で始まっていたら何もしない
        String t = (new File( path )).getName();
        if( t.startsWith( "." ) )
        {
            return;
        }

        //
        String tmp = File.createTempFile( "pp_cs2java", ".txt" ).getAbsolutePath();// Path.GetTempFileName();
        String str_package = "";
        int lines = 0;
        // Encoding enc = Encoding.Default;
        Vector<String> local_defines = new Vector<String>();
        /*
         * if ( s_encoding == "UTF-8" ) { enc = new UTF8Encoding( false ); }
         * else { enc = Encoding.GetEncoding( s_encoding ); }
         */
        String enc = mEncoding;

        String tmp2 = File.createTempFile( "pp_cs2java", ".txt" ).getAbsolutePath();
        String indent = "";
        if( -mShiftIndent > 0 )
        {
            for( int i = 0; i < -mShiftIndent; i++ )
            {
                indent += " ";
            }
        }

        // INCLUDEの処理
        doInclude( path, tmp2, indent );

        // ファイルをロード
        BufferedReader reader =
        	new BufferedReader(
        		new InputStreamReader( new BOMSkipFileInputStream( tmp2 ), mEncoding ) );
        SourceText src = new SourceText( reader );
        
        //TODO: このへんから書き始めよう
    }

    /**
     * "//INCLUDE"の処理を行います．
     * 
     * @param in_path
     *            入力ファイルのパス．
     * @param out_path
     *            出力ファイルのパス．
     * @param indent
     *            インデント用の文字列．
     */
    static void doInclude(
        String in_path,
        String out_path,
        String indent )
    {
        BufferedWriter sw = null;
        BufferedReader sr = null;
        try
        {
            sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( out_path ), mEncoding ) );
            sr = new BufferedReader( new InputStreamReader( new BOMSkipFileInputStream( in_path ), mEncoding ) );
            String line = "";
            while( (line = sr.readLine()) != null )
            {
                String linetrim = line.trim();
                if( linetrim.startsWith( "//INCLUDE " ) )
                {
                    doIncludeAll( in_path, indent, sw, linetrim );
                }
                else if( linetrim.startsWith( "//INCLUDE-SECTION " ) )
                {
                    doIncludeSection( in_path, indent, sw, linetrim );
                }
                else
                {
                    sw.write( line );
                    sw.newLine();
                }
            }
        }
        catch( Exception ex )
        {
            System.err.println( "pp_cs2java#preprocessCor; ex=" + ex );
            bailOut( ex );
        }
        finally
        {
            if( sw != null )
            {
                try
                {
                    sw.close();
                }
                catch( Exception ex2 )
                {
                    System.err.println( "pp_cs2java#preprocessCor; ex2=" + ex2 );
                    bailOut( ex2 );
                }
            }
            if( sr != null )
            {
                try
                {
                    sr.close();
                }
                catch( Exception ex2 )
                {
                    System.err.println( "pp_cs2java#preprocessCor; ex2=" + ex2 );
                    bailOut( ex2 );
                }
            }
        }
    }

    /**
     * セクション指定のINCLUDE処理（INCLUDE-SECTION）を行います．
     * 
     * @param path
     * @param indent
     * @param sw
     * @param linetrim
     */
    public static void doIncludeSection(
        String path,
        String indent,
        BufferedWriter sw,
        String linetrim )
    {
        String s = linetrim.substring( 18 );
        String[] spl = s.split( " " );
        String section_name = spl[0];
        String p = spl[1];
        String include_path = util.combine( util.getDirectoryName( path ), p );
        if( !mIncluded.contains( p ) )
        {
            mIncluded.add( p );
        }
        if( (new File( include_path )).exists() )
        {
            BufferedReader sr_include = null;
            try
            {
                sr_include = new BufferedReader( new InputStreamReader( new BOMSkipFileInputStream( include_path ), mEncoding ) );
                String line2 = "";
                boolean section_begin = false;
                while( (line2 = sr_include.readLine()) != null )
                {
                    if( section_begin )
                    {
                        String strim = line2.trim();
                        if( strim.startsWith( "//SECTION-END-" ) )
                        {
                            String name = strim.substring( 14 );
                            if( str.compare( name, section_name ) )
                            {
                                section_begin = false;
                                break;
                            }
                        }
                        else
                        {
                            sw.write( indent + line2 );
                            sw.newLine();
                        }
                    }
                    else
                    {
                        String strim = line2.trim();
                        if( strim.startsWith( "//SECTION-BEGIN-" ) )
                        {
                            String name = strim.substring( 16 );
                            if( str.compare( name, section_name ) )
                            {
                                section_begin = true;
                            }
                        }
                    }
                }
            }
            catch( Exception ex )
            {
                System.err.println( "pp_cs2java#preprocessCor; ex=" + ex );
                bailOut( ex );
            }
            finally
            {
                if( sr_include != null )
                {
                    try
                    {
                        sr_include.close();
                    }
                    catch( Exception ex2 )
                    {
                        System.err.println( "pp_cs2java#preprocessCor; ex2=" + ex2 );
                        bailOut( ex2 );
                    }
                }
            }
        }
    }

    /**
     * ファイル全部をINCLUDEする処理を行います．
     * 
     * @param path
     * @param indent
     * @param sw
     * @param linetrim
     */
    public static void doIncludeAll(
        String path,
        String indent,
        BufferedWriter sw,
        String linetrim )
    {
        String p = linetrim.substring( 10 );
        String include_path = util.combine( util.getDirectoryName( path ), p );
        if( !mIncluded.contains( p ) )
        {
            mIncluded.add( p );
        }
        if( (new File( include_path )).exists() )
        {
            BufferedReader sr_include = null;
            try
            {
                sr_include = new BufferedReader( new InputStreamReader( new BOMSkipFileInputStream( include_path ), mEncoding ) );
                String line2 = "";
                while( (line2 = sr_include.readLine()) != null )
                {
                    sw.write( indent + line2 );
                    sw.newLine();
                }
            }
            catch( Exception ex )
            {
                System.err.println( "pp_cs2java#preprocessCor; ex=" + ex );
                bailOut( ex );
            }
            finally
            {
                if( sr_include != null )
                {
                    try
                    {
                        sr_include.close();
                    }
                    catch( Exception ex2 )
                    {
                        System.err.println( "pp_cs2java#preprocessCor; ex2=" + ex2 );
                        bailOut( ex2 );
                    }
                }
            }
        }
    }

    static void preprocessCor(
        String path ) throws IOException
    {
        String t = (new File( path )).getName();
        if( t.startsWith( "." ) )
        {
            return;
        }
        String tmp = File.createTempFile( "pp_cs2java", ".txt" ).getAbsolutePath();
        String str_package = "";
        int lines = 0;
        // Vector<String> local_defines = new Vector<String>();
        String enc = mEncoding;

        String tmp2 = File.createTempFile( "pp_cs2java", ".txt" ).getAbsolutePath();
        String indent = "";
        if( -mShiftIndent > 0 )
        {
            for( int i = 0; i < -mShiftIndent; i++ )
            {
                indent += " ";
            }
        }

        doInclude( path, tmp2, indent );

        BufferedWriter sw = null;
        BufferedReader sr = null;
        try
        {
            sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( tmp ), mEncoding ) );
            sr = new BufferedReader( new InputStreamReader( new BOMSkipFileInputStream( tmp2 ), mEncoding ) );
            String line = "";
            int line_num = 0;
            boolean comment_mode = false;
            String comment_indent = "";
            WordReplaceContext context = new WordReplaceContext();
            // ローカルの#define
            Vector<String> local_defines = new Vector<String>();
            while( (line = sr.readLine()) != null )
            {
                String linetrim = line.trim();
                line_num++;
                if( line.startsWith( "#" ) )
                {
                    String trim = line.replace( " ", "" );
                    if( trim.startsWith( "#if!" ) )
                    {
                        String name = trim.substring( 4 );
                        Directives ds = new Directives();
                        DirectiveUnit d = new DirectiveUnit( name, true );
                        ds.push( d );
                        mCurrentDirctive.push( ds );
                    }
                    else if( trim.startsWith( "#if" ) )
                    {
                        String name = trim.substring( 3 );
                        Directives ds = new Directives();
                        DirectiveUnit d = new DirectiveUnit( name, false );
                        ds.push( d );
                        mCurrentDirctive.push( ds );
                    }
                    else if( trim.startsWith( "#else" ) || trim.startsWith( "#elif" ) )
                    {
                        if( mCurrentDirctive.size() > 0 )
                        {
                            // 現在設定されているディレクティブを取得
                            Directives current = mCurrentDirctive.pop();

                            // boolを反転する
                            // まず現在のを配列に取り出して
                            Vector<DirectiveUnit> cache = new Vector<DirectiveUnit>();
                            int c = current.size();
                            for( int i = 0; i < c; i++ )
                            {
                                cache.add( current.pop() );
                            }
                            // 否定して格納
                            for( int i = c - 1; i >= 0; i-- )
                            {
                                DirectiveUnit d = cache.get( i );
                                if( i == 0 )
                                {
                                    d.not = !d.not;
                                }
                                current.push( d );
                            }

                            // elifの場合，さらに追加を行う
                            if( trim.startsWith( "#elif" ) )
                            {
                                String name = trim.substring( 5 );
                                boolean not = false;
                                if( trim.startsWith( "#elif!" ) )
                                {
                                    name = trim.substring( 6 );
                                    not = true;
                                }
                                DirectiveUnit d = new DirectiveUnit( name, not );
                                current.push( d );
                            }

                            // 格納
                            mCurrentDirctive.push( current );
                        }
                    }
                    else if( trim.startsWith( "#endif" ) )
                    {
                        if( mCurrentDirctive.size() > 0 )
                        {
                            mCurrentDirctive.pop();
                        }
                    }
                    else if( trim.startsWith( "#define" ) )
                    {
                        String direct = trim.replace( " ", "" );
                        direct = direct.substring( 7 );
                        local_defines.add( direct );
                    }
                    continue;
                }
                if( linetrim.startsWith( "package" ) )
                {
                    String trim = linetrim.substring( 7 ).replace( " ", "" );
                    int index = trim.indexOf( "//" );
                    if( index >= 1 )
                    {
                        trim = trim.substring( 0, index );
                    }
                    str_package = trim.replace( ";", "" );
                    if( !mPackages.contains( str_package ) )
                    {
                        mPackages.add( str_package );
                    }
                }

                // この行を出力するかどうかを決める
                if( !decideWhetherToPrint( local_defines ) )
                {
                    continue;
                }

                line = replaceText( line, context );

                // 型名の処理
                line = replaceTypeName( line );

                // foreachの処理
                line = replaceForEach( line );

                // イベントハンドラの処理
                line = replaceEventHandler( line );

                // インデントの処理
                line = adjustIndent( line );

                // コメント処理
                if( mParseComment )
                {
                    boolean draft_comment_mode;
                    int ind = line.indexOf( "///" );
                    if( ind >= 0 )
                    {
                        if( line.trim().startsWith( "///" ) )
                        {
                            draft_comment_mode = true;
                        }
                        else
                        {
                            draft_comment_mode = false;
                        }
                    }
                    else
                    {
                        draft_comment_mode = false;
                    }
                    if( comment_mode )
                    {
                        if( draft_comment_mode )
                        {
                            if( line.indexOf( "</summary>" ) >= 0 )
                            {
                                comment_indent = line.substring( 0, ind );
                                comment_mode = draft_comment_mode;
                                continue;
                            }
                            comment_indent = line.substring( 0, ind );
                            line = comment_indent + " *" + parseParamComment( line.substring( ind + 3 ) );
                        }
                        else
                        {
                            // コメントモードが終了したとき
                            sw.write( comment_indent + " */" );
                            sw.newLine();
                        }
                    }
                    else
                    {
                        if( draft_comment_mode )
                        {
                            if( line.indexOf( "<summary>" ) >= 0 )
                            {
                                comment_indent = line.substring( 0, ind );
                                line = comment_indent + "/**";
                            }
                            else if( line.indexOf( "</summary>" ) >= 0 )
                            {
                                comment_indent = line.substring( 0, ind );
                                comment_mode = draft_comment_mode;
                                continue;
                            }
                            else
                            {
                                comment_indent = line.substring( 0, ind );
                                line = comment_indent + " * " + parseParamComment( line.substring( ind + 3 ) );
                            }
                        }
                    }
                    comment_mode = draft_comment_mode;
                }
                if( !line.contains( "#region" ) && !line.contains( "#endregion" ) && !line.contains( "[Serializable]" ) )
                {
                    sw.write( line );
                    sw.newLine();
                    if( !str.compare( line, "" ) )
                    {
                        lines++;
                    }
                }
            }
        }
        catch( Exception ex )
        {
            System.err.println( "pp_cs2java#preprocessCor; ex=" + ex );
            bailOut( ex );
        }
        finally
        {
            if( sw != null )
            {
                try
                {
                    sw.close();
                }
                catch( Exception ex2 )
                {
                    System.err.println( "pp_cs2java#preprocessCor; ex2=" + ex2 );
                    bailOut( ex2 );
                }
            }
            if( sr != null )
            {
                try
                {
                    sr.close();
                }
                catch( Exception ex2 )
                {
                    System.err.println( "pp_cs2java#preprocessCor; ex2=" + ex2 );
                    bailOut( ex2 );
                }
            }
        }

        String out_path = "";
        if( str.compare( mTargetFileOut, "" ) )
        {
            if( str.compare( str_package, "" ) )
            {
                out_path = util.combine( mBaseDir, util.getFileNameWithoutExtension( path ) + ".java" );
            }
            else
            {
                String[] spl = str_package.split( "." );
                if( !(new File( mBaseDir )).exists() )
                {
                    util.createDirectory( mBaseDir );
                }
                for( int i = 0; i < spl.length; i++ )
                {
                    String dir = mBaseDir;
                    for( int j = 0; j <= i; j++ )
                    {
                        dir = util.combine( dir, spl[j] );
                    }
                    if( !(new File( dir )).exists() )
                    {
                        util.createDirectory( dir );
                    }
                }
                out_path = mBaseDir;
                for( int i = 0; i < spl.length; i++ )
                {
                    out_path = util.combine( out_path, spl[i] );
                }
                out_path = util.combine( out_path, util.getFileNameWithoutExtension( path ) + ".java" );
            }
        }
        else
        {
            out_path = mTargetFileOut;
        }

        if( (new File( out_path )).exists() )
        {
            (new File( out_path )).delete();
        }
        if( !mIgnoreEmpty || (mIgnoreEmpty && lines > 0) )
        {
            if( !str.compare( str_package, "" ) || !mIgnoreUnknownPackage )
            {
                String class_name = util.getFileNameWithoutExtension( path );
                mClasses.add( class_name );
                util.copyFile( tmp, out_path );
            }
        }
        (new File( tmp )).delete();
    }

    /**
     * 行を出力するかどうかを決めます．
     * 
     * @param local_defines
     *            ローカルで定義されているプリプロセッサディレクティブのリスト．
     * @return 行を出力する場合はtrue，そうでない場合はfalseを返します．
     */
    private static boolean decideWhetherToPrint(
        Vector<String> local_defines )
    {
        boolean print_this_line = true;// s_current_dirctive.Count <= 0;

        {
            // ディレクティブの定義状態を調べる
            // 現在のディレクティブを全て取り出す
            Vector<DirectiveUnit> defs = new Vector<DirectiveUnit>(); // 定義されているべきディレクティブ
            Vector<DirectiveUnit> defs_not = new Vector<DirectiveUnit>(); // 定義されているとだめなディレクティブ
            for( Directives ds : mCurrentDirctive )
            {
                for( DirectiveUnit d : ds )
                {
                    (d.not ? defs_not : defs).add( d );
                }
            }
            // defsのアイテムについて，s_definesとlocal_definesに全て入ってるかどうかチェック
            for( DirectiveUnit d : defs )
            {
                if( (!mDefines.contains( d.name )) && (!local_defines.contains( d.name )) )
                {
                    print_this_line = false;
                    break;
                }
            }
            // defs_notのアイテム全てについて，s_definesまたはlocal_definesに入っていないことをチェック
            if( print_this_line )
            {
                for( DirectiveUnit d : defs_not )
                {
                    if( mDefines.contains( d.name ) )
                    {
                        print_this_line = false;
                        break;
                    }
                    if( local_defines.contains( d.name ) )
                    {
                        print_this_line = false;
                        break;
                    }
                }
            }
        }
        return print_this_line;
    }

    /**
     * @param line
     * @return
     */
    private static String adjustIndent(
        String line )
    {
        if( mShiftIndent < 0 )
        {
            String search = "";
            for( int i = 0; i < -mShiftIndent; i++ )
            {
                search += " ";// new String( ' ', -s_shift_indent );
            }
            if( line.startsWith( search ) )
            {
                line = line.substring( -mShiftIndent );
            }
        }
        else if( mShiftIndent > 0 )
        {
            String s = "";
            for( int i = 0; i < mShiftIndent; i++ )
            {
                s += " ";
            }
            line = s + line;
        }
        return line;
    }

    /**
     * イベントハンドラの記述箇所の処理を行います．
     * 
     * @param line
     *            処理する行データ．
     * @return 処理後の行データ．
     */
    private static String replaceEventHandler(
        String line )
    {
        int indx_event_handler = line.indexOf( "EventHandler" );
        boolean success = true;
        if( indx_event_handler < 0 )
        {
            success = false;
        }

        // "EventHandler"の直前にある"new "を検出
        int indx_new = -1;
        if( success )
        {
            indx_new = line.lastIndexOf( "new ", indx_event_handler );
            if( indx_new < 0 )
            {
                success = false;
            }
        }

        // イベントハンドラの追加なら1，削除なら-1，演算子が見つからなかったら0
        int operator_mode = 0;
        int indx_operator = -1;
        if( success )
        {
            success = false;
            indx_operator = line.lastIndexOf( "+=", indx_new );
            if( indx_operator >= 0 )
            {
                operator_mode = 1;
                success = true;
            }
            else
            {
                indx_operator = line.lastIndexOf( "-=", indx_new );
                if( indx_operator >= 0 )
                {
                    operator_mode = -1;
                    success = true;
                }
            }
        }

        // +=, -=演算子の直前にあるドットを検出
        int indx_dot = -1;
        if( success )
        {
            indx_dot = line.lastIndexOf( ".", indx_operator );
            if( indx_dot < 0 )
            {
                success = false;
            }
        }

        // "EventHandler"の直後にある"("を検出
        int indx_bla = -1;
        if( success )
        {
            indx_bla = line.indexOf( "(", indx_event_handler );
            if( indx_bla < 0 )
            {
                success = false;
            }
        }

        // 行の最後にある")"を検出
        int indx_cket = -1;
        if( success )
        {
            indx_cket = line.lastIndexOf( ")" );
            if( indx_cket < 0 )
            {
                success = false;
            }
        }

        // 上記のインデクスるは，下記の順番で並んでないといけない
        // indx_dot < indx_operator < indx_new < indx_event_handler
        // < indx_bla < indx_cket
        if( success )
        {
            success = (indx_dot < indx_operator) && (indx_operator < indx_new) && (indx_new < indx_event_handler) && (indx_event_handler < indx_bla) && (indx_bla < indx_cket);
        }

        // すべてのマッチが成功した！
        if( success )
        {
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
        return line;
    }

    /**
     * foreachの置換処理を行います．
     * 
     * @param line
     *            処理する行データ．
     * @return 処理後の行データ．
     */
    private static String replaceForEach(
        String line )
    {
        int index_foreach = line.indexOf( "foreach" );
        if( index_foreach >= 0 )
        {
            int index_in = line.indexOf( " in " );
            if( index_in >= 0 )
            {
                line = line.substring( 0, index_foreach ) + "for" + line.substring( index_foreach + 7, index_in ) + " : " + line.substring( index_in + 4 );
            }
        }
        return line;
    }

    /**
     * 型名の置換を行います．
     * 
     * @param line
     *            処理する行データ．
     * @return 処理後の行データ．
     */
    private static String replaceTypeName(
        String line )
    {
        int index_typeof = line.indexOf( "typeof" );
        while( index_typeof >= 0 )
        {
            int bra = line.indexOf( "(", index_typeof );
            int cket = line.indexOf( ")", index_typeof );
            if( bra < 0 || cket < 0 )
            {
                break;
            }
            String prefix = line.substring( 0, index_typeof );
            String suffix = line.substring( cket + 1 );
            String typename = line.substring( bra + 1, cket ).trim();
            String javaclass = typename + ".class";
            if( str.compare( typename, "int" ) )
            {
                javaclass = "Integer.TYPE";
            }
            else if( str.compare( typename, "float" ) )
            {
                javaclass = "Float.TYPE";
            }
            else if( str.compare( typename, "double" ) )
            {
                javaclass = "Double.TYPE";
            }
            else if( str.compare( typename, "void" ) )
            {
                javaclass = "Void.TYPE";
            }
            else if( str.compare( typename, "bool" ) || str.compare( typename, "boolean" ) )
            {
                javaclass = "Boolean.TYPE";
            }
            else if( str.compare( typename, "byte" ) )
            {
                javaclass = "Byte.TYPE";
            }
            line = prefix + javaclass + " " + suffix;
            index_typeof = line.indexOf( "typeof" );
        }
        return line;
    }

    /**
     * s_current_directiveの状態を1行で出力します(最後の改行無し)
     */
    private static void printCurrentDirectives()
    {
        boolean first0 = true;
        for( Directives d : mCurrentDirctive/* .ToArray() */)
        {
            System.out.print( (first0 ? "" : ",") + "[" );
            first0 = false;
            boolean first = true;
            for( DirectiveUnit du : d/* .ToArray() */)
            {
                System.out.print( (first ? "" : ",") + (du.not ? "!" : "") + du.name );
                first = false;
            }
            System.out.print( "]" );
        }
    }

    /**
     * &lt;param name="foo"&gt;comment&lt;param/&gt; のようなコメントを \@param foo
     * comment みたいに整形する
     */
    private static String parseParamComment(
        String line )
    {
        String bla1 = "<param name=\"";
        int indx_pre = line.indexOf( bla1 );
        boolean mode_return = false;
        if( indx_pre < 0 )
        {
            bla1 = "<returns";
            indx_pre = line.indexOf( bla1 );
            if( indx_pre < 0 )
            {
                return line;
            }
            else
            {
                mode_return = true;
            }
        }
        int indx_cket1 = -1;
        String cket1 = "";
        if( mode_return )
        {
            cket1 = ">";
        }
        else
        {
            cket1 = "\">";
        }
        indx_cket1 = line.indexOf( cket1, indx_pre );
        if( indx_cket1 < 0 )
        {
            return line;
        }
        int indx_bla2 = line.lastIndexOf( "</" );
        if( indx_bla2 < 0 )
        {
            return line;
        }
        int indx_cket2 = line.indexOf( ">", indx_bla2 );
        if( indx_cket2 < 0 )
        {
            return line;
        }
        String pre = line.substring( 0, indx_pre );
        String param = "";
        if( !mode_return )
        {
            param = line.substring( indx_pre + bla1.length(), indx_cket1 );
        }
        String message = line.substring( indx_cket1 + cket1.length(), indx_bla2 );
        if( mode_return )
        {
            return pre + "@return " + message;
        }
        else
        {
            return pre + "@param " + param + " " + message;
        }
    }

    private static String replaceText_OLD(
        String line,
        WordReplaceContext context )
    {
        for( int i = 0; i < REPLACE.length/* .GetLength( 0 ) */; i++ )
        {
            line = line.replace( REPLACE[i][0], REPLACE[i][1] );
        }
        return line;
    }

    private static boolean[] checkStringLiteralAndComment(
        String line,
        WordReplaceContext context )
    {
        // 文字を無視するかどうかを表す
        boolean[] status = new boolean[line.length()];
        for( int i = 0; i < status.length; i++ )
        {
            status[i] = false;
        }

        // /**/によるコメントアウトを検出
        boolean line_comment_started = false; // //による行コメントが開始されているかどうか
        for( int i = 0; i < line.length(); i++ )
        {
            char c = line.charAt( i );
            if( line_comment_started )
            {
                status[i] = true;
                continue;
            }
            if( c == '/' )
            {
                if( context.isStringLiteralStarted )
                {
                    status[i] = true;
                }
                else
                {
                    if( context.isCommentStarted )
                    {
                        if( i > 0 && line.charAt( i - 1 ) == '*' )
                        {
                            status[i - 1] = true;
                            status[i] = true;
                            context.isCommentStarted = false;
                        }
                    }
                    else
                    {
                        if( i > 0 && line.charAt( i - 1 ) == '/' )
                        {
                            status[i - 1] = true;
                            status[i] = true;
                            line_comment_started = true;
                        }
                    }
                }
            }
            else if( c == '*' )
            {
                if( context.isStringLiteralStarted )
                {
                    status[i] = true;
                }
                else
                {
                    if( !context.isCommentStarted )
                    {
                        if( i > 0 && line.charAt( i - 1 ) == '/' )
                        {
                            status[i - 1] = true;
                            status[i] = true;
                            context.isCommentStarted = true;
                        }
                    }
                }
            }
            else if( c == '"' )
            {
                if( context.isStringLiteralStarted )
                {
                    if( i > 0 && line.charAt( i - 1 ) == '\\' )
                    {
                        status[i] = true;
                    }
                    else
                    {
                        status[i] = true;
                        context.isStringLiteralStarted = false;
                    }
                }
                else
                {
                    if( context.isCommentStarted )
                    {
                        status[i] = true;
                    }
                    else
                    {
                        status[i] = true;
                        context.isStringLiteralStarted = true;
                    }
                }
            }
            else
            {
                if( context.isStringLiteralStarted || context.isCommentStarted )
                {
                    status[i] = true;
                }
            }
        }
        /*
         * string d = ""; for( int i = 0; i < status.Length; i++ ){ d +=
         * (status[i] ? "T" : "F"); } System.out.println( line );
         * System.out.println( d );
         */
        return status;
    }

    private static String replaceText(
        String line,
        WordReplaceContext context )
    {
        // 置換する文字列を検索
        for( int i = 0; i < REPLACE.length/* .GetLength( 0 ) */; i++ )
        {
            String search = REPLACE[i][0];
            String replace = REPLACE[i][1];
            boolean changed = true;
            int start = 0;
            int indx = line.indexOf( search, start );
            while( changed || indx >= 0 )
            {
                changed = false;
                WordReplaceContext ct = (WordReplaceContext)context.clone();
                boolean[] status = checkStringLiteralAndComment( line, ct );
                // System.out.println( "replaceText_DRAFT; search=" + search +
                // "; indx=" + indx );
                if( indx >= 0 )
                {
                    boolean replace_ok = true;
                    for( int j = indx; j < indx + search.length(); j++ )
                    {
                        if( status[j] )
                        {
                            replace_ok = false;
                            break;
                        }
                    }
                    // System.out.println( "replaceText_DRAFT; replace_ok=" +
                    // replace_ok );
                    if( replace_ok )
                    {
                        line = (indx > 0 ? line.substring( 0, indx ) : "") + replace + line.substring( indx + search.length() );
                        start = indx + 1;
                        changed = true;
                    }
                    else
                    {
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
