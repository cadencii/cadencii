using System;
using System.IO;

class diff2html{
    public static void Main( string[] args ){
        if( args.Length <= 0 ){
            return;
        }
        if( !File.Exists( args[0] ) ){
            return;
        }
        string infile = args[0];
        string outfile = Path.GetFileNameWithoutExtension( infile ) + ".html";
        using( StreamWriter sw = new StreamWriter( outfile ) )
        using( StreamReader sr = new StreamReader( infile ) ){
            sw.WriteLine( "<html>" );
            sw.WriteLine( "<body>" );
            sw.WriteLine( "<pre>" );
            string line = "";
            while( (line = sr.ReadLine()) != null ){
                string col_str = "";
                string col_back = "";
                if( line.StartsWith( "===" ) || line.StartsWith( "---" ) || line.StartsWith( "+++" ) ){
                    col_str = "#800000";
                    col_back = "#ffff80";
                }else if( line.StartsWith( "-" ) ){
                    col_back = "#ff8080";
                }else if( line.StartsWith( "+" ) ){
                    col_back = "#80ff80";
                }else if( line.StartsWith( "@" ) ){
                    col_str = "#ff0000";
                }
                string style = "";
                if( col_str != "" ){
                    style += "color: " + col_str + "; ";
                }
                if( col_back != "" ){
                    style += "background-color: " + col_back + "; ";
                }
                if( style == "" ){
                    sw.Write( "<code>" );
                }else{
                    sw.Write( "<code style='" + style + "'>" );
                }
                //sw.Write( line.Replace( "<", "&lt;" ).Replace( ">", "&gt;" ).Replace( "&", "&amp;" ) );
                sw.Write( line );
                sw.WriteLine( "</code>" );
            }
            sw.WriteLine( "</pre>" );
            sw.WriteLine( "</body>" );
            sw.WriteLine( "</html>" );
        }
    }
}