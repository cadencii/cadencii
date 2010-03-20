#if ENABLE_SCRIPT
/*
 * ScriptServer.cs
 * Copyright (C) 2010 kbinani
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
using org.kbinani;
using org.kbinani.apputil;
using org.kbinani.java.io;
using org.kbinani.java.util;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;

    public static class ScriptServer {
        private static TreeMap<String, ScriptInvoker> scripts = new TreeMap<String, ScriptInvoker>();

        /// <summary>
        /// 指定したIDのスクリプトを再読込みするか、または新規の場合読み込んで追加します。
        /// </summary>
        /// <param name="id"></param>
        public static void reload( String id ) {
            String dir = Utility.getScriptPath();
            String file = PortUtil.combinePath( dir, id );
            if ( !PortUtil.isFileExists( file ) ) {
                return;
            }

            ScriptInvoker si = Utility.loadScript( file );
            scripts.put( id, si );
        }

        /// <summary>
        /// スクリプトを読み込み、コンパイルします。
        /// </summary>
        public static void reload() {
            // 拡張子がcs, txtのファイルを列挙
            String dir = Utility.getScriptPath();
            Vector<String> files = new Vector<String>();
            files.addAll( Arrays.asList( PortUtil.listFiles( dir, ".txt" ) ) );
            files.addAll( Arrays.asList( PortUtil.listFiles( dir, ".cs" ) ) );

            // 既存のスクリプトに無いまたは新しいやつはロード。
            Vector<String> added = new Vector<String>(); //追加または更新が行われたスクリプトのID
            foreach ( String file in files ) {
                string id = PortUtil.getFileName( file );
                double time = PortUtil.getFileLastModified( file );
                added.add( id );

                boolean loadthis = true;
                if ( scripts.containsKey( id ) ) {
                    double otime = scripts.get( id ).fileTimestamp;
                    if ( time <= otime ) {
                        // 前回コンパイルした時点でのスクリプトファイルよりも更新日が同じか古い。
                        loadthis = false;
                    }
                }

                // ロードする処理
                if ( !loadthis ) {
                    continue;
                }

                ScriptInvoker si = Utility.loadScript( file );
                scripts.put( id, si );
            }

            // 削除されたスクリプトがあれば登録を解除する
            boolean changed = true;
            while ( changed ) {
                changed = false;
                for ( Iterator<String> itr = scripts.keySet().iterator(); itr.hasNext(); ) {
                    String id = itr.next();
                    if ( !added.contains( id ) ) {
                        scripts.remove( id );
                        changed = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// スクリプトを実行します。
        /// </summary>
        /// <param name="evsd"></param>
        public static boolean invokeScript( String id, VsqFileEx vsq ) {
            ScriptInvoker script_invoker = null;
            if ( scripts.containsKey( id ) ) {
                script_invoker = scripts.get( id );
            } else {
                return false;
            }
            if ( script_invoker != null && script_invoker.scriptDelegate != null ) {
                try {
                    VsqFileEx work = (VsqFileEx)vsq.clone();
                    ScriptReturnStatus ret = ScriptReturnStatus.ERROR;
                    if ( script_invoker.scriptDelegate is EditVsqScriptDelegate ) {
                        boolean b_ret = ((EditVsqScriptDelegate)script_invoker.scriptDelegate).Invoke( work );
                        if ( b_ret ) {
                            ret = ScriptReturnStatus.EDITED;
                        } else {
                            ret = ScriptReturnStatus.ERROR;
                        }
                    } else if ( script_invoker.scriptDelegate is EditVsqScriptDelegateEx ) {
                        boolean b_ret = ((EditVsqScriptDelegateEx)script_invoker.scriptDelegate).Invoke( work );
                        if ( b_ret ) {
                            ret = ScriptReturnStatus.EDITED;
                        } else {
                            ret = ScriptReturnStatus.ERROR;
                        }
                    } else if ( script_invoker.scriptDelegate is EditVsqScriptDelegateWithStatus ) {
                        ret = ((EditVsqScriptDelegateWithStatus)script_invoker.scriptDelegate).Invoke( work );
                    } else if ( script_invoker.scriptDelegate is EditVsqScriptDelegateExWithStatus ) {
                        ret = ((EditVsqScriptDelegateExWithStatus)script_invoker.scriptDelegate).Invoke( work );
                    } else {
                        ret = ScriptReturnStatus.ERROR;
                    }
                    if ( ret == ScriptReturnStatus.ERROR ) {
                        AppManager.showMessageBox( _( "Script aborted" ), "Cadencii", PortUtil.MSGBOX_DEFAULT_OPTION, PortUtil.MSGBOX_INFORMATION_MESSAGE );
                    } else if ( ret == ScriptReturnStatus.EDITED ) {
                        CadenciiCommand run = VsqFileEx.generateCommandReplace( work );
                        AppManager.register( vsq.executeCommand( run ) );
                    }
                    String config_file = configFileNameFromScriptFileName( script_invoker.ScriptFile );
                    FileOutputStream fs = null;
                    boolean delete_xml_when_exit = false; // xmlを消すときtrue
                    try {
                        fs = new FileOutputStream( config_file );
                        script_invoker.Serializer.serialize( fs, null );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "AppManager#invokeScript; ex=" + ex );
                        delete_xml_when_exit = true;
                    } finally {
                        if ( fs != null ) {
                            try {
                                fs.close();
                                if ( delete_xml_when_exit ) {
                                    PortUtil.deleteFile( config_file );
                                }
                            } catch ( Exception ex2 ) {
                                PortUtil.stderr.println( "AppManager#invokeScript; ex2=" + ex2 );
                            }
                        }
                    }
                    return (ret == ScriptReturnStatus.EDITED);
                } catch ( Exception ex ) {
                    AppManager.showMessageBox( _( "Script runtime error:" ) + " " + ex, _( "Error" ), PortUtil.MSGBOX_DEFAULT_OPTION, PortUtil.MSGBOX_INFORMATION_MESSAGE );
                    PortUtil.stderr.println( "AppManager#invokeScript; ex=" + ex );
                }
            } else {
                AppManager.showMessageBox( _( "Script compilation failed." ), _( "Error" ), PortUtil.MSGBOX_DEFAULT_OPTION, PortUtil.MSGBOX_WARNING_MESSAGE );
            }
            return false;
        }

        /// <summary>
        /// スクリプトのファイル名から、そのスクリプトの設定ファイルの名前を決定します。
        /// </summary>
        /// <param name="script_file"></param>
        /// <returns></returns>
        public static String configFileNameFromScriptFileName( String script_file ) {
            String dir = PortUtil.combinePath( Utility.getApplicationDataPath(), "script" );
            if ( !PortUtil.isDirectoryExists( dir ) ) {
                PortUtil.createDirectory( dir );
            }
            return PortUtil.combinePath( dir, PortUtil.getFileNameWithoutExtension( script_file ) + ".config" );
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        /// <summary>
        /// 読み込まれたスクリプトのIDを順に返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        public static Iterator<String> getScriptIdIterator() {
            return scripts.keySet().iterator();
        }

        /// <summary>
        /// 指定したIDが示すスクリプトの、表示上の名称を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String getDisplayName( String id ) {
            if ( scripts.containsKey( id ) ) {
                ScriptInvoker invoker = scripts.get( id );
                if ( invoker.getDisplayNameDelegate != null ) {
                    String ret = "";
                    try {
                        ret = invoker.getDisplayNameDelegate();
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "ScriptServer#getDisplayName; ex=" + ex );
                        ret = PortUtil.getFileNameWithoutExtension( id );
                    }
                    return ret;
                }
            }
            return PortUtil.getFileNameWithoutExtension( id );
        }

        /// <summary>
        /// 指定したIDが示すスクリプトの、コンパイルした時点でのソースコードの更新日を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static double getTimestamp( String id ) {
            if ( scripts.containsKey( id ) ) {
                return scripts.get( id ).fileTimestamp;
            } else {
                return 0;
            }
        }

        /// <summary>
        /// 指定したIDが示すスクリプトが利用可能かどうかを表すbool値を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static boolean isAvailable( String id ) {
            if ( scripts.containsKey( id ) ) {
                return scripts.get( id ).scriptDelegate != null;
            } else {
                return false;
            }
        }

        /// <summary>
        /// 指定したIDが示すスクリプトの、コンパイル時のメッセージを取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String getCompileMessage( String id ) {
            if ( scripts.containsKey( id ) ) {
                return scripts.get( id ).ErrorMessage;
            } else {
                return "";
            }
        }
    }

}

#endif
