/*
 * SymbolTable.cs
 * Copyright (c) 2008-2009 kbinani
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
using System.Text;
using System.Windows.Forms;
using bocoree;
using bocoree.java.util;
using bocoree.java.io;

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
        private static boolean s_initialized = false;
#if JAVA
        public static final String[][] _KEY_JP = {
#else
        public static readonly String[,] _KEY_JP = {
#endif
            {"あ", "a"},
            {"い", "i"},
            {"う", "M"},
            {"え", "e"},
            {"お", "o"},
            {"か", "k a"},
            {"き", "k' i"},
            {"く", "k M"},
            {"け", "k e"},
            {"こ", "k o"},
            {"さ", "s a"},
            {"し", "S i"},
            {"す", "s M"},
            {"せ", "s e"},
            {"そ", "s o"},
            {"た", "t a"},
            {"ち", "tS i"},
            {"つ", "ts M"},
            {"て", "t e"},
            {"と", "t o"},
            {"な", "n a"},
            {"に", "J i"},
            {"ぬ", "n M"},
            {"ね", "n e"},
            {"の", "n o"},
            {"は", "h a"},
            {"ひ", "C i"},
            {"ふ", "p\\ M"},
            {"へ", "h e"},
            {"ほ", "h o"},
            {"ま", "m a"},
            {"み", "m' i"},
            {"む", "m M"},
            {"め", "m e"},
            {"も", "m o"},
            {"や", "j a"},
            {"ゆ", "j M"},
            {"よ", "j o"},
            {"ら", "4 a"},
            {"り", "4' i"},
            {"る", "4 M"},
            {"れ", "4 e "},
            {"ろ", "4 o"},
            {"わ", "w a"},
            {"ゐ", "w i"},
            {"ゑ", "w e"},
            {"を", "o"},
            {"ぁ", "a"},
            {"ぃ", "i"},
            {"ぅ", "M"},
            {"ぇ", "e"},
            {"ぉ", "o"},
            {"が", "g a"},
            {"ぎ", "g' i"},
            {"ぐ", "g M"},
            {"げ", "g e"},
            {"ご", "g o"},
            {"ざ", "dz a"},
            {"じ", "dZ i"},
            {"ず", "dz M"},
            {"ぜ", "dz e"},
            {"ぞ", "dz o"},
            {"だ", "d a"},
            {"ぢ", "dZ i"},
            {"づ", "dz M"},
            {"で", "d e"},
            {"ど", "d o"},
            {"ば", "b a"},
            {"び", "b' i"},
            {"ぶ", "b M"},
            {"べ", "b e"},
            {"ぼ", "b o"},
            {"ぱ", "p a"},
            {"ぴ", "p' i"},
            {"ぷ", "p M"},
            {"ぺ", "p e"},
            {"ぽ", "p o"},
            {"いぇ", "j e"},
            {"うぃ", "w i"},
            {"うぇ", "w e"},
            {"うぉ", "w o"},
            {"きゃ", "k' a"},
            {"きゅ", "k' M"},
            {"きぇ", "k' e"},
            {"きょ", "k' o"},
            {"しゃ", "S a"},
            {"しゅ", "S M"},
            {"しぇ", "S e"},
            {"しょ", "S o"},
            {"ちゃ", "tS a"},
            {"ちゅ", "tS M"},
            {"ちぇ", "tS e"},
            {"ちょ", "tS o"},
            {"にゃ", "J a"},
            {"にゅ", "J M"},
            {"にぇ", "J e"},
            {"にょ", "J o"},
            {"ひゃ", "C a"},
            {"ひゅ", "C M"},
            {"ひぇ", "C e"},
            {"ひょ", "C o"},
            {"ふゃ", "p\\' a"},
            {"ふぃ", "p\\' i"},
            {"ふゅ", "p\\' M"},
            {"ふぇ", "p\\ e"},
            {"みゃ", "m' a"},
            {"みゅ", "m' M"},
            {"みぇ", "m' e"},
            {"みょ", "m' o"},
            {"りゃ", "4' a"},
            {"りゅ", "4' M"},
            {"りぇ", "4' e"},
            {"りょ", "4' o"},
            {"ぎゃ", "g' a"},
            {"ぎゅ", "g' M"},
            {"ぎぇ", "g' e"},
            {"ぎょ", "g' o"},
            {"じゃ", "dZ a"},
            {"じゅ", "dZ M"},
            {"じぇ", "dZ e"},
            {"じょ", "dZ o"},
            {"びゃ", "b' a"},
            {"びゅ", "b' M"},
            {"びぇ", "b' e"},
            {"びょ", "b' o"},
            {"ぴゃ", "p' a"},
            {"ぴゅ", "p' M"},
            {"ぴぇ", "p' e"},
            {"ぴょ", "p' o"},
            {"ふぁ", "p\\ a"},
            {"ふぉ", "p\\ o"},
            {"てゃ", "t' a"},
            {"てぃ", "t' i"},
            {"てゅ", "t' M"},
            {"てぇ", "t' e"},
            {"てょ", "t' o"},
            {"でゃ", "d' a"},
            {"でぃ", "d' i"},
            {"でゅ", "d' M"},
            {"でぇ", "d' e"},
            {"でょ", "d' o"},
            {"すぃ", "s i"},
            {"ずぃ", "dz i"},
            {"とぅ", "t M"},
            {"どぅ", "d M"},
            {"ゃ", "j a"},
            {"ゅ", "j M"},
            {"ょ", "j o"},
            {"ん", "n"},
            {"ア", "a"},
            {"イ", "i"},
            {"ウ", "M"},
            {"エ", "e"},
            {"オ", "o"},
            {"カ", "k a"},
            {"キ", "k' i"},
            {"ク", "k M"},
            {"ケ", "k e"},
            {"コ", "k o"},
            {"サ", "s a"},
            {"シ", "S i"},
            {"ス", "s M"},
            {"セ", "s e"},
            {"ソ", "s o"},
            {"タ", "t a"},
            {"チ", "tS i"},
            {"ツ", "ts M"},
            {"テ", "t e"},
            {"ト", "t o"},
            {"ナ", "n a"},
            {"ニ", "J i"},
            {"ヌ", "n M"},
            {"ネ", "n e"},
            {"ノ", "n o"},
            {"ハ", "h a"},
            {"ヒ", "C i"},
            {"フ", "p\\ M"},
            {"ヘ", "h e"},
            {"ホ", "h o"},
            {"マ", "m a"},
            {"ミ", "m' i"},
            {"ム", "m M"},
            {"メ", "m e"},
            {"モ", "m o"},
            {"ヤ", "j a"},
            {"ユ", "j M"},
            {"ヨ", "j o"},
            {"ラ", "4 a"},
            {"リ", "4' i"},
            {"ル", "4 M"},
            {"レ", "4 e "},
            {"ロ", "4 o"},
            {"ワ", "w a"},
            {"ヰ", "w i"},
            {"ヱ", "w e"},
            {"ヲ", "o"},
            {"ァ", "a"},
            {"ィ", "i"},
            {"ゥ", "M"},
            {"ェ", "e"},
            {"ォ", "o"},
            {"ガ", "g a"},
            {"ギ", "g' i"},
            {"グ", "g M"},
            {"ゲ", "g e"},
            {"ゴ", "g o"},
            {"ザ", "dz a"},
            {"ジ", "dZ i"},
            {"ズ", "dz M"},
            {"ゼ", "dz e"},
            {"ゾ", "dz o"},
            {"ダ", "d a"},
            {"ヂ", "dZ i"},
            {"ヅ", "dz M"},
            {"デ", "d e"},
            {"ド", "d o"},
            {"バ", "b a"},
            {"ビ", "b' i"},
            {"ブ", "b M"},
            {"ベ", "b e"},
            {"ボ", "b o"},
            {"パ", "p a"},
            {"ピ", "p' i"},
            {"プ", "p M"},
            {"ペ", "p e"},
            {"ポ", "p o"},
            {"イェ", "j e"},
            {"ウィ", "w i"},
            {"ウェ", "w e"},
            {"ウォ", "w o"},
            {"キャ", "k' a"},
            {"キュ", "k' M"},
            {"キェ", "k' e"},
            {"キョ", "k' o"},
            {"シャ", "S a"},
            {"シュ", "S M"},
            {"シェ", "S e"},
            {"ショ", "S o"},
            {"チャ", "tS a"},
            {"チュ", "tS M"},
            {"チェ", "tS e"},
            {"チョ", "tS o"},
            {"ニャ", "J a"},
            {"ニュ", "J M"},
            {"ニェ", "J e"},
            {"ニョ", "J o"},
            {"ヒャ", "C a"},
            {"ヒュ", "C M"},
            {"ヒェ", "C e"},
            {"ヒョ", "C o"},
            {"フャ", "p\\' a"},
            {"フィ", "p\\' i"},
            {"フュ", "p\\' M"},
            {"フェ", "p\\ e"},
            {"ミャ", "m' a"},
            {"ミュ", "m' M"},
            {"ミェ", "m' e"},
            {"ミョ", "m' o"},
            {"リャ", "4' a"},
            {"リュ", "4' M"},
            {"リェ", "4' e"},
            {"リョ", "4' o"},
            {"ギャ", "g' a"},
            {"ギュ", "g' M"},
            {"ギェ", "g' e"},
            {"ギョ", "g' o"},
            {"ジャ", "dZ a"},
            {"ジュ", "dZ M"},
            {"ジェ", "dZ e"},
            {"ジョ", "dZ o"},
            {"ビャ", "b' a"},
            {"ビュ", "b' M"},
            {"ビェ", "b' e"},
            {"ビョ", "b' o"},
            {"ピャ", "p' a"},
            {"ピュ", "p' M"},
            {"ピェ", "p' e"},
            {"ピョ", "p' o"},
            {"ファ", "p\\ a"},
            {"フォ", "p\\ o"},
            {"テャ", "t' a"},
            {"ティ", "t' i"},
            {"テュ", "t' M"},
            {"テェ", "t' e"},
            {"テョ", "t' o"},
            {"デャ", "d' a"},
            {"ディ", "d' i"},
            {"デュ", "d' M"},
            {"デェ", "d' e"},
            {"デョ", "d' o"},
            {"スィ", "s i"},
            {"ズィ", "dz i"},
            {"トゥ", "t M"},
            {"ドゥ", "d M"},
            {"ャ", "j a"},
            {"ュ", "j M"},
            {"ョ", "j o"},
            {"ン", "n"},
            {"ヴ", "b M"},
            {"a", "a"},
            {"e", "e"},
            {"i", "i"},
            {"m", "n"},
            {"n", "n"},
            {"o", "o"},
            {"u", "M"},
            {"A", "a"},
            {"E", "e"},
            {"I", "i"},
            {"M", "n"},
            {"N", "n"},
            {"O", "o"},
            {"U", "M"},
            {"ka", "k a"},
            {"ki", "k' i"},
            {"ku", "k M"},
            {"ke", "k e"},
            {"ko", "k o"},
            {"kya", "k' a"},
            {"kyu", "k' M"},
            {"kyo", "k' o"},
            {"sa", "s a"},
            {"si", "s i"},
            {"su", "s M"},
            {"se", "s e"},
            {"so", "s o"},
            {"ta", "t a"},
            {"ti", "t' i"},
            {"tu", "t M"},
            {"te", "t e"},
            {"to", "t o"},
            {"tya", "t' a"},
            {"tyu", "t' M"},
            {"tyo", "t' o"},
            {"na", "n a"},
            {"ni", "J i"},
            {"nu", "n M"},
            {"ne", "n e"},
            {"no", "n o"},
            {"nya", "J a"},
            {"nyu", "J M"},
            {"nyo", "J o"},
            {"ha", "h a"},
            {"hi", "C i"},
            {"he", "h e"},
            {"ho", "h o"},
            {"hya", "C a"},
            {"hyu", "C M"},
            {"hyo", "C o"},
            {"ma", "m a"},
            {"mi", "m' i"},
            {"mu", "m M"},
            {"me", "m e"},
            {"mo", "m o"},
            {"mya", "m' a"},
            {"myu", "m' M"},
            {"myo", "m' o"},
            {"ya", "j a"},
            {"yu", "j M"},
            {"ye", "j e"},
            {"yo", "j o"},
            {"ra", "4 a"},
            {"ri", "4' i"},
            {"ru", "4 M"},
            {"re", "4 e"},
            {"ro", "4 o"},
            {"rya", "4' a"},
            {"ryu", "4' M"},
            {"ryo", "4' o"},
            {"wa", "w a"},
            {"wi", "w i"},
            {"we", "w e"},
            {"wo", "w o"},
            {"ga", "g a"},
            {"gi", "g' i"},
            {"gu", "g M"},
            {"ge", "g e"},
            {"go", "g o"},
            {"gya", "g' a"},
            {"gyu", "g' M"},
            {"gyo", "g' o"},
            {"za", "dz a"},
            {"zi", "dz i"},
            {"zu", "dz M"},
            {"ze", "dz e"},
            {"zo", "dz o"},
            {"da", "d a"},
            {"di", "d' i"},
            {"du", "d M"},
            {"de", "d e"},
            {"do", "d o"},
            {"dya", "d' a"},
            {"dyu", "d' M"},
            {"dyo", "d' o"},
            {"ba", "b a"},
            {"bi", "b' i"},
            {"bu", "b M"},
            {"be", "b e"},
            {"bo", "b o"},
            {"bya", "b' a"},
            {"byu", "b' M"},
            {"byo", "b' o"},
            {"pa", "p a"},
            {"pi", "p' i"},
            {"pu", "p M"},
            {"pe", "p e"},
            {"po", "p o"},
            {"pya", "p' a"},
            {"pyu", "p' M"},
            {"pyo", "p' o"},
            {"sha", "S a"},
            {"shi", "S i"},
            {"shu", "S M"},
            {"sho", "S o"},
            {"tsu", "ts M"},
            {"cha", "tS a"},
            {"chi", "tS i"},
            {"chu", "tS M"},
            {"cho", "tS o"},
            {"fu", "p\\ M"},
            {"ja", "dZ a"},
            {"ji", "dZ i"},
            {"ju", "dZ M"},
            {"jo", "dZ o"},
        };
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
            s_default_jp = new SymbolTable( "DEFAULT_JP", _KEY_JP, true );
            s_table.clear();
            s_table.put( 0, s_default_jp );
            int count = 0;

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
                    count++;
                    String dict = PortUtil.combinePath( path, files[i] );
                    s_table.put( count, new SymbolTable( dict, true, false ) );
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
                    count++;
                    String dict = PortUtil.combinePath( path2, files2[i] );
                    s_table.put( count, new SymbolTable( dict, false, false ) );
                }
            }
            s_initialized = true;
        }


        public static boolean attatch( String phrase, ByRef<String> result ) {
#if DEBUG
            PortUtil.println( "SymbolTable.Attatch" );
            PortUtil.println( "    phrase=" + phrase );
#endif
            for ( Iterator itr = s_table.keySet().iterator(); itr.hasNext(); ) {
                int key = (Integer)itr.next();
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
            PortUtil.println( "SymbolTable.Sort()" );
#endif
            TreeMap<Integer, SymbolTable> buff = new TreeMap<Integer, SymbolTable>();
            for ( Iterator itr = s_table.keySet().iterator(); itr.hasNext(); ) {
                int key = (Integer)itr.next();
                buff.put( key, (SymbolTable)s_table.get( key ).clone() );
            }
            s_table.clear();
            int count = list.size();
            for ( int i = 0; i < count; i++ ) {
                ValuePair<String, Boolean> itemi = list.get( i );
#if DEBUG
                PortUtil.println( "    list[i]=" + itemi.getKey() + "," + itemi.getValue() );
#endif
                for ( Iterator itr = buff.keySet().iterator(); itr.hasNext(); ) {
                    int key = (Integer)itr.next();
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
            for ( Iterator itr = m_dict.keySet().iterator(); itr.hasNext(); ) {
                String key = (String)itr.next();
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
                            if ( m_dict.containsKey( spl[0] ) ) {
                                PortUtil.println( "SymbolTable..ctor" );
                                PortUtil.println( "    dictionary already contains key: " + spl[0] );
                            } else {
                                m_dict.put( spl[0], spl[1] );
                            }
                        }
                    }
                }
            } catch ( Exception ex ) {
                PortUtil.println( "SymbolTable..ctor" );
                PortUtil.println( "    " + ex );
            } finally {
                if ( sr != null ) {
                    try {
                        sr.close();
                    } catch ( Exception ex2 ) {
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

#if JAVA
        private SymbolTable( String name, String[][] key, boolean enabled ){
#else
        private SymbolTable( String name, String[,] key, boolean enabled ) {
#endif
            m_enabled = enabled;
            m_name = name;
            m_dict = new TreeMap<String, String>();
#if JAVA
            for( int i = 0; i < key.length; i++ ){
                if( m_dict.containsKey( key[i][0] ) ){
                }else{
                    m_dict.put( key[i][0], key[i][1] );
                }
            }
#else
            for ( int i = 0; i < key.GetLength( 0 ); i++ ) {
                if ( m_dict.containsKey( key[i, 0] ) ) {
                } else {
                    m_dict.put( key[i, 0], key[i, 1] );
                }
            }
#endif
        }
    }

#if !JAVA
}
#endif
