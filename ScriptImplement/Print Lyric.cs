public static class Print_Lyric {
    public static bool Edit( org.kbinani.vsq.VsqFile Vsq ) {
        System.IO.StreamWriter sw = null;
        try {
            sw = new System.IO.StreamWriter( @"c:\lyrics.txt" );
            for ( int i = 0; i < Vsq.Track.get( 1 ).getEventCount(); i++ ) {
                org.kbinani.vsq.VsqEvent item = Vsq.Track.get( 1 ).getEvent( i );
                if ( item.ID.type == org.kbinani.vsq.VsqIDType.Anote ) {
                    int clStart = item.Clock;
                    int clEnd = clStart + item.ID.Length;
                    double secStart = Vsq.getSecFromClock( clStart );
                    double secEnd = Vsq.getSecFromClock( clEnd );
                    sw.WriteLine( secStart + "\t" + secEnd + "\t" + item.ID.LyricHandle.L0.Phrase + "\t" + item.ID.LyricHandle.L0.getPhoneticSymbol() );
                }
            }
        } catch {
            return false;
        } finally {
            if ( sw != null ) {
                sw.Close();
            }
        }
        return true;
    }
}