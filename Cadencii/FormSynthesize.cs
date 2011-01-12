/*
 * FormSynthesize.cs
 * Copyright © 2008-2011 kbinani
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/Cadencii/FormSynthesize.java

import java.util.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.media.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
import org.kbinani.componentmodel.*;
#else
using System;
using System.Windows.Forms;
using org.kbinani.apputil;
using org.kbinani.componentmodel;
using org.kbinani.java.util;
using org.kbinani.media;
using org.kbinani.vsq;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using BDoWorkEventArgs = System.ComponentModel.DoWorkEventArgs;
    using BEventArgs = System.EventArgs;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
    using boolean = System.Boolean;
    using BRunWorkerCompletedEventArgs = System.ComponentModel.RunWorkerCompletedEventArgs;
    using Integer = System.Int32;
#endif

    /// <summary>
    /// レンダリングの進捗状況を表示しながら，バックグラウンドでレンダリングを行うフォーム．
    /// フォームのLoadと同時にレンダリングが始まる．
    /// </summary>
#if JAVA
    public class FormSynthesize extends BDialog {
#else
    public class FormSynthesize : BDialog {
#endif
        private VsqFileEx mVsq;
        private int mPresend = 500;
        private Vector<PatchWorkQueue> mQueue;
        private int mFinished = -1;
        //private boolean mReflectAmpToWave = false;
        //private boolean mIsPartialMode = false;
        private boolean mIsCancelRequired = false;
        private WaveGenerator mGenerator = null;
        private FormMain mMainWindow = null;

        private BTimer timer;
        private BBackgroundWorker bgWork;

        public FormSynthesize( FormMain main_window,
                               VsqFileEx vsq,
                               int presend,
                               PatchWorkQueue queue )
#if JAVA
        {
            this( vsq, presend, new Integer[] { track }, new String[]{ file }, new Integer[]{ clock_start }, new Integer[]{ clock_end }, reflect_amp_to_wave );
#else
            : this( main_window, vsq, presend, Arrays.asList( new PatchWorkQueue[]{ queue } ) ) {
#endif
        }

        public FormSynthesize( FormMain main_window,
                               VsqFileEx vsq, 
                               int presend, 
                               Vector<PatchWorkQueue> queue ) {
#if JAVA
            super();
#endif
            mMainWindow = main_window;
            mVsq = vsq;
            mPresend = presend;
            mQueue = queue;
#if JAVA
            initialize();
            timer = new BTimer();
#else
            InitializeComponent();
            timer = new BTimer( this.components );
            bgWork = new BBackgroundWorker();
            bgWork.WorkerReportsProgress = true;
            bgWork.WorkerSupportsCancellation = true;
#endif
            timer.setDelay( 1000 );
            registerEventHandlers();
            setResources();
            lblProgress.setText( "0/" + mQueue.size() );
            int totalClocks = 0;
            int vsqClocks = mVsq.TotalClocks + 240;
            for ( int i = 0; i < mQueue.size(); i++ ) {
                int e = mQueue.get( i ).clockEnd;
                if ( e == int.MaxValue ) {
                    e = vsqClocks;
                }
                totalClocks += e - mQueue.get( i ).clockStart;
            }
            progressWhole.setMaximum( totalClocks );
            progressWhole.setMinimum( 0 );
            progressWhole.setValue( 0 );
            applyLanguage();
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        #region public methods
        public void applyLanguage() {
            setTitle( _( "Synthesize" ) );
            lblSynthesizing.setText( _( "now synthesizing..." ) );
            btnCancel.setText( _( "Cancel" ) );
        }

        /// <summary>
        /// レンダリングが完了したトラックの個数を取得します
        /// </summary>
        public int getFinished() {
            return mFinished;
        }
        #endregion

        #region helper methods
        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void startSynthesize() {
            timer.start();
            bgWork.runWorkerAsync();
        }

        private void updateProgress( Object sender, int value ) {
            int totalClocks = 0;
            for ( int i = 0; i < value; i++ ) {
                int end = mQueue[i].clockEnd;
                if ( end == int.MaxValue ) {
                    end = mVsq.TotalClocks + 240;
                }
                totalClocks += (end - mQueue[i].clockStart);
            }
            progressWhole.setValue( totalClocks );
            lblProgress.setText( value + "/" + mQueue.size() );
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
            this.Load += new EventHandler( FormSynthesize_Load );
            bgWork.DoWork += new System.ComponentModel.DoWorkEventHandler( bgWork_DoWork );
            bgWork.RunWorkerCompleted += 
                new System.ComponentModel.RunWorkerCompletedEventHandler( bgWork_RunWorkerCompleted );
            timer.Tick += new EventHandler( timer_Tick );
            this.FormClosing += new FormClosingEventHandler( FormSynthesize_FormClosing );
            btnCancel.Click += new EventHandler( btnCancel_Click );
        }

        private void setResources() {
        }
        #endregion

        #region event handlers
        public void FormSynthesize_Load( Object sender, BEventArgs e ) {
            lblTime.setText( "" );
            startSynthesize();
        }

        public void bgWork_DoWork( Object sender, BDoWorkEventArgs e ) {
            try {
                int channel = mVsq.config.WaveFileOutputChannel == 1 ? 1 : 2;
                double amp_master = VocaloSysUtil.getAmplifyCoeffFromFeder( mVsq.Mixer.MasterFeder );
                double pan_left_master = VocaloSysUtil.getAmplifyCoeffFromPanLeft( mVsq.Mixer.MasterPanpot );
                double pan_right_master = VocaloSysUtil.getAmplifyCoeffFromPanRight( mVsq.Mixer.MasterPanpot );

                int numTrack = mVsq.Track.size();
                String tmppath = AppManager.getTempWaveDir();
                mFinished = 0;

                for ( int k = 0; k < mQueue.size(); k++ ) {
                    PatchWorkQueue q = mQueue[k];
                    int track = q.track;
#if JAVA
                    UpdateProgress( this, 1 );
#else
                    this.Invoke( new UpdateProgressEventHandler( this.updateProgress ), this, k );
#endif
                    VsqTrack vsq_track = mVsq.Track.get( track );
                    int count = vsq_track.getEventCount();
                    if ( count > 0 ) {
                        double amp_track = VocaloSysUtil.getAmplifyCoeffFromFeder( mVsq.Mixer.Slave.get( track - 1 ).Feder );
                        double pan_left_track = VocaloSysUtil.getAmplifyCoeffFromPanLeft( mVsq.Mixer.Slave.get( track - 1 ).Panpot );
                        double pan_right_track = VocaloSysUtil.getAmplifyCoeffFromPanRight( mVsq.Mixer.Slave.get( track - 1 ).Panpot );
                        double amp_left = amp_track * pan_left_track;
                        double amp_right = amp_track * pan_right_track;
                        int total_clocks = mVsq.TotalClocks;
                        double total_sec = mVsq.getSecFromClock( total_clocks );

                        RendererKind kind = VsqFileEx.getTrackRendererKind( vsq_track );
                        mGenerator = VSTiDllManager.getWaveGenerator( kind );
#if DEBUG
                        PortUtil.println( "FormSynthesize#bgWork_DoWork; mGenerator.GetType()=" + mGenerator.GetType() );
#endif
                        Amplifier amp = new Amplifier();
                        amp.setRoot( mGenerator );
                        if ( q.renderAll ) {
                            amp.setAmplify( amp_left, amp_right );
                        }
                        mGenerator.setReceiver( amp );
                        mGenerator.setGlobalConfig( AppManager.editorConfig );
                        mGenerator.setMainWindow( mMainWindow );

                        Mixer mixer = new Mixer();
                        mixer.setRoot( mGenerator );
                        mixer.setGlobalConfig( AppManager.editorConfig );
                        amp.setReceiver( mixer );

                        if ( q.renderAll && mVsq.config.WaveFileOutputFromMasterTrack ) {
                            // トラック全体を合成するモードで，かつ，他トラックを合成して出力するよう指示された場合
                            if ( numTrack > 2 ) {
                                for ( int i = 1; i < numTrack; i++ ) {
                                    if ( i == track ) continue;
                                    String file = PortUtil.combinePath( tmppath, i + ".wav" );
                                    if ( !PortUtil.isFileExists( file ) ) {
                                        // mixするべきファイルが揃っていないのでbailout
                                        return;
                                    }
                                    WaveReader r = new WaveReader( file );
                                    double end_sec = mVsq.getSecFromClock( q.clockStart );
                                    r.setOffsetSeconds( end_sec );
                                    Amplifier amp_i_unit = new Amplifier();
                                    amp_i_unit.setRoot( mGenerator );
                                    double amp_i = VocaloSysUtil.getAmplifyCoeffFromFeder( mVsq.Mixer.Slave.get( i - 1 ).Feder );
                                    double pan_left_i = VocaloSysUtil.getAmplifyCoeffFromPanLeft( mVsq.Mixer.Slave.get( i - 1 ).Panpot );
                                    double pan_right_i = VocaloSysUtil.getAmplifyCoeffFromPanRight( mVsq.Mixer.Slave.get( i - 1 ).Panpot );
                                    double amp_left_i = amp_i * pan_left_i;
                                    double amp_right_i = amp_i * pan_right_i;
#if DEBUG
                                    PortUtil.println( "FormSynthesize#bgWork_DoWork; #" + i + "; amp_left_i=" + amp_left_i + "; amp_right_i=" + amp_right_i );
#endif
                                    amp_i_unit.setAmplify( amp_left_i, amp_right_i );
                                    FileWaveSender wave_sender = new FileWaveSender( r );
                                    wave_sender.setRoot( mGenerator );
                                    wave_sender.setGlobalConfig( AppManager.editorConfig );

                                    amp_i_unit.setSender( wave_sender );
                                    mixer.addSender( amp_i_unit );
                                }
                            }
                        }

                        PortUtil.deleteFile( q.file );
                        FileWaveReceiver wave_receiver = new FileWaveReceiver( q.file, channel, 16 );
                        wave_receiver.setRoot( mGenerator );
                        wave_receiver.setGlobalConfig( AppManager.editorConfig );
                        Amplifier amp_unit_master = new Amplifier();
                        amp_unit_master.setRoot( mGenerator );
                        if ( q.renderAll ) {
                            double l = amp_master * pan_left_master;
                            double r = amp_master * pan_right_master;
                            amp_unit_master.setAmplify( l, r );
                        }
                        mixer.setReceiver( amp_unit_master );
                        amp_unit_master.setReceiver( wave_receiver );

                        int end = q.clockEnd;
                        if( end == int.MaxValue ) end = mVsq.TotalClocks + 240;
                        int sample_rate = mVsq.config.SamplingRate;
                        mGenerator.init( mVsq, track, q.clockStart, end, sample_rate );

                        double sec_start = mVsq.getSecFromClock( q.clockStart );
                        double sec_end = mVsq.getSecFromClock( end );
                        long samples = (long)((sec_end - sec_start) * sample_rate);
                        mGenerator.begin( samples );

                        mFinished++;
                        if ( mIsCancelRequired ) break;
                    }
                }
#if JAVA
                UpdateProgress( this, m_tracks.Length );
#else
                this.Invoke( new UpdateProgressEventHandler( this.updateProgress ), this, mQueue.size() );
#endif
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "FormSynthesize#bgWork_DoWork; ex=" + ex );
            }
        }

        public void FormSynthesize_FormClosing( Object sender, BFormClosingEventArgs e ) {
            timer.stop();
            if ( bgWork.isBusy() ) {
                mIsCancelRequired = true;
                if( mGenerator != null ){
                    mGenerator.stop();
                    mGenerator = null;
                }
                setDialogResult( BDialogResult.CANCEL );
            } else {
                setDialogResult( BDialogResult.OK );
            }
        }

        public void bgWork_RunWorkerCompleted( Object sender, BRunWorkerCompletedEventArgs e ) {
            timer.stop();
            setDialogResult( BDialogResult.OK );
            close();
        }

        public void timer_Tick( Object sender, BEventArgs e ) {
            double progress = mGenerator.getProgress() * 100;

#if DEBUG
            PortUtil.println( "FormSynthesize#timer_Tick; progress=" + progress );
#endif
            /*long elapsed = (long)VSTiProxy.getElapsedSeconds();
            long remaining = (long)VSTiProxy.computeRemainintSeconds();
            if ( progress >= 0.0 && remaining >= 0.0 ) {
                lblTime.setText( _( "Remaining" ) + " " + getTimeSpanString( remaining ) + " (" + getTimeSpanString( elapsed ) + " " + _( "elapsed" ) + ")" );
            } else {
                lblTime.setText( _( "Remaining" ) + " [unknown] (" + getTimeSpanString( elapsed ) + " " + _( "elapsed" ) + ")" );
            }*/
            progressOne.setValue( (int)progress > 100 ? 100 : (int)progress );
        }

        public void btnCancel_Click( Object sender, BEventArgs e ) {
            mIsCancelRequired = true;
            mGenerator.stop();
            setDialogResult( BDialogResult.CANCEL );
        }
        #endregion

        #region UI implementation
#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/Cadencii/FormSynthesize.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/Cadencii/FormSynthesize.java
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
        private BLabel lblTime;
        #endregion
#endif
        #endregion

    }

#if !JAVA
}
#endif
