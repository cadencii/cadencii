/*
 * FormGenerateStf.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.editotoini.
 *
 * org.kbinani.editotoini is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.editotoini is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.editotoini;

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/EditOtoIni/FormGenerateStf.java

import java.util.*;
import org.kbinani.*;
import org.kbinani.componentmodel.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using org.kbinani;
using org.kbinani.java.util;
using org.kbinani.windows.forms;
using org.kbinani.componentmodel;

namespace org.kbinani.editotoini {
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;
    using BDoWorkEventArgs = System.ComponentModel.DoWorkEventArgs;
#endif

#if JAVA
    public class FormGenerateStf extends BDialog {
#else
    public class FormGenerateStf : BDialog {
#endif
        public enum GenerateMode {
            FRQ,
            STF,
        }

        private Vector<StfQueueArgs> m_list_files;
        private String m_oto_ini;
        private boolean m_abort_required = false;
        private GenerateMode m_mode = GenerateMode.STF;
        private long m_remaining = 0;
        private long m_elapsed = 0;
        private BBackgroundWorker bgWork;

        public FormGenerateStf( String oto_ini, Vector<StfQueueArgs> list, GenerateMode mode ) {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            bgWork = new BBackgroundWorker();
            bgWork.setWorkerReportsProgress( true );
            registerEventHandlers();
            setResources();
            m_oto_ini = oto_ini;
            m_list_files = list;
            m_mode = mode;
            btnCancel.setText( _( "Cancel" ) );
            setTitle( PortUtil.formatMessage( _( "Generate {0} file" ), mode + "" ) );
        }

        private void FormGenerateStf_Load( Object sender, BEventArgs e ) {
            bgWork.runWorkerAsync();
        }

        private static String _( String id ) {
            return org.kbinani.apputil.Messaging.getMessage( id );
        }

        private void bgWork_DoWork( Object sender, BDoWorkEventArgs e ){
#if DEBUG
            Console.WriteLine( "FormUtauVoiceConfig#bgWork_DoWork; m_oto_ini=" + m_oto_ini );
#endif
            if ( m_oto_ini.Equals( "" ) ) {
                return;
            }
            if ( !PortUtil.isFileExists( m_oto_ini ) ) {
                return;
            }
            String straightVoiceDB = PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "straightVoiceDB.exe" );
            String resampler = AppManager.getPathResampler();
#if DEBUG
            Console.WriteLine( "FormUtauVoiceConfig#bgWork_DoWork; straightVoiceDB=" + straightVoiceDB );
#endif
#if JAVA
            long started_date = new Date().getTime();
#else
            long started_date = (long)(DateTime.Now.Ticks * 100.0 / 1e9);
#if DEBUG
            PortUtil.println( "DateTime.Now.Ticks=" + DateTime.Now.Ticks );
            PortUtil.println( "started_date=" + started_date );
#endif
#endif
            if ( m_mode == GenerateMode.STF ) {
                #region STF
                if ( !PortUtil.isFileExists( straightVoiceDB ) ) {
                    org.kbinani.windows.forms.Utility.showMessageBox( _( "Analyzer, 'straightVoiceDB.exe' does not exist." ),
                                             _( "Error" ),
                                             PortUtil.OK_OPTION,
                                             org.kbinani.windows.forms.Utility.MSGBOX_WARNING_MESSAGE );
                    return;
                }
                String dir = PortUtil.getDirectoryName( m_oto_ini );
                String analyzed = PortUtil.combinePath( dir, "analyzed" );
                if ( !PortUtil.isDirectoryExists( analyzed ) ) {
                    PortUtil.createDirectory( analyzed );
                }

                int count = m_list_files.size();
                int actual_count = 0;
                double total_bytes = 0.0;//処理しなければならないwaveファイルの容量
                double processed_bytes = 0.0;//処理済のwaveファイルの容量
                for ( int i = 0; i < count; i++ ) {
                    StfQueueArgs item = m_list_files.get( i );
                    String wav_name = item.waveName;
                    String wav_file = PortUtil.combinePath( dir, wav_name );
                    String stf_file = PortUtil.combinePath( analyzed, PortUtil.getFileNameWithoutExtension( wav_name ) + ".stf" );
                    if ( PortUtil.isFileExists( stf_file ) ) {
                        continue;
                    }
                    try {
                        total_bytes += PortUtil.getFileLength( wav_file );
                    } catch ( Exception ex ) {
                    }
                    actual_count++;
                }
                bgWork.reportProgress( 0, new int[] { 0, actual_count } );

                int actual_progress = 0;
                for ( int i = 0; i < count; i++ ) {
                    if ( m_abort_required ) {
                        break;
                    }
                    StfQueueArgs item = m_list_files.get( i );
                    String wav_name = item.waveName;
                    String sOffset = item.offset;
                    String sBlank = item.blank;
#if DEBUG
                    Console.WriteLine( "FormUtauVoiceConfig#bgWork_DoWork; wav_name=" + wav_name );
#endif
                    String wav_file = PortUtil.combinePath( dir, wav_name );
                    String stf_file = PortUtil.combinePath( analyzed, PortUtil.getFileNameWithoutExtension( wav_name ) + ".stf" );
                    if ( PortUtil.isFileExists( stf_file ) ) {
                        continue;
                    }
                    Process process = null;
                    try {
#if JAVA
                        Runtime r = Runtime.getRuntime();
                        Vector<String> cmds = new Vector<String>();
                        cmds.add( straightVoiceDB );
                        cmds.add( wav_file );
                        cmds.add( stf_file );
                        cmds.add( sOffset + "" );
                        cmds.add( sBlank + "" );
                        process = r.exec( cmds.toArray( new String[]{} ) );
                        process.waitFor();
#else
                        process = new Process();
                        process.StartInfo.FileName = straightVoiceDB;
                        process.StartInfo.Arguments = "\"" + wav_file + "\" \"" + stf_file + "\" " + sOffset + " " + sBlank;
                        process.StartInfo.WorkingDirectory = Application.StartupPath;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        process.Start();
                        process.PriorityClass = ProcessPriorityClass.BelowNormal;
                        process.WaitForExit();
#endif
                    } catch ( Exception ex ) {
                    } finally {
                        if ( process != null ) {
                            try {
#if JAVA
                                process.destroy();
#else
                                process.Dispose();
#endif
                            } catch ( Exception ex2 ) {
                            }
                        }
                    }
                    actual_progress++;
                    try {
                        processed_bytes += PortUtil.getFileLength( wav_file );
                    } catch ( Exception ex ) {
                    }

#if JAVA
#else
                    m_elapsed = (long)(DateTime.Now.Ticks * 100.0 / 1e9) - started_date;
#endif
                    double elapsed_seconds = m_elapsed;
                    double remaining_seconds = total_bytes * elapsed_seconds / processed_bytes - elapsed_seconds;
                    m_remaining = (long)remaining_seconds;
                    // 
                    bgWork.reportProgress( actual_progress * 100 / actual_count, new int[] { actual_progress, actual_count } );
                }
                bgWork.reportProgress( 100, new int[] { actual_count, actual_count } );
                #endregion
            } else {
                if ( !PortUtil.isFileExists( resampler ) ) {
                    org.kbinani.windows.forms.Utility.showMessageBox( _( "Don't know the path of 'resampler.exe'. Please check the configuration of Cadencii." ),
                                     _( "Error" ),
                                     PortUtil.OK_OPTION,
                                     org.kbinani.windows.forms.Utility.MSGBOX_WARNING_MESSAGE );
                    return;
                }
                String dir = PortUtil.getDirectoryName( m_oto_ini );

                int count = m_list_files.size();
                int actual_count = 0;
                double total_bytes = 0.0;
                double processed_bytes = 0.0;
                for ( int i = 0; i < count; i++ ) {
                    StfQueueArgs item = m_list_files.get( i );
                    String wav_name = item.waveName;
                    String wav_file = PortUtil.combinePath( dir, wav_name );
                    String frq_file = PortUtil.combinePath( dir, wav_name.Replace( ".", "_" ) + ".frq" );
                    if ( PortUtil.isFileExists( frq_file ) ) {
                        continue;
                    }
                    try {
                        total_bytes += PortUtil.getFileLength( wav_file );
                    } catch ( Exception ex ) {
                    }
                    actual_count++;
                }
                bgWork.reportProgress( 0, new int[] { 0, actual_count } );

                String temp_wav = PortUtil.createTempFile() + ".wav";
                int actual_progress = 0;
                for ( int i = 0; i < count; i++ ) {
                    if ( m_abort_required ) {
                        break;
                    }
                    StfQueueArgs item = m_list_files.get( i );
                    String wav_name = item.waveName;
#if DEBUG
                    Console.WriteLine( "FormUtauVoiceConfig#bgWork_DoWork; wav_name=" + wav_name );
#endif
                    String wav_file = PortUtil.combinePath( dir, wav_name );
                    String frq_file = PortUtil.combinePath( dir, wav_name.Replace( ".", "_" ) + ".frq" );
                    if ( PortUtil.isFileExists( frq_file ) ) {
                        continue;
                    }
                    Process process = null;
                    try {
#if JAVA
                        Runtime r = Runtime.getRuntime();
                        Vector<String> cmds = new Vector<String>();
                        cmds.add( resampler );
                        cmds.add( wav_file );
                        cmds.add( temp_wav );
                        cmds.add( "C3" );
                        cmds.add( "100" );
                        process = r.exec( cmds.toArray( new String[]{} ) );
                        process.waitFor();
#else
                        process = new Process();
                        process.StartInfo.FileName = resampler;
                        process.StartInfo.Arguments = "\"" + wav_file + "\" \"" + temp_wav + "\" C3 100";
                        process.StartInfo.WorkingDirectory = Application.StartupPath;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        process.Start();
                        process.PriorityClass = ProcessPriorityClass.BelowNormal;
                        process.WaitForExit();
#endif
                    } catch ( Exception ex ) {
                    } finally {
                        if ( process != null ) {
                            try {
#if JAVA
                                process.destroy();
#else
                                process.Dispose();
#endif
                            } catch ( Exception ex2 ) {
                            }
                        }
                    }

                    actual_progress++;
                    try {
                        processed_bytes += PortUtil.getFileLength( wav_file );
                    } catch ( Exception ex ) {
                    }

#if JAVA
                    m_elapsed = (new Date()).getTime() - started_date;
#else
                    m_elapsed = (long)(DateTime.Now.Ticks * 100.0 / 1e9) - started_date;
#endif
                    double elapsed_seconds = m_elapsed;
                    double remaining_seconds = total_bytes * elapsed_seconds / processed_bytes - elapsed_seconds;
                    m_remaining = (long)remaining_seconds;
                    // 
                    bgWork.reportProgress( actual_progress * 100 / actual_count, new int[] { actual_progress, actual_count } );
                }
                try {
                    PortUtil.deleteFile( temp_wav );
                } catch ( Exception ex ) {
                }
                bgWork.reportProgress( 100, new int[] { actual_count, actual_count } );
            }
        }

#if JAVA
        private void bgWork_ProgressChanged( Object sender, BProgressChangedEventArgs e )
#else
        private void bgWork_ProgressChanged( Object sender, ProgressChangedEventArgs e )
#endif
        {
            progressBar.setValue( e.ProgressPercentage );

            if ( e.UserState is int[] ) {
                int[] rational = (int[])e.UserState;
                if ( rational.Length >= 2 ) {
                    lblPercent.setText( e.ProgressPercentage + " % (" + rational[0] + "/" + rational[1] + ")" );
                } else {
                    lblPercent.setText( e.ProgressPercentage + " %" );
                }
            } else {
                lblPercent.setText( e.ProgressPercentage + " %" );
            }

            lblTime.setText( _( "Remaining" ) + " " + getTimeSpanString( m_remaining ) + " (" + getTimeSpanString( m_elapsed ) + " " + _( "elapsed" ) + ")" );
        }

        private void btnCancel_Click( Object sender, BEventArgs e ) {
            m_abort_required = true;
            if ( bgWork.isBusy() ) {
                while ( bgWork.isBusy() ) {
#if JAVA
                    Thread.sleep( 0 );
#else
                    Application.DoEvents();
#endif
                }
            }
            setDialogResult( BDialogResult.CANCEL );
        }

#if JAVA
        private void bgWork_RunWorkerCompleted( Object sender, BRunWorkerCompletedEventArgs e )
#else
        private void bgWork_RunWorkerCompleted( Object sender, RunWorkerCompletedEventArgs e )
#endif
        {
            this.close();
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
#if JAVA
            loadEvent.add( new BEventHandler( this, "FormGenerateStf_Load" ) );
            btnCancel.clickEvent.add( new BEventHandler( this, "btnCancel_Click" ) );
            bgWork.doWorkEvent.add( new BDoWorkEventHandler( this, "bgWork_DoWork" ) );
            bgWork.progressChangedEvent.add( new BProgressChangedEventHandler( this, "bgWork_ProgressChanged" ) );
            bgWork.runWorkerCompletedEvent.add( new BRunWorkerCompletedEventHandler( this, "bgWork_WorkerCompleted" ) );
#else
            this.btnCancel.Click += new System.EventHandler( this.btnCancel_Click );
            this.bgWork.DoWork += new System.ComponentModel.DoWorkEventHandler( this.bgWork_DoWork );
            this.bgWork.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.bgWork_RunWorkerCompleted );
            this.bgWork.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler( this.bgWork_ProgressChanged );
            this.Load += new System.EventHandler( this.FormGenerateStf_Load );
#endif
        }

        private void setResources() {
        }

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/EditOtoIni/FormGenerateStf.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/EditOtoIni/FormGenerateStf.java
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
        protected override void Dispose( bool disposing ) {
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
            this.lblPercent = new BLabel();
            this.progressBar = new BProgressBar();
            this.btnCancel = new BButton();
            this.lblTime = new BLabel();
            this.SuspendLayout();
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point( 12, 17 );
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size( 21, 12 );
            this.lblPercent.TabIndex = 0;
            this.lblPercent.Text = "0 %";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point( 14, 50 );
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size( 345, 23 );
            this.progressBar.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 284, 86 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point( 12, 35 );
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size( 137, 12 );
            this.lblTime.TabIndex = 3;
            this.lblTime.Text = "remaining 0s (elapsed 0s)";
            // 
            // FormGenerateStf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 371, 121 );
            this.Controls.Add( this.lblTime );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.progressBar );
            this.Controls.Add( this.lblPercent );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGenerateStf";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormGenerateStf";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BLabel lblPercent;
        private BProgressBar progressBar;
        private BButton btnCancel;
        private BLabel lblTime;
        #endregion
#endif
    }

#if !JAVA
}
#endif
