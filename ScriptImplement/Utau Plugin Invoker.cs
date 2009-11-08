using System;
using System.IO;
using System.Windows.Forms;
using Boare.Cadencii;
using System.Collections.Generic;
using Boare.Lib.Vsq;
using bocoree.util;

public class Utau_Plugin_Invoker {
    private static bool s_finished = false;
    private static string s_plugin_txt_path = @"E:\Program Files\UTAU\plugins\picedit\plugin.txt";

    public static ScriptReturnStatus Edit( VsqFileEx vsq ) {
        if ( AppManager.getSelectedEventCount() <= 0 ) {
            return ScriptReturnStatus.NOT_EDITED;
        }
        string pluginTxtPath = s_plugin_txt_path;
        if ( pluginTxtPath == "" ) {
            AppManager.showMessageBox( "pluginTxtPath=" + pluginTxtPath );
            return ScriptReturnStatus.ERROR;
        }
        if ( !System.IO.File.Exists( pluginTxtPath ) ) {
            AppManager.showMessageBox( "'" + pluginTxtPath + "' does not exists" );
            return ScriptReturnStatus.ERROR;
        }
        System.Text.Encoding shift_jis = System.Text.Encoding.GetEncoding( "Shift_JIS" );
        string name = "";
        string exe_path = "";
        using ( StreamReader sr = new StreamReader( pluginTxtPath, shift_jis ) ) {
            string line = "";
            while ( (line = sr.ReadLine()) != null ) {
                string[] spl = line.Split( new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries );
                if ( line.StartsWith( "name=" ) ) {
                    name = spl[1];
                } else if ( line.StartsWith( "execute=" ) ) {
                    exe_path = Path.Combine( Path.GetDirectoryName( pluginTxtPath ), spl[1] );
                }
            }
        }
        if ( exe_path == "" ) {
            return ScriptReturnStatus.ERROR;
        }
        if ( !System.IO.File.Exists( exe_path ) ) {
            AppManager.showMessageBox( "'" + exe_path + "' does not exists" );
            return ScriptReturnStatus.ERROR;
        }

        // ustを用意 ------------------------------------------------------------------------
        // 方針は，一度VsqFileに音符を格納->UstFile#.ctor( VsqFile )を使って一括変換
        // メイン画面で選択されているアイテムを列挙
        List<VsqEvent> items = new List<VsqEvent>(); // Ustに追加する音符のリスト
        List<int> map_id = new List<int>(); // ustの[#index]が、map_id[index].InternalIDというIDを持つVsqEventに相当することを記録しておくリスト
        int num_selected = 0; // 選択されていた音符の個数
        for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
            SelectedEventEntry item_itr = (SelectedEventEntry)itr.next();
            if ( item_itr.original.ID.type == VsqIDType.Anote ) {
                items.Add( (VsqEvent)item_itr.original.clone() );
                num_selected++;
            }
        }
        items.Sort();

        // R用音符のテンプレート
        VsqEvent template = new VsqEvent();
        template.ID.type = VsqIDType.Anote;
        template.ID.LyricHandle = new LyricHandle( "R", "" );
        template.InternalID = -1; // VSQ上には実際に存在しないダミーであることを表す

        int count = items.Count;
        // アイテムが2個以上の場合は、間にRを挿入する必要があるかどうか判定
        if ( count > 1 ) {
            List<VsqEvent> add = new List<VsqEvent>(); // 追加するR
            VsqEvent last_event = items[0];
            for ( int i = 1; i < count; i++ ) {
                VsqEvent item = items[i];
                if ( last_event.Clock + last_event.ID.Length < item.Clock ) {
                    VsqEvent add_temp = (VsqEvent)template.clone();
                    add_temp.Clock = last_event.Clock + last_event.ID.Length;
                    add_temp.ID.Length = item.Clock - add_temp.Clock;
                    add.Add( add_temp );
                }
                last_event = item;
            }
            foreach ( VsqEvent v in add ) {
                items.Add( v );
            }
            items.Sort();
        }

        // ヘッダ
        int TEMPO = 120;

        // 選択アイテムの直前直後に未選択アイテムがあるかどうかを判定
        int clock_begin = items[0].Clock;
        int clock_end = items[items.Count - 1].Clock;
        int clock_end_end = clock_end + items[items.Count - 1].ID.Length;
        VsqTrack vsq_track = vsq.Track.get( AppManager.getSelected() );
        VsqEvent tlast_event = null;
        VsqEvent prev = null;
        VsqEvent next = null;

