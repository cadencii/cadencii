using System;
using System.IO;
//using System.Drawing;

class makeRes{
    static string infile = "";
    static string outfile = "";
    static string package = "";
    static string name_space = "";

    public static void Main( string[] args ){
        // 引数を解釈
        string current = "";
        foreach( string s in args ){
            if( s.StartsWith( "-" ) ){
                current = s;
            }else{
                if( current == "-i" ){
                    infile = s;
                    current = "";
                }else if( current == "-o" ){
                    outfile = s;
                    current = "";
                }else if( current == "-p" ){
                    package = s;
                    current = "";
                }else if( current == "-n" ){
                    name_space = s;
                    current = "";
                }
            }
        }

        if( infile == "" || outfile == "" ){
            Console.WriteLine( "makeRes:" );
            Console.WriteLine( "    -i    input file" );
            Console.WriteLine( "    -o    output file" );
            Console.WriteLine( "    -p    package name [optional]" );
            Console.WriteLine( "    -n    namespace [optional]" );
            return;
        }
        if( !File.Exists( infile ) ){
            Console.WriteLine( "error; input file does not exists" );
            return;
        }

        using( StreamWriter sw = new StreamWriter( outfile ) )
        using( StreamReader sr = new StreamReader( infile ) ){
            string basedir = Path.GetDirectoryName( infile );
            // header
            string cs_space = (name_space == "" ? "" : "    ");
            sw.WriteLine( "#if JAVA" );
            if( package != "" ){
                sw.WriteLine( "package " + package + ";" );
                sw.WriteLine();
            }
            sw.WriteLine( "import java.awt.*;" );
            sw.WriteLine( "import org.kbinani.*;" );
            sw.WriteLine( "#" + "else" );
            sw.WriteLine( "using System;" );
            sw.WriteLine( "using System.IO;" );
            sw.WriteLine( "using System.Drawing;" );
            sw.WriteLine( "using bocoree;" );
            sw.WriteLine();
            if( name_space != "" ){
                sw.WriteLine( "namespace " + name_space + "{" );
            }
            sw.WriteLine( "#endif" );
            sw.WriteLine( cs_space + "public class Resources{" );
            string line = "";
            while( (line = sr.ReadLine()) != null ){
                string[] spl = line.Split( '\t' );
                if( spl.Length < 3 ){
                    continue;
                }
                string name = spl[0];
                string type = spl[1];
                string tpath = spl[2];
                string path = Path.Combine( basedir, tpath );
                if( !File.Exists( path ) ){
                    continue;
                }

                if( type == "Image" ){
                    string instance = "s_" + name;
                    string stored = "s_stored_" + name;
                    string stored_value = "";
                    using( FileStream fs = new FileStream( path, FileMode.Open, FileAccess.Read ) ){
                        int len = (int)fs.Length;
                        byte[] data = new byte[len];
                        fs.Read( data, 0, len );
                        stored_value = Convert.ToBase64String( data );
                    }
                    if( stored_value.Length < 128 ){
                        sw.WriteLine( cs_space + "    private static readonly String " + stored + " = \"" + stored_value + "\";" );
                    }else{
                        sw.WriteLine( cs_space + "    private static readonly String " + stored + " = \"" + stored_value.Substring( 0, 128 ) + "\" +" );
                        stored_value = stored_value.Substring( 128 );
                        //Console.WriteLine( "remaining=" + stored_value );
                        while( stored_value.Length >= 128 ){
                            sw.WriteLine( cs_space + "        \"" + stored_value.Substring( 0, 128 ) + "\" +" );
                            stored_value = stored_value.Substring( 128 );
                            //Console.WriteLine( "remaining=" + stored_value );
                            if( stored_value.Length < 128 ){
                                break;
                            }
                        }
                        sw.WriteLine( cs_space + "        \"" + stored_value + "\";" );
                    }
                    sw.WriteLine( cs_space + "    private static Image " + instance + " = null;" );
                    sw.WriteLine( cs_space + "    public static Image get_" + name + "(){" );
                    sw.WriteLine( cs_space + "        if( " + instance + " == null ){" );
                    sw.WriteLine( cs_space + "            byte[] data = Base64.decode( " + stored + " );" );
                    sw.WriteLine( "#if JAVA" );
                    sw.WriteLine( cs_space + "            Frame frame = new Frame();" );
                    sw.WriteLine( cs_space + "            " + instance + " = frame.getToolKit().createImage( data );" );
                    sw.WriteLine( "#else" );
                    sw.WriteLine( cs_space + "            using( MemoryStream ms = new MemoryStream( data ) ){" );
                    sw.WriteLine( cs_space + "                " + instance + " = Image.FromStream( ms );" );
                    sw.WriteLine( cs_space + "            }" );
                    sw.WriteLine( "#endif" );
                    sw.WriteLine( cs_space + "        }" );
                    sw.WriteLine( cs_space + "        return " + instance + ";" );
                    sw.WriteLine( cs_space + "    }" );
                    sw.WriteLine();
                }
            }
            sw.WriteLine( cs_space + "}" );
            if( name_space != "" ){
                sw.WriteLine( "#if !JAVA" );
                sw.WriteLine( "}" );
                sw.WriteLine( "#endif" );
            }
        }
    }
}