import java.io.*;
import com.boare.corlib.*;

class file2binArraySrc{
    public static void main( String[] args ){
        if( args.length < 2 ){
            return;
        }
        try{
            File file = new File( args[0] );
            int length = (int)file.length();
            FileInputStream in = new FileInputStream( file );
            StreamWriter out = new StreamWriter( args[1] );
            out.write( "String data = \"" );
            byte[] buf = new byte[length];
            in.read( buf, 0, length );
            out.writeLine( Base64.encode( buf ) + "\";" );
            in.close();
            out.close();
        }catch( Exception ex ){
        }
    }
}
