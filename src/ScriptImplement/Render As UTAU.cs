// Render As UTAU.cs for Cadencii
// written by kbinani & 88
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using cadencii.vsq;
using cadencii.apputil;
using cadencii;
using cadencii.java.util;
using cadencii;



public class RenderAsUtau : Form
{
    private System.ComponentModel.IContainer components = null;
    public static string Resampler = "";
    public static string WavTool = "";
    public static string Singer = "";
    public static string LastWave = "";
    private Label lblResampler;
    private TextBox txtSinger;
    private Button btnSinger;
    private GroupBox groupConfig;
    private Button btnWavtool;
    private Button btnResampler;
    private TextBox txtWavtool;
    private TextBox txtResampler;
    private Label lblWavtool;
    private Button btnOk;
    private FolderBrowserDialog folderBrowserDialog;
    private OpenFileDialog openFileDialog;
    private PictureBox pictSumbnail;
    private TextBox txtProf;
    private GroupBox groupSinger;
    private Label lblName;
    private static SaveFileDialog saveFileDialog;
    private Label lblDirectory;
    private Button btnCancel;

    struct Phon
    {
        public string Lyric;
        public string FileName;
        public int ClockLength;
        public float Tempo;
        public bool ModeR;
        public Phon(string lyric, string file_name, int clock_length, float tempo, bool mode_r)
        {
            Lyric = lyric;
            FileName = file_name;
            ClockLength = clock_length;
            Tempo = tempo;
            ModeR = mode_r;
        }
    }

    /// <summary>
    /// 原音設定の引数．
    /// </summary>
    struct OtoArgs
    {
        public string Alias;
        public int msOffset;
        public int msConsonant;
        public int msBlank;
        public int msPreUtterance;
        public int msOverwrap;
    }

    public RenderAsUtau()
    {
        InitializeComponent();
        txtResampler.Text = Resampler;
        txtWavtool.Text = WavTool;
        txtSinger.Text = Singer;
        folderBrowserDialog.SelectedPath = Singer;
        saveFileDialog = new SaveFileDialog();
        if (LastWave != "") {
            try {
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(LastWave);
            } catch {
            }
        }
        saveFileDialog.Filter = "Wave File(*.wav)|*.wav|All Files(*.*)|*.*";

        bool entered = false;
        if (txtResampler.Text != "") {
            try {
                string dir = Path.GetDirectoryName(txtResampler.Text);
                openFileDialog.InitialDirectory = dir;
                entered = true;
            } catch {
            }
        }
        if (!entered && txtWavtool.Text != "") {
            try {
                string dir = Path.GetDirectoryName(txtWavtool.Text);
                openFileDialog.InitialDirectory = dir;
                entered = true;
            } catch {
            }
        }
        CheckOkButtonAvailable();
        UpdateProfile();
        applyLanguage();
    }

    void applyLanguage()
    {
        if (Messaging.getLanguage() == "ja") {
            groupSinger.Text = "音源";
            groupConfig.Text = "設定";
            btnCancel.Text = "取消";
            btnOk.Text = "了解";
            btnSinger.Text = "探す";
            btnResampler.Text = "探す";
            btnWavtool.Text = "探す";
            lblDirectory.Text = "音源フォルダ";
        }
    }

    void UpdateProfile()
    {
        if (Singer == "" || !Directory.Exists(Singer)) {
            pictSumbnail.Image = null;
            lblName.Text = "(Unknown)";
            return;
        }
        string character = Path.Combine(Singer, "character.txt");
        if (File.Exists(character)) {
            using (cp932reader sr = new cp932reader(character)) {
                string line = "";
                while ((line = sr.ReadLine()) != null) {
                    string[] spl = line.Split("=".ToCharArray(), 2);
                    if (spl.Length >= 2) {
                        if (spl[0].ToLower() == "name") {
                            lblName.Text = spl[1];
                        } else if (spl[0].ToLower() == "image") {
                            string image = Path.Combine(Singer, spl[1]);
                            if (File.Exists(image)) {
                                try {
                                    pictSumbnail.Image = System.Drawing.Bitmap.FromFile(image);
                                } catch {
                                }
                            }
                        }
                    }
                }
            }
        }

        string readme = Path.Combine(Singer, "readme.txt");
        if (File.Exists(readme)) {
            using (cp932reader sr = new cp932reader(readme)) {
                txtProf.Text = sr.ReadToEnd();
            }
        } else {
            txtProf.Text = "";
        }
    }