        // VsqFile -> UstFileのコンバート
        VsqFile conv = new VsqFile( "Miku", 2, 4, 4, (int)(6e7 / TEMPO) );
        VsqTrack conv_track = conv.Track.get( 1 );
        for ( Iterator itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {
            VsqEvent item_itr = (VsqEvent)itr.next();
            if ( item_itr.Clock == clock_begin ) {
                if ( tlast_event != null ) {
                    // アイテムあり
                    if ( tlast_event.Clock + tlast_event.ID.Length == clock_begin ) {
                        // ゲートタイム差0で接続
                        prev = (VsqEvent)tlast_event.clone();
                    } else {
                        // 時間差アリで接続
                        prev = (VsqEvent)template.clone();
                        prev.Clock = tlast_event.Clock + tlast_event.ID.Length;
                        prev.ID.Length = clock_begin - (tlast_event.Clock + tlast_event.ID.Length);
                    }
                }
            }
            tlast_event = item_itr;
            if ( item_itr.Clock == clock_end ) {
                if ( itr.hasNext() ) {
                    VsqEvent foo = (VsqEvent)itr.next();
                    if ( foo.Clock == clock_end_end ) {
                        // ゲートタイム差0で接続
                        next = (VsqEvent)foo.clone();
                    } else {
                        // 時間差アリで接続
                        next = (VsqEvent)template.clone();
                        next.Clock = clock_end_end;
                        next.ID.Length = foo.Clock - clock_end_end;
                    }
                }
                break;
            }

        }

        // [#PREV]を追加
        if ( prev != null ) {
            prev.Clock -= clock_begin;
            conv_track.addEvent( prev );
        }

        // ゲートタイムを計算しながら追加
        count = items.Count;
        for ( int i = 0; i < count; i++ ) {
            VsqEvent itemi = items[i];
            itemi.Clock -= clock_begin;
            conv_track.addEvent( itemi );
            map_id.Add( itemi.InternalID );
        }

        // [#NEXT]を追加
        if ( next != null ) {
            next.Clock -= clock_begin;
            conv_track.addEvent( next );
        }

        // PIT, PBSを追加
        copyCurve( vsq_track.getCurve( "pit" ), conv_track.getCurve( "pit" ), clock_begin );
        copyCurve( vsq_track.getCurve( "pbs" ), conv_track.getCurve( "pbs" ), clock_begin );

        string temp = Path.GetTempFileName();
        UstFile tust = new UstFile( conv, 1 );
        if ( prev != null ) {
            tust.getTrack( 0 ).getEvent( 0 ).Index = int.MinValue;
        }
        if ( next != null ) {
            tust.getTrack( 0 ).getEvent( tust.getTrack( 0 ).getEventCount() - 1 ).Index = int.MaxValue;
        }
        tust.write( temp );

        // 起動 -----------------------------------------------------------------------------
        StartPluginArgs arg = new StartPluginArgs();
        arg.exePath = exe_path;
        arg.tmpFile = temp;
        System.Threading.Thread t = new System.Threading.Thread( new System.Threading.ParameterizedThreadStart( runPlugin ) );
        s_finished = false;
        t.Start( arg );
        while ( !s_finished ) {
            System.Threading.Thread.Sleep( 100 );
            Application.DoEvents();
        }

        // 結果を反映 -----------------------------------------------------------------------
        UstFile ust = null;
        VsqFile vsq_edited = null;
        try {
            ust = new UstFile( temp );
            vsq_edited = new VsqFile( ust );
        } catch ( Exception ex ) {
            AppManager.showMessageBox( "invalid ust file; ex=" + ex );
            return ScriptReturnStatus.ERROR;
        }

        // プラグインの実行結果が、どのクロック範囲に及んでいるかを判定
        UstTrack tr = ust.getTrack( 0 );
        int clock_edit_start = clock_begin;
        int clock_edit_end = clock_end_end;
        int event_size = tr.getEventCount();
        int counter = -1;
        int clock = 0;
        // clock_edit_startを取得
        for ( int i = 0; i < event_size; i++ ) {
            UstEvent ue = tr.getEvent( i );
            counter++;
            if ( prev == null ) {
                clock_edit_start = clock;
                break;
            } else {
                if ( counter > 0 ) {
                    clock_edit_start = clock;
                    break;
                }
            }
            clock += ue.Length;
        }
        counter = -1;
        // clock_edit_endを取得
        for ( int i = event_size - 1; i >= 0; i-- ) {
            UstEvent ue = tr.getEvent( i );
            counter++;
            if ( prev == null ) {
                clock_edit_end = clock + ue.Length;
                break;
            } else {
                if ( counter > 0 ) {
                    clock_edit_end = clock + ue.Length;
                    break;
                }
            }
            clock += ue.Length;
        }
        int len = clock_edit_end - clock_edit_start;

        // vsq_editedのクロックをシフト
        // clock_edit_startが、clock_beginにくるように移動させる
        VsqFile.shift( vsq_edited, clock_begin - clock_edit_start );
        clock_edit_start = clock_begin;          // シフト後の、被編集範囲の開始位置
        clock_edit_end = clock_edit_start + len; // シフト後の、非編集範囲の終了位置

        // clock_edit_end != clock_endの場合。clock_end以降のアイテムをずらす必要がある
        if ( clock_edit_end != clock_end ) {
            int shift = clock_edit_end - clock_end;
            int track_size = vsq.Track.size();
            for ( int i = 0; i < track_size; i++ ) {
                VsqTrack track_edit = vsq.Track.get( i );
                foreach ( CurveType s in AppManager.CURVE_USAGE ) {
                    VsqBPList list = track_edit.getCurve( s.getName() );
                    if ( list == null ) {
                        continue;
                    }
                    int num_points = list.size();
                    if ( shift > 0 ) {
                        // 遅らせる
                        for ( int j = num_points - 1; j >= 0; j-- ) {
                            int cl = list.getKeyClock( j );
                            if ( clock_end <= cl ) {
                                list.move( cl, cl + shift, list.getElement( j ) );
                            } else if ( cl < clock_end ) {
                                break;
                            }
                        }
                    } else {
                        // clock_edit_end <= * < clock_end の間のデータ点を削除
                        for ( int j = num_points - 1; j >= 0; j-- ) {
                            int cl = list.getKeyClock( j );
                            if ( clock_edit_end <= cl && cl < clock_end ) {
                                list.remove( cl );
                            } else if ( cl < clock_edit_end ) {
                                break;
                            }
                        }

                        // アイテムをずらす
                        num_points = list.size();
                        for ( int j = 0; j < num_points; j++ ) {
                            int cl = list.getKeyClock( j );
                            if ( clock_edit_end <= cl && cl < clock_end ) {
                                list.move( cl, cl + shift, list.getElement( j ) );
                            } else if ( clock_end <= cl ) {
                                break;
                            }
                        }
                    }
                }
            }
        }

        int track_count = ust.getTrackCount();
        for ( int i = 0; i < track_count; i++ ) {
            UstTrack track = ust.getTrack( i );
            int event_count = track.getEventCount();
            int shift_required_clock = 0; //選択部の音符以降を、このクロック数だけずらす必要がある.
            for ( int j = 0; j < event_count; j++ ) {
                UstEvent itemj = track.getEvent( j );
                if ( itemj.Index == int.MinValue ) {
                    AppManager.showMessageBox( "[#PREV]" );
                } else if ( itemj.Index == int.MaxValue ) {
                    AppManager.showMessageBox( "[#NEXT]" );
                } else {
                    if ( 0 <= itemj.Index && itemj.Index < map_id.Count ) {
                        // 既存の音符(ダミーRも含む)に対する編集
                        int internal_id = map_id[itemj.Index];
                        if ( internal_id > 0 ) {
                            // 普通の音符
                            VsqEvent edit = vsq_track.findEventFromID( internal_id );
                            if ( edit != null ) {
                                edit.ID.Dynamics = itemj.Intensity;
                                shift_required_clock += itemj.getLength() - edit.ID.getLength();
                                edit.ID.setLength( itemj.getLength() );
                                edit.ID.Note = itemj.Note;
                                edit.UstEvent = itemj;
                            }
                        } else {
                            // ダミーR
                        }
                    } else {
                        // 新規の音符が追加された
                    }
                }
            }
        }

        try {
            System.IO.File.Delete( temp );
        } catch ( Exception ex ) {
        }
        return ScriptReturnStatus.ERROR;
    }

    private static void copyCurve( VsqBPList src, VsqBPList dest, int clock_shift ) {
        int last_value = src.getDefault();
        int count = src.size();
        bool first_over_zero = true;
        for ( int i = 0; i < count; i++ ) {
            int cl = src.getKeyClock( i ) - clock_shift;
            int value = src.getElementA( i );
            if ( cl < 0 ) {
                last_value = value;
            } else {
                if ( first_over_zero ) {
                    first_over_zero = false;
                    if ( last_value != src.getDefault() ) {
                        dest.add( 0, last_value );
                    }
                }
                dest.add( cl, value );
            }
        }
    }

    private static void runPlugin( object arg ) {
        StartPluginArgs a = (StartPluginArgs)arg;
        string exe_path = a.exePath;
        string temp = a.tmpFile;
        using ( System.Diagnostics.Process p = new System.Diagnostics.Process() ) {
            p.StartInfo.FileName = exe_path;
            p.StartInfo.Arguments = "\"" + temp + "\"";
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName( exe_path );
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            p.Start();
            p.WaitForExit();
        }
        s_finished = true;
    }

    private class StartPluginArgs {
        public string exePath = "";
        public string tmpFile = "";
    }
}
