using System;
using System.IO;

class parseBinFile{
    static Parser s_parser = Parser.uint32;

    public static void Main( string[] args ){
        string infile = "";
        string outfile = "";
        string current = "";
        foreach( string s in args ){
            if( current == "" ){
                if( s.StartsWith( "-" ) ){
                    if( s == "-int32" ){
                        s_parser = Parser.int32;
                        current = "";
                    }else if( s == "-uint32" ){
                        s_parser = Parser.uint32;
                        current = "";
                    }else if( s == "-int8" ){
                        s_parser = Parser.int8;
                        current = "";
                    }else if( s == "-uint8" ){
                        s_parser = Parser.uint8;
                        current = "";
                    }else if( s == "-int16" ){
                        s_parser = Parser.int16;
                        current = "";
                    }else if( s == "-uint16" ){
                        s_parser = Parser.uint16;
                        current = "";
                    }else if( s == "-int64" ){
                        s_parser = Parser.int64;
                        current = "";
                    }else if( s == "-uint64" ){
                        s_parser = Parser.uint64;
                        current = "";
                    }else if( s == "-float32" ){
                        s_parser = Parser.float32;
                        current = "";
                    }else if( s == "-float64" ){
                        s_parser = Parser.float64;
                        current = "";
                    }else{
                        current = s;
                    }
                }else{
                    outfile = s;
                }
            }else if( current == "-i" ){
                infile = s;
                current = "";
            }
        }
        
        Console.WriteLine( "parser=" + s_parser );
        Console.WriteLine( "infile=" + infile );
        Console.WriteLine( "outfile=" + outfile );
        int BUFLEN = 512;
        byte[] buf = new byte[BUFLEN];
        using( StreamWriter sw = new StreamWriter( outfile ) )
        using( FileStream fs = new FileStream( infile, FileMode.Open, FileAccess.Read ) ){
            while( true ){
                int len = fs.Read( buf, 0, BUFLEN );
                if( len <= 0 ){
                    break;
                }else{
                    int pos = 0;
                    while( pos < len ){
                        switch( s_parser ){
                            case Parser.int8:{
                                byte b = buf[pos];
        //mada
                                pos++;
                                break;
                            }
                            case Parser.uint8:{
                                sw.WriteLine( buf[pos] );
                                pos++;
                                break;
                            }
                            case Parser.int16:{
                                sw.WriteLine( BitConverter.ToInt16( buf, pos ) );
                                pos += 2;
                                break;
                            }
                            case Parser.uint16:{
                                sw.WriteLine( BitConverter.ToUInt16( buf, pos ) );
                                pos += 2;
                                break;
                            }
                            case Parser.int32:{
                                sw.WriteLine( BitConverter.ToInt32( buf, pos ) );
                                pos += 4;
                                break;
                            }
                            case Parser.uint32:{
                                sw.WriteLine( BitConverter.ToUInt32( buf, pos ) );
                                pos += 4;
                                break;
                            }
                            case Parser.int64:{
                                sw.WriteLine( BitConverter.ToInt64( buf, pos ) );
                                pos += 8;
                                break;
                            }
                            case Parser.uint64:{
                                sw.WriteLine( BitConverter.ToUInt64( buf, pos ) );
                                pos += 8;
                                break;
                            }
                            case Parser.float32:{
                                sw.WriteLine( BitConverter.ToSingle( buf, pos ) );
                                pos += 4;
                                break;
                            }
                            case Parser.float64:{
                                sw.WriteLine( BitConverter.ToDouble( buf, pos ) );
                                pos += 8;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}

enum Parser{
    int8,
    uint8,
    int16,
    uint16,
    int32,
    uint32,
    int64,
    uint64,
    float32,
    float64,
}
