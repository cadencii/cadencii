/*
 * MessageBody.cs
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

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.util.Iterator;
import java.util.TreeMap;
import java.util.Vector;

import cadencii.ByRef;
import cadencii.PortUtil;
import cadencii.str;

#else

using System;
using cadencii;
using cadencii.java.util;
using cadencii.java.io;

namespace cadencii.apputil {
    using boolean = System.Boolean;
#endif

    public class MessageBody
    {
        public String lang;
        public String poHeader = "";
        public TreeMap<String, MessageBodyEntry> list = new TreeMap<String, MessageBodyEntry>();

        public MessageBody( String lang_ ) {
            lang = lang_;
        }

        public MessageBody( String lang, String[] ids, String[] messages ) {
            this.lang = lang;
            list = new TreeMap<String, MessageBodyEntry>();
            for( int i = 0; i < ids.Length; i++ ) {
                list.put( ids[i], new MessageBodyEntry( messages[i], new String[] { } ) );
            }
        }

        public MessageBody( String lang_, String file ) {
            lang = lang_;
            poHeader = "";
            BufferedReader sr = null;
            try {
                sr = new BufferedReader( new InputStreamReader( new FileInputStream( file ), "UTF-8" ) );
                String line2 = "";
                while ( (line2 = sr.readLine()) != null ) {
                    ByRef<String> msgid = new ByRef<String>( "" );
                    String first_line = line2;
                    ByRef<String[]> location = new ByRef<String[]>();
                    String last_line = readTillMessageEnd( sr, first_line, "msgid", msgid, location );
                    ByRef<String> msgstr = new ByRef<String>( "" );
                    ByRef<String[]> location_dumy = new ByRef<String[]>();
                    last_line = readTillMessageEnd( sr, last_line, "msgstr", msgstr, location_dumy );
                    if ( PortUtil.getStringLength( msgid.value ) > 0 ) {
                        list.put( msgid.value, new MessageBodyEntry( msgstr.value, location.value ) );
                    } else {
                        poHeader = msgstr.value;
                        String[] spl = PortUtil.splitString( poHeader, new char[] { (char)0x0d, (char)0x0a }, true );
                        poHeader = "";
                        int count = 0;
                        for ( int i = 0; i < spl.Length; i++ ) {
                            String line = spl[i];
                            String[] spl2 = PortUtil.splitString( line, new char[] { ':' }, 2 );
                            if ( spl2.Length == 2 ) {
                                String name = spl2[0].Trim();
                                String ct = "Content-Type";
                                String cte = "Content-Transfer-Encoding";
                                if ( name.ToLower().Equals( ct.ToLower() ) ) {
                                    poHeader += (count == 0 ? "" : "\n") + "Content-Type: text/plain; charset=UTF-8";
                                } else if ( name.ToLower().Equals( cte.ToLower() ) ) {
                                    poHeader += (count == 0 ? "" : "\n") + "Content-Transfer-Encoding: 8bit";
                                } else {
                                    poHeader += (count == 0 ? "" : "\n") + line;
                                }
                            } else {
                                poHeader += (count == 0 ? "" : "\n") + line;
                            }
                            count++;
                        }
                    }
                }
            } catch ( Exception ex ) {
            } finally {
                if ( sr != null ) {
                    try {
                        sr.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        public String getMessage( String id ) {
            if ( list.containsKey( id ) ) {
                String ret = list.get( id ).message;
                if ( ret.Equals( "" ) ) {
                    return id;
                } else {
                    return list.get( id ).message;
                }
            }
            return id;
        }

        public MessageBodyEntry getMessageDetail( String id ) {
            if ( list.containsKey( id ) ) {
                String ret = list.get( id ).message;
                if ( ret.Equals( "" ) ) {
                    return new MessageBodyEntry( id, new String[] { } );
                } else {
                    return list.get( id );
                }
            }
            return new MessageBodyEntry( id, new String[] { } );
        }

        public void write( String file ) {
            BufferedWriter sw = null;
            try {
                sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( file ), "UTF-8" ) );
                if ( !poHeader.Equals( "" ) ) {
                    sw.write( "msgid \"\"" );
                    sw.newLine();
                    sw.write( "msgstr \"\"" );
                    sw.newLine();
                    String[] spl = PortUtil.splitString( poHeader, new char[] { (char)0x0d, (char)0x0a }, true );
                    for ( int i = 0; i < spl.Length; i++ ){
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
                for ( Iterator<String> itr = list.keySet().iterator(); itr.hasNext(); ){
                    String key = itr.next();
                    String skey = key.Replace( "\n", "\\n\"\n\"" );
                    MessageBodyEntry mbe = list.get( key );
                    String s = mbe.message;
                    Vector<String> location = mbe.location;
                    int count = location.Count;
                    for ( int i = 0; i < count; i++ ) {
                        sw.write( "#: " + location[ i ] );
                        sw.newLine();
                    }
                    sw.write( "msgid \"" + skey + "\"" );
                    sw.newLine();
                    s = s.Replace( "\n", "\\n\"\n\"" );
                    sw.write( "msgstr \"" + s + "\"" );
                    sw.newLine();
                    sw.newLine();
                }
            } catch ( Exception ex ) {
            } finally {
                if ( sw != null ) {
                    try {
                        sw.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        private static void separateEntryAndMessage( String source, ByRef<String> entry, ByRef<String> message ) {
            String line = source.Trim();
            entry.value = "";
            message.value = "";
            if ( PortUtil.getStringLength( line ) <= 0 ) {
                return;
            }
            int index_space = line.IndexOf( ' ' );
            int index_dquoter = line.IndexOf( '"' );
            int index = Math.Min( index_dquoter, index_space );
            entry.value = line.Substring( 0, index );
            message.value = line.Substring( index_dquoter + 1 );
            message.value = message.value.Substring( 0, PortUtil.getStringLength( message.value ) - 1 );
        }

        private static String readTillMessageEnd( java.io.BufferedReader sr, String first_line, String entry, ByRef<String> msg, ByRef<String[]> locations )
#if JAVA
            throws IOException
#endif
        {
            msg.value = "";
            String line = first_line;
            Vector<String> location = new Vector<String>();
            boolean entry_found = false;
            if ( line.StartsWith( entry ) ) {
                // 1行目がすでに"entry"の行だった場合
                ByRef<String> dum = new ByRef<String>( "" );
                ByRef<String> dum2 = new ByRef<String>( "" );
                separateEntryAndMessage( line, dum, dum2 );
                msg.value += dum2.value;
            } else {
                while ( (line = sr.readLine()) != null ) {
                    if ( line.StartsWith( "#:" ) ) {
                        line = line.Substring( 2 ).Trim();
                        location.Add( line );
                    } else if ( line.StartsWith( entry ) ) {
                        ByRef<String> dum = new ByRef<String>( "" );
                        ByRef<String> dum2 = new ByRef<String>( "" );
                        separateEntryAndMessage( line, dum, dum2 );
                        msg.value += dum2.value;
                        break;
                    }
                }
            }
            locations.value = location.ToArray();
            String ret = "";
            while ( (line = sr.readLine()) != null ) {
                if ( !line.StartsWith( "\"" ) ) {
                    msg.value = msg.value.Replace( "\\\"", "\"" );
                    msg.value = msg.value.Replace( "\\n", "\n" );
                    return line;
                }
                int index = line.LastIndexOf( "\"" );
                msg.value += line.Substring( 1, index - 1 );
            }
            msg.value = msg.value.Replace( "\\\"", "\"" );
            msg.value = msg.value.Replace( "\\n", "\n" );
            if( line == null ){
                line = "";
            }
            return line;
        }
    }

#if !JAVA
}
#endif
