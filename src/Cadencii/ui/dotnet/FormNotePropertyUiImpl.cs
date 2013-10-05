/*
 * FormNotePropertyUiImpl.cs
 * Copyright © 2012 kbinani
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
using System.Drawing;
using cadencii.apputil;

namespace cadencii
{
    public class FormNotePropertyUiImpl : Form, FormNotePropertyUi
    {
        private FormNotePropertyUiListener listener;
        protected System.Windows.Forms.FormWindowState lastWindowState = System.Windows.Forms.FormWindowState.Normal;

        public FormNotePropertyUiImpl(FormNotePropertyUiListener listener)
        {
            this.listener = listener;
            InitializeComponent();
            Util.applyFontRecurse(this, AppManager.editorConfig.getBaseFont());
        }


        #region FormNotePropertyUiの実装

        public void addComponent(object c)
        {
            if (c == null) {
                return;
            }
            if (!(c is Control)) {
                return;
            }
            Control control = (Control)c;
            this.Controls.Add(control);
            control.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        public bool isWindowMinimized()
        {
            return this.WindowState == System.Windows.Forms.FormWindowState.Minimized;
        }

        public void deiconfyWindow()
        {
            this.WindowState = FormWindowState.Normal;
        }

        public void setTitle(string title)
        {
            this.Text = title;
        }

        public void close()
        {
            this.Close();
        }

        public void setMenuCloseAccelerator(Keys value)
        {
            this.menuClose.ShortcutKeys = value;
        }

        public void setAlwaysOnTop(bool alwaysOnTop)
        {
            this.TopMost = alwaysOnTop;
        }

        public bool isAlwaysOnTop()
        {
            return this.TopMost;
        }

        public void setBounds(int x, int y, int width, int height)
        {
            this.Bounds = new System.Drawing.Rectangle(x, y, width, height);
        }

        public int getX()
        {
            return this.Location.X;
        }

        public int getY()
        {
            return this.Location.Y;
        }

        public int getWidth()
        {
            return this.Width;
        }

        public int getHeight()
        {
            return this.Height;
        }

        public void setVisible(bool visible)
        {
            this.Visible = visible;
        }

        public bool isVisible()
        {
            return this.Visible;
        }

        public int getWorkingAreaX()
        {
            Rectangle r = Screen.GetWorkingArea(this);
            return r.X;
        }

        public int getWorkingAreaY()
        {
            Rectangle r = Screen.GetWorkingArea(this);
            return r.Y;
        }

        public int getWorkingAreaWidth()
        {
            Rectangle r = Screen.GetWorkingArea(this);
            return r.Width;
        }

        public int getWorkingAreaHeight()
        {
            Rectangle r = Screen.GetWorkingArea(this);
            return r.Height;
        }

        public void hideWindow()
        {
            this.Visible = false;
        }

        #endregion


        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.lastWindowState != this.WindowState) {
                this.listener.windowStateChanged();
            }
            this.lastWindowState = this.WindowState;
        }

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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuWindow});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(188, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            this.menuStrip.Visible = false;
            // 
            // menuWindow
            // 
            this.menuWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuClose});
            this.menuWindow.Name = "menuWindow";
            this.menuWindow.Size = new System.Drawing.Size(72, 20);
            this.menuWindow.Text = "Window(&W)";
            // 
            // menuClose
            // 
            this.menuClose.Name = "menuClose";
            this.menuClose.Size = new System.Drawing.Size(115, 22);
            this.menuClose.Text = "Close(&C)";
            this.menuClose.Click += new System.EventHandler(this.menuClose_Click);
            // 
            // FormNotePropertyUiImpl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(188, 291);
            this.Controls.Add(this.menuStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "FormNotePropertyUiImpl";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Note Property";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormNotePropertyUiImpl_Load);
            this.SizeChanged += new System.EventHandler(this.FormNotePropertyUiImpl_SizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNotePropertyUiImpl_FormClosing);
            this.LocationChanged += new System.EventHandler(this.FormNotePropertyUiImpl_LocationChanged);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem menuWindow;
        private ToolStripMenuItem menuClose;

        private void FormNotePropertyUiImpl_Load(object sender, System.EventArgs e)
        {
            this.listener.onLoad();
        }

        private void menuClose_Click(object sender, System.EventArgs e)
        {
            this.listener.menuCloseClick();
        }

        private void FormNotePropertyUiImpl_SizeChanged(object sender, System.EventArgs e)
        {
            this.listener.locationOrSizeChanged();
        }

        private void FormNotePropertyUiImpl_LocationChanged(object sender, System.EventArgs e)
        {
            this.listener.locationOrSizeChanged();
        }

        private void FormNotePropertyUiImpl_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != System.Windows.Forms.CloseReason.UserClosing) {
                return;
            }
            e.Cancel = true;
            this.listener.formClosing();
        }
    }
}
