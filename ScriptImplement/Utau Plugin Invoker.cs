using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using org.kbinani.cadencii;
using org.kbinani.java.util;
using org.kbinani.vsq;

public class Utau_Plugin_Invoker {
    private static bool s_finished = false;
    private static string s_plugin_txt_path = @"E:\Program Files\UTAU\plugins\Lyric Diphonizer\plugin.txt";
    private static readonly string s_class_name = "Utau_Plugin_Invoker";

    public static ScriptReturnStatus Edit( VsqFileEx vsq ) {
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
        VsqTrack vsq_track = vsq.Track.get( selected );
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
        VsqEvent singer_event = vsq.Track.get( 1 ).getSingerEventAt( clock_begin );
        string voice_dir = "";
        SingerConfig sc = AppManager.getSingerInfoUtau( singer_event.ID.IconHandle.Program );
        if ( sc != null ) {
            voice_dir = sc.VOICEIDSTR;
        }
        tust.setVoiceDir( voice_dir );

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
        System.Threading.Thread thread = new System.Threading.Thread( new System.Threading.ParameterizedThreadStart( runPlugin ) );
        s_finished = false;
        thread.Start( arg );
        while ( !s_finished ) {
            System.Threading.Thread.Sleep( 100 );
            Application.DoEvents();
        }

        // 結果を反映 -----------------------------------------------------------------------
        using ( StreamReader sr = new StreamReader( temp, Encoding.GetEncoding( 932 ) ) ) {
            string line = "";
            string current_parse = "";
            int clock = clock_begin;
            int tlength = 0;
            while ( (line = sr.ReadLine()) != null ) {
                Console.WriteLine( s_class_name + "#Edit; line=" + line );
                if ( line.StartsWith( "[#" ) ){
                    current_parse = line;
                    clock += tlength;
                    continue;
                }

                if ( current_parse == "[#SETTING]" ) {
                } else if ( current_parse == "[#TRACKEND]" ) {
                } else if ( current_parse == "[#PREV]" ) {
                } else if ( current_parse == "[#NEXT]" ) {
                } else if ( current_parse.StartsWith( "[#" ) ) {
                    int indx_blacket = current_parse.IndexOf( ']' );
                    string str_num = current_parse.Substring( 2, indx_blacket - 2 );
                    Console.WriteLine( s_class_name + "#Edit; str_num=" + str_num );
                    int num = -1;
                    if ( !int.TryParse( str_num, out num ) ) {
                        Console.WriteLine( s_class_name + "#Edit; format error; str_num=" + str_num );
                        continue;
                    }
                    if ( num < 0 || map_id.Count <= num ) {
                        Console.WriteLine( s_class_name + "#Edit; invalid range; num=" + num + "; map_id.Count=" + map_id.Count );
                        continue;
                    }
                    VsqEvent target = vsq_track.findEventFromID( map_id[num] );
                    if ( target == null ) {
                        Console.WriteLine( s_class_name + "#Edit; target event not found; num=" + num + "; map_id[num]=" + map_id[num] );
                        continue;
                    }
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
                            target.ID.Dynamics = v;
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
                        // pitとpbsの処理を何とかする．
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
        try {
            //System.IO.File.Delete( temp );
            //TODO:後でコメントをはずすこと
            Console.WriteLine( s_class_name + "#Edit; temp=" + temp );
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

    private static void runPlugin( object arg ) {
        StartPluginArgs a = (StartPluginArgs)arg;
        string exe_path = a.exePath;
        string temp = a.tmpFile;
        string dquote = new string( (char)0x22, 1 );
        Console.WriteLine( s_class_name + "#runPlugin; dquote=" + dquote );
        using ( System.Diagnostics.Process p = new System.Diagnostics.Process() ) {
            p.StartInfo.FileName = exe_path;
            p.StartInfo.Arguments = dquote + temp + dquote;
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
