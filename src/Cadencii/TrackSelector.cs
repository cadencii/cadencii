/*
 * TrackSelector.cs
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
#define COMPONENT_ENABLE_LOCATION
//#define MONITOR_FPS
//#define OLD_IMPL_MOUSE_TRACER
using System;
using System.Threading;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using cadencii;
using cadencii.apputil;
using cadencii.java.awt;
using cadencii.java.util;
using cadencii.vsq;
using cadencii.windows.forms;

namespace cadencii
{
    using Graphics = cadencii.java.awt.Graphics2D;

    /// <summary>
    /// コントロールカーブ，トラックの一覧，歌手変更イベントなどを表示するコンポーネント．
    /// </summary>
    public class TrackSelector : UserControl
    {
        #region constants and internal enums
        private enum MouseDownMode
        {
            NONE,
            CURVE_EDIT,
            TRACK_LIST,
            SINGER_LIST,
            /// <summary>
            /// マウス長押しによるVELの編集。マウスがDownされ、MouseHoverが発生するのを待っている状態
            /// </summary>
            VEL_WAIT_HOVER,
            /// <summary>
            /// マウス長押しによるVELの編集。MouseHoverが発生し、編集している状態
            /// </summary>
            VEL_EDIT,
            /// <summary>
            /// ベジエカーブのデータ点または制御点を移動させているモード
            /// </summary>
            BEZIER_MODE,
            /// <summary>
            /// ベジエカーブのデータ点の範囲選択をするモード
            /// </summary>
            BEZIER_SELECT,
            /// <summary>
            /// ベジエカーブのデータ点を新規に追加し、マウスドラッグにより制御点の位置を変えているモード
            /// </summary>
            BEZIER_ADD_NEW,
            /// <summary>
            /// 既存のベジエカーブのデータ点を追加し、マウスドラッグにより制御点の位置を変えているモード
            /// </summary>
            BEZIER_EDIT,
            /// <summary>
            ///  エンベロープのデータ点を移動させているモード
            /// </summary>
            ENVELOPE_MOVE,
            /// <summary>
            /// 先行発音を移動させているモード
            /// </summary>
            PRE_UTTERANCE_MOVE,
            /// <summary>
            /// オーバーラップを移動させているモード
            /// </summary>
            OVERLAP_MOVE,
            /// <summary>
            /// データ点を移動しているモード
            /// </summary>
            POINT_MOVE,
        }

        /// <summary>
        /// ベジエ曲線の色
        /// </summary>
        private static readonly Color COLOR_BEZIER_CURVE = PortUtil.Navy;
        /// <summary>
        /// ベジエ曲線の補助線の色
        /// </summary>
        private static readonly Color COLOR_BEZIER_AUXILIARY = Color.orange;
        /// <summary>
        /// ベジエ曲線の制御点の色
        /// </summary>
        private static readonly Color COLOR_BEZIER_DOT_NORMAL = new Color(237, 107, 158);
        /// <summary>
        /// ベジエ曲線の制御点の枠色
        /// </summary>
        private static readonly Color COLOR_BEZIER_DOT_NORMAL_DARK = new Color(153, 19, 70);
        /// <summary>
        /// ベジエ曲線のデータ点の色
        /// </summary>
        private static readonly Color COLOR_BEZIER_DOT_BASE = new Color(125, 198, 34);
        /// <summary>
        /// ベジエ曲線のデータ点の枠色
        /// </summary>
        private static readonly Color COLOR_BEZIER_DOT_BASE_DARK = new Color(62, 99, 17);

        /// <summary>
        /// データ点のハイライト色
        /// </summary>
        private static readonly Color COLOR_DOT_HILIGHT = PortUtil.Coral;
        private static readonly Color COLOR_A244R255G023B012 = new Color(255, 23, 12, 244);
        private static readonly Color COLOR_A144R255G255B255 = new Color(255, 255, 255, 144);
        private static readonly Color COLOR_A072R255G255B255 = new Color(255, 255, 255, 72);
        private static readonly Color COLOR_A127R008G166B172 = new Color(8, 166, 172, 127);
        private static readonly Color COLOR_A098R000G000B000 = new Color(0, 0, 0, 98);
        /// <summary>
        /// 歌手変更を表すボックスの枠線のハイライト色
        /// </summary>
        private static readonly Color COLOR_SINGERBOX_BORDER_HILIGHT = new Color(246, 251, 10);
        /// <summary>
        /// 歌手変更を表すボックスの枠線の色
        /// </summary>
        private static readonly Color COLOR_SINGERBOX_BORDER = new Color(182, 182, 182);
        /// <summary>
        /// ビブラートコントロールカーブの、ビブラート以外の部分を塗りつぶす時の色
        /// </summary>
        private static readonly Color COLOR_VIBRATO_SHADOW = new Color(0, 0, 0, 127);
        /// <summary>
        /// マウスの軌跡を描くときの塗りつぶし色
        /// </summary>
        private static readonly Color COLOR_MOUSE_TRACER = new Color(8, 166, 172, 127);
        /// <summary>
        /// ベロシティを画面に描くときの，棒グラフの幅(pixel)
        /// </summary>
        public const int VEL_BAR_WIDTH = 8;
        /// <summary>
        /// パフォーマンスカウンタ用バッファの容量
        /// </summary>
        const int NUM_PCOUNTER = 50;
        /// <summary>
        /// コントロールの下辺から、TRACKタブまでのオフセット(px)
        /// </summary>
        public const int OFFSET_TRACK_TAB = 19;
        const int FOOTER = 7;
        /// <summary>
        /// コントロールの上端と、グラフのY軸最大値位置との距離
        /// </summary>
        public const int HEADER = 8;
        const int BUF_LEN = 512;
        /// <summary>
        /// 歌手変更イベントの表示矩形の幅
        /// </summary>
        const int SINGER_ITEM_WIDTH = 66;
        /// <summary>
        /// RENDERボタンの幅(px)
        /// </summary>
        const int PX_WIDTH_RENDER = 10;
        /// <summary>
        /// カーブ制御点の幅（実際は_DOT_WID * 2 + 1ピクセルで描画される）
        /// </summary>
        const int DOT_WID = 3;
        /// <summary>
        /// カーブの種類を表す部分の，1個あたりの高さ（ピクセル，余白を含む）
        /// TrackSelectorの推奨表示高さは，HEIGHT_WITHOUT_CURVE + UNIT_HEIGHT_PER_CURVE * (カーブの個数)となる
        /// </summary>
        const int UNIT_HEIGHT_PER_CURVE = 18;
        /// <summary>
        /// カーブの種類を除いた部分の高さ（ピクセル）．
        /// TrackSelectorの推奨表示高さは，HEIGHT_WITHOUT_CURVE + UNIT_HEIGHT_PER_CURVE * (カーブの個数)となる
        /// </summary>
        const int HEIGHT_WITHOUT_CURVE = OFFSET_TRACK_TAB * 2 + UNIT_HEIGHT_PER_CURVE;
        /// <summary>
        /// トラックの名前表示部分の最大表示幅（ピクセル）
        /// </summary>
        const int TRACK_SELECTOR_MAX_WIDTH = 80;
        /// <summary>
        /// 先行発音を表示する旗を描画する位置のy座標
        /// </summary>
        const int OFFSET_PRE = 15;
        /// <summary>
        /// オーバーラップを表示する旗を描画する位置のy座標
        /// </summary>
        const int OFFSET_OVL = 40;
        /// <summary>
        /// 旗の上下に追加するスペース(ピクセル)
        /// </summary>
        const int FLAG_SPACE = 2;
        #endregion

        /// <summary>
        /// 現在最前面に表示されているカーブ
        /// </summary>
        private CurveType mSelectedCurve = CurveType.VEL;
        /// <summary>
        /// 現在最前面カーブのすぐ後ろに表示されているカーブ
        /// </summary>
        private CurveType mLastSelectedCurve = CurveType.DYN;
        /// <summary>
        /// コントロールカーブを表示するモードかどうか
        /// </summary>
        private bool mCurveVisible = true;
        /// <summary>
        /// 現在のマウス位置におけるカーブの値
        /// </summary>
        private int mMouseValue;
        /// <summary>
        /// 編集しているBezierChainのID
        /// </summary>
        public int mEditingChainID = -1;
        /// <summary>
        /// 編集しているBezierPointのID
        /// </summary>
        public int mEditingPointID = -1;
        /// <summary>
        /// マウスがカーブ部分に下ろされている最中かどうかを表すフラグ
        /// </summary>
        private bool mMouseDowned = false;
        /// <summary>
        /// マウスのトレーサ。コントロールカーブ用の仮想スクリーン座標で表す。
        /// </summary>
        private MouseTracer mMouseTracer = new MouseTracer();
        private bool mPencilMoved = false;
        private Thread mMouseHoverThread = null;
        /// <summary>
        /// cmenuSingerのメニューアイテムを初期化するのに使用したRenderer。
        /// </summary>
        private RendererKind mCMenuSingerPrepared = RendererKind.NULL;
        /// <summary>
        /// マウスがDownしてからUpするまでのモード
        /// </summary>
        private MouseDownMode mMouseDownMode = MouseDownMode.NONE;
        /// <summary>
        /// マウスがDownしてからマウスが移動したかどうかを表す。
        /// </summary>
        private bool mMouseMoved = false;
        /// <summary>
        /// マウスドラッグで歌手変更イベントの矩形を移動開始した時の、マウス位置におけるクロック
        /// </summary>
        private int mSingerMoveStartedClock;
        /// <summary>
        /// cmenuSinger用のツールチップの幅を記録しておく。
        /// </summary>
        private int[] mCMenuSingerTooltipWidth;
        /// <summary>
        /// マウス長押しによるVELの編集。選択されている音符のInternalID
        /// </summary>
        private int mVelEditLastSelectedID = -1;
        /// <summary>
        /// マウス長押しによるVELの編集。棒グラフのてっぺんの座標と、マウスが降りた座標の差分。プラスなら、マウスの方が下になる。
        /// </summary>
        private int mVelEditShiftY = 0;
        /// <summary>
        /// マウス長押しによるVELの編集。編集対象の音符のリスト。
        /// </summary>
        private SortedDictionary<int, SelectedEventEntry> mVelEditSelected = new SortedDictionary<int, SelectedEventEntry>();
        /// <summary>
        /// 現在編集操作が行われているBezierChainの、編集直前のオリジナル
        /// </summary>
        private BezierChain mEditingBezierOriginal = null;
        /// <summary>
        /// CTRLキー。MacOSXの場合はMenu
        /// </summary>
        private Keys mModifierKey = Keys.Control;
        /// <summary>
        /// スペースキーが押されているかどうか。
        /// MouseDown時に範囲選択モードをスキップする必要があるので、FormMainでの処理に加えてこのクラス内部でも処理する必要がある
        /// </summary>
        private bool mSpaceKeyDowned = false;
        /// <summary>
        /// マウスがDownした位置の座標．xは仮想スクリーン座標．yは通常のe.Location.Y
        /// </summary>
        private Point mMouseDownLocation = new Point();
        /// <summary>
        /// エンベロープ点を動かすモードで，選択されているInternalID．
        /// </summary>
        private int mEnvelopeEdigintID = -1;
        /// <summary>
        /// エンベロープ点を動かすモードで，選択されている点のタイプ
        /// </summary>
        private int mEnvelopePointKind = -1;
        /// <summary>
        /// エンベロープ点を動かすモードで，編集される前のオリジナルのエンベロープ
        /// </summary>
        private UstEnvelope mEnvelopeOriginal = null;
        /// <summary>
        /// エンベロープ点を動かすモードで、点が移動可能な範囲の始点(秒)
        /// </summary>
        private double mEnvelopeDotBegin;
        /// <summary>
        /// エンベロープ点を動かすモードで、点が移動可能な範囲の終点(秒)
        /// </summary>
        private double mEnvelopeDotEnd;
        /// <summary>
        /// 編集中のエンベロープ
        /// </summary>
        private UstEnvelope mEnvelopeEditing = null;
        /// <summary>
        /// 編集中のエンベロープの範囲の始点（秒）
        /// </summary>
        private double mEnvelopeRangeBegin;
        /// <summary>
        /// 編集中のエンベロープの範囲の終点（秒）
        /// </summary>
        private double mEnvelopeRangeEnd;

        /// <summary>
        /// 現在PreUtteranceを編集中のVsqEventのID
        /// </summary>
        private int mPreUtteranceEditingID;

        /// <summary>
        /// 現在オーバーラップを編集中のVsqEventのID
        /// </summary>
        private int mOverlapEditingID;
        /// <summary>
        /// オーバーラップを編集する前の音符情報
        /// </summary>
        private VsqEvent mPreOverlapOriginal = null;
        /// <summary>
        /// オーバーラップを編集中の音符情報
        /// </summary>
        private VsqEvent mPreOverlapEditing = null;

        /// <summary>
        /// MouseDown時のControl.Modifiersの状態。
        /// </summary>
        private Keys mModifierOnMouseDown = Keys.None;
        /// <summary>
        /// 移動しているデータ点のリスト
        /// </summary>
        private List<BPPair> mMovingPoints = new List<BPPair>();
        /// <summary>
        /// このコントロールの推奨最少表示高さの前回の値．
        /// 推奨表示高さが変わったかどうかを検出するのに使う
        /// </summary>
        private int mLastPreferredMinHeight;
        /// <summary>
        /// 描画幅が2ピクセルのストローク
        /// </summary>
        private BasicStroke mStroke2px = null;
        /// <summary>
        /// デフォルトのストローク
        /// </summary>
        private BasicStroke mStrokeDefault = null;
        /// <summary>
        /// 折れ線グラフを効率よく描画するための描画器
        /// </summary>
        private LineGraphDrawer mGraphDrawer = null;
        private Graphics2D mGraphics = null;
        /// <summary>
        /// メイン画面への参照
        /// </summary>
        private FormMain mMainWindow = null;
        /// <summary>
        /// Overlap, Presendを描画するときに使うフォントで，一文字あたり何ピクセルになるか
        /// </summary>
        private float mTextWidthPerLetter = 0.0f;
        /// <summary>
        /// Overlap, Presendを描画するときに使うフォントの，文字の描画高さ
        /// </summary>
        private int mTextHeight = 0;
        /// <summary>
        /// カーブ種類とメニューアイテムを紐付けるマップ
        /// </summary>
        private SortedDictionary<CurveType, ToolStripMenuItem> mMenuMap = new SortedDictionary<CurveType, ToolStripMenuItem>();
        /// <summary>
        /// ツールチップに表示されるプログラム
        /// </summary>
        private int mTooltipProgram;
        /// <summary>
        /// ツールチップに表示されるLanguage
        /// </summary>
        private int mTooltipLanguage;
        /// <summary>
        /// TrackSelectorで表示させているカーブの一覧
        /// </summary>
        private List<CurveType> mViewingCurves = new List<CurveType>();

        /// <summary>
        /// 最前面に表示するカーブの種類が変更されたとき発生するイベント．
        /// </summary>
        public event SelectedCurveChangedEventHandler SelectedCurveChanged;
        /// <summary>
        /// 表示するトラック番号が変更されたとき発生するイベント．
        /// </summary>
        public event SelectedTrackChangedEventHandler SelectedTrackChanged;
        /// <summary>
        /// VSQの編集コマンドが発行されたとき発生するイベント．
        /// </summary>
        public event EventHandler CommandExecuted;
        /// <summary>
        /// トラックの歌声合成が要求されたとき発生するイベント．
        /// </summary>
        public event RenderRequiredEventHandler RenderRequired;
        /// <summary>
        /// このコントロールの推奨最少表示高さが変わったとき発生するイベント．
        /// </summary>
        public event EventHandler PreferredMinHeightChanged;

        /// <summary>
        /// コンストラクタ．
        /// </summary>
        public TrackSelector(FormMain main_window)
        {
            this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
            InitializeComponent();
            mMainWindow = main_window;
            registerEventHandlers();
            setResources();
            mModifierKey = Keys.Control;
            mMenuMap[CurveType.VEL] = cmenuCurveVelocity;
            mMenuMap[CurveType.Accent] = cmenuCurveAccent;
            mMenuMap[CurveType.Decay] = cmenuCurveDecay;

            mMenuMap[CurveType.DYN] = cmenuCurveDynamics;
            mMenuMap[CurveType.VibratoRate] = cmenuCurveVibratoRate;
            mMenuMap[CurveType.VibratoDepth] = cmenuCurveVibratoDepth;

            mMenuMap[CurveType.reso1amp] = cmenuCurveReso1Amp;
            mMenuMap[CurveType.reso1bw] = cmenuCurveReso1BW;
            mMenuMap[CurveType.reso1freq] = cmenuCurveReso1Freq;
            mMenuMap[CurveType.reso2amp] = cmenuCurveReso2Amp;
            mMenuMap[CurveType.reso2bw] = cmenuCurveReso2BW;
            mMenuMap[CurveType.reso2freq] = cmenuCurveReso2Freq;
            mMenuMap[CurveType.reso3amp] = cmenuCurveReso3Amp;
            mMenuMap[CurveType.reso3bw] = cmenuCurveReso3BW;
            mMenuMap[CurveType.reso3freq] = cmenuCurveReso3Freq;
            mMenuMap[CurveType.reso4amp] = cmenuCurveReso4Amp;
            mMenuMap[CurveType.reso4bw] = cmenuCurveReso4BW;
            mMenuMap[CurveType.reso4freq] = cmenuCurveReso4Freq;

            mMenuMap[CurveType.harmonics] = cmenuCurveHarmonics;
            mMenuMap[CurveType.BRE] = cmenuCurveBreathiness;
            mMenuMap[CurveType.BRI] = cmenuCurveBrightness;
            mMenuMap[CurveType.CLE] = cmenuCurveClearness;
            mMenuMap[CurveType.OPE] = cmenuCurveOpening;
            mMenuMap[CurveType.GEN] = cmenuCurveGenderFactor;

            mMenuMap[CurveType.POR] = cmenuCurvePortamentoTiming;
            mMenuMap[CurveType.PIT] = cmenuCurvePitchBend;
            mMenuMap[CurveType.PBS] = cmenuCurvePitchBendSensitivity;

            mMenuMap[CurveType.fx2depth] = cmenuCurveEffect2Depth;
            mMenuMap[CurveType.Env] = cmenuCurveEnvelope;
        }

        /// <summary>
        /// 表示するコントロールのカーブの種類を、AppManager.EditorConfigの設定に応じて更新します
        /// </summary>
        public void updateVisibleCurves()
        {
            mViewingCurves.Clear();
            if (AppManager.editorConfig.CurveVisibleVelocity) {
                mViewingCurves.Add(CurveType.VEL);
            }
            if (AppManager.editorConfig.CurveVisibleAccent) {
                mViewingCurves.Add(CurveType.Accent);
            }
            if (AppManager.editorConfig.CurveVisibleDecay) {
                mViewingCurves.Add(CurveType.Decay);
            }
            if (AppManager.editorConfig.CurveVisibleEnvelope) {
                mViewingCurves.Add(CurveType.Env);
            }
            if (AppManager.editorConfig.CurveVisibleDynamics) {
                mViewingCurves.Add(CurveType.DYN);
            }
            if (AppManager.editorConfig.CurveVisibleBreathiness) {
                mViewingCurves.Add(CurveType.BRE);
            }
            if (AppManager.editorConfig.CurveVisibleBrightness) {
                mViewingCurves.Add(CurveType.BRI);
            }
            if (AppManager.editorConfig.CurveVisibleClearness) {
                mViewingCurves.Add(CurveType.CLE);
            }
            if (AppManager.editorConfig.CurveVisibleOpening) {
                mViewingCurves.Add(CurveType.OPE);
            }
            if (AppManager.editorConfig.CurveVisibleGendorfactor) {
                mViewingCurves.Add(CurveType.GEN);
            }
            if (AppManager.editorConfig.CurveVisiblePortamento) {
                mViewingCurves.Add(CurveType.POR);
            }
            if (AppManager.editorConfig.CurveVisiblePit) {
                mViewingCurves.Add(CurveType.PIT);
            }
            if (AppManager.editorConfig.CurveVisiblePbs) {
                mViewingCurves.Add(CurveType.PBS);
            }
            if (AppManager.editorConfig.CurveVisibleVibratoRate) {
                mViewingCurves.Add(CurveType.VibratoRate);
            }
            if (AppManager.editorConfig.CurveVisibleVibratoDepth) {
                mViewingCurves.Add(CurveType.VibratoDepth);
            }
            if (AppManager.editorConfig.CurveVisibleHarmonics) {
                mViewingCurves.Add(CurveType.harmonics);
            }
            if (AppManager.editorConfig.CurveVisibleFx2Depth) {
                mViewingCurves.Add(CurveType.fx2depth);
            }
            if (AppManager.editorConfig.CurveVisibleReso1) {
                mViewingCurves.Add(CurveType.reso1freq);
                mViewingCurves.Add(CurveType.reso1bw);
                mViewingCurves.Add(CurveType.reso1amp);
            }
            if (AppManager.editorConfig.CurveVisibleReso2) {
                mViewingCurves.Add(CurveType.reso2freq);
                mViewingCurves.Add(CurveType.reso2bw);
                mViewingCurves.Add(CurveType.reso2amp);
            }
            if (AppManager.editorConfig.CurveVisibleReso3) {
                mViewingCurves.Add(CurveType.reso3freq);
                mViewingCurves.Add(CurveType.reso3bw);
                mViewingCurves.Add(CurveType.reso3amp);
            }
            if (AppManager.editorConfig.CurveVisibleReso4) {
                mViewingCurves.Add(CurveType.reso4freq);
                mViewingCurves.Add(CurveType.reso4bw);
                mViewingCurves.Add(CurveType.reso4amp);
            }
        }

        /// <summary>
        /// メニューアイテムから，そのアイテムが担当しているカーブ種類を取得します
        /// </summary>
        private CurveType getCurveTypeFromMenu(ToolStripMenuItem menu)
        {
            foreach (var curve in mMenuMap.Keys) {
                ToolStripMenuItem search = mMenuMap[curve];
                if (menu == search) {
                    return curve;
                }
            }
            return CurveType.Empty;
        }

        #region java.awt.Component
        public void setBounds(int x, int y, int width, int height)
        {
            base.Bounds = new System.Drawing.Rectangle(x, y, width, height);
        }

        public void setBounds(cadencii.java.awt.Rectangle rc)
        {
            base.Bounds = new System.Drawing.Rectangle(rc.x, rc.y, rc.width, rc.height);
        }

        public cadencii.java.awt.Cursor getCursor()
        {
            System.Windows.Forms.Cursor c = base.Cursor;
            cadencii.java.awt.Cursor ret = null;
            if (c.Equals(System.Windows.Forms.Cursors.Arrow)) {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.DEFAULT_CURSOR);
            } else if (c.Equals(System.Windows.Forms.Cursors.Cross)) {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.CROSSHAIR_CURSOR);
            } else if (c.Equals(System.Windows.Forms.Cursors.Default)) {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.DEFAULT_CURSOR);
            } else if (c.Equals(System.Windows.Forms.Cursors.Hand)) {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.HAND_CURSOR);
            } else if (c.Equals(System.Windows.Forms.Cursors.IBeam)) {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.TEXT_CURSOR);
            } else if (c.Equals(System.Windows.Forms.Cursors.PanEast)) {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.E_RESIZE_CURSOR);
            } else if (c.Equals(System.Windows.Forms.Cursors.PanNE)) {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.NE_RESIZE_CURSOR);
            } else if (c.Equals(System.Windows.Forms.Cursors.PanNorth)) {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.N_RESIZE_CURSOR);
            } else if (c.Equals(System.Windows.Forms.Cursors.PanNW)) {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.NW_RESIZE_CURSOR);
            } else if (c.Equals(System.Windows.Forms.Cursors.PanSE)) {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.SE_RESIZE_CURSOR);
            } else if (c.Equals(System.Windows.Forms.Cursors.PanSouth)) {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.S_RESIZE_CURSOR);
            } else if (c.Equals(System.Windows.Forms.Cursors.PanSW)) {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.SW_RESIZE_CURSOR);
            } else if (c.Equals(System.Windows.Forms.Cursors.PanWest)) {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.W_RESIZE_CURSOR);
            } else if (c.Equals(System.Windows.Forms.Cursors.SizeAll)) {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.MOVE_CURSOR);
            } else {
                ret = new cadencii.java.awt.Cursor(cadencii.java.awt.Cursor.CUSTOM_CURSOR);
            }
            ret.cursor = c;
            return ret;
        }

        public void setCursor(cadencii.java.awt.Cursor value)
        {
            base.Cursor = value.cursor;
        }

#if COMPONENT_ENABLE_TOOL_TIP_TEXT
        public void setToolTipText( String value )
        {
            base.ToolTipText = value;
        }

        public String getToolTipText()
        {
            return base.ToolTipText;
        }
#endif

#if COMPONENT_PARENT_AS_OWNERITEM
        public Object getParent() {
            return base.OwnerItem;
        }
#else
        public Object getParent()
        {
            return base.Parent;
        }
#endif

        public string getName()
        {
            return base.Name;
        }

        public void setName(string value)
        {
            base.Name = value;
        }

#if COMPONENT_ENABLE_LOCATION
        public cadencii.java.awt.Point getLocationOnScreen()
        {
            System.Drawing.Point p = base.PointToScreen(base.Location);
            return new cadencii.java.awt.Point(p.X, p.Y);
        }

        public cadencii.java.awt.Point getLocation()
        {
            System.Drawing.Point loc = this.Location;
            return new cadencii.java.awt.Point(loc.X, loc.Y);
        }

        public void setLocation(int x, int y)
        {
            base.Location = new System.Drawing.Point(x, y);
        }

        public void setLocation(cadencii.java.awt.Point p)
        {
            base.Location = new System.Drawing.Point(p.x, p.y);
        }
#endif

        public cadencii.java.awt.Rectangle getBounds()
        {
            System.Drawing.Rectangle r = base.Bounds;
            return new cadencii.java.awt.Rectangle(r.X, r.Y, r.Width, r.Height);
        }

#if COMPONENT_ENABLE_X
        public int getX() {
            return base.Left;
        }
#endif

#if COMPONENT_ENABLE_Y
        public int getY() {
            return base.Top;
        }
#endif

        public int getWidth()
        {
            return base.Width;
        }

        public int getHeight()
        {
            return base.Height;
        }

        public cadencii.java.awt.Dimension getSize()
        {
            return new cadencii.java.awt.Dimension(base.Size.Width, base.Size.Height);
        }

        public void setSize(int width, int height)
        {
            base.Size = new System.Drawing.Size(width, height);
        }

        public void setSize(cadencii.java.awt.Dimension d)
        {
            setSize(d.width, d.height);
        }

        public void setBackground(cadencii.java.awt.Color color)
        {
            base.BackColor = System.Drawing.Color.FromArgb(color.getRed(), color.getGreen(), color.getBlue());
        }

        public cadencii.java.awt.Color getBackground()
        {
            return new cadencii.java.awt.Color(base.BackColor.R, base.BackColor.G, base.BackColor.B);
        }

        public void setForeground(cadencii.java.awt.Color color)
        {
            base.ForeColor = color.color;
        }

        public cadencii.java.awt.Color getForeground()
        {
            return new cadencii.java.awt.Color(base.ForeColor.R, base.ForeColor.G, base.ForeColor.B);
        }

        public bool isEnabled()
        {
            return base.Enabled;
        }

        public void setEnabled(bool value)
        {
            base.Enabled = value;
        }

        public void requestFocus()
        {
            base.Focus();
        }

        public bool isFocusOwner()
        {
            return base.Focused;
        }

        public void setPreferredSize(cadencii.java.awt.Dimension size)
        {
            base.Size = new System.Drawing.Size(size.width, size.height);
        }

        public cadencii.java.awt.Font getFont()
        {
            return new cadencii.java.awt.Font(base.Font);
        }

        public void setFont(cadencii.java.awt.Font font)
        {
            if (font == null) {
                return;
            }
            if (font.font == null) {
                return;
            }
            base.Font = font.font;
        }
        #endregion

        #region common APIs of org.kbinani.*
        // root implementation is in BForm.cs
        public java.awt.Point pointToScreen(java.awt.Point point_on_client)
        {
            java.awt.Point p = getLocationOnScreen();
            return new java.awt.Point(p.x + point_on_client.x, p.y + point_on_client.y);
        }

        public java.awt.Point pointToClient(java.awt.Point point_on_screen)
        {
            java.awt.Point p = getLocationOnScreen();
            return new java.awt.Point(point_on_screen.x - p.x, point_on_screen.y - p.y);
        }

        public Object getTag()
        {
            return base.Tag;
        }

        public void setTag(Object value)
        {
            base.Tag = value;
        }
        #endregion

        private LineGraphDrawer getGraphDrawer()
        {
            if (mGraphDrawer == null) {
                mGraphDrawer = new LineGraphDrawer(LineGraphDrawer.TYPE_STEP);
            }
            return mGraphDrawer;
        }

        /// <summary>
        /// 描画幅が2ピクセルのストロークを取得します
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

        public void applyLanguage()
        {
        }

        public void applyFont(java.awt.Font font)
        {
            Util.applyFontRecurse(this, font);
            Util.applyContextMenuFontRecurse(cmenuSinger, font);
            Util.applyContextMenuFontRecurse(cmenuCurve, font);
        }

        private int getMaxColumns()
        {
            int max_columns = AppManager.keyWidth / AppManager.MIN_KEY_WIDTH;
            if (max_columns < 1) {
                max_columns = 1;
            }
            return max_columns;
        }

        public int getRowsPerColumn()
        {
            int max_columns = getMaxColumns();
            int row_per_column = mViewingCurves.Count / max_columns;
            if (row_per_column * max_columns < mViewingCurves.Count) {
                row_per_column++;
            }
            return row_per_column;
        }

        /// <summary>
        /// このコントロールの推奨最小表示高さを取得します
        /// </summary>
        public int getPreferredMinSize()
        {
            return HEIGHT_WITHOUT_CURVE + UNIT_HEIGHT_PER_CURVE * getRowsPerColumn();
        }

        /// <summary>
        /// このコントロールの親ウィンドウを取得します
        /// </summary>
        /// <returns></returns>
        public FormMain getMainForm()
        {
            return mMainWindow;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="command"></param>
        /// <param name="register">Undo/Redo用バッファにExecuteの結果を格納するかどうかを指定するフラグ</param>
        private void executeCommand(CadenciiCommand command, bool register)
        {
            if (register) {
                AppManager.editHistory.register(AppManager.getVsqFile().executeCommand(command));
            } else {
                AppManager.getVsqFile().executeCommand(command);
            }
            try {
                if (CommandExecuted != null) {
                    CommandExecuted.Invoke(this, new EventArgs());
                }
            } catch (Exception ex) {
                serr.println("TrackSelector#executeCommand; ex=" + ex);
            }
        }

        public ValuePair<int, int> getSelectedRegion()
        {
            int x0 = AppManager.mCurveSelectedInterval.getStart();
            int x1 = AppManager.mCurveSelectedInterval.getEnd();
            int min = Math.Min(x0, x1);
            int max = Math.Max(x0, x1);
            return new ValuePair<int, int>(min, max);
        }

        /// <summary>
        /// 現在最前面に表示され，編集可能となっているカーブの種類を取得または設定します
        /// </summary>
        public CurveType getSelectedCurve()
        {
            return mSelectedCurve;
        }

        public void setSelectedCurve(CurveType value)
        {
            CurveType old = mSelectedCurve;
            mSelectedCurve = value;
            if (!old.equals(mSelectedCurve)) {
                mLastSelectedCurve = old;
                try {
                    if (SelectedCurveChanged != null) {
                        SelectedCurveChanged.Invoke(this, mSelectedCurve);
                    }
                } catch (Exception ex) {
                    serr.println("TrackSelector#setSelectedCurve; ex=" + ex);
                }
            }
        }

        /// <summary>
        /// エディタのy方向の位置から，カーブの値を求めます
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public int valueFromYCoord(int y)
        {
            int max = 127;
            int min = 0;
            if (mSelectedCurve.equals(CurveType.VEL)) {
                int selected = AppManager.getSelected();
                if (AppManager.mDrawIsUtau[selected - 1]) {
                    max = UstEvent.MAX_INTENSITY;
                    min = UstEvent.MIN_INTENSITY;
                } else {
                    max = mSelectedCurve.getMaximum();
                    min = mSelectedCurve.getMinimum();
                }
            } else {
                max = mSelectedCurve.getMaximum();
                min = mSelectedCurve.getMinimum();
            }
            return valueFromYCoord(y, max, min);
        }

        public int valueFromYCoord(int y, int max, int min)
        {
            int oy = getHeight() - 42;
            float order = getGraphHeight() / (float)(max - min);
            return (int)((oy - y) / order) + min;
        }

        public int yCoordFromValue(int value)
        {
            int max = 127;
            int min = 0;
            if (mSelectedCurve.equals(CurveType.VEL)) {
                int selected = AppManager.getSelected();
                if (AppManager.mDrawIsUtau[selected - 1]) {
                    max = UstEvent.MAX_INTENSITY;
                    min = UstEvent.MIN_INTENSITY;
                } else {
                    max = mSelectedCurve.getMaximum();
                    min = mSelectedCurve.getMinimum();
                }
            } else {
                max = mSelectedCurve.getMaximum();
                min = mSelectedCurve.getMinimum();
            }
            return yCoordFromValue(value, max, min);
        }

        public int yCoordFromValue(int value, int max, int min)
        {
            int oy = getHeight() - 42;
            float order = getGraphHeight() / (float)(max - min);
            return oy - (int)((value - min) * order);
        }

        /// <summary>
        /// カーブエディタを表示するかどうかを取得します
        /// </summary>
        public bool isCurveVisible()
        {
            return mCurveVisible;
        }

        /// <summary>
        /// カーブエディタを表示するかどうかを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setCurveVisible(bool value)
        {
            mCurveVisible = value;
        }

        private Graphics2D getGraphics()
        {
            if (mGraphics == null) {
                mGraphics = new Graphics2D(null);
            }
            return mGraphics;
        }

        /// <summary>
        /// オーバーライドされます
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics2D g = getGraphics();
            g.nativeGraphics = e.Graphics;
            paint(g);
        }

        /// <summary>
        /// x軸方向の表示倍率。pixel/clock
        /// </summary>
        public float getScaleY()
        {
            int max = mSelectedCurve.getMaximum();
            int min = mSelectedCurve.getMinimum();
            int oy = getHeight() - 42;
            return getGraphHeight() / (float)(max - min);
        }

        /// <summary>
        /// 指定したコントロールカーブの名前を表示するボックスが，どの位置にあるかを計算します．
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public Rectangle getRectFromCurveType(CurveType curve)
        {
            int row_per_column = getRowsPerColumn();

            int centre = (getGraphHeight() + UNIT_HEIGHT_PER_CURVE) / 2 + 3;
            int index = 100;
            for (int i = 0; i < mViewingCurves.Count; i++) {
                if (mViewingCurves[i].equals(curve)) {
                    index = i;
                    break;
                }
            }
            int ix = index / row_per_column;
            int iy = index - ix * row_per_column;
            int x = 7 + ix * AppManager.MIN_KEY_WIDTH;
            int y = centre - row_per_column * UNIT_HEIGHT_PER_CURVE / 2 + 2 + UNIT_HEIGHT_PER_CURVE * iy;
            int min_size = getPreferredMinSize();
            if (mLastPreferredMinHeight != min_size) {
                try {
                    if (PreferredMinHeightChanged != null) {
                        PreferredMinHeightChanged.Invoke(this, new EventArgs());
                    }
                } catch (Exception ex) {
                    serr.println("TrackSelector#getRectFromCurveType; ex=" + ex);
                }
                mLastPreferredMinHeight = min_size;
            }
            return new Rectangle(x, y, 56, 14);
        }

        /// <summary>
        /// コントロール画面を描画します
        /// </summary>
        /// <param name="graphics"></param>
        public void paint(Graphics graphics)
        {
            int width = getWidth();
            int height = getHeight();
            int graph_height = getGraphHeight();
            Dimension size = new Dimension(width + 2, height);
            Graphics2D g = (Graphics2D)graphics;
            Color brs_string = Color.black;
            Color rect_curve = new Color(41, 46, 55);
            int centre = HEADER + graph_height / 2;
            g.setColor(PortUtil.DarkGray);
            g.fillRect(0, size.height - 2 * OFFSET_TRACK_TAB, size.width, 2 * OFFSET_TRACK_TAB);
            int numeric_view = mMouseValue;
            Point p = pointToClient(PortUtil.getMousePosition());
            Point mouse = new Point(p.x, p.y);
            VsqFileEx vsq = AppManager.getVsqFile();
            int selected = AppManager.getSelected();
            int key_width = AppManager.keyWidth;
            int stdx = AppManager.mMainWindowController.getStartToDrawX();
            int graph_max_y = HEADER + graph_height;
            int graph_min_y = HEADER;

            try {
                #region SINGER
                Shape last = g.getClip();
                g.setColor(AppManager.COLOR_BORDER);
                g.drawLine(0, size.height - 2 * OFFSET_TRACK_TAB,
                            size.width - 0, size.height - 2 * OFFSET_TRACK_TAB);
                g.setFont(AppManager.baseFont8);
                g.setColor(brs_string);
                g.drawString(
                    "SINGER",
                    9,
                    size.height - 2 * OFFSET_TRACK_TAB + OFFSET_TRACK_TAB / 2 - AppManager.baseFont8OffsetHeight);
                g.clipRect(key_width, size.height - 2 * OFFSET_TRACK_TAB,
                            size.width - key_width, OFFSET_TRACK_TAB);
                VsqTrack vsq_track = null;
                if (vsq != null) {
                    vsq_track = vsq.Track[selected];
                }
                if (vsq_track != null) {
                    int ycoord = size.height - 2 * OFFSET_TRACK_TAB + 1;

                    // 左端での歌手を最初に描画
                    int x_at_left = AppManager.keyWidth + AppManager.keyOffset;
                    int clock_at_left = AppManager.clockFromXCoord(x_at_left);
                    VsqEvent singer_at_left = vsq_track.getSingerEventAt(clock_at_left);
                    if (singer_at_left != null) {
                        Rectangle rc =
                            new Rectangle(x_at_left, ycoord,
                                           SINGER_ITEM_WIDTH, OFFSET_TRACK_TAB - 2);
                        g.setColor(Color.lightGray);
                        g.fillRect(rc.x, rc.y, rc.width, rc.height);
                        g.setColor(COLOR_SINGERBOX_BORDER);
                        g.drawRect(rc.x, rc.y, rc.width, rc.height);
                        g.setColor(brs_string);
                        g.drawString(
                            singer_at_left.ID.IconHandle.IDS,
                            rc.x, rc.y + OFFSET_TRACK_TAB / 2 - AppManager.baseFont8OffsetHeight);
                    }

                    // 歌手設定を順に描画
                    int event_count = vsq_track.getEventCount();
                    for (int i = 0; i < event_count; i++) {
                        VsqEvent ve = vsq_track.getEvent(i);
                        if (ve.ID.type != VsqIDType.Singer) {
                            continue;
                        }
                        int clock = ve.Clock;
                        IconHandle singer_handle = (IconHandle)ve.ID.IconHandle;
                        int x = AppManager.xCoordFromClocks(clock);
                        if (x < x_at_left) {
                            continue;
                        }
                        Rectangle rc =
                            new Rectangle(x, ycoord,
                                           SINGER_ITEM_WIDTH, OFFSET_TRACK_TAB - 2);
                        if (AppManager.itemSelection.isEventContains(selected, ve.InternalID)) {
                            g.setColor(AppManager.getHilightColor());
                        } else {
                            g.setColor(Color.white);
                        }
                        g.fillRect(rc.x, rc.y, rc.width, rc.height);
                        g.setColor(COLOR_SINGERBOX_BORDER);
                        g.drawRect(rc.x, rc.y, rc.width, rc.height);
                        g.setColor(brs_string);
                        g.drawString(
                            singer_handle.IDS,
                            rc.x, rc.y + OFFSET_TRACK_TAB / 2 - AppManager.baseFont8OffsetHeight);
                    }
                }
                g.setClip(last);
                #endregion

                #region トラック選択欄
                int selecter_width = getSelectorWidth();
                g.setColor(AppManager.COLOR_BORDER);
                g.drawLine(0, size.height - OFFSET_TRACK_TAB,
                            size.width, size.height - OFFSET_TRACK_TAB);
                g.setColor(brs_string);
                g.drawString("TRACK", 9, size.height - OFFSET_TRACK_TAB + OFFSET_TRACK_TAB / 2 - AppManager.baseFont8OffsetHeight);
                if (vsq != null) {
                    for (int i = 0; i < AppManager.MAX_NUM_TRACK; i++) {
                        int x = key_width + i * selecter_width;
#if DEBUG
                        try {
#endif
                            drawTrackTab(g,
                                          new Rectangle(x, size.height - OFFSET_TRACK_TAB + 1, selecter_width, OFFSET_TRACK_TAB - 1),
                                          (i + 1 < vsq.Track.Count) ? (i + 1) + " " + vsq.Track[i + 1].getName() : "",
                                          (i == selected - 1),
                                          vsq_track.getCommon().PlayMode >= 0,
                                          AppManager.getRenderRequired(i + 1),
                                          AppManager.HILIGHT[i],
                                          AppManager.RENDER[i]);
#if DEBUG
                        } catch (Exception ex) {
                            AppManager.debugWriteLine("TrackSelector.DrawTo; ex=" + ex);
                        }
#endif
                    }
                }
                #endregion

                int clock_at_mouse = AppManager.clockFromXCoord(mouse.x);
                int pbs_at_mouse = 0;
                if (mCurveVisible) {
                    #region カーブエディタ
                    // カーブエディタの下の線
                    g.setColor(new Color(156, 161, 169));
                    g.drawLine(key_width, size.height - 42,
                                size.width - 3, size.height - 42);

                    // カーブエディタの上の線
                    g.setColor(new Color(46, 47, 50));
                    g.drawLine(key_width, HEADER,
                                size.width - 3, HEADER);

                    g.setColor(new Color(125, 123, 124));
                    g.drawLine(key_width, 0,
                                key_width, size.height - 1);

                    if (AppManager.isCurveSelectedIntervalEnabled()) {
                        int x0 = AppManager.xCoordFromClocks(AppManager.mCurveSelectedInterval.getStart());
                        int x1 = AppManager.xCoordFromClocks(AppManager.mCurveSelectedInterval.getEnd());
                        g.setColor(COLOR_A072R255G255B255);
                        g.fillRect(x0, HEADER, x1 - x0, graph_height);
                    }

                    #region 小節ごとのライン
                    if (vsq != null) {
                        int dashed_line_step = AppManager.getPositionQuantizeClock();
                        g.clipRect(key_width, HEADER, size.width - key_width, size.height - 2 * OFFSET_TRACK_TAB);
                        Color white100 = new Color(0, 0, 0, 100);
                        for (Iterator<VsqBarLineType> itr = vsq.getBarLineIterator(AppManager.clockFromXCoord(width)); itr.hasNext(); ) {
                            VsqBarLineType blt = itr.next();
                            int x = AppManager.xCoordFromClocks(blt.clock());
                            int local_clock_step = 480 * 4 / blt.getLocalDenominator();
                            if (blt.isSeparator()) {
                                g.setColor(white100);
                                g.drawLine(x, size.height - 42 - 1, x, 8 + 1);
                            } else {
                                g.setColor(white100);
                                g.drawLine(x, centre - 5, x, centre + 6);
                                Color pen = new Color(12, 12, 12);
                                g.setColor(pen);
                                g.drawLine(x, 8, x, 14);
                                g.drawLine(x, size.height - 43, x, size.height - 42 - 6);
                            }
                            if (dashed_line_step > 1 && AppManager.isGridVisible()) {
                                int numDashedLine = local_clock_step / dashed_line_step;
                                Color pen = new Color(65, 65, 65);
                                g.setColor(pen);
                                for (int i = 1; i < numDashedLine; i++) {
                                    int x2 = AppManager.xCoordFromClocks(blt.clock() + i * dashed_line_step);
                                    g.drawLine(x2, centre - 2, x2, centre + 3);
                                    g.drawLine(x2, 8, x2, 12);
                                    g.drawLine(x2, size.height - 43, x2, size.height - 43 - 4);
                                }
                            }
                        }
                        g.setClip(null);
                    }
                    #endregion

                    if (vsq_track != null) {
                        Color color = AppManager.getHilightColor();
                        Color front = new Color(color.getRed(), color.getGreen(), color.getBlue(), 150);
                        Color back = new Color(255, 249, 255, 44);
                        Color vel_color = new Color(64, 78, 30);

                        // 後ろに描くカーブ
                        if (mLastSelectedCurve.equals(CurveType.VEL) || mLastSelectedCurve.equals(CurveType.Accent) || mLastSelectedCurve.equals(CurveType.Decay)) {
                            drawVEL(g, vsq_track, back, false, mLastSelectedCurve);
                        } else if (mLastSelectedCurve.equals(CurveType.VibratoRate) || mLastSelectedCurve.equals(CurveType.VibratoDepth)) {
                            drawVibratoControlCurve(g, vsq_track, mLastSelectedCurve, back, false);
                        } else {
                            VsqBPList list_back = vsq_track.getCurve(mLastSelectedCurve.getName());
                            if (list_back != null) {
                                drawVsqBPList(g, list_back, back, false);
                            }
                        }

                        // 手前に描くカーブ
                        if (mSelectedCurve.equals(CurveType.VEL) || mSelectedCurve.equals(CurveType.Accent) || mSelectedCurve.equals(CurveType.Decay)) {
                            drawVEL(g, vsq_track, vel_color, true, mSelectedCurve);
                        } else if (mSelectedCurve.equals(CurveType.VibratoRate) || mSelectedCurve.equals(CurveType.VibratoDepth)) {
                            drawVibratoControlCurve(g, vsq_track, mSelectedCurve, front, true);
                        } else if (mSelectedCurve.equals(CurveType.Env)) {
                            drawEnvelope(g, selected, front);
                        } else {
                            VsqBPList list_front = vsq_track.getCurve(mSelectedCurve.getName());
                            if (list_front != null) {
                                drawVsqBPList(g, list_front, front, true);
                            }
                            if (mSelectedCurve.equals(CurveType.PIT)) {
                                #region PBSの値に応じて，メモリを記入する
                                System.Drawing.Drawing2D.SmoothingMode old = g.nativeGraphics.SmoothingMode;
                                g.nativeGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                                Color nrml = new Color(0, 0, 0, 190);
                                Color dash = new Color(0, 0, 0, 128);
                                Stroke nrml_stroke = new BasicStroke();
                                Stroke dash_stroke = new BasicStroke(1.0f, 0, 0, 10.0f, new float[] { 2.0f, 2.0f }, 0.0f);
                                VsqBPList pbs = vsq_track.MetaText.PBS;
                                pbs_at_mouse = pbs.getValue(clock_at_mouse);
                                int c = pbs.size();
                                int premeasure = vsq.getPreMeasureClocks();
                                int clock_start = AppManager.clockFromXCoord(key_width);
                                int clock_end = AppManager.clockFromXCoord(width);
                                if (clock_start < premeasure && premeasure < clock_end) {
                                    clock_start = premeasure;
                                }
                                int last_pbs = pbs.getValue(clock_start);
                                int last_clock = clock_start;
                                int ycenter = yCoordFromValue(0);
                                g.setColor(nrml);
                                g.drawLine(key_width, ycenter, width, ycenter);
                                for (int i = 0; i < c; i++) {
                                    int cl = pbs.getKeyClock(i);
                                    if (cl < clock_start) {
                                        continue;
                                    }
                                    if (clock_end < cl) {
                                        break;
                                    }
                                    int thispbs = pbs.getElement(i);
                                    if (last_pbs == thispbs) {
                                        continue;
                                    }
                                    // last_clockからclの範囲で，PBSの値がlas_pbs
                                    int max = last_pbs;
                                    int min = -last_pbs;
                                    int x1 = AppManager.xCoordFromClocks(last_clock);
                                    int x2 = AppManager.xCoordFromClocks(cl);
                                    for (int j = min + 1; j <= max - 1; j++) {
                                        if (j == 0) {
                                            continue;
                                        }
                                        int y = yCoordFromValue((int)(j * 8192 / (double)last_pbs));
                                        if (j % 2 == 0) {
                                            g.setColor(nrml);
                                            g.setStroke(nrml_stroke);
                                            g.drawLine(x1, y, x2, y);
                                        } else {
                                            g.setColor(dash);
                                            g.setStroke(dash_stroke);
                                            g.drawLine(x1, y, x2, y);
                                        }
                                    }
                                    g.setStroke(new BasicStroke());
                                    last_clock = cl;
                                    last_pbs = thispbs;
                                }
                                int max0 = last_pbs;
                                int min0 = -last_pbs;
                                int x10 = AppManager.xCoordFromClocks(last_clock);
                                int x20 = AppManager.xCoordFromClocks(clock_end);
                                for (int j = min0 + 1; j <= max0 - 1; j++) {
                                    if (j == 0) {
                                        continue;
                                    }
                                    int y = yCoordFromValue((int)(j * 8192 / (double)last_pbs));
                                    Color pen = dash;
                                    if (j % 2 == 0) {
                                        pen = nrml;
                                    }
                                    g.setColor(pen);
                                    g.drawLine(x10, y, x20, y);
                                }
                                g.nativeGraphics.SmoothingMode = old;
                                #endregion
                            }
                            drawAttachedCurve(g, vsq.AttachedCurves.get(AppManager.getSelected() - 1).get(mSelectedCurve));
                        }
                    }

                    if (AppManager.isWholeSelectedIntervalEnabled()) {
                        int start = AppManager.xCoordFromClocks(AppManager.mWholeSelectedInterval.getStart()) + 2;
                        int end = AppManager.xCoordFromClocks(AppManager.mWholeSelectedInterval.getEnd()) + 2;
                        g.setColor(COLOR_A098R000G000B000);
                        g.fillRect(start, HEADER, end - start, graph_height);
                    }

                    if (mMouseDowned) {
                        #region 選択されたツールに応じて描画
                        int value = valueFromYCoord(mouse.y);
                        if (clock_at_mouse < vsq.getPreMeasure()) {
                            clock_at_mouse = vsq.getPreMeasure();
                        }
                        int max = mSelectedCurve.getMaximum();
                        int min = mSelectedCurve.getMinimum();
                        if (value < min) {
                            value = min;
                        } else if (max < value) {
                            value = max;
                        }
                        EditTool tool = AppManager.getSelectedTool();
                        if (tool == EditTool.LINE) {
#if OLD_IMPL_MOUSE_TRACER
                            int xini = AppManager.xCoordFromClocks( m_line_start.x );
                            int yini = yCoordFromValue( m_line_start.y );
                            g.setColor( s_pen_050_140_150 );
                            g.drawLine( xini, yini, AppManager.xCoordFromClocks( clock_at_mouse ), yCoordFromValue( value ) );
#else
                            if (mMouseTracer.size() > 0) {
                                Point pt = mMouseTracer.iterator().First();
                                int xini = pt.x - stdx;
                                int yini = pt.y;
                                g.setColor(Color.ORANGE);
                                g.setStroke(getStroke2px());
                                g.nativeGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                                g.drawLine(xini, yini, AppManager.xCoordFromClocks(clock_at_mouse), yCoordFromValue(value));
                                g.nativeGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                                g.setStroke(getStrokeDefault());
                            }
#endif
                        } else if (tool == EditTool.PENCIL) {
                            if (mMouseTracer.size() > 0 && !AppManager.isCurveMode()) {
                                LineGraphDrawer d = getGraphDrawer();
                                d.clear();
                                d.setGraphics(g);
                                d.setBaseLineY(graph_max_y);
                                d.setFill(true);
                                d.setDotMode(LineGraphDrawer.DOTMODE_NO);
                                d.setDrawLine(false);
                                d.setFillColor(COLOR_MOUSE_TRACER);
                                foreach (var pt in mMouseTracer.iterator()) {
                                    int x = pt.x - stdx;
                                    int y = pt.y;
                                    if (y < graph_min_y) {
                                        y = graph_min_y;
                                    } else if (graph_max_y < y) {
                                        y = graph_max_y;
                                    }
                                    d.append(x, y);
                                }
                                d.flush();

                                /*Vector<Integer> ptx = new Vector<Integer>();
                                Vector<Integer> pty = new Vector<Integer>();
                                int height = getHeight() - 42;

                                int count = 0;
                                int lastx = 0;
                                int lasty = 0;
                                for ( Iterator<Point> itr = mMouseTracer.iterator(); itr.hasNext(); ) {
                                    Point pt = itr.next();
                                    int key = pt.x;
                                    int x = key - stdx;
                                    int y = pt.y;
                                    if ( y < 8 ) {
                                        y = 8;
                                    } else if ( height < y ) {
                                        y = height;
                                    }
                                    if ( count == 0 ) {
                                        lasty = height;
                                    }
                                    ptx.add( x ); pty.add( lasty );
                                    ptx.add( x ); pty.add( y );
                                    lastx = x;
                                    lasty = y;
                                    count++;
                                }

                                ptx.add( lastx ); pty.add( height );
                                g.setColor( new Color( 8, 166, 172, 127 ) );
                                int nPoints = ptx.size();
                                g.fillPolygon( PortUtil.convertIntArray( ptx.toArray( new Integer[] { } ) ),
                                               PortUtil.convertIntArray( pty.toArray( new Integer[] { } ) ),
                                               nPoints );*/
                            }
                        } else if (tool == EditTool.ERASER || tool == EditTool.ARROW) {
                            if (mMouseDownMode == MouseDownMode.CURVE_EDIT && mMouseMoved && AppManager.mCurveSelectingRectangle.width != 0) {
                                int xini = AppManager.xCoordFromClocks(AppManager.mCurveSelectingRectangle.x);
                                int xend = AppManager.xCoordFromClocks(AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width);
                                int x_start = Math.Min(xini, xend);
                                if (x_start < key_width) {
                                    x_start = key_width;
                                }
                                int x_end = Math.Max(xini, xend);
                                int yini = yCoordFromValue(AppManager.mCurveSelectingRectangle.y);
                                int yend = yCoordFromValue(AppManager.mCurveSelectingRectangle.y + AppManager.mCurveSelectingRectangle.height);
                                int y_start = Math.Min(yini, yend);
                                int y_end = Math.Max(yini, yend);
                                if (y_start < 8) y_start = 8;
                                if (y_end > height - 42 - 8) y_end = height - 42;
                                if (x_start < x_end) {
                                    g.setColor(COLOR_A144R255G255B255);
                                    g.fillRect(x_start, y_start, x_end - x_start, y_end - y_start);
                                }
                            } else if (mMouseDownMode == MouseDownMode.VEL_EDIT && mVelEditSelected.ContainsKey(mVelEditLastSelectedID)) {
                                if (mSelectedCurve.equals(CurveType.VEL)) {
                                    numeric_view = mVelEditSelected[mVelEditLastSelectedID].editing.ID.Dynamics;
                                } else if (mSelectedCurve.equals(CurveType.Accent)) {
                                    numeric_view = mVelEditSelected[mVelEditLastSelectedID].editing.ID.DEMaccent;
                                } else if (mSelectedCurve.equals(CurveType.Decay)) {
                                    numeric_view = mVelEditSelected[mVelEditLastSelectedID].editing.ID.DEMdecGainRate;
                                }
                            }
                        }
                        if (mMouseDownMode == MouseDownMode.SINGER_LIST && AppManager.getSelectedTool() != EditTool.ERASER) {
                            foreach (var item in AppManager.itemSelection.getEventIterator()) {
                                int x = AppManager.xCoordFromClocks(item.editing.Clock);
                                g.setColor(COLOR_SINGERBOX_BORDER_HILIGHT);
                                g.drawRect(x, size.height - 2 * OFFSET_TRACK_TAB + 1,
                                           SINGER_ITEM_WIDTH, OFFSET_TRACK_TAB - 2);
                            }
                        }
                        #endregion
                    }
                    #endregion
                }

                if (mCurveVisible) {
                    #region カーブの種類一覧
                    Font text_font = AppManager.baseFont9;
                    int text_font_height = AppManager.baseFont9Height;
                    int text_font_offset = AppManager.baseFont9OffsetHeight;
                    Color font_color_normal = Color.black;
                    g.setColor(new Color(212, 212, 212));
                    g.fillRect(0, 0, key_width, size.height - 2 * OFFSET_TRACK_TAB);

                    // 現在表示されているカーブの名前
                    g.setFont(text_font);
                    g.setColor(brs_string);
                    bool is_utau_mode = AppManager.mDrawIsUtau[selected - 1];
                    string name = (is_utau_mode && mSelectedCurve.equals(CurveType.VEL)) ? "INT" : mSelectedCurve.getName();
                    g.drawString(name, 7, text_font_height / 2 - text_font_offset + 1);

                    for (int i = 0; i < mViewingCurves.Count; i++) {
                        CurveType curve = mViewingCurves[i];
                        Rectangle rc = getRectFromCurveType(curve);
                        if (curve.equals(mSelectedCurve) || curve.equals(mLastSelectedCurve)) {
                            g.setColor(new Color(108, 108, 108));
                            g.fillRect(rc.x, rc.y, rc.width, rc.height);
                        }
                        g.setColor(rect_curve);
                        g.drawRect(rc.x, rc.y, rc.width, rc.height);
                        int rc_str_x = rc.x + 2;
                        int rc_str_y = rc.y + text_font_height / 2 - text_font_offset + 1;
                        string n = curve.getName();
                        if (is_utau_mode && curve.equals(CurveType.VEL)) {
                            n = "INT";
                        }
                        if (curve.equals(mSelectedCurve)) {
                            g.setColor(Color.white);
                            g.drawString(n, rc_str_x, rc_str_y);
                        } else {
                            g.setColor(font_color_normal);
                            g.drawString(n, rc_str_x, rc_str_y);
                        }
                    }
                    #endregion
                }

                #region 現在のマーカー
                int marker_x = AppManager.xCoordFromClocks(AppManager.getCurrentClock());
                if (key_width <= marker_x && marker_x <= size.width) {
                    g.setColor(Color.white);
                    g.setStroke(new BasicStroke(2f));
                    g.drawLine(marker_x, 0, marker_x, size.height - 18);
                    g.setStroke(new BasicStroke());
                }
                #endregion

                // マウス位置での値
                if (isInRect(mouse.x, mouse.y, new Rectangle(key_width, HEADER, width, graph_height)) &&
                     mMouseDownMode != MouseDownMode.PRE_UTTERANCE_MOVE &&
                     mMouseDownMode != MouseDownMode.OVERLAP_MOVE &&
                     mMouseDownMode != MouseDownMode.VEL_EDIT) {
                    int align = 1;
                    int valign = 0;
                    int shift = 50;
                    if (mSelectedCurve.equals(CurveType.PIT)) {
                        valign = 1;
                        shift = 100;
                    }
                    g.setFont(AppManager.baseFont10Bold);
                    g.setColor(Color.white);
                    PortUtil.drawStringEx(g,
                                           mMouseValue + "",
                                           AppManager.baseFont10Bold,
                                           new Rectangle(mouse.x - 100, mouse.y - shift, 100, 100),
                                           align,
                                           valign);
                    if (mSelectedCurve.equals(CurveType.PIT)) {
                        float delta_note = mMouseValue * pbs_at_mouse / 8192.0f;
                        align = 1;
                        valign = -1;
                        PortUtil.drawStringEx(g,
                                               PortUtil.formatDecimal("#0.00", delta_note),
                                               AppManager.baseFont10Bold,
                                               new Rectangle(mouse.x - 100, mouse.y, 100, 100),
                                               align,
                                               valign);
                    }
                }
            } catch (Exception ex) {
                serr.println("TrackSelector#paint; ex= " + ex);
            }
        }

        /// <summary>
        /// 指定したトラックのエンベロープ，先行発音，オーバーラップを画面に描画します
        /// </summary>
        /// <param name="g"></param>
        /// <param name="track"></param>
        /// <param name="fill_color"></param>
        private void drawEnvelope(Graphics2D g, int track_index, Color fill_color)
        {
            int key_width = AppManager.keyWidth;
            int width = getWidth();
            int height = getHeight();
            g.setClip(key_width, 0, width - key_width, height);
            int clock_start = AppManager.clockFromXCoord(key_width);
            int clock_end = AppManager.clockFromXCoord(width);

            VsqFileEx vsq = AppManager.getVsqFile();
            VsqTrack track = vsq.Track[track_index];
            VsqEvent itr_prev = null;
            VsqEvent itr_item = null;
            VsqEvent itr_next = null;
            Point mouse = pointToClient(PortUtil.getMousePosition());

            Color brs = fill_color;
            Point selected_point = new Point();
            bool selected_found = false;
            // yが範囲内なので，xも検索するときtrue
            bool search_mouse = (0 <= mouse.y && mouse.y <= height);
            IEnumerator<VsqEvent> itr = track.getNoteEventIterator().GetEnumerator();
            int dotwid = DOT_WID * 2 + 1;
            int tolerance = AppManager.editorConfig.PxTolerance;
            // 選択アイテムが1個以上あるので，検索するときtrue
            bool search_sel = AppManager.itemSelection.getEventCount() > 0;
            while (true) {
                bool draw_env_points = false;
                itr_prev = itr_item;
                itr_item = itr_next;
                if (itr.MoveNext()) {
                    itr_next = itr.Current;
                } else {
                    itr_next = null;
                }
                if (itr_item == null && itr_prev == null && itr_next == null) {
                    break;
                }
                if (itr_item == null) {
                    continue;
                }
                VsqEvent item = itr_item;
                if (item.Clock + item.ID.getLength() < clock_start) {
                    continue;
                }
                if (clock_end < item.Clock) {
                    break;
                }
                VsqEvent prev_item = itr_prev;
                VsqEvent next_item = itr_next;
                if (prev_item != null) {
                    if (prev_item.Clock + prev_item.ID.getLength() < item.Clock) {
                        // 直前の音符と接続していないのでnullにする
                        prev_item = null;
                    }
                }
                if (next_item != null) {
                    if (item.Clock + item.ID.getLength() < next_item.Clock) {
                        // 直後の音符と接続していないのでnullにする
                        next_item = null;
                    }
                }
                ByRef<int> preutterance = new ByRef<int>();
                ByRef<int> overlap = new ByRef<int>();
                Polygon points = getEnvelopePoints(vsq.TempoTable, prev_item, item, next_item, preutterance, overlap);
                if (mMouseDownMode == MouseDownMode.ENVELOPE_MOVE && item.InternalID == mEnvelopeEdigintID) {
                    selected_point = new Point(points.xpoints[mEnvelopePointKind], points.ypoints[mEnvelopePointKind]);
                    selected_found = true;
                }

                // 編集中のアイテムだったら描く
                // エンベロープ
                if (!draw_env_points) {
                    if (mMouseDownMode == MouseDownMode.ENVELOPE_MOVE && item.InternalID == mEnvelopeEdigintID) {
                        draw_env_points = true;
                    }
                }
                // 先行発音
                if (!draw_env_points) {
                    if (mMouseDownMode == MouseDownMode.PRE_UTTERANCE_MOVE && item.InternalID == mPreUtteranceEditingID) {
                        draw_env_points = true;
                    }
                }
                // オーバーラップ
                if (!draw_env_points) {
                    if (mMouseDownMode == MouseDownMode.OVERLAP_MOVE && item.InternalID == mOverlapEditingID) {
                        draw_env_points = true;
                    }
                }

                // マウスのx座標が範囲内なら描く
                if (!draw_env_points) {
                    if (search_mouse) {
                        if (points.xpoints[0] - tolerance <= mouse.x && mouse.x <= points.xpoints[points.npoints - 1] + tolerance) {
                            draw_env_points = true;
                        }
                    }
                }

                // 選択されてたら描く
                if (!draw_env_points && search_sel) {
                    if (AppManager.itemSelection.isEventContains(track_index, item.InternalID)) {
                        draw_env_points = true;
                    }
                }

                // 多角形
                g.setColor(brs);
                g.fillPolygon(points);
                g.setColor(Color.white);
                g.drawPolyline(points.xpoints, points.ypoints, points.npoints);

                if (draw_env_points) {
                    // データ点の表示
                    for (int i = 1; i < 6; i++) {
                        Point p = new Point(points.xpoints[i], points.ypoints[i]);
                        Rectangle rc = new Rectangle(p.x - DOT_WID, p.y - DOT_WID, dotwid, dotwid);
                        g.setColor(COLOR_BEZIER_DOT_NORMAL);
                        g.fillRect(rc.x, rc.y, rc.width, rc.height);
                        g.setColor(COLOR_BEZIER_DOT_NORMAL);
                        g.drawRect(rc.x, rc.y, rc.width, rc.height);
                    }

                    // 旗を描く
                    drawPreutteranceAndOverlap(
                        g,
                        preutterance.value, overlap.value,
                        item.UstEvent.getPreUtterance(), item.UstEvent.getVoiceOverlap());
                }
            }

            // 選択されている点のハイライト表示
            if (selected_found) {
                Rectangle rc = new Rectangle(selected_point.x - DOT_WID, selected_point.y - DOT_WID, dotwid, dotwid);
                g.setColor(AppManager.getHilightColor());
                g.fillRect(rc.x, rc.y, rc.width, rc.height);
                g.setColor(COLOR_BEZIER_DOT_NORMAL);
                g.drawRect(rc.x, rc.y, rc.width, rc.height);
            }

            g.setClip(null);
        }

        /// <summary>
        /// 先行発音，またはオーバーラップを表示する旗に描く文字列を取得します
        /// </summary>
        /// <param name="flag_is_pre_utterance">先行発音用の文字列を取得する場合にtrue，そうでなければfalseを指定します</param>
        /// <param name="value">先行発音，またはオーバーラップの値</param>
        /// <returns>旗に描くための文字列（Overlap: 0.00など）</returns>
        private static string getFlagTitle(bool flag_is_pre_utterance, float value)
        {
            if (flag_is_pre_utterance) {
                return "Pre Utterance: " + PortUtil.formatDecimal("0.00", value);
            } else {
                return "Overlap: " + PortUtil.formatDecimal("0.00", value);
            }
        }

        /// <summary>
        /// 指定した文字列を旗に書いたときの，旗のサイズを計算します
        /// </summary>
        /// <param name="flag_title"></param>
        private Dimension getFlagBounds(string flag_title)
        {
            if (mTextWidthPerLetter <= 0.0f) {
                Font font = AppManager.baseFont10;
                Dimension s = Util.measureString(flag_title + " ", font);
                mTextWidthPerLetter = s.width / (float)flag_title.Length;
                mTextHeight = s.height;
            }
            return new Dimension((int)(flag_title.Length * mTextWidthPerLetter), mTextHeight);
        }

        /// <summary>
        /// 先行発音とオーバーラップを表示する旗を描画します
        /// </summary>
        /// <param name="g"></param>
        /// <param name="px_preutterance"></param>
        /// <param name="px_overlap"></param>
        /// <param name="preutterance"></param>
        /// <param name="overlap"></param>
        private void drawPreutteranceAndOverlap(Graphics2D g, int px_preutterance, int px_overlap, float preutterance, float overlap)
        {
            int graph_height = getGraphHeight();
            g.setColor(PortUtil.Orange);
            g.drawLine(px_preutterance, HEADER + 1, px_preutterance, graph_height + HEADER);
            g.setColor(PortUtil.LawnGreen);
            g.drawLine(px_overlap, HEADER + 1, px_overlap, graph_height + HEADER);

            string s_pre = getFlagTitle(true, preutterance);
            string s_ovl = getFlagTitle(false, overlap);
            Font font = AppManager.baseFont10;
            int font_height = AppManager.baseFont10Height;
            int font_offset = AppManager.baseFont10OffsetHeight;
            Dimension pre_bounds = getFlagBounds(s_pre);
            Dimension ovl_bounds = getFlagBounds(s_ovl);

            Color pen = new Color(0, 0, 0, 50);
            Color transp = new Color(PortUtil.Orange.getRed(), PortUtil.Orange.getGreen(), PortUtil.Orange.getBlue(), 50);
            g.setColor(transp);
            g.fillRect(px_preutterance, OFFSET_PRE - FLAG_SPACE, pre_bounds.width, pre_bounds.height + FLAG_SPACE * 2);
            g.setColor(pen);
            g.drawRect(px_preutterance, OFFSET_PRE - FLAG_SPACE, pre_bounds.width, pre_bounds.height + FLAG_SPACE * 2);
            transp = new Color(PortUtil.LawnGreen.getRed(), PortUtil.LawnGreen.getGreen(), PortUtil.LawnGreen.getBlue(), 50);
            g.setColor(transp);
            g.fillRect(px_overlap, OFFSET_OVL - FLAG_SPACE, ovl_bounds.width, ovl_bounds.height + FLAG_SPACE * 2);
            g.setColor(pen);
            g.drawRect(px_overlap, OFFSET_OVL - FLAG_SPACE, ovl_bounds.width, ovl_bounds.height + FLAG_SPACE * 2);

            g.setFont(font);
            g.setColor(Color.black);
            g.drawString(s_pre, px_preutterance + 1, OFFSET_PRE + font_height / 2 - font_offset + 1);
            g.drawString(s_ovl, px_overlap + 1, OFFSET_OVL + font_height / 2 - font_offset + 1);
        }

        /// <summary>
        /// 画面上の指定した点に、コントロールカーブのデータ点があるかどうかを調べます
        /// </summary>
        /// <param name="locx">調べたい点の画面上のx座標</param>
        /// <param name="locy">調べたい点の画面上のy座標</param>
        /// <returns>データ点が見つかれば，データ点のid，そうでなければ-1を返します</returns>
        private long findDataPointAt(int locx, int locy)
        {
            if (mSelectedCurve.equals(CurveType.Accent) ||
                 mSelectedCurve.equals(CurveType.Decay) ||
                 mSelectedCurve.equals(CurveType.Env) ||
                 mSelectedCurve.equals(CurveType.VEL)) {
                return -1;
            }
            if (mSelectedCurve.equals(CurveType.VibratoDepth) ||
                 mSelectedCurve.equals(CurveType.VibratoRate)) {
                //TODO: この辺
            } else {
                VsqBPList list = AppManager.getVsqFile().Track[AppManager.getSelected()].getCurve(mSelectedCurve.getName());
                int count = list.size();
                int w = DOT_WID * 2 + 1;
                for (int i = 0; i < count; i++) {
                    int clock = list.getKeyClock(i);
                    VsqBPPair item = list.getElementB(i);
                    int x = AppManager.xCoordFromClocks(clock);
                    if (x + DOT_WID < AppManager.keyWidth) {
                        continue;
                    }
                    if (getWidth() < x - DOT_WID) {
                        break;
                    }
                    int y = yCoordFromValue(item.value);
                    Rectangle rc = new Rectangle(x - DOT_WID, y - DOT_WID, w, w);
                    if (isInRect(locx, locy, rc)) {
                        return item.id;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 画面上の指定した点に、エンベロープのポイントがあるかどうかを調べます
        /// </summary>
        /// <param name="locx">調べたい点の画面上のx座標</param>
        /// <param name="locy">調べたい点の画面上のy座標</param>
        /// <param name="internal_id">見つかったエンベロープ・ポイントを保持しているVsqEventのID</param>
        /// <param name="point_kind">見つかったエンベロープ・ポイントのタイプ。(p1,v1)なら1、(p2,v2)なら2，(p5,v5)なら3，(p3,v3)なら4，(p4,v4)なら5</param>
        /// <returns>見つかった場合は真を、そうでなければ偽を返します</returns>
        private bool findEnvelopePointAt(int locx, int locy, ByRef<int> internal_id, ByRef<int> point_kind)
        {
            return findEnvelopeCore(locx, locy, internal_id, point_kind, null);
        }

        /// <summary>
        /// 画面上の指定した位置に，先行発音またはオーバーラップ用の旗が表示されているかどうかを調べます
        /// </summary>
        /// <param name="locx">調べたい点の画面上のx座標</param>
        /// <param name="locy">調べたい点の画面上のy座標</param>
        /// <param name="internal_id">見つかったイベントを表現するVsqEventのInternalID</param>
        /// <param name="found_flag_was_overlap">見つかった旗がオーバーラップのものであった場合にtrue，それ以外はfalse</param>
        /// <returns>旗が見つかった場合にtrue，それ以外はfalseを返します</returns>
        private bool findPreUtteranceOrOverlapAt(int locx, int locy, ByRef<int> internal_id, ByRef<Boolean> found_flag_was_overlap)
        {
            return findEnvelopeCore(locx, locy, internal_id, null, found_flag_was_overlap);
        }

        /// <summary>
        /// findPreUtteranceOrOverlapAtとfindEnvelopePointAtから呼ばれるユーティリティ
        /// </summary>
        /// <param name="locx"></param>
        /// <param name="locy"></param>
        /// <param name="internal_id"></param>
        /// <param name="point_kind"></param>
        /// <param name="found_flag_was_overlap"></param>
        /// <returns></returns>
        private bool findEnvelopeCore(
            int locx, int locy,
            ByRef<int> internal_id,
            ByRef<int> point_kind, ByRef<Boolean> found_flag_was_overlap)
        {
            internal_id.value = -1;
            if (point_kind != null) {
                point_kind.value = -1;
            }

            int clock_start = AppManager.clockFromXCoord(AppManager.keyWidth);
            int clock_end = AppManager.clockFromXCoord(getWidth());
            int dotwid = DOT_WID * 2 + 1;
            VsqFileEx vsq = AppManager.getVsqFile();
            IEnumerator<VsqEvent> itr = vsq.Track[AppManager.getSelected()].getNoteEventIterator().GetEnumerator();
            VsqEvent itr_prev = null;
            VsqEvent itr_item = null;
            VsqEvent itr_next = null;
            ByRef<int> px_preutterance = new ByRef<int>();
            ByRef<int> px_overlap = new ByRef<int>();
            Dimension size = new Dimension();
            while (true) {
                itr_prev = itr_item;
                itr_item = itr_next;
                if (itr.MoveNext()) {
                    itr_next = itr.Current;
                } else {
                    itr_next = null;
                }
                if (itr_prev == null && itr_item == null && itr_next == null) {
                    break;
                }
                VsqEvent item = itr_item;
                if (item == null) {
                    continue;
                }
                if (item.Clock + item.ID.getLength() < clock_start) {
                    continue;
                }
                if (clock_end < item.Clock) {
                    break;
                }
                VsqEvent prev_item = itr_prev;
                VsqEvent next_item = itr_next;
                if (prev_item != null) {
                    if (prev_item.Clock + prev_item.ID.getLength() < item.Clock) {
                        // 直前の音符と接続していないのでnullにする
                        prev_item = null;
                    }
                }
                if (next_item != null) {
                    if (item.Clock + item.ID.getLength() < next_item.Clock) {
                        // 直後の音符と接続していないのでnullにする
                        next_item = null;
                    }
                }
                // エンベロープの点の座標を計算
                Polygon points = getEnvelopePoints(vsq.TempoTable, prev_item, item, next_item, px_preutterance, px_overlap);
                // エンベロープの点の当たり判定
                if (point_kind != null) {
                    for (int i = 5; i >= 1; i--) {
                        Point p = new Point(points.xpoints[i], points.ypoints[i]);
                        Rectangle rc = new Rectangle(p.x - DOT_WID, p.y - DOT_WID, dotwid, dotwid);
                        if (isInRect(locx, locy, rc)) {
                            internal_id.value = item.InternalID;
                            point_kind.value = i;
                            return true;
                        }
                    }
                }
                // 先行発音の旗の当たり判定
                if (found_flag_was_overlap != null) {
                    string title_preutterance = getFlagTitle(true, item.UstEvent.getPreUtterance());
                    size = getFlagBounds(title_preutterance);
                    if (Utility.isInRect(locx, locy, px_preutterance.value, OFFSET_PRE - FLAG_SPACE, size.width, size.height + FLAG_SPACE * 2)) {
                        internal_id.value = item.InternalID;
                        found_flag_was_overlap.value = false;
                        return true;
                    }
                    // オーバーラップ用の旗の当たり判定
                    string title_overlap = getFlagTitle(false, item.UstEvent.getVoiceOverlap());
                    size = getFlagBounds(title_overlap);
                    if (Utility.isInRect(locx, locy, px_overlap.value, OFFSET_OVL - FLAG_SPACE, size.width, size.height + FLAG_SPACE * 2)) {
                        internal_id.value = item.InternalID;
                        found_flag_was_overlap.value = true;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 指定したアイテムのエンベロープを画面に描画するための多角形を取得します
        /// </summary>
        /// <param name="tempo_table">クロックから秒時を計算するためのテンポテーブル</param>
        /// <param name="prev_item">直前にある音符イベント，直前が休符(UTAUでのR)の場合はnull</param>
        /// <param name="item">エンベロープを調べる対象の音符イベント</param>
        /// <param name="next_item">直後にある音符イベント，直後が休符(UTAUでのR)の場合はnull</param>
        /// <param name="px_pre_utteramce">先行発音を描画するための旗のx座標</param>
        /// <param name="px_overlap">オーバーラップを描画するための旗のx座標</param>
        /// <returns>指定した音符イベントのエンベロープを描画するための多角形．x方向の単位は画面上のピクセル単位，y方向の単位はエンベロープの値と同じ単位</returns>
        private Polygon getEnvelopePoints(
            TempoVector tempo_table,
            VsqEvent prev_item, VsqEvent item, VsqEvent next_item,
            ByRef<int> px_pre_utteramce, ByRef<int> px_overlap)
        {
            ByRef<Double> sec_env_start1 = new ByRef<Double>(0.0);
            ByRef<Double> sec_env_end1 = new ByRef<Double>(0.0);
            getEnvelopeRegion(tempo_table, prev_item, item, next_item, sec_env_start1, sec_env_end1);

            UstEvent ust_event1 = item.UstEvent;
            if (ust_event1 == null) {
                ust_event1 = new UstEvent();
            }
            UstEnvelope draw_target = ust_event1.getEnvelope();
            if (draw_target == null) {
                draw_target = new UstEnvelope();
            }
            double sec_pre_utterance1 = ust_event1.getPreUtterance() / 1000.0;
            double sec_overlap1 = ust_event1.getVoiceOverlap() / 1000.0;

            TempoVectorSearchContext context = new TempoVectorSearchContext();
            int px_env_start1 =
                AppManager.xCoordFromClocks(
                    (int)tempo_table.getClockFromSec(sec_env_start1.value, context));
            if (px_pre_utteramce != null) {
                px_pre_utteramce.value = px_env_start1;
            }
            double sec_p1 = sec_env_start1.value + draw_target.p1 / 1000.0;
            double sec_p2 = sec_env_start1.value + (draw_target.p1 + draw_target.p2) / 1000.0;
            double sec_p5 = sec_env_start1.value + (draw_target.p1 + draw_target.p2 + draw_target.p5) / 1000.0;
            double sec_p3 = sec_env_end1.value - (draw_target.p3 + draw_target.p4) / 1000.0;
            double sec_p4 = sec_env_end1.value - draw_target.p4 / 1000.0;
            int p1 = AppManager.xCoordFromClocks((int)tempo_table.getClockFromSec(sec_p1, context));
            int p2 = AppManager.xCoordFromClocks((int)tempo_table.getClockFromSec(sec_p2, context));
            int p5 = AppManager.xCoordFromClocks((int)tempo_table.getClockFromSec(sec_p5, context));
            int p3 = AppManager.xCoordFromClocks((int)tempo_table.getClockFromSec(sec_p3, context));
            int p4 = AppManager.xCoordFromClocks((int)tempo_table.getClockFromSec(sec_p4, context));
            int px_env_end1 = AppManager.xCoordFromClocks((int)tempo_table.getClockFromSec(sec_env_end1.value, context));
            if (px_overlap != null) {
                px_overlap.value =
                    AppManager.xCoordFromClocks(
                        (int)tempo_table.getClockFromSec(sec_env_start1.value + sec_overlap1, context));
            }
            int v1 = yCoordFromValue(draw_target.v1);
            int v2 = yCoordFromValue(draw_target.v2);
            int v3 = yCoordFromValue(draw_target.v3);
            int v4 = yCoordFromValue(draw_target.v4);
            int v5 = yCoordFromValue(draw_target.v5);
            int y = yCoordFromValue(0);
            return new Polygon(new int[] { px_env_start1, p1, p2, p5, p3, p4, px_env_end1 },
                                new int[] { y, v1, v2, v5, v3, v4, y },
                                7);
        }

        /// <summary>
        /// 前後の音符の有無や先行発音などにより，音符のエンベロープがどの範囲に及ぶかを調べます
        /// </summary>
        /// <param name="tempo_table">クロックを秒時に変換するためのテンポテーブル</param>
        /// <param name="item_prev">直前の音符．休符であればnullを指定する</param>
        /// <param name="item">調べる対象の音符</param>
        /// <param name="item_next">直後の音符．休符であればnullを指定する</param>
        /// <param name="env_start_sec">エンベロープの開始時刻(秒)</param>
        /// <param name="env_end_sec">エンベロープの終了時刻(秒)</param>
        private void getEnvelopeRegion(
            TempoVector tempo_table,
            VsqEvent item_prev, VsqEvent item, VsqEvent item_next,
            ByRef<Double> env_start_sec, ByRef<Double> env_end_sec)
        {
            double sec_start1 = tempo_table.getSecFromClock(item.Clock);
            double sec_end1 = tempo_table.getSecFromClock(item.Clock + item.ID.getLength());
            UstEvent ust_event1 = item.UstEvent;
            if (ust_event1 == null) {
                ust_event1 = new UstEvent();
            }
            UstEnvelope draw_target = ust_event1.getEnvelope();
            if (draw_target == null) {
                draw_target = new UstEnvelope();
            }
            double sec_pre_utterance1 = ust_event1.getPreUtterance() / 1000.0;
            double sec_overlap1 = ust_event1.getVoiceOverlap() / 1000.0;

            // 先行発音があることによる，この音符のエンベロープの実際の開始位置
            double sec_env_start1 = sec_start1 - sec_pre_utterance1;

            // 直後の音符の有る無しで，この音符のエンベロープの実際の終了位置が変わる
            double sec_env_end1 = sec_end1;
            if (item_next != null && item_next.UstEvent != null) {
                // 直後に音符がある場合
                UstEvent ust_event2 = item_next.UstEvent;
                double sec_pre_utterance2 = ust_event2.getPreUtterance() / 1000.0;
                double sec_overlap2 = ust_event2.getVoiceOverlap() / 1000.0;
                sec_env_end1 = sec_end1 - sec_pre_utterance2 + sec_overlap2;
            }

            env_start_sec.value = sec_env_start1;
            env_end_sec.value = sec_env_end1;
        }

        private void drawTrackTab(Graphics2D g, Rectangle destRect, string name, bool selected, bool enabled, bool render_required, Color hilight, Color render_button_hilight)
        {
            int x = destRect.x;
            int panel_width = render_required ? destRect.width - 10 : destRect.width;
            Color panel_color = enabled ? hilight : new Color(125, 123, 124);
            Color button_color = enabled ? render_button_hilight : new Color(125, 123, 124);
            Color panel_title = Color.black;
            Color button_title = selected ? Color.white : Color.black;
            Color border = selected ? Color.white : new Color(118, 123, 138);

            // 背景(選択されている場合)
            if (selected) {
                g.setColor(panel_color);
                g.fillRect(destRect.x, destRect.y, destRect.width, destRect.height);
                if (render_required && enabled) {
                    g.setColor(render_button_hilight);
                    g.fillRect(destRect.x + destRect.width - 10, destRect.y, 10, destRect.height);
                }
            }

            // 左縦線
            g.setColor(border);
            g.drawLine(destRect.x, destRect.y,
                        destRect.x, destRect.y + destRect.height - 1);
            if (PortUtil.getStringLength(name) > 0) {
                // 上横線
                g.setColor(border);
                g.drawLine(destRect.x + 1, destRect.y,
                            destRect.x + destRect.width, destRect.y);
            }
            if (render_required) {
                g.setColor(border);
                g.drawLine(destRect.x + destRect.width - 10, destRect.y,
                            destRect.x + destRect.width - 10, destRect.y + destRect.height - 1);
            }
            g.clipRect(destRect.x, destRect.y, destRect.width, destRect.height);
            string title = Utility.trimString(name, AppManager.baseFont8, panel_width);
            g.setFont(AppManager.baseFont8);
            g.setColor(panel_title);
            g.drawString(title, destRect.x + 2, destRect.y + destRect.height / 2 - AppManager.baseFont8OffsetHeight);
            if (render_required) {
                g.setColor(button_title);
                g.drawString("R", destRect.x + destRect.width - PX_WIDTH_RENDER, destRect.y + destRect.height / 2 - AppManager.baseFont8OffsetHeight);
            }
            if (selected) {
                g.setColor(border);
                g.drawLine(destRect.x + destRect.width - 1, destRect.y,
                            destRect.x + destRect.width - 1, destRect.y + destRect.height - 1);
                g.setColor(border);
                g.drawLine(destRect.x, destRect.y + destRect.height - 1,
                            destRect.x + destRect.width, destRect.y + destRect.height - 1);
            }
            g.setClip(null);
            g.setColor(AppManager.COLOR_BORDER);
            g.drawLine(destRect.x + destRect.width, destRect.y,
                        destRect.x + destRect.width, destRect.y + destRect.height - 1);
        }

        /// <summary>
        /// トラック選択部分の、トラック1個分の幅を調べます。pixel
        /// </summary>
        public int getSelectorWidth()
        {
            int draft = TRACK_SELECTOR_MAX_WIDTH;
            int maxTotalWidth = getWidth() - AppManager.keyWidth; // トラックの一覧を表示するのに利用できる最大の描画幅
            int numTrack = 1;
            VsqFileEx vsq = AppManager.getVsqFile();
            if (vsq != null) {
                numTrack = vsq.Track.Count;
            }
            if (draft * (numTrack - 1) <= maxTotalWidth) {
                return draft;
            } else {
                return (int)((maxTotalWidth) / (numTrack - 1.0f));
            }
        }

        /// <summary>
        /// コンポーネントを再描画する
        /// </summary>
        public void doInvalidate()
        {
            this.Invalidate();
        }

        /// <summary>
        /// ベロシティを、与えられたグラフィックgを用いて描画します
        /// </summary>
        /// <param name="g"></param>
        /// <param name="track"></param>
        /// <param name="color"></param>
        /// <param name="is_front"></param>
        /// <param name="type"></param>
        public void drawVEL(Graphics2D g, VsqTrack track, Color color, bool is_front, CurveType type)
        {
            Point mouse = pointToClient(PortUtil.getMousePosition());

            int HEADER = 8;
            int graph_height = getGraphHeight();
            // 描画する値の最大値
            int max = 100;
            // 描画する値の最小値
            int min = 0;

            int height = getHeight();
            int width = getWidth();
            int oy = height - 42;
            Shape last_clip = g.getClip();
            int stdx = AppManager.mMainWindowController.getStartToDrawX();
            int key_width = AppManager.keyWidth;
            int xoffset = key_width - stdx;
            g.clipRect(key_width, HEADER, width - key_width, graph_height);
            float scale = AppManager.mMainWindowController.getScaleX();
            int selected = AppManager.getSelected();

            g.setFont(AppManager.baseFont10Bold);
            bool cursor_should_be_hand = false;
            lock (AppManager.mDrawObjects) {
                List<DrawObject> target_list = AppManager.mDrawObjects[selected - 1];
                int count = target_list.Count;
                int i_start = AppManager.mDrawStartIndex[selected - 1];
                for (int i = i_start; i < count; i++) {
                    DrawObject dobj = target_list[i];
                    if (dobj.mType != DrawObjectType.Note) {
                        continue;
                    }
                    int x = dobj.mRectangleInPixel.x + xoffset;
                    if (x + VEL_BAR_WIDTH < 0) {
                        continue;
                    } else if (width < x) {
                        break;
                    } else {
                        int value = 0;
                        if (type.equals(CurveType.VEL)) {
                            if (AppManager.mDrawIsUtau[selected - 1]) {
                                value = dobj.mIntensity;
                                max = UstEvent.MAX_INTENSITY;
                                min = UstEvent.MIN_INTENSITY;
                            } else {
                                value = dobj.mVelocity;
                                max = 127;
                                min = 0;
                            }
                        } else if (type.equals(CurveType.Accent)) {
                            value = dobj.mAccent;
                            max = 100;
                            min = 0;
                        } else if (type.equals(CurveType.Decay)) {
                            value = dobj.mDecay;
                            max = 100;
                            min = 0;
                        }
                        //float order = (type.equals( CurveType.VEL )) ? graph_height / 127f : graph_height / 100f;

                        int y = oy - graph_height * (value - min) / (max - min);
                        if (is_front && AppManager.itemSelection.isEventContains(selected, dobj.mInternalID)) {
                            g.setColor(COLOR_A127R008G166B172);
                            g.fillRect(x, y, VEL_BAR_WIDTH, oy - y);
                            if (mMouseDownMode == MouseDownMode.VEL_EDIT) {
                                int editing = 0;
                                if (mVelEditSelected.ContainsKey(dobj.mInternalID)) {
                                    VsqEvent ve_editing = mVelEditSelected[dobj.mInternalID].editing;
                                    if (mSelectedCurve.equals(CurveType.VEL)) {
                                        if (AppManager.mDrawIsUtau[selected - 1]) {
                                            editing = ve_editing.UstEvent == null ? 100 : ve_editing.UstEvent.getIntensity();
                                        } else {
                                            editing = ve_editing.ID.Dynamics;
                                        }
                                    } else if (mSelectedCurve.equals(CurveType.Accent)) {
                                        editing = ve_editing.ID.DEMaccent;
                                    } else if (mSelectedCurve.equals(CurveType.Decay)) {
                                        editing = ve_editing.ID.DEMdecGainRate;
                                    }
                                    int edit_y = oy - graph_height * (editing - min) / (max - min);
                                    g.setColor(COLOR_A244R255G023B012);
                                    g.fillRect(x, edit_y, VEL_BAR_WIDTH, oy - edit_y);
                                    g.setColor(Color.white);
                                    g.drawString(editing + "", x + VEL_BAR_WIDTH, (edit_y > oy - 20) ? oy - 20 : edit_y);
                                }
                            }
                        } else {
                            g.setColor(color);
                            g.fillRect(x, y, VEL_BAR_WIDTH, oy - y);
                        }
                        if (mMouseDownMode == MouseDownMode.VEL_EDIT) {
                            cursor_should_be_hand = true;
                        } else {
                            if (AppManager.getSelectedTool() == EditTool.ARROW && is_front && isInRect(mouse.x, mouse.y, new Rectangle(x, y, VEL_BAR_WIDTH, oy - y))) {
                                cursor_should_be_hand = true;
                            }
                        }
                    }
                }
            }
            if (cursor_should_be_hand) {
                if (getCursor().getType() != java.awt.Cursor.HAND_CURSOR) {
                    setCursor(new java.awt.Cursor(java.awt.Cursor.HAND_CURSOR));
                }
            } else {
                if (getCursor().getType() != java.awt.Cursor.DEFAULT_CURSOR) {
                    setCursor(new java.awt.Cursor(java.awt.Cursor.DEFAULT_CURSOR));
                }
            }
            g.setClip(last_clip);
        }

        /// <summary>
        /// ベジエ曲線によるコントロールカーブを描画します
        /// </summary>
        /// <param name="g"></param>
        /// <param name="chains"></param>
        private void drawAttachedCurve(Graphics2D g, List<BezierChain> chains)
        {
#if DEBUG
            try {
#endif
                int visibleMinX = AppManager.keyWidth;
                int visibleMaxX = mMainWindow.pictPianoRoll.getWidth() + AppManager.keyWidth + AppManager.keyOffset;
                Color hilight = Color.blue;// AppManager.getHilightColor();
                int chains_count = chains.Count;
                for (int i = 0; i < chains_count; i++) {
                    BezierChain target_chain = chains[i];
                    int chain_id = target_chain.id;
                    if (target_chain.points.Count <= 0) {
                        continue;
                    }
                    BezierPoint next;
                    BezierPoint current = target_chain.points[0];
                    Point pxNext;
                    Point pxCurrent = getScreenCoord(current.getBase());
                    int target_chain_points_count = target_chain.points.Count;
                    bool breaked = false;
                    for (int j = 0; j < target_chain_points_count; j++) {
                        next = target_chain.points[j];
                        int next_x = AppManager.xCoordFromClocks((int)next.getBase().getX());
                        pxNext = new Point(next_x, yCoordFromValue((int)next.getBase().getY()));
                        Point pxControlCurrent = getScreenCoord(current.getControlRight());
                        Point pxControlNext = getScreenCoord(next.getControlLeft());

                        // ベジエ曲線本体を描く
                        if (isVisibleOnScreen(visibleMinX, visibleMaxX, pxCurrent.x, pxNext.x)) {
                            if (current.getControlRightType() == BezierControlType.None &&
                                 next.getControlLeftType() == BezierControlType.None) {
                                g.setColor(COLOR_BEZIER_CURVE);
                                g.drawLine(pxCurrent.x, pxCurrent.y, pxNext.x, pxNext.y);
                            } else {
                                Point ctrl1 = (current.getControlRightType() == BezierControlType.None) ? pxCurrent : pxControlCurrent;
                                Point ctrl2 = (next.getControlLeftType() == BezierControlType.None) ? pxNext : pxControlNext;
                                g.setColor(COLOR_BEZIER_CURVE);
                                PortUtil.drawBezier(g, pxCurrent.x, pxCurrent.y,
                                                        ctrl1.x, ctrl1.y,
                                                        ctrl2.x, ctrl2.y,
                                                        pxNext.x, pxNext.y);
                            }
                        }
                        int minX = pxCurrent.x;
                        int maxX = pxNext.x;

                        if (current.getControlRightType() != BezierControlType.None) {
                            if (isVisibleOnScreen(visibleMinX, visibleMaxX, pxCurrent.x, pxControlCurrent.x)) {
                                g.setColor(COLOR_BEZIER_AUXILIARY);
                                g.drawLine(pxCurrent.x, pxCurrent.y, pxControlCurrent.x, pxControlCurrent.y);
                            }
                            minX = Math.Min(minX, pxCurrent.x);
                            maxX = Math.Max(maxX, pxControlCurrent.x);
                        }
                        if (next.getControlLeftType() != BezierControlType.None) {
                            if (isVisibleOnScreen(visibleMinX, visibleMaxX, pxControlNext.x, pxNext.x)) {
                                g.setColor(COLOR_BEZIER_AUXILIARY);
                                g.drawLine(pxNext.x, pxNext.y, pxControlNext.x, pxControlNext.y);
                            }
                            minX = Math.Min(minX, pxControlNext.x);
                            maxX = Math.Max(maxX, pxNext.x);
                        }

                        if (visibleMaxX < minX) {
                            breaked = true;
                            break;
                        }

                        // 右コントロール点
                        if (current.getControlRightType() == BezierControlType.Normal) {
                            Rectangle rc = new Rectangle(pxControlCurrent.x - DOT_WID,
                                                          pxControlCurrent.y - DOT_WID,
                                                          DOT_WID * 2 + 1,
                                                          DOT_WID * 2 + 1);
                            if (chain_id == mEditingChainID && current.getID() == mEditingPointID) {
                                g.setColor(hilight);
                                g.fillOval(rc.x, rc.y, rc.width, rc.height);
                            } else {
                                g.setColor(COLOR_BEZIER_DOT_NORMAL);
                                g.fillOval(rc.x, rc.y, rc.width, rc.height);
                            }
                            g.setColor(COLOR_BEZIER_DOT_NORMAL_DARK);
                            g.drawOval(rc.x, rc.y, rc.width, rc.height);
                        }

                        // 左コントロール点
                        if (next.getControlLeftType() == BezierControlType.Normal) {
                            Rectangle rc = new Rectangle(pxControlNext.x - DOT_WID,
                                                          pxControlNext.y - DOT_WID,
                                                          DOT_WID * 2 + 1,
                                                          DOT_WID * 2 + 1);
                            if (chain_id == mEditingChainID && next.getID() == mEditingPointID) {
                                g.setColor(hilight);
                                g.fillOval(rc.x, rc.y, rc.width, rc.height);
                            } else {
                                g.setColor(COLOR_BEZIER_DOT_NORMAL);
                                g.fillOval(rc.x, rc.y, rc.width, rc.height);
                            }
                            g.setColor(COLOR_BEZIER_DOT_NORMAL_DARK);
                            g.drawOval(rc.x, rc.y, rc.width, rc.height);
                        }

                        // データ点
                        Rectangle rc2 = new Rectangle(pxCurrent.x - DOT_WID,
                                                        pxCurrent.y - DOT_WID,
                                                        DOT_WID * 2 + 1,
                                                        DOT_WID * 2 + 1);
                        if (chain_id == mEditingChainID && current.getID() == mEditingPointID) {
                            g.setColor(hilight);
                            g.fillRect(rc2.x, rc2.y, rc2.width, rc2.height);
                        } else {
                            g.setColor(COLOR_BEZIER_DOT_BASE);
                            g.fillRect(rc2.x, rc2.y, rc2.width, rc2.height);
                        }
                        g.setColor(COLOR_BEZIER_DOT_BASE_DARK);
                        g.drawRect(rc2.x, rc2.y, rc2.width, rc2.height);
                        pxCurrent = pxNext;
                        current = next;
                    }
                    if (!breaked) {
                        next = target_chain.points[target_chain.points.Count - 1];
                        pxNext = getScreenCoord(next.getBase());
                        Rectangle rc_last = new Rectangle(pxNext.x - DOT_WID,
                                                           pxNext.y - DOT_WID,
                                                           DOT_WID * 2 + 1,
                                                           DOT_WID * 2 + 1);
                        if (chain_id == mEditingChainID && next.getID() == mEditingPointID) {
                            g.setColor(hilight);
                            g.fillRect(rc_last.x, rc_last.y, rc_last.width, rc_last.height);
                        } else {
                            g.setColor(COLOR_BEZIER_DOT_BASE);
                            g.fillRect(rc_last.x, rc_last.y, rc_last.width, rc_last.height);
                        }
                        g.setColor(COLOR_BEZIER_DOT_BASE_DARK);
                        g.drawRect(rc_last.x, rc_last.y, rc_last.width, rc_last.height);
                    }
                }
#if DEBUG
            } catch (Exception ex) {
                AppManager.debugWriteLine("TrackSelector+DrawAttatchedCurve");
                AppManager.debugWriteLine("    ex=" + ex);
            }
#endif
        }

        /// <summary>
        /// スクリーンの範囲に直線が可視状態となるかを判定する
        /// </summary>
        /// <param name="visibleMinX"></param>
        /// <param name="visibleMaxX"></param>
        /// <param name="startX"></param>
        /// <param name="endX"></param>
        /// <returns></returns>
        private bool isVisibleOnScreen(int visibleMinX, int visibleMaxX, int startX, int endX)
        {
            return ((visibleMinX <= startX && startX <= visibleMaxX) || (visibleMinX <= endX && endX <= visibleMaxX) || (startX < visibleMinX && visibleMaxX <= endX));
        }

        private Point getScreenCoord(PointD pt)
        {
            return new Point(AppManager.xCoordFromClocks((int)pt.getX()), yCoordFromValue((int)pt.getY()));
        }

        /// <summary>
        /// ビブラートのRate, Depthカーブを描画します
        /// </summary>
        /// <param name="g">描画に使用するグラフィックス</param>
        /// <param name="draw_target">描画対象のトラック</param>
        /// <param name="type">描画するカーブの種類</param>
        /// <param name="color">塗りつぶしに使う色</param>
        /// <param name="is_front">最前面に表示するモードかどうか</param>
        public void drawVibratoControlCurve(Graphics2D g, VsqTrack draw_target, CurveType type, Color color, bool is_front)
        {
            if (!is_front) {
                return;
            }
            Shape last_clip = g.getClip();
            int graph_height = getGraphHeight();
            int key_width = AppManager.keyWidth;
            int width = getWidth();
            int height = getHeight();
            g.clipRect(key_width, HEADER,
                        width - key_width, graph_height);

            int cl_start = AppManager.clockFromXCoord(key_width);
            int cl_end = AppManager.clockFromXCoord(width);
            int max = type.getMaximum();
            int min = type.getMinimum();

            int oy = height - 42;
            float order = graph_height / (float)(max - min);

            // カーブを描く
            int last_shadow_x = key_width;
            foreach (var ve in draw_target.getNoteEventIterator()) {
                int start = ve.Clock + ve.ID.VibratoDelay;
                int end = ve.Clock + ve.ID.getLength();
                if (end < cl_start) {
                    continue;
                }
                if (cl_end < start) {
                    break;
                }
                if (ve.ID.VibratoHandle == null) {
                    continue;
                }
                VibratoHandle handle = ve.ID.VibratoHandle;
                if (handle == null) {
                    continue;
                }
                int x1 = AppManager.xCoordFromClocks(start);

                // 左側の影付にする部分を描画
                g.setColor(COLOR_VIBRATO_SHADOW);
                g.fillRect(last_shadow_x, HEADER, x1 - last_shadow_x, graph_height);
                int x2 = AppManager.xCoordFromClocks(end);
                last_shadow_x = x2;

                if (x1 < x2) {
                    // 描画器を取得、初期化
                    LineGraphDrawer d = getGraphDrawer();
                    d.clear();
                    d.setGraphics(g);
                    d.setBaseLineY(oy);
                    d.setFillColor(color);
                    d.setFill(true);
                    d.setDotMode(LineGraphDrawer.DOTMODE_NO);
                    d.setDrawLine(false);
                    d.setLineColor(Color.white);
                    d.setDrawLine(true);

                    int draw_width = x2 - x1;
                    int start_value = 0;

                    // typeに応じてカーブを取得
                    VibratoBPList list = null;
                    if (type.equals(CurveType.VibratoRate)) {
                        start_value = handle.getStartRate();
                        list = handle.getRateBP();
                    } else if (type.equals(CurveType.VibratoDepth)) {
                        start_value = handle.getStartDepth();
                        list = handle.getDepthBP();
                    }
                    if (list == null) {
                        continue;
                    }

                    // 描画
                    int last_y = oy - (int)((start_value - min) * order);
                    d.append(x1, last_y);
                    int c = list.getCount();
                    for (int i = 0; i < c; i++) {
                        VibratoBPPair item = list.getElement(i);
                        int x = x1 + (int)(item.X * draw_width);
                        int y = oy - (int)((item.Y - min) * order);
                        d.append(x, y);
                        last_y = y;
                    }
                    d.append(x2, last_y);

                    d.flush();
                }
            }

            // 右側の影付にする部分を描画
            g.setColor(COLOR_VIBRATO_SHADOW);
            g.fillRect(last_shadow_x, HEADER, width - key_width, graph_height);

            g.setClip(last_clip);
        }

        /// <summary>
        /// BPList(コントロールカーブ)を指定したグラフィックスを用いて描画します
        /// </summary>
        /// <param name="g">描画に使用するグラフィックス</param>
        /// <param name="list">描画するコントロールカーブ</param>
        /// <param name="color">X軸とデータ線の間の塗りつぶしに使用する色</param>
        /// <param name="is_front">最前面に表示するモードかどうか</param>
        public void drawVsqBPList(Graphics2D g, VsqBPList list, Color color, bool is_front)
        {
            Point pmouse = pointToClient(PortUtil.getMousePosition());
            int max = list.getMaximum();
            int min = list.getMinimum();
            int graph_height = getGraphHeight();
            int width = getWidth();
            int height = getHeight();
            float order = graph_height / (float)(max - min);
            int oy = height - 42;
            int key_width = AppManager.keyWidth;

            int start = key_width;
            int start_clock = AppManager.clockFromXCoord(start);
            int end = width;
            int end_clock = AppManager.clockFromXCoord(end);

            // グラフ描画器の取得と設定
            LineGraphDrawer d = getGraphDrawer();
            d.clear();
            d.setGraphics(g);
            d.setBaseLineY(oy);
            d.setDotSize(DOT_WID);
            d.setFillColor(color);
            d.setDotColor(Color.white);
            d.setLineColor(Color.white);
            int dot_mode = is_front ? LineGraphDrawer.DOTMODE_NEAR : LineGraphDrawer.DOTMODE_NO;
            if (pmouse.y < 0 || height < pmouse.y) {
                dot_mode = LineGraphDrawer.DOTMODE_NO;
            }
            d.setDotMode(dot_mode);
            d.setDrawLine(is_front);
            d.setMouseX(pmouse.x);

            // グラフの描画
            int first_y = list.getValue(start_clock);
            int last_y = oy - (int)((first_y - min) * order);
            d.append(0, last_y);

            int c = list.size();
            if (c > 0) {
                int first_clock = list.getKeyClock(0);
                int last_x = AppManager.xCoordFromClocks(first_clock);
                first_y = list.getValue(first_clock);
                last_y = oy - (int)((first_y - min) * order);

                for (int i = 0; i < c; i++) {
                    int clock = list.getKeyClock(i);
                    if (clock < start_clock) {
                        continue;
                    }
                    if (end_clock < clock) {
                        break;
                    }
                    int x = AppManager.xCoordFromClocks(clock);
                    VsqBPPair v = list.getElementB(i);
                    int y = oy - (int)((v.value - min) * order);
                    d.append(x, y);
                    last_y = y;
                }
            }

            d.append(width + DOT_WID + DOT_WID, last_y);
            d.flush();

            // 最前面のBPListの場合
            if (!is_front) {
                // 最前面じゃなかったら帰る
                return;
            }
            // 選択されているデータ点をハイライト表示する
            int w = DOT_WID * 2 + 1;
            g.setColor(COLOR_DOT_HILIGHT);
            foreach (var id in AppManager.itemSelection.getPointIDIterator()) {
                VsqBPPairSearchContext ret = list.findElement(id);
                if (ret.index < 0) {
                    continue;
                }
                int clock = ret.clock;
                int value = ret.point.value;

                int x = AppManager.xCoordFromClocks(clock);
                if (x < key_width) {
                    continue;
                } else if (width < x) {
                    break;
                }
                int y = oy - (int)((value - min) * order);
                g.fillRect(x - DOT_WID, y - DOT_WID, w, w);
            }

            // 移動中のデータ点をハイライト表示する
            if (mMouseDownMode == MouseDownMode.POINT_MOVE) {
                int dx = pmouse.x + AppManager.mMainWindowController.getStartToDrawX() - mMouseDownLocation.x;
                int dy = pmouse.y - mMouseDownLocation.y;
                foreach (var item in mMovingPoints) {
                    int x = AppManager.xCoordFromClocks(item.Clock) + dx;
                    int y = yCoordFromValue(item.Value) + dy;
                    g.setColor(COLOR_DOT_HILIGHT);
                    g.fillRect(x - DOT_WID, y - DOT_WID, w, w);
                }
            }
        }

        /// <summary>
        /// カーブエディタのグラフ部分の高さを取得します(pixel)
        /// </summary>
        public int getGraphHeight()
        {
            return getHeight() - 42 - 8;
        }

        /// <summary>
        /// カーブエディタのグラフ部分の幅を取得します。(pixel)
        /// </summary>
        public int getGraphWidth()
        {
            return getWidth() - AppManager.keyWidth;
        }

        public void TrackSelector_Load(Object sender, EventArgs e)
        {
            this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
        }

        public void TrackSelector_MouseClick(Object sender, MouseEventArgs e)
        {
            if (mCurveVisible) {
                if (e.Button == MouseButtons.Left) {
                    // カーブの種類一覧上で発生したイベントかどうかを検査
                    for (int i = 0; i < mViewingCurves.Count; i++) {
                        CurveType curve = mViewingCurves[i];
                        Rectangle r = getRectFromCurveType(curve);
                        if (isInRect(e.X, e.Y, r)) {
                            changeCurve(curve);
                            return;
                        }
                    }
                } else if (e.Button == MouseButtons.Right) {
                    if (0 <= e.X && e.X <= AppManager.keyWidth &&
                         0 <= e.Y && e.Y <= getHeight() - 2 * OFFSET_TRACK_TAB) {
                        foreach (var tsi in cmenuCurve.Items) {
                            if (tsi is ToolStripMenuItem) {
                                ToolStripMenuItem tsmi = (ToolStripMenuItem)tsi;
                                tsmi.Checked = false;
                                foreach (var tsi2 in tsmi.DropDownItems) {
                                    if (tsi2 is System.Windows.Forms.ToolStripMenuItem) {
                                        System.Windows.Forms.ToolStripMenuItem tsmi2 = (System.Windows.Forms.ToolStripMenuItem)tsi2;
                                        tsmi2.Checked = false;
                                    }
                                }
                            }
                        }
                        RendererKind kind = VsqFileEx.getTrackRendererKind(AppManager.getVsqFile().Track[AppManager.getSelected()]);
                        if (kind == RendererKind.VOCALOID1) {
                            cmenuCurveVelocity.Visible = true;
                            cmenuCurveAccent.Visible = true;
                            cmenuCurveDecay.Visible = true;

                            cmenuCurveSeparator1.Visible = true;
                            cmenuCurveDynamics.Visible = true;
                            cmenuCurveVibratoRate.Visible = true;
                            cmenuCurveVibratoDepth.Visible = true;

                            cmenuCurveSeparator2.Visible = true;
                            cmenuCurveReso1.Visible = true;
                            cmenuCurveReso2.Visible = true;
                            cmenuCurveReso3.Visible = true;
                            cmenuCurveReso4.Visible = true;

                            cmenuCurveSeparator3.Visible = true;
                            cmenuCurveHarmonics.Visible = true;
                            cmenuCurveBreathiness.Visible = true;
                            cmenuCurveBrightness.Visible = true;
                            cmenuCurveClearness.Visible = true;
                            cmenuCurveOpening.Visible = false;
                            cmenuCurveGenderFactor.Visible = true;

                            cmenuCurveSeparator4.Visible = true;
                            cmenuCurvePortamentoTiming.Visible = true;
                            cmenuCurvePitchBend.Visible = true;
                            cmenuCurvePitchBendSensitivity.Visible = true;

                            cmenuCurveSeparator5.Visible = true;
                            cmenuCurveEffect2Depth.Visible = true;
                            cmenuCurveEnvelope.Visible = false;

                            cmenuCurveBreathiness.Text = "Noise";
                            cmenuCurveVelocity.Text = "Velocity";
                        } else if (kind == RendererKind.UTAU || kind == RendererKind.VCNT) {
                            cmenuCurveVelocity.Visible = (kind == RendererKind.UTAU);
                            cmenuCurveAccent.Visible = false;
                            cmenuCurveDecay.Visible = false;

                            cmenuCurveSeparator1.Visible = false;
                            cmenuCurveDynamics.Visible = false;
                            cmenuCurveVibratoRate.Visible = true;
                            cmenuCurveVibratoDepth.Visible = true;

                            cmenuCurveSeparator2.Visible = false;
                            cmenuCurveReso1.Visible = false;
                            cmenuCurveReso2.Visible = false;
                            cmenuCurveReso3.Visible = false;
                            cmenuCurveReso4.Visible = false;

                            cmenuCurveSeparator3.Visible = false;
                            cmenuCurveHarmonics.Visible = false;
                            cmenuCurveBreathiness.Visible = false;
                            cmenuCurveBrightness.Visible = false;
                            cmenuCurveClearness.Visible = false;
                            cmenuCurveOpening.Visible = false;
                            cmenuCurveGenderFactor.Visible = false;

                            cmenuCurveSeparator4.Visible = true;
                            cmenuCurvePortamentoTiming.Visible = false;
                            cmenuCurvePitchBend.Visible = true;
                            cmenuCurvePitchBendSensitivity.Visible = true;

                            cmenuCurveSeparator5.Visible = true;
                            cmenuCurveEffect2Depth.Visible = false;
                            cmenuCurveEnvelope.Visible = true;

                            if (kind == RendererKind.UTAU) {
                                cmenuCurveVelocity.Text = "Intensity";
                            }
                        } else {
                            cmenuCurveVelocity.Visible = true;
                            cmenuCurveAccent.Visible = true;
                            cmenuCurveDecay.Visible = true;

                            cmenuCurveSeparator1.Visible = true;
                            cmenuCurveDynamics.Visible = true;
                            cmenuCurveVibratoRate.Visible = true;
                            cmenuCurveVibratoDepth.Visible = true;

                            cmenuCurveSeparator2.Visible = false;
                            cmenuCurveReso1.Visible = false;
                            cmenuCurveReso2.Visible = false;
                            cmenuCurveReso3.Visible = false;
                            cmenuCurveReso4.Visible = false;

                            cmenuCurveSeparator3.Visible = true;
                            cmenuCurveHarmonics.Visible = false;
                            cmenuCurveBreathiness.Visible = true;
                            cmenuCurveBrightness.Visible = true;
                            cmenuCurveClearness.Visible = true;
                            cmenuCurveOpening.Visible = true;
                            cmenuCurveGenderFactor.Visible = true;

                            cmenuCurveSeparator4.Visible = true;
                            cmenuCurvePortamentoTiming.Visible = true;
                            cmenuCurvePitchBend.Visible = true;
                            cmenuCurvePitchBendSensitivity.Visible = true;

                            cmenuCurveSeparator5.Visible = false;
                            cmenuCurveEffect2Depth.Visible = false;
                            cmenuCurveEnvelope.Visible = false;

                            cmenuCurveBreathiness.Text = "Breathiness";
                            cmenuCurveVelocity.Text = "Velocity";
                        }
                        foreach (var tsi in cmenuCurve.Items) {
                            if (tsi is ToolStripMenuItem) {
                                ToolStripMenuItem tsmi = (ToolStripMenuItem)tsi;
                                CurveType ct = getCurveTypeFromMenu(tsmi);
                                if (ct.equals(mSelectedCurve)) {
                                    tsmi.Checked = true;
                                    break;
                                }
                                foreach (var tsi2 in tsmi.DropDownItems) {
                                    if (tsi2 is System.Windows.Forms.ToolStripMenuItem) {
                                        var tsmi2 = (System.Windows.Forms.ToolStripMenuItem)tsi2;
                                        CurveType ct2 = getCurveTypeFromMenu(tsmi2);
                                        if (ct2.equals(mSelectedCurve)) {
                                            tsmi2.Checked = true;
                                            if (ct2.equals(CurveType.reso1amp) || ct2.equals(CurveType.reso1bw) || ct2.equals(CurveType.reso1freq) ||
                                                 ct2.equals(CurveType.reso2amp) || ct2.equals(CurveType.reso2bw) || ct2.equals(CurveType.reso2freq) ||
                                                 ct2.equals(CurveType.reso3amp) || ct2.equals(CurveType.reso3bw) || ct2.equals(CurveType.reso3freq) ||
                                                 ct2.equals(CurveType.reso4amp) || ct2.equals(CurveType.reso4bw) || ct2.equals(CurveType.reso4freq)) {
                                                tsmi.Checked = true;//親アイテムもチェック。Resonance*用
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        cmenuCurve.Show(this, e.X, e.Y);
                    }
                }
            }
        }

        public void SelectNextCurve()
        {
            int index = 0;
            if (mViewingCurves.Count >= 2) {
                for (int i = 0; i < mViewingCurves.Count; i++) {
                    if (mViewingCurves[i].equals(mSelectedCurve)) {
                        index = i;
                        break;
                    }
                }
                index++;
                if (mViewingCurves.Count <= index) {
                    index = 0;
                }
                changeCurve(mViewingCurves[index]);
            }
        }

        public void SelectPreviousCurve()
        {
            int index = 0;
            if (mViewingCurves.Count >= 2) {
                for (int i = 0; i < mViewingCurves.Count; i++) {
                    if (mViewingCurves[i].equals(mSelectedCurve)) {
                        index = i;
                        break;
                    }
                }
                index--;
                if (index < 0) {
                    index = mViewingCurves.Count - 1;
                }
                changeCurve(mViewingCurves[index]);
            }
        }

        public BezierPoint HandleMouseMoveForBezierMove(int clock, int value, int value_raw, BezierPickedSide picked)
        {
            BezierChain target = AppManager.getVsqFile().AttachedCurves.get(AppManager.getSelected() - 1).getBezierChain(mSelectedCurve, AppManager.itemSelection.getLastBezier().chainID);
            int point_id = AppManager.itemSelection.getLastBezier().pointID;
            int index = -1;
            for (int i = 0; i < target.points.Count; i++) {
                if (target.points[i].getID() == point_id) {
                    index = i;
                    break;
                }
            }
            float scale_x = AppManager.mMainWindowController.getScaleX();
            float scale_y = getScaleY();
            BezierPoint ret = new BezierPoint(0, 0);
            if (index >= 0) {
                BezierPoint item = target.points[index];
                if (picked == BezierPickedSide.BASE) {
                    // データ点を動かす
                    Point old = target.points[index].getBase().toPoint();
                    item.setBase(new PointD(clock, value));
                    if (!BezierChain.isBezierImplicit(target)) {
                        // X軸について陰でなくなった場合
                        // データ点のX座標だけ元に戻し，もう一度チェックを試みる
                        item.setBase(new PointD(old.x, value));
                        if (!BezierChain.isBezierImplicit(target)) {
                            // 駄目ならX, Y両方元に戻す
                            item.setBase(new PointD(old.x, old.y));
                        }
                    }
                    ret = (BezierPoint)target.points[index].clone();
                } else if (picked == BezierPickedSide.LEFT) {
                    // 左制御点を動かす
                    if (item.getControlLeftType() == BezierControlType.Master) {
                        // 右制御点を同時に動かさない場合
                        PointD old_left = new PointD(item.getControlLeft());
                        item.setControlLeft(new PointD(clock, value_raw));
                        if (!BezierChain.isBezierImplicit(target)) {
                            // X軸について陰でなくなった場合
                            // X座標だけ元に戻し，もう一度チェックを試みる
                            item.setControlLeft(new PointD(old_left.getX(), value_raw));
                            if (!BezierChain.isBezierImplicit(target)) {
                                // 駄目ならX, Y両方戻す
                                item.setControlLeft(old_left);
                            }
                        }
                    } else {
                        // 右制御点を同時に動かす場合(デフォルト)
                        PointD old_left = new PointD(item.getControlLeft());
                        PointD old_right = new PointD(item.getControlRight());
                        PointD old_base = new PointD(item.getBase());

                        // 新しい座標値を計算，設定
                        PointD new_left = new PointD(clock, value_raw);
                        PointD new_right = getCounterPoint(old_base, old_right, new_left, scale_x, scale_y);
                        item.setControlLeft(new_left);
                        item.setControlRight(new_right);

                        // X軸方向に陰かどうかチェック
                        if (!BezierChain.isBezierImplicit(target)) {
                            // 駄目なら，Xを元に戻す
                            new_left.setX(old_left.getX());
                            new_right = getCounterPoint(old_base, old_right, new_left, scale_x, scale_y);
                            item.setControlLeft(new_left);
                            item.setControlRight(new_right);
                            if (!BezierChain.isBezierImplicit(target)) {
                                // それでもだめなら両方戻す
                                item.setControlLeft(old_left);
                                item.setControlRight(old_right);
                            }
                        }
                    }
                    ret = (BezierPoint)item.clone();
                } else if (picked == BezierPickedSide.RIGHT) {
                    // 右制御点を動かす
                    if (item.getControlRightType() == BezierControlType.Master) {
                        // 左制御点を同時に動かさない場合
                        PointD old_right = item.getControlRight();
                        item.setControlRight(new PointD(clock, value));
                        if (!BezierChain.isBezierImplicit(target)) {
                            // Xだけ元に戻す
                            item.setControlRight(new PointD(old_right.getX(), value));
                            if (!BezierChain.isBezierImplicit(target)) {
                                item.setControlRight(old_right);
                            }
                        }
                    } else {
                        // 左制御点を同時に動かす場合(デフォルト)
                        PointD old_left = new PointD(item.getControlLeft());
                        PointD old_right = new PointD(item.getControlRight());
                        PointD old_base = new PointD(item.getBase());

                        // 新しい座標値を計算，設定
                        PointD new_right = new PointD(clock, value_raw);
                        PointD new_left = getCounterPoint(old_base, old_left, new_right, scale_x, scale_y);
                        item.setControlRight(new_right);
                        item.setControlLeft(new_left);

                        // X軸方向に陰かどうかチェック
                        if (!BezierChain.isBezierImplicit(target)) {
                            // 駄目ならXだけ元に戻す
                            new_right.setX(old_right.getX());
                            new_left = getCounterPoint(old_base, old_left, new_right, scale_x, scale_y);
                            item.setControlRight(new_right);
                            item.setControlLeft(new_left);
                            if (!BezierChain.isBezierImplicit(target)) {
                                item.setControlLeft(old_left);
                                item.setControlRight(old_right);
                            }
                        }
                    }
                    ret = (BezierPoint)item.clone();
                }
            }
            return ret;
        }

        /// <summary>
        /// slave_point_original, base_point, moving_pointがこの順で1直線に並んでいる時，
        /// base_pointを回転軸としてmoving_pointを動かした場合に，
        /// 回転に伴ってslave_point_originalが移動した先の座標を計算します．
        /// ただし，上記の各点の座標値はscalex，scaleyを乗じた上で計算されます
        /// </summary>
        /// <param name="base_point"></param>
        /// <param name="slave_point_original"></param>
        /// <param name="moving_point"></param>
        /// <param name="scalex"></param>
        /// <param name="scaley"></param>
        /// <returns></returns>
        private static PointD getCounterPoint(
            PointD base_point,
            PointD slave_point_original,
            PointD moving_point,
            double scalex, double scaley)
        {
            // 移動後の点と回転軸との為す角を計算
            double theta =
                Math.Atan2(
                    (moving_point.getY() - base_point.getY()) * scaley,
                    (moving_point.getX() - base_point.getX()) * scalex);
            // 直線なので，逆サイドの偏角は+180度
            theta += Math.PI;
            // 逆サイドの点と回転軸との距離を計算
            double dx = (slave_point_original.getX() - base_point.getX()) * scalex;
            double dy = (slave_point_original.getY() - base_point.getY()) * scaley;
            double length = Math.Sqrt(dx * dx + dy * dy);
            // 逆サイドの点の座標を計算
            return new PointD(
                length * Math.Cos(theta) / scalex + base_point.getX(),
                length * Math.Sin(theta) / scaley + base_point.getY());
        }

        public BezierPoint HandleMouseMoveForBezierMove(MouseEventArgs e, BezierPickedSide picked)
        {
            int clock = AppManager.clockFromXCoord(e.X);
            int value = valueFromYCoord(e.Y);
            int value_raw = value;

            if (clock < AppManager.getVsqFile().getPreMeasure()) {
                clock = AppManager.getVsqFile().getPreMeasure();
            }
            int max = mSelectedCurve.getMaximum();
            int min = mSelectedCurve.getMinimum();
            if (value < min) {
                value = min;
            } else if (max < value) {
                value = max;
            }
            return HandleMouseMoveForBezierMove(clock, value, value_raw, picked);
        }

        public void TrackSelector_MouseMove(Object sender, MouseEventArgs e)
        {
            int value = valueFromYCoord(e.Y);
            int value_raw = value;
            int max = mSelectedCurve.getMaximum();
            int min = mSelectedCurve.getMinimum();
            int selected = AppManager.getSelected();
            bool is_utau_mode = AppManager.mDrawIsUtau[selected - 1];
            if (is_utau_mode && mSelectedCurve.equals(CurveType.VEL)) {
                max = UstEvent.MAX_INTENSITY;
                min = UstEvent.MIN_INTENSITY;
            }
            if (value < min) {
                value = min;
            } else if (max < value) {
                value = max;
            }
            mMouseValue = value;

            if (e.Button == MouseButtons.None) {
                return;
            }
            int stdx = AppManager.mMainWindowController.getStartToDrawX();
            if ((e.X + stdx != mMouseDownLocation.x || e.Y != mMouseDownLocation.y)) {
                if (mMouseHoverThread != null && mMouseHoverThread.IsAlive) {
                    mMouseHoverThread.Abort();
                }
                if (mMouseDownMode == MouseDownMode.VEL_WAIT_HOVER) {
                    mMouseDownMode = MouseDownMode.VEL_EDIT;
                }
                mMouseMoved = true;
            }
            if (AppManager.isPlaying()) {
                return;
            }
            int clock = AppManager.clockFromXCoord(e.X);

            VsqFileEx vsq = AppManager.getVsqFile();
            if (clock < vsq.getPreMeasure()) {
                clock = vsq.getPreMeasure();
            }

            if (e.Button == MouseButtons.Left &&
                 0 <= e.Y && e.Y <= getHeight() - 2 * OFFSET_TRACK_TAB &&
                 mMouseDownMode == MouseDownMode.CURVE_EDIT) {
                EditTool selected_tool = AppManager.getSelectedTool();
                if (selected_tool == EditTool.PENCIL) {
                    mPencilMoved = e.X + stdx != mMouseDownLocation.x ||
                                     e.Y != mMouseDownLocation.y;
                    mMouseTracer.append(e.X + stdx, e.Y);
                } else if (selected_tool == EditTool.LINE) {
                    mPencilMoved = e.X + stdx != mMouseDownLocation.x ||
                                     e.Y != mMouseDownLocation.y;
                } else if (selected_tool == EditTool.ARROW ||
                            selected_tool == EditTool.ERASER) {
                    int draft_clock = clock;
                    if (AppManager.editorConfig.CurveSelectingQuantized) {
                        int unit = AppManager.getPositionQuantizeClock();
                        int odd = clock % unit;
                        int nclock = clock;
                        nclock -= odd;
                        if (odd > unit / 2) {
                            nclock += unit;
                        }
                        draft_clock = nclock;
                    }
                    AppManager.mCurveSelectingRectangle.width = draft_clock - AppManager.mCurveSelectingRectangle.x;
                    AppManager.mCurveSelectingRectangle.height = value - AppManager.mCurveSelectingRectangle.y;
                }
            } else if (mMouseDownMode == MouseDownMode.SINGER_LIST) {
                int dclock = clock - mSingerMoveStartedClock;
                foreach (var item in AppManager.itemSelection.getEventIterator()) {
                    item.editing.Clock = item.original.Clock + dclock;
                }
            } else if (mMouseDownMode == MouseDownMode.VEL_EDIT) {
                int t_value = valueFromYCoord(e.Y - mVelEditShiftY);
                int d_vel = 0;
                VsqEvent ve_original = mVelEditSelected[mVelEditLastSelectedID].original;
                if (mSelectedCurve.equals(CurveType.VEL)) {
                    if (is_utau_mode) {
                        d_vel = t_value - ((ve_original.UstEvent == null) ? 100 : ve_original.UstEvent.getIntensity());
                    } else {
                        d_vel = t_value - ve_original.ID.Dynamics;
                    }
                } else if (mSelectedCurve.equals(CurveType.Accent)) {
                    d_vel = t_value - mVelEditSelected[mVelEditLastSelectedID].original.ID.DEMaccent;
                } else if (mSelectedCurve.equals(CurveType.Decay)) {
                    d_vel = t_value - mVelEditSelected[mVelEditLastSelectedID].original.ID.DEMdecGainRate;
                }
                foreach (var id in mVelEditSelected.Keys) {
                    if (mSelectedCurve.equals(CurveType.VEL)) {
                        VsqEvent item = mVelEditSelected[id].original;
                        int new_vel = item.ID.Dynamics + d_vel;
                        if (is_utau_mode) {
                            new_vel = item.UstEvent == null ? 100 + d_vel : item.UstEvent.getIntensity() + d_vel;
                        }
                        if (new_vel < min) {
                            new_vel = min;
                        } else if (max < new_vel) {
                            new_vel = max;
                        }
                        if (is_utau_mode) {
                            VsqEvent item_o = mVelEditSelected[id].editing;
                            if (item_o.UstEvent == null) {
                                item_o.UstEvent = new UstEvent();
                            }
                            item_o.UstEvent.setIntensity(new_vel);
                        } else {
                            mVelEditSelected[id].editing.ID.Dynamics = new_vel;
                        }
                    } else if (mSelectedCurve.equals(CurveType.Accent)) {
                        int new_vel = mVelEditSelected[id].original.ID.DEMaccent + d_vel;
                        if (new_vel < min) {
                            new_vel = min;
                        } else if (max < new_vel) {
                            new_vel = max;
                        }
                        mVelEditSelected[id].editing.ID.DEMaccent = new_vel;
                    } else if (mSelectedCurve.equals(CurveType.Decay)) {
                        int new_vel = mVelEditSelected[id].original.ID.DEMdecGainRate + d_vel;
                        if (new_vel < min) {
                            new_vel = min;
                        } else if (max < new_vel) {
                            new_vel = max;
                        }
                        mVelEditSelected[id].editing.ID.DEMdecGainRate = new_vel;
                    }
                }
            } else if (mMouseDownMode == MouseDownMode.BEZIER_MODE) {
                HandleMouseMoveForBezierMove(clock, value, value_raw, AppManager.itemSelection.getLastBezier().picked);
            } else if (mMouseDownMode == MouseDownMode.BEZIER_ADD_NEW || mMouseDownMode == MouseDownMode.BEZIER_EDIT) {
                BezierChain target = vsq.AttachedCurves.get(selected - 1).getBezierChain(mSelectedCurve, AppManager.itemSelection.getLastBezier().chainID);
                int point_id = AppManager.itemSelection.getLastBezier().pointID;
                int index = -1;
                for (int i = 0; i < target.points.Count; i++) {
                    if (target.points[i].getID() == point_id) {
                        index = i;
                        break;
                    }
                }
                if (index >= 0) {
                    BezierPoint item = target.points[index];
                    Point old_right = item.getControlRight().toPoint();
                    Point old_left = item.getControlLeft().toPoint();
                    BezierControlType old_right_type = item.getControlRightType();
                    BezierControlType old_left_type = item.getControlLeftType();
                    int cl = clock;
                    int va = value_raw;
                    int dx = (int)item.getBase().getX() - cl;
                    int dy = (int)item.getBase().getY() - va;
                    if (item.getBase().getX() + dx >= 0) {
                        item.setControlRight(new PointD(clock, value_raw));
                        item.setControlLeft(new PointD(clock + 2 * dx, value_raw + 2 * dy));
                        item.setControlRightType(BezierControlType.Normal);
                        item.setControlLeftType(BezierControlType.Normal);
                        if (!BezierChain.isBezierImplicit(target)) {
                            item.setControlLeft(new PointD(old_left.x, old_left.y));
                            item.setControlRight(new PointD(old_right.x, old_right.y));
                            item.setControlLeftType(old_left_type);
                            item.setControlRightType(old_right_type);
                        }
                    }
                }
            } else if (mMouseDownMode == MouseDownMode.ENVELOPE_MOVE) {
                double sec = vsq.getSecFromClock(AppManager.clockFromXCoord(e.X));
                int v = valueFromYCoord(e.Y);
                if (v < 0) {
                    v = 0;
                } else if (200 < v) {
                    v = 200;
                }
                if (sec < mEnvelopeDotBegin) {
                    sec = mEnvelopeDotBegin;
                } else if (mEnvelopeDotEnd < sec) {
                    sec = mEnvelopeDotEnd;
                }
                if (mEnvelopePointKind == 1) {
                    mEnvelopeEditing.p1 = (int)((sec - mEnvelopeRangeBegin) * 1000.0);
                    mEnvelopeEditing.v1 = v;
                } else if (mEnvelopePointKind == 2) {
                    mEnvelopeEditing.p2 = (int)((sec - mEnvelopeRangeBegin) * 1000.0) - mEnvelopeEditing.p1;
                    mEnvelopeEditing.v2 = v;
                } else if (mEnvelopePointKind == 3) {
                    mEnvelopeEditing.p5 = (int)((sec - mEnvelopeRangeBegin) * 1000.0) - mEnvelopeEditing.p1 - mEnvelopeEditing.p2;
                    mEnvelopeEditing.v5 = v;
                } else if (mEnvelopePointKind == 4) {
                    mEnvelopeEditing.p3 = (int)((mEnvelopeRangeEnd - sec) * 1000.0) - mEnvelopeEditing.p4;
                    mEnvelopeEditing.v3 = v;
                } else if (mEnvelopePointKind == 5) {
                    mEnvelopeEditing.p4 = (int)((mEnvelopeRangeEnd - sec) * 1000.0);
                    mEnvelopeEditing.v4 = v;
                }
            } else if (mMouseDownMode == MouseDownMode.PRE_UTTERANCE_MOVE) {
                int clock_at_downed = AppManager.clockFromXCoord(mMouseDownLocation.x - stdx);
                double dsec = vsq.getSecFromClock(clock) - vsq.getSecFromClock(clock_at_downed);
                float draft_preutterance = mPreOverlapOriginal.UstEvent.getPreUtterance() - (float)(dsec * 1000);
                mPreOverlapEditing.UstEvent.setPreUtterance(draft_preutterance);
            } else if (mMouseDownMode == MouseDownMode.OVERLAP_MOVE) {
                int clock_at_downed = AppManager.clockFromXCoord(mMouseDownLocation.x - stdx);
                double dsec = vsq.getSecFromClock(clock) - vsq.getSecFromClock(clock_at_downed);
                float draft_overlap = mPreOverlapOriginal.UstEvent.getVoiceOverlap() + (float)(dsec * 1000);
                mPreOverlapEditing.UstEvent.setVoiceOverlap(draft_overlap);
            }
        }

        /// <summary>
        /// 指定した位置にあるBezierPointを検索します。
        /// </summary>
        /// <param name="location"></param>
        /// <param name="list"></param>
        /// <param name="found_chain"></param>
        /// <param name="found_point"></param>
        /// <param name="found_side"></param>
        /// <param name="dot_width"></param>
        /// <param name="px_tolerance"></param>
        private void findBezierPointAt(int locx,
                                        int locy,
                                        List<BezierChain> list,
                                        ByRef<BezierChain> found_chain,
                                        ByRef<BezierPoint> found_point,
                                        ByRef<BezierPickedSide> found_side,
                                        int dot_width,
                                        int px_tolerance)
        {
            found_chain.value = null;
            found_point.value = null;
            found_side.value = BezierPickedSide.BASE;
            int shift = dot_width + px_tolerance;
            int width = (dot_width + px_tolerance) * 2;
            int c = list.Count;
            Point location = new Point(locx, locy);
            for (int i = 0; i < c; i++) {
                BezierChain bc = list[i];
                foreach (var bp in bc.points) {
                    Point p = getScreenCoord(bp.getBase());
                    Rectangle r = new Rectangle(p.x - shift, p.y - shift, width, width);
                    if (isInRect(locx, locy, r)) {
                        found_chain.value = bc;
                        found_point.value = bp;
                        found_side.value = BezierPickedSide.BASE;
                        return;
                    }

                    if (bp.getControlLeftType() != BezierControlType.None) {
                        p = getScreenCoord(bp.getControlLeft());
                        r = new Rectangle(p.x - shift, p.y - shift, width, width);
                        if (isInRect(locx, locy, r)) {
                            found_chain.value = bc;
                            found_point.value = bp;
                            found_side.value = BezierPickedSide.LEFT;
                            return;
                        }
                    }

                    if (bp.getControlRightType() != BezierControlType.None) {
                        p = getScreenCoord(bp.getControlRight());
                        r = new Rectangle(p.x - shift, p.y - shift, width, width);
                        if (isInRect(locx, locy, r)) {
                            found_chain.value = bc;
                            found_point.value = bp;
                            found_side.value = BezierPickedSide.RIGHT;
                            return;
                        }
                    }
                }
            }
        }

        private void processMouseDownSelectRegion(MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) != Keys.Control) {
                AppManager.itemSelection.clearPoint();
            }

            int clock = AppManager.clockFromXCoord(e.X);
            int quantized_clock = clock;
            int unit = AppManager.getPositionQuantizeClock();
            int odd = clock % unit;
            quantized_clock -= odd;
            if (odd > unit / 2) {
                quantized_clock += unit;
            }

            int max = mSelectedCurve.getMaximum();
            int min = mSelectedCurve.getMinimum();
            int value = valueFromYCoord(e.Y);
            if (value < min) {
                value = min;
            } else if (max < value) {
                value = max;
            }

            if (AppManager.editorConfig.CurveSelectingQuantized) {
                AppManager.mCurveSelectingRectangle = new Rectangle(quantized_clock, value, 0, 0);
            } else {
                AppManager.mCurveSelectingRectangle = new Rectangle(clock, value, 0, 0);
            }
        }

        public void TrackSelector_MouseDown(Object sender, MouseEventArgs e)
        {
#if DEBUG
            AppManager.debugWriteLine("TrackSelector_MouseDown");
#endif
            VsqFileEx vsq = AppManager.getVsqFile();
            mMouseDownLocation.x = e.X + AppManager.mMainWindowController.getStartToDrawX();
            mMouseDownLocation.y = e.Y;
            int clock = AppManager.clockFromXCoord(e.X);
            int selected = AppManager.getSelected();
            int height = getHeight();
            int width = getWidth();
            int key_width = AppManager.keyWidth;
            VsqTrack vsq_track = vsq.Track[selected];
            mMouseMoved = false;
            mMouseDowned = true;
            if (AppManager.keyWidth < e.X && clock < vsq.getPreMeasure()) {
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }
            int stdx = AppManager.mMainWindowController.getStartToDrawX();
            mModifierOnMouseDown = Control.ModifierKeys;
            int max = mSelectedCurve.getMaximum();
            int min = mSelectedCurve.getMinimum();
            int value = valueFromYCoord(e.Y);
            if (value < min) {
                value = min;
            } else if (max < value) {
                value = max;
            }

            if (height - OFFSET_TRACK_TAB <= e.Y && e.Y < height) {
                if (e.Button == MouseButtons.Left) {
                    #region MouseDown occured on track list
                    mMouseDownMode = MouseDownMode.TRACK_LIST;
                    //AppManager.isCurveSelectedIntervalEnabled() = false;
                    mMouseTracer.clear();
                    int selecter_width = getSelectorWidth();
                    if (vsq != null) {
                        for (int i = 0; i < AppManager.MAX_NUM_TRACK; i++) {
                            int x = key_width + i * selecter_width;
                            if (vsq.Track.Count > i + 1) {
                                if (x <= e.X && e.X < x + selecter_width) {
                                    int new_selected = i + 1;
                                    if (AppManager.getSelected() != new_selected) {
                                        AppManager.setSelected(i + 1);
                                        try {
                                            if (SelectedTrackChanged != null) {
                                                SelectedTrackChanged.Invoke(this, i + 1);
                                            }
                                        } catch (Exception ex) {
                                            serr.println("TrackSelector#TrackSelector_MouseDown; ex=" + ex);
                                        }
                                        Invalidate();
                                        return;
                                    } else if (x + selecter_width - PX_WIDTH_RENDER <= e.X && e.X < e.X + selecter_width) {
                                        if (AppManager.getRenderRequired(AppManager.getSelected()) && !AppManager.isPlaying()) {
                                            try {
                                                if (RenderRequired != null) {
                                                    RenderRequired.Invoke(this, AppManager.getSelected());
                                                }
                                            } catch (Exception ex) {
                                                serr.println("TrackSelector#TrackSelector_MouseDown; ex=" + ex);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            } else if (height - 2 * OFFSET_TRACK_TAB <= e.Y && e.Y < height - OFFSET_TRACK_TAB) {
                #region MouseDown occured on singer tab
                mMouseDownMode = MouseDownMode.SINGER_LIST;
                AppManager.itemSelection.clearPoint();
                mMouseTracer.clear();
                VsqEvent ve = null;
                if (key_width <= e.X && e.X <= width) {
                    ve = findItemAt(e.X, e.Y);
                }
                if (AppManager.getSelectedTool() == EditTool.ERASER) {
                    #region EditTool.Eraser
                    if (ve != null && ve.Clock > 0) {
                        CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventDelete(selected, ve.InternalID));
                        executeCommand(run, true);
                    }
                    #endregion
                } else {
                    if (ve != null) {
                        if ((mModifierOnMouseDown & mModifierKey) == mModifierKey) {
                            if (AppManager.itemSelection.isEventContains(AppManager.getSelected(), ve.InternalID)) {
                                List<int> old = new List<int>();
                                foreach (var item in AppManager.itemSelection.getEventIterator()) {
                                    int id = item.original.InternalID;
                                    if (id != ve.InternalID) {
                                        old.Add(id);
                                    }
                                }
                                AppManager.itemSelection.clearEvent();
                                AppManager.itemSelection.addEventAll(old);
                            } else {
                                AppManager.itemSelection.addEvent(ve.InternalID);
                            }
                        } else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift) {
                            int last_clock = AppManager.itemSelection.getLastEvent().original.Clock;
                            int tmin = Math.Min(ve.Clock, last_clock);
                            int tmax = Math.Max(ve.Clock, last_clock);
                            List<int> add_required = new List<int>();
                            for (Iterator<VsqEvent> itr = AppManager.getVsqFile().Track[AppManager.getSelected()].getEventIterator(); itr.hasNext(); ) {
                                VsqEvent item = itr.next();
                                if (item.ID.type == VsqIDType.Singer && tmin <= item.Clock && item.Clock <= tmax) {
                                    add_required.Add(item.InternalID);
                                    //AppManager.AddSelectedEvent( item.InternalID );
                                }
                            }
                            add_required.Add(ve.InternalID);
                            AppManager.itemSelection.addEventAll(add_required);
                        } else {
                            if (!AppManager.itemSelection.isEventContains(AppManager.getSelected(), ve.InternalID)) {
                                AppManager.itemSelection.clearEvent();
                            }
                            AppManager.itemSelection.addEvent(ve.InternalID);
                        }
                        mSingerMoveStartedClock = clock;
                    } else {
                        AppManager.itemSelection.clearEvent();
                    }
                }
                #endregion
            } else {
                #region MouseDown occred on other position
                bool clock_inner_note = false; //マウスの降りたクロックが，ノートの範囲内かどうかをチェック
                int left_clock = AppManager.clockFromXCoord(AppManager.keyWidth);
                int right_clock = AppManager.clockFromXCoord(getWidth());
                for (Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                    VsqEvent ve = itr.next();
                    if (ve.ID.type == VsqIDType.Anote) {
                        int start = ve.Clock;
                        if (right_clock < start) {
                            break;
                        }
                        int end = ve.Clock + ve.ID.getLength();
                        if (end < left_clock) {
                            continue;
                        }
                        if (start <= clock && clock < end) {
                            clock_inner_note = true;
                            break;
                        }
                    }
                }
#if DEBUG
                AppManager.debugWriteLine("    clock_inner_note=" + clock_inner_note);
#endif
                if (AppManager.keyWidth <= e.X) {
                    if (e.Button == MouseButtons.Left && !mSpaceKeyDowned) {
                        mMouseDownMode = MouseDownMode.CURVE_EDIT;
                        int quantized_clock = clock;
                        int unit = AppManager.getPositionQuantizeClock();
                        int odd = clock % unit;
                        quantized_clock -= odd;
                        if (odd > unit / 2) {
                            quantized_clock += unit;
                        }

                        int px_shift = DOT_WID + AppManager.editorConfig.PxToleranceBezier;
                        int px_width = px_shift * 2 + 1;

                        if (AppManager.getSelectedTool() == EditTool.LINE) {
                            #region Line
                            if (AppManager.isCurveMode()) {
                                if (mSelectedCurve.equals(CurveType.Env)) {
                                    if (processMouseDownEnvelope(e)) {
                                        Invalidate();
                                        return;
                                    }
                                    if (processMouseDownPreutteranceAndOverlap(e)) {
                                        Invalidate();
                                        return;
                                    }
                                } else if (!mSelectedCurve.equals(CurveType.VEL) &&
                                            !mSelectedCurve.equals(CurveType.Accent) &&
                                            !mSelectedCurve.equals(CurveType.Decay) &&
                                            !mSelectedCurve.equals(CurveType.Env)) {
                                    if (processMouseDownBezier(e)) {
                                        Invalidate();
                                        return;
                                    }
                                }
                            } else {
                                if (mSelectedCurve.equals(CurveType.Env)) {
                                    if (processMouseDownEnvelope(e)) {
                                        Invalidate();
                                        return;
                                    }
                                    if (processMouseDownPreutteranceAndOverlap(e)) {
                                        Invalidate();
                                        return;
                                    }
                                }
                            }
                            mMouseTracer.clear();
                            mMouseTracer.appendFirst(e.X + stdx, e.Y);
                            #endregion
                        } else if (AppManager.getSelectedTool() == EditTool.PENCIL) {
                            #region Pencil
                            if (AppManager.isCurveMode()) {
                                #region CurveMode
                                if (mSelectedCurve.equals(CurveType.VibratoRate) || mSelectedCurve.equals(CurveType.VibratoDepth)) {
                                    // todo: TrackSelector_MouseDownのベジエ曲線
                                } else if (mSelectedCurve.equals(CurveType.Env)) {
                                    if (processMouseDownEnvelope(e)) {
                                        Invalidate();
                                        return;
                                    }
                                    if (processMouseDownPreutteranceAndOverlap(e)) {
                                        Invalidate();
                                        return;
                                    }
                                } else if (!mSelectedCurve.equals(CurveType.VEL) &&
                                            !mSelectedCurve.equals(CurveType.Accent) &&
                                            !mSelectedCurve.equals(CurveType.Decay) &&
                                            !mSelectedCurve.equals(CurveType.Env)) {
                                    if (processMouseDownBezier(e)) {
                                        Invalidate();
                                        return;
                                    }
                                } else {
                                    mMouseDownMode = MouseDownMode.NONE;
                                }
                                #endregion
                            } else {
                                #region NOT CurveMode
                                if (mSelectedCurve.equals(CurveType.Env)) {
                                    if (processMouseDownEnvelope(e)) {
                                        Invalidate();
                                        return;
                                    }
                                    if (processMouseDownPreutteranceAndOverlap(e)) {
                                        Invalidate();
                                        return;
                                    }
                                }
                                mMouseTracer.clear();
                                int x = e.X + AppManager.mMainWindowController.getStartToDrawX();
                                mMouseTracer.appendFirst(x, e.Y);
                                mPencilMoved = false;

                                mMouseHoverThread = new Thread(new ThreadStart(MouseHoverEventGenerator));
                                mMouseHoverThread.Start();
                                #endregion
                            }
                            #endregion
                        } else if (AppManager.getSelectedTool() == EditTool.ARROW) {
                            #region Arrow
                            bool found = false;
                            if (mSelectedCurve.isScalar() || mSelectedCurve.isAttachNote()) {
                                if (mSelectedCurve.equals(CurveType.Env)) {
                                    if (processMouseDownEnvelope(e)) {
                                        Invalidate();
                                        return;
                                    }
                                    if (processMouseDownPreutteranceAndOverlap(e)) {
                                        Invalidate();
                                        return;
                                    }
                                }
                                mMouseDownMode = MouseDownMode.NONE;
                            } else {
                                // まずベジエ曲線の点にヒットしてないかどうかを検査
                                List<BezierChain> dict = AppManager.getVsqFile().AttachedCurves.get(AppManager.getSelected() - 1).get(mSelectedCurve);
                                AppManager.itemSelection.clearBezier();
                                for (int i = 0; i < dict.Count; i++) {
                                    BezierChain bc = dict[i];
                                    foreach (var bp in bc.points) {
                                        Point pt = getScreenCoord(bp.getBase());
                                        Rectangle rc = new Rectangle(pt.x - px_shift, pt.y - px_shift, px_width, px_width);
                                        if (isInRect(e.X, e.Y, rc)) {
                                            AppManager.itemSelection.addBezier(new SelectedBezierPoint(bc.id, bp.getID(), BezierPickedSide.BASE, bp));
                                            mEditingBezierOriginal = (BezierChain)bc.clone();
                                            found = true;
                                            break;
                                        }

                                        if (bp.getControlLeftType() != BezierControlType.None) {
                                            pt = getScreenCoord(bp.getControlLeft());
                                            rc = new Rectangle(pt.x - px_shift, pt.y - px_shift, px_width, px_width);
                                            if (isInRect(e.X, e.Y, rc)) {
                                                AppManager.itemSelection.addBezier(new SelectedBezierPoint(bc.id, bp.getID(), BezierPickedSide.LEFT, bp));
                                                mEditingBezierOriginal = (BezierChain)bc.clone();
                                                found = true;
                                                break;
                                            }
                                        }

                                        if (bp.getControlRightType() != BezierControlType.None) {
                                            pt = getScreenCoord(bp.getControlRight());
                                            rc = new Rectangle(pt.x - px_shift, pt.y - px_shift, px_width, px_width);
                                            if (isInRect(e.X, e.Y, rc)) {
                                                AppManager.itemSelection.addBezier(new SelectedBezierPoint(bc.id, bp.getID(), BezierPickedSide.RIGHT, bp));
                                                mEditingBezierOriginal = (BezierChain)bc.clone();
                                                found = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (found) {
                                        break;
                                    }
                                }
                                if (found) {
                                    mMouseDownMode = MouseDownMode.BEZIER_MODE;
                                }
                            }

                            // ベジエ曲線の点にヒットしなかった場合
                            if (!found) {
                                #region NOT CurveMode
                                VsqEvent ve = findItemAt(e.X, e.Y);
                                // マウス位置の音符アイテムを検索
                                if (ve != null) {
                                    bool found2 = false;
                                    if ((mModifierOnMouseDown & mModifierKey) == mModifierKey) {
                                        // clicked with CTRL key
                                        List<int> list = new List<int>();
                                        foreach (var item in AppManager.itemSelection.getEventIterator()) {
                                            VsqEvent ve2 = item.original;
                                            if (ve.InternalID == ve2.InternalID) {
                                                found2 = true;
                                            } else {
                                                list.Add(ve2.InternalID);
                                            }
                                        }
                                        AppManager.itemSelection.clearEvent();
                                        AppManager.itemSelection.addEventAll(list);
                                    } else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift) {
                                        // clicked with Shift key
                                        SelectedEventEntry last_selected = AppManager.itemSelection.getLastEvent();
                                        if (last_selected != null) {
                                            int last_clock = last_selected.original.Clock;
                                            int tmin = Math.Min(ve.Clock, last_clock);
                                            int tmax = Math.Max(ve.Clock, last_clock);
                                            List<int> add_required = new List<int>();
                                            foreach (var item in AppManager.getVsqFile().Track[AppManager.getSelected()].getNoteEventIterator()) {
                                                if (tmin <= item.Clock && item.Clock <= tmax) {
                                                    add_required.Add(item.InternalID);
                                                }
                                            }
                                            AppManager.itemSelection.addEventAll(add_required);
                                        }
                                    } else {
                                        // no modefier key
                                        if (!AppManager.itemSelection.isEventContains(AppManager.getSelected(), ve.InternalID)) {
                                            AppManager.itemSelection.clearEvent();
                                        }
                                    }
                                    if (!found2) {
                                        AppManager.itemSelection.addEvent(ve.InternalID);
                                    }

                                    mMouseDownMode = MouseDownMode.VEL_WAIT_HOVER;
                                    mVelEditLastSelectedID = ve.InternalID;
                                    if (mSelectedCurve.equals(CurveType.VEL)) {
                                        if (AppManager.mDrawIsUtau[selected - 1]) {
                                            mVelEditShiftY = e.Y - yCoordFromValue(ve.UstEvent == null ? 100 : ve.UstEvent.getIntensity());
                                        } else {
                                            mVelEditShiftY = e.Y - yCoordFromValue(ve.ID.Dynamics);
                                        }
                                    } else if (mSelectedCurve.equals(CurveType.Accent)) {
                                        mVelEditShiftY = e.Y - yCoordFromValue(ve.ID.DEMaccent);
                                    } else if (mSelectedCurve.equals(CurveType.Decay)) {
                                        mVelEditShiftY = e.Y - yCoordFromValue(ve.ID.DEMdecGainRate);
                                    }
                                    mVelEditSelected.Clear();
                                    if (AppManager.itemSelection.isEventContains(AppManager.getSelected(), mVelEditLastSelectedID)) {
                                        foreach (var item in AppManager.itemSelection.getEventIterator()) {
                                            mVelEditSelected[item.original.InternalID] =
                                                                    new SelectedEventEntry(AppManager.getSelected(),
                                                                                            item.original,
                                                                                            item.editing);
                                        }
                                    } else {
                                        mVelEditSelected[mVelEditLastSelectedID] =
                                                                new SelectedEventEntry(AppManager.getSelected(),
                                                                                        (VsqEvent)ve.clone(),
                                                                                        (VsqEvent)ve.clone());
                                    }
                                    mMouseHoverThread = new Thread(new ThreadStart(MouseHoverEventGenerator));
                                    mMouseHoverThread.Start();
                                    Invalidate();
                                    return;
                                }

                                // マウス位置のデータポイントを検索
                                long id = findDataPointAt(e.X, e.Y);
                                if (id > 0) {
                                    if (AppManager.itemSelection.isPointContains(id)) {
                                        if ((mModifierOnMouseDown & mModifierKey) == mModifierKey) {
                                            AppManager.itemSelection.removePoint(id);
                                            mMouseDownMode = MouseDownMode.NONE;
                                            Invalidate();
                                            return;
                                        }
                                    } else {
                                        if ((mModifierOnMouseDown & mModifierKey) != mModifierKey) {
                                            AppManager.itemSelection.clearPoint();
                                        }
                                        AppManager.itemSelection.addPoint(mSelectedCurve, id);
                                    }

                                    mMouseDownMode = MouseDownMode.POINT_MOVE;
                                    mMovingPoints.Clear();
                                    VsqBPList list = AppManager.getVsqFile().Track[AppManager.getSelected()].getCurve(mSelectedCurve.getName());
                                    if (list != null) {
                                        int count = list.size();
                                        for (int i = 0; i < count; i++) {
                                            VsqBPPair item = list.getElementB(i);
                                            if (AppManager.itemSelection.isPointContains(item.id)) {
                                                mMovingPoints.Add(new BPPair(list.getKeyClock(i), item.value));
                                            }
                                        }
                                        Invalidate();
                                        return;
                                    }
                                } else {
                                    if ((mModifierOnMouseDown & Keys.Control) != Keys.Control) {
                                        AppManager.itemSelection.clearPoint();
                                    }
                                    if ((mModifierOnMouseDown & Keys.Shift) != Keys.Shift && (mModifierOnMouseDown & mModifierKey) != mModifierKey) {
                                        AppManager.itemSelection.clearPoint();
                                    }
                                }

                                if ((mModifierOnMouseDown & mModifierKey) != mModifierKey) {
                                    AppManager.setCurveSelectedIntervalEnabled(false);
                                }
                                if (AppManager.editorConfig.CurveSelectingQuantized) {
                                    AppManager.mCurveSelectingRectangle = new Rectangle(quantized_clock, value, 0, 0);
                                } else {
                                    AppManager.mCurveSelectingRectangle = new Rectangle(clock, value, 0, 0);
                                }
                                #endregion
                            }
                            #endregion
                        } else if (AppManager.getSelectedTool() == EditTool.ERASER) {
                            #region Eraser
                            VsqEvent ve3 = findItemAt(e.X, e.Y);
                            if (ve3 != null) {
                                AppManager.itemSelection.clearEvent();
                                CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventDelete(selected,
                                                                                                                  ve3.InternalID));
                                executeCommand(run, true);
                            } else {
                                if (AppManager.isCurveMode()) {
                                    List<BezierChain> list = vsq.AttachedCurves.get(AppManager.getSelected() - 1).get(mSelectedCurve);
                                    if (list != null) {
                                        ByRef<BezierChain> chain = new ByRef<BezierChain>();
                                        ByRef<BezierPoint> point = new ByRef<BezierPoint>();
                                        ByRef<BezierPickedSide> side = new ByRef<BezierPickedSide>();
                                        findBezierPointAt(e.X, e.Y, list, chain, point, side, DOT_WID, AppManager.editorConfig.PxToleranceBezier);
                                        if (point.value != null) {
                                            if (side.value == BezierPickedSide.BASE) {
                                                // データ点自体を削除
                                                BezierChain work = (BezierChain)chain.value.clone();
                                                int count = work.points.Count;
                                                if (count > 1) {
                                                    // 2個以上のデータ点があるので、BezierChainを置換
                                                    for (int i = 0; i < count; i++) {
                                                        BezierPoint bp = work.points[i];
                                                        if (bp.getID() == point.value.getID()) {
                                                            work.points.RemoveAt(i);
                                                            break;
                                                        }
                                                    }
                                                    CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain(selected,
                                                                                                                       mSelectedCurve,
                                                                                                                       chain.value.id,
                                                                                                                       work,
                                                                                                                       AppManager.editorConfig.getControlCurveResolutionValue());
                                                    executeCommand(run, true);
                                                    mMouseDownMode = MouseDownMode.NONE;
                                                    Invalidate();
                                                    return;
                                                } else {
                                                    // 1個しかデータ点がないので、BezierChainを削除
                                                    CadenciiCommand run = VsqFileEx.generateCommandDeleteBezierChain(AppManager.getSelected(),
                                                                                                                      mSelectedCurve,
                                                                                                                      chain.value.id,
                                                                                                                      AppManager.editorConfig.getControlCurveResolutionValue());
                                                    executeCommand(run, true);
                                                    mMouseDownMode = MouseDownMode.NONE;
                                                    Invalidate();
                                                    return;
                                                }
                                            } else {
                                                // 滑らかにするオプションを解除する
                                                BezierChain work = (BezierChain)chain.value.clone();
                                                int count = work.points.Count;
                                                for (int i = 0; i < count; i++) {
                                                    BezierPoint bp = work.points[i];
                                                    if (bp.getID() == point.value.getID()) {
                                                        bp.setControlLeftType(BezierControlType.None);
                                                        bp.setControlRightType(BezierControlType.None);
                                                        break;
                                                    }
                                                }
                                                CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain(AppManager.getSelected(),
                                                                                                                   mSelectedCurve,
                                                                                                                   chain.value.id,
                                                                                                                   work,
                                                                                                                   AppManager.editorConfig.getControlCurveResolutionValue());
                                                executeCommand(run, true);
                                                mMouseDownMode = MouseDownMode.NONE;
                                                Invalidate();
                                                return;
                                            }
                                        }
                                    }
                                } else {
                                    long id = findDataPointAt(e.X, e.Y);
                                    if (id > 0) {
                                        VsqBPList item = AppManager.getVsqFile().Track[AppManager.getSelected()].getCurve(mSelectedCurve.getName());
                                        if (item != null) {
                                            VsqBPList work = (VsqBPList)item.clone();
                                            VsqBPPairSearchContext context = work.findElement(id);
                                            if (context.point.id == id) {
                                                work.remove(context.clock);
                                                CadenciiCommand run = new CadenciiCommand(
                                                    VsqCommand.generateCommandTrackCurveReplace(selected,
                                                                                                 mSelectedCurve.getName(),
                                                                                                 work));
                                                executeCommand(run, true);
                                                mMouseDownMode = MouseDownMode.NONE;
                                                Invalidate();
                                                return;
                                            }
                                        }
                                    }
                                }

                                if ((mModifierOnMouseDown & Keys.Shift) != Keys.Shift && (mModifierOnMouseDown & mModifierKey) != mModifierKey) {
                                    AppManager.itemSelection.clearPoint();
                                }
                                if (AppManager.editorConfig.CurveSelectingQuantized) {
                                    AppManager.mCurveSelectingRectangle = new Rectangle(quantized_clock, value, 0, 0);
                                } else {
                                    AppManager.mCurveSelectingRectangle = new Rectangle(clock, value, 0, 0);
                                }
                            }
                            #endregion
                        }
                    } else if (e.Button == MouseButtons.Right) {
                        if (AppManager.isCurveMode()) {
                            if (!mSelectedCurve.equals(CurveType.VEL) && !mSelectedCurve.equals(CurveType.Env)) {
                                List<BezierChain> dict = AppManager.getVsqFile().AttachedCurves.get(AppManager.getSelected() - 1).get(mSelectedCurve);
                                AppManager.itemSelection.clearBezier();
                                bool found = false;
                                for (int i = 0; i < dict.Count; i++) {
                                    BezierChain bc = dict[i];
                                    foreach (var bp in bc.points) {
                                        Point pt = getScreenCoord(bp.getBase());
                                        Rectangle rc = new Rectangle(pt.x - DOT_WID, pt.y - DOT_WID, 2 * DOT_WID + 1, 2 * DOT_WID + 1);
                                        if (isInRect(e.X, e.Y, rc)) {
                                            AppManager.itemSelection.addBezier(new SelectedBezierPoint(bc.id, bp.getID(), BezierPickedSide.BASE, bp));
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (found) {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                } else {
                    AppManager.setCurveSelectedIntervalEnabled(false);
                }
                #endregion
            }
            Invalidate();
        }

        private bool processMouseDownBezier(MouseEventArgs e)
        {
#if DEBUG
            sout.println("TrackSelector::processMouseDownBezier");
#endif
            int clock = AppManager.clockFromXCoord(e.X);
            int max = mSelectedCurve.getMaximum();
            int min = mSelectedCurve.getMinimum();
            int value = valueFromYCoord(e.Y);
            if (value < min) {
                value = min;
            } else if (max < value) {
                value = max;
            }
            int px_shift = DOT_WID + AppManager.editorConfig.PxToleranceBezier;
            int px_width = px_shift * 2 + 1;
            Keys modifier = Control.ModifierKeys;

            int track = AppManager.getSelected();
            bool too_near = false; // clicked position is too near to existing bezier points
            bool is_middle = false;

            // check whether bezier point exists on clicked position
            List<BezierChain> dict = AppManager.getVsqFile().AttachedCurves.get(track - 1).get(mSelectedCurve);
            ByRef<BezierChain> found_chain = new ByRef<BezierChain>();
            ByRef<BezierPoint> found_point = new ByRef<BezierPoint>();
            ByRef<BezierPickedSide> found_side = new ByRef<BezierPickedSide>();
            findBezierPointAt(
                e.X, e.Y,
                dict, found_chain, found_point, found_side,
                DOT_WID, AppManager.editorConfig.PxToleranceBezier);
#if DEBUG
            sout.println("TrackSelector::processMouseDownBezier; (found_chain.value==null)=" + (found_chain.value == null));
#endif

            if (found_chain.value != null) {
                AppManager.itemSelection.addBezier(
                    new SelectedBezierPoint(
                        found_chain.value.id, found_point.value.getID(),
                        found_side.value, found_point.value));
                mEditingBezierOriginal = (BezierChain)found_chain.value.clone();
                mMouseDownMode = MouseDownMode.BEZIER_MODE;
            } else {
                if (AppManager.getSelectedTool() != EditTool.PENCIL) {
                    return false;
                }
                BezierChain target_chain = null;
                for (int j = 0; j < dict.Count; j++) {
                    BezierChain bc = dict[j];
                    for (int i = 1; i < bc.size(); i++) {
                        if (!is_middle && bc.points[i - 1].getBase().getX() <= clock && clock <= bc.points[i].getBase().getX()) {
                            target_chain = (BezierChain)bc.clone();
                            is_middle = true;
                        }
                        if (!too_near) {
                            foreach (var bp in bc.points) {
                                Point pt = getScreenCoord(bp.getBase());
                                Rectangle rc = new Rectangle(pt.x - px_shift, pt.y - px_shift, px_width, px_width);
                                if (isInRect(e.X, e.Y, rc)) {
                                    too_near = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!too_near) {
                    if ((modifier & mModifierKey) != mModifierKey && target_chain == null) {
                        // search BezierChain just before the clicked position
                        int tmax = -1;
                        for (int i = 0; i < dict.Count; i++) {
                            BezierChain bc = dict[i];
                            bc.points.Sort();
                            // check most nearest data point from clicked position
                            int last = (int)bc.points[bc.points.Count - 1].getBase().getX();
                            if (tmax < last && last < clock) {
                                tmax = last;
                                target_chain = (BezierChain)bc.clone();
                            }
                        }
                    }

#if DEBUG
                    AppManager.debugWriteLine("    (target_chain==null)=" + (target_chain == null));
                    if (target_chain != null && found_point.value != null) {
                        sout.println("TrackSelector::procesMouseDownBezier; before:(" + found_point.value.getPosition(found_side.value) + "," + found_point.value.getPosition(found_side.value) + "); after:(" + clock + "," + value + ")");
                    }
#endif
                    // fork whether target_chain is null or not
                    PointD pt = new PointD(clock, value);
                    BezierPoint bp = null;
                    int chain_id = -1;
                    int point_id = -1;
                    if (target_chain == null) {
                        // generate new BezierChain
                        BezierChain adding = new BezierChain();
                        bp = new BezierPoint(pt, pt, pt);
                        point_id = adding.getNextId();
                        bp.setID(point_id);
                        adding.add(bp);
                        chain_id = AppManager.getVsqFile().AttachedCurves.get(track - 1).getNextId(mSelectedCurve);
#if DEBUG
                        AppManager.debugWriteLine("    new chain_id=" + chain_id);
#endif
                        CadenciiCommand run = VsqFileEx.generateCommandAddBezierChain(track,
                                                                                mSelectedCurve,
                                                                                chain_id,
                                                                                AppManager.editorConfig.getControlCurveResolutionValue(),
                                                                                adding);
                        executeCommand(run, false);
                        mMouseDownMode = MouseDownMode.BEZIER_ADD_NEW;
                    } else {
                        mEditingBezierOriginal = (BezierChain)target_chain.clone();
                        bp = new BezierPoint(pt, pt, pt);
                        point_id = target_chain.getNextId();
                        bp.setID(point_id);
                        target_chain.add(bp);
                        target_chain.points.Sort();
                        chain_id = target_chain.id;
                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain(track,
                                                                                    mSelectedCurve,
                                                                                    target_chain.id,
                                                                                    target_chain,
                                                                                    AppManager.editorConfig.getControlCurveResolutionValue());
                        executeCommand(run, false);
                        mMouseDownMode = MouseDownMode.BEZIER_EDIT;
                    }
                    AppManager.itemSelection.clearBezier();
                    AppManager.itemSelection.addBezier(new SelectedBezierPoint(chain_id, point_id, BezierPickedSide.BASE, bp));
                } else {
                    mMouseDownMode = MouseDownMode.NONE;
                }
            }
            return true;
        }

        private bool processMouseDownPreutteranceAndOverlap(MouseEventArgs e)
        {
            ByRef<int> internal_id = new ByRef<int>();
            ByRef<Boolean> found_flag_was_overlap = new ByRef<Boolean>();
            if (findPreUtteranceOrOverlapAt(e.X, e.Y, internal_id, found_flag_was_overlap)) {
                if (found_flag_was_overlap.value) {
                    mOverlapEditingID = internal_id.value;
                    mPreOverlapEditing = AppManager.getVsqFile().Track[AppManager.getSelected()].findEventFromID(mOverlapEditingID);
                    if (mPreOverlapEditing == null) {
                        mMouseDownMode = MouseDownMode.NONE;
                        return false;
                    }
                    mPreOverlapOriginal = (VsqEvent)mPreOverlapEditing.clone();
                    mMouseDownMode = MouseDownMode.OVERLAP_MOVE;
                    return true;
                } else {
                    mPreUtteranceEditingID = internal_id.value;
                    mPreOverlapEditing = AppManager.getVsqFile().Track[AppManager.getSelected()].findEventFromID(mPreUtteranceEditingID);
                    if (mPreOverlapEditing == null) {
                        mMouseDownMode = MouseDownMode.NONE;
                        return false;
                    }
                    mPreOverlapOriginal = (VsqEvent)mPreOverlapEditing.clone();
                    mMouseDownMode = MouseDownMode.PRE_UTTERANCE_MOVE;
                    return true;
                }
            }
            return false;
        }

        private bool processMouseDownEnvelope(MouseEventArgs e)
        {
            ByRef<int> internal_id = new ByRef<int>(-1);
            ByRef<int> point_kind = new ByRef<int>(-1);
            if (!findEnvelopePointAt(e.X, e.Y, internal_id, point_kind)) {
                return false;
            }
#if DEBUG
            sout.println("processTrackSelectorMouseDownForEnvelope; internal_id=" + internal_id.value + "; point_kind=" + point_kind.value);
#endif
            mEnvelopeOriginal = null;
            VsqFileEx vsq = AppManager.getVsqFile();
            VsqTrack vsq_track = vsq.Track[AppManager.getSelected()];
            VsqEvent found = vsq_track.findEventFromID(internal_id.value);
            if (found == null) {
                return false;
            }
            if (found.UstEvent != null && found.UstEvent.getEnvelope() != null) {
                mEnvelopeOriginal = (UstEnvelope)found.UstEvent.getEnvelope().clone();
                mEnvelopeEditing = found.UstEvent.getEnvelope();
            }
            if (mEnvelopeOriginal == null) {
                found.UstEvent.setEnvelope(new UstEnvelope());
                mEnvelopeEditing = found.UstEvent.getEnvelope();
                mEnvelopeOriginal = (UstEnvelope)found.UstEvent.getEnvelope().clone();
            }
            mMouseDownMode = MouseDownMode.ENVELOPE_MOVE;
            mEnvelopeEdigintID = internal_id.value;
            mEnvelopePointKind = point_kind.value;

            // エンベロープ点が移動可能な範囲を、あらかじめ取得
            // 描画される位置を取得
            VsqEvent item_prev = null;
            VsqEvent item = null;
            VsqEvent item_next = null;
            IEnumerator<VsqEvent> itr = vsq_track.getNoteEventIterator().GetEnumerator();
            while (true) {
                item_prev = item;
                item = item_next;
                if (itr.MoveNext()) {
                    item_next = itr.Current;
                } else {
                    item_next = null;
                }
                if (item_prev == null && item == null && item_next == null) {
                    break;
                }
                if (item == null) {
                    continue;
                }
                if (item.InternalID == mEnvelopeEdigintID) {
                    break;
                }
            }
            if (item_prev != null) {
                // 直前の音符と接触しているかどうか
                if (item_prev.Clock + item_prev.ID.getLength() < item.Clock) {
                    item_prev = null;
                }
            }
            if (item_next != null) {
                // 直後の音符と接触しているかどうか
                if (item.Clock + item.ID.getLength() < item_next.Clock) {
                    item_next = null;
                }
            }
            ByRef<Double> env_start = new ByRef<Double>(0.0);
            ByRef<Double> env_end = new ByRef<Double>(0.0);
            getEnvelopeRegion(vsq.TempoTable, item_prev, item, item_next, env_start, env_end);

            mEnvelopeRangeBegin = env_start.value;
            mEnvelopeRangeEnd = env_end.value;
            if (mEnvelopePointKind == 1) {
                mEnvelopeDotBegin = mEnvelopeRangeBegin;
                mEnvelopeDotEnd = mEnvelopeRangeEnd - (mEnvelopeOriginal.p4 + mEnvelopeOriginal.p3 + mEnvelopeOriginal.p5 + mEnvelopeOriginal.p2) / 1000.0;
            } else if (mEnvelopePointKind == 2) {
                mEnvelopeDotBegin = mEnvelopeRangeBegin + mEnvelopeOriginal.p1 / 1000.0;
                mEnvelopeDotEnd = mEnvelopeRangeEnd - (mEnvelopeOriginal.p4 + mEnvelopeOriginal.p3 + mEnvelopeOriginal.p5) / 1000.0;
            } else if (mEnvelopePointKind == 3) {
                mEnvelopeDotBegin = mEnvelopeRangeBegin + (mEnvelopeOriginal.p1 + mEnvelopeOriginal.p2) / 1000.0;
                mEnvelopeDotEnd = mEnvelopeRangeEnd - (mEnvelopeOriginal.p4 + mEnvelopeOriginal.p3) / 1000.0;
            } else if (mEnvelopePointKind == 4) {
                mEnvelopeDotBegin = mEnvelopeRangeBegin + (mEnvelopeOriginal.p1 + mEnvelopeOriginal.p2 + mEnvelopeOriginal.p5) / 1000.0;
                mEnvelopeDotEnd = mEnvelopeRangeEnd - mEnvelopeOriginal.p4 / 1000.0;
            } else if (mEnvelopePointKind == 5) {
                mEnvelopeDotBegin = mEnvelopeRangeBegin + (mEnvelopeOriginal.p1 + mEnvelopeOriginal.p2 + mEnvelopeOriginal.p5 + mEnvelopeOriginal.p3) / 1000.0;
                mEnvelopeDotEnd = mEnvelopeRangeEnd;
            }
            return true;
        }

        private void changeCurve(CurveType curve)
        {
#if DEBUG
            sout.println("TrackSelector#changCurve; getViewingCurveCount()=" + mViewingCurves.Count);
#endif
            if (!mSelectedCurve.equals(curve)) {
                mLastSelectedCurve = mSelectedCurve;
                mSelectedCurve = curve;
                try {
                    if (SelectedCurveChanged != null) {
                        SelectedCurveChanged.Invoke(this, curve);
                    }
                } catch (Exception ex) {
                    serr.println("TrackSelector#changeCurve; ex=" + ex);
                }
            }
        }

        private static bool isInRect(int x, int y, Rectangle rc)
        {
            if (rc.x <= x && x <= rc.x + rc.width) {
                if (rc.y <= y && y <= rc.y + rc.height) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }

        /// <summary>
        /// クリックされた位置にある音符イベントまたは歌手変更イベントを取得します
        /// </summary>
        /// <param name="location"></param>
        /// <param name="position_x"></param>
        /// <returns></returns>
        private VsqEvent findItemAt(int locx, int locy)
        {
            if (AppManager.getVsqFile() == null) {
                return null;
            }
            VsqTrack target = AppManager.getVsqFile().Track[AppManager.getSelected()];
            int count = target.getEventCount();
            for (int i = 0; i < count; i++) {
                VsqEvent ve = target.getEvent(i);
                if (ve.ID.type == VsqIDType.Singer) {
                    int x = AppManager.xCoordFromClocks(ve.Clock);
                    if (getHeight() - 2 * OFFSET_TRACK_TAB <= locy &&
                         locy <= getHeight() - OFFSET_TRACK_TAB &&
                         x <= locx && locx <= x + SINGER_ITEM_WIDTH) {
                        return ve;
                    } else if (getWidth() < x) {
                        //return null;
                    }
                } else if (ve.ID.type == VsqIDType.Anote) {
                    int x = AppManager.xCoordFromClocks(ve.Clock);
                    int y = 0;
                    if (mSelectedCurve.equals(CurveType.VEL)) {
                        y = yCoordFromValue(ve.ID.Dynamics);
                    } else if (mSelectedCurve.equals(CurveType.Accent)) {
                        y = yCoordFromValue(ve.ID.DEMaccent);
                    } else if (mSelectedCurve.equals(CurveType.Decay)) {
                        y = yCoordFromValue(ve.ID.DEMdecGainRate);
                    } else {
                        continue;
                    }
                    if (0 <= locy && locy <= getHeight() - 2 * OFFSET_TRACK_TAB &&
                         AppManager.keyWidth <= locx && locx <= getWidth()) {
                        if (y <= locy && locy <= getHeight() - FOOTER && x <= locx && locx <= x + VEL_BAR_WIDTH) {
                            return ve;
                        }
                    }
                }
            }
            return null;
        }

        public void TrackSelector_MouseUp(Object sender, MouseEventArgs e)
        {
#if DEBUG
            AppManager.debugWriteLine("TrackSelector_MouseUp");
#endif
            mMouseDowned = false;
            if (mMouseHoverThread != null) {
                if (mMouseHoverThread.IsAlive) {
                    mMouseHoverThread.Abort();
                }
            }

            if (!mCurveVisible) {
                mMouseDownMode = MouseDownMode.NONE;
                Invalidate();
                return;
            }

            int selected = AppManager.getSelected();
            bool is_utau_mode = AppManager.mDrawIsUtau[selected - 1];
            int stdx = AppManager.mMainWindowController.getStartToDrawX();

            int max = mSelectedCurve.getMaximum();
            int min = mSelectedCurve.getMinimum();
            VsqFileEx vsq = AppManager.getVsqFile();
            VsqTrack vsq_track = vsq.Track[selected];
#if DEBUG
            AppManager.debugWriteLine("    max,min=" + max + "," + min);
#endif
            if (mMouseDownMode == MouseDownMode.BEZIER_ADD_NEW ||
                 mMouseDownMode == MouseDownMode.BEZIER_MODE ||
                 mMouseDownMode == MouseDownMode.BEZIER_EDIT) {
                if (e.Button == MouseButtons.Left && sender is TrackSelector) {
                    int chain_id = AppManager.itemSelection.getLastBezier().chainID;
                    BezierChain edited = (BezierChain)vsq.AttachedCurves.get(selected - 1).getBezierChain(mSelectedCurve, chain_id).clone();
                    if (mMouseDownMode == MouseDownMode.BEZIER_ADD_NEW) {
                        edited.id = chain_id;
                        CadenciiCommand pre = VsqFileEx.generateCommandDeleteBezierChain(selected,
                                                                                          mSelectedCurve,
                                                                                          chain_id,
                                                                                          AppManager.editorConfig.getControlCurveResolutionValue());
                        executeCommand(pre, false);
                        CadenciiCommand run = VsqFileEx.generateCommandAddBezierChain(selected,
                                                                                       mSelectedCurve,
                                                                                       chain_id,
                                                                                       AppManager.editorConfig.getControlCurveResolutionValue(),
                                                                                       edited);
                        executeCommand(run, true);
                    } else if (mMouseDownMode == MouseDownMode.BEZIER_EDIT) {
                        CadenciiCommand pre = VsqFileEx.generateCommandReplaceBezierChain(selected,
                                                                                           mSelectedCurve,
                                                                                           chain_id,
                                                                                           mEditingBezierOriginal,
                                                                                           AppManager.editorConfig.getControlCurveResolutionValue());
                        executeCommand(pre, false);
                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain(selected,
                                                                                           mSelectedCurve,
                                                                                           chain_id,
                                                                                           edited,
                                                                                           AppManager.editorConfig.getControlCurveResolutionValue());
                        executeCommand(run, true);
                    } else if (mMouseDownMode == MouseDownMode.BEZIER_MODE && mMouseMoved) {
                        vsq.AttachedCurves.get(selected - 1).setBezierChain(mSelectedCurve, chain_id, mEditingBezierOriginal);
                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain(selected,
                                                                                           mSelectedCurve,
                                                                                           chain_id,
                                                                                           edited,
                                                                                           AppManager.editorConfig.getControlCurveResolutionValue());
                        executeCommand(run, true);
#if DEBUG
                        AppManager.debugWriteLine("    m_mouse_down_mode=" + mMouseDownMode);
                        AppManager.debugWriteLine("    chain_id=" + chain_id);
#endif

                    }
                }
            } else if (mMouseDownMode == MouseDownMode.CURVE_EDIT ||
                      mMouseDownMode == MouseDownMode.VEL_WAIT_HOVER) {
                if (e.Button == MouseButtons.Left) {
                    if (AppManager.getSelectedTool() == EditTool.ARROW) {
                        #region Arrow
                        if (mSelectedCurve.equals(CurveType.Env)) {

                        } else if (!mSelectedCurve.equals(CurveType.VEL) && !mSelectedCurve.equals(CurveType.Accent) && !mSelectedCurve.equals(CurveType.Decay)) {
                            if (AppManager.mCurveSelectingRectangle.width == 0) {
                                AppManager.setCurveSelectedIntervalEnabled(false);
                            } else {
                                if (!AppManager.isCurveSelectedIntervalEnabled()) {
                                    int start = Math.Min(AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width);
                                    int end = Math.Max(AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width);
                                    AppManager.mCurveSelectedInterval = new SelectedRegion(start);
                                    AppManager.mCurveSelectedInterval.setEnd(end);
#if DEBUG
                                    AppManager.debugWriteLine("TrackSelector#TrackSelector_MouseUp; selected_region is set to TRUE");
#endif
                                    AppManager.setCurveSelectedIntervalEnabled(true);
                                } else {
                                    int start = Math.Min(AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width);
                                    int end = Math.Max(AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width);
                                    int old_start = AppManager.mCurveSelectedInterval.getStart();
                                    int old_end = AppManager.mCurveSelectedInterval.getEnd();
                                    AppManager.mCurveSelectedInterval = new SelectedRegion(Math.Min(start, old_start));
                                    AppManager.mCurveSelectedInterval.setEnd(Math.Max(end, old_end));
                                }

                                if ((mModifierOnMouseDown & Keys.Control) != Keys.Control) {
#if DEBUG
                                    sout.println("TrackSelector#TrackSelector_MouseUp; CTRL was not pressed");
#endif
                                    AppManager.itemSelection.clearPoint();
                                }
                                if (!mSelectedCurve.equals(CurveType.Accent) &&
                                     !mSelectedCurve.equals(CurveType.Decay) &&
                                     !mSelectedCurve.equals(CurveType.Env) &&
                                     !mSelectedCurve.equals(CurveType.VEL) &&
                                     !mSelectedCurve.equals(CurveType.VibratoDepth) &&
                                     !mSelectedCurve.equals(CurveType.VibratoRate)) {
                                    VsqBPList list = vsq_track.getCurve(mSelectedCurve.getName());
                                    int count = list.size();
                                    Rectangle rc = new Rectangle(Math.Min(AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width),
                                                                  Math.Min(AppManager.mCurveSelectingRectangle.y, AppManager.mCurveSelectingRectangle.y + AppManager.mCurveSelectingRectangle.height),
                                                                  Math.Abs(AppManager.mCurveSelectingRectangle.width),
                                                                  Math.Abs(AppManager.mCurveSelectingRectangle.height));
#if DEBUG
                                    sout.println("TrackSelectro#TrackSelectro_MouseUp; rc={x=" + rc.x + ", y=" + rc.y + ", width=" + rc.width + ", height=" + rc.height + "}");
#endif
                                    for (int i = 0; i < count; i++) {
                                        int clock = list.getKeyClock(i);
                                        VsqBPPair item = list.getElementB(i);
                                        if (isInRect(clock, item.value, rc)) {
#if DEBUG
                                            sout.println("TrackSelector#TrackSelectro_MosueUp; selected; clock=" + clock + "; id=" + item.id);
#endif
                                            AppManager.itemSelection.addPoint(mSelectedCurve, item.id);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    } else if (AppManager.getSelectedTool() == EditTool.ERASER) {
                        #region Eraser
                        if (AppManager.isCurveMode()) {
                            List<BezierChain> list = vsq.AttachedCurves.get(selected - 1).get(mSelectedCurve);
                            if (list != null) {
                                int x = Math.Min(AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width);
                                int y = Math.Min(AppManager.mCurveSelectingRectangle.y, AppManager.mCurveSelectingRectangle.y + AppManager.mCurveSelectingRectangle.height);
                                Rectangle rc = new Rectangle(x, y, Math.Abs(AppManager.mCurveSelectingRectangle.width), Math.Abs(AppManager.mCurveSelectingRectangle.height));

                                bool changed = false; //1箇所でも削除が実行されたらtrue

                                int count = list.Count;
                                List<BezierChain> work = new List<BezierChain>();
                                for (int i = 0; i < count; i++) {
                                    BezierChain chain = list[i];
                                    BezierChain chain_copy = new BezierChain();
                                    chain_copy.setColor(chain.getColor());
                                    chain_copy.Default = chain.Default;
                                    chain_copy.id = chain.id;
                                    int point_count = chain.points.Count;
                                    for (int j = 0; j < point_count; j++) {
                                        BezierPoint point = chain.points[j];
                                        Point basepoint = point.getBase().toPoint();
                                        Point ctrl_l = point.getControlLeft().toPoint();
                                        Point ctrl_r = point.getControlRight().toPoint();
                                        if (isInRect(basepoint.x, basepoint.y, rc)) {
                                            // データ点が選択範囲に入っているので、追加しない
                                            changed = true;
                                            continue;
                                        } else {
                                            if ((point.getControlLeftType() != BezierControlType.None && isInRect(ctrl_l.x, ctrl_l.y, rc)) ||
                                                 (point.getControlRightType() != BezierControlType.None && isInRect(ctrl_r.x, ctrl_r.y, rc))) {
                                                // 制御点が選択範囲に入っているので、「滑らかにする」オプションを解除して追加
                                                BezierPoint point_copy = (BezierPoint)point.clone();
                                                point_copy.setControlLeftType(BezierControlType.None);
                                                point_copy.setControlRightType(BezierControlType.None);
                                                chain_copy.points.Add(point_copy);
                                                changed = true;
                                                continue;
                                            } else {
                                                // 選択範囲に入っていないので、普通に追加
                                                chain_copy.points.Add((BezierPoint)point.clone());
                                            }
                                        }
                                    }
                                    if (chain_copy.points.Count > 0) {
                                        work.Add(chain_copy);
                                    }
                                }
                                if (changed) {
                                    SortedDictionary<CurveType, List<BezierChain>> comm = new SortedDictionary<CurveType, List<BezierChain>>();
                                    comm[mSelectedCurve] = work;
                                    CadenciiCommand run = VsqFileEx.generateCommandReplaceAttachedCurveRange(selected, comm);
                                    executeCommand(run, true);
                                }
                            }
                        } else {
                            if (mSelectedCurve.equals(CurveType.VEL) || mSelectedCurve.equals(CurveType.Accent) || mSelectedCurve.equals(CurveType.Decay)) {
                                #region VEL Accent Delay
                                int start = Math.Min(AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width);
                                int end = Math.Max(AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width);
                                int old_start = AppManager.mCurveSelectedInterval.getStart();
                                int old_end = AppManager.mCurveSelectedInterval.getEnd();
                                AppManager.mCurveSelectedInterval = new SelectedRegion(Math.Min(start, old_start));
                                AppManager.mCurveSelectedInterval.setEnd(Math.Max(end, old_end));
                                AppManager.itemSelection.clearEvent();
                                List<int> deleting = new List<int>();
                                foreach (var ev in vsq_track.getNoteEventIterator()) {
                                    if (start <= ev.Clock && ev.Clock <= end) {
                                        deleting.Add(ev.InternalID);
                                    }
                                }
                                if (deleting.Count > 0) {
                                    CadenciiCommand er_run = new CadenciiCommand(
                                        VsqCommand.generateCommandEventDeleteRange(selected, deleting));
                                    executeCommand(er_run, true);
                                }
                                #endregion
                            } else if (mSelectedCurve.equals(CurveType.VibratoRate) || mSelectedCurve.equals(CurveType.VibratoDepth)) {
                                #region VibratoRate ViratoDepth
                                int er_start = Math.Min(AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width);
                                int er_end = Math.Max(AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width);
                                List<int> internal_ids = new List<int>();
                                List<VsqID> items = new List<VsqID>();
                                foreach (var ve in vsq_track.getNoteEventIterator()) {
                                    if (ve.ID.VibratoHandle == null) {
                                        continue;
                                    }
                                    int cl_vib_start = ve.Clock + ve.ID.VibratoDelay;
                                    int cl_vib_length = ve.ID.getLength() - ve.ID.VibratoDelay;
                                    int cl_vib_end = cl_vib_start + cl_vib_length;
                                    int clear_start = int.MaxValue;
                                    int clear_end = int.MinValue;
                                    if (er_start < cl_vib_start && cl_vib_start < er_end && er_end <= cl_vib_end) {
                                        // cl_vib_startからer_endまでをリセット
                                        clear_start = cl_vib_start;
                                        clear_end = er_end;
                                    } else if (cl_vib_start <= er_start && er_end <= cl_vib_end) {
                                        // er_startからer_endまでをリセット
                                        clear_start = er_start;
                                        clear_end = er_end;
                                    } else if (cl_vib_start < er_start && er_start < cl_vib_end && cl_vib_end < er_end) {
                                        // er_startからcl_vib_endまでをリセット
                                        clear_start = er_start;
                                        clear_end = cl_vib_end;
                                    } else if (er_start < cl_vib_start && cl_vib_end < er_end) {
                                        // 全部リセット
                                        clear_start = cl_vib_start;
                                        clear_end = cl_vib_end;
                                    }
                                    if (clear_start < clear_end) {
                                        float f_clear_start = (clear_start - cl_vib_start) / (float)cl_vib_length;
                                        float f_clear_end = (clear_end - cl_vib_start) / (float)cl_vib_length;
                                        VsqID item = (VsqID)ve.ID.clone();
                                        VibratoBPList target = null;
                                        if (mSelectedCurve.equals(CurveType.VibratoDepth)) {
                                            target = item.VibratoHandle.getDepthBP();
                                        } else {
                                            target = item.VibratoHandle.getRateBP();
                                        }
                                        List<float> bpx = new List<float>();
                                        List<int> bpy = new List<int>();
                                        bool start_added = false;
                                        bool end_added = false;
                                        for (int i = 0; i < target.getCount(); i++) {
                                            VibratoBPPair vbpp = target.getElement(i);
                                            if (vbpp.X < f_clear_start) {
                                                bpx.Add(vbpp.X);
                                                bpy.Add(vbpp.Y);
                                            } else if (f_clear_start == vbpp.X) {
                                                bpx.Add(vbpp.X);
                                                bpy.Add(64);
                                                start_added = true;
                                            } else if (f_clear_start < vbpp.X && !start_added) {
                                                bpx.Add(f_clear_start);
                                                bpy.Add(64);
                                                start_added = true;
                                            } else if (f_clear_end == vbpp.X) {
                                                bpx.Add(vbpp.X);
                                                bpy.Add(vbpp.Y);
                                                end_added = true;
                                            } else if (f_clear_end < vbpp.X && !end_added) {
                                                int y = vbpp.Y;
                                                if (i > 0) {
                                                    y = target.getElement(i - 1).Y;
                                                }
                                                bpx.Add(f_clear_end);
                                                bpy.Add(y);
                                                end_added = true;
                                                bpx.Add(vbpp.X);
                                                bpy.Add(vbpp.Y);
                                            } else if (f_clear_end < vbpp.X) {
                                                bpx.Add(vbpp.X);
                                                bpy.Add(vbpp.Y);
                                            }
                                        }
                                        if (mSelectedCurve.equals(CurveType.VibratoDepth)) {
                                            item.VibratoHandle.setDepthBP(
                                                new VibratoBPList(
                                                    PortUtil.convertFloatArray(bpx.ToArray()),
                                                    PortUtil.convertIntArray(bpy.ToArray())));
                                        } else {
                                            item.VibratoHandle.setRateBP(
                                                new VibratoBPList(
                                                    PortUtil.convertFloatArray(bpx.ToArray()),
                                                    PortUtil.convertIntArray(bpy.ToArray())));
                                        }
                                        internal_ids.Add(ve.InternalID);
                                        items.Add(item);
                                    }
                                }
                                CadenciiCommand run = new CadenciiCommand(
                                    VsqCommand.generateCommandEventChangeIDContaintsRange(selected,
                                                                                           PortUtil.convertIntArray(internal_ids.ToArray()),
                                                                                           items.ToArray()));
                                executeCommand(run, true);
                                #endregion
                            } else if (mSelectedCurve.equals(CurveType.Env)) {

                            } else {
                                #region Other Curves
                                VsqBPList work = vsq_track.getCurve(mSelectedCurve.getName());

                                // 削除するべきデータ点のリストを作成
                                int x = Math.Min(AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width);
                                int y = Math.Min(AppManager.mCurveSelectingRectangle.y, AppManager.mCurveSelectingRectangle.y + AppManager.mCurveSelectingRectangle.height);
                                Rectangle rc = new Rectangle(x, y, Math.Abs(AppManager.mCurveSelectingRectangle.width), Math.Abs(AppManager.mCurveSelectingRectangle.height));
                                List<long> delete = new List<long>();
                                int count = work.size();
                                for (int i = 0; i < count; i++) {
                                    int clock = work.getKeyClock(i);
                                    VsqBPPair item = work.getElementB(i);
                                    if (isInRect(clock, item.value, rc)) {
                                        delete.Add(item.id);
                                    }
                                }

                                if (delete.Count > 0) {
                                    CadenciiCommand run_eraser = new CadenciiCommand(
                                        VsqCommand.generateCommandTrackCurveEdit2(selected, mSelectedCurve.getName(), delete, new SortedDictionary<int, VsqBPPair>()));
                                    executeCommand(run_eraser, true);
                                }
                                #endregion
                            }
                        }
                        #endregion
                    } else if (!AppManager.isCurveMode() && (AppManager.getSelectedTool() == EditTool.PENCIL || AppManager.getSelectedTool() == EditTool.LINE)) {
                        #region Pencil & Line
                        mMouseTracer.append(e.X + stdx, e.Y);
                        if (mPencilMoved) {
                            if (mSelectedCurve.equals(CurveType.VEL) || mSelectedCurve.equals(CurveType.Accent) || mSelectedCurve.equals(CurveType.Decay)) {
                                #region VEL Accent Decay
                                int start = mMouseTracer.firstKey();
                                int end = mMouseTracer.lastKey();
                                start = AppManager.clockFromXCoord(start - stdx);
                                end = AppManager.clockFromXCoord(end - stdx);
#if DEBUG
                                AppManager.debugWriteLine("        start=" + start);
                                AppManager.debugWriteLine("        end=" + end);
#endif
                                SortedDictionary<int, int> velocity = new SortedDictionary<int, int>();
                                foreach (var ve in vsq_track.getNoteEventIterator()) {
                                    if (start <= ve.Clock && ve.Clock < end) {
                                        int i = -1;
                                        int lkey = 0;
                                        int lvalue = 0;
                                        int count = mMouseTracer.size();
                                        foreach (var p in mMouseTracer.iterator()) {
                                            i++;
                                            int key = p.x;
                                            int value = p.y;
                                            if (i == 0) {
                                                lkey = key;
                                                lvalue = value;
                                                continue;
                                            }
                                            int key0 = lkey;
                                            int key1 = key;
                                            int key0_clock = AppManager.clockFromXCoord(key0 - stdx);
                                            int key1_clock = AppManager.clockFromXCoord(key1 - stdx);
#if DEBUG
                                            AppManager.debugWriteLine("        key0,key1=" + key0 + "," + key1);
#endif
                                            if (key0_clock < ve.Clock && ve.Clock < key1_clock) {
                                                int key0_value = valueFromYCoord(lvalue);
                                                int key1_value = valueFromYCoord(value);
                                                float a = (key1_value - key0_value) / (float)(key1_clock - key0_clock);
                                                float b = key0_value - a * key0_clock;
                                                int new_value = (int)(a * ve.Clock + b);
                                                velocity[ve.InternalID] = new_value;
                                            } else if (key0_clock == ve.Clock) {
                                                velocity[ve.InternalID] = valueFromYCoord(lvalue);
                                            } else if (key1_clock == ve.Clock) {
                                                velocity[ve.InternalID] = valueFromYCoord(value);
                                            }
                                            lkey = key;
                                            lvalue = value;
                                        }
                                    }
                                }
                                if (velocity.Count > 0) {
                                    List<ValuePair<int, int>> cpy = new List<ValuePair<int, int>>();
                                    foreach (var internal_id in velocity.Keys) {
                                        int value = (int)velocity[internal_id];
                                        cpy.Add(new ValuePair<int, int>(internal_id, value));
                                    }
                                    CadenciiCommand run = null;
                                    if (mSelectedCurve.equals(CurveType.VEL)) {
                                        if (is_utau_mode) {
                                            int size = velocity.Count;
                                            VsqEvent[] events = new VsqEvent[size];
                                            int i = 0;
                                            foreach (var internal_id in velocity.Keys) {
                                                VsqEvent item = (VsqEvent)vsq_track.findEventFromID(internal_id).clone();
                                                if (item.UstEvent == null) {
                                                    item.UstEvent = new UstEvent();
                                                }
                                                item.UstEvent.setIntensity(velocity[internal_id]);
                                                events[i] = item;
                                                i++;
                                            }
                                            run = new CadenciiCommand(
                                                VsqCommand.generateCommandEventReplaceRange(
                                                    selected, events));
                                        } else {
                                            run = new CadenciiCommand(
                                                VsqCommand.generateCommandEventChangeVelocity(selected, cpy));
                                        }
                                    } else if (mSelectedCurve.equals(CurveType.Accent)) {
                                        run = new CadenciiCommand(
                                            VsqCommand.generateCommandEventChangeAccent(selected, cpy));
                                    } else if (mSelectedCurve.equals(CurveType.Decay)) {
                                        run = new CadenciiCommand(
                                            VsqCommand.generateCommandEventChangeDecay(selected, cpy));
                                    }
                                    executeCommand(run, true);
                                }
                                #endregion
                            } else if (mSelectedCurve.equals(CurveType.VibratoRate) || mSelectedCurve.equals(CurveType.VibratoDepth)) {
                                #region VibratoRate || VibratoDepth
                                int step_clock = AppManager.editorConfig.getControlCurveResolutionValue();
                                int step_px = (int)(step_clock * AppManager.mMainWindowController.getScaleX());
                                if (step_px <= 0) {
                                    step_px = 1;
                                }
                                int start = mMouseTracer.firstKey();
                                int end = mMouseTracer.lastKey();
#if DEBUG
                                AppManager.debugWriteLine("    start,end=" + start + " " + end);
#endif
                                List<int> internal_ids = new List<int>();
                                List<VsqID> items = new List<VsqID>();
                                foreach (var ve in vsq_track.getNoteEventIterator()) {
                                    if (ve.ID.VibratoHandle == null) {
                                        continue;
                                    }
                                    int cl_vib_start = ve.Clock + ve.ID.VibratoDelay;
                                    float cl_vib_length = ve.ID.getLength() - ve.ID.VibratoDelay;

                                    // 仮想スクリーン上の、ビブラートの描画開始位置
                                    int vib_start = AppManager.xCoordFromClocks(cl_vib_start) + stdx;

                                    // 仮想スクリーン上の、ビブラートの描画終了位置
                                    int vib_end = AppManager.xCoordFromClocks(ve.Clock + ve.ID.getLength()) + stdx;

                                    // マウスのトレースと、ビブラートの描画範囲がオーバーラップしている部分を検出
                                    int chk_start = Math.Max(vib_start, start);
                                    int chk_end = Math.Min(vib_end, end);
                                    if (chk_end <= chk_start) {
                                        // オーバーラップしていないのでスキップ
                                        continue;
                                    }

                                    float add_min = (AppManager.clockFromXCoord(chk_start - stdx) - cl_vib_start) / cl_vib_length;
                                    float add_max = (AppManager.clockFromXCoord(chk_end - stdx) - cl_vib_start) / cl_vib_length;

                                    List<ValuePair<float, int>> edit = new List<ValuePair<float, int>>();
                                    int lclock = -2 * step_clock;
                                    ValuePair<float, int> first = null; // xの値が0以下の最大のデータ点
                                    ValuePair<float, int> last = null;//xの値が1以上の最小のデータ点
                                    foreach (var p in mMouseTracer.iterator()) {
                                        if (p.x < chk_start) {
                                            continue;
                                        } else if (chk_end < p.x) {
                                            break;
                                        }
                                        int clock = AppManager.clockFromXCoord(p.x - stdx);
                                        if (clock - lclock < step_clock) {
                                            continue;
                                        }
                                        int val = valueFromYCoord(p.y);
                                        if (val < min) {
                                            val = min;
                                        } else if (max < val) {
                                            val = max;
                                        }
                                        float x = (clock - cl_vib_start) / cl_vib_length;
                                        ValuePair<float, int> tmp = new ValuePair<float, int>(x, val);
                                        if (0.0f < x && x < 1.0f) {
                                            edit.Add(tmp);
                                        } else if (x <= 0.0f) {
                                            first = tmp;
                                        } else if (1.0f <= x && last != null) {
                                            last = tmp;
                                        }
                                        lclock = clock;
                                    }
                                    if (first != null) {
                                        first.setKey(0.0f);
                                        edit.Add(first);
                                    }
                                    if (last != null) {
                                        last.setKey(1.0f);
                                        edit.Add(last);
                                    }

                                    VibratoBPList target = null;
                                    if (mSelectedCurve.equals(CurveType.VibratoRate)) {
                                        target = ve.ID.VibratoHandle.getRateBP();
                                    } else {
                                        target = ve.ID.VibratoHandle.getDepthBP();
                                    }
                                    if (target.getCount() > 0) {
                                        for (int i = 0; i < target.getCount(); i++) {
                                            if (target.getElement(i).X < add_min || add_max < target.getElement(i).X) {
                                                edit.Add(new ValuePair<float, int>(target.getElement(i).X,
                                                                                         target.getElement(i).Y));
                                            }
                                        }
                                    }
                                    edit.Sort();
                                    VsqID id = (VsqID)ve.ID.clone();
                                    float[] bpx = new float[edit.Count];
                                    int[] bpy = new int[edit.Count];
                                    for (int i = 0; i < edit.Count; i++) {
                                        bpx[i] = edit[i].getKey();
                                        bpy[i] = edit[i].getValue();
                                    }
                                    if (mSelectedCurve.equals(CurveType.VibratoDepth)) {
                                        id.VibratoHandle.setDepthBP(new VibratoBPList(bpx, bpy));
                                    } else {
                                        id.VibratoHandle.setRateBP(new VibratoBPList(bpx, bpy));
                                    }
                                    internal_ids.Add(ve.InternalID);
                                    items.Add(id);
                                }
                                if (internal_ids.Count > 0) {
                                    CadenciiCommand run = new CadenciiCommand(
                                        VsqCommand.generateCommandEventChangeIDContaintsRange(selected,
                                                                                               PortUtil.convertIntArray(internal_ids.ToArray()),
                                                                                               items.ToArray()));
                                    executeCommand(run, true);
                                }
                                #endregion
                            } else if (mSelectedCurve.equals(CurveType.Env)) {
                                #region Env

                                #endregion
                            } else {
                                #region Other Curves
                                int track = selected;
                                int step_clock = AppManager.editorConfig.getControlCurveResolutionValue();
                                int step_px = (int)(step_clock * AppManager.mMainWindowController.getScaleX());
                                if (step_px <= 0) {
                                    step_px = 1;
                                }
                                int start = mMouseTracer.firstKey();
                                int end = mMouseTracer.lastKey();
                                int clock_start = AppManager.clockFromXCoord(start - stdx);
                                int clock_end = AppManager.clockFromXCoord(end - stdx);
                                int last = start;

#if DEBUG
                                sout.println("TrackSelector#TrackSelector_MouseUp; start, end=" + start + ", " + end);
#endif
                                VsqBPList list = vsq.Track[track].getCurve(mSelectedCurve.getName());
                                long maxid = list.getMaxID();

                                // 削除するものを列挙
                                List<long> delete = new List<long>();
                                int c = list.size();
                                for (int i = 0; i < c; i++) {
                                    int clock = list.getKeyClock(i);
                                    if (clock_start <= clock && clock <= clock_end) {
                                        delete.Add(list.getElementB(i).id);
                                    } else if (clock_end < clock) {
                                        break;
                                    }
                                }

                                SortedDictionary<int, VsqBPPair> add = new SortedDictionary<int, VsqBPPair>();
                                int lvalue = int.MinValue;
                                int lclock = -2 * step_clock;
                                int index = 0;
                                foreach (var p in mMouseTracer.iterator()) {
                                    if (p.x < start) {
                                        continue;
                                    } else if (end < p.x) {
                                        break;
                                    }
                                    int clock = AppManager.clockFromXCoord(p.x - stdx);
                                    if (clock - lclock < step_clock) {
                                        continue;
                                    }
                                    int value = valueFromYCoord(p.y);
                                    if (value < min) {
                                        value = min;
                                    } else if (max < value) {
                                        value = max;
                                    }
                                    if (value != lvalue) {
                                        index++;
                                        add[clock] = new VsqBPPair(value, maxid + index);
                                        lvalue = value;
                                        lclock = clock;
                                    }
                                }

                                // clock_endでの値
                                int valueAtEnd = list.getValue(clock_end);
                                if (add.ContainsKey(clock_end)) {
                                    VsqBPPair v = add[clock_end];
                                    v.value = valueAtEnd;
                                    add[clock_end] = v;
                                } else {
                                    index++;
                                    add[clock_end] = new VsqBPPair(valueAtEnd, maxid + index);
                                }

                                CadenciiCommand pen_run = new CadenciiCommand(
                                    VsqCommand.generateCommandTrackCurveEdit2(track, mSelectedCurve.getName(), delete, add));
                                executeCommand(pen_run, true);
                                #endregion
                            }
                        }
                        mMouseTracer.clear();
                        #endregion
                    }
                }
                mMouseDowned = false;
            } else if (mMouseDownMode == MouseDownMode.SINGER_LIST) {
                if (mMouseMoved) {
                    int count = AppManager.itemSelection.getEventCount();
                    if (count > 0) {
                        int[] ids = new int[count];
                        int[] clocks = new int[count];
                        VsqID[] values = new VsqID[count];
                        int i = -1;
                        bool is_valid = true;
                        bool contains_first_singer = false;
                        int premeasure = vsq.getPreMeasureClocks();
                        foreach (var item in AppManager.itemSelection.getEventIterator()) {
                            i++;
                            ids[i] = item.original.InternalID;
                            clocks[i] = item.editing.Clock;
                            values[i] = item.original.ID;
                            if (clocks[i] < premeasure) {
                                is_valid = false;
                                // breakしてはいけない。clock=0のを確実に検出するため
                            }
                            if (item.original.Clock == 0) {
                                contains_first_singer = true;
                                break;
                            }
                        }
                        if (contains_first_singer) {
                            System.Media.SystemSounds.Asterisk.Play();
                        } else {
                            if (!is_valid) {
                                int tmin = clocks[0];
                                for (int j = 1; j < count; j++) {
                                    tmin = Math.Min(tmin, clocks[j]);
                                }
                                int dclock = premeasure - tmin;
                                for (int j = 0; j < count; j++) {
                                    clocks[j] += dclock;
                                }
                                System.Media.SystemSounds.Asterisk.Play();
                            }
                            bool changed = false;
                            for (int j = 0; j < ids.Length; j++) {
                                foreach (var item in AppManager.itemSelection.getEventIterator()) {
                                    if (item.original.InternalID == ids[j] && item.original.Clock != clocks[j]) {
                                        changed = true;
                                        break;
                                    }
                                }
                                if (changed) {
                                    break;
                                }
                            }
                            if (changed) {
                                CadenciiCommand run = new CadenciiCommand(
                                    VsqCommand.generateCommandEventChangeClockAndIDContaintsRange(selected, ids, clocks, values));
                                executeCommand(run, true);
                            }
                        }
                    }
                }
            } else if (mMouseDownMode == MouseDownMode.VEL_EDIT) {
                if (mSelectedCurve.equals(CurveType.VEL) && is_utau_mode) {
                    int count = mVelEditSelected.Count;
                    VsqEvent[] values = new VsqEvent[count];
                    int i = 0;
                    foreach (var internal_id in mVelEditSelected.Keys) {
                        VsqEvent item = (VsqEvent)vsq_track.findEventFromID(internal_id).clone();
                        if (item.UstEvent == null) {
                            item.UstEvent = new UstEvent();
                        }
                        item.UstEvent.setIntensity(mVelEditSelected[internal_id].editing.UstEvent.getIntensity());
                        values[i] = item;
                        i++;
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventReplaceRange(selected, values));
                    executeCommand(run, true);
                } else {
                    int count = mVelEditSelected.Count;
                    int[] ids = new int[count];
                    VsqID[] values = new VsqID[count];
                    int i = -1;
                    foreach (var id in mVelEditSelected.Keys) {
                        i++;
                        ids[i] = id;
                        values[i] = (VsqID)mVelEditSelected[id].editing.ID.clone();
                    }
                    CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventChangeIDContaintsRange(selected, ids, values));
                    executeCommand(run, true);
                }
                if (mVelEditSelected.Count == 1) {
                    AppManager.itemSelection.clearEvent();
                    AppManager.itemSelection.addEvent(mVelEditLastSelectedID);
                }
            } else if (mMouseDownMode == MouseDownMode.ENVELOPE_MOVE) {
                mMouseDownMode = MouseDownMode.NONE;
                if (mMouseMoved) {
                    VsqTrack target = vsq_track;
                    VsqEvent edited = (VsqEvent)target.findEventFromID(mEnvelopeEdigintID).clone();

                    // m_envelope_originalに，編集前のが入っているので，いったん置き換え
                    int count = target.getEventCount();
                    for (int i = 0; i < count; i++) {
                        VsqEvent item = target.getEvent(i);
                        if (item.ID.type == VsqIDType.Anote && item.InternalID == mEnvelopeEdigintID) {
                            item.UstEvent.setEnvelope(mEnvelopeOriginal);
                            target.setEvent(i, item);
                            break;
                        }
                    }

                    // コマンドを発行
                    CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventReplace(selected,
                                                                                                       edited));
                    executeCommand(run, true);
                }
            } else if (mMouseDownMode == MouseDownMode.PRE_UTTERANCE_MOVE) {
                mMouseDownMode = MouseDownMode.NONE;
                if (mMouseMoved) {
                    VsqTrack target = vsq_track;
                    VsqEvent edited = (VsqEvent)target.findEventFromID(mPreUtteranceEditingID).clone();

                    // m_envelope_originalに，編集前のが入っているので，いったん置き換え
                    int count = target.getEventCount();
                    for (int i = 0; i < count; i++) {
                        VsqEvent item = target.getEvent(i);
                        if (item.ID.type == VsqIDType.Anote && item.InternalID == mPreUtteranceEditingID) {
                            target.setEvent(i, mPreOverlapOriginal);
                            break;
                        }
                    }

                    // コマンドを発行
                    CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventReplace(selected,
                                                                                                       edited));
                    executeCommand(run, true);
                }
            } else if (mMouseDownMode == MouseDownMode.OVERLAP_MOVE) {
                mMouseDownMode = MouseDownMode.NONE;
                if (mMouseMoved) {
                    VsqTrack target = vsq_track;
                    VsqEvent edited = (VsqEvent)target.findEventFromID(mOverlapEditingID).clone();

                    // m_envelope_originalに，編集前のが入っているので，いったん置き換え
                    int count = target.getEventCount();
                    for (int i = 0; i < count; i++) {
                        VsqEvent item = target.getEvent(i);
                        if (item.ID.type == VsqIDType.Anote && item.InternalID == mOverlapEditingID) {
                            target.setEvent(i, mPreOverlapOriginal);
                            break;
                        }
                    }

                    // コマンドを発行
                    CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventReplace(selected,
                                                                                                       edited));
                    executeCommand(run, true);
                }
            } else if (mMouseDownMode == MouseDownMode.POINT_MOVE) {
                if (mMouseMoved) {
                    Point pmouse = pointToClient(PortUtil.getMousePosition());
                    Point mouse = new Point(pmouse.x, pmouse.y);
                    int dx = mouse.x + AppManager.mMainWindowController.getStartToDrawX() - mMouseDownLocation.x;
                    int dy = mouse.y - mMouseDownLocation.y;

                    string curve = mSelectedCurve.getName();
                    VsqTrack work = (VsqTrack)vsq_track.clone();
                    VsqBPList list = work.getCurve(curve);
                    VsqBPList work_list = (VsqBPList)list.clone();
                    int min0 = list.getMinimum();
                    int max0 = list.getMaximum();
                    int count = list.size();
                    for (int i = 0; i < count; i++) {
                        int clock = list.getKeyClock(i);
                        VsqBPPair item = list.getElementB(i);
                        if (AppManager.itemSelection.isPointContains(item.id)) {
                            int x = AppManager.xCoordFromClocks(clock) + dx + 1;
                            int y = yCoordFromValue(item.value) + dy - 1;

                            int nclock = AppManager.clockFromXCoord(x);
                            int nvalue = valueFromYCoord(y);
                            if (nvalue < min0) {
                                nvalue = min0;
                            }
                            if (max0 < nvalue) {
                                nvalue = max0;
                            }
                            work_list.move(clock, nclock, nvalue);
                        }
                    }
                    work.setCurve(curve, work_list);
                    BezierCurves beziers = vsq.AttachedCurves.get(selected - 1);
                    CadenciiCommand run = VsqFileEx.generateCommandTrackReplace(selected, work, beziers);
                    executeCommand(run, true);
                }
                mMovingPoints.Clear();
            }
            mMouseDownMode = MouseDownMode.NONE;
            Invalidate();
        }

        public void TrackSelector_MouseHover(Object sender, EventArgs e)
        {
#if DEBUG
            AppManager.debugWriteLine("TrackSelector_MouseHover");
            AppManager.debugWriteLine("    m_mouse_downed=" + mMouseDowned);
#endif
            //if ( m_selected_curve.equals( CurveType.Accent ) || m_selected_curve.equals( CurveType.Decay ) || m_selected_curve.equals( CurveType.Env ) ) {
            if (mSelectedCurve.equals(CurveType.Env)) {
                return;
            }
            if (mMouseDowned && !mPencilMoved && AppManager.getSelectedTool() == EditTool.PENCIL &&
                 !mSelectedCurve.equals(CurveType.VEL)) {
                Point pmouse = pointToClient(PortUtil.getMousePosition());
                Point mouse = new Point(pmouse.x, pmouse.y);
                int clock = AppManager.clockFromXCoord(mouse.x);
                int value = valueFromYCoord(mouse.y);
                int min = mSelectedCurve.getMinimum();
                int max = mSelectedCurve.getMaximum();

                int selected = AppManager.getSelected();
                VsqFileEx vsq = AppManager.getVsqFile();
                VsqTrack vsq_track = vsq.Track[selected];

                if (value < min) {
                    value = min;
                } else if (max < value) {
                    value = max;
                }
                if (mSelectedCurve.equals(CurveType.VibratoRate) || mSelectedCurve.equals(CurveType.VibratoDepth)) {
                    // マウスの位置がビブラートの範囲かどうかを調べる
                    float x = -1f;
                    VsqID edited = null;
                    int event_id = -1;
                    foreach (var ve in vsq_track.getNoteEventIterator()) {
                        if (ve.ID.VibratoHandle == null) {
                            continue;
                        }
                        int cl_vib_start = ve.Clock + ve.ID.VibratoDelay;
                        int cl_vib_end = ve.Clock + ve.ID.getLength();
                        if (cl_vib_start <= clock && clock < cl_vib_end) {
                            x = (clock - cl_vib_start) / (float)(cl_vib_end - cl_vib_start);
                            edited = (VsqID)ve.ID.clone();
                            event_id = ve.InternalID;
                            break;
                        }
                    }
#if DEBUG
                    AppManager.debugWriteLine("    x=" + x);
#endif
                    if (0f <= x && x <= 1f) {
                        if (mSelectedCurve.equals(CurveType.VibratoRate)) {
                            if (x == 0f) {
                                edited.VibratoHandle.setStartRate(value);
                            } else {
                                if (edited.VibratoHandle.getRateBP().getCount() <= 0) {
                                    edited.VibratoHandle.setRateBP(new VibratoBPList(new float[] { x },
                                                                                       new int[] { value }));
                                } else {
                                    List<float> xs = new List<float>();
                                    List<int> vals = new List<int>();
                                    bool first = true;
                                    VibratoBPList ratebp = edited.VibratoHandle.getRateBP();
                                    int c = ratebp.getCount();
                                    for (int i = 0; i < c; i++) {
                                        VibratoBPPair itemi = ratebp.getElement(i);
                                        if (itemi.X < x) {
                                            xs.Add(itemi.X);
                                            vals.Add(itemi.Y);
                                        } else if (itemi.X == x) {
                                            xs.Add(x);
                                            vals.Add(value);
                                            first = false;
                                        } else {
                                            if (first) {
                                                xs.Add(x);
                                                vals.Add(value);
                                                first = false;
                                            }
                                            xs.Add(itemi.X);
                                            vals.Add(itemi.Y);
                                        }
                                    }
                                    if (first) {
                                        xs.Add(x);
                                        vals.Add(value);
                                    }
                                    edited.VibratoHandle.setRateBP(
                                        new VibratoBPList(
                                            PortUtil.convertFloatArray(xs.ToArray()),
                                            PortUtil.convertIntArray(vals.ToArray())));
                                }
                            }
                        } else {
                            if (x == 0f) {
                                edited.VibratoHandle.setStartDepth(value);
                            } else {
                                if (edited.VibratoHandle.getDepthBP().getCount() <= 0) {
                                    edited.VibratoHandle.setDepthBP(
                                        new VibratoBPList(new float[] { x }, new int[] { value }));
                                } else {
                                    List<float> xs = new List<float>();
                                    List<int> vals = new List<int>();
                                    bool first = true;
                                    VibratoBPList depthbp = edited.VibratoHandle.getDepthBP();
                                    int c = depthbp.getCount();
                                    for (int i = 0; i < c; i++) {
                                        VibratoBPPair itemi = depthbp.getElement(i);
                                        if (itemi.X < x) {
                                            xs.Add(itemi.X);
                                            vals.Add(itemi.Y);
                                        } else if (itemi.X == x) {
                                            xs.Add(x);
                                            vals.Add(value);
                                            first = false;
                                        } else {
                                            if (first) {
                                                xs.Add(x);
                                                vals.Add(value);
                                                first = false;
                                            }
                                            xs.Add(itemi.X);
                                            vals.Add(itemi.Y);
                                        }
                                    }
                                    if (first) {
                                        xs.Add(x);
                                        vals.Add(value);
                                    }
                                    edited.VibratoHandle.setDepthBP(
                                        new VibratoBPList(
                                            PortUtil.convertFloatArray(xs.ToArray()),
                                            PortUtil.convertIntArray(vals.ToArray())));
                                }
                            }
                        }
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandEventChangeIDContaints(
                                selected,
                                event_id,
                                edited));
                        executeCommand(run, true);
                    }
                } else {
                    VsqBPList list = vsq_track.getCurve(mSelectedCurve.getName());
                    if (list != null) {
                        List<long> delete = new List<long>();
                        SortedDictionary<int, VsqBPPair> add = new SortedDictionary<int, VsqBPPair>();
                        long maxid = list.getMaxID();
                        if (list.isContainsKey(clock)) {
                            int c = list.size();
                            for (int i = 0; i < c; i++) {
                                int cl = list.getKeyClock(i);
                                if (cl == clock) {
                                    delete.Add(list.getElementB(i).id);
                                    break;
                                }
                            }
                        }
                        add[clock] = new VsqBPPair(value, maxid + 1);
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandTrackCurveEdit2(selected,
                                                                       mSelectedCurve.getName(),
                                                                       delete,
                                                                       add));
                        executeCommand(run, true);
                    }
                }
            } else if (mMouseDownMode == MouseDownMode.VEL_WAIT_HOVER) {
#if DEBUG
                AppManager.debugWriteLine("    entered VelEdit");
                AppManager.debugWriteLine("    m_veledit_selected.Count=" + mVelEditSelected.Count);
                AppManager.debugWriteLine("    m_veledit_last_selectedid=" + mVelEditLastSelectedID);
                AppManager.debugWriteLine("    m_veledit_selected.ContainsKey(m_veledit_last_selectedid" + mVelEditSelected.ContainsKey(mVelEditLastSelectedID));
#endif
                mMouseDownMode = MouseDownMode.VEL_EDIT;
                Invalidate();
            }
        }

        private void MouseHoverEventGenerator()
        {
            Thread.Sleep((int)(System.Windows.Forms.SystemInformation.MouseHoverTime * 0.8));
            Invoke(new EventHandler(TrackSelector_MouseHover));
        }

        public void TrackSelector_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            if (mMouseHoverThread != null && mMouseHoverThread.IsAlive) {
                mMouseHoverThread.Abort();
            }

            VsqFileEx vsq = AppManager.getVsqFile();
            int selected = AppManager.getSelected();
            VsqTrack vsq_track = vsq.Track[selected];
            int height = getHeight();
            int width = getWidth();
            int key_width = AppManager.keyWidth;

            if (e.Button == MouseButtons.Left) {
                if (0 <= e.Y && e.Y <= height - 2 * OFFSET_TRACK_TAB) {
                    #region MouseDown occured on curve-pane
                    if (key_width <= e.X && e.X <= width) {
                        if (!mSelectedCurve.equals(CurveType.VEL) &&
                             !mSelectedCurve.equals(CurveType.Accent) &&
                             !mSelectedCurve.equals(CurveType.Decay) &&
                             !mSelectedCurve.equals(CurveType.Env)) {
                            // ベジエデータ点にヒットしているかどうかを検査
                            //int track = AppManager.getSelected();
                            int clock = AppManager.clockFromXCoord(e.X);
                            List<BezierChain> dict = vsq.AttachedCurves.get(selected - 1).get(mSelectedCurve);
                            BezierChain target_chain = null;
                            BezierPoint target_point = null;
                            bool found = false;
                            int dict_size = dict.Count;
                            for (int i = 0; i < dict_size; i++) {
                                BezierChain bc = dict[i];
                                foreach (var bp in bc.points) {
                                    Point pt = getScreenCoord(bp.getBase());
                                    Rectangle rc = new Rectangle(pt.x - DOT_WID, pt.y - DOT_WID, 2 * DOT_WID + 1, 2 * DOT_WID + 1);
                                    if (isInRect(e.X, e.Y, rc)) {
                                        found = true;
                                        target_point = (BezierPoint)bp.clone();
                                        target_chain = (BezierChain)bc.clone();
                                        break;
                                    }

                                    if (bp.getControlLeftType() != BezierControlType.None) {
                                        pt = getScreenCoord(bp.getControlLeft());
                                        rc = new Rectangle(pt.x - DOT_WID, pt.y - DOT_WID, 2 * DOT_WID + 1, 2 * DOT_WID + 1);
                                        if (isInRect(e.X, e.Y, rc)) {
                                            found = true;
                                            target_point = (BezierPoint)bp.clone();
                                            target_chain = (BezierChain)bc.clone();
                                            break;
                                        }
                                    }
                                    if (bp.getControlRightType() != BezierControlType.None) {
                                        pt = getScreenCoord(bp.getControlRight());
                                        rc = new Rectangle(pt.x - DOT_WID, pt.y - DOT_WID, 2 * DOT_WID + 1, 2 * DOT_WID + 1);
                                        if (isInRect(e.X, e.Y, rc)) {
                                            found = true;
                                            target_point = (BezierPoint)bp.clone();
                                            target_chain = (BezierChain)bc.clone();
                                            break;
                                        }
                                    }
                                }
                                if (found) {
                                    break;
                                }
                            }
                            if (found) {
                                #region ダブルクリックした位置にベジエデータ点があった場合
                                int chain_id = target_chain.id;
                                BezierChain before = (BezierChain)target_chain.clone();
                                FormBezierPointEditController fbpe = null;
                                try {
                                    fbpe = new FormBezierPointEditController(this,
                                                                    mSelectedCurve,
                                                                    chain_id,
                                                                    target_point.getID());
                                    mEditingChainID = chain_id;
                                    mEditingPointID = target_point.getID();
                                    {//TODO:
                                        sout.println("TrackSelector_MouseDoubleClick; start to show editor");
                                    }
                                    int ret = AppManager.showModalDialog(fbpe.getUi(), mMainWindow);
                                    {//TODO:
                                        sout.println("TrackSelector_MouseDoubleCLick; ret=" + ret);
                                    }
                                    mEditingChainID = -1;
                                    mEditingPointID = -1;
                                    BezierChain after = vsq.AttachedCurves.get(selected - 1).getBezierChain(mSelectedCurve, chain_id);
                                    // 編集前の状態に戻す
                                    CadenciiCommand revert =
                                        VsqFileEx.generateCommandReplaceBezierChain(
                                            selected,
                                            mSelectedCurve,
                                            chain_id,
                                            before,
                                            AppManager.editorConfig.getControlCurveResolutionValue());
                                    executeCommand(revert, false);
                                    if (ret == 1) {
                                        // ダイアログの結果がOKで、かつベジエ曲線が単調増加なら編集を適用
                                        if (BezierChain.isBezierImplicit(target_chain)) {
                                            CadenciiCommand run =
                                                VsqFileEx.generateCommandReplaceBezierChain(
                                                    selected,
                                                    mSelectedCurve,
                                                    chain_id,
                                                    after,
                                                    AppManager.editorConfig.getControlCurveResolutionValue());
                                            executeCommand(run, true);
                                        }
                                    }
                                } catch (Exception ex) {
                                } finally {
                                    if (fbpe != null) {
                                        try {
                                            fbpe.getUi().close();
                                        } catch (Exception ex2) {
                                        }
                                    }
                                }
                                #endregion
                            } else {
                                #region ダブルクリックした位置にベジエデータ点が無かった場合
                                VsqBPList list = vsq_track.getCurve(mSelectedCurve.getName());
                                bool bp_found = false;
                                long bp_id = -1;
                                int tclock = 0;
                                if (list != null) {
                                    int list_size = list.size();
                                    for (int i = 0; i < list_size; i++) {
                                        int c = list.getKeyClock(i);
                                        int bpx = AppManager.xCoordFromClocks(c);
                                        if (e.X < bpx - DOT_WID) {
                                            break;
                                        }
                                        if (bpx - DOT_WID <= e.X && e.X <= bpx + DOT_WID) {
                                            VsqBPPair bp = list.getElementB(i);
                                            int bpy = yCoordFromValue(bp.value);
                                            if (bpy - DOT_WID <= e.Y && e.Y <= bpy + DOT_WID) {
                                                bp_found = true;
                                                bp_id = bp.id;
                                                tclock = c;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (bp_found) {
                                    AppManager.itemSelection.clearPoint();
                                    AppManager.itemSelection.addPoint(mSelectedCurve, bp_id);
                                    FormCurvePointEdit dialog =
                                        new FormCurvePointEdit(mMainWindow, bp_id, mSelectedCurve);
                                    int tx = AppManager.xCoordFromClocks(tclock);
                                    Point pt = pointToScreen(new Point(tx, 0));
                                    Invalidate();
                                    dialog.Location =
                                        new System.Drawing.Point(pt.x - dialog.Width / 2, pt.y - dialog.Height);
                                    AppManager.showModalDialog(dialog, mMainWindow);
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                } else if (height - 2 * OFFSET_TRACK_TAB <= e.Y && e.Y <= height - OFFSET_TRACK_TAB) {
                    #region MouseDown occured on singer list
                    if (AppManager.getSelectedTool() != EditTool.ERASER) {
                        VsqEvent ve = null;
                        if (key_width <= e.X && e.X <= width) {
                            ve = findItemAt(e.X, e.Y);
                        }
                        RendererKind renderer = VsqFileEx.getTrackRendererKind(vsq_track);
                        if (ve == null) {
                            int x_at_left = key_width + AppManager.keyOffset;
                            Rectangle rc_left_singer_box =
                                new Rectangle(
                                    x_at_left,
                                    height - 2 * OFFSET_TRACK_TAB + 1,
                                    SINGER_ITEM_WIDTH, OFFSET_TRACK_TAB - 2);
                            if (isInRect(e.X, e.Y, rc_left_singer_box)) {
                                // マウス位置に歌手変更が無かった場合であって、かつ、
                                // マウス位置が左端の常時歌手表示部の内部だった場合
                                int clock_at_left = AppManager.clockFromXCoord(x_at_left);
                                ve = vsq_track.getSingerEventAt(clock_at_left);
                            }
                        }
                        if (ve != null) {
                            // マウス位置に何らかのアイテムがあった場合
                            if (ve.ID.type != VsqIDType.Singer) {
                                return;
                            }
                            if (!mCMenuSingerPrepared.Equals(renderer)) {
                                prepareSingerMenu(renderer);
                            }
                            cmenuSinger.SingerChangeExists = true;
                            cmenuSinger.InternalID = ve.InternalID;
                            foreach (var item in cmenuSinger.Items) {
                                var tsmi = item as TrackSelectorSingerDropdownMenuItem;
                                if (tsmi.Language == ve.ID.IconHandle.Language &&
                                     tsmi.Program == ve.ID.IconHandle.Program) {
                                    tsmi.Checked = true;
                                } else {
                                    tsmi.Checked = false;
                                }
                            }
                            cmenuSinger.Show(this, e.X, e.Y);
                        } else if (key_width <= e.X && e.X <= width) {
                            // マウス位置に何もアイテムが無かった場合
                            if (!mCMenuSingerPrepared.Equals(renderer)) {
                                prepareSingerMenu(renderer);
                            }
                            string singer = AppManager.editorConfig.DefaultSingerName;
                            int clock = AppManager.clockFromXCoord(e.X);
                            int last_clock = 0;
                            for (Iterator<VsqEvent> itr = vsq_track.getSingerEventIterator(); itr.hasNext(); ) {
                                VsqEvent ve2 = itr.next();
                                if (last_clock <= clock && clock < ve2.Clock) {
                                    singer = ((IconHandle)ve2.ID.IconHandle).IDS;
                                    break;
                                }
                                last_clock = ve2.Clock;
                            }
                            cmenuSinger.SingerChangeExists = false;
                            cmenuSinger.Clock = clock;
                            foreach (var item in cmenuSinger.Items) {
                                var tsmi = item as ToolStripMenuItem;
                                tsmi.Checked = false;
                            }
                            cmenuSinger.Show(this, e.X, e.Y);
                        }
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// 指定した歌声合成システムの歌手のリストを作成し，コンテキストメニューを準備します．
        /// </summary>
        /// <param name="renderer"></param>
        public void prepareSingerMenu(RendererKind renderer)
        {
            cmenuSinger.Items.Clear();
            List<SingerConfig> items = null;
            if (renderer == RendererKind.UTAU || renderer == RendererKind.VCNT) {
                items = AppManager.editorConfig.UtauSingers;
            } else if (renderer == RendererKind.VOCALOID1) {
                items = new List<SingerConfig>(VocaloSysUtil.getSingerConfigs(SynthesizerType.VOCALOID1));
            } else if (renderer == RendererKind.VOCALOID2) {
                items = new List<SingerConfig>(VocaloSysUtil.getSingerConfigs(SynthesizerType.VOCALOID2));
#if ENABLE_AQUESTONE
            } else if (renderer == RendererKind.AQUES_TONE) {
                items = new List<SingerConfig>();
                items.AddRange(AquesToneDriver.Singers);
            } else if (renderer == RendererKind.AQUES_TONE2) {
                items = new List<SingerConfig>();
                items.AddRange(AquesTone2Driver.Singers);
#endif
            } else {
                return;
            }
            int count = 0;
            foreach (var sc in items) {
                string tip = "";
                if (renderer == RendererKind.UTAU || renderer == RendererKind.VCNT) {
                    if (sc != null) {
                        tip = "Name: " + sc.VOICENAME +
                              "\nDirectory: " + sc.VOICEIDSTR;
                    }
                } else if (renderer == RendererKind.VOCALOID1) {
                    if (sc != null) {
                        tip = "Original: " + VocaloSysUtil.getOriginalSinger(sc.Language, sc.Program, SynthesizerType.VOCALOID1) +
                              "\nHarmonics: " + sc.Harmonics +
                              "\nNoise: " + sc.Breathiness +
                              "\nBrightness: " + sc.Brightness +
                              "\nClearness: " + sc.Clearness +
                              "\nGender Factor: " + sc.GenderFactor +
                              "\nReso1(Freq,BandWidth,Amp): " + sc.Resonance1Frequency + ", " + sc.Resonance1BandWidth + ", " + sc.Resonance1Amplitude +
                              "\nReso2(Freq,BandWidth,Amp): " + sc.Resonance2Frequency + ", " + sc.Resonance2BandWidth + ", " + sc.Resonance2Amplitude +
                              "\nReso3(Freq,BandWidth,Amp): " + sc.Resonance3Frequency + ", " + sc.Resonance3BandWidth + ", " + sc.Resonance3Amplitude +
                              "\nReso4(Freq,BandWidth,Amp): " + sc.Resonance4Frequency + ", " + sc.Resonance4BandWidth + ", " + sc.Resonance4Amplitude;
                    }
                } else if (renderer == RendererKind.VOCALOID2) {
                    if (sc != null) {
                        tip = "Original: " + VocaloSysUtil.getOriginalSinger(sc.Language, sc.Program, SynthesizerType.VOCALOID2) +
                              "\nBreathiness: " + sc.Breathiness +
                              "\nBrightness: " + sc.Brightness +
                              "\nClearness: " + sc.Clearness +
                              "\nGender Factor: " + sc.GenderFactor +
                              "\nOpening: " + sc.Opening;
                    }
                } else if (renderer == RendererKind.AQUES_TONE || renderer == RendererKind.AQUES_TONE2) {
                    if (sc != null) {
                        tip = "Name: " + sc.VOICENAME;
                    }
                }
                if (sc != null) {
                    TrackSelectorSingerDropdownMenuItem tsmi =
                        new TrackSelectorSingerDropdownMenuItem();
                    tsmi.Text = sc.VOICENAME;
                    tsmi.ToolTipText = tip;
                    tsmi.ToolTipPxWidth = 0;
                    tsmi.Language = sc.Language;
                    tsmi.Program = sc.Program;
                    tsmi.Click += new EventHandler(cmenusinger_Click);
                    tsmi.MouseHover += new EventHandler(cmenusinger_MouseHover);
                    cmenuSinger.Items.Add(tsmi);
                    count++;
                }
            }
            cmenuSinger.VisibleChanged += new EventHandler(cmenuSinger_VisibleChanged);
            mCMenuSingerTooltipWidth = new int[count];
            //m_cmenusinger_map = list.ToArray();
            for (int i = 0; i < count; i++) {
                mCMenuSingerTooltipWidth[i] = 0;
            }
            mCMenuSingerPrepared = renderer;
        }

        public void cmenuSinger_VisibleChanged(Object sender, EventArgs e)
        {
            toolTip.Hide(cmenuSinger);
        }

        public void cmenusinger_MouseEnter(Object sender, EventArgs e)
        {
            cmenusinger_MouseHover(sender, e);
        }

        public void cmenusinger_MouseHover(Object sender, EventArgs e)
        {
            try {
                TrackSelectorSingerDropdownMenuItem menu =
                    (TrackSelectorSingerDropdownMenuItem)sender;
                string tip = menu.ToolTipText;
                int language = menu.Language;
                int program = menu.Program;

                // tooltipを表示するy座標を決める
                int y = 0;
                foreach (var i in cmenuSinger.Items) {
                    var item = i as TrackSelectorSingerDropdownMenuItem;
                    if (language == item.Language &&
                         program == item.Program) {
                        break;
                    }
                    y += item.Height;
                }

                int tip_width = menu.ToolTipPxWidth;
                var ppts = cmenuSinger.PointToScreen(new System.Drawing.Point(0, 0));
                Point pts = new Point(ppts.X, ppts.Y);
                Rectangle rrc = PortUtil.getScreenBounds(this);
                Rectangle rc = new Rectangle(rrc.x, rrc.y, rrc.width, rrc.height);
                mTooltipProgram = program;
                mTooltipLanguage = language;
                if (pts.x + cmenuSinger.Width + tip_width > rc.width) {
                    toolTip.Show(tip, cmenuSinger, new System.Drawing.Point(-tip_width, y), 5000);
                } else {
                    toolTip.Show(tip, cmenuSinger, new System.Drawing.Point(cmenuSinger.Width, y), 5000);
                }
            } catch (Exception ex) {
                sout.println("TarckSelectro.tsmi_MouseHover; ex=" + ex);
                AppManager.debugWriteLine("TarckSelectro.tsmi_MouseHover; ex=" + ex);
            }
        }

        public void cmenusinger_Click(Object sender, EventArgs e)
        {
            if (!(sender is TrackSelectorSingerDropdownMenuItem)) {
                return;
            }
            TrackSelectorSingerDropdownMenuItem menu = (TrackSelectorSingerDropdownMenuItem)sender;
            int language = menu.Language;
            int program = menu.Program;
            VsqID item = Utility.getSingerID(mCMenuSingerPrepared, program, language);
            if (item != null) {
                int selected = AppManager.getSelected();
                if (cmenuSinger.SingerChangeExists) {
                    int id = cmenuSinger.InternalID;
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventChangeIDContaints(selected, id, item));
#if DEBUG
                    AppManager.debugWriteLine("TrackSelector#tsmi_Click; item.IconHandle.Program" + item.IconHandle.Program);
#endif
                    executeCommand(run, true);
                } else {
                    int clock = cmenuSinger.Clock;
                    VsqEvent ve = new VsqEvent(clock, item);
                    CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventAdd(selected, ve));
                    executeCommand(run, true);
                }
            }
        }

        private void toolTip_Draw(Object sender, System.Windows.Forms.DrawToolTipEventArgs e)
        {
            if (!(sender is System.Windows.Forms.ToolTip)) {
                return;
            }

            System.Drawing.Rectangle rrc = e.Bounds;
            Rectangle rc = new Rectangle(rrc.X, rrc.Y, rrc.Width, rrc.Height);
#if DEBUG
            sout.println("TrackSelector#toolTip_Draw; sender.GetType()=" + sender.GetType());
#endif

            foreach (var tsi in cmenuSinger.Items) {
                if (!(tsi is TrackSelectorSingerDropdownMenuItem)) {
                    continue;
                }
                TrackSelectorSingerDropdownMenuItem menu =
                    (TrackSelectorSingerDropdownMenuItem)tsi;
                if (menu.Language == mTooltipLanguage &&
                     menu.Program == mTooltipProgram) {
                    menu.ToolTipPxWidth = rc.width;
                    break;
                }
            }
            e.DrawBackground();
            e.DrawBorder();
            e.DrawText(System.Windows.Forms.TextFormatFlags.VerticalCenter | System.Windows.Forms.TextFormatFlags.Left | System.Windows.Forms.TextFormatFlags.NoFullWidthCharacterBreak);
        }

        public void TrackSelector_KeyDown(Object sender, KeyEventArgs e)
        {
            if ((e.KeyCode & System.Windows.Forms.Keys.Space) == System.Windows.Forms.Keys.Space) {
                mSpaceKeyDowned = true;
            }
        }

        public void TrackSelector_KeyUp(Object sender, KeyEventArgs e)
        {
            if ((e.KeyCode & System.Windows.Forms.Keys.Space) == System.Windows.Forms.Keys.Space) {
                mSpaceKeyDowned = false;
            }
        }

        public void cmenuCurveCommon_Click(Object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem) {
                ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
                CurveType curve = getCurveTypeFromMenu(tsmi);
                if (!curve.Equals(CurveType.Empty)) {
                    changeCurve(curve);
                }
            }
        }

        private void registerEventHandlers()
        {
            this.toolTip.Draw += new System.Windows.Forms.DrawToolTipEventHandler(this.toolTip_Draw);
            this.cmenuCurveVelocity.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveAccent.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveDecay.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveDynamics.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveVibratoRate.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveVibratoDepth.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveReso1Freq.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveReso1BW.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveReso1Amp.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveReso2Freq.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveReso2BW.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveReso2Amp.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveReso3Freq.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveReso3BW.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveReso3Amp.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveReso4Freq.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveReso4BW.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveReso4Amp.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveHarmonics.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveBreathiness.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveBrightness.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveClearness.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveOpening.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveGenderFactor.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurvePortamentoTiming.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurvePitchBend.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurvePitchBendSensitivity.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveEffect2Depth.Click += new EventHandler(cmenuCurveCommon_Click);
            this.cmenuCurveEnvelope.Click += new EventHandler(cmenuCurveCommon_Click);
            this.Load += new EventHandler(this.TrackSelector_Load);

            this.MouseMove += new MouseEventHandler(TrackSelector_MouseMove);
            this.MouseDoubleClick += new MouseEventHandler(TrackSelector_MouseDoubleClick);
            this.KeyUp += new KeyEventHandler(TrackSelector_KeyUp);
            this.MouseClick += new MouseEventHandler(TrackSelector_MouseClick);
            this.MouseDown += new MouseEventHandler(TrackSelector_MouseDown);
            this.MouseUp += new MouseEventHandler(TrackSelector_MouseUp);
            this.KeyDown += new KeyEventHandler(TrackSelector_KeyDown);
        }

        private void setResources()
        {
        }

        #region UI Impl for C#
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cmenuSinger = new TrackSelectorSingerPopupMenu(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmenuCurve = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmenuCurveVelocity = new ToolStripMenuItem();
            this.cmenuCurveAccent = new ToolStripMenuItem();
            this.cmenuCurveDecay = new ToolStripMenuItem();
            this.cmenuCurveSeparator1 = new ToolStripSeparator();
            this.cmenuCurveDynamics = new ToolStripMenuItem();
            this.cmenuCurveVibratoRate = new ToolStripMenuItem();
            this.cmenuCurveVibratoDepth = new ToolStripMenuItem();
            this.cmenuCurveSeparator2 = new ToolStripSeparator();
            this.cmenuCurveReso1 = new ToolStripMenuItem();
            this.cmenuCurveReso1Freq = new ToolStripMenuItem();
            this.cmenuCurveReso1BW = new ToolStripMenuItem();
            this.cmenuCurveReso1Amp = new ToolStripMenuItem();
            this.cmenuCurveReso2 = new ToolStripMenuItem();
            this.cmenuCurveReso2Freq = new ToolStripMenuItem();
            this.cmenuCurveReso2BW = new ToolStripMenuItem();
            this.cmenuCurveReso2Amp = new ToolStripMenuItem();
            this.cmenuCurveReso3 = new ToolStripMenuItem();
            this.cmenuCurveReso3Freq = new ToolStripMenuItem();
            this.cmenuCurveReso3BW = new ToolStripMenuItem();
            this.cmenuCurveReso3Amp = new ToolStripMenuItem();
            this.cmenuCurveReso4 = new ToolStripMenuItem();
            this.cmenuCurveReso4Freq = new ToolStripMenuItem();
            this.cmenuCurveReso4BW = new ToolStripMenuItem();
            this.cmenuCurveReso4Amp = new ToolStripMenuItem();
            this.cmenuCurveSeparator3 = new ToolStripSeparator();
            this.cmenuCurveHarmonics = new ToolStripMenuItem();
            this.cmenuCurveBreathiness = new ToolStripMenuItem();
            this.cmenuCurveBrightness = new ToolStripMenuItem();
            this.cmenuCurveClearness = new ToolStripMenuItem();
            this.cmenuCurveOpening = new ToolStripMenuItem();
            this.cmenuCurveGenderFactor = new ToolStripMenuItem();
            this.cmenuCurveSeparator4 = new ToolStripSeparator();
            this.cmenuCurvePortamentoTiming = new ToolStripMenuItem();
            this.cmenuCurvePitchBend = new ToolStripMenuItem();
            this.cmenuCurvePitchBendSensitivity = new ToolStripMenuItem();
            this.cmenuCurveSeparator5 = new ToolStripSeparator();
            this.cmenuCurveEffect2Depth = new ToolStripMenuItem();
            this.cmenuCurveEnvelope = new ToolStripMenuItem();
            this.cmenuCurve.SuspendLayout();
            this.SuspendLayout();
            //
            // cmenuSinger
            //
            this.cmenuSinger.Name = "cmenuSinger";
            this.cmenuSinger.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.cmenuSinger.ShowCheckMargin = true;
            this.cmenuSinger.ShowImageMargin = false;
            this.cmenuSinger.Size = new System.Drawing.Size(153, 26);
            //
            // toolTip
            //
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.OwnerDraw = true;
            this.toolTip.ReshowDelay = 0;
            //
            // cmenuCurve
            //
            this.cmenuCurve.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveVelocity,
            this.cmenuCurveAccent,
            this.cmenuCurveDecay,
            this.cmenuCurveSeparator1,
            this.cmenuCurveDynamics,
            this.cmenuCurveVibratoRate,
            this.cmenuCurveVibratoDepth,
            this.cmenuCurveSeparator2,
            this.cmenuCurveReso1,
            this.cmenuCurveReso2,
            this.cmenuCurveReso3,
            this.cmenuCurveReso4,
            this.cmenuCurveSeparator3,
            this.cmenuCurveHarmonics,
            this.cmenuCurveBreathiness,
            this.cmenuCurveBrightness,
            this.cmenuCurveClearness,
            this.cmenuCurveOpening,
            this.cmenuCurveGenderFactor,
            this.cmenuCurveSeparator4,
            this.cmenuCurvePortamentoTiming,
            this.cmenuCurvePitchBend,
            this.cmenuCurvePitchBendSensitivity,
            this.cmenuCurveSeparator5,
            this.cmenuCurveEffect2Depth,
            this.cmenuCurveEnvelope});
            this.cmenuCurve.Name = "cmenuCurve";
            this.cmenuCurve.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.cmenuCurve.ShowImageMargin = false;
            this.cmenuCurve.Size = new System.Drawing.Size(160, 496);
            //
            // cmenuCurveVelocity
            //
            this.cmenuCurveVelocity.Name = "cmenuCurveVelocity";
            this.cmenuCurveVelocity.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveVelocity.Text = "Velocity(&V)";
            //
            // cmenuCurveAccent
            //
            this.cmenuCurveAccent.Name = "cmenuCurveAccent";
            this.cmenuCurveAccent.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveAccent.Text = "Accent";
            //
            // cmenuCurveDecay
            //
            this.cmenuCurveDecay.Name = "cmenuCurveDecay";
            this.cmenuCurveDecay.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveDecay.Text = "Decay";
            //
            // cmenuCurveSeparator1
            //
            this.cmenuCurveSeparator1.Name = "cmenuCurveSeparator1";
            this.cmenuCurveSeparator1.Size = new System.Drawing.Size(156, 6);
            //
            // cmenuCurveDynamics
            //
            this.cmenuCurveDynamics.Name = "cmenuCurveDynamics";
            this.cmenuCurveDynamics.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveDynamics.Text = "Dynamics";
            //
            // cmenuCurveVibratoRate
            //
            this.cmenuCurveVibratoRate.Name = "cmenuCurveVibratoRate";
            this.cmenuCurveVibratoRate.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveVibratoRate.Text = "Vibrato Rate";
            //
            // cmenuCurveVibratoDepth
            //
            this.cmenuCurveVibratoDepth.Name = "cmenuCurveVibratoDepth";
            this.cmenuCurveVibratoDepth.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveVibratoDepth.Text = "Vibrato Depth";
            //
            // cmenuCurveSeparator2
            //
            this.cmenuCurveSeparator2.Name = "cmenuCurveSeparator2";
            this.cmenuCurveSeparator2.Size = new System.Drawing.Size(156, 6);
            //
            // cmenuCurveReso1
            //
            this.cmenuCurveReso1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso1Freq,
            this.cmenuCurveReso1BW,
            this.cmenuCurveReso1Amp});
            this.cmenuCurveReso1.Name = "cmenuCurveReso1";
            this.cmenuCurveReso1.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveReso1.Text = "Resonance 1";
            //
            // cmenuCurveReso1Freq
            //
            this.cmenuCurveReso1Freq.Name = "cmenuCurveReso1Freq";
            this.cmenuCurveReso1Freq.Size = new System.Drawing.Size(128, 22);
            this.cmenuCurveReso1Freq.Text = "Frequency";
            //
            // cmenuCurveReso1BW
            //
            this.cmenuCurveReso1BW.Name = "cmenuCurveReso1BW";
            this.cmenuCurveReso1BW.Size = new System.Drawing.Size(128, 22);
            this.cmenuCurveReso1BW.Text = "Band Width";
            //
            // cmenuCurveReso1Amp
            //
            this.cmenuCurveReso1Amp.Name = "cmenuCurveReso1Amp";
            this.cmenuCurveReso1Amp.Size = new System.Drawing.Size(128, 22);
            this.cmenuCurveReso1Amp.Text = "Amplitude";
            //
            // cmenuCurveReso2
            //
            this.cmenuCurveReso2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso2Freq,
            this.cmenuCurveReso2BW,
            this.cmenuCurveReso2Amp});
            this.cmenuCurveReso2.Name = "cmenuCurveReso2";
            this.cmenuCurveReso2.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveReso2.Text = "Resonance 2";
            //
            // cmenuCurveReso2Freq
            //
            this.cmenuCurveReso2Freq.Name = "cmenuCurveReso2Freq";
            this.cmenuCurveReso2Freq.Size = new System.Drawing.Size(128, 22);
            this.cmenuCurveReso2Freq.Text = "Frequency";
            //
            // cmenuCurveReso2BW
            //
            this.cmenuCurveReso2BW.Name = "cmenuCurveReso2BW";
            this.cmenuCurveReso2BW.Size = new System.Drawing.Size(128, 22);
            this.cmenuCurveReso2BW.Text = "Band Width";
            //
            // cmenuCurveReso2Amp
            //
            this.cmenuCurveReso2Amp.Name = "cmenuCurveReso2Amp";
            this.cmenuCurveReso2Amp.Size = new System.Drawing.Size(128, 22);
            this.cmenuCurveReso2Amp.Text = "Amplitude";
            //
            // cmenuCurveReso3
            //
            this.cmenuCurveReso3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso3Freq,
            this.cmenuCurveReso3BW,
            this.cmenuCurveReso3Amp});
            this.cmenuCurveReso3.Name = "cmenuCurveReso3";
            this.cmenuCurveReso3.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveReso3.Text = "Resonance 3";
            //
            // cmenuCurveReso3Freq
            //
            this.cmenuCurveReso3Freq.Name = "cmenuCurveReso3Freq";
            this.cmenuCurveReso3Freq.Size = new System.Drawing.Size(128, 22);
            this.cmenuCurveReso3Freq.Text = "Frequency";
            //
            // cmenuCurveReso3BW
            //
            this.cmenuCurveReso3BW.Name = "cmenuCurveReso3BW";
            this.cmenuCurveReso3BW.Size = new System.Drawing.Size(128, 22);
            this.cmenuCurveReso3BW.Text = "Band Width";
            //
            // cmenuCurveReso3Amp
            //
            this.cmenuCurveReso3Amp.Name = "cmenuCurveReso3Amp";
            this.cmenuCurveReso3Amp.Size = new System.Drawing.Size(128, 22);
            this.cmenuCurveReso3Amp.Text = "Amplitude";
            //
            // cmenuCurveReso4
            //
            this.cmenuCurveReso4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso4Freq,
            this.cmenuCurveReso4BW,
            this.cmenuCurveReso4Amp});
            this.cmenuCurveReso4.Name = "cmenuCurveReso4";
            this.cmenuCurveReso4.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveReso4.Text = "Resonance 4";
            //
            // cmenuCurveReso4Freq
            //
            this.cmenuCurveReso4Freq.Name = "cmenuCurveReso4Freq";
            this.cmenuCurveReso4Freq.Size = new System.Drawing.Size(128, 22);
            this.cmenuCurveReso4Freq.Text = "Frequency";
            //
            // cmenuCurveReso4BW
            //
            this.cmenuCurveReso4BW.Name = "cmenuCurveReso4BW";
            this.cmenuCurveReso4BW.Size = new System.Drawing.Size(128, 22);
            this.cmenuCurveReso4BW.Text = "Band Width";
            //
            // cmenuCurveReso4Amp
            //
            this.cmenuCurveReso4Amp.Name = "cmenuCurveReso4Amp";
            this.cmenuCurveReso4Amp.Size = new System.Drawing.Size(128, 22);
            this.cmenuCurveReso4Amp.Text = "Amplitude";
            //
            // cmenuCurveSeparator3
            //
            this.cmenuCurveSeparator3.Name = "cmenuCurveSeparator3";
            this.cmenuCurveSeparator3.Size = new System.Drawing.Size(156, 6);
            //
            // cmenuCurveHarmonics
            //
            this.cmenuCurveHarmonics.Name = "cmenuCurveHarmonics";
            this.cmenuCurveHarmonics.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveHarmonics.Text = "Harmonics";
            //
            // cmenuCurveBreathiness
            //
            this.cmenuCurveBreathiness.Name = "cmenuCurveBreathiness";
            this.cmenuCurveBreathiness.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveBreathiness.Text = "Noise";
            //
            // cmenuCurveBrightness
            //
            this.cmenuCurveBrightness.Name = "cmenuCurveBrightness";
            this.cmenuCurveBrightness.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveBrightness.Text = "Brightness";
            //
            // cmenuCurveClearness
            //
            this.cmenuCurveClearness.Name = "cmenuCurveClearness";
            this.cmenuCurveClearness.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveClearness.Text = "Clearness";
            //
            // cmenuCurveOpening
            //
            this.cmenuCurveOpening.Name = "cmenuCurveOpening";
            this.cmenuCurveOpening.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveOpening.Text = "Opening";
            //
            // cmenuCurveGenderFactor
            //
            this.cmenuCurveGenderFactor.Name = "cmenuCurveGenderFactor";
            this.cmenuCurveGenderFactor.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveGenderFactor.Text = "Gender Factor";
            //
            // cmenuCurveSeparator4
            //
            this.cmenuCurveSeparator4.Name = "cmenuCurveSeparator4";
            this.cmenuCurveSeparator4.Size = new System.Drawing.Size(156, 6);
            //
            // cmenuCurvePortamentoTiming
            //
            this.cmenuCurvePortamentoTiming.Name = "cmenuCurvePortamentoTiming";
            this.cmenuCurvePortamentoTiming.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurvePortamentoTiming.Text = "Portamento Timing";
            //
            // cmenuCurvePitchBend
            //
            this.cmenuCurvePitchBend.Name = "cmenuCurvePitchBend";
            this.cmenuCurvePitchBend.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurvePitchBend.Text = "Pitch Bend";
            //
            // cmenuCurvePitchBendSensitivity
            //
            this.cmenuCurvePitchBendSensitivity.Name = "cmenuCurvePitchBendSensitivity";
            this.cmenuCurvePitchBendSensitivity.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurvePitchBendSensitivity.Text = "Pitch Bend Sensitivity";
            //
            // cmenuCurveSeparator5
            //
            this.cmenuCurveSeparator5.Name = "cmenuCurveSeparator5";
            this.cmenuCurveSeparator5.Size = new System.Drawing.Size(156, 6);
            //
            // cmenuCurveEffect2Depth
            //
            this.cmenuCurveEffect2Depth.Name = "cmenuCurveEffect2Depth";
            this.cmenuCurveEffect2Depth.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveEffect2Depth.Text = "Effect2 Depth";
            //
            // cmenuCurveEnvelope
            //
            this.cmenuCurveEnvelope.Name = "cmenuCurveEnvelope";
            this.cmenuCurveEnvelope.Size = new System.Drawing.Size(159, 22);
            this.cmenuCurveEnvelope.Text = "Envelope";
            //
            // TrackSelector
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.DoubleBuffered = true;
            this.Name = "TrackSelector";
            this.Size = new System.Drawing.Size(430, 228);
            this.cmenuCurve.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TrackSelectorSingerPopupMenu cmenuSinger;
        private System.Windows.Forms.ToolTip toolTip;
        private ContextMenuStrip cmenuCurve;
        private ToolStripMenuItem cmenuCurveVelocity;
        private ToolStripSeparator cmenuCurveSeparator2;
        private ToolStripMenuItem cmenuCurveReso1;
        private ToolStripMenuItem cmenuCurveReso1Freq;
        private ToolStripMenuItem cmenuCurveReso1BW;
        private ToolStripMenuItem cmenuCurveReso1Amp;
        private ToolStripMenuItem cmenuCurveReso2;
        private ToolStripMenuItem cmenuCurveReso2Freq;
        private ToolStripMenuItem cmenuCurveReso2BW;
        private ToolStripMenuItem cmenuCurveReso2Amp;
        private ToolStripMenuItem cmenuCurveReso3;
        private ToolStripMenuItem cmenuCurveReso3Freq;
        private ToolStripMenuItem cmenuCurveReso3BW;
        private ToolStripMenuItem cmenuCurveReso3Amp;
        private ToolStripMenuItem cmenuCurveReso4;
        private ToolStripMenuItem cmenuCurveReso4Freq;
        private ToolStripMenuItem cmenuCurveReso4BW;
        private ToolStripMenuItem cmenuCurveReso4Amp;
        private ToolStripSeparator cmenuCurveSeparator3;
        private ToolStripMenuItem cmenuCurveHarmonics;
        private ToolStripMenuItem cmenuCurveDynamics;
        private ToolStripSeparator cmenuCurveSeparator1;
        private ToolStripMenuItem cmenuCurveBreathiness;
        private ToolStripMenuItem cmenuCurveBrightness;
        private ToolStripMenuItem cmenuCurveClearness;
        private ToolStripMenuItem cmenuCurveGenderFactor;
        private ToolStripSeparator cmenuCurveSeparator4;
        private ToolStripMenuItem cmenuCurvePortamentoTiming;
        private ToolStripSeparator cmenuCurveSeparator5;
        private ToolStripMenuItem cmenuCurveEffect2Depth;
        private ToolStripMenuItem cmenuCurveOpening;
        private ToolStripMenuItem cmenuCurveAccent;
        private ToolStripMenuItem cmenuCurveDecay;
        private ToolStripMenuItem cmenuCurveVibratoRate;
        private ToolStripMenuItem cmenuCurveVibratoDepth;
        private ToolStripMenuItem cmenuCurvePitchBend;
        private ToolStripMenuItem cmenuCurvePitchBendSensitivity;
        private ToolStripMenuItem cmenuCurveEnvelope;

        #endregion
    }

}
