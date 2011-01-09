/*
 * SingerConfigSys.cs
 * Copyright (C) 2009-2010 kbinani
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

import java.io.*;
import java.util.*;
import org.kbinani.*;
#else
#if __cplusplus
#else
using System;
using System.Collections.Generic;
#endif

namespace org
{
    namespace kbinani
    {
        namespace vsq
        {
#endif

#if __cplusplus
            class SingerConfigSys
#else
            public class SingerConfigSys
#endif
            {
                public const int MAX_SINGERS = 0x4000;

#if JAVA
                private Vector<SingerConfig> m_installed_singers = new Vector<SingerConfig>();
#elif __cplusplus
                private vector<SingerConfig> m_installed_singers;
#else
                private List<SingerConfig> m_installed_singers = new List<SingerConfig>();
#endif
#if JAVA
                private Vector<SingerConfig> m_singer_configs = new Vector<SingerConfig>();
#elif __cplusplus
                private vector<SingerConfig> m_singer_configs;
#else
                private List<SingerConfig> m_singer_configs = new List<SingerConfig>();
#endif

                /// <summary>
                /// 
                /// </summary>
                /// <param name="path_voicedb">音源のデータディレクトリ(ex:"C:\Program Files\VOCALOID2\voicedbdir")</param>
                /// <param name="path_installed_singers">音源のインストールディレクトリ(ex:new String[]{ "C:\Program Files\VOCALOID2\voicedbdir\BXXXXXXXXXXXXXXX", "D:\singers\BNXXXXXXXXXX" })</param>
#if JAVA
                public SingerConfigSys( String path_voicedb, Vector<String> path_installed_singers )
#elif __cplusplus
                public SingerConfigSys( String path_voicedb, vector<string> &path_installed_singers )
#else
                public SingerConfigSys( String path_voicedb, List<String> path_installed_singers )
#endif
                {
                    m_installed_singers = new List<SingerConfig>();
                    m_singer_configs = new List<SingerConfig>();
                    String map = fsys.combine( path_voicedb, "voice.map" );
                    if ( !fsys.isFileExists( map ) ) {
                        return;
                    }

                    // インストールされている歌手の情報を読み取る。miku.vvd等から。
                    for ( int j = 0; j < vec.size( path_installed_singers ); j++ ) {
                        String ipath = path_installed_singers[j];
#if DEBUG
                        sout.println( "SingerConfigSys#.ctor; path_installed_singers[" + j + "]=" + path_installed_singers[j] );
#endif
                        //TODO: ここでエラー起こる場合があるよ。SingerConfigSys::.ctor
                        //      実際にディレクトリがある場合にのみ，ファイルのリストアップをするようにした．
                        //      これで治っているかどうか要確認
                        if ( PortUtil.isDirectoryExists( ipath ) ) {
                            String[] vvds = PortUtil.listFiles( ipath, "*.vvd" );
                            if ( vvds.Length > 0 ) {
                                SingerConfig installed = SingerConfig.fromVvd( vvds[0], 0, 0 );
                                vec.add( m_installed_singers, installed );
                                break;
                            }
                        }
                    }

                    // voice.mapから、プログラムチェンジ、バンクセレクトと音源との紐付け情報を読み出す。
                    RandomAccessFile fs = null;
                    try {
                        fs = new RandomAccessFile( map, "r" );
                        List<byte> dat = new List<byte>();// byte[8];
                        vec.ensureCapacity( dat, 8 );
                        fs.seek( 0x20 );
                        for ( int language = 0; language < 0x80; language++ ) {
                            for ( int program = 0; program < 0x80; program++ ) {
                                fs.read( dat, 0, 8 );
                                long value = conv.make_int64_le( dat );
                                if ( value >= 1 ) {
                                    String vvd = fsys.combine( path_voicedb, "vvoice" + value + ".vvd" );
                                    SingerConfig item = SingerConfig.fromVvd( vvd, language, program );
                                    vec.add( m_singer_configs, item );
                                }
                            }
                        }
                    } catch ( Exception ex ) {
                        serr.println( "SingerConfigSys#.ctor; ex=" + ex );
                    } finally {
                        if ( fs != null ) {
                            try {
                                fs.close();
                            } catch ( Exception ex2 ) {
                                serr.println( "SingerConfigSys#.ctor; ex2=" + ex2 );
                            }
                        }
                    }

                    // m_singer_configsの情報から、m_installed_singersの歌唱言語情報を類推する
                    int size = vec.size( m_installed_singers );
                    for ( int i = 0; i < size; i++ ) {
                        SingerConfig sc = vec.get( m_installed_singers, i );
                        String searchid = sc.VOICEIDSTR;
                        int size2 = vec.size( m_singer_configs );
                        for ( int j = 0; j < size2; j++ ) {
                            SingerConfig sc2 = vec.get( m_singer_configs, j );
                            if ( str.compare( sc2.VOICEIDSTR, searchid ) ) {
                                sc.Language = sc2.Language;
                                break;
                            }
                        }
                    }
                }

                public void getInstalledSingers( List<SingerConfig> dst )
                {
                    int size = vec.size( m_installed_singers );
                    vec.clear( dst );
                    for ( int i = 0; i < size; i++ ) {
                        vec.add( dst, vec.get( m_installed_singers, i ) );
                    }
                }

                /// <summary>
                /// Gets the VsqID of program change.
                /// </summary>
                /// <param name="program_change"></param>
                /// <returns></returns>        
                public VsqID getSingerID( int language, int program )
                {
                    VsqID ret = new VsqID( 0 );
                    ret.type = VsqIDType.Singer;
                    SingerConfig sc = null;
                    for ( int i = 0; i < vec.size( m_singer_configs ); i++ ) {
                        SingerConfig itemi = vec.get( m_singer_configs, i );
                        if ( itemi.Language == language && itemi.Program == program ) {
                            sc = itemi;
                            break;
                        }
                    }
                    if ( sc == null ) {
                        sc = new SingerConfig();
                    }
                    ret.IconHandle = new IconHandle();
                    ret.IconHandle.IconID = "$0701" + str.format( sc.Language, 2, 16 ) + str.format( sc.Program, 2, 16 );
                    ret.IconHandle.IDS = sc.VOICENAME;
                    ret.IconHandle.Index = 0;
                    ret.IconHandle.Language = sc.Language;
                    ret.IconHandle.setLength( 1 );
                    ret.IconHandle.Original = sc.Language << 8 | sc.Program;
                    ret.IconHandle.Program = sc.Program;
                    ret.IconHandle.Caption = "";
                    return ret;
                }

                /// <summary>
                /// Gets the singer information of pecified program change.
                /// </summary>
                /// <param name="program_change"></param>
                /// <returns></returns>
                public SingerConfig getSingerInfo( int language, int program )
                {
                    int size = vec.size( m_installed_singers );
                    for ( int i = 0; i < size; i++ ) {
                        SingerConfig item = vec.get( m_installed_singers, i );
                        if ( item.Language == language && item.Program == program ) {
                            return item;
                        }
                    }
                    return null;
                }

                /// <summary>
                /// Gets the list of singer configs.
                /// </summary>
                /// <returns></returns>
                public void getSingerConfigs( List<SingerConfig> dst )
                {
                    vec.clear( dst );
                    int size = vec.size( m_singer_configs );
                    for ( int i = 0; i < size; i++ ) {
                        vec.add( dst, vec.get( m_singer_configs, i ) );
                    }
                }
            }

#if !JAVA
        }
    }
}
#endif
