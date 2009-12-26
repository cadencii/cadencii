using System;
using System.Collections.Generic;
using org.kbinani;
using org.kbinani.vsq;
using org.kbinani.java.util;

class randomizeAnalysis{
    [STAThread]
    public static void Main( string[] args ){
        System.Windows.Forms.OpenFileDialog d = new System.Windows.Forms.OpenFileDialog();
        if ( d.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
            string f = d.FileName;
            string dir = System.IO.Path.GetDirectoryName( f );
            string name = System.IO.Path.GetFileNameWithoutExtension( f );
            Console.WriteLine( "name=" + name );
            int index = name.IndexOf( "-001" );
            string basename = "";
            if ( index >= 0 ) {
                basename = name.Substring( 0, index );
            }
            int count = 0;
            Dictionary<int, int> dict = new Dictionary<int, int>();
            while ( true ) {
                count++;
                string open = PortUtil.combinePath( dir, basename + "-" + PortUtil.formatDecimal( "000", count ) + ".vsq" );
                Console.WriteLine( "open=" + open );
                if ( !PortUtil.isFileExists( open ) ) {
                    break;
                }
                VsqFile vsq = new VsqFile( open, "Shift_JIS" );
                int i = -1;
                int b = 3840;
                for ( Iterator itr = vsq.Track.get( 1 ).getNoteEventIterator(); itr.hasNext(); ) {
                    i++;
                    VsqEvent item = (VsqEvent)itr.next();
                    if ( i == 0 ){
                        continue;
                    }
                    int ideal_start = b + i * 480;
                    int shift = item.Clock - ideal_start;
                    if( dict.ContainsKey( shift ) ){
                        dict[shift] = dict[shift] + 1;
                    }else{
                        dict.Add( shift, 1 );
                    }
                }
            }
            int max = 0;
            int min = 0;
            foreach( int key in dict.Keys ){
                max = Math.Max( max, key );
                min = Math.Min( min, key );
            }
            using ( System.IO.StreamWriter sw = new System.IO.StreamWriter( PortUtil.combinePath( dir, basename + ".txt" ) ) ) {
                for( int i = min; i <= max; i++ ){
                    if ( dict.ContainsKey( i ) ){
                        sw.WriteLine( i + "\t" + dict[i] );
                    }else{
                        sw.WriteLine( i + "\t0" );
                    }
                }
            }
        }
    }
}
