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
using cadencii;
using cadencii.java.util;

namespace cadencii.apputil
{
#endif

    public class Messaging {
        private static String s_lang = "";
        private static Vector<MessageBody> s_messages = new Vector<MessageBody>();

        public static String[] getKeys( String lang ) {
            for( Iterator<MessageBody> itr = s_messages.iterator(); itr.hasNext(); ){
                MessageBody dict = itr.next();
                if ( lang.Equals( dict.lang ) ) {
                    Vector<String> list = new Vector<String>();
                    for ( Iterator<String> itr2 = dict.list.keySet().iterator(); itr2.hasNext(); ) {
                        String key = itr2.next();
                        list.add( key );
                    }
                    return list.toArray( new String[] { } );
                }
            }
            return null;
        }

        public static String[] getRegisteredLanguage() {
            Vector<String> res = new Vector<String>();
            for ( Iterator<MessageBody> itr = s_messages.iterator(); itr.hasNext(); ) {
                MessageBody dict = itr.next();
                res.add( dict.lang );
            }
            return res.toArray( new String[] { } );
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
            s_messages.clear();
            String[] files = PortUtil.listFiles( directory, ".po" );
            for ( int i = 0; i < files.Length; i++ ){
                String name = PortUtil.getFileName( files[i] );
                String fname = Path.Combine( directory, name );
                appendFromFile( fname );
            }
        }

        public static void appendFromFile( String file ) {
            s_messages.add( new MessageBody( PortUtil.getFileNameWithoutExtension( file ), file ) );
        }

        public static MessageBodyEntry getMessageDetail( String id ) {
            if ( s_lang.Equals( "" ) ) {
                s_lang = "en";
            }
            for ( Iterator<MessageBody> itr = s_messages.iterator(); itr.hasNext(); ){
                MessageBody mb = itr.next();
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
            for ( Iterator<MessageBody> itr = s_messages.iterator(); itr.hasNext(); ){
                MessageBody mb = itr.next();
                if ( mb.lang.Equals( s_lang ) ) {
                    return mb.getMessage( id );
                }
            }
            return id;
        }

        public static string getRuntimeLanguageName()
        {
            var name = System.Windows.Forms.Application.CurrentCulture.Name;
            if (name == "ja" || name.StartsWith("ja-")) {
                return "ja";
            } else {
                return name;
            }
        }
    }

#if !JAVA
}
#endif
