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
    private static readonly String TEXT = "" +
        "using System;\n" +
        "using System.Collections.Generic;\n" +
        "using System.IO;\n" +
        "using System.Text;\n" +
        "using System.Windows.Forms;\n" +
        "using org.kbinani.cadencii;\n" +
        "using org.kbinani.java.util;\n" +
        "using org.kbinani.vsq;\n" +
        "\n" +
        "public class {0} {\n" +
        "    private static bool s_finished = false;\n" +
        "    private static string s_plugin_txt_path = @\"{1}\";\n" +
        "    private static readonly string s_class_name = \"{0}\";\n" +
        "\n" +
        "    public static ScriptReturnStatus Edit( VsqFileEx vsq ) {\n" +
        "        if ( AppManager.getSelectedEventCount() <= 0 ) {\n" +
        "            return ScriptReturnStatus.NOT_EDITED;\n" +
        "        }\n" +
        "\n" +
        "        int selected = AppManager.getSelected();\n" +
        "        \n" +
        "        string pluginTxtPath = s_plugin_txt_path;\n" +
        "        if ( pluginTxtPath == \"\" ) {\n" +
        "            AppManager.showMessageBox( \"pluginTxtPath=\" + pluginTxtPath );\n" +
        "            return ScriptReturnStatus.ERROR;\n" +
        "        }\n" +
        "        if ( !System.IO.File.Exists( pluginTxtPath ) ) {\n" +
        "            AppManager.showMessageBox( \"'\" + pluginTxtPath + \"' does not exists\" );\n" +
        "            return ScriptReturnStatus.ERROR;\n" +
        "        }\n" +
        "\n" +
        "        System.Text.Encoding shift_jis = System.Text.Encoding.GetEncoding( \"Shift_JIS\" );\n" +
        "        string name = \"\";\n" +
        "        string exe_path = \"\";\n" +
        "        using ( StreamReader sr = new StreamReader( pluginTxtPath, shift_jis ) ) {\n" +
        "            string line = \"\";\n" +
        "            while ( (line = sr.ReadLine()) != null ) {\n" +
        "                string[] spl = line.Split( new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries );\n" +
        "                if ( line.StartsWith( \"name=\" ) ) {\n" +
        "                    name = spl[1];\n" +
        "                } else if ( line.StartsWith( \"execute=\" ) ) {\n" +
        "                    exe_path = Path.Combine( Path.GetDirectoryName( pluginTxtPath ), spl[1] );\n" +
        "                }\n" +
        "            }\n" +
        "        }\n" +
        "        if ( exe_path == \"\" ) {\n" +
        "            return ScriptReturnStatus.ERROR;\n" +
        "        }\n" +
        "        if ( !System.IO.File.Exists( exe_path ) ) {\n" +
        "            AppManager.showMessageBox( \"'\" + exe_path + \"' does not exists\" );\n" +
        "            return ScriptReturnStatus.ERROR;\n" +
        "        }\n" +
        "\n" +
        "        // ustを用意 ------------------------------------------------------------------------\n" +
        "        // 方針は，一度VsqFileに音符を格納->UstFile#.ctor( VsqFile )を使って一括変換\n" +
        "        // メイン画面で選択されているアイテムを列挙\n" +
        "        List<VsqEvent> items = new List<VsqEvent>(); // Ustに追加する音符のリスト\n" +
        "        List<int> map_id = new List<int>(); // ustの[#index]が、map_id[index].InternalIDというIDを持つVsqEventに相当することを記録しておくリスト\n" +
        "        int num_selected = 0; // 選択されていた音符の個数\n" +
        "        for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {\n" +
        "            SelectedEventEntry item_itr = (SelectedEventEntry)itr.next();\n" +
        "            if ( item_itr.original.ID.type == VsqIDType.Anote ) {\n" +
        "                items.Add( (VsqEvent)item_itr.original.clone() );\n" +
        "                num_selected++;\n" +
        "            }\n" +
        "        }\n" +
        "        items.Sort();\n" +
        "\n" +
        "        // R用音符のテンプレート\n" +
        "        VsqEvent template = new VsqEvent();\n" +
        "        template.ID.type = VsqIDType.Anote;\n" +
        "        template.ID.LyricHandle = new LyricHandle( \"R\", \"\" );\n" +
        "        template.InternalID = -1; // VSQ上には実際に存在しないダミーであることを表す\n" +
        "\n" +
        "        int count = items.Count;\n" +
        "        // アイテムが2個以上の場合は、間にRを挿入する必要があるかどうか判定\n" +
        "        if ( count > 1 ) {\n" +
        "            List<VsqEvent> add = new List<VsqEvent>(); // 追加するR\n" +
        "            VsqEvent last_event = items[0];\n" +
        "            for ( int i = 1; i < count; i++ ) {\n" +
        "                VsqEvent item = items[i];\n" +
        "                if ( last_event.Clock + last_event.ID.Length < item.Clock ) {\n" +
        "                    VsqEvent add_temp = (VsqEvent)template.clone();\n" +
        "                    add_temp.Clock = last_event.Clock + last_event.ID.Length;\n" +
        "                    add_temp.ID.Length = item.Clock - add_temp.Clock;\n" +
        "                    add.Add( add_temp );\n" +
        "                }\n" +
        "                last_event = item;\n" +
        "            }\n" +
        "            foreach ( VsqEvent v in add ) {\n" +
        "                items.Add( v );\n" +
        "            }\n" +
        "            items.Sort();\n" +
        "        }\n" +
        "\n" +
        "        // ヘッダ\n" +
        "        int TEMPO = 120;\n" +
        "\n" +
        "        // 選択アイテムの直前直後に未選択アイテムがあるかどうかを判定\n" +
        "        int clock_begin = items[0].Clock;\n" +
        "        int clock_end = items[items.Count - 1].Clock;\n" +
        "        int clock_end_end = clock_end + items[items.Count - 1].ID.Length;\n" +
        "        VsqTrack vsq_track = vsq.Track.get( selected );\n" +
        "        VsqEvent tlast_event = null;\n" +
        "        VsqEvent prev = null;\n" +
        "        VsqEvent next = null;\n" +
        "\n" +
        "        // VsqFile -> UstFileのコンバート\n" +
        "        VsqFile conv = new VsqFile( \"Miku\", 2, 4, 4, (int)(6e7 / TEMPO) );\n" +
        "        VsqTrack conv_track = conv.Track.get( 1 );\n" +
        "        for ( Iterator itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {\n" +
        "            VsqEvent item_itr = (VsqEvent)itr.next();\n" +
        "            if ( item_itr.Clock == clock_begin ) {\n" +
        "                if ( tlast_event != null ) {\n" +
        "                    // アイテムあり\n" +
        "                    if ( tlast_event.Clock + tlast_event.ID.Length == clock_begin ) {\n" +
        "                        // ゲートタイム差0で接続\n" +
        "                        prev = (VsqEvent)tlast_event.clone();\n" +
        "                    } else {\n" +
        "                        // 時間差アリで接続\n" +
        "                        prev = (VsqEvent)template.clone();\n" +
        "                        prev.Clock = tlast_event.Clock + tlast_event.ID.Length;\n" +
        "                        prev.ID.Length = clock_begin - (tlast_event.Clock + tlast_event.ID.Length);\n" +
        "                    }\n" +
        "                }\n" +
        "            }\n" +
        "            tlast_event = item_itr;\n" +
        "            if ( item_itr.Clock == clock_end ) {\n" +
        "                if ( itr.hasNext() ) {\n" +
        "                    VsqEvent foo = (VsqEvent)itr.next();\n" +
        "                    if ( foo.Clock == clock_end_end ) {\n" +
        "                        // ゲートタイム差0で接続\n" +
        "                        next = (VsqEvent)foo.clone();\n" +
        "                    } else {\n" +
        "                        // 時間差アリで接続\n" +
        "                        next = (VsqEvent)template.clone();\n" +
        "                        next.Clock = clock_end_end;\n" +
        "                        next.ID.Length = foo.Clock - clock_end_end;\n" +
        "                    }\n" +
        "                }\n" +
        "                break;\n" +
        "            }\n" +
        "\n" +
        "        }\n" +
        "\n" +
        "        // [#PREV]を追加\n" +
        "        if ( prev != null ) {\n" +
        "            prev.Clock -= clock_begin;\n" +
        "            conv_track.addEvent( prev );\n" +
        "        }\n" +
        "\n" +
        "        // ゲートタイムを計算しながら追加\n" +
        "        count = items.Count;\n" +
        "        for ( int i = 0; i < count; i++ ) {\n" +
        "            VsqEvent itemi = items[i];\n" +
        "            itemi.Clock -= clock_begin;\n" +
        "            conv_track.addEvent( itemi );\n" +
        "            map_id.Add( itemi.InternalID );\n" +
        "        }\n" +
        "\n" +
        "        // [#NEXT]を追加\n" +
        "        if ( next != null ) {\n" +
        "            next.Clock -= clock_begin;\n" +
        "            conv_track.addEvent( next );\n" +
        "        }\n" +
        "\n" +
        "        // PIT, PBSを追加\n" +
        "        copyCurve( vsq_track.getCurve( \"pit\" ), conv_track.getCurve( \"pit\" ), clock_begin );\n" +
        "        copyCurve( vsq_track.getCurve( \"pbs\" ), conv_track.getCurve( \"pbs\" ), clock_begin );\n" +
        "\n" +
        "        string temp = Path.GetTempFileName();\n" +
        "        UstFile tust = new UstFile( conv, 1 );\n" +
        "        VsqEvent singer_event = vsq.Track.get( 1 ).getSingerEventAt( clock_begin );\n" +
        "        string voice_dir = \"\";\n" +
        "        SingerConfig sc = AppManager.getSingerInfoUtau( singer_event.ID.IconHandle.Program );\n" +
        "        if ( sc != null ) {\n" +
        "            voice_dir = sc.VOICEIDSTR;\n" +
        "        }\n" +
        "        tust.setVoiceDir( voice_dir );\n" +
        "\n" +
        "        if ( prev != null ) {\n" +
        "            tust.getTrack( 0 ).getEvent( 0 ).Index = int.MinValue;\n" +
        "        }\n" +
        "        if ( next != null ) {\n" +
        "            tust.getTrack( 0 ).getEvent( tust.getTrack( 0 ).getEventCount() - 1 ).Index = int.MaxValue;\n" +
        "        }\n" +
        "        tust.write( temp, false );\n" +
        "        //#if DEBUG\n" +
        "        using ( StreamReader sr = new StreamReader( temp, Encoding.GetEncoding( \"Shift_JIS\" ) ) ) {\n" +
        "            Console.WriteLine( s_class_name + \"#Edit; before: text=\" + sr.ReadToEnd() );\n" +
        "        }\n" +
        "        //#endif\n" +
        "\n" +
        "        // 起動 -----------------------------------------------------------------------------\n" +
        "        StartPluginArgs arg = new StartPluginArgs();\n" +
        "        arg.exePath = exe_path;\n" +
        "        arg.tmpFile = temp;\n" +
        "        System.Threading.Thread thread = new System.Threading.Thread( new System.Threading.ParameterizedThreadStart( runPlugin ) );\n" +
        "        s_finished = false;\n" +
        "        thread.Start( arg );\n" +
        "        while ( !s_finished ) {\n" +
        "            System.Threading.Thread.Sleep( 100 );\n" +
        "            Application.DoEvents();\n" +
        "        }\n" +
        "\n" +
        "        //#if DEBUG\n" +
        "        using ( StreamReader sr = new StreamReader( temp, Encoding.GetEncoding( \"Shift_JIS\" ) ) ) {\n" +
        "            Console.WriteLine( s_class_name + \"#Edit; after : text=\" + sr.ReadToEnd() );\n" +
        "        }\n" +
        "        //#endif\n" +
        "\n" +
        "        // 結果を反映 -----------------------------------------------------------------------\n" +
        "        using ( StreamReader sr = new StreamReader( temp, Encoding.GetEncoding( 932 ) ) ) {\n" +
        "            string line = \"\";\n" +
        "            string current_parse = \"\";\n" +
        "            int clock = clock_begin;\n" +
        "            int tlength = 0;\n" +
        "            while ( (line = sr.ReadLine()) != null ) {\n" +
        "                Console.WriteLine( s_class_name + \"#Edit; line=\" + line );\n" +
        "                if ( line.StartsWith( \"[#\" ) ){\n" +
        "                    current_parse = line;\n" +
        "                    clock += tlength;\n" +
        "                    continue;\n" +
        "                }\n" +
        "\n" +
        "                if ( current_parse == \"[#SETTING]\" ) {\n" +
        "                } else if ( current_parse == \"[#TRACKEND]\" ) {\n" +
        "                } else if ( current_parse == \"[#PREV]\" ) {\n" +
        "                } else if ( current_parse == \"[#NEXT]\" ) {\n" +
        "                } else if ( current_parse.StartsWith( \"[#\" ) ) {\n" +
        "                    int indx_blacket = current_parse.IndexOf( ']' );\n" +
        "                    string str_num = current_parse.Substring( 2, indx_blacket - 2 );\n" +
        "                    Console.WriteLine( s_class_name + \"#Edit; str_num=\" + str_num );\n" +
        "                    int num = -1;\n" +
        "                    if ( !int.TryParse( str_num, out num ) ) {\n" +
        "                        Console.WriteLine( s_class_name + \"#Edit; format error; str_num=\" + str_num );\n" +
        "                        continue;\n" +
        "                    }\n" +
        "                    if ( num < 0 || map_id.Count <= num ) {\n" +
        "                        Console.WriteLine( s_class_name + \"#Edit; invalid range; num=\" + num + \"; map_id.Count=\" + map_id.Count );\n" +
        "                        continue;\n" +
        "                    }\n" +
        "                    VsqEvent target = vsq_track.findEventFromID( map_id[num] );\n" +
        "                    if ( target == null ) {\n" +
        "                        Console.WriteLine( s_class_name + \"#Edit; target event not found; num=\" + num + \"; map_id[num]=\" + map_id[num] );\n" +
        "                        continue;\n" +
        "                    }\n" +
        "                    if ( target.UstEvent == null ) {\n" +
        "                        target.UstEvent = new UstEvent();\n" +
        "                    }\n" +
        "                    string[] spl = line.Split( '=' );\n" +
        "                    if ( spl.Length < 2 ) {\n" +
        "                        continue;\n" +
        "                    }\n" +
        "                    string left = spl[0];\n" +
        "                    string right = spl[1];\n" +
        "                    if ( left == \"Length\" ) {\n" +
        "                        int v = target.ID.getLength();\n" +
        "                        try {\n" +
        "                            v = int.Parse( right );\n" +
        "                            target.ID.setLength( v );\n" +
        "                            target.UstEvent.Length = v;\n" +
        "                            tlength = v;\n" +
        "                        } catch {\n" +
        "                        }\n" +
        "                    } else if ( left == \"Lyric\" ) {\n" +
        "                        if ( target.ID.LyricHandle == null ) {\n" +
        "                            target.ID.LyricHandle = new LyricHandle( \"あ\", \"a\" );\n" +
        "                        }\n" +
        "                        target.ID.LyricHandle.L0.Phrase = right;\n" +
        "                        target.UstEvent.Lyric = right;\n" +
        "                        AppManager.changePhrase( vsq, selected, target, clock, right );\n" +
        "                    } else if ( left == \"NoteNum\" ) {\n" +
        "                        int v = target.ID.Note;\n" +
        "                        try {\n" +
        "                            v = int.Parse( right );\n" +
        "                            target.ID.Note = v;\n" +
        "                            target.UstEvent.Note = v;\n" +
        "                        } catch {\n" +
        "                        }\n" +
        "                    } else if ( left == \"Intensity\" ) {\n" +
        "                        int v = target.ID.Dynamics;\n" +
        "                        try {\n" +
        "                            v = int.Parse( right );\n" +
        "                            target.ID.Dynamics = v;\n" +
        "                            target.UstEvent.Intensity = v;\n" +
        "                        } catch {\n" +
        "                        }\n" +
        "                    } else if ( left == \"PBType\" ) {\n" +
        "                        int v = target.UstEvent.PBType;\n" +
        "                        try {\n" +
        "                            v = int.Parse( right );\n" +
        "                            target.UstEvent.PBType = v;\n" +
        "                        } catch {\n" +
        "                        }\n" +
        "                    } else if ( left == \"Piches\" ) {\n" +
        "                        string[] spl2 = right.Split( ',' );\n" +
        "                        float[] t = new float[spl2.Length];\n" +
        "                        for ( int i = 0; i < spl2.Length; i++ ) {\n" +
        "                            float v = 0;\n" +
        "                            try {\n" +
        "                                v = float.Parse( spl2[i] );\n" +
        "                                t[i] = v;\n" +
        "                            } catch {\n" +
        "                            }\n" +
        "                        }\n" +
        "                        target.UstEvent.Pitches = t;\n" +
        "                        // pitとpbsの処理を何とかする．\n" +
        "                    } else if ( left == \"Tempo\" ) {\n" +
        "                        float v = 125f;\n" +
        "                        try {\n" +
        "                            v = float.Parse( right );\n" +
        "                            vsq.TempoTable.add( new TempoTableEntry( target.Clock, (int)(60e6 / v), 0.0 ) );\n" +
        "                            vsq.updateTempoInfo();\n" +
        "                        } catch {\n" +
        "                        }\n" +
        "                    } else if ( left == \"VBR\" ) {\n" +
        "                        target.UstEvent.Vibrato = new UstVibrato( line );\n" +
        "                    } else if ( left == \"PBW\" ||\n" +
        "                                left == \"PBS\" ||\n" +
        "                                left == \"PBY\" ||\n" +
        "                                left == \"PBM\" ) {\n" +
        "                        if ( target.UstEvent.Portamento == null ) {\n" +
        "                            target.UstEvent.Portamento = new UstPortamento();\n" +
        "                        }\n" +
        "                        target.UstEvent.Portamento.ParseLine( line );\n" +
        "                    } else if ( left == \"Envelope\" ) {\n" +
        "                        target.UstEvent.Envelope = new UstEnvelope( line );\n" +
        "                    } else if ( left == \"VoiceOverlap\" ) {\n" +
        "                        float v = target.UstEvent.VoiceOverlap;\n" +
        "                        try {\n" +
        "                            v = float.Parse( right );\n" +
        "                            target.UstEvent.VoiceOverlap = v;\n" +
        "                        } catch {\n" +
        "                        }\n" +
        "                    } else if ( left == \"PreUtterance\" ) {\n" +
        "                        float v = target.UstEvent.PreUtterance;\n" +
        "                        try {\n" +
        "                            v = float.Parse( right );\n" +
        "                            target.UstEvent.PreUtterance = v;\n" +
        "                        } catch {\n" +
        "                        }\n" +
        "                    }\n" +
        "                }\n" +
        "            }\n" +
        "        }\n" +
        "        try {\n" +
        "            //System.IO.File.Delete( temp );\n" +
        "            //TODO:後でコメントをはずすこと\n" +
        "            Console.WriteLine( s_class_name + \"#Edit; temp=\" + temp );\n" +
        "        } catch ( Exception ex ) {\n" +
        "        }\n" +
        "        return ScriptReturnStatus.EDITED;\n" +
        "    }\n" +
        "\n" +
        "    private static void copyCurve( VsqBPList src, VsqBPList dest, int clock_shift ) {\n" +
        "        int last_value = src.getDefault();\n" +
        "        int count = src.size();\n" +
        "        bool first_over_zero = true;\n" +
        "        for ( int i = 0; i < count; i++ ) {\n" +
        "            int cl = src.getKeyClock( i ) - clock_shift;\n" +
        "            int value = src.getElementA( i );\n" +
        "            if ( cl < 0 ) {\n" +
        "                last_value = value;\n" +
        "            } else {\n" +
        "                if ( first_over_zero ) {\n" +
        "                    first_over_zero = false;\n" +
        "                    if ( last_value != src.getDefault() ) {\n" +
        "                        dest.add( 0, last_value );\n" +
        "                    }\n" +
        "                }\n" +
        "                dest.add( cl, value );\n" +
        "            }\n" +
        "        }\n" +
        "    }\n" +
        "\n" +
        "    private static void runPlugin( object arg ) {\n" +
        "        StartPluginArgs a = (StartPluginArgs)arg;\n" +
        "        string exe_path = a.exePath;\n" +
        "        string temp = a.tmpFile;\n" +
        "        string dquote = new string( (char)0x22, 1 );\n" +
        "        Console.WriteLine( s_class_name + \"#runPlugin; dquote=\" + dquote );\n" +
        "        using ( System.Diagnostics.Process p = new System.Diagnostics.Process() ) {\n" +
        "            p.StartInfo.FileName = exe_path;\n" +
        "            p.StartInfo.Arguments = dquote + temp + dquote;\n" +
        "            p.StartInfo.WorkingDirectory = Path.GetDirectoryName( exe_path );\n" +
        "            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;\n" +
        "            p.Start();\n" +
        "            p.WaitForExit();\n" +
        "        }\n" +
        "        s_finished = true;\n" +
        "    }\n" +
        "\n" +
        "    private class StartPluginArgs {\n" +
        "        public string exePath = \"\";\n" +
        "        public string tmpFile = \"\";\n" +
        "    }\n" +
        "}\n" +
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
}
