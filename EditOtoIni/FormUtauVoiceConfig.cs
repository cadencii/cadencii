/*
 * FormUtauVoiceConfig.cs
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
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

using Boare.Lib.AppUtil;
using Boare.Lib.Media;
using bocoree;

namespace Boare.EditOtoIni {

    using boolean = System.Boolean;

    public partial class FormUtauVoiceConfig : Form {
        enum MouseMode {
            None,
            MiddleDrag,
            MoveOffset,
            MoveConsonant,
            MoveBlank,
            MovePreUtterance,
            MoveOverlap,
        }

        enum FlagType : int {
            Offset = 0,
            Consonant = 1,
            Blank = 2,
            PreUtterance = 3,
            Overlap = 4,
        }

        const int LINE_HEIGHT = 20;
        const int TRACKBAR_MAX = 35;
        const int TRACKBAR_MIN = 15;
        const int ORDER = 10;

        private Vector<Boare.Cadencii.WaveDrawContext> m_drawer = new Vector<Boare.Cadencii.WaveDrawContext>();
        private int m_index = -1;
        private double m_px_per_sec = 1000;
        private float m_start_to_draw = 0.0f;
        private SolidBrush m_brs_offset = new SolidBrush( Color.FromArgb( 192, 192, 255 ) );
        private SolidBrush m_brs_consonant = new SolidBrush( Color.FromArgb( 255, 192, 255 ) );
        private SolidBrush m_brs_blank = new SolidBrush( Color.FromArgb( 192, 192, 255 ) );
        private Pen m_pen_overlap = new Pen( Color.FromArgb( 0, 255, 0 ) );
        private Pen m_pen_preutterance = new Pen( Color.FromArgb( 255, 0, 0 ) );
        private float m_offset;
        private float m_consonant;
        private float m_blank;
        private float m_pre_utterance;
        private float m_overlap;
        private String m_file = "";
        private Rectangle[] m_flag_box = new Rectangle[5];
        private float m_length;
        private int m_font_draw_offset = 0;
        private String m_font_name = "";
        private float m_font_size = -1;
        private MouseMode m_mode = MouseMode.None;
        private float m_move_init;
        /// <summary>
        /// MouseDownイベントが起きた座標
        /// </summary>
        private Point m_mouse_downed;
        /// <summary>
        /// mouseDownイベントが起きた時点での、m_start_to_drawの値
        /// </summary>
        private float m_mouse_downed_start_to_draw;
        private MediaPlayer m_player;
        private String m_oto_ini = "";
        private int m_last_preview = 0;
        private boolean m_edited = false;
        private int m_trackbar_value = 20;
        private Thread m_mouse_hover_generator = null;
        private boolean m_cancel_required = false;
        private Rectangle m_current_bounds;

        public FormUtauVoiceConfig() {
            InitializeComponent();
            pictWave.MouseWheel += new MouseEventHandler( pictWave_MouseWheel );
            splitContainerIn.Panel1.BorderStyle = BorderStyle.None;
            splitContainerIn.Panel2.BorderStyle = BorderStyle.FixedSingle;
            splitContainerIn.Panel2.BorderColor = SystemColors.ControlDark;

            splitContainerOut.Panel1.BorderStyle = BorderStyle.None;
            splitContainerOut.Panel2.BorderStyle = BorderStyle.FixedSingle;
            splitContainerOut.Panel2.BorderColor = SystemColors.ControlDark;

            splitContainerIn.Panel1.Controls.Add( panelLeft );
            panelLeft.Dock = DockStyle.Fill;
            splitContainerIn.Panel2.Controls.Add( panelRight );
            panelRight.Dock = DockStyle.Fill;

            splitContainerOut.Panel1.Controls.Add( splitContainerIn );
            splitContainerIn.Dock = DockStyle.Fill;
            splitContainerOut.Panel2.Controls.Add( panelBottom );
            panelBottom.Dock = DockStyle.Fill;

            splitContainerOut.Dock = DockStyle.Fill;

            UpdateScale();
            m_player = new MediaPlayer();
            ApplyLanguage();
        }

        [STAThread]
        public static void Main( string[] args ) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new FormUtauVoiceConfig() );
        }

        private void refreshScreen() {
            if ( !bgWorkScreen.IsBusy ) {
                bgWorkScreen.RunWorkerAsync();
            }
        }

        /// <summary>
        /// アプリケーションデータの保存位置を取得します
        /// Gets the path for application data
        /// </summary>
        public static String getApplicationDataPath() {
            String dir = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Boare" );
            if ( !Directory.Exists( dir ) ) {
                Directory.CreateDirectory( dir );
            }
            String dir2 = Path.Combine( dir, "EditOtoIni" );
            if ( !Directory.Exists( dir2 ) ) {
                Directory.CreateDirectory( dir2 );
            }
            return dir2;
        }

        public FormConfigUtauVoiceConfig CurrentConfig {
            get {
                FormConfigUtauVoiceConfig ret = new FormConfigUtauVoiceConfig();
                ret.Bounds = m_current_bounds;
                ret.State = this.WindowState;
                ret.InnerSplitterDistancePercentage = splitContainerIn.SplitterDistance / (float)splitContainerIn.Width * 100.0f;
                ret.OuterSplitterDistancePercentage = splitContainerOut.SplitterDistance / (float)splitContainerOut.Height * 100.0f;
                ret.WaveViewScale = m_trackbar_value;
                ret.ColumnWidthAlias = columnHeaderAlias.Width;
                ret.ColumnWidthBlank = columnHeaderBlank.Width;
                ret.ColumnWidthConsonant = columnHeaderConsonant.Width;
                ret.ColumnWidthFileName = columnHeaderFilename.Width;
                ret.ColumnWidthFrq = columnHeaderFrq.Width;
                ret.ColumnWidthOffset = columnHeaderOffset.Width;
                ret.ColumnWidthOverlap = columnHeaderOverlap.Width;
                ret.ColumnWidthPreUtterance = columnHeaderPreUtterance.Width;
                ret.ColumnWidthStf = columnHeaderStf.Width;
                return ret;
            }
            set {
#if DEBUG
                Console.WriteLine( "FormUtauVoiceConfig#set_CurrentConfig" );
#endif
                if ( value.State != FormWindowState.Maximized ) {
                    this.Bounds = value.Bounds;
                } else {
                    this.WindowState = FormWindowState.Maximized;
                }
                splitContainerIn.SplitterDistance = (int)(splitContainerIn.Width * value.InnerSplitterDistancePercentage / 100.0f);
                splitContainerOut.SplitterDistance = (int)(splitContainerOut.Height * value.OuterSplitterDistancePercentage / 100.0f);
                if ( value.WaveViewScale < TRACKBAR_MIN ) {
                    m_trackbar_value = TRACKBAR_MIN;
                } else if ( TRACKBAR_MAX < value.WaveViewScale ) {
                    m_trackbar_value = TRACKBAR_MAX;
                } else {
                    m_trackbar_value = value.WaveViewScale;
                }
                columnHeaderAlias.Width = value.ColumnWidthAlias;
                columnHeaderBlank.Width = value.ColumnWidthBlank;
                columnHeaderConsonant.Width = value.ColumnWidthConsonant;
                columnHeaderFilename.Width = value.ColumnWidthFileName;
                columnHeaderFrq.Width = value.ColumnWidthFrq;
                columnHeaderOffset.Width = value.ColumnWidthOffset;
                columnHeaderOverlap.Width = value.ColumnWidthOverlap;
                columnHeaderPreUtterance.Width = value.ColumnWidthPreUtterance;
                columnHeaderStf.Width = value.ColumnWidthStf;
                UpdateScale();
            }
        }

        public void ApplyFont( Font font ) {
            this.Font = font;
            foreach ( Control c in this.Controls ) {
                Misc.ApplyFontRecurse( c, font );
            }
            Misc.ApplyToolStripFontRecurse( menuFile, font );
        }

        private void HoverWaitThread(){
            Thread.Sleep( SystemInformation.MouseHoverTime );
            EventHandler eh = new EventHandler( pictWave_MouseHover );
            this.Invoke( eh, pictWave, new EventArgs() );
        }

        private void pictWave_MouseHover( object sender, EventArgs e ) {
        }

        public void ApplyLanguage() {
            UpdateFormTitle();
            
            menuFile.Text = _( "File" ) + "(&F)";
            menuFileOpen.Text = _( "Open" ) + "(&O)";
            menuFileSave.Text = _( "Save" ) + "(&S)";
            menuFileSaveAs.Text = _( "Save As" ) + "(&A)";
            menuFileQuit.Text = _( "Quit" ) + "(&Q)";
            menuEdit.Text = _( "Edit" ) + "(&E)";
            menuEditGenerateFRQ.Text = _( "Generate FRQ files" );
            menuEditGenerateSTF.Text = _( "Generate STF files" );
            menuView.Text = _( "View" ) + "(&V)";
            menuViewSearchNext.Text = _( "Search Next" ) + "(&N)";
            menuViewSearchPrevious.Text = _( "Search Previous" ) + "(&N)";

            lblFileName.Text = _( "File Name" );
            lblAlias.Text = _( "Alias" );
            lblOffset.Text = _( "Offset" );
            lblConsonant.Text = _( "Consonant" );
            lblBlank.Text = _( "Blank" );
            lblPreUtterance.Text = _( "Pre Utterance" );
            lblOverlap.Text = _( "Overlap" );

            columnHeaderFilename.Text = _( "File Name" );
            columnHeaderAlias.Text = _( "Alias" );
            columnHeaderOffset.Text = _( "Offset" );
            columnHeaderConsonant.Text = _( "Consonant" );
            columnHeaderBlank.Text = _( "Blank" );
            columnHeaderPreUtterance.Text = _( "Pre Utterance" );
            columnHeaderOverlap.Text = _( "Overlap" );

            try {
                openFileDialog.Filter = _( "Voice DB Config(*.ini)|*.ini" ) + "|" + _( "All Files(*.*)|*.*" );
                saveFileDialog.Filter = _( "Voice DB Config(*.ini)|*.ini" ) + "|" + _( "All Files(*.*)|*.*" );
            } catch {
                openFileDialog.Filter = "Voice DB Config(*.ini)|*.ini|All Files(*.*)|*.*";
                saveFileDialog.Filter = "Voice DB Config(*.ini)|*.ini|All Files(*.*)|*.*";
            }

            btnRefreshStf.Text = _( "Refresh STF" );
            btnRefreshFrq.Text = _( "Refresh FRQ" );

            lblSearch.Text = _( "Search" ) + ":";
            buttonNext.Text = _( "Next" );
            buttonPrevious.Text = _( "Previous" );
            const int CLEALANCE = 6;
            txtSearch.Left = lblSearch.Left + lblSearch.Width + CLEALANCE;
            buttonNext.Left = txtSearch.Left + txtSearch.Width + CLEALANCE;
            buttonPrevious.Left = buttonNext.Left + buttonNext.Width + CLEALANCE;
        }

        private void UpdateFormTitle() {
            String f = m_oto_ini;
            if ( f.Equals( "" ) ){
                f = "Untitled";
            }
            String title = _( "Voice DB Config" ) + " - " + f + (m_edited ? " *" : "");
            if ( title != this.Text ) {
                this.Text = title;
            }
        }

        private static String _( String id ) {
            return Messaging.GetMessage( id );
        }

        private void pictWave_MouseWheel( object sender, MouseEventArgs e ) {
            int draft = hScroll.Value - e.Delta / 120 * hScroll.LargeChange / 2;
            if ( draft > hScroll.Maximum - hScroll.LargeChange + 1 ) {
                draft = hScroll.Maximum - hScroll.LargeChange + 1;
            }
            if ( draft < hScroll.Minimum ) {
                draft = hScroll.Minimum;
            }
            if ( draft != hScroll.Value ) {
                hScroll.Value = draft;
            }
        }

        public void Open( String oto_ini_path ) {
            m_oto_ini = oto_ini_path;
            UpdateFormTitle();
            m_cancel_required = false;
            listFiles.Items.Clear();
            bgWorkRead.RunWorkerAsync( oto_ini_path );
        }

        private void AddItem( Boare.Cadencii.ValuePair<boolean, String[]> arg ) {
            String[] columns = arg.Value;
            boolean exists = arg.Key;
            ListViewItem item = new ListViewItem( columns );
            item.Tag = exists;
            listFiles.Items.Add( item );
        }

        private void bgWorkRead_DoWork( object sender, DoWorkEventArgs e ) {
            String oto_ini_path = (String)e.Argument;
            int c = m_drawer.size();
            for ( int i = 0; i < c; i++ ) {
                m_drawer.get( i ).Dispose();
            }
            m_drawer.clear();

            if ( !File.Exists( oto_ini_path ) ) {
                return;
            }

            String dir = Path.GetDirectoryName( oto_ini_path );
            using ( StreamReader sr = new StreamReader( oto_ini_path, Encoding.GetEncoding( "Shift_JIS" ) ) ) {
                while ( sr.Peek() >= 0 ) {
                    if ( m_cancel_required ) {
                        break;
                    }
                    String line = sr.ReadLine();
                    int eq = line.IndexOf( '=' );
                    if ( eq <= 0 ) {
                        continue;
                    }
                    String f = line.Substring( 0, eq );
                    line = line.Substring( eq + 1 );
                    String[] spl = line.Split( ',' );
                    String file = Path.Combine( dir, f );
                    Boare.Cadencii.WaveDrawContext wdc = new Boare.Cadencii.WaveDrawContext( file );
                    wdc.Name = f + spl[0];
                    String wave_name = Path.GetFileNameWithoutExtension( f );
                    String ext = Path.GetExtension( file ).Replace( ".", "" );
                    String f2 = wave_name + "_" + ext + ".frq"; // f.Replace( ".wav", "_wav.frq" );
                    String freq = Path.Combine( dir, f2 );
                    boolean freq_exists = File.Exists( freq );
                    if ( freq_exists ) {
                        wdc.Freq = Boare.Cadencii.UtauFreq.FromFrq( freq );
                    }
                    m_drawer.add( wdc );
                    Vector<String> columns = new Vector<String>( spl );
                    columns.insertElementAt( f, 0 );
                    columns.add( freq_exists ? "○" : "" );
                    String stf = Path.Combine( Path.Combine( dir, "analyzed" ), wave_name + ".stf" );
                    boolean stf_exists = File.Exists( stf );
                    columns.add( stf_exists ? "○" : "" );
                    Boare.Cadencii.BSimpleDelegate<Boare.Cadencii.ValuePair<boolean, String[]>> deleg =
                        new Boare.Cadencii.BSimpleDelegate<Boare.Cadencii.ValuePair<boolean, String[]>>( AddItem );
                    this.Invoke( deleg,
                                 new Boare.Cadencii.ValuePair<boolean, String[]>( File.Exists( file ),
                                                                                  columns.toArray( new String[] { } ) ) );
                }
            }
            m_edited = false;
        }

        private void listFiles_SelectedIndexChanged( object sender, EventArgs e ) {
            if ( listFiles.SelectedIndices.Count <= 0 ){
                return;
            }
            int index = listFiles.SelectedIndices[0];
            ListViewItem selected_item = listFiles.Items[index];
            String name = selected_item.Text + selected_item.SubItems[1].Text;
            boolean enabled = true;
            if ( selected_item.Tag != null && selected_item.Tag is boolean ) {
                enabled = (boolean)selected_item.Tag;
            }
            if ( !enabled ) {
                listFiles.SelectedIndices.Clear();
                return;
            }
            int c = m_drawer.size();
            m_index = -1;
            for ( int i = 0; i < c; i++ ) {
                if ( name.Equals( m_drawer.get( i ).Name ) ) {
                    m_index = i;
                    m_length = m_drawer.get( i ).Length;
                    int c2 = selected_item.SubItems.Count;
                    String[] spl = new String[c2];
                    for ( int i2 = 0; i2 < c2; i2++ ) {
                        spl[i2] = selected_item.SubItems[i2].Text;
                    }
                    boolean old = Edited;
                    txtFileName.Text = spl[0];
                    m_file = spl[0];
                    txtAlias.Text = spl[1];
                    float o;

                    if ( float.TryParse( spl[2], out o ) ) {
                        m_offset = round2Digits( o );
                        txtOffset.Text = m_offset.ToString();
                    }
                    
                    if ( float.TryParse( spl[3], out o ) ) {
                        m_consonant = round2Digits( o );
                        txtConsonant.Text = m_consonant.ToString();
                    }

                    if ( float.TryParse( spl[4], out o ) ) {
                        m_blank = round2Digits( o );
                        txtBlank.Text = m_blank.ToString();
                    }

                    if ( float.TryParse( spl[5], out o ) ) {
                        m_pre_utterance = round2Digits( o );
                        txtPreUtterance.Text = m_pre_utterance.ToString();
                    }

                    if ( float.TryParse( spl[6], out o ) ) {
                        m_overlap = round2Digits( o );
                        txtOverlap.Text = m_overlap.ToString();
                    }
                    Edited = old;
                    float minimum = Math.Min( m_overlap, Math.Min( m_offset, m_pre_utterance ) );
                    minimum = Math.Min( minimum, 0.0f );
#if DEBUG
                    Console.WriteLine( "FormUtauVoiceConfig#listFiles_SelectedIndexChanged; minimum=" + minimum );
                    Console.WriteLine( "FormUtauVoiceConfig#listFiles_SelectedIndexChanged; m_drawer.get( i ).Length=" + m_drawer.get( i ).Length );
                    Console.WriteLine( "FormUtauVoiceConfig#listFiles_SelectedIndexChanged; A; hScroll.Minimum=" + hScroll.Minimum + "; hScroll.Maximum=" + hScroll.Maximum );
#endif
                    hScroll.Minimum = (int)(minimum * ORDER);
                    hScroll.Maximum = (int)(m_drawer.get( i ).Length * 1000 * ORDER);
                    if ( m_start_to_draw < hScroll.Minimum / 1000.0f / ORDER ) {
                        m_start_to_draw = hScroll.Minimum / 1000.0f / ORDER;
                    }
#if DEBUG
                    Console.WriteLine( "FormUtauVoiceConfig#listFiles_SelectedIndexChanged; B; hScroll.Minimum=" + hScroll.Minimum + "; hScroll.Maximum=" + hScroll.Maximum );
#endif
                    break;
                }
            }
            refreshScreen();
        }

        /// <summary>
        /// pがrcの中にあるかどうかを判定します
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rc"></param>
        /// <returns></returns>
        public static boolean isInRect( Point p, Rectangle rc ) {
            if ( rc.X <= p.X ) {
                if ( p.X <= rc.X + rc.Width ) {
                    if ( rc.Y <= p.Y ) {
                        if ( p.Y <= rc.Y + rc.Height ) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void pictWave_Paint( object sender, PaintEventArgs e ) {
            if ( 0 <= m_index && m_index < m_drawer.size() ) {
                /*int c = listFiles.Items[m_index].SubItems.Count;
                String[] spl = new String[c];
                for ( int i = 0; i < c; i++ ) {
                    spl[i] = listFiles.Items[m_index].SubItems[i].Text;
                }*/
                e.Graphics.Clear( Color.White );
                int h = pictWave.Height;

                int x_offset = XFromSec( 0.0f );
                Rectangle rc = new Rectangle( x_offset, 0, (int)(m_offset / 1000.0f * m_px_per_sec), h );
                e.Graphics.FillRectangle( m_brs_offset, rc );

                int x_consonant = XFromSec( m_offset / 1000.0f );
                rc = new Rectangle( x_consonant, 0, (int)(m_consonant / 1000.0f * m_px_per_sec), h );
                e.Graphics.FillRectangle( m_brs_consonant, rc );

                int x_blank = XFromSec( m_drawer.get( m_index ).Length - m_blank / 1000.0f );
                rc = new Rectangle( x_blank, 0, (int)(m_blank / 1000.0f * m_px_per_sec), h );
                e.Graphics.FillRectangle( m_brs_blank, rc );

                m_drawer.get( m_index ).Draw( e.Graphics,
                                        Pens.Black,
                                        new Rectangle( 0, 0, pictWave.Width, h ),
                                        m_start_to_draw,
                                        (float)(m_start_to_draw + pictWave.Width / m_px_per_sec) );

                int x_overlap = XFromSec( m_overlap / 1000.0f );
                e.Graphics.DrawLine( m_pen_overlap, new Point( x_overlap, 0 ), new Point( x_overlap, h ) );

                int x_pre_utterance = XFromSec( m_pre_utterance / 1000.0f );
                e.Graphics.DrawLine( m_pen_preutterance, new Point( x_pre_utterance, 0 ), new Point( x_pre_utterance, h ) );

                Font font = AppManager.cadenciiConfig.BaseFont;

                int x_lastpreview = XFromSec( m_last_preview / 1000.0f );
                e.Graphics.DrawLine( Pens.Blue, new Point( x_lastpreview, 0 ), new Point( x_lastpreview, h ) );
                e.Graphics.DrawString( m_last_preview + " ms", font, Brushes.Black, new PointF( x_lastpreview + 1, 1 ) );

                e.Graphics.DrawString( m_file, font, Brushes.Black, new PointF( 1, LINE_HEIGHT ) );

                if ( font.Name != m_font_name || font.SizeInPoints != m_font_size ) {
                    m_font_draw_offset = Misc.GetStringDrawOffset( font ) - 1;
                    m_font_name = font.Name;
                    m_font_size = font.SizeInPoints;
                }

                using ( SolidBrush brs = new SolidBrush( Color.FromArgb( 180, Color.White ) ) ) {
                    rc = GetFlagRect( FlagType.Offset, font );
                    m_flag_box[0] = rc;
                    e.Graphics.FillRectangle( brs, rc );
                    e.Graphics.DrawRectangle( Pens.Black, rc );
                    e.Graphics.DrawString( _( "Offset" ) + ": " + m_offset + " ms",
                                           font, 
                                           Brushes.Black, 
                                           rc.X + 1,
                                           rc.Y + rc.Height * 0.5f - m_font_draw_offset );

                    rc = GetFlagRect( FlagType.Consonant, font );
                    m_flag_box[1] = rc;
                    e.Graphics.FillRectangle( brs, rc );
                    e.Graphics.DrawRectangle( Pens.Black, rc );
                    e.Graphics.DrawString( _( "Consonant" ) + ": " + m_consonant + " ms", 
                                           font,
                                           Brushes.Black,
                                           rc.X + 1,
                                           rc.Y + rc.Height * 0.5f - m_font_draw_offset );

                    rc = GetFlagRect( FlagType.Blank, font );
                    m_flag_box[2] = rc;
                    e.Graphics.FillRectangle( brs, rc );
                    e.Graphics.DrawRectangle( Pens.Black, rc );
                    e.Graphics.DrawString( _( "Blank" ) + ": " + m_blank + " ms",
                                           font,
                                           Brushes.Black,
                                           rc.X + 1,
                                           rc.Y + rc.Height * 0.5f - m_font_draw_offset );

                    rc = GetFlagRect( FlagType.PreUtterance, font );
                    m_flag_box[3] = rc;
                    e.Graphics.FillRectangle( brs, rc );
                    e.Graphics.DrawRectangle( Pens.Black, rc );
                    e.Graphics.DrawString( _( "Pre Utterance" ) + ": " + m_pre_utterance + " ms",
                                           font, 
                                           Brushes.Black, 
                                           rc.X + 1,
                                           rc.Y + rc.Height * 0.5f - m_font_draw_offset );

                    rc = GetFlagRect( FlagType.Overlap, font );
                    m_flag_box[4] = rc;
                    e.Graphics.FillRectangle( brs, rc );
                    e.Graphics.DrawRectangle( Pens.Black, rc );
                    e.Graphics.DrawString( _( "Overlap" ) + ": " + m_overlap + " ms",
                                           font, 
                                           Brushes.Black, 
                                           rc.X + 1,
                                           rc.Y + rc.Height * 0.5f - m_font_draw_offset );
                }
            }
        }

        private Rectangle GetFlagRect( FlagType type, Font font ) {
            int i = (int)type;
            int x = 0;
            String s = "";
            switch ( type ) {
                case FlagType.Offset:
                    x = XFromSec( m_offset / 1000.0f );
                    s = _( "Offset" ) + ": " + m_offset + " ms";
                    break;
                case FlagType.Consonant:
                    x = XFromSec( (m_offset + m_consonant) / 1000.0f );
                    s = _( "Consonant" ) + ": " + m_consonant + " ms";
                    break;
                case FlagType.Blank:
                    x = XFromSec( m_length - m_blank / 1000.0f );
                    s = _( "Blank" ) + ": " + m_blank + " ms";
                    break;
                case FlagType.PreUtterance:
                    x = XFromSec( m_pre_utterance / 1000.0f );
                    s = _( "Pre Utterance" ) + ": " + m_pre_utterance + " ms";
                    break;
                case FlagType.Overlap:
                    x = XFromSec( m_overlap / 1000.0f );
                    s = _( "Overlap" ) + ": " + m_overlap + " ms";
                    break;
            }
            SizeF size = Misc.MeasureString( s, font );
            return new Rectangle( x, LINE_HEIGHT * (i + 2), (int)(size.Width * 1.1f), (int)(size.Height * 1.1f) );
        }

        private int XFromSec( float sec ) {
            return (int)((sec - m_start_to_draw) * m_px_per_sec);
        }

        private float SecFromX( int x ) {
            return x / (float)m_px_per_sec + m_start_to_draw;
        }

        private static float round2Digits( float value ) {
            return (float)Math.Round( value, 2 );
        }

        private void UpdateScale() {
            m_px_per_sec = 10.0 * Math.Pow( 10.0, m_trackbar_value / 10.0 );
            hScroll.LargeChange = (int)(pictWave.Width / m_px_per_sec * 1000 * ORDER);
        }

        private void hScroll_ValueChanged( object sender, EventArgs e ) {
            m_start_to_draw = hScroll.Value / 1000.0f / ORDER;
            refreshScreen();
        }

        private void pictWave_MouseDown( object sender, MouseEventArgs e ) {
            m_mouse_downed = e.Location;
            m_mouse_downed_start_to_draw = m_start_to_draw;
            if ( m_mouse_hover_generator != null && m_mouse_hover_generator.IsAlive ) {
                m_mouse_hover_generator.Abort();
                while ( m_mouse_hover_generator.IsAlive ) {
                    Application.DoEvents();
                }
            }
            m_mouse_hover_generator = new Thread( new ThreadStart( this.HoverWaitThread ) );
            m_mouse_hover_generator.Start();
            if ( e.Button == MouseButtons.Middle ) {
                m_mode = MouseMode.MiddleDrag;
            } else if ( e.Button == MouseButtons.Left ) {
                m_mode = MouseMode.None;
                for ( int i = 0; i < m_flag_box.Length; i++ ) {
                    Rectangle rc = m_flag_box[i];
                    if ( isInRect( e.Location, rc ) ) {
                        switch ( i ) {
                            case 0:
                                m_mode = MouseMode.MoveOffset;
                                m_move_init = m_offset;
                                return;
                            case 1:
                                m_mode = MouseMode.MoveConsonant;
                                m_move_init = m_consonant;
                                return;
                            case 2:
                                m_mode = MouseMode.MoveBlank;
                                m_move_init = m_blank;
                                return;
                            case 3:
                                m_mode = MouseMode.MovePreUtterance;
                                m_move_init = m_pre_utterance;
                                return;
                            case 4:
                                m_mode = MouseMode.MoveOverlap;
                                m_move_init = m_overlap;
                                return;
                        }
                        break;
                    }
                }

                if ( 0 <= m_index && m_index < listFiles.Items.Count ){
                    String file = Path.Combine( Path.GetDirectoryName( m_oto_ini ), listFiles.Items[m_index].SubItems[0].Text );
                    if ( File.Exists( file ) && m_player.SoundLocation != file ) {
                        m_player.Close();
                        m_player.Load( file );
                    }
                    double sec = SecFromX( e.X );
                    m_last_preview = (int)(sec * 1000);
                    m_player.PlayFrom( m_last_preview / 1000.0 );
                    refreshScreen();
                }
            }
        }

        private void pictWave_MouseMove( object sender, MouseEventArgs e ) {
            if ( m_mouse_hover_generator != null && m_mouse_hover_generator.IsAlive ) {
                m_mouse_hover_generator.Abort();
            }
            boolean check_hscroll_minimum = false;
            float minimum = 0;
            if ( m_mode == MouseMode.MiddleDrag ) {
                int dx = e.X - m_mouse_downed.X;
                int draft = (int)((m_mouse_downed_start_to_draw - dx / (float)m_px_per_sec) * 1000 * ORDER);
                if ( draft > hScroll.Maximum - hScroll.LargeChange + 1 ) {
                    draft = hScroll.Maximum - hScroll.LargeChange + 1;
                }
                if ( draft < hScroll.Minimum ) {
                    draft = hScroll.Minimum;
                }
                if ( hScroll.Value != draft ) {
                    hScroll.Value = draft;
                }
            } else if ( m_mode == MouseMode.MoveOffset ) {
                int dx = e.X - m_mouse_downed.X;
                float draft = round2Digits( m_move_init + (float)(dx / m_px_per_sec * 1000) );
                txtOffset.Text = draft.ToString();
                check_hscroll_minimum = true;
                minimum = Math.Min( draft, minimum );
            } else if ( m_mode == MouseMode.MoveConsonant ) {
                int dx = e.X - m_mouse_downed.X;
                float draft = round2Digits( m_move_init + (float)(dx / m_px_per_sec * 1000) );
                txtConsonant.Text = draft.ToString();
            } else if ( m_mode == MouseMode.MoveBlank ) {
                int dx = e.X - m_mouse_downed.X;
                float draft = round2Digits( m_move_init - (float)(dx / m_px_per_sec * 1000) );
                txtBlank.Text = draft.ToString();
            } else if ( m_mode == MouseMode.MovePreUtterance ) {
                int dx = e.X - m_mouse_downed.X;
                float draft = round2Digits( m_move_init + (float)(dx / m_px_per_sec * 1000) );
                txtPreUtterance.Text = draft.ToString();
                check_hscroll_minimum = true;
                minimum = Math.Min( draft, minimum );
            } else if ( m_mode == MouseMode.MoveOverlap ) {
                int dx = e.X - m_mouse_downed.X;
                float draft = round2Digits( m_move_init + (float)(dx / m_px_per_sec * 1000) );
                txtOverlap.Text = draft.ToString();
                check_hscroll_minimum = true;
                minimum = Math.Min( draft, minimum );
            }
            if ( check_hscroll_minimum ) {
                float draft_minimum = minimum * ORDER;
                if ( draft_minimum < hScroll.Minimum ) {
                    hScroll.Minimum = (int)draft_minimum;
                    if ( m_start_to_draw < draft_minimum / 1000.0f / ORDER ) {
                        m_start_to_draw = draft_minimum / 1000.0f / ORDER;
                    }
                    refreshScreen();
                }
            }
        }

        private boolean Edited {
            get {
                return m_edited;
            }
            set {
                boolean old = m_edited;
                m_edited = value;
                if ( m_edited != old ) {
                    UpdateFormTitle();
                }
            }
        }

        private void pictWave_MouseUp( object sender, MouseEventArgs e ) {
            m_mode = MouseMode.None;
        }

        private void txtAlias_TextChanged( object sender, EventArgs e ) {
            if ( 0 <= m_index && m_index < listFiles.Items.Count ) {
                listFiles.Items[m_index].SubItems[1].Text = txtAlias.Text;
                Edited = true;
                pictWave.Invalidate();
            }
        }

        private void txtOffset_TextChanged( object sender, EventArgs e ) {
            float i;
            if ( !float.TryParse( txtOffset.Text, out i ) ) {
                return;
            }
            m_offset = round2Digits( i );
            if ( 0 <= m_index && m_index < listFiles.Items.Count ) {
                listFiles.Items[m_index].SubItems[2].Text = txtOffset.Text;
                Edited = true;
                pictWave.Invalidate();
            }
        }

        private void txtConsonant_TextChanged( object sender, EventArgs e ) {
            float i;
            if ( !float.TryParse( txtConsonant.Text, out i ) ) {
                return;
            }
            m_consonant = round2Digits( i );
            if ( 0 <= m_index && m_index < listFiles.Items.Count ) {
                listFiles.Items[m_index].SubItems[3].Text = txtConsonant.Text;
                Edited = true;
                pictWave.Invalidate();
            }
        }

        private void txtBlank_TextChanged( object sender, EventArgs e ) {
            float i;
            if ( !float.TryParse( txtBlank.Text, out i ) ) {
                return;
            }
            m_blank = round2Digits( i );
            if ( 0 <= m_index && m_index < listFiles.Items.Count ) {
                listFiles.Items[m_index].SubItems[4].Text = txtBlank.Text;
                Edited = true;
                pictWave.Invalidate();
            }
        }

        private void txtPreUtterance_TextChanged( object sender, EventArgs e ) {
            float i;
            if ( !float.TryParse( txtPreUtterance.Text, out i ) ) {
                return;
            }
            m_pre_utterance = round2Digits( i );
            if ( 0 <= m_index && m_index < listFiles.Items.Count ) {
                listFiles.Items[m_index].SubItems[5].Text = txtPreUtterance.Text;
                Edited = true;
                pictWave.Invalidate();
            }
        }

        private void txtOverlap_TextChanged( object sender, EventArgs e ) {
            float i;
            if ( !float.TryParse( txtOverlap.Text, out i ) ) {
                return;
            }
            m_overlap = round2Digits( i );
            if ( 0 <= m_index && m_index < listFiles.Items.Count ) {
                listFiles.Items[m_index].SubItems[6].Text = txtOverlap.Text;
                Edited = true;
                pictWave.Invalidate();
            }
        }

        private void FormUtauVoiceConfig_FormClosed( object sender, FormClosedEventArgs e ) {
            m_cancel_required = true;
            while ( bgWorkRead.IsBusy ) {
                Application.DoEvents();
            }
            if ( m_player != null ) {
                m_player.Close();
            }
        }

        private void btnMinus_Click( object sender, EventArgs e ) {
            if ( TRACKBAR_MIN < m_trackbar_value ) {
                m_trackbar_value--;
                UpdateScale();
                refreshScreen();
            }
        }

        private void btnPlus_Click( object sender, EventArgs e ) {
            if ( m_trackbar_value < TRACKBAR_MAX ) {
                m_trackbar_value++;
                UpdateScale();
                refreshScreen();
            }
        }

        private void bgWorkRead_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e ) {
            Edited = false;
        }

        private void menuFileOpen_Click( object sender, EventArgs e ) {
            if ( openFileDialog.ShowDialog() == DialogResult.OK ) {
                Open( openFileDialog.FileName );
            }
        }

        private void menuFileQuit_Click( object sender, EventArgs e ) {
            this.Close();
        }

        private void menuFileSave_Click( object sender, EventArgs e ) {
            if ( m_oto_ini.Equals( "" ) ) {
                if ( saveFileDialog.ShowDialog() != DialogResult.OK ) {
                    return;
                }
                m_oto_ini = saveFileDialog.FileName;
            }
            SaveCor( m_oto_ini );
            Edited = false;
        }

        private void menuFileSaveAs_Click( object sender, EventArgs e ) {
            if ( saveFileDialog.ShowDialog() != DialogResult.OK ) {
                return;
            }
            m_oto_ini = saveFileDialog.FileName;
            SaveCor( m_oto_ini );
            Edited = false;
        }

        private void SaveCor( String file ) {
            // oto.ini
            using ( StreamWriter sw = new StreamWriter( file, false, Encoding.GetEncoding( "Shift_JIS" ) ) ) {
                int i1 = listFiles.Items.Count;
                for ( int i = 0; i < i1; i++ ) {
                    int i2 = listFiles.Items[i].SubItems.Count;
                    sw.Write( listFiles.Items[i].SubItems[0].Text + "=" );
                    for ( int j = 1; j <= 6; j++ ) {
                        sw.Write( (j > 1 ? "," : "") + listFiles.Items[i].SubItems[j].Text );
                    }
                    sw.WriteLine( "" );
                }
            }

            // analyzed/oto.ini
            String analyzed = Path.Combine( Path.GetDirectoryName( file ), "analyzed" );
            String analyzed_oto_ini = Path.Combine( analyzed, Path.GetFileName( file ) );
            if ( !Directory.Exists( analyzed ) ) {
                Directory.CreateDirectory( analyzed );
            }
            using ( StreamWriter sw = new StreamWriter( analyzed_oto_ini, false, Encoding.GetEncoding( "Shift_JIS" ) ) ) {
                int count = listFiles.Items.Count;
                for ( int i = 0; i < count; i++ ) {
                    int i2 = listFiles.Items[i].SubItems.Count;
                    sw.Write( listFiles.Items[i].SubItems[0].Text + "=" );
                    for ( int j = 1; j <= 6; j++ ) {
                        String add = listFiles.Items[i].SubItems[j].Text;
                        if ( j == 2 || j == 4 ) { // j==2はoffset, j==4はblank。STF化した場合、この2つは0固定になる。
                            add = "0";
                        }
                        sw.Write( (j > 1 ? "," : "") + add );
                    }
                    sw.WriteLine( "" );
                }
            }
        }

        private void menuFileOpen_MouseEnter( object sender, EventArgs e ) {
            statusLblTootip.Text = _( "Open Voice DB configuration file" );
        }

        private void menuFileSave_MouseEnter( object sender, EventArgs e ) {
            statusLblTootip.Text = _( "Save Voice DB configuration file." );
        }

        private void menuFileSaveAs_MouseEnter( object sender, EventArgs e ) {
            statusLblTootip.Text = _( "Save Voice DB configuration file with new name." );
        }

        private void menuFileQuit_MouseEnter( object sender, EventArgs e ) {
            statusLblTootip.Text = _( "Close this window." );
        }

        private void FormUtauVoiceConfig_Load( object sender, EventArgs e ) {
            FormConfigUtauVoiceConfig config = null;
            String config_path = Path.Combine( getApplicationDataPath(), "config.xml" );
            if ( File.Exists( config_path ) ) {
                using ( FileStream fs = new FileStream( config_path, FileMode.Open, FileAccess.Read ) ) {
                    XmlSerializer xs = new XmlSerializer( typeof( FormConfigUtauVoiceConfig ) );
                    try {
                        config = (FormConfigUtauVoiceConfig)xs.Deserialize( fs );
                    } catch ( Exception ex ) {
                        config = null;
                    }
                }
                if ( config != null ) {
                    this.CurrentConfig = config;
                }
            }

            // Cadencii本体の設定を読み込む
            Messaging.LoadMessages();
            String dir = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Boare" );
            if ( !Directory.Exists( dir ) ) {
                return;
            }
            String dir2 = Path.Combine( dir, "Cadencii" );
            if ( !Directory.Exists( dir2 ) ) {
                return;
            }
            String path_config_cadencii = Path.Combine( dir2, "config.xml" );
            if ( !File.Exists( path_config_cadencii ) ) {
                return;
            }
            Boare.Cadencii.EditorConfig cadencii_config = null;
            using ( FileStream fs = new FileStream( path_config_cadencii, FileMode.Open, FileAccess.Read ) ) {
                XmlSerializer xs = new XmlSerializer( typeof( Boare.Cadencii.EditorConfig ) );
                cadencii_config = (Boare.Cadencii.EditorConfig)xs.Deserialize( fs );
                String lang = cadencii_config.Language;
                Messaging.Language = lang;
            }
            if ( cadencii_config != null ) {
                AppManager.cadenciiConfig = cadencii_config;
            }
            ApplyLanguage();
        }

        private void FormUtauVoiceConfig_FormClosing( object sender, FormClosingEventArgs e ) {
            if ( Edited ) {
                DialogResult dr = MessageBox.Show( _( "Do you want to save this config?" ), "EditOtoIni", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
                if ( dr == DialogResult.Cancel ) {
                    e.Cancel = true;
                    return;
                } else if ( dr == DialogResult.Yes ) {
                    if ( !m_oto_ini.Equals( "" ) ) {
                        SaveCor( m_oto_ini );
                    } else {
                        DialogResult dr2 = saveFileDialog.ShowDialog();
                        if ( dr2 == DialogResult.Cancel ) {
                            e.Cancel = true;
                            return;
                        } else if ( dr2 == DialogResult.OK ) {
                            m_oto_ini = saveFileDialog.FileName;
                            SaveCor( m_oto_ini );
                        }
                    }
                }
            }

            String config_path = Path.Combine( getApplicationDataPath(), "config.xml" );
#if DEBUG
            Console.WriteLine( "FormUtauVoiceConfig#FormUtauVoiceConfig_FormClosing; config_path=" + config_path );
#endif
            FormConfigUtauVoiceConfig config = this.CurrentConfig;
            using ( FileStream fs = new FileStream( config_path, FileMode.Create, FileAccess.Write ) ) {
                XmlSerializer xs = new XmlSerializer( typeof( FormConfigUtauVoiceConfig ) );
                xs.Serialize( fs, config );
            }
        }

        private void FormUtauVoiceConfig_SizeChanged( object sender, EventArgs e ) {
            if ( this.WindowState == FormWindowState.Normal ) {
                m_current_bounds = this.Bounds;
            }
        }

        private void menuEditGenerateSTF_Click( object sender, EventArgs e ) {
            generateSTForFRQ( FormGenerateStf.GenerateMode.STF );
        }

        private void checkSTFExistence() {
            int count = listFiles.Items.Count;
            if( m_oto_ini.Equals( "" ) ){
                return;
            }
            String analyzed = Path.Combine( Path.GetDirectoryName( m_oto_ini ), "analyzed" );
            for ( int i = 0; i < count; i++ ) {
                ListViewItem item = listFiles.Items[i];
                String wav_name = item.SubItems[0].Text;
                String stf_path = Path.Combine( analyzed, Path.GetFileNameWithoutExtension( wav_name ) + ".stf" );
                item.SubItems[8].Text = File.Exists( stf_path ) ? "○" : "";
            }
        }

        private void checkFRQExistence() {
            int count = listFiles.Items.Count;
            if ( m_oto_ini.Equals( "" ) ) {
                return;
            }
            String dir = Path.GetDirectoryName( m_oto_ini );
            for ( int i = 0; i < count; i++ ) {
                ListViewItem item = listFiles.Items[i];
                String wav_name = item.SubItems[0].Text;
                String frq_path = Path.Combine( dir, wav_name.Replace( ".", "_" ) + ".frq" );
                item.SubItems[7].Text = File.Exists( frq_path ) ? "○" : "";
            }
        }

        private void btnRefreshStf_Click( object sender, EventArgs e ) {
            if ( m_file.Equals( "" ) ) {
                return;
            }
            if ( m_oto_ini.Equals( "" ) ) {
                return;
            }
            String dir = Path.GetDirectoryName( m_oto_ini );
            String analyzed = Path.Combine( dir, "analyzed" );
            String wav_path = Path.Combine( dir, m_file );
            if ( !File.Exists( wav_path ) ) {
                return;
            }
            String stf_path = Path.Combine( analyzed, Path.GetFileNameWithoutExtension( m_file ) + ".stf" );
            if ( File.Exists( stf_path ) ) {
                try {
                    File.Delete( stf_path );
                } catch ( Exception ex ) {
                }
            }
            StfQueueArgs queue = new StfQueueArgs();
            queue.waveName = m_file;
            queue.offset = m_offset + "";
            queue.blank = m_blank + "";
            Vector<StfQueueArgs> list = new Vector<StfQueueArgs>();
            list.add( queue );
            using ( FormGenerateStf form = new FormGenerateStf( m_oto_ini, list, FormGenerateStf.GenerateMode.STF ) ) {
                form.ShowDialog();
            }
            checkSTFExistence();
            Edited = true;
        }

        private void btnRefreshFrq_Click( object sender, EventArgs e ) {
            if ( m_file.Equals( "" ) ) {
                return;
            }
            if ( m_oto_ini.Equals( "" ) ) {
                return;
            }
            String dir = Path.GetDirectoryName( m_oto_ini );
            String wav_path = Path.Combine( dir, m_file );
            if ( !File.Exists( wav_path ) ) {
                return;
            }
            String frq_path = Path.Combine( dir, m_file.Replace( ".", "_" ) + ".frq" );
            if ( File.Exists( frq_path ) ) {
                try {
                    File.Delete( frq_path );
                } catch ( Exception ex ) {
                }
            }
            StfQueueArgs queue = new StfQueueArgs();
            queue.waveName = m_file;
            queue.offset = m_offset + "";
            queue.blank = m_blank + "";
            Vector<StfQueueArgs> list = new Vector<StfQueueArgs>();
            list.add( queue );
            using ( FormGenerateStf form = new FormGenerateStf( m_oto_ini, list, FormGenerateStf.GenerateMode.FRQ ) ) {
                form.ShowDialog();
            }
            checkFRQExistence();
            Edited = true;
        }

        private void menuEditGenerateFRQ_Click( object sender, EventArgs e ) {
            generateSTForFRQ( FormGenerateStf.GenerateMode.FRQ );
        }

        private void generateSTForFRQ( FormGenerateStf.GenerateMode mode ) {
            Vector<StfQueueArgs> list = new Vector<StfQueueArgs>();
            int count = listFiles.Items.Count;
            for ( int i = 0; i < count; i++ ) {
                ListViewItem item = listFiles.Items[i];
                StfQueueArgs queue = new StfQueueArgs();
                queue.waveName = item.SubItems[0].Text;
                queue.offset = item.SubItems[2].Text;
                queue.blank = item.SubItems[4].Text;
                list.add( queue );
            }
            if ( list.size() <= 0 ) {
                if ( mode == FormGenerateStf.GenerateMode.STF ) {
                    checkSTFExistence();
                } else {
                    checkFRQExistence();
                }
                return;
            }
            using ( FormGenerateStf form = new FormGenerateStf( m_oto_ini, list, mode ) ) {
                form.ShowDialog();
            }
            if ( mode == FormGenerateStf.GenerateMode.STF ) {
                checkSTFExistence();
            } else {
                checkFRQExistence();
            }
            Edited = true;
        }

        private void bgWorkScreen_DoWork( object sender, DoWorkEventArgs e ) {
            this.Invoke( new Boare.Cadencii.VoidDelegate( this.refreshScreenCore ) );
        }

        private void refreshScreenCore() {
            pictWave.Invalidate();
        }

        private void txtSearch_TextChanged( object sender, EventArgs e ) {
            searchCor( false );
        }

        private void buttonNext_Click( object sender, EventArgs e ) {
            searchCor( false );
        }

        private void buttonPrevious_Click( object sender, EventArgs e ) {
            searchCor( true );
        }

        private bool checkListFileItem( int index, String search ) {
            ListViewItem item = listFiles.Items[index];
            if ( item.SubItems[0].Text.Contains( search ) ) {
                item.Selected = true;
                listFiles.EnsureVisible( index );
                return true;
            }
            if ( item.SubItems[1].Text.Contains( search ) ) {
                item.Selected = true;
                listFiles.EnsureVisible( index );
                return true;
            }
            return false;
        }

        private void setSearchTextColor( bool found ) {
            if ( found ) {
                txtSearch.BackColor = SystemColors.Window;
                txtSearch.ForeColor = SystemColors.WindowText;
            } else {
                txtSearch.BackColor = Color.LightCoral;
                txtSearch.ForeColor = Color.White;
            }
        }

        private void searchCor( bool go_back ) {
            if ( txtSearch.Text.Equals( "" ) ) {
                setSearchTextColor( false );
                return;
            }
            int first_index;
            if ( listFiles.SelectedIndices.Count <= 0 ) {
                if ( listFiles.Items.Count <= 0 ) {
                    setSearchTextColor( false );
                    return;
                }
                first_index = 0;
            } else {
                first_index = listFiles.SelectedIndices[0];
            }
            String search = txtSearch.Text;
            if ( go_back ) {
                for ( int i = first_index - 1; i >= 0; i-- ) {
                    if ( checkListFileItem( i, search ) ) {
                        setSearchTextColor( true );
                        return;
                    }
                }
                for ( int i = listFiles.Items.Count - 1; i >= first_index; i-- ) {
                    if ( checkListFileItem( i, search ) ) {
                        setSearchTextColor( true );
                        return;
                    }
                }
            } else {
                for ( int i = first_index + 1; i < listFiles.Items.Count; i++ ) {
                    if ( checkListFileItem( i, search ) ) {
                        setSearchTextColor( true );
                        return;
                    }
                }
                for ( int i = 0; i <= first_index; i++ ) {
                    if ( checkListFileItem( i, search ) ) {
                        setSearchTextColor( true );
                        return;
                    }
                }
            }
            setSearchTextColor( false );
        }

        private void menuViewSearchNext_Click( object sender, EventArgs e ) {
            searchCor( false );
        }

        private void menuViewSearchPrevious_Click( object sender, EventArgs e ) {
            searchCor( true );
        }
    }

}
