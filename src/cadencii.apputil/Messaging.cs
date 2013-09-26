/*
 * Messaging.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA

package cadencii.apputil;

import cadencii.*;
import java.util.Vector;
import java.util.Iterator;

#else

using System;
using System.IO;
using System.Collections.Generic;
using cadencii;
using cadencii.java.util;

namespace cadencii.apputil
{
#endif

    public class Messaging {
        private static String s_lang = "";
        private static List<MessageBody> s_messages = new List<MessageBody>();

        public static String[] getKeys( String lang ) {
            foreach (var dict in s_messages) {
                if ( lang.Equals( dict.lang ) ) {
                    List<String> list = new List<String>();
                    foreach (var key in dict.list.Keys) {
                        list.Add( key );
                    }
                    return list.ToArray();
                }
            }
            return null;
        }

        public static String[] getRegisteredLanguage() {
            List<String> res = new List<String>();
            foreach (var dict in s_messages) {
                res.Add( dict.lang );
            }
            return res.ToArray();
        }

        public static String getLanguage() {
            if ( !s_lang.Equals( "" ) ) {
                return s_lang;
            } else {
                s_lang = "en";
                return s_lang;
            }
        }

        public static void setLanguage( String value ) {
            if ( !value.Equals( "" ) ) {
                s_lang = value;
            } else {
                s_lang = "en";
            }
        }

        /// <summary>
        /// 現在の実行ディレクトリにある言語設定ファイルを全て読込み、メッセージリストに追加します
        /// </summary>
        public static void loadMessages() {
            loadMessages( PortUtil.getApplicationStartupPath() );
        }

        /// <summary>
        /// 指定されたディレクトリにある言語設定ファイルを全て読込み、メッセージリストに追加します
        /// </summary>
        /// <param name="directory"></param>
        public static void loadMessages( String directory ) {
            s_messages.Clear();
            String[] files = PortUtil.listFiles( directory, ".po" );
            for ( int i = 0; i < files.Length; i++ ){
                String name = PortUtil.getFileName( files[i] );
                String fname = Path.Combine( directory, name );
                appendFromFile( fname );
            }
        }

        public static void appendFromFile( String file ) {
            s_messages.Add( new MessageBody( PortUtil.getFileNameWithoutExtension( file ), file ) );
        }

        public static MessageBodyEntry getMessageDetail( String id ) {
            if ( s_lang.Equals( "" ) ) {
                s_lang = "en";
            }
            foreach (var mb in s_messages){
                if ( mb.lang.Equals( s_lang ) ) {
                    return mb.getMessageDetail( id );
                }
            }
            return new MessageBodyEntry( id, new String[] { } );
        }

        public static String getMessage( String id ) {
            if ( s_lang.Equals( "" ) ) {
                s_lang = "en";
            }
            foreach (var mb in s_messages) {
                if ( mb.lang.Equals( s_lang ) ) {
                    return mb.getMessage( id );
                }
            }
            return id;
        }
    }

#if !JAVA
}
#endif
