/*
 * Messaging.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Lib.AppUtil.
 *
 * Boare.Lib.AppUtil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * Boare.Lib.AppUtil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Boare.Lib.AppUtil {

    public static class Messaging {
        private static string s_lang = "";
        private static List<MessageBody> s_messages = new List<MessageBody>();
        
        public static string[] GetKeys( string lang ) {
            foreach ( MessageBody dict in s_messages ) {
                if ( lang == dict.lang ) {
                    List<string> list = new List<string>();
                    foreach ( string key in dict.list.Keys ) {
                        list.Add( key );
                    }
                    return list.ToArray();
                }
            }
            return null;
        }

        public static string[] GetRegisteredLanguage() {
            List<string> res = new List<string>();
            foreach ( MessageBody dict in s_messages ) {
                res.Add( dict.lang );
            }
            return res.ToArray();
        }

        public static string Language {
            get {
                if ( s_lang != "" ) {
                    return s_lang;
                } else {
                    s_lang = "en";
                    return s_lang;
                }
            }
            set {
                if ( value != "" ) {
                    s_lang = value;
                } else {
                    s_lang = "en";
                }
            }
        }

        /// <summary>
        /// 現在の実行ディレクトリにある言語設定ファイルを全て読込み、メッセージリストに追加します
        /// </summary>
        public static void LoadMessages() {
            LoadMessages( Application.StartupPath );
        }

        /// <summary>
        /// 指定されたディレクトリにある言語設定ファイルを全て読込み、メッセージリストに追加します
        /// </summary>
        /// <param name="directory"></param>
        public static void LoadMessages( string directory ) {
            DirectoryInfo current = new DirectoryInfo( directory );
            s_messages.Clear();
#if DEBUG
            Console.WriteLine( "Messaging+LoadMessages()" );
#endif
            foreach ( FileInfo fi in current.GetFiles( "*.po" ) ) {
                string fname = fi.FullName;
#if DEBUG
                Console.WriteLine( "    fname=" + fname );
#endif
                AppendFromFile( fname );
            }
        }

        public static void AppendFromFile( string file ) {
            s_messages.Add( new MessageBody( Path.GetFileNameWithoutExtension( file ), file ) );
        }

        public static MessageBodyEntry GetMessageDetail( string id ) {
            if ( s_lang.Length <= 0 ) {
                s_lang = "en";
            }
            foreach ( MessageBody mb in s_messages ) {
                if ( mb.lang == s_lang ) {
                    return mb.GetMessageDetail( id );
                }
            }
            return new MessageBodyEntry( id, new string[] { } );
        }

        public static string GetMessage( string id ) {
            if ( s_lang.Length <= 0 ) {
                s_lang = "en";
            }
            foreach ( MessageBody mb in s_messages ) {
                if ( mb.lang == s_lang ) {
                    return mb.GetMessage( id );
                }
            }
            return id;
        }
    }

}
