using System;
using System.IO;

class ParseUtauPluginInvoker{
    public static void Main( string[] args ){
        using( StreamWriter sw = new StreamWriter( args[1] ) )
        using( StreamReader sr = new StreamReader( args[0] ) ){
            /*string text = tr.ReadToEnd();
            text = text.Replace( "\"", "\\\"" );
            text = text.Replace( "\n", @"\n"" +" + "\n\"" );
            text = text.Replace( "Utau_Plugin_Manager", "{0}" );
            text = text.Replace( "E:\\Program Files\\UTAU\\plugins\\picedit\\plugin.txt", "{1}" );
            sw.WriteLine( text );*/
            string line = "";
            while( (line = sr.ReadLine()) != null ){
                line = line.Replace( "\"", "\\\"" );
                line = line.Replace( "Utau_Plugin_Manager", "{0}" );
                line = line.Replace( "E:\\Program Files\\UTAU\\plugins\\picedit\\plugin.txt", "{1}" );
                sw.WriteLine( "        \"" + line + "\\n\" +" );
            }
        }
    }
}
