using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using cadencii;
using cadencii.java.util;
using cadencii.vsq;
using cadencii;



public class Utau_Plugin_Invoker : Form
{
    class StartPluginArgs
    {
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

    private Utau_Plugin_Invoker(string exe_path, string temp_file)
    {
        InitializeComponent();
        m_exe_path = exe_path;
        m_temp = temp_file;
    }

    private void InitializeComponent()
    {
        this.lblMessage = new System.Windows.Forms.Label();
        this.bgWork = new System.ComponentModel.BackgroundWorker();
        this.SuspendLayout();
        // 
        // lblMessage
        // 
        this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.lblMessage.Location = new System.Drawing.Point(12, 21);
        this.lblMessage.Name = "lblMessage";
        this.lblMessage.Size = new System.Drawing.Size(289, 23);
        this.lblMessage.TabIndex = 0;
        this.lblMessage.Text = "waiting plugin process...";
        this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // bgWork
        // 
        this.bgWork.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWork_DoWork);
        this.bgWork.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWork_RunWorkerCompleted);
        // 
        // Utau_Plugin_Invoker
        // 
        this.ClientSize = new System.Drawing.Size(313, 119);
        this.Controls.Add(this.lblMessage);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "Utau_Plugin_Invoker";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Utau Plugin Invoker";
        this.Load += new System.EventHandler(this.Utau_Plugin_Invoker_Load);
        this.ResumeLayout(false);

    }

    public static ScriptReturnStatus Edit(VsqFileEx vsq)
    {
        // 選択状態のアイテムがなければ戻る
        if (AppManager.itemSelection.getEventCount() <= 0) {
            return ScriptReturnStatus.NOT_EDITED;
        }

        // 現在のトラック
        int selected = AppManager.getSelected();
        VsqTrack vsq_track = vsq.Track[selected];
        vsq_track.sortEvent();

        // プラグイン情報の定義ファイル(plugin.txt)があるかどうかチェック
        string pluginTxtPath = s_plugin_txt_path;
        if (pluginTxtPath == "") {
            AppManager.showMessageBox("pluginTxtPath=" + pluginTxtPath);
            return ScriptReturnStatus.ERROR;
        }
        if (!System.IO.File.Exists(pluginTxtPath)) {
            AppManager.showMessageBox("'" + pluginTxtPath + "' does not exists");
            return ScriptReturnStatus.ERROR;
        }

        // plugin.txtがあれば，プラグインの実行ファイルのパスを取得する
        System.Text.Encoding shift_jis = System.Text.Encoding.GetEncoding("Shift_JIS");
        string name = "";
        string exe_path = "";
        using (StreamReader sr = new StreamReader(pluginTxtPath, shift_jis)) {
            string line = "";
            while ((line = sr.ReadLine()) != null) {
                string[] spl = line.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (line.StartsWith("name=")) {
                    name = spl[1];
                } else if (line.StartsWith("execute=")) {
                    exe_path = Path.Combine(Path.GetDirectoryName(pluginTxtPath), spl[1]);
                }
            }
        }
        if (exe_path == "") {
            return ScriptReturnStatus.ERROR;
        }
        if (!System.IO.File.Exists(exe_path)) {
            AppManager.showMessageBox("'" + exe_path + "' does not exists");
            return ScriptReturnStatus.ERROR;
        }

        // 選択状態のアイテムの最初と最後がどこか調べる
        // clock_start, clock_endは，最終的にはPREV, NEXTを含んだ範囲を表すことになる
        // sel_start, sel_endはPREV, NEXTを含まない選択範囲を表す
        int id_start = -1;
        int clock_start = int.MaxValue;
        int id_end = -1;
        int clock_end = int.MinValue;
        int sel_start = 0;
        int sel_end = 0;
        foreach (var item in AppManager.itemSelection.getEventIterator()) {
            if (item.original.ID.type != VsqIDType.Anote) {
                continue;
            }
            int clock = item.original.Clock;
            if (clock < clock_start) {
                id_start = item.original.InternalID;
                clock_start = clock;
                sel_start = clock;
            }
            clock += item.original.ID.getLength();
            if (clock_end < clock) {
                id_end = item.original.InternalID;
                clock_end = clock;
                sel_end = clock;
            }
        }

        // 選択範囲の前後の音符を探す
        VsqEvent ve_prev = null;
        VsqEvent ve_next = null;
        VsqEvent l = null;
        foreach (var item in vsq_track.getNoteEventIterator()) {
            if (item.InternalID == id_start) {
                if (l != null) {
                    ve_prev = l;
                }
            }
            if (l != null) {
                if (l.InternalID == id_end) {
                    ve_next = item;
                }
            }
            l = item;
            if (ve_prev != null && ve_next != null) {
                break;
            }
        }
        int next_rest_clock = -1;
        bool prev_is_rest = false;
        if (ve_prev != null) {
            // 直前の音符がある場合
            if (ve_prev.Clock + ve_prev.ID.getLength() == clock_start) {
                // 接続している
                clock_start = ve_prev.Clock;
            } else {
                // 接続していない
                int new_clock_start = ve_prev.Clock + ve_prev.ID.getLength();
                clock_start = new_clock_start;
            }
        } else {
            // 無い場合
            if (vsq.getPreMeasureClocks() < clock_start) {
                prev_is_rest = true;
            }
            int new_clock_start = vsq.getPreMeasureClocks();
            clock_start = new_clock_start;
        }
        if (ve_next != null) {
            // 直後の音符がある場合
            if (ve_next.Clock == clock_end) {
                // 接続している
                clock_end = ve_next.Clock + ve_next.ID.getLength();
            } else {
                // 接続していない
                next_rest_clock = clock_end;
                clock_end = ve_next.Clock;
            }
        }

        // 作業用のVSQに，選択範囲のアイテムを格納
        VsqFileEx v = (VsqFileEx)vsq.clone();// new VsqFile( "Miku", 1, 4, 4, 500000 );
        // 選択トラックだけ残して他を削る
        for (int i = 1; i < selected; i++) {
            v.Track.RemoveAt(1);
        }
        for (int i = selected + 1; i < v.Track.Count; i++) {
            v.Track.RemoveAt(selected + 1);
        }
        // 選択トラックの音符を全消去する
        VsqTrack v_track = v.Track[1];
        v_track.MetaText.getEventList().clear();
        foreach (var item in vsq_track.getNoteEventIterator()) {
            if (clock_start <= item.Clock && item.Clock + item.ID.getLength() <= clock_end) {
                v_track.addEvent((VsqEvent)item.clone(), item.InternalID);
            }
        }
        // 最後のRを手動で追加．これは自動化できない
        if (next_rest_clock != -1) {
            VsqEvent item = (VsqEvent)ve_next.clone();
            item.ID.LyricHandle.L0.Phrase = "R";
            item.Clock = next_rest_clock;
            item.ID.setLength(clock_end - next_rest_clock);
            v_track.addEvent(item);
        }
        // 0～選択範囲の開始位置までを削除する
        v.removePart(0, clock_start);

        // vsq -> ustに変換
        // キーがustのIndex, 値がInternalID
        SortedDictionary<int, int> map = new SortedDictionary<int, int>();
        UstFile u = new UstFile(v, 1, map);

        u.write(Path.Combine(PortUtil.getApplicationStartupPath(), "u.ust"));

        // PREV, NEXTのIndex値を設定する
        if (ve_prev != null || prev_is_rest) {
            u.getTrack(0).getEvent(0).Index = UstFile.PREV_INDEX;
        }
        if (ve_next != null) {
            u.getTrack(0).getEvent(u.getTrack(0).getEventCount() - 1).Index = UstFile.NEXT_INDEX;
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
        u.write(temp, option);

        StringBuilder before = new StringBuilder();
        using (StreamReader sr = new StreamReader(temp, System.Text.Encoding.GetEncoding("Shift_JIS"))) {
            string line = "";
            while ((line = sr.ReadLine()) != null) {
                before.AppendLine(line);
            }
        }
        string md5_before = PortUtil.getMD5FromString(before.ToString());
        // プラグインの実行ファイルを起動
        Utau_Plugin_Invoker dialog = new Utau_Plugin_Invoker(exe_path, temp);
        dialog.ShowDialog();
        StringBuilder after = new StringBuilder();
        using (StreamReader sr = new StreamReader(temp, System.Text.Encoding.GetEncoding("Shift_JIS"))) {
            string line = "";
            while ((line = sr.ReadLine()) != null) {
                after.AppendLine(line);
            }
        }
        string md5_after = PortUtil.getMD5FromString(after.ToString());
        if (md5_before == md5_after) {
            // 編集されなかったようだ
            return ScriptReturnStatus.NOT_EDITED;
        }

        // プラグインの実行結果をustオブジェクトにロード
        UstFile r = new UstFile(temp);
        if (r.getTrackCount() < 1) {
            return ScriptReturnStatus.ERROR;
        }

        // 変更のなかったものについてはプラグインは記録してこないので，
        // 最初の値を代入するようにする
        UstTrack utrack_src = u.getTrack(0);
        UstTrack utrack_dst = r.getTrack(0);
        for (int i = 0; i < utrack_dst.getEventCount(); i++) {
            UstEvent ue_dst = utrack_dst.getEvent(i);
            int index = ue_dst.Index;
            UstEvent ue_src = utrack_src.findEventFromIndex(index);
            if (ue_src == null) {
                continue;
            }
            if (!ue_dst.isEnvelopeSpecified() && ue_src.isEnvelopeSpecified()) {
                ue_dst.setEnvelope(ue_src.getEnvelope());
            }
            if (!ue_dst.isIntensitySpecified() && ue_src.isIntensitySpecified()) {
                ue_dst.setIntensity(ue_src.getIntensity());
            }
            if (!ue_dst.isLengthSpecified() && ue_src.isLengthSpecified()) {
                ue_dst.setLength(ue_src.getLength());
            }
            if (!ue_dst.isLyricSpecified() && ue_src.isLyricSpecified()) {
                ue_dst.setLyric(ue_src.getLyric());
            }
            if (!ue_dst.isModurationSpecified() && ue_src.isModurationSpecified()) {
                ue_dst.setModuration(ue_src.getModuration());
            }
            if (!ue_dst.isNoteSpecified() && ue_src.isNoteSpecified()) {
                ue_dst.setNote(ue_src.getNote());
            }
            if (!ue_dst.isPBTypeSpecified() && ue_src.isPBTypeSpecified()) {
                ue_dst.setPBType(ue_src.getPBType());
            }
            if (!ue_dst.isPitchesSpecified() && ue_src.isPitchesSpecified()) {
                ue_dst.setPitches(ue_src.getPitches());
            }
            if (!ue_dst.isPortamentoSpecified() && ue_src.isPortamentoSpecified()) {
                ue_dst.setPortamento(ue_src.getPortamento());
            }
            if (!ue_dst.isPreUtteranceSpecified() && ue_src.isPreUtteranceSpecified()) {
                ue_dst.setPreUtterance(ue_src.getPreUtterance());
            }
            if (!ue_dst.isStartPointSpecified() && ue_src.isStartPointSpecified()) {
                ue_dst.setStartPoint(ue_src.getStartPoint());
            }
            if (!ue_dst.isTempoSpecified() && ue_src.isTempoSpecified()) {
                ue_dst.setTempo(ue_src.getTempo());
            }
            if (!ue_dst.isVibratoSpecified() && ue_src.isVibratoSpecified()) {
                ue_dst.setVibrato(ue_src.getVibrato());
            }
            if (!ue_dst.isVoiceOverlapSpecified() && ue_src.isVoiceOverlapSpecified()) {
                ue_dst.setVoiceOverlap(ue_src.getVoiceOverlap());
            }
        }

        // PREVとNEXT含めて，clock_startからclock_endまでプラグインに渡したけれど，
        // それが伸びて帰ってきたか縮んで帰ってきたか．
        int ret_length = 0;
        UstTrack r_track = r.getTrack(0);
        int size = r_track.getEventCount();
        for (int i = 0; i < size; i++) {
            UstEvent ue = r_track.getEvent(i);
            // 戻りのustには，変更があったものしか記録されていない
            int ue_length = ue.getLength();
            if (!ue.isLengthSpecified() && map.ContainsKey(ue.Index)) {
                int internal_id = map[ue.Index];
                VsqEvent found_item = vsq_track.findEventFromID(internal_id);
                if (found_item != null) {
                    ue_length = found_item.ID.getLength();
                }
            }
            // PREV, ENDの場合は長さに加えない
            if (ue.Index != UstFile.NEXT_INDEX &&
                 ue.Index != UstFile.PREV_INDEX) {
                ret_length += ue_length;
            }
        }

        // 伸び縮みがあった場合
        // 伸ばしたり縮めたりするよ
        int delta = ret_length - (sel_end - sel_start);
        if (delta > 0) {
            // のびた
            vsq.insertBlank(selected, sel_end, delta);
        } else if (delta < 0) {
            // 縮んだ
            vsq.removePart(selected, sel_end + delta, sel_end);
        }

        // r_trackの内容をvsq_trackに転写
        size = r_track.getEventCount();
        int c = clock_start;
        for (int i = 0; i < size; i++) {
            UstEvent ue = r_track.getEvent(i);
            if (ue.Index == UstFile.NEXT_INDEX || ue.Index == UstFile.PREV_INDEX) {
                // PREVとNEXTは単に無視する
                continue;
            }
            int ue_length = ue.getLength();
            if (map.ContainsKey(ue.Index)) {
                // 既存の音符の編集
                VsqEvent target = vsq_track.findEventFromID(map[ue.Index]);
                if (target == null) {
                    // そんなばかな・・・
                    continue;
                }
                if (!ue.isLengthSpecified()) {
                    ue_length = target.ID.getLength();
                }
                if (target.UstEvent == null) {
                    target.UstEvent = (UstEvent)ue.clone();
                }
                // utau固有のパラメータを転写
                // pitchは後でやるので無視していい
                // テンポもあとでやるので無視していい
                if (ue.isEnvelopeSpecified()) {
                    target.UstEvent.setEnvelope(ue.getEnvelope());
                }
                if (ue.isModurationSpecified()) {
                    target.UstEvent.setModuration(ue.getModuration());
                }
                if (ue.isPBTypeSpecified()) {
                    target.UstEvent.setPBType(ue.getPBType());
                }
                if (ue.isPortamentoSpecified()) {
                    target.UstEvent.setPortamento(ue.getPortamento());
                }
                if (ue.isPreUtteranceSpecified()) {
                    target.UstEvent.setPreUtterance(ue.getPreUtterance());
                }
                if (ue.isStartPointSpecified()) {
                    target.UstEvent.setStartPoint(ue.getStartPoint());
                }
                if (ue.isVibratoSpecified()) {
                    target.UstEvent.setVibrato(ue.getVibrato());
                }
                if (ue.isVoiceOverlapSpecified()) {
                    target.UstEvent.setVoiceOverlap(ue.getVoiceOverlap());
                }
                // vocaloid, utauで同じ意味のパラメータを転写
                if (ue.isIntensitySpecified()) {
                    target.UstEvent.setIntensity(ue.getIntensity());
                    target.ID.Dynamics = ue.getIntensity();
                }
                if (ue.isLengthSpecified()) {
                    target.UstEvent.setLength(ue.getLength());
                    target.ID.setLength(ue.getLength());
                }
                if (ue.isLyricSpecified()) {
                    target.UstEvent.setLyric(ue.getLyric());
                    target.ID.LyricHandle.L0.Phrase = ue.getLyric();
                }
                if (ue.isNoteSpecified()) {
                    target.UstEvent.setNote(ue.getNote());
                    target.ID.Note = ue.getNote();
                }
            } else {
                // マップに入っていないので，新しい音符の追加だと思う
                if (ue.getLyric() == "R") {
                    // 休符．なにもしない
                } else {
                    VsqEvent newe = new VsqEvent();
                    newe.Clock = c;
                    newe.UstEvent = (UstEvent)ue.clone();
                    newe.ID = new VsqID();
                    AppManager.editorConfig.applyDefaultSingerStyle(newe.ID);
                    if (ue.isIntensitySpecified()) {
                        newe.ID.Dynamics = ue.getIntensity();
                    }
                    newe.ID.LyricHandle = new LyricHandle("あ", "a");
                    if (ue.isLyricSpecified()) {
                        newe.ID.LyricHandle.L0.Phrase = ue.getLyric();
                    }
                    newe.ID.Note = ue.getNote();
                    newe.ID.setLength(ue.getLength());
                    newe.ID.type = VsqIDType.Anote;
                    // internal id はaddEventメソッドで自動で割り振られる
                    vsq_track.addEvent(newe);
                }
            }

            // テンポの追加がないかチェック
            if (ue.isTempoSpecified()) {
                insertTempoInto(vsq, c, ue.getTempo());
            }

            c += ue_length;
        }

        // ピッチを転写
        // pitのデータがほしいので，PREV, NEXTを削除して，VsqFileにコンバートする
        UstFile uf = (UstFile)r.clone();
        // prev, nextを削除
        UstTrack uf_track = uf.getTrack(0);
        for (int i = 0; i < uf_track.getEventCount(); ) {
            UstEvent ue = uf_track.getEvent(i);
            if (ue.Index == UstFile.NEXT_INDEX ||
                 ue.Index == UstFile.PREV_INDEX) {
                uf_track.removeEventAt(i);
            } else {
                i++;
            }
        }
        uf.updateTempoInfo();
        // VsqFileにコンバート
        VsqFile uf_vsq = new VsqFile(uf);
        // uf_vsqの最初のトラックの0からret_lengthクロックまでが，
        // vsq_trackのsel_startからsel_start+ret_lengthクロックまでに対応する．
        // まずPBSをコピーする
        CurveType[] type = new CurveType[] { CurveType.PBS, CurveType.PIT };
        foreach (CurveType ct in type) {
            // コピー元を取得
            VsqBPList src = uf_vsq.Track[1].getCurve(ct.getName());
            if (src != null) {
                // コピー先を取得
                VsqBPList dst = vsq_track.getCurve(ct.getName());
                if (dst == null) {
                    // コピー先がnullだった場合は作成
                    dst = new VsqBPList(ct.getName(), ct.getDefault(), ct.getMinimum(), ct.getMaximum());
                    vsq_track.setCurve(ct.getName(), dst);
                }
                // あとで復元するので，最終位置での値を保存しておく
                int value_at_end = dst.getValue(sel_start + ret_length);
                // 復元するかどうか．最終位置にそもそもデータ点があれば復帰の必要がないので．
                bool do_revert = (dst.findIndexFromClock(sel_start + ret_length) < 0);
                // [sel_start, sel_start + ret_length)の範囲の値を削除しておく
                size = dst.size();
                for (int i = size - 1; i >= 0; i--) {
                    int cl = dst.getKeyClock(i);
                    if (sel_start <= cl && cl < sel_start + ret_length) {
                        dst.removeElementAt(i);
                    }
                }
                // コピーを実行
                size = src.size();
                for (int i = 0; i < size; i++) {
                    int cl = src.getKeyClock(i);
                    if (ret_length <= cl) {
                        break;
                    }
                    int value = src.getElementA(i);
                    dst.add(cl + sel_start, value);
                }
                // コピー後，最終位置での値が元と異なる場合，元に戻すようにする
                if (do_revert && dst.getValue(sel_start + ret_length) != value_at_end) {
                    dst.add(sel_start + ret_length, value_at_end);
                }
            }
        }

        return ScriptReturnStatus.EDITED;
    }

    /// <summary>
    /// 指定したVSQの指定した位置に，テンポの挿入を試みます．
    /// 既存のテンポがある場合，値が上書きされます
    /// </summary>
    /// <param name="vsq">挿入対象のVSQ</param>
    /// <param name="clock">挿入位置</param>
    /// <param name="tempo">楽譜表記上のテンポ．BPS</param>
    private static void insertTempoInto(VsqFileEx vsq, int clock, float t)
    {
        // clockの位置にテンポ変更があるかどうか？
        int num_tempo = vsq.TempoTable.Count;
        int index = -1;
        for (int j = 0; j < num_tempo; j++) {
            TempoTableEntry itemj = vsq.TempoTable[j];
            if (itemj.Clock == clock) {
                index = j;
                break;
            }
        }
        int tempo = (int)(60e6 / t);
        if (index >= 0) {
            // clock位置に既存のテンポ変更がある場合，テンポ値を変更
            TempoTableEntry itemj = vsq.TempoTable[index];
            itemj.Tempo = tempo;
        } else {
            // 既存のものはないので新規に追加
            vsq.TempoTable.Add(new TempoTableEntry(clock, tempo, 0.0));
        }
        // テンポテーブルを更新
        vsq.TempoTable.updateTempoInfo();
    }

    private void Utau_Plugin_Invoker_Load(object sender, EventArgs e)
    {
        bgWork.RunWorkerAsync();
    }

    private void bgWork_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
        string dquote = new string((char)0x22, 1);
        using (System.Diagnostics.Process p = new System.Diagnostics.Process()) {
            p.StartInfo.FileName = m_exe_path;
            p.StartInfo.Arguments = dquote + m_temp + dquote;
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(m_exe_path);
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            p.Start();
            p.WaitForExit();
        }
    }

    private void bgWork_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
    {
        this.Close();
    }

    public static string GetDisplayName()
    {
        return s_display_name;
    }
}
