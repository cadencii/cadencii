public static class Print_MetaText {
    public static bool Edit( Boare.Lib.Vsq.VsqFile vsq ) {
        vsq.Track.get( 1 ).printMetaText( @"c:\meta_text.txt" );
        return true;
    }
}