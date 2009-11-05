//あ
using System;
using System.IO;
using System.Windows.Forms;
using Boare.Cadencii;
using Boare.Lib.Vsq;

public class Test{
    private static bool s_finished = false;
    private static string s_plugin_txt_path = "E:\\Program Files\\UTAU\\plugins\\N音間ピッチエディタ\\plugin.txt";

    public static ScriptReturnStatus Edit( VsqFileEx vsq ){
        if( AppManager.getSelectedEventCount() <= 0 ){
            return ScriptReturnStatus.NOT_EDITED;
        }
        string pluginTxtPath = s_plugin_txt_path;
        if( pluginTxtPath == "" ){
            AppManager.showMessageBox( "pluginTxtPath=" + pluginTxtPath );
            return ScriptReturnStatus.ERROR;
        }
        if( !System.IO.File.Exists( pluginTxtPath ) ){
            AppManager.showMessageBox( "'" + pluginTxtPath + "' does not exists" );
            return ScriptReturnStatus.ERROR;
        }
        System.Text.Encoding shift_jis = System.Text.Encoding.GetEncoding( "Shift_JIS" );
        string name = "";
        string exe_path = "";
        using( StreamReader sr = new StreamReader( pluginTxtPath, shift_jis ) ){
            string line = "";
            while( (line = sr.ReadLine()) != null ){
                string[] spl = line.Split( new char[]{ '=' }, StringSplitOptions.RemoveEmptyEntries );
                if( line.StartsWith( "name=" ) ){
                    name = spl[1];
                }else if( line.StartsWith( "execute=" ) ){
                    exe_path = Path.Combine( Path.GetDirectoryName( pluginTxtPath ), spl[1] );
                }
            }
        }
        if( exe_path == "" ){
            return ScriptReturnStatus.ERROR;
        }
        if( !System.IO.File.Exists( exe_path ) ){
            AppManager.showMessageBox( "'" + exe_path + "' does not exists" );
            return ScriptReturnStatus.ERROR;
        }

        // ustを用意
        string temp = Path.GetTempFileName();
        using( BufferedWriter sw = new BufferedWriter( new OutputStreamWriter( new FileInputStream( temp ), "Shift_JIS" ) ) ){
            // メイン画面で選択されているアイテムを列挙
            List<VsqEvent> items = new List<VsqEvent>();
            for( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                SelectedEventEntry item_itr = (SelectedEventEntry)itr.next();
                if( item_itr.original.ID.type == VsqIDType.Anote ){
                    items.Add( (VsqEvent)item_itr.original.clone() );
                }
            }
            items.Sort();

            // R用音符のテンプレート
            VsqEvent template = new VsqEvent();
            template.ID.type = VsqIDType.Anote;
            template.ID.LyricHandle = new LyricHandle( "R", "" );

            int count = items.Count;
            // アイテムが2個以上の場合は、間にRを挿入する必要があるかどうか判定
            if( count > 1 ){
                List<VsqEvent> add = new List<VsqEvent>(); // 追加するR
                VsqEvent last_event = items[0];
                for( int i = 1; i < count; i++ ){
                    VsqEvent item = items[i];
                    if( last_event.Clock + last_event.ID.Length < item.Clock ){
                        VsqEvent add_temp = (VsqEvent)template.clone();
                        add_temp.Clock = last_event.Clock + last_event.ID.Length;
                        add_temp.ID.Length = item.Clock - add_temp.Clock;
                        add.Add( add_temp );
                    }
                    last_event = item;
                }
                foreach( VsqEvent v in add ){
                    items.Add( v );
                }
                items.Sort();
            }

            // ヘッダ
            int TEMPO = 120;
            sw.WriteLine( "[#SETTING]" );
            sw.WriteLine( "Tempo=" + TEMPO );
            sw.WriteLine( "VoiceDir=" ); // TODO
            sw.WriteLine( "CacheDir=" );

            // 選択アイテムの直前直後に未選択アイテムがあるかどうかを判定
            int clock_begin = items[0].Clock;
            int clock_end = items[items.Count - 1].Clock;
            VsqTrack vsq_track = vsq.Track.get( AppManager.getSelected() );
            VsqEvent tlast_event = null;
            VsqEvent prev = null;
            VsqEvent next = null;
            for( Iterator itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ){
                VsqEvent item_itr = (VsqEvent)itr.next();
                if( item_itr.Clock == clock_begin ){
                    if( tlast_event != null ){
                        // アイテムあり
                        if( tlast_event.Clock + tlast_event.ID.Length == clock_begin ){
                            // ゲートタイム差0で接続
                            prev = (VsqEvent)tlast_event.clone();
                        }else{
                            // 時間差アリで接続
                            prev = (VsqEvent)template.clone();
                            prev.Clock = tlast_event.Clock + tlast_event.ID.Length;
                            prev.ID.Length = clock_begin - (tlast_event.Clock + tlast_event.ID.Length);
                        }
                    }
                }
                tlast_event = item_itr;
                if( item_itr.Clock == clock_end ){
                    if( itr.hasNext() ){
                        VsqEvent foo = (VsqEvent)itr.next();
                        int clock_end_end = clock_end + items[items.Count - 1].ID.Length;
                        if( foo.Clock == clock_end_end ){
                            // ゲートタイム差0で接続
                            next = (VsqEvent)foo.clone();
                        }else{
                            // 時間差アリで接続
                            next = (VsqEvent)template.clone();
                            next.Clock = clock_end_end;
                            next.ID.Length = foo.Clock - clock_end_end;
                        }
                    }
                    break;
                }
            }

//この辺の実装をUstFile#.ctor( VsqFile )で簡単にやりたい！
            // [#PREV]を追加
            if( prev != null ){
                UstEvent e_ust = 
            }

            // ゲートタイムを計算しながら追加
            count = items.Count;
            for( int i = 0; i < count; i++ ){
                VsqEvent itemi = items[i];
            }
        }

        // 起動
        StartPluginArgs arg = new StartPluginArgs();
        arg.exePath = exe_path;
        arg.tmpFile = temp;
        System.Threading.Thread t = new System.Threading.Thread( new System.Threading.ParameterizedThreadStart( runPlugin ) );
        s_finished = false;
        t.Start( arg );
        while( !s_finished ){
            Application.DoEvents();
        }
        
        // 結果を反映
        UstFile ust = null;
        try{
            ust = new UstFile( temp );
        }catch( Exception ex ){
            AppManager.showMessageBox( "invalid ust file; ex=" + ex );
            return ScriptReturnStatus.ERROR;
        }
        int track_count = ust.getTrackCount();
        for( int i = 0; i < track_count; i++ ){
            UstTrack track = ust.getTrack( i );
            int event_count = track.getEventCount();
            for( int j = 0; j < event_count; j++ ){
                UstEvent itemj = track.getEvent( j );
                if( itemj.Index == int.MinValue ){
                    AppManager.showMessageBox( "[#PREV]" );
                }else if( itemj.Index == int.MaxValue ){
                    AppManager.showMessageBox( "[#NEXT]" );
                }else{
                    AppManager.showMessageBox( itemj.Index + "" );
                }
            }
        }

        try{
            //System.IO.File.Delete( temp );
        }catch( Exception ex ){
        }
        return ScriptReturnStatus.ERROR;
    }

    private static void runPlugin( object arg ){
        StartPluginArgs a = (StartPluginArgs)arg;
        string exe_path = a.exePath;
        string temp = a.tmpFile;
        using( System.Diagnostics.Process p = new System.Diagnostics.Process() ){
            p.StartInfo.FileName = exe_path;
            p.StartInfo.Arguments = "\"" + temp + "\"";
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName( exe_path );
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            p.Start();
            p.WaitForExit();
        }
        s_finished = true;
    }
    
    private class StartPluginArgs{
        public string exePath = "";
        public string tmpFile = "";
    }
}
