#define BINARYFILE_TO_BASE64
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

//using Boare.Lib.AppUtil;

namespace DevUtl {
    class Program {
        static void Main( string[] args ) {
#if BINARYFILE_TO_BASE64
            Console.WriteLine( "BINARYFILE_TO_BASE64" );
            if ( args.Length < 1 ) {
                Console.WriteLine( "error; too few arguments" );
                return;
            }
            if ( !File.Exists( args[0] ) ) {
                Console.WriteLine( "error; file not found" );
                return;
            }
            string str = "";
            using ( FileStream fs = new FileStream( args[0], FileMode.Open ) ) {
                byte[] b = new byte[fs.Length];
                fs.Read( b, 0, b.Length );
                str = Convert.ToBase64String( b );
            }
            int length = str.Length;
            int split_length = 100;
            Console.Write( "string foo = " );
            uint count = 0;
            while ( length > 0 ) {
                count++;
                string pref = "             ";
                if ( count == 1 ) {
                    pref = "";
                }
                if ( length < split_length ) {
                    Console.WriteLine( pref + "\"" + str + "\";" );
                    break;
                } else {
                    string part = str.Substring( 0, split_length );
                    str = str.Substring( split_length );
                    length = str.Length;
                    Console.WriteLine( pref + "\"" + part + "\" +" );
                }
            }
#endif
#if BINARYFILE_TO_BYTEARRAY
            Console.WriteLine( "BINARYFILE_TO_BYTEARRAY" );
            if ( args.Length < 2 ) {
                Console.WriteLine( "error; too few arguments" );
                return;
            }
            if ( !File.Exists( args[0] ) ) {
                Console.WriteLine( "error; file not found" );
                return;
            }
            byte[] hoge = new byte[] { 0x00, 0x01, };
            using ( StreamWriter sw = new StreamWriter( args[1], false, Encoding.UTF8 ) ) {
                sw.Write( "byte[] foo = new byte[] { " );
                bool first = true;
                using ( FileStream fs = new FileStream( args[0], FileMode.Open ) ) {
                    const int BUF = 20;
                    byte[] buffer = new byte[BUF];
                    while ( true ) {
                        int len = fs.Read( buffer, 0, BUF );
                        if ( len <= 0 ) {
                            break;
                        }
                        if ( first ) {
                            first = false;
                        } else {
                            sw.WriteLine();
                            sw.Write( "                          " );
                        }
                        for ( int i = 0; i < len; i++ ) {
                            sw.Write( "0x" + Convert.ToString( buffer[i], 16 ) + ", " );
                        }
                    }
                }
                sw.WriteLine( "};" );
            }
#else
#if LANGUAGE_FILE_CONVERSION
            Console.WriteLine( "LANGUAGE_FILE_CONVERSION" );
            //Console.WriteLine( "input the name of message definition file" );
            string msg_dat = @"C:\cvs\lipsync\LipSync\en.lang";//            Console.ReadLine();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            using ( StreamReader sr = new StreamReader( msg_dat ) ) {
                while ( sr.Peek() >= 0 ) {
                    string line = sr.ReadLine();
                    if ( line.StartsWith( "#" ) ) {
                        continue;
                    }
                    string[] spl = line.Split( "\t".ToCharArray() );
                    dict.Add( spl[0], spl[1] );
                }
            }

            while ( true ) {
                Console.WriteLine( "input edit target file" );
                string cs = Console.ReadLine();
                string new_file = Path.Combine( Path.GetDirectoryName( cs ), Path.GetFileNameWithoutExtension( cs ) + "_.tmp" );
                using ( StreamWriter sw = new StreamWriter( new_file ) )
                using ( StreamReader sr = new StreamReader( cs ) ) {
                    while ( sr.Peek() >= 0 ) {
                        sw.WriteLine( sr.ReadLine() );
                    }
                }

                using ( StreamWriter sw = new StreamWriter( cs ) )
                using ( StreamReader sr = new StreamReader( new_file ) ) {
                    while ( sr.Peek() >= 0 ) {
                        string line = sr.ReadLine();
                        int index = line.IndexOf( "Messaging.GetMessage( MessageID." );
                        if ( index >= 0 ) {
                            while ( index >= 0 ) {
                                int right = line.IndexOf( ")", index );
                                string item = line.Substring( index + 32, right - (index + 32) );
                                item = item.Trim();
                                Console.WriteLine( "item=\"" + item + "\"" );
                                string new_line = line.Substring( 0, index ) + "_( \"" + dict[item] + "\" )" + line.Substring( right + 1 );
                                line = new_line;
                                index = line.IndexOf( "Messaging.GetMessage( MessageID." );
                            }
                            sw.WriteLine( line );
                        } else {
                            sw.WriteLine( line );
                        }
                    }
                }
            }
#endif
#endif
        }
    }
}
