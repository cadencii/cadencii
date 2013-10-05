/*
 * FormNoteExpressionConfig.cs
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
using System.Drawing;
using System.Windows.Forms;
using cadencii.apputil;
using cadencii.vsq;
using cadencii.windows.forms;
using cadencii.java.util;

namespace cadencii
{
    public class FormNoteExpressionConfig : Form
    {
        bool m_apply_current_track = false;
        NoteHeadHandle m_note_head_handle = null;

        public FormNoteExpressionConfig(SynthesizerType type, NoteHeadHandle note_head_handle)
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            Util.applyFontRecurse(this, AppManager.editorConfig.getBaseFont());
            applyLanguage();

            if (note_head_handle != null) {
                m_note_head_handle = (NoteHeadHandle)note_head_handle.clone();
            }

            if (type == SynthesizerType.VOCALOID1) {
                flowLayoutPanel.Controls.Remove(groupDynamicsControl);
                flowLayoutPanel.Controls.Remove(panelVocaloid2Template);
                flowLayoutPanel.Controls.Remove(groupPitchControl);
            } else {
                flowLayoutPanel.Controls.Remove(groupAttack);
            }

            //comboAttackTemplateを更新
            NoteHeadHandle empty = new NoteHeadHandle();
            comboAttackTemplate.Items.Clear();
            empty.IconID = "$01010000";
            empty.setCaption("[Non Attack]");
            comboAttackTemplate.Items.Add(empty);
            comboAttackTemplate.SelectedItem = empty;
            string icon_id = "";
            if (m_note_head_handle != null) {
                icon_id = m_note_head_handle.IconID;
                txtDuration.Text = m_note_head_handle.getDuration() + "";
                txtDepth.Text = m_note_head_handle.getDepth() + "";
            } else {
                txtDuration.Enabled = false;
                txtDepth.Enabled = false;
                trackDuration.Enabled = false;
                trackDepth.Enabled = false;
            }
            foreach (var item in VocaloSysUtil.attackConfigIterator(SynthesizerType.VOCALOID1)) {
                comboAttackTemplate.Items.Add(item);
                if (item.IconID.Equals(icon_id)) {
                    comboAttackTemplate.SelectedItem = comboAttackTemplate.Items[comboAttackTemplate.Items.Count - 1];
                }
            }
            comboAttackTemplate.SelectedIndexChanged += new EventHandler(comboAttackTemplate_SelectedIndexChanged);

            comboTemplate.Items.Clear();
            string[] strs = new string[]{
                "[Select a template]",
                "normal",
                "accent",
                "strong accent",
                "legato",
                "slow legate",
            };
            for (int i = 0; i < strs.Length; i++) {
                comboTemplate.Items.Add(strs[i]);
            }

            Size current_size = this.ClientSize;
            this.ClientSize = new Size(current_size.Width, flowLayoutPanel.ClientSize.Height + flowLayoutPanel.Top * 2);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        #region public methods
        public NoteHeadHandle getEditedNoteHeadHandle()
        {
            return m_note_head_handle;
        }

        public void applyLanguage()
        {
            lblTemplate.Text = _("Template");
            lblTemplate.Mnemonic(Keys.T);
            groupPitchControl.Text = _("Pitch Control");
            lblBendDepth.Text = _("Bend Depth");
            lblBendDepth.Mnemonic(Keys.B);
            lblBendLength.Text = _("Bend Length");
            lblBendLength.Mnemonic(Keys.L);
            chkUpPortamento.Text = _("Add portamento in rising movement");
            chkUpPortamento.Mnemonic(Keys.R);
            chkDownPortamento.Text = _("Add portamento in falling movement");
            chkDownPortamento.Mnemonic(Keys.F);

            groupAttack.Text = _("Attack Control (VOCALOID1)");
            groupDynamicsControl.Text = _("Dynamics Control (VOCALOID2)");
            lblDecay.Text = _("Decay");
            lblDecay.Mnemonic(Keys.D);
            lblAccent.Text = _("Accent");
            lblAccent.Mnemonic(Keys.A);

            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");

            lblTemplate.Left = comboTemplate.Left - lblTemplate.Width;
            this.Text = _("Expression control property");
        }

        public int getPMBendDepth()
        {
            return trackBendDepth.Value;
        }

        public void setPMBendDepth(int value)
        {
            trackBendDepth.Value = value;
            txtBendDepth.Text = value + "";
        }

        public int getPMBendLength()
        {
            return trackBendLength.Value;
        }

        public void setPMBendLength(int value)
        {
            trackBendLength.Value = value;
            txtBendLength.Text = value + "";
        }

        public int getPMbPortamentoUse()
        {
            int ret = 0;
            if (chkUpPortamento.Checked) {
                ret += 1;
            }
            if (chkDownPortamento.Checked) {
                ret += 2;
            }
            return ret;
        }

        public void setPMbPortamentoUse(int value)
        {
            if (value % 2 == 1) {
                chkUpPortamento.Checked = true;
            } else {
                chkUpPortamento.Checked = false;
            }
            if (value >= 2) {
                chkDownPortamento.Checked = true;
            } else {
                chkDownPortamento.Checked = false;
            }
        }

        public int getDEMdecGainRate()
        {
            return trackDecay.Value;
        }

        public void setDEMdecGainRate(int value)
        {
            trackDecay.Value = value;
            txtDecay.Text = value + "";
        }

        public int getDEMaccent()
        {
            return trackAccent.Value;
        }

        public void setDEMaccent(int value)
        {
            trackAccent.Value = value;
            txtAccent.Text = value + "";
        }

        public bool getApplyCurrentTrack()
        {
            return m_apply_current_track;
        }
        #endregion

        #region helper methods
        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void registerEventHandlers()
        {
            txtBendLength.TextChanged += new EventHandler(txtBendLength_TextChanged);
            txtBendDepth.TextChanged += new EventHandler(txtBendDepth_TextChanged);
            trackBendLength.ValueChanged += new EventHandler(trackBendLength_Scroll);
            trackBendDepth.ValueChanged += new EventHandler(trackBendDepth_Scroll);
            txtAccent.TextChanged += new EventHandler(txtAccent_TextChanged);
            txtDecay.TextChanged += new EventHandler(txtDecay_TextChanged);
            trackAccent.ValueChanged += new EventHandler(trackAccent_Scroll);
            trackDecay.ValueChanged += new EventHandler(trackDecay_Scroll);
            btnOK.Click += new EventHandler(btnOK_Click);
            comboTemplate.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            txtDepth.TextChanged += new EventHandler(txtDepth_TextChanged);
            txtDuration.TextChanged += new EventHandler(txtDuration_TextChanged);
            trackDepth.ValueChanged += new EventHandler(trackDepth_Scroll);
            trackDuration.ValueChanged += new EventHandler(trackDuration_Scroll);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
        public void comboAttackTemplate_SelectedIndexChanged(Object sender, EventArgs e)
        {
            int index = comboAttackTemplate.SelectedIndex;
            if (index < 0) {
                return;
            }
            if (index == 0) {
                m_note_head_handle = null;
                txtDuration.Enabled = false;
                trackDuration.Enabled = false;
                txtDepth.Enabled = false;
                trackDepth.Enabled = false;
                return;
            }
            txtDuration.Enabled = true;
            trackDuration.Enabled = true;
            txtDepth.Enabled = true;
            trackDepth.Enabled = true;
            NoteHeadHandle aconfig = (NoteHeadHandle)comboAttackTemplate.SelectedItem;
            if (m_note_head_handle == null) {
                txtDuration.Text = aconfig.getDuration() + "";
                txtDepth.Text = aconfig.getDepth() + "";
            }
            m_note_head_handle = (NoteHeadHandle)aconfig.clone();
            m_note_head_handle.setDuration(trackDuration.Value);
            m_note_head_handle.setDepth(trackDepth.Value);
        }

        public void trackBendDepth_Scroll(Object sender, EventArgs e)
        {
            string s = trackBendDepth.Value + "";
            if (s != txtBendDepth.Text) {
                txtBendDepth.Text = s;
            }
        }

        public void txtBendDepth_TextChanged(Object sender, EventArgs e)
        {
            try {
                int draft = int.Parse(txtBendDepth.Text);
                if (draft < trackBendDepth.Minimum) {
                    draft = trackBendDepth.Minimum;
                    txtBendDepth.Text = draft + "";
                } else if (trackBendDepth.Maximum < draft) {
                    draft = trackBendDepth.Maximum;
                    txtBendDepth.Text = draft + "";
                }
                if (draft != trackBendDepth.Value) {
                    trackBendDepth.Value = draft;
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormNoteExpressionConfig) + ".txtBendDepth_TextChanged; ex=" + ex + "\n");
            }
        }

        public void trackBendLength_Scroll(Object sender, EventArgs e)
        {
            string s = trackBendLength.Value + "";
            if (s != txtBendLength.Text) {
                txtBendLength.Text = s;
            }
        }

        public void txtBendLength_TextChanged(Object sender, EventArgs e)
        {
            try {
                int draft = int.Parse(txtBendLength.Text);
                if (draft < trackBendLength.Minimum) {
                    draft = trackBendLength.Minimum;
                    txtBendLength.Text = draft + "";
                } else if (trackBendLength.Maximum < draft) {
                    draft = trackBendLength.Maximum;
                    txtBendLength.Text = draft + "";
                }
                if (draft != trackBendLength.Value) {
                    trackBendLength.Value = draft;
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormNoteExpressionConfig) + ".txtBendLength_TextChanged; ex=" + ex + "\n");
            }
        }

        public void trackDecay_Scroll(Object sender, EventArgs e)
        {
            string s = trackDecay.Value + "";
            if (s != txtDecay.Text) {
                txtDecay.Text = s;
            }
        }

        public void txtDecay_TextChanged(Object sender, EventArgs e)
        {
            try {
                int draft = int.Parse(txtDecay.Text);
                if (draft < trackDecay.Minimum) {
                    draft = trackDecay.Minimum;
                    txtDecay.Text = draft + "";
                } else if (trackDecay.Maximum < draft) {
                    draft = trackDecay.Maximum;
                    txtDecay.Text = draft + "";
                }
                if (draft != trackDecay.Value) {
                    trackDecay.Value = draft;
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormNoteExpressionConfig) + ".txtDecay_TextChanged; ex=" + ex + "\n");
            }
        }

        public void trackAccent_Scroll(Object sender, EventArgs e)
        {
            string s = trackAccent.Value + "";
            if (s != txtAccent.Text) {
                txtAccent.Text = s;
            }
        }

        public void txtAccent_TextChanged(Object sender, EventArgs e)
        {
            try {
                int draft = int.Parse(txtAccent.Text);
                if (draft < trackAccent.Minimum) {
                    draft = trackAccent.Minimum;
                    txtAccent.Text = draft + "";
                } else if (trackAccent.Maximum < draft) {
                    draft = trackAccent.Maximum;
                    txtAccent.Text = draft + "";
                }
                if (draft != trackAccent.Value) {
                    trackAccent.Value = draft;
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormNoteExpressionConfig) + ".txtAccent_TextChanged; ex=" + ex + "\n");
            }
        }

        public void btnOK_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public void comboBox1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            int index = comboTemplate.SelectedIndex - 1;
            if (index < 0 || 4 < index) {
                return;
            }
            int[] pm_bend_depth = new int[] { 8, 8, 8, 20, 20 };
            int[] pm_bend_length = new int[] { 0, 0, 0, 0, 0 };
            int[] pmb_portamento_use = new int[] { 0, 0, 0, 3, 3 };
            int[] dem_dec_gain_rate = new int[] { 50, 50, 70, 50, 50 };
            int[] dem_accent = new int[] { 50, 68, 80, 42, 25 };
            setPMBendDepth(pm_bend_depth[index]);
            setPMBendLength(pm_bend_length[index]);
            setPMbPortamentoUse(pmb_portamento_use[index]);
            setDEMdecGainRate(dem_dec_gain_rate[index]);
            setDEMaccent(dem_accent[index]);
        }

        public void btnApply_Click(Object sender, EventArgs e)
        {
            if (AppManager.showMessageBox(_("Would you like to change singer style for all events?"),
                                  FormMain._APP_NAME,
                                  cadencii.windows.forms.Utility.MSGBOX_YES_NO_OPTION,
                                  cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE) == DialogResult.Yes) {
                m_apply_current_track = true;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        public void trackDuration_Scroll(Object sender, EventArgs e)
        {
            string s = trackDuration.Value + "";
            if (s != txtDuration.Text) {
                txtDuration.Text = s;
            }
            if (m_note_head_handle != null) {
                m_note_head_handle.setDuration(trackDuration.Value);
            }
        }

        public void trackDepth_Scroll(Object sender, EventArgs e)
        {
            string s = trackDepth.Value + "";
            if (s != txtDepth.Text) {
                txtDepth.Text = s;
            }
            if (m_note_head_handle != null) {
                m_note_head_handle.setDepth(trackDepth.Value);
            }
        }

        public void txtDuration_TextChanged(Object sender, EventArgs e)
        {
            try {
                int draft = int.Parse(txtDuration.Text);
                if (draft < trackDuration.Minimum) {
                    draft = trackDuration.Minimum;
                    txtDuration.Text = draft + "";
                } else if (trackDuration.Maximum < draft) {
                    draft = trackDuration.Maximum;
                    txtDuration.Text = draft + "";
                }
                if (draft != trackDuration.Value) {
                    trackDuration.Value = draft;
                }
                if (m_note_head_handle != null) {
                    m_note_head_handle.setDuration(draft);
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormNoteExpressionConfig) + ".txtDuration_TextChanged; ex=" + ex + "\n");
            }
        }

        public void txtDepth_TextChanged(Object sender, EventArgs e)
        {
            try {
                int draft = int.Parse(txtDepth.Text);
                if (draft < trackDepth.Minimum) {
                    draft = trackDepth.Minimum;
                    txtDepth.Text = draft + "";
                } else if (trackDepth.Maximum < draft) {
                    draft = trackDepth.Maximum;
                    txtDepth.Text = draft + "";
                }
                if (draft != trackDepth.Value) {
                    trackDepth.Value = draft;
                }
                if (m_note_head_handle != null) {
                    m_note_head_handle.setDepth(trackDepth.Value);
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormNoteExpressionConfig) + ".txtDepth_TextChanged; ex=" + ex + "\n");
            }
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        #endregion

        #region UI implementation
        #region UI Impl for C#
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupPitchControl = new GroupBox();
            this.label5 = new Label();
            this.label4 = new Label();
            this.txtBendLength = new cadencii.NumberTextBox();
            this.txtBendDepth = new cadencii.NumberTextBox();
            this.trackBendLength = new TrackBar();
            this.trackBendDepth = new TrackBar();
            this.chkDownPortamento = new CheckBox();
            this.chkUpPortamento = new CheckBox();
            this.lblBendLength = new Label();
            this.lblBendDepth = new Label();
            this.groupDynamicsControl = new GroupBox();
            this.label7 = new Label();
            this.label6 = new Label();
            this.txtAccent = new cadencii.NumberTextBox();
            this.txtDecay = new cadencii.NumberTextBox();
            this.trackAccent = new TrackBar();
            this.trackDecay = new TrackBar();
            this.lblAccent = new Label();
            this.lblDecay = new Label();
            this.lblTemplate = new Label();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.comboTemplate = new ComboBox();
            this.groupAttack = new GroupBox();
            this.lblAttackTemplate = new Label();
            this.comboAttackTemplate = new ComboBox();
            this.txtDepth = new cadencii.NumberTextBox();
            this.txtDuration = new cadencii.NumberTextBox();
            this.trackDepth = new TrackBar();
            this.trackDuration = new TrackBar();
            this.lblDepth = new Label();
            this.lblDuration = new Label();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panelVocaloid2Template = new UserControl();
            this.panelButtons = new UserControl();
            this.groupPitchControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendDepth)).BeginInit();
            this.groupDynamicsControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackAccent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDecay)).BeginInit();
            this.groupAttack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDuration)).BeginInit();
            this.flowLayoutPanel.SuspendLayout();
            this.panelVocaloid2Template.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPitchControl
            // 
            this.groupPitchControl.Controls.Add(this.label5);
            this.groupPitchControl.Controls.Add(this.label4);
            this.groupPitchControl.Controls.Add(this.txtBendLength);
            this.groupPitchControl.Controls.Add(this.txtBendDepth);
            this.groupPitchControl.Controls.Add(this.trackBendLength);
            this.groupPitchControl.Controls.Add(this.trackBendDepth);
            this.groupPitchControl.Controls.Add(this.chkDownPortamento);
            this.groupPitchControl.Controls.Add(this.chkUpPortamento);
            this.groupPitchControl.Controls.Add(this.lblBendLength);
            this.groupPitchControl.Controls.Add(this.lblBendDepth);
            this.groupPitchControl.Location = new System.Drawing.Point(3, 38);
            this.groupPitchControl.Name = "groupPitchControl";
            this.groupPitchControl.Size = new System.Drawing.Size(367, 130);
            this.groupPitchControl.TabIndex = 0;
            this.groupPitchControl.TabStop = false;
            this.groupPitchControl.Text = "Pitch Control (VOCALOID2)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(345, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "%";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(345, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "%";
            // 
            // txtBendLength
            // 
            this.txtBendLength.BackColor = System.Drawing.SystemColors.Window;
            this.txtBendLength.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtBendLength.Location = new System.Drawing.Point(300, 39);
            this.txtBendLength.Name = "txtBendLength";
            this.txtBendLength.Size = new System.Drawing.Size(39, 19);
            this.txtBendLength.TabIndex = 5;
            this.txtBendLength.Text = "0";
            this.txtBendLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBendLength.Type = cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtBendDepth
            // 
            this.txtBendDepth.BackColor = System.Drawing.SystemColors.Window;
            this.txtBendDepth.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtBendDepth.Location = new System.Drawing.Point(300, 13);
            this.txtBendDepth.Name = "txtBendDepth";
            this.txtBendDepth.Size = new System.Drawing.Size(39, 19);
            this.txtBendDepth.TabIndex = 2;
            this.txtBendDepth.Text = "8";
            this.txtBendDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBendDepth.Type = cadencii.NumberTextBox.ValueType.Integer;
            // 
            // trackBendLength
            // 
            this.trackBendLength.AutoSize = false;
            this.trackBendLength.Location = new System.Drawing.Point(138, 40);
            this.trackBendLength.Maximum = 100;
            this.trackBendLength.Name = "trackBendLength";
            this.trackBendLength.Size = new System.Drawing.Size(156, 18);
            this.trackBendLength.TabIndex = 4;
            this.trackBendLength.TickFrequency = 10;
            // 
            // trackBendDepth
            // 
            this.trackBendDepth.AutoSize = false;
            this.trackBendDepth.Location = new System.Drawing.Point(138, 14);
            this.trackBendDepth.Maximum = 100;
            this.trackBendDepth.Name = "trackBendDepth";
            this.trackBendDepth.Size = new System.Drawing.Size(156, 18);
            this.trackBendDepth.TabIndex = 1;
            this.trackBendDepth.TickFrequency = 10;
            this.trackBendDepth.Value = 8;
            // 
            // chkDownPortamento
            // 
            this.chkDownPortamento.AutoSize = true;
            this.chkDownPortamento.Location = new System.Drawing.Point(20, 96);
            this.chkDownPortamento.Name = "chkDownPortamento";
            this.chkDownPortamento.Size = new System.Drawing.Size(224, 16);
            this.chkDownPortamento.TabIndex = 7;
            this.chkDownPortamento.Text = "Add portamento in falling movement(&F)";
            this.chkDownPortamento.UseVisualStyleBackColor = true;
            // 
            // chkUpPortamento
            // 
            this.chkUpPortamento.AutoSize = true;
            this.chkUpPortamento.Location = new System.Drawing.Point(20, 71);
            this.chkUpPortamento.Name = "chkUpPortamento";
            this.chkUpPortamento.Size = new System.Drawing.Size(222, 16);
            this.chkUpPortamento.TabIndex = 6;
            this.chkUpPortamento.Text = "Add portamento in rising movement(&R)";
            this.chkUpPortamento.UseVisualStyleBackColor = true;
            // 
            // lblBendLength
            // 
            this.lblBendLength.AutoSize = true;
            this.lblBendLength.Location = new System.Drawing.Point(20, 46);
            this.lblBendLength.Name = "lblBendLength";
            this.lblBendLength.Size = new System.Drawing.Size(83, 12);
            this.lblBendLength.TabIndex = 3;
            this.lblBendLength.Text = "Bend Length(&L)";
            // 
            // lblBendDepth
            // 
            this.lblBendDepth.AutoSize = true;
            this.lblBendDepth.Location = new System.Drawing.Point(20, 20);
            this.lblBendDepth.Name = "lblBendDepth";
            this.lblBendDepth.Size = new System.Drawing.Size(81, 12);
            this.lblBendDepth.TabIndex = 0;
            this.lblBendDepth.Text = "Bend Depth(&B)";
            // 
            // groupDynamicsControl
            // 
            this.groupDynamicsControl.Controls.Add(this.label7);
            this.groupDynamicsControl.Controls.Add(this.label6);
            this.groupDynamicsControl.Controls.Add(this.txtAccent);
            this.groupDynamicsControl.Controls.Add(this.txtDecay);
            this.groupDynamicsControl.Controls.Add(this.trackAccent);
            this.groupDynamicsControl.Controls.Add(this.trackDecay);
            this.groupDynamicsControl.Controls.Add(this.lblAccent);
            this.groupDynamicsControl.Controls.Add(this.lblDecay);
            this.groupDynamicsControl.Location = new System.Drawing.Point(3, 174);
            this.groupDynamicsControl.Name = "groupDynamicsControl";
            this.groupDynamicsControl.Size = new System.Drawing.Size(367, 74);
            this.groupDynamicsControl.TabIndex = 1;
            this.groupDynamicsControl.TabStop = false;
            this.groupDynamicsControl.Text = "Dynamics Control (VOCALOID2)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(345, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "%";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(345, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "%";
            // 
            // txtAccent
            // 
            this.txtAccent.BackColor = System.Drawing.SystemColors.Window;
            this.txtAccent.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtAccent.Location = new System.Drawing.Point(300, 43);
            this.txtAccent.Name = "txtAccent";
            this.txtAccent.Size = new System.Drawing.Size(39, 19);
            this.txtAccent.TabIndex = 13;
            this.txtAccent.Text = "50";
            this.txtAccent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAccent.Type = cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtDecay
            // 
            this.txtDecay.BackColor = System.Drawing.SystemColors.Window;
            this.txtDecay.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDecay.Location = new System.Drawing.Point(300, 17);
            this.txtDecay.Name = "txtDecay";
            this.txtDecay.Size = new System.Drawing.Size(39, 19);
            this.txtDecay.TabIndex = 10;
            this.txtDecay.Text = "50";
            this.txtDecay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDecay.Type = cadencii.NumberTextBox.ValueType.Integer;
            // 
            // trackAccent
            // 
            this.trackAccent.AutoSize = false;
            this.trackAccent.Location = new System.Drawing.Point(138, 44);
            this.trackAccent.Maximum = 100;
            this.trackAccent.Name = "trackAccent";
            this.trackAccent.Size = new System.Drawing.Size(156, 18);
            this.trackAccent.TabIndex = 12;
            this.trackAccent.TickFrequency = 10;
            this.trackAccent.Value = 50;
            // 
            // trackDecay
            // 
            this.trackDecay.AutoSize = false;
            this.trackDecay.Location = new System.Drawing.Point(138, 18);
            this.trackDecay.Maximum = 100;
            this.trackDecay.Name = "trackDecay";
            this.trackDecay.Size = new System.Drawing.Size(156, 18);
            this.trackDecay.TabIndex = 9;
            this.trackDecay.TickFrequency = 10;
            this.trackDecay.Value = 50;
            // 
            // lblAccent
            // 
            this.lblAccent.AutoSize = true;
            this.lblAccent.Location = new System.Drawing.Point(18, 50);
            this.lblAccent.Name = "lblAccent";
            this.lblAccent.Size = new System.Drawing.Size(57, 12);
            this.lblAccent.TabIndex = 11;
            this.lblAccent.Text = "Accent(&A)";
            // 
            // lblDecay
            // 
            this.lblDecay.AutoSize = true;
            this.lblDecay.Location = new System.Drawing.Point(18, 24);
            this.lblDecay.Name = "lblDecay";
            this.lblDecay.Size = new System.Drawing.Size(53, 12);
            this.lblDecay.TabIndex = 8;
            this.lblDecay.Text = "Decay(&D)";
            // 
            // lblTemplate
            // 
            this.lblTemplate.AutoSize = true;
            this.lblTemplate.Location = new System.Drawing.Point(165, 6);
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size(67, 12);
            this.lblTemplate.TabIndex = 2;
            this.lblTemplate.Text = "Template(&T)";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(285, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(198, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(78, 23);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // comboTemplate
            // 
            this.comboTemplate.FormattingEnabled = true;
            this.comboTemplate.Items.AddRange(new Object[] {
            "[Select a template]",
            "normal",
            "accent",
            "strong accent",
            "legato",
            "slow legato"});
            this.comboTemplate.Location = new System.Drawing.Point(242, 3);
            this.comboTemplate.Name = "comboTemplate";
            this.comboTemplate.Size = new System.Drawing.Size(121, 20);
            this.comboTemplate.TabIndex = 0;
            this.comboTemplate.Text = "[Select a template]";
            // 
            // groupAttack
            // 
            this.groupAttack.Controls.Add(this.lblAttackTemplate);
            this.groupAttack.Controls.Add(this.comboAttackTemplate);
            this.groupAttack.Controls.Add(this.txtDepth);
            this.groupAttack.Controls.Add(this.txtDuration);
            this.groupAttack.Controls.Add(this.trackDepth);
            this.groupAttack.Controls.Add(this.trackDuration);
            this.groupAttack.Controls.Add(this.lblDepth);
            this.groupAttack.Controls.Add(this.lblDuration);
            this.groupAttack.Location = new System.Drawing.Point(3, 254);
            this.groupAttack.Name = "groupAttack";
            this.groupAttack.Size = new System.Drawing.Size(367, 107);
            this.groupAttack.TabIndex = 17;
            this.groupAttack.TabStop = false;
            this.groupAttack.Text = "Attack (VOCALOID1)";
            // 
            // lblAttackTemplate
            // 
            this.lblAttackTemplate.AutoSize = true;
            this.lblAttackTemplate.Location = new System.Drawing.Point(18, 23);
            this.lblAttackTemplate.Name = "lblAttackTemplate";
            this.lblAttackTemplate.Size = new System.Drawing.Size(105, 12);
            this.lblAttackTemplate.TabIndex = 2;
            this.lblAttackTemplate.Text = "Attack Variation(&V)";
            // 
            // comboAttackTemplate
            // 
            this.comboAttackTemplate.FormattingEnabled = true;
            this.comboAttackTemplate.Location = new System.Drawing.Point(143, 20);
            this.comboAttackTemplate.Name = "comboAttackTemplate";
            this.comboAttackTemplate.Size = new System.Drawing.Size(121, 20);
            this.comboAttackTemplate.TabIndex = 0;
            this.comboAttackTemplate.Text = "[Non Attack]";
            // 
            // txtDepth
            // 
            this.txtDepth.BackColor = System.Drawing.SystemColors.Window;
            this.txtDepth.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDepth.Location = new System.Drawing.Point(300, 72);
            this.txtDepth.Name = "txtDepth";
            this.txtDepth.Size = new System.Drawing.Size(39, 19);
            this.txtDepth.TabIndex = 13;
            this.txtDepth.Text = "64";
            this.txtDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDepth.Type = cadencii.NumberTextBox.ValueType.Integer;
            // 
            // txtDuration
            // 
            this.txtDuration.BackColor = System.Drawing.SystemColors.Window;
            this.txtDuration.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDuration.Location = new System.Drawing.Point(300, 46);
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.Size = new System.Drawing.Size(39, 19);
            this.txtDuration.TabIndex = 10;
            this.txtDuration.Text = "64";
            this.txtDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDuration.Type = cadencii.NumberTextBox.ValueType.Integer;
            // 
            // trackDepth
            // 
            this.trackDepth.AutoSize = false;
            this.trackDepth.Location = new System.Drawing.Point(138, 69);
            this.trackDepth.Maximum = 127;
            this.trackDepth.Name = "trackDepth";
            this.trackDepth.Size = new System.Drawing.Size(156, 18);
            this.trackDepth.TabIndex = 12;
            this.trackDepth.TickFrequency = 10;
            this.trackDepth.Value = 64;
            // 
            // trackDuration
            // 
            this.trackDuration.AutoSize = false;
            this.trackDuration.Location = new System.Drawing.Point(138, 43);
            this.trackDuration.Maximum = 127;
            this.trackDuration.Name = "trackDuration";
            this.trackDuration.Size = new System.Drawing.Size(156, 18);
            this.trackDuration.TabIndex = 9;
            this.trackDuration.TickFrequency = 10;
            this.trackDuration.Value = 64;
            // 
            // lblDepth
            // 
            this.lblDepth.AutoSize = true;
            this.lblDepth.Location = new System.Drawing.Point(18, 75);
            this.lblDepth.Name = "lblDepth";
            this.lblDepth.Size = new System.Drawing.Size(51, 12);
            this.lblDepth.TabIndex = 11;
            this.lblDepth.Text = "Depth(&D)";
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(18, 49);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(64, 12);
            this.lblDuration.TabIndex = 8;
            this.lblDuration.Text = "Duration(&D)";
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel.Controls.Add(this.panelVocaloid2Template);
            this.flowLayoutPanel.Controls.Add(this.groupPitchControl);
            this.flowLayoutPanel.Controls.Add(this.groupDynamicsControl);
            this.flowLayoutPanel.Controls.Add(this.groupAttack);
            this.flowLayoutPanel.Controls.Add(this.panelButtons);
            this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel.Location = new System.Drawing.Point(9, 9);
            this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(373, 418);
            this.flowLayoutPanel.TabIndex = 18;
            // 
            // panelVocaloid2Template
            // 
            this.panelVocaloid2Template.Controls.Add(this.comboTemplate);
            this.panelVocaloid2Template.Controls.Add(this.lblTemplate);
            this.panelVocaloid2Template.Location = new System.Drawing.Point(3, 3);
            this.panelVocaloid2Template.Name = "panelVocaloid2Template";
            this.panelVocaloid2Template.Size = new System.Drawing.Size(367, 29);
            this.panelVocaloid2Template.TabIndex = 19;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Controls.Add(this.btnOK);
            this.panelButtons.Location = new System.Drawing.Point(3, 367);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(367, 48);
            this.panelButtons.TabIndex = 19;
            // 
            // FormNoteExpressionConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(390, 514);
            this.Controls.Add(this.flowLayoutPanel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(6000, 6000);
            this.MinimizeBox = false;
            this.Name = "FormNoteExpressionConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Default Singer Style";
            this.groupPitchControl.ResumeLayout(false);
            this.groupPitchControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendDepth)).EndInit();
            this.groupDynamicsControl.ResumeLayout(false);
            this.groupDynamicsControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackAccent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDecay)).EndInit();
            this.groupAttack.ResumeLayout(false);
            this.groupAttack.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDuration)).EndInit();
            this.flowLayoutPanel.ResumeLayout(false);
            this.panelVocaloid2Template.ResumeLayout(false);
            this.panelVocaloid2Template.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox groupPitchControl;
        private GroupBox groupDynamicsControl;
        private Label lblBendDepth;
        private Label lblTemplate;
        private Label lblBendLength;
        private CheckBox chkDownPortamento;
        private CheckBox chkUpPortamento;
        private TrackBar trackBendDepth;
        private TrackBar trackBendLength;
        private TrackBar trackAccent;
        private TrackBar trackDecay;
        private Label lblAccent;
        private Label lblDecay;
        private NumberTextBox txtBendLength;
        private NumberTextBox txtBendDepth;
        private NumberTextBox txtAccent;
        private NumberTextBox txtDecay;
        private Label label5;
        private Label label4;
        private Label label7;
        private Label label6;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private ComboBox comboTemplate;
        private GroupBox groupAttack;
        private NumberTextBox txtDepth;
        private NumberTextBox txtDuration;
        private TrackBar trackDepth;
        private TrackBar trackDuration;
        private Label lblDepth;
        private Label lblDuration;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private UserControl panelButtons;
        private UserControl panelVocaloid2Template;
        private ComboBox comboAttackTemplate;
        private Label lblAttackTemplate;
        #endregion
        #endregion

    }

}
