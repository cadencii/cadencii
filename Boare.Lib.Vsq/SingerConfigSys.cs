/*
 * SingerConfigSys.cs
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
using System.Collections.Generic;
using System.IO;

using bocoree;

namespace Boare.Lib.Vsq {

    public class SingerConfigSys {
        private const int MAX_SINGERS = 0x4000;

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
            String map = Path.Combine( path_voicedb, "voice.map" );
            if ( !File.Exists( map ) ) {
                return;
            }
            RandomAccessFile fs = null;
            try {
                fs = new RandomAccessFile( map, "r" );
                byte[] dat = new byte[8];
                fs.seek( 0x20 );
                for ( int i = 0; i < MAX_SINGERS; i++ ) {
                    fs.read( dat, 0, 8 );
                    ulong value = VocaloSysUtil.makelong_le( dat );
                    if ( value >= 1 ) {
                        String vvd = Path.Combine( path_voicedb, "vvoice" + value + ".vvd" );
                        SingerConfig item = SingerConfig.fromVvd( vvd, 0 );
                        item.Program = i;

                        int original = -1;
                        for ( Iterator itr = m_installed_singers.iterator(); itr.hasNext(); ) {
                            SingerConfig sc = (SingerConfig)itr.next();
                            if ( sc.VOICEIDSTR.Equals( item.VOICEIDSTR ) ) {
                                original = sc.Program;
                                break;
                            }
                        }
                        if ( original < 0 ) {
                            foreach ( String ipath in path_installed_singers ) {
                                if ( ipath.EndsWith( item.VOICEIDSTR ) ) {
                                    String[] vvds = Directory.GetFiles( ipath, "*.vvd" );
                                    if ( vvds.Length > 0 ) {
                                        original = m_installed_singers.size();
                                        SingerConfig installed = SingerConfig.fromVvd( vvds[0], original );
                                        installed.Program = original;
                                        m_installed_singers.add( installed );
                                        break;
                                    }
                                }
                            }
                        }

                        item.Original = original;
                        m_singer_configs.add( item );
                    }
                }
            } catch ( Exception ex ) {
                if ( fs != null ) {
                    try {
                        fs.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        public SingerConfig[] getInstalledSingers() {
            return m_installed_singers.toArray( new SingerConfig[]{} );
        }

        /// <summary>
        /// Gets the VsqID of program change.
        /// </summary>
        /// <param name="program_change"></param>
        /// <returns></returns>        
        public VsqID getSingerID( String singer ) {
            VsqID ret = new VsqID( 0 );
            ret.type = VsqIDType.Singer;
            SingerConfig sc = null;
            for ( int i = 0; i < m_singer_configs.size(); i++ ) {
                if ( m_singer_configs.get( i ).VOICENAME.Equals( singer ) ) {
                    sc = m_singer_configs.get( i );
                    break;
                }
            }
            if ( sc == null ) {
                sc = new SingerConfig();
            }
            int lang = 0;
            for( Iterator itr = m_installed_singers.iterator(); itr.hasNext(); ){
                SingerConfig sc2 = (SingerConfig)itr.next();
                if ( sc.VOICEIDSTR.Equals( sc2.VOICEIDSTR ) ) {
                    lang = (int)VocaloSysUtil.getLanguageFromName( sc.VOICENAME );
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
        
        /// <summary>
        /// Gets the singer information of pecified program change.
        /// </summary>
        /// <param name="program_change"></param>
        /// <returns></returns>
        public SingerConfig getSingerInfo( String singer ) {
            for ( Iterator itr = m_installed_singers.iterator(); itr.hasNext(); ){
                SingerConfig item = (SingerConfig)itr.next();
                if ( item.VOICENAME.Equals( singer ) ) {
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
            return m_singer_configs.toArray( new SingerConfig[]{} );
        }
    }

}
