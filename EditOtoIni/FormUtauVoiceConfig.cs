﻿/*
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
#if JAVA
package org.kbinani.EditOtoIni;

import java.awt.*;
import java.io.*;
import java.util.*;
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.componentModel.*;
import org.kbinani.media.*;
import org.kbinani.xml.*;
import org.kbinani.windows.forms.*;
#else
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Boare.Cadencii;
using Boare.Lib.AppUtil;
using Boare.Lib.Media;
using bocoree;
using bocoree.awt;
using bocoree.io;
using bocoree.util;
using bocoree.windows.forms;
using bocoree.xml;

namespace Boare.EditOtoIni {
    using BDoWorkEventArgs = System.ComponentModel.DoWorkEventArgs;
    using BEventArgs = System.EventArgs;
    using BEventHandler = System.EventHandler;
    using boolean = System.Boolean;
    using Float = System.Single;
    using Graphics = bocoree.awt.Graphics2D;
    using java = bocoree;
#endif

#if JAVA
    public class FormUtauVoiceConfig extends BForm
#else
    public class FormUtauVoiceConfig : BForm {
#endif
        enum MouseMode {
            None,
            MiddleDrag,
            MoveOffset,
            MoveConsonant,
            MoveBlank,
            MovePreUtterance,
            MoveOverlap,
        }

        public class FlagType {
            public static readonly FlagType Offset = new FlagType( 0 );
            public static readonly FlagType Consonant = new FlagType( 1 );
            public static readonly FlagType Blank = new FlagType( 2 );
            public static readonly FlagType PreUtterance = new FlagType( 3 );
            public static readonly FlagType Overlap = new FlagType( 4 );

            private int m_value;

            private FlagType( int value ) {
                m_value = value;
            }

            public int getValue() {
                return m_value;
            }
        }

        const int LINE_HEIGHT = 20;
        const int TRACKBAR_MAX = 35;
        const int TRACKBAR_MIN = 15;
        const int ORDER = 10;

        private Vector<WaveDrawContext> m_drawer = new Vector<WaveDrawContext>();
        private int m_index = -1;
        private double m_px_per_sec = 1000;
        private float m_start_to_draw = 0.0f;
        private Color m_brs_offset = new Color( 192, 192, 255 );
        private Color m_brs_consonant = new Color( 255, 192, 255 );
        private Color m_brs_blank = new Color( 192, 192, 255 );
        private Color m_pen_overlap = new Color( 0, 255, 0 );
        private Color m_pen_preutterance = new Color( 255, 0, 0 );
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
#if JAVA
            initialize();
#else
            InitializeComponent();
#endif
            pictWave.MouseWheel += new MouseEventHandler( pictWave_MouseWheel );
            splitContainerIn.Panel1.BorderStyle = BorderStyle.None;
            splitContainerIn.Panel2.BorderStyle = BorderStyle.FixedSingle;
            splitContainerIn.Panel2.BorderColor = System.Drawing.SystemColors.ControlDark;

            splitContainerOut.Panel1.BorderStyle = BorderStyle.None;
            splitContainerOut.Panel2.BorderStyle = BorderStyle.FixedSingle;
            splitContainerOut.Panel2.BorderColor = System.Drawing.SystemColors.ControlDark;

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

#if JAVA
        public static void main( String[] args ){
            FormUtauVoiceConfig form = new FormUtauVoiceConfig();
            form.setDefaultCloseOperation( JFrame.EXIT_ON_CLOSE );
            form.setVisible( true );
#else
        [STAThread]
        public static void Main( string[] args ) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new FormUtauVoiceConfig() );
#endif
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
            String dir = PortUtil.combinePath( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Boare" );
            if ( !PortUtil.isDirectoryExists( dir ) ) {
                PortUtil.createDirectory( dir );
            }
            String dir2 = PortUtil.combinePath( dir, "EditOtoIni" );
            if ( !PortUtil.isDirectoryExists( dir2 ) ) {
                PortUtil.createDirectory( dir2 );
            }
            return dir2;
        }

        public FormConfigUtauVoiceConfig getCurrentConfig() {
            FormConfigUtauVoiceConfig ret = new FormConfigUtauVoiceConfig();
            ret.Bounds = new XmlRectangle( m_current_bounds );
            ret.State = this.getExtendedState();
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

        public void setCurrentConfig( FormConfigUtauVoiceConfig value ) {
#if DEBUG
            Console.WriteLine( "FormUtauVoiceConfig#set_CurrentConfig" );
#endif
            if ( value.State != BForm.MAXIMIZED_BOTH ) {
                this.setBounds( value.Bounds.toRectangle() );
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

        public void ApplyFont( java.awt.Font font ) {
            Util.applyFontRecurse( this, font );
            Util.applyToolStripFontRecurse( menuFile, font );
        }

        private void HoverWaitThread() {
            Thread.Sleep( SystemInformation.MouseHoverTime );
            EventHandler eh = new EventHandler( pictWave_MouseHover );
            this.Invoke( eh, pictWave, new EventArgs() );
        }

        private void pictWave_MouseHover( Object sender, EventArgs e ) {
        }

        public void ApplyLanguage() {
            UpdateFormTitle();

            menuFile.setText( _( "File" ) + "(&F)" );
            menuFileOpen.setText( _( "Open" ) + "(&O)" );
            menuFileSave.setText( _( "Save" ) + "(&S)" );
            menuFileSaveAs.setText( _( "Save As" ) + "(&A)" );
            menuFileQuit.setText( _( "Quit" ) + "(&Q)" );
            menuEdit.setText( _( "Edit" ) + "(&E)" );
            menuEditGenerateFRQ.setText( _( "Generate FRQ files" ) );
            menuEditGenerateSTF.setText( _( "Generate STF files" ) );
            menuView.setText( _( "View" ) + "(&V)" );
            menuViewSearchNext.setText( _( "Search Next" ) + "(&N)" );
            menuViewSearchPrevious.setText( _( "Search Previous" ) + "(&N)" );

            lblFileName.setText( _( "File Name" ) );
            lblAlias.setText( _( "Alias" ) );
            lblOffset.setText( _( "Offset" ) );
            lblConsonant.setText( _( "Consonant" ) );
            lblBlank.setText( _( "Blank" ) );
            lblPreUtterance.setText( _( "Pre Utterance" ) );
            lblOverlap.setText( _( "Overlap" ) );

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
            } catch ( Exception ex ) {
                openFileDialog.Filter = "Voice DB Config(*.ini)|*.ini|All Files(*.*)|*.*";
                saveFileDialog.Filter = "Voice DB Config(*.ini)|*.ini|All Files(*.*)|*.*";
            }

            btnRefreshStf.setText( _( "Refresh STF" ) );
            btnRefreshFrq.setText( _( "Refresh FRQ" ) );

            lblSearch.setText( _( "Search" ) + ":" );
            buttonNext.setText( _( "Next" ) );
            buttonPrevious.setText( _( "Previous" ) );
#if !JAVA
            int CLEALANCE = 6;
            txtSearch.Left = lblSearch.Left + lblSearch.Width + CLEALANCE;
            buttonNext.Left = txtSearch.Left + txtSearch.Width + CLEALANCE;
            buttonPrevious.Left = buttonNext.Left + buttonNext.Width + CLEALANCE;
#endif
        }

        private void UpdateFormTitle() {
            String f = m_oto_ini;
            if ( f.Equals( "" ) ) {
                f = "Untitled";
            }
            String title = _( "Voice DB Config" ) + " - " + f + (m_edited ? " *" : "");
            if ( title != this.getTitle() ) {
                this.setTitle( title );
            }
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void pictWave_MouseWheel( Object sender, MouseEventArgs e ) {
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
#if JAVA
            bgWorkRead.runWorkerAsync( oto_ini_path );
#else
            bgWorkRead.RunWorkerAsync( oto_ini_path );
#endif
        }

        private void AddItem( Object sender, boolean bool_value, String[] stringarr_value ) {
            String[] columns = stringarr_value;
            boolean exists = bool_value;
            ListViewItem item = new ListViewItem( columns );
            item.Tag = exists;
            listFiles.Items.Add( item );
        }

        private void bgWorkRead_DoWork( Object sender, DoWorkEventArgs e ) {
            String oto_ini_path = (String)e.Argument;
            int c = m_drawer.size();
            for ( int i = 0; i < c; i++ ) {
                m_drawer.get( i ).Dispose();
            }
            m_drawer.clear();

#if DEBUG
            PortUtil.println( "FormUtauVoiceConfig#bgWorkRead_DoWork; oto_init_path=" + oto_ini_path );
#endif
            if ( !PortUtil.isFileExists( oto_ini_path ) ) {
#if DEBUG
                PortUtil.println( "FormUtauVoiceConfig#bgWorkRead_DoWork; '" + oto_ini_path + "' not found" );
#endif
                return;
            }

            String dir = PortUtil.getDirectoryName( oto_ini_path );
            BufferedReader sr = null;
            try {
                sr = new BufferedReader( new InputStreamReader( new FileInputStream( oto_ini_path ), "Shift_JIS" ) );
                String line = "";
                while ( (line = sr.readLine()) != null ) {
#if DEBUG
                    PortUtil.println( "FormUtauVoiceConfig#bgWorkRead_DoWork; line=" + line );
#endif
                    if ( m_cancel_required ) {
#if DEBUG
                        PortUtil.println( "FormUtauVoiceConfig#bgWorkRead_DoWork; cancel required" );
#endif
                        break;
                    }
                    int eq = line.IndexOf( '=' );
                    if ( eq <= 0 ) {
                        continue;
                    }
                    String f = line.Substring( 0, eq );
                    line = line.Substring( eq + 1 );
                    String[] spl = line.Split( ',' );
                    String file = PortUtil.combinePath( dir, f );
                    Boare.Cadencii.WaveDrawContext wdc = new Boare.Cadencii.WaveDrawContext( file );
                    wdc.setName( f + spl[0] );
                    String wave_name = PortUtil.getFileNameWithoutExtension( f );
                    String ext = PortUtil.getExtension( file ).Replace( ".", "" );
                    String f2 = wave_name + "_" + ext + ".frq"; // f.Replace( ".wav", "_wav.frq" );
                    String freq = PortUtil.combinePath( dir, f2 );
                    boolean freq_exists = PortUtil.isFileExists( freq );
                    if ( freq_exists ) {
                        wdc.Freq = Boare.Cadencii.UtauFreq.FromFrq( freq );
                    }
                    m_drawer.add( wdc );
                    Vector<String> columns = new Vector<String>( spl );
                    columns.insertElementAt( f, 0 );
                    columns.add( freq_exists ? "○" : "" );
                    String stf = PortUtil.combinePath( PortUtil.combinePath( dir, "analyzed" ), wave_name + ".stf" );
                    boolean stf_exists = PortUtil.isFileExists( stf );
                    columns.add( stf_exists ? "○" : "" );
                    AddItemEventHandler deleg =
                        new AddItemEventHandler( AddItem );
                    this.Invoke( deleg,
                                 this,
                                 PortUtil.isFileExists( file ),
                                 columns.toArray( new String[] { } ) );
                }
            } catch ( Exception ex ) {
#if DEBUG
                PortUtil.println( "FormUtauVoiceConfig#bgWorkRead_DoWork; ex=" + ex );
#endif
            } finally {
                if ( sr != null ) {
                    try {
                        sr.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
            m_edited = false;
        }

        private void listFiles_SelectedIndexChanged( Object sender, EventArgs e ) {
            if ( listFiles.SelectedIndices.Count <= 0 ) {
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
                if ( name.Equals( m_drawer.get( i ).getName() ) ) {
                    m_index = i;
                    m_length = m_drawer.get( i ).getLength();
                    int c2 = selected_item.SubItems.Count;
                    String[] spl = new String[c2];
                    for ( int i2 = 0; i2 < c2; i2++ ) {
                        spl[i2] = selected_item.SubItems[i2].Text;
                    }
                    boolean old = getEdited();
                    txtFileName.setText( spl[0] );
                    m_file = spl[0];
                    txtAlias.setText( spl[1] );
                    ByRef<Float> o = new ByRef<Float>();

                    if ( PortUtil.tryParseFloat( spl[2], o ) ) {
                        m_offset = round2Digits( o.value );
                        txtOffset.setText( m_offset.ToString() );
                    }

                    if ( PortUtil.tryParseFloat( spl[3], o ) ) {
                        m_consonant = round2Digits( o.value );
                        txtConsonant.setText( m_consonant.ToString() );
                    }

                    if ( PortUtil.tryParseFloat( spl[4], o ) ) {
                        m_blank = round2Digits( o.value );
                        txtBlank.setText( m_blank.ToString() );
                    }

                    if ( PortUtil.tryParseFloat( spl[5], o ) ) {
                        m_pre_utterance = round2Digits( o.value );
                        txtPreUtterance.setText( m_pre_utterance.ToString() );
                    }

                    if ( PortUtil.tryParseFloat( spl[6], o ) ) {
                        m_overlap = round2Digits( o.value );
                        txtOverlap.setText( m_overlap.ToString() );
                    }
                    setEdited( old );
                    float minimum = Math.Min( m_overlap, Math.Min( m_offset, m_pre_utterance ) );
                    minimum = Math.Min( minimum, 0.0f );
#if DEBUG
                    Console.WriteLine( "FormUtauVoiceConfig#listFiles_SelectedIndexChanged; minimum=" + minimum );
                    Console.WriteLine( "FormUtauVoiceConfig#listFiles_SelectedIndexChanged; m_drawer.get( i ).Length=" + m_drawer.get( i ).getLength() );
                    Console.WriteLine( "FormUtauVoiceConfig#listFiles_SelectedIndexChanged; A; hScroll.Minimum=" + hScroll.Minimum + "; hScroll.Maximum=" + hScroll.Maximum );
#endif
                    hScroll.Minimum = (int)(minimum * ORDER);
                    hScroll.Maximum = (int)(m_drawer.get( i ).getLength() * 1000 * ORDER);
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
            if ( rc.x <= p.x ) {
                if ( p.x <= rc.x + rc.width ) {
                    if ( rc.y <= p.y ) {
                        if ( p.y <= rc.y + rc.height ) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void pictWave_Paint( Object sender, PaintEventArgs e ) {
            if ( 0 <= m_index && m_index < m_drawer.size() ) {
                paint( new Graphics2D( e.Graphics ) );
            }
        }

        private void paint( Graphics g1 ) {
            Graphics2D g = (Graphics2D)g1;
            /*int c = listFiles.Items[m_index].SubItems.Count;
            String[] spl = new String[c];
            for ( int i = 0; i < c; i++ ) {
                spl[i] = listFiles.Items[m_index].SubItems[i].Text;
            }*/
            g.clearRect( 0, 0, pictWave.Width, pictWave.Height );
            int h = pictWave.Height;

            int x_offset = XFromSec( 0.0f );
            Rectangle rc = new Rectangle( x_offset, 0, (int)(m_offset / 1000.0f * m_px_per_sec), h );
            g.setColor( m_brs_offset );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );

            int x_consonant = XFromSec( m_offset / 1000.0f );
            rc = new Rectangle( x_consonant, 0, (int)(m_consonant / 1000.0f * m_px_per_sec), h );
            g.setColor( m_brs_consonant );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );

            int x_blank = XFromSec( m_drawer.get( m_index ).getLength() - m_blank / 1000.0f );
            rc = new Rectangle( x_blank, 0, (int)(m_blank / 1000.0f * m_px_per_sec), h );
            g.setColor( m_brs_blank );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );

            m_drawer.get( m_index ).draw( g,
                                          Color.black,
                                          new Rectangle( 0, 0, pictWave.Width, h ),
                                          m_start_to_draw,
                                          (float)(m_start_to_draw + pictWave.Width / m_px_per_sec) );

            int x_overlap = XFromSec( m_overlap / 1000.0f );
            g.setColor( m_pen_overlap );
            g.drawLine( x_overlap, 0, x_overlap, h );

            int x_pre_utterance = XFromSec( m_pre_utterance / 1000.0f );
            g.setColor( m_pen_preutterance );
            g.drawLine( x_pre_utterance, 0, x_pre_utterance, h );

            Font font = AppManager.cadenciiConfig.getBaseFont();

            int x_lastpreview = XFromSec( m_last_preview / 1000.0f );
            g.setColor( Color.blue );
            g.drawLine( x_lastpreview, 0, x_lastpreview, h );
            g.setFont( font );
            g.setColor( Color.black );
            g.drawString( m_last_preview + " ms", x_lastpreview + 1, 1 );

            g.drawString( m_file, 1, LINE_HEIGHT );

            if ( font.font.Name != m_font_name || font.font.SizeInPoints != m_font_size ) {
                m_font_draw_offset = Util.getStringDrawOffset( font ) - 1;
                m_font_name = font.font.Name;
                m_font_size = font.font.SizeInPoints;
            }

            Color brs = new Color( 255, 255, 255, 180 );
            //using ( SolidBrush brs = new SolidBrush( Color.FromArgb( 180, Color.White ) ) ) {
            rc = GetFlagRect( FlagType.Offset, font );
            m_flag_box[0] = rc;
            g.setColor( brs );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );
            g.setColor( Color.black );
            g.drawRect( rc.x, rc.y, rc.width, rc.height );
            g.drawString( _( "Offset" ) + ": " + m_offset + " ms", rc.x + 1, rc.y + rc.height * 0.5f - m_font_draw_offset );

            rc = GetFlagRect( FlagType.Consonant, font );
            m_flag_box[1] = rc;
            g.setColor( brs );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );
            g.setColor( Color.black );
            g.drawRect( rc.x, rc.y, rc.width, rc.height );
            g.drawString( _( "Consonant" ) + ": " + m_consonant + " ms", rc.x + 1, rc.y + rc.height * 0.5f - m_font_draw_offset );

            rc = GetFlagRect( FlagType.Blank, font );
            m_flag_box[2] = rc;
            g.setColor( brs );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );
            g.setColor( Color.black );
            g.drawRect( rc.x, rc.y, rc.width, rc.height );
            g.drawString( _( "Blank" ) + ": " + m_blank + " ms", rc.x + 1, rc.y + rc.height * 0.5f - m_font_draw_offset );

            rc = GetFlagRect( FlagType.PreUtterance, font );
            m_flag_box[3] = rc;
            g.setColor( brs );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );
            g.setColor( Color.black );
            g.drawRect( rc.x, rc.y, rc.width, rc.height );
            g.drawString( _( "Pre Utterance" ) + ": " + m_pre_utterance + " ms", rc.x + 1, rc.y + rc.height * 0.5f - m_font_draw_offset );

            rc = GetFlagRect( FlagType.Overlap, font );
            m_flag_box[4] = rc;
            g.setColor( brs );
            g.fillRect( rc.x, rc.y, rc.width, rc.height );
            g.setColor( Color.black );
            g.drawRect( rc.x, rc.y, rc.width, rc.height );
            g.drawString( _( "Overlap" ) + ": " + m_overlap + " ms", rc.x + 1, rc.y + rc.height * 0.5f - m_font_draw_offset );
            //}
        }

        private Rectangle GetFlagRect( FlagType type, Font font ) {
            int i = type.getValue();
            int x = 0;
            String s = "";
            if ( i == FlagType.Offset.getValue() ) {
                x = XFromSec( m_offset / 1000.0f );
                s = _( "Offset" ) + ": " + m_offset + " ms";
            } else if ( i == FlagType.Consonant.getValue() ) {
                x = XFromSec( (m_offset + m_consonant) / 1000.0f );
                s = _( "Consonant" ) + ": " + m_consonant + " ms";
            } else if ( i == FlagType.Blank.getValue() ) {
                x = XFromSec( m_length - m_blank / 1000.0f );
                s = _( "Blank" ) + ": " + m_blank + " ms";
            } else if ( i == FlagType.PreUtterance.getValue() ) {
                x = XFromSec( m_pre_utterance / 1000.0f );
                s = _( "Pre Utterance" ) + ": " + m_pre_utterance + " ms";
            } else if ( i == FlagType.Overlap.getValue() ) {
                x = XFromSec( m_overlap / 1000.0f );
                s = _( "Overlap" ) + ": " + m_overlap + " ms";
            }
            Dimension size = Util.measureString( s, font );
            return new Rectangle( x, LINE_HEIGHT * (i + 2), (int)(size.width * 1.1f), (int)(size.height * 1.1f) );
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

        private void hScroll_ValueChanged( Object sender, EventArgs e ) {
            m_start_to_draw = hScroll.Value / 1000.0f / ORDER;
            refreshScreen();
        }

        private void pictWave_MouseDown( Object sender, MouseEventArgs e ) {
            m_mouse_downed = new Point( e.X, e.Y );
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
                    if ( isInRect( new Point( e.X, e.Y ), rc ) ) {
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

                if ( 0 <= m_index && m_index < listFiles.Items.Count ) {
                    String file = PortUtil.combinePath( PortUtil.getDirectoryName( m_oto_ini ), listFiles.Items[m_index].SubItems[0].Text );
                    if ( PortUtil.isFileExists( file ) && m_player.SoundLocation != file ) {
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

        private void pictWave_MouseMove( Object sender, MouseEventArgs e ) {
            if ( m_mouse_hover_generator != null && m_mouse_hover_generator.IsAlive ) {
                m_mouse_hover_generator.Abort();
            }
            boolean check_hscroll_minimum = false;
            float minimum = 0;
            if ( m_mode == MouseMode.MiddleDrag ) {
                int dx = e.X - m_mouse_downed.x;
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
                int dx = e.X - m_mouse_downed.x;
                float draft = round2Digits( m_move_init + (float)(dx / m_px_per_sec * 1000) );
                txtOffset.Text = draft.ToString();
                check_hscroll_minimum = true;
                minimum = Math.Min( draft, minimum );
            } else if ( m_mode == MouseMode.MoveConsonant ) {
                int dx = e.X - m_mouse_downed.x;
                float draft = round2Digits( m_move_init + (float)(dx / m_px_per_sec * 1000) );
                txtConsonant.Text = draft.ToString();
            } else if ( m_mode == MouseMode.MoveBlank ) {
                int dx = e.X - m_mouse_downed.x;
                float draft = round2Digits( m_move_init - (float)(dx / m_px_per_sec * 1000) );
                txtBlank.Text = draft.ToString();
            } else if ( m_mode == MouseMode.MovePreUtterance ) {
                int dx = e.X - m_mouse_downed.x;
                float draft = round2Digits( m_move_init + (float)(dx / m_px_per_sec * 1000) );
                txtPreUtterance.Text = draft.ToString();
                check_hscroll_minimum = true;
                minimum = Math.Min( draft, minimum );
            } else if ( m_mode == MouseMode.MoveOverlap ) {
                int dx = e.X - m_mouse_downed.x;
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

        private boolean getEdited() {
            return m_edited;
        }

        private void setEdited( boolean value ) {
            boolean old = m_edited;
            m_edited = value;
            if ( m_edited != old ) {
                UpdateFormTitle();
            }
        }

        private void pictWave_MouseUp( Object sender, MouseEventArgs e ) {
            m_mode = MouseMode.None;
        }

        private void txtAlias_TextChanged( Object sender, EventArgs e ) {
            if ( 0 <= m_index && m_index < listFiles.Items.Count ) {
                listFiles.Items[m_index].SubItems[1].Text = txtAlias.Text;
                setEdited( true );
                pictWave.Invalidate();
            }
        }

        private void txtOffset_TextChanged( Object sender, EventArgs e ) {
            float i;
            try {
                i = PortUtil.parseFloat( txtOffset.getText() );
            } catch ( Exception ex ) {
                return;
            }
            m_offset = round2Digits( i );
            if ( 0 <= m_index && m_index < listFiles.Items.Count ) {
                listFiles.Items[m_index].SubItems[2].Text = txtOffset.Text;
                setEdited( true );
                pictWave.Invalidate();
            }
        }

        private void txtConsonant_TextChanged( Object sender, EventArgs e ) {
            float i;
            try {
                i = PortUtil.parseFloat( txtConsonant.getText() );
            } catch ( Exception ex ) {
                return;
            }
            m_consonant = round2Digits( i );
            if ( 0 <= m_index && m_index < listFiles.Items.Count ) {
                listFiles.Items[m_index].SubItems[3].Text = txtConsonant.Text;
                setEdited( true );
                pictWave.Invalidate();
            }
        }

        private void txtBlank_TextChanged( Object sender, EventArgs e ) {
            float i;
            try {
                i = PortUtil.parseFloat( txtBlank.getText() );
            } catch ( Exception ex ) {
                return;
            }
            m_blank = round2Digits( i );
            if ( 0 <= m_index && m_index < listFiles.Items.Count ) {
                listFiles.Items[m_index].SubItems[4].Text = txtBlank.Text;
                setEdited( true );
                pictWave.Invalidate();
            }
        }

        private void txtPreUtterance_TextChanged( Object sender, EventArgs e ) {
            float i;
            try {
                i = PortUtil.parseFloat( txtPreUtterance.getText() );
            } catch ( Exception ex ) {
                return;
            }
            m_pre_utterance = round2Digits( i );
            if ( 0 <= m_index && m_index < listFiles.Items.Count ) {
                listFiles.Items[m_index].SubItems[5].Text = txtPreUtterance.Text;
                setEdited( true );
                pictWave.Invalidate();
            }
        }

        private void txtOverlap_TextChanged( Object sender, EventArgs e ) {
            float i;
            try {
                i = PortUtil.parseFloat( txtOverlap.getText() );
            } catch ( Exception ex ) {
                return;
            }
            m_overlap = round2Digits( i );
            if ( 0 <= m_index && m_index < listFiles.Items.Count ) {
                listFiles.Items[m_index].SubItems[6].Text = txtOverlap.Text;
                setEdited( true );
                pictWave.Invalidate();
            }
        }

        private void FormUtauVoiceConfig_FormClosed( Object sender, FormClosedEventArgs e ) {
            m_cancel_required = true;
            while ( bgWorkRead.IsBusy ) {
                Application.DoEvents();
            }
            if ( m_player != null ) {
                m_player.Close();
            }
        }

        private void btnMinus_Click( Object sender, EventArgs e ) {
            if ( TRACKBAR_MIN < m_trackbar_value ) {
                m_trackbar_value--;
                UpdateScale();
                refreshScreen();
            }
        }

        private void btnPlus_Click( Object sender, EventArgs e ) {
            if ( m_trackbar_value < TRACKBAR_MAX ) {
                m_trackbar_value++;
                UpdateScale();
                refreshScreen();
            }
        }

        private void bgWorkRead_RunWorkerCompleted( Object sender, RunWorkerCompletedEventArgs e ) {
            setEdited( false );
        }

        private void menuFileOpen_Click( Object sender, EventArgs e ) {
            if ( openFileDialog.ShowDialog() == DialogResult.OK ) {
                Open( openFileDialog.FileName );
            }
        }

        private void menuFileQuit_Click( Object sender, EventArgs e ) {
            this.Close();
        }

        private void menuFileSave_Click( Object sender, EventArgs e ) {
            if ( m_oto_ini.Equals( "" ) ) {
                if ( saveFileDialog.ShowDialog() != DialogResult.OK ) {
                    return;
                }
                m_oto_ini = saveFileDialog.FileName;
            }
            saveCor( m_oto_ini );
            setEdited( false );
        }

        private void menuFileSaveAs_Click( Object sender, EventArgs e ) {
            if ( saveFileDialog.ShowDialog() != DialogResult.OK ) {
                return;
            }
            m_oto_ini = saveFileDialog.FileName;
            saveCor( m_oto_ini );
            setEdited( false );
        }

        private void saveCor( String file ) {
            // oto.ini
            BufferedWriter sw = null;
            try {
                sw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( file ), "Shift_JIS" ) );
                int i1 = listFiles.Items.Count;
                for ( int i = 0; i < i1; i++ ) {
                    int i2 = listFiles.Items[i].SubItems.Count;
                    sw.write( listFiles.Items[i].SubItems[0].Text + "=" );
                    for ( int j = 1; j <= 6; j++ ) {
                        sw.write( (j > 1 ? "," : "") + listFiles.Items[i].SubItems[j].Text );
                    }
                    sw.newLine();
                }
            } catch ( Exception ex ) {
            } finally {
                if ( sw != null ) {
                    try {
                        sw.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }

            // analyzed/oto.ini
            String analyzed = PortUtil.combinePath( PortUtil.getDirectoryName( file ), "analyzed" );
            String analyzed_oto_ini = PortUtil.combinePath( analyzed, PortUtil.getFileName( file ) );
            if ( !PortUtil.isDirectoryExists( analyzed ) ) {
                PortUtil.createDirectory( analyzed );
            }
            BufferedWriter sw2 = null;
            try {
                sw2 = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( analyzed_oto_ini ), "Shift_JIS" ) );
                int count = listFiles.Items.Count;
                for ( int i = 0; i < count; i++ ) {
                    int i2 = listFiles.Items[i].SubItems.Count;
                    sw2.write( listFiles.Items[i].SubItems[0].Text + "=" );
                    for ( int j = 1; j <= 6; j++ ) {
                        String add = listFiles.Items[i].SubItems[j].Text;
                        if ( j == 2 || j == 4 ) { // j==2はoffset, j==4はblank。STF化した場合、この2つは0固定になる。
                            add = "0";
                        }
                        sw2.write( (j > 1 ? "," : "") + add );
                    }
                    sw2.newLine();
                }
            } catch ( Exception ex ) {
            } finally {
                if ( sw2 != null ) {
                    try {
                        sw2.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        private void menuFileOpen_MouseEnter( Object sender, BEventArgs e ) {
            statusLblTootip.Text = _( "Open Voice DB configuration file" );
        }

        private void menuFileSave_MouseEnter( Object sender, BEventArgs e ) {
            statusLblTootip.Text = _( "Save Voice DB configuration file." );
        }

        private void menuFileSaveAs_MouseEnter( Object sender, BEventArgs e ) {
            statusLblTootip.Text = _( "Save Voice DB configuration file with new name." );
        }

        private void menuFileQuit_MouseEnter( Object sender, BEventArgs e ) {
            statusLblTootip.Text = _( "Close this window." );
        }

        private void FormUtauVoiceConfig_Load( Object sender, BEventArgs e ) {
            FormConfigUtauVoiceConfig config = null;
            String config_path = PortUtil.combinePath( getApplicationDataPath(), "config.xml" );
            if ( PortUtil.isFileExists( config_path ) ) {
                FileInputStream fout = null;
                try {
                    fout = new FileInputStream( config_path );
#if JAVA
                    XmlSerializer xs = new XmlSerializer( FormConfigUtauVoiceConfig.class );
#else
                    XmlSerializer xs = new XmlSerializer( typeof( FormConfigUtauVoiceConfig ) );
#endif
                    try {
                        config = (FormConfigUtauVoiceConfig)xs.deserialize( fout );
                    } catch ( Exception ex ) {
                        config = null;
                    }
                } catch ( Exception ex1 ) {
                } finally {
                    if ( fout != null ) {
                        try {
                            fout.close();
                        } catch ( Exception ex2 ) {
                        }
                    }
                }
                if ( config != null ) {
                    this.setCurrentConfig( config );
                }
            }

            // Cadencii本体の設定を読み込む
            Messaging.loadMessages();
            String dir = PortUtil.combinePath( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Boare" );
            if ( !PortUtil.isDirectoryExists( dir ) ) {
                return;
            }
            String dir2 = PortUtil.combinePath( dir, "Cadencii" );
            if ( !PortUtil.isDirectoryExists( dir2 ) ) {
                return;
            }
            String path_config_cadencii = PortUtil.combinePath( dir2, "config.xml" );
            if ( !PortUtil.isFileExists( path_config_cadencii ) ) {
                return;
            }
            Boare.Cadencii.EditorConfig cadencii_config = null;
            FileInputStream fin = null;
            try {
                fin = new FileInputStream( path_config_cadencii );
#if JAVA
                XmlSerializer xs = new XmlSerializer( com.boare.cadencii.EditorConfig.class );
                cadencii_config = (com.boare.cadencii.EditorConfig)xs.deserialize( fin );
#else
                XmlSerializer xs = new XmlSerializer( typeof( Boare.Cadencii.EditorConfig ) );
                cadencii_config = (Boare.Cadencii.EditorConfig)xs.deserialize( fin );
#endif
                String lang = cadencii_config.Language;
                Messaging.setLanguage( lang );
            } catch ( Exception ex ) {
            } finally {
                if ( fin != null ) {
                    try {
                        fin.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
            if ( cadencii_config != null ) {
                AppManager.cadenciiConfig = cadencii_config;
            }
            ApplyLanguage();
        }

        private void FormUtauVoiceConfig_FormClosing( Object sender, FormClosingEventArgs e ) {
            if ( getEdited() ) {
                DialogResult dr = MessageBox.Show( _( "Do you want to save this config?" ), "EditOtoIni", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
                if ( dr == DialogResult.Cancel ) {
                    e.Cancel = true;
                    return;
                } else if ( dr == DialogResult.Yes ) {
                    if ( !m_oto_ini.Equals( "" ) ) {
                        saveCor( m_oto_ini );
                    } else {
                        DialogResult dr2 = saveFileDialog.ShowDialog();
                        if ( dr2 == DialogResult.Cancel ) {
                            e.Cancel = true;
                            return;
                        } else if ( dr2 == DialogResult.OK ) {
                            m_oto_ini = saveFileDialog.FileName;
                            saveCor( m_oto_ini );
                        }
                    }
                }
            }

            String config_path = PortUtil.combinePath( getApplicationDataPath(), "config.xml" );
#if DEBUG
            Console.WriteLine( "FormUtauVoiceConfig#FormUtauVoiceConfig_FormClosing; config_path=" + config_path );
#endif
            FormConfigUtauVoiceConfig config = this.getCurrentConfig();
            FileOutputStream fs = null;
            try {
                fs = new FileOutputStream( config_path );
                XmlSerializer xs = new XmlSerializer( typeof( FormConfigUtauVoiceConfig ) );
                xs.serialize( fs, config );
            } catch ( Exception ex ) {
#if DEBUG
                Console.WriteLine( "FormUtauVoiceConfig#FormUtauVoiceConfig_FormClosing; ex=" + ex );
#endif
            } finally {
                if ( fs != null ) {
                    try {
                        fs.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        private void FormUtauVoiceConfig_SizeChanged( Object sender, BEventArgs e ) {
            if ( this.WindowState == FormWindowState.Normal ) {
                m_current_bounds = new Rectangle( this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height );
            }
        }

        private void menuEditGenerateSTF_Click( Object sender, BEventArgs e ) {
            generateSTForFRQ( FormGenerateStf.GenerateMode.STF );
        }

        private void checkSTFExistence() {
            int count = listFiles.Items.Count;
            if ( m_oto_ini.Equals( "" ) ) {
                return;
            }
            String analyzed = PortUtil.combinePath( PortUtil.getDirectoryName( m_oto_ini ), "analyzed" );
            for ( int i = 0; i < count; i++ ) {
                ListViewItem item = listFiles.Items[i];
                String wav_name = item.SubItems[0].Text;
                String stf_path = PortUtil.combinePath( analyzed, PortUtil.getFileNameWithoutExtension( wav_name ) + ".stf" );
                item.SubItems[8].Text = PortUtil.isFileExists( stf_path ) ? "○" : "";
            }
        }

        private void checkFRQExistence() {
            int count = listFiles.Items.Count;
            if ( m_oto_ini.Equals( "" ) ) {
                return;
            }
            String dir = PortUtil.getDirectoryName( m_oto_ini );
            for ( int i = 0; i < count; i++ ) {
                ListViewItem item = listFiles.Items[i];
                String wav_name = item.SubItems[0].Text;
                String frq_path = PortUtil.combinePath( dir, wav_name.Replace( ".", "_" ) + ".frq" );
                item.SubItems[7].Text = PortUtil.isFileExists( frq_path ) ? "○" : "";
            }
        }

        private void btnRefreshStf_Click( Object sender, BEventArgs e ) {
            if ( m_file.Equals( "" ) ) {
                return;
            }
            if ( m_oto_ini.Equals( "" ) ) {
                return;
            }
            String dir = PortUtil.getDirectoryName( m_oto_ini );
            String analyzed = PortUtil.combinePath( dir, "analyzed" );
            String wav_path = PortUtil.combinePath( dir, m_file );
            if ( !PortUtil.isFileExists( wav_path ) ) {
                return;
            }
            String stf_path = PortUtil.combinePath( analyzed, PortUtil.getFileNameWithoutExtension( m_file ) + ".stf" );
            if ( PortUtil.isFileExists( stf_path ) ) {
                try {
                    PortUtil.deleteFile( stf_path );
                } catch ( Exception ex ) {
                }
            }
            StfQueueArgs queue = new StfQueueArgs();
            queue.waveName = m_file;
            queue.offset = m_offset + "";
            queue.blank = m_blank + "";
            Vector<StfQueueArgs> list = new Vector<StfQueueArgs>();
            list.add( queue );
            FormGenerateStf form = new FormGenerateStf( m_oto_ini, list, FormGenerateStf.GenerateMode.STF );
            form.showDialog();
            form = null;
            checkSTFExistence();
            setEdited( true );
        }

        private void btnRefreshFrq_Click( Object sender, BEventArgs e ) {
            if ( m_file.Equals( "" ) ) {
                return;
            }
            if ( m_oto_ini.Equals( "" ) ) {
                return;
            }
            String dir = PortUtil.getDirectoryName( m_oto_ini );
            String wav_path = PortUtil.combinePath( dir, m_file );
            if ( !PortUtil.isFileExists( wav_path ) ) {
                return;
            }
            String frq_path = PortUtil.combinePath( dir, m_file.Replace( ".", "_" ) + ".frq" );
            if ( PortUtil.isFileExists( frq_path ) ) {
                try {
                    PortUtil.deleteFile( frq_path );
                } catch ( Exception ex ) {
                }
            }
            StfQueueArgs queue = new StfQueueArgs();
            queue.waveName = m_file;
            queue.offset = m_offset + "";
            queue.blank = m_blank + "";
            Vector<StfQueueArgs> list = new Vector<StfQueueArgs>();
            list.add( queue );
            FormGenerateStf form = new FormGenerateStf( m_oto_ini, list, FormGenerateStf.GenerateMode.FRQ );
            form.showDialog();
            checkFRQExistence();
            setEdited( true );
        }

        private void menuEditGenerateFRQ_Click( Object sender, EventArgs e ) {
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
            setEdited( true );
        }

        private void bgWorkScreen_DoWork( Object sender, BDoWorkEventArgs e ) {
            this.Invoke( new BEventHandler( this.refreshScreenCore ) );
        }

        private void refreshScreenCore( Object sender, BEventArgs e ) {
            pictWave.Invalidate();
        }

        private void txtSearch_TextChanged( Object sender, BEventArgs e ) {
            searchCor( false );
        }

        private void buttonNext_Click( Object sender, BEventArgs e ) {
            searchCor( false );
        }

        private void buttonPrevious_Click( Object sender, EventArgs e ) {
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
                txtSearch.BackColor = System.Drawing.SystemColors.Window;
                txtSearch.ForeColor = System.Drawing.SystemColors.WindowText;
            } else {
                txtSearch.BackColor = System.Drawing.Color.LightCoral;
                txtSearch.ForeColor = System.Drawing.Color.White;
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

        private void menuViewSearchNext_Click( Object sender, EventArgs e ) {
            searchCor( false );
        }

        private void menuViewSearchPrevious_Click( Object sender, EventArgs e ) {
            searchCor( true );
        }
#if JAVA
        #region UI Impl for Java
	    private JPanel splitContainerOut = null;
	    private JMenuBar jJMenuBar = null;
	    private JMenu menuFile = null;
	    private JMenuItem menuFileOpen = null;
	    private JMenuItem menuFileSave = null;
	    private JMenuItem menuFileSaveAs = null;
	    private JSeparator jMenuItem3 = null;
	    private JMenuItem menuFileQuit = null;
	    private JSplitPane jSplitPane = null;
	    private JSplitPane splitContainerIn = null;
	    private JPanel panelLeft = null;
	    private JScrollPane jScrollPane = null;
	    private JTable jTable = null;
	    private JPanel panelLeftBottom = null;
	    private JButton buttonNext = null;
	    private JLabel lblSearch = null;
	    private JTextField txtSearch = null;
	    private JButton buttonPrevious = null;
	    private JLabel jLabel1 = null;
	    private JPanel panelRight = null;
	    private JLabel lblFileName = null;
	    private JTextField txtFileName = null;
	    private JLabel lblAlias = null;
	    private JTextField txtAlias = null;
	    private JLabel lblOffset = null;
	    private JTextField txtOffset = null;
	    private JLabel lblConsonant = null;
	    private JTextField txtConsonant = null;
	    private JLabel lblBlank = null;
	    private JLabel lblPreUtterance = null;
	    private JLabel lblOverlap = null;
	    private JTextField txtBlank = null;
	    private JTextField txtPreUtterance = null;
	    private JTextField txtOverlap = null;
	    private JButton btnRefreshFrq = null;
	    private JButton btnRefreshStf = null;
	    private JLabel jLabel3 = null;
	    private JLabel jLabel4 = null;
	    private JPanel panelBottom = null;
	    private JPanel pictWave = null;
	    private JPanel jPanel5 = null;
	    private JScrollBar hScroll = null;
	    private JButton btnMinus = null;
	    private JButton btnPlus = null;
	    private JMenu menuEdit = null;
	    private JMenuItem menuEditGenerateFrq = null;
	    private JMenuItem menuEditGenerateStf = null;
	    private JMenu menuView = null;
	    private JMenuItem menuViewSearchNext = null;
	    private JMenuItem menuViewSearchPrevious = null;
	    private DefaultTableModel listFiles = null;
	    private String[] columnHeaders = new String[]{ "FIle Name", "Alias", "offste", "Consonant", "Blank", "pre Utrerance", "Overlap" };
	    private JLabel jLabel = null;

	    /**
	     * This method initializes this
	     * 
	     * @return void
	     */
	    private void initialize() {
		    this.setSize(712, 507);
		    this.setJMenuBar(getJJMenuBar());
		    this.setContentPane(getSplitContainerOut());
		    this.setTitle("JFrame");
	    }

	    /**
	     * This method initializes splitContainerOut	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getSplitContainerOut() {
		    if (splitContainerOut == null) {
			    splitContainerOut = new JPanel();
			    splitContainerOut.setLayout(new BorderLayout());
			    splitContainerOut.add(getJSplitPane(), BorderLayout.CENTER);
		    }
		    return splitContainerOut;
	    }

	    /**
	     * This method initializes jJMenuBar	
	     * 	
	     * @return javax.swing.JMenuBar	
	     */
	    private JMenuBar getJJMenuBar() {
		    if (jJMenuBar == null) {
			    jJMenuBar = new JMenuBar();
			    jJMenuBar.add(getMenuFile());
			    jJMenuBar.add(getMenuEdit());
			    jJMenuBar.add(getMenuView());
		    }
		    return jJMenuBar;
	    }

	    /**
	     * This method initializes menuFile	
	     * 	
	     * @return javax.swing.JMenu	
	     */
	    private JMenu getMenuFile() {
		    if (menuFile == null) {
			    menuFile = new JMenu();
			    menuFile.setText("File");
			    menuFile.add(getMenuFileOpen());
			    menuFile.add(getMenuFileSave());
			    menuFile.add(getMenuFileSaveAs());
			    menuFile.add(getJMenuItem3());
			    menuFile.add(getMenuFileQuit());
		    }
		    return menuFile;
	    }

	    /**
	     * This method initializes menuFileOpen	
	     * 	
	     * @return javax.swing.JMenuItem	
	     */
	    private JMenuItem getMenuFileOpen() {
		    if (menuFileOpen == null) {
			    menuFileOpen = new JMenuItem();
			    menuFileOpen.setText("Open");
		    }
		    return menuFileOpen;
	    }

	    /**
	     * This method initializes menuFileSave	
	     * 	
	     * @return javax.swing.JMenuItem	
	     */
	    private JMenuItem getMenuFileSave() {
		    if (menuFileSave == null) {
			    menuFileSave = new JMenuItem();
			    menuFileSave.setText("Save");
		    }
		    return menuFileSave;
	    }

	    /**
	     * This method initializes menuFileSaveAs	
	     * 	
	     * @return javax.swing.JMenuItem	
	     */
	    private JMenuItem getMenuFileSaveAs() {
		    if (menuFileSaveAs == null) {
			    menuFileSaveAs = new JMenuItem();
			    menuFileSaveAs.setText("Save As(&A)");
		    }
		    return menuFileSaveAs;
	    }

	    /**
	     * This method initializes jMenuItem3	
	     * 	
	     * @return javax.swing.JMenuItem	
	     */
	    private JSeparator getJMenuItem3() {
		    if (jMenuItem3 == null) {
			    jMenuItem3 = new JSeparator();
			    jMenuItem3.setBorder(null);
		    }
		    return jMenuItem3;
	    }

	    /**
	     * This method initializes menuFileQuit	
	     * 	
	     * @return javax.swing.JMenuItem	
	     */
	    private JMenuItem getMenuFileQuit() {
		    if (menuFileQuit == null) {
			    menuFileQuit = new JMenuItem();
			    menuFileQuit.setText("Close");
		    }
		    return menuFileQuit;
	    }

	    /**
	     * This method initializes jSplitPane	
	     * 	
	     * @return javax.swing.JSplitPane	
	     */
	    private JSplitPane getJSplitPane() {
		    if (jSplitPane == null) {
			    jSplitPane = new JSplitPane();
			    jSplitPane.setOrientation(JSplitPane.VERTICAL_SPLIT);
			    jSplitPane.setTopComponent(getSplitContainerIn());
			    jSplitPane.setBottomComponent(getPanelBottom());
			    jSplitPane.setDividerLocation(260);
		    }
		    return jSplitPane;
	    }

	    /**
	     * This method initializes splitContainerIn	
	     * 	
	     * @return javax.swing.JSplitPane	
	     */
	    private JSplitPane getSplitContainerIn() {
		    if (splitContainerIn == null) {
			    splitContainerIn = new JSplitPane();
			    splitContainerIn.setDividerLocation(450);
			    splitContainerIn.setRightComponent(getPanelRight());
			    splitContainerIn.setLeftComponent(getPanelLeft());
		    }
		    return splitContainerIn;
	    }

	    /**
	     * This method initializes panelLeft	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getPanelLeft() {
		    if (panelLeft == null) {
			    GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			    gridBagConstraints1.gridx = 0;
			    gridBagConstraints1.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints1.gridy = 1;
			    GridBagConstraints gridBagConstraints = new GridBagConstraints();
			    gridBagConstraints.fill = GridBagConstraints.BOTH;
			    gridBagConstraints.gridy = 0;
			    gridBagConstraints.weightx = 1.0;
			    gridBagConstraints.weighty = 1.0D;
			    gridBagConstraints.gridheight = 1;
			    gridBagConstraints.gridx = 0;
			    panelLeft = new JPanel();
			    panelLeft.setLayout(new GridBagLayout());
			    panelLeft.add(getJScrollPane(), gridBagConstraints);
			    panelLeft.add(getPanelLeftBottom(), gridBagConstraints1);
		    }
		    return panelLeft;
	    }

	    /**
	     * This method initializes jScrollPane	
	     * 	
	     * @return javax.swing.JScrollPane	
	     */
	    private JScrollPane getJScrollPane() {
		    if (jScrollPane == null) {
			    jScrollPane = new JScrollPane();
			    jScrollPane.setViewportView(getListFiles());
		    }
		    return jScrollPane;
	    }

	    /**
	     * This method initializes listFiles	
	     * 	
	     * @return javax.swing.JTable	
	     */
	    private JTable getListFiles() {
		    if (jTable == null) {
			    jTable = new JTable();
			    jTable.setModel( getTableModel() );
		    }
		    return jTable;
	    }

	    private DefaultTableModel getTableModel(){
		    if( listFiles == null){
			    listFiles = new DefaultTableModel( columnHeaders, 0  );
		    }
		    return listFiles;
	    }
    	
	    /**
	     * This method initializes panelLeftBottom	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getPanelLeftBottom() {
		    if (panelLeftBottom == null) {
			    GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			    gridBagConstraints6.gridx = 4;
			    gridBagConstraints6.weightx = 1.0D;
			    gridBagConstraints6.gridy = 0;
			    jLabel1 = new JLabel();
			    jLabel1.setText("");
			    GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			    gridBagConstraints5.gridx = 3;
			    gridBagConstraints5.insets = new Insets(2, 8, 2, 0);
			    gridBagConstraints5.gridy = 0;
			    GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			    gridBagConstraints4.fill = GridBagConstraints.NONE;
			    gridBagConstraints4.gridy = 0;
			    gridBagConstraints4.weightx = 0.0D;
			    gridBagConstraints4.insets = new Insets(2, 8, 2, 0);
			    gridBagConstraints4.gridx = 1;
			    GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			    gridBagConstraints3.gridx = 2;
			    gridBagConstraints3.insets = new Insets(2, 8, 2, 0);
			    GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			    gridBagConstraints2.gridx = 0;
			    gridBagConstraints2.insets = new Insets(2, 16, 2, 0);
			    gridBagConstraints2.gridy = 0;
			    lblSearch = new JLabel();
			    lblSearch.setText("Search:");
			    lblSearch.setHorizontalAlignment(SwingConstants.LEFT);
			    panelLeftBottom = new JPanel();
			    panelLeftBottom.setLayout(new GridBagLayout());
			    panelLeftBottom.add(getButtonNext(), gridBagConstraints3);
			    panelLeftBottom.add(lblSearch, gridBagConstraints2);
			    panelLeftBottom.add(getTxtSearch(), gridBagConstraints4);
			    panelLeftBottom.add(getButtonPrevious(), gridBagConstraints5);
			    panelLeftBottom.add(jLabel1, gridBagConstraints6);
		    }
		    return panelLeftBottom;
	    }

	    /**
	     * This method initializes buttonNext	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getButtonNext() {
		    if (buttonNext == null) {
			    buttonNext = new JButton();
			    buttonNext.setText("Next");
			    buttonNext.setHorizontalAlignment(SwingConstants.LEFT);
		    }
		    return buttonNext;
	    }

	    /**
	     * This method initializes txtSearch	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtSearch() {
		    if (txtSearch == null) {
			    txtSearch = new JTextField();
			    txtSearch.setPreferredSize(new Dimension(100, 20));
			    txtSearch.setHorizontalAlignment(JTextField.LEFT);
		    }
		    return txtSearch;
	    }

	    /**
	     * This method initializes buttonPrevious	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getButtonPrevious() {
		    if (buttonPrevious == null) {
			    buttonPrevious = new JButton();
			    buttonPrevious.setText("Previous");
			    buttonPrevious.setHorizontalAlignment(SwingConstants.LEFT);
		    }
		    return buttonPrevious;
	    }

	    /**
	     * This method initializes panelRight	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getPanelRight() {
		    if (panelRight == null) {
			    GridBagConstraints gridBagConstraints28 = new GridBagConstraints();
			    gridBagConstraints28.gridx = 2;
			    gridBagConstraints28.anchor = GridBagConstraints.WEST;
			    gridBagConstraints28.insets = new Insets(0, 0, 0, 16);
			    gridBagConstraints28.gridy = 3;
			    jLabel = new JLabel();
			    jLabel.setText("ms");
			    GridBagConstraints gridBagConstraints24 = new GridBagConstraints();
			    gridBagConstraints24.gridx = 0;
			    gridBagConstraints24.insets = new Insets(16, 0, 0, 0);
			    gridBagConstraints24.gridy = 0;
			    jLabel4 = new JLabel();
			    jLabel4.setText("");
			    GridBagConstraints gridBagConstraints23 = new GridBagConstraints();
			    gridBagConstraints23.gridx = 0;
			    gridBagConstraints23.weighty = 1.0D;
			    gridBagConstraints23.gridy = 10;
			    jLabel3 = new JLabel();
			    jLabel3.setText("");
			    GridBagConstraints gridBagConstraints22 = new GridBagConstraints();
			    gridBagConstraints22.gridx = 1;
			    gridBagConstraints22.anchor = GridBagConstraints.WEST;
			    gridBagConstraints22.insets = new Insets(2, 16, 2, 0);
			    gridBagConstraints22.gridwidth = 2;
			    gridBagConstraints22.gridy = 9;
			    GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
			    gridBagConstraints21.gridx = 1;
			    gridBagConstraints21.anchor = GridBagConstraints.WEST;
			    gridBagConstraints21.insets = new Insets(2, 16, 2, 0);
			    gridBagConstraints21.gridwidth = 2;
			    gridBagConstraints21.gridy = 8;
			    GridBagConstraints gridBagConstraints20 = new GridBagConstraints();
			    gridBagConstraints20.fill = GridBagConstraints.NONE;
			    gridBagConstraints20.gridy = 7;
			    gridBagConstraints20.weightx = 1.0;
			    gridBagConstraints20.insets = new Insets(2, 16, 2, 0);
			    gridBagConstraints20.anchor = GridBagConstraints.WEST;
			    gridBagConstraints20.gridx = 1;
			    GridBagConstraints gridBagConstraints19 = new GridBagConstraints();
			    gridBagConstraints19.fill = GridBagConstraints.NONE;
			    gridBagConstraints19.gridy = 6;
			    gridBagConstraints19.weightx = 1.0;
			    gridBagConstraints19.insets = new Insets(2, 16, 2, 0);
			    gridBagConstraints19.anchor = GridBagConstraints.WEST;
			    gridBagConstraints19.gridx = 1;
			    GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
			    gridBagConstraints18.fill = GridBagConstraints.NONE;
			    gridBagConstraints18.gridy = 5;
			    gridBagConstraints18.weightx = 1.0;
			    gridBagConstraints18.insets = new Insets(2, 16, 2, 0);
			    gridBagConstraints18.anchor = GridBagConstraints.WEST;
			    gridBagConstraints18.gridx = 1;
			    GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
			    gridBagConstraints17.gridx = 0;
			    gridBagConstraints17.anchor = GridBagConstraints.WEST;
			    gridBagConstraints17.insets = new Insets(0, 16, 0, 0);
			    gridBagConstraints17.gridy = 7;
			    lblOverlap = new JLabel();
			    lblOverlap.setText("Overlap");
			    GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
			    gridBagConstraints16.gridx = 0;
			    gridBagConstraints16.anchor = GridBagConstraints.WEST;
			    gridBagConstraints16.insets = new Insets(0, 16, 0, 0);
			    gridBagConstraints16.gridy = 6;
			    lblPreUtterance = new JLabel();
			    lblPreUtterance.setText("Pre Utterance");
			    GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
			    gridBagConstraints15.gridx = 0;
			    gridBagConstraints15.anchor = GridBagConstraints.WEST;
			    gridBagConstraints15.insets = new Insets(0, 16, 0, 0);
			    gridBagConstraints15.gridy = 5;
			    lblBlank = new JLabel();
			    lblBlank.setText("Blank");
			    GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
			    gridBagConstraints14.fill = GridBagConstraints.NONE;
			    gridBagConstraints14.gridy = 4;
			    gridBagConstraints14.weightx = 1.0;
			    gridBagConstraints14.insets = new Insets(2, 16, 2, 0);
			    gridBagConstraints14.anchor = GridBagConstraints.WEST;
			    gridBagConstraints14.gridx = 1;
			    GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			    gridBagConstraints13.gridx = 0;
			    gridBagConstraints13.anchor = GridBagConstraints.WEST;
			    gridBagConstraints13.insets = new Insets(0, 16, 0, 0);
			    gridBagConstraints13.gridy = 4;
			    lblConsonant = new JLabel();
			    lblConsonant.setText("Consonant");
			    GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			    gridBagConstraints12.fill = GridBagConstraints.NONE;
			    gridBagConstraints12.gridy = 3;
			    gridBagConstraints12.weightx = 1.0;
			    gridBagConstraints12.insets = new Insets(2, 16, 2, 0);
			    gridBagConstraints12.anchor = GridBagConstraints.WEST;
			    gridBagConstraints12.gridwidth = 1;
			    gridBagConstraints12.gridx = 1;
			    GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
			    gridBagConstraints11.gridx = 0;
			    gridBagConstraints11.anchor = GridBagConstraints.WEST;
			    gridBagConstraints11.insets = new Insets(0, 16, 0, 0);
			    gridBagConstraints11.gridy = 3;
			    lblOffset = new JLabel();
			    lblOffset.setText("Offset");
			    GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
			    gridBagConstraints10.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints10.gridy = 2;
			    gridBagConstraints10.weightx = 1.0;
			    gridBagConstraints10.insets = new Insets(2, 16, 2, 16);
			    gridBagConstraints10.anchor = GridBagConstraints.WEST;
			    gridBagConstraints10.gridwidth = 2;
			    gridBagConstraints10.gridx = 1;
			    GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
			    gridBagConstraints9.gridx = 0;
			    gridBagConstraints9.insets = new Insets(0, 16, 0, 0);
			    gridBagConstraints9.anchor = GridBagConstraints.WEST;
			    gridBagConstraints9.gridy = 2;
			    lblAlias = new JLabel();
			    lblAlias.setText("Alias");
			    GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
			    gridBagConstraints8.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints8.gridy = 1;
			    gridBagConstraints8.weightx = 1.0D;
			    gridBagConstraints8.anchor = GridBagConstraints.WEST;
			    gridBagConstraints8.insets = new Insets(2, 16, 2, 16);
			    gridBagConstraints8.gridwidth = 2;
			    gridBagConstraints8.gridx = 1;
			    GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			    gridBagConstraints7.gridx = 0;
			    gridBagConstraints7.insets = new Insets(0, 16, 0, 0);
			    gridBagConstraints7.anchor = GridBagConstraints.WEST;
			    gridBagConstraints7.gridy = 1;
			    lblFileName = new JLabel();
			    lblFileName.setText("File Name");
			    panelRight = new JPanel();
			    panelRight.setLayout(new GridBagLayout());
			    panelRight.add(lblFileName, gridBagConstraints7);
			    panelRight.add(getTxtFileName(), gridBagConstraints8);
			    panelRight.add(lblAlias, gridBagConstraints9);
			    panelRight.add(getTxtAlias(), gridBagConstraints10);
			    panelRight.add(lblOffset, gridBagConstraints11);
			    panelRight.add(getTxtOffset(), gridBagConstraints12);
			    panelRight.add(lblConsonant, gridBagConstraints13);
			    panelRight.add(getTxtConsonant(), gridBagConstraints14);
			    panelRight.add(lblBlank, gridBagConstraints15);
			    panelRight.add(lblPreUtterance, gridBagConstraints16);
			    panelRight.add(lblOverlap, gridBagConstraints17);
			    panelRight.add(getTxtBlank(), gridBagConstraints18);
			    panelRight.add(getTxtPreUtterance(), gridBagConstraints19);
			    panelRight.add(getTxtOverlap(), gridBagConstraints20);
			    panelRight.add(getBtnRefreshFrq(), gridBagConstraints21);
			    panelRight.add(getBtnRefreshStf(), gridBagConstraints22);
			    panelRight.add(jLabel3, gridBagConstraints23);
			    panelRight.add(jLabel4, gridBagConstraints24);
			    panelRight.add(jLabel, gridBagConstraints28);
		    }
		    return panelRight;
	    }

	    /**
	     * This method initializes txtFileName	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtFileName() {
		    if (txtFileName == null) {
			    txtFileName = new JTextField();
			    txtFileName.setPreferredSize(new Dimension(100, 20));
		    }
		    return txtFileName;
	    }

	    /**
	     * This method initializes txtAlias	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtAlias() {
		    if (txtAlias == null) {
			    txtAlias = new JTextField();
			    txtAlias.setPreferredSize(new Dimension(100, 20));
		    }
		    return txtAlias;
	    }

	    /**
	     * This method initializes txtOffset	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtOffset() {
		    if (txtOffset == null) {
			    txtOffset = new JTextField();
			    txtOffset.setPreferredSize(new Dimension(59, 20));
		    }
		    return txtOffset;
	    }

	    /**
	     * This method initializes txtConsonant	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtConsonant() {
		    if (txtConsonant == null) {
			    txtConsonant = new JTextField();
			    txtConsonant.setPreferredSize(new Dimension(59, 20));
		    }
		    return txtConsonant;
	    }

	    /**
	     * This method initializes txtBlank	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtBlank() {
		    if (txtBlank == null) {
			    txtBlank = new JTextField();
			    txtBlank.setPreferredSize(new Dimension(59, 20));
		    }
		    return txtBlank;
	    }

	    /**
	     * This method initializes txtPreUtterance	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtPreUtterance() {
		    if (txtPreUtterance == null) {
			    txtPreUtterance = new JTextField();
			    txtPreUtterance.setPreferredSize(new Dimension(59, 20));
		    }
		    return txtPreUtterance;
	    }

	    /**
	     * This method initializes txtOverlap	
	     * 	
	     * @return javax.swing.JTextField	
	     */
	    private JTextField getTxtOverlap() {
		    if (txtOverlap == null) {
			    txtOverlap = new JTextField();
			    txtOverlap.setPreferredSize(new Dimension(59, 20));
		    }
		    return txtOverlap;
	    }

	    /**
	     * This method initializes btnRefreshFrq	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnRefreshFrq() {
		    if (btnRefreshFrq == null) {
			    btnRefreshFrq = new JButton();
			    btnRefreshFrq.setText("Refresh FRQ");
		    }
		    return btnRefreshFrq;
	    }

	    /**
	     * This method initializes btnRefreshStf	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnRefreshStf() {
		    if (btnRefreshStf == null) {
			    btnRefreshStf = new JButton();
			    btnRefreshStf.setText("Refresh STF");
		    }
		    return btnRefreshStf;
	    }

	    /**
	     * This method initializes panelBottom	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getPanelBottom() {
		    if (panelBottom == null) {
			    GridBagConstraints gridBagConstraints27 = new GridBagConstraints();
			    gridBagConstraints27.gridx = 0;
			    gridBagConstraints27.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints27.weightx = 1.0D;
			    gridBagConstraints27.anchor = GridBagConstraints.SOUTH;
			    gridBagConstraints27.gridy = 1;
			    GridBagConstraints gridBagConstraints25 = new GridBagConstraints();
			    gridBagConstraints25.gridx = 0;
			    gridBagConstraints25.weightx = 1.0D;
			    gridBagConstraints25.weighty = 1.0D;
			    gridBagConstraints25.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints25.anchor = GridBagConstraints.NORTH;
			    gridBagConstraints25.gridy = 0;
			    panelBottom = new JPanel();
			    panelBottom.setLayout(new GridBagLayout());
			    panelBottom.add(getPictWave(), gridBagConstraints25);
			    panelBottom.add(getJPanel5(), gridBagConstraints27);
		    }
		    return panelBottom;
	    }

	    /**
	     * This method initializes pictWave	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getPictWave() {
		    if (pictWave == null) {
			    pictWave = new JPanel();
			    pictWave.setLayout(new GridBagLayout());
		    }
		    return pictWave;
	    }

	    /**
	     * This method initializes jPanel5	
	     * 	
	     * @return javax.swing.JPanel	
	     */
	    private JPanel getJPanel5() {
		    if (jPanel5 == null) {
			    GridBagConstraints gridBagConstraints26 = new GridBagConstraints();
			    gridBagConstraints26.fill = GridBagConstraints.HORIZONTAL;
			    gridBagConstraints26.gridx = 0;
			    gridBagConstraints26.gridy = 0;
			    gridBagConstraints26.weightx = 1.0D;
			    gridBagConstraints26.weighty = 1.0;
			    jPanel5 = new JPanel();
			    jPanel5.setLayout(new GridBagLayout());
			    jPanel5.add(getHScroll(), gridBagConstraints26);
			    jPanel5.add(getBtnMinus(), new GridBagConstraints());
			    jPanel5.add(getBtnPlus(), new GridBagConstraints());
		    }
		    return jPanel5;
	    }

	    /**
	     * This method initializes hScroll	
	     * 	
	     * @return javax.swing.JScrollBar	
	     */
	    private JScrollBar getHScroll() {
		    if (hScroll == null) {
			    hScroll = new JScrollBar();
			    hScroll.setOrientation(JScrollBar.HORIZONTAL);
		    }
		    return hScroll;
	    }

	    /**
	     * This method initializes btnMinus	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnMinus() {
		    if (btnMinus == null) {
			    btnMinus = new JButton();
			    btnMinus.setText("-");
			    btnMinus.setPreferredSize(new Dimension(38, 16));
		    }
		    return btnMinus;
	    }

	    /**
	     * This method initializes btnPlus	
	     * 	
	     * @return javax.swing.JButton	
	     */
	    private JButton getBtnPlus() {
		    if (btnPlus == null) {
			    btnPlus = new JButton();
			    btnPlus.setText("+");
			    btnPlus.setPreferredSize(new Dimension(41, 16));
		    }
		    return btnPlus;
	    }

	    /**
	     * This method initializes menuEdit	
	     * 	
	     * @return javax.swing.JMenu	
	     */
	    private JMenu getMenuEdit() {
		    if (menuEdit == null) {
			    menuEdit = new JMenu();
			    menuEdit.setText("Edit");
			    menuEdit.add(getMenuEditGenerateFrq());
			    menuEdit.add(getMenuEditGenerateStf());
		    }
		    return menuEdit;
	    }

	    /**
	     * This method initializes menuEditGenerateFrq	
	     * 	
	     * @return javax.swing.JMenuItem	
	     */
	    private JMenuItem getMenuEditGenerateFrq() {
		    if (menuEditGenerateFrq == null) {
			    menuEditGenerateFrq = new JMenuItem();
			    menuEditGenerateFrq.setText("Generate FRQ files");
		    }
		    return menuEditGenerateFrq;
	    }

	    /**
	     * This method initializes menuEditGenerateStf	
	     * 	
	     * @return javax.swing.JMenuItem	
	     */
	    private JMenuItem getMenuEditGenerateStf() {
		    if (menuEditGenerateStf == null) {
			    menuEditGenerateStf = new JMenuItem();
			    menuEditGenerateStf.setText("Generate STF files");
		    }
		    return menuEditGenerateStf;
	    }

	    /**
	     * This method initializes menuView	
	     * 	
	     * @return javax.swing.JMenu	
	     */
	    private JMenu getMenuView() {
		    if (menuView == null) {
			    menuView = new JMenu();
			    menuView.setText("View");
			    menuView.add(getMenuViewSearchNext());
			    menuView.add(getMenuViewSearchPrevious());
		    }
		    return menuView;
	    }

	    /**
	     * This method initializes menuViewSearchNext	
	     * 	
	     * @return javax.swing.JMenuItem	
	     */
	    private JMenuItem getMenuViewSearchNext() {
		    if (menuViewSearchNext == null) {
			    menuViewSearchNext = new JMenuItem();
			    menuViewSearchNext.setText("Search Next");
		    }
		    return menuViewSearchNext;
	    }

	    /**
	     * This method initializes menuViewSearchPrevious	
	     * 	
	     * @return javax.swing.JMenuItem	
	     */
	    private JMenuItem getMenuViewSearchPrevious() {
		    if (menuViewSearchPrevious == null) {
			    menuViewSearchPrevious = new JMenuItem();
			    menuViewSearchPrevious.setText("Search Previous");
		    }
		    return menuViewSearchPrevious;
	    }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( FormUtauVoiceConfig ) );
            this.listFiles = new System.Windows.Forms.ListView();
            this.columnHeaderFilename = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAlias = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderOffset = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderConsonant = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderBlank = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderPreUtterance = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderOverlap = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderFrq = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderStf = new System.Windows.Forms.ColumnHeader();
            this.menuStrip = new BMenuBar();
            this.menuFile = new BMenuItem();
            this.menuFileOpen = new BMenuItem();
            this.menuFileSave = new BMenuItem();
            this.menuFileSaveAs = new BMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileQuit = new BMenuItem();
            this.menuEdit = new BMenuItem();
            this.menuEditGenerateFRQ = new BMenuItem();
            this.menuEditGenerateSTF = new BMenuItem();
            this.menuView = new BMenuItem();
            this.menuViewSearchNext = new BMenuItem();
            this.menuViewSearchPrevious = new BMenuItem();
            this.panelRight = new System.Windows.Forms.Panel();
            this.btnRefreshFrq = new BButton();
            this.btnRefreshStf = new BButton();
            this.label9 = new BLabel();
            this.txtOverlap = new Boare.Cadencii.NumberTextBox();
            this.lblOverlap = new BLabel();
            this.label7 = new BLabel();
            this.txtPreUtterance = new Boare.Cadencii.NumberTextBox();
            this.lblPreUtterance = new BLabel();
            this.label5 = new BLabel();
            this.txtBlank = new Boare.Cadencii.NumberTextBox();
            this.lblBlank = new BLabel();
            this.label3 = new BLabel();
            this.txtConsonant = new Boare.Cadencii.NumberTextBox();
            this.lblConsonant = new BLabel();
            this.label2 = new BLabel();
            this.txtOffset = new Boare.Cadencii.NumberTextBox();
            this.lblOffset = new BLabel();
            this.txtAlias = new BTextBox();
            this.lblAlias = new BLabel();
            this.txtFileName = new BTextBox();
            this.lblFileName = new BLabel();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnMinus = new BButton();
            this.btnPlus = new BButton();
            this.hScroll = new System.Windows.Forms.HScrollBar();
            this.pictWave = new BPictureBox();
            this.bgWorkRead = new System.ComponentModel.BackgroundWorker();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLblTootip = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerIn = new Boare.Lib.AppUtil.BSplitContainer();
            this.splitContainerOut = new Boare.Lib.AppUtil.BSplitContainer();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.cmenuListFiles = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.generateSTRAIGHTFileToolStripMenuItem = new BMenuItem();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.buttonPrevious = new BButton();
            this.buttonNext = new BButton();
            this.lblSearch = new BLabel();
            this.txtSearch = new BTextBox();
            this.bgWorkScreen = new System.ComponentModel.BackgroundWorker();
            this.menuStrip.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictWave)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.cmenuListFiles.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // listFiles
            // 
            this.listFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listFiles.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFilename,
            this.columnHeaderAlias,
            this.columnHeaderOffset,
            this.columnHeaderConsonant,
            this.columnHeaderBlank,
            this.columnHeaderPreUtterance,
            this.columnHeaderOverlap,
            this.columnHeaderFrq,
            this.columnHeaderStf} );
            this.listFiles.FullRowSelect = true;
            this.listFiles.HideSelection = false;
            this.listFiles.Location = new System.Drawing.Point( 0, 0 );
            this.listFiles.MultiSelect = false;
            this.listFiles.Name = "listFiles";
            this.listFiles.Size = new System.Drawing.Size( 454, 242 );
            this.listFiles.TabIndex = 0;
            this.listFiles.UseCompatibleStateImageBehavior = false;
            this.listFiles.View = System.Windows.Forms.View.Details;
            this.listFiles.SelectedIndexChanged += new System.EventHandler( this.listFiles_SelectedIndexChanged );
            // 
            // columnHeaderFilename
            // 
            this.columnHeaderFilename.Text = "File Name";
            this.columnHeaderFilename.Width = 75;
            // 
            // columnHeaderAlias
            // 
            this.columnHeaderAlias.Text = "Alias";
            this.columnHeaderAlias.Width = 42;
            // 
            // columnHeaderOffset
            // 
            this.columnHeaderOffset.Text = "Offset";
            this.columnHeaderOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderOffset.Width = 50;
            // 
            // columnHeaderConsonant
            // 
            this.columnHeaderConsonant.Text = "Consonant";
            this.columnHeaderConsonant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderConsonant.Width = 72;
            // 
            // columnHeaderBlank
            // 
            this.columnHeaderBlank.Text = "Blank";
            this.columnHeaderBlank.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderBlank.Width = 51;
            // 
            // columnHeaderPreUtterance
            // 
            this.columnHeaderPreUtterance.Text = "Pre Utterance";
            this.columnHeaderPreUtterance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderPreUtterance.Width = 92;
            // 
            // columnHeaderOverlap
            // 
            this.columnHeaderOverlap.Text = "Overlap";
            this.columnHeaderOverlap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderOverlap.Width = 61;
            // 
            // columnHeaderFrq
            // 
            this.columnHeaderFrq.Text = "FRQ";
            this.columnHeaderFrq.Width = 51;
            // 
            // columnHeaderStf
            // 
            this.columnHeaderStf.Text = "STF";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuView} );
            this.menuStrip.Location = new System.Drawing.Point( 0, 0 );
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size( 772, 24 );
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuFileOpen,
            this.menuFileSave,
            this.menuFileSaveAs,
            this.toolStripMenuItem1,
            this.menuFileQuit} );
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size( 51, 20 );
            this.menuFile.Text = "File(&F)";
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.Name = "menuFileOpen";
            this.menuFileOpen.Size = new System.Drawing.Size( 129, 22 );
            this.menuFileOpen.Text = "Open(&O)";
            this.menuFileOpen.MouseEnter += new System.EventHandler( this.menuFileOpen_MouseEnter );
            this.menuFileOpen.Click += new System.EventHandler( this.menuFileOpen_Click );
            // 
            // menuFileSave
            // 
            this.menuFileSave.Name = "menuFileSave";
            this.menuFileSave.Size = new System.Drawing.Size( 129, 22 );
            this.menuFileSave.Text = "Save(&S)";
            this.menuFileSave.MouseEnter += new System.EventHandler( this.menuFileSave_MouseEnter );
            this.menuFileSave.Click += new System.EventHandler( this.menuFileSave_Click );
            // 
            // menuFileSaveAs
            // 
            this.menuFileSaveAs.Name = "menuFileSaveAs";
            this.menuFileSaveAs.Size = new System.Drawing.Size( 129, 22 );
            this.menuFileSaveAs.Text = "Save As(&A)";
            this.menuFileSaveAs.MouseEnter += new System.EventHandler( this.menuFileSaveAs_MouseEnter );
            this.menuFileSaveAs.Click += new System.EventHandler( this.menuFileSaveAs_Click );
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size( 126, 6 );
            // 
            // menuFileQuit
            // 
            this.menuFileQuit.Name = "menuFileQuit";
            this.menuFileQuit.Size = new System.Drawing.Size( 129, 22 );
            this.menuFileQuit.Text = "Close(&C)";
            this.menuFileQuit.MouseEnter += new System.EventHandler( this.menuFileQuit_MouseEnter );
            this.menuFileQuit.Click += new System.EventHandler( this.menuFileQuit_Click );
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuEditGenerateFRQ,
            this.menuEditGenerateSTF} );
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size( 52, 20 );
            this.menuEdit.Text = "Edit(&E)";
            // 
            // menuEditGenerateFRQ
            // 
            this.menuEditGenerateFRQ.Name = "menuEditGenerateFRQ";
            this.menuEditGenerateFRQ.Size = new System.Drawing.Size( 169, 22 );
            this.menuEditGenerateFRQ.Text = "Generate FRQ files";
            this.menuEditGenerateFRQ.Click += new System.EventHandler( this.menuEditGenerateFRQ_Click );
            // 
            // menuEditGenerateSTF
            // 
            this.menuEditGenerateSTF.Name = "menuEditGenerateSTF";
            this.menuEditGenerateSTF.Size = new System.Drawing.Size( 169, 22 );
            this.menuEditGenerateSTF.Text = "Generate STF files";
            this.menuEditGenerateSTF.Click += new System.EventHandler( this.menuEditGenerateSTF_Click );
            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuViewSearchNext,
            this.menuViewSearchPrevious} );
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size( 58, 20 );
            this.menuView.Text = "View(&V)";
            // 
            // menuViewSearchNext
            // 
            this.menuViewSearchNext.Name = "menuViewSearchNext";
            this.menuViewSearchNext.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.menuViewSearchNext.Size = new System.Drawing.Size( 216, 22 );
            this.menuViewSearchNext.Text = "Search Next(&N)";
            this.menuViewSearchNext.Click += new System.EventHandler( this.menuViewSearchNext_Click );
            // 
            // menuViewSearchPrevious
            // 
            this.menuViewSearchPrevious.Name = "menuViewSearchPrevious";
            this.menuViewSearchPrevious.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F3)));
            this.menuViewSearchPrevious.Size = new System.Drawing.Size( 216, 22 );
            this.menuViewSearchPrevious.Text = "Search Previous(&P)";
            this.menuViewSearchPrevious.Click += new System.EventHandler( this.menuViewSearchPrevious_Click );
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add( this.btnRefreshFrq );
            this.panelRight.Controls.Add( this.btnRefreshStf );
            this.panelRight.Controls.Add( this.label9 );
            this.panelRight.Controls.Add( this.txtOverlap );
            this.panelRight.Controls.Add( this.lblOverlap );
            this.panelRight.Controls.Add( this.label7 );
            this.panelRight.Controls.Add( this.txtPreUtterance );
            this.panelRight.Controls.Add( this.lblPreUtterance );
            this.panelRight.Controls.Add( this.label5 );
            this.panelRight.Controls.Add( this.txtBlank );
            this.panelRight.Controls.Add( this.lblBlank );
            this.panelRight.Controls.Add( this.label3 );
            this.panelRight.Controls.Add( this.txtConsonant );
            this.panelRight.Controls.Add( this.lblConsonant );
            this.panelRight.Controls.Add( this.label2 );
            this.panelRight.Controls.Add( this.txtOffset );
            this.panelRight.Controls.Add( this.lblOffset );
            this.panelRight.Controls.Add( this.txtAlias );
            this.panelRight.Controls.Add( this.lblAlias );
            this.panelRight.Controls.Add( this.txtFileName );
            this.panelRight.Controls.Add( this.lblFileName );
            this.panelRight.Location = new System.Drawing.Point( 457, 30 );
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size( 210, 273 );
            this.panelRight.TabIndex = 2;
            // 
            // btnRefreshFrq
            // 
            this.btnRefreshFrq.Location = new System.Drawing.Point( 95, 189 );
            this.btnRefreshFrq.Name = "btnRefreshFrq";
            this.btnRefreshFrq.Size = new System.Drawing.Size( 100, 23 );
            this.btnRefreshFrq.TabIndex = 21;
            this.btnRefreshFrq.Text = "Refresh FRQ";
            this.btnRefreshFrq.UseVisualStyleBackColor = true;
            this.btnRefreshFrq.Click += new System.EventHandler( this.btnRefreshFrq_Click );
            // 
            // btnRefreshStf
            // 
            this.btnRefreshStf.Location = new System.Drawing.Point( 95, 218 );
            this.btnRefreshStf.Name = "btnRefreshStf";
            this.btnRefreshStf.Size = new System.Drawing.Size( 100, 23 );
            this.btnRefreshStf.TabIndex = 20;
            this.btnRefreshStf.Text = "Refresh STF";
            this.btnRefreshStf.UseVisualStyleBackColor = true;
            this.btnRefreshStf.Click += new System.EventHandler( this.btnRefreshStf_Click );
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point( 160, 167 );
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size( 20, 12 );
            this.label9.TabIndex = 19;
            this.label9.Text = "ms";
            // 
            // txtOverlap
            // 
            this.txtOverlap.BackColor = System.Drawing.SystemColors.Window;
            this.txtOverlap.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtOverlap.Location = new System.Drawing.Point( 95, 164 );
            this.txtOverlap.Name = "txtOverlap";
            this.txtOverlap.Size = new System.Drawing.Size( 59, 19 );
            this.txtOverlap.TabIndex = 18;
            this.txtOverlap.Text = "0";
            this.txtOverlap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtOverlap.Type = Boare.Cadencii.NumberTextBox.ValueType.Float;
            this.txtOverlap.TextChanged += new System.EventHandler( this.txtOverlap_TextChanged );
            // 
            // lblOverlap
            // 
            this.lblOverlap.AutoSize = true;
            this.lblOverlap.Location = new System.Drawing.Point( 15, 167 );
            this.lblOverlap.Name = "lblOverlap";
            this.lblOverlap.Size = new System.Drawing.Size( 44, 12 );
            this.lblOverlap.TabIndex = 17;
            this.lblOverlap.Text = "Overlap";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 160, 142 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 20, 12 );
            this.label7.TabIndex = 16;
            this.label7.Text = "ms";
            // 
            // txtPreUtterance
            // 
            this.txtPreUtterance.BackColor = System.Drawing.SystemColors.Window;
            this.txtPreUtterance.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtPreUtterance.Location = new System.Drawing.Point( 95, 139 );
            this.txtPreUtterance.Name = "txtPreUtterance";
            this.txtPreUtterance.Size = new System.Drawing.Size( 59, 19 );
            this.txtPreUtterance.TabIndex = 15;
            this.txtPreUtterance.Text = "0";
            this.txtPreUtterance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPreUtterance.Type = Boare.Cadencii.NumberTextBox.ValueType.Float;
            this.txtPreUtterance.TextChanged += new System.EventHandler( this.txtPreUtterance_TextChanged );
            // 
            // lblPreUtterance
            // 
            this.lblPreUtterance.AutoSize = true;
            this.lblPreUtterance.Location = new System.Drawing.Point( 15, 142 );
            this.lblPreUtterance.Name = "lblPreUtterance";
            this.lblPreUtterance.Size = new System.Drawing.Size( 76, 12 );
            this.lblPreUtterance.TabIndex = 14;
            this.lblPreUtterance.Text = "Pre Utterance";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 160, 117 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 20, 12 );
            this.label5.TabIndex = 13;
            this.label5.Text = "ms";
            // 
            // txtBlank
            // 
            this.txtBlank.BackColor = System.Drawing.SystemColors.Window;
            this.txtBlank.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtBlank.Location = new System.Drawing.Point( 95, 114 );
            this.txtBlank.Name = "txtBlank";
            this.txtBlank.Size = new System.Drawing.Size( 59, 19 );
            this.txtBlank.TabIndex = 12;
            this.txtBlank.Text = "0";
            this.txtBlank.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBlank.Type = Boare.Cadencii.NumberTextBox.ValueType.Float;
            this.txtBlank.TextChanged += new System.EventHandler( this.txtBlank_TextChanged );
            // 
            // lblBlank
            // 
            this.lblBlank.AutoSize = true;
            this.lblBlank.Location = new System.Drawing.Point( 15, 117 );
            this.lblBlank.Name = "lblBlank";
            this.lblBlank.Size = new System.Drawing.Size( 34, 12 );
            this.lblBlank.TabIndex = 11;
            this.lblBlank.Text = "Blank";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 160, 92 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 20, 12 );
            this.label3.TabIndex = 10;
            this.label3.Text = "ms";
            // 
            // txtConsonant
            // 
            this.txtConsonant.BackColor = System.Drawing.SystemColors.Window;
            this.txtConsonant.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtConsonant.Location = new System.Drawing.Point( 95, 89 );
            this.txtConsonant.Name = "txtConsonant";
            this.txtConsonant.Size = new System.Drawing.Size( 59, 19 );
            this.txtConsonant.TabIndex = 9;
            this.txtConsonant.Text = "0";
            this.txtConsonant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtConsonant.Type = Boare.Cadencii.NumberTextBox.ValueType.Float;
            this.txtConsonant.TextChanged += new System.EventHandler( this.txtConsonant_TextChanged );
            // 
            // lblConsonant
            // 
            this.lblConsonant.AutoSize = true;
            this.lblConsonant.Location = new System.Drawing.Point( 15, 92 );
            this.lblConsonant.Name = "lblConsonant";
            this.lblConsonant.Size = new System.Drawing.Size( 59, 12 );
            this.lblConsonant.TabIndex = 8;
            this.lblConsonant.Text = "Consonant";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 160, 67 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 20, 12 );
            this.label2.TabIndex = 7;
            this.label2.Text = "ms";
            // 
            // txtOffset
            // 
            this.txtOffset.BackColor = System.Drawing.SystemColors.Window;
            this.txtOffset.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtOffset.Location = new System.Drawing.Point( 95, 64 );
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size( 59, 19 );
            this.txtOffset.TabIndex = 6;
            this.txtOffset.Text = "0";
            this.txtOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtOffset.Type = Boare.Cadencii.NumberTextBox.ValueType.Float;
            this.txtOffset.TextChanged += new System.EventHandler( this.txtOffset_TextChanged );
            // 
            // lblOffset
            // 
            this.lblOffset.AutoSize = true;
            this.lblOffset.Location = new System.Drawing.Point( 15, 67 );
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size( 37, 12 );
            this.lblOffset.TabIndex = 5;
            this.lblOffset.Text = "Offset";
            // 
            // txtAlias
            // 
            this.txtAlias.Location = new System.Drawing.Point( 95, 39 );
            this.txtAlias.Name = "txtAlias";
            this.txtAlias.Size = new System.Drawing.Size( 100, 19 );
            this.txtAlias.TabIndex = 4;
            this.txtAlias.TextChanged += new System.EventHandler( this.txtAlias_TextChanged );
            // 
            // lblAlias
            // 
            this.lblAlias.AutoSize = true;
            this.lblAlias.Location = new System.Drawing.Point( 15, 42 );
            this.lblAlias.Name = "lblAlias";
            this.lblAlias.Size = new System.Drawing.Size( 31, 12 );
            this.lblAlias.TabIndex = 3;
            this.lblAlias.Text = "Alias";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point( 95, 14 );
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size( 100, 19 );
            this.txtFileName.TabIndex = 2;
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point( 15, 17 );
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size( 57, 12 );
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "File Name";
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add( this.btnMinus );
            this.panelBottom.Controls.Add( this.btnPlus );
            this.panelBottom.Controls.Add( this.hScroll );
            this.panelBottom.Controls.Add( this.pictWave );
            this.panelBottom.Location = new System.Drawing.Point( 0, 306 );
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size( 667, 161 );
            this.panelBottom.TabIndex = 4;
            // 
            // btnMinus
            // 
            this.btnMinus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinus.Location = new System.Drawing.Point( 603, 145 );
            this.btnMinus.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size( 32, 16 );
            this.btnMinus.TabIndex = 4;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler( this.btnMinus_Click );
            // 
            // btnPlus
            // 
            this.btnPlus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlus.Location = new System.Drawing.Point( 635, 145 );
            this.btnPlus.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size( 32, 16 );
            this.btnPlus.TabIndex = 3;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler( this.btnPlus_Click );
            // 
            // hScroll
            // 
            this.hScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hScroll.Location = new System.Drawing.Point( 0, 145 );
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new System.Drawing.Size( 603, 16 );
            this.hScroll.TabIndex = 2;
            this.hScroll.ValueChanged += new System.EventHandler( this.hScroll_ValueChanged );
            // 
            // pictWave
            // 
            this.pictWave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictWave.Location = new System.Drawing.Point( 0, 0 );
            this.pictWave.Margin = new System.Windows.Forms.Padding( 0 );
            this.pictWave.Name = "pictWave";
            this.pictWave.Size = new System.Drawing.Size( 667, 145 );
            this.pictWave.TabIndex = 1;
            this.pictWave.TabStop = false;
            this.pictWave.MouseMove += new System.Windows.Forms.MouseEventHandler( this.pictWave_MouseMove );
            this.pictWave.MouseDown += new System.Windows.Forms.MouseEventHandler( this.pictWave_MouseDown );
            this.pictWave.Paint += new System.Windows.Forms.PaintEventHandler( this.pictWave_Paint );
            this.pictWave.MouseUp += new System.Windows.Forms.MouseEventHandler( this.pictWave_MouseUp );
            // 
            // bgWorkRead
            // 
            this.bgWorkRead.DoWork += new System.ComponentModel.DoWorkEventHandler( this.bgWorkRead_DoWork );
            this.bgWorkRead.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler( this.bgWorkRead_RunWorkerCompleted );
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.statusLblTootip} );
            this.statusStrip.Location = new System.Drawing.Point( 0, 473 );
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size( 772, 22 );
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLblTootip
            // 
            this.statusLblTootip.Name = "statusLblTootip";
            this.statusLblTootip.Size = new System.Drawing.Size( 0, 17 );
            // 
            // splitContainerIn
            // 
            this.splitContainerIn.FixedPanel = System.Windows.Forms.FixedPanel.None;
            this.splitContainerIn.IsSplitterFixed = false;
            this.splitContainerIn.Location = new System.Drawing.Point( 673, 275 );
            this.splitContainerIn.Name = "splitContainerIn";
            this.splitContainerIn.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // 
            // 
            this.splitContainerIn.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerIn.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerIn.Panel1.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainerIn.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerIn.Panel1.Location = new System.Drawing.Point( 1, 1 );
            this.splitContainerIn.Panel1.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 4 );
            this.splitContainerIn.Panel1.Name = "m_panel1";
            this.splitContainerIn.Panel1.Padding = new System.Windows.Forms.Padding( 1 );
            this.splitContainerIn.Panel1.Size = new System.Drawing.Size( 101, 190 );
            this.splitContainerIn.Panel1.TabIndex = 0;
            this.splitContainerIn.Panel1MinSize = 25;
            // 
            // 
            // 
            this.splitContainerIn.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerIn.Panel2.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainerIn.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerIn.Panel2.Location = new System.Drawing.Point( 108, 1 );
            this.splitContainerIn.Panel2.Margin = new System.Windows.Forms.Padding( 0 );
            this.splitContainerIn.Panel2.Name = "m_panel2";
            this.splitContainerIn.Panel2.Padding = new System.Windows.Forms.Padding( 1 );
            this.splitContainerIn.Panel2.Size = new System.Drawing.Size( 147, 190 );
            this.splitContainerIn.Panel2.TabIndex = 1;
            this.splitContainerIn.Panel2MinSize = 25;
            this.splitContainerIn.Size = new System.Drawing.Size( 256, 192 );
            this.splitContainerIn.SplitterDistance = 103;
            this.splitContainerIn.SplitterWidth = 4;
            this.splitContainerIn.TabIndex = 6;
            this.splitContainerIn.Text = "bSplitContainer1";
            // 
            // splitContainerOut
            // 
            this.splitContainerOut.FixedPanel = System.Windows.Forms.FixedPanel.None;
            this.splitContainerOut.IsSplitterFixed = false;
            this.splitContainerOut.Location = new System.Drawing.Point( 673, 30 );
            this.splitContainerOut.Name = "splitContainerOut";
            this.splitContainerOut.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // 
            // 
            this.splitContainerOut.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerOut.Panel1.BorderColor = System.Drawing.Color.Black;
            this.splitContainerOut.Panel1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainerOut.Panel1.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 4 );
            this.splitContainerOut.Panel1.Name = "m_panel1";
            this.splitContainerOut.Panel1.Size = new System.Drawing.Size( 441, 111 );
            this.splitContainerOut.Panel1.TabIndex = 0;
            this.splitContainerOut.Panel1MinSize = 25;
            // 
            // 
            // 
            this.splitContainerOut.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerOut.Panel2.BorderColor = System.Drawing.Color.Black;
            this.splitContainerOut.Panel2.Location = new System.Drawing.Point( 0, 115 );
            this.splitContainerOut.Panel2.Margin = new System.Windows.Forms.Padding( 0 );
            this.splitContainerOut.Panel2.Name = "m_panel2";
            this.splitContainerOut.Panel2.Size = new System.Drawing.Size( 441, 105 );
            this.splitContainerOut.Panel2.TabIndex = 1;
            this.splitContainerOut.Panel2MinSize = 25;
            this.splitContainerOut.Size = new System.Drawing.Size( 441, 220 );
            this.splitContainerOut.SplitterDistance = 111;
            this.splitContainerOut.SplitterWidth = 4;
            this.splitContainerOut.TabIndex = 2;
            this.splitContainerOut.Text = "bSplitContainer1";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "oto.ini";
            // 
            // cmenuListFiles
            // 
            this.cmenuListFiles.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.generateSTRAIGHTFileToolStripMenuItem} );
            this.cmenuListFiles.Name = "cmenuListFiles";
            this.cmenuListFiles.Size = new System.Drawing.Size( 197, 26 );
            // 
            // generateSTRAIGHTFileToolStripMenuItem
            // 
            this.generateSTRAIGHTFileToolStripMenuItem.Name = "generateSTRAIGHTFileToolStripMenuItem";
            this.generateSTRAIGHTFileToolStripMenuItem.Size = new System.Drawing.Size( 196, 22 );
            this.generateSTRAIGHTFileToolStripMenuItem.Text = "Generate STRAIGHT file";
            // 
            // panelLeft
            // 
            this.panelLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.panelLeft.Controls.Add( this.buttonPrevious );
            this.panelLeft.Controls.Add( this.buttonNext );
            this.panelLeft.Controls.Add( this.lblSearch );
            this.panelLeft.Controls.Add( this.txtSearch );
            this.panelLeft.Controls.Add( this.listFiles );
            this.panelLeft.Location = new System.Drawing.Point( 0, 30 );
            this.panelLeft.Margin = new System.Windows.Forms.Padding( 0 );
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size( 454, 273 );
            this.panelLeft.TabIndex = 7;
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPrevious.AutoSize = true;
            this.buttonPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonPrevious.Image = global::EditOtoIni.Properties.Resources.arrow_090;
            this.buttonPrevious.Location = new System.Drawing.Point( 227, 245 );
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size( 75, 25 );
            this.buttonPrevious.TabIndex = 4;
            this.buttonPrevious.Text = "Previous";
            this.buttonPrevious.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.Click += new System.EventHandler( this.buttonPrevious_Click );
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNext.AutoSize = true;
            this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonNext.Image = global::EditOtoIni.Properties.Resources.arrow_270;
            this.buttonNext.Location = new System.Drawing.Point( 166, 245 );
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size( 55, 25 );
            this.buttonNext.TabIndex = 3;
            this.buttonNext.Text = "Next";
            this.buttonNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler( this.buttonNext_Click );
            // 
            // lblSearch
            // 
            this.lblSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point( 12, 251 );
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size( 42, 12 );
            this.lblSearch.TabIndex = 2;
            this.lblSearch.Text = "Search:";
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSearch.Location = new System.Drawing.Point( 60, 248 );
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size( 100, 19 );
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler( this.txtSearch_TextChanged );
            // 
            // bgWorkScreen
            // 
            this.bgWorkScreen.DoWork += new System.ComponentModel.DoWorkEventHandler( this.bgWorkScreen_DoWork );
            // 
            // FormUtauVoiceConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 772, 495 );
            this.Controls.Add( this.panelLeft );
            this.Controls.Add( this.splitContainerIn );
            this.Controls.Add( this.panelBottom );
            this.Controls.Add( this.splitContainerOut );
            this.Controls.Add( this.panelRight );
            this.Controls.Add( this.menuStrip );
            this.Controls.Add( this.statusStrip );
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormUtauVoiceConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Voice DB Config. - Untitled";
            this.Load += new System.EventHandler( this.FormUtauVoiceConfig_Load );
            this.SizeChanged += new System.EventHandler( this.FormUtauVoiceConfig_SizeChanged );
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler( this.FormUtauVoiceConfig_FormClosed );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.FormUtauVoiceConfig_FormClosing );
            this.menuStrip.ResumeLayout( false );
            this.menuStrip.PerformLayout();
            this.panelRight.ResumeLayout( false );
            this.panelRight.PerformLayout();
            this.panelBottom.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.pictWave)).EndInit();
            this.statusStrip.ResumeLayout( false );
            this.statusStrip.PerformLayout();
            this.cmenuListFiles.ResumeLayout( false );
            this.panelLeft.ResumeLayout( false );
            this.panelLeft.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listFiles;
        private System.Windows.Forms.ColumnHeader columnHeaderFilename;
        private System.Windows.Forms.ColumnHeader columnHeaderAlias;
        private System.Windows.Forms.ColumnHeader columnHeaderOffset;
        private System.Windows.Forms.ColumnHeader columnHeaderConsonant;
        private System.Windows.Forms.ColumnHeader columnHeaderBlank;
        private System.Windows.Forms.ColumnHeader columnHeaderPreUtterance;
        private System.Windows.Forms.ColumnHeader columnHeaderOverlap;
        private System.Windows.Forms.ColumnHeader columnHeaderFrq;
        private BPictureBox pictWave;
        private Boare.Lib.AppUtil.BSplitContainer splitContainerOut;
        private BMenuBar menuStrip;
        private System.Windows.Forms.Panel panelRight;
        private BMenuItem menuFile;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.HScrollBar hScroll;
        private System.ComponentModel.BackgroundWorker bgWorkRead;
        private System.Windows.Forms.StatusStrip statusStrip;
        private Boare.Lib.AppUtil.BSplitContainer splitContainerIn;
        private BLabel lblFileName;
        private BTextBox txtAlias;
        private BLabel lblAlias;
        private BTextBox txtFileName;
        private BLabel label2;
        private NumberTextBox txtOffset;
        private BLabel lblOffset;
        private BLabel label9;
        private NumberTextBox txtOverlap;
        private BLabel lblOverlap;
        private BLabel label7;
        private NumberTextBox txtPreUtterance;
        private BLabel lblPreUtterance;
        private BLabel label5;
        private NumberTextBox txtBlank;
        private BLabel lblBlank;
        private BLabel label3;
        private NumberTextBox txtConsonant;
        private BLabel lblConsonant;
        private BButton btnPlus;
        private BButton btnMinus;
        private BMenuItem menuFileOpen;
        private BMenuItem menuFileSave;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private BMenuItem menuFileQuit;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private BMenuItem menuFileSaveAs;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripStatusLabel statusLblTootip;
        private System.Windows.Forms.ColumnHeader columnHeaderStf;
        private System.Windows.Forms.ContextMenuStrip cmenuListFiles;
        private BMenuItem generateSTRAIGHTFileToolStripMenuItem;
        private BMenuItem menuEdit;
        private BMenuItem menuEditGenerateSTF;
        private BButton btnRefreshStf;
        private BButton btnRefreshFrq;
        private BMenuItem menuEditGenerateFRQ;
        private System.Windows.Forms.Panel panelLeft;
        private BTextBox txtSearch;
        private BLabel lblSearch;
        private BButton buttonNext;
        private BButton buttonPrevious;
        private System.ComponentModel.BackgroundWorker bgWorkScreen;
        private BMenuItem menuView;
        private BMenuItem menuViewSearchNext;
        private BMenuItem menuViewSearchPrevious;
        #endregion
#endif
    }

#if !JAVA
}
#endif
