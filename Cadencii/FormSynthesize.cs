/*
 * FormSynthesize.cs
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
        private Integer[] mTracks;
        private String[] mFiles;
        private int[] mClockStart;
        private int[] mClockEnd;
        private int mFinished = -1;
        private boolean mRenderingStarted = false;
        private boolean mReflectAmpToWave = false;
        private boolean mIsPartialMode = false;
        private boolean mIsCancelRequired = false;

        private BTimer timer;
        private BBackgroundWorker bgWork;

        public FormSynthesize( VsqFileEx vsq,
                               int presend,
                               int track,
                               String file,
                               int clock_start,
                               int clock_end,
                               boolean reflect_amp_to_wave )
#if JAVA
        {
            this( vsq, presend, new Integer[] { track }, new String[]{ file }, new Integer[]{ clock_start }, new Integer[]{ clock_end }, reflect_amp_to_wave );
#else
            : this( vsq, presend, new Integer[] { track }, new String[] { file }, new Integer[] { clock_start }, new Integer[] { clock_end }, reflect_amp_to_wave ) {
#endif
            mIsPartialMode = true;
        }

        public FormSynthesize( VsqFileEx vsq, 
                               int presend, 
                               Integer[] tracks,
                               String[] files,
                               Integer[] start,
                               Integer[] end,
                               boolean reflect_amp_to_wave ) {
#if JAVA
            super();
#endif
            mVsq = vsq;
            mPresend = presend;
            mTracks = tracks;
            mFiles = files;
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
            lblProgress.setText( "0/" + mTracks.Length );
            int totalClocks = 0;
            int vsqClocks = mVsq.TotalClocks + 240;
            for ( int i = 0; i < start.Length; i++ ) {
                int e = end[i];
                if ( e == int.MaxValue ) {
                    e = vsqClocks;
                }
                totalClocks += e - start[i];
            }
            progressWhole.setMaximum( totalClocks );
            progressWhole.setMinimum( 0 );
            progressWhole.setValue( 0 );
            mClockStart = new int[start.Length];
            mClockEnd = new int[end.Length];
            for( int i = 0; i < start.Length; i++ ){
                mClockStart[i] = start[i];
                mClockEnd[i] = end[i];
            }
            mReflectAmpToWave = reflect_amp_to_wave;
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
            //if ( VSTiProxy.CurrentUser.Equals( "" ) ) {
                //VSTiProxy.CurrentUser = AppManager.getID();
                timer.start();
                mRenderingStarted = true;
                bgWork.runWorkerAsync();
           // } else {
              //  mRenderingStarted = false;
               // setDialogResult( BDialogResult.CANCEL );
            //}
        }

        private void updateProgress( Object sender, int value ) {
            int totalClocks = 0;
            for ( int i = 0; i < value; i++ ) {
                int end = mClockEnd[i];
                if ( end == int.MaxValue ) {
                    end = mVsq.TotalClocks + 240;
                }
                totalClocks += (end - mClockStart[i]);
            }
            progressWhole.setValue( totalClocks );
            lblProgress.setText( value + "/" + mTracks.Length );
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
            loadEvent.add( new BEventHandler( this, "FormSynthesize_Load" ) );
            bgWork.doWorkEvent.add( new BDoWorkEventHandler( this, "bgWork_DoWork" ) );
            bgWork.runWorkerCompletedEvent.add( new BRunWorkerCompletedEventHandler( this, "bgWork_RunWorkerCompleted" ) );
            timer.tickEvent.add(  new BEventHandler( this, "timer_Tick" ) );
            formClosingEvent.add( new BFormClosingEventHandler( this, "FormSynthesize_FormClosing" ) );
            btnCancel.clickEvent.add( new BEventHandler( this, "btnCancel_Click" ) );
        }

        private void setResources() {
        }
        #endregion

        #region event handlers
        public void FormSynthesize_Load( Object sender, BEventArgs e ) {
            lblTime.setText( "" );
            startSynthesize();
        }

        public void bgWork_DoWork( object sender, BDoWorkEventArgs e ) {
#if USE_OLD_SYNTH_IMPL
            try {
                int channel = AppManager.editorConfig.WaveFileOutputChannel == 1 ? 1 : 2;
                double amp_master = VocaloSysUtil.getAmplifyCoeffFromFeder( mVsq.Mixer.MasterFeder );
                double pan_left_master = VocaloSysUtil.getAmplifyCoeffFromPanLeft( mVsq.Mixer.MasterPanpot );
                double pan_right_master = VocaloSysUtil.getAmplifyCoeffFromPanRight( mVsq.Mixer.MasterPanpot );

                int numTrack = mVsq.Track.size();
                String tmppath = AppManager.getTempWaveDir();
                mFinished = 0;

                for ( int k = 0; k < mTracks.Length; k++ ) {
                    int track = mTracks[k];
#if JAVA
                    UpdateProgress( this, 1 );
#else
                    this.Invoke( new UpdateProgressEventHandler( this.updateProgress ), this, k );
#endif
                    int count = mVsq.Track.get( track ).getEventCount();
                    if ( count > 0 ) {
#if DEBUG
                        AppManager.debugWriteLine( "FormSynthesize#bgWork_DoWork" );
                        AppManager.debugWriteLine( "    VsqUtil.VstiDllPath=" + VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID2 ) );
#endif
                        double amp_track = VocaloSysUtil.getAmplifyCoeffFromFeder( mVsq.Mixer.Slave.get( track - 1 ).Feder );
                        double pan_left_track = VocaloSysUtil.getAmplifyCoeffFromPanLeft( mVsq.Mixer.Slave.get( track - 1 ).Panpot );
                        double pan_right_track = VocaloSysUtil.getAmplifyCoeffFromPanRight( mVsq.Mixer.Slave.get( track - 1 ).Panpot );
                        double amp_left = amp_master * amp_track * pan_left_master * pan_left_track;
                        double amp_right = amp_master * amp_track * pan_right_master * pan_right_track;
                        int total_clocks = mVsq.TotalClocks;
                        double total_sec = mVsq.getSecFromClock( total_clocks );
                        WaveWriter ww = null;
                        Vector<WaveReader> readers = new Vector<WaveReader>();

                        if ( mIsPartialMode && AppManager.editorConfig.WaveFileOutputFromMasterTrack ) {
                            if ( numTrack > 2 ) {
                                // track以外にもトラックがあるので。
                                for ( int i = 1; i < numTrack; i++ ) {
                                    if ( i == track ) {
                                        continue;
                                    }
                                    String file = PortUtil.combinePath( tmppath, i + ".wav" );
                                    WaveReader r = new WaveReader( file );
                                    r.setTag( i );
                                    readers.add( r );
                                }
                            }
                        }

                        try {
                            // 上書きするので消す
                            PortUtil.deleteFile( mFiles[k] );
                            // WaveWriter内のファイルストリーム入出力クラスRandomAccessFileは、書き込みモードのみ
                            // というのを選択できないので。
                            ww = new WaveWriter( mFiles[k], channel, 16, VSTiProxy.SAMPLE_RATE );
                            int end = mClockEnd[k];
                            if ( end == int.MaxValue ) {
                                end = mVsq.TotalClocks + 240;
                            }
                            VSTiProxy.render( mVsq,
                                              track,
                                              ww,
                                              mVsq.getSecFromClock( mClockStart[k] ),
                                              mVsq.getSecFromClock( end ),
                                              mPresend,
                                              false,
                                              readers.toArray( new WaveReader[] { } ),
                                              0.0,
                                              false,
                                              tmppath,
                                              mReflectAmpToWave );
                            if ( mIsCancelRequired ) {
                                break;
                            }
                            mFinished++;
                        } catch ( Exception ex ) {
                            AppManager.reportError( ex, "FormSynthesize#bgWork_DoWork", 0 );
                        } finally {
                            if ( ww != null ) {
                                try {
#if DEBUG
                                    PortUtil.println( "FormSynthesize#bgWork_DoWork; calling ww#close; track=" + k );
#endif
                                    ww.close();
                                } catch ( Exception ex2 ) {
                                    PortUtil.stderr.println( "FormSynthesize#bgWork_DoWork; ex2=" + ex2 );
                                }
                            }
                        }
                    }
                }
#if JAVA
                UpdateProgress( this, m_tracks.Length );
#else
                this.Invoke( new UpdateProgressEventHandler( this.updateProgress ), this, mTracks.Length );
#endif
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "FormSynthesize#bgWork_DoWork; ex=" + ex );
            }
#else // USE_OLD_SYNTH_IMPL
            try {
                int channel = AppManager.editorConfig.WaveFileOutputChannel == 1 ? 1 : 2;
                double amp_master = VocaloSysUtil.getAmplifyCoeffFromFeder( mVsq.Mixer.MasterFeder );
                double pan_left_master = VocaloSysUtil.getAmplifyCoeffFromPanLeft( mVsq.Mixer.MasterPanpot );
                double pan_right_master = VocaloSysUtil.getAmplifyCoeffFromPanRight( mVsq.Mixer.MasterPanpot );

                int numTrack = mVsq.Track.size();
                String tmppath = AppManager.getTempWaveDir();
                mFinished = 0;

                for ( int k = 0; k < mTracks.Length; k++ ) {
                    int track = mTracks[k];
#if JAVA
                    UpdateProgress( this, 1 );
#else
                    this.Invoke( new UpdateProgressEventHandler( this.updateProgress ), this, k );
#endif
                    VsqTrack vsq_track = mVsq.Track.get( track );
                    int count = vsq_track.getEventCount();
                    if ( count > 0 ) {
#if DEBUG
                        AppManager.debugWriteLine( "FormSynthesize#bgWork_DoWork" );
                        AppManager.debugWriteLine( "    VsqUtil.VstiDllPath=" + VocaloSysUtil.getDllPathVsti( SynthesizerType.VOCALOID2 ) );
#endif
                        double amp_track = VocaloSysUtil.getAmplifyCoeffFromFeder( mVsq.Mixer.Slave.get( track - 1 ).Feder );
                        double pan_left_track = VocaloSysUtil.getAmplifyCoeffFromPanLeft( mVsq.Mixer.Slave.get( track - 1 ).Panpot );
                        double pan_right_track = VocaloSysUtil.getAmplifyCoeffFromPanRight( mVsq.Mixer.Slave.get( track - 1 ).Panpot );
                        double amp_left = amp_master * amp_track * pan_left_master * pan_left_track;
                        double amp_right = amp_master * amp_track * pan_right_master * pan_right_track;
                        int total_clocks = mVsq.TotalClocks;
                        double total_sec = mVsq.getSecFromClock( total_clocks );

                        RendererKind kind = VsqFileEx.getTrackRendererKind( vsq_track );
                        AppManager.waveGenerator = VSTiProxy.getWaveGenerator( kind );

                        Amplifier amp = new Amplifier();
                        AppManager.waveGenerator.setReceiver( amp );
                        AppManager.waveGenerator.setGlobalConfig( AppManager.editorConfig );

                        Mixer mixer = new Mixer();
                        mixer.setGlobalConfig( AppManager.editorConfig );
                        amp.setReceiver( mixer );

                        if ( mIsPartialMode && AppManager.editorConfig.WaveFileOutputFromMasterTrack ) {
                            if ( numTrack > 2 ) {
                                for ( int i = 1; i < numTrack; i++ ) {
                                    if ( i == track ) continue;
                                    String file = PortUtil.combinePath( tmppath, i + ".wav" );
                                    WaveReader r = new WaveReader( file );
                                    FileWaveSender wave_sender = new FileWaveSender( r );
                                    wave_sender.setGlobalConfig( AppManager.editorConfig );
                                    mixer.addSender( wave_sender );
                                }
                            }
                        }

                        PortUtil.deleteFile( mFiles[k] );
                        FileWaveReceiver wave_receiver = new FileWaveReceiver( mFiles[k], 2, 16, VSTiProxy.SAMPLE_RATE );
                        wave_receiver.setGlobalConfig( AppManager.editorConfig );
                        mixer.setReceiver( wave_receiver );

                        int end = mClockEnd[k];
                        if( end == int.MaxValue ) end = mVsq.TotalClocks + 240;
                        AppManager.waveGenerator.init( mVsq, track, mClockStart[k], end );

                        double sec_start = mVsq.getSecFromClock( mClockStart[k] );
                        double sec_end = mVsq.getSecFromClock( end );
                        long samples = (long)((sec_end - sec_start) * VSTiProxy.SAMPLE_RATE);
                        AppManager.waveGenerator.begin( samples );

                        mFinished++;
                        if ( mIsCancelRequired ) break;
                    }
                }
#if JAVA
                UpdateProgress( this, m_tracks.Length );
#else
                this.Invoke( new UpdateProgressEventHandler( this.updateProgress ), this, mTracks.Length );
#endif
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "FormSynthesize#bgWork_DoWork; ex=" + ex );
            }
#endif // USE_OLD_SYNTH_IMPL
        }

        public void FormSynthesize_FormClosing( Object sender, BFormClosingEventArgs e ) {
#if USE_OLD_SYNTH_IMPL
            timer.stop();
            if ( bgWork.isBusy() ) {
                VSTiProxy.abortRendering();
                mIsCancelRequired = true;
                while ( bgWork.isBusy() ) {
#if JAVA
                try{
                    Thread.sleep( 0 );
                }catch( Exception ex ){
                    break;
                }
#else // JAVA
                    Application.DoEvents();
#endif // JAVA
                }
                setDialogResult( BDialogResult.CANCEL );
            } else {
                setDialogResult( BDialogResult.OK );
            }
#else // USE_OLD_SYNTH_IMPL
            timer.stop();
            if ( bgWork.isBusy() ) {
                mIsCancelRequired = true;
                if ( AppManager.waveGenerator != null ) {
                    AppManager.waveGenerator.stop();
                    AppManager.waveGenerator = null;
                }
                setDialogResult( BDialogResult.CANCEL );
            } else {
                setDialogResult( BDialogResult.OK );
            }
#endif // USE_OLD_SYNTH_IMPL
        }

        public void bgWork_RunWorkerCompleted( Object sender, BRunWorkerCompletedEventArgs e ) {
            timer.stop();
            setDialogResult( BDialogResult.OK );
            close();
        }

        public void timer_Tick( Object sender, BEventArgs e ) {
#if USE_OLD_SYNTH_IMPL
            double progress = VSTiProxy.getProgress();
#else // USE_OLD_SYNTH_IMPL
            double progress = AppManager.waveGenerator.getProgress() * 100;
#endif // USE_OLD_SYNTH_IMPL

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
#if USE_OLD_SYNTH_IMPL
            VSTiProxy.abortRendering();
#else
            AppManager.waveGenerator.stop();
#endif
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
