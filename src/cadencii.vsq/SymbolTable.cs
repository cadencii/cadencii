/*
 * SymbolTable.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package cadencii.vsq;

import java.util.*;
import java.io.*;
import cadencii.*;
#else
using System;
using System.Collections.Generic;
using System.IO;
using cadencii.java.io;
using cadencii.java.util;

    namespace cadencii
    {
        namespace vsq
        {
            using boolean = System.Boolean;
#endif

            /// <summary>
            /// 歌詞から発音記号列を引き当てるための辞書を表現するクラス
            /// </summary>
#if JAVA
            public class SymbolTable implements Cloneable
#else
            public class SymbolTable : ICloneable
#endif
            {
                /// <summary>
                /// 辞書本体
                /// </summary>
                private TreeMap<String, SymbolTableEntry> mDict;
                /// <summary>
                /// 辞書の名前
                /// </summary>
                private String mName;
                /// <summary>
                /// 辞書を有効とするかどうか
                /// </summary>
                private boolean mEnabled;
                /// <summary>
                /// 英単語の分節分割などにより，この辞書を使うことによって最大いくつの発音記号列に分割されるか
                /// </summary>
                private int mMaxDivisions = 1;

                #region static field
                /// <summary>
                /// 辞書のリスト，辞書の優先順位の順番で格納
                /// </summary>
                private static Vector<SymbolTable> mTable = new Vector<SymbolTable>();
                /// <summary>
                /// VOCALOID2のシステム辞書を読み込んだかどうか
                /// </summary>
                private static boolean mInitialized = false;
                #endregion

                #region Static Method and Property
                /// <summary>
                /// 英単語の分節分割などにより，登録されている辞書を使うことによって最大いくつの発音記号列に分割されるか
                /// </summary>
                /// <returns></returns>
                public static int getMaxDivisions()
                {
                    int max = 1;
                    int size = mTable.size();
                    for ( int i = 0; i < size; i++ ) {
                        SymbolTable table = mTable.get( i );
                        max = Math.Max( max, table.mMaxDivisions );
                    }
                    return max;
                }

                /// <summary>
                /// 指定した優先順位の辞書本体を取得します
                /// </summary>
                /// <param name="index"></param>
                /// <returns></returns>
                public static SymbolTable getSymbolTable( int index )
                {
                    if ( !mInitialized ) {
                        loadSystemDictionaries();
                    }
                    if ( 0 <= index && index < mTable.size() ) {
                        return mTable.get( index );
                    } else {
                        return null;
                    }
                }

                /// <summary>
                /// 指定した辞書ファイルを読み込みます。
                /// </summary>
                /// <param name="dictionary_file"></param>
                /// <param name="name"></param>
                public static void loadDictionary( String dictionary_file, String name )
                {
                    SymbolTable table = new SymbolTable( dictionary_file, false, true, "UTF-8" );
                    table.mName = name;
                    mTable.add( table );
                }

                /// <summary>
                /// VOCALOID2システムが使用する辞書を読み込みます。
                /// </summary>
                public static void loadSystemDictionaries()
                {
                    if ( mInitialized ) {
                        return;
                    }
                    // 辞書フォルダからの読込み
                    String editor_path = VocaloSysUtil.getEditorPath( SynthesizerType.VOCALOID2 );
                    if ( editor_path != "" ) {
                        String path = Path.Combine( PortUtil.getDirectoryName( editor_path ), "UDIC" );
                        if (!Directory.Exists(path)) {
                            return;
                        }
                        String[] files = PortUtil.listFiles( path, "*.udc" );
                        for ( int i = 0; i < files.Length; i++ ) {
                            files[i] = PortUtil.getFileName( files[i] );
                            String dict = Path.Combine( path, files[i] );
                            mTable.add( new SymbolTable( dict, true, false, "Shift_JIS" ) );
                        }
                    }
                    mInitialized = true;
                }

                /// <summary>
                /// 指定したディレクトリにある拡張辞書ファイル(拡張子*.eudc)を全て読み込みます
                /// </summary>
                /// <param name="directory"></param>
                public static void loadAllDictionaries( String directory )
                {
                    // 起動ディレクトリ
                    if (Directory.Exists(directory)) {
                        String[] files2 = PortUtil.listFiles( directory, "*.eudc" );
                        for ( int i = 0; i < files2.Length; i++ ) {
                            files2[i] = PortUtil.getFileName( files2[i] );
                            String dict = Path.Combine( directory, files2[i] );
                            mTable.add( new SymbolTable( dict, true, false, "UTF-8" ) );
                        }
                    }
                }

                /// <summary>
                /// 指定した歌詞から、発音記号列を引き当てます
                /// </summary>
                /// <param name="phrase"></param>
                /// <returns></returns>
                public static SymbolTableEntry attatch( String phrase )
                {
                    int size = mTable.size();
                    for ( int i = 0; i < size; i++ ) {
                        SymbolTable table = mTable.get( i );
                        if ( table.isEnabled() ) {
                            SymbolTableEntry ret = table.attatchImp( phrase );
                            if ( ret != null ) {
                                return ret;
                            }
                        }
                    }
                    return null;
                }

                [Obsolete]
                public static bool attatch( String phrase, out String result )
                {
                    var entry = attatch( phrase );
                    if ( entry == null ) {
                        result = "a";
                        return false;
                    } else {
                        result = entry.getSymbol();
                        return true;
                    }
                }

                [Obsolete]
                public static bool attatch( string phrase, ByRef<string> result )
                {
                    var entry = attatch( phrase );
                    if ( entry == null ) {
                        result.value = "a";
                        return false;
                    } else {
                        result.value = entry.getSymbol();
                        return true;
                    }
                }

                /// <summary>
                /// 登録されている辞書の個数を取得します
                /// </summary>
                /// <returns></returns>
                public static int getCount()
                {
                    if ( !mInitialized ) {
                        loadSystemDictionaries();
                    }
                    return mTable.size();
                }

                /// <summary>
                /// 辞書の優先順位と有効・無効を一括設定します
                /// </summary>
                /// <param name="list">辞書の名前・有効かどうかを表したValuePairを、辞書の優先順位の順番に格納したリスト</param>
                public static void changeOrder( Vector<ValuePair<String, Boolean>> list )
                {
                    // 現在の辞書をバッファに退避
                    Vector<SymbolTable> buff = new Vector<SymbolTable>();
                    int size = mTable.size();
                    for ( int i = 0; i < size; i++ ) {
                        buff.add( mTable.get( i ) );
                    }

                    // 現在の辞書をいったんクリア
                    mTable.Clear();

                    int count = list.size();
                    for ( int i = 0; i < count; i++ ) {
                        ValuePair<String, Boolean> itemi = list.get( i );
                        for ( int j = 0; j < size; j++ ) {
                            SymbolTable table = buff.get( j );
                            if ( table.getName().Equals( itemi.getKey() ) ) {
                                table.setEnabled( itemi.getValue() );
                                mTable.add( table );
                                break;
                            }
                        }
                    }
                }
                #endregion

                /// <summary>
                /// この辞書のディープ・コピーを取得します
                /// </summary>
                /// <returns></returns>
#if !JAVA
                public object Clone()
                {
                    return clone();
                }
#endif

                /// <summary>
                /// この辞書のディープ・コピーを取得します
                /// </summary>
                /// <returns></returns>
                public Object clone()
                {
                    SymbolTable ret = new SymbolTable();
                    ret.mDict = new TreeMap<String, SymbolTableEntry>();
                    for ( Iterator<String> itr = mDict.keySet().iterator(); itr.hasNext(); ) {
                        String key = itr.next();
                        ret.mDict.put( key, (SymbolTableEntry)mDict.get( key ).clone() );
                    }
                    ret.mName = mName;
                    ret.mEnabled = mEnabled;
                    ret.mMaxDivisions = mMaxDivisions;
                    return ret;
                }

                /// <summary>
                /// 使ってはいけないコンストラクタ
                /// </summary>
                private SymbolTable()
                {
                }

                /// <summary>
                /// 辞書の名前を取得します
                /// </summary>
                /// <returns></returns>
                public String getName()
                {
                    return mName;
                }

                /// <summary>
                /// 辞書が有効かどうかを取得します
                /// </summary>
                /// <returns></returns>
                public boolean isEnabled()
                {
                    return mEnabled;
                }

                /// <summary>
                /// 辞書が有効かどうかを設定します
                /// </summary>
                /// <param name="value"></param>
                public void setEnabled( boolean value )
                {
                    mEnabled = value;
                }

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="path">読み込む辞書ファイルのパス</param>
                /// <param name="is_udc_mode">VOCALOID2仕様の辞書ファイルかどうか</param>
                /// <param name="enabled">辞書ファイルを有効とするかどうか</param>
                /// <param name="encoding">辞書ファイルのテキストエンコーディング</param>
                public SymbolTable( String path, boolean is_udc_mode, boolean enabled, String encoding )
                {
                    mDict = new TreeMap<String, SymbolTableEntry>();
                    mEnabled = enabled;
                    if (!System.IO.File.Exists(path)) {
                        return;
                    }
                    mName = PortUtil.getFileName( path );
                    BufferedReader sr = null;
                    try {
                        sr = new BufferedReader( new InputStreamReader( new FileInputStream( path ), encoding ) );
                        if ( sr == null ) {
                            return;
                        }
                        String line;
                        while ( sr.ready() ) {
                            line = sr.readLine();
                            if ( line.StartsWith( "//" ) ) {
                                continue;
                            }
                            String key = "";
                            String word = "";
                            String symbol = "";
                            if ( is_udc_mode ) {
                                String[] spl = PortUtil.splitString( line, new String[] { "\t" }, 2, true );
                                if ( spl.Length >= 2 ) {
                                    key = spl[0].ToLower();
                                    word = key;
                                    symbol = spl[1];
                                }
                            } else {
                                String[] spl = PortUtil.splitString( line, new String[] { "\t\t" }, 2, true );
                                if ( spl.Length >= 2 ) {
                                    String[] spl_word = PortUtil.splitString( spl[0], '\t' );
                                    mMaxDivisions = Math.Max( spl_word.Length, mMaxDivisions );
                                    key = spl[0].Replace( "-\t", "" );
                                    word = spl[0];
                                    symbol = spl[1];
                                }
                            }
                            if ( !key.Equals( "" ) ) {
                                if ( !mDict.containsKey( key ) ) {
                                    mDict.put( key, new SymbolTableEntry( word, symbol ) );
                                }
                            }
                        }
                    } catch ( Exception ex ) {
                        serr.println( "SymbolTable#.ctor; ex=" + ex );
                    } finally {
                        if ( sr != null ) {
                            try {
                                sr.close();
                            } catch ( Exception ex2 ) {
                                serr.println( "SymbolTable#.ctor; ex=" + ex2 );
                            }
                        }
                    }
                }

                /// <summary>
                /// 指定した文字列から、発音記号列を引き当てます
                /// </summary>
                /// <param name="phrase"></param>
                /// <returns></returns>
                private SymbolTableEntry attatchImp( String phrase )
                {
                    String s = phrase.ToLower();
                    if ( mDict.containsKey( s ) ) {
                        return mDict.get( s );
                    } else {
                        return null;
                    }
                }
            }

#if !JAVA
        }
    }
#endif
