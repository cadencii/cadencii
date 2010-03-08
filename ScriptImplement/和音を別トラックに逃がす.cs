using System;
using System.Collections.Generic;
using org.kbinani;
using org.kbinani.vsq;
using org.kbinani.cadencii;

public class 和音を別トラックに逃がす {
    public static ScriptReturnStatus Edit( VsqFileEx vsq ) {
        int selected = AppManager.getSelected();

        List<VsqEventList> add = new List<VsqEventList>();

        VsqTrack vsq_track = vsq.Track.get( selected );
        for ( int i = 0; i < vsq_track.getEventCount(); i++ ) {
            VsqEvent itemi = vsq_track.getEvent( i );
            if ( itemi.ID.type != VsqIDType.Anote ) {
                continue;
            }

            int istart = itemi.Clock;
            int iend = itemi.Clock + itemi.ID.getLength();
            bool changed = true;
            while ( changed ) {
                changed = false;
                for ( int j = 0; j < vsq_track.getEventCount(); j++ ) {
                    VsqEvent itemj = vsq_track.getEvent( j );

                    if ( isOverlapped( itemi, itemj ) ) {
                        if ( add.Count <= 0 ) {
                            // 和音用リストがまだひとつも無い場合
                            VsqEventList newlist = new VsqEventList();
                            newlist.add( (VsqEvent)itemj.clone() );
                            add.Add( newlist );
                        } else {
                            // 和音用のリストに，重複する音符が登録されていないかどうかをチェック
                            bool added = false;
                            for ( int k = 0; k < add.Count; k++ ) {
                                VsqEventList list = add[k];
                                bool filled = false;
                                for ( int m = 0; m < list.getCount(); m++ ) {
                                    VsqEvent itemm = list.getElement( m );
                                    if ( isOverlapped( itemj, itemm ) ) {
                                        filled = true;
                                        break;
                                    }
                                }

                                if ( !filled ) {
                                    // itemjを挿入しても問題ない
                                    list.add( (VsqEvent)itemj.clone() );
                                    added = true;
                                    break;
                                }
                            }

                            if ( !added ) {
                                // どれも埋まっていて，itemjを挿入できなかった．
                                VsqEventList newlist = new VsqEventList();
                                newlist.add( (VsqEvent)itemj.clone() );
                                add.Add( newlist );
                            }
                        }

                        // itemjを削除
                        vsq_track.removeEvent( j );
                        changed = true;
                        break;
                    }
                }
            }
        }

        VsqEvent singer = vsq_track.getSingerEventAt( 0 );
        for ( int i = 0; i < add.Count && vsq.Track.size() + 1 < 17; i++ ) {
            VsqTrack newtrack = new VsqTrack( "add" + i, singer.ID.IconHandle.IDS );
            add[i].add( (VsqEvent)singer.clone() );
            newtrack.MetaText.Events = add[i];
            vsq.Track.add( newtrack );
        }

        return ScriptReturnStatus.EDITED;
    }

    private static bool isOverlapped( VsqEvent itemi, VsqEvent itemj ) {
        if ( itemi.ID.type != VsqIDType.Anote ||
             itemj.ID.type != VsqIDType.Anote ) {
            return false;
        }

        int istart = itemi.Clock;
        int iend = istart + itemi.ID.getLength();
        int jstart = itemj.Clock;
        int jend = jstart + itemj.ID.getLength();
        bool overlapped = false;
        if ( jstart <= istart ) {
            if ( istart < jend ) {
                return true;
            }
        }
        if ( istart <= jstart ) {
            if ( jstart < iend ) {
                return true;
            }
        }
        return false;
    }
}
