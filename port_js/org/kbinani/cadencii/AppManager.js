/*
 * AppManager.cs
 * Copyright (C) 2009-2010 kbinani
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
//#define ENABLE_OBSOLUTE_COMMAND
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.cadencii == undefined ) org.kbinani.cadencii = {};
if( org.kbinani.cadencii.AppManager == undefined ){

    org.kbinani.cadencii.AppManager = {};

    org.kbinani.cadencii.AppManager.MIN_KEY_WIDTH = 68;
    org.kbinani.cadencii.AppManager.MAX_KEY_WIDTH = org.kbinani.cadencii.AppManager.MIN_KEY_WIDTH * 5;
    org.kbinani.cadencii.AppManager.CONFIG_FILE_NAME = "config.xml";
    /// <summary>
    /// OSのクリップボードに貼り付ける文字列の接頭辞．これがついていた場合，クリップボードの文字列をCadenciiが使用できると判断する．
    /// </summary>
    org.kbinani.cadencii.AppManager.CLIP_PREFIX = "CADENCIIOBJ";
    /// <summary>
    /// 強弱記号の，ピアノロール画面上の表示幅（ピクセル）
    /// </summary>
    org.kbinani.cadencii.AppManager.DYNAFF_ITEM_WIDTH = 40;
    org.kbinani.cadencii.AppManager.FONT_SIZE8 = 14;
    org.kbinani.cadencii.AppManager.FONT_SIZE9 = org.kbinani.cadencii.AppManager.FONT_SIZE8 + 1;
    org.kbinani.cadencii.AppManager.FONT_SIZE10 = org.kbinani.cadencii.AppManager.FONT_SIZE8 + 2;
    org.kbinani.cadencii.AppManager.FONT_SIZE50 = org.kbinani.cadencii.AppManager.FONT_SIZE8 + 42;

    /// <summary>
    /// 鍵盤の表示幅(pixel)
    /// </summary>
    org.kbinani.cadencii.AppManager.keyWidth = org.kbinani.cadencii.AppManager.MIN_KEY_WIDTH;// * 2;
    /// <summary>
    /// keyWidth+keyOffsetの位置からが、0になってる
    /// </summary>
    org.kbinani.cadencii.AppManager.keyOffset = 6;
    /// <summary>
    /// エディタの設定
    /// </summary>
    org.kbinani.cadencii.AppManager.editorConfig = new org.kbinani.cadencii.EditorConfig();
    /*
    /// <summary>
    /// AttachedCurve用のシリアライザ
    /// </summary>
    public static XmlSerializer xmlSerializerListBezierCurves = new XmlSerializer( AttachedCurve.class );*/
    /*
    public static Font baseFont8 = new Font( "Dialog", Font.PLAIN, FONT_SIZE8 );
    public static Font baseFont9 = new Font( "Dialog", Font.PLAIN, FONT_SIZE9 );
    /// <summary>
    /// ピアノロールの歌詞の描画に使用されるフォント。
    /// </summary>
    public static Font baseFont10 = new Font( "Dialog", Font.PLAIN, FONT_SIZE10 );
    /// <summary>
    /// ピアノロールの歌詞の描画に使用されるフォント。（発音記号固定の物の場合）
    /// </summary>
    public static Font baseFont10Bold = new Font( "Dialog", Font.BOLD, FONT_SIZE10 );
    public static Font baseFont50Bold = new Font( "Dialog", Font.BOLD, FONT_SIZE50 );
    /// <summary>
    /// 歌詞を音符の（高さ方向の）真ん中に描画するためのオフセット
    /// </summary>
    public static int baseFont10OffsetHeight = 0;
    public static int baseFont8OffsetHeight = 0;
    public static int baseFont9OffsetHeight = 0;
    public static int baseFont50OffsetHeight = 0;
    public static int baseFont8Height = FONT_SIZE8;
    public static int baseFont9Height = FONT_SIZE9;
    public static int baseFont10Height = FONT_SIZE10;
    public static int baseFont50Height = FONT_SIZE50;
#if ENABLE_PROPERTY
    public static PropertyPanel propertyPanel;
    public static FormNoteProperty propertyWindow;
#endif
    public static FormIconPalette iconPalette = null;*/

    /*#region Static Readonly Fields
    public static readonly Color[] HILIGHT = new Color[] { 
        new Color( 181, 220, 16 ),
        new Color( 231, 244, 49 ),
        new Color( 252, 230, 29 ),
        new Color( 247, 171, 20 ),
        new Color( 249, 94, 17 ),
        new Color( 234, 27, 47 ),
        new Color( 175, 20, 80 ),
        new Color( 183, 24, 149 ),
        new Color( 105, 22, 158 ),
        new Color( 22, 36, 163 ),
        new Color( 37, 121, 204 ),
        new Color( 29, 179, 219 ),
        new Color( 24, 239, 239 ),
        new Color( 25, 206, 175 ),
        new Color( 23, 160, 134 ),
        new Color( 79, 181, 21 ) };
    public static readonly Color[] RENDER = new Color[]{
        new Color( 19, 143, 52 ),
        new Color( 158, 154, 18 ),
        new Color( 160, 143, 23 ),
        new Color( 145, 98, 15 ),
        new Color( 142, 52, 12 ),
        new Color( 142, 19, 37 ),
        new Color( 96, 13, 47 ),
        new Color( 117, 17, 98 ),
        new Color( 62, 15, 99 ),
        new Color( 13, 23, 84 ),
        new Color( 25, 84, 132 ),
        new Color( 20, 119, 142 ),
        new Color( 19, 142, 139 ),
        new Color( 17, 122, 102 ),
        new Color( 13, 86, 72 ),
        new Color( 43, 91, 12 ) };
    public static readonly String[] USINGS = new String[] { "using System;",
                                         "using System.IO;",
                                         "using org.kbinani.vsq;",
                                         "using org.kbinani.cadencii;",
                                         "using org.kbinani;",
                                         "using org.kbinani.java.io;",
                                         "using org.kbinani.java.util;",
                                         "using org.kbinani.java.awt;",
                                         "using org.kbinani.media;",
                                         "using org.kbinani.apputil;",
                                         "using System.Windows.Forms;",
                                         "using System.Collections.Generic;",
                                         "using System.Drawing;",
                                         "using System.Text;",
                                         "using System.Xml.Serialization;" };
    public static readonly Vector<BKeys> SHORTCUT_ACCEPTABLE = new Vector<BKeys>( Arrays.asList( new BKeys[]{
        BKeys.A,
        BKeys.B,
        BKeys.C,
        BKeys.D,
        BKeys.D0,
        BKeys.D1,
        BKeys.D2,
        BKeys.D3,
        BKeys.D4,
        BKeys.D5,
        BKeys.D6,
        BKeys.D7,
        BKeys.D8,
        BKeys.D9,
        BKeys.Down,
        BKeys.E,
        BKeys.F,
        BKeys.F1,
        BKeys.F2,
        BKeys.F3,
        BKeys.F4,
        BKeys.F5,
        BKeys.F6,
        BKeys.F7,
        BKeys.F8,
        BKeys.F9,
        BKeys.F10,
        BKeys.F11,
        BKeys.F12,
        BKeys.F13,
        BKeys.F14,
        BKeys.F15,
        BKeys.F16,
        BKeys.F17,
        BKeys.F18,
        BKeys.F19,
        BKeys.F20,
        BKeys.F21,
        BKeys.F22,
        BKeys.F23,
        BKeys.F24,
        BKeys.G,
        BKeys.H,
        BKeys.I,
        BKeys.J,
        BKeys.K,
        BKeys.L,
        BKeys.Left,
        BKeys.M,
        BKeys.N,
        BKeys.NumPad0,
        BKeys.NumPad1,
        BKeys.NumPad2,
        BKeys.NumPad3,
        BKeys.NumPad4,
        BKeys.NumPad5,
        BKeys.NumPad6,
        BKeys.NumPad7,
        BKeys.NumPad8,
        BKeys.NumPad9,
        BKeys.O,
        BKeys.P,
        BKeys.PageDown,
        BKeys.PageUp,
        BKeys.Q,
        BKeys.R,
        BKeys.Right,
        BKeys.S,
        BKeys.Space,
        BKeys.T,
        BKeys.U,
        BKeys.Up,
        BKeys.V,
        BKeys.W,
        BKeys.X,
        BKeys.Y,
        BKeys.Z,
        BKeys.Delete,
        BKeys.Home,
        BKeys.End,
    } ) );
    #endregion

    #region Private Static Fields*/
    org.kbinani.cadencii.AppManager._s_base_tempo = 480000;
    org.kbinani.cadencii.AppManager._s_hilight_brush = org.kbinani.PortUtil.CornflowerBlue.clone();
    /*private static Object s_locker;
    private static BTimer s_auto_backup_timer;
    #endregion*/

    org.kbinani.cadencii.AppManager._s_vsq = new org.kbinani.vsq.VsqFile( "MikU", 2, 4, 4, 500000 );
    org.kbinani.cadencii.AppManager._s_file = "";
    org.kbinani.cadencii.AppManager._s_selected = 1;
    org.kbinani.cadencii.AppManager._s_current_clock = 0;
    org.kbinani.cadencii.AppManager._s_playing = false;
    org.kbinani.cadencii.AppManager._s_repeat_mode = false;
    org.kbinani.cadencii.AppManager._s_grid_visible = false;
    /*
    private static EditMode s_edit_mode = EditMode.NONE;*/
    /// <summary>
    /// トラックのオーバーレイ表示
    /// </summary>
    org.kbinani.cadencii.AppManager._s_overlay = true;
    /// <summary>
    /// 選択されているイベントのリスト
    /// </summary>
    org.kbinani.cadencii.AppManager._s_selected_events = new Array();
    /*/// <summary>
    /// 選択されているテンポ変更イベントのリスト
    /// </summary>
    private static TreeMap<Integer, SelectedTempoEntry> _s_selected_tempo = new TreeMap<Integer, SelectedTempoEntry>();
    private static int s_last_selected_tempo = -1;
    /// <summary>
    /// 選択されている拍子変更イベントのリスト
    /// </summary>
    private static TreeMap<Integer, SelectedTimesigEntry> _s_selected_timesig = new TreeMap<Integer, SelectedTimesigEntry>();
    private static int s_last_selected_timesig = -1;
    private static EditTool _s_selected_tool = EditTool.PENCIL;
#if !TREECOM
    private static Vector<ICommand> s_commands = new Vector<ICommand>();
    private static int s_command_position = -1;
#endif
    /// <summary>
    /// 選択されているベジエ点のリスト
    /// </summary>
    private static Vector<SelectedBezierPoint> _s_selected_bezier = new Vector<SelectedBezierPoint>();
    /// <summary>
    /// 最後に選択されたベジエ点
    /// </summary>
    private static SelectedBezierPoint s_last_selected_bezier = new SelectedBezierPoint();
    /// <summary>
    /// 現在の演奏カーソルの位置(m_current_clockと意味は同じ。CurrentClockが変更されると、自動で更新される)
    /// </summary>
    private static PlayPositionSpecifier s_current_play_position = new PlayPositionSpecifier();

    /// <summary>
    /// selectedPointIDsに格納されているデータ点の，CurveType
    /// </summary>
    private static CurveType selectedPointCurveType = CurveType.Empty;
    private static Vector<Long> selectedPointIDs = new Vector<Long>();
    /// <summary>
    /// Playingプロパティにロックをかけるためのオブジェクト
    /// </summary>
    private static Object playingPropertyLocker = new Object();

    #region 選択範囲の管理
    /// <summary>
    /// SelectedRegionが有効かどうかを表すフラグ
    /// </summary>
    private static boolean wholeSelectedIntervalEnabled = false;
    /// <summary>
    /// Ctrlキーを押しながらのマウスドラッグ操作による選択が行われた範囲(単位：クロック)
    /// </summary>
    public static SelectedRegion wholeSelectedInterval = new SelectedRegion( 0 );
    /// <summary>
    /// コントロールカーブ上で現在選択されている範囲（x:クロック、y:各コントロールカーブの単位に同じ）。マウスが動いているときのみ使用
    /// </summary>
    public static Rectangle curveSelectingRectangle = new Rectangle();
    /// <summary>
    /// コントロールカーブ上で選択された範囲（単位：クロック）
    /// </summary>
    public static SelectedRegion curveSelectedInterval = new SelectedRegion( 0 );
    /// <summary>
    /// 選択範囲が有効かどうか。
    /// </summary>
    private static boolean curveSelectedIntervalEnabled = false;
    /// <summary>
    /// 範囲選択モードで音符を動かしている最中の、選択範囲の開始位置（クロック）。マウスが動いているときのみ使用
    /// </summary>
    public static int wholeSelectedIntervalStartForMoving = 0;
    #endregion*/

    /// <summary>
    /// 自動ノーマライズモードかどうかを表す値を取得、または設定します。
    /// </summary> 
    org.kbinani.cadencii.AppManager.autoNormalize = false;
    /// <summary>
    /// エンドマーカーの位置(clock)
    /// </summary>
    org.kbinani.cadencii.AppManager.endMarker = 0;
    /// <summary>
    /// エンドマーカーが有効かどうか
    /// </summary>
    org.kbinani.cadencii.AppManager.endMarkerEnabled = false;
    /// <summary>
    /// x方向の表示倍率(pixel/clock)
    /// </summary>
    org.kbinani.cadencii.AppManager.scaleX = 0.1;
    /// <summary>
    /// スタートマーカーの位置(clock)
    /// </summary>
    org.kbinani.cadencii.AppManager.startMarker = 0;
    /// <summary>
    /// Bezierカーブ編集モードが有効かどうかを表す
    /// </summary>
    org.kbinani.cadencii.AppManager._s_is_curve_mode = false;
    /// <summary>
    /// スタートマーカーが有効かどうか
    /// </summary>
    org.kbinani.cadencii.AppManager.startMarkerEnabled = false;
    /// <summary>
    /// 再生時に自動スクロールするかどうか
    /// </summary>
    org.kbinani.cadencii.AppManager.autoScroll = true;
    /// <summary>
    /// 最初のバッファが書き込まれたかどうか
    /// </summary>
    org.kbinani.cadencii.AppManager.firstBufferWritten = false;
    /// <summary>
    /// リアルタイム再生で使用しようとしたVSTiが有効だったかどうか。
    /// </summary>
    org.kbinani.cadencii.AppManager.rendererAvailable = false;
    /// <summary>
    /// プレビュー再生が開始された時刻
    /// </summary>
    org.kbinani.cadencii.AppManager.previewStartedTime = 0;
    /// <summary>
    /// 画面左端位置での、仮想画面上の画面左端から測ったピクセル数．
    /// FormMain.hScroll.ValueとFormMain.trackBar.Valueで決まる．
    /// </summary>
    org.kbinani.cadencii.AppManager._startToDrawX = 0;
    /// <summary>
    /// 画面上端位置での、仮想画面上の画面上端から図ったピクセル数．
    /// FormMain.vScroll.Value，FormMain.vScroll.Height，FormMain.vScroll.Maximum,AppManager.editorConfig.PxTrackHeightによって決まる
    /// </summary>
    org.kbinani.cadencii.AppManager._startToDrawY = 0;
    org.kbinani.cadencii.AppManager.selectedPaletteTool = "";
    /*public static ScreenStatus lastScreenStatus = new ScreenStatus();*/
    org.kbinani.cadencii.AppManager._s_id = "";
    /*public static FormMain mainWindow = null;*/
    /*
    /// <summary>
    /// ミキサーダイアログ
    /// </summary>
    public static FormMixer mixerWindow;*/
    /**
     * 画面に描かれるエントリのリスト．trackBar.Valueの変更やエントリの編集などのたびに更新される
     */
    org.kbinani.cadencii.AppManager.drawObjects = new Array();
    /*public static LyricTextBox inputTextBox = null;
    public static int addingEventLength;
    public static VsqEvent addingEvent;*/
    /**
     * AppManager.m_draw_objectsを描く際の，最初に検索されるインデクス．
     */
    org.kbinani.cadencii.AppManager.drawStartIndex = new Array( 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 );
    /*/// <summary>
    /// マウスが降りていて，かつ範囲選択をしているときに立つフラグ
    /// </summary>
    public static boolean isPointerDowned = false;
    /// <summary>
    /// マウスが降りた仮想スクリーン上の座標(ピクセル)
    /// </summary>
    public static Point mouseDownLocation = new Point();
    public static int lastTrackSelectorHeight;
    /// <summary>
    /// UTAUの原音設定のリスト。TreeMapのキーは、oto.iniのあるディレクトリ名になっている。
    /// </summary>
    public static TreeMap<String, UtauVoiceDB> utauVoiceDB = new TreeMap<String, UtauVoiceDB>();
    /// <summary>
    /// 最後にレンダリングが行われた時の、トラックの情報が格納されている。
    /// </summary>
    public static RenderedStatus[] lastRenderedStatus = new RenderedStatus[16];
    /// <summary>
    /// RenderingStatusをXMLシリアライズするためのシリアライザ
    /// </summary>
    public static XmlSerializer renderingStatusSerializer = new XmlSerializer( typeof( RenderedStatus ) );
    /// <summary>
    /// wavを出力するための一時ディレクトリのパス。
    /// </summary>
    private static String tempWaveDir = "";
    /// <summary>
    /// 再生開始からの経過時刻がこの秒数以下の場合、再生を止めることが禁止される。
    /// </summary>
    public static double forbidFlipPlayingThresholdSeconds = 0.2;
    /// <summary>
    /// ピアノロール画面に，コントロールカーブをオーバーレイしているモード
    /// </summary>
    public static boolean curveOnPianoroll = false;

    #region 裏設定項目
    /// <summary>
    /// 再生中にWAVE波形の描画をスキップするかどうか（デフォルトはtrue）
    /// </summary>
    public static boolean skipDrawingWaveformWhenPlaying = true;
    /// <summary>
    /// コントロールカーブに、音符の境界線を重ね描きするかどうか（デフォルトはtrue）
    /// </summary>
    public static boolean drawItemBorderInControlCurveView = true;
    /// <summary>
    /// コントロールカーブに、データ点を表す四角を描くかどうか（デフォルトはtrue）
    /// </summary>
    public static boolean drawCurveDotInControlCurveView = true;
    /// <summary>
    /// ピアノロール画面に、現在選択中の歌声合成エンジンの種類を描くかどうか
    /// </summary>
    public static boolean drawOverSynthNameOnPianoroll = true;
    /// <summary>
    /// 音符の長さが変更されたとき、ビブラートの長さがどう影響を受けるかを決める因子
    /// </summary>
    public static VibratoLengthEditingRule vibratoLengthEditingRule = VibratoLengthEditingRule.PERCENTAGE;
    /// <summary>
    /// ピアノロール上で右クリックでコンテキストメニューを表示するかどうか
    /// </summary>
    public static boolean showContextMenuWhenRightClickedOnPianoroll = false;
    #endregion // 裏設定項目

    public static BEvent<BEventHandler> gridVisibleChangedEvent = new BEvent<BEventHandler>();
    public static BEvent<BEventHandler> previewStartedEvent = new BEvent<BEventHandler>();
    public static BEvent<BEventHandler> previewAbortedEvent = new BEvent<BEventHandler>();
    public static BEvent<SelectedEventChangedEventHandler> selectedEventChangedEvent = new BEvent<SelectedEventChangedEventHandler>();
    public static BEvent<BEventHandler> selectedToolChangedEvent = new BEvent<BEventHandler>();
    /// <summary>
    /// CurrentClockプロパティの値が変更された時発生します
    /// </summary>
    public static BEvent<BEventHandler> currentClockChangedEvent = new BEvent<BEventHandler>();

    private const String TEMPDIR_NAME = "cadencii";*/

    org.kbinani.cadencii.AppManager.getStartToDrawX = function() {
        return this._startToDrawX;
    };

    org.kbinani.cadencii.AppManager.setStartToDrawX = function( value ) {
        this._startToDrawX = value;
    };

    org.kbinani.cadencii.AppManager.getStartToDrawY = function() {
        return this._startToDrawY;
    };

    org.kbinani.cadencii.AppManager.setStartToDrawY = function( value ) {
        this._startToDrawY = value;
    };

    /// <summary>
    /// 音の高さを表すnoteから、画面に描くべきy座標を計算します
    /// </summary>
    /// <param name="note"></param>
    /// <returns></returns>
    org.kbinani.cadencii.AppManager.yCoordFromNote = function( note, start_to_draw_y ) {
        var stdy = this._startToDrawY;
        if( arguments.length == 2 ){
            stdy = arguments[1];
        }
        return (int)(-1 * (note - 127.0) * this.editorConfig.PxTrackHeight) - stdy;
    };

    /// <summary>
    /// ピアノロール画面のy座標から、その位置における音の高さを取得します
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    org.kbinani.cadencii.AppManager.noteFromYCoord = function( y ) {
        return org.kbinani.PortUtil.castToInt( 127 - this._noteFromYCoordCore( y ) );
    };

    org.kbinani.cadencii.AppManager.noteFromYCoordDoublePrecision = function( y ) {
        return 127.0 - this._noteFromYCoordCore( y );
    };

    org.kbinani.cadencii.AppManager._noteFromYCoordCore = function( y ) {
        return (this._startToDrawY + y) / this.editorConfig.PxTrackHeight;
    };

    /*
    /// <summary>
    /// 指定した音声合成システムを識別する文字列(DSB301, DSB202等)を取得します
    /// </summary>
    /// <param name="kind">音声合成システムの種類</param>
    /// <returns>音声合成システムを識別する文字列(VOCALOID2=DSB301, VOCALOID1[1.0,1.1]=DSB202, AquesTone=AQT000, Straight x UTAU=STR000, UTAU=UTAU000)</returns>
    public static String getVersionStringFromRendererKind( RendererKind kind ) {
        if ( kind == RendererKind.AQUES_TONE ) {
            return "AQT000";
        } else if ( kind == RendererKind.STRAIGHT_UTAU ) {
            return "STR000";
        } else if ( kind == RendererKind.UTAU ) {
            return "UTU000";
        } else if ( kind == RendererKind.VOCALOID1_100 || kind == RendererKind.VOCALOID1_101 ) {
            return "DSB202";
        } else if ( kind == RendererKind.VOCALOID2 ) {
            return "DSB301";
        }
        return "";
    }

    /// <summary>
    /// 指定した音声合成システムが使用する歌手のリストを取得します
    /// </summary>
    /// <param name="kind">音声合成システムの種類</param>
    /// <returns>歌手のリスト</returns>
    public static Vector<VsqID> getSingerListFromRendererKind( RendererKind kind ) {
        Vector<VsqID> singers = null;
        if ( kind == RendererKind.AQUES_TONE ) {
            singers = new Vector<VsqID>();
#if ENABLE_AQUESTONE
            SingerConfig[] list = AquesToneDriver.SINGERS;
            for ( int i = 0; i < list.Length; i++ ) {
                SingerConfig sc = list[i];
                singers.add( getSingerIDAquesTone( sc.Program ) );
            }
#endif
        } else if ( kind == RendererKind.STRAIGHT_UTAU || kind == RendererKind.UTAU ) {
            Vector<SingerConfig> list = editorConfig.UtauSingers;
            singers = new Vector<VsqID>();
            for ( Iterator<SingerConfig> itr = list.iterator(); itr.hasNext(); ) {
                SingerConfig sc = itr.next();
                singers.add( getSingerIDUtau( sc.Language, sc.Program ) );
            }
        } else if ( kind == RendererKind.VOCALOID1_100 || kind == RendererKind.VOCALOID1_101 ) {
            SingerConfig[] configs = VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID1 );
            singers = new Vector<VsqID>();
            for ( int i = 0; i < configs.Length; i++ ) {
                SingerConfig sc = configs[i];
                singers.add( VocaloSysUtil.getSingerID( sc.Language, sc.Program, SynthesizerType.VOCALOID1 ) );
            }
        } else if ( kind == RendererKind.VOCALOID2 ) {
            singers = new Vector<VsqID>();
            SingerConfig[] configs = VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID2 );
            for ( int i = 0; i < configs.Length; i++ ) {
                SingerConfig sc = configs[i];
                singers.add( VocaloSysUtil.getSingerID( sc.Language, sc.Program, SynthesizerType.VOCALOID2 ) );
            }
        }
        return singers;
    }

    /// <summary>
    /// 指定したVSQファイルの，指定したトラック上の，指定したゲートタイム位置に音符イベントがあると仮定して，
    /// 指定した音符イベントの歌詞を指定した値に変更します．Consonant Adjustment，VoiceOverlap，およびPreUtteranceが同時に自動で変更されます．
    /// </summary>
    /// <param name="vsq"></param>
    /// <param name="track"></param>
    /// <param name="item"></param>
    /// <param name="clock"></param>
    /// <param name="new_phrase"></param>
    public static void changePhrase( VsqFileEx vsq, int track, VsqEvent item, int clock, String new_phrase ) {
        String phonetic_symbol = "";
        SymbolTableEntry entry = SymbolTable.attatch( new_phrase );
        if ( entry == null ) {
            phonetic_symbol = "a";
        } else {
            phonetic_symbol = entry.getSymbol();
        }
        String str_phonetic_symbol = phonetic_symbol;

        // consonant adjustment
        String[] spl = PortUtil.splitString( str_phonetic_symbol, new char[] { ' ', ',' }, true );
        String consonant_adjustment = "";
        for ( int i = 0; i < spl.Length; i++ ) {
            consonant_adjustment += (i == 0 ? "" : " ") + (VsqPhoneticSymbol.isConsonant( spl[i] ) ? 64 : 0);
        }

        // overlap, preUtterancec
        if ( vsq != null ) {
            VsqTrack vsq_track = vsq.Track.get( track );
            VsqEvent singer = vsq_track.getSingerEventAt( clock );
            SingerConfig sc = getSingerInfoUtau( singer.ID.IconHandle.Language, singer.ID.IconHandle.Program );
            if ( sc != null && utauVoiceDB.containsKey( sc.VOICEIDSTR ) ) {
                UtauVoiceDB db = utauVoiceDB.get( sc.VOICEIDSTR );
                OtoArgs oa = db.attachFileNameFromLyric( new_phrase );
                if ( item.UstEvent == null ){
                    item.UstEvent = new UstEvent();
                }
                item.UstEvent.VoiceOverlap = oa.msOverlap;
                item.UstEvent.PreUtterance = oa.msPreUtterance;
            }
        }
    }

    public static void serializeRenderingStatus( String temppath, int track ) {
        FileOutputStream fs = null;
        try {
            fs = new FileOutputStream( PortUtil.combinePath( temppath, track + ".xml" ) );
            renderingStatusSerializer.serialize( fs, lastRenderedStatus[track - 1] );
        } catch ( Exception ex ) {
            PortUtil.stderr.println( "FormMain#patchWorkToFreeze; ex=" + ex );
        } finally {
            if ( fs != null ) {
                try {
                    fs.close();
                } catch ( Exception ex2 ) {
                    PortUtil.stderr.println( "FormMain#patchWorkToFreeze; ex2=" + ex2 );
                }
            }
        }
    }

    public static void reportError( Exception ex, String message, int level ) {
        PortUtil.stderr.println( message + "; ex=" + ex );
        if ( level < 0 ) {
            FormCompileResult dialog = null;
            try {
#if JAVA
                dialog = new FormCompileResult( message, ex.toString() );
#else
                dialog = new FormCompileResult( message, "Message:\r\n" + ex.Message + "\r\n\r\nStackTrace:\r\n" + ex.StackTrace );
#endif
                beginShowDialog();
                dialog.setModal( true );
                dialog.setVisible( true );
                endShowDialog();
            } catch ( Exception ex2 ) {
            } finally {
                if ( dialog != null ) {
                    try {
                        dialog.close();
                    } catch ( Exception ex3 ) {
                    }
                }
            }
        }
    }*/

    /**
     * クロック数から、画面に描くべきx座標の値を取得します。
     * @param clocks [double]
     * @return [int]
     */
    org.kbinani.cadencii.AppManager.xCoordFromClocks = function( clocks ) {
        return org.kbinani.PortUtil.castToInt( this.keyWidth + clocks * this.scaleX - this._startToDrawX ) + this.keyOffset;
    };

    /**
     * 画面のx座標からクロック数を取得します
     * @param x [int]
     * @return [int]
     */
    org.kbinani.cadencii.AppManager.clockFromXCoord = function( x ) {
        return org.kbinani.PortUtil.castToInt( (x + this._startToDrawX - this.keyOffset - this.keyWidth) / this.scaleX );
    };

    /*#region 選択範囲の管理
    public static boolean isWholeSelectedIntervalEnabled() {
        return wholeSelectedIntervalEnabled;
    }

    public static boolean isCurveSelectedIntervalEnabled() {
        return curveSelectedIntervalEnabled;
    }

    public static void setWholeSelectedIntervalEnabled( boolean value ) {
        wholeSelectedIntervalEnabled = value;
        if ( value ) {
            curveSelectedIntervalEnabled = false;
        }
    }

    public static void setCurveSelectedIntervalEnabled( boolean value ) {
        curveSelectedIntervalEnabled = value;
        if ( value ) {
            wholeSelectedIntervalEnabled = false;
        }
    }
    #endregion

    #region MessageBoxのラッパー
    public static BDialogResult showMessageBox( String text ) {
        return showMessageBox( text, "", org.kbinani.windows.forms.Utility.MSGBOX_DEFAULT_OPTION, org.kbinani.windows.forms.Utility.MSGBOX_PLAIN_MESSAGE );
    }

    public static BDialogResult showMessageBox( String text, String caption ) {
        return showMessageBox( text, caption, org.kbinani.windows.forms.Utility.MSGBOX_DEFAULT_OPTION, org.kbinani.windows.forms.Utility.MSGBOX_PLAIN_MESSAGE );
    }

    public static BDialogResult showMessageBox( String text, String caption, int optionType ) {
        return showMessageBox( text, caption, optionType, org.kbinani.windows.forms.Utility.MSGBOX_PLAIN_MESSAGE );
    }

    public static void beginShowDialog() {
#if ENABLE_PROPERTY
        boolean property = (propertyWindow != null) ? propertyWindow.isAlwaysOnTop() : false;
        if ( property ) {
            propertyWindow.setAlwaysOnTop( false );
        }
#endif
        boolean mixer = (mixerWindow != null) ? mixerWindow.isAlwaysOnTop() : false;
        if ( mixer ) {
            mixerWindow.setAlwaysOnTop( false );
        }
    }

    public static void endShowDialog() {
#if ENABLE_PROPERTY
        boolean property = (propertyWindow != null) ? propertyWindow.isAlwaysOnTop() : false;
        if ( property ) {
            propertyWindow.setAlwaysOnTop( true );
        }
#endif
        boolean mixer = (mixerWindow != null) ? mixerWindow.isAlwaysOnTop() : false;
        if ( mixer ) {
            mixerWindow.setAlwaysOnTop( true );
        }
        if ( mainWindow != null ) {
            mainWindow.requestFocus();
        }
    }
    
    public static BDialogResult showMessageBox( String text, String caption, int optionType, int messageType ) {
        beginShowDialog();
        BDialogResult ret = org.kbinani.windows.forms.Utility.showMessageBox( text, caption, optionType, messageType );
        endShowDialog();
        return ret;
    }
    #endregion

    #region BGM 関連
    public static int getBgmCount() {
        if ( _s_vsq == null ) {
            return 0;
        } else {
            return _s_vsq.BgmFiles.size();
        }
    }

    public static BgmFile getBgm( int index ) {
        if ( _s_vsq == null ) {
            return null;
        }
        return _s_vsq.BgmFiles.get( index );
    }

    public static void removeBgm( int index ) {
        if ( _s_vsq == null ) {
            return;
        }
        Vector<BgmFile> list = new Vector<BgmFile>();
        int count = _s_vsq.BgmFiles.size();
        for ( int i = 0; i < count; i++ ) {
            if ( i != index ) {
                list.add( (BgmFile)_s_vsq.BgmFiles.get( i ).clone() );
            }
        }
        CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate( list );
        register( _s_vsq.executeCommand( run ) );
        mainWindow.setEdited( true );
        mixerWindow.updateStatus();
    }

    public static void clearBgm() {
        if ( _s_vsq == null ) {
            return;
        }
        Vector<BgmFile> list = new Vector<BgmFile>();
        CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate( list );
        register( _s_vsq.executeCommand( run ) );
        mainWindow.setEdited( true );
        mixerWindow.updateStatus();
    }

    public static void addBgm( String file ) {
        if ( _s_vsq == null ) {
            return;
        }
        Vector<BgmFile> list = new Vector<BgmFile>();
        int count = _s_vsq.BgmFiles.size();
        for ( int i = 0; i < count; i++ ) {
            list.add( (BgmFile)_s_vsq.BgmFiles.get( i ).clone() );
        }
        BgmFile item = new BgmFile();
        item.file = file;
        item.feder = 0;
        item.panpot = 0;
        list.add( item );
        CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate( list );
        register( _s_vsq.executeCommand( run ) );
        mainWindow.setEdited( true );
        mixerWindow.updateStatus();
    }
    #endregion

    #region 音量の取得
    /// <summary>
    /// 第track番目のトラックの，現在の音量を取得します
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    public static AmplifyCoefficient getAmplifyCoeffNormalTrack( int track ) {
        AmplifyCoefficient ret = new AmplifyCoefficient();
        ret.left = 1.0;
        ret.right = 1.0;
        if ( _s_vsq != null && 1 <= track && track < _s_vsq.Track.size() ) {
            double amp_master = VocaloSysUtil.getAmplifyCoeffFromFeder( _s_vsq.Mixer.MasterFeder );
            double pan_left_master = VocaloSysUtil.getAmplifyCoeffFromPanLeft( _s_vsq.Mixer.MasterPanpot );
            double pan_right_master = VocaloSysUtil.getAmplifyCoeffFromPanRight( _s_vsq.Mixer.MasterPanpot );
            double amp_track = VocaloSysUtil.getAmplifyCoeffFromFeder( _s_vsq.Mixer.Slave.get( track - 1 ).Feder );
            double pan_left_track = VocaloSysUtil.getAmplifyCoeffFromPanLeft( _s_vsq.Mixer.Slave.get( track - 1 ).Panpot );
            double pan_right_track = VocaloSysUtil.getAmplifyCoeffFromPanRight( _s_vsq.Mixer.Slave.get( track - 1 ).Panpot );
            ret.left = amp_master * amp_track * pan_left_master * pan_left_track;
            ret.right = amp_master * amp_track * pan_right_master * pan_right_track;
        }
        return ret;
    }

    public static AmplifyCoefficient getAmplifyCoeffBgm( int index ) {
        AmplifyCoefficient ret = new AmplifyCoefficient();
        ret.left = 1.0;
        ret.right = 1.0;
        if ( _s_vsq != null && 0 <= index && index < _s_vsq.BgmFiles.size() ) {
            BgmFile item = _s_vsq.BgmFiles.get( index );
            if ( item.mute == 1 ) {
                ret.left = 0.0;
                ret.right = 0.0;
            } else {
                double amp_master = VocaloSysUtil.getAmplifyCoeffFromFeder( _s_vsq.Mixer.MasterFeder );
                double pan_left_master = VocaloSysUtil.getAmplifyCoeffFromPanLeft( _s_vsq.Mixer.MasterPanpot );
                double pan_right_master = VocaloSysUtil.getAmplifyCoeffFromPanRight( _s_vsq.Mixer.MasterPanpot );
                double amp_track = VocaloSysUtil.getAmplifyCoeffFromFeder( item.feder );
                double pan_left_track = VocaloSysUtil.getAmplifyCoeffFromPanLeft( item.panpot );
                double pan_right_track = VocaloSysUtil.getAmplifyCoeffFromPanRight( item.panpot );
                ret.left = amp_master * amp_track * pan_left_master * pan_left_track;
                ret.right = amp_master * amp_track * pan_right_master * pan_right_track;
            }
        }
        return ret;
    }
    #endregion

    #region 自動保存
    public static void updateAutoBackupTimerStatus() {
        if ( !_s_file.Equals( "" ) && editorConfig.AutoBackupIntervalMinutes > 0 ) {
            double millisec = editorConfig.AutoBackupIntervalMinutes * 60.0 * 1000.0;
            int draft = (int)millisec;
            if ( millisec > int.MaxValue ) {
                draft = int.MaxValue;
            }
            s_auto_backup_timer.setDelay( draft );
            s_auto_backup_timer.start();
        } else {
            s_auto_backup_timer.stop();
        }
    }

    private static void handleAutoBackupTimerTick( Object sender, BEventArgs e ) {
        if ( !_s_file.Equals( "" ) && PortUtil.isFileExists( _s_file ) ) {
            String path = PortUtil.getDirectoryName( _s_file );
            String backup = PortUtil.combinePath( path, "~$" + PortUtil.getFileName( _s_file ) );
            String file2 = PortUtil.combinePath( path, PortUtil.getFileNameWithoutExtension( backup ) + ".vsq" );
            if ( PortUtil.isFileExists( backup ) ) {
                try {
                    PortUtil.deleteFile( backup );
                } catch ( Exception ex ) {
                }
            }
            if ( PortUtil.isFileExists( file2 ) ) {
                try {
                    PortUtil.deleteFile( file2 );
                } catch ( Exception ex ) {
                }
            }
            saveToCor( backup );
        }
    }
    #endregion

    public static void debugWriteLine( String message ) {
#if DEBUG
        PortUtil.println( message );
#endif
    }

    /// <summary>
    /// FormMainを識別するID
    /// </summary>
    public static String getID() {
        return s_id;
    }

    public static String _( String id ) {
        return Messaging.getMessage( id );
    }

    /// <summary>
    /// 音声ファイルのキャッシュディレクトリのパスを設定します。
    /// このメソッドでは、キャッシュディレクトリの変更に伴う他の処理は実行されません。
    /// </summary>
    /// <param name="value"></param>
    public static void setTempWaveDir( String value ) {
#if DEBUG
        PortUtil.println( "AppManager#setTempWaveDir; before: \"" + tempWaveDir + "\"" );
        PortUtil.println( "                           after:  \"" + value + "\"" );
#endif
        tempWaveDir = value;
    }

    /// <summary>
    /// 音声ファイルのキャッシュディレクトリのパスを取得します。
    /// </summary>
    /// <returns></returns>
    public static String getTempWaveDir() {
        return tempWaveDir;
    }

    /// <summary>
    /// Cadenciiが使用する一時ディレクトリのパスを取得します。
    /// </summary>
    /// <returns></returns>
    public static String getCadenciiTempDir() {
        String temp = PortUtil.combinePath( PortUtil.getTempPath(), TEMPDIR_NAME );
        if ( !PortUtil.isDirectoryExists( temp ) ) {
            PortUtil.createDirectory( temp );
        }
        return temp;
    }

    /// <summary>
    /// ベジエ曲線を編集するモードかどうかを取得します。
    /// </summary>
    /// <returns></returns>
    public static boolean isCurveMode() {
        return s_is_curve_mode;
    }

    /// <summary>
    /// ベジエ曲線を編集するモードかどうかを設定します。
    /// </summary>
    /// <param name="value"></param>
    public static void setCurveMode( boolean value ) {
        boolean old = s_is_curve_mode;
        s_is_curve_mode = value;
        if ( old != s_is_curve_mode ) {
            try {
                selectedToolChangedEvent.raise( typeof( AppManager ), new BEventArgs() );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "AppManager#setCurveMode; ex=" + ex );
            }
        }
    }

#if !TREECOM
    /// <summary>
    /// アンドゥ・リドゥ用のコマンド履歴を削除します。
    /// </summary>
    public static void clearCommandBuffer() {
        s_commands.clear();
        s_command_position = -1;
    }

    /// <summary>
    /// アンドゥ処理を行います。
    /// </summary>
    public static void undo() {
        if ( isUndoAvailable() ) {
            Vector<ValuePair<Integer, Integer>> before_ids = new Vector<ValuePair<Integer, Integer>>();
            for ( Iterator<SelectedEventEntry> itr = getSelectedEventIterator(); itr.hasNext(); ) {
                SelectedEventEntry item = itr.next();
                before_ids.add( new ValuePair<Integer, Integer>( item.track, item.original.InternalID ) );
            }

            ICommand run_src = s_commands.get( s_command_position );
            CadenciiCommand run = (CadenciiCommand)run_src;
            if ( run.vsqCommand != null ) {
                if ( run.vsqCommand.Type == VsqCommandType.TRACK_DELETE ) {
                    int track = (Integer)run.vsqCommand.Args[0];
                    if ( track == getSelected() && track >= 2 ) {
                        setSelected( track - 1 );
                    }
                }
            }
            ICommand inv = _s_vsq.executeCommand( run );
            if ( run.type == CadenciiCommandType.BGM_UPDATE ) {
                if ( mainWindow != null ) {
                    mainWindow.updateBgmMenuState();
                }
            }
            s_commands.set( s_command_position, inv );
            s_command_position--;

            cleanupDeadSelection( before_ids );
            updateSelectedEventInstance();
        }
    }

    /// <summary>
    /// リドゥ処理を行います。
    /// </summary>
    public static void redo() {
        if ( isRedoAvailable() ) {
            Vector<ValuePair<Integer, Integer>> before_ids = new Vector<ValuePair<Integer, Integer>>();
            for ( Iterator<SelectedEventEntry> itr = getSelectedEventIterator(); itr.hasNext(); ) {
                SelectedEventEntry item = itr.next();
                before_ids.add( new ValuePair<Integer, Integer>( item.track, item.original.InternalID ) );
            }

            ICommand run_src = s_commands.get( s_command_position + 1 );
            CadenciiCommand run = (CadenciiCommand)run_src;
            if ( run.vsqCommand != null ) {
                if ( run.vsqCommand.Type == VsqCommandType.TRACK_DELETE ) {
                    int track = (Integer)run.args[0];
                    if ( track == getSelected() && track >= 2 ) {
                        setSelected( track - 1 );
                    }
                }
            }
            ICommand inv = _s_vsq.executeCommand( run );
            if ( run.type == CadenciiCommandType.BGM_UPDATE ) {
                if ( mainWindow != null ) {
                    mainWindow.updateBgmMenuState();
                }
            }
            s_commands.set( s_command_position + 1, inv );
            s_command_position++;

            cleanupDeadSelection( before_ids );
            updateSelectedEventInstance();
        }
    }

    /// <summary>
    /// 選択中のアイテムが編集された場合、編集にあわせてオブジェクトを更新する。
    /// </summary>
    public static void updateSelectedEventInstance() {
        if ( _s_vsq == null ) {
            return;
        }
        VsqTrack vsq_track = _s_vsq.Track.get( _s_selected );

        for ( Iterator<SelectedEventEntry> itr = getSelectedEventIterator(); itr.hasNext(); ) {
            SelectedEventEntry item = itr.next();
            int internal_id = item.original.InternalID;
            item.original = vsq_track.findEventFromID( internal_id );
        }
    }

    /// <summary>
    /// 「選択されている」と登録されているオブジェクトのうち、Undo, Redoなどによって存在しなくなったものを登録解除する
    /// </summary>
    public static void cleanupDeadSelection( Vector<ValuePair<Integer, Integer>> before_ids ) {
        for ( Iterator<ValuePair<Integer, Integer>> itr = before_ids.iterator(); itr.hasNext(); ) {
            ValuePair<Integer, Integer> specif = itr.next();
            boolean found = false;
            for ( Iterator<VsqEvent> itr2 = _s_vsq.Track.get( specif.getKey() ).getNoteEventIterator(); itr2.hasNext(); ) {
                VsqEvent item = itr2.next();
                if ( item.InternalID == specif.getValue() ) {
                    found = true;
                    break;
                }
            }
            if ( !found ) {
                AppManager.removeSelectedEvent( specif.getValue() );
            }
        }
    }

    /// <summary>
    /// アンドゥ・リドゥ用のコマンドバッファに、指定されたコマンドを登録します。
    /// </summary>
    /// <param name="command"></param>
    public static void register( ICommand command ) {
        if ( s_command_position == s_commands.size() - 1 ) {
            // 新しいコマンドバッファを追加する場合
            s_commands.add( command );
            s_command_position = s_commands.size() - 1;
        } else {
            // 既にあるコマンドバッファを上書きする場合
            s_commands.set( s_command_position + 1, command );
            for ( int i = s_commands.size() - 1; i >= s_command_position + 2; i-- ) {
                s_commands.removeElementAt( i );
            }
            s_command_position++;
        }
    }

    /// <summary>
    /// リドゥ操作が可能かどうかを取得します。
    /// </summary>
    /// <returns>リドゥ操作が可能ならtrue、そうでなければfalseを返します。</returns>
    public static boolean isRedoAvailable() {
        if ( s_commands.size() > 0 && 0 <= s_command_position + 1 && s_command_position + 1 < s_commands.size() ) {
            return true;
        } else {
            return false;
        }
    }

    /// <summary>
    /// アンドゥ操作が可能かどうかを取得します。
    /// </summary>
    /// <returns>アンドゥ操作が可能ならtrue、そうでなければfalseを返します。</returns>
    public static boolean isUndoAvailable() {
        if ( s_commands.size() > 0 && 0 <= s_command_position && s_command_position < s_commands.size() ) {
            return true;
        } else {
            return false;
        }
    }
#endif

    /// <summary>
    /// 現在選択されているツールを取得します。
    /// </summary>
    /// <returns></returns>
    public static EditTool getSelectedTool() {
        return _s_selected_tool;
    }

    /// <summary>
    /// 現在選択されているツールを設定します。
    /// </summary>
    /// <param name="value"></param>
    public static void setSelectedTool( EditTool value ) {
        EditTool old = _s_selected_tool;
        _s_selected_tool = value;
        if ( old != _s_selected_tool ) {
            try {
                selectedToolChangedEvent.raise( typeof( AppManager ), new EventArgs() );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "AppManager#setSelectedTool; ex=" + ex );
            }
        }
    }

    #region SelectedBezier
    /// <summary>
    /// 選択されているベジエ曲線のデータ点を順に返す反復子を取得します。
    /// </summary>
    /// <returns></returns>
    public static Iterator<SelectedBezierPoint> getSelectedBezierIterator() {
        return _s_selected_bezier.iterator();
    }

    /// <summary>
    /// 最後に選択状態となったベジエ曲線のデータ点を取得します。
    /// </summary>
    /// <returns>最後に選択状態となったベジエ曲線のデータ点を返します。選択状態となっているベジエ曲線がなければnullを返します。</returns>
    public static SelectedBezierPoint getLastSelectedBezier() {
        if ( s_last_selected_bezier.chainID < 0 || s_last_selected_bezier.pointID < 0 ) {
            return null;
        } else {
            return s_last_selected_bezier;
        }
    }

    /// <summary>
    /// 指定されたベジエ曲線のデータ点を選択状態にします。
    /// </summary>
    /// <param name="selected">選択状態にするデータ点。</param>
    public static void addSelectedBezier( SelectedBezierPoint selected ) {
        s_last_selected_bezier = selected;
        int index = -1;
        for ( int i = 0; i < _s_selected_bezier.size(); i++ ) {
            if ( _s_selected_bezier.get( i ).chainID == selected.chainID &&
                _s_selected_bezier.get( i ).pointID == selected.pointID ) {
                index = i;
                break;
            }
        }
        if ( index >= 0 ) {
            _s_selected_bezier.set( index, selected );
        } else {
            _s_selected_bezier.add( selected );
        }
        checkSelectedItemExistence();
    }

    /// <summary>
    /// すべてのベジエ曲線のデータ点の選択状態を解除します。
    /// </summary>
    public static void clearSelectedBezier() {
        _s_selected_bezier.clear();
        s_last_selected_bezier.chainID = -1;
        s_last_selected_bezier.pointID = -1;
        checkSelectedItemExistence();
    }
    #endregion

    #region SelectedTimesig
    /// <summary>
    /// 最後に選択状態となった拍子変更設定を取得します。
    /// </summary>
    /// <returns>最後に選択状態となった拍子変更設定を返します。選択状態となっている拍子変更設定が無ければnullを返します。</returns>
    public static SelectedTimesigEntry getLastSelectedTimesig() {
        if ( _s_selected_timesig.containsKey( s_last_selected_timesig ) ) {
            return _s_selected_timesig.get( s_last_selected_timesig );
        } else {
            return null;
        }
    }

    public static int getLastSelectedTimesigBarcount() {
        return s_last_selected_timesig;
    }

    public static void addSelectedTimesig( int barcount ) {
        clearSelectedEvent(); //ここ注意！
        clearSelectedTempo();
        s_last_selected_timesig = barcount;
        if ( !_s_selected_timesig.containsKey( barcount ) ) {
            for ( Iterator<TimeSigTableEntry> itr = _s_vsq.TimesigTable.iterator(); itr.hasNext(); ) {
                TimeSigTableEntry tte = itr.next();
                if ( tte.BarCount == barcount ) {
                    _s_selected_timesig.put( barcount, new SelectedTimesigEntry( tte, (TimeSigTableEntry)tte.clone() ) );
                    break;
                }
            }
        }
        checkSelectedItemExistence();
    }

    public static void clearSelectedTimesig() {
        _s_selected_timesig.clear();
        s_last_selected_timesig = -1;
        checkSelectedItemExistence();
    }

    public static int getSelectedTimesigCount() {
        return _s_selected_timesig.size();
    }

    public static Iterator<ValuePair<Integer, SelectedTimesigEntry>> getSelectedTimesigIterator() {
        Vector<ValuePair<Integer, SelectedTimesigEntry>> list = new Vector<ValuePair<Integer, SelectedTimesigEntry>>();
        for ( Iterator<Integer> itr = _s_selected_timesig.keySet().iterator(); itr.hasNext(); ) {
            int clock = itr.next();
            list.add( new ValuePair<Integer, SelectedTimesigEntry>( clock, _s_selected_timesig.get( clock ) ) );
        }
        return list.iterator();
    }

    public static boolean isSelectedTimesigContains( int barcount ) {
        return _s_selected_timesig.containsKey( barcount );
    }

    public static SelectedTimesigEntry getSelectedTimesig( int barcount ) {
        if ( _s_selected_timesig.containsKey( barcount ) ) {
            return _s_selected_timesig.get( barcount );
        } else {
            return null;
        }
    }

    public static void removeSelectedTimesig( int barcount ) {
        if ( _s_selected_timesig.containsKey( barcount ) ) {
            _s_selected_timesig.remove( barcount );
            checkSelectedItemExistence();
        }
    }
    #endregion

    #region SelectedTempo
    public static SelectedTempoEntry getLastSelectedTempo() {
        if ( _s_selected_tempo.containsKey( s_last_selected_tempo ) ) {
            return _s_selected_tempo.get( s_last_selected_tempo );
        } else {
            return null;
        }
    }

    public static int getLastSelectedTempoClock() {
        return s_last_selected_tempo;
    }

    public static void addSelectedTempo( int clock ) {
        clearSelectedEvent(); //ここ注意！
        clearSelectedTimesig();
        s_last_selected_tempo = clock;
        if ( !_s_selected_tempo.containsKey( clock ) ) {
            for ( Iterator<TempoTableEntry> itr = _s_vsq.TempoTable.iterator(); itr.hasNext(); ) {
                TempoTableEntry tte = itr.next();
                if ( tte.Clock == clock ) {
                    _s_selected_tempo.put( clock, new SelectedTempoEntry( tte, (TempoTableEntry)tte.clone() ) );
                    break;
                }
            }
        }
        checkSelectedItemExistence();
    }

    public static void clearSelectedTempo() {
        _s_selected_tempo.clear();
        s_last_selected_tempo = -1;
        checkSelectedItemExistence();
    }

    public static int getSelectedTempoCount() {
        return _s_selected_tempo.size();
    }

    public static Iterator<ValuePair<Integer, SelectedTempoEntry>> getSelectedTempoIterator() {
        Vector<ValuePair<Integer, SelectedTempoEntry>> list = new Vector<ValuePair<Integer, SelectedTempoEntry>>();
        for ( Iterator<Integer> itr = _s_selected_tempo.keySet().iterator(); itr.hasNext(); ) {
            int clock = itr.next();
            list.add( new ValuePair<Integer, SelectedTempoEntry>( clock, _s_selected_tempo.get( clock ) ) );
        }
        return list.iterator();
    }

    public static boolean isSelectedTempoContains( int clock ) {
        return _s_selected_tempo.containsKey( clock );
    }

    public static SelectedTempoEntry getSelectedTempo( int clock ) {
        if ( _s_selected_tempo.containsKey( clock ) ) {
            return _s_selected_tempo.get( clock );
        } else {
            return null;
        }
    }

    public static void removeSelectedTempo( int clock ) {
        if ( _s_selected_tempo.containsKey( clock ) ) {
            _s_selected_tempo.remove( clock );
            checkSelectedItemExistence();
        }
    }
    #endregion

    #region SelectedEvent
    public static void removeSelectedEvent( int id ) {
        removeSelectedEventCor( id, false );
        checkSelectedItemExistence();
    }

    public static void removeSelectedEventSilent( int id ) {
        removeSelectedEventCor( id, true );
        checkSelectedItemExistence();
    }

    private static void removeSelectedEventCor( int id, boolean silent ) {
        int count = _s_selected_events.size();
        for ( int i = 0; i < count; i++ ) {
            if ( _s_selected_events.get( i ).original.InternalID == id ) {
                _s_selected_events.removeElementAt( i );
                break;
            }
        }
        if ( !silent ) {
#if ENABLE_PROPERTY
            propertyPanel.UpdateValue( _s_selected );
#endif
        }
    }

    public static void removeSelectedEventRange( int[] ids ) {
        Vector<Integer> v_ids = new Vector<Integer>( Arrays.asList( PortUtil.convertIntArray( ids ) ) );
        Vector<Integer> index = new Vector<Integer>();
        int count = _s_selected_events.size();
        for ( int i = 0; i < count; i++ ) {
            if ( v_ids.contains( _s_selected_events.get( i ).original.InternalID ) ) {
                index.add( i );
                if ( index.size() == ids.Length ) {
                    break;
                }
            }
        }
        count = index.size();
        for ( int i = count - 1; i >= 0; i-- ) {
            _s_selected_events.removeElementAt( i );
        }
#if ENABLE_PROPERTY
        propertyPanel.UpdateValue( _s_selected );
#endif
        checkSelectedItemExistence();
    }

    public static void addSelectedEventAll( Vector<Integer> list ) {
        clearSelectedTempo();
        clearSelectedTimesig();
        VsqEvent[] index = new VsqEvent[list.size()];
        int count = 0;
        int c = list.size();
        for ( Iterator<VsqEvent> itr = _s_vsq.Track.get( _s_selected ).getEventIterator(); itr.hasNext(); ) {
            VsqEvent ev = itr.next();
            int find = -1;
            for ( int i = 0; i < c; i++ ) {
                if ( list.get( i ) == ev.InternalID ) {
                    find = i;
                    break;
                }
            }
            if ( 0 <= find ) {
                index[find] = ev;
                count++;
            }
            if ( count == list.size() ) {
                break;
            }
        }
        for ( int i = 0; i < index.Length; i++ ) {
            if ( !isSelectedEventContains( _s_selected, index[i].InternalID ) ) {
                _s_selected_events.add( new SelectedEventEntry( _s_selected, index[i], (VsqEvent)index[i].clone() ) );
            }
        }
#if ENABLE_PROPERTY
        propertyPanel.UpdateValue( _s_selected );
#endif
        checkSelectedItemExistence();
    }

    public static void addSelectedEvent( int id ) {
        addSelectedEventCor( id, false );
        checkSelectedItemExistence();
    }

    public static void addSelectedEventSilent( int id ) {
        addSelectedEventCor( id, true );
        checkSelectedItemExistence();
    }

    private static void addSelectedEventCor( int id, boolean silent ) {
        clearSelectedTempo();
        clearSelectedTimesig();
        for ( Iterator<VsqEvent> itr = _s_vsq.Track.get( _s_selected ).getEventIterator(); itr.hasNext(); ) {
            VsqEvent ev = itr.next();
            if ( ev.InternalID == id ) {
                if ( isSelectedEventContains( _s_selected, id ) ) {
                    // すでに選択されていた場合
                    int count = _s_selected_events.size();
                    for ( int i = 0; i < count; i++ ) {
                        SelectedEventEntry item = _s_selected_events.get( i );
                        if ( item.original.InternalID == id ) {
                            _s_selected_events.removeElementAt( i );
                            break;
                        }
                    }
                }

                _s_selected_events.add( new SelectedEventEntry( _s_selected, ev, (VsqEvent)ev.clone() ) );
                if ( !silent ) {
                    try {
                        selectedEventChangedEvent.raise( typeof( AppManager ), false );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "AppManager#addSelectedEventCor; ex=" + ex );
                    }
                }
                break;
            }
        }
        if ( !silent ) {
#if ENABLE_PROPERTY
            propertyPanel.UpdateValue( _s_selected );
#endif
        }
    }

    public static void clearSelectedEvent() {
        _s_selected_events.clear();
#if ENABLE_PROPERTY
        propertyPanel.UpdateValue( _s_selected );
#endif
        checkSelectedItemExistence();
    }

    public static boolean isSelectedEventContains( int track, int id ) {
        int count = _s_selected_events.size();
        for ( int i = 0; i < count; i++ ) {
            SelectedEventEntry item = _s_selected_events.get( i );
            if ( item.original.InternalID == id && item.track == track ) {
                return true;
            }
        }
        return false;
    }

    public static Iterator<SelectedEventEntry> getSelectedEventIterator() {
        return _s_selected_events.iterator();
    }

    public static SelectedEventEntry getLastSelectedEvent() {
        if ( _s_selected_events.size() <= 0 ) {
            return null;
        } else {
            return _s_selected_events.get( _s_selected_events.size() - 1 );
        }
    }*/

    /**
     * @return [int]
     */
    org.kbinani.cadencii.AppManager.getSelectedEventCount = function() {
        return this._s_selected_events.length;
    }

    /*
    #region SelectedPoint
    public static void clearSelectedPoint() {
        selectedPointIDs.clear();
        selectedPointCurveType = CurveType.Empty;
        checkSelectedItemExistence();
    }

    public static void addSelectedPoint( CurveType curve, long id ) {
        addSelectedPointAll( curve, new long[] { id } );
        checkSelectedItemExistence();
    }

    public static void addSelectedPointAll( CurveType curve, long[] ids ) {
        if ( !curve.equals( selectedPointCurveType ) ) {
            selectedPointIDs.clear();
            selectedPointCurveType = curve;
        }
        for ( int i = 0; i < ids.Length; i++ ) {
            if ( !selectedPointIDs.contains( ids[i] ) ) {
                selectedPointIDs.add( ids[i] );
            }
        }
        checkSelectedItemExistence();
    }

    public static boolean isSelectedPointContains( long id ) {
        return selectedPointIDs.contains( id );
    }

    public static CurveType getSelectedPointCurveType() {
        return selectedPointCurveType;
    }

    public static Iterator<Long> getSelectedPointIDIterator() {
        return selectedPointIDs.iterator();
    }

    public static int getSelectedPointIDCount() {
        return selectedPointIDs.size();
    }

    public static void removeSelectedPoint( long id ) {
        selectedPointIDs.removeElement( id );
        checkSelectedItemExistence();
    }
    #endregion

    /// <summary>
    /// 現在選択されたアイテムが存在するかどうかを調べ，必要であればSelectedEventChangedイベントを発生させます
    /// </summary>
    private static void checkSelectedItemExistence() {
        boolean ret = _s_selected_bezier.size() == 0 &&
                      _s_selected_events.size() == 0 &&
                      _s_selected_tempo.size() == 0 &&
                      _s_selected_timesig.size() == 0 &&
                      selectedPointIDs.size() == 0;
        try {
            selectedEventChangedEvent.raise( typeof( AppManager ), ret );
        } catch ( Exception ex ) {
            PortUtil.stderr.println( "AppManager#checkSelectedItemExistence; ex=" + ex );
        }
    }

    public static boolean isOverlay() {
        return _s_overlay;
    }

    public static void setOverlay( boolean value ) {
        _s_overlay = value;
    }

    public static boolean getRenderRequired( int track ) {
        if ( _s_vsq == null ) {
            return false;
        }
        return _s_vsq.editorStatus.renderRequired[track - 1];
    }

    public static void setRenderRequired( int track, boolean value ) {
        if ( _s_vsq == null ) {
            return;
        }
        _s_vsq.editorStatus.renderRequired[track - 1] = value;
    }

    /// <summary>
    /// 現在の編集モード
    /// </summary>
    public static EditMode getEditMode() {
        return s_edit_mode;
    }

    public static void setEditMode( EditMode value ) {
        s_edit_mode = value;
    }*/

    /**
     * グリッドを表示するか否かを表すフラグを取得または設定します
     */
    org.kbinani.cadencii.AppManager.isGridVisible = function() {
        return this._s_grid_visible;
    };

    org.kbinani.cadencii.AppManager.setGridVisible = function( value ) {
        if ( value != this._s_grid_visible ) {
            this._s_grid_visible = value;
            /*try {
                gridVisibleChangedEvent.raise( typeof( AppManager ), new BEventArgs() );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "AppManager#setGridVisible; ex=" + ex );
            }*/
        }
    };

    /**
     * 現在のプレビューがリピートモードであるかどうかを表す値を取得または設定します
     */
    org.kbinani.cadencii.AppManager.isRepeatMode = function() {
        return this._s_repeat_mode;
    };

    org.kbinani.cadencii.AppManager.setRepeatMode = function( value ) {
        this._s_repeat_mode = value;
    };

    /// <summary>
    /// 現在プレビュー中かどうかを示す値を取得または設定します
    /// </summary>
    org.kbinani.cadencii.AppManager.isPlaying = function() {
        return this._s_playing;
    };

    /*public static void setPlaying( boolean value ) {
        lock ( playingPropertyLocker ) {
#if DEBUG
            DateTime time = DateTime.Now;
            PortUtil.println( "AppManager#setPlaying; entry; now=" + time + "; _s_playing=" + _s_playing + "; value=" + value );
#endif
            boolean previous = _s_playing;
            _s_playing = value;
            if ( previous != _s_playing ) {
                if ( _s_playing ) {
                    try {
                        previewStartedEvent.raise( typeof( AppManager ), new BEventArgs() );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "AppManager#setPlaying; ex=" + ex );
                    }
                } else if ( !_s_playing ) {
                    try {
                        previewAbortedEvent.raise( typeof( AppManager ), new BEventArgs() );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "AppManager#setPlaying; ex=" + ex );
                    }
                }
            }
#if DEBUG
            PortUtil.println( "AppManager#setPlaying; done; now=" + time );
#endif
        }
    }

    /// <summary>
    /// _vsq_fileにセットされたvsqファイルの名前を取得します。
    /// </summary>
    public static String getFileName() {
        return _s_file;
    }

    private static void saveToCor( String file ) {
        boolean hide = false;
        if ( _s_vsq != null ) {
            String path = PortUtil.getDirectoryName( file );
            String file2 = PortUtil.combinePath( path, PortUtil.getFileNameWithoutExtension( file ) + ".vsq" );
            _s_vsq.writeAsXml( file );
            _s_vsq.write( file2 );
#if !JAVA
            if ( hide ) {
                try {
                    System.IO.File.SetAttributes( file, System.IO.FileAttributes.Hidden );
                    System.IO.File.SetAttributes( file2, System.IO.FileAttributes.Hidden );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "AppManager#saveToCor; ex=" + ex );
                }
            }
#endif
        }
    }

    public static void saveTo( String file ) {
        if ( _s_vsq != null ) {
            if ( editorConfig.UseProjectCache ) {
                // キャッシュディレクトリの処理
                String dir = PortUtil.getDirectoryName( file );
                String name = PortUtil.getFileNameWithoutExtension( file );
                String cacheDir = PortUtil.combinePath( dir, name + ".cadencii" );

                if ( !PortUtil.isDirectoryExists( cacheDir ) ) {
                    try {
                        PortUtil.createDirectory( cacheDir );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "AppManager#saveTo; ex=" + ex );
                        showMessageBox( PortUtil.formatMessage( _( "failed creating cache directory, '{0}'." ), cacheDir ),
                                        _( "Info." ),
                                        PortUtil.OK_OPTION,
                                        org.kbinani.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE );
                        return;
                    }
                }

                String currentCacheDir = getTempWaveDir();
                if ( !currentCacheDir.Equals( cacheDir ) ) {
                    for ( int i = 1; i < _s_vsq.Track.size(); i++ ) {
                        String wavFrom = PortUtil.combinePath( currentCacheDir, i + ".wav" );
                        String wavTo = PortUtil.combinePath( cacheDir, i + ".wav" );
                        if ( PortUtil.isFileExists( wavFrom ) ) {
                            if ( PortUtil.isFileExists( wavTo ) ) {
                                try {
                                    PortUtil.deleteFile( wavTo );
                                } catch ( Exception ex ) {
                                    PortUtil.stderr.println( "AppManager#saveTo; ex=" + ex );
                                }
                            }
                            try {
                                PortUtil.moveFile( wavFrom, wavTo );
                            } catch ( Exception ex ) {
                                PortUtil.stderr.println( "AppManager#saveTo; ex=" + ex );
                                showMessageBox( PortUtil.formatMessage( _( "failed copying WAVE cache file, '{0}'." ), wavFrom ),
                                                _( "Error" ),
                                                PortUtil.OK_OPTION,
                                                org.kbinani.windows.forms.Utility.MSGBOX_WARNING_MESSAGE );
                                break;
                            }
                        }

                        String xmlFrom = PortUtil.combinePath( currentCacheDir, i + ".xml" );
                        String xmlTo = PortUtil.combinePath( cacheDir, i + ".xml" );
                        if ( PortUtil.isFileExists( xmlFrom ) ) {
                            if ( PortUtil.isFileExists( xmlTo ) ) {
                                try {
                                    PortUtil.deleteFile( xmlTo );
                                } catch ( Exception ex ) {
                                    PortUtil.stderr.println( "AppManager#saveTo; ex=" + ex );
                                }
                            }
                            try {
                                PortUtil.moveFile( xmlFrom, xmlTo );
                            } catch ( Exception ex ) {
                                PortUtil.stderr.println( "AppManager#saveTo; ex=" + ex );
                                showMessageBox( PortUtil.formatMessage( _( "failed copying XML cache file, '{0}'." ), xmlFrom ),
                                                _( "Error" ),
                                                PortUtil.OK_OPTION,
                                                org.kbinani.windows.forms.Utility.MSGBOX_WARNING_MESSAGE );
                                break;
                            }
                        }
                    }

                    setTempWaveDir( cacheDir );
                }
                _s_vsq.cacheDir = cacheDir;
            }
        }

        saveToCor( file );
        
        if ( _s_vsq != null ) {
            _s_file = file;
            editorConfig.pushRecentFiles( _s_file );
            if ( !s_auto_backup_timer.isRunning() && editorConfig.AutoBackupIntervalMinutes > 0 ) {
                double millisec = editorConfig.AutoBackupIntervalMinutes * 60.0 * 1000.0;
                int draft = (int)millisec;
                if ( millisec > int.MaxValue ) {
                    draft = int.MaxValue;
                }
                s_auto_backup_timer.setDelay( draft );
                s_auto_backup_timer.start();
            }
        }
    }*/

    /// <summary>
    /// 現在の演奏マーカーの位置を取得します。
    /// </summary>
    org.kbinani.cadencii.AppManager.getCurrentClock = function() {
        return this._s_current_clock;
    };

    /*/// <summary>
    /// 現在の演奏マーカーの位置を設定します。
    /// </summary>
    public static void setCurrentClock( int value ) {
        int old = _s_current_clock;
        _s_current_clock = value;
        int barcount = _s_vsq.getBarCountFromClock( _s_current_clock );
        int bar_top_clock = _s_vsq.getClockFromBarCount( barcount );
        Timesig timesig = _s_vsq.getTimesigAt( _s_current_clock );
        int clock_per_beat = 480 / 4 * timesig.denominator;
        int beat = (_s_current_clock - bar_top_clock) / clock_per_beat;
        s_current_play_position.barCount = barcount - _s_vsq.getPreMeasure() + 1;
        s_current_play_position.beat = beat + 1;
        s_current_play_position.clock = _s_current_clock - bar_top_clock - clock_per_beat * beat;
        s_current_play_position.denominator = timesig.denominator;
        s_current_play_position.numerator = timesig.numerator;
        s_current_play_position.tempo = _s_vsq.getTempoAt( _s_current_clock );
        if ( old != _s_current_clock ) {
            try {
                currentClockChangedEvent.raise( typeof( AppManager ), new BEventArgs() );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "AppManager#setCurrentClock; ex=" + ex );
            }
        }
    }

    /// <summary>
    /// 現在の演奏カーソルの位置(m_current_clockと意味は同じ。CurrentClockが変更されると、自動で更新される)
    /// </summary>
    public static PlayPositionSpecifier getPlayPosition() {
        return s_current_play_position;
    }*/

    /**
     * 現在選択されているトラックを取得または設定します
     */
    org.kbinani.cadencii.AppManager.getSelected = function() {
        if( this._s_vsq != null && this._s_vsq.Track != null ){
            var tracks = this._s_vsq.Track.length;
            if ( tracks <= this._s_selected ) {
                this._s_selected = tracks - 1;
            }
        }else{
            this._s_selected = 1;
        }
        return this._s_selected;
    };

    org.kbinani.cadencii.AppManager.setSelected = function( value ) {
        this._s_selected = value;
    };

    /**
    /// <summary>
    /// vsqファイルを読込みます
    /// </summary>
    /// <param name="file"></param>
    public static void readVsq( String file ) {
        _s_selected = 1;
        _s_file = file;
        VsqFileEx newvsq = null;
        try {
            newvsq = VsqFileEx.readFromXml( file );
        } catch ( Exception ex ) {
            PortUtil.stderr.println( "AppManager#readVsq; ex=" + ex );
            return;
        }
        if ( newvsq == null ) {
            return;
        }
        _s_vsq = newvsq;
        for ( int i = 0; i < _s_vsq.editorStatus.renderRequired.Length; i++ ) {
            if ( i < _s_vsq.Track.size() - 1 ) {
                _s_vsq.editorStatus.renderRequired[i] = true;
            } else {
                _s_vsq.editorStatus.renderRequired[i] = false;
            }
        }
        startMarker = _s_vsq.getPreMeasureClocks();
        int bar = _s_vsq.getPreMeasure() + 1;
        endMarker = _s_vsq.getClockFromBarCount( bar );
        if ( _s_vsq.Track.size() >= 1 ) {
            _s_selected = 1;
        } else {
            _s_selected = -1;
        }
        if ( mainWindow != null ) {
            mainWindow.updateBgmMenuState();
        }
    }*/

    /**
     * vsqファイル。
     */
    org.kbinani.cadencii.AppManager.getVsqFile = function() {
        return this._s_vsq;
    };

    org.kbinani.cadencii.AppManager.setVsqFile = function( vsq ) {
        this._s_vsq = vsq;
        /*for ( var i = 0; i < this._s_vsq.editorStatus.renderRequired.length; i++ ) {
            if ( i < this._s_vsq.Track.length - 1 ) {
                _s_vsq.editorStatus.renderRequired[i] = true;
            } else {
                _s_vsq.editorStatus.renderRequired[i] = false;
            }
        }*/
        this._s_file = "";
        this.startMarker = this._s_vsq.getPreMeasureClocks();
        var bar = this._s_vsq.getPreMeasure() + 1;
        this.endMarker = this._s_vsq.getClockFromBarCount( bar );
        /*this.s_auto_backup_timer.stop();*/
        /*if ( mainWindow != null ) {
            mainWindow.updateBgmMenuState();
        }*/
    };

    /*public static void init() {
        loadConfig();
#if !JAVA
        // UTAU歌手のアイコンを読み込み、起動画面に表示を要求する
        int c = editorConfig.UtauSingers.size();
        for ( int i = 0; i < c; i++ ) {
            SingerConfig sc = editorConfig.UtauSingers.get( i );
            if ( sc == null ) {
                continue;
            }
            String dir = sc.VOICEIDSTR;
#if DEBUG
            PortUtil.stdout.println( "AppManager#init; dir=" + dir );
#endif
            String character = PortUtil.combinePath( dir, "character.txt" );
            if ( !PortUtil.isFileExists( character ) ) {
#if DEBUG
                PortUtil.println( "AppManager#init; file not found: " + character );
#endif
                continue;
            }

            String path_image = "";
            BufferedReader br = null;
            try {
                br = new BufferedReader( new InputStreamReader( new FileInputStream( character ), "Shift_JIS" ) );
                String line = "";
                while ( (line = br.readLine()) != null ) {
                    if ( !line.StartsWith( "image" ) ) {
                        continue;
                    }
                    String[] spl = line.Split( '=' );
                    if ( spl.Length < 2 ) {
                        continue;
                    }
                    String token = spl[0].Trim().ToLower();
                    String img = spl[1].Trim();
                    if ( !token.Equals( "image" ) ) {
                        continue;
                    }
                    path_image = PortUtil.combinePath( dir, img );
                    break;
                }
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "AppManager#init; ex=" + ex );
            } finally {
                if ( br != null ) {
                    try {
                        br.close();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "AppManager#init; ex2=" + ex2 );
                    }
                }
            }

#if DEBUG
            PortUtil.println( "AppManager#init; path_image=" + path_image );
#endif
            Cadencii.splash.addIconThreadSafe( path_image, sc.VOICENAME );
        }
#endif

        VSTiProxy.init();
#if !JAVA
        // アイコンパレード, VOCALOID1
        SingerConfigSys singer_config_sys1 = VocaloSysUtil.getSingerConfigSys( SynthesizerType.VOCALOID1 );
        if ( singer_config_sys1 != null ) {
            foreach ( SingerConfig sc in singer_config_sys1.getInstalledSingers() ) {
                if ( sc == null ) {
                    continue;
                }
                String name = sc.VOICENAME.ToLower();
                String path_image = PortUtil.combinePath(
                                        PortUtil.combinePath(
                                            PortUtil.getApplicationStartupPath(), "resources" ),
                                        name + ".png" );
#if DEBUG
                PortUtil.println( "AppManager#init; path_image=" + path_image );
#endif
                Cadencii.splash.addIconThreadSafe( path_image, sc.VOICENAME );
            }
        }

        // アイコンパレード、VOCALOID2
        SingerConfigSys singer_config_sys2 = VocaloSysUtil.getSingerConfigSys( SynthesizerType.VOCALOID2 );
        if ( singer_config_sys2 != null ) {
            foreach ( SingerConfig sc in singer_config_sys2.getInstalledSingers() ) {
                if ( sc == null ) {
                    continue;
                }
                String name = sc.VOICENAME.ToLower();
                String path_image = PortUtil.combinePath(
                                        PortUtil.combinePath(
                                            PortUtil.getApplicationStartupPath(), "resources" ),
                                        name + ".png" );
#if DEBUG
                PortUtil.println( "AppManager#init; path_image=" + path_image );
#endif
                Cadencii.splash.addIconThreadSafe( path_image, sc.VOICENAME );
            }
        }
#endif

        PlaySound.init();
        s_locker = new Object();
        // VOCALOID2の辞書を読み込み
        SymbolTable.loadSystemDictionaries();
        // 日本語辞書
        SymbolTable.loadDictionary(
            PortUtil.combinePath( PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "resources" ), "dict_ja.txt" ), 
            "DEFAULT_JP" );
        // 英語辞書
        SymbolTable.loadDictionary( 
            PortUtil.combinePath( PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "resources" ), "dict_en.txt" ),
            "DEFAULT_EN" );
        // 拡張辞書
        SymbolTable.loadAllDictionaries( PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "udic" ) );
        VSTiProxy.CurrentUser = "";
#if JAVA
        Util.isApplyFontRecurseEnabled = false;
#endif

        #region Apply User Dictionary Configuration
        try {
            Vector<ValuePair<String, Boolean>> current = new Vector<ValuePair<String, Boolean>>();
            for ( int i = 0; i < SymbolTable.getCount(); i++ ) {
                current.add( new ValuePair<String, Boolean>( SymbolTable.getSymbolTable( i ).getName(), false ) );
            }
            Vector<ValuePair<String, Boolean>> config_data = new Vector<ValuePair<String, Boolean>>();
            int count = editorConfig.UserDictionaries.size();
            for ( int i = 0; i < count; i++ ) {
                String[] spl = PortUtil.splitString( editorConfig.UserDictionaries.get( i ), new char[] { '\t' }, 2 );
                config_data.add( new ValuePair<String, Boolean>( spl[0], (spl[1].Equals( "T" ) ? true : false) ) );
#if DEBUG
                AppManager.debugWriteLine( "    " + spl[0] + "," + spl[1] );
#endif
            }
            Vector<ValuePair<String, Boolean>> common = new Vector<ValuePair<String, Boolean>>();
            for ( int i = 0; i < config_data.size(); i++ ) {
                for ( int j = 0; j < current.size(); j++ ) {
                    if ( config_data.get( i ).getKey().Equals( current.get( j ).getKey() ) ) {
                        current.get( j ).setValue( true ); //こっちのbooleanは、AppManager.EditorConfigのUserDictionariesにもKeyが含まれているかどうかを表すので注意
                        common.add( new ValuePair<String, Boolean>( config_data.get( i ).getKey(), config_data.get( i ).getValue() ) );
                        break;
                    }
                }
            }
            for ( int i = 0; i < current.size(); i++ ) {
                if ( !current.get( i ).getValue() ) {
                    common.add( new ValuePair<String, Boolean>( current.get( i ).getKey(), false ) );
                }
            }
            SymbolTable.changeOrder( common );
        } catch ( Exception ex ) {
            PortUtil.stderr.println( "AppManager#init; ex=" + ex );
        }
        #endregion

#if !TREECOM
        s_id = PortUtil.getMD5FromString( (long)PortUtil.getCurrentTime() + "" ).Replace( "_", "" );
        tempWaveDir = PortUtil.combinePath( getCadenciiTempDir(), s_id );
        if ( !PortUtil.isDirectoryExists( tempWaveDir ) ) {
            PortUtil.createDirectory( tempWaveDir );
        }
        String log = PortUtil.combinePath( getTempWaveDir(), "run.log" );
#endif

        for ( Iterator<SingerConfig> itr = editorConfig.UtauSingers.iterator(); itr.hasNext(); ) {
            SingerConfig config = itr.next();
            UtauVoiceDB db = null;
            try {
                db = new UtauVoiceDB( config );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "AppManager#init; ex=" + ex );
                db = null;
            }
            if ( db != null ) {
                utauVoiceDB.put( config.VOICEIDSTR, db );
            }
        }

        s_auto_backup_timer = new BTimer();
#if JAVA
        s_auto_backup_timer.tickEvent.add( new BEventHandler( AppManager.class, "handleAutoBackupTimerTick" ) );
#else
        s_auto_backup_timer.Tick += handleAutoBackupTimerTick;
#endif
    }

    #region クリップボードの管理
#if CLIPBOARD_AS_TEXT
    /// <summary>
    /// オブジェクトをシリアライズし，クリップボードに格納するための文字列を作成します
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static String getSerializedText( Object obj ) 
#if JAVA
        throws IOException
#endif
    {
        String str = "";
        ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
        ObjectOutputStream objectOutputStream = new ObjectOutputStream( outputStream );
        objectOutputStream.writeObject( obj );

        byte[] arr = outputStream.toByteArray();
#if JAVA
        str = CLIP_PREFIX + ":" + obj.getClass().getName() + ":" + Base64.encode( arr );
#else
        str = CLIP_PREFIX + ":" + obj.GetType().FullName + ":" + Base64.encode( arr );
#endif
        return str;
    }

    /// <summary>
    /// クリップボードに格納された文字列を元に，デシリアライズされたオブジェクトを取得します
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private static Object getDeserializedObjectFromText( String s ) {
        if ( s.StartsWith( CLIP_PREFIX ) ) {
            int index = s.IndexOf( ":" );
            index = s.IndexOf( ":", index + 1 );
            Object ret = null;
            try {
                ByteArrayInputStream bais = new ByteArrayInputStream( Base64.decode( s.Substring( index + 1 ) ) );
                ObjectInputStream ois = new ObjectInputStream( bais );
                ret = ois.readObject();
            } catch ( Exception ex ) {
                ret = null;
            }
            return ret;
        } else {
            return null;
        }
    }
#endif

    public static void clearClipBoard() {
#if CLIPBOARD_AS_TEXT
        if ( PortUtil.isClipboardContainsText() ) {
            String clip = PortUtil.getClipboardText();
            if ( clip != null && clip.StartsWith( CLIP_PREFIX ) ) {
                PortUtil.clearClipboard();
            }
        }
#else
        if ( Clipboard.ContainsData( typeof( ClipboardEntry ) + "" ) ) {
            Clipboard.Clear();
        }
#endif
    }

    public static void setClipboard( ClipboardEntry item ) {
#if CLIPBOARD_AS_TEXT
        String clip = "";
        try {
            clip = getSerializedText( item );
        } catch ( Exception ex ) {
            PortUtil.stderr.println( "AppManager#setClipboard; ex=" + ex );
            return;
        }
        PortUtil.clearClipboard();
        PortUtil.setClipboardText( clip );
#else
        Clipboard.SetDataObject( item, false );
#endif
    }

    public static ClipboardEntry getCopiedItems() {
        ClipboardEntry ce = null;
#if CLIPBOARD_AS_TEXT
        if ( PortUtil.isClipboardContainsText() ) {
            String clip = PortUtil.getClipboardText();
            if ( clip.StartsWith( CLIP_PREFIX ) ) {
                int index1 = clip.IndexOf( ":" );
                int index2 = clip.IndexOf( ":", index1 + 1 );
                String typename = clip.Substring( index1 + 1, index2 - index1 - 1 );
#if JAVA
                if ( typename.Equals( ClipboardEntry.class.getName() ) ) {
#else
                if ( typename.Equals( typeof( ClipboardEntry ).FullName ) ) {
#endif
                    try {
                        ce = (ClipboardEntry)getDeserializedObjectFromText( clip );
                    } catch ( Exception ex ) {
                    }
                }
            }
        }
#else
        IDataObject dobj = Clipboard.GetDataObject();
        if ( dobj != null ) {
            Object obj = dobj.GetData( typeof( ClipboardEntry ) );
            if ( obj != null && obj is ClipboardEntry ) {
                ce = (ClipboardEntry)obj;
            }
        }
#endif
        if ( ce == null ) {
            ce = new ClipboardEntry();
        }
        if ( ce.beziers == null ) {
            ce.beziers = new TreeMap<CurveType, Vector<BezierChain>>();
        }
        if ( ce.events == null ) {
            ce.events = new Vector<VsqEvent>();
        }
        if ( ce.points == null ) {
            ce.points = new TreeMap<CurveType, VsqBPList>();
        }
        if ( ce.tempo == null ) {
            ce.tempo = new Vector<TempoTableEntry>();
        }
        if ( ce.timesig == null ) {
            ce.timesig = new Vector<TimeSigTableEntry>();
        }
        return ce;
    }

    private static void setClipboard(
        Vector<VsqEvent> events,
        Vector<TempoTableEntry> tempo,
        Vector<TimeSigTableEntry> timesig,
        TreeMap<CurveType, VsqBPList> curve,
        TreeMap<CurveType, Vector<BezierChain>> bezier,
        int copy_started_clock ) {
        ClipboardEntry ce = new ClipboardEntry();
        ce.events = events;
        ce.tempo = tempo;
        ce.timesig = timesig;
        ce.points = curve;
        ce.beziers = bezier;
        ce.copyStartedClock = copy_started_clock;
#if CLIPBOARD_AS_TEXT
        String clip = "";
        try {
            clip = getSerializedText( ce );
        } catch ( Exception ex ) {
#if JAVA
            System.err.println( "AppManager#setCopiedEvent; ex=" + ex );
#else
#if DEBUG
            PortUtil.println( "AppManager#setCopiedEvent; ex=" + ex );
#endif
#endif
            return;
        }
        PortUtil.clearClipboard();
        PortUtil.setClipboardText( clip );
#else
        Clipboard.SetDataObject( ce, false );
#endif
    }

    public static void setCopiedEvent( Vector<VsqEvent> item, int copy_started_clock ) {
        setClipboard( item, null, null, null, null, copy_started_clock );
    }

    public static void setCopiedTempo( Vector<TempoTableEntry> item, int copy_started_clock ) {
        setClipboard( null, item, null, null, null, copy_started_clock );
    }

    public static void setCopiedTimesig( Vector<TimeSigTableEntry> item, int copy_started_clock ) {
        setClipboard( null, null, item, null, null, copy_started_clock );
    }

    public static void setCopiedCurve( TreeMap<CurveType, VsqBPList> item, int copy_started_clock ) {
        setClipboard( null, null, null, item, null, copy_started_clock );
    }

    public static void setCopiedBezier( TreeMap<CurveType, Vector<BezierChain>> item, int copy_started_clock ) {
        setClipboard( null, null, null, null, item, copy_started_clock );
    }
    #endregion*/

    /**
     * 位置クオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
     * @return [int]
     */
    org.kbinani.cadencii.AppManager.getPositionQuantizeClock = function() {
        return org.kbinani.cadencii.QuantizeModeUtil.getQuantizeClock( this.editorConfig.getPositionQuantize(), this.editorConfig.isPositionQuantizeTriplet() );
    };

    /**
     * 音符長さクオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
     * @return [int]
     */
    org.kbinani.cadencii.AppManager.getLengthQuantizeClock = function() {
        return org.kbinani.cadencii.QuantizeModeUtil.getQuantizeClock( this.editorConfig.getLengthQuantize(), this.editorConfig.isLengthQuantizeTriplet() );
    };

    /*
    /// <summary>
    /// 現在の設定を設定ファイルに書き込みます。
    /// </summary>
    public static void saveConfig() {
        // ユーザー辞書の情報を取り込む
        editorConfig.UserDictionaries.clear();
        int count = SymbolTable.getCount();
        for ( int i = 0; i < count; i++ ) {
            SymbolTable table = SymbolTable.getSymbolTable( i );
            editorConfig.UserDictionaries.add( table.getName() + "\t" + (table.isEnabled() ? "T" : "F") );
        }
        editorConfig.KeyWidth = keyWidth;

        String file = PortUtil.combinePath( Utility.getApplicationDataPath(), CONFIG_FILE_NAME );
        try {
            EditorConfig.serialize( editorConfig, file );
        } catch ( Exception ex ) {
            PortUtil.stderr.println( "AppManager#saveConfig; ex=" + ex );
        }
    }

    /// <summary>
    /// 設定ファイルを読み込みます。
    /// 設定ファイルが壊れていたり存在しない場合、デフォルトの設定が使われます。
    /// </summary>
    public static void loadConfig() {
        String appdata = Utility.getApplicationDataPath();
        if ( appdata.Equals( "" ) ) {
            editorConfig = new EditorConfig();
            return;
        }
        String config_file = PortUtil.combinePath( appdata, CONFIG_FILE_NAME );
        EditorConfig ret = null;
        if ( PortUtil.isFileExists( config_file ) ) {
            try {
                ret = EditorConfig.deserialize( config_file );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "AppManager#loadConfig; ex=" + ex );
                ret = null;
            }
        } else {
            config_file = PortUtil.combinePath( appdata, CONFIG_FILE_NAME );
            if ( PortUtil.isFileExists( config_file ) ) {
                try {
                    ret = EditorConfig.deserialize( config_file );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "AppManager#locdConfig; ex=" + ex );
                    ret = null;
                }
            }
        }
        if ( ret == null ) {
            ret = new EditorConfig();
#if !JAVA
            String name = Application.CurrentCulture.Name;
            String lang = "";
            if ( name.Equals( "ja" ) ||
                 name.StartsWith( "ja-" ) ) {
                lang = "ja";
            } else {
                lang = name;
            }
            ret.Language = lang;
            Messaging.setLanguage( lang );
            PortUtil.println( "AppManager#loadConfig; Application.CurrentCulture.Name=" + Application.CurrentCulture.Name );
#endif
        }
        editorConfig = ret;
        int count = SymbolTable.getCount();
        for ( int i = 0; i < count; i++ ) {
            SymbolTable st = SymbolTable.getSymbolTable( i );
            boolean found = false;
            for ( Iterator<String> itr = editorConfig.UserDictionaries.iterator(); itr.hasNext(); ) {
                String s = itr.next();
                String[] spl = PortUtil.splitString( s, new char[] { '\t' }, 2 );
                if ( st.getName().Equals( spl[0] ) ) {
                    found = true;
                    break;
                }
            }
            if ( !found ) {
                editorConfig.UserDictionaries.add( st.getName() + "\tT" );
            }
        }
#if ENABLE_MIDI
        MidiPlayer.DeviceGeneral = (uint)editorConfig.MidiDeviceGeneral.PortNumber;
        MidiPlayer.DeviceMetronome = (uint)editorConfig.MidiDeviceMetronome.PortNumber;
        MidiPlayer.NoteBell = editorConfig.MidiNoteBell;
        MidiPlayer.NoteNormal = editorConfig.MidiNoteNormal;
        MidiPlayer.PreUtterance = editorConfig.MidiPreUtterance;
        MidiPlayer.ProgramBell = editorConfig.MidiProgramBell;
        MidiPlayer.ProgramNormal = editorConfig.MidiProgramNormal;
        MidiPlayer.RingBell = editorConfig.MidiRingBell;
#endif

        int draft_key_width = editorConfig.KeyWidth;
        if ( draft_key_width < MIN_KEY_WIDTH ) {
            draft_key_width = MIN_KEY_WIDTH;
        } else if ( MAX_KEY_WIDTH < draft_key_width ) {
            draft_key_width = MAX_KEY_WIDTH;
        }
        keyWidth = draft_key_width;
    }

    public static VsqID getSingerIDUtau( int language, int program ) {
        VsqID ret = new VsqID( 0 );
        ret.type = VsqIDType.Singer;
        int index = language << 7 | program;
        if ( 0 <= index && index < editorConfig.UtauSingers.size() ) {
            SingerConfig sc = editorConfig.UtauSingers.get( index );
            ret.IconHandle = new IconHandle();
            ret.IconHandle.IconID = "$0701" + PortUtil.toHexString( language, 2 ) + PortUtil.toHexString( program, 2 );
            ret.IconHandle.IDS = sc.VOICENAME;
            ret.IconHandle.Index = 0;
            ret.IconHandle.Language = language;
            ret.IconHandle.setLength( 1 );
            ret.IconHandle.Original = language << 8 | program;
            ret.IconHandle.Program = program;
            ret.IconHandle.Caption = "";
            return ret;
        } else {
            ret.IconHandle = new IconHandle();
            ret.IconHandle.Program = 0;
            ret.IconHandle.Language = 0;
            ret.IconHandle.IconID = "$0701" + PortUtil.toHexString( 0, 4 );
            ret.IconHandle.IDS = "Unknown";
            ret.type = VsqIDType.Singer;
            return ret;
        }
    }

    public static SingerConfig getSingerInfoUtau( int language, int program ) {
        int index = language << 7 | program;
        if ( 0 <= index && index < editorConfig.UtauSingers.size() ) {
            return editorConfig.UtauSingers.get( index );
        } else {
            return null;
        }
    }

    public static SingerConfig getSingerInfoAquesTone( int program_change ) {
#if ENABLE_AQUESTONE
        for ( int i = 0; i < AquesToneDriver.SINGERS.Length; i++ ) {
            if ( program_change == AquesToneDriver.SINGERS[i].Program ) {
                return AquesToneDriver.SINGERS[i];
            }
        }
#endif
        return null;
    }

    public static VsqID getSingerIDAquesTone( int program ) {
        VsqID ret = new VsqID( 0 );
        ret.type = VsqIDType.Singer;
        int index = -1;
#if ENABLE_AQUESTONE
        int c = AquesToneDriver.SINGERS.Length;
        for ( int i = 0; i < c; i++ ) {
            if ( AquesToneDriver.SINGERS[i].Program == program ) {
                index = i;
                break;
            }
        }
        if ( index >= 0 ) {
            SingerConfig sc = AquesToneDriver.SINGERS[index];
            int lang = 0;
            ret.IconHandle = new IconHandle();
            ret.IconHandle.IconID = "$0701" + PortUtil.toHexString( lang, 2 ) + PortUtil.toHexString( program, 2 );
            ret.IconHandle.IDS = sc.VOICENAME;
            ret.IconHandle.Index = 0;
            ret.IconHandle.Language = lang;
            ret.IconHandle.setLength( 1 );
            ret.IconHandle.Original = lang << 8 | program;
            ret.IconHandle.Program = program;
            ret.IconHandle.Caption = "";
            return ret;
        } else {
#endif
            ret.IconHandle = new IconHandle();
            ret.IconHandle.Program = 0;
            ret.IconHandle.Language = 0;
            ret.IconHandle.IconID = "$0701" + PortUtil.toHexString( 0, 2 ) + PortUtil.toHexString( 0, 2 );
            ret.IconHandle.IDS = "Unknown";
            ret.type = VsqIDType.Singer;
            return ret;
#if ENABLE_AQUESTONE
        }
#endif
    }*/

    org.kbinani.cadencii.AppManager.getHilightColor = function() {
        return this._s_hilight_brush;
    };

    org.kbinani.cadencii.AppManager.setHilightColor = function( value ) {
        this._s_hilight_brush = value;
    };

/*
    /// <summary>
    /// ベースとなるテンポ。
    /// </summary>
    public static int getBaseTempo() {
        return _s_base_tempo;
    }

    public static void setBaseTempo( int value ) {
        _s_base_tempo = value;
    }*/
}
