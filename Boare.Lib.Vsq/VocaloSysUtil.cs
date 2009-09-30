/*
 * VocaloSysUtil.s
 * Copyright (c) 2009 kbinani
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
using System.Collections.Generic;
using Microsoft.Win32;

using bocoree;

namespace Boare.Lib.Vsq {

    using boolean = Boolean;

    public static class VocaloSysUtil{
        private static TreeMap<SynthesizerType, SingerConfigSys> s_singer_config_sys = new TreeMap<SynthesizerType, SingerConfigSys>();
        private static TreeMap<SynthesizerType, ExpressionConfigSys> s_exp_config_sys = new TreeMap<SynthesizerType, ExpressionConfigSys>();
        private static TreeMap<SynthesizerType, String> s_path_vsti = new TreeMap<SynthesizerType, String>();
        private static TreeMap<SynthesizerType, String> s_path_editor = new TreeMap<SynthesizerType, String>();

        static VocaloSysUtil() {
            ExpressionConfigSys exp_config_sys1 = null;
            try {
                Vector<String> dir1 = new Vector<String>();
                RegistryKey key1 = Registry.LocalMachine.OpenSubKey( "SOFTWARE\\VOCALOID", false );
                String path_voicedb1 = "";
                String path_expdb1 = "";
                Vector<String> installed_singers1 = new Vector<String>();
                if ( key1 != null ) {
                    String header1 = "HKLM\\SOFTWARE\\VOCALOID";
                    print( key1, header1, dir1 );
#if DEBUG
                    using ( StreamWriter sw = new StreamWriter( Path.Combine( System.Windows.Forms.Application.StartupPath, "reg_keys_vocalo1.txt" ) ) ) {
                        foreach ( String s in dir1 ) {
                            sw.WriteLine( s );
                        }
                    }
#endif
                    key1.Close();
                    String path_vsti;
                    String path_editor;
                    extract( dir1,
                             header1,
                             out path_vsti,
                             out path_voicedb1,
                             out path_expdb1,
                             out path_editor,
                             installed_singers1 );
                    s_path_vsti.put( SynthesizerType.VOCALOID1, path_vsti );
                    s_path_editor.put( SynthesizerType.VOCALOID1, path_editor );
                }
                SingerConfigSys singer_config_sys = new SingerConfigSys( path_voicedb1, installed_singers1.toArray( new String[] { } ) );
                if ( File.Exists( Path.Combine( path_expdb1, "expression.map" ) ) ) {
                    exp_config_sys1 = new ExpressionConfigSys( path_expdb1 );
                }
                s_singer_config_sys.put( SynthesizerType.VOCALOID1, singer_config_sys );
            } catch ( Exception ex ) {
                Console.WriteLine( "VocaloSysUtil..cctor; ex=" + ex );
                SingerConfigSys singer_config_sys = new SingerConfigSys( "", new String[] { } );
                exp_config_sys1 = null;
                s_singer_config_sys.put( SynthesizerType.VOCALOID1, singer_config_sys );
            }
            if ( exp_config_sys1 == null ) {
                exp_config_sys1 = ExpressionConfigSys.getVocaloid1Default();
            }
            s_exp_config_sys.put( SynthesizerType.VOCALOID1, exp_config_sys1 );
#if DEBUG
            exp_config_sys1.printTo( @"C:\vocalo1.txt" );
#endif

            ExpressionConfigSys exp_config_sys2 = null;
            try{
                Vector<String> dir2 = new Vector<String>();
                RegistryKey key2 = Registry.LocalMachine.OpenSubKey( "SOFTWARE\\VOCALOID2", false );
                String path_voicedb2 = "";
                String path_expdb2 = "";
                Vector<String> installed_singers2 = new Vector<String>();
                if ( key2 != null ) {
                    String header2 = "HKLM\\SOFTWARE\\VOCALOID2";
                    print( key2, header2, dir2 );
#if DEBUG
                    using ( StreamWriter sw = new StreamWriter( Path.Combine( System.Windows.Forms.Application.StartupPath, "reg_keys_vocalo2.txt" ) ) ) {
                        foreach ( String s in dir2 ) {
                            sw.WriteLine( s );
                        }
                    }
#endif
                    key2.Close();
                    String path_vsti;
                    String path_editor;
                    extract( dir2,
                             header2,
                             out path_vsti,
                             out path_voicedb2,
                             out path_expdb2,
                             out path_editor,
                             installed_singers2 );
                    s_path_vsti.put( SynthesizerType.VOCALOID2, path_vsti );
                    s_path_editor.put( SynthesizerType.VOCALOID2, path_editor );
                }
                SingerConfigSys singer_config_sys = new SingerConfigSys( path_voicedb2, installed_singers2.toArray( new String[] { } ) );
                if ( File.Exists( Path.Combine( path_expdb2, "expression.map" ) ) ) {
                    exp_config_sys2 = new ExpressionConfigSys( path_expdb2 );
                }
                s_singer_config_sys.put( SynthesizerType.VOCALOID2, singer_config_sys );
            } catch ( Exception ex ) {
                Console.WriteLine( "VocaloSysUtil..cctor; ex=" + ex );
                SingerConfigSys singer_config_sys = new SingerConfigSys( "", new String[] { } );
                exp_config_sys2 = null;
                s_singer_config_sys.put( SynthesizerType.VOCALOID2, singer_config_sys );
            }
            if ( exp_config_sys2 == null ) {
                exp_config_sys2 = ExpressionConfigSys.getVocaloid2Default();
            }
            s_exp_config_sys.put( SynthesizerType.VOCALOID2, exp_config_sys2 );
#if DEBUG
            exp_config_sys2.printTo( @"C:\vocalo2.txt" );
#endif
        }

        /// <summary>
        /// ビブラートのプリセットタイプから，VibratoHandleを作成します
        /// </summary>
        /// <param name="type"></param>
        /// <param name="vibrato_clocks"></param>
        /// <returns></returns>
        public static VibratoHandle getDefaultVibratoHandle( String icon_id, int vibrato_length, SynthesizerType type ) {
            if ( s_exp_config_sys.containsKey( type ) ) {
                for ( Iterator itr = s_exp_config_sys.get( type ).vibratoConfigIterator(); itr.hasNext(); ) {
                    VibratoConfig vconfig = (VibratoConfig)itr.next();
                    if ( vconfig.contents.IconID.Equals( icon_id ) ) {
                        VibratoHandle ret = (VibratoHandle)vconfig.contents.clone();
                        ret.Length = vibrato_length;
                        return ret;
                    }
                }
            }
            VibratoHandle empty = new VibratoHandle();
            empty.IconID = "$04040000";
            return empty;
        }


        private static void extract( Vector<String> dir,
                                     String header,
                                     out String path_vsti, 
                                     out String path_voicedb, 
                                     out String path_expdb, 
                                     out String path_editor,
                                     Vector<String> installed_singers ) {
            Vector<String> application = new Vector<String>();
            Vector<String> expression = new Vector<String>();
            Vector<String> voice = new Vector<String>();
            path_vsti = "";
            path_expdb = "";
            path_voicedb = "";
            path_editor = "";
            for( Iterator itr = dir.iterator(); itr.hasNext(); ) {
                String s = (String)itr.next();
                if ( s.StartsWith( header + "\\APPLICATION" ) ) {
                    application.add( s.Substring( (header + "\\APPLICATION").Length ) );
                } else if ( s.StartsWith( header + "\\DATABASE\\EXPRESSION" ) ) {
                    expression.add( s.Substring( (header + "\\DATABASE\\EXPRESSION").Length ) );
                } else if ( s.StartsWith( header + "\\DATABASE\\VOICE" ) ) {
                    voice.add( s.Substring( (header + "\\DATABASE\\VOICE\\").Length ) );
                }
            }

            // path_vstiを取得
            for( Iterator itr = application.iterator(); itr.hasNext(); ){
                String s = (String)itr.next();
                String[] spl = s.Split( '\t' );
                if ( spl.Length >= 3 && spl[1].Equals( "PATH" ) ){
                    if ( spl[2].ToLower().EndsWith( ".dll" ) ) {
                        path_vsti = spl[2];
                    } else if ( spl[2].ToLower().EndsWith( ".exe" ) ) {
                        path_editor = spl[2];
                    }
                }
            }

            // path_vicedbを取得
            Vector<String> voice_ids = new Vector<String>();
            // 最初はpath_voicedbの取得と、id（BHXXXXXXXXXXXXXXXX）のようなシリアルを取得
            for( Iterator itr = voice.iterator(); itr.hasNext(); ){
                String s = (String)itr.next();
                String[] spl = s.Split( '\t' );
                if ( spl.Length >= 2 ) {
                    if ( spl[0].Equals( "VOICEDIR" ) ) {
                        path_voicedb = spl[1];
                    } else if ( spl.Length >= 3 ) {
                        String[] spl2 = spl[0].Split( '\\' );
                        if ( spl2.Length == 1 ) {
                            if ( !voice_ids.contains( spl2[0] ) ) {
                                voice_ids.add( spl2[0] );
                            }
                        }
                    }
                }
            }
            // 取得したシリアルを元に、installed_singersを取得
            for( Iterator itr = voice_ids.iterator(); itr.hasNext(); ) {
                String s = (String)itr.next();
                String install_dir = "";
                for( Iterator itr2 = voice.iterator(); itr2.hasNext(); ){
                    String s2 = (String)itr2.next();
                    if ( s2.StartsWith( header + "\\" + s + "\t" ) ) {
                        String[] spl = s2.Split( '\t' );
                        if ( spl.Length >= 3 && spl[1].Equals( "INSTALLDIR" ) ) {
                            install_dir = Path.Combine( spl[2], s );
                            break;
                        }
                    }
                }
                if ( install_dir.Equals( "" ) ) {
                    install_dir = Path.Combine( path_voicedb, s );
                }
                installed_singers.add( install_dir );
            }

            // path_expdbを取得
            Vector<String> exp_ids = new Vector<String>();
            // 最初はpath_expdbの取得と、id（BHXXXXXXXXXXXXXXXX）のようなシリアルを取得
            for( Iterator itr = expression.iterator(); itr.hasNext(); ){
                String s = (String)itr.next();
#if DEBUG
                Console.WriteLine( "VocaloSysUtil#extract; s=" + s );
#endif
                String[] spl = s.Split( new char[]{ '\t' }, StringSplitOptions.RemoveEmptyEntries );
                if ( spl.Length >= 2 ) {
                    if ( spl[0].Equals( "EXPRESSIONDIR" ) ) {
                        path_expdb = spl[1];
                    } else if ( spl.Length >= 3 ) {
                        String[] spl2 = spl[0].Split( '\\' );
                        if ( spl2.Length == 1 ) {
                            if ( !exp_ids.contains( spl2[0] ) ) {
                                exp_ids.add( spl2[0] );
                            }
                        }
                    }
                }
            }
            // 取得したシリアルを元に、installed_singersを取得
            /*foreach ( String s in exp_ids ) {
                String install_dir = "";
                foreach ( String s2 in expression ) {
                    if ( s2.StartsWith( header + "\\" + s + "\t" ) ) {
                        String[] spl = s2.Split( '\t' );
                        if ( spl.Length >= 3 && spl[1].Equals( "INSTALLDIR" ) ) {
                            install_dir = Path.Combine( spl[2], s );
                            break;
                        }
                    }
                }
                if ( install_dir.Equals( "" ) ) {
                    install_dir = Path.Combine( path_expdb, s );
                }
                installed_singers.Add( install_dir );
            }*/

