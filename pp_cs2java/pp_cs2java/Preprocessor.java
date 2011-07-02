/*
 * pp_cs2java
 * Copyright © 2010-2011 kbinani
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

class Preprocessor{
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
    static final String[][] REPLACE_CS2JAVA = new String[][]{
        {"string", "String"},
        {" bool ", " boolean "},
        { ".Equals(", ".equals(" }, { ".ToString(", ".toString(" },
        { ".StartsWith(", ".startsWith(" }, { ".EndsWith(", ".endsWith(" },
        { ".Substring(", ".substring(" }, { " const ", " static final " },
        { " readonly ", " final " }, { " struct ", " class " },
        { "base.", "super." }, { " override ", " " }, { " virtual ", " " },
        { " is ", " instanceof " }, { ".Length", ".length" },
        { "int.MaxValue", "Integer.MAX_VALUE" },
        { "int.MinValue", "Integer.MIN_VALUE" },
        { "double.MaxValue", "Double.MAX_VALUE" },
        { "double.MinValue", "Double.MIN_VALUE" },
        { " lock", " synchronized" }, { ".Trim()", ".trim()" },
        { ".Replace(", ".replace(" }, { ".ToCharArray()", ".toCharArray()" },
        { "Math.Min(", "Math.min(" }, { "Math.Max(", "Math.max(" },
        { "Math.Log(", "Math.log(" }, { "Math.Exp(", "Math.exp(" },
        { "Math.Ceiling(", "Math.ceil(" }, { "Math.Floor(", "Math.floor(" },
        { "Math.Abs(", "Math.abs(" }, { "Math.Pow(", "Math.pow(" },
        { "Math.Sin(", "Math.sin(" }, { "Math.Cos(", "Math.cos(" },
        { "Math.Tan(", "Math.tan(" }, { "Math.Sqrt(", "Math.sqrt(" },
        { "Math.Asin(", "Math.asin(" }, { "Math.Acos(", "Math.acos(" },
        { "Math.Atan2(", "Math.atan2(" }, { "Math.Atan(", "Math.atan(" },
        { ".ToLower()", ".toLowerCase()" }, { ".ToUpper()", ".toUpperCase()" },
        { ".IndexOf(", ".indexOf(" },
        { " : ICloneable", " implements Cloneable" },
        { " : Iterator", " implements Iterator" },
        { ".LastIndexOf(", ".lastIndexOf(" }, { "base", "super" },
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
        { " Type ", " Class " }, { " List<", " Vector<" },
        { ".Count", ".size()" }, { ".Clear()", ".clear()" }, };
    static String[][] REPLACE_CS2CPP = new String[][]{ { "public ", "public: " },
        { "private ", "private: " }, { "vec.", "vec::" }, { "dic.", "dic::" },
        { "sout.", "sout::" }, { "serr.", "serr::" }, { "conv.", "conv::" },
        { "fsys.", "fsys::" }, { "str.", "str::" }, { "List<", "vector<" },
        { "this.", "this->" }, };

    // private static Regex reg_eventhandler = new Regex(
    // @"(?<pre>.*?)(?<instance>\w*)[.]*(?<event>\w*)\s*(?<operator>[\+\-]\=)\s*new\s*(?<handler>\w*)EventHandler\s*\(\s*(?<method>.*)\s*\)"
    // );

    static class ProcessFileContext{
        public int lines;
        public String packageName;

        private BufferedWriter writer;
        /**
         * 直前の行に[PureVirtualFunction]属性が指定されていた場合にtrue
         */
        private boolean previousLineContainsPureVirtualFunctionAttribute = false;

        public ProcessFileContext( BufferedWriter writer ){
            lines = 0;
            packageName = "";
            this.writer = writer;
        }
        
        public void writeLine( String line ){
            int indx = line.indexOf( "[PureVirtualFunction]" );
            if( indx >= 0 ){
                previousLineContainsPureVirtualFunctionAttribute = true;
                return;
            }
            if( previousLineContainsPureVirtualFunctionAttribute ){
                previousLineContainsPureVirtualFunctionAttribute = false;
                String trimmed = line.trim();
                int first = line.indexOf( trimmed );
                if( first < 0 ){
                    first = 0;
                }
                int indexSemicolon = line.indexOf( ";" );
                String suffix = trimmed;
                if( indexSemicolon >= 0 ){
                    suffix = trimmed.replace( ";", " = 0;" );
                }
                String newLine = line.substring( 0, first ) + "virtual " + suffix;
                line = newLine;
            }
            try{
                writer.write( line );
                writer.newLine();
            }catch( Exception ex ){
                ex.printStackTrace( System.err );
            }
        }
    }

    static void printUsage(){
        System.out.println( "pp_cs2java" );
        System.out.println( "Copyright (C) 2011 kbinani, All Rights Reserved" );
        System.out.println( "Usage:" );
        System.out
                .println( "    pp_cs2java -t [search path] -b [output path] {options}" );
        System.out
                .println( "    pp_cs2java -i [in file] -o [out file] {options}" );
        System.out.println( "Options:" );
        System.out
                .println( "    -r                     enable recursive search" );
        System.out
                .println( "    -e                     do not ignore empty file" );
        System.out.println( "    -c                     enable comment parse" );
        System.out
                .println( "    -D[name]               define preprocessor directive" );
        System.out
                .println( "    -t [path]              set search directory path" );
        System.out.println( "    -i [path]              set target file" );
        System.out.println( "    -o [path]              name of output file" );
        System.out
                .println( "    -b [path]              set output directory path" );
        System.out
                .println( "    -s [number]            increase indent [number] column(s)" );
        System.out.println( "                           (decrease if minus)" );
        System.out
                .println( "    -encoding [enc. name]  set text file encoding" );
        System.out
                .println( "    -m [path]              set path of source code for debug" );
        System.out
                .println( "    -u                     ignoring unknown package" );
        System.out
                .println( "    -l [path]              set path of \"import\" log-file" );
        System.out
                .println( "    -ly                    overwrite \"import\" log-file" );
        System.out.println( "    -h,-help               print this help" );
        System.out
                .println( "    --replace-X            enable replace words list" );
        System.out.println( "                           X: java or cpp" );
    }

    public static void main(
        String[] args ){
        // 引数を解析
        boolean print_usage = parseArguments( args );

        // -i オプションと -t オプションは共存できないので死ぬ
        if( !str.compare( mTargetDir, "" ) && !str.compare( mTargetFile, "" ) ){
            System.out
                    .println( "error; confliction in command line arguments. -i and -t option can't co-exists" );
            return;
        }

        if( print_usage ){
            printUsage();
        }
        if( args.length <= 0 ){
            return;
        }

        // 読み込みファイルと，出力先のディレクトリがどちらも設定されなかった場合は何も出来ない
        if( str.compare( mTargetFile, "" ) && str.compare( mTargetDir, "" ) ){
            System.out.println( "error; target file or path has not specified" );
            return;
        }

        if( !str.compare( mTargetDir, "" ) ){
            if( str.compare( mBaseDir, "" ) ){
                System.out.println( "error; output path has not specified" );
                return;
            }

            if( mRecurse ){
                preprocessRecurse( mTargetDir );
            }else{
                preprocess( mTargetDir );
            }
        }
        if( !str.compare( mTargetFile, "" ) ){
            try{
                preprocessFile( mTargetFile );
            }catch( Exception ex ){
                System.err.println( "pp_cs2java#main; ex=" + ex );
                ex.printStackTrace();
            }
        }

        if( !str.compare( mMainClassPath, "" ) ){
            BufferedWriter sw = null;
            try{
                sw =
                    new BufferedWriter( new OutputStreamWriter(
                            new FileOutputStream( mMainClassPath ) ) );
                for( String pkg : mPackages ){
                    sw.write( "import " + pkg + ".*;" );
                    sw.newLine();
                }
                sw.write( "class "
                    + util.getFileNameWithoutExtension( mMainClassPath ) + "{" );
                sw.newLine();
                sw.write( "    public static void main( String[] args ){" );
                sw.newLine();
                int count = 0;
                for( String cls : mClasses ){
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

        if( !str.compare( mLogfile, "" ) ){
            BufferedWriter sw = null;
            try{
                boolean append = mLogfileOverwrite;
                if( !util.isFileExists( mLogfile ) ){
                    append = false;
                }
                sw =
                    new BufferedWriter( new OutputStreamWriter(
                            new FileOutputStream( mLogfile, append ) ) );
                for( String s : mIncluded ){
                    sw.write( s );
                    sw.newLine();
                }
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
    }

    /**
     * 実行時の引数を解析します．
     * 
     * @param args
     *            解析する引数．
     * @return 使い方の説明文を表示するオプションがONだった場合にtrue，それ以外はfalseを返します．
     */
    private static boolean parseArguments(
        String[] args ){
        String current_parse = "";
        boolean print_usage = false;
        if( args.length <= 0 ){
            print_usage = true;
        }
        for( int i = 0; i < args.length; i++ ){
            if( args[i].startsWith( "-" ) && !str.compare( current_parse, "-s" ) ){
                current_parse = args[i];
                if( str.compare( current_parse, "-r" ) ){
                    mRecurse = true;
                    current_parse = "";
                }else if( str.compare( current_parse, "-e" ) ){
                    mIgnoreEmpty = false;
                    current_parse = "";
                }else if( str.compare( current_parse, "-c" ) ){
                    mParseComment = true;
                    current_parse = "";
                }else if( str.compare( current_parse, "-u" ) ){
                    mIgnoreUnknownPackage = true;
                    current_parse = "";
                }else if( current_parse.startsWith( "-D" ) ){
                    String def = current_parse.substring( 2 );
                    if( !mDefines.contains( def ) ){
                        mDefines.add( def );
                    }
                    current_parse = "";
                }else if( str.compare( current_parse, "-h" )
                    || str.compare( current_parse, "-help" ) ){
                    print_usage = true;
                }else if( str.compare( current_parse, "-ly" ) ){
                    mLogfileOverwrite = true;
                }else if( current_parse.startsWith( "--replace-" ) ){
                    String type =
                        current_parse.substring( "--replace-".length() );
                    if( str.compare( type, "java" ) ){
                        REPLACE = REPLACE_CS2JAVA;
                        mMode = ReplaceMode.JAVA;
                    }else if( str.compare( type, "cpp" ) ){
                        REPLACE = REPLACE_CS2CPP;
                        mMode = ReplaceMode.CPP;
                    }
                }
            }else{
                if( str.compare( current_parse, "-t" ) ){
                    mTargetDir = args[i];
                    current_parse = "";
                }else if( str.compare( current_parse, "-b" ) ){
                    mBaseDir = args[i];
                    current_parse = "";
                }else if( str.compare( current_parse, "-encoding" ) ){
                    mEncoding = args[i];
                    current_parse = "";
                }else if( str.compare( current_parse, "-s" ) ){
                    mShiftIndent = Integer.parseInt( args[i] );
                    current_parse = "";
                }else if( str.compare( current_parse, "-m" ) ){
                    mMainClassPath = args[i];
                    current_parse = "";
                }else if( str.compare( current_parse, "-i" ) ){
                    mTargetFile = args[i];
                    current_parse = "";
                }else if( str.compare( current_parse, "-o" ) ){
                    mTargetFileOut = args[i];
                    current_parse = "";
                }else if( str.compare( current_parse, "-l" ) ){
                    mLogfile = args[i];
                    current_parse = "";
                }
            }
        }
        return print_usage;
    }

    static void bailOut(
        Exception ex ){
        ex.printStackTrace();
        System.exit( 1 );
    }

    static void preprocess(
        String directory_path ){
        for( String fi : util.listFiles( directory_path, ".cs" ) ){
            try{
                preprocessFile( fi );
            }catch( Exception ex ){
                System.err.println( "pp_cs2java#preprocess; ex=" + ex );
                bailOut( ex );
            }
        }
    }

    static void preprocessRecurse(
        String dir ){
        preprocess( dir );
        File f = new File( dir );
        for( File subdir : f.listFiles() ){
            if( subdir.isDirectory() ){
                preprocessRecurse( subdir.getAbsolutePath() );
            }
        }
    }

    /**
     * 1個のファイルに対してプリプロセス処理を行います．
     * 
     * @param in_path
     *            処理するファイルのパス．
     */
    static void preprocessFile(
        String in_path ) throws IOException, Exception{
        // ファイル名が.で始まっていたら何もしない
        String t = (new File( in_path )).getName();
        if( t.startsWith( "." ) ){
            return;
        }

        String out_path =
            File.createTempFile( "pp_cs2java", ".txt" ).getAbsolutePath();
        String tmp2 =
            File.createTempFile( "pp_cs2java", ".txt" ).getAbsolutePath();
        String indent = "";
        if( -mShiftIndent > 0 ){
            for( int i = 0; i < -mShiftIndent; i++ ){
                indent += " ";
            }
        }

        // INCLUDEの処理
        doInclude( in_path, tmp2, indent );

        // ファイルをロード
        BufferedReader reader = null;
        SourceText src = null;
        try{
            reader =
                new BufferedReader( new InputStreamReader(
                        new BOMSkipFileInputStream( tmp2 ), mEncoding ) );
            src = new SourceText( reader );
        }catch( Exception ex ){
            ex.printStackTrace( System.err );
            return;
        }finally{
            if( reader != null ){
                try{
                    reader.close();
                }catch( Exception ex ){
                    ex.printStackTrace( System.err );
                }
            }
        }

        // 一時ファイルはこれ以降不要なので削除
        (new File( tmp2 )).delete();

        // 既に定義されているプリプロセッサディレクティブを持っておく
        Vector<String> local_defines = new Vector<String>();
        for( String d : mDefines ){
            local_defines.add( d );
        }

        // 書きこむ
        BufferedWriter writer = null;
        ProcessFileContext context = null;
        try{
            writer =
                new BufferedWriter( new OutputStreamWriter(
                        new FileOutputStream( out_path ), mEncoding ) );
            int count = src.getLineCount();
            context = new ProcessFileContext( writer );
            processFileRecursive( src, context, 0, count - 1, local_defines );
        }catch( Exception ex ){
            ex.printStackTrace( System.err );
        }finally{
            if( writer != null ){
                try{
                    writer.close();
                }catch( Exception ex ){
                    ex.printStackTrace( System.err );
                }
            }
        }

        copyResultFile( in_path, out_path, context.packageName, context.lines );
    }

    private static void processFileRecursive(
        SourceText src,
        ProcessFileContext context,
        int start_line_number,
        int end_line_number,
        Vector<String> local_defines
    )
        throws Exception
    {
        for( int i = start_line_number; i <= end_line_number; i++ ){
            String line = src.getLine( i );
            if( src.isContainsPreprocessorDirective( i, "#define" ) ){
                // ローカルのディレクティブ定義を検出
                int indx = line.indexOf( "#define" );
                String directive = "";
                String suffix = "";
                int j = indx + "#define".length();
                while( j < line.length() ){
                    if( src.isInComment( i, j ) ){
                        suffix = line.substring( j );
                        break;
                    }
                    directive = directive + String.valueOf( line.charAt( j ) );
                    j++;
                }
                directive = directive.trim();
                local_defines.add( directive );
                if( mMode == ReplaceMode.CPP ){
                    context.writeLine( "#define " + directive + " 1" + suffix  );
                }
            }else if( src.isContainsPreprocessorDirective( i, "#if" )
                || src.isContainsPreprocessorDirective( i, "#elif" ) ){
                // #ifまたは#elifが来た場合，式を評価して内部に入るかどうかを決めなければならない
                String search = "#if";
                boolean readin = true;
                int indx = line.indexOf( search );
                if( indx < 0 ){
                    search = "#elif";
                    indx = line.indexOf( search );
                }
                String equation =
                    line.substring( indx + search.length() ).replace( " ", "" );
                readin = Evaluator.eval( equation, local_defines );
                int end_if = src.findEndIfSentence( i );
                int else_if = src.findElseSentence( i );
                
                if( mMode == ReplaceMode.CPP ){
                    context.writeLine( line );
                }
                
                if( readin ){
                    // #if, #elifの判定がtrueなためブロックの中身を読みに行く場合，
                    // ブロックの中身がどこまでかを検出
                    int end_line = else_if;
                    if( end_line < 0 ){
                        end_line = end_if;
                    }
                    processFileRecursive( src, context, i + 1, end_line - 1, local_defines );
                    i = end_if;
                    if( mMode == ReplaceMode.CPP ){
                        context.writeLine( "#endif" );
                    }
                    continue;
                }else{
                    // #if, #elifの判定がfalseなため，次の#else, #elifまでスキップする．
                    // 次の#else,#elifがない場合，#endifまでスキップする
                    if( else_if >= 0 ){
                        i = else_if - 1;
                    }else{
                        i = end_if;
                        if( mMode == ReplaceMode.CPP ){
                            context.writeLine( "#endif" );
                        }
                    }
                    continue;
                }
            }else if( src.isContainsPreprocessorDirective( i, "#else" ) ){
                if( mMode == ReplaceMode.CPP ){
                    context.writeLine( line );
                }
                // elseが来た場合，中身を読み込まなくてはいけない
                int end_if = src.findEndIfSentence( i );
                processFileRecursive( src, context, i + 1, end_if - 1, local_defines );
                i = end_if - 1;
            }else if( src.isContainsPreprocessorDirective( i, "#region" )
                || src.isContainsPreprocessorDirective( i, "#endregion" ) ){
                continue;
            }else if( src.isContainsPreprocessorDirective( i, "#endif" ) ){
                if( mMode == ReplaceMode.CPP ){
                    context.writeLine( line );
                }
                continue;
            }else{
                context.lines++;

                line = replaceText( i, src );

                // 型名の処理
                line = replaceTypeName( line );
                // foreachの処理
                line = replaceForEach( line );
                // イベントハンドラの処理
                line = replaceEventHandler( line );
                // インデントの処理
                line = adjustIndent( line );

                context.writeLine( line );
                // パッケージ名を検出
                int index_package = line.indexOf( "package " );
                if( index_package >= 0 ){
                    context.packageName =
                        line.substring( index_package ).trim();
                }
            }
        }
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
        String indent ){
        BufferedWriter sw = null;
        BufferedReader sr = null;
        try{
            sw =
                new BufferedWriter( new OutputStreamWriter(
                        new FileOutputStream( out_path ), mEncoding ) );
            sr =
                new BufferedReader( new InputStreamReader(
                        new BOMSkipFileInputStream( in_path ), mEncoding ) );
            String line = "";
            while( (line = sr.readLine()) != null ){
                String linetrim = line.trim();
                if( linetrim.startsWith( "//INCLUDE " ) ){
                    doIncludeAll( in_path, indent, sw, linetrim );
                }else if( linetrim.startsWith( "//INCLUDE-SECTION " ) ){
                    doIncludeSection( in_path, indent, sw, linetrim );
                }else{
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
        String linetrim ){
        String s = linetrim.substring( 18 );
        String[] spl = s.split( " " );
        String section_name = spl[0];
        String p = spl[1];
        String include_path = util.combine( util.getDirectoryName( path ), p );
        if( !mIncluded.contains( p ) ){
            mIncluded.add( p );
        }
        if( (new File( include_path )).exists() ){
            BufferedReader sr_include = null;
            try{
                sr_include =
                    new BufferedReader( new InputStreamReader(
                            new BOMSkipFileInputStream( include_path ),
                            mEncoding ) );
                String line2 = "";
                boolean section_begin = false;
                while( (line2 = sr_include.readLine()) != null ){
                    if( section_begin ){
                        String strim = line2.trim();
                        if( strim.startsWith( "//SECTION-END-" ) ){
                            String name = strim.substring( 14 );
                            if( str.compare( name, section_name ) ){
                                section_begin = false;
                                break;
                            }
                        }else{
                            sw.write( indent + line2 );
                            sw.newLine();
                        }
                    }else{
                        String strim = line2.trim();
                        if( strim.startsWith( "//SECTION-BEGIN-" ) ){
                            String name = strim.substring( 16 );
                            if( str.compare( name, section_name ) ){
                                section_begin = true;
                            }
                        }
                    }
                }
            }catch( Exception ex ){
                System.err.println( "pp_cs2java#preprocessCor; ex=" + ex );
                bailOut( ex );
            }finally{
                if( sr_include != null ){
                    try{
                        sr_include.close();
                    }catch( Exception ex2 ){
                        System.err.println( "pp_cs2java#preprocessCor; ex2="
                            + ex2 );
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
        String linetrim ){
        String p = linetrim.substring( 10 );
        String include_path = util.combine( util.getDirectoryName( path ), p );
        if( !mIncluded.contains( p ) ){
            mIncluded.add( p );
        }
        if( (new File( include_path )).exists() ){
            BufferedReader sr_include = null;
            try{
                sr_include =
                    new BufferedReader( new InputStreamReader(
                            new BOMSkipFileInputStream( include_path ),
                            mEncoding ) );
                String line2 = "";
                while( (line2 = sr_include.readLine()) != null ){
                    sw.write( indent + line2 );
                    sw.newLine();
                }
            }catch( Exception ex ){
                System.err.println( "pp_cs2java#preprocessCor; ex=" + ex );
                bailOut( ex );
            }finally{
                if( sr_include != null ){
                    try{
                        sr_include.close();
                    }catch( Exception ex2 ){
                        System.err.println( "pp_cs2java#preprocessCor; ex2="
                            + ex2 );
                        bailOut( ex2 );
                    }
                }
            }
        }
    }

    /**
     * @param path
     *            プリプロセスの元になったファイル
     * @param path_result
     *            プリプロセスの結果ファイル
     */
    private static void copyResultFile(
        String path,
        String path_result,
        String str_package,
        int lines ) throws FileNotFoundException, IOException{
        String out_path = "";
        if( str.compare( mTargetFileOut, "" ) ){
            if( str.compare( str_package, "" ) ){
                out_path =
                    util.combine( mBaseDir, util
                            .getFileNameWithoutExtension( path ) + ".java" );
            }else{
                String[] spl = str_package.split( "." );
                if( !(new File( mBaseDir )).exists() ){
                    util.createDirectory( mBaseDir );
                }
                for( int i = 0; i < spl.length; i++ ){
                    String dir = mBaseDir;
                    for( int j = 0; j <= i; j++ ){
                        dir = util.combine( dir, spl[j] );
                    }
                    if( !(new File( dir )).exists() ){
                        util.createDirectory( dir );
                    }
                }
                out_path = mBaseDir;
                for( int i = 0; i < spl.length; i++ ){
                    out_path = util.combine( out_path, spl[i] );
                }
                out_path =
                    util.combine( out_path, util
                            .getFileNameWithoutExtension( path ) + ".java" );
            }
        }else{
            out_path = mTargetFileOut;
        }

        if( (new File( out_path )).exists() ){
            (new File( out_path )).delete();
        }
        if( !mIgnoreEmpty || (mIgnoreEmpty && lines > 0) ){
            if( !str.compare( str_package, "" ) || !mIgnoreUnknownPackage ){
                String class_name = util.getFileNameWithoutExtension( path );
                mClasses.add( class_name );
                util.copyFile( path_result, out_path );
            }
        }
        (new File( path_result )).delete();
    }

    /**
     * @param line
     * @return
     */
    private static String adjustIndent(
        String line ){
        if( mShiftIndent < 0 ){
            String search = "";
            for( int i = 0; i < -mShiftIndent; i++ ){
                search += " ";// new String( ' ', -s_shift_indent );
            }
            if( line.startsWith( search ) ){
                line = line.substring( -mShiftIndent );
            }
        }else if( mShiftIndent > 0 ){
            String s = "";
            for( int i = 0; i < mShiftIndent; i++ ){
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
        String line ){
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
        // indx_dot < indx_operator < indx_new < indx_event_handler
        // < indx_bla < indx_cket
        if( success ){
            success =
                (indx_dot < indx_operator) && (indx_operator < indx_new)
                    && (indx_new < indx_event_handler)
                    && (indx_event_handler < indx_bla)
                    && (indx_bla < indx_cket);
        }

        // すべてのマッチが成功した！
        if( success ){
            String pre_instance = line.substring( 0, indx_dot );
            String ev = line.substring( indx_dot + 1, indx_operator );
            String handler =
                line.substring( indx_new + "new ".length(), indx_bla );
            String method = line.substring( indx_bla + 1, indx_cket );
            ev = ev.trim();
            ev =
                ev.substring( 0, 1 ).toLowerCase() + ev.substring( 1 )
                    + "Event";
            handler = handler.trim();
            method = method.trim();
            line =
                pre_instance + "." + ev
                    + (operator_mode > 0 ? ".add( " : ".remove( ") + "new "
                    + handler + "( this, \"" + method + "\" ) );";
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
        String line ){
        int index_foreach = line.indexOf( "foreach" );
        if( index_foreach >= 0 ){
            int index_in = line.indexOf( " in " );
            if( index_in >= 0 ){
                line =
                    line.substring( 0, index_foreach ) + "for"
                        + line.substring( index_foreach + 7, index_in ) + " : "
                        + line.substring( index_in + 4 );
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
        String line ){
        int index_typeof = line.indexOf( "typeof" );
        while( index_typeof >= 0 ){
            int bra = line.indexOf( "(", index_typeof );
            int cket = line.indexOf( ")", index_typeof );
            if( bra < 0 || cket < 0 ){
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
            }else if( str.compare( typename, "bool" )
                || str.compare( typename, "boolean" ) ){
                javaclass = "Boolean.TYPE";
            }else if( str.compare( typename, "byte" ) ){
                javaclass = "Byte.TYPE";
            }
            line = prefix + javaclass + suffix;
            index_typeof = line.indexOf( "typeof" );
        }
        return line;
    }

    /**
     * 指定した行番号の行データを，起動時に指定された置換ルールに基づいて置換します．
     * 
     * @param line_number
     *            置換を行う行の行番号．
     * @param src
     *            置換対象のドキュメント．
     * @return 置換後の行データを返します．
     */
    private static String replaceText(
        int line_number,
        SourceText src ){
        String line = src.getLine( line_number );
        for( int i = 0; i < REPLACE.length; i++ ){
            String search = REPLACE[i][0];
            String replace = REPLACE[i][1];
            boolean changed = true;
            int start = 0;
            int indx = line.indexOf( search, start );
            while( changed || indx >= 0 ){
                changed = false;
                if( indx >= 0 ){
                    boolean replace_ok = true;
                    // 検出した文字列がコメントの範囲に一部でも含まれていたら，置換は行わない
                    for( int j = indx; j < indx + search.length(); j++ ){
                        if( src.isInComment( line_number, j ) ){
                            replace_ok = false;
                            break;
                        }
                    }
                    // 置換しても大丈夫な場合
                    if( replace_ok ){
                        line =
                            (indx > 0 ? line.substring( 0, indx ) : "")
                                + replace
                                + line.substring( indx + search.length() );
                        start = indx + 1;
                        changed = true;
                    }else{
                        start++;
                    }
                }
                indx = line.indexOf( search, start );
            }
        }
        return line;
    }
}
