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

import java.awt.*;
import java.io.*;
import java.util.*;
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.util;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using boolean = System.Boolean;
    using BEventArgs = System.EventArgs;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
#endif

#if JAVA
    public class FormMixer extends BForm {
#else
    public class FormMixer : BForm {
#endif
        private FormMain m_parent;
        private VolumeTracker[] m_tracker;

#if JAVA
        public BEvent<FederChangedEventHandler> federChangedEvent = new BEvent<FederChangedEventHandler>();
        public BEvent<PanpotChangedEventHandler> panpotChangedEvent = new BEvent<PanpotChangedEventHandler>();
        public BEvent<SoloChangedEventHandler> soloChanged = new BEvent<SoloChangedEventHandler>();
        public BEvent<MuteChangedEventHandler> muteChanged = new BEvent<MuteChangedEventHandler>();
        public BEvent<TopMostChangedEventHandler> topMostChanged = new BEvent<TopMostChangedEventHandler>();
#else
        public event FederChangedEventHandler FederChanged;
        public event PanpotChangedEventHandler PanpotChanged;
        public event SoloChangedEventHandler SoloChanged;
        public event MuteChangedEventHandler MuteChanged;
        public event TopMostChangedEventHandler TopMostChanged;
#endif

        public void applyLanguage() {
            setTitle( _( "Mixer" ) );
            chkTopmost.setText( _( "Top Most" ) );
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
#if !JAVA
                    m_tracker[i].Dispose();
#endif
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
                hScroll.setMinimum( 0 );
                hScroll.setValue( 0 );
                hScroll.setMaximum( 0 );
                hScroll.setVisibleAmount( 1 );
            } else {
                // num_vtracker_on_panel個のVolumeTrackerのうち，panel_capacity個しか，画面上に同時表示できない
                hScroll.setMinimum( 0 );
                hScroll.setValue( 0 );
                hScroll.setMaximum( num_vtracker_on_panel - 1 );
                hScroll.setVisibleAmount( panel_capacity );
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
#if !JAVA
                m_tracker[j].Location = new Point( j * (VolumeTracker.WIDTH + 1), 0 );
#endif
                m_tracker[j].setSoloButtonVisible( true );
                m_tracker[j].setMuted( (vme.Mute == 1) );
                m_tracker[j].setSolo( (vme.Solo == 1) );
                m_tracker[j].setTag( j + 1 );
#if JAVA
                m_tracker[j].isMutedChangedEvent.add( new BEventHandler( this, "FormMixer_IsMutedChanged" ) );
                m_tracker[j].isSoloChangedEvent.add( new BEventHandler( this, "FormMixer_IsSoloChanged" ) );
                m_tracker[j].federChangedEvent.add( new BEventHandler( this, "FormMixer_FederChanged" ) );
                m_tracker[j].panpotChangedEvent.add( new BEventHandler( this, "FormMixer_PanpotChanged" ) );
#else
                m_tracker[j].IsMutedChanged += new EventHandler( FormMixer_IsMutedChanged );
                m_tracker[j].IsSoloChanged += new EventHandler( FormMixer_IsSoloChanged );
                m_tracker[j].FederChanged += new EventHandler( FormMixer_FederChanged );
                m_tracker[j].PanpotChanged += new EventHandler( FormMixer_PanpotChanged );
#endif

#if JAVA
#else
                panel1.Controls.Add( m_tracker[j] );
#endif
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
#if !JAVA
                m_tracker[j].Location = new Point( j * (VolumeTracker.WIDTH + 1), 0 );
#endif
                m_tracker[j].setSoloButtonVisible( false );
                m_tracker[j].setMuted( (item.mute == 1) );
                m_tracker[j].setSolo( false );
                m_tracker[j].setTag( (int)(-i - 1) );
#if JAVA
                m_tracker[j].isMutedChangedEvent.add( new BEventHandler( this, "FormMixer_IsMutedChanged" ) );
                //m_tracker[j].isSoloChangedEvent.add( new BEventHandler( this, "FormMixer_IsSoloChanged" ) );
                m_tracker[j].federChangedEvent.add( new BEventHandler( this, "FormMixer_FederChanged" ) );
                m_tracker[j].panpotChangedEvent.add( new BEventHandler( this, "FormMixer_PanpotChanged" ) );
#else
                m_tracker[j].IsMutedChanged += new EventHandler( FormMixer_IsMutedChanged );
                //m_tracker[j].IsSoloChanged += new EventHandler( FormMixer_IsSoloChanged );
                m_tracker[j].FederChanged += new EventHandler( FormMixer_FederChanged );
                m_tracker[j].PanpotChanged += new EventHandler( FormMixer_PanpotChanged );
#endif

#if JAVA
#else
                panel1.Controls.Add( m_tracker[j] );
#endif
            }
            volumeMaster.setFeder( AppManager.getVsqFile().Mixer.MasterFeder );
            volumeMaster.setPanpot( AppManager.getVsqFile().Mixer.MasterPanpot );

#if JAVA
#else
            panel1.Width = (VolumeTracker.WIDTH + 1) * (screen_num - 1);
            volumeMaster.Location = new Point( (screen_num - 1) * (VolumeTracker.WIDTH + 1) + 3, 0 );
            chkTopmost.Left = panel1.Width;
            this.MaximumSize = Size.Empty;
            this.MinimumSize = Size.Empty;
            this.ClientSize = new Size( screen_num * (VolumeTracker.WIDTH + 1) + 3, 279 );
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.Invalidate();
            m_parent.requestFocusInWindow();
#endif
        }

        private void FormMixer_PanpotChanged( Object sender, BEventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (int)parent.getTag();
#if JAVA
            try{
                panpotChangedEvent.raise( track, parent.getPanpot() );
            }catch( Exception ex ){
                System.err.println( "FormMixer#FormMixer_PanpotChanged; ex=" + ex );
            }
#else
            if ( PanpotChanged != null ) {
                PanpotChanged( track, parent.getPanpot() );
            }
#endif
        }

        private void FormMixer_FederChanged( Object sender, BEventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (int)parent.getTag();
#if JAVA
            try{
                federChangedEvent.raise( track, parent.getFeder() );
            }catch( Exception ex ){
                System.err.println( "FormMixer#FormMixer_FederChanged; ex=" + ex );
            }
#else
            if ( FederChanged != null ) {
                FederChanged( track, parent.getFeder() );
            }
#endif
        }

        private void FormMixer_IsSoloChanged( Object sender, BEventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (int)parent.getTag();
#if JAVA
            try{
                soloChangedEvent.raise( track, parent.iSolo() );
            }catch( Exception ex ){
                System.err.println( "FormMixer#FormMixer_IsSoloChanged; ex=" + ex );
            }
#else
            if ( SoloChanged != null ) {
                SoloChanged( track, parent.isSolo() );
            }
#endif
        }

        private void FormMixer_IsMutedChanged( Object sender, BEventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (int)parent.getTag();
#if JAVA
            try{
                muteChangedEvent.raise( track, parent.isMuted() );
            }catch( Exception ex ){
                System.err.println( "FormMixer#FormMixer_IsMutedChanged; ex=" + ex );
            }
#else
            if ( MuteChanged != null ) {
                MuteChanged( track, parent.isMuted() );
            }
#endif
        }

        public FormMixer( FormMain parent ) {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            volumeMaster.setFeder( 0 );
            volumeMaster.setMuted( false );
            volumeMaster.setSolo( true );
            volumeMaster.setNumber( "Master" );
            volumeMaster.setPanpot( 0 );
            volumeMaster.setSoloButtonVisible( false );
            volumeMaster.setTitle( "" );
            applyLanguage();
            m_parent = parent;
#if !JAVA
            this.SetStyle( ControlStyles.DoubleBuffer, true );
#endif
        }

        private void menuVisualReturn_Click( Object sender, BEventArgs e ) {
            m_parent.flipMixerDialogVisible( false );
        }

        private void FormMixer_FormClosing( Object sender, BFormClosingEventArgs e ) {
            m_parent.flipMixerDialogVisible( false );
            e.Cancel = true;
        }

#if !JAVA
        private void panel1_Paint( Object sender, PaintEventArgs e ) {
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
#endif

        private void veScrollBar_ValueChanged( Object sender, BEventArgs e ) {
            this.Invalidate();
        }

        private void volumeMaster_FederChanged( Object sender, BEventArgs e ) {
#if JAVA
            try{
                federChangedEvent.raise( 0, volumeMaster.getFeder() );
            }catch( Exception ex ){
                System.err.println( "FormMixer#volumeMaster_FederChanged; ex=" + ex );
            }
#else
            if ( FederChanged != null ) {
                FederChanged( 0, volumeMaster.getFeder() );
            }
#endif
        }

        private void volumeMaster_PanpotChanged( Object sender, BEventArgs e ) {
#if JAVA
            try{
                panpotChangedEvent.raise( 0, volumeMaster.getPanpot() );
            }catch( Exception ex ){
                System.err.println( "FormMixer#volumeMaster_PanpotChanged; ex=" + ex );
            }
#else
            if ( PanpotChanged != null ) {
                PanpotChanged( 0, volumeMaster.getPanpot() );
            }
#endif
        }

        private void volumeMaster_IsMutedChanged( Object sender, BEventArgs e ) {
#if JAVA
            try{
                muteChangedEvent.raise( 0, volumeMaster.isMuted() );
            }catch( Exception ex ){
                System.err.println( "FormMixer#volumeMaster_IsMutedChanged; ex=" + ex );
            }
#else
            if ( MuteChanged != null ) {
                MuteChanged( 0, volumeMaster.isMuted() );
            }
#endif
        }

        private void chkTopmost_CheckedChanged( Object sender, BEventArgs e ) {
#if JAVA
            try{
                topMostChangedEvent.raise( this, chkTopMost.isSelected() );
            }catch( Exception ex ){
                System.err.println( "FormMixer#chkTopmost_CheckedChanged; ex=" + ex );
            }
#else
            if ( TopMostChanged != null ) {
                TopMostChanged( this, chkTopmost.Checked );
            }
#endif
            this.TopMost = chkTopmost.isSelected(); // ここはthis.ShowTopMostにしてはいけない
        }

        public boolean isShowTopMost() {
#if JAVA
            return false;
#else
            return this.TopMost;
#endif
        }

        public void setShowTopMost( boolean value ) {
#if JAVA
#else
            this.TopMost = value;
            chkTopmost.setSelected( value );
#endif
        }

        private void registerEventHandlers() {
#if JAVA
            menuVisualReturn.clickEvent.add( new BEventHandler( this, "menuVisualReturn_Click" ) );
//            panel1.Paint += new System.Windows.Forms.PaintEventHandler( this.panel1_Paint );
            hScroll.ValueChanged += new System.EventHandler( this.veScrollBar_ValueChanged );
            volumeMaster.panpotChangedEvent.add( new BEventHandler( this, "volumeMaster_PanpotChanged" ) );
            volumeMaster.isMutedChangedEvent.add( new BEventHandler( this, "volumeMaster_IsMutedChanged" ) );
            volumeMaster.federChangedEvent.add( new BEventHandler( this, "volumeMaster_FederChanged" ) );
            chkTopmost.checkedChangedEvent.add( new BEventHandler( this, "chkTopmost_CheckedChanged" ) );
            formClosingEvent.add( new BFormClosingEventHandler( this, "FormMixer_FormClosing" ) );
#else
            this.menuVisualReturn.Click += new System.EventHandler( this.menuVisualReturn_Click );
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler( this.panel1_Paint );
            this.hScroll.ValueChanged += new System.EventHandler( this.veScrollBar_ValueChanged );
            this.volumeMaster.PanpotChanged += new System.EventHandler( this.volumeMaster_PanpotChanged );
            this.volumeMaster.IsMutedChanged += new System.EventHandler( this.volumeMaster_IsMutedChanged );
            this.volumeMaster.FederChanged += new System.EventHandler( this.volumeMaster_FederChanged );
            this.chkTopmost.CheckedChanged += new System.EventHandler( this.chkTopmost_CheckedChanged );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.FormMixer_FormClosing );
#endif
        }

        private void setResources() {
            setIconImage( Resources.get_icon() );
        }
#if JAVA
	    private JPanel jContentPane = null;
	    private JPanel panel1 = null;
	    private JScrollBar hScroll = null;
	    private VolumeTracker volumeMaster = null;
	    private JCheckBox chkTopmost = null;

	    /**
	     * This method initializes this
	     * 
	     * @return void
	     */
	    private void initialize() {
		    this.setSize(377, 653);
		    this.setContentPane(getJContentPane());
		    this.setTitle("JFrame");
	    }

	    /**
	     * This method initializes jContentPane
	     * 
	     * @return javax.swing.JPanel
	     */
	    private JPanel getJContentPane() {
		    if (jContentPane == null) {
			    GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			    gridBagConstraints5.gridx = 1;
			    gridBagConstraints5.weightx = 0.0D;
			    gridBagConstraints5.anchor = GridBagConstraints.SOUTH;
			    gridBagConstraints5.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints5.gridy = 1;
			    GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			    gridBagConstraints4.gridx = 1;
			    gridBagConstraints4.weightx = 0.0D;
			    gridBagConstraints4.weighty = 1.0D;
			    gridBagConstraints4.fill = GridBagConstraints.BOTH;
			    gridBagConstraints4.anchor = GridBagConstraints.NORTH;
			    gridBagConstraints4.gridy = 0;
			    GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			    gridBagConstraints3.gridx = 0;
			    gridBagConstraints3.fill = GridBagConstraints.BOTH;
			    gridBagConstraints3.weightx = 1.0D;
			    gridBagConstraints3.weighty = 1.0D;
			    gridBagConstraints3.gridy = 0;
			    GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			    gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints1.gridy = 1;
			    gridBagConstraints1.weighty = 0.0D;
			    gridBagConstraints1.anchor = GridBagConstraints.SOUTH;
			    gridBagConstraints1.weightx = 1.0D;
			    gridBagConstraints1.gridx = 0;
			    GridBagConstraints gridBagConstraints = new GridBagConstraints();
			    gridBagConstraints.gridx = 0;
			    gridBagConstraints.weighty = 1.0D;
			    gridBagConstraints.fill = GridBagConstraints.VERTICAL;
			    gridBagConstraints.anchor = GridBagConstraints.NORTH;
			    gridBagConstraints.gridy = 0;
			    jContentPane = new JPanel();
			    jContentPane.setLayout(new GridBagLayout());
			    jContentPane.add(getHScroll(), gridBagConstraints1);
			    jContentPane.add(getPanel1(), gridBagConstraints3);
			    jContentPane.add(getVolumeMaster(), gridBagConstraints4);
			    jContentPane.add(getChkTopmost(), gridBagConstraints5);
		    }
		    return jContentPane;
	    }

	    /**
	     * This method initializes panel1	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getPanel1() {
		    if (panel1 == null) {
			    panel1 = new JPanel();
			    panel1.setLayout(new GridBagLayout());
		    }
		    return panel1;
	    }

	    /**
	     * This method initializes hScroll	
	     * 	
	     * @return javax.swing.JScrollBar	
	     */
	    private JScrollBar getHScroll() {
		    if (hScroll == null) {
			    hScroll = new JScrollBar();
			    hScroll.setOrientation(JScrollBar.HORIZONTAL);
		    }
		    return hScroll;
	    }

	    /**
	     * This method initializes volumeMaster	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getVolumeMaster() {
		    if (volumeMaster == null) {
			    volumeMaster = new VolumeTracker();
		    }
		    return volumeMaster;
	    }

	    /**
	     * This method initializes chkTopmost	
	     * 	
	     * @return javax.swing.JCheckBox	
	     */
	    private JCheckBox getChkTopmost() {
		    if (chkTopmost == null) {
			    chkTopmost = new JCheckBox();
			    chkTopmost.setText("Top most");
			    chkTopmost.setText("Top most");
		    }
		    return chkTopmost;
	    }
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
            this.menuMain = new BMenuBar();
            this.menuVisual = new BMenuItem();
            this.menuVisualReturn = new BMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.hScroll = new bocoree.windows.forms.BHScrollBar();
            this.volumeMaster = new Boare.Cadencii.VolumeTracker();
            this.chkTopmost = new BCheckBox();
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
            this.hScroll.LargeChange = 2;
            this.hScroll.Location = new System.Drawing.Point( 0, 260 );
            this.hScroll.Maximum = 1;
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new System.Drawing.Size( 85, 19 );
            this.hScroll.TabIndex = 0;
            // 
            // volumeMaster
            // 
            this.volumeMaster.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))) );
            this.volumeMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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

        private BMenuBar menuMain;
        private BMenuItem menuVisual;
        private BMenuItem menuVisualReturn;
        private VolumeTracker volumeMaster;
        private System.Windows.Forms.Panel panel1;
        private bocoree.windows.forms.BHScrollBar hScroll;
        private BCheckBox chkTopmost;
        #endregion
#endif
    }

#if !JAVA
}
#endif