#if DEBUG
            Console.WriteLine( "path_vsti=" + path_vsti );
            Console.WriteLine( "path_voicedb=" + path_voicedb );
            Console.WriteLine( "path_expdb=" + path_expdb );
            Console.WriteLine( "installed_singers=" );
            for( Iterator itr = installed_singers.iterator(); itr.hasNext(); ){
                String s = (String)itr.next();
                Console.WriteLine( "    " + s );
            }
#endif
        }

        // レジストリkey内の値を再帰的に検索し、ファイルfpに順次出力する
        private static void print( RegistryKey key, String parent_name, Vector<String> list ){
            if ( key == null ) {
                return;
            }

            // 直下のキー内を再帰的にリストアップ
            String[] subkeys = key.GetSubKeyNames();
            foreach( String s in subkeys ){
                RegistryKey subkey = key.OpenSubKey( s, false );
                print( subkey, parent_name + "\\" + s, list );
                subkey.Close();
            }

            // 直下の値を出力
            String[] valuenames = key.GetValueNames();
            foreach( String s in valuenames ){
                RegistryValueKind kind = key.GetValueKind( s );
                if ( kind == RegistryValueKind.String ){
                    String str = parent_name + "\t" + s + "\t" + (String)key.GetValue( s, "" );
                    list.add( str );
                }
            }
        }

        public static Iterator attackConfigIterator( SynthesizerType type ) {
            if ( s_exp_config_sys.containsKey( type ) ) {
                return s_exp_config_sys.get( type ).attackConfigIterator();
            } else {
                return new ListIterator<AttackConfig>( new List<AttackConfig>() );
            }
        }

        public static Iterator vibratoConfigIterator( SynthesizerType type ) {
            if ( s_exp_config_sys.containsKey( type ) ) {
                return s_exp_config_sys.get( type ).vibratoConfigIterator();
            } else {
                return new ListIterator<VibratoConfig>( new List<VibratoConfig>() );
            }
        }

        /// <summary>
        /// Gets the name of original singer of specified program change.
        /// </summary>
        /// <param name="singer"></param>
        /// <returns></returns>
        public static String getOriginalSinger( String singer, SynthesizerType type ) {
            String voiceidstr = "";
            if ( !s_singer_config_sys.containsKey( type ) ){
                return "";
            }
            SingerConfigSys scs = s_singer_config_sys.get( type );
            SingerConfig[] singer_configs = scs.getSingerConfigs();
            for ( int i = 0; i < singer_configs.Length; i++ ) {
                if ( singer.Equals( singer_configs[i].VOICENAME ) ) {
                    voiceidstr = singer_configs[i].VOICEIDSTR;
                    break;
                }
            }
            if ( voiceidstr.Equals( "" ) ) {
                return "";
            }
            SingerConfig[] installed_singers = scs.getInstalledSingers();
            for ( int i = 0; i < installed_singers.Length; i++ ) {
                if ( voiceidstr.Equals( installed_singers[i].VOICEIDSTR ) ) {
                    return installed_singers[i].VOICENAME;
                }
            }
            return "";
        }

        public static VsqID getSingerID( String singer, SynthesizerType type ) {
            if ( !s_singer_config_sys.containsKey( type ) ) {
                return null;
            } else {
                return s_singer_config_sys.get( type ).getSingerID( singer );
            }
        }

        public static String getEditorPath( SynthesizerType type ) {
            if ( !s_path_editor.containsKey( type ) ) {
                return "";
            } else {
                return s_path_editor.get( type );
            }
        }

        public static String getDllPathVsti( SynthesizerType type ) {
            if ( !s_path_vsti.containsKey( type ) ) {
                return "";
            } else {
                return s_path_vsti.get( type );
            }
        }

        public static SingerConfig[] getSingerConfigs( SynthesizerType type ) {
            if ( !s_singer_config_sys.containsKey( type ) ) {
                return new SingerConfig[] { };
            } else {
                return s_singer_config_sys.get( type ).getSingerConfigs();
            }
        }

        /// <summary>
        /// Gets the voice language of specified program change
        /// </summary>
        /// <param name="name">name of singer</param>
        /// <returns></returns>
        public static VsqVoiceLanguage getLanguageFromName( String name ) {
            switch ( name.ToLower() ) {
                case "meiko":
                case "kaito":
                case "miku":
                case "rin":
                case "len":
                case "rin_act2":
                case "len_act2":
                case "gackpoid":
                case "luka_jpn":
                case "megpoid":
                    return VsqVoiceLanguage.Japanese;
                case "sweet_ann":
                case "prima":
                case "luka_eng":
                case "sonika":
                    return VsqVoiceLanguage.English;
            }
            return VsqVoiceLanguage.Default;
        }

        public static double getAmplifyCoeffFromPanLeft( int pan ) {
            return pan / -64.0 + 1.0;
        }

        public static double getAmplifyCoeffFromPanRight( int pan ) {
            return pan / 64.0 + 1.0;
        }

        public static double getAmplifyCoeffFromFeder( int feder ) {
            return Math.Exp( -1.26697245e-02 + 1.18448420e-01 * feder / 10.0 );
        }

        /// <summary>
        /// Transform the byte array(length=8) to unsigned long, assuming that the byte array is little endian.
        /// </summary>
        /// <param name="oct"></param>
        /// <returns></returns>
        public static ulong makelong_le( byte[] oct ) {
            return (ulong)oct[7] << 56 | (ulong)oct[6] << 48 | (ulong)oct[5] << 40 | (ulong)oct[4] << 32 | (ulong)oct[3] << 24 | (ulong)oct[2] << 16 | (ulong)oct[1] << 8 | (ulong)oct[0];
        }
    }

    /*public static class VocaloSysUtil_ {
        private static boolean s_initialized = false;

        private static String s_dll_path2 = "";
        private static String s_editor_path2 = "";
        private static String s_voicedbdir2 = "";
        private static Vector<SingerConfig> s_installed_singers2 = new Vector<SingerConfig>();
        private static Vector<SingerConfig> s_singer_configs2 = new Vector<SingerConfig>();

        private static String s_dll_path1 = "";
        private static String s_editor_path1 = "";
        private static String s_voicedbdir1 = "";
        private static Vector<SingerConfig> s_installed_singers1 = new Vector<SingerConfig>();
        private static Vector<SingerConfig> s_singer_configs1 = new Vector<SingerConfig>();

        private const int MAX_SINGERS = 0x4000;

        static VocaloSysUtil_() {
            init_vocalo2();
            init_vocalo1();
        }

        /// <summary>
        /// Gets the name of original singer of specified program change.
        /// </summary>
        /// <param name="singer"></param>
        /// <returns></returns>
        public static String getOriginalSinger1( String singer ) {
            String voiceidstr = "";
            for ( int i = 0; i < s_singer_configs1.size(); i++ ) {
                if ( singer.Equals( s_singer_configs1.get( i ).VOICENAME ) ) {
                    voiceidstr = s_singer_configs1.get( i ).VOICEIDSTR;
                }
            }
            if ( voiceidstr.Equals( "" ) ) {
                return "";
            }
            for ( int i = 0; i < s_installed_singers1.size(); i++ ) {
                if ( voiceidstr.Equals( s_installed_singers1.get( i ).VOICEIDSTR ) ) {
                    return s_installed_singers1.get( i ).VOICENAME;
                }
            }
            return "";
        }

        /// <summary>
        /// Gets the name of original singer of specified program change.
        /// </summary>
        /// <param name="singer"></param>
        /// <returns></returns>
        public static String getOriginalSinger2( String singer ) {
            String voiceidstr = "";
            for ( int i = 0; i < s_singer_configs2.size(); i++ ) {
                if ( singer.Equals( s_singer_configs2.get( i ).VOICENAME ) ) {
                    voiceidstr = s_singer_configs2.get( i ).VOICEIDSTR;
                }
            }
            if ( voiceidstr.Equals( "" ) ) {
                return "";
            }
            for ( int i = 0; i < s_installed_singers2.size(); i++ ) {
                if ( voiceidstr.Equals( s_installed_singers2.get( i ).VOICEIDSTR ) ) {
                    return s_installed_singers2.get( i ).VOICENAME;
                }
            }
            return "";
        }

        /// <summary>
        /// Gets the voice language of specified program change
        /// </summary>
        /// <param name="name">name of singer</param>
        /// <returns></returns>
        public static VsqVoiceLanguage getLanguageFromName( String name ) {
            switch ( name ) {
                case "MEIKO":
                case "KAITO":
                case "Miku":
                case "Rin":
                case "Len":
                case "Rin_ACT2":
                case "Len_ACT2":
                case "Gackpoid":
                case "Luka_JPN":
                case "Megpoid":
                    return VsqVoiceLanguage.Japanese;
                case "Sweet_Ann":
                case "Prima":
                case "Luka_ENG":
                    return VsqVoiceLanguage.English;
            }
            return VsqVoiceLanguage.Default;
        }

        public static VsqID getSingerID1( String singer_name ) {
            VsqID ret = new VsqID( 0 );
            ret.type = VsqIDType.Singer;
            SingerConfig sc = null;
            for ( int i = 0; i < s_singer_configs1.size(); i++ ) {
                if ( s_singer_configs1.get( i ).VOICENAME.Equals( singer_name ) ) {
                    sc = s_singer_configs1.get( i );
                    break;
                }
            }
            if ( sc == null ) {
                sc = new SingerConfig();
            }
            int lang = 0;
            for ( Iterator itr = s_installed_singers1.iterator(); itr.hasNext(); ){
                SingerConfig sc2 = (SingerConfig)itr.next();
                if ( sc.VOICEIDSTR.Equals( sc2.VOICEIDSTR ) ) {
                    lang = (int)getLanguageFromName( sc.VOICENAME );
                    break;
                }
            }
            ret.IconHandle = new IconHandle();
            ret.IconHandle.IconID = "$0701" + sc.Program.ToString( "0000" );
            ret.IconHandle.IDS = sc.VOICENAME;
            ret.IconHandle.Index = 0;
            ret.IconHandle.Language = lang;
            ret.IconHandle.Length = 1;
            ret.IconHandle.Original = sc.Original;
            ret.IconHandle.Program = sc.Program;
            ret.IconHandle.Caption = "";
            return ret;
        }

        public static VsqID getSingerID2( String singer_name ) {
            VsqID ret = new VsqID( 0 );
            ret.type = VsqIDType.Singer;
            SingerConfig sc = null;
            for ( int i = 0; i < s_singer_configs2.size(); i++ ) {
                if ( s_singer_configs2.get( i ).VOICENAME.Equals( singer_name ) ) {
                    sc = s_singer_configs2.get( i );
                    break;
                }
            }
            if ( sc == null ) {
                sc = new SingerConfig();
            }
            int lang = 0;
            for ( Iterator itr = s_installed_singers2.iterator(); itr.hasNext(); ){
                SingerConfig sc2 = (SingerConfig)itr.next();
                if ( sc.VOICEIDSTR.Equals( sc2.VOICEIDSTR ) ) {
                    lang = (int)getLanguageFromName( sc.VOICENAME );
                    break;
                }
            }
            ret.IconHandle = new IconHandle();
            ret.IconHandle.IconID = "$0701" + sc.Program.ToString( "0000" );
            ret.IconHandle.IDS = sc.VOICENAME;
            ret.IconHandle.Index = 0;
            ret.IconHandle.Language = lang;
            ret.IconHandle.Length = 1;
            ret.IconHandle.Original = sc.Original;
            ret.IconHandle.Program = sc.Program;
            ret.IconHandle.Caption = "";
            return ret;
        }

        public static SingerConfig[] getSingerConfigs1() {
            return s_singer_configs1.toArray( new SingerConfig[]{} );
        }

        public static SingerConfig[] getSingerConfigs2() {
            return s_singer_configs2.toArray( new SingerConfig[]{} );
        }

        public static double getAmplifyCoeffFromPanLeft( int pan ) {
            return pan / -64.0 + 1.0;
        }

        public static double getAmplifyCoeffFromPanRight( int pan ) {
            return pan / 64.0 + 1.0;
        }

        public static double getAmplifyCoeffFromFeder( int feder ) {
            return Math.Exp( -1.26697245e-02 + 1.18448420e-01 * feder / 10.0 );
        }

        public static String getEditorPath2() {
            return s_editor_path2;
        }

        public static String getEditorPath1() {
            return s_editor_path1;
        }

        public static String getDllPathVsti2() {
            return s_dll_path2;
        }

        public static String getDllPathVsti1() {
            return s_dll_path1;
        }

        /// <summary>
        /// VOCALOID1システムのプロパティを取得
        /// </summary>
        private static void init_vocalo1() {
            // vocaloid1 dll path
            RegistryKey v1application = null;
            v1application = Registry.LocalMachine.OpenSubKey( "SOFTWARE\\VOCALOID\\APPLICATION", false );
            if ( v1application != null ) {
                String[] keys = v1application.GetSubKeyNames();
                for ( int i = 0; i < keys.Length; i++ ) {
                    RegistryKey key = v1application.OpenSubKey( keys[i], false );
                    if ( key != null ) {
                        String name = (String)key.GetValue( "PATH" );
                        if ( name.ToLower().EndsWith( "\\vocaloid.dll" ) ) {
                            s_dll_path1 = name;
                        } else if ( name.ToLower().EndsWith( "\\vocaloid.exe" ) ) {
                            s_editor_path1 = name;
                        }
                        key.Close();
                    }
                }
                v1application.Close();
            }

            // voicedbdir for vocaloid1
            RegistryKey v1database = Registry.LocalMachine.OpenSubKey( "SOFTWARE\\VOCALOID\\DATABASE\\VOICE", false );
            if ( v1database != null ) {
                s_voicedbdir1 = (String)v1database.GetValue( "VOICEDIR", "" );
#if DEBUG
                Console.WriteLine( "s_voicedbdir1=" + s_voicedbdir1 );
#endif
                // インストールされている歌手のVOICEIDSTRを列挙
                String[] singer_voiceidstrs = v1database.GetSubKeyNames();
                Vector<String> vvoice_keys = new Vector<String>();
                Vector<SingerConfig> vvoice_values = new Vector<SingerConfig>();
                foreach ( String voiceidstr in singer_voiceidstrs ) {
                    RegistryKey singer = v1database.OpenSubKey( voiceidstr );
                    if ( singer == null ) {
                        continue;
                    }
                    RegistryKey vvoice = singer.OpenSubKey( "vvoice" );
                    if ( vvoice != null ) {
                        String[] vvoices = vvoice.GetValueNames();

                        // インストールされた歌手の.vvdを読みにいく
                        // installdir以下の、拡張子.vvdのファイルを探す
                        foreach ( String file in Directory.GetFiles( Path.Combine( s_voicedbdir1, voiceidstr ), "*.vvd" ) ) {
                            SingerConfig config = SingerConfig.fromVvd( file, 0 ); //とりあえずプログラムチェンジは0
                            s_installed_singers1.add( config );
                        }

                        // vvoice*.vvdを読みにいく。
                        foreach ( String s in vvoices ) {
#if DEBUG
                            Console.WriteLine( "s=" + s );
#endif
                            String file = Path.Combine( s_voicedbdir1, s + ".vvd" );
                            if ( File.Exists( file ) ) {
                                SingerConfig config = SingerConfig.fromVvd( file, 0 );
                                vvoice_keys.add( s );
                                vvoice_values.add( config );
                            }
                        }
                    }
                    singer.Close();
                }

                // voice.mapを読み込んで、s_singer_configs1のプログラムチェンジを更新する
                String map = Path.Combine( s_voicedbdir1, "voice.map" );
                if ( File.Exists( map ) ) {
                    using ( FileStream fs = new FileStream( map, FileMode.Open, FileAccess.Read ) ) {
                        byte[] dat = new byte[8];
                        fs.Seek( 0x20, SeekOrigin.Begin );
                        for ( int i = 0; i < MAX_SINGERS; i++ ) {
                            fs.Read( dat, 0, 8 );
                            ulong value = makelong_le( dat );
                            if ( value >= 1 ) {
#if DEBUG
                                Console.WriteLine( "value=" + value );
#endif
                                for ( int j = 0; j < vvoice_keys.size(); j++ ) {
                                    if ( vvoice_keys.get( j ).Equals( "vvoice" + value ) ) {
                                        vvoice_values.get( j ).Program = i;
                                    }
                                }
                            }
                        }
                    }
                }

                // s_installed_singers1のSingerConfigのProgramとOriginalを適当に頒番する
                for ( int i = 0; i < s_installed_singers1.size(); i++ ) {
                    s_installed_singers1.get( i ).Program = i;
                    s_installed_singers1.get( i ).Original = i;
                }

                // s_singer_configs1を更新
                for ( int i = 0; i < vvoice_values.size(); i++ ) {
                    for ( int j = 0; j < s_installed_singers1.size(); j++ ) {
                        if ( vvoice_values.get( i ).VOICEIDSTR.Equals( s_installed_singers1.get( j ).VOICEIDSTR ) ) {
                            vvoice_values.get( i ).Original = s_installed_singers1.get( j ).Program;
                            break;
                        }
                    }
                    s_singer_configs1.add( vvoice_values.get( i ) );
                }
                v1database.Close();
            }
#if DEBUG
            Console.WriteLine( "installed" );
            for ( Iterator itr = s_installed_singers1.iterator(); itr.hasNext(); ){
                SingerConfig sc = (SingerConfig)itr.next();
                Console.WriteLine( "VOICENAME=" + sc.VOICENAME + "; VOICEIDSTR=" + sc.VOICEIDSTR + "; Program=" + sc.Program + "; Original=" + sc.Original );
            }
            Console.WriteLine( "singer configs" );
            for ( Iterator itr = s_singer_configs1.iterator(); itr.hasNext(); ){
                SingerConfig sc = (SingerConfig)itr.next();
                Console.WriteLine( "VOICENAME=" + sc.VOICENAME + "; VOICEIDSTR=" + sc.VOICEIDSTR + "; Program=" + sc.Program + "; Original=" + sc.Original );
            }
#endif
        }

        /// <summary>
        /// VOCALOID2システムのプロパティを取得
        /// </summary>
        private static void init_vocalo2() {
            // 最初はvstiとeditorのパスを取得
            RegistryKey v2application = Registry.LocalMachine.OpenSubKey( "SOFTWARE\\VOCALOID2\\APPLICATION", false );
            if ( v2application == null ) {
                v2application = Registry.LocalMachine.OpenSubKey( "SOFTWARE\\VOCALOID2_DEMO\\APPLICATION", false );
            }
            if ( v2application != null ) {
                String[] keys = v2application.GetSubKeyNames();
                for ( int i = 0; i < keys.Length; i++ ) {
                    RegistryKey key = v2application.OpenSubKey( keys[i], false );
                    if ( key != null ) {
                        String name = (String)key.GetValue( "PATH" );
                        if ( name.ToLower().EndsWith( "\\vocaloid2.dll" ) ) {
                            s_dll_path2 = name;
                        } else if ( name.ToLower().EndsWith( "\\vocaloid2_demo.dll" ) ) {
                            s_dll_path2 = name;
                        } else if ( name.ToLower().EndsWith( "\\vocaloid2.exe" ) ) {
                            s_editor_path2 = name;
                        }
                        key.Close();
                    }
                }
                v2application.Close();
            }

            // 歌声データベースを取得
            RegistryKey v2database = Registry.LocalMachine.OpenSubKey( "SOFTWARE\\VOCALOID2\\DATABASE\\VOICE", false );
            if ( v2database != null ) {
                // データベース（というよりもvoice.map）が保存されているパスを取得
                s_voicedbdir2 = (String)v2database.GetValue( "VOICEDIR", "" );
                // インストールされている歌手のVOICEIDSTRを列挙
                String[] singer_voiceidstrs = v2database.GetSubKeyNames();
                Vector<String> vvoice_keys = new Vector<String>();
                Vector<SingerConfig> vvoice_values = new Vector<SingerConfig>();
                foreach ( String voiceidstr in singer_voiceidstrs ) {
                    RegistryKey singer = v2database.OpenSubKey( voiceidstr );
                    if ( singer == null ) {
                        continue;
                    }
                    String installdir = (String)singer.GetValue( "INSTALLDIR", "" );
#if DEBUG
                    Console.WriteLine( "installdir=" + installdir );
#endif
                    RegistryKey vvoice = singer.OpenSubKey( "vvoice" );
                    if ( vvoice != null ) {
                        String[] vvoices = vvoice.GetValueNames();

                        // インストールされた歌手の.vvdを読みにいく
                        // installdir以下の、拡張子.vvdのファイルを探す
                        foreach ( String file in Directory.GetFiles( Path.Combine( installdir, voiceidstr ), "*.vvd" ) ) {
                            SingerConfig config = SingerConfig.fromVvd( file, 0 ); //とりあえずプログラムチェンジは0
                            s_installed_singers2.add( config );
                        }

                        // vvoice*.vvdを読みにいく。場所は、installdirではなく、s_voicedbdir2
                        foreach ( String s in vvoices ) {
                            String file = Path.Combine( s_voicedbdir2, s + ".vvd" );
                            if ( File.Exists( file ) ) {
                                SingerConfig config = SingerConfig.fromVvd( file, 0 );
                                vvoice_keys.add( s );
                                vvoice_values.add( config );
                            }
                        }
                    }
                    singer.Close();
                }

                // voice.mapを読み込んで、s_singer_configs2のプログラムチェンジを更新する
                String map = Path.Combine( s_voicedbdir2, "voice.map" );
                if ( File.Exists( map ) ) {
                    using ( FileStream fs = new FileStream( map, FileMode.Open, FileAccess.Read ) ) {
                        byte[] dat = new byte[8];
                        fs.Seek( 0x20, SeekOrigin.Begin );
                        for ( int i = 0; i < MAX_SINGERS; i++ ) {
                            fs.Read( dat, 0, 8 );
                            ulong value = makelong_le( dat );
                            if ( value >= 1 ) {
#if DEBUG
                                Console.WriteLine( "value=" + value );
#endif
                                for ( int j = 0; j < vvoice_keys.size(); j++ ) {
                                    if ( vvoice_keys.get( j ).Equals( "vvoice" + value ) ) {
                                        vvoice_values.get( j ).Program = i;
                                    }
                                }
                            }
                        }
                    }
                }

                // s_installed_singers2のSingerConfigのProgramとOriginalを適当に頒番する
                for ( int i = 0; i < s_installed_singers2.size(); i++ ) {
                    s_installed_singers2.get( i ).Program = i;
                    s_installed_singers2.get( i ).Original = i;
                }

                // s_singer_configs2を更新
                for ( int i = 0; i < vvoice_values.size(); i++ ) {
                    for ( int j = 0; j < s_installed_singers2.size(); j++ ) {
                        if ( vvoice_values.get( i ).VOICEIDSTR.Equals( s_installed_singers2.get( j ).VOICEIDSTR ) ) {
                            vvoice_values.get( i ).Original = s_installed_singers2.get( j ).Program;
                            break;
                        }
                    }
                    s_singer_configs2.add( vvoice_values.get( i ) );
                }
                v2database.Close();
            }
#if DEBUG
            Console.WriteLine( "installed" );
            for ( Iterator itr = s_installed_singers2.iterator(); itr.hasNext(); ){
                SingerConfig sc = (SingerConfig)itr.next();
                Console.WriteLine( "VOICENAME=" + sc.VOICENAME + "; VOICEIDSTR=" + sc.VOICEIDSTR + "; Program=" + sc.Program + "; Original=" + sc.Original );
            }
            Console.WriteLine( "singer configs" );
            for ( Iterator itr = s_singer_configs2.iterator(); itr.hasNext(); ){
                SingerConfig sc = (SingerConfig)itr.next();
                Console.WriteLine( "VOICENAME=" + sc.VOICENAME + "; VOICEIDSTR=" + sc.VOICEIDSTR + "; Program=" + sc.Program + "; Original=" + sc.Original );
            }
#endif
        }

        /// <summary>
        /// Transform the byte array(length=8) to unsigned long, assuming that the byte array is little endian.
        /// </summary>
        /// <param name="oct"></param>
        /// <returns></returns>
        public static ulong makelong_le( byte[] oct ) {
            return (ulong)oct[7] << 56 | (ulong)oct[6] << 48 | (ulong)oct[5] << 40 | (ulong)oct[4] << 32 | (ulong)oct[3] << 24 | (ulong)oct[2] << 16 | (ulong)oct[1] << 8 | (ulong)oct[0];
        }
    }*/

}
