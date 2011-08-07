/*
 * TrackSelector.cs
 * Copyright © 2008-2011 kbinani
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
#if JAVA
package org.kbinani.cadencii;

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/cadencii/TrackSelector.java

import java.awt.*;
import java.awt.event.*;
import java.util.*;
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
#define COMPONENT_ENABLE_LOCATION
//#define MONITOR_FPS
//#define OLD_IMPL_MOUSE_TRACER
using System;
using System.Threading;
using org.kbinani;
using org.kbinani.apputil;
using org.kbinani.java.awt;
using org.kbinani.java.awt.event_;
using org.kbinani.java.util;
using org.kbinani.vsq;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii
{
    using BEventArgs = System.EventArgs;
    using BKeyEventArgs = System.Windows.Forms.KeyEventArgs;
    using BMouseButtons = System.Windows.Forms.MouseButtons;
    using BMouseEventArgs = System.Windows.Forms.MouseEventArgs;

    using BEventHandler = System.EventHandler;
    using BKeyEventHandler = System.Windows.Forms.KeyEventHandler;
    using BMouseEventHandler = System.Windows.Forms.MouseEventHandler;

    using boolean = System.Boolean;
    using Float = System.Single;
    using Graphics = org.kbinani.java.awt.Graphics2D;
    using Integer = System.Int32;
    using Long = System.Int64;
#endif

    /// <summary>
    /// コントロールカーブ，トラックの一覧，歌手変更イベントなどを表示するコンポーネント．
    /// </summary>
#if JAVA
    public class TrackSelector extends BPanel
#else
    public class TrackSelector : BPanel
#endif
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
        private static readonly Color COLOR_BEZIER_DOT_NORMAL = new Color( 237, 107, 158 );
        /// <summary>
        /// ベジエ曲線の制御点の枠色
        /// </summary>
        private static readonly Color COLOR_BEZIER_DOT_NORMAL_DARK = new Color( 153, 19, 70 );
        /// <summary>
        /// ベジエ曲線のデータ点の色
        /// </summary>
        private static readonly Color COLOR_BEZIER_DOT_BASE = new Color( 125, 198, 34 );
        /// <summary>
        /// ベジエ曲線のデータ点の枠色
        /// </summary>
        private static readonly Color COLOR_BEZIER_DOT_BASE_DARK = new Color( 62, 99, 17 );

        /// <summary>
        /// データ点のハイライト色
        /// </summary>
        private static readonly Color COLOR_DOT_HILIGHT = PortUtil.Coral;
        private static readonly Color COLOR_A244R255G023B012 = new Color( 255, 23, 12, 244 );
        private static readonly Color COLOR_A144R255G255B255 = new Color( 255, 255, 255, 144 );
        private static readonly Color COLOR_A072R255G255B255 = new Color( 255, 255, 255, 72 );
        private static readonly Color COLOR_A127R008G166B172 = new Color( 8, 166, 172, 127 );
        private static readonly Color COLOR_A098R000G000B000 = new Color( 0, 0, 0, 98 );
        /// <summary>
        /// 歌手変更を表すボックスの枠線のハイライト色
        /// </summary>
        private static readonly Color COLOR_SINGERBOX_BORDER_HILIGHT = new Color( 246, 251, 10 );
        /// <summary>
        /// 歌手変更を表すボックスの枠線の色
        /// </summary>
        private static readonly Color COLOR_SINGERBOX_BORDER = new Color( 182, 182, 182 );
        /// <summary>
        /// ビブラートコントロールカーブの、ビブラート以外の部分を塗りつぶす時の色
        /// </summary>
        private static readonly Color COLOR_VIBRATO_SHADOW = new Color( 0, 0, 0, 127 );
        /// <summary>
        /// マウスの軌跡を描くときの塗りつぶし色
        /// </summary>
        private static readonly Color COLOR_MOUSE_TRACER = new Color( 8, 166, 172, 127 );
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
        private boolean mCurveVisible = true;
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
        private boolean mMouseDowned = false;
        /// <summary>
        /// マウスのトレーサ。コントロールカーブ用の仮想スクリーン座標で表す。
        /// </summary>
        private MouseTracer mMouseTracer = new MouseTracer();
        private boolean mPencilMoved = false;
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
        private boolean mMouseMoved = false;
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
        private TreeMap<Integer, SelectedEventEntry> mVelEditSelected = new TreeMap<Integer, SelectedEventEntry>();
        /// <summary>
        /// 現在編集操作が行われているBezierChainの、編集直前のオリジナル
        /// </summary>
        private BezierChain mEditingBezierOriginal = null;
        /// <summary>
        /// CTRLキー。MacOSXの場合はMenu
        /// </summary>
        private int mModifierKey = InputEvent.CTRL_MASK;
        /// <summary>
        /// スペースキーが押されているかどうか。
        /// MouseDown時に範囲選択モードをスキップする必要があるので、FormMainでの処理に加えてこのクラス内部でも処理する必要がある
        /// </summary>
        private boolean mSpaceKeyDowned = false;
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
        private int mModifierOnMouseDown = 0;
        /// <summary>
        /// 移動しているデータ点のリスト
        /// </summary>
        private Vector<BPPair> mMovingPoints = new Vector<BPPair>();
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
        private TreeMap<CurveType, BMenuItem> mMenuMap = new TreeMap<CurveType, BMenuItem>();
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
        private Vector<CurveType> mViewingCurves = new Vector<CurveType>();

        /// <summary>
        /// 最前面に表示するカーブの種類が変更されたとき発生するイベント．
        /// </summary>
#if JAVA
        public BEvent<SelectedCurveChangedEventHandler> selectedCurveChangedEvent = new BEvent<SelectedCurveChangedEventHandler>();
#else
        public event SelectedCurveChangedEventHandler SelectedCurveChanged;
#endif
        /// <summary>
        /// 表示するトラック番号が変更されたとき発生するイベント．
        /// </summary>
#if JAVA
        public BEvent<SelectedTrackChangedEventHandler> selectedTrackChangedEvent = new BEvent<SelectedTrackChangedEventHandler>();
#else
        public event SelectedTrackChangedEventHandler SelectedTrackChanged;
#endif
        /// <summary>
        /// VSQの編集コマンドが発行されたとき発生するイベント．
        /// </summary>
#if JAVA
        public BEvent<BEventHandler> commandExecutedEvent = new BEvent<BEventHandler>();
#else
        public event BEventHandler CommandExecuted;
#endif
        /// <summary>
        /// トラックの歌声合成が要求されたとき発生するイベント．
        /// </summary>
#if JAVA
        public BEvent<RenderRequiredEventHandler> renderRequiredEvent = new BEvent<RenderRequiredEventHandler>();
#else
        public event RenderRequiredEventHandler RenderRequired;
#endif
        /// <summary>
        /// このコントロールの推奨最少表示高さが変わったとき発生するイベント．
        /// </summary>
#if JAVA
        public BEvent<BEventHandler> preferredMinHeightChangedEvent = new BEvent<BEventHandler>();
#else
        public event BEventHandler PreferredMinHeightChanged;
#endif

#if JAVA
        /**
         * デザイナで使用するコンストラクタ．
         * 実際にはTrackSelector( FormMain )を使用すること
         */
        public TrackSelector()
        {
            this( null );
        }
