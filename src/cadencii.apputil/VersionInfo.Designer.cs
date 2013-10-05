/*
 * VersionInfo.Designer.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Drawing;

namespace cadencii.apputil
{

    partial class VersionInfo
    {
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
            this.components = new System.ComponentModel.Container();
            this.btnFlip = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.btnSaveAuthorList = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // btnFlip
            //
            this.btnFlip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFlip.Location = new System.Drawing.Point(13, 391);
            this.btnFlip.Name = "btnFlip";
            this.btnFlip.Size = new System.Drawing.Size(75, 21);
            this.btnFlip.TabIndex = 2;
            this.btnFlip.Text = "クレジット";
            this.btnFlip.UseVisualStyleBackColor = true;
            this.btnFlip.Click += new System.EventHandler(this.btnFlip_Click);
            //
            // btnOK
            //
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(211, 391);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 21);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            //
            // timer
            //
            this.timer.Interval = 30;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            //
            // btnSaveAuthorList
            //
            this.btnSaveAuthorList.Location = new System.Drawing.Point(123, 391);
            this.btnSaveAuthorList.Name = "btnSaveAuthorList";
            this.btnSaveAuthorList.Size = new System.Drawing.Size(43, 21);
            this.btnSaveAuthorList.TabIndex = 3;
            this.btnSaveAuthorList.Text = "button1";
            this.btnSaveAuthorList.UseVisualStyleBackColor = true;
            this.btnSaveAuthorList.Visible = false;
            //
            // VersionInfoEx
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(300, 426);
            this.Controls.Add(this.btnSaveAuthorList);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnFlip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(306, 451);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(306, 451);
            this.Name = "VersionInfoEx";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VersionInfoEx";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.VersionInfoEx_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VersionInfoEx_KeyDown);
            this.FontChanged += new System.EventHandler(this.VersionInfoEx_FontChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public void applyLanguage()
        {
            string about = string.Format(_("About {0}"), m_app_name);
            string credit = _("Credit");
            cadencii.java.awt.Dimension size1 = Util.measureString(about, new cadencii.java.awt.Font(btnFlip.Font));
            cadencii.java.awt.Dimension size2 = Util.measureString(credit, new cadencii.java.awt.Font(btnFlip.Font));
            m_button_width_about = Math.Max(75, (int)(size1.width * 1.3));
            m_button_width_credit = Math.Max(75, (int)(size2.width * 1.3));
            if (m_credit_mode) {
                btnFlip.Width = m_button_width_about;
                btnFlip.Text = about;
            } else {
                btnFlip.Width = m_button_width_credit;
                btnFlip.Text = credit;
            }
            this.Text = about;
        }
        private System.Windows.Forms.Button btnFlip;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnSaveAuthorList;
    }
}
