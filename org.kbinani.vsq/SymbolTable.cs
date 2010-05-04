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

namespace org.kbinani.vsq.impl {
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

#if JAVA
    public class SymbolTable implements Cloneable {
#else
    public class SymbolTable : ICloneable {
#endif
        private TreeMap<String, SymbolTableEntry> m_dict;
        private String m_name;
        private boolean m_enabled;

        #region Static Field
        private static TreeMap<Integer, SymbolTable> s_table = new TreeMap<Integer, SymbolTable>();
        private static SymbolTable s_default_jp = null;
        private static SymbolTable s_default_en = null;
        private static boolean s_initialized = false;
        #endregion

        #region Static Method and Property
        public static SymbolTable getSymbolTable( int index ) {
            if ( !s_initialized ) {
                loadDictionary();
            }
            if ( 0 <= index && index < s_table.size() ) {
                return s_table.get( index );
            } else {
                return null;
            }
        }

        public static void loadDictionary() {
#if DEBUG
            PortUtil.println( "SymbolTable.LoadDictionary()" );
#endif
            Vector<String[]> listja = new Vector<String[]>();
            BufferedReader brja = null;
            try {
                String filejp = PortUtil.combinePath( PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "resources" ), "dict_ja.txt" );
                brja = new BufferedReader( new InputStreamReader( new FileInputStream( filejp ), "UTF-8" ) );
                String line = "";
                while ( (line = brja.readLine()) != null ) {
                    String[] spl = PortUtil.splitString( line, new String[]{ "\t\t" }, false );
                    if ( spl.Length < 2 ) {
                        continue;
                    }
                    listja.add( new String[] { spl[0], spl[1] } );
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "SymbolTable#loadDictionary; ex=" + ex );
            } finally {
                if ( brja != null ) {
                    try {
                        brja.close();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "SymbolTable#loadDictionary; ex2=" + ex2 );
                    }
                }
            }
            s_default_jp = new SymbolTable( "DEFAULT_JP", listja.toArray( new String[][] { } ), true );

            Vector<String[]> listen = new Vector<String[]>();
            BufferedReader bren = null;
            try {
                String fileen = PortUtil.combinePath( PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "resources" ), "dict_en.txt" );
                bren = new BufferedReader( new InputStreamReader( new FileInputStream( fileen ), "UTF-8" ) );
                String line = "";
                while ( (line = bren.readLine()) != null ) {
                    String[] spl = PortUtil.splitString( line, '\t' );
                    if ( spl.Length < 2 ) {
                        continue;
                    }
                    listen.add( new String[] { spl[0], spl[1] } );
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "SymbolTable#loadDictionary; ex=" + ex );
            } finally {
                if ( bren != null ) {
                    try {
                        bren.close();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "SymbolTable#loadDictionary; ex2=" + ex2 );
                    }
                }
            }
            s_default_en = new SymbolTable( "DEFAULT_EN", listen.toArray( new String[][] { } ), true );

            s_table.clear();
            int count = 0;
            s_table.put( count, s_default_en );
            count++;
            s_table.put( count, s_default_jp );
            count++;

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

            // 起動ディレクトリ
            String path2 = PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "udic" );
            if ( PortUtil.isDirectoryExists( path2 ) ) {
                String[] files2 = PortUtil.listFiles( path2, "*.eudc" );
                for ( int i = 0; i < files2.Length; i++ ) {
                    files2[i] = PortUtil.getFileName( files2[i] );
#if DEBUG
                    PortUtil.println( "    files2[i]=" + files2[i] );
#endif
                    String dict = PortUtil.combinePath( path2, files2[i] );
                    s_table.put( count, new SymbolTable( dict, false, false ) );
                    count++;
                }
            }
            s_initialized = true;
        }

        public static boolean attatch( String phrase, ByRef<String> result ) {
#if DEBUG
            PortUtil.println( "SymbolTable.Attatch" );
            PortUtil.println( "    phrase=" + phrase );
#endif
            for ( Iterator<Integer> itr = s_table.keySet().iterator(); itr.hasNext(); ) {
                int key = itr.next();
                SymbolTable table = s_table.get( key );
                if ( table.isEnabled() ) {
                    if ( table.attatchImp( phrase, result ) ) {
                        return true;
                    }
                }
            }
            result.value = "a";
            return false;
        }

        public static int getCount() {
            if ( !s_initialized ) {
                loadDictionary();
            }
            return s_table.size();
        }

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

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

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

        private SymbolTable() {
        }

        public String getName() {
            return m_name;
        }

        public boolean isEnabled() {
            return m_enabled;
        }

        public void setEnabled( boolean value ) {
            m_enabled = value;
        }

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
                    sr = new BufferedReader( new InputStreamReader( new FileInputStream( path ), "UTF8" ) );
                    if ( sr == null ) {
                        return;
                    }
                }
                String line;
                while ( sr.ready() ) {
                    line = sr.readLine();
                    if ( !line.StartsWith( "//" ) ) {
                        String[] spl = PortUtil.splitString( line, new String[] { "\t" }, 2, true );
                        if ( spl.Length >= 2 ) {
                            String key = spl[0].ToLower();
                            if ( m_dict.containsKey( key ) ) {
                                PortUtil.println( "SymbolTable..ctor" );
                                PortUtil.println( "    dictionary already contains key: " + key );
                            } else {
                                m_dict.put( key, spl[1] );
                            }
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
        }

        private boolean attatchImp( String phrase, ByRef<String> result ) {
            String s = phrase.ToLower();
            if ( m_dict.containsKey( s ) ) {
                result.value = m_dict.get( s );
                return true;
            } else {
                result.value = "a";
                return false;
            }
        }

        private SymbolTable( String name, String[][] key, boolean enabled ) {
            m_enabled = enabled;
            m_name = name;
            m_dict = new TreeMap<String, String>();
            for( int i = 0; i < key.Length; i++ ){
                String k = key[i][0].ToLower();
                if( m_dict.containsKey( k ) ){
                    continue;
                }
                m_dict.put( k, key[i][1] );
            }
        }
    }

#if !JAVA
}
#endif

