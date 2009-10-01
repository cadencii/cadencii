/*
 * FormMain.cs
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
//#define MONITOR_FPS
#define AUTHOR_LIST_SAVE_BUTTON_VISIBLE
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Boare.Lib.AppUtil;
using Boare.Lib.Media;
using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Cadencii {

    using boolean = Boolean;
    using Integer = Int32;

    public delegate void VoidDelegate();

    public partial class FormMain : Form {
        #region Static Readonly Field
        private readonly SolidBrush s_brs_192_192_192 = new SolidBrush( Color.FromArgb( 192, 192, 192 ) );
        private readonly SolidBrush s_brs_a098_000_000_000 = new SolidBrush( Color.FromArgb( 98, 0, 0, 0 ) );
        private readonly SolidBrush s_brs_106_108_108 = new SolidBrush( Color.FromArgb( 106, 108, 108 ) );
        private readonly SolidBrush s_brs_180_180_180 = new SolidBrush( Color.FromArgb( 180, 180, 180 ) );
        private readonly SolidBrush s_brs_212_212_212 = new SolidBrush( Color.FromArgb( 212, 212, 212 ) );
        private readonly SolidBrush s_brs_125_123_124 = new SolidBrush( Color.FromArgb( 125, 123, 124 ) );
        private readonly SolidBrush s_brs_240_240_240 = new SolidBrush( Color.FromArgb( 240, 240, 240 ) );
        private readonly SolidBrush s_brs_011_233_244 = new SolidBrush( Color.FromArgb( 11, 233, 244 ) );
        private readonly SolidBrush s_brs_182_182_182 = new SolidBrush( Color.FromArgb( 182, 182, 182 ) );
        private readonly SolidBrush s_brs_072_077_098 = new SolidBrush( Color.FromArgb( 72, 77, 98 ) );
        private readonly SolidBrush s_brs_153_153_153 = new SolidBrush( Color.FromArgb( 153, 153, 153 ) );
        private readonly SolidBrush s_brs_147_147_147 = new SolidBrush( Color.FromArgb( 147, 147, 147 ) );
        private readonly SolidBrush s_brs_000_255_214 = new SolidBrush( Color.FromArgb( 0, 255, 214 ) );
        private readonly Pen s_pen_112_112_112 = new Pen( Color.FromArgb( 112, 112, 112 ) );
        private readonly Pen s_pen_118_123_138 = new Pen( Color.FromArgb( 118, 123, 138 ) );
        private readonly Pen s_pen_LU = new Pen( Color.FromArgb( 128, 106, 52, 255 ) );
        private readonly Pen s_pen_RD = new Pen( Color.FromArgb( 204, 40, 47, 255 ) );
        private readonly Pen s_pen_161_157_136 = new Pen( Color.FromArgb( 161, 157, 136 ) );
        private readonly Pen s_pen_209_204_172 = new Pen( Color.FromArgb( 209, 204, 172 ) );
        private readonly Pen s_pen_160_160_160 = new Pen( Color.FromArgb( 160, 160, 160 ) );
        private readonly Pen s_pen_105_105_105 = new Pen( Color.FromArgb( 105, 105, 105 ) );
        private readonly Pen s_pen_106_108_108 = new Pen( Color.FromArgb( 106, 108, 108 ) );
        private readonly Pen s_pen_212_212_212 = new Pen( Color.FromArgb( 212, 212, 212 ) );
        private readonly Pen s_pen_051_051_000 = new Pen( Color.FromArgb( 51, 51, 0 ) );
        private readonly Pen s_pen_125_123_124 = new Pen( Color.FromArgb( 125, 123, 124 ) );
        private readonly Pen s_pen_187_187_255 = new Pen( Color.FromArgb( 187, 187, 255 ) );
        private readonly Pen s_pen_007_007_151 = new Pen( Color.FromArgb( 7, 7, 151 ) );
        private readonly Pen s_pen_a136_000_000_000 = new Pen( Color.FromArgb( 136, Color.Black ) );
        private readonly Pen s_pen_dashed_171_171_171 = new Pen( Color.FromArgb( 171, 171, 171 ) );
        private readonly Pen s_pen_dashed_209_204_172 = new Pen( Color.FromArgb( 209, 204, 172 ) );
        private readonly Pen s_pen_065_065_065 = new Pen( Color.FromArgb( 65, 65, 65 ) );
        private readonly Color s_txtbox_backcolor = Color.FromArgb( 128, 128, 128 );
        public readonly Color[] s_HIDDEN = new Color[]{
            Color.FromArgb( 181, 162, 123 ),
            Color.FromArgb( 179, 181, 123 ),
            Color.FromArgb( 157, 181, 123 ),
            Color.FromArgb( 135, 181, 123 ),
            Color.FromArgb( 123, 181, 133 ),
            Color.FromArgb( 123, 181, 154 ),
            Color.FromArgb( 123, 181, 176 ),
            Color.FromArgb( 123, 164, 181 ),
            Color.FromArgb( 123, 142, 181 ),
            Color.FromArgb( 125, 123, 181 ),
            Color.FromArgb( 169, 123, 181 ),
            Color.FromArgb( 181, 123, 171 ),
            Color.FromArgb( 181, 123, 149 ),
            Color.FromArgb( 181, 123, 127 ),
            Color.FromArgb( 181, 140, 123 ),
            Color.FromArgb( 181, 126, 123 ) };
        private readonly Color s_note_fill = Color.FromArgb( 181, 220, 86 );
        private readonly AuthorListEntry[] _CREDIT = new AuthorListEntry[]{
            new AuthorListEntry( "is developped by:", FontStyle.Italic ),
            new AuthorListEntry( "kbinani" ),
            new AuthorListEntry( "HAL(修羅場P)" ),
            new AuthorListEntry(),
            new AuthorListEntry(),
            new AuthorListEntry( "Special Thanks to", FontStyle.Italic| FontStyle.Bold ),
            new AuthorListEntry(),
            new AuthorListEntry( "tool icons designer:", FontStyle.Italic ),
            new AuthorListEntry( "Yusuke KAMIYAMANE" ),
            new AuthorListEntry(),
            new AuthorListEntry( "developper of STRAIGHT LIBRARY:", FontStyle.Italic ),
            new AuthorListEntry( "Hideki KAWAHARA" ),
            new AuthorListEntry( "Tetsu TAKAHASHI" ),
            new AuthorListEntry( "Hideki BANNO" ),
            new AuthorListEntry( "Masanori MORISE" ),
            new AuthorListEntry( "(sorry, list is not complete)", FontStyle.Italic ),
            new AuthorListEntry(),
            new AuthorListEntry( "developper of v.Connect:", FontStyle.Italic ),
            new AuthorListEntry( "HAL(修羅場P)" ),
            new AuthorListEntry(),
            new AuthorListEntry( "developper of UTAU:", FontStyle.Italic ),
            new AuthorListEntry( "飴屋/菖蒲" ),
            new AuthorListEntry(),
            new AuthorListEntry( "promoter:", FontStyle.Italic ),
            new AuthorListEntry( "zhuo" ),
            new AuthorListEntry(),
            new AuthorListEntry( "library tester:", FontStyle.Italic ),
            new AuthorListEntry( "evm" ),
            new AuthorListEntry( "そろそろP" ),
            new AuthorListEntry( "めがね１１０" ),
            new AuthorListEntry( "上総" ),
            new AuthorListEntry( "NOIKE" ),
            new AuthorListEntry( "逃亡者" ),
            new AuthorListEntry(),
            new AuthorListEntry( "translator:", FontStyle.Italic ),
            new AuthorListEntry( "Eji (zh-TW translation)" ),
            new AuthorListEntry( "kankan (zh-TW translation)" ),
            new AuthorListEntry(),
            new AuthorListEntry(),
            new AuthorListEntry( "Thanks to", FontStyle.Italic | FontStyle.Bold ),
            new AuthorListEntry(),
            new AuthorListEntry( "ないしょの人" ),
            new AuthorListEntry( "naquadah" ),
            new AuthorListEntry( "1zo" ),
            new AuthorListEntry( "Amby" ),
            new AuthorListEntry( "ケロッグ" ),
            new AuthorListEntry( "beginner" ),
            new AuthorListEntry( "b2ox" ),
            new AuthorListEntry( "麻太郎" ),
            new AuthorListEntry( "もみじぱん" ),
            new AuthorListEntry( "PEX" ),
            new AuthorListEntry( "やなぎがうら" ),
            new AuthorListEntry( "cocoonP" ),
            new AuthorListEntry( "かつ" ),
            new AuthorListEntry( "ちゃそ" ),
            new AuthorListEntry( "all members of Cadencii bbs", FontStyle.Italic ),
            new AuthorListEntry(),
            new AuthorListEntry( "     ... and you !", FontStyle.Bold | FontStyle.Italic ),
        };
        private readonly Font s_F9PT = new Font( FontFamily.GenericSansSerif, 9f, GraphicsUnit.Point );
        #endregion

        #region Constants and internal enums
        /// <summary>
        /// カーブエディタ画面の編集モード
        /// </summary>
        internal enum CurveEditMode {
            /// <summary>
            /// 何もしていない
            /// </summary>
            NONE,
            /// <summary>
            /// 鉛筆ツールで編集するモード
            /// </summary>
            EDIT,
            /// <summary>
            /// ラインツールで編集するモード
            /// </summary>
            LINE,
            /// <summary>
            /// 鉛筆ツールでVELを編集するモード
            /// </summary>
            EDIT_VEL,
            /// <summary>
            /// ラインツールでVELを編集するモード
            /// </summary>
            LINE_VEL,
            /// <summary>
            /// 真ん中ボタンでドラッグ中
            /// </summary>
            MIDDLE_DRAG,
        }

        internal enum ExtDragXMode {
            RIGHT,
            LEFT,
            NONE,
        }

        internal enum ExtDragYMode {
            UP,
            DOWN,
            NONE,
        }

        internal enum GameControlMode {
            DISABLED,
            NORMAL,
            KEYBOARD,
            CURSOR,
        }

        enum OverviewMouseDownMode {
            NONE,
            LEFT,
            MIDDLE,
        }

        /// <summary>
        /// スクロールバーの最小サイズ(ピクセル)
        /// </summary>
        private const int MIN_BAR_ACTUAL_LENGTH = 17;
        /// <summary>
        /// エントリの端を移動する時の、ハンドル許容範囲の幅
        /// </summary>
        private const int _EDIT_HANDLE_WIDTH = 7;
        private const int _TOOL_BAR_HEIGHT = 46;
        /// <summary>
        /// 単音プレビュー時に、wave生成完了を待つ最大の秒数
        /// </summary>
        private const double _WAIT_LIMIT = 5.0;
        public const String _APP_NAME = "Cadencii";
        /// <summary>
        /// 表情線の先頭部分のピクセル幅
        /// </summary>
        private const int _PX_ACCENT_HEADER = 21;
        /// <summary>
        /// パフォーマンスカウンタ用バッファの容量
        /// </summary>
        private const int _NUM_PCOUNTER = 50;
        private const String _VERSION_HISTORY_URL = "http://www.ne.jp/asahi/kbinani/home/cadencii/version_history.xml";
        /// <summary>
        /// コントロールカーブが不可視状態における、splitContainer1.Panel2の最小サイズ
        /// </summary>
        private const int _SPL1_PANEL2_MIN_HEIGHT = 34;
        /// <summary>
        /// splitContainer2.Panel2の最小サイズ
        /// </summary>
        private const int _SPL2_PANEL2_MIN_HEIGHT = 25;
        /// <summary>
        /// splitContainer*で使用するSplitterWidthプロパティの値
        /// </summary>
        private const int _SPL_SPLITTER_WIDTH = 4;
        const int _PICT_POSITION_INDICATOR_HEIGHT = 48;
        const int _SCROLL_WIDTH = 16;
        /// <summary>
        /// Overviewペインの高さ
        /// </summary>
        const int _OVERVIEW_HEIGHT = 50;
        /// <summary>
        /// splitContainerPropertyの最小幅
        /// </summary>
        const int _PROPERTY_DOCK_MIN_WIDTH = 50;
        /// <summary>
        /// Overviewに描く音符を表す円の直径
        /// </summary>
        const int _OVERVIEW_DOT_DIAM = 2;
        /// <summary>
        /// btnLeft, btnRightを押した時の、スクロール速度(px/sec)。
        /// </summary>
        const float _OVERVIEW_SCROLL_SPEED = 500.0f;
        const int _OVERVIEW_SCALE_COUNT_MAX = 7;
        const int _OVERVIEW_SCALE_COUNT_MIN = 3;
        #endregion

        #region Static Field
        /// <summary>
        /// CTRLキー。MacOSXの場合はMenu
        /// </summary>
        private Keys s_modifier_key = Keys.Control;
        #endregion

        #region Fields
        private VersionInfo m_versioninfo = null;
        private Cursor HAND;
        private TextBoxEx m_input_textbox = null;
        /// <summary>
        /// ボタンがDownされた位置。(座標はpictureBox基準)
        /// </summary>
        private Point m_button_initial;
        /// <summary>
        /// 真ん中ボタンがダウンされたときのvscrollのvalue値
        /// </summary>
        private int m_middle_button_vscroll;
        /// <summary>
        /// 真ん中ボタンがダウンされたときのhscrollのvalue値
        /// </summary>
        private int m_middle_button_hscroll;
        private boolean m_edited = false;
        /// <summary>
        /// パフォーマンスカウンタ
        /// </summary>
        private float[] m_performance = new float[_NUM_PCOUNTER];
        /// <summary>
        /// 最後にpictureBox1_Paintが実行された時刻
        /// </summary>
        private DateTime m_last_ignitted;
        /// <summary>
        /// パフォーマンスカウンタから算出される画面の更新速度
        /// </summary>
        private float m_fps = 0f;
        /// <summary>
        /// カーブエディタの編集モード
        /// </summary>
        private CurveEditMode m_edit_curve_mode = CurveEditMode.NONE;
        /// <summary>
        /// ピアノロールの右クリックが表示される直前のマウスの位置
        /// </summary>
        private Point m_cMenuOpenedPosition;
#if USE_DOBJ
        /// <summary>
        /// 画面に描かれるエントリのリスト．trackBar.Valueの変更やエントリの編集などのたびに更新される
        /// </summary>
        private Vector<Vector<DrawObject>> m_draw_objects;
#endif
        /// <summary>
        /// m_draw_objectsを描く際の，最初に検索されるインデクス．
        /// </summary>
        private int[] m_draw_start_index = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// トラック名の入力に使用するテキストボックス
        /// </summary>
        private TextBoxEx m_txtbox_track_name;
        /// <summary>
        /// ピアノロールの画面外へのドラッグ時、前回自動スクロール操作を行った時刻
        /// </summary>
        private DateTime m_timer_drag_last_ignitted;
        /// <summary>
        /// 画面外への自動スクロールモード
        /// </summary>
        private ExtDragXMode m_ext_dragx = ExtDragXMode.NONE;
        private ExtDragYMode m_ext_dragy = ExtDragYMode.NONE;
        /// <summary>
        /// EditMode=MoveEntryで，移動を開始する直前のマウスの仮想スクリーン上の位置
        /// </summary>
        private Point m_mouse_move_init;
        /// <summary>
        /// EditMode=MoveEntryで，移動を開始する直前のマウスの位置と，音符の先頭との距離(ピクセル)
        /// </summary>
        private int m_mouse_move_offset;
        /// <summary>
        /// マウスが降りた仮想スクリーン上の座標(ピクセル)
        /// </summary>
        private Point m_pointer_mouse_down;
        /// <summary>
        /// マウスが降りていて，かつ範囲選択をしているときに立つフラグ
        /// </summary>
        private boolean m_pointer_downed = false;
        /// <summary>
        /// マウスが降りているかどうかを表すフラグ．m_pointer_downedとは別なので注意
        /// </summary>
        private boolean m_mouse_downed = false;
        private VsqEvent m_adding;
        private int m_adding_length;
        /// <summary>
        /// テンポ変更の位置を、マウスドラッグで移動させているかどうかを表すフラグ
        /// </summary>
        private boolean m_tempo_dragging = false;
        private int m_tempo_dragging_deltaclock = 0;
        /// <summary>
        /// 拍子変更の位置を、マウスドラッグで移動させているかどうかを表すフラグ
        /// </summary>
        private boolean m_timesig_dragging = false;
        private int m_timesig_dragging_deltaclock = 0;
        private boolean m_mouse_downed_trackselector = false;
        private ExtDragXMode m_ext_dragx_trackselector = ExtDragXMode.NONE;
        private boolean m_mouse_moved = false;
        /// <summary>
        /// マウスホバーを発生させるスレッド
        /// </summary>
        private Thread m_mouse_hover_thread = null;
        private ImeMode m_last_imemode = ImeMode.On;
        private boolean m_last_symbol_edit_mode = false;
        /// <summary>
        /// 鉛筆のモード
        /// </summary>
        private PencilMode m_pencil_mode = new PencilMode();
        private boolean m_startmark_dragging = false;
        private boolean m_endmark_dragging = false;
        /// <summary>
        /// ビブラート範囲を編集中の音符のInternalID
        /// </summary>
        private int m_vibrato_editing_id = -1;
        private TrackSelector trackSelector;
        /// <summary>
        /// このフォームがアクティブ化されているかどうか
        /// </summary>
        private boolean m_form_activated = true;
        private GameControlMode m_game_mode = GameControlMode.DISABLED;
        /// <summary>
        /// 直接再生モード時の、再生開始した位置の曲頭からの秒数
        /// </summary>
        private float m_direct_play_shift = 0.0f;
        /// <summary>
        /// プレビュー再生の長さ
        /// </summary>
        private double m_preview_ending_time;
        private System.Windows.Forms.Timer m_timer;
        private boolean m_last_pov_r = false;
        private boolean m_last_pov_l = false;
        private boolean m_last_pov_u = false;
        private boolean m_last_pov_d = false;
        private boolean m_last_btn_x = false;
        private boolean m_last_btn_o = false;
        private boolean m_last_btn_re = false;
        private boolean m_last_btn_tr = false;
        private boolean m_last_select = false;
        /// <summary>
        /// 前回ゲームコントローラのイベントを処理した時刻
        /// </summary>
        private DateTime m_last_event_processed;
        /// <summary>
        /// splitContainer2.Panel2を最小化する直前の、splitContainer2.SplitterDistance
        /// </summary>
        private int m_last_splitcontainer2_split_distance = -1;
        private boolean m_spacekey_downed = false;
        private MidiInDevice m_midi_in = null;
        private FormMidiImExport m_midi_imexport_dialog = null;
        private TreeMap<EditTool, Cursor> m_cursor = new TreeMap<EditTool, Cursor>();
        private Preference m_preference_dlg;
        private ToolStripButton m_strip_ddbtn_metronome;
        //private FormUtauVoiceConfig m_utau_voice_dialog = null;
        private PropertyPanelContainer m_property_panel_container;
        private Vector<ToolStripItem> m_palette_tools = new Vector<ToolStripItem>();

        private int m_overview_direction = 1;
        private Thread m_overview_update_thread = null;
        private int m_overview_start_to_draw_clock_initial_value;
        /// <summary>
        /// btnLeftまたはbtnRightが下りた時刻
        /// </summary>
        private DateTime m_overview_btn_downed;
        /// <summary>
        /// Overview画面左端でのクロック
        /// </summary>
        private int m_overview_start_to_draw_clock = 0;
        /// <summary>
        /// Overview画面の表示倍率
        /// </summary>
        private float m_overview_px_per_clock = 0.01f;
        /// <summary>
        /// Overview画面に表示されている音符の，平均ノートナンバー．これが，縦方向の中心線に反映される
        /// </summary>
        private float m_overview_average_note = 60.0f;
        /// <summary>
        /// Overview画面でマウスが降りている状態かどうか
        /// </summary>
        private OverviewMouseDownMode m_overview_mouse_down_mode = OverviewMouseDownMode.NONE;
        /// <summary>
        /// Overview画面で、マウスが下りた位置のx座標
        /// </summary>
        private int m_overview_mouse_downed_locationx;
        private int m_overview_scale_count = 5;
        #endregion

        public FormMain() {
#if DEBUG
            bocoree.debug.push_log( "FormMain..ctor()" );
            bocoree.debug.push_log( "    " + Environment.OSVersion.ToString() );
            bocoree.debug.push_log( "    FormID=" + AppManager.getID() );
            AppManager.debugWriteLine( "FormMain..ctor()" );
#endif
            AppManager.baseFont10Bold = new Font( AppManager.editorConfig.BaseFontName, 10, FontStyle.Bold );
            AppManager.baseFont8 = new Font( AppManager.editorConfig.BaseFontName, 8 );
            AppManager.baseFont10 = new Font( AppManager.editorConfig.BaseFontName, 10 );
            AppManager.baseFont9 = new Font( AppManager.editorConfig.BaseFontName, 9 );

            s_modifier_key = ((AppManager.editorConfig.Platform == Platform.Macintosh) ? Keys.Menu : Keys.Control);

            AppManager.setVsqFile( new VsqFileEx( AppManager.editorConfig.DefaultSingerName,
                                                  AppManager.editorConfig.DefaultPreMeasure,
                                                  4,
                                                  4,
                                                  500000 ) );
            InitializeComponent();

            m_overview_scale_count = AppManager.editorConfig.OverviewScaleCount;
            m_overview_px_per_clock = getOverviewScaleX( m_overview_scale_count );

            menuVisualOverview.Checked = AppManager.editorConfig.OverviewEnabled;
            m_property_panel_container = new PropertyPanelContainer();

            m_strip_ddbtn_metronome = new ToolStripButton( "Metronome", Properties.Resources.alarm_clock );
            m_strip_ddbtn_metronome.Name = "m_strip_ddbtn_metronome";
            m_strip_ddbtn_metronome.CheckOnClick = true;
            m_strip_ddbtn_metronome.Checked = AppManager.editorConfig.MetronomeEnabled;
            m_strip_ddbtn_metronome.CheckedChanged += new EventHandler( m_strip_ddbtn_metronome_CheckedChanged );
            toolStripBottom.Items.Add( m_strip_ddbtn_metronome );

            trackSelector = new TrackSelector();
            trackSelector.BackColor = Color.FromArgb( 108, 108, 108 );
            trackSelector.CurveVisible = true;
            trackSelector.Location = new Point( 0, 242 );
            trackSelector.Margin = new Padding( 0 );
            trackSelector.Name = "trackSelector";
            trackSelector.SelectedCurve = CurveType.VEL;
            trackSelector.Size = new Size( 446, 250 );
            trackSelector.TabIndex = 0;
            trackSelector.MouseClick += new MouseEventHandler( this.trackSelector_MouseClick );
            trackSelector.SelectedTrackChanged += new BSimpleDelegate<int>( this.trackSelector_SelectedTrackChanged );
            trackSelector.MouseUp += new MouseEventHandler( this.trackSelector_MouseUp );
            trackSelector.MouseDown += new MouseEventHandler( trackSelector_MouseDown );
            trackSelector.SelectedCurveChanged += new BSimpleDelegate<CurveType>( this.trackSelector_SelectedCurveChanged );
            trackSelector.MouseMove += new MouseEventHandler( this.trackSelector_MouseMove );
            trackSelector.RenderRequired += new BSimpleDelegate<int[]>( this.trackSelector_RenderRequired );
            trackSelector.PreviewKeyDown += new PreviewKeyDownEventHandler( this.trackSelector_PreviewKeyDown );
            trackSelector.KeyDown += new KeyEventHandler( commonCaptureSpaceKeyDown );
            trackSelector.KeyUp += new KeyEventHandler( commonCaptureSpaceKeyUp );
            UpdateTrackSelectorVisibleCurve();

            stripBtnScroll.Checked = AppManager.autoScroll;
            applySelectedTool();
            ApplyQuantizeMode();

            // Palette Toolの読み込み
            updatePaletteTool();

            // toolStipの位置を，前回終了時の位置に戻す
            Vector<ToolStrip> top = new Vector<ToolStrip>();
            Vector<ToolStrip> bottom = new Vector<ToolStrip>();
            if ( toolStripTool.Parent != null ) {
                if ( toolStripTool.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    toolStripContainer.TopToolStripPanel.Controls.Remove( toolStripTool );
                    top.add( toolStripTool );
                } else if ( toolStripTool.Parent.Equals( toolStripContainer.BottomToolStripPanel ) ) {
                    toolStripContainer.BottomToolStripPanel.Controls.Remove( toolStripTool );
                    bottom.add( toolStripTool );
                }
            }
            if ( toolStripMeasure.Parent != null ) {
                if ( toolStripMeasure.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    toolStripContainer.TopToolStripPanel.Controls.Remove( toolStripMeasure );
                    top.add( toolStripMeasure );
                } else if ( toolStripMeasure.Parent.Equals( toolStripContainer.BottomToolStripPanel ) ) {
                    toolStripContainer.BottomToolStripPanel.Controls.Remove( toolStripMeasure );
                    bottom.add( toolStripMeasure );
                }
            }
            if ( toolStripPosition.Parent != null ) {
                if ( toolStripPosition.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    toolStripContainer.TopToolStripPanel.Controls.Remove( toolStripPosition );
                    top.add( toolStripPosition );
                } else if ( toolStripPosition.Parent.Equals( toolStripContainer.BottomToolStripPanel ) ) {
                    toolStripContainer.BottomToolStripPanel.Controls.Remove( toolStripPosition );
                    bottom.add( toolStripPosition );
                }
            }
            if ( toolStripFile.Parent != null ) {
                if ( toolStripFile.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    toolStripContainer.TopToolStripPanel.Controls.Remove( toolStripFile );
                    top.add( toolStripFile );
                } else if ( toolStripFile.Parent.Equals( toolStripContainer.BottomToolStripPanel ) ) {
                    toolStripContainer.BottomToolStripPanel.Controls.Remove( toolStripFile );
                    bottom.add( toolStripFile );
                }
            }
            /*if ( toolStripPaletteTools.Parent != null ) {
                if ( toolStripPaletteTools.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    toolStripContainer.TopToolStripPanel.Controls.Remove( toolStripPaletteTools );
                    if ( toolStripPaletteTools.Visible ) {
                        top.Add( toolStripPaletteTools );
                    }
                } else if ( toolStripPaletteTools.Parent.Equals( toolStripContainer.BottomToolStripPanel ) ) {
                    toolStripContainer.BottomToolStripPanel.Controls.Remove( toolStripPaletteTools );
                    if ( toolStripPaletteTools.Visible ) {
                        bottom.Add( toolStripPaletteTools );
                    }
                }
            }*/

            splitContainer1.Panel1.BorderStyle = BorderStyle.None;
            splitContainer1.Panel2.BorderStyle = BorderStyle.None;
            splitContainer1.BackColor = Color.FromArgb( 212, 212, 212 );
            splitContainer2.Panel1.Controls.Add( panel1 );
            panel1.Dock = DockStyle.Fill;
            splitContainer2.Panel2.Controls.Add( panel2 );
            splitContainer2.Panel2.BorderStyle = BorderStyle.FixedSingle;
            splitContainer2.Panel2.BorderColor = Color.FromArgb( 112, 112, 112 );
            splitContainer1.Panel1.Controls.Add( splitContainer2 );
            panel2.Dock = DockStyle.Fill;
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add( trackSelector );
            trackSelector.Dock = DockStyle.Fill;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Panel2MinSize = trackSelector.PreferredMinSize;
            splitContainerProperty.FixedPanel = FixedPanel.Panel1;
            splitContainerProperty.Panel1.Controls.Add( m_property_panel_container );
            m_property_panel_container.Dock = DockStyle.Fill;
            splitContainerProperty.Panel2.Controls.Add( splitContainer1 );
            splitContainerProperty.Dock = DockStyle.Fill;

            // コントロールの位置・サイズを調節
            splitContainer2.Panel1.SuspendLayout();
            panel1.SuspendLayout();
            pictPianoRoll.SuspendLayout();
            vScroll.SuspendLayout();
            // panel1の中身は
            // picturePositionIndicator
            picturePositionIndicator.Left = 0;
            picturePositionIndicator.Top = 0;
            picturePositionIndicator.Width = panel1.Width;
            // pictPianoRoll
            pictPianoRoll.Left = 0;
            pictPianoRoll.Top = picturePositionIndicator.Height;
            pictPianoRoll.Width = panel1.Width - vScroll.Width;
            pictPianoRoll.Height = panel1.Height - picturePositionIndicator.Height - hScroll.Height;
            // vScroll
            vScroll.Left = pictPianoRoll.Width;
            vScroll.Top = picturePositionIndicator.Height;
            vScroll.Height = pictPianoRoll.Height;
            // pictureBox3
            pictureBox3.Left = 0;
            pictureBox3.Top = panel1.Height - hScroll.Height;
            pictureBox3.Height = hScroll.Height;
            // hScroll
            hScroll.Left = pictureBox3.Width;
            hScroll.Top = panel1.Height - hScroll.Height;
            hScroll.Width = panel1.Width - pictureBox3.Width - trackBar.Width - pictureBox2.Width;
            // trackBar
            trackBar.Left = pictureBox3.Width + hScroll.Width;
            trackBar.Top = panel1.Height - hScroll.Height;
            // pictureBox2
            pictureBox2.Left = panel1.Width - vScroll.Width;
            pictureBox2.Top = panel1.Height - hScroll.Height;

            vScroll.ResumeLayout();
            pictPianoRoll.ResumeLayout();
            panel1.ResumeLayout();
            splitContainer2.Panel1.ResumeLayout();

            pictPianoRoll.MouseWheel += new MouseEventHandler( pictPianoRoll_MouseWheel );
            trackSelector.MouseWheel += new MouseEventHandler( trackSelector_MouseWheel );
            picturePositionIndicator.MouseWheel += new MouseEventHandler( picturePositionIndicator_MouseWheel );
            menuVisualOverview.CheckedChanged += new EventHandler( menuVisualOverview_CheckedChanged );

            hScroll.Maximum = AppManager.getVsqFile().TotalClocks + 240;
            hScroll.SmallChange = 240;
            hScroll.LargeChange = 240 * 4;

            vScroll.Maximum = AppManager.editorConfig.PxTrackHeight * 128;
            vScroll.SmallChange = 24;
            vScroll.LargeChange = 24 * 4;

            trackSelector.CurveVisible = true;

            // 左上のやつから順に登録
            toolStripTool.Location = AppManager.editorConfig.ToolEditTool.Location;
            toolStripMeasure.Location = AppManager.editorConfig.ToolMeasureLocation.Location;
            toolStripPosition.Location = AppManager.editorConfig.ToolPositionLocation.Location;
            toolStripFile.Location = AppManager.editorConfig.ToolFileLocation.Location;
            //toolStripPaletteTools.Location = AppManager.EditorConfig.ToolPaletteLocation.Location;

            AddToolStripInPositionOrder( toolStripContainer.TopToolStripPanel, top );
            AddToolStripInPositionOrder( toolStripContainer.BottomToolStripPanel, bottom );

            toolStripTool.ParentChanged += new System.EventHandler( this.toolStripEdit_ParentChanged );
            toolStripTool.Move += new System.EventHandler( this.toolStripEdit_Move );
            toolStripMeasure.ParentChanged += new System.EventHandler( this.toolStripMeasure_ParentChanged );
            toolStripMeasure.Move += new System.EventHandler( this.toolStripMeasure_Move );
            toolStripPosition.ParentChanged += new System.EventHandler( this.toolStripPosition_ParentChanged );
            toolStripPosition.Move += new System.EventHandler( this.toolStripPosition_Move );
            toolStripFile.ParentChanged += new EventHandler( toolStripFile_ParentChanged );
            toolStripFile.Move += new EventHandler( toolStripFile_Move );
            //toolStripPaletteTools.ParentChanged += new EventHandler( toolStripPaletteTools_ParentChanged );
            //toolStripPaletteTools.Move += new EventHandler( toolStripPaletteTools_Move );
            
            m_input_textbox = new TextBoxEx();
            m_input_textbox.Visible = false;
            m_input_textbox.BorderStyle = BorderStyle.None;
            m_input_textbox.Width = 80;
            m_input_textbox.BackColor = Color.White;
            m_input_textbox.AcceptsReturn = true;
            m_input_textbox.Font = new Font( AppManager.editorConfig.BaseFontName, 9 );
            m_input_textbox.Enabled = false;
            panel1.Controls.Add( m_input_textbox );
            m_input_textbox.Parent = pictPianoRoll;

            int fps = 1000 / AppManager.editorConfig.MaximumFrameRate;
            timer.Interval = (fps <= 0) ? 1 : fps;
#if DEBUG
            menuHelpDebug.Visible = true;
#endif
            const String _HAND = "AAACAAEAICAAABAAEADoAgAAFgAAACgAAAAgAAAAQAAAAAEABAAAAAAAgAIAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAgAAAAACAAACAgAAAAACAAIAAgAAAgIAAwMDAAICAgAD/AAAAAP8AAP//AAAAAP8A/wD/AAD//wD///8AAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/wAAAAAAAAAAAAAAAAAAD" +
                "//wAAAAAAAAAAAAAAAAAA//8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP8AAAAA/wAAAAAAAAAAA" +
                "A//AAAAAP/wAAAAAAAAAAAP/wAAAAD/8AAAAAAAAAAAAP8AAAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAA//8AAAAAAAAAAAAAAAAAAP//AAAAAAAAAAAAAAAAAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD////////////////////////////////////////////+f////" +
                "D////gf///4H////D///8/z//+H4f//B+D//wfg//+H4f//z/P///w////4H///+B////w////+f//////////////////////////" +
                "//////////////////w==";
            using ( MemoryStream ms = new MemoryStream( Convert.FromBase64String( _HAND ) ) ) {
                HAND = new Cursor( ms );
            }

            ApplyShortcut();
        }

        private void m_strip_ddbtn_metronome_CheckedChanged( object sender, EventArgs e ) {
            AppManager.editorConfig.MetronomeEnabled = m_strip_ddbtn_metronome.Checked;
            if ( AppManager.editorConfig.MetronomeEnabled && AppManager.getEditMode() == EditMode.REALTIME ) {
                MidiPlayer.RestartMetronome();
            }
        }

        private void commonStripPaletteTool_Clicked( object sender, EventArgs e ) {
            String id = "";  //選択されたツールのID
            if ( sender is ToolStripButton ) {
                ToolStripButton tsb = (ToolStripButton)sender;
                if ( tsb.Tag != null && tsb.Tag is String ) {
                    id = (String)tsb.Tag;
                    AppManager.selectedPaletteTool = id;
                    AppManager.setSelectedTool( EditTool.PALETTE_TOOL );
                    tsb.Checked = true;
                }
            } else if ( sender is ToolStripMenuItem ) {
                ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
                if ( tsmi.Tag != null && tsmi.Tag is String ) {
                    id = (String)tsmi.Tag;
                    AppManager.selectedPaletteTool = id;
                    AppManager.setSelectedTool( EditTool.PALETTE_TOOL );
                    tsmi.Checked = true;
                }
            }

            foreach ( ToolStripItem item in toolStripTool.Items ) {
                if ( item is ToolStripButton ) {
                    ToolStripButton button = (ToolStripButton)item;
                    if ( button.Tag != null && button.Tag is String ) {
                        if ( ((String)button.Tag).Equals( id ) ) {
                            button.Checked = true;
                        } else {
                            button.Checked = false;
                        }
                    }
                }
            }
            foreach ( ToolStripItem item in cMenuPianoPaletteTool.DropDownItems ) {
                if ( item is ToolStripMenuItem ) {
                    ToolStripMenuItem menu = (ToolStripMenuItem)item;
                    if ( menu.Tag != null && menu.Tag is String ) {
                        if ( ((String)menu.Tag).Equals( id ) ) {
                            menu.Checked = true;
                        } else {
                            menu.Checked = false;
                        }
                    }
                }
            }
            foreach ( ToolStripItem item in cMenuTrackSelectorPaletteTool.DropDownItems ) {
                if ( item is ToolStripMenuItem ) {
                    ToolStripMenuItem menu = (ToolStripMenuItem)item;
                    if ( menu.Tag != null && menu.Tag is String ) {
                        if ( ((String)menu.Tag).Equals( id ) ) {
                            menu.Checked = true;
                        } else {
                            menu.Checked = false;
                        }
                    }
                }
            }
        }

        #region m_input_textbox
        private void m_input_textbox_KeyDown( object sender, KeyEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "m_input_textbox_KeyDown" );
            AppManager.debugWriteLine( "    e.KeyCode=" + e.KeyCode );
#endif
            if ( e.KeyCode == Keys.Tab || e.KeyCode == Keys.Return ) {
                ExecuteLyricChangeCommand();
                int selected = AppManager.getSelected();
                int index = -1;
                VsqTrack track = AppManager.getVsqFile().Track.get( selected );
                track.sortEvent();
                if ( e.KeyCode == Keys.Tab ) {
                    int clock = 0;
                    for ( int i = 0; i < track.getEventCount(); i++ ) {
                        VsqEvent item = track.getEvent( i );
                        if ( item.InternalID == AppManager.getLastSelectedEvent().original.InternalID ) {
                            index = i;
                            clock = item.Clock;
                            break;
                        }
                    }
                    if ( (e.Modifiers & Keys.Shift) == Keys.Shift ) {
                        // 1個前の音符イベントを検索
                        int tindex = -1;
                        for ( int i = track.getEventCount() - 1; i >= 0; i-- ) {
                            VsqEvent ve = track.getEvent( i );
                            if ( ve.ID.type == VsqIDType.Anote && i != index && ve.Clock <= clock ) {
                                tindex = i;
                                break;
                            }
                        }
                        index = tindex;
                    } else {
                        // 1個後の音符イベントを検索
                        int tindex = -1;
                        for ( int i = 0; i < track.getEventCount(); i++ ) {
                            VsqEvent ve = track.getEvent( i );
                            if ( ve.ID.type == VsqIDType.Anote && i != index && ve.Clock >= clock ) {
                                tindex = i;
                                break;
                            }
                        }
                        index = tindex;
                    }
                }
                if ( 0 <= index && index < track.getEventCount() ) {
                    AppManager.clearSelectedEvent();
                    VsqEvent item = track.getEvent( index );
                    AppManager.addSelectedEvent( item.InternalID );
                    int x = xCoordFromClocks( item.Clock );
                    int y = yCoordFromNote( item.ID.Note );
                    boolean phonetic_symbol_edit_mode = ((TagLyricTextBox)m_input_textbox.Tag).PhoneticSymbolEditMode;
                    ShowInputTextBox( item.ID.LyricHandle.L0.Phrase, 
                                            item.ID.LyricHandle.L0.getPhoneticSymbol(),
                                            new Point( x, y ),
                                            phonetic_symbol_edit_mode );
                    int clWidth = (int)(m_input_textbox.Width / AppManager.scaleX);
#if DEBUG
                    System.Diagnostics.Debug.WriteLine( "    clWidth=" + clWidth );
#endif
                    // 画面上にm_input_textboxが見えるように，移動
                    const int SPACE = 20;
                    if ( x < AppManager.KEY_LENGTH || pictPianoRoll.Width < x + m_input_textbox.Width ) {
                        int clock, clock_x;
                        if ( x < AppManager.KEY_LENGTH ) {
                            clock = item.Clock;
                            clock_x = AppManager.KEY_LENGTH + SPACE;
                        } else {
                            clock = item.Clock + clWidth;
                            clock_x = pictPianoRoll.Width - SPACE;
                        }
                        double draft_d = (73 - clock_x) / AppManager.scaleX + clock;
                        if ( draft_d < 0.0 ) {
                            draft_d = 0.0;
                        }
                        int draft = (int)draft_d;
                        if ( draft < hScroll.Minimum ) {
                            draft = hScroll.Minimum;
                        } else if ( hScroll.Maximum < draft ) {
                            draft = hScroll.Maximum;
                        }
                        hScroll.Value = draft;
                    } else {
                        refreshScreen();
                    }
                } else {
                    HideInputTextBox();
                }
            }
        }

        private void m_input_textbox_KeyUp( object sender, KeyEventArgs e ) {
            if ( (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) && (Control.ModifierKeys == Keys.Alt) ) {
                if ( m_input_textbox.Enabled ) {
                    FlipInputTextBoxMode();
                }
            } else if ( e.KeyCode == Keys.Escape ) {
                HideInputTextBox();
            }
        }
        
        private void m_input_textbox_ImeModeChanged( object sender, EventArgs e ) {
            m_last_imemode = m_input_textbox.ImeMode;
        }
        #endregion

        private void m_toolbar_edit_SelectedToolChanged( EditTool tool ) {
            AppManager.setSelectedTool( tool );
        }

        private void m_toolbar_measure_EndMarkerClick( object sender, EventArgs e ) {
            AppManager.endMarkerEnabled = !AppManager.endMarkerEnabled;
#if DEBUG
            AppManager.debugWriteLine( "m_toolbar_measure_EndMarkerClick" );
            AppManager.debugWriteLine( "    m_config.EndMarkerEnabled=" + AppManager.endMarkerEnabled );
#endif
            refreshScreen();
        }

        private void m_toolbar_measure_StartMarkerClick( object sender, EventArgs e ) {
            AppManager.startMarkerEnabled = !AppManager.startMarkerEnabled;
#if DEBUG
            AppManager.debugWriteLine( "m_toolbar_measure_StartMarkerClick" );
            AppManager.debugWriteLine( "    m_config.StartMarkerEnabled=" + AppManager.startMarkerEnabled );
#endif
            refreshScreen();
        }

        private void itm_Click( object sender, EventArgs e ) {
            if ( sender is ToolStripItem ) {
                ToolStripItem item = (ToolStripItem)sender;
                if ( item.Tag is String ) {
                    String filename = (String)item.Tag;
                    openVsqCor( filename );
                    refreshScreen();
                }
            }
        }

        private void itm_MouseEnter( object sender, EventArgs e ) {
            if ( sender is ToolStripItem ) {
                ToolStripItem item = (ToolStripItem)sender;
                statusLabel.Text = item.ToolTipText;
            }
        }

        #region AppManager
        private void AppManager_CurrentClockChanged( object sender, EventArgs e ) {
            stripLblBeat.Text = AppManager.getPlayPosition().Numerator + "/" + AppManager.getPlayPosition().Denominator;
            stripLblTempo.Text = (60e6 / (float)AppManager.getPlayPosition().Tempo).ToString( "#.00" );
            stripLblCursor.Text = AppManager.getPlayPosition().BarCount + " : " + AppManager.getPlayPosition().Beat + " : " + AppManager.getPlayPosition().Clock.ToString( "000" );
        }

        private void AppManager_GridVisibleChanged( object sender, EventArgs e ) {
            menuVisualGridline.Checked = AppManager.isGridVisible();
            stripBtnGrid.Checked = AppManager.isGridVisible();
            cMenuPianoGrid.Checked = AppManager.isGridVisible();
        }

        private void AppManager_PreviewAborted( object sender, EventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "AppManager_PreviewAborted" );
#endif
            if ( AppManager.getEditMode() == EditMode.REALTIME ) {
                AppManager.setEditMode( EditMode.NONE );
            }
            VSTiProxy.abortRendering();
            AppManager.firstBufferWritten = false;
            if ( m_midi_in != null ) {
                m_midi_in.Stop();
            }

            PlaySound.Reset();
            for ( int i = 0; i < m_draw_start_index.Length; i++ ) {
                m_draw_start_index[i] = 0;
            }
            MidiPlayer.Stop();
            timer.Stop();
        }

        private void AppManager_PreviewStarted( object sender, EventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "m_config_PreviewStarted" );
#endif
            PlaySound.Reset();
            VsqFileEx vsq = AppManager.getVsqFile();
            String renderer = vsq.Track.get( AppManager.getSelected() ).getCommon().Version;
            int clock = AppManager.getCurrentClock();
            m_direct_play_shift = (float)vsq.getSecFromClock( clock );
            if ( AppManager.getEditMode() != EditMode.REALTIME ) {
                int selected = AppManager.getSelected();
                String tmppath = AppManager.getTempWaveDir();

                double amp_master = VocaloSysUtil.getAmplifyCoeffFromFeder( vsq.Mixer.MasterFeder );
                double pan_left_master = VocaloSysUtil.getAmplifyCoeffFromPanLeft( vsq.Mixer.MasterPanpot );
                double pan_right_master = VocaloSysUtil.getAmplifyCoeffFromPanRight( vsq.Mixer.MasterPanpot );

                // 選択されたトラック以外のレンダリングを行う
                Vector<Int32> render_all = new Vector<Int32>();
                int track_count = vsq.Track.size();
                for ( int track = 1; track < track_count; track++ ) {
                    if ( track == selected || !(vsq.Track.get( track ).getCommon().PlayMode >= 0) ) {
                        continue;
                    }
                    String file = Path.Combine( tmppath, track + ".wav" );
                    if ( !PortUtil.isFileExists( file ) ) {
                        render_all.add( track );
                    }
                }
                if ( render_all.size() > 0 ) {
                    Render( render_all.toArray( new Int32[]{} ) );
                }

                Vector<WaveReader> sounds = new Vector<WaveReader>();
                track_count = vsq.Track.size();
                for ( int track = 1; track < track_count; track++ ) {
                    VsqTrack vsq_track = vsq.Track.get( track );
                    if ( track == selected || !(vsq_track.getCommon().PlayMode >= 0) ) {
                        continue;
                    }

                    String file = Path.Combine( tmppath, track + ".wav" );
                    String tmpfile = Path.Combine( tmppath, "temp.wav" );
                    int t_start = vsq_track.getEditedStart();
                    int t_end = vsq_track.getEditedEnd();
                    int start = t_start;
                    int end = t_end;

                    // 編集が施された範囲に存在している音符を探し、（音符の末尾と次の音符の先頭の接続を無視した場合の）最長一致範囲を決める
                    int index_start = -1; //startから始まっている音符のインデックス
                    int event_count = vsq_track.getEventCount();
                    for ( int i = 0; i < event_count; i++ ) {
                        VsqEvent item = vsq_track.getEvent( i );
                        if ( item.Clock <= t_start && t_start <= item.Clock + item.ID.Length ) {
                            start = item.Clock;
                            index_start = i;
                            break;
                        }
                    }
                    int index_end = -1;
                    for ( int i = event_count - 1; i >= 0; i-- ) {
                        VsqEvent item = vsq_track.getEvent( i );
                        if ( item.Clock <= t_end && t_end <= item.Clock + item.ID.Length ) {
                            end = item.Clock + item.ID.Length;
                            index_end = i;
                            break;
                        }
                    }

                    // 音符の末尾と次の音符の先頭がつながっている場合、レンダリング範囲を広げる
                    if ( index_start >= 0 ) {
                        for ( int i = index_start - 1; i >= 0; i-- ) {
                            VsqEvent ve = vsq_track.getEvent( i );
                            if ( ve.ID.type == VsqIDType.Anote ) {
                                int endpoint = ve.Clock + ve.ID.Length;
                                if ( endpoint == start ) {
                                    start = ve.Clock;
                                    index_start = i;
                                } else if ( endpoint < start ) {
                                    break;
                                }
                            }
                        }
                    }
                    if ( index_end >= 0 ) {
                        for ( int i = index_end + 1; i < event_count; i++ ) {
                            VsqEvent ve = vsq_track.getEvent( i );
                            if ( ve.ID.type == VsqIDType.Anote ) {
                                int startpoint = ve.Clock;
                                if ( end == ve.Clock ) {
                                    end = ve.Clock + ve.ID.Length;
                                    index_end = i;
                                } else if ( end < startpoint ) {
                                    break;
                                }
                            }
                        }
                    }

                    if ( start < end ) {
#if DEBUG
                        AppManager.debugWriteLine( "    partial rendering!" );
#endif
                        int temp_premeasure = AppManager.getVsqFile().getPresendClockAt( start, AppManager.editorConfig.PreSendTime ) * 2;
                        boolean successed = false;
                        using ( FormSynthesize dlg = new FormSynthesize( vsq,
                                                                         AppManager.editorConfig.PreSendTime,
                                                                         track,
                                                                         tmpfile,
                                                                         start,
                                                                         end,
                                                                         temp_premeasure,
                                                                         false ) ) {
                            if ( dlg.ShowDialog() == DialogResult.OK ) {
                                successed = true;
                            }
                        }
                        if ( successed ) {
                            vsq_track.resetEditedArea();
                            using ( Wave main = new Wave( file ) )
                            using ( Wave temp = new Wave( tmpfile ) ) {
                                double sec_start = vsq.getSecFromClock( start );
                                double sec_end = vsq.getSecFromClock( end );

                                main.Replace( temp,
                                              0,
                                              (uint)(sec_start * main.SampleRate),
                                              (uint)((sec_end - sec_start) * temp.SampleRate) );
                                main.Write( file );
                            }
                        }
                    }

                    WaveReader wr = new WaveReader( file );
                    wr.Tag = track;
                    sounds.add( wr );
                }

                // リアルタイム再生用のデータを準備
                m_preview_ending_time = vsq.getSecFromClock( AppManager.getVsqFile().TotalClocks ) + 1.0;

                // clock以降に音符があるかどうかを調べる
                int count = 0;
                for ( Iterator itr = vsq.Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                    VsqEvent ve = (VsqEvent)itr.next();
                    if ( ve.Clock >= clock ) {
                        count++;
                        break;
                    }
                }

                int bgm_count = AppManager.getBgmCount();
                double pre_measure_sec = vsq.getSecFromClock( vsq.getPreMeasureClocks() );
                for ( int i = 0; i < bgm_count; i++ ) {
                    BgmFile bgm = AppManager.getBgm( i );
                    WaveReader wr = new WaveReader( bgm.file );
                    wr.Tag = (int)(-i - 1);
                    double offset = bgm.readOffsetSeconds;
                    if ( bgm.startAfterPremeasure ) {
                        offset -= pre_measure_sec;
                    }
                    wr.OffsetSeconds = offset;
                    sounds.add( wr );
                }

                if ( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().PlayMode >= 0 && count > 0 ) {
                    int ms_presend = AppManager.editorConfig.PreSendTime;
                    if ( renderer.StartsWith( VSTiProxy.RENDERER_UTU0 ) ) {
                        ms_presend = 0;
                    }
#if DEBUG
                    AppManager.debugWriteLine( "m_preview_ending_time=" + m_preview_ending_time );
#endif
                    VSTiProxy.render( AppManager.getVsqFile(),
                                      selected,
                                      null,
                                      m_direct_play_shift,
                                      m_preview_ending_time,
                                      ms_presend,
                                      true,
                                      sounds.toArray( new WaveReader[]{} ),
                                      m_direct_play_shift,
                                      true,
                                      AppManager.getTempWaveDir(),
                                      false );

                    for ( int i = 0; i < m_draw_start_index.Length; i++ ) {
                        m_draw_start_index[i] = 0;
                    }
                    int clock_now = AppManager.getCurrentClock();
                    double sec_now = AppManager.getVsqFile().getSecFromClock( clock_now );
                } else {
                    VSTiProxy.render( new VsqFileEx( "Miku", AppManager.getVsqFile().getPreMeasure(), 4, 4, 500000 ),
                                      1,
                                      null,
                                      0,
                                      m_preview_ending_time,
                                      AppManager.editorConfig.PreSendTime,
                                      true,
                                      sounds.toArray( new WaveReader[]{} ),
                                      m_direct_play_shift,
                                      true,
                                      AppManager.getTempWaveDir(),
                                      false );
                }
            }

            m_last_ignitted = DateTime.Now;
            if ( AppManager.getEditMode() == EditMode.REALTIME ) {
                if ( m_midi_in != null ) {
                    m_midi_in.Start();
                }
                AppManager.rendererAvailable = false;
                MidiPlayer.SetSpeed( AppManager.editorConfig.RealtimeInputSpeed, m_last_ignitted );
                MidiPlayer.Start( AppManager.getVsqFile(), clock, m_last_ignitted );
            } else {
                AppManager.rendererAvailable = VSTiProxy.isRendererAvailable( renderer );
            }
            AppManager.firstBufferWritten = true;
            AppManager.previewStartedTime = m_last_ignitted;
#if DEBUG
            AppManager.debugWriteLine( "    m_config.VsqFile.TotalClocks=" + AppManager.getVsqFile().TotalClocks );
            AppManager.debugWriteLine( "    total seconds=" + AppManager.getVsqFile().getSecFromClock( (int)AppManager.getVsqFile().TotalClocks ) );
#endif
            timer.Enabled = true;
        }

        private void AppManager_SelectedToolChanged( object sender, EventArgs e ) {
            applySelectedTool();
        }

        private void AppManager_SelectedEventChanged( boolean selected_event_is_null ) {
            menuEditCut.Enabled = !selected_event_is_null;
            menuEditPaste.Enabled = !selected_event_is_null;
            menuEditDelete.Enabled = !selected_event_is_null;
            cMenuPianoCut.Enabled = !selected_event_is_null;
            cMenuPianoCopy.Enabled = !selected_event_is_null;
            cMenuPianoDelete.Enabled = !selected_event_is_null;
            cMenuPianoExpressionProperty.Enabled = !selected_event_is_null;
        }
        #endregion

        #region pictPianoRoll
        private void pictPianoRoll_MouseClick( object sender, MouseEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "pictPianoRoll_MouseClick" );
#endif
            Keys modefier = Control.ModifierKeys;
            if ( e.Button == MouseButtons.Left ) {
                if ( m_mouse_hover_thread != null ) {
                    m_mouse_hover_thread.Abort();
                }

                // クリック位置にIDが無いかどうかを検査
                Rectangle id_rect;
                VsqEvent item = GetItemAtClickedPosition( e.Location, out id_rect );
#if DEBUG
                AppManager.debugWriteLine( "    (item==null)=" + (item == null) );
#endif
                if ( item != null && 
                     AppManager.getEditMode() != EditMode.MOVE_ENTRY_WAIT_MOVE && 
                     AppManager.getEditMode() != EditMode.MOVE_ENTRY && 
                     AppManager.getEditMode() != EditMode.EDIT_LEFT_EDGE && 
                     AppManager.getEditMode() != EditMode.EDIT_RIGHT_EDGE &&
                     AppManager.getEditMode() != EditMode.MIDDLE_DRAG ) {
                    if ( (modefier & Keys.Shift) != Keys.Shift && (modefier & s_modifier_key) != s_modifier_key ) {
                        AppManager.clearSelectedEvent();
                    }
                    AppManager.addSelectedEvent( item.InternalID );
                    int selected = AppManager.getSelected();
                    int internal_id = item.InternalID;
                    HideInputTextBox();
                    if ( AppManager.getSelectedTool() == EditTool.ERASER ) {
                        CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventDelete( selected, internal_id ) );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                        Edited = true;
                        AppManager.clearSelectedEvent();
                        return;
                    } else if ( AppManager.getSelectedTool() == EditTool.PALETTE_TOOL ) {
                        Vector<Int32> internal_ids = new Vector<Int32>();
                        for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                            SelectedEventEntry see = (SelectedEventEntry)itr.next();
                            internal_ids.add( see.original.InternalID );
                        }
                        MouseButtons btn = e.Button;
                        if ( m_spacekey_downed ) {
                            btn = MouseButtons.Middle;
                        }
                        boolean result = PaletteToolServer.InvokePaletteTool( AppManager.selectedPaletteTool,
                                                                           AppManager.getSelected(),
                                                                           internal_ids.toArray( new Int32[]{} ),
                                                                           btn );
                        if ( result ) {
                            Edited = true;
                            AppManager.clearSelectedEvent();
                            return;
                        }
                    }
                } else {
                    if ( AppManager.getEditMode() != EditMode.MOVE_ENTRY_WAIT_MOVE &&
                            AppManager.getEditMode() != EditMode.MOVE_ENTRY && 
                            AppManager.getEditMode() != EditMode.EDIT_LEFT_EDGE && 
                            AppManager.getEditMode() != EditMode.EDIT_RIGHT_EDGE ) {
                        if ( !m_pointer_downed ) {
                            AppManager.clearSelectedEvent();
                        }
                        HideInputTextBox();
                    }
                    if ( AppManager.getSelectedTool() == EditTool.ERASER ) {
                        // マウス位置にビブラートの波波があったら削除する
                        int stdx = AppManager.startToDrawX;
                        int stdy = StartToDrawY;
#if USE_DOBJ
                        for ( int i = 0; i < m_draw_objects.get( AppManager.getSelected() - 1 ).size(); i++ ) {
                            DrawObject dobj = m_draw_objects.get( AppManager.getSelected() - 1 ).get( i );
                            if ( dobj.pxRectangle.X + dobj.pxRectangle.Width - stdx < 0 ) {
                                continue;
                            } else if ( pictPianoRoll.Width < dobj.pxRectangle.X - stdx ) {
                                break;
                            }
                            Rectangle rc = new Rectangle( dobj.pxRectangle.X + dobj.pxVibratoDelay - stdx,
                                                          dobj.pxRectangle.Y + AppManager.editorConfig.PxTrackHeight - stdy,
                                                          dobj.pxRectangle.Width - dobj.pxVibratoDelay,
                                                          AppManager.editorConfig.PxTrackHeight );
#else
                        VsqTrack vsq_track = AppManager.VsqFile.getTrack( AppManager.Selected );
                        float scalex = AppManager.ScaleX;
                        for( Iterator itr0 = vsq_track.getNoteEventIterator(); itr0.hasNext();){
                            VsqEvent evnt = (VsqEvent)itr0.next();
                            if ( evnt.ID.VibratoHandle == null ){
                                continue;
                            }
                            int event_sx = XCoordFromClocks( evnt.Clock );
                            int event_ex = XCoordFromClocks( evnt.Clock + evnt.ID.Length);
                            int vib_sx = XCoordFromClocks( evnt.Clock + evnt.ID.VibratoDelay);
                            Rectangle rc = new Rectangle( vib_sx,
                                                          YCoordFromNote( evnt.ID.Note, stdy ),
                                                          event_ex - vib_sx,
                                                          AppManager.EditorConfig.PxTrackHeight );
#endif
                            if ( IsInRect( e.Location, rc ) ) {
                                //ビブラートの範囲なのでビブラートを消す
                                VsqID item2 = null;
                                int internal_id = -1;
#if USE_DOBJ
                                internal_id = dobj.InternalID;
                                for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ve = (VsqEvent)itr.next();
                                    if ( ve.InternalID == dobj.InternalID ) {
                                        item2 = (VsqID)ve.ID.clone();
                                        break;
                                    }
                                }
#else
                                item2 = evnt.ID;
                                internal_id = evnt.InternalID;
#endif
                                if ( item2 != null ) {
                                    item2.VibratoHandle = null;
                                    CadenciiCommand run = new CadenciiCommand(
                                        VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(),
                                                                                          internal_id,
                                                                                          item2 ) );
                                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                    Edited = true;
                                }
                                break;
                            }
                        }
                    }
                }
            } else if ( e.Button == MouseButtons.Right ) {
                boolean show_context_menu = (e.X > AppManager.KEY_LENGTH);
                if ( m_mouse_hover_thread != null ) {
                    if ( !m_mouse_hover_thread.IsAlive && AppManager.editorConfig.PlayPreviewWhenRightClick ) {
                        show_context_menu = false;
                    }
                } else {
                    if ( AppManager.editorConfig.PlayPreviewWhenRightClick ) {
                        show_context_menu = false;
                    }
                }
                show_context_menu = show_context_menu && !m_mouse_moved;
                if ( show_context_menu ) {
                    if ( m_mouse_hover_thread != null ) {
                        m_mouse_hover_thread.Abort();
                    }
                    Rectangle id_rect;
                    VsqEvent item = GetItemAtClickedPosition( e.Location, out id_rect );
                    if ( item != null ) {
                        if ( !AppManager.isSelectedEventContains( AppManager.getSelected(), item.InternalID ) ) {
                            AppManager.clearSelectedEvent();
                        }
                        AppManager.addSelectedEvent( item.InternalID );
                    }
                    boolean item_is_null = (item == null);
                    cMenuPianoCopy.Enabled = !item_is_null;
                    cMenuPianoCut.Enabled = !item_is_null;
                    cMenuPianoDelete.Enabled = !item_is_null;
                    cMenuPianoImportLyric.Enabled = !item_is_null;
                    cMenuPianoExpressionProperty.Enabled = !item_is_null;

                    int clock = clockFromXCoord( e.X );
                    cMenuPianoPaste.Enabled = ((AppManager.getCopiedItems().events.size() != 0) && (clock >= AppManager.getVsqFile().getPreMeasureClocks()));
                    refreshScreen();

                    m_cMenuOpenedPosition = e.Location;
                    cMenuPiano.Show( pictPianoRoll, e.Location );
                } else {
                    Rectangle id_rect;
                    VsqEvent item = GetItemAtClickedPosition( m_button_initial, out id_rect );
#if DEBUG
                    AppManager.debugWriteLine( "pitcPianoRoll_MouseClick; button is right; (item==null)=" + (item == null) );
#endif
                    if ( item != null ) {
                        int itemx = xCoordFromClocks( item.Clock );
                        int itemy = yCoordFromNote( item.ID.Note );
                    }
                }
            } else if ( e.Button == MouseButtons.Middle ) {
                if ( AppManager.getSelectedTool() == EditTool.PALETTE_TOOL ) {
                    Rectangle id_rect;
                    VsqEvent item = GetItemAtClickedPosition( e.Location, out id_rect );
                    if ( item != null ) {
                        if ( AppManager.getSelectedTool() == EditTool.PALETTE_TOOL ) {
                            AppManager.clearSelectedEvent();
                            AppManager.addSelectedEvent( item.InternalID );
                            Vector<Int32> internal_ids = new Vector<Int32>();
                            for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                                SelectedEventEntry see = (SelectedEventEntry)itr.next();
                                internal_ids.add( see.original.InternalID );
                            }
                            boolean result = PaletteToolServer.InvokePaletteTool( AppManager.selectedPaletteTool,
                                                                               AppManager.getSelected(),
                                                                               internal_ids.toArray( new Int32[]{} ),
                                                                               e.Button );
                            if ( result ) {
                                Edited = true;
                                AppManager.clearSelectedEvent();
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void pictPianoRoll_MouseDoubleClick( object sender, MouseEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "pictureBox1_MouseDoubleClick" );
#endif
            Rectangle rect;
            VsqEvent item = GetItemAtClickedPosition( e.Location, out rect );
            if ( item != null ) {
                if ( AppManager.getSelectedTool() != EditTool.PALETTE_TOOL ) {
                    AppManager.clearSelectedEvent();
                    AppManager.addSelectedEvent( item.InternalID );
                    m_mouse_hover_thread.Abort();
                    if ( !AppManager.editorConfig.KeepLyricInputMode ) {
                        m_last_symbol_edit_mode = false;
                    }
                    ShowInputTextBox( item.ID.LyricHandle.L0.Phrase, item.ID.LyricHandle.L0.getPhoneticSymbol(), rect.Location, m_last_symbol_edit_mode );
                    refreshScreen();
                    return;
                }
            } else {
                AppManager.clearSelectedEvent();
                HideInputTextBox();
                if ( AppManager.editorConfig.ShowExpLine && AppManager.KEY_LENGTH <= e.X ) {
                    int stdx = AppManager.startToDrawX;
                    int stdy = StartToDrawY;
#if USE_DOBJ
                    for ( Iterator itr = m_draw_objects.get( AppManager.getSelected() - 1 ).iterator() ;itr.hasNext(); ){
                        DrawObject dobj = (DrawObject)itr.next();
                        // 表情コントロールプロパティを表示するかどうかを決める
                        rect = new Rectangle(
                            dobj.pxRectangle.X - stdx,
                            dobj.pxRectangle.Y - stdy + AppManager.editorConfig.PxTrackHeight,
                            21,
                            AppManager.editorConfig.PxTrackHeight );
#else
                    for( Iterator itr = AppManager.VsqFile.getTrack( AppManager.Selected ).getNoteEventIterator(); itr.hasNext(); ){
                        VsqEvent evnt = (VsqEvent)itr.next();
                        int event_ex = XCoordFromClocks( evnt.Clock + evnt.ID.Length );
                        if ( event_ex < 0 ) {
                            continue;
                        }
                        int event_sx = XCoordFromClocks( evnt.Clock );
                        if ( pictPianoRoll.Width < event_sx ) {
                            break;
                        }
                        int vib_sx = XCoordFromClocks( evnt.Clock + evnt.ID.VibratoDelay);
                        int event_sy = YCoordFromNote( evnt.ID.Note, stdy );
                        rect = new Rectangle( event_sx, event_sy, 21, AppManager.EditorConfig.PxTrackHeight );
#endif
                        if ( IsInRect( e.Location, rect ) ) {
                            VsqEvent selected = null;
#if USE_DOBJ
                            for ( Iterator itr2 = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventIterator(); itr2.hasNext(); ) {
                                VsqEvent ev = (VsqEvent)itr2.next();
                                if ( ev.InternalID == dobj.InternalID ) {
                                    selected = ev;
                                    break;
                                }
                            }
#else
                            selected = evnt;
#endif
                            if ( selected != null ) {
                                if ( m_mouse_hover_thread != null ) {
                                    m_mouse_hover_thread.Abort();
                                }
                                SynthesizerType type = SynthesizerType.VOCALOID2;
                                if ( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                                    type = SynthesizerType.VOCALOID1;
                                }
                                using ( FormNoteExpressionConfig dlg = new FormNoteExpressionConfig( type, selected.ID.NoteHeadHandle ) ) {
                                    dlg.PMBendDepth = selected.ID.PMBendDepth;
                                    dlg.PMBendLength = selected.ID.PMBendLength;
                                    dlg.PMbPortamentoUse = selected.ID.PMbPortamentoUse;
                                    dlg.DEMdecGainRate = selected.ID.DEMdecGainRate;
                                    dlg.DEMaccent = selected.ID.DEMaccent;
                                    dlg.Location = GetFormPreferedLocation( dlg );
                                    if ( dlg.ShowDialog() == DialogResult.OK ) {
                                        VsqID id = (VsqID)selected.ID.clone();
                                        id.PMBendDepth = dlg.PMBendDepth;
                                        id.PMBendLength = dlg.PMBendLength;
                                        id.PMbPortamentoUse = dlg.PMbPortamentoUse;
                                        id.DEMdecGainRate = dlg.DEMdecGainRate;
                                        id.DEMaccent = dlg.DEMaccent;
                                        id.NoteHeadHandle = dlg.getEditedNoteHeadHandle();
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(), selected.InternalID, id ) );
                                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                        Edited = true;
                                        refreshScreen();
                                    }
                                }
                                return;
                            }
                            break;
                        }

                        // ビブラートプロパティダイアログを表示するかどうかを決める
#if USE_DOBJ
                        rect = new Rectangle( dobj.pxRectangle.X - stdx + 21,
                                              dobj.pxRectangle.Y - stdy + AppManager.editorConfig.PxTrackHeight,
                                              dobj.pxRectangle.Width - 21,
                                              AppManager.editorConfig.PxTrackHeight );
#else
                        if ( evnt.ID.VibratoHandle == null ){
                            continue;
                        }
                        rect = new Rectangle( event_sx + 21, 
                                              event_sy + AppManager.EditorConfig.PxTrackHeight,
                                              event_ex - event_sx - 21,
                                              AppManager.EditorConfig.PxTrackHeight );
#endif
                        if ( IsInRect( e.Location, rect ) ) {
                            VsqEvent selected = null;
#if USE_DOBJ
                            for ( Iterator itr2 = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventIterator(); itr2.hasNext(); ) {
                                VsqEvent ev = (VsqEvent)itr2.next();
                                if ( ev.InternalID == dobj.InternalID ) {
                                    selected = ev;
                                    break;
                                }
                            }
#else
                            selected = evnt;
#endif
                            if ( selected != null ) {
                                if ( m_mouse_hover_thread != null ) {
                                    m_mouse_hover_thread.Abort();
                                }
                                SynthesizerType type = SynthesizerType.VOCALOID2;
#if DEBUG
                                Console.WriteLine( "FormMain#pictPianoRoll_MouseDoubleClick; version=" + AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version );
#endif
                                if ( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                                    type = SynthesizerType.VOCALOID1;
                                }
                                using ( FormVibratoConfig dlg = new FormVibratoConfig( selected.ID.VibratoHandle, selected.ID.Length, AppManager.editorConfig.DefaultVibratoLength, type ) ) {
                                    dlg.Location = GetFormPreferedLocation( dlg );
                                    if ( dlg.ShowDialog() == DialogResult.OK ) {
                                        VsqID t = (VsqID)selected.ID.clone();
                                        t.VibratoHandle = dlg.VibratoHandle;
                                        if ( t.VibratoHandle != null ) {
                                            int vibrato_length = t.VibratoHandle.Length;
                                            int note_length = selected.ID.Length;
                                            t.VibratoDelay = note_length - vibrato_length;
                                        }
                                        CadenciiCommand run = new CadenciiCommand( 
                                            VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(),
                                                                                    selected.InternalID,
                                                                                    t ) );
                                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                        Edited = true;
                                        refreshScreen();
                                    }
                                }
                                return;
                            }
                            break;
                        }

                    }
                }
            }

            // 必要な操作が何も無ければ，クリック位置にソングポジションを移動
            if ( e.Button == MouseButtons.Left && AppManager.KEY_LENGTH < e.X ) {
                int unit = AppManager.getPositionQuantizeClock();
                int clock = clockFromXCoord( e.X );
                int odd = clock % unit;
                clock -= odd;
                if ( odd > unit / 2 ) {
                    clock += unit;
                }
                AppManager.setCurrentClock( clock );
            }
        }

        private void pictPianoRoll_MouseDown( object sender, MouseEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "pictPianoRoll_MouseDown" );
#endif
            if ( AppManager.getEditMode() == EditMode.REALTIME ) {
                return;
            }

            m_mouse_moved = false;
            if ( !AppManager.isPlaying() && 0 <= e.X && e.X <= AppManager.KEY_LENGTH ) {
                int note = noteFromYCoord( e.Y );
                if ( 0 <= note && note <= 126 ) {
                    if ( e.Button == MouseButtons.Left ) {
                        KeySoundPlayer.Play( note );
                    }
                    return;
                }
            }

            AppManager.clearSelectedTempo();
            AppManager.clearSelectedTimesig();
            AppManager.clearSelectedPoint();
            if ( e.Button == MouseButtons.Left ) {
                AppManager.selectedRegionEnabled = false;
            }

            m_mouse_downed = true;
            m_button_initial = e.Location;
            Keys modefier = Control.ModifierKeys;
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }


            if ( e.Button == MouseButtons.Middle || (e.Button != MouseButtons.Middle && m_spacekey_downed) ) {
                AppManager.setEditMode( EditMode.MIDDLE_DRAG );
                m_middle_button_vscroll = vScroll.Value;
                m_middle_button_hscroll = hScroll.Value;
                return;
            }

            Rectangle rect;
            VsqEvent item = GetItemAtClickedPosition( e.Location, out rect );

            EditTool selected_tool = AppManager.getSelectedTool();

            // マウス位置にある音符を検索
            if ( item == null ) {
                #region 音符がなかった時
#if DEBUG
                AppManager.debugWriteLine( "    No Event" );
#endif
                if ( AppManager.getLastSelectedEvent() != null ) {
                    ExecuteLyricChangeCommand();
                }
                boolean start_mouse_hover_generator = true;
                if ( (modefier & s_modifier_key) == s_modifier_key ) {
                    AppManager.selectedRegionEnabled = true;
                    int stdx = AppManager.startToDrawX;
                    int x = e.X + stdx;
                    if ( AppManager.editorConfig.CurveSelectingQuantized ) {
                        int clock = clockFromXCoord( e.X );
                        int unit = AppManager.getPositionQuantizeClock();
                        int odd = clock % unit;
                        int nclock = clock;
                        nclock -= odd;
                        if ( odd > unit / 2 ) {
                            nclock += unit;
                        }
                        x = xCoordFromClocks( nclock ) + stdx;
                    }
                    AppManager.selectedRegion = new SelectedRegion( x );
                    AppManager.selectedRegion.SetEnd( x );
                    m_pointer_downed = true;
                } else {
                    boolean vibrato_found = false;
                    if ( selected_tool == EditTool.LINE || selected_tool == EditTool.PENCIL ) {
                        // ビブラート範囲の編集
                        int px_vibrato_length = 0;
                        int stdx = AppManager.startToDrawX;
                        int stdy = StartToDrawY;
                        m_vibrato_editing_id = -1;
#if USE_DOBJ
                        Rectangle pxFound = new Rectangle();
                        for ( int i = 0; i < m_draw_objects.get( AppManager.getSelected() - 1 ).size(); i++ ) {
                            DrawObject dobj = m_draw_objects.get( AppManager.getSelected() - 1 ).get( i );
                            if ( dobj.pxRectangle.Width <= dobj.pxVibratoDelay ) {
                                continue;
                            }
                            if ( dobj.pxRectangle.X + dobj.pxRectangle.Width - stdx < 0 ) {
                                continue;
                            } else if ( pictPianoRoll.Width < dobj.pxRectangle.X - stdx ) {
                                break;
                            }
                            Rectangle rc = new Rectangle( dobj.pxRectangle.X + dobj.pxVibratoDelay - stdx - _EDIT_HANDLE_WIDTH / 2,
                                                dobj.pxRectangle.Y + AppManager.editorConfig.PxTrackHeight - stdy,
                                                _EDIT_HANDLE_WIDTH,
                                                AppManager.editorConfig.PxTrackHeight );
#else
                        int clock = 0;
                        int note = 0;
                        int length = 0;
                        for ( Iterator itr = AppManager.VsqFile.getTrack( AppManager.Selected ).getNoteEventIterator(); itr.hasNext(); ){
                            VsqEvent evnt = (VsqEvent)itr.next();
                            if ( evnt.ID.VibratoHandle == null ){
                                continue;
                            }
                            int event_ex = XCoordFromClocks( evnt.Clock + evnt.ID.Length );
                            if ( event_ex < 0 ) {
                                continue;
                            }
                            int event_sx = XCoordFromClocks( evnt.Clock );
                            if ( pictPianoRoll.Width < event_sx ) {
                                break;
                            }
                            int vib_sx = XCoordFromClocks( evnt.Clock + evnt.ID.VibratoDelay );
                            Rectangle rc = new Rectangle( event_sx - _EDIT_HANDLE_WIDTH / 2,
                                                          YCoordFromNote( evnt.ID.Note ) + AppManager.EditorConfig.PxTrackHeight,
                                                          _EDIT_HANDLE_WIDTH,
                                                          AppManager.EditorConfig.PxTrackHeight );
#endif
                            if ( IsInRect( e.Location, rc ) ) {
                                vibrato_found = true;
#if USE_DOBJ
                                m_vibrato_editing_id = dobj.InternalID;
                                pxFound = dobj.pxRectangle;
                                px_vibrato_length = dobj.pxRectangle.Width - dobj.pxVibratoDelay;
#else
                                m_vibrato_editing_id = evnt.InternalID;
                                clock = evnt.Clock + evnt.ID.VibratoDelay;
                                note = evnt.ID.Note - 1;
                                length = evnt.ID.Length;
                                px_vibrato_length = event_ex - vib_sx;
#endif
                                break;
                            }
                        }
                        if ( vibrato_found ) {
#if USE_DOBJ
                            int clock = clockFromXCoord( pxFound.X + pxFound.Width - px_vibrato_length - stdx );
                            int note = noteFromYCoord( pxFound.Y + AppManager.editorConfig.PxTrackHeight - stdy );
                            int length = (int)(pxFound.Width / AppManager.scaleX);
#endif
                            m_adding = new VsqEvent( clock, new VsqID( 0 ) );
                            m_adding.ID.type = VsqIDType.Anote;
                            m_adding.ID.Note = note;
                            m_adding.ID.Length = (int)(px_vibrato_length / AppManager.scaleX);
                            m_adding_length = length;
                            m_adding.ID.VibratoDelay = length - (int)(px_vibrato_length / AppManager.scaleX);
                            AppManager.setEditMode( EditMode.EDIT_VIBRATO_DELAY );
                            start_mouse_hover_generator = false;
                        }
                    }
                    if ( !vibrato_found ) {
                        if ( (selected_tool == EditTool.PENCIL || selected_tool == EditTool.LINE) &&
                            e.Button == MouseButtons.Left &&
                            e.X >= AppManager.KEY_LENGTH ) {
                            int clock = clockFromXCoord( e.X );
                            if ( AppManager.getVsqFile().getPreMeasureClocks() - AppManager.editorConfig.PxTolerance / AppManager.scaleX <= clock ) { //10ピクセルまでは許容範囲
                                if ( AppManager.getVsqFile().getPreMeasureClocks() > clock ) { //だけど矯正するよ。
                                    clock = AppManager.getVsqFile().getPreMeasureClocks();
                                }
                                int note = noteFromYCoord( e.Y );
                                AppManager.clearSelectedEvent();
                                int unit = AppManager.getPositionQuantizeClock();
                                int odd = clock % unit;
                                int new_clock = clock - odd;
                                if ( odd > unit / 2 ) {
                                    new_clock += unit;
                                }
                                m_adding = new VsqEvent( new_clock, new VsqID( 0 ) );
                                m_adding.ID.PMBendDepth = AppManager.editorConfig.DefaultPMBendDepth;
                                m_adding.ID.PMBendLength = AppManager.editorConfig.DefaultPMBendLength;
                                m_adding.ID.PMbPortamentoUse = AppManager.editorConfig.DefaultPMbPortamentoUse;
                                m_adding.ID.DEMdecGainRate = AppManager.editorConfig.DefaultDEMdecGainRate;
                                m_adding.ID.DEMaccent = AppManager.editorConfig.DefaultDEMaccent;
                                if ( m_pencil_mode.Mode == PencilModeEnum.Off ) {
                                    AppManager.setEditMode( EditMode.ADD_ENTRY );
                                    m_button_initial = e.Location;
                                    m_adding.ID.Length = 0;
                                    m_adding.ID.Note = note;
                                    Cursor = Cursors.Arrow;
#if DEBUG
                                    AppManager.debugWriteLine( "    EditMode=" + AppManager.getEditMode() );
#endif
                                } else {
                                    AppManager.setEditMode( EditMode.ADD_FIXED_LENGTH_ENTRY );
                                    m_adding.ID.Length = m_pencil_mode.GetUnitLength();
                                    m_adding.ID.Note = note;
                                    Cursor = Cursors.Arrow;
                                }
                            } else {
                                SystemSounds.Asterisk.Play();
                            }
                        } else if ( (selected_tool == EditTool.ARROW || selected_tool == EditTool.PALETTE_TOOL) && e.Button == MouseButtons.Left ) {
                            AppManager.selectedRegionEnabled = false;
                            AppManager.clearSelectedEvent();
                            m_pointer_mouse_down = new Point( e.X + AppManager.startToDrawX, e.Y + StartToDrawY );
                            m_pointer_downed = true;
#if DEBUG
                            AppManager.debugWriteLine( "    EditMode=" + AppManager.getEditMode() );
#endif
                        }
                    }
                }
                if ( e.Button == MouseButtons.Right && !AppManager.editorConfig.PlayPreviewWhenRightClick ) {
                    start_mouse_hover_generator = false;
                }
                if ( start_mouse_hover_generator ) {
                    m_mouse_hover_thread = new Thread( new ParameterizedThreadStart( MouseHoverEventGenerator ) );
                    m_mouse_hover_thread.Start( noteFromYCoord( e.Y ) );
                }
                #endregion
            } else {
                #region 音符があった時
#if DEBUG
                AppManager.debugWriteLine( "    Event Found" );
#endif
                if ( selected_tool != EditTool.ERASER ) {
                    m_mouse_hover_thread = new Thread( new ParameterizedThreadStart( MouseHoverEventGenerator ) );
                    m_mouse_hover_thread.Start( item.ID.Note );
                }
                // まず、両端の編集モードに移行可能かどうか調べる
                if ( selected_tool != EditTool.ERASER && selected_tool != EditTool.PALETTE_TOOL && e.Button == MouseButtons.Left ) {
                    int stdx = AppManager.startToDrawX;
                    int stdy = StartToDrawY;
#if USE_DOBJ
                    for ( Iterator itr = m_draw_objects.get( AppManager.getSelected() - 1 ).iterator(); itr.hasNext(); ){
                        DrawObject dobj = (DrawObject)itr.next();
                        Rectangle rc = new Rectangle( dobj.pxRectangle.X - stdx, dobj.pxRectangle.Y - stdy, _EDIT_HANDLE_WIDTH, dobj.pxRectangle.Height );
#else
                    for( Iterator itr = AppManager.VsqFile.getTrack( AppManager.Selected ).getNoteEventIterator(); itr.hasNext(); ){
                        VsqEvent evnt = (VsqEvent)itr.next();
                        int event_ex = XCoordFromClocks( evnt.Clock + evnt.ID.Length );
                        if ( event_ex < 0 ) {
                            continue;
                        }
                        int event_sx = XCoordFromClocks( evnt.Clock );
                        if ( pictPianoRoll.Width < event_sx ) {
                            break;
                        }
                        int event_sy = YCoordFromNote( evnt.ID.Note, stdy );

                        // 左端
                        Rectangle rc = new Rectangle( event_sx - _EDIT_HANDLE_WIDTH / 2, 
                                                      event_sy,
                                                      _EDIT_HANDLE_WIDTH,
                                                      AppManager.EditorConfig.PxTrackHeight );
#endif
                        if ( IsInRect( e.Location, rc ) ) {
                            AppManager.setEditMode( EditMode.EDIT_LEFT_EDGE );
                            if ( !AppManager.isSelectedEventContains( AppManager.getSelected(), item.InternalID ) ) {
                                AppManager.clearSelectedEvent();
                            }
                            AppManager.addSelectedEvent( item.InternalID );
                            this.Cursor = Cursors.VSplit;
                            refreshScreen();
#if DEBUG
                            AppManager.debugWriteLine( "    EditMode=" + AppManager.getEditMode() );
#endif
                            return;
                        }
#if USE_DOBJ
                        rc = new Rectangle( dobj.pxRectangle.X + dobj.pxRectangle.Width - stdx - _EDIT_HANDLE_WIDTH, dobj.pxRectangle.Y - stdy, _EDIT_HANDLE_WIDTH, dobj.pxRectangle.Height );
#else
                        rect = new Rectangle( event_ex - _EDIT_HANDLE_WIDTH / 2,
                                              event_sy,
                                              _EDIT_HANDLE_WIDTH,
                                              AppManager.EditorConfig.PxTrackHeight );
#endif
                        if ( IsInRect( e.Location, rc ) ) {
                            AppManager.setEditMode( EditMode.EDIT_RIGHT_EDGE );
                            if ( !AppManager.isSelectedEventContains( AppManager.getSelected(), item.InternalID ) ) {
                                AppManager.clearSelectedEvent();
                            }
                            AppManager.addSelectedEvent( item.InternalID );
                            this.Cursor = Cursors.VSplit;
                            refreshScreen();
#if DEBUG
                            AppManager.debugWriteLine( "    EditMode=" + AppManager.getEditMode() );
#endif
                            return;
                        }
                    }
                }
                if ( e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle ) {
                    if ( selected_tool == EditTool.PALETTE_TOOL ) {
                        AppManager.setEditMode( EditMode.NONE );
                        AppManager.clearSelectedEvent();
                        AppManager.addSelectedEvent( item.InternalID );
                    } else if ( selected_tool != EditTool.ERASER ) {
                        AppManager.setEditMode( EditMode.MOVE_ENTRY_WAIT_MOVE );
                        m_mouse_move_init = new Point( e.X + AppManager.startToDrawX, e.Y + StartToDrawY );
                        int head_x = xCoordFromClocks( item.Clock );
                        m_mouse_move_offset = e.X - head_x;
                        if ( (modefier & Keys.Shift) == Keys.Shift ) {
                            // 範囲選択
                            int last_id = AppManager.getLastSelectedEvent().original.InternalID;
                            int last_clock = 0;
                            int this_clock = 0;
                            boolean this_found = false, last_found = false;
                            for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventIterator(); itr.hasNext(); ) {
                                VsqEvent ev = (VsqEvent)itr.next();
                                if ( ev.InternalID == last_id ) {
                                    last_clock = ev.Clock;
                                    last_found = true;
                                } else if ( ev.InternalID == item.InternalID ) {
                                    this_clock = ev.Clock;
                                    this_found = true;
                                }
                                if ( last_found && this_found ) {
                                    break;
                                }
                            }
                            int start = Math.Min( last_clock, this_clock );
                            int end = Math.Max( last_clock, this_clock );
                            Vector<Int32> add_required = new Vector<Int32>();
                            for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventIterator(); itr.hasNext(); ) {
                                VsqEvent ev = (VsqEvent)itr.next();
                                if ( start <= ev.Clock && ev.Clock <= end ) {
                                    if ( !add_required.contains( ev.InternalID ) ) {
                                        add_required.add( ev.InternalID );
                                    }
                                }
                            }
                            if ( !add_required.contains( item.InternalID ) ) {
                                add_required.add( item.InternalID );
                            }
                            AppManager.addSelectedEventRange( add_required.toArray( new Int32[]{} ) );
                        } else if ( (modefier & s_modifier_key) == s_modifier_key ) {
                            // CTRLキーを押しながら選択／選択解除
                            if ( AppManager.isSelectedEventContains( AppManager.getSelected(), item.InternalID ) ) {
                                AppManager.removeSelectedEvent( item.InternalID );
                            } else {
                                AppManager.addSelectedEvent( item.InternalID );
                            }
                        } else {
                            if ( !AppManager.isSelectedEventContains( AppManager.getSelected(), item.InternalID ) ) {
                                // MouseDownしたアイテムが、まだ選択されていなかった場合。当該アイテム単独に選択しなおす
                                AppManager.clearSelectedEvent();
                            }
                            AppManager.addSelectedEvent( item.InternalID );
                        }
                        this.Cursor = Cursors.Hand;
#if DEBUG
                        AppManager.debugWriteLine( "    EditMode=" + AppManager.getEditMode() );
                        AppManager.debugWriteLine( "    m_config.SelectedEvent.Count=" + AppManager.getSelectedEventCount() );
#endif
                    }
                }
                #endregion
            }
            refreshScreen();
        }

        private void pictPianoRoll_MouseMove( object sender, MouseEventArgs e ) {
            if ( m_form_activated ) {
                if ( m_input_textbox != null && !m_input_textbox.IsDisposed && !m_input_textbox.Visible && !AppManager.propertyPanel.Editing ) {
                    pictPianoRoll.Focus();
                }
            }
            if ( !m_mouse_moved && AppManager.getEditMode() == EditMode.MIDDLE_DRAG ) {
                this.Cursor = HAND;
            }

            if ( e.Location.X != m_button_initial.X || e.Location.Y != m_button_initial.Y ) {
                m_mouse_moved = true;
            }
            if ( !(AppManager.getEditMode() == EditMode.MIDDLE_DRAG) && AppManager.isPlaying() ) {
                return;
            }

            if ( AppManager.getEditMode() == EditMode.MOVE_ENTRY_WAIT_MOVE ) {
                int x = e.X + AppManager.startToDrawX;
                int y = e.Y + StartToDrawY;
                if ( m_mouse_move_init.X != x || m_mouse_move_init.Y != y ) {
                    AppManager.setEditMode( EditMode.MOVE_ENTRY );
                }
            }

            if ( m_mouse_moved && m_mouse_hover_thread != null ) {
                m_mouse_hover_thread.Abort();
            }

            int clock = clockFromXCoord( e.X );
            if ( m_mouse_downed ) {
                if ( m_ext_dragx == ExtDragXMode.NONE ) {
                    if ( AppManager.KEY_LENGTH > e.X ) {
                        m_ext_dragx = ExtDragXMode.LEFT;
                    } else if ( pictPianoRoll.Width < e.X ) {
                        m_ext_dragx = ExtDragXMode.RIGHT;
                    }
                } else {
                    if ( AppManager.KEY_LENGTH <= e.X && e.X <= pictPianoRoll.Width ) {
                        m_ext_dragx = ExtDragXMode.NONE;
                    }
                }

                if ( m_ext_dragy == ExtDragYMode.NONE ) {
                    if ( 0 > e.Y ) {
                        m_ext_dragy = ExtDragYMode.UP;
                    } else if ( pictPianoRoll.Height < e.Y ) {
                        m_ext_dragy = ExtDragYMode.DOWN;
                    }
                } else {
                    if ( 0 <= e.Y && e.Y <= pictPianoRoll.Height ) {
                        m_ext_dragy = ExtDragYMode.NONE;
                    }
                }
            } else {
                m_ext_dragx = ExtDragXMode.NONE;
                m_ext_dragy = ExtDragYMode.NONE;
            }

            if ( m_ext_dragx == ExtDragXMode.RIGHT || m_ext_dragx == ExtDragXMode.LEFT ) {
                DateTime now = DateTime.Now;
                double dt = now.Subtract( m_timer_drag_last_ignitted ).TotalSeconds;
                m_timer_drag_last_ignitted = now;
                int px_move = AppManager.editorConfig.MouseDragIncrement;
                if ( px_move / dt > AppManager.editorConfig.MouseDragMaximumRate ) {
                    px_move = (int)(dt * AppManager.editorConfig.MouseDragMaximumRate);
                }
                double d_draft;
                if ( m_ext_dragx == ExtDragXMode.RIGHT ) {
                    int right_clock = clockFromXCoord( pictPianoRoll.Width );
                    int dclock = (int)(px_move / AppManager.scaleX);
                    d_draft = (73 - pictPianoRoll.Width) / AppManager.scaleX + right_clock + dclock;
                } else {
                    px_move *= -1;
                    int left_clock = clockFromXCoord( AppManager.KEY_LENGTH );
                    int dclock = (int)(px_move / AppManager.scaleX);
                    d_draft = (73 - AppManager.KEY_LENGTH) / AppManager.scaleX + left_clock + dclock;
                }
                if ( d_draft < 0.0 ) {
                    d_draft = 0.0;
                }
                int draft = (int)d_draft;
                if ( hScroll.Maximum < draft ) {
                    if ( AppManager.getEditMode() == EditMode.ADD_ENTRY || AppManager.getEditMode() == EditMode.MOVE_ENTRY || AppManager.getEditMode() == EditMode.ADD_FIXED_LENGTH_ENTRY ) {
                        hScroll.Maximum = draft;
                    } else {
                        draft = hScroll.Maximum;
                    }
                }
                if ( draft < hScroll.Minimum ) {
                    draft = hScroll.Minimum;
                }
                hScroll.Value = draft;
            }
            if ( m_ext_dragy == ExtDragYMode.UP || m_ext_dragy == ExtDragYMode.DOWN ) {
                DateTime now = DateTime.Now;
                double dt = now.Subtract( m_timer_drag_last_ignitted ).TotalSeconds;
                m_timer_drag_last_ignitted = now;
                int px_move = AppManager.editorConfig.MouseDragIncrement;
                if ( px_move / dt > AppManager.editorConfig.MouseDragMaximumRate ) {
                    px_move = (int)(dt * AppManager.editorConfig.MouseDragMaximumRate);
                }
                if ( m_ext_dragy == ExtDragYMode.UP ) {
                    px_move *= -1;
                }
                int draft_stdy = StartToDrawY + px_move;
                int draft = (int)((draft_stdy * (double)vScroll.Maximum) / (128.0 * AppManager.editorConfig.PxTrackHeight - vScroll.Height));
                if ( draft < 0 ) {
                    draft = 0;
                }
                int df = (int)draft;
                if ( df < vScroll.Minimum ) {
                    df = vScroll.Minimum;
                } else if ( vScroll.Maximum < df ) {
                    df = vScroll.Maximum;
                }
                vScroll.Value = df;
            }

            // 選択範囲にあるイベントを選択．
            int stdy = StartToDrawY;
            if ( m_pointer_downed ) {
                if ( AppManager.selectedRegionEnabled ) {
                    int x = e.X + AppManager.startToDrawX;
                    if ( AppManager.editorConfig.CurveSelectingQuantized ) {
                        int clock1 = clockFromXCoord( e.X );
                        int unit = AppManager.getPositionQuantizeClock();
                        int odd = clock1 % unit;
                        int nclock = clock1;
                        nclock -= odd;
                        if ( odd > unit / 2 ) {
                            nclock += unit;
                        }
                        x = xCoordFromClocks( nclock ) + AppManager.startToDrawX;
                    }
                    AppManager.selectedRegion.SetEnd( x );
                } else {
                    Point mouse = new Point( e.X + AppManager.startToDrawX, e.Y + StartToDrawY );
                    int tx, ty, twidth, theight;
                    int lx = m_pointer_mouse_down.X;
                    if ( lx < mouse.X ) {
                        tx = lx;
                        twidth = mouse.X - lx;
                    } else {
                        tx = mouse.X;
                        twidth = lx - mouse.X;
                    }
                    int ly = m_pointer_mouse_down.Y;
                    if ( ly < mouse.Y ) {
                        ty = ly;
                        theight = mouse.Y - ly;
                    } else {
                        ty = mouse.Y;
                        theight = ly - mouse.Y;
                    }

                    Rectangle rect = new Rectangle( tx, ty, twidth, theight );
                    Vector<Int32> add_required = new Vector<Int32>();
                    int internal_id = -1;
#if USE_DOBJ
                    for ( Iterator itr = m_draw_objects.get( AppManager.getSelected() - 1 ).iterator(); itr.hasNext(); ){
                        DrawObject dobj = (DrawObject)itr.next();
                        int x0 = dobj.pxRectangle.X;
                        int x1 = dobj.pxRectangle.X + dobj.pxRectangle.Width;
                        int y0 = dobj.pxRectangle.Y;
                        int y1 = dobj.pxRectangle.Y + dobj.pxRectangle.Height;
                        internal_id = dobj.InternalID;
#else
                    //int stdy = StartToDrawY;
                    for ( Iterator itr = AppManager.VsqFile.getTrack( AppManager.Selected ).getNoteEventIterator(); itr.hasNext(); ){
                        VsqEvent evnt = (VsqEvent)itr.next();
                        int x0 = XCoordFromClocks( evnt.Clock );
                        int x1 = XCoordFromClocks( evnt.Clock + evnt.ID.Length );
                        int y0 = YCoordFromNote( evnt.ID.Note, stdy );
                        int y1 = y0 + AppManager.EditorConfig.PxTrackHeight;
                        internal_id = evnt.InternalID;
#endif
                        if ( x1 < tx ) {
                            continue;
                        }
                        if ( tx + twidth < x0 ) {
                            break;
                        }
                        boolean found = IsInRect( new Point( x0, y0 ), rect ) | IsInRect( new Point( x0, y1 ), rect ) | IsInRect( new Point( x1, y0 ), rect ) | IsInRect( new Point( x1, y1 ), rect );
                        if ( found ) {
                            add_required.add( internal_id );
                        } else {
                            if ( x0 <= tx && tx + twidth <= x1 ) {
                                if ( ty < y0 ) {
                                    if ( y0 <= ty + theight ) {
                                        add_required.add( internal_id );
                                    }
                                } else if ( y0 <= ty && ty < y1 ) {
                                    add_required.add( internal_id );
                                }
                            } else if ( y0 <= ty && ty + theight <= y1 ) {
                                if ( tx < x0 ) {
                                    if ( x0 <= tx + twidth ) {
                                        add_required.add( internal_id );
                                    }
                                } else if ( x0 <= tx && tx < x1 ) {
                                    add_required.add( internal_id );
                                }
                            }
                        }
                    }
                    Vector<Integer> remove_required = new Vector<Integer>();
                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                        SelectedEventEntry selected = (SelectedEventEntry)itr.next();
                        if ( !add_required.contains( selected.original.InternalID ) ) {
                            remove_required.add( selected.original.InternalID );
                        }
                    }
                    if ( remove_required.size() > 0 ) {
                        AppManager.removeSelectedEventRange( remove_required.toArray( new Integer[]{} ) );
                    }
                    for ( Iterator itr = new ListIterator<int>( add_required ); itr.hasNext(); ) {
                        int id = (int)itr.next();
                        if ( AppManager.isSelectedEventContains( AppManager.getSelected(), id ) ) {
                            itr.remove();
                        }
                    }
                    AppManager.addSelectedEventRange( add_required.toArray( new Int32[]{} ) );
                }
            }

            if ( AppManager.getEditMode() == EditMode.MIDDLE_DRAG ) {
                #region MiddleDrag
                int dx = e.X - m_button_initial.X;
                int dy = e.Y - m_button_initial.Y;
                double new_vscroll_value = (double)m_middle_button_vscroll - dy * (double)vScroll.Maximum / (128.0 * AppManager.editorConfig.PxTrackHeight - (double)vScroll.Height);
                double new_hscroll_value = (double)m_middle_button_hscroll - (double)dx / AppManager.scaleX;
                if ( new_vscroll_value < vScroll.Minimum ) {
                    vScroll.Value = vScroll.Minimum;
                } else if ( vScroll.Maximum < new_vscroll_value ) {
                    vScroll.Value = vScroll.Maximum;
                } else {
                    vScroll.Value = (int)new_vscroll_value;
                }
                if ( new_hscroll_value < hScroll.Minimum ) {
                    hScroll.Value = hScroll.Minimum;
                } else if ( hScroll.Maximum < new_hscroll_value ) {
                    hScroll.Value = hScroll.Maximum;
                } else {
                    hScroll.Value = (int)new_hscroll_value;
                }
                if ( AppManager.isPlaying() ) {
                    return;
                }
                #endregion
                return;
            } else if ( AppManager.getEditMode() == EditMode.ADD_ENTRY ) {
                #region AddEntry
                int unit = AppManager.getLengthQuantizeClock();
                int length = clock - m_adding.Clock;
                int odd = length % unit;
                int new_length = length - odd;

                if ( unit * AppManager.scaleX > 10 ) { //これをしないと、グリッド2個分増えることがある
                    int next_clock = clockFromXCoord( e.X + 10 );
                    int next_length = next_clock - m_adding.Clock;
                    int next_new_length = next_length - (next_length % unit);
                    if ( next_new_length == new_length + unit ) {
                        new_length = next_new_length;
                    }
                }

                if ( new_length <= 0 ) {
                    new_length = 0;
                }
                m_adding.ID.Length = new_length;
                #endregion
            } else if ( AppManager.getEditMode() == EditMode.MOVE_ENTRY ) {
                #region MoveEntry
                if ( AppManager.getSelectedEventCount() > 0 ) {
                    VsqEvent original = AppManager.getLastSelectedEvent().original;
                    int note = noteFromYCoord( e.Y );                           // 現在のマウス位置でのnote
                    int note_init = original.ID.Note;
                    int dnote = note - note_init;

                    int tclock = clockFromXCoord( e.X - m_mouse_move_offset );
                    int clock_init = original.Clock;

                    int dclock = tclock - clock_init;

                    if ( AppManager.editorConfig.PositionQuantize != QuantizeMode.off ) {
                        int unit = AppManager.getPositionQuantizeClock();
                        int new_clock = original.Clock + dclock;
                        int odd = new_clock % unit;
                        new_clock -= odd;
                        if ( odd > unit / 2 ) {
                            new_clock += unit;
                        }
                        dclock = new_clock - clock_init;
                    }

                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                        SelectedEventEntry item = (SelectedEventEntry)itr.next();
                        int new_clock = item.original.Clock + dclock;
                        /*int odd = new_clock % unit;
                        new_clock -= odd;
                        if ( odd > unit / 2 ) {
                            new_clock += unit;
                        }*/
                        int new_note = item.original.ID.Note + dnote;
                        item.editing.Clock = new_clock;
                        item.editing.ID.Note = new_note;
                    }
                }
                #endregion
            } else if ( AppManager.getEditMode() == EditMode.EDIT_LEFT_EDGE ) {
                #region EditLeftEdge
                int unit = AppManager.getLengthQuantizeClock();
                VsqEvent original = AppManager.getLastSelectedEvent().original;
                int clock_init = original.Clock;
                int dclock = clock - clock_init;
                for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                    SelectedEventEntry item = (SelectedEventEntry)itr.next();
                    int end_clock = item.original.Clock + item.original.ID.Length;
                    int new_clock = item.original.Clock + dclock;
                    int length = end_clock - new_clock;
                    int odd = length % unit;
                    int new_length = length - odd;
                    if ( odd > unit / 2 ) {
                        new_length += unit;
                    }
                    if ( new_length <= 0 ) {
                        new_length = unit;
                    }
                    item.editing.Clock = end_clock - new_length;
                    item.editing.ID.Length = new_length;
                }
                #endregion
            } else if ( AppManager.getEditMode() == EditMode.EDIT_RIGHT_EDGE ) {
                #region EditRightEdge
                int unit = AppManager.getLengthQuantizeClock();

                VsqEvent original = AppManager.getLastSelectedEvent().original;
                int dlength = clock - (original.Clock + original.ID.Length);
                for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                    SelectedEventEntry item = (SelectedEventEntry)itr.next();
                    int length = item.original.ID.Length + dlength;
                    int odd = length % unit;
                    int new_length = length - odd;
                    if ( odd > unit / 2 ) {
                        new_length += unit;
                    }
                    if ( new_length <= 0 ) {
                        new_length = unit;
                    }
                    item.editing.ID.Length = new_length;
                }
                #endregion
            } else if ( AppManager.getEditMode() == EditMode.ADD_FIXED_LENGTH_ENTRY ) {
                #region AddFixedLengthEntry
                int note = noteFromYCoord( e.Y );
                int unit = AppManager.getPositionQuantizeClock();
                int new_clock = clockFromXCoord( e.X );
                int odd = new_clock % unit;
                new_clock -= odd;
                if ( odd > unit / 2 ) {
                    new_clock += unit;
                }
                m_adding.ID.Note = note;
                m_adding.Clock = new_clock;
                #endregion
            } else if ( AppManager.getEditMode() == EditMode.EDIT_VIBRATO_DELAY ) {
                #region EditVibratoDelay
                int new_vibrato_start = clock;
                int old_vibrato_end = m_adding.Clock + m_adding.ID.Length;
                int new_vibrato_length = old_vibrato_end - new_vibrato_start;
                int max_length = (int)(m_adding_length - _PX_ACCENT_HEADER / AppManager.scaleX);
                if ( max_length < 0 ) {
                    max_length = 0;
                }
                if ( new_vibrato_length > max_length ) {
                    new_vibrato_start = old_vibrato_end - max_length;
                    new_vibrato_length = max_length;
                }
                if ( new_vibrato_length < 0 ) {
                    new_vibrato_start = old_vibrato_end;
                    new_vibrato_length = 0;
                }
                m_adding.Clock = new_vibrato_start;
                m_adding.ID.Length = new_vibrato_length;
                UpdatePositionViewFromMousePosition( clock );
                if ( !timer.Enabled ) {
                    refreshScreen();
                }
                #endregion
                return;
            }
            UpdatePositionViewFromMousePosition( clock );

            // カーソルの形を決める
            if ( !m_mouse_downed ) {
                boolean split_cursor = false;
                boolean hand_cursor = false;
                int stdx = AppManager.startToDrawX;
#if USE_DOBJ
                for ( Iterator itr = m_draw_objects.get( AppManager.getSelected() - 1 ).iterator(); itr.hasNext(); ){
                    DrawObject dobj = (DrawObject)itr.next();
                    // 音符左側の編集領域
#else
                for ( Iterator itr = AppManager.VsqFile.getTrack( AppManager.Selected ).getNoteEventIterator(); itr.hasNext(); ){

#endif
                    Rectangle rc = new Rectangle(
                                        dobj.pxRectangle.X - stdx,
                                        dobj.pxRectangle.Y - stdy,
                                        _EDIT_HANDLE_WIDTH,
                                        AppManager.editorConfig.PxTrackHeight );
                    if ( IsInRect( e.Location, rc ) ) {
                        split_cursor = true;
                        break;
                    }

                    // 音符右側の編集領域
                    rc = new Rectangle( dobj.pxRectangle.X + dobj.pxRectangle.Width - stdx - _EDIT_HANDLE_WIDTH,
                                        dobj.pxRectangle.Y - stdy,
                                        _EDIT_HANDLE_WIDTH,
                                        AppManager.editorConfig.PxTrackHeight );
                    if ( IsInRect( e.Location, rc ) ) {
                        split_cursor = true;
                        break;
                    }

                    // 音符本体
                    rc = new Rectangle( dobj.pxRectangle.X - stdx,
                                        dobj.pxRectangle.Y - stdy,
                                        dobj.pxRectangle.Width,
                                        dobj.pxRectangle.Height );
                    if ( AppManager.editorConfig.ShowExpLine && !dobj.Overwrapped ) {
                        rc.Height *= 2;
                        if ( IsInRect( e.Location, rc ) ) {
                            // ビブラートの開始位置
                            rc = new Rectangle( dobj.pxRectangle.X + dobj.pxVibratoDelay - stdx - _EDIT_HANDLE_WIDTH / 2,
                                                dobj.pxRectangle.Y + AppManager.editorConfig.PxTrackHeight - stdy,
                                                _EDIT_HANDLE_WIDTH,
                                                AppManager.editorConfig.PxTrackHeight );
                            if ( IsInRect( e.Location, rc ) ) {
                                split_cursor = true;
                                break;
                            } else {
                                hand_cursor = true;
                                break;
                            }
                        }
                    } else {
                        if ( IsInRect( e.Location, rc ) ) {
                            hand_cursor = true;
                            break;
                        }
                    }
                }

                if ( split_cursor ) {
                    this.Cursor = Cursors.VSplit;
                } else if ( hand_cursor ) {
                    this.Cursor = Cursors.Hand;
                } else {
                    this.Cursor = Cursors.Default;
                }
            }
            if ( !timer.Enabled ) {
                refreshScreen();
            }
        }

        private void pictPianoRoll_MouseUp( object sender, MouseEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "pictureBox1_MouseUp" );
            AppManager.debugWriteLine( "    m_config.EditMode=" + AppManager.getEditMode() );
#endif
            m_pointer_downed = false;
            m_mouse_downed = false;

            Keys modefier = Control.ModifierKeys;
            if ( AppManager.getEditMode() == EditMode.MIDDLE_DRAG ) {
                this.Cursor = Cursors.Default;
            } else if ( AppManager.getEditMode() == EditMode.ADD_ENTRY || AppManager.getEditMode() == EditMode.ADD_FIXED_LENGTH_ENTRY ) {
                #region AddEntry || AddFixedLengthEntry
                if ( AppManager.getSelected() >= 0 ) {
                    if ( (AppManager.getEditMode() == EditMode.ADD_FIXED_LENGTH_ENTRY) ||
                         (AppManager.getEditMode() == EditMode.ADD_ENTRY && (m_button_initial.X != e.X || m_button_initial.Y != e.Y) && m_adding.ID.Length > 0) ) {
                        LyricHandle lyric = new LyricHandle( "a", "a" );
                        VibratoHandle vibrato = null;
                        int vibrato_delay = 0;
                        if ( AppManager.editorConfig.EnableAutoVibrato ) {
                            int note_length = m_adding.ID.Length;
                            // 音符位置での拍子を調べる
                            int denom, numer;
                            AppManager.getVsqFile().getTimesigAt( m_adding.Clock, out numer, out denom );

                            // ビブラートを自動追加するかどうかを決める閾値
                            int threshold = 480 * 4 / denom * (int)AppManager.editorConfig.AutoVibratoMinimumLength;
                            if ( note_length >= threshold ) {
                                int vibrato_clocks = 0;
                                switch ( AppManager.editorConfig.DefaultVibratoLength ) {
                                    case DefaultVibratoLength.L100:
                                        vibrato_clocks = note_length;
                                        break;
                                    case DefaultVibratoLength.L50:
                                        vibrato_clocks = note_length / 2;
                                        break;
                                    case DefaultVibratoLength.L66:
                                        vibrato_clocks = note_length * 2 / 3;
                                        break;
                                    case DefaultVibratoLength.L75:
                                        vibrato_clocks = note_length * 3 / 4;
                                        break;
                                }
                                SynthesizerType type = SynthesizerType.VOCALOID2;
                                String default_icon_id = AppManager.editorConfig.AutoVibratoType2;
                                if ( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version.Equals( VSTiProxy.RENDERER_DSB2 ) ) {
                                    type = SynthesizerType.VOCALOID1;
                                    default_icon_id = AppManager.editorConfig.AutoVibratoType1;
                                }
                                vibrato = VocaloSysUtil.getDefaultVibratoHandle( default_icon_id, vibrato_clocks, type );
                                vibrato_delay = note_length - vibrato_clocks;
                            }
                        }

                        // 自動ノーマライズのモードで、処理を分岐
                        if ( AppManager.autoNormalize ) {
                            VsqTrack work = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).Clone();
                            m_adding.ID.type = VsqIDType.Anote;
                            m_adding.ID.Dynamics = 64;
                            m_adding.ID.VibratoHandle = vibrato;
                            m_adding.ID.LyricHandle = lyric;
                            m_adding.ID.VibratoDelay = vibrato_delay;
                            //m_adding.InternalID = work.GetNextId( 0 );
                            boolean changed = true;
                            while ( changed ) {
                                changed = false;
                                for ( int i = 0; i < work.getEventCount(); i++ ) {
                                    int start_clock = work.getEvent( i ).Clock;
                                    int end_clock = work.getEvent( i ).ID.Length + start_clock;
                                    if ( start_clock < m_adding.Clock && m_adding.Clock < end_clock ) {
                                        work.getEvent( i ).ID.Length = m_adding.Clock - start_clock;
                                        changed = true;
                                    } else if ( start_clock == m_adding.Clock ) {
                                        work.removeEvent( i );
                                        changed = true;
                                        break;
                                    } else if ( m_adding.Clock < start_clock && start_clock < m_adding.Clock + m_adding.ID.Length ) {
                                        m_adding.ID.Length = start_clock - m_adding.Clock;
                                        changed = true;
                                    }
                                }
                            }
                            work.addEvent( (VsqEvent)m_adding.clone() );
                            CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( AppManager.getSelected(),
                                                                                         work,
                                                                                         AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ) );
                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                            Edited = true;
                        } else {
                            VsqEvent[] items = new VsqEvent[1];
                            m_adding.ID.type = VsqIDType.Anote;
                            m_adding.ID.Dynamics = 64;
                            items[0] = new VsqEvent( 0, m_adding.ID );
                            items[0].Clock = m_adding.Clock;
                            items[0].ID.LyricHandle = lyric;
                            items[0].ID.VibratoDelay = vibrato_delay;
                            items[0].ID.VibratoHandle = vibrato;
#if DEBUG
                            AppManager.debugWriteLine( "        items[0].ID.ToString()=" + items[0].ID.ToString() );
#endif
                            CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventAddRange( AppManager.getSelected(), items ) );
                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                            Edited = true;
                        }
                    }
                }
                #endregion
            } else if ( AppManager.getEditMode() == EditMode.MOVE_ENTRY ) {
                #region MoveEntry
#if DEBUG
                AppManager.debugWriteLine( "    m_config.SelectedEvent.Count=" + AppManager.getSelectedEventCount() );
#endif
                if ( AppManager.getSelectedEventCount() > 0 ) {
                    VsqEvent original = AppManager.getLastSelectedEvent().original;
                    if ( original.Clock != AppManager.getLastSelectedEvent().editing.Clock || original.ID.Note != AppManager.getLastSelectedEvent().editing.ID.Note ) {
                        int count = AppManager.getSelectedEventCount();
                        int[] ids = new int[count];
                        int[] clocks = new int[count];
                        VsqID[] values = new VsqID[count];
                        int i = -1;
                        boolean out_of_range = false;
                        for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                            SelectedEventEntry ev = (SelectedEventEntry)itr.next();
                            i++;
                            ids[i] = ev.original.InternalID;
                            clocks[i] = ev.editing.Clock;
                            if ( clocks[i] < AppManager.getVsqFile().getPreMeasureClocks() ) {
                                out_of_range = true;
                            }
                            values[i] = ev.editing.ID;
                            if ( values[i].Note < 0 || 128 < values[i].Note ) {
                                out_of_range = true;
                            }
                        }
                        if ( !out_of_range ) {
                            CadenciiCommand run = new CadenciiCommand(
                                VsqCommand.generateCommandEventChangeClockAndIDContaintsRange( AppManager.getSelected(),
                                                                                               ids,
                                                                                               clocks,
                                                                                               values ) );
                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                            Edited = true;
                        } else {
                            SystemSounds.Asterisk.Play();
                        }
                    } else {
                        /*if ( (modefier & Keys.Shift) == Keys.Shift || (modefier & Keys.Control) == Keys.Control ) {
                            Rectangle rc;
                            VsqEvent select = IdOfClickedPosition( e.Location, out rc );
                            if ( select != null ) {
                                m_config.addSelectedEvent( item.InternalID );
                            }
                        }*/
                    }
                    lock ( m_draw_objects ) {
                        Collections.sort( m_draw_objects.get( AppManager.getSelected() - 1 ) );
                    }
                }
                #endregion
            } else if ( AppManager.getEditMode() == EditMode.EDIT_LEFT_EDGE ) {
                #region EditLeftEdge
                VsqEvent original = AppManager.getLastSelectedEvent().original;
                if ( original.Clock != AppManager.getLastSelectedEvent().editing.Clock ||
                    original.ID.Length != original.ID.Length ) {
                    int count = AppManager.getSelectedEventCount();
                    int[] ids = new int[count];
                    int[] clocks = new int[count];
                    VsqID[] values = new VsqID[count];
                    int i = -1;
                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                        SelectedEventEntry ev = (SelectedEventEntry)itr.next();
                        i++;
                        if ( ev.editing.ID.VibratoHandle == null ) {
                            ids[i] = ev.original.InternalID;
                            clocks[i] = ev.editing.Clock;
                            values[i] = ev.editing.ID;
                        } else {
                            int draft_vibrato_length = ev.editing.ID.Length - ev.editing.ID.VibratoDelay;
                            if ( draft_vibrato_length <= 0 ) {
                                // ビブラートを削除
                                ev.editing.ID.VibratoHandle = null;
                                ev.editing.ID.VibratoDelay = 0;
                            } else {
                                // ビブラートは温存
                                ev.editing.ID.VibratoHandle.Length = draft_vibrato_length;
                            }
                            ids[i] = ev.original.InternalID;
                            clocks[i] = ev.editing.Clock;
                            values[i] = ev.editing.ID;
                        }
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventChangeClockAndIDContaintsRange( AppManager.getSelected(),
                                                                             ids,
                                                                             clocks,
                                                                             values ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    Edited = true;
                }
                #endregion
            } else if ( AppManager.getEditMode() == EditMode.EDIT_RIGHT_EDGE ) {
                #region EditRightEdge
                VsqEvent original = AppManager.getLastSelectedEvent().original;
                if ( original.ID.Length != AppManager.getLastSelectedEvent().editing.ID.Length ) {
                    int count = AppManager.getSelectedEventCount();
                    int[] ids = new int[count];
                    int[] clocks = new int[count];
                    VsqID[] values = new VsqID[count];
                    int i = -1;
                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                        SelectedEventEntry ev = (SelectedEventEntry)itr.next();
                        i++;
                        if ( ev.editing.ID.VibratoHandle == null ) {
                            ids[i] = ev.original.InternalID;
                            clocks[i] = ev.editing.Clock;
                            values[i] = ev.editing.ID;
                        } else {
                            int draft_vibrato_length = ev.editing.ID.Length - ev.editing.ID.VibratoDelay;
                            if ( draft_vibrato_length <= 0 ) {
                                // ビブラートを削除
                                ev.editing.ID.VibratoHandle = null;
                                ev.editing.ID.VibratoDelay = 0;
                            } else {
                                // ビブラートは温存
                                ev.editing.ID.VibratoHandle.Length = draft_vibrato_length;
                            }
                            ids[i] = ev.original.InternalID;
                            clocks[i] = ev.editing.Clock;
                            values[i] = ev.editing.ID;
                        }
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventChangeClockAndIDContaintsRange( AppManager.getSelected(),
                                                                             ids,
                                                                             clocks,
                                                                             values ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    Edited = true;
                }
                #endregion
            } else if ( AppManager.getEditMode() == EditMode.EDIT_VIBRATO_DELAY ) {
                #region EditVibratoDelay
                if ( m_mouse_moved ) {
                    double max_length = m_adding_length - _PX_ACCENT_HEADER / AppManager.scaleX;
                    double rate = m_adding.ID.Length / max_length;
                    if ( rate > 0.99 ) {
                        rate = 1.0;
                    }
                    int vibrato_length = (int)(m_adding_length * rate);
                    VsqEvent item = null;
                    for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                        VsqEvent ve = (VsqEvent)itr.next();
                        if ( ve.InternalID == m_vibrato_editing_id ) {
                            item = (VsqEvent)ve.clone();
                            break;
                        }
                    }
                    if ( item != null ) {
                        if ( vibrato_length <= 0 ) {
                            item.ID.VibratoHandle = null;
                            item.ID.VibratoDelay = item.ID.Length;
                        } else {
                            item.ID.VibratoHandle.Length = vibrato_length;
                            item.ID.VibratoDelay = item.ID.Length - vibrato_length;
                        }
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(), m_vibrato_editing_id, item.ID ) );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                        Edited = true;
                    }
                }
                #endregion
            } else if ( AppManager.selectedRegionEnabled ) {
                int stdx = AppManager.startToDrawX;
                int start = clockFromXCoord( AppManager.selectedRegion.Start - stdx );
                int end = clockFromXCoord( AppManager.selectedRegion.End - stdx );
                AppManager.clearSelectedEvent();
                Vector<Int32> add_required = new Vector<Int32>();
                for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventIterator(); itr.hasNext(); ) {
                    VsqEvent ve = (VsqEvent)itr.next();
                    if ( start <= ve.Clock && ve.Clock <= end ) {
                        add_required.add( ve.InternalID );
                    }
                }
                AppManager.addSelectedEventRange( add_required.toArray( new Int32[]{} ) );
            }
            refreshScreen();
            if ( AppManager.getEditMode() != EditMode.REALTIME ) {
                AppManager.setEditMode( EditMode.NONE );
            }
        }

        private void pictPianoRoll_MouseWheel( object sender, MouseEventArgs e ) {
            boolean horizontal = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
            if( AppManager.editorConfig.ScrollHorizontalOnWheel ){
                horizontal = !horizontal;
            }
            if ( horizontal ) {
                hScroll.Value = NewHScrollValueFromWheelDelta( e.Delta );
            } else {
                double new_val = (double)vScroll.Value - e.Delta;
                if ( new_val > vScroll.Maximum ) {
                    vScroll.Value = vScroll.Maximum;
                } else if ( new_val < vScroll.Minimum ) {
                    vScroll.Value = vScroll.Minimum;
                } else {
                    vScroll.Value = (int)new_val;
                }
            }
            refreshScreen();
        }

        private void pictPianoRoll_Paint( object sender, PaintEventArgs e ) {
            DrawTo( e.Graphics, pictPianoRoll.Size, pictPianoRoll.PointToClient( Control.MousePosition ) );

            // マーカー
            int marker_x = (int)(AppManager.getCurrentClock() * AppManager.scaleX + 6 + AppManager.KEY_LENGTH - AppManager.startToDrawX);
            if ( AppManager.KEY_LENGTH <= marker_x && marker_x <= pictPianoRoll.Width ) {
                e.Graphics.DrawLine( new Pen( Color.White, 2f ),
                                     new Point( marker_x, 0 ),
                                     new Point( marker_x, pictPianoRoll.Height ) );
            }


            DateTime dnow = DateTime.Now;
            for ( int i = 0; i < _NUM_PCOUNTER - 1; i++ ) {
                m_performance[i] = m_performance[i + 1];
            }
            m_performance[_NUM_PCOUNTER - 1] = (float)dnow.Subtract( m_last_ignitted ).TotalSeconds;
            m_last_ignitted = dnow;
            float sum = 0f;
            for ( int i = 0; i < _NUM_PCOUNTER; i++ ) {
                sum += m_performance[i];
            }
            m_fps = _NUM_PCOUNTER / sum;

            if ( AppManager.selectedRegionEnabled ) {
                int stdx = AppManager.startToDrawX;
                int start = AppManager.selectedRegion.Start - stdx;
                int end = AppManager.selectedRegion.End - stdx;
                e.Graphics.FillRectangle(
                    s_brs_a098_000_000_000,
                    new Rectangle( start, 0, end - start, pictPianoRoll.Height ) );
            } else if ( m_pointer_downed ) {
                Point mouse = pictPianoRoll.PointToClient( Control.MousePosition );
                int tx, ty, twidth, theight;
                int lx = m_pointer_mouse_down.X - AppManager.startToDrawX;
                if ( lx < mouse.X ) {
                    tx = lx;
                    twidth = mouse.X - lx;
                } else {
                    tx = mouse.X;
                    twidth = lx - mouse.X;
                }
                int ly = m_pointer_mouse_down.Y - StartToDrawY;
                if ( ly < mouse.Y ) {
                    ty = ly;
                    theight = mouse.Y - ly;
                } else {
                    ty = mouse.Y;
                    theight = ly - mouse.Y;
                }
                if ( tx < AppManager.KEY_LENGTH ) {
                    int txold = tx;
                    tx = AppManager.KEY_LENGTH;
                    twidth -= (tx - txold);
                }
                Rectangle rc = new Rectangle( tx, ty, twidth, theight );
                Pen pen = new Pen( Color.FromArgb( 200, Color.Black ) );
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                e.Graphics.FillRectangle( new SolidBrush( Color.FromArgb( 100, Color.Black ) ), rc );
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawRectangle( pen, rc );
            }
#if MONITOR_FPS
            e.Graphics.DrawString(
                m_fps.ToString( "000.000" ),
                new Font( "Verdana", 40, FontStyle.Bold ),
                Brushes.Red,
                new PointF( 0, 0 ) );
#endif
        }

        private void pictPianoRoll_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e ) {
#if DEBUG
            System.Diagnostics.Debug.WriteLine( "pictureBox1_PreviewKeyDown" );
            System.Diagnostics.Debug.WriteLine( "    e.KeyCode=" + e.KeyCode );
#endif
            if ( e.KeyCode == Keys.Tab && AppManager.getSelectedEventCount() > 0 ) {
                VsqEvent original = AppManager.getLastSelectedEvent().original;
                if ( original == null ) {
                    return;
                }
                int x = xCoordFromClocks( original.Clock );
                int y = yCoordFromNote( original.ID.Note );
                if ( !AppManager.editorConfig.KeepLyricInputMode ) {
                    m_last_symbol_edit_mode = false;
                }
                ShowInputTextBox( original.ID.LyricHandle.L0.Phrase, original.ID.LyricHandle.L0.getPhoneticSymbol(), new Point( x, y ), m_last_symbol_edit_mode );
                e.IsInputKey = true;
                refreshScreen();
            }
            ProcessSpecialShortcutKey( e );
        }
        #endregion

        #region menuVisual*
        private void menuVisualMixer_Click( object sender, EventArgs e ) {
            menuVisualMixer.Checked = !menuVisualMixer.Checked;
            AppManager.editorConfig.MixerVisible = menuVisualMixer.Checked;
            AppManager.mixerWindow.Visible = AppManager.editorConfig.MixerVisible;
            this.Focus();
        }

        private void menuVisualGridline_CheckedChanged( object sender, EventArgs e ) {
            AppManager.setGridVisible( menuVisualGridline.Checked );
            refreshScreen();
        }

        private void menuVisualLyrics_CheckedChanged( object sender, EventArgs e ) {
            AppManager.editorConfig.ShowLyric = menuVisualLyrics.Checked;
        }

        private void menuVisualNoteProperty_CheckedChanged( object sender, EventArgs e ) {
            AppManager.editorConfig.ShowExpLine = menuVisualNoteProperty.Checked;
        }

        private void menuVisualPitchLine_CheckedChanged( object sender, EventArgs e ) {
            AppManager.editorConfig.ViewAtcualPitch = menuVisualPitchLine.Checked;
        }

        private void menuVisualControlTrack_CheckedChanged( object sender, EventArgs e ) {
            trackSelector.CurveVisible = menuVisualControlTrack.Checked;
            if ( menuVisualControlTrack.Checked ) {
                splitContainer1.IsSplitterFixed = false;
                splitContainer1.SplitterDistance = splitContainer1.Height - trackSelector.PreferredMinSize - splitContainer1.SplitterWidth;
                splitContainer1.Panel2MinSize = trackSelector.PreferredMinSize;
            } else {
                splitContainer1.IsSplitterFixed = true;
                splitContainer1.Panel2MinSize = _SPL1_PANEL2_MIN_HEIGHT;
                splitContainer1.SplitterDistance = splitContainer1.Height - _SPL1_PANEL2_MIN_HEIGHT - splitContainer1.SplitterWidth;
            }
            refreshScreen();
        }

        private void menuHiddenVisualForwardParameter_Click( object sender, EventArgs e ) {
            trackSelector.SelectNextCurve();
        }

        private void menuHiddenVisualBackwardParameter_Click( object sender, EventArgs e ) {
            trackSelector.SelectPreviousCurve();
        }

        private void menuVisualWaveform_CheckedChanged( object sender, EventArgs e ) {
            AppManager.editorConfig.ViewWaveform = menuVisualWaveform.Checked;
            UpdateSplitContainer2Size();
        }

        private void menuVisualControlTrack_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Show/Hide control curves." );
        }

        private void menuVisualMixer_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Show/Hide mixer window." );
        }

        private void menuVisualWaveform_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Show/Hide waveform." );
        }

        private void menuVisualProperty_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Show/Hide property window." );
        }

        private void menuVisualGridline_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Show/Hide grid line." );
        }

        private void menuVisualStartMarker_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Enable/Disable start marker." );
        }

        private void menuVisualEndMarker_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Enable/Disable end marker." );
        }

        private void menuVisualLyrics_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Show/Hide lyrics." );
        }

        private void menuVisualNoteProperty_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Show/Hide expression lines." );
        }

        private void menuVisualPitchLine_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Show/Hide pitch bend lines." );
        }
        #endregion

        #region m_mixer_dlg
        private void m_mixer_dlg_SoloChanged( int track, boolean solo ) {
#if DEBUG
            AppManager.debugWriteLine( "m_mixer_dlg_SoloChanged" );
            AppManager.debugWriteLine( "    track=" + track );
            AppManager.debugWriteLine( "    solo=" + solo );
#endif
            if ( track == 0 ) {
                // ここはなし
            } else if ( track > 0 ) {
                AppManager.getVsqFile().Mixer.Slave.get( track - 1 ).Solo = solo ? 1 : 0;
            } else {
                // ここもなし
            }
        }

        private void m_mixer_dlg_MuteChanged( int track, boolean mute ) {
#if DEBUG
            AppManager.debugWriteLine( "m_mixer_dlg_MuteChanged" );
            AppManager.debugWriteLine( "    track=" + track );
            AppManager.debugWriteLine( "    mute=" + mute );
#endif
            if ( track == 0 ) {
                AppManager.getVsqFile().Mixer.MasterMute = mute ? 1 : 0;
            } else if ( track > 0 ) {
                AppManager.getVsqFile().Mixer.Slave.get( track - 1 ).Mute = mute ? 1 : 0;
            } else {
                AppManager.getBgm( -track - 1 ).mute = mute ? 1 : 0;
            }
        }

        private void m_mixer_dlg_PanpotChanged( int track, int panpot ) {
            if ( track == 0 ) {
                // master
                AppManager.getVsqFile().Mixer.MasterPanpot = panpot;
            } else if ( track > 0 ) {
                // slave
                AppManager.getVsqFile().Mixer.Slave.get( track - 1 ).Panpot = panpot;
            } else {
                AppManager.getBgm( -track - 1 ).panpot = panpot;
            }
        }

        private void m_mixer_dlg_FederChanged( int track, int feder ) {
#if DEBUG
            Console.WriteLine( "FormMain#m_mixer_dlg_FederChanged; track=" + track + "; feder=" + feder );
#endif
            if ( track == 0 ) {
                AppManager.getVsqFile().Mixer.MasterFeder = feder;
            } else if ( track > 0 ) {
                AppManager.getVsqFile().Mixer.Slave.get( track - 1 ).Feder = feder;
            } else {
                AppManager.getBgm( -track - 1 ).feder = feder;
            }
        }

        private void m_mixer_dlg_TopMostChanged( boolean arg ) {
            AppManager.editorConfig.MixerTopMost = arg;
        }
        #endregion

        #region FormMain
        private void FormMain_FormClosed( object sender, FormClosedEventArgs e ) {
            ClearTempWave();
            String tempdir = AppManager.getTempWaveDir();
            String log = Path.Combine( tempdir, "run.log" );
            bocoree.debug.close();
            try {
                if ( PortUtil.isFileExists( log ) ) {
                    PortUtil.deleteFile( log );
                }
                Directory.Delete( tempdir, true );
            } catch ( Exception ex ) {
            }
            VSTiProxy.abortRendering();
            VSTiProxy.terminate();
            MidiPlayer.Stop();
        }

        private void FormMain_FormClosing( object sender, FormClosingEventArgs e ) {
            if ( Edited ) {
                String file = AppManager.getFileName();
                if ( file.Equals( "" ) ) {
                    file = "Untitled";
                } else {
                    file = Path.GetFileName( file );
                }
                DialogResult ret = MessageBox.Show( _( "Save this sequence?" ),
                                                    _( "Affirmation" ),
                                                    MessageBoxButtons.YesNoCancel,
                                                    MessageBoxIcon.Question );
                switch ( ret ) {
                    case DialogResult.Yes:
                        if ( AppManager.getFileName().Equals( "" ) ) {
                            DialogResult dr = DialogResult.Cancel;
                            if ( AppManager.editorConfig.UseCustomFileDialog ) {
                                using ( FileDialog fd = new FileDialog( FileDialog.DialogMode.Save ) ) {
                                    if ( saveXmlVsqDialog.FileName != "" ) {
                                        fd.FileName = saveXmlVsqDialog.FileName;
                                    }
                                    fd.Filter = saveXmlVsqDialog.Filter;
                                    dr = fd.ShowDialog();
                                    if ( dr == DialogResult.OK ) {
                                        saveXmlVsqDialog.FileName = fd.FileName;
                                    }
                                }
                            } else {
                                dr = saveXmlVsqDialog.ShowDialog();
                            }
                            if ( dr == DialogResult.OK ) {
                                AppManager.saveTo( saveXmlVsqDialog.FileName );
                            } else {
                                e.Cancel = true;
                                return;
                            }
                        } else {
                            AppManager.saveTo( AppManager.getFileName() );
                        }
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                }
            }
            AppManager.editorConfig.WindowMaximized = (this.WindowState == FormWindowState.Maximized);
            AppManager.saveConfig();
            UtauRenderingRunner.clearCache();
            StraightRenderingRunner.clearCache();
            if ( m_midi_in != null ) {
                m_midi_in.Dispose();
            }
            bgWorkScreen.Dispose();
            e.Cancel = false;
        }

        private void FormMain_Load( object sender, EventArgs e ) {
            applyLanguage();
            trackBar.Value = AppManager.editorConfig.DefaultXScale;
            AppManager.setCurrentClock( 0 );
            Edited = false;

            AppManager.PreviewStarted += new EventHandler( AppManager_PreviewStarted );
            AppManager.PreviewAborted += new EventHandler( AppManager_PreviewAborted );
            AppManager.GridVisibleChanged += new EventHandler( AppManager_GridVisibleChanged );
            AppManager.SelectedEventChanged += new BSimpleDelegate<boolean>( AppManager_SelectedEventChanged );
            AppManager.CurrentClockChanged += new EventHandler( AppManager_CurrentClockChanged );
            AppManager.SelectedToolChanged += new EventHandler( AppManager_SelectedToolChanged );
            EditorConfig.QuantizeModeChanged += new EventHandler( EditorConfig_QuantizeModeChanged );
            m_property_panel_container.StateChangeRequired += new BSimpleDelegate<PropertyPanelState.PanelState>( m_property_panel_container_StateChangeRequired );

            UpdateRecentFileMenu();

            // C3が画面中央に来るように調整
            int draft_start_to_draw_y = AppManager.KEY_LENGTH * AppManager.editorConfig.PxTrackHeight - pictPianoRoll.Height / 2;
            int draft_vscroll_value = (int)((draft_start_to_draw_y * (double)vScroll.Maximum) / (128 * AppManager.editorConfig.PxTrackHeight - vScroll.Height));
            try {
                vScroll.Value = draft_vscroll_value;
            } catch {
            }

            // x=97がプリメジャークロックになるように調整
            int cp = AppManager.getVsqFile().getPreMeasureClocks();
            int draft_hscroll_value = (int)(cp - 24.0 / AppManager.scaleX);
            try {
                hScroll.Value = draft_hscroll_value;
            } catch {
            }

            s_pen_dashed_171_171_171.DashPattern = new float[] { 3, 3 };
            s_pen_dashed_209_204_172.DashPattern = new float[] { 3, 3 };

            menuVisualNoteProperty.Checked = AppManager.editorConfig.ShowExpLine;
            menuVisualLyrics.Checked = AppManager.editorConfig.ShowLyric;
            menuVisualMixer.Checked = AppManager.editorConfig.MixerVisible;
            menuVisualPitchLine.Checked = AppManager.editorConfig.ViewAtcualPitch;

            AppManager.mixerWindow = new FormMixer( this );

            updateMenuFonts();

            AppManager.mixerWindow.FederChanged += new FormMixer.FederChangedEventHandler( m_mixer_dlg_FederChanged );
            AppManager.mixerWindow.PanpotChanged += new FormMixer.PanpotChangedEventHandler( m_mixer_dlg_PanpotChanged );
            AppManager.mixerWindow.MuteChanged += new FormMixer.MuteChangedEventHandler( m_mixer_dlg_MuteChanged );
            AppManager.mixerWindow.SoloChanged += new FormMixer.SoloChangedEventHandler( m_mixer_dlg_SoloChanged );
            AppManager.mixerWindow.TopMostChanged += new BSimpleDelegate<boolean>( m_mixer_dlg_TopMostChanged );
            AppManager.mixerWindow.ShowTopMost = AppManager.editorConfig.MixerTopMost;
            AppManager.mixerWindow.updateStatus();
            if ( AppManager.editorConfig.MixerVisible ) {
                AppManager.mixerWindow.Show();
            }

            trackSelector.CommandExecuted += new TrackSelector.CommandExecutedEventHandler( trackSelector_CommandExecuted );

            UpdateScriptShortcut();

            ClearTempWave();
            SetHScrollRange( hScroll.Maximum );
            SetVScrollRange( vScroll.Maximum );
            m_pencil_mode.Mode = PencilModeEnum.Off;
            UpdateCMenuPianoFixed();
            GameControlerLoad();
            ReloadMidiIn();
            menuVisualWaveform.Checked = AppManager.editorConfig.ViewWaveform;
            UpdateSplitContainer2Size();

            updateRendererMenu();

            if ( AppManager.editorConfig.WindowMaximized ) {
                this.WindowState = FormWindowState.Maximized;
            } else {
                this.WindowState = FormWindowState.Normal;
            }
            this.Bounds = AppManager.editorConfig.WindowRect;
            UpdateLayout();

            // プロパティウィンドウの位置を復元
            Rectangle rcScreen = Screen.GetWorkingArea( this );
            Point p = this.Location;
            Point p0 = AppManager.editorConfig.PropertyWindowStatus.Bounds.Location;
            Point a = new Point( p.X + p0.X, p.Y + p0.Y );
            Rectangle rc = new Rectangle( a.X,
                                          a.Y,
                                          AppManager.editorConfig.PropertyWindowStatus.Bounds.Width,
                                          AppManager.editorConfig.PropertyWindowStatus.Bounds.Height );

            if ( a.Y > rcScreen.Top + rcScreen.Height ) {
                a = new Point( a.X, rcScreen.Top + rcScreen.Height - rc.Height );
            }
            if ( a.Y < rcScreen.Top ) {
                a = new Point( a.X, rcScreen.Top );
            }
            if ( a.X > rcScreen.Left + rcScreen.Width ) {
                a = new Point( rcScreen.Left + rcScreen.Width - rc.Width, a.Y );
            }
            if ( a.X < rcScreen.Left ) {
                a = new Point( rcScreen.Left, a.Y );
            }
#if DEBUG
            AppManager.debugWriteLine( "FormMain_Load; a=" + a );
#endif
            AppManager.propertyWindow.Bounds = new Rectangle( a.X, a.Y, rc.Width, rc.Height );
            AppManager.propertyWindow.LocationChanged += new EventHandler( m_note_proerty_dlg_LocationOrSizeChanged );
            AppManager.propertyWindow.SizeChanged += new EventHandler( m_note_proerty_dlg_LocationOrSizeChanged );
            AppManager.propertyWindow.FormClosing += new FormClosingEventHandler( m_note_proerty_dlg_FormClosing );
            AppManager.propertyPanel.CommandExecuteRequired += new CommandExecuteRequiredEventHandler( m_note_proerty_dlg_CommandExecuteRequired );
            AppManager.propertyWindow.FormCloseShortcutKey = AppManager.editorConfig.GetShortcutKeyFor( menuVisualProperty );
            UpdatePropertyPanelState( AppManager.editorConfig.PropertyWindowStatus.State );
            updateBgmMenuState();

            this.SizeChanged += new System.EventHandler( this.FormMain_SizeChanged );
            this.LocationChanged += new System.EventHandler( this.FormMain_LocationChanged );
            Refresh();
#if DEBUG
            //VocaloSysUtil_DRAFT.getLanguageFromName( "" );
            /*ExpressionConfigSys exp_config_sys = new ExpressionConfigSys( @"C:\Program Files\VOCALOID2\expdbdir" );
            Console.WriteLine( "vibrato:" );
            for ( Iterator itr = exp_config_sys.vibratoConfigIterator(); itr.hasNext(); ) {
                VibratoConfig vc = (VibratoConfig)itr.next();
                Console.WriteLine( "file=" + vc.file );
            }
            Console.WriteLine( "attack:" );
            for ( Iterator itr = exp_config_sys.attackConfigIterator(); itr.hasNext(); ) {
                AttackConfig ac = (AttackConfig)itr.next();
                Console.WriteLine( "file=" + ac.file );
            }

            byte[] dat = BitConverter.GetBytes( 4 );
            using ( StreamWriter sw = new StreamWriter( @"C:\get_bytes.txt" ) ) {
                for ( int i = 0; i < dat.Length; i++ ) {
                    sw.WriteLine( dat[i] );
                }
            }
            using ( TextMemoryStream tms = new TextMemoryStream( @"C:\a.txt", Encoding.ASCII ) ) {
                tms.rewind();
                while ( tms.peek() >= 0 ) {
                    Console.WriteLine( tms.readLine() );
                }
            }
            WaveDrawContext wdc = new WaveDrawContext( @"C:\ぴょ.wav" );
            using ( Bitmap b = new Bitmap( 500, 200 ) ) {
                using ( Graphics g = Graphics.FromImage( b ) ) {
                    wdc.Draw( g, Pens.Black, new Rectangle( 0, 0, 500, 200 ), 0.0f, 0.5f );
                }
                b.Save( @"C:\ぴょ.wav.png", System.Drawing.Imaging.ImageFormat.Png );
            }
            try {
                UtauFreq uf = UtauFreq.FromFrq( @"C:\あ_wav.frq" );
                uf.Write( new FileStream( @"C:\regenerated.frq", FileMode.Create, FileAccess.Write ) );
            } catch {
            }*/
            menuHidden.Visible = true;
            /*using ( StreamWriter sw = new StreamWriter( Path.Combine( Application.StartupPath, "Keys.txt" ) ) ) {
                foreach ( Keys key in Enum.GetValues( typeof( Keys ) ) ) {
                    sw.WriteLine( (int)key + "\t" + key.ToString() );
                }
            }*/
            /*OpenFileDialog ofd = new OpenFileDialog();
            XmlSerializer xs = new XmlSerializer( typeof( VsqFileEx ) );
            while ( ofd.ShowDialog() == DialogResult.OK ) {
                VsqFileEx vsq = new VsqFileEx( ofd.FileName );
                vsq.Track.get( 1 ).getEvent( 1 ).UstEvent = new UstEvent();
                using ( FileStream fs = new FileStream( ofd.FileName + "_regen.xml", FileMode.Create ) ) {
                    xs.Serialize( fs, vsq );
                }
            }*/
            /*Cursor c = SynthCursor( Properties.Resources.arrow_135 );
            if ( c != null ) {
                HAND = c;
            }*/
            /*MessageBody mb = new MessageBody( "ja", Path.Combine( Application.StartupPath, "ja.po" ) );
            mb.Write( Path.Combine( Application.StartupPath, "foo.po" ) );*/
            /*OpenFileDialog ofd = new OpenFileDialog();
            Wave.TestEnabled = true;
            while ( ofd.ShowDialog() == DialogResult.OK ) {
                String file = Path.Combine( Path.GetDirectoryName( ofd.FileName ), Path.GetFileNameWithoutExtension( ofd.FileName ) + ".txt" );
                using ( StreamWriter sw = new StreamWriter( file ) )
                using ( Wave w = new Wave( ofd.FileName ) ) {
                    w.TrimSilence();
                    int WID = 2048;
                    double[] wind = new double[WID];
                    for ( int j = 0; j < WID; j++ ) {
                        wind[j] = bocoree.math.window_func( bocoree.math.WindowFunctionType.Hamming, (double)j / (double)WID );
                    }
                    uint i = w.SampleRate;
                    //for ( uint i = 0; i < w.TotalSamples; i+=10 ) {
                        double f0 = w.TEST_GetF0( i, wind );
                        double n = 12.0 * Math.Log( f0 / 440.0, 2.0 ) + 69.0;
                        sw.WriteLine( i / (double)w.SampleRate + "\t" + n + "\t" + f0 );
                    //}
                }
            }*/
            /*bocoree.debug.push_log( "installed singers 1" );
            SingerConfig[] s1 = VocaloSysUtil.getInstalledSingers1();
            foreach ( SingerConfig sc in s1 ) {
                bocoree.debug.push_log( "    " + sc );
            }
            bocoree.debug.push_log( "installed singers 2" );
            SingerConfig[] s2 = VocaloSysUtil.getInstalledSingers2();
            foreach ( SingerConfig sc in s2 ) {
                bocoree.debug.push_log( "    " + sc );
            }
            if ( AppManager.EditorConfig.PathUtauVSTi != "" ) {
                bocoree.debug.push_log( "installed singers utau" );
                UtauSingerConfigSys uscs = new UtauSingerConfigSys( Path.GetDirectoryName( AppManager.EditorConfig.PathUtauVSTi ) );
                s2 = uscs.getInstalledSingers();
                foreach ( SingerConfig sc in s2 ) {
                    bocoree.debug.push_log( "    " + sc );
                }
            }
            Console.WriteLine( VocaloSysUtil.getLanguage2( 0 ) );*/

            /*OpenFileDialog ofd = new OpenFileDialog();
                const String format = "    {0,8} 0x{1:X4} {2,-32} 0x{3:X2} 0x{4:X2}";
                const String format0 = "    {0,8} 0x{1:X4} {2,-32} 0x{3:X2}";
            while ( ofd.ShowDialog() == DialogResult.OK ) {
                VsqFile vf = new VsqFile( ofd.FileName );
                vf.getTrack( 1 ).getCommon().Version = "UTU000";
                VsqNrpn[] nrpns = VsqFile.generateNRPN( vf, 1, 500 );
                String file = Path.Combine( Path.GetDirectoryName( ofd.FileName ), Path.GetFileNameWithoutExtension( ofd.FileName ) + "_regen.txt" );
                using ( StreamWriter sw = new StreamWriter( file ) ) {
                    for ( int i = 0; i < nrpns.Length; i++ ) {
                        VsqNrpn vn = nrpns[i];
                        if ( vn.DataLsbSpecified ) {
                            sw.WriteLine( String.Format( format, vn.Clock, vn.Nrpn, NRPN.getName( vn.Nrpn ), vn.DataMsb, vn.DataLsb ) );
                        } else {
                            sw.WriteLine( String.Format( format0, vn.Clock, vn.Nrpn, NRPN.getName( vn.Nrpn ), vn.DataMsb ) );
                        }
                    }
                }
            }*/
            /*unsafe {
                WavePlay w = new WavePlay( 44100, 44100 );
                w.on_your_mark( new String[] { }, 0 );
                float* left = (float*)System.Runtime.InteropServices.Marshal.AllocHGlobal( sizeof( float ) * 10000 );
                float* right = (float*)System.Runtime.InteropServices.Marshal.AllocHGlobal( sizeof( float ) * 10000 );
                float** buf = (float**)System.Runtime.InteropServices.Marshal.AllocHGlobal( sizeof( float* ) * 2 );
                buf[0] = left;
                buf[1] = right;
                float wv = 0.0f;
                for ( int i = 0; i < 10000; i++ ) {
                    wv += 0.002f;
                    if ( wv > 0.2f ) {
                        wv = -0.2f;
                    }
                    left[i] = wv;
                    right[i] = -wv;
                }
                for ( int i = 0; i < 100; i++ ) {
                    w.append( buf, 10000, 0.2, 0.2 );
                }
                w.flush_and_exit( 0.2, 0.2 );
                while ( w.is_alive() ) {
                }
            }*/
#endif
        }

        private void m_property_panel_container_StateChangeRequired( PropertyPanelState.PanelState arg ) {
            UpdatePropertyPanelState( arg );
        }

        private void m_note_proerty_dlg_CommandExecuteRequired( CadenciiCommand command ) {
#if DEBUG
            AppManager.debugWriteLine( "m_note_property_dlg_CommandExecuteRequired" );
#endif
            AppManager.register( AppManager.getVsqFile().executeCommand( command ) );
            UpdateDrawObjectList();
            refreshScreen();
            Edited = true;
        }

        private void m_note_proerty_dlg_FormClosing( object sender, FormClosingEventArgs e ) {
            if ( e.CloseReason == CloseReason.UserClosing ) {
                e.Cancel = true;
                UpdatePropertyPanelState( PropertyPanelState.PanelState.Hidden );
            }
        }

        private void m_note_proerty_dlg_LocationOrSizeChanged( object sender, EventArgs e ) {
#if DEBUG
            Console.WriteLine( "m_note_proeprty_dlg_LocationOrSizeChanged; WindowState=" + AppManager.propertyWindow.WindowState );
#endif
            if ( AppManager.editorConfig.PropertyWindowStatus.State == PropertyPanelState.PanelState.Window ) {
                if ( AppManager.propertyWindow.WindowState == FormWindowState.Minimized ) {
                    UpdatePropertyPanelState( PropertyPanelState.PanelState.Docked );
                } else {
                    Point parent = this.Location;
                    Point proeprty = AppManager.propertyWindow.Location;
                    AppManager.editorConfig.PropertyWindowStatus.Bounds = new Rectangle( proeprty.X - parent.X,
                                                                                         proeprty.Y - parent.Y,
                                                                                         AppManager.propertyWindow.Width,
                                                                                         AppManager.propertyWindow.Height );
                }
            }
        }

        private void FormMain_LocationChanged( object sender, EventArgs e ) {
            if ( this.WindowState == FormWindowState.Normal ) {
                AppManager.editorConfig.WindowRect = this.Bounds;
            }
        }

        private void FormMain_SizeChanged( object sender, EventArgs e ) {
            if ( this.WindowState == FormWindowState.Normal ) {
                AppManager.editorConfig.WindowRect = this.Bounds;
                AppManager.propertyWindow.WindowState = FormWindowState.Normal;
                AppManager.propertyWindow.Visible = AppManager.editorConfig.PropertyWindowStatus.State == PropertyPanelState.PanelState.Window;
                AppManager.mixerWindow.Visible = AppManager.editorConfig.MixerVisible;
                UpdateLayout();
            } else if ( this.WindowState == FormWindowState.Minimized ) {
                AppManager.propertyWindow.Visible = false;
                AppManager.mixerWindow.Visible = false;
            } else if ( this.WindowState == FormWindowState.Maximized ) {
                AppManager.propertyWindow.WindowState = FormWindowState.Normal;
                AppManager.propertyWindow.Visible = AppManager.editorConfig.PropertyWindowStatus.State == PropertyPanelState.PanelState.Window;
                AppManager.mixerWindow.Visible = AppManager.editorConfig.MixerVisible;
            }
        }

        private void FormMain_MouseWheel( object sender, MouseEventArgs e ) {
            if ( (Control.ModifierKeys & Keys.Shift) == Keys.Shift ) {
                hScroll.Value = NewHScrollValueFromWheelDelta( e.Delta );
            } else {
                double new_val = (double)vScroll.Value - e.Delta;
                if ( new_val > vScroll.Maximum ) {
                    vScroll.Value = vScroll.Maximum;
                } else if ( new_val < vScroll.Minimum ) {
                    vScroll.Value = vScroll.Minimum;
                } else {
                    vScroll.Value = (int)new_val;
                }
            }
            refreshScreen();
        }

        private void FormMain_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e ) {
            ProcessSpecialShortcutKey( e );
        }

        private void FormMain_Deactivate( object sender, EventArgs e ) {
            m_form_activated = false;
        }

        private void FormMain_Activated( object sender, EventArgs e ) {
            m_form_activated = true;
        }
        #endregion

        private void m_timer_Tick( object sender, EventArgs e ) {
            if ( !m_form_activated ) {
                return;
            }
            try {
                DateTime now = DateTime.Now;
                byte[] buttons;
                int pov0;
                winmmhelp.JoyGetStatus( 0, out buttons, out pov0 );
                boolean event_processed = false;
                double dt_ms = now.Subtract( m_last_event_processed ).TotalMilliseconds;

                EditorConfig m = AppManager.editorConfig;
                boolean btn_x = (0 <= m.GameControlerCross && m.GameControlerCross < buttons.Length && buttons[m.GameControlerCross] > 0x00);
                boolean btn_o = (0 <= m.GameControlerCircle && m.GameControlerCircle < buttons.Length && buttons[m.GameControlerCircle] > 0x00);
                boolean btn_tr = (0 <= m.GameControlerTriangle && m.GameControlerTriangle < buttons.Length && buttons[m.GameControlerTriangle] > 0x00);
                boolean btn_re = (0 <= m.GameControlerRectangle && m.GameControlerRectangle < buttons.Length && buttons[m.GameControlerRectangle] > 0x00);
                boolean pov_r = pov0 == m.GameControlPovRight;
                boolean pov_l = pov0 == m.GameControlPovLeft;
                boolean pov_u = pov0 == m.GameControlPovUp;
                boolean pov_d = pov0 == m.GameControlPovDown;
                boolean L1 = (0 <= m.GameControlL1 && m.GameControlL1 < buttons.Length && buttons[m.GameControlL1] > 0x00);
                boolean R1 = (0 <= m.GameControlL2 && m.GameControlL2 < buttons.Length && buttons[m.GameControlR1] > 0x00);
                boolean L2 = (0 <= m.GameControlR1 && m.GameControlR1 < buttons.Length && buttons[m.GameControlL2] > 0x00);
                boolean R2 = (0 <= m.GameControlR2 && m.GameControlR2 < buttons.Length && buttons[m.GameControlR2] > 0x00);
                boolean SELECT = (0 <= m.GameControlSelect && m.GameControlSelect <= buttons.Length && buttons[m.GameControlSelect] > 0x00);
                if ( m_game_mode == GameControlMode.NORMAL ) {
                    m_last_btn_x = btn_x;

                    if ( !event_processed && !btn_o && m_last_btn_o ) {
                        if ( AppManager.isPlaying() ) {
                            timer.Stop();
                        }
                        AppManager.setPlaying( !AppManager.isPlaying() );
                        m_last_event_processed = now;
                        event_processed = true;
                    }
                    m_last_btn_o = btn_o;

                    if ( !event_processed && pov_r && dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval ) {
                        Forward();
                        m_last_event_processed = now;
                        event_processed = true;
                    }
                    m_last_pov_r = pov_r;

                    if ( !event_processed && pov_l && dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval ) {
                        Rewind();
                        m_last_event_processed = now;
                        event_processed = true;
                    }
                    m_last_pov_l = pov_l;

                    if ( !event_processed && pov_u && dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval ) {
                        int draft_vscroll = vScroll.Value - AppManager.editorConfig.PxTrackHeight * 3;
                        if ( draft_vscroll < vScroll.Minimum ) {
                            draft_vscroll = vScroll.Minimum;
                        }
                        vScroll.Value = draft_vscroll;
                        refreshScreen();
                        m_last_event_processed = now;
                        event_processed = true;
                    }

                    if ( !event_processed && pov_d && dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval ) {
                        int draft_vscroll = vScroll.Value + AppManager.editorConfig.PxTrackHeight * 3;
                        if ( draft_vscroll > vScroll.Maximum ) {
                            draft_vscroll = vScroll.Maximum;
                        }
                        vScroll.Value = draft_vscroll;
                        refreshScreen();
                        m_last_event_processed = now;
                        event_processed = true;
                    }

                    if ( !event_processed && !SELECT && m_last_select ) {
                        event_processed = true;
                        m_game_mode = GameControlMode.KEYBOARD;
                        stripLblGameCtrlMode.Text = m_game_mode.ToString();
                        stripLblGameCtrlMode.Image = Properties.Resources.piano;
                    }
                    m_last_select = SELECT;
                } else if ( m_game_mode == GameControlMode.KEYBOARD ) {
                    if ( !event_processed && !SELECT && m_last_select ) {
                        event_processed = true;
                        m_game_mode = GameControlMode.NORMAL;
                        UpdateGameControlerStatus();
                        m_last_select = SELECT;
                        return;
                    }
                    m_last_select = SELECT;
                    if ( L1 && R1 && L2 && R2 && dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval ) {
                        if ( AppManager.isPlaying() ) {
                            AppManager.setEditMode( EditMode.NONE );
                            AppManager.setPlaying( false );
                            timer.Stop();
                        } else {
                            m_timer.Enabled = false;
                            using ( FormRealtimeConfig frc = new FormRealtimeConfig() ) {
                                if ( frc.ShowDialog() == DialogResult.OK ) {
                                    m_adding = null;
                                    AppManager.setEditMode( EditMode.REALTIME );
                                    AppManager.editorConfig.RealtimeInputSpeed = frc.Speed;
                                    AppManager.setPlaying( true );
                                }
                            }
                            m_timer.Enabled = true;
                        }
                        m_last_btn_o = btn_o;
                        m_last_btn_x = btn_x;
                        m_last_btn_re = btn_re;
                        m_last_btn_tr = btn_tr;
                        m_last_pov_l = pov_l;
                        m_last_pov_d = pov_d;
                        m_last_pov_r = pov_r;
                        m_last_pov_u = pov_u;
                        return;
                    }

                    int note = -1;
                    if ( pov_r && !m_last_pov_r ) {
                        note = 60;
                    } else if ( btn_re && !m_last_btn_re ) {
                        note = 62;
                    } else if ( btn_tr && !m_last_btn_tr ) {
                        note = 64;
                    } else if ( btn_o && !m_last_btn_o ) {
                        note = 65;
                    } else if ( btn_x && !m_last_btn_x ) {
                        note = 67;
                    } else if ( pov_u && !m_last_pov_u ) {
                        note = 59;
                    } else if ( pov_l && !m_last_pov_l ) {
                        note = 57;
                    } else if ( pov_d && !m_last_pov_d ) {
                        note = 55;
                    }
                    if ( note >= 0 ) {
                        if ( L1 ) {
                            note += 12;
                        } else if ( L2 ) {
                            note -= 12;
                        }
                        if ( R1 ) {
                            note += 1;
                        } else if ( R2 ) {
                            note -= 1;
                        }
                    }
                    m_last_btn_o = btn_o;
                    m_last_btn_x = btn_x;
                    m_last_btn_re = btn_re;
                    m_last_btn_tr = btn_tr;
                    m_last_pov_l = pov_l;
                    m_last_pov_d = pov_d;
                    m_last_pov_r = pov_r;
                    m_last_pov_u = pov_u;
                    if ( note >= 0 ) {
#if DEBUG
                        AppManager.debugWriteLine( "FormMain+m_timer_Tick" );
                        AppManager.debugWriteLine( "    note=" + note );
#endif
                        if ( AppManager.isPlaying() ) {
                            int clock = AppManager.getCurrentClock();
                            if ( m_adding != null ) {
                                m_adding.ID.Length = clock - m_adding.Clock;
                                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventAdd( AppManager.getSelected(),
                                                                                                        m_adding ) );
                                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                if ( !Edited ) {
                                    Edited = true;
                                }
#if USE_DOBJ
                                UpdateDrawObjectList();
#endif
                            }
                            m_adding = new VsqEvent( clock, new VsqID( 0 ) );
                            m_adding.ID.type = VsqIDType.Anote;
                            m_adding.ID.Dynamics = 64;
                            m_adding.ID.VibratoHandle = null;
                            m_adding.ID.LyricHandle = new LyricHandle( "a", "a" );
                            m_adding.ID.Note = note;
                        }
                        if ( AppManager.getEditMode() == EditMode.REALTIME ) {
                            MidiPlayer.PlayImmediate( (byte)note );
                        } else {
                            KeySoundPlayer.Play( note );
                        }
                    } else {
                        if ( AppManager.isPlaying() && m_adding != null ) {
                            m_adding.ID.Length = AppManager.getCurrentClock() - m_adding.Clock;
                        }
                    }
                }
            } catch ( Exception ex ) {
#if DEBUG
                AppManager.debugWriteLine( "    ex=" + ex );
#endif
                m_game_mode = GameControlMode.DISABLED;
                UpdateGameControlerStatus();
                if ( AppManager.getEditMode() == EditMode.REALTIME ) {
                    AppManager.setPlaying( false );
                    AppManager.setEditMode( EditMode.NONE );
                    m_adding = null;
                }
                m_timer.Stop();
            }
        }

        private void EditorConfig_QuantizeModeChanged( object sender, EventArgs e ) {
            ApplyQuantizeMode();
        }

        #region menuFile*
        private void menuFileSaveNamed_Click( object sender, EventArgs e ) {
            for ( int track = 1; track < AppManager.getVsqFile().Track.size(); track++ ) {
                if ( AppManager.getVsqFile().Track.get( track ).getEventCount() == 0 ) {
                    MessageBox.Show(
                        String.Format(
                            _( "Invalid note data.\nTrack {0} : {1}\n\n-> Piano roll : Blank sequence." ), track, AppManager.getVsqFile().Track.get( track ).getName()
                        ),
                        _APP_NAME,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation );
                    return;
                }
            }
            DialogResult dr = DialogResult.Cancel;
            if ( AppManager.editorConfig.UseCustomFileDialog ) {
                using ( FileDialog fd = new FileDialog( FileDialog.DialogMode.Open ) ) {
                    if ( saveXmlVsqDialog.FileName != "" ) {
                        fd.FileName = saveXmlVsqDialog.FileName;
                    }
                    fd.Filter = saveXmlVsqDialog.Filter;
                    dr = fd.ShowDialog();
                    if ( dr == DialogResult.OK ) {
                        saveXmlVsqDialog.FileName = fd.FileName;
                    }
                }
            } else {
                dr = saveXmlVsqDialog.ShowDialog();
            }

            if ( dr == DialogResult.OK ) {
                String file = saveXmlVsqDialog.FileName;
                AppManager.saveTo( file );
                UpdateRecentFileMenu();
                Edited = false;
            }
        }

        private void commonFileSave_Click( object sender, EventArgs e ) {
            for ( int track = 1; track < AppManager.getVsqFile().Track.size(); track++ ) {
                if ( AppManager.getVsqFile().Track.get( track ).getEventCount() == 0 ) {
                    MessageBox.Show(
                        String.Format(
                            _( "Invalid note data.\nTrack {0} : {1}\n\n-> Piano roll : Blank sequence." ), track, AppManager.getVsqFile().Track.get( track ).getName()
                        ),
                        _APP_NAME,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation );
                    return;
                }
            }
            String file = AppManager.getFileName();
            if ( AppManager.getFileName().Equals( "" ) ) {
                DialogResult dr = DialogResult.Cancel;
                if ( AppManager.editorConfig.UseCustomFileDialog ) {
                    using ( FileDialog fd = new FileDialog( FileDialog.DialogMode.Open ) ) {
                        if ( saveXmlVsqDialog.FileName != "" ) {
                            fd.FileName = saveXmlVsqDialog.FileName;
                        }
                        fd.Filter = saveXmlVsqDialog.Filter;
                        dr = fd.ShowDialog();
                        if ( dr == DialogResult.OK ) {
                            saveXmlVsqDialog.FileName = fd.FileName;
                        }
                    }
                } else {
                    dr = saveXmlVsqDialog.ShowDialog();
                }

                if ( dr == DialogResult.OK ) {
                    file = saveXmlVsqDialog.FileName;
                }
            }
            if ( file != "" ) {
                AppManager.saveTo( file );
                UpdateRecentFileMenu();
                Edited = false;
            }
        }

        private void menuFileQuit_Click( object sender, EventArgs e ) {
            this.Close();
        }

        private void menuFileExportWave_Click( object sender, EventArgs e ) {
            DialogResult dr = DialogResult.Cancel;
            String filename = "";
            if ( AppManager.editorConfig.UseCustomFileDialog ) {
                using ( FileDialog fd = new FileDialog( FileDialog.DialogMode.Save ) ) {
                    fd.Title = _( "Wave Export" );
                    try {
                        fd.Filter = _( "Wave File(*.wav)|*.wav" ) + "|" + _( "All Files(*.*)|*.*" );
                    } catch {
                        fd.Filter = "Wave File(*.wav)|*.wav|All Files(*.*)|*.*";
                    }
                    dr = fd.ShowDialog();
                    filename = fd.FileName;
                }
            } else {
                using ( SaveFileDialog sfd = new SaveFileDialog() ) {
                    sfd.Title = _( "Wave Export" );
                    try {
                        sfd.Filter = _( "Wave File(*.wav)|*.wav" ) + "|" + _( "All Files(*.*)|*.*" );
                    } catch {
                        sfd.Filter = "Wave File(*.wav)|*.wav|All Files(*.*)|*.*";
                    }
                    dr = sfd.ShowDialog();
                    filename = sfd.FileName;
                }
            }

            if ( dr == DialogResult.OK ) {
                using ( FormSynthesize fs = new FormSynthesize(
                    AppManager.getVsqFile(),
                    AppManager.editorConfig.PreSendTime,
                    new int[] { AppManager.getSelected() },
                    new String[] { filename },
                    AppManager.getVsqFile().TotalClocks + 240,
                    true ) ) {

                    DateTime started = DateTime.Now;
                    fs.ShowDialog();
#if DEBUG
                    bocoree.debug.push_log( "elapsed time=" + DateTime.Now.Subtract( started ).TotalSeconds + "sec" );
#endif
                }
            }
        }

        private void menuFileExport_DropDownOpening( object sender, EventArgs e ) {
            menuFileExportWave.Enabled = (AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventCount() > 0) && VSTiProxy.CurrentUser.Equals( "" );
        }

        private void menuFileImportMidi_Click( object sender, EventArgs e ) {
            if ( m_midi_imexport_dialog == null ) {
                m_midi_imexport_dialog = new FormMidiImExport();
            }
            m_midi_imexport_dialog.ListTrack.Items.Clear();
            m_midi_imexport_dialog.Mode = FormMidiImExport.FormMidiMode.Import;

            DialogResult dr = DialogResult.Cancel;
            if ( AppManager.editorConfig.UseCustomFileDialog ) {
                using ( FileDialog fd = new FileDialog( FileDialog.DialogMode.Open ) ) {
                    if ( openMidiDialog.FileName != "" ) {
                        fd.FileName = openMidiDialog.FileName;
                    }
                    fd.Filter = openMidiDialog.Filter;
                    fd.Location = GetFormPreferedLocation( fd );
                    dr = fd.ShowDialog();
                    if ( dr == DialogResult.OK ) {
                        openMidiDialog.FileName = fd.FileName;
                    }
                }
            } else {
                dr = openMidiDialog.ShowDialog();
            }

            if ( dr != DialogResult.OK ) {
                return;
            }
            m_midi_imexport_dialog.Location = GetFormPreferedLocation( m_midi_imexport_dialog );
            MidiFile mf = null;
            try {
                mf = new MidiFile( openMidiDialog.FileName );
            } catch ( Exception ex ) {
                MessageBox.Show( _( "Invalid MIDI file." ), _( "Error" ), MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                return;
            }
            if ( mf == null ) {
                MessageBox.Show( _( "Invalid MIDI file." ), _( "Error" ), MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                return;
            }
            int count = mf.getTrackCount();
            Encoding def_enc = Encoding.GetEncoding( 0 );
            for ( int i = 0; i < count; i++ ) {
                String track_name = "";
                int notes = 0;
                Vector<MidiEvent> events = mf.getMidiEventList( i );

                // トラック名を取得
                for ( int j = 0; j < events.size(); j++ ) {
                    if ( events.get( j ).FirstByte == 0xff && events.get( j ).Data.Length >= 2 && events.get( j ).Data[0] == 0x03 ) {
                        track_name = def_enc.GetString( events.get( j ).Data, 1, events.get( j ).Data.Length - 1 );
                        break;
                    }
                }

                // イベント数を数える
                for ( int j = 0; j < events.size(); j++ ) {
                    if ( (events.get( j ).FirstByte & 0xf0) == 0x90 && events.get( j ).Data.Length > 1 && events.get( j ).Data[1] > 0x00 ) {
                        notes++;
                    }
                }
                m_midi_imexport_dialog.ListTrack.Items.Add( new ListViewItem( new String[] { i.ToString(), track_name, notes.ToString() } ) );
                m_midi_imexport_dialog.ListTrack.Items[i].Checked = true;
            }

            if ( m_midi_imexport_dialog.ShowDialog() != DialogResult.OK ) {
                return;
            }

            // インポートするしないにかかわらずテンポと拍子を取得
            VsqFileEx tempo = new VsqFileEx( "Miku", 2, 4, 4, 500000 ); //テンポリスト用のVsqFile。テンポの部分のみ使用
            tempo.executeCommand( VsqCommand.generateCommandChangePreMeasure( 0 ) );
            boolean tempo_added = false;
            boolean timesig_added = false;
            tempo.TempoTable.clear();
            tempo.TimesigTable.clear();
            int mf_getTrackCount = mf.getTrackCount();
            for ( int i = 0; i < mf_getTrackCount; i++ ) {
                Vector<MidiEvent> events = mf.getMidiEventList( i );
                boolean t_tempo_added = false;   //第iトラックからテンポをインポートしたかどうか
                boolean t_timesig_added = false; //第iトラックから拍子をインポートしたかどうか
                int events_Count = events.size();
                for ( int j = 0; j < events_Count; j++ ) {
                    if ( !tempo_added && events.get( j ).FirstByte == 0xff && events.get( j ).Data.Length >= 4 && events.get( j ).Data[0] == 0x51 ) {
                        int vtempo = events.get( j ).Data[1] << 16 | events.get( j ).Data[2] << 8 | events.get( j ).Data[3];
                        tempo.TempoTable.add( new TempoTableEntry( (int)events.get( j ).Clock, vtempo, 0.0 ) );
                        t_tempo_added = true;
                    }
                    if ( !timesig_added && events.get( j ).FirstByte == 0xff && events.get( j ).Data.Length >= 5 && events.get( j ).Data[0] == 0x58 ) {
                        int num = events.get( j ).Data[1];
                        int den = 1;
                        for ( int k = 0; k < events.get( j ).Data[2]; k++ ) {
                            den = den * 2;
                        }
                        tempo.TimesigTable.add( new TimeSigTableEntry( (int)events.get( j ).Clock, num, den, 0 ) );
                        t_timesig_added = true;
                    }
                }
                if ( t_tempo_added ) {
                    tempo_added = true;
                }
                if ( t_timesig_added ) {
                    timesig_added = true;
                }
                if ( timesig_added && tempo_added ) {
                    // 両方ともインポート済みならexit。2個以上のトラックから、重複してテンポや拍子をインポートするのはNG（たぶん）
                    break;
                }
            }
            boolean contains_zero = false;
            for ( int i = 0; i < tempo.TempoTable.size(); i++ ) {
                if ( tempo.TempoTable.get( i ).Clock == 0 ) {
                    contains_zero = true;
                    break;
                }
            }
            if ( !contains_zero ) {
                tempo.TempoTable.add( new TempoTableEntry( 0, 500000, 0.0 ) );
            }
            contains_zero = false;
            for ( int i = 0; i < tempo.TimesigTable.size(); i++ ) {
                if ( tempo.TimesigTable.get( i ).Clock == 0 ) {
                    contains_zero = true;
                    break;
                }
            }
            if ( !contains_zero ) {
                tempo.TimesigTable.add( new TimeSigTableEntry( 0, 4, 4, 0 ) );
            }
            VsqFileEx work = (VsqFileEx)AppManager.getVsqFile().Clone(); //後でReplaceコマンドを発行するための作業用
            double sec_at_premeasure = work.getSecFromClock( work.getPreMeasureClocks() );
            if ( !m_midi_imexport_dialog.isPreMeasure() ){
                sec_at_premeasure = 0.0;
            }
            VsqFileEx copy_src = (VsqFileEx)tempo.Clone();
            if ( sec_at_premeasure != 0.0 ) {
                VsqFileEx.shift( copy_src, sec_at_premeasure );
            }
            tempo.updateTempoInfo();
            tempo.updateTimesigInfo();

            // tempoをインポート
            boolean import_tempo = m_midi_imexport_dialog.Tempo;
            if ( import_tempo ) {
                // 最初に、workにある全てのイベント・コントロールカーブ・ベジエ曲線をtempoのテンポテーブルに合うように、シフトする
                ShiftClockToMatchWith( work, copy_src, work.getSecFromClock( work.getPreMeasureClocks() ) );

                work.TempoTable.clear();
                Vector<TempoTableEntry> list = copy_src.TempoTable;
                int list_count = list.size();
                for ( int i = 0; i < list_count; i++ ) {
                    TempoTableEntry item = list.get( i );
                    work.TempoTable.add( new TempoTableEntry( item.Clock, item.Tempo, item.Time ) );
                }
                work.updateTempoInfo();
            }

            // timesig
            if ( m_midi_imexport_dialog.Timesig ) {
                work.TimesigTable.clear();
                Vector<TimeSigTableEntry> list = tempo.TimesigTable;
                int list_count = list.size();
                for ( int i = 0; i < list_count; i++ ) {
                    TimeSigTableEntry item = list.get( i );
                    work.TimesigTable.add( new TimeSigTableEntry( item.Clock,
                                                                  item.Numerator,
                                                                  item.Denominator,
                                                                  item.BarCount ) );
                }
                Collections.sort( work.TimesigTable );
                work.updateTimesigInfo();
            }

#if DEBUG
            AppManager.debugWriteLine( "menuFileImportMidi_Click" );
            AppManager.debugWriteLine( "    work.TempoTable" );
            for ( int i = 0; i < work.TempoTable.size(); i++ ) {
                AppManager.debugWriteLine( "        clock,tempo=" + work.TempoTable.get( i ).Clock + "," + work.TempoTable.get( i ).Tempo );
            }
            AppManager.debugWriteLine( "    tempo.TempoTable" );
            for ( int i = 0; i < tempo.TempoTable.size(); i++ ) {
                AppManager.debugWriteLine( "        clock,tempo=" + tempo.TempoTable.get( i ).Clock + "," + tempo.TempoTable.get( i ).Tempo );
            }
#endif

            for ( int i = 0; i < m_midi_imexport_dialog.ListTrack.Items.Count; i++ ) {
                if ( !m_midi_imexport_dialog.ListTrack.Items[i].Checked ) {
                    continue;
                }
                if ( work.Track.size() + 1 > 16 ) {
                    break;
                }
                VsqTrack work_track = new VsqTrack( m_midi_imexport_dialog.ListTrack.Items[i].SubItems[1].Text, "Miku" );
                Vector<MidiEvent> events = mf.getMidiEventList( i );
                Collections.sort( events );
#if DEBUG
                using ( StreamWriter sw = new StreamWriter( "C:\\import" + i + ".txt" ) ) {
                    int c = events.size();
                    sw.WriteLine( "clock\tfirstByte" );
                    for ( int j = 0; j < c; j++ ) {
                        MidiEvent item = events.get( j );
                        if ( (item.firstByte & 0xf0) == 0x90 || (item.firstByte & 0xf0) == 0x80 ) {
                            sw.WriteLine( item.clock + "\t0x" + string.Format( "{0:X}", item.firstByte ) );
                        }
                    }
                }
#endif

                // note
                if ( m_midi_imexport_dialog.Notes ) {
                    int[] onclock_each_note = new int[128];
                    for ( int j = 0; j < 128; j++ ) {
                        onclock_each_note[j] = -1;
                    }
                    for ( int j = 0; j < events.size(); j++ ) {
                        if ( ((events.get( j ).FirstByte & 0xf0) == 0x90 && events.get( j ).Data.Length >= 2 && events.get( j ).Data[1] == 0) ||
                             ((events.get( j ).FirstByte & 0xf0) == 0x80 && events.get( j ).Data.Length >= 2) ) {
                            int clock_off = (int)events.get( j ).Clock;
                            int note = (int)events.get( j ).Data[0];
                            if ( onclock_each_note[note] >= 0 ) {
                                double time_clock_on = tempo.getSecFromClock( onclock_each_note[note] ) + sec_at_premeasure;
                                double time_clock_off = tempo.getSecFromClock( clock_off ) + sec_at_premeasure;
                                int add_clock_on = (int)work.getClockFromSec( time_clock_on );
                                int add_clock_off = (int)work.getClockFromSec( time_clock_off );
                                VsqID vid = new VsqID( 0 );
                                vid.type = VsqIDType.Anote;
                                vid.Length = add_clock_off - add_clock_on;
                                String phrase = "a";
                                if ( m_midi_imexport_dialog.Lyric ) {
                                    for ( int k = 0; k < events.size(); k++ ) {
                                        if ( onclock_each_note[note] <= (int)events.get( k ).Clock && (int)events.get( k ).Clock <= clock_off ) {
                                            if ( events.get( k ).FirstByte == 0xff && events.get( k ).Data.Length >= 2 && events.get( k ).Data[0] == 0x05 ) {
                                                phrase = def_enc.GetString( events.get( k ).Data, 1, events.get( k ).Data.Length - 1 );
                                                break;
                                            }
                                        }
                                    }
                                }
                                vid.LyricHandle = new LyricHandle( phrase, "a" );
                                vid.Note = note;

                                // ビブラート
                                if ( AppManager.editorConfig.EnableAutoVibrato ) {
                                    int note_length = vid.Length;
                                    // 音符位置での拍子を調べる
                                    int denom, numer;
                                    work.getTimesigAt( add_clock_on, out numer, out denom );

                                    // ビブラートを自動追加するかどうかを決める閾値
                                    int threshold = 480 * 4 / denom * (int)AppManager.editorConfig.AutoVibratoMinimumLength;
                                    if ( note_length >= threshold ) {
                                        int vibrato_clocks = 0;
                                        switch ( AppManager.editorConfig.DefaultVibratoLength ) {
                                            case DefaultVibratoLength.L100:
                                                vibrato_clocks = note_length;
                                                break;
                                            case DefaultVibratoLength.L50:
                                                vibrato_clocks = note_length / 2;
                                                break;
                                            case DefaultVibratoLength.L66:
                                                vibrato_clocks = note_length * 2 / 3;
                                                break;
                                            case DefaultVibratoLength.L75:
                                                vibrato_clocks = note_length * 3 / 4;
                                                break;
                                        }
                                        // とりあえずVOCALOID2のデフォルトビブラートの設定を使用
                                        vid.VibratoHandle = VocaloSysUtil.getDefaultVibratoHandle( AppManager.editorConfig.AutoVibratoType2,
                                                                                                   vibrato_clocks,
                                                                                                   SynthesizerType.VOCALOID2 );
                                        vid.VibratoDelay = note_length - vibrato_clocks;
                                    }
                                }
                                
                                VsqEvent ve = new VsqEvent( add_clock_on, vid );
                                work_track.addEvent( ve );
                            }
                        }
                        if ( (events.get( j ).FirstByte & 0xf0) == 0x90 && events.get( j ).Data.Length >= 2 && events.get( j ).Data[1] > 0 ) {
                            int note = events.get( j ).Data[0];
                            onclock_each_note[note] = (int)events.get( j ).Clock;
                        }
                    }

                    int track = work.Track.size();
                    AppManager.setRenderRequired( track, true );
                    CadenciiCommand run_add = VsqFileEx.generateCommandAddTrack( work_track,
                                                                                 new VsqMixerEntry( 0, 0, 0, 0 ),
                                                                                 track,
                                                                                 new BezierCurves() );
                    work.executeCommand( run_add );
                }
            }

            CadenciiCommand lastrun = VsqFileEx.generateCommandReplace( work );
            AppManager.register( AppManager.getVsqFile().executeCommand( lastrun ) );
            Edited = true;
            refreshScreen();
        }

        private void menuFileExportMidi_Click( object sender, EventArgs e ) {
            if ( m_midi_imexport_dialog == null ) {
                m_midi_imexport_dialog = new FormMidiImExport();
            }
            m_midi_imexport_dialog.ListTrack.Items.Clear();
            VsqFileEx vsq = (VsqFileEx)AppManager.getVsqFile().Clone();

            for ( int i = 0; i < vsq.Track.size(); i++ ) {
                VsqTrack track = vsq.Track.get( i );
                int notes = 0;
                for ( Iterator itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                    object obj = itr.next();
                    notes++;
                }
                m_midi_imexport_dialog.ListTrack.Items.Add( new ListViewItem( new String[] { i.ToString(), track.getName(), notes.ToString() } ) );
                m_midi_imexport_dialog.ListTrack.Items[i].Checked = true;// i != 0;
            }
            m_midi_imexport_dialog.Mode = FormMidiImExport.FormMidiMode.Export;
            m_midi_imexport_dialog.Location = GetFormPreferedLocation( m_midi_imexport_dialog );
            if ( m_midi_imexport_dialog.ShowDialog() == DialogResult.OK ) {
                if ( !m_midi_imexport_dialog.isPreMeasure() ) {
                    vsq.removePart( 0, vsq.getPreMeasureClocks() );
                }
                int track_count = 0;
                for ( int i = 0; i < m_midi_imexport_dialog.ListTrack.Items.Count; i++ ) {
                    if ( m_midi_imexport_dialog.ListTrack.Items[i].Checked ) {
                        track_count++;
                    }
                }
                if ( track_count == 0 ) {
                    return;
                }

                DialogResult dr = DialogResult.Cancel;
                if ( AppManager.editorConfig.UseCustomFileDialog ) {
                    using ( FileDialog fd = new FileDialog( FileDialog.DialogMode.Save ) ) {
                        if ( saveMidiDialog.FileName != "" ) {
                            fd.FileName = saveMidiDialog.FileName;
                        }
                        fd.Filter = saveMidiDialog.Filter;
                        dr = fd.ShowDialog();
                        if ( dr == DialogResult.OK ) {
                            saveMidiDialog.FileName = fd.FileName;
                        }
                    }
                } else {
                    dr = saveMidiDialog.ShowDialog();
                }

                if ( dr == DialogResult.OK ) {
                    System.Text.Encoding def_enc = System.Text.Encoding.GetEncoding( 0 ); // システムのデフォルトエンコーディング
                    RandomAccessFile fs = null;
                    try {
                        fs = new RandomAccessFile( saveMidiDialog.FileName, "rw" );
                        // ヘッダー
                        fs.write( new byte[] { 0x4d, 0x54, 0x68, 0x64 }, 0, 4 );
                        //データ長
                        fs.write( (byte)0x00 );
                        fs.write( (byte)0x00 );
                        fs.write( (byte)0x00 );
                        fs.write( (byte)0x06 );
                        //フォーマット
                        fs.write( (byte)0x00 );
                        fs.write( (byte)0x01 );
                        //トラック数
                        VsqFile.writeUnsignedShort( fs, (ushort)track_count );
                        //時間単位
                        fs.write( (byte)0x01 );
                        fs.write( (byte)0xe0 );
                        int count = -1;
                        for ( int i = 0; i < m_midi_imexport_dialog.ListTrack.Items.Count; i++ ) {
                            if ( !m_midi_imexport_dialog.ListTrack.Items[i].Checked ) {
                                continue;
                            }
                            VsqTrack track = vsq.Track.get( i );
                            count++;
                            fs.write( new byte[] { 0x4d, 0x54, 0x72, 0x6b }, 0, 4 );
                            //データ長。とりあえず0を入れておく
                            fs.write( new byte[] { 0x00, 0x00, 0x00, 0x00 }, 0, 4 );
                            long first_position = fs.getFilePointer();
                            //トラック名
                            VsqFile.writeFlexibleLengthUnsignedLong( fs, 0 );//デルタタイム
                            fs.write( (byte)0xff );//ステータスタイプ
                            fs.write( (byte)0x03 );//イベントタイプSequence/Track Name
                            byte[] track_name = def_enc.GetBytes( track.getName() );
                            fs.write( (byte)track_name.Length );
                            fs.write( track_name, 0, track_name.Length );

                            Vector<MidiEvent> events = new Vector<MidiEvent>();

                            // tempo
                            boolean print_tempo = m_midi_imexport_dialog.Tempo;
                            if ( print_tempo && count == 0 ) {
                                Vector<MidiEvent> tempo_events = vsq.generateTempoChange();
                                for ( int j = 0; j < tempo_events.size(); j++ ) {
                                    events.add( tempo_events.get( j ) );
                                }
                            }

                            // timesig
                            if ( m_midi_imexport_dialog.Timesig && count == 0 ) {
                                Vector<MidiEvent> timesig_events = vsq.generateTimeSig();
                                for ( int j = 0; j < timesig_events.size(); j++ ) {
                                    events.add( timesig_events.get( j ) );
                                }
                            }

                            // Notes
                            if ( m_midi_imexport_dialog.Notes ) {
                                for ( Iterator itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ve = (VsqEvent)itr.next();
                                    int clock_on = ve.Clock;
                                    int clock_off = ve.Clock + ve.ID.Length;
                                    if ( !print_tempo ) {
                                        // テンポを出力しない場合、テンポを500000（120）と見なしてクロックを再計算
                                        double time_on = vsq.getSecFromClock( clock_on );
                                        double time_off = vsq.getSecFromClock( clock_off );
                                        clock_on = (int)(960.0 * time_on);
                                        clock_off = (int)(960.0 * time_off);
                                    }
                                    MidiEvent noteon = new MidiEvent();
                                    noteon.Clock = clock_on;
                                    noteon.FirstByte = 0x90;
                                    noteon.Data = new byte[2];
                                    noteon.Data[0] = (byte)ve.ID.Note;
                                    noteon.Data[1] = 0x40;
                                    events.add( noteon );
                                    MidiEvent noteoff = new MidiEvent();
                                    noteoff.Clock = clock_off;
                                    noteoff.FirstByte = 0x80;
                                    noteoff.Data = new byte[2];
                                    noteoff.Data[0] = (byte)ve.ID.Note;
                                    noteoff.Data[1] = 0x7f;
                                    events.add( noteoff );
                                }
                            }

                            // lyric
                            if ( m_midi_imexport_dialog.Lyric ) {
                                for ( Iterator itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ve = (VsqEvent)itr.next();
                                    int clock_on = ve.Clock;
                                    if ( !print_tempo ) {
                                        double time_on = vsq.getSecFromClock( clock_on );
                                        clock_on = (int)(960.0 * time_on);
                                    }
                                    MidiEvent add = new MidiEvent();
                                    add.Clock = clock_on;
                                    add.FirstByte = 0xff;
                                    byte[] lyric = def_enc.GetBytes( ve.ID.LyricHandle.L0.Phrase );
                                    add.Data = new byte[lyric.Length + 1];
                                    add.Data[0] = 0x05;
                                    for ( int j = 0; j < lyric.Length; j++ ) {
                                        add.Data[j + 1] = lyric[j];
                                    }
                                    events.add( add );
                                }
                            }

                            // vocaloid metatext
                            Vector<MidiEvent> meta;
                            if ( m_midi_imexport_dialog.VocaloidMetatext && i > 0 ) {
                                meta = vsq.generateMetaTextEvent( i, Encoding.GetEncoding( "Shift_JIS" ) );
                            } else {
                                meta = new Vector<MidiEvent>();
                            }

                            // vocaloid nrpn
                            Vector<MidiEvent> vocaloid_nrpn_midievent;
                            if ( m_midi_imexport_dialog.VocaloidNrpn && i > 0 ) {
                                VsqNrpn[] vsqnrpn = VsqFileEx.generateNRPN( (VsqFile)vsq, i, AppManager.editorConfig.PreSendTime );
                                NrpnData[] nrpn = VsqNrpn.convert( vsqnrpn );

                                vocaloid_nrpn_midievent = new Vector<MidiEvent>();
                                for ( int j = 0; j < nrpn.Length; j++ ) {
                                    MidiEvent me = new MidiEvent();
                                    me.Clock = nrpn[j].getClock();
                                    me.FirstByte = 0xb0;
                                    me.Data = new byte[2];
                                    me.Data[0] = nrpn[j].getParameter();
                                    me.Data[1] = nrpn[j].Value;
                                    vocaloid_nrpn_midievent.add( me );
                                }
                            } else {
                                vocaloid_nrpn_midievent = new Vector<MidiEvent>();
                            }
#if DEBUG
                            Console.WriteLine( "menuFileExportMidi_Click" );
                            Console.WriteLine( "    vocaloid_nrpn_midievent.size()=" + vocaloid_nrpn_midievent.size() );
#endif

                            // midi eventを出力
                            Collections.sort( events );
                            long last_clock = 0;
                            int events_count = events.size();
                            if ( events_count > 0 ) {
                                for ( int j = 0; j < events_count; j++ ) {
                                    if ( events.get( j ).Clock > 0 && meta.size() > 0 ) {
                                        for ( int k = 0; k < meta.size(); k++ ) {
                                            VsqFile.writeFlexibleLengthUnsignedLong( fs, 0 );
                                            meta.get( k ).writeData( fs );
                                        }
                                        meta.clear();
                                        last_clock = 0;
                                    }
                                    long clock = events.get( j ).Clock;
                                    while ( vocaloid_nrpn_midievent.size() > 0 && vocaloid_nrpn_midievent.get( 0 ).Clock < clock ) {
                                        VsqFile.writeFlexibleLengthUnsignedLong( fs, (ulong)(vocaloid_nrpn_midievent.get( 0 ).Clock - last_clock) );
                                        last_clock = vocaloid_nrpn_midievent.get( 0 ).Clock;
                                        vocaloid_nrpn_midievent.get( 0 ).writeData( fs );
                                        vocaloid_nrpn_midievent.removeElementAt( 0 );
                                    }
                                    VsqFile.writeFlexibleLengthUnsignedLong( fs, (ulong)(events.get( j ).Clock - last_clock) );
                                    events.get( j ).writeData( fs );
                                    last_clock = events.get( j ).Clock;
                                }
                            } else {
                                int c = vocaloid_nrpn_midievent.size();
                                for ( int k = 0; k < meta.size(); k++ ) {
                                    VsqFile.writeFlexibleLengthUnsignedLong( fs, 0 );
                                    meta.get( k ).writeData( fs );
                                }
                                meta.clear();
                                last_clock = 0;
                                for ( int j = 0; j < c; j++ ) {
                                    MidiEvent item = vocaloid_nrpn_midievent.get( j );
                                    long clock = item.clock;
                                    VsqFile.writeFlexibleLengthUnsignedLong( fs, (ulong)(clock - last_clock) );
                                    item.writeData( fs );
                                    last_clock = clock;
                                }
                            }

                            // トラックエンドを記入し、
                            VsqFile.writeFlexibleLengthUnsignedLong( fs, (ulong)0 );
                            fs.write( (byte)0xff );
                            fs.write( (byte)0x2f );
                            fs.write( (byte)0x00 );
                            // チャンクの先頭に戻ってチャンクのサイズを記入
                            long pos = fs.getFilePointer();
                            fs.seek( first_position - 4 );
                            VsqFile.writeUnsignedInt( fs, (uint)(pos - first_position) );
                            // ファイルを元の位置にseek
                            fs.seek( pos );
                        }
                    } catch ( Exception ex ) {
                    } finally {
                        if ( fs != null ) {
                            try {
                                fs.close();
                            } catch ( Exception ex2 ) {
                            }
                        }
                    }
                }
            }
        }

        private void menuFileOpenVsq_Click( object sender, EventArgs e ) {
            if ( !DirtyCheck() ) {
                return;
            }

            DialogResult dr = DialogResult.Cancel;
            int filterindex = 1;
            if ( AppManager.editorConfig.LastUsedExtension.Equals( ".vsq" ) ) {
                filterindex = 2;
            }
            if ( AppManager.editorConfig.UseCustomFileDialog ) {
                using ( FileDialog fd = new FileDialog( FileDialog.DialogMode.Open ) ) {
                    if ( openMidiDialog.FileName != "" ) {
                        fd.FileName = openMidiDialog.FileName;
                    }
                    fd.Filter = openMidiDialog.Filter;
                    fd.FilterIndex = filterindex;
                    dr = fd.ShowDialog();
                    if ( dr == DialogResult.OK ) {
                        openMidiDialog.FileName = fd.FileName;
                        if ( fd.FilterIndex == 1 ) {
                            AppManager.editorConfig.LastUsedExtension = ".mid";
                        } else if ( fd.FilterIndex == 2 ) {
                            AppManager.editorConfig.LastUsedExtension = ".vsq";
                        }
                    }
                }
            } else {
                openMidiDialog.FilterIndex = filterindex;
                dr = openMidiDialog.ShowDialog();
                if ( dr == DialogResult.OK ) {
#if DEBUG
                    AppManager.debugWriteLine( "openMidiDialog.FilterIndex=" + openMidiDialog.FilterIndex );
#endif
                    if ( openMidiDialog.FilterIndex == 1 ) {
                        AppManager.editorConfig.LastUsedExtension = ".mid";
                    } else if ( openMidiDialog.FilterIndex == 2 ) {
                        AppManager.editorConfig.LastUsedExtension = ".vsq";
                    }
                }
            }
            if ( dr != DialogResult.OK ) {
                return;
            }
            try {
                VsqFileEx vsq = new VsqFileEx( openMidiDialog.FileName, Encoding.GetEncoding( "Shift_JIS" ) );
                AppManager.setVsqFile( vsq );
            } catch ( Exception ex ) {
#if DEBUG
                Console.WriteLine( "FormMain#menuFileOpenVsq_Click; ex=" + ex );
#endif
                MessageBox.Show( _( "Invalid VSQ/VOCALOID MIDI file" ), _( "Error" ), MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                return;
            }
            AppManager.setSelected( 1 );
            ClearExistingData();
            Edited = false;
            AppManager.mixerWindow.updateStatus();
            ClearTempWave();
#if USE_DOBJ
            UpdateDrawObjectList();
#endif
            refreshScreen();
        }

        private void menuFileOpenUst_Click( object sender, EventArgs e ) {
            if ( !DirtyCheck() ) {
                return;
            }
            DialogResult dr = DialogResult.Cancel;
            if ( AppManager.editorConfig.UseCustomFileDialog ) {
                using ( FileDialog fd = new FileDialog( FileDialog.DialogMode.Open ) ) {
                    if ( openUstDialog.FileName != "" ) {
                        fd.FileName = openUstDialog.FileName;
                    }
                    fd.Filter = openUstDialog.Filter;
                    dr = fd.ShowDialog();
                    if ( dr == DialogResult.OK ) {
                        openUstDialog.FileName = fd.FileName;
                    }
                }
            } else {
                dr = openUstDialog.ShowDialog();
            }

            if ( dr != DialogResult.OK ) {
                return;
            }

            try {
                UstFile ust = new UstFile( openUstDialog.FileName );
                VsqFileEx vsq = new VsqFileEx( ust );
                ClearExistingData();
                AppManager.setVsqFile( vsq );
                Edited = false;
                AppManager.mixerWindow.updateStatus();
                ClearTempWave();
#if USE_DOBJ
                UpdateDrawObjectList();
#endif
                refreshScreen();
            } catch {
            }
        }

        private void menuFileImportVsq_Click( object sender, EventArgs e ) {
            DialogResult dr = DialogResult.Cancel;
            if ( AppManager.editorConfig.UseCustomFileDialog ) {
                using ( FileDialog fd = new FileDialog( FileDialog.DialogMode.Open ) ) {
                    if ( openMidiDialog.FileName != "" ) {
                        fd.FileName = openMidiDialog.FileName;
                    }
                    fd.Filter = openMidiDialog.Filter;
                    dr = fd.ShowDialog();
                    if ( dr == DialogResult.OK ) {
                        openMidiDialog.FileName = fd.FileName;
                    }
                }
            } else {
                dr = openMidiDialog.ShowDialog();
            }

            if ( dr != DialogResult.OK ) {
                return;
            }
            VsqFileEx vsq = null;
            try {
                vsq = new VsqFileEx( openMidiDialog.FileName, Encoding.GetEncoding( "Shift_JIS" ) );
            } catch ( Exception ex ) {
                MessageBox.Show( _( "Invalid VSQ/VOCALOID MIDI file" ), _( "Error" ), MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                return;
            }
            if ( m_midi_imexport_dialog == null ) {
                m_midi_imexport_dialog = new FormMidiImExport();
            }
            m_midi_imexport_dialog.ListTrack.Items.Clear();
            for ( int track = 1; track < vsq.Track.size(); track++ ) {
                m_midi_imexport_dialog.ListTrack.Items.Add( new ListViewItem( new String[] { track.ToString(), 
                                                                                             vsq.Track.get( track ).getName(),
                                                                                             vsq.Track.get( track ).getEventCount().ToString() } ) );
                m_midi_imexport_dialog.ListTrack.Items[track - 1].Checked = true;
            }
            m_midi_imexport_dialog.Mode = FormMidiImExport.FormMidiMode.ImportVsq;
            m_midi_imexport_dialog.Tempo = false;
            m_midi_imexport_dialog.Timesig = false;
            m_midi_imexport_dialog.Location = GetFormPreferedLocation( m_midi_imexport_dialog );
            if ( m_midi_imexport_dialog.ShowDialog() != DialogResult.OK ) {
                return;
            }

            Vector<Integer> add_track = new Vector<Integer>();
            for ( int i = 0; i < m_midi_imexport_dialog.ListTrack.Items.Count; i++ ) {
                if ( m_midi_imexport_dialog.ListTrack.Items[i].Checked ) {
                    add_track.add( i + 1 );
                }
            }
            if ( add_track.size() <= 0 ) {
                return;
            }

            VsqFileEx replace = (VsqFileEx)AppManager.getVsqFile().Clone();
            double premeasure_sec_replace = replace.getSecFromClock( replace.getPreMeasureClocks() );
            double premeasure_sec_vsq = vsq.getSecFromClock( vsq.getPreMeasureClocks() );

            if ( m_midi_imexport_dialog.Tempo ) {
                ShiftClockToMatchWith( replace, vsq, vsq.getSecFromClock( vsq.getPreMeasureClocks() ) );
                // テンポテーブルを置き換え
                replace.TempoTable.clear();
                for ( int i = 0; i < vsq.TempoTable.size(); i++ ) {
                    replace.TempoTable.add( (TempoTableEntry)vsq.TempoTable.get( i ).Clone() );
                }
                replace.updateTempoInfo();
                replace.updateTotalClocks();
            }

            if ( m_midi_imexport_dialog.Timesig ) {
                // 拍子をリプレースする場合
                replace.TimesigTable.clear();
                for ( int i = 0; i < vsq.TimesigTable.size(); i++ ) {
                    replace.TimesigTable.add( (TimeSigTableEntry)vsq.TimesigTable.get( i ).Clone() );
                }
                replace.updateTimesigInfo();
            }

            for ( Iterator itr = add_track.iterator(); itr.hasNext(); ){
                int track = (Integer)itr.next();
                if ( replace.Track.size() + 1 >= 16 ) {
                    break;
                }
                if ( !m_midi_imexport_dialog.Tempo ) {
                    // テンポをリプレースしない場合。インポートするトラックのクロックを調節する
                    for ( Iterator itr2 = vsq.Track.get( track ).getEventIterator(); itr2.hasNext(); ) {
                        VsqEvent item = (VsqEvent)itr2.next();
                        if ( item.ID.type == VsqIDType.Singer && item.Clock == 0 ) {
                            continue;
                        }
                        int clock = item.Clock;
                        double sec_start = vsq.getSecFromClock( clock ) - premeasure_sec_vsq + premeasure_sec_replace;
                        double sec_end = vsq.getSecFromClock( clock + item.ID.Length ) - premeasure_sec_vsq + premeasure_sec_replace;
                        int clock_start = (int)replace.getClockFromSec( sec_start );
                        int clock_end = (int)replace.getClockFromSec( sec_end );
                        item.Clock = clock_start;
                        item.ID.Length = clock_end - clock_start;
                        if ( item.ID.VibratoHandle != null ) {
                            double sec_vib_start = vsq.getSecFromClock( clock + item.ID.VibratoDelay ) - premeasure_sec_vsq + premeasure_sec_replace;
                            int clock_vib_start = (int)replace.getClockFromSec( sec_vib_start );
                            item.ID.VibratoDelay = clock_vib_start - clock_start;
                            item.ID.VibratoHandle.Length = clock_end - clock_vib_start;
                        }
                    }

                    // コントロールカーブをシフト
                    foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                        VsqBPList item = vsq.Track.get( track ).getCurve( ct.Name );
                        if ( item == null ) {
                            continue;
                        }
                        VsqBPList repl = new VsqBPList( item.getDefault(), item.getMinimum(), item.getMaximum() );
                        for ( int i = 0; i < item.size(); i++ ) {
                            int clock = item.getKeyClock( i );
                            int value = item.getElement( i );
                            double sec = vsq.getSecFromClock( clock ) - premeasure_sec_vsq + premeasure_sec_replace;
                            if ( sec >= premeasure_sec_replace ) {
                                int clock_new = (int)replace.getClockFromSec( sec );
                                repl.add( clock_new, value );
                            }
                        }
                        vsq.Track.get( track ).setCurve( ct.Name, repl );
                    }

                    // ベジエカーブをシフト
                    foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                        Vector<BezierChain> list = vsq.AttachedCurves.get( track - 1 ).get( ct );
                        if ( list == null ) {
                            continue;
                        }
                        for ( Iterator itr2 = list.iterator(); itr2.hasNext(); ){
                            BezierChain chain = (BezierChain)itr2.next();
                            for ( Iterator itr3 = chain.points.iterator(); itr3.hasNext(); ){
                                BezierPoint point = (BezierPoint)itr3.next();
                                PointD bse = new PointD( replace.getClockFromSec( vsq.getSecFromClock( point.getBase().X ) - premeasure_sec_vsq + premeasure_sec_replace ),
                                                         point.getBase().Y );
                                PointD ctrl_r = new PointD( replace.getClockFromSec( vsq.getSecFromClock( point.controlLeft.X ) - premeasure_sec_vsq + premeasure_sec_replace ),
                                                            point.controlLeft.Y );
                                PointD ctrl_l = new PointD( replace.getClockFromSec( vsq.getSecFromClock( point.controlRight.X ) - premeasure_sec_vsq + premeasure_sec_replace ),
                                                            point.controlRight.Y );
                                point.setBase( bse );
                                point.controlLeft = ctrl_l;
                                point.controlRight = ctrl_r;
                            }
                        }
                    }
                }
                replace.Track.add( vsq.Track.get( track ) );
                replace.AttachedCurves.add( vsq.AttachedCurves.get( track - 1 ) );
            }

            // コマンドを発行し、実行
            CadenciiCommand run = VsqFileEx.generateCommandReplace( replace );
            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
            Edited = true;
        }

        private void commonFileOpen_Click( object sender, EventArgs e ) {
            if ( !DirtyCheck() ) {
                return;
            }
            DialogResult dr = DialogResult.Cancel;
            if ( AppManager.editorConfig.UseCustomFileDialog ) {
                using ( FileDialog fd = new FileDialog( FileDialog.DialogMode.Open ) ) {
                    if ( openXmlVsqDialog.FileName != "" ) {
                        fd.FileName = openXmlVsqDialog.FileName;
                    }
                    fd.Filter = openXmlVsqDialog.Filter;
                    dr = fd.ShowDialog();
                    if ( dr == DialogResult.OK ) {
                        openXmlVsqDialog.FileName = fd.FileName;
                    }
                }
            } else {
                //openXmlVsqDialog
                dr = openXmlVsqDialog.ShowDialog();
            }
            if ( dr == DialogResult.OK ) {
                if ( AppManager.isPlaying() ) {
                    AppManager.setPlaying( false );
                }
                openVsqCor( openXmlVsqDialog.FileName );
                ClearExistingData();
                Edited = false;
                AppManager.mixerWindow.updateStatus();
                ClearTempWave();
#if USE_DOBJ
                UpdateDrawObjectList();
#endif
                refreshScreen();
            }
        }

        private void commonFileNew_Click( object sender, EventArgs e ) {
            if ( !DirtyCheck() ) {
                return;
            }
            AppManager.setSelected( 1 );
            AppManager.setVsqFile( new VsqFileEx( AppManager.editorConfig.DefaultSingerName, AppManager.editorConfig.DefaultPreMeasure, 4, 4, 500000 ) );
            ClearExistingData();
            Edited = false;
            AppManager.mixerWindow.updateStatus();
            ClearTempWave();
#if USE_DOBJ
            UpdateDrawObjectList();
#endif
            refreshScreen();
        }

        private void menuFileNew_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Create new project." );
        }

        private void menuFileOpen_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Open Cadencii project." );
        }

        private void menuFileSave_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Save current project." );
        }

        private void menuFileSaveNamed_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Save current project with new name." );
        }

        private void menuFileOpenVsq_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Open VSQ / VOCALOID MIDI and create new project." );
        }

        private void menuFileOpenUst_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Open UTAU project and create new project." );
        }

        private void menuFileImport_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Import." );
        }

        private void menuFileImportVsq_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Import VSQ / VOCALOID MIDI." );
        }

        private void menuFileImportMidi_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Import Standard MIDI." );
        }

        private void menuFileExportWave_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Export to WAVE file." );
        }

        private void menuFileExportMidi_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Export to Standard MIDI." );
        }

        private void menuFileRecent_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Recent projects." );
        }

        private void menuFileQuit_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Close this window." );
        }
        #endregion

        #region menuSetting*
        private void menuSettingDefaultSingerStyle_Click( object sender, EventArgs e ) {
            using ( FormSingerStyleConfig dlg = new FormSingerStyleConfig() ) {
                dlg.PMBendDepth = AppManager.editorConfig.DefaultPMBendDepth;
                dlg.PMBendLength = AppManager.editorConfig.DefaultPMBendLength;
                dlg.PMbPortamentoUse = AppManager.editorConfig.DefaultPMbPortamentoUse;
                dlg.DEMdecGainRate = AppManager.editorConfig.DefaultDEMdecGainRate;
                dlg.DEMaccent = AppManager.editorConfig.DefaultDEMaccent;

                dlg.Location = GetFormPreferedLocation( dlg );
                if ( dlg.ShowDialog() == DialogResult.OK ) {
                    if ( dlg.ApplyCurrentTrack ) {
                        VsqTrack copy = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).Clone();
                        boolean changed = false;
                        for ( int i = 0; i < copy.getEventCount(); i++ ) {
                            if ( copy.getEvent( i ).ID.type == VsqIDType.Anote ) {
                                copy.getEvent( i ).ID.PMBendDepth = dlg.PMBendDepth;
                                copy.getEvent( i ).ID.PMBendLength = dlg.PMBendLength;
                                copy.getEvent( i ).ID.PMbPortamentoUse = dlg.PMbPortamentoUse;
                                copy.getEvent( i ).ID.DEMdecGainRate = dlg.DEMdecGainRate;
                                copy.getEvent( i ).ID.DEMaccent = dlg.DEMaccent;
                                changed = true;
                            }
                        }
                        if ( changed ) {
                            CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( AppManager.getSelected(),
                                                                                         copy,
                                                                                         AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ) );
                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
#if USE_DOBJ
                            UpdateDrawObjectList();
#endif
                            refreshScreen();
                        }
                    }
                    AppManager.editorConfig.DefaultPMBendDepth = dlg.PMBendDepth;
                    AppManager.editorConfig.DefaultPMBendLength = dlg.PMBendLength;
                    AppManager.editorConfig.DefaultPMbPortamentoUse = dlg.PMbPortamentoUse;
                    AppManager.editorConfig.DefaultDEMdecGainRate = dlg.DEMdecGainRate;
                    AppManager.editorConfig.DefaultDEMaccent = dlg.DEMaccent;
                }
            }
        }

        private void menuSettingMidi_Click( object sender, EventArgs e ) {
            using ( FormMidiConfig form = new FormMidiConfig() ) {
                form.Location = GetFormPreferedLocation( form );
                if ( form.ShowDialog() == DialogResult.OK ) {

                }
            }
        }

        private void menuSettingPreference_Click( object sender, EventArgs e ) {
            if ( m_preference_dlg == null ) {
                m_preference_dlg = new Preference();
            }
            m_preference_dlg.BaseFont = new Font( AppManager.editorConfig.BaseFontName, 9 );
            m_preference_dlg.ScreenFont = new Font( AppManager.editorConfig.ScreenFontName, 9 );
            //m_preference_dlg.CounterFont = new Font( AppManager.EditorConfig.CounterFontName, 9 );
            m_preference_dlg.WheelOrder = AppManager.editorConfig.WheelOrder;
            m_preference_dlg.CursorFixed = AppManager.editorConfig.CursorFixed;
            m_preference_dlg.DefaultVibratoLength = AppManager.editorConfig.DefaultVibratoLength;
            m_preference_dlg.AutoVibratoMinimumLength = AppManager.editorConfig.AutoVibratoMinimumLength;
            m_preference_dlg.AutoVibratoType1 = AppManager.editorConfig.AutoVibratoType1;
            m_preference_dlg.AutoVibratoType2 = AppManager.editorConfig.AutoVibratoType2;
            m_preference_dlg.EnableAutoVibrato = AppManager.editorConfig.EnableAutoVibrato;
            m_preference_dlg.PreMeasure = AppManager.getVsqFile().getPreMeasure();
            m_preference_dlg.PreSendTime = AppManager.editorConfig.PreSendTime;
            m_preference_dlg.ControlCurveResolution = AppManager.editorConfig.ControlCurveResolution;
            m_preference_dlg.DefaultSingerName = AppManager.editorConfig.DefaultSingerName;
            m_preference_dlg.ScrollHorizontalOnWheel = AppManager.editorConfig.ScrollHorizontalOnWheel;
            m_preference_dlg.MaximumFrameRate = AppManager.editorConfig.MaximumFrameRate;
            m_preference_dlg.Platform = AppManager.editorConfig.Platform;
            m_preference_dlg.KeepLyricInputMode = AppManager.editorConfig.KeepLyricInputMode;
            m_preference_dlg.PxTrackHeight = AppManager.editorConfig.PxTrackHeight;
            m_preference_dlg.MouseHoverTime = AppManager.editorConfig.MouseHoverTime;
            m_preference_dlg.PlayPreviewWhenRightClick = AppManager.editorConfig.PlayPreviewWhenRightClick;
            m_preference_dlg.CurveSelectingQuantized = AppManager.editorConfig.CurveSelectingQuantized;
            m_preference_dlg.CurveVisibleAccent = AppManager.editorConfig.CurveVisibleAccent;
            m_preference_dlg.CurveVisibleBre = AppManager.editorConfig.CurveVisibleBreathiness;
            m_preference_dlg.CurveVisibleBri = AppManager.editorConfig.CurveVisibleBrightness;
            m_preference_dlg.CurveVisibleCle = AppManager.editorConfig.CurveVisibleClearness;
            m_preference_dlg.CurveVisibleDecay = AppManager.editorConfig.CurveVisibleDecay;
            m_preference_dlg.CurveVisibleDyn = AppManager.editorConfig.CurveVisibleDynamics;
            m_preference_dlg.CurveVisibleGen = AppManager.editorConfig.CurveVisibleGendorfactor;
            m_preference_dlg.CurveVisibleOpe = AppManager.editorConfig.CurveVisibleOpening;
            m_preference_dlg.CurveVisiblePit = AppManager.editorConfig.CurveVisiblePit;
            m_preference_dlg.CurveVisiblePbs = AppManager.editorConfig.CurveVisiblePbs;
            m_preference_dlg.CurveVisiblePor = AppManager.editorConfig.CurveVisiblePortamento;
            m_preference_dlg.CurveVisibleVel = AppManager.editorConfig.CurveVisibleVelocity;
            m_preference_dlg.CurveVisibleVibratoDepth = AppManager.editorConfig.CurveVisibleVibratoDepth;
            m_preference_dlg.CurveVisibleVibratoRate = AppManager.editorConfig.CurveVisibleVibratoRate;
            m_preference_dlg.CurveVisibleFx2Depth = AppManager.editorConfig.CurveVisibleFx2Depth;
            m_preference_dlg.CurveVisibleHarmonics = AppManager.editorConfig.CurveVisibleHarmonics;
            m_preference_dlg.CurveVisibleReso1 = AppManager.editorConfig.CurveVisibleReso1;
            m_preference_dlg.CurveVisibleReso2 = AppManager.editorConfig.CurveVisibleReso2;
            m_preference_dlg.CurveVisibleReso3 = AppManager.editorConfig.CurveVisibleReso3;
            m_preference_dlg.CurveVisibleReso4 = AppManager.editorConfig.CurveVisibleReso4;
            m_preference_dlg.CurveVisibleEnvelope = AppManager.editorConfig.CurveVisibleEnvelope;
            m_preference_dlg.MidiInPort = AppManager.editorConfig.MidiInPort.PortNumber;
            m_preference_dlg.InvokeWithWine = AppManager.editorConfig.InvokeUtauCoreWithWine;
            m_preference_dlg.PathResampler = AppManager.editorConfig.PathResampler;
            m_preference_dlg.PathWavtool = AppManager.editorConfig.PathWavtool;
            m_preference_dlg.UtauSingers = AppManager.editorConfig.UtauSingers;
            m_preference_dlg.UseCustomFileDialog = AppManager.editorConfig.UseCustomFileDialog;
            m_preference_dlg.SelfDeRomantization = AppManager.editorConfig.SelfDeRomanization;
            m_preference_dlg.AutoBackupIntervalMinutes = AppManager.editorConfig.AutoBackupIntervalMinutes;

            m_preference_dlg.Location = GetFormPreferedLocation( m_preference_dlg );
            
            if ( m_preference_dlg.ShowDialog() == DialogResult.OK ) {
                AppManager.editorConfig.BaseFontName = m_preference_dlg.BaseFont.Name;
                AppManager.editorConfig.BaseFontSize = m_preference_dlg.BaseFont.SizeInPoints;
                updateMenuFonts();

                AppManager.editorConfig.ScreenFontName = m_preference_dlg.ScreenFont.Name;
                //AppManager.EditorConfig.CounterFontName = m_preference_dlg.CounterFont.Name;
                AppManager.editorConfig.WheelOrder = m_preference_dlg.WheelOrder;
                AppManager.editorConfig.CursorFixed = m_preference_dlg.CursorFixed;

                AppManager.editorConfig.DefaultVibratoLength = m_preference_dlg.DefaultVibratoLength;
                AppManager.editorConfig.AutoVibratoMinimumLength = m_preference_dlg.AutoVibratoMinimumLength;
                AppManager.editorConfig.AutoVibratoType1 = m_preference_dlg.AutoVibratoType1;
                AppManager.editorConfig.AutoVibratoType2 = m_preference_dlg.AutoVibratoType2;

                AppManager.editorConfig.EnableAutoVibrato = m_preference_dlg.EnableAutoVibrato;
                AppManager.editorConfig.PreSendTime = m_preference_dlg.PreSendTime;
                AppManager.editorConfig.DefaultPreMeasure = m_preference_dlg.PreMeasure;
                if ( m_preference_dlg.PreMeasure != AppManager.getVsqFile().getPreMeasure() ) {
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandChangePreMeasure( m_preference_dlg.PreMeasure ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    Edited = true;
                }
                AppManager.editorConfig.Language = m_preference_dlg.Language;
                if ( Messaging.Language != AppManager.editorConfig.Language ) {
                    Messaging.Language = AppManager.editorConfig.Language;
                    applyLanguage();
                    m_preference_dlg.ApplyLanguage();
                    AppManager.mixerWindow.ApplyLanguage();
                    if ( m_versioninfo != null && !m_versioninfo.IsDisposed ) {
                        m_versioninfo.ApplyLanguage();
                    }
                    AppManager.propertyWindow.ApplyLanguage();
                    AppManager.propertyPanel.UpdateValue( AppManager.getSelected() );
                }

                AppManager.editorConfig.ControlCurveResolution = m_preference_dlg.ControlCurveResolution;
                AppManager.editorConfig.DefaultSingerName = m_preference_dlg.DefaultSingerName;
                AppManager.editorConfig.ScrollHorizontalOnWheel = m_preference_dlg.ScrollHorizontalOnWheel;
                AppManager.editorConfig.MaximumFrameRate = m_preference_dlg.MaximumFrameRate;
                int fps = 1000 / AppManager.editorConfig.MaximumFrameRate;
                timer.Interval = (fps <= 0) ? 1 : fps;
                AppManager.editorConfig.Platform = m_preference_dlg.Platform;
                s_modifier_key = ((AppManager.editorConfig.Platform == Platform.Macintosh) ? Keys.Menu : Keys.Control);
                ApplyShortcut();
                AppManager.editorConfig.KeepLyricInputMode = m_preference_dlg.KeepLyricInputMode;
                if ( AppManager.editorConfig.PxTrackHeight != m_preference_dlg.PxTrackHeight ) {
                    AppManager.editorConfig.PxTrackHeight = m_preference_dlg.PxTrackHeight;
#if USE_DOBJ
                    UpdateDrawObjectList();
#endif
                }
                AppManager.editorConfig.MouseHoverTime = m_preference_dlg.MouseHoverTime;
                AppManager.editorConfig.PlayPreviewWhenRightClick = m_preference_dlg.PlayPreviewWhenRightClick;
                AppManager.editorConfig.CurveSelectingQuantized = m_preference_dlg.CurveSelectingQuantized;

                AppManager.editorConfig.CurveVisibleAccent = m_preference_dlg.CurveVisibleAccent;
                AppManager.editorConfig.CurveVisibleBreathiness = m_preference_dlg.CurveVisibleBre;
                AppManager.editorConfig.CurveVisibleBrightness = m_preference_dlg.CurveVisibleBri;
                AppManager.editorConfig.CurveVisibleClearness = m_preference_dlg.CurveVisibleCle;
                AppManager.editorConfig.CurveVisibleDecay = m_preference_dlg.CurveVisibleDecay;
                AppManager.editorConfig.CurveVisibleDynamics = m_preference_dlg.CurveVisibleDyn;
                AppManager.editorConfig.CurveVisibleGendorfactor = m_preference_dlg.CurveVisibleGen;
                AppManager.editorConfig.CurveVisibleOpening = m_preference_dlg.CurveVisibleOpe;
                AppManager.editorConfig.CurveVisiblePit = m_preference_dlg.CurveVisiblePit;
                AppManager.editorConfig.CurveVisiblePbs = m_preference_dlg.CurveVisiblePbs;
                AppManager.editorConfig.CurveVisiblePortamento = m_preference_dlg.CurveVisiblePor;
                AppManager.editorConfig.CurveVisibleVelocity = m_preference_dlg.CurveVisibleVel;
                AppManager.editorConfig.CurveVisibleVibratoDepth = m_preference_dlg.CurveVisibleVibratoDepth;
                AppManager.editorConfig.CurveVisibleVibratoRate = m_preference_dlg.CurveVisibleVibratoRate;
                AppManager.editorConfig.CurveVisibleFx2Depth = m_preference_dlg.CurveVisibleFx2Depth;
                AppManager.editorConfig.CurveVisibleHarmonics = m_preference_dlg.CurveVisibleHarmonics;
                AppManager.editorConfig.CurveVisibleReso1 = m_preference_dlg.CurveVisibleReso1;
                AppManager.editorConfig.CurveVisibleReso2 = m_preference_dlg.CurveVisibleReso2;
                AppManager.editorConfig.CurveVisibleReso3 = m_preference_dlg.CurveVisibleReso3;
                AppManager.editorConfig.CurveVisibleReso4 = m_preference_dlg.CurveVisibleReso4;
                AppManager.editorConfig.CurveVisibleEnvelope = m_preference_dlg.CurveVisibleEnvelope;

                AppManager.editorConfig.MidiInPort.PortNumber = m_preference_dlg.MidiInPort;
                UpdateMidiInStatus();
                ReloadMidiIn();

                AppManager.editorConfig.InvokeUtauCoreWithWine = m_preference_dlg.InvokeWithWine;
                AppManager.editorConfig.PathResampler = m_preference_dlg.PathResampler;
                AppManager.editorConfig.PathWavtool = m_preference_dlg.PathWavtool;
                AppManager.editorConfig.UtauSingers.clear();
                for( Iterator itr = m_preference_dlg.UtauSingers.iterator(); itr.hasNext(); ){
                    SingerConfig sc = (SingerConfig)itr.next();
                    AppManager.editorConfig.UtauSingers.add( (SingerConfig)sc.Clone() );
                }
                AppManager.editorConfig.UseCustomFileDialog = m_preference_dlg.UseCustomFileDialog;
                AppManager.editorConfig.SelfDeRomanization = m_preference_dlg.SelfDeRomantization;
                AppManager.editorConfig.AutoBackupIntervalMinutes = m_preference_dlg.AutoBackupIntervalMinutes;

                Vector<CurveType> visible_curves = new Vector<CurveType>();
                trackSelector.clearViewingCurve();

                UpdateTrackSelectorVisibleCurve();
                updateRendererMenu();
                AppManager.updateAutoBackupTimerStatus();

                AppManager.saveConfig();
                applyLanguage();
                UpdateTrackSelectorVisibleCurve();
                UpdateScriptShortcut();
#if USE_DOBJ
                UpdateDrawObjectList();
#endif
                refreshScreen();
            }
        }

        private void menuSettingShortcut_Click( object sender, EventArgs e ) {
            TreeMap<String, ValuePair<String, Keys[]>> dict = new TreeMap<String, ValuePair<String, Keys[]>>();
            TreeMap<String, Keys[]> configured = AppManager.editorConfig.GetShortcutKeysDictionary();

            // スクリプトのToolStripMenuITemを蒐集
            Vector<String> script_shortcut = new Vector<String>();
            foreach ( ToolStripItem tsi in menuScript.DropDownItems ) {
                if ( tsi is ToolStripMenuItem ) {
                    ToolStripMenuItem tsmi = (ToolStripMenuItem)tsi;
                    if ( tsmi.DropDownItems.Count == 1 && tsmi.DropDownItems[0] is ToolStripMenuItem ) {
                        ToolStripMenuItem item = (ToolStripMenuItem)tsmi.DropDownItems[0];
                        script_shortcut.add( item.Name );
                        if ( !configured.containsKey( item.Name ) ) {
                            configured.put( item.Name, new Keys[] { } );
                        }
                    }
                }
            }

            for ( Iterator itr = configured.keySet().iterator(); itr.hasNext(); ){
                String name = (String)itr.next();
                ToolStripMenuItem menu = SearchMenuItemFromName( name );
                if ( menu != null ) {
                    String parent = "";
                    if ( menu.OwnerItem != null && menu.OwnerItem.Name != menuHidden.Name ) {
                        String s = menu.OwnerItem.Text;
                        int i = s.IndexOf( "(&" );
                        if ( i > 0 ) {
                            s = s.Substring( 0, i );
                        }
                        parent = s + " -> ";
                    }
                    String s1 = menu.Text;
                    int i1 = s1.IndexOf( "(&" );
                    if ( i1 > 0 ) {
                        s1 = s1.Substring( 0, i1 );
                    }
                    if ( script_shortcut.contains( name ) ) {
                        String s2 = menuScript.Text;
                        int i2 = s2.IndexOf( "(&" );
                        if ( i2 > 0 ) {
                            s2 = s2.Substring( 0, i2 );
                        }
                        parent = s2 + " -> " + parent;
                    }
                    dict.put( parent + s1, new ValuePair<String, Keys[]>( name, configured.get( name ) ) );
                }
            }

            using ( FormShortcutKeys form = new FormShortcutKeys( dict ) ) {
                form.Location = GetFormPreferedLocation( form );
                if ( form.ShowDialog() == DialogResult.OK ) {
                    TreeMap<String, ValuePair<String, Keys[]>> res = form.Result;
                    for ( Iterator itr = res.keySet().iterator(); itr.hasNext(); ){
                        String display = (String)itr.next();
                        String name = res.get( display ).Key;
                        Keys[] keys = res.get( display ).Value;
                        boolean found = false;
                        for ( int i = 0; i < AppManager.editorConfig.ShortcutKeys.size(); i++ ) {
                            if ( AppManager.editorConfig.ShortcutKeys.get( i ).Key.Equals( name ) ) {
                                AppManager.editorConfig.ShortcutKeys.get( i ).Value = keys;
                                found = true;
                                break;
                            }
                        }
                        if ( !found ) {
                            AppManager.editorConfig.ShortcutKeys.add( new ValuePairOfStringArrayOfKeys( name, keys ) );
                        }
                    }
                    ApplyShortcut();
                    AppManager.propertyWindow.FormCloseShortcutKey = AppManager.editorConfig.GetShortcutKeyFor( menuVisualProperty );
                }
            }
        }

        private void menuSettingGameControlerLoad_Click( object sender, EventArgs e ) {
            GameControlerLoad();
        }

        private void menuSettingGameControlerRemove_Click( object sender, EventArgs e ) {
            GameControlerRemove();
        }

        private void menuSettingGameControlerSetting_Click( object sender, EventArgs e ) {
            using ( FormGameControlerConfig dlg = new FormGameControlerConfig() ) {
                dlg.Location = GetFormPreferedLocation( dlg );
                if ( dlg.ShowDialog() == DialogResult.OK ) {
                    AppManager.editorConfig.GameControlerRectangle = dlg.Rectangle;
                    AppManager.editorConfig.GameControlerTriangle = dlg.Triangle;
                    AppManager.editorConfig.GameControlerCircle = dlg.Circle;
                    AppManager.editorConfig.GameControlerCross = dlg.Cross;
                    AppManager.editorConfig.GameControlL1 = dlg.L1;
                    AppManager.editorConfig.GameControlL2 = dlg.L2;
                    AppManager.editorConfig.GameControlR1 = dlg.R1;
                    AppManager.editorConfig.GameControlR2 = dlg.R2;
                    AppManager.editorConfig.GameControlSelect = dlg.Select;
                    AppManager.editorConfig.GameControlStart = dlg.Start;
                    AppManager.editorConfig.GameControlPovDown = dlg.PovDown;
                    AppManager.editorConfig.GameControlPovLeft = dlg.PovLeft;
                    AppManager.editorConfig.GameControlPovUp = dlg.PovUp;
                    AppManager.editorConfig.GameControlPovRight = dlg.PovRight;
                }
            }
        }
        #endregion

        #region menuEdit*
        private void menuEditDelete_Click( object sender, EventArgs e ) {
            DeleteEvent();
        }

        private void commonEditPaste_Click( object sender, EventArgs e ) {
            pasteEvent();
        }

        private void commonEditCopy_Click( object sender, EventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "commonEditCopy_Click" );
#endif
            copyEvent();
        }

        private void commonEditCut_Click( object sender, EventArgs e ) {
            CutEvent();
        }

        private void menuEdit_DropDownOpening( object sender, EventArgs e ) {
            updateCopyAndPasteButtonStatus();
        }

        private void commonEditUndo_Click( object sender, EventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "menuEditUndo_Click" );
#endif
            undo();
            refreshScreen();
        }


        private void commonEditRedo_Click( object sender, EventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "menuEditRedo_Click" );
#endif
            redo();
            refreshScreen();
        }

        private void menuEditSelectAllEvents_Click( object sender, EventArgs e ) {
            selectAllEvent();
        }

        private void menuEditSelectAll_Click( object sender, EventArgs e ) {
            selectAll();
        }

        private void menuEditAutoNormalizeMode_Click( object sender, EventArgs e ) {
            AppManager.autoNormalize = !AppManager.autoNormalize;
            menuEditAutoNormalizeMode.Checked = AppManager.autoNormalize;
        }

        private void menuEditUndo_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Undo." );
        }

        private void menuEditRedo_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Redo." );
        }

        private void menuEditCut_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Cut selected items." );
        }

        private void menuEditCopy_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Copy selected items." );
        }

        private void menuEditPaste_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Paste copied items to current song position." );
        }

        private void menuEditDelete_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Delete selected items." );
        }

        private void menuEditAutoNormalizeMode_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Avoid automaticaly polyphonic editing." );
        }

        private void menuEditSelectAll_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Select all items and control curves of current track." );
        }

        private void menuEditSelectAllEvents_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Select all items of current track." );
        }
        #endregion

        #region menuLyric*
        private void menuLyric_DropDownOpening( object sender, EventArgs e ) {
            menuLyricExpressionProperty.Enabled = (AppManager.getLastSelectedEvent() != null);
        }

        private void menuLyricExpressionProperty_Click( object sender, EventArgs e ) {
            NoteExpressionProperty();
        }

        private void menuLyricDictionary_Click( object sender, EventArgs e ) {
            using ( FormWordDictionary dlg = new FormWordDictionary() ) {
                dlg.Location = GetFormPreferedLocation( dlg );
                if ( dlg.ShowDialog() == DialogResult.OK ) {
                    Vector<KeyValuePair<String, boolean>> result = dlg.Result;
                    SymbolTable.changeOrder( result.toArray( new KeyValuePair<String, boolean>[]{} ) );
                }
            }
        }

        private void menuLyricVibratoProperty_Click( object sender, EventArgs e ) {
            NoteVibratoProperty();
        }
        #endregion

        #region menuJob
        private void menuJobRealTime_Click( object sender, EventArgs e ) {
            if ( !AppManager.isPlaying() ) {
                m_adding = null;
                AppManager.setEditMode( EditMode.REALTIME );
                AppManager.setPlaying( true );
                menuJobRealTime.Text = _( "Stop Realtime Input" );
            } else {
                timer.Stop();
                AppManager.setPlaying( false );
                AppManager.setEditMode( EditMode.NONE );
                menuJobRealTime.Text = _( "Start Realtime Input" );
            }
        }

        private void menuJobReloadVsti_Click( object sender, EventArgs e ) {
            //VSTiProxy.ReloadPlugin(); //todo: FormMain+menuJobReloadVsti_Click
        }
        
        private void menuJob_DropDownOpening( object sender, EventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "menuJob_DropDownOpening" );
            AppManager.debugWriteLine( "    menuJob.Bounds=" + menuJob.Bounds );
#endif
            if ( AppManager.getSelectedEventCount() <= 1 ) {
                menuJobConnect.Enabled = false;
            } else {
                // menuJobConnect(音符の結合)がEnableされるかどうかは、選択されている音符がピアノロール上で連続かどうかで決まる
                int[] list = new int[AppManager.getSelectedEventCount()];
                for ( int i = 0; i < AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventCount(); i++ ) {
                    int count = -1;
                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                        SelectedEventEntry item = (SelectedEventEntry)itr.next();
                        int key = item.original.InternalID;
                        count++;
                        if ( key == AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEvent( i ).InternalID ) {
                            list[count] = i;
                            break;
                        }
                    }
                }
                boolean changed = true;
                while ( changed ) {
                    changed = false;
                    for ( int i = 0; i < list.Length - 1; i++ ) {
                        if ( list[i] > list[i + 1] ) {
                            int b = list[i];
                            list[i] = list[i + 1];
                            list[i + 1] = b;
                            changed = true;
                        }
                    }
                }
                boolean continued = true;
                for ( int i = 0; i < list.Length - 1; i++ ) {
                    if ( list[i] + 1 != list[i + 1] ) {
                        continued = false;
                        break;
                    }
                }
                menuJobConnect.Enabled = continued;
            }

            menuJobLyric.Enabled = AppManager.getLastSelectedEvent() != null;
        }

        private void menuJobLyric_Click( object sender, EventArgs e ) {
            ImportLyric();
        }

        private void menuJobConnect_Click( object sender, EventArgs e ) {
            int count = AppManager.getSelectedEventCount();
            int[] clocks = new int[count];
            VsqID[] ids = new VsqID[count];
            int[] internalids = new int[count];
            int i = -1;
            for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                SelectedEventEntry item = (SelectedEventEntry)itr.next();
                i++;
                clocks[i] = item.original.Clock;
                ids[i] = (VsqID)item.original.ID.clone();
                internalids[i] = item.original.InternalID;
            }
            boolean changed = true;
            while ( changed ) {
                changed = false;
                for ( int j = 0; j < clocks.Length - 1; j++ ) {
                    if ( clocks[j] > clocks[j + 1] ) {
                        int b = clocks[j];
                        clocks[j] = clocks[j + 1];
                        clocks[j + 1] = b;
                        VsqID a = ids[j];
                        ids[j] = ids[j + 1];
                        ids[j + 1] = a;
                        changed = true;
                        b = internalids[j];
                        internalids[j] = internalids[j + 1];
                        internalids[j + 1] = b;
                    }
                }
            }

            for ( int j = 0; j < ids.Length - 1; j++ ) {
                ids[j].Length = clocks[j + 1] - clocks[j];
            }
            CadenciiCommand run = new CadenciiCommand( 
                VsqCommand.generateCommandEventChangeIDContaintsRange( AppManager.getSelected(), internalids, ids ) );
            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
            Edited = true;
            this.Refresh();
        }

        private void menuJobInsertBar_Click( object sender, EventArgs e ) {
            int total_clock = AppManager.getVsqFile().TotalClocks;
            int total_barcount = AppManager.getVsqFile().getBarCountFromClock( total_clock ) + 1;
            using ( FormInsertBar dlg = new FormInsertBar( total_barcount ) ) {
                int current_clock = AppManager.getCurrentClock();
                int barcount = AppManager.getVsqFile().getBarCountFromClock( current_clock );
                int draft = barcount - AppManager.getVsqFile().getPreMeasure() + 1;
                if ( draft <= 0 ) {
                    draft = 1;
                }
                dlg.Position = draft;

                dlg.Location = GetFormPreferedLocation( dlg );
                if ( dlg.ShowDialog() == DialogResult.OK ) {
                    int pos = dlg.Position + AppManager.getVsqFile().getPreMeasure() - 1;
                    int length = dlg.Length;

                    int clock_start = AppManager.getVsqFile().getClockFromBarCount( pos );
                    int clock_end = AppManager.getVsqFile().getClockFromBarCount( pos + length );
                    int dclock = clock_end - clock_start;
                    VsqFileEx temp = (VsqFileEx)AppManager.getVsqFile().Clone();

                    for ( int track = 1; track < temp.Track.size(); track++ ) {
                        BezierCurves newbc = new BezierCurves();
                        foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                            int index = ct.Index;
                            if ( index < 0 ) {
                                continue;
                            }

                            Vector<BezierChain> list = new Vector<BezierChain>();
                            for ( Iterator itr = temp.AttachedCurves.get( track - 1 ).get( ct ).iterator(); itr.hasNext(); ){
                                BezierChain bc = (BezierChain)itr.next();
                                if ( bc.size() < 2 ) {
                                    continue;
                                }
                                int chain_start = (int)bc.points.get( 0 ).getBase().X;
                                int chain_end = (int)bc.points.get( bc.points.size() - 1 ).getBase().X;

                                if ( clock_start <= chain_start ) {
                                    for ( int i = 0; i < bc.points.size(); i++ ) {
                                        PointD t = bc.points.get( i ).getBase();
                                        bc.points.get( i ).setBase( new PointD( t.X + dclock, t.Y ) );
                                    }
                                    list.add( bc );
                                } else if ( chain_start < clock_start && clock_start < chain_end ) {
                                    BezierChain adding1 = bc.extractPartialBezier( chain_start, clock_start );
                                    BezierChain adding2 = bc.extractPartialBezier( clock_start, chain_end );
                                    for ( int i = 0; i < adding2.points.size(); i++ ) {
                                        PointD t = adding2.points.get( i ).getBase();
                                        adding2.points.get( i ).setBase( new PointD( t.X + dclock, t.Y ) );
                                    }
                                    //PointD t2 = adding1.points[adding1.points.Count - 1].Base;
                                    adding1.points.get( adding1.points.size() - 1 ).setControlRightType( BezierControlType.None );
                                    /*BezierPoint bp = new BezierPoint( t2.X + dclock, t2.Y );
                                    bp.ControlLeftType = BezierControlType.None;
                                    bp.ControlRightType = BezierControlType.None;
                                    adding1.points.Add( bp );*/
                                    adding2.points.get( 0 ).setControlLeftType( BezierControlType.None );
                                    for ( int i = 0; i < adding2.points.size(); i++ ) {
                                        adding1.points.add( adding2.points.get( i ) );
                                    }
                                    adding1.id = bc.id;
                                    list.add( adding1 );
                                } else {
                                    list.add( (BezierChain)bc.Clone() );
                                }
                            }

                            newbc.set( ct, list );
                        }
                        temp.AttachedCurves.set( track - 1, newbc );
                    }

                    for ( int track = 1; track < AppManager.getVsqFile().Track.size(); track++ ) {
                        for ( int i = 0; i < temp.Track.get( track ).getEventCount(); i++ ) {
                            if ( temp.Track.get( track ).getEvent( i ).Clock >= clock_start ) {
                                temp.Track.get( track ).getEvent( i ).Clock += dclock;
                            }
                        }
                        foreach ( CurveType curve in AppManager.CURVE_USAGE ) {
                            if ( curve.IsScalar || curve.IsAttachNote ) {
                                continue;
                            }
                            VsqBPList target = temp.Track.get( track ).getCurve( curve.Name );
                            VsqBPList src = AppManager.getVsqFile().Track.get( track ).getCurve( curve.Name );
                            target.clear();
                            for ( Iterator itr = src.keyClockIterator(); itr.hasNext(); ) {
                                int key = (int)itr.next();
                                if ( key >= clock_start ) {
                                    target.add( key + dclock, src.getValue( key ) );
                                } else {
                                    target.add( key, src.getValue( key ) );
                                }
                            }
                        }
                    }
                    for ( int i = 0; i < temp.TempoTable.size(); i++ ) {
                        if ( temp.TempoTable.get( i ).Clock >= clock_start ) {
                            temp.TempoTable.get( i ).Clock = temp.TempoTable.get( i ).Clock + dclock;
                        }
                    }
                    for ( int i = 0; i < temp.TimesigTable.size(); i++ ) {
                        if ( temp.TimesigTable.get( i ).Clock >= clock_start ) {
                            temp.TimesigTable.get( i ).Clock = temp.TimesigTable.get( i ).Clock + dclock;
                        }
                    }
                    temp.updateTempoInfo();
                    temp.updateTimesigInfo();

                    CadenciiCommand run = VsqFileEx.generateCommandReplace( temp );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    Edited = true;
                    this.Refresh();
                }
            }
        }

        private void menuJobDeleteBar_Click( object sender, EventArgs e ) {
            int total_clock = AppManager.getVsqFile().TotalClocks;
            int total_barcount = AppManager.getVsqFile().getBarCountFromClock( total_clock ) + 1;
            int clock = AppManager.getCurrentClock();
            int barcount = AppManager.getVsqFile().getBarCountFromClock( clock );
            using ( FormDeleteBar dlg = new FormDeleteBar( total_barcount ) ) {
                int draft = barcount - AppManager.getVsqFile().getPreMeasure() + 1;
                if ( draft <= 0 ) {
                    draft = 1;
                }
                dlg.Start = draft;
                dlg.End = draft + 1;

                dlg.Location = GetFormPreferedLocation( dlg );
                if ( dlg.ShowDialog() == DialogResult.OK ) {
                    VsqFileEx temp = (VsqFileEx)AppManager.getVsqFile().Clone();
                    int start = dlg.Start + AppManager.getVsqFile().getPreMeasure() - 1;
                    int end = dlg.End + AppManager.getVsqFile().getPreMeasure() - 1;
#if DEBUG
                    AppManager.debugWriteLine( "FormMain+menuJobDeleteBar_Click" );
                    AppManager.debugWriteLine( "    start,end=" + start + "," + end );
#endif
                    int clock_start = temp.getClockFromBarCount( start );
                    int clock_end = temp.getClockFromBarCount( end );
                    int dclock = clock_end - clock_start;
                    for ( int track = 1; track < temp.Track.size(); track++ ) {
                        BezierCurves newbc = new BezierCurves();
                        foreach ( CurveType ct in AppManager.CURVE_USAGE ) {
                            int index = ct.Index;
                            if ( index < 0 ) {
                                continue;
                            }

                            Vector<BezierChain> list = new Vector<BezierChain>();
                            for ( Iterator itr = temp.AttachedCurves.get( track - 1 ).get( ct ).iterator(); itr.hasNext(); ){
                                BezierChain bc = (BezierChain)itr.next();
                                if ( bc.size() < 2 ) {
                                    continue;
                                }
                                int chain_start = (int)bc.points.get( 0 ).getBase().X;
                                int chain_end = (int)bc.points.get( bc.points.size() - 1 ).getBase().X;

                                if ( clock_start < chain_start && chain_start < clock_end && clock_end < chain_end ) {
                                    BezierChain adding = bc.extractPartialBezier( clock_end, chain_end );
                                    adding.id = bc.id;
                                    for ( int i = 0; i < adding.points.size(); i++ ) {
                                        PointD t = adding.points.get( i ).getBase();
                                        adding.points.get( i ).setBase( new PointD( t.X - dclock, t.Y ) );
                                    }
                                    list.add( adding );
                                } else if ( chain_start < clock_start && clock_end < chain_end ) {
                                    BezierChain adding1 = bc.extractPartialBezier( chain_start, clock_start );
                                    adding1.id = bc.id;
                                    adding1.points.get( adding1.points.size() - 1 ).setControlRightType( BezierControlType.None );
                                    BezierChain adding2 = bc.extractPartialBezier( clock_end, chain_end );
                                    adding2.points.get( 0 ).setControlLeftType( BezierControlType.None );
                                    PointD t = adding2.points.get( 0 ).getBase();
                                    adding2.points.get( 0 ).setBase( new PointD( t.X - dclock, t.Y ) );
                                    adding1.points.add( adding2.points.get( 0 ) );
                                    for ( int i = 1; i < adding2.points.size(); i++ ) {
                                        t = adding2.points.get( i ).getBase();
                                        adding2.points.get( i ).setBase( new PointD( t.X - dclock, t.Y ) );
                                        adding1.points.add( adding2.points.get( i ) );
                                    }
                                    list.add( adding1 );
                                } else if ( chain_start < clock_start && clock_start < chain_end && chain_end < clock_end ) {
                                    BezierChain adding = bc.extractPartialBezier( chain_start, clock_start );
                                    adding.id = bc.id;
                                    list.add( adding );
                                } else if ( clock_end <= chain_start || chain_end <= clock_start ) {
                                    if ( clock_end <= chain_start ) {
                                        for ( int i = 0; i < bc.points.size(); i++ ) {
                                            PointD t = bc.points.get( i ).getBase();
                                            bc.points.get( i ).setBase( new PointD( t.X - dclock, t.Y ) );
                                        }
                                    }
                                    list.add( (BezierChain)bc.Clone() );
                                }
                            }

                            newbc.set( ct, list );
                        }
                        temp.AttachedCurves.set( track - 1, newbc );
                    }

                    temp.removePart( clock_start, clock_end );
                    CadenciiCommand run = VsqFileEx.generateCommandReplace( temp );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    Edited = true;
                    this.Refresh();
                }
            }
        }

        private void menuJobNormalize_Click( object sender, EventArgs e ) {
            VsqFile work = (VsqFile)AppManager.getVsqFile().Clone();
            int track = AppManager.getSelected();
            boolean changed = true;
            boolean total_changed = false;

            // 最初、開始時刻が同じになっている奴を削除
            while ( changed ) {
                changed = false;
                for ( int i = 0; i < work.Track.get( track ).getEventCount() - 1; i++ ) {
                    int clock = work.Track.get( track ).getEvent( i ).Clock;
                    int id = work.Track.get( track ).getEvent( i ).InternalID;
                    for ( int j = i + 1; j < work.Track.get( track ).getEventCount(); j++ ) {
                        if ( clock == work.Track.get( track ).getEvent( j ).Clock ) {
                            if ( id < work.Track.get( track ).getEvent( j ).InternalID ) { //内部IDが小さい＝より高年齢（音符追加時刻が古い）
                                work.Track.get( track ).removeEvent( i );
                            } else {
                                work.Track.get( track ).removeEvent( j );
                            }
                            changed = true;
                            total_changed = true;
                            break;
                        }
                    }
                    if ( changed ) {
                        break;
                    }
                }
            }

            changed = true;
            while ( changed ) {
                changed = false;
                for ( int i = 0; i < work.Track.get( track ).getEventCount() - 1; i++ ) {
                    int start_clock = work.Track.get( track ).getEvent( i ).Clock;
                    int end_clock = work.Track.get( track ).getEvent( i ).ID.Length + start_clock;
                    for ( int j = i + 1; j < work.Track.get( track ).getEventCount(); j++ ) {
                        int this_start_clock = work.Track.get( track ).getEvent( j ).Clock;
                        if ( this_start_clock < end_clock ) {
                            work.Track.get( track ).getEvent( i ).ID.Length = this_start_clock - start_clock;
                            changed = true;
                            total_changed = true;
                        }
                    }
                }
            }

            if ( total_changed ) {
                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandReplace( work ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                Edited = true;
                refreshScreen();
            }
        }

        private void menuJobNormalize_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Correct overlapped item." );
        }

        private void menuJobInsertBar_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Insert bar." );
        }

        private void menuJobDeleteBar_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Delete bar." );
        }

        private void menuJobRandomize_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Randomize items." ) + _( "(not implemented)" );
        }

        private void menuJobConnect_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Lengthen note end to neighboring note." );
        }

        private void menuJobLyric_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Import lyric." );
        }

        private void menuJobRewire_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Import tempo from ReWire host." ) + _( "(not implemented)" );
        }

        private void menuJobRealTime_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Start realtime input." );
        }

        private void menuJobReloadVsti_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Reload VSTi dll." ) + _( "(not implemented)" );
        }
        #endregion

        #region menuScript
        private void menuScriptUpdate_Click( object sender, EventArgs e ) {
            UpdateScriptShortcut();
            ApplyShortcut();
        }
        #endregion

        #region vScroll
        private void vScroll_Enter( object sender, EventArgs e ) {
            pictPianoRoll.Select();
        }

        private void vScroll_Resize( object sender, EventArgs e ) {
            SetVScrollRange( vScroll.Maximum );
        }

        private void vScroll_ValueChanged( object sender, EventArgs e ) {
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
            refreshScreen();
        }
        #endregion

        #region hScroll
        private void hScroll_Enter( object sender, EventArgs e ) {
            pictPianoRoll.Select();
        }

        private void hScroll_Resize( object sender, EventArgs e ) {
            SetHScrollRange( hScroll.Maximum );
        }

        private void hScroll_ValueChanged( object sender, EventArgs e ) {
#if DEBUG
            //Console.WriteLine( "hScroll_ValueChanged" );
            //Console.WriteLine( "    Value/Maximum=" + hScroll.Value + "/" + hScroll.Maximum );
            //Console.WriteLine( "    LargeChange=" + hScroll.LargeChange );
#endif
            AppManager.startToDrawX = (int)(hScroll.Value * AppManager.scaleX);
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
            refreshScreen();
        }
        #endregion

        #region picturePositionIndicator
        private void picturePositionIndicator_MouseWheel( object sender, MouseEventArgs e ) {
            hScroll.Value = NewHScrollValueFromWheelDelta( e.Delta );
        }

        private void picturePositionIndicator_MouseDoubleClick( object sender, MouseEventArgs e ) {
            if ( e.X < AppManager.KEY_LENGTH || Width - 3 < e.X ) {
                return;
            }
            if ( e.Button == MouseButtons.Left ) {
                if ( 18 < e.Y && e.Y <= 32 ) {
                    #region テンポの変更
                    int index = -1;
                    for ( int i = 0; i < AppManager.getVsqFile().TempoTable.size(); i++ ) {
                        int clock = AppManager.getVsqFile().TempoTable.get( i ).Clock;
                        int x = xCoordFromClocks( clock );
                        if ( x < 0 ) {
                            continue;
                        } else if ( Width < x ) {
                            break;
                        }
                        String s = (60e6 / (float)AppManager.getVsqFile().TempoTable.get( i ).Tempo).ToString( "#.00" );
                        SizeF size = AppManager.measureString( s, new Font( AppManager.editorConfig.ScreenFontName, 8 ) );
                        if ( IsInRect( e.Location, new Rectangle( x, 14, (int)size.Width, 14 ) ) ) {
                            index = i;
                            break;
                        }
                    }

                    if ( index < 0 ) {
                        return;
                    }

                    TempoTableEntry tte = AppManager.getVsqFile().TempoTable.get( index );
                    AppManager.clearSelectedTempo();
                    AppManager.addSelectedTempo( tte.Clock );
                    int bar_count = AppManager.getVsqFile().getBarCountFromClock( tte.Clock );
                    int bar_top_clock = AppManager.getVsqFile().getClockFromBarCount( bar_count );
                    int local_denominator, local_numerator;
                    AppManager.getVsqFile().getTimesigAt( tte.Clock, out local_numerator, out local_denominator );
                    int clock_per_beat = 480 * 4 / local_denominator;
                    int clocks_in_bar = tte.Clock - bar_top_clock;
                    int beat_in_bar = clocks_in_bar / clock_per_beat + 1;
                    int clocks_in_beat = clocks_in_bar - (beat_in_bar - 1) * clock_per_beat;
                    using ( FormTempoConfig dlg = new FormTempoConfig( bar_count, beat_in_bar, local_numerator, clocks_in_beat, clock_per_beat, (decimal)(6e7 / tte.Tempo), AppManager.getVsqFile().getPreMeasure() ) ) {
                        dlg.Location = GetFormPreferedLocation( dlg );
                        if ( dlg.ShowDialog() == DialogResult.OK ) {
                            int new_beat = dlg.BeatCount;
                            int new_clocks_in_beat = dlg.Clock;
                            int new_clock = bar_top_clock + (new_beat - 1) * clock_per_beat + new_clocks_in_beat;
                            CadenciiCommand run = new CadenciiCommand( 
                                VsqCommand.generateCommandUpdateTempo( new_clock, new_clock, (int)(6e7 / (double)dlg.Tempo) ) );
                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                            Edited = true;
                            refreshScreen();
                        }
                    }

                    #endregion
                } else if ( 32 < e.Y && e.Y <= picturePositionIndicator.Height - 1 ) {
                    #region 拍子の変更
                    int index = -1;
                    // クリック位置に拍子が表示されているかどうか検査
                    for ( int i = 0; i < AppManager.getVsqFile().TimesigTable.size(); i++ ) {
                        String s = AppManager.getVsqFile().TimesigTable.get( i ).Numerator + "/" + AppManager.getVsqFile().TimesigTable.get( i ).Denominator;
                        int x = xCoordFromClocks( AppManager.getVsqFile().TimesigTable.get( i ).Clock );
                        SizeF size = AppManager.measureString( s, new Font( AppManager.editorConfig.ScreenFontName, 8 ) );
                        if ( IsInRect( e.Location, new Rectangle( x, 28, (int)size.Width, 14 ) ) ) {
                            index = i;
                            break;
                        }
                    }

                    if ( index < 0 ) {
                        return;
                    }

                    int pre_measure = AppManager.getVsqFile().getPreMeasure();
                    int clock = clockFromXCoord( e.X );
                    int bar_count = AppManager.getVsqFile().getBarCountFromClock( clock );
                    int numerator, denominator;
                    int total_clock = AppManager.getVsqFile().TotalClocks;
                    //int max_barcount = AppManager.VsqFile.getBarCountFromClock( total_clock );
                    //int min_barcount = 1 - pre_measure;
                    AppManager.getVsqFile().getTimesigAt( clock, out numerator, out denominator );
                    boolean num_enabled = !(bar_count == 0);
                    using ( FormBeatConfig dlg = new FormBeatConfig( bar_count - pre_measure + 1, numerator, denominator, num_enabled, pre_measure ) ) {
                        dlg.Location = GetFormPreferedLocation( dlg );
                        if ( dlg.ShowDialog() == DialogResult.OK ) {
                            if ( dlg.EndSpecified ) {
                                int[] new_barcounts = new int[2];
                                int[] numerators = new int[2];
                                int[] denominators = new int[2];
                                int[] barcounts = new int[2];
                                new_barcounts[0] = dlg.Start + pre_measure - 1;
                                new_barcounts[1] = dlg.End + pre_measure - 1;
                                numerators[0] = dlg.Numerator;
                                denominators[0] = dlg.Denominator;
                                numerators[1] = numerator;
                                denominators[1] = denominator;
                                barcounts[0] = bar_count;
                                barcounts[1] = dlg.End + pre_measure - 1;
                                CadenciiCommand run = new CadenciiCommand( 
                                    VsqCommand.generateCommandUpdateTimesigRange( barcounts, new_barcounts, numerators, denominators ) );
                                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                Edited = true;
                            } else {
#if DEBUG
                                Console.WriteLine( "picturePositionIndicator_MouseDoubleClick" );
                                Console.WriteLine( "    bar_count=" + bar_count );
                                Console.WriteLine( "    dlg.Start+pre_measure-1=" + (dlg.Start + pre_measure - 1) );
#endif
                                CadenciiCommand run = new CadenciiCommand(
                                    VsqCommand.generateCommandUpdateTimesig( bar_count, dlg.Start + pre_measure - 1, dlg.Numerator, dlg.Denominator ) );
                                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                Edited = true;
                            }
                        }
                    }
                    #endregion
                }
                picturePositionIndicator.Refresh();
                pictPianoRoll.Refresh();
            }
        }

        private void picturePositionIndicator_MouseDown( object sender, MouseEventArgs e ) {
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }

            if ( e.X < AppManager.KEY_LENGTH || Width - 3 < e.X ) {
                return;
            }

            m_startmark_dragging = false;
            m_endmark_dragging = false;
            Keys modifier = Control.ModifierKeys;
            if ( e.Button == MouseButtons.Left ) {
                if ( 0 <= e.Y && e.Y <= 18 ) {
                    #region スタート/エンドマーク
                    if ( AppManager.startMarkerEnabled ) {
                        int startx = xCoordFromClocks( AppManager.startMarker ) - AppManager.editorConfig.PxTolerance;
                        if ( startx <= e.X && e.X <= startx + AppManager.editorConfig.PxTolerance * 2 + Properties.Resources.start_marker.Width ) {
                            m_startmark_dragging = true;
                        }
                    }
                    if ( AppManager.endMarkerEnabled && !m_startmark_dragging ) {
                        int endx = xCoordFromClocks( AppManager.endMarker ) - Properties.Resources.end_marker.Width - AppManager.editorConfig.PxTolerance;
                        if ( endx <= e.X && e.X <= endx + AppManager.editorConfig.PxTolerance * 2 + Properties.Resources.end_marker.Width ) {
                            m_endmark_dragging = true;
                        }
                    }
                    #endregion
                } else if ( 18 < e.Y && e.Y <= 32 ) {
                    #region テンポ
                    int index = -1;
                    for ( int i = 0; i < AppManager.getVsqFile().TempoTable.size(); i++ ) {
                        int clock = AppManager.getVsqFile().TempoTable.get( i ).Clock;
                        int x = xCoordFromClocks( clock );
                        if ( x < 0 ) {
                            continue;
                        } else if ( Width < x ) {
                            break;
                        }
                        String s = (60e6 / (float)AppManager.getVsqFile().TempoTable.get( i ).Tempo).ToString( "#.00" );
                        SizeF size = AppManager.measureString( s, new Font( AppManager.editorConfig.ScreenFontName, 8 ) );
                        if ( IsInRect( e.Location, new Rectangle( x, 14, (int)size.Width, 14 ) ) ) {
                            index = i;
                            break;
                        }
                    }

                    if ( index >= 0 ) {
                        int clock = AppManager.getVsqFile().TempoTable.get( index ).Clock;
                        if ( AppManager.getSelectedTool() != EditTool.ERASER ) {
                            int mouse_clock = clockFromXCoord( e.X );
                            m_tempo_dragging_deltaclock = mouse_clock - clock;
                            m_tempo_dragging = true;
                        }
                        if ( (modifier & Keys.Shift) == Keys.Shift ) {
                            if ( AppManager.getSelectedTempoCount() > 0 ) {
                                int last_clock = AppManager.getLastSelectedTempoClock();
                                int start = Math.Min( last_clock, clock );
                                int end = Math.Max( last_clock, clock );
                                for ( int i = 0; i < AppManager.getVsqFile().TempoTable.size(); i++ ) {
                                    int tclock = AppManager.getVsqFile().TempoTable.get( i ).Clock;
                                    if ( tclock < start ) {
                                        continue;
                                    } else if ( end < tclock ) {
                                        break;
                                    }
                                    if ( start <= tclock && tclock <= end ) {
                                        AppManager.addSelectedTempo( tclock );
                                    }
                                }
                            } else {
                                AppManager.addSelectedTempo( clock );
                            }
                        } else if ( (modifier & s_modifier_key) == s_modifier_key ) {
                            if ( AppManager.isSelectedTempoContains( clock ) ) {
                                AppManager.removeSelectedTempo( clock );
                            } else {
                                AppManager.addSelectedTempo( clock );
                            }
                        } else {
                            if ( !AppManager.isSelectedTempoContains( clock ) ) {
                                AppManager.clearSelectedTempo();
                            }
                            AppManager.addSelectedTempo( clock );
                        }
                    } else {
                        AppManager.clearSelectedEvent();
                        AppManager.clearSelectedTempo();
                        AppManager.clearSelectedTimesig();
                    }
                    #endregion
                } else if ( 32 < e.Y && e.Y <= picturePositionIndicator.Height - 1 ) {
                    #region 拍子
                    // クリック位置に拍子が表示されているかどうか検査
                    int index = -1;
                    for ( int i = 0; i < AppManager.getVsqFile().TimesigTable.size(); i++ ) {
                        String s = AppManager.getVsqFile().TimesigTable.get( i ).Numerator + "/" + AppManager.getVsqFile().TimesigTable.get( i ).Denominator;
                        int x = xCoordFromClocks( AppManager.getVsqFile().TimesigTable.get( i ).Clock );
                        SizeF size = AppManager.measureString( s, new Font( AppManager.editorConfig.ScreenFontName, 8 ) );
                        if ( IsInRect( e.Location, new Rectangle( x, 28, (int)size.Width, 14 ) ) ) {
                            index = i;
                            break;
                        }
                    }

                    if ( index >= 0 ) {
                        int barcount = AppManager.getVsqFile().TimesigTable.get( index ).BarCount;
                        if ( AppManager.getSelectedTool() != EditTool.ERASER ) {
                            int barcount_clock = AppManager.getVsqFile().getClockFromBarCount( barcount );
                            int mouse_clock = clockFromXCoord( e.X );
                            m_timesig_dragging_deltaclock = mouse_clock - barcount_clock;
                            m_timesig_dragging = true;
                        }
                        if ( (modifier & Keys.Shift) == Keys.Shift ) {
                            if ( AppManager.getSelectedTimesigCount() > 0 ) {
                                int last_barcount = AppManager.getLastSelectedTimesigBarcount();
                                int start = Math.Min( last_barcount, barcount );
                                int end = Math.Max( last_barcount, barcount );
                                for ( int i = 0; i < AppManager.getVsqFile().TimesigTable.size(); i++ ) {
                                    int tbarcount = AppManager.getVsqFile().TimesigTable.get( i ).BarCount;
                                    if ( tbarcount < start ) {
                                        continue;
                                    } else if ( end < tbarcount ) {
                                        break;
                                    }
                                    if ( start <= tbarcount && tbarcount <= end ) {
                                        AppManager.addSelectedTimesig( AppManager.getVsqFile().TimesigTable.get( i ).BarCount );
                                    }
                                }
                            } else {
                                AppManager.addSelectedTimesig( barcount );
                            }
                        } else if ( (modifier & s_modifier_key) == s_modifier_key ) {
                            if ( AppManager.isSelectedTimesigContains( barcount ) ) {
                                AppManager.removeSelectedTimesig( barcount );
                            } else {
                                AppManager.addSelectedTimesig( barcount );
                            }
                        } else {
                            if ( !AppManager.isSelectedTimesigContains( barcount ) ) {
                                AppManager.clearSelectedTimesig();
                            }
                            AppManager.addSelectedTimesig( barcount );
                        }
                    } else {
                        AppManager.clearSelectedEvent();
                        AppManager.clearSelectedTempo();
                        AppManager.clearSelectedTimesig();
                    }
                    #endregion
                }
            }
            refreshScreen();
        }

        private void picturePositionIndicator_MouseClick( object sender, MouseEventArgs e ) {
            if ( e.X < AppManager.KEY_LENGTH || Width - 3 < e.X ) {
                return;
            }

            Keys modifier = Control.ModifierKeys;
#if DEBUG
            AppManager.debugWriteLine( "picturePositionIndicator_MouseClick" );
#endif
            if ( e.Button == MouseButtons.Left ) {
                if ( 4 <= e.Y && e.Y <= 18 ) {
                    #region マーカー位置の変更
                    if ( !m_startmark_dragging && !m_endmark_dragging ) {

                        int clock = clockFromXCoord( e.X );
                        if ( AppManager.editorConfig.PositionQuantize != QuantizeMode.off ) {
                            int unit = AppManager.getPositionQuantizeClock();
                            int odd = clock % unit;
                            clock -= odd;
                            if ( odd > unit / 2 ) {
                                clock += unit;
                            }
                        }
                        AppManager.setCurrentClock( clock );
                        refreshScreen();
                    } else {
                        m_startmark_dragging = false;
                        m_endmark_dragging = false;
                    }
                    #endregion
                } else if ( 18 < e.Y && e.Y <= 32 ) {
                    if ( m_tempo_dragging ) {
                        int count = AppManager.getSelectedTempoCount();
                        int[] clocks = new int[count];
                        int[] new_clocks = new int[count];
                        int[] tempos = new int[count];
                        int i = -1;
                        boolean contains_first_tempo = false;
                        for ( Iterator itr = AppManager.getSelectedTempoIterator(); itr.hasNext(); ){
                            ValuePair<Integer, SelectedTempoEntry> item = (ValuePair<Integer, SelectedTempoEntry>)itr.next();
                            int clock = item.Key;
                            i++;
                            clocks[i] = clock;
                            if ( clock == 0 ) {
                                contains_first_tempo = true;
                                break;
                            }
                            TempoTableEntry editing = AppManager.getSelectedTempo( clock ).editing;
                            new_clocks[i] = editing.Clock;
                            tempos[i] = editing.Tempo;
                        }
                        if ( contains_first_tempo ) {
                            System.Media.SystemSounds.Asterisk.Play();
                        } else {
                            CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandUpdateTempoRange( clocks, new_clocks, tempos ) );
                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                            Edited = true;
                        }
                        m_tempo_dragging = false;
                    } else if( !m_startmark_dragging && !m_endmark_dragging ) {
                        #region テンポの変更
#if DEBUG
                        AppManager.debugWriteLine( "TempoChange" );
#endif
                        AppManager.clearSelectedEvent();
                        AppManager.clearSelectedTimesig();
                        if ( AppManager.getSelectedTempoCount() > 0 ) {
                            #region テンポ変更があった場合
                            int index = -1;
                            int clock = AppManager.getLastSelectedTempoClock();
                            for ( int i = 0; i < AppManager.getVsqFile().TempoTable.size(); i++ ) {
                                if ( clock == AppManager.getVsqFile().TempoTable.get( i ).Clock ) {
                                    index = i;
                                    break;
                                }
                            }
                            if ( index >= 0 && AppManager.getSelectedTool() == EditTool.ERASER ) {
                                #region ツールがEraser
                                if ( AppManager.getVsqFile().TempoTable.get( index ).Clock == 0 ) {
                                    statusLabel.Text = _( "Cannot remove first symbol of track!" );
                                    SystemSounds.Asterisk.Play();
                                    return;
                                }
                                CadenciiCommand run = new CadenciiCommand(
                                    VsqCommand.generateCommandUpdateTempo( AppManager.getVsqFile().TempoTable.get( index ).Clock,
                                                                 AppManager.getVsqFile().TempoTable.get( index ).Clock,
                                                                 -1 ) );
                                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                Edited = true;
                                #endregion
                            }
                            #endregion
                        } else {
                            #region テンポ変更がなかった場合
                            AppManager.clearSelectedEvent();
                            AppManager.clearSelectedTempo();
                            AppManager.clearSelectedTimesig();
                            switch ( AppManager.getSelectedTool() ) {
                                case EditTool.ARROW:
                                case EditTool.ERASER:
                                    break;
                                case EditTool.PENCIL:
                                case EditTool.LINE:
                                    int changing_clock = clockFromXCoord( e.X );
                                    int changing_tempo = AppManager.getVsqFile().getTempoAt( changing_clock );
                                    int bar_count;
                                    int bar_top_clock;
                                    int local_denominator, local_numerator;
                                    bar_count = AppManager.getVsqFile().getBarCountFromClock( changing_clock );
                                    bar_top_clock = AppManager.getVsqFile().getClockFromBarCount( bar_count );
                                    int index2 = -1;
                                    for ( int i = 0; i < AppManager.getVsqFile().TimesigTable.size(); i++ ) {
                                        if ( AppManager.getVsqFile().TimesigTable.get( i ).BarCount > bar_count ) {
                                            index2 = i;
                                            break;
                                        }
                                    }
                                    if ( index2 >= 1 ) {
                                        local_denominator = AppManager.getVsqFile().TimesigTable.get( index2 - 1 ).Denominator;
                                        local_numerator = AppManager.getVsqFile().TimesigTable.get( index2 - 1 ).Numerator;
                                    } else {
                                        local_denominator = AppManager.getVsqFile().TimesigTable.get( 0 ).Denominator;
                                        local_numerator = AppManager.getVsqFile().TimesigTable.get( 0 ).Numerator;
                                    }
                                    int clock_per_beat = 480 * 4 / local_denominator;
                                    int clocks_in_bar = changing_clock - bar_top_clock;
                                    int beat_in_bar = clocks_in_bar / clock_per_beat + 1;
                                    int clocks_in_beat = clocks_in_bar - (beat_in_bar - 1) * clock_per_beat;
                                    using ( FormTempoConfig dlg = new FormTempoConfig(
                                        bar_count - AppManager.getVsqFile().getPreMeasure() + 1,
                                        beat_in_bar,
                                        local_numerator,
                                        clocks_in_beat,
                                        clock_per_beat,
                                        (decimal)(6e7 / changing_tempo),
                                        AppManager.getVsqFile().getPreMeasure() ) ) {
                                        dlg.Location = GetFormPreferedLocation( dlg );
                                        if ( dlg.ShowDialog() == DialogResult.OK ) {
                                            int new_beat = dlg.BeatCount;
                                            int new_clocks_in_beat = dlg.Clock;
                                            int new_clock = bar_top_clock + (new_beat - 1) * clock_per_beat + new_clocks_in_beat;
#if DEBUG
                                            AppManager.debugWriteLine( "    new_beat=" + new_beat );
                                            AppManager.debugWriteLine( "    new_clocks_in_beat=" + new_clocks_in_beat );
                                            AppManager.debugWriteLine( "    changing_clock=" + changing_clock );
                                            AppManager.debugWriteLine( "    new_clock=" + new_clock );
#endif
                                            CadenciiCommand run = new CadenciiCommand(
                                                VsqCommand.generateCommandUpdateTempo( new_clock, new_clock, (int)(6e7 / (double)dlg.Tempo) ) );
                                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                            Edited = true;
                                            refreshScreen();
                                        }
                                    }
                                    break;
                            }
                            #endregion
                        }
                        m_startmark_dragging = false;
                        m_endmark_dragging = false;
                        #endregion
                    }
                } else if ( 32 < e.Y && e.Y <= picturePositionIndicator.Height - 1 ) {
                    if ( m_timesig_dragging ) {
                        int count = AppManager.getSelectedTimesigCount();
                        int[] barcounts = new int[count];
                        int[] new_barcounts = new int[count];
                        int[] numerators = new int[count];
                        int[] denominators = new int[count];
                        int i = -1;
                        boolean contains_first_bar = false;
                        for ( Iterator itr = AppManager.getSelectedTimesigIterator(); itr.hasNext(); ){
                            ValuePair<Integer, SelectedTimesigEntry> item = (ValuePair<Integer, SelectedTimesigEntry>)itr.next();
                            int bar = item.Key;
                            i++;
                            barcounts[i] = bar;
                            if ( bar == 0 ) {
                                contains_first_bar = true;
                                break;
                            }
                            TimeSigTableEntry editing = AppManager.getSelectedTimesig( bar ).editing;
                            new_barcounts[i] = editing.BarCount;
                            numerators[i] = editing.Numerator;
                            denominators[i] = editing.Denominator;
                        }
                        if ( contains_first_bar ) {
                            System.Media.SystemSounds.Asterisk.Play();
                        } else {
                            CadenciiCommand run = new CadenciiCommand(
                                VsqCommand.generateCommandUpdateTimesigRange( barcounts, new_barcounts, numerators, denominators ) );
                            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                            Edited = true;
                        }
                        m_timesig_dragging = false;
                    } else if ( !m_startmark_dragging && !m_endmark_dragging ) {
                        #region 拍子の変更
                        AppManager.clearSelectedEvent();
                        AppManager.clearSelectedTempo();
                        if ( AppManager.getSelectedTimesigCount() > 0 ) {
                            #region 拍子変更があった場合
                            int index = 0;
                            int last_barcount = AppManager.getLastSelectedTimesigBarcount();
                            for ( int i = 0; i < AppManager.getVsqFile().TimesigTable.size(); i++ ) {
                                if ( AppManager.getVsqFile().TimesigTable.get( i ).BarCount == last_barcount ) {
                                    index = i;
                                    break;
                                }
                            }
                            if ( AppManager.getSelectedTool() == EditTool.ERASER ) {
                                #region ツールがEraser
                                if ( AppManager.getVsqFile().TimesigTable.get( index ).Clock == 0 ) {
                                    statusLabel.Text = _( "Cannot remove first symbol of track!" );
                                    SystemSounds.Asterisk.Play();
                                    return;
                                }
                                int barcount = AppManager.getVsqFile().TimesigTable.get( index ).BarCount;
                                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandUpdateTimesig( barcount, barcount, -1, -1 ) );
                                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                Edited = true;
                                #endregion
                            }
                            #endregion
                        } else {
                            #region 拍子変更がなかった場合
                            AppManager.clearSelectedEvent();
                            AppManager.clearSelectedTempo();
                            AppManager.clearSelectedTimesig();
                            switch ( AppManager.getSelectedTool() ) {
                                case EditTool.ERASER:
                                case EditTool.ARROW:
                                    break;
                                case EditTool.PENCIL:
                                case EditTool.LINE:
                                    int pre_measure = AppManager.getVsqFile().getPreMeasure();
                                    int clock = clockFromXCoord( e.X );
                                    int bar_count = AppManager.getVsqFile().getBarCountFromClock( clock );
                                    int numerator, denominator;
                                    AppManager.getVsqFile().getTimesigAt( clock, out numerator, out denominator );
                                    int total_clock = AppManager.getVsqFile().TotalClocks;
                                    //int max_barcount = AppManager.VsqFile.getBarCountFromClock( total_clock ) - pre_measure + 1;
                                    //int min_barcount = 1;
#if DEBUG
                                    AppManager.debugWriteLine( "FormMain.picturePositionIndicator_MouseClick; bar_count=" + (bar_count - pre_measure + 1) );
#endif
                                    using ( FormBeatConfig dlg = new FormBeatConfig( bar_count - pre_measure + 1, numerator, denominator, true, pre_measure ) ) {
                                        dlg.Location = GetFormPreferedLocation( dlg );
                                        if ( dlg.ShowDialog() == DialogResult.OK ) {
                                            if ( dlg.EndSpecified ) {
                                                int[] new_barcounts = new int[2];
                                                int[] numerators = new int[2];
                                                int[] denominators = new int[2];
                                                int[] barcounts = new int[2];
                                                new_barcounts[0] = dlg.Start + pre_measure - 1;
                                                new_barcounts[1] = dlg.End + pre_measure - 1 + 1;
                                                numerators[0] = dlg.Numerator;
                                                numerators[1] = numerator;
                                                
                                                denominators[0] = dlg.Denominator;
                                                denominators[1] = denominator;
                                                
                                                barcounts[0] = dlg.Start + pre_measure - 1;
                                                barcounts[1] = dlg.End + pre_measure - 1 + 1;
                                                CadenciiCommand run = new CadenciiCommand(
                                                    VsqCommand.generateCommandUpdateTimesigRange( barcounts, new_barcounts, numerators, denominators ) );
                                                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                                Edited = true;
                                            } else {
                                                CadenciiCommand run = new CadenciiCommand( 
                                                    VsqCommand.generateCommandUpdateTimesig( bar_count, 
                                                                                   dlg.Start + pre_measure - 1, 
                                                                                   dlg.Numerator, 
                                                                                   dlg.Denominator ) );
                                                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                                                Edited = true;
                                            }
                                        }
                                    }
                                    break;
                            }
                            #endregion
                        }
                        m_startmark_dragging = false;
                        m_endmark_dragging = false;
                        #endregion
                    }
                }
            }
            pictPianoRoll.Refresh();
            picturePositionIndicator.Refresh();
        }

        private void picturePositionIndicator_MouseMove( object sender, MouseEventArgs e ) {
            if ( m_tempo_dragging ) {
                int clock = clockFromXCoord( e.X ) - m_tempo_dragging_deltaclock;
                int step = AppManager.getPositionQuantizeClock();
                int odd = clock % step;
                clock -= odd;
                if ( odd > step / 2 ) {
                    clock += step;
                }
                int last_clock = AppManager.getLastSelectedTempoClock();
                int dclock = clock - last_clock;
                for ( Iterator itr = AppManager.getSelectedTempoIterator(); itr.hasNext(); ){
                    ValuePair<Integer, SelectedTempoEntry> item = (ValuePair<Integer, SelectedTempoEntry>)itr.next();
                    int key = item.Key;
                    AppManager.getSelectedTempo( key ).editing.Clock = AppManager.getSelectedTempo( key ).original.Clock + dclock;
                }
                picturePositionIndicator.Refresh();
            } else if ( m_timesig_dragging ) {
                int clock = clockFromXCoord( e.X ) - m_timesig_dragging_deltaclock;
                int barcount = AppManager.getVsqFile().getBarCountFromClock( clock );
                int last_barcount = AppManager.getLastSelectedTimesigBarcount();
                int dbarcount = barcount - last_barcount;
                for ( Iterator itr = AppManager.getSelectedTimesigIterator(); itr.hasNext(); ){
                    ValuePair<Integer, SelectedTimesigEntry> item = (ValuePair<Integer, SelectedTimesigEntry>)itr.next();
                    int bar = item.Key;
                    AppManager.getSelectedTimesig( bar ).editing.BarCount = AppManager.getSelectedTimesig( bar ).original.BarCount + dbarcount;
                }
                picturePositionIndicator.Refresh();
            } else if ( m_startmark_dragging ) {
                int clock = clockFromXCoord( e.X );
                int unit = AppManager.getPositionQuantizeClock();
                int odd = clock % unit;
                clock -= odd;
                if ( odd > unit / 2 ) {
                    clock += unit;
                }
                int draft_start = Math.Min( clock, AppManager.endMarker );
                int draft_end = Math.Max( clock, AppManager.endMarker );
                if ( draft_start != AppManager.startMarker ) {
                    AppManager.startMarker = draft_start;
                }
                if ( draft_end != AppManager.endMarker ) {
                    AppManager.endMarker = draft_end;
                }
                refreshScreen();
            } else if ( m_endmark_dragging ) {
                int clock = clockFromXCoord( e.X );
                int unit = AppManager.getPositionQuantizeClock();
                int odd = clock % unit;
                clock -= odd;
                if ( odd > unit / 2 ) {
                    clock += unit;
                }
                int draft_start = Math.Min( clock, AppManager.startMarker );
                int draft_end = Math.Max( clock, AppManager.startMarker );
                if ( draft_start != AppManager.startMarker ) {
                    AppManager.startMarker = draft_start;
                }
                if ( draft_end != AppManager.endMarker ) {
                    AppManager.endMarker = draft_end;
                }
                refreshScreen();
            }
        }

        private void picturePositionIndicator_MouseLeave( object sender, EventArgs e ) {
            m_startmark_dragging = false;
            m_endmark_dragging = false;
            m_tempo_dragging = false;
            m_timesig_dragging = false;
        }

        private void picturePositionIndicator_Paint( object sender, PaintEventArgs e ) {
            picturePositionIndicatorDrawTo( e.Graphics );
        }

        private void picturePositionIndicator_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e ) {
            ProcessSpecialShortcutKey( e );
        }
        #endregion

        #region trackBar
        private void trackBar_Enter( object sender, EventArgs e ) {
            pictPianoRoll.Select();
        }

        private void trackBar_MouseDown( object sender, MouseEventArgs e ) {
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
        }

        private void trackBar_ValueChanged( object sender, EventArgs e ) {
            AppManager.scaleX = trackBar.Value / 480f;
            AppManager.startToDrawX = (int)(hScroll.Value * AppManager.scaleX);
#if USE_DOBJ
            UpdateDrawObjectList();
#endif
            this.Refresh();
        }
        #endregion

        #region menuHelp
        private void menuHelpAbout_Click( object sender, EventArgs e ) {
            String version_str = AppManager.getVersion() + "\n\n" +
                                 AppManager.getAssemblyNameAndFileVersion( typeof( Boare.Lib.AppUtil.Util ) ) + "\n" +
                                 AppManager.getAssemblyNameAndFileVersion( typeof( Boare.Lib.Media.Wave ) ) + "\n" +
                                 AppManager.getAssemblyNameAndFileVersion( typeof( Boare.Lib.Vsq.VsqFile ) ) + "\n" +
                                 AppManager.getAssemblyNameAndFileVersion( typeof( bocoree.math ) /*) + "\n" +
                                 AppManager.GetAssemblyNameAndFileVersion( typeof( Boare.Cadencii.vstidrv )*/ );
            if ( m_versioninfo == null ) {
                m_versioninfo = new Boare.Cadencii.VersionInfo( _APP_NAME, version_str );
                m_versioninfo.Credit = Boare.Cadencii.Properties.Resources.author_list;
                m_versioninfo.AuthorList = _CREDIT;
#if DEBUG
#if AUTHOR_LIST_SAVE_BUTTON_VISIBLE
                m_versioninfo.SaveAuthorListVisible = true;
#else
                m_versioninfo.SaveAuthorListVisible = false;
#endif
#else
                m_versioninfo.SaveAuthorListVisible = false;
#endif
                m_versioninfo.Show();
            } else {
                if ( m_versioninfo.IsDisposed ) {
                    m_versioninfo = new Boare.Cadencii.VersionInfo( _APP_NAME, version_str );
                    m_versioninfo.Credit = Boare.Cadencii.Properties.Resources.author_list;
                    m_versioninfo.AuthorList = _CREDIT;
#if DEBUG
#if AUTHOR_LIST_SAVE_BUTTON_VISIBLE
                    m_versioninfo.SaveAuthorListVisible = true;
#else
                    m_versioninfo.SaveAuthorListVisible = false;
#endif
#else
                    m_versioninfo.SaveAuthorListVisible = false;
#endif
                }
                m_versioninfo.Show();
            }
        }

        private void menuHelpDebug_Click( object sender, EventArgs e ) {
            Console.WriteLine( "menuHelpDebug_Click" );
#if DEBUG
            /*InputBox ib = new InputBox( "input shift seconds" );
            if ( ib.ShowDialog() == DialogResult.OK ) {
                VsqFileEx vsq = (VsqFileEx)AppManager.getVsqFile().Clone();
                VsqFileEx.shift( vsq, double.Parse( ib.Result ) );
                CadenciiCommand run = VsqFileEx.generateCommandReplace( vsq );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                Edited = true;
            }*/
            /*DialogResult dr = DialogResult.OK;
            while ( dr == DialogResult.OK ) {
                FileDialog fd = new FileDialog( FileDialog.DialogMode.Open );
                fd.Filter = "All Files(*.*)|*.*|VSQ Format(*.vsq)|*.vsq";
                dr = fd.ShowDialog();
                AppManager.DebugWriteLine( "fd.FileName=" + fd.FileName );
            }*/
            /*using ( OpenFileDialog ofd = new OpenFileDialog() ) {
                if ( ofd.ShowDialog() == DialogResult.OK ) {
                    using ( SaveFileDialog sfd = new SaveFileDialog() ) {
                        sfd.InitialDirectory = Path.GetDirectoryName( ofd.FileName );
                        sfd.FileName = Path.GetFileNameWithoutExtension( ofd.FileName ) + ".txt";
                        if ( sfd.ShowDialog() == DialogResult.OK ) {
                            using ( Wave w = new Wave( ofd.FileName ) ) {
                                w.PrintToText( sfd.FileName );
                            }
                        }
                    }
                }
            }*/

            /*using ( OpenFileDialog ofd = new OpenFileDialog() ) {
                if ( ofd.ShowDialog() == DialogResult.OK ) {
                    String file = ofd.FileName;
                    using ( Wave wv = new Wave( file ) ) {
                        const double sec_dt = 0.0025;
                        uint samples = wv.TotalSamples;
                        const int _WINDOW_WIDTH = 2048;
                        uint spl_dt = (uint)(sec_dt * wv.SampleRate);
                        double[] window_func = new double[_WINDOW_WIDTH];
                        for ( int i = 0; i < _WINDOW_WIDTH; i++ ) {
                            window_func[i] = bocoree.math.window_func( bocoree.math.WindowFunctionType.Hamming, i / (double)_WINDOW_WIDTH );
                        }
                        Wave.TestEnabled = false;

                        InputBox ib = new InputBox( "input clock count" );
                        if ( ib.ShowDialog() == DialogResult.OK ) {
                            int count = 0;
                            if ( int.TryParse( ib.Result, out count ) ) {
                                Wave.TestEnabled = true;
                                double f0 = wv.GetF0( (uint)count, window_func );
                                MessageBox.Show( "f0=" + f0 + " at sample=" + count );
                                Wave.TestEnabled = false;
                            }
                        }
                        if ( MessageBox.Show( "calculate all formant profile?", "Cadencii", MessageBoxButtons.YesNo ) == DialogResult.Yes ) {
                            using ( StreamWriter sw = new StreamWriter( ofd.FileName + "_formanto.txt" ) ) {
                                FormantoDetectionArguments fda = new FormantoDetectionArguments();
                                fda.PeakDetectionThreshold = 0.05;
                                for ( uint spl_i = 0; spl_i < samples; spl_i += spl_dt ) {
                                    double f0 = wv.GetF0( spl_i, window_func, fda );
                                    double note;
                                    if ( f0 > 0.0 ) {
                                        note = 12.0 * Math.Log( f0 / 440.0, 2.0 ) + 69;
                                    } else {
                                        note = 0.0;
                                    }
                                    sw.WriteLine( spl_i + "\t" + (spl_i / (double)wv.SampleRate) + "\t" + f0 + "\t" + note + "\t" + Math.Round( note, 0, MidpointRounding.AwayFromZero ) );
                                }
                            }
                        }
                        if ( MessageBox.Show( "calculate volume profile?", "Cadencii", MessageBoxButtons.YesNo ) == DialogResult.Yes ) {
                            using ( StreamWriter sw = new StreamWriter( ofd.FileName + "_volume.txt" ) ) {
                                for ( uint spl_i = 0; spl_i < samples; spl_i += spl_dt ) {
                                    double volume = wv.GetVolume( (int)spl_i, window_func );
                                    sw.WriteLine( spl_i + "\t" + (spl_i / (double)wv.SampleRate) + "\t" + volume );
                                }
                            }
                        }
                    }
                }
            }*/
#endif
#if FOO
            using ( OpenFileDialog ofd = new OpenFileDialog() ) {
                if ( ofd.ShowDialog() == DialogResult.OK ) {
                    using ( Wave wv = new Wave( ofd.FileName ) ) {
                        wv.TrimSilence();
                        const int _WIN_LEN = 441;
                        double[] window = new double[_WIN_LEN];
                        for ( int i = 0; i < _WIN_LEN; i++ ) {
                            window[i] = bocoree.math.window_func( bocoree.math.WindowFunctionType.Hamming, (double)i / (double)_WIN_LEN );
                        }
                        using ( StreamWriter sw = new StreamWriter( ofd.FileName + ".txt" ) ) {
                            for ( int i = 0; i < wv.TotalSamples - _WIN_LEN / 10; i += _WIN_LEN / 10 ) {
                                sw.WriteLine( i / (double)wv.SampleRate + "\t" + wv.GetVolume( i, window ) );
                            }
                        }
                        /*using ( StreamWriter sw = new StreamWriter( ofd.FileName + ".txt" ) ) {
                            double to = 7000.0 * 2.0 / (double)wv.SampleRate;
                            int jmax = (int)(to * _WIN_LEN);
                            double resolution = 1e-3;
                            uint di = (uint)(resolution * wv.SampleRate);
                            for ( uint i = 0; i < wv.TotalSamples; i += di ) {
                                double[] formanto = wv.GetFormanto( i, window );
                                for ( int j = 1; j < _WIN_LEN && j < jmax; j++ ) {
                                    //double f = (double)j / (double)_WIN_LEN * (double)wv.SampleRate / 2.0;
                                    sw.Write( "{0:f4}\t", Math.Abs( formanto[j] ) );
                                }
                                sw.WriteLine();
                            }
                        }*/
                    }
                }
            }
#endif
        }
        #endregion

        #region trackSelector
        private void trackSelector_CommandExecuted() {
            Edited = true;
            refreshScreen();
        }

        private void trackSelector_MouseClick( object sender, MouseEventArgs e ) {
            if ( e.Button == MouseButtons.Right ) {
                if ( AppManager.KEY_LENGTH < e.X && e.X < trackSelector.Width - AppManager.KEY_LENGTH ) {
                    if ( trackSelector.Height - TrackSelector.OFFSET_TRACK_TAB <= e.Y && e.Y <= trackSelector.Height ) {
                        cMenuTrackTab.Show( trackSelector, e.Location );
                    } else {
                        cMenuTrackSelector.Show( trackSelector, e.Location );
                    }
                }
            }
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
        }

        private void trackSelector_MouseDown( object sender, MouseEventArgs e ) {
            if ( AppManager.KEY_LENGTH < e.X ) {
                m_mouse_downed_trackselector = true;
                if ( e.Button == MouseButtons.Middle || m_spacekey_downed ) {
                    m_edit_curve_mode = CurveEditMode.MIDDLE_DRAG;
                    m_button_initial = e.Location;
                    m_middle_button_hscroll = hScroll.Value;
                    this.Cursor = HAND;
                }
            }
        }

        private void trackSelector_MouseMove( object sender, MouseEventArgs e ) {
            if ( m_form_activated ) {
                if ( m_input_textbox != null && !m_input_textbox.IsDisposed && !m_input_textbox.Visible && !AppManager.propertyPanel.Editing ) {
                    trackSelector.Focus();
                }
            }
            if ( e.Button == MouseButtons.None ) {
                int cl = clockFromXCoord( e.X );
                UpdatePositionViewFromMousePosition( cl );
                refreshScreen();
                return;
            }
            int parent_width = ((TrackSelector)sender).Width;
            if ( m_edit_curve_mode == CurveEditMode.MIDDLE_DRAG ) {
                int dx = e.X - m_button_initial.X;
                int dy = e.Y - m_button_initial.Y;
                double new_hscroll_value = (double)m_middle_button_hscroll - (double)dx / AppManager.scaleX;
                int draft;
                if ( new_hscroll_value < hScroll.Minimum ) {
                    draft = hScroll.Minimum;
                } else if ( hScroll.Maximum < new_hscroll_value ) {
                    draft = hScroll.Maximum;
                } else {
                    draft = (int)new_hscroll_value;
                }
                if ( AppManager.isPlaying() ) {
                    return;
                }
                if ( hScroll.Value != draft ) {
                    hScroll.Value = draft;
                }
            } else {
                if ( m_mouse_downed_trackselector ) {
                    if ( m_ext_dragx_trackselector == ExtDragXMode.NONE ) {
                        if ( AppManager.KEY_LENGTH > e.X ) {
                            m_ext_dragx_trackselector = ExtDragXMode.LEFT;
                        } else if ( parent_width < e.X ) {
                            m_ext_dragx_trackselector = ExtDragXMode.RIGHT;
                        }
                    } else {
                        if ( AppManager.KEY_LENGTH <= e.X && e.X <= parent_width ) {
                            m_ext_dragx_trackselector = ExtDragXMode.NONE;
                        }
                    }
                } else {
                    m_ext_dragx_trackselector = ExtDragXMode.NONE;
                }

                if ( m_ext_dragx_trackselector != ExtDragXMode.NONE ) {
                    DateTime now = DateTime.Now;
                    double dt = now.Subtract( m_timer_drag_last_ignitted ).TotalSeconds;
                    m_timer_drag_last_ignitted = now;
                    int px_move = AppManager.editorConfig.MouseDragIncrement;
                    if ( px_move / dt > AppManager.editorConfig.MouseDragMaximumRate ) {
                        px_move = (int)(dt * AppManager.editorConfig.MouseDragMaximumRate);
                    }
                    double d_draft;
                    if ( m_ext_dragx_trackselector == ExtDragXMode.RIGHT ) {
                        int right_clock = clockFromXCoord( parent_width + 5 );
                        int dclock = (int)(px_move / AppManager.scaleX);
                        d_draft = (73 - trackSelector.Width) / AppManager.scaleX + right_clock + dclock;
                    } else {
                        px_move *= -1;
                        int left_clock = clockFromXCoord( AppManager.KEY_LENGTH );
                        int dclock = (int)(px_move / AppManager.scaleX);
                        d_draft = (73 - AppManager.KEY_LENGTH) / AppManager.scaleX + left_clock + dclock;
                    }
                    if ( d_draft < 0.0 ) {
                        d_draft = 0.0;
                    }
                    int draft = (int)d_draft;
                    if ( hScroll.Maximum < draft ) {
                        hScroll.Maximum = draft;
                    }
                    if ( draft < hScroll.Minimum ) {
                        draft = hScroll.Minimum;
                    }
                    hScroll.Value = draft;
                }
            }
            int clock = clockFromXCoord( e.X );
            UpdatePositionViewFromMousePosition( clock );
            refreshScreen();
        }

        private void trackSelector_MouseUp( object sender, MouseEventArgs e ) {
            m_mouse_downed_trackselector = false;
            if ( m_edit_curve_mode == CurveEditMode.MIDDLE_DRAG ) {
                m_edit_curve_mode = CurveEditMode.NONE;
                this.Cursor = Cursors.Default;
            }
        }

        private void trackSelector_MouseWheel( object sender, MouseEventArgs e ) {
            if ( (Control.ModifierKeys & Keys.Shift) == Keys.Shift ) {
                double new_val = (double)vScroll.Value - e.Delta;
                if ( new_val > vScroll.Maximum ) {
                    vScroll.Value = vScroll.Maximum;
                } else if ( new_val < vScroll.Minimum ) {
                    vScroll.Value = vScroll.Minimum;
                } else {
                    vScroll.Value = (int)new_val;
                }
            } else {
                hScroll.Value = NewHScrollValueFromWheelDelta( e.Delta );
            }
            refreshScreen();
        }

        private void trackSelector_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e ) {
            ProcessSpecialShortcutKey( e );
        }

        private void trackSelector_RenderRequired( int[] tracks ) {
            Render( tracks );
            Vector<Integer> t = new Vector<Integer>( tracks );
            if ( t.contains( AppManager.getSelected() ) ) {
                String file = Path.Combine( AppManager.getTempWaveDir(), AppManager.getSelected() + ".wav" );
                if ( PortUtil.isFileExists( file ) ) {
                    Thread loadwave_thread = new Thread( new ParameterizedThreadStart( this.LoadWaveThreadProc ) );
                    loadwave_thread.IsBackground = true;
                    loadwave_thread.Start( file );
                }
            }
        }

        private void trackSelector_SelectedCurveChanged( CurveType type ) {
            refreshScreen();
        }        
        
        private void trackSelector_SelectedTrackChanged( int selected ) {
            if ( menuVisualWaveform.Checked ) {
                waveView.Clear();
                String file = Path.Combine( AppManager.getTempWaveDir(), selected + ".wav" );
                if ( PortUtil.isFileExists( file ) ) {
                    Thread load_wave = new Thread( new ParameterizedThreadStart( this.LoadWaveThreadProc ) );
                    load_wave.IsBackground = true;
                    load_wave.Start( (object)file );
                }
            }
            AppManager.clearSelectedBezier();
            AppManager.clearSelectedEvent();
            AppManager.clearSelectedPoint();
#if USE_DOBJ
            UpdateDrawObjectList();
#endif
            refreshScreen();
        }
        #endregion

        #region cMenuPiano*
        private void cMenuPianoDelete_Click( object sender, EventArgs e ) {
            DeleteEvent();
        }

        private void cMenuPianoVibratoProperty_Click( object sender, EventArgs e ) {
            NoteVibratoProperty();
        }

        private void cMenuPianoPaste_Click( object sender, EventArgs e ) {
            pasteEvent();
        }

        private void cMenuPianoCopy_Click( object sender, EventArgs e ) {
            copyEvent();
        }

        private void cMenuPianoCut_Click( object sender, EventArgs e ) {
            CutEvent();
        }
        
        private void cMenuPianoExpression_Click( object sender, EventArgs e ) {
            if ( AppManager.getSelectedEventCount() > 0 ) {
                SynthesizerType type = SynthesizerType.VOCALOID2;
                if ( AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                    type = SynthesizerType.VOCALOID1;
                }
                VsqEvent original = AppManager.getLastSelectedEvent().original;
                using ( FormNoteExpressionConfig dlg = new FormNoteExpressionConfig( type, original.ID.NoteHeadHandle ) ) {
                    int id = AppManager.getLastSelectedEvent().original.InternalID;
                    dlg.PMBendDepth = original.ID.PMBendDepth;
                    dlg.PMBendLength = original.ID.PMBendLength;
                    dlg.PMbPortamentoUse = original.ID.PMbPortamentoUse;
                    dlg.DEMdecGainRate = original.ID.DEMdecGainRate;
                    dlg.DEMaccent = original.ID.DEMaccent;
                    if ( dlg.ShowDialog() == DialogResult.OK ) {
                        VsqID copy = (VsqID)original.ID.clone();
                        copy.PMBendDepth = dlg.PMBendDepth;
                        copy.PMBendLength = dlg.PMBendLength;
                        copy.PMbPortamentoUse = dlg.PMbPortamentoUse;
                        copy.DEMdecGainRate = dlg.DEMdecGainRate;
                        copy.DEMaccent = dlg.DEMaccent;
                        copy.NoteHeadHandle = dlg.getEditedNoteHeadHandle();
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(), id, copy ) );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                        Edited = true;
                    }
                }
            }
        }

        private void cMenuPianoPointer_Click( object sender, EventArgs e ) {
            AppManager.setSelectedTool( EditTool.ARROW );
        }

        private void cMenuPianoPencil_Click( object sender, EventArgs e ) {
            AppManager.setSelectedTool( EditTool.PENCIL );
        }

        private void cMenuPianoEraser_Click( object sender, EventArgs e ) {
            AppManager.setSelectedTool( EditTool.ERASER );
        }

        private void h_positionQuantize04( object sender, EventArgs e ) {
            AppManager.editorConfig.PositionQuantize = QuantizeMode.p4;
        }

        private void h_positionQuantize08( object sender, EventArgs e ) {
            AppManager.editorConfig.PositionQuantize = QuantizeMode.p8;
        }

        private void h_positionQuantize16( object sender, EventArgs e ) {
            AppManager.editorConfig.PositionQuantize = QuantizeMode.p16;
        }

        private void h_positionQuantize32( object sender, EventArgs e ) {
            AppManager.editorConfig.PositionQuantize = QuantizeMode.p32;
        }

        private void h_positionQuantize64( object sender, EventArgs e ) {
            AppManager.editorConfig.PositionQuantize = QuantizeMode.p64;
        }

        private void h_positionQuantize128( object sender, EventArgs e ) {
            AppManager.editorConfig.PositionQuantize = QuantizeMode.p128;
        }

        private void h_positionQuantizeOff( object sender, EventArgs e ) {
            AppManager.editorConfig.PositionQuantize = QuantizeMode.off;
        }

        private void h_positionQuantizeTriplet( object sender, EventArgs e ) {
            AppManager.editorConfig.PositionQuantizeTriplet = !AppManager.editorConfig.PositionQuantizeTriplet;
        }

        private void h_lengthQuantize04( object sender, EventArgs e ) {
            AppManager.editorConfig.LengthQuantize = QuantizeMode.p4;
        }

        private void h_lengthQuantize08( object sender, EventArgs e ) {
            AppManager.editorConfig.LengthQuantize = QuantizeMode.p8;
        }

        private void h_lengthQuantize16( object sender, EventArgs e ) {
            AppManager.editorConfig.LengthQuantize = QuantizeMode.p16;
        }

        private void h_lengthQuantize32( object sender, EventArgs e ) {
            AppManager.editorConfig.LengthQuantize = QuantizeMode.p32;
        }

        private void h_lengthQuantize64( object sender, EventArgs e ) {
            AppManager.editorConfig.LengthQuantize = QuantizeMode.p64;
        }

        private void h_lengthQuantize128( object sender, EventArgs e ) {
            AppManager.editorConfig.LengthQuantize = QuantizeMode.p128;
        }

        private void h_lengthQuantizeOff( object sender, EventArgs e ) {
            AppManager.editorConfig.LengthQuantize = QuantizeMode.off;
        }

        private void h_lengthQuantizeTriplet( object sender, EventArgs e ) {
            AppManager.editorConfig.LengthQuantizeTriplet = !AppManager.editorConfig.LengthQuantizeTriplet;
        }

        private void cMenuPianoGrid_Click( object sender, EventArgs e ) {
            cMenuPianoGrid.Checked = !cMenuPianoGrid.Checked;
            AppManager.setGridVisible( cMenuPianoGrid.Checked );
        }

        private void cMenuPianoUndo_Click( object sender, EventArgs e ) {
            undo();
        }

        private void cMenuPianoRedo_Click( object sender, EventArgs e ) {
            redo();
        }

        private void cMenuPianoSelectAllEvents_Click( object sender, EventArgs e ) {
            selectAllEvent();
        }       
        
        private void cMenuPianoProperty_Click( object sender, EventArgs e ) {
            NoteExpressionProperty();
        }
        
        private void cMenuPianoImportLyric_Click( object sender, EventArgs e ) {
            ImportLyric();
        }

        private void cMenuPiano_Opening( object sender, CancelEventArgs e ) {
            updateCopyAndPasteButtonStatus();
            cMenuPianoImportLyric.Enabled = AppManager.getLastSelectedEvent() != null;
        }

        private void cMenuPianoSelectAll_Click( object sender, EventArgs e ) {
            selectAll();
        }

        private void cMenuPianoFixed01_Click( object sender, EventArgs e ) {
            m_pencil_mode.Mode = PencilModeEnum.L1;
            UpdateCMenuPianoFixed();
        }
        
        private void cMenuPianoFixed02_Click( object sender, EventArgs e ) {
            m_pencil_mode.Mode = PencilModeEnum.L2;
            UpdateCMenuPianoFixed();
        }
        
        private void cMenuPianoFixed04_Click( object sender, EventArgs e ) {
            m_pencil_mode.Mode = PencilModeEnum.L4;
            UpdateCMenuPianoFixed();
        }

        private void cMenuPianoFixed08_Click( object sender, EventArgs e ) {
            m_pencil_mode.Mode = PencilModeEnum.L8;
            UpdateCMenuPianoFixed();
        }
        
        private void cMenuPianoFixed16_Click( object sender, EventArgs e ) {
            m_pencil_mode.Mode = PencilModeEnum.L16;
            UpdateCMenuPianoFixed();
        }
        
        private void cMenuPianoFixed32_Click( object sender, EventArgs e ) {
            m_pencil_mode.Mode = PencilModeEnum.L32;
            UpdateCMenuPianoFixed();
        }

        private void cMenuPianoFixed64_Click( object sender, EventArgs e ) {
            m_pencil_mode.Mode = PencilModeEnum.L64;
            UpdateCMenuPianoFixed();
        }

        private void cMenuPianoFixed128_Click( object sender, EventArgs e ) {
            m_pencil_mode.Mode = PencilModeEnum.L128;
            UpdateCMenuPianoFixed();
        }

        private void cMenuPianoFixedOff_Click( object sender, EventArgs e ) {
            m_pencil_mode.Mode = PencilModeEnum.Off;
            UpdateCMenuPianoFixed();
        }

        private void cMenuPianoFixedTriplet_Click( object sender, EventArgs e ) {
            m_pencil_mode.Triplet = !m_pencil_mode.Triplet;
            UpdateCMenuPianoFixed();
        }
        
        private void cMenuPianoFixedDotted_Click( object sender, EventArgs e ) {
            m_pencil_mode.Dot = !m_pencil_mode.Dot;
            UpdateCMenuPianoFixed();
        }

        private void cMenuPianoCurve_Click( object sender, EventArgs e ) {
            AppManager.setCurveMode( !AppManager.isCurveMode() );
            applySelectedTool();
        }
        #endregion

        #region menuTrack*
        private void menuTrack_DropDownOpening( object sender, EventArgs e ) {
            UpdateTrackMenuStatus();
        }

        private void menuTrackCopy_Click( object sender, EventArgs e ) {
            CopyTrackCore();
        }

        private void menuTrackChangeName_Click( object sender, EventArgs e ) {
            ChangeTrackNameCore();
        }

        private void menuTrackDelete_Click( object sender, EventArgs e ) {
            DeleteTrackCore();
        }

        private void menuTrackOn_Click( object sender, EventArgs e ) {
            menuTrackOn.Checked = !menuTrackOn.Checked;
            CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandTrackChangePlayMode( AppManager.getSelected(),
                                                                                                      menuTrackOn.Checked ? 1 : -1 ) );
            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
            Edited = true;
            refreshScreen();
        }

        private void menuTrackAdd_Click( object sender, EventArgs e ) {
            AddTrackCore();
        }

        private void menuTrackOverlay_Click( object sender, EventArgs e ) {
            AppManager.setOverlay( !AppManager.isOverlay() );
            refreshScreen();
        }

        private void menuTrackRenderCurrent_Click( object sender, EventArgs e ) {
            Render( new int[] { AppManager.getSelected() } );
        }

        private void commonTrackRenderAll_Click( object sender, EventArgs e ) {
            Vector<Integer> list = new Vector<int>();
            int c = AppManager.getVsqFile().Track.size();
            for ( int i = 1; i < c; i++ ) {
                if ( AppManager.getRenderRequired( i ) ) {
                    list.add( i );
                }
            }
            if ( list.size() <= 0 ) {
                return;
            }
            Render( list.toArray( new int[] { } ) );
        }

        private void menuTrackRenderer_DropDownOpening( object sender, EventArgs e ) {
            updateRendererMenu();
        }

        private void menuTrackOn_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Enable current track." );
        }

        private void menuTrackAdd_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Add new track." );
        }

        private void menuTrackCopy_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Copy current track." ); 
        }

        private void menuTrackChangeName_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Change track name." );
        }

        private void menuTrackDelete_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Delete current track." );
        }

        private void menuTrackRenderCurrent_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Render current track." );
        }

        private void menuTrackRenderAll_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Render all tracks." );
        }

        private void menuTrackOverlay_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Show background items." );
        }

        private void menuTrackRenderer_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Select voice synthesis engine." );
        }

        private void menuTrackRendererVOCALOID1_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "VOCALOID1" );
        }

        private void menuTrackRendererVOCALOID2_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "VOCALOID2" );
        }

        private void menuTrackRendererUtau_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "UTAU" );
        }

        private void menuTrackMasterTuning_MouseEnter( object sender, EventArgs e ) {
            statusLabel.Text = _( "Set global pitch shift." );
        }
        #endregion

        #region menuHidden*
        private void menuHiddenTrackNext_Click( object sender, EventArgs e ) {
            if ( AppManager.getSelected() == AppManager.getVsqFile().Track.size() - 1 ) {
                AppManager.setSelected( 1 );
            } else {
                AppManager.setSelected( AppManager.getSelected() + 1 );
            }
            refreshScreen();
        }

        private void menuHiddenTrackBack_Click( object sender, EventArgs e ) {
            if ( AppManager.getSelected() == 1 ) {
                AppManager.setSelected( AppManager.getVsqFile().Track.size() - 1 );
            } else {
                AppManager.setSelected( AppManager.getSelected() - 1 );
            }
            refreshScreen();
        }

        private void menuHiddenEditPaste_Click( object sender, EventArgs e ) {
            pasteEvent();
        }

        private void menuHiddenEditFlipToolPointerPencil_Click( object sender, EventArgs e ) {
            if ( AppManager.getSelectedTool() == EditTool.ARROW ) {
                AppManager.setSelectedTool( EditTool.PENCIL );
            } else {
                AppManager.setSelectedTool( EditTool.ARROW );
            }
            refreshScreen();
        }

        private void menuHiddenEditFlipToolPointerEraser_Click( object sender, EventArgs e ) {
            if ( AppManager.getSelectedTool() == EditTool.ARROW ) {
                AppManager.setSelectedTool( EditTool.ERASER );
            } else {
                AppManager.setSelectedTool( EditTool.ARROW );
            }
            refreshScreen();
        }
        
        private void menuHiddenEditLyric_Click( object sender, EventArgs e ) {
            if ( !m_input_textbox.Enabled && AppManager.getSelectedEventCount() > 0 ) {
                VsqEvent original = AppManager.getLastSelectedEvent().original;
                int clock = original.Clock;
                int note = original.ID.Note;
                Point pos = new Point( xCoordFromClocks( clock ), yCoordFromNote( note ) );
                if ( !AppManager.editorConfig.KeepLyricInputMode ) {
                    m_last_symbol_edit_mode = false;
                }
                ShowInputTextBox( original.ID.LyricHandle.L0.Phrase, original.ID.LyricHandle.L0.getPhoneticSymbol(), pos, m_last_symbol_edit_mode );
            } else if ( m_input_textbox.Enabled ) {
                TagLyricTextBox tltb = (TagLyricTextBox)m_input_textbox.Tag;
                if ( tltb.PhoneticSymbolEditMode ) {
                    FlipInputTextBoxMode();
                }
            }
        }
        #endregion

        #region cMenuTrackTab
        private void cMenuTrackTabCopy_Click( object sender, EventArgs e ) {
            CopyTrackCore();
        }

        private void cMenuTrackTabChangeName_Click( object sender, EventArgs e ) {
            ChangeTrackNameCore();
        }

        private void cMenuTrackTabTrackOn_Click( object sender, EventArgs e ) {
            cMenuTrackTabTrackOn.Checked = !cMenuTrackTabTrackOn.Checked;
            CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandTrackChangePlayMode( AppManager.getSelected(),
                                                                                                      cMenuTrackTabTrackOn.Checked ? 1 : -1 ) );
            AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
            Edited = true;
            refreshScreen();
        }

        private void cMenuTrackTabDelete_Click( object sender, EventArgs e ) {
            DeleteTrackCore();
        }

        private void cMenuTrackTabAdd_Click( object sender, EventArgs e ) {
            AddTrackCore();
        }

        private void cMenuTrackTab_Opening( object sender, CancelEventArgs e ) {
            UpdateTrackMenuStatus();
        }

        private void UpdateTrackMenuStatus() {
            cMenuTrackTabDelete.Enabled = menuTrackDelete.Enabled = (AppManager.getVsqFile().Track.size() >= 3);
            cMenuTrackTabAdd.Enabled = menuTrackAdd.Enabled = (AppManager.getVsqFile().Track.size() <= 16);
            cMenuTrackTabCopy.Enabled = menuTrackCopy.Enabled = (AppManager.getVsqFile().Track.size() <= 16);
            cMenuTrackTabTrackOn.Checked = menuTrackOn.Checked = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().PlayMode >= 0;
            if ( AppManager.getVsqFile().Track.size() > 2 ) {
                cMenuTrackTabOverlay.Enabled = menuTrackOverlay.Enabled = true;
                cMenuTrackTabOverlay.Checked = menuTrackOverlay.Checked = AppManager.isOverlay();
            } else {
                cMenuTrackTabOverlay.Enabled = menuTrackOverlay.Enabled = false;
                cMenuTrackTabOverlay.Checked = menuTrackOverlay.Checked = false;
            }
            cMenuTrackTabRenderCurrent.Enabled = menuTrackRenderCurrent.Enabled = !AppManager.isPlaying();
            cMenuTrackTabRenderAll.Enabled = menuTrackRenderAll.Enabled = !AppManager.isPlaying();
            cMenuTrackTabRendererVOCALOID1.Checked = menuTrackRendererVOCALOID1.Checked = false;
            cMenuTrackTabRendererVOCALOID2.Checked = menuTrackRendererVOCALOID2.Checked = false;
            cMenuTrackTabRendererUtau.Checked = menuTrackRendererUtau.Checked = false;
            cMenuTrackTabRendererStraight.Checked = menuTrackRendererStraight.Checked = false;

            String version = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version;
            if ( version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                cMenuTrackTabRendererVOCALOID1.Checked = menuTrackRendererVOCALOID1.Checked = true;
            } else if ( version.StartsWith( VSTiProxy.RENDERER_DSB3 ) ) {
                cMenuTrackTabRendererVOCALOID2.Checked = menuTrackRendererVOCALOID2.Checked = true;
            } else if ( version.StartsWith( VSTiProxy.RENDERER_UTU0 ) ) {
                cMenuTrackTabRendererUtau.Checked = menuTrackRendererUtau.Checked = true;
            } else if ( version.StartsWith( VSTiProxy.RENDERER_STR0 ) ) {
                cMenuTrackTabRendererStraight.Checked = menuTrackRendererStraight.Checked = true;
            }
        }

        private void cMenuTrackTabOverlay_Click( object sender, EventArgs e ) {
            AppManager.setOverlay( !AppManager.isOverlay() );
            refreshScreen();
        }

        private void cMenuTrackTabRenderCurrent_Click( object sender, EventArgs e ) {
            Render( new int[] { AppManager.getSelected() } );
        }

        private void cMenuTrackTabRenderer_DropDownOpening( object sender, EventArgs e ) {
            updateRendererMenu();
        }
        #endregion

        #region m_txtbox_track_name
        private void m_txtbox_track_name_KeyUp( object sender, KeyEventArgs e ) {
            if ( e.KeyCode == Keys.Enter ) {
                CadenciiCommand run = new CadenciiCommand(
                    VsqCommand.generateCommandTrackChangeName( AppManager.getSelected(), m_txtbox_track_name.Text ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                Edited = true;
                m_txtbox_track_name.Dispose();
                m_txtbox_track_name = null;
                refreshScreen();
            } else if ( e.KeyCode == Keys.Escape ) {
                m_txtbox_track_name.Dispose();
                m_txtbox_track_name = null;
            }
        }
        #endregion

        #region cMenuTrackSelector
        private void cMenuTrackSelector_Opening( object sender, CancelEventArgs e ) {
            updateCopyAndPasteButtonStatus();

            // 選択ツールの状態に合わせて表示を更新
            cMenuTrackSelectorPointer.CheckState = CheckState.Unchecked;
            cMenuTrackSelectorPencil.CheckState = CheckState.Unchecked;
            cMenuTrackSelectorLine.CheckState = CheckState.Unchecked;
            cMenuTrackSelectorEraser.CheckState = CheckState.Unchecked;
            switch ( AppManager.getSelectedTool() ) {
                case EditTool.ARROW:
                    cMenuTrackSelectorPointer.CheckState = CheckState.Indeterminate;
                    break;
                case EditTool.PENCIL:
                    cMenuTrackSelectorPencil.CheckState = CheckState.Indeterminate;
                    break;
                case EditTool.LINE:
                    cMenuTrackSelectorLine.CheckState = CheckState.Indeterminate;
                    break;
                case EditTool.ERASER:
                    cMenuTrackSelectorEraser.CheckState = CheckState.Indeterminate;
                    break;
            }
            cMenuTrackSelectorCurve.Checked = AppManager.isCurveMode();
        }

        private void cMenuTrackSelectorPointer_Click( object sender, EventArgs e ) {
            AppManager.setSelectedTool( EditTool.ARROW );
            refreshScreen();
        }

        private void cMenuTrackSelectorPencil_Click( object sender, EventArgs e ) {
            AppManager.setSelectedTool( EditTool.PENCIL );
            refreshScreen();
        }

        private void cMenuTrackSelectorLine_Click( object sender, EventArgs e ) {
            AppManager.setSelectedTool( EditTool.LINE );
        }

        private void cMenuTrackSelectorEraser_Click( object sender, EventArgs e ) {
            AppManager.setSelectedTool( EditTool.ERASER );
        }

        private void cMenuTrackSelectorCurve_Click( object sender, EventArgs e ) {
            AppManager.setCurveMode( !AppManager.isCurveMode() );
        }

        private void cMenuTrackSelectorSelectAll_Click( object sender, EventArgs e ) {
            selectAllEvent();
        }

        private void cMenuTrackSelectorCut_Click( object sender, EventArgs e ) {
            CutEvent();
        }
        
        private void cMenuTrackSelectorCopy_Click( object sender, EventArgs e ) {
            copyEvent();
        }

        private void cMenuTrackSelectorDelete_Click( object sender, EventArgs e ) {
            DeleteEvent();
        }

        private void cMenuTrackSelectorDeleteBezier_Click( object sender, EventArgs e ) {
            foreach ( SelectedBezierPoint sbp in AppManager.getSelectedBezierEnumerator() ) {
                int chain_id = sbp.chainID;
                int point_id = sbp.pointID;
                BezierChain chain = (BezierChain)AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).getBezierChain( trackSelector.SelectedCurve, chain_id ).Clone();
                int index = -1;
                for ( int i = 0; i < chain.points.size(); i++ ) {
                    if ( chain.points.get( i ).ID == point_id ) {
                        index = i;
                        break;
                    }
                }
                if ( index >= 0 ) {
                    chain.points.removeElementAt( index );
                    if ( chain.points.size() == 0 ) {
                        CadenciiCommand run = VsqFileEx.generateCommandDeleteBezierChain( AppManager.getSelected(),
                                                                                   trackSelector.SelectedCurve,
                                                                                   chain_id,
                                                                                   AppManager.editorConfig.ControlCurveResolution.Value );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    } else {
                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( AppManager.getSelected(),
                                                                                    trackSelector.SelectedCurve,
                                                                                    chain_id,
                                                                                    chain,
                                                                                    AppManager.editorConfig.ControlCurveResolution.Value );
                        AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    }
                    Edited = true;
                    refreshScreen();
                    break;
                }
            }
        }

        private void cMenuTrackSelectorPaste_Click( object sender, EventArgs e ) {
            pasteEvent();
        }
                
        private void cMenuTrackSelectorUndo_Click( object sender, EventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "cMenuTrackSelectorUndo_Click" );
#endif
            undo();
            refreshScreen();
        }

        private void cMenuTrackSelectorRedo_Click( object sender, EventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "cMenuTrackSelectorRedo_Click" );
#endif
            redo();
            refreshScreen();
        }
        #endregion
        
        private void pictureBox3_MouseDown( object sender, MouseEventArgs e ) {
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
        }

        private void pictureBox2_MouseDown( object sender, MouseEventArgs e ) {
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
        }
        
        private void menuStrip1_MouseDown( object sender, MouseEventArgs e ) {
            if ( m_txtbox_track_name != null ) {
                if ( !m_txtbox_track_name.IsDisposed ) {
                    m_txtbox_track_name.Dispose();
                }
                m_txtbox_track_name = null;
            }
        }

        private void timer_Tick( object sender, EventArgs e ) {
            float play_time = -1.0f;
            if ( AppManager.rendererAvailable ) {
                // レンダリング用VSTiが利用可能な状態でAppManager_PreviewStartedした場合
                if ( !AppManager.firstBufferWritten ) {
                    return;
                }
                play_time = VSTiProxy.getPlayTime();
            } else {
                play_time = (float)DateTime.Now.Subtract( AppManager.previewStartedTime ).TotalSeconds;
            }
            if ( AppManager.getEditMode() == EditMode.REALTIME ) {
                play_time = play_time * AppManager.editorConfig.RealtimeInputSpeed;
            }
            float now = (float)(play_time + m_direct_play_shift);
            
            if ( (play_time < 0.0 || m_preview_ending_time < now) && AppManager.getEditMode() != EditMode.REALTIME ) {
                AppManager.setPlaying( false );
                timer.Stop();
                if ( AppManager.startMarkerEnabled ) {
                    AppManager.setCurrentClock( AppManager.startMarker );
                }
                ensureCursorVisible();
                return;
            }
            int clock = (int)AppManager.getVsqFile().getClockFromSec( now );
            if ( clock > hScroll.Maximum ) {
                if ( AppManager.getEditMode() == EditMode.REALTIME ) {
                    hScroll.Maximum = clock + (int)((pictPianoRoll.Width - AppManager.KEY_LENGTH) / 2.0f / AppManager.scaleX);
                } else {
                    //AppManager.CurrentClock = 0;
                    //EnsureCursorVisible();
                    if ( !AppManager.isRepeatMode() ) {
                        timer.Stop();
                        AppManager.setPlaying( false );
                    }
                }
            } else if ( AppManager.endMarkerEnabled && clock > (int)AppManager.endMarker && AppManager.getEditMode() != EditMode.REALTIME ) {
                AppManager.setCurrentClock( (AppManager.startMarkerEnabled) ? AppManager.startMarker : 0 );
                ensureCursorVisible();
                AppManager.setPlaying( false );
                if ( AppManager.isRepeatMode() ) {
                    AppManager.setPlaying( true );
                } else {
                    timer.Stop();
                }
            } else {
                AppManager.setCurrentClock( (int)clock );
                if ( AppManager.autoScroll ) {
                    if ( AppManager.editorConfig.CursorFixed ) {
                        float f_draft = clock - (pictPianoRoll.Width / 2 + 34 - 70) / AppManager.scaleX;
                        if ( f_draft < 0f ) {
                            f_draft = 0;
                        }
                        int draft = (int)(f_draft);
                        if ( draft < hScroll.Minimum ) {
                            draft = hScroll.Minimum;
                        } else if ( hScroll.Maximum < draft ) {
                            draft = hScroll.Maximum;
                        }
                        if ( hScroll.Value != draft ) {
                            hScroll.Value = draft;
                        }
                    } else {
                        ensureCursorVisible();
                    }
                }
            }
            refreshScreen();
        }

        private void bgWorkScreen_DoWork( object sender, DoWorkEventArgs e ) {
            try {
                this.Invoke( new VoidDelegate( this.RefreshScreenCore ) );
            } catch {
            }
        }

        private void toolStripEdit_Move( object sender, EventArgs e ) {
            AppManager.editorConfig.ToolEditTool.Location = toolStripTool.Location;
        }

        private void toolStripEdit_ParentChanged( object sender, EventArgs e ) {
            if ( toolStripTool.Parent != null ) {
                if ( toolStripTool.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    AppManager.editorConfig.ToolEditTool.Parent = ToolStripLocation.ParentPanel.Top;
                } else {
                    AppManager.editorConfig.ToolEditTool.Parent = ToolStripLocation.ParentPanel.Bottom;
                }
            }
        }

        private void toolStripPosition_Move( object sender, EventArgs e ) {
            AppManager.editorConfig.ToolPositionLocation.Location = toolStripPosition.Location;
        }

        private void toolStripPosition_ParentChanged( object sender, EventArgs e ) {
            if ( toolStripPosition.Parent != null ) {
                if ( toolStripPosition.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    AppManager.editorConfig.ToolPositionLocation.Parent = ToolStripLocation.ParentPanel.Top;
                } else {
                    AppManager.editorConfig.ToolPositionLocation.Parent = ToolStripLocation.ParentPanel.Bottom;
                }
            }
        }

        private void toolStripMeasure_Move( object sender, EventArgs e ) {
            AppManager.editorConfig.ToolMeasureLocation.Location = toolStripMeasure.Location;
        }

        private void toolStripMeasure_ParentChanged( object sender, EventArgs e ) {
            if ( toolStripMeasure.Parent != null ) {
                if ( toolStripMeasure.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    AppManager.editorConfig.ToolMeasureLocation.Parent = ToolStripLocation.ParentPanel.Top;
                } else {
                    AppManager.editorConfig.ToolMeasureLocation.Parent = ToolStripLocation.ParentPanel.Bottom;
                }
            }
        }

        void toolStripFile_Move( object sender, EventArgs e ) {
            AppManager.editorConfig.ToolFileLocation.Location = toolStripFile.Location;
        }

        void toolStripFile_ParentChanged( object sender, EventArgs e ) {
            if ( toolStripFile.Parent != null ) {
                if ( toolStripFile.Parent.Equals( toolStripContainer.TopToolStripPanel ) ) {
                    AppManager.editorConfig.ToolFileLocation.Parent = ToolStripLocation.ParentPanel.Top;
                } else {
                    AppManager.editorConfig.ToolFileLocation.Parent = ToolStripLocation.ParentPanel.Bottom;
                }
            }
        }

        #region stripBtn*
        private void stripBtnGrid_CheckedChanged( object sender, EventArgs e ) {
            AppManager.setGridVisible( stripBtnGrid.Checked );
        }

        private void stripBtnArrow_Click( object sender, EventArgs e ) {
            AppManager.setSelectedTool( EditTool.ARROW );
        }

        private void stripBtnPencil_Click( object sender, EventArgs e ) {
            AppManager.setSelectedTool( EditTool.PENCIL );
        }

        private void stripBtnLine_Click( object sender, EventArgs e ) {
            AppManager.setSelectedTool( EditTool.LINE );
        }

        private void stripBtnEraser_Click( object sender, EventArgs e ) {
            AppManager.setSelectedTool( EditTool.ERASER );
        }

        private void stripBtnCurve_Click( object sender, EventArgs e ) {
            AppManager.setCurveMode( !AppManager.isCurveMode() );
        }

        private void stripBtnPlay_Click( object sender, EventArgs e ) {
            if ( !AppManager.isPlaying() ) {
                AppManager.setPlaying( true );
            }
            pictPianoRoll.Focus();
        }

        private void stripBtnScroll_Click( object sender, EventArgs e ) {
            stripBtnScroll.Checked = !stripBtnScroll.Checked;
            AppManager.autoScroll = stripBtnScroll.Checked;
            pictPianoRoll.Focus();
        }

        private void stripBtnLoop_Click( object sender, EventArgs e ) {
            stripBtnLoop.Checked = !stripBtnLoop.Checked;
            AppManager.setRepeatMode( stripBtnLoop.Checked );
            pictPianoRoll.Focus();
        }

        private void stripBtnStop_Click( object sender, EventArgs e ) {
            AppManager.setPlaying( false );
            timer.Stop();
            pictPianoRoll.Focus();
        }

        private void stripBtnStartMarker_Click( object sender, EventArgs e ) {
            stripBtnStartMarker.Checked = !stripBtnStartMarker.Checked;
            AppManager.startMarkerEnabled = stripBtnStartMarker.Checked;
            pictPianoRoll.Focus();
            refreshScreen();
        }

        private void stripBtnEndMarker_Click( object sender, EventArgs e ) {
            stripBtnEndMarker.Checked = !stripBtnEndMarker.Checked;
            AppManager.endMarkerEnabled = stripBtnEndMarker.Checked;
            pictPianoRoll.Focus();
            refreshScreen();
        }

        private void stripBtnMoveEnd_Click( object sender, EventArgs e ) {
            if ( AppManager.isPlaying() ) {
                AppManager.setPlaying( false );
            }
            AppManager.setCurrentClock( AppManager.getVsqFile().TotalClocks );
            ensureCursorVisible();
            refreshScreen();
        }

        private void stripBtnMoveTop_Click( object sender, EventArgs e ) {
            if ( AppManager.isPlaying() ) {
                AppManager.setPlaying( false );
            }
            AppManager.setCurrentClock( 0 );
            ensureCursorVisible();
            refreshScreen();
        }

        private void stripBtnRewind_Click( object sender, EventArgs e ) {
            Rewind();
        }

        private void stripBtnForward_Click( object sender, EventArgs e ) {
            Forward();
        }
        #endregion

        private void commonCaptureSpaceKeyDown( object sender, KeyEventArgs e ) {
            if ( (e.KeyCode & Keys.Space) == Keys.Space ) {
                m_spacekey_downed = true;
            }
        }

        private void commonCaptureSpaceKeyUp( object sender, KeyEventArgs e ) {
            if ( (e.KeyCode & Keys.Space) == Keys.Space ) {
                m_spacekey_downed = false;
            }
        }

        private void commonRendererVOCALOID1_Click( object sender, EventArgs e ) {
            String old = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version;
            if ( !old.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                VsqTrack item = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).Clone();
                Vector<VsqID> singers = new Vector<VsqID>();
                foreach ( SingerConfig sc in VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID1 ) ) {
                    singers.add( VocaloSysUtil.getSingerID( sc.VOICENAME, SynthesizerType.VOCALOID1 ) );
                }
                item.changeRenderer( "DSB202", singers );
                CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( AppManager.getSelected(),
                                                                             item,
                                                                             AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedStart( 0 );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedEnd( AppManager.getVsqFile().TotalClocks );
                cMenuTrackTabRendererVOCALOID1.Checked = true;
                cMenuTrackTabRendererVOCALOID2.Checked = false;
                cMenuTrackTabRendererUtau.Checked = false;
                cMenuTrackTabRendererStraight.Checked = false;
                menuTrackRendererVOCALOID1.Checked = true;
                menuTrackRendererVOCALOID2.Checked = false;
                menuTrackRendererUtau.Checked = false;
                menuTrackRendererStraight.Checked = false;
                Edited = true;
                refreshScreen();
            }
        }

        private void commonRendererVOCALOID2_Click( object sender, EventArgs e ) {
            String old = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version;
            if ( !old.StartsWith( VSTiProxy.RENDERER_DSB3 ) ) {
                VsqTrack item = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).Clone();
                Vector<VsqID> singers = new Vector<VsqID>();
                foreach ( SingerConfig sc in VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID2 ) ) {
                    singers.add( VocaloSysUtil.getSingerID( sc.VOICENAME, SynthesizerType.VOCALOID2 ) );
                }
                item.changeRenderer( "DSB301", singers );
                CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( AppManager.getSelected(),
                                                                             item,
                                                                             AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedStart( 0 );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedEnd( AppManager.getVsqFile().TotalClocks );
                cMenuTrackTabRendererVOCALOID1.Checked = false;
                cMenuTrackTabRendererVOCALOID2.Checked = true;
                cMenuTrackTabRendererUtau.Checked = false;
                cMenuTrackTabRendererStraight.Checked = false;
                menuTrackRendererVOCALOID1.Checked = false;
                menuTrackRendererVOCALOID2.Checked = true;
                menuTrackRendererUtau.Checked = false;
                menuTrackRendererStraight.Checked = false;
                Edited = true;
                refreshScreen();
            }
        }

        private void commonRendererUtau_Click( object sender, EventArgs e ) {
            String old = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version;
            if ( !old.StartsWith( VSTiProxy.RENDERER_UTU0 ) ) {
                VsqTrack item = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).Clone();
                Vector<SingerConfig> list = AppManager.editorConfig.UtauSingers;
                Vector<VsqID> singers = new Vector<VsqID>();
                for( Iterator itr = list.iterator(); itr.hasNext(); ){
                    SingerConfig sc = (SingerConfig)itr.next();
                    singers.add( AppManager.getSingerIDUtau( sc.VOICENAME ) );
                }
                item.changeRenderer( "UTU000", singers );
                CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( AppManager.getSelected(),
                                                                             item,
                                                                             AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedStart( 0 );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedEnd( AppManager.getVsqFile().TotalClocks );
                cMenuTrackTabRendererVOCALOID1.Checked = false;
                cMenuTrackTabRendererVOCALOID2.Checked = false;
                cMenuTrackTabRendererUtau.Checked = true;
                cMenuTrackTabRendererStraight.Checked = false;
                menuTrackRendererVOCALOID1.Checked = false;
                menuTrackRendererVOCALOID2.Checked = false;
                menuTrackRendererUtau.Checked = true;
                menuTrackRendererStraight.Checked = false;
                Edited = true;
                refreshScreen();
            }
        }

        private void commonRendererStraight_Click( object sender, EventArgs e ) {
            String old = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version;
            if ( !old.StartsWith( VSTiProxy.RENDERER_STR0 ) ) {
                VsqTrack item = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).Clone();
                Vector<SingerConfig> list = AppManager.editorConfig.UtauSingers;
                Vector<VsqID> singers = new Vector<VsqID>();
                for ( Iterator itr = list.iterator(); itr.hasNext(); ) {
                    SingerConfig sc = (SingerConfig)itr.next();
                    singers.add( AppManager.getSingerIDUtau( sc.VOICENAME ) );
                }
                item.changeRenderer( "STR000", singers );
                CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( AppManager.getSelected(),
                                                                             item,
                                                                             AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ) );
                AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedStart( 0 );
                AppManager.getVsqFile().Track.get( AppManager.getSelected() ).setEditedEnd( AppManager.getVsqFile().TotalClocks );
                cMenuTrackTabRendererVOCALOID1.Checked = false;
                cMenuTrackTabRendererVOCALOID2.Checked = false;
                cMenuTrackTabRendererUtau.Checked = false;
                cMenuTrackTabRendererStraight.Checked = true;
                menuTrackRendererVOCALOID1.Checked = false;
                menuTrackRendererVOCALOID2.Checked = false;
                menuTrackRendererUtau.Checked = false;
                menuTrackRendererStraight.Checked = true;
                Edited = true;
                refreshScreen();
            }
        }

        private void toolStripContainer_TopToolStripPanel_SizeChanged( object sender, EventArgs e ) {
            if ( this.WindowState == FormWindowState.Minimized ) {
                return;
            }
            Size minsize = GetWindowMinimumSize();
            int wid = this.Width;
            int hei = this.Height;
            boolean change_size_required = false;
            if ( minsize.Width > wid ) {
                wid = minsize.Width;
                change_size_required = true;
            }
            if ( minsize.Height > hei ) {
                hei = minsize.Height;
                change_size_required = true;
            }
            this.MinimumSize = GetWindowMinimumSize();
            if ( change_size_required ) {
                this.Size = new Size( wid, hei );
            }
        }

        private void stripDDBtnSpeed_DropDownOpening( object sender, EventArgs e ) {
            if ( AppManager.editorConfig.RealtimeInputSpeed == 1.0f ) {
                stripDDBtnSpeed100.Checked = true;
                stripDDBtnSpeed050.Checked = false;
                stripDDBtnSpeed033.Checked = false;
                stripDDBtnSpeedTextbox.Text = "100";
            } else if ( AppManager.editorConfig.RealtimeInputSpeed == 0.5f ) {
                stripDDBtnSpeed100.Checked = false;
                stripDDBtnSpeed050.Checked = true;
                stripDDBtnSpeed033.Checked = false;
                stripDDBtnSpeedTextbox.Text = "50";
            } else if ( AppManager.editorConfig.RealtimeInputSpeed == 1.0f / 3.0f ) {
                stripDDBtnSpeed100.Checked = false;
                stripDDBtnSpeed050.Checked = false;
                stripDDBtnSpeed033.Checked = true;
                stripDDBtnSpeedTextbox.Text = "33.333";
            } else {
                stripDDBtnSpeed100.Checked = false;
                stripDDBtnSpeed050.Checked = false;
                stripDDBtnSpeed033.Checked = false;
                stripDDBtnSpeedTextbox.Text = (AppManager.editorConfig.RealtimeInputSpeed * 100.0f).ToString();
            }
        }

        private void stripDDBtnSpeed100_Click( object sender, EventArgs e ) {
            changeRealtimeInputSpeed( 1.0f );
            AppManager.editorConfig.RealtimeInputSpeed = 1.0f;
            UpdateStripDDBtnSpeed();
        }

        private void stripDDBtnSpeed050_Click( object sender, EventArgs e ) {
            changeRealtimeInputSpeed( 0.5f );
            AppManager.editorConfig.RealtimeInputSpeed = 0.5f;
            UpdateStripDDBtnSpeed();
        }

        private void stripDDBtnSpeed033_Click( object sender, EventArgs e ) {
            changeRealtimeInputSpeed( 1.0f / 3.0f );
            AppManager.editorConfig.RealtimeInputSpeed = 1.0f / 3.0f;
            UpdateStripDDBtnSpeed();
        }

        private void stripDDBtnSpeedTextbox_KeyDown( object sender, KeyEventArgs e ) {
            if ( e.KeyCode == Keys.Enter ) {
                float v;
                try {
                    v = PortUtil.parseFloat( stripDDBtnSpeedTextbox.Text );
                    changeRealtimeInputSpeed( v / 100.0f );
                    AppManager.editorConfig.RealtimeInputSpeed = v / 100.0f;
                    stripDDBtnSpeed.HideDropDown();
                    UpdateStripDDBtnSpeed();
                } catch ( Exception ex ) {
                }
            }
        }

        public void changeRealtimeInputSpeed( float newv ) {
            float old = AppManager.editorConfig.RealtimeInputSpeed;
            DateTime now = DateTime.Now;
            float play_time = (float)now.Subtract( AppManager.previewStartedTime ).TotalSeconds * old / newv;
            int sec = (int)(Math.Floor( play_time ) + 0.1);
            int millisec = (int)((play_time - sec) * 1000);
            AppManager.previewStartedTime = now.Subtract( new TimeSpan( 0, 0, 0, sec, millisec ) );
            MidiPlayer.SetSpeed( newv, AppManager.previewStartedTime );
        }

        /// <summary>
        /// stripDDBtnSpeedの表示状態を更新します
        /// </summary>
        private void UpdateStripDDBtnSpeed() {
            stripDDBtnSpeed.Text = _( "Speed" ) + " " + (AppManager.editorConfig.RealtimeInputSpeed * 100) + "%";
        }

        private void menuSetting_DropDownOpening( object sender, EventArgs e ) {
            menuSettingMidi.Enabled = AppManager.getEditMode() != EditMode.REALTIME;
        }

        private void menuVisualProperty_Click( object sender, EventArgs e ) {
            if ( menuVisualProperty.Checked ) {
                if ( AppManager.editorConfig.PropertyWindowStatus.WindowState == FormWindowState.Minimized ) {
                    UpdatePropertyPanelState( PropertyPanelState.PanelState.Docked );
                } else {
                    UpdatePropertyPanelState( PropertyPanelState.PanelState.Window );
                }
            } else {
                UpdatePropertyPanelState( PropertyPanelState.PanelState.Hidden );
            }
        }

        private void menuSettingUtauVoiceDB_Click( object sender, EventArgs e ) {
            String edit_oto_ini = Path.Combine( Application.StartupPath, "EditOtoIni.exe" );
            if ( !PortUtil.isFileExists( edit_oto_ini ) ) {
                return;
            }

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Environment.GetEnvironmentVariable( "ComSpec" );
            psi.Arguments = "/c \"" + edit_oto_ini + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start( psi );
        }

        private void menuVisualOverview_CheckedChanged( object sender, EventArgs e ) {
            AppManager.editorConfig.OverviewEnabled = menuVisualOverview.Checked;
            UpdateLayout();
        }

        private void pictOverview_MouseMove( object sender, MouseEventArgs e ) {
            if ( m_overview_mouse_down_mode == OverviewMouseDownMode.LEFT ) {
                int draft = getOverviewStartToDrawX( e.X );
                if ( draft < 0 ) {
                    draft = 0;
                }
                AppManager.startToDrawX = draft;
                refreshScreen();
            } else if ( m_overview_mouse_down_mode == OverviewMouseDownMode.MIDDLE ) {
                int dx = e.X - m_overview_mouse_downed_locationx;
                int draft = m_overview_start_to_draw_clock_initial_value - (int)(dx / m_overview_px_per_clock);
                int clock = getOverviewClockFromXCoord( pictOverview.Width, draft );
                if ( AppManager.getVsqFile().TotalClocks < clock ) {
                    draft = AppManager.getVsqFile().TotalClocks - (int)(pictOverview.Width / m_overview_px_per_clock);
                }
                if ( draft < 0 ) {
                    draft = 0;
                }
                m_overview_start_to_draw_clock = draft;
                refreshScreen();
            }
        }

        private int getOverviewStartToDrawX( int mouse_x ) {
            float clock = mouse_x / m_overview_px_per_clock + m_overview_start_to_draw_clock;
            int clock_at_left = (int)(clock - (pictPianoRoll.Width - AppManager.KEY_LENGTH) / AppManager.scaleX / 2);
            return (int)(clock_at_left * AppManager.scaleX);
        }

        private void pictOverview_MouseDown( object sender, MouseEventArgs e ) {
            if ( e.Button == MouseButtons.Left ) {
#if DEBUG
                Console.WriteLine( "e.Clicks=" + e.Clicks );
#endif
                if ( e.Clicks == 1 ) {
                    m_overview_mouse_down_mode = OverviewMouseDownMode.LEFT;
                    int draft = getOverviewStartToDrawX( e.X );
                    if ( draft < 0 ) {
                        draft = 0;
                    }
                    AppManager.startToDrawX = draft;
                    refreshScreen();
                }
            } else if ( e.Button == MouseButtons.Middle ) {
                m_overview_mouse_down_mode = OverviewMouseDownMode.MIDDLE;
                m_overview_mouse_downed_locationx = e.X;
                m_overview_start_to_draw_clock_initial_value = m_overview_start_to_draw_clock;
            }
        }

        private void pictOverview_MouseUp( object sender, MouseEventArgs e ) {
            if ( m_overview_mouse_down_mode == OverviewMouseDownMode.LEFT ) {
                AppManager.startToDrawX = (int)(hScroll.Value * AppManager.scaleX);
            }
            m_overview_mouse_down_mode = OverviewMouseDownMode.NONE;
            refreshScreen();
        }

        private void pictOverview_Paint( object sender, PaintEventArgs e ) {
            Graphics g = e.Graphics;
            int count = 0;
            int sum = 0;
            int height = pictOverview.Height;
            using ( Pen pen = new Pen( s_note_fill, _OVERVIEW_DOT_DIAM ) ) {
                for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                    VsqEvent item = (VsqEvent)itr.next();
                    int x = getOverviewXCoordFromClock( item.Clock );
                    if ( x < 0 ) {
                        continue;
                    }
                    if ( pictOverview.Width < x ) {
                        break;
                    }
                    count++;
                    sum += item.ID.Note;
                    int y = height - (height / 2 + (int)((item.ID.Note - m_overview_average_note) * _OVERVIEW_DOT_DIAM));
                    int length = (int)(item.ID.Length * m_overview_px_per_clock);
                    if ( length < _OVERVIEW_DOT_DIAM ) {
                        length = _OVERVIEW_DOT_DIAM;
                    }
                    g.DrawLine( pen, new Point( x, y ), new Point( x + length, y ) );
                }
            }
            int current_start = clockFromXCoord( AppManager.KEY_LENGTH );
            int current_end = clockFromXCoord( pictPianoRoll.Width );
            int x_start = getOverviewXCoordFromClock( current_start );
            int x_end = getOverviewXCoordFromClock( current_end );

            // 小節ごとの線
            int clock_start = getOverviewClockFromXCoord( 0 );
            int clock_end = getOverviewClockFromXCoord( pictOverview.Width );
            int premeasure = AppManager.getVsqFile().getPreMeasure();
            g.ResetClip();
            using ( Pen pen_bold = new Pen( Color.FromArgb( 130, Color.Black ), 2 ) )
            using ( Pen pen = new Pen( Color.FromArgb( 130, Color.Black ) ) ) {
                for ( Iterator itr = AppManager.getVsqFile().getBarLineIterator( clock_end ); itr.hasNext(); ) {
                    VsqBarLineType bar = (VsqBarLineType)itr.next();
                    if ( bar.clock() < clock_start ) {
                        continue;
                    }
                    if ( clock_end < bar.clock() ) {
                        break;
                    }
                    if ( bar.isSeparator() ) {
                        int barcount = bar.getBarCount() - premeasure + 1;
                        int x = getOverviewXCoordFromClock( bar.clock() );
                        if ( (barcount % 5 == 0 && barcount > 0) || barcount == 1 ) {
                            g.DrawLine( pen_bold, new Point( x, 0 ), new Point( x, pictOverview.Height ) );
                            SizeF size = g.MeasureString( barcount + "", AppManager.baseFont9 );
                            g.DrawString( barcount + "", AppManager.baseFont9, Brushes.White, new PointF( x + 1, 1 ) );
                            g.SetClip( new RectangleF( x + 1, 1, size.Width, size.Height ), System.Drawing.Drawing2D.CombineMode.Exclude );
                        } else {
                            g.DrawLine( pen, new Point( x, 0 ), new Point( x, pictOverview.Height ) );
                        }
                    }
                }
            }
            g.ResetClip();

            // 現在の表示範囲
            Rectangle rc = new Rectangle( x_start, 0, x_end - x_start, height - 1 );
            g.FillRectangle( new SolidBrush( Color.FromArgb( 50, Color.White ) ), rc );
            g.DrawRectangle( new Pen( AppManager.getHilightColor() ), rc );
            if ( count > 0 ) {
                m_overview_average_note = sum / (float)count;
            }

            // ソングポジション
            int px_current_clock = (int)((AppManager.getCurrentClock() - m_overview_start_to_draw_clock) * m_overview_px_per_clock);
            g.DrawLine( new Pen( Color.White, 2 ), new Point( px_current_clock, 0 ), new Point( px_current_clock, pictOverview.Height ) );
        }

        private int getOverviewXCoordFromClock( int clock ) {
            return (int)((clock - m_overview_start_to_draw_clock) * m_overview_px_per_clock);
        }

        private int getOverviewClockFromXCoord( int x, int start_to_draw_clock ) {
            return (int)(x / m_overview_px_per_clock) + start_to_draw_clock;
        }

        private int getOverviewClockFromXCoord( int x ){
            return getOverviewClockFromXCoord( x, m_overview_start_to_draw_clock );
        }

        private void pictOverview_MouseDoubleClick( object sender, MouseEventArgs e ) {
            m_overview_mouse_down_mode = OverviewMouseDownMode.NONE;
            int draft_stdx = getOverviewStartToDrawX( e.X );
            int draft = (int)(draft_stdx / AppManager.scaleX);
            if ( draft < hScroll.Minimum ) {
                draft = hScroll.Minimum;
            } else if ( hScroll.Maximum < draft ) {
                draft = hScroll.Maximum;
            }
            hScroll.Value = draft;
            refreshScreen();
        }

        private void btnLeft_MouseDown( object sender, MouseEventArgs e ) {
            m_overview_btn_downed = DateTime.Now;
            m_overview_start_to_draw_clock_initial_value = m_overview_start_to_draw_clock;
            if ( m_overview_update_thread != null ) {
                try {
                    m_overview_update_thread.Abort();
                    while ( m_overview_update_thread.IsAlive ) {
                        Application.DoEvents();
                    }
                } catch { }
                m_overview_update_thread = null;
            }
            m_overview_direction = -1;
            m_overview_update_thread = new Thread( new ThreadStart( updateOverview ) );
            m_overview_update_thread.Start();
        }

        private void btnLeft_MouseUp( object sender, MouseEventArgs e ) {
            if ( m_overview_update_thread != null ) {
                try {
                    m_overview_update_thread.Abort();
                    while ( m_overview_update_thread.IsAlive ) {
                        Application.DoEvents();
                    }
                } catch { }
                m_overview_update_thread = null;
            }
        }

        private void btnRight_MouseDown( object sender, MouseEventArgs e ) {
            m_overview_btn_downed = DateTime.Now;
            m_overview_start_to_draw_clock_initial_value = m_overview_start_to_draw_clock;
            if ( m_overview_update_thread != null ) {
                try {
                    while ( m_overview_update_thread.IsAlive ) {
                        Application.DoEvents();
                    }
                } catch { }
                m_overview_update_thread = null;
            }
            m_overview_direction = 1;
            m_overview_update_thread = new Thread( new ThreadStart( updateOverview ) );
            m_overview_update_thread.Start();
        }

        private void btnRight_MouseUp( object sender, MouseEventArgs e ) {
            if ( m_overview_update_thread != null ) {
                try {
                    m_overview_update_thread.Abort();
                    while ( m_overview_update_thread != null && m_overview_update_thread.IsAlive ) {
                        Application.DoEvents();
                    }
                } catch { }
                m_overview_update_thread = null;
            }
        }

        private void updateOverview() {
            while ( true ) {
#if DEBUG
                Console.WriteLine( "updateOverview" );
#endif
                Thread.Sleep( 100 );
                double dt = DateTime.Now.Subtract( m_overview_btn_downed ).TotalSeconds;
                int draft = (int)(m_overview_start_to_draw_clock_initial_value + m_overview_direction * dt * _OVERVIEW_SCROLL_SPEED / m_overview_px_per_clock);
                int clock = getOverviewClockFromXCoord( pictOverview.Width, draft );
                if ( AppManager.getVsqFile().TotalClocks < clock ) {
                    draft = AppManager.getVsqFile().TotalClocks - (int)(pictOverview.Width / m_overview_px_per_clock);
                }
                if ( draft < 0 ) {
                    draft = 0;
                }
                m_overview_start_to_draw_clock = draft;
                this.Invoke( new VoidDelegate( pictOverview.Invalidate ) );
            }
        }

        private void btnMooz_Click( object sender, EventArgs e ) {
            int draft = m_overview_scale_count - 1;
            if ( draft < _OVERVIEW_SCALE_COUNT_MIN ) {
                draft = _OVERVIEW_SCALE_COUNT_MIN;
            }
            m_overview_scale_count = draft;
            m_overview_px_per_clock = getOverviewScaleX( m_overview_scale_count );
            AppManager.editorConfig.OverviewScaleCount = m_overview_scale_count;
            refreshScreen();
        }

        private void btnZoom_Click( object sender, EventArgs e ) {
            int draft = m_overview_scale_count + 1;
            if ( _OVERVIEW_SCALE_COUNT_MAX < draft ) {
                draft = _OVERVIEW_SCALE_COUNT_MAX;
            }
            m_overview_scale_count = draft;
            m_overview_px_per_clock = getOverviewScaleX( m_overview_scale_count );
            AppManager.editorConfig.OverviewScaleCount = m_overview_scale_count;
            refreshScreen();
        }

        private float getOverviewScaleX( int scale_count ) {
            return (float)Math.Pow( 10.0, 0.2 * scale_count - 3.0 );
        }
    }

}
