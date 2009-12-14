public static class Print_MetaText {
    public static bool Edit( org.kbinani.vsq.VsqFile vsq ) {
        vsq.Track.get( 1 ).printMetaText( @"c:\meta_text.txt" );
        return true;
    }
}