/*
 * FormGenerateKeySound.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using cadencii.java.util;
using cadencii.media;
using cadencii.vsq;
using cadencii.windows.forms;

namespace cadencii
{

    public class FormGenerateKeySound : Form
    {
        private delegate void updateTitleDelegate(string title);

        public class PrepareStartArgument
        {
            public string singer = "Miku";
            public double amplitude = 1.0;
            public string directory = "";
            public bool replace = true;
        }

        const int _SAMPLE_RATE = 44100;

        private FolderBrowserDialog folderBrowser;
        private System.ComponentModel.BackgroundWorker bgWork;
        private SingerConfig[] m_singer_config1;
        private SingerConfig[] m_singer_config2;
        private SingerConfig[] m_singer_config_utau;
        private bool m_cancel_required = false;
        /// <summary>
        /// 処理が終わったら自動でフォームを閉じるかどうか。デフォルトではfalse（閉じない）
        /// </summary>
        private bool m_close_when_finished = false;

        public FormGenerateKeySound(bool close_when_finished)
        {
            InitializeComponent();
            bgWork = new System.ComponentModel.BackgroundWorker();
            bgWork.WorkerReportsProgress = true;
            bgWork.WorkerSupportsCancellation = true;
            folderBrowser = new FolderBrowserDialog();

            m_close_when_finished = close_when_finished;
            m_singer_config1 = VocaloSysUtil.getSingerConfigs(SynthesizerType.VOCALOID1);
            m_singer_config2 = VocaloSysUtil.getSingerConfigs(SynthesizerType.VOCALOID2);
            m_singer_config_utau = AppManager.editorConfig.UtauSingers.ToArray();
            if (m_singer_config1.Length > 0) {
                comboSingingSynthSystem.Items.Add("VOCALOID1");
            }
            if (m_singer_config2.Length > 0) {
                comboSingingSynthSystem.Items.Add("VOCALOID2");
            }

            // 取りあえず最初に登録されているresamplerを使うってことで
            string resampler = AppManager.editorConfig.getResamplerAt(0);
            if (m_singer_config_utau.Length > 0 &&
                 AppManager.editorConfig.PathWavtool != null && File.Exists(AppManager.editorConfig.PathWavtool) &&
                 resampler != null && File.Exists(resampler)) {
                comboSingingSynthSystem.Items.Add("UTAU");
            }
            if (comboSingingSynthSystem.Items.Count > 0) {
                comboSingingSynthSystem.SelectedIndex = 0;
            }
            updateSinger();
            txtDir.Text = Utility.getKeySoundPath();

            registerEventHandlers();
        }

        #region helper methods
        private void registerEventHandlers()
        {
            bgWork.DoWork += new DoWorkEventHandler(bgWork_DoWork);
            bgWork.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWork_RunWorkerCompleted);
            bgWork.ProgressChanged += new ProgressChangedEventHandler(bgWork_ProgressChanged);
        }

        private void updateSinger()
        {
            if (comboSingingSynthSystem.SelectedIndex < 0) {
                return;
            }
            string singer = (string)comboSingingSynthSystem.SelectedItem;
            SingerConfig[] list = null;
            if (singer.Equals("VOCALOID1")) {
                list = m_singer_config1;
            } else if (singer.Equals("VOCALOID2")) {
                list = m_singer_config2;
            } else if (singer.Equals("UTAU")) {
                list = m_singer_config_utau;
            }
            comboSinger.Items.Clear();
            if (list == null) {
                return;
            }
            for (int i = 0; i < list.Length; i++) {
                comboSinger.Items.Add(list[i].VOICENAME);
            }
            if (comboSinger.Items.Count > 0) {
                comboSinger.SelectedIndex = 0;
            }
        }

        private void updateTitle(string title)
        {
            this.Text = title;
        }

        private void updateEnabled(bool enabled)
        {
            comboSinger.Enabled = enabled;
            comboSingingSynthSystem.Enabled = enabled;
            txtDir.ReadOnly = !enabled;
            btnBrowse.Enabled = enabled;
            btnExecute.Enabled = enabled;
            chkIgnoreExistingWavs.Enabled = enabled;
            if (enabled) {
                btnCancel.Text = "Close";
            } else {
                btnCancel.Text = "Cancel";
            }
        }
        #endregion

        #region event handlers
        public void comboSingingSynthSystem_SelectedIndexChanged(Object sender, EventArgs e)
        {
            updateSinger();
        }

        public void btnBrowse_Click(Object sender, EventArgs e)
        {
            folderBrowser.SelectedPath = txtDir.Text;
            if (folderBrowser.ShowDialog(this) != DialogResult.OK) {
                return;
            }
            txtDir.Text = folderBrowser.SelectedPath;
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            if (bgWork.IsBusy) {
                m_cancel_required = true;
                while (m_cancel_required) {
                    Application.DoEvents();
                }
            } else {
                this.Close();
            }
        }

        public void btnExecute_Click(Object sender, EventArgs e)
        {
            PrepareStartArgument arg = new PrepareStartArgument();
            arg.singer = (string)comboSinger.SelectedItem;
            arg.amplitude = 1.0;
            arg.directory = txtDir.Text;
            arg.replace = chkIgnoreExistingWavs.Checked;
            updateEnabled(false);
            bgWork.RunWorkerAsync(arg);
        }

        public void bgWork_DoWork(Object sender, DoWorkEventArgs e)
        {
#if DEBUG
            sout.println("FormGenerateKeySound#bgWork_DoWork");
#endif
            PrepareStartArgument arg = (PrepareStartArgument)e.Argument;
            string singer = arg.singer;
            double amp = arg.amplitude;
            string dir = arg.directory;
            bool replace = arg.replace;
            // 音源を準備
            if (!Directory.Exists(dir)) {
                PortUtil.createDirectory(dir);
            }

            for (int i = 0; i < 127; i++) {
                string path = Path.Combine(dir, i + ".wav");
                sout.println("writing \"" + path + "\" ...");
                if (replace || (!replace && !File.Exists(path))) {
                    try {
                        GenerateSinglePhone(i, singer, path, amp);
                        if (File.Exists(path)) {
                            try {
                                Wave wv = new Wave(path);
                                wv.trimSilence();
                                wv.monoralize();
                                wv.write(path);
                            } catch (Exception ex0) {
                                serr.println("FormGenerateKeySound#bgWork_DoWork; ex0=" + ex0);
                                Logger.write(typeof(FormGenerateKeySound) + ".bgWork_DoWork; ex=" + ex0 + "\n");
                            }
                        }
                    } catch (Exception ex) {
                        Logger.write(typeof(FormGenerateKeySound) + ".bgWork_DoWork; ex=" + ex + "\n");
                        serr.println("FormGenerateKeySound#bgWork_DoWork; ex=" + ex);
                    }
                }
                sout.println(" done");
                if (m_cancel_required) {
                    m_cancel_required = false;
                    break;
                }
                bgWork.ReportProgress((int)(i / 127.0 * 100.0), null);
            }
            m_cancel_required = false;
        }

        private void bgWork_ProgressChanged(Object sender, ProgressChangedEventArgs e)
        {
            string title = "Progress: " + e.ProgressPercentage + "%";
            this.Invoke(new updateTitleDelegate(this.updateTitle), new Object[] { title });
        }

        public void Program_FormClosed(Object sender, FormClosedEventArgs e)
        {
            VSTiDllManager.terminate();
        }

        public void bgWork_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            updateEnabled(true);
            if (m_close_when_finished) {
                Close();
            }
        }
        #endregion

        #region public static methods
        public static void GenerateSinglePhone(int note, string singer, string file, double amp)
        {
            string renderer = "";
            SingerConfig[] singers1 = VocaloSysUtil.getSingerConfigs(SynthesizerType.VOCALOID1);
            int c = singers1.Length;
            string first_found_singer = "";
            string first_found_renderer = "";
            for (int i = 0; i < c; i++) {
                if (first_found_singer.Equals("")) {
                    first_found_singer = singers1[i].VOICENAME;
                    first_found_renderer = VsqFileEx.RENDERER_DSB2;
                }
                if (singers1[i].VOICENAME.Equals(singer)) {
                    renderer = VsqFileEx.RENDERER_DSB2;
                    break;
                }
            }

            SingerConfig[] singers2 = VocaloSysUtil.getSingerConfigs(SynthesizerType.VOCALOID2);
            c = singers2.Length;
            for (int i = 0; i < c; i++) {
                if (first_found_singer.Equals("")) {
                    first_found_singer = singers2[i].VOICENAME;
                    first_found_renderer = VsqFileEx.RENDERER_DSB3;
                }
                if (singers2[i].VOICENAME.Equals(singer)) {
                    renderer = VsqFileEx.RENDERER_DSB3;
                    break;
                }
            }

            foreach (var sc in AppManager.editorConfig.UtauSingers) {
                if (first_found_singer.Equals("")) {
                    first_found_singer = sc.VOICENAME;
                    first_found_renderer = VsqFileEx.RENDERER_UTU0;
                }
                if (sc.VOICENAME.Equals(singer)) {
                    renderer = VsqFileEx.RENDERER_UTU0;
                    break;
                }
            }

            VsqFileEx vsq = new VsqFileEx(singer, 1, 4, 4, 500000);
            if (renderer.Equals("")) {
                singer = first_found_singer;
                renderer = first_found_renderer;
            }
            vsq.Track[1].getCommon().Version = renderer;
            VsqEvent item = new VsqEvent(1920, new VsqID(0));
            item.ID.LyricHandle = new LyricHandle("あ", "a");
            item.ID.setLength(480);
            item.ID.Note = note;
            item.ID.VibratoHandle = null;
            item.ID.type = VsqIDType.Anote;
            vsq.Track[1].addEvent(item);
            vsq.updateTotalClocks();
            int ms_presend = 500;
            string tempdir = Path.Combine(AppManager.getCadenciiTempDir(), AppManager.getID());
            if (!Directory.Exists(tempdir)) {
                try {
                    PortUtil.createDirectory(tempdir);
                } catch (Exception ex) {
                    Logger.write(typeof(FormGenerateKeySound) + ".GenerateSinglePhone; ex=" + ex + "\n");
                    serr.println("Program#GenerateSinglePhone; ex=" + ex);
                    return;
                }
            }
            WaveWriter ww = null;
            try {
                ww = new WaveWriter(file);
                RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[1]);
                WaveGenerator generator = VSTiDllManager.getWaveGenerator(kind);
                int sample_rate = vsq.config.SamplingRate;
                FileWaveReceiver receiver = new FileWaveReceiver(file, 1, 16, sample_rate);
                generator.setReceiver(receiver);
                generator.setGlobalConfig(AppManager.editorConfig);
#if DEBUG
                sout.println("FormGenerateKeySound#GenerateSinglePhone; sample_rate=" + sample_rate);
#endif
                generator.init(vsq, 1, 0, vsq.TotalClocks, sample_rate);
                double total_sec = vsq.getSecFromClock(vsq.TotalClocks) + 1.0;
                WorkerStateImp state = new WorkerStateImp();
                generator.begin((long)(total_sec * sample_rate), state);
            } catch (Exception ex) {
                serr.println("FormGenerateKeySound#GenerateSinglePhone; ex=" + ex);
                Logger.write(typeof(FormGenerateKeySound) + ".GenerateSinglePhone; ex=" + ex + "\n");
            } finally {
                if (ww != null) {
                    try {
                        ww.close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormGenerateKeySound) + ".GenerateSinglePhone; ex=" + ex2 + "\n");
                        serr.println("FormGenerateKeySound#GenerateSinglePhone; ex2=" + ex2);
                    }
                }
            }
        }
        #endregion

        #region UI implementation
        private void InitializeComponent()
        {
            this.btnExecute = new Button();
            this.btnCancel = new Button();
            this.comboSingingSynthSystem = new ComboBox();
            this.lblSingingSynthSystem = new Label();
            this.lblSinger = new Label();
            this.comboSinger = new ComboBox();
            this.chkIgnoreExistingWavs = new CheckBox();
            this.txtDir = new TextBox();
            this.btnBrowse = new Button();
            this.lblDir = new Label();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecute.Location = new System.Drawing.Point(286, 126);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 0;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new EventHandler(this.btnExecute_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(205, 126);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            // 
            // comboSingingSynthSystem
            // 
            this.comboSingingSynthSystem.FormattingEnabled = true;
            this.comboSingingSynthSystem.Location = new System.Drawing.Point(151, 12);
            this.comboSingingSynthSystem.Name = "comboSingingSynthSystem";
            this.comboSingingSynthSystem.Size = new System.Drawing.Size(121, 20);
            this.comboSingingSynthSystem.TabIndex = 2;
            this.comboSingingSynthSystem.SelectedIndexChanged += new EventHandler(this.comboSingingSynthSystem_SelectedIndexChanged);
            // 
            // lblSingingSynthSystem
            // 
            this.lblSingingSynthSystem.AutoSize = true;
            this.lblSingingSynthSystem.Location = new System.Drawing.Point(12, 15);
            this.lblSingingSynthSystem.Name = "lblSingingSynthSystem";
            this.lblSingingSynthSystem.Size = new System.Drawing.Size(119, 12);
            this.lblSingingSynthSystem.TabIndex = 3;
            this.lblSingingSynthSystem.Text = "Singing Synth. System";
            // 
            // lblSinger
            // 
            this.lblSinger.AutoSize = true;
            this.lblSinger.Location = new System.Drawing.Point(12, 39);
            this.lblSinger.Name = "lblSinger";
            this.lblSinger.Size = new System.Drawing.Size(37, 12);
            this.lblSinger.TabIndex = 4;
            this.lblSinger.Text = "Singer";
            // 
            // comboSinger
            // 
            this.comboSinger.FormattingEnabled = true;
            this.comboSinger.Location = new System.Drawing.Point(151, 36);
            this.comboSinger.Name = "comboSinger";
            this.comboSinger.Size = new System.Drawing.Size(121, 20);
            this.comboSinger.TabIndex = 5;
            // 
            // chkIgnoreExistingWavs
            // 
            this.chkIgnoreExistingWavs.AutoSize = true;
            this.chkIgnoreExistingWavs.Checked = true;
            this.chkIgnoreExistingWavs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIgnoreExistingWavs.Location = new System.Drawing.Point(12, 63);
            this.chkIgnoreExistingWavs.Name = "chkIgnoreExistingWavs";
            this.chkIgnoreExistingWavs.Size = new System.Drawing.Size(135, 16);
            this.chkIgnoreExistingWavs.TabIndex = 6;
            this.chkIgnoreExistingWavs.Text = "Ignore Existing WAVs";
            this.chkIgnoreExistingWavs.UseVisualStyleBackColor = true;
            // 
            // txtDir
            // 
            this.txtDir.Location = new System.Drawing.Point(94, 88);
            this.txtDir.Name = "txtDir";
            this.txtDir.Size = new System.Drawing.Size(209, 19);
            this.txtDir.TabIndex = 7;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(309, 86);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(40, 23);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
            // 
            // lblDir
            // 
            this.lblDir.AutoSize = true;
            this.lblDir.Location = new System.Drawing.Point(12, 91);
            this.lblDir.Name = "lblDir";
            this.lblDir.Size = new System.Drawing.Size(66, 12);
            this.lblDir.TabIndex = 9;
            this.lblDir.Text = "Output Path";
            // 
            // Program
            // 
            this.ClientSize = new System.Drawing.Size(373, 161);
            this.Controls.Add(this.lblDir);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtDir);
            this.Controls.Add(this.chkIgnoreExistingWavs);
            this.Controls.Add(this.comboSinger);
            this.Controls.Add(this.lblSinger);
            this.Controls.Add(this.lblSingingSynthSystem);
            this.Controls.Add(this.comboSingingSynthSystem);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnExecute);
            this.Name = "Program";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Program_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox comboSingingSynthSystem;
        private Label lblSingingSynthSystem;
        private Label lblSinger;
        private System.Windows.Forms.ComboBox comboSinger;
        private CheckBox chkIgnoreExistingWavs;
        private TextBox txtDir;
        private System.Windows.Forms.Button btnBrowse;
        private Label lblDir;

        #endregion

    }

}
