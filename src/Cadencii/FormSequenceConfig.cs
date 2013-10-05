/*
 * FormSequenceConfig.cs
 * Copyright © 2011 kbinani
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
using cadencii.java.awt;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.media;
using cadencii.vsq;
using cadencii.windows.forms;



namespace cadencii
{

    class FormSequenceConfig : System.Windows.Forms.Form
    {
        public FormSequenceConfig()
        {
            InitializeComponent();
            applyLanguage();

            // wave channel
            comboChannel.Items.Clear();
            comboChannel.Items.Add(_("Monoral"));
            comboChannel.Items.Add(_("Stereo"));

            // sample rate
            comboSampleRate.Items.Clear();
            comboSampleRate.Items.Add("44100");
            comboSampleRate.Items.Add("48000");
            comboSampleRate.Items.Add("96000");
            comboSampleRate.SelectedIndex = 0;

            // pre-measure
            comboPreMeasure.Items.Clear();
            for (int i = AppManager.MIN_PRE_MEASURE; i <= AppManager.MAX_PRE_MEASURE; i++) {
                comboPreMeasure.Items.Add(i);
            }

            registerEventHandlers();
            setResources();
            Util.applyFontRecurse(this, AppManager.editorConfig.getBaseFont());
        }

        #region public methods
        public void applyLanguage()
        {
            this.Text = _("Sequence config");
            btnCancel.Text = _("Cancel");
            btnOK.Text = _("OK");

            groupWaveFileOutput.Text = _("Wave File Output");
            lblChannel.Text = _("Channel");
            lblChannel.Mnemonic(Keys.C);
            labelSampleRate.Text = _("Sample rate");
            labelSampleRate.Mnemonic(Keys.S);
            radioMasterTrack.Text = _("Master Track");
            radioCurrentTrack.Text = _("Current Track");
            labelSampleRate.Text = _("Sample rate");

            int current_index = comboChannel.SelectedIndex;
            comboChannel.Items.Clear();
            comboChannel.Items.Add(_("Monoral"));
            comboChannel.Items.Add(_("Stereo"));
            comboChannel.SelectedIndex = current_index;

            groupSequence.Text = _("Sequence");
            labelPreMeasure.Text = _("Pre-measure");
        }

        /// <summary>
        /// プリメジャーの設定値を取得します
        /// </summary>
        /// <returns></returns>
        public int getPreMeasure()
        {
            int indx = comboPreMeasure.SelectedIndex;
            int ret = 1;
            if (indx >= 0) {
                ret = AppManager.MIN_PRE_MEASURE + indx;
            } else {
                string s = comboPreMeasure.Text;
                try {
                    ret = int.Parse(s);
                } catch (Exception ex) {
                    ret = AppManager.MIN_PRE_MEASURE;
                }
            }
            if (ret < AppManager.MIN_PRE_MEASURE) {
                ret = AppManager.MIN_PRE_MEASURE;
            }
            if (AppManager.MAX_PRE_MEASURE < ret) {
                ret = AppManager.MAX_PRE_MEASURE;
            }
            return ret;
        }

        /// <summary>
        /// プリメジャーの設定値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setPreMeasure(int value)
        {
            int indx = value - AppManager.MIN_PRE_MEASURE;
            if (indx < 0) {
                indx = 0;
            }
            if (comboPreMeasure.Items.Count <= indx) {
                indx = comboPreMeasure.Items.Count - 1;
            }
            comboPreMeasure.SelectedIndex = indx;
        }

        /// <summary>
        /// サンプリングレートの設定値を取得します
        /// </summary>
        /// <returns></returns>
        public int getSampleRate()
        {
            int index = comboSampleRate.SelectedIndex;
            string s = "44100";
            if (index >= 0) {
                s = (string)comboSampleRate.Items[index];
            } else {
                s = comboSampleRate.Text;
            }
            int ret = 44100;
            try {
                ret = int.Parse(s);
            } catch (Exception ex) {
                ret = 44100;
            }
            return ret;
        }

        /// <summary>
        /// サンプリングレートの設定値を設定します
        /// </summary>
        /// <param name="value"></param>
        public void setSampleRate(int value)
        {
            comboSampleRate.SelectedIndex = 0;
            for (int i = 0; i < comboSampleRate.Items.Count; i++) {
                string s = (string)comboSampleRate.Items[i];
                int rate = 0;
                try {
                    rate = int.Parse(s);
                } catch (Exception ex) {
                    rate = 0;
                }
                if (rate == value) {
                    comboSampleRate.SelectedIndex = i;
                    break;
                }
            }
        }

        public bool isWaveFileOutputFromMasterTrack()
        {
            return radioMasterTrack.Checked;
        }

        public void setWaveFileOutputFromMasterTrack(bool value)
        {
            radioMasterTrack.Checked = value;
            radioCurrentTrack.Checked = !value;
        }

        public int getWaveFileOutputChannel()
        {
            if (comboChannel.SelectedIndex <= 0) {
                return 1;
            } else {
                return 2;
            }
        }

        public void setWaveFileOutputChannel(int value)
        {
            if (value == 1) {
                comboChannel.SelectedIndex = 0;
            } else {
                comboChannel.SelectedIndex = 1;
            }
        }
        #endregion

        #region event handlers
        public void btnOK_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public void btnCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        #endregion

        #region helper methods
        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void registerEventHandlers()
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void setResources()
        {
        }
        #endregion

        #region ui implementation
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
            this.groupWaveFileOutput = new System.Windows.Forms.GroupBox();
            this.comboSampleRate = new System.Windows.Forms.ComboBox();
            this.labelSampleRate = new System.Windows.Forms.Label();
            this.radioCurrentTrack = new System.Windows.Forms.RadioButton();
            this.radioMasterTrack = new System.Windows.Forms.RadioButton();
            this.lblChannel = new System.Windows.Forms.Label();
            this.comboChannel = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupSequence = new System.Windows.Forms.GroupBox();
            this.labelPreMeasure = new System.Windows.Forms.Label();
            this.comboPreMeasure = new System.Windows.Forms.ComboBox();
            this.groupWaveFileOutput.SuspendLayout();
            this.groupSequence.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupWaveFileOutput
            // 
            this.groupWaveFileOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupWaveFileOutput.Controls.Add(this.comboSampleRate);
            this.groupWaveFileOutput.Controls.Add(this.labelSampleRate);
            this.groupWaveFileOutput.Controls.Add(this.radioCurrentTrack);
            this.groupWaveFileOutput.Controls.Add(this.radioMasterTrack);
            this.groupWaveFileOutput.Controls.Add(this.lblChannel);
            this.groupWaveFileOutput.Controls.Add(this.comboChannel);
            this.groupWaveFileOutput.Location = new System.Drawing.Point(12, 12);
            this.groupWaveFileOutput.Name = "groupWaveFileOutput";
            this.groupWaveFileOutput.Size = new System.Drawing.Size(316, 107);
            this.groupWaveFileOutput.TabIndex = 28;
            this.groupWaveFileOutput.TabStop = false;
            this.groupWaveFileOutput.Text = "Wave File Output";
            // 
            // comboSampleRate
            // 
            this.comboSampleRate.Items.AddRange(new Object[] {
            "Mono",
            "Stereo"});
            this.comboSampleRate.Location = new System.Drawing.Point(135, 48);
            this.comboSampleRate.Name = "comboSampleRate";
            this.comboSampleRate.Size = new System.Drawing.Size(117, 20);
            this.comboSampleRate.TabIndex = 31;
            // 
            // labelSampleRate
            // 
            this.labelSampleRate.AutoSize = true;
            this.labelSampleRate.Location = new System.Drawing.Point(22, 51);
            this.labelSampleRate.Name = "labelSampleRate";
            this.labelSampleRate.Size = new System.Drawing.Size(66, 12);
            this.labelSampleRate.TabIndex = 30;
            this.labelSampleRate.Text = "Sample rate";
            // 
            // radioCurrentTrack
            // 
            this.radioCurrentTrack.AutoSize = true;
            this.radioCurrentTrack.Checked = true;
            this.radioCurrentTrack.Location = new System.Drawing.Point(155, 75);
            this.radioCurrentTrack.Name = "radioCurrentTrack";
            this.radioCurrentTrack.Size = new System.Drawing.Size(61, 16);
            this.radioCurrentTrack.TabIndex = 29;
            this.radioCurrentTrack.TabStop = true;
            this.radioCurrentTrack.Text = "Current";
            this.radioCurrentTrack.UseVisualStyleBackColor = true;
            // 
            // radioMasterTrack
            // 
            this.radioMasterTrack.AutoSize = true;
            this.radioMasterTrack.Location = new System.Drawing.Point(24, 75);
            this.radioMasterTrack.Name = "radioMasterTrack";
            this.radioMasterTrack.Size = new System.Drawing.Size(91, 16);
            this.radioMasterTrack.TabIndex = 28;
            this.radioMasterTrack.Text = "Master Track";
            this.radioMasterTrack.UseVisualStyleBackColor = true;
            // 
            // lblChannel
            // 
            this.lblChannel.AutoSize = true;
            this.lblChannel.Location = new System.Drawing.Point(22, 27);
            this.lblChannel.Name = "lblChannel";
            this.lblChannel.Size = new System.Drawing.Size(66, 12);
            this.lblChannel.TabIndex = 25;
            this.lblChannel.Text = "Channel (&C)";
            // 
            // comboChannel
            // 
            this.comboChannel.FormattingEnabled = true;
            this.comboChannel.Items.AddRange(new Object[] {
            "Mono",
            "Stereo"});
            this.comboChannel.Location = new System.Drawing.Point(135, 24);
            this.comboChannel.Name = "comboChannel";
            this.comboChannel.Size = new System.Drawing.Size(97, 20);
            this.comboChannel.TabIndex = 27;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(240, 207);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 23);
            this.btnCancel.TabIndex = 201;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(146, 207);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(88, 23);
            this.btnOK.TabIndex = 200;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // groupSequence
            // 
            this.groupSequence.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSequence.Controls.Add(this.labelPreMeasure);
            this.groupSequence.Controls.Add(this.comboPreMeasure);
            this.groupSequence.Location = new System.Drawing.Point(12, 125);
            this.groupSequence.Name = "groupSequence";
            this.groupSequence.Size = new System.Drawing.Size(316, 62);
            this.groupSequence.TabIndex = 202;
            this.groupSequence.TabStop = false;
            this.groupSequence.Text = "Sequence";
            // 
            // labelPreMeasure
            // 
            this.labelPreMeasure.AutoSize = true;
            this.labelPreMeasure.Location = new System.Drawing.Point(22, 27);
            this.labelPreMeasure.Name = "labelPreMeasure";
            this.labelPreMeasure.Size = new System.Drawing.Size(71, 12);
            this.labelPreMeasure.TabIndex = 25;
            this.labelPreMeasure.Text = "Pre-measure";
            // 
            // comboPreMeasure
            // 
            this.comboPreMeasure.FormattingEnabled = true;
            this.comboPreMeasure.Items.AddRange(new Object[] {
            "Mono",
            "Stereo"});
            this.comboPreMeasure.Location = new System.Drawing.Point(135, 24);
            this.comboPreMeasure.Name = "comboPreMeasure";
            this.comboPreMeasure.Size = new System.Drawing.Size(97, 20);
            this.comboPreMeasure.TabIndex = 27;
            // 
            // FormSequenceConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(341, 246);
            this.Controls.Add(this.groupSequence);
            this.Controls.Add(this.groupWaveFileOutput);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSequenceConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Sequence config";
            this.groupWaveFileOutput.ResumeLayout(false);
            this.groupWaveFileOutput.PerformLayout();
            this.groupSequence.ResumeLayout(false);
            this.groupSequence.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblChannel;
        private System.Windows.Forms.ComboBox comboChannel;
        private System.Windows.Forms.GroupBox groupWaveFileOutput;
        private System.Windows.Forms.RadioButton radioCurrentTrack;
        private System.Windows.Forms.RadioButton radioMasterTrack;
        private System.Windows.Forms.Label labelSampleRate;
        private System.Windows.Forms.GroupBox groupSequence;
        private System.Windows.Forms.Label labelPreMeasure;
        private System.Windows.Forms.ComboBox comboPreMeasure;
        private System.Windows.Forms.ComboBox comboSampleRate;

        #endregion

    }

}
