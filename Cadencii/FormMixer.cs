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

import java.awt.*;
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
using org.kbinani.java.awt;
using org.kbinani.java.util;
using org.kbinani.javax.swing;
using org.kbinani.vsq;
using org.kbinani.windows.forms;

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

        #region public methods
        public void updateSoloMute() {
#if DEBUG
            PortUtil.println( "FormMixer#updateSoloMute" );
#endif
            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq == null ) {
                return;
            }
            // マスター
            boolean masterMuted = vsq.getMasterMute();
            volumeMaster.setMuted( masterMuted );

            // VSQのトラック
            boolean soloSpecificationExists = false; // 1トラックでもソロ指定があればtrue
            for ( int i = 1; i < vsq.Track.size(); i++ ) {
                if ( vsq.getSolo( i ) ) {
                    soloSpecificationExists = true;
                    break;
                }
            }
            for( int track = 1; track < vsq.Track.size(); track++ ){
                if ( soloSpecificationExists ) {
                    if ( vsq.getSolo( track ) ) {
                        m_tracker.get( track - 1 ).setSolo( true );
                        m_tracker.get( track - 1 ).setMuted( masterMuted ? true : vsq.getMute( track ) );
                    } else {
                        m_tracker.get( track - 1 ).setSolo( false );
                        m_tracker.get( track - 1 ).setMuted( true );
                    }
                } else {
                    m_tracker.get( track - 1 ).setSolo( vsq.getSolo( track ) );
                    m_tracker.get( track - 1 ).setMuted( masterMuted ? true : vsq.getMute( track ) );
                }
            }

            // BGM
            int offset = vsq.Track.size() - 1;
            for ( int i = 0; i < vsq.BgmFiles.size(); i++ ) {
                m_tracker.get( offset + i ).setMuted( masterMuted ? true : vsq.BgmFiles.get( i ).mute == 1 );
            }
        }

        public void applyShortcut( KeyStroke shortcut ) {
            menuVisualReturn.setAccelerator( shortcut );
        }

        public void applyLanguage() {
            setTitle( _( "Mixer" ) );
            chkTopmost.setText( _( "Top Most" ) );
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
                hScroll.setPreferredSize( new Dimension( (VolumeTracker.WIDTH + 1) * num_vtracker_on_panel, hScroll.getHeight() ) );
            } else {
                // num_vtracker_on_panel個のVolumeTrackerのうち，panel_capacity個しか，画面上に同時表示できない
                hScroll.setMinimum( 0 );
                hScroll.setValue( 0 );
                hScroll.setMaximum( num_vtracker_on_panel * VolumeTracker.WIDTH );
                hScroll.setVisibleAmount( panel_capacity * VolumeTracker.WIDTH );
                hScroll.setPreferredSize( new Dimension( (VolumeTracker.WIDTH + 1) * panel_capacity, hScroll.getHeight() ) );
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
            for ( Iterator<VsqMixerEntry> itr = vsq.Mixer.Slave.iterator(); itr.hasNext(); ) {
                VsqMixerEntry vme = itr.next();
                j++;
                VolumeTracker tracker = m_tracker.get( j );
                tracker.setFeder( vme.Feder );
                tracker.setPanpot( vme.Panpot );
                tracker.setTitle( vsq.Track.get( j + 1 ).getName() );
                tracker.setNumber( (j + 1) + "" );
                tracker.setLocation( j * (VolumeTracker.WIDTH + 1), 0 );
                tracker.setSoloButtonVisible( true );
                tracker.setMuted( (vme.Mute == 1) );
                tracker.setSolo( (vme.Solo == 1) );
                tracker.setTag( j + 1 );
                tracker.setSoloButtonVisible( true );
#if JAVA
#else
                panel1.Controls.Add( tracker );
#endif
            }
            int count = AppManager.getBgmCount();
            for ( int i = 0; i < count; i++ ) {
                j++;
                BgmFile item = AppManager.getBgm( i );
                VolumeTracker tracker = m_tracker.get( j );
                tracker.setFeder( item.feder );
                tracker.setPanpot( item.panpot );
                tracker.setTitle( PortUtil.getFileName( item.file ) );
                tracker.setNumber( "" );
                tracker.setLocation( j * (VolumeTracker.WIDTH + 1), 0 );
                tracker.setSoloButtonVisible( false );
                tracker.setMuted( (item.mute == 1) );
                tracker.setSolo( false );
                tracker.setTag( (int)(-i - 1) );
                tracker.setSoloButtonVisible( false );
#if JAVA
#else
                panel1.Controls.Add( tracker );
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

        public void setShowTopMost( boolean value ) {
            setAlwaysOnTop( value );
            chkTopmost.setSelected( value );
        }
        #endregion

        #region helper methods
        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void registerEventHandlers() {
            menuVisualReturn.clickEvent.add( new BEventHandler( this, "menuVisualReturn_Click" ) );
#if JAVA
            //TODO: fixme: FormMixer#registerEventHandlers; paint event handler for panel1
#else
            panel1.Paint += new System.Windows.Forms.PaintEventHandler( this.panel1_Paint );
#endif
            hScroll.valueChangedEvent.add( new BEventHandler( this, "veScrollBar_ValueChanged" ) );
            volumeMaster.panpotChangedEvent.add( new BEventHandler( this, "volumeMaster_PanpotChanged" ) );
            volumeMaster.federChangedEvent.add( new BEventHandler( this, "volumeMaster_FederChanged" ) );
            chkTopmost.checkedChangedEvent.add( new BEventHandler( this, "chkTopmost_CheckedChanged" ) );
            formClosingEvent.add( new BFormClosingEventHandler( this, "FormMixer_FormClosing" ) );
            volumeMaster.muteButtonClick.add( new BEventHandler( this, "volumeMaster_MuteButtonClick" ) );
        }

        private void setResources() {
            setIconImage( Resources.get_icon() );
        }
        #endregion

        #region event handlers
        public void FormMixer_PanpotChanged( Object sender, BEventArgs e ) {
            if ( sender == null ) return;
            if ( !(sender is VolumeTracker) ) return;
            VolumeTracker parent = (VolumeTracker)sender;
            Object tag = parent.getTag();
            if ( tag == null ) return;
            if ( !(tag is Integer) ) return;
            int track = (Integer)tag;
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
            if ( sender == null ) return;
            if ( !(sender is VolumeTracker) ) return;
            VolumeTracker parent = (VolumeTracker)sender;
            Object tag = parent.getTag();
            if ( tag == null ) return;
            if ( !(tag is Integer) ) return;
            int track = (Integer)tag;
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

        public void menuVisualReturn_Click( Object sender, BEventArgs e ) {
            m_parent.flipMixerDialogVisible( false );
        }

        public void FormMixer_FormClosing( Object sender, BFormClosingEventArgs e ) {
            m_parent.flipMixerDialogVisible( false );
            e.Cancel = true;
        }

#if !JAVA
        public void panel1_Paint( Object sender, PaintEventArgs e ) {
            int stdx = hScroll.getValue();
            using ( Pen pen_102_102_102 = new Pen( System.Drawing.Color.FromArgb( 102, 102, 102 ) ) ) {
                for ( int i = 0; i < m_tracker.size(); i++ ) {
                    int x = -stdx + (i + 1) * (VolumeTracker.WIDTH + 1);
                    e.Graphics.DrawLine(
                        pen_102_102_102,
                        new System.Drawing.Point( x - 1, 0 ),
                        new System.Drawing.Point( x - 1, 261 + 4 ) );
                }
            }
        }
#endif

        public void veScrollBar_ValueChanged( Object sender, BEventArgs e ) {
            int stdx = hScroll.getValue();
            for ( int i = 0; i < m_tracker.size(); i++ ) {
                m_tracker.get( i ).setLocation( -stdx + (VolumeTracker.WIDTH + 1) * i, 0 );
            }
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

        public void chkTopmost_CheckedChanged( Object sender, BEventArgs e ) {
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
        #endregion

        #region UI implementation
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
            this.hScroll = new BHScrollBar();
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
        private BHScrollBar hScroll;
        private BCheckBox chkTopmost;
        #endregion
#endif
        #endregion

    }

#if !JAVA
}
#endif
