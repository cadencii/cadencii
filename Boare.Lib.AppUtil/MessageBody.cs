/*
 * MessageBody.cs
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
using System.Text;
using System.IO;

namespace Boare.Lib.AppUtil {

    public class MessageBody {
        public string lang;
        public string po_header = "";
        public Dictionary<string, MessageBodyEntry> list = new Dictionary<string, MessageBodyEntry>();

        public MessageBody( string lang_ ) {
            lang = lang_;
        }

        public MessageBody( string lang, string[] ids, string[] messages ) {
            this.lang = lang;
            list = new Dictionary<string, MessageBodyEntry>();
            for( int i = 0; i < ids.Length; i++ ) {
                list.Add( ids[i], new MessageBodyEntry( messages[i], new string[] { } ) );
            }
        }

        public MessageBody( string lang_, string file ) {
            lang = lang_;
            po_header = "";
            using ( StreamReader sr = new StreamReader( file ) ) {
                while ( sr.Peek() >= 0 ) {
                    string msgid;
                    string first_line = sr.ReadLine();
                    string[] location;
                    string last_line = ReadTillMessageEnd( sr, first_line, "msgid", out msgid, out location );
                    string msgstr;
                    string[] location_dumy;
                    last_line = ReadTillMessageEnd( sr, last_line, "msgstr", out msgstr, out location_dumy );
                    if ( msgid.Length > 0 ) {
                        if ( list.ContainsKey( msgid ) ) {
                            list[msgid] = new MessageBodyEntry( msgstr, location );
                        } else {
                            list.Add( msgid, new MessageBodyEntry( msgstr, location ) );
                        }
                    } else {
                        po_header = msgstr;
                        string[] spl = po_header.Split( new char[] { (char)0x0d, (char)0x0a }, StringSplitOptions.RemoveEmptyEntries );
                        po_header = "";
                        int count = 0;
                        foreach ( string line in spl ) {
                            string[] spl2 = line.Split( new char[] { ':' }, 2 );
                            if ( spl2.Length == 2 ) {
                                string name = spl2[0].Trim();
                                if ( name.ToLower() == "Content-Type".ToLower() ) {
                                    po_header += (count == 0 ? "" : "\n") + "Content-Type: text/plain; charset=UTF-8";
                                } else if ( name.ToLower() == "Content-Transfer-Encoding".ToLower() ) {
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
            }
#if DEBUG
            Console.WriteLine( "MessageBody..ctor; po_header=" + po_header );
#endif
        }

        public string GetMessage( string id ) {
            if ( list.ContainsKey( id ) ) {
                string ret = list[id].Message;
                if ( ret == "" ) {
                    return id;
                } else {
                    return list[id].Message;
                }
            }
            return id;
        }

        public MessageBodyEntry GetMessageDetail( string id ) {
            if ( list.ContainsKey( id ) ) {
                string ret = list[id].Message;
                if ( ret == "" ) {
                    return new MessageBodyEntry( id, new string[] { } );
                } else {
                    return list[id];
                }
            }
            return new MessageBodyEntry( id, new string[] { } );
        }

        public void Write( string file ) {
            using ( StreamWriter sw = new StreamWriter( file ) ) {
                if ( po_header != "" ) {
                    sw.WriteLine( "msgid \"\"" );
                    sw.WriteLine( "msgstr \"\"" );
                    string[] spl = po_header.Split( new char[] { (char)0x0d, (char)0x0a }, StringSplitOptions.RemoveEmptyEntries );
                    foreach ( string line in spl ) {
                        sw.WriteLine( "\"" + line + "\\" + "n\"" );
                    }
                    sw.WriteLine();
                } else {
                    sw.WriteLine( "msgid \"\"" );
                    sw.WriteLine( "msgstr \"\"" );
                    sw.WriteLine( "\"Content-Type: text/plain; charset=UTF-8\\" + "n\"" );
                    sw.WriteLine( "\"Content-Transfer-Encoding: 8bit\\" + "n\"" );
                    sw.WriteLine();
                }
                foreach ( string key in list.Keys ) {
                    string skey = key.Replace( "\n", "\\n\"\n\"" );
                    string s = list[key].Message;
                    List<string> location = list[key].Location;
                    for ( int i = 0; i < location.Count; i++ ) {
                        sw.WriteLine( "#: " + location[i] );
                    }
                    sw.WriteLine( "msgid \"" + skey + "\"" );
                    s = s.Replace( "\n", "\\n\"\n\"" );
                    sw.WriteLine( "msgstr \"" + s + "\"" );
                    sw.WriteLine();
                }
            }
        }

        private static void SeparateEntryAndMessage( string source, out string entry, out string message ) {
            string line = source.Trim();
            entry = "";
            message = "";
            if ( line.Length <= 0 ) {
                return;
            }
            int index_space = line.IndexOf( ' ' );
            int index_dquoter = line.IndexOf( '"' );
            int index = Math.Min( index_dquoter, index_space );
            entry = line.Substring( 0, index );
            message = line.Substring( index_dquoter + 1 );
            message = message.Substring( 0, message.Length - 1 );
        }

        private static string ReadTillMessageEnd( StreamReader sr, string first_line, string entry, out string msg, out string[] locations ) {
            msg = "";
            string line = first_line;
            List<string> location = new List<string>();
            bool entry_found = false;
            if ( line.StartsWith( entry ) ) {
                // 1行目がすでに"entry"の行だった場合
                string dum, dum2;
                SeparateEntryAndMessage( line, out dum, out dum2 );
                msg += dum2;
            } else {
                while ( true ) {
                    if ( line.StartsWith( "#:" ) ) {
                        line = line.Substring( 2 ).Trim();
                        location.Add( line );
                    } else if ( line.StartsWith( entry ) ) {
                        string dum, dum2;
                        SeparateEntryAndMessage( line, out dum, out dum2 );
                        msg += dum2;
                        break;
                    }
                    if ( sr.Peek() >= 0 ) {
                        line = sr.ReadLine();
                    } else {
                        break;
                    }
                }
            }
            locations = location.ToArray();
            string ret = "";
            while ( sr.Peek() >= 0 ) {
                line = sr.ReadLine();
                if ( !line.StartsWith( "\"" ) ) {
                    msg = msg.Replace( "\\\"", "\"" );
                    msg = msg.Replace( "\\n", "\n" );
                    return line;
                }
                int index = line.LastIndexOf( "\"" );
                msg += line.Substring( 1, index - 1 );
            }
            msg = msg.Replace( "\\\"", "\"" );
            msg = msg.Replace( "\\n", "\n" );
            return line;
        }
    }

}
