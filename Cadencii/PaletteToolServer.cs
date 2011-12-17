#if ENABLE_SCRIPT
/*
 * PaletteToolServer.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using com.github.cadencii.apputil;
using com.github.cadencii.java.util;
using com.github.cadencii.vsq;
using com.github.cadencii.xml;

namespace com.github.cadencii {
    using boolean = System.Boolean;

    /// <summary>
    /// パレットツールを一元管理するクラス
    /// </summary>
    public static class PaletteToolServer {
        /// <summary>
        /// 読み込まれたパレットツールのコレクション
        /// </summary>
        public static TreeMap<String, Object> loadedTools = new TreeMap<String, Object>();

        /// <summary>
        /// パレットツールを読み込みます
        /// </summary>
        public static void init() {
            String path = Utility.getToolPath();
            if ( !Directory.Exists( path ) ) {
                return;
            }

            FileInfo[] files = new DirectoryInfo( path ).GetFiles( "*.txt" );
            foreach ( FileInfo file in files ) {
                String code = "";
                StreamReader sr = null;
                try {
                    sr = new StreamReader( file.FullName );
                    code += sr.ReadToEnd();
                } catch ( Exception ex ) {
                    Logger.write( typeof( PaletteToolServer ) + ".init; ex=" + ex + "\n" );
                } finally {
                    if ( sr != null ) {
                        try {
                            sr.Close();
                        } catch ( Exception ex2 ) {
                            Logger.write( typeof( PaletteToolServer ) + ".init; ex=" + ex2 + "\n" );
                        }
                    }
                }

                Assembly asm = null;
                Vector<String> errors = new Vector<String>();
                try {
                    asm = Utility.compileScript( code, errors );
                } catch ( Exception ex ) {
                    serr.println( "PaletteToolServer#init; ex=" + ex );
                    asm = null;
                }
                if ( asm == null ) {
                    continue;
                }

                if ( asm == null ) {
                    continue;
                }

                foreach ( Type t in asm.GetTypes() ) {
                    if ( t.IsClass && t.IsPublic && !t.IsAbstract && t.GetInterface( typeof( IPaletteTool ).FullName ) != null ) {
                        try {
#if DEBUG
                            AppManager.debugWriteLine( "t.FullName=" + t.FullName );
#endif
                            Object instance = asm.CreateInstance( t.FullName );
                            String dir = Path.Combine( Utility.getApplicationDataPath(), "tool" );
                            String cfg = Path.GetFileNameWithoutExtension( file.FullName ) + ".config";
                            String config = Path.Combine( dir, cfg );
                            if ( fsys.isFileExists( config ) ) {
                                XmlStaticMemberSerializer xsms = new XmlStaticMemberSerializer( instance.GetType() );
                                FileStream fs = null;
                                boolean errorOnDeserialize = false;
                                try {
                                    fs = new FileStream( config, FileMode.Open, FileAccess.Read );
                                    try {
                                        xsms.Deserialize( fs );
                                    } catch ( Exception ex ) {
                                        errorOnDeserialize = true;
                                        serr.println( "PaletteToolServer#init; ex=" + ex );
                                    }
                                } catch ( Exception ex ) {
                                    serr.println( "PaletteToolServer#init; ex=" + ex );
                                } finally {
                                    if ( fs != null ) {
                                        try {
                                            fs.Close();
                                        } catch ( Exception ex2 ) {
                                            serr.println( "PaletteToolServer#init; ex2=" + ex2 );
                                        }
                                    }
                                }
                                if ( errorOnDeserialize ) {
                                    try {
                                        PortUtil.deleteFile( config );
                                    } catch ( Exception ex ) {
                                        serr.println( "PaletteToolServer#init; ex=" + ex );
                                    }
                                }
                            }
                            String id = Path.GetFileNameWithoutExtension( file.FullName );
                            loadedTools.put( id, instance );
                        } catch ( Exception ex ) {
                            serr.println( "PlaetteToolServer#init; ex=" + ex );
                        }
                    }
                }
            }
        }

        public static String _( String id ) {
            return Messaging.getMessage( id );
        }

        /// <summary>
        /// パレットツールを実行します
        /// </summary>
        /// <param name="id">実行するパレットツールのID</param>
        /// <param name="track">編集対象のトラック番号</param>
        /// <param name="vsq_event_intrenal_ids">編集対象のInternalIDのリスト</param>
        /// <param name="button">パレットツールが押し下げられた時のマウスボタンの種類</param>
        /// <returns>パレットツールによって編集が加えられた場合true。そうでなければfalse(パレットツールがエラーを起こした場合も含む)。</returns>
        public static boolean invokePaletteTool( String id, int track, int[] vsq_event_intrenal_ids, MouseButtons button ) {
            if ( loadedTools.containsKey( id ) ) {
                VsqFileEx vsq = AppManager.getVsqFile();
                VsqTrack item = (VsqTrack)vsq.Track.get( track ).clone();
                Object objPal = loadedTools.get( id );
                if ( objPal == null ) {
                    return false;
                }
                if ( !(objPal is IPaletteTool) ) {
                    return false;
                }
                IPaletteTool pal = (IPaletteTool)objPal;
                boolean edited = false;
                try {
                    edited = pal.edit( item, vsq_event_intrenal_ids, button );
                } catch ( Exception ex ) {
                    AppManager.showMessageBox(
                        PortUtil.formatMessage( _( "Palette tool '{0}' reported an error.\nPlease copy the exception text and report it to developper." ), id ),
                        "Error",
                        com.github.cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                        com.github.cadencii.windows.forms.Utility.MSGBOX_ERROR_MESSAGE );
                    serr.println( typeof( PaletteToolServer ) + ".invokePaletteTool; ex=" + ex );
                    edited = false;
                }
                if ( edited ) {
                    CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( track, item, vsq.AttachedCurves.get( track - 1 ) );
                    AppManager.editHistory.register( vsq.executeCommand( run ) );
                }
                return edited;
            } else {
                return false;
            }
        }
    }

}
#endif
