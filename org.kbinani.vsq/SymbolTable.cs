/*
 * SymbolTable.cs
 * Copyright (C) 2008-2010 kbinani
 *
 * This file is part of org.kbinani.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.vsq;

import java.util.*;
import java.io.*;
import org.kbinani.*;
#else
using System;
using org.kbinani.java.io;
using org.kbinani.java.util;

namespace org.kbinani.vsq {
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

    /// <summary>
    /// 歌詞から発音記号列を引き当てるための辞書を表現するクラス
    /// </summary>
#if JAVA
    public class SymbolTable implements Cloneable {
#else
    public class SymbolTable : ICloneable {
#endif
        /// <summary>
        /// 辞書本体
        /// </summary>
        private TreeMap<String, SymbolTableEntry> m_dict;
        /// <summary>
        /// 辞書の名前
        /// </summary>
        private String m_name;
        /// <summary>
        /// 辞書を有効とするかどうか
        /// </summary>
        private boolean m_enabled;
        /// <summary>
        /// 英単語の分節分割などにより，この辞書を使うことによって最大いくつの発音記号列に分割されるか
        /// </summary>
        private int m_max_divisions = 1;

        #region static field
        /// <summary>
        /// 辞書のリスト（TreeMapのkeyは、辞書の優先順位）
        /// </summary>
        private static TreeMap<Integer, SymbolTable> s_table = new TreeMap<Integer, SymbolTable>();
        /// <summary>
        /// VOCALOID2のシステム辞書を読み込んだかどうか
        /// </summary>
        private static boolean s_initialized = false;
        #endregion

        #region Static Method and Property
        /// <summary>
        /// 英単語の分節分割などにより，登録されている辞書を使うことによって最大いくつの発音記号列に分割されるか
        /// </summary>
        /// <returns></returns>
        public static int getMaxDivisions() {
            int max = 1;
            for ( Iterator<Integer> itr = s_table.keySet().iterator(); itr.hasNext(); ) {
                int key = itr.next();
                SymbolTable table = s_table.get( key );
                max = Math.Max( max, table.m_max_divisions );
            }
            return max;
        }

        /// <summary>
        /// 指定した優先順位の辞書本体を取得します
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static SymbolTable getSymbolTable( int index ) {
            if ( !s_initialized ) {
                loadSystemDictionaries();
            }
            if ( 0 <= index && index < s_table.size() ) {
                return s_table.get( index );
            } else {
                return null;
            }
        }

        /// <summary>
        /// 指定した辞書ファイルを読み込みます。
        /// </summary>
        /// <param name="dictionary_file"></param>
        /// <param name="name"></param>
        public static void loadDictionary( String dictionary_file, String name ) {
            SymbolTable table = new SymbolTable( dictionary_file, false, true );
            table.m_name = name;
            int count = s_table.size();
            s_table.put( count, table );
        }

        /// <summary>
        /// VOCALOID2システムが使用する辞書を読み込みます。
        /// </summary>
        public static void loadSystemDictionaries() {
            if ( s_initialized ) {
                return;
            }
            int count = s_table.size();
            // 辞書フォルダからの読込み
            String editor_path = VocaloSysUtil.getEditorPath( SynthesizerType.VOCALOID2 );
            if ( editor_path != "" ) {
                String path = PortUtil.combinePath( PortUtil.getDirectoryName( editor_path ), "UDIC" );
                if ( !PortUtil.isDirectoryExists( path ) ) {
                    return;
                }
                String[] files = PortUtil.listFiles( path, "*.udc" );
                for ( int i = 0; i < files.Length; i++ ) {
                    files[i] = PortUtil.getFileName( files[i] );
#if DEBUG
                    PortUtil.println( "    files[i]=" + files[i] );
#endif
                    String dict = PortUtil.combinePath( path, files[i] );
                    s_table.put( count, new SymbolTable( dict, true, false ) );
                    count++;
                }
            }
            s_initialized = true;
        }

        /// <summary>
        /// 指定したディレクトリにある拡張辞書ファイル(拡張子*.eudc)を全て読み込みます
        /// </summary>
        /// <param name="directory"></param>
        public static void loadAllDictionaries( String directory ) {
            // 起動ディレクトリ
            int count = s_table.size();
            if ( PortUtil.isDirectoryExists( directory ) ) {
                String[] files2 = PortUtil.listFiles( directory, "*.eudc" );
                for ( int i = 0; i < files2.Length; i++ ) {
                    files2[i] = PortUtil.getFileName( files2[i] );
                    String dict = PortUtil.combinePath( directory, files2[i] );
                    s_table.put( count, new SymbolTable( dict, false, false ) );
                    count++;
                }
            }
        }

        /// <summary>
        /// 指定した歌詞から、発音記号列を引き当てます
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public static SymbolTableEntry attatch( String phrase ) {
#if DEBUG
            PortUtil.println( "SymbolTable.Attatch" );
            PortUtil.println( "    phrase=" + phrase );
#endif
            for ( Iterator<Integer> itr = s_table.keySet().iterator(); itr.hasNext(); ) {
                int key = itr.next();
                SymbolTable table = s_table.get( key );
                if ( table.isEnabled() ) {
                    SymbolTableEntry ret = table.attatchImp( phrase );
                    if ( ret != null ) {
                        return ret;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 登録されている辞書の個数を取得します
        /// </summary>
        /// <returns></returns>
        public static int getCount() {
            if ( !s_initialized ) {
                loadSystemDictionaries();
            }
            return s_table.size();
        }

        /// <summary>
        /// 辞書の優先順位と有効・無効を一括設定します
        /// </summary>
        /// <param name="list">辞書の名前・有効かどうかを表したValuePairを、辞書の優先順位の順番に格納したリスト</param>
        public static void changeOrder( Vector<ValuePair<String, Boolean>> list ) {
#if DEBUG
            PortUtil.println( "SymbolTable#changeOrder" );
#endif
            TreeMap<Integer, SymbolTable> buff = new TreeMap<Integer, SymbolTable>();
            for ( Iterator<Integer> itr = s_table.keySet().iterator(); itr.hasNext(); ) {
                int key = itr.next();
                buff.put( key, (SymbolTable)s_table.get( key ).clone() );
            }
            s_table.clear();
            int count = list.size();
            for ( int i = 0; i < count; i++ ) {
                ValuePair<String, Boolean> itemi = list.get( i );
#if DEBUG
                PortUtil.println( "SymbolTable#changeOrder; list[" + i + "]=" + itemi.getKey() + "," + itemi.getValue() );
#endif
                for ( Iterator<Integer> itr = buff.keySet().iterator(); itr.hasNext(); ) {
                    int key = itr.next();
                    SymbolTable table = buff.get( key );
                    if ( table.getName().Equals( itemi.getKey() ) ) {
                        table.setEnabled( itemi.getValue() );
                        s_table.put( i, table );
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
        public object Clone() {
            return clone();
        }
#endif

        /// <summary>
        /// この辞書のディープ・コピーを取得します
        /// </summary>
        /// <returns></returns>
        public Object clone() {
            SymbolTable ret = new SymbolTable();
            ret.m_dict = new TreeMap<String, SymbolTableEntry>();
            for ( Iterator<String> itr = m_dict.keySet().iterator(); itr.hasNext(); ) {
                String key = itr.next();
                ret.m_dict.put( key, (SymbolTableEntry)m_dict.get( key ).clone() );
            }
            ret.m_name = m_name;
            ret.m_enabled = m_enabled;
            return ret;
        }

        /// <summary>
        /// 使ってはいけないコンストラクタ
        /// </summary>
        private SymbolTable() {
        }

        /// <summary>
        /// 辞書の名前を取得します
        /// </summary>
        /// <returns></returns>
        public String getName() {
            return m_name;
        }

        /// <summary>
        /// 辞書が有効かどうかを取得します
        /// </summary>
        /// <returns></returns>
        public boolean isEnabled() {
            return m_enabled;
        }

        /// <summary>
        /// 辞書が有効かどうかを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setEnabled( boolean value ) {
            m_enabled = value;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path">読み込む辞書ファイルのパス</param>
        /// <param name="is_udc_mode">VOCALOID2仕様の辞書ファイルかどうか</param>
        /// <param name="enabled">辞書ファイルを有効とするかどうか</param>
        public SymbolTable( String path, boolean is_udc_mode, boolean enabled ) {
            m_dict = new TreeMap<String, SymbolTableEntry>();
            m_enabled = enabled;
            if ( !PortUtil.isFileExists( path ) ) {
                return;
            }
            m_name = PortUtil.getFileName( path );
            BufferedReader sr = null;
            try {
                if ( is_udc_mode ) {
                    sr = new BufferedReader( new InputStreamReader( new FileInputStream( path ), "Shift_JIS" ) );
                    if ( sr == null ) {
                        return;
                    }
                } else {
                    sr = new BufferedReader( new InputStreamReader( new FileInputStream( path ), "UTF-8" ) );
                    if ( sr == null ) {
                        return;
                    }
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
                            m_max_divisions = Math.Max( spl_word.Length, m_max_divisions );
                            key = spl[0].Replace( "-\t", "" );
                            word = spl[0];
                            symbol = spl[1];
                        }
                    }
                    if ( !key.Equals( "" ) ) {
                        if ( !m_dict.containsKey( key ) ) {
                            m_dict.put( key, new SymbolTableEntry( word, symbol ) );
                        }
                    }
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "SymbolTable#.ctor; ex=" + ex );
            } finally {
                if ( sr != null ) {
                    try {
                        sr.close();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "SymbolTable#.ctor; ex=" + ex2 );
                    }
                }
            }
#if DEBUG
            PortUtil.println( "SymbolTable#.ctor; m_max_divisions=" + m_max_divisions );
#endif
        }

        /// <summary>
        /// 指定した文字列から、発音記号列を引き当てます
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        private SymbolTableEntry attatchImp( String phrase ) {
            String s = phrase.ToLower();
            if ( m_dict.containsKey( s ) ) {
                return m_dict.get( s );
                /*result.value = m_dict.get( s ).Symbol.Replace( '\t', ' ' );
                return true;*/
            } else {
                return null;
            }
        }
    }

#if !JAVA
}
#endif
