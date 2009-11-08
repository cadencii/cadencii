using System;
using System.IO;

class ExtractEventHandlerAndResource{
    static string in_file = "";
    static string event_out_file = "";
    static string res_out_file = "";
    static string edited_out_file = "";

    public static void Main( string[] args ){
        string current = "";
        foreach( string s in args ){
            if( s.StartsWith( "-" ) ){
                current = s;
            }else{
                if( current == "" ){
                    in_file = s;
                }else if( current == "-e" ){
                    event_out_file = s;
                    current = "";
                }else if( current == "-r" ){
                    res_out_file = s;
                    current = "";
                }else if( current == "-o" ){
                    edited_out_file = s;
                    current = "";
                }
            }
        }
        
        if( in_file == "" || 
            (in_file != "" && event_out_file == "" && res_out_file == "") ||
            (in_file != "" && edited_out_file == "") ){
            Console.WriteLine( "extractEventHandlerAndResource:" );
            Console.WriteLine( "    [NONE]  input file" );
            Console.WriteLine( "    -o      output file for \"edited\" input file" );
            Console.WriteLine( "    -e      output file for event handlers" );
            Console.WriteLine( "    -r      output file for resources" );
            return;
        }
        
        string infile = args[0];
        Mode mode = Mode.DEFAULT;
        StreamWriter event_out = null;
        if( event_out_file != "" ){
            event_out = new StreamWriter( event_out_file );
            event_out.WriteLine( "private void registerEventHandlers(){" );
        }
        StreamWriter res_out = null;
        if( res_out_file != "" ){
            res_out = new StreamWriter( res_out_file );
            res_out.WriteLine( "private void setResources(){" );
        }
        StreamWriter edited_out = null;
        if( edited_out_file != "" ){
            edited_out = new StreamWriter( edited_out_file );
        }
        using( StreamReader sr = new StreamReader( infile ) ){
            string line = "";
            int bra_count = 0; // 中括弧の出現回数。"{"が出たら++, "}"が出たら--
            bool first_bra_found = false; //InitializeComponentを囲む最初の中括弧が出たかどうか
            while( (line = sr.ReadLine()) != null ){
                if( mode == Mode.DEFAULT ){
                    if( line.IndexOf( " void InitializeComponent" ) >= 0 ){
                        int i = countChar( line, '{' );
                        if( i > 0 ){
                            first_bra_found = true;
                        }
                        bra_count += i;
                        mode = Mode.READ_METHOD;
                    }
                    if( edited_out != null ){
                        edited_out.WriteLine( line );
                    }
                }else if( mode == Mode.READ_METHOD ){
                    //Console.WriteLine( "mode=" + mode + "; bra_count=" + bra_count );
                    int i = countChar( line, '{' );
                    if( !first_bra_found && i > 0 ){
                        first_bra_found = true;
                    }
                    bra_count += i;
                    bra_count -= countChar( line, '}' );
                    if( line.IndexOf( "EventHandler" ) >= 0 && line.IndexOf( "+=" ) >= 0 && line.IndexOf( "new" ) >= 0 ){
                        if( event_out != null ){
                            event_out.WriteLine( line );
                        }
                    }else if( line.IndexOf( "Resources." ) >= 0 ){
                        if( res_out != null ){
                            res_out.WriteLine( line );
                        }
                    }else{
                        if( edited_out != null ){
                            edited_out.WriteLine( line );
                        }
                    }
                    if( first_bra_found && bra_count == 0 ){
                        mode = Mode.DEFAULT;
                    }
                }
            }
        }
        
        if( event_out != null ){
            event_out.WriteLine( "}" );
            event_out.Close();
        }
        if( res_out != null ){
            res_out.WriteLine( "}" );
            res_out.Close();
        }
        if( edited_out != null ){
            edited_out.Close();
        }
    }
    
    private static int countChar( string s, char search ){
        int ret = 0;
        int index = 0;
        int found = s.IndexOf( search, index );
        while( found >= 0 ){
            ret++;
            index = found + 1;
            found = s.IndexOf( search, index );
        }
        return ret;
    }
}

enum Mode{
    DEFAULT,
    READ_METHOD,
}
