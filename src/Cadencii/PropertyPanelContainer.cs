#if ENABLE_PROPERTY
/*
 * PropertyPanelContainer.cs
 * Copyright © 2009-2011 kbinani
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
using cadencii.java.awt;
using cadencii.windows.forms;

namespace cadencii
{

    public class PropertyPanelContainer : UserControl
    {
        public const int _TITLE_HEIGHT = 29;
        public event StateChangeRequiredEventHandler StateChangeRequired;

        public PropertyPanelContainer()
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
        }

        public void addComponent(Control c)
        {
            panelMain.Controls.Add(c);
            c.Dock = DockStyle.Fill;
        }

        public void panelTitle_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            handleRestoreWindow();
        }

        public void btnClose_Click(Object sender, EventArgs e)
        {
            handleClose();
        }

        public void btnWindow_Click(Object sender, EventArgs e)
        {
            handleRestoreWindow();
        }

        private void handleClose()
        {
            invokeStateChangeRequiredEvent(PanelState.Hidden);
        }

        private void handleRestoreWindow()
        {
            invokeStateChangeRequiredEvent(PanelState.Window);
        }

        private void invokeStateChangeRequiredEvent(PanelState state)
        {
            if (StateChangeRequired != null) {
                StateChangeRequired(this, state);
            }
        }

        /// <summary>
        /// javaは自動レイアウトなのでいらない
        /// </summary>
        private void panelMain_SizeChanged(Object sender, EventArgs e)
        {
            panelTitle.Left = 0;
            panelTitle.Top = 0;
            panelTitle.Height = _TITLE_HEIGHT;
            panelTitle.Width = this.Width;

            panelMain.Top = _TITLE_HEIGHT;
            panelMain.Left = 0;
            panelMain.Width = this.Width;
            panelMain.Height = this.Height - _TITLE_HEIGHT;
        }

        private void registerEventHandlers()
        {
            this.panelMain.SizeChanged += new EventHandler(panelMain_SizeChanged);
            this.btnClose.Click += new EventHandler(btnClose_Click);
            this.btnWindow.Click += new EventHandler(btnWindow_Click);
            this.panelTitle.MouseDoubleClick += new MouseEventHandler(panelTitle_MouseDoubleClick);
        }

        private void setResources()
        {
            this.btnClose.Image = Properties.Resources.cross_small;
            this.btnWindow.Image = Properties.Resources.chevron_small_collapse;
        }

        #region ui impl for C#
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

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMain = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnWindow = new System.Windows.Forms.Button();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.panelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Location = new System.Drawing.Point(0, 29);
            this.panelMain.Margin = new System.Windows.Forms.Padding(0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(159, 283);
            this.panelMain.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(133, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(23, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnWindow
            // 
            this.btnWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWindow.Location = new System.Drawing.Point(104, 3);
            this.btnWindow.Name = "btnWindow";
            this.btnWindow.Size = new System.Drawing.Size(23, 23);
            this.btnWindow.TabIndex = 2;
            this.btnWindow.UseVisualStyleBackColor = true;
            // 
            // panelTitle
            // 
            this.panelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTitle.Controls.Add(this.btnWindow);
            this.panelTitle.Controls.Add(this.btnClose);
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(159, 29);
            this.panelTitle.TabIndex = 3;
            // 
            // PropertyPanelContainer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelTitle);
            this.Controls.Add(this.panelMain);
            this.Name = "PropertyPanelContainer";
            this.Size = new System.Drawing.Size(159, 312);
            this.panelTitle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private Button btnClose;
        private Button btnWindow;
        private System.Windows.Forms.Panel panelTitle;
        #endregion
    }

}
#endif
