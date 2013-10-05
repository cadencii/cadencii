/*
 * FormSingerStyleConfig.cs
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
using cadencii.apputil;
using cadencii;
using cadencii.java.awt;
using cadencii.windows.forms;



namespace cadencii
{

    class FormSingerStyleConfig : Form
    {
        bool m_apply_current_track = false;

        public FormSingerStyleConfig()
        {
            InitializeComponent();

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

            registerEventHandlers();
            setResources();
            Util.applyFontRecurse(this, AppManager.editorConfig.getBaseFont());
            applyLanguage();
        }

        #region public methods
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

            groupDynamicsControl.Text = _("Dynamics Control");
            lblDecay.Text = _("Decay");
            lblDecay.Mnemonic(Keys.D);
            lblAccent.Text = _("Accent");
            lblAccent.Mnemonic(Keys.A);

            btnOK.Text = _("OK");
            btnCancel.Text = _("Cancel");
            btnApply.Text = _("Apply to current track");
            btnApply.Mnemonic(Keys.C);

            lblTemplate.Left = comboTemplate.Left - lblTemplate.Width;
            this.Text = _("Default Singer Style");
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
            btnApply.Click += new EventHandler(btnApply_Click);
            comboTemplate.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void setResources()
        {
        }
        #endregion

        #region event handlers
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
                //txtBendDepth.Text = trackBendDepth.Value + "";
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
                //txtBendLength.Text = trackBendLength.Value + "";
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
                //txtDecay.Text = trackDecay.Value + "";
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
                //txtAccent.Text = trackAccent.Value + "";
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
            this.btnApply = new Button();
            this.comboTemplate = new ComboBox();
            this.groupPitchControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBendDepth)).BeginInit();
            this.groupDynamicsControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackAccent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackDecay)).BeginInit();
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
            this.groupPitchControl.Location = new System.Drawing.Point(8, 38);
            this.groupPitchControl.Name = "groupPitchControl";
            this.groupPitchControl.Size = new System.Drawing.Size(367, 130);
            this.groupPitchControl.TabIndex = 0;
            this.groupPitchControl.TabStop = false;
            this.groupPitchControl.Text = "Pitch Control";
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
            this.groupDynamicsControl.Location = new System.Drawing.Point(8, 174);
            this.groupDynamicsControl.Name = "groupDynamicsControl";
            this.groupDynamicsControl.Size = new System.Drawing.Size(367, 74);
            this.groupDynamicsControl.TabIndex = 1;
            this.groupDynamicsControl.TabStop = false;
            this.groupDynamicsControl.Text = "Dynamics Control";
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
            this.lblTemplate.Location = new System.Drawing.Point(163, 15);
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size(67, 12);
            this.lblTemplate.TabIndex = 2;
            this.lblTemplate.Text = "Template(&T)";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(290, 259);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(203, 259);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(78, 23);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(8, 259);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(153, 23);
            this.btnApply.TabIndex = 14;
            this.btnApply.Text = "Apply to current track(&C)";
            this.btnApply.UseVisualStyleBackColor = true;
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
            this.comboTemplate.Location = new System.Drawing.Point(253, 12);
            this.comboTemplate.Name = "comboTemplate";
            this.comboTemplate.Size = new System.Drawing.Size(121, 20);
            this.comboTemplate.TabIndex = 0;
            this.comboTemplate.Text = "[Select a template]";
            // 
            // FormSingerStyleConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 308);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lblTemplate);
            this.Controls.Add(this.groupDynamicsControl);
            this.Controls.Add(this.groupPitchControl);
            this.Controls.Add(this.comboTemplate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(6000, 6000);
            this.MinimizeBox = false;
            this.Name = "FormSingerStyleConfig";
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
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ComboBox comboTemplate;
        #endregion
        #endregion

    }

}
