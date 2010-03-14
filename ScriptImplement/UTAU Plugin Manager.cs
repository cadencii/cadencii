using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using org.kbinani;
using org.kbinani.apputil;
using org.kbinani.cadencii;

class UtauPluginManager : Form {
    delegate void VoidDelegate();
    
    private Button btnOk;
    private Button btnAdd;
    private Button btnRemove;
    private Button btnCancel;
    /// <summary>
    /// この部分は、Utau Plugin Invoker.csについて、以下のテキスト処理を行ったもの。
    /// 1. 「"」を「\"」に置換（テキストモード）
    /// 2. 「\n」を「" +\n"」に置換（正規表現モード）
    /// 3. 「Utau_Plugin_Invoker」を「{0}」に置換。
    /// 4. 「s_plugin_txt_path = @"E:\Program Files\UTAU\..(一部略)..";」を「s_plugin_txt_path = @"{1}";」に書き換え。
    /// </summary>
    private static readonly String TEXT = "" +
        "using System;" +
        "using System.Collections.Generic;" +
        "using System.IO;" +
        "using System.Text;" +
        "using System.Windows.Forms;" +
        "using System.Threading;" +
        "using org.kbinani.cadencii;" +
        "using org.kbinani.java.util;" +
        "using org.kbinani.vsq;" +
        "" +
        "public class {0} : Form {" +
        "    class StartPluginArgs {" +
        "        public string exePath = \"\";" +
        "        public string tmpFile = \"\";" +
        "    }" +
        "" +
        "    private static string s_plugin_txt_path = @\"{1}\";" +
        "    private Label lblMessage;" +
        "    private static readonly string s_class_name = \"{0}\";" +
        "    private static readonly string s_display_name = \"{0}\";" +
        "" +
        "    private string m_exe_path = \"\";" +
        "    private System.ComponentModel.BackgroundWorker bgWork;" +
        "    private string m_temp = \"\";" +
        "" +
        "    private {0}( string exe_path, string temp_file ) {" +
        "        InitializeComponent();" +
        "        m_exe_path = exe_path;" +
        "        m_temp = temp_file;" +
        "    }" +
        "" +
        "    private void InitializeComponent() {" +
        "        this.lblMessage = new System.Windows.Forms.Label();" +
        "        this.bgWork = new System.ComponentModel.BackgroundWorker();" +
        "        this.SuspendLayout();" +
        "        // " +
        "        // lblMessage" +
        "        // " +
        "        this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)" +
        "                    | System.Windows.Forms.AnchorStyles.Right)));" +
        "        this.lblMessage.Location = new System.Drawing.Point( 12, 21 );" +
        "        this.lblMessage.Name = \"lblMessage\";" +
        "        this.lblMessage.Size = new System.Drawing.Size( 289, 23 );" +
        "        this.lblMessage.TabIndex = 0;" +
        "        this.lblMessage.Text = \"waiting plugin process...\";" +
        "        this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;" +
        "        // " +
        "        // bgWork" +
        "        // " +
        "        this.bgWork.DoWork += new System.ComponentModel.DoWorkEventHandler( this.bgWork_DoWork );" +
        "        this.bgWork.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.bgWork_RunWorkerCompleted );" +
        "        // " +
        "        // {0}" +
        "        // " +
        "        this.ClientSize = new System.Drawing.Size( 313, 119 );" +
        "        this.Controls.Add( this.lblMessage );" +
        "        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;" +
        "        this.MaximizeBox = false;" +
        "        this.MinimizeBox = false;" +
        "        this.Name = \"{0}\";" +
        "        this.ShowIcon = false;" +
        "        this.ShowInTaskbar = false;" +
        "        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;" +
        "        this.Text = \"Utau Plugin Invoker\";" +
        "        this.Load += new System.EventHandler( this.{0}_Load );" +
        "        this.ResumeLayout( false );" +
        "" +
        "    }" +
        "" +
        "    public static ScriptReturnStatus Edit( VsqFileEx vsq ) {" +
        "        if ( AppManager.getSelectedEventCount() <= 0 ) {" +
        "            return ScriptReturnStatus.NOT_EDITED;" +
        "        }" +
        "" +
        "        int selected = AppManager.getSelected();" +
        "        " +
        "        string pluginTxtPath = s_plugin_txt_path;" +
        "        if ( pluginTxtPath == \"\" ) {" +
        "            AppManager.showMessageBox( \"pluginTxtPath=\" + pluginTxtPath );" +
        "            return ScriptReturnStatus.ERROR;" +
        "        }" +
        "        if ( !System.IO.File.Exists( pluginTxtPath ) ) {" +
        "            AppManager.showMessageBox( \"'\" + pluginTxtPath + \"' does not exists\" );" +
        "            return ScriptReturnStatus.ERROR;" +
        "        }" +
        "" +
        "        System.Text.Encoding shift_jis = System.Text.Encoding.GetEncoding( \"Shift_JIS\" );" +
        "        string name = \"\";" +
        "        string exe_path = \"\";" +
        "        using ( StreamReader sr = new StreamReader( pluginTxtPath, shift_jis ) ) {" +
        "            string line = \"\";" +
        "            while ( (line = sr.ReadLine()) != null ) {" +
        "                string[] spl = line.Split( new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries );" +
        "                if ( line.StartsWith( \"name=\" ) ) {" +
        "                    name = spl[1];" +
        "                } else if ( line.StartsWith( \"execute=\" ) ) {" +
        "                    exe_path = Path.Combine( Path.GetDirectoryName( pluginTxtPath ), spl[1] );" +
        "                }" +
        "            }" +
        "        }" +
        "        if ( exe_path == \"\" ) {" +
        "            return ScriptReturnStatus.ERROR;" +
        "        }" +
        "        if ( !System.IO.File.Exists( exe_path ) ) {" +
        "            AppManager.showMessageBox( \"'\" + exe_path + \"' does not exists\" );" +
        "            return ScriptReturnStatus.ERROR;" +
        "        }" +
        "" +
        "        // ustを用意 ------------------------------------------------------------------------" +
        "        // 方針は，一度VsqFileに音符を格納->UstFile#.ctor( VsqFile )を使って一括変換" +
        "        // メイン画面で選択されているアイテムを列挙" +
        "        List<VsqEvent> items = new List<VsqEvent>(); // Ustに追加する音符のリスト" +
        "        List<int> map_id = new List<int>(); // ustの[#index]が、map_id[index].InternalIDというIDを持つVsqEventに相当することを記録しておくリスト" +
        "        int num_selected = 0; // 選択されていた音符の個数" +
        "        for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {" +
        "            SelectedEventEntry item_itr = (SelectedEventEntry)itr.next();" +
        "            if ( item_itr.original.ID.type == VsqIDType.Anote ) {" +
        "                items.Add( (VsqEvent)item_itr.original.clone() );" +
        "                num_selected++;" +
        "            }" +
        "        }" +
        "        items.Sort();" +
        "" +
        "        // R用音符のテンプレート" +
        "        VsqEvent template = new VsqEvent();" +
        "        template.ID.type = VsqIDType.Anote;" +
        "        template.ID.LyricHandle = new LyricHandle( \"R\", \"\" );" +
        "        template.InternalID = -1; // VSQ上には実際に存在しないダミーであることを表す" +
        "" +
        "        int count = items.Count;" +
        "        // アイテムが2個以上の場合は、間にRを挿入する必要があるかどうか判定" +
        "        if ( count > 1 ) {" +
        "            List<VsqEvent> add = new List<VsqEvent>(); // 追加するR" +
        "            VsqEvent last_event = items[0];" +
        "            for ( int i = 1; i < count; i++ ) {" +
        "                VsqEvent item = items[i];" +
        "                if ( last_event.Clock + last_event.ID.Length < item.Clock ) {" +
        "                    VsqEvent add_temp = (VsqEvent)template.clone();" +
        "                    add_temp.Clock = last_event.Clock + last_event.ID.Length;" +
        "                    add_temp.ID.Length = item.Clock - add_temp.Clock;" +
        "                    add.Add( add_temp );" +
        "                }" +
        "                last_event = item;" +
        "            }" +
        "            foreach ( VsqEvent v in add ) {" +
        "                items.Add( v );" +
        "            }" +
        "            items.Sort();" +
        "        }" +
        "" +
        "        // ヘッダ" +
        "        int TEMPO = 120;" +
        "" +
        "        // 選択アイテムの直前直後に未選択アイテムがあるかどうかを判定" +
        "        int clock_begin = items[0].Clock;" +
        "        int clock_end = items[items.Count - 1].Clock;" +
        "        int clock_end_end = clock_end + items[items.Count - 1].ID.Length;" +
        "        VsqTrack vsq_track = vsq.Track.get( selected );" +
        "        VsqEvent tlast_event = null;" +
        "        VsqEvent prev = null;" +
        "        VsqEvent next = null;" +
        "" +
        "        // VsqFile -> UstFileのコンバート" +
        "        VsqFile conv = new VsqFile( \"Miku\", 2, 4, 4, (int)(6e7 / TEMPO) );" +
        "        VsqTrack conv_track = conv.Track.get( 1 );" +
        "        for ( Iterator itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {" +
        "            VsqEvent item_itr = (VsqEvent)itr.next();" +
        "            if ( item_itr.Clock == clock_begin ) {" +
        "                if ( tlast_event != null ) {" +
        "                    // アイテムあり" +
        "                    if ( tlast_event.Clock + tlast_event.ID.Length == clock_begin ) {" +
        "                        // ゲートタイム差0で接続" +
        "                        prev = (VsqEvent)tlast_event.clone();" +
        "                    } else {" +
        "                        // 時間差アリで接続" +
        "                        prev = (VsqEvent)template.clone();" +
        "                        prev.Clock = tlast_event.Clock + tlast_event.ID.Length;" +
        "                        prev.ID.Length = clock_begin - (tlast_event.Clock + tlast_event.ID.Length);" +
        "                    }" +
        "                }" +
        "            }" +
        "            tlast_event = item_itr;" +
        "            if ( item_itr.Clock == clock_end ) {" +
        "                if ( itr.hasNext() ) {" +
        "                    VsqEvent foo = (VsqEvent)itr.next();" +
        "                    if ( foo.Clock == clock_end_end ) {" +
        "                        // ゲートタイム差0で接続" +
        "                        next = (VsqEvent)foo.clone();" +
        "                    } else {" +
        "                        // 時間差アリで接続" +
        "                        next = (VsqEvent)template.clone();" +
        "                        next.Clock = clock_end_end;" +
        "                        next.ID.Length = foo.Clock - clock_end_end;" +
        "                    }" +
        "                }" +
        "                break;" +
        "            }" +
        "" +
        "        }" +
        "" +
        "        // [#PREV]を追加" +
        "        if ( prev != null ) {" +
        "            prev.Clock -= clock_begin;" +
        "            conv_track.addEvent( prev );" +
        "        }" +
        "" +
        "        // ゲートタイムを計算しながら追加" +
        "        count = items.Count;" +
        "        for ( int i = 0; i < count; i++ ) {" +
        "            VsqEvent itemi = items[i];" +
        "            itemi.Clock -= clock_begin;" +
        "            conv_track.addEvent( itemi );" +
        "            map_id.Add( itemi.InternalID );" +
        "        }" +
        "" +
        "        // [#NEXT]を追加" +
        "        if ( next != null ) {" +
        "            next.Clock -= clock_begin;" +
        "            conv_track.addEvent( next );" +
        "        }" +
        "" +
        "        // PIT, PBSを追加" +
        "        copyCurve( vsq_track.getCurve( \"pit\" ), conv_track.getCurve( \"pit\" ), clock_begin );" +
        "        copyCurve( vsq_track.getCurve( \"pbs\" ), conv_track.getCurve( \"pbs\" ), clock_begin );" +
        "" +
        "        string temp = Path.GetTempFileName();" +
        "        UstFile tust = new UstFile( conv, 1 );" +
        "        VsqEvent singer_event = vsq.Track.get( 1 ).getSingerEventAt( clock_begin );" +
        "        string voice_dir = \"\";" +
        "        SingerConfig sc = AppManager.getSingerInfoUtau( singer_event.ID.IconHandle.Language, singer_event.ID.IconHandle.Program );" +
        "        if ( sc != null ) {" +
        "            voice_dir = sc.VOICEIDSTR;" +
        "        }" +
        "        tust.setVoiceDir( voice_dir );" +
        "" +
        "        if ( prev != null ) {" +
        "            tust.getTrack( 0 ).getEvent( 0 ).Index = int.MinValue;" +
        "        }" +
        "        if ( next != null ) {" +
        "            tust.getTrack( 0 ).getEvent( tust.getTrack( 0 ).getEventCount() - 1 ).Index = int.MaxValue;" +
        "        }" +
        "        tust.write( temp, false );" +
        "" +
        "        // 起動 -----------------------------------------------------------------------------" +
        "        {0} dialog = new {0}( exe_path, temp );" +
        "        dialog.ShowDialog();" +
        "" +
        "        // 結果を反映 -----------------------------------------------------------------------" +
        "        List<int> pit_added_ids = new List<int>(); // Pitchesが追加されたので、後でPIT, PBSに反映させる処理が必要なVsqEventの、InternalID" +
        "        using ( StreamReader sr = new StreamReader( temp, Encoding.GetEncoding( 932 ) ) ) {" +
        "            string line = \"\";" +
        "            string current_parse = \"\";" +
        "            int clock = clock_begin;" +
        "            int tlength = 0;" +
        "            while ( (line = sr.ReadLine()) != null ) {" +
        "                if ( line.StartsWith( \"[#\" ) ){" +
        "                    current_parse = line;" +
        "                    clock += tlength;" +
        "                    continue;" +
        "                }" +
        "" +
        "                if ( current_parse == \"[#SETTING]\" ) {" +
        "                } else if ( current_parse == \"[#TRACKEND]\" ) {" +
        "                } else if ( current_parse == \"[#PREV]\" ) {" +
        "                } else if ( current_parse == \"[#NEXT]\" ) {" +
        "                } else if ( current_parse.StartsWith( \"[#\" ) ) {" +
        "                    int indx_blacket = current_parse.IndexOf( ']' );" +
        "                    string str_num = current_parse.Substring( 2, indx_blacket - 2 );" +
        "                    int num = -1;" +
        "                    if ( !int.TryParse( str_num, out num ) ) {" +
        "                        continue;" +
        "                    }" +
        "                    if ( num < 0 || map_id.Count <= num ) {" +
        "                        continue;" +
        "                    }" +
        "                    VsqEvent target = vsq_track.findEventFromID( map_id[num] );" +
        "                    if ( target == null ) {" +
        "                        continue;" +
        "                    }" +
        "                    if ( target.UstEvent == null ) {" +
        "                        target.UstEvent = new UstEvent();" +
        "                    }" +
        "                    string[] spl = line.Split( '=' );" +
        "                    if ( spl.Length < 2 ) {" +
        "                        continue;" +
        "                    }" +
        "                    string left = spl[0];" +
        "                    string right = spl[1];" +
        "                    if ( left == \"Length\" ) {" +
        "                        int v = target.ID.getLength();" +
        "                        try {" +
        "                            v = int.Parse( right );" +
        "                            target.ID.setLength( v );" +
        "                            target.UstEvent.Length = v;" +
        "                            tlength = v;" +
        "                        } catch {" +
        "                        }" +
        "                    } else if ( left == \"Lyric\" ) {" +
        "                        if ( target.ID.LyricHandle == null ) {" +
        "                            target.ID.LyricHandle = new LyricHandle( \"あ\", \"a\" );" +
        "                        }" +
        "                        target.ID.LyricHandle.L0.Phrase = right;" +
        "                        target.UstEvent.Lyric = right;" +
        "                        AppManager.changePhrase( vsq, selected, target, clock, right );" +
        "                    } else if ( left == \"NoteNum\" ) {" +
        "                        int v = target.ID.Note;" +
        "                        try {" +
        "                            v = int.Parse( right );" +
        "                            target.ID.Note = v;" +
        "                            target.UstEvent.Note = v;" +
        "                        } catch {" +
        "                        }" +
        "                    } else if ( left == \"Intensity\" ) {" +
        "                        int v = target.ID.Dynamics;" +
        "                        try {" +
        "                            v = int.Parse( right );" +
        "                            target.ID.Dynamics = v;" +
        "                            target.UstEvent.Intensity = v;" +
        "                        } catch {" +
        "                        }" +
        "                    } else if ( left == \"PBType\" ) {" +
        "                        int v = target.UstEvent.PBType;" +
        "                        try {" +
        "                            v = int.Parse( right );" +
        "                            target.UstEvent.PBType = v;" +
        "                        } catch {" +
        "                        }" +
        "                    } else if ( left == \"Piches\" ) {" +
        "                        string[] spl2 = right.Split( ',' );" +
        "                        float[] t = new float[spl2.Length];" +
        "                        for ( int i = 0; i < spl2.Length; i++ ) {" +
        "                            float v = 0;" +
        "                            try {" +
        "                                v = float.Parse( spl2[i] );" +
        "                                t[i] = v;" +
        "                            } catch {" +
        "                            }" +
        "                        }" +
        "                        target.UstEvent.Pitches = t;" +
        "" +
        "                        if ( !pit_added_ids.Contains( map_id[num] ) ) {" +
        "                            pit_added_ids.Add( map_id[num] );" +
        "                        }" +
        "                    } else if ( left == \"Tempo\" ) {" +
        "                        float v = 125f;" +
        "                        try {" +
        "                            v = float.Parse( right );" +
        "                            vsq.TempoTable.add( new TempoTableEntry( target.Clock, (int)(60e6 / v), 0.0 ) );" +
        "                            vsq.updateTempoInfo();" +
        "                        } catch {" +
        "                        }" +
        "                    } else if ( left == \"VBR\" ) {" +
        "                        target.UstEvent.Vibrato = new UstVibrato( line );" +
        "                    } else if ( left == \"PBW\" ||" +
        "                                left == \"PBS\" ||" +
        "                                left == \"PBY\" ||" +
        "                                left == \"PBM\" ) {" +
        "                        if ( target.UstEvent.Portamento == null ) {" +
        "                            target.UstEvent.Portamento = new UstPortamento();" +
        "                        }" +
        "                        target.UstEvent.Portamento.ParseLine( line );" +
        "                    } else if ( left == \"Envelope\" ) {" +
        "                        target.UstEvent.Envelope = new UstEnvelope( line );" +
        "                    } else if ( left == \"VoiceOverlap\" ) {" +
        "                        float v = target.UstEvent.VoiceOverlap;" +
        "                        try {" +
        "                            v = float.Parse( right );" +
        "                            target.UstEvent.VoiceOverlap = v;" +
        "                        } catch {" +
        "                        }" +
        "                    } else if ( left == \"PreUtterance\" ) {" +
        "                        float v = target.UstEvent.PreUtterance;" +
        "                        try {" +
        "                            v = float.Parse( right );" +
        "                            target.UstEvent.PreUtterance = v;" +
        "                        } catch {" +
        "                        }" +
        "                    }" +
        "                }" +
        "            }" +
        "        }" +
        "" +
        "        // PitchesをPIT, PBSに反映させる処理" +
        "        VsqBPList cpit = vsq_track.getCurve( \"pit\" );" +
        "        if ( cpit == null ) {" +
        "            cpit = new VsqBPList( CurveType.PIT.getName()," +
        "                                  CurveType.PIT.getDefault()," +
        "                                  CurveType.PIT.getMinimum()," +
        "                                  CurveType.PIT.getMaximum() );" +
        "            vsq_track.setCurve( \"pit\", cpit );" +
        "        }" +
        "        VsqBPList cpbs = vsq_track.getCurve( \"pbs\" );" +
        "        if ( cpbs == null ) {" +
        "            cpbs = new VsqBPList( CurveType.PBS.getName()," +
        "                                  CurveType.PBS.getDefault()," +
        "                                  CurveType.PBS.getMinimum()," +
        "                                  CurveType.PBS.getMaximum() );" +
        "            vsq_track.setCurve( \"pbs\", cpbs );" +
        "        }" +
        "        for ( int i = 0; i < pit_added_ids.Count; i++ ) {" +
        "            int internal_id = pit_added_ids[i];" +
        "            VsqEvent target = vsq_track.findEventFromID( internal_id );" +
        "            if ( target == null ) {" +
        "                continue;" +
        "            }" +
        "" +
        "            // ピッチベンド絶対値の最大値を調べる" +
        "            float abs_pit_max = 0;" +
        "            for ( int j = 0; j < target.UstEvent.Pitches.Length; j++ ) {" +
        "                abs_pit_max = Math.Max( abs_pit_max, Math.Abs( target.UstEvent.Pitches[j] ) );" +
        "            }" +
        "" +
        "            // ピッチベンドを表現するのに最低限必要なPBSを調べる。" +
        "            int pbs = (int)(abs_pit_max / 100.0);" +
        "            if ( pbs * 100 != abs_pit_max ) {" +
        "                // abs_pit_maxが100の倍数で無い場合。" +
        "                pbs++;" +
        "            }" +
        "            if ( pbs < 1 ) {" +
        "                pbs = 1;" +
        "            }" +
        "            if ( CurveType.PBS.getMaximum() < pbs ){" +
        "                pbs = CurveType.PBS.getMaximum();" +
        "            }" +
        "" +
        "            // これからPITをいじる範囲内のPBSが、pbsと違う値になっていた場合の処理" +
        "            double sec_pitstart = vsq.getSecFromClock( target.Clock ) - target.UstEvent.PreUtterance / 1000.0;" +
        "            int pit_start = (int)vsq.getClockFromSec( sec_pitstart );" +
        "            int pbtype = target.UstEvent.PBType;" +
        "            if ( pbtype < 1 ) {" +
        "                pbtype = 5;" +
        "            }" +
        "            target.UstEvent.PBType = pbtype;" +
        "            int pit_end = pit_start + pbtype * target.UstEvent.Pitches.Length;" +
        "            int pbs_at_pitend = cpbs.getValue( pit_end - 1 );" +
        "            for ( int j = 0; j < cpbs.size(); ) {" +
        "                int jclock = cpbs.getKeyClock( j );" +
        "                if ( pit_start <= jclock && jclock < pit_end ) {" +
        "                    int jpbs = cpbs.getElement( j );" +
        "                    if ( jpbs != pbs ) {" +
        "                        cpbs.removeElementAt( j );" +
        "                    } else {" +
        "                        j++;" +
        "                    }" +
        "                } else if ( pit_end < jclock ) {" +
        "                    break;" +
        "                } else {" +
        "                    j++;" +
        "                }" +
        "            }" +
        "            if ( cpbs.getValue( pit_start ) != pbs ) {" +
        "                cpbs.add( pit_start, pbs );" +
        "            }" +
        "            if ( pbs_at_pitend != pbs ) {" +
        "                cpbs.add( pit_end, pbs_at_pitend );" +
        "            }" +
        "" +
        "            // これからPITをいじる範囲にPITが指定されていたら削除する" +
        "            int pit_at_pitend = cpit.getValue( pit_end );" +
        "            for ( int j = 0; j < cpit.size(); ) {" +
        "                int jclock = cpit.getKeyClock( j );" +
        "                if ( pit_start <= jclock && jclock < pit_end ) {" +
        "                    cpit.removeElementAt( j );" +
        "                } else if ( pit_end < jclock ) {" +
        "                    break;" +
        "                } else {" +
        "                    j++;" +
        "                }" +
        "            }" +
        "" +
        "            // PITを追加。" +
        "            int lastpit = CurveType.PIT.getMinimum() - 1;" +
        "            for ( int j = 0; j < target.UstEvent.Pitches.Length; j++ ) {" +
        "                int jclock = pit_start + j * pbtype;" +
        "                int pit = (int)(8192.0 * target.UstEvent.Pitches[j] / 100.0 / (double)pbs);" +
        "                if ( pit < CurveType.PIT.getMinimum() ) {" +
        "                    pit = CurveType.PIT.getMinimum();" +
        "                } else if ( CurveType.PIT.getMaximum() < pit ) {" +
        "                    pit = CurveType.PIT.getMaximum();" +
        "                }" +
        "                if ( pit != lastpit ) {" +
        "                    cpit.add( jclock, pit );" +
        "                    lastpit = pit;" +
        "                }" +
        "            }" +
        "            if ( cpit.getValue( pit_end ) != pit_at_pitend ) {" +
        "                cpit.add( pit_end, pit_at_pitend );" +
        "            }" +
        "        }" +
        "" +
        "        try {" +
        "            System.IO.File.Delete( temp );" +
        "        } catch ( Exception ex ) {" +
        "        }" +
        "        return ScriptReturnStatus.EDITED;" +
        "    }" +
        "" +
        "    private static void copyCurve( VsqBPList src, VsqBPList dest, int clock_shift ) {" +
        "        int last_value = src.getDefault();" +
        "        int count = src.size();" +
        "        bool first_over_zero = true;" +
        "        for ( int i = 0; i < count; i++ ) {" +
        "            int cl = src.getKeyClock( i ) - clock_shift;" +
        "            int value = src.getElementA( i );" +
        "            if ( cl < 0 ) {" +
        "                last_value = value;" +
        "            } else {" +
        "                if ( first_over_zero ) {" +
        "                    first_over_zero = false;" +
        "                    if ( last_value != src.getDefault() ) {" +
        "                        dest.add( 0, last_value );" +
        "                    }" +
        "                }" +
        "                dest.add( cl, value );" +
        "            }" +
        "        }" +
        "    }" +
        "" +
        "    private void {0}_Load( object sender, EventArgs e ) {" +
        "        bgWork.RunWorkerAsync();" +
        "    }" +
        "" +
        "    private void bgWork_DoWork( object sender, System.ComponentModel.DoWorkEventArgs e ) {" +
        "        string dquote = new string( (char)0x22, 1 );" +
        "        using ( System.Diagnostics.Process p = new System.Diagnostics.Process() ) {" +
        "            p.StartInfo.FileName = m_exe_path;" +
        "            p.StartInfo.Arguments = dquote + m_temp + dquote;" +
        "            p.StartInfo.WorkingDirectory = Path.GetDirectoryName( m_exe_path );" +
        "            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;" +
        "            p.Start();" +
        "            p.WaitForExit();" +
        "        }" +
        "    }" +
        "" +
        "    private void bgWork_RunWorkerCompleted( object sender, System.ComponentModel.RunWorkerCompletedEventArgs e ) {" +
        "        this.Close();" +
        "    }" +
        "" +
        "    public static String GetDisplayName() {" +
        "        return s_display_name;" +
        "    }" +
        "}" +
        "";
    private ListView listPlugins;
    private ColumnHeader headerName;
    private ColumnHeader headerPath;
    private OpenFileDialog openFileDialog;

