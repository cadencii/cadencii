#if JAVA
import java.io.*;
import org.kbinani.*;
import org.kbinani.vsq.*;
#else
using System;
using org.kbinani;
using org.kbinani.vsq;
using org.kbinani.java.io;
#endif

class Vsq2vxt{
#if JAVA
    public static void main( String[] args ) throws Exception {
#else
    public static void Main( String[] args ){
#endif

#if JAVA
        if( args.length != 3 ){
#else
        if( args.Length != 3 ){
#endif
            PortUtil.println( "vsq2vxt" );
            PortUtil.println( "Usage: vsq2vxt [*.vsq] [track#] [output]" );
            return;
        }
        String path_vsq = args[0];
        String str_track = args[1];
        String path_out = args[2];
        if( !PortUtil.isFileExists( path_vsq ) ){
            PortUtil.println( "error@Vsq2vxt#main; file not fount: '" + path_vsq + "'" );
            return;
        }
        int track = 1;
        try{
            track = PortUtil.parseInt( str_track );
        }catch( Exception ex ){
            PortUtil.println( "error@Vsq2vxt#main; invalid number expression: " + str_track );
            return;
        }

        if( PortUtil.isFileExists( path_out ) ){
            PortUtil.println( "'" + path_out + "' already exists. Overwrite?(y/n)" );
            BufferedReader br = null;
            String ret = "n";
            try{
#if JAVA
                br =  new BufferedReader( new InputStreamReader( System.in ) );
                ret = br.readLine().trim();
#else
                ret = Console.ReadLine();
#endif
            }catch( Exception ex ){
            }
#if JAVA
            if( !ret.startsWith( "y" ) && !ret.startsWith( "Y" ) ){
#else
            if( !ret.StartsWith( "y" ) && !ret.StartsWith( "Y" ) ){
#endif
                return;
            }
        }

        VsqFile vsq = null;
        try{
            vsq = new VsqFile( path_vsq, "Shift_JIS" );
        }catch( Exception ex ){
            PortUtil.println( "error@Vsq2vxt#main; invalid VSQ; ex=" + ex );
            return;
        }
        if( vsq == null ){
            PortUtil.println( "error@Vsq2vxt#main; invalid VSQ" );
            return;
        }
        if( track < 1 || vsq.Track.size() <= track ){
            PortUtil.println( "error@Vsq2vxt#main; invalid track number; for this VSQ file, track# should be 1 <= track# <=" + (vsq.Track.size() - 1) );
            return;
        }
        VsqTrack vsq_track = vsq.Track.get( track );
        try{
            vsq_track.printMetaText( path_out, "UTF-8" );
        }catch( Exception ex ){
            PortUtil.println( "error@Vsq2vxt#main; ex=" + ex );
        }
    }
}
