/*
 * SymbolTable.java
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of com.boare.vsq.
 *
 * com.boare.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.vsq;

import java.util.*;
import java.io.*;
import com.boare.corlib.*;

public class SymbolTable implements Cloneable {
    private TreeMap<String, String> m_dict;
    private String m_name;
    private boolean m_enabled;

    private static TreeMap<Integer, SymbolTable> s_table = new TreeMap<Integer, SymbolTable>();
    private static SymbolTable s_default_jp = null;
    private static boolean s_initialized = false;
    public static final String[][] _KEY_JP = {
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
        s_default_jp = new SymbolTable( "DEFAULT_JP", _KEY_JP, true );
        s_table.clear();
        s_table.put( 0, s_default_jp );
        int count = 0;

        // 辞書フォルダからの読込み
        String editor_path = VocaloSysUtil.getEditorPath2();
        if ( editor_path.length() > 0 ) {
            String path = Path.combine( Path.getFileName( editor_path ), "UDIC" );
            File fpath = new File( path );
            if ( !fpath.exists() ) {
                return;
            }
            File[] files = fpath.listFiles( new FileFilterWithExtension( ".udc" ) );
            for( int i = 0; i < files.length; i++ ) {
                String s = Path.getFileName( files[i].getAbsolutePath() );
                count++;
                String dict = Path.combine( path, s );
                s_table.put( count, new SymbolTable( dict, true, false ) );
            }
        }

        // 起動ディレクトリ
        String path2 = Path.combine( ".\\", "udic" );
        File fpath2 = new File( path2 );
        if ( fpath2.exists() ) {
            File[] files2 = fpath2.listFiles( new FileFilterWithExtension( ".eudc" ) );
            for ( int i = 0; i < files2.length; i++ ) {
                String s = Path.getFileName( files2[i].getAbsolutePath() );
                count++;
                String dict = Path.combine( path2, s );
                s_table.put( count, new SymbolTable( dict, false, false ) );
            }
        }
        s_initialized = true;
    }


    public static boolean attatch( String phrase, StringBuilder result ) {
        for ( Iterator itr = s_table.keySet().iterator(); itr.hasNext(); ) {
            int key = (Integer)itr.next();
            if ( s_table.get( key ).isEnabled() ) {
                if ( s_table.get( key ).attatchImp( phrase, result ) ) {
                    return true;
                }
            }
        }
        result.setLength( 0 );
        result.append( "a" );
        return false;
    }

    public static int getCount() {
        if ( !s_initialized ) {
            loadDictionary();
        }
        return s_table.size();
    }

    public static void changeOrder( KeyValuePair<String, Boolean>[] list ) {
        TreeMap<Integer, SymbolTable> buff = new TreeMap<Integer, SymbolTable>();
        for( Iterator itr = s_table.keySet().iterator(); itr.hasNext(); ){
            int key = (Integer)itr.next();
            buff.put( key, (SymbolTable)s_table.get( key ).clone() );
        }
        s_table.clear();
        for ( int i = 0; i < list.length; i++ ) {
            for ( Iterator itr = buff.keySet().iterator(); itr.hasNext(); ) {
                int key = (Integer)itr.next();
                if ( buff.get( key ).getName().equals( list[i].key ) ) {
                    buff.get( key ).setEnabled( list[i].value );
                    s_table.put( i, buff.get( key ) );
                    break;
                }
            }
        }
    }

    public Object clone() {
        SymbolTable ret = new SymbolTable();
        ret.m_dict = new TreeMap<String, String>();
        for( Iterator itr = m_dict.keySet().iterator(); itr.hasNext(); ){
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
    
    public void setEnabled( boolean value ){
        m_enabled = value;
    }

    public SymbolTable( String path, boolean is_udc_mode, boolean enabled ) {
        m_dict = new TreeMap<String, String>();
        m_enabled = enabled;
        if ( !(new File( path )).exists() ) {
            return;
        }
        m_name = Path.getFileName( path );
        StreamReader sr1 = null;
        StreamReader sr2 = null;
        try {
            if ( is_udc_mode ) {
                sr1 = new StreamReader( path, "SJIS" );
                if ( sr1 == null ) {
                    return;
                }
            } else {
                sr2 = new StreamReader( path );
                if ( sr2 == null ) {
                    return;
                }
            }
            String line;
            while( (is_udc_mode ? (line = sr1.readLine()) != null :
                                  (line = sr2.readLine()) != null) ){
            //int peek = (is_udc_mode) ? sr1.peek() : sr2.peek();
            //while ( peek >= 0 ) {
                //line = (is_udc_mode) ? sr1.readLine() : sr2.readLine();
                if ( !line.startsWith( "//" ) ) {
                    String[] dum_spl = line.split( "\t" );
                    Vector<String> spl = new Vector<String>();
                    for( String s : dum_spl ){
                        if( s != null && !s.equals( "" ) ){
                            spl.add( s );
                        }
                    }
                    if ( spl.size() >= 2 ) {
                        if ( !m_dict.containsKey( spl.get( 0 ) ) ) {
                            m_dict.put( spl.get( 0 ), spl.get( 1 ) );
                        }
                    }
                }
                //peek = (is_udc_mode) ? sr1.peek() : sr2.peek();
            }
            if ( sr1 != null ) {
                sr1.close();
            }
            if ( sr2 != null ) {
                sr2.close();
            }
        } catch ( Exception ex ) {
            System.out.println( "SymbolTable..ctor" );
            System.out.println( "    " + ex );
        }
    }

    private boolean attatchImp( String phrase, StringBuilder result ) {
        String s = phrase.toLowerCase();
        if ( m_dict.containsKey( s ) ) {
            result.setLength( 0 );
            result.append( m_dict.get( s ) );
            return true;
        } else {
            result.setLength( 0 );
            result.append( "a" );
            return false;
        }
    }

    private SymbolTable( String name, String[][] key, boolean enabled ) {
        m_enabled = enabled;
        m_name = name;
        m_dict = new TreeMap<String, String>();
        for ( int i = 0; i < key.length; i++ ) {
            if ( m_dict.containsKey( key[i][0] ) ) {
            } else {
                m_dict.put( key[i][0], key[i][1] );
            }
        }
    }
}
