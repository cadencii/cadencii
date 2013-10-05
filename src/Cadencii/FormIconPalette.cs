/*
 * FormIconPalette.cs
 * Copyright © 2010-2011 kbinani
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
using System.Linq;
using System.Collections.Generic;
using cadencii.apputil;
using cadencii.java.awt;
using cadencii.java.util;
using cadencii.vsq;
using cadencii.windows.forms;

namespace cadencii
{

    class DraggableBButton : Button
    {
        private IconDynamicsHandle mHandle = null;

        public IconDynamicsHandle getHandle()
        {
            return mHandle;
        }

        public void setHandle(IconDynamicsHandle value)
        {
            mHandle = value;
        }
    }

    public class FormIconPalette : Form
    {
        private List<Button> dynaffButtons = new List<Button>();
        private List<Button> crescendButtons = new List<Button>();
        private List<Button> decrescendButtons = new List<Button>();
        private int buttonWidth = 40;
        private FormMain mMainWindow = null;
        private bool mPreviousAlwaysOnTop;

        public FormIconPalette(FormMain main_window)
        {
            InitializeComponent();
            mMainWindow = main_window;
            applyLanguage();
            Util.applyFontRecurse(this, AppManager.editorConfig.getBaseFont());
            init();
            registerEventHandlers();
            SortedDictionary<string, Keys[]> dict = AppManager.editorConfig.getShortcutKeysDictionary(mMainWindow.getDefaultShortcutKeys());
            if (dict.ContainsKey("menuVisualIconPalette")) {
                Keys[] keys = dict["menuVisualIconPalette"];
                Keys shortcut = Keys.None;
                keys.Aggregate(shortcut, (seed, key) => seed | key);
                menuWindowHide.ShortcutKeys = shortcut;
            }
        }

        #region public methods
        /// <summary>
        /// AlwaysOnTopが強制的にfalseにされる直前の，AlwaysOnTop値を取得します．
        /// </summary>
        public bool getPreviousAlwaysOnTop()
        {
            return mPreviousAlwaysOnTop;
        }

        /// <summary>
        /// AlwaysOnTopが強制的にfalseにされる直前の，AlwaysOnTop値を設定しておきます．
        /// </summary>
        public void setPreviousAlwaysOnTop(bool value)
        {
            mPreviousAlwaysOnTop = value;
        }

        public void applyLanguage()
        {
            this.Text = _("Icon Palette");
        }

        public void applyShortcut(Keys shortcut)
        {
            menuWindowHide.ShortcutKeys = shortcut;
        }
        #endregion

        #region helper methods
        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        private void registerEventHandlers()
        {
            this.Load += new EventHandler(FormIconPalette_Load);
            this.FormClosing += new FormClosingEventHandler(FormIconPalette_FormClosing);
            menuWindowHide.Click += new EventHandler(menuWindowHide_Click);
        }

        private void init()
        {
            foreach (var handle in VocaloSysUtil.dynamicsConfigIterator(SynthesizerType.VOCALOID1)) {
                string icon_id = handle.IconID;
                DraggableBButton btn = new DraggableBButton();
                btn.Name = icon_id;
                btn.setHandle(handle);
                string buttonIconPath = handle.getButtonImageFullPath();

                bool setimg = System.IO.File.Exists(buttonIconPath);
                if (setimg) {
                    btn.Image = System.Drawing.Image.FromStream(new System.IO.FileStream(buttonIconPath, System.IO.FileMode.Open, System.IO.FileAccess.Read));
                } else {
                    System.Drawing.Image img = null;
                    string str = "";
                    string caption = handle.IDS;
                    if (caption.Equals("cresc_1")) {
                        img = Properties.Resources.cresc1;
                    } else if (caption.Equals("cresc_2")) {
                        img = Properties.Resources.cresc2;
                    } else if (caption.Equals("cresc_3")) {
                        img = Properties.Resources.cresc3;
                    } else if (caption.Equals("cresc_4")) {
                        img = Properties.Resources.cresc4;
                    } else if (caption.Equals("cresc_5")) {
                        img = Properties.Resources.cresc5;
                    } else if (caption.Equals("dim_1")) {
                        img = Properties.Resources.dim1;
                    } else if (caption.Equals("dim_2")) {
                        img = Properties.Resources.dim2;
                    } else if (caption.Equals("dim_3")) {
                        img = Properties.Resources.dim3;
                    } else if (caption.Equals("dim_4")) {
                        img = Properties.Resources.dim4;
                    } else if (caption.Equals("dim_5")) {
                        img = Properties.Resources.dim5;
                    } else if (caption.Equals("Dynaff11")) {
                        str = "fff";
                    } else if (caption.Equals("Dynaff12")) {
                        str = "ff";
                    } else if (caption.Equals("Dynaff13")) {
                        str = "f";
                    } else if (caption.Equals("Dynaff21")) {
                        str = "mf";
                    } else if (caption.Equals("Dynaff22")) {
                        str = "mp";
                    } else if (caption.Equals("Dynaff31")) {
                        str = "p";
                    } else if (caption.Equals("Dynaff32")) {
                        str = "pp";
                    } else if (caption.Equals("Dynaff33")) {
                        str = "ppp";
                    }
                    if (img != null) {
                        btn.Image = img;
                    } else {
                        btn.Text = str;
                    }
                }
                btn.MouseDown += new MouseEventHandler(handleCommonMouseDown);
                btn.Size = new System.Drawing.Size(buttonWidth, buttonWidth);
                int iw = 0;
                int ih = 0;
                if (icon_id.StartsWith(IconDynamicsHandle.ICONID_HEAD_DYNAFF)) {
                    // dynaff
                    dynaffButtons.Add(btn);
                    ih = 0;
                    iw = dynaffButtons.Count - 1;
                } else if (icon_id.StartsWith(IconDynamicsHandle.ICONID_HEAD_CRESCEND)) {
                    // crescend
                    crescendButtons.Add(btn);
                    ih = 1;
                    iw = crescendButtons.Count - 1;
                } else if (icon_id.StartsWith(IconDynamicsHandle.ICONID_HEAD_DECRESCEND)) {
                    // decrescend
                    decrescendButtons.Add(btn);
                    ih = 2;
                    iw = decrescendButtons.Count - 1;
                } else {
                    continue;
                }
                btn.Location = new System.Drawing.Point(iw * buttonWidth, ih * buttonWidth);
                this.Controls.Add(btn);
                btn.BringToFront();
            }

            // ウィンドウのサイズを固定化する
            int height = 0;
            int width = 0;
            if (dynaffButtons.Count > 0) {
                height += buttonWidth;
            }
            width = Math.Max(width, buttonWidth * dynaffButtons.Count);
            if (crescendButtons.Count > 0) {
                height += buttonWidth;
            }
            width = Math.Max(width, buttonWidth * crescendButtons.Count);
            if (decrescendButtons.Count > 0) {
                height += buttonWidth;
            }
            width = Math.Max(width, buttonWidth * decrescendButtons.Count);
            this.ClientSize = new System.Drawing.Size(width, height);
            var size = this.Size;
            this.MaximumSize = new System.Drawing.Size(size.Width, size.Height);
            this.MinimumSize = new System.Drawing.Size(size.Width, size.Height);
        }
        #endregion

        #region event handlers
        public void FormIconPalette_Load(Object sender, EventArgs e)
        {
            // コンストラクタから呼ぶと、スレッドが違うので（たぶん）うまく行かない
            this.TopMost = true;
        }

        public void FormIconPalette_FormClosing(Object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }

        public void menuWindowHide_Click(Object sender, EventArgs e)
        {
            this.Visible = false;
        }

        public void handleCommonMouseDown(Object sender, MouseEventArgs e)
        {
            if (AppManager.getEditMode() != EditMode.NONE) {
                return;
            }
            DraggableBButton btn = (DraggableBButton)sender;
            if (mMainWindow != null) {
                mMainWindow.BringToFront();
            }

            IconDynamicsHandle handle = btn.getHandle();
            VsqEvent item = new VsqEvent();
            item.Clock = 0;
            item.ID.Note = 60;
            item.ID.type = VsqIDType.Aicon;
            item.ID.IconDynamicsHandle = (IconDynamicsHandle)handle.clone();
            int length = handle.getLength();
            if (length <= 0) {
                length = 1;
            }
            item.ID.setLength(length);
            AppManager.mAddingEvent = item;

            btn.DoDragDrop(handle, System.Windows.Forms.DragDropEffects.All);
        }
        #endregion

        #region UI implementation
        private void InitializeComponent()
        {
            this.menuBar = new MenuStrip();
            this.menuWindow = new ToolStripMenuItem();
            this.menuWindowHide = new ToolStripMenuItem();
            this.menuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuWindow});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(458, 24);
            this.menuBar.TabIndex = 0;
            this.menuBar.Text = "bMenuBar1";
            // 
            // menuWindow
            // 
            this.menuWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuWindowHide});
            this.menuWindow.Name = "menuWindow";
            this.menuWindow.Size = new System.Drawing.Size(55, 20);
            this.menuWindow.Text = "Window";
            // 
            // menuWindowHide
            // 
            this.menuWindowHide.Name = "menuWindowHide";
            this.menuWindowHide.Size = new System.Drawing.Size(93, 22);
            this.menuWindowHide.Text = "Hide";
            // 
            // FormIconPalette
            // 
            this.ClientSize = new System.Drawing.Size(458, 342);
            this.Controls.Add(this.menuBar);
            this.MainMenuStrip = this.menuBar;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormIconPalette";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Icon Palette";
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private MenuStrip menuBar;
        private ToolStripMenuItem menuWindow;
        private ToolStripMenuItem menuWindowHide;

        #endregion

    }

}
