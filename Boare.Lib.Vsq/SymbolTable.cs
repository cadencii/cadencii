/*
 * SymbolTable.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Lib.Vsq.
 *
 * Boare.Lib.Vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.Vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;

using bocoree;

namespace Boare.Lib.Vsq {

    using boolean = System.Boolean;

    public class SymbolTable : ICloneable {
        private TreeMap<String, String> m_dict;
        private String m_name;
        private boolean m_enabled;

        #region Static Field
        private static SortedList<int, SymbolTable> s_table = new SortedList<int, SymbolTable>();
        private static SymbolTable s_default_jp = null;
        private static boolean s_initialized = false;
        public static readonly String[,] _KEY_JP = {
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
            {"ふ", @"p\ M"},
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
            {"ふゃ", @"p\' a"},
            {"ふぃ", @"p\' i"},
            {"ふゅ", @"p\' M"},
            {"ふぇ", @"p\ e"},
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
            {"ふぁ", @"p\ a"},
            {"ふぉ", @"p\ o"},
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
            {"フ", @"p\ M"},
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
            {"フャ", @"p\' a"},
            {"フィ", @"p\' i"},
            {"フュ", @"p\' M"},
            {"フェ", @"p\ e"},
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
            {"ファ", @"p\ a"},
            {"フォ", @"p\ o"},
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
            {"fu", @"p\ M"},
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
            if ( 0 <= index && index < s_table.Count ) {
                return s_table[index];
            } else {
                return null;
            }
        }

        public static void loadDictionary() {
#if DEBUG
            Console.WriteLine( "SymbolTable.LoadDictionary()" );
#endif
            s_default_jp = new SymbolTable( "DEFAULT_JP", _KEY_JP, true );
            s_table.Clear();
            s_table.Add( 0, s_default_jp );
            int count = 0;

            // 辞書フォルダからの読込み
            String editor_path = VocaloSysUtil.getEditorPath( SynthesizerType.VOCALOID2 );
            if ( editor_path != "" ) {
                String path = PortUtil.combinePath( Path.GetDirectoryName( editor_path ), "UDIC" );
                if ( !Directory.Exists( path ) ) {
                    return;
                }
                String[] files = Directory.GetFiles( path, "*.udc" );
                for ( int i = 0; i < files.Length; i++ ) {
                    files[i] = Path.GetFileName( files[i] );
#if DEBUG
                    Console.WriteLine( "    files[i]=" + files[i] );
#endif
                    count++;
                    String dict = PortUtil.combinePath( path, files[i] );
                    s_table.Add( count, new SymbolTable( dict, true, false ) );
                }
            }

            // 起動ディレクトリ
            String path2 = PortUtil.combinePath( Application.StartupPath, "udic" );
            if ( Directory.Exists( path2 ) ) {
                String[] files2 = Directory.GetFiles( path2, "*.eudc" );
                for ( int i = 0; i < files2.Length; i++ ) {
                    files2[i] = Path.GetFileName( files2[i] );
#if DEBUG
                    Console.WriteLine( "    files2[i]=" + files2[i] );
#endif
                    count++;
                    String dict = PortUtil.combinePath( path2, files2[i] );
                    s_table.Add( count, new SymbolTable( dict, false, false ) );
                }
            }
            s_initialized = true;
        }


        public static boolean attatch( String phrase, out String result ) {
#if DEBUG
            Console.WriteLine( "SymbolTable.Attatch" );
            Console.WriteLine( "    phrase=" + phrase );
#endif
            for ( int i = 0; i < s_table.Keys.Count; i++ ) {
                int key = s_table.Keys[i];
                if ( s_table[key].isEnabled() ) {
                    if ( s_table[key].attatchImp( phrase, out result ) ) {
                        return true;
                    }
                }
            }
            result = "a";
            return false;
        }

        public static int getCount() {
            if ( !s_initialized ) {
                loadDictionary();
            }
            return s_table.Count;
        }

        public static void changeOrder( KeyValuePair<String, boolean>[] list ) {
#if DEBUG
            Console.WriteLine( "SymbolTable.Sort()" );
#endif
            SortedList<int, SymbolTable> buff = new SortedList<int, SymbolTable>();
            foreach ( int key in s_table.Keys ) {
                buff.Add( key, (SymbolTable)s_table[key].Clone() );
            }
            s_table.Clear();
            for ( int i = 0; i < list.Length; i++ ) {
#if DEBUG
                Console.WriteLine( "    list[i]=" + list[i].Key + "," + list[i].Value );
#endif
                for ( int j = 0; j < buff.Keys.Count; j++ ) {
                    int key = buff.Keys[j];
                    if ( buff[key].getName().Equals( list[i].Key ) ) {
                        buff[key].setEnabled( list[i].Value );
                        s_table.Add( i, buff[key] );
                        break;
                    }
                }
            }
        }
        #endregion

        public object Clone() {
            SymbolTable ret = new SymbolTable();
            ret.m_dict = new TreeMap<String, String>();
            for ( Iterator itr = m_dict.keySet().iterator(); itr.hasNext(); ){
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
            if ( !PortUtil.isFileExists( path ) ) {
                return;
            }
            m_name = Path.GetFileName( path );
            cp932reader sr1 = null;
            StreamReader sr2 = null;
            try {
                if ( is_udc_mode ) {
                    sr1 = new cp932reader( path );
                    if ( sr1 == null ) {
                        return;
                    }
                } else {
                    sr2 = new StreamReader( path, Encoding.UTF8 );
                    if ( sr2 == null ) {
                        return;
                    }
                }
                String line;
                int peek = (is_udc_mode) ? sr1.Peek() : sr2.Peek();
                while ( peek >= 0 ) {
                    line = (is_udc_mode) ? sr1.ReadLine() : sr2.ReadLine();
                    if ( !line.StartsWith( "//" ) ) {
                        String[] spl = PortUtil.splitString( line, new String[]{ "\t" }, 2, true );
                        if ( spl.Length >= 2 ) {
                            if ( m_dict.containsKey( spl[0] ) ) {
                                bocoree.debug.push_log( "SymbolTable..ctor" );
                                bocoree.debug.push_log( "    dictionary already contains key: " + spl[0] );
                            } else {
                                m_dict.put( spl[0], spl[1] );
                            }
                        }
                    }
                    peek = (is_udc_mode) ? sr1.Peek() : sr2.Peek();
                }
            } catch ( Exception ex ) {
                bocoree.debug.push_log( "SymbolTable..ctor" );
                bocoree.debug.push_log( "    " + ex );
            } finally {
                if ( sr1 != null ) {
                    sr1.Close();
                }
                if ( sr2 != null ) {
                    sr2.Close();
                }
            }
        }

        private boolean attatchImp( String phrase, out String result ) {
            String s = phrase.ToLower();
            if ( m_dict.containsKey( s ) ) {
                result = m_dict.get( s );
                return true;
            } else {
                result = "a";
                return false;
            }
        }

        private SymbolTable( String name, String[,] key, boolean enabled ) {
#if DEBUG
            Console.WriteLine( "SymolTable.ctor(String,String[,])" );
            Console.WriteLine( "    key.GetLength(0)=" + key.GetLength( 0 ) );
#endif
            m_enabled = enabled;
            m_name = name;
            m_dict = new TreeMap<String, String>();
            for ( int i = 0; i < key.GetLength( 0 ); i++ ) {
                if ( m_dict.containsKey( key[i, 0] ) ) {
#if DEBUG
                    throw new ApplicationException( "dictionary already contains key: " + key[i, 0] );
#endif
                } else {
                    m_dict.put( key[i, 0], key[i, 1] );
                }
            }
        }
    }

}
