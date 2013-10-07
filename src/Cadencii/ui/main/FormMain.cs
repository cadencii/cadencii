/*
 * FormMain.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
//#define USE_BGWORK_SCREEN
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Text;
using System.Threading;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.ComponentModel;
using cadencii.apputil;
using cadencii.java.awt;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.javax.sound.midi;
using cadencii.media;
using cadencii.vsq;
using cadencii.vsq.io;
using cadencii.windows.forms;
using cadencii.xml;
using cadencii.utau;
using cadencii.ui;



namespace cadencii
{

    /// <summary>
    /// エディタのメイン画面クラス
    /// </summary>
    public partial class FormMain : Form, FormMainUi, PropertyWindowListener
    {
        /// <summary>
        /// 特殊なキーの組み合わせのショートカットと、メニューアイテムとの紐付けを保持するクラスです。
        /// </summary>
        private class SpecialShortcutHolder
        {
            /// <summary>
            /// ショートカットキーを表すKeyStrokeクラスのインスタンス
            /// </summary>
            public Keys shortcut;
            /// <summary>
            /// ショートカットキーとの紐付けを行う相手先のメニューアイテム
            /// </summary>
            public ToolStripMenuItem menu;

            /// <summary>
            /// ショートカットキーとメニューアイテムを指定したコンストラクタ
            /// </summary>
            /// <param name="shortcut">ショートカットキー</param>
            /// <param name="menu">ショートカットキーとの紐付けを行うメニューアイテム</param>
            public SpecialShortcutHolder(Keys shortcut, ToolStripMenuItem menu)
            {
                this.shortcut = shortcut;
                this.menu = menu;
            }
        }

        /// <summary>
        /// refreshScreenを呼び出す時に使うデリゲート
        /// </summary>
        /// <param name="value"></param>
        delegate void DelegateRefreshScreen(bool value);

        #region static readonly field
        /// <summary>
        /// ピアノロールでの，音符の塗りつぶし色
        /// </summary>
        public static readonly Color mColorNoteFill = new Color(181, 220, 86);
        private readonly Color mColorR105G105B105 = new Color(105, 105, 105);
        private readonly Color mColorR187G187B255 = new Color(187, 187, 255);
        private readonly Color mColorR007G007B151 = new Color(7, 7, 151);
        private readonly Color mColorR065G065B065 = new Color(65, 65, 65);
        private readonly Color mColorTextboxBackcolor = new Color(128, 128, 128);
        private readonly Color mColorR214G214B214 = new Color(214, 214, 214);
        private readonly AuthorListEntry[] _CREDIT = new AuthorListEntry[]{
            new AuthorListEntry( "is developped by:", 2 ),
            new AuthorListEntry( "kbinani", "@kbinani" ),
            new AuthorListEntry( "修羅場P", "@shurabaP" ),
            new AuthorListEntry( "もみじぱん", "@momijipan" ),
            new AuthorListEntry( "結晶", "@gondam" ),
            new AuthorListEntry( "" ),
            new AuthorListEntry(),
            new AuthorListEntry(),
            new AuthorListEntry( "Special Thanks to", 3 ),
            new AuthorListEntry(),
            new AuthorListEntry( "tool icons designer:", 2 ),
            new AuthorListEntry( "Yusuke KAMIYAMANE", "@ykamiyamane" ),
            new AuthorListEntry(),
            new AuthorListEntry( "developper of WORLD:", 2 ),
            new AuthorListEntry( "Masanori MORISE", "@m_morise" ),
            new AuthorListEntry(),
            new AuthorListEntry( "developper of v.Connect-STAND:", 2 ),
            new AuthorListEntry( "修羅場P", "@shurabaP" ),
            new AuthorListEntry(),
            new AuthorListEntry( "developper of UTAU:", 2 ),
            new AuthorListEntry( "飴屋/菖蒲", "@ameyaP_" ),
            new AuthorListEntry(),
            new AuthorListEntry( "developper of RebarDotNet:", 2 ),
            new AuthorListEntry( "Anthony Baraff" ),
            new AuthorListEntry(),
            new AuthorListEntry( "promoter:", 2 ),
            new AuthorListEntry( "zhuo", "@zhuop" ),
            new AuthorListEntry(),
            new AuthorListEntry( "library tester:", 2 ),
            new AuthorListEntry( "evm" ),
            new AuthorListEntry( "そろそろP" ),
            new AuthorListEntry( "めがね１１０" ),
            new AuthorListEntry( "上総" ),
            new AuthorListEntry( "NOIKE", "@knoike" ),
            new AuthorListEntry( "逃亡者" ),
            new AuthorListEntry(),
            new AuthorListEntry( "translator:", 2 ),
            new AuthorListEntry( "Eji (zh-TW translation)", "@ejiwarp" ),
            new AuthorListEntry( "kankan (zh-TW translation)" ),
            new AuthorListEntry( "yxmline (zh-CN translation)" ),
            new AuthorListEntry( "BubblyYoru (en translation)", "@BubblyYoru" ),
            new AuthorListEntry( "BeForU (kr translation)", "@BeForU" ),
            new AuthorListEntry(),
            new AuthorListEntry(),
            new AuthorListEntry( "Thanks to", 3 ),
            new AuthorListEntry(),
            new AuthorListEntry( "ないしょの人" ),
            new AuthorListEntry( "naquadah" ),
            new AuthorListEntry( "1zo" ),
            new AuthorListEntry( "Amby" ),
            new AuthorListEntry( "ケロッグ" ),
            new AuthorListEntry( "beginner" ),
            new AuthorListEntry( "b2ox", "@b2ox" ),
            new AuthorListEntry( "麻太郎" ),
            new AuthorListEntry( "PEX", "@pex_zeo" ),
            new AuthorListEntry( "やなぎがうら" ),
            new AuthorListEntry( "cocoonP", "@cocoonP" ),
            new AuthorListEntry( "かつ" ),
            new AuthorListEntry( "ちゃそ", "@marimarikerori" ),
            new AuthorListEntry( "ちょむ" ),
            new AuthorListEntry( "whimsoft" ),
            new AuthorListEntry( "kitiketao", "@okoktaokokta" ),
            new AuthorListEntry( "カプチ２" ),
            new AuthorListEntry( "あにぃ" ),
            new AuthorListEntry( "tomo" ),
            new AuthorListEntry( "ナウ□マP", "@now_romaP" ),
            new AuthorListEntry( "内藤　魅亜", "@mianaito" ),
            new AuthorListEntry( "空茶", "@maizeziam" ),
            new AuthorListEntry( "いぬくま" ),
            new AuthorListEntry( "shu-t", "@shu_sonicwave" ),
            new AuthorListEntry( "さささ", "@sasasa3396" ),
            new AuthorListEntry( "あろも～ら", "@aromora" ),
            new AuthorListEntry( "空耳P", "@soramiku" ),
            new AuthorListEntry( "kotoi" ),
            new AuthorListEntry( "げっぺータロー", "@geppeitaro" ),
            new AuthorListEntry( "みけCAT", "@mikecat_mixc" ),
            new AuthorListEntry( "ぎんじ" ),
            new AuthorListEntry( "BeForU", "@BeForU" ),
            new AuthorListEntry( "all members of Cadencii bbs", 2 ),
            new AuthorListEntry(),
            new AuthorListEntry( "     ... and you !", 3 ),
        };
        #endregion

        #region constants and internal enums
        /// <summary>
        /// カーブエディタ画面の編集モード
        /// </summary>
        enum CurveEditMode
        {
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

        enum ExtDragXMode
        {
            RIGHT,
            LEFT,
            NONE,
        }

        enum ExtDragYMode
        {
            UP,
            DOWN,
            NONE,
        }

        enum GameControlMode
        {
            DISABLED,
            NORMAL,
            KEYBOARD,
            CURSOR,
        }

        enum PositionIndicatorMouseDownMode
        {
            NONE,
            MARK_START,
            MARK_END,
            TEMPO,
            TIMESIG,
        }

        /// <summary>
        /// スクロールバーの最小サイズ(ピクセル)
        /// </summary>
        public const int MIN_BAR_ACTUAL_LENGTH = 17;
        /// <summary>
        /// エントリの端を移動する時の、ハンドル許容範囲の幅
        /// </summary>
        public const int _EDIT_HANDLE_WIDTH = 7;
        public const int _TOOL_BAR_HEIGHT = 46;
        /// <summary>
        /// 単音プレビュー時に、wave生成完了を待つ最大の秒数
        /// </summary>
        public const double _WAIT_LIMIT = 5.0;
        public const string _APP_NAME = "Cadencii";
        /// <summary>
        /// 表情線の先頭部分のピクセル幅
        /// </summary>
        public const int _PX_ACCENT_HEADER = 21;
        public const string RECENT_UPDATE_INFO_URL = "http://www.cadencii.info/recent.php";
        /// <summary>
        /// splitContainer2.Panel2の最小サイズ
        /// </summary>
        public const int _SPL2_PANEL2_MIN_HEIGHT = 25;
        /// <summary>
        /// splitContainer*で使用するSplitterWidthプロパティの値
        /// </summary>
        public const int _SPL_SPLITTER_WIDTH = 4;
        const int _PICT_POSITION_INDICATOR_HEIGHT = 48;
        const int _SCROLL_WIDTH = 16;
        /// <summary>
        /// Overviewペインの高さ
        /// </summary>
        public const int _OVERVIEW_HEIGHT = 50;
        /// <summary>
        /// splitContainerPropertyの最小幅
        /// </summary>
        const int _PROPERTY_DOCK_MIN_WIDTH = 50;
        /// <summary>
        /// WAVE再生時のバッファーサイズの最大値
        /// </summary>
        const int MAX_WAVE_MSEC_RESOLUTION = 1000;
        /// <summary>
        /// WAVE再生時のバッファーサイズの最小値
        /// </summary>
        const int MIN_WAVE_MSEC_RESOLUTION = 100;
        #endregion

        #region static field
        /// <summary>
        /// refreshScreenが呼ばれている最中かどうか
        /// </summary>
        private static bool mIsRefreshing = false;
        /// <summary>
        /// CTRLキー。MacOSXの場合はMenu
        /// </summary>
        public Keys s_modifier_key = Keys.Control;
        #endregion

        #region fields
        /// <summary>
        /// コントローラ
        /// </summary>
        private FormMainController controller = null;
        public VersionInfo mVersionInfo = null;
        public System.Windows.Forms.Cursor HAND;
        /// <summary>
        /// ボタンがDownされた位置。(座標はpictureBox基準)
        /// </summary>
        public Point mButtonInitial = new Point();
        /// <summary>
        /// 真ん中ボタンがダウンされたときのvscrollのvalue値
        /// </summary>
        public int mMiddleButtonVScroll;
        /// <summary>
        /// 真ん中ボタンがダウンされたときのhscrollのvalue値
        /// </summary>
        public int mMiddleButtonHScroll;
        public bool mEdited = false;
        /// <summary>
        /// 最後にメイン画面が更新された時刻(秒単位)
        /// </summary>
        private double mLastScreenRefreshedSec;
        /// <summary>
        /// カーブエディタの編集モード
        /// </summary>
        private CurveEditMode mEditCurveMode = CurveEditMode.NONE;
        /// <summary>
        /// ピアノロールの右クリックが表示される直前のマウスの位置
        /// </summary>
        public Point mContextMenuOpenedPosition = new Point();
        /// <summary>
        /// ピアノロールの画面外へのドラッグ時、前回自動スクロール操作を行った時刻
        /// </summary>
        public double mTimerDragLastIgnitted;
        /// <summary>
        /// 画面外への自動スクロールモード
        /// </summary>
        private ExtDragXMode mExtDragX = ExtDragXMode.NONE;
        private ExtDragYMode mExtDragY = ExtDragYMode.NONE;
        /// <summary>
        /// EditMode=MoveEntryで，移動を開始する直前のマウスの仮想スクリーン上の位置
        /// </summary>
        public Point mMouseMoveInit = new Point();
        /// <summary>
        /// EditMode=MoveEntryで，移動を開始する直前のマウスの位置と，音符の先頭との距離(ピクセル)
        /// </summary>
        public int mMouseMoveOffset;
        /// <summary>
        /// マウスが降りているかどうかを表すフラグ．AppManager.isPointerDownedとは別なので注意
        /// </summary>
        public bool mMouseDowned = false;
        public int mTempoDraggingDeltaClock = 0;
        public int mTimesigDraggingDeltaClock = 0;
        public bool mMouseDownedTrackSelector = false;
        private ExtDragXMode mExtDragXTrackSelector = ExtDragXMode.NONE;
        public bool mMouseMoved = false;
#if ENABLE_MOUSEHOVER
        /// <summary>
        /// マウスホバーを発生させるスレッド
        /// </summary>
        public Thread mMouseHoverThread = null;
#endif
        public bool mLastIsImeModeOn = true;
        public bool mLastSymbolEditMode = false;
        /// <summary>
        /// 鉛筆のモード
        /// </summary>
        public PencilMode mPencilMode = new PencilMode();
        /// <summary>
        /// ビブラート範囲を編集中の音符のInternalID
        /// </summary>
        public int mVibratoEditingId = -1;
        /// <summary>
        /// このフォームがアクティブ化されているかどうか
        /// </summary>
        public bool mFormActivated = true;
        private GameControlMode mGameMode = GameControlMode.DISABLED;
        public System.Windows.Forms.Timer mTimer;
        public bool mLastPovR = false;
        public bool mLastPovL = false;
        public bool mLastPovU = false;
        public bool mLastPovD = false;
        public bool mLastBtnX = false;
        public bool mLastBtnO = false;
        public bool mLastBtnRe = false;
        public bool mLastBtnTr = false;
        public bool mLastBtnSelect = false;
        /// <summary>
        /// 前回ゲームコントローラのイベントを処理した時刻
        /// </summary>
        public double mLastEventProcessed;
        public bool mSpacekeyDowned = false;
#if ENABLE_MIDI
        public MidiInDevice mMidiIn = null;
#endif
#if ENABLE_MTC
        public MidiInDevice m_midi_in_mtc = null;
#endif
        public FormMidiImExport mDialogMidiImportAndExport = null;
        public SortedDictionary<EditTool, java.awt.Cursor> mCursor = new SortedDictionary<EditTool, java.awt.Cursor>();
        private Preference mDialogPreference;
#if ENABLE_PROPERTY
        public PropertyPanelContainer mPropertyPanelContainer;
#endif
#if ENABLE_SCRIPT
        public List<System.Windows.Forms.ToolBarButton> mPaletteTools = new List<System.Windows.Forms.ToolBarButton>();
#endif

        /// <summary>
        /// PositionIndicatorのマウスモード
        /// </summary>
        private PositionIndicatorMouseDownMode mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
        /// <summary>
        /// AppManager.keyWidthを調節するモードに入ったかどうか
        /// </summary>
        public bool mKeyLengthSplitterMouseDowned = false;
        /// <summary>
        /// AppManager.keyWidthを調節するモードに入る直前での、マウスのスクリーン座標
        /// </summary>
        public Point mKeyLengthSplitterInitialMouse = new Point();
        /// <summary>
        /// AppManager.keyWidthを調節するモードに入る直前での、keyWidthの値
        /// </summary>
        public int mKeyLengthInitValue = 68;
        /// <summary>
        /// AppManager.keyWidthを調節するモードに入る直前での、trackSelectorのgetRowsPerColumn()の値
        /// </summary>
        public int mKeyLengthTrackSelectorRowsPerColumn = 1;
        /// <summary>
        /// AppManager.keyWidthを調節するモードに入る直前での、splitContainer1のSplitterLocationの値
        /// </summary>
        public int mKeyLengthSplitterDistance = 0;
        public OpenFileDialog openXmlVsqDialog;
        public SaveFileDialog saveXmlVsqDialog;
        public OpenFileDialog openUstDialog;
        public OpenFileDialog openMidiDialog;
        public SaveFileDialog saveMidiDialog;
        public OpenFileDialog openWaveDialog;
        public System.Windows.Forms.Timer timer;
        public System.ComponentModel.BackgroundWorker bgWorkScreen;
        /// <summary>
        /// アイコンパレットのドラッグ＆ドロップ処理中，一度でもpictPianoRoll内にマウスが入ったかどうか
        /// </summary>
        private bool mIconPaletteOnceDragEntered = false;
        private byte mMtcFrameLsb;
        private byte mMtcFrameMsb;
        private byte mMtcSecLsb;
        private byte mMtcSecMsb;
        private byte mMtcMinLsb;
        private byte mMtcMinMsb;
        private byte mMtcHourLsb;
        private byte mMtcHourMsb;
        /// <summary>
        /// MTCを最後に受信した時刻
        /// </summary>
        private double mMtcLastReceived = 0.0;
        /// <summary>
        /// 特殊な取り扱いが必要なショートカットのキー列と、対応するメニューアイテムを保存しておくリスト。
        /// </summary>
        private List<SpecialShortcutHolder> mSpecialShortcutHolders = new List<SpecialShortcutHolder>();
        /// <summary>
        /// 歌詞流し込み用のダイアログ
        /// </summary>
        private FormImportLyric mDialogImportLyric = null;
        /// <summary>
        /// デフォルトのストローク
        /// </summary>
        private BasicStroke mStrokeDefault = null;
        /// <summary>
        /// 描画幅2pxのストローク
        /// </summary>
        private BasicStroke mStroke2px = null;
        /// <summary>
        /// pictureBox2の描画ループで使うグラフィックス
        /// </summary>
        private Graphics2D mGraphicsPictureBox2 = null;
        /// <summary>
        /// ピアノロールの縦方向の拡大率を変更するパネル上でのマウスの状態。
        /// 0がデフォルト、&gt;0は+ボタンにマウスが降りた状態、&lt;0は-ボタンにマウスが降りた状態
        /// </summary>
        private int mPianoRollScaleYMouseStatus = 0;
        /// <summary>
        /// 再生中にソングポジションが前進だけしてほしいので，逆行を防ぐために前回のソングポジションを覚えておく
        /// </summary>
        private int mLastClock = 0;
        /// <summary>
        /// PositionIndicatorに表示しているポップアップのクロック位置
        /// </summary>
        private int mPositionIndicatorPopupShownClock;
        /// <summary>
        /// 合成器の種類のメニュー項目を管理するハンドラをまとめたリスト
        /// </summary>
        private List<RendererMenuHandler> renderer_menu_handler_;
        private FormWindowState mWindowState = FormWindowState.Normal;
#if MONITOR_FPS
        /// <summary>
        /// パフォーマンスカウンタ
        /// </summary>
        private double[] mFpsDrawTime = new double[128];
        private int mFpsDrawTimeIndex = 0;
        /// <summary>
        /// パフォーマンスカウンタから算出される画面の更新速度
        /// </summary>
        private float mFps = 0f;
        private double[] mFpsDrawTime2 = new double[128];
        private float mFps2 = 0f;
#endif
        #endregion

        #region constructor
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="file">最初に開くxvsq，vsqファイルのパス</param>
        public FormMain(FormMainController controller, string file)
        {
            this.controller = controller;
            this.controller.setupUi(this);

            // 言語設定を反映させる
            Messaging.setLanguage(AppManager.editorConfig.Language);

#if ENABLE_PROPERTY
            AppManager.propertyPanel = new PropertyPanel();
            AppManager.propertyWindow = new FormNotePropertyController(this);
            AppManager.propertyWindow.getUi().addComponent(AppManager.propertyPanel);
#endif

#if DEBUG
            AppManager.debugWriteLine("FormMain..ctor()");
#endif
            AppManager.baseFont10Bold = new Font(AppManager.editorConfig.BaseFontName, java.awt.Font.BOLD, AppManager.FONT_SIZE10);
            AppManager.baseFont8 = new Font(AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, AppManager.FONT_SIZE8);
            AppManager.baseFont10 = new Font(AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, AppManager.FONT_SIZE10);
            AppManager.baseFont9 = new Font(AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, AppManager.FONT_SIZE9);
            AppManager.baseFont50Bold = new Font(AppManager.editorConfig.BaseFontName, java.awt.Font.BOLD, AppManager.FONT_SIZE50);

            s_modifier_key = Keys.Control;
            VsqFileEx tvsq =
                new VsqFileEx(
                    AppManager.editorConfig.DefaultSingerName,
                    1,
                    4,
                    4,
                    500000);
            RendererKind kind = AppManager.editorConfig.DefaultSynthesizer;
            string renderer = kind.getVersionString();
            List<VsqID> singers = AppManager.getSingerListFromRendererKind(kind);
            tvsq.Track[1].changeRenderer(renderer, singers);
            AppManager.setVsqFile(tvsq);

            trackSelector = new TrackSelector(this); // initializeで引数なしのコンストラクタが呼ばれるのを予防
            //TODO: javaのwaveViewはどこで作られるんだっけ？
            waveView = new WaveView();
            //TODO: これはひどい
            panelWaveformZoom = (WaveformZoomUiImpl)(new WaveformZoomController(this, waveView)).getUi();

            InitializeComponent();
            timer = new System.Windows.Forms.Timer(this.components);

            panelOverview.setMainForm(this);
            pictPianoRoll.setMainForm(this);
            bgWorkScreen = new System.ComponentModel.BackgroundWorker();
            initializeRendererMenuHandler();

            this.panelWaveformZoom.Controls.Add(this.waveView);
            this.waveView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.waveView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.waveView.Location = new System.Drawing.Point(66, 0);
            this.waveView.Margin = new System.Windows.Forms.Padding(0);
            this.waveView.Name = "waveView";
            this.waveView.Size = new System.Drawing.Size(355, 59);
            this.waveView.TabIndex = 17;
            openXmlVsqDialog = new OpenFileDialog();
            openXmlVsqDialog.Filter = string.Join("|", new[] { "VSQ Format(*.vsq)|*.vsq", "XML-VSQ Format(*.xvsq)|*.xvsq" });

            saveXmlVsqDialog = new SaveFileDialog();
            saveXmlVsqDialog.Filter = string.Join("|", new[] { "VSQ Format(*.vsq)|*.vsq", "XML-VSQ Format(*.xvsq)|*.xvsq", "All files(*.*)|*.*" });

            openUstDialog = new OpenFileDialog();
            openUstDialog.Filter = string.Join("|", new[] { "UTAU Project File(*.ust)|*.ust", "All Files(*.*)|*.*" });

            openMidiDialog = new OpenFileDialog();
            saveMidiDialog = new SaveFileDialog();
            openWaveDialog = new OpenFileDialog();

            /*mOverviewScaleCount = AppManager.editorConfig.OverviewScaleCount;
            mOverviewPixelPerClock = getOverviewScaleX( mOverviewScaleCount );*/

            menuVisualOverview.Checked = AppManager.editorConfig.OverviewEnabled;
#if ENABLE_PROPERTY
            mPropertyPanelContainer = new PropertyPanelContainer();
#endif

            registerEventHandlers();
            setResources();

#if !ENABLE_SCRIPT
            menuSettingPaletteTool.setVisible( false );
            menuScript.setVisible( false );
#endif
            trackSelector.updateVisibleCurves();
            trackSelector.setBackground(new Color(108, 108, 108));
            trackSelector.setCurveVisible(true);
            trackSelector.setSelectedCurve(CurveType.VEL);
            trackSelector.setLocation(new Point(0, 242));
            trackSelector.Margin = new System.Windows.Forms.Padding(0);
            trackSelector.Name = "trackSelector";
            trackSelector.setSize(446, 250);
            trackSelector.TabIndex = 0;
            trackSelector.MouseClick += new MouseEventHandler(trackSelector_MouseClick);
            trackSelector.MouseUp += new MouseEventHandler(trackSelector_MouseUp);
            trackSelector.MouseDown += new MouseEventHandler(trackSelector_MouseDown);
            trackSelector.MouseMove += new MouseEventHandler(trackSelector_MouseMove);
            trackSelector.KeyDown += new KeyEventHandler(handleSpaceKeyDown);
            trackSelector.KeyUp += new KeyEventHandler(handleSpaceKeyUp);
            trackSelector.PreviewKeyDown += new PreviewKeyDownEventHandler(trackSelector_PreviewKeyDown);
            trackSelector.SelectedTrackChanged += new SelectedTrackChangedEventHandler(trackSelector_SelectedTrackChanged);
            trackSelector.SelectedCurveChanged += new SelectedCurveChangedEventHandler(trackSelector_SelectedCurveChanged);
            trackSelector.RenderRequired += new RenderRequiredEventHandler(trackSelector_RenderRequired);
            trackSelector.PreferredMinHeightChanged += new EventHandler(trackSelector_PreferredMinHeightChanged);
            trackSelector.MouseDoubleClick += new MouseEventHandler(trackSelector_MouseDoubleClick);

            splitContainer1.Panel2MinSize = trackSelector.getPreferredMinSize();
            var minimum_size = getWindowMinimumSize();
            this.MinimumSize = new System.Drawing.Size(minimum_size.width, minimum_size.height);
            stripBtnScroll.Pushed = AppManager.mAutoScroll;

            applySelectedTool();
            applyQuantizeMode();

            // Palette Toolの読み込み
#if ENABLE_SCRIPT
            updatePaletteTool();
#endif

            splitContainer1.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            splitContainer1.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            splitContainer1.BackColor = System.Drawing.Color.FromArgb(212, 212, 212);
            splitContainer2.Panel1.Controls.Add(panel1);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Panel2.Controls.Add(panelWaveformZoom);
            //splitContainer2.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            splitContainer2.Panel2.BorderColor = System.Drawing.Color.FromArgb(112, 112, 112);
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            panelWaveformZoom.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(trackSelector);
            trackSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Panel2MinSize = trackSelector.getPreferredMinSize();
            splitContainerProperty.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;

#if ENABLE_PROPERTY
            splitContainerProperty.Panel1.Controls.Add(mPropertyPanelContainer);
            mPropertyPanelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
#else
            splitContainerProperty.setDividerLocation( 0 );
            splitContainerProperty.setEnabled( false );
            menuVisualProperty.setVisible( false );
#endif

            splitContainerProperty.Panel2.Controls.Add(splitContainer1);
            splitContainerProperty.Dock = System.Windows.Forms.DockStyle.Fill;

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
            pictPianoRoll.Bounds = new System.Drawing.Rectangle(0, picturePositionIndicator.Height, panel1.Width - vScroll.Width, panel1.Height - picturePositionIndicator.Height - hScroll.Height);
            // vScroll
            vScroll.Left = pictPianoRoll.getWidth();
            vScroll.Top = picturePositionIndicator.Height;
            vScroll.Height = pictPianoRoll.getHeight();
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

            updatePropertyPanelState(AppManager.editorConfig.PropertyWindowStatus.State);

            pictPianoRoll.MouseWheel += new MouseEventHandler(pictPianoRoll_MouseWheel);
            trackSelector.MouseWheel += new MouseEventHandler(trackSelector_MouseWheel);
            picturePositionIndicator.MouseWheel += new MouseEventHandler(picturePositionIndicator_MouseWheel);

            menuVisualOverview.CheckedChanged += new EventHandler(menuVisualOverview_CheckedChanged);

            hScroll.Maximum = AppManager.getVsqFile().TotalClocks + 240;
            hScroll.LargeChange = 240 * 4;

            vScroll.Maximum = (int)(controller.getScaleY() * 100 * 128);
            vScroll.LargeChange = 24 * 4;
            hScroll.SmallChange = 240;
            vScroll.SmallChange = 24;

            trackSelector.setCurveVisible(true);

            // inputTextBoxの初期化
            AppManager.mInputTextBox = new LyricTextBox();
            AppManager.mInputTextBox.Visible = false;
            AppManager.mInputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            AppManager.mInputTextBox.Width = 80;
            AppManager.mInputTextBox.AcceptsReturn = true;
            AppManager.mInputTextBox.BackColor = System.Drawing.Color.White;
            AppManager.mInputTextBox.Font = new System.Drawing.Font(AppManager.editorConfig.BaseFontName, AppManager.FONT_SIZE9, System.Drawing.FontStyle.Regular);
            AppManager.mInputTextBox.Enabled = false;
            AppManager.mInputTextBox.KeyPress += mInputTextBox_KeyPress;
            AppManager.mInputTextBox.Parent = pictPianoRoll;
            panel1.Controls.Add(AppManager.mInputTextBox);

            int fps = 1000 / AppManager.editorConfig.MaximumFrameRate;
            timer.Interval = (fps <= 0) ? 1 : fps;

#if DEBUG
            menuHelpDebug.Visible = true;
#endif // DEBUG

            string _HAND = "AAACAAEAICAAABAAEADoAgAAFgAAACgAAAAgAAAAQAAAAAEABAAAAAAAgAIAAAAAAAAAAAAAAAAAAAAAAAAAA" +
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
            System.IO.MemoryStream ms = null;
            try {
                ms = new System.IO.MemoryStream(Base64.decode(_HAND));
                HAND = new System.Windows.Forms.Cursor(ms);
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".ctor; ex=" + ex + "\n");
            } finally {
                if (ms != null) {
                    try {
                        ms.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".ctor; ex=" + ex2 + "\n");
                    }
                }
            }

            menuHelpLogSwitch.Checked = Logger.isEnabled();
            applyShortcut();

            AppManager.mMixerWindow = new FormMixer(this);
            AppManager.iconPalette = new FormIconPalette(this);

            // ファイルを開く
            if (file != "") {
                if (System.IO.File.Exists(file)) {
                    string low_file = file.ToLower();
                    if (low_file.EndsWith(".xvsq")) {
                        openVsqCor(low_file);
                        //AppManager.readVsq( file );
                    } else if (low_file.EndsWith(".vsq")) {
                        VsqFileEx vsq = null;
                        try {
                            vsq = new VsqFileEx(file, "Shift_JIS");
                            AppManager.setVsqFile(vsq);
                            updateBgmMenuState();
                        } catch (Exception ex) {
                            Logger.write(typeof(FormMain) + ".ctor; ex=" + ex + "\n");
                            serr.println("FormMain#.ctor; ex=" + ex);
                        }
                    }
                }
            }

            trackBar.Value = AppManager.editorConfig.DefaultXScale;
            AppManager.setCurrentClock(0);
            setEdited(false);

            AppManager.PreviewStarted += new EventHandler(AppManager_PreviewStarted);
            AppManager.PreviewAborted += new EventHandler(AppManager_PreviewAborted);
            AppManager.GridVisibleChanged += new EventHandler(AppManager_GridVisibleChanged);
            AppManager.itemSelection.SelectedEventChanged += new SelectedEventChangedEventHandler(ItemSelectionModel_SelectedEventChanged);
            AppManager.SelectedToolChanged += new EventHandler(AppManager_SelectedToolChanged);
            AppManager.UpdateBgmStatusRequired += new EventHandler(AppManager_UpdateBgmStatusRequired);
            AppManager.MainWindowFocusRequired += new EventHandler(AppManager_MainWindowFocusRequired);
            AppManager.EditedStateChanged += new EditedStateChangedEventHandler(AppManager_EditedStateChanged);
            AppManager.WaveViewReloadRequired += new WaveViewRealoadRequiredEventHandler(AppManager_WaveViewRealoadRequired);
            EditorConfig.QuantizeModeChanged += new EventHandler(handleEditorConfig_QuantizeModeChanged);

#if ENABLE_PROPERTY
            mPropertyPanelContainer.StateChangeRequired += new StateChangeRequiredEventHandler(mPropertyPanelContainer_StateChangeRequired);
#endif

            updateRecentFileMenu();

            // C3が画面中央に来るように調整
            int draft_start_to_draw_y = 68 * (int)(100 * controller.getScaleY()) - pictPianoRoll.getHeight() / 2;
            int draft_vscroll_value = (int)((draft_start_to_draw_y * (double)vScroll.Maximum) / (128 * (int)(100 * controller.getScaleY()) - vScroll.Height));
            try {
                vScroll.Value = draft_vscroll_value;
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".FormMain_Load; ex=" + ex + "\n");
            }

            // x=97がプリメジャークロックになるように調整
            int cp = AppManager.getVsqFile().getPreMeasureClocks();
            int draft_hscroll_value = (int)(cp - 24.0 * controller.getScaleXInv());
            try {
                hScroll.Value = draft_hscroll_value;
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".FormMain_Load; ex=" + ex + "\n");
            }

            //s_pen_dashed_171_171_171.DashPattern = new float[] { 3, 3 };
            //s_pen_dashed_209_204_172.DashPattern = new float[] { 3, 3 };

            menuVisualNoteProperty.Checked = AppManager.editorConfig.ShowExpLine;
            menuVisualLyrics.Checked = AppManager.editorConfig.ShowLyric;
            menuVisualMixer.Checked = AppManager.editorConfig.MixerVisible;
            menuVisualPitchLine.Checked = AppManager.editorConfig.ViewAtcualPitch;

            updateMenuFonts();

            AppManager.mMixerWindow.FederChanged += new FederChangedEventHandler(mixerWindow_FederChanged);
            AppManager.mMixerWindow.PanpotChanged += new PanpotChangedEventHandler(mixerWindow_PanpotChanged);
            AppManager.mMixerWindow.MuteChanged += new MuteChangedEventHandler(mixerWindow_MuteChanged);
            AppManager.mMixerWindow.SoloChanged += new SoloChangedEventHandler(mixerWindow_SoloChanged);
            AppManager.mMixerWindow.updateStatus();
            if (AppManager.editorConfig.MixerVisible) {
                AppManager.mMixerWindow.Visible = true;
            }
            AppManager.mMixerWindow.FormClosing += new FormClosingEventHandler(mixerWindow_FormClosing);

            Point p1 = AppManager.editorConfig.FormIconPaletteLocation.toPoint();
            if (!PortUtil.isPointInScreens(p1)) {
                Rectangle workingArea = PortUtil.getWorkingArea(this);
                p1 = new Point(workingArea.x, workingArea.y);
            }
            AppManager.iconPalette.Location = new System.Drawing.Point(p1.x, p1.y);
            if (AppManager.editorConfig.IconPaletteVisible) {
                AppManager.iconPalette.Visible = true;
            }
            AppManager.iconPalette.FormClosing += new FormClosingEventHandler(iconPalette_FormClosing);
            AppManager.iconPalette.LocationChanged += new EventHandler(iconPalette_LocationChanged);

            trackSelector.CommandExecuted += new EventHandler(trackSelector_CommandExecuted);

#if ENABLE_SCRIPT
            updateScriptShortcut();
            // RunOnceという名前のスクリプトがあれば，そいつを実行
            foreach (var id in ScriptServer.getScriptIdIterator()) {
                if (PortUtil.getFileNameWithoutExtension(id).ToLower() == "runonce") {
                    ScriptServer.invokeScript(id, AppManager.getVsqFile());
                    break;
                }
            }
#endif

            clearTempWave();
            updateVibratoPresetMenu();
            mPencilMode.setMode(PencilModeEnum.Off);
            updateCMenuPianoFixed();
            loadGameControler();
#if ENABLE_MIDI
            reloadMidiIn();
#endif
            menuVisualWaveform.Checked = AppManager.editorConfig.ViewWaveform;

            updateRendererMenu();

            // ウィンドウの位置・サイズを再現
            if (AppManager.editorConfig.WindowMaximized) {
                this.WindowState = FormWindowState.Maximized;
            } else {
                this.WindowState = FormWindowState.Normal;
            }
            Rectangle bounds = AppManager.editorConfig.WindowRect;
            this.Bounds = new System.Drawing.Rectangle(bounds.x, bounds.y, bounds.width, bounds.height);
            // ウィンドウ位置・サイズの設定値が、使えるディスプレイのどれにも被っていない場合
            Rectangle rc2 = PortUtil.getScreenBounds(this);
            if (bounds.x < rc2.x ||
                 rc2.x + rc2.width < bounds.x + bounds.width ||
                 bounds.y < rc2.y ||
                 rc2.y + rc2.height < bounds.y + bounds.height) {
                bounds.x = rc2.x;
                bounds.y = rc2.y;
                this.Bounds = new System.Drawing.Rectangle(bounds.x, bounds.y, bounds.width, bounds.height);
                AppManager.editorConfig.WindowRect = bounds;
            }
            this.LocationChanged += new EventHandler(FormMain_LocationChanged);

            updateScrollRangeHorizontal();
            updateScrollRangeVertical();

            // プロパティウィンドウの位置を復元
            Rectangle rc1 = PortUtil.getScreenBounds(this);
            Rectangle rcScreen = new Rectangle(rc1.x, rc1.y, rc1.width, rc1.height);
            var p = this.Location;
            XmlRectangle xr = AppManager.editorConfig.PropertyWindowStatus.Bounds;
            Point p0 = new Point(xr.x, xr.y);
            Point a = new Point(p.X + p0.x, p.Y + p0.y);
            Rectangle rc = new Rectangle(a.x,
                                          a.y,
                                          AppManager.editorConfig.PropertyWindowStatus.Bounds.getWidth(),
                                          AppManager.editorConfig.PropertyWindowStatus.Bounds.getHeight());

            if (a.y > rcScreen.y + rcScreen.height) {
                a = new Point(a.x, rcScreen.y + rcScreen.height - rc.height);
            }
            if (a.y < rcScreen.y) {
                a = new Point(a.x, rcScreen.y);
            }
            if (a.x > rcScreen.x + rcScreen.width) {
                a = new Point(rcScreen.x + rcScreen.width - rc.width, a.y);
            }
            if (a.x < rcScreen.x) {
                a = new Point(rcScreen.x, a.y);
            }
#if DEBUG
            AppManager.debugWriteLine("FormMain_Load; a=" + a);
#endif

#if ENABLE_PROPERTY
            AppManager.propertyWindow.getUi().setBounds(a.x, a.y, rc.width, rc.height);
            AppManager.propertyPanel.CommandExecuteRequired += new CommandExecuteRequiredEventHandler(propertyPanel_CommandExecuteRequired);
#endif
            updateBgmMenuState();
            AppManager.mLastTrackSelectorHeight = trackSelector.getPreferredMinSize();
            flipControlCurveVisible(true);

            Refresh();
            updateLayout();
#if DEBUG
            menuHidden.Visible = true;
#else
            menuHidden.Visible = false;
#endif

#if !ENABLE_VOCALOID
            menuTrackRenderer.remove( menuTrackRendererVOCALOID2 );
            menuTrackRenderer.remove( menuTrackRendererVOCALOID1 );
            cMenuTrackTabRenderer.remove( cMenuTrackTabRendererVOCALOID2 );
            cMenuTrackTabRenderer.remove( cMenuTrackTabRendererVOCALOID1 );
#endif

#if !ENABLE_AQUESTONE
            menuTrackRenderer.DropDownItems.Remove( menuTrackRendererAquesTone );
            menuTrackRenderer.DropDownItems.Remove( menuTrackRendererAquesTone2 );
            cMenuTrackTabRenderer.DropDownItems.Remove( cMenuTrackTabRendererAquesTone );
            cMenuTrackTabRenderer.DropDownItems.Remove( cMenuTrackTabRendererAquesTone2 );
#endif

#if DEBUG
            System.Collections.Generic.List<ValuePair<string, string>> list = new System.Collections.Generic.List<ValuePair<string, string>>();
            foreach (System.Reflection.FieldInfo fi in typeof(EditorConfig).GetFields()) {
                if (fi.IsPublic && !fi.IsStatic) {
                    list.Add(new ValuePair<string, string>(fi.Name, fi.FieldType.ToString()));
                }
            }

            foreach (System.Reflection.PropertyInfo pi in typeof(EditorConfig).GetProperties()) {
                if (!pi.CanRead || !pi.CanWrite) {
                    continue;
                }
                System.Reflection.MethodInfo getmethod = pi.GetGetMethod();
                System.Reflection.MethodInfo setmethod = pi.GetSetMethod();
                if (!setmethod.IsPublic || setmethod.IsStatic) {
                    continue;
                }
                if (!getmethod.IsPublic || getmethod.IsStatic) {
                    continue;
                }
                list.Add(new ValuePair<string, string>(pi.Name, pi.PropertyType.ToString()));
            }

            list.Sort();

            System.IO.StreamWriter sw = null;
            try {
                sw = new System.IO.StreamWriter("EditorConfig.txt");
                foreach (ValuePair<string, string> s in list) {
                    sw.WriteLine(s.Key);
                }
                sw.WriteLine("--------------------------------------------");
                foreach (ValuePair<string, string> s in list) {
                    sw.WriteLine(s.Value + "\t" + s.Key + ";");
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".ctor; ex=" + ex + "\n");
            } finally {
                if (sw != null) {
                    try {
                        sw.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".ctor; ex=" + ex2 + "\n");
                    }
                }
            }
#endif
        }
        #endregion

        #region FormMainUiの実装

        public void focusPianoRoll()
        {
            pictPianoRoll.Focus();
        }

        #endregion

        #region helper methods
        /// <summary>
        /// renderer_menu_handler_ を初期化する
        /// </summary>
        private void initializeRendererMenuHandler()
        {
            renderer_menu_handler_ = new List<RendererMenuHandler>();
            renderer_menu_handler_.Clear();
            renderer_menu_handler_.Add(new RendererMenuHandler(RendererKind.VOCALOID1,
                                                                 menuTrackRendererVOCALOID1,
                                                                 cMenuTrackTabRendererVOCALOID1,
                                                                 menuVisualPluginUiVocaloid1));
            renderer_menu_handler_.Add(new RendererMenuHandler(RendererKind.VOCALOID2,
                                                                 menuTrackRendererVOCALOID2,
                                                                 cMenuTrackTabRendererVOCALOID2,
                                                                 menuVisualPluginUiVocaloid2));
            renderer_menu_handler_.Add(new RendererMenuHandler(RendererKind.STRAIGHT_UTAU,
                                                                 menuTrackRendererVCNT,
                                                                 cMenuTrackTabRendererStraight,
                                                                 null));
            renderer_menu_handler_.Add(new RendererMenuHandler(RendererKind.UTAU,
                                                                 menuTrackRendererUtau,
                                                                 cMenuTrackTabRendererUtau,
                                                                 null));
            renderer_menu_handler_.Add(new RendererMenuHandler(RendererKind.AQUES_TONE,
                                                                 menuTrackRendererAquesTone,
                                                                 cMenuTrackTabRendererAquesTone,
                                                                 menuVisualPluginUiAquesTone));
            renderer_menu_handler_.Add(new RendererMenuHandler(RendererKind.AQUES_TONE2,
                                                                 menuTrackRendererAquesTone2,
                                                                 cMenuTrackTabRendererAquesTone2,
                                                                 menuVisualPluginUiAquesTone2));
        }

        /// <summary>
        /// 指定した歌手とリサンプラーについて，設定値に登録されていないものだったら登録する．
        /// </summary>
        /// <param name="resampler_path"></param>
        /// <param name="singer_path"></param>
        private void checkUnknownResamplerAndSinger(ByRef<string> resampler_path, ByRef<string> singer_path)
        {
            string utau = Utility.getExecutingUtau();
            string utau_dir = "";
            if (utau != "") {
                utau_dir = PortUtil.getDirectoryName(utau);
            }

            // 可能なら，VOICEの文字列を置換する
            string search = "%VOICE%";
            if (singer_path.value.StartsWith(search) && singer_path.value.Length > search.Length) {
                singer_path.value = singer_path.value.Substring(search.Length);
                singer_path.value = Path.Combine(Path.Combine(utau_dir, "voice"), singer_path.value);
            }

            // 歌手はknownかunknownか？
            // 歌手指定が知らない歌手だった場合に，ダイアログを出すかどうか
            bool check_unknown_singer = false;
            if (System.IO.File.Exists(Path.Combine(singer_path.value, "oto.ini"))) {
                // oto.iniが存在する場合
                // editorConfigに入っていない場合に，ダイアログを出す
                bool found = false;
                for (int i = 0; i < AppManager.editorConfig.UtauSingers.Count; i++) {
                    SingerConfig sc = AppManager.editorConfig.UtauSingers[i];
                    if (sc == null) {
                        continue;
                    }
                    if (sc.VOICEIDSTR == singer_path.value) {
                        found = true;
                        break;
                    }
                }
                check_unknown_singer = !found;
            }

            // リサンプラーが知っているやつかどうか
            bool check_unknwon_resampler = false;
#if DEBUG
            sout.println("FormMain#checkUnknownResamplerAndSinger; resampler_path.value=" + resampler_path.value);
#endif
            string resampler_dir = PortUtil.getDirectoryName(resampler_path.value);
            if (resampler_dir == "") {
                // ディレクトリが空欄なので，UTAUのデフォルトのリサンプラー指定である
                resampler_path.value = Path.Combine(utau_dir, resampler_path.value);
                resampler_dir = PortUtil.getDirectoryName(resampler_path.value);
            }
            if (resampler_dir != "" && System.IO.File.Exists(resampler_path.value)) {
                bool found = false;
                for (int i = 0; i < AppManager.editorConfig.getResamplerCount(); i++) {
                    string resampler = AppManager.editorConfig.getResamplerAt(i);
                    if (resampler == resampler_path.value) {
                        found = true;
                        break;
                    }
                }
                check_unknwon_resampler = !found;
            }

            // unknownな歌手やリサンプラーが発見された場合.
            // 登録するかどうか問い合わせるダイアログを出す
            FormCheckUnknownSingerAndResampler dialog = null;
            try {
                if (check_unknown_singer || check_unknwon_resampler) {
                    dialog = new FormCheckUnknownSingerAndResampler(singer_path.value, check_unknown_singer, resampler_path.value, check_unknwon_resampler);
                    dialog.Location = getFormPreferedLocation(dialog);
                    DialogResult dr = AppManager.showModalDialog(dialog, this);
                    if (dr != DialogResult.OK) {
                        return;
                    }

                    // 登録する
                    // リサンプラー
                    if (dialog.isResamplerChecked()) {
                        string path = dialog.getResamplerPath();
                        if (System.IO.File.Exists(path)) {
                            AppManager.editorConfig.addResampler(path);
                        }
                    }
                    // 歌手
                    if (dialog.isSingerChecked()) {
                        string path = dialog.getSingerPath();
                        if (Directory.Exists(path)) {
                            SingerConfig sc = new SingerConfig();
                            Utility.readUtauSingerConfig(path, sc);
                            AppManager.editorConfig.UtauSingers.Add(sc);
                        }
                        AppManager.reloadUtauVoiceDB();
                    }
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".checkUnknownResamplerAndSinger; ex=" + ex + "\n");
            } finally {
                if (dialog != null) {
                    try {
                        dialog.Close();
                    } catch (Exception ex2) {
                    }
                }
            }
        }

        /// <summary>
        /// ピアノロールの縦軸の拡大率をdelta段階上げます
        /// </summary>
        /// <param name="delta"></param>
        private void zoomY(int delta)
        {
            int scaley = AppManager.editorConfig.PianoRollScaleY;
            int draft = scaley + delta;
            if (draft < EditorConfig.MIN_PIANOROLL_SCALEY) {
                draft = EditorConfig.MIN_PIANOROLL_SCALEY;
            }
            if (EditorConfig.MAX_PIANOROLL_SCALEY < draft) {
                draft = EditorConfig.MAX_PIANOROLL_SCALEY;
            }
            if (scaley != draft) {
                AppManager.editorConfig.PianoRollScaleY = draft;
                updateScrollRangeVertical();
                controller.setStartToDrawY(calculateStartToDrawY(vScroll.Value));
                updateDrawObjectList();
            }
        }

        /// <summary>
        /// ズームスライダの現在の値から，横方向の拡大率を計算します
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float getScaleXFromTrackBarValue(int value)
        {
            return value / 480.0f;
        }

        /// <summary>
        /// ユーザー定義のビブラートのプリセット関係のメニューの表示状態を更新します
        /// </summary>
        private void updateVibratoPresetMenu()
        {
            // 現在の項目数に過不足があれば調節する
            int size = AppManager.editorConfig.AutoVibratoCustom.Count;
            int delta = size - menuLyricCopyVibratoToPreset.DropDownItems.Count;
            if (delta > 0) {
                // 項目を増やさないといけない
                for (int i = 0; i < delta; i++) {
                    System.Windows.Forms.ToolStripMenuItem item =
                        new System.Windows.Forms.ToolStripMenuItem(
                            "", null, new EventHandler(handleVibratoPresetSubelementClick));
                    menuLyricCopyVibratoToPreset.DropDownItems.Add(item);
                }
            } else if (delta < 0) {
                // 項目を減らさないといけない
                for (int i = 0; i < -delta; i++) {
                    System.Windows.Forms.ToolStripItem item = menuLyricCopyVibratoToPreset.DropDownItems[0];
                    menuLyricCopyVibratoToPreset.DropDownItems.RemoveAt(0);
                    item.Dispose();
                }
            }

            // 表示状態を更新
            for (int i = 0; i < size; i++) {
                VibratoHandle handle = AppManager.editorConfig.AutoVibratoCustom[i];
                menuLyricCopyVibratoToPreset.DropDownItems[i].Text = handle.getCaption();
            }
        }

        /// <summary>
        /// MIDIステップ入力中に，ソングポジションが動いたときの処理を行います
        /// AppManager.mAddingEventが非nullの時，音符の先頭は決まっているので，
        /// ソングポジションと，音符の先頭との距離から音符の長さを算出し，更新する
        /// AppManager.mAddingEventがnullの時は何もしない
        /// </summary>
        private void updateNoteLengthStepSequencer()
        {
            if (!controller.isStepSequencerEnabled()) {
                return;
            }

            VsqEvent item = AppManager.mAddingEvent;
            if (item == null) {
                return;
            }

            int song_position = AppManager.getCurrentClock();
            int start = item.Clock;
            int length = song_position - start;
            if (length < 0) length = 0;
            Utility.editLengthOfVsqEvent(
                item,
                length,
                AppManager.vibratoLengthEditingRule);
        }

        /// <summary>
        /// 現在追加しようとしている音符の内容(AppManager.mAddingEvent)をfixします
        /// </summary>
        /// <returns></returns>
        private void fixAddingEvent()
        {
            VsqFileEx vsq = AppManager.getVsqFile();
            int selected = AppManager.getSelected();
            VsqTrack vsq_track = vsq.Track[selected];
            LyricHandle lyric = new LyricHandle("あ", "a");
            VibratoHandle vibrato = null;
            int vibrato_delay = 0;
            if (AppManager.editorConfig.EnableAutoVibrato) {
                int note_length = AppManager.mAddingEvent.ID.getLength();
                // 音符位置での拍子を調べる
                Timesig timesig = vsq.getTimesigAt(AppManager.mAddingEvent.Clock);

                // ビブラートを自動追加するかどうかを決める閾値
                int threshold = AppManager.editorConfig.AutoVibratoThresholdLength;
                if (note_length >= threshold) {
                    int vibrato_clocks = 0;
                    if (AppManager.editorConfig.DefaultVibratoLength == DefaultVibratoLengthEnum.L100) {
                        vibrato_clocks = note_length;
                    } else if (AppManager.editorConfig.DefaultVibratoLength == DefaultVibratoLengthEnum.L50) {
                        vibrato_clocks = note_length / 2;
                    } else if (AppManager.editorConfig.DefaultVibratoLength == DefaultVibratoLengthEnum.L66) {
                        vibrato_clocks = note_length * 2 / 3;
                    } else if (AppManager.editorConfig.DefaultVibratoLength == DefaultVibratoLengthEnum.L75) {
                        vibrato_clocks = note_length * 3 / 4;
                    }
                    SynthesizerType type = SynthesizerType.VOCALOID2;
                    RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
                    if (kind == RendererKind.VOCALOID1) {
                        type = SynthesizerType.VOCALOID1;
                    }
                    vibrato = AppManager.editorConfig.createAutoVibrato(type, vibrato_clocks);
                    vibrato_delay = note_length - vibrato_clocks;
                }
            }

            // oto.iniの設定を反映
            VsqEvent item = vsq_track.getSingerEventAt(AppManager.mAddingEvent.Clock);
            SingerConfig singerConfig = null;
            if (item != null && item.ID != null && item.ID.IconHandle != null) {
                singerConfig = AppManager.getSingerInfoUtau(item.ID.IconHandle.Language, item.ID.IconHandle.Program);
            }

            if (singerConfig != null && AppManager.mUtauVoiceDB.ContainsKey(singerConfig.VOICEIDSTR)) {
                UtauVoiceDB utauVoiceDb = AppManager.mUtauVoiceDB[singerConfig.VOICEIDSTR];
                OtoArgs otoArgs = utauVoiceDb.attachFileNameFromLyric(lyric.L0.Phrase, AppManager.mAddingEvent.ID.Note);
                AppManager.mAddingEvent.UstEvent.setPreUtterance(otoArgs.msPreUtterance);
                AppManager.mAddingEvent.UstEvent.setVoiceOverlap(otoArgs.msOverlap);
            }

            // 自動ノーマライズのモードで、処理を分岐
            if (AppManager.mAutoNormalize) {
                VsqTrack work = (VsqTrack)vsq_track.clone();
                AppManager.mAddingEvent.ID.type = VsqIDType.Anote;
                AppManager.mAddingEvent.ID.Dynamics = 64;
                AppManager.mAddingEvent.ID.VibratoHandle = vibrato;
                AppManager.mAddingEvent.ID.LyricHandle = lyric;
                AppManager.mAddingEvent.ID.VibratoDelay = vibrato_delay;

                bool changed = true;
                while (changed) {
                    changed = false;
                    for (int i = 0; i < work.getEventCount(); i++) {
                        int start_clock = work.getEvent(i).Clock;
                        int end_clock = work.getEvent(i).ID.getLength() + start_clock;
                        if (start_clock < AppManager.mAddingEvent.Clock && AppManager.mAddingEvent.Clock < end_clock) {
                            work.getEvent(i).ID.setLength(AppManager.mAddingEvent.Clock - start_clock);
                            changed = true;
                        } else if (start_clock == AppManager.mAddingEvent.Clock) {
                            work.removeEvent(i);
                            changed = true;
                            break;
                        } else if (AppManager.mAddingEvent.Clock < start_clock && start_clock < AppManager.mAddingEvent.Clock + AppManager.mAddingEvent.ID.getLength()) {
                            AppManager.mAddingEvent.ID.setLength(start_clock - AppManager.mAddingEvent.Clock);
                            changed = true;
                        }
                    }
                }
                VsqEvent add = (VsqEvent)AppManager.mAddingEvent.clone();
                work.addEvent(add);
                CadenciiCommand run = VsqFileEx.generateCommandTrackReplace(selected,
                                                                             work,
                                                                             AppManager.getVsqFile().AttachedCurves.get(selected - 1));
                AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                setEdited(true);
            } else {
                VsqEvent[] items = new VsqEvent[1];
                AppManager.mAddingEvent.ID.type = VsqIDType.Anote;
                AppManager.mAddingEvent.ID.Dynamics = 64;
                items[0] = (VsqEvent)AppManager.mAddingEvent.clone();// new VsqEvent( 0, AppManager.addingEvent.ID );
                items[0].Clock = AppManager.mAddingEvent.Clock;
                items[0].ID.LyricHandle = lyric;
                items[0].ID.VibratoDelay = vibrato_delay;
                items[0].ID.VibratoHandle = vibrato;

                CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventAddRange(AppManager.getSelected(), items));
                AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                setEdited(true);
            }
        }

        /// <summary>
        /// 現在のツールバーの場所を保存します
        /// </summary>
        private void saveToolbarLocation()
        {
            if (this.WindowState == System.Windows.Forms.FormWindowState.Minimized) return;
            // どのツールバーが一番上かつ左にあるか？
            var list = new System.Collections.Generic.List<RebarBand>();
            list.AddRange(new RebarBand[]{
                bandFile,
                bandMeasure,
                bandPosition,
                bandTool });
            // ソートする
            bool changed = true;
            while (changed) {
                changed = false;
                for (int i = 0; i < list.Count - 1; i++) {
                    // y座標が大きいか，y座標が同じでもx座標が大きい場合に入れ替える
                    bool swap =
                        (list[i].Location.Y > list[i + 1].Location.Y) ||
                        (list[i].Location.Y == list[i + 1].Location.Y && list[i].Location.X > list[i + 1].Location.X);
                    if (swap) {
                        var a = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = a;
                        changed = true;
                    }
                }
            }
            // 各ツールバー毎に，ツールバーの状態を検出して保存
            saveToolbarLocationCore(
                list,
                bandFile,
                out AppManager.editorConfig.BandSizeFile,
                out AppManager.editorConfig.BandNewRowFile,
                out AppManager.editorConfig.BandOrderFile);
            saveToolbarLocationCore(
                list,
                bandMeasure,
                out AppManager.editorConfig.BandSizeMeasure,
                out AppManager.editorConfig.BandNewRowMeasure,
                out AppManager.editorConfig.BandOrderMeasure);
            saveToolbarLocationCore(
                list,
                bandPosition,
                out AppManager.editorConfig.BandSizePosition,
                out AppManager.editorConfig.BandNewRowPosition,
                out AppManager.editorConfig.BandOrderPosition);
            saveToolbarLocationCore(
                list,
                bandTool,
                out AppManager.editorConfig.BandSizeTool,
                out AppManager.editorConfig.BandNewRowTool,
                out AppManager.editorConfig.BandOrderTool);
        }

        /// <summary>
        /// ツールバーの位置の順に並べ替えたリストの中の一つのツールバーに対して，その状態を検出して保存
        /// </summary>
        /// <param name="list"></param>
        /// <param name="band"></param>
        /// <param name="band_size"></param>
        /// <param name="new_row"></param>
        private void saveToolbarLocationCore(
            System.Collections.Generic.List<RebarBand> list,
            RebarBand band,
            out int band_size,
            out bool new_row,
            out int band_order)
        {
            band_size = 0;
            new_row = true;
            band_order = 0;
            var indx = list.IndexOf(band);
            if (indx < 0) return;
            new_row = (indx == 0) ? false : (list[indx - 1].Location.Y < list[indx].Location.Y);
            band_size = band.BandSize;
            band_order = indx;
        }

        private static int doQuantize(int clock, int unit)
        {
            int odd = clock % unit;
            int new_clock = clock - odd;
            if (odd > unit / 2) {
                new_clock += unit;
            }
            return new_clock;
        }

        /// <summary>
        /// デフォルトのストロークを取得します
        /// </summary>
        /// <returns></returns>
        private BasicStroke getStrokeDefault()
        {
            if (mStrokeDefault == null) {
                mStrokeDefault = new BasicStroke();
            }
            return mStrokeDefault;
        }

        /// <summary>
        /// 描画幅が2pxのストロークを取得します
        /// </summary>
        /// <returns></returns>
        private BasicStroke getStroke2px()
        {
            if (mStroke2px == null) {
                mStroke2px = new BasicStroke(2.0f);
            }
            return mStroke2px;
        }

        /// <summary>
        /// 選択された音符の長さを、指定したゲートタイム分長くします。
        /// </summary>
        /// <param name="delta_length"></param>
        private void lengthenSelectedEvent(int delta_length)
        {
            if (delta_length == 0) {
                return;
            }

            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq == null) {
                return;
            }

            int selected = AppManager.getSelected();

            List<VsqEvent> items = new List<VsqEvent>();
            foreach (var item in AppManager.itemSelection.getEventIterator()) {
                if (item.editing.ID.type != VsqIDType.Anote &&
                     item.editing.ID.type != VsqIDType.Aicon) {
                    continue;
                }

                // クレッシェンド、デクレッシェンドでないものを省く
                if (item.editing.ID.type == VsqIDType.Aicon) {
                    if (item.editing.ID.IconDynamicsHandle == null) {
                        continue;
                    }
                    if (!item.editing.ID.IconDynamicsHandle.isCrescendType() &&
                         !item.editing.ID.IconDynamicsHandle.isDecrescendType()) {
                        continue;
                    }
                }

                // 長さを変える。0未満になると0に直す
                int length = item.editing.ID.getLength();
                int draft = length + delta_length;
                if (draft < 0) {
                    draft = 0;
                }
                if (length == draft) {
                    continue;
                }

                // ビブラートの長さを変更
                VsqEvent add = (VsqEvent)item.editing.clone();
                Utility.editLengthOfVsqEvent(add, draft, AppManager.vibratoLengthEditingRule);
                items.Add(add);
            }

            if (items.Count <= 0) {
                return;
            }

            // コマンドを発行
            CadenciiCommand run = new CadenciiCommand(
                VsqCommand.generateCommandEventReplaceRange(
                    selected, items.ToArray()));
            AppManager.editHistory.register(vsq.executeCommand(run));

            // 編集されたものを再選択する
            foreach (var item in items) {
                AppManager.itemSelection.addEvent(item.InternalID);
            }

            // 編集が施された。
            setEdited(true);
            updateDrawObjectList();

            refreshScreen();
        }

        /// <summary>
        /// 選択された音符の音程とゲートタイムを、指定されたノートナンバーおよびゲートタイム分上下させます。
        /// </summary>
        /// <param name="delta_note"></param>
        /// <param name="delta_clock"></param>
        private void moveUpDownLeftRight(int delta_note, int delta_clock)
        {
            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq == null) {
                return;
            }

            List<VsqEvent> items = new List<VsqEvent>();
            int selected = AppManager.getSelected();
            int note_max = -1;
            int note_min = 129;
            int clock_max = int.MinValue;
            int clock_min = int.MaxValue;
            foreach (var item in AppManager.itemSelection.getEventIterator()) {
                if (item.editing.ID.type != VsqIDType.Anote) {
                    continue;
                }
                VsqEvent add = null;

                // 音程
                int note = item.editing.ID.Note;
                if (delta_note != 0 && 0 <= note + delta_note && note + delta_note <= 127) {
                    add = (VsqEvent)item.editing.clone();
                    add.ID.Note += delta_note;
                    note_max = Math.Max(note_max, add.ID.Note);
                    note_min = Math.Min(note_min, add.ID.Note);
                }

                // ゲートタイム
                int clockstart = item.editing.Clock;
                int clockend = clockstart + item.editing.ID.getLength();
                if (delta_clock != 0) {
                    if (add == null) {
                        add = (VsqEvent)item.editing.clone();
                    }
                    add.Clock += delta_clock;
                    clock_max = Math.Max(clock_max, clockend + delta_clock);
                    clock_min = Math.Min(clock_min, clockstart);
                }

                if (add != null) {
                    items.Add(add);
                }
            }
            if (items.Count <= 0) {
                return;
            }

            // コマンドを発行
            CadenciiCommand run = new CadenciiCommand(
                VsqCommand.generateCommandEventReplaceRange(
                    selected, items.ToArray()));
            AppManager.editHistory.register(vsq.executeCommand(run));

            // 編集されたものを再選択する
            foreach (var item in items) {
                AppManager.itemSelection.addEvent(item.InternalID);
            }

            // 編集が施された。
            setEdited(true);
            updateDrawObjectList();

            // 音符が見えるようにする。音程方向
            if (delta_note > 0) {
                note_max++;
                if (127 < note_max) {
                    note_max = 127;
                }
                ensureVisibleY(note_max);
            } else if (delta_note < 0) {
                note_min -= 2;
                if (note_min < 0) {
                    note_min = 0;
                }
                ensureVisibleY(note_min);
            }

            // 音符が見えるようにする。時間方向
            if (delta_clock > 0) {
                ensureVisible(clock_max);
            } else if (delta_clock < 0) {
                ensureVisible(clock_min);
            }
            refreshScreen();
        }

        /// <summary>
        /// マウス位置におけるIDを返します。該当するIDが無ければnullを返します
        /// rectには、該当するIDがあればその画面上での形状を、該当するIDがなければ、
        /// 画面上で最も近かったIDの画面上での形状を返します
        /// </summary>
        /// <param name="mouse_position"></param>
        /// <returns></returns>
        private VsqEvent getItemAtClickedPosition(Point mouse_position, ByRef<Rectangle> rect)
        {
            rect.value = new Rectangle();
            int width = pictPianoRoll.getWidth();
            int height = pictPianoRoll.getHeight();
            int key_width = AppManager.keyWidth;

            // マウスが可視範囲になければ死ぬ
            if (mouse_position.x < key_width || width < mouse_position.x) {
                return null;
            }
            if (mouse_position.y < 0 || height < mouse_position.y) {
                return null;
            }

            // 表示中のトラック番号が異常だったら死ぬ
            int selected = AppManager.getSelected();
            if (selected < 1) {
                return null;
            }
            lock (AppManager.mDrawObjects) {
                List<DrawObject> dobj_list = AppManager.mDrawObjects[selected - 1];
                int count = dobj_list.Count;
                int start_to_draw_x = controller.getStartToDrawX();
                int start_to_draw_y = controller.getStartToDrawY();
                VsqFileEx vsq = AppManager.getVsqFile();
                VsqTrack vsq_track = vsq.Track[selected];

                for (int i = 0; i < count; i++) {
                    DrawObject dobj = dobj_list[i];
                    int x = dobj.mRectangleInPixel.x + key_width - start_to_draw_x;
                    int y = dobj.mRectangleInPixel.y - start_to_draw_y;
                    if (mouse_position.x < x) {
                        continue;
                    }
                    if (x + dobj.mRectangleInPixel.width < mouse_position.x) {
                        continue;
                    }
                    if (width < x) {
                        break;
                    }
                    if (mouse_position.y < y) {
                        continue;
                    }
                    if (y + dobj.mRectangleInPixel.height < mouse_position.y) {
                        continue;
                    }
                    int internal_id = dobj.mInternalID;
                    for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                        VsqEvent item = itr.next();
                        if (item.InternalID == internal_id) {
                            rect.value = new Rectangle(x, y, dobj.mRectangleInPixel.width, dobj.mRectangleInPixel.height);
                            return item;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 真ん中ボタンで画面を移動させるときの、vScrollの値を計算します。
        /// 計算には、mButtonInitial, mMiddleButtonVScrollの値が使われます。
        /// </summary>
        /// <returns></returns>
        private int computeVScrollValueForMiddleDrag(int mouse_y)
        {
            int dy = mouse_y - mButtonInitial.y;
            int max = vScroll.Maximum - vScroll.LargeChange;
            int min = vScroll.Minimum;
            double new_vscroll_value = (double)mMiddleButtonVScroll - dy * max / (128.0 * (int)(100.0 * controller.getScaleY()) - (double)pictPianoRoll.getHeight());
            int value = (int)new_vscroll_value;
            if (value < min) {
                value = min;
            } else if (max < value) {
                value = max;
            }
            return value;
        }

        /// <summary>
        /// 真ん中ボタンで画面を移動させるときの、hScrollの値を計算します。
        /// 計算には、mButtonInitial, mMiddleButtonHScrollの値が使われます。
        /// </summary>
        /// <returns></returns>
        private int computeHScrollValueForMiddleDrag(int mouse_x)
        {
            int dx = mouse_x - mButtonInitial.x;
            int max = hScroll.Maximum - hScroll.LargeChange;
            int min = hScroll.Minimum;
            double new_hscroll_value = (double)mMiddleButtonHScroll - (double)dx * controller.getScaleXInv();
            int value = (int)new_hscroll_value;
            if (value < min) {
                value = min;
            } else if (max < value) {
                value = max;
            }
            return value;
        }

        /// <summary>
        /// 仮想スクリーン上でみた時の，現在のピアノロール画面の上端のy座標が指定した値とするための，vScrollの値を計算します
        /// calculateStartToDrawYの逆関数です
        /// </summary>
        private int calculateVScrollValueFromStartToDrawY(int start_to_draw_y)
        {
            return (int)(start_to_draw_y / controller.getScaleY());
        }

        /// <summary>
        /// 現在表示されているピアノロール画面の右上の、仮想スクリーン上座標で見たときのy座標(pixel)を取得します
        /// </summary>
        private int calculateStartToDrawY(int vscroll_value)
        {
            int min = vScroll.Minimum;
            int max = vScroll.Maximum - vScroll.LargeChange;
            int value = vscroll_value;
            if (value < min) {
                value = min;
            } else if (max < value) {
                value = max;
            }
            return (int)(value * controller.getScaleY());
        }

        /// <summary>
        /// Downloads update information xml, and deserialize it.
        /// </summary>
        /// <returns></returns>
        private updater.UpdateInfo downloadUpdateInfo()
        {
            var xml_contents = "";
            try {
                var url = RECENT_UPDATE_INFO_URL;
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream())) {
                    xml_contents = reader.ReadToEnd();
                }
            } catch {
                return null;
            }

            updater.UpdateInfo update_info = null;
            var xml = new System.Xml.XmlDocument();
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(updater.UpdateInfo));
            try {
                xml.LoadXml(xml_contents);
                using (var stream = new MemoryStream()) {
                    xml.Save(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    update_info = serializer.Deserialize(stream) as updater.UpdateInfo;
                }
            } catch { }
            return update_info;
        }

        /// <summary>
        /// Show update information async.
        /// </summary>
        private void showUpdateInformationAsync(bool is_explicit_update_check)
        {
            menuHelpCheckForUpdates.Enabled = false;
            updater.UpdateInfo update_info = null;
            var worker = new System.ComponentModel.BackgroundWorker();
            worker.DoWork += (s, e) => {
                update_info = downloadUpdateInfo();
            };
            worker.RunWorkerCompleted += (s, e) => {
                if (update_info != null && update_info.DownloadUrl != "") {
                    var current_version = new Version(BAssemblyInfo.fileVersion);
                    var recent_version_string = string.Format("{0}.{1}.{2}", update_info.Major, update_info.Minor, update_info.Build);
                    var recent_version = new Version(recent_version_string);
                    if (current_version < recent_version) {
                        var form = Factory.createUpdateCheckForm();
                        form.setDownloadUrl(update_info.DownloadUrl);
                        form.setFont(AppManager.editorConfig.getBaseFont().font);
                        form.setOkButtonText(_("OK"));
                        form.setTitle(_("Check For Updates"));
                        form.setMessage(string.Format(_("New version {0} is available."), recent_version_string));
                        form.setAutomaticallyCheckForUpdates(!AppManager.editorConfig.DoNotAutomaticallyCheckForUpdates);
                        form.setAutomaticallyCheckForUpdatesMessage(_("Automatically check for updates"));
                        form.okButtonClicked += () => form.close();
                        form.downloadLinkClicked += () => {
                            form.close();
                            System.Diagnostics.Process.Start(update_info.DownloadUrl);
                        };
                        form.showDialog(this);
                        AppManager.editorConfig.DoNotAutomaticallyCheckForUpdates = !form.isAutomaticallyCheckForUpdates();
                    } else if (is_explicit_update_check) {
                        MessageBox.Show(_("Cadencii is up to date"),
                                        _("Info"),
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                } else if (is_explicit_update_check) {
                    MessageBox.Show(_("Can't get update information. Please retry after few minutes."),
                                    _("Error"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                var t = new System.Windows.Forms.Timer();
                t.Tick += (timer_sender, timer_args) => {
                    menuHelpCheckForUpdates.Enabled = true;
                    t.Stop();
                };
                t.Interval = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
                t.Start();
            };
            worker.RunWorkerAsync();
        }
        #endregion

        #region public methods
        /// <summary>
        /// デフォルトのショートカットキーを格納したリストを取得します
        /// </summary>
        public List<ValuePairOfStringArrayOfKeys> getDefaultShortcutKeys()
        {
#if JAVA_MAC
            Keys ctrl = Keys.Menu;
#else
            Keys ctrl = Keys.Control;
#endif
            List<ValuePairOfStringArrayOfKeys> ret = new List<ValuePairOfStringArrayOfKeys>(new ValuePairOfStringArrayOfKeys[]{
                new ValuePairOfStringArrayOfKeys( menuFileNew.Name, new Keys[]{ ctrl, Keys.N } ),
                new ValuePairOfStringArrayOfKeys( menuFileOpen.Name, new Keys[]{ ctrl, Keys.O } ),
                new ValuePairOfStringArrayOfKeys( menuFileOpenVsq.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuFileSave.Name, new Keys[]{ ctrl, Keys.S } ),
                new ValuePairOfStringArrayOfKeys( menuFileQuit.Name, new Keys[]{ ctrl, Keys.Q } ),
                new ValuePairOfStringArrayOfKeys( menuFileSaveNamed.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuFileImportVsq.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuFileOpenUst.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuFileImportMidi.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuFileExportWave.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuFileExportMidi.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuEditUndo.Name, new Keys[]{ ctrl, Keys.Z } ),
                new ValuePairOfStringArrayOfKeys( menuEditRedo.Name, new Keys[]{ ctrl, Keys.Shift, Keys.Z } ),
                new ValuePairOfStringArrayOfKeys( menuEditCut.Name, new Keys[]{ ctrl, Keys.X } ),
                new ValuePairOfStringArrayOfKeys( menuEditCopy.Name, new Keys[]{ ctrl, Keys.C } ),
                new ValuePairOfStringArrayOfKeys( menuEditPaste.Name, new Keys[]{ ctrl, Keys.V } ),
                new ValuePairOfStringArrayOfKeys( menuEditSelectAll.Name, new Keys[]{ ctrl, Keys.A } ),
                new ValuePairOfStringArrayOfKeys( menuEditSelectAllEvents.Name, new Keys[]{ ctrl, Keys.Shift, Keys.A } ),
                new ValuePairOfStringArrayOfKeys( menuEditDelete.Name, new Keys[]{ Keys.Back } ),
                new ValuePairOfStringArrayOfKeys( menuVisualMixer.Name, new Keys[]{ Keys.F3 } ),
                new ValuePairOfStringArrayOfKeys( menuVisualWaveform.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuVisualProperty.Name, new Keys[]{ Keys.F6 } ),
                new ValuePairOfStringArrayOfKeys( menuVisualGridline.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuVisualStartMarker.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuVisualEndMarker.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuVisualLyrics.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuVisualNoteProperty.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuVisualPitchLine.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuVisualIconPalette.Name, new Keys[]{ Keys.F4 } ),
                new ValuePairOfStringArrayOfKeys( menuJobNormalize.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuJobInsertBar.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuJobDeleteBar.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuJobRandomize.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuJobConnect.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuJobLyric.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackOn.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackAdd.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackCopy.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackChangeName.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackDelete.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackRenderCurrent.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackRenderAll.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackOverlay.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackRendererVOCALOID1.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackRendererVOCALOID2.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuTrackRendererUtau.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuLyricExpressionProperty.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuLyricVibratoProperty.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuLyricDictionary.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuScriptUpdate.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuSettingPreference.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuSettingGameControlerSetting.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuSettingGameControlerLoad.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuSettingPaletteTool.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuSettingShortcut.Name, new Keys[]{} ),
                //new ValuePairOfStringArrayOfKeys( menuSettingSingerProperty.getName(), new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuHelpAbout.Name, new Keys[]{} ),
                new ValuePairOfStringArrayOfKeys( menuHiddenEditLyric.Name, new Keys[]{ Keys.F2 } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenEditFlipToolPointerPencil.Name, new Keys[]{ ctrl, Keys.W } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenEditFlipToolPointerEraser.Name, new Keys[]{ ctrl, Keys.E } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenVisualForwardParameter.Name, new Keys[]{ ctrl, Keys.Alt, Keys.PageDown } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenVisualBackwardParameter.Name, new Keys[]{ ctrl, Keys.Alt, Keys.PageUp } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenTrackNext.Name, new Keys[]{ ctrl, Keys.PageDown } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenTrackBack.Name, new Keys[]{ ctrl, Keys.PageUp } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenSelectBackward.Name, new Keys[]{ Keys.Alt, Keys.Left } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenSelectForward.Name, new Keys[]{ Keys.Alt, Keys.Right } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenMoveUp.Name, new Keys[]{ Keys.Shift, Keys.Up } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenMoveDown.Name, new Keys[]{ Keys.Shift, Keys.Down } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenMoveLeft.Name, new Keys[]{ Keys.Shift, Keys.Left } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenMoveRight.Name, new Keys[]{ Keys.Shift, Keys.Right } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenLengthen.Name, new Keys[]{ ctrl, Keys.Right } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenShorten.Name, new Keys[]{ ctrl, Keys.Left } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenGoToEndMarker.Name, new Keys[]{ ctrl, Keys.End } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenGoToStartMarker.Name, new Keys[]{ ctrl, Keys.Home } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenPlayFromStartMarker.Name, new Keys[]{ ctrl, Keys.Enter } ),
                new ValuePairOfStringArrayOfKeys( menuHiddenFlipCurveOnPianorollMode.Name, new Keys[]{ Keys.Tab } ),
            });
            return ret;
        }

        /// <summary>
        /// マウスの真ん中ボタンが押されたかどうかを調べます。
        /// スペースキー+左ボタンで真ん中ボタンとみなすかどうか、というオプションも考慮される。
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool isMouseMiddleButtonDowned(MouseButtons button)
        {
            bool ret = false;
            if (AppManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier) {
                if (mSpacekeyDowned && button == MouseButtons.Left) {
                    ret = true;
                }
            } else {
                if (button == MouseButtons.Middle) {
                    ret = true;
                }
            }
            return ret;
        }

        /// <summary>
#if USE_BGWORK_SCREEN
        /// 画面をメインスレッドとは別のワーカースレッドを用いて再描画します。
#else
        /// 画面を再描画します。
#endif
        /// 再描画間隔が設定値より短い場合再描画がスキップされます。
        /// </summary>
        public void refreshScreen(bool force)
        {
#if USE_BGWORK_SCREEN
            if ( !bgWorkScreen.IsBusy ) {
                double now = PortUtil.getCurrentTime();
                double dt = now - mLastScreenRefreshedSec;
                double mindt = 1.0 / AppManager.editorConfig.MaximumFrameRate;
                if ( dt > mindt ) {
                    mLastScreenRefreshedSec = now;
                    bgWorkScreen.RunWorkerAsync();
                }
            }
#else
            if (mIsRefreshing) {
                return;
            } else {
                double now = PortUtil.getCurrentTime();
                double dt = now - mLastScreenRefreshedSec;
                double mindt = 1.0 / AppManager.editorConfig.MaximumFrameRate;
                if (force || (!force && dt > mindt)) {
                    mIsRefreshing = true;

                    mLastScreenRefreshedSec = now;
                    refreshScreenCore(this, null);

                    mIsRefreshing = false;
                }
            }
#endif
        }

        public void refreshScreen()
        {
            refreshScreen(false);
        }

        public void refreshScreenCore(Object sender, EventArgs e)
        {
#if MONITOR_FPS
            double t0 = PortUtil.getCurrentTime();
#endif
            pictPianoRoll.Refresh();
            picturePositionIndicator.Refresh();
            trackSelector.Refresh();
            pictureBox2.Refresh();
            if (menuVisualWaveform.Checked) {
                waveView.Refresh();
            }
            if (AppManager.editorConfig.OverviewEnabled) {
                panelOverview.Refresh();
            }
#if MONITOR_FPS
            double t = PortUtil.getCurrentTime();
            mFpsDrawTime[mFpsDrawTimeIndex] = t;
            mFpsDrawTime2[mFpsDrawTimeIndex] = t - t0;

            mFpsDrawTimeIndex++;
            if (mFpsDrawTimeIndex >= mFpsDrawTime.Length) {
                mFpsDrawTimeIndex = 0;
            }
            mFps = (float)(mFpsDrawTime.Length / (t - mFpsDrawTime[mFpsDrawTimeIndex]));

            int cnt = 0;
            double sum = 0.0;
            for (int i = 0; i < mFpsDrawTime2.Length; i++) {
                double v = mFpsDrawTime2[i];
                if (v > 0.0f) {
                    cnt++;
                }
                sum += v;
            }
            mFps2 = (float)(cnt / sum);
#endif
        }

        /// <summary>
        /// 現在のゲームコントローラのモードに応じてstripLblGameCtrlModeの表示状態を更新します。
        /// </summary>
        public void updateGameControlerStatus(Object sender, EventArgs e)
        {
            if (mGameMode == GameControlMode.DISABLED) {
                stripLblGameCtrlMode.Text = _("Disabled");
                stripLblGameCtrlMode.Image = Properties.Resources.slash;
            } else if (mGameMode == GameControlMode.CURSOR) {
                stripLblGameCtrlMode.Text = _("Cursor");
                stripLblGameCtrlMode.Image = null;
            } else if (mGameMode == GameControlMode.KEYBOARD) {
                stripLblGameCtrlMode.Text = _("Keyboard");
                stripLblGameCtrlMode.Image = Properties.Resources.piano;
            } else if (mGameMode == GameControlMode.NORMAL) {
                stripLblGameCtrlMode.Text = _("Normal");
                stripLblGameCtrlMode.Image = null;
            }
        }

        public int calculateStartToDrawX()
        {
            return (int)(hScroll.Value * controller.getScaleX());
        }

        /// <summary>
        /// 現在選択されている音符よりも1個前方の音符を選択しなおします。
        /// </summary>
        public void selectBackward()
        {
            int count = AppManager.itemSelection.getEventCount();
            if (count <= 0) {
                return;
            }
            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq == null) {
                return;
            }
            int selected = AppManager.getSelected();
            VsqTrack vsq_track = vsq.Track[selected];

            // 選択されている音符のうち、最も前方にあるものがどれかを調べる
            int min_clock = int.MaxValue;
            int internal_id = -1;
            VsqIDType type = VsqIDType.Unknown;
            foreach (var item in AppManager.itemSelection.getEventIterator()) {
                if (item.editing.Clock <= min_clock) {
                    min_clock = item.editing.Clock;
                    internal_id = item.original.InternalID;
                    type = item.original.ID.type;
                }
            }
            if (internal_id == -1 || type == VsqIDType.Unknown) {
                return;
            }

            // 1個前のアイテムのIDを検索
            int last_id = -1;
            int clock = AppManager.getCurrentClock();
            for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                if (item.ID.type != type) {
                    continue;
                }
                if (item.InternalID == internal_id) {
                    break;
                }
                last_id = item.InternalID;
                clock = item.Clock;
            }
            if (last_id == -1) {
                return;
            }

            // 選択しなおす
            AppManager.itemSelection.clearEvent();
            AppManager.itemSelection.addEvent(last_id);
            ensureVisible(clock);
        }

        /// <summary>
        /// 現在選択されている音符よりも1個後方の音符を選択しなおします。
        /// </summary>
        public void selectForward()
        {
            int count = AppManager.itemSelection.getEventCount();
            if (count <= 0) {
                return;
            }
            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq == null) {
                return;
            }
            int selected = AppManager.getSelected();
            VsqTrack vsq_track = vsq.Track[selected];

            // 選択されている音符のうち、最も後方にあるものがどれかを調べる
            int max_clock = int.MinValue;
            int internal_id = -1;
            VsqIDType type = VsqIDType.Unknown;
            foreach (var item in AppManager.itemSelection.getEventIterator()) {
                if (max_clock <= item.editing.Clock) {
                    max_clock = item.editing.Clock;
                    internal_id = item.original.InternalID;
                    type = item.original.ID.type;
                }
            }
            if (internal_id == -1 || type == VsqIDType.Unknown) {
                return;
            }

            // 1個後ろのアイテムのIDを検索
            int last_id = -1;
            int clock = AppManager.getCurrentClock();
            bool break_next = false;
            for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                if (item.ID.type != type) {
                    continue;
                }
                if (item.InternalID == internal_id) {
                    break_next = true;
                    last_id = item.InternalID;
                    clock = item.Clock;
                    continue;
                }
                last_id = item.InternalID;
                clock = item.Clock;
                if (break_next) {
                    break;
                }
            }
            if (last_id == -1) {
                return;
            }

            // 選択しなおす
            AppManager.itemSelection.clearEvent();
            AppManager.itemSelection.addEvent(last_id);
            ensureVisible(clock);
        }

        public void invalidatePictOverview(Object sender, EventArgs e)
        {
            panelOverview.Invalidate();
        }

        public void updateBgmMenuState()
        {
            menuTrackBgm.DropDownItems.Clear();
            int count = AppManager.getBgmCount();
            if (count > 0) {
                for (int i = 0; i < count; i++) {
                    BgmFile item = AppManager.getBgm(i);
                    var menu = new ToolStripMenuItem();
                    menu.Text = PortUtil.getFileName(item.file);
                    menu.ToolTipText = item.file;

                    BgmMenuItem menu_remove = new BgmMenuItem(i);
                    menu_remove.Text = _("Remove");
                    menu_remove.ToolTipText = item.file;
                    menu_remove.Click += new EventHandler(handleBgmRemove_Click);
                    menu.DropDownItems.Add(menu_remove);

                    BgmMenuItem menu_start_after_premeasure = new BgmMenuItem(i);
                    menu_start_after_premeasure.Text = _("Start After Premeasure");
                    menu_start_after_premeasure.Name = "menu_start_after_premeasure" + i;
                    menu_start_after_premeasure.CheckOnClick = true;
                    menu_start_after_premeasure.Checked = item.startAfterPremeasure;
                    menu_start_after_premeasure.CheckedChanged += new EventHandler(handleBgmStartAfterPremeasure_CheckedChanged);
                    menu.DropDownItems.Add(menu_start_after_premeasure);

                    BgmMenuItem menu_offset_second = new BgmMenuItem(i);
                    menu_offset_second.Text = _("Set Offset Seconds");
                    menu_offset_second.ToolTipText = item.readOffsetSeconds + " " + _("seconds");
                    menu_offset_second.Click += new EventHandler(handleBgmOffsetSeconds_Click);
                    menu.DropDownItems.Add(menu_offset_second);

                    menuTrackBgm.DropDownItems.Add(menu);
                }
                menuTrackBgm.DropDownItems.Add(new ToolStripSeparator());
            }
            var menu_add = new ToolStripMenuItem();
            menu_add.Text = _("Add");
            menu_add.Click += new EventHandler(handleBgmAdd_Click);
            menuTrackBgm.DropDownItems.Add(menu_add);
        }


#if ENABLE_PROPERTY
        public void updatePropertyPanelState(PanelState state)
        {
#if DEBUG
            sout.println("FormMain#updatePropertyPanelState; state=" + state);
#endif
            if (state == PanelState.Docked) {
                mPropertyPanelContainer.addComponent(AppManager.propertyPanel);
                menuVisualProperty.Checked = true;
                AppManager.editorConfig.PropertyWindowStatus.State = PanelState.Docked;
                splitContainerProperty.setPanel1Hidden(false);
                splitContainerProperty.setSplitterFixed(false);
                splitContainerProperty.setDividerSize(_SPL_SPLITTER_WIDTH);
                splitContainerProperty.Panel1MinSize = _PROPERTY_DOCK_MIN_WIDTH;
                int w = AppManager.editorConfig.PropertyWindowStatus.DockWidth;
                if (w < _PROPERTY_DOCK_MIN_WIDTH) {
                    w = _PROPERTY_DOCK_MIN_WIDTH;
                }
                splitContainerProperty.setDividerLocation(w);
#if DEBUG
                sout.println("FormMain#updatePropertyPanelState; state=Docked; w=" + w);
#endif
                AppManager.editorConfig.PropertyWindowStatus.WindowState = FormWindowState.Minimized;
                AppManager.propertyWindow.getUi().hideWindow();
            } else if (state == PanelState.Hidden) {
                if (AppManager.propertyWindow.getUi().isVisible()) {
                    AppManager.propertyWindow.getUi().hideWindow();
                }
                menuVisualProperty.Checked = false;
                if (AppManager.editorConfig.PropertyWindowStatus.State == PanelState.Docked) {
                    AppManager.editorConfig.PropertyWindowStatus.DockWidth = splitContainerProperty.getDividerLocation();
                }
                AppManager.editorConfig.PropertyWindowStatus.State = PanelState.Hidden;
                splitContainerProperty.Panel1MinSize = 0;
                splitContainerProperty.setPanel1Hidden(true);
                splitContainerProperty.setDividerLocation(0);
                splitContainerProperty.setDividerSize(0);
                splitContainerProperty.setSplitterFixed(true);
            } else if (state == PanelState.Window) {
                AppManager.propertyWindow.getUi().addComponent(AppManager.propertyPanel);
                var parent = this.Location;
                XmlRectangle rc = AppManager.editorConfig.PropertyWindowStatus.Bounds;
                Point property = new Point(rc.x, rc.y);
                int x = parent.X + property.x;
                int y = parent.Y + property.y;
                int width = rc.width;
                int height = rc.height;
                AppManager.propertyWindow.getUi().setBounds(x, y, width, height);
                int workingAreaX = AppManager.propertyWindow.getUi().getWorkingAreaX();
                int workingAreaY = AppManager.propertyWindow.getUi().getWorkingAreaY();
                int workingAreaWidth = AppManager.propertyWindow.getUi().getWorkingAreaWidth();
                int workingAreaHeight = AppManager.propertyWindow.getUi().getWorkingAreaHeight();
                Point appropriateLocation = getAppropriateDialogLocation(
                    x, y, width, height,
                    workingAreaX, workingAreaY, workingAreaWidth, workingAreaHeight
                );
                AppManager.propertyWindow.getUi().setBounds(appropriateLocation.x, appropriateLocation.y, width, height);
                // setVisible -> NORMALとすると，javaの場合見栄えが悪くなる
                AppManager.propertyWindow.getUi().setVisible(true);
                if (AppManager.propertyWindow.getUi().isWindowMinimized()) {
                    AppManager.propertyWindow.getUi().deiconfyWindow();
                }
                menuVisualProperty.Checked = true;
                if (AppManager.editorConfig.PropertyWindowStatus.State == PanelState.Docked) {
                    AppManager.editorConfig.PropertyWindowStatus.DockWidth = splitContainerProperty.getDividerLocation();
                }
                AppManager.editorConfig.PropertyWindowStatus.State = PanelState.Window;
                splitContainerProperty.Panel1MinSize = 0;
                splitContainerProperty.setPanel1Hidden(true);
                splitContainerProperty.setDividerLocation(0);
                splitContainerProperty.setDividerSize(0);
                splitContainerProperty.setSplitterFixed(true);
                AppManager.editorConfig.PropertyWindowStatus.WindowState = FormWindowState.Normal;
            }
        }
#endif


        /// <summary>
        /// メインメニュー項目の中から，Nameプロパティがnameであるものを検索します．見つからなければnullを返す．
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public Object searchMenuItemFromName(string name, ByRef<Object> parent)
        {
            int count = menuStripMain.Items.Count;
            for (int i = 0; i < count; i++) {
                Object tsi = menuStripMain.Items[i];
                Object ret = searchMenuItemRecurse(name, tsi, parent);
                if (ret != null) {
                    if (parent.value == null) {
                        parent.value = tsi;
                    }
                    return ret;
                }
            }
            return null;
        }

        /// <summary>
        /// 指定されたメニューアイテムから，Nameプロパティがnameであるものを再帰的に検索します．見つからなければnullを返す
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        public Object searchMenuItemRecurse(string name, Object tree, ByRef<Object> parent)
        {
            string tree_name = "";
            System.Windows.Forms.ToolStripMenuItem menu = null;
            if (tree is System.Windows.Forms.ToolStripItem) {
                if (tree is System.Windows.Forms.ToolStripMenuItem) {
                    menu = (System.Windows.Forms.ToolStripMenuItem)tree;
                }
                tree_name = ((System.Windows.Forms.ToolStripItem)tree).Name;
            } else {
                return null;
            }

            if (tree_name == name) {
                parent.value = null;
                return tree;
            } else {
                if (menu == null) {
                    return null;
                }
                int count = menu.DropDownItems.Count;
                for (int i = 0; i < count; i++) {
                    System.Windows.Forms.ToolStripItem tsi = menu.DropDownItems[i];
                    string tsi_name = "";
                    if (tsi is System.Windows.Forms.ToolStripItem) {
                        tsi_name = ((System.Windows.Forms.ToolStripItem)tsi).Name;
                    } else {
                        continue;
                    }

                    if (tsi_name == name) {
                        return tsi;
                    }
                    Object ret = searchMenuItemRecurse(name, tsi, parent);
                    if (ret != null) {
                        parent.value = tsi;
                        return ret;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// フォームをマウス位置に出す場合に推奨されるフォーム位置を計算します
        /// </summary>
        /// <param name="dlg"></param>
        /// <returns></returns>
        public System.Drawing.Point getFormPreferedLocation(int dialogWidth, int dialogHeight)
        {
            Point mouse = PortUtil.getMousePosition();
            Rectangle rcScreen = PortUtil.getWorkingArea(this);
            int top = mouse.y - dialogHeight / 2;
            if (top + dialogHeight > rcScreen.y + rcScreen.height) {
                // ダイアログの下端が隠れる場合、位置をずらす
                top = rcScreen.y + rcScreen.height - dialogHeight;
            }
            if (top < rcScreen.y) {
                // ダイアログの上端が隠れる場合、位置をずらす
                top = rcScreen.y;
            }
            int left = mouse.x - dialogWidth / 2;
            if (left + dialogWidth > rcScreen.x + rcScreen.width) {
                // ダイアログの右端が隠れる場合，位置をずらす
                left = rcScreen.x + rcScreen.width - dialogWidth;
            }
            if (left < rcScreen.x) {
                // ダイアログの左端が隠れる場合，位置をずらす
                left = rcScreen.x;
            }
            return new System.Drawing.Point(left, top);
        }

        /// <summary>
        /// フォームをマウス位置に出す場合に推奨されるフォーム位置を計算します
        /// </summary>
        /// <param name="dlg"></param>
        /// <returns></returns>
        public System.Drawing.Point getFormPreferedLocation(Form dlg)
        {
            return getFormPreferedLocation(dlg.Width, dlg.Height);
        }

        public void updateLayout()
        {
            int width = panel1.Width;
            int height = panel1.Height;

            if (AppManager.editorConfig.OverviewEnabled) {
                panelOverview.Height = _OVERVIEW_HEIGHT;
            } else {
                panelOverview.Height = 0;
            }
            panelOverview.Width = width;
            int key_width = AppManager.keyWidth;

            /*btnMooz.setBounds( 3, 12, 23, 23 );
            btnZoom.setBounds( 26, 12, 23, 23 );*/

            picturePositionIndicator.Width = width;
            picturePositionIndicator.Height = _PICT_POSITION_INDICATOR_HEIGHT;

            hScroll.Top = 0;
            hScroll.Left = key_width;
            hScroll.Width = width - key_width - _SCROLL_WIDTH - trackBar.Width;
            hScroll.Height = _SCROLL_WIDTH;

            vScroll.Width = _SCROLL_WIDTH;
            vScroll.Height = height - _PICT_POSITION_INDICATOR_HEIGHT - _SCROLL_WIDTH * 4 - panelOverview.Height;

            pictPianoRoll.Width = width - _SCROLL_WIDTH;
            pictPianoRoll.Height = height - _PICT_POSITION_INDICATOR_HEIGHT - _SCROLL_WIDTH - panelOverview.Height;

            pictureBox3.Width = key_width - _SCROLL_WIDTH;
            pictKeyLengthSplitter.Width = _SCROLL_WIDTH;
            pictureBox3.Height = _SCROLL_WIDTH;
            pictureBox2.Height = _SCROLL_WIDTH * 4;
            trackBar.Height = _SCROLL_WIDTH;

            panelOverview.Top = 0;
            panelOverview.Left = 0;

            picturePositionIndicator.Top = panelOverview.Height;
            picturePositionIndicator.Left = 0;

            pictPianoRoll.Top = _PICT_POSITION_INDICATOR_HEIGHT + panelOverview.Height;
            pictPianoRoll.Left = 0;

            vScroll.Top = _PICT_POSITION_INDICATOR_HEIGHT + panelOverview.Height;
            vScroll.Left = width - _SCROLL_WIDTH;

            pictureBox3.Top = height - _SCROLL_WIDTH;
            pictureBox3.Left = 0;
            pictKeyLengthSplitter.Top = height - _SCROLL_WIDTH;
            pictKeyLengthSplitter.Left = pictureBox3.Width;

            hScroll.Top = height - _SCROLL_WIDTH;
            hScroll.Left = pictureBox3.Width + pictKeyLengthSplitter.Width;

            trackBar.Top = height - _SCROLL_WIDTH;
            trackBar.Left = width - _SCROLL_WIDTH - trackBar.Width;

            pictureBox2.Top = height - _SCROLL_WIDTH * 4;
            pictureBox2.Left = width - _SCROLL_WIDTH;

            waveView.Top = 0;
            waveView.Left = key_width;
            waveView.Width = width - key_width;
            waveView.Height = panelWaveformZoom.Height;
        }

        public void updateRendererMenu()
        {
            renderer_menu_handler_.ForEach((handler) => handler.updateRendererAvailability(AppManager.editorConfig));

            // UTAU用のサブアイテムを更新
            int count = AppManager.editorConfig.getResamplerCount();
            // サブアイテムの個数を整える
            int delta = count - menuTrackRendererUtau.DropDownItems.Count;
            if (delta > 0) {
                // 増やす
                for (int i = 0; i < delta; i++) {
                    cMenuTrackTabRendererUtau.DropDownItems.Add("", null, new EventHandler(handleChangeRenderer));
                    menuTrackRendererUtau.DropDownItems.Add("", null, new EventHandler(handleChangeRenderer));
                }
            } else if (delta < 0) {
                // 減らす
                for (int i = 0; i < -delta; i++) {
                    cMenuTrackTabRendererUtau.DropDownItems.RemoveAt(0);
                    menuTrackRendererUtau.DropDownItems.RemoveAt(0);
                }
            }

            for (int i = 0; i < count; i++) {
                string path = AppManager.editorConfig.getResamplerAt(i);
                string name = PortUtil.getFileNameWithoutExtension(path);
                menuTrackRendererUtau.DropDownItems[i].Text = name;
                cMenuTrackTabRendererUtau.DropDownItems[i].Text = name;

                menuTrackRendererUtau.DropDownItems[i].ToolTipText = path;
                cMenuTrackTabRendererUtau.DropDownItems[i].ToolTipText = path;
            }
        }

        public void drawUtauVibrato(Graphics2D g, UstVibrato vibrato, int note, int clock_start, int clock_width)
        {
            //SmoothingMode old = g.SmoothingMode;
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            // 魚雷を描いてみる
            int y0 = AppManager.yCoordFromNote(note - 0.5f);
            int x0 = AppManager.xCoordFromClocks(clock_start);
            int px_width = AppManager.xCoordFromClocks(clock_start + clock_width) - x0;
            int boxheight = (int)(vibrato.Depth * 2 / 100.0 * (int)(100.0 * controller.getScaleY()));
            int px_shift = (int)(vibrato.Shift / 100.0 * vibrato.Depth / 100.0 * (int)(100.0 * controller.getScaleY()));

            // vibrato in
            int cl_vibin_end = clock_start + (int)(clock_width * vibrato.In / 100.0);
            int x_vibin_end = AppManager.xCoordFromClocks(cl_vibin_end);
            Point ul = new Point(x_vibin_end, y0 - boxheight / 2 - px_shift);
            Point dl = new Point(x_vibin_end, y0 + boxheight / 2 - px_shift);
            g.setColor(Color.black);
            g.drawPolyline(new int[] { x0, ul.x, dl.x },
                            new int[] { y0, ul.y, dl.y },
                            3);

            // vibrato out
            int cl_vibout_start = clock_start + clock_width - (int)(clock_width * vibrato.Out / 100.0);
            int x_vibout_start = AppManager.xCoordFromClocks(cl_vibout_start);
            Point ur = new Point(x_vibout_start, y0 - boxheight / 2 - px_shift);
            Point dr = new Point(x_vibout_start, y0 + boxheight / 2 - px_shift);
            g.drawPolyline(new int[] { x0 + px_width, ur.x, dr.x },
                           new int[] { y0, ur.y, dr.y },
                           3);

            // box
            int boxwidth = x_vibout_start - x_vibin_end;
            if (boxwidth > 0) {
                g.drawPolyline(new int[] { ul.x, dl.x, dr.x, ur.x },
                               new int[] { ul.y, dl.y, dr.y, ur.y },
                               4);
            }

            // buf1に、vibrato in/outによる増幅率を代入
            float[] buf1 = new float[clock_width + 1];
            for (int clock = clock_start; clock <= clock_start + clock_width; clock++) {
                buf1[clock - clock_start] = 1.0f;
            }
            // vibin
            if (cl_vibin_end - clock_start > 0) {
                for (int clock = clock_start; clock <= cl_vibin_end; clock++) {
                    int i = clock - clock_start;
                    buf1[i] *= i / (float)(cl_vibin_end - clock_start);
                }
            }
            if (clock_start + clock_width - cl_vibout_start > 0) {
                for (int clock = clock_start + clock_width; clock >= cl_vibout_start; clock--) {
                    int i = clock - clock_start;
                    float v = (clock_start + clock_width - clock) / (float)(clock_start + clock_width - cl_vibout_start);
                    buf1[i] = buf1[i] * v;
                }
            }

            // buf2に、shiftによるy座標のシフト量を代入
            float[] buf2 = new float[clock_width + 1];
            for (int i = 0; i < clock_width; i++) {
                buf2[i] = px_shift * buf1[i];
            }
            try {
                double phase = 2.0 * Math.PI * vibrato.Phase / 100.0;
                double omega = 2.0 * Math.PI / vibrato.Period;   //角速度(rad/msec)
                double msec = AppManager.getVsqFile().getSecFromClock(clock_start - 1) * 1000.0;
                float px_track_height = (int)(controller.getScaleY() * 100.0f);
                phase -= (AppManager.getVsqFile().getSecFromClock(clock_start) * 1000.0 - msec) * omega;
                for (int clock = clock_start; clock <= clock_start + clock_width; clock++) {
                    int i = clock - clock_start;
                    double t_msec = AppManager.getVsqFile().getSecFromClock(clock) * 1000.0;
                    phase += (t_msec - msec) * omega;
                    msec = t_msec;
                    buf2[i] += (float)(vibrato.Depth * 0.01f * px_track_height * buf1[i] * Math.Sin(phase));
                }
                int[] listx = new int[clock_width + 1];
                int[] listy = new int[clock_width + 1];
                for (int clock = clock_start; clock <= clock_start + clock_width; clock++) {
                    int i = clock - clock_start;
                    listx[i] = AppManager.xCoordFromClocks(clock);
                    listy[i] = (int)(y0 + buf2[i]);
                }
                if (listx.Length >= 2) {
                    g.setColor(Color.red);
                    g.drawPolyline(listx, listy, listx.Length);
                }
                //g.SmoothingMode = old;
            } catch (Exception oex) {
                Logger.write(typeof(FormMain) + ".drawUtauVibato; ex=" + oex + "\n");
#if DEBUG
                AppManager.debugWriteLine("DrawUtauVibrato; oex=" + oex);
#endif
            }
        }

#if ENABLE_SCRIPT
        /// <summary>
        /// Palette Toolの表示を更新します
        /// </summary>
        public void updatePaletteTool()
        {
            int count = 0;
            int num_has_dialog = 0;
            foreach (var item in mPaletteTools) {
                toolBarTool.Buttons.Add(item);
            }
            string lang = Messaging.getLanguage();
            bool first = true;
            foreach (var id in PaletteToolServer.loadedTools.Keys) {
                count++;
                IPaletteTool ipt = (IPaletteTool)PaletteToolServer.loadedTools[id];
                System.Drawing.Bitmap icon = ipt.getIcon();
                string name = ipt.getName(lang);
                string desc = ipt.getDescription(lang);

                // toolStripPaletteTools
                System.Windows.Forms.ToolBarButton tsb = new System.Windows.Forms.ToolBarButton();
                tsb.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
                if (icon != null) {
                    imageListTool.Images.Add(icon);
                    tsb.ImageIndex = imageListTool.Images.Count - 1;
                }
                tsb.Text = name;
                tsb.ToolTipText = desc;
                tsb.Tag = id;
                if (first) {
                    var sep = new System.Windows.Forms.ToolBarButton();
                    sep.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
                    toolBarTool.Buttons.Add(sep);
                    first = false;
                }
                mPaletteTools.Add(tsb);
                toolBarTool.Buttons.Add(tsb);

                // cMenuTrackSelector
                PaletteToolMenuItem tsmi = new PaletteToolMenuItem(id);
                tsmi.Text = name;
                tsmi.ToolTipText = desc;
                tsmi.Click += new EventHandler(handleStripPaletteTool_Click);
                cMenuTrackSelectorPaletteTool.DropDownItems.Add(tsmi);

                // cMenuPiano
                PaletteToolMenuItem tsmi2 = new PaletteToolMenuItem(id);
                tsmi2.Text = name;
                tsmi2.ToolTipText = desc;
                tsmi2.Click += new EventHandler(handleStripPaletteTool_Click);
                cMenuPianoPaletteTool.DropDownItems.Add(tsmi2);

                // menuSettingPaletteTool
                if (ipt.hasDialog()) {
                    PaletteToolMenuItem tsmi3 = new PaletteToolMenuItem(id);
                    tsmi3.Text = name;
                    tsmi3.Click += new EventHandler(handleSettingPaletteTool);
                    menuSettingPaletteTool.DropDownItems.Add(tsmi3);
                    num_has_dialog++;
                }
            }
            if (count == 0) {
                cMenuTrackSelectorPaletteTool.Visible = false;
                cMenuPianoPaletteTool.Visible = false;
            }
            if (num_has_dialog == 0) {
                menuSettingPaletteTool.Visible = false;
            }
        }
#endif

        public void updateCopyAndPasteButtonStatus()
        {
            // copy cut deleteの表示状態更新
            bool selected_is_null = (AppManager.itemSelection.getEventCount() == 0) &&
                                       (AppManager.itemSelection.getTempoCount() == 0) &&
                                       (AppManager.itemSelection.getTimesigCount() == 0) &&
                                       (AppManager.itemSelection.getPointIDCount() == 0);

            int selected_point_id_count = AppManager.itemSelection.getPointIDCount();
            cMenuTrackSelectorCopy.Enabled = selected_point_id_count > 0;
            cMenuTrackSelectorCut.Enabled = selected_point_id_count > 0;
            cMenuTrackSelectorDeleteBezier.Enabled = (AppManager.isCurveMode() && AppManager.itemSelection.getLastBezier() != null);
            if (selected_point_id_count > 0) {
                cMenuTrackSelectorDelete.Enabled = true;
            } else {
                SelectedEventEntry last = AppManager.itemSelection.getLastEvent();
                if (last == null) {
                    cMenuTrackSelectorDelete.Enabled = false;
                } else {
                    cMenuTrackSelectorDelete.Enabled = last.original.ID.type == VsqIDType.Singer;
                }
            }

            cMenuPianoCopy.Enabled = !selected_is_null;
            cMenuPianoCut.Enabled = !selected_is_null;
            cMenuPianoDelete.Enabled = !selected_is_null;

            menuEditCopy.Enabled = !selected_is_null;
            menuEditCut.Enabled = !selected_is_null;
            menuEditDelete.Enabled = !selected_is_null;

            ClipboardEntry ce = AppManager.clipboard.getCopiedItems();
            int copy_started_clock = ce.copyStartedClock;
            SortedDictionary<CurveType, VsqBPList> copied_curve = ce.points;
            SortedDictionary<CurveType, List<BezierChain>> copied_bezier = ce.beziers;
            bool copied_is_null = (ce.events.Count == 0) &&
                                  (ce.tempo.Count == 0) &&
                                  (ce.timesig.Count == 0) &&
                                  (copied_curve.Count == 0) &&
                                  (copied_bezier.Count == 0);
            bool enabled = !copied_is_null;
            if (copied_curve.Count == 1) {
                // 1種類のカーブがコピーされている場合→コピーされているカーブの種類と、現在選択されているカーブの種類とで、最大値と最小値が一致している場合のみ、ペースト可能
                CurveType ct = CurveType.Empty;
                foreach (var c in copied_curve.Keys) {
                    ct = c;
                }
                CurveType selected = trackSelector.getSelectedCurve();
                if (ct.getMaximum() == selected.getMaximum() &&
                     ct.getMinimum() == selected.getMinimum() &&
                     !selected.isScalar() && !selected.isAttachNote()) {
                    enabled = true;
                } else {
                    enabled = false;
                }
            } else if (copied_curve.Count >= 2) {
                // 複数種類のカーブがコピーされている場合→そのままペーストすればOK
                enabled = true;
            }
            cMenuTrackSelectorPaste.Enabled = enabled;
            cMenuPianoPaste.Enabled = enabled;
            menuEditPaste.Enabled = enabled;

            /*int copy_started_clock;
            bool copied_is_null = (AppManager.GetCopiedEvent().Count == 0) &&
                                  (AppManager.GetCopiedTempo( out copy_started_clock ).Count == 0) &&
                                  (AppManager.GetCopiedTimesig( out copy_started_clock ).Count == 0) &&
                                  (AppManager.GetCopiedCurve( out copy_started_clock ).Count == 0) &&
                                  (AppManager.GetCopiedBezier( out copy_started_clock ).Count == 0);
            menuEditCut.isEnabled() = !selected_is_null;
            menuEditCopy.isEnabled() = !selected_is_null;
            menuEditDelete.isEnabled() = !selected_is_null;
            //stripBtnCopy.isEnabled() = !selected_is_null;
            //stripBtnCut.isEnabled() = !selected_is_null;

            if ( AppManager.GetCopiedEvent().Count != 0 ) {
                menuEditPaste.isEnabled() = (AppManager.CurrentClock >= AppManager.VsqFile.getPreMeasureClocks());
                //stripBtnPaste.isEnabled() = (AppManager.CurrentClock >= AppManager.VsqFile.getPreMeasureClocks());
            } else {
                menuEditPaste.isEnabled() = !copied_is_null;
                //stripBtnPaste.isEnabled() = !copied_is_null;
            }*/
        }

        /// <summary>
        /// 現在の編集データを全て破棄する。DirtyCheckは行われない。
        /// </summary>
        public void clearExistingData()
        {
            AppManager.editHistory.clear();
            AppManager.itemSelection.clearBezier();
            AppManager.itemSelection.clearEvent();
            AppManager.itemSelection.clearTempo();
            AppManager.itemSelection.clearTimesig();
            if (AppManager.isPlaying()) {
                AppManager.setPlaying(false, this);
            }
            waveView.unloadAll();
        }

        /// <summary>
        /// 保存されていない編集内容があるかどうかチェックし、必要なら確認ダイアログを出す。
        /// </summary>
        /// <returns>保存されていない保存内容などない場合、または、保存する必要がある場合で（保存しなくてよいと指定された場合または保存が行われた場合）にtrueを返す</returns>
        public bool dirtyCheck()
        {
            if (mEdited) {
                string file = AppManager.getFileName();
                if (file == "") {
                    file = "Untitled";
                } else {
                    file = PortUtil.getFileName(file);
                }
                DialogResult dr = AppManager.showMessageBox(_("Save this sequence?"),
                                                              _("Affirmation"),
                                                              cadencii.windows.forms.Utility.MSGBOX_YES_NO_CANCEL_OPTION,
                                                              cadencii.windows.forms.Utility.MSGBOX_QUESTION_MESSAGE);
                if (dr == DialogResult.Yes) {
                    if (AppManager.getFileName() == "") {
                        var dr2 = AppManager.showModalDialog(saveXmlVsqDialog, false, this);
                        if (dr2 == System.Windows.Forms.DialogResult.OK) {
                            string sf = saveXmlVsqDialog.FileName;
                            AppManager.saveTo(sf);
                            return true;
                        } else {
                            return false;
                        }
                    } else {
                        AppManager.saveTo(AppManager.getFileName());
                        return true;
                    }
                } else if (dr == DialogResult.No) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        }

        public void loadWave(Object arg)
        {
            Object[] argArr = (Object[])arg;
            string file = (string)argArr[0];
            int track = (int)argArr[1];
            waveView.load(track, file);
        }

        /// <summary>
        /// AppManager.editorConfig.ViewWaveformの値をもとに、splitterContainer2の表示状態を更新します
        /// </summary>
        public void updateSplitContainer2Size(bool save_to_config)
        {
            if (AppManager.editorConfig.ViewWaveform) {
                splitContainer2.setPanel2MinSize(_SPL2_PANEL2_MIN_HEIGHT);
                splitContainer2.setSplitterFixed(false);
                splitContainer2.setPanel2Hidden(false);
                splitContainer2.setDividerSize(_SPL_SPLITTER_WIDTH);
                int lastloc = AppManager.editorConfig.SplitContainer2LastDividerLocation;
                if (lastloc <= 0 || lastloc > splitContainer2.getHeight()) {
                    int draft = splitContainer2.getHeight() - 100;
                    if (draft <= 0) {
                        draft = splitContainer2.getHeight() / 2;
                    }
                    splitContainer2.setDividerLocation(draft);
                } else {
                    splitContainer2.setDividerLocation(lastloc);
                }
            } else {
                if (save_to_config) {
                    AppManager.editorConfig.SplitContainer2LastDividerLocation = splitContainer2.getDividerLocation();
                }
                splitContainer2.setPanel2MinSize(0);
                splitContainer2.setPanel2Hidden(true);
                splitContainer2.setDividerSize(0);
                splitContainer2.setDividerLocation(splitContainer2.getHeight());
                splitContainer2.setSplitterFixed(true);
            }
        }

        /// <summary>
        /// ウィンドウの表示内容に応じて、ウィンドウサイズの最小値を計算します
        /// </summary>
        /// <returns></returns>
        public Dimension getWindowMinimumSize()
        {
            Dimension current_minsize = new Dimension(MinimumSize.Width, MinimumSize.Height);
            Dimension client = new Dimension(this.ClientSize.Width, this.ClientSize.Height);
            Dimension current = new Dimension(this.Size.Width, this.Size.Height);
            return new Dimension(current_minsize.width,
                                  splitContainer1.getPanel2MinSize() +
                                  _SCROLL_WIDTH + _PICT_POSITION_INDICATOR_HEIGHT + pictPianoRoll.getMinimumSize().height +
                                  rebar.Height +
                                  menuStripMain.Height + statusStrip.Height +
                                  (current.height - client.height) +
                                  20);
        }

        /// <summary>
        /// 現在のAppManager.mInputTextBoxの状態を元に、歌詞の変更を反映させるコマンドを実行します
        /// </summary>
        public void executeLyricChangeCommand()
        {
            if (!AppManager.mInputTextBox.Enabled) {
                return;
            }
            if (AppManager.mInputTextBox.IsDisposed) {
                return;
            }
            SelectedEventEntry last_selected_event = AppManager.itemSelection.getLastEvent();
            bool phonetic_symbol_edit_mode = AppManager.mInputTextBox.isPhoneticSymbolEditMode();

            int selected = AppManager.getSelected();
            VsqFileEx vsq = AppManager.getVsqFile();
            VsqTrack vsq_track = vsq.Track[selected];

            // 後続に、連続している音符が何個あるか検査
            int maxcount = SymbolTable.getMaxDivisions(); // 音節の分割によって，高々maxcount個までにしか分割されない
            bool check_started = false;
            int endclock = 0;  // 直前の音符の終了クロック
            int count = 0;     // 後続音符の連続個数
            int start_index = -1;
            int indx = -1;
            for (Iterator<int> itr = vsq_track.indexIterator(IndexIteratorKind.NOTE); itr.hasNext(); ) {
                indx = itr.next();
                VsqEvent itemi = vsq_track.getEvent(indx);
                if (itemi.InternalID == last_selected_event.original.InternalID) {
                    check_started = true;
                    endclock = itemi.Clock + itemi.ID.getLength();
                    count = 1;
                    start_index = indx;
                    continue;
                }
                if (check_started) {
                    if (count + 1 > maxcount) {
                        break;
                    }
                    if (itemi.Clock <= endclock) {
                        count++;
                        endclock = itemi.Clock + itemi.ID.getLength();
                    } else {
                        break;
                    }
                }
            }

            // 後続の音符をリストアップ
            VsqEvent[] items = new VsqEvent[count];
            string[] original_symbol = new string[count];
            string[] original_phrase = new string[count];
            bool[] symbol_protected = new bool[count];
            indx = -1;
            for (Iterator<int> itr = vsq_track.indexIterator(IndexIteratorKind.NOTE); itr.hasNext(); ) {
                int index = itr.next();
                if (index < start_index) {
                    continue;
                }
                indx++;
                if (count <= indx) {
                    break;
                }
                VsqEvent ve = vsq_track.getEvent(index);
                items[indx] = (VsqEvent)ve.clone();
                original_symbol[indx] = ve.ID.LyricHandle.L0.getPhoneticSymbol();
                original_phrase[indx] = ve.ID.LyricHandle.L0.Phrase;
                symbol_protected[indx] = ve.ID.LyricHandle.L0.PhoneticSymbolProtected;
            }

#if DEBUG
            AppManager.debugWriteLine("    original_phase,symbol=" + original_phrase + "," + original_symbol[0]);
            AppManager.debugWriteLine("    phonetic_symbol_edit_mode=" + phonetic_symbol_edit_mode);
            AppManager.debugWriteLine("    AppManager.mInputTextBox.setText(=" + AppManager.mInputTextBox.Text);
#endif
            string[] phrase = new string[count];
            string[] phonetic_symbol = new string[count];
            for (int i = 0; i < count; i++) {
                phrase[i] = original_phrase[i];
                phonetic_symbol[i] = original_symbol[i];
            }
            string txt = AppManager.mInputTextBox.Text;
            int txtlen = PortUtil.getStringLength(txt);
            if (txtlen > 0) {
                // 1文字目は，UTAUの連続音入力のハイフンの可能性があるので，無駄に置換されるのを回避
                phrase[0] = txt.Substring(0, 1) + ((txtlen > 1) ? txt.Substring(1).Replace("-", "") : "");
            } else {
                phrase[0] = "";
            }
            if (!phonetic_symbol_edit_mode) {
                // 歌詞を編集するモードで、
                if (AppManager.editorConfig.SelfDeRomanization) {
                    // かつローマ字の入力を自動でひらがなに展開する設定だった場合。
                    // ローマ字をひらがなに展開
                    phrase[0] = KanaDeRomanization.Attach(phrase[0]);
                }
            }

            // 発音記号または歌詞が変更された場合の処理
            if ((phonetic_symbol_edit_mode && AppManager.mInputTextBox.Text != original_symbol[0]) ||
                 (!phonetic_symbol_edit_mode && phrase[0] != original_phrase[0])) {
                if (phonetic_symbol_edit_mode) {
                    // 発音記号を編集するモード
                    phrase[0] = AppManager.mInputTextBox.getBufferText();
                    phonetic_symbol[0] = AppManager.mInputTextBox.Text;

                    // 入力された発音記号のうち、有効なものだけをピックアップ
                    string[] spl = PortUtil.splitString(phonetic_symbol[0], new char[] { ' ' }, true);
                    List<string> list = new List<string>();
                    for (int i = 0; i < spl.Length; i++) {
                        string s = spl[i];
                        if (VsqPhoneticSymbol.isValidSymbol(s)) {
                            list.Add(s);
                        }
                    }

                    // ピックアップした発音記号をスペース区切りで結合
                    phonetic_symbol[0] = "";
                    bool first = true;
                    foreach (var s in list) {
                        if (first) {
                            phonetic_symbol[0] += s;
                            first = false;
                        } else {
                            phonetic_symbol[0] += " " + s;
                        }
                    }

                    // 発音記号を編集すると、自動で「発音記号をプロテクトする」モードになるよ
                    symbol_protected[0] = true;
                } else {
                    // 歌詞を編集するモード
                    if (!symbol_protected[0]) {
                        // 発音記号をプロテクトしない場合、歌詞から発音記号を引当てる
                        SymbolTableEntry entry = SymbolTable.attatch(phrase[0]);
                        if (entry == null) {
                            phonetic_symbol[0] = "a";
                        } else {
                            phonetic_symbol[0] = entry.getSymbol();
                            // 分節の分割記号'-'が入っている場合
#if DEBUG
                            sout.println("FormMain#executeLyricChangeCommand; word=" + entry.Word + "; symbol=" + entry.getSymbol() + "; rawSymbol=" + entry.getRawSymbol());
#endif
                            if (entry.Word.IndexOf('-') >= 0) {
                                string[] spl_phrase = PortUtil.splitString(entry.Word, '\t');
                                if (spl_phrase.Length <= count) {
                                    // 分節の分割数が，後続の音符数と同じか少ない場合
                                    string[] spl_symbol = PortUtil.splitString(entry.getRawSymbol(), '\t');
                                    for (int i = 0; i < spl_phrase.Length; i++) {
                                        phrase[i] = spl_phrase[i];
                                        phonetic_symbol[i] = spl_symbol[i];
                                    }
                                } else {
                                    // 後続の音符の個数が足りない
                                    phrase[0] = entry.Word.Replace("\t", "");
                                }
                            }
                        }
                    } else {
                        // 発音記号をプロテクトする場合、発音記号は最初のやつを使う
                        phonetic_symbol[0] = original_symbol[0];
                    }
                }
#if DEBUG
                AppManager.debugWriteLine("    phrase,phonetic_symbol=" + phrase + "," + phonetic_symbol);
#endif

                for (int j = 0; j < count; j++) {
                    if (phonetic_symbol_edit_mode) {
                        items[j].ID.LyricHandle.L0.setPhoneticSymbol(phonetic_symbol[j]);
                    } else {
                        items[j].ID.LyricHandle.L0.Phrase = phrase[j];
                        items[j].ID.LyricHandle.L0.setPhoneticSymbol(phonetic_symbol[j]);
                        AppManager.applyUtauParameter(vsq_track, items[j]);
                    }
                    if (original_symbol[j] != phonetic_symbol[j]) {
                        List<string> spl = items[j].ID.LyricHandle.L0.getPhoneticSymbolList();
                        List<int> adjustment = new List<int>();
                        for (int i = 0; i < spl.Count; i++) {
                            string s = spl[i];
                            adjustment.Add(VsqPhoneticSymbol.isConsonant(s) ? 64 : 0);
                        }
                        items[j].ID.LyricHandle.L0.setConsonantAdjustmentList(adjustment);
                    }
                }

                CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventReplaceRange(selected, items));
                AppManager.editHistory.register(vsq.executeCommand(run));
                setEdited(true);
            }
        }

        /// <summary>
        /// 識別済みのゲームコントローラを取り外します
        /// </summary>
        public void removeGameControler()
        {
            if (mTimer != null) {
                mTimer.Stop();
                mTimer.Dispose();
                mTimer = null;
            }
            mGameMode = GameControlMode.DISABLED;
            updateGameControlerStatus(null, null);
        }

        /// <summary>
        /// PCに接続されているゲームコントローラを識別・接続します
        /// </summary>
        public void loadGameControler()
        {
            try {
                bool init_success = false;
                int num_joydev = winmmhelp.JoyInit();
                if (num_joydev <= 0) {
                    init_success = false;
                } else {
                    init_success = true;
                }
                if (init_success) {
                    mGameMode = GameControlMode.NORMAL;
                    stripLblGameCtrlMode.Image = null;
                    stripLblGameCtrlMode.Text = mGameMode.ToString();
                    mTimer = new System.Windows.Forms.Timer();
                    mTimer.Interval = 10;
                    mTimer.Tick += new EventHandler(mTimer_Tick);
                    mTimer.Start();
                } else {
                    mGameMode = GameControlMode.DISABLED;
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".loadGameControler; ex=" + ex + "\n");
                mGameMode = GameControlMode.DISABLED;
#if DEBUG
                AppManager.debugWriteLine("FormMain+ReloadGameControler");
                AppManager.debugWriteLine("    ex=" + ex);
#endif
            }
            updateGameControlerStatus(null, null);
        }

#if ENABLE_MIDI
        /// <summary>
        /// MIDI入力句デバイスを再読込みします
        /// </summary>
        public void reloadMidiIn()
        {
            if (mMidiIn != null) {
                mMidiIn.MidiReceived -= new MidiReceivedEventHandler(mMidiIn_MidiReceived);
                mMidiIn.close();
                mMidiIn = null;
            }
            int portNumber = AppManager.editorConfig.MidiInPort.PortNumber;
            int portNumberMtc = AppManager.editorConfig.MidiInPortMtc.PortNumber;
#if DEBUG
            sout.println("FormMain#reloadMidiIn; portNumber=" + portNumber + "; portNumberMtc=" + portNumberMtc);
#endif
            try {
                mMidiIn = new MidiInDevice(portNumber);
                mMidiIn.MidiReceived += new MidiReceivedEventHandler(mMidiIn_MidiReceived);
#if ENABLE_MTC
                if ( portNumber == portNumberMtc ) {
                    m_midi_in.setReceiveSystemCommonMessage( true );
                    m_midi_in.setReceiveSystemRealtimeMessage( true );
                    m_midi_in.MidiReceived += handleMtcMidiReceived;
                    m_midi_in.Start();
                } else {
                    m_midi_in.setReceiveSystemCommonMessage( false );
                    m_midi_in.setReceiveSystemRealtimeMessage( false );
                }
#else
                mMidiIn.setReceiveSystemCommonMessage(false);
                mMidiIn.setReceiveSystemRealtimeMessage(false);
#endif
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".reloadMidiIn; ex=" + ex + "\n");
                serr.println("FormMain#reloadMidiIn; ex=" + ex);
            }

#if ENABLE_MTC
            if ( m_midi_in_mtc != null ) {
                m_midi_in_mtc.MidiReceived -= handleMtcMidiReceived;
                m_midi_in_mtc.Dispose();
                m_midi_in_mtc = null;
            }
            if ( portNumber != portNumberMtc ) {
                try {
                    m_midi_in_mtc = new MidiInDevice( AppManager.editorConfig.MidiInPortMtc.PortNumber );
                    m_midi_in_mtc.MidiReceived += handleMtcMidiReceived;
                    m_midi_in_mtc.setReceiveSystemCommonMessage( true );
                    m_midi_in_mtc.setReceiveSystemRealtimeMessage( true );
                    m_midi_in_mtc.Start();
                } catch ( Exception ex ) {
                    Logger.write( typeof( FormMain ) + ".reloadMidiIn; ex=" + ex + "\n" );
                    serr.println( "FormMain#reloadMidiIn; ex=" + ex );
                }
            }
#endif
            updateMidiInStatus();
        }
#endif

#if ENABLE_MIDI
        public void updateMidiInStatus()
        {
            int midiport = AppManager.editorConfig.MidiInPort.PortNumber;
            List<MidiDevice.Info> devices = new List<MidiDevice.Info>();
            foreach (MidiDevice.Info info in MidiSystem.getMidiDeviceInfo()) {
                MidiDevice device = null;
                try {
                    device = MidiSystem.getMidiDevice(info);
                } catch (Exception ex) {
                    device = null;
                }
                if (device == null) continue;
                int max = device.getMaxTransmitters();
                if (max > 0 || max == -1) {
                    devices.Add(info);
                }
            }
            if (midiport < 0 || devices.Count <= 0) {
                stripLblMidiIn.Text = _("Disabled");
                stripLblMidiIn.Image = Properties.Resources.slash;
            } else {
                if (midiport >= devices.Count) {
                    midiport = 0;
                    AppManager.editorConfig.MidiInPort.PortNumber = midiport;
                }
                stripLblMidiIn.Text = devices[midiport].getName();
                stripLblMidiIn.Image = Properties.Resources.piano;
            }
        }
#endif

#if ENABLE_SCRIPT
        /// <summary>
        /// スクリプトフォルダ中のスクリプトへのショートカットを作成する
        /// </summary>
        public void updateScriptShortcut()
        {
            // 既存のアイテムを削除
            menuScript.DropDownItems.Clear();
            // スクリプトをリロード
            ScriptServer.reload();

            // スクリプトごとのメニューを追加
            int count = 0;
            foreach (var id in ScriptServer.getScriptIdIterator()) {
                if (PortUtil.getFileNameWithoutExtension(id).ToLower() == "runonce") {
                    continue;
                }
                string display = ScriptServer.getDisplayName(id);
                // スクリプトのメニューに共通のヘッダー(menuScript)を付ける．
                // こうしておくと，menuSettingShortcut_Clickで，スクリプトのメニューが
                // menuScriptの子だと自動で認識される
                string name = "menuScript" + id.Replace('.', '_');
                PaletteToolMenuItem item = new PaletteToolMenuItem(id);
                item.Text = display;
                item.Name = name;
                item.Click += new EventHandler(handleScriptMenuItem_Click);
                menuScript.DropDownItems.Add(item);
                count++;
            }

            // 「スクリプトのリストを更新」を追加
            if (count > 0) {
                menuScript.DropDownItems.Add(new ToolStripSeparator());
            }
            menuScript.DropDownItems.Add(menuScriptUpdate);
            Util.applyToolStripFontRecurse(menuScript, AppManager.editorConfig.getBaseFont());
            applyShortcut();
        }
#endif
        /// <summary>
        /// 指定したノートナンバーが可視状態となるよう、縦スクロールバーを移動させます。
        /// </summary>
        /// <param name="note"></param>
        public void ensureVisibleY(int note)
        {
            Action<int> setVScrollValue = (value) => {
                int draft = Math.Min(Math.Max(value, vScroll.Minimum), vScroll.Maximum);
                vScroll.Value = draft;
            };
            if (note <= 0) {
                setVScrollValue(vScroll.Maximum - vScroll.LargeChange);
                return;
            } else if (note >= 127) {
                vScroll.Value = vScroll.Minimum;
                return;
            }
            int height = pictPianoRoll.getHeight();
            int noteTop = AppManager.noteFromYCoord(0); //画面上端でのノートナンバー
            int noteBottom = AppManager.noteFromYCoord(height); // 画面下端でのノートナンバー

            int maximum = vScroll.Maximum;
            int track_height = (int)(100 * controller.getScaleY());
            // ノートナンバーnoteの現在のy座標がいくらか？
            int note_y = AppManager.yCoordFromNote(note);
            if (note < noteBottom) {
                // ノートナンバーnoteBottomの現在のy座標が新しいnoteのy座標と同一になるよう，startToDrawYを変える
                // startToDrawYを次の値にする必要がある
                int new_start_to_draw_y = controller.getStartToDrawY() + (note_y - height);
                int value = calculateVScrollValueFromStartToDrawY(new_start_to_draw_y);
                setVScrollValue(value);
            } else if (noteTop < note) {
                // ノートナンバーnoteTopの現在のy座標が，ノートナンバーnoteの新しいy座標と同一になるよう，startToDrawYを変える
                int new_start_to_draw_y = controller.getStartToDrawY() + (note_y - 0);
                int value = calculateVScrollValueFromStartToDrawY(new_start_to_draw_y);
                setVScrollValue(value);
            }
        }

        /// <summary>
        /// 指定したゲートタイムがピアノロール上で可視状態となるよう、横スクロールバーを移動させます。
        /// </summary>
        /// <param name="clock"></param>
        public void ensureVisible(int clock)
        {
            // カーソルが画面内にあるかどうか検査
            int clock_left = AppManager.clockFromXCoord(AppManager.keyWidth);
            int clock_right = AppManager.clockFromXCoord(pictPianoRoll.getWidth());
            int uwidth = clock_right - clock_left;
            if (clock < clock_left || clock_right < clock) {
                int cl_new_center = (clock / uwidth) * uwidth + uwidth / 2;
                float f_draft = cl_new_center - (pictPianoRoll.getWidth() / 2 + 34 - 70) * controller.getScaleXInv();
                if (f_draft < 0f) {
                    f_draft = 0;
                }
                int draft = (int)(f_draft);
                if (draft < hScroll.Minimum) {
                    draft = hScroll.Minimum;
                } else if (hScroll.Maximum < draft) {
                    draft = hScroll.Maximum;
                }
                if (hScroll.Value != draft) {
                    AppManager.mDrawStartIndex[AppManager.getSelected() - 1] = 0;
                    hScroll.Value = draft;
                }
            }
        }

        /// <summary>
        /// プレイカーソルが見えるようスクロールする
        /// </summary>
        public void ensureCursorVisible()
        {
            ensureVisible(AppManager.getCurrentClock());
        }

        /// <summary>
        /// 特殊なショートカットキーを処理します。
        /// </summary>
        /// <param name="e"></param>
        /// <param name="onPreviewKeyDown">PreviewKeyDownイベントから送信されてきた場合、true（送る側が設定する）</param>
        public void processSpecialShortcutKey(KeyEventArgs e, bool onPreviewKeyDown)
        {
#if DEBUG
            sout.println("FormMain#processSpecialShortcutKey");
#endif
            // 歌詞入力用のテキストボックスが表示されていたら，何もしない
            if (AppManager.mInputTextBox.Enabled) {
                AppManager.mInputTextBox.Focus();
                return;
            }

            bool flipPlaying = false; // 再生/停止状態の切り替えが要求されたらtrue

            // 最初に、特殊な取り扱いが必要なショートカット、について、
            // 該当するショートカットがあればそいつらを発動する。
            Keys stroke = e.KeyCode | e.Modifiers;

            if (onPreviewKeyDown && e.KeyCode != Keys.None) {
                foreach (SpecialShortcutHolder holder in mSpecialShortcutHolders) {
                    if (stroke == holder.shortcut) {
                        try {
#if DEBUG
                            sout.println("FormMain#processSpecialShortcutKey; perform click: name=" + holder.menu.Name);
#endif
                            holder.menu.PerformClick();
                        } catch (Exception ex) {
                            Logger.write(typeof(FormMain) + ".processSpecialShortcutKey; ex=" + ex + "\n");
                            serr.println("FormMain#processSpecialShortcutKey; ex=" + ex);
                        }
                        if (e.KeyCode == System.Windows.Forms.Keys.Tab) {
                            focusPianoRoll();
                        }
                        return;
                    }
                }
            }

            if (e.Modifiers != Keys.None) {
#if DEBUG
                sout.println("FormMain#processSpecialShortcutKey; bailout with (modifier != VK_UNDEFINED)");
#endif
                return;
            }

            EditMode edit_mode = AppManager.getEditMode();

            if (e.KeyCode == System.Windows.Forms.Keys.Return) {
                // MIDIステップ入力のときの処理
                if (controller.isStepSequencerEnabled()) {
                    if (AppManager.mAddingEvent != null) {
                        fixAddingEvent();
                        AppManager.mAddingEvent = null;
                        refreshScreen(true);
                    }
                }
            } else if (e.KeyCode == System.Windows.Forms.Keys.Space) {
                if (!AppManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier) {
                    flipPlaying = true;
                }
            } else if (e.KeyCode == System.Windows.Forms.Keys.OemPeriod) {
                if (!onPreviewKeyDown) {

                    if (AppManager.isPlaying()) {
                        AppManager.setPlaying(false, this);
                    } else {
                        VsqFileEx vsq = AppManager.getVsqFile();
                        if (!vsq.config.StartMarkerEnabled) {
                            AppManager.setCurrentClock(0);
                        } else {
                            AppManager.setCurrentClock(vsq.config.StartMarker);
                        }
                        refreshScreen();
                    }
                }
            } else if (e.KeyCode == System.Windows.Forms.Keys.Add || e.KeyCode == System.Windows.Forms.Keys.Oemplus || e.KeyCode == System.Windows.Forms.Keys.Right) {
                if (onPreviewKeyDown) {
                    forward();
                }
            } else if (e.KeyCode == System.Windows.Forms.Keys.Subtract || e.KeyCode == System.Windows.Forms.Keys.OemMinus || e.KeyCode == System.Windows.Forms.Keys.Left) {
                if (onPreviewKeyDown) {
                    rewind();
                }
            } else if (e.KeyCode == System.Windows.Forms.Keys.Escape) {
                // ステップ入力中の場合，入力中の音符をクリアする
                VsqEvent item = AppManager.mAddingEvent;
                if (controller.isStepSequencerEnabled() && item != null) {
                    // 入力中だった音符の長さを取得し，
                    int length = item.ID.getLength();
                    AppManager.mAddingEvent = null;
                    int clock = AppManager.getCurrentClock();
                    int clock_draft = clock - length;
                    if (clock_draft < 0) {
                        clock_draft = 0;
                    }
                    // その分だけソングポジションを戻す．
                    AppManager.setCurrentClock(clock_draft);
                    refreshScreen(true);
                }
            } else {
                if (!AppManager.isPlaying()) {
                    // 最初に戻る、の機能を発動
                    Keys[] specialGoToFirst = AppManager.editorConfig.SpecialShortcutGoToFirst;
                    if (specialGoToFirst != null && specialGoToFirst.Length > 0) {
                        Keys shortcut = specialGoToFirst.Aggregate(Keys.None, (seed, key) => seed | key);
                        if (e.KeyCode == shortcut) {
                            AppManager.setCurrentClock(0);
                            ensureCursorVisible();
                            refreshScreen();
                        }
                    }
                }
            }
            if (!onPreviewKeyDown && flipPlaying) {
                if (AppManager.isPlaying()) {
                    double elapsed = PlaySound.getPosition();
                    double threshold = AppManager.mForbidFlipPlayingThresholdSeconds;
                    if (threshold < 0) {
                        threshold = 0.0;
                    }
                    if (elapsed > threshold) {
                        timer.Stop();
                        AppManager.setPlaying(false, this);
                    }
                } else {
                    AppManager.setPlaying(true, this);
                }
            }
            if (e.KeyCode == System.Windows.Forms.Keys.Tab) {
                focusPianoRoll();
            }
        }

        public void updateScrollRangeHorizontal()
        {
            // コンポーネントの高さが0の場合，スクロールの設定が出来ないので．
            int pwidth = pictPianoRoll.getWidth();
            int hwidth = hScroll.Width;
            if (pwidth <= 0 || hwidth <= 0) {
                return;
            }

            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq == null) return;
            int l = vsq.TotalClocks;
            float scalex = controller.getScaleX();
            int key_width = AppManager.keyWidth;
            int pict_piano_roll_width = pwidth - key_width;
            int large_change = (int)(pict_piano_roll_width / scalex);
            int maximum = (int)(l + large_change);

            int thumb_width = System.Windows.Forms.SystemInformation.HorizontalScrollBarThumbWidth;
            int box_width = (int)(large_change / (float)maximum * (hwidth - 2 * thumb_width));
            if (box_width < AppManager.editorConfig.MinimumScrollHandleWidth) {
                box_width = AppManager.editorConfig.MinimumScrollHandleWidth;
                if (hwidth - 2 * thumb_width > box_width) {
                    maximum = l * (hwidth - 2 * thumb_width) / (hwidth - 2 * thumb_width - box_width);
                    large_change = l * box_width / (hwidth - 2 * thumb_width - box_width);
                }
            }

            if (large_change <= 0) large_change = 1;
            if (maximum <= 0) maximum = 1;
            hScroll.LargeChange = large_change;
            hScroll.Maximum = maximum;

            int old_value = hScroll.Value;
            if (old_value > maximum - large_change) {
                hScroll.Value = maximum - large_change;
            }
        }

        public void updateScrollRangeVertical()
        {
            // コンポーネントの高さが0の場合，スクロールの設定が出来ないので．
            int pheight = pictPianoRoll.getHeight();
            int vheight = vScroll.Height;
            if (pheight <= 0 || vheight <= 0) {
                return;
            }

            float scaley = controller.getScaleY();

            int maximum = (int)(128 * (int)(100 * scaley) / scaley);
            int large_change = (int)(pheight / scaley);

            int thumb_height = System.Windows.Forms.SystemInformation.VerticalScrollBarThumbHeight;
            int box_height = (int)(large_change / (float)maximum * (vheight - 2 * thumb_height));
            if (box_height < AppManager.editorConfig.MinimumScrollHandleWidth) {
                box_height = AppManager.editorConfig.MinimumScrollHandleWidth;
                maximum = (int)(((128.0 * (int)(100 * scaley) - pheight) / scaley) * (vheight - 2 * thumb_height) / (vheight - 2 * thumb_height - box_height));
                large_change = (int)(((128.0 * (int)(100 * scaley) - pheight) / scaley) * box_height / (vheight - 2 * thumb_height - box_height));
            }

            if (large_change <= 0) large_change = 1;
            if (maximum <= 0) maximum = 1;
            vScroll.LargeChange = large_change;
            vScroll.Maximum = maximum;
            vScroll.SmallChange = 100;

            int new_value = maximum - large_change;
            if (new_value < vScroll.Minimum) {
                new_value = vScroll.Minimum;
            }
            if (vScroll.Value > new_value) {
                vScroll.Value = new_value;
            }
        }

        /// <summary>
        /// コントロールトラックの表示・非表示状態を更新します
        /// </summary>
        public void flipControlCurveVisible(bool visible)
        {
            trackSelector.setCurveVisible(visible);
            if (visible) {
                splitContainer1.setSplitterFixed(false);
                splitContainer1.setDividerSize(_SPL_SPLITTER_WIDTH);
                splitContainer1.setDividerLocation(splitContainer1.getHeight() - AppManager.mLastTrackSelectorHeight - splitContainer1.getDividerSize());
                splitContainer1.setPanel2MinSize(trackSelector.getPreferredMinSize());
            } else {
                AppManager.mLastTrackSelectorHeight = splitContainer1.getHeight() - splitContainer1.getDividerLocation() - splitContainer1.getDividerSize();
                splitContainer1.setSplitterFixed(true);
                splitContainer1.setDividerSize(0);
                int panel2height = TrackSelector.OFFSET_TRACK_TAB * 2;
                splitContainer1.setDividerLocation(splitContainer1.getHeight() - panel2height - splitContainer1.getDividerSize());
                splitContainer1.setPanel2MinSize(panel2height);
            }
            refreshScreen();
        }

        /// <summary>
        /// ミキサーダイアログの表示・非表示状態を更新します
        /// </summary>
        /// <param name="visible">表示状態にする場合true，そうでなければfalse</param>
        public void flipMixerDialogVisible(bool visible)
        {
            AppManager.mMixerWindow.Visible = visible;
            AppManager.editorConfig.MixerVisible = visible;
            if (visible != menuVisualMixer.Checked) {
                menuVisualMixer.Checked = visible;
            }
        }

        /// <summary>
        /// アイコンパレットの表示・非表示状態を更新します
        /// </summary>
        public void flipIconPaletteVisible(bool visible)
        {
            AppManager.iconPalette.Visible = visible;
            AppManager.editorConfig.IconPaletteVisible = visible;
            if (visible != menuVisualIconPalette.Checked) {
                menuVisualIconPalette.Checked = visible;
            }
        }

        /// <summary>
        /// メニューのショートカットキーを、AppManager.EditorConfig.ShorcutKeysの内容に応じて変更します
        /// </summary>
        public void applyShortcut()
        {
            mSpecialShortcutHolders.Clear();

            SortedDictionary<string, Keys[]> dict = AppManager.editorConfig.getShortcutKeysDictionary(this.getDefaultShortcutKeys());
            #region menuStripMain
            ByRef<Object> parent = new ByRef<Object>(null);
            foreach (var key in dict.Keys) {
                if (key == "menuEditCopy" || key == "menuEditCut" || key == "menuEditPaste" || key == "SpecialShortcutGoToFirst") {
                    continue;
                }
                Object menu = searchMenuItemFromName(key, parent);
                if (menu != null) {
                    string menu_name = "";
                    if (menu is ToolStripMenuItem) {
                        menu_name = ((ToolStripMenuItem)menu).Name;
                    } else {
                        continue;
                    }
                    applyMenuItemShortcut(dict, menu, menu_name);
                }
            }
            if (dict.ContainsKey("menuEditCopy")) {
                applyMenuItemShortcut(dict, menuHiddenCopy, "menuEditCopy");
            }
            if (dict.ContainsKey("menuEditCut")) {
                applyMenuItemShortcut(dict, menuHiddenCut, "menuEditCut");
            }
            if (dict.ContainsKey("menuEditCopy")) {
                applyMenuItemShortcut(dict, menuHiddenPaste, "menuEditPaste");
            }
            #endregion

            List<ValuePair<string, ToolStripMenuItem[]>> work = new List<ValuePair<string, ToolStripMenuItem[]>>();
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuEditUndo", new ToolStripMenuItem[] { cMenuPianoUndo, cMenuTrackSelectorUndo }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuEditRedo", new ToolStripMenuItem[] { cMenuPianoRedo, cMenuTrackSelectorRedo }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuEditCut", new ToolStripMenuItem[] { cMenuPianoCut, cMenuTrackSelectorCut, menuEditCut }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuEditCopy", new ToolStripMenuItem[] { cMenuPianoCopy, cMenuTrackSelectorCopy, menuEditCopy }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuEditPaste", new ToolStripMenuItem[] { cMenuPianoPaste, cMenuTrackSelectorPaste, menuEditPaste }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuEditSelectAll", new ToolStripMenuItem[] { cMenuPianoSelectAll, cMenuTrackSelectorSelectAll }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuEditSelectAllEvents", new ToolStripMenuItem[] { cMenuPianoSelectAllEvents }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuEditDelete", new ToolStripMenuItem[] { menuEditDelete }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuVisualGridline", new ToolStripMenuItem[] { cMenuPianoGrid }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuJobLyric", new ToolStripMenuItem[] { cMenuPianoImportLyric }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuLyricExpressionProperty", new ToolStripMenuItem[] { cMenuPianoExpressionProperty }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuLyricVibratoProperty", new ToolStripMenuItem[] { cMenuPianoVibratoProperty }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuTrackOn", new ToolStripMenuItem[] { cMenuTrackTabTrackOn }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuTrackAdd", new ToolStripMenuItem[] { cMenuTrackTabAdd }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuTrackCopy", new ToolStripMenuItem[] { cMenuTrackTabCopy }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuTrackDelete", new ToolStripMenuItem[] { cMenuTrackTabDelete }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuTrackRenderCurrent", new ToolStripMenuItem[] { cMenuTrackTabRenderCurrent }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuTrackRenderAll", new ToolStripMenuItem[] { cMenuTrackTabRenderAll }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuTrackOverlay", new ToolStripMenuItem[] { cMenuTrackTabOverlay }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuTrackRendererVOCALOID1", new ToolStripMenuItem[] { cMenuTrackTabRendererVOCALOID1 }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuTrackRendererVOCALOID2", new ToolStripMenuItem[] { cMenuTrackTabRendererVOCALOID2 }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuTrackRendererAquesTone", new ToolStripMenuItem[] { menuTrackRendererAquesTone }));
            work.Add(new ValuePair<string, ToolStripMenuItem[]>("menuTrackRendererVCNT", new ToolStripMenuItem[] { menuTrackRendererVCNT }));
            int c = work.Count;
            for (int j = 0; j < c; j++) {
                ValuePair<string, ToolStripMenuItem[]> item = work[j];
                if (dict.ContainsKey(item.getKey())) {
                    Keys[] k = dict[item.getKey()];
                    string s = Utility.getShortcutDisplayString(k);
                    if (s != "") {
                        for (int i = 0; i < item.getValue().Length; i++) {
                            item.getValue()[i].ShortcutKeyDisplayString = s;
                        }
                    }
                }
            }

            // ミキサーウィンドウ
            if (AppManager.mMixerWindow != null) {
                if (dict.ContainsKey("menuVisualMixer")) {
                    Keys shortcut = dict["menuVisualMixer"].Aggregate(Keys.None, (seed, key) => seed | key);
                    AppManager.mMixerWindow.applyShortcut(shortcut);
                }
            }

            // アイコンパレット
            if (AppManager.iconPalette != null) {
                if (dict.ContainsKey("menuVisualIconPalette")) {
                    Keys shortcut = dict["menuVisualIconPalette"].Aggregate(Keys.None, (seed, key) => seed | key);
                    AppManager.iconPalette.applyShortcut(shortcut);
                }
            }

#if ENABLE_PROPERTY
            // プロパティ
            if (AppManager.propertyWindow != null) {
                if (dict.ContainsKey(menuVisualProperty.Name)) {
                    Keys shortcut = dict[menuVisualProperty.Name].Aggregate(Keys.None, (seed, key) => seed | key);
                    AppManager.propertyWindow.applyShortcut(shortcut);
                }
            }
#endif

            // スクリプトにショートカットを適用
            int count = menuScript.DropDownItems.Count;
            for (int i = 0; i < count; i++) {
                System.Windows.Forms.ToolStripItem tsi = menuScript.DropDownItems[i];
                if (tsi is System.Windows.Forms.ToolStripMenuItem) {
                    System.Windows.Forms.ToolStripMenuItem tsmi = (System.Windows.Forms.ToolStripMenuItem)tsi;
                    if (tsmi.DropDownItems.Count == 1) {
                        System.Windows.Forms.ToolStripItem subtsi_tsmi = tsmi.DropDownItems[0];
                        if (subtsi_tsmi is System.Windows.Forms.ToolStripMenuItem) {
                            System.Windows.Forms.ToolStripMenuItem dd_run = (System.Windows.Forms.ToolStripMenuItem)subtsi_tsmi;
                            if (dict.ContainsKey(PortUtil.getComponentName(dd_run))) {
                                applyMenuItemShortcut(dict, tsmi, PortUtil.getComponentName(tsi));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// dictの中から
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="item"></param>
        /// <param name="item_name"></param>
        /// <param name="default_shortcut"></param>
        public void applyMenuItemShortcut(SortedDictionary<string, Keys[]> dict, Object item, string item_name)
        {
            try {
                if (dict.ContainsKey(item_name)) {
#if DEBUG
                    if (!(item is ToolStripMenuItem)) {
                        throw new Exception("FormMain#applyMenuItemShortcut; item is NOT BMenuItem");
                    }
#endif // DEBUG
                    if (item is ToolStripMenuItem) {
                        ToolStripMenuItem menu = (ToolStripMenuItem)item;
                        Keys[] keys = dict[item_name];
                        Keys shortcut = keys.Aggregate(Keys.None, (seed, key) => seed | key);

                        if (shortcut == System.Windows.Forms.Keys.Delete) {
                            menu.ShortcutKeyDisplayString = "Delete";
                            menu.ShortcutKeys = Keys.None;
                            mSpecialShortcutHolders.Add(new SpecialShortcutHolder(shortcut, menu));
                        } else {
                            try {
                                menu.ShortcutKeyDisplayString = "";
                                menu.ShortcutKeys = shortcut;
                            } catch (Exception ex) {
                                // ショートカットの適用に失敗する→特殊な取り扱いが必要
                                menu.ShortcutKeyDisplayString = Utility.getShortcutDisplayString(keys);
                                menu.ShortcutKeys = Keys.None;
                                mSpecialShortcutHolders.Add(new SpecialShortcutHolder(shortcut, menu));
                            }
                        }
                    }
                } else {
                    if (item is System.Windows.Forms.ToolStripMenuItem) {
                        ((System.Windows.Forms.ToolStripMenuItem)item).ShortcutKeys = Keys.None;
                    }
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".applyMenuItemShortcut; ex=" + ex + "\n");
            }
        }

        /// <summary>
        /// ソングポジションを1小節進めます
        /// </summary>
        public void forward()
        {
            bool playing = AppManager.isPlaying();
            if (playing) {
                return;
            }
            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq == null) {
                return;
            }
            int cl_clock = AppManager.getCurrentClock();
            int unit = QuantizeModeUtil.getQuantizeClock(
                AppManager.editorConfig.getPositionQuantize(),
                AppManager.editorConfig.isPositionQuantizeTriplet());
            int cl_new = doQuantize(cl_clock + unit, unit);

            if (cl_new <= hScroll.Maximum + (pictPianoRoll.getWidth() - AppManager.keyWidth) * controller.getScaleXInv()) {
                // 表示の更新など
                AppManager.setCurrentClock(cl_new);

                // ステップ入力時の処理
                updateNoteLengthStepSequencer();

                ensureCursorVisible();
                AppManager.setPlaying(playing, this);
                refreshScreen();
            }
        }

        /// <summary>
        /// ソングポジションを1小節戻します
        /// </summary>
        public void rewind()
        {
            bool playing = AppManager.isPlaying();
            if (playing) {
                return;
            }
            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq == null) {
                return;
            }
            int cl_clock = AppManager.getCurrentClock();
            int unit = QuantizeModeUtil.getQuantizeClock(
                AppManager.editorConfig.getPositionQuantize(),
                AppManager.editorConfig.isPositionQuantizeTriplet());
            int cl_new = doQuantize(cl_clock - unit, unit);
            if (cl_new < 0) {
                cl_new = 0;
            }

            AppManager.setCurrentClock(cl_new);

            // ステップ入力時の処理
            updateNoteLengthStepSequencer();

            ensureCursorVisible();
            AppManager.setPlaying(playing, this);
            refreshScreen();
        }

        /// <summary>
        /// cMenuPianoの固定長音符入力の各メニューのチェック状態をm_pencil_modeを元に更新します
        /// </summary>
        public void updateCMenuPianoFixed()
        {
            cMenuPianoFixed01.Checked = false;
            cMenuPianoFixed02.Checked = false;
            cMenuPianoFixed04.Checked = false;
            cMenuPianoFixed08.Checked = false;
            cMenuPianoFixed16.Checked = false;
            cMenuPianoFixed32.Checked = false;
            cMenuPianoFixed64.Checked = false;
            cMenuPianoFixed128.Checked = false;
            cMenuPianoFixedOff.Checked = false;
            cMenuPianoFixedTriplet.Checked = false;
            cMenuPianoFixedDotted.Checked = false;
            PencilModeEnum mode = mPencilMode.getMode();
            if (mode == PencilModeEnum.L1) {
                cMenuPianoFixed01.Checked = true;
            } else if (mode == PencilModeEnum.L2) {
                cMenuPianoFixed02.Checked = true;
            } else if (mode == PencilModeEnum.L4) {
                cMenuPianoFixed04.Checked = true;
            } else if (mode == PencilModeEnum.L8) {
                cMenuPianoFixed08.Checked = true;
            } else if (mode == PencilModeEnum.L16) {
                cMenuPianoFixed16.Checked = true;
            } else if (mode == PencilModeEnum.L32) {
                cMenuPianoFixed32.Checked = true;
            } else if (mode == PencilModeEnum.L64) {
                cMenuPianoFixed64.Checked = true;
            } else if (mode == PencilModeEnum.L128) {
                cMenuPianoFixed128.Checked = true;
            } else if (mode == PencilModeEnum.Off) {
                cMenuPianoFixedOff.Checked = true;
            }
            cMenuPianoFixedTriplet.Checked = mPencilMode.isTriplet();
            cMenuPianoFixedDotted.Checked = mPencilMode.isDot();
        }

        public void clearTempWave()
        {
            string tmppath = Path.Combine(AppManager.getCadenciiTempDir(), AppManager.getID());
            if (!Directory.Exists(tmppath)) {
                return;
            }

            // 今回このPCが起動されるよりも以前に，Cadenciiが残したデータを削除する
            //TODO: システムカウンタは約49日でリセットされてしまい，厳密には実装できないようなので，保留．

            // このFormMainのインスタンスが使用したデータを消去する
            for (int i = 1; i <= AppManager.MAX_NUM_TRACK; i++) {
                string file = Path.Combine(tmppath, i + ".wav");
                if (System.IO.File.Exists(file)) {
                    for (int error = 0; error < 100; error++) {
                        try {
                            PortUtil.deleteFile(file);
                            break;
                        } catch (Exception ex) {
                            Logger.write(typeof(FormMain) + ".clearTempWave; ex=" + ex + "\n");
#if DEBUG
                            cadencii.debug.push_log("FormMain+ClearTempWave()");
                            cadencii.debug.push_log("    ex=" + ex.ToString());
                            cadencii.debug.push_log("    error_count=" + error);
#endif

                            Thread.Sleep(100);
                        }
                    }
                }
            }
            string whd = Path.Combine(tmppath, UtauWaveGenerator.FILEBASE + ".whd");
            if (System.IO.File.Exists(whd)) {
                try {
                    PortUtil.deleteFile(whd);
                } catch (Exception ex) {
                    Logger.write(typeof(FormMain) + ".clearTempWave; ex=" + ex + "\n");
                }
            }
            string dat = Path.Combine(tmppath, UtauWaveGenerator.FILEBASE + ".dat");
            if (System.IO.File.Exists(dat)) {
                try {
                    PortUtil.deleteFile(dat);
                } catch (Exception ex) {
                    Logger.write(typeof(FormMain) + ".clearTempWave; ex=" + ex + "\n");
                }
            }
        }

        /// <summary>
        /// 鍵盤音キャッシュの中から指定したノートナンバーの音源を捜し、再生します。
        /// </summary>
        /// <param name="note">再生する音の高さを指定するノートナンバー</param>
        public void playPreviewSound(int note)
        {
            KeySoundPlayer.play(note);
        }

#if ENABLE_MOUSEHOVER
        public void MouseHoverEventGenerator( Object arg ) {
            int note = (int)arg;
            if ( AppManager.editorConfig.MouseHoverTime > 0 ) {
                Thread.Sleep( AppManager.editorConfig.MouseHoverTime );
            }
            KeySoundPlayer.play( note );
        }
#endif

        /// <summary>
        /// このコンポーネントの表示言語を、現在の言語設定に従って更新します。
        /// </summary>
        public void applyLanguage()
        {
            openXmlVsqDialog.Filter = string.Empty;
            try {
                openXmlVsqDialog.Filter = string.Join("|", new[] { _("XML-VSQ Format(*.xvsq)|*.xvsq"), _("All Files(*.*)|*.*") });
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".applyLanguage; ex=" + ex + "\n");
                openXmlVsqDialog.Filter = string.Join("|", new[] { "XML-VSQ Format(*.xvsq)|*.xvsq", "All Files(*.*)|*.*" });
            }

            saveXmlVsqDialog.Filter = string.Empty;
            try {
                saveXmlVsqDialog.Filter = string.Join("|", new[] { _("XML-VSQ Format(*.xvsq)|*.xvsq"), _("All Files(*.*)|*.*") });
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".applyLanguage; ex=" + ex + "\n");
                saveXmlVsqDialog.Filter = string.Join("|", new[] { "XML-VSQ Format(*.xvsq)|*.xvsq", "All Files(*.*)|*.*" });
            }

            openUstDialog.Filter = string.Empty;
            try {
                openUstDialog.Filter = string.Join("|", new[] { _("UTAU Script Format(*.ust)|*.ust"), _("All Files(*.*)|*.*") });
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".applyLanguage; ex=" + ex + "\n");
                openUstDialog.Filter = string.Join("|", new[] { "UTAU Script Format(*.ust)|*.ust", "All Files(*.*)|*.*" });
            }

            openMidiDialog.Filter = string.Empty;
            try {
                openMidiDialog.Filter = string.Join("|", new[] {
                    _( "MIDI Format(*.mid)|*.mid" ),
                    _( "VSQ Format(*.vsq)|*.vsq" ),
                    _( "VSQX Format(*.vsqx)|*.vsqx" ),
                    _( "All Files(*.*)|*.*" ) });
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".applyLanguage; ex=" + ex + "\n");
                openMidiDialog.Filter = string.Join("|", new[] {
                    "MIDI Format(*.mid)|*.mid",
                    "VSQ Format(*.vsq)|*.vsq",
                    "VSQX Format(*.vsqx)|*.vsqx",
                    "All Files(*.*)|*.*" });
            }

            saveMidiDialog.Filter = string.Empty;
            try {
                saveMidiDialog.Filter = string.Join("|", new[] {
                    _( "MIDI Format(*.mid)|*.mid" ),
                    _( "VSQ Format(*.vsq)|*.vsq" ),
                    _( "All Files(*.*)|*.*" ) });
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".applyLanguage; ex=" + ex + "\n");
                saveMidiDialog.Filter = string.Join("|", new[] {
                    "MIDI Format(*.mid)|*.mid",
                    "VSQ Format(*.vsq)|*.vsq",
                    "All Files(*.*)|*.*" });
            }

            openWaveDialog.Filter = string.Empty;
            try {
                openWaveDialog.Filter = string.Join("|", new[] {
                    _( "Wave File(*.wav)|*.wav" ),
                    _( "All Files(*.*)|*.*" ) });
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".applyLanguage; ex=" + ex + "\n");
                openWaveDialog.Filter = string.Join("|", new[] {
                    "Wave File(*.wav)|*.wav",
                    "All Files(*.*)|*.*" });
            }

            stripLblGameCtrlMode.ToolTipText = _("Game controler");

            this.Invoke(new EventHandler(updateGameControlerStatus));

            stripBtnPointer.Text = _("Pointer");
            stripBtnPointer.ToolTipText = _("Pointer");
            stripBtnPencil.Text = _("Pencil");
            stripBtnPencil.ToolTipText = _("Pencil");
            stripBtnLine.Text = _("Line");
            stripBtnLine.ToolTipText = _("Line");
            stripBtnEraser.Text = _("Eraser");
            stripBtnEraser.ToolTipText = _("Eraser");
            stripBtnCurve.Text = _("Curve");
            stripBtnCurve.ToolTipText = _("Curve");
            stripBtnGrid.Text = _("Grid");
            stripBtnGrid.ToolTipText = _("Grid");
            if (AppManager.isPlaying()) {
                stripBtnPlay.Text = _("Stop");
            } else {
                stripBtnPlay.Text = _("Play");
            }

            stripBtnMoveTop.ToolTipText = _("Move to beginning measure");
            stripBtnMoveEnd.ToolTipText = _("Move to end measure");
            stripBtnForward.ToolTipText = _("Move forward");
            stripBtnRewind.ToolTipText = _("Move backwared");
            stripBtnLoop.ToolTipText = _("Repeat");
            stripBtnScroll.ToolTipText = _("Auto scroll");

            #region main menu
            menuFile.Text = _("File");
            menuFile.Mnemonic(Keys.F);
            menuFileNew.Text = _("New");
            menuFileNew.Mnemonic(Keys.N);
            menuFileOpen.Text = _("Open");
            menuFileOpen.Mnemonic(Keys.O);
            menuFileOpenVsq.Text = _("Open VSQX/VSQ/Vocaloid MIDI");
            menuFileOpenVsq.Mnemonic(Keys.V);
            menuFileOpenUst.Text = _("Open UTAU project file");
            menuFileOpenUst.Mnemonic(Keys.U);
            menuFileSave.Text = _("Save");
            menuFileSave.Mnemonic(Keys.S);
            menuFileSaveNamed.Text = _("Save as");
            menuFileSaveNamed.Mnemonic(Keys.A);
            menuFileImport.Text = _("Import");
            menuFileImport.Mnemonic(Keys.I);
            menuFileImportVsq.Text = _("VSQ / Vocaloid Midi");
            menuFileExport.Text = _("Export");
            menuFileExport.Mnemonic(Keys.E);
            menuFileExportWave.Text = _("WAVE");
            menuFileExportParaWave.Text = _("Serial numbered WAVEs");
            menuFileExportUst.Text = _("UTAU project file");
            menuFileExportVxt.Text = _("Metatext for vConnect");
            menuFileExportVsq.Text = _("VSQ File");
            menuFileExportVsqx.Text = _("VSQX File");
            menuFileRecent.Text = _("Open Recent");
            menuFileRecent.Mnemonic(Keys.R);
            menuFileRecentClear.Text = _("Clear Menu");
            menuFileQuit.Text = _("Quit");
            menuFileQuit.Mnemonic(Keys.Q);

            menuEdit.Text = _("Edit");
            menuEdit.Mnemonic(Keys.E);
            menuEditUndo.Text = _("Undo");
            menuEditUndo.Mnemonic(Keys.U);
            menuEditRedo.Text = _("Redo");
            menuEditRedo.Mnemonic(Keys.R);
            menuEditCut.Text = _("Cut");
            menuEditCut.Mnemonic(Keys.T);
            menuEditCopy.Text = _("Copy");
            menuEditCopy.Mnemonic(Keys.C);
            menuEditPaste.Text = _("Paste");
            menuEditPaste.Mnemonic(Keys.P);
            menuEditDelete.Text = _("Delete");
            menuEditDelete.Mnemonic(Keys.D);
            menuEditAutoNormalizeMode.Text = _("Auto normalize mode");
            menuEditAutoNormalizeMode.Mnemonic(Keys.N);
            menuEditSelectAll.Text = _("Select All");
            menuEditSelectAll.Mnemonic(Keys.A);
            menuEditSelectAllEvents.Text = _("Select all events");
            menuEditSelectAllEvents.Mnemonic(Keys.E);

            menuVisual.Text = _("View");
            menuVisual.Mnemonic(Keys.V);
            menuVisualControlTrack.Text = _("Control track");
            menuVisualControlTrack.Mnemonic(Keys.C);
            menuVisualMixer.Text = _("Mixer");
            menuVisualMixer.Mnemonic(Keys.X);
            menuVisualWaveform.Text = _("Waveform");
            menuVisualWaveform.Mnemonic(Keys.W);
            menuVisualProperty.Text = _("Property window");
            menuVisualOverview.Text = _("Navigation");
            menuVisualOverview.Mnemonic(Keys.V);
            menuVisualGridline.Text = _("Grid line");
            menuVisualGridline.Mnemonic(Keys.G);
            menuVisualStartMarker.Text = _("Start marker");
            menuVisualStartMarker.Mnemonic(Keys.S);
            menuVisualEndMarker.Text = _("End marker");
            menuVisualEndMarker.Mnemonic(Keys.E);
            menuVisualLyrics.Text = _("Lyrics/Phoneme");
            menuVisualLyrics.Mnemonic(Keys.L);
            menuVisualNoteProperty.Text = _("Note expression/vibrato");
            menuVisualNoteProperty.Mnemonic(Keys.N);
            menuVisualPitchLine.Text = _("Pitch line");
            menuVisualPitchLine.Mnemonic(Keys.P);
            menuVisualPluginUi.Text = _("VSTi plugin UI");
            menuVisualPluginUi.Mnemonic(Keys.U);
            menuVisualIconPalette.Text = _("Icon palette");
            menuVisualIconPalette.Mnemonic(Keys.I);

            menuJob.Text = _("Job");
            menuJob.Mnemonic(Keys.J);
            menuJobNormalize.Text = _("Normalize notes");
            menuJobNormalize.Mnemonic(Keys.N);
            menuJobInsertBar.Text = _("Insert bars");
            menuJobInsertBar.Mnemonic(Keys.I);
            menuJobDeleteBar.Text = _("Delete bars");
            menuJobDeleteBar.Mnemonic(Keys.D);
            menuJobRandomize.Text = _("Randomize");
            menuJobRandomize.Mnemonic(Keys.R);
            menuJobConnect.Text = _("Connect notes");
            menuJobConnect.Mnemonic(Keys.C);
            menuJobLyric.Text = _("Insert lyrics");
            menuJobLyric.Mnemonic(Keys.L);

            menuTrack.Text = _("Track");
            menuTrack.Mnemonic(Keys.T);
            menuTrackOn.Text = _("Track on");
            menuTrackOn.Mnemonic(Keys.K);
            menuTrackAdd.Text = _("Add track");
            menuTrackAdd.Mnemonic(Keys.A);
            menuTrackCopy.Text = _("Copy track");
            menuTrackCopy.Mnemonic(Keys.C);
            menuTrackChangeName.Text = _("Rename track");
            menuTrackDelete.Text = _("Delete track");
            menuTrackDelete.Mnemonic(Keys.D);
            menuTrackRenderCurrent.Text = _("Render current track");
            menuTrackRenderCurrent.Mnemonic(Keys.T);
            menuTrackRenderAll.Text = _("Render all tracks");
            menuTrackRenderAll.Mnemonic(Keys.S);
            menuTrackOverlay.Text = _("Overlay");
            menuTrackOverlay.Mnemonic(Keys.O);
            menuTrackRenderer.Text = _("Renderer");
            menuTrackRenderer.Mnemonic(Keys.R);
            menuTrackRendererVOCALOID1.Mnemonic(Keys.D1);
            menuTrackRendererVOCALOID2.Mnemonic(Keys.D3);
            menuTrackRendererUtau.Mnemonic(Keys.D4);
            menuTrackRendererVCNT.Mnemonic(Keys.D5);
            menuTrackRendererAquesTone.Mnemonic(Keys.D6);

            menuLyric.Text = _("Lyrics");
            menuLyric.Mnemonic(Keys.L);
            menuLyricExpressionProperty.Text = _("Note expression property");
            menuLyricExpressionProperty.Mnemonic(Keys.E);
            menuLyricVibratoProperty.Text = _("Note vibrato property");
            menuLyricVibratoProperty.Mnemonic(Keys.V);
            menuLyricApplyUtauParameters.Text = _("Apply UTAU Parameters");
            menuLyricApplyUtauParameters.Mnemonic(Keys.A);
            menuLyricPhonemeTransformation.Text = _("Phoneme transformation");
            menuLyricPhonemeTransformation.Mnemonic(Keys.T);
            menuLyricDictionary.Text = _("User word dictionary");
            menuLyricDictionary.Mnemonic(Keys.C);
            menuLyricCopyVibratoToPreset.Text = _("Copy vibrato config to preset");
            menuLyricCopyVibratoToPreset.Mnemonic(Keys.P);

            menuScript.Text = _("Script");
            menuScript.Mnemonic(Keys.C);
            menuScriptUpdate.Text = _("Update script list");
            menuScriptUpdate.Mnemonic(Keys.U);

            menuSetting.Text = _("Setting");
            menuSetting.Mnemonic(Keys.S);
            menuSettingPreference.Text = _("Preference");
            menuSettingPreference.Mnemonic(Keys.P);
            menuSettingGameControler.Text = _("Game controler");
            menuSettingGameControler.Mnemonic(Keys.G);
            menuSettingGameControlerLoad.Text = _("Load");
            menuSettingGameControlerLoad.Mnemonic(Keys.L);
            menuSettingGameControlerRemove.Text = _("Remove");
            menuSettingGameControlerRemove.Mnemonic(Keys.R);
            menuSettingGameControlerSetting.Text = _("Setting");
            menuSettingGameControlerSetting.Mnemonic(Keys.S);
            menuSettingSequence.Text = _("Sequence config");
            menuSettingSequence.Mnemonic(Keys.S);
            menuSettingShortcut.Text = _("Shortcut key");
            menuSettingShortcut.Mnemonic(Keys.K);
            menuSettingDefaultSingerStyle.Text = _("Singing style defaults");
            menuSettingDefaultSingerStyle.Mnemonic(Keys.D);
            menuSettingPositionQuantize.Text = _("Quantize");
            menuSettingPositionQuantize.Mnemonic(Keys.Q);
            menuSettingPositionQuantizeOff.Text = _("Off");
            menuSettingPositionQuantizeTriplet.Text = _("Triplet");
            //menuSettingSingerProperty.setText( _( "Singer Properties" ) );
            //menuSettingSingerProperty.setMnemonic( Keys.S );
            menuSettingPaletteTool.Text = _("Palette Tool");
            menuSettingPaletteTool.Mnemonic(Keys.T);
            menuSettingVibratoPreset.Text = _("Vibrato preset");
            menuSettingVibratoPreset.Mnemonic(Keys.V);

            menuTools.Text = _("Tools");
            menuTools.Mnemonic(Keys.O);
            menuToolsCreateVConnectSTANDDb.Text = _("Create vConnect-STAND DB");

            menuHelp.Text = _("Help");
            menuHelp.Mnemonic(Keys.H);
            menuHelpCheckForUpdates.Text = _("Check For Updates");
            menuHelpLog.Text = _("Log");
            menuHelpLog.Mnemonic(Keys.L);
            menuHelpLogSwitch.Text = Logger.isEnabled() ? _("Disable") : _("Enable");
            menuHelpLogSwitch.Mnemonic(Keys.L);
            menuHelpLogOpen.Text = _("Open");
            menuHelpLogOpen.Mnemonic(Keys.O);
            menuHelpAbout.Text = _("About Cadencii");
            menuHelpAbout.Mnemonic(Keys.A);
            menuHelpManual.Text = _("Manual") + " (PDF)";

            menuHiddenCopy.Text = _("Copy");
            menuHiddenCut.Text = _("Cut");
            menuHiddenEditFlipToolPointerEraser.Text = _("Chagne tool pointer / eraser");
            menuHiddenEditFlipToolPointerPencil.Text = _("Change tool pointer / pencil");
            menuHiddenEditLyric.Text = _("Start lyric input");
            menuHiddenGoToEndMarker.Text = _("GoTo end marker");
            menuHiddenGoToStartMarker.Text = _("Goto start marker");
            menuHiddenLengthen.Text = _("Lengthen");
            menuHiddenMoveDown.Text = _("Move down");
            menuHiddenMoveLeft.Text = _("Move left");
            menuHiddenMoveRight.Text = _("Move right");
            menuHiddenMoveUp.Text = _("Move up");
            menuHiddenPaste.Text = _("Paste");
            menuHiddenPlayFromStartMarker.Text = _("Play from start marker");
            menuHiddenSelectBackward.Text = _("Select backward");
            menuHiddenSelectForward.Text = _("Select forward");
            menuHiddenShorten.Text = _("Shorten");
            menuHiddenTrackBack.Text = _("Previous track");
            menuHiddenTrackNext.Text = _("Next track");
            menuHiddenVisualBackwardParameter.Text = _("Previous control curve");
            menuHiddenVisualForwardParameter.Text = _("Next control curve");
            menuHiddenFlipCurveOnPianorollMode.Text = _("Change pitch drawing mode");
            #endregion

            #region cMenuPiano
            cMenuPianoPointer.Text = _("Arrow");
            cMenuPianoPointer.Mnemonic(Keys.A);
            cMenuPianoPencil.Text = _("Pencil");
            cMenuPianoPencil.Mnemonic(Keys.W);
            cMenuPianoEraser.Text = _("Eraser");
            cMenuPianoEraser.Mnemonic(Keys.E);
            cMenuPianoPaletteTool.Text = _("Palette Tool");

            cMenuPianoCurve.Text = _("Curve");
            cMenuPianoCurve.Mnemonic(Keys.V);

            cMenuPianoFixed.Text = _("Note Fixed Length");
            cMenuPianoFixed.Mnemonic(Keys.N);
            cMenuPianoFixedTriplet.Text = _("Triplet");
            cMenuPianoFixedOff.Text = _("Off");
            cMenuPianoFixedDotted.Text = _("Dot");
            cMenuPianoQuantize.Text = _("Quantize");
            cMenuPianoQuantize.Mnemonic(Keys.Q);
            cMenuPianoQuantizeTriplet.Text = _("Triplet");
            cMenuPianoQuantizeOff.Text = _("Off");
            cMenuPianoGrid.Text = _("Show/Hide Grid Line");
            cMenuPianoGrid.Mnemonic(Keys.S);

            cMenuPianoUndo.Text = _("Undo");
            cMenuPianoUndo.Mnemonic(Keys.U);
            cMenuPianoRedo.Text = _("Redo");
            cMenuPianoRedo.Mnemonic(Keys.R);

            cMenuPianoCut.Text = _("Cut");
            cMenuPianoCut.Mnemonic(Keys.T);
            cMenuPianoPaste.Text = _("Paste");
            cMenuPianoPaste.Mnemonic(Keys.P);
            cMenuPianoCopy.Text = _("Copy");
            cMenuPianoCopy.Mnemonic(Keys.C);
            cMenuPianoDelete.Text = _("Delete");
            cMenuPianoDelete.Mnemonic(Keys.D);

            cMenuPianoSelectAll.Text = _("Select All");
            cMenuPianoSelectAll.Mnemonic(Keys.A);
            cMenuPianoSelectAllEvents.Text = _("Select All Events");
            cMenuPianoSelectAllEvents.Mnemonic(Keys.E);

            cMenuPianoExpressionProperty.Text = _("Note Expression Property");
            cMenuPianoExpressionProperty.Mnemonic(Keys.P);
            cMenuPianoVibratoProperty.Text = _("Note Vibrato Property");
            cMenuPianoImportLyric.Text = _("Insert Lyrics");
            cMenuPianoImportLyric.Mnemonic(Keys.P);
            #endregion

            #region cMenuTrackTab
            cMenuTrackTabTrackOn.Text = _("Track On");
            cMenuTrackTabTrackOn.Mnemonic(Keys.K);
            cMenuTrackTabAdd.Text = _("Add Track");
            cMenuTrackTabAdd.Mnemonic(Keys.A);
            cMenuTrackTabCopy.Text = _("Copy Track");
            cMenuTrackTabCopy.Mnemonic(Keys.C);
            cMenuTrackTabChangeName.Text = _("Rename Track");
            cMenuTrackTabDelete.Text = _("Delete Track");
            cMenuTrackTabDelete.Mnemonic(Keys.D);

            cMenuTrackTabRenderCurrent.Text = _("Render Current Track");
            cMenuTrackTabRenderCurrent.Mnemonic(Keys.T);
            cMenuTrackTabRenderAll.Text = _("Render All Tracks");
            cMenuTrackTabRenderAll.Mnemonic(Keys.S);
            cMenuTrackTabOverlay.Text = _("Overlay");
            cMenuTrackTabOverlay.Mnemonic(Keys.O);
            cMenuTrackTabRenderer.Text = _("Renderer");
            cMenuTrackTabRenderer.Mnemonic(Keys.R);
            #endregion

            #region cMenuTrackSelector
            cMenuTrackSelectorPointer.Text = _("Arrow");
            cMenuTrackSelectorPointer.Mnemonic(Keys.A);
            cMenuTrackSelectorPencil.Text = _("Pencil");
            cMenuTrackSelectorPencil.Mnemonic(Keys.W);
            cMenuTrackSelectorLine.Text = _("Line");
            cMenuTrackSelectorLine.Mnemonic(Keys.L);
            cMenuTrackSelectorEraser.Text = _("Eraser");
            cMenuTrackSelectorEraser.Mnemonic(Keys.E);
            cMenuTrackSelectorPaletteTool.Text = _("Palette Tool");

            cMenuTrackSelectorCurve.Text = _("Curve");
            cMenuTrackSelectorCurve.Mnemonic(Keys.V);

            cMenuTrackSelectorUndo.Text = _("Undo");
            cMenuTrackSelectorUndo.Mnemonic(Keys.U);
            cMenuTrackSelectorRedo.Text = _("Redo");
            cMenuTrackSelectorRedo.Mnemonic(Keys.R);

            cMenuTrackSelectorCut.Text = _("Cut");
            cMenuTrackSelectorCut.Mnemonic(Keys.T);
            cMenuTrackSelectorCopy.Text = _("Copy");
            cMenuTrackSelectorCopy.Mnemonic(Keys.C);
            cMenuTrackSelectorPaste.Text = _("Paste");
            cMenuTrackSelectorPaste.Mnemonic(Keys.P);
            cMenuTrackSelectorDelete.Text = _("Delete");
            cMenuTrackSelectorDelete.Mnemonic(Keys.D);
            cMenuTrackSelectorDeleteBezier.Text = _("Delete Bezier Point");
            cMenuTrackSelectorDeleteBezier.Mnemonic(Keys.B);

            cMenuTrackSelectorSelectAll.Text = _("Select All Events");
            cMenuTrackSelectorSelectAll.Mnemonic(Keys.E);
            #endregion

            #region cMenuPositionIndicator
            cMenuPositionIndicatorStartMarker.Text = _("Set start marker");
            cMenuPositionIndicatorEndMarker.Text = _("Set end marker");
            #endregion

            stripLblGameCtrlMode.ToolTipText = _("Game Controler");

            // Palette Tool
#if DEBUG
            AppManager.debugWriteLine("FormMain#applyLanguage; Messaging.Language=" + Messaging.getLanguage());
#endif
#if ENABLE_SCRIPT
            int count = toolBarTool.Buttons.Count;// toolStripTool.getComponentCount();
            for (int i = 0; i < count; i++) {
                Object tsi = toolBarTool.Buttons[i];// toolStripTool.getComponentAtIndex( i );
                if (tsi is System.Windows.Forms.ToolBarButton) {
                    System.Windows.Forms.ToolBarButton tsb = (System.Windows.Forms.ToolBarButton)tsi;
                    if (tsb.Style == System.Windows.Forms.ToolBarButtonStyle.ToggleButton && tsb.Tag != null && tsb.Tag is string) {
                        string id = (string)tsb.Tag;
                        if (PaletteToolServer.loadedTools.ContainsKey(id)) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.loadedTools[id];
                            tsb.Text = ipt.getName(Messaging.getLanguage());
                            tsb.ToolTipText = ipt.getDescription(Messaging.getLanguage());
                        }
                    }
                }
            }

            foreach (var tsi in cMenuPianoPaletteTool.DropDownItems) {
                if (tsi is System.Windows.Forms.ToolStripMenuItem) {
                    var tsmi = (System.Windows.Forms.ToolStripMenuItem)tsi;
                    if (tsmi.Tag != null && tsmi.Tag is string) {
                        string id = (string)tsmi.Tag;
                        if (PaletteToolServer.loadedTools.ContainsKey(id)) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.loadedTools[id];
                            tsmi.Text = ipt.getName(Messaging.getLanguage());
                            tsmi.ToolTipText = ipt.getDescription(Messaging.getLanguage());
                        }
                    }
                }
            }

            foreach (var tsi in cMenuTrackSelectorPaletteTool.DropDownItems) {
                if (tsi is System.Windows.Forms.ToolStripMenuItem) {
                    var tsmi = (System.Windows.Forms.ToolStripMenuItem)tsi;
                    if (tsmi.Tag != null && tsmi.Tag is string) {
                        string id = (string)tsmi.Tag;
                        if (PaletteToolServer.loadedTools.ContainsKey(id)) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.loadedTools[id];
                            tsmi.Text = ipt.getName(Messaging.getLanguage());
                            tsmi.ToolTipText = ipt.getDescription(Messaging.getLanguage());
                        }
                    }
                }
            }

            foreach (var tsi in menuSettingPaletteTool.DropDownItems) {
                if (tsi is System.Windows.Forms.ToolStripMenuItem) {
                    var tsmi = (System.Windows.Forms.ToolStripMenuItem)tsi;
                    if (tsmi.Tag != null && tsmi.Tag is string) {
                        string id = (string)tsmi.Tag;
                        if (PaletteToolServer.loadedTools.ContainsKey(id)) {
                            IPaletteTool ipt = (IPaletteTool)PaletteToolServer.loadedTools[id];
                            tsmi.Text = ipt.getName(Messaging.getLanguage());
                        }
                    }
                }
            }

            foreach (var id in PaletteToolServer.loadedTools.Keys) {
                IPaletteTool ipt = (IPaletteTool)PaletteToolServer.loadedTools[id];
                ipt.applyLanguage(Messaging.getLanguage());
            }
#endif
        }

        /// <summary>
        /// 歌詞の流し込みダイアログを開き，選択された音符を起点に歌詞を流し込みます
        /// </summary>
        public void importLyric()
        {
            int start = 0;
            int selected = AppManager.getSelected();
            VsqFileEx vsq = AppManager.getVsqFile();
            VsqTrack vsq_track = vsq.Track[selected];
            int selectedid = AppManager.itemSelection.getLastEvent().original.InternalID;
            int numEvents = vsq_track.getEventCount();
            for (int i = 0; i < numEvents; i++) {
                if (selectedid == vsq_track.getEvent(i).InternalID) {
                    start = i;
                    break;
                }
            }
            int count = vsq_track.getEventCount() - 1 - start + 1;
            try {
                if (mDialogImportLyric == null) {
                    mDialogImportLyric = new FormImportLyric(count);
                } else {
                    mDialogImportLyric.setMaxNotes(count);
                }
                mDialogImportLyric.Location = getFormPreferedLocation(mDialogImportLyric);
                DialogResult dr = AppManager.showModalDialog(mDialogImportLyric, this);
                if (dr == DialogResult.OK) {
                    string[] phrases = mDialogImportLyric.getLetters();
#if DEBUG
                    foreach (string s in phrases) {
                        sout.println("FormMain#importLyric; phrases; s=" + s);
                    }
#endif
                    int min = Math.Min(count, phrases.Length);
                    List<string> new_phrases = new List<string>();
                    List<string> new_symbols = new List<string>();
                    for (int i = 0; i < phrases.Length; i++) {
                        SymbolTableEntry entry = SymbolTable.attatch(phrases[i]);
                        if (new_phrases.Count + 1 > count) {
                            break;
                        }
                        if (entry == null) {
                            new_phrases.Add(phrases[i]);
                            new_symbols.Add("a");
                        } else {
                            if (entry.Word.IndexOf('-') >= 0) {
                                // 分節に分割する必要がある
                                string[] spl = PortUtil.splitString(entry.Word, '\t');
                                if (new_phrases.Count + spl.Length > count) {
                                    // 分節の全部を分割すると制限個数を超えてしまう
                                    // 分割せずにハイフンを付けたまま登録
                                    new_phrases.Add(entry.Word.Replace("\t", ""));
                                    new_symbols.Add(entry.getSymbol());
                                } else {
                                    string[] spl_symbol = PortUtil.splitString(entry.getRawSymbol(), '\t');
                                    for (int j = 0; j < spl.Length; j++) {
                                        new_phrases.Add(spl[j]);
                                        new_symbols.Add(spl_symbol[j]);
                                    }
                                }
                            } else {
                                // 分節に分割しない
                                new_phrases.Add(phrases[i]);
                                new_symbols.Add(entry.getSymbol());
                            }
                        }
                    }
                    VsqEvent[] new_events = new VsqEvent[new_phrases.Count];
                    int indx = -1;
                    for (Iterator<int> itr = vsq_track.indexIterator(IndexIteratorKind.NOTE); itr.hasNext(); ) {
                        int index = itr.next();
                        if (index < start) {
                            continue;
                        }
                        indx++;
                        VsqEvent item = vsq_track.getEvent(index);
                        new_events[indx] = (VsqEvent)item.clone();
                        new_events[indx].ID.LyricHandle.L0.Phrase = new_phrases[indx];
                        new_events[indx].ID.LyricHandle.L0.setPhoneticSymbol(new_symbols[indx]);
                        AppManager.applyUtauParameter(vsq_track, new_events[indx]);
                        if (indx + 1 >= new_phrases.Count) {
                            break;
                        }
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventReplaceRange(selected, new_events));
                    AppManager.editHistory.register(vsq.executeCommand(run));
                    setEdited(true);
                    Refresh();
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".importLyric; ex=" + ex + "\n");
            } finally {
                mDialogImportLyric.Hide();
            }
        }

        /// <summary>
        /// 選択されている音符のビブラートを編集するためのダイアログを起動し、編集を行います。
        /// </summary>
        public void editNoteVibratoProperty()
        {
            SelectedEventEntry item = AppManager.itemSelection.getLastEvent();
            if (item == null) {
                return;
            }

            VsqEvent ev = item.original;
            int selected = AppManager.getSelected();
            VsqFileEx vsq = AppManager.getVsqFile();
            RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
            SynthesizerType type = SynthesizerType.VOCALOID2;
            if (kind == RendererKind.VOCALOID1) {
                type = SynthesizerType.VOCALOID1;
            }
            FormVibratoConfig dlg = null;
            try {
                dlg = new FormVibratoConfig(
                    ev.ID.VibratoHandle,
                    ev.ID.getLength(),
                    AppManager.editorConfig.DefaultVibratoLength,
                    type,
                    AppManager.editorConfig.UseUserDefinedAutoVibratoType);
                dlg.Location = getFormPreferedLocation(dlg);
                DialogResult dr = AppManager.showModalDialog(dlg, this);
                if (dlg.DialogResult == DialogResult.OK) {
                    VsqEvent edited = (VsqEvent)ev.clone();
                    if (dlg.getVibratoHandle() != null) {
                        edited.ID.VibratoHandle = (VibratoHandle)dlg.getVibratoHandle().clone();
                        //edited.ID.VibratoHandle.setStartDepth( AppManager.editorConfig.DefaultVibratoDepth );
                        //edited.ID.VibratoHandle.setStartRate( AppManager.editorConfig.DefaultVibratoRate );
                        edited.ID.VibratoDelay = ev.ID.getLength() - dlg.getVibratoHandle().getLength();
                    } else {
                        edited.ID.VibratoHandle = null;
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventChangeIDContaints(selected, ev.InternalID, edited.ID));
                    AppManager.editHistory.register(vsq.executeCommand(run));
                    setEdited(true);
                    refreshScreen();
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".editNoteVibratoProperty; ex=" + ex + "\n");
            } finally {
                if (dlg != null) {
                    try {
                        dlg.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".editNoteVibratoProperty; ex=" + ex2 + "\n");
                    }
                }
            }
        }

        /// <summary>
        /// 選択されている音符の表情を編集するためのダイアログを起動し、編集を行います。
        /// </summary>
        public void editNoteExpressionProperty()
        {
            SelectedEventEntry item = AppManager.itemSelection.getLastEvent();
            if (item == null) {
                return;
            }

            VsqEvent ev = item.original;
            SynthesizerType type = SynthesizerType.VOCALOID2;
            int selected = AppManager.getSelected();
            VsqFileEx vsq = AppManager.getVsqFile();
            RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
            if (kind == RendererKind.VOCALOID1) {
                type = SynthesizerType.VOCALOID1;
            }
            FormNoteExpressionConfig dlg = null;
            try {
                dlg = new FormNoteExpressionConfig(type, ev.ID.NoteHeadHandle);
                dlg.setPMBendDepth(ev.ID.PMBendDepth);
                dlg.setPMBendLength(ev.ID.PMBendLength);
                dlg.setPMbPortamentoUse(ev.ID.PMbPortamentoUse);
                dlg.setDEMdecGainRate(ev.ID.DEMdecGainRate);
                dlg.setDEMaccent(ev.ID.DEMaccent);

                dlg.Location = getFormPreferedLocation(dlg);
                DialogResult dr = AppManager.showModalDialog(dlg, this);
                if (dr == DialogResult.OK) {
                    VsqEvent edited = (VsqEvent)ev.clone();
                    edited.ID.PMBendDepth = dlg.getPMBendDepth();
                    edited.ID.PMBendLength = dlg.getPMBendLength();
                    edited.ID.PMbPortamentoUse = dlg.getPMbPortamentoUse();
                    edited.ID.DEMdecGainRate = dlg.getDEMdecGainRate();
                    edited.ID.DEMaccent = dlg.getDEMaccent();
                    edited.ID.NoteHeadHandle = dlg.getEditedNoteHeadHandle();
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventChangeIDContaints(selected, ev.InternalID, edited.ID));
                    AppManager.editHistory.register(vsq.executeCommand(run));
                    setEdited(true);
                    refreshScreen();
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".editNoteExpressionProperty; ex=" + ex + "\n");
            } finally {
                if (dlg != null) {
                    try {
                        dlg.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".editNoteExpressionProperty; ex=" + ex2 + "\n");
                    }
                }
            }
        }

        /// <summary>
        /// マウスのスクロールによって受け取ったスクロール幅から、実際に縦スクロールバーに渡す値(候補値)を計算します。
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public int computeScrollValueFromWheelDelta(int delta)
        {
            double new_val = (double)hScroll.Value - delta * AppManager.editorConfig.WheelOrder / (5.0 * controller.getScaleX());
            if (new_val < 0.0) {
                new_val = 0;
            }
            int max = hScroll.Maximum - hScroll.LargeChange;
            int draft = (int)new_val;
            if (draft > max) {
                draft = max;
            } else if (draft < hScroll.Minimum) {
                draft = hScroll.Minimum;
            }
            return draft;
        }

        #region 音符の編集関連
        public void selectAll()
        {

            AppManager.itemSelection.clearEvent();
            AppManager.itemSelection.clearTempo();
            AppManager.itemSelection.clearTimesig();
            AppManager.itemSelection.clearPoint();
            int min = int.MaxValue;
            int max = int.MinValue;
            int premeasure = AppManager.getVsqFile().getPreMeasureClocks();
            List<int> add_required = new List<int>();
            for (Iterator<VsqEvent> itr = AppManager.getVsqFile().Track[AppManager.getSelected()].getEventIterator(); itr.hasNext(); ) {
                VsqEvent ve = itr.next();
                if (premeasure <= ve.Clock) {
                    add_required.Add(ve.InternalID);
                    min = Math.Min(min, ve.Clock);
                    max = Math.Max(max, ve.Clock + ve.ID.getLength());
                }
            }
            if (add_required.Count > 0) {
                AppManager.itemSelection.addEventAll(add_required);
            }
            foreach (CurveType vct in Utility.CURVE_USAGE) {
                if (vct.isScalar() || vct.isAttachNote()) {
                    continue;
                }
                VsqBPList target = AppManager.getVsqFile().Track[AppManager.getSelected()].getCurve(vct.getName());
                if (target == null) {
                    continue;
                }
                int count = target.size();
                if (count >= 1) {
                    //int[] keys = target.getKeys();
                    int max_key = target.getKeyClock(count - 1);
                    max = Math.Max(max, target.getValue(max_key));
                    for (int i = 0; i < count; i++) {
                        int key = target.getKeyClock(i);
                        if (premeasure <= key) {
                            min = Math.Min(min, key);
                            break;
                        }
                    }
                }
            }
            if (min < premeasure) {
                min = premeasure;
            }
            if (min < max) {
                //int stdx = AppManager.startToDrawX;
                //min = xCoordFromClocks( min ) + stdx;
                //max = xCoordFromClocks( max ) + stdx;
                AppManager.mWholeSelectedInterval = new SelectedRegion(min);
                AppManager.mWholeSelectedInterval.setEnd(max);
                AppManager.setWholeSelectedIntervalEnabled(true);
            }
        }

        public void selectAllEvent()
        {
            AppManager.itemSelection.clearTempo();
            AppManager.itemSelection.clearTimesig();
            AppManager.itemSelection.clearEvent();
            AppManager.itemSelection.clearPoint();
            int selected = AppManager.getSelected();
            VsqFileEx vsq = AppManager.getVsqFile();
            VsqTrack vsq_track = vsq.Track[selected];
            int premeasureclock = vsq.getPreMeasureClocks();
            List<int> add_required = new List<int>();
            for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                VsqEvent ev = itr.next();
                if (ev.ID.type == VsqIDType.Anote && ev.Clock >= premeasureclock) {
                    add_required.Add(ev.InternalID);
                }
            }
            if (add_required.Count > 0) {
                AppManager.itemSelection.addEventAll(add_required);
            }
            refreshScreen();
        }

        public void deleteEvent()
        {
#if DEBUG
            AppManager.debugWriteLine(
                "FormMain#deleteEvent(); AppManager.mInputTextBox.isEnabled()=" +
                AppManager.mInputTextBox.Enabled);
#endif

            if (AppManager.mInputTextBox.Visible) {
                return;
            }
#if ENABLE_PROPERTY
            if (AppManager.propertyPanel.isEditing()) {
                return;
            }
#endif

            int selected = AppManager.getSelected();
            VsqFileEx vsq = AppManager.getVsqFile();
            VsqTrack vsq_track = vsq.Track[selected];

            if (AppManager.itemSelection.getEventCount() > 0) {
                List<int> ids = new List<int>();
                bool contains_aicon = false;
                foreach (var ev in AppManager.itemSelection.getEventIterator()) {
                    ids.Add(ev.original.InternalID);
                    if (ev.original.ID.type == VsqIDType.Aicon) {
                        contains_aicon = true;
                    }
                }
                VsqCommand run = VsqCommand.generateCommandEventDeleteRange(selected, ids);
                if (AppManager.isWholeSelectedIntervalEnabled()) {
                    VsqFileEx work = (VsqFileEx)vsq.clone();
                    work.executeCommand(run);
                    int stdx = controller.getStartToDrawX();
                    int start_clock = AppManager.mWholeSelectedInterval.getStart();
                    int end_clock = AppManager.mWholeSelectedInterval.getEnd();
                    List<List<BPPair>> curves = new List<List<BPPair>>();
                    List<CurveType> types = new List<CurveType>();
                    VsqTrack work_vsq_track = work.Track[selected];
                    foreach (CurveType vct in Utility.CURVE_USAGE) {
                        if (vct.isScalar() || vct.isAttachNote()) {
                            continue;
                        }
                        VsqBPList work_curve = work_vsq_track.getCurve(vct.getName());
                        List<BPPair> t = new List<BPPair>();
                        t.Add(new BPPair(start_clock, work_curve.getValue(start_clock)));
                        t.Add(new BPPair(end_clock, work_curve.getValue(end_clock)));
                        curves.Add(t);
                        types.Add(vct);
                    }
                    List<string> strs = new List<string>();
                    for (int i = 0; i < types.Count; i++) {
                        strs.Add(types[i].getName());
                    }
                    CadenciiCommand delete_curve = new CadenciiCommand(
                        VsqCommand.generateCommandTrackCurveEditRange(selected, strs, curves));
                    work.executeCommand(delete_curve);
                    if (contains_aicon) {
                        work.Track[selected].reflectDynamics();
                    }
                    CadenciiCommand run2 = new CadenciiCommand(VsqCommand.generateCommandReplace(work));
                    AppManager.editHistory.register(vsq.executeCommand(run2));
                    setEdited(true);
                } else {
                    CadenciiCommand run2 = null;
                    if (contains_aicon) {
                        VsqFileEx work = (VsqFileEx)vsq.clone();
                        work.executeCommand(run);
                        VsqTrack vsq_track_copied = work.Track[selected];
                        vsq_track_copied.reflectDynamics();
                        run2 = VsqFileEx.generateCommandTrackReplace(selected,
                                                                      vsq_track_copied,
                                                                      work.AttachedCurves.get(selected - 1));
                    } else {
                        run2 = new CadenciiCommand(run);
                    }
                    AppManager.editHistory.register(vsq.executeCommand(run2));
                    setEdited(true);
                    AppManager.itemSelection.clearEvent();
                }
                Refresh();
            } else if (AppManager.itemSelection.getTempoCount() > 0) {
                List<int> clocks = new List<int>();
                foreach (var item in AppManager.itemSelection.getTempoIterator()) {
                    if (item.getKey() <= 0) {
                        string msg = _("Cannot remove first symbol of track!");
                        statusLabel.Text = msg;
                        SystemSounds.Asterisk.Play();
                        return;
                    }
                    clocks.Add(item.getKey());
                }
                int[] dum = new int[clocks.Count];
                for (int i = 0; i < dum.Length; i++) {
                    dum[i] = -1;
                }
                CadenciiCommand run = new CadenciiCommand(
                    VsqCommand.generateCommandUpdateTempoRange(PortUtil.convertIntArray(clocks.ToArray()),
                                                                PortUtil.convertIntArray(clocks.ToArray()),
                                                                dum));
                AppManager.editHistory.register(vsq.executeCommand(run));
                setEdited(true);
                AppManager.itemSelection.clearTempo();
                Refresh();
            } else if (AppManager.itemSelection.getTimesigCount() > 0) {
#if DEBUG
                AppManager.debugWriteLine("    Timesig");
#endif
                int[] barcounts = new int[AppManager.itemSelection.getTimesigCount()];
                int[] numerators = new int[AppManager.itemSelection.getTimesigCount()];
                int[] denominators = new int[AppManager.itemSelection.getTimesigCount()];
                int count = -1;
                foreach (var item in AppManager.itemSelection.getTimesigIterator()) {
                    int key = item.getKey();
                    SelectedTimesigEntry value = item.getValue();
                    count++;
                    barcounts[count] = key;
                    if (key <= 0) {
                        string msg = "Cannot remove first symbol of track!";
                        statusLabel.Text = _(msg);
                        SystemSounds.Asterisk.Play();
                        return;
                    }
                    numerators[count] = -1;
                    denominators[count] = -1;
                }
                CadenciiCommand run = new CadenciiCommand(
                    VsqCommand.generateCommandUpdateTimesigRange(barcounts, barcounts, numerators, denominators));
                AppManager.editHistory.register(vsq.executeCommand(run));
                setEdited(true);
                AppManager.itemSelection.clearTimesig();
                Refresh();
            }
            if (AppManager.itemSelection.getPointIDCount() > 0) {
#if DEBUG
                AppManager.debugWriteLine("    Curve");
#endif
                string curve;
                if (!trackSelector.getSelectedCurve().isAttachNote()) {
                    curve = trackSelector.getSelectedCurve().getName();
                    VsqBPList src = vsq_track.getCurve(curve);
                    VsqBPList list = (VsqBPList)src.clone();
                    List<int> remove_clock_queue = new List<int>();
                    int count = list.size();
                    for (int i = 0; i < count; i++) {
                        VsqBPPair item = list.getElementB(i);
                        if (AppManager.itemSelection.isPointContains(item.id)) {
                            remove_clock_queue.Add(list.getKeyClock(i));
                        }
                    }
                    count = remove_clock_queue.Count;
                    for (int i = 0; i < count; i++) {
                        list.remove(remove_clock_queue[i]);
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandTrackCurveReplace(selected,
                                                                     trackSelector.getSelectedCurve().getName(),
                                                                     list));
                    AppManager.editHistory.register(vsq.executeCommand(run));
                    setEdited(true);
                } else {
                    //todo: FormMain+DeleteEvent; VibratoDepth, VibratoRateの場合
                }
                AppManager.itemSelection.clearPoint();
                refreshScreen();
            }
        }

        public void pasteEvent()
        {
            int clock = AppManager.getCurrentClock();
            int unit = AppManager.getPositionQuantizeClock();
            clock = doQuantize(clock, unit);

            VsqCommand add_event = null; // VsqEventを追加するコマンド

            ClipboardEntry ce = AppManager.clipboard.getCopiedItems();
            int copy_started_clock = ce.copyStartedClock;
            List<VsqEvent> copied_events = ce.events;
#if DEBUG
            sout.println("FormMain#pasteEvent; copy_started_clock=" + copy_started_clock);
            sout.println("FormMain#pasteEvent; copied_events.size()=" + copied_events.Count);
#endif
            if (copied_events.Count != 0) {
                // VsqEventのペーストを行うコマンドを発行
                int dclock = clock - copy_started_clock;
                if (clock >= AppManager.getVsqFile().getPreMeasureClocks()) {
                    List<VsqEvent> paste = new List<VsqEvent>();
                    int count = copied_events.Count;
                    for (int i = 0; i < count; i++) {
                        VsqEvent item = (VsqEvent)copied_events[i].clone();
                        item.Clock = copied_events[i].Clock + dclock;
                        paste.Add(item);
                    }
                    add_event = VsqCommand.generateCommandEventAddRange(
                        AppManager.getSelected(), paste.ToArray());
                }
            }
            List<TempoTableEntry> copied_tempo = ce.tempo;
            if (copied_tempo.Count != 0) {
                // テンポ変更の貼付けを実行
                int dclock = clock - copy_started_clock;
                int count = copied_tempo.Count;
                int[] clocks = new int[count];
                int[] tempos = new int[count];
                for (int i = 0; i < count; i++) {
                    TempoTableEntry item = copied_tempo[i];
                    clocks[i] = item.Clock + dclock;
                    tempos[i] = item.Tempo;
                }
                CadenciiCommand run = new CadenciiCommand(
                    VsqCommand.generateCommandUpdateTempoRange(clocks, clocks, tempos));
                AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                setEdited(true);
                refreshScreen();
                return;
            }
            List<TimeSigTableEntry> copied_timesig = ce.timesig;
            if (copied_timesig.Count > 0) {
                // 拍子変更の貼付けを実行
                int bar_count = AppManager.getVsqFile().getBarCountFromClock(clock);
                int min_barcount = copied_timesig[0].BarCount;
                foreach (var tste in copied_timesig) {
                    min_barcount = Math.Min(min_barcount, tste.BarCount);
                }
                int dbarcount = bar_count - min_barcount;
                int count = copied_timesig.Count;
                int[] barcounts = new int[count];
                int[] numerators = new int[count];
                int[] denominators = new int[count];
                for (int i = 0; i < count; i++) {
                    TimeSigTableEntry item = copied_timesig[i];
                    barcounts[i] = item.BarCount + dbarcount;
                    numerators[i] = item.Numerator;
                    denominators[i] = item.Denominator;
                }
                CadenciiCommand run = new CadenciiCommand(
                    VsqCommand.generateCommandUpdateTimesigRange(
                        barcounts, barcounts, numerators, denominators));
                AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                setEdited(true);
                refreshScreen();
                return;
            }

            // BPPairの貼付け
            VsqCommand edit_bpcurve = null; // BPListを変更するコマンド
            SortedDictionary<CurveType, VsqBPList> copied_curve = ce.points;
#if DEBUG
            sout.println("FormMain#pasteEvent; copied_curve.size()=" + copied_curve.Count);
#endif
            if (copied_curve.Count > 0) {
                int dclock = clock - copy_started_clock;

                SortedDictionary<string, VsqBPList> work = new SortedDictionary<string, VsqBPList>();
                foreach (var curve in copied_curve.Keys) {
                    VsqBPList list = copied_curve[curve];
#if DEBUG
                    AppManager.debugWriteLine("FormMain#pasteEvent; curve=" + curve);
#endif
                    if (curve.isScalar()) {
                        continue;
                    }
                    if (list.size() <= 0) {
                        continue;
                    }
                    if (curve.isAttachNote()) {
                        //todo: FormMain+PasteEvent; VibratoRate, VibratoDepthカーブのペースト処理
                    } else {
                        VsqBPList target = (VsqBPList)AppManager.getVsqFile().Track[AppManager.getSelected()].getCurve(curve.getName()).clone();
                        int count = list.size();
#if DEBUG
                        sout.println("FormMain#pasteEvent; list.getCount()=" + count);
#endif
                        int min = list.getKeyClock(0) + dclock;
                        int max = list.getKeyClock(count - 1) + dclock;
                        int valueAtEnd = target.getValue(max);
                        for (int i = 0; i < target.size(); i++) {
                            int cl = target.getKeyClock(i);
                            if (min <= cl && cl <= max) {
                                target.removeElementAt(i);
                                i--;
                            }
                        }
                        int lastClock = min;
                        for (int i = 0; i < count - 1; i++) {
                            lastClock = list.getKeyClock(i) + dclock;
                            target.add(lastClock, list.getElementA(i));
                        }
                        // 最後のやつ
                        if (lastClock < max - 1) {
                            target.add(max - 1, list.getElementA(count - 1));
                        }
                        target.add(max, valueAtEnd);
                        if (copied_curve.Count == 1) {
                            work[trackSelector.getSelectedCurve().getName()] = target;
                        } else {
                            work[curve.getName()] = target;
                        }
                    }
                }
#if DEBUG
                sout.println("FormMain#pasteEvent; work.size()=" + work.Count);
#endif
                if (work.Count > 0) {
                    string[] curves = new string[work.Count];
                    VsqBPList[] bplists = new VsqBPList[work.Count];
                    int count = -1;
                    foreach (var s in work.Keys) {
                        count++;
                        curves[count] = s;
                        bplists[count] = work[s];
                    }
                    edit_bpcurve = VsqCommand.generateCommandTrackCurveReplaceRange(AppManager.getSelected(), curves, bplists);
                }
                AppManager.itemSelection.clearPoint();
            }

            // ベジエ曲線の貼付け
            CadenciiCommand edit_bezier = null;
            SortedDictionary<CurveType, List<BezierChain>> copied_bezier = ce.beziers;
#if DEBUG
            sout.println("FormMain#pasteEvent; copied_bezier.size()=" + copied_bezier.Count);
#endif
            if (copied_bezier.Count > 0) {
                int dclock = clock - copy_started_clock;
                BezierCurves attached_curve = (BezierCurves)AppManager.getVsqFile().AttachedCurves.get(AppManager.getSelected() - 1).clone();
                SortedDictionary<CurveType, List<BezierChain>> command_arg = new SortedDictionary<CurveType, List<BezierChain>>();
                foreach (var curve in copied_bezier.Keys) {
                    if (curve.isScalar()) {
                        continue;
                    }
                    foreach (var bc in copied_bezier[curve]) {
                        BezierChain bc_copy = (BezierChain)bc.clone();
                        foreach (var bp in bc_copy.points) {
                            bp.setBase(new PointD(bp.getBase().getX() + dclock, bp.getBase().getY()));
                        }
                        attached_curve.mergeBezierChain(curve, bc_copy);
                    }
                    List<BezierChain> arg = new List<BezierChain>();
                    foreach (var bc in attached_curve.get(curve)) {
                        arg.Add(bc);
                    }
                    command_arg[curve] = arg;
                }
                edit_bezier = VsqFileEx.generateCommandReplaceAttachedCurveRange(AppManager.getSelected(), command_arg);
            }

            int commands = 0;
            commands += (add_event != null) ? 1 : 0;
            commands += (edit_bpcurve != null) ? 1 : 0;
            commands += (edit_bezier != null) ? 1 : 0;

#if DEBUG
            AppManager.debugWriteLine("FormMain#pasteEvent; commands=" + commands);
            AppManager.debugWriteLine("FormMain#pasteEvent; (add_event != null)=" + (add_event != null));
            AppManager.debugWriteLine("FormMain#pasteEvent; (edit_bpcurve != null)=" + (edit_bpcurve != null));
            AppManager.debugWriteLine("FormMain#pasteEvent; (edit_bezier != null)=" + (edit_bezier != null));
#endif
            if (commands == 1) {
                if (add_event != null) {
                    CadenciiCommand run = new CadenciiCommand(add_event);
                    AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                } else if (edit_bpcurve != null) {
                    CadenciiCommand run = new CadenciiCommand(edit_bpcurve);
                    AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                } else if (edit_bezier != null) {
                    AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(edit_bezier));
                }
                AppManager.getVsqFile().updateTotalClocks();
                setEdited(true);
                refreshScreen();
            } else if (commands > 1) {
                VsqFileEx work = (VsqFileEx)AppManager.getVsqFile().clone();
                if (add_event != null) {
                    work.executeCommand(add_event);
                }
                if (edit_bezier != null) {
                    work.executeCommand(edit_bezier);
                }
                if (edit_bpcurve != null) {
                    // edit_bpcurveのVsqCommandTypeはTrackEditCurveRangeしかありえない
                    work.executeCommand(edit_bpcurve);
                }
                CadenciiCommand run = VsqFileEx.generateCommandReplace(work);
                AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                AppManager.getVsqFile().updateTotalClocks();
                setEdited(true);
                refreshScreen();
            }
        }

        /// <summary>
        /// アイテムのコピーを行います
        /// </summary>
        public void copyEvent()
        {
#if DEBUG
            AppManager.debugWriteLine("FormMain#copyEvent");
#endif
            int min = int.MaxValue; // コピーされたアイテムの中で、最小の開始クロック

            if (AppManager.isWholeSelectedIntervalEnabled()) {
#if DEBUG
                sout.println("FormMain#copyEvent; selected with CTRL key");
#endif
                int stdx = controller.getStartToDrawX();
                int start_clock = AppManager.mWholeSelectedInterval.getStart();
                int end_clock = AppManager.mWholeSelectedInterval.getEnd();
                ClipboardEntry ce = new ClipboardEntry();
                ce.copyStartedClock = start_clock;
                ce.points = new SortedDictionary<CurveType, VsqBPList>();
                ce.beziers = new SortedDictionary<CurveType, List<BezierChain>>();
                for (int i = 0; i < Utility.CURVE_USAGE.Length; i++) {
                    CurveType vct = Utility.CURVE_USAGE[i];
                    VsqBPList list = AppManager.getVsqFile().Track[AppManager.getSelected()].getCurve(vct.getName());
                    if (list == null) {
                        continue;
                    }
                    List<BezierChain> tmp_bezier = new List<BezierChain>();
                    copyCurveCor(AppManager.getSelected(),
                                  vct,
                                  start_clock,
                                  end_clock,
                                  tmp_bezier);
                    VsqBPList tmp_bplist = new VsqBPList(list.getName(), list.getDefault(), list.getMinimum(), list.getMaximum());
                    int c = list.size();
                    for (int j = 0; j < c; j++) {
                        int clock = list.getKeyClock(j);
                        if (start_clock <= clock && clock <= end_clock) {
                            tmp_bplist.add(clock, list.getElement(j));
                        } else if (end_clock < clock) {
                            break;
                        }
                    }
                    ce.beziers[vct] = tmp_bezier;
                    ce.points[vct] = tmp_bplist;
                }

                if (AppManager.itemSelection.getEventCount() > 0) {
                    List<VsqEvent> list = new List<VsqEvent>();
                    foreach (var item in AppManager.itemSelection.getEventIterator()) {
                        if (item.original.ID.type == VsqIDType.Anote) {
                            min = Math.Min(item.original.Clock, min);
                            list.Add((VsqEvent)item.original.clone());
                        }
                    }
                    ce.events = list;
                }
                AppManager.clipboard.setClipboard(ce);
            } else if (AppManager.itemSelection.getEventCount() > 0) {
                List<VsqEvent> list = new List<VsqEvent>();
                foreach (var item in AppManager.itemSelection.getEventIterator()) {
                    min = Math.Min(item.original.Clock, min);
                    list.Add((VsqEvent)item.original.clone());
                }
                AppManager.clipboard.setCopiedEvent(list, min);
            } else if (AppManager.itemSelection.getTempoCount() > 0) {
                List<TempoTableEntry> list = new List<TempoTableEntry>();
                foreach (var item in AppManager.itemSelection.getTempoIterator()) {
                    int key = item.getKey();
                    SelectedTempoEntry value = item.getValue();
                    min = Math.Min(value.original.Clock, min);
                    list.Add((TempoTableEntry)value.original.clone());
                }
                AppManager.clipboard.setCopiedTempo(list, min);
            } else if (AppManager.itemSelection.getTimesigCount() > 0) {
                List<TimeSigTableEntry> list = new List<TimeSigTableEntry>();
                foreach (var item in AppManager.itemSelection.getTimesigIterator()) {
                    int key = item.getKey();
                    SelectedTimesigEntry value = item.getValue();
                    min = Math.Min(value.original.Clock, min);
                    list.Add((TimeSigTableEntry)value.original.clone());
                }
                AppManager.clipboard.setCopiedTimesig(list, min);
            } else if (AppManager.itemSelection.getPointIDCount() > 0) {
                ClipboardEntry ce = new ClipboardEntry();
                ce.points = new SortedDictionary<CurveType, VsqBPList>();
                ce.beziers = new SortedDictionary<CurveType, List<BezierChain>>();

                ValuePair<int, int> t = trackSelector.getSelectedRegion();
                int start = t.getKey();
                int end = t.getValue();
                ce.copyStartedClock = start;
                List<BezierChain> tmp_bezier = new List<BezierChain>();
                copyCurveCor(AppManager.getSelected(),
                              trackSelector.getSelectedCurve(),
                              start,
                              end,
                              tmp_bezier);
                if (tmp_bezier.Count > 0) {
                    // ベジエ曲線が1個以上コピーされた場合
                    // 範囲内のデータ点を追加する
                    ce.beziers[trackSelector.getSelectedCurve()] = tmp_bezier;
                    CurveType curve = trackSelector.getSelectedCurve();
                    VsqBPList list = AppManager.getVsqFile().Track[AppManager.getSelected()].getCurve(curve.getName());
                    if (list != null) {
                        VsqBPList tmp_bplist = new VsqBPList(list.getName(), list.getDefault(), list.getMinimum(), list.getMaximum());
                        int c = list.size();
                        for (int i = 0; i < c; i++) {
                            int clock = list.getKeyClock(i);
                            if (start <= clock && clock <= end) {
                                tmp_bplist.add(clock, list.getElement(i));
                            } else if (end < clock) {
                                break;
                            }
                        }
                        ce.points[curve] = tmp_bplist;
                    }
                } else {
                    // ベジエ曲線がコピーされなかった場合
                    // AppManager.selectedPointIDIteratorの中身のみを選択
                    CurveType curve = trackSelector.getSelectedCurve();
                    VsqBPList list = AppManager.getVsqFile().Track[AppManager.getSelected()].getCurve(curve.getName());
                    if (list != null) {
                        VsqBPList tmp_bplist = new VsqBPList(curve.getName(), curve.getDefault(), curve.getMinimum(), curve.getMaximum());
                        foreach (var id in AppManager.itemSelection.getPointIDIterator()) {
                            VsqBPPairSearchContext cxt = list.findElement(id);
                            if (cxt.index >= 0) {
                                tmp_bplist.add(cxt.clock, cxt.point.value);
                            }
                        }
                        if (tmp_bplist.size() > 0) {
                            ce.copyStartedClock = tmp_bplist.getKeyClock(0);
                            ce.points[curve] = tmp_bplist;
                        }
                    }
                }
                AppManager.clipboard.setClipboard(ce);
            }
        }

        public void cutEvent()
        {
            // まずコピー
            copyEvent();

            int track = AppManager.getSelected();

            // 選択されたノートイベントがあれば、まず、削除を行うコマンドを発行
            VsqCommand delete_event = null;
            bool other_command_executed = false;
            if (AppManager.itemSelection.getEventCount() > 0) {
                List<int> ids = new List<int>();
                foreach (var item in AppManager.itemSelection.getEventIterator()) {
                    ids.Add(item.original.InternalID);
                }
                delete_event = VsqCommand.generateCommandEventDeleteRange(AppManager.getSelected(), ids);
            }

            // Ctrlキーを押しながらドラッグしたか、そうでないかで分岐
            if (AppManager.isWholeSelectedIntervalEnabled() || AppManager.itemSelection.getPointIDCount() > 0) {
                int stdx = controller.getStartToDrawX();
                int start_clock, end_clock;
                if (AppManager.isWholeSelectedIntervalEnabled()) {
                    start_clock = AppManager.mWholeSelectedInterval.getStart();
                    end_clock = AppManager.mWholeSelectedInterval.getEnd();
                } else {
                    start_clock = trackSelector.getSelectedRegion().getKey();
                    end_clock = trackSelector.getSelectedRegion().getValue();
                }

                // クローンを作成
                VsqFileEx work = (VsqFileEx)AppManager.getVsqFile().clone();
                if (delete_event != null) {
                    // 選択されたノートイベントがあれば、クローンに対して削除を実行
                    work.executeCommand(delete_event);
                }

                // BPListに削除処理を施す
                for (int i = 0; i < Utility.CURVE_USAGE.Length; i++) {
                    CurveType curve = Utility.CURVE_USAGE[i];
                    VsqBPList list = work.Track[track].getCurve(curve.getName());
                    if (list == null) {
                        continue;
                    }
                    int c = list.size();
                    List<long> delete = new List<long>();
                    if (AppManager.isWholeSelectedIntervalEnabled()) {
                        // 一括選択モード
                        for (int j = 0; j < c; j++) {
                            int clock = list.getKeyClock(j);
                            if (start_clock <= clock && clock <= end_clock) {
                                delete.Add(list.getElementB(j).id);
                            } else if (end_clock < clock) {
                                break;
                            }
                        }
                    } else {
                        // 普通の範囲選択
                        foreach (var id in AppManager.itemSelection.getPointIDIterator()) {
                            delete.Add(id);
                        }
                    }
                    VsqCommand tmp = VsqCommand.generateCommandTrackCurveEdit2(track, curve.getName(), delete, new SortedDictionary<int, VsqBPPair>());
                    work.executeCommand(tmp);
                }

                // ベジエ曲線に削除処理を施す
                List<CurveType> target_curve = new List<CurveType>();
                if (AppManager.isWholeSelectedIntervalEnabled()) {
                    // ctrlによる全選択モード
                    for (int i = 0; i < Utility.CURVE_USAGE.Length; i++) {
                        CurveType ct = Utility.CURVE_USAGE[i];
                        if (ct.isScalar() || ct.isAttachNote()) {
                            continue;
                        }
                        target_curve.Add(ct);
                    }
                } else {
                    // 普通の選択モード
                    target_curve.Add(trackSelector.getSelectedCurve());
                }
                work.AttachedCurves.get(AppManager.getSelected() - 1).deleteBeziers(target_curve, start_clock, end_clock);

                // コマンドを発行し、実行
                CadenciiCommand run = VsqFileEx.generateCommandReplace(work);
                AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                this.setEdited(true);

                other_command_executed = true;
            } else if (AppManager.itemSelection.getTempoCount() > 0) {
                // テンポ変更のカット
                int count = -1;
                int[] dum = new int[AppManager.itemSelection.getTempoCount()];
                int[] clocks = new int[AppManager.itemSelection.getTempoCount()];
                foreach (var item in AppManager.itemSelection.getTempoIterator()) {
                    int key = item.getKey();
                    SelectedTempoEntry value = item.getValue();
                    count++;
                    dum[count] = -1;
                    clocks[count] = value.original.Clock;
                }
                CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandUpdateTempoRange(clocks, clocks, dum));
                AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                setEdited(true);
                other_command_executed = true;
            } else if (AppManager.itemSelection.getTimesigCount() > 0) {
                // 拍子変更のカット
                int[] barcounts = new int[AppManager.itemSelection.getTimesigCount()];
                int[] numerators = new int[AppManager.itemSelection.getTimesigCount()];
                int[] denominators = new int[AppManager.itemSelection.getTimesigCount()];
                int count = -1;
                foreach (var item in AppManager.itemSelection.getTimesigIterator()) {
                    int key = item.getKey();
                    SelectedTimesigEntry value = item.getValue();
                    count++;
                    barcounts[count] = value.original.BarCount;
                    numerators[count] = -1;
                    denominators[count] = -1;
                }
                CadenciiCommand run = new CadenciiCommand(
                    VsqCommand.generateCommandUpdateTimesigRange(barcounts, barcounts, numerators, denominators));
                AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                setEdited(true);
                other_command_executed = true;
            }

            // 冒頭で作成した音符イベント削除以外に、コマンドが実行されなかった場合
            if (delete_event != null && !other_command_executed) {
                CadenciiCommand run = new CadenciiCommand(delete_event);
                AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                setEdited(true);
            }

            refreshScreen();
        }

        public void copyCurveCor(
            int track,
            CurveType curve_type,
            int start,
            int end,
            List<BezierChain> copied_chain
        )
        {
            foreach (var bc in AppManager.getVsqFile().AttachedCurves.get(track - 1).get(curve_type)) {
                int len = bc.points.Count;
                if (len < 2) {
                    continue;
                }
                int chain_start = (int)bc.points[0].getBase().getX();
                int chain_end = (int)bc.points[len - 1].getBase().getX();
                BezierChain add = null;
                if (start < chain_start && chain_start < end && end < chain_end) {
                    // (1) chain_start ~ end をコピー
                    try {
                        add = bc.extractPartialBezier(chain_start, end);
                    } catch (Exception ex) {
                        Logger.write(typeof(FormMain) + ".copyCurveCor; ex=" + ex + "\n");
                        add = null;
                    }
                } else if (chain_start <= start && end <= chain_end) {
                    // (2) start ~ endをコピー
                    try {
                        add = bc.extractPartialBezier(start, end);
                    } catch (Exception ex) {
                        Logger.write(typeof(FormMain) + ".copyCurveCor; ex=" + ex + "\n");
                        add = null;
                    }
                } else if (chain_start < start && start < chain_end && chain_end <= end) {
                    // (3) start ~ chain_endをコピー
                    try {
                        add = bc.extractPartialBezier(start, chain_end);
                    } catch (Exception ex) {
                        Logger.write(typeof(FormMain) + ".copyCurveCor; ex=" + ex + "\n");
                        add = null;
                    }
                } else if (start <= chain_start && chain_end <= end) {
                    // (4) 全部コピーでOK
                    add = (BezierChain)bc.clone();
                }
                if (add != null) {
                    copied_chain.Add(add);
                }
            }
        }
        #endregion

        #region トラックの編集関連
        /// <summary>
        /// トラック全体のコピーを行います。
        /// </summary>
        public void copyTrackCore()
        {
            VsqFileEx vsq = AppManager.getVsqFile();
            int selected = AppManager.getSelected();
            VsqTrack track = (VsqTrack)vsq.Track[selected].clone();
            track.setName(track.getName() + " (1)");
            CadenciiCommand run = VsqFileEx.generateCommandAddTrack(track,
                                                                     vsq.Mixer.Slave[selected - 1],
                                                                     vsq.Track.Count,
                                                                     vsq.AttachedCurves.get(selected - 1)); ;
            AppManager.editHistory.register(vsq.executeCommand(run));
            setEdited(true);
            AppManager.mMixerWindow.updateStatus();
            refreshScreen();
        }

        /// <summary>
        /// トラックの名前変更を行います。
        /// </summary>
        public void changeTrackNameCore()
        {
            InputBox ib = null;
            try {
                int selected = AppManager.getSelected();
                VsqFileEx vsq = AppManager.getVsqFile();
                ib = new InputBox(_("Input new name of track"));
                ib.setResult(vsq.Track[selected].getName());
                ib.Location = getFormPreferedLocation(ib);
                DialogResult dr = AppManager.showModalDialog(ib, this);
                if (dr == DialogResult.OK) {
                    string ret = ib.getResult();
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandTrackChangeName(selected, ret));
                    AppManager.editHistory.register(vsq.executeCommand(run));
                    setEdited(true);
                    refreshScreen();
                }
            } catch (Exception ex) {
            } finally {
                if (ib != null) {
                    ib.Close();
                }
            }
        }

        /// <summary>
        /// トラックの削除を行います。
        /// </summary>
        public void deleteTrackCore()
        {
            int selected = AppManager.getSelected();
            VsqFileEx vsq = AppManager.getVsqFile();
            if (AppManager.showMessageBox(
                    PortUtil.formatMessage(_("Do you wish to remove track? {0} : '{1}'"), selected, vsq.Track[selected].getName()),
                    _APP_NAME,
                    cadencii.windows.forms.Utility.MSGBOX_YES_NO_OPTION,
                    cadencii.windows.forms.Utility.MSGBOX_QUESTION_MESSAGE) == DialogResult.Yes) {
                CadenciiCommand run = VsqFileEx.generateCommandDeleteTrack(selected);
                if (selected >= 2) {
                    AppManager.setSelected(selected - 1);
                }
                AppManager.editHistory.register(vsq.executeCommand(run));
                updateDrawObjectList();
                setEdited(true);
                AppManager.mMixerWindow.updateStatus();
                refreshScreen();
            }
        }

        /// <summary>
        /// トラックの追加を行います。
        /// </summary>
        public void addTrackCore()
        {
            VsqFileEx vsq = AppManager.getVsqFile();
            int i = vsq.Track.Count;
            string name = "Voice" + i;
            string singer = AppManager.editorConfig.DefaultSingerName;
            VsqTrack vsq_track = new VsqTrack(name, singer);

            RendererKind kind = AppManager.editorConfig.DefaultSynthesizer;
            string renderer = kind.getVersionString();
            List<VsqID> singers = AppManager.getSingerListFromRendererKind(kind);

            vsq_track.changeRenderer(renderer, singers);
            CadenciiCommand run = VsqFileEx.generateCommandAddTrack(vsq_track,
                                                                     new VsqMixerEntry(0, 0, 0, 0),
                                                                     i,
                                                                     new BezierCurves());
            AppManager.editHistory.register(vsq.executeCommand(run));
            updateDrawObjectList();
            setEdited(true);
            AppManager.setSelected(i);
            AppManager.mMixerWindow.updateStatus();
            refreshScreen();
        }
        #endregion

        /// <summary>
        /// length, positionの各Quantizeモードに応じて、
        /// 関連する全てのメニュー・コンテキストメニューの表示状態を更新します。
        /// </summary>
        public void applyQuantizeMode()
        {
            cMenuPianoQuantize04.Checked = false;
            cMenuPianoQuantize08.Checked = false;
            cMenuPianoQuantize16.Checked = false;
            cMenuPianoQuantize32.Checked = false;
            cMenuPianoQuantize64.Checked = false;
            cMenuPianoQuantize128.Checked = false;
            cMenuPianoQuantizeOff.Checked = false;

#if ENABLE_STRIP_DROPDOWN
            stripDDBtnQuantize04.Checked = false;
            stripDDBtnQuantize08.Checked = false;
            stripDDBtnQuantize16.Checked = false;
            stripDDBtnQuantize32.Checked = false;
            stripDDBtnQuantize64.Checked = false;
            stripDDBtnQuantize128.Checked = false;
            stripDDBtnQuantizeOff.Checked = false;
#endif

            menuSettingPositionQuantize04.Checked = false;
            menuSettingPositionQuantize08.Checked = false;
            menuSettingPositionQuantize16.Checked = false;
            menuSettingPositionQuantize32.Checked = false;
            menuSettingPositionQuantize64.Checked = false;
            menuSettingPositionQuantize128.Checked = false;
            menuSettingPositionQuantizeOff.Checked = false;

            QuantizeMode qm = AppManager.editorConfig.getPositionQuantize();
            bool triplet = AppManager.editorConfig.isPositionQuantizeTriplet();
            stripDDBtnQuantizeParent.Text =
                "QUANTIZE " + QuantizeModeUtil.getString(qm) +
                ((qm != QuantizeMode.off && triplet) ? " [3]" : "");
            if (AppManager.editorConfig.getPositionQuantize() == QuantizeMode.p4) {
                cMenuPianoQuantize04.Checked = true;
#if ENABLE_STRIP_DROPDOWN
                stripDDBtnQuantize04.Checked = true;
#endif
                stripDDBtnQuantizeParent.ImageKey = "note004.png";
                menuSettingPositionQuantize04.Checked = true;
            } else if (AppManager.editorConfig.getPositionQuantize() == QuantizeMode.p8) {
                cMenuPianoQuantize08.Checked = true;
#if ENABLE_STRIP_DROPDOWN
                stripDDBtnQuantize08.Checked = true;
#endif
                stripDDBtnQuantizeParent.ImageKey = "note008.png";
                menuSettingPositionQuantize08.Checked = true;
            } else if (AppManager.editorConfig.getPositionQuantize() == QuantizeMode.p16) {
                cMenuPianoQuantize16.Checked = true;
#if ENABLE_STRIP_DROPDOWN
                stripDDBtnQuantize16.Checked = true;
#endif
                stripDDBtnQuantizeParent.ImageKey = "note016.png";
                menuSettingPositionQuantize16.Checked = true;
            } else if (AppManager.editorConfig.getPositionQuantize() == QuantizeMode.p32) {
                cMenuPianoQuantize32.Checked = true;
#if ENABLE_STRIP_DROPDOWN
                stripDDBtnQuantize32.Checked = true;
#endif
                stripDDBtnQuantizeParent.ImageKey = "note032.png";
                menuSettingPositionQuantize32.Checked = true;
            } else if (AppManager.editorConfig.getPositionQuantize() == QuantizeMode.p64) {
                cMenuPianoQuantize64.Checked = true;
#if ENABLE_STRIP_DROPDOWN
                stripDDBtnQuantize64.Checked = true;
#endif
                stripDDBtnQuantizeParent.ImageKey = "note064.png";
                menuSettingPositionQuantize64.Checked = true;
            } else if (AppManager.editorConfig.getPositionQuantize() == QuantizeMode.p128) {
                cMenuPianoQuantize128.Checked = true;
#if ENABLE_STRIP_DROPDOWN
                stripDDBtnQuantize128.Checked = true;
#endif
                stripDDBtnQuantizeParent.ImageKey = "note128.png";
                menuSettingPositionQuantize128.Checked = true;
            } else if (AppManager.editorConfig.getPositionQuantize() == QuantizeMode.off) {
                cMenuPianoQuantizeOff.Checked = true;
#if ENABLE_STRIP_DROPDOWN
                stripDDBtnQuantizeOff.Checked = true;
#endif
                stripDDBtnQuantizeParent.ImageKey = "notenull.png";
                menuSettingPositionQuantizeOff.Checked = true;
            }
            cMenuPianoQuantizeTriplet.Checked = AppManager.editorConfig.isPositionQuantizeTriplet();
#if ENABLE_STRIP_DROPDOWN
            stripDDBtnQuantizeTriplet.Checked = AppManager.editorConfig.isPositionQuantizeTriplet();
#endif
            menuSettingPositionQuantizeTriplet.Checked = AppManager.editorConfig.isPositionQuantizeTriplet();
        }

        /// <summary>
        /// 現在選択されている編集ツールに応じて、メニューのチェック状態を更新します
        /// </summary>
        public void applySelectedTool()
        {
            EditTool tool = AppManager.getSelectedTool();

            int count = toolBarTool.Buttons.Count;
            for (int i = 0; i < count; i++) {
                Object tsi = toolBarTool.Buttons[i];
                if (tsi is System.Windows.Forms.ToolBarButton) {
                    System.Windows.Forms.ToolBarButton tsb = (System.Windows.Forms.ToolBarButton)tsi;
                    Object tag = tsb.Tag;
                    if (tsb.Style == System.Windows.Forms.ToolBarButtonStyle.ToggleButton && tag != null && tag is string) {
#if ENABLE_SCRIPT
                        if (tool == EditTool.PALETTE_TOOL) {
                            string id = (string)tag;
                            tsb.Pushed = (AppManager.mSelectedPaletteTool == id);
                        } else
#endif // ENABLE_SCRIPT
 {
                            tsb.Pushed = false;
                        }
                    }
                }
            }
            foreach (var tsi in cMenuTrackSelectorPaletteTool.DropDownItems) {
                if (tsi is PaletteToolMenuItem) {
                    PaletteToolMenuItem tsmi = (PaletteToolMenuItem)tsi;
                    string id = tsmi.getPaletteToolID();
                    bool sel = false;
#if ENABLE_SCRIPT
                    if (tool == EditTool.PALETTE_TOOL) {
                        sel = (AppManager.mSelectedPaletteTool == id);
                    }
#endif
                    tsmi.Checked = sel;
                }
            }

            foreach (var tsi in cMenuPianoPaletteTool.DropDownItems) {
                if (tsi is PaletteToolMenuItem) {
                    PaletteToolMenuItem tsmi = (PaletteToolMenuItem)tsi;
                    string id = tsmi.getPaletteToolID();
                    bool sel = false;
#if ENABLE_SCRIPT
                    if (tool == EditTool.PALETTE_TOOL) {
                        sel = (AppManager.mSelectedPaletteTool == id);
                    }
#endif
                    tsmi.Checked = sel;
                }
            }

            EditTool selected_tool = AppManager.getSelectedTool();
            cMenuPianoPointer.Checked = (selected_tool == EditTool.ARROW);
            cMenuPianoPencil.Checked = (selected_tool == EditTool.PENCIL);
            cMenuPianoEraser.Checked = (selected_tool == EditTool.ERASER);

            cMenuTrackSelectorPointer.Checked = (selected_tool == EditTool.ARROW);
            cMenuTrackSelectorPencil.Checked = (selected_tool == EditTool.PENCIL);
            cMenuTrackSelectorLine.Checked = (selected_tool == EditTool.LINE);
            cMenuTrackSelectorEraser.Checked = (selected_tool == EditTool.ERASER);

            stripBtnPointer.Pushed = (selected_tool == EditTool.ARROW);
            stripBtnPencil.Pushed = (selected_tool == EditTool.PENCIL);
            stripBtnLine.Pushed = (selected_tool == EditTool.LINE);
            stripBtnEraser.Pushed = (selected_tool == EditTool.ERASER);

            cMenuPianoCurve.Checked = AppManager.isCurveMode();
            cMenuTrackSelectorCurve.Checked = AppManager.isCurveMode();
            stripBtnCurve.Pushed = AppManager.isCurveMode();
        }

        /// <summary>
        /// 描画すべきオブジェクトのリスト，AppManager.drawObjectsを更新します
        /// </summary>
        public void updateDrawObjectList()
        {
            lock (AppManager.mDrawObjects) {
                if (AppManager.getVsqFile() == null) {
                    return;
                }
                for (int i = 0; i < AppManager.mDrawStartIndex.Length; i++) {
                    AppManager.mDrawStartIndex[i] = 0;
                }
                for (int i = 0; i < AppManager.mDrawObjects.Length; i++) {
                    AppManager.mDrawObjects[i].Clear();
                }

                int xoffset = AppManager.keyOffset;// 6 + AppManager.keyWidth;
                int yoffset = (int)(127 * (int)(100 * controller.getScaleY()));
                float scalex = controller.getScaleX();
                Font SMALL_FONT = null;
                try {
                    SMALL_FONT = new Font(AppManager.editorConfig.ScreenFontName, java.awt.Font.PLAIN, AppManager.FONT_SIZE8);
                    int track_height = (int)(100 * controller.getScaleY());
                    VsqFileEx vsq = AppManager.getVsqFile();
                    int track_count = vsq.Track.Count;
                    Polygon env = new Polygon(new int[7], new int[7], 7);
                    ByRef<int> overlap_x = new ByRef<int>(0);
                    for (int track = 1; track < track_count; track++) {
                        VsqTrack vsq_track = vsq.Track[track];
                        List<DrawObject> tmp = AppManager.mDrawObjects[track - 1];
                        RendererKind kind = VsqFileEx.getTrackRendererKind(vsq_track);
                        AppManager.mDrawIsUtau[track - 1] = kind == RendererKind.UTAU;

                        // 音符イベント
                        foreach (var item in vsq_track.getNoteEventIterator()) {
                            if (item.ID.LyricHandle == null) {
                                continue;
                            }
                            int timesig = item.Clock;
                            int length = item.ID.getLength();
                            int note = item.ID.Note;
                            int x = (int)(timesig * scalex + xoffset);
                            int y = -note * track_height + yoffset;
                            int lyric_width = (int)(length * scalex);
                            string lyric_jp = item.ID.LyricHandle.L0.Phrase;
                            string lyric_en = item.ID.LyricHandle.L0.getPhoneticSymbol();
                            string title = Utility.trimString(lyric_jp + " [" + lyric_en + "]", SMALL_FONT, lyric_width);
                            int accent = item.ID.DEMaccent;
                            int px_vibrato_start = x + lyric_width;
                            int px_vibrato_end = x;
                            int px_vibrato_delay = lyric_width * 2;
                            int vib_delay = length;
                            if (item.ID.VibratoHandle != null) {
                                vib_delay = item.ID.VibratoDelay;
                                double rate = (double)vib_delay / (double)length;
                                px_vibrato_delay = _PX_ACCENT_HEADER + (int)((lyric_width - _PX_ACCENT_HEADER) * rate);
                            }
                            VibratoBPList rate_bp = null;
                            VibratoBPList depth_bp = null;
                            int rate_start = 0;
                            int depth_start = 0;
                            if (item.ID.VibratoHandle != null) {
                                rate_bp = item.ID.VibratoHandle.getRateBP();
                                depth_bp = item.ID.VibratoHandle.getDepthBP();
                                rate_start = item.ID.VibratoHandle.getStartRate();
                                depth_start = item.ID.VibratoHandle.getStartDepth();
                            }

                            // analyzed/のSTFが引き当てられるかどうか
                            // UTAUのWAVが引き当てられるかどうか
                            bool is_valid_for_utau = false;
                            VsqEvent singer_at_clock = vsq_track.getSingerEventAt(timesig);
                            int program = singer_at_clock.ID.IconHandle.Program;
                            if (0 <= program && program < AppManager.editorConfig.UtauSingers.Count) {
                                SingerConfig sc = AppManager.editorConfig.UtauSingers[program];
                                // 通常のUTAU音源
                                if (AppManager.mUtauVoiceDB.ContainsKey(sc.VOICEIDSTR)) {
                                    UtauVoiceDB db = AppManager.mUtauVoiceDB[sc.VOICEIDSTR];
                                    OtoArgs oa = db.attachFileNameFromLyric(lyric_jp, note);
                                    if (oa.fileName == null ||
                                        (oa.fileName != null && oa.fileName == "")) {
                                        is_valid_for_utau = false;
                                    } else {
                                        is_valid_for_utau = System.IO.File.Exists(Path.Combine(sc.VOICEIDSTR, oa.fileName));
                                    }
                                }
                            }
                            int intensity = item.UstEvent == null ? 100 : item.UstEvent.getIntensity();

                            //追加
                            tmp.Add(new DrawObject(DrawObjectType.Note,
                                                     vsq,
                                                     new Rectangle(x, y, lyric_width, track_height),
                                                     title,
                                                     accent,
                                                     item.ID.DEMdecGainRate,
                                                     item.ID.Dynamics,
                                                     item.InternalID,
                                                     px_vibrato_delay,
                                                     false,
                                                     item.ID.LyricHandle.L0.PhoneticSymbolProtected,
                                                     rate_bp,
                                                     depth_bp,
                                                     rate_start,
                                                     depth_start,
                                                     item.ID.Note,
                                                     item.UstEvent.getEnvelope(),
                                                     length,
                                                     timesig,
                                                     is_valid_for_utau,
                                                     is_valid_for_utau, // vConnect-STANDはstfファイルを必要としないので，
                                                     vib_delay,
                                                     intensity));
                        }

                        // Dynaff, Crescendイベント
                        for (Iterator<VsqEvent> itr = vsq_track.getDynamicsEventIterator(); itr.hasNext(); ) {
                            VsqEvent item_itr = itr.next();
                            IconDynamicsHandle handle = item_itr.ID.IconDynamicsHandle;
                            if (handle == null) {
                                continue;
                            }
                            int clock = item_itr.Clock;
                            int length = item_itr.ID.getLength();
                            if (length <= 0) {
                                length = 1;
                            }
                            int raw_width = (int)(length * scalex);
                            DrawObjectType type = DrawObjectType.Note;
                            int width = 0;
                            string str = "";
                            if (handle.isDynaffType()) {
                                // 強弱記号
                                type = DrawObjectType.Dynaff;
                                width = AppManager.DYNAFF_ITEM_WIDTH;
                                int startDyn = handle.getStartDyn();
                                if (startDyn == 120) {
                                    str = "fff";
                                } else if (startDyn == 104) {
                                    str = "ff";
                                } else if (startDyn == 88) {
                                    str = "f";
                                } else if (startDyn == 72) {
                                    str = "mf";
                                } else if (startDyn == 56) {
                                    str = "mp";
                                } else if (startDyn == 40) {
                                    str = "p";
                                } else if (startDyn == 24) {
                                    str = "pp";
                                } else if (startDyn == 8) {
                                    str = "ppp";
                                } else {
                                    str = "?";
                                }
                            } else if (handle.isCrescendType()) {
                                // クレッシェンド
                                type = DrawObjectType.Crescend;
                                width = raw_width;
                                str = handle.IDS;
                            } else if (handle.isDecrescendType()) {
                                // デクレッシェンド
                                type = DrawObjectType.Decrescend;
                                width = raw_width;
                                str = handle.IDS;
                            }
                            if (type == DrawObjectType.Note) {
                                continue;
                            }
                            int note = item_itr.ID.Note;
                            int x = (int)(clock * scalex + xoffset);
                            int y = -note * (int)(100 * controller.getScaleY()) + yoffset;
                            tmp.Add(new DrawObject(type,
                                                     vsq,
                                                     new Rectangle(x, y, width, track_height),
                                                     str,
                                                     0,
                                                     0,
                                                     0,
                                                     item_itr.InternalID,
                                                     0,
                                                     false,
                                                     false,
                                                     null,
                                                     null,
                                                     0,
                                                     0,
                                                     item_itr.ID.Note,
                                                     null,
                                                     length,
                                                     clock,
                                                     true,
                                                     true,
                                                     length,
                                                     0));
                        }

                        // 重複部分があるかどうかを判定
                        int count = tmp.Count;
                        for (int i = 0; i < count - 1; i++) {
                            DrawObject itemi = tmp[i];
                            DrawObjectType parent_type = itemi.mType;
                            /*if ( itemi.type != DrawObjectType.Note ) {
                                continue;
                            }*/
                            bool overwrapped = false;
                            int istart = itemi.mClock;
                            int iend = istart + itemi.mLength;
                            if (itemi.mIsOverlapped) {
                                continue;
                            }
                            for (int j = i + 1; j < count; j++) {
                                DrawObject itemj = tmp[j];
                                if ((itemj.mType == DrawObjectType.Note && parent_type != DrawObjectType.Note) ||
                                     (itemj.mType != DrawObjectType.Note && parent_type == DrawObjectType.Note)) {
                                    continue;
                                }
                                int jstart = itemj.mClock;
                                int jend = jstart + itemj.mLength;
                                if (jstart <= istart) {
                                    if (istart < jend) {
                                        overwrapped = true;
                                        itemj.mIsOverlapped = true;
                                        // breakできない．2個以上の重複を検出する必要があるので．
                                    }
                                }
                                if (istart <= jstart) {
                                    if (jstart < iend) {
                                        overwrapped = true;
                                        itemj.mIsOverlapped = true;
                                    }
                                }
                            }
                            if (overwrapped) {
                                itemi.mIsOverlapped = true;
                            }
                        }
                        tmp.Sort();
                    }
                } catch (Exception ex) {
                    Logger.write(typeof(FormMain) + ".updateDrawObjectList; ex=" + ex + "\n");
                    serr.println("FormMain#updateDrawObjectList; ex=" + ex);
                } finally {
                    if (SMALL_FONT != null) {
                        SMALL_FONT.font.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// editorConfigのRecentFilesを元に，menuFileRecentのドロップダウンアイテムを更新します
        /// </summary>
        public void updateRecentFileMenu()
        {
            int added = 0;
            menuFileRecent.DropDownItems.Clear();
            if (AppManager.editorConfig.RecentFiles != null) {
                for (int i = 0; i < AppManager.editorConfig.RecentFiles.Count; i++) {
                    string item = AppManager.editorConfig.RecentFiles[i];
                    if (item == null) {
                        continue;
                    }
                    if (item != "") {
                        string short_name = PortUtil.getFileName(item);
                        bool available = System.IO.File.Exists(item);
                        RecentFileMenuItem itm = new RecentFileMenuItem(item);
                        itm.Text = short_name;
                        string tooltip = "";
                        if (!available) {
                            tooltip = _("[file not found]") + " ";
                        }
                        tooltip += item;
                        itm.ToolTipText = tooltip;
                        itm.Enabled = available;
                        itm.Click += new EventHandler(handleRecentFileMenuItem_Click);
                        itm.MouseEnter += new EventHandler(handleRecentFileMenuItem_MouseEnter);
                        menuFileRecent.DropDownItems.Add(itm);
                        added++;
                    }
                }
            } else {
                AppManager.editorConfig.pushRecentFiles("");
            }
            menuFileRecent.DropDownItems.Add(new ToolStripSeparator());
            menuFileRecent.DropDownItems.Add(menuFileRecentClear);
            menuFileRecent.Enabled = true;
        }

        /// <summary>
        /// 最後に保存したときから変更されているかどうかを取得または設定します
        /// </summary>
        public bool isEdited()
        {
            return mEdited;
        }

        public void setEdited(bool value)
        {
            mEdited = value;
            string file = AppManager.getFileName();
            if (file == "") {
                file = "Untitled";
            } else {
                file = PortUtil.getFileNameWithoutExtension(file);
            }
            if (mEdited) {
                file += " *";
            }
            string title = file + " - " + _APP_NAME;
            if (this.Text != title) {
                this.Text = title;
            }
            bool redo = AppManager.editHistory.hasRedoHistory();
            bool undo = AppManager.editHistory.hasUndoHistory();
            menuEditRedo.Enabled = redo;
            menuEditUndo.Enabled = undo;
            cMenuPianoRedo.Enabled = redo;
            cMenuPianoUndo.Enabled = undo;
            cMenuTrackSelectorRedo.Enabled = redo;
            cMenuTrackSelectorUndo.Enabled = undo;
            stripBtnUndo.Enabled = undo;
            stripBtnRedo.Enabled = redo;
            //AppManager.setRenderRequired( AppManager.getSelected(), true );
            updateScrollRangeHorizontal();
            updateDrawObjectList();
            panelOverview.updateCachedImage();

#if ENABLE_PROPERTY
            AppManager.propertyPanel.updateValue(AppManager.getSelected());
#endif
        }

        /// <summary>
        /// 入力用のテキストボックスを初期化します
        /// </summary>
        public void showInputTextBox(string phrase, string phonetic_symbol, Point position, bool phonetic_symbol_edit_mode)
        {
#if DEBUG
            AppManager.debugWriteLine("InitializeInputTextBox");
#endif
            hideInputTextBox();

            AppManager.mInputTextBox.KeyUp += new KeyEventHandler(mInputTextBox_KeyUp);
            AppManager.mInputTextBox.KeyDown += new KeyEventHandler(mInputTextBox_KeyDown);
            AppManager.mInputTextBox.ImeModeChanged += mInputTextBox_ImeModeChanged;

            AppManager.mInputTextBox.ImeMode = mLastIsImeModeOn ? System.Windows.Forms.ImeMode.Hiragana : System.Windows.Forms.ImeMode.Off;
            if (phonetic_symbol_edit_mode) {
                AppManager.mInputTextBox.setBufferText(phrase);
                AppManager.mInputTextBox.setPhoneticSymbolEditMode(true);
                AppManager.mInputTextBox.Text = phonetic_symbol;
                AppManager.mInputTextBox.BackColor = mColorTextboxBackcolor.color;
            } else {
                AppManager.mInputTextBox.setBufferText(phonetic_symbol);
                AppManager.mInputTextBox.setPhoneticSymbolEditMode(false);
                AppManager.mInputTextBox.Text = phrase;
                AppManager.mInputTextBox.BackColor = System.Drawing.Color.White;
            }
            AppManager.mInputTextBox.Font = new System.Drawing.Font(AppManager.editorConfig.BaseFontName, AppManager.FONT_SIZE9, System.Drawing.FontStyle.Regular);
            System.Drawing.Point p = new System.Drawing.Point(position.x + 4, position.y + 2);
            AppManager.mInputTextBox.Location = p;

            AppManager.mInputTextBox.Parent = pictPianoRoll;
            AppManager.mInputTextBox.Enabled = true;
            AppManager.mInputTextBox.Visible = true;
            AppManager.mInputTextBox.Focus();
            AppManager.mInputTextBox.SelectAll();
        }

        public void hideInputTextBox()
        {
            AppManager.mInputTextBox.KeyUp -= new KeyEventHandler(mInputTextBox_KeyUp);
            AppManager.mInputTextBox.KeyDown -= new KeyEventHandler(mInputTextBox_KeyDown);
            AppManager.mInputTextBox.ImeModeChanged -= mInputTextBox_ImeModeChanged;
            mLastSymbolEditMode = AppManager.mInputTextBox.isPhoneticSymbolEditMode();
            AppManager.mInputTextBox.Visible = false;
            AppManager.mInputTextBox.Parent = null;
            AppManager.mInputTextBox.Enabled = false;
            focusPianoRoll();
        }

        /// <summary>
        /// 歌詞入力用テキストボックスのモード（歌詞/発音記号）を切り替えます
        /// </summary>
        public void flipInputTextBoxMode()
        {
            string new_value = AppManager.mInputTextBox.Text;
            if (!AppManager.mInputTextBox.isPhoneticSymbolEditMode()) {
                AppManager.mInputTextBox.BackColor = mColorTextboxBackcolor.color;
            } else {
                AppManager.mInputTextBox.BackColor = System.Drawing.Color.White;
            }
            AppManager.mInputTextBox.Text = AppManager.mInputTextBox.getBufferText();
            AppManager.mInputTextBox.setBufferText(new_value);
            AppManager.mInputTextBox.setPhoneticSymbolEditMode(!AppManager.mInputTextBox.isPhoneticSymbolEditMode());
        }

        /// <summary>
        /// アンドゥ処理を行います
        /// </summary>
        public void undo()
        {
            if (AppManager.editHistory.hasUndoHistory()) {
                AppManager.undo();
                menuEditRedo.Enabled = AppManager.editHistory.hasRedoHistory();
                menuEditUndo.Enabled = AppManager.editHistory.hasUndoHistory();
                cMenuPianoRedo.Enabled = AppManager.editHistory.hasRedoHistory();
                cMenuPianoUndo.Enabled = AppManager.editHistory.hasUndoHistory();
                cMenuTrackSelectorRedo.Enabled = AppManager.editHistory.hasRedoHistory();
                cMenuTrackSelectorUndo.Enabled = AppManager.editHistory.hasUndoHistory();
                AppManager.mMixerWindow.updateStatus();
                setEdited(true);
                updateDrawObjectList();

#if ENABLE_PROPERTY
                if (AppManager.propertyPanel != null) {
                    AppManager.propertyPanel.updateValue(AppManager.getSelected());
                }
#endif
            }
        }

        /// <summary>
        /// リドゥ処理を行います
        /// </summary>
        public void redo()
        {
            if (AppManager.editHistory.hasRedoHistory()) {
                AppManager.redo();
                menuEditRedo.Enabled = AppManager.editHistory.hasRedoHistory();
                menuEditUndo.Enabled = AppManager.editHistory.hasUndoHistory();
                cMenuPianoRedo.Enabled = AppManager.editHistory.hasRedoHistory();
                cMenuPianoUndo.Enabled = AppManager.editHistory.hasUndoHistory();
                cMenuTrackSelectorRedo.Enabled = AppManager.editHistory.hasRedoHistory();
                cMenuTrackSelectorUndo.Enabled = AppManager.editHistory.hasUndoHistory();
                AppManager.mMixerWindow.updateStatus();
                setEdited(true);
                updateDrawObjectList();

#if ENABLE_PROPERTY
                if (AppManager.propertyPanel != null) {
                    AppManager.propertyPanel.updateValue(AppManager.getSelected());
                }
#endif
            }
        }

        /// <summary>
        /// xvsqファイルを開きます
        /// </summary>
        /// <returns>ファイルを開くのに成功した場合trueを，それ以外はfalseを返します</returns>
        public bool openVsqCor(string file)
        {
            if (AppManager.readVsq(file)) {
                return true;
            }
            if (AppManager.getVsqFile().Track.Count >= 2) {
                updateScrollRangeHorizontal();
            }
            AppManager.editorConfig.pushRecentFiles(file);
            updateRecentFileMenu();
            setEdited(false);
            AppManager.editHistory.clear();
            AppManager.mMixerWindow.updateStatus();

            // キャッシュwaveなどの処理
            if (AppManager.editorConfig.UseProjectCache) {
                #region キャッシュディレクトリの処理
                VsqFileEx vsq = AppManager.getVsqFile();
                string cacheDir = vsq.cacheDir; // xvsqに保存されていたキャッシュのディレクトリ
                string dir = PortUtil.getDirectoryName(file);
                string name = PortUtil.getFileNameWithoutExtension(file);
                string estimatedCacheDir = Path.Combine(dir, name + ".cadencii"); // ファイル名から推測されるキャッシュディレクトリ
                if (cacheDir == null) {
                    cacheDir = "";
                }
                if (cacheDir != "" &&
                     Directory.Exists(cacheDir) &&
                     estimatedCacheDir != "" &&
                     cacheDir != estimatedCacheDir) {
                    // ファイル名から推測されるキャッシュディレクトリ名と
                    // xvsqに指定されているキャッシュディレクトリと異なる場合
                    // cacheDirの必要な部分をestimatedCacheDirに移す

                    // estimatedCacheDirが存在しない場合、新しく作る
#if DEBUG
                    sout.println("FormMain#openVsqCor;fsys.isDirectoryExists( estimatedCacheDir )=" + Directory.Exists(estimatedCacheDir));
#endif
                    if (!Directory.Exists(estimatedCacheDir)) {
                        try {
                            PortUtil.createDirectory(estimatedCacheDir);
                        } catch (Exception ex) {
                            Logger.write(typeof(FormMain) + ".openVsqCor; ex=" + ex + "\n");
                            serr.println("FormMain#openVsqCor; ex=" + ex);
                            AppManager.showMessageBox(PortUtil.formatMessage(_("cannot create cache directory: '{0}'"), estimatedCacheDir),
                                                       _("Info."),
                                                       PortUtil.OK_OPTION,
                                                       cadencii.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE);
                            return true;
                        }
                    }

                    // ファイルを移す
                    for (int i = 1; i < vsq.Track.Count; i++) {
                        string wavFrom = Path.Combine(cacheDir, i + ".wav");
                        string xmlFrom = Path.Combine(cacheDir, i + ".xml");

                        string wavTo = Path.Combine(estimatedCacheDir, i + ".wav");
                        string xmlTo = Path.Combine(estimatedCacheDir, i + ".xml");
                        if (System.IO.File.Exists(wavFrom)) {
                            try {
                                PortUtil.moveFile(wavFrom, wavTo);
                            } catch (Exception ex) {
                                Logger.write(typeof(FormMain) + ".openVsqCor; ex=" + ex + "\n");
                                serr.println("FormMain#openVsqCor; ex=" + ex);
                            }
                        }
                        if (System.IO.File.Exists(xmlFrom)) {
                            try {
                                PortUtil.moveFile(xmlFrom, xmlTo);
                            } catch (Exception ex) {
                                Logger.write(typeof(FormMain) + ".openVsqCor; ex=" + ex + "\n");
                                serr.println("FormMain#openVsqCor; ex=" + ex);
                            }
                        }
                    }
                }
                cacheDir = estimatedCacheDir;

                // キャッシュが無かったら作成
                if (!Directory.Exists(cacheDir)) {
                    try {
                        PortUtil.createDirectory(cacheDir);
                    } catch (Exception ex) {
                        Logger.write(typeof(FormMain) + ".openVsqCor; ex=" + ex + "\n");
                        serr.println("FormMain#openVsqCor; ex=" + ex);
                        AppManager.showMessageBox(PortUtil.formatMessage(_("cannot create cache directory: '{0}'"), estimatedCacheDir),
                                                   _("Info."),
                                                   PortUtil.OK_OPTION,
                                                   cadencii.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE);
                        return true;
                    }
                }

                // RenderedStatusを読み込む
                for (int i = 1; i < vsq.Track.Count; i++) {
                    AppManager.deserializeRenderingStatus(cacheDir, i);
                }

                // キャッシュ内のwavを、waveViewに読み込む
                waveView.unloadAll();
                for (int i = 1; i < vsq.Track.Count; i++) {
                    string wav = Path.Combine(cacheDir, i + ".wav");
#if DEBUG
                    sout.println("FormMain#openVsqCor; wav=" + wav + "; isExists=" + System.IO.File.Exists(wav));
#endif
                    if (!System.IO.File.Exists(wav)) {
                        continue;
                    }
                    waveView.load(i - 1, wav);
                }

                // 一時ディレクトリを、cachedirに変更
                AppManager.setTempWaveDir(cacheDir);
                #endregion
            }
            return false;
        }

        public void updateMenuFonts()
        {
            if (AppManager.editorConfig.BaseFontName == "") {
                return;
            }
            Font font = AppManager.editorConfig.getBaseFont();
            Util.applyFontRecurse(this, font);
#if !JAVA_MAC
            Util.applyContextMenuFontRecurse(cMenuPiano, font);
            Util.applyContextMenuFontRecurse(cMenuTrackSelector, font);
            if (AppManager.mMixerWindow != null) {
                Util.applyFontRecurse(AppManager.mMixerWindow, font);
            }
            Util.applyContextMenuFontRecurse(cMenuTrackTab, font);
            trackSelector.applyFont(font);
            Util.applyToolStripFontRecurse(menuFile, font);
            Util.applyToolStripFontRecurse(menuEdit, font);
            Util.applyToolStripFontRecurse(menuVisual, font);
            Util.applyToolStripFontRecurse(menuJob, font);
            Util.applyToolStripFontRecurse(menuTrack, font);
            Util.applyToolStripFontRecurse(menuLyric, font);
            Util.applyToolStripFontRecurse(menuScript, font);
            Util.applyToolStripFontRecurse(menuSetting, font);
            Util.applyToolStripFontRecurse(menuHelp, font);
#endif
            Util.applyFontRecurse(toolBarFile, font);
            Util.applyFontRecurse(toolBarMeasure, font);
            Util.applyFontRecurse(toolBarPosition, font);
            Util.applyFontRecurse(toolBarTool, font);
            if (mDialogPreference != null) {
                Util.applyFontRecurse(mDialogPreference, font);
            }

            AppManager.baseFont10Bold = new Font(AppManager.editorConfig.BaseFontName, java.awt.Font.BOLD, AppManager.FONT_SIZE10);
            AppManager.baseFont8 = new Font(AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, AppManager.FONT_SIZE8);
            AppManager.baseFont10 = new Font(AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, AppManager.FONT_SIZE10);
            AppManager.baseFont9 = new Font(AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, AppManager.FONT_SIZE9);
            AppManager.baseFont50Bold = new Font(AppManager.editorConfig.BaseFontName, java.awt.Font.BOLD, AppManager.FONT_SIZE50);
            AppManager.baseFont10OffsetHeight = Util.getStringDrawOffset(AppManager.baseFont10);
            AppManager.baseFont8OffsetHeight = Util.getStringDrawOffset(AppManager.baseFont8);
            AppManager.baseFont9OffsetHeight = Util.getStringDrawOffset(AppManager.baseFont9);
            AppManager.baseFont50OffsetHeight = Util.getStringDrawOffset(AppManager.baseFont50Bold);
            AppManager.baseFont8Height = Util.measureString(Util.PANGRAM, AppManager.baseFont8).height;
            AppManager.baseFont9Height = Util.measureString(Util.PANGRAM, AppManager.baseFont9).height;
            AppManager.baseFont10Height = Util.measureString(Util.PANGRAM, AppManager.baseFont10).height;
            AppManager.baseFont50Height = Util.measureString(Util.PANGRAM, AppManager.baseFont50Bold).height;
        }

        public void picturePositionIndicatorDrawTo(java.awt.Graphics g1)
        {
            Graphics2D g = (Graphics2D)g1;
            Font SMALL_FONT = AppManager.baseFont8;
            int small_font_offset = AppManager.baseFont8OffsetHeight;
            try {
                int key_width = AppManager.keyWidth;
                int width = picturePositionIndicator.Width;
                int height = picturePositionIndicator.Height;
                VsqFileEx vsq = AppManager.getVsqFile();

                #region 小節ごとの線
                int dashed_line_step = AppManager.getPositionQuantizeClock();
                for (Iterator<VsqBarLineType> itr = vsq.getBarLineIterator(AppManager.clockFromXCoord(width)); itr.hasNext(); ) {
                    VsqBarLineType blt = itr.next();
                    int local_clock_step = 480 * 4 / blt.getLocalDenominator();
                    int x = AppManager.xCoordFromClocks(blt.clock());
                    if (blt.isSeparator()) {
                        int current = blt.getBarCount() - vsq.getPreMeasure() + 1;
                        g.setColor(mColorR105G105B105);
                        g.drawLine(x, 0, x, 49);
                        // 小節の数字
                        //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.setColor(Color.black);
                        g.setFont(SMALL_FONT);
                        g.drawString(current + "", x + 4, 8 - small_font_offset + 1);
                        //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    } else {
                        g.setColor(mColorR105G105B105);
                        g.drawLine(x, 11, x, 16);
                        g.drawLine(x, 26, x, 31);
                        g.drawLine(x, 41, x, 46);
                    }
                    if (dashed_line_step > 1 && AppManager.isGridVisible()) {
                        int numDashedLine = local_clock_step / dashed_line_step;
                        for (int i = 1; i < numDashedLine; i++) {
                            int x2 = AppManager.xCoordFromClocks(blt.clock() + i * dashed_line_step);
                            g.setColor(mColorR065G065B065);
                            g.drawLine(x2, 9 + 5, x2, 14 + 3);
                            g.drawLine(x2, 24 + 5, x2, 29 + 3);
                            g.drawLine(x2, 39 + 5, x2, 44 + 3);
                        }
                    }
                }
                #endregion

                if (vsq != null) {
                    #region 拍子の変更
                    int c = vsq.TimesigTable.Count;
                    for (int i = 0; i < c; i++) {
                        TimeSigTableEntry itemi = vsq.TimesigTable[i];
                        int clock = itemi.Clock;
                        int barcount = itemi.BarCount;
                        int x = AppManager.xCoordFromClocks(clock);
                        if (width < x) {
                            break;
                        }
                        string s = itemi.Numerator + "/" + itemi.Denominator;
                        g.setFont(SMALL_FONT);
                        if (AppManager.itemSelection.isTimesigContains(barcount)) {
                            g.setColor(AppManager.getHilightColor());
                            g.drawString(s, x + 4, 40 - small_font_offset + 1);
                        } else {
                            g.setColor(Color.black);
                            g.drawString(s, x + 4, 40 - small_font_offset + 1);
                        }

                        if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TIMESIG) {
                            if (AppManager.itemSelection.isTimesigContains(barcount)) {
                                int edit_clock_x = AppManager.xCoordFromClocks(vsq.getClockFromBarCount(AppManager.itemSelection.getTimesig(barcount).editing.BarCount));
                                g.setColor(mColorR187G187B255);
                                g.drawLine(edit_clock_x - 1, 32,
                                            edit_clock_x - 1, picturePositionIndicator.Height - 1);
                                g.setColor(mColorR007G007B151);
                                g.drawLine(edit_clock_x, 32,
                                            edit_clock_x, picturePositionIndicator.Height - 1);
                            }
                        }
                    }
                    #endregion

                    #region テンポの変更
                    g.setFont(SMALL_FONT);
                    c = vsq.TempoTable.Count;
                    for (int i = 0; i < c; i++) {
                        TempoTableEntry itemi = vsq.TempoTable[i];
                        int clock = itemi.Clock;
                        int x = AppManager.xCoordFromClocks(clock);
                        if (width < x) {
                            break;
                        }
                        string s = PortUtil.formatDecimal("#.00", 60e6 / (float)itemi.Tempo);
                        if (AppManager.itemSelection.isTempoContains(clock)) {
                            g.setColor(AppManager.getHilightColor());
                            g.drawString(s, x + 4, 24 - small_font_offset + 1);
                        } else {
                            g.setColor(Color.black);
                            g.drawString(s, x + 4, 24 - small_font_offset + 1);
                        }

                        if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TEMPO) {
                            if (AppManager.itemSelection.isTempoContains(clock)) {
                                int edit_clock_x = AppManager.xCoordFromClocks(AppManager.itemSelection.getTempo(clock).editing.Clock);
                                g.setColor(mColorR187G187B255);
                                g.drawLine(edit_clock_x - 1, 18,
                                            edit_clock_x - 1, 32);
                                g.setColor(mColorR007G007B151);
                                g.drawLine(edit_clock_x, 18,
                                            edit_clock_x, 32);
                            }
                        }
                    }
                    #endregion
                }

                #region 現在のマーカー
                // ソングポジション
                float xoffset = key_width + AppManager.keyOffset - controller.getStartToDrawX();
                int marker_x = (int)(AppManager.getCurrentClock() * controller.getScaleX() + xoffset);
                if (key_width <= marker_x && marker_x <= width) {
                    g.setStroke(new BasicStroke(2.0f));
                    g.setColor(Color.white);
                    g.drawLine(marker_x, 0, marker_x, height);
                    g.setStroke(new BasicStroke());
                }

                // スタートマーカーとエンドマーカー
                bool right = false;
                bool left = false;
                if (vsq.config.StartMarkerEnabled) {
                    int x = AppManager.xCoordFromClocks(vsq.config.StartMarker);
                    if (x < key_width) {
                        left = true;
                    } else if (width < x) {
                        right = true;
                    } else {
                        g.drawImage(
                            Properties.Resources.start_marker, x, 3, this);
                    }
                }
                if (vsq.config.EndMarkerEnabled) {
                    int x = AppManager.xCoordFromClocks(vsq.config.EndMarker) - 6;
                    if (x < key_width) {
                        left = true;
                    } else if (width < x) {
                        right = true;
                    } else {
                        g.drawImage(
                            Properties.Resources.end_marker, x, 3, this);
                    }
                }

                // 範囲外にスタートマーカーとエンドマーカーがある場合のマーク
                if (right) {
                    g.setColor(Color.white);
                    g.fillPolygon(
                        new int[] { width - 6, width, width - 6 },
                        new int[] { 3, 10, 16 },
                        3);
                }
                if (left) {
                    g.setColor(Color.white);
                    g.fillPolygon(
                        new int[] { key_width + 7, key_width + 1, key_width + 7 },
                        new int[] { 3, 10, 16 },
                        3);
                }
                #endregion

                #region TEMPO & BEAT
                // TEMPO BEATの文字の部分。小節数が被っている可能性があるので、塗り潰す
                g.setColor(new Color(picturePositionIndicator.BackColor));
                g.fillRect(0, 0, AppManager.keyWidth, 48);
                // 横ライン上
                g.setColor(new Color(104, 104, 104));
                g.drawLine(0, 17, width, 17);
                // 横ライン中央
                g.drawLine(0, 32, width, 32);
                // 横ライン下
                g.drawLine(0, 47, width, 47);
                // 縦ライン
                g.drawLine(AppManager.keyWidth, 0, AppManager.keyWidth, 48);
                /* TEMPO&BEATとピアノロールの境界 */
                g.drawLine(AppManager.keyWidth, 48, width - 18, 48);
                g.setFont(SMALL_FONT);
                g.setColor(Color.black);
                g.drawString("TEMPO", 11, 24 - small_font_offset + 1);
                g.drawString("BEAT", 11, 40 - small_font_offset + 1);
                #endregion
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".picturePositionIndicatorDrawTo; ex=" + ex + "\n");
                serr.println("FormMain#picturePositionIndicatorDrawTo; ex=" + ex);
            }
        }

        /// <summary>
        /// イベントハンドラを登録します。
        /// </summary>
        public void registerEventHandlers()
        {
            this.Load += new EventHandler(FormMain_Load);
            menuFileNew.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileNew.Click += new EventHandler(handleFileNew_Click);
            menuFileOpen.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileOpen.Click += new EventHandler(handleFileOpen_Click);
            menuFileSave.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileSave.Click += new EventHandler(handleFileSave_Click);
            menuFileSaveNamed.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileSaveNamed.Click += new EventHandler(menuFileSaveNamed_Click);
            menuFileOpenVsq.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileOpenVsq.Click += new EventHandler(menuFileOpenVsq_Click);
            menuFileOpenUst.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileOpenUst.Click += new EventHandler(menuFileOpenUst_Click);
            menuFileImport.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileImportMidi.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileImportMidi.Click += new EventHandler(menuFileImportMidi_Click);
            menuFileImportUst.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileImportUst.Click += new EventHandler(menuFileImportUst_Click);
            menuFileImportVsq.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileImportVsq.Click += new EventHandler(menuFileImportVsq_Click);
            menuFileExport.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileExport.DropDownOpening += new EventHandler(menuFileExport_DropDownOpening);
            menuFileExportWave.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileExportWave.Click += new EventHandler(menuFileExportWave_Click);
            menuFileExportParaWave.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileExportParaWave.Click += new EventHandler(menuFileExportParaWave_Click);
            menuFileExportMidi.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileExportMidi.Click += new EventHandler(menuFileExportMidi_Click);
            menuFileExportMusicXml.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileExportMusicXml.Click += new EventHandler(menuFileExportMusicXml_Click);
            menuFileExportUst.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileExportUst.Click += new EventHandler(menuFileExportUst_Click);
            menuFileExportVsq.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileExportVsq.Click += new EventHandler(menuFileExportVsq_Click);
            menuFileExportVsqx.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileExportVsqx.Click += new EventHandler(menuFileExportVsqx_Click);
            menuFileExportVxt.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileExportVxt.Click += new EventHandler(menuFileExportVxt_Click);
            menuFileRecent.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileRecentClear.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileRecentClear.Click += new EventHandler(menuFileRecentClear_Click);
            menuFileQuit.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuFileQuit.Click += new EventHandler(menuFileQuit_Click);
            menuEdit.DropDownOpening += new EventHandler(menuEdit_DropDownOpening);
            menuEditUndo.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuEditUndo.Click += new EventHandler(handleEditUndo_Click);
            menuEditRedo.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuEditRedo.Click += new EventHandler(handleEditRedo_Click);
            menuEditCut.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuEditCut.Click += new EventHandler(handleEditCut_Click);
            menuEditCopy.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuEditCopy.Click += new EventHandler(handleEditCopy_Click);
            menuEditPaste.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuEditPaste.Click += new EventHandler(handleEditPaste_Click);
            menuEditDelete.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuEditDelete.Click += new EventHandler(menuEditDelete_Click);
            menuEditAutoNormalizeMode.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuEditAutoNormalizeMode.Click += new EventHandler(menuEditAutoNormalizeMode_Click);
            menuEditSelectAll.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuEditSelectAll.Click += new EventHandler(menuEditSelectAll_Click);
            menuEditSelectAllEvents.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuEditSelectAllEvents.Click += new EventHandler(menuEditSelectAllEvents_Click);
            menuVisualOverview.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuVisualControlTrack.CheckedChanged += new EventHandler(menuVisualControlTrack_CheckedChanged);
            menuVisualControlTrack.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuVisualMixer.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuVisualMixer.Click += new EventHandler(menuVisualMixer_Click);
            menuVisualWaveform.CheckedChanged += new EventHandler(menuVisualWaveform_CheckedChanged);
            menuVisualWaveform.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuVisualProperty.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuVisualProperty.CheckedChanged += new EventHandler(menuVisualProperty_CheckedChanged);
            menuVisualGridline.CheckedChanged += new EventHandler(menuVisualGridline_CheckedChanged);
            menuVisualGridline.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuVisualIconPalette.Click += new EventHandler(menuVisualIconPalette_Click);
            menuVisualIconPalette.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuVisualStartMarker.Click += new EventHandler(handleStartMarker_Click);
            menuVisualStartMarker.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuVisualEndMarker.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuVisualEndMarker.Click += new EventHandler(handleEndMarker_Click);
            menuVisualLyrics.CheckedChanged += new EventHandler(menuVisualLyrics_CheckedChanged);
            menuVisualLyrics.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuVisualNoteProperty.CheckedChanged += new EventHandler(menuVisualNoteProperty_CheckedChanged);
            menuVisualNoteProperty.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuVisualPitchLine.CheckedChanged += new EventHandler(menuVisualPitchLine_CheckedChanged);
            menuVisualPitchLine.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuVisualPluginUi.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuVisualPluginUi.DropDownOpening += new EventHandler(menuVisualPluginUi_DropDownOpening);
            menuVisualPluginUiVocaloid1.Click += new EventHandler(menuVisualPluginUiVocaloidCommon_Click);
            menuVisualPluginUiVocaloid2.Click += new EventHandler(menuVisualPluginUiVocaloidCommon_Click);
            menuVisualPluginUiAquesTone.Click += new EventHandler(menuVisualPluginUiAquesTone_Click);
            menuJob.DropDownOpening += new EventHandler(menuJob_DropDownOpening);
            menuJobNormalize.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuJobNormalize.Click += new EventHandler(menuJobNormalize_Click);
            menuJobInsertBar.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuJobInsertBar.Click += new EventHandler(menuJobInsertBar_Click);
            menuJobDeleteBar.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuJobDeleteBar.Click += new EventHandler(menuJobDeleteBar_Click);
            menuJobRandomize.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuJobRandomize.Click += new EventHandler(menuJobRandomize_Click);
            menuJobConnect.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuJobConnect.Click += new EventHandler(menuJobConnect_Click);
            menuJobLyric.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuJobLyric.Click += new EventHandler(menuJobLyric_Click);
            menuTrack.DropDownOpening += new EventHandler(menuTrack_DropDownOpening);
            menuTrackOn.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuTrackBgm.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuTrackOn.Click += new EventHandler(handleTrackOn_Click);
            menuTrackAdd.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuTrackAdd.Click += new EventHandler(menuTrackAdd_Click);
            menuTrackCopy.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuTrackCopy.Click += new EventHandler(menuTrackCopy_Click);
            menuTrackChangeName.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuTrackChangeName.Click += new EventHandler(menuTrackChangeName_Click);
            menuTrackDelete.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuTrackDelete.Click += new EventHandler(menuTrackDelete_Click);
            menuTrackRenderCurrent.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuTrackRenderCurrent.Click += new EventHandler(menuTrackRenderCurrent_Click);
            menuTrackRenderAll.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuTrackRenderAll.Click += new EventHandler(handleTrackRenderAll_Click);
            menuTrackOverlay.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuTrackOverlay.Click += new EventHandler(menuTrackOverlay_Click);
            menuTrackRenderer.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuTrackRenderer.DropDownOpening += new EventHandler(menuTrackRenderer_DropDownOpening);
            menuTrackRendererVOCALOID1.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuTrackRendererVOCALOID1.Click += new EventHandler(handleChangeRenderer);
            menuTrackRendererVOCALOID2.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuTrackRendererVOCALOID2.Click += new EventHandler(handleChangeRenderer);
            menuTrackRendererUtau.MouseEnter += new EventHandler(handleMenuMouseEnter);
            //UTAUはresamplerを識別するのでmenuTrackRendererUtauのサブアイテムのClickイベントを拾う
            //menuTrackRendererUtau.Click += new EventHandler( handleChangeRenderer );
            menuTrackRendererVCNT.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuTrackRendererVCNT.Click += new EventHandler(handleChangeRenderer);
            menuTrackRendererAquesTone.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuTrackRendererAquesTone.Click += new EventHandler(handleChangeRenderer);
            menuLyric.DropDownOpening += new EventHandler(menuLyric_DropDownOpening);
            menuLyricCopyVibratoToPreset.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuLyricExpressionProperty.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuLyricExpressionProperty.Click += new EventHandler(menuLyricExpressionProperty_Click);
            menuLyricVibratoProperty.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuLyricVibratoProperty.Click += new EventHandler(menuLyricVibratoProperty_Click);
            menuLyricPhonemeTransformation.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuLyricDictionary.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuLyricDictionary.Click += new EventHandler(menuLyricDictionary_Click);
            menuLyricPhonemeTransformation.Click += new EventHandler(menuLyricPhonemeTransformation_Click);
            menuLyricApplyUtauParameters.Click += new EventHandler(menuLyricApplyUtauParameters_Click);
            menuScriptUpdate.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuScriptUpdate.Click += new EventHandler(menuScriptUpdate_Click);
            menuSettingPreference.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuSettingPreference.Click += new EventHandler(menuSettingPreference_Click);
            menuSettingGameControler.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuSettingGameControlerSetting.Click += new EventHandler(menuSettingGameControlerSetting_Click);
            menuSettingGameControlerLoad.Click += new EventHandler(menuSettingGameControlerLoad_Click);
            menuSettingGameControlerRemove.Click += new EventHandler(menuSettingGameControlerRemove_Click);
            menuSettingSequence.Click += new EventHandler(menuSettingSequence_Click);
            menuSettingSequence.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuSettingShortcut.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuSettingShortcut.Click += new EventHandler(menuSettingShortcut_Click);
            menuSettingVibratoPreset.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuSettingVibratoPreset.Click += new EventHandler(menuSettingVibratoPreset_Click);
            menuSettingDefaultSingerStyle.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuSettingDefaultSingerStyle.Click += new EventHandler(menuSettingDefaultSingerStyle_Click);
            menuSettingPaletteTool.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuSettingPositionQuantize.MouseEnter += new EventHandler(handleMenuMouseEnter);
            menuSettingPositionQuantize04.Click += new EventHandler(handlePositionQuantize);
            menuSettingPositionQuantize08.Click += new EventHandler(handlePositionQuantize);
            menuSettingPositionQuantize16.Click += new EventHandler(handlePositionQuantize);
            menuSettingPositionQuantize32.Click += new EventHandler(handlePositionQuantize);
            menuSettingPositionQuantize64.Click += new EventHandler(handlePositionQuantize);
            menuSettingPositionQuantize128.Click += new EventHandler(handlePositionQuantize);
            menuSettingPositionQuantizeOff.Click += new EventHandler(handlePositionQuantize);
            menuSettingPositionQuantizeTriplet.Click += new EventHandler(handlePositionQuantizeTriplet_Click);
            menuHelpAbout.Click += new EventHandler(menuHelpAbout_Click);
            menuHelpManual.Click += new EventHandler(menuHelpManual_Click);
            menuHelpLogSwitch.CheckedChanged += new EventHandler(menuHelpLogSwitch_CheckedChanged);
            menuHelpLogOpen.Click += new EventHandler(menuHelpLogOpen_Click);
            menuHelpDebug.Click += new EventHandler(menuHelpDebug_Click);
            menuHiddenEditLyric.Click += new EventHandler(menuHiddenEditLyric_Click);
            menuHiddenEditFlipToolPointerPencil.Click += new EventHandler(menuHiddenEditFlipToolPointerPencil_Click);
            menuHiddenEditFlipToolPointerEraser.Click += new EventHandler(menuHiddenEditFlipToolPointerEraser_Click);
            menuHiddenVisualForwardParameter.Click += new EventHandler(menuHiddenVisualForwardParameter_Click);
            menuHiddenVisualBackwardParameter.Click += new EventHandler(menuHiddenVisualBackwardParameter_Click);
            menuHiddenTrackNext.Click += new EventHandler(menuHiddenTrackNext_Click);
            menuHiddenTrackBack.Click += new EventHandler(menuHiddenTrackBack_Click);
            menuHiddenCopy.Click += new EventHandler(handleEditCopy_Click);
            menuHiddenPaste.Click += new EventHandler(handleEditPaste_Click);
            menuHiddenCut.Click += new EventHandler(handleEditCut_Click);
            menuHiddenSelectBackward.Click += new EventHandler(menuHiddenSelectBackward_Click);
            menuHiddenSelectForward.Click += new EventHandler(menuHiddenSelectForward_Click);
            menuHiddenMoveUp.Click += new EventHandler(menuHiddenMoveUp_Click);
            menuHiddenMoveDown.Click += new EventHandler(menuHiddenMoveDown_Click);
            menuHiddenMoveLeft.Click += new EventHandler(menuHiddenMoveLeft_Click);
            menuHiddenMoveRight.Click += new EventHandler(menuHiddenMoveRight_Click);
            menuHiddenLengthen.Click += new EventHandler(menuHiddenLengthen_Click);
            menuHiddenShorten.Click += new EventHandler(menuHiddenShorten_Click);
            menuHiddenGoToEndMarker.Click += new EventHandler(menuHiddenGoToEndMarker_Click);
            menuHiddenGoToStartMarker.Click += new EventHandler(menuHiddenGoToStartMarker_Click);
            menuHiddenPlayFromStartMarker.Click += new EventHandler(menuHiddenPlayFromStartMarker_Click);
            menuHiddenPrintPoToCSV.Click += new EventHandler(menuHiddenPrintPoToCSV_Click);
            menuHiddenFlipCurveOnPianorollMode.Click += new EventHandler(menuHiddenFlipCurveOnPianorollMode_Click);

            cMenuPiano.Opening += new CancelEventHandler(cMenuPiano_Opening);
            cMenuPianoPointer.Click += new EventHandler(cMenuPianoPointer_Click);
            cMenuPianoPencil.Click += new EventHandler(cMenuPianoPencil_Click);
            cMenuPianoEraser.Click += new EventHandler(cMenuPianoEraser_Click);
            cMenuPianoCurve.Click += new EventHandler(cMenuPianoCurve_Click);
            cMenuPianoFixed01.Click += new EventHandler(cMenuPianoFixed01_Click);
            cMenuPianoFixed02.Click += new EventHandler(cMenuPianoFixed02_Click);
            cMenuPianoFixed04.Click += new EventHandler(cMenuPianoFixed04_Click);
            cMenuPianoFixed08.Click += new EventHandler(cMenuPianoFixed08_Click);
            cMenuPianoFixed16.Click += new EventHandler(cMenuPianoFixed16_Click);
            cMenuPianoFixed32.Click += new EventHandler(cMenuPianoFixed32_Click);
            cMenuPianoFixed64.Click += new EventHandler(cMenuPianoFixed64_Click);
            cMenuPianoFixed128.Click += new EventHandler(cMenuPianoFixed128_Click);
            cMenuPianoFixedOff.Click += new EventHandler(cMenuPianoFixedOff_Click);
            cMenuPianoFixedTriplet.Click += new EventHandler(cMenuPianoFixedTriplet_Click);
            cMenuPianoFixedDotted.Click += new EventHandler(cMenuPianoFixedDotted_Click);
            cMenuPianoQuantize04.Click += new EventHandler(handlePositionQuantize);
            cMenuPianoQuantize08.Click += new EventHandler(handlePositionQuantize);
            cMenuPianoQuantize16.Click += new EventHandler(handlePositionQuantize);
            cMenuPianoQuantize32.Click += new EventHandler(handlePositionQuantize);
            cMenuPianoQuantize64.Click += new EventHandler(handlePositionQuantize);
            cMenuPianoQuantize128.Click += new EventHandler(handlePositionQuantize);
            cMenuPianoQuantizeOff.Click += new EventHandler(handlePositionQuantize);
            cMenuPianoQuantizeTriplet.Click += new EventHandler(handlePositionQuantizeTriplet_Click);
            cMenuPianoGrid.Click += new EventHandler(cMenuPianoGrid_Click);
            cMenuPianoUndo.Click += new EventHandler(cMenuPianoUndo_Click);
            cMenuPianoRedo.Click += new EventHandler(cMenuPianoRedo_Click);
            cMenuPianoCut.Click += new EventHandler(cMenuPianoCut_Click);
            cMenuPianoCopy.Click += new EventHandler(cMenuPianoCopy_Click);
            cMenuPianoPaste.Click += new EventHandler(cMenuPianoPaste_Click);
            cMenuPianoDelete.Click += new EventHandler(cMenuPianoDelete_Click);
            cMenuPianoSelectAll.Click += new EventHandler(cMenuPianoSelectAll_Click);
            cMenuPianoSelectAllEvents.Click += new EventHandler(cMenuPianoSelectAllEvents_Click);
            cMenuPianoImportLyric.Click += new EventHandler(cMenuPianoImportLyric_Click);
            cMenuPianoExpressionProperty.Click += new EventHandler(cMenuPianoProperty_Click);
            cMenuPianoVibratoProperty.Click += new EventHandler(cMenuPianoVibratoProperty_Click);
            cMenuTrackTab.Opening += new CancelEventHandler(cMenuTrackTab_Opening);
            cMenuTrackTabTrackOn.Click += new EventHandler(handleTrackOn_Click);
            cMenuTrackTabAdd.Click += new EventHandler(cMenuTrackTabAdd_Click);
            cMenuTrackTabCopy.Click += new EventHandler(cMenuTrackTabCopy_Click);
            cMenuTrackTabChangeName.Click += new EventHandler(cMenuTrackTabChangeName_Click);
            cMenuTrackTabDelete.Click += new EventHandler(cMenuTrackTabDelete_Click);
            cMenuTrackTabRenderCurrent.Click += new EventHandler(cMenuTrackTabRenderCurrent_Click);
            cMenuTrackTabRenderAll.Click += new EventHandler(handleTrackRenderAll_Click);
            cMenuTrackTabOverlay.Click += new EventHandler(cMenuTrackTabOverlay_Click);
            cMenuTrackTabRenderer.DropDownOpening += new EventHandler(cMenuTrackTabRenderer_DropDownOpening);
            cMenuTrackTabRendererVOCALOID1.Click += new EventHandler(handleChangeRenderer);
            cMenuTrackTabRendererVOCALOID2.Click += new EventHandler(handleChangeRenderer);
            cMenuTrackTabRendererStraight.Click += new EventHandler(handleChangeRenderer);
            cMenuTrackTabRendererAquesTone.Click += new EventHandler(handleChangeRenderer);
            cMenuTrackSelector.Opening += new CancelEventHandler(cMenuTrackSelector_Opening);
            cMenuTrackSelectorPointer.Click += new EventHandler(cMenuTrackSelectorPointer_Click);
            cMenuTrackSelectorPencil.Click += new EventHandler(cMenuTrackSelectorPencil_Click);
            cMenuTrackSelectorLine.Click += new EventHandler(cMenuTrackSelectorLine_Click);
            cMenuTrackSelectorEraser.Click += new EventHandler(cMenuTrackSelectorEraser_Click);
            cMenuTrackSelectorCurve.Click += new EventHandler(cMenuTrackSelectorCurve_Click);
            cMenuTrackSelectorUndo.Click += new EventHandler(cMenuTrackSelectorUndo_Click);
            cMenuTrackSelectorRedo.Click += new EventHandler(cMenuTrackSelectorRedo_Click);
            cMenuTrackSelectorCut.Click += new EventHandler(cMenuTrackSelectorCut_Click);
            cMenuTrackSelectorCopy.Click += new EventHandler(cMenuTrackSelectorCopy_Click);
            cMenuTrackSelectorPaste.Click += new EventHandler(cMenuTrackSelectorPaste_Click);
            cMenuTrackSelectorDelete.Click += new EventHandler(cMenuTrackSelectorDelete_Click);
            cMenuTrackSelectorDeleteBezier.Click += new EventHandler(cMenuTrackSelectorDeleteBezier_Click);
            cMenuTrackSelectorSelectAll.Click += new EventHandler(cMenuTrackSelectorSelectAll_Click);
            cMenuPositionIndicatorEndMarker.Click += new EventHandler(cMenuPositionIndicatorEndMarker_Click);
            cMenuPositionIndicatorStartMarker.Click += new EventHandler(cMenuPositionIndicatorStartMarker_Click);
            trackBar.ValueChanged += new EventHandler(trackBar_ValueChanged);
            trackBar.Enter += new EventHandler(trackBar_Enter);
            bgWorkScreen.DoWork += new DoWorkEventHandler(bgWorkScreen_DoWork);
            timer.Tick += new EventHandler(timer_Tick);
            pictKeyLengthSplitter.MouseMove += new MouseEventHandler(pictKeyLengthSplitter_MouseMove);
            pictKeyLengthSplitter.MouseDown += new MouseEventHandler(pictKeyLengthSplitter_MouseDown);
            pictKeyLengthSplitter.MouseUp += new MouseEventHandler(pictKeyLengthSplitter_MouseUp);
            panelOverview.KeyUp += new KeyEventHandler(handleSpaceKeyUp);
            panelOverview.KeyDown += new KeyEventHandler(handleSpaceKeyDown);
            vScroll.ValueChanged += new EventHandler(vScroll_ValueChanged);
            //this.Resize += new EventHandler( handleVScrollResize );
            pictPianoRoll.Resize += new EventHandler(handleVScrollResize);
            vScroll.Enter += new EventHandler(vScroll_Enter);
            hScroll.ValueChanged += new EventHandler(hScroll_ValueChanged);
            hScroll.Resize += new EventHandler(hScroll_Resize);
            hScroll.Enter += new EventHandler(hScroll_Enter);
            picturePositionIndicator.PreviewKeyDown += new PreviewKeyDownEventHandler(picturePositionIndicator_PreviewKeyDown);
            picturePositionIndicator.MouseMove += new MouseEventHandler(picturePositionIndicator_MouseMove);
            picturePositionIndicator.MouseClick += new MouseEventHandler(picturePositionIndicator_MouseClick);
            picturePositionIndicator.MouseDoubleClick += new MouseEventHandler(picturePositionIndicator_MouseDoubleClick);
            picturePositionIndicator.MouseDown += new MouseEventHandler(picturePositionIndicator_MouseDown);
            picturePositionIndicator.MouseUp += new MouseEventHandler(picturePositionIndicator_MouseUp);
            picturePositionIndicator.Paint += new PaintEventHandler(picturePositionIndicator_Paint);
            pictPianoRoll.PreviewKeyDown += new PreviewKeyDownEventHandler(pictPianoRoll_PreviewKeyDown);
            pictPianoRoll.KeyUp += new KeyEventHandler(handleSpaceKeyUp);
            pictPianoRoll.KeyUp += new KeyEventHandler(pictPianoRoll_KeyUp);
            pictPianoRoll.MouseMove += new MouseEventHandler(pictPianoRoll_MouseMove);
            pictPianoRoll.MouseDoubleClick += new MouseEventHandler(pictPianoRoll_MouseDoubleClick);
            pictPianoRoll.MouseClick += new MouseEventHandler(pictPianoRoll_MouseClick);
            pictPianoRoll.MouseDown += new MouseEventHandler(pictPianoRoll_MouseDown);
            pictPianoRoll.MouseUp += new MouseEventHandler(pictPianoRoll_MouseUp);
            pictPianoRoll.KeyDown += new KeyEventHandler(handleSpaceKeyDown);
            waveView.MouseDoubleClick += new MouseEventHandler(waveView_MouseDoubleClick);
            waveView.MouseDown += new MouseEventHandler(waveView_MouseDown);
            waveView.MouseUp += new MouseEventHandler(waveView_MouseUp);
            waveView.MouseMove += new MouseEventHandler(waveView_MouseMove);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(FormMain_DragEnter);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(FormMain_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(FormMain_DragOver);
            this.DragLeave += new EventHandler(FormMain_DragLeave);

            pictureBox2.MouseDown += new MouseEventHandler(pictureBox2_MouseDown);
            pictureBox2.MouseUp += new MouseEventHandler(pictureBox2_MouseUp);
            pictureBox2.Paint += new PaintEventHandler(pictureBox2_Paint);
            toolBarTool.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(toolBarTool_ButtonClick);
            rebar.SizeChanged += new EventHandler(toolStripContainer_TopToolStripPanel_SizeChanged);
            stripDDBtnQuantize04.Click += handlePositionQuantize;
            stripDDBtnQuantize08.Click += handlePositionQuantize;
            stripDDBtnQuantize16.Click += handlePositionQuantize;
            stripDDBtnQuantize32.Click += handlePositionQuantize;
            stripDDBtnQuantize64.Click += handlePositionQuantize;
            stripDDBtnQuantize128.Click += handlePositionQuantize;
            stripDDBtnQuantizeOff.Click += handlePositionQuantize;
            stripDDBtnQuantizeTriplet.Click += handlePositionQuantizeTriplet_Click;
            toolBarFile.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(toolBarFile_ButtonClick);
            toolBarPosition.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(toolBarPosition_ButtonClick);
            toolBarMeasure.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(toolBarMeasure_ButtonClick);
            toolBarMeasure.MouseDown += new MouseEventHandler(toolBarMeasure_MouseDown);
            stripBtnStepSequencer.CheckedChanged += new EventHandler(stripBtnStepSequencer_CheckedChanged);
            this.Deactivate += new EventHandler(FormMain_Deactivate);
            this.Activated += new EventHandler(FormMain_Activated);
            this.FormClosed += new FormClosedEventHandler(FormMain_FormClosed);
            this.FormClosing += new FormClosingEventHandler(FormMain_FormClosing);
            this.PreviewKeyDown += new PreviewKeyDownEventHandler(FormMain_PreviewKeyDown);
            this.SizeChanged += FormMain_SizeChanged;
            panelOverview.Enter += new EventHandler(panelOverview_Enter);
        }

        public void setResources()
        {
            try {
                this.stripLblGameCtrlMode.Image = Properties.Resources.slash;
                this.stripLblMidiIn.Image = Properties.Resources.slash;

                this.stripBtnStepSequencer.Image = Properties.Resources.piano;
                this.Icon = Properties.Resources.Icon1;
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".setResources; ex=" + ex + "\n");
                serr.println("FormMain#setResources; ex=" + ex);
            }
        }
        #endregion // public methods

        #region event handlers
        public void menuWindowMinimize_Click(Object sender, EventArgs e)
        {
            var state = this.WindowState;
            if (state != FormWindowState.Minimized) {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        //BOOKMARK: panelOverview
        #region panelOverview
        public void panelOverview_Enter(Object sender, EventArgs e)
        {
            controller.navigationPanelGotFocus();
        }
        #endregion

        //BOOKMARK: inputTextBox
        #region AppManager.mInputTextBox
        public void mInputTextBox_KeyDown(Object sender, KeyEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#mInputTextBox_KeyDown");
#endif
            bool shift = (e.Modifiers & System.Windows.Forms.Keys.Shift) == System.Windows.Forms.Keys.Shift;
            bool tab = e.KeyCode == System.Windows.Forms.Keys.Tab;
            bool enter = e.KeyCode == System.Windows.Forms.Keys.Return;
            if (tab || enter) {
                executeLyricChangeCommand();
                int selected = AppManager.getSelected();
                int index = -1;
                int width = pictPianoRoll.getWidth();
                int height = pictPianoRoll.getHeight();
                int key_width = AppManager.keyWidth;
                VsqTrack track = AppManager.getVsqFile().Track[selected];
                track.sortEvent();
                if (tab) {
                    int clock = 0;
                    int search_index = AppManager.itemSelection.getLastEvent().original.InternalID;
                    int c = track.getEventCount();
                    for (int i = 0; i < c; i++) {
                        VsqEvent item = track.getEvent(i);
                        if (item.InternalID == search_index) {
                            index = i;
                            clock = item.Clock;
                            break;
                        }
                    }
                    if (shift) {
                        // 1個前の音符イベントを検索
                        int tindex = -1;
                        for (int i = track.getEventCount() - 1; i >= 0; i--) {
                            VsqEvent ve = track.getEvent(i);
                            if (ve.ID.type == VsqIDType.Anote && i != index && ve.Clock <= clock) {
                                tindex = i;
                                break;
                            }
                        }
                        index = tindex;
                    } else {
                        // 1個後の音符イベントを検索
                        int tindex = -1;
                        int c2 = track.getEventCount();
                        for (int i = 0; i < c2; i++) {
                            VsqEvent ve = track.getEvent(i);
                            if (ve.ID.type == VsqIDType.Anote && i != index && ve.Clock >= clock) {
                                tindex = i;
                                break;
                            }
                        }
                        index = tindex;
                    }
                }
                if (0 <= index && index < track.getEventCount()) {
                    AppManager.itemSelection.clearEvent();
                    VsqEvent item = track.getEvent(index);
                    AppManager.itemSelection.addEvent(item.InternalID);
                    int x = AppManager.xCoordFromClocks(item.Clock);
                    int y = AppManager.yCoordFromNote(item.ID.Note);
                    bool phonetic_symbol_edit_mode = AppManager.mInputTextBox.isPhoneticSymbolEditMode();
                    showInputTextBox(
                        item.ID.LyricHandle.L0.Phrase,
                        item.ID.LyricHandle.L0.getPhoneticSymbol(),
                        new Point(x, y),
                        phonetic_symbol_edit_mode);
                    int clWidth = (int)(AppManager.mInputTextBox.Width * controller.getScaleXInv());

                    // 画面上にAppManager.mInputTextBoxが見えるように，移動
                    int SPACE = 20;
                    // vScrollやhScrollをいじった場合はfalseにする．
                    bool refresh_screen = true;
                    // X軸方向について，見えるように移動
                    if (x < key_width || width < x + AppManager.mInputTextBox.Width) {
                        int clock, clock_x;
                        if (x < key_width) {
                            // 左に隠れてしまう場合
                            clock = item.Clock;
                        } else {
                            // 右に隠れてしまう場合
                            clock = item.Clock + clWidth;
                        }
                        if (shift) {
                            // 左方向に移動していた場合
                            // 右から３分の１の位置に移動させる
                            clock_x = width - (width - key_width) / 3;
                        } else {
                            // 右方向に移動していた場合
                            clock_x = key_width + (width - key_width) / 3;
                        }
                        double draft_d = (key_width + AppManager.keyOffset - clock_x) * controller.getScaleXInv() + clock;
                        if (draft_d < 0.0) {
                            draft_d = 0.0;
                        }
                        int draft = (int)draft_d;
                        if (draft < hScroll.Minimum) {
                            draft = hScroll.Minimum;
                        } else if (hScroll.Maximum < draft) {
                            draft = hScroll.Maximum;
                        }
                        refresh_screen = false;
                        hScroll.Value = draft;
                    }
                    // y軸方向について，見えるように移動
                    int track_height = (int)(100 * controller.getScaleY());
                    if (y <= 0 || height - track_height <= y) {
                        int note = item.ID.Note;
                        if (y <= 0) {
                            // 上にはみ出してしまう場合
                            note = item.ID.Note + 1;
                        } else {
                            // 下にはみ出してしまう場合
                            note = item.ID.Note - 2;
                        }
                        if (127 < note) {
                            note = 127;
                        }
                        if (note < 0) {
                            note = 0;
                        }
                        ensureVisibleY(note);
                    }
                    if (refresh_screen) {
                        refreshScreen();
                    }
                } else {
                    int id = AppManager.itemSelection.getLastEvent().original.InternalID;
                    AppManager.itemSelection.clearEvent();
                    AppManager.itemSelection.addEvent(id);
                    hideInputTextBox();
                }
            }
        }

        public void mInputTextBox_KeyUp(Object sender, KeyEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#mInputTextBox_KeyUp");
#endif
            bool flip = (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) && (e.Modifiers == Keys.Alt);
            bool hide = e.KeyCode == Keys.Escape;

            if (flip) {
                if (AppManager.mInputTextBox.Visible) {
                    flipInputTextBoxMode();
                }
            } else if (hide) {
                hideInputTextBox();
            }
        }

        public void mInputTextBox_ImeModeChanged(Object sender, EventArgs e)
        {
            mLastIsImeModeOn = AppManager.mInputTextBox.ImeMode == System.Windows.Forms.ImeMode.Hiragana;
        }

        public void mInputTextBox_KeyPress(Object sender, KeyPressEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#mInputTextBox_KeyPress");
#endif
            //           Enter                                  Tab
            e.Handled = (e.KeyChar == Convert.ToChar(13)) || (e.KeyChar == Convert.ToChar(09));
        }
        #endregion

        //BOOKMARK: AppManager
        #region AppManager
        public void AppManager_EditedStateChanged(Object sender, bool edited)
        {
            setEdited(edited);
        }

        public void AppManager_GridVisibleChanged(Object sender, EventArgs e)
        {
            menuVisualGridline.Checked = AppManager.isGridVisible();
            stripBtnGrid.Pushed = AppManager.isGridVisible();
            cMenuPianoGrid.Checked = AppManager.isGridVisible();
        }

        public void AppManager_MainWindowFocusRequired(Object sender, EventArgs e)
        {
            this.Focus();
        }

        public void AppManager_PreviewAborted(Object sender, EventArgs e)
        {
#if DEBUG
            sout.println("FormMain#AppManager_PreviewAborted");
#endif
            stripBtnPlay.ImageKey = "control.png";
            stripBtnPlay.Text = _("Play");
            timer.Stop();

            for (int i = 0; i < AppManager.mDrawStartIndex.Length; i++) {
                AppManager.mDrawStartIndex[i] = 0;
            }
#if ENABLE_MIDI
            //MidiPlayer.stop();
#endif // ENABLE_MIDI
        }

        public void AppManager_PreviewStarted(Object sender, EventArgs e)
        {
#if DEBUG
            sout.println("FormMain#AppManager_PreviewStarted");
#endif
            AppManager.mAddingEvent = null;
            int selected = AppManager.getSelected();
            VsqFileEx vsq = AppManager.getVsqFile();
            RendererKind renderer = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
            int clock = AppManager.getCurrentClock();
            mLastClock = clock;
            double now = PortUtil.getCurrentTime();
            AppManager.mPreviewStartedTime = now;
            timer.Start();
            stripBtnPlay.ImageKey = "control_pause.png";
            stripBtnPlay.Text = _("Stop");
        }

        public void AppManager_SelectedToolChanged(Object sender, EventArgs e)
        {
            applySelectedTool();
        }

        public void ItemSelectionModel_SelectedEventChanged(Object sender, bool selected_event_is_null)
        {
            menuEditCut.Enabled = !selected_event_is_null;
            menuEditPaste.Enabled = !selected_event_is_null;
            menuEditDelete.Enabled = !selected_event_is_null;
            cMenuPianoCut.Enabled = !selected_event_is_null;
            cMenuPianoCopy.Enabled = !selected_event_is_null;
            cMenuPianoDelete.Enabled = !selected_event_is_null;
            cMenuPianoExpressionProperty.Enabled = !selected_event_is_null;
            menuLyricVibratoProperty.Enabled = !selected_event_is_null;
            menuLyricExpressionProperty.Enabled = !selected_event_is_null;
            stripBtnCut.Enabled = !selected_event_is_null;
            stripBtnCopy.Enabled = !selected_event_is_null;
        }

        public void AppManager_UpdateBgmStatusRequired(Object sender, EventArgs e)
        {
            updateBgmMenuState();
        }

        public void AppManager_WaveViewRealoadRequired(Object sender, WaveViewRealoadRequiredEventArgs arg)
        {
            int track = arg.track;
            string file = arg.file;
            double sec_start = arg.secStart;
            double sec_end = arg.secEnd;
            if (sec_start <= sec_end) {
                waveView.reloadPartial(track - 1, file, sec_start, sec_end);
            } else {
                waveView.load(track - 1, file);
            }
        }
        #endregion

        //BOOKMARK: pictPianoRoll
        #region pictPianoRoll
        public void pictPianoRoll_KeyUp(Object sender, KeyEventArgs e)
        {
            processSpecialShortcutKey(e, false);
        }

        public void pictPianoRoll_MouseClick(Object sender, MouseEventArgs e)
        {
#if DEBUG
            AppManager.debugWriteLine("pictPianoRoll_MouseClick");
#endif
            Keys modefiers = Control.ModifierKeys;
            EditMode edit_mode = AppManager.getEditMode();

            bool is_button_left = e.Button == MouseButtons.Left;
            int selected = AppManager.getSelected();

            if (e.Button == MouseButtons.Left) {
#if ENABLE_MOUSEHOVER
                if ( mMouseHoverThread != null ) {
                    mMouseHoverThread.Abort();
                }
#endif

                // クリック位置にIDが無いかどうかを検査
                ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>(new Rectangle());
                VsqEvent item = getItemAtClickedPosition(new Point(e.X, e.Y), out_id_rect);
                Rectangle id_rect = out_id_rect.value;
#if DEBUG
                AppManager.debugWriteLine("    (item==null)=" + (item == null));
#endif
                if (item != null &&
                     edit_mode != EditMode.MOVE_ENTRY_WAIT_MOVE &&
                     edit_mode != EditMode.MOVE_ENTRY &&
                     edit_mode != EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE &&
                     edit_mode != EditMode.MOVE_ENTRY_WHOLE &&
                     edit_mode != EditMode.EDIT_LEFT_EDGE &&
                     edit_mode != EditMode.EDIT_RIGHT_EDGE &&
                     edit_mode != EditMode.MIDDLE_DRAG &&
                     edit_mode != EditMode.CURVE_ON_PIANOROLL) {
                    if ((modefiers & Keys.Shift) != Keys.Shift && (modefiers & s_modifier_key) != s_modifier_key) {
                        AppManager.itemSelection.clearEvent();
                    }
                    AppManager.itemSelection.addEvent(item.InternalID);
                    int internal_id = item.InternalID;
                    hideInputTextBox();
                    if (AppManager.getSelectedTool() == EditTool.ERASER) {
                        CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventDelete(selected, internal_id));
                        AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                        setEdited(true);
                        AppManager.itemSelection.clearEvent();
                        return;
#if ENABLE_SCRIPT
                    } else if (AppManager.getSelectedTool() == EditTool.PALETTE_TOOL) {
                        List<int> internal_ids = new List<int>();
                        foreach (var see in AppManager.itemSelection.getEventIterator()) {
                            internal_ids.Add(see.original.InternalID);
                        }
                        MouseButtons btn = e.Button;
                        if (isMouseMiddleButtonDowned(btn)) {
                            btn = MouseButtons.Middle;
                        }
                        bool result = PaletteToolServer.invokePaletteTool(AppManager.mSelectedPaletteTool,
                                                                              selected,
                                                                              internal_ids.ToArray(),
                                                                              btn);
                        if (result) {
                            setEdited(true);
                            AppManager.itemSelection.clearEvent();
                            return;
                        }
#endif
                    }
                } else {
                    if (edit_mode != EditMode.MOVE_ENTRY_WAIT_MOVE &&
                         edit_mode != EditMode.MOVE_ENTRY &&
                         edit_mode != EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE &&
                         edit_mode != EditMode.MOVE_ENTRY_WHOLE &&
                         edit_mode != EditMode.EDIT_LEFT_EDGE &&
                         edit_mode != EditMode.EDIT_RIGHT_EDGE &&
                         edit_mode != EditMode.EDIT_VIBRATO_DELAY) {
                        if (!AppManager.mIsPointerDowned) {
                            AppManager.itemSelection.clearEvent();
                        }
                        hideInputTextBox();
                    }
                    if (AppManager.getSelectedTool() == EditTool.ERASER) {
                        // マウス位置にビブラートの波波があったら削除する
                        int stdx = controller.getStartToDrawX();
                        int stdy = controller.getStartToDrawY();
                        for (int i = 0; i < AppManager.mDrawObjects[selected - 1].Count; i++) {
                            DrawObject dobj = AppManager.mDrawObjects[selected - 1][i];
                            if (dobj.mRectangleInPixel.x + controller.getStartToDrawX() + dobj.mRectangleInPixel.width - stdx < 0) {
                                continue;
                            } else if (pictPianoRoll.getWidth() < dobj.mRectangleInPixel.x + AppManager.keyWidth - stdx) {
                                break;
                            }
                            Rectangle rc = new Rectangle(dobj.mRectangleInPixel.x + AppManager.keyWidth + dobj.mVibratoDelayInPixel - stdx,
                                                          dobj.mRectangleInPixel.y + (int)(100 * controller.getScaleY()) - stdy,
                                                          dobj.mRectangleInPixel.width - dobj.mVibratoDelayInPixel,
                                                          (int)(100 * controller.getScaleY()));
                            if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                //ビブラートの範囲なのでビブラートを消す
                                VsqEvent item3 = null;
                                VsqID item2 = null;
                                int internal_id = -1;
                                internal_id = dobj.mInternalID;
                                foreach (var ve in AppManager.getVsqFile().Track[selected].getNoteEventIterator()) {
                                    if (ve.InternalID == dobj.mInternalID) {
                                        item2 = (VsqID)ve.ID.clone();
                                        item3 = ve;
                                        break;
                                    }
                                }
                                if (item2 != null) {
                                    item2.VibratoHandle = null;
                                    CadenciiCommand run = new CadenciiCommand(
                                        VsqCommand.generateCommandEventChangeIDContaints(selected,
                                                                                          internal_id,
                                                                                          item2));
                                    AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                                    setEdited(true);
                                }
                                break;
                            }
                        }
                    }
                }
            } else if (e.Button == MouseButtons.Right) {
                bool show_context_menu = (e.X > AppManager.keyWidth);
#if ENABLE_MOUSEHOVER
                if ( mMouseHoverThread != null ) {
                    if ( !mMouseHoverThread.IsAlive && AppManager.editorConfig.PlayPreviewWhenRightClick ) {
                        show_context_menu = false;
                    }
                } else {
                    if ( AppManager.editorConfig.PlayPreviewWhenRightClick ) {
                        show_context_menu = false;
                    }
                }
#endif
                show_context_menu = AppManager.showContextMenuWhenRightClickedOnPianoroll ? (show_context_menu && !mMouseMoved) : false;
                if (show_context_menu) {
#if ENABLE_MOUSEHOVER
                    if ( mMouseHoverThread != null ) {
                        mMouseHoverThread.Abort();
                    }
#endif
                    ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>();
                    VsqEvent item = getItemAtClickedPosition(new Point(e.X, e.Y), out_id_rect);
                    Rectangle id_rect = out_id_rect.value;
                    if (item != null) {
                        if (!AppManager.itemSelection.isEventContains(AppManager.getSelected(), item.InternalID)) {
                            AppManager.itemSelection.clearEvent();
                        }
                        AppManager.itemSelection.addEvent(item.InternalID);
                    }
                    bool item_is_null = (item == null);
                    cMenuPianoCopy.Enabled = !item_is_null;
                    cMenuPianoCut.Enabled = !item_is_null;
                    cMenuPianoDelete.Enabled = !item_is_null;
                    cMenuPianoImportLyric.Enabled = !item_is_null;
                    cMenuPianoExpressionProperty.Enabled = !item_is_null;

                    int clock = AppManager.clockFromXCoord(e.X);
                    cMenuPianoPaste.Enabled = ((AppManager.clipboard.getCopiedItems().events.Count != 0) && (clock >= AppManager.getVsqFile().getPreMeasureClocks()));
                    refreshScreen();

                    mContextMenuOpenedPosition = new Point(e.X, e.Y);
                    cMenuPiano.Show(pictPianoRoll, e.X, e.Y);
                } else {
                    ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>();
                    VsqEvent item = getItemAtClickedPosition(mButtonInitial, out_id_rect);
                    Rectangle id_rect = out_id_rect.value;
#if DEBUG
                    AppManager.debugWriteLine("pitcPianoRoll_MouseClick; button is right; (item==null)=" + (item == null));
#endif
                    if (item != null) {
                        int itemx = AppManager.xCoordFromClocks(item.Clock);
                        int itemy = AppManager.yCoordFromNote(item.ID.Note);
                    }
                }
            } else if (e.Button == MouseButtons.Middle) {
#if ENABLE_SCRIPT
                if (AppManager.getSelectedTool() == EditTool.PALETTE_TOOL) {
                    ByRef<Rectangle> out_id_rect = new ByRef<Rectangle>();
                    VsqEvent item = getItemAtClickedPosition(new Point(e.X, e.Y), out_id_rect);
                    Rectangle id_rect = out_id_rect.value;
                    if (item != null) {
                        AppManager.itemSelection.clearEvent();
                        AppManager.itemSelection.addEvent(item.InternalID);
                        List<int> internal_ids = new List<int>();
                        foreach (var see in AppManager.itemSelection.getEventIterator()) {
                            internal_ids.Add(see.original.InternalID);
                        }
                        bool result = PaletteToolServer.invokePaletteTool(AppManager.mSelectedPaletteTool,
                                                                           AppManager.getSelected(),
                                                                           internal_ids.ToArray(),
                                                                           e.Button);
                        if (result) {
                            setEdited(true);
                            AppManager.itemSelection.clearEvent();
                            return;
                        }
                    }
                }
#endif
            }
        }

        public void pictPianoRoll_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
#if DEBUG
            AppManager.debugWriteLine("FormMain#pictPianoRoll_MouseDoubleClick");
#endif
            ByRef<Rectangle> out_rect = new ByRef<Rectangle>();
            VsqEvent item = getItemAtClickedPosition(new Point(e.X, e.Y), out_rect);
            Rectangle rect = out_rect.value;
            int selected = AppManager.getSelected();
            VsqFileEx vsq = AppManager.getVsqFile();
            if (item != null && item.ID.type == VsqIDType.Anote) {
#if ENABLE_SCRIPT
                if (AppManager.getSelectedTool() != EditTool.PALETTE_TOOL)
#endif

 {
                    AppManager.itemSelection.clearEvent();
                    AppManager.itemSelection.addEvent(item.InternalID);
#if ENABLE_MOUSEHOVER
                    mMouseHoverThread.Abort();
#endif
                    if (!AppManager.editorConfig.KeepLyricInputMode) {
                        mLastSymbolEditMode = false;
                    }
                    showInputTextBox(
                        item.ID.LyricHandle.L0.Phrase,
                        item.ID.LyricHandle.L0.getPhoneticSymbol(),
                        new Point(rect.x, rect.y),
                        mLastSymbolEditMode);
                    refreshScreen();
                    return;
                }
            } else {
                AppManager.itemSelection.clearEvent();
                hideInputTextBox();
                if (AppManager.editorConfig.ShowExpLine && AppManager.keyWidth <= e.X) {
                    int stdx = controller.getStartToDrawX();
                    int stdy = controller.getStartToDrawY();
                    foreach (var dobj in AppManager.mDrawObjects[selected - 1]) {
                        // 表情コントロールプロパティを表示するかどうかを決める
                        rect = new Rectangle(
                            dobj.mRectangleInPixel.x + AppManager.keyWidth - stdx,
                            dobj.mRectangleInPixel.y - stdy + (int)(100 * controller.getScaleY()),
                            21,
                            (int)(100 * controller.getScaleY()));
                        if (Utility.isInRect(new Point(e.X, e.Y), rect)) {
                            VsqEvent selectedEvent = null;
                            for (Iterator<VsqEvent> itr2 = vsq.Track[selected].getEventIterator(); itr2.hasNext(); ) {
                                VsqEvent ev = itr2.next();
                                if (ev.InternalID == dobj.mInternalID) {
                                    selectedEvent = ev;
                                    break;
                                }
                            }
                            if (selectedEvent != null) {
#if ENABLE_MOUSEHOVER
                                if ( mMouseHoverThread != null ) {
                                    mMouseHoverThread.Abort();
                                }
#endif
                                SynthesizerType type = SynthesizerType.VOCALOID2;
                                RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
                                if (kind == RendererKind.VOCALOID1) {
                                    type = SynthesizerType.VOCALOID1;
                                }
                                FormNoteExpressionConfig dlg = null;
                                try {
                                    dlg = new FormNoteExpressionConfig(type, selectedEvent.ID.NoteHeadHandle);
                                    dlg.setPMBendDepth(selectedEvent.ID.PMBendDepth);
                                    dlg.setPMBendLength(selectedEvent.ID.PMBendLength);
                                    dlg.setPMbPortamentoUse(selectedEvent.ID.PMbPortamentoUse);
                                    dlg.setDEMdecGainRate(selectedEvent.ID.DEMdecGainRate);
                                    dlg.setDEMaccent(selectedEvent.ID.DEMaccent);
                                    dlg.Location = getFormPreferedLocation(dlg);
                                    DialogResult dr = AppManager.showModalDialog(dlg, this);
                                    if (dr == DialogResult.OK) {
                                        VsqID id = (VsqID)selectedEvent.ID.clone();
                                        id.PMBendDepth = dlg.getPMBendDepth();
                                        id.PMBendLength = dlg.getPMBendLength();
                                        id.PMbPortamentoUse = dlg.getPMbPortamentoUse();
                                        id.DEMdecGainRate = dlg.getDEMdecGainRate();
                                        id.DEMaccent = dlg.getDEMaccent();
                                        id.NoteHeadHandle = dlg.getEditedNoteHeadHandle();
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandEventChangeIDContaints(selected, selectedEvent.InternalID, id));
                                        AppManager.editHistory.register(vsq.executeCommand(run));
                                        setEdited(true);
                                        refreshScreen();
                                    }
                                } catch (Exception ex) {
                                    Logger.write(typeof(FormMain) + ".pictPianoRoll_MouseDoubleClick; ex=" + ex + "\n");
                                    serr.println(typeof(FormMain) + ".pictPianoRoll_MouseDoubleClick" + ex);
                                } finally {
                                    if (dlg != null) {
                                        try {
                                            dlg.Close();
                                        } catch (Exception ex2) {
                                            Logger.write(typeof(FormMain) + ".pictPianoRoll_MouseDoubleClick; ex=" + ex2 + "\n");
                                            serr.println(typeof(FormMain) + ".pictPianoRoll_MouseDoubleClick");
                                        }
                                    }
                                }
                                return;
                            }
                            break;
                        }

                        // ビブラートプロパティダイアログを表示するかどうかを決める
                        rect = new Rectangle(
                            dobj.mRectangleInPixel.x + AppManager.keyWidth - stdx + 21,
                            dobj.mRectangleInPixel.y - stdy + (int)(100 * controller.getScaleY()),
                            dobj.mRectangleInPixel.width - 21,
                            (int)(100 * controller.getScaleY()));
                        if (Utility.isInRect(new Point(e.X, e.Y), rect)) {
                            VsqEvent selectedEvent = null;
                            for (Iterator<VsqEvent> itr2 = vsq.Track[selected].getEventIterator(); itr2.hasNext(); ) {
                                VsqEvent ev = itr2.next();
                                if (ev.InternalID == dobj.mInternalID) {
                                    selectedEvent = ev;
                                    break;
                                }
                            }
                            if (selectedEvent != null) {
#if ENABLE_MOUSEHOVER
                                if ( mMouseHoverThread != null ) {
                                    mMouseHoverThread.Abort();
                                }
#endif
                                SynthesizerType type = SynthesizerType.VOCALOID2;
                                RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
#if DEBUG
                                sout.println("FormMain#pictPianoRoll_MouseDoubleClick; kind=" + kind);
#endif
                                if (kind == RendererKind.VOCALOID1) {
                                    type = SynthesizerType.VOCALOID1;
                                }
                                FormVibratoConfig dlg = null;
                                try {
                                    dlg = new FormVibratoConfig(
                                        selectedEvent.ID.VibratoHandle,
                                        selectedEvent.ID.getLength(),
                                        AppManager.editorConfig.DefaultVibratoLength,
                                        type,
                                        AppManager.editorConfig.UseUserDefinedAutoVibratoType);
                                    dlg.Location = getFormPreferedLocation(dlg);
                                    DialogResult dr = AppManager.showModalDialog(dlg, this);
                                    if (dr == DialogResult.OK) {
                                        VsqID t = (VsqID)selectedEvent.ID.clone();
                                        VibratoHandle handle = dlg.getVibratoHandle();
#if DEBUG
                                        sout.println("FormMain#pictPianoRoll_MouseDoubleClick; (handle==null)=" + (handle == null));
#endif
                                        if (handle != null) {
                                            string iconid = handle.IconID;
                                            int vibrato_length = handle.getLength();
                                            int note_length = selectedEvent.ID.getLength();
                                            t.VibratoDelay = note_length - vibrato_length;
                                            t.VibratoHandle = handle;
                                        } else {
                                            t.VibratoHandle = null;
                                        }
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandEventChangeIDContaints(
                                                selected,
                                                selectedEvent.InternalID,
                                                t));
                                        AppManager.editHistory.register(vsq.executeCommand(run));
                                        setEdited(true);
                                        refreshScreen();
                                    }
                                } catch (Exception ex) {
                                    Logger.write(typeof(FormMain) + ".pictPianoRoll_MouseDoubleClick; ex=" + ex + "\n");
                                } finally {
                                    if (dlg != null) {
                                        try {
                                            dlg.Close();
                                        } catch (Exception ex2) {
                                            Logger.write(typeof(FormMain) + ".pictPianoRoll_MouseDoubleClick; ex=" + ex2 + "\n");
                                        }
                                    }
                                }
                                return;
                            }
                            break;
                        }

                    }
                }
            }

            if (e.Button == MouseButtons.Left) {
                // 必要な操作が何も無ければ，クリック位置にソングポジションを移動
                if (AppManager.keyWidth < e.X) {
                    int clock = doQuantize(AppManager.clockFromXCoord(e.X), AppManager.getPositionQuantizeClock());
                    AppManager.setCurrentClock(clock);
                }
            } else if (e.Button == MouseButtons.Middle) {
                // ツールをポインター <--> 鉛筆に切り替える
                if (AppManager.keyWidth < e.X) {
                    if (AppManager.getSelectedTool() == EditTool.ARROW) {
                        AppManager.setSelectedTool(EditTool.PENCIL);
                    } else {
                        AppManager.setSelectedTool(EditTool.ARROW);
                    }
                }
            }
        }

        public void pictPianoRoll_MouseDown(Object sender, MouseEventArgs e0)
        {
#if DEBUG
            AppManager.debugWriteLine("pictPianoRoll_MouseDown");
#endif
            MouseButtons btn0 = e0.Button;
            if (isMouseMiddleButtonDowned(btn0)) {
                btn0 = MouseButtons.Middle;
            }
            MouseEventArgs e = new MouseEventArgs(btn0, e0.Clicks, e0.X, e0.Y, e0.Delta);

            mMouseMoved = false;
            if (!AppManager.isPlaying() && 0 <= e.X && e.X <= AppManager.keyWidth) {
                int note = AppManager.noteFromYCoord(e.Y);
                if (0 <= note && note <= 126) {
                    if (e.Button == MouseButtons.Left) {
                        KeySoundPlayer.play(note);
                    }
                    return;
                }
            }

            AppManager.itemSelection.clearTempo();
            AppManager.itemSelection.clearTimesig();
            AppManager.itemSelection.clearPoint();
            /*if ( e.Button == BMouseButtons.Left ) {
                AppManager.selectedRegionEnabled = false;
            }*/

            mMouseDowned = true;
            mButtonInitial = new Point(e.X, e.Y);
            Keys modefier = Control.ModifierKeys;

            EditTool selected_tool = AppManager.getSelectedTool();
#if ENABLE_SCRIPT
            if (selected_tool != EditTool.PALETTE_TOOL && e.Button == MouseButtons.Middle) {
#else
            if ( e.Button == BMouseButtons.Middle ) {
#endif
                AppManager.setEditMode(EditMode.MIDDLE_DRAG);
                mMiddleButtonVScroll = vScroll.Value;
                mMiddleButtonHScroll = hScroll.Value;
                return;
            }

            int stdx = controller.getStartToDrawX();
            int stdy = controller.getStartToDrawY();
            if (e.Button == MouseButtons.Left && AppManager.mCurveOnPianoroll && (selected_tool == EditTool.PENCIL || selected_tool == EditTool.LINE)) {
                pictPianoRoll.mMouseTracer.clear();
                pictPianoRoll.mMouseTracer.appendFirst(e.X + stdx, e.Y + stdy);
                this.Cursor = Cursors.Default;
                AppManager.setEditMode(EditMode.CURVE_ON_PIANOROLL);
                return;
            }

            ByRef<Rectangle> out_rect = new ByRef<Rectangle>();
            VsqEvent item = getItemAtClickedPosition(new Point(e.X, e.Y), out_rect);
            Rectangle rect = out_rect.value;

#if ENABLE_SCRIPT
            if (selected_tool == EditTool.PALETTE_TOOL && item == null && e.Button == MouseButtons.Middle) {
                AppManager.setEditMode(EditMode.MIDDLE_DRAG);
                mMiddleButtonVScroll = vScroll.Value;
                mMiddleButtonHScroll = hScroll.Value;
                return;
            }
#endif

            int selected = AppManager.getSelected();
            VsqFileEx vsq = AppManager.getVsqFile();
            VsqTrack vsq_track = vsq.Track[selected];
            int key_width = AppManager.keyWidth;

            // マウス位置にある音符を検索
            if (item == null) {
                if (e.Button == MouseButtons.Left) {
                    AppManager.setWholeSelectedIntervalEnabled(false);
                }
                #region 音符がなかった時
#if DEBUG
                AppManager.debugWriteLine("    No Event");
#endif
                if (AppManager.itemSelection.getLastEvent() != null) {
                    executeLyricChangeCommand();
                }
                bool start_mouse_hover_generator = true;

                // CTRLキーを押しながら範囲選択
                if ((modefier & s_modifier_key) == s_modifier_key) {
                    AppManager.setWholeSelectedIntervalEnabled(true);
                    AppManager.setCurveSelectedIntervalEnabled(false);
                    AppManager.itemSelection.clearPoint();
                    int startClock = AppManager.clockFromXCoord(e.X);
                    if (AppManager.editorConfig.CurveSelectingQuantized) {
                        int unit = AppManager.getPositionQuantizeClock();
                        startClock = doQuantize(startClock, unit);
                    }
                    AppManager.mWholeSelectedInterval = new SelectedRegion(startClock);
                    AppManager.mWholeSelectedInterval.setEnd(startClock);
                    AppManager.mIsPointerDowned = true;
                } else {
                    DrawObject vibrato_dobj = null;
                    if (selected_tool == EditTool.LINE || selected_tool == EditTool.PENCIL) {
                        // ビブラート範囲の編集
                        int px_vibrato_length = 0;
                        mVibratoEditingId = -1;
                        Rectangle pxFound = new Rectangle();
                        List<DrawObject> target_list = AppManager.mDrawObjects[selected - 1];
                        int count = target_list.Count;
                        for (int i = 0; i < count; i++) {
                            DrawObject dobj = target_list[i];
                            if (dobj.mRectangleInPixel.width <= dobj.mVibratoDelayInPixel) {
                                continue;
                            }
                            if (dobj.mRectangleInPixel.x + key_width + dobj.mRectangleInPixel.width - stdx < 0) {
                                continue;
                            } else if (pictPianoRoll.getWidth() < dobj.mRectangleInPixel.x + key_width - stdx) {
                                break;
                            }
                            Rectangle rc = new Rectangle(dobj.mRectangleInPixel.x + key_width + dobj.mVibratoDelayInPixel - stdx - _EDIT_HANDLE_WIDTH / 2,
                                                dobj.mRectangleInPixel.y + (int)(100 * controller.getScaleY()) - stdy,
                                                _EDIT_HANDLE_WIDTH,
                                                (int)(100 * controller.getScaleY()));
                            if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                vibrato_dobj = dobj;
                                //vibrato_found = true;
                                mVibratoEditingId = dobj.mInternalID;
                                pxFound.x = dobj.mRectangleInPixel.x;
                                pxFound.y = dobj.mRectangleInPixel.y;
                                pxFound.width = dobj.mRectangleInPixel.width;
                                pxFound.height = dobj.mRectangleInPixel.height;// = new Rectangle dobj.mRectangleInPixel;
                                pxFound.x += key_width;
                                px_vibrato_length = dobj.mRectangleInPixel.width - dobj.mVibratoDelayInPixel;
                                break;
                            }
                        }
                        if (vibrato_dobj != null) {
                            int clock = AppManager.clockFromXCoord(pxFound.x + pxFound.width - px_vibrato_length - stdx);
                            int note = vibrato_dobj.mNote - 1;// AppManager.noteFromYCoord( pxFound.y + (int)(100 * AppManager.getScaleY()) - stdy );
                            int length = vibrato_dobj.mClock + vibrato_dobj.mLength - clock;// (int)(pxFound.width * AppManager.getScaleXInv());
                            AppManager.mAddingEvent = new VsqEvent(clock, new VsqID(0));
                            AppManager.mAddingEvent.ID.type = VsqIDType.Anote;
                            AppManager.mAddingEvent.ID.Note = note;
                            AppManager.mAddingEvent.ID.setLength(length);
                            AppManager.mAddingEventLength = vibrato_dobj.mLength;
                            AppManager.mAddingEvent.ID.VibratoDelay = length - (int)(px_vibrato_length * controller.getScaleXInv());
                            AppManager.setEditMode(EditMode.EDIT_VIBRATO_DELAY);
                            start_mouse_hover_generator = false;
                        }
                    }
                    if (vibrato_dobj == null) {
                        if ((selected_tool == EditTool.PENCIL || selected_tool == EditTool.LINE) &&
                            e.Button == MouseButtons.Left &&
                            e.X >= key_width) {
                            int clock = AppManager.clockFromXCoord(e.X);
                            if (AppManager.getVsqFile().getPreMeasureClocks() - AppManager.editorConfig.PxTolerance * controller.getScaleXInv() <= clock) {
                                //10ピクセルまでは許容範囲
                                if (AppManager.getVsqFile().getPreMeasureClocks() > clock) { //だけど矯正するよ。
                                    clock = AppManager.getVsqFile().getPreMeasureClocks();
                                }
                                int note = AppManager.noteFromYCoord(e.Y);
                                AppManager.itemSelection.clearEvent();
                                int unit = AppManager.getPositionQuantizeClock();
                                int new_clock = doQuantize(clock, unit);
                                AppManager.mAddingEvent = new VsqEvent(new_clock, new VsqID(0));
                                // デフォルトの歌唱スタイルを適用する
                                AppManager.editorConfig.applyDefaultSingerStyle(AppManager.mAddingEvent.ID);
                                if (mPencilMode.getMode() == PencilModeEnum.Off) {
                                    AppManager.setEditMode(EditMode.ADD_ENTRY);
                                    mButtonInitial = new Point(e.X, e.Y);
                                    AppManager.mAddingEvent.ID.setLength(0);
                                    AppManager.mAddingEvent.ID.Note = note;
                                    this.Cursor = Cursors.Default;
#if DEBUG
                                    AppManager.debugWriteLine("    EditMode=" + AppManager.getEditMode());
#endif
                                } else {
                                    AppManager.setEditMode(EditMode.ADD_FIXED_LENGTH_ENTRY);
                                    AppManager.mAddingEvent.ID.setLength(mPencilMode.getUnitLength());
                                    AppManager.mAddingEvent.ID.Note = note;
                                    this.Cursor = Cursors.Default;
                                }
                            } else {
                                SystemSounds.Asterisk.Play();
                            }
#if ENABLE_SCRIPT
                        } else if ((selected_tool == EditTool.ARROW || selected_tool == EditTool.PALETTE_TOOL) && e.Button == MouseButtons.Left) {
#else
                        } else if ( (selected_tool == EditTool.ARROW) && e.Button == BMouseButtons.Left ) {
#endif
                            AppManager.setWholeSelectedIntervalEnabled(false);
                            AppManager.itemSelection.clearEvent();
                            AppManager.mMouseDownLocation = new Point(e.X + stdx, e.Y + stdy);
                            AppManager.mIsPointerDowned = true;
#if DEBUG
                            AppManager.debugWriteLine("    EditMode=" + AppManager.getEditMode());
#endif
                        }
                    }
                }
                if (e.Button == MouseButtons.Right && !AppManager.editorConfig.PlayPreviewWhenRightClick) {
                    start_mouse_hover_generator = false;
                }
#if ENABLE_MOUSEHOVER
                if ( start_mouse_hover_generator ) {
                    mMouseHoverThread = new Thread( new ParameterizedThreadStart( MouseHoverEventGenerator ) );
                    mMouseHoverThread.Start( AppManager.noteFromYCoord( e.Y ) );
                }
#endif
                #endregion
            } else {
                #region 音符があった時
#if DEBUG
                AppManager.debugWriteLine("    Event Found");
#endif
                if (AppManager.itemSelection.isEventContains(selected, item.InternalID)) {
                    executeLyricChangeCommand();
                }
                hideInputTextBox();
                if (selected_tool != EditTool.ERASER) {
#if ENABLE_MOUSEHOVER
                    mMouseHoverThread = new Thread( new ParameterizedThreadStart( MouseHoverEventGenerator ) );
                    mMouseHoverThread.Start( item.ID.Note );
#endif
                }

                // まず、両端の編集モードに移行可能かどうか調べる
                if (item.ID.type != VsqIDType.Aicon ||
                     (item.ID.type == VsqIDType.Aicon && !item.ID.IconDynamicsHandle.isDynaffType())) {
#if ENABLE_SCRIPT
                    if (selected_tool != EditTool.ERASER && selected_tool != EditTool.PALETTE_TOOL && e.Button == MouseButtons.Left) {
#else
                    if ( selected_tool != EditTool.ERASER && e.Button == BMouseButtons.Left ) {
#endif
                        int min_width = 4 * _EDIT_HANDLE_WIDTH;
                        foreach (var dobj in AppManager.mDrawObjects[selected - 1]) {
                            int edit_handle_width = _EDIT_HANDLE_WIDTH;
                            if (dobj.mRectangleInPixel.width < min_width) {
                                edit_handle_width = dobj.mRectangleInPixel.width / 4;
                            }

                            // 左端の"のり代"にマウスがあるかどうか
                            Rectangle rc = new Rectangle(dobj.mRectangleInPixel.x - stdx + key_width,
                                                          dobj.mRectangleInPixel.y - stdy,
                                                          edit_handle_width,
                                                          dobj.mRectangleInPixel.height);
                            if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                AppManager.setWholeSelectedIntervalEnabled(false);
                                AppManager.setEditMode(EditMode.EDIT_LEFT_EDGE);
                                if (!AppManager.itemSelection.isEventContains(selected, item.InternalID)) {
                                    AppManager.itemSelection.clearEvent();
                                }
                                AppManager.itemSelection.addEvent(item.InternalID);
                                this.Cursor = System.Windows.Forms.Cursors.VSplit;
                                refreshScreen();
#if DEBUG
                                AppManager.debugWriteLine("    EditMode=" + AppManager.getEditMode());
#endif
                                return;
                            }

                            // 右端の糊代にマウスがあるかどうか
                            rc = new Rectangle(dobj.mRectangleInPixel.x + key_width + dobj.mRectangleInPixel.width - stdx - edit_handle_width,
                                                dobj.mRectangleInPixel.y - stdy,
                                                edit_handle_width,
                                                dobj.mRectangleInPixel.height);
                            if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                AppManager.setWholeSelectedIntervalEnabled(false);
                                AppManager.setEditMode(EditMode.EDIT_RIGHT_EDGE);
                                if (!AppManager.itemSelection.isEventContains(selected, item.InternalID)) {
                                    AppManager.itemSelection.clearEvent();
                                }
                                AppManager.itemSelection.addEvent(item.InternalID);
                                this.Cursor = System.Windows.Forms.Cursors.VSplit;
                                refreshScreen();
                                return;
                            }
                        }
                    }
                }

                if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle) {
#if ENABLE_SCRIPT
                    if (selected_tool == EditTool.PALETTE_TOOL) {
                        AppManager.setWholeSelectedIntervalEnabled(false);
                        AppManager.setEditMode(EditMode.NONE);
                        AppManager.itemSelection.clearEvent();
                        AppManager.itemSelection.addEvent(item.InternalID);
                    } else
#endif
                        if (selected_tool != EditTool.ERASER) {
                            mMouseMoveInit = new Point(e.X + stdx, e.Y + stdy);
                            int head_x = AppManager.xCoordFromClocks(item.Clock);
                            mMouseMoveOffset = e.X - head_x;
                            if ((modefier & Keys.Shift) == Keys.Shift) {
                                // シフトキー同時押しによる範囲選択
                                List<int> add_required = new List<int>();
                                add_required.Add(item.InternalID);

                                // 現在の選択アイテムがある場合，
                                // 直前に選択したアイテムと，現在選択しようとしているアイテムとの間にあるアイテムを
                                // 全部選択する
                                SelectedEventEntry sel = AppManager.itemSelection.getLastEvent();
                                if (sel != null) {
                                    int last_id = sel.original.InternalID;
                                    int last_clock = 0;
                                    int this_clock = 0;
                                    bool this_found = false, last_found = false;
                                    for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                                        VsqEvent ev = itr.next();
                                        if (ev.InternalID == last_id) {
                                            last_clock = ev.Clock;
                                            last_found = true;
                                        } else if (ev.InternalID == item.InternalID) {
                                            this_clock = ev.Clock;
                                            this_found = true;
                                        }
                                        if (last_found && this_found) {
                                            break;
                                        }
                                    }
                                    int start = Math.Min(last_clock, this_clock);
                                    int end = Math.Max(last_clock, this_clock);
                                    for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                                        VsqEvent ev = itr.next();
                                        if (start <= ev.Clock && ev.Clock <= end) {
                                            if (!add_required.Contains(ev.InternalID)) {
                                                add_required.Add(ev.InternalID);
                                            }
                                        }
                                    }
                                }
                                AppManager.itemSelection.addEventAll(add_required);
                            } else if ((modefier & s_modifier_key) == s_modifier_key) {
                                // CTRLキーを押しながら選択／選択解除
                                if (AppManager.itemSelection.isEventContains(selected, item.InternalID)) {
                                    AppManager.itemSelection.removeEvent(item.InternalID);
                                } else {
                                    AppManager.itemSelection.addEvent(item.InternalID);
                                }
                            } else {
                                if (!AppManager.itemSelection.isEventContains(selected, item.InternalID)) {
                                    // MouseDownしたアイテムが、まだ選択されていなかった場合。当該アイテム単独に選択しなおす
                                    AppManager.itemSelection.clearEvent();
                                }
                                AppManager.itemSelection.addEvent(item.InternalID);
                            }

                            // 範囲選択モードで、かつマウス位置の音符がその範囲に入っていた場合にのみ、MOVE_ENTRY_WHOLE_WAIT_MOVEに移行
                            if (AppManager.isWholeSelectedIntervalEnabled() &&
                                 AppManager.mWholeSelectedInterval.getStart() <= item.Clock &&
                                 item.Clock <= AppManager.mWholeSelectedInterval.getEnd()) {
                                AppManager.setEditMode(EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE);
                                AppManager.mWholeSelectedIntervalStartForMoving = AppManager.mWholeSelectedInterval.getStart();
                            } else {
                                AppManager.setWholeSelectedIntervalEnabled(false);
                                AppManager.setEditMode(EditMode.MOVE_ENTRY_WAIT_MOVE);
                            }

                            this.Cursor = Cursors.Hand;
#if DEBUG
                            AppManager.debugWriteLine("    EditMode=" + AppManager.getEditMode());
                            AppManager.debugWriteLine("    m_config.SelectedEvent.Count=" + AppManager.itemSelection.getEventCount());
#endif
                        }
                }
                #endregion
            }
            refreshScreen();
        }

        public void pictPianoRoll_MouseMove(Object sender, MouseEventArgs e)
        {
            lock (AppManager.mDrawObjects) {
                if (mFormActivated) {
#if ENABLE_PROPERTY
                    if (AppManager.mInputTextBox != null && !AppManager.mInputTextBox.IsDisposed && !AppManager.mInputTextBox.Visible && !AppManager.propertyPanel.isEditing()) {
#else
                    if (AppManager.mInputTextBox != null && !AppManager.mInputTextBox.IsDisposed && !AppManager.mInputTextBox.Visible) {
#endif
                        focusPianoRoll();
                    }
                }

                EditMode edit_mode = AppManager.getEditMode();
                int stdx = controller.getStartToDrawX();
                int stdy = controller.getStartToDrawY();
                int selected = AppManager.getSelected();
                EditTool selected_tool = AppManager.getSelectedTool();

                if (edit_mode == EditMode.CURVE_ON_PIANOROLL && AppManager.mCurveOnPianoroll) {
                    pictPianoRoll.mMouseTracer.append(e.X + stdx, e.Y + stdy);
                    if (!timer.Enabled) {
                        refreshScreen();
                    }
                    return;
                }

                if (!mMouseMoved && edit_mode == EditMode.MIDDLE_DRAG) {
                    this.Cursor = HAND;
                }

                if (e.X != mButtonInitial.x || e.Y != mButtonInitial.y) {
                    mMouseMoved = true;
                }
                if (!(edit_mode == EditMode.MIDDLE_DRAG) && AppManager.isPlaying()) {
                    return;
                }

                if (edit_mode == EditMode.MOVE_ENTRY_WAIT_MOVE ||
                     edit_mode == EditMode.MOVE_ENTRY_WHOLE_WAIT_MOVE) {
                    int x = e.X + stdx;
                    int y = e.Y + stdy;
                    if (mMouseMoveInit.x != x || mMouseMoveInit.y != y) {
                        if (edit_mode == EditMode.MOVE_ENTRY_WAIT_MOVE) {
                            AppManager.setEditMode(EditMode.MOVE_ENTRY);
                            edit_mode = EditMode.MOVE_ENTRY;
                        } else {
                            AppManager.setEditMode(EditMode.MOVE_ENTRY_WHOLE);
                            edit_mode = EditMode.MOVE_ENTRY_WHOLE;
                        }
                    }
                }

#if ENABLE_MOUSEHOVER
                if (mMouseMoved && mMouseHoverThread != null) {
                    mMouseHoverThread.Abort();
                }
#endif

                int clock = AppManager.clockFromXCoord(e.X);
                if (mMouseDowned) {
                    if (mExtDragX == ExtDragXMode.NONE) {
                        if (AppManager.keyWidth > e.X) {
                            mExtDragX = ExtDragXMode.LEFT;
                        } else if (pictPianoRoll.getWidth() < e.X) {
                            mExtDragX = ExtDragXMode.RIGHT;
                        }
                    } else {
                        if (AppManager.keyWidth <= e.X && e.X <= pictPianoRoll.getWidth()) {
                            mExtDragX = ExtDragXMode.NONE;
                        }
                    }

                    if (mExtDragY == ExtDragYMode.NONE) {
                        if (0 > e.Y) {
                            mExtDragY = ExtDragYMode.UP;
                        } else if (pictPianoRoll.getHeight() < e.Y) {
                            mExtDragY = ExtDragYMode.DOWN;
                        }
                    } else {
                        if (0 <= e.Y && e.Y <= pictPianoRoll.getHeight()) {
                            mExtDragY = ExtDragYMode.NONE;
                        }
                    }
                } else {
                    mExtDragX = ExtDragXMode.NONE;
                    mExtDragY = ExtDragYMode.NONE;
                }

                if (edit_mode == EditMode.MIDDLE_DRAG) {
                    mExtDragX = ExtDragXMode.NONE;
                    mExtDragY = ExtDragYMode.NONE;
                }

                double now = 0, dt = 0;
                if (mExtDragX != ExtDragXMode.NONE || mExtDragY != ExtDragYMode.NONE) {
                    now = PortUtil.getCurrentTime();
                    dt = now - mTimerDragLastIgnitted;
                }
                if (mExtDragX == ExtDragXMode.RIGHT || mExtDragX == ExtDragXMode.LEFT) {
                    int px_move = AppManager.editorConfig.MouseDragIncrement;
                    if (px_move / dt > AppManager.editorConfig.MouseDragMaximumRate) {
                        px_move = (int)(dt * AppManager.editorConfig.MouseDragMaximumRate);
                    }
                    double d_draft;
                    if (mExtDragX == ExtDragXMode.LEFT) {
                        px_move *= -1;
                    }
                    int left_clock = AppManager.clockFromXCoord(AppManager.keyWidth);
                    float inv_scale_x = controller.getScaleXInv();
                    int dclock = (int)(px_move * inv_scale_x);
                    d_draft = 5 * inv_scale_x + left_clock + dclock;
                    if (d_draft < 0.0) {
                        d_draft = 0.0;
                    }
                    int draft = (int)d_draft;
                    if (hScroll.Maximum < draft) {
                        if (edit_mode == EditMode.ADD_ENTRY ||
                             edit_mode == EditMode.MOVE_ENTRY ||
                             edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY ||
                             edit_mode == EditMode.DRAG_DROP) {
                            hScroll.Maximum = draft;
                        } else {
                            draft = hScroll.Maximum;
                        }
                    }
                    if (draft < hScroll.Minimum) {
                        draft = hScroll.Minimum;
                    }
                    hScroll.Value = draft;
                }
                if (mExtDragY == ExtDragYMode.UP || mExtDragY == ExtDragYMode.DOWN) {
                    int min = vScroll.Minimum;
                    int max = vScroll.Maximum - vScroll.LargeChange;
                    int px_move = AppManager.editorConfig.MouseDragIncrement;
                    if (px_move / dt > AppManager.editorConfig.MouseDragMaximumRate) {
                        px_move = (int)(dt * AppManager.editorConfig.MouseDragMaximumRate);
                    }
                    px_move += 50;
                    if (mExtDragY == ExtDragYMode.UP) {
                        px_move *= -1;
                    }
                    int draft = vScroll.Value + px_move;
                    if (draft < 0) {
                        draft = 0;
                    }
                    int df = (int)draft;
                    if (df < min) {
                        df = min;
                    } else if (max < df) {
                        df = max;
                    }
                    vScroll.Value = df;
                }
                if (mExtDragX != ExtDragXMode.NONE || mExtDragY != ExtDragYMode.NONE) {
                    mTimerDragLastIgnitted = now;
                }

                // 選択範囲にあるイベントを選択．
                if (AppManager.mIsPointerDowned) {
                    if (AppManager.isWholeSelectedIntervalEnabled()) {
                        int endClock = AppManager.clockFromXCoord(e.X);
                        if (AppManager.editorConfig.CurveSelectingQuantized) {
                            int unit = AppManager.getPositionQuantizeClock();
                            endClock = doQuantize(endClock, unit);
                        }
                        AppManager.mWholeSelectedInterval.setEnd(endClock);
                    } else {
                        Point mouse = new Point(e.X + stdx, e.Y + stdy);
                        int tx, ty, twidth, theight;
                        int lx = AppManager.mMouseDownLocation.x;
                        if (lx < mouse.x) {
                            tx = lx;
                            twidth = mouse.x - lx;
                        } else {
                            tx = mouse.x;
                            twidth = lx - mouse.x;
                        }
                        int ly = AppManager.mMouseDownLocation.y;
                        if (ly < mouse.y) {
                            ty = ly;
                            theight = mouse.y - ly;
                        } else {
                            ty = mouse.y;
                            theight = ly - mouse.y;
                        }

                        Rectangle rect = new Rectangle(tx, ty, twidth, theight);
                        List<int> add_required = new List<int>();
                        int internal_id = -1;
                        foreach (var dobj in AppManager.mDrawObjects[selected - 1]) {
                            int x0 = dobj.mRectangleInPixel.x + AppManager.keyWidth;
                            int x1 = dobj.mRectangleInPixel.x + AppManager.keyWidth + dobj.mRectangleInPixel.width;
                            int y0 = dobj.mRectangleInPixel.y;
                            int y1 = dobj.mRectangleInPixel.y + dobj.mRectangleInPixel.height;
                            internal_id = dobj.mInternalID;
                            if (x1 < tx) {
                                continue;
                            }
                            if (tx + twidth < x0) {
                                break;
                            }
                            bool found = Utility.isInRect(new Point(x0, y0), rect) |
                                            Utility.isInRect(new Point(x0, y1), rect) |
                                            Utility.isInRect(new Point(x1, y0), rect) |
                                            Utility.isInRect(new Point(x1, y1), rect);
                            if (found) {
                                add_required.Add(internal_id);
                            } else {
                                if (x0 <= tx && tx + twidth <= x1) {
                                    if (ty < y0) {
                                        if (y0 <= ty + theight) {
                                            add_required.Add(internal_id);
                                        }
                                    } else if (y0 <= ty && ty < y1) {
                                        add_required.Add(internal_id);
                                    }
                                } else if (y0 <= ty && ty + theight <= y1) {
                                    if (tx < x0) {
                                        if (x0 <= tx + twidth) {
                                            add_required.Add(internal_id);
                                        }
                                    } else if (x0 <= tx && tx < x1) {
                                        add_required.Add(internal_id);
                                    }
                                }
                            }
                        }
                        List<int> remove_required = new List<int>();
                        foreach (var selectedEvent in AppManager.itemSelection.getEventIterator()) {
                            if (!add_required.Contains(selectedEvent.original.InternalID)) {
                                remove_required.Add(selectedEvent.original.InternalID);
                            }
                        }
                        if (remove_required.Count > 0) {
                            AppManager.itemSelection.removeEventRange(PortUtil.convertIntArray(remove_required.ToArray()));
                        }
                        add_required.RemoveAll((id) => AppManager.itemSelection.isEventContains(selected, id));
                        AppManager.itemSelection.addEventAll(add_required);
                    }
                }

                if (edit_mode == EditMode.MIDDLE_DRAG) {
                    #region MiddleDrag
                    int drafth = computeHScrollValueForMiddleDrag(e.X);
                    int draftv = computeVScrollValueForMiddleDrag(e.Y);
                    bool moved = false;
                    if (drafth != hScroll.Value) {
                        //moved = true;
                        //hScroll.beQuiet();
                        hScroll.Value = drafth;
                    }
                    if (draftv != vScroll.Value) {
                        //moved = true;
                        //vScroll.beQuiet();
                        vScroll.Value = draftv;
                    }
                    //if ( moved ) {
                    //    vScroll.setQuiet( false );
                    //    hScroll.setQuiet( false );
                    //    refreshScreen( true );
                    //}
                    refreshScreen(true);
                    if (AppManager.isPlaying()) {
                        return;
                    }
                    #endregion
                    return;
                } else if (edit_mode == EditMode.ADD_ENTRY) {
                    #region ADD_ENTRY
                    int unit = AppManager.getLengthQuantizeClock();
                    int length = clock - AppManager.mAddingEvent.Clock;
                    int odd = length % unit;
                    int new_length = length - odd;

                    if (unit * controller.getScaleX() > 10) { //これをしないと、グリッド2個分増えることがある
                        int next_clock = AppManager.clockFromXCoord(e.X + 10);
                        int next_length = next_clock - AppManager.mAddingEvent.Clock;
                        int next_new_length = next_length - (next_length % unit);
                        if (next_new_length == new_length + unit) {
                            new_length = next_new_length;
                        }
                    }

                    if (new_length <= 0) {
                        new_length = 0;
                    }
                    AppManager.mAddingEvent.ID.setLength(new_length);
                    #endregion
                } else if (edit_mode == EditMode.MOVE_ENTRY || edit_mode == EditMode.MOVE_ENTRY_WHOLE) {
                    #region MOVE_ENTRY, MOVE_ENTRY_WHOLE
                    if (AppManager.itemSelection.getEventCount() > 0) {
                        VsqEvent original = AppManager.itemSelection.getLastEvent().original;
                        int note = AppManager.noteFromYCoord(e.Y);                           // 現在のマウス位置でのnote
                        int note_init = original.ID.Note;
                        int dnote = (edit_mode == EditMode.MOVE_ENTRY) ? note - note_init : 0;

                        int tclock = AppManager.clockFromXCoord(e.X - mMouseMoveOffset);
                        int clock_init = original.Clock;

                        int dclock = tclock - clock_init;

                        if (AppManager.editorConfig.getPositionQuantize() != QuantizeMode.off) {
                            int unit = AppManager.getPositionQuantizeClock();
                            int new_clock = doQuantize(original.Clock + dclock, unit);
                            dclock = new_clock - clock_init;
                        }

                        AppManager.mWholeSelectedIntervalStartForMoving = AppManager.mWholeSelectedInterval.getStart() + dclock;

                        foreach (var item in AppManager.itemSelection.getEventIterator()) {
                            int new_clock = item.original.Clock + dclock;
                            int new_note = item.original.ID.Note + dnote;
                            item.editing.Clock = new_clock;
                            item.editing.ID.Note = new_note;
                        }
                    }
                    #endregion
                } else if (edit_mode == EditMode.EDIT_LEFT_EDGE) {
                    #region EditLeftEdge
                    int unit = AppManager.getLengthQuantizeClock();
                    VsqEvent original = AppManager.itemSelection.getLastEvent().original;
                    int clock_init = original.Clock;
                    int dclock = clock - clock_init;
                    foreach (var item in AppManager.itemSelection.getEventIterator()) {
                        int end_clock = item.original.Clock + item.original.ID.getLength();
                        int new_clock = item.original.Clock + dclock;
                        int new_length = doQuantize(end_clock - new_clock, unit);
                        if (new_length <= 0) {
                            new_length = unit;
                        }
                        item.editing.Clock = end_clock - new_length;
                        if (AppManager.vibratoLengthEditingRule == VibratoLengthEditingRule.PERCENTAGE) {
                            double percentage = item.original.ID.VibratoDelay / (double)item.original.ID.getLength() * 100.0;
                            int newdelay = (int)(new_length * percentage / 100.0);
                            item.editing.ID.VibratoDelay = newdelay;
                        }
                        item.editing.ID.setLength(new_length);
                    }
                    #endregion
                } else if (edit_mode == EditMode.EDIT_RIGHT_EDGE) {
                    #region EditRightEdge
                    int unit = AppManager.getLengthQuantizeClock();

                    VsqEvent original = AppManager.itemSelection.getLastEvent().original;
                    int dlength = clock - (original.Clock + original.ID.getLength());
                    foreach (var item in AppManager.itemSelection.getEventIterator()) {
                        int new_length = doQuantize(item.original.ID.getLength() + dlength, unit);
                        if (new_length <= 0) {
                            new_length = unit;
                        }
                        if (AppManager.vibratoLengthEditingRule == VibratoLengthEditingRule.PERCENTAGE) {
                            double percentage = item.original.ID.VibratoDelay / (double)item.original.ID.getLength() * 100.0;
                            int newdelay = (int)(new_length * percentage / 100.0);
                            item.editing.ID.VibratoDelay = newdelay;
                        }
                        item.editing.ID.setLength(new_length);
#if DEBUG
                        sout.println("FormMain#pictPianoRoll_MouseMove; length(before,after)=(" + item.original.ID.getLength() + "," + item.editing.ID.getLength() + ")");
#endif
                    }
                    #endregion
                } else if (edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY) {
                    #region AddFixedLengthEntry
                    int note = AppManager.noteFromYCoord(e.Y);
                    int unit = AppManager.getPositionQuantizeClock();
                    int new_clock = doQuantize(AppManager.clockFromXCoord(e.X), unit);
                    AppManager.mAddingEvent.ID.Note = note;
                    AppManager.mAddingEvent.Clock = new_clock;
                    #endregion
                } else if (edit_mode == EditMode.EDIT_VIBRATO_DELAY) {
                    #region EditVibratoDelay
                    int new_vibrato_start = clock;
                    int old_vibrato_end = AppManager.mAddingEvent.Clock + AppManager.mAddingEvent.ID.getLength();
                    int new_vibrato_length = old_vibrato_end - new_vibrato_start;
                    int max_length = (int)(AppManager.mAddingEventLength - _PX_ACCENT_HEADER * controller.getScaleXInv());
                    if (max_length < 0) {
                        max_length = 0;
                    }
                    if (new_vibrato_length > max_length) {
                        new_vibrato_start = old_vibrato_end - max_length;
                        new_vibrato_length = max_length;
                    }
                    if (new_vibrato_length < 0) {
                        new_vibrato_start = old_vibrato_end;
                        new_vibrato_length = 0;
                    }
                    AppManager.mAddingEvent.Clock = new_vibrato_start;
                    AppManager.mAddingEvent.ID.setLength(new_vibrato_length);
                    if (!timer.Enabled) {
                        refreshScreen();
                    }
                    #endregion
                    return;
                } else if (edit_mode == EditMode.DRAG_DROP) {
                    #region DRAG_DROP
                    // クオンタイズの処理
                    int unit = AppManager.getPositionQuantizeClock();
                    int clock1 = doQuantize(clock, unit);
                    int note = AppManager.noteFromYCoord(e.Y);
                    AppManager.mAddingEvent.Clock = clock1;
                    AppManager.mAddingEvent.ID.Note = note;
                    #endregion
                }

                // カーソルの形を決める
                if (!mMouseDowned &&
                     edit_mode != EditMode.CURVE_ON_PIANOROLL &&
                     !(AppManager.mCurveOnPianoroll && (selected_tool == EditTool.PENCIL || selected_tool == EditTool.LINE))) {
                    bool split_cursor = false;
                    bool hand_cursor = false;
                    int min_width = 4 * _EDIT_HANDLE_WIDTH;
                    foreach (var dobj in AppManager.mDrawObjects[selected - 1]) {
                        Rectangle rc;
                        if (dobj.mType != DrawObjectType.Dynaff) {
                            int edit_handle_width = _EDIT_HANDLE_WIDTH;
                            if (dobj.mRectangleInPixel.width < min_width) {
                                edit_handle_width = dobj.mRectangleInPixel.width / 4;
                            }

                            // 音符左側の編集領域
                            rc = new Rectangle(
                                                dobj.mRectangleInPixel.x + AppManager.keyWidth - stdx,
                                                dobj.mRectangleInPixel.y - stdy,
                                                edit_handle_width,
                                                (int)(100 * controller.getScaleY()));
                            if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                split_cursor = true;
                                break;
                            }

                            // 音符右側の編集領域
                            rc = new Rectangle(dobj.mRectangleInPixel.x + AppManager.keyWidth + dobj.mRectangleInPixel.width - stdx - edit_handle_width,
                                                dobj.mRectangleInPixel.y - stdy,
                                                edit_handle_width,
                                                (int)(100 * controller.getScaleY()));
                            if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                split_cursor = true;
                                break;
                            }
                        }

                        // 音符本体
                        rc = new Rectangle(dobj.mRectangleInPixel.x + AppManager.keyWidth - stdx,
                                            dobj.mRectangleInPixel.y - stdy,
                                            dobj.mRectangleInPixel.width,
                                            dobj.mRectangleInPixel.height);
                        if (dobj.mType == DrawObjectType.Note) {
                            if (AppManager.editorConfig.ShowExpLine && !dobj.mIsOverlapped) {
                                rc.height *= 2;
                                if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                    // ビブラートの開始位置
                                    rc = new Rectangle(dobj.mRectangleInPixel.x + AppManager.keyWidth + dobj.mVibratoDelayInPixel - stdx - _EDIT_HANDLE_WIDTH / 2,
                                                        dobj.mRectangleInPixel.y + (int)(100 * controller.getScaleY()) - stdy,
                                                        _EDIT_HANDLE_WIDTH,
                                                        (int)(100 * controller.getScaleY()));
                                    if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                        split_cursor = true;
                                        break;
                                    } else {
                                        hand_cursor = true;
                                        break;
                                    }
                                }
                            } else {
                                if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                    hand_cursor = true;
                                    break;
                                }
                            }
                        } else {
                            if (Utility.isInRect(new Point(e.X, e.Y), rc)) {
                                hand_cursor = true;
                                break;
                            }
                        }
                    }

                    if (split_cursor) {
                        Cursor = System.Windows.Forms.Cursors.VSplit;
                    } else if (hand_cursor) {
                        this.Cursor = Cursors.Hand;
                    } else {
                        this.Cursor = Cursors.Default;
                    }
                }
                if (!timer.Enabled) {
                    refreshScreen(true);
                }
            }
        }

        /// <summary>
        /// ピアノロールからマウスボタンが離れたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void pictPianoRoll_MouseUp(Object sender, MouseEventArgs e)
        {
#if DEBUG
            AppManager.debugWriteLine("pictureBox1_MouseUp");
            AppManager.debugWriteLine("    m_config.EditMode=" + AppManager.getEditMode());
#endif
            AppManager.mIsPointerDowned = false;
            mMouseDowned = false;

            Keys modefiers = Control.ModifierKeys;

            EditMode edit_mode = AppManager.getEditMode();
            VsqFileEx vsq = AppManager.getVsqFile();
            int selected = AppManager.getSelected();
            VsqTrack vsq_track = vsq.Track[selected];
            CurveType selected_curve = trackSelector.getSelectedCurve();
            int stdx = controller.getStartToDrawX();
            int stdy = controller.getStartToDrawY();
            double d2_13 = 8192; // = 2^13
            int track_height = (int)(100 * controller.getScaleY());
            int half_track_height = track_height / 2;

            if (edit_mode == EditMode.CURVE_ON_PIANOROLL) {
                if (pictPianoRoll.mMouseTracer.size() > 1) {
                    // マウスの軌跡の左右端(px)
                    int px_start = pictPianoRoll.mMouseTracer.firstKey();
                    int px_end = pictPianoRoll.mMouseTracer.lastKey();

                    // マウスの軌跡の左右端(クロック)
                    int cl_start = AppManager.clockFromXCoord(px_start - stdx);
                    int cl_end = AppManager.clockFromXCoord(px_end - stdx);

                    // 編集が行われたかどうか
                    bool edited = false;
                    // 作業用のPITカーブのコピー
                    VsqBPList pit = (VsqBPList)vsq_track.getCurve("pit").clone();
                    VsqBPList pbs = (VsqBPList)vsq_track.getCurve("pbs"); // こっちはクローンしないよ

                    // トラック内の全音符に対して、マウス軌跡と被っている部分のPITを編集する
                    foreach (var item in vsq_track.getNoteEventIterator()) {
                        int cl_item_start = item.Clock;
                        if (cl_end < cl_item_start) {
                            break;
                        }
                        int cl_item_end = cl_item_start + item.ID.getLength();
                        if (cl_item_end < cl_start) {
                            continue;
                        }

                        // ここに到達するってことは、pitに編集が加えられるってこと。
                        edited = true;

                        // マウス軌跡と被っている部分のPITを削除
                        int cl_remove_start = Math.Max(cl_item_start, cl_start);
                        int cl_remove_end = Math.Min(cl_item_end, cl_end);
                        int value_at_remove_end = pit.getValue(cl_remove_end);
                        int value_at_remove_start = pit.getValue(cl_remove_start);
                        List<int> remove = new List<int>();
                        foreach (var clock in pit.keyClockIterator()) {
                            if (cl_remove_start <= clock && clock <= cl_remove_end) {
                                remove.Add(clock);
                            }
                        }
                        foreach (var clock in remove) {
                            pit.remove(clock);
                        }
                        remove = null;

                        int px_item_start = AppManager.xCoordFromClocks(cl_item_start) + stdx;
                        int px_item_end = AppManager.xCoordFromClocks(cl_item_end) + stdx;

                        int lastv = value_at_remove_start;
                        bool cl_item_end_added = false;
                        bool cl_item_start_added = false;
                        int last_px = 0, last_py = 0;
                        foreach (var p in pictPianoRoll.mMouseTracer.iterator()) {
                            if (p.x < px_item_start) {
                                last_px = p.x;
                                last_py = p.y;
                                continue;
                            }
                            if (px_item_end < p.x) {
                                break;
                            }

                            int clock = AppManager.clockFromXCoord(p.x - stdx);
                            if (clock < cl_item_start) {
                                last_px = p.x;
                                last_py = p.y;
                                continue;
                            } else if (cl_item_end < clock) {
                                break;
                            }
                            double note = AppManager.noteFromYCoordDoublePrecision(p.y - stdy - half_track_height);
                            int v_pit = (int)(d2_13 / (double)pbs.getValue(clock) * (note - item.ID.Note));

                            // 正規化
                            if (v_pit < pit.getMinimum()) {
                                v_pit = pit.getMinimum();
                            } else if (pit.getMaximum() < v_pit) {
                                v_pit = pit.getMaximum();
                            }

                            if (cl_item_start < clock && !cl_item_start_added &&
                                 cl_start <= cl_item_start && cl_item_start < cl_end) {
                                // これから追加しようとしているデータ点の時刻が、音符の開始時刻よりも後なんだけれど、
                                // 音符の開始時刻におけるデータをまだ書き込んでない場合
                                double a = (p.y - last_py) / (double)(p.x - last_px);
                                double x_at_clock = AppManager.xCoordFromClocks(cl_item_start) + stdx;
                                double ext_y = last_py + a * (x_at_clock - last_px);
                                double tnote = AppManager.noteFromYCoordDoublePrecision((int)(ext_y - stdy - half_track_height));
                                int t_vpit = (int)(d2_13 / (double)pbs.getValue(cl_item_start) * (tnote - item.ID.Note));
                                pit.add(cl_item_start, t_vpit);
                                lastv = t_vpit;
                                cl_item_start_added = true;
                            }

                            // 直前の値と違っている場合にのみ追加
                            if (v_pit != lastv) {
                                pit.add(clock, v_pit);
                                lastv = v_pit;
                                if (clock == cl_item_end) {
                                    cl_item_end_added = true;
                                } else if (clock == cl_item_start) {
                                    cl_item_start_added = true;
                                }
                            }
                        }

                        if (!cl_item_end_added &&
                             cl_start <= cl_item_end && cl_item_end <= cl_end) {
                            pit.add(cl_item_end, lastv);
                        }

                        pit.add(cl_remove_end, value_at_remove_end);
                    }

                    // 編集操作が行われた場合のみ、コマンドを発行
                    if (edited) {
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandTrackCurveReplace(selected, "PIT", pit));
                        AppManager.editHistory.register(vsq.executeCommand(run));
                        setEdited(true);
                    }
                }
                pictPianoRoll.mMouseTracer.clear();
                AppManager.setEditMode(EditMode.NONE);
                return;
            }

            if (edit_mode == EditMode.MIDDLE_DRAG) {
                this.Cursor = Cursors.Default;
            } else if (edit_mode == EditMode.ADD_ENTRY || edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY) {
                #region AddEntry || AddFixedLengthEntry
                if (AppManager.getSelected() >= 0) {
                    if ((edit_mode == EditMode.ADD_FIXED_LENGTH_ENTRY) ||
                         (edit_mode == EditMode.ADD_ENTRY && (mButtonInitial.x != e.X || mButtonInitial.y != e.Y) && AppManager.mAddingEvent.ID.getLength() > 0)) {
                        if (AppManager.mAddingEvent.Clock < vsq.getPreMeasureClocks()) {
                            SystemSounds.Asterisk.Play();
                        } else {
                            fixAddingEvent();
                        }
                    }
                }
                #endregion
            } else if (edit_mode == EditMode.MOVE_ENTRY) {
                #region MoveEntry
#if DEBUG
                sout.println("FormMain#pictPianoRoll_MouseUp; edit_mode is MOVE_ENTRY");
#endif
                if (AppManager.itemSelection.getEventCount() > 0) {
                    SelectedEventEntry last_selected_event = AppManager.itemSelection.getLastEvent();
#if DEBUG
                    sout.println("FormMain#pictPianoRoll_MouseUp; last_selected_event.original.InternalID=" + last_selected_event.original.InternalID);
#endif
                    VsqEvent original = last_selected_event.original;
                    if (original.Clock != last_selected_event.editing.Clock ||
                         original.ID.Note != last_selected_event.editing.ID.Note) {
                        bool out_of_range = false; // プリメジャーにめり込んでないかどうか
                        bool contains_dynamics = false; // Dynaff, Crescend, Desrecendが含まれているかどうか
                        VsqTrack copied = (VsqTrack)vsq_track.clone();
                        int clockAtPremeasure = vsq.getPreMeasureClocks();
                        foreach (var ev in AppManager.itemSelection.getEventIterator()) {
                            int internal_id = ev.original.InternalID;
                            if (ev.editing.Clock < clockAtPremeasure) {
                                out_of_range = true;
                                break;
                            }
                            if (ev.editing.ID.Note < 0 || 128 < ev.editing.ID.Note) {
                                out_of_range = true;
                                break;
                            }
                            for (Iterator<VsqEvent> itr2 = copied.getEventIterator(); itr2.hasNext(); ) {
                                VsqEvent item = itr2.next();
                                if (item.InternalID == internal_id) {
                                    item.Clock = ev.editing.Clock;
                                    item.ID = (VsqID)ev.editing.ID.clone();
                                    break;
                                }
                            }
                            if (ev.original.ID.type == VsqIDType.Aicon) {
                                contains_dynamics = true;
                            }
                        }
                        if (out_of_range) {
                            SystemSounds.Asterisk.Play();
                        } else {
                            if (contains_dynamics) {
                                copied.reflectDynamics();
                            }
                            CadenciiCommand run = VsqFileEx.generateCommandTrackReplace(selected,
                                                                                         copied,
                                                                                         vsq.AttachedCurves.get(selected - 1));
                            AppManager.editHistory.register(vsq.executeCommand(run));
                            AppManager.itemSelection.updateSelectedEventInstance();
                            setEdited(true);
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
                    lock (AppManager.mDrawObjects) {
                        AppManager.mDrawObjects[selected - 1].Sort();
                    }
                }
                #endregion
            } else if (edit_mode == EditMode.EDIT_LEFT_EDGE || edit_mode == EditMode.EDIT_RIGHT_EDGE) {
                #region EDIT_LEFT_EDGE | EDIT_RIGHT_EDGE
                if (mMouseMoved) {
                    VsqEvent original = AppManager.itemSelection.getLastEvent().original;
                    int count = AppManager.itemSelection.getEventCount();
                    int[] ids = new int[count];
                    int[] clocks = new int[count];
                    VsqID[] values = new VsqID[count];
                    int i = -1;
                    bool contains_aicon = false; // dynaff, crescend, decrescendが含まれていればtrue
                    foreach (var ev in AppManager.itemSelection.getEventIterator()) {
                        if (ev.original.ID.type == VsqIDType.Aicon) {
                            contains_aicon = true;
                        }
                        i++;

                        Utility.editLengthOfVsqEvent(ev.editing, ev.editing.ID.getLength(), AppManager.vibratoLengthEditingRule);
                        ids[i] = ev.original.InternalID;
                        clocks[i] = ev.editing.Clock;
                        values[i] = ev.editing.ID;
                    }

                    CadenciiCommand run = null;
                    if (contains_aicon) {
                        VsqFileEx copied_vsq = (VsqFileEx)vsq.clone();
                        VsqCommand vsq_command =
                            VsqCommand.generateCommandEventChangeClockAndIDContaintsRange(selected,
                                                                                           ids,
                                                                                           clocks,
                                                                                           values);
                        copied_vsq.executeCommand(vsq_command);
                        VsqTrack copied = (VsqTrack)copied_vsq.Track[selected].clone();
                        copied.reflectDynamics();
                        run = VsqFileEx.generateCommandTrackReplace(selected,
                                                                     copied,
                                                                     vsq.AttachedCurves.get(selected - 1));
                    } else {
                        run = new CadenciiCommand(
                            VsqCommand.generateCommandEventChangeClockAndIDContaintsRange(selected,
                                                                                 ids,
                                                                                 clocks,
                                                                                 values));
                    }
                    AppManager.editHistory.register(vsq.executeCommand(run));
                    setEdited(true);
                }
                #endregion
            } else if (edit_mode == EditMode.EDIT_VIBRATO_DELAY) {
                #region EditVibratoDelay
                if (mMouseMoved) {
                    double max_length = AppManager.mAddingEventLength - _PX_ACCENT_HEADER * controller.getScaleXInv();
                    double rate = AppManager.mAddingEvent.ID.getLength() / max_length;
                    if (rate > 0.99) {
                        rate = 1.0;
                    }
                    int vibrato_length = (int)(AppManager.mAddingEventLength * rate);
                    VsqEvent item = null;
                    foreach (var ve in vsq_track.getNoteEventIterator()) {
                        if (ve.InternalID == mVibratoEditingId) {
                            item = (VsqEvent)ve.clone();
                            break;
                        }
                    }
                    if (item != null) {
                        if (vibrato_length <= 0) {
                            item.ID.VibratoHandle = null;
                            item.ID.VibratoDelay = item.ID.getLength();
                        } else {
                            item.ID.VibratoHandle.setLength(vibrato_length);
                            item.ID.VibratoDelay = item.ID.getLength() - vibrato_length;
                        }
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandEventChangeIDContaints(selected, mVibratoEditingId, item.ID));
                        AppManager.editHistory.register(vsq.executeCommand(run));
                        setEdited(true);
                    }
                }
                #endregion
            } else if (edit_mode == EditMode.MOVE_ENTRY_WHOLE) {
#if DEBUG
                sout.println("FormMain#pictPianoRoll_MouseUp; EditMode.MOVE_ENTRY_WHOLE");
#endif
                #region MOVE_ENTRY_WHOLE
                int src_clock_start = AppManager.mWholeSelectedInterval.getStart();
                int src_clock_end = AppManager.mWholeSelectedInterval.getEnd();
                int dst_clock_start = AppManager.mWholeSelectedIntervalStartForMoving;
                int dst_clock_end = dst_clock_start + (src_clock_end - src_clock_start);
                int dclock = dst_clock_start - src_clock_start;

                int num = AppManager.itemSelection.getEventCount();
                int[] selected_ids = new int[num]; // 後段での再選択用のInternalIDのリスト
                int last_selected_id = AppManager.itemSelection.getLastEvent().original.InternalID;

                // 音符イベントを移動
                VsqTrack work = (VsqTrack)vsq_track.clone();
                int k = 0;
                foreach (var item in AppManager.itemSelection.getEventIterator()) {
                    int internal_id = item.original.InternalID;
                    selected_ids[k] = internal_id;
                    k++;
#if DEBUG
                    sout.println("FormMain#pictPianoRoll_MouseUp; internal_id=" + internal_id);
#endif
                    foreach (var vsq_event in work.getNoteEventIterator()) {
                        if (internal_id == vsq_event.InternalID) {
#if DEBUG
                            sout.println("FormMain#pictPianoRoll_MouseUp; before: clock=" + vsq_event.Clock + "; after: clock=" + item.editing.Clock);
#endif
                            vsq_event.Clock = item.editing.Clock;
                            break;
                        }
                    }
                }

                // 全てのコントロールカーブのデータ点を移動
                for (int i = 0; i < Utility.CURVE_USAGE.Length; i++) {
                    CurveType curve_type = Utility.CURVE_USAGE[i];
                    VsqBPList bplist = work.getCurve(curve_type.getName());
                    if (bplist == null) {
                        continue;
                    }

                    // src_clock_startからsrc_clock_endの範囲にあるデータ点をコピー＆削除
                    VsqBPList copied = new VsqBPList(bplist.getName(), bplist.getDefault(), bplist.getMinimum(), bplist.getMaximum());
                    int size = bplist.size();
                    for (int j = size - 1; j >= 0; j--) {
                        int clock = bplist.getKeyClock(j);
                        if (src_clock_start <= clock && clock <= src_clock_end) {
                            VsqBPPair bppair = bplist.getElementB(j);
                            copied.add(clock, bppair.value);
                            bplist.removeElementAt(j);
                        }
                    }

                    // dst_clock_startからdst_clock_endの範囲にあるコントロールカーブのデータ点をすべて削除
                    size = bplist.size();
                    for (int j = size - 1; j >= 0; j--) {
                        int clock = bplist.getKeyClock(j);
                        if (dst_clock_start <= clock && clock <= dst_clock_end) {
                            bplist.removeElementAt(j);
                        }
                    }

                    // コピーしたデータを、クロックをずらしながら追加
                    size = copied.size();
                    for (int j = 0; j < size; j++) {
                        int clock = copied.getKeyClock(j);
                        VsqBPPair bppair = copied.getElementB(j);
                        bplist.add(clock + dclock, bppair.value);
                    }
                }

                // コマンドを作成＆実行
                CadenciiCommand run = VsqFileEx.generateCommandTrackReplace(selected,
                                                                             work,
                                                                             vsq.AttachedCurves.get(selected - 1));
                AppManager.editHistory.register(vsq.executeCommand(run));

                // 選択範囲を更新
                AppManager.mWholeSelectedInterval = new SelectedRegion(dst_clock_start);
                AppManager.mWholeSelectedInterval.setEnd(dst_clock_end);
                AppManager.mWholeSelectedIntervalStartForMoving = dst_clock_start;

                // 音符の再選択
                AppManager.itemSelection.clearEvent();
                List<int> list_selected_ids = new List<int>();
                for (int i = 0; i < num; i++) {
                    list_selected_ids.Add(selected_ids[i]);
                }
                AppManager.itemSelection.addEventAll(list_selected_ids);
                AppManager.itemSelection.addEvent(last_selected_id);

                setEdited(true);
                #endregion
            } else if (AppManager.isWholeSelectedIntervalEnabled()) {
                int start = AppManager.mWholeSelectedInterval.getStart();
                int end = AppManager.mWholeSelectedInterval.getEnd();
#if DEBUG
                sout.println("FormMain#pictPianoRoll_MouseUp; WholeSelectedInterval; (start,end)=" + start + ", " + end);
#endif
                AppManager.itemSelection.clearEvent();

                // 音符の選択状態を更新
                List<int> add_required_event = new List<int>();
                for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                    VsqEvent ve = itr.next();
                    if (start <= ve.Clock && ve.Clock + ve.ID.getLength() <= end) {
                        add_required_event.Add(ve.InternalID);
                    }
                }
                AppManager.itemSelection.addEventAll(add_required_event);

                // コントロールカーブ点の選択状態を更新
                List<long> add_required_point = new List<long>();
                VsqBPList list = vsq_track.getCurve(selected_curve.getName());
                if (list != null) {
                    int count = list.size();
                    for (int i = 0; i < count; i++) {
                        int clock = list.getKeyClock(i);
                        if (clock < start) {
                            continue;
                        } else if (end < clock) {
                            break;
                        } else {
                            VsqBPPair v = list.getElementB(i);
                            add_required_point.Add(v.id);
                        }
                    }
                }
                if (add_required_point.Count > 0) {
                    AppManager.itemSelection.addPointAll(selected_curve,
                                                    PortUtil.convertLongArray(add_required_point.ToArray()));
                }
            }
        heaven:
            AppManager.setEditMode(EditMode.NONE);
            refreshScreen(true);
        }

        public void pictPianoRoll_MouseWheel(Object sender, MouseEventArgs e)
        {
            Keys modifier = Control.ModifierKeys;
            bool horizontal = (modifier & Keys.Shift) == Keys.Shift;
            if (AppManager.editorConfig.ScrollHorizontalOnWheel) {
                horizontal = !horizontal;
            }
            if ((modifier & Keys.Control) == Keys.Control) {
                // ピアノロール拡大率を変更
                if (horizontal) {
                    int max = trackBar.Maximum;
                    int min = trackBar.Minimum;
                    int width = max - min;
                    int delta = (width / 10) * (e.Delta > 0 ? 1 : -1);
                    int old_tbv = trackBar.Value;
                    int draft = old_tbv + delta;
                    if (draft < min) {
                        draft = min;
                    }
                    if (max < draft) {
                        draft = max;
                    }
                    if (old_tbv != draft) {

                        // マウス位置を中心に拡大されるようにしたいので．
                        // マウスのスクリーン座標
                        Point screen_p_at_mouse = PortUtil.getMousePosition();
                        // ピアノロール上でのマウスのx座標
                        int x_at_mouse = pictPianoRoll.PointToClient(new System.Drawing.Point(screen_p_at_mouse.x, screen_p_at_mouse.y)).X;
                        // マウス位置でのクロック -> こいつが保存される
                        int clock_at_mouse = AppManager.clockFromXCoord(x_at_mouse);
                        // 古い拡大率
                        float scale0 = controller.getScaleX();
                        // 新しい拡大率
                        float scale1 = getScaleXFromTrackBarValue(draft);
                        // 古いstdx
                        int stdx0 = controller.getStartToDrawX();
                        int stdx1 = (int)(clock_at_mouse * (scale1 - scale0) + stdx0);
                        // 新しいhScroll.Value
                        int hscroll_value = (int)(stdx1 / scale1);
                        if (hscroll_value < hScroll.Minimum) {
                            hscroll_value = hScroll.Minimum;
                        }
                        if (hScroll.Maximum < hscroll_value) {
                            hscroll_value = hScroll.Maximum;
                        }

                        controller.setScaleX(scale1);
                        controller.setStartToDrawX(stdx1);
                        hScroll.Value = hscroll_value;
                        trackBar.Value = draft;
                    }
                } else {
                    zoomY(e.Delta > 0 ? 1 : -1);
                }
            } else {
                // スクロール操作
                if (e.X <= AppManager.keyWidth || pictPianoRoll.getWidth() < e.X) {
                    horizontal = false;
                }
                if (horizontal) {
                    hScroll.Value = computeScrollValueFromWheelDelta(e.Delta);
                } else {
                    double new_val = (double)vScroll.Value - e.Delta * 10;
                    int min = vScroll.Minimum;
                    int max = vScroll.Maximum - vScroll.LargeChange;
                    if (new_val > max) {
                        vScroll.Value = max;
                    } else if (new_val < min) {
                        vScroll.Value = min;
                    } else {
                        vScroll.Value = (int)new_val;
                    }
                }
            }
            refreshScreen();
        }

        public void pictPianoRoll_PreviewKeyDown(Object sender, PreviewKeyDownEventArgs e)
        {
            KeyEventArgs e0 = new KeyEventArgs(e.KeyData);
            processSpecialShortcutKey(e0, true);
        }
        #endregion

        //BOOKMARK: iconPalette
        #region iconPalette
        public void iconPalette_LocationChanged(Object sender, EventArgs e)
        {
            var point = AppManager.iconPalette.Location;
            AppManager.editorConfig.FormIconPaletteLocation = new XmlPoint(point.X, point.Y);
        }

        public void iconPalette_FormClosing(Object sender, FormClosingEventArgs e)
        {
            flipIconPaletteVisible(AppManager.iconPalette.Visible);
        }
        #endregion

        //BOOKMARK: menuVisual
        #region menuVisual*
        public void menuVisualProperty_CheckedChanged(Object sender, EventArgs e)
        {
#if ENABLE_PROPERTY
            if (menuVisualProperty.Checked) {
                if (AppManager.editorConfig.PropertyWindowStatus.WindowState == FormWindowState.Minimized) {
                    updatePropertyPanelState(PanelState.Docked);
                } else {
                    updatePropertyPanelState(PanelState.Window);
                }
            } else {
                updatePropertyPanelState(PanelState.Hidden);
            }
#endif
        }

        public void menuVisualOverview_CheckedChanged(Object sender, EventArgs e)
        {
#if DEBUG
            sout.println("FormMain#menuVisualOverview_CheckedChanged; menuVisualOverview.isSelected()=" + menuVisualOverview.Checked);
#endif
            AppManager.editorConfig.OverviewEnabled = menuVisualOverview.Checked;
            updateLayout();
        }

        public void menuVisualMixer_Click(Object sender, EventArgs e)
        {
            bool v = !AppManager.editorConfig.MixerVisible;
            flipMixerDialogVisible(v);
            this.Focus();
        }

        public void menuVisualGridline_CheckedChanged(Object sender, EventArgs e)
        {
            AppManager.setGridVisible(menuVisualGridline.Checked);
            refreshScreen();
        }

        public void menuVisualIconPalette_Click(Object sender, EventArgs e)
        {
            bool v = !AppManager.editorConfig.IconPaletteVisible;
            flipIconPaletteVisible(v);
        }

        public void menuVisualLyrics_CheckedChanged(Object sender, EventArgs e)
        {
            AppManager.editorConfig.ShowLyric = menuVisualLyrics.Checked;
        }

        public void menuVisualNoteProperty_CheckedChanged(Object sender, EventArgs e)
        {
            AppManager.editorConfig.ShowExpLine = menuVisualNoteProperty.Checked;
            refreshScreen();
        }

        public void menuVisualPitchLine_CheckedChanged(Object sender, EventArgs e)
        {
            AppManager.editorConfig.ViewAtcualPitch = menuVisualPitchLine.Checked;
        }

        public void menuVisualControlTrack_CheckedChanged(Object sender, EventArgs e)
        {
            flipControlCurveVisible(menuVisualControlTrack.Checked);
        }

        public void menuVisualWaveform_CheckedChanged(Object sender, EventArgs e)
        {
            AppManager.editorConfig.ViewWaveform = menuVisualWaveform.Checked;
            updateSplitContainer2Size(true);
        }

        public void menuVisualPluginUi_DropDownOpening(Object sender, EventArgs e)
        {
#if ENABLE_VOCALOID
            // VOCALOID1, 2
            int c = VSTiDllManager.vocaloidDriver.Count;
            for (int i = 0; i < c; i++) {
                VocaloidDriver vd = VSTiDllManager.vocaloidDriver[i];
                bool chkv = true;
                if (vd == null) {
                    chkv = false;
                } else if (!vd.loaded) {
                    chkv = false;
                } else if (vd.getUi(this) == null) {
                    chkv = false;
                } else if (vd.getUi(this).IsDisposed) {
                    chkv = false;
                } else if (!vd.getUi(this).Visible) {
                    chkv = false;
                }
                RendererKind kind = vd.getRendererKind();
                if (kind == RendererKind.VOCALOID1) {
                    menuVisualPluginUiVocaloid1.Checked = chkv;
                } else if (kind == RendererKind.VOCALOID2) {
                    menuVisualPluginUiVocaloid2.Checked = chkv;
                }
            }
#endif

#if ENABLE_AQUESTONE
            // AquesTone
            AquesToneDriver drv = VSTiDllManager.getAquesToneDriver();
            bool chk = true;
            if (drv == null) {
                chk = false;
            } else if (!drv.loaded) {
                chk = false;
            } else {
                FormPluginUi ui = drv.getUi(this);
                if (ui == null) {
                    chk = false;
                } else if (ui.IsDisposed) {
                    chk = false;
                } else if (!ui.Visible) {
                    chk = false;
                }
            }
            menuVisualPluginUiAquesTone.Checked = chk;
#endif
        }

        public void menuVisualPluginUiVocaloidCommon_Click(Object sender, EventArgs e)
        {
            RendererKind search = RendererKind.NULL;
            //int vocaloid = 0;
            if (sender == menuVisualPluginUiVocaloid1) {
                search = RendererKind.VOCALOID1;
                //vocaloid = 1;
            } else if (sender == menuVisualPluginUiVocaloid2) {
                search = RendererKind.VOCALOID2;
                //vocaloid = 2;
            } else {
                return;
            }
#if DEBUG
            sout.println("FormMain#menuVisualPluginVocaloidCommon_Click; search=" + search);
#endif

#if ENABLE_VOCALOID
            int c = VSTiDllManager.vocaloidDriver.Count;
            for (int i = 0; i < c; i++) {
                VocaloidDriver vd = VSTiDllManager.vocaloidDriver[i];
                bool chk = true;
                if (vd == null) {
                    chk = false;
                } else if (!vd.loaded) {
                    chk = false;
                } else {
                    FormPluginUi ui = vd.getUi(this);
                    if (ui == null) {
                        chk = false;
                    } else if (ui.IsDisposed) {
                        chk = false;
                    }
                }
                if (!chk) {
                    continue;
                }
                RendererKind kind = vd.getRendererKind();
                bool v = true;
                if (kind == search) {
                    if (search == RendererKind.VOCALOID1) {
                        v = !menuVisualPluginUiVocaloid1.Checked;
                        menuVisualPluginUiVocaloid1.Checked = v;
                        vd.getUi(this).Visible = v;
                    } else if (search == RendererKind.VOCALOID2) {
                        v = !menuVisualPluginUiVocaloid2.Checked;
                        menuVisualPluginUiVocaloid2.Checked = v;
                        vd.getUi(this).Visible = v;
                    }
                    break;
                }
            }
#endif
        }

        private void onClickVisualPluginUiAquesTone(System.Windows.Forms.ToolStripMenuItem menu, AquesToneDriverBase drv)
        {
            bool visible = !menu.Checked;
            menu.Checked = visible;
#if ENABLE_AQUESTONE
            bool chk = true;
            FormPluginUi ui = null;
            if (drv == null) {
                chk = false;
            } else if (!drv.loaded) {
                chk = false;
            } else {
                ui = drv.getUi(this);
                if (ui == null) {
                    chk = false;
                } else if (ui.IsDisposed) {
                    chk = false;
                }
            }
            if (!chk) {
                menu.Checked = false;
                return;
            }
            if (ui != null && !ui.IsDisposed) {
                ui.Visible = visible;
            }
#endif
        }

        public void menuVisualPluginUiAquesTone_Click(Object sender, EventArgs e)
        {
            onClickVisualPluginUiAquesTone(menuVisualPluginUiAquesTone, VSTiDllManager.getAquesToneDriver());
        }

        private void menuVisualPluginUiAquesTone2_Click(object sender, EventArgs e)
        {
            onClickVisualPluginUiAquesTone(menuVisualPluginUiAquesTone2, VSTiDllManager.getAquesTone2Driver());
        }
        #endregion

        //BOOKMARK: mixerWindow
        #region mixerWindow
        public void mixerWindow_FormClosing(Object sender, FormClosingEventArgs e)
        {
            flipMixerDialogVisible(AppManager.mMixerWindow.Visible);
        }

        public void mixerWindow_SoloChanged(int track, bool solo)
        {
#if DEBUG
            AppManager.debugWriteLine("FormMain#mixerWindow_SoloChanged");
            AppManager.debugWriteLine("    track=" + track);
            AppManager.debugWriteLine("    solo=" + solo);
#endif
            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq == null) {
                return;
            }
            vsq.setSolo(track, solo);
            if (AppManager.mMixerWindow != null) {
                AppManager.mMixerWindow.updateStatus();
            }
        }

        public void mixerWindow_MuteChanged(int track, bool mute)
        {
#if DEBUG
            AppManager.debugWriteLine("FormMain#mixerWindow_MuteChanged");
            AppManager.debugWriteLine("    track=" + track);
            AppManager.debugWriteLine("    mute=" + mute);
#endif
            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq == null) {
                return;
            }
            if (track < 0) {
                AppManager.getBgm(-track - 1).mute = mute ? 1 : 0;
            } else {
                vsq.setMute(track, mute);
            }
            if (AppManager.mMixerWindow != null) {
                AppManager.mMixerWindow.updateStatus();
            }
        }

        public void mixerWindow_PanpotChanged(int track, int panpot)
        {
            if (track == 0) {
                // master
                AppManager.getVsqFile().Mixer.MasterPanpot = panpot;
            } else if (track > 0) {
                // slave
                AppManager.getVsqFile().Mixer.Slave[track - 1].Panpot = panpot;
            } else {
                AppManager.getBgm(-track - 1).panpot = panpot;
            }
        }

        public void mixerWindow_FederChanged(int track, int feder)
        {
#if DEBUG
            sout.println("FormMain#mixerWindow_FederChanged; track=" + track + "; feder=" + feder);
#endif
            if (track == 0) {
                AppManager.getVsqFile().Mixer.MasterFeder = feder;
            } else if (track > 0) {
                AppManager.getVsqFile().Mixer.Slave[track - 1].Feder = feder;
            } else {
                AppManager.getBgm(-track - 1).feder = feder;
            }
        }
        #endregion

        #region mPropertyPanelContainer
#if ENABLE_PROPERTY
        public void mPropertyPanelContainer_StateChangeRequired(Object sender, PanelState arg)
        {
            updatePropertyPanelState(arg);
        }
#endif
        #endregion

        #region propertyPanel
#if ENABLE_PROPERTY
        public void propertyPanel_CommandExecuteRequired(Object sender, CadenciiCommand command)
        {
#if DEBUG
            AppManager.debugWriteLine("m_note_property_dlg_CommandExecuteRequired");
#endif
            AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(command));
            updateDrawObjectList();
            refreshScreen();
            setEdited(true);
        }
#endif
        #endregion

        //BOOKMARK: propertyWindow
        #region PropertyWindowListenerの実装

#if ENABLE_PROPERTY
        public void propertyWindowFormClosing()
        {
#if DEBUG
            sout.println("FormMain#propertyWindowFormClosing");
#endif
            updatePropertyPanelState(PanelState.Hidden);
        }
#endif

#if ENABLE_PROPERTY
        public void propertyWindowStateChanged()
        {
#if DEBUG
            sout.println("FormMain#propertyWindow_WindowStateChanged");
#endif
            if (AppManager.editorConfig.PropertyWindowStatus.State == PanelState.Window) {
#if DEBUG
                sout.println("FormMain#proprtyWindow_WindowStateChanged; isWindowMinimized=" + AppManager.propertyWindow.getUi().isWindowMinimized());
#endif
                if (AppManager.propertyWindow.getUi().isWindowMinimized()) {
                    updatePropertyPanelState(PanelState.Docked);
                }
            }
        }

        public void propertyWindowLocationOrSizeChanged()
        {
#if DEBUG
            sout.println("FormMain#propertyWindow_LocationOrSizeChanged");
#endif
            if (AppManager.editorConfig.PropertyWindowStatus.State == PanelState.Window) {
                if (AppManager.propertyWindow != null && false == AppManager.propertyWindow.getUi().isWindowMinimized()) {
                    var parent = this.Location;
                    int propertyX = AppManager.propertyWindow.getUi().getX();
                    int propertyY = AppManager.propertyWindow.getUi().getY();
                    AppManager.editorConfig.PropertyWindowStatus.Bounds =
                        new XmlRectangle(propertyX - parent.X,
                                          propertyY - parent.Y,
                                          AppManager.propertyWindow.getUi().getWidth(),
                                          AppManager.propertyWindow.getUi().getHeight());
                }
            }
        }
#endif
        #endregion

        //BOOKMARK: FormMain
        #region FormMain
        public void handleDragExit()
        {
            AppManager.setEditMode(EditMode.NONE);
            mIconPaletteOnceDragEntered = false;
        }

        private void FormMain_DragLeave(Object sender, EventArgs e)
        {
            handleDragExit();
        }

        /// <summary>
        /// アイテムがドラッグされている最中の処理を行います
        /// </summary>
        public void handleDragOver(int screen_x, int screen_y)
        {
            if (AppManager.getEditMode() != EditMode.DRAG_DROP) {
                return;
            }
            var pt = pictPianoRoll.PointToScreen(System.Drawing.Point.Empty);
            if (!mIconPaletteOnceDragEntered) {
                int keywidth = AppManager.keyWidth;
                Rectangle rc = new Rectangle(pt.X + keywidth, pt.Y, pictPianoRoll.getWidth() - keywidth, pictPianoRoll.getHeight());
                if (Utility.isInRect(new Point(screen_x, screen_y), rc)) {
                    mIconPaletteOnceDragEntered = true;
                } else {
                    return;
                }
            }
            MouseEventArgs e0 = new MouseEventArgs(MouseButtons.Left,
                                                    1,
                                                    screen_x - pt.X,
                                                    screen_y - pt.Y,
                                                    0);
            pictPianoRoll_MouseMove(this, e0);
        }

        private void FormMain_DragOver(Object sender, System.Windows.Forms.DragEventArgs e)
        {
            handleDragOver(e.X, e.Y);
        }

        /// <summary>
        /// ピアノロールにドロップされたIconDynamicsHandleの受け入れ処理を行います
        /// </summary>
        public void handleDragDrop(IconDynamicsHandle handle, int screen_x, int screen_y)
        {
            if (handle == null) {
                return;
            }
            var locPianoroll = pictPianoRoll.PointToScreen(System.Drawing.Point.Empty);
            // ドロップ位置を特定して，アイテムを追加する
            int x = screen_x - locPianoroll.X;
            int y = screen_y - locPianoroll.Y;
            int clock1 = AppManager.clockFromXCoord(x);

            // クオンタイズの処理
            int unit = AppManager.getPositionQuantizeClock();
            int clock = doQuantize(clock1, unit);

            int note = AppManager.noteFromYCoord(y);
            VsqFileEx vsq = AppManager.getVsqFile();
            int clockAtPremeasure = vsq.getPreMeasureClocks();
            if (clock < clockAtPremeasure) {
                return;
            }
            if (note < 0 || 128 < note) {
                return;
            }

            int selected = AppManager.getSelected();
            VsqTrack vsq_track = vsq.Track[selected];
            VsqTrack work = (VsqTrack)vsq_track.clone();

            if (AppManager.mAddingEvent == null) {
                // ここは多分起こらない
                return;
            }
            VsqEvent item = (VsqEvent)AppManager.mAddingEvent.clone();
            item.Clock = clock;
            item.ID.Note = note;
            work.addEvent(item);
            work.reflectDynamics();
            CadenciiCommand run = VsqFileEx.generateCommandTrackReplace(selected, work, vsq.AttachedCurves.get(selected - 1));
            AppManager.editHistory.register(vsq.executeCommand(run));
            setEdited(true);
            AppManager.setEditMode(EditMode.NONE);
            refreshScreen();
        }

        private void FormMain_DragDrop(Object sender, System.Windows.Forms.DragEventArgs e)
        {
            AppManager.setEditMode(EditMode.NONE);
            mIconPaletteOnceDragEntered = false;
            mMouseDowned = false;
            if (!e.Data.GetDataPresent(typeof(IconDynamicsHandle))) {
                return;
            }
            var locPianoroll = pictPianoRoll.PointToScreen(System.Drawing.Point.Empty);
            int keywidth = AppManager.keyWidth;
            Rectangle rcPianoroll = new Rectangle(locPianoroll.X + keywidth,
                                                   locPianoroll.Y,
                                                   pictPianoRoll.getWidth() - keywidth,
                                                   pictPianoRoll.getHeight());
            if (!Utility.isInRect(new Point(e.X, e.Y), rcPianoroll)) {
                return;
            }

            // dynaff, crescend, decrescend のどれがドロップされたのか検査
            IconDynamicsHandle this_is_it = (IconDynamicsHandle)e.Data.GetData(typeof(IconDynamicsHandle));
            if (this_is_it == null) {
                return;
            }

            handleDragDrop(this_is_it, e.X, e.Y);
        }

        /// <summary>
        /// ドラッグの開始処理を行います
        /// </summary>
        public void handleDragEnter()
        {
            AppManager.setEditMode(EditMode.DRAG_DROP);
            mMouseDowned = true;
        }

        private void FormMain_DragEnter(Object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(IconDynamicsHandle))) {
                // ドロップ可能
                e.Effect = System.Windows.Forms.DragDropEffects.All;
                handleDragEnter();
            } else {
                e.Effect = System.Windows.Forms.DragDropEffects.None;
                AppManager.setEditMode(EditMode.NONE);
            }
        }
        public void FormMain_FormClosed(Object sender, FormClosedEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#FormMain_FormClosed");
#endif
            clearTempWave();
            string tempdir = Path.Combine(AppManager.getCadenciiTempDir(), AppManager.getID());
            if (!Directory.Exists(tempdir)) {
                PortUtil.createDirectory(tempdir);
            }
            string log = Path.Combine(tempdir, "run.log");
            cadencii.debug.close();
            try {
                if (System.IO.File.Exists(log)) {
                    PortUtil.deleteFile(log);
                }
                PortUtil.deleteDirectory(tempdir, true);
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".FormMain_FormClosed; ex=" + ex + "\n");
                serr.println("FormMain#FormMain_FormClosed; ex=" + ex);
            }
            AppManager.stopGenerator();
            VSTiDllManager.terminate();
#if ENABLE_MIDI
            //MidiPlayer.stop();
            if (mMidiIn != null) {
                mMidiIn.close();
            }
#endif
#if ENABLE_MTC
            if ( m_midi_in_mtc != null ) {
                m_midi_in_mtc.Close();
            }
#endif
            PlaySound.kill();
            PluginLoader.cleanupUnusedAssemblyCache();
        }

        public void FormMain_FormClosing(Object sender, FormClosingEventArgs e)
        {
            // 設定値を格納
            if (AppManager.editorConfig.ViewWaveform) {
                AppManager.editorConfig.SplitContainer2LastDividerLocation = splitContainer2.getDividerLocation();
            }
            if (AppManager.editorConfig.PropertyWindowStatus.State == PanelState.Docked) {
                AppManager.editorConfig.PropertyWindowStatus.DockWidth = splitContainerProperty.getDividerLocation();
            }
            if (e.CloseReason == System.Windows.Forms.CloseReason.WindowsShutDown) {
                return;
            }
            bool cancel = handleFormClosing();
            e.Cancel = cancel;
        }

        /// <summary>
        /// ウィンドウが閉じようとしているときの処理を行う
        /// 戻り値がtrueの場合，ウィンドウが閉じるのをキャンセルする処理が必要
        /// </summary>
        /// <returns></returns>
        public bool handleFormClosing()
        {
            if (isEdited()) {
                string file = AppManager.getFileName();
                if (file.Equals("")) {
                    file = "Untitled";
                } else {
                    file = PortUtil.getFileName(file);
                }
                DialogResult ret = AppManager.showMessageBox(_("Save this sequence?"),
                                                               _("Affirmation"),
                                                               cadencii.windows.forms.Utility.MSGBOX_YES_NO_CANCEL_OPTION,
                                                               cadencii.windows.forms.Utility.MSGBOX_QUESTION_MESSAGE);
                if (ret == DialogResult.Yes) {
                    if (AppManager.getFileName().Equals("")) {
                        var dr = AppManager.showModalDialog(saveXmlVsqDialog, false, this);
                        if (dr == System.Windows.Forms.DialogResult.OK) {
                            AppManager.saveTo(saveXmlVsqDialog.FileName);
                        } else {
                            return true;
                        }
                    } else {
                        AppManager.saveTo(AppManager.getFileName());
                    }

                } else if (ret == DialogResult.Cancel) {
                    return true;
                }
            }
            AppManager.editorConfig.WindowMaximized = (this.WindowState == FormWindowState.Maximized);
            AppManager.saveConfig();
            UtauWaveGenerator.clearCache();
            VConnectWaveGenerator.clearCache();

#if ENABLE_MIDI
            if (mMidiIn != null) {
                mMidiIn.close();
            }
#endif
            bgWorkScreen.Dispose();
            return false;
        }

        public void FormMain_LocationChanged(Object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal) {
                var bounds = this.Bounds;
                AppManager.editorConfig.WindowRect = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            }
        }

        public void FormMain_Load(Object sender, EventArgs e)
        {
            applyLanguage();

            // ツールバーの位置を復帰させる
            // toolStipの位置を，前回終了時の位置に戻す
            int chevron_width = AppManager.editorConfig.ChevronWidth;
            this.bandFile = new RebarBand();
            this.bandPosition = new RebarBand();
            this.bandMeasure = new RebarBand();
            this.bandTool = new RebarBand();

            bool variant_height = false;
            this.bandFile.VariantHeight = variant_height;
            this.bandPosition.VariantHeight = variant_height;
            this.bandMeasure.VariantHeight = variant_height;
            this.bandTool.VariantHeight = variant_height;

            int MAX_BAND_HEIGHT = 26;// toolBarTool.Height;

            this.rebar.Controls.Add(this.toolBarFile);
            this.rebar.Controls.Add(this.toolBarTool);
            this.rebar.Controls.Add(this.toolBarPosition);
            this.rebar.Controls.Add(this.toolBarMeasure);
            // bandFile
            this.bandFile.AllowVertical = false;
            this.bandFile.Child = this.toolBarFile;
            this.bandFile.Header = -1;
            this.bandFile.Integral = 1;
            this.bandFile.MaxHeight = MAX_BAND_HEIGHT;
            this.bandFile.UseChevron = true;
            if (toolBarFile.Buttons.Count > 0) {
                this.bandFile.IdealWidth =
                    toolBarFile.Buttons[toolBarFile.Buttons.Count - 1].Rectangle.Right + chevron_width;
            }
            this.bandFile.BandSize = AppManager.editorConfig.BandSizeFile;
            this.bandFile.NewRow = AppManager.editorConfig.BandNewRowFile;
            // bandPosition
            this.bandPosition.AllowVertical = false;
            this.bandPosition.Child = this.toolBarPosition;
            this.bandPosition.Header = -1;
            this.bandPosition.Integral = 1;
            this.bandPosition.MaxHeight = MAX_BAND_HEIGHT;
            this.bandPosition.UseChevron = true;
            if (toolBarPosition.Buttons.Count > 0) {
                this.bandPosition.IdealWidth =
                    toolBarPosition.Buttons[toolBarPosition.Buttons.Count - 1].Rectangle.Right + chevron_width;
            }
            this.bandPosition.BandSize = AppManager.editorConfig.BandSizePosition;
            this.bandPosition.NewRow = AppManager.editorConfig.BandNewRowPosition;
            // bandMeasure
            this.bandMeasure.AllowVertical = false;
            this.bandMeasure.Child = this.toolBarMeasure;
            this.bandMeasure.Header = -1;
            this.bandMeasure.Integral = 1;
            this.bandMeasure.MaxHeight = MAX_BAND_HEIGHT;
            this.bandMeasure.UseChevron = true;
            if (toolBarMeasure.Buttons.Count > 0) {
                this.bandMeasure.IdealWidth =
                    toolBarMeasure.Buttons[toolBarMeasure.Buttons.Count - 1].Rectangle.Right + chevron_width;
            }
            this.bandMeasure.BandSize = AppManager.editorConfig.BandSizeMeasure;
            this.bandMeasure.NewRow = AppManager.editorConfig.BandNewRowMeasure;
            // bandTool
            this.bandTool.AllowVertical = false;
            this.bandTool.Child = this.toolBarTool;
            this.bandTool.Header = -1;
            this.bandTool.Integral = 1;
            this.bandTool.MaxHeight = MAX_BAND_HEIGHT;
            this.bandTool.UseChevron = true;
            if (toolBarTool.Buttons.Count > 0) {
                this.bandTool.IdealWidth =
                    toolBarTool.Buttons[toolBarTool.Buttons.Count - 1].Rectangle.Right + chevron_width;
            }
            this.bandTool.BandSize = AppManager.editorConfig.BandSizeTool;
            this.bandTool.NewRow = AppManager.editorConfig.BandNewRowTool;
            // 一度リストに入れてから追加する
            var bands = new RebarBand[] { null, null, null, null };
            // 番号がおかしくないかチェック
            if (AppManager.editorConfig.BandOrderFile < 0 || bands.Length <= AppManager.editorConfig.BandOrderFile) AppManager.editorConfig.BandOrderFile = 0;
            if (AppManager.editorConfig.BandOrderMeasure < 0 || bands.Length <= AppManager.editorConfig.BandOrderMeasure) AppManager.editorConfig.BandOrderMeasure = 0;
            if (AppManager.editorConfig.BandOrderPosition < 0 || bands.Length <= AppManager.editorConfig.BandOrderPosition) AppManager.editorConfig.BandOrderPosition = 0;
            if (AppManager.editorConfig.BandOrderTool < 0 || bands.Length <= AppManager.editorConfig.BandOrderTool) AppManager.editorConfig.BandOrderTool = 0;
            bands[AppManager.editorConfig.BandOrderFile] = bandFile;
            bands[AppManager.editorConfig.BandOrderMeasure] = bandMeasure;
            bands[AppManager.editorConfig.BandOrderPosition] = bandPosition;
            bands[AppManager.editorConfig.BandOrderTool] = bandTool;
            // nullチェック
            bool null_exists = false;
            for (var i = 0; i < bands.Length; i++) {
                if (bands[i] == null) {
                    null_exists = true;
                    break;
                }
            }
            if (null_exists) {
                // 番号に矛盾があれば，デフォルトの並び方で
                bands[0] = bandFile;
                bands[1] = bandMeasure;
                bands[2] = bandPosition;
                bands[3] = bandTool;
                bandFile.NewRow = true;
                bandMeasure.NewRow = true;
                bandPosition.NewRow = true;
                bandTool.NewRow = true;
            }

            // 追加
            for (var i = 0; i < bands.Length; i++) {
                if (i == 0) bands[i].NewRow = true;
                bands[i].MinHeight = 24;
                this.rebar.Bands.Add(bands[i]);
            }

#if DEBUG
            sout.println("FormMain#.ctor; this.Width=" + this.Width);
#endif
            bandTool.Resize += this.toolStripEdit_Resize;
            bandMeasure.Resize += this.toolStripMeasure_Resize;
            bandPosition.Resize += this.toolStripPosition_Resize;
            bandFile.Resize += this.toolStripFile_Resize;

            updateSplitContainer2Size(false);

            ensureVisibleY(60);

            // 鍵盤用の音源の準備．Javaはこの機能は削除で．
            // 鍵盤用のキャッシュが古い位置に保存されている場合。
            string cache_new = Utility.getKeySoundPath();
            string cache_old = Path.Combine(PortUtil.getApplicationStartupPath(), "cache");
            if (Directory.Exists(cache_old)) {
                bool exists = false;
                for (int i = 0; i < 127; i++) {
                    string s = Path.Combine(cache_new, i + ".wav");
                    if (System.IO.File.Exists(s)) {
                        exists = true;
                        break;
                    }
                }

                // 新しいキャッシュが1つも無い場合に、古いディレクトリからコピーする
                if (!exists) {
                    for (int i = 0; i < 127; i++) {
                        string wav_from = Path.Combine(cache_old, i + ".wav");
                        string wav_to = Path.Combine(cache_new, i + ".wav");
                        if (System.IO.File.Exists(wav_from)) {
                            try {
                                PortUtil.copyFile(wav_from, wav_to);
                                PortUtil.deleteFile(wav_from);
                            } catch (Exception ex) {
                                Logger.write(typeof(FormMain) + ".FormMain_Load; ex=" + ex + "\n");
                                serr.println("FormMain#FormMain_Load; ex=" + ex);
                            }
                        }
                    }
                }
            }

            // 足りてないキャッシュがひとつでもあればFormGenerateKeySound発動する
            bool cache_is_incomplete = false;
            for (int i = 0; i < 127; i++) {
                string wav = Path.Combine(cache_new, i + ".wav");
                if (!System.IO.File.Exists(wav)) {
                    cache_is_incomplete = true;
                    break;
                }
            }

            bool init_key_sound_player_immediately = true; //FormGenerateKeySoundの終了を待たずにKeySoundPlayer.initするかどうか。
            if (!AppManager.editorConfig.DoNotAskKeySoundGeneration && cache_is_incomplete) {
                FormAskKeySoundGenerationController dialog = null;
                int dialog_result = 0;
                bool always_check_this = !AppManager.editorConfig.DoNotAskKeySoundGeneration;
                try {
                    dialog = new FormAskKeySoundGenerationController();
                    dialog.setupUi(new FormAskKeySoundGenerationUiImpl(dialog));
                    dialog.getUi().setAlwaysPerformThisCheck(always_check_this);
                    dialog_result = AppManager.showModalDialog(dialog.getUi(), this);
                    always_check_this = dialog.getUi().isAlwaysPerformThisCheck();
                } catch (Exception ex) {
                    Logger.write(typeof(FormMain) + ".FormMain_Load; ex=" + ex + "\n");
                    serr.println("FormMain#FormMain_Load; ex=" + ex);
                } finally {
                    if (dialog != null) {
                        try {
                            dialog.getUi().close(true);
                        } catch (Exception ex2) {
                            Logger.write(typeof(FormMain) + ".FormMain_Load; ex=" + ex2 + "\n");
                            serr.println("FormMain#FormMain_Load; ex2=" + ex2);
                        }
                    }
                }
                AppManager.editorConfig.DoNotAskKeySoundGeneration = !always_check_this;

                if (dialog_result == 1) {
                    FormGenerateKeySound form = null;
                    try {
                        form = new FormGenerateKeySound(true);
                        form.FormClosed += new FormClosedEventHandler(FormGenerateKeySound_FormClosed);
                        form.ShowDialog();
                    } catch (Exception ex) {
                        Logger.write(typeof(FormMain) + ".FormMain_Load; ex=" + ex + "\n");
                        serr.println("FormMain#FormMain_Load; ex=" + ex);
                    }
                    init_key_sound_player_immediately = false;
                }
            }

            if (init_key_sound_player_immediately) {
                try {
                    KeySoundPlayer.init();
                } catch (Exception ex) {
                    Logger.write(typeof(FormMain) + ".FormMain_Load; ex=" + ex + "\n");
                    serr.println("FormMain#FormMain_Load; ex=" + ex);
                }
            }

            if (!AppManager.editorConfig.DoNotAutomaticallyCheckForUpdates) {
                showUpdateInformationAsync(false);
            }
        }

        public void FormGenerateKeySound_FormClosed(Object sender, FormClosedEventArgs e)
        {
            try {
                KeySoundPlayer.init();
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".FormGenerateKeySound_FormClosed; ex=" + ex + "\n");
                serr.println("FormMain#FormGenerateKeySound_FormClosed; ex=" + ex);
            }
        }

        void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if (mWindowState == this.WindowState) {
                return;
            }
            var state = this.WindowState;
            if (state == FormWindowState.Normal || state == FormWindowState.Maximized) {
                if (state == FormWindowState.Normal) {
                    var bounds = this.Bounds;
                    AppManager.editorConfig.WindowRect = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                }
#if ENABLE_PROPERTY
                // プロパティウィンドウの状態を更新
                if (AppManager.editorConfig.PropertyWindowStatus.State == PanelState.Window) {
                    if (AppManager.propertyWindow.getUi().isWindowMinimized()) {
                        AppManager.propertyWindow.getUi().deiconfyWindow();
                    }
                    if (!AppManager.propertyWindow.getUi().isVisible()) {
                        AppManager.propertyWindow.getUi().setVisible(true);
                    }
                }
#endif
                // ミキサーウィンドウの状態を更新
                bool vm = AppManager.editorConfig.MixerVisible;
                if (vm != AppManager.mMixerWindow.Visible) {
                    AppManager.mMixerWindow.Visible = vm;
                }

                // アイコンパレットの状態を更新
                if (AppManager.iconPalette != null && menuVisualIconPalette.Checked) {
                    if (!AppManager.iconPalette.Visible) {
                        AppManager.iconPalette.Visible = true;
                    }
                }
                updateLayout();
                this.Focus();
            } else if (state == FormWindowState.Minimized) {
#if ENABLE_PROPERTY
                AppManager.propertyWindow.getUi().setVisible(false);
#endif
                AppManager.mMixerWindow.Visible = false;
                if (AppManager.iconPalette != null) {
                    AppManager.iconPalette.Visible = false;
                }
            }/* else if ( state == BForm.MAXIMIZED_BOTH ) {
#if ENABLE_PROPERTY
                AppManager.propertyWindow.setExtendedState( BForm.NORMAL );
                AppManager.propertyWindow.setVisible( AppManager.editorConfig.PropertyWindowStatus.State == PanelState.Window );
#endif
                AppManager.mMixerWindow.setVisible( AppManager.editorConfig.MixerVisible );
                if ( AppManager.iconPalette != null && menuVisualIconPalette.isSelected() ) {
                    AppManager.iconPalette.setVisible( true );
                }
                this.requestFocus();
            }*/
        }

        public void FormMain_MouseWheel(Object sender, MouseEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#FormMain_MouseWheel");
#endif
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift) {
                hScroll.Value = computeScrollValueFromWheelDelta(e.Delta);
            } else {
                int max = vScroll.Maximum - vScroll.LargeChange;
                int min = vScroll.Minimum;
                double new_val = (double)vScroll.Value - e.Delta;
                if (new_val > max) {
                    vScroll.Value = max;
                } else if (new_val < min) {
                    vScroll.Value = min;
                } else {
                    vScroll.Value = (int)new_val;
                }
            }
            refreshScreen();
        }

        public void FormMain_PreviewKeyDown(Object sender, PreviewKeyDownEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#FormMain_PreviewKeyDown");
#endif
            KeyEventArgs ex = new KeyEventArgs(e.KeyData);
            processSpecialShortcutKey(ex, true);
        }

        public void handleVScrollResize(Object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized) {
                updateScrollRangeVertical();
                controller.setStartToDrawY(calculateStartToDrawY(vScroll.Value));
            }
        }

        public void FormMain_Deactivate(Object sender, EventArgs e)
        {
            mFormActivated = false;
        }

        public void FormMain_Activated(Object sender, EventArgs e)
        {
            mFormActivated = true;
        }
        #endregion

        #region mTimer
        public void mTimer_Tick(Object sender, EventArgs e)
        {
            if (!mFormActivated) {
                return;
            }
            try {
                double now = PortUtil.getCurrentTime();
                byte[] buttons;
                int pov0;
                bool ret = winmmhelp.JoyGetStatus(0, out buttons, out pov0);
                bool event_processed = false;
                double dt_ms = (now - mLastEventProcessed) * 1000.0;

                EditorConfig m = AppManager.editorConfig;
                bool btn_x = (0 <= m.GameControlerCross && m.GameControlerCross < buttons.Length && buttons[m.GameControlerCross] > 0x00);
                bool btn_o = (0 <= m.GameControlerCircle && m.GameControlerCircle < buttons.Length && buttons[m.GameControlerCircle] > 0x00);
                bool btn_tr = (0 <= m.GameControlerTriangle && m.GameControlerTriangle < buttons.Length && buttons[m.GameControlerTriangle] > 0x00);
                bool btn_re = (0 <= m.GameControlerRectangle && m.GameControlerRectangle < buttons.Length && buttons[m.GameControlerRectangle] > 0x00);
                bool pov_r = pov0 == m.GameControlPovRight;
                bool pov_l = pov0 == m.GameControlPovLeft;
                bool pov_u = pov0 == m.GameControlPovUp;
                bool pov_d = pov0 == m.GameControlPovDown;
                bool L1 = (0 <= m.GameControlL1 && m.GameControlL1 < buttons.Length && buttons[m.GameControlL1] > 0x00);
                bool R1 = (0 <= m.GameControlL2 && m.GameControlL2 < buttons.Length && buttons[m.GameControlR1] > 0x00);
                bool L2 = (0 <= m.GameControlR1 && m.GameControlR1 < buttons.Length && buttons[m.GameControlL2] > 0x00);
                bool R2 = (0 <= m.GameControlR2 && m.GameControlR2 < buttons.Length && buttons[m.GameControlR2] > 0x00);
                bool SELECT = (0 <= m.GameControlSelect && m.GameControlSelect <= buttons.Length && buttons[m.GameControlSelect] > 0x00);
                if (mGameMode == GameControlMode.NORMAL) {
                    mLastBtnX = btn_x;

                    if (!event_processed && !btn_o && mLastBtnO) {
                        if (AppManager.isPlaying()) {
                            timer.Stop();
                        }
                        AppManager.setPlaying(!AppManager.isPlaying(), this);
                        mLastEventProcessed = now;
                        event_processed = true;
                    }
                    mLastBtnO = btn_o;

                    if (!event_processed && pov_r && dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval) {
                        forward();
                        mLastEventProcessed = now;
                        event_processed = true;
                    }
                    mLastPovR = pov_r;

                    if (!event_processed && pov_l && dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval) {
                        rewind();
                        mLastEventProcessed = now;
                        event_processed = true;
                    }
                    mLastPovL = pov_l;

                    if (!event_processed && pov_u && dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval) {
                        int draft_vscroll = vScroll.Value - (int)(100 * controller.getScaleY()) * 3;
                        if (draft_vscroll < vScroll.Minimum) {
                            draft_vscroll = vScroll.Minimum;
                        }
                        vScroll.Value = draft_vscroll;
                        refreshScreen();
                        mLastEventProcessed = now;
                        event_processed = true;
                    }

                    if (!event_processed && pov_d && dt_ms > AppManager.editorConfig.GameControlerMinimumEventInterval) {
                        int draft_vscroll = vScroll.Value + (int)(100 * controller.getScaleY()) * 3;
                        if (draft_vscroll > vScroll.Maximum) {
                            draft_vscroll = vScroll.Maximum;
                        }
                        vScroll.Value = draft_vscroll;
                        refreshScreen();
                        mLastEventProcessed = now;
                        event_processed = true;
                    }

                    if (!event_processed && !SELECT && mLastBtnSelect) {
                        event_processed = true;
                        mGameMode = GameControlMode.KEYBOARD;
                        stripLblGameCtrlMode.Text = mGameMode.ToString();
                        stripLblGameCtrlMode.Image = Properties.Resources.piano;
                    }
                    mLastBtnSelect = SELECT;
                } else if (mGameMode == GameControlMode.KEYBOARD) {
                    if (!event_processed && !SELECT && mLastBtnSelect) {
                        event_processed = true;
                        mGameMode = GameControlMode.NORMAL;
                        updateGameControlerStatus(null, null);
                        mLastBtnSelect = SELECT;
                        return;
                    }
                    mLastBtnSelect = SELECT;

                    int note = -1;
                    if (pov_r && !mLastPovR) {
                        note = 60;
                    } else if (btn_re && !mLastBtnRe) {
                        note = 62;
                    } else if (btn_tr && !mLastBtnTr) {
                        note = 64;
                    } else if (btn_o && !mLastBtnO) {
                        note = 65;
                    } else if (btn_x && !mLastBtnX) {
                        note = 67;
                    } else if (pov_u && !mLastPovU) {
                        note = 59;
                    } else if (pov_l && !mLastPovL) {
                        note = 57;
                    } else if (pov_d && !mLastPovD) {
                        note = 55;
                    }
                    if (note >= 0) {
                        if (L1) {
                            note += 12;
                        } else if (L2) {
                            note -= 12;
                        }
                        if (R1) {
                            note += 1;
                        } else if (R2) {
                            note -= 1;
                        }
                    }
                    mLastBtnO = btn_o;
                    mLastBtnX = btn_x;
                    mLastBtnRe = btn_re;
                    mLastBtnTr = btn_tr;
                    mLastPovL = pov_l;
                    mLastPovD = pov_d;
                    mLastPovR = pov_r;
                    mLastPovU = pov_u;
                    if (note >= 0) {
#if DEBUG
                        AppManager.debugWriteLine("FormMain#mTimer_Tick");
                        AppManager.debugWriteLine("    note=" + note);
#endif
                        if (AppManager.isPlaying()) {
                            int clock = AppManager.getCurrentClock();
                            int selected = AppManager.getSelected();
                            if (AppManager.mAddingEvent != null) {
                                AppManager.mAddingEvent.ID.setLength(clock - AppManager.mAddingEvent.Clock);
                                CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventAdd(selected,
                                                                                                               AppManager.mAddingEvent));
                                AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                                if (!isEdited()) {
                                    setEdited(true);
                                }
                                updateDrawObjectList();
                            }
                            AppManager.mAddingEvent = new VsqEvent(clock, new VsqID(0));
                            AppManager.mAddingEvent.ID.type = VsqIDType.Anote;
                            AppManager.mAddingEvent.ID.Dynamics = 64;
                            AppManager.mAddingEvent.ID.VibratoHandle = null;
                            AppManager.mAddingEvent.ID.LyricHandle = new LyricHandle("a", "a");
                            AppManager.mAddingEvent.ID.Note = note;
                        }
                        KeySoundPlayer.play(note);
                    } else {
                        if (AppManager.isPlaying() && AppManager.mAddingEvent != null) {
                            AppManager.mAddingEvent.ID.setLength(AppManager.getCurrentClock() - AppManager.mAddingEvent.Clock);
                        }
                    }
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".mTimer_Tick; ex=" + ex + "\n");
#if DEBUG
                AppManager.debugWriteLine("    ex=" + ex);
#endif
                mGameMode = GameControlMode.DISABLED;
                updateGameControlerStatus(null, null);
                mTimer.Stop();
            }
        }
        #endregion

        //BOOKMARK: menuFile
        #region menuFile*
        public void menuFileRecentClear_Click(Object sender, EventArgs e)
        {
            if (AppManager.editorConfig.RecentFiles != null) {
                AppManager.editorConfig.RecentFiles.Clear();
            }
            updateRecentFileMenu();
        }

        public void menuFileSaveNamed_Click(Object sender, EventArgs e)
        {
            for (int track = 1; track < AppManager.getVsqFile().Track.Count; track++) {
                if (AppManager.getVsqFile().Track[track].getEventCount() == 0) {
                    AppManager.showMessageBox(
                        PortUtil.formatMessage(
                            _("Invalid note data.\nTrack {0} : {1}\n\n-> Piano roll : Blank sequence."), track, AppManager.getVsqFile().Track[track].getName()
                        ),
                        _APP_NAME,
                        cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                        cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE);
                    return;
                }
            }

            string dir = AppManager.editorConfig.getLastUsedPathOut("xvsq");
            saveXmlVsqDialog.SetSelectedFile(dir);
            var dr = AppManager.showModalDialog(saveXmlVsqDialog, false, this);
            if (dr == System.Windows.Forms.DialogResult.OK) {
                string file = saveXmlVsqDialog.FileName;
                AppManager.editorConfig.setLastUsedPathOut(file, ".xvsq");
                AppManager.saveTo(file);
                updateRecentFileMenu();
                setEdited(false);
            }
        }

        public void menuFileQuit_Click(Object sender, EventArgs e)
        {
            Close();
        }

        public void menuFileExport_DropDownOpening(Object sender, EventArgs e)
        {
            menuFileExportWave.Enabled = (AppManager.getVsqFile().Track[AppManager.getSelected()].getEventCount() > 0);
        }

        public void menuFileExportMidi_Click(Object sender, EventArgs e)
        {
            if (mDialogMidiImportAndExport == null) {
                mDialogMidiImportAndExport = new FormMidiImExport();
            }
            mDialogMidiImportAndExport.listTrack.Items.Clear();
            VsqFileEx vsq = (VsqFileEx)AppManager.getVsqFile().clone();

            for (int i = 0; i < vsq.Track.Count; i++) {
                VsqTrack track = vsq.Track[i];
                int notes = 0;
                foreach (var obj in track.getNoteEventIterator()) {
                    notes++;
                }
                mDialogMidiImportAndExport.listTrack.AddRow(new string[] { i + "", track.getName(), notes + "" }, true);
            }
            mDialogMidiImportAndExport.setMode(FormMidiImExport.FormMidiMode.EXPORT);
            mDialogMidiImportAndExport.Location = getFormPreferedLocation(mDialogMidiImportAndExport);
            DialogResult dr = AppManager.showModalDialog(mDialogMidiImportAndExport, this);
            if (dr == DialogResult.OK) {
                if (!mDialogMidiImportAndExport.isPreMeasure()) {
                    vsq.removePart(0, vsq.getPreMeasureClocks());
                }
                int track_count = 0;
                for (int i = 0; i < mDialogMidiImportAndExport.listTrack.Items.Count; i++) {
                    if (mDialogMidiImportAndExport.listTrack.Items[i].Checked) {
                        track_count++;
                    }
                }
                if (track_count == 0) {
                    return;
                }

                string dir = AppManager.editorConfig.getLastUsedPathOut("mid");
                saveMidiDialog.SetSelectedFile(dir);
                var dialog_result = AppManager.showModalDialog(saveMidiDialog, false, this);

                if (dialog_result == System.Windows.Forms.DialogResult.OK) {
                    FileStream fs = null;
                    string filename = saveMidiDialog.FileName;
                    AppManager.editorConfig.setLastUsedPathOut(filename, ".mid");
                    try {
                        fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        // ヘッダー
                        fs.Write(new byte[] { 0x4d, 0x54, 0x68, 0x64 }, 0, 4);
                        //データ長
                        fs.WriteByte((byte)0x00);
                        fs.WriteByte((byte)0x00);
                        fs.WriteByte((byte)0x00);
                        fs.WriteByte((byte)0x06);
                        //フォーマット
                        fs.WriteByte((byte)0x00);
                        fs.WriteByte((byte)0x01);
                        //トラック数
                        VsqFile.writeUnsignedShort(fs, track_count);
                        //時間単位
                        fs.WriteByte((byte)0x01);
                        fs.WriteByte((byte)0xe0);
                        int count = -1;
                        for (int i = 0; i < mDialogMidiImportAndExport.listTrack.Items.Count; i++) {
                            if (!mDialogMidiImportAndExport.listTrack.Items[i].Checked) {
                                continue;
                            }
                            VsqTrack track = vsq.Track[i];
                            count++;
                            fs.Write(new byte[] { 0x4d, 0x54, 0x72, 0x6b }, 0, 4);
                            //データ長。とりあえず0を入れておく
                            fs.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }, 0, 4);
                            long first_position = fs.Position;
                            //トラック名
                            VsqFile.writeFlexibleLengthUnsignedLong(fs, 0);//デルタタイム
                            fs.WriteByte((byte)0xff);//ステータスタイプ
                            fs.WriteByte((byte)0x03);//イベントタイプSequence/Track Name
                            byte[] track_name = PortUtil.getEncodedByte("Shift_JIS", track.getName());
                            fs.WriteByte((byte)track_name.Length);
                            fs.Write(track_name, 0, track_name.Length);

                            List<MidiEvent> events = new List<MidiEvent>();

                            // tempo
                            bool print_tempo = mDialogMidiImportAndExport.isTempo();
                            if (print_tempo && count == 0) {
                                List<MidiEvent> tempo_events = vsq.generateTempoChange();
                                for (int j = 0; j < tempo_events.Count; j++) {
                                    events.Add(tempo_events[j]);
                                }
                            }

                            // timesig
                            if (mDialogMidiImportAndExport.isTimesig() && count == 0) {
                                List<MidiEvent> timesig_events = vsq.generateTimeSig();
                                for (int j = 0; j < timesig_events.Count; j++) {
                                    events.Add(timesig_events[j]);
                                }
                            }

                            // Notes
                            if (mDialogMidiImportAndExport.isNotes()) {
                                foreach (var ve in track.getNoteEventIterator()) {
                                    int clock_on = ve.Clock;
                                    int clock_off = ve.Clock + ve.ID.getLength();
                                    if (!print_tempo) {
                                        // テンポを出力しない場合、テンポを500000（120）と見なしてクロックを再計算
                                        double time_on = vsq.getSecFromClock(clock_on);
                                        double time_off = vsq.getSecFromClock(clock_off);
                                        clock_on = (int)(960.0 * time_on);
                                        clock_off = (int)(960.0 * time_off);
                                    }
                                    MidiEvent noteon = new MidiEvent();
                                    noteon.clock = clock_on;
                                    noteon.firstByte = 0x90;
                                    noteon.data = new int[2];
                                    noteon.data[0] = ve.ID.Note;
                                    noteon.data[1] = ve.ID.Dynamics;
                                    events.Add(noteon);
                                    MidiEvent noteoff = new MidiEvent();
                                    noteoff.clock = clock_off;
                                    noteoff.firstByte = 0x80;
                                    noteoff.data = new int[2];
                                    noteoff.data[0] = ve.ID.Note;
                                    noteoff.data[1] = 0x7f;
                                    events.Add(noteoff);
                                }
                            }

                            // lyric
                            if (mDialogMidiImportAndExport.isLyric()) {
                                foreach (var ve in track.getNoteEventIterator()) {
                                    int clock_on = ve.Clock;
                                    if (!print_tempo) {
                                        double time_on = vsq.getSecFromClock(clock_on);
                                        clock_on = (int)(960.0 * time_on);
                                    }
                                    MidiEvent add = new MidiEvent();
                                    add.clock = clock_on;
                                    add.firstByte = 0xff;
                                    byte[] lyric = PortUtil.getEncodedByte("Shift_JIS", ve.ID.LyricHandle.L0.Phrase);
                                    add.data = new int[lyric.Length + 1];
                                    add.data[0] = 0x05;
                                    for (int j = 0; j < lyric.Length; j++) {
                                        add.data[j + 1] = lyric[j];
                                    }
                                    events.Add(add);
                                }
                            }

                            // vocaloid metatext
                            List<MidiEvent> meta;
                            if (mDialogMidiImportAndExport.isVocaloidMetatext() && i > 0) {
                                meta = vsq.generateMetaTextEvent(i, "Shift_JIS");
                            } else {
                                meta = new List<MidiEvent>();
                            }

                            // vocaloid nrpn
                            List<MidiEvent> vocaloid_nrpn_midievent;
                            if (mDialogMidiImportAndExport.isVocaloidNrpn() && i > 0) {
                                VsqNrpn[] vsqnrpn = VsqFileEx.generateNRPN((VsqFile)vsq, i, AppManager.editorConfig.PreSendTime);
                                NrpnData[] nrpn = VsqNrpn.convert(vsqnrpn);

                                vocaloid_nrpn_midievent = new List<MidiEvent>();
                                for (int j = 0; j < nrpn.Length; j++) {
                                    MidiEvent me = new MidiEvent();
                                    me.clock = nrpn[j].getClock();
                                    me.firstByte = 0xb0;
                                    me.data = new int[2];
                                    me.data[0] = nrpn[j].getParameter();
                                    me.data[1] = nrpn[j].Value;
                                    vocaloid_nrpn_midievent.Add(me);
                                }
                            } else {
                                vocaloid_nrpn_midievent = new List<MidiEvent>();
                            }
#if DEBUG
                            sout.println("menuFileExportMidi_Click");
                            sout.println("    vocaloid_nrpn_midievent.size()=" + vocaloid_nrpn_midievent.Count);
#endif

                            // midi eventを出力
                            events.Sort();
                            long last_clock = 0;
                            int events_count = events.Count;
                            if (events_count > 0) {
                                for (int j = 0; j < events_count; j++) {
                                    if (events[j].clock > 0 && meta.Count > 0) {
                                        for (int k = 0; k < meta.Count; k++) {
                                            VsqFile.writeFlexibleLengthUnsignedLong(fs, 0);
                                            meta[k].writeData(fs);
                                        }
                                        meta.Clear();
                                        last_clock = 0;
                                    }
                                    long clock = events[j].clock;
                                    while (vocaloid_nrpn_midievent.Count > 0 && vocaloid_nrpn_midievent[0].clock < clock) {
                                        VsqFile.writeFlexibleLengthUnsignedLong(fs, (long)(vocaloid_nrpn_midievent[0].clock - last_clock));
                                        last_clock = vocaloid_nrpn_midievent[0].clock;
                                        vocaloid_nrpn_midievent[0].writeData(fs);
                                        vocaloid_nrpn_midievent.RemoveAt(0);
                                    }
                                    VsqFile.writeFlexibleLengthUnsignedLong(fs, (long)(events[j].clock - last_clock));
                                    events[j].writeData(fs);
                                    last_clock = events[j].clock;
                                }
                            } else {
                                int c = vocaloid_nrpn_midievent.Count;
                                for (int k = 0; k < meta.Count; k++) {
                                    VsqFile.writeFlexibleLengthUnsignedLong(fs, 0);
                                    meta[k].writeData(fs);
                                }
                                meta.Clear();
                                last_clock = 0;
                                for (int j = 0; j < c; j++) {
                                    MidiEvent item = vocaloid_nrpn_midievent[j];
                                    long clock = item.clock;
                                    VsqFile.writeFlexibleLengthUnsignedLong(fs, (long)(clock - last_clock));
                                    item.writeData(fs);
                                    last_clock = clock;
                                }
                            }

                            // トラックエンドを記入し、
                            VsqFile.writeFlexibleLengthUnsignedLong(fs, (long)0);
                            fs.WriteByte((byte)0xff);
                            fs.WriteByte((byte)0x2f);
                            fs.WriteByte((byte)0x00);
                            // チャンクの先頭に戻ってチャンクのサイズを記入
                            long pos = fs.Position;
                            fs.Seek(first_position - 4, SeekOrigin.Begin);
                            VsqFile.writeUnsignedInt(fs, pos - first_position);
                            // ファイルを元の位置にseek
                            fs.Seek(pos, SeekOrigin.Begin);
                        }
                    } catch (Exception ex) {
                        Logger.write(typeof(FormMain) + ".menuFileExportMidi_Click; ex=" + ex + "\n");
                    } finally {
                        if (fs != null) {
                            try {
                                fs.Close();
                            } catch (Exception ex2) {
                                Logger.write(typeof(FormMain) + ".menuFileExportMidi_Click; ex=" + ex2 + "\n");
                            }
                        }
                    }
                }
            }
        }

        public void menuFileExportMusicXml_Click(Object sender, EventArgs e)
        {
            SaveFileDialog dialog = null;
            try {
                VsqFileEx vsq = AppManager.getVsqFile();
                if (vsq == null) {
                    return;
                }
                string first = AppManager.editorConfig.getLastUsedPathOut("xml");
                dialog = new SaveFileDialog();
                dialog.SetSelectedFile(first);
                dialog.Filter = string.Join("|", new[] { _("MusicXML(*.xml)|*.xml"), _("All Files(*.*)|*.*") });
                var result = AppManager.showModalDialog(dialog, false, this);
                if (result != System.Windows.Forms.DialogResult.OK) {
                    return;
                }
                string file = dialog.FileName;
                var writer = new MusicXmlWriter();
                writer.write(vsq, file);
                AppManager.editorConfig.setLastUsedPathOut(file, ".xml");
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuFileExportMusicXml_Click; ex=" + ex + "\n");
                serr.println("FormMain#menuFileExportMusicXml_Click; ex=" + ex);
            } finally {
                if (dialog != null) {
                    try {
                        dialog.Dispose();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".menuFileExportMusicXml_Click; ex=" + ex2 + "\n");
                        serr.println("FormMain#menuFileExportMusicXml_Click; ex2=" + ex2);
                    }
                }
            }
        }

        public void menuFileExportParaWave_Click(Object sender, EventArgs e)
        {
            // 出力するディレクトリを選択
            string dir = "";
            FolderBrowserDialog file_dialog = null;
            try {
                file_dialog = new FolderBrowserDialog();
                string initial_dir = AppManager.editorConfig.getLastUsedPathOut("wav");
                file_dialog.Description = _("Choose destination directory");
                file_dialog.SelectedPath = initial_dir;
                DialogResult ret = AppManager.showModalDialog(file_dialog, this);
                if (ret != DialogResult.OK) {
                    return;
                }
                dir = file_dialog.SelectedPath;
                // 1.wavはダミー
                initial_dir = Path.Combine(dir, "1.wav");
                AppManager.editorConfig.setLastUsedPathOut(initial_dir, ".wav");
            } catch (Exception ex) {
            } finally {
                if (file_dialog != null) {
                    try {
                        file_dialog.Dispose();
                    } catch (Exception ex2) {
                    }
                }
            }

            // 全部レンダリング済みの状態にするためのキュー
            VsqFileEx vsq = AppManager.getVsqFile();
            List<int> tracks = new List<int>();
            int size = vsq.Track.Count;
            for (int i = 1; i < size; i++) {
                tracks.Add(i);
            }
            List<PatchWorkQueue> queue = AppManager.patchWorkCreateQueue(tracks);

            // 全トラックをファイルに出力するためのキュー
            int clockStart = vsq.config.StartMarkerEnabled ? vsq.config.StartMarker : 0;
            int clockEnd = vsq.config.EndMarkerEnabled ? vsq.config.EndMarker : vsq.TotalClocks + 240;
            if (clockStart > clockEnd) {
                AppManager.showMessageBox(
                    _("invalid rendering region; start>=end"),
                    _("Error"),
                    PortUtil.OK_OPTION,
                    cadencii.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE);
                return;
            }
            for (int i = 1; i < size; i++) {
                PatchWorkQueue q = new PatchWorkQueue();
                q.track = i;
                q.clockStart = clockStart;
                q.clockEnd = clockEnd;
                q.file = Path.Combine(dir, i + ".wav");
                q.renderAll = true;
                q.vsq = vsq;
                queue.Add(q);
            }

            // 合成ダイアログを出す
            FormWorker fw = null;
            try {
                fw = new FormWorker();
                fw.setupUi(new FormWorkerUi(fw));
                fw.getUi().setTitle(_("Synthesize"));
                fw.getUi().setText(_("now synthesizing..."));

                SynthesizeWorker worker = new SynthesizeWorker(this);

                for (int i = 0; i < queue.Count; i++) {
                    PatchWorkQueue q = queue[i];
                    fw.addJob(worker, "processQueue", q.getMessage(), q.getJobAmount(), q);
                }

                fw.startJob();
                AppManager.showModalDialog(fw.getUi(), this);
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuFileExportParaWave; ex=" + ex + "\n");
            } finally {
                if (fw != null) {
                    try {
                        fw.getUi().close();
                    } catch (Exception ex2) {
                    }
                }
            }
        }

        public void menuFileExportUst_Click(Object sender, EventArgs e)
        {
            VsqFileEx vsq = (VsqFileEx)AppManager.getVsqFile().clone();

            // どのトラックを出力するか決める
            int selected = AppManager.getSelected();

            // 出力先のファイル名を選ぶ
            SaveFileDialog dialog = null;
            var dialog_result = DialogResult.Cancel;
            string file_name = "";
            try {
                string last_path = AppManager.editorConfig.getLastUsedPathOut("ust");
                dialog = new SaveFileDialog();
                dialog.SetSelectedFile(last_path);
                dialog.Title = _("Export UTAU (*.ust)");
                dialog.Filter = string.Join("|", new[] { _("UTAU Script Format(*.ust)|*.ust"), _("All Files(*.*)|*.*") });
                dialog_result = AppManager.showModalDialog(dialog, false, this);
                if (dialog_result != System.Windows.Forms.DialogResult.OK) {
                    return;
                }
                file_name = dialog.FileName;
                AppManager.editorConfig.setLastUsedPathOut(file_name, ".ust");
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuFileExportUst_Click; ex=" + ex + "\n");
            } finally {
                if (dialog != null) {
                    try {
                        dialog.Dispose();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".menuFileExportUst_Click; ex=" + ex2 + "\n");
                    }
                }
            }

            // 出力処理
            vsq.removePart(0, vsq.getPreMeasureClocks());
            UstFile ust = new UstFile(vsq, selected);
            // voice dirを設定
            VsqTrack vsq_track = vsq.Track[selected];
            VsqEvent singer = vsq_track.getSingerEventAt(0);
            string voice_dir = "";
            if (singer != null) {
                int program = singer.ID.IconHandle.Program;
                int size = AppManager.editorConfig.UtauSingers.Count;
                if (0 <= program && program < size) {
                    SingerConfig cfg = AppManager.editorConfig.UtauSingers[program];
                    voice_dir = cfg.VOICEIDSTR;
                }
            }
            ust.setVoiceDir(voice_dir);
            ust.setWavTool(AppManager.editorConfig.PathWavtool);
            int resampler_index = VsqFileEx.getTrackResamplerUsed(vsq_track);
            if (0 <= resampler_index && resampler_index < AppManager.editorConfig.getResamplerCount()) {
                ust.setResampler(
                    AppManager.editorConfig.getResamplerAt(resampler_index));
            }
            ust.write(file_name);
        }

        public void menuFileExportVsq_Click(Object sender, EventArgs e)
        {
            VsqFileEx vsq = AppManager.getVsqFile();

            // 出力先のファイル名を選ぶ
            SaveFileDialog dialog = null;
            var dialog_result = DialogResult.Cancel;
            string file_name = "";
            try {
                string last_path = AppManager.editorConfig.getLastUsedPathOut("vsq");
                dialog = new SaveFileDialog();
                dialog.SetSelectedFile(last_path);
                dialog.Title = _("Export VSQ (*.vsq)");
                dialog.Filter = string.Join("|", new[] { _("VSQ Format(*.vsq)|*.vsq"), _("All Files(*.*)|*.*") });
                dialog_result = AppManager.showModalDialog(dialog, false, this);
                if (dialog_result != System.Windows.Forms.DialogResult.OK) {
                    return;
                }
                file_name = dialog.FileName;
                AppManager.editorConfig.setLastUsedPathOut(file_name, ".vsq");
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuFileExportVsq_Click; ex=" + ex + "\n");
            } finally {
                if (dialog != null) {
                    try {
                        dialog.Dispose();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".menuFileExportVsq_Click; ex=" + ex2 + "\n");
                    }
                }
            }

            // 出力処理
            VsqFile tvsq = (VsqFile)vsq;
            tvsq.write(file_name, AppManager.editorConfig.PreSendTime, "Shift_JIS");
        }

        private void menuFileExportVsqx_Click(object sender, EventArgs e)
        {
            VsqFileEx sequence = AppManager.getVsqFile();
            using (var dialog = new System.Windows.Forms.SaveFileDialog()) {
                dialog.Title = _("Export VSQX (*.vsqx)");
                dialog.Filter = _("VSQX Format(*.vsqx)|*.vsqx") + "|" + _("All Files(*.*)|*.*");
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    var file_path = dialog.FileName;
                    var writer = new VsqxWriter();
                    writer.write(sequence, file_path);
                }
            }
        }

        public void menuFileExportVxt_Click(Object sender, EventArgs e)
        {
            // UTAUの歌手が登録されていない場合は警告を表示
            if (AppManager.editorConfig.UtauSingers.Count <= 0) {
                DialogResult dr = AppManager.showMessageBox(
                    _("UTAU singer not registered yet.\nContinue ?"),
                    _("Info"),
                    cadencii.windows.forms.Utility.MSGBOX_YES_NO_OPTION,
                    cadencii.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE);
                if (dr != DialogResult.Yes) {
                    return;
                }
            }

            VsqFileEx vsq = AppManager.getVsqFile();

            // 出力先のファイル名を選ぶ
            SaveFileDialog dialog = null;
            var dialog_result = DialogResult.Cancel;
            string file_name = "";
            try {
                string last_path = AppManager.editorConfig.getLastUsedPathOut("txt");
                dialog = new SaveFileDialog();
                dialog.SetSelectedFile(last_path);
                dialog.Title = _("Metatext for vConnect");
                dialog.Filter = string.Join("|", new[] { _("Text File(*.txt)|*.txt"), _("All Files(*.*)|*.*") });
                dialog_result = AppManager.showModalDialog(dialog, false, this);
                if (dialog_result != System.Windows.Forms.DialogResult.OK) {
                    return;
                }
                file_name = dialog.FileName;
                AppManager.editorConfig.setLastUsedPathOut(file_name, ".txt");
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuFileExportVxt_Click; ex=" + ex + "\n");
            } finally {
                if (dialog != null) {
                    try {
                        dialog.Dispose();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".menuFileExportVxt_Click; ex=" + ex2 + "\n");
                    }
                }
            }

            // 出力処理
            int selected = AppManager.getSelected();
            VsqTrack vsq_track = vsq.Track[selected];
            StreamWriter bw = null;
            try {
                bw = new StreamWriter(file_name, false, new UTF8Encoding(false));
                string oto_ini = AppManager.editorConfig.UtauSingers[0].VOICEIDSTR;
                // 先頭に登録されている歌手変更を検出
                VsqEvent singer = null;
                int c = vsq_track.getEventCount();
                for (int i = 0; i < c; i++) {
                    VsqEvent itemi = vsq_track.getEvent(i);
                    if (itemi.ID.type == VsqIDType.Singer) {
                        singer = itemi;
                        break;
                    }
                }
                // 歌手のプログラムチェンジから，歌手の原音設定へのパスを取得する
                if (singer != null) {
                    int indx = singer.ID.IconHandle.Program;
                    if (0 <= indx && indx < AppManager.editorConfig.UtauSingers.Count) {
                        oto_ini = AppManager.editorConfig.UtauSingers[indx].VOICEIDSTR;
                    }
                }

                // oto.iniで終わってる？
                if (!oto_ini.EndsWith("oto.ini")) {
                    oto_ini = Path.Combine(oto_ini, "oto.ini");
                }

                // 出力
                VConnectWaveGenerator.prepareMetaText(
                    bw, vsq_track, oto_ini, vsq.TotalClocks, false);
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuFileExportVxt_Click; ex=" + ex + "\n");
                serr.println(typeof(FormMain) + ".menuFileExportVxt_Click; ex=" + ex);
            } finally {
                if (bw != null) {
                    try {
                        bw.Close();
                    } catch (Exception ex2) {
                    }
                }
            }
        }

        public void menuFileExportWave_Click(Object sender, EventArgs e)
        {
            var dialog_result = DialogResult.Cancel;
            string filename = "";
            SaveFileDialog sfd = null;
            try {
                string last_path = AppManager.editorConfig.getLastUsedPathOut("wav");
#if DEBUG
                sout.println("FormMain#menuFileExportWave_Click; last_path=" + last_path);
#endif
                sfd = new SaveFileDialog();
                sfd.SetSelectedFile(last_path);
                sfd.Title = _("Wave Export");
                sfd.Filter = string.Join("|", new[] { _("Wave File(*.wav)|*.wav"), _("All Files(*.*)|*.*") });
                dialog_result = AppManager.showModalDialog(sfd, false, this);
                if (dialog_result != System.Windows.Forms.DialogResult.OK) {
                    return;
                }
                filename = sfd.FileName;
                AppManager.editorConfig.setLastUsedPathOut(filename, ".wav");
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuFileExportWave_Click; ex=" + ex + "\n");
            } finally {
                if (sfd != null) {
                    try {
                        sfd.Dispose();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".menuFileExportWave_Click; ex=" + ex2 + "\n");
                    }
                }
            }

            VsqFileEx vsq = AppManager.getVsqFile();
            int clockStart = vsq.config.StartMarkerEnabled ? vsq.config.StartMarker : 0;
            int clockEnd = vsq.config.EndMarkerEnabled ? vsq.config.EndMarker : vsq.TotalClocks + 240;
            if (clockStart > clockEnd) {
                AppManager.showMessageBox(
                    _("invalid rendering region; start>=end"),
                    _("Error"),
                    PortUtil.OK_OPTION,
                    cadencii.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE);
                return;
            }
            List<int> other_tracks = new List<int>();
            int selected = AppManager.getSelected();
            for (int i = 1; i < vsq.Track.Count; i++) {
                if (i != selected) {
                    other_tracks.Add(i);
                }
            }
            List<PatchWorkQueue> queue =
                AppManager.patchWorkCreateQueue(other_tracks);
            PatchWorkQueue q = new PatchWorkQueue();
            q.track = selected;
            q.clockStart = clockStart;
            q.clockEnd = clockEnd;
            q.file = filename;
            q.renderAll = true;
            q.vsq = vsq;
            // 末尾に追加
            queue.Add(q);
            double started = PortUtil.getCurrentTime();

            FormWorker fs = null;
            try {
                fs = new FormWorker();
                fs.setupUi(new FormWorkerUi(fs));
                fs.getUi().setTitle(_("Synthesize"));
                fs.getUi().setText(_("now synthesizing..."));

                SynthesizeWorker worker = new SynthesizeWorker(this);

                foreach (PatchWorkQueue qb in queue) {
                    fs.addJob(worker, "processQueue", qb.getMessage(), qb.getJobAmount(), qb);
                }

                fs.startJob();
                AppManager.showModalDialog(fs.getUi(), this);
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuFileExportWave_Click; ex=" + ex + "\n");
            } finally {
                if (fs != null) {
                    try {
                        fs.getUi().close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".menuFileExportWave_Click; ex=" + ex2 + "\n");
                    }
                }
            }
        }

        public void menuFileImportMidi_Click(Object sender, EventArgs e)
        {
            if (mDialogMidiImportAndExport == null) {
                mDialogMidiImportAndExport = new FormMidiImExport();
            }
            mDialogMidiImportAndExport.listTrack.Items.Clear();
            mDialogMidiImportAndExport.setMode(FormMidiImExport.FormMidiMode.IMPORT);

            string dir = AppManager.editorConfig.getLastUsedPathIn("mid");
            openMidiDialog.SetSelectedFile(dir);
            var dialog_result = AppManager.showModalDialog(openMidiDialog, true, this);

            if (dialog_result != System.Windows.Forms.DialogResult.OK) {
                return;
            }
            mDialogMidiImportAndExport.Location = getFormPreferedLocation(mDialogMidiImportAndExport);
            MidiFile mf = null;
            try {
                string filename = openMidiDialog.FileName;
                AppManager.editorConfig.setLastUsedPathIn(filename, ".mid");
                mf = new MidiFile(filename);
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuFileImportMidi_Click; ex=" + ex + "\n");
                AppManager.showMessageBox(
                    _("Invalid MIDI file."),
                    _("Error"),
                    cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                    cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE);
                return;
            }
            if (mf == null) {
                AppManager.showMessageBox(
                    _("Invalid MIDI file."),
                    _("Error"),
                    cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                    cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE);
                return;
            }
            int count = mf.getTrackCount();

            Func<int[], string> get_string_from_metatext = (buffer) => {
                var encoding_candidates = new List<Encoding>();
                encoding_candidates.Add(Encoding.GetEncoding("Shift_JIS"));
                encoding_candidates.Add(Encoding.Default);
                encoding_candidates.Add(Encoding.UTF8);
                encoding_candidates.AddRange(Encoding.GetEncodings().Select((encoding) => encoding.GetEncoding()));
                foreach (var encoding in encoding_candidates) {
                    try {
                        return encoding.GetString(buffer.Select((b) => (byte)(0xFF & b)).ToArray(), 0, buffer.Length);
                    } catch {
                        continue;
                    }
                }
                return string.Empty;
            };

            for (int i = 0; i < count; i++) {
                int notes = 0;
                List<MidiEvent> events = mf.getMidiEventList(i);
                int events_count = events.Count;

                // トラック名を取得
                string track_name =
                    events
                        .Where((item) => item.firstByte == 0xff && item.data.Length >= 2 && item.data[0] == 0x03)
                        .Select((item) => {
                            int[] d = item.data.Skip(1).ToArray();
                            return get_string_from_metatext(d);
                        })
                        .FirstOrDefault();

                // イベント数を数える
                for (int j = 0; j < events_count; j++) {
                    MidiEvent item = events[j];
                    if ((item.firstByte & 0xf0) == 0x90 && item.data.Length > 1 && item.data[1] > 0x00) {
                        notes++;
                    }
                }
                mDialogMidiImportAndExport.listTrack.AddRow(
                    new string[] { i + "", track_name, notes + "" }, true);
            }

            DialogResult dr = AppManager.showModalDialog(mDialogMidiImportAndExport, this);
            if (dr != DialogResult.OK) {
                return;
            }

            bool secondBasis = mDialogMidiImportAndExport.isSecondBasis();
            int offsetClocks = mDialogMidiImportAndExport.getOffsetClocks();
            double offsetSeconds = mDialogMidiImportAndExport.getOffsetSeconds();
            bool importFromPremeasure = mDialogMidiImportAndExport.isPreMeasure();

            // インポートするしないにかかわらずテンポと拍子を取得
            VsqFileEx tempo = new VsqFileEx("Miku", 2, 4, 4, 500000); //テンポリスト用のVsqFile。テンポの部分のみ使用
            tempo.executeCommand(VsqCommand.generateCommandChangePreMeasure(0));
            bool tempo_added = false;
            bool timesig_added = false;
            tempo.TempoTable.Clear();
            tempo.TimesigTable.Clear();
            int mf_getTrackCount = mf.getTrackCount();
            for (int i = 0; i < mf_getTrackCount; i++) {
                List<MidiEvent> events = mf.getMidiEventList(i);
                bool t_tempo_added = false;   //第iトラックからテンポをインポートしたかどうか
                bool t_timesig_added = false; //第iトラックから拍子をインポートしたかどうか
                int last_timesig_clock = 0; // 最後に拍子変更を検出したゲートタイム
                int last_num = 4; // 最後に検出した拍子変更の分子
                int last_den = 4; // 最後に検出した拍子変更の分母
                int last_barcount = 0;
                int events_Count = events.Count;
                for (int j = 0; j < events_Count; j++) {
                    MidiEvent itemj = events[j];
                    if (!tempo_added && itemj.firstByte == 0xff && itemj.data.Length >= 4 && itemj.data[0] == 0x51) {
                        bool contains_same_clock = false;
                        int size = tempo.TempoTable.Count;
                        // 同時刻のテンポ変更は、最初以外無視する
                        for (int k = 0; k < size; k++) {
                            if (tempo.TempoTable[k].Clock == itemj.clock) {
                                contains_same_clock = true;
                                break;
                            }
                        }
                        if (!contains_same_clock) {
                            int vtempo = itemj.data[1] << 16 | itemj.data[2] << 8 | itemj.data[3];
                            tempo.TempoTable.Add(new TempoTableEntry((int)itemj.clock, vtempo, 0.0));
                            t_tempo_added = true;
                        }
                    }
                    if (!timesig_added && itemj.firstByte == 0xff && itemj.data.Length >= 5 && itemj.data[0] == 0x58) {
                        int num = itemj.data[1];
                        int den = 1;
                        for (int k = 0; k < itemj.data[2]; k++) {
                            den = den * 2;
                        }
                        int clock_per_bar = last_num * 480 * 4 / last_den;
                        int barcount_at_itemj = last_barcount + ((int)itemj.clock - last_timesig_clock) / clock_per_bar;
                        // 同時刻の拍子変更は、最初以外無視する
                        int size = tempo.TimesigTable.Count;
                        bool contains_same_clock = false;
                        for (int k = 0; k < size; k++) {
                            if (tempo.TimesigTable[k].Clock == itemj.clock) {
                                contains_same_clock = true;
                                break;
                            }
                        }
                        if (!contains_same_clock) {
                            tempo.TimesigTable.Add(new TimeSigTableEntry((int)itemj.clock, num, den, barcount_at_itemj));
                            last_timesig_clock = (int)itemj.clock;
                            last_den = den;
                            last_num = num;
                            last_barcount = barcount_at_itemj;
                            t_timesig_added = true;
                        }
                    }
                }
                if (t_tempo_added) {
                    tempo_added = true;
                }
                if (t_timesig_added) {
                    timesig_added = true;
                }
                if (timesig_added && tempo_added) {
                    // 両方ともインポート済みならexit。2個以上のトラックから、重複してテンポや拍子をインポートするのはNG（たぶん）
                    break;
                }
            }
            bool contains_zero = false;
            int c = tempo.TempoTable.Count;
            for (int i = 0; i < c; i++) {
                if (tempo.TempoTable[i].Clock == 0) {
                    contains_zero = true;
                    break;
                }
            }
            if (!contains_zero) {
                tempo.TempoTable.Add(new TempoTableEntry(0, 500000, 0.0));
            }
            contains_zero = false;
            // =>
            // Thanks, げっぺータロー.
            // BEFORE:
            // c = tempo.TempoTable.size();
            // AFTER:
            c = tempo.TimesigTable.Count;
            // <=
            for (int i = 0; i < c; i++) {
                if (tempo.TimesigTable[i].Clock == 0) {
                    contains_zero = true;
                    break;
                }
            }
            if (!contains_zero) {
                tempo.TimesigTable.Add(new TimeSigTableEntry(0, 4, 4, 0));
            }
            VsqFileEx work = (VsqFileEx)AppManager.getVsqFile().clone(); //後でReplaceコマンドを発行するための作業用
            int preMeasureClocks = work.getPreMeasureClocks();
            double sec_at_premeasure = work.getSecFromClock(preMeasureClocks);
            if (!mDialogMidiImportAndExport.isPreMeasure()) {
                sec_at_premeasure = 0.0;
            }
            VsqFileEx copy_src = (VsqFileEx)tempo.clone();
            if (sec_at_premeasure != 0.0) {
                int t = work.TempoTable[0].Tempo;
                VsqFileEx.shift(copy_src, sec_at_premeasure, t);
            }
            tempo.updateTempoInfo();
            tempo.updateTimesigInfo();

            // tempoをインポート
            bool import_tempo = mDialogMidiImportAndExport.isTempo();
            if (import_tempo) {
#if DEBUG
                sout.println("FormMain#menuFileImportMidi_Click; sec_at_premeasure=" + sec_at_premeasure);
#endif
                // 最初に、workにある全てのイベント・コントロールカーブ・ベジエ曲線をtempoのテンポテーブルに合うように、シフトする
                //ShiftClockToMatchWith( work, copy_src, work.getSecFromClock( work.getPreMeasureClocks() ) );
                //ShiftClockToMatchWith( work, copy_src, copy_src.getSecFromClock( copy_src.getPreMeasureClocks() ) );
                if (secondBasis) {
                    shiftClockToMatchWith(work, copy_src, sec_at_premeasure);
                }

                work.TempoTable.Clear();
                List<TempoTableEntry> list = copy_src.TempoTable;
                int list_count = list.Count;
                for (int i = 0; i < list_count; i++) {
                    TempoTableEntry item = list[i];
                    work.TempoTable.Add(new TempoTableEntry(item.Clock, item.Tempo, item.Time));
                }
                work.updateTempoInfo();
            }

            // timesig
            if (mDialogMidiImportAndExport.isTimesig()) {
                work.TimesigTable.Clear();
                List<TimeSigTableEntry> list = tempo.TimesigTable;
                int list_count = list.Count;
                for (int i = 0; i < list_count; i++) {
                    TimeSigTableEntry item = list[i];
                    work.TimesigTable.Add(
                        new TimeSigTableEntry(
                            item.Clock,
                            item.Numerator,
                            item.Denominator,
                            item.BarCount));
                }
                work.TimesigTable.Sort();
                work.updateTimesigInfo();
            }

            for (int i = 0; i < mDialogMidiImportAndExport.listTrack.Items.Count; i++) {
                if (!mDialogMidiImportAndExport.listTrack.Items[i].Checked) {
                    continue;
                }
                if (work.Track.Count + 1 > AppManager.MAX_NUM_TRACK) {
                    break;
                }
                VsqTrack work_track = new VsqTrack(mDialogMidiImportAndExport.listTrack.Items[i].SubItems[1].Text, "Miku");

                // デフォルトの音声合成システムに切り替え
                RendererKind kind = AppManager.editorConfig.DefaultSynthesizer;
                string renderer = kind.getVersionString();
                List<VsqID> singers = AppManager.getSingerListFromRendererKind(kind);
                work_track.changeRenderer(renderer, singers);

                List<MidiEvent> events = mf.getMidiEventList(i);
                events.Sort();
                int events_count = events.Count;

                // note
                if (mDialogMidiImportAndExport.isNotes()) {
                    int[] onclock_each_note = new int[128];
                    int[] velocity_each_note = new int[128];
                    for (int j = 0; j < 128; j++) {
                        onclock_each_note[j] = -1;
                        velocity_each_note[j] = 64;
                    }
                    int last_note = -1;
                    for (int j = 0; j < events_count; j++) {
                        MidiEvent itemj = events[j];
                        int not_closed_note = -1;
                        if ((itemj.firstByte & 0xf0) == 0x90 && itemj.data.Length >= 2 && itemj.data[1] > 0) {
                            for (int m = 0; m < 128; m++) {
                                if (onclock_each_note[m] >= 0) {
                                    not_closed_note = m;
                                    break;
                                }
                            }
                        }
#if DEBUG
                        sout.println("FormMain#menuFileImprotMidi_Click; not_closed_note=" + not_closed_note);
#endif
                        if (((itemj.firstByte & 0xf0) == 0x90 && itemj.data.Length >= 2 && itemj.data[1] == 0) ||
                             ((itemj.firstByte & 0xf0) == 0x80 && itemj.data.Length >= 2) ||
                             not_closed_note >= 0) {
                            int clock_off = (int)itemj.clock;
                            int note = (int)itemj.data[0];
                            if (not_closed_note >= 0) {
                                note = not_closed_note;
                            }
                            if (onclock_each_note[note] >= 0) {
                                int add_clock_on = onclock_each_note[note];
                                int add_clock_off = clock_off;
                                if (secondBasis) {
                                    double time_clock_on = tempo.getSecFromClock(onclock_each_note[note]) + sec_at_premeasure + offsetSeconds;
                                    double time_clock_off = tempo.getSecFromClock(clock_off) + sec_at_premeasure + offsetSeconds;
                                    add_clock_on = (int)work.getClockFromSec(time_clock_on);
                                    add_clock_off = (int)work.getClockFromSec(time_clock_off);
                                } else {
                                    add_clock_on += (importFromPremeasure ? preMeasureClocks : 0) + offsetClocks;
                                    add_clock_off += (importFromPremeasure ? preMeasureClocks : 0) + offsetClocks;
                                }
                                if (add_clock_on < 0) {
                                    add_clock_on = 0;
                                }
                                if (add_clock_off < 0) {
                                    continue;
                                }
                                VsqID vid = new VsqID(0);
                                vid.type = VsqIDType.Anote;
                                vid.setLength(add_clock_off - add_clock_on);
#if DEBUG
                                sout.println("FormMain#menuFileImportMidi_Click; vid.Length=" + vid.getLength());
#endif
                                string phrase = "a";
                                if (mDialogMidiImportAndExport.isLyric()) {
                                    for (int k = 0; k < events_count; k++) {
                                        MidiEvent itemk = events[k];
                                        if (onclock_each_note[note] <= (int)itemk.clock && (int)itemk.clock <= clock_off) {
                                            if (itemk.firstByte == 0xff && itemk.data.Length >= 2 && itemk.data[0] == 0x05) {
                                                int[] d = new int[itemk.data.Length - 1];
                                                for (int m = 1; m < itemk.data.Length; m++) {
                                                    d[m - 1] = 0xff & itemk.data[m];
                                                }
                                                phrase = get_string_from_metatext(d);
                                                break;
                                            }
                                        }
                                    }
                                }
                                vid.LyricHandle = new LyricHandle(phrase, "a");
                                vid.Note = note;
                                vid.Dynamics = velocity_each_note[note];
                                // デフォルとの歌唱スタイルを適用する
                                AppManager.editorConfig.applyDefaultSingerStyle(vid);

                                // ビブラート
                                if (AppManager.editorConfig.EnableAutoVibrato) {
                                    int note_length = vid.getLength();
                                    // 音符位置での拍子を調べる
                                    Timesig timesig = work.getTimesigAt(add_clock_on);

                                    // ビブラートを自動追加するかどうかを決める閾値
                                    int threshold = AppManager.editorConfig.AutoVibratoThresholdLength;
                                    if (note_length >= threshold) {
                                        int vibrato_clocks = 0;
                                        DefaultVibratoLengthEnum vib_length = AppManager.editorConfig.DefaultVibratoLength;
                                        if (vib_length == DefaultVibratoLengthEnum.L100) {
                                            vibrato_clocks = note_length;
                                        } else if (vib_length == DefaultVibratoLengthEnum.L50) {
                                            vibrato_clocks = note_length / 2;
                                        } else if (vib_length == DefaultVibratoLengthEnum.L66) {
                                            vibrato_clocks = note_length * 2 / 3;
                                        } else if (vib_length == DefaultVibratoLengthEnum.L75) {
                                            vibrato_clocks = note_length * 3 / 4;
                                        }
                                        // とりあえずVOCALOID2のデフォルトビブラートの設定を使用
                                        vid.VibratoHandle = AppManager.editorConfig.createAutoVibrato(SynthesizerType.VOCALOID2, vibrato_clocks);
                                        vid.VibratoDelay = note_length - vibrato_clocks;
                                    }
                                }

                                VsqEvent ve = new VsqEvent(add_clock_on, vid);
                                work_track.addEvent(ve);
                                onclock_each_note[note] = -1;
                            }
                        }
                        if ((itemj.firstByte & 0xf0) == 0x90 && itemj.data.Length >= 2 && itemj.data[1] > 0) {
                            int note = itemj.data[0];
                            onclock_each_note[note] = (int)itemj.clock;
                            int vel = itemj.data[1];
                            velocity_each_note[note] = vel;
                            last_note = note;
                        }
                    }

                    int track = work.Track.Count;
                    CadenciiCommand run_add =
                        VsqFileEx.generateCommandAddTrack(
                            work_track,
                            new VsqMixerEntry(0, 0, 0, 0),
                            track,
                            new BezierCurves());
                    work.executeCommand(run_add);
                }
            }

            CadenciiCommand lastrun = VsqFileEx.generateCommandReplace(work);
            AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(lastrun));
            setEdited(true);
            refreshScreen();
        }

        public void menuFileImportUst_Click(Object sender, EventArgs e)
        {
            OpenFileDialog dialog = null;
            try {
                // 読み込むファイルを選ぶ
                string dir = AppManager.editorConfig.getLastUsedPathIn("ust");
                dialog = new OpenFileDialog();
                dialog.SetSelectedFile(dir);
                var dialog_result = AppManager.showModalDialog(dialog, true, this);
                if (dialog_result != DialogResult.OK) {
                    return;
                }
                string file = dialog.FileName;
                AppManager.editorConfig.setLastUsedPathIn(file, ".ust");

                // ustを読み込む
                UstFile ust = new UstFile(file);

                // vsqに変換
                VsqFile vsq = new VsqFile(ust);
                vsq.insertBlank(0, vsq.getPreMeasureClocks());

                // RendererKindをUTAUに指定
                for (int i = 1; i < vsq.Track.Count; i++) {
                    VsqTrack vsq_track = vsq.Track[i];
                    VsqFileEx.setTrackRendererKind(vsq_track, RendererKind.UTAU);
                }

                // unknownな歌手とresamplerを何とかする
                ByRef<string> ref_resampler = new ByRef<string>(ust.getResampler());
                ByRef<string> ref_singer = new ByRef<string>(ust.getVoiceDir());
                checkUnknownResamplerAndSinger(ref_resampler, ref_singer);

                // 歌手変更を何とかする
                int program = 0;
                for (int i = 0; i < AppManager.editorConfig.UtauSingers.Count; i++) {
                    SingerConfig sc = AppManager.editorConfig.UtauSingers[i];
                    if (sc == null) {
                        continue;
                    }
                    if (sc.VOICEIDSTR == ref_singer.value) {
                        program = i;
                        break;
                    }
                }
                // 歌手変更のテンプレートを作成
                VsqID singer_id = Utility.getSingerID(RendererKind.UTAU, program, 0);
                if (singer_id == null) {
                    singer_id = new VsqID();
                    singer_id.type = VsqIDType.Singer;
                    singer_id.IconHandle = new IconHandle();
                    singer_id.IconHandle.Program = program;
                    singer_id.IconHandle.IconID = "$0401" + PortUtil.toHexString(0, 4);
                }
                // トラックの歌手変更イベントをすべて置き換える
                for (int i = 1; i < vsq.Track.Count; i++) {
                    VsqTrack vsq_track = vsq.Track[i];
                    int c = vsq_track.getEventCount();
                    for (int j = 0; j < c; j++) {
                        VsqEvent itemj = vsq_track.getEvent(j);
                        if (itemj.ID.type == VsqIDType.Singer) {
                            itemj.ID = (VsqID)singer_id.clone();
                        }
                    }
                }

                // resamplerUsedを更新(可能なら)
                for (int j = 1; j < vsq.Track.Count; j++) {
                    VsqTrack vsq_track = vsq.Track[j];
                    for (int i = 0; i < AppManager.editorConfig.getResamplerCount(); i++) {
                        string resampler = AppManager.editorConfig.getResamplerAt(i);
                        if (resampler == ref_resampler.value) {
                            VsqFileEx.setTrackResamplerUsed(vsq_track, i);
                            break;
                        }
                    }
                }

                // 読込先のvsqと，インポートするvsqではテンポテーブルがずれているので，
                // 読み込んだ方のvsqの内容を，現在のvsqと合致するように編集する
                VsqFileEx dst = (VsqFileEx)AppManager.getVsqFile().clone();
                vsq.adjustClockToMatchWith(dst.TempoTable);

                // トラック数の上限になるまで挿入を実行
                int size = vsq.Track.Count;
                for (int i = 1; i < size; i++) {
                    if (dst.Track.Count + 1 >= VsqFile.MAX_TRACKS + 1) {
                        // トラック数の上限
                        break;
                    }
                    dst.Track.Add(vsq.Track[i]);
                    dst.AttachedCurves.add(new BezierCurves());
                    dst.Mixer.Slave.Add(new VsqMixerEntry());
                }

                // コマンドを発行して実行
                CadenciiCommand run = VsqFileEx.generateCommandReplace(dst);
                AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                AppManager.mMixerWindow.updateStatus();
                setEdited(true);
                refreshScreen(true);
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuFileImportUst_Click; ex=" + ex + "\t");
            } finally {
                if (dialog != null) {
                    try {
                        dialog.Dispose();
                    } catch (Exception ex) {
                    }
                }
            }
        }

        public void menuFileImportVsq_Click(Object sender, EventArgs e)
        {
            string dir = AppManager.editorConfig.getLastUsedPathIn(AppManager.editorConfig.LastUsedExtension);
            openMidiDialog.SetSelectedFile(dir);
            var dialog_result = AppManager.showModalDialog(openMidiDialog, true, this);

            if (dialog_result != System.Windows.Forms.DialogResult.OK) {
                return;
            }
            VsqFileEx vsq = null;
            string filename = openMidiDialog.FileName;
            AppManager.editorConfig.setLastUsedPathIn(filename, ".vsq");
            try {
                vsq = new VsqFileEx(filename, "Shift_JIS");
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuFileImportVsq_Click; ex=" + ex + "\n");
                AppManager.showMessageBox(_("Invalid VSQ/VOCALOID MIDI file"), _("Error"), cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION, cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE);
                return;
            }
            if (mDialogMidiImportAndExport == null) {
                mDialogMidiImportAndExport = new FormMidiImExport();
            }
            mDialogMidiImportAndExport.listTrack.Items.Clear();
            for (int track = 1; track < vsq.Track.Count; track++) {
                mDialogMidiImportAndExport.listTrack.AddRow(new string[] {
                    track + "",
                    vsq.Track[ track ].getName(),
                    vsq.Track[ track ].getEventCount() + "" }, true);
            }
            mDialogMidiImportAndExport.setMode(FormMidiImExport.FormMidiMode.IMPORT_VSQ);
            mDialogMidiImportAndExport.setTempo(false);
            mDialogMidiImportAndExport.setTimesig(false);
            mDialogMidiImportAndExport.Location = getFormPreferedLocation(mDialogMidiImportAndExport);
            DialogResult dr = AppManager.showModalDialog(mDialogMidiImportAndExport, this);
            if (dr != DialogResult.OK) {
                return;
            }

            List<int> add_track = new List<int>();
            for (int i = 0; i < mDialogMidiImportAndExport.listTrack.Items.Count; i++) {
                if (mDialogMidiImportAndExport.listTrack.Items[i].Checked) {
                    add_track.Add(i + 1);
                }
            }
            if (add_track.Count <= 0) {
                return;
            }

            VsqFileEx replace = (VsqFileEx)AppManager.getVsqFile().clone();
            double premeasure_sec_replace = replace.getSecFromClock(replace.getPreMeasureClocks());
            double premeasure_sec_vsq = vsq.getSecFromClock(vsq.getPreMeasureClocks());

            if (mDialogMidiImportAndExport.isTempo()) {
                shiftClockToMatchWith(replace, vsq, premeasure_sec_replace - premeasure_sec_vsq);
                // テンポテーブルを置き換え
                replace.TempoTable.Clear();
                for (int i = 0; i < vsq.TempoTable.Count; i++) {
                    replace.TempoTable.Add((TempoTableEntry)vsq.TempoTable[i].clone());
                }
                replace.updateTempoInfo();
                replace.updateTotalClocks();
            }

            if (mDialogMidiImportAndExport.isTimesig()) {
                // 拍子をリプレースする場合
                replace.TimesigTable.Clear();
                for (int i = 0; i < vsq.TimesigTable.Count; i++) {
                    replace.TimesigTable.Add((TimeSigTableEntry)vsq.TimesigTable[i].clone());
                }
                replace.updateTimesigInfo();
            }

            foreach (var track in add_track) {
                if (replace.Track.Count + 1 >= AppManager.MAX_NUM_TRACK) {
                    break;
                }
                if (!mDialogMidiImportAndExport.isTempo()) {
                    // テンポをリプレースしない場合。インポートするトラックのクロックを調節する
                    for (Iterator<VsqEvent> itr2 = vsq.Track[track].getEventIterator(); itr2.hasNext(); ) {
                        VsqEvent item = itr2.next();
                        if (item.ID.type == VsqIDType.Singer && item.Clock == 0) {
                            continue;
                        }
                        int clock = item.Clock;
                        double sec_start = vsq.getSecFromClock(clock) - premeasure_sec_vsq + premeasure_sec_replace;
                        double sec_end = vsq.getSecFromClock(clock + item.ID.getLength()) - premeasure_sec_vsq + premeasure_sec_replace;
                        int clock_start = (int)replace.getClockFromSec(sec_start);
                        int clock_end = (int)replace.getClockFromSec(sec_end);
                        item.Clock = clock_start;
                        item.ID.setLength(clock_end - clock_start);
                        if (item.ID.VibratoHandle != null) {
                            double sec_vib_start = vsq.getSecFromClock(clock + item.ID.VibratoDelay) - premeasure_sec_vsq + premeasure_sec_replace;
                            int clock_vib_start = (int)replace.getClockFromSec(sec_vib_start);
                            item.ID.VibratoDelay = clock_vib_start - clock_start;
                            item.ID.VibratoHandle.setLength(clock_end - clock_vib_start);
                        }
                    }

                    // コントロールカーブをシフト
                    foreach (CurveType ct in Utility.CURVE_USAGE) {
                        VsqBPList item = vsq.Track[track].getCurve(ct.getName());
                        if (item == null) {
                            continue;
                        }
                        VsqBPList repl = new VsqBPList(item.getName(), item.getDefault(), item.getMinimum(), item.getMaximum());
                        for (int i = 0; i < item.size(); i++) {
                            int clock = item.getKeyClock(i);
                            int value = item.getElement(i);
                            double sec = vsq.getSecFromClock(clock) - premeasure_sec_vsq + premeasure_sec_replace;
                            if (sec >= premeasure_sec_replace) {
                                int clock_new = (int)replace.getClockFromSec(sec);
                                repl.add(clock_new, value);
                            }
                        }
                        vsq.Track[track].setCurve(ct.getName(), repl);
                    }

                    // ベジエカーブをシフト
                    foreach (CurveType ct in Utility.CURVE_USAGE) {
                        List<BezierChain> list = vsq.AttachedCurves.get(track - 1).get(ct);
                        if (list == null) {
                            continue;
                        }
                        foreach (var chain in list) {
                            foreach (var point in chain.points) {
                                PointD bse = new PointD(replace.getClockFromSec(vsq.getSecFromClock(point.getBase().getX()) - premeasure_sec_vsq + premeasure_sec_replace),
                                                         point.getBase().getY());
                                PointD ctrl_r = new PointD(replace.getClockFromSec(vsq.getSecFromClock(point.controlLeft.getX()) - premeasure_sec_vsq + premeasure_sec_replace),
                                                            point.controlLeft.getY());
                                PointD ctrl_l = new PointD(replace.getClockFromSec(vsq.getSecFromClock(point.controlRight.getX()) - premeasure_sec_vsq + premeasure_sec_replace),
                                                            point.controlRight.getY());
                                point.setBase(bse);
                                point.controlLeft = ctrl_l;
                                point.controlRight = ctrl_r;
                            }
                        }
                    }
                }
                replace.Mixer.Slave.Add(new VsqMixerEntry());
                replace.Track.Add(vsq.Track[track]);
                replace.AttachedCurves.add(vsq.AttachedCurves.get(track - 1));
            }

            // コマンドを発行し、実行
            CadenciiCommand run = VsqFileEx.generateCommandReplace(replace);
            AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
            setEdited(true);
        }

        public void menuFileOpenUst_Click(Object sender, EventArgs e)
        {
            if (!dirtyCheck()) {
                return;
            }

            string dir = AppManager.editorConfig.getLastUsedPathIn("ust");
            openUstDialog.SetSelectedFile(dir);
            var dialog_result = AppManager.showModalDialog(openUstDialog, true, this);

            if (dialog_result != System.Windows.Forms.DialogResult.OK) {
                return;
            }

            try {
                string filename = openUstDialog.FileName;
                AppManager.editorConfig.setLastUsedPathIn(filename, ".ust");

                // ust読み込み
                UstFile ust = new UstFile(filename);

                // vsqに変換
                VsqFileEx vsq = new VsqFileEx(ust);
                vsq.insertBlank(0, vsq.getPreMeasureClocks());

                // すべてのトラックの合成器指定をUTAUにする
                for (int i = 1; i < vsq.Track.Count; i++) {
                    VsqTrack vsq_track = vsq.Track[i];
                    VsqFileEx.setTrackRendererKind(vsq_track, RendererKind.UTAU);
                }

                // unknownな歌手やresamplerを何とかする
                ByRef<string> ref_resampler = new ByRef<string>(ust.getResampler());
                ByRef<string> ref_singer = new ByRef<string>(ust.getVoiceDir());
                checkUnknownResamplerAndSinger(ref_resampler, ref_singer);

                // 歌手変更を何とかする
                int program = 0;
                for (int i = 0; i < AppManager.editorConfig.UtauSingers.Count; i++) {
                    SingerConfig sc = AppManager.editorConfig.UtauSingers[i];
                    if (sc == null) {
                        continue;
                    }
                    if (sc.VOICEIDSTR == ref_singer.value) {
                        program = i;
                        break;
                    }
                }
                // 歌手変更のテンプレートを作成
                VsqID singer_id = Utility.getSingerID(RendererKind.UTAU, program, 0);
                if (singer_id == null) {
                    singer_id = new VsqID();
                    singer_id.type = VsqIDType.Singer;
                    singer_id.IconHandle = new IconHandle();
                    singer_id.IconHandle.Program = program;
                    singer_id.IconHandle.IconID = "$0401" + PortUtil.toHexString(0, 4);
                }
                // トラックの歌手変更イベントをすべて置き換える
                for (int i = 1; i < vsq.Track.Count; i++) {
                    VsqTrack vsq_track = vsq.Track[i];
                    int c = vsq_track.getEventCount();
                    for (int j = 0; j < c; j++) {
                        VsqEvent itemj = vsq_track.getEvent(j);
                        if (itemj.ID.type == VsqIDType.Singer) {
                            itemj.ID = (VsqID)singer_id.clone();
                        }
                    }
                }

                // resamplerUsedを更新(可能なら)
                for (int j = 1; j < vsq.Track.Count; j++) {
                    VsqTrack vsq_track = vsq.Track[j];
                    for (int i = 0; i < AppManager.editorConfig.getResamplerCount(); i++) {
                        string resampler = AppManager.editorConfig.getResamplerAt(i);
                        if (resampler == ref_resampler.value) {
                            VsqFileEx.setTrackResamplerUsed(vsq_track, i);
                            break;
                        }
                    }
                }

                clearExistingData();
                AppManager.setVsqFile(vsq);
                setEdited(true);
                AppManager.mMixerWindow.updateStatus();
                clearTempWave();
                updateDrawObjectList();
                refreshScreen();

            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuFileOpenUst_Click; ex=" + ex + "\n");
#if DEBUG
                sout.println("FormMain#menuFileOpenUst_Click; ex=" + ex);
#endif
            }
        }

        public void menuFileOpenVsq_Click(Object sender, EventArgs e)
        {
            if (!dirtyCheck()) {
                return;
            }

            string[] filters = openMidiDialog.Filter.Split('|');
            int filter_index = -1;
            string filter = "";
            foreach (string f in filters) {
                ++filter_index;
                if (f.EndsWith(AppManager.editorConfig.LastUsedExtension)) {
                    break;
                }
            }

            openMidiDialog.FilterIndex = filter_index;
            string dir = AppManager.editorConfig.getLastUsedPathIn(filter);
            openMidiDialog.SetSelectedFile(dir);
            var dialog_result = AppManager.showModalDialog(openMidiDialog, true, this);
            string ext = ".vsq";
            if (dialog_result == System.Windows.Forms.DialogResult.OK) {
#if DEBUG
                AppManager.debugWriteLine("openMidiDialog.Filter=" + openMidiDialog.Filter);
#endif
                string selected_filter = openMidiDialog.SelectedFilter();
                if (selected_filter.EndsWith(".mid")) {
                    AppManager.editorConfig.LastUsedExtension = ".mid";
                    ext = ".mid";
                } else if (selected_filter.EndsWith(".vsq")) {
                    AppManager.editorConfig.LastUsedExtension = ".vsq";
                    ext = ".vsq";
                } else if (selected_filter.EndsWith(".vsqx")) {
                    AppManager.editorConfig.LastUsedExtension = ".vsqx";
                    ext = ".vsqx";
                }
            } else {
                return;
            }
            try {
                string filename = openMidiDialog.FileName;
                string actualReadFile = filename;
                bool isVsqx = filename.EndsWith(".vsqx");
                if (isVsqx) {
                    actualReadFile = PortUtil.createTempFile();
                    VsqFile temporarySequence = VsqxReader.readFromVsqx(filename);
                    temporarySequence.write(actualReadFile);
                }
                VsqFileEx vsq = new VsqFileEx(actualReadFile, "Shift_JIS");
                if (isVsqx) {
                    PortUtil.deleteFile(actualReadFile);
                }
                AppManager.editorConfig.setLastUsedPathIn(filename, ext);
                AppManager.setVsqFile(vsq);
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuFileOpenVsq_Click; ex=" + ex + "\n");
#if DEBUG
                sout.println("FormMain#menuFileOpenVsq_Click; ex=" + ex);
#endif
                AppManager.showMessageBox(
                    _("Invalid VSQ/VOCALOID MIDI file"),
                    _("Error"),
                    cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                    cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE);
                return;
            }
            AppManager.setSelected(1);
            clearExistingData();
            setEdited(true);
            AppManager.mMixerWindow.updateStatus();
            clearTempWave();
            updateDrawObjectList();
            refreshScreen();
        }
        #endregion

        //BOOKMARK: menuSetting
        #region menuSetting*
        public void menuSettingDefaultSingerStyle_Click(Object sender, EventArgs e)
        {
            FormSingerStyleConfig dlg = null;
            try {
                dlg = new FormSingerStyleConfig();
                dlg.setPMBendDepth(AppManager.editorConfig.DefaultPMBendDepth);
                dlg.setPMBendLength(AppManager.editorConfig.DefaultPMBendLength);
                dlg.setPMbPortamentoUse(AppManager.editorConfig.DefaultPMbPortamentoUse);
                dlg.setDEMdecGainRate(AppManager.editorConfig.DefaultDEMdecGainRate);
                dlg.setDEMaccent(AppManager.editorConfig.DefaultDEMaccent);

                int selected = AppManager.getSelected();
                dlg.Location = getFormPreferedLocation(dlg);
                DialogResult dr = AppManager.showModalDialog(dlg, this);
                if (dr == DialogResult.OK) {
                    AppManager.editorConfig.DefaultPMBendDepth = dlg.getPMBendDepth();
                    AppManager.editorConfig.DefaultPMBendLength = dlg.getPMBendLength();
                    AppManager.editorConfig.DefaultPMbPortamentoUse = dlg.getPMbPortamentoUse();
                    AppManager.editorConfig.DefaultDEMdecGainRate = dlg.getDEMdecGainRate();
                    AppManager.editorConfig.DefaultDEMaccent = dlg.getDEMaccent();
                    if (dlg.getApplyCurrentTrack()) {
                        VsqFileEx vsq = AppManager.getVsqFile();
                        VsqTrack vsq_track = vsq.Track[selected];
                        VsqTrack copy = (VsqTrack)vsq_track.clone();
                        bool changed = false;
                        for (int i = 0; i < copy.getEventCount(); i++) {
                            if (copy.getEvent(i).ID.type == VsqIDType.Anote) {
                                AppManager.editorConfig.applyDefaultSingerStyle(copy.getEvent(i).ID);
                                changed = true;
                            }
                        }
                        if (changed) {
                            CadenciiCommand run =
                                VsqFileEx.generateCommandTrackReplace(
                                    selected,
                                    copy,
                                    vsq.AttachedCurves.get(selected - 1));
                            AppManager.editHistory.register(vsq.executeCommand(run));
                            updateDrawObjectList();
                            refreshScreen();
                        }
                    }
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuSettingDefaultSingerStyle_Click; ex=" + ex + "\n");
            } finally {
                if (dlg != null) {
                    try {
                        dlg.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".menuSettingDefaultSingerStyle_Click; ex=" + ex2 + "\n");
                    }
                }
            }
        }

        public void menuSettingGameControlerLoad_Click(Object sender, EventArgs e)
        {
            loadGameControler();
        }

        public void menuSettingGameControlerRemove_Click(Object sender, EventArgs e)
        {
            removeGameControler();
        }

        public void menuSettingGameControlerSetting_Click(Object sender, EventArgs e)
        {
            FormGameControlerConfig dlg = null;
            try {
                dlg = new FormGameControlerConfig();
                dlg.Location = getFormPreferedLocation(dlg);
                DialogResult dr = AppManager.showModalDialog(dlg, this);
                if (dr == DialogResult.OK) {
                    AppManager.editorConfig.GameControlerRectangle = dlg.getRectangle();
                    AppManager.editorConfig.GameControlerTriangle = dlg.getTriangle();
                    AppManager.editorConfig.GameControlerCircle = dlg.getCircle();
                    AppManager.editorConfig.GameControlerCross = dlg.getCross();
                    AppManager.editorConfig.GameControlL1 = dlg.getL1();
                    AppManager.editorConfig.GameControlL2 = dlg.getL2();
                    AppManager.editorConfig.GameControlR1 = dlg.getR1();
                    AppManager.editorConfig.GameControlR2 = dlg.getR2();
                    AppManager.editorConfig.GameControlSelect = dlg.getSelect();
                    AppManager.editorConfig.GameControlStart = dlg.getStart();
                    AppManager.editorConfig.GameControlPovDown = dlg.getPovDown();
                    AppManager.editorConfig.GameControlPovLeft = dlg.getPovLeft();
                    AppManager.editorConfig.GameControlPovUp = dlg.getPovUp();
                    AppManager.editorConfig.GameControlPovRight = dlg.getPovRight();
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuSettingGrameControlerSetting_Click; ex=" + ex + "\n");
            } finally {
                if (dlg != null) {
                    try {
                        dlg.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".menuSettingGameControlerSetting_Click; ex=" + ex2 + "\n");
                    }
                }
            }
        }

        public void menuSettingPreference_Click(Object sender, EventArgs e)
        {
            try {
                if (mDialogPreference == null) {
                    mDialogPreference = new Preference();
                }
                mDialogPreference.setBaseFont(new Font(AppManager.editorConfig.BaseFontName, java.awt.Font.PLAIN, AppManager.FONT_SIZE9));
                mDialogPreference.setScreenFont(new Font(AppManager.editorConfig.ScreenFontName, java.awt.Font.PLAIN, AppManager.FONT_SIZE9));
                mDialogPreference.setWheelOrder(AppManager.editorConfig.WheelOrder);
                mDialogPreference.setCursorFixed(AppManager.editorConfig.CursorFixed);
                mDialogPreference.setDefaultVibratoLength(AppManager.editorConfig.DefaultVibratoLength);
                mDialogPreference.setAutoVibratoThresholdLength(AppManager.editorConfig.AutoVibratoThresholdLength);
                mDialogPreference.setAutoVibratoType1(AppManager.editorConfig.AutoVibratoType1);
                mDialogPreference.setAutoVibratoType2(AppManager.editorConfig.AutoVibratoType2);
                mDialogPreference.setAutoVibratoTypeCustom(AppManager.editorConfig.AutoVibratoTypeCustom);
                mDialogPreference.setEnableAutoVibrato(AppManager.editorConfig.EnableAutoVibrato);
                mDialogPreference.setPreSendTime(AppManager.editorConfig.PreSendTime);
                mDialogPreference.setControlCurveResolution(AppManager.editorConfig.ControlCurveResolution);
                mDialogPreference.setDefaultSingerName(AppManager.editorConfig.DefaultSingerName);
                mDialogPreference.setScrollHorizontalOnWheel(AppManager.editorConfig.ScrollHorizontalOnWheel);
                mDialogPreference.setMaximumFrameRate(AppManager.editorConfig.MaximumFrameRate);
                mDialogPreference.setKeepLyricInputMode(AppManager.editorConfig.KeepLyricInputMode);
                mDialogPreference.setPxTrackHeight(AppManager.editorConfig.PxTrackHeight);
                mDialogPreference.setMouseHoverTime(AppManager.editorConfig.getMouseHoverTime());
                mDialogPreference.setPlayPreviewWhenRightClick(AppManager.editorConfig.PlayPreviewWhenRightClick);
                mDialogPreference.setCurveSelectingQuantized(AppManager.editorConfig.CurveSelectingQuantized);
                mDialogPreference.setCurveVisibleAccent(AppManager.editorConfig.CurveVisibleAccent);
                mDialogPreference.setCurveVisibleBre(AppManager.editorConfig.CurveVisibleBreathiness);
                mDialogPreference.setCurveVisibleBri(AppManager.editorConfig.CurveVisibleBrightness);
                mDialogPreference.setCurveVisibleCle(AppManager.editorConfig.CurveVisibleClearness);
                mDialogPreference.setCurveVisibleDecay(AppManager.editorConfig.CurveVisibleDecay);
                mDialogPreference.setCurveVisibleDyn(AppManager.editorConfig.CurveVisibleDynamics);
                mDialogPreference.setCurveVisibleGen(AppManager.editorConfig.CurveVisibleGendorfactor);
                mDialogPreference.setCurveVisibleOpe(AppManager.editorConfig.CurveVisibleOpening);
                mDialogPreference.setCurveVisiblePit(AppManager.editorConfig.CurveVisiblePit);
                mDialogPreference.setCurveVisiblePbs(AppManager.editorConfig.CurveVisiblePbs);
                mDialogPreference.setCurveVisiblePor(AppManager.editorConfig.CurveVisiblePortamento);
                mDialogPreference.setCurveVisibleVel(AppManager.editorConfig.CurveVisibleVelocity);
                mDialogPreference.setCurveVisibleVibratoDepth(AppManager.editorConfig.CurveVisibleVibratoDepth);
                mDialogPreference.setCurveVisibleVibratoRate(AppManager.editorConfig.CurveVisibleVibratoRate);
                mDialogPreference.setCurveVisibleFx2Depth(AppManager.editorConfig.CurveVisibleFx2Depth);
                mDialogPreference.setCurveVisibleHarmonics(AppManager.editorConfig.CurveVisibleHarmonics);
                mDialogPreference.setCurveVisibleReso1(AppManager.editorConfig.CurveVisibleReso1);
                mDialogPreference.setCurveVisibleReso2(AppManager.editorConfig.CurveVisibleReso2);
                mDialogPreference.setCurveVisibleReso3(AppManager.editorConfig.CurveVisibleReso3);
                mDialogPreference.setCurveVisibleReso4(AppManager.editorConfig.CurveVisibleReso4);
                mDialogPreference.setCurveVisibleEnvelope(AppManager.editorConfig.CurveVisibleEnvelope);
#if ENABLE_MIDI
                mDialogPreference.setMidiInPort(AppManager.editorConfig.MidiInPort.PortNumber);
#endif
#if ENABLE_MTC
            	m_preference_dlg.setMtcMidiInPort( AppManager.editorConfig.MidiInPortMtc.PortNumber );

#endif
                List<string> resamplers = new List<string>();
                int size = AppManager.editorConfig.getResamplerCount();
                for (int i = 0; i < size; i++) {
                    resamplers.Add(AppManager.editorConfig.getResamplerAt(i));
                }
                mDialogPreference.setResamplersConfig(resamplers);
                mDialogPreference.setPathWavtool(AppManager.editorConfig.PathWavtool);
                mDialogPreference.setUtausingers(AppManager.editorConfig.UtauSingers);
                mDialogPreference.setSelfDeRomantization(AppManager.editorConfig.SelfDeRomanization);
                mDialogPreference.setAutoBackupIntervalMinutes(AppManager.editorConfig.AutoBackupIntervalMinutes);
                mDialogPreference.setUseSpaceKeyAsMiddleButtonModifier(AppManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier);
                mDialogPreference.setPathAquesTone(AppManager.editorConfig.PathAquesTone);
                mDialogPreference.setPathAquesTone2(AppManager.editorConfig.PathAquesTone2);
                mDialogPreference.setUseProjectCache(AppManager.editorConfig.UseProjectCache);
                mDialogPreference.setAquesToneRequired(!AppManager.editorConfig.DoNotUseAquesTone);
                mDialogPreference.setAquesTone2Requried(!AppManager.editorConfig.DoNotUseAquesTone2);
                mDialogPreference.setVocaloid1Required(!AppManager.editorConfig.DoNotUseVocaloid1);
                mDialogPreference.setVocaloid2Required(!AppManager.editorConfig.DoNotUseVocaloid2);
                mDialogPreference.setBufferSize(AppManager.editorConfig.BufferSizeMilliSeconds);
                mDialogPreference.setDefaultSynthesizer(AppManager.editorConfig.DefaultSynthesizer);
                mDialogPreference.setUseUserDefinedAutoVibratoType(AppManager.editorConfig.UseUserDefinedAutoVibratoType);
                mDialogPreference.setEnableWideCharacterWorkaround(AppManager.editorConfig.UseWideCharacterWorkaround);

                mDialogPreference.Location = getFormPreferedLocation(mDialogPreference);

                DialogResult dr = AppManager.showModalDialog(mDialogPreference, this);
                if (dr == DialogResult.OK) {
                    string old_base_font_name = AppManager.editorConfig.BaseFontName;
                    float old_base_font_size = AppManager.editorConfig.BaseFontSize;
                    Font new_base_font = mDialogPreference.getBaseFont();
                    if (!old_base_font_name.Equals(new_base_font.getName()) ||
                         old_base_font_size != new_base_font.getSize2D()) {
                        AppManager.editorConfig.BaseFontName = mDialogPreference.getBaseFont().getName();
                        AppManager.editorConfig.BaseFontSize = mDialogPreference.getBaseFont().getSize2D();
                        updateMenuFonts();
                    }

                    AppManager.editorConfig.ScreenFontName = mDialogPreference.getScreenFont().getName();
                    AppManager.editorConfig.WheelOrder = mDialogPreference.getWheelOrder();
                    AppManager.editorConfig.CursorFixed = mDialogPreference.isCursorFixed();

                    AppManager.editorConfig.DefaultVibratoLength = mDialogPreference.getDefaultVibratoLength();
                    AppManager.editorConfig.AutoVibratoThresholdLength = mDialogPreference.getAutoVibratoThresholdLength();
                    AppManager.editorConfig.AutoVibratoType1 = mDialogPreference.getAutoVibratoType1();
                    AppManager.editorConfig.AutoVibratoType2 = mDialogPreference.getAutoVibratoType2();
                    AppManager.editorConfig.AutoVibratoTypeCustom = mDialogPreference.getAutoVibratoTypeCustom();

                    AppManager.editorConfig.EnableAutoVibrato = mDialogPreference.isEnableAutoVibrato();
                    AppManager.editorConfig.PreSendTime = mDialogPreference.getPreSendTime();
                    AppManager.editorConfig.Language = mDialogPreference.getLanguage();
                    if (!Messaging.getLanguage().Equals(AppManager.editorConfig.Language)) {
                        Messaging.setLanguage(AppManager.editorConfig.Language);
                        applyLanguage();
                        mDialogPreference.applyLanguage();
                        AppManager.mMixerWindow.applyLanguage();
                        if (mVersionInfo != null && !mVersionInfo.IsDisposed) {
                            mVersionInfo.applyLanguage();
                        }
#if ENABLE_PROPERTY
                        AppManager.propertyWindow.applyLanguage();
                        AppManager.propertyPanel.updateValue(AppManager.getSelected());
#endif
                        if (mDialogMidiImportAndExport != null) {
                            mDialogMidiImportAndExport.applyLanguage();
                        }
                    }

                    AppManager.editorConfig.ControlCurveResolution = mDialogPreference.getControlCurveResolution();
                    AppManager.editorConfig.DefaultSingerName = mDialogPreference.getDefaultSingerName();
                    AppManager.editorConfig.ScrollHorizontalOnWheel = mDialogPreference.isScrollHorizontalOnWheel();
                    AppManager.editorConfig.MaximumFrameRate = mDialogPreference.getMaximumFrameRate();
                    int fps = 1000 / AppManager.editorConfig.MaximumFrameRate;
                    timer.Interval = (fps <= 0) ? 1 : fps;
                    applyShortcut();
                    AppManager.editorConfig.KeepLyricInputMode = mDialogPreference.isKeepLyricInputMode();
                    if (AppManager.editorConfig.PxTrackHeight != mDialogPreference.getPxTrackHeight()) {
                        AppManager.editorConfig.PxTrackHeight = mDialogPreference.getPxTrackHeight();
                        updateDrawObjectList();
                    }
                    AppManager.editorConfig.setMouseHoverTime(mDialogPreference.getMouseHoverTime());
                    AppManager.editorConfig.PlayPreviewWhenRightClick = mDialogPreference.isPlayPreviewWhenRightClick();
                    AppManager.editorConfig.CurveSelectingQuantized = mDialogPreference.isCurveSelectingQuantized();

                    AppManager.editorConfig.CurveVisibleAccent = mDialogPreference.isCurveVisibleAccent();
                    AppManager.editorConfig.CurveVisibleBreathiness = mDialogPreference.isCurveVisibleBre();
                    AppManager.editorConfig.CurveVisibleBrightness = mDialogPreference.isCurveVisibleBri();
                    AppManager.editorConfig.CurveVisibleClearness = mDialogPreference.isCurveVisibleCle();
                    AppManager.editorConfig.CurveVisibleDecay = mDialogPreference.isCurveVisibleDecay();
                    AppManager.editorConfig.CurveVisibleDynamics = mDialogPreference.isCurveVisibleDyn();
                    AppManager.editorConfig.CurveVisibleGendorfactor = mDialogPreference.isCurveVisibleGen();
                    AppManager.editorConfig.CurveVisibleOpening = mDialogPreference.isCurveVisibleOpe();
                    AppManager.editorConfig.CurveVisiblePit = mDialogPreference.isCurveVisiblePit();
                    AppManager.editorConfig.CurveVisiblePbs = mDialogPreference.isCurveVisiblePbs();
                    AppManager.editorConfig.CurveVisiblePortamento = mDialogPreference.isCurveVisiblePor();
                    AppManager.editorConfig.CurveVisibleVelocity = mDialogPreference.isCurveVisibleVel();
                    AppManager.editorConfig.CurveVisibleVibratoDepth = mDialogPreference.isCurveVisibleVibratoDepth();
                    AppManager.editorConfig.CurveVisibleVibratoRate = mDialogPreference.isCurveVisibleVibratoRate();
                    AppManager.editorConfig.CurveVisibleFx2Depth = mDialogPreference.isCurveVisibleFx2Depth();
                    AppManager.editorConfig.CurveVisibleHarmonics = mDialogPreference.isCurveVisibleHarmonics();
                    AppManager.editorConfig.CurveVisibleReso1 = mDialogPreference.isCurveVisibleReso1();
                    AppManager.editorConfig.CurveVisibleReso2 = mDialogPreference.isCurveVisibleReso2();
                    AppManager.editorConfig.CurveVisibleReso3 = mDialogPreference.isCurveVisibleReso3();
                    AppManager.editorConfig.CurveVisibleReso4 = mDialogPreference.isCurveVisibleReso4();
                    AppManager.editorConfig.CurveVisibleEnvelope = mDialogPreference.isCurveVisibleEnvelope();

#if ENABLE_MIDI
                    AppManager.editorConfig.MidiInPort.PortNumber = mDialogPreference.getMidiInPort();
#endif
#if ENABLE_MTC
                    AppManager.editorConfig.MidiInPortMtc.PortNumber = m_preference_dlg.getMtcMidiInPort();
#endif
#if ENABLE_MIDI || ENABLE_MTC
                    updateMidiInStatus();
                    reloadMidiIn();
#endif

                    List<string> new_resamplers = new List<string>();
                    mDialogPreference.copyResamplersConfig(new_resamplers);
                    AppManager.editorConfig.clearResampler();
                    for (int i = 0; i < new_resamplers.Count; i++) {
                        AppManager.editorConfig.addResampler(new_resamplers[i]);
                    }
                    AppManager.editorConfig.PathWavtool = mDialogPreference.getPathWavtool();

                    AppManager.editorConfig.UtauSingers.Clear();
                    foreach (var sc in mDialogPreference.getUtausingers()) {
                        AppManager.editorConfig.UtauSingers.Add((SingerConfig)sc.clone());
                    }
                    AppManager.reloadUtauVoiceDB();

                    AppManager.editorConfig.SelfDeRomanization = mDialogPreference.isSelfDeRomantization();
                    AppManager.editorConfig.AutoBackupIntervalMinutes = mDialogPreference.getAutoBackupIntervalMinutes();
                    AppManager.editorConfig.UseSpaceKeyAsMiddleButtonModifier = mDialogPreference.isUseSpaceKeyAsMiddleButtonModifier();

#if ENABLE_AQUESTONE
                    var old_aquestone_config = Tuple.Create(AppManager.editorConfig.PathAquesTone, AppManager.editorConfig.DoNotUseAquesTone);
                    AppManager.editorConfig.PathAquesTone = mDialogPreference.getPathAquesTone();
                    AppManager.editorConfig.DoNotUseAquesTone = !mDialogPreference.isAquesToneRequired();
                    if (old_aquestone_config.Item1 != AppManager.editorConfig.PathAquesTone
                        || old_aquestone_config.Item2 != AppManager.editorConfig.DoNotUseAquesTone) {
                        VSTiDllManager.reloadAquesTone();
                    }

                    var old_aquestone2_config = Tuple.Create(AppManager.editorConfig.PathAquesTone2, AppManager.editorConfig.DoNotUseAquesTone2);
                    AppManager.editorConfig.PathAquesTone2 = mDialogPreference.getPathAquesTone2();
                    AppManager.editorConfig.DoNotUseAquesTone2 = !mDialogPreference.isAquesTone2Required();
                    if (old_aquestone2_config.Item1 != AppManager.editorConfig.PathAquesTone2
                        || old_aquestone2_config.Item2 != AppManager.editorConfig.DoNotUseAquesTone2) {
                        VSTiDllManager.reloadAquesTone2();
                    }
#endif
                    updateRendererMenu();

                    //AppManager.editorConfig.__revoked__WaveFileOutputFromMasterTrack = mDialogPreference.isWaveFileOutputFromMasterTrack();
                    //AppManager.editorConfig.__revoked__WaveFileOutputChannel = mDialogPreference.getWaveFileOutputChannel();
                    if (AppManager.editorConfig.UseProjectCache && !mDialogPreference.isUseProjectCache()) {
                        // プロジェクト用キャッシュを使用していたが，使用しないように変更された場合.
                        // プロジェクト用キャッシュが存在するなら，共用のキャッシュに移動する．
                        string file = AppManager.getFileName();
                        if (file != null && !file.Equals("")) {
                            string dir = PortUtil.getDirectoryName(file);
                            string name = PortUtil.getFileNameWithoutExtension(file);
                            string projectCacheDir = Path.Combine(dir, name + ".cadencii");
                            string commonCacheDir = Path.Combine(AppManager.getCadenciiTempDir(), AppManager.getID());
                            if (Directory.Exists(projectCacheDir)) {
                                VsqFileEx vsq = AppManager.getVsqFile();
                                for (int i = 1; i < vsq.Track.Count; i++) {
                                    // wavを移動
                                    string wavFrom = Path.Combine(projectCacheDir, i + ".wav");
                                    string wavTo = Path.Combine(commonCacheDir, i + ".wav");
                                    if (!System.IO.File.Exists(wavFrom)) {
                                        continue;
                                    }
                                    if (System.IO.File.Exists(wavTo)) {
                                        try {
                                            PortUtil.deleteFile(wavTo);
                                        } catch (Exception ex) {
                                            Logger.write(typeof(FormMain) + ".menuSettingPreference_Click; ex=" + ex + "\n");
                                            serr.println("FormMain#menuSettingPreference_Click; ex=" + ex);
                                            continue;
                                        }
                                    }
                                    try {
                                        PortUtil.moveFile(wavFrom, wavTo);
                                    } catch (Exception ex) {
                                        Logger.write(typeof(FormMain) + ".menuSettingPreference_Click; ex=" + ex + "\n");
                                        serr.println("FormMain#menuSettingPreference_Click; ex=" + ex);
                                    }

                                    // xmlを移動
                                    string xmlFrom = Path.Combine(projectCacheDir, i + ".xml");
                                    string xmlTo = Path.Combine(commonCacheDir, i + ".xml");
                                    if (!System.IO.File.Exists(xmlFrom)) {
                                        continue;
                                    }
                                    if (System.IO.File.Exists(xmlTo)) {
                                        try {
                                            PortUtil.deleteFile(xmlTo);
                                        } catch (Exception ex) {
                                            Logger.write(typeof(FormMain) + ".menuSettingPreference_Click; ex=" + ex + "\n");
                                            serr.println("FormMain#menuSettingPreference_Click; ex=" + ex);
                                            continue;
                                        }
                                    }
                                    try {
                                        PortUtil.moveFile(xmlFrom, xmlTo);
                                    } catch (Exception ex) {
                                        Logger.write(typeof(FormMain) + ".menuSettingPreference_Click; ex=" + ex + "\n");
                                        serr.println("FormMain#menuSettingPreference_Click; ex=" + ex);
                                    }
                                }

                                // projectCacheDirが空なら，ディレクトリごと削除する
                                string[] files = PortUtil.listFiles(projectCacheDir, "*.*");
                                if (files.Length <= 0) {
                                    try {
                                        PortUtil.deleteDirectory(projectCacheDir);
                                    } catch (Exception ex) {
                                        Logger.write(typeof(FormMain) + ".menuSettingPreference_Click; ex=" + ex + "\n");
                                        serr.println("FormMain#menuSettingPreference_Click; ex=" + ex);
                                    }
                                }

                                // キャッシュのディレクトリを再指定
                                AppManager.setTempWaveDir(commonCacheDir);
                            }
                        }
                    }
                    AppManager.editorConfig.UseProjectCache = mDialogPreference.isUseProjectCache();
                    AppManager.editorConfig.DoNotUseVocaloid1 = !mDialogPreference.isVocaloid1Required();
                    AppManager.editorConfig.DoNotUseVocaloid2 = !mDialogPreference.isVocaloid2Required();
                    AppManager.editorConfig.BufferSizeMilliSeconds = mDialogPreference.getBufferSize();
                    AppManager.editorConfig.DefaultSynthesizer = mDialogPreference.getDefaultSynthesizer();
                    AppManager.editorConfig.UseUserDefinedAutoVibratoType = mDialogPreference.isUseUserDefinedAutoVibratoType();
                    AppManager.editorConfig.UseWideCharacterWorkaround = mDialogPreference.isEnableWideCharacterWorkaround();

                    trackSelector.prepareSingerMenu(VsqFileEx.getTrackRendererKind(AppManager.getVsqFile().Track[AppManager.getSelected()]));
                    trackSelector.updateVisibleCurves();

                    updateRendererMenu();
                    AppManager.updateAutoBackupTimerStatus();

                    // editorConfig.PxTrackHeightが変更されている可能性があるので，更新が必要
                    controller.setStartToDrawY(calculateStartToDrawY(vScroll.Value));

                    if (menuVisualControlTrack.Checked) {
                        splitContainer1.setPanel2MinSize(trackSelector.getPreferredMinSize());
                    }

                    AppManager.saveConfig();
                    applyLanguage();
#if ENABLE_SCRIPT
                    updateScriptShortcut();
#endif

                    updateDrawObjectList();
                    refreshScreen();
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuSettingPreference_Click; ex=" + ex + "\n");
                AppManager.debugWriteLine("FormMain#menuSettingPreference_Click; ex=" + ex);
            }
        }

        public void menuSettingShortcut_Click(Object sender, EventArgs e)
        {
            SortedDictionary<string, ValuePair<string, Keys[]>> dict = new SortedDictionary<string, ValuePair<string, Keys[]>>();
            SortedDictionary<string, Keys[]> configured = AppManager.editorConfig.getShortcutKeysDictionary(this.getDefaultShortcutKeys());
#if DEBUG
            sout.println("FormMain#menuSettingShortcut_Click; configured=");
            foreach (var name in configured.Keys) {
                Keys[] keys = configured[name];
                string disp = Utility.getShortcutDisplayString(keys);
                sout.println("    " + name + " -> " + disp);
            }
#endif

            // スクリプトのToolStripMenuITemを蒐集
            List<string> script_shortcut = new List<string>();
            foreach (var tsi in menuScript.DropDownItems) {
                if (tsi is System.Windows.Forms.ToolStripMenuItem) {
                    var tsmi = (System.Windows.Forms.ToolStripMenuItem)tsi;
                    string name = tsmi.Name;
                    script_shortcut.Add(name);
                    if (!configured.ContainsKey(name)) {
                        configured[name] = new Keys[] { };
                    }
                }
            }

            foreach (var name in configured.Keys) {
                ByRef<Object> owner = new ByRef<Object>(null);
                Object menu = searchMenuItemFromName(name, owner);
#if DEBUG
                if (menu == null || owner.value == null) {
                    serr.println("FormMain#enuSettingShrtcut_Click; name=" + name + "; menu is null");
                    continue;
                }
#endif
                ToolStripMenuItem casted_owner_item = null;
                if (owner.value is ToolStripMenuItem) {
                    casted_owner_item = (ToolStripMenuItem)owner.value;
                }
                if (casted_owner_item == null) {
                    continue;
                }
                string parent = "";
                if (!casted_owner_item.Name.Equals(menuHidden.Name)) {
                    string s = casted_owner_item.Text;
                    int i = s.IndexOf("(&");
                    if (i > 0) {
                        s = s.Substring(0, i);
                    }
                    parent = s + " -> ";
                }
                ToolStripMenuItem casted_menu = null;
                if (menu is ToolStripMenuItem) {
                    casted_menu = (ToolStripMenuItem)menu;
                }
                if (casted_menu == null) {
                    continue;
                }
                string s1 = casted_menu.Text;
                int i1 = s1.IndexOf("(&");
                if (i1 > 0) {
                    s1 = s1.Substring(0, i1);
                }
                dict[parent + s1] = new ValuePair<string, Keys[]>(name, configured[name]);
            }

            // 最初に戻る、のショートカットキー
            Keys[] keysGoToFirst = AppManager.editorConfig.SpecialShortcutGoToFirst;
            if (keysGoToFirst == null) {
                keysGoToFirst = new Keys[] { };
            }
            dict[_("Go to the first")] = new ValuePair<string, Keys[]>("SpecialShortcutGoToFirst", keysGoToFirst);

            FormShortcutKeys form = null;
            try {
                form = new FormShortcutKeys(dict, this);
                form.Location = getFormPreferedLocation(form);
                DialogResult dr = AppManager.showModalDialog(form, this);
                if (dr == DialogResult.OK) {
                    SortedDictionary<string, ValuePair<string, Keys[]>> res = form.getResult();
                    foreach (var display in res.Keys) {
                        string name = res[display].getKey();
                        Keys[] keys = res[display].getValue();
                        bool found = false;
                        if (name.Equals("SpecialShortcutGoToFirst")) {
                            AppManager.editorConfig.SpecialShortcutGoToFirst = keys;
                        } else {
                            for (int i = 0; i < AppManager.editorConfig.ShortcutKeys.Count; i++) {
                                if (AppManager.editorConfig.ShortcutKeys[i].Key.Equals(name)) {
                                    AppManager.editorConfig.ShortcutKeys[i].Value = keys;
                                    found = true;
                                    break;
                                }
                            }
                            if (!found) {
                                AppManager.editorConfig.ShortcutKeys.Add(new ValuePairOfStringArrayOfKeys(name, keys));
                            }
                        }
                    }
                    applyShortcut();
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuSettingShortcut_Click; ex=" + ex + "\n");
            } finally {
                if (form != null) {
                    try {
                        form.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".menuSettingSHortcut_Click; ex=" + ex2 + "\n");
                    }
                }
            }
        }

        public void menuSettingVibratoPreset_Click(Object sender, EventArgs e)
        {
            FormVibratoPreset dialog = null;
            try {
                dialog = new FormVibratoPreset(AppManager.editorConfig.AutoVibratoCustom);
                dialog.Location = getFormPreferedLocation(dialog);
                DialogResult ret = AppManager.showModalDialog(dialog, this);
                if (ret != DialogResult.OK) {
                    return;
                }

                // ダイアログの結果を取得
                List<VibratoHandle> result = dialog.getResult();

                // ダイアログ結果を，設定値にコピー
                // ダイアログのコンストラクタであらかじめcloneされているので，
                // ここではcloneする必要はない．
                AppManager.editorConfig.AutoVibratoCustom.Clear();
                for (int i = 0; i < result.Count; i++) {
                    AppManager.editorConfig.AutoVibratoCustom.Add(result[i]);
                }

                // メニューの表示状態を更新
                updateVibratoPresetMenu();
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuSettingVibratoPreset_Click; ex=" + ex + "\n");
            } finally {
                if (dialog != null) {
                    try {
                        dialog.Dispose();
                    } catch (Exception ex2) {
                    }
                }
            }
        }
        #endregion

        //BOOKMARK: menuEdit
        #region menuEdit*
        public void menuEditDelete_Click(Object sender, EventArgs e)
        {
            deleteEvent();
        }

        public void menuEdit_DropDownOpening(Object sender, EventArgs e)
        {
            updateCopyAndPasteButtonStatus();
        }

        public void handleEditUndo_Click(Object sender, EventArgs e)
        {
#if DEBUG
            AppManager.debugWriteLine("menuEditUndo_Click");
#endif
            undo();
            refreshScreen();
        }


        public void handleEditRedo_Click(Object sender, EventArgs e)
        {
#if DEBUG
            AppManager.debugWriteLine("menuEditRedo_Click");
#endif
            redo();
            refreshScreen();
        }

        public void menuEditSelectAllEvents_Click(Object sender, EventArgs e)
        {
            selectAllEvent();
        }

        public void menuEditSelectAll_Click(Object sender, EventArgs e)
        {
            selectAll();
        }

        public void menuEditAutoNormalizeMode_Click(Object sender, EventArgs e)
        {
            AppManager.mAutoNormalize = !AppManager.mAutoNormalize;
            menuEditAutoNormalizeMode.Checked = AppManager.mAutoNormalize;
        }
        #endregion

        //BOOKMARK: menuLyric
        #region menuLyric*
        public void menuLyric_DropDownOpening(Object sender, EventArgs e)
        {
            menuLyricCopyVibratoToPreset.Enabled = false;

            int num = AppManager.itemSelection.getEventCount();
            if (num <= 0) {
                return;
            }
            SelectedEventEntry item = AppManager.itemSelection.getEventIterator().First();
            if (item.original.ID.type != VsqIDType.Anote) {
                return;
            }
            if (item.original.ID.VibratoHandle == null) {
                return;
            }

            menuLyricCopyVibratoToPreset.Enabled = true;
        }

        public void menuLyricExpressionProperty_Click(Object sender, EventArgs e)
        {
            editNoteExpressionProperty();
        }

        public void menuLyricPhonemeTransformation_Click(Object sender, EventArgs e)
        {
            List<int> internal_ids = new List<int>();
            List<VsqID> ids = new List<VsqID>();
            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq == null) {
                return;
            }
            int selected = AppManager.getSelected();
            VsqTrack vsq_track = vsq.Track[selected];
            foreach (var item in vsq_track.getNoteEventIterator()) {
                VsqID id = item.ID;
                if (id.LyricHandle.L0.PhoneticSymbolProtected) {
                    continue;
                }
                string phrase = id.LyricHandle.L0.Phrase;
                string symbolOld = id.LyricHandle.L0.getPhoneticSymbol();
                string symbolResult = symbolOld;
                SymbolTableEntry entry = SymbolTable.attatch(phrase);
                if (entry == null) {
                    continue;
                }
                symbolResult = entry.getSymbol();
                if (symbolResult.Equals(symbolOld)) {
                    continue;
                }
                VsqID idNew = (VsqID)id.clone();
                idNew.LyricHandle.L0.setPhoneticSymbol(symbolResult);
                ids.Add(idNew);
                internal_ids.Add(item.InternalID);
            }
            if (ids.Count <= 0) {
                return;
            }
            CadenciiCommand run = new CadenciiCommand(
                VsqCommand.generateCommandEventChangeIDContaintsRange(
                    selected,
                    PortUtil.convertIntArray(internal_ids.ToArray()),
                    ids.ToArray()));
            AppManager.editHistory.register(vsq.executeCommand(run));
            setEdited(true);
        }

        /// <summary>
        /// 現在表示しているトラックの，選択状態の音符イベントについて，それぞれのイベントの
        /// 時刻でのUTAU歌手に応じて，UTAUの各種パラメータを原音設定のものにリセットします
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuLyricApplyUtauParameters_Click(Object sender, EventArgs e)
        {
            // 選択されているトラックの番号
            int selected = AppManager.getSelected();
            // シーケンス
            VsqFileEx vsq = AppManager.getVsqFile();
            VsqTrack vsq_track = vsq.Track[selected];

            // 選択状態にあるイベントを取り出す
            List<VsqEvent> replace = new List<VsqEvent>();
            foreach (var sel_item in AppManager.itemSelection.getEventIterator()) {
                VsqEvent item = sel_item.original;
                if (item.ID.type != VsqIDType.Anote) {
                    continue;
                }
                VsqEvent edit = (VsqEvent)item.clone();
                // UTAUのパラメータを適用
                AppManager.applyUtauParameter(vsq_track, edit);
                // 合成したとき，意味のある変更が行われたか？
                if (edit.UstEvent.equalsForSynth(item.UstEvent)) {
                    continue;
                }
                // 意味のある変更があったので，リストに登録
                replace.Add(edit);
            }

            // コマンドを発行
            CadenciiCommand run = new CadenciiCommand(
                VsqCommand.generateCommandEventReplaceRange(selected, replace.ToArray()));
            // コマンドを実行
            AppManager.editHistory.register(vsq.executeCommand(run));
            setEdited(true);
        }

        public void menuLyricDictionary_Click(Object sender, EventArgs e)
        {
            FormWordDictionaryController dlg = null;
            try {
                dlg = new FormWordDictionaryController();
                var p = getFormPreferedLocation(dlg.getWidth(), dlg.getHeight());
                dlg.setLocation(p.X, p.Y);
                int dr = AppManager.showModalDialog(dlg.getUi(), this);
                if (dr == 1) {
                    List<ValuePair<string, Boolean>> result = dlg.getResult();
                    SymbolTable.changeOrder(result);
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuLyricDictionary_Click; ex=" + ex + "\n");
                serr.println("FormMain#menuLyricDictionary_Click; ex=" + ex);
            } finally {
                if (dlg != null) {
                    try {
                        dlg.close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".menuLyricDictionary_Click; ex=" + ex2 + "\n");
                    }
                }
            }
        }

        public void menuLyricVibratoProperty_Click(Object sender, EventArgs e)
        {
            editNoteVibratoProperty();
        }
        #endregion

        //BOOKMARK: menuJob
        #region menuJob
        public void menuJobReloadVsti_Click(Object sender, EventArgs e)
        {
            //VSTiProxy.ReloadPlugin(); //todo: FormMain+menuJobReloadVsti_Click
        }

        public void menuJob_DropDownOpening(Object sender, EventArgs e)
        {
            if (AppManager.itemSelection.getEventCount() <= 1) {
                menuJobConnect.Enabled = false;
            } else {
                // menuJobConnect(音符の結合)がEnableされるかどうかは、選択されている音符がピアノロール上で連続かどうかで決まる
                int[] list = new int[AppManager.itemSelection.getEventCount()];
                for (int i = 0; i < AppManager.getVsqFile().Track[AppManager.getSelected()].getEventCount(); i++) {
                    int count = -1;
                    foreach (var item in AppManager.itemSelection.getEventIterator()) {
                        int key = item.original.InternalID;
                        count++;
                        if (key == AppManager.getVsqFile().Track[AppManager.getSelected()].getEvent(i).InternalID) {
                            list[count] = i;
                            break;
                        }
                    }
                }
                bool changed = true;
                while (changed) {
                    changed = false;
                    for (int i = 0; i < list.Length - 1; i++) {
                        if (list[i] > list[i + 1]) {
                            int b = list[i];
                            list[i] = list[i + 1];
                            list[i + 1] = b;
                            changed = true;
                        }
                    }
                }
                bool continued = true;
                for (int i = 0; i < list.Length - 1; i++) {
                    if (list[i] + 1 != list[i + 1]) {
                        continued = false;
                        break;
                    }
                }
                menuJobConnect.Enabled = continued;
            }

            menuJobLyric.Enabled = AppManager.itemSelection.getLastEvent() != null;
        }

        public void menuJobLyric_Click(Object sender, EventArgs e)
        {
            importLyric();
        }

        public void menuJobConnect_Click(Object sender, EventArgs e)
        {
            int count = AppManager.itemSelection.getEventCount();
            int[] clocks = new int[count];
            VsqID[] ids = new VsqID[count];
            int[] internalids = new int[count];
            int i = -1;
            foreach (var item in AppManager.itemSelection.getEventIterator()) {
                i++;
                clocks[i] = item.original.Clock;
                ids[i] = (VsqID)item.original.ID.clone();
                internalids[i] = item.original.InternalID;
            }
            bool changed = true;
            while (changed) {
                changed = false;
                for (int j = 0; j < clocks.Length - 1; j++) {
                    if (clocks[j] > clocks[j + 1]) {
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

            for (int j = 0; j < ids.Length - 1; j++) {
                ids[j].setLength(clocks[j + 1] - clocks[j]);
            }
            CadenciiCommand run = new CadenciiCommand(
                VsqCommand.generateCommandEventChangeIDContaintsRange(AppManager.getSelected(), internalids, ids));
            AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
            setEdited(true);
            Refresh();
        }

        public void menuJobInsertBar_Click(Object sender, EventArgs e)
        {
            int total_clock = AppManager.getVsqFile().TotalClocks;
            int total_barcount = AppManager.getVsqFile().getBarCountFromClock(total_clock) + 1;
            FormInsertBar dlg = null;
            try {
                dlg = new FormInsertBar(total_barcount);
                int current_clock = AppManager.getCurrentClock();
                int barcount = AppManager.getVsqFile().getBarCountFromClock(current_clock);
                int draft = barcount - AppManager.getVsqFile().getPreMeasure() + 1;
                if (draft <= 0) {
                    draft = 1;
                }
                dlg.setPosition(draft);

                dlg.Location = getFormPreferedLocation(dlg);
                DialogResult dr = AppManager.showModalDialog(dlg, this);
                if (dr == DialogResult.OK) {
                    int pos = dlg.getPosition() + AppManager.getVsqFile().getPreMeasure() - 1;
                    int length = dlg.getLength();

                    int clock_start = AppManager.getVsqFile().getClockFromBarCount(pos);
                    int clock_end = AppManager.getVsqFile().getClockFromBarCount(pos + length);
                    int dclock = clock_end - clock_start;
                    VsqFileEx temp = (VsqFileEx)AppManager.getVsqFile().clone();

                    for (int track = 1; track < temp.Track.Count; track++) {
                        BezierCurves newbc = new BezierCurves();
                        foreach (CurveType ct in Utility.CURVE_USAGE) {
                            int index = ct.getIndex();
                            if (index < 0) {
                                continue;
                            }

                            List<BezierChain> list = new List<BezierChain>();
                            foreach (var bc in temp.AttachedCurves.get(track - 1).get(ct)) {
                                if (bc.size() < 2) {
                                    continue;
                                }
                                int chain_start = (int)bc.points[0].getBase().getX();
                                int chain_end = (int)bc.points[bc.points.Count - 1].getBase().getX();

                                if (clock_start <= chain_start) {
                                    for (int i = 0; i < bc.points.Count; i++) {
                                        PointD t = bc.points[i].getBase();
                                        bc.points[i].setBase(new PointD(t.getX() + dclock, t.getY()));
                                    }
                                    list.Add(bc);
                                } else if (chain_start < clock_start && clock_start < chain_end) {
                                    BezierChain adding1 = bc.extractPartialBezier(chain_start, clock_start);
                                    BezierChain adding2 = bc.extractPartialBezier(clock_start, chain_end);
                                    for (int i = 0; i < adding2.points.Count; i++) {
                                        PointD t = adding2.points[i].getBase();
                                        adding2.points[i].setBase(new PointD(t.getX() + dclock, t.getY()));
                                    }
                                    adding1.points[adding1.points.Count - 1].setControlRightType(BezierControlType.None);
                                    adding2.points[0].setControlLeftType(BezierControlType.None);
                                    for (int i = 0; i < adding2.points.Count; i++) {
                                        adding1.points.Add(adding2.points[i]);
                                    }
                                    adding1.id = bc.id;
                                    list.Add(adding1);
                                } else {
                                    list.Add((BezierChain)bc.clone());
                                }
                            }

                            newbc.set(ct, list);
                        }
                        temp.AttachedCurves.set(track - 1, newbc);
                    }

                    for (int track = 1; track < AppManager.getVsqFile().Track.Count; track++) {
                        for (int i = 0; i < temp.Track[track].getEventCount(); i++) {
                            if (temp.Track[track].getEvent(i).Clock >= clock_start) {
                                temp.Track[track].getEvent(i).Clock += dclock;
                            }
                        }
                        foreach (CurveType curve in Utility.CURVE_USAGE) {
                            if (curve.isScalar() || curve.isAttachNote()) {
                                continue;
                            }
                            VsqBPList target = temp.Track[track].getCurve(curve.getName());
                            VsqBPList src = AppManager.getVsqFile().Track[track].getCurve(curve.getName());
                            target.clear();
                            foreach (var key in src.keyClockIterator()) {
                                if (key >= clock_start) {
                                    target.add(key + dclock, src.getValue(key));
                                } else {
                                    target.add(key, src.getValue(key));
                                }
                            }
                        }
                    }
                    for (int i = 0; i < temp.TempoTable.Count; i++) {
                        if (temp.TempoTable[i].Clock >= clock_start) {
                            temp.TempoTable[i].Clock = temp.TempoTable[i].Clock + dclock;
                        }
                    }
                    for (int i = 0; i < temp.TimesigTable.Count; i++) {
                        if (temp.TimesigTable[i].Clock >= clock_start) {
                            temp.TimesigTable[i].Clock = temp.TimesigTable[i].Clock + dclock;
                        }
                    }
                    temp.updateTempoInfo();
                    temp.updateTimesigInfo();

                    CadenciiCommand run = VsqFileEx.generateCommandReplace(temp);
                    AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                    setEdited(true);
                    Refresh();
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuJobInsertBar_Click; ex=" + ex + "\n");
            } finally {
                if (dlg != null) {
                    try {
                        dlg.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".menuJobInsertBar_Click; ex=" + ex2 + "\n");
                    }
                }
            }
        }

        public void menuJobChangePreMeasure_Click(Object sender, EventArgs e)
        {
            InputBox dialog = null;
            try {
                dialog = new InputBox(_("input pre-measure"));
                int old_pre_measure = AppManager.getVsqFile().getPreMeasure();
                dialog.setResult(old_pre_measure + "");
                dialog.Location = getFormPreferedLocation(dialog);
                DialogResult ret = AppManager.showModalDialog(dialog, this);
                if (ret == DialogResult.OK) {
                    string str_result = dialog.getResult();
                    int result = old_pre_measure;
                    try {
                        result = int.Parse(str_result);
                    } catch (Exception ex) {
                        result = old_pre_measure;
                    }
                    if (result < AppManager.MIN_PRE_MEASURE) {
                        result = AppManager.MIN_PRE_MEASURE;
                    }
                    if (result > AppManager.MAX_PRE_MEASURE) {
                        result = AppManager.MAX_PRE_MEASURE;
                    }
                    if (old_pre_measure != result) {
                        CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandChangePreMeasure(result));
                        AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                        AppManager.getVsqFile().updateTotalClocks();
                        refreshScreen(true);
                        setEdited(true);
                    }
                }
            } catch (Exception ex) {
                return;
            } finally {
                if (dialog != null) {
                    try {
                        dialog.Close();
                    } catch (Exception ex2) {
                    }
                }
            }
        }

        public void menuSettingSequence_Click(Object sender, EventArgs e)
        {
            VsqFileEx vsq = AppManager.getVsqFile();

            FormSequenceConfig dialog = new FormSequenceConfig();
            int old_channels = vsq.config.WaveFileOutputChannel;
            bool old_output_master = vsq.config.WaveFileOutputFromMasterTrack;
            int old_sample_rate = vsq.config.SamplingRate;
            int old_pre_measure = vsq.getPreMeasure();

            dialog.setWaveFileOutputChannel(old_channels);
            dialog.setWaveFileOutputFromMasterTrack(old_output_master);
            dialog.setSampleRate(old_sample_rate);
            dialog.setPreMeasure(old_pre_measure);

            dialog.Location = getFormPreferedLocation(dialog);
            if (AppManager.showModalDialog(dialog, this) != DialogResult.OK) {
                return;
            }

            int new_channels = dialog.getWaveFileOutputChannel();
            bool new_output_master = dialog.isWaveFileOutputFromMasterTrack();
            int new_sample_rate = dialog.getSampleRate();
            int new_pre_measure = dialog.getPreMeasure();

            CadenciiCommand run =
                VsqFileEx.generateCommandChangeSequenceConfig(
                    new_sample_rate,
                    new_channels,
                    new_output_master,
                    new_pre_measure);
            AppManager.editHistory.register(vsq.executeCommand(run));
            setEdited(true);
        }

        public void menuJobDeleteBar_Click(Object sender, EventArgs e)
        {
            int total_clock = AppManager.getVsqFile().TotalClocks;
            int total_barcount = AppManager.getVsqFile().getBarCountFromClock(total_clock) + 1;
            int clock = AppManager.getCurrentClock();
            int barcount = AppManager.getVsqFile().getBarCountFromClock(clock);
            FormDeleteBar dlg = null;
            try {
                dlg = new FormDeleteBar(total_barcount);
                int draft = barcount - AppManager.getVsqFile().getPreMeasure() + 1;
                if (draft <= 0) {
                    draft = 1;
                }
                dlg.setStart(draft);
                dlg.setEnd(draft + 1);

                dlg.Location = getFormPreferedLocation(dlg);
                DialogResult dr = AppManager.showModalDialog(dlg, this);
                if (dr == DialogResult.OK) {
                    VsqFileEx temp = (VsqFileEx)AppManager.getVsqFile().clone();
                    int start = dlg.getStart() + AppManager.getVsqFile().getPreMeasure() - 1;
                    int end = dlg.getEnd() + AppManager.getVsqFile().getPreMeasure() - 1;
#if DEBUG
                    AppManager.debugWriteLine("FormMain+menuJobDeleteBar_Click");
                    AppManager.debugWriteLine("    start,end=" + start + "," + end);
#endif
                    int clock_start = temp.getClockFromBarCount(start);
                    int clock_end = temp.getClockFromBarCount(end);
                    int dclock = clock_end - clock_start;
                    for (int track = 1; track < temp.Track.Count; track++) {
                        BezierCurves newbc = new BezierCurves();
                        for (int j = 0; j < Utility.CURVE_USAGE.Length; j++) {
                            CurveType ct = Utility.CURVE_USAGE[j];
                            int index = ct.getIndex();
                            if (index < 0) {
                                continue;
                            }

                            List<BezierChain> list = new List<BezierChain>();
                            foreach (var bc in temp.AttachedCurves.get(track - 1).get(ct)) {
                                if (bc.size() < 2) {
                                    continue;
                                }
                                int chain_start = (int)bc.points[0].getBase().getX();
                                int chain_end = (int)bc.points[bc.points.Count - 1].getBase().getX();

                                if (clock_start < chain_start && chain_start < clock_end && clock_end < chain_end) {
                                    BezierChain adding = bc.extractPartialBezier(clock_end, chain_end);
                                    adding.id = bc.id;
                                    for (int i = 0; i < adding.points.Count; i++) {
                                        PointD t = adding.points[i].getBase();
                                        adding.points[i].setBase(new PointD(t.getX() - dclock, t.getY()));
                                    }
                                    list.Add(adding);
                                } else if (chain_start < clock_start && clock_end < chain_end) {
                                    BezierChain adding1 = bc.extractPartialBezier(chain_start, clock_start);
                                    adding1.id = bc.id;
                                    adding1.points[adding1.points.Count - 1].setControlRightType(BezierControlType.None);
                                    BezierChain adding2 = bc.extractPartialBezier(clock_end, chain_end);
                                    adding2.points[0].setControlLeftType(BezierControlType.None);
                                    PointD t = adding2.points[0].getBase();
                                    adding2.points[0].setBase(new PointD(t.getX() - dclock, t.getY()));
                                    adding1.points.Add(adding2.points[0]);
                                    for (int i = 1; i < adding2.points.Count; i++) {
                                        t = adding2.points[i].getBase();
                                        adding2.points[i].setBase(new PointD(t.getX() - dclock, t.getY()));
                                        adding1.points.Add(adding2.points[i]);
                                    }
                                    list.Add(adding1);
                                } else if (chain_start < clock_start && clock_start < chain_end && chain_end < clock_end) {
                                    BezierChain adding = bc.extractPartialBezier(chain_start, clock_start);
                                    adding.id = bc.id;
                                    list.Add(adding);
                                } else if (clock_end <= chain_start || chain_end <= clock_start) {
                                    if (clock_end <= chain_start) {
                                        for (int i = 0; i < bc.points.Count; i++) {
                                            PointD t = bc.points[i].getBase();
                                            bc.points[i].setBase(new PointD(t.getX() - dclock, t.getY()));
                                        }
                                    }
                                    list.Add((BezierChain)bc.clone());
                                }
                            }

                            newbc.set(ct, list);
                        }
                        temp.AttachedCurves.set(track - 1, newbc);
                    }

                    temp.removePart(clock_start, clock_end);
                    CadenciiCommand run = VsqFileEx.generateCommandReplace(temp);
                    AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                    setEdited(true);
                    Refresh();
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuJobDeleteBar_Click; ex=" + ex + "\n");
            } finally {
                if (dlg != null) {
                    try {
                        dlg.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".menuJobDeleteBar_Click; ex=" + ex2 + "\n");
                    }
                }
            }
        }

        public void menuJobNormalize_Click(Object sender, EventArgs e)
        {
            VsqFile work = (VsqFile)AppManager.getVsqFile().clone();
            int track = AppManager.getSelected();
            bool changed = true;
            bool total_changed = false;

            // 最初、開始時刻が同じになっている奴を削除
            while (changed) {
                changed = false;
                for (int i = 0; i < work.Track[track].getEventCount() - 1; i++) {
                    int clock = work.Track[track].getEvent(i).Clock;
                    int id = work.Track[track].getEvent(i).InternalID;
                    for (int j = i + 1; j < work.Track[track].getEventCount(); j++) {
                        if (clock == work.Track[track].getEvent(j).Clock) {
                            if (id < work.Track[track].getEvent(j).InternalID) { //内部IDが小さい＝より高年齢（音符追加時刻が古い）
                                work.Track[track].removeEvent(i);
                            } else {
                                work.Track[track].removeEvent(j);
                            }
                            changed = true;
                            total_changed = true;
                            break;
                        }
                    }
                    if (changed) {
                        break;
                    }
                }
            }

            changed = true;
            while (changed) {
                changed = false;
                for (int i = 0; i < work.Track[track].getEventCount() - 1; i++) {
                    int start_clock = work.Track[track].getEvent(i).Clock;
                    int end_clock = work.Track[track].getEvent(i).ID.getLength() + start_clock;
                    for (int j = i + 1; j < work.Track[track].getEventCount(); j++) {
                        int this_start_clock = work.Track[track].getEvent(j).Clock;
                        if (this_start_clock < end_clock) {
                            work.Track[track].getEvent(i).ID.setLength(this_start_clock - start_clock);
                            changed = true;
                            total_changed = true;
                        }
                    }
                }
            }

            if (total_changed) {
                CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandReplace(work));
                AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                setEdited(true);
                refreshScreen();
            }
        }

        public void menuJobRandomize_Click(Object sender, EventArgs e)
        {
            FormRandomize dlg = null;
            try {
                dlg = new FormRandomize();
                dlg.Location = getFormPreferedLocation(dlg);
                DialogResult dr = AppManager.showModalDialog(dlg, this);
                if (dr == DialogResult.OK) {
                    VsqFileEx vsq = AppManager.getVsqFile();
                    int preMeasure = vsq.getPreMeasure();
                    int startBar = dlg.getStartBar() + (preMeasure - 1);
                    int startBeat = dlg.getStartBeat() - 1;
                    int endBar = dlg.getEndBar() + (preMeasure - 1);
                    int endBeat = dlg.getEndBeat();
                    int startBarClock = vsq.getClockFromBarCount(startBar);
                    int endBarClock = vsq.getClockFromBarCount(endBar);
                    Timesig startTimesig = vsq.getTimesigAt(startBarClock);
                    Timesig endTimesig = vsq.getTimesigAt(endBarClock);
                    int startClock = startBarClock + startBeat * 480 * 4 / startTimesig.denominator;
                    int endClock = endBarClock + endBeat * 480 * 4 / endTimesig.denominator;

                    int selected = AppManager.getSelected();
                    VsqTrack vsq_track = vsq.Track[selected];
                    VsqTrack work = (VsqTrack)vsq_track.clone();
                    Random r = new Random();

                    // 音符位置のシフト
                    #region 音符位置のシフト
                    if (dlg.isPositionRandomizeEnabled()) {
                        int[] sigmaPreset = new int[] { 10, 20, 30, 60, 120 };
                        int sigma = sigmaPreset[dlg.getPositionRandomizeValue() - 1]; // 標準偏差

                        int count = work.getEventCount(); // イベントの個数
                        int lastItemIndex = -1;  // 直前に処理した音符イベントのインデクス
                        int thisItemIndex = -1;  // 処理中の音符イベントのインデクス
                        int nextItemIndex = -1;  // 処理中の音符イベントの次の音符イベントのインデクス
                        double sqrt2 = Math.Sqrt(2.0);
                        int clockPreMeasure = vsq.getPreMeasureClocks(); // プリメジャーいちでのゲートタイム

                        while (true) {
                            // nextItemIndexを決定
                            if (nextItemIndex != -2) {
                                int start = nextItemIndex + 1;
                                nextItemIndex = -2;  // -2は、トラックの最後まで走査し終わった、という意味
                                for (int i = start; i < count; i++) {
                                    if (work.getEvent(i).ID.type == VsqIDType.Anote) {
                                        nextItemIndex = i;
                                        break;
                                    }
                                }
                            }

                            if (thisItemIndex >= 0) {
                                // ここにメインの処理
                                VsqEvent lastItem = lastItemIndex >= 0 ? work.getEvent(lastItemIndex) : null;
                                VsqEvent thisItem = work.getEvent(thisItemIndex);
                                VsqEvent nextItem = nextItemIndex >= 0 ? work.getEvent(nextItemIndex) : null;
                                int lastItemClock = lastItem == null ? 0 : lastItem.Clock;
                                int lastItemLength = lastItem == null ? 0 : lastItem.ID.getLength();

                                int clock = thisItem.Clock;
                                int length = thisItem.ID.getLength();
                                if (startClock <= thisItem.Clock && thisItem.Clock + thisItem.ID.getLength() <= endClock) {
                                    int draftClock = 0;
                                    int draftLength = 0;
                                    int draftLastItemLength = lastItemLength;
                                    // 音符のめり込み等のチェックをクリアするまで、draft(Clock|Length|LastItemLength)をトライ＆エラーで決定する
                                    while (true) {
                                        int x = 3 * sigma;
                                        while (Math.Abs(x) > 2 * sigma) {
                                            double d = r.NextDouble();
                                            double y = (d - 0.5) * 2.0;
                                            x = (int)(sigma * sqrt2 * math.erfinv(y));
                                        }
                                        draftClock = clock + x;
                                        draftLength = clock + length - draftClock;
                                        if (lastItem != null) {
                                            if (clock == lastItemClock + lastItemLength) {
                                                // 音符が連続していた場合
                                                draftLastItemLength = lastItem.ID.getLength() + x;
                                            }
                                        }
                                        // 音符がめり込んだりしてないかどうかをチェック
                                        if (draftClock < clockPreMeasure) {
                                            continue;
                                        }
                                        if (lastItem != null) {
                                            if (clock != lastItemClock + lastItemLength) {
                                                // 音符が連続していなかった場合に、直前の音符にめり込んでいないかどうか
                                                if (draftClock + draftLength < lastItem.Clock + lastItem.ID.getLength()) {
                                                    continue;
                                                }
                                            }
                                        }
                                        // チェックにクリアしたのでループを抜ける
                                        break;
                                    }
                                    // draft*の値を適用
                                    thisItem.Clock = draftClock;
                                    thisItem.ID.setLength(draftLength);
                                    if (lastItem != null) {
                                        lastItem.ID.setLength(draftLastItemLength);
                                    }
                                } else if (endClock < thisItem.Clock) {
                                    break;
                                }
                            }

                            // インデクスを移す
                            lastItemIndex = thisItemIndex;
                            thisItemIndex = nextItemIndex;

                            if (lastItemIndex == -2 && thisItemIndex == -2 && nextItemIndex == -2) {
                                // トラックの最後まで走査し終わったので抜ける
                                break;
                            }
                        }
                    }
                    #endregion

                    // ピッチベンドのランダマイズ
                    #region ピッチベンドのランダマイズ
                    if (dlg.isPitRandomizeEnabled()) {
                        int pattern = dlg.getPitRandomizePattern();
                        int value = dlg.getPitRandomizeValue();
                        double order = 1.0 / Math.Pow(2.0, 5.0 - value);
                        int[] patternPreset = pattern == 1 ? Utility.getRandomizePitPattern1() : pattern == 2 ? Utility.getRandomizePitPattern2() : Utility.getRandomizePitPattern3();
                        int resolution = dlg.getResolution();
                        VsqBPList pit = work.getCurve("pit");
                        VsqBPList pbs = work.getCurve("pbs");
                        int pbsAtStart = pbs.getValue(startClock);
                        int pbsAtEnd = pbs.getValue(endClock);

                        // startClockからendClock - 1までのpit, pbsをクリアする
                        int count = pit.size();
                        for (int i = count - 1; i >= 0; i--) {
                            int keyClock = pit.getKeyClock(i);
                            if (startClock <= keyClock && keyClock < endClock) {
                                pit.removeElementAt(i);
                            }
                        }
                        count = pbs.size();
                        for (int i = count - 1; i >= 0; i--) {
                            int keyClock = pbs.getKeyClock(i);
                            if (startClock <= keyClock && keyClock < endClock) {
                                pbs.removeElementAt(i);
                            }
                        }

                        // pbsをデフォルト値にする
                        if (pbsAtStart != 2) {
                            pbs.add(startClock, 2);
                        }
                        if (pbsAtEnd != 2) {
                            pbs.add(endClock, pbsAtEnd);
                        }

                        StringBuilder sb = new StringBuilder();
                        count = pit.size();
                        bool first = true;
                        for (int i = 0; i < count; i++) {
                            int clock = pit.getKeyClock(i);
                            if (clock < startClock) {
                                int v = pit.getElementA(i);
                                sb.Append((first ? "" : ",") + (clock + "=" + v));
                                first = false;
                            } else if (clock <= endClock) {
                                break;
                            }
                        }
                        double d = r.NextDouble();
                        int start = (int)(d * (patternPreset.Length - 1));
                        for (int clock = startClock; clock < endClock; clock += resolution) {
                            int copyIndex = start + (clock - startClock);
                            int odd = copyIndex / patternPreset.Length;
                            copyIndex = copyIndex - patternPreset.Length * odd;
                            int v = (int)(patternPreset[copyIndex] * order);
                            sb.Append((first ? "" : ",") + (clock + "=" + v));
                            first = false;
                            //pit.add( clock, v );
                        }
                        for (int i = 0; i < count; i++) {
                            int clock = pit.getKeyClock(i);
                            if (endClock <= clock) {
                                int v = pit.getElementA(i);
                                sb.Append((first ? "" : ",") + (clock + "=" + v));
                                first = false;
                            }
                        }
                        pit.setData(sb.ToString());
                    }
                    #endregion

                    CadenciiCommand run = VsqFileEx.generateCommandTrackReplace(selected, work, vsq.AttachedCurves.get(selected - 1));
                    AppManager.editHistory.register(vsq.executeCommand(run));
                    setEdited(true);
                }
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuJobRandomize_Click; ex=" + ex + "\n");
                serr.println("FormMain#menuJobRandomize_Click; ex=" + ex);
            } finally {
                if (dlg != null) {
                    try {
                        dlg.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".menuJobRandomize_Click; ex=" + ex2 + "\n");
                        serr.println("FormMain#menuJobRandomize; ex2=" + ex2);
                    }
                }
            }
        }

        #endregion

        //BOOKMARK: menuScript
        #region menuScript
        public void menuScriptUpdate_Click(Object sender, EventArgs e)
        {
#if ENABLE_SCRIPT
            updateScriptShortcut();
            applyShortcut();
#endif
        }
        #endregion

        //BOOKMARK: vScroll
        #region vScroll
        public void vScroll_Enter(Object sender, EventArgs e)
        {
            focusPianoRoll();
        }

        public void vScroll_ValueChanged(Object sender, EventArgs e)
        {
            controller.setStartToDrawY(calculateStartToDrawY(vScroll.Value));
            if (AppManager.getEditMode() != EditMode.MIDDLE_DRAG) {
                // MIDDLE_DRAGのときは，pictPianoRoll_MouseMoveでrefreshScreenされるので，それ以外のときだけ描画・
                refreshScreen(true);
            }
        }
        #endregion

        //BOOKMARK: waveView
        #region waveView
        public void waveView_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle) {
                // ツールをポインター <--> 鉛筆に切り替える
                if (e.Y < trackSelector.getHeight() - TrackSelector.OFFSET_TRACK_TAB * 2) {
                    if (AppManager.getSelectedTool() == EditTool.ARROW) {
                        AppManager.setSelectedTool(EditTool.PENCIL);
                    } else {
                        AppManager.setSelectedTool(EditTool.ARROW);
                    }
                }
            }
        }

        public void waveView_MouseDown(Object sender, MouseEventArgs e)
        {
#if DEBUG
            sout.println("waveView_MouseDown; isMiddleButtonDowned=" + isMouseMiddleButtonDowned(e.Button));
#endif
            if (isMouseMiddleButtonDowned(e.Button)) {
                mEditCurveMode = CurveEditMode.MIDDLE_DRAG;
                mButtonInitial = new Point(e.X, e.Y);
                mMiddleButtonHScroll = hScroll.Value;
                this.Cursor = HAND;
            }
        }

        public void waveView_MouseUp(Object sender, MouseEventArgs e)
        {
            if (mEditCurveMode == CurveEditMode.MIDDLE_DRAG) {
                mEditCurveMode = CurveEditMode.NONE;
                this.Cursor = Cursors.Default;
            }
        }

        public void waveView_MouseMove(Object sender, MouseEventArgs e)
        {
            if (mEditCurveMode == CurveEditMode.MIDDLE_DRAG) {
                int draft = computeHScrollValueForMiddleDrag(e.X);
                if (hScroll.Value != draft) {
                    hScroll.Value = draft;
                }
            }
        }
        #endregion

        //BOOKMARK: hScroll
        #region hScroll
        public void hScroll_Enter(Object sender, EventArgs e)
        {
            focusPianoRoll();
        }

        public void hScroll_Resize(Object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized) {
                updateScrollRangeHorizontal();
            }
        }

        public void hScroll_ValueChanged(Object sender, EventArgs e)
        {
            int stdx = calculateStartToDrawX();
            controller.setStartToDrawX(stdx);
            if (AppManager.getEditMode() != EditMode.MIDDLE_DRAG) {
                // MIDDLE_DRAGのときは，pictPianoRoll_MouseMoveでrefreshScreenされるので，それ以外のときだけ描画・
                refreshScreen(true);
            }
        }
        #endregion

        //BOOKMARK: picturePositionIndicator
        #region picturePositionIndicator
        public void picturePositionIndicator_MouseWheel(Object sender, MouseEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#picturePositionIndicator_MouseWheel");
#endif
            hScroll.Value = computeScrollValueFromWheelDelta(e.Delta);
        }

        public void picturePositionIndicator_MouseClick(Object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && 0 < e.Y && e.Y <= 18 && AppManager.keyWidth < e.X) {
                // クリックされた位置でのクロックを保存
                int clock = AppManager.clockFromXCoord(e.X);
                int unit = AppManager.getPositionQuantizeClock();
                clock = doQuantize(clock, unit);
                if (clock < 0) {
                    clock = 0;
                }
                mPositionIndicatorPopupShownClock = clock;
                cMenuPositionIndicator.Show(picturePositionIndicator, e.X, e.Y);
            }
        }

        public void picturePositionIndicator_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.X < AppManager.keyWidth || this.Width - 3 < e.X) {
                return;
            }
            if (e.Button == MouseButtons.Left) {
                VsqFileEx vsq = AppManager.getVsqFile();
                if (18 < e.Y && e.Y <= 32) {
                    #region テンポの変更
#if DEBUG
                    AppManager.debugWriteLine("TempoChange");
#endif
                    AppManager.itemSelection.clearEvent();
                    AppManager.itemSelection.clearTimesig();

                    if (AppManager.itemSelection.getTempoCount() > 0) {
                        #region テンポ変更があった場合
                        int index = -1;
                        int clock = AppManager.itemSelection.getLastTempoClock();
                        for (int i = 0; i < vsq.TempoTable.Count; i++) {
                            if (clock == vsq.TempoTable[i].Clock) {
                                index = i;
                                break;
                            }
                        }
                        if (index >= 0) {
                            if (AppManager.getSelectedTool() == EditTool.ERASER) {
                                #region ツールがEraser
                                if (vsq.TempoTable[index].Clock == 0) {
                                    string msg = _("Cannot remove first symbol of track!");
                                    statusLabel.Text = msg;
                                    SystemSounds.Asterisk.Play();
                                    return;
                                }
                                CadenciiCommand run = new CadenciiCommand(
                                    VsqCommand.generateCommandUpdateTempo(vsq.TempoTable[index].Clock,
                                                                           vsq.TempoTable[index].Clock,
                                                                           -1));
                                AppManager.editHistory.register(vsq.executeCommand(run));
                                setEdited(true);
                                #endregion
                            } else {
                                #region ツールがEraser以外
                                TempoTableEntry tte = vsq.TempoTable[index];
                                AppManager.itemSelection.clearTempo();
                                AppManager.itemSelection.addTempo(tte.Clock);
                                int bar_count = vsq.getBarCountFromClock(tte.Clock);
                                int bar_top_clock = vsq.getClockFromBarCount(bar_count);
                                //int local_denominator, local_numerator;
                                Timesig timesig = vsq.getTimesigAt(tte.Clock);
                                int clock_per_beat = 480 * 4 / timesig.denominator;
                                int clocks_in_bar = tte.Clock - bar_top_clock;
                                int beat_in_bar = clocks_in_bar / clock_per_beat + 1;
                                int clocks_in_beat = clocks_in_bar - (beat_in_bar - 1) * clock_per_beat;
                                FormTempoConfig dlg = null;
                                try {
                                    dlg = new FormTempoConfig(bar_count, beat_in_bar, timesig.numerator, clocks_in_beat, clock_per_beat, (float)(6e7 / tte.Tempo), AppManager.getVsqFile().getPreMeasure());
                                    dlg.Location = getFormPreferedLocation(dlg);
                                    DialogResult dr = AppManager.showModalDialog(dlg, this);
                                    if (dr == DialogResult.OK) {
                                        int new_beat = dlg.getBeatCount();
                                        int new_clocks_in_beat = dlg.getClock();
                                        int new_clock = bar_top_clock + (new_beat - 1) * clock_per_beat + new_clocks_in_beat;
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandUpdateTempo(new_clock, new_clock, (int)(6e7 / (double)dlg.getTempo())));
                                        AppManager.editHistory.register(vsq.executeCommand(run));
                                        setEdited(true);
                                        refreshScreen();
                                    }
                                } catch (Exception ex) {
                                    Logger.write(typeof(FormMain) + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex + "\n");
                                    serr.println("FormMain#picturePositionIndicator_MouseDoubleClick; ex=" + ex);
                                } finally {
                                    if (dlg != null) {
                                        try {
                                            dlg.Close();
                                        } catch (Exception ex2) {
                                            Logger.write(typeof(FormMain) + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex2 + "\n");
                                            serr.println("FormMain#picturePositionIndicator_MouseDoubleClick; ex2=" + ex2);
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion
                    } else {
                        #region テンポ変更がなかった場合
                        AppManager.itemSelection.clearEvent();
                        AppManager.itemSelection.clearTempo();
                        AppManager.itemSelection.clearTimesig();
                        EditTool selected = AppManager.getSelectedTool();
                        if (selected == EditTool.PENCIL ||
                            selected == EditTool.LINE) {
                            int changing_clock = AppManager.clockFromXCoord(e.X);
                            int changing_tempo = vsq.getTempoAt(changing_clock);
                            int bar_count;
                            int bar_top_clock;
                            int local_denominator, local_numerator;
                            bar_count = vsq.getBarCountFromClock(changing_clock);
                            bar_top_clock = vsq.getClockFromBarCount(bar_count);
                            int index2 = -1;
                            for (int i = 0; i < vsq.TimesigTable.Count; i++) {
                                if (vsq.TimesigTable[i].BarCount > bar_count) {
                                    index2 = i;
                                    break;
                                }
                            }
                            if (index2 >= 1) {
                                local_denominator = vsq.TimesigTable[index2 - 1].Denominator;
                                local_numerator = vsq.TimesigTable[index2 - 1].Numerator;
                            } else {
                                local_denominator = vsq.TimesigTable[0].Denominator;
                                local_numerator = vsq.TimesigTable[0].Numerator;
                            }
                            int clock_per_beat = 480 * 4 / local_denominator;
                            int clocks_in_bar = changing_clock - bar_top_clock;
                            int beat_in_bar = clocks_in_bar / clock_per_beat + 1;
                            int clocks_in_beat = clocks_in_bar - (beat_in_bar - 1) * clock_per_beat;
                            FormTempoConfig dlg = null;
                            try {
                                dlg = new FormTempoConfig(bar_count - vsq.getPreMeasure() + 1,
                                                           beat_in_bar,
                                                           local_numerator,
                                                           clocks_in_beat,
                                                           clock_per_beat,
                                                           (float)(6e7 / changing_tempo),
                                                           vsq.getPreMeasure());
                                dlg.Location = getFormPreferedLocation(dlg);
                                DialogResult dr = AppManager.showModalDialog(dlg, this);
                                if (dr == DialogResult.OK) {
                                    int new_beat = dlg.getBeatCount();
                                    int new_clocks_in_beat = dlg.getClock();
                                    int new_clock = bar_top_clock + (new_beat - 1) * clock_per_beat + new_clocks_in_beat;
#if DEBUG
                                    AppManager.debugWriteLine("    new_beat=" + new_beat);
                                    AppManager.debugWriteLine("    new_clocks_in_beat=" + new_clocks_in_beat);
                                    AppManager.debugWriteLine("    changing_clock=" + changing_clock);
                                    AppManager.debugWriteLine("    new_clock=" + new_clock);
#endif
                                    CadenciiCommand run = new CadenciiCommand(
                                        VsqCommand.generateCommandUpdateTempo(new_clock, new_clock, (int)(6e7 / (double)dlg.getTempo())));
                                    AppManager.editHistory.register(vsq.executeCommand(run));
                                    setEdited(true);
                                    refreshScreen();
                                }
                            } catch (Exception ex) {
                                Logger.write(typeof(FormMain) + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex + "\n");
                            } finally {
                                if (dlg != null) {
                                    try {
                                        dlg.Close();
                                    } catch (Exception ex2) {
                                        Logger.write(typeof(FormMain) + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex2 + "\n");
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
                    #endregion
                } else if (32 < e.Y && e.Y <= picturePositionIndicator.Height - 1) {
                    #region 拍子の変更
                    AppManager.itemSelection.clearEvent();
                    AppManager.itemSelection.clearTempo();
                    if (AppManager.itemSelection.getTimesigCount() > 0) {
                        #region 拍子変更があった場合
                        int index = 0;
                        int last_barcount = AppManager.itemSelection.getLastTimesigBarcount();
                        for (int i = 0; i < vsq.TimesigTable.Count; i++) {
                            if (vsq.TimesigTable[i].BarCount == last_barcount) {
                                index = i;
                                break;
                            }
                        }
                        if (AppManager.getSelectedTool() == EditTool.ERASER) {
                            #region ツールがEraser
                            if (vsq.TimesigTable[index].Clock == 0) {
                                string msg = _("Cannot remove first symbol of track!");
                                statusLabel.Text = msg;
                                SystemSounds.Asterisk.Play();
                                return;
                            }
                            int barcount = vsq.TimesigTable[index].BarCount;
                            CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandUpdateTimesig(barcount, barcount, -1, -1));
                            AppManager.editHistory.register(vsq.executeCommand(run));
                            setEdited(true);
                            #endregion
                        } else {
                            #region ツールがEraser以外
                            int pre_measure = vsq.getPreMeasure();
                            int clock = AppManager.clockFromXCoord(e.X);
                            int bar_count = vsq.getBarCountFromClock(clock);
                            int total_clock = vsq.TotalClocks;
                            Timesig timesig = vsq.getTimesigAt(clock);
                            bool num_enabled = !(bar_count == 0);
                            FormBeatConfigController dlg = null;
                            try {
                                dlg = new FormBeatConfigController(bar_count - pre_measure + 1, timesig.numerator, timesig.denominator, num_enabled, pre_measure);
                                var p = getFormPreferedLocation(dlg.getWidth(), dlg.getHeight());
                                dlg.setLocation(p.X, p.Y);
                                int dr = AppManager.showModalDialog(dlg.getUi(), this);
                                if (dr == 1) {
                                    if (dlg.isEndSpecified()) {
                                        int[] new_barcounts = new int[2];
                                        int[] numerators = new int[2];
                                        int[] denominators = new int[2];
                                        int[] barcounts = new int[2];
                                        new_barcounts[0] = dlg.getStart() + pre_measure - 1;
                                        new_barcounts[1] = dlg.getEnd() + pre_measure - 1;
                                        numerators[0] = dlg.getNumerator();
                                        denominators[0] = dlg.getDenominator();
                                        numerators[1] = timesig.numerator;
                                        denominators[1] = timesig.denominator;
                                        barcounts[0] = bar_count;
                                        barcounts[1] = dlg.getEnd() + pre_measure - 1;
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandUpdateTimesigRange(barcounts, new_barcounts, numerators, denominators));
                                        AppManager.editHistory.register(vsq.executeCommand(run));
                                        setEdited(true);
                                    } else {
#if DEBUG
                                        sout.println("picturePositionIndicator_MouseDoubleClick");
                                        sout.println("    bar_count=" + bar_count);
                                        sout.println("    dlg.Start+pre_measure-1=" + (dlg.getStart() + pre_measure - 1));
#endif
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandUpdateTimesig(bar_count,
                                                                                     dlg.getStart() + pre_measure - 1,
                                                                                     dlg.getNumerator(),
                                                                                     dlg.getDenominator()));
                                        AppManager.editHistory.register(vsq.executeCommand(run));
                                        setEdited(true);
                                    }
                                }
                            } catch (Exception ex) {
                                Logger.write(typeof(FormMain) + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex + "\n");
                                serr.println("FormMain#picturePositionIndicator_MouseDoubleClick; ex=" + ex);
                            } finally {
                                if (dlg != null) {
                                    try {
                                        dlg.close();
                                    } catch (Exception ex2) {
                                        Logger.write(typeof(FormMain) + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex2 + "\n");
                                        serr.println("FormMain#picturePositionIndicator_MouseDoubleClic; ex2=" + ex2);
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion
                    } else {
                        #region 拍子変更がなかった場合
                        AppManager.itemSelection.clearEvent();
                        AppManager.itemSelection.clearTempo();
                        AppManager.itemSelection.clearTimesig();
                        EditTool selected = AppManager.getSelectedTool();
                        if (selected == EditTool.PENCIL ||
                            selected == EditTool.LINE) {
                            int pre_measure = AppManager.getVsqFile().getPreMeasure();
                            int clock = AppManager.clockFromXCoord(e.X);
                            int bar_count = AppManager.getVsqFile().getBarCountFromClock(clock);
                            int numerator, denominator;
                            Timesig timesig = AppManager.getVsqFile().getTimesigAt(clock);
                            int total_clock = AppManager.getVsqFile().TotalClocks;
                            //int max_barcount = AppManager.VsqFile.getBarCountFromClock( total_clock ) - pre_measure + 1;
                            //int min_barcount = 1;
#if DEBUG
                            AppManager.debugWriteLine("FormMain.picturePositionIndicator_MouseClick; bar_count=" + (bar_count - pre_measure + 1));
#endif
                            FormBeatConfigController dlg = null;
                            try {
                                dlg = new FormBeatConfigController(bar_count - pre_measure + 1, timesig.numerator, timesig.denominator, true, pre_measure);
                                var p = getFormPreferedLocation(dlg.getWidth(), dlg.getHeight());
                                dlg.setLocation(p.X, p.Y);
                                int dr = AppManager.showModalDialog(dlg.getUi(), this);
                                if (dr == 1) {
                                    if (dlg.isEndSpecified()) {
                                        int[] new_barcounts = new int[2];
                                        int[] numerators = new int[2];
                                        int[] denominators = new int[2];
                                        int[] barcounts = new int[2];
                                        new_barcounts[0] = dlg.getStart() + pre_measure - 1;
                                        new_barcounts[1] = dlg.getEnd() + pre_measure - 1 + 1;
                                        numerators[0] = dlg.getNumerator();
                                        numerators[1] = timesig.numerator;

                                        denominators[0] = dlg.getDenominator();
                                        denominators[1] = timesig.denominator;

                                        barcounts[0] = dlg.getStart() + pre_measure - 1;
                                        barcounts[1] = dlg.getEnd() + pre_measure - 1 + 1;
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandUpdateTimesigRange(barcounts, new_barcounts, numerators, denominators));
                                        AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                                        setEdited(true);
                                    } else {
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandUpdateTimesig(bar_count,
                                                                           dlg.getStart() + pre_measure - 1,
                                                                           dlg.getNumerator(),
                                                                           dlg.getDenominator()));
                                        AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                                        setEdited(true);
                                    }
                                }
                            } catch (Exception ex) {
                                Logger.write(typeof(FormMain) + ".picutrePositionIndicator_MouseDoubleClick; ex=" + ex + "\n");
                            } finally {
                                if (dlg != null) {
                                    try {
                                        dlg.close();
                                    } catch (Exception ex2) {
                                        Logger.write(typeof(FormMain) + ".picturePositionIndicator_MouseDoubleClick; ex=" + ex2 + "\n");
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
                    #endregion
                }
                picturePositionIndicator.Refresh();
                pictPianoRoll.Refresh();
            }
        }

        public void picturePositionIndicator_MouseDown(Object sender, MouseEventArgs e)
        {
            if (e.X < AppManager.keyWidth || this.Width - 3 < e.X) {
                return;
            }

            mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
            Keys modifiers = Control.ModifierKeys;
            VsqFileEx vsq = AppManager.getVsqFile();
            if (e.Button == MouseButtons.Left) {
                if (0 <= e.Y && e.Y <= 18) {
                    #region スタート/エンドマーク
                    int tolerance = AppManager.editorConfig.PxTolerance;
                    int start_marker_width = Properties.Resources.start_marker.Width;
                    int end_marker_width = Properties.Resources.end_marker.Width;
                    int startx = AppManager.xCoordFromClocks(vsq.config.StartMarker);
                    int endx = AppManager.xCoordFromClocks(vsq.config.EndMarker);

                    // マウスの当たり判定が重なるようなら，判定幅を最小にする
                    int start0 = startx - tolerance;
                    int start1 = startx + start_marker_width + tolerance;
                    int end0 = endx - end_marker_width - tolerance;
                    int end1 = endx + tolerance;
                    if (vsq.config.StartMarkerEnabled && vsq.config.EndMarkerEnabled) {
                        if (start0 < end1 && end1 < start1 ||
                            start1 < end0 && end0 < start1) {
                            start0 = startx;
                            start1 = startx + start_marker_width;
                            end0 = endx - end_marker_width;
                            end1 = endx;
                        }
                    }

                    if (vsq.config.StartMarkerEnabled) {
                        if (start0 <= e.X && e.X <= start1) {
                            mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.MARK_START;
                        }
                    }
                    if (vsq.config.EndMarkerEnabled && mPositionIndicatorMouseDownMode != PositionIndicatorMouseDownMode.MARK_START) {
                        if (end0 <= e.X && e.X <= end1) {
                            mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.MARK_END;
                        }
                    }
                    #endregion
                } else if (18 < e.Y && e.Y <= 32) {
                    #region テンポ
                    int index = -1;
                    int count = AppManager.getVsqFile().TempoTable.Count;
                    for (int i = 0; i < count; i++) {
                        int clock = AppManager.getVsqFile().TempoTable[i].Clock;
                        int x = AppManager.xCoordFromClocks(clock);
                        if (x < 0) {
                            continue;
                        } else if (this.Width < x) {
                            break;
                        }
                        string s = PortUtil.formatDecimal("#.00", 60e6 / (float)AppManager.getVsqFile().TempoTable[i].Tempo);
                        Dimension size = Util.measureString(s, new Font(AppManager.editorConfig.ScreenFontName, java.awt.Font.PLAIN, AppManager.FONT_SIZE8));
                        if (Utility.isInRect(new Point(e.X, e.Y), new Rectangle(x, 14, (int)size.width, 14))) {
                            index = i;
                            break;
                        }
                    }

                    if (index >= 0) {
                        int clock = AppManager.getVsqFile().TempoTable[index].Clock;
                        if (AppManager.getSelectedTool() != EditTool.ERASER) {
                            int mouse_clock = AppManager.clockFromXCoord(e.X);
                            mTempoDraggingDeltaClock = mouse_clock - clock;
                            mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.TEMPO;
                        }
                        if ((modifiers & Keys.Shift) == Keys.Shift) {
                            if (AppManager.itemSelection.getTempoCount() > 0) {
                                int last_clock = AppManager.itemSelection.getLastTempoClock();
                                int start = Math.Min(last_clock, clock);
                                int end = Math.Max(last_clock, clock);
                                for (int i = 0; i < AppManager.getVsqFile().TempoTable.Count; i++) {
                                    int tclock = AppManager.getVsqFile().TempoTable[i].Clock;
                                    if (tclock < start) {
                                        continue;
                                    } else if (end < tclock) {
                                        break;
                                    }
                                    if (start <= tclock && tclock <= end) {
                                        AppManager.itemSelection.addTempo(tclock);
                                    }
                                }
                            } else {
                                AppManager.itemSelection.addTempo(clock);
                            }
                        } else if ((modifiers & s_modifier_key) == s_modifier_key) {
                            if (AppManager.itemSelection.isTempoContains(clock)) {
                                AppManager.itemSelection.removeTempo(clock);
                            } else {
                                AppManager.itemSelection.addTempo(clock);
                            }
                        } else {
                            if (!AppManager.itemSelection.isTempoContains(clock)) {
                                AppManager.itemSelection.clearTempo();
                            }
                            AppManager.itemSelection.addTempo(clock);
                        }
                    } else {
                        AppManager.itemSelection.clearEvent();
                        AppManager.itemSelection.clearTempo();
                        AppManager.itemSelection.clearTimesig();
                    }
                    #endregion
                } else if (32 < e.Y && e.Y <= picturePositionIndicator.Height - 1) {
                    #region 拍子
                    // クリック位置に拍子が表示されているかどうか検査
                    int index = -1;
                    for (int i = 0; i < AppManager.getVsqFile().TimesigTable.Count; i++) {
                        string s = AppManager.getVsqFile().TimesigTable[i].Numerator + "/" + AppManager.getVsqFile().TimesigTable[i].Denominator;
                        int x = AppManager.xCoordFromClocks(AppManager.getVsqFile().TimesigTable[i].Clock);
                        Dimension size = Util.measureString(s, new Font(AppManager.editorConfig.ScreenFontName, java.awt.Font.PLAIN, AppManager.FONT_SIZE8));
                        if (Utility.isInRect(new Point(e.X, e.Y), new Rectangle(x, 28, (int)size.width, 14))) {
                            index = i;
                            break;
                        }
                    }

                    if (index >= 0) {
                        int barcount = AppManager.getVsqFile().TimesigTable[index].BarCount;
                        if (AppManager.getSelectedTool() != EditTool.ERASER) {
                            int barcount_clock = AppManager.getVsqFile().getClockFromBarCount(barcount);
                            int mouse_clock = AppManager.clockFromXCoord(e.X);
                            mTimesigDraggingDeltaClock = mouse_clock - barcount_clock;
                            mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.TIMESIG;
                        }
                        if ((modifiers & Keys.Shift) == Keys.Shift) {
                            if (AppManager.itemSelection.getTimesigCount() > 0) {
                                int last_barcount = AppManager.itemSelection.getLastTimesigBarcount();
                                int start = Math.Min(last_barcount, barcount);
                                int end = Math.Max(last_barcount, barcount);
                                for (int i = 0; i < AppManager.getVsqFile().TimesigTable.Count; i++) {
                                    int tbarcount = AppManager.getVsqFile().TimesigTable[i].BarCount;
                                    if (tbarcount < start) {
                                        continue;
                                    } else if (end < tbarcount) {
                                        break;
                                    }
                                    if (start <= tbarcount && tbarcount <= end) {
                                        AppManager.itemSelection.addTimesig(AppManager.getVsqFile().TimesigTable[i].BarCount);
                                    }
                                }
                            } else {
                                AppManager.itemSelection.addTimesig(barcount);
                            }
                        } else if ((modifiers & s_modifier_key) == s_modifier_key) {
                            if (AppManager.itemSelection.isTimesigContains(barcount)) {
                                AppManager.itemSelection.removeTimesig(barcount);
                            } else {
                                AppManager.itemSelection.addTimesig(barcount);
                            }
                        } else {
                            if (!AppManager.itemSelection.isTimesigContains(barcount)) {
                                AppManager.itemSelection.clearTimesig();
                            }
                            AppManager.itemSelection.addTimesig(barcount);
                        }
                    } else {
                        AppManager.itemSelection.clearEvent();
                        AppManager.itemSelection.clearTempo();
                        AppManager.itemSelection.clearTimesig();
                    }
                    #endregion
                }
            }
            refreshScreen();
        }

        public void picturePositionIndicator_MouseUp(Object sender, MouseEventArgs e)
        {
            Keys modifiers = Control.ModifierKeys;
#if DEBUG
            AppManager.debugWriteLine("picturePositionIndicator_MouseClick");
#endif
            if (e.Button == MouseButtons.Left) {
                VsqFileEx vsq = AppManager.getVsqFile();
                if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.NONE) {
                    if (4 <= e.Y && e.Y <= 18) {
                        #region マーカー位置の変更
                        int clock = AppManager.clockFromXCoord(e.X);
                        if (AppManager.editorConfig.getPositionQuantize() != QuantizeMode.off) {
                            int unit = AppManager.getPositionQuantizeClock();
                            clock = doQuantize(clock, unit);
                        }
                        if (AppManager.isPlaying()) {
                            AppManager.setPlaying(false, this);
                            AppManager.setCurrentClock(clock);
                            AppManager.setPlaying(true, this);
                        } else {
                            AppManager.setCurrentClock(clock);
                        }
                        refreshScreen();
                        #endregion
                    } else if (18 < e.Y && e.Y <= 32) {
                        #region テンポの変更
#if DEBUG
                        AppManager.debugWriteLine("TempoChange");
#endif
                        AppManager.itemSelection.clearEvent();
                        AppManager.itemSelection.clearTimesig();
                        if (AppManager.itemSelection.getTempoCount() > 0) {
                            #region テンポ変更があった場合
                            int index = -1;
                            int clock = AppManager.itemSelection.getLastTempoClock();
                            for (int i = 0; i < vsq.TempoTable.Count; i++) {
                                if (clock == vsq.TempoTable[i].Clock) {
                                    index = i;
                                    break;
                                }
                            }
                            if (index >= 0 && AppManager.getSelectedTool() == EditTool.ERASER) {
                                #region ツールがEraser
                                if (vsq.TempoTable[index].Clock == 0) {
                                    string msg = _("Cannot remove first symbol of track!");
                                    statusLabel.Text = msg;
                                    SystemSounds.Asterisk.Play();
                                    return;
                                }
                                CadenciiCommand run = new CadenciiCommand(
                                    VsqCommand.generateCommandUpdateTempo(vsq.TempoTable[index].Clock,
                                                                           vsq.TempoTable[index].Clock,
                                                                           -1));
                                AppManager.editHistory.register(vsq.executeCommand(run));
                                setEdited(true);
                                #endregion
                            }
                            #endregion
                        }
                        mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
                        #endregion
                    } else if (32 < e.Y && e.Y <= picturePositionIndicator.Height - 1) {
                        #region 拍子の変更
                        AppManager.itemSelection.clearEvent();
                        AppManager.itemSelection.clearTempo();
                        if (AppManager.itemSelection.getTimesigCount() > 0) {
                            #region 拍子変更があった場合
                            int index = 0;
                            int last_barcount = AppManager.itemSelection.getLastTimesigBarcount();
                            for (int i = 0; i < vsq.TimesigTable.Count; i++) {
                                if (vsq.TimesigTable[i].BarCount == last_barcount) {
                                    index = i;
                                    break;
                                }
                            }
                            if (AppManager.getSelectedTool() == EditTool.ERASER) {
                                #region ツールがEraser
                                if (vsq.TimesigTable[index].Clock == 0) {
                                    string msg = _("Cannot remove first symbol of track!");
                                    statusLabel.Text = msg;
                                    SystemSounds.Asterisk.Play();
                                    return;
                                }
                                int barcount = vsq.TimesigTable[index].BarCount;
                                CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandUpdateTimesig(barcount, barcount, -1, -1));
                                AppManager.editHistory.register(vsq.executeCommand(run));
                                setEdited(true);
                                #endregion
                            }
                            #endregion
                        }
                        #endregion
                    }
                } else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TEMPO) {
                    int count = AppManager.itemSelection.getTempoCount();
                    int[] clocks = new int[count];
                    int[] new_clocks = new int[count];
                    int[] tempos = new int[count];
                    int i = -1;
                    bool contains_first_tempo = false;
                    foreach (var item in AppManager.itemSelection.getTempoIterator()) {
                        int clock = item.getKey();
                        i++;
                        clocks[i] = clock;
                        if (clock == 0) {
                            contains_first_tempo = true;
                            break;
                        }
                        TempoTableEntry editing = AppManager.itemSelection.getTempo(clock).editing;
                        new_clocks[i] = editing.Clock;
                        tempos[i] = editing.Tempo;
                    }
                    if (contains_first_tempo) {
                        SystemSounds.Asterisk.Play();
                    } else {
                        CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandUpdateTempoRange(clocks, new_clocks, tempos));
                        AppManager.editHistory.register(vsq.executeCommand(run));
                        setEdited(true);
                    }
                } else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TIMESIG) {
                    int count = AppManager.itemSelection.getTimesigCount();
                    int[] barcounts = new int[count];
                    int[] new_barcounts = new int[count];
                    int[] numerators = new int[count];
                    int[] denominators = new int[count];
                    int i = -1;
                    bool contains_first_bar = false;
                    foreach (var item in AppManager.itemSelection.getTimesigIterator()) {
                        int bar = item.getKey();
                        i++;
                        barcounts[i] = bar;
                        if (bar == 0) {
                            contains_first_bar = true;
                            break;
                        }
                        TimeSigTableEntry editing = AppManager.itemSelection.getTimesig(bar).editing;
                        new_barcounts[i] = editing.BarCount;
                        numerators[i] = editing.Numerator;
                        denominators[i] = editing.Denominator;
                    }
                    if (contains_first_bar) {
                        SystemSounds.Asterisk.Play();
                    } else {
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandUpdateTimesigRange(barcounts, new_barcounts, numerators, denominators));
                        AppManager.editHistory.register(vsq.executeCommand(run));
                        setEdited(true);
                    }
                }
            }
            mPositionIndicatorMouseDownMode = PositionIndicatorMouseDownMode.NONE;
            pictPianoRoll.Refresh();
            picturePositionIndicator.Refresh();
        }

        public void picturePositionIndicator_MouseMove(Object sender, MouseEventArgs e)
        {
            VsqFileEx vsq = AppManager.getVsqFile();
            if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TEMPO) {
                int clock = AppManager.clockFromXCoord(e.X) - mTempoDraggingDeltaClock;
                int step = AppManager.getPositionQuantizeClock();
                clock = doQuantize(clock, step);
                int last_clock = AppManager.itemSelection.getLastTempoClock();
                int dclock = clock - last_clock;
                foreach (var item in AppManager.itemSelection.getTempoIterator()) {
                    int key = item.getKey();
                    AppManager.itemSelection.getTempo(key).editing.Clock = AppManager.itemSelection.getTempo(key).original.Clock + dclock;
                }
                picturePositionIndicator.Refresh();
            } else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.TIMESIG) {
                int clock = AppManager.clockFromXCoord(e.X) - mTimesigDraggingDeltaClock;
                int barcount = vsq.getBarCountFromClock(clock);
                int last_barcount = AppManager.itemSelection.getLastTimesigBarcount();
                int dbarcount = barcount - last_barcount;
                foreach (var item in AppManager.itemSelection.getTimesigIterator()) {
                    int bar = item.getKey();
                    AppManager.itemSelection.getTimesig(bar).editing.BarCount = AppManager.itemSelection.getTimesig(bar).original.BarCount + dbarcount;
                }
                picturePositionIndicator.Refresh();
            } else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.MARK_START) {
                int clock = AppManager.clockFromXCoord(e.X);
                int unit = AppManager.getPositionQuantizeClock();
                clock = doQuantize(clock, unit);
                if (clock < 0) {
                    clock = 0;
                }
                int draft_start = Math.Min(clock, vsq.config.EndMarker);
                int draft_end = Math.Max(clock, vsq.config.EndMarker);
                if (draft_start != vsq.config.StartMarker) {
                    vsq.config.StartMarker = draft_start;
                    setEdited(true);
                }
                if (draft_end != vsq.config.EndMarker) {
                    vsq.config.EndMarker = draft_end;
                    setEdited(true);
                }
                refreshScreen();
            } else if (mPositionIndicatorMouseDownMode == PositionIndicatorMouseDownMode.MARK_END) {
                int clock = AppManager.clockFromXCoord(e.X);
                int unit = AppManager.getPositionQuantizeClock();
                clock = doQuantize(clock, unit);
                if (clock < 0) {
                    clock = 0;
                }
                int draft_start = Math.Min(clock, vsq.config.StartMarker);
                int draft_end = Math.Max(clock, vsq.config.StartMarker);
                if (draft_start != vsq.config.StartMarker) {
                    vsq.config.StartMarker = draft_start;
                    setEdited(true);
                }
                if (draft_end != vsq.config.EndMarker) {
                    vsq.config.EndMarker = draft_end;
                    setEdited(true);
                }
                refreshScreen();
            }
        }

        public void picturePositionIndicator_Paint(Object sender, PaintEventArgs e)
        {
            Graphics2D g = new Graphics2D(e.Graphics);
            picturePositionIndicatorDrawTo(g);
#if MONITOR_FPS
            g.setColor(Color.red);
            g.setFont(AppManager.baseFont10Bold);
            g.drawString(PortUtil.formatDecimal("#.00", mFps) + " / " + PortUtil.formatDecimal("#.00", mFps2), 5, 5);
#endif
        }

        public void picturePositionIndicator_PreviewKeyDown(Object sender, PreviewKeyDownEventArgs e)
        {
            KeyEventArgs e0 = new KeyEventArgs(e.KeyData);
            processSpecialShortcutKey(e0, true);
        }
        #endregion

        //BOOKMARK: trackBar
        #region trackBar
        public void trackBar_Enter(Object sender, EventArgs e)
        {
            focusPianoRoll();
        }

        public void trackBar_ValueChanged(Object sender, EventArgs e)
        {
            controller.setScaleX(getScaleXFromTrackBarValue(trackBar.Value));
            controller.setStartToDrawX(calculateStartToDrawX());
            updateDrawObjectList();
            Refresh();
        }
        #endregion

        //BOOKMARK: menuHelp
        #region menuHelp
        public void menuHelpAbout_Click(Object sender, EventArgs e)
        {
#if DEBUG
            sout.println("FormMain#menuHelpAbout_Click");
#endif

            string version_str = Utility.getVersion() + "\n\n" +
                                 Utility.getAssemblyNameAndFileVersion(typeof(cadencii.apputil.Util)) + "\n" +
                                 Utility.getAssemblyNameAndFileVersion(typeof(cadencii.media.Wave)) + "\n" +
                                 Utility.getAssemblyNameAndFileVersion(typeof(cadencii.vsq.VsqFile)) + "\n" +
                                 Utility.getAssemblyNameAndFileVersion(typeof(cadencii.math));
            if (mVersionInfo == null) {
                mVersionInfo = new VersionInfo(_APP_NAME, version_str);
                mVersionInfo.setAuthorList(_CREDIT);
                mVersionInfo.Show();
            } else {
                if (mVersionInfo.IsDisposed) {
                    mVersionInfo = new VersionInfo(_APP_NAME, version_str);
                    mVersionInfo.setAuthorList(_CREDIT);
                }
                mVersionInfo.Show();
            }
        }

        public void menuHelpDebug_Click(Object sender, EventArgs e)
        {
#if DEBUG
            int.Parse("X");
            sout.println("FormMain#menuHelpDebug_Click");
#endif
        }

        public void menuHelpManual_Click(Object sender, EventArgs e)
        {
            // 現在のUI言語と同じ版のマニュアルファイルがあるかどうか探す
            string lang = Messaging.getLanguage();
            string pdf = Path.Combine(PortUtil.getApplicationStartupPath(), "manual_" + lang + ".pdf");
            if (!System.IO.File.Exists(pdf)) {
                // 無ければ英語版のマニュアルを表示することにする
                pdf = Path.Combine(PortUtil.getApplicationStartupPath(), "manual_en.pdf");
            }
            if (!System.IO.File.Exists(pdf)) {
                AppManager.showMessageBox(
                    _("file not found"),
                    _APP_NAME,
                    cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                    cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE);
                return;
            }
            System.Diagnostics.Process.Start(pdf);
        }

        public void menuHelpLogSwitch_CheckedChanged(Object sender, EventArgs e)
        {
            Logger.setEnabled(menuHelpLogSwitch.Checked);
            if (menuHelpLogSwitch.Checked) {
                menuHelpLogSwitch.Text = _("Enabled");
            } else {
                menuHelpLogSwitch.Text = _("Disabled");
            }
        }

        public void menuHelpLogOpen_Click(Object sender, EventArgs e)
        {
            string file = Logger.getPath();
            if (file == null || (file != null && (!System.IO.File.Exists(file)))) {
                // ログがまだできてないのでダイアログ出す
                AppManager.showMessageBox(
                    _("Log file has not generated yet."),
                    _("Info"),
                    PortUtil.OK_OPTION,
                    cadencii.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE);
                return;
            }

            // ログファイルを開く
            try {
                System.Diagnostics.Process.Start(file);
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".menuHelpLogOpen_Click; ex=" + ex + "\n");
            }
        }
        #endregion

        //BOOKMARK: trackSelector
        #region trackSelector
        public void trackSelector_CommandExecuted(Object sender, EventArgs e)
        {
            setEdited(true);
            refreshScreen();
        }

        public void trackSelector_MouseClick(Object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) {
                if (AppManager.keyWidth < e.X && e.X < trackSelector.getWidth()) {
                    if (trackSelector.getHeight() - TrackSelector.OFFSET_TRACK_TAB <= e.Y && e.Y <= trackSelector.getHeight()) {
                        cMenuTrackTab.Show(trackSelector, e.X, e.Y);
                    } else {
                        cMenuTrackSelector.Show(trackSelector, e.X, e.Y);
                    }
                }
            }
        }

        public void trackSelector_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle) {
                // ツールをポインター <--> 鉛筆に切り替える
                if (AppManager.keyWidth < e.X &&
                     e.Y < trackSelector.getHeight() - TrackSelector.OFFSET_TRACK_TAB * 2) {
                    if (AppManager.getSelectedTool() == EditTool.ARROW) {
                        AppManager.setSelectedTool(EditTool.PENCIL);
                    } else {
                        AppManager.setSelectedTool(EditTool.ARROW);
                    }
                }
            }
        }

        public void trackSelector_MouseDown(Object sender, MouseEventArgs e)
        {
            if (AppManager.keyWidth < e.X) {
                mMouseDownedTrackSelector = true;
                if (isMouseMiddleButtonDowned(e.Button)) {
                    mEditCurveMode = CurveEditMode.MIDDLE_DRAG;
                    mButtonInitial = new Point(e.X, e.Y);
                    mMiddleButtonHScroll = hScroll.Value;
                    this.Cursor = HAND;
                }
            }
        }

        public void trackSelector_MouseMove(Object sender, MouseEventArgs e)
        {
            if (mFormActivated && AppManager.mInputTextBox != null) {
                bool input_visible = !AppManager.mInputTextBox.IsDisposed && AppManager.mInputTextBox.Visible;
#if ENABLE_PROPERTY
                bool prop_editing = AppManager.propertyPanel.isEditing();
#else
                bool prop_editing = false;
#endif
                if (!input_visible && !prop_editing) {
                    trackSelector.requestFocus();
                }
            }
            if (e.Button == MouseButtons.None) {
                if (!timer.Enabled) {
                    refreshScreen(true);
                }
                return;
            }
            int parent_width = ((TrackSelector)sender).getWidth();
            if (mEditCurveMode == CurveEditMode.MIDDLE_DRAG) {
                if (AppManager.isPlaying()) {
                    return;
                }

                int draft = computeHScrollValueForMiddleDrag(e.X);
                if (hScroll.Value != draft) {
                    hScroll.Value = draft;
                }
            } else {
                if (mMouseDownedTrackSelector) {
                    if (mExtDragXTrackSelector == ExtDragXMode.NONE) {
                        if (AppManager.keyWidth > e.X) {
                            mExtDragXTrackSelector = ExtDragXMode.LEFT;
                        } else if (parent_width < e.X) {
                            mExtDragXTrackSelector = ExtDragXMode.RIGHT;
                        }
                    } else {
                        if (AppManager.keyWidth <= e.X && e.X <= parent_width) {
                            mExtDragXTrackSelector = ExtDragXMode.NONE;
                        }
                    }
                } else {
                    mExtDragXTrackSelector = ExtDragXMode.NONE;
                }

                if (mExtDragXTrackSelector != ExtDragXMode.NONE) {
                    double now = PortUtil.getCurrentTime();
                    double dt = now - mTimerDragLastIgnitted;
                    mTimerDragLastIgnitted = now;
                    int px_move = AppManager.editorConfig.MouseDragIncrement;
                    if (px_move / dt > AppManager.editorConfig.MouseDragMaximumRate) {
                        px_move = (int)(dt * AppManager.editorConfig.MouseDragMaximumRate);
                    }
                    px_move += 5;
                    if (mExtDragXTrackSelector == ExtDragXMode.LEFT) {
                        px_move *= -1;
                    }
                    double d_draft = hScroll.Value + px_move * controller.getScaleXInv();
                    if (d_draft < 0.0) {
                        d_draft = 0.0;
                    }
                    int draft = (int)d_draft;
                    if (hScroll.Maximum < draft) {
                        hScroll.Maximum = draft;
                    }
                    if (draft < hScroll.Minimum) {
                        draft = hScroll.Minimum;
                    }
                    hScroll.Value = draft;
                }
            }
            if (!timer.Enabled) {
                refreshScreen(true);
            }
        }

        public void trackSelector_MouseUp(Object sender, MouseEventArgs e)
        {
            mMouseDownedTrackSelector = false;
            if (mEditCurveMode == CurveEditMode.MIDDLE_DRAG) {
                mEditCurveMode = CurveEditMode.NONE;
                this.Cursor = Cursors.Default;
            }
        }

        public void trackSelector_MouseWheel(Object sender, MouseEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#trackSelector_MouseWheel");
#endif
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift) {
                double new_val = (double)vScroll.Value - e.Delta;
                int max = vScroll.Maximum - vScroll.Minimum;
                int min = vScroll.Minimum;
                if (new_val > max) {
                    vScroll.Value = max;
                } else if (new_val < min) {
                    vScroll.Value = min;
                } else {
                    vScroll.Value = (int)new_val;
                }
            } else {
                hScroll.Value = computeScrollValueFromWheelDelta(e.Delta);
            }
            refreshScreen();
        }

        public void trackSelector_PreferredMinHeightChanged(Object sender, EventArgs e)
        {
            if (menuVisualControlTrack.Checked) {
                splitContainer1.setPanel2MinSize(trackSelector.getPreferredMinSize());
#if DEBUG
                sout.println("FormMain#trackSelector_PreferredMinHeightChanged; splitContainer1.Panel2MinSize changed");
#endif
            }
        }

        public void trackSelector_PreviewKeyDown(Object sender, PreviewKeyDownEventArgs e)
        {
            KeyEventArgs e0 = new KeyEventArgs(e.KeyData);
            processSpecialShortcutKey(e0, true);
        }

        public void trackSelector_RenderRequired(Object sender, int track)
        {
            List<int> list = new List<int>();
            list.Add(track);
            AppManager.patchWorkToFreeze(this, list);
            /*int selected = AppManager.getSelected();
            Vector<Integer> t = new Vector<Integer>( Arrays.asList( PortUtil.convertIntArray( tracks ) ) );
            if ( t.contains( selected) ) {
                String file = fsys.combine( AppManager.getTempWaveDir(), selected + ".wav" );
                if ( PortUtil.isFileExists( file ) ) {
                    Thread loadwave_thread = new Thread( new ParameterizedThreadStart( this.loadWave ) );
                    loadwave_thread.IsBackground = true;
                    loadwave_thread.Start( new Object[]{ file, selected - 1 } );
                }
            }*/
        }

        public void trackSelector_SelectedCurveChanged(Object sender, CurveType type)
        {
            refreshScreen();
        }

        public void trackSelector_SelectedTrackChanged(Object sender, int selected)
        {
            AppManager.itemSelection.clearBezier();
            AppManager.itemSelection.clearEvent();
            AppManager.itemSelection.clearPoint();
            updateDrawObjectList();
            refreshScreen();
        }
        #endregion

        //BOOKMARK: cMenuPiano
        #region cMenuPiano*
        public void cMenuPianoDelete_Click(Object sender, EventArgs e)
        {
            deleteEvent();
        }

        public void cMenuPianoVibratoProperty_Click(Object sender, EventArgs e)
        {
            editNoteVibratoProperty();
        }

        public void cMenuPianoPaste_Click(Object sender, EventArgs e)
        {
            pasteEvent();
        }

        public void cMenuPianoCopy_Click(Object sender, EventArgs e)
        {
            copyEvent();
        }

        public void cMenuPianoCut_Click(Object sender, EventArgs e)
        {
            cutEvent();
        }

        public void cMenuPianoExpression_Click(Object sender, EventArgs e)
        {
            if (AppManager.itemSelection.getEventCount() > 0) {
                VsqFileEx vsq = AppManager.getVsqFile();
                int selected = AppManager.getSelected();
                SynthesizerType type = SynthesizerType.VOCALOID2;
                RendererKind kind = VsqFileEx.getTrackRendererKind(vsq.Track[selected]);
                if (kind == RendererKind.VOCALOID1) {
                    type = SynthesizerType.VOCALOID1;
                }
                VsqEvent original = AppManager.itemSelection.getLastEvent().original;
                FormNoteExpressionConfig dlg = null;
                try {
                    dlg = new FormNoteExpressionConfig(type, original.ID.NoteHeadHandle);
                    int id = AppManager.itemSelection.getLastEvent().original.InternalID;
                    dlg.setPMBendDepth(original.ID.PMBendDepth);
                    dlg.setPMBendLength(original.ID.PMBendLength);
                    dlg.setPMbPortamentoUse(original.ID.PMbPortamentoUse);
                    dlg.setDEMdecGainRate(original.ID.DEMdecGainRate);
                    dlg.setDEMaccent(original.ID.DEMaccent);
                    DialogResult dr = AppManager.showModalDialog(dlg, this);
                    if (dr == DialogResult.OK) {
                        VsqID copy = (VsqID)original.ID.clone();
                        copy.PMBendDepth = dlg.getPMBendDepth();
                        copy.PMBendLength = dlg.getPMBendLength();
                        copy.PMbPortamentoUse = dlg.getPMbPortamentoUse();
                        copy.DEMdecGainRate = dlg.getDEMdecGainRate();
                        copy.DEMaccent = dlg.getDEMaccent();
                        copy.NoteHeadHandle = dlg.getEditedNoteHeadHandle();
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandEventChangeIDContaints(selected, id, copy));
                        AppManager.editHistory.register(vsq.executeCommand(run));
                        setEdited(true);
                    }
                } catch (Exception ex) {
                    Logger.write(typeof(FormMain) + ".cMenuPianoExpression_Click; ex=" + ex + "\n");
                } finally {
                    if (dlg != null) {
                        try {
                            dlg.Close();
                        } catch (Exception ex2) {
                            Logger.write(typeof(FormMain) + ".cMenuPianoExpression_Click; ex=" + ex2 + "\n");
                        }
                    }
                }
            }
        }

        public void cMenuPianoPointer_Click(Object sender, EventArgs e)
        {
            AppManager.setSelectedTool(EditTool.ARROW);
        }

        public void cMenuPianoPencil_Click(Object sender, EventArgs e)
        {
            AppManager.setSelectedTool(EditTool.PENCIL);
        }

        public void cMenuPianoEraser_Click(Object sender, EventArgs e)
        {
            AppManager.setSelectedTool(EditTool.ERASER);
        }

        public void cMenuPianoGrid_Click(Object sender, EventArgs e)
        {
            bool new_v = !AppManager.isGridVisible();
            cMenuPianoGrid.Checked = new_v;
            AppManager.setGridVisible(new_v);
        }

        public void cMenuPianoUndo_Click(Object sender, EventArgs e)
        {
            undo();
        }

        public void cMenuPianoRedo_Click(Object sender, EventArgs e)
        {
            redo();
        }

        public void cMenuPianoSelectAllEvents_Click(Object sender, EventArgs e)
        {
            selectAllEvent();
        }

        public void cMenuPianoProperty_Click(Object sender, EventArgs e)
        {
            editNoteExpressionProperty();
        }

        public void cMenuPianoImportLyric_Click(Object sender, EventArgs e)
        {
            importLyric();
        }

        public void cMenuPiano_Opening(Object sender, CancelEventArgs e)
        {
            updateCopyAndPasteButtonStatus();
            cMenuPianoImportLyric.Enabled = AppManager.itemSelection.getLastEvent() != null;
        }

        public void cMenuPianoSelectAll_Click(Object sender, EventArgs e)
        {
            selectAll();
        }

        public void cMenuPianoFixed01_Click(Object sender, EventArgs e)
        {
            mPencilMode.setMode(PencilModeEnum.L1);
            updateCMenuPianoFixed();
        }

        public void cMenuPianoFixed02_Click(Object sender, EventArgs e)
        {
            mPencilMode.setMode(PencilModeEnum.L2);
            updateCMenuPianoFixed();
        }

        public void cMenuPianoFixed04_Click(Object sender, EventArgs e)
        {
            mPencilMode.setMode(PencilModeEnum.L4);
            updateCMenuPianoFixed();
        }

        public void cMenuPianoFixed08_Click(Object sender, EventArgs e)
        {
            mPencilMode.setMode(PencilModeEnum.L8);
            updateCMenuPianoFixed();
        }

        public void cMenuPianoFixed16_Click(Object sender, EventArgs e)
        {
            mPencilMode.setMode(PencilModeEnum.L16);
            updateCMenuPianoFixed();
        }

        public void cMenuPianoFixed32_Click(Object sender, EventArgs e)
        {
            mPencilMode.setMode(PencilModeEnum.L32);
            updateCMenuPianoFixed();
        }

        public void cMenuPianoFixed64_Click(Object sender, EventArgs e)
        {
            mPencilMode.setMode(PencilModeEnum.L64);
            updateCMenuPianoFixed();
        }

        public void cMenuPianoFixed128_Click(Object sender, EventArgs e)
        {
            mPencilMode.setMode(PencilModeEnum.L128);
            updateCMenuPianoFixed();
        }

        public void cMenuPianoFixedOff_Click(Object sender, EventArgs e)
        {
            mPencilMode.setMode(PencilModeEnum.Off);
            updateCMenuPianoFixed();
        }

        public void cMenuPianoFixedTriplet_Click(Object sender, EventArgs e)
        {
            mPencilMode.setTriplet(!mPencilMode.isTriplet());
            updateCMenuPianoFixed();
        }

        public void cMenuPianoFixedDotted_Click(Object sender, EventArgs e)
        {
            mPencilMode.setDot(!mPencilMode.isDot());
            updateCMenuPianoFixed();
        }

        public void cMenuPianoCurve_Click(Object sender, EventArgs e)
        {
            AppManager.setCurveMode(!AppManager.isCurveMode());
            applySelectedTool();
        }
        #endregion

        //BOOKMARK: menuTrack
        #region menuTrack*
        public void menuTrack_DropDownOpening(Object sender, EventArgs e)
        {
#if DEBUG
            sout.println("FormMain#menuTrack_DropDownOpening");
#endif
            updateTrackMenuStatus();
        }

        public void menuTrackCopy_Click(Object sender, EventArgs e)
        {
            copyTrackCore();
        }

        public void menuTrackChangeName_Click(Object sender, EventArgs e)
        {
            changeTrackNameCore();
        }

        public void menuTrackDelete_Click(Object sender, EventArgs e)
        {
            deleteTrackCore();
        }

        public void menuTrackAdd_Click(Object sender, EventArgs e)
        {
            addTrackCore();
        }

        public void menuTrackOverlay_Click(Object sender, EventArgs e)
        {
            AppManager.setOverlay(!AppManager.isOverlay());
            refreshScreen();
        }

        public void menuTrackRenderCurrent_Click(Object sender, EventArgs e)
        {
            List<int> tracks = new List<int>();
            tracks.Add(AppManager.getSelected());
            AppManager.patchWorkToFreeze(this, tracks);
        }

        public void menuTrackRenderer_DropDownOpening(Object sender, EventArgs e)
        {
            updateRendererMenu();
        }
        #endregion

        //BOOKMARK: menuHidden
        #region menuHidden*
        public void menuHiddenVisualForwardParameter_Click(Object sender, EventArgs e)
        {
            trackSelector.SelectNextCurve();
        }

        public void menuHiddenVisualBackwardParameter_Click(Object sender, EventArgs e)
        {
            trackSelector.SelectPreviousCurve();
        }

        public void menuHiddenTrackNext_Click(Object sender, EventArgs e)
        {
            if (AppManager.getSelected() == AppManager.getVsqFile().Track.Count - 1) {
                AppManager.setSelected(1);
            } else {
                AppManager.setSelected(AppManager.getSelected() + 1);
            }
            refreshScreen();
        }

        public void menuHiddenShorten_Click(Object sender, EventArgs e)
        {
            QuantizeMode qmode = AppManager.editorConfig.getLengthQuantize();
            bool triplet = AppManager.editorConfig.isLengthQuantizeTriplet();
            int delta = -QuantizeModeUtil.getQuantizeClock(qmode, triplet);
            lengthenSelectedEvent(delta);
        }

        public void menuHiddenTrackBack_Click(Object sender, EventArgs e)
        {
            if (AppManager.getSelected() == 1) {
                AppManager.setSelected(AppManager.getVsqFile().Track.Count - 1);
            } else {
                AppManager.setSelected(AppManager.getSelected() - 1);
            }
            refreshScreen();
        }

        public void menuHiddenEditPaste_Click(Object sender, EventArgs e)
        {
            pasteEvent();
        }

        public void menuHiddenFlipCurveOnPianorollMode_Click(Object sender, EventArgs e)
        {
            AppManager.mCurveOnPianoroll = !AppManager.mCurveOnPianoroll;
            refreshScreen();
        }

        public void menuHiddenGoToEndMarker_Click(Object sender, EventArgs e)
        {
            if (AppManager.isPlaying()) {
                return;
            }

            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq.config.EndMarkerEnabled) {
                AppManager.setCurrentClock(vsq.config.EndMarker);
                ensureCursorVisible();
                refreshScreen();
            }
        }

        public void menuHiddenGoToStartMarker_Click(Object sender, EventArgs e)
        {
            if (AppManager.isPlaying()) {
                return;
            }

            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq.config.StartMarkerEnabled) {
                AppManager.setCurrentClock(vsq.config.StartMarker);
                ensureCursorVisible();
                refreshScreen();
            }
        }

        public void menuHiddenLengthen_Click(Object sender, EventArgs e)
        {
            QuantizeMode qmode = AppManager.editorConfig.getLengthQuantize();
            bool triplet = AppManager.editorConfig.isLengthQuantizeTriplet();
            int delta = QuantizeModeUtil.getQuantizeClock(qmode, triplet);
            lengthenSelectedEvent(delta);
        }

        public void menuHiddenMoveDown_Click(Object sender, EventArgs e)
        {
            moveUpDownLeftRight(-1, 0);
        }

        public void menuHiddenMoveUp_Click(Object sender, EventArgs e)
        {
            moveUpDownLeftRight(1, 0);
        }

        public void menuHiddenPlayFromStartMarker_Click(Object sender, EventArgs e)
        {
            if (AppManager.isPlaying()) {
                return;
            }
            VsqFileEx vsq = AppManager.getVsqFile();
            if (!vsq.config.StartMarkerEnabled) {
                return;
            }

            AppManager.setCurrentClock(vsq.config.StartMarker);
            AppManager.setPlaying(true, this);
        }

        void menuHiddenPrintPoToCSV_Click(Object sender, EventArgs e)
        {
#if DEBUG
            sout.println("FormMain#menuHiddenPrintPoToCSV_Click");
#endif

            List<string> keys = new List<string>();
            string[] langs = Messaging.getRegisteredLanguage();
            foreach (string lang in langs) {
                foreach (string key in Messaging.getKeys(lang)) {
                    if (!keys.Contains(key)) {
                        keys.Add(key);
                    }
                }
            }

            keys.Sort();
            string dir = PortUtil.getApplicationStartupPath();
            string fname = Path.Combine(dir, "cadencii_trans.csv");
#if DEBUG
            sout.println("FormMain#menuHiddenPrintPoToCSV_Click; fname=" + fname);
#endif
            string old_lang = Messaging.getLanguage();
            StreamWriter br = null;
            try {
                br = new StreamWriter(fname, false, new UTF8Encoding(false));
                string line = "\"en\"";
                foreach (string lang in langs) {
                    line += ",\"" + lang + "\"";
                }
                br.WriteLine(line);
                foreach (string key in keys) {
                    line = "\"" + key + "\"";
                    foreach (string lang in langs) {
                        Messaging.setLanguage(lang);
                        line += ",\"" + Messaging.getMessage(key) + "\"";
                    }
                    br.WriteLine(line);
                }
            } catch (Exception ex) {
                serr.println("FormMain#menuHiddenPrintPoToCSV_Click; ex=" + ex);
            } finally {
                if (br != null) {
                    try {
                        br.Close();
                    } catch (Exception ex2) {
                    }
                }
            }
            Messaging.setLanguage(old_lang);
        }

        public void menuHiddenMoveLeft_Click(Object sender, EventArgs e)
        {
            QuantizeMode mode = AppManager.editorConfig.getPositionQuantize();
            bool triplet = AppManager.editorConfig.isPositionQuantizeTriplet();
            int delta = -QuantizeModeUtil.getQuantizeClock(mode, triplet);
#if DEBUG
            sout.println("FormMain#menuHiddenMoveLeft_Click; delta=" + delta);
#endif
            moveUpDownLeftRight(0, delta);
        }

        public void menuHiddenMoveRight_Click(Object sender, EventArgs e)
        {
            QuantizeMode mode = AppManager.editorConfig.getPositionQuantize();
            bool triplet = AppManager.editorConfig.isPositionQuantizeTriplet();
            int delta = QuantizeModeUtil.getQuantizeClock(mode, triplet);
            moveUpDownLeftRight(0, delta);
        }

        public void menuHiddenSelectBackward_Click(Object sender, EventArgs e)
        {
            selectBackward();
        }

        public void menuHiddenSelectForward_Click(Object sender, EventArgs e)
        {
            selectForward();
        }

        public void menuHiddenEditFlipToolPointerPencil_Click(Object sender, EventArgs e)
        {
            if (AppManager.getSelectedTool() == EditTool.ARROW) {
                AppManager.setSelectedTool(EditTool.PENCIL);
            } else {
                AppManager.setSelectedTool(EditTool.ARROW);
            }
            refreshScreen();
        }

        public void menuHiddenEditFlipToolPointerEraser_Click(Object sender, EventArgs e)
        {
            if (AppManager.getSelectedTool() == EditTool.ARROW) {
                AppManager.setSelectedTool(EditTool.ERASER);
            } else {
                AppManager.setSelectedTool(EditTool.ARROW);
            }
            refreshScreen();
        }

        public void menuHiddenEditLyric_Click(Object sender, EventArgs e)
        {
            bool input_enabled = AppManager.mInputTextBox.Enabled;
            if (!input_enabled && AppManager.itemSelection.getEventCount() > 0) {
                VsqEvent original = AppManager.itemSelection.getLastEvent().original;
                int clock = original.Clock;
                int note = original.ID.Note;
                Point pos = new Point(AppManager.xCoordFromClocks(clock), AppManager.yCoordFromNote(note));
                if (!AppManager.editorConfig.KeepLyricInputMode) {
                    mLastSymbolEditMode = false;
                }
                showInputTextBox(original.ID.LyricHandle.L0.Phrase,
                                  original.ID.LyricHandle.L0.getPhoneticSymbol(),
                                  pos, mLastSymbolEditMode);
                refreshScreen();
            } else if (input_enabled) {
                if (AppManager.mInputTextBox.isPhoneticSymbolEditMode()) {
                    flipInputTextBoxMode();
                }
            }
        }
        #endregion

        //BOOKMARK: cMenuTrackTab
        #region cMenuTrackTab
        public void cMenuTrackTabCopy_Click(Object sender, EventArgs e)
        {
            copyTrackCore();
        }

        public void cMenuTrackTabChangeName_Click(Object sender, EventArgs e)
        {
            changeTrackNameCore();
        }

        public void cMenuTrackTabDelete_Click(Object sender, EventArgs e)
        {
            deleteTrackCore();
        }

        public void cMenuTrackTabAdd_Click(Object sender, EventArgs e)
        {
            addTrackCore();
        }

        public void cMenuTrackTab_Opening(Object sender, CancelEventArgs e)
        {
#if DEBUG
            sout.println("FormMain#cMenuTrackTab_Opening");
#endif
            updateTrackMenuStatus();
        }

        public void updateTrackMenuStatus()
        {
            VsqFileEx vsq = AppManager.getVsqFile();
            int selected = AppManager.getSelected();
            VsqTrack vsq_track = vsq.Track[selected];
            int tracks = vsq.Track.Count;
            cMenuTrackTabDelete.Enabled = tracks >= 3;
            menuTrackDelete.Enabled = tracks >= 3;
            cMenuTrackTabAdd.Enabled = tracks <= AppManager.MAX_NUM_TRACK;
            menuTrackAdd.Enabled = tracks <= AppManager.MAX_NUM_TRACK;
            cMenuTrackTabCopy.Enabled = tracks <= AppManager.MAX_NUM_TRACK;
            menuTrackCopy.Enabled = tracks <= AppManager.MAX_NUM_TRACK;

            bool on = vsq_track.isTrackOn();
            cMenuTrackTabTrackOn.Checked = on;
            menuTrackOn.Checked = on;

            if (tracks > 2) {
                cMenuTrackTabOverlay.Enabled = true;
                menuTrackOverlay.Enabled = true;
                cMenuTrackTabOverlay.Checked = AppManager.isOverlay();
                menuTrackOverlay.Checked = AppManager.isOverlay();
            } else {
                cMenuTrackTabOverlay.Enabled = false;
                menuTrackOverlay.Enabled = false;
                cMenuTrackTabOverlay.Checked = false;
                menuTrackOverlay.Checked = false;
            }
            cMenuTrackTabRenderCurrent.Enabled = !AppManager.isPlaying();
            menuTrackRenderCurrent.Enabled = !AppManager.isPlaying();
            cMenuTrackTabRenderAll.Enabled = !AppManager.isPlaying();
            menuTrackRenderAll.Enabled = !AppManager.isPlaying();

            var kind = VsqFileEx.getTrackRendererKind(vsq_track);
            renderer_menu_handler_.ForEach((handler) => handler.updateCheckedState(kind));
        }

        public void cMenuTrackTabOverlay_Click(Object sender, EventArgs e)
        {
            AppManager.setOverlay(!AppManager.isOverlay());
            refreshScreen();
        }

        public void cMenuTrackTabRenderCurrent_Click(Object sender, EventArgs e)
        {
            List<int> tracks = new List<int>();
            tracks.Add(AppManager.getSelected());
            AppManager.patchWorkToFreeze(this, tracks);
        }

        public void cMenuTrackTabRenderer_DropDownOpening(Object sender, EventArgs e)
        {
            updateRendererMenu();
        }
        #endregion

        #region cPotisionIndicator
        public void cMenuPositionIndicatorStartMarker_Click(Object sender, EventArgs e)
        {
            int clock = mPositionIndicatorPopupShownClock;
            VsqFileEx vsq = AppManager.getVsqFile();
            vsq.config.StartMarkerEnabled = true;
            vsq.config.StartMarker = clock;
            if (vsq.config.EndMarker < clock) {
                vsq.config.EndMarker = clock;
            }
            menuVisualStartMarker.Checked = true;
            setEdited(true);
            picturePositionIndicator.Refresh();
        }

        public void cMenuPositionIndicatorEndMarker_Click(Object sender, EventArgs e)
        {
            int clock = mPositionIndicatorPopupShownClock;
            VsqFileEx vsq = AppManager.getVsqFile();
            vsq.config.EndMarkerEnabled = true;
            vsq.config.EndMarker = clock;
            if (vsq.config.StartMarker > clock) {
                vsq.config.StartMarker = clock;
            }
            menuVisualEndMarker.Checked = true;
            setEdited(true);
            picturePositionIndicator.Refresh();
        }
        #endregion

        //BOOKMARK: cMenuTrackSelector
        #region cMenuTrackSelector
        public void cMenuTrackSelector_Opening(Object sender, CancelEventArgs e)
        {
            updateCopyAndPasteButtonStatus();

            // 選択ツールの状態に合わせて表示を更新
            cMenuTrackSelectorPointer.Checked = false;
            cMenuTrackSelectorPencil.Checked = false;
            cMenuTrackSelectorLine.Checked = false;
            cMenuTrackSelectorEraser.Checked = false;
            EditTool tool = AppManager.getSelectedTool();
            if (tool == EditTool.ARROW) {
                cMenuTrackSelectorPointer.Checked = true;
            } else if (tool == EditTool.PENCIL) {
                cMenuTrackSelectorPencil.Checked = true;
            } else if (tool == EditTool.LINE) {
                cMenuTrackSelectorLine.Checked = true;
            } else if (tool == EditTool.ERASER) {
                cMenuTrackSelectorEraser.Checked = true;
            }
            cMenuTrackSelectorCurve.Checked = AppManager.isCurveMode();
        }

        public void cMenuTrackSelectorPointer_Click(Object sender, EventArgs e)
        {
            AppManager.setSelectedTool(EditTool.ARROW);
            refreshScreen();
        }

        public void cMenuTrackSelectorPencil_Click(Object sender, EventArgs e)
        {
            AppManager.setSelectedTool(EditTool.PENCIL);
            refreshScreen();
        }

        public void cMenuTrackSelectorLine_Click(Object sender, EventArgs e)
        {
            AppManager.setSelectedTool(EditTool.LINE);
        }

        public void cMenuTrackSelectorEraser_Click(Object sender, EventArgs e)
        {
            AppManager.setSelectedTool(EditTool.ERASER);
        }

        public void cMenuTrackSelectorCurve_Click(Object sender, EventArgs e)
        {
            AppManager.setCurveMode(!AppManager.isCurveMode());
        }

        public void cMenuTrackSelectorSelectAll_Click(Object sender, EventArgs e)
        {
            selectAllEvent();
        }

        public void cMenuTrackSelectorCut_Click(Object sender, EventArgs e)
        {
            cutEvent();
        }

        public void cMenuTrackSelectorCopy_Click(Object sender, EventArgs e)
        {
            copyEvent();
        }

        public void cMenuTrackSelectorDelete_Click(Object sender, EventArgs e)
        {
            deleteEvent();
        }

        public void cMenuTrackSelectorDeleteBezier_Click(Object sender, EventArgs e)
        {
            foreach (var sbp in AppManager.itemSelection.getBezierIterator()) {
                int chain_id = sbp.chainID;
                int point_id = sbp.pointID;
                VsqFileEx vsq = AppManager.getVsqFile();
                int selected = AppManager.getSelected();
                BezierChain chain = (BezierChain)vsq.AttachedCurves.get(selected - 1).getBezierChain(trackSelector.getSelectedCurve(), chain_id).clone();
                int index = -1;
                for (int i = 0; i < chain.points.Count; i++) {
                    if (chain.points[i].getID() == point_id) {
                        index = i;
                        break;
                    }
                }
                if (index >= 0) {
                    chain.points.RemoveAt(index);
                    if (chain.points.Count == 0) {
                        CadenciiCommand run = VsqFileEx.generateCommandDeleteBezierChain(selected,
                                                                                          trackSelector.getSelectedCurve(),
                                                                                          chain_id,
                                                                                          AppManager.editorConfig.getControlCurveResolutionValue());
                        AppManager.editHistory.register(vsq.executeCommand(run));
                    } else {
                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain(selected,
                                                                                           trackSelector.getSelectedCurve(),
                                                                                           chain_id,
                                                                                           chain,
                                                                                           AppManager.editorConfig.getControlCurveResolutionValue());
                        AppManager.editHistory.register(vsq.executeCommand(run));
                    }
                    setEdited(true);
                    refreshScreen();
                    break;
                }
            }
        }

        public void cMenuTrackSelectorPaste_Click(Object sender, EventArgs e)
        {
            pasteEvent();
        }

        public void cMenuTrackSelectorUndo_Click(Object sender, EventArgs e)
        {
#if DEBUG
            AppManager.debugWriteLine("cMenuTrackSelectorUndo_Click");
#endif
            undo();
            refreshScreen();
        }

        public void cMenuTrackSelectorRedo_Click(Object sender, EventArgs e)
        {
#if DEBUG
            AppManager.debugWriteLine("cMenuTrackSelectorRedo_Click");
#endif
            redo();
            refreshScreen();
        }
        #endregion

        #region buttonVZoom & buttonVMooz
        public void buttonVZoom_Click(Object sender, EventArgs e)
        {
            zoomY(1);
        }

        public void buttonVMooz_Click(Object sender, EventArgs e)
        {
            zoomY(-1);
        }
        #endregion

        #region pictureBox2
        public void pictureBox2_Paint(Object sender, PaintEventArgs e)
        {
            if (mGraphicsPictureBox2 == null) {
                mGraphicsPictureBox2 = new Graphics2D(null);
            }
            mGraphicsPictureBox2.nativeGraphics = e.Graphics;
            int width = pictureBox2.Width;
            int height = pictureBox2.Height;
            int unit_height = height / 4;
            mGraphicsPictureBox2.setColor(mColorR214G214B214);
            mGraphicsPictureBox2.fillRect(0, 0, width, height);
            if (mPianoRollScaleYMouseStatus > 0) {
                mGraphicsPictureBox2.setColor(Color.gray);
                mGraphicsPictureBox2.fillRect(0, 0, width, unit_height);
            } else if (mPianoRollScaleYMouseStatus < 0) {
                mGraphicsPictureBox2.setColor(Color.gray);
                mGraphicsPictureBox2.fillRect(0, unit_height * 2, width, unit_height);
            }
            mGraphicsPictureBox2.setStroke(getStrokeDefault());
            mGraphicsPictureBox2.setColor(Color.gray);
            //mGraphicsPictureBox2.drawRect( 0, 0, width - 1, unit_height * 2 );
            mGraphicsPictureBox2.drawLine(0, unit_height, width, unit_height);
            mGraphicsPictureBox2.drawLine(0, unit_height * 2, width, unit_height * 2);
            mGraphicsPictureBox2.setStroke(getStroke2px());
            int cx = width / 2;
            int cy = unit_height / 2;
            mGraphicsPictureBox2.setColor((mPianoRollScaleYMouseStatus > 0) ? Color.lightGray : Color.gray);
            mGraphicsPictureBox2.drawLine(cx - 4, cy, cx + 4, cy);
            mGraphicsPictureBox2.drawLine(cx, cy - 4, cx, cy + 4);
            cy += unit_height * 2;
            mGraphicsPictureBox2.setColor((mPianoRollScaleYMouseStatus < 0) ? Color.lightGray : Color.gray);
            mGraphicsPictureBox2.drawLine(cx - 4, cy, cx + 4, cy);
        }

        public void pictureBox2_MouseDown(Object sender, MouseEventArgs e)
        {
            // 拡大・縮小ボタンが押されたかどうか判定
            int height = pictureBox2.Height;
            int width = pictureBox2.Width;
            int height4 = height / 4;
            if (0 <= e.X && e.X < width) {
                int scaley = AppManager.editorConfig.PianoRollScaleY;
                if (0 <= e.Y && e.Y < height4) {
                    if (scaley + 1 <= EditorConfig.MAX_PIANOROLL_SCALEY) {
                        zoomY(1);
                        mPianoRollScaleYMouseStatus = 1;
                    } else {
                        mPianoRollScaleYMouseStatus = 0;
                    }
                } else if (height4 * 2 <= e.Y && e.Y < height4 * 3) {
                    if (scaley - 1 >= EditorConfig.MIN_PIANOROLL_SCALEY) {
                        zoomY(-1);
                        mPianoRollScaleYMouseStatus = -1;
                    } else {
                        mPianoRollScaleYMouseStatus = 0;
                    }
                } else {
                    mPianoRollScaleYMouseStatus = 0;
                }
            } else {
                mPianoRollScaleYMouseStatus = 0;
            }
            refreshScreen();
        }

        public void pictureBox2_MouseUp(Object sender, MouseEventArgs e)
        {
            mPianoRollScaleYMouseStatus = 0;
            pictureBox2.Invalidate();
        }
        #endregion

        //BOOKMARK: stripBtn
        #region stripBtn*
        public void stripBtnGrid_Click(Object sender, EventArgs e)
        {
            bool new_v = !AppManager.isGridVisible();
            stripBtnGrid.Pushed = new_v;
            AppManager.setGridVisible(new_v);
        }

        public void stripBtnArrow_Click(Object sender, EventArgs e)
        {
            AppManager.setSelectedTool(EditTool.ARROW);
        }

        public void stripBtnPencil_Click(Object sender, EventArgs e)
        {
            AppManager.setSelectedTool(EditTool.PENCIL);
        }

        public void stripBtnLine_Click(Object sender, EventArgs e)
        {
            AppManager.setSelectedTool(EditTool.LINE);
        }

        public void stripBtnEraser_Click(Object sender, EventArgs e)
        {
            AppManager.setSelectedTool(EditTool.ERASER);
        }

        public void stripBtnCurve_Click(Object sender, EventArgs e)
        {
            AppManager.setCurveMode(!AppManager.isCurveMode());
        }

        public void stripBtnPlay_Click(Object sender, EventArgs e)
        {
            AppManager.setPlaying(!AppManager.isPlaying(), this);
            focusPianoRoll();
        }

        public void stripBtnScroll_CheckedChanged(Object sender, EventArgs e)
        {
            bool pushed = stripBtnScroll.Pushed;
            AppManager.mAutoScroll = pushed;
#if DEBUG
            sout.println("FormMain#stripBtnScroll_CheckedChanged; pushed=" + pushed);
#endif
            focusPianoRoll();
        }

        public void stripBtnLoop_CheckedChanged(Object sender, EventArgs e)
        {
            bool pushed = stripBtnLoop.Pushed;
            AppManager.setRepeatMode(pushed);
            focusPianoRoll();
        }

        public void stripBtnStepSequencer_CheckedChanged(Object sender, EventArgs e)
        {
            // AppManager.mAddingEventがnullかどうかで処理が変わるのでnullにする
            AppManager.mAddingEvent = null;
            // モードを切り替える
            controller.setStepSequencerEnabled(stripBtnStepSequencer.Checked);

            // MIDIの受信を開始
#if ENABLE_MIDI
            if (controller.isStepSequencerEnabled()) {
                mMidiIn.start();
            } else {
                mMidiIn.stop();
            }
#endif
        }

        public void stripBtnStop_Click(Object sender, EventArgs e)
        {
            AppManager.setPlaying(false, this);
            timer.Stop();
            focusPianoRoll();
        }

        public void stripBtnMoveEnd_Click(Object sender, EventArgs e)
        {
            if (AppManager.isPlaying()) {
                AppManager.setPlaying(false, this);
            }
            AppManager.setCurrentClock(AppManager.getVsqFile().TotalClocks);
            ensureCursorVisible();
            refreshScreen();
        }

        public void stripBtnMoveTop_Click(Object sender, EventArgs e)
        {
            if (AppManager.isPlaying()) {
                AppManager.setPlaying(false, this);
            }
            AppManager.setCurrentClock(0);
            ensureCursorVisible();
            refreshScreen();
        }

        public void stripBtnRewind_Click(Object sender, EventArgs e)
        {
            rewind();
        }

        public void stripBtnForward_Click(Object sender, EventArgs e)
        {
            forward();
        }
        #endregion

        //BOOKMARK: pictKeyLengthSplitter
        #region pictKeyLengthSplitter
        public void pictKeyLengthSplitter_MouseDown(Object sender, MouseEventArgs e)
        {
            mKeyLengthSplitterMouseDowned = true;
            mKeyLengthSplitterInitialMouse = PortUtil.getMousePosition();
            mKeyLengthInitValue = AppManager.keyWidth;
            mKeyLengthTrackSelectorRowsPerColumn = trackSelector.getRowsPerColumn();
            mKeyLengthSplitterDistance = splitContainer1.getDividerLocation();
        }

        public void pictKeyLengthSplitter_MouseMove(Object sender, MouseEventArgs e)
        {
            if (!mKeyLengthSplitterMouseDowned) {
                return;
            }
            int dx = PortUtil.getMousePosition().x - mKeyLengthSplitterInitialMouse.x;
            int draft = mKeyLengthInitValue + dx;
            if (draft < AppManager.MIN_KEY_WIDTH) {
                draft = AppManager.MIN_KEY_WIDTH;
            } else if (AppManager.MAX_KEY_WIDTH < draft) {
                draft = AppManager.MAX_KEY_WIDTH;
            }
            AppManager.keyWidth = draft;
            int current = trackSelector.getRowsPerColumn();
            if (current >= mKeyLengthTrackSelectorRowsPerColumn) {
                int max_divider_location = splitContainer1.getHeight() - splitContainer1.getDividerSize() - splitContainer1.getPanel2MinSize();
                if (max_divider_location < mKeyLengthSplitterDistance) {
                    splitContainer1.setDividerLocation(max_divider_location);
                } else {
                    splitContainer1.setDividerLocation(mKeyLengthSplitterDistance);
                }
            }
            updateLayout();
            refreshScreen();
        }

        public void pictKeyLengthSplitter_MouseUp(Object sender, MouseEventArgs e)
        {
            mKeyLengthSplitterMouseDowned = false;
        }
        #endregion

        #region toolBarMeasure
        void toolBarMeasure_MouseDown(Object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // マウス位置にあるボタンを捜す
            System.Windows.Forms.ToolBarButton c = null;
            foreach (System.Windows.Forms.ToolBarButton btn in toolBarMeasure.Buttons) {
                System.Drawing.Rectangle rc = btn.Rectangle;
                if (Utility.isInRect(e.X, e.Y, rc.Left, rc.Top, rc.Width, rc.Height)) {
                    c = btn;
                    break;
                }
            }
            if (c == null) {
                return;
            }

            if (c == stripDDBtnQuantizeParent) {
                System.Drawing.Rectangle rc = stripDDBtnQuantizeParent.Rectangle;
                stripDDBtnQuantize.Show(
                    toolBarMeasure,
                    new System.Drawing.Point(rc.Left, rc.Bottom));
            }
        }

        void toolBarMeasure_ButtonClick(Object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            if (e.Button == stripBtnStartMarker) {
                handleStartMarker_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnEndMarker) {
                handleEndMarker_Click(e.Button, new EventArgs());
            }/* else if ( e.Button == stripDDBtnLengthParent ) {
                System.Drawing.Rectangle rc = stripDDBtnLengthParent.Rectangle;
                stripDDBtnLength.Show(
                    toolBarMeasure,
                    new System.Drawing.Point( rc.Left, rc.Bottom ) );
            } else if ( e.Button == stripDDBtnQuantizeParent ) {
                System.Drawing.Rectangle rc = stripDDBtnQuantizeParent.Rectangle;
                stripDDBtnQuantize.Show(
                    toolBarMeasure,
                    new System.Drawing.Point( rc.Left, rc.Bottom ) );
            }*/
        }
        #endregion

        void toolBarTool_ButtonClick(Object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            if (e.Button == stripBtnPointer) {
                stripBtnArrow_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnPencil) {
                stripBtnPencil_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnLine) {
                stripBtnLine_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnEraser) {
                stripBtnEraser_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnGrid) {
                stripBtnGrid_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnCurve) {
                stripBtnCurve_Click(e.Button, new EventArgs());
            } else {
                handleStripPaletteTool_Click(e.Button, new EventArgs());
            }
        }

        void toolBarPosition_ButtonClick(Object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            if (e.Button == stripBtnMoveTop) {
                stripBtnMoveTop_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnRewind) {
                stripBtnRewind_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnForward) {
                stripBtnForward_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnMoveEnd) {
                stripBtnMoveEnd_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnPlay) {
                stripBtnPlay_Click(e.Button, new EventArgs());
                //} else if ( e.Button == stripBtnStop ) {
                //stripBtnStop_Click( e.Button, new EventArgs() );
            } else if (e.Button == stripBtnScroll) {
                stripBtnScroll.Pushed = !stripBtnScroll.Pushed;
                stripBtnScroll_CheckedChanged(e.Button, new EventArgs());
            } else if (e.Button == stripBtnLoop) {
                stripBtnLoop.Pushed = !stripBtnLoop.Pushed;
                stripBtnLoop_CheckedChanged(e.Button, new EventArgs());
            }
        }

        void toolBarFile_ButtonClick(Object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            if (e.Button == stripBtnFileNew) {
                handleFileNew_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnFileOpen) {
                handleFileOpen_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnFileSave) {
                handleFileSave_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnCut) {
                handleEditCut_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnCopy) {
                handleEditCopy_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnPaste) {
                handleEditPaste_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnUndo) {
                handleEditUndo_Click(e.Button, new EventArgs());
            } else if (e.Button == stripBtnRedo) {
                handleEditRedo_Click(e.Button, new EventArgs());
            }
        }

        public void handleVibratoPresetSubelementClick(Object sender, EventArgs e)
        {
            if (sender == null) {
                return;
            }
            if (!(sender is System.Windows.Forms.ToolStripMenuItem)) {
                return;
            }

            // イベントの送信元を特定
            System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
            string text = item.Text;

            // メニューの表示文字列から，どの設定値についてのイベントかを探す
            VibratoHandle target = null;
            int size = AppManager.editorConfig.AutoVibratoCustom.Count;
            for (int i = 0; i < size; i++) {
                VibratoHandle handle = AppManager.editorConfig.AutoVibratoCustom[i];
                if (text.Equals(handle.getCaption())) {
                    target = handle;
                    break;
                }
            }

            // ターゲットが特定できなかったらbailout
            if (target == null) {
                return;
            }

            // 選択状態のアイテムを取得
            IEnumerable<SelectedEventEntry> itr = AppManager.itemSelection.getEventIterator();
            if (itr.Count() == 0) {
                // アイテムがないのでbailout
                return;
            }
            VsqEvent ev = itr.First().original;
            if (ev.ID.VibratoHandle == null) {
                return;
            }

            // 設定値にコピー
            VibratoHandle h = ev.ID.VibratoHandle;
            target.setStartRate(h.getStartRate());
            target.setStartDepth(h.getStartDepth());
            if (h.getRateBP() == null) {
                target.setRateBP(null);
            } else {
                target.setRateBP((VibratoBPList)h.getRateBP().clone());
            }
            if (h.getDepthBP() == null) {
                target.setDepthBP(null);
            } else {
                target.setDepthBP((VibratoBPList)h.getDepthBP().clone());
            }
        }

        public void timer_Tick(Object sender, EventArgs e)
        {
            if (AppManager.isGeneratorRunning()) {
                MonitorWaveReceiver monitor = MonitorWaveReceiver.getInstance();
                double play_time = 0.0;
                if (monitor != null) {
                    play_time = monitor.getPlayTime();
                }
                double now = play_time + AppManager.mDirectPlayShift;
                int clock = (int)AppManager.getVsqFile().getClockFromSec(now);
                if (mLastClock <= clock) {
                    mLastClock = clock;
                    AppManager.setCurrentClock(clock);
                    if (AppManager.mAutoScroll) {
                        ensureCursorVisible();
                    }
                }
            } else {
                AppManager.setPlaying(false, this);
                int ending_clock = AppManager.getPreviewEndingClock();
                AppManager.setCurrentClock(ending_clock);
                if (AppManager.mAutoScroll) {
                    ensureCursorVisible();
                }
                refreshScreen(true);
                if (AppManager.isRepeatMode()) {
                    int dest_clock = 0;
                    VsqFileEx vsq = AppManager.getVsqFile();
                    if (vsq.config.StartMarkerEnabled) {
                        dest_clock = vsq.config.StartMarker;
                    }
                    AppManager.setCurrentClock(dest_clock);
                    AppManager.setPlaying(true, this);
                }
            }
            refreshScreen();
        }

        public void bgWorkScreen_DoWork(Object sender, DoWorkEventArgs e)
        {
            try {
                this.Invoke(new EventHandler(this.refreshScreenCore));
            } catch (Exception ex) {
                serr.println("FormMain#bgWorkScreen_DoWork; ex=" + ex);
                Logger.write(typeof(FormMain) + ".bgWorkScreen_DoWork; ex=" + ex + "\n");
            }
        }

        public void toolStripEdit_Resize(Object sender, EventArgs e)
        {
            saveToolbarLocation();
        }

        public void toolStripPosition_Resize(Object sender, EventArgs e)
        {
            saveToolbarLocation();
        }

        public void toolStripMeasure_Resize(Object sender, EventArgs e)
        {
            saveToolbarLocation();
        }

        void toolStripFile_Resize(Object sender, EventArgs e)
        {
            saveToolbarLocation();
        }

        public void toolStripContainer_TopToolStripPanel_SizeChanged(Object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) {
                return;
            }
            Dimension minsize = getWindowMinimumSize();
            int wid = this.Width;
            int hei = this.Height;
            bool change_size_required = false;
            if (minsize.width > wid) {
                wid = minsize.width;
                change_size_required = true;
            }
            if (minsize.height > hei) {
                hei = minsize.height;
                change_size_required = true;
            }
            var min_size = getWindowMinimumSize();
            this.MinimumSize = new System.Drawing.Size(min_size.width, min_size.height);
            if (change_size_required) {
                this.Size = new System.Drawing.Size(wid, hei);
            }
        }

        public void handleRecentFileMenuItem_Click(Object sender, EventArgs e)
        {
            if (sender is RecentFileMenuItem) {
                RecentFileMenuItem item = (RecentFileMenuItem)sender;
                string filename = item.getFilePath();
                if (!dirtyCheck()) {
                    return;
                }
                openVsqCor(filename);
                clearExistingData();
                AppManager.mMixerWindow.updateStatus();
                clearTempWave();
                updateDrawObjectList();
                refreshScreen();
            }
        }

        public void handleRecentFileMenuItem_MouseEnter(Object sender, EventArgs e)
        {
            if (sender is RecentFileMenuItem) {
                RecentFileMenuItem item = (RecentFileMenuItem)sender;
                statusLabel.Text = item.ToolTipText;
            }
        }

        public void handleStripPaletteTool_Click(Object sender, EventArgs e)
        {
            string id = "";  //選択されたツールのID
#if ENABLE_SCRIPT
            if (sender is System.Windows.Forms.ToolBarButton) {
                System.Windows.Forms.ToolBarButton tsb = (System.Windows.Forms.ToolBarButton)sender;
                if (tsb.Tag != null && tsb.Tag is string) {
                    id = (string)tsb.Tag;
                    AppManager.mSelectedPaletteTool = id;
                    AppManager.setSelectedTool(EditTool.PALETTE_TOOL);
                    tsb.Pushed = true;
                }
            } else if (sender is ToolStripMenuItem) {
                ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
                if (tsmi.Tag != null && tsmi.Tag is string) {
                    id = (string)tsmi.Tag;
                    AppManager.mSelectedPaletteTool = id;
                    AppManager.setSelectedTool(EditTool.PALETTE_TOOL);
                    tsmi.Checked = true;
                }
            }
#endif

            int count = toolBarTool.Buttons.Count;
            for (int i = 0; i < count; i++) {
                Object item = toolBarTool.Buttons[i];
                if (item is System.Windows.Forms.ToolBarButton) {
                    System.Windows.Forms.ToolBarButton button = (System.Windows.Forms.ToolBarButton)item;
                    if (button.Style == System.Windows.Forms.ToolBarButtonStyle.ToggleButton && button.Tag != null && button.Tag is string) {
                        if (((string)button.Tag).Equals(id)) {
                            button.Pushed = true;
                        } else {
                            button.Pushed = false;
                        }
                    }
                }
            }

            foreach (var item in cMenuPianoPaletteTool.DropDownItems) {
                if (item is PaletteToolMenuItem) {
                    PaletteToolMenuItem menu = (PaletteToolMenuItem)item;
                    string tagged_id = menu.getPaletteToolID();
                    menu.Checked = (id == tagged_id);
                }
            }

            //MenuElement[] sub_cmenu_track_selectro_palette_tool = cMenuTrackSelectorPaletteTool.getSubElements();
            //for ( int i = 0; i < sub_cmenu_track_selectro_palette_tool.Length; i++ ) {
            //MenuElement item = sub_cmenu_track_selectro_palette_tool[i];
            foreach (var item in cMenuTrackSelectorPaletteTool.DropDownItems) {
                if (item is PaletteToolMenuItem) {
                    PaletteToolMenuItem menu = (PaletteToolMenuItem)item;
                    string tagged_id = menu.getPaletteToolID();
                    menu.Checked = (id == tagged_id);
                }
            }
        }

        public void handleTrackOn_Click(Object sender, EventArgs e)
        {
            int selected = AppManager.getSelected();
            VsqTrack vsq_track = AppManager.getVsqFile().Track[selected];
            bool old_status = vsq_track.isTrackOn();
            bool new_status = !old_status;
            int last_play_mode = vsq_track.getCommon().LastPlayMode;
            CadenciiCommand run = new CadenciiCommand(
                VsqCommand.generateCommandTrackChangePlayMode(
                    selected,
                    new_status ? last_play_mode : PlayMode.Off,
                    last_play_mode));
            AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
            menuTrackOn.Checked = new_status;
            cMenuTrackTabTrackOn.Checked = new_status;
            setEdited(true);
            refreshScreen();
        }

        public void handleTrackRenderAll_Click(Object sender, EventArgs e)
        {
            List<int> list = new List<int>();
            int c = AppManager.getVsqFile().Track.Count;
            for (int i = 1; i < c; i++) {
                if (AppManager.getRenderRequired(i)) {
                    list.Add(i);
                }
            }
            if (list.Count <= 0) {
                return;
            }
            AppManager.patchWorkToFreeze(this, list);
        }

        public void handleEditorConfig_QuantizeModeChanged(Object sender, EventArgs e)
        {
            applyQuantizeMode();
        }

        public void handleFileSave_Click(Object sender, EventArgs e)
        {
            for (int track = 1; track < AppManager.getVsqFile().Track.Count; track++) {
                if (AppManager.getVsqFile().Track[track].getEventCount() == 0) {
                    AppManager.showMessageBox(
                        PortUtil.formatMessage(
                            _("Invalid note data.\nTrack {0} : {1}\n\n-> Piano roll : Blank sequence."),
                            track,
                            AppManager.getVsqFile().Track[track].getName()),
                        _APP_NAME,
                        cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                        cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE);
                    return;
                }
            }
            string file = AppManager.getFileName();
            if (file.Equals("")) {
                string last_file = AppManager.editorConfig.getLastUsedPathOut("xvsq");
                if (!last_file.Equals("")) {
                    string dir = PortUtil.getDirectoryName(last_file);
                    saveXmlVsqDialog.SetSelectedFile(dir);
                }
                var dr = AppManager.showModalDialog(saveXmlVsqDialog, false, this);
                if (dr == System.Windows.Forms.DialogResult.OK) {
                    file = saveXmlVsqDialog.FileName;
                    AppManager.editorConfig.setLastUsedPathOut(file, ".xvsq");
                }
            }
            if (file != "") {
                AppManager.saveTo(file);
                updateRecentFileMenu();
                setEdited(false);
            }
        }

        public void handleFileOpen_Click(Object sender, EventArgs e)
        {
            if (!dirtyCheck()) {
                return;
            }
            string dir = AppManager.editorConfig.getLastUsedPathIn("xvsq");
            openXmlVsqDialog.SetSelectedFile(dir);
            var dialog_result = AppManager.showModalDialog(openXmlVsqDialog, true, this);
            if (dialog_result != System.Windows.Forms.DialogResult.OK) {
                return;
            }
            if (AppManager.isPlaying()) {
                AppManager.setPlaying(false, this);
            }
            string file = openXmlVsqDialog.FileName;
            AppManager.editorConfig.setLastUsedPathIn(file, ".xvsq");
            if (openVsqCor(file)) {
                AppManager.showMessageBox(
                    _("Invalid XVSQ file"),
                    _("Error"),
                    cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                    cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE);
                return;
            }
            clearExistingData();

            setEdited(false);
            AppManager.mMixerWindow.updateStatus();
            clearTempWave();
            updateDrawObjectList();
            refreshScreen();
        }

        public void handleStripButton_Enter(Object sender, EventArgs e)
        {
            focusPianoRoll();
        }

        public void handleFileNew_Click(Object sender, EventArgs e)
        {
            if (!dirtyCheck()) {
                return;
            }
            AppManager.setSelected(1);
            VsqFileEx vsq = new VsqFileEx(AppManager.editorConfig.DefaultSingerName, 1, 4, 4, 500000);

            RendererKind kind = AppManager.editorConfig.DefaultSynthesizer;
            string renderer = kind.getVersionString();
            List<VsqID> singers = AppManager.getSingerListFromRendererKind(kind);
            vsq.Track[1].changeRenderer(renderer, singers);

            AppManager.setVsqFile(vsq);
            clearExistingData();
            for (int i = 0; i < AppManager.mLastRenderedStatus.Length; i++) {
                AppManager.mLastRenderedStatus[i] = null;
            }
            setEdited(false);
            AppManager.mMixerWindow.updateStatus();
            clearTempWave();

            // キャッシュディレクトリのパスを、デフォルトに戻す
            AppManager.setTempWaveDir(Path.Combine(AppManager.getCadenciiTempDir(), AppManager.getID()));

            updateDrawObjectList();
            refreshScreen();
        }

        public void handleEditPaste_Click(Object sender, EventArgs e)
        {
            pasteEvent();
        }

        public void handleEditCopy_Click(Object sender, EventArgs e)
        {
#if DEBUG
            AppManager.debugWriteLine("handleEditCopy_Click");
#endif
            copyEvent();
        }

        public void handleEditCut_Click(Object sender, EventArgs e)
        {
            cutEvent();
        }

        public void handlePositionQuantize(Object sender, EventArgs e)
        {
            QuantizeMode qm = AppManager.editorConfig.getPositionQuantize();
            if (sender == cMenuPianoQuantize04 ||
#if ENABLE_STRIP_DROPDOWN
 sender == stripDDBtnQuantize04 ||
#endif
 sender == menuSettingPositionQuantize04) {
                qm = QuantizeMode.p4;
            } else if (sender == cMenuPianoQuantize08 ||
#if ENABLE_STRIP_DROPDOWN
 sender == stripDDBtnQuantize08 ||
#endif
 sender == menuSettingPositionQuantize08) {
                qm = QuantizeMode.p8;
            } else if (sender == cMenuPianoQuantize16 ||
#if ENABLE_STRIP_DROPDOWN
 sender == stripDDBtnQuantize16 ||
#endif
 sender == menuSettingPositionQuantize16) {
                qm = QuantizeMode.p16;
            } else if (sender == cMenuPianoQuantize32 ||
#if ENABLE_STRIP_DROPDOWN
 sender == stripDDBtnQuantize32 ||
#endif
 sender == menuSettingPositionQuantize32) {
                qm = QuantizeMode.p32;
            } else if (sender == cMenuPianoQuantize64 ||
#if ENABLE_STRIP_DROPDOWN
 sender == stripDDBtnQuantize64 ||
#endif
 sender == menuSettingPositionQuantize64) {
                qm = QuantizeMode.p64;
            } else if (sender == cMenuPianoQuantize128 ||
#if ENABLE_STRIP_DROPDOWN
 sender == stripDDBtnQuantize128 ||
#endif
 sender == menuSettingPositionQuantize128) {
                qm = QuantizeMode.p128;
            } else if (sender == cMenuPianoQuantizeOff ||
#if ENABLE_STRIP_DROPDOWN
 sender == stripDDBtnQuantizeOff ||
#endif
 sender == menuSettingPositionQuantizeOff) {
                qm = QuantizeMode.off;
            }
            AppManager.editorConfig.setPositionQuantize(qm);
            AppManager.editorConfig.setLengthQuantize(qm);
            refreshScreen();
        }

        public void handlePositionQuantizeTriplet_Click(Object sender, EventArgs e)
        {
            bool triplet = !AppManager.editorConfig.isPositionQuantizeTriplet();
            AppManager.editorConfig.setPositionQuantizeTriplet(triplet);
            AppManager.editorConfig.setLengthQuantizeTriplet(triplet);
            refreshScreen();
        }

        public void handleStartMarker_Click(Object sender, EventArgs e)
        {
            VsqFileEx vsq = AppManager.getVsqFile();
            vsq.config.StartMarkerEnabled = !vsq.config.StartMarkerEnabled;
            menuVisualStartMarker.Checked = vsq.config.StartMarkerEnabled;
            setEdited(true);
            focusPianoRoll();
            refreshScreen();
        }

        public void handleEndMarker_Click(Object sender, EventArgs e)
        {
            VsqFileEx vsq = AppManager.getVsqFile();
            vsq.config.EndMarkerEnabled = !vsq.config.EndMarkerEnabled;
            stripBtnEndMarker.Pushed = vsq.config.EndMarkerEnabled;
            menuVisualEndMarker.Checked = vsq.config.EndMarkerEnabled;
            setEdited(true);
            focusPianoRoll();
            refreshScreen();
        }

        /// <summary>
        /// メニューの説明をステータスバーに表示するための共通のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void handleMenuMouseEnter(Object sender, EventArgs e)
        {
            if (sender == null) {
                return;
            }

            bool notfound = false;
            string text = "";
            if (sender == menuEditUndo) {
                text = _("Undo.");
            } else if (sender == menuEditRedo) {
                text = _("Redo.");
            } else if (sender == menuEditCut) {
                text = _("Cut selected items.");
            } else if (sender == menuEditCopy) {
                text = _("Copy selected items.");
            } else if (sender == menuEditPaste) {
                text = _("Paste copied items to current song position.");
            } else if (sender == menuEditDelete) {
                text = _("Delete selected items.");
            } else if (sender == menuEditAutoNormalizeMode) {
                text = _("Avoid automaticaly polyphonic editing.");
            } else if (sender == menuEditSelectAll) {
                text = _("Select all items and control curves of current track.");
            } else if (sender == menuEditSelectAllEvents) {
                text = _("Select all items of current track.");
            } else if (sender == menuVisualControlTrack) {
                text = _("Show/Hide control curves.");
            } else if (sender == menuVisualEndMarker) {
                text = _("Enable/Disable end marker.");
            } else if (sender == menuVisualGridline) {
                text = _("Show/Hide grid line.");
            } else if (sender == menuVisualIconPalette) {
                text = _("Show/Hide icon palette");
            } else if (sender == menuVisualLyrics) {
                text = _("Show/Hide lyrics.");
            } else if (sender == menuVisualMixer) {
                text = _("Show/Hide mixer window.");
            } else if (sender == menuVisualNoteProperty) {
                text = _("Show/Hide expression lines.");
            } else if (sender == menuVisualOverview) {
                text = _("Show/Hide navigation view");
            } else if (sender == menuVisualPitchLine) {
                text = _("Show/Hide pitch bend lines.");
            } else if (sender == menuVisualPluginUi) {
                text = _("Open VSTi plugin window");
            } else if (sender == menuVisualProperty) {
                text = _("Show/Hide property window.");
            } else if (sender == menuVisualStartMarker) {
                text = _("Enable/Disable start marker.");
            } else if (sender == menuVisualWaveform) {
                text = _("Show/Hide waveform.");
            } else if (sender == menuFileNew) {
                text = _("Create new project.");
            } else if (sender == menuFileOpen) {
                text = _("Open Cadencii project.");
            } else if (sender == menuFileSave) {
                text = _("Save current project.");
            } else if (sender == menuFileSaveNamed) {
                text = _("Save current project with new name.");
            } else if (sender == menuFileOpenVsq) {
                text = _("Open VSQ / VOCALOID MIDI and create new project.");
            } else if (sender == menuFileOpenUst) {
                text = _("Open UTAU project and create new project.");
            } else if (sender == menuFileImport) {
                text = _("Import.");
            } else if (sender == menuFileImportMidi) {
                text = _("Import Standard MIDI.");
            } else if (sender == menuFileImportUst) {
                text = _("Import UTAU project");
            } else if (sender == menuFileImportVsq) {
                text = _("Import VSQ / VOCALOID MIDI.");
            } else if (sender == menuFileExport) {
                text = _("Export.");
            } else if (sender == menuFileExportParaWave) {
                text = _("Export all tracks to serial numbered WAVEs");
            } else if (sender == menuFileExportWave) {
                text = _("Export to WAVE file.");
            } else if (sender == menuFileExportMusicXml) {
                text = _("Export current track as Music XML");
            } else if (sender == menuFileExportMidi) {
                text = _("Export to Standard MIDI.");
            } else if (sender == menuFileExportUst) {
                text = _("Export current track as UTAU project file");
            } else if (sender == menuFileExportVsq) {
                text = _("Export to VSQ");
            } else if (sender == menuFileExportVsqx) {
                text = _("Export to VSQX");
            } else if (sender == menuFileExportVxt) {
                text = _("Export current track as Meta-text for vConnect");
            } else if (sender == menuFileRecent) {
                text = _("Recent projects.");
            } else if (sender == menuFileQuit) {
                text = _("Close this window.");
            } else if (sender == menuJobConnect) {
                text = _("Lengthen note end to neighboring note.");
            } else if (sender == menuJobLyric) {
                text = _("Import lyric.");
            } else if (sender == menuJobRewire) {
                text = _("Import tempo from ReWire host.") + _("(not implemented)");
            } else if (sender == menuJobReloadVsti) {
                text = _("Reload VSTi dll.") + _("(not implemented)");
            } else if (sender == menuJobNormalize) {
                text = _("Correct overlapped item.");
            } else if (sender == menuJobInsertBar) {
                text = _("Insert bar.");
            } else if (sender == menuJobDeleteBar) {
                text = _("Delete bar.");
            } else if (sender == menuJobRandomize) {
                text = _("Randomize items.");
            } else if (sender == menuLyricExpressionProperty) {
                text = _("Edit portamento/accent/decay of selected item");
            } else if (sender == menuLyricVibratoProperty) {
                text = _("Edit vibrato length and type of selected item");
            } else if (sender == menuLyricPhonemeTransformation) {
                text = _("Translate all phrase into phonetic symbol");
            } else if (sender == menuLyricDictionary) {
                text = _("Open configuration dialog for phonetic symnol dictionaries");
            } else if (sender == menuLyricCopyVibratoToPreset) {
                text = _("Copy vibrato config of selected item into vibrato preset");
            } else if (sender == menuScriptUpdate) {
                text = _("Read and compile all scripts and update the list of them");
            } else if (sender == menuSettingPreference) {
                text = _("Open configuration dialog for editor configs");
            } else if (sender == menuSettingPositionQuantize) {
                text = _("Change quantize resolution");
            } else if (sender == menuSettingGameControler) {
                text = _("Connect/Remove/Configure game controler");
            } else if (sender == menuSettingPaletteTool) {
                text = _("Configuration of palette tool");
            } else if (sender == menuSettingShortcut) {
                text = _("Open configuration dialog for shortcut key");
            } else if (sender == menuSettingVibratoPreset) {
                text = _("Open configuration dialog for vibrato preset");
            } else if (sender == menuSettingDefaultSingerStyle) {
                text = _("Edit default singer style");
            } else if (sender == menuSettingSequence) {
                text = _("Configuration of this sequence.");
            } else if (sender == menuTrackAdd) {
                text = _("Add new track.");
            } else if (sender == menuTrackBgm) {
                text = _("Add/Remove/Edit background music");
            } else if (sender == menuTrackOn) {
                text = _("Enable current track.");
            } else if (sender == menuTrackCopy) {
                text = _("Copy current track.");
            } else if (sender == menuTrackChangeName) {
                text = _("Change track name.");
            } else if (sender == menuTrackDelete) {
                text = _("Delete current track.");
            } else if (sender == menuTrackRenderCurrent) {
                text = _("Render current track.");
            } else if (sender == menuTrackRenderAll) {
                text = _("Render all tracks.");
            } else if (sender == menuTrackOverlay) {
                text = _("Show background items.");
            } else if (sender == menuTrackRenderer) {
                text = _("Select voice synthesis engine.");
            } else if (sender == menuTrackRendererAquesTone) {
                text = _("AquesTone");
            } else if (sender == menuTrackRendererUtau) {
                text = _("UTAU");
            } else if (sender == menuTrackRendererVCNT) {
                text = _("vConnect-STAND");
            } else if (sender == menuTrackRendererVOCALOID1) {
                text = _("VOCALOID1");
            } else if (sender == menuTrackRendererVOCALOID2) {
                text = _("VOCALOID2");
            } else if (sender == menuFileRecentClear) {
                text = _("Clear menu items");
            } else {
                notfound = true;
            }

#if DEBUG
            if (notfound && sender is ToolStripMenuItem) {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                Logger.write(typeof(FormMain) + ".handleMenuMouseEnter; cannot find message for " + item.Name + "\n");
            }
#endif
            statusLabel.Text = text;
        }

        public void handleSpaceKeyDown(Object sender, KeyEventArgs e)
        {
            if ((e.KeyCode & System.Windows.Forms.Keys.Space) == System.Windows.Forms.Keys.Space) {
                mSpacekeyDowned = true;
            }
        }

        public void handleSpaceKeyUp(Object sender, KeyEventArgs e)
        {
            if ((e.KeyCode & System.Windows.Forms.Keys.Space) == System.Windows.Forms.Keys.Space) {
                mSpacekeyDowned = false;
            }
        }

        public void handleChangeRenderer(Object sender, EventArgs e)
        {
            RendererKind kind = RendererKind.NULL;
            int resampler_index = -1;
            var menu_handler = renderer_menu_handler_.FirstOrDefault((handler) => handler.isMatch(sender));
            if (menu_handler != null && menu_handler.RendererKind != RendererKind.UTAU) {
                kind = menu_handler.RendererKind;
            } else {
                // イベント送信元のアイテムが，cMenuTrackTabRendererUtauまたは
                // menuTrackRendererUTAUのサブアイテムかどうかをチェック
                if (sender is System.Windows.Forms.ToolStripMenuItem) {
                    System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
                    resampler_index = cMenuTrackTabRendererUtau.DropDownItems.IndexOf(item);
                    if (resampler_index < 0) {
                        resampler_index = menuTrackRendererUtau.DropDownItems.IndexOf(item);
                    }
                }
                if (resampler_index < 0) {
                    // 検出できないのでbailout
                    return;
                }

                // 検出できた
                // このばあいは確実にUTAU
                kind = RendererKind.UTAU;
            }

            VsqFileEx vsq = AppManager.getVsqFile();
            int selected = AppManager.getSelected();
            VsqTrack vsq_track = vsq.Track[selected];
            RendererKind old = VsqFileEx.getTrackRendererKind(vsq_track);
            int old_resampler_index = VsqFileEx.getTrackResamplerUsed(vsq_track);
            bool changed = (old != kind);
            if (!changed && kind == RendererKind.UTAU) {
                changed = (old_resampler_index != resampler_index);
            }

            if (!changed) { return; }

            var track_copy = (VsqTrack)vsq_track.clone();
            List<VsqID> singers = AppManager.getSingerListFromRendererKind(kind);
            string renderer = kind.getVersionString();
            if (singers == null) {
                serr.println("FormMain#changeRendererCor; singers is null");
                return;
            }

            track_copy.changeRenderer(renderer, singers);
            VsqFileEx.setTrackRendererKind(track_copy, kind);
            if (kind == RendererKind.UTAU) {
                VsqFileEx.setTrackResamplerUsed(track_copy, resampler_index);
            }
            CadenciiCommand run = VsqFileEx.generateCommandTrackReplace(selected,
                                                                         track_copy,
                                                                         vsq.AttachedCurves.get(selected - 1));
            AppManager.editHistory.register(vsq.executeCommand(run));

            renderer_menu_handler_.ForEach((handler) => handler.updateCheckedState(kind));
            for (int i = 0; i < cMenuTrackTabRendererUtau.DropDownItems.Count; i++) {
                ((System.Windows.Forms.ToolStripMenuItem)cMenuTrackTabRendererUtau.DropDownItems[i]).Checked = (i == resampler_index);
            }
            for (int i = 0; i < menuTrackRendererUtau.DropDownItems.Count; i++) {
                ((System.Windows.Forms.ToolStripMenuItem)menuTrackRendererUtau.DropDownItems[i]).Checked = (i == resampler_index);
            }
            setEdited(true);
            refreshScreen();
        }

        public void handleBgmOffsetSeconds_Click(Object sender, EventArgs e)
        {
            if (!(sender is BgmMenuItem)) {
                return;
            }
            BgmMenuItem menu = (BgmMenuItem)sender;
            int index = menu.getBgmIndex();
            InputBox ib = null;
            try {
                ib = new InputBox(_("Input Offset Seconds"));
                ib.Location = getFormPreferedLocation(ib);
                ib.setResult(AppManager.getBgm(index).readOffsetSeconds + "");
                DialogResult dr = AppManager.showModalDialog(ib, this);
                if (dr != DialogResult.OK) {
                    return;
                }
                List<BgmFile> list = new List<BgmFile>();
                int count = AppManager.getBgmCount();
                BgmFile item = null;
                for (int i = 0; i < count; i++) {
                    if (i == index) {
                        item = (BgmFile)AppManager.getBgm(i).clone();
                        list.Add(item);
                    } else {
                        list.Add(AppManager.getBgm(i));
                    }
                }
                double draft;
                try {
                    draft = double.Parse(ib.getResult());
                    item.readOffsetSeconds = draft;
                    menu.ToolTipText = draft + " " + _("seconds");
                } catch (Exception ex3) {
                    Logger.write(typeof(FormMain) + ".handleBgmOffsetSeconds_Click; ex=" + ex3 + "\n");
                }
                CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate(list);
                AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
                setEdited(true);
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".handleBgmOffsetSeconds_Click; ex=" + ex + "\n");
            } finally {
                if (ib != null) {
                    try {
                        ib.Dispose();
                    } catch (Exception ex2) {
                        Logger.write(typeof(FormMain) + ".handleBgmOffsetSeconds_Click; ex=" + ex2 + "\n");
                    }
                }
            }
        }

        public void handleBgmStartAfterPremeasure_CheckedChanged(Object sender, EventArgs e)
        {
            if (!(sender is BgmMenuItem)) {
                return;
            }
            BgmMenuItem menu = (BgmMenuItem)sender;
            int index = menu.getBgmIndex();
            List<BgmFile> list = new List<BgmFile>();
            int count = AppManager.getBgmCount();
            for (int i = 0; i < count; i++) {
                if (i == index) {
                    BgmFile item = (BgmFile)AppManager.getBgm(i).clone();
                    item.startAfterPremeasure = menu.Checked;
                    list.Add(item);
                } else {
                    list.Add(AppManager.getBgm(i));
                }
            }
            CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate(list);
            AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(run));
            setEdited(true);
        }

        public void handleBgmAdd_Click(Object sender, EventArgs e)
        {
            string dir = AppManager.editorConfig.getLastUsedPathIn("wav");
            openWaveDialog.SetSelectedFile(dir);
            var ret = AppManager.showModalDialog(openWaveDialog, true, this);
            if (ret != System.Windows.Forms.DialogResult.OK) {
                return;
            }

            string file = openWaveDialog.FileName;
            AppManager.editorConfig.setLastUsedPathIn(file, ".wav");

            // 既に開かれていたらキャンセル
            int count = AppManager.getBgmCount();
            bool found = false;
            for (int i = 0; i < count; i++) {
                BgmFile item = AppManager.getBgm(i);
                if (file == item.file) {
                    found = true;
                    break;
                }
            }
            if (found) {
                AppManager.showMessageBox(
                    PortUtil.formatMessage(_("file '{0}' is already registered as BGM."), file),
                    _("Error"),
                    cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                    cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE);
                return;
            }

            // 登録
            AppManager.addBgm(file);
            setEdited(true);
            updateBgmMenuState();
        }

        public void handleBgmRemove_Click(Object sender, EventArgs e)
        {
            if (!(sender is BgmMenuItem)) {
                return;
            }
            BgmMenuItem parent = (BgmMenuItem)sender;
            int index = parent.getBgmIndex();
            BgmFile bgm = AppManager.getBgm(index);
            if (AppManager.showMessageBox(PortUtil.formatMessage(_("remove '{0}'?"), bgm.file),
                                  "Cadencii",
                                  cadencii.windows.forms.Utility.MSGBOX_YES_NO_OPTION,
                                  cadencii.windows.forms.Utility.MSGBOX_QUESTION_MESSAGE) != DialogResult.Yes) {
                return;
            }
            AppManager.removeBgm(index);
            setEdited(true);
            updateBgmMenuState();
        }

        public void handleSettingPaletteTool(Object sender, EventArgs e)
        {
#if ENABLE_SCRIPT
            if (!(sender is PaletteToolMenuItem)) {
                return;
            }
            PaletteToolMenuItem tsmi = (PaletteToolMenuItem)sender;
            string id = tsmi.getPaletteToolID();
            if (!PaletteToolServer.loadedTools.ContainsKey(id)) {
                return;
            }
            Object instance = PaletteToolServer.loadedTools[id];
            IPaletteTool ipt = (IPaletteTool)instance;
            if (ipt.openDialog() == System.Windows.Forms.DialogResult.OK) {
                XmlSerializer xsms = new XmlSerializer(instance.GetType(), true);
                string dir = Path.Combine(Utility.getApplicationDataPath(), "tool");
                if (!Directory.Exists(dir)) {
                    PortUtil.createDirectory(dir);
                }
                string cfg = id + ".config";
                string config = Path.Combine(dir, cfg);
                FileStream fs = null;
                try {
                    fs = new FileStream(config, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    xsms.serialize(fs, null);
                } catch (Exception ex) {
                    Logger.write(typeof(FormMain) + ".handleSettingPaletteTool; ex=" + ex + "\n");
                } finally {
                    if (fs != null) {
                        try {
                            fs.Close();
                        } catch (Exception ex2) {
                            Logger.write(typeof(FormMain) + ".handleSettingPaletteTool; ex=" + ex2 + "\n");
                        }
                    }
                }
            }
#endif
        }

#if ENABLE_SCRIPT
        public void handleScriptMenuItem_Click(Object sender, EventArgs e)
        {
#if DEBUG
            sout.println("FormMain#handleScriptMenuItem_Click; sender.GetType()=" + sender.GetType());
#endif
            try {
                string dir = Utility.getScriptPath();
                string id = "";
                if (sender is PaletteToolMenuItem) {
                    id = ((PaletteToolMenuItem)sender).getPaletteToolID();
                }
#if DEBUG
                sout.println("FormMain#handleScriptMenuItem_Click; id=" + id);
#endif
                string script_file = Path.Combine(dir, id);
                if (ScriptServer.getTimestamp(id) != PortUtil.getFileLastModified(script_file)) {
                    ScriptServer.reload(id);
                }
                if (ScriptServer.isAvailable(id)) {
                    if (ScriptServer.invokeScript(id, AppManager.getVsqFile())) {
                        setEdited(true);
                        updateDrawObjectList();
                        int selected = AppManager.getSelected();
#if DEBUG
                        sout.println("FormMain#handleScriptMenuItem_Click; ScriptServer.invokeScript has returned TRUE");
#endif
                        AppManager.itemSelection.updateSelectedEventInstance();
                        AppManager.propertyPanel.updateValue(selected);
                        refreshScreen();
                    }
                } else {
                    FormCompileResult dlg = null;
                    try {
                        dlg = new FormCompileResult(_("Failed loading script."), ScriptServer.getCompileMessage(id));
                        AppManager.showModalDialog(dlg, this);
                    } catch (Exception ex) {
                        Logger.write(typeof(FormMain) + ".handleScriptMenuItem_Click; ex=" + ex + "\n");
                    } finally {
                        if (dlg != null) {
                            try {
                                dlg.Close();
                            } catch (Exception ex2) {
                                Logger.write(typeof(FormMain) + ".handleScriptMenuItem_Click; ex=" + ex2 + "\n");
                            }
                        }
                    }
                }
            } catch (Exception ex3) {
                Logger.write(typeof(FormMain) + ".handleScriptMenuItem_Click; ex=" + ex3 + "\n");
#if DEBUG
                sout.println("AppManager#dd_run_Click; ex3=" + ex3);
#endif
            }
        }
#endif

#if ENABLE_MTC
        /// <summary>
        /// MTC用のMIDI-INデバイスからMIDIを受信します。
        /// </summary>
        /// <param name="now"></param>
        /// <param name="dataArray"></param>
        private void handleMtcMidiReceived( double now, byte[] dataArray ) {
            byte data = (byte)(dataArray[1] & 0x0f);
            byte type = (byte)((dataArray[1] >> 4) & 0x0f);
            if ( type == 0 ) {
                mtcFrameLsb = data;
            } else if ( type == 1 ) {
                mtcFrameMsb = data;
            } else if ( type == 2 ) {
                mtcSecLsb = data;
            } else if ( type == 3 ) {
                mtcSecMsb = data;
            } else if ( type == 4 ) {
                mtcMinLsb = data;
            } else if ( type == 5 ) {
                mtcMinMsb = data;
            } else if ( type == 6 ) {
                mtcHourLsb = data;
            } else if ( type == 7 ) {
                mtcHourMsb = (byte)(data & 1);
                int fpsType = (data & 6) >> 1;
                double fps = 30.0;
                if ( fpsType == 0 ) {
                    fps = 24.0;
                } else if ( fpsType == 1 ) {
                    fps = 25;
                } else if ( fpsType == 2 ) {
                    fps = 30000.0 / 1001.0;
                } else if ( fpsType == 3 ) {
                    fps = 30.0;
                }
                int hour = (mtcHourMsb << 4 | mtcHourLsb);
                int min = (mtcMinMsb << 4 | mtcMinLsb);
                int sec = (mtcSecMsb << 4 | mtcSecLsb);
                int frame = (mtcFrameMsb << 4 | mtcFrameLsb) + 2;
                double time = (hour * 60.0 + min) * 60.0 + sec + frame / fps;
                mtcLastReceived = now;
#if DEBUG
                int clock = (int)AppManager.getVsqFile().getClockFromSec( time );
                AppManager.setCurrentClock( clock );
#endif
                /*if ( !AppManager.isPlaying() ) {
                    AppManager.setEditMode( EditMode.REALTIME_MTC );
                    AppManager.setPlaying( true );
                    EventHandler handler = new EventHandler( AppManager_PreviewStarted );
                    if ( handler != null ) {
                        this.Invoke( handler );
                        while ( VSTiProxy.getPlayTime() <= 0.0 ) {
                            System.Windows.Forms.Application.DoEvents();
                        }
                        AppManager.setPlaying( true );
                    }
                }*/
#if DEBUG
                sout.println( "FormMain#handleMtcMidiReceived; time=" + time );
#endif
            }
        }
#endif

#if ENABLE_MIDI
        public void mMidiIn_MidiReceived(Object sender, javax.sound.midi.MidiMessage message)
        {
            byte[] data = message.getMessage();
#if DEBUG
            sout.println("FormMain#mMidiIn_MidiReceived; data.Length=" + data.Length);
#endif
            if (data.Length <= 2) {
                return;
            }
#if DEBUG
            sout.println("FormMain#mMidiIn_MidiReceived; AppManager.isPlaying()=" + AppManager.isPlaying());
#endif
            if (AppManager.isPlaying()) {
                return;
            }
#if DEBUG
            sout.println("FormMain#mMidiIn_MidiReceived; isStepSequencerEnabeld()=" + controller.isStepSequencerEnabled());
#endif
            if (false == controller.isStepSequencerEnabled()) {
                return;
            }
            int code = data[0] & 0xf0;
            if (code != 0x80 && code != 0x90) {
                return;
            }
            if (code == 0x90 && data[2] == 0x00) {
                code = 0x80;//ベロシティ0のNoteOnはNoteOff
            }

            int note = (0xff & data[1]);

            int clock = AppManager.getCurrentClock();
            int unit = AppManager.getPositionQuantizeClock();
            if (unit > 1) {
                clock = doQuantize(clock, unit);
            }

#if DEBUG
            sout.println("FormMain#mMidiIn_Received; clock=" + clock + "; note=" + note);
#endif
            if (code == 0x80) {
                /*if ( AppManager.mAddingEvent != null ) {
                    int len = clock - AppManager.mAddingEvent.Clock;
                    if ( len <= 0 ) {
                        len = unit;
                    }
                    AppManager.mAddingEvent.ID.Length = len;
                    int selected = AppManager.getSelected();
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventAdd( selected,
                                                                                                   AppManager.mAddingEvent ) );
                    AppManager.register( AppManager.getVsqFile().executeCommand( run ) );
                    if ( !isEdited() ) {
                        setEdited( true );
                    }
                    updateDrawObjectList();
                }*/
            } else if (code == 0x90) {
                if (AppManager.mAddingEvent != null) {
                    // mAddingEventがnullでない場合は打ち込みの試行中(未確定の音符がある)
                    // であるので，ノートだけが変わるようにする
                    clock = AppManager.mAddingEvent.Clock;
                } else {
                    AppManager.mAddingEvent = new VsqEvent();
                }
                AppManager.mAddingEvent.Clock = clock;
                if (AppManager.mAddingEvent.ID == null) {
                    AppManager.mAddingEvent.ID = new VsqID();
                }
                AppManager.mAddingEvent.ID.type = VsqIDType.Anote;
                AppManager.mAddingEvent.ID.Dynamics = 64;
                AppManager.mAddingEvent.ID.VibratoHandle = null;
                if (AppManager.mAddingEvent.ID.LyricHandle == null) {
                    AppManager.mAddingEvent.ID.LyricHandle = new LyricHandle("a", "a");
                }
                AppManager.mAddingEvent.ID.LyricHandle.L0.Phrase = "a";
                AppManager.mAddingEvent.ID.LyricHandle.L0.setPhoneticSymbol("a");
                AppManager.mAddingEvent.ID.Note = note;

                // 音符の長さを計算
                int length = QuantizeModeUtil.getQuantizeClock(
                        AppManager.editorConfig.getLengthQuantize(),
                        AppManager.editorConfig.isLengthQuantizeTriplet());

                // 音符の長さを設定
                Utility.editLengthOfVsqEvent(
                    AppManager.mAddingEvent,
                    length,
                    AppManager.vibratoLengthEditingRule);

                // 現在位置は，音符の末尾になる
                AppManager.setCurrentClock(clock + length);

                // 画面を再描画
                if (this.InvokeRequired) {
                    DelegateRefreshScreen deleg = null;
                    try {
                        deleg = new DelegateRefreshScreen(refreshScreen);
                    } catch (Exception ex4) {
                        deleg = null;
                    }
                    if (deleg != null) {
                        this.Invoke(deleg, true);
                    }
                } else {
                    refreshScreen(true);
                }
                // 鍵盤音を鳴らす
                KeySoundPlayer.play(note);
            }
        }
#endif
        #endregion

        #region public static methods
        /// <summary>
        /// 文字列を、現在の言語設定に従って翻訳します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        /// <summary>
        /// VsqEvent, VsqBPList, BezierCurvesの全てのクロックを、tempoに格納されているテンポテーブルに
        /// 合致するようにシフトします．ただし，このメソッド内ではtargetのテンポテーブルは変更せず，クロック値だけが変更される．
        /// </summary>
        /// <param name="work"></param>
        /// <param name="tempo"></param>
        public static void shiftClockToMatchWith(VsqFileEx target, VsqFile tempo, double shift_seconds)
        {
            // テンポをリプレースする場合。
            // まずクロック値を、リプレース後のモノに置き換え
            for (int track = 1; track < target.Track.Count; track++) {
                // ノート・歌手イベントをシフト
                for (Iterator<VsqEvent> itr = target.Track[track].getEventIterator(); itr.hasNext(); ) {
                    VsqEvent item = itr.next();
                    if (item.ID.type == VsqIDType.Singer && item.Clock == 0) {
                        continue;
                    }
                    int clock = item.Clock;
                    double sec_start = target.getSecFromClock(clock) + shift_seconds;
                    double sec_end = target.getSecFromClock(clock + item.ID.getLength()) + shift_seconds;
                    int clock_start = (int)tempo.getClockFromSec(sec_start);
                    int clock_end = (int)tempo.getClockFromSec(sec_end);
                    item.Clock = clock_start;
                    item.ID.setLength(clock_end - clock_start);
                    if (item.ID.VibratoHandle != null) {
                        double sec_vib_start = target.getSecFromClock(clock + item.ID.VibratoDelay) + shift_seconds;
                        int clock_vib_start = (int)tempo.getClockFromSec(sec_vib_start);
                        item.ID.VibratoDelay = clock_vib_start - clock_start;
                        item.ID.VibratoHandle.setLength(clock_end - clock_vib_start);
                    }
                }

                // コントロールカーブをシフト
                for (int j = 0; j < Utility.CURVE_USAGE.Length; j++) {
                    CurveType ct = Utility.CURVE_USAGE[j];
                    VsqBPList item = target.Track[track].getCurve(ct.getName());
                    if (item == null) {
                        continue;
                    }
                    VsqBPList repl = new VsqBPList(item.getName(), item.getDefault(), item.getMinimum(), item.getMaximum());
                    for (int i = 0; i < item.size(); i++) {
                        int clock = item.getKeyClock(i);
                        int value = item.getElement(i);
                        double sec = target.getSecFromClock(clock) + shift_seconds;
                        if (sec >= 0) {
                            int clock_new = (int)tempo.getClockFromSec(sec);
                            repl.add(clock_new, value);
                        }
                    }
                    target.Track[track].setCurve(ct.getName(), repl);
                }

                // ベジエカーブをシフト
                for (int j = 0; j < Utility.CURVE_USAGE.Length; j++) {
                    CurveType ct = Utility.CURVE_USAGE[j];
                    List<BezierChain> list = target.AttachedCurves.get(track - 1).get(ct);
                    if (list == null) {
                        continue;
                    }
                    foreach (var chain in list) {
                        foreach (var point in chain.points) {
                            PointD bse = new PointD(tempo.getClockFromSec(target.getSecFromClock(point.getBase().getX()) + shift_seconds),
                                                     point.getBase().getY());
                            double rx = point.getBase().getX() + point.controlRight.getX();
                            double new_rx = tempo.getClockFromSec(target.getSecFromClock(rx) + shift_seconds);
                            PointD ctrl_r = new PointD(new_rx - bse.getX(), point.controlRight.getY());

                            double lx = point.getBase().getX() + point.controlLeft.getX();
                            double new_lx = tempo.getClockFromSec(target.getSecFromClock(lx) + shift_seconds);
                            PointD ctrl_l = new PointD(new_lx - bse.getX(), point.controlLeft.getY());
                            point.setBase(bse);
                            point.controlLeft = ctrl_l;
                            point.controlRight = ctrl_r;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// スクリーンに対して、ウィンドウの最適な位置を取得する
        /// </summary>
        public static Point getAppropriateDialogLocation(int x, int y, int width, int height, int workingAreaX, int workingAreaY, int workingAreaWidth, int workingAreaHeight)
        {
            int top = y;
            if (top + height > workingAreaY + workingAreaHeight) {
                // ダイアログの下端が隠れる場合、位置をずらす
                top = workingAreaY + workingAreaHeight - height;
            }
            if (top < workingAreaY) {
                // ダイアログの上端が隠れる場合、位置をずらす
                top = workingAreaY;
            }
            int left = x;
            if (left + width > workingAreaX + workingAreaWidth) {
                left = workingAreaX + workingAreaWidth - width;
            }
            if (left < workingAreaX) {
                left = workingAreaX;
            }
            return new Point(left, top);
        }

        /// <summary>
        /// フォームのタイトルバーが画面内に入るよう、Locationを正規化します
        /// </summary>
        /// <param name="form"></param>
        public static void normalizeFormLocation(Form dlg)
        {
            Rectangle rcScreen = PortUtil.getWorkingArea(dlg);
            Point p = getAppropriateDialogLocation(
                dlg.Left, dlg.Top, dlg.Width, dlg.Height,
                rcScreen.x, rcScreen.y, rcScreen.width, rcScreen.height
            );
            dlg.Location = new System.Drawing.Point(p.x, p.y);
        }
        #endregion

        private void menuToolsCreateVConnectSTANDDb_Click(object sender, EventArgs e)
        {
            string creator = Path.Combine(System.Windows.Forms.Application.StartupPath, "vConnectStandDBConvert.exe");
            if (System.IO.File.Exists(creator)) {
                Process.Start(creator);
            }
        }

        private void menuHelpCheckForUpdates_Click(object sender, EventArgs args)
        {
            showUpdateInformationAsync(true);
        }
    }

}
