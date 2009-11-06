using System;
using System.IO;
using System.Text;

class GetStringFromText{
    public static void Main( string[] args ){
        using( StreamReader sr = new StreamReader( args[0] ) ){
            string line = "";
            Console.Write( "private static readonly String TEXT = \"\"" );
            while( (line = sr.ReadLine()) != null ){
                Console.WriteLine( " +" );
                line = line.Replace( "\\", "\\" + "\\" );
                line = line.Replace( "\"", "\\" + "\"" );
                Console.Write( "    \"" + line + "\"" );
            }
            Console.WriteLine( ";" );
        }
    }
}
