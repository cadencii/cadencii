/*
 * Program.cs
 * Copyright (C) 2008-2010 kbinani
 *
 * This file is part of org.kbinani.generatekeysound.
 *
 * org.kbinani.generatekeysound is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.generatekeysound is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.ComponentModel;
using System.Windows.Forms;
using org.kbinani;
using org.kbinani.cadencii;
using org.kbinani.java.util;
using org.kbinani.media;
using org.kbinani.vsq;

namespace org.kbinani.generatekeysound {

    class Program : Form {
        const int _SAMPLE_RATE = 44100;
        private SingerConfig[] m_singer_config1;
        private SingerConfig[] m_singer_config2;
        private SingerConfig[] m_singer_config_utau;
        private bool m_cancel_required = false;

        private Button btnExecute;
        private Button btnCancel;
        private ComboBox comboSingingSynthSystem;
        private Label lblSingingSynthSystem;
        private Label lblSinger;
        private ComboBox comboSinger;
        private CheckBox chkIgnoreExistingWavs;
        private TextBox txtDir;
        private Button btnBrowse;
        private FolderBrowserDialog folderBrowser;
        private System.ComponentModel.BackgroundWorker bgWork;
        private Label lblDir;

        public Program() {
            InitializeComponent();
            m_singer_config1 = VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID1 );
            m_singer_config2 = VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID2 );
            m_singer_config_utau = AppManager.editorConfig.UtauSingers.toArray( new SingerConfig[] { } );
            if ( m_singer_config1.Length > 0 ) {
                comboSingingSynthSystem.Items.Add( "VOCALOID1" );
            }
            if ( m_singer_config2.Length > 0 ) {
                comboSingingSynthSystem.Items.Add( "VOCALOID2" );
            }
            if ( m_singer_config_utau.Length > 0 ) {
                comboSingingSynthSystem.Items.Add( "UTAU" );
            }
            if ( comboSingingSynthSystem.Items.Count > 0 ) {
                comboSingingSynthSystem.SelectedIndex = 0;
            }
            updateSinger();
            txtDir.Text = PortUtil.combinePath( Application.StartupPath, "cache" );
        }

        private void updateSinger() {
            if ( comboSingingSynthSystem.SelectedIndex < 0 ) {
                return;
            }
            String singer = (String)comboSingingSynthSystem.SelectedItem;
            SingerConfig[] list = null;
            if ( singer.Equals( "VOCALOID1" ) ) {
                list = m_singer_config1;
            } else if ( singer.Equals( "VOCALOID2" ) ) {
                list = m_singer_config2;
            } else if ( singer.Equals( "UTAU" ) ) {
                list = m_singer_config_utau;
            }
            comboSinger.Items.Clear();
            if ( list == null ) {
                return;
            }
            for ( int i = 0; i < list.Length; i++ ) {
                comboSinger.Items.Add( list[i].VOICENAME );
            }
            if ( comboSinger.Items.Count > 0 ) {
                comboSinger.SelectedIndex = 0;
            }
        }

        [STAThread]
        static void Main( string[] args ) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            AppManager.init();
#if DEBUG
            org.kbinani.debug.push_log( "resampler=" + AppManager.editorConfig.PathResampler );
            org.kbinani.debug.push_log( "wavtool=" + AppManager.editorConfig.PathWavtool );
#endif
            String singer = "Miku";
            object locker = new object();
            double amp = 1.0;
            String dir = PortUtil.combinePath( Application.StartupPath, "cache" );
            bool replace = true;
            int search = -1;
            int arguments = 0;
            while ( search + 1 < args.Length ) {
                search++;
                switch ( args[search].ToLower() ) {
                    case "-help":
                    case "-h":
                    case "/help":
                    case "/h":
                    case "-?":
                    case "/?":
                    case "--h":
                    case "--help":
                        arguments++;
                        ShowHelp();
                        return;
                    case "-amplify":
                    case "-a":
                    case "/amplify":
                    case "/a":
                        if ( search + 1 < args.Length ) {
                            double t_amp = amp;
                            if ( double.TryParse( args[search + 1], out t_amp ) ) {
                                if ( t_amp < 0.0 ) {
                                    Console.WriteLine( "error; amilify coefficient must be >= 0. specified value was \"" + t_amp + "\"" );
                                    return;
                                }
                                amp = t_amp;
                            } else {
                                InvalidNumberExpressionAt( args[search + 1] );
                                return;
                            }
                        } else {
                            TooFewArgumentFor( args[search] );
                            return;
                        }
                        arguments++;
                        search++;
                        break;
                    case "-singer":
                    case "-s":
                    case "/singer":
                    case "/s":
                        if ( search + 1 < args.Length ) {
                            singer = args[search + 1];
                        } else {
                            TooFewArgumentFor( args[search] );
                            return;
                        }
                        arguments++;
                        search++;
                        break;
                    case "-dir":
                    case "-d":
                    case "/dir":
                    case "/d":
                        if ( search + 1 < args.Length ) {
                            dir = args[search + 1];
                        } else {
                            TooFewArgumentFor( args[search] );
                            return;
                        }
                        arguments++;
                        search++;
                        break;
                    case "-replace":
                    case "-r":
                    case "/replace":
                    case "/r":
                        replace = true;
                        arguments++;
                        break;
                    default:
                        Console.WriteLine( "error; unknown option \"" + args[search] + "\"" );
                        return;
                }
            }
            if ( arguments == 0 ) {
                Application.Run( new Program() );
            } else {
                PrepareStartArgument arg = new PrepareStartArgument();
                arg.singer = singer;
                arg.amplitude = amp;
                arg.directory = dir;
                arg.replace = replace;
                run( arg );
            }
        }

        static void InvalidNumberExpressionAt( string expression ) {
            Console.WriteLine( "error; string parse error. invalid number expression at \"" + expression + "\"" );
        }

        static void TooFewArgumentFor( string argument ) {
            Console.WriteLine( "error; too few argument for \"" + argument + "\"" );
        }

        static void ShowHelp() {
            Console.WriteLine( "GenerateKeySound, Copyright (C) 2008-2009, kbinani" );
            Console.WriteLine( "Usage: GenerateKeySound [options]" );
            Console.WriteLine( "    -help            Shows this message and return (short: -h, -?)" );
            Console.WriteLine( "    -amplify AMP     Sets sound amplify coefficients (short: -a)" );
            Console.WriteLine( "                     AMP must be 0 <= AMP (defualt is 1.0)" );
            //Console.WriteLine( "    -pchange NUMBER  Sets the value of Program Change (short: -p)" );
            Console.WriteLine( "    -dir DIRECTORY   Specifies the directory of output (short: -d)" );
            Console.WriteLine( "                     default of DIRECTORY is \"." + System.IO.Path.DirectorySeparatorChar + "cache\"" );
            Console.WriteLine( "    -replace         Switch to overwrite exisiting WAVs (short: -r)" );
            Console.WriteLine( "    -singer          Specifies singer (short: -s)" );
            Console.WriteLine();
            Console.WriteLine( "Options can be of the form -option or /option" );
        }

        public static unsafe void GenerateSinglePhone( int note, string singer, string file, double amp ) {
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

            for ( Iterator itr = AppManager.editorConfig.UtauSingers.iterator(); itr.hasNext(); ) {
                SingerConfig sc = (SingerConfig)itr.next();
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
            item.ID.Length = 480;
            item.ID.Note = note;
            item.ID.VibratoHandle = null;
            item.ID.type = VsqIDType.Anote;
            vsq.Track.get( 1 ).addEvent( item );
            vsq.updateTotalClocks();
            int ms_presend = 500;
            using ( WaveWriter ww = new WaveWriter( file ) ) {
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
                                  AppManager.getTempWaveDir(),
                                  false );
            }
        }

        private void InitializeComponent() {
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.comboSingingSynthSystem = new System.Windows.Forms.ComboBox();
            this.lblSingingSynthSystem = new System.Windows.Forms.Label();
            this.lblSinger = new System.Windows.Forms.Label();
            this.comboSinger = new System.Windows.Forms.ComboBox();
            this.chkIgnoreExistingWavs = new System.Windows.Forms.CheckBox();
            this.txtDir = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblDir = new System.Windows.Forms.Label();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.bgWork = new System.ComponentModel.BackgroundWorker();
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

        private void comboSingingSynthSystem_SelectedIndexChanged( object sender, EventArgs e ) {
            updateSinger();
        }

        private void btnBrowse_Click( object sender, EventArgs e ) {
            folderBrowser.SelectedPath = txtDir.Text;
            if ( folderBrowser.ShowDialog() != DialogResult.OK ) {
                return;
            }
            txtDir.Text = folderBrowser.SelectedPath;
        }

        private void btnCancel_Click( object sender, EventArgs e ) {
            if ( bgWork.IsBusy ) {
                m_cancel_required = true;
                while ( m_cancel_required ) {
                    Application.DoEvents();
                }
            } else {
                this.Close();
            }
        }

        private void btnExecute_Click( object sender, EventArgs e ) {
            PrepareStartArgument arg = new PrepareStartArgument();
            arg.singer = (String)comboSinger.SelectedItem;
            arg.amplitude = 1.0;
            arg.directory = txtDir.Text;
            arg.replace = chkIgnoreExistingWavs.Checked;
            updateEnabled( false );
            bgWork.RunWorkerAsync( arg );
        }

        class PrepareStartArgument {
            public String singer = "Miku";
            public double amplitude = 1.0;
            public String directory = "";
            public bool replace = true;
        }

        private void bgWork_DoWork( object sender, DoWorkEventArgs e ) {
            PrepareStartArgument arg = (PrepareStartArgument)e.Argument;
            String singer = arg.singer;
            double amp = arg.amplitude;
            String dir = arg.directory;
            bool replace = arg.replace;
            // 音源を準備
            if ( !PortUtil.isDirectoryExists( dir ) ) {
                System.IO.Directory.CreateDirectory( dir );
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
                            } catch {
                            }
                        }
                    } catch {
                    }
                }
                Console.WriteLine( " done" );
                if ( m_cancel_required ) {
                    m_cancel_required = false;
                    break;
                }
                bgWork.ReportProgress( (int)(i / 127.0 * 100.0) );
            }
            m_cancel_required = false;
        }

        private static void run( PrepareStartArgument arg ) {
            String singer = arg.singer;
            double amp = arg.amplitude;
            String dir = arg.directory;
            bool replace = arg.replace;
            // 音源を準備
            if ( !PortUtil.isDirectoryExists( dir ) ) {
                System.IO.Directory.CreateDirectory( dir );
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
                            } catch {
                            }
                        }
                    } catch {
                    }
                }
                Console.WriteLine( " done" );
            }
        }

        private void bgWork_ProgressChanged( object sender, System.ComponentModel.ProgressChangedEventArgs e ) {
            this.Invoke( new updateTitleDelegate( this.updateTitle ), new object[] { "Progress: " + e.ProgressPercentage + "%" } );
        }

        private delegate void updateTitleDelegate( String title );

        private void updateTitle( String title ) {
            this.Text = title;
        }

        private void Program_FormClosed( object sender, FormClosedEventArgs e ) {
            VSTiProxy.terminate();
        }

        private void bgWork_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e ) {
            updateEnabled( true );
        }

        private void updateEnabled( bool enabled ) {
            comboSinger.Enabled = enabled;
            comboSingingSynthSystem.Enabled = enabled;
            txtDir.ReadOnly = !enabled;
            btnBrowse.Enabled = enabled;
            btnExecute.Enabled = enabled;
            chkIgnoreExistingWavs.Enabled = enabled;
            if ( enabled ) {
                btnCancel.Text = "Close";
            } else {
                btnCancel.Text = "Cancel";
            }
        }
    }

}
