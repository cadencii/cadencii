/*
 * FormMixer.Designer.cs
 * Copyright (c) 2008-2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;

namespace Boare.Cadencii {
    
    using boolean = Boolean;

    partial class FormMixer {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( boolean disposing ) {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuVisual = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVisualReturn = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.hScroll = new System.Windows.Forms.HScrollBar();
            this.volumeMaster = new Boare.Cadencii.VolumeTracker();
            this.chkTopmost = new System.Windows.Forms.CheckBox();
            this.menuMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuVisual} );
            this.menuMain.Location = new System.Drawing.Point( 0, 0 );
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size( 173, 24 );
            this.menuMain.TabIndex = 1;
            this.menuMain.Text = "menuStrip1";
            this.menuMain.Visible = false;
            // 
            // menuVisual
            // 
            this.menuVisual.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuVisualReturn} );
            this.menuVisual.Name = "menuVisual";
            this.menuVisual.Size = new System.Drawing.Size( 57, 20 );
            this.menuVisual.Text = "表示(&V)";
            // 
            // menuVisualReturn
            // 
            this.menuVisualReturn.Name = "menuVisualReturn";
            this.menuVisualReturn.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.menuVisualReturn.Size = new System.Drawing.Size( 177, 22 );
            this.menuVisualReturn.Text = "エディタ画面へ戻る";
            this.menuVisualReturn.Click += new System.EventHandler( this.menuVisualReturn_Click );
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.hScroll );
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Margin = new System.Windows.Forms.Padding( 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 85, 279 );
            this.panel1.TabIndex = 6;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler( this.panel1_Paint );
            // 
            // hScroll
            // 
            this.hScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hScroll.LargeChange = 10;
            this.hScroll.Location = new System.Drawing.Point( 0, 260 );
            this.hScroll.Margin = new System.Windows.Forms.Padding( 0 );
            this.hScroll.Maximum = 1;
            this.hScroll.Minimum = 0;
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new System.Drawing.Size( 85, 19 );
            this.hScroll.SmallChange = 1;
            this.hScroll.TabIndex = 0;
            this.hScroll.Value = 0;
            this.hScroll.ValueChanged += new System.EventHandler( this.veScrollBar_ValueChanged );
            // 
            // volumeMaster
            // 
            this.volumeMaster.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))) );
            this.volumeMaster.Feder = 0;
            this.volumeMaster.IsMuted = false;
            this.volumeMaster.IsSolo = true;
            this.volumeMaster.Location = new System.Drawing.Point( 87, 0 );
            this.volumeMaster.Margin = new System.Windows.Forms.Padding( 0 );
            this.volumeMaster.Name = "volumeMaster";
            this.volumeMaster.Number = "Master";
            this.volumeMaster.Panpot = 0;
            this.volumeMaster.Size = new System.Drawing.Size( 85, 261 );
            this.volumeMaster.SoloButtonVisible = false;
            this.volumeMaster.TabIndex = 5;
            this.volumeMaster.Title = "";
            this.volumeMaster.PanpotChanged += new System.EventHandler( this.volumeMaster_PanpotChanged );
            this.volumeMaster.IsMutedChanged += new System.EventHandler( this.volumeMaster_IsMutedChanged );
            this.volumeMaster.FederChanged += new System.EventHandler( this.volumeMaster_FederChanged );
            // 
            // chkTopmost
            // 
            this.chkTopmost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkTopmost.Location = new System.Drawing.Point( 87, 261 );
            this.chkTopmost.Margin = new System.Windows.Forms.Padding( 0 );
            this.chkTopmost.Name = "chkTopmost";
            this.chkTopmost.Size = new System.Drawing.Size( 85, 18 );
            this.chkTopmost.TabIndex = 7;
            this.chkTopmost.Text = "Top Most";
            this.chkTopmost.UseVisualStyleBackColor = true;
            this.chkTopmost.CheckedChanged += new System.EventHandler( this.chkTopmost_CheckedChanged );
            // 
            // FormMixer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))) );
            this.ClientSize = new System.Drawing.Size( 173, 279 );
            this.Controls.Add( this.chkTopmost );
            this.Controls.Add( this.panel1 );
            this.Controls.Add( this.volumeMaster );
            this.Controls.Add( this.menuMain );
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMixer";
            this.ShowInTaskbar = false;
            this.Text = "Mixer";
            this.Paint += new System.Windows.Forms.PaintEventHandler( this.FormMixer_Paint );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.FormMixer_FormClosing );
            this.menuMain.ResumeLayout( false );
            this.menuMain.PerformLayout();
            this.panel1.ResumeLayout( false );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuVisual;
        private System.Windows.Forms.ToolStripMenuItem menuVisualReturn;
        private VolumeTracker volumeMaster;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.HScrollBar hScroll;
        private System.Windows.Forms.CheckBox chkTopmost;
    }
}