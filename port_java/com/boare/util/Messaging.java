/*
 * Messaging.java
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of com.boare.util.
 *
 * com.boare.util is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * com.boare.util is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.util;

import java.util.*;
import java.io.*;
import com.boare.corlib.*;

public class Messaging {
    private static String s_lang = "";
    private static Vector<MessageBody> s_messages = new Vector<MessageBody>();
    
    public static String[] getKeys( String lang ) {
        for( Iterator itr = s_messages.iterator(); itr.hasNext(); ){
            MessageBody dict = (MessageBody)itr.next();
            if ( lang.equals( dict.lang ) ) {
                Vector<String> list = new Vector<String>();
                for( Iterator itr2 = dict.list.keySet().iterator(); itr.hasNext(); ){
                    String key = (String)itr2.next();
                    list.add( key );
                }
                return list.toArray( new String[]{} );
            }
        }
        return null;
    }

    public static String[] getRegisteredLanguage() {
        Vector<String> res = new Vector<String>();
        for( Iterator itr = s_messages.iterator(); itr.hasNext(); ){
            MessageBody dict = (MessageBody)itr.next();
            res.add( dict.lang );
        }
        return res.toArray( new String[]{} );
    }

    public static String getLanguage() {
        if ( s_lang != "" ) {
            return s_lang;
        } else {
            s_lang = "en";
            return s_lang;
        }
    }
    
    public static void setLanguage( String value ){
        if ( value != "" ) {
            s_lang = value;
        } else {
            s_lang = "en";
        }
    }

    /// <summary>
    /// 現在の実行ディレクトリにある言語設定ファイルを全て読込み、メッセージリストに追加します
    /// </summary>
    public static void loadMessages() {
        loadMessages( "." );
    }

    /// <summary>
    /// 指定されたディレクトリにある言語設定ファイルを全て読込み、メッセージリストに追加します
    /// </summary>
    /// <param name="directory"></param>
    public static void loadMessages( String directory ) {
        s_messages.clear();
        File f = new File( directory );
		File[] list = f.listFiles( new FileFilterWithExtension( ".po" ) );
		for( int i = 0; i < list.length; i++ ){
			File fi = list[i];
            String fname = fi.getName();
            appendFromFile( fname );
        }
    }

    public static void appendFromFile( String file ) {
System.out.println( "Messaging.appendFromFile; file=" + file );
        s_messages.add( new MessageBody( Path.getFileNameWithoutExtension( file ), file ) );
    }

    public static MessageBodyEntry getMessageDetail( String id ) {
        if ( s_lang.equals( "" ) ) {
            s_lang = "en";
        }
        for( Iterator itr = s_messages.iterator(); itr.hasNext(); ){
            MessageBody mb = (MessageBody)itr.next();
            if ( mb.lang.equals( s_lang ) ) {
                return mb.getMessageDetail( id );
            }
        }
        return new MessageBodyEntry( id, new String[] { } );
    }

    public static String getMessage( String id ) {
        if ( s_lang.equals( "" ) ) {
            s_lang = "en";
        }
        for( Iterator itr = s_messages.iterator(); itr.hasNext(); ){
            MessageBody mb = (MessageBody)itr.next();
            if ( mb.lang.equals( s_lang ) ) {
                return mb.getMessage( id );
            }
        }
        return id;
    }
}