    public static bool Edit(VsqFile vsq)
    {
        using (RenderAsUtau dlg = new RenderAsUtau()) {
            if (dlg.ShowDialog() == DialogResult.OK) {
                Singer = dlg.txtSinger.Text;
                Resampler = dlg.txtResampler.Text;
                WavTool = dlg.txtWavtool.Text;
                string script = Path.Combine(Application.StartupPath, Path.Combine("script", "Render As UTAU.cs"));//Script.ScriptPath;
                string temp_dir = Path.Combine(Path.GetDirectoryName(script), Path.GetFileNameWithoutExtension(script));

#if DEBUG
                if (!Directory.Exists(temp_dir)) {
                    Directory.CreateDirectory(temp_dir);
                }
                StreamWriter sw = new StreamWriter(Path.Combine(temp_dir, "log.txt"));
#endif
                // 原音設定を読み込み
                Dictionary<string, OtoArgs> config = new Dictionary<string, OtoArgs>();
                string singer_name = Path.GetFileName(Singer);
                string config_file = Path.Combine(Singer, "oto.ini");
#if DEBUG
                sw.WriteLine("Singer=" + Singer);
                sw.WriteLine("singer_name=" + singer_name);
                sw.WriteLine("config_file=" + config_file);
#endif
                if (File.Exists(config_file)) {
                    using (cp932reader sr = new cp932reader(config_file)) {
                        string line;
                        while (sr.Peek() >= 0) {
                            try {
                                line = sr.ReadLine();
                                string[] spl = line.Split('=');
                                string file_name = spl[0]; // あ.wav
                                string a2 = spl[1]; // ,0,36,64,0,0
                                string a1 = Path.GetFileNameWithoutExtension(file_name);
                                spl = a2.Split(',');
                                OtoArgs oa = new OtoArgs();
                                oa.Alias = spl[0];
                                oa.msOffset = int.Parse(spl[1]);
                                oa.msConsonant = int.Parse(spl[2]);
                                oa.msBlank = int.Parse(spl[3]);
                                oa.msPreUtterance = int.Parse(spl[4]);
                                oa.msOverwrap = int.Parse(spl[5]);
                                config.Add(a1, oa);
                            } catch {
                            }
                        }
                    }
                }

                int track = AppManager.getSelected();
                List<Phon> phons = new List<Phon>();
                if (!Directory.Exists(temp_dir)) {
                    Directory.CreateDirectory(temp_dir);
                }
                int count = -1;
                double sec_end = 0;
                double sec_end_old = 0;
                foreach (var item in vsq.Track[track].getNoteEventIterator()) {
                    count++;
                    double sec_start = vsq.getSecFromClock(item.Clock);
                    sec_end_old = sec_end;
                    sec_end = vsq.getSecFromClock(item.Clock + item.ID.Length);
                    float t_temp = (float)(item.ID.Length / (sec_end - sec_start) / 8.0);
                    if ((count == 0 && sec_start > 0.0) || (sec_start > sec_end_old)) {
                        double sec_start2 = sec_end_old;
                        double sec_end2 = sec_start;
                        float t_temp2 = (float)(item.Clock / (sec_end2 - sec_start2) / 8.0);
                        phons.Add(new Phon("R", Path.Combine(Singer, "R.wav"), item.Clock, t_temp2, true));
                        count++;
                    }
                    string lyric = item.ID.LyricHandle.L0.Phrase;
                    string note = NoteStringFromNoteNumber(item.ID.Note);
#if DEBUG
                    sw.WriteLine("note=" + note);
#endif
                    string millisec = ((int)((sec_end - sec_start) * 1000) + 50).ToString();

                    //4_あ_C#4_550.wav
                    string filename = Path.Combine(temp_dir, count + "_" + item.ID.Note + "_" + millisec + ".wav");
#if DEBUG
                    sw.WriteLine("filename=" + filename);
                    sw.WriteLine();
#endif
                    if (File.Exists(filename)) {
                        PortUtil.deleteFile(filename);
                    }

                    phons.Add(new Phon(lyric, filename, item.ID.Length, t_temp, false));

                    OtoArgs oa = new OtoArgs();
                    if (config.ContainsKey(lyric)) {
                        oa = config[lyric];
                    }
                    int velocity = 100;
                    int moduration = 100;
                    string flags = "L";
                    int time_percent = 100;
                    //                                                                                          C4             100                  L             0                   550              0                      0                  100              100
                    string arg = "\"" + Path.Combine(Singer, lyric + ".wav") + "\" \"" + filename + "\" \"" + note + "\" " + time_percent + " " + flags + " " + oa.msOffset + " " + millisec + " " + oa.msConsonant + " " + oa.msBlank + " " + velocity + " " + moduration;

                    using (System.Diagnostics.Process p = new System.Diagnostics.Process()) {
                        p.StartInfo.FileName = "\"" + Resampler + "\"";
                        p.StartInfo.Arguments = arg;
                        p.StartInfo.WorkingDirectory = temp_dir;
                        p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        p.Start();
                        p.WaitForExit();
                    }
                }
#if DEBUG
                sw.Close();
#endif

                string filebase = "temp.wav";
                string file = Path.Combine(temp_dir, filebase);
                if (File.Exists(file)) {
                    PortUtil.deleteFile(file);
                }
                string file_whd = Path.Combine(temp_dir, filebase + ".whd");
                if (File.Exists(file_whd)) {
                    PortUtil.deleteFile(file_whd);
                }
                string file_dat = Path.Combine(temp_dir, filebase + ".dat");
                if (File.Exists(file_dat)) {
                    PortUtil.deleteFile(file_dat);
                }

                // wavtoolを呼び出す
                for (int i = 0; i < phons.Count; i++) {
                    OtoArgs oa = new OtoArgs();
                    if (config.ContainsKey(phons[i].Lyric)) {
                        oa = config[phons[i].Lyric];
                    }
                    // 次の音符の先行発声とオーバーラップを取得
                    OtoArgs oa_next = new OtoArgs();
                    if (i + 1 < phons.Count) {
                        if (config.ContainsKey(phons[i + 1].Lyric)) {
                            oa_next = config[phons[i + 1].Lyric];
                        }
                    }
                    int mten = oa.msPreUtterance + oa_next.msOverwrap - oa_next.msPreUtterance;
                    string arg = filebase + " \"" + phons[i].FileName + "\" 0 " + phons[i].ClockLength + "@" + string.Format("{0:f2}", phons[i].Tempo) + mten.ToString("+#;-#;0");
                    if (phons[i].ModeR) {
                        arg += " 0 0";
                    } else {
                        arg += " 0 5 35 0 100 100 100 " + oa.msOverwrap; // エンベロープ
                    }

                    using (System.Diagnostics.Process p = new System.Diagnostics.Process()) {
                        p.StartInfo.FileName = "\"" + WavTool + "\"";
                        p.StartInfo.Arguments = arg;
                        p.StartInfo.WorkingDirectory = temp_dir;
                        p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        p.Start();
                        p.WaitForExit();
                    }
                }

                // 波形とヘッダを結合
                using (FileStream fs = new FileStream(file, FileMode.Create)) {
                    string[] files = new string[] { file_whd, file_dat };
                    int buflen = 512;
                    byte[] buff = new byte[buflen];
                    for (int i = 0; i < files.Length; i++) {
                        using (FileStream fs2 = new FileStream(files[i], FileMode.Open)) {
                            int len = fs2.Read(buff, 0, buflen);
                            while (len > 0) {
                                fs.Write(buff, 0, len);
                                len = fs2.Read(buff, 0, buflen);
                            }
                        }
                    }
                }

                // 後片付け
                foreach (Phon ph in phons) {
                    if (!ph.ModeR) {
                        if (File.Exists(ph.FileName)) {
                            PortUtil.deleteFile(ph.FileName);
                        }
                    }
                }
                if (File.Exists(file_whd)) {
                    PortUtil.deleteFile(file_whd);
                }
                if (File.Exists(file_dat)) {
                    PortUtil.deleteFile(file_dat);
                }

                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    if (File.Exists(saveFileDialog.FileName)) {
                        PortUtil.deleteFile(saveFileDialog.FileName);
                    }
                    LastWave = saveFileDialog.FileName;
                    PortUtil.moveFile(file, saveFileDialog.FileName);
                } else {
                    PortUtil.deleteFile(file);
                }
                return true;
            } else {
                return false;
            }
        }
    }

    private static string NoteStringFromNoteNumber(int note_number)
    {
        int odd = note_number % 12;
        string head = (new string[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" })[odd];
        return head + (note_number / 12 - 1);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.lblResampler = new System.Windows.Forms.Label();
        this.txtSinger = new System.Windows.Forms.TextBox();
        this.btnSinger = new System.Windows.Forms.Button();
        this.groupConfig = new System.Windows.Forms.GroupBox();
        this.btnWavtool = new System.Windows.Forms.Button();
        this.btnResampler = new System.Windows.Forms.Button();
        this.txtWavtool = new System.Windows.Forms.TextBox();
        this.txtResampler = new System.Windows.Forms.TextBox();
        this.lblWavtool = new System.Windows.Forms.Label();
        this.btnOk = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
        this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
        this.pictSumbnail = new System.Windows.Forms.PictureBox();
        this.txtProf = new System.Windows.Forms.TextBox();
        this.groupSinger = new System.Windows.Forms.GroupBox();
        this.lblName = new System.Windows.Forms.Label();
        this.lblDirectory = new System.Windows.Forms.Label();
        this.groupConfig.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictSumbnail)).BeginInit();
        this.groupSinger.SuspendLayout();
        this.SuspendLayout();
        // 
        // lblResampler
        // 
        this.lblResampler.AutoSize = true;
        this.lblResampler.Location = new System.Drawing.Point(11, 29);
        this.lblResampler.Name = "lblResampler";
        this.lblResampler.Size = new System.Drawing.Size(55, 12);
        this.lblResampler.TabIndex = 0;
        this.lblResampler.Text = "resampler";
        // 
        // txtSinger
        // 
        this.txtSinger.Location = new System.Drawing.Point(98, 176);
        this.txtSinger.Name = "txtSinger";
        this.txtSinger.Size = new System.Drawing.Size(215, 19);
        this.txtSinger.TabIndex = 2;
        this.txtSinger.TextChanged += new System.EventHandler(this.txtSinger_TextChanged);
        // 
        // btnSinger
        // 
        this.btnSinger.Location = new System.Drawing.Point(319, 174);
        this.btnSinger.Name = "btnSinger";
        this.btnSinger.Size = new System.Drawing.Size(56, 23);
        this.btnSinger.TabIndex = 3;
        this.btnSinger.Text = "Browse";
        this.btnSinger.UseVisualStyleBackColor = true;
        this.btnSinger.Click += new System.EventHandler(this.btnSinger_Click);
        // 
        // groupConfig
        // 
        this.groupConfig.Controls.Add(this.btnWavtool);
        this.groupConfig.Controls.Add(this.btnResampler);
        this.groupConfig.Controls.Add(this.txtWavtool);
        this.groupConfig.Controls.Add(this.txtResampler);
        this.groupConfig.Controls.Add(this.lblWavtool);
        this.groupConfig.Controls.Add(this.lblResampler);
        this.groupConfig.Location = new System.Drawing.Point(14, 236);
        this.groupConfig.Name = "groupConfig";
        this.groupConfig.Size = new System.Drawing.Size(394, 106);
        this.groupConfig.TabIndex = 4;
        this.groupConfig.TabStop = false;
        this.groupConfig.Text = "Configuration";
        // 
        // btnWavtool
        // 
        this.btnWavtool.Location = new System.Drawing.Point(319, 49);
        this.btnWavtool.Name = "btnWavtool";
        this.btnWavtool.Size = new System.Drawing.Size(56, 23);
        this.btnWavtool.TabIndex = 7;
        this.btnWavtool.Text = "Browse";
        this.btnWavtool.UseVisualStyleBackColor = true;
        this.btnWavtool.Click += new System.EventHandler(this.btnWavtool_Click);
        // 
        // btnResampler
        // 
        this.btnResampler.Location = new System.Drawing.Point(319, 24);
        this.btnResampler.Name = "btnResampler";
        this.btnResampler.Size = new System.Drawing.Size(56, 23);
        this.btnResampler.TabIndex = 5;
        this.btnResampler.Text = "Browse";
        this.btnResampler.UseVisualStyleBackColor = true;
        this.btnResampler.Click += new System.EventHandler(this.btnResampler_Click);
        // 
        // txtWavtool
        // 
        this.txtWavtool.Location = new System.Drawing.Point(77, 51);
        this.txtWavtool.Name = "txtWavtool";
        this.txtWavtool.Size = new System.Drawing.Size(236, 19);
        this.txtWavtool.TabIndex = 6;
        this.txtWavtool.TextChanged += new System.EventHandler(this.txtWavtool_TextChanged);
        // 
        // txtResampler
        // 
        this.txtResampler.Location = new System.Drawing.Point(77, 26);
        this.txtResampler.Name = "txtResampler";
        this.txtResampler.Size = new System.Drawing.Size(236, 19);
        this.txtResampler.TabIndex = 5;
        this.txtResampler.TextChanged += new System.EventHandler(this.txtResampler_TextChanged);
        // 
        // lblWavtool
        // 
        this.lblWavtool.AutoSize = true;
        this.lblWavtool.Location = new System.Drawing.Point(11, 54);
        this.lblWavtool.Name = "lblWavtool";
        this.lblWavtool.Size = new System.Drawing.Size(44, 12);
        this.lblWavtool.TabIndex = 1;
        this.lblWavtool.Text = "wavtool";
        // 
        // btnOk
        // 
        this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnOk.Location = new System.Drawing.Point(252, 356);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(75, 23);
        this.btnOk.TabIndex = 6;
        this.btnOk.Text = "OK";
        this.btnOk.UseVisualStyleBackColor = true;
        // 
        // btnCancel
        // 
        this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnCancel.Location = new System.Drawing.Point(333, 356);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(75, 23);
        this.btnCancel.TabIndex = 7;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        // 
        // openFileDialog
        // 
        this.openFileDialog.FileName = "openFileDialog1";
        // 
        // pictSumbnail
        // 
        this.pictSumbnail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.pictSumbnail.Location = new System.Drawing.Point(18, 52);
        this.pictSumbnail.Name = "pictSumbnail";
        this.pictSumbnail.Size = new System.Drawing.Size(100, 100);
        this.pictSumbnail.TabIndex = 8;
        this.pictSumbnail.TabStop = false;
        // 
        // txtProf
        // 
        this.txtProf.BackColor = System.Drawing.SystemColors.Window;
        this.txtProf.Location = new System.Drawing.Point(131, 47);
        this.txtProf.Multiline = true;
        this.txtProf.Name = "txtProf";
        this.txtProf.ReadOnly = true;
        this.txtProf.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        this.txtProf.Size = new System.Drawing.Size(247, 111);
        this.txtProf.TabIndex = 9;
        this.txtProf.WordWrap = false;
        // 
        // groupSinger
        // 
        this.groupSinger.Controls.Add(this.lblDirectory);
        this.groupSinger.Controls.Add(this.lblName);
        this.groupSinger.Controls.Add(this.pictSumbnail);
        this.groupSinger.Controls.Add(this.txtProf);
        this.groupSinger.Controls.Add(this.txtSinger);
        this.groupSinger.Controls.Add(this.btnSinger);
        this.groupSinger.Location = new System.Drawing.Point(14, 12);
        this.groupSinger.Name = "groupSinger";
        this.groupSinger.Size = new System.Drawing.Size(394, 218);
        this.groupSinger.TabIndex = 10;
        this.groupSinger.TabStop = false;
        this.groupSinger.Text = "Singer";
        // 
        // lblName
        // 
        this.lblName.AutoSize = true;
        this.lblName.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
        this.lblName.Location = new System.Drawing.Point(21, 23);
        this.lblName.Name = "lblName";
        this.lblName.Size = new System.Drawing.Size(74, 13);
        this.lblName.TabIndex = 10;
        this.lblName.Text = "(Unknown)";
        // 
        // lblDirectory
        // 
        this.lblDirectory.AutoSize = true;
        this.lblDirectory.Location = new System.Drawing.Point(11, 179);
        this.lblDirectory.Name = "lblDirectory";
        this.lblDirectory.Size = new System.Drawing.Size(81, 12);
        this.lblDirectory.TabIndex = 11;
        this.lblDirectory.Text = "voice directory";
        // 
        // RenderAsUtau
        // 
        this.AcceptButton = this.btnOk;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(420, 393);
        this.Controls.Add(this.groupSinger);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnOk);
        this.Controls.Add(this.groupConfig);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "RenderAsUtau";
        this.ShowIcon = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Render As UTAU";
        this.groupConfig.ResumeLayout(false);
        this.groupConfig.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictSumbnail)).EndInit();
        this.groupSinger.ResumeLayout(false);
        this.groupSinger.PerformLayout();
        this.ResumeLayout(false);

    }

    private void btnSinger_Click(object sender, EventArgs e)
    {
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
            txtSinger.Text = folderBrowserDialog.SelectedPath;
            UpdateProfile();
        }
    }

    private void CheckOkButtonAvailable()
    {
        if (!File.Exists(Resampler) || !File.Exists(WavTool) || !Directory.Exists(Singer)) {
            btnOk.Enabled = false;
        } else {
            btnOk.Enabled = true;
        }
    }

    private void btnResampler_Click(object sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog() == DialogResult.OK) {
            txtResampler.Text = openFileDialog.FileName;
        }
    }

    private void btnWavtool_Click(object sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog() == DialogResult.OK) {
            txtWavtool.Text = openFileDialog.FileName;
        }
    }

    private void txtResampler_TextChanged(object sender, EventArgs e)
    {
        Resampler = txtResampler.Text;
        CheckOkButtonAvailable();
    }

    private void txtWavtool_TextChanged(object sender, EventArgs e)
    {
        WavTool = txtWavtool.Text;
        CheckOkButtonAvailable();
    }

    private void txtSinger_TextChanged(object sender, EventArgs e)
    {
        Singer = txtSinger.Text;
        CheckOkButtonAvailable();
        UpdateProfile();
    }
}