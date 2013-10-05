/*
 * FormImportLyric.cs
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
using System.Collections.Generic;
using cadencii.apputil;
using cadencii;
using cadencii.java.util;
using cadencii.windows.forms;

namespace cadencii
{

    class FormImportLyric : System.Windows.Forms.Form
    {
        private int m_max_notes = 1;

        public FormImportLyric(int max_notes)
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            applyLanguage();
            setMaxNotes(max_notes);
            Util.applyFontRecurse(this, AppManager.editorConfig.getBaseFont());
        }

        #region public methods
        public void __setVisible(bool value)
        {
            base.Visible = value;
            this.txtLyrics.HideSelection = false;
            this.txtLyrics.SelectAll();
            this.txtLyrics.Focus();
        }

        public void applyLanguage()
        {
            this.Text = _("Import lyrics");
            btnCancel.Text = _("Cancel");
            btnOK.Text = _("OK");
        }

        /// <summary>
        /// このダイアログに入力できる最大の文字数を設定します．
        /// </summary>
        /// <param name="max_notes"></param>
        public void setMaxNotes(int max_notes)
        {
            string notes = (max_notes > 1) ? " [notes]" : " [note]";
            this.lblNotes.Text = "Max : " + max_notes + notes;
            this.m_max_notes = max_notes;
        }

        public string[] getLetters()
        {
            List<char> _SMALL = new List<char>(new char[] { 'ぁ', 'ぃ', 'ぅ', 'ぇ', 'ぉ',
                                                             'ゃ', 'ゅ', 'ょ',
                                                             'ァ', 'ィ', 'ゥ', 'ェ', 'ォ',
                                                             'ャ', 'ュ', 'ョ' });
            string tmp = "";
            for (int i = 0; i < m_max_notes; i++) {
                if (i >= txtLyrics.Lines.Length) {
                    break;
                }
                try {
                    int start = txtLyrics.GetFirstCharIndexFromLine(i);
                    int end = txtLyrics.GetFirstCharIndexFromLine(i) + txtLyrics.Lines[i].Length;
                    tmp += txtLyrics.Text.Substring(start, end - start) + " ";
                } catch (Exception ex) {
                    Logger.write(typeof(FormImportLyric) + ".getLetters; ex=" + ex + "\n");
                }
            }
            string[] spl = PortUtil.splitString(tmp, new char[] { '\n', '\t', ' ', '　', '\r' }, true);
            List<string> ret = new List<string>();
            for (int j = 0; j < spl.Length; j++) {
                string s = spl[j];
                char[] list = s.ToCharArray();
                string t = "";
                int i = -1;
                while (i + 1 < list.Length) {
                    i++;
                    if (0x41 <= list[i] && list[i] <= 0x176) {
                        t += list[i] + "";
                    } else {
                        if (PortUtil.getStringLength(t) > 0) {
                            ret.Add(t);
                            t = "";
                        }
                        if (i + 1 < list.Length) {
                            if (_SMALL.Contains(list[i + 1])) {
                                // 次の文字が拗音の場合
                                ret.Add(list[i] + "" + list[i + 1] + "");
                                i++;
                            } else {
                                ret.Add(list[i] + "");
                            }
                        } else {
                            ret.Add(list[i] + "");
                        }
                    }
                }
                if (PortUtil.getStringLength(t) > 0) {
                    ret.Add(t);
                }
            }
            return ret.ToArray();
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

        #region UI implementation
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

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtLyrics = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblNotes = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtLyrics
            // 
            this.txtLyrics.AcceptsReturn = true;
            this.txtLyrics.AcceptsTab = true;
            this.txtLyrics.Location = new System.Drawing.Point(12, 35);
            this.txtLyrics.Multiline = true;
            this.txtLyrics.Name = "txtLyrics";
            this.txtLyrics.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLyrics.Size = new System.Drawing.Size(426, 263);
            this.txtLyrics.TabIndex = 0;
            this.txtLyrics.WordWrap = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(363, 317);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(269, 317);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(15, 16);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(78, 12);
            this.lblNotes.TabIndex = 3;
            this.lblNotes.Text = "Max : *[notes]";
            // 
            // FormImportLyric
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(450, 352);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtLyrics);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImportLyric";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Import lyrics";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox txtLyrics;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblNotes;
        #endregion

    }

}
