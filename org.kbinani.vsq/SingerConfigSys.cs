/*
 * SingerConfigSys.cs
 * Copyright © 2009-2010 kbinani
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
using System;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.java.io;

namespace org.kbinani.vsq {
#endif

    public class SingerConfigSys {
        public const int MAX_SINGERS = 0x4000;

        private Vector<SingerConfig> m_installed_singers = new Vector<SingerConfig>();
        private Vector<SingerConfig> m_singer_configs = new Vector<SingerConfig>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path_voicedb">音源のデータディレクトリ(ex:"C:\Program Files\VOCALOID2\voicedbdir")</param>
        /// <param name="path_installed_singers">音源のインストールディレクトリ(ex:new String[]{ "C:\Program Files\VOCALOID2\voicedbdir\BXXXXXXXXXXXXXXX", "D:\singers\BNXXXXXXXXXX" })</param>
        public SingerConfigSys( String path_voicedb, String[] path_installed_singers ) {
            m_installed_singers = new Vector<SingerConfig>();
            m_singer_configs = new Vector<SingerConfig>();
            String map = PortUtil.combinePath( path_voicedb, "voice.map" );
            if ( !PortUtil.isFileExists( map ) ) {
                return;
            }

            // インストールされている歌手の情報を読み取る。miku.vvd等から。
            for ( int j = 0; j < path_installed_singers.Length; j++ ) {
                String ipath = path_installed_singers[j];
#if DEBUG
                PortUtil.println( "SingerConfigSys#.ctor; path_installed_singers[" + j + "]=" + path_installed_singers[j] );
#endif
                //TODO: ここでエラー起こる場合があるよ。SingerConfigSys::.ctor
                //      実際にディレクトリがある場合にのみ，ファイルのリストアップをするようにした．
                //      これで治っているかどうか要確認
                if ( PortUtil.isDirectoryExists( ipath ) ) {
                    String[] vvds = PortUtil.listFiles( ipath, "*.vvd" );
                    if ( vvds.Length > 0 ) {
                        SingerConfig installed = SingerConfig.fromVvd( vvds[0], 0, 0 );
                        m_installed_singers.add( installed );
                        break;
                    }
                }
            }
            
            // voice.mapから、プログラムチェンジ、バンクセレクトと音源との紐付け情報を読み出す。
            RandomAccessFile fs = null;
            try {
                fs = new RandomAccessFile( map, "r" );
                byte[] dat = new byte[8];
                fs.seek( 0x20 );
                for ( int language = 0; language < 0x80; language++ ) {
                    for ( int program = 0; program < 0x80; program++ ) {
                        fs.read( dat, 0, 8 );
                        long value = PortUtil.make_int64_le( dat );
                        if ( value >= 1 ) {
                            String vvd = PortUtil.combinePath( path_voicedb, "vvoice" + value + ".vvd" );
                            SingerConfig item = SingerConfig.fromVvd( vvd, language, program );
                            m_singer_configs.add( item );
                        }
                    }
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "SingerConfigSys#.ctor; ex=" + ex );
            } finally {
                if ( fs != null ) {
                    try {
                        fs.close();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "SingerConfigSys#.ctor; ex2=" + ex2 );
                    }
                }
            }

            // m_singer_configsの情報から、m_installed_singersの歌唱言語情報を類推する
            for ( Iterator<SingerConfig> itr = m_installed_singers.iterator(); itr.hasNext(); ) {
                SingerConfig sc = itr.next();
                String searchid = sc.VOICEIDSTR;
                for ( Iterator<SingerConfig> itr2 = m_singer_configs.iterator(); itr2.hasNext(); ) {
                    SingerConfig sc2 = itr2.next();
                    if ( sc2.VOICEIDSTR.Equals( searchid ) ) {
                        sc.Language = sc2.Language;
                        break;
                    }
                }
            }
        }

        public SingerConfig[] getInstalledSingers() {
            return m_installed_singers.toArray( new SingerConfig[] { } );
        }

        /// <summary>
        /// Gets the VsqID of program change.
        /// </summary>
        /// <param name="program_change"></param>
        /// <returns></returns>        
        public VsqID getSingerID( int language, int program ) {
            VsqID ret = new VsqID( 0 );
            ret.type = VsqIDType.Singer;
            SingerConfig sc = null;
            for ( int i = 0; i < m_singer_configs.size(); i++ ) {
                SingerConfig itemi = m_singer_configs.get( i );
                if ( itemi.Language == language && itemi.Program == program ) {
                    sc = itemi;
                    break;
                }
            }
            if ( sc == null ) {
                sc = new SingerConfig();
            }
            ret.IconHandle = new IconHandle();
            ret.IconHandle.IconID = "$0701" + PortUtil.toHexString( sc.Language, 2 ) + PortUtil.toHexString( sc.Program, 2 );
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
        public SingerConfig getSingerInfo( int language, int program ) {
            for ( Iterator<SingerConfig> itr = m_installed_singers.iterator(); itr.hasNext(); ) {
                SingerConfig item = itr.next();
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
        public SingerConfig[] getSingerConfigs() {
            return m_singer_configs.toArray( new SingerConfig[] { } );
        }
    }

#if !JAVA
}
#endif