    /// <summary>
    /// plugin.txtのパスのリスト
    /// </summary>
    public static List<string> Plugins = new List<string>();
    public static int ColumnWidthName = 100;
    public static int ColumnWidthPath = 200;
    public static int DialogWidth = 295;
    public static int DialogHeight = 352;
    private static List<string> oldPlugins = null;

    public UtauPluginManager() {
        InitializeComponent();
        if ( ColumnWidthName > 0 ) {
            headerName.Width = ColumnWidthName;
        }
        if ( ColumnWidthPath > 0 ) {
            headerPath.Width = ColumnWidthPath;
        }
        if ( DialogWidth > 0 ) {
            Width = DialogWidth;
        }
        if ( DialogHeight > 0 ) {
            Height = DialogHeight;
        }

        btnAdd.Text = _( "Add" );
        btnRemove.Text = _( "Remove" );
        btnOk.Text = _( "OK" );
        btnCancel.Text = _( "Cancel" );
        headerName.Text = _( "Name" );
        headerPath.Text = _( "Path" );

        oldPlugins = new List<string>();
        Encoding sjis = Encoding.GetEncoding( "Shift_JIS" );
        foreach ( string s in getPlugins() ) {
            if ( !System.IO.File.Exists( s ) ) {
                continue;
            }
            string name = getPluginName( s );
            if ( name != "" ) {
                listPlugins.Items.Add( new ListViewItem( new string[] { name, s } ) );
            }
            oldPlugins.Add( s );
        }
    }

