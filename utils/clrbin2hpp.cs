using System;
using System.Reflection;
using System.IO;

class CLRBin2Hpp{
    public static void Main( string[] args ){
        string bin = args[0];
        if( !File.Exists( bin ) ){
            Console.WriteLine( "error; file not found" );
            return;
        }

        FileInfo fi = new FileInfo( bin );
        bin = fi.FullName;
        Assembly asm = null;
        try{
            asm = Assembly.LoadFile( bin );
        }catch( Exception ex ){
            Console.WriteLine( "error; assembly load error; ex=" + ex );
            return;
        }

        if( asm == null ){
            Console.WriteLine( "error; assembly load error" );
        }

        // 型毎にファイルを作る
        string basedir = Path.GetDirectoryName( bin );
        foreach( Type t in asm.GetTypes() ){
            string header_file = "";
            try{
                // 名前空間を調べる
                string nspace = t.Namespace;
                string[] spl = nspace.Split( '.' );

                // ディレクトリがあるか確かめる．無いなら作る
                string dir = "";
                for( int i = 0; i < spl.Length; i++ ){
                    dir = Path.Combine( dir, spl[i] );
                    string test_dir = Path.Combine( basedir, dir );
                    if( !Directory.Exists( test_dir ) ){
                        Directory.CreateDirectory( test_dir );
                    }
                }
                header_file = Path.Combine( basedir, Path.Combine( dir, t.Name + ".h" ) );
            }catch{
                continue;
            }
            printType( t, header_file );
        }
    }

    static void printType( Type t, string filename ){
        using( StreamWriter sw = new StreamWriter( filename ) ){
            sw.WriteLine( "class " + t.Name + "{" );

            // public instance methodを出力
            sw.WriteLine( "public:" );
            foreach( MethodInfo mi in t.GetMethods( BindingFlags.Instance | BindingFlags.Public ) ){
                printMethod( mi, sw, "    " );
            }
            sw.WriteLine();

            // public static methodを出力
            foreach( MethodInfo mi in t.GetMethods( BindingFlags.Static | BindingFlags.Public ) ){
                printMethod( mi, sw, "    static " );
            }
            sw.WriteLine();

            // private instance methodを出力
            sw.WriteLine( "private:" );
            foreach( MethodInfo mi in t.GetMethods( BindingFlags.Instance | BindingFlags.NonPublic ) ){
                printMethod( mi, sw, "    " );
            }
            sw.WriteLine();

            // private static methodを出力
            foreach( MethodInfo mi in t.GetMethods( BindingFlags.Static | BindingFlags.NonPublic ) ){
                printMethod( mi, sw, "    static " );
            }
            sw.WriteLine();

            sw.WriteLine( "};" );
        }
    }

    static void printMethod( MethodInfo mi, StreamWriter sw, string prefix ){
        // インデント
        sw.Write( prefix );
        // 戻り値の型
        sw.Write( parseTypeName( mi.ReturnType ) + " " );
        // メソッド名
        sw.Write( mi.Name );
        // 引数
        ParameterInfo[] pis = mi.GetParameters();
        if( pis.Length > 0 ){
            sw.Write( "( " );
            for( int i = 0; i < pis.Length; i++ ){
                if( i > 0 ){
                    sw.Write( ", " );
                }
                sw.Write( parseTypeName( pis[i].ParameterType ) + " " + pis[i].Name );
            }
            sw.WriteLine( " );" );
        }else{
            sw.WriteLine( "();" );
        }
    }

    static string parseTypeName( Type t ){
        string name = t + "";
        int array_num = 0;
        while( name.EndsWith( "[]" ) ){
            array_num++;
            name = name.Substring( 0, name.Length - 2 );
        }
        bool ends_with_asterisk = name.EndsWith( "*" );
        if( ends_with_asterisk ){
            name = name.Substring( 0, name.Length - 1 );
        }
        
        string ret = "";
        bool premitive = true;
        switch( name ){
            case "System.Byte":
                ret = "unsigned char";
                break;
            case "System.Int16":
                ret = "short";
                break;
            case "System.UInt16":
                ret = "unsigned short";
                break;
            case "System.Int32":
                ret = "int";
                break;
            case "System.UInt32":
                ret = "unsigned int";
                break;
            case "System.Int64":
                ret = "long long";
                break;
            case "System.UInt64":
                ret = "unsigned long long";
                break;
            case "System.Boolean":
                ret = "bool";
                break;
            case "System.Void":
                ret = "void";
                break;
            case "System.IntPtr":
                ret = "void*";
                break;
            case "System.String":
                ret = "std::wstring";
                break;
            case "System.SByte":
                ret = "char";
                break;
            case "System.Single":
                ret = "float";
                break;
            case "System.Double":
                ret = "double";
                break;
            case "System.Char":
                ret = "wchar_t";
                break;
            default:
                ret = name.Replace( ".", "::" );
                premitive = false;
                break;
        }
        if( ends_with_asterisk ){
            ret += "*";
        }else if( !premitive ){
            ret += "&";
        }
        for( int i = 0; i < array_num; i++ ){
            ret += "[]";
        }
        return ret;
    }
}