#endif

        /// <summary>
        /// コンストラクタ．
        /// </summary>
        public TrackSelector( FormMain main_window )
        {
#if JAVA
            super();
            initialize();
            getCmenuCurve();
            getCmenuSinger();
#else
            this.SetStyle( System.Windows.Forms.ControlStyles.DoubleBuffer, true );
            this.SetStyle( System.Windows.Forms.ControlStyles.UserPaint, true );
            InitializeComponent();
#endif
            mMainWindow = main_window;
            registerEventHandlers();
            setResources();
            mModifierKey =
#if JAVA_MAC
                InputEvent.META_MASK;
#else
                InputEvent.CTRL_MASK;
#endif
            mMenuMap.put( CurveType.VEL, cmenuCurveVelocity );
            mMenuMap.put( CurveType.Accent, cmenuCurveAccent );
            mMenuMap.put( CurveType.Decay, cmenuCurveDecay );

            mMenuMap.put( CurveType.DYN, cmenuCurveDynamics );
            mMenuMap.put( CurveType.VibratoRate, cmenuCurveVibratoRate );
            mMenuMap.put( CurveType.VibratoDepth, cmenuCurveVibratoDepth );

            mMenuMap.put( CurveType.reso1amp, cmenuCurveReso1Amp );
            mMenuMap.put( CurveType.reso1bw, cmenuCurveReso1BW );
            mMenuMap.put( CurveType.reso1freq, cmenuCurveReso1Freq );
            mMenuMap.put( CurveType.reso2amp, cmenuCurveReso2Amp );
            mMenuMap.put( CurveType.reso2bw, cmenuCurveReso2BW );
            mMenuMap.put( CurveType.reso2freq, cmenuCurveReso2Freq );
            mMenuMap.put( CurveType.reso3amp, cmenuCurveReso3Amp );
            mMenuMap.put( CurveType.reso3bw, cmenuCurveReso3BW );
            mMenuMap.put( CurveType.reso3freq, cmenuCurveReso3Freq );
            mMenuMap.put( CurveType.reso4amp, cmenuCurveReso4Amp );
            mMenuMap.put( CurveType.reso4bw, cmenuCurveReso4BW );
            mMenuMap.put( CurveType.reso4freq, cmenuCurveReso4Freq );

            mMenuMap.put( CurveType.harmonics, cmenuCurveHarmonics );
            mMenuMap.put( CurveType.BRE, cmenuCurveBreathiness );
            mMenuMap.put( CurveType.BRI, cmenuCurveBrightness );
            mMenuMap.put( CurveType.CLE, cmenuCurveClearness );
            mMenuMap.put( CurveType.OPE, cmenuCurveOpening );
            mMenuMap.put( CurveType.GEN, cmenuCurveGenderFactor );

            mMenuMap.put( CurveType.POR, cmenuCurvePortamentoTiming );
            mMenuMap.put( CurveType.PIT, cmenuCurvePitchBend );
            mMenuMap.put( CurveType.PBS, cmenuCurvePitchBendSensitivity );

            mMenuMap.put( CurveType.fx2depth, cmenuCurveEffect2Depth );
            mMenuMap.put( CurveType.Env, cmenuCurveEnvelope );
        }

        /// <summary>
        /// 表示するコントロールのカーブの種類を、AppManager.EditorConfigの設定に応じて更新します
        /// </summary>
        public void updateVisibleCurves()
        {
            if ( AppManager.editorConfig.CurveVisibleVelocity )
            {
                addViewingCurve( CurveType.VEL );
            }
            if ( AppManager.editorConfig.CurveVisibleAccent )
            {
                addViewingCurve( CurveType.Accent );
            }
            if ( AppManager.editorConfig.CurveVisibleDecay )
            {
                addViewingCurve( CurveType.Decay );
            }
            if ( AppManager.editorConfig.CurveVisibleVibratoRate )
            {
                addViewingCurve( CurveType.VibratoRate );
            }
            if ( AppManager.editorConfig.CurveVisibleVibratoDepth )
            {
                addViewingCurve( CurveType.VibratoDepth );
            }
            if ( AppManager.editorConfig.CurveVisibleDynamics )
            {
                addViewingCurve( CurveType.DYN );
            }
            if ( AppManager.editorConfig.CurveVisibleBreathiness )
            {
                addViewingCurve( CurveType.BRE );
            }
            if ( AppManager.editorConfig.CurveVisibleBrightness )
            {
                addViewingCurve( CurveType.BRI );
            }
            if ( AppManager.editorConfig.CurveVisibleClearness )
            {
                addViewingCurve( CurveType.CLE );
            }
            if ( AppManager.editorConfig.CurveVisibleOpening )
            {
                addViewingCurve( CurveType.OPE );
            }
            if ( AppManager.editorConfig.CurveVisibleGendorfactor )
            {
                addViewingCurve( CurveType.GEN );
            }
            if ( AppManager.editorConfig.CurveVisiblePortamento )
            {
                addViewingCurve( CurveType.POR );
            }
            if ( AppManager.editorConfig.CurveVisiblePit )
            {
                addViewingCurve( CurveType.PIT );
            }
            if ( AppManager.editorConfig.CurveVisiblePbs )
            {
                addViewingCurve( CurveType.PBS );
            }
            if ( AppManager.editorConfig.CurveVisibleHarmonics )
            {
                addViewingCurve( CurveType.harmonics );
            }
            if ( AppManager.editorConfig.CurveVisibleFx2Depth )
            {
                addViewingCurve( CurveType.fx2depth );
            }
            if ( AppManager.editorConfig.CurveVisibleReso1 )
            {
                addViewingCurve( CurveType.reso1freq );
                addViewingCurve( CurveType.reso1bw );
                addViewingCurve( CurveType.reso1amp );
            }
            if ( AppManager.editorConfig.CurveVisibleReso2 )
            {
                addViewingCurve( CurveType.reso2freq );
                addViewingCurve( CurveType.reso2bw );
                addViewingCurve( CurveType.reso2amp );
            }
            if ( AppManager.editorConfig.CurveVisibleReso3 )
            {
                addViewingCurve( CurveType.reso3freq );
                addViewingCurve( CurveType.reso3bw );
                addViewingCurve( CurveType.reso3amp );
            }
            if ( AppManager.editorConfig.CurveVisibleReso4 )
            {
                addViewingCurve( CurveType.reso4freq );
                addViewingCurve( CurveType.reso4bw );
                addViewingCurve( CurveType.reso4amp );
            }
            if ( AppManager.editorConfig.CurveVisibleEnvelope )
            {
                addViewingCurve( CurveType.Env );
            }
        }

        /// <summary>
        /// コントロールトラックに表示しているカーブ種類の個数を取得します．
        /// </summary>
        /// <returns></returns>
        private int getViewingCurveCount()
        {
            return mViewingCurves.size();
        }

        /// <summary>
        /// コントロールトラックに表示している，第index番目のカーブ種類を取得します．
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private CurveType getViewingCurveElement( int index )
        {
            return mViewingCurves.get( index );
        }

        /// <summary>
        /// コントロールトラックに表示しているカーブ種類のリストに，カーブ種類を追加します．
        /// </summary>
        /// <param name="curve"></param>
        private void addViewingCurveRange( CurveType[] curve )
        {
            for ( int j = 0; j < curve.Length; j++ )
            {
                boolean found = false;
                for ( int i = 0; i < mViewingCurves.size(); i++ )
                {
                    if ( mViewingCurves.get( i ).equals( curve[j] ) )
                    {
                        found = true;
                        break;
                    }
                }
                if ( !found )
                {
                    mViewingCurves.add( curve[j] );
                }
            }
            if ( mViewingCurves.size() >= 2 )
            {
                boolean changed = true;
                while ( changed )
                {
                    changed = false;
                    for ( int i = 0; i < mViewingCurves.size() - 1; i++ )
                    {
                        if ( mViewingCurves.get( i ).getIndex() > mViewingCurves.get( i + 1 ).getIndex() )
                        {
                            CurveType b = (CurveType)mViewingCurves.get( i ).clone();
                            mViewingCurves.set( i, (CurveType)mViewingCurves.get( i + 1 ).clone() );
                            mViewingCurves.set( i + 1, b );
                            changed = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// コントロールトラックに表示しているカーブ種類のリストに，カーブ種類を追加します．
        /// </summary>
        /// <param name="curve"></param>
        public void addViewingCurve( CurveType curve )
        {
            addViewingCurveRange( new CurveType[] { curve } );
        }

        /// <summary>
        /// コントロールトラックに表示しているカーブ種類のリストをクリアします．
        /// </summary>
        public void clearViewingCurve()
        {
            mViewingCurves.clear();
        }

        /// <summary>
        /// このコントロールに担当させるカーブを削除します
        /// </summary>
        /// <param name="curve"></param>
        private void removeViewingCurve( CurveType curve )
        {
            for ( int i = 0; i < mViewingCurves.size(); i++ )
            {
                if ( mViewingCurves.get( i ).equals( curve ) )
                {
                    mViewingCurves.removeElementAt( i );
                    break;
                }
            }
        }

        /// <summary>
        /// メニューアイテムから，そのアイテムが担当しているカーブ種類を取得します
        /// </summary>
        private CurveType getCurveTypeFromMenu( BMenuItem menu )
        {
            for( Iterator<CurveType> itr = mMenuMap.keySet().iterator(); itr.hasNext(); ){
                CurveType curve = itr.next();
                BMenuItem search = mMenuMap.get( curve );
                if( menu == search ){
                    return curve;
                }
            }
            return CurveType.Empty;
        }

#if !JAVA
        #region java.awt.Component
        public void setBounds( int x, int y, int width, int height )
        {
            base.Bounds = new System.Drawing.Rectangle( x, y, width, height );
        }

        public void setBounds( org.kbinani.java.awt.Rectangle rc )
        {
            base.Bounds = new System.Drawing.Rectangle( rc.x, rc.y, rc.width, rc.height );
        }

        public org.kbinani.java.awt.Cursor getCursor()
        {
            System.Windows.Forms.Cursor c = base.Cursor;
            org.kbinani.java.awt.Cursor ret = null;
            if ( c.Equals( System.Windows.Forms.Cursors.Arrow ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.DEFAULT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Cross ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.CROSSHAIR_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Default ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.DEFAULT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.Hand ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.HAND_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.IBeam ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.TEXT_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanEast ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.E_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNE ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.NE_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNorth ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.N_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanNW ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.NW_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSE ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.SE_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSouth ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.S_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanSW ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.SW_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.PanWest ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.W_RESIZE_CURSOR );
            } else if ( c.Equals( System.Windows.Forms.Cursors.SizeAll ) ) {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.MOVE_CURSOR );
            } else {
                ret = new org.kbinani.java.awt.Cursor( org.kbinani.java.awt.Cursor.CUSTOM_CURSOR );
            }
            ret.cursor = c;
            return ret;
        }

        public void setCursor( org.kbinani.java.awt.Cursor value )
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

        public String getName()
        {
            return base.Name;
        }

        public void setName( String value )
        {
            base.Name = value;
        }

#if COMPONENT_ENABLE_LOCATION
        public org.kbinani.java.awt.Point getLocationOnScreen()
        {
            System.Drawing.Point p = base.PointToScreen( base.Location );
            return new org.kbinani.java.awt.Point( p.X, p.Y );
        }

        public org.kbinani.java.awt.Point getLocation()
        {
            System.Drawing.Point loc = this.Location;
            return new org.kbinani.java.awt.Point( loc.X, loc.Y );
        }

        public void setLocation( int x, int y )
        {
            base.Location = new System.Drawing.Point( x, y );
        }

        public void setLocation( org.kbinani.java.awt.Point p )
        {
            base.Location = new System.Drawing.Point( p.x, p.y );
        }
#endif

        public org.kbinani.java.awt.Rectangle getBounds()
        {
            System.Drawing.Rectangle r = base.Bounds;
            return new org.kbinani.java.awt.Rectangle( r.X, r.Y, r.Width, r.Height );
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

        public org.kbinani.java.awt.Dimension getSize()
        {
            return new org.kbinani.java.awt.Dimension( base.Size.Width, base.Size.Height );
        }

        public void setSize( int width, int height )
        {
            base.Size = new System.Drawing.Size( width, height );
        }

        public void setSize( org.kbinani.java.awt.Dimension d )
        {
            setSize( d.width, d.height );
        }

        public void setBackground( org.kbinani.java.awt.Color color )
        {
            base.BackColor = System.Drawing.Color.FromArgb( color.getRed(), color.getGreen(), color.getBlue() );
        }

        public org.kbinani.java.awt.Color getBackground()
        {
            return new org.kbinani.java.awt.Color( base.BackColor.R, base.BackColor.G, base.BackColor.B );
        }

        public void setForeground( org.kbinani.java.awt.Color color )
        {
            base.ForeColor = color.color;
        }

        public org.kbinani.java.awt.Color getForeground()
        {
            return new org.kbinani.java.awt.Color( base.ForeColor.R, base.ForeColor.G, base.ForeColor.B );
        }

        public bool isEnabled()
        {
            return base.Enabled;
        }

        public void setEnabled( bool value )
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

        public void setPreferredSize( org.kbinani.java.awt.Dimension size )
        {
            base.Size = new System.Drawing.Size( size.width, size.height );
        }

        public org.kbinani.java.awt.Font getFont()
        {
            return new org.kbinani.java.awt.Font( base.Font );
        }

        public void setFont( org.kbinani.java.awt.Font font )
        {
            if ( font == null ) {
                return;
            }
            if ( font.font == null ) {
                return;
            }
            base.Font = font.font;
        }
        #endregion
#endif

        #region common APIs of org.kbinani.*
        // root implementation is in BForm.cs
        public java.awt.Point pointToScreen( java.awt.Point point_on_client )
        {
            java.awt.Point p = getLocationOnScreen();
            return new java.awt.Point( p.x + point_on_client.x, p.y + point_on_client.y );
        }

        public java.awt.Point pointToClient( java.awt.Point point_on_screen )
        {
            java.awt.Point p = getLocationOnScreen();
            return new java.awt.Point( point_on_screen.x - p.x, point_on_screen.y - p.y );
        }

#if JAVA
        Object tag = null;
        public Object getTag(){
            return tag;
        }

        public void setTag( Object value ){
            tag = value;
        }
#else
        public Object getTag()
        {
            return base.Tag;
        }

        public void setTag( Object value )
        {
            base.Tag = value;
        }
#endif
        #endregion

        private LineGraphDrawer getGraphDrawer()
        {
            if ( mGraphDrawer == null ) {
                mGraphDrawer = new LineGraphDrawer( LineGraphDrawer.TYPE_STEP );
            }
            return mGraphDrawer;
        }

        /// <summary>
        /// 描画幅が2ピクセルのストロークを取得します
        /// </summary>
        /// <returns></returns>
        private BasicStroke getStroke2px()
        {
            if ( mStroke2px == null ) {
                mStroke2px = new BasicStroke( 2.0f );
            }
            return mStroke2px;
        }

        /// <summary>
        /// デフォルトのストロークを取得します
        /// </summary>
        /// <returns></returns>
        private BasicStroke getStrokeDefault()
        {
            if ( mStrokeDefault == null ) {
                mStrokeDefault = new BasicStroke();
            }
            return mStrokeDefault;
        }

        public void applyLanguage()
        {
        }

        public void applyFont( java.awt.Font font )
        {
            Util.applyFontRecurse( this, font );
            Util.applyContextMenuFontRecurse( cmenuSinger, font );
            Util.applyContextMenuFontRecurse( cmenuCurve, font );
        }

        private int getMaxColumns()
        {
            int max_columns = AppManager.keyWidth / AppManager.MIN_KEY_WIDTH;
            if ( max_columns < 1 ) {
                max_columns = 1;
            }
            return max_columns;
        }

        public int getRowsPerColumn()
        {
            int max_columns = getMaxColumns();
            int row_per_column = getViewingCurveCount() / max_columns;
            if ( row_per_column * max_columns < getViewingCurveCount() ) {
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
        private void executeCommand( CadenciiCommand command, boolean register )
        {
            if ( register ) {
                AppManager.editHistory.register( AppManager.getVsqFile().executeCommand( command ) );
            } else {
                AppManager.getVsqFile().executeCommand( command );
            }
            try {
#if JAVA
                commandExecutedEvent.raise( this, new BEventArgs() );
#elif QT_VERSION
                commandExecuted( this, new BEventArgs() );
#else
                if ( CommandExecuted != null ) {
                    CommandExecuted.Invoke( this, new BEventArgs() );
                }
#endif
            } catch ( Exception ex ) {
                serr.println( "TrackSelector#executeCommand; ex=" + ex );
            }
        }

        public ValuePair<Integer, Integer> getSelectedRegion()
        {
            int x0 = AppManager.mCurveSelectedInterval.getStart();
            int x1 = AppManager.mCurveSelectedInterval.getEnd();
            int min = Math.Min( x0, x1 );
            int max = Math.Max( x0, x1 );
            return new ValuePair<Integer, Integer>( min, max );
        }

        /// <summary>
        /// 現在最前面に表示され，編集可能となっているカーブの種類を取得または設定します
        /// </summary>
        public CurveType getSelectedCurve()
        {
            return mSelectedCurve;
        }

        public void setSelectedCurve( CurveType value )
        {
            CurveType old = mSelectedCurve;
            mSelectedCurve = value;
            if ( !old.equals( mSelectedCurve ) ) {
                mLastSelectedCurve = old;
                try {
#if JAVA
                    selectedCurveChangedEvent.raise( this, mSelectedCurve );
#elif QT_VERSION
                    selectedCurveChanged( this, mSelectedCurve );
#else
                    if ( SelectedCurveChanged != null ) {
                        SelectedCurveChanged.Invoke( this, mSelectedCurve );
                    }
#endif
                } catch ( Exception ex ) {
                    serr.println( "TrackSelector#setSelectedCurve; ex=" + ex );
                }
            }
        }

        /// <summary>
        /// エディタのy方向の位置から，カーブの値を求めます
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public int valueFromYCoord( int y )
        {
            int max = 127;
            int min = 0;
            if ( mSelectedCurve.equals( CurveType.VEL ) ) {
                int selected = AppManager.getSelected();
                if ( AppManager.mDrawIsUtau[selected - 1] ) {
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
            return valueFromYCoord( y, max, min );
        }

        public int valueFromYCoord( int y, int max, int min )
        {
            int oy = getHeight() - 42;
            float order = getGraphHeight() / (float)(max - min);
            return (int)((oy - y) / order) + min;
        }

        public int yCoordFromValue( int value )
        {
            int max = 127;
            int min = 0;
            if ( mSelectedCurve.equals( CurveType.VEL ) ) {
                int selected = AppManager.getSelected();
                if ( AppManager.mDrawIsUtau[selected - 1] ) {
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
            return yCoordFromValue( value, max, min );
        }

        public int yCoordFromValue( int value, int max, int min )
        {
            int oy = getHeight() - 42;
            float order = getGraphHeight() / (float)(max - min);
            return oy - (int)((value - min) * order);
        }

        /// <summary>
        /// カーブエディタを表示するかどうかを取得します
        /// </summary>
        public boolean isCurveVisible()
        {
            return mCurveVisible;
        }

        /// <summary>
        /// カーブエディタを表示するかどうかを設定します
        /// </summary>
        /// <param name="value"></param>
        public void setCurveVisible( boolean value )
        {
            mCurveVisible = value;
        }

#if !JAVA
        private Graphics2D getGraphics()
        {
            if ( mGraphics == null ) {
                mGraphics = new Graphics2D( null );
            }
            return mGraphics;
        }

        /// <summary>
        /// オーバーライドされます
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint( System.Windows.Forms.PaintEventArgs e )
        {
            Graphics2D g = getGraphics();
            g.nativeGraphics = e.Graphics;
            paint( g );
        }
#endif

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
        public Rectangle getRectFromCurveType( CurveType curve )
        {
            int row_per_column = getRowsPerColumn();

            int centre = (getGraphHeight() + UNIT_HEIGHT_PER_CURVE) / 2 + 3;
            int index = 100;
            for ( int i = 0; i < getViewingCurveCount(); i++ ) {
                if ( getViewingCurveElement( i ).equals( curve ) ) {
                    index = i;
                    break;
                }
            }
            int ix = index / row_per_column;
            int iy = index - ix * row_per_column;
            int x = 7 + ix * AppManager.MIN_KEY_WIDTH;
            int y = centre - row_per_column * UNIT_HEIGHT_PER_CURVE / 2 + 2 + UNIT_HEIGHT_PER_CURVE * iy;
            int min_size = getPreferredMinSize();
            if ( mLastPreferredMinHeight != min_size ) {
                try {
#if JAVA
                    preferredMinHeightChangedEvent.raise( this, new BEventArgs() );
#else
                    if ( PreferredMinHeightChanged != null ) {
                        PreferredMinHeightChanged.Invoke( this, new BEventArgs() );
                    }
#endif
                } catch ( Exception ex ) {
                    serr.println( "TrackSelector#getRectFromCurveType; ex=" + ex );
                }
                mLastPreferredMinHeight = min_size;
            }
            return new Rectangle( x, y, 56, 14 );
        }

        /// <summary>
        /// コントロール画面を描画します
        /// </summary>
        /// <param name="graphics"></param>
        public void paint( Graphics graphics )
        {
#if JAVA
            super.paint( graphics );
#endif
            int width = getWidth();
            int height = getHeight();
            int graph_height = getGraphHeight();
            Dimension size = new Dimension( width + 2, height );
            Graphics2D g = (Graphics2D)graphics;
            Color brs_string = Color.black;
            Color rect_curve = new Color( 41, 46, 55 );
            int centre = HEADER + graph_height / 2;
            g.setColor( PortUtil.DarkGray );
            g.fillRect( 0, size.height - 2 * OFFSET_TRACK_TAB, size.width, 2 * OFFSET_TRACK_TAB );
            int numeric_view = mMouseValue;
            Point p = pointToClient( PortUtil.getMousePosition() );
            Point mouse = new Point( p.x, p.y );
            VsqFileEx vsq = AppManager.getVsqFile();
            int selected = AppManager.getSelected();
            int key_width = AppManager.keyWidth;
            int stdx = AppManager.getStartToDrawX();
            int graph_max_y = HEADER + graph_height;
            int graph_min_y = HEADER;

            try {
                #region SINGER
                Shape last = g.getClip();
                g.setColor( AppManager.COLOR_BORDER );
                g.drawLine( 0, size.height - 2 * OFFSET_TRACK_TAB,
                            size.width - 0, size.height - 2 * OFFSET_TRACK_TAB );
                g.setFont( AppManager.baseFont8 );
                g.setColor( brs_string );
                g.drawString(
                    "SINGER",
                    9,
                    size.height - 2 * OFFSET_TRACK_TAB + OFFSET_TRACK_TAB / 2 - AppManager.baseFont8OffsetHeight );
                g.clipRect( key_width, size.height - 2 * OFFSET_TRACK_TAB,
                            size.width - key_width, OFFSET_TRACK_TAB );
                VsqTrack vsq_track = null;
                if ( vsq != null ) {
                    vsq_track = vsq.Track.get( selected );
                }
                if ( vsq_track != null ) {
                    int ycoord = size.height - 2 * OFFSET_TRACK_TAB + 1;

                    // 左端での歌手を最初に描画
                    int x_at_left = AppManager.keyWidth + AppManager.keyOffset;
                    int clock_at_left = AppManager.clockFromXCoord( x_at_left );
                    VsqEvent singer_at_left = vsq_track.getSingerEventAt( clock_at_left );
                    if ( singer_at_left != null ) {
                        Rectangle rc =
                            new Rectangle( x_at_left, ycoord,
                                           SINGER_ITEM_WIDTH, OFFSET_TRACK_TAB - 2 );
                        g.setColor( Color.lightGray );
                        g.fillRect( rc.x, rc.y, rc.width, rc.height );
                        g.setColor( COLOR_SINGERBOX_BORDER );
                        g.drawRect( rc.x, rc.y, rc.width, rc.height );
                        g.setColor( brs_string );
                        g.drawString(
                            singer_at_left.ID.IconHandle.IDS,
                            rc.x, rc.y + OFFSET_TRACK_TAB / 2 - AppManager.baseFont8OffsetHeight );
                    }

                    // 歌手設定を順に描画
                    int event_count = vsq_track.getEventCount();
                    for ( int i = 0; i < event_count; i++ ) {
                        VsqEvent ve = vsq_track.getEvent( i );
                        if ( ve.ID.type != VsqIDType.Singer ) {
                            continue;
                        }
                        int clock = ve.Clock;
                        IconHandle singer_handle = (IconHandle)ve.ID.IconHandle;
                        int x = AppManager.xCoordFromClocks( clock );
                        if ( x < x_at_left ) {
                            continue;
                        }
                        Rectangle rc =
                            new Rectangle( x, ycoord,
                                           SINGER_ITEM_WIDTH, OFFSET_TRACK_TAB - 2 );
                        if ( AppManager.itemSelection.isEventContains( selected, ve.InternalID ) ) {
                            g.setColor( AppManager.getHilightColor() );
                        } else {
                            g.setColor( Color.white );
                        }
                        g.fillRect( rc.x, rc.y, rc.width, rc.height );
                        g.setColor( COLOR_SINGERBOX_BORDER );
                        g.drawRect( rc.x, rc.y, rc.width, rc.height );
                        g.setColor( brs_string );
                        g.drawString(
                            singer_handle.IDS,
                            rc.x, rc.y + OFFSET_TRACK_TAB / 2 - AppManager.baseFont8OffsetHeight );
                    }
                }
                g.setClip( last );
                #endregion

                #region トラック選択欄
                int selecter_width = getSelectorWidth();
                g.setColor( AppManager.COLOR_BORDER );
                g.drawLine( 0, size.height - OFFSET_TRACK_TAB,
                            size.width, size.height - OFFSET_TRACK_TAB );
                g.setColor( brs_string );
                g.drawString( "TRACK", 9, size.height - OFFSET_TRACK_TAB + OFFSET_TRACK_TAB / 2 - AppManager.baseFont8OffsetHeight );
                if ( vsq != null ) {
                    for ( int i = 0; i < 16; i++ ) {
                        int x = key_width + i * selecter_width;
#if DEBUG
                        try {
#endif
                            drawTrackTab( g,
                                          new Rectangle( x, size.height - OFFSET_TRACK_TAB + 1, selecter_width, OFFSET_TRACK_TAB - 1 ),
                                          (i + 1 < vsq.Track.size()) ? (i + 1) + " " + vsq.Track.get( i + 1 ).getName() : "",
                                          (i == selected - 1),
                                          vsq_track.getCommon().PlayMode >= 0,
                                          AppManager.getRenderRequired( i + 1 ),
                                          AppManager.HILIGHT[i],
                                          AppManager.RENDER[i] );
#if DEBUG
                        } catch ( Exception ex ) {
                            AppManager.debugWriteLine( "TrackSelector.DrawTo; ex=" + ex );
                        }
#endif
                    }
                }
                #endregion

                int clock_at_mouse = AppManager.clockFromXCoord( mouse.x );
                int pbs_at_mouse = 0;
                if ( mCurveVisible ) {
                    #region カーブエディタ
                    // カーブエディタの下の線
                    g.setColor( new Color( 156, 161, 169 ) );
                    g.drawLine( key_width, size.height - 42,
                                size.width - 3, size.height - 42 );

                    // カーブエディタの上の線
                    g.setColor( new Color( 46, 47, 50 ) );
                    g.drawLine( key_width, HEADER,
                                size.width - 3, HEADER );

                    g.setColor( new Color( 125, 123, 124 ) );
                    g.drawLine( key_width, 0,
                                key_width, size.height - 1 );

                    if ( AppManager.isCurveSelectedIntervalEnabled() ) {
                        int x0 = AppManager.xCoordFromClocks( AppManager.mCurveSelectedInterval.getStart() );
                        int x1 = AppManager.xCoordFromClocks( AppManager.mCurveSelectedInterval.getEnd() );
                        g.setColor( COLOR_A072R255G255B255 );
                        g.fillRect( x0, HEADER, x1 - x0, graph_height );
                    }

                    #region 小節ごとのライン
                    if ( vsq != null ) {
                        int dashed_line_step = AppManager.getPositionQuantizeClock();
                        g.clipRect( key_width, HEADER, size.width - key_width, size.height - 2 * OFFSET_TRACK_TAB );
                        Color white100 = new Color( 0, 0, 0, 100 );
                        for ( Iterator<VsqBarLineType> itr = vsq.getBarLineIterator( AppManager.clockFromXCoord( width ) ); itr.hasNext(); ) {
                            VsqBarLineType blt = itr.next();
                            int x = AppManager.xCoordFromClocks( blt.clock() );
                            int local_clock_step = 480 * 4 / blt.getLocalDenominator();
                            if ( blt.isSeparator() ) {
                                g.setColor( white100 );
                                g.drawLine( x, size.height - 42 - 1, x, 8 + 1 );
                            } else {
                                g.setColor( white100 );
                                g.drawLine( x, centre - 5, x, centre + 6 );
                                Color pen = new Color( 12, 12, 12 );
                                g.setColor( pen );
                                g.drawLine( x, 8, x, 14 );
                                g.drawLine( x, size.height - 43, x, size.height - 42 - 6 );
                            }
                            if ( dashed_line_step > 1 && AppManager.isGridVisible() ) {
                                int numDashedLine = local_clock_step / dashed_line_step;
                                Color pen = new Color( 65, 65, 65 );
                                g.setColor( pen );
                                for ( int i = 1; i < numDashedLine; i++ ) {
                                    int x2 = AppManager.xCoordFromClocks( blt.clock() + i * dashed_line_step );
                                    g.drawLine( x2, centre - 2, x2, centre + 3 );
                                    g.drawLine( x2, 8, x2, 12 );
                                    g.drawLine( x2, size.height - 43, x2, size.height - 43 - 4 );
                                }
                            }
                        }
                        g.setClip( null );
                    }
                    #endregion

                    if ( vsq_track != null ) {
                        Color color = AppManager.getHilightColor();
                        Color front = new Color( color.getRed(), color.getGreen(), color.getBlue(), 150 );
                        Color back = new Color( 255, 249, 255, 44 );
                        Color vel_color = new Color( 64, 78, 30 );

                        // 後ろに描くカーブ
                        if ( mLastSelectedCurve.equals( CurveType.VEL ) || mLastSelectedCurve.equals( CurveType.Accent ) || mLastSelectedCurve.equals( CurveType.Decay ) ) {
                            drawVEL( g, vsq_track, back, false, mLastSelectedCurve );
                        } else if ( mLastSelectedCurve.equals( CurveType.VibratoRate ) || mLastSelectedCurve.equals( CurveType.VibratoDepth ) ) {
                            drawVibratoControlCurve( g, vsq_track, mLastSelectedCurve, back, false );
                        } else {
                            VsqBPList list_back = vsq_track.getCurve( mLastSelectedCurve.getName() );
                            if ( list_back != null ) {
                                drawVsqBPList( g, list_back, back, false );
                            }
                        }

                        // 手前に描くカーブ
                        if ( mSelectedCurve.equals( CurveType.VEL ) || mSelectedCurve.equals( CurveType.Accent ) || mSelectedCurve.equals( CurveType.Decay ) ) {
                            drawVEL( g, vsq_track, vel_color, true, mSelectedCurve );
                        } else if ( mSelectedCurve.equals( CurveType.VibratoRate ) || mSelectedCurve.equals( CurveType.VibratoDepth ) ) {
                            drawVibratoControlCurve( g, vsq_track, mSelectedCurve, front, true );
                        } else if ( mSelectedCurve.equals( CurveType.Env ) ) {
                            drawEnvelope( g, selected, front );
                        } else {
                            VsqBPList list_front = vsq_track.getCurve( mSelectedCurve.getName() );
                            if ( list_front != null ) {
                                drawVsqBPList( g, list_front, front, true );
                            }
                            if ( mSelectedCurve.equals( CurveType.PIT ) ) {
                                #region PBSの値に応じて，メモリを記入する
#if !JAVA
                                System.Drawing.Drawing2D.SmoothingMode old = g.nativeGraphics.SmoothingMode;
                                g.nativeGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
#endif
                                Color nrml = new Color( 0, 0, 0, 190 );
                                Color dash = new Color( 0, 0, 0, 128 );
                                Stroke nrml_stroke = new BasicStroke();
                                Stroke dash_stroke = new BasicStroke( 1.0f, 0, 0, 10.0f, new float[] { 2.0f, 2.0f }, 0.0f );
                                VsqBPList pbs = vsq_track.MetaText.PBS;
                                pbs_at_mouse = pbs.getValue( clock_at_mouse );
                                int c = pbs.size();
                                int premeasure = vsq.getPreMeasureClocks();
                                int clock_start = AppManager.clockFromXCoord( key_width );
                                int clock_end = AppManager.clockFromXCoord( width );
                                if ( clock_start < premeasure && premeasure < clock_end ) {
                                    clock_start = premeasure;
                                }
                                int last_pbs = pbs.getValue( clock_start );
                                int last_clock = clock_start;
                                int ycenter = yCoordFromValue( 0 );
                                g.setColor( nrml );
                                g.drawLine( key_width, ycenter, width, ycenter );
                                for ( int i = 0; i < c; i++ ) {
                                    int cl = pbs.getKeyClock( i );
                                    if ( cl < clock_start ) {
                                        continue;
                                    }
                                    if ( clock_end < cl ) {
                                        break;
                                    }
                                    int thispbs = pbs.getElement( i );
                                    if ( last_pbs == thispbs ) {
                                        continue;
                                    }
                                    // last_clockからclの範囲で，PBSの値がlas_pbs
                                    int max = last_pbs;
                                    int min = -last_pbs;
                                    int x1 = AppManager.xCoordFromClocks( last_clock );
                                    int x2 = AppManager.xCoordFromClocks( cl );
                                    for ( int j = min + 1; j <= max - 1; j++ ) {
                                        if ( j == 0 ) {
                                            continue;
                                        }
                                        int y = yCoordFromValue( (int)(j * 8192 / (double)last_pbs) );
                                        if ( j % 2 == 0 ) {
                                            g.setColor( nrml );
                                            g.setStroke( nrml_stroke );
                                            g.drawLine( x1, y, x2, y );
                                        } else {
                                            g.setColor( dash );
                                            g.setStroke( dash_stroke );
                                            g.drawLine( x1, y, x2, y );
                                        }
                                    }
                                    g.setStroke( new BasicStroke() );
                                    last_clock = cl;
                                    last_pbs = thispbs;
                                }
                                int max0 = last_pbs;
                                int min0 = -last_pbs;
                                int x10 = AppManager.xCoordFromClocks( last_clock );
                                int x20 = AppManager.xCoordFromClocks( clock_end );
                                for ( int j = min0 + 1; j <= max0 - 1; j++ ) {
                                    if ( j == 0 ) {
                                        continue;
                                    }
                                    int y = yCoordFromValue( (int)(j * 8192 / (double)last_pbs) );
                                    Color pen = dash;
                                    if ( j % 2 == 0 ) {
                                        pen = nrml;
                                    }
                                    g.setColor( pen );
                                    g.drawLine( x10, y, x20, y );
                                }
#if !JAVA
                                g.nativeGraphics.SmoothingMode = old;
#endif
                                #endregion
                            }
                            drawAttachedCurve( g, vsq.AttachedCurves.get( AppManager.getSelected() - 1 ).get( mSelectedCurve ) );
                        }
                    }

                    if ( AppManager.isWholeSelectedIntervalEnabled() ) {
                        int start = AppManager.xCoordFromClocks( AppManager.mWholeSelectedInterval.getStart() ) + 2;
                        int end = AppManager.xCoordFromClocks( AppManager.mWholeSelectedInterval.getEnd() ) + 2;
                        g.setColor( COLOR_A098R000G000B000 );
                        g.fillRect( start, HEADER, end - start, graph_height );
                    }

                    if ( mMouseDowned ) {
                        #region 選択されたツールに応じて描画
                        int value = valueFromYCoord( mouse.y );
                        if ( clock_at_mouse < vsq.getPreMeasure() ) {
                            clock_at_mouse = vsq.getPreMeasure();
                        }
                        int max = mSelectedCurve.getMaximum();
                        int min = mSelectedCurve.getMinimum();
                        if ( value < min ) {
                            value = min;
                        } else if ( max < value ) {
                            value = max;
                        }
                        EditTool tool = AppManager.getSelectedTool();
                        if ( tool == EditTool.LINE ) {
#if OLD_IMPL_MOUSE_TRACER
                            int xini = AppManager.xCoordFromClocks( m_line_start.x );
                            int yini = yCoordFromValue( m_line_start.y );
                            g.setColor( s_pen_050_140_150 );
                            g.drawLine( xini, yini, AppManager.xCoordFromClocks( clock_at_mouse ), yCoordFromValue( value ) );
#else
                            if ( mMouseTracer.size() > 0 ) {
                                Point pt = mMouseTracer.iterator().next();
                                int xini = pt.x - stdx;
                                int yini = pt.y;
                                g.setColor( Color.ORANGE );
                                g.setStroke( getStroke2px() );
#if !JAVA
                                g.nativeGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
#endif
                                g.drawLine( xini, yini, AppManager.xCoordFromClocks( clock_at_mouse ), yCoordFromValue( value ) );
#if !JAVA
                                g.nativeGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
#endif
                                g.setStroke( getStrokeDefault() );
                            }
#endif
                        } else if ( tool == EditTool.PENCIL ) {
                            if ( mMouseTracer.size() > 0 && !AppManager.isCurveMode() ) {
                                LineGraphDrawer d = getGraphDrawer();
                                d.clear();
                                d.setGraphics( g );
                                d.setBaseLineY( graph_max_y );
                                d.setFill( true );
                                d.setDotMode( LineGraphDrawer.DOTMODE_NO );
                                d.setDrawLine( false );
                                d.setFillColor( COLOR_MOUSE_TRACER );
                                for ( Iterator<Point> itr = mMouseTracer.iterator(); itr.hasNext(); ) {
                                    Point pt = itr.next();
                                    int x = pt.x - stdx;
                                    int y = pt.y;
                                    if ( y < graph_min_y ) {
                                        y = graph_min_y;
                                    } else if ( graph_max_y < y ) {
                                        y = graph_max_y;
                                    }
                                    d.append( x, y );
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
                        } else if ( tool == EditTool.ERASER || tool == EditTool.ARROW ) {
                            if ( mMouseDownMode == MouseDownMode.CURVE_EDIT && mMouseMoved && AppManager.mCurveSelectingRectangle.width != 0 ) {
                                int xini = AppManager.xCoordFromClocks( AppManager.mCurveSelectingRectangle.x );
                                int xend = AppManager.xCoordFromClocks( AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width );
                                int x_start = Math.Min( xini, xend );
                                if ( x_start < key_width ) {
                                    x_start = key_width;
                                }
                                int x_end = Math.Max( xini, xend );
                                int yini = yCoordFromValue( AppManager.mCurveSelectingRectangle.y );
                                int yend = yCoordFromValue( AppManager.mCurveSelectingRectangle.y + AppManager.mCurveSelectingRectangle.height );
                                int y_start = Math.Min( yini, yend );
                                int y_end = Math.Max( yini, yend );
                                if ( y_start < 8 ) y_start = 8;
                                if ( y_end > height - 42 - 8 ) y_end = height - 42;
                                if ( x_start < x_end ) {
                                    g.setColor( COLOR_A144R255G255B255 );
                                    g.fillRect( x_start, y_start, x_end - x_start, y_end - y_start );
                                }
                            } else if ( mMouseDownMode == MouseDownMode.VEL_EDIT && mVelEditSelected.containsKey( mVelEditLastSelectedID ) ) {
                                if ( mSelectedCurve.equals( CurveType.VEL ) ) {
                                    numeric_view = mVelEditSelected.get( mVelEditLastSelectedID ).editing.ID.Dynamics;
                                } else if ( mSelectedCurve.equals( CurveType.Accent ) ) {
                                    numeric_view = mVelEditSelected.get( mVelEditLastSelectedID ).editing.ID.DEMaccent;
                                } else if ( mSelectedCurve.equals( CurveType.Decay ) ) {
                                    numeric_view = mVelEditSelected.get( mVelEditLastSelectedID ).editing.ID.DEMdecGainRate;
                                }
                            }
                        }
                        if ( mMouseDownMode == MouseDownMode.SINGER_LIST && AppManager.getSelectedTool() != EditTool.ERASER ) {
                            for ( Iterator<SelectedEventEntry> itr = AppManager.itemSelection.getEventIterator(); itr.hasNext(); ) {
                                SelectedEventEntry item = itr.next();
                                int x = AppManager.xCoordFromClocks( item.editing.Clock );
                                g.setColor( COLOR_SINGERBOX_BORDER_HILIGHT );
                                g.drawRect( x, size.height - 2 * OFFSET_TRACK_TAB + 1,
                                           SINGER_ITEM_WIDTH, OFFSET_TRACK_TAB - 2 );
                            }
                        }
                        #endregion
                    }
                    #endregion
                }

                if ( mCurveVisible ) {
                    #region カーブの種類一覧
#if JAVA
                    Font text_font = AppManager.baseFont8;
                    int text_font_height = AppManager.baseFont8Height;
                    int text_font_offset = AppManager.baseFont8OffsetHeight;
#else
                    Font text_font = AppManager.baseFont9;
                    int text_font_height = AppManager.baseFont9Height;
                    int text_font_offset = AppManager.baseFont9OffsetHeight;
#endif
                    Color font_color_normal = Color.black;
                    g.setColor( new Color( 212, 212, 212 ) );
                    g.fillRect( 0, 0, key_width, size.height - 2 * OFFSET_TRACK_TAB );

                    // 現在表示されているカーブの名前
                    g.setFont( text_font );
                    g.setColor( brs_string );
                    boolean is_utau_mode = AppManager.mDrawIsUtau[selected - 1];
                    String name = (is_utau_mode && mSelectedCurve.equals( CurveType.VEL )) ? "INT" : mSelectedCurve.getName();
                    g.drawString( name, 7, text_font_height / 2 - text_font_offset + 1 );

                    for ( int i = 0; i < getViewingCurveCount(); i++ ) {
                        CurveType curve = getViewingCurveElement( i );
                        Rectangle rc = getRectFromCurveType( curve );
                        if ( curve.equals( mSelectedCurve ) || curve.equals( mLastSelectedCurve ) ) {
                            g.setColor( new Color( 108, 108, 108 ) );
                            g.fillRect( rc.x, rc.y, rc.width, rc.height );
                        }
                        g.setColor( rect_curve );
                        g.drawRect( rc.x, rc.y, rc.width, rc.height );
                        int rc_str_x = rc.x + 2;
                        int rc_str_y = rc.y + text_font_height / 2 - text_font_offset + 1;
                        String n = curve.getName();
                        if ( is_utau_mode && curve.equals( CurveType.VEL ) ) {
                            n = "INT";
                        }
                        if ( curve.equals( mSelectedCurve ) ) {
                            g.setColor( Color.white );
                            g.drawString( n, rc_str_x, rc_str_y );
                        } else {
                            g.setColor( font_color_normal );
                            g.drawString( n, rc_str_x, rc_str_y );
                        }
                    }
                    #endregion
                }

                #region 現在のマーカー
                int marker_x = AppManager.xCoordFromClocks( AppManager.getCurrentClock() );
                if ( key_width <= marker_x && marker_x <= size.width ) {
                    g.setColor( Color.white );
                    g.setStroke( new BasicStroke( 2f ) );
                    g.drawLine( marker_x, 0, marker_x, size.height - 18 );
                    g.setStroke( new BasicStroke() );
                }
                #endregion

                // マウス位置での値
                if ( isInRect( mouse.x, mouse.y, new Rectangle( key_width, HEADER, width, graph_height ) ) &&
                     mMouseDownMode != MouseDownMode.PRE_UTTERANCE_MOVE &&
                     mMouseDownMode != MouseDownMode.OVERLAP_MOVE &&
                     mMouseDownMode != MouseDownMode.VEL_EDIT ) {
                    int align = 1;
                    int valign = 0;
                    int shift = 50;
                    if ( mSelectedCurve.equals( CurveType.PIT ) ) {
                        valign = 1;
                        shift = 100;
                    }
                    g.setFont( AppManager.baseFont10Bold );
                    g.setColor( Color.white );
                    PortUtil.drawStringEx( g,
                                           mMouseValue + "",
                                           AppManager.baseFont10Bold,
                                           new Rectangle( mouse.x - 100, mouse.y - shift, 100, 100 ),
                                           align,
                                           valign );
                    if ( mSelectedCurve.equals( CurveType.PIT ) ) {
                        float delta_note = mMouseValue * pbs_at_mouse / 8192.0f;
                        align = 1;
                        valign = -1;
                        PortUtil.drawStringEx( g,
                                               PortUtil.formatDecimal( "#0.00", delta_note ),
                                               AppManager.baseFont10Bold,
                                               new Rectangle( mouse.x - 100, mouse.y, 100, 100 ),
                                               align,
                                               valign );
                    }
                }
            } catch ( Exception ex ) {
                serr.println( "TrackSelector#paint; ex= " + ex );
#if JAVA
                ex.printStackTrace();
#endif
            }
        }

        /// <summary>
        /// 指定したトラックのエンベロープ，先行発音，オーバーラップを画面に描画します
        /// </summary>
        /// <param name="g"></param>
        /// <param name="track"></param>
        /// <param name="fill_color"></param>
        private void drawEnvelope( Graphics2D g, int track_index, Color fill_color )
        {
            int key_width = AppManager.keyWidth;
            int width = getWidth();
            int height = getHeight();
            g.setClip( key_width, 0, width - key_width, height );
            int clock_start = AppManager.clockFromXCoord( key_width );
            int clock_end = AppManager.clockFromXCoord( width );

            VsqFileEx vsq = AppManager.getVsqFile();
            VsqTrack track = vec.get( vsq.Track, track_index );
            VsqEvent itr_prev = null;
            VsqEvent itr_item = null;
            VsqEvent itr_next = null;
            Point mouse = pointToClient( PortUtil.getMousePosition() );

            Color brs = fill_color;
            Point selected_point = new Point();
            boolean selected_found = false;
            // yが範囲内なので，xも検索するときtrue
            boolean search_mouse = (0 <= mouse.y && mouse.y <= height);
            Iterator<VsqEvent> itr = track.getNoteEventIterator();
            int dotwid = DOT_WID * 2 + 1;
            int tolerance = AppManager.editorConfig.PxTolerance;
            // 選択アイテムが1個以上あるので，検索するときtrue
            boolean search_sel = AppManager.itemSelection.getEventCount() > 0;
            while ( true ) {
                boolean draw_env_points = false;
                itr_prev = itr_item;
                itr_item = itr_next;
                if ( itr.hasNext() ) {
                    itr_next = itr.next();
                } else {
                    itr_next = null;
                }
                if ( itr_item == null && itr_prev == null && itr_next == null ) {
                    break;
                }
                if ( itr_item == null ) {
                    continue;
                }
                VsqEvent item = itr_item;
                if ( item.Clock + item.ID.getLength() < clock_start ) {
                    continue;
                }
                if ( clock_end < item.Clock ) {
                    break;
                }
                VsqEvent prev_item = itr_prev;
                VsqEvent next_item = itr_next;
                if ( prev_item != null ) {
                    if ( prev_item.Clock + prev_item.ID.getLength() < item.Clock ) {
                        // 直前の音符と接続していないのでnullにする
                        prev_item = null;
                    }
                }
                if ( next_item != null ) {
                    if ( item.Clock + item.ID.getLength() < next_item.Clock ) {
                        // 直後の音符と接続していないのでnullにする
                        next_item = null;
                    }
                }
                ByRef<Integer> preutterance = new ByRef<Integer>();
                ByRef<Integer> overlap = new ByRef<Integer>();
                Polygon points = getEnvelopePoints( vsq.TempoTable, prev_item, item, next_item, preutterance, overlap );
                if ( mMouseDownMode == MouseDownMode.ENVELOPE_MOVE && item.InternalID == mEnvelopeEdigintID ) {
                    selected_point = new Point( points.xpoints[mEnvelopePointKind], points.ypoints[mEnvelopePointKind] );
                    selected_found = true;
                }

                // 編集中のアイテムだったら描く
                // エンベロープ
                if ( !draw_env_points ) {
                    if ( mMouseDownMode == MouseDownMode.ENVELOPE_MOVE && item.InternalID == mEnvelopeEdigintID ) {
                        draw_env_points = true;
                    }
                }
                // 先行発音
                if ( !draw_env_points ) {
                    if ( mMouseDownMode == MouseDownMode.PRE_UTTERANCE_MOVE && item.InternalID == mPreUtteranceEditingID ) {
                        draw_env_points = true;
                    }
                }
                // オーバーラップ
                if ( !draw_env_points ) {
                    if ( mMouseDownMode == MouseDownMode.OVERLAP_MOVE && item.InternalID == mOverlapEditingID ) {
                        draw_env_points = true;
                    }
                }

                // マウスのx座標が範囲内なら描く
                if ( !draw_env_points ) {
                    if ( search_mouse ) {
                        if ( points.xpoints[0] - tolerance <= mouse.x && mouse.x <= points.xpoints[points.npoints - 1] + tolerance ) {
                            draw_env_points = true;
                        }
                    }
                }

                // 選択されてたら描く
                if ( !draw_env_points && search_sel ) {
                    if ( AppManager.itemSelection.isEventContains( track_index, item.InternalID ) ) {
                        draw_env_points = true;
                    }
                }

                // 多角形
                g.setColor( brs );
                g.fillPolygon( points );
                g.setColor( Color.white );
                g.drawPolyline( points.xpoints, points.ypoints, points.npoints );

                if ( draw_env_points ) {
                    // データ点の表示
                    for ( int i = 1; i < 6; i++ ) {
                        Point p = new Point( points.xpoints[i], points.ypoints[i] );
                        Rectangle rc = new Rectangle( p.x - DOT_WID, p.y - DOT_WID, dotwid, dotwid );
                        g.setColor( COLOR_BEZIER_DOT_NORMAL );
                        g.fillRect( rc.x, rc.y, rc.width, rc.height );
                        g.setColor( COLOR_BEZIER_DOT_NORMAL );
                        g.drawRect( rc.x, rc.y, rc.width, rc.height );
                    }

                    // 旗を描く
                    drawPreutteranceAndOverlap(
                        g, 
                        preutterance.value, overlap.value, 
                        item.UstEvent.getPreUtterance(), item.UstEvent.getVoiceOverlap() );
                }
            }

            // 選択されている点のハイライト表示
            if ( selected_found ) {
                Rectangle rc = new Rectangle( selected_point.x - DOT_WID, selected_point.y - DOT_WID, dotwid, dotwid );
                g.setColor( AppManager.getHilightColor() );
                g.fillRect( rc.x, rc.y, rc.width, rc.height );
                g.setColor( COLOR_BEZIER_DOT_NORMAL );
                g.drawRect( rc.x, rc.y, rc.width, rc.height );
            }
            
            g.setClip( null );
        }

        /// <summary>
        /// 先行発音，またはオーバーラップを表示する旗に描く文字列を取得します
        /// </summary>
        /// <param name="flag_is_pre_utterance">先行発音用の文字列を取得する場合にtrue，そうでなければfalseを指定します</param>
        /// <param name="value">先行発音，またはオーバーラップの値</param>
        /// <returns>旗に描くための文字列（Overlap: 0.00など）</returns>
        private static String getFlagTitle( boolean flag_is_pre_utterance, float value )
        {
            if ( flag_is_pre_utterance ) {
                return "Pre Utterance: " + PortUtil.formatDecimal( "0.00", value );
            } else {
                return "Overlap: " + PortUtil.formatDecimal( "0.00", value );
            }
        }

        /// <summary>
        /// 指定した文字列を旗に書いたときの，旗のサイズを計算します
        /// </summary>
        /// <param name="flag_title"></param>
        private Dimension getFlagBounds( String flag_title )
        {
            if ( mTextWidthPerLetter <= 0.0f ) {
#if JAVA
                Font font = AppManager.baseFont9;
#else
                Font font = AppManager.baseFont10;
#endif
                Dimension s = Util.measureString( flag_title + " ", font );
                mTextWidthPerLetter = s.width / (float)str.length( flag_title );
                mTextHeight = s.height;
            }
            return new Dimension( (int)(str.length( flag_title) * mTextWidthPerLetter), mTextHeight );
        }

        /// <summary>
        /// 先行発音とオーバーラップを表示する旗を描画します
        /// </summary>
        /// <param name="g"></param>
        /// <param name="px_preutterance"></param>
        /// <param name="px_overlap"></param>
        /// <param name="preutterance"></param>
        /// <param name="overlap"></param>
        private void drawPreutteranceAndOverlap( Graphics2D g, int px_preutterance, int px_overlap, float preutterance, float overlap )
        {
            int graph_height = getGraphHeight();
            g.setColor( PortUtil.Orange );
            g.drawLine( px_preutterance, HEADER + 1, px_preutterance, graph_height + HEADER );
            g.setColor( PortUtil.LawnGreen );
            g.drawLine( px_overlap, HEADER + 1, px_overlap, graph_height + HEADER );

            String s_pre = getFlagTitle( true, preutterance );
            String s_ovl = getFlagTitle( false, overlap );
#if JAVA
            Font font = AppManager.baseFont9;
            int font_height = AppManager.baseFont9Height;
            int font_offset = AppManager.baseFont9OffsetHeight;
#else
            Font font = AppManager.baseFont10;
            int font_height = AppManager.baseFont10Height;
            int font_offset = AppManager.baseFont10OffsetHeight;
#endif
            Dimension pre_bounds = getFlagBounds( s_pre );
            Dimension ovl_bounds = getFlagBounds( s_ovl );

            Color pen = new Color( 0, 0, 0, 50 );
            Color transp = new Color( PortUtil.Orange.getRed(), PortUtil.Orange.getGreen(), PortUtil.Orange.getBlue(), 50 );
            g.setColor( transp );
            g.fillRect( px_preutterance, OFFSET_PRE - FLAG_SPACE, pre_bounds.width, pre_bounds.height + FLAG_SPACE * 2);
            g.setColor( pen );
            g.drawRect( px_preutterance, OFFSET_PRE - FLAG_SPACE, pre_bounds.width, pre_bounds.height + FLAG_SPACE * 2 );
            transp = new Color( PortUtil.LawnGreen.getRed(), PortUtil.LawnGreen.getGreen(), PortUtil.LawnGreen.getBlue(), 50 );
            g.setColor( transp );
            g.fillRect( px_overlap, OFFSET_OVL - FLAG_SPACE, ovl_bounds.width, ovl_bounds.height + FLAG_SPACE * 2 );
            g.setColor( pen );
            g.drawRect( px_overlap, OFFSET_OVL - FLAG_SPACE, ovl_bounds.width, ovl_bounds.height + FLAG_SPACE * 2 );

            g.setFont( font );
            g.setColor( Color.black );
            g.drawString( s_pre, px_preutterance + 1, OFFSET_PRE + font_height / 2 - font_offset + 1  );
            g.drawString( s_ovl, px_overlap + 1, OFFSET_OVL + font_height / 2 - font_offset + 1 );
        }

        /// <summary>
        /// 画面上の指定した点に、コントロールカーブのデータ点があるかどうかを調べます
        /// </summary>
        /// <param name="locx">調べたい点の画面上のx座標</param>
        /// <param name="locy">調べたい点の画面上のy座標</param>
        /// <returns>データ点が見つかれば，データ点のid，そうでなければ-1を返します</returns>
        private long findDataPointAt( int locx, int locy )
        {
            if ( mSelectedCurve.equals( CurveType.Accent ) ||
                 mSelectedCurve.equals( CurveType.Decay ) ||
                 mSelectedCurve.equals( CurveType.Env ) ||
                 mSelectedCurve.equals( CurveType.VEL ) ) {
                return -1;
            }
            if ( mSelectedCurve.equals( CurveType.VibratoDepth ) ||
                 mSelectedCurve.equals( CurveType.VibratoRate ) ) {
                //TODO: この辺
            } else {
                VsqBPList list = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( mSelectedCurve.getName() );
                int count = list.size();
                int w = DOT_WID * 2 + 1;
                for ( int i = 0; i < count; i++ ) {
                    int clock = list.getKeyClock( i );
                    VsqBPPair item = list.getElementB( i );
                    int x = AppManager.xCoordFromClocks( clock );
                    if ( x + DOT_WID < AppManager.keyWidth ) {
                        continue;
                    }
                    if ( getWidth() < x - DOT_WID ) {
                        break;
                    }
                    int y = yCoordFromValue( item.value );
                    Rectangle rc = new Rectangle( x - DOT_WID, y - DOT_WID, w, w );
                    if ( isInRect( locx, locy, rc ) ) {
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
        private boolean findEnvelopePointAt( int locx, int locy, ByRef<Integer> internal_id, ByRef<Integer> point_kind )
        {
            return findEnvelopeCore( locx, locy, internal_id, point_kind, null );
        }

        /// <summary>
        /// 画面上の指定した位置に，先行発音またはオーバーラップ用の旗が表示されているかどうかを調べます
        /// </summary>
        /// <param name="locx">調べたい点の画面上のx座標</param>
        /// <param name="locy">調べたい点の画面上のy座標</param>
        /// <param name="internal_id">見つかったイベントを表現するVsqEventのInternalID</param>
        /// <param name="found_flag_was_overlap">見つかった旗がオーバーラップのものであった場合にtrue，それ以外はfalse</param>
        /// <returns>旗が見つかった場合にtrue，それ以外はfalseを返します</returns>
        private boolean findPreUtteranceOrOverlapAt( int locx, int locy, ByRef<Integer> internal_id, ByRef<Boolean> found_flag_was_overlap )
        {
            return findEnvelopeCore( locx, locy, internal_id, null, found_flag_was_overlap );
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
        private boolean findEnvelopeCore(
            int locx, int locy, 
            ByRef<Integer> internal_id, 
            ByRef<Integer> point_kind, ByRef<Boolean> found_flag_was_overlap )
        {
            internal_id.value = -1;
            if ( point_kind != null ) {
                point_kind.value = -1;
            }

            int clock_start = AppManager.clockFromXCoord( AppManager.keyWidth );
            int clock_end = AppManager.clockFromXCoord( getWidth() );
            int dotwid = DOT_WID * 2 + 1;
            VsqFileEx vsq = AppManager.getVsqFile();
            Iterator<VsqEvent> itr = vsq.Track.get( AppManager.getSelected() ).getNoteEventIterator();
            VsqEvent itr_prev = null;
            VsqEvent itr_item = null;
            VsqEvent itr_next = null;
            ByRef<Integer> px_preutterance = new ByRef<Integer>();
            ByRef<Integer> px_overlap = new ByRef<Integer>();
            Dimension size = new Dimension();
            while( true ){
                itr_prev = itr_item;
                itr_item = itr_next;
                if ( itr.hasNext() ) {
                    itr_next = itr.next();
                } else {
                    itr_next = null;
                }
                if ( itr_prev == null && itr_item == null && itr_next == null ) {
                    break;
                }
                VsqEvent item = itr_item;
                if ( item == null ) {
                    continue;
                }
                if ( item.Clock + item.ID.getLength() < clock_start ) {
                    continue;
                }
                if ( clock_end < item.Clock ) {
                    break;
                }
                VsqEvent prev_item = itr_prev;
                VsqEvent next_item = itr_next;
                if ( prev_item != null ) {
                    if ( prev_item.Clock + prev_item.ID.getLength() < item.Clock ) {
                        // 直前の音符と接続していないのでnullにする
                        prev_item = null;
                    }
                }
                if ( next_item != null ) {
                    if ( item.Clock + item.ID.getLength() < next_item.Clock ) {
                        // 直後の音符と接続していないのでnullにする
                        next_item = null;
                    }
                }
                // エンベロープの点の座標を計算
                Polygon points = getEnvelopePoints( vsq.TempoTable, prev_item, item, next_item, px_preutterance, px_overlap );
                // エンベロープの点の当たり判定
                if ( point_kind != null ) {
                    for ( int i = 5; i >= 1; i-- ) {
                        Point p = new Point( points.xpoints[i], points.ypoints[i] );
                        Rectangle rc = new Rectangle( p.x - DOT_WID, p.y - DOT_WID, dotwid, dotwid );
                        if ( isInRect( locx, locy, rc ) ) {
                            internal_id.value = item.InternalID;
                            point_kind.value = i;
                            return true;
                        }
                    }
                }
                // 先行発音の旗の当たり判定
                if ( found_flag_was_overlap != null ) {
                    String title_preutterance = getFlagTitle( true, item.UstEvent.getPreUtterance() );
                    size = getFlagBounds( title_preutterance );
                    if ( Utility.isInRect( locx, locy, px_preutterance.value, OFFSET_PRE - FLAG_SPACE, size.width, size.height + FLAG_SPACE * 2 ) ) {
                        internal_id.value = item.InternalID;
                        found_flag_was_overlap.value = false;
                        return true;
                    }
                    // オーバーラップ用の旗の当たり判定
                    String title_overlap = getFlagTitle( false, item.UstEvent.getVoiceOverlap() );
                    size = getFlagBounds( title_overlap );
                    if ( Utility.isInRect( locx, locy, px_overlap.value, OFFSET_OVL - FLAG_SPACE, size.width, size.height + FLAG_SPACE * 2 ) ) {
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
            ByRef<Integer> px_pre_utteramce, ByRef<Integer> px_overlap )
        {
            ByRef<Double> sec_env_start1 = new ByRef<Double>( 0.0 );
            ByRef<Double> sec_env_end1 = new ByRef<Double>( 0.0 );
            getEnvelopeRegion( tempo_table, prev_item, item, next_item, sec_env_start1, sec_env_end1 );
            
            UstEvent ust_event1 = item.UstEvent;
            if ( ust_event1 == null ) {
                ust_event1 = new UstEvent();
            }
            UstEnvelope draw_target = ust_event1.getEnvelope();
            if ( draw_target == null ) {
                draw_target = new UstEnvelope();
            }
            double sec_pre_utterance1 = ust_event1.getPreUtterance() / 1000.0;
            double sec_overlap1 = ust_event1.getVoiceOverlap() / 1000.0;

            TempoVectorSearchContext context = new TempoVectorSearchContext();
            int px_env_start1 = 
                AppManager.xCoordFromClocks( 
                    (int)tempo_table.getClockFromSec( sec_env_start1.value, context ) );
            if ( px_pre_utteramce != null ) {
                px_pre_utteramce.value = px_env_start1;
            }
            double sec_p1 = sec_env_start1.value + draw_target.p1 / 1000.0;
            double sec_p2 = sec_env_start1.value + (draw_target.p1 + draw_target.p2) / 1000.0;
            double sec_p5 = sec_env_start1.value +(draw_target.p1 + draw_target.p2 + draw_target.p5) / 1000.0;
            double sec_p3 = sec_env_end1.value - (draw_target.p3 + draw_target.p4) / 1000.0;
            double sec_p4 = sec_env_end1.value - draw_target.p4 / 1000.0;
            int p1 = AppManager.xCoordFromClocks( (int)tempo_table.getClockFromSec( sec_p1, context ) );
            int p2 = AppManager.xCoordFromClocks( (int)tempo_table.getClockFromSec( sec_p2, context ) );
            int p5 = AppManager.xCoordFromClocks( (int)tempo_table.getClockFromSec( sec_p5, context ) );
            int p3 = AppManager.xCoordFromClocks( (int)tempo_table.getClockFromSec( sec_p3, context ) );
            int p4 = AppManager.xCoordFromClocks( (int)tempo_table.getClockFromSec( sec_p4, context ) );
            int px_env_end1 = AppManager.xCoordFromClocks( (int)tempo_table.getClockFromSec( sec_env_end1.value, context ) );
            if ( px_overlap != null ) {
                px_overlap.value = 
                    AppManager.xCoordFromClocks(
                        (int)tempo_table.getClockFromSec( sec_env_start1.value + sec_overlap1, context ) );
            }
            int v1 = yCoordFromValue( draw_target.v1 );
            int v2 = yCoordFromValue( draw_target.v2 );
            int v3 = yCoordFromValue( draw_target.v3 );
            int v4 = yCoordFromValue( draw_target.v4 );
            int v5 = yCoordFromValue( draw_target.v5 );
            int y = yCoordFromValue( 0 );
            return new Polygon( new int[] { px_env_start1, p1, p2, p5, p3, p4, px_env_end1 },
                                new int[] { y, v1, v2, v5, v3, v4, y },
                                7 );
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
            ByRef<Double> env_start_sec, ByRef<Double> env_end_sec )
        {
            double sec_start1 = tempo_table.getSecFromClock( item.Clock );
            double sec_end1 = tempo_table.getSecFromClock( item.Clock + item.ID.getLength() );
            UstEvent ust_event1 = item.UstEvent;
            if ( ust_event1 == null ) {
                ust_event1 = new UstEvent();
            }
            UstEnvelope draw_target = ust_event1.getEnvelope();
            if ( draw_target == null ) {
                draw_target = new UstEnvelope();
            }
            double sec_pre_utterance1 = ust_event1.getPreUtterance() / 1000.0;
            double sec_overlap1 = ust_event1.getVoiceOverlap() / 1000.0;

            // 先行発音があることによる，この音符のエンベロープの実際の開始位置
            double sec_env_start1 = sec_start1 - sec_pre_utterance1;

            // 直後の音符の有る無しで，この音符のエンベロープの実際の終了位置が変わる
            double sec_env_end1 = sec_end1;
            if ( item_next != null && item_next.UstEvent != null ) {
                // 直後に音符がある場合
                UstEvent ust_event2 = item_next.UstEvent;
                double sec_pre_utterance2 = ust_event2.getPreUtterance() / 1000.0;
                double sec_overlap2 = ust_event2.getVoiceOverlap() / 1000.0;
                sec_env_end1 = sec_end1 - sec_pre_utterance2 + sec_overlap2;
            }

            env_start_sec.value = sec_env_start1;
            env_end_sec.value = sec_env_end1;
        }

        private void drawTrackTab( Graphics2D g, Rectangle destRect, String name, boolean selected, boolean enabled, boolean render_required, Color hilight, Color render_button_hilight )
        {
            int x = destRect.x;
            int panel_width = render_required ? destRect.width - 10 : destRect.width;
            Color panel_color = enabled ? hilight : new Color( 125, 123, 124 );
            Color button_color = enabled ? render_button_hilight : new Color( 125, 123, 124 );
            Color panel_title = Color.black;
            Color button_title = selected ? Color.white : Color.black;
            Color border = selected ? Color.white : new Color( 118, 123, 138 );

            // 背景(選択されている場合)
            if ( selected ) {
                g.setColor( panel_color );
                g.fillRect( destRect.x, destRect.y, destRect.width, destRect.height );
                if ( render_required && enabled ) {
                    g.setColor( render_button_hilight );
                    g.fillRect( destRect.x + destRect.width - 10, destRect.y, 10, destRect.height );
                }
            }

            // 左縦線
            g.setColor( border );
            g.drawLine( destRect.x, destRect.y,
                        destRect.x, destRect.y + destRect.height - 1 );
            if ( PortUtil.getStringLength( name ) > 0 ) {
                // 上横線
                g.setColor( border );
                g.drawLine( destRect.x + 1, destRect.y,
                            destRect.x + destRect.width, destRect.y );
            }
            if ( render_required ) {
                g.setColor( border );
            g.drawLine( destRect.x + destRect.width - 10, destRect.y,
                        destRect.x + destRect.width - 10, destRect.y + destRect.height - 1 );
            }
            g.clipRect( destRect.x, destRect.y, destRect.width, destRect.height );
            String title = Utility.trimString( name, AppManager.baseFont8, panel_width );
            g.setFont( AppManager.baseFont8 );
            g.setColor( panel_title );
            g.drawString( title, destRect.x + 2, destRect.y + destRect.height / 2 - AppManager.baseFont8OffsetHeight );
            if ( render_required ) {
                g.setColor( button_title );
                g.drawString( "R", destRect.x + destRect.width - PX_WIDTH_RENDER, destRect.y + destRect.height / 2 - AppManager.baseFont8OffsetHeight );
            }
            if ( selected ) {
                g.setColor( border );
                g.drawLine( destRect.x + destRect.width - 1, destRect.y,
                            destRect.x + destRect.width - 1, destRect.y + destRect.height - 1 );
                g.setColor( border );
                g.drawLine( destRect.x, destRect.y + destRect.height - 1,
                            destRect.x + destRect.width, destRect.y + destRect.height - 1 );
            }
            g.setClip( null );
            g.setColor( AppManager.COLOR_BORDER );
            g.drawLine( destRect.x + destRect.width, destRect.y,
                        destRect.x + destRect.width, destRect.y + destRect.height - 1 );
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
            if ( vsq != null ) {
                numTrack = vsq.Track.size();
            }
            if ( draft * (numTrack - 1) <= maxTotalWidth ) {
                return draft;
            } else {
                return (int)((maxTotalWidth) / (numTrack - 1.0f));
            }
        }

        /// <summary>
        /// ベロシティを、与えられたグラフィックgを用いて描画します
        /// </summary>
        /// <param name="g"></param>
        /// <param name="track"></param>
        /// <param name="color"></param>
        /// <param name="is_front"></param>
        /// <param name="type"></param>
        public void drawVEL( Graphics2D g, VsqTrack track, Color color, boolean is_front, CurveType type )
        {
            Point mouse = pointToClient( PortUtil.getMousePosition() );

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
            int stdx = AppManager.getStartToDrawX();
            int key_width = AppManager.keyWidth;
            int xoffset = key_width - stdx;
            g.clipRect( key_width, HEADER, width - key_width, graph_height );
            float scale = AppManager.getScaleX();
            int selected = AppManager.getSelected();

            g.setFont( AppManager.baseFont10Bold );
            boolean cursor_should_be_hand = false;
            lock ( AppManager.mDrawObjects ) {
                Vector<DrawObject> target_list = AppManager.mDrawObjects.get( selected - 1 );
                int count = target_list.size();
                int i_start = AppManager.mDrawStartIndex[selected - 1];
                for ( int i = i_start; i < count; i++ ) {
                    DrawObject dobj = target_list.get( i );
                    if ( dobj.mType != DrawObjectType.Note ) {
                        continue;
                    }
                    int x = dobj.mRectangleInPixel.x + xoffset;
                    if ( x + VEL_BAR_WIDTH < 0 ) {
                        continue;
                    } else if ( width < x ) {
                        break;
                    } else {
                        int value = 0;
                        if ( type.equals( CurveType.VEL ) ) {
                            if ( AppManager.mDrawIsUtau[selected - 1] ) {
                                value = dobj.mIntensity;
                                max = UstEvent.MAX_INTENSITY;
                                min = UstEvent.MIN_INTENSITY;
                            } else {
                                value = dobj.mVelocity;
                                max = 127;
                                min = 0;
                            }
                        } else if ( type.equals( CurveType.Accent ) ) {
                            value = dobj.mAccent;
                            max = 100;
                            min = 0;
                        } else if ( type.equals( CurveType.Decay ) ) {
                            value = dobj.mDecay;
                            max = 100;
                            min = 0;
                        }
                        //float order = (type.equals( CurveType.VEL )) ? graph_height / 127f : graph_height / 100f;

                        int y = oy - graph_height * (value - min) / (max - min);
                        if ( is_front && AppManager.itemSelection.isEventContains( selected, dobj.mInternalID ) ) {
                            g.setColor( COLOR_A127R008G166B172 );
                            g.fillRect( x, y, VEL_BAR_WIDTH, oy - y );
                            if ( mMouseDownMode == MouseDownMode.VEL_EDIT ) {
                                int editing = 0;
                                if ( mVelEditSelected.containsKey( dobj.mInternalID ) ) {
                                    VsqEvent ve_editing = mVelEditSelected.get( dobj.mInternalID ).editing;
                                    if ( mSelectedCurve.equals( CurveType.VEL ) ) {
                                        if ( AppManager.mDrawIsUtau[selected - 1] ) {
                                            editing = ve_editing.UstEvent == null ? 100 : ve_editing.UstEvent.getIntensity();
                                        } else {
                                            editing = ve_editing.ID.Dynamics;
                                        }
                                    } else if ( mSelectedCurve.equals( CurveType.Accent ) ) {
                                        editing = ve_editing.ID.DEMaccent;
                                    } else if ( mSelectedCurve.equals( CurveType.Decay ) ) {
                                        editing = ve_editing.ID.DEMdecGainRate;
                                    }
                                    int edit_y = oy - graph_height * (editing - min) / (max - min);
                                    g.setColor( COLOR_A244R255G023B012 );
                                    g.fillRect( x, edit_y, VEL_BAR_WIDTH, oy - edit_y );
                                    g.setColor( Color.white );
                                    g.drawString( editing + "", x + VEL_BAR_WIDTH, (edit_y > oy - 20) ? oy - 20 : edit_y );
                                }
                            }
                        } else {
                            g.setColor( color );
                            g.fillRect( x, y, VEL_BAR_WIDTH, oy - y );
                        }
                        if ( mMouseDownMode == MouseDownMode.VEL_EDIT ) {
                            cursor_should_be_hand = true;
                        } else {
                            if ( AppManager.getSelectedTool() == EditTool.ARROW && is_front && isInRect( mouse.x, mouse.y, new Rectangle( x, y, VEL_BAR_WIDTH, oy - y ) ) ) {
                                cursor_should_be_hand = true;
                            }
                        }
                    }
                }
            }
            if ( cursor_should_be_hand ) {
                if ( getCursor().getType() != java.awt.Cursor.HAND_CURSOR ) {
                    setCursor( new Cursor( java.awt.Cursor.HAND_CURSOR ) );
                }
            } else {
                if ( getCursor().getType() != java.awt.Cursor.DEFAULT_CURSOR ) {
                    setCursor( new Cursor( java.awt.Cursor.DEFAULT_CURSOR ) );
                }
            }
            g.setClip( last_clip );
        }

        /// <summary>
        /// ベジエ曲線によるコントロールカーブを描画します
        /// </summary>
        /// <param name="g"></param>
        /// <param name="chains"></param>
        private void drawAttachedCurve( Graphics2D g, Vector<BezierChain> chains )
        {
#if DEBUG
            try {
#endif
                Color hilight = Color.blue;// AppManager.getHilightColor();
                int chains_count = chains.size();
                for ( int i = 0; i < chains_count; i++ ) {
                    BezierChain target_chain = chains.get( i );
                    int chain_id = target_chain.id;
                    if ( target_chain.points.size() <= 0 ) {
                        continue;
                    }
                    BezierPoint next;
                    BezierPoint current = target_chain.points.get( 0 );
                    Point pxNext;
                    Point pxCurrent = getScreenCoord( current.getBase() );
                    int target_chain_points_count = target_chain.points.size();
                    for ( int j = 0; j < target_chain_points_count; j++ ) {
                        next = target_chain.points.get( j );
                        int next_x = AppManager.xCoordFromClocks( (int)next.getBase().getX() );
                        pxNext = new Point( next_x, yCoordFromValue( (int)next.getBase().getY() ) );
                        Point pxControlCurrent = getScreenCoord( current.getControlRight() );
                        Point pxControlNext = getScreenCoord( next.getControlLeft() );

                        // ベジエ曲線本体を描く
                        //g.SmoothingMode = SmoothingMode.AntiAlias;
                        if ( current.getControlRightType() == BezierControlType.None &&
                             next.getControlLeftType() == BezierControlType.None ) {
                            g.setColor( COLOR_BEZIER_CURVE );
                            g.drawLine( pxCurrent.x, pxCurrent.y, pxNext.x, pxNext.y );
                        } else {
                            Point ctrl1 = (current.getControlRightType() == BezierControlType.None) ? pxCurrent : pxControlCurrent;
                            Point ctrl2 = (next.getControlLeftType() == BezierControlType.None) ? pxNext : pxControlNext;
                            g.setColor( COLOR_BEZIER_CURVE );
                            PortUtil.drawBezier( g, pxCurrent.x, pxCurrent.y,
                                                    ctrl1.x, ctrl1.y,
                                                    ctrl2.x, ctrl2.y,
                                                    pxNext.x, pxNext.y );
                        }

                        if ( current.getControlRightType() != BezierControlType.None ) {
                            g.setColor( COLOR_BEZIER_AUXILIARY );
                            g.drawLine( pxCurrent.x, pxCurrent.y, pxControlCurrent.x, pxControlCurrent.y );
                        }
                        if ( next.getControlLeftType() != BezierControlType.None ) {
                            g.setColor( COLOR_BEZIER_AUXILIARY );
                            g.drawLine( pxNext.x, pxNext.y, pxControlNext.x, pxControlNext.y );
                        }
                        //g.SmoothingMode = SmoothingMode.Default;

                        // 右コントロール点
                        if ( current.getControlRightType() == BezierControlType.Normal ) {
                            Rectangle rc = new Rectangle( pxControlCurrent.x - DOT_WID,
                                                          pxControlCurrent.y - DOT_WID,
                                                          DOT_WID * 2 + 1,
                                                          DOT_WID * 2 + 1 );
                            if ( chain_id == mEditingChainID && current.getID() == mEditingPointID ) {
                                g.setColor( hilight );
                                g.fillOval( rc.x, rc.y, rc.width, rc.height );
                            } else {
                                g.setColor( COLOR_BEZIER_DOT_NORMAL );
                                g.fillOval( rc.x, rc.y, rc.width, rc.height );
                            }
                            g.setColor( COLOR_BEZIER_DOT_NORMAL_DARK );
                            g.drawOval( rc.x, rc.y, rc.width, rc.height );
                        }

                        // 左コントロール点
                        if ( next.getControlLeftType() == BezierControlType.Normal ) {
                            Rectangle rc = new Rectangle( pxControlNext.x - DOT_WID,
                                                          pxControlNext.y - DOT_WID,
                                                          DOT_WID * 2 + 1,
                                                          DOT_WID * 2 + 1 );
                            if ( chain_id == mEditingChainID && next.getID() == mEditingPointID ) {
                                g.setColor( hilight );
                                g.fillOval( rc.x, rc.y, rc.width, rc.height );
                            } else {
                                g.setColor( COLOR_BEZIER_DOT_NORMAL );
                                g.fillOval( rc.x, rc.y, rc.width, rc.height );
                            }
                            g.setColor( COLOR_BEZIER_DOT_NORMAL_DARK );
                            g.drawOval( rc.x, rc.y, rc.width, rc.height );
                        }

                        // データ点
                        Rectangle rc2 = new Rectangle( pxCurrent.x - DOT_WID,
                                                        pxCurrent.y - DOT_WID,
                                                        DOT_WID * 2 + 1,
                                                        DOT_WID * 2 + 1 );
                        if ( chain_id == mEditingChainID && current.getID() == mEditingPointID ) {
                            g.setColor( hilight );
                            g.fillRect( rc2.x, rc2.y, rc2.width, rc2.height );
                        } else {
                            g.setColor( COLOR_BEZIER_DOT_BASE );
                            g.fillRect( rc2.x, rc2.y, rc2.width, rc2.height );
                        }
                        g.setColor( COLOR_BEZIER_DOT_BASE_DARK );
                        g.drawRect( rc2.x, rc2.y, rc2.width, rc2.height );
                        pxCurrent = pxNext;
                        current = next;
                    }
                    next = target_chain.points.get( target_chain.points.size() - 1 );
                    pxNext = getScreenCoord( next.getBase() );
                    Rectangle rc_last = new Rectangle( pxNext.x - DOT_WID,
                                                       pxNext.y - DOT_WID,
                                                       DOT_WID * 2 + 1,
                                                       DOT_WID * 2 + 1 );
                    if ( chain_id == mEditingChainID && next.getID() == mEditingPointID ) {
                        g.setColor( hilight );
                        g.fillRect( rc_last.x, rc_last.y, rc_last.width, rc_last.height );
                    } else {
                        g.setColor( COLOR_BEZIER_DOT_BASE );
                        g.fillRect( rc_last.x, rc_last.y, rc_last.width, rc_last.height );
                    }
                    g.setColor( COLOR_BEZIER_DOT_BASE_DARK );
                    g.drawRect( rc_last.x, rc_last.y, rc_last.width, rc_last.height );
                }
#if DEBUG
            } catch ( Exception ex ) {
                AppManager.debugWriteLine( "TrackSelector+DrawAttatchedCurve" );
                AppManager.debugWriteLine( "    ex=" + ex );
            }
#endif
        }

        private Point getScreenCoord( PointD pt )
        {
            return new Point( AppManager.xCoordFromClocks( (int)pt.getX() ), yCoordFromValue( (int)pt.getY() ) );
        }

        /// <summary>
        /// ビブラートのRate, Depthカーブを描画します
        /// </summary>
        /// <param name="g">描画に使用するグラフィックス</param>
        /// <param name="draw_target">描画対象のトラック</param>
        /// <param name="type">描画するカーブの種類</param>
        /// <param name="color">塗りつぶしに使う色</param>
        /// <param name="is_front">最前面に表示するモードかどうか</param>
        public void drawVibratoControlCurve( Graphics2D g, VsqTrack draw_target, CurveType type, Color color, boolean is_front )
        {
            if ( !is_front ) {
                return;
            }
            Shape last_clip = g.getClip();
            int graph_height = getGraphHeight();
            int key_width = AppManager.keyWidth;
            int width = getWidth();
            int height = getHeight();
            g.clipRect( key_width, HEADER,
                        width - key_width, graph_height );

            int cl_start = AppManager.clockFromXCoord( key_width );
            int cl_end = AppManager.clockFromXCoord( width );
            int max = type.getMaximum();
            int min = type.getMinimum();

            int oy = height - 42;
            float order = graph_height / (float)(max - min);

            // カーブを描く
            int last_shadow_x = key_width;
            for ( Iterator<VsqEvent> itr = draw_target.getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent ve = itr.next();
                int start = ve.Clock + ve.ID.VibratoDelay;
                int end = ve.Clock + ve.ID.getLength();
                if ( end < cl_start ) {
                    continue;
                }
                if ( cl_end < start ) {
                    break;
                }
                if ( ve.ID.VibratoHandle == null ) {
                    continue;
                }
                VibratoHandle handle = ve.ID.VibratoHandle;
                if ( handle == null ) {
                    continue;
                }
                int x1 = AppManager.xCoordFromClocks( start );

                // 左側の影付にする部分を描画
                g.setColor( COLOR_VIBRATO_SHADOW );
                g.fillRect( last_shadow_x, HEADER, x1 - last_shadow_x, graph_height );
                int x2 = AppManager.xCoordFromClocks( end );
                last_shadow_x = x2;

                if ( x1 < x2 ) {
                    // 描画器を取得、初期化
                    LineGraphDrawer d = getGraphDrawer();
                    d.clear();
                    d.setGraphics( g );
                    d.setBaseLineY( oy );
                    d.setFillColor( color );
                    d.setFill( true );
                    d.setDotMode( LineGraphDrawer.DOTMODE_NO );
                    d.setDrawLine( false );
                    d.setLineColor( Color.white );
                    d.setDrawLine( true );

                    int draw_width = x2 - x1;
                    int start_value = 0;

                    // typeに応じてカーブを取得
                    VibratoBPList list = null;
                    if ( type.equals( CurveType.VibratoRate ) ) {
                        start_value = handle.getStartRate();
                        list = handle.getRateBP();
                    } else if ( type.equals( CurveType.VibratoDepth ) ) {
                        start_value = handle.getStartDepth();
                        list = handle.getDepthBP();
                    }
                    if ( list == null ) {
                        continue;
                    }

                    // 描画
                    int last_y = oy - (int)((start_value - min) * order);
                    d.append( x1, last_y );
                    int c = list.getCount();
                    for ( int i = 0; i < c; i++ ) {
                        VibratoBPPair item = list.getElement( i );
                        int x = x1 + (int)(item.X * draw_width);
                        int y = oy - (int)((item.Y - min) * order);
                        d.append( x, y );
                        last_y = y;
                    }
                    d.append( x2, last_y );

                    d.flush();
                }
            }

            // 右側の影付にする部分を描画
            g.setColor( COLOR_VIBRATO_SHADOW );
            g.fillRect( last_shadow_x, HEADER, width - key_width, graph_height );

            g.setClip( last_clip );
        }

        /// <summary>
        /// BPList(コントロールカーブ)を指定したグラフィックスを用いて描画します
        /// </summary>
        /// <param name="g">描画に使用するグラフィックス</param>
        /// <param name="list">描画するコントロールカーブ</param>
        /// <param name="color">X軸とデータ線の間の塗りつぶしに使用する色</param>
        /// <param name="is_front">最前面に表示するモードかどうか</param>
        public void drawVsqBPList( Graphics2D g, VsqBPList list, Color color, boolean is_front )
        {
            Point pmouse = pointToClient( PortUtil.getMousePosition() );
            int max = list.getMaximum();
            int min = list.getMinimum();
            int graph_height = getGraphHeight();
            int width = getWidth();
            int height = getHeight();
            float order = graph_height / (float)(max - min);
            int oy = height - 42;
            int key_width = AppManager.keyWidth;

            int start = key_width;
            int start_clock = AppManager.clockFromXCoord( start );
            int end = width;
            int end_clock = AppManager.clockFromXCoord( end );

            // グラフ描画器の取得と設定
            LineGraphDrawer d = getGraphDrawer();
            d.clear();
            d.setGraphics( g );
            d.setBaseLineY( oy );
            d.setDotSize( DOT_WID );
            d.setFillColor( color );
            d.setDotColor( Color.white );
            d.setLineColor( Color.white );
            int dot_mode = is_front ? LineGraphDrawer.DOTMODE_NEAR : LineGraphDrawer.DOTMODE_NO;
            if ( pmouse.y < 0 || height < pmouse.y ) {
                dot_mode = LineGraphDrawer.DOTMODE_NO;
            }
            d.setDotMode( dot_mode );
            d.setDrawLine( is_front );
            d.setMouseX( pmouse.x );

            // グラフの描画
            int first_y = list.getValue( start_clock );
            int last_y = oy - (int)((first_y - min) * order);
            d.append( 0, last_y );

            int c = list.size();
            if ( c > 0 ) {
                int first_clock = list.getKeyClock( 0 );
                int last_x = AppManager.xCoordFromClocks( first_clock );
                first_y = list.getValue( first_clock );
                last_y = oy - (int)((first_y - min) * order);

                for ( int i = 0; i < c; i++ ) {
                    int clock = list.getKeyClock( i );
                    if ( clock < start_clock ) {
                        continue;
                    }
                    if ( end_clock < clock ) {
                        break;
                    }
                    int x = AppManager.xCoordFromClocks( clock );
                    VsqBPPair v = list.getElementB( i );
                    int y = oy - (int)((v.value - min) * order);
                    d.append( x, y );
                    last_y = y;
                }
            }

            d.append( width + DOT_WID + DOT_WID, last_y );
            d.flush();

            // 最前面のBPListの場合
            if ( !is_front ) {
                // 最前面じゃなかったら帰る
                return;
            }
            // 選択されているデータ点をハイライト表示する
            int w = DOT_WID * 2 + 1;
            g.setColor( COLOR_DOT_HILIGHT );
            for ( Iterator<Long> itr = AppManager.itemSelection.getPointIDIterator(); itr.hasNext(); ) {
                long id = itr.next();
                VsqBPPairSearchContext ret = list.findElement( id );
                if ( ret.index < 0 ) {
                    continue;
                }
                int clock = ret.clock;
                int value = ret.point.value;

                int x = AppManager.xCoordFromClocks( clock );
                if ( x < key_width ) {
                    continue;
                } else if ( width < x ) {
                    break;
                }
                int y = oy - (int)((value - min) * order);
                g.fillRect( x - DOT_WID, y - DOT_WID, w, w );
            }

            // 移動中のデータ点をハイライト表示する
            if ( mMouseDownMode == MouseDownMode.POINT_MOVE ) {
                int dx = pmouse.x + AppManager.getStartToDrawX() - mMouseDownLocation.x;
                int dy = pmouse.y - mMouseDownLocation.y;
                for ( Iterator<BPPair> itr = mMovingPoints.iterator(); itr.hasNext(); ) {
                    BPPair item = itr.next();
                    int x = AppManager.xCoordFromClocks( item.Clock ) + dx;
                    int y = yCoordFromValue( item.Value ) + dy;
                    g.setColor( COLOR_DOT_HILIGHT );
                    g.fillRect( x - DOT_WID, y - DOT_WID, w, w );
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

#if !JAVA
        public void TrackSelector_Load( Object sender, BEventArgs e )
        {
            this.SetStyle( System.Windows.Forms.ControlStyles.DoubleBuffer, true );
            this.SetStyle( System.Windows.Forms.ControlStyles.UserPaint, true );
            this.SetStyle( System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true );
        }
#endif

        public void TrackSelector_MouseClick( Object sender, BMouseEventArgs e )
        {
            if ( mCurveVisible ) {
                if ( e.Button == BMouseButtons.Left ) {
                    // カーブの種類一覧上で発生したイベントかどうかを検査
                    for ( int i = 0; i < getViewingCurveCount(); i++ ) {
                        CurveType curve = getViewingCurveElement( i );
                        Rectangle r = getRectFromCurveType( curve );
                        if ( isInRect( e.X, e.Y, r ) ) {
                            changeCurve( curve );
                            return;
                        }
                    }
                } else if ( e.Button == BMouseButtons.Right ) {
                    if ( 0 <= e.X && e.X <= AppManager.keyWidth &&
                         0 <= e.Y && e.Y <= getHeight() - 2 * OFFSET_TRACK_TAB ) {
                        MenuElement[] sub_cmenu_curve = cmenuCurve.getSubElements();
                        for ( int i = 0; i < sub_cmenu_curve.Length; i++ ) {
                            MenuElement tsi = sub_cmenu_curve[i];
                            if ( tsi is BMenuItem ) {
                                BMenuItem tsmi = (BMenuItem)tsi;
                                tsmi.setSelected( false );
                                MenuElement[] sub_tsmi = tsmi.getSubElements();
                                for ( int j = 0; j < sub_tsmi.Length; j++ ) {
                                    MenuElement tsi2 = sub_tsmi[j];
                                    if ( tsi2 is BMenuItem ) {
                                        BMenuItem tsmi2 = (BMenuItem)tsi2;
                                        tsmi2.setSelected( false );
                                    }
                                }
                            }
                        }
                        RendererKind kind = VsqFileEx.getTrackRendererKind( AppManager.getVsqFile().Track.get( AppManager.getSelected() ) );
                        if ( kind == RendererKind.VOCALOID1 ) {
                            cmenuCurveVelocity.setVisible( true );
                            cmenuCurveAccent.setVisible( true );
                            cmenuCurveDecay.setVisible( true );

                            cmenuCurveSeparator1.setVisible( true );
                            cmenuCurveDynamics.setVisible( true );
                            cmenuCurveVibratoRate.setVisible( true );
                            cmenuCurveVibratoDepth.setVisible( true );

                            cmenuCurveSeparator2.setVisible( true );
                            cmenuCurveReso1.setVisible( true );
                            cmenuCurveReso2.setVisible( true );
                            cmenuCurveReso3.setVisible( true );
                            cmenuCurveReso4.setVisible( true );

                            cmenuCurveSeparator3.setVisible( true );
                            cmenuCurveHarmonics.setVisible( true );
                            cmenuCurveBreathiness.setVisible( true );
                            cmenuCurveBrightness.setVisible( true );
                            cmenuCurveClearness.setVisible( true );
                            cmenuCurveOpening.setVisible( false );
                            cmenuCurveGenderFactor.setVisible( true );

                            cmenuCurveSeparator4.setVisible( true );
                            cmenuCurvePortamentoTiming.setVisible( true );
                            cmenuCurvePitchBend.setVisible( true );
                            cmenuCurvePitchBendSensitivity.setVisible( true );

                            cmenuCurveSeparator5.setVisible( true );
                            cmenuCurveEffect2Depth.setVisible( true );
                            cmenuCurveEnvelope.setVisible( false );

                            cmenuCurveBreathiness.setText( "Noise" );
                            cmenuCurveVelocity.setText( "Velocity" );
                        } else if ( kind == RendererKind.UTAU || kind == RendererKind.VCNT ) {
                            cmenuCurveVelocity.setVisible( (kind == RendererKind.UTAU) );
                            cmenuCurveAccent.setVisible( false );
                            cmenuCurveDecay.setVisible( false );

                            cmenuCurveSeparator1.setVisible( false );
                            cmenuCurveDynamics.setVisible( false );
                            cmenuCurveVibratoRate.setVisible( true );
                            cmenuCurveVibratoDepth.setVisible( true );

                            cmenuCurveSeparator2.setVisible( false );
                            cmenuCurveReso1.setVisible( false );
                            cmenuCurveReso2.setVisible( false );
                            cmenuCurveReso3.setVisible( false );
                            cmenuCurveReso4.setVisible( false );

                            cmenuCurveSeparator3.setVisible( false );
                            cmenuCurveHarmonics.setVisible( false );
                            cmenuCurveBreathiness.setVisible( false );
                            cmenuCurveBrightness.setVisible( false );
                            cmenuCurveClearness.setVisible( false );
                            cmenuCurveOpening.setVisible( false );
                            cmenuCurveGenderFactor.setVisible( false );

                            cmenuCurveSeparator4.setVisible( true );
                            cmenuCurvePortamentoTiming.setVisible( false );
                            cmenuCurvePitchBend.setVisible( true );
                            cmenuCurvePitchBendSensitivity.setVisible( true );

                            cmenuCurveSeparator5.setVisible( true );
                            cmenuCurveEffect2Depth.setVisible( false );
                            cmenuCurveEnvelope.setVisible( true );

                            if ( kind == RendererKind.UTAU ) {
                                cmenuCurveVelocity.setText( "Intensity" );
                            }
                        } else {
                            cmenuCurveVelocity.setVisible( true );
                            cmenuCurveAccent.setVisible( true );
                            cmenuCurveDecay.setVisible( true );

                            cmenuCurveSeparator1.setVisible( true );
                            cmenuCurveDynamics.setVisible( true );
                            cmenuCurveVibratoRate.setVisible( true );
                            cmenuCurveVibratoDepth.setVisible( true );

                            cmenuCurveSeparator2.setVisible( false );
                            cmenuCurveReso1.setVisible( false );
                            cmenuCurveReso2.setVisible( false );
                            cmenuCurveReso3.setVisible( false );
                            cmenuCurveReso4.setVisible( false );

                            cmenuCurveSeparator3.setVisible( true );
                            cmenuCurveHarmonics.setVisible( false );
                            cmenuCurveBreathiness.setVisible( true );
                            cmenuCurveBrightness.setVisible( true );
                            cmenuCurveClearness.setVisible( true );
                            cmenuCurveOpening.setVisible( true );
                            cmenuCurveGenderFactor.setVisible( true );

                            cmenuCurveSeparator4.setVisible( true );
                            cmenuCurvePortamentoTiming.setVisible( true );
                            cmenuCurvePitchBend.setVisible( true );
                            cmenuCurvePitchBendSensitivity.setVisible( true );

                            cmenuCurveSeparator5.setVisible( false );
                            cmenuCurveEffect2Depth.setVisible( false );
                            cmenuCurveEnvelope.setVisible( false );

                            cmenuCurveBreathiness.setText( "Breathiness" );
                            cmenuCurveVelocity.setText( "Velocity" );
                        }
                        for ( int i = 0; i < sub_cmenu_curve.Length; i++ ) {
                            MenuElement tsi = sub_cmenu_curve[i];
                            if ( tsi is BMenuItem ) {
                                BMenuItem tsmi = (BMenuItem)tsi;
                                CurveType ct = getCurveTypeFromMenu( tsmi );
                                if ( ct.equals( mSelectedCurve ) ) {
                                    tsmi.setSelected( true );
                                    break;
                                }
                                MenuElement[] sub_tsmi = tsmi.getSubElements();
                                for ( int j = 0; j < sub_tsmi.Length; j++ ) {
                                    MenuElement tsi2 = sub_tsmi[j];
                                    if ( tsi2 is BMenuItem ) {
                                        BMenuItem tsmi2 = (BMenuItem)tsi2;
                                        CurveType ct2 = getCurveTypeFromMenu( tsmi2 );
                                        if ( ct2.equals( mSelectedCurve ) ) {
                                            tsmi2.setSelected( true );
                                            if ( ct2.equals( CurveType.reso1amp ) || ct2.equals( CurveType.reso1bw ) || ct2.equals( CurveType.reso1freq ) ||
                                                 ct2.equals( CurveType.reso2amp ) || ct2.equals( CurveType.reso2bw ) || ct2.equals( CurveType.reso2freq ) ||
                                                 ct2.equals( CurveType.reso3amp ) || ct2.equals( CurveType.reso3bw ) || ct2.equals( CurveType.reso3freq ) ||
                                                 ct2.equals( CurveType.reso4amp ) || ct2.equals( CurveType.reso4bw ) || ct2.equals( CurveType.reso4freq ) ) {
                                                tsmi.setSelected( true );//親アイテムもチェック。Resonance*用
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        cmenuCurve.show( this, e.X, e.Y );
                    }
                }
            }
        }

        public void SelectNextCurve()
        {
            int index = 0;
            if ( getViewingCurveCount() >= 2 ) {
                for ( int i = 0; i < getViewingCurveCount(); i++ ) {
                    if ( getViewingCurveElement( i ).equals( mSelectedCurve ) ) {
                        index = i;
                        break;
                    }
                }
                index++;
                if ( getViewingCurveCount() <= index ) {
                    index = 0;
                }
                changeCurve( getViewingCurveElement( index ) );
            }
        }

        public void SelectPreviousCurve()
        {
            int index = 0;
            if ( getViewingCurveCount() >= 2 ) {
                for ( int i = 0; i < getViewingCurveCount(); i++ ) {
                    if ( getViewingCurveElement( i ).equals( mSelectedCurve ) ) {
                        index = i;
                        break;
                    }
                }
                index--;
                if ( index < 0 ) {
                    index = getViewingCurveCount() - 1;
                }
                changeCurve( getViewingCurveElement( index ) );
            }
        }

        public BezierPoint HandleMouseMoveForBezierMove( int clock, int value, int value_raw, BezierPickedSide picked )
        {
            BezierChain target = AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).getBezierChain( mSelectedCurve, AppManager.itemSelection.getLastBezier().chainID );
            int point_id = AppManager.itemSelection.getLastBezier().pointID;
            int index = -1;
            for ( int i = 0; i < target.points.size(); i++ ) {
                if ( target.points.get( i ).getID() == point_id ) {
                    index = i;
                    break;
                }
            }
            float scale_x = AppManager.getScaleX();
            float scale_y = getScaleY();
            BezierPoint ret = new BezierPoint( 0, 0 );
            if ( index >= 0 ) {
                BezierPoint item = target.points.get( index );
                if ( picked == BezierPickedSide.BASE ) {
                    // データ点を動かす
                    Point old = target.points.get( index ).getBase().toPoint();
                    item.setBase( new PointD( clock, value ) );
                    if ( !BezierChain.isBezierImplicit( target ) ) {
                        // X軸について陰でなくなった場合
                        // データ点のX座標だけ元に戻し，もう一度チェックを試みる
                        item.setBase( new PointD( old.x, value ) );
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            // 駄目ならX, Y両方元に戻す
                            item.setBase( new PointD( old.x, old.y ) );
                        }
                    }
                    ret = (BezierPoint)target.points.get( index ).clone();
                } else if ( picked == BezierPickedSide.LEFT ) {
                    // 左制御点を動かす
                    if ( item.getControlLeftType() == BezierControlType.Master ) {
                        // 右制御点を同時に動かさない場合
                        PointD old_left = new PointD( item.getControlLeft() );
                        item.setControlLeft( new PointD( clock, value_raw ) );
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            // X軸について陰でなくなった場合
                            // X座標だけ元に戻し，もう一度チェックを試みる
                            item.setControlLeft( new PointD( old_left.getX(), value_raw ) );
                            if ( !BezierChain.isBezierImplicit( target ) ) {
                                // 駄目ならX, Y両方戻す
                                item.setControlLeft( old_left );
                            }
                        }
                    } else {
                        // 右制御点を同時に動かす場合(デフォルト)
                        PointD old_left = new PointD( item.getControlLeft() );
                        PointD old_right = new PointD( item.getControlRight() );
                        PointD old_base = new PointD( item.getBase() );

                        // 新しい座標値を計算，設定
                        PointD new_left = new PointD( clock, value_raw );
                        PointD new_right = getCounterPoint( old_base, old_right, new_left, scale_x, scale_y );
                        item.setControlLeft( new_left );
                        item.setControlRight( new_right );

                        // X軸方向に陰かどうかチェック
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            // 駄目なら，Xを元に戻す
                            new_left.setX( old_left.getX() );
                            new_right = getCounterPoint( old_base, old_right, new_left, scale_x, scale_y );
                            item.setControlLeft( new_left );
                            item.setControlRight( new_right );
                            if ( !BezierChain.isBezierImplicit( target ) ) {
                                // それでもだめなら両方戻す
                                item.setControlLeft( old_left );
                                item.setControlRight( old_right );
                            }
                        }
                    }
                    ret = (BezierPoint)item.clone();
                } else if ( picked == BezierPickedSide.RIGHT ) {
                    // 右制御点を動かす
                    if ( item.getControlRightType() == BezierControlType.Master ) {
                        // 左制御点を同時に動かさない場合
                        PointD old_right = item.getControlRight();
                        item.setControlRight( new PointD( clock, value ) );
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            // Xだけ元に戻す
                            item.setControlRight( new PointD( old_right.getX(), value ) );
                            if ( !BezierChain.isBezierImplicit( target ) ) {
                                item.setControlRight( old_right );
                            }
                        }
                    } else {
                        // 左制御点を同時に動かす場合(デフォルト)
                        PointD old_left = new PointD( item.getControlLeft() );
                        PointD old_right = new PointD( item.getControlRight() );
                        PointD old_base = new PointD( item.getBase() );

                        // 新しい座標値を計算，設定
                        PointD new_right = new PointD( clock, value_raw );
                        PointD new_left = getCounterPoint( old_base, old_left, new_right, scale_x, scale_y );
                        item.setControlRight( new_right );
                        item.setControlLeft( new_left );

                        // X軸方向に陰かどうかチェック
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            // 駄目ならXだけ元に戻す
                            new_right.setX( old_right.getX() );
                            new_left = getCounterPoint( old_base, old_left, new_right, scale_x, scale_y );
                            item.setControlRight( new_right );
                            item.setControlLeft( new_left );
                            if ( !BezierChain.isBezierImplicit( target ) ) {
                                item.setControlLeft( old_left );
                                item.setControlRight( old_right );
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
            double scalex, double scaley )
        {
            // 移動後の点と回転軸との為す角を計算
            double theta =
                Math.Atan2(
                    (moving_point.getY() - base_point.getY()) * scaley,
                    (moving_point.getX() - base_point.getX()) * scalex );
            // 直線なので，逆サイドの偏角は+180度
            theta += Math.PI;
            // 逆サイドの点と回転軸との距離を計算
            double dx = (slave_point_original.getX() - base_point.getX()) * scalex;
            double dy = (slave_point_original.getY() - base_point.getY()) * scaley;
            double length = Math.Sqrt( dx * dx + dy * dy );
            // 逆サイドの点の座標を計算
            return new PointD(
                length * Math.Cos( theta ) / scalex + base_point.getX(),
                length * Math.Sin( theta ) / scaley + base_point.getY() );
        }

        public BezierPoint HandleMouseMoveForBezierMove( BMouseEventArgs e, BezierPickedSide picked )
        {
            int clock = AppManager.clockFromXCoord( e.X );
            int value = valueFromYCoord( e.Y );
            int value_raw = value;

            if ( clock < AppManager.getVsqFile().getPreMeasure() ) {
                clock = AppManager.getVsqFile().getPreMeasure();
            }
            int max = mSelectedCurve.getMaximum();
            int min = mSelectedCurve.getMinimum();
            if ( value < min ) {
                value = min;
            } else if ( max < value ) {
                value = max;
            }
            return HandleMouseMoveForBezierMove( clock, value, value_raw, picked );
        }

        public void TrackSelector_MouseMove( Object sender, BMouseEventArgs e )
        {
            int value = valueFromYCoord( e.Y );
            int value_raw = value;
            int max = mSelectedCurve.getMaximum();
            int min = mSelectedCurve.getMinimum();
            int selected = AppManager.getSelected();
            boolean is_utau_mode = AppManager.mDrawIsUtau[selected - 1];
            if ( is_utau_mode && mSelectedCurve.equals( CurveType.VEL ) ) {
                max = UstEvent.MAX_INTENSITY;
                min = UstEvent.MIN_INTENSITY;
            }
            if ( value < min ) {
                value = min;
            } else if ( max < value ) {
                value = max;
            }
            mMouseValue = value;

            if ( e.Button == BMouseButtons.None ) {
                return;
            }
            int stdx = AppManager.getStartToDrawX();
            if ( (e.X + stdx != mMouseDownLocation.x || e.Y != mMouseDownLocation.y) ) {
#if JAVA
                if( mMouseHoverThread != null && mMouseHoverThread.isAlive() ){
                    mMouseHoverThread.stop();
                }
#else
                if ( mMouseHoverThread != null && mMouseHoverThread.IsAlive ) {
                    mMouseHoverThread.Abort();
                }
#endif
                if ( mMouseDownMode == MouseDownMode.VEL_WAIT_HOVER ) {
                    mMouseDownMode = MouseDownMode.VEL_EDIT;
                }
                mMouseMoved = true;
            }
            if ( AppManager.isPlaying() ) {
                return;
            }
            int clock = AppManager.clockFromXCoord( e.X );

            VsqFileEx vsq = AppManager.getVsqFile();
            if ( clock < vsq.getPreMeasure() ) {
                clock = vsq.getPreMeasure();
            }

            if ( e.Button == BMouseButtons.Left &&
                 0 <= e.Y && e.Y <= getHeight() - 2 * OFFSET_TRACK_TAB &&
                 mMouseDownMode == MouseDownMode.CURVE_EDIT ) {
                EditTool selected_tool = AppManager.getSelectedTool();
                if ( selected_tool == EditTool.PENCIL ) {
                    mPencilMoved = e.X + stdx != mMouseDownLocation.x ||
                                     e.Y != mMouseDownLocation.y;
                    mMouseTracer.append( e.X + stdx, e.Y );
                } else if ( selected_tool == EditTool.LINE ) {
                    mPencilMoved = e.X + stdx != mMouseDownLocation.x ||
                                     e.Y != mMouseDownLocation.y;
                } else if ( selected_tool == EditTool.ARROW ||
                            selected_tool == EditTool.ERASER ) {
                    int draft_clock = clock;
                    if ( AppManager.editorConfig.CurveSelectingQuantized ) {
                        int unit = AppManager.getPositionQuantizeClock();
                        int odd = clock % unit;
                        int nclock = clock;
                        nclock -= odd;
                        if ( odd > unit / 2 ) {
                            nclock += unit;
                        }
                        draft_clock = nclock;
                    }
                    AppManager.mCurveSelectingRectangle.width = draft_clock - AppManager.mCurveSelectingRectangle.x;
                    AppManager.mCurveSelectingRectangle.height = value - AppManager.mCurveSelectingRectangle.y;
                }
            } else if ( mMouseDownMode == MouseDownMode.SINGER_LIST ) {
                int dclock = clock - mSingerMoveStartedClock;
                for ( Iterator<SelectedEventEntry> itr = AppManager.itemSelection.getEventIterator(); itr.hasNext(); ) {
                    SelectedEventEntry item = itr.next();
                    item.editing.Clock = item.original.Clock + dclock;
                }
            } else if ( mMouseDownMode == MouseDownMode.VEL_EDIT ) {
                int t_value = valueFromYCoord( e.Y - mVelEditShiftY );
                int d_vel = 0;
                VsqEvent ve_original = mVelEditSelected.get( mVelEditLastSelectedID ).original;
                if ( mSelectedCurve.equals( CurveType.VEL ) ) {
                    if ( is_utau_mode ) {
                        d_vel = t_value - ((ve_original.UstEvent == null) ? 100 : ve_original.UstEvent.getIntensity());
                    } else {
                        d_vel = t_value - ve_original.ID.Dynamics;
                    }
                } else if ( mSelectedCurve.equals( CurveType.Accent ) ) {
                    d_vel = t_value - mVelEditSelected.get( mVelEditLastSelectedID ).original.ID.DEMaccent;
                } else if ( mSelectedCurve.equals( CurveType.Decay ) ) {
                    d_vel = t_value - mVelEditSelected.get( mVelEditLastSelectedID ).original.ID.DEMdecGainRate;
                }
                for ( Iterator<Integer> itr = mVelEditSelected.keySet().iterator(); itr.hasNext(); ) {
                    int id = itr.next();
                    if ( mSelectedCurve.equals( CurveType.VEL ) ) {
                        VsqEvent item = mVelEditSelected.get( id ).original;
                        int new_vel = item.ID.Dynamics + d_vel;
                        if ( is_utau_mode ) {
                            new_vel = item.UstEvent == null ? 100 + d_vel : item.UstEvent.getIntensity() + d_vel;
                        }
                        if ( new_vel < min ) {
                            new_vel = min;
                        } else if ( max < new_vel ) {
                            new_vel = max;
                        }
                        if ( is_utau_mode ) {
                            VsqEvent item_o = mVelEditSelected.get( id ).editing;
                            if ( item_o.UstEvent == null ) {
                                item_o.UstEvent = new UstEvent();
                            }
                            item_o.UstEvent.setIntensity( new_vel );
                        } else {
                            mVelEditSelected.get( id ).editing.ID.Dynamics = new_vel;
                        }
                    } else if ( mSelectedCurve.equals( CurveType.Accent ) ) {
                        int new_vel = mVelEditSelected.get( id ).original.ID.DEMaccent + d_vel;
                        if ( new_vel < min ) {
                            new_vel = min;
                        } else if ( max < new_vel ) {
                            new_vel = max;
                        }
                        mVelEditSelected.get( id ).editing.ID.DEMaccent = new_vel;
                    } else if ( mSelectedCurve.equals( CurveType.Decay ) ) {
                        int new_vel = mVelEditSelected.get( id ).original.ID.DEMdecGainRate + d_vel;
                        if ( new_vel < min ) {
                            new_vel = min;
                        } else if ( max < new_vel ) {
                            new_vel = max;
                        }
                        mVelEditSelected.get( id ).editing.ID.DEMdecGainRate = new_vel;
                    }
                }
            } else if ( mMouseDownMode == MouseDownMode.BEZIER_MODE ) {
                HandleMouseMoveForBezierMove( clock, value, value_raw, AppManager.itemSelection.getLastBezier().picked );
            } else if ( mMouseDownMode == MouseDownMode.BEZIER_ADD_NEW || mMouseDownMode == MouseDownMode.BEZIER_EDIT ) {
                BezierChain target = vsq.AttachedCurves.get( selected - 1 ).getBezierChain( mSelectedCurve, AppManager.itemSelection.getLastBezier().chainID );
                int point_id = AppManager.itemSelection.getLastBezier().pointID;
                int index = -1;
                for ( int i = 0; i < target.points.size(); i++ ) {
                    if ( target.points.get( i ).getID() == point_id ) {
                        index = i;
                        break;
                    }
                }
                if ( index >= 0 ) {
                    BezierPoint item = target.points.get( index );
                    Point old_right = item.getControlRight().toPoint();
                    Point old_left = item.getControlLeft().toPoint();
                    BezierControlType old_right_type = item.getControlRightType();
                    BezierControlType old_left_type = item.getControlLeftType();
                    int cl = clock;
                    int va = value_raw;
                    int dx = (int)item.getBase().getX() - cl;
                    int dy = (int)item.getBase().getY() - va;
                    if ( item.getBase().getX() + dx >= 0 ) {
                        item.setControlRight( new PointD( clock, value_raw ) );
                        item.setControlLeft( new PointD( clock + 2 * dx, value_raw + 2 * dy ) );
                        item.setControlRightType( BezierControlType.Normal );
                        item.setControlLeftType( BezierControlType.Normal );
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            item.setControlLeft( new PointD( old_left.x, old_left.y ) );
                            item.setControlRight( new PointD( old_right.x, old_right.y ) );
                            item.setControlLeftType( old_left_type );
                            item.setControlRightType( old_right_type );
                        }
                    }
                }
            } else if ( mMouseDownMode == MouseDownMode.ENVELOPE_MOVE ) {
                double sec = vsq.getSecFromClock( AppManager.clockFromXCoord( e.X ) );
                int v = valueFromYCoord( e.Y );
                if ( v < 0 ) {
                    v = 0;
                } else if ( 200 < v ) {
                    v = 200;
                }
                if ( sec < mEnvelopeDotBegin ) {
                    sec = mEnvelopeDotBegin;
                } else if ( mEnvelopeDotEnd < sec ) {
                    sec = mEnvelopeDotEnd;
                }
                if ( mEnvelopePointKind == 1 ) {
                    mEnvelopeEditing.p1 = (int)((sec - mEnvelopeRangeBegin) * 1000.0);
                    mEnvelopeEditing.v1 = v;
                } else if ( mEnvelopePointKind == 2 ) {
                    mEnvelopeEditing.p2 = (int)((sec - mEnvelopeRangeBegin) * 1000.0) - mEnvelopeEditing.p1;
                    mEnvelopeEditing.v2 = v;
                } else if ( mEnvelopePointKind == 3 ) {
                    mEnvelopeEditing.p5 = (int)((sec - mEnvelopeRangeBegin) * 1000.0) - mEnvelopeEditing.p1 - mEnvelopeEditing.p2;
                    mEnvelopeEditing.v5 = v;
                } else if ( mEnvelopePointKind == 4 ) {
                    mEnvelopeEditing.p3 = (int)((mEnvelopeRangeEnd - sec) * 1000.0) - mEnvelopeEditing.p4;
                    mEnvelopeEditing.v3 = v;
                } else if ( mEnvelopePointKind == 5 ) {
                    mEnvelopeEditing.p4 = (int)((mEnvelopeRangeEnd - sec) * 1000.0);
                    mEnvelopeEditing.v4 = v;
                }
            } else if ( mMouseDownMode == MouseDownMode.PRE_UTTERANCE_MOVE ) {
                int clock_at_downed = AppManager.clockFromXCoord( mMouseDownLocation.x - stdx );
                double dsec = vsq.getSecFromClock( clock ) - vsq.getSecFromClock( clock_at_downed );
                float draft_preutterance = mPreOverlapOriginal.UstEvent.getPreUtterance() - (float)(dsec * 1000);
                mPreOverlapEditing.UstEvent.setPreUtterance( draft_preutterance );
            } else if ( mMouseDownMode == MouseDownMode.OVERLAP_MOVE ) {
                int clock_at_downed = AppManager.clockFromXCoord( mMouseDownLocation.x - stdx );
                double dsec = vsq.getSecFromClock( clock ) - vsq.getSecFromClock( clock_at_downed );
                float draft_overlap = mPreOverlapOriginal.UstEvent.getVoiceOverlap() + (float)(dsec * 1000);
                mPreOverlapEditing.UstEvent.setVoiceOverlap( draft_overlap );
            }
#if JAVA
            repaint();
#endif
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
        private void findBezierPointAt( int locx,
                                        int locy,
                                        Vector<BezierChain> list,
                                        ByRef<BezierChain> found_chain,
                                        ByRef<BezierPoint> found_point,
                                        ByRef<BezierPickedSide> found_side,
                                        int dot_width,
                                        int px_tolerance )
        {
            found_chain.value = null;
            found_point.value = null;
            found_side.value = BezierPickedSide.BASE;
            int shift = dot_width + px_tolerance;
            int width = (dot_width + px_tolerance) * 2;
            int c = list.size();
            Point location = new Point( locx, locy );
            for ( int i = 0; i < c; i++ ) {
                BezierChain bc = list.get( i );
                for ( Iterator<BezierPoint> itr = bc.points.iterator(); itr.hasNext(); ) {
                    BezierPoint bp = itr.next();
                    Point p = getScreenCoord( bp.getBase() );
                    Rectangle r = new Rectangle( p.x - shift, p.y - shift, width, width );
                    if ( isInRect( locx, locy, r ) ) {
                        found_chain.value = bc;
                        found_point.value = bp;
                        found_side.value = BezierPickedSide.BASE;
                        return;
                    }

                    if ( bp.getControlLeftType() != BezierControlType.None ) {
                        p = getScreenCoord( bp.getControlLeft() );
                        r = new Rectangle( p.x - shift, p.y - shift, width, width );
                        if ( isInRect( locx, locy, r ) ) {
                            found_chain.value = bc;
                            found_point.value = bp;
                            found_side.value = BezierPickedSide.LEFT;
                            return;
                        }
                    }

                    if ( bp.getControlRightType() != BezierControlType.None ) {
                        p = getScreenCoord( bp.getControlRight() );
                        r = new Rectangle( p.x - shift, p.y - shift, width, width );
                        if ( isInRect( locx, locy, r ) ) {
                            found_chain.value = bc;
                            found_point.value = bp;
                            found_side.value = BezierPickedSide.RIGHT;
                            return;
                        }
                    }
                }
            }
        }

        private void processMouseDownSelectRegion( BMouseEventArgs e )
        {
            if ( (PortUtil.getCurrentModifierKey() & InputEvent.CTRL_MASK) != InputEvent.CTRL_MASK ) {
                AppManager.itemSelection.clearPoint();
            }

            int clock = AppManager.clockFromXCoord( e.X );
            int quantized_clock = clock;
            int unit = AppManager.getPositionQuantizeClock();
            int odd = clock % unit;
            quantized_clock -= odd;
            if ( odd > unit / 2 ) {
                quantized_clock += unit;
            }

            int max = mSelectedCurve.getMaximum();
            int min = mSelectedCurve.getMinimum();
            int value = valueFromYCoord( e.Y );
            if ( value < min ) {
                value = min;
            } else if ( max < value ) {
                value = max;
            }

            if ( AppManager.editorConfig.CurveSelectingQuantized ) {
                AppManager.mCurveSelectingRectangle = new Rectangle( quantized_clock, value, 0, 0 );
            } else {
                AppManager.mCurveSelectingRectangle = new Rectangle( clock, value, 0, 0 );
            }
        }

        public void TrackSelector_MouseDown( Object sender, BMouseEventArgs e )
        {
#if DEBUG
            AppManager.debugWriteLine( "TrackSelector_MouseDown" );
#endif
            VsqFileEx vsq = AppManager.getVsqFile();
            mMouseDownLocation.x = e.X + AppManager.getStartToDrawX();
            mMouseDownLocation.y = e.Y;
            int clock = AppManager.clockFromXCoord( e.X );
            int selected = AppManager.getSelected();
            int height = getHeight();
            int width = getWidth();
            int key_width = AppManager.keyWidth;
            VsqTrack vsq_track = vec.get( vsq.Track, selected );
            mMouseMoved = false;
            mMouseDowned = true;
            if ( AppManager.keyWidth < e.X && clock < vsq.getPreMeasure() ) {
#if !JAVA
                System.Media.SystemSounds.Asterisk.Play();
#endif
                return;
            }
            int stdx = AppManager.getStartToDrawX();
            mModifierOnMouseDown = PortUtil.getCurrentModifierKey();
            int max = mSelectedCurve.getMaximum();
            int min = mSelectedCurve.getMinimum();
            int value = valueFromYCoord( e.Y );
            if ( value < min ) {
                value = min;
            } else if ( max < value ) {
                value = max;
            }

            if ( height - OFFSET_TRACK_TAB <= e.Y && e.Y < height ) {
                if ( e.Button == BMouseButtons.Left ) {
                    #region MouseDown occured on track list
                    mMouseDownMode = MouseDownMode.TRACK_LIST;
                    //AppManager.isCurveSelectedIntervalEnabled() = false;
                    mMouseTracer.clear();
                    int selecter_width = getSelectorWidth();
                    if ( vsq != null ) {
                        for ( int i = 0; i < 16; i++ ) {
                            int x = key_width + i * selecter_width;
                            if ( vec.size( vsq.Track ) > i + 1 ) {
                                if ( x <= e.X && e.X < x + selecter_width ) {
                                    int new_selected = i + 1;
                                    if ( AppManager.getSelected() != new_selected ) {
                                        AppManager.setSelected( i + 1 );
                                        try {
#if JAVA
                                            selectedTrackChangedEvent.raise( this, (i + 1) );
#else
                                            if ( SelectedTrackChanged != null ) {
                                                SelectedTrackChanged.Invoke( this, i + 1 );
                                            }
#endif
                                        } catch ( Exception ex ) {
                                            serr.println( "TrackSelector#TrackSelector_MouseDown; ex=" + ex );
                                        }
                                        invalidate();
                                        return;
                                    } else if ( x + selecter_width - PX_WIDTH_RENDER <= e.X && e.X < e.X + selecter_width ) {
                                        if ( AppManager.getRenderRequired( AppManager.getSelected() ) && !AppManager.isPlaying() ) {
                                            try {
#if JAVA
                                                renderRequiredEvent.raise( this, AppManager.getSelected() );
#else
                                                if ( RenderRequired != null ) {
                                                    RenderRequired.Invoke( this, AppManager.getSelected() );
                                                }
#endif
                                            } catch ( Exception ex ) {
                                                serr.println( "TrackSelector#TrackSelector_MouseDown; ex=" + ex );
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            } else if ( height - 2 * OFFSET_TRACK_TAB <= e.Y && e.Y < height - OFFSET_TRACK_TAB ) {
                #region MouseDown occured on singer tab
                mMouseDownMode = MouseDownMode.SINGER_LIST;
                AppManager.itemSelection.clearPoint();
                mMouseTracer.clear();
                VsqEvent ve = null;
                if ( key_width <= e.X && e.X <= width ){
                    ve = findItemAt( e.X, e.Y );
                }
                if ( AppManager.getSelectedTool() == EditTool.ERASER ) {
                    #region EditTool.Eraser
                    if ( ve != null && ve.Clock > 0 ) {
                        CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventDelete( selected, ve.InternalID ) );
                        executeCommand( run, true );
                    }
                    #endregion
                } else {
                    if ( ve != null ) {
                        if ( (mModifierOnMouseDown & mModifierKey) == mModifierKey ) {
                            if ( AppManager.itemSelection.isEventContains( AppManager.getSelected(), ve.InternalID ) ) {
                                Vector<Integer> old = new Vector<Integer>();
                                for ( Iterator<SelectedEventEntry> itr = AppManager.itemSelection.getEventIterator(); itr.hasNext(); ) {
                                    SelectedEventEntry item = itr.next();
                                    int id = item.original.InternalID;
                                    if ( id != ve.InternalID ) {
                                        old.add( id );
                                    }
                                }
                                AppManager.itemSelection.clearEvent();
                                AppManager.itemSelection.addEventAll( old );
                            } else {
                                AppManager.itemSelection.addEvent( ve.InternalID );
                            }
                        } else if ( (PortUtil.getCurrentModifierKey() & InputEvent.SHIFT_MASK) == InputEvent.SHIFT_MASK ) {
                            int last_clock = AppManager.itemSelection.getLastEvent().original.Clock;
                            int tmin = Math.Min( ve.Clock, last_clock );
                            int tmax = Math.Max( ve.Clock, last_clock );
                            Vector<Integer> add_required = new Vector<Integer>();
                            for ( Iterator<VsqEvent> itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventIterator(); itr.hasNext(); ) {
                                VsqEvent item = itr.next();
                                if ( item.ID.type == VsqIDType.Singer && tmin <= item.Clock && item.Clock <= tmax ) {
                                    add_required.add( item.InternalID );
                                    //AppManager.AddSelectedEvent( item.InternalID );
                                }
                            }
                            add_required.add( ve.InternalID );
                            AppManager.itemSelection.addEventAll( add_required );
                        } else {
                            if ( !AppManager.itemSelection.isEventContains( AppManager.getSelected(), ve.InternalID ) ) {
                                AppManager.itemSelection.clearEvent();
                            }
                            AppManager.itemSelection.addEvent( ve.InternalID );
                        }
                        mSingerMoveStartedClock = clock;
                    } else {
                        AppManager.itemSelection.clearEvent();
                    }
                }
                #endregion
            } else {
                #region MouseDown occred on other position
                boolean clock_inner_note = false; //マウスの降りたクロックが，ノートの範囲内かどうかをチェック
                int left_clock = AppManager.clockFromXCoord( AppManager.keyWidth );
                int right_clock = AppManager.clockFromXCoord( getWidth() );
                for ( Iterator<VsqEvent> itr = vsq_track.getEventIterator(); itr.hasNext(); ) {
                    VsqEvent ve = itr.next();
                    if ( ve.ID.type == VsqIDType.Anote ) {
                        int start = ve.Clock;
                        if ( right_clock < start ) {
                            break;
                        }
                        int end = ve.Clock + ve.ID.getLength();
                        if ( end < left_clock ) {
                            continue;
                        }
                        if ( start <= clock && clock < end ) {
                            clock_inner_note = true;
                            break;
                        }
                    }
                }
#if DEBUG
                AppManager.debugWriteLine( "    clock_inner_note=" + clock_inner_note );
#endif
                if ( AppManager.keyWidth <= e.X ) {
                    if ( e.Button == BMouseButtons.Left && !mSpaceKeyDowned ) {
                        mMouseDownMode = MouseDownMode.CURVE_EDIT;
                        int quantized_clock = clock;
                        int unit = AppManager.getPositionQuantizeClock();
                        int odd = clock % unit;
                        quantized_clock -= odd;
                        if ( odd > unit / 2 ) {
                            quantized_clock += unit;
                        }

                        int px_shift = DOT_WID + AppManager.editorConfig.PxToleranceBezier;
                        int px_width = px_shift * 2 + 1;

                        if ( AppManager.getSelectedTool() == EditTool.LINE ) {
                            #region Line
                            if ( AppManager.isCurveMode() ) {
                                if ( mSelectedCurve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownEnvelope( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                    if ( processMouseDownPreutteranceAndOverlap( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                } else if ( !mSelectedCurve.equals( CurveType.VEL ) &&
                                            !mSelectedCurve.equals( CurveType.Accent ) &&
                                            !mSelectedCurve.equals( CurveType.Decay ) &&
                                            !mSelectedCurve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownBezier( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                }
                            } else {
                                if ( mSelectedCurve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownEnvelope( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                    if ( processMouseDownPreutteranceAndOverlap( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                }
                            }
                            mMouseTracer.clear();
                            mMouseTracer.appendFirst( e.X + stdx, e.Y );
                            #endregion
                        } else if ( AppManager.getSelectedTool() == EditTool.PENCIL ) {
                            #region Pencil
                            if ( AppManager.isCurveMode() ) {
                                #region CurveMode
                                if ( mSelectedCurve.equals( CurveType.VibratoRate ) || mSelectedCurve.equals( CurveType.VibratoDepth ) ) {
                                    // todo: TrackSelector_MouseDownのベジエ曲線
                                } else if ( mSelectedCurve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownEnvelope( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                    if ( processMouseDownPreutteranceAndOverlap( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                } else if ( !mSelectedCurve.equals( CurveType.VEL ) &&
                                            !mSelectedCurve.equals( CurveType.Accent ) &&
                                            !mSelectedCurve.equals( CurveType.Decay ) &&
                                            !mSelectedCurve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownBezier( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                } else {
                                    mMouseDownMode = MouseDownMode.NONE;
                                }
                                #endregion
                            } else {
                                #region NOT CurveMode
                                if ( mSelectedCurve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownEnvelope( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                    if ( processMouseDownPreutteranceAndOverlap( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                }
                                mMouseTracer.clear();
                                int x = e.X + AppManager.getStartToDrawX();
                                mMouseTracer.appendFirst( x, e.Y );
                                mPencilMoved = false;

#if JAVA
                                mMouseHoverThread = new MouseHoverEventGeneratorProc();
                                mMouseHoverThread.start();
#else
                                mMouseHoverThread = new Thread( new ThreadStart( MouseHoverEventGenerator ) );
                                mMouseHoverThread.Start();
#endif
                                #endregion
                            }
                            #endregion
                        } else if ( AppManager.getSelectedTool() == EditTool.ARROW ) {
                            #region Arrow
                            boolean found = false;
                            if ( mSelectedCurve.isScalar() || mSelectedCurve.isAttachNote() ) {
                                if ( mSelectedCurve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownEnvelope( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                    if ( processMouseDownPreutteranceAndOverlap( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                }
                                mMouseDownMode = MouseDownMode.NONE;
                            } else {
                                // まずベジエ曲線の点にヒットしてないかどうかを検査
                                Vector<BezierChain> dict = AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).get( mSelectedCurve );
                                AppManager.itemSelection.clearBezier();
                                for ( int i = 0; i < dict.size(); i++ ) {
                                    BezierChain bc = dict.get( i );
                                    for ( Iterator<BezierPoint> itr = bc.points.iterator(); itr.hasNext(); ) {
                                        BezierPoint bp = itr.next();
                                        Point pt = getScreenCoord( bp.getBase() );
                                        Rectangle rc = new Rectangle( pt.x - px_shift, pt.y - px_shift, px_width, px_width );
                                        if ( isInRect( e.X, e.Y, rc ) ) {
                                            AppManager.itemSelection.addBezier( new SelectedBezierPoint( bc.id, bp.getID(), BezierPickedSide.BASE, bp ) );
                                            mEditingBezierOriginal = (BezierChain)bc.clone();
                                            found = true;
                                            break;
                                        }

                                        if ( bp.getControlLeftType() != BezierControlType.None ) {
                                            pt = getScreenCoord( bp.getControlLeft() );
                                            rc = new Rectangle( pt.x - px_shift, pt.y - px_shift, px_width, px_width );
                                            if ( isInRect( e.X, e.Y, rc ) ) {
                                                AppManager.itemSelection.addBezier( new SelectedBezierPoint( bc.id, bp.getID(), BezierPickedSide.LEFT, bp ) );
                                                mEditingBezierOriginal = (BezierChain)bc.clone();
                                                found = true;
                                                break;
                                            }
                                        }

                                        if ( bp.getControlRightType() != BezierControlType.None ) {
                                            pt = getScreenCoord( bp.getControlRight() );
                                            rc = new Rectangle( pt.x - px_shift, pt.y - px_shift, px_width, px_width );
                                            if ( isInRect( e.X, e.Y, rc ) ) {
                                                AppManager.itemSelection.addBezier( new SelectedBezierPoint( bc.id, bp.getID(), BezierPickedSide.RIGHT, bp ) );
                                                mEditingBezierOriginal = (BezierChain)bc.clone();
                                                found = true;
                                                break;
                                            }
                                        }
                                    }
                                    if ( found ) {
                                        break;
                                    }
                                }
                                if ( found ) {
                                    mMouseDownMode = MouseDownMode.BEZIER_MODE;
                                }
                            }

                            // ベジエ曲線の点にヒットしなかった場合
                            if ( !found ) {
                                #region NOT CurveMode
                                VsqEvent ve = findItemAt( e.X, e.Y );
                                // マウス位置の音符アイテムを検索
                                if ( ve != null ) {
                                    boolean found2 = false;
                                    if ( (mModifierOnMouseDown & mModifierKey) == mModifierKey ) {
                                        // clicked with CTRL key
                                        Vector<Integer> list = new Vector<Integer>();
                                        for ( Iterator<SelectedEventEntry> itr = AppManager.itemSelection.getEventIterator(); itr.hasNext(); ) {
                                            SelectedEventEntry item = itr.next();
                                            VsqEvent ve2 = item.original;
                                            if ( ve.InternalID == ve2.InternalID ) {
                                                found2 = true;
                                            } else {
                                                list.add( ve2.InternalID );
                                            }
                                        }
                                        AppManager.itemSelection.clearEvent();
                                        AppManager.itemSelection.addEventAll( list );
                                    } else if ( (PortUtil.getCurrentModifierKey() & InputEvent.SHIFT_MASK) == InputEvent.SHIFT_MASK ) {
                                        // clicked with Shift key
                                        SelectedEventEntry last_selected = AppManager.itemSelection.getLastEvent();
                                        if ( last_selected != null ) {
                                            int last_clock = last_selected.original.Clock;
                                            int tmin = Math.Min( ve.Clock, last_clock );
                                            int tmax = Math.Max( ve.Clock, last_clock );
                                            Vector<Integer> add_required = new Vector<Integer>();
                                            for ( Iterator<VsqEvent> itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                                                VsqEvent item = itr.next();
                                                if ( tmin <= item.Clock && item.Clock <= tmax ) {
                                                    add_required.add( item.InternalID );
                                                }
                                            }
                                            AppManager.itemSelection.addEventAll( add_required );
                                        }
                                    } else {
                                        // no modefier key
                                        if ( !AppManager.itemSelection.isEventContains( AppManager.getSelected(), ve.InternalID ) ) {
                                            AppManager.itemSelection.clearEvent();
                                        }
                                    }
                                    if ( !found2 ) {
                                        AppManager.itemSelection.addEvent( ve.InternalID );
                                    }

                                    mMouseDownMode = MouseDownMode.VEL_WAIT_HOVER;
                                    mVelEditLastSelectedID = ve.InternalID;
                                    if ( mSelectedCurve.equals( CurveType.VEL ) ) {
                                        if ( AppManager.mDrawIsUtau[selected - 1] ) {
                                            mVelEditShiftY = e.Y - yCoordFromValue( ve.UstEvent == null ? 100 : ve.UstEvent.getIntensity() );
                                        } else {
                                            mVelEditShiftY = e.Y - yCoordFromValue( ve.ID.Dynamics );
                                        }
                                    } else if ( mSelectedCurve.equals( CurveType.Accent ) ) {
                                        mVelEditShiftY = e.Y - yCoordFromValue( ve.ID.DEMaccent );
                                    } else if ( mSelectedCurve.equals( CurveType.Decay ) ) {
                                        mVelEditShiftY = e.Y - yCoordFromValue( ve.ID.DEMdecGainRate );
                                    }
                                    mVelEditSelected.clear();
                                    if ( AppManager.itemSelection.isEventContains( AppManager.getSelected(), mVelEditLastSelectedID ) ) {
                                        for ( Iterator<SelectedEventEntry> itr = AppManager.itemSelection.getEventIterator(); itr.hasNext(); ) {
                                            SelectedEventEntry item = itr.next();
                                            mVelEditSelected.put( item.original.InternalID,
                                                                    new SelectedEventEntry( AppManager.getSelected(),
                                                                                            item.original,
                                                                                            item.editing ) );
                                        }
                                    } else {
                                        mVelEditSelected.put( mVelEditLastSelectedID,
                                                                new SelectedEventEntry( AppManager.getSelected(),
                                                                                        (VsqEvent)ve.clone(),
                                                                                        (VsqEvent)ve.clone() ) );
                                    }
#if JAVA
                                    mMouseHoverThread = new MouseHoverEventGeneratorProc();
                                    mMouseHoverThread.start();
#else
                                    mMouseHoverThread = new Thread( new ThreadStart( MouseHoverEventGenerator ) );
                                    mMouseHoverThread.Start();
#endif
                                    invalidate();
                                    return;
                                }

                                // マウス位置のデータポイントを検索
                                long id = findDataPointAt( e.X, e.Y );
                                if ( id > 0 ) {
                                    if ( AppManager.itemSelection.isPointContains( id ) ) {
                                        if ( (mModifierOnMouseDown & mModifierKey) == mModifierKey ) {
                                            AppManager.itemSelection.removePoint( id );
                                            mMouseDownMode = MouseDownMode.NONE;
                                            invalidate();
                                            return;
                                        }
                                    } else {
                                        if ( (mModifierOnMouseDown & mModifierKey) != mModifierKey ) {
                                            AppManager.itemSelection.clearPoint();
                                        }
                                        AppManager.itemSelection.addPoint( mSelectedCurve, id );
                                    }

                                    mMouseDownMode = MouseDownMode.POINT_MOVE;
                                    mMovingPoints.clear();
                                    VsqBPList list = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( mSelectedCurve.getName() );
                                    if ( list != null ) {
                                        int count = list.size();
                                        for ( int i = 0; i < count; i++ ) {
                                            VsqBPPair item = list.getElementB( i );
                                            if ( AppManager.itemSelection.isPointContains( item.id ) ) {
                                                mMovingPoints.add( new BPPair( list.getKeyClock( i ), item.value ) );
                                            }
                                        }
                                        invalidate();
                                        return;
                                    }
                                } else {
                                    if ( (mModifierOnMouseDown & InputEvent.CTRL_MASK) != InputEvent.CTRL_MASK ) {
                                        AppManager.itemSelection.clearPoint();
                                    }
                                    if ( (mModifierOnMouseDown & InputEvent.SHIFT_MASK) != InputEvent.SHIFT_MASK && (mModifierOnMouseDown & mModifierKey) != mModifierKey ) {
                                        AppManager.itemSelection.clearPoint();
                                    }
                                }

                                if ( (mModifierOnMouseDown & mModifierKey) != mModifierKey ) {
                                    AppManager.setCurveSelectedIntervalEnabled( false );
                                }
                                if ( AppManager.editorConfig.CurveSelectingQuantized ) {
                                    AppManager.mCurveSelectingRectangle = new Rectangle( quantized_clock, value, 0, 0 );
                                } else {
                                    AppManager.mCurveSelectingRectangle = new Rectangle( clock, value, 0, 0 );
                                }
                                #endregion
                            }
                            #endregion
                        } else if ( AppManager.getSelectedTool() == EditTool.ERASER ) {
                            #region Eraser
                            VsqEvent ve3 = findItemAt( e.X, e.Y );
                            if ( ve3 != null ) {
                                AppManager.itemSelection.clearEvent();
                                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventDelete( selected,
                                                                                                                  ve3.InternalID ) );
                                executeCommand( run, true );
                            } else {
                                if ( AppManager.isCurveMode() ) {
                                    Vector<BezierChain> list = vsq.AttachedCurves.get( AppManager.getSelected() - 1 ).get( mSelectedCurve );
                                    if ( list != null ) {
                                        ByRef<BezierChain> chain = new ByRef<BezierChain>();
                                        ByRef<BezierPoint> point = new ByRef<BezierPoint>();
                                        ByRef<BezierPickedSide> side = new ByRef<BezierPickedSide>();
                                        findBezierPointAt( e.X, e.Y, list, chain, point, side, DOT_WID, AppManager.editorConfig.PxToleranceBezier );
                                        if ( point.value != null ) {
                                            if ( side.value == BezierPickedSide.BASE ) {
                                                // データ点自体を削除
                                                BezierChain work = (BezierChain)chain.value.clone();
                                                int count = work.points.size();
                                                if ( count > 1 ) {
                                                    // 2個以上のデータ点があるので、BezierChainを置換
                                                    for ( int i = 0; i < count; i++ ) {
                                                        BezierPoint bp = work.points.get( i );
                                                        if ( bp.getID() == point.value.getID() ) {
                                                            work.points.removeElementAt( i );
                                                            break;
                                                        }
                                                    }
                                                    CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( selected,
                                                                                                                       mSelectedCurve,
                                                                                                                       chain.value.id,
                                                                                                                       work,
                                                                                                                       AppManager.editorConfig.getControlCurveResolutionValue() );
                                                    executeCommand( run, true );
                                                    mMouseDownMode = MouseDownMode.NONE;
                                                    invalidate();
                                                    return;
                                                } else {
                                                    // 1個しかデータ点がないので、BezierChainを削除
                                                    CadenciiCommand run = VsqFileEx.generateCommandDeleteBezierChain( AppManager.getSelected(),
                                                                                                                      mSelectedCurve,
                                                                                                                      chain.value.id,
                                                                                                                      AppManager.editorConfig.getControlCurveResolutionValue() );
                                                    executeCommand( run, true );
                                                    mMouseDownMode = MouseDownMode.NONE;
                                                    invalidate();
                                                    return;
                                                }
                                            } else {
                                                // 滑らかにするオプションを解除する
                                                BezierChain work = (BezierChain)chain.value.clone();
                                                int count = work.points.size();
                                                for ( int i = 0; i < count; i++ ) {
                                                    BezierPoint bp = work.points.get( i );
                                                    if ( bp.getID() == point.value.getID() ) {
                                                        bp.setControlLeftType( BezierControlType.None );
                                                        bp.setControlRightType( BezierControlType.None );
                                                        break;
                                                    }
                                                }
                                                CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( AppManager.getSelected(),
                                                                                                                   mSelectedCurve,
                                                                                                                   chain.value.id,
                                                                                                                   work,
                                                                                                                   AppManager.editorConfig.getControlCurveResolutionValue() );
                                                executeCommand( run, true );
                                                mMouseDownMode = MouseDownMode.NONE;
                                                invalidate();
                                                return;
                                            }
                                        }
                                    }
                                } else {
                                    long id = findDataPointAt( e.X, e.Y );
                                    if ( id > 0 ) {
                                        VsqBPList item = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( mSelectedCurve.getName() );
                                        if ( item != null ) {
                                            VsqBPList work = (VsqBPList)item.clone();
                                            VsqBPPairSearchContext context = work.findElement( id );
                                            if ( context.point.id == id ) {
                                                work.remove( context.clock );
                                                CadenciiCommand run = new CadenciiCommand(
                                                    VsqCommand.generateCommandTrackCurveReplace( selected,
                                                                                                 mSelectedCurve.getName(),
                                                                                                 work ) );
                                                executeCommand( run, true );
                                                mMouseDownMode = MouseDownMode.NONE;
                                                invalidate();
                                                return;
                                            }
                                        }
                                    }
                                }

                                if ( (mModifierOnMouseDown & InputEvent.SHIFT_MASK) != InputEvent.SHIFT_MASK && (mModifierOnMouseDown & mModifierKey) != mModifierKey ) {
                                    AppManager.itemSelection.clearPoint();
                                }
                                if ( AppManager.editorConfig.CurveSelectingQuantized ) {
                                    AppManager.mCurveSelectingRectangle = new Rectangle( quantized_clock, value, 0, 0 );
                                } else {
                                    AppManager.mCurveSelectingRectangle = new Rectangle( clock, value, 0, 0 );
                                }
                            }
                            #endregion
                        }
                    } else if ( e.Button == BMouseButtons.Right ) {
                        if ( AppManager.isCurveMode() ) {
                            if ( !mSelectedCurve.equals( CurveType.VEL ) && !mSelectedCurve.equals( CurveType.Env ) ) {
                                Vector<BezierChain> dict = AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).get( mSelectedCurve );
                                AppManager.itemSelection.clearBezier();
                                boolean found = false;
                                for ( int i = 0; i < dict.size(); i++ ) {
                                    BezierChain bc = dict.get( i );
                                    for ( Iterator<BezierPoint> itr = bc.points.iterator(); itr.hasNext(); ) {
                                        BezierPoint bp = itr.next();
                                        Point pt = getScreenCoord( bp.getBase() );
                                        Rectangle rc = new Rectangle( pt.x - DOT_WID, pt.y - DOT_WID, 2 * DOT_WID + 1, 2 * DOT_WID + 1 );
                                        if ( isInRect( e.X, e.Y, rc ) ) {
                                            AppManager.itemSelection.addBezier( new SelectedBezierPoint( bc.id, bp.getID(), BezierPickedSide.BASE, bp ) );
                                            found = true;
                                            break;
                                        }
                                    }
                                    if ( found ) {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                } else {
                    AppManager.setCurveSelectedIntervalEnabled( false );
                }
                #endregion
            }
            invalidate();
        }

        private boolean processMouseDownBezier( BMouseEventArgs e )
        {
#if DEBUG
            sout.println( "TrackSelector::processMouseDownBezier" );
#endif
            int clock = AppManager.clockFromXCoord( e.X );
            int max = mSelectedCurve.getMaximum();
            int min = mSelectedCurve.getMinimum();
            int value = valueFromYCoord( e.Y );
            if ( value < min ) {
                value = min;
            } else if ( max < value ) {
                value = max;
            }
            int px_shift = DOT_WID + AppManager.editorConfig.PxToleranceBezier;
            int px_width = px_shift * 2 + 1;
            int modifier = PortUtil.getCurrentModifierKey();

            int track = AppManager.getSelected();
            boolean too_near = false; // clicked position is too near to existing bezier points
            boolean is_middle = false;

            // check whether bezier point exists on clicked position
            Vector<BezierChain> dict = AppManager.getVsqFile().AttachedCurves.get( track - 1 ).get( mSelectedCurve );
            ByRef<BezierChain> found_chain = new ByRef<BezierChain>();
            ByRef<BezierPoint> found_point = new ByRef<BezierPoint>();
            ByRef<BezierPickedSide> found_side = new ByRef<BezierPickedSide>();
            findBezierPointAt(
                e.X, e.Y,
                dict, found_chain, found_point, found_side,
                DOT_WID, AppManager.editorConfig.PxToleranceBezier );
#if DEBUG
            sout.println( "TrackSelector::processMouseDownBezier; (found_chain.value==null)=" + (found_chain.value == null) );
#endif

            if ( found_chain.value != null ) {
                AppManager.itemSelection.addBezier(
                    new SelectedBezierPoint(
                        found_chain.value.id, found_point.value.getID(),
                        found_side.value, found_point.value ) );
                mEditingBezierOriginal = (BezierChain)found_chain.value.clone();
                mMouseDownMode = MouseDownMode.BEZIER_MODE;
            } else {
                if ( AppManager.getSelectedTool() != EditTool.PENCIL ) {
                    return false;
                }
                BezierChain target_chain = null;
                for ( int j = 0; j < dict.size(); j++ ) {
                    BezierChain bc = dict.get( j );
                    for ( int i = 1; i < bc.size(); i++ ) {
                        if ( !is_middle && bc.points.get( i - 1 ).getBase().getX() <= clock && clock <= bc.points.get( i ).getBase().getX() ) {
                            target_chain = (BezierChain)bc.clone();
                            is_middle = true;
                        }
                        if ( !too_near ) {
                            for ( Iterator<BezierPoint> itr = bc.points.iterator(); itr.hasNext(); ) {
                                BezierPoint bp = itr.next();
                                Point pt = getScreenCoord( bp.getBase() );
                                Rectangle rc = new Rectangle( pt.x - px_shift, pt.y - px_shift, px_width, px_width );
                                if ( isInRect( e.X, e.Y, rc ) ) {
                                    too_near = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if ( !too_near ) {
                    if ( (modifier & mModifierKey) != mModifierKey && target_chain == null ) {
                        // search BezierChain just before the clicked position
                        int tmax = -1;
                        for ( int i = 0; i < dict.size(); i++ ) {
                            BezierChain bc = dict.get( i );
                            Collections.sort( bc.points );
                            // check most nearest data point from clicked position
                            int last = (int)bc.points.get( bc.points.size() - 1 ).getBase().getX();
                            if ( tmax < last && last < clock ) {
                                tmax = last;
                                target_chain = (BezierChain)bc.clone();
                            }
                        }
                    }

#if DEBUG
                    AppManager.debugWriteLine( "    (target_chain==null)=" + (target_chain == null) );
                    if ( target_chain != null && found_point.value != null ) {
                        sout.println( "TrackSelector::procesMouseDownBezier; before:(" + found_point.value.getPosition( found_side.value ) + "," + found_point.value.getPosition( found_side.value ) + "); after:(" + clock + "," + value + ")" );
                    }
#endif
                    // fork whether target_chain is null or not
                    PointD pt = new PointD( clock, value );
                    BezierPoint bp = null;
                    int chain_id = -1;
                    int point_id = -1;
                    if ( target_chain == null ) {
                        // generate new BezierChain
                        BezierChain adding = new BezierChain();
                        bp = new BezierPoint( pt, pt, pt );
                        point_id = adding.getNextId();
                        bp.setID( point_id );
                        adding.add( bp );
                        chain_id = AppManager.getVsqFile().AttachedCurves.get( track - 1 ).getNextId( mSelectedCurve );
#if DEBUG
                        AppManager.debugWriteLine( "    new chain_id=" + chain_id );
#endif
                        CadenciiCommand run = VsqFileEx.generateCommandAddBezierChain( track,
                                                                                mSelectedCurve,
                                                                                chain_id,
                                                                                AppManager.editorConfig.getControlCurveResolutionValue(),
                                                                                adding );
                        executeCommand( run, false );
                        mMouseDownMode = MouseDownMode.BEZIER_ADD_NEW;
                    } else {
                        mEditingBezierOriginal = (BezierChain)target_chain.clone();
                        bp = new BezierPoint( pt, pt, pt );
                        point_id = target_chain.getNextId();
                        bp.setID( point_id );
                        target_chain.add( bp );
                        Collections.sort( target_chain.points );
                        chain_id = target_chain.id;
                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( track,
                                                                                    mSelectedCurve,
                                                                                    target_chain.id,
                                                                                    target_chain,
                                                                                    AppManager.editorConfig.getControlCurveResolutionValue() );
                        executeCommand( run, false );
                        mMouseDownMode = MouseDownMode.BEZIER_EDIT;
                    }
                    AppManager.itemSelection.clearBezier();
                    AppManager.itemSelection.addBezier( new SelectedBezierPoint( chain_id, point_id, BezierPickedSide.BASE, bp ) );
                } else {
                    mMouseDownMode = MouseDownMode.NONE;
                }
            }
            return true;
        }

        private boolean processMouseDownPreutteranceAndOverlap( BMouseEventArgs e )
        {
            ByRef<Integer> internal_id = new ByRef<Integer>();
            ByRef<Boolean> found_flag_was_overlap = new ByRef<Boolean>();
            if ( findPreUtteranceOrOverlapAt( e.X, e.Y, internal_id, found_flag_was_overlap ) ) {
                if ( found_flag_was_overlap.value ) {
                    mOverlapEditingID = internal_id.value;
                    mPreOverlapEditing = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).findEventFromID( mOverlapEditingID );
                    if ( mPreOverlapEditing == null ) {
                        mMouseDownMode = MouseDownMode.NONE;
                        return false;
                    }
                    mPreOverlapOriginal = (VsqEvent)mPreOverlapEditing.clone();
                    mMouseDownMode = MouseDownMode.OVERLAP_MOVE;
                    return true;
                } else {
                    mPreUtteranceEditingID = internal_id.value;
                    mPreOverlapEditing = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).findEventFromID( mPreUtteranceEditingID );
                    if ( mPreOverlapEditing == null ) {
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

        private boolean processMouseDownEnvelope( BMouseEventArgs e )
        {
            ByRef<Integer> internal_id = new ByRef<Integer>( -1 );
            ByRef<Integer> point_kind = new ByRef<Integer>( -1 );
            if ( !findEnvelopePointAt( e.X, e.Y, internal_id, point_kind ) ) {
                return false;
            }
#if DEBUG
            sout.println( "processTrackSelectorMouseDownForEnvelope; internal_id=" + internal_id.value + "; point_kind=" + point_kind.value );
#endif
            mEnvelopeOriginal = null;
            VsqFileEx vsq = AppManager.getVsqFile();
            VsqTrack vsq_track = vsq.Track.get( AppManager.getSelected() );
            VsqEvent found = vsq_track.findEventFromID( internal_id.value );
            if ( found == null ) {
                return false;
            }
            if ( found.UstEvent != null && found.UstEvent.getEnvelope() != null ) {
                mEnvelopeOriginal = (UstEnvelope)found.UstEvent.getEnvelope().clone();
                mEnvelopeEditing = found.UstEvent.getEnvelope();
            }
            if ( mEnvelopeOriginal == null ) {
                found.UstEvent.setEnvelope( new UstEnvelope() );
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
            Iterator<VsqEvent> itr = vsq_track.getNoteEventIterator();
            while ( true ) {
                item_prev = item;
                item = item_next;
                if ( itr.hasNext() ) {
                    item_next = itr.next();
                } else {
                    item_next = null;
                }
                if ( item_prev == null && item == null && item_next == null ) {
                    break;
                }
                if ( item == null ) {
                    continue;
                }
                if ( item.InternalID == mEnvelopeEdigintID ) {
                    break;
                }
            }
            if ( item_prev != null ) {
                // 直前の音符と接触しているかどうか
                if ( item_prev.Clock + item_prev.ID.getLength() < item.Clock ) {
                    item_prev = null;
                }
            }
            if ( item_next != null ) {
                // 直後の音符と接触しているかどうか
                if ( item.Clock + item.ID.getLength() < item_next.Clock ) {
                    item_next = null;
                }
            }
            ByRef<Double> env_start = new ByRef<Double>( 0.0 );
            ByRef<Double> env_end = new ByRef<Double>( 0.0 );
            getEnvelopeRegion( vsq.TempoTable, item_prev, item, item_next, env_start, env_end );

            mEnvelopeRangeBegin = env_start.value;
            mEnvelopeRangeEnd = env_end.value;
            if ( mEnvelopePointKind == 1 ) {
                mEnvelopeDotBegin = mEnvelopeRangeBegin;
                mEnvelopeDotEnd = mEnvelopeRangeEnd - (mEnvelopeOriginal.p4 + mEnvelopeOriginal.p3 + mEnvelopeOriginal.p5 + mEnvelopeOriginal.p2) / 1000.0;
            } else if ( mEnvelopePointKind == 2 ) {
                mEnvelopeDotBegin = mEnvelopeRangeBegin + mEnvelopeOriginal.p1 / 1000.0;
                mEnvelopeDotEnd = mEnvelopeRangeEnd - (mEnvelopeOriginal.p4 + mEnvelopeOriginal.p3 + mEnvelopeOriginal.p5) / 1000.0;
            } else if ( mEnvelopePointKind == 3 ) {
                mEnvelopeDotBegin = mEnvelopeRangeBegin + (mEnvelopeOriginal.p1 + mEnvelopeOriginal.p2) / 1000.0;
                mEnvelopeDotEnd = mEnvelopeRangeEnd - (mEnvelopeOriginal.p4 + mEnvelopeOriginal.p3) / 1000.0;
            } else if ( mEnvelopePointKind == 4 ) {
                mEnvelopeDotBegin = mEnvelopeRangeBegin + (mEnvelopeOriginal.p1 + mEnvelopeOriginal.p2 + mEnvelopeOriginal.p5) / 1000.0;
                mEnvelopeDotEnd = mEnvelopeRangeEnd - mEnvelopeOriginal.p4 / 1000.0;
            } else if ( mEnvelopePointKind == 5 ) {
                mEnvelopeDotBegin = mEnvelopeRangeBegin + (mEnvelopeOriginal.p1 + mEnvelopeOriginal.p2 + mEnvelopeOriginal.p5 + mEnvelopeOriginal.p3) / 1000.0;
                mEnvelopeDotEnd = mEnvelopeRangeEnd;
            }
            return true;
        }

        private void changeCurve( CurveType curve )
        {
#if DEBUG
            sout.println( "TrackSelector#changCurve; getViewingCurveCount()=" + getViewingCurveCount() );
#endif
            if ( !mSelectedCurve.equals( curve ) ) {
                mLastSelectedCurve = mSelectedCurve;
                mSelectedCurve = curve;
                try {
#if JAVA
                    selectedCurveChangedEvent.raise( this, curve );
#elif QT_VERSION
                    selectedCurveChanged( this, curve );
#else
                    if ( SelectedCurveChanged != null ) {
                        SelectedCurveChanged.Invoke( this, curve );
                    }
#endif
                } catch ( Exception ex ) {
                    serr.println( "TrackSelector#changeCurve; ex=" + ex );
                }
            }
        }

        private static boolean isInRect( int x, int y, Rectangle rc )
        {
            if ( rc.x <= x && x <= rc.x + rc.width ) {
                if ( rc.y <= y && y <= rc.y + rc.height ) {
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
        private VsqEvent findItemAt( int locx, int locy )
        {
            if ( AppManager.getVsqFile() == null ) {
                return null;
            }
            VsqTrack target = AppManager.getVsqFile().Track.get( AppManager.getSelected() );
            int count = target.getEventCount();
            for ( int i = 0; i < count; i++ ) {
                VsqEvent ve = target.getEvent( i );
                if ( ve.ID.type == VsqIDType.Singer ) {
                    int x = AppManager.xCoordFromClocks( ve.Clock );
                    if ( getHeight() - 2 * OFFSET_TRACK_TAB <= locy &&
                         locy <= getHeight() - OFFSET_TRACK_TAB &&
                         x <= locx && locx <= x + SINGER_ITEM_WIDTH ) {
                        return ve;
                    } else if ( getWidth() < x ) {
                        //return null;
                    }
                } else if ( ve.ID.type == VsqIDType.Anote ) {
                    int x = AppManager.xCoordFromClocks( ve.Clock );
                    int y = 0;
                    if ( mSelectedCurve.equals( CurveType.VEL ) ) {
                        y = yCoordFromValue( ve.ID.Dynamics );
                    } else if ( mSelectedCurve.equals( CurveType.Accent ) ) {
                        y = yCoordFromValue( ve.ID.DEMaccent );
                    } else if ( mSelectedCurve.equals( CurveType.Decay ) ) {
                        y = yCoordFromValue( ve.ID.DEMdecGainRate );
                    } else {
                        continue;
                    }
                    if ( 0 <= locy && locy <= getHeight() - 2 * OFFSET_TRACK_TAB &&
                         AppManager.keyWidth <= locx && locx <= getWidth() ) {
                        if ( y <= locy && locy <= getHeight() - FOOTER && x <= locx && locx <= x + VEL_BAR_WIDTH ) {
                            return ve;
                        }
                    }
                }
            }
            return null;
        }

        public void TrackSelector_MouseUp( Object sender, BMouseEventArgs e )
        {
#if DEBUG
            AppManager.debugWriteLine( "TrackSelector_MouseUp" );
#endif
            mMouseDowned = false;
            if ( mMouseHoverThread != null ) {
#if JAVA
                if( mMouseHoverThread.isAlive() ){
                    mMouseHoverThread.stop();
                }
#else
                if ( mMouseHoverThread.IsAlive ) {
                    mMouseHoverThread.Abort();
                }
#endif
            }

            if ( !mCurveVisible ) {
                mMouseDownMode = MouseDownMode.NONE;
                invalidate();
                return;
            }

            int selected = AppManager.getSelected();
            boolean is_utau_mode = AppManager.mDrawIsUtau[selected - 1];
            int stdx = AppManager.getStartToDrawX();

            int max = mSelectedCurve.getMaximum();
            int min = mSelectedCurve.getMinimum();
            VsqFileEx vsq = AppManager.getVsqFile();
            VsqTrack vsq_track = vsq.Track.get( selected );
#if DEBUG
            AppManager.debugWriteLine( "    max,min=" + max + "," + min );
#endif
            if ( mMouseDownMode == MouseDownMode.BEZIER_ADD_NEW ||
                 mMouseDownMode == MouseDownMode.BEZIER_MODE ||
                 mMouseDownMode == MouseDownMode.BEZIER_EDIT ) {
                if ( e.Button == BMouseButtons.Left && sender is TrackSelector ) {
                    int chain_id = AppManager.itemSelection.getLastBezier().chainID;
                    BezierChain edited = (BezierChain)vsq.AttachedCurves.get( selected - 1 ).getBezierChain( mSelectedCurve, chain_id ).clone();
                    if ( mMouseDownMode == MouseDownMode.BEZIER_ADD_NEW ) {
                        edited.id = chain_id;
                        CadenciiCommand pre = VsqFileEx.generateCommandDeleteBezierChain( selected,
                                                                                          mSelectedCurve,
                                                                                          chain_id,
                                                                                          AppManager.editorConfig.getControlCurveResolutionValue() );
                        executeCommand( pre, false );
                        CadenciiCommand run = VsqFileEx.generateCommandAddBezierChain( selected,
                                                                                       mSelectedCurve,
                                                                                       chain_id,
                                                                                       AppManager.editorConfig.getControlCurveResolutionValue(),
                                                                                       edited );
                        executeCommand( run, true );
                    } else if ( mMouseDownMode == MouseDownMode.BEZIER_EDIT ) {
                        CadenciiCommand pre = VsqFileEx.generateCommandReplaceBezierChain( selected,
                                                                                           mSelectedCurve,
                                                                                           chain_id,
                                                                                           mEditingBezierOriginal,
                                                                                           AppManager.editorConfig.getControlCurveResolutionValue() );
                        executeCommand( pre, false );
                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( selected,
                                                                                           mSelectedCurve,
                                                                                           chain_id,
                                                                                           edited,
                                                                                           AppManager.editorConfig.getControlCurveResolutionValue() );
                        executeCommand( run, true );
                    } else if ( mMouseDownMode == MouseDownMode.BEZIER_MODE && mMouseMoved ) {
                        vsq.AttachedCurves.get( selected - 1 ).setBezierChain( mSelectedCurve, chain_id, mEditingBezierOriginal );
                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( selected,
                                                                                           mSelectedCurve,
                                                                                           chain_id,
                                                                                           edited,
                                                                                           AppManager.editorConfig.getControlCurveResolutionValue() );
                        executeCommand( run, true );
#if DEBUG
                        AppManager.debugWriteLine( "    m_mouse_down_mode=" + mMouseDownMode );
                        AppManager.debugWriteLine( "    chain_id=" + chain_id );
#endif

                    }
                }
            } else if ( mMouseDownMode == MouseDownMode.CURVE_EDIT ||
                      mMouseDownMode == MouseDownMode.VEL_WAIT_HOVER ) {
                if ( e.Button == BMouseButtons.Left ) {
                    if ( AppManager.getSelectedTool() == EditTool.ARROW ) {
                        #region Arrow
                        if ( mSelectedCurve.equals( CurveType.Env ) ) {

                        } else if ( !mSelectedCurve.equals( CurveType.VEL ) && !mSelectedCurve.equals( CurveType.Accent ) && !mSelectedCurve.equals( CurveType.Decay ) ) {
                            if ( AppManager.mCurveSelectingRectangle.width == 0 ) {
                                AppManager.setCurveSelectedIntervalEnabled( false );
                            } else {
                                if ( !AppManager.isCurveSelectedIntervalEnabled() ) {
                                    int start = Math.Min( AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width );
                                    int end = Math.Max( AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width );
                                    AppManager.mCurveSelectedInterval = new SelectedRegion( start );
                                    AppManager.mCurveSelectedInterval.setEnd( end );
#if DEBUG
                                    AppManager.debugWriteLine( "TrackSelector#TrackSelector_MouseUp; selected_region is set to TRUE" );
#endif
                                    AppManager.setCurveSelectedIntervalEnabled( true );
                                } else {
                                    int start = Math.Min( AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width );
                                    int end = Math.Max( AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width );
                                    int old_start = AppManager.mCurveSelectedInterval.getStart();
                                    int old_end = AppManager.mCurveSelectedInterval.getEnd();
                                    AppManager.mCurveSelectedInterval = new SelectedRegion( Math.Min( start, old_start ) );
                                    AppManager.mCurveSelectedInterval.setEnd( Math.Max( end, old_end ) );
                                }

                                if ( (mModifierOnMouseDown & InputEvent.CTRL_MASK) != InputEvent.CTRL_MASK ) {
#if DEBUG
                                    sout.println( "TrackSelector#TrackSelector_MouseUp; CTRL was not pressed" );
#endif
                                    AppManager.itemSelection.clearPoint();
                                }
                                if ( !mSelectedCurve.equals( CurveType.Accent ) &&
                                     !mSelectedCurve.equals( CurveType.Decay ) &&
                                     !mSelectedCurve.equals( CurveType.Env ) &&
                                     !mSelectedCurve.equals( CurveType.VEL ) &&
                                     !mSelectedCurve.equals( CurveType.VibratoDepth ) &&
                                     !mSelectedCurve.equals( CurveType.VibratoRate ) ) {
                                    VsqBPList list = vsq_track.getCurve( mSelectedCurve.getName() );
                                    int count = list.size();
                                    Rectangle rc = new Rectangle( Math.Min( AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width ),
                                                                  Math.Min( AppManager.mCurveSelectingRectangle.y, AppManager.mCurveSelectingRectangle.y + AppManager.mCurveSelectingRectangle.height ),
                                                                  Math.Abs( AppManager.mCurveSelectingRectangle.width ),
                                                                  Math.Abs( AppManager.mCurveSelectingRectangle.height ) );
#if DEBUG
                                    sout.println( "TrackSelectro#TrackSelectro_MouseUp; rc={x=" + rc.x + ", y=" + rc.y + ", width=" + rc.width + ", height=" + rc.height + "}" );
#endif
                                    for ( int i = 0; i < count; i++ ) {
                                        int clock = list.getKeyClock( i );
                                        VsqBPPair item = list.getElementB( i );
                                        if ( isInRect( clock, item.value, rc ) ) {
#if DEBUG
                                            sout.println( "TrackSelector#TrackSelectro_MosueUp; selected; clock=" + clock + "; id=" + item.id );
#endif
                                            AppManager.itemSelection.addPoint( mSelectedCurve, item.id );
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    } else if ( AppManager.getSelectedTool() == EditTool.ERASER ) {
                        #region Eraser
                        if ( AppManager.isCurveMode() ) {
                            Vector<BezierChain> list = vsq.AttachedCurves.get( selected - 1 ).get( mSelectedCurve );
                            if ( list != null ) {
                                int x = Math.Min( AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width );
                                int y = Math.Min( AppManager.mCurveSelectingRectangle.y, AppManager.mCurveSelectingRectangle.y + AppManager.mCurveSelectingRectangle.height );
                                Rectangle rc = new Rectangle( x, y, Math.Abs( AppManager.mCurveSelectingRectangle.width ), Math.Abs( AppManager.mCurveSelectingRectangle.height ) );

                                boolean changed = false; //1箇所でも削除が実行されたらtrue

                                int count = list.size();
                                Vector<BezierChain> work = new Vector<BezierChain>();
                                for ( int i = 0; i < count; i++ ) {
                                    BezierChain chain = list.get( i );
                                    BezierChain chain_copy = new BezierChain();
                                    chain_copy.setColor( chain.getColor() );
                                    chain_copy.Default = chain.Default;
                                    chain_copy.id = chain.id;
                                    int point_count = chain.points.size();
                                    for ( int j = 0; j < point_count; j++ ) {
                                        BezierPoint point = chain.points.get( j );
                                        Point basepoint = point.getBase().toPoint();
                                        Point ctrl_l = point.getControlLeft().toPoint();
                                        Point ctrl_r = point.getControlRight().toPoint();
                                        if ( isInRect( basepoint.x, basepoint.y, rc ) ) {
                                            // データ点が選択範囲に入っているので、追加しない
                                            changed = true;
                                            continue;
                                        } else {
                                            if ( (point.getControlLeftType() != BezierControlType.None && isInRect( ctrl_l.x, ctrl_l.y, rc )) ||
                                                 (point.getControlRightType() != BezierControlType.None && isInRect( ctrl_r.x, ctrl_r.y, rc )) ) {
                                                // 制御点が選択範囲に入っているので、「滑らかにする」オプションを解除して追加
                                                BezierPoint point_copy = (BezierPoint)point.clone();
                                                point_copy.setControlLeftType( BezierControlType.None );
                                                point_copy.setControlRightType( BezierControlType.None );
                                                chain_copy.points.add( point_copy );
                                                changed = true;
                                                continue;
                                            } else {
                                                // 選択範囲に入っていないので、普通に追加
                                                chain_copy.points.add( (BezierPoint)point.clone() );
                                            }
                                        }
                                    }
                                    if ( chain_copy.points.size() > 0 ) {
                                        work.add( chain_copy );
                                    }
                                }
                                if ( changed ) {
                                    TreeMap<CurveType, Vector<BezierChain>> comm = new TreeMap<CurveType, Vector<BezierChain>>();
                                    comm.put( mSelectedCurve, work );
                                    CadenciiCommand run = VsqFileEx.generateCommandReplaceAttachedCurveRange( selected, comm );
                                    executeCommand( run, true );
                                }
                            }
                        } else {
                            if ( mSelectedCurve.equals( CurveType.VEL ) || mSelectedCurve.equals( CurveType.Accent ) || mSelectedCurve.equals( CurveType.Decay ) ) {
                                #region VEL Accent Delay
                                int start = Math.Min( AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width );
                                int end = Math.Max( AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width );
                                int old_start = AppManager.mCurveSelectedInterval.getStart();
                                int old_end = AppManager.mCurveSelectedInterval.getEnd();
                                AppManager.mCurveSelectedInterval = new SelectedRegion( Math.Min( start, old_start ) );
                                AppManager.mCurveSelectedInterval.setEnd( Math.Max( end, old_end ) );
                                AppManager.itemSelection.clearEvent();
                                Vector<Integer> deleting = new Vector<Integer>();
                                for ( Iterator<VsqEvent> itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ev = itr.next();
                                    if ( start <= ev.Clock && ev.Clock <= end ) {
                                        deleting.add( ev.InternalID );
                                    }
                                }
                                if ( deleting.size() > 0 ) {
                                    CadenciiCommand er_run = new CadenciiCommand(
                                        VsqCommand.generateCommandEventDeleteRange( selected, deleting ) );
                                    executeCommand( er_run, true );
                                }
                                #endregion
                            } else if ( mSelectedCurve.equals( CurveType.VibratoRate ) || mSelectedCurve.equals( CurveType.VibratoDepth ) ) {
                                #region VibratoRate ViratoDepth
                                int er_start = Math.Min( AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width );
                                int er_end = Math.Max( AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width );
                                Vector<Integer> internal_ids = new Vector<Integer>();
                                Vector<VsqID> items = new Vector<VsqID>();
                                for ( Iterator<VsqEvent> itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ve = itr.next();
                                    if ( ve.ID.VibratoHandle == null ) {
                                        continue;
                                    }
                                    int cl_vib_start = ve.Clock + ve.ID.VibratoDelay;
                                    int cl_vib_length = ve.ID.getLength() - ve.ID.VibratoDelay;
                                    int cl_vib_end = cl_vib_start + cl_vib_length;
                                    int clear_start = int.MaxValue;
                                    int clear_end = int.MinValue;
                                    if ( er_start < cl_vib_start && cl_vib_start < er_end && er_end <= cl_vib_end ) {
                                        // cl_vib_startからer_endまでをリセット
                                        clear_start = cl_vib_start;
                                        clear_end = er_end;
                                    } else if ( cl_vib_start <= er_start && er_end <= cl_vib_end ) {
                                        // er_startからer_endまでをリセット
                                        clear_start = er_start;
                                        clear_end = er_end;
                                    } else if ( cl_vib_start < er_start && er_start < cl_vib_end && cl_vib_end < er_end ) {
                                        // er_startからcl_vib_endまでをリセット
                                        clear_start = er_start;
                                        clear_end = cl_vib_end;
                                    } else if ( er_start < cl_vib_start && cl_vib_end < er_end ) {
                                        // 全部リセット
                                        clear_start = cl_vib_start;
                                        clear_end = cl_vib_end;
                                    }
                                    if ( clear_start < clear_end ) {
                                        float f_clear_start = (clear_start - cl_vib_start) / (float)cl_vib_length;
                                        float f_clear_end = (clear_end - cl_vib_start) / (float)cl_vib_length;
                                        VsqID item = (VsqID)ve.ID.clone();
                                        VibratoBPList target = null;
                                        if ( mSelectedCurve.equals( CurveType.VibratoDepth ) ) {
                                            target = item.VibratoHandle.getDepthBP();
                                        } else {
                                            target = item.VibratoHandle.getRateBP();
                                        }
                                        Vector<Float> bpx = new Vector<Float>();
                                        Vector<Integer> bpy = new Vector<Integer>();
                                        boolean start_added = false;
                                        boolean end_added = false;
                                        for ( int i = 0; i < target.getCount(); i++ ) {
                                            VibratoBPPair vbpp = target.getElement( i );
                                            if ( vbpp.X < f_clear_start ) {
                                                bpx.add( vbpp.X );
                                                bpy.add( vbpp.Y );
                                            } else if ( f_clear_start == vbpp.X ) {
                                                bpx.add( vbpp.X );
                                                bpy.add( 64 );
                                                start_added = true;
                                            } else if ( f_clear_start < vbpp.X && !start_added ) {
                                                bpx.add( f_clear_start );
                                                bpy.add( 64 );
                                                start_added = true;
                                            } else if ( f_clear_end == vbpp.X ) {
                                                bpx.add( vbpp.X );
                                                bpy.add( vbpp.Y );
                                                end_added = true;
                                            } else if ( f_clear_end < vbpp.X && !end_added ) {
                                                int y = vbpp.Y;
                                                if ( i > 0 ) {
                                                    y = target.getElement( i - 1 ).Y;
                                                }
                                                bpx.add( f_clear_end );
                                                bpy.add( y );
                                                end_added = true;
                                                bpx.add( vbpp.X );
                                                bpy.add( vbpp.Y );
                                            } else if ( f_clear_end < vbpp.X ) {
                                                bpx.add( vbpp.X );
                                                bpy.add( vbpp.Y );
                                            }
                                        }
                                        if ( mSelectedCurve.equals( CurveType.VibratoDepth ) ) {
                                            item.VibratoHandle.setDepthBP(
                                                new VibratoBPList(
                                                    PortUtil.convertFloatArray( bpx.toArray( new Float[] { } ) ),
                                                    PortUtil.convertIntArray( bpy.toArray( new Integer[] { } ) ) ) );
                                        } else {
                                            item.VibratoHandle.setRateBP(
                                                new VibratoBPList(
                                                    PortUtil.convertFloatArray( bpx.toArray( new Float[] { } ) ),
                                                    PortUtil.convertIntArray( bpy.toArray( new Integer[] { } ) ) ) );
                                        }
                                        internal_ids.add( ve.InternalID );
                                        items.add( item );
                                    }
                                }
                                CadenciiCommand run = new CadenciiCommand(
                                    VsqCommand.generateCommandEventChangeIDContaintsRange( selected,
                                                                                           PortUtil.convertIntArray( internal_ids.toArray( new Integer[] { } ) ),
                                                                                           items.toArray( new VsqID[] { } ) ) );
                                executeCommand( run, true );
                                #endregion
                            } else if ( mSelectedCurve.equals( CurveType.Env ) ) {

                            } else {
                                #region Other Curves
                                VsqBPList work = vsq_track.getCurve( mSelectedCurve.getName() );

                                // 削除するべきデータ点のリストを作成
                                int x = Math.Min( AppManager.mCurveSelectingRectangle.x, AppManager.mCurveSelectingRectangle.x + AppManager.mCurveSelectingRectangle.width );
                                int y = Math.Min( AppManager.mCurveSelectingRectangle.y, AppManager.mCurveSelectingRectangle.y + AppManager.mCurveSelectingRectangle.height );
                                Rectangle rc = new Rectangle( x, y, Math.Abs( AppManager.mCurveSelectingRectangle.width ), Math.Abs( AppManager.mCurveSelectingRectangle.height ) );
                                Vector<Long> delete = new Vector<Long>();
                                int count = work.size();
                                for ( int i = 0; i < count; i++ ) {
                                    int clock = work.getKeyClock( i );
                                    VsqBPPair item = work.getElementB( i );
                                    if ( isInRect( clock, item.value, rc ) ) {
                                        delete.add( item.id );
                                    }
                                }

                                if ( delete.size() > 0 ) {
                                    CadenciiCommand run_eraser = new CadenciiCommand(
                                        VsqCommand.generateCommandTrackCurveEdit2( selected, mSelectedCurve.getName(), delete, new TreeMap<Integer, VsqBPPair>() ) );
                                    executeCommand( run_eraser, true );
                                }
                                #endregion
                            }
                        }
                        #endregion
                    } else if ( !AppManager.isCurveMode() && (AppManager.getSelectedTool() == EditTool.PENCIL || AppManager.getSelectedTool() == EditTool.LINE) ) {
                        #region Pencil & Line
                        mMouseTracer.append( e.X + stdx, e.Y );
                        if ( mPencilMoved ) {
                            if ( mSelectedCurve.equals( CurveType.VEL ) || mSelectedCurve.equals( CurveType.Accent ) || mSelectedCurve.equals( CurveType.Decay ) ) {
                                #region VEL Accent Decay
                                int start = mMouseTracer.firstKey();
                                int end = mMouseTracer.lastKey();
                                start = AppManager.clockFromXCoord( start - stdx );
                                end = AppManager.clockFromXCoord( end - stdx );
#if DEBUG
                                AppManager.debugWriteLine( "        start=" + start );
                                AppManager.debugWriteLine( "        end=" + end );
#endif
                                TreeMap<Integer, Integer> velocity = new TreeMap<Integer, Integer>();
                                for ( Iterator<VsqEvent> itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ve = itr.next();
                                    if ( start <= ve.Clock && ve.Clock < end ) {
                                        int i = -1;
                                        int lkey = 0;
                                        int lvalue = 0;
                                        int count = mMouseTracer.size();
                                        for ( Iterator<Point> itr2 = mMouseTracer.iterator(); itr2.hasNext(); ) {
                                            i++;
                                            Point p = itr2.next();
                                            int key = p.x;
                                            int value = p.y;
                                            if ( i == 0 ) {
                                                lkey = key;
                                                lvalue = value;
                                                continue;
                                            }
                                            int key0 = lkey;
                                            int key1 = key;
                                            int key0_clock = AppManager.clockFromXCoord( key0 - stdx );
                                            int key1_clock = AppManager.clockFromXCoord( key1 - stdx );
#if DEBUG
                                            AppManager.debugWriteLine( "        key0,key1=" + key0 + "," + key1 );
#endif
                                            if ( key0_clock < ve.Clock && ve.Clock < key1_clock ) {
                                                int key0_value = valueFromYCoord( lvalue );
                                                int key1_value = valueFromYCoord( value );
                                                float a = (key1_value - key0_value) / (float)(key1_clock - key0_clock);
                                                float b = key0_value - a * key0_clock;
                                                int new_value = (int)(a * ve.Clock + b);
                                                velocity.put( ve.InternalID, new_value );
                                            } else if ( key0_clock == ve.Clock ) {
                                                velocity.put( ve.InternalID, valueFromYCoord( lvalue ) );
                                            } else if ( key1_clock == ve.Clock ) {
                                                velocity.put( ve.InternalID, valueFromYCoord( value ) );
                                            }
                                            lkey = key;
                                            lvalue = value;
                                        }
                                    }
                                }
                                if ( velocity.size() > 0 ) {
                                    Vector<ValuePair<Integer, Integer>> cpy = new Vector<ValuePair<Integer, Integer>>();
                                    for ( Iterator<Integer> itr = velocity.keySet().iterator(); itr.hasNext(); ) {
                                        int internal_id = itr.next();
                                        int value = (Integer)velocity.get( internal_id );
                                        cpy.add( new ValuePair<Integer, Integer>( internal_id, value ) );
                                    }
                                    CadenciiCommand run = null;
                                    if ( mSelectedCurve.equals( CurveType.VEL ) ) {
                                        if ( is_utau_mode ) {
                                            int size = velocity.size();
                                            VsqEvent[] events = new VsqEvent[size];
                                            int i = 0;
                                            for ( Iterator<Integer> itr = velocity.keySet().iterator(); itr.hasNext(); ){
                                                int internal_id = itr.next();
                                                VsqEvent item = (VsqEvent)vsq_track.findEventFromID( internal_id ).clone();
                                                if ( item.UstEvent == null ) {
                                                    item.UstEvent = new UstEvent();
                                                }
                                                item.UstEvent.setIntensity( velocity.get( internal_id ) );
                                                events[i] = item;
                                                i++;
                                            }
                                            run = new CadenciiCommand(
                                                VsqCommand.generateCommandEventReplaceRange(
                                                    selected, events ) );
                                        } else {
                                            run = new CadenciiCommand( 
                                                VsqCommand.generateCommandEventChangeVelocity( selected, cpy ) );
                                        }
                                    } else if ( mSelectedCurve.equals( CurveType.Accent ) ) {
                                        run = new CadenciiCommand(
                                            VsqCommand.generateCommandEventChangeAccent( selected, cpy ) );
                                    } else if ( mSelectedCurve.equals( CurveType.Decay ) ) {
                                        run = new CadenciiCommand(
                                            VsqCommand.generateCommandEventChangeDecay( selected, cpy ) );
                                    }
                                    executeCommand( run, true );
                                }
                                #endregion
                            } else if ( mSelectedCurve.equals( CurveType.VibratoRate ) || mSelectedCurve.equals( CurveType.VibratoDepth ) ) {
                                #region VibratoRate || VibratoDepth
                                int step_clock = AppManager.editorConfig.getControlCurveResolutionValue();
                                int step_px = (int)(step_clock * AppManager.getScaleX());
                                if ( step_px <= 0 ) {
                                    step_px = 1;
                                }
                                int start = mMouseTracer.firstKey();
                                int end = mMouseTracer.lastKey();
#if DEBUG
                                AppManager.debugWriteLine( "    start,end=" + start + " " + end );
#endif
                                Vector<Integer> internal_ids = new Vector<Integer>();
                                Vector<VsqID> items = new Vector<VsqID>();
                                for ( Iterator<VsqEvent> itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ve = itr.next();
                                    if ( ve.ID.VibratoHandle == null ) {
                                        continue;
                                    }
                                    int cl_vib_start = ve.Clock + ve.ID.VibratoDelay;
                                    float cl_vib_length = ve.ID.getLength() - ve.ID.VibratoDelay;

                                    // 仮想スクリーン上の、ビブラートの描画開始位置
                                    int vib_start = AppManager.xCoordFromClocks( cl_vib_start ) + stdx;

                                    // 仮想スクリーン上の、ビブラートの描画終了位置
                                    int vib_end = AppManager.xCoordFromClocks( ve.Clock + ve.ID.getLength() ) + stdx;

                                    // マウスのトレースと、ビブラートの描画範囲がオーバーラップしている部分を検出
                                    int chk_start = Math.Max( vib_start, start );
                                    int chk_end = Math.Min( vib_end, end );
                                    if ( chk_end <= chk_start ) {
                                        // オーバーラップしていないのでスキップ
                                        continue;
                                    }

                                    float add_min = (AppManager.clockFromXCoord( chk_start - stdx ) - cl_vib_start) / cl_vib_length;
                                    float add_max = (AppManager.clockFromXCoord( chk_end - stdx ) - cl_vib_start) / cl_vib_length;

                                    Vector<ValuePair<Float, Integer>> edit = new Vector<ValuePair<Float, Integer>>();
                                    int lclock = -2 * step_clock;
                                    ValuePair<Float, Integer> first = null; // xの値が0以下の最大のデータ点
                                    ValuePair<Float, Integer> last = null;//xの値が1以上の最小のデータ点
                                    for ( Iterator<Point> itr2 = mMouseTracer.iterator(); itr2.hasNext(); ) {
                                        Point p = itr2.next();
                                        if ( p.x < chk_start ) {
                                            continue;
                                        } else if ( chk_end < p.x ) {
                                            break;
                                        }
                                        int clock = AppManager.clockFromXCoord( p.x - stdx );
                                        if ( clock - lclock < step_clock ) {
                                            continue;
                                        }
                                        int val = valueFromYCoord( p.y );
                                        if ( val < min ) {
                                            val = min;
                                        } else if ( max < val ) {
                                            val = max;
                                        }
                                        float x = (clock - cl_vib_start) / cl_vib_length;
                                        ValuePair<Float, Integer> tmp = new ValuePair<Float, Integer>( x, val );
                                        if ( 0.0f < x && x < 1.0f ) {
                                            edit.add( tmp );
                                        } else if ( x <= 0.0f ) {
                                            first = tmp;
                                        } else if ( 1.0f <= x && last != null ) {
                                            last = tmp;
                                        }
                                        lclock = clock;
                                    }
                                    if ( first != null ) {
                                        first.setKey( 0.0f );
                                        edit.add( first );
                                    }
                                    if ( last != null ) {
                                        last.setKey( 1.0f );
                                        edit.add( last );
                                    }

                                    VibratoBPList target = null;
                                    if ( mSelectedCurve.equals( CurveType.VibratoRate ) ) {
                                        target = ve.ID.VibratoHandle.getRateBP();
                                    } else {
                                        target = ve.ID.VibratoHandle.getDepthBP();
                                    }
                                    if ( target.getCount() > 0 ) {
                                        for ( int i = 0; i < target.getCount(); i++ ) {
                                            if ( target.getElement( i ).X < add_min || add_max < target.getElement( i ).X ) {
                                                edit.add( new ValuePair<Float, Integer>( target.getElement( i ).X,
                                                                                         target.getElement( i ).Y ) );
                                            }
                                        }
                                    }
                                    Collections.sort( edit );
                                    VsqID id = (VsqID)ve.ID.clone();
                                    float[] bpx = new float[edit.size()];
                                    int[] bpy = new int[edit.size()];
                                    for ( int i = 0; i < edit.size(); i++ ) {
                                        bpx[i] = edit.get( i ).getKey();
                                        bpy[i] = edit.get( i ).getValue();
                                    }
                                    if ( mSelectedCurve.equals( CurveType.VibratoDepth ) ) {
                                        id.VibratoHandle.setDepthBP( new VibratoBPList( bpx, bpy ) );
                                    } else {
                                        id.VibratoHandle.setRateBP( new VibratoBPList( bpx, bpy ) );
                                    }
                                    internal_ids.add( ve.InternalID );
                                    items.add( id );
                                }
                                if ( internal_ids.size() > 0 ) {
                                    CadenciiCommand run = new CadenciiCommand(
                                        VsqCommand.generateCommandEventChangeIDContaintsRange( selected,
                                                                                               PortUtil.convertIntArray( internal_ids.toArray( new Integer[] { } ) ),
                                                                                               items.toArray( new VsqID[] { } ) ) );
                                    executeCommand( run, true );
                                }
                                #endregion
                            } else if ( mSelectedCurve.equals( CurveType.Env ) ) {
                                #region Env

                                #endregion
                            } else {
                                #region Other Curves
                                int track = selected;
                                int step_clock = AppManager.editorConfig.getControlCurveResolutionValue();
                                int step_px = (int)(step_clock * AppManager.getScaleX());
                                if ( step_px <= 0 ) {
                                    step_px = 1;
                                }
                                int start = mMouseTracer.firstKey();
                                int end = mMouseTracer.lastKey();
                                int clock_start = AppManager.clockFromXCoord( start - stdx );
                                int clock_end = AppManager.clockFromXCoord( end - stdx );
                                int last = start;

#if DEBUG
                                sout.println( "TrackSelector#TrackSelector_MouseUp; start, end=" + start + ", " + end );
#endif
                                VsqBPList list = vsq.Track.get( track ).getCurve( mSelectedCurve.getName() );
                                long maxid = list.getMaxID();

                                // 削除するものを列挙
                                Vector<Long> delete = new Vector<Long>();
                                int c = list.size();
                                for ( int i = 0; i < c; i++ ) {
                                    int clock = list.getKeyClock( i );
                                    if ( clock_start <= clock && clock <= clock_end ) {
                                        delete.add( list.getElementB( i ).id );
                                    } else if ( clock_end < clock ) {
                                        break;
                                    }
                                }

                                TreeMap<Integer, VsqBPPair> add = new TreeMap<Integer, VsqBPPair>();
                                int lvalue = int.MinValue;
                                int lclock = -2 * step_clock;
                                int index = 0;
                                for ( Iterator<Point> itr = mMouseTracer.iterator(); itr.hasNext(); ) {
                                    Point p = itr.next();
                                    if ( p.x < start ) {
                                        continue;
                                    } else if ( end < p.x ) {
                                        break;
                                    }
                                    int clock = AppManager.clockFromXCoord( p.x - stdx );
                                    if ( clock - lclock < step_clock ) {
                                        continue;
                                    }
                                    int value = valueFromYCoord( p.y );
                                    if ( value < min ) {
                                        value = min;
                                    } else if ( max < value ) {
                                        value = max;
                                    }
                                    if ( value != lvalue ) {
                                        index++;
                                        add.put( clock, new VsqBPPair( value, maxid + index ) );
                                        lvalue = value;
                                        lclock = clock;
                                    }
                                }

                                // clock_endでの値
                                int valueAtEnd = list.getValue( clock_end );
                                if ( add.containsKey( clock_end ) ) {
                                    VsqBPPair v = add.get( clock_end );
                                    v.value = valueAtEnd;
                                    add.put( clock_end, v );
                                } else {
                                    index++;
                                    add.put( clock_end, new VsqBPPair( valueAtEnd, maxid + index ) );
                                }

                                CadenciiCommand pen_run = new CadenciiCommand(
                                    VsqCommand.generateCommandTrackCurveEdit2( track, mSelectedCurve.getName(), delete, add ) );
                                executeCommand( pen_run, true );
                                #endregion
                            }
                        }
                        mMouseTracer.clear();
                        #endregion
                    }
                }
                mMouseDowned = false;
            } else if ( mMouseDownMode == MouseDownMode.SINGER_LIST ) {
                if ( mMouseMoved ) {
                    int count = AppManager.itemSelection.getEventCount();
                    if ( count > 0 ) {
                        int[] ids = new int[count];
                        int[] clocks = new int[count];
                        VsqID[] values = new VsqID[count];
                        int i = -1;
                        boolean is_valid = true;
                        boolean contains_first_singer = false;
                        int premeasure = vsq.getPreMeasureClocks();
                        for ( Iterator<SelectedEventEntry> itr = AppManager.itemSelection.getEventIterator(); itr.hasNext(); ) {
                            SelectedEventEntry item = itr.next();
                            i++;
                            ids[i] = item.original.InternalID;
                            clocks[i] = item.editing.Clock;
                            values[i] = item.original.ID;
                            if ( clocks[i] < premeasure ) {
                                is_valid = false;
                                // breakしてはいけない。clock=0のを確実に検出するため
                            }
                            if ( item.original.Clock == 0 ) {
                                contains_first_singer = true;
                                break;
                            }
                        }
                        if ( contains_first_singer ) {
#if !JAVA
                            System.Media.SystemSounds.Asterisk.Play();
#endif
                        } else {
                            if ( !is_valid ) {
                                int tmin = clocks[0];
                                for ( int j = 1; j < count; j++ ) {
                                    tmin = Math.Min( tmin, clocks[j] );
                                }
                                int dclock = premeasure - tmin;
                                for ( int j = 0; j < count; j++ ) {
                                    clocks[j] += dclock;
                                }
#if !JAVA
                                System.Media.SystemSounds.Asterisk.Play();
#endif
                            }
                            boolean changed = false;
                            for ( int j = 0; j < ids.Length; j++ ) {
                                for ( Iterator<SelectedEventEntry> itr = AppManager.itemSelection.getEventIterator(); itr.hasNext(); ) {
                                    SelectedEventEntry item = itr.next();
                                    if ( item.original.InternalID == ids[j] && item.original.Clock != clocks[j] ) {
                                        changed = true;
                                        break;
                                    }
                                }
                                if ( changed ) {
                                    break;
                                }
                            }
                            if ( changed ) {
                                CadenciiCommand run = new CadenciiCommand(
                                    VsqCommand.generateCommandEventChangeClockAndIDContaintsRange( selected, ids, clocks, values ) );
                                executeCommand( run, true );
                            }
                        }
                    }
                }
            } else if ( mMouseDownMode == MouseDownMode.VEL_EDIT ) {
                if ( mSelectedCurve.equals( CurveType.VEL ) && is_utau_mode ) {
                    int count = mVelEditSelected.size();
                    VsqEvent[] values = new VsqEvent[count];
                    int i = 0;
                    for ( Iterator<Integer> itr = mVelEditSelected.keySet().iterator(); itr.hasNext(); ) {
                        int internal_id = itr.next();
                        VsqEvent item = (VsqEvent)vsq_track.findEventFromID( internal_id ).clone();
                        if ( item.UstEvent == null ) {
                            item.UstEvent = new UstEvent();
                        }
                        item.UstEvent.setIntensity( mVelEditSelected.get( internal_id ).editing.UstEvent.getIntensity() );
                        values[i] = item;
                        i++;
                    }
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventReplaceRange( selected, values ) );
                    executeCommand( run, true );
                } else {
                    int count = mVelEditSelected.size();
                    int[] ids = new int[count];
                    VsqID[] values = new VsqID[count];
                    int i = -1;
                    for ( Iterator<Integer> itr = mVelEditSelected.keySet().iterator(); itr.hasNext(); ) {
                        int id = itr.next();
                        i++;
                        ids[i] = id;
                        values[i] = (VsqID)mVelEditSelected.get( id ).editing.ID.clone();
                    }
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventChangeIDContaintsRange( selected, ids, values ) );
                    executeCommand( run, true );
                }
                if ( mVelEditSelected.size() == 1 ) {
                    AppManager.itemSelection.clearEvent();
                    AppManager.itemSelection.addEvent( mVelEditLastSelectedID );
                }
            } else if ( mMouseDownMode == MouseDownMode.ENVELOPE_MOVE ) {
                mMouseDownMode = MouseDownMode.NONE;
                if ( mMouseMoved ) {
                    VsqTrack target = vsq_track;
                    VsqEvent edited = (VsqEvent)target.findEventFromID( mEnvelopeEdigintID ).clone();

                    // m_envelope_originalに，編集前のが入っているので，いったん置き換え
                    int count = target.getEventCount();
                    for ( int i = 0; i < count; i++ ) {
                        VsqEvent item = target.getEvent( i );
                        if ( item.ID.type == VsqIDType.Anote && item.InternalID == mEnvelopeEdigintID ) {
                            item.UstEvent.setEnvelope( mEnvelopeOriginal );
                            target.setEvent( i, item );
                            break;
                        }
                    }

                    // コマンドを発行
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventReplace( selected,
                                                                                                       edited ) );
                    executeCommand( run, true );
                }
            } else if ( mMouseDownMode == MouseDownMode.PRE_UTTERANCE_MOVE ) {
                mMouseDownMode = MouseDownMode.NONE;
                if ( mMouseMoved ) {
                    VsqTrack target = vsq_track;
                    VsqEvent edited = (VsqEvent)target.findEventFromID( mPreUtteranceEditingID ).clone();

                    // m_envelope_originalに，編集前のが入っているので，いったん置き換え
                    int count = target.getEventCount();
                    for ( int i = 0; i < count; i++ ) {
                        VsqEvent item = target.getEvent( i );
                        if ( item.ID.type == VsqIDType.Anote && item.InternalID == mPreUtteranceEditingID ) {
                            target.setEvent( i, mPreOverlapOriginal );
                            break;
                        }
                    }

                    // コマンドを発行
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventReplace( selected,
                                                                                                       edited ) );
                    executeCommand( run, true );
                }
            } else if ( mMouseDownMode == MouseDownMode.OVERLAP_MOVE ) {
                mMouseDownMode = MouseDownMode.NONE;
                if ( mMouseMoved ) {
                    VsqTrack target = vsq_track;
                    VsqEvent edited = (VsqEvent)target.findEventFromID( mOverlapEditingID ).clone();

                    // m_envelope_originalに，編集前のが入っているので，いったん置き換え
                    int count = target.getEventCount();
                    for ( int i = 0; i < count; i++ ) {
                        VsqEvent item = target.getEvent( i );
                        if ( item.ID.type == VsqIDType.Anote && item.InternalID == mOverlapEditingID ) {
                            target.setEvent( i, mPreOverlapOriginal );
                            break;
                        }
                    }

                    // コマンドを発行
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventReplace( selected,
                                                                                                       edited ) );
                    executeCommand( run, true );
                }
            } else if ( mMouseDownMode == MouseDownMode.POINT_MOVE ) {
                if ( mMouseMoved ) {
                    Point pmouse = pointToClient( PortUtil.getMousePosition() );
                    Point mouse = new Point( pmouse.x, pmouse.y );
                    int dx = mouse.x + AppManager.getStartToDrawX() - mMouseDownLocation.x;
                    int dy = mouse.y - mMouseDownLocation.y;

                    String curve = mSelectedCurve.getName();
                    VsqTrack work = (VsqTrack)vsq_track.clone();
                    VsqBPList list = work.getCurve( curve );
                    VsqBPList work_list = (VsqBPList)list.clone();
                    int min0 = list.getMinimum();
                    int max0 = list.getMaximum();
                    int count = list.size();
                    for ( int i = 0; i < count; i++ ) {
                        int clock = list.getKeyClock( i );
                        VsqBPPair item = list.getElementB( i );
                        if ( AppManager.itemSelection.isPointContains( item.id ) ) {
                            int x = AppManager.xCoordFromClocks( clock ) + dx + 1;
                            int y = yCoordFromValue( item.value ) + dy - 1;

                            int nclock = AppManager.clockFromXCoord( x );
                            int nvalue = valueFromYCoord( y );
                            if ( nvalue < min0 ) {
                                nvalue = min0;
                            }
                            if ( max0 < nvalue ) {
                                nvalue = max0;
                            }
                            work_list.move( clock, nclock, nvalue );
                        }
                    }
                    work.setCurve( curve, work_list );
                    BezierCurves beziers = vsq.AttachedCurves.get( selected - 1 );
                    CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( selected, work, beziers );
                    executeCommand( run, true );
                }
                mMovingPoints.clear();
            }
            mMouseDownMode = MouseDownMode.NONE;
#if JAVA
            repaint();
#else
            invalidate();
#endif
        }

        public void TrackSelector_MouseHover( Object sender, BEventArgs e )
        {
#if DEBUG
            AppManager.debugWriteLine( "TrackSelector_MouseHover" );
            AppManager.debugWriteLine( "    m_mouse_downed=" + mMouseDowned );
#endif
            //if ( m_selected_curve.equals( CurveType.Accent ) || m_selected_curve.equals( CurveType.Decay ) || m_selected_curve.equals( CurveType.Env ) ) {
            if ( mSelectedCurve.equals( CurveType.Env ) ) {
                return;
            }
            if ( mMouseDowned && !mPencilMoved && AppManager.getSelectedTool() == EditTool.PENCIL &&
                 !mSelectedCurve.equals( CurveType.VEL ) ) {
                Point pmouse = pointToClient( PortUtil.getMousePosition() );
                Point mouse = new Point( pmouse.x, pmouse.y );
                int clock = AppManager.clockFromXCoord( mouse.x );
                int value = valueFromYCoord( mouse.y );
                int min = mSelectedCurve.getMinimum();
                int max = mSelectedCurve.getMaximum();

                int selected = AppManager.getSelected();
                VsqFileEx vsq = AppManager.getVsqFile();
                VsqTrack vsq_track = vec.get( vsq.Track, selected );

                if ( value < min ) {
                    value = min;
                } else if ( max < value ) {
                    value = max;
                }
                if ( mSelectedCurve.equals( CurveType.VibratoRate ) || mSelectedCurve.equals( CurveType.VibratoDepth ) ) {
                    // マウスの位置がビブラートの範囲かどうかを調べる
                    float x = -1f;
                    VsqID edited = null;
                    int event_id = -1;
                    for ( Iterator<VsqEvent> itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {
                        VsqEvent ve = itr.next();
                        if ( ve.ID.VibratoHandle == null ) {
                            continue;
                        }
                        int cl_vib_start = ve.Clock + ve.ID.VibratoDelay;
                        int cl_vib_end = ve.Clock + ve.ID.getLength();
                        if ( cl_vib_start <= clock && clock < cl_vib_end ) {
                            x = (clock - cl_vib_start) / (float)(cl_vib_end - cl_vib_start);
                            edited = (VsqID)ve.ID.clone();
                            event_id = ve.InternalID;
                            break;
                        }
                    }
#if DEBUG
                    AppManager.debugWriteLine( "    x=" + x );
#endif
                    if ( 0f <= x && x <= 1f ) {
                        if ( mSelectedCurve.equals( CurveType.VibratoRate ) ) {
                            if ( x == 0f ) {
                                edited.VibratoHandle.setStartRate( value );
                            } else {
                                if ( edited.VibratoHandle.getRateBP().getCount() <= 0 ) {
                                    edited.VibratoHandle.setRateBP( new VibratoBPList( new float[] { x },
                                                                                       new int[] { value } ) );
                                } else {
                                    Vector<Float> xs = new Vector<Float>();
                                    Vector<Integer> vals = new Vector<Integer>();
                                    boolean first = true;
                                    VibratoBPList ratebp = edited.VibratoHandle.getRateBP();
                                    int c = ratebp.getCount();
                                    for ( int i = 0; i < c; i++ ) {
                                        VibratoBPPair itemi = ratebp.getElement( i );
                                        if ( itemi.X < x ) {
                                            xs.add( itemi.X );
                                            vals.add( itemi.Y );
                                        } else if ( itemi.X == x ) {
                                            xs.add( x );
                                            vals.add( value );
                                            first = false;
                                        } else {
                                            if ( first ) {
                                                xs.add( x );
                                                vals.add( value );
                                                first = false;
                                            }
                                            xs.add( itemi.X );
                                            vals.add( itemi.Y );
                                        }
                                    }
                                    if ( first ) {
                                        xs.add( x );
                                        vals.add( value );
                                    }
                                    edited.VibratoHandle.setRateBP(
                                        new VibratoBPList(
                                            PortUtil.convertFloatArray( xs.toArray( new Float[] { } ) ),
                                            PortUtil.convertIntArray( vals.toArray( new Integer[] { } ) ) ) );
                                }
                            }
                        } else {
                            if ( x == 0f ) {
                                edited.VibratoHandle.setStartDepth( value );
                            } else {
                                if ( edited.VibratoHandle.getDepthBP().getCount() <= 0 ) {
                                    edited.VibratoHandle.setDepthBP(
                                        new VibratoBPList( new float[] { x }, new int[] { value } ) );
                                } else {
                                    Vector<Float> xs = new Vector<Float>();
                                    Vector<Integer> vals = new Vector<Integer>();
                                    boolean first = true;
                                    VibratoBPList depthbp = edited.VibratoHandle.getDepthBP();
                                    int c = depthbp.getCount();
                                    for ( int i = 0; i < c; i++ ) {
                                        VibratoBPPair itemi = depthbp.getElement( i );
                                        if ( itemi.X < x ) {
                                            xs.add( itemi.X );
                                            vals.add( itemi.Y );
                                        } else if ( itemi.X == x ) {
                                            xs.add( x );
                                            vals.add( value );
                                            first = false;
                                        } else {
                                            if ( first ) {
                                                xs.add( x );
                                                vals.add( value );
                                                first = false;
                                            }
                                            xs.add( itemi.X );
                                            vals.add( itemi.Y );
                                        }
                                    }
                                    if ( first ) {
                                        xs.add( x );
                                        vals.add( value );
                                    }
                                    edited.VibratoHandle.setDepthBP(
                                        new VibratoBPList(
                                            PortUtil.convertFloatArray( xs.toArray( new Float[] { } ) ),
                                            PortUtil.convertIntArray( vals.toArray( new Integer[] { } ) ) ) );
                                }
                            }
                        }
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandEventChangeIDContaints(
                                selected,
                                event_id,
                                edited ) );
                        executeCommand( run, true );
                    }
                } else {
                    VsqBPList list = vsq_track.getCurve( mSelectedCurve.getName() );
                    if ( list != null ) {
                        Vector<Long> delete = new Vector<Long>();
                        TreeMap<Integer, VsqBPPair> add = new TreeMap<Integer, VsqBPPair>();
                        long maxid = list.getMaxID();
                        if ( list.isContainsKey( clock ) ) {
                            int c = list.size();
                            for ( int i = 0; i < c; i++ ) {
                                int cl = list.getKeyClock( i );
                                if ( cl == clock ) {
                                    delete.add( list.getElementB( i ).id );
                                    break;
                                }
                            }
                        }
                        add.put( clock, new VsqBPPair( value, maxid + 1 ) );
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandTrackCurveEdit2( selected,
                                                                       mSelectedCurve.getName(),
                                                                       delete,
                                                                       add ) );
                        executeCommand( run, true );
                    }
                }
            } else if ( mMouseDownMode == MouseDownMode.VEL_WAIT_HOVER ) {
#if DEBUG
                AppManager.debugWriteLine( "    entered VelEdit" );
                AppManager.debugWriteLine( "    m_veledit_selected.Count=" + mVelEditSelected.size() );
                AppManager.debugWriteLine( "    m_veledit_last_selectedid=" + mVelEditLastSelectedID );
                AppManager.debugWriteLine( "    m_veledit_selected.ContainsKey(m_veledit_last_selectedid" + mVelEditSelected.containsKey( mVelEditLastSelectedID ) );
#endif
                mMouseDownMode = MouseDownMode.VEL_EDIT;
                invalidate();
            }
        }

#if JAVA
        private class MouseHoverEventGeneratorProc extends Thread{
            public void run(){
                try{
                    Thread.sleep( 1000 );
                }catch( Exception ex ){
                    return;
                }
                TrackSelector_MouseHover( this, new BEventArgs() );
            }
        }
#else
        private void MouseHoverEventGenerator()
        {
            Thread.Sleep( (int)(System.Windows.Forms.SystemInformation.MouseHoverTime * 0.8) );
            Invoke( new BEventHandler( TrackSelector_MouseHover ) );
        }
#endif

        public void TrackSelector_MouseDoubleClick( Object sender, BMouseEventArgs e )
        {
#if JAVA
            if( mMouseHoverThread != null && mMouseHoverThread.isAlive() ){
                mMouseHoverThread.stop();
            }
#else
            if ( mMouseHoverThread != null && mMouseHoverThread.IsAlive ) {
                mMouseHoverThread.Abort();
            }
#endif

            VsqFileEx vsq = AppManager.getVsqFile();
            int selected = AppManager.getSelected();
            VsqTrack vsq_track = vsq.Track.get( selected );
            int height = getHeight();
            int width = getWidth();
            int key_width = AppManager.keyWidth;

            if ( e.Button == BMouseButtons.Left ) {
                if ( 0 <= e.Y && e.Y <= height - 2 * OFFSET_TRACK_TAB ) {
                    #region MouseDown occured on curve-pane
                    if ( key_width <= e.X && e.X <= width ) {
                        if ( !mSelectedCurve.equals( CurveType.VEL ) &&
                             !mSelectedCurve.equals( CurveType.Accent ) &&
                             !mSelectedCurve.equals( CurveType.Decay ) &&
                             !mSelectedCurve.equals( CurveType.Env ) ) {
                            // ベジエデータ点にヒットしているかどうかを検査
                            //int track = AppManager.getSelected();
                            int clock = AppManager.clockFromXCoord( e.X );
                            Vector<BezierChain> dict = vsq.AttachedCurves.get( selected - 1 ).get( mSelectedCurve );
                            BezierChain target_chain = null;
                            BezierPoint target_point = null;
                            boolean found = false;
                            int dict_size = dict.size();
                            for ( int i = 0; i < dict_size; i++ ) {
                                BezierChain bc = dict.get( i );
                                for ( Iterator<BezierPoint> itr = bc.points.iterator(); itr.hasNext(); ) {
                                    BezierPoint bp = itr.next();
                                    Point pt = getScreenCoord( bp.getBase() );
                                    Rectangle rc = new Rectangle( pt.x - DOT_WID, pt.y - DOT_WID, 2 * DOT_WID + 1, 2 * DOT_WID + 1 );
                                    if ( isInRect( e.X, e.Y, rc ) ) {
                                        found = true;
                                        target_point = (BezierPoint)bp.clone();
                                        target_chain = (BezierChain)bc.clone();
                                        break;
                                    }

                                    if ( bp.getControlLeftType() != BezierControlType.None ) {
                                        pt = getScreenCoord( bp.getControlLeft() );
                                        rc = new Rectangle( pt.x - DOT_WID, pt.y - DOT_WID, 2 * DOT_WID + 1, 2 * DOT_WID + 1 );
                                        if ( isInRect( e.X, e.Y, rc ) ) {
                                            found = true;
                                            target_point = (BezierPoint)bp.clone();
                                            target_chain = (BezierChain)bc.clone();
                                            break;
                                        }
                                    }
                                    if ( bp.getControlRightType() != BezierControlType.None ) {
                                        pt = getScreenCoord( bp.getControlRight() );
                                        rc = new Rectangle( pt.x - DOT_WID, pt.y - DOT_WID, 2 * DOT_WID + 1, 2 * DOT_WID + 1 );
                                        if ( isInRect( e.X, e.Y, rc ) ) {
                                            found = true;
                                            target_point = (BezierPoint)bp.clone();
                                            target_chain = (BezierChain)bc.clone();
                                            break;
                                        }
                                    }
                                }
                                if ( found ) {
                                    break;
                                }
                            }
                            if ( found ) {
                                #region ダブルクリックした位置にベジエデータ点があった場合
                                int chain_id = target_chain.id;
                                BezierChain before = (BezierChain)target_chain.clone();
                                FormBezierPointEdit fbpe = null;
                                try {
                                    fbpe = new FormBezierPointEdit( this,
                                                                    mSelectedCurve,
                                                                    chain_id,
                                                                    target_point.getID() );
                                    mEditingChainID = chain_id;
                                    mEditingPointID = target_point.getID();
                                    BDialogResult ret = AppManager.showModalDialog( fbpe, mMainWindow );
                                    mEditingChainID = -1;
                                    mEditingPointID = -1;
                                    BezierChain after = vsq.AttachedCurves.get( selected - 1 ).getBezierChain( mSelectedCurve, chain_id );
                                    // 編集前の状態に戻す
                                    CadenciiCommand revert =
                                        VsqFileEx.generateCommandReplaceBezierChain(
                                            selected,
                                            mSelectedCurve,
                                            chain_id,
                                            before,
                                            AppManager.editorConfig.getControlCurveResolutionValue() );
                                    executeCommand( revert, false );
                                    if ( ret == BDialogResult.OK ) {
                                        // ダイアログの結果がOKで、かつベジエ曲線が単調増加なら編集を適用
                                        if ( BezierChain.isBezierImplicit( target_chain ) ) {
                                            CadenciiCommand run =
                                                VsqFileEx.generateCommandReplaceBezierChain(
                                                    selected,
                                                    mSelectedCurve,
                                                    chain_id,
                                                    after,
                                                    AppManager.editorConfig.getControlCurveResolutionValue() );
                                            executeCommand( run, true );
                                        }
                                    }
                                } catch ( Exception ex ) {
                                } finally {
                                    if ( fbpe != null ) {
                                        try {
                                            fbpe.close();
                                        } catch ( Exception ex2 ) {
                                        }
                                    }
                                }
                                #endregion
                            } else {
                                #region ダブルクリックした位置にベジエデータ点が無かった場合
                                VsqBPList list = vsq_track.getCurve( mSelectedCurve.getName() );
                                boolean bp_found = false;
                                long bp_id = -1;
                                int tclock = 0;
                                if ( list != null ) {
                                    int list_size = list.size();
                                    for ( int i = 0; i < list_size; i++ ) {
                                        int c = list.getKeyClock( i );
                                        int bpx = AppManager.xCoordFromClocks( c );
                                        if ( e.X < bpx - DOT_WID ) {
                                            break;
                                        }
                                        if ( bpx - DOT_WID <= e.X && e.X <= bpx + DOT_WID ) {
                                            VsqBPPair bp = list.getElementB( i );
                                            int bpy = yCoordFromValue( bp.value );
                                            if ( bpy - DOT_WID <= e.Y && e.Y <= bpy + DOT_WID ) {
                                                bp_found = true;
                                                bp_id = bp.id;
                                                tclock = c;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if ( bp_found ) {
                                    AppManager.itemSelection.clearPoint();
                                    AppManager.itemSelection.addPoint( mSelectedCurve, bp_id );
                                    FormCurvePointEdit dialog =
                                        new FormCurvePointEdit( mMainWindow, bp_id, mSelectedCurve );
                                    int tx = AppManager.xCoordFromClocks( tclock );
                                    Point pt = pointToScreen( new Point( tx, 0 ) );
                                    invalidate();
                                    dialog.setLocation(
                                        new Point( pt.x - dialog.getWidth() / 2, pt.y - dialog.getHeight() ) );
                                    AppManager.showModalDialog( dialog, mMainWindow );
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                } else if ( height - 2 * OFFSET_TRACK_TAB <= e.Y && e.Y <= height - OFFSET_TRACK_TAB ) {
                    #region MouseDown occured on singer list
                    if ( AppManager.getSelectedTool() != EditTool.ERASER ) {
                        VsqEvent ve = null;
                        if( key_width <= e.X && e.X <= width ){
                            ve = findItemAt( e.X, e.Y );
                        }
                        RendererKind renderer = VsqFileEx.getTrackRendererKind( vsq_track );
                        if ( ve == null ) {
                            int x_at_left = key_width + AppManager.keyOffset;
                            Rectangle rc_left_singer_box =
                                new Rectangle(
                                    x_at_left,
                                    height - 2 * OFFSET_TRACK_TAB + 1,
                                    SINGER_ITEM_WIDTH, OFFSET_TRACK_TAB - 2 );
                            if ( isInRect( e.X, e.Y, rc_left_singer_box ) ) {
                                // マウス位置に歌手変更が無かった場合であって、かつ、
                                // マウス位置が左端の常時歌手表示部の内部だった場合
                                int clock_at_left = AppManager.clockFromXCoord( x_at_left );
                                ve = vsq_track.getSingerEventAt( clock_at_left );
                            }
                        }
                        if ( ve != null ) {
                            // マウス位置に何らかのアイテムがあった場合
                            if ( ve.ID.type != VsqIDType.Singer ) {
                                return;
                            }
                            if ( !mCMenuSingerPrepared.Equals( renderer ) ) {
                                prepareSingerMenu( renderer );
                            }
                            cmenuSinger.SingerChangeExists = true;
                            cmenuSinger.InternalID = ve.InternalID;
                            MenuElement[] sub_cmenu_singer = cmenuSinger.getSubElements();
                            for ( int i = 0; i < sub_cmenu_singer.Length; i++ ) {
                                TrackSelectorSingerDropdownMenuItem tsmi =
                                    (TrackSelectorSingerDropdownMenuItem)sub_cmenu_singer[i];
                                if ( tsmi.Language == ve.ID.IconHandle.Language &&
                                     tsmi.Program == ve.ID.IconHandle.Program ) {
                                    tsmi.setSelected( true );
                                } else {
                                    tsmi.setSelected( false );
                                }
                            }
                            cmenuSinger.show( this, e.X, e.Y );
                        } else if ( key_width <= e.X && e.X <= width ) {
                            // マウス位置に何もアイテムが無かった場合
                            if ( !mCMenuSingerPrepared.Equals( renderer ) ) {
                                prepareSingerMenu( renderer );
                            }
                            String singer = AppManager.editorConfig.DefaultSingerName;
                            int clock = AppManager.clockFromXCoord( e.X );
                            int last_clock = 0;
                            for ( Iterator<VsqEvent> itr = vsq_track.getSingerEventIterator(); itr.hasNext(); ) {
                                VsqEvent ve2 = itr.next();
                                if ( last_clock <= clock && clock < ve2.Clock ) {
                                    singer = ((IconHandle)ve2.ID.IconHandle).IDS;
                                    break;
                                }
                                last_clock = ve2.Clock;
                            }
                            cmenuSinger.SingerChangeExists = false;
                            cmenuSinger.Clock = clock;
                            MenuElement[] sub_cmenu_singer = cmenuSinger.getSubElements();
                            for ( int i = 0; i < sub_cmenu_singer.Length; i++ ) {
                                BMenuItem tsmi = (BMenuItem)sub_cmenu_singer[i];
                                tsmi.setSelected( false );
                            }
                            cmenuSinger.show( this, e.X, e.Y );
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
        public void prepareSingerMenu( RendererKind renderer )
        {
            cmenuSinger.removeAll();
            Vector<SingerConfig> items = null;
            if ( renderer == RendererKind.UTAU || renderer == RendererKind.VCNT ) {
                items = AppManager.editorConfig.UtauSingers;
            } else if ( renderer == RendererKind.VOCALOID1 ) {
                items = new Vector<SingerConfig>( Arrays.asList( VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID1 ) ) );
            } else if ( renderer == RendererKind.VOCALOID2 ) {
                items = new Vector<SingerConfig>( Arrays.asList( VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID2 ) ) );
#if ENABLE_AQUESTONE
            } else if ( renderer == RendererKind.AQUES_TONE ) {
                items = new Vector<SingerConfig>();
                int c = AquesToneDriver.SINGERS.Length;
                for ( int i = 0; i < c; i++ ) {
                    items.add( AppManager.getSingerInfoAquesTone( i ) );
                }
#endif
            } else {
                return;
            }
            int count = 0;
            for ( Iterator<SingerConfig> itr = items.iterator(); itr.hasNext(); ) {
                SingerConfig sc = itr.next();
                String tip = "";
                if ( renderer == RendererKind.UTAU || renderer == RendererKind.VCNT ) {
                    if ( sc != null ) {
                        tip = "Name: " + sc.VOICENAME +
                              "\nDirectory: " + sc.VOICEIDSTR;
                    }
                } else if ( renderer == RendererKind.VOCALOID1 ) {
                    if ( sc != null ) {
                        tip = "Original: " + VocaloSysUtil.getOriginalSinger( sc.Language, sc.Program, SynthesizerType.VOCALOID1 ) +
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
                } else if ( renderer == RendererKind.VOCALOID2 ) {
                    if ( sc != null ) {
                        tip = "Original: " + VocaloSysUtil.getOriginalSinger( sc.Language, sc.Program, SynthesizerType.VOCALOID2 ) +
                              "\nBreathiness: " + sc.Breathiness +
                              "\nBrightness: " + sc.Brightness +
                              "\nClearness: " + sc.Clearness +
                              "\nGender Factor: " + sc.GenderFactor +
                              "\nOpening: " + sc.Opening;
                    }
                } else if ( renderer == RendererKind.AQUES_TONE ) {
                    if ( sc != null ) {
                        tip = "Name: " + sc.VOICENAME;
                    }
                }
                if ( sc != null ) {
                    TrackSelectorSingerDropdownMenuItem tsmi =
                        new TrackSelectorSingerDropdownMenuItem();
                    tsmi.setText( sc.VOICENAME );
                    tsmi.ToolTipText = tip;
                    tsmi.ToolTipPxWidth = 0;
                    tsmi.Language = sc.Language;
                    tsmi.Program = sc.Program;
                    tsmi.Click += new BEventHandler( cmenusinger_Click );
#if JAVA
                    // TODO: tsmi.MouseHover
#else
                    tsmi.MouseHover += new BEventHandler( cmenusinger_MouseHover );
#endif
                    cmenuSinger.add( tsmi );
                    count++;
                }
            }
#if JAVA
            cmenuSinger.visibleChangedEvent.add( new BEventHandler( this, "cmenuSinger_VisibleChanged" ) );
#else
            cmenuSinger.VisibleChanged += new BEventHandler( cmenuSinger_VisibleChanged );
#endif
            mCMenuSingerTooltipWidth = new int[count];
            //m_cmenusinger_map = list.ToArray();
            for ( int i = 0; i < count; i++ ) {
                mCMenuSingerTooltipWidth[i] = 0;
            }
            mCMenuSingerPrepared = renderer;
        }

        public void cmenuSinger_VisibleChanged( Object sender, BEventArgs e )
        {
#if JAVA
#else
            toolTip.Hide( cmenuSinger );
#endif
        }

        public void cmenusinger_MouseEnter( Object sender, BEventArgs e )
        {
            cmenusinger_MouseHover( sender, e );
        }

        public void cmenusinger_MouseHover( Object sender, BEventArgs e )
        {
#if !JAVA
            try {
                TrackSelectorSingerDropdownMenuItem menu =
                    (TrackSelectorSingerDropdownMenuItem)sender;
                String tip = menu.ToolTipText;
                int language = menu.Language;
                int program = menu.Program;

                // tooltipを表示するy座標を決める
                int y = 0;
                MenuElement[] sub = cmenuSinger.getSubElements();
                for ( int i = 0; i < sub.Length; i++ ) {
                    TrackSelectorSingerDropdownMenuItem item =
                        (TrackSelectorSingerDropdownMenuItem)sub[i];
                    if ( language == item.Language &&
                         program == item.Program ) {
                        break;
                    }
                    y += item.getHeight();
                }

                int tip_width = menu.ToolTipPxWidth;
                Point ppts = cmenuSinger.pointToScreen( new Point( 0, 0 ) );
                Point pts = new Point( ppts.x, ppts.y );
                Rectangle rrc = PortUtil.getScreenBounds( this );
                Rectangle rc = new Rectangle( rrc.x, rrc.y, rrc.width, rrc.height );
                mTooltipProgram = program;
                mTooltipLanguage = language;
                if ( pts.x + cmenuSinger.getWidth() + tip_width > rc.width ) {
                    toolTip.Show( tip, cmenuSinger, new System.Drawing.Point( -tip_width, y ), 5000 );
                } else {
                    toolTip.Show( tip, cmenuSinger, new System.Drawing.Point( cmenuSinger.Width, y ), 5000 );
                }
            } catch ( Exception ex ) {
                sout.println( "TarckSelectro.tsmi_MouseHover; ex=" + ex );
                AppManager.debugWriteLine( "TarckSelectro.tsmi_MouseHover; ex=" + ex );
            }
#endif
        }

        public void cmenusinger_Click( Object sender, BEventArgs e )
        {
            if ( !(sender is TrackSelectorSingerDropdownMenuItem) ) {
                return;
            }
            TrackSelectorSingerDropdownMenuItem menu = (TrackSelectorSingerDropdownMenuItem)sender;
            int language = menu.Language;
            int program = menu.Program;
            VsqID item = Utility.getSingerID( mCMenuSingerPrepared, program, language );
            if ( item != null ) {
                int selected = AppManager.getSelected();
                if ( cmenuSinger.SingerChangeExists ) {
                    int id = cmenuSinger.InternalID;
                    CadenciiCommand run = new CadenciiCommand(
                        VsqCommand.generateCommandEventChangeIDContaints( selected, id, item ) );
#if DEBUG
                    AppManager.debugWriteLine( "TrackSelector#tsmi_Click; item.IconHandle.Program" + item.IconHandle.Program );
#endif
                    executeCommand( run, true );
                } else {
                    int clock = cmenuSinger.Clock;
                    VsqEvent ve = new VsqEvent( clock, item );
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventAdd( selected, ve ) );
                    executeCommand( run, true );
                }
            }
        }

#if !JAVA
        private void toolTip_Draw( Object sender, System.Windows.Forms.DrawToolTipEventArgs e )
        {
            if ( !(sender is System.Windows.Forms.ToolTip) ) {
                return;
            }

            System.Drawing.Rectangle rrc = e.Bounds;
            Rectangle rc = new Rectangle( rrc.X, rrc.Y, rrc.Width, rrc.Height );
#if DEBUG
            sout.println( "TrackSelector#toolTip_Draw; sender.GetType()=" + sender.GetType() );
#endif

            MenuElement[] sub_cmenu_singer = cmenuSinger.getSubElements();
            for ( int i = 0; i < sub_cmenu_singer.Length; i++ ) {
                MenuElement tsi = sub_cmenu_singer[i];
                if ( !(tsi is TrackSelectorSingerDropdownMenuItem) ) {
                    continue;
                }
                TrackSelectorSingerDropdownMenuItem menu =
                    (TrackSelectorSingerDropdownMenuItem)tsi;
                if ( menu.Language == mTooltipLanguage &&
                     menu.Program == mTooltipProgram ) {
                    menu.ToolTipPxWidth = rc.width;
                    //((BMenuItem)tsi).setTag( tag ); <- ??
                    break;
                }
            }
            e.DrawBackground();
            e.DrawBorder();
            e.DrawText( System.Windows.Forms.TextFormatFlags.VerticalCenter | System.Windows.Forms.TextFormatFlags.Left | System.Windows.Forms.TextFormatFlags.NoFullWidthCharacterBreak );
        }
#endif

        public void TrackSelector_KeyDown( Object sender, BKeyEventArgs e )
        {
#if JAVA
            if( (e.KeyCode & KeyEvent.VK_SPACE) == KeyEvent.VK_SPACE )
#else
            if ( (e.KeyCode & System.Windows.Forms.Keys.Space) == System.Windows.Forms.Keys.Space )
#endif
 {
                mSpaceKeyDowned = true;
            }
        }

        public void TrackSelector_KeyUp( Object sender, BKeyEventArgs e )
        {
#if JAVA
            if( (e.KeyCode & KeyEvent.VK_SPACE) == KeyEvent.VK_SPACE )
#else
            if ( (e.KeyCode & System.Windows.Forms.Keys.Space) == System.Windows.Forms.Keys.Space )
#endif
 {
                mSpaceKeyDowned = false;
            }
        }

        public void cmenuCurveCommon_Click( Object sender, BEventArgs e )
        {
            if ( sender is BMenuItem ) {
                BMenuItem tsmi = (BMenuItem)sender;
                CurveType curve = getCurveTypeFromMenu( tsmi );
                if( !curve.Equals( CurveType.Empty ) ){
                    changeCurve( curve );
                }
            }
        }

        private void registerEventHandlers()
        {
#if JAVA
#else
            this.toolTip.Draw += new System.Windows.Forms.DrawToolTipEventHandler( this.toolTip_Draw );
#endif
            this.cmenuCurveVelocity.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveAccent.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveDecay.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveDynamics.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveVibratoRate.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveVibratoDepth.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveReso1Freq.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveReso1BW.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveReso1Amp.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveReso2Freq.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveReso2BW.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveReso2Amp.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveReso3Freq.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveReso3BW.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveReso3Amp.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveReso4Freq.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveReso4BW.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveReso4Amp.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveHarmonics.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveBreathiness.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveBrightness.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveClearness.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveOpening.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveGenderFactor.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurvePortamentoTiming.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurvePitchBend.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurvePitchBendSensitivity.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveEffect2Depth.Click += new BEventHandler( cmenuCurveCommon_Click );
            this.cmenuCurveEnvelope.Click += new BEventHandler( cmenuCurveCommon_Click );
#if !JAVA
            this.Load += new BEventHandler( this.TrackSelector_Load );
#endif

            this.MouseMove += new BMouseEventHandler( TrackSelector_MouseMove );
            this.MouseDoubleClick += new BMouseEventHandler( TrackSelector_MouseDoubleClick );
            this.KeyUp += new BKeyEventHandler( TrackSelector_KeyUp );
            this.MouseClick += new BMouseEventHandler( TrackSelector_MouseClick );
            this.MouseDown += new BMouseEventHandler( TrackSelector_MouseDown );
            this.MouseUp += new BMouseEventHandler( TrackSelector_MouseUp );
            this.KeyDown += new BKeyEventHandler( TrackSelector_KeyDown );
        }

        private void setResources()
        {
        }

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/cadencii/TrackSelector.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/cadencii/TrackSelector.java
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
        protected override void Dispose( boolean disposing )
        {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cmenuSinger = new TrackSelectorSingerPopupMenu( this.components );
            this.toolTip = new System.Windows.Forms.ToolTip( this.components );
            this.cmenuCurve = new org.kbinani.windows.forms.BPopupMenu( this.components );
            this.cmenuCurveVelocity = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveAccent = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveDecay = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveSeparator1 = new org.kbinani.windows.forms.BMenuSeparator();
            this.cmenuCurveDynamics = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveVibratoRate = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveVibratoDepth = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveSeparator2 = new org.kbinani.windows.forms.BMenuSeparator();
            this.cmenuCurveReso1 = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso1Freq = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso1BW = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso1Amp = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso2 = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso2Freq = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso2BW = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso2Amp = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso3 = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso3Freq = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso3BW = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso3Amp = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso4 = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso4Freq = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso4BW = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveReso4Amp = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveSeparator3 = new org.kbinani.windows.forms.BMenuSeparator();
            this.cmenuCurveHarmonics = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveBreathiness = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveBrightness = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveClearness = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveOpening = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveGenderFactor = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveSeparator4 = new org.kbinani.windows.forms.BMenuSeparator();
            this.cmenuCurvePortamentoTiming = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurvePitchBend = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurvePitchBendSensitivity = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveSeparator5 = new org.kbinani.windows.forms.BMenuSeparator();
            this.cmenuCurveEffect2Depth = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurveEnvelope = new org.kbinani.windows.forms.BMenuItem();
            this.cmenuCurve.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmenuSinger
            // 
            this.cmenuSinger.Name = "cmenuSinger";
            this.cmenuSinger.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.cmenuSinger.ShowCheckMargin = true;
            this.cmenuSinger.ShowImageMargin = false;
            this.cmenuSinger.Size = new System.Drawing.Size( 153, 26 );
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
            this.cmenuCurve.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
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
            this.cmenuCurveEnvelope} );
            this.cmenuCurve.Name = "cmenuCurve";
            this.cmenuCurve.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.cmenuCurve.ShowImageMargin = false;
            this.cmenuCurve.Size = new System.Drawing.Size( 160, 496 );
            // 
            // cmenuCurveVelocity
            // 
            this.cmenuCurveVelocity.Name = "cmenuCurveVelocity";
            this.cmenuCurveVelocity.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveVelocity.Text = "Velocity(&V)";
            // 
            // cmenuCurveAccent
            // 
            this.cmenuCurveAccent.Name = "cmenuCurveAccent";
            this.cmenuCurveAccent.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveAccent.Text = "Accent";
            // 
            // cmenuCurveDecay
            // 
            this.cmenuCurveDecay.Name = "cmenuCurveDecay";
            this.cmenuCurveDecay.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveDecay.Text = "Decay";
            // 
            // cmenuCurveSeparator1
            // 
            this.cmenuCurveSeparator1.Name = "cmenuCurveSeparator1";
            this.cmenuCurveSeparator1.Size = new System.Drawing.Size( 156, 6 );
            // 
            // cmenuCurveDynamics
            // 
            this.cmenuCurveDynamics.Name = "cmenuCurveDynamics";
            this.cmenuCurveDynamics.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveDynamics.Text = "Dynamics";
            // 
            // cmenuCurveVibratoRate
            // 
            this.cmenuCurveVibratoRate.Name = "cmenuCurveVibratoRate";
            this.cmenuCurveVibratoRate.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveVibratoRate.Text = "Vibrato Rate";
            // 
            // cmenuCurveVibratoDepth
            // 
            this.cmenuCurveVibratoDepth.Name = "cmenuCurveVibratoDepth";
            this.cmenuCurveVibratoDepth.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveVibratoDepth.Text = "Vibrato Depth";
            // 
            // cmenuCurveSeparator2
            // 
            this.cmenuCurveSeparator2.Name = "cmenuCurveSeparator2";
            this.cmenuCurveSeparator2.Size = new System.Drawing.Size( 156, 6 );
            // 
            // cmenuCurveReso1
            // 
            this.cmenuCurveReso1.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso1Freq,
            this.cmenuCurveReso1BW,
            this.cmenuCurveReso1Amp} );
            this.cmenuCurveReso1.Name = "cmenuCurveReso1";
            this.cmenuCurveReso1.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveReso1.Text = "Resonance 1";
            // 
            // cmenuCurveReso1Freq
            // 
            this.cmenuCurveReso1Freq.Name = "cmenuCurveReso1Freq";
            this.cmenuCurveReso1Freq.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso1Freq.Text = "Frequency";
            // 
            // cmenuCurveReso1BW
            // 
            this.cmenuCurveReso1BW.Name = "cmenuCurveReso1BW";
            this.cmenuCurveReso1BW.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso1BW.Text = "Band Width";
            // 
            // cmenuCurveReso1Amp
            // 
            this.cmenuCurveReso1Amp.Name = "cmenuCurveReso1Amp";
            this.cmenuCurveReso1Amp.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso1Amp.Text = "Amplitude";
            // 
            // cmenuCurveReso2
            // 
            this.cmenuCurveReso2.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso2Freq,
            this.cmenuCurveReso2BW,
            this.cmenuCurveReso2Amp} );
            this.cmenuCurveReso2.Name = "cmenuCurveReso2";
            this.cmenuCurveReso2.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveReso2.Text = "Resonance 2";
            // 
            // cmenuCurveReso2Freq
            // 
            this.cmenuCurveReso2Freq.Name = "cmenuCurveReso2Freq";
            this.cmenuCurveReso2Freq.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso2Freq.Text = "Frequency";
            // 
            // cmenuCurveReso2BW
            // 
            this.cmenuCurveReso2BW.Name = "cmenuCurveReso2BW";
            this.cmenuCurveReso2BW.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso2BW.Text = "Band Width";
            // 
            // cmenuCurveReso2Amp
            // 
            this.cmenuCurveReso2Amp.Name = "cmenuCurveReso2Amp";
            this.cmenuCurveReso2Amp.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso2Amp.Text = "Amplitude";
            // 
            // cmenuCurveReso3
            // 
            this.cmenuCurveReso3.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso3Freq,
            this.cmenuCurveReso3BW,
            this.cmenuCurveReso3Amp} );
            this.cmenuCurveReso3.Name = "cmenuCurveReso3";
            this.cmenuCurveReso3.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveReso3.Text = "Resonance 3";
            // 
            // cmenuCurveReso3Freq
            // 
            this.cmenuCurveReso3Freq.Name = "cmenuCurveReso3Freq";
            this.cmenuCurveReso3Freq.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso3Freq.Text = "Frequency";
            // 
            // cmenuCurveReso3BW
            // 
            this.cmenuCurveReso3BW.Name = "cmenuCurveReso3BW";
            this.cmenuCurveReso3BW.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso3BW.Text = "Band Width";
            // 
            // cmenuCurveReso3Amp
            // 
            this.cmenuCurveReso3Amp.Name = "cmenuCurveReso3Amp";
            this.cmenuCurveReso3Amp.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso3Amp.Text = "Amplitude";
            // 
            // cmenuCurveReso4
            // 
            this.cmenuCurveReso4.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso4Freq,
            this.cmenuCurveReso4BW,
            this.cmenuCurveReso4Amp} );
            this.cmenuCurveReso4.Name = "cmenuCurveReso4";
            this.cmenuCurveReso4.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveReso4.Text = "Resonance 4";
            // 
            // cmenuCurveReso4Freq
            // 
            this.cmenuCurveReso4Freq.Name = "cmenuCurveReso4Freq";
            this.cmenuCurveReso4Freq.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso4Freq.Text = "Frequency";
            // 
            // cmenuCurveReso4BW
            // 
            this.cmenuCurveReso4BW.Name = "cmenuCurveReso4BW";
            this.cmenuCurveReso4BW.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso4BW.Text = "Band Width";
            // 
            // cmenuCurveReso4Amp
            // 
            this.cmenuCurveReso4Amp.Name = "cmenuCurveReso4Amp";
            this.cmenuCurveReso4Amp.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso4Amp.Text = "Amplitude";
            // 
            // cmenuCurveSeparator3
            // 
            this.cmenuCurveSeparator3.Name = "cmenuCurveSeparator3";
            this.cmenuCurveSeparator3.Size = new System.Drawing.Size( 156, 6 );
            // 
            // cmenuCurveHarmonics
            // 
            this.cmenuCurveHarmonics.Name = "cmenuCurveHarmonics";
            this.cmenuCurveHarmonics.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveHarmonics.Text = "Harmonics";
            // 
            // cmenuCurveBreathiness
            // 
            this.cmenuCurveBreathiness.Name = "cmenuCurveBreathiness";
            this.cmenuCurveBreathiness.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveBreathiness.Text = "Noise";
            // 
            // cmenuCurveBrightness
            // 
            this.cmenuCurveBrightness.Name = "cmenuCurveBrightness";
            this.cmenuCurveBrightness.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveBrightness.Text = "Brightness";
            // 
            // cmenuCurveClearness
            // 
            this.cmenuCurveClearness.Name = "cmenuCurveClearness";
            this.cmenuCurveClearness.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveClearness.Text = "Clearness";
            // 
            // cmenuCurveOpening
            // 
            this.cmenuCurveOpening.Name = "cmenuCurveOpening";
            this.cmenuCurveOpening.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveOpening.Text = "Opening";
            // 
            // cmenuCurveGenderFactor
            // 
            this.cmenuCurveGenderFactor.Name = "cmenuCurveGenderFactor";
            this.cmenuCurveGenderFactor.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveGenderFactor.Text = "Gender Factor";
            // 
            // cmenuCurveSeparator4
            // 
            this.cmenuCurveSeparator4.Name = "cmenuCurveSeparator4";
            this.cmenuCurveSeparator4.Size = new System.Drawing.Size( 156, 6 );
            // 
            // cmenuCurvePortamentoTiming
            // 
            this.cmenuCurvePortamentoTiming.Name = "cmenuCurvePortamentoTiming";
            this.cmenuCurvePortamentoTiming.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurvePortamentoTiming.Text = "Portamento Timing";
            // 
            // cmenuCurvePitchBend
            // 
            this.cmenuCurvePitchBend.Name = "cmenuCurvePitchBend";
            this.cmenuCurvePitchBend.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurvePitchBend.Text = "Pitch Bend";
            // 
            // cmenuCurvePitchBendSensitivity
            // 
            this.cmenuCurvePitchBendSensitivity.Name = "cmenuCurvePitchBendSensitivity";
            this.cmenuCurvePitchBendSensitivity.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurvePitchBendSensitivity.Text = "Pitch Bend Sensitivity";
            // 
            // cmenuCurveSeparator5
            // 
            this.cmenuCurveSeparator5.Name = "cmenuCurveSeparator5";
            this.cmenuCurveSeparator5.Size = new System.Drawing.Size( 156, 6 );
            // 
            // cmenuCurveEffect2Depth
            // 
            this.cmenuCurveEffect2Depth.Name = "cmenuCurveEffect2Depth";
            this.cmenuCurveEffect2Depth.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveEffect2Depth.Text = "Effect2 Depth";
            // 
            // cmenuCurveEnvelope
            // 
            this.cmenuCurveEnvelope.Name = "cmenuCurveEnvelope";
            this.cmenuCurveEnvelope.Size = new System.Drawing.Size( 159, 22 );
            this.cmenuCurveEnvelope.Text = "Envelope";
            // 
            // TrackSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.DoubleBuffered = true;
            this.Name = "TrackSelector";
            this.Size = new System.Drawing.Size( 430, 228 );
            this.cmenuCurve.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private TrackSelectorSingerPopupMenu cmenuSinger;
        private System.Windows.Forms.ToolTip toolTip;
        private BPopupMenu cmenuCurve;
        private BMenuItem cmenuCurveVelocity;
        private BMenuSeparator cmenuCurveSeparator2;
        private BMenuItem cmenuCurveReso1;
        private BMenuItem cmenuCurveReso1Freq;
        private BMenuItem cmenuCurveReso1BW;
        private BMenuItem cmenuCurveReso1Amp;
        private BMenuItem cmenuCurveReso2;
        private BMenuItem cmenuCurveReso2Freq;
        private BMenuItem cmenuCurveReso2BW;
        private BMenuItem cmenuCurveReso2Amp;
        private BMenuItem cmenuCurveReso3;
        private BMenuItem cmenuCurveReso3Freq;
        private BMenuItem cmenuCurveReso3BW;
        private BMenuItem cmenuCurveReso3Amp;
        private BMenuItem cmenuCurveReso4;
        private BMenuItem cmenuCurveReso4Freq;
        private BMenuItem cmenuCurveReso4BW;
        private BMenuItem cmenuCurveReso4Amp;
        private BMenuSeparator cmenuCurveSeparator3;
        private BMenuItem cmenuCurveHarmonics;
        private BMenuItem cmenuCurveDynamics;
        private BMenuSeparator cmenuCurveSeparator1;
        private BMenuItem cmenuCurveBreathiness;
        private BMenuItem cmenuCurveBrightness;
        private BMenuItem cmenuCurveClearness;
        private BMenuItem cmenuCurveGenderFactor;
        private BMenuSeparator cmenuCurveSeparator4;
        private BMenuItem cmenuCurvePortamentoTiming;
        private BMenuSeparator cmenuCurveSeparator5;
        private BMenuItem cmenuCurveEffect2Depth;
        private BMenuItem cmenuCurveOpening;
        private BMenuItem cmenuCurveAccent;
        private BMenuItem cmenuCurveDecay;
        private BMenuItem cmenuCurveVibratoRate;
        private BMenuItem cmenuCurveVibratoDepth;
        private BMenuItem cmenuCurvePitchBend;
        private BMenuItem cmenuCurvePitchBendSensitivity;
        private BMenuItem cmenuCurveEnvelope;

        #endregion
#endif
    }

#if !JAVA
}
#endif
