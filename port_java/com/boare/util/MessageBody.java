/*
 * MessageBody.java
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

public class MessageBody {
    public String lang;
    public String po_header = "";
    public TreeMap<String, MessageBodyEntry> list = new TreeMap<String, MessageBodyEntry>();
    
    class ReadTillMessageEndArg{
        public String msg;
        public String[] locations;
    }

    public MessageBody( String lang_ ) {
        lang = lang_;
    }

    public MessageBody( String lang, String[] ids, String[] messages ) {
        this.lang = lang;
        list = new TreeMap<String, MessageBodyEntry>();
        for( int i = 0; i < ids.length; i++ ) {
            list.put( ids[i], new MessageBodyEntry( messages[i], new String[] { } ) );
        }
    }

    public MessageBody( String lang_, String file ) {
        lang = lang_;
        po_header = "";
        try{
            StreamReader sr = new StreamReader( file, "UTF-8" );
            while ( true ) {
                String msgid;
                ReadTillMessageEndArg arg = new ReadTillMessageEndArg();
                String first_line = sr.readLine();
                if( first_line == null ){
                    break;
                }
                String[] location;
                String last_line = readTillMessageEnd( sr, first_line, "msgid", arg );
                msgid = arg.msg;
                location = arg.locations;
                String msgstr;
                String[] location_dumy;
                last_line = readTillMessageEnd( sr, last_line, "msgstr", arg );
                msgstr = arg.msg;
                location_dumy = arg.locations;
                if ( msgid.length() > 0 ) {
                    list.put( msgid, new MessageBodyEntry( msgstr, location ) );
                } else {
                    po_header = msgstr;
                    String[] spl = po_header.split( "\n" );
                    po_header = "";
                    int count = 0;
                    for( int i = 0; i < spl.length; i++ ){
                        String line = spl[i];
                        String[] spl2 = line.split( ":", 2 );
                        if ( spl2.length == 2 ) {
                            String name = spl2[0].trim();
                            if ( name.toLowerCase().equals( "Content-Type".toLowerCase() ) ) {
                                po_header += (count == 0 ? "" : "\n") + "Content-Type: text/plain; charset=UTF-8";
                            } else if ( name.toLowerCase().equals( "Content-Transfer-Encoding".toLowerCase() ) ) {
                                po_header += (count == 0 ? "" : "\n") + "Content-Transfer-Encoding: 8bit";
                            } else {
                                po_header += (count == 0 ? "" : "\n") + line;
                            }
                        } else {
                            po_header += (count == 0 ? "" : "\n") + line;
                        }
                        count++;
                    }
                }
            }
        }catch( Exception ex ){
            System.out.println( "MessageBody..ctor; ex=" + ex );
        }
    }

    public String getMessage( String id ) {
        if ( list.containsKey( id ) ) {
            String ret = list.get( id ).Message;
            if ( ret == null || (ret != null && ret.equals( "" )) ) {
                return id;
            } else {
                return ret;
            }
        }
        return id;
    }

    public MessageBodyEntry getMessageDetail( String id ) {
        if ( list.containsKey( id ) ) {
            String ret = list.get( id ).Message;
            if ( ret.equals( "" ) ) {
                return new MessageBodyEntry( id, new String[] { } );
            } else {
                return list.get( id );
            }
        }
        return new MessageBodyEntry( id, new String[] { } );
    }

    public void write( String file ) {
        try{
            BufferedWriter sw = new BufferedWriter( new FileWriter( file ) );
            if ( po_header != "" ) {
                sw.write( "msgid \"\"" );
                sw.newLine();
                sw.write( "msgstr \"\"" );
                sw.newLine();
                String[] spl = po_header.split( "\n" );
                for( int i = 0; i < spl.length; i++ ){
                    String line = spl[i];
                    sw.write( "\"" + line + "\\" + "n\"" );
                    sw.newLine();
                }
                sw.newLine();
            } else {
                sw.write( "msgid \"\"" );
                sw.newLine();
                sw.write( "msgstr \"\"" );
                sw.newLine();
                sw.write( "\"Content-Type: text/plain; charset=UTF-8\\" + "n\"" );
                sw.newLine();
                sw.write( "\"Content-Transfer-Encoding: 8bit\\" + "n\"" );
                sw.newLine();
                sw.newLine();
            }
            for( Iterator<String> itr = list.keySet().iterator(); itr.hasNext(); ){
                String key = (String)itr.next();
                String skey = key.replace( "\n", "\\n\"\n\"" );
                String s = list.get( key ).Message;
                Vector<String> location = list.get( key ).Location;
                for ( int i = 0; i < location.size(); i++ ) {
                    sw.write( "#: " + location.elementAt( i ) );
                    sw.newLine();
                }
                sw.write( "msgid \"" + skey + "\"" );
                sw.newLine();
                s = s.replace( "\n", "\\n\"\n\"" );
                sw.write( "msgstr \"" + s + "\"" );
                sw.newLine();
                sw.newLine();
            }
        }catch( Exception ex ){
        }
    }

    private static void separateEntryAndMessage( String source, StringBuilder entry, StringBuilder message ) {
        String line = source.trim();
        entry.setLength( 0 );
        message.setLength( 0 );
        if ( line.length() <= 0 ) {
            return;
        }
        int index_space = line.indexOf( ' ' );
        int index_dquoter = line.indexOf( '"' );
        int index = Math.min( index_dquoter, index_space );
        entry.append( line.substring( 0, index ) );
        message.append( line.substring( index_dquoter + 1 ) );
        String s = message.toString().substring( 0, message.length() - 1 );
        message.setLength( 0 );
        message.append( s );
    }

    private static String readTillMessageEnd( StreamReader sr, String first_line, String entry, ReadTillMessageEndArg arg ) throws IOException {
        arg.msg = "";
        String line = first_line;
        Vector<String> location = new Vector<String>();
        boolean entry_found = false;
        if ( line.startsWith( entry ) ) {
            // 1行目がすでに"entry"の行だった場合
            StringBuilder dum = new StringBuilder();
            StringBuilder dum2 = new StringBuilder();
            separateEntryAndMessage( line, dum, dum2 );
            arg.msg += dum2.toString();
        } else {
            while ( true ) {
                if ( line.startsWith( "#:" ) ) {
                    line = line.substring( 2 ).trim();
                    location.add( line );
                } else if ( line.startsWith( entry ) ) {
                    StringBuilder dum = new StringBuilder();
                    StringBuilder dum2 = new StringBuilder();
                    separateEntryAndMessage( line, dum, dum2 );
                    arg.msg += dum2.toString();
                    break;
                }
                line = sr.readLine();
                if( line == null ){
                    break;
                }
            }
        }
        arg.locations = location.toArray( new String[]{} );
        String ret = "";
        while ( true ) {
            line = sr.readLine();
            if( line == null ){
                break;
            }
            if ( !line.startsWith( "\"" ) ) {
                arg.msg = arg.msg.replace( "\\\"", "\"" );
                arg.msg = arg.msg.replace( "\\n", "\n" );
                return line;
            }
            int index = line.lastIndexOf( "\"" );
            arg.msg += line.substring( 1, index - 1 );
        }
        arg.msg = arg.msg.replace( "\\\"", "\"" );
        arg.msg = arg.msg.replace( "\\n", "\n" );
        return line;
    }
}
