/*
 * FormMixer.cs
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
#if JAVA
package org.kbinani.Cadencii;
#else
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.util;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
#endif

    public class FormMixer : Form {
        private FormMain m_parent;
        private VolumeTracker[] m_tracker;

        public delegate void FederChangedEventHandler( int track, int feder );
        public delegate void PanpotChangedEventHandler( int track, int panpot );
        public delegate void SoloChangedEventHandler( int track, boolean solo );
        public delegate void MuteChangedEventHandler( int track, boolean mute );

        public event FederChangedEventHandler FederChanged;
        public event PanpotChangedEventHandler PanpotChanged;
        public event SoloChangedEventHandler SoloChanged;
        public event MuteChangedEventHandler MuteChanged;
        public event TopMostChangedEventHandler TopMostChanged;

        public void ApplyLanguage() {
            Text = _( "Mixer" );
            chkTopmost.Text = _( "Top Most" );
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public void updateStatus() {
            int num = AppManager.getVsqFile().Mixer.Slave.size() + AppManager.getBgmCount();
#if DEBUG
            PortUtil.println( "FormMixer#UpdateStatus; num=" + num );
#endif
            if ( m_tracker != null ) {
                for ( int i = 0; i < m_tracker.Length; i++ ) {
                    m_tracker[i].Dispose();
                    m_tracker[i] = null;
                }
                m_tracker = null;
            }
            m_tracker = new VolumeTracker[num];

            // 同時に表示できるVolumeTrackerの個数を計算
            Size max = Screen.GetWorkingArea( this ).Size;
            Size bordersize = SystemInformation.FrameBorderSize;
            int max_client_width = max.Width - 2 * bordersize.Width;
            int max_num = (int)Math.Floor( max_client_width / (VolumeTracker.WIDTH + 1.0f) );
            num++;

            int screen_num = num <= max_num ? num : max_num; //スクリーン上に表示するVolumeTrackerの個数

            // panel1上に配置するVolumeTrackerの個数
            int num_vtracker_on_panel = AppManager.getVsqFile().Mixer.Slave.size() + AppManager.getBgmCount();
            // panel1上に配置可能なVolumeTrackerの個数
            int panel_capacity = max_num - 1;

            if ( panel_capacity >= num_vtracker_on_panel ) {
                // volumeMaster以外の全てのVolumeTrackerを，画面上に同時表示可能
                hScroll.Minimum = 0;
                hScroll.Value = 0;
                hScroll.Maximum = 0;
                hScroll.LargeChange = 1;
            } else {
                // num_vtracker_on_panel個のVolumeTrackerのうち，panel_capacity個しか，画面上に同時表示できない
                hScroll.Minimum = 0;
                hScroll.Value = 0;
                hScroll.Maximum = num_vtracker_on_panel - 1;
                hScroll.LargeChange = panel_capacity;
            }

#if DEBUG
            AppManager.debugWriteLine( "FormMixer#updateStatus;" );
            AppManager.debugWriteLine( "    num_vtracker_on_panel=" + num_vtracker_on_panel );
            AppManager.debugWriteLine( "    panel_capacity=" + panel_capacity );
            AppManager.debugWriteLine( "    hScroll.Maximum=" + hScroll.Maximum );
            AppManager.debugWriteLine( "    hScroll.LargeChange=" + hScroll.LargeChange );
#endif

            int j = -1;
            for ( Iterator itr = AppManager.getVsqFile().Mixer.Slave.iterator(); itr.hasNext(); ) {
                VsqMixerEntry vme = (VsqMixerEntry)itr.next();
                j++;
                m_tracker[j] = new VolumeTracker();
                m_tracker[j].setFeder( vme.Feder );
                m_tracker[j].setPanpot( vme.Panpot );
                m_tracker[j].setTitle( AppManager.getVsqFile().Track.get( j + 1 ).getName() );
                m_tracker[j].setNumber( (j + 1) + "" );
                m_tracker[j].Location = new Point( j * (VolumeTracker.WIDTH + 1), 0 );
                m_tracker[j].setSoloButtonVisible( true );
                m_tracker[j].setMuted( (vme.Mute == 1) );
                m_tracker[j].setSolo( (vme.Solo == 1) );
                m_tracker[j].Tag = j + 1;
                m_tracker[j].IsMutedChanged += new EventHandler( FormMixer_IsMutedChanged );
                m_tracker[j].IsSoloChanged += new EventHandler( FormMixer_IsSoloChanged );
                m_tracker[j].FederChanged += new EventHandler( FormMixer_FederChanged );
                m_tracker[j].PanpotChanged += new EventHandler( FormMixer_PanpotChanged );
                panel1.Controls.Add( m_tracker[j] );
            }
            int count = AppManager.getBgmCount();
            for ( int i = 0; i < count; i++ ) {
                j++;
                BgmFile item = AppManager.getBgm( i );
                m_tracker[j] = new VolumeTracker();
                m_tracker[j].setFeder( item.feder );
                m_tracker[j].setPanpot( item.panpot );
                m_tracker[j].setTitle( Path.GetFileName( item.file ) );
                m_tracker[j].setNumber( "" );
                m_tracker[j].Location = new Point( j * (VolumeTracker.WIDTH + 1), 0 );
                m_tracker[j].setSoloButtonVisible( false );
                m_tracker[j].setMuted( (item.mute == 1) );
                m_tracker[j].setSolo( false );
                m_tracker[j].Tag = (int)(-i - 1);
                m_tracker[j].IsMutedChanged += new EventHandler( FormMixer_IsMutedChanged );
                //m_tracker[j].IsSoloChanged += new EventHandler( FormMixer_IsSoloChanged );
                m_tracker[j].FederChanged += new EventHandler( FormMixer_FederChanged );
                m_tracker[j].PanpotChanged += new EventHandler( FormMixer_PanpotChanged );
                panel1.Controls.Add( m_tracker[j] );
            }
            volumeMaster.setFeder( AppManager.getVsqFile().Mixer.MasterFeder );
            volumeMaster.setPanpot( AppManager.getVsqFile().Mixer.MasterPanpot );
            panel1.Width = (VolumeTracker.WIDTH + 1) * (screen_num - 1);
            volumeMaster.Location = new Point( (screen_num - 1) * (VolumeTracker.WIDTH + 1) + 3, 0 );
            chkTopmost.Left = panel1.Width;
            this.MaximumSize = Size.Empty;
            this.MinimumSize = Size.Empty;
            this.ClientSize = new Size( screen_num * (VolumeTracker.WIDTH + 1) + 3, 279 );
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.Invalidate();
            m_parent.Focus();
        }

        private void FormMixer_PanpotChanged( object sender, EventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (int)parent.Tag;
            if ( PanpotChanged != null ) {
                PanpotChanged( track, parent.getPanpot() );
            }
        }

        private void FormMixer_FederChanged( object sender, EventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (int)parent.Tag;
            if ( FederChanged != null ) {
                FederChanged( track, parent.getFeder() );
            }
        }

        private void FormMixer_IsSoloChanged( object sender, EventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (int)parent.Tag;
            if ( SoloChanged != null ) {
                SoloChanged( track, parent.isSolo() );
            }
        }

        private void FormMixer_IsMutedChanged( object sender, EventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (int)parent.Tag;
            if ( MuteChanged != null ) {
                MuteChanged( track, parent.isMuted() );
            }
        }

        public FormMixer( FormMain parent ) {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            volumeMaster.setFeder( 0 );
            volumeMaster.setMuted( false );
            volumeMaster.setSolo( true );
            volumeMaster.setNumber( "Master" );
            volumeMaster.setPanpot( 0 );
            volumeMaster.setSoloButtonVisible( false );
            volumeMaster.setTitle( "" );
            ApplyLanguage();
            m_parent = parent;
            this.Icon = Properties.Resources.Icon1;
            this.SetStyle( ControlStyles.DoubleBuffer, true );
        }

        private void menuVisualReturn_Click( object sender, EventArgs e ) {
            m_parent.flipMixerDialogVisible( false );
        }

        private void FormMixer_FormClosing( object sender, FormClosingEventArgs e ) {
            m_parent.flipMixerDialogVisible( false );
            e.Cancel = true;
        }

        private void FormMixer_Paint( object sender, PaintEventArgs e ) {
            int val = (int)hScroll.Value;
            for ( int i = 0; i < m_tracker.Length; i++ ) {
                m_tracker[i].Location = new Point( (i - val) * (VolumeTracker.WIDTH + 1), 0 );
            }
            using ( Pen pen_062_061_062 = new Pen( Color.FromArgb( 62, 61, 62 ) ) ) {
                int x2 = (m_tracker.Length - 1) * (VolumeTracker.WIDTH + 1);
                e.Graphics.DrawLine( pen_062_061_062,
                                     new Point( x2, 0 ),
                                     new Point( x2, this.ClientSize.Height ) );
                e.Graphics.DrawLine( pen_062_061_062,
                                     new Point( x2 + 1, 0 ),
                                     new Point( x2 + 1, this.ClientSize.Height ) );
                e.Graphics.DrawLine( pen_062_061_062,
                                     new Point( x2 + 2, 0 ),
                                     new Point( x2 + 2, this.ClientSize.Height ) );
            }
        }

        private void panel1_Paint( object sender, PaintEventArgs e ) {
            using ( Pen pen_102_102_102 = new Pen( Color.FromArgb( 102, 102, 102 ) ) ) {
                for ( int i = 0; i < m_tracker.Length; i++ ) {
                    int x = (i + 1) * (VolumeTracker.WIDTH + 1);
                    e.Graphics.DrawLine(
                        pen_102_102_102,
                        new Point( x - 1, 0 ),
                        new Point( x - 1, 261 + 4 ) );
                }
            }
        }

        private void veScrollBar_ValueChanged( object sender, EventArgs e ) {
            this.Invalidate();
        }

        private void volumeMaster_FederChanged( object sender, EventArgs e ) {
            if ( FederChanged != null ) {
                FederChanged( 0, volumeMaster.getFeder() );
            }
        }

        private void volumeMaster_PanpotChanged( object sender, EventArgs e ) {
            if ( PanpotChanged != null ) {
                PanpotChanged( 0, volumeMaster.getPanpot() );
            }
        }

        private void volumeMaster_IsMutedChanged( object sender, EventArgs e ) {
            if ( MuteChanged != null ) {
                MuteChanged( 0, volumeMaster.isMuted() );
            }
        }

        private void chkTopmost_CheckedChanged( object sender, EventArgs e ) {
            if ( TopMostChanged != null ) {
                TopMostChanged( this, chkTopmost.Checked );
            }
            this.TopMost = chkTopmost.Checked; // ここはthis.ShowTopMostにしてはいけない
        }

        public boolean ShowTopMost {
            get {
                return this.TopMost;
            }
            set {
                this.TopMost = value;
                chkTopmost.Checked = value;
            }
        }

        private void registerEventHandlers() {
            this.menuVisualReturn.Click += new System.EventHandler( this.menuVisualReturn_Click );
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler( this.panel1_Paint );
            this.hScroll.ValueChanged += new System.EventHandler( this.veScrollBar_ValueChanged );
            this.volumeMaster.PanpotChanged += new System.EventHandler( this.volumeMaster_PanpotChanged );
            this.volumeMaster.IsMutedChanged += new System.EventHandler( this.volumeMaster_IsMutedChanged );
            this.volumeMaster.FederChanged += new System.EventHandler( this.volumeMaster_FederChanged );
            this.chkTopmost.CheckedChanged += new System.EventHandler( this.chkTopmost_CheckedChanged );
            this.Paint += new System.Windows.Forms.PaintEventHandler( this.FormMixer_Paint );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.FormMixer_FormClosing );
        }

        private void setResources() {
        }
#if JAVA
#else
        #region UI Impl for C#
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
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.hScroll );
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Margin = new System.Windows.Forms.Padding( 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 85, 279 );
            this.panel1.TabIndex = 6;
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
            // 
            // volumeMaster
            // 
            this.volumeMaster.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))) );
            this.volumeMaster.Location = new System.Drawing.Point( 87, 0 );
            this.volumeMaster.Margin = new System.Windows.Forms.Padding( 0 );
            this.volumeMaster.Name = "volumeMaster";
            this.volumeMaster.Size = new System.Drawing.Size( 85, 261 );
            this.volumeMaster.TabIndex = 5;
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
        #endregion
#endif
    }

}
