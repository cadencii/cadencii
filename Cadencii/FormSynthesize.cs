﻿/*
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
#if JAVA
package org.kbinani.Cadencii;

import java.util.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.media.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
import org.kbinani.componentModel.*;
#else
using System;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using Boare.Lib.Media;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.util;
using bocoree.windows.forms;
using bocoree.componentModel;

namespace Boare.Cadencii {
    using BDoWorkEventArgs = System.ComponentModel.DoWorkEventArgs;
    using BEventArgs = System.EventArgs;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
    using boolean = System.Boolean;
    using BRunWorkerCompletedEventArgs = System.ComponentModel.RunWorkerCompletedEventArgs;
    using Integer = Int32;
#endif

    /// <summary>
    /// レンダリングの進捗状況を表示しながら，バックグラウンドでレンダリングを行うフォーム．フォームのLoadと同時にレンダリングが始まる．
    /// </summary>
#if JAVA
    public class FormSynthesize extends BForm {
#else
    public class FormSynthesize : BForm {
#endif
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
            registerEventHandlers();
            setResources();
            lblProgress.Text = "1/" + 1;
            progressWhole.Maximum = 1;
            m_partial_mode = true;
            m_clock_start = clock_start;
            m_clock_end = clock_end;
            m_temp_premeasure = temp_premeasure;
            m_reflect_amp_to_wave = reflect_amp_to_wave;
            applyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public FormSynthesize( VsqFileEx vsq, int presend, int[] tracks, String[] files, int end, boolean reflect_amp_to_wave ) {
            m_vsq = vsq;
            m_presend = presend;
            m_tracks = tracks;
            m_files = files;
            InitializeComponent();
            registerEventHandlers();
            setResources();
            lblProgress.Text = "1/" + m_tracks.Length;
            progressWhole.Maximum = m_tracks.Length;
            m_partial_mode = false;
            m_clock_end = end;
            m_reflect_amp_to_wave = reflect_amp_to_wave;
            applyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public void applyLanguage() {
            setTitle( _( "Synthesize" ) );
            lblSynthesizing.Text = _( "now synthesizing..." );
            btnCancel.Text = _( "Cancel" );
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        /// <summary>
        /// レンダリングが完了したトラックのリストを取得します
        /// </summary>
        public int[] getFinished() {
            Vector<Integer> list = new Vector<Integer>();
            for ( int i = 0; i <= m_finished; i++ ) {
                list.Add( m_tracks[i] );
            }
            return list.toArray( new Integer[] { } );
        }

        private void FormSynthesize_Load( Object sender, BEventArgs e ) {
            lblTime.Text = "";
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

        private void UpdateProgress( Object sender, int value ) {
            progressWhole.Value = value > progressWhole.Maximum ? progressWhole.Maximum : value;
            lblProgress.Text = value + "/" + m_tracks.Length;
            m_finished = value - 1;
        }

        private void bgWork_DoWork( Object sender, BDoWorkEventArgs e ) {
            double amp_master = VocaloSysUtil.getAmplifyCoeffFromFeder( m_vsq.Mixer.MasterFeder );
            double pan_left_master = VocaloSysUtil.getAmplifyCoeffFromPanLeft( m_vsq.Mixer.MasterPanpot );
            double pan_right_master = VocaloSysUtil.getAmplifyCoeffFromPanRight( m_vsq.Mixer.MasterPanpot );
            if ( m_partial_mode ) {
                this.Invoke( new UpdateProgressEventHandler( this.UpdateProgress ), this, (Object)1 );
                double amp_track = VocaloSysUtil.getAmplifyCoeffFromFeder( m_vsq.Mixer.Slave.get( m_tracks[0] - 1 ).Feder );
                double pan_left_track = VocaloSysUtil.getAmplifyCoeffFromPanLeft( m_vsq.Mixer.Slave.get( m_tracks[0] - 1 ).Panpot );
                double pan_right_track = VocaloSysUtil.getAmplifyCoeffFromPanRight( m_vsq.Mixer.Slave.get( m_tracks[0] - 1 ).Panpot );
                double amp_left = amp_master * amp_track * pan_left_master * pan_left_track;
                double amp_right = amp_master * amp_track * pan_right_master * pan_right_track;
                WaveWriter ww = null;
                try {
                    ww = new WaveWriter( m_files[0] );
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
                } catch ( Exception ex ) {
                } finally {
                    if ( ww != null ) {
                        try {
#if !JAVA
                            ww.Dispose();
#endif
                        } catch ( Exception ex2 ) {
                        }
                    }
                }
            } else {
                for ( int i = 0; i < m_tracks.Length; i++ ) {
                    this.Invoke( new UpdateProgressEventHandler( this.UpdateProgress ), this, (Object)(i + 1) );
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
                        WaveWriter ww = null;
                        try {
                            ww = new WaveWriter( m_files[i] );
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
                        } catch ( Exception ex ) {
                        } finally {
                            if ( ww != null ) {
                                try {
#if !JAVA
                                    ww.Dispose();
#endif
                                } catch ( Exception ex2 ) {
                                }
                            }
                        }
                    }
                }
            }
        }

        private void FormSynthesize_FormClosing( Object sender, BFormClosingEventArgs e ) {
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

        private void bgWork_RunWorkerCompleted( Object sender, BRunWorkerCompletedEventArgs e ) {
            timer.Enabled = false;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void timer_Tick( Object sender, BEventArgs e ) {
            double progress = VSTiProxy.getProgress();
            long elapsed = (long)VSTiProxy.getElapsedSeconds();
            long remaining = (long)VSTiProxy.computeRemainintSeconds();
            if ( progress >= 0.0 && remaining >= 0.0 ) {
                lblTime.Text = _( "Remaining" ) + " " + getTimeSpanString( remaining ) + " (" + getTimeSpanString( elapsed ) + " " + _( "elapsed" ) + ")";
            } else {
                lblTime.Text = _( "Remaining" ) + " [unknown] (" + getTimeSpanString( elapsed ) + " " + _( "elapsed" ) + ")";
            }
            progressOne.Value = (int)progress > 100 ? 100 : (int)progress;
        }

        private static String getTimeSpanString( long span ) {
            int sec_per_day = 24 * 60 * 60;
            int sec_per_hour = 60 * 60;
            int sec_per_min = 60;
            String ret = "";
            boolean added = false;
            int i = (int)(span / sec_per_day);
            if ( i > 0 ) {
                ret += i + _( "day" ) + " ";
                added = true;
                span -= i * sec_per_day;
            }
            i = (int)(span / sec_per_hour);
            if ( added || i > 0 ) {
                ret += PortUtil.formatDecimal( added ? "00" : "0", i ) + _( "hour" ) + " ";
                added = true;
                span -= i * sec_per_hour;
            }
            i = (int)(span / sec_per_min);
            if ( added || i > 0 ) {
                ret += PortUtil.formatDecimal( added ? "00" : "0", i ) + _( "min" ) + " ";
                added = true;
                span -= i * sec_per_min;
            }
            return ret + PortUtil.formatDecimal( added ? "00" : "0", span ) + _( "sec" );
        }

        private void registerEventHandlers() {
            this.bgWork.DoWork += new System.ComponentModel.DoWorkEventHandler( this.bgWork_DoWork );
            this.bgWork.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.bgWork_RunWorkerCompleted );
            this.timer.Tick += new System.EventHandler( this.timer_Tick );
            this.Load += new System.EventHandler( this.FormSynthesize_Load );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.FormSynthesize_FormClosing );
        }

        private void setResources() {
        }

#if JAVA
        #region UI Impl for Java
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
            this.components = new System.ComponentModel.Container();
            this.progressWhole = new BProgressBar();
            this.lblSynthesizing = new BLabel();
            this.lblProgress = new BLabel();
            this.progressOne = new BProgressBar();
            this.btnCancel = new BButton();
            this.bgWork = new BBackgroundWorker();
            this.timer = new BTimer( this.components );
            this.lblTime = new BLabel();
            this.SuspendLayout();
            // 
            // progressWhole
            // 
            this.progressWhole.Location = new System.Drawing.Point( 12, 49 );
            this.progressWhole.Name = "progressWhole";
            this.progressWhole.Size = new System.Drawing.Size( 345, 23 );
            this.progressWhole.TabIndex = 0;
            // 
            // lblSynthesizing
            // 
            this.lblSynthesizing.AutoSize = true;
            this.lblSynthesizing.Location = new System.Drawing.Point( 12, 9 );
            this.lblSynthesizing.Name = "lblSynthesizing";
            this.lblSynthesizing.Size = new System.Drawing.Size( 98, 12 );
            this.lblSynthesizing.TabIndex = 1;
            this.lblSynthesizing.Text = "now synthesizing...";
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point( 336, 9 );
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size( 23, 12 );
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = "1/1";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressOne
            // 
            this.progressOne.Location = new System.Drawing.Point( 12, 78 );
            this.progressOne.Name = "progressOne";
            this.progressOne.Size = new System.Drawing.Size( 345, 23 );
            this.progressOne.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 277, 113 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 80, 23 );
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // bgWork
            // 
            this.bgWork.WorkerReportsProgress = true;
            this.bgWork.WorkerSupportsCancellation = true;
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point( 12, 29 );
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size( 137, 12 );
            this.lblTime.TabIndex = 5;
            this.lblTime.Text = "remaining 0s (elapsed 0s)";
            // 
            // FormSynthesize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 371, 158 );
            this.Controls.Add( this.lblTime );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.progressOne );
            this.Controls.Add( this.lblProgress );
            this.Controls.Add( this.lblSynthesizing );
            this.Controls.Add( this.progressWhole );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSynthesize";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Synthesize";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BProgressBar progressWhole;
        private BLabel lblSynthesizing;
        private BLabel lblProgress;
        private BProgressBar progressOne;
        private BButton btnCancel;
        private BBackgroundWorker bgWork;
        private BTimer timer;
        private BLabel lblTime;
        #endregion
#endif

    }

#if !JAVA
}
#endif
