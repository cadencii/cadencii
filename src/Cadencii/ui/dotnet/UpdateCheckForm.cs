/*
 * UpdateCheckForm.cs
 * Copyright © 2013 kbinani
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
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace cadencii.ui.dotnet
{
    class UpdateCheckForm : Form, cadencii.ui.UpdateCheckForm
    {
        private Label label1;
        private LinkLabel linkLabel1;
        private CheckBox checkBox1;
        private Button button1;

        public event EventHandler okButtonClicked;
        public event EventHandler downloadLinkClicked;

        public UpdateCheckForm()
        {
            InitializeComponent();
        }

        public void showDialog(object parent)
        {
            if (parent != null && parent is IWin32Window) {
                ShowDialog(parent as IWin32Window);
            } else {
                ShowDialog();
            }
        }

        public void setTitle(string title)
        {
            Text = title;
        }

        public void setFont(Font font)
        {
            cadencii.apputil.Util.applyFontRecurse(this, font);
        }

        public void setOkButtonText(string text)
        {
            button1.Text = text;
        }

        public void setDownloadUrl(string url)
        {
            linkLabel1.Text = url;
        }

        public void setMessage(string text)
        {
            label1.Text = text;
        }

        public void close()
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public bool isAutomaticallyCheckForUpdates()
        {
            return checkBox1.Checked;
        }

        public void setAutomaticallyCheckForUpdates(bool value)
        {
            checkBox1.Checked = value;
        }

        public void setAutomaticallyCheckForUpdatesMessage(string message)
        {
            checkBox1.Text = message;
        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(274, 97);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoEllipsis = true;
            this.linkLabel1.Location = new System.Drawing.Point(23, 50);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(335, 36);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "linkLabel1";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(25, 101);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 16);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // UpdateCheckForm
            // 
            this.ClientSize = new System.Drawing.Size(382, 142);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateCheckForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (okButtonClicked != null) {
                okButtonClicked.Invoke(sender, e);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (downloadLinkClicked != null) {
                downloadLinkClicked.Invoke(sender, new EventArgs());
            }
        }
    }
}
