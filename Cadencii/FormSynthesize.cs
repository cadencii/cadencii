/*
 * FormSynthesize.cs
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using Boare.Lib.Media;
using bocoree;

namespace Boare.Cadencii {

    using boolean = System.Boolean;
    using Integer = Int32;

    /// <summary>
    /// レンダリングの進捗状況を表示しながら，バックグラウンドでレンダリングを行うフォーム．フォームのLoadと同時にレンダリングが始まる．
    /// </summary>
    public partial class FormSynthesize : Form {
        private VsqFileEx m_vsq;
        private int m_presend = 500;
        private int[] m_tracks;
        private String[] m_files;
        private int m_clock_start;
        private int m_clock_end;
        private boolean m_partial_mode = false;
        private int m_temp_premeasure = 0;
        private int m_finished = -1;
        private boolean m_rendering_started = false;
        private boolean m_reflect_amp_to_wave = false;

        public FormSynthesize( VsqFileEx vsq,
                               int presend,
                               int track,
                               String file,
                               int clock_start,
                               int clock_end,
                               int temp_premeasure,
                               boolean reflect_amp_to_wave ) {
            m_vsq = vsq;
            m_presend = presend;
            m_tracks = new int[] { track };
            m_files = new String[] { file };
            InitializeComponent();
            lblProgress.Text = "1/" + 1;
            progressWhole.Maximum = 1;
            m_partial_mode = true;
            m_clock_start = clock_start;
            m_clock_end = clock_end;
            m_temp_premeasure = temp_premeasure;
            m_reflect_amp_to_wave = reflect_amp_to_wave;
            ApplyLanguage();
            Misc.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }
        
        public FormSynthesize( VsqFileEx vsq, int presend, int[] tracks, String[] files, int end, boolean reflect_amp_to_wave ) {
            m_vsq = vsq;
            m_presend = presend;
            m_tracks = tracks;
            m_files = files;
            InitializeComponent();
            lblProgress.Text = "1/" + m_tracks.Length;
            progressWhole.Maximum = m_tracks.Length;
            m_partial_mode = false;
            m_clock_end = end;
            m_reflect_amp_to_wave = reflect_amp_to_wave;
            ApplyLanguage();
            Misc.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }

        public void ApplyLanguage() {
            this.Text = _( "Synthesize" );
            lblSynthesizing.Text = _( "now synthesizing..." );
            btnCancel.Text = _( "Cancel" );
        }

        private static String _( String id ) {
            return Messaging.GetMessage( id );
        }

        /// <summary>
        /// レンダリングが完了したトラックのリストを取得します
        /// </summary>
        public int[] Finished {
            get {
                Vector<Integer> list = new Vector<Integer>();
                for ( int i = 0; i <= m_finished; i++ ) {
                    list.Add( m_tracks[i] );
                }
                return list.toArray( new Integer[]{} );
            }
        }

        private void FormSynthesize_Load( object sender, EventArgs e ) {
            Start();
        }

        private void Start() {
            if ( VSTiProxy.CurrentUser.Equals( "" ) ) {
                VSTiProxy.CurrentUser = AppManager.getID();
                timer.Enabled = true;
                m_rendering_started = true;
                bgWork.RunWorkerAsync();
            } else {
                m_rendering_started = false;
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void UpdateProgress( int value ) {
            progressWhole.Value = value > progressWhole.Maximum ? progressWhole.Maximum : value;
            lblProgress.Text = value + "/" + m_tracks.Length;
            m_finished = value - 1;
        }

        private void bgWork_DoWork( object sender, DoWorkEventArgs e ) {
            double amp_master = VocaloSysUtil.getAmplifyCoeffFromFeder( m_vsq.Mixer.MasterFeder );
            double pan_left_master = VocaloSysUtil.getAmplifyCoeffFromPanLeft( m_vsq.Mixer.MasterPanpot );
            double pan_right_master = VocaloSysUtil.getAmplifyCoeffFromPanRight( m_vsq.Mixer.MasterPanpot );
            if ( m_partial_mode ) {
                this.Invoke( new BSimpleDelegate<int>( this.UpdateProgress ), new object[] { (object)1 } );
                double amp_track = VocaloSysUtil.getAmplifyCoeffFromFeder( m_vsq.Mixer.Slave.get( m_tracks[0] - 1 ).Feder );
                double pan_left_track = VocaloSysUtil.getAmplifyCoeffFromPanLeft( m_vsq.Mixer.Slave.get( m_tracks[0] - 1 ).Panpot );
                double pan_right_track = VocaloSysUtil.getAmplifyCoeffFromPanRight( m_vsq.Mixer.Slave.get( m_tracks[0] - 1 ).Panpot );
                double amp_left = amp_master * amp_track * pan_left_master * pan_left_track;
                double amp_right = amp_master * amp_track * pan_right_master * pan_right_track;
                using ( WaveWriter ww = new WaveWriter( m_files[0] ) ) {
                    VSTiProxy.render( m_vsq,
                                      m_tracks[0],
                                      ww,
                                      m_vsq.getSecFromClock( m_clock_start ),
                                      m_vsq.getSecFromClock( m_clock_end ),
                                      m_presend,
                                      false,
                                      new WaveReader[] { },
                                      0.0,
                                      false,
                                      AppManager.getTempWaveDir(),
                                      m_reflect_amp_to_wave );
                }
            } else {
                for ( int i = 0; i < m_tracks.Length; i++ ) {
                    this.Invoke( new BSimpleDelegate<int>( this.UpdateProgress ), new object[] { (object)(i + 1) } );
                    Vector<VsqNrpn> nrpn = new Vector<VsqNrpn>( VsqFile.generateNRPN( m_vsq, m_tracks[i], m_presend ) );
                    int count = m_vsq.Track.get( m_tracks[i] ).getEventCount();
                    if ( count > 0 ) {
#if DEBUG
                        AppManager.debugWriteLine( "FormSynthesize+bgWork_DoWork" );
                        AppManager.debugWriteLine( "    System.IO.Directory.GetCurrentDirectory()=" + System.IO.Directory.GetCurrentDirectory() );
                        AppManager.debugWriteLine( "    VsqUtil.VstiDllPath=" + VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID2 ) );
#endif
                        double amp_track = VocaloSysUtil.getAmplifyCoeffFromFeder( m_vsq.Mixer.Slave.get( m_tracks[i] - 1 ).Feder );
                        double pan_left_track = VocaloSysUtil.getAmplifyCoeffFromPanLeft( m_vsq.Mixer.Slave.get( m_tracks[i] - 1 ).Panpot );
                        double pan_right_track = VocaloSysUtil.getAmplifyCoeffFromPanRight( m_vsq.Mixer.Slave.get( m_tracks[i] - 1 ).Panpot );
                        double amp_left = amp_master * amp_track * pan_left_master * pan_left_track;
                        double amp_right = amp_master * amp_track * pan_right_master * pan_right_track;
                        int total_clocks = m_vsq.TotalClocks;
                        double total_sec = m_vsq.getSecFromClock( total_clocks );
                        using ( WaveWriter ww = new WaveWriter( m_files[i] ) ) {
                            VSTiProxy.render( m_vsq,
                                              m_tracks[i],
                                              ww,
                                              m_vsq.getSecFromClock( m_clock_start ),
                                              m_vsq.getSecFromClock( m_clock_end ),
                                              m_presend,
                                              false,
                                              new WaveReader[] { },
                                              0.0,
                                              false,
                                              AppManager.getTempWaveDir(),
                                              m_reflect_amp_to_wave );
                        }
                    }
                }
            }
        }

        private void FormSynthesize_FormClosing( object sender, FormClosingEventArgs e ) {
            timer.Enabled = false;
            if ( m_rendering_started ) {
                VSTiProxy.CurrentUser = "";
            }
            if ( bgWork.IsBusy ) {
                VSTiProxy.abortRendering();
                bgWork.CancelAsync();
                while ( bgWork.IsBusy ) {
                    Application.DoEvents();
                }
                DialogResult = DialogResult.Cancel;
            } else {
                DialogResult = DialogResult.OK;
            }
        }

        private void bgWork_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e ) {
            timer.Enabled = false;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void timer_Tick( object sender, EventArgs e ) {
            double progress = VSTiProxy.getProgress();
            TimeSpan elapsed = new TimeSpan( 0, 0, (int)VSTiProxy.getElapsedSeconds() );
            if ( progress >= 0.0 ) {
                TimeSpan remaining = new TimeSpan( 0, 0, 0, (int)VSTiProxy.computeRemainintSeconds() );
                lblTime.Text = _( "Remaining" ) + " " + getTimeSpanString( remaining ) + " (" + getTimeSpanString( elapsed ) + " " + _( "elapsed" ) + ")";
            } else {
                lblTime.Text = _( "Remaining" ) + " [unknown] (" + getTimeSpanString( elapsed ) + " " + _( "elapsed" ) + ")";
            }
            progressOne.Value = (int)progress > 100 ? 100 : (int)progress;
        }

        private static String getTimeSpanString( TimeSpan span ) {
            String ret = "";
            boolean added = false;
            if ( span.Days > 0 ) {
                ret += span.Days + _( "day" ) + " ";
                added = true;
            }
            if ( added || span.Hours > 0 ) {
                ret += string.Format( added ? "{0:d2}" : "{0}", span.Hours ) + _( "hour" ) + " ";
                added = true;
            }
            if ( added || span.Minutes > 0 ) {
                ret += string.Format( added ? "{0:d2}" : "{0}", span.Minutes ) + _( "min" ) + " ";
                added = true;
            }
            return ret + string.Format( added ? "{0:d2}" : "{0}", span.Seconds ) + _( "sec" );
        }
    }

}
