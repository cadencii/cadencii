/*
 * FormGenerateKeySound.cs
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
using System;
using System.Windows.Forms;
using org.kbinani.componentmodel;
using org.kbinani.java.util;
using org.kbinani.media;
using org.kbinani.vsq;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using BDoWorkEventArgs = System.ComponentModel.DoWorkEventArgs;
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;
    using BRunWorkerCompletedEventArgs = System.ComponentModel.RunWorkerCompletedEventArgs;

    public class FormGenerateKeySound : BForm {
        public class PrepareStartArgument {
            public String singer = "Miku";
            public double amplitude = 1.0;
            public String directory = "";
            public boolean replace = true;
        }

        const int _SAMPLE_RATE = 44100;
        private SingerConfig[] m_singer_config1;
        private SingerConfig[] m_singer_config2;
        private SingerConfig[] m_singer_config_utau;
        private boolean m_cancel_required = false;
        /// <summary>
        /// 処理が終わったら自動でフォームを閉じるかどうか。デフォルトではfalse（閉じない）
        /// </summary>
        private boolean m_close_when_finished = false;

        private BButton btnExecute;
        private BButton btnCancel;
        private BComboBox comboSingingSynthSystem;
        private BLabel lblSingingSynthSystem;
        private BLabel lblSinger;
        private BComboBox comboSinger;
        private BCheckBox chkIgnoreExistingWavs;
        private BTextBox txtDir;
        private BButton btnBrowse;
        private FolderBrowserDialog folderBrowser;
        private BBackgroundWorker bgWork;
        private BLabel lblDir;

        public FormGenerateKeySound( boolean close_when_finished ) {
            InitializeComponent();
            m_close_when_finished = close_when_finished;
            m_singer_config1 = VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID1 );
            m_singer_config2 = VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID2 );
            m_singer_config_utau = AppManager.editorConfig.UtauSingers.toArray( new SingerConfig[] { } );
            if ( m_singer_config1.Length > 0 ) {
                comboSingingSynthSystem.addItem( "VOCALOID1" );
            }
            if ( m_singer_config2.Length > 0 ) {
                comboSingingSynthSystem.addItem( "VOCALOID2" );
            }
            if ( m_singer_config_utau.Length > 0 ) {
                comboSingingSynthSystem.addItem( "UTAU" );
            }
            if ( comboSingingSynthSystem.getItemCount() > 0 ) {
                comboSingingSynthSystem.setSelectedIndex( 0 );
            }
            updateSinger();
            txtDir.setText( AppManager.getKeySoundPath() );
        }

        private void updateSinger() {
            if ( comboSingingSynthSystem.getSelectedIndex() < 0 ) {
                return;
            }
            String singer = (String)comboSingingSynthSystem.getSelectedItem();
            SingerConfig[] list = null;
            if ( singer.Equals( "VOCALOID1" ) ) {
                list = m_singer_config1;
            } else if ( singer.Equals( "VOCALOID2" ) ) {
                list = m_singer_config2;
            } else if ( singer.Equals( "UTAU" ) ) {
                list = m_singer_config_utau;
            }
            comboSinger.removeAllItems();
            if ( list == null ) {
                return;
            }
            for ( int i = 0; i < list.Length; i++ ) {
                comboSinger.addItem( list[i].VOICENAME );
            }
            if ( comboSinger.getItemCount() > 0 ) {
                comboSinger.setSelectedIndex( 0 );
            }
        }

        private void InitializeComponent() {
            this.btnExecute = new BButton();
            this.btnCancel = new BButton();
            this.comboSingingSynthSystem = new BComboBox();
            this.lblSingingSynthSystem = new BLabel();
            this.lblSinger = new BLabel();
            this.comboSinger = new BComboBox();
            this.chkIgnoreExistingWavs = new BCheckBox();
            this.txtDir = new BTextBox();
            this.btnBrowse = new BButton();
            this.lblDir = new BLabel();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.bgWork = new BBackgroundWorker();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecute.Location = new System.Drawing.Point( 286, 126 );
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size( 75, 23 );
            this.btnExecute.TabIndex = 0;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler( this.btnExecute_Click );
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point( 205, 126 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler( this.btnCancel_Click );
            // 
            // comboSingingSynthSystem
            // 
            this.comboSingingSynthSystem.FormattingEnabled = true;
            this.comboSingingSynthSystem.Location = new System.Drawing.Point( 151, 12 );
            this.comboSingingSynthSystem.Name = "comboSingingSynthSystem";
            this.comboSingingSynthSystem.Size = new System.Drawing.Size( 121, 20 );
            this.comboSingingSynthSystem.TabIndex = 2;
            this.comboSingingSynthSystem.SelectedIndexChanged += new System.EventHandler( this.comboSingingSynthSystem_SelectedIndexChanged );
            // 
            // lblSingingSynthSystem
            // 
            this.lblSingingSynthSystem.AutoSize = true;
            this.lblSingingSynthSystem.Location = new System.Drawing.Point( 12, 15 );
            this.lblSingingSynthSystem.Name = "lblSingingSynthSystem";
            this.lblSingingSynthSystem.Size = new System.Drawing.Size( 119, 12 );
            this.lblSingingSynthSystem.TabIndex = 3;
            this.lblSingingSynthSystem.Text = "Singing Synth. System";
            // 
            // lblSinger
            // 
            this.lblSinger.AutoSize = true;
            this.lblSinger.Location = new System.Drawing.Point( 12, 39 );
            this.lblSinger.Name = "lblSinger";
            this.lblSinger.Size = new System.Drawing.Size( 37, 12 );
            this.lblSinger.TabIndex = 4;
            this.lblSinger.Text = "Singer";
            // 
            // comboSinger
            // 
            this.comboSinger.FormattingEnabled = true;
            this.comboSinger.Location = new System.Drawing.Point( 151, 36 );
            this.comboSinger.Name = "comboSinger";
            this.comboSinger.Size = new System.Drawing.Size( 121, 20 );
            this.comboSinger.TabIndex = 5;
            // 
            // chkIgnoreExistingWavs
            // 
            this.chkIgnoreExistingWavs.AutoSize = true;
            this.chkIgnoreExistingWavs.Checked = true;
            this.chkIgnoreExistingWavs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIgnoreExistingWavs.Location = new System.Drawing.Point( 12, 63 );
            this.chkIgnoreExistingWavs.Name = "chkIgnoreExistingWavs";
            this.chkIgnoreExistingWavs.Size = new System.Drawing.Size( 135, 16 );
            this.chkIgnoreExistingWavs.TabIndex = 6;
            this.chkIgnoreExistingWavs.Text = "Ignore Existing WAVs";
            this.chkIgnoreExistingWavs.UseVisualStyleBackColor = true;
            // 
            // txtDir
            // 
            this.txtDir.Location = new System.Drawing.Point( 94, 88 );
            this.txtDir.Name = "txtDir";
            this.txtDir.Size = new System.Drawing.Size( 209, 19 );
            this.txtDir.TabIndex = 7;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point( 309, 86 );
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size( 40, 23 );
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler( this.btnBrowse_Click );
            // 
            // lblDir
            // 
            this.lblDir.AutoSize = true;
            this.lblDir.Location = new System.Drawing.Point( 12, 91 );
            this.lblDir.Name = "lblDir";
            this.lblDir.Size = new System.Drawing.Size( 66, 12 );
            this.lblDir.TabIndex = 9;
            this.lblDir.Text = "Output Path";
            // 
            // bgWork
            // 
            this.bgWork.WorkerReportsProgress = true;
            this.bgWork.WorkerSupportsCancellation = true;
            this.bgWork.DoWork += new System.ComponentModel.DoWorkEventHandler( this.bgWork_DoWork );
            this.bgWork.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.bgWork_RunWorkerCompleted );
            this.bgWork.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler( this.bgWork_ProgressChanged );
            // 
            // Program
            // 
            this.ClientSize = new System.Drawing.Size( 373, 161 );
            this.Controls.Add( this.lblDir );
            this.Controls.Add( this.btnBrowse );
            this.Controls.Add( this.txtDir );
            this.Controls.Add( this.chkIgnoreExistingWavs );
            this.Controls.Add( this.comboSinger );
            this.Controls.Add( this.lblSinger );
            this.Controls.Add( this.lblSingingSynthSystem );
            this.Controls.Add( this.comboSingingSynthSystem );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnExecute );
            this.Name = "Program";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler( this.Program_FormClosed );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        private void comboSingingSynthSystem_SelectedIndexChanged( Object sender, BEventArgs e ) {
            updateSinger();
        }

        private void btnBrowse_Click( Object sender, BEventArgs e ) {
            folderBrowser.SelectedPath = txtDir.getText();
            if ( folderBrowser.ShowDialog() != DialogResult.OK ) {
                return;
            }
            txtDir.setText( folderBrowser.SelectedPath );
        }

        private void btnCancel_Click( Object sender, BEventArgs e ) {
            if ( bgWork.isBusy() ) {
                m_cancel_required = true;
                while ( m_cancel_required ) {
                    Application.DoEvents();
                }
            } else {
                this.close();
            }
        }

        private void btnExecute_Click( Object sender, BEventArgs e ) {
            PrepareStartArgument arg = new PrepareStartArgument();
            arg.singer = (String)comboSinger.getSelectedItem();
            arg.amplitude = 1.0;
            arg.directory = txtDir.getText();
            arg.replace = chkIgnoreExistingWavs.isSelected();
            updateEnabled( false );
            bgWork.runWorkerAsync( arg );
        }

        public static unsafe void GenerateSinglePhone( int note, String singer, String file, double amp ) {
            String renderer = "";
            SingerConfig[] singers1 = VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID1 );
            int c = singers1.Length;
            String first_found_singer = "";
            String first_found_renderer = "";
            for ( int i = 0; i < c; i++ ) {
                if ( first_found_singer.Equals( "" ) ) {
                    first_found_singer = singers1[i].VOICENAME;
                    first_found_renderer = VSTiProxy.RENDERER_DSB2;
                }
                if ( singers1[i].VOICENAME.Equals( singer ) ) {
                    renderer = VSTiProxy.RENDERER_DSB2;
                    break;
                }
            }

            SingerConfig[] singers2 = VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID2 );
            c = singers2.Length;
            for ( int i = 0; i < c; i++ ) {
                if ( first_found_singer.Equals( "" ) ) {
                    first_found_singer = singers2[i].VOICENAME;
                    first_found_renderer = VSTiProxy.RENDERER_DSB3;
                }
                if ( singers2[i].VOICENAME.Equals( singer ) ) {
                    renderer = VSTiProxy.RENDERER_DSB3;
                    break;
                }
            }

            for ( Iterator<SingerConfig> itr = AppManager.editorConfig.UtauSingers.iterator(); itr.hasNext(); ) {
                SingerConfig sc = itr.next();
                if ( first_found_singer.Equals( "" ) ) {
                    first_found_singer = sc.VOICENAME;
                    first_found_renderer = VSTiProxy.RENDERER_UTU0;
                }
                if ( sc.VOICENAME.Equals( singer ) ) {
                    renderer = VSTiProxy.RENDERER_UTU0;
                    break;
                }
            }

            VsqFileEx vsq = new VsqFileEx( singer, 1, 4, 4, 500000 );
            if ( renderer.Equals( "" ) ) {
                singer = first_found_singer;
                renderer = first_found_renderer;
            }
            vsq.Track.get( 1 ).getCommon().Version = renderer;
            VsqEvent item = new VsqEvent( 1920, new VsqID( 0 ) );
            item.ID.LyricHandle = new LyricHandle( "あ", "a" );
            item.ID.setLength( 480 );
            item.ID.Note = note;
            item.ID.VibratoHandle = null;
            item.ID.type = VsqIDType.Anote;
            vsq.Track.get( 1 ).addEvent( item );
            vsq.updateTotalClocks();
            int ms_presend = 500;
            String tempdir = PortUtil.combinePath( AppManager.getCadenciiTempDir(), AppManager.getID() );
            if ( !PortUtil.isDirectoryExists( tempdir ) ) {
                try {
                    PortUtil.createDirectory( tempdir );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "Program#GenerateSinglePhone; ex=" + ex );
                    return;
                }
            }
            WaveWriter ww = null;
            try {
                ww = new WaveWriter( file );
                VSTiProxy.render( vsq,
                                  1,
                                  ww,
                                  0.0,
                                  vsq.getSecFromClock( vsq.TotalClocks ) + 1.0,
                                  ms_presend,
                                  false,
                                  new WaveReader[] { },
                                  0.0,
                                  false,
                                  tempdir,
                                  false );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "FormGenerateKeySound#GenerateSinglePhone; ex=" + ex );
            } finally {
                if ( ww != null ) {
                    try {
                        ww.close();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "FormGenerateKeySound#GenerateSinglePhone; ex2=" + ex2 );
                    }
                }
            }
        }

        private void bgWork_DoWork( Object sender, BDoWorkEventArgs e ) {
            PrepareStartArgument arg = (PrepareStartArgument)e.Argument;
            String singer = arg.singer;
            double amp = arg.amplitude;
            String dir = arg.directory;
            boolean replace = arg.replace;
            // 音源を準備
            if ( !PortUtil.isDirectoryExists( dir ) ) {
                PortUtil.createDirectory( dir );
            }

            for ( int i = 0; i < 127; i++ ) {
                string path = PortUtil.combinePath( dir, i + ".wav" );
                Console.Write( "writing \"" + path + "\" ..." );
                if ( replace || (!replace && !PortUtil.isFileExists( path )) ) {
                    try {
                        GenerateSinglePhone( i, singer, path, amp );
                        if ( PortUtil.isFileExists( path ) ) {
                            try {
                                Wave wv = new Wave( path );
                                wv.trimSilence();
                                wv.monoralize();
                                wv.write( path );
                            } catch( Exception ex0 ) {
                                PortUtil.stderr.println( "FormGenerateKeySound#bgWork_DoWork; ex0=" + ex0 );
                            }
                        }
                    } catch ( Exception ex ){
                        PortUtil.stderr.println( "FormGenerateKeySound#bgWork_DoWork; ex=" + ex );
                    }
                }
                PortUtil.println( " done" );
                if ( m_cancel_required ) {
                    m_cancel_required = false;
                    break;
                }
                bgWork.reportProgress( (int)(i / 127.0 * 100.0) );
            }
            m_cancel_required = false;
        }

        private void bgWork_ProgressChanged( Object sender, System.ComponentModel.ProgressChangedEventArgs e ) {
            this.Invoke( new updateTitleDelegate( this.updateTitle ), new Object[] { "Progress: " + e.ProgressPercentage + "%" } );
        }

        private delegate void updateTitleDelegate( String title );

        private void updateTitle( String title ) {
            setTitle( title );
        }

        private void Program_FormClosed( Object sender, FormClosedEventArgs e ) {
            VSTiProxy.terminate();
        }

        private void bgWork_RunWorkerCompleted( Object sender, BRunWorkerCompletedEventArgs e ) {
            updateEnabled( true );
            if ( m_close_when_finished ) {
                close();
            }
        }

        private void updateEnabled( boolean enabled ) {
            comboSinger.setEnabled( enabled );
            comboSingingSynthSystem.setEnabled( enabled );
            txtDir.ReadOnly = !enabled;
            btnBrowse.setEnabled( enabled );
            btnExecute.setEnabled( enabled );
            chkIgnoreExistingWavs.setEnabled( enabled );
            if ( enabled ) {
                btnCancel.setText( "Close" );
            } else {
                btnCancel.setText( "Cancel" );
            }
        }

    }

}
