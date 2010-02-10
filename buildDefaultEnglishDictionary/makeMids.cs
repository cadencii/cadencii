using System;
using System.IO;
using org.kbinani.vsq;

class WordsFromDBFiles{
    public static void Main( string[] args ){
        if( args.Length < 3 ){
            Console.WriteLine( "makeMids [singer(ex. Miku)] [DSB(ex. DSB303)] [extension(ex. .vsq)]" );
            return;
        }
        string singer = args[0];
        string dsb = args[1];
        string ext = args[2];
        if( !Directory.Exists( singer ) ){
            Directory.CreateDirectory( singer );
        }
        using( StreamReader sr = new StreamReader( "parsed.txt" ) ){
            string line = "";
            VsqFile src = new VsqFile( singer, 2, 4, 4, 500000 );
            VsqFile vsq = (VsqFile)src.clone();
            int clock = 480 * 4 * 2;
            int count = 0;
            int numVsqs = 0;
            while( (line = sr.ReadLine()) != null ){
                VsqEvent item = new VsqEvent();
                item.Clock = clock;
                item.ID = new VsqID();
                item.ID.type = VsqIDType.Anote;
                item.ID.setLength( 240 );
                item.ID.Note = 64;
                item.ID.LyricHandle = new LyricHandle();
                item.ID.LyricHandle.L0 = new Lyric( line, "u:" );
                vsq.Track.get( 1 ).addEvent( item, count + 1 );
                
                clock += 240;
                count++;
                if( count > 5000 ){
                    vsq.Track.get( 1 ).getCommon().Version = dsb;
                    vsq.Track.get( 1 ).sortEvent();
                    vsq.write( singer + "\\" + numVsqs + ext );
                    numVsqs++;
                    vsq = null;
                    vsq = (VsqFile)src.clone();
                    clock = 480 * 4 * 2;
                    count = 0;
                }
            }
            if( count > 0 ){
                vsq.Track.get( 1 ).getCommon().Version = dsb;
                vsq.Track.get( 1 ).sortEvent();
                vsq.write( singer + "\\" + numVsqs + ext );
            }
        }
    }
}