    private static List<string> getPlugins() {
        if ( Plugins == null ) {
            Plugins = new List<string>();
        }
        return Plugins;
    }

    public static ScriptReturnStatus Edit( VsqFileEx vsq ) {
        UtauPluginManager dialog = new UtauPluginManager();
        dialog.ShowDialog();
        return ScriptReturnStatus.NOT_EDITED;
    }

    private void InitializeComponent() {
        this.btnOk = new System.Windows.Forms.Button();
        this.btnAdd = new System.Windows.Forms.Button();
        this.btnRemove = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.listPlugins = new System.Windows.Forms.ListView();
        this.headerName = new System.Windows.Forms.ColumnHeader();
        this.headerPath = new System.Windows.Forms.ColumnHeader();
        this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
        this.SuspendLayout();
        // 
        // btnOk
        // 
        this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnOk.Location = new System.Drawing.Point( 119, 283 );
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size( 75, 23 );
        this.btnOk.TabIndex = 7;
        this.btnOk.Text = "OK";
        this.btnOk.UseVisualStyleBackColor = true;
        this.btnOk.Click += new System.EventHandler( this.btnOk_Click );
        // 
        // btnAdd
        // 
        this.btnAdd.Location = new System.Drawing.Point( 12, 12 );
        this.btnAdd.Name = "btnAdd";
        this.btnAdd.Size = new System.Drawing.Size( 75, 23 );
        this.btnAdd.TabIndex = 9;
        this.btnAdd.Text = "Add";
        this.btnAdd.UseVisualStyleBackColor = true;
        this.btnAdd.Click += new System.EventHandler( this.btnAdd_Click );
        // 
        // btnRemove
        // 
        this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnRemove.Location = new System.Drawing.Point( 200, 12 );
        this.btnRemove.Name = "btnRemove";
        this.btnRemove.Size = new System.Drawing.Size( 75, 23 );
        this.btnRemove.TabIndex = 10;
        this.btnRemove.Text = "Remove";
        this.btnRemove.UseVisualStyleBackColor = true;
        this.btnRemove.Click += new System.EventHandler( this.btnRemove_Click );
        // 
        // btnCancel
        // 
        this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnCancel.Location = new System.Drawing.Point( 200, 283 );
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
        this.btnCancel.TabIndex = 11;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        // 
        // listPlugins
        // 
        this.listPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.listPlugins.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.headerName,
            this.headerPath} );
        this.listPlugins.FullRowSelect = true;
        this.listPlugins.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
        this.listPlugins.Location = new System.Drawing.Point( 12, 41 );
        this.listPlugins.Name = "listPlugins";
        this.listPlugins.Size = new System.Drawing.Size( 263, 236 );
        this.listPlugins.TabIndex = 12;
        this.listPlugins.UseCompatibleStateImageBehavior = false;
        this.listPlugins.View = System.Windows.Forms.View.Details;
        // 
        // headerName
        // 
        this.headerName.Text = "Name";
        // 
        // headerPath
        // 
        this.headerPath.Text = "Path";
        // 
        // openFileDialog
        // 
        this.openFileDialog.FileName = "plugin.txt";
        // 
        // UtauPluginManager
        // 
        this.AcceptButton = this.btnOk;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size( 287, 318 );
        this.Controls.Add( this.listPlugins );
        this.Controls.Add( this.btnCancel );
        this.Controls.Add( this.btnRemove );
        this.Controls.Add( this.btnAdd );
        this.Controls.Add( this.btnOk );
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "UtauPluginManager";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "UTAU Plugin Manager";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.UtauPluginManager_FormClosing );
        this.ResumeLayout( false );

    }

    private void btnAdd_Click( object sender, EventArgs e ) {
        if ( openFileDialog.ShowDialog() != DialogResult.OK ) {
            return;
        }

        string file = openFileDialog.FileName;
        string name = getPluginName( file );
        if ( name == "" ) {
            return;
        }

        foreach ( ListViewItem litem in listPlugins.Items ) {
            if ( litem.SubItems.Count < 2 ) {
                continue;
            }
            string tname = litem.SubItems[0].Text;
            string tfile = litem.SubItems[1].Text;

            if ( tname == name ) {
                org.kbinani.windows.forms.BDialogResult dr =
                    AppManager.showMessageBox( string.Format( _( "Script named '{0}' is already registered. Overwrite?" ), name ),
                                               "UTAU Plugin Manager",
                                               PortUtil.MSGBOX_OK_CANCEL_OPTION,
                                               PortUtil.MSGBOX_QUESTION_MESSAGE );
                if ( dr != org.kbinani.windows.forms.BDialogResult.YES ) {
                    return;
                }

                listPlugins.Items.Remove( litem );
                foreach ( string f in getPlugins() ) {
                    if ( f == tfile ) {
                        getPlugins().Remove( f );
                        break;
                    }
                }
            }
        }

        listPlugins.Items.Add( new ListViewItem( new string[] { name, file } ) );
        getPlugins().Add( file );
    }

    private static string getPluginName( string plugin_txt_file ) {
        if ( !PortUtil.isFileExists( plugin_txt_file ) ) {
            return "";
        }

        string name = "";
        using ( StreamReader sr = new StreamReader( plugin_txt_file, Encoding.GetEncoding( "Shift_JIS" ) ) ) {
            string line = "";
            while ( (line = sr.ReadLine()) != null ) {
                if ( line.StartsWith( "name=" ) ) {
                    name = line.Substring( 5 ).Trim();
                    break;
                }
            }
        }
        return name;
    }

    private static string _( string id ) {
        string lang = Messaging.getLanguage();
        if ( lang != "en" ) {
            if ( id == "Script named '{0}' is already registered. Overwrite?" ) {
                if ( lang == "ja" ) {
                    return "'{0}' という名前のスクリプトは既に登録されています。上書きしますか？";
                }
            } else if ( id == "Remove '{0}'?" ) {
                if ( lang == "ja" ) {
                    return "'{0}' を削除しますか？";
                }
            } else if ( id == "Add" ) {
                if ( lang == "ja" ) {
                    return "追加";
                }
            } else if ( id == "Remove" ) {
                if ( lang == "ja" ) {
                    return "削除";
                }
            } else if ( id == "OK" ) {
                if ( lang == "ja" ) {
                    return "了解";
                }
            } else if ( id == "Cancel" ) {
                if ( lang == "ja" ) {
                    return "取消";
                }
            } else if ( id == "Name" ) {
                if ( lang == "ja" ) {
                    return "名称";
                }
            } else if ( id == "Path" ) {
                if ( lang == "ja" ) {
                    return "保存場所";
                }
            }
        }
        return id;
    }

    private void UtauPluginManager_FormClosing( object sender, FormClosingEventArgs e ) {
        ColumnWidthName = headerName.Width;
        ColumnWidthPath = headerPath.Width;
        if ( WindowState == FormWindowState.Normal ) {
            DialogWidth = Width;
            DialogHeight = Height;
        }
    }

    private void btnRemove_Click( object sender, EventArgs e ) {
        int count = listPlugins.SelectedIndices.Count;
        if ( count <= 0 ) {
            return;
        }

        int indx = listPlugins.SelectedIndices[0];
        ListViewItem litem = listPlugins.Items[indx];
        if ( litem.SubItems.Count < 2 ){
            return;
        }
        string name = litem.SubItems[0].Text;
        string path = litem.SubItems[1].Text;

        org.kbinani.windows.forms.BDialogResult dr =
            AppManager.showMessageBox( string.Format( _( "Remove '{0}'?" ), name ),
                                       "UTAU Plugin Manager",
                                       PortUtil.MSGBOX_YES_NO_OPTION,
                                       PortUtil.MSGBOX_QUESTION_MESSAGE );
        if ( dr != org.kbinani.windows.forms.BDialogResult.YES ) {
            return;
        }

        listPlugins.Items.Remove( litem );
        if ( getPlugins().Contains( path ) ) {
            getPlugins().Remove( path );
        }
    }

    private void btnOk_Click( object sender, EventArgs e ) {
        if ( oldPlugins != null ) {
            // 削除されたスクリプトをアンインストールする
            foreach ( string file in oldPlugins ) {
                if ( !getPlugins().Contains( file ) ) {
                    string name = getPluginName( file );
                    if ( name != "" ) {
                        string script_path = PortUtil.combinePath( AppManager.getScriptPath(), name + ".txt" );
                        if ( PortUtil.isFileExists( script_path ) ) {
                            PortUtil.deleteFile( script_path );
                        }
                    }
                }
            }
        }

        foreach ( string file in getPlugins() ) {
            if ( !PortUtil.isFileExists( file ) ) {
                continue;
            }

            string name = getPluginName( file );
            if ( name == "" ) {
                continue;
            }
            char[] invalid_classname = new char[] { ' ', '!', '#', '$', '%', '&', '\'', '(', ')', '=', '-', '~', '^', '`', '@', '{', '}', '[', ']', '+', '*', ';', '.' };
            foreach ( char c in invalid_classname ) {
                name = name.Replace( c, '_' );
            }
            string text = TEXT;
            string code = text.Replace( "{0}", name );
            code = code.Replace( "{1}", file );
            using ( StreamWriter sw = new StreamWriter( PortUtil.combinePath( AppManager.getScriptPath(), name + ".txt" ) ) ) {
                sw.WriteLine( code );
            }
        }

        if ( AppManager.mainWindow != null ) {
            VoidDelegate deleg = new VoidDelegate( AppManager.mainWindow.updateScriptShortcut );
            if ( deleg != null ) {
                AppManager.mainWindow.Invoke( deleg );
            }
        }
    }

    public static String GetDisplayName() {
        String lang = Messaging.getLanguage();
        if ( lang == "ja" ) {
            return "UTAU用プラグインをインストール";
        } else {
            return "Install UTAU Plugin";
        }
    }
}
