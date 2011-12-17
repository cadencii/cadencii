public static class Print_MetaText {
    public static bool Edit( com.github.cadencii.vsq.VsqFile vsq ) {
        vsq.Track.get( 1 ).printMetaText( @"c:\meta_text.txt", "Shift_JIS" );
        return true;
    }
}