#if !JAVA
namespace org.kbinani.vsq {
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

#if JAVA
    public class SymbolTable implements Cloneable {
#else
    public class SymbolTable : ICloneable {
#endif
        private TreeMap<String, String> m_dict;
        private String m_name;
        private boolean m_enabled;

        #region Static Field
        private static TreeMap<Integer, SymbolTable> s_table = new TreeMap<Integer, SymbolTable>();
        private static SymbolTable s_default_jp = null;
        private static SymbolTable s_default_en = null;
        private static boolean s_initialized = false;
        #endregion

        #region Static Method and Property
        public static SymbolTable getSymbolTable( int index ) {
            if ( !s_initialized ) {
                loadDictionary();
            }
            if ( 0 <= index && index < s_table.size() ) {
                return s_table.get( index );
            } else {
                return null;
            }
        }

        public static void loadDictionary() {
#if DEBUG
            PortUtil.println( "SymbolTable.LoadDictionary()" );
#endif
            Vector<String[]> listja = new Vector<String[]>();
            BufferedReader brja = null;
            try {
                String filejp = PortUtil.combinePath( PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "resources" ), "dict_ja.txt" );
                brja = new BufferedReader( new InputStreamReader( new FileInputStream( filejp ), "UTF-8" ) );
                String line = "";
                while ( (line = brja.readLine()) != null ) {
                    String[] spl = PortUtil.splitString( line, '\t' );
                    if ( spl.Length < 2 ) {
                        continue;
                    }
                    listja.add( new String[] { spl[0], spl[1] } );
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "SymbolTable#loadDictionary; ex=" + ex );
            } finally {
                if ( brja != null ) {
                    try {
                        brja.close();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "SymbolTable#loadDictionary; ex2=" + ex2 );
                    }
                }
            }
            s_default_jp = new SymbolTable( "DEFAULT_JP", listja.toArray( new String[][] { } ), true );

            Vector<String[]> listen = new Vector<String[]>();
            BufferedReader bren = null;
            try {
                String fileen = PortUtil.combinePath( PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "resources" ), "dict_en.txt" );
                bren = new BufferedReader( new InputStreamReader( new FileInputStream( fileen ), "UTF-8" ) );
                String line = "";
                while ( (line = bren.readLine()) != null ) {
                    String[] spl = PortUtil.splitString( line, '\t' );
                    if ( spl.Length < 2 ) {
                        continue;
                    }
                    listen.add( new String[] { spl[0], spl[1] } );
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "SymbolTable#loadDictionary; ex=" + ex );
            } finally {
                if ( bren != null ) {
                    try {
                        bren.close();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "SymbolTable#loadDictionary; ex2=" + ex2 );
                    }
                }
            }
            s_default_en = new SymbolTable( "DEFAULT_EN", listen.toArray( new String[][] { } ), true );

            s_table.clear();
            int count = 0;
            s_table.put( count, s_default_en );
            count++;
            s_table.put( count, s_default_jp );
            count++;

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

            // 起動ディレクトリ
            String path2 = PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "udic" );
            if ( PortUtil.isDirectoryExists( path2 ) ) {
                String[] files2 = PortUtil.listFiles( path2, "*.eudc" );
                for ( int i = 0; i < files2.Length; i++ ) {
                    files2[i] = PortUtil.getFileName( files2[i] );
#if DEBUG
                    PortUtil.println( "    files2[i]=" + files2[i] );
#endif
                    String dict = PortUtil.combinePath( path2, files2[i] );
                    s_table.put( count, new SymbolTable( dict, false, false ) );
                    count++;
                }
            }
            s_initialized = true;
        }

        public static boolean attatch( String phrase, ByRef<String> result ) {
#if DEBUG
            PortUtil.println( "SymbolTable.Attatch" );
            PortUtil.println( "    phrase=" + phrase );
#endif
            for ( Iterator<Integer> itr = s_table.keySet().iterator(); itr.hasNext(); ) {
                int key = itr.next();
                SymbolTable table = s_table.get( key );
                if ( table.isEnabled() ) {
                    if ( table.attatchImp( phrase, result ) ) {
                        return true;
                    }
                }
            }
            result.value = "a";
            return false;
        }

        public static int getCount() {
            if ( !s_initialized ) {
                loadDictionary();
            }
            return s_table.size();
        }

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

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

        public Object clone() {
            SymbolTable ret = new SymbolTable();
            ret.m_dict = new TreeMap<String, String>();
            for ( Iterator<String> itr = m_dict.keySet().iterator(); itr.hasNext(); ) {
                String key = itr.next();
                ret.m_dict.put( key, m_dict.get( key ) );
            }
            ret.m_name = m_name;
            ret.m_enabled = m_enabled;
            return ret;
        }

        private SymbolTable() {
        }

        public String getName() {
            return m_name;
        }

        public boolean isEnabled() {
            return m_enabled;
        }

        public void setEnabled( boolean value ) {
            m_enabled = value;
        }

        public SymbolTable( String path, boolean is_udc_mode, boolean enabled ) {
            m_dict = new TreeMap<String, String>();
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
                    sr = new BufferedReader( new InputStreamReader( new FileInputStream( path ), "UTF8" ) );
                    if ( sr == null ) {
                        return;
                    }
                }
                String line;
                while ( sr.ready() ) {
                    line = sr.readLine();
                    if ( !line.StartsWith( "//" ) ) {
                        String[] spl = PortUtil.splitString( line, new String[] { "\t" }, 2, true );
                        if ( spl.Length >= 2 ) {
                            String key = spl[0].ToLower();
                            if ( m_dict.containsKey( key ) ) {
                                PortUtil.println( "SymbolTable..ctor" );
                                PortUtil.println( "    dictionary already contains key: " + key );
                            } else {
                                m_dict.put( key, spl[1] );
                            }
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
        }

        private boolean attatchImp( String phrase, ByRef<String> result ) {
            String s = phrase.ToLower();
            if ( m_dict.containsKey( s ) ) {
                result.value = m_dict.get( s );
                return true;
            } else {
                result.value = "a";
                return false;
            }
        }

        private SymbolTable( String name, String[][] key, boolean enabled ) {
            m_enabled = enabled;
            m_name = name;
            m_dict = new TreeMap<String, String>();
            for( int i = 0; i < key.Length; i++ ){
                String k = key[i][0].ToLower();
                if( m_dict.containsKey( k ) ){
                    continue;
                }
                m_dict.put( k, key[i][1] );
            }
        }
    }

#if !JAVA
}
#endif
