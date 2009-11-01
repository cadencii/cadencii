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
            sw.WriteLine( "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">" );
            sw.WriteLine( "<html lang=\"ja-JP\">" );
            sw.WriteLine( "<head>" );
            sw.WriteLine( "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">" );
            sw.WriteLine( "<meta http-equiv=\"Content-Style-Type\" content=\"text/css\">" );
            sw.WriteLine( "<title>" + Path.GetFileNameWithoutExtension( infile ) + "</title>" );
            sw.WriteLine( "<style type=\"text/css\">" );
            sw.WriteLine( "<!--" );
            sw.WriteLine( ".header{" );
            sw.WriteLine( "    color: #800000;" );
            sw.WriteLine( "    background-color: #ffff80;" );
            sw.WriteLine( "}" );
            sw.WriteLine( ".removed{" );
            sw.WriteLine( "    background-color: #ff8080;" );
            sw.WriteLine( "}" );
            sw.WriteLine( ".added{" );
            sw.WriteLine( "    background-color: #80ff80;" );
            sw.WriteLine( "}" );
            sw.WriteLine( ".location{" );
            sw.WriteLine( "    color: #ff0000;" );
            sw.WriteLine( "}" );
            sw.WriteLine( "-->" );
            sw.WriteLine( "</style>" );
            sw.WriteLine( "</head>" );
            sw.WriteLine( "<body>" );
            sw.WriteLine( "<pre>" );
            string line = "";
            while( (line = sr.ReadLine()) != null ){
                if( line.Trim() == "" ){
                    sw.WriteLine( "<br>" );
                    continue;
                }
                string style_class = "";
                if( line.StartsWith( "===" ) || line.StartsWith( "---" ) || line.StartsWith( "+++" ) ){
                    style_class = "header";
                }else if( line.StartsWith( "-" ) ){
                    style_class = "removed";
                }else if( line.StartsWith( "+" ) ){
                    style_class = "added";
                }else if( line.StartsWith( "@" ) ){
                    style_class = "location";
                }
                if( style_class == "" ){
                    sw.Write( "<code>" );
                }else{
                    sw.Write( "<code class=" + style_class + ">" );
                }
                sw.Write( line.Replace( "<", "&lt;" ).Replace( ">", "&gt;" ).Replace( "&", "&amp;" ) );
                sw.WriteLine( "</code>" );
            }
            sw.WriteLine( "</pre>" );
            sw.WriteLine( "</body>" );
            sw.WriteLine( "</html>" );
        }
    }
}
