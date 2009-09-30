/*
 * FormGenerateStf.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.EditOtoIni.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using bocoree;

namespace Boare.EditOtoIni {

    using boolean = Boolean;

    public partial class FormGenerateStf : Form {
        public enum GenerateMode {
            FRQ,
            STF,
        }

        private Vector<StfQueueArgs> m_list_files;
        private String m_oto_ini;
        private boolean m_abort_required = false;
        private GenerateMode m_mode = GenerateMode.STF;
        private TimeSpan m_remaining = new TimeSpan( 0, 0, 0 );
        private TimeSpan m_elapsed = new TimeSpan( 0, 0, 0 );

        public FormGenerateStf( String oto_ini, Vector<StfQueueArgs> list, GenerateMode mode ) {
            InitializeComponent();
            m_oto_ini = oto_ini;
            m_list_files = list;
            m_mode = mode;
            btnCancel.Text = _( "Cancel" );
            this.Text = string.Format( _( "Generate {0} file" ), mode + "" );
        }

        private void FormGenerateStf_Load( object sender, EventArgs e ) {
            bgWork.RunWorkerAsync();
        }

        private static String _( String id ) {
            return Boare.Lib.AppUtil.Messaging.GetMessage( id );
        }

        private void bgWork_DoWork( object sender, DoWorkEventArgs e ) {
#if DEBUG
            Console.WriteLine( "FormUtauVoiceConfig#bgWork_DoWork; m_oto_ini=" + m_oto_ini );
#endif
            if ( m_oto_ini.Equals( "" ) ) {
                return;
            }
            if ( !File.Exists( m_oto_ini ) ) {
                return;
            }
            String straightVoiceDB = Path.Combine( Application.StartupPath, "straightVoiceDB.exe" );
            String resampler = AppManager.cadenciiConfig.PathResampler;
#if DEBUG
            Console.WriteLine( "FormUtauVoiceConfig#bgWork_DoWork; straightVoiceDB=" + straightVoiceDB );
#endif
            DateTime started_date = DateTime.Now;
            if ( m_mode == GenerateMode.STF ) {
                #region STF
                if ( !File.Exists( straightVoiceDB ) ) {
                    MessageBox.Show( _( "Analyzer, 'straightVoiceDB.exe' does not exist." ),
                                     _( "Error" ),
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Exclamation );
                    return;
                }
                String dir = Path.GetDirectoryName( m_oto_ini );
                String analyzed = Path.Combine( dir, "analyzed" );
                if ( !Directory.Exists( analyzed ) ) {
                    Directory.CreateDirectory( analyzed );
                }

                int count = m_list_files.size();
                int actual_count = 0;
                double total_bytes = 0.0;//処理しなければならないwaveファイルの容量
                double processed_bytes = 0.0;//処理済のwaveファイルの容量
                for ( int i = 0; i < count; i++ ) {
                    StfQueueArgs item = m_list_files.get( i );
                    String wav_name = item.waveName;
                    String wav_file = Path.Combine( dir, wav_name );
                    String stf_file = Path.Combine( analyzed, Path.GetFileNameWithoutExtension( wav_name ) + ".stf" );
                    if ( File.Exists( stf_file ) ) {
                        continue;
                    }
                    try {
                        total_bytes += new FileInfo( wav_file ).Length;
                    } catch {
                    }
                    actual_count++;
                }
                bgWork.ReportProgress( 0, new int[] { 0, actual_count } );

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
                    String wav_file = Path.Combine( dir, wav_name );
                    String stf_file = Path.Combine( analyzed, Path.GetFileNameWithoutExtension( wav_name ) + ".stf" );
                    if ( File.Exists( stf_file ) ) {
                        continue;
                    }
                    using ( Process process = new Process() ) {
                        process.StartInfo.FileName = straightVoiceDB;
                        process.StartInfo.Arguments = "\"" + wav_file + "\" \"" + stf_file + "\" " + sOffset + " " + sBlank;
                        process.StartInfo.WorkingDirectory = Application.StartupPath;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        process.Start();
                        process.PriorityClass = ProcessPriorityClass.BelowNormal;
                        process.WaitForExit();
                    }
                    actual_progress++;
                    try {
                        processed_bytes += new FileInfo( wav_file ).Length;
                    } catch {
                    }

                    m_elapsed = DateTime.Now.Subtract( started_date );
                    double elapsed_seconds = m_elapsed.TotalSeconds;
                    double remaining_seconds = total_bytes * elapsed_seconds / processed_bytes - elapsed_seconds;
                    m_remaining = new TimeSpan( 0, 0, 0, (int)remaining_seconds );
                    // 
                    bgWork.ReportProgress( actual_progress * 100 / actual_count, new int[] { actual_progress, actual_count } );
                }
                bgWork.ReportProgress( 100, new int[] { actual_count, actual_count } );
                #endregion
            } else {
                if ( !File.Exists( resampler ) ) {
                    MessageBox.Show( _( "Don't know the path of 'resampler.exe'. Please check the configuration of Cadencii." ),
                                     _( "Error" ),
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Exclamation );
                    return;
                }
                String dir = Path.GetDirectoryName( m_oto_ini );

                int count = m_list_files.size();
                int actual_count = 0;
                double total_bytes = 0.0;
                double processed_bytes = 0.0;
                for ( int i = 0; i < count; i++ ) {
                    StfQueueArgs item = m_list_files.get( i );
                    String wav_name = item.waveName;
                    String wav_file = Path.Combine( dir, wav_name );
                    String frq_file = Path.Combine( dir, wav_name.Replace( ".", "_" ) + ".frq" );
                    if ( File.Exists( frq_file ) ) {
                        continue;
                    }
                    try {
                        total_bytes += new FileInfo( wav_file ).Length;
                    } catch {
                    }
                    actual_count++;
                }
                bgWork.ReportProgress( 0, new int[] { 0, actual_count } );

                String temp_wav = Path.GetTempFileName() + ".wav";
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
                    String wav_file = Path.Combine( dir, wav_name );
                    String frq_file = Path.Combine( dir, wav_name.Replace( ".", "_" ) + ".frq" );
                    if ( File.Exists( frq_file ) ) {
                        continue;
                    }
                    using ( Process process = new Process() ) {
                        process.StartInfo.FileName = resampler;
                        process.StartInfo.Arguments = "\"" + wav_file + "\" \"" + temp_wav + "\" C3 100";
                        process.StartInfo.WorkingDirectory = Application.StartupPath;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        process.Start();
                        process.PriorityClass = ProcessPriorityClass.BelowNormal;
                        process.WaitForExit();
                    }
                    actual_progress++;
                    try {
                        processed_bytes += new FileInfo( wav_file ).Length;
                    } catch {
                    }

                    m_elapsed = DateTime.Now.Subtract( started_date );
                    double elapsed_seconds = m_elapsed.TotalSeconds;
                    double remaining_seconds = total_bytes * elapsed_seconds / processed_bytes - elapsed_seconds;
                    m_remaining = new TimeSpan( 0, 0, 0, (int)remaining_seconds );
                    // 
                    bgWork.ReportProgress( actual_progress * 100 / actual_count, new int[] { actual_progress, actual_count } );
                }
                try {
                    File.Delete( temp_wav );
                } catch {
                }
                bgWork.ReportProgress( 100, new int[] { actual_count, actual_count } );
            }
        }

        private void bgWork_ProgressChanged( object sender, ProgressChangedEventArgs e ) {
            progressBar.Value = e.ProgressPercentage;

            if( e.UserState is int[] ){
                int[] rational = (int[])e.UserState;
                if ( rational.Length >= 2 ) {
                    lblPercent.Text = e.ProgressPercentage + " % (" + rational[0] + "/" + rational[1] + ")";
                } else {
                    lblPercent.Text = e.ProgressPercentage + " %";
                }
            }else{
                lblPercent.Text = e.ProgressPercentage + " %";
            }

            lblTime.Text = _( "Remaining" ) + " " + getTimeSpanString( m_remaining ) + " (" + getTimeSpanString( m_elapsed ) + " " + _( "elapsed" ) + ")";
        }

        private void btnCancel_Click( object sender, EventArgs e ) {
            m_abort_required = true;
        }

        private void bgWork_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e ) {
            this.Close();
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
