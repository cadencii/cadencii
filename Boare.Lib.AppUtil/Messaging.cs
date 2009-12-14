/*
 * Messaging.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of org.kbinani.apputil.
 *
 * org.kbinani.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.apputil;

import org.kbinani.*;
import java.util.*;
#else
using System;
using bocoree;
using bocoree.java.util;

namespace org.kbinani.apputil {
#endif

    public class Messaging {
        private static String s_lang = "";
        private static Vector<MessageBody> s_messages = new Vector<MessageBody>();
        
        public static String[] getKeys( String lang ) {
            for( Iterator itr = s_messages.iterator(); itr.hasNext(); ){
                MessageBody dict = (MessageBody)itr.next();
                if ( lang.Equals( dict.lang ) ) {
                    Vector<String> list = new Vector<String>();
                    for ( Iterator itr2 = dict.list.keySet().iterator(); itr2.hasNext(); ) {
                        String key = (String)itr2.next();
                        list.add( key );
                    }
                    return list.toArray( new String[] { } );
                }
            }
            return null;
        }

        public static String[] getRegisteredLanguage() {
            Vector<String> res = new Vector<String>();
            for ( Iterator itr = s_messages.iterator(); itr.hasNext(); ) {
                MessageBody dict = (MessageBody)itr.next();
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
#if DEBUG
            Console.WriteLine( "Messaging+LoadMessages()" );
#endif
            String[] files = PortUtil.listFiles( directory, ".po" );
            for ( int i = 0; i < files.Length; i++ ){
                String fname = PortUtil.combinePath( directory, files[i] );
#if DEBUG
                Console.WriteLine( "    fname=" + fname );
#endif
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
            for ( Iterator itr = s_messages.iterator(); itr.hasNext(); ){
                MessageBody mb = (MessageBody)itr.next();
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
            for ( Iterator itr = s_messages.iterator(); itr.hasNext(); ){
                MessageBody mb = (MessageBody)itr.next();
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
