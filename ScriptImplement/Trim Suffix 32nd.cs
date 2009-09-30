public static class Trim_Suffix_32nd {
    public static bool Edit( Boare.Lib.Vsq.VsqFile Vsq ) {
        for ( int i = 1; i < Vsq.Track.size(); i++ ) {
            for ( int j = 0; j < Vsq.Track.get( i ).getEventCount(); j++ ) {
                Boare.Lib.Vsq.VsqEvent item = Vsq.Track.get( i ).getEvent( j );
                if ( item.ID.type == Boare.Lib.Vsq.VsqIDType.Anote ) {
                    // 32分音符の長さは，クロック数に直すと60クロック
                    if ( item.ID.Length > 60 ) {
                        item.ID.Length -= 60;
                    }
                }
            }
        }
        return true;
    }
}