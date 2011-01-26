using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using org.kbinani.cadencii;
using org.kbinani.java.util;
using org.kbinani.vsq;
using org.kbinani;

public class Utau_Plugin_Invoker : Form {
    class StartPluginArgs {
        public string exePath = "";
        public string tmpFile = "";
    }

    private static string s_plugin_txt_path = @"E:\Program Files\UTAU\plugins\TestUtauScript\plugin.txt";
    private Label lblMessage;
    private static readonly string s_class_name = "Utau_Plugin_Invoker";
    private static readonly string s_display_name = "Utau_Plugin_Invoker";

    private string m_exe_path = "";
    private System.ComponentModel.BackgroundWorker bgWork;
    private string m_temp = "";

    private Utau_Plugin_Invoker( string exe_path, string temp_file ) {
        InitializeComponent();
        m_exe_path = exe_path;
        m_temp = temp_file;
    }

    private void InitializeComponent() {
        this.lblMessage = new System.Windows.Forms.Label();
        this.bgWork = new System.ComponentModel.BackgroundWorker();
        this.SuspendLayout();
        // 
        // lblMessage
        // 
        this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.lblMessage.Location = new System.Drawing.Point( 12, 21 );
        this.lblMessage.Name = "lblMessage";
        this.lblMessage.Size = new System.Drawing.Size( 289, 23 );
        this.lblMessage.TabIndex = 0;
        this.lblMessage.Text = "waiting plugin process...";
        this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // bgWork
        // 
        this.bgWork.DoWork += new System.ComponentModel.DoWorkEventHandler( this.bgWork_DoWork );
        this.bgWork.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.bgWork_RunWorkerCompleted );
        // 
        // Utau_Plugin_Invoker
        // 
        this.ClientSize = new System.Drawing.Size( 313, 119 );
        this.Controls.Add( this.lblMessage );
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "Utau_Plugin_Invoker";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Utau Plugin Invoker";
        this.Load += new System.EventHandler( this.Utau_Plugin_Invoker_Load );
        this.ResumeLayout( false );

    }

    public static ScriptReturnStatus Edit( VsqFileEx vsq )
    {
        Console.WriteLine( "AppManager.getSelectedEventCount()=" + AppManager.getSelectedEventCount() );

        // 選択状態のアイテムがなければ戻る
        if ( AppManager.getSelectedEventCount() <= 0 ) {
            return ScriptReturnStatus.NOT_EDITED;
        }

        // 現在のトラック
        int selected = AppManager.getSelected();
        VsqTrack vsq_track = vsq.Track.get( selected );
        vsq_track.sortEvent();

        // プラグイン情報の定義ファイル(plugin.txt)があるかどうかチェック
        string pluginTxtPath = s_plugin_txt_path;
        if ( pluginTxtPath == "" ) {
            AppManager.showMessageBox( "pluginTxtPath=" + pluginTxtPath );
            return ScriptReturnStatus.ERROR;
        }
        if ( !System.IO.File.Exists( pluginTxtPath ) ) {
            AppManager.showMessageBox( "'" + pluginTxtPath + "' does not exists" );
            return ScriptReturnStatus.ERROR;
        }

        // plugin.txtがあれば，プラグインの実行ファイルのパスを取得する
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
        Console.WriteLine( "exe_path=" + exe_path );
        if ( exe_path == "" ) {
            return ScriptReturnStatus.ERROR;
        }
        if ( !System.IO.File.Exists( exe_path ) ) {
            AppManager.showMessageBox( "'" + exe_path + "' does not exists" );
            return ScriptReturnStatus.ERROR;
        }

        // 選択状態のアイテムの最初と最後がどこか調べる
        int id_start = -1;
        int clock_start = int.MaxValue;
        int id_end = -1;
        int clock_end = int.MinValue;
        for ( Iterator<SelectedEventEntry> itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
            SelectedEventEntry item = itr.next();
            if ( item.original.ID.type != VsqIDType.Anote ) {
                continue;
            }
            int clock = item.original.Clock;
            if ( clock < clock_start ) {
                id_start = item.original.InternalID;
                clock_start = clock;
            }
            clock += item.original.ID.getLength();
            if ( clock_end < clock ) {
                id_end = item.original.InternalID;
                clock_end = clock;
            }
        }
        Console.WriteLine( "id_start=" + id_start );
        Console.WriteLine( "id_end=" + id_end );
        Console.WriteLine( "#1; (clock_start,clock_end)=" + "(" + clock_start + "," + clock_end + ")" );

        // 選択範囲の前後の音符を探す
        VsqEvent ve_prev = null;
        VsqEvent ve_next = null;
        VsqEvent l = null;
        for ( Iterator<VsqEvent> itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {
            VsqEvent item = itr.next();
            if ( item.InternalID == id_start ) {
                if ( l != null ) {
                    ve_prev = l;
                }
            }
            if ( l != null ) {
                if ( l.InternalID == id_end ) {
                    ve_next = item;
                }
            }
            l = item;
            if ( ve_prev != null && ve_next != null ) {
                break;
            }
        }
        if ( ve_prev != null ) {
            Console.WriteLine( "ve_prev.InternalID=" + ve_prev.InternalID );
        }
        if ( ve_next != null ) {
            Console.WriteLine( "ve_next.InternalID=" + ve_next.InternalID );
        }
        int next_rest_clock = -1;
        bool prev_is_rest = false;
        if ( ve_prev != null ) {
            // 直前の音符がある場合
            if ( ve_prev.Clock + ve_prev.ID.getLength() == clock_start ) {
                // 接続している
                clock_start = ve_prev.Clock;
            } else {
                // 接続していない
                clock_start = ve_prev.Clock + ve_prev.ID.getLength();
            }
        } else {
            // 無い場合
            if ( vsq.getPreMeasureClocks() < clock_start ) {
                prev_is_rest = true;
            }
            clock_start = vsq.getPreMeasureClocks();
        }
        if ( ve_next != null ) {
            // 直後の音符がある場合
            if ( ve_next.Clock == clock_end ) {
                // 接続している
                clock_end = ve_next.Clock + ve_next.ID.getLength();
            } else {
                // 接続していない
                next_rest_clock = clock_end;
                clock_end = ve_next.Clock;
            }
        }
        Console.WriteLine( "#2; (clock_start,clock_end)=" + "(" + clock_start + "," + clock_end + ")" );

        // 作業用のVSQに，選択範囲のアイテムを格納
        VsqFile v = new VsqFile( "Miku", 1, 4, 4, 500000 );
        VsqTrack v_track = v.Track.get( 1 );
        for ( Iterator<VsqEvent> itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {
            VsqEvent item = itr.next();
            if ( clock_start <= item.Clock && item.Clock + item.ID.getLength() <= clock_end ) {
                v_track.addEvent( (VsqEvent)item.clone() );
            }
        }
        // 最後のRを手動で追加．これは自動化できない
        if ( next_rest_clock != -1 ) {
            VsqEvent item = (VsqEvent)ve_next.clone();
            item.ID.LyricHandle.L0.Phrase = "R";
            item.Clock = next_rest_clock;
            item.ID.setLength( clock_end - next_rest_clock );
            v_track.addEvent( item );
        }
        // 0～選択範囲の開始位置までを削除する
        v.removePart( 0, clock_start );

        // vsq -> ustに変換
        // キーがustのIndex, 値がInternalID
        Vector<ValuePair<int, int>> map = new Vector<ValuePair<int, int>>();
        UstFile u = new UstFile( v, 1, map );

        // PREV, NEXTのIndex値を設定する
        if ( ve_prev != null || prev_is_rest ) {
            u.getTrack( 0 ).getEvent( 0 ).Index = UstFile.PREV_INDEX;
        }
        if ( ve_next != null ) {
            u.getTrack( 0 ).getEvent( u.getTrack( 0 ).getEventCount() - 1 ).Index = UstFile.NEXT_INDEX;
        }

        // ustファイルに出力
        UstFileWriteOptions option = new UstFileWriteOptions();
        option.settingCacheDir = false;
        option.settingOutFile = false;
        option.settingProjectName = false;
        option.settingTempo = true;
        option.settingTool1 = true;
        option.settingTool2 = true;
        option.settingTracks = false;
        option.settingVoiceDir = true;
        option.trackEnd = false;
        string temp = Path.GetTempFileName();
        u.write( temp, option );

        // プラグインの実行ファイルを起動
        Utau_Plugin_Invoker dialog = new Utau_Plugin_Invoker( exe_path, temp );
        dialog.ShowDialog();

        // TODO: このへんから, 上にあるPREV, ENDのところも何とかすること．
        return ScriptReturnStatus.ERROR;
    }

    public static ScriptReturnStatus _Edit( VsqFileEx vsq )
    {
        if ( AppManager.getSelectedEventCount() <= 0 ) {
            return ScriptReturnStatus.NOT_EDITED;
        }

        int selected = AppManager.getSelected();
        
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
        int num_selected = 0; // 選択されていた音符の個数
        for ( Iterator<SelectedEventEntry> itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
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
        VsqTrack vsq_track = vsq.Track.get( selected );
        VsqEvent tlast_event = null;
        VsqEvent prev = null;
        VsqEvent next = null;

        // VsqFile -> UstFileのコンバート
        VsqFile conv = new VsqFile( "Miku", 2, 4, 4, (int)(6e7 / TEMPO) );
        VsqTrack conv_track = conv.Track.get( 1 );
        for ( Iterator<VsqEvent> itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {
            VsqEvent item_itr = itr.next();
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
            conv_track.addEvent( prev, prev.InternalID );
        }

        // ゲートタイムを計算しながら追加
        count = items.Count;
        for ( int i = 0; i < count; i++ ) {
            VsqEvent itemi = items[i];
            itemi.Clock -= clock_begin;
            conv_track.addEvent( itemi, itemi.InternalID );
        }

        // [#NEXT]を追加
        if ( next != null ) {
            next.Clock -= clock_begin;
            conv_track.addEvent( next, next.InternalID );
        }

        // PIT, PBSを追加
        copyCurve( vsq_track.getCurve( "pit" ), conv_track.getCurve( "pit" ), clock_begin );
        copyCurve( vsq_track.getCurve( "pbs" ), conv_track.getCurve( "pbs" ), clock_begin );

        string temp = Path.GetTempFileName();
        Vector<ValuePair<int, int>> map_id = new Vector<ValuePair<int, int>>(); // キーが[#   ]の番号、値がInternalID
        UstFile tust = new UstFile( conv, 1, map_id );

        Console.WriteLine( "Utau_Plugin_Invoker#Edit;" );
        for ( int i = 0; i < map_id.size(); i++ ) {
            ValuePair<int, int> itemi = map_id.get( i );
            Console.WriteLine( "[#" + PortUtil.formatDecimal( "0000", itemi.getKey() ) + "] => " + itemi.getValue() );
        }

        VsqEvent singer_event = vsq.Track.get( 1 ).getSingerEventAt( clock_begin );
        string voice_dir = "";
        SingerConfig sc = AppManager.getSingerInfoUtau( singer_event.ID.IconHandle.Language, singer_event.ID.IconHandle.Program );
        if ( sc != null ) {
            voice_dir = sc.VOICEIDSTR;
        }
        tust.setVoiceDir( voice_dir );

        if ( prev != null ) {
            tust.getTrack( 0 ).getEvent( 0 ).Index = UstFile.PREV_INDEX;
        }
        if ( next != null ) {
            tust.getTrack( 0 ).getEvent( tust.getTrack( 0 ).getEventCount() - 1 ).Index = UstFile.NEXT_INDEX;
        }
        UstFileWriteOptions options = new UstFileWriteOptions();
        options.settingTempo = true;
        options.settingVoiceDir = true;
        options.settingCacheDir = true;
        tust.write( temp, options );

        // 起動 -----------------------------------------------------------------------------
        Utau_Plugin_Invoker dialog = new Utau_Plugin_Invoker( exe_path, temp );
        dialog.ShowDialog();

        // 結果を反映 -----------------------------------------------------------------------
        List<int> pit_added_ids = new List<int>(); // Pitchesが追加されたので、後でPIT, PBSに反映させる処理が必要なVsqEventの、InternalID
        VsqEvent dustbox = new VsqEvent(); // Lyric=Rのプロパティを捨てるためのゴミ箱
        dustbox.ID.LyricHandle = new LyricHandle( "a", "a" );
        using ( StreamReader sr = new StreamReader( temp, Encoding.GetEncoding( 932 ) ) ) {
            string line = "";
            string current_parse = "";
            int clock = clock_begin;
            int tlength = 0;
            int index = -1; // 先頭から何番目の音符か？map_id.get( index ).getKey()が、現在処理中のUstEvent.Index, map_id.get( index ).getValue()が、現在処理中のVsqEvent.InternalID
            while ( (line = sr.ReadLine()) != null ) {
                if ( line.StartsWith( "[#" ) ){
                    current_parse = line;
                    clock += tlength;
                    if ( line != "[#SETTING]" && line != "[#TRACKEND]" && line != "[#INSERT]" ) {
                        index++;
                    }
                    if ( current_parse == "[#INSERT]" ) {
                        VsqEvent newitem = new VsqEvent();
                        newitem.Clock = clock;
                        newitem.ID.setLength( 0 );
                        newitem.ID.type = VsqIDType.Anote;
                        int id = vsq_track.addEvent( newitem );
                        int max_num = -1;
                        for ( Iterator<ValuePair<int, int>> itr = map_id.iterator(); itr.hasNext(); ) {
                            int num = itr.next().getKey();
                            max_num = Math.Max( max_num, num );
                        }
                        max_num++;
                        map_id.Add( new ValuePair<int, int>( max_num, id ) ); // 末尾に追加されるので、indexとの整合性は破綻しない
                        current_parse = "[#" + PortUtil.formatDecimal( "0000", max_num ) + "]";
                    } else if ( current_parse == "[#DELETE]" ) {
                        int internal_id = map_id.get( index ).getValue();
                        int i = vsq_track.findEventIndexFromID( internal_id );

                        Console.WriteLine( "Utau_Plugin_Invoker#Edit; DELETE; internal_id=" + internal_id + "; index=" + i );

                        if ( 0 <= i  && i < vsq_track.getEventCount() ) {
                            vsq_track.removeEvent( i );
                        }
                    }
                    continue;
                }

                if ( current_parse == "[#SETTING]" ) {
                } else if ( current_parse == "[#TRACKEND]" ) {
                } else if ( current_parse == "[#PREV]" ) {
                } else if ( current_parse == "[#NEXT]" ) {
                //} else if ( current_parse == "[#INSERT]" ) { NEVER ENEBLE THIS LINE!!
                } else if ( current_parse == "[#DELETE]" ) {
                    //do nothing
                } else if ( current_parse.StartsWith( "[#" ) ) {
                    int indx_blacket = current_parse.IndexOf( ']' );
                    string str_num = current_parse.Substring( 2, indx_blacket - 2 );
                    int num = -1;
                    if ( !int.TryParse( str_num, out num ) ) {
                        continue;
                    }
                    int id = -1;
                    bool found = false;
                    for ( Iterator<ValuePair<int, int>> itr = map_id.iterator(); itr.hasNext(); ) {
                        ValuePair<int, int> item = itr.next();
                        if ( num == item.getKey() ) {
                            id = item.getValue();
                            found = true;
                            break;
                        }
                    }
                    if ( !found ) {
                        continue;
                    }
                    VsqEvent target = vsq_track.findEventFromID( id );
                    if ( target == null ) {
                        target = dustbox;
                    }
                    target.Clock = clock;
                    tlength = target.ID.getLength();
                    if ( target.UstEvent == null ) {
                        target.UstEvent = new UstEvent();
                    }
                    string[] spl = line.Split( '=' );
                    if ( spl.Length < 2 ) {
                        continue;
                    }
                    string left = spl[0];
                    string right = spl[1];
                    if ( left == "Length" ) {
                        int v = target.ID.getLength();
                        try {
                            v = int.Parse( right );
                            target.ID.setLength( v );
                            target.UstEvent.Length = v;
                            tlength = v;
                        } catch {
                        }
                    } else if ( left == "Lyric" ) {
                        if ( target.ID.LyricHandle == null ) {
                            target.ID.LyricHandle = new LyricHandle( "あ", "a" );
                        }
                        target.ID.LyricHandle.L0.Phrase = right;
                        target.UstEvent.Lyric = right;
                        AppManager.changePhrase( vsq, selected, target, clock, right );
                    } else if ( left == "NoteNum" ) {
                        int v = target.ID.Note;
                        try {
                            v = int.Parse( right );
                            target.ID.Note = v;
                            target.UstEvent.Note = v;
                        } catch {
                        }
                    } else if ( left == "Intensity" ) {
                        int v = target.ID.Dynamics;
                        try {
                            v = int.Parse( right );
                            target.UstEvent.Intensity = v;
                        } catch {
                        }
                    } else if ( left == "PBType" ) {
                        int v = target.UstEvent.PBType;
                        try {
                            v = int.Parse( right );
                            target.UstEvent.PBType = v;
                        } catch {
                        }
                    } else if ( left == "Piches" ) {
                        string[] spl2 = right.Split( ',' );
                        float[] t = new float[spl2.Length];
                        for ( int i = 0; i < spl2.Length; i++ ) {
                            float v = 0;
                            try {
                                v = float.Parse( spl2[i] );
                                t[i] = v;
                            } catch {
                            }
                        }
                        target.UstEvent.Pitches = t;

                        if ( !pit_added_ids.Contains( id ) ) {
                            pit_added_ids.Add( id );
                        }
                    } else if ( left == "Tempo" ) {
                        float v = 125f;
                        try {
                            v = float.Parse( right );
                            vsq.TempoTable.add( new TempoTableEntry( target.Clock, (int)(60e6 / v), 0.0 ) );
                            vsq.updateTempoInfo();
                        } catch {
                        }
                    } else if ( left == "VBR" ) {
                        target.UstEvent.Vibrato = new UstVibrato( line );
                    } else if ( left == "PBW" ||
                                left == "PBS" ||
                                left == "PBY" ||
                                left == "PBM" ) {
                        if ( target.UstEvent.Portamento == null ) {
                            target.UstEvent.Portamento = new UstPortamento();
                        }
                        target.UstEvent.Portamento.ParseLine( line );
                    } else if ( left == "Envelope" ) {
                        target.UstEvent.Envelope = new UstEnvelope( line );
                    } else if ( left == "VoiceOverlap" ) {
                        float v = target.UstEvent.VoiceOverlap;
                        try {
                            v = float.Parse( right );
                            target.UstEvent.VoiceOverlap = v;
                        } catch {
                        }
                    } else if ( left == "PreUtterance" ) {
                        float v = target.UstEvent.PreUtterance;
                        try {
                            v = float.Parse( right );
                            target.UstEvent.PreUtterance = v;
                        } catch {
                        }
                    }
                }
            }
        }

        // PitchesをPIT, PBSに反映させる処理
        VsqBPList cpit = vsq_track.getCurve( "pit" );
        if ( cpit == null ) {
            cpit = new VsqBPList( CurveType.PIT.getName(),
                                  CurveType.PIT.getDefault(),
                                  CurveType.PIT.getMinimum(),
                                  CurveType.PIT.getMaximum() );
            vsq_track.setCurve( "pit", cpit );
        }
        VsqBPList cpbs = vsq_track.getCurve( "pbs" );
        if ( cpbs == null ) {
            cpbs = new VsqBPList( CurveType.PBS.getName(),
                                  CurveType.PBS.getDefault(),
                                  CurveType.PBS.getMinimum(),
                                  CurveType.PBS.getMaximum() );
            vsq_track.setCurve( "pbs", cpbs );
        }
        for ( int i = 0; i < pit_added_ids.Count; i++ ) {
            int internal_id = pit_added_ids[i];
            VsqEvent target = vsq_track.findEventFromID( internal_id );
            if ( target == null ) {
                continue;
            }

            // ピッチベンド絶対値の最大値を調べる
            float abs_pit_max = 0;
            for ( int j = 0; j < target.UstEvent.Pitches.Length; j++ ) {
                abs_pit_max = Math.Max( abs_pit_max, Math.Abs( target.UstEvent.Pitches[j] ) );
            }

            // ピッチベンドを表現するのに最低限必要なPBSを調べる。
            int pbs = (int)(abs_pit_max / 100.0);
            if ( pbs * 100 != abs_pit_max ) {
                // abs_pit_maxが100の倍数で無い場合。
                pbs++;
            }
            if ( pbs < 1 ) {
                pbs = 1;
            }
            if ( CurveType.PBS.getMaximum() < pbs ){
                pbs = CurveType.PBS.getMaximum();
            }

            // これからPITをいじる範囲内のPBSが、pbsと違う値になっていた場合の処理
            double sec_pitstart = vsq.getSecFromClock( target.Clock ) - target.UstEvent.PreUtterance / 1000.0;
            int pit_start = (int)vsq.getClockFromSec( sec_pitstart );
            int pbtype = target.UstEvent.PBType;
            if ( pbtype < 1 ) {
                pbtype = 5;
            }
            target.UstEvent.PBType = pbtype;
            int pit_end = pit_start + pbtype * target.UstEvent.Pitches.Length;
            int pbs_at_pitend = cpbs.getValue( pit_end - 1 );
            for ( int j = 0; j < cpbs.size(); ) {
                int jclock = cpbs.getKeyClock( j );
                if ( pit_start <= jclock && jclock < pit_end ) {
                    int jpbs = cpbs.getElement( j );
                    if ( jpbs != pbs ) {
                        cpbs.removeElementAt( j );
                    } else {
                        j++;
                    }
                } else if ( pit_end < jclock ) {
                    break;
                } else {
                    j++;
                }
            }
            if ( cpbs.getValue( pit_start ) != pbs ) {
                cpbs.add( pit_start, pbs );
            }
            if ( pbs_at_pitend != pbs ) {
                cpbs.add( pit_end, pbs_at_pitend );
            }

            // これからPITをいじる範囲にPITが指定されていたら削除する
            int pit_at_pitend = cpit.getValue( pit_end );
            for ( int j = 0; j < cpit.size(); ) {
                int jclock = cpit.getKeyClock( j );
                if ( pit_start <= jclock && jclock < pit_end ) {
                    cpit.removeElementAt( j );
                } else if ( pit_end < jclock ) {
                    break;
                } else {
                    j++;
                }
            }

            // PITを追加。
            int lastpit = CurveType.PIT.getMinimum() - 1;
            for ( int j = 0; j < target.UstEvent.Pitches.Length; j++ ) {
                int jclock = pit_start + j * pbtype;
                int pit = (int)(8192.0 * target.UstEvent.Pitches[j] / 100.0 / (double)pbs);
                if ( pit < CurveType.PIT.getMinimum() ) {
                    pit = CurveType.PIT.getMinimum();
                } else if ( CurveType.PIT.getMaximum() < pit ) {
                    pit = CurveType.PIT.getMaximum();
                }
                if ( pit != lastpit ) {
                    cpit.add( jclock, pit );
                    lastpit = pit;
                }
            }
            if ( cpit.getValue( pit_end ) != pit_at_pitend ) {
                cpit.add( pit_end, pit_at_pitend );
            }
        }

        try {
            System.IO.File.Delete( temp );
        } catch ( Exception ex ) {
        }
        return ScriptReturnStatus.EDITED;
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

    private void Utau_Plugin_Invoker_Load( object sender, EventArgs e ) {
        bgWork.RunWorkerAsync();
    }

    private void bgWork_DoWork( object sender, System.ComponentModel.DoWorkEventArgs e ) {
        string dquote = new string( (char)0x22, 1 );
        using ( System.Diagnostics.Process p = new System.Diagnostics.Process() ) {
            p.StartInfo.FileName = m_exe_path;
            p.StartInfo.Arguments = dquote + m_temp + dquote;
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName( m_exe_path );
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            p.Start();
            p.WaitForExit();
        }
    }

    private void bgWork_RunWorkerCompleted( object sender, System.ComponentModel.RunWorkerCompletedEventArgs e ) {
        this.Close();
    }

    public static String GetDisplayName() {
        return s_display_name;
    }
}
