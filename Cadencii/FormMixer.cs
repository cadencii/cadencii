/*
 * FormMixer.cs
 * Copyright (C) 2008-2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.cadencii;

//INCLUDE-SECTION IMPORT ..\BuildJavaUI\src\org\kbinani\Cadencii\FormMixer.java

import java.util.*;
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.Drawing;
using System.Windows.Forms;
using org.kbinani.apputil;
using org.kbinani.java.util;
using org.kbinani.javax.swing;
using org.kbinani.vsq;
using org.kbinani.windows.forms;
using org.kbinani.java.awt;

namespace org.kbinani.cadencii {
    using BEventArgs = System.EventArgs;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

#if JAVA
    public class FormMixer extends BForm {
#else
    public class FormMixer : BForm {
#endif
        private FormMain m_parent;
        private Vector<VolumeTracker> m_tracker = null;

#if JAVA
        public BEvent<FederChangedEventHandler> federChangedEvent = new BEvent<FederChangedEventHandler>();
        public BEvent<PanpotChangedEventHandler> panpotChangedEvent = new BEvent<PanpotChangedEventHandler>();
        public BEvent<SoloChangedEventHandler> soloChangedEvent = new BEvent<SoloChangedEventHandler>();
        public BEvent<MuteChangedEventHandler> muteChangedEvent = new BEvent<MuteChangedEventHandler>();
        public BEvent<TopMostChangedEventHandler> topMostChangedEvent = new BEvent<TopMostChangedEventHandler>();
#else
        public event FederChangedEventHandler FederChanged;
        public event PanpotChangedEventHandler PanpotChanged;
        public event SoloChangedEventHandler SoloChanged;
        public event MuteChangedEventHandler MuteChanged;
        public event TopMostChangedEventHandler TopMostChanged;
#endif

        public void updateSoloMute() {
#if DEBUG
            PortUtil.println( "FormMixer#updateSoloMute" );
#endif
            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq == null ) {
                return;
            }
            volumeMaster.setMuted( vsq.getMasterMute() );
            boolean soloSpecificationExists = false; // 1トラックでもソロ指定があればtrue
            for ( int i = 1; i < vsq.Track.size(); i++ ) {
                if ( vsq.getSolo( i ) ) {
                    soloSpecificationExists = true;
                    break;
                }
            }
            for ( int i = 0; i < m_tracker.size(); i++ ) {
                if ( soloSpecificationExists ) {
                    if ( vsq.getSolo( i + 1 ) ) {
                        m_tracker.get( i ).setSolo( true );
                        m_tracker.get( i ).setMuted( vsq.getMute( i + 1 ) );
                    } else {
                        m_tracker.get( i ).setSolo( false );
                        m_tracker.get( i ).setMuted( true );
                    }
                } else {
                    m_tracker.get( i ).setSolo( vsq.getSolo( i + 1 ) );
                    m_tracker.get( i ).setMuted( vsq.getMute( i + 1 ) );
                }
            }
        }

        public void applyShortcut( KeyStroke shortcut ) {
            menuVisualReturn.setAccelerator( shortcut );
        }

        public void applyLanguage() {
            setTitle( _( "Mixer" ) );
            chkTopmost.setText( _( "Top Most" ) );
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public void updateStatus() {
            VsqFileEx vsq = AppManager.getVsqFile();
            int num = vsq.Mixer.Slave.size() + AppManager.getBgmCount();
#if DEBUG
            PortUtil.println( "FormMixer#UpdateStatus; num=" + num );
#endif
#if !OLD_IMPL
            if ( m_tracker == null ) {
                m_tracker = new Vector<VolumeTracker>();
            }
            if ( m_tracker.size() < num ) {
                int remain = num - m_tracker.size();
                for ( int i = 0; i < remain; i++ ) {
                    VolumeTracker item = new VolumeTracker();
                    item.muteButtonClick.add( new BEventHandler( this, "FormMixer_MuteButtonClick" ) );
                    item.soloButtonClick.add( new BEventHandler( this, "FormMixer_SoloButtonClick" ) );
                    item.federChangedEvent.add( new BEventHandler( this, "FormMixer_FederChanged" ) );
                    item.panpotChangedEvent.add( new BEventHandler( this, "FormMixer_PanpotChanged" ) );
                    m_tracker.add( item );
                }
            } else if ( m_tracker.size() > num ) {
                int delete = m_tracker.size() - num;
                for ( int i = 0; i < delete; i++ ) {
                    m_tracker.removeElementAt( m_tracker.size() - 1 );
                }
            }
#else
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
#endif

            // 同時に表示できるVolumeTrackerの個数を計算
            int max = PortUtil.getWorkingArea( this ).width;
            int bordersize = 4;// TODO: ここもともとは SystemInformation.FrameBorderSize;だった
            int max_client_width = max - 2 * bordersize;
            int max_num = (int)Math.Floor( max_client_width / (VolumeTracker.WIDTH + 1.0f) );
            num++;

            int screen_num = num <= max_num ? num : max_num; //スクリーン上に表示するVolumeTrackerの個数

            // panel1上に配置するVolumeTrackerの個数
            int num_vtracker_on_panel = vsq.Mixer.Slave.size() + AppManager.getBgmCount();
            // panel1上に配置可能なVolumeTrackerの個数
            int panel_capacity = max_num - 1;

            if ( panel_capacity >= num_vtracker_on_panel ) {
                // volumeMaster以外の全てのVolumeTrackerを，画面上に同時表示可能
                hScroll.setMinimum( 0 );
                hScroll.setValue( 0 );
                hScroll.setMaximum( 0 );
                hScroll.setVisibleAmount( 1 );
                hScroll.setPreferredSize( new Dimension( VolumeTracker.WIDTH * num_vtracker_on_panel, hScroll.getHeight() ) );
            } else {
                // num_vtracker_on_panel個のVolumeTrackerのうち，panel_capacity個しか，画面上に同時表示できない
                hScroll.setMinimum( 0 );
                hScroll.setValue( 0 );
                hScroll.setMaximum( num_vtracker_on_panel - 1 );
                hScroll.setVisibleAmount( panel_capacity );
                hScroll.setPreferredSize( new Dimension( VolumeTracker.WIDTH * panel_capacity, hScroll.getHeight() ) );
            }
            hScroll.setLocation( 0, VolumeTracker.HEIGHT );

#if DEBUG
            AppManager.debugWriteLine( "FormMixer#updateStatus;" );
            AppManager.debugWriteLine( "    num_vtracker_on_panel=" + num_vtracker_on_panel );
            AppManager.debugWriteLine( "    panel_capacity=" + panel_capacity );
            AppManager.debugWriteLine( "    hScroll.Maximum=" + hScroll.getMaximum() );
            AppManager.debugWriteLine( "    hScroll.LargeChange=" + hScroll.getVisibleAmount() );
#endif

            int j = -1;
            for ( Iterator itr = vsq.Mixer.Slave.iterator(); itr.hasNext(); ) {
                VsqMixerEntry vme = (VsqMixerEntry)itr.next();
                j++;
                //m_tracker[j] = new VolumeTracker();
                m_tracker[j].setFeder( vme.Feder );
                m_tracker[j].setPanpot( vme.Panpot );
                m_tracker[j].setTitle( vsq.Track.get( j + 1 ).getName() );
                m_tracker[j].setNumber( (j + 1) + "" );
#if !JAVA
                m_tracker[j].Location = new System.Drawing.Point( j * (VolumeTracker.WIDTH + 1), 0 );
#endif
                m_tracker[j].setSoloButtonVisible( true );
                m_tracker[j].setMuted( (vme.Mute == 1) );
                m_tracker[j].setSolo( (vme.Solo == 1) );
                m_tracker[j].setTag( j + 1 );
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
                m_tracker[j].setTitle( PortUtil.getFileName( item.file ) );
                m_tracker[j].setNumber( "" );
#if !JAVA
                m_tracker[j].Location = new System.Drawing.Point( j * (VolumeTracker.WIDTH + 1), 0 );
#endif
                m_tracker[j].setSoloButtonVisible( false );
                m_tracker[j].setMuted( (item.mute == 1) );
                m_tracker[j].setSolo( false );
                m_tracker[j].setTag( (int)(-i - 1) );
                m_tracker[j].setSoloButtonVisible( true );
#if JAVA
#else
                panel1.Controls.Add( m_tracker[j] );
#endif
            }
            volumeMaster.setFeder( vsq.Mixer.MasterFeder );
            volumeMaster.setPanpot( vsq.Mixer.MasterPanpot );
            volumeMaster.setSoloButtonVisible( false );

#if JAVA
#else
            panel1.Width = (VolumeTracker.WIDTH + 1) * (screen_num - 1);
            volumeMaster.Location = new System.Drawing.Point( (screen_num - 1) * (VolumeTracker.WIDTH + 1) + 3, 0 );
            chkTopmost.Left = panel1.Width;
            this.MaximumSize = Size.Empty;
            this.MinimumSize = Size.Empty;
            this.ClientSize = new Size( screen_num * (VolumeTracker.WIDTH + 1) + 3, VolumeTracker.HEIGHT + hScroll.Height );
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.Invalidate();
            m_parent.requestFocusInWindow();
#endif
        }

        public void FormMixer_PanpotChanged( Object sender, BEventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (Integer)parent.getTag();
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

        public void FormMixer_FederChanged( Object sender, BEventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (Integer)parent.getTag();
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

        public void FormMixer_SoloButtonClick( Object sender, BEventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (Integer)parent.getTag();
#if JAVA
            try{
                soloChangedEvent.raise( track, parent.isSolo() );
            }catch( Exception ex ){
                System.err.println( "FormMixer#FormMixer_IsSoloChanged; ex=" + ex );
            }
#else
            if ( SoloChanged != null ) {
                SoloChanged( track, parent.isSolo() );
            }
#endif
            updateSoloMute();
        }

        public void FormMixer_MuteButtonClick( Object sender, BEventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (Integer)parent.getTag();
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
            updateSoloMute();
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
            using ( Pen pen_102_102_102 = new Pen( System.Drawing.Color.FromArgb( 102, 102, 102 ) ) ) {
                for ( int i = 0; i < m_tracker.size(); i++ ) {
                    int x = (i + 1) * (VolumeTracker.WIDTH + 1);
                    e.Graphics.DrawLine(
                        pen_102_102_102,
                        new System.Drawing.Point( x - 1, 0 ),
                        new System.Drawing.Point( x - 1, 261 + 4 ) );
                }
            }
        }
#endif

        private void veScrollBar_ValueChanged( Object sender, BEventArgs e ) {
            this.invalidate();
        }

        public void volumeMaster_FederChanged( Object sender, BEventArgs e ) {
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

        public void volumeMaster_PanpotChanged( Object sender, BEventArgs e ) {
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

        public void volumeMaster_MuteButtonClick( Object sender, BEventArgs e ) {
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
                topMostChangedEvent.raise( this, chkTopmost.isSelected() );
            }catch( Exception ex ){
                System.err.println( "FormMixer#chkTopmost_CheckedChanged; ex=" + ex );
            }
#else
            if ( TopMostChanged != null ) {
                TopMostChanged( this, chkTopmost.isSelected() );
            }
#endif
            setAlwaysOnTop( chkTopmost.isSelected() ); // ここはthis.ShowTopMostにしてはいけない
        }

        public boolean isShowTopMost() {
            return isAlwaysOnTop();
        }

        public void setShowTopMost( boolean value ) {
            setAlwaysOnTop( value );
            chkTopmost.setSelected( value );
        }

        private void registerEventHandlers() {
#if JAVA
            menuVisualReturn.clickEvent.add( new BEventHandler( this, "menuVisualReturn_Click" ) );
//            panel1.Paint += new System.Windows.Forms.PaintEventHandler( this.panel1_Paint );
            hScroll.valueChangedEvent.add( new BEventHandler( this, "veScrollBar_ValueChanged" ) );
            volumeMaster.panpotChangedEvent.add( new BEventHandler( this, "volumeMaster_PanpotChanged" ) );
            volumeMaster.federChangedEvent.add( new BEventHandler( this, "volumeMaster_FederChanged" ) );
            chkTopmost.checkedChangedEvent.add( new BEventHandler( this, "chkTopmost_CheckedChanged" ) );
            formClosingEvent.add( new BFormClosingEventHandler( this, "FormMixer_FormClosing" ) );
#else
            this.menuVisualReturn.Click += new System.EventHandler( this.menuVisualReturn_Click );
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler( this.panel1_Paint );
            this.hScroll.ValueChanged += new System.EventHandler( this.veScrollBar_ValueChanged );
            this.volumeMaster.panpotChangedEvent.add( new BEventHandler( this, "volumeMaster_PanpotChanged" ) );
            this.volumeMaster.federChangedEvent.add( new BEventHandler( this, "volumeMaster_FederChanged" ) );
            this.chkTopmost.CheckedChanged += new System.EventHandler( this.chkTopmost_CheckedChanged );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.FormMixer_FormClosing );
#endif
            volumeMaster.muteButtonClick.add( new BEventHandler( this, "volumeMaster_MuteButtonClick" ) );
        }

        private void setResources() {
            setIconImage( Resources.get_icon() );
        }

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ..\BuildJavaUI\src\org\kbinani\Cadencii\FormMixer.java
        //INCLUDE-SECTION METHOD ..\BuildJavaUI\src\org\kbinani\Cadencii\FormMixer.java
        #endregion
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
            this.menuMain = new org.kbinani.windows.forms.BMenuBar();
            this.menuVisual = new org.kbinani.windows.forms.BMenuItem();
            this.menuVisualReturn = new org.kbinani.windows.forms.BMenuItem();
            this.panel1 = new org.kbinani.windows.forms.BPanel();
            this.hScroll = new org.kbinani.cadencii.HScroll();
            this.chkTopmost = new org.kbinani.windows.forms.BCheckBox();
            this.volumeMaster = new org.kbinani.cadencii.VolumeTracker();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuVisual} );
            this.menuMain.Location = new System.Drawing.Point( 0, 0 );
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size( 170, 26 );
            this.menuMain.TabIndex = 1;
            this.menuMain.Text = "menuStrip1";
            this.menuMain.Visible = false;
            // 
            // menuVisual
            // 
            this.menuVisual.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuVisualReturn} );
            this.menuVisual.Name = "menuVisual";
            this.menuVisual.Size = new System.Drawing.Size( 62, 22 );
            this.menuVisual.Text = "表示(&V)";
            // 
            // menuVisualReturn
            // 
            this.menuVisualReturn.Name = "menuVisualReturn";
            this.menuVisualReturn.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.menuVisualReturn.Size = new System.Drawing.Size( 206, 22 );
            this.menuVisualReturn.Text = "エディタ画面へ戻る";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Margin = new System.Windows.Forms.Padding( 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 85, 284 );
            this.panel1.TabIndex = 6;
            // 
            // hScroll
            // 
            this.hScroll.LargeChange = 2;
            this.hScroll.Location = new System.Drawing.Point( 0, 284 );
            this.hScroll.Maximum = 1;
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new System.Drawing.Size( 85, 19 );
            this.hScroll.TabIndex = 0;
            // 
            // chkTopmost
            // 
            this.chkTopmost.Location = new System.Drawing.Point( 85, 284 );
            this.chkTopmost.Margin = new System.Windows.Forms.Padding( 0 );
            this.chkTopmost.Name = "chkTopmost";
            this.chkTopmost.Size = new System.Drawing.Size( 85, 19 );
            this.chkTopmost.TabIndex = 7;
            this.chkTopmost.Text = "Top Most";
            this.chkTopmost.UseVisualStyleBackColor = true;
            // 
            // volumeMaster
            // 
            this.volumeMaster.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))) );
            this.volumeMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.volumeMaster.Location = new System.Drawing.Point( 85, 0 );
            this.volumeMaster.Margin = new System.Windows.Forms.Padding( 0 );
            this.volumeMaster.Name = "volumeMaster";
            this.volumeMaster.Size = new System.Drawing.Size( 85, 284 );
            this.volumeMaster.TabIndex = 5;
            // 
            // FormMixer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))) );
            this.ClientSize = new System.Drawing.Size( 170, 304 );
            this.Controls.Add( this.hScroll );
            this.Controls.Add( this.panel1 );
            this.Controls.Add( this.chkTopmost );
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
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BMenuBar menuMain;
        private BMenuItem menuVisual;
        private BMenuItem menuVisualReturn;
        private VolumeTracker volumeMaster;
        private BPanel panel1;
        private HScroll hScroll;
        private BCheckBox chkTopmost;
        #endregion
#endif
    }

#if !JAVA
}
#endif
