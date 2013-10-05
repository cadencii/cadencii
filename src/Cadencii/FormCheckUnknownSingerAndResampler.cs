/*
 * FormCheckUnknownSingerAndResampler.cs
 * Copyright © 2010 kbinani
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
using cadencii.apputil;
using cadencii.vsq;
using cadencii;
using cadencii.java.util;
using cadencii.windows.forms;
using cadencii.java.awt;



namespace cadencii
{

    public class FormCheckUnknownSingerAndResampler : System.Windows.Forms.Form
    {
        /// <summary>
        /// コンストラクタ．
        /// </summary>
        /// <param name="singer"></param>
        /// <param name="apply_singer"></param>
        /// <param name="resampler"></param>
        /// <param name="apply_resampler"></param>
        public FormCheckUnknownSingerAndResampler(string singer, bool apply_singer, string resampler, bool apply_resampler)
        {
            InitializeComponent();
            applyLanguage();
            Util.applyFontRecurse(this, AppManager.editorConfig.getBaseFont());

            // singers
            checkSingerImport.Checked = apply_singer;
            checkSingerImport.Enabled = apply_singer;
            textSingerPath.ReadOnly = true;
            textSingerPath.Enabled = apply_singer;
            if (apply_singer) {
                textSingerPath.Text = singer;
                SingerConfig sc = new SingerConfig();
                string path_image = Utility.readUtauSingerConfig(singer, sc);
#if DEBUG
                sout.println("FormCheckUnknownSingerAndResampler#.ctor;  path_image=" + path_image);
#endif
                Image img = IconParader.createIconImage(path_image, sc.VOICENAME);
                pictureSinger.Image = img.image;
                labelSingerName.Text = sc.VOICENAME;
            }

            // resampler
            checkResamplerImport.Checked = apply_resampler;
            checkResamplerImport.Enabled = apply_resampler;
            textResamplerPath.ReadOnly = true;
            textResamplerPath.Enabled = apply_resampler;
            if (apply_resampler) {
                textResamplerPath.Text = resampler;
            }

            registerEventHandlers();
        }

        #region public methods
        /// <summary>
        /// 原音の項目にチェックが入れられたか
        /// </summary>
        /// <returns></returns>
        public bool isSingerChecked()
        {
            return checkSingerImport.Checked;
        }

        /// <summary>
        /// 原音のパスを取得します
        /// </summary>
        /// <returns></returns>
        public string getSingerPath()
        {
            return textSingerPath.Text;
        }

        /// <summary>
        /// リサンプラーの項目にチェックが入れられたかどうか
        /// </summary>
        /// <returns></returns>
        public bool isResamplerChecked()
        {
            return checkResamplerImport.Checked;
        }

        /// <summary>
        /// リサンプラーのパスを取得します
        /// </summary>
        /// <returns></returns>
        public string getResamplerPath()
        {
            return textResamplerPath.Text;
        }
        #endregion

        #region helper methods
        /// <summary>
        /// イベントハンドラを登録します
        /// </summary>
        private void registerEventHandlers()
        {
        }

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void applyLanguage()
        {
            this.Text = _("Unknown singers and resamplers");
            labelMessage.Text = _("These singers and resamplers are not registered to Cadencii.\nCheck the box if you want to register them.");
            checkSingerImport.Text = _("Import singer");
            checkResamplerImport.Text = _("Import resampler");
        }
        #endregion

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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.checkSingerImport = new System.Windows.Forms.CheckBox();
            this.pictureSinger = new cadencii.IconParader();
            this.labelSingerName = new System.Windows.Forms.Label();
            this.textSingerPath = new System.Windows.Forms.TextBox();
            this.checkResamplerImport = new System.Windows.Forms.CheckBox();
            this.textResamplerPath = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSinger)).BeginInit();
            this.SuspendLayout();
            //
            // buttonCancel
            //
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(300, 254);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 11;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            //
            // buttonOk
            //
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(219, 254);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 10;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            //
            // labelMessage
            //
            this.labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMessage.Location = new System.Drawing.Point(12, 12);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(357, 57);
            this.labelMessage.TabIndex = 146;
            this.labelMessage.Text = "These singers and resamplers are not registered to Cadencii.\r\nCheck the box if yo" +
                "u want to register them.";
            //
            // checkSingerImport
            //
            this.checkSingerImport.AutoSize = true;
            this.checkSingerImport.Checked = true;
            this.checkSingerImport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSingerImport.Location = new System.Drawing.Point(14, 83);
            this.checkSingerImport.Name = "checkSingerImport";
            this.checkSingerImport.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.checkSingerImport.Size = new System.Drawing.Size(101, 16);
            this.checkSingerImport.TabIndex = 148;
            this.checkSingerImport.Text = "Import singer";
            this.checkSingerImport.UseVisualStyleBackColor = true;
            //
            // pictureSinger
            //
            this.pictureSinger.Location = new System.Drawing.Point(38, 105);
            this.pictureSinger.MaximumSize = new System.Drawing.Size(48, 48);
            this.pictureSinger.MinimumSize = new System.Drawing.Size(48, 48);
            this.pictureSinger.Name = "pictureSinger";
            this.pictureSinger.Size = new System.Drawing.Size(48, 48);
            this.pictureSinger.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureSinger.TabIndex = 149;
            this.pictureSinger.TabStop = false;
            //
            // labelSingerName
            //
            this.labelSingerName.AutoSize = true;
            this.labelSingerName.Location = new System.Drawing.Point(103, 111);
            this.labelSingerName.Name = "labelSingerName";
            this.labelSingerName.Size = new System.Drawing.Size(40, 12);
            this.labelSingerName.TabIndex = 150;
            this.labelSingerName.Text = "(name)";
            //
            // textSingerPath
            //
            this.textSingerPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textSingerPath.Location = new System.Drawing.Point(99, 130);
            this.textSingerPath.Name = "textSingerPath";
            this.textSingerPath.Size = new System.Drawing.Size(270, 19);
            this.textSingerPath.TabIndex = 151;
            //
            // checkResamplerImport
            //
            this.checkResamplerImport.AutoSize = true;
            this.checkResamplerImport.Checked = true;
            this.checkResamplerImport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkResamplerImport.Location = new System.Drawing.Point(14, 192);
            this.checkResamplerImport.Name = "checkResamplerImport";
            this.checkResamplerImport.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.checkResamplerImport.Size = new System.Drawing.Size(120, 16);
            this.checkResamplerImport.TabIndex = 152;
            this.checkResamplerImport.Text = "Import resampler";
            this.checkResamplerImport.UseVisualStyleBackColor = true;
            //
            // textResamplerPath
            //
            this.textResamplerPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textResamplerPath.Location = new System.Drawing.Point(38, 214);
            this.textResamplerPath.Name = "textResamplerPath";
            this.textResamplerPath.Size = new System.Drawing.Size(331, 19);
            this.textResamplerPath.TabIndex = 153;
            //
            // FormCheckUnknownSingerAndResampler
            //
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(387, 289);
            this.Controls.Add(this.textResamplerPath);
            this.Controls.Add(this.checkResamplerImport);
            this.Controls.Add(this.textSingerPath);
            this.Controls.Add(this.labelSingerName);
            this.Controls.Add(this.pictureSinger);
            this.Controls.Add(this.checkSingerImport);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCheckUnknownSingerAndResampler";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Unknown singers and resamplers";
            ((System.ComponentModel.ISupportInitialize)(this.pictureSinger)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.CheckBox checkSingerImport;
        private System.Windows.Forms.Label labelSingerName;
        private System.Windows.Forms.TextBox textSingerPath;
        private System.Windows.Forms.CheckBox checkResamplerImport;
        private System.Windows.Forms.TextBox textResamplerPath;
        private IconParader pictureSinger;

        #endregion
    }

}
