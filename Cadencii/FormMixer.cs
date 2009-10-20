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
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.util;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    public partial class FormMixer : Form {
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
                m_tracker[j].Feder = vme.Feder;
                m_tracker[j].Panpot = vme.Panpot;
                m_tracker[j].Title = AppManager.getVsqFile().Track.get( j + 1 ).getName();
                m_tracker[j].Number = (j + 1).ToString();
                m_tracker[j].Location = new Point( j * (VolumeTracker.WIDTH + 1), 0 );
                m_tracker[j].SoloButtonVisible = true;
                m_tracker[j].IsMuted = (vme.Mute == 1);
                m_tracker[j].IsSolo = (vme.Solo == 1);
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
                m_tracker[j].Feder = item.feder;
                m_tracker[j].Panpot = item.panpot;
                m_tracker[j].Title = Path.GetFileName( item.file );
                m_tracker[j].Number = "";
                m_tracker[j].Location = new Point( j * (VolumeTracker.WIDTH + 1), 0 );
                m_tracker[j].SoloButtonVisible = false;
                m_tracker[j].IsMuted = (item.mute == 1);
                m_tracker[j].IsSolo = false;
                m_tracker[j].Tag = (int)(-i - 1);
                m_tracker[j].IsMutedChanged += new EventHandler( FormMixer_IsMutedChanged );
                //m_tracker[j].IsSoloChanged += new EventHandler( FormMixer_IsSoloChanged );
                m_tracker[j].FederChanged += new EventHandler( FormMixer_FederChanged );
                m_tracker[j].PanpotChanged += new EventHandler( FormMixer_PanpotChanged );
                panel1.Controls.Add( m_tracker[j] );
            }
            volumeMaster.Feder = AppManager.getVsqFile().Mixer.MasterFeder;
            volumeMaster.Panpot = AppManager.getVsqFile().Mixer.MasterPanpot;
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
                PanpotChanged( track, parent.Panpot );
            }
        }

        private void FormMixer_FederChanged( object sender, EventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (int)parent.Tag;
            if ( FederChanged != null ) {
                FederChanged( track, parent.Feder );
            }
        }

        private void FormMixer_IsSoloChanged( object sender, EventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (int)parent.Tag;
            if ( SoloChanged != null ) {
                SoloChanged( track, parent.IsSolo );
            }
        }

        private void FormMixer_IsMutedChanged( object sender, EventArgs e ) {
            VolumeTracker parent = (VolumeTracker)sender;
            int track = (int)parent.Tag;
            if ( MuteChanged != null ) {
                MuteChanged( track, parent.IsMuted );
            }
        }

        public FormMixer( FormMain parent ) {
            InitializeComponent();
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
            using ( Pen pen_062_061_062 = new Pen( Color.FromArgb( 62, 61, 62 ) ) ){
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
                FederChanged( 0, volumeMaster.Feder );
            }
        }

        private void volumeMaster_PanpotChanged( object sender, EventArgs e ) {
            if ( PanpotChanged != null ) {
                PanpotChanged( 0, volumeMaster.Panpot );
            }
        }

        private void volumeMaster_IsMutedChanged( object sender, EventArgs e ) {
            if ( MuteChanged != null ) {
                MuteChanged( 0, volumeMaster.IsMuted );
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
    }

}
