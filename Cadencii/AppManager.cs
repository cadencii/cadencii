/*
 * AppManager.cs
 * Copyright © 2009-2011 kbinani
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
#if JAVA
package com.github.cadencii;

import java.awt.*;
import java.io.*;
import java.lang.reflect.*;
import java.util.*;
import javax.swing.*;
import com.github.cadencii.*;
import com.github.cadencii.apputil.*;
import com.github.cadencii.vsq.*;
import com.github.cadencii.windows.forms.*;
import com.github.cadencii.xml.*;
import com.github.cadencii.media.*;
import com.github.cadencii.ui.*;

#else

using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using Microsoft.CSharp;
using com.github.cadencii.apputil;
using com.github.cadencii.java.awt;
using com.github.cadencii.java.io;
using com.github.cadencii.java.util;
using com.github.cadencii.media;
using com.github.cadencii.vsq;
using com.github.cadencii.windows.forms;
using com.github.cadencii.xml;

namespace com.github.cadencii
{
    using BEventArgs = System.EventArgs;
    using BEventHandler = System.EventHandler;
    using boolean = System.Boolean;
    using Integer = System.Int32;
    using Long = System.Int64;
    using Float = System.Single;
#endif

#if JAVA
    class GeneratorRunner extends Thread
    {
        private WaveGenerator mGenerator = null;
        private long mSamples;
        private WorkerState mState;

        public GeneratorRunner( WaveGenerator generator, long samples, WorkerState state )
        {
            mGenerator = generator;
            mSamples = samples;
            mState = state;
        }

        public void run()
        {
            mGenerator.begin( mSamples, mState );
        }
    }
#endif

    class RunGeneratorQueue
    {
        public WaveGenerator generator;
        public long samples;
    }

    public partial class AppManager
    {
        public const int MIN_KEY_WIDTH = 68;
        public const int MAX_KEY_WIDTH = MIN_KEY_WIDTH * 5;
        /// <summary>
        /// プリメジャーの最小値
        /// </summary>
        public const int MIN_PRE_MEASURE = 1;
        /// <summary>
        /// プリメジャーの最大値
        /// </summary>
        public const int MAX_PRE_MEASURE = 8;
        private const String CONFIG_FILE_NAME = "config.xml";
        /// <summary>
        /// 強弱記号の，ピアノロール画面上の表示幅（ピクセル）
        /// </summary>
        public const int DYNAFF_ITEM_WIDTH = 40;
#if JAVA
        public const int FONT_SIZE8 = 14;
#else
        public const int FONT_SIZE8 = 8;
#endif
        public const int FONT_SIZE9 = FONT_SIZE8 + 1;
        public const int FONT_SIZE10 = FONT_SIZE8 + 2;
        public const int FONT_SIZE50 = FONT_SIZE8 + 42;

        /// <summary>
        /// 鍵盤の表示幅(pixel)
        /// </summary>
        public static int keyWidth = MIN_KEY_WIDTH * 2;
        /// <summary>
        /// keyWidth+keyOffsetの位置からが、0になってる
        /// </summary>
        public const int keyOffset = 6;
        /// <summary>
        /// エディタの設定
        /// </summary>
        public static EditorConfig editorConfig = new EditorConfig();
        /// <summary>
        /// AttachedCurve用のシリアライザ
        /// </summary>
#if JAVA
        public static XmlSerializer xmlSerializerListBezierCurves = new XmlSerializer( AttachedCurve.class );
#else
        public static XmlSerializer xmlSerializerListBezierCurves = new XmlSerializer( typeof( AttachedCurve ) );
#endif
        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont8 = new Font( "Dialog", Font.PLAIN, FONT_SIZE8 );
        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont9 = new Font( "Dialog", Font.PLAIN, FONT_SIZE9 );
        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont10 = new Font( "Dialog", Font.PLAIN, FONT_SIZE10 );
        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont10Bold = new Font( "Dialog", Font.BOLD, FONT_SIZE10 );
        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont50Bold = new Font( "Dialog", Font.BOLD, FONT_SIZE50 );
        /// <summary>
        /// 歌詞を音符の（高さ方向の）真ん中に描画するためのオフセット。
        /// たとえば，文字列の中心軸がy_centerを通るように描画したい場合は，
        /// <code>g.drawString( ..., x, y_center - baseFont10OffsetHeight + 1 )</code>
        /// とすればよい
        /// </summary>
        public static int baseFont10OffsetHeight = 0;
        /// <summary>
        /// 歌詞を音符の（高さ方向の）真ん中に描画するためのオフセット。
        /// たとえば，文字列の中心軸がy_centerを通るように描画したい場合は，
        /// <code>g.drawString( ..., x, y_center - baseFont8OffsetHeight + 1 )</code>
        /// とすればよい
        /// </summary>
        public static int baseFont8OffsetHeight = 0;
        /// <summary>
        /// 歌詞を音符の（高さ方向の）真ん中に描画するためのオフセット。
        /// たとえば，文字列の中心軸がy_centerを通るように描画したい場合は，
        /// <code>g.drawString( ..., x, y_center - baseFont9OffsetHeight + 1 )</code>
        /// とすればよい
        /// </summary>
        public static int baseFont9OffsetHeight = 0;
        /// <summary>
        /// 歌詞を音符の（高さ方向の）真ん中に描画するためのオフセット。
        /// たとえば，文字列の中心軸がy_centerを通るように描画したい場合は，
        /// <code>g.drawString( ..., x, y_center - baseFont50OffsetHeight + 1 )</code>
        /// とすればよい
        /// </summary>
        public static int baseFont50OffsetHeight = 0;
        /// <summary>
        /// フォントオブジェクトbaseFont8の描画時の高さ
        /// </summary>
        public static int baseFont8Height = FONT_SIZE8;
        /// <summary>
        /// フォントオブジェクトbaseFont9の描画時の高さ
        /// </summary>
        public static int baseFont9Height = FONT_SIZE9;
        /// <summary>
        /// フォントオブジェクトbaseFont1-の描画時の高さ
        /// </summary>
        public static int baseFont10Height = FONT_SIZE10;
        /// <summary>
        /// フォントオブジェクトbaseFont50の描画時の高さ
        /// </summary>
        public static int baseFont50Height = FONT_SIZE50;
#if ENABLE_PROPERTY
        /// <summary>
        /// プロパティパネルのインスタンス
        /// </summary>
        public static PropertyPanel propertyPanel;
        /// <summary>
        /// プロパティウィンドウが分離した場合のプロパティウィンドウのインスタンス。
        /// メインウィンドウとプロパティウィンドウが分離している時、propertyPanelがpropertyWindowの子になる
        /// </summary>
        public static FormNotePropertyController propertyWindow;
#endif
        /// <summary>
        /// アイコンパレット・ウィンドウのインスタンス
        /// </summary>
        public static FormIconPalette iconPalette = null;
        /// <summary>
        /// クリップボード管理クラスのインスタンス
        /// </summary>
        public static ClipboardModel clipboard = null;
        /// <summary>
        /// 選択アイテムの管理クラスのインスタンス
        /// </summary>
        public static ItemSelectionModel itemSelection = null;
        /// <summary>
        /// 編集履歴を管理するmodel
        /// </summary>
        public static EditHistoryModel editHistory = null;

        #region Static Readonly Fields
        /// <summary>
        /// トラックの背景部分の塗りつぶし色。16トラックそれぞれで異なる
        /// </summary>
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
        /// <summary>
        /// トラックをレンダリングするためのボタンの背景色。16トラックそれぞれで異なる
        /// </summary>
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
        /// <summary>
        /// スクリプトに前置されるusingのリスト
        /// </summary>
        public static readonly String[] usingS = new String[] { "using System;",
                                             "using System.IO;",
                                             "using com.github.cadencii.vsq;",
                                             "using com.github.cadencii;",
                                             "using com.github.cadencii.java.io;",
                                             "using com.github.cadencii.java.util;",
                                             "using com.github.cadencii.java.awt;",
                                             "using com.github.cadencii.media;",
                                             "using com.github.cadencii.apputil;",
                                             "using System.Windows.Forms;",
                                             "using System.Collections.Generic;",
                                             "using System.Drawing;",
                                             "using System.Text;",
                                             "using System.Xml.Serialization;" };
        /// <summary>
        /// ショートカットキーとして受付可能なキーのリスト
        /// </summary>
        public static readonly Vector<BKeys> SHORTCUT_ACCEPTABLE = new Vector<BKeys>( Arrays.asList( new BKeys[]{
            BKeys.A,
            BKeys.B,
            BKeys.Back,
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
        /// <summary>
        /// UTAU関連のテキストファイルで受け付けるエンコーディングの種類
        /// </summary>
        public static readonly String[] TEXT_ENCODINGS_IN_UTAU = new String[] { "Shift_JIS", "UTF-16", "US-ANSI" };
        /// <summary>
        /// よく使うボーダー線の色
        /// </summary>
        public static readonly Color COLOR_BORDER = new Color( 118, 123, 138 );
        #endregion

        #region Private Static Fields
        private static Color mHilightBrush = PortUtil.CornflowerBlue;
        private static Object mLocker;
        private static BTimer mAutoBackupTimer;
        /// <summary>
        /// 現在稼働しているWaveGenerator．稼働していないときはnull
        /// </summary>
        private static WaveGenerator mWaveGenerator = null;
        /// <summary>
        /// mWaveGeneratorの停止を行うためのコマンダー
        /// </summary>
        private static WorkerStateImp mWaveGeneratorState = new WorkerStateImp();
        /// <summary>
        /// mWaveGeneratorを動かしているスレッド
        /// </summary>
        private static Thread mPreviewThread;

#if !TREECOM
        private static VsqFileEx mVsq;
#endif
        private static String mFile = "";
        private static int mSelected = 1;
        private static int mCurrentClock = 0;
        private static boolean mPlaying = false;
        private static boolean mRepeatMode = false;
        private static boolean mGridVisible = false;
        private static EditMode mEditMode = EditMode.NONE;
        /// <summary>
        /// トラックのオーバーレイ表示
        /// </summary>
        private static boolean mOverlay = true;
        private static EditTool mSelectedTool = EditTool.PENCIL;

        /// <summary>
        /// Playingプロパティにロックをかけるためのオブジェクト
        /// </summary>
        private static Object mLockerPlayingProperty = new Object();
        #endregion

        #region 選択範囲の管理
        /// <summary>
        /// SelectedRegionが有効かどうかを表すフラグ
        /// </summary>
        private static boolean mWholeSelectedIntervalEnabled = false;
        /// <summary>
        /// Ctrlキーを押しながらのマウスドラッグ操作による選択が行われた範囲(単位：クロック)
        /// </summary>
        public static SelectedRegion mWholeSelectedInterval = new SelectedRegion( 0 );
        /// <summary>
        /// コントロールカーブ上で現在選択されている範囲（x:クロック、y:各コントロールカーブの単位に同じ）。マウスが動いているときのみ使用
        /// </summary>
        public static Rectangle mCurveSelectingRectangle = new Rectangle();
        /// <summary>
        /// コントロールカーブ上で選択された範囲（単位：クロック）
        /// </summary>
        public static SelectedRegion mCurveSelectedInterval = new SelectedRegion( 0 );
        /// <summary>
        /// 選択範囲が有効かどうか。
        /// </summary>
        private static boolean mCurveSelectedIntervalEnabled = false;
        /// <summary>
        /// 範囲選択モードで音符を動かしている最中の、選択範囲の開始位置（クロック）。マウスが動いているときのみ使用
        /// </summary>
        public static int mWholeSelectedIntervalStartForMoving = 0;
        #endregion

        /// <summary>
        /// 自動ノーマライズモードかどうかを表す値を取得、または設定します。
        /// </summary>
        public static boolean mAutoNormalize = false;
        /// <summary>
        /// Bezierカーブ編集モードが有効かどうかを表す
        /// </summary>
        private static boolean mIsCurveMode = false;
        /// <summary>
        /// 再生時に自動スクロールするかどうか
        /// </summary>
        public static boolean mAutoScroll = true;
        /// <summary>
        /// プレビュー再生が開始された時刻
        /// </summary>
        public static double mPreviewStartedTime;
        /// <summary>
        /// 現在選択中のパレットアイテムの名前
        /// </summary>
        public static String mSelectedPaletteTool = "";
        /// <summary>
        /// このCadenciiのID。起動ごとにユニークな値が設定され、一時フォルダのフォルダ名等に使用する
        /// </summary>
        private static String mID = "";
        /// <summary>
        /// ダイアログを表示中かどうか
        /// </summary>
        private static boolean mShowingDialog = false;
        /// <summary>
        /// メインの編集画面のインスタンス
        /// </summary>
        public static FormMain mMainWindow = null;
        /// <summary>
        /// メイン画面のコントローラ
        /// </summary>
        public static FormMainController mMainWindowController = null;
        /// <summary>
        /// ミキサーダイアログ
        /// </summary>
        public static FormMixer mMixerWindow;
        /// <summary>
        /// 画面に描かれるエントリのリスト．trackBar.Valueの変更やエントリの編集などのたびに更新される
        /// </summary>
        public static Vector<Vector<DrawObject>> mDrawObjects;
        /// <summary>
        /// 歌詞入力に使用するテキストボックス
        /// </summary>
        public static LyricTextBox mInputTextBox = null;
        public static int mAddingEventLength;
        /// <summary>
        /// 音符の追加操作における，追加中の音符
        /// </summary>
        public static VsqEvent mAddingEvent;
        /// <summary>
        /// AppManager.m_draw_objectsを描く際の，最初に検索されるインデクス．
        /// </summary>
        public static int[] mDrawStartIndex = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 各トラックがUTAUモードかどうか．mDrawObjectsと同じタイミングで更新される
        /// </summary>
        public static boolean[] mDrawIsUtau = new boolean[16];
        /// <summary>
        /// マウスが降りていて，かつ範囲選択をしているときに立つフラグ
        /// </summary>
        public static boolean mIsPointerDowned = false;
        /// <summary>
        /// マウスが降りた仮想スクリーン上の座標(ピクセル)
        /// </summary>
        public static Point mMouseDownLocation = new Point();
        public static int mLastTrackSelectorHeight;
        /// <summary>
        /// UTAUの原音設定のリスト。TreeMapのキーは、oto.iniのあるディレクトリ名になっている。
        /// </summary>
        public static TreeMap<String, UtauVoiceDB> mUtauVoiceDB = new TreeMap<String, UtauVoiceDB>();
        /// <summary>
        /// 最後にレンダリングが行われた時の、トラックの情報が格納されている。
        /// </summary>
        public static RenderedStatus[] mLastRenderedStatus = new RenderedStatus[16];
        /// <summary>
        /// RenderingStatusをXMLシリアライズするためのシリアライザ
        /// </summary>
        public static XmlSerializer mRenderingStatusSerializer = new XmlSerializer( typeof( RenderedStatus ) );
        /// <summary>
        /// wavを出力するための一時ディレクトリのパス。
        /// </summary>
        private static String mTempWaveDir = "";
        /// <summary>
        /// 再生開始からの経過時刻がこの秒数以下の場合、再生を止めることが禁止される。
        /// </summary>
        public static double mForbidFlipPlayingThresholdSeconds = 0.2;
        /// <summary>
        /// ピアノロール画面に，コントロールカーブをオーバーレイしているモード
        /// </summary>
        public static boolean mCurveOnPianoroll = false;
        /// <summary>
        /// 直接再生モード時の、再生開始した位置の曲頭からの秒数
        /// </summary>
        public static float mDirectPlayShift = 0.0f;
        /// <summary>
        /// プレビュー終了位置のクロック
        /// </summary>
        public static int mPreviewEndingClock = 0;

#if DEBUG
        /// <summary>
        /// ログ出力用
        /// </summary>
        private static BufferedWriter mDebugLog = null;
#endif

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
        public static boolean showContextMenuWhenRightClickedOnPianoroll = true;
        #endregion // 裏設定項目

        /// <summary>
        /// メイン画面で、グリッド表示のOn/Offが切り替わった時発生するイベント
        /// </summary>
#if JAVA
        public static BEvent<BEventHandler> gridVisibleChangedEvent = new BEvent<BEventHandler>();
#elif QT_VERSION
        public: signals: void gridVisibleChanged( QObject sender, QObject e );
#else
        public static event BEventHandler GridVisibleChanged;
#endif

        /// <summary>
        /// プレビュー再生が開始された時発生するイベント
        /// </summary>
#if JAVA
        public static BEvent<BEventHandler> previewStartedEvent = new BEvent<BEventHandler>();
#elif QT_VERSION
        public: signals: void previewStartedEvent( QObject sender, QObject e );
#else
        public static event BEventHandler PreviewStarted;
#endif

        /// <summary>
        /// プレビュー再生が終了した時発生するイベント
        /// </summary>
#if JAVA
        public static BEvent<BEventHandler> previewAbortedEvent = new BEvent<BEventHandler>();
#elif QT_VERSION
        public: signals: void previewAborted( QObject sender, QObject e );
#else
        public static event BEventHandler PreviewAborted;
#endif

        /// <summary>
        /// 編集ツールが変化した時発生するイベント
        /// </summary>
#if JAVA
        public static BEvent<BEventHandler> selectedToolChangedEvent = new BEvent<BEventHandler>();
#elif QT_VERSION
        public: signals: void selectedToolChanged( QObject sender, QObject e );
#else
        public static event BEventHandler SelectedToolChanged;
#endif

        /// <summary>
        /// BGMに何らかの変更があった時発生するイベント
        /// </summary>
#if JAVA
        public static BEvent<BEventHandler> updateBgmStatusRequiredEvent = new BEvent<BEventHandler>();
#elif QT_VERSION
        public: signals: void updateBgmStatusRequired( QObject sender, QObject e );
#else
        public static event BEventHandler UpdateBgmStatusRequired;
#endif

        /// <summary>
        /// メインウィンドウにフォーカスを当てる要求があった時発生するイベント
        /// </summary>
#if JAVA
        public static BEvent<BEventHandler> mainWindowFocusRequiredEvent = new BEvent<BEventHandler>();
#elif QT_VERSION
        public: signals: void mainWindowFocusRequired( QObject sender, QObject e );
#else
        public static event BEventHandler MainWindowFocusRequired;
#endif

        /// <summary>
        /// 編集されたかどうかを表す値に変更が要求されたときに発生するイベント
        /// </summary>
#if JAVA
        public static BEvent<EditedStateChangedEventHandler> editedStateChangedEvent = new BEvent<EditedStateChangedEventHandler>();
#elif QT_VERSION
        public: signals: void editedStateChanged( QObject sender, bool edited );
#else
        public static event EditedStateChangedEventHandler EditedStateChanged;
#endif

        /// <summary>
        /// 波形ビューのリロードが要求されたとき発生するイベント．
        /// GeneralEventArgsの引数は，トラック番号,waveファイル名,開始時刻(秒),終了時刻(秒)が格納されたObject[]配列
        /// 開始時刻＞終了時刻の場合は，partialではなく全体のリロード要求
        /// </summary>
#if JAVA
        public static BEvent<WaveViewRealoadRequiredEventHandler> waveViewReloadRequiredEvent = new BEvent<WaveViewRealoadRequiredEventHandler>();
#elif QT_VERSION
        public: signals: void waveViewReloadRequired( QObject sender, int track, QString path, double sec_start, double sec_end );
#else
        public static event WaveViewRealoadRequiredEventHandler WaveViewReloadRequired;
#endif

        private const String TEMPDIR_NAME = "cadencii";

        /// <summary>
        /// voacloidrv.shからwineを呼ぶために，ProcessBuilderに渡す
        /// 引数リストの最初の部分を取得します
        /// </summary>
        public static Vector<String> getWineProxyArgument()
        {
            Vector<String> ret = new Vector<String>();
            ret.add( "/bin/sh" );
            String vocaloidrv_sh =
                Utility.normalizePath( fsys.combine( PortUtil.getApplicationStartupPath(), "vocaloidrv.sh" ) );
            ret.add( vocaloidrv_sh );

            String wine_prefix =
                Utility.normalizePath( editorConfig.WinePrefix );
            ret.add( wine_prefix );

            String wine_top =
                Utility.normalizePath( editorConfig.WineTop );
            ret.add( wine_top );
            return ret;
        }

        /// <summary>
        /// プレビュー再生を開始します．
        /// 合成処理などが途中でキャンセルされた場合にtrue，それ以外の場合にfalseを返します
        /// </summary>
        private static boolean previewStart( FormMain form )
        {
            int selected = mSelected;
            RendererKind renderer = VsqFileEx.getTrackRendererKind( mVsq.Track.get( selected ) );
            int clock = mCurrentClock;
            mDirectPlayShift = (float)mVsq.getSecFromClock( clock );
            // リアルタイム再生で無い場合
            String tmppath = getTempWaveDir();

            int track_count = mVsq.Track.size();

            Vector<Integer> tracks = new Vector<Integer>();
            for ( int track = 1; track < track_count; track++ ) {
                tracks.add( track );
            }

            if ( patchWorkToFreeze( form, tracks ) ) {
                // キャンセルされた
#if DEBUG
                sout.println( "AppManager#previewStart; patchWorkToFreeze returns true" );
#endif
                return true;
            }

            WaveSenderDriver driver = new WaveSenderDriver();
            Vector<Amplifier> waves = new Vector<Amplifier>();
            for ( int i = 0; i < tracks.size(); i++ ) {
                int track = tracks.get( i );
                String file = fsys.combine( tmppath, track + ".wav" );
                WaveReader wr = null;
                try {
                    wr = new WaveReader( file );
                    wr.setOffsetSeconds( mDirectPlayShift );
                    Amplifier a = new Amplifier();
                    FileWaveSender f = new FileWaveSender( wr );
                    a.setSender( f );
                    a.setAmplifierView( mMixerWindow.getVolumeTracker( track ) );
                    waves.add( a );
                    a.setRoot( driver );
                    f.setRoot( driver );
                } catch ( Exception ex ) {
                    Logger.write( typeof( AppManager ) + ".previewStart; ex=" + ex + "\n" );
                    serr.println( "AppManager.previewStart; ex=" + ex );
#if JAVA
                    ex.printStackTrace();
#endif
                }
            }

            // clock以降に音符があるかどうかを調べる
            int count = 0;
            for ( Iterator<VsqEvent> itr = mVsq.Track.get( selected ).getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent ve = itr.next();
                if ( ve.Clock >= clock ) {
                    count++;
                    break;
                }
            }

            int bgm_count = getBgmCount();
            double pre_measure_sec = mVsq.getSecFromClock( mVsq.getPreMeasureClocks() );
            for ( int i = 0; i < bgm_count; i++ ) {
                BgmFile bgm = getBgm( i );
                WaveReader wr = null;
                try {
                    wr = new WaveReader( bgm.file );
                    double offset = bgm.readOffsetSeconds + mDirectPlayShift;
                    if ( bgm.startAfterPremeasure ) {
                        offset -= pre_measure_sec;
                    }
                    wr.setOffsetSeconds( offset );
#if DEBUG
                    sout.println( "AppManager.previewStart; bgm.file=" + bgm.file + "; offset=" + offset );

#endif
                    Amplifier a = new Amplifier();
                    FileWaveSender f = new FileWaveSender( wr );
                    a.setSender( f );
                    a.setAmplifierView( AppManager.mMixerWindow.getVolumeTrackerBgm( i ) );
                    waves.add( a );
                    a.setRoot( driver );
                    f.setRoot( driver );
                } catch ( Exception ex ) {
                    Logger.write( typeof( AppManager ) + ".previewStart; ex=" + ex + "\n" );
                    serr.println( "AppManager.previewStart; ex=" + ex );
                }
            }

            // 最初のsenderをドライバにする
            driver.setSender( waves.get( 0 ) );
            Mixer m = new Mixer();
            m.setRoot( driver );
            driver.setReceiver( m );
            stopGenerator();
            setGenerator( driver );
            Amplifier amp = new Amplifier();
            amp.setRoot( driver );
            amp.setAmplifierView( mMixerWindow.getVolumeTrackerMaster() );
            m.setReceiver( amp );
            MonitorWaveReceiver monitor = MonitorWaveReceiver.prepareInstance();
            monitor.setRoot( driver );
            amp.setReceiver( monitor );
            for ( int i = 1; i < waves.size(); i++ ) {
                m.addSender( waves.get( i ) );
            }

            int end_clock = mVsq.TotalClocks;
            if ( mVsq.config.EndMarkerEnabled ) {
                end_clock = mVsq.config.EndMarker;
            }
            mPreviewEndingClock = end_clock;
            double end_sec = mVsq.getSecFromClock( end_clock );
            int sample_rate = mVsq.config.SamplingRate;
            long samples = (long)((end_sec - mDirectPlayShift) * sample_rate);
            driver.init( mVsq, mSelected, 0, end_clock, sample_rate );
#if DEBUG
            sout.println( "AppManager.previewStart; calling runGenerator..." );
#endif
            runGenerator( samples );
#if DEBUG
            sout.println( "AppManager.previewStart; calling runGenerator... done" );
#endif
            return false;
        }

        public static int getPreviewEndingClock()
        {
            return mPreviewEndingClock;
        }

        /// <summary>
        /// プレビュー再生を停止します
        /// </summary>
        private static void previewStop()
        {
            stopGenerator();
        }

        /// <summary>
        /// 指定したトラックのレンダリングが必要な部分を再レンダリングし，ツギハギすることでトラックのキャッシュを最新の状態にします．
        /// レンダリングが途中でキャンセルされた場合にtrue，そうでない場合にfalseを返します．
        /// </summary>
        /// <param name="tracks"></param>
        public static boolean patchWorkToFreeze( FormMain main_window, Vector<Integer> tracks )
        {
            mVsq.updateTotalClocks();
            Vector<PatchWorkQueue> queue = patchWorkCreateQueue( tracks );
#if DEBUG
            sout.println( "AppManager#patchWorkToFreeze; queue.size()=" + queue.size() );
#endif

            FormWorker fw = new FormWorker();
            fw.setupUi( new FormWorkerUi( fw ) );
            fw.getUi().setTitle( _( "Synthesize" ) );
            fw.getUi().setText( _( "now synthesizing..." ) );

            double total = 0;
            SynthesizeWorker worker = new SynthesizeWorker( main_window );
            foreach( PatchWorkQueue q in queue ){
                // ジョブを追加
                double job_amount = q.getJobAmount();
                fw.addJob( worker, "processQueue", q.getMessage(), job_amount, q );
                total += job_amount;
            }

            // パッチワークをするジョブを追加
            fw.addJob( worker, "patchWork", _( "patchwork" ), total, new Object[] { queue, tracks } );

            // ジョブを開始
            fw.startJob();

            // ダイアログを表示する
            beginShowDialog();
            boolean ret = fw.getUi().showDialogTo( main_window );
#if DEBUG
            sout.println( "AppManager#patchWorkToFreeze; showDialog returns " + ret );
#endif
            endShowDialog();
            return ret;
        }

        public static void invokeWaveViewReloadRequiredEvent( int track, String wavePath, double secStart, double secEnd )
        {
            try {
#if QT_VERSION
                waveViewReloadRequired( this, track, wavePath, secStart, secEnd );
#else
                WaveViewRealoadRequiredEventArgs arg = new WaveViewRealoadRequiredEventArgs();
                arg.track = track;
                arg.file = wavePath;
                arg.secStart = secStart;
                arg.secEnd = secEnd;
#if JAVA
                waveViewReloadRequiredEvent.raise( AppManager.class, arg );
#else
                if ( WaveViewReloadRequired != null ) {
                    WaveViewReloadRequired.Invoke( typeof( AppManager ), arg );
                }
#endif
#endif
            } catch ( Exception ex ) {
                Logger.write( typeof( AppManager ) + ".invokeWaveViewReloadRequiredEvent; ex=" + ex + "\n" );
                sout.println( typeof( AppManager ) + ".invokeWaveViewReloadRequiredEvent; ex=" + ex );
#if JAVA
                ex.printStackTrace();
#endif
            }
        }

        /// <summary>
        /// 指定したトラックについて，再合成が必要な範囲を抽出し，それらのリストを作成します
        /// </summary>
        /// <param name="tracks">リストを作成するトラック番号の一覧</param>
        /// <returns></returns>
        public static Vector<PatchWorkQueue> patchWorkCreateQueue( Vector<Integer> tracks )
        {
            mVsq.updateTotalClocks();
            String temppath = getTempWaveDir();
            int presend = editorConfig.PreSendTime;
            int totalClocks = mVsq.TotalClocks;

            Vector<PatchWorkQueue> queue = new Vector<PatchWorkQueue>();
            int[] startIndex = new int[tracks.size() + 1]; // startList, endList, trackList, filesの内，第startIndex[j]からが，第tracks[j]トラックについてのレンダリング要求かを表す.

            for ( int k = 0; k < tracks.size(); k++ ) {
                startIndex[k] = queue.size();
                int track = tracks.get( k );
                VsqTrack vsq_track = mVsq.Track.get( track );
                String wavePath = fsys.combine( temppath, track + ".wav" );

                if ( mLastRenderedStatus[track - 1] == null ) {
                    // この場合は全部レンダリングする必要がある
                    PatchWorkQueue q = new PatchWorkQueue();
                    q.track = track;
                    q.clockStart = 0;
                    q.clockEnd = totalClocks + 240;
                    q.file = wavePath;
                    q.vsq = mVsq;
                    queue.add( q );
                    continue;
                }

                // 部分レンダリング
                EditedZoneUnit[] areas =
                    Utility.detectRenderedStatusDifference( mLastRenderedStatus[track - 1],
                                                            new RenderedStatus(
                                                                (VsqTrack)vsq_track.clone(),
                                                                mVsq.TempoTable,
                                                                (SequenceConfig)mVsq.config.clone() ) );

                // areasとかぶっている音符がどれかを判定する
                EditedZone zone = new EditedZone();
                zone.add( areas );
                checkSerializedEvents( zone, vsq_track, mVsq.TempoTable, areas );
                checkSerializedEvents( zone, mLastRenderedStatus[track - 1].track, mLastRenderedStatus[track - 1].tempo, areas );

                // レンダリング済みのwaveがあれば、zoneに格納された編集範囲に隣接する前後が無音でない場合、
                // 編集範囲を無音部分まで延長する。
                if ( fsys.isFileExists( wavePath ) ) {
                    WaveReader wr = null;
                    try {
                        wr = new WaveReader( wavePath );
                        int sampleRate = wr.getSampleRate();
                        int buflen = 1024;
                        double[] left = new double[buflen];
                        double[] right = new double[buflen];

                        // まずzoneから編集範囲を抽出
                        Vector<EditedZoneUnit> areasList = new Vector<EditedZoneUnit>();
                        for ( Iterator<EditedZoneUnit> itr = zone.iterator(); itr.hasNext(); ) {
                            EditedZoneUnit e = itr.next();
                            areasList.add( (EditedZoneUnit)e.clone() );
                        }

                        for ( Iterator<EditedZoneUnit> itr = areasList.iterator(); itr.hasNext(); ) {
                            EditedZoneUnit e = itr.next();
                            int exStart = e.mStart;
                            int exEnd = e.mEnd;

                            // 前方に1クロックずつ検索する。
                            int end = e.mStart;
                            int start = end - 1;
                            double secEnd = mVsq.getSecFromClock( end );
                            long saEnd = (long)(secEnd * sampleRate);
                            double secStart = 0.0;
                            long saStart = 0;
                            while ( true ) {
                                start = end - 1;
                                if ( start < 0 ) {
                                    start = 0;
                                    break;
                                }
                                secStart = mVsq.getSecFromClock( start );
                                saStart = (long)(secStart * sampleRate);
                                int samples = (int)(saEnd - saStart);
                                long pos = saStart;
                                boolean allzero = true;
                                while ( samples > 0 ) {
                                    int delta = samples > buflen ? buflen : samples;
                                    wr.read( pos, delta, left, right );
                                    for ( int i = 0; i < delta; i++ ) {
                                        if ( left[i] != 0.0 || right[i] != 0.0 ) {
                                            allzero = false;
                                            break;
                                        }
                                    }
                                    pos += delta;
                                    samples -= delta;
                                    if ( !allzero ) {
                                        break;
                                    }
                                }
                                if ( allzero ) {
                                    break;
                                }
                                secEnd = secStart;
                                end = start;
                                saEnd = saStart;
                            }
                            // endクロックより先は無音であるようだ。
                            exStart = end;

                            // 後方に1クロックずつ検索する
                            if ( e.mEnd < int.MaxValue ) {
                                start = e.mEnd;
                                secStart = mVsq.getSecFromClock( start );
                                while ( true ) {
                                    end = start + 1;
                                    secEnd = mVsq.getSecFromClock( end );
                                    saEnd = (long)(secEnd * sampleRate);
                                    int samples = (int)(saEnd - saStart);
                                    long pos = saStart;
                                    boolean allzero = true;
                                    while ( samples > 0 ) {
                                        int delta = samples > buflen ? buflen : samples;
                                        wr.read( pos, delta, left, right );
                                        for ( int i = 0; i < delta; i++ ) {
                                            if ( left[i] != 0.0 || right[i] != 0.0 ) {
                                                allzero = false;
                                                break;
                                            }
                                        }
                                        pos += delta;
                                        samples -= delta;
                                        if ( !allzero ) {
                                            break;
                                        }
                                    }
                                    if ( allzero ) {
                                        break;
                                    }
                                    secStart = secEnd;
                                    start = end;
                                    saStart = saEnd;
                                }
                                // startクロック以降は無音のようだ
                                exEnd = start;
                            }
#if DEBUG
                            if ( e.mStart != exStart ) {
                                sout.println( "FormMain#patchWorkToFreeze; start extended; " + e.mStart + " => " + exStart );
                            }
                            if ( e.mEnd != exEnd ) {
                                sout.println( "FormMain#patchWorkToFreeze; end extended; " + e.mEnd + " => " + exEnd );
                            }
#endif

                            zone.add( exStart, exEnd );
                        }
                    } catch ( Exception ex ) {
                        Logger.write( typeof( FormMain ) + ".patchWorkToFreeze; ex=" + ex + "\n" );
                        serr.println( "FormMain#patchWorkToFreeze; ex=" + ex );
                    } finally {
                        if ( wr != null ) {
                            try {
                                wr.close();
                            } catch ( Exception ex2 ) {
                                Logger.write( typeof( FormMain ) + ".patchWorkToFreeze; ex=" + ex2 + "\n" );
                                serr.println( "FormMain#patchWorkToFreeze; ex2=" + ex2 );
                            }
                        }
                    }
                }

                // zoneに、レンダリングが必要なアイテムの範囲が格納されているので。
                int j = -1;
#if DEBUG
                sout.println( "AppManager#patchWorkCreateQueue; track#" + track );
#endif
                for ( Iterator<EditedZoneUnit> itr = zone.iterator(); itr.hasNext(); ) {
                    EditedZoneUnit unit = itr.next();
                    j++;
                    PatchWorkQueue q = new PatchWorkQueue();
                    q.track = track;
                    q.clockStart = unit.mStart;
                    q.clockEnd = unit.mEnd;
#if DEBUG
                    sout.println( "    start=" + unit.mStart + "; end=" + unit.mEnd );
#endif
                    q.file = fsys.combine( temppath, track + "_" + j + ".wav" );
                    q.vsq = mVsq;
                    queue.add( q );
                }
            }
            startIndex[tracks.size()] = queue.size();

            if ( queue.size() <= 0 ) {
                // パッチワークする必要なし
                for ( int i = 0; i < tracks.size(); i++ ) {
                    setRenderRequired( tracks.get( i ), false );
                }
            }

            return queue;
        }

        /// <summary>
        /// 指定されたトラックにあるイベントの内、配列areasで指定されたゲートタイム範囲とオーバーラップしているか、
        /// または連続している音符を抽出し、その範囲をzoneに追加します。
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="vsq_track"></param>
        /// <param name="tempo_vector"></param>
        /// <param name="areas"></param>
        private static void checkSerializedEvents( EditedZone zone, VsqTrack vsq_track, TempoVector tempo_vector, EditedZoneUnit[] areas )
        {
            if ( vsq_track == null || zone == null || areas == null ) {
                return;
            }
            if ( areas.Length == 0 ) {
                return;
            }

            // まず，先行発音も考慮した音符の範囲を列挙する
            Vector<Integer> clockStartList = new Vector<Integer>();
            Vector<Integer> clockEndList = new Vector<Integer>();
            Vector<Integer> internalIdList = new Vector<Integer>();
            int size = vsq_track.getEventCount();
            RendererKind kind = VsqFileEx.getTrackRendererKind( vsq_track );
            for ( int i = 0; i < size; i++ ) {
                VsqEvent item = vsq_track.getEvent( i );
                int clock_start = item.Clock;
                int clock_end = item.Clock + item.ID.getLength();
                int internal_id = item.InternalID;
                if ( item.ID.type == VsqIDType.Anote ) {
                    if ( kind == RendererKind.UTAU ) {
                        // 秒単位の先行発音
                        double sec_pre_utterance = item.UstEvent.getPreUtterance() / 1000.0;
                        // 先行発音を考慮した，音符の開始秒
                        double sec_at_clock_start_act = tempo_vector.getSecFromClock( clock_start ) - sec_pre_utterance;
                        // 上記をクロック数に変換した物
                        int clock_start_draft = (int)tempo_vector.getClockFromSec( sec_at_clock_start_act );
                        // くり上がりがあるかもしれないので検査
                        while ( sec_at_clock_start_act < tempo_vector.getSecFromClock( clock_start_draft ) && 0 < clock_start_draft ) {
                            clock_start_draft--;
                        }
                        clock_start = clock_start_draft;
                    }
                } else {
                    internal_id = -1;
                }

                // リストに追加
                clockStartList.add( clock_start );
                clockEndList.add( clock_end );
                internalIdList.add( internal_id );
            }

            TreeMap<Integer, Integer> ids = new TreeMap<Integer, Integer>();
            //for ( Iterator<Integer> itr = vsq_track.indexIterator( IndexIteratorKind.NOTE ); itr.hasNext(); ) {
            for( int indx = 0; indx < size; indx++ ){
                int internal_id = internalIdList.get( indx );
                if ( internal_id == -1 ) {
                    continue;
                }
                int clockStart = clockStartList.get( indx );// item.Clock;
                int clockEnd = clockEndList.get( indx );// clockStart + item.ID.getLength();
                for ( int i = 0; i < areas.Length; i++ ) {
                    EditedZoneUnit area = areas[i];
                    if ( clockStart < area.mEnd && area.mEnd <= clockEnd ) {
                        if ( !ids.containsKey( internal_id ) ) {
                            ids.put( internal_id, indx );
                            zone.add( clockStart, clockEnd );
                        }
                    } else if ( clockStart <= area.mStart && area.mStart < clockEnd ) {
                        if ( !ids.containsKey( internal_id ) ) {
                            ids.put( internal_id, indx );
                            zone.add( clockStart, clockEnd );
                        }
                    } else if ( area.mStart <= clockStart && clockEnd < area.mEnd ) {
                        if ( !ids.containsKey( internal_id ) ) {
                            ids.put( internal_id, indx );
                            zone.add( clockStart, clockEnd );
                        }
                    } else if ( clockStart <= area.mStart && area.mEnd < clockEnd ) {
                        if ( !ids.containsKey( internal_id ) ) {
                            ids.put( internal_id, indx );
                            zone.add( clockStart, clockEnd );
                        }
                    }
                }
            }

            // idsに登録された音符のうち、前後がつながっているものを列挙する。
            boolean changed = true;
            int numEvents = vsq_track.getEventCount();
            while ( changed ) {
                changed = false;
                for ( Iterator<Integer> itr = ids.keySet().iterator(); itr.hasNext(); ) {
                    int id = itr.next();
                    int indx = ids.get( id ); // InternalIDがidのアイテムの禁書目録
                    //VsqEvent item = vsq_track.getEvent( indx );

                    // アイテムを遡り、連続していれば追加する
                    int clock = clockStartList.get( indx );// item.Clock;
                    for ( int i = indx - 1; i >= 0; i-- ) {
                        //VsqEvent search = vsq_track.getEvent( i );
                        int internal_id = internalIdList.get( i );
                        if ( internal_id == -1 ) {
                            continue;
                        }
                        int searchClock = clockStartList.get( i );// search.Clock;
                        //int searchLength = search.ID.getLength();
                        int searchClockEnd = clockEndList.get( i );//
                        // 一個前の音符の終了位置が，この音符の開始位置と同じが後ろにある場合 -> 重なり有りと判定
                        if ( clock <= searchClockEnd ) {
                            if ( !ids.containsKey( internal_id ) ) {
                                ids.put( internal_id, i );
                                zone.add( searchClock, searchClockEnd );
                                changed = true;
                            }
                            clock = searchClock;
                        } else {
                            break;
                        }
                    }

                    // アイテムを辿り、連続していれば追加する
                    clock = clockEndList.get( indx );// item.Clock + item.ID.getLength();
                    for ( int i = indx + 1; i < numEvents; i++ ) {
                        //VsqEvent search = vsq_track.getEvent( i );
                        int internal_id = internalIdList.get( i );
                        if ( internal_id == -1 ) {
                            continue;
                        }
                        int searchClock = clockStartList.get( i );// search.Clock;
                        int searchClockEnd = clockEndList.get( i );// search.ID.getLength();
                        // 一行後ろの音符の開始位置が，この音符の終了位置と同じが後ろにある場合 -> 重なり有りと判定
                        if ( searchClock <= clock ) {
                            if ( !ids.containsKey( internal_id ) ) {
                                ids.put( internal_id, i );
                                zone.add( searchClock, searchClockEnd );
                                changed = true;
                            }
                            clock = searchClockEnd;
                        } else {
                            break;
                        }
                    }

                    if ( changed ) {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 波形生成器が実行中かどうかを取得します
        /// </summary>
        /// <returns></returns>
        public static boolean isGeneratorRunning()
        {
            boolean ret = false;
            lock ( mLocker ) {
                WaveGenerator g = mWaveGenerator;
                if ( g != null ) {
                    ret = g.isRunning();
                }
            }
            return ret;
        }

        /// <summary>
        /// 波形生成器を停止します
        /// </summary>
        public static void stopGenerator()
        {
            lock ( mLocker ) {
                WaveGenerator g = mWaveGenerator;
                if ( g != null ) {
                    mWaveGeneratorState.requestCancel();
                    while ( mWaveGenerator.isRunning() ) {
#if JAVA
                        try{
                            Thread.sleep( 100 );
                        }catch( Exception ex ){
                        }
#elif __cplusplus
                        TODO
#else
                        Thread.Sleep( 100 );
#endif
                    }
                }
                mWaveGenerator = null;
            }
        }

        /// <summary>
        /// 波形生成器を設定します
        /// </summary>
        /// <param name="generator"></param>
        public static void setGenerator( WaveGenerator generator )
        {
            lock ( mLocker ) {
                WaveGenerator g = mWaveGenerator;
                if ( g != null ) {
                    mWaveGeneratorState.requestCancel();
                    while ( g.isRunning() ) {
#if JAVA
                        try{
                            Thread.sleep( 100 );
                        }catch( Exception ex ){
                        }
#elif __cplusplus
                        TODO
#else
                        Thread.Sleep( 100 );
#endif
                    }
                }
                mWaveGenerator = generator;
            }
        }

        /// <summary>
        /// 波形生成器を別スレッドで実行します
        /// </summary>
        /// <param name="samples">合成するサンプル数．波形合成器のbeginメソッドに渡される</param>
        public static void runGenerator( long samples )
        {
            lock ( mLocker ) {
#if DEBUG
                sout.println( "AppManager#runGenerator; (mPreviewThread==null)=" + (mPreviewThread == null) );
#endif
                Thread t = mPreviewThread;
                if ( t != null ) {
#if DEBUG
#if JAVA
                    sout.println( "AppManager#runGenerator; mPreviewThread.getState()=" + t.getState() );
#else
                    sout.println( "AppManager#runGenerator; mPreviewThread.ThreadState=" + t.ThreadState );
#endif
#endif
#if JAVA
                    if( t.getState() != Thread.State.TERMINATED ){
#else
                    if ( t.ThreadState != ThreadState.Stopped ) {
#endif
                        WaveGenerator g = mWaveGenerator;
                        if ( g != null ) {
                            mWaveGeneratorState.requestCancel();
                            while ( mWaveGenerator.isRunning() ) {
#if JAVA
                                try{
                                    Thread.sleep( 100 );
                                }catch( Exception ex ){
                                }
#elif __cplusplus
                                TODO
#else
                                Thread.Sleep( 100 );
#endif
                            }
                        }
#if DEBUG
                        sout.println( "AppManager#runGenerator; waiting stop..." );
#endif
#if JAVA
                        while( t.getState() != Thread.State.TERMINATED ){
                            try{
                                Thread.sleep( 100 );
                            }catch( Exception ex ){
                            }
                        }
#else
                        while ( t.ThreadState != ThreadState.Stopped ) {
                            Thread.Sleep( 100 );
                        }
#endif
#if DEBUG
                        sout.println( "AppManager#runGenerator; waiting stop... done" );
#endif
                    }
                }

                mWaveGeneratorState.reset();
#if JAVA
                mPreviewThread = new GeneratorRunner( mWaveGenerator, samples, mWaveGeneratorState );
                mPreviewThread.start();
#else
                RunGeneratorQueue q = new RunGeneratorQueue();
                q.generator = mWaveGenerator;
                q.samples = samples;
                mPreviewThread = new Thread(
                    new ParameterizedThreadStart( runGeneratorCore ) );
                mPreviewThread.Start( q );
#endif
            }
        }

        private static void runGeneratorCore( Object argument )
        {
            RunGeneratorQueue q = (RunGeneratorQueue)argument;
            WaveGenerator g = q.generator;
            long samples = q.samples;
            try {
                g.begin( samples, mWaveGeneratorState );
            } catch ( Exception ex ) {
                Logger.write( typeof( AppManager ) + ".runGeneratorCore; ex=" + ex + "\n" );
                sout.println( "AppManager#runGeneratorCore; ex=" + ex );
            }
        }

        /// <summary>
        /// 音の高さを表すnoteから、画面に描くべきy座標を計算します
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public static int yCoordFromNote( float note )
        {
            return yCoordFromNote( note, mMainWindowController.getStartToDrawY() );
        }

        /// <summary>
        /// 音の高さを表すnoteから、画面に描くべきy座標を計算します
        /// </summary>
        /// <param name="note"></param>
        /// <param name="start_to_draw_y"></param>
        /// <returns></returns>
        public static int yCoordFromNote( float note, int start_to_draw_y )
        {
            return (int)(-1 * (note - 127.0f) * (int)(mMainWindowController.getScaleY() * 100)) - start_to_draw_y;
        }

        /// <summary>
        /// ピアノロール画面のy座標から、その位置における音の高さを取得します
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int noteFromYCoord( int y )
        {
            return 127 - (int)noteFromYCoordCore( y );
        }

        /// <summary>
        /// ピアノロール画面のy座標から、その位置における音の高さを取得します
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double noteFromYCoordDoublePrecision( int y )
        {
            return 127.0 - noteFromYCoordCore( y );
        }

        private static double noteFromYCoordCore( int y )
        {
            return (double)(mMainWindowController.getStartToDrawY() + y) / (double)((int)(mMainWindowController.getScaleY() * 100));
        }

        /// <summary>
        /// 指定した音声合成システムが使用する歌手のリストを取得します
        /// </summary>
        /// <param name="kind">音声合成システムの種類</param>
        /// <returns>歌手のリスト</returns>
        public static Vector<VsqID> getSingerListFromRendererKind( RendererKind kind )
        {
            Vector<VsqID> singers = null;
            if ( kind == RendererKind.AQUES_TONE ) {
                singers = new Vector<VsqID>();
#if ENABLE_AQUESTONE
                singers.AddRange( AquesToneDriver.Singers.Select( ( config ) => getSingerIDAquesTone( config.Program ) ) );
#endif
            } else if ( kind == RendererKind.AQUES_TONE2 ) {
                singers = new Vector<VsqID>();
#if ENABLE_AQUESTONE
                singers.AddRange( AquesTone2Driver.Singers.Select( ( config ) => getSingerIDAquesTone2( config.Program ) ) );
#endif
            } else if ( kind == RendererKind.VCNT || kind == RendererKind.UTAU ) {
                Vector<SingerConfig> list = editorConfig.UtauSingers;
                singers = new Vector<VsqID>();
                for ( Iterator<SingerConfig> itr = list.iterator(); itr.hasNext(); ) {
                    SingerConfig sc = itr.next();
                    singers.add( getSingerIDUtau( sc.Language, sc.Program ) );
                }
            } else if ( kind == RendererKind.VOCALOID1 ) {
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
        /// 指定したトラックの，指定した音符イベントについて，UTAUのパラメータを適用します
        /// </summary>
        /// <param name="vsq_track"></param>
        /// <param name="item"></param>
        public static void applyUtauParameter( VsqTrack vsq_track, VsqEvent item )
        {
            VsqEvent singer = vsq_track.getSingerEventAt( item.Clock );
            SingerConfig sc = getSingerInfoUtau( singer.ID.IconHandle.Language, singer.ID.IconHandle.Program );
            if ( sc != null && mUtauVoiceDB.containsKey( sc.VOICEIDSTR ) ) {
                UtauVoiceDB db = mUtauVoiceDB.get( sc.VOICEIDSTR );
                OtoArgs oa = db.attachFileNameFromLyric( item.ID.LyricHandle.L0.Phrase );
                if ( item.UstEvent == null ) {
                    item.UstEvent = new UstEvent();
                }
                item.UstEvent.setVoiceOverlap( oa.msOverlap );
                item.UstEvent.setPreUtterance( oa.msPreUtterance );
            }
        }

        /// <summary>
        /// 指定したディレクトリにある合成ステータスのxmlデータを読み込みます
        /// </summary>
        /// <param name="directory">読み込むxmlが保存されたディレクトリ</param>
        /// <param name="track">読み込みを行うトラックの番号</param>
        public static void deserializeRenderingStatus( String directory, int track )
        {
            String xml = fsys.combine( directory, track + ".xml" );
            RenderedStatus status = null;
            if ( fsys.isFileExists( xml ) ) {
                FileInputStream fs = null;
                try {
                    fs = new FileInputStream( xml );
                    Object obj = mRenderingStatusSerializer.deserialize( fs );
                    if ( obj != null && obj is RenderedStatus ) {
                        status = (RenderedStatus)obj;
                    }
                } catch ( Exception ex ) {
                    Logger.write( typeof( AppManager ) + ".deserializeRederingStatus; ex=" + ex + "\n" );
                    status = null;
                    serr.println( "AppManager#deserializeRederingStatus; ex=" + ex );
                } finally {
                    if ( fs != null ) {
                        try {
                            fs.close();
                        } catch ( Exception ex2 ) {
                            Logger.write( typeof( AppManager ) + ".deserializeRederingStatus; ex=" + ex2 + "\n" );
                            serr.println( "AppManager#deserializeRederingStatus; ex2=" + ex2 );
                        }
                    }
                }
            }
            mLastRenderedStatus[track - 1] = status;
        }

        /// <summary>
        /// 指定したトラックの合成ステータスを，指定したxmlファイルに保存します．
        /// </summary>
        /// <param name="temppath"></param>
        /// <param name="track"></param>
        public static void serializeRenderingStatus( String temppath, int track )
        {
            FileOutputStream fs = null;
            boolean failed = true;
            String xml = fsys.combine( temppath, track + ".xml" );
            try {
                fs = new FileOutputStream( xml );
                mRenderingStatusSerializer.serialize( fs, mLastRenderedStatus[track - 1] );
                failed = false;
            } catch ( Exception ex ) {
                serr.println( "FormMain#patchWorkToFreeze; ex=" + ex );
                Logger.write( typeof( AppManager ) + ".serializeRenderingStauts; ex=" + ex + "\n" );
            } finally {
                if ( fs != null ) {
                    try {
                        fs.close();
                    } catch ( Exception ex2 ) {
                        serr.println( "FormMain#patchWorkToFreeze; ex2=" + ex2 );
                        Logger.write( typeof( AppManager ) + ".serializeRenderingStatus; ex=" + ex2 + "\n" );
                    }
                }
            }

            // シリアライズに失敗した場合，該当するxmlを削除する
            if( failed ){
                if( fsys.isFileExists( xml ) ){
                    try{
                        PortUtil.deleteFile( xml );
                    }catch( Exception ex ){
                        Logger.write( typeof( AppManager ) + ".serializeRendererStatus; ex=" + ex + "\n" );
#if JAVA
                        ex.printStackTrace();
#endif
                    }
                }
            }
        }

        /// <summary>
        /// クロック数から、画面に描くべきx座標の値を取得します。
        /// </summary>
        /// <param name="clocks"></param>
        /// <returns></returns>
        public static int xCoordFromClocks( double clocks )
        {
            return xCoordFromClocks( clocks, mMainWindowController.getScaleX(), mMainWindowController.getStartToDrawX() );
        }

        /// <summary>
        /// クロック数から、画面に描くべきx座標の値を取得します。
        /// </summary>
        /// <param name="clocks"></param>
        /// <returns></returns>
        public static int xCoordFromClocks( double clocks, float scalex, int start_to_draw_x )
        {
            return (int)(keyWidth + clocks * scalex - start_to_draw_x) + keyOffset;
        }

        /// <summary>
        /// 画面のx座標からクロック数を取得します
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int clockFromXCoord( int x )
        {
            return (int)((x + mMainWindowController.getStartToDrawX() - keyOffset - keyWidth) * mMainWindowController.getScaleXInv());
        }

        #region 選択範囲の管理
        public static boolean isWholeSelectedIntervalEnabled()
        {
            return mWholeSelectedIntervalEnabled;
        }

        public static boolean isCurveSelectedIntervalEnabled()
        {
            return mCurveSelectedIntervalEnabled;
        }

        public static void setWholeSelectedIntervalEnabled( boolean value )
        {
            mWholeSelectedIntervalEnabled = value;
            if ( value ) {
                mCurveSelectedIntervalEnabled = false;
            }
        }

        public static void setCurveSelectedIntervalEnabled( boolean value )
        {
            mCurveSelectedIntervalEnabled = value;
            if ( value ) {
                mWholeSelectedIntervalEnabled = false;
            }
        }
        #endregion

        #region MessageBoxのラッパー
        public static BDialogResult showMessageBox( String text )
        {
            return showMessageBox( text, "", com.github.cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION, com.github.cadencii.windows.forms.Utility.MSGBOX_PLAIN_MESSAGE );
        }

        public static BDialogResult showMessageBox( String text, String caption )
        {
            return showMessageBox( text, caption, com.github.cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION, com.github.cadencii.windows.forms.Utility.MSGBOX_PLAIN_MESSAGE );
        }

        public static BDialogResult showMessageBox( String text, String caption, int optionType )
        {
            return showMessageBox( text, caption, optionType, com.github.cadencii.windows.forms.Utility.MSGBOX_PLAIN_MESSAGE );
        }

        /// <summary>
        /// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="main_form"></param>
        /// <returns></returns>
#if JAVA
        public static BDialogResult showModalDialog( BDialog dialog, Component parent_form )
#else
        public static BDialogResult showModalDialog( BDialog dialog, System.Windows.Forms.Form parent_form )
#endif
        {
            beginShowDialog();
            BDialogResult ret = dialog.showDialog( parent_form );
            endShowDialog();
            return ret;
        }

        /// <summary>
        /// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="main_form"></param>
        /// <returns></returns>
#if JAVA
        public static int showModalDialog( UiBase dialog, Component parent_form )
#else
        public static int showModalDialog( UiBase dialog, System.Windows.Forms.Form parent_form )
#endif
        {
            beginShowDialog();
            int ret = dialog.showDialog( parent_form );
            endShowDialog();
            return ret;
        }

        /// <summary>
        /// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="main_form"></param>
        /// <returns></returns>
#if JAVA
        public static BDialogResult showModalDialog( BFolderBrowser dialog, Frame main_form )
#else
        public static BDialogResult showModalDialog( BFolderBrowser dialog, System.Windows.Forms.Form main_form )
#endif
        {
            beginShowDialog();
            BDialogResult ret = dialog.showDialog( main_form );
            endShowDialog();
            return ret;
        }

        /// <summary>
        /// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="open_mode"></param>
        /// <param name="main_form"></param>
        /// <returns></returns>
#if JAVA
        public static int showModalDialog( BFileChooser dialog, boolean open_mode, Object main_form )
#else
        public static int showModalDialog( BFileChooser dialog, boolean open_mode, System.Windows.Forms.Form main_form )
#endif
        {
            beginShowDialog();
            int ret = 0;

            if ( open_mode ) {
#if JAVA
                if( main_form instanceof Frame ){
                    ret = dialog.showOpenDialog( (Frame)main_form );
                }else if( main_form instanceof Dialog ){
                    ret = dialog.showOpenDialog( (Dialog)main_form );
                }else{
                    ret = BFileChooser.ERROR_OPTION;
                }
#else
                ret = dialog.showOpenDialog( main_form );
#endif
            } else {
#if JAVA
                if( main_form instanceof Frame ){
                    ret = dialog.showSaveDialog( (Frame)main_form );
                }else if( main_form instanceof Dialog ){
                    ret = dialog.showSaveDialog( (Dialog)main_form );
                }else{
                    ret = BFileChooser.ERROR_OPTION;
                }
#else
                ret = dialog.showSaveDialog( main_form );
#endif
            }
            endShowDialog();
            return ret;
        }

        /// <summary>
        /// beginShowDialogが呼ばれた後，endShowDialogがまだ呼ばれていないときにtrue
        /// </summary>
        /// <returns></returns>
        public static boolean isShowingDialog()
        {
            return mShowingDialog;
        }

        /// <summary>
        /// モーダルなダイアログを出すために，プロパティウィンドウとミキサーウィンドウの「最前面に表示」設定を一時的にOFFにします
        /// </summary>
        private static void beginShowDialog()
        {
            mShowingDialog = true;
#if ENABLE_PROPERTY
            if ( propertyWindow != null ) {
                boolean previous = propertyWindow.getUi().isAlwaysOnTop();
                propertyWindow.setPreviousAlwaysOnTop( previous );
                if ( previous ) {
                    propertyWindow.getUi().setAlwaysOnTop( false );
                }
            }
#endif
            if ( mMixerWindow != null ) {
                boolean previous = mMixerWindow.isAlwaysOnTop();
                mMixerWindow.setPreviousAlwaysOnTop( previous );
                if ( previous ) {
                    mMixerWindow.setAlwaysOnTop( false );
                }
            }

            if ( iconPalette != null ) {
                boolean previous = iconPalette.isAlwaysOnTop();
                iconPalette.setPreviousAlwaysOnTop( previous );
                if ( previous ) {
                    iconPalette.setAlwaysOnTop( false );
                }
            }
        }

        /// <summary>
        /// beginShowDialogで一時的にOFFにした「最前面に表示」設定を元に戻します
        /// </summary>
        private static void endShowDialog()
        {
#if ENABLE_PROPERTY
            if ( propertyWindow != null ) {
                propertyWindow.getUi().setAlwaysOnTop( propertyWindow.getPreviousAlwaysOnTop() );
            }
#endif
            if ( mMixerWindow != null ) {
                mMixerWindow.setAlwaysOnTop( mMixerWindow.getPreviousAlwaysOnTop() );
            }

            if ( iconPalette != null ) {
                iconPalette.setAlwaysOnTop( iconPalette.getPreviousAlwaysOnTop() );
            }

            try {
#if JAVA
                mainWindowFocusRequiredEvent.raise( AppManager.class, new BEventArgs() );
#elif QT_VERSION
                mainWindowFocusRequired( this, null );
#else
                if ( MainWindowFocusRequired != null ) {
                    MainWindowFocusRequired.Invoke( typeof( AppManager ), new EventArgs() );
                }
#endif
            } catch ( Exception ex ) {
                Logger.write( typeof( AppManager ) + ".endShowDialog; ex=" + ex + "\n" );
                sout.println( typeof( AppManager ) + ".endShowDialog; ex=" + ex );
            }
            mShowingDialog = false;
        }

        public static BDialogResult showMessageBox( String text, String caption, int optionType, int messageType )
        {
            beginShowDialog();
            BDialogResult ret = com.github.cadencii.windows.forms.Utility.showMessageBox( text, caption, optionType, messageType );
            endShowDialog();
            return ret;
        }
        #endregion

        #region BGM 関連
        public static int getBgmCount()
        {
            if ( mVsq == null ) {
                return 0;
            } else {
                return mVsq.BgmFiles.size();
            }
        }

        public static BgmFile getBgm( int index )
        {
            if ( mVsq == null ) {
                return null;
            }
            return mVsq.BgmFiles.get( index );
        }

        public static void removeBgm( int index )
        {
            if ( mVsq == null ) {
                return;
            }
            Vector<BgmFile> list = new Vector<BgmFile>();
            int count = mVsq.BgmFiles.size();
            for ( int i = 0; i < count; i++ ) {
                if ( i != index ) {
                    list.add( (BgmFile)mVsq.BgmFiles.get( i ).clone() );
                }
            }
            CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate( list );
            editHistory.register( mVsq.executeCommand( run ) );
            try {
#if JAVA
                editedStateChangedEvent.raise( typeof( AppManager ), true );
#elif QT_VERSION
                editedStateChanged( this, true );
#else
                if ( EditedStateChanged != null ) {
                    EditedStateChanged.Invoke( typeof( AppManager ), true );
                }
#endif
            } catch ( Exception ex ) {
                Logger.write( typeof( AppManager ) + ".removeBgm; ex=" + ex + "\n" );
                serr.println( typeof( AppManager ) + ".removeBgm; ex=" + ex );
            }
            mMixerWindow.updateStatus();
        }

        public static void clearBgm()
        {
            if ( mVsq == null ) {
                return;
            }
            Vector<BgmFile> list = new Vector<BgmFile>();
            CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate( list );
            editHistory.register( mVsq.executeCommand( run ) );
            try {
#if JAVA
                editedStateChangedEvent.raise( typeof( AppManager ), true );
#elif QT_VERSION
                editedStateChanged( this, true );
#else
                if ( EditedStateChanged != null ) {
                    EditedStateChanged.Invoke( typeof( AppManager ), true );
                }
#endif
            } catch ( Exception ex ) {
                Logger.write( typeof( AppManager ) + ".removeBgm; ex=" + ex + "\n" );
                serr.println( typeof( AppManager ) + ".removeBgm; ex=" + ex );
            }
            mMixerWindow.updateStatus();
        }

        public static void addBgm( String file )
        {
            if ( mVsq == null ) {
                return;
            }
            Vector<BgmFile> list = new Vector<BgmFile>();
            int count = mVsq.BgmFiles.size();
            for ( int i = 0; i < count; i++ ) {
                list.add( (BgmFile)mVsq.BgmFiles.get( i ).clone() );
            }
            BgmFile item = new BgmFile();
            item.file = file;
            item.feder = 0;
            item.panpot = 0;
            list.add( item );
            CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate( list );
            editHistory.register( mVsq.executeCommand( run ) );
            try {
#if JAVA
                editedStateChangedEvent.raise( typeof( AppManager ), true );
#elif QT_VERSION
                editedStateChanged( this, true );
#else
                if ( EditedStateChanged != null ) {
                    EditedStateChanged.Invoke( typeof( AppManager ), true );
                }
#endif
            } catch ( Exception ex ) {
                Logger.write( typeof( AppManager ) + ".removeBgm; ex=" + ex + "\n" );
                serr.println( typeof( AppManager ) + ".removeBgm; ex=" + ex );
            }
            mMixerWindow.updateStatus();
        }
        #endregion

        #region 自動保存
        public static void updateAutoBackupTimerStatus()
        {
            if ( !mFile.Equals( "" ) && editorConfig.AutoBackupIntervalMinutes > 0 ) {
                double millisec = editorConfig.AutoBackupIntervalMinutes * 60.0 * 1000.0;
                int draft = (int)millisec;
                if ( millisec > int.MaxValue ) {
                    draft = int.MaxValue;
                }
                mAutoBackupTimer.setDelay( draft );
                mAutoBackupTimer.start();
            } else {
                mAutoBackupTimer.stop();
            }
        }

        public static void handleAutoBackupTimerTick( Object sender, BEventArgs e )
        {
#if DEBUG
            sout.println( "AppManager::handleAutoBackupTimerTick" );
#endif
            if ( !mFile.Equals( "" ) && fsys.isFileExists( mFile ) ) {
                String path = PortUtil.getDirectoryName( mFile );
                String backup = fsys.combine( path, "~$" + PortUtil.getFileName( mFile ) );
                String file2 = fsys.combine( path, PortUtil.getFileNameWithoutExtension( backup ) + ".vsq" );
                if ( fsys.isFileExists( backup ) ) {
                    try {
                        PortUtil.deleteFile( backup );
                    } catch ( Exception ex ) {
                        serr.println( "AppManager::handleAutoBackupTimerTick; ex=" + ex );
                        Logger.write( typeof( AppManager ) + ".handleAutoBackupTimerTick; ex=" + ex + "\n" );
                    }
                }
                if ( fsys.isFileExists( file2 ) ) {
                    try {
                        PortUtil.deleteFile( file2 );
                    } catch ( Exception ex ) {
                        serr.println( "AppManager::handleAutoBackupTimerTick; ex=" + ex );
                        Logger.write( typeof( AppManager ) + ".handleAutoBackupTimerTick; ex=" + ex + "\n" );
                    }
                }
                saveToCor( backup );
            }
        }
        #endregion

        public static void debugWriteLine( String message )
        {
#if DEBUG
            try {
                if ( mDebugLog == null ) {
                    String log_file = fsys.combine( PortUtil.getApplicationStartupPath(), "log.txt" );
                    mDebugLog = new BufferedWriter( new FileWriter( log_file ) );
                }
                mDebugLog.write( message );
                mDebugLog.newLine();
            } catch ( Exception ex ) {
                serr.println( "AppManager#debugWriteLine; ex=" + ex );
                Logger.write( typeof( AppManager ) + ".debugWriteLine; ex=" + ex + "\n" );
            }
            sout.println( message );
#endif
        }

        /// <summary>
        /// FormMainを識別するID
        /// </summary>
        public static String getID()
        {
            return mID;
        }

        /// <summary>
        /// gettext
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String _( String id )
        {
            return Messaging.getMessage( id );
        }

        /// <summary>
        /// 音声ファイルのキャッシュディレクトリのパスを設定します。
        /// このメソッドでは、キャッシュディレクトリの変更に伴う他の処理は実行されません。
        /// </summary>
        /// <param name="value"></param>
        public static void setTempWaveDir( String value )
        {
#if DEBUG
            sout.println( "AppManager#setTempWaveDir; before: \"" + mTempWaveDir + "\"" );
            sout.println( "                           after:  \"" + value + "\"" );
#endif
            mTempWaveDir = value;
        }

        /// <summary>
        /// 音声ファイルのキャッシュディレクトリのパスを取得します。
        /// </summary>
        /// <returns></returns>
        public static String getTempWaveDir()
        {
            return mTempWaveDir;
        }

        /// <summary>
        /// Cadenciiが使用する一時ディレクトリのパスを取得します。
        /// </summary>
        /// <returns></returns>
        public static String getCadenciiTempDir()
        {
            String temp = fsys.combine( PortUtil.getTempPath(), TEMPDIR_NAME );
            if ( !fsys.isDirectoryExists( temp ) ) {
                PortUtil.createDirectory( temp );
            }
            return temp;
        }

        /// <summary>
        /// ベジエ曲線を編集するモードかどうかを取得します。
        /// </summary>
        /// <returns></returns>
        public static boolean isCurveMode()
        {
            return mIsCurveMode;
        }

        /// <summary>
        /// ベジエ曲線を編集するモードかどうかを設定します。
        /// </summary>
        /// <param name="value"></param>
        public static void setCurveMode( boolean value )
        {
            boolean old = mIsCurveMode;
            mIsCurveMode = value;
            if ( old != mIsCurveMode ) {
                try {
#if JAVA
                    selectedToolChangedEvent.raise( typeof( AppManager ), new BEventArgs() );
#elif QT_VERSION
                    selectedToolChanged( this, null );
#else
                    if ( SelectedToolChanged != null ) {
                        SelectedToolChanged.Invoke( typeof( AppManager ), new EventArgs() );
                    }
#endif
                } catch ( Exception ex ) {
                    serr.println( "AppManager#setCurveMode; ex=" + ex );
                    Logger.write( typeof( AppManager ) + ".setCurveMode; ex=" + ex + "\n" );
                }
            }
        }

#if !TREECOM
        /// <summary>
        /// アンドゥ処理を行います。
        /// </summary>
        public static void undo()
        {
            if ( editHistory.hasUndoHistory() ) {
                Vector<ValuePair<Integer, Integer>> before_ids = new Vector<ValuePair<Integer, Integer>>();
                for ( Iterator<SelectedEventEntry> itr = itemSelection.getEventIterator(); itr.hasNext(); ) {
                    SelectedEventEntry item = itr.next();
                    before_ids.add( new ValuePair<Integer, Integer>( item.track, item.original.InternalID ) );
                }

                ICommand run_src = editHistory.getUndo();
                CadenciiCommand run = (CadenciiCommand)run_src;
                if ( run.vsqCommand != null ) {
                    if ( run.vsqCommand.Type == VsqCommandType.TRACK_DELETE ) {
                        int track = (Integer)run.vsqCommand.Args[0];
                        if ( track == getSelected() && track >= 2 ) {
                            setSelected( track - 1 );
                        }
                    }
                }
                ICommand inv = mVsq.executeCommand( run );
                if ( run.type == CadenciiCommandType.BGM_UPDATE ) {
                    try {
#if JAVA
                        updateBgmStatusRequiredEvent.raise( typeof( AppManager ), new BEventArgs() );
#elif QT_VERSION
                        updateBgmStatusRequired( this, null );
#else
                        if ( UpdateBgmStatusRequired != null ) {
                            UpdateBgmStatusRequired.Invoke( typeof( AppManager ), new EventArgs() );
                        }
#endif
                    } catch ( Exception ex ) {
                        Logger.write( typeof( AppManager ) + ".undo; ex=" + ex + "\n" );
                        serr.println( typeof( AppManager ) + ".undo; ex=" + ex );
                    }
                }
                editHistory.registerAfterUndo( inv );

                cleanupDeadSelection( before_ids );
                itemSelection.updateSelectedEventInstance();
            }
        }

        /// <summary>
        /// リドゥ処理を行います。
        /// </summary>
        public static void redo()
        {
            if ( editHistory.hasRedoHistory() ) {
                Vector<ValuePair<Integer, Integer>> before_ids = new Vector<ValuePair<Integer, Integer>>();
                for ( Iterator<SelectedEventEntry> itr = itemSelection.getEventIterator(); itr.hasNext(); ) {
                    SelectedEventEntry item = itr.next();
                    before_ids.add( new ValuePair<Integer, Integer>( item.track, item.original.InternalID ) );
                }

                ICommand run_src = editHistory.getRedo();
                CadenciiCommand run = (CadenciiCommand)run_src;
                if ( run.vsqCommand != null ) {
                    if ( run.vsqCommand.Type == VsqCommandType.TRACK_DELETE ) {
                        int track = (Integer)run.args[0];
                        if ( track == getSelected() && track >= 2 ) {
                            setSelected( track - 1 );
                        }
                    }
                }
                ICommand inv = mVsq.executeCommand( run );
                if ( run.type == CadenciiCommandType.BGM_UPDATE ) {
                    try {
#if JAVA
                        updateBgmStatusRequiredEvent.raise( typeof( AppManager ), new EventArgs() );
#elif QT_VERSION
                        updateBgmStatusRequired( this, null );
#else
                        if ( UpdateBgmStatusRequired != null ) {
                            UpdateBgmStatusRequired.Invoke( typeof( AppManager ), new EventArgs() );
                        }
#endif
                    } catch ( Exception ex ) {
                        Logger.write( typeof( AppManager ) + ".redo; ex=" + ex + "\n" );
                        serr.println( typeof( AppManager ) + ".redo; ex=" + ex );
                    }
                }
                editHistory.registerAfterRedo( inv );

                cleanupDeadSelection( before_ids );
                itemSelection.updateSelectedEventInstance();
            }
        }

        /// <summary>
        /// 「選択されている」と登録されているオブジェクトのうち、Undo, Redoなどによって存在しなくなったものを登録解除する
        /// </summary>
        public static void cleanupDeadSelection( Vector<ValuePair<Integer, Integer>> before_ids )
        {
            int size = mVsq.Track.size();
            for ( Iterator<ValuePair<Integer, Integer>> itr = before_ids.iterator(); itr.hasNext(); ) {
                ValuePair<Integer, Integer> specif = itr.next();
                boolean found = false;
                int track = specif.getKey();
                int internal_id = specif.getValue();
                if ( 1 <= track && track < size ) {
                    for ( Iterator<VsqEvent> itr2 = mVsq.Track.get( track ).getNoteEventIterator(); itr2.hasNext(); ) {
                        VsqEvent item = itr2.next();
                        if ( item.InternalID == internal_id ) {
                            found = true;
                            break;
                        }
                    }
                }
                if ( !found ) {
                    AppManager.itemSelection.removeEvent( internal_id );
                }
            }
        }
#endif

        /// <summary>
        /// 現在選択されているツールを取得します。
        /// </summary>
        /// <returns></returns>
        public static EditTool getSelectedTool()
        {
            return mSelectedTool;
        }

        /// <summary>
        /// 現在選択されているツールを設定します。
        /// </summary>
        /// <param name="value"></param>
        public static void setSelectedTool( EditTool value )
        {
            EditTool old = mSelectedTool;
            mSelectedTool = value;
            if ( old != mSelectedTool ) {
                try {
#if JAVA
                    selectedToolChangedEvent.raise( typeof( AppManager ), new EventArgs() );
#elif QT_VERSION
                    selectedToolChanged( this, null );
#else
                    if ( SelectedToolChanged != null ) {
                        SelectedToolChanged.Invoke( typeof( AppManager ), new EventArgs() );
                    }
#endif

                } catch ( Exception ex ) {
                    serr.println( "AppManager#setSelectedTool; ex=" + ex );
                    Logger.write( typeof( AppManager ) + ".setSelectedTool; ex=" + ex + "\n" );
                }
            }
        }

        public static boolean isOverlay()
        {
            return mOverlay;
        }

        public static void setOverlay( boolean value )
        {
            mOverlay = value;
        }

        public static boolean getRenderRequired( int track )
        {
            if ( mVsq == null ) {
                return false;
            }
            return mVsq.editorStatus.renderRequired[track - 1];
        }

        public static void setRenderRequired( int track, boolean value )
        {
            if ( mVsq == null ) {
                return;
            }
            mVsq.editorStatus.renderRequired[track - 1] = value;
        }

        /// <summary>
        /// 現在の編集モードを取得します．
        /// </summary>
        public static EditMode getEditMode()
        {
            return mEditMode;
        }

        /// <summary>
        /// 現在の編集モードを設定します．
        /// </summary>
        /// <param name="value"></param>
        public static void setEditMode( EditMode value )
        {
            mEditMode = value;
        }

        /// <summary>
        /// グリッドを表示するか否かを表す値を取得します
        /// </summary>
        public static boolean isGridVisible()
        {
            return mGridVisible;
        }

        /// <summary>
        /// グリッドを表示するか否かを設定します
        /// </summary>
        /// <param name="value"></param>
        public static void setGridVisible( boolean value )
        {
            if ( value != mGridVisible ) {
                mGridVisible = value;
                try {
#if JAVA
                    gridVisibleChangedEvent.raise( typeof( AppManager ), new BEventArgs() );
#elif QT_VERSION
                    gridVisibleChanged( this, null );
#else
                    if ( GridVisibleChanged != null ) {
                        GridVisibleChanged.Invoke( typeof( AppManager ), new EventArgs() );
                    }
#endif
                } catch ( Exception ex ) {
                    serr.println( "AppManager#setGridVisible; ex=" + ex );
                    Logger.write( typeof( AppManager ) + ".setGridVisible; ex=" + ex + "\n" );
                }
            }
        }

        /// <summary>
        /// 現在のプレビューがリピートモードであるかどうかを表す値を取得します
        /// </summary>
        public static boolean isRepeatMode()
        {
            return mRepeatMode;
        }

        /// <summary>
        /// 現在のプレビューがリピートモードかどうかを設定します
        /// </summary>
        /// <param name="value"></param>
        public static void setRepeatMode( boolean value )
        {
            mRepeatMode = value;
        }

        /// <summary>
        /// 現在プレビュー中かどうかを示す値を取得します
        /// </summary>
        public static boolean isPlaying()
        {
            return mPlaying;
        }

        /// <summary>
        /// プレビュー再生中かどうかを設定します．このプロパティーを切り替えることで，再生の開始と停止を行います．
        /// </summary>
        /// <param name="value"></param>
        /// <param name="form"></param>
        public static void setPlaying( boolean value, FormMain form )
        {
#if DEBUG
            sout.println( "AppManager#setPlaying; value=" + value );
#endif
            lock ( mLockerPlayingProperty ) {
                boolean previous = mPlaying;
                mPlaying = value;
                if ( previous != mPlaying ) {
                    if ( mPlaying ) {
                        try {
                            if( previewStart( form ) ){
#if DEBUG
                                sout.println( "AppManager#setPlaying; previewStart returns true" );
#endif
                                mPlaying = false;
                                return;
                            }
#if JAVA
                            previewStartedEvent.raise( typeof( AppManager ), new BEventArgs() );
#elif QT_VERSION
                            previewStarted( this, null );
#else
                            if ( PreviewStarted != null ) {
                                PreviewStarted.Invoke( typeof( AppManager ), new EventArgs() );
                            }
#endif
                        } catch ( Exception ex ) {
                            serr.println( "AppManager#setPlaying; ex=" + ex );
                            Logger.write( typeof( AppManager ) + ".setPlaying; ex=" + ex + "\n" );
#if JAVA
                            ex.printStackTrace();
#endif
                        }
                    } else if ( !mPlaying ) {
                        try {
                            previewStop();
#if DEBUG
                            sout.println( "AppManager#setPlaying; raise previewAbortedEvent" );
#endif
#if JAVA
                            previewAbortedEvent.raise( typeof( AppManager ), new BEventArgs() );
#elif QT_VERSION
                            previewAbortedEvent( this, null );
#else
                            if ( PreviewAborted != null ) {
                                PreviewAborted.Invoke( typeof( AppManager ), new EventArgs() );
                            }
#endif
                        } catch ( Exception ex ) {
                            serr.println( "AppManager#setPlaying; ex=" + ex );
                            Logger.write( typeof( AppManager ) + ".setPlaying; ex=" + ex + "\n" );
#if JAVA
                            ex.printStackTrace();
#endif
                        }
                    }
                }
            }
        }

        /// <summary>
        /// _vsq_fileにセットされたvsqファイルの名前を取得します。
        /// </summary>
        public static String getFileName()
        {
            return mFile;
        }

        private static void saveToCor( String file )
        {
            boolean hide = false;
            if ( mVsq != null ) {
                String path = PortUtil.getDirectoryName( file );
                //String file2 = fsys.combine( path, PortUtil.getFileNameWithoutExtension( file ) + ".vsq" );
                mVsq.writeAsXml( file );
                //mVsq.write( file2 );
#if !JAVA
                if ( hide ) {
                    try {
                        System.IO.File.SetAttributes( file, System.IO.FileAttributes.Hidden );
                        //System.IO.File.SetAttributes( file2, System.IO.FileAttributes.Hidden );
                    } catch ( Exception ex ) {
                        serr.println( "AppManager#saveToCor; ex=" + ex );
                        Logger.write( typeof( AppManager ) + ".saveToCor; ex=" + ex + "\n" );
                    }
                }
#endif
            }
        }

        public static void saveTo( String file )
        {
            if ( mVsq != null ) {
                if ( editorConfig.UseProjectCache ) {
                    // キャッシュディレクトリの処理
                    String dir = PortUtil.getDirectoryName( file );
                    String name = PortUtil.getFileNameWithoutExtension( file );
                    String cacheDir = fsys.combine( dir, name + ".cadencii" );

                    if ( !fsys.isDirectoryExists( cacheDir ) ) {
                        try {
                            PortUtil.createDirectory( cacheDir );
                        } catch ( Exception ex ) {
                            serr.println( "AppManager#saveTo; ex=" + ex );
                            showMessageBox( PortUtil.formatMessage( _( "failed creating cache directory, '{0}'." ), cacheDir ),
                                            _( "Info." ),
                                            PortUtil.OK_OPTION,
                                            com.github.cadencii.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE );
                            Logger.write( typeof( AppManager ) + ".saveTo; ex=" + ex + "\n" );
                            return;
                        }
                    }

                    String currentCacheDir = getTempWaveDir();
                    if ( !currentCacheDir.Equals( cacheDir ) ) {
                        for ( int i = 1; i < mVsq.Track.size(); i++ ) {
                            String wavFrom = fsys.combine( currentCacheDir, i + ".wav" );
                            String wavTo = fsys.combine( cacheDir, i + ".wav" );
                            if ( fsys.isFileExists( wavFrom ) ) {
                                if ( fsys.isFileExists( wavTo ) ) {
                                    try {
                                        PortUtil.deleteFile( wavTo );
                                    } catch ( Exception ex ) {
                                        serr.println( "AppManager#saveTo; ex=" + ex );
                                        Logger.write( typeof( AppManager ) + ".saveTo; ex=" + ex + "\n" );
                                    }
                                }
                                try {
                                    PortUtil.moveFile( wavFrom, wavTo );
                                } catch ( Exception ex ) {
                                    serr.println( "AppManager#saveTo; ex=" + ex );
                                    showMessageBox( PortUtil.formatMessage( _( "failed copying WAVE cache file, '{0}'." ), wavFrom ),
                                                    _( "Error" ),
                                                    PortUtil.OK_OPTION,
                                                    com.github.cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE );
                                    Logger.write( typeof( AppManager ) + ".saveTo; ex=" + ex + "\n" );
                                    break;
                                }
                            }

                            String xmlFrom = fsys.combine( currentCacheDir, i + ".xml" );
                            String xmlTo = fsys.combine( cacheDir, i + ".xml" );
                            if ( fsys.isFileExists( xmlFrom ) ) {
                                if ( fsys.isFileExists( xmlTo ) ) {
                                    try {
                                        PortUtil.deleteFile( xmlTo );
                                    } catch ( Exception ex ) {
                                        serr.println( "AppManager#saveTo; ex=" + ex );
                                        Logger.write( typeof( AppManager ) + ".saveTo; ex=" + ex + "\n" );
                                    }
                                }
                                try {
                                    PortUtil.moveFile( xmlFrom, xmlTo );
                                } catch ( Exception ex ) {
                                    serr.println( "AppManager#saveTo; ex=" + ex );
                                    showMessageBox( PortUtil.formatMessage( _( "failed copying XML cache file, '{0}'." ), xmlFrom ),
                                                    _( "Error" ),
                                                    PortUtil.OK_OPTION,
                                                    com.github.cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE );
                                    Logger.write( typeof( AppManager ) + ".saveTo; ex=" + ex + "\n" );
                                    break;
                                }
                            }
                        }

                        setTempWaveDir( cacheDir );
                    }
                    mVsq.cacheDir = cacheDir;
                }
            }

            saveToCor( file );

            if ( mVsq != null ) {
                mFile = file;
                editorConfig.pushRecentFiles( mFile );
                if ( !mAutoBackupTimer.isRunning() && editorConfig.AutoBackupIntervalMinutes > 0 ) {
                    double millisec = editorConfig.AutoBackupIntervalMinutes * 60.0 * 1000.0;
                    int draft = (int)millisec;
                    if ( millisec > int.MaxValue ) {
                        draft = int.MaxValue;
                    }
                    mAutoBackupTimer.setDelay( draft );
                    mAutoBackupTimer.start();
                }
            }
        }

        /// <summary>
        /// 現在の演奏マーカーの位置を取得します。
        /// </summary>
        public static int getCurrentClock()
        {
            return mCurrentClock;
        }

        /// <summary>
        /// 現在の演奏マーカーの位置を設定します。
        /// </summary>
        public static void setCurrentClock( int value )
        {
            mCurrentClock = value;
        }

        /// <summary>
        /// 現在選択されているトラックを取得または設定します
        /// </summary>
        public static int getSelected()
        {
            int tracks = mVsq.Track.size();
            if ( tracks <= mSelected ) {
                mSelected = tracks - 1;
            }
            return mSelected;
        }

        public static void setSelected( int value )
        {
            mSelected = value;
        }

#if !JAVA
        [Obsolete]
        public static int Selected
        {
            get
            {
                return getSelected();
            }
        }
#endif

        /// <summary>
        /// xvsqファイルを読込みます．キャッシュディレクトリの更新は行われません
        /// </summary>
        /// <param name="file"></param>
        /// <returns>ファイルの読み込みに成功した場合にtrueを，それ以外の場合はfalseを返します</returns>
        public static boolean readVsq( String file )
        {
            mSelected = 1;
            mFile = file;
            VsqFileEx newvsq = null;
            try {
                newvsq = VsqFileEx.readFromXml( file );
            } catch ( Exception ex ) {
                serr.println( "AppManager#readVsq; ex=" + ex );
#if JAVA
                ex.printStackTrace();
#endif
                Logger.write( typeof( AppManager ) + ".readVsq; ex=" + ex + "\n" );
                return true;
            }
            if ( newvsq == null ) {
                return true;
            }
            mVsq = newvsq;
            for ( int i = 0; i < mVsq.editorStatus.renderRequired.Length; i++ ) {
                if ( i < mVsq.Track.size() - 1 ) {
                    mVsq.editorStatus.renderRequired[i] = true;
                } else {
                    mVsq.editorStatus.renderRequired[i] = false;
                }
            }
            //mStartMarker = mVsq.getPreMeasureClocks();
            //int bar = mVsq.getPreMeasure() + 1;
            //mEndMarker = mVsq.getClockFromBarCount( bar );
            if ( mVsq.Track.size() >= 1 ) {
                mSelected = 1;
            } else {
                mSelected = -1;
            }
            try {
#if JAVA
                updateBgmStatusRequiredEvent.raise( typeof( AppManager ), new EventArgs() );
#elif QT_VERSION
                updateBgmStatusRequired( this, null );
#else
                if ( UpdateBgmStatusRequired != null ) {
                    UpdateBgmStatusRequired.Invoke( typeof( AppManager ), new EventArgs() );
                }
#endif
            } catch ( Exception ex ) {
                Logger.write( typeof( AppManager ) + ".readVsq; ex=" + ex + "\n" );
                serr.println( typeof( AppManager ) + ".readVsq; ex=" + ex );
            }
            return false;
        }

#if !TREECOM
        /// <summary>
        /// vsqファイル。
        /// </summary>
        public static VsqFileEx getVsqFile()
        {
            return mVsq;
        }

#if !JAVA
        [Obsolete]
        public static VsqFileEx VsqFile
        {
            get
            {
                return getVsqFile();
            }
        }
#endif
#endif

        public static void setVsqFile( VsqFileEx vsq )
        {
            mVsq = vsq;
            for ( int i = 0; i < mVsq.editorStatus.renderRequired.Length; i++ ) {
                if ( i < mVsq.Track.size() - 1 ) {
                    mVsq.editorStatus.renderRequired[i] = true;
                } else {
                    mVsq.editorStatus.renderRequired[i] = false;
                }
            }
            mFile = "";
            //mStartMarker = mVsq.getPreMeasureClocks();
            //int bar = mVsq.getPreMeasure() + 1;
            //mEndMarker = mVsq.getClockFromBarCount( bar );
            mAutoBackupTimer.stop();
            setCurrentClock( mVsq.getPreMeasureClocks() );
            try {
#if JAVA
                updateBgmStatusRequiredEvent.raise( typeof( AppManager ), new EventArgs() );
#elif QT_VERSION
                updateBgmStatusRequired( this, null );
#else
                if ( UpdateBgmStatusRequired != null ) {
                    UpdateBgmStatusRequired.Invoke( typeof( AppManager ), new EventArgs() );
                }
#endif
            } catch ( Exception ex ) {
                Logger.write( typeof( AppManager ) + ".setVsqFile; ex=" + ex + "\n" );
                serr.println( typeof( AppManager ) + ".setVsqFile; ex=" + ex );
            }
        }

        public static void init()
        {
            loadConfig();
            clipboard = new ClipboardModel();
            itemSelection = new ItemSelectionModel();
            editHistory = new EditHistoryModel();
#if !JAVA
            // UTAU歌手のアイコンを読み込み、起動画面に表示を要求する
            int c = editorConfig.UtauSingers.size();
            for ( int i = 0; i < c; i++ ) {
                SingerConfig sc = editorConfig.UtauSingers.get( i );
                if ( sc == null ) {
                    continue;
                }
                String dir = sc.VOICEIDSTR;
                SingerConfig sc_temp = new SingerConfig();
                String path_image = Utility.readUtauSingerConfig( dir, sc_temp );

#if DEBUG
                sout.println( "AppManager#init; path_image=" + path_image );
#endif
                if ( Cadencii.splash != null ) {
                    try {
                        Cadencii.splash.addIconThreadSafe( path_image, sc.VOICENAME );
                    } catch ( Exception ex ) {
                        serr.println( "AppManager#init; ex=" + ex );
                        Logger.write( typeof( AppManager ) + ".init; ex=" + ex + "\n" );
                    }
                }
            }
#endif

#if JAVA
            // getvocaloidinfo.exeを呼ぶ
            String tmp = PortUtil.createTempFile();
            String prefix = Utility.normalizePath( editorConfig.WinePrefix );
#if JAVA_MAC
            // wine経由でユーティリティを呼ぶ
            String vocaloidrv_sh = fsys.combine( PortUtil.getApplicationStartupPath(), "vocaloidrv.sh" );
            String winetop = Utility.normalizePath( editorConfig.WineTop );
            String getvocaloidinfo =
                fsys.combine( PortUtil.getApplicationStartupPath(), "getvocaloidinfo.exe" );
#if DEBUG
            sout.println( "AppManager#init; isFileExists(getvocaloidinfo)=" + fsys.isFileExists( getvocaloidinfo ) );
            sout.println( "AppManager#init; isFileExists(vocaloidrv_sh)=" + fsys.isFileExists( vocaloidrv_sh ) );
            sout.println( "AppManager#init; isDirectoryExists(winetop)=" + fsys.isDirectoryExists( winetop ) );
            sout.println( "AppManager#init; isDirectoryExists(prefix)=" + fsys.isDirectoryExists( prefix ) );
#endif // DEBUG
            try{
                Process p = Runtime.getRuntime().exec(
                    new String[]{
                        "/bin/sh",
                        vocaloidrv_sh,
                        prefix,
                        winetop,
                        getvocaloidinfo,
                        tmp,
                     } );
                while( true ){
                    try{
                        p.exitValue();
                    }catch( Exception ex0 ){
                        continue;
                    }
                    break;
                }
            }catch( Exception ex ){
                ex.printStackTrace();
            }
#else // JAVA_MAC
            //TODO:
#endif // JAVA_MAC

            // 戻りのテキストファイルを読み込む
            Vector<String> reg_list = new Vector<String>();
            BufferedReader br = null;
            try{
                br = new BufferedReader( new InputStreamReader( new FileInputStream( tmp ), "Shift_JIS" ) );
                String line = "";
                while( (line = br.readLine()) != null ){
#if DEBUG
                    sout.println( "AppManager#init; line=" + line );
#endif // DEBUG
                    reg_list.add( line );
                }
            }catch( Exception ex ){
            }finally{
                if( br != null ){
                    try{
                        br.close();
                    }catch( Exception ex2 ){
                    }
                }
            }
#if !DEBUG
            try{
                PortUtil.deleteFile( tmp );
            }catch( Exception ex ){
            }
#endif // !DEBUG
            VocaloSysUtil.init( reg_list, prefix );
#else // JAVA
            VocaloSysUtil.init();
#endif // JAVA

            editorConfig.check();
            keyWidth = editorConfig.KeyWidth;
            VSTiDllManager.init();
#if !JAVA
            // アイコンパレード, VOCALOID1
            SingerConfigSys singer_config_sys1 = VocaloSysUtil.getSingerConfigSys( SynthesizerType.VOCALOID1 );
            if ( singer_config_sys1 != null ) {
                foreach ( SingerConfig sc in singer_config_sys1.getInstalledSingers() ) {
                    if ( sc == null ) {
                        continue;
                    }
                    String name = sc.VOICENAME.ToLower();
                    String path_image = fsys.combine(
                                            fsys.combine(
                                                PortUtil.getApplicationStartupPath(), "resources" ),
                                            name + ".png" );
#if DEBUG
                    sout.println( "AppManager#init; path_image=" + path_image );
#endif
                    if ( Cadencii.splash != null ) {
                        try {
                            Cadencii.splash.addIconThreadSafe( path_image, sc.VOICENAME );
                        } catch ( Exception ex ) {
                            serr.println( "AppManager#init; ex=" + ex );
                            Logger.write( typeof( AppManager ) + ".init; ex=" + ex + "\n" );
                        }
                    }
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
                    String path_image = fsys.combine(
                                            fsys.combine(
                                                PortUtil.getApplicationStartupPath(), "resources" ),
                                            name + ".png" );
#if DEBUG
                    sout.println( "AppManager#init; path_image=" + path_image );
#endif
                    if ( Cadencii.splash != null ) {
                        try {
                            Cadencii.splash.addIconThreadSafe( path_image, sc.VOICENAME );
                        } catch ( Exception ex ) {
                            serr.println( "AppManager#init; ex=" + ex );
                            Logger.write( typeof( AppManager ) + ".init; ex=" + ex + "\n" );
                        }
                    }
                }
            }
#endif

            PlaySound.init();
            mLocker = new Object();
            // VOCALOID2の辞書を読み込み
            SymbolTable.loadSystemDictionaries();
            // 日本語辞書
            SymbolTable.loadDictionary(
                fsys.combine( fsys.combine( PortUtil.getApplicationStartupPath(), "resources" ), "dict_ja.txt" ),
                "DEFAULT_JP" );
            // 英語辞書
            SymbolTable.loadDictionary(
                fsys.combine( fsys.combine( PortUtil.getApplicationStartupPath(), "resources" ), "dict_en.txt" ),
                "DEFAULT_EN" );
            // 拡張辞書
            SymbolTable.loadAllDictionaries( fsys.combine( PortUtil.getApplicationStartupPath(), "udic" ) );
            //VSTiProxy.CurrentUser = "";
#if JAVA
            Util.isApplyFontRecurseEnabled = false;
#endif

            // 辞書の設定を適用
            try {
                // 現在辞書リストに読込済みの辞書を列挙
                Vector<ValuePair<String, Boolean>> current = new Vector<ValuePair<String, Boolean>>();
                for ( int i = 0; i < SymbolTable.getCount(); i++ ) {
                    current.add( new ValuePair<String, Boolean>( SymbolTable.getSymbolTable( i ).getName(), false ) );
                }
                // editorConfig.UserDictionariesの設定値をコピー
                Vector<ValuePair<String, Boolean>> config_data = new Vector<ValuePair<String, Boolean>>();
                int count = editorConfig.UserDictionaries.size();
                for ( int i = 0; i < count; i++ ) {
                    String[] spl = PortUtil.splitString( editorConfig.UserDictionaries.get( i ), new char[] { '\t' }, 2 );
                    config_data.add( new ValuePair<String, Boolean>( spl[0], (spl[1].Equals( "T" ) ? true : false) ) );
#if DEBUG
                    AppManager.debugWriteLine( "    " + spl[0] + "," + spl[1] );
#endif
                }
                // 辞書リストとeditorConfigの設定を比較する
                // currentの方には、editorConfigと共通するものについてのみsetValue(true)とする
                Vector<ValuePair<String, Boolean>> common = new Vector<ValuePair<String, Boolean>>();
                for ( int i = 0; i < config_data.size(); i++ ) {
                    for ( int j = 0; j < current.size(); j++ ) {
                        if ( config_data.get( i ).getKey().Equals( current.get( j ).getKey() ) ) {
                            // editorConfig.UserDictionariesにもKeyが含まれていたらtrue
                            current.get( j ).setValue( true );
                            common.add( new ValuePair<String, Boolean>( config_data.get( i ).getKey(), config_data.get( i ).getValue() ) );
                            break;
                        }
                    }
                }
                // editorConfig.UserDictionariesに登録されていないが、辞書リストには読み込まれている場合。
                // この場合は、デフォルトでENABLEとし、優先順位を最後尾とする。
                for ( int i = 0; i < current.size(); i++ ) {
                    if ( !current.get( i ).getValue() ) {
                        common.add( new ValuePair<String, Boolean>( current.get( i ).getKey(), true ) );
                    }
                }
                SymbolTable.changeOrder( common );
            } catch ( Exception ex ) {
                serr.println( "AppManager#init; ex=" + ex );
            }

#if !TREECOM
            mID = PortUtil.getMD5FromString( (long)PortUtil.getCurrentTime() + "" ).Replace( "_", "" );
            mTempWaveDir = fsys.combine( getCadenciiTempDir(), mID );
            if ( !fsys.isDirectoryExists( mTempWaveDir ) ) {
                PortUtil.createDirectory( mTempWaveDir );
            }
            String log = fsys.combine( getTempWaveDir(), "run.log" );
#endif

            reloadUtauVoiceDB();

            mAutoBackupTimer = new BTimer();
#if JAVA
            mAutoBackupTimer.tickEvent.add( new BEventHandler( AppManager.class, "handleAutoBackupTimerTick" ) );
#else
            mAutoBackupTimer.Tick += new BEventHandler( handleAutoBackupTimerTick );
#endif
        }

        /// <summary>
        /// utauVoiceDBフィールドのリストを一度クリアし，
        /// editorConfig.Utausingersの情報を元に最新の情報に更新します
        /// </summary>
        public static void reloadUtauVoiceDB()
        {
            mUtauVoiceDB.clear();
            for ( Iterator<SingerConfig> itr = editorConfig.UtauSingers.iterator(); itr.hasNext(); ) {
                SingerConfig config = itr.next();

                // 通常のUTAU音源
                UtauVoiceDB db = null;
                try {
                    db = new UtauVoiceDB( config );
                } catch ( Exception ex ) {
                    serr.println( "AppManager#reloadUtauVoiceDB; ex=" + ex );
                    db = null;
                    Logger.write( typeof( AppManager ) + ".reloadUtauVoiceDB; ex=" + ex + "\n" );
                }
                if ( db != null ) {
                    mUtauVoiceDB.put( config.VOICEIDSTR, db );
                }
            }
        }

        /// <summary>
        /// 位置クオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
        /// </summary>
        /// <returns></returns>
        public static int getPositionQuantizeClock()
        {
            return QuantizeModeUtil.getQuantizeClock( editorConfig.getPositionQuantize(), editorConfig.isPositionQuantizeTriplet() );
        }

        /// <summary>
        /// 音符長さクオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
        /// </summary>
        /// <returns></returns>
        public static int getLengthQuantizeClock()
        {
            return QuantizeModeUtil.getQuantizeClock( editorConfig.getLengthQuantize(), editorConfig.isLengthQuantizeTriplet() );
        }

        public static void serializeEditorConfig( EditorConfig instance, String file )
        {
            FileOutputStream fs = null;
            try {
                fs = new FileOutputStream( file );
                EditorConfig.getSerializer().serialize( fs, instance );
            } catch ( Exception ex ) {
                Logger.write( typeof( EditorConfig ) + ".serialize; ex=" + ex + "\n" );
            } finally {
                if ( fs != null ) {
                    try {
                        fs.close();
                    } catch ( Exception ex2 ) {
                        Logger.write( typeof( EditorConfig ) + ".serialize; ex=" + ex2 + "\n" );
                    }
                }
            }
        }

        public static EditorConfig deserializeEditorConfig( String file )
        {
            EditorConfig ret = null;
            FileInputStream fs = null;
            try {
                fs = new FileInputStream( file );
                ret = (EditorConfig)EditorConfig.getSerializer().deserialize( fs );
            } catch ( Exception ex ) {
#if JAVA
                serr.println( "EditorConfig#deserialize; ex=" + ex );
                ex.printStackTrace();
#endif
                Logger.write( typeof( EditorConfig ) + ".deserialize; ex=" + ex + "\n" );
            } finally {
                if ( fs != null ) {
                    try {
                        fs.close();
                    } catch ( Exception ex2 ) {
                        Logger.write( typeof( EditorConfig ) + ".deserialize; ex=" + ex2 + "\n" );
                    }
                }
            }

            if ( ret == null ) {
                return null;
            }

            if ( mMainWindow != null ) {
                Vector<ValuePairOfStringArrayOfKeys> defs = mMainWindow.getDefaultShortcutKeys();
                for ( int j = 0; j < defs.size(); j++ ) {
                    boolean found = false;
                    for ( int i = 0; i < ret.ShortcutKeys.size(); i++ ) {
                        if ( defs.get( j ).Key.Equals( ret.ShortcutKeys.get( i ).Key ) ) {
                            found = true;
                            break;
                        }
                    }
                    if ( !found ) {
                        ret.ShortcutKeys.add( defs.get( j ) );
                    }
                }
            }

            // バッファーサイズを正規化
            if ( ret.BufferSizeMilliSeconds < EditorConfig.MIN_BUFFER_MILLIXEC ) {
                ret.BufferSizeMilliSeconds = EditorConfig.MIN_BUFFER_MILLIXEC;
            } else if ( EditorConfig.MAX_BUFFER_MILLISEC < ret.BufferSizeMilliSeconds ) {
                ret.BufferSizeMilliSeconds = EditorConfig.MAX_BUFFER_MILLISEC;
            }
            return ret;
        }

        /// <summary>
        /// 現在の設定を設定ファイルに書き込みます。
        /// </summary>
        public static void saveConfig()
        {
            // ユーザー辞書の情報を取り込む
            editorConfig.UserDictionaries.clear();
            int count = SymbolTable.getCount();
            for ( int i = 0; i < count; i++ ) {
                SymbolTable table = SymbolTable.getSymbolTable( i );
                editorConfig.UserDictionaries.add( table.getName() + "\t" + (table.isEnabled() ? "T" : "F") );
            }
            editorConfig.KeyWidth = keyWidth;

#if !JAVA
            // chevronの幅を保存
            if ( Rebar.CHEVRON_WIDTH > 0 ) {
                editorConfig.ChevronWidth = Rebar.CHEVRON_WIDTH;
            }
#endif

            // シリアライズして保存
            String file = fsys.combine( Utility.getConfigPath(), CONFIG_FILE_NAME );
#if DEBUG
            sout.println( "AppManager#saveConfig; file=" + file );
#endif
            try {
                serializeEditorConfig( editorConfig, file );
            } catch ( Exception ex ) {
                serr.println( "AppManager#saveConfig; ex=" + ex );
                Logger.write( typeof( AppManager ) + ".saveConfig; ex=" + ex + "\n" );
#if JAVA
                ex.printStackTrace();
#endif
            }
        }

        /// <summary>
        /// 設定ファイルを読み込みます。
        /// 設定ファイルが壊れていたり存在しない場合、デフォルトの設定が使われます。
        /// </summary>
        public static void loadConfig()
        {
            String appdata = Utility.getApplicationDataPath();
#if DEBUG
            sout.println( "AppManager#loadConfig; appdata=" + appdata );
#endif
            if ( appdata.Equals( "" ) ) {
                editorConfig = new EditorConfig();
                return;
            }

            // バージョン番号付きのファイル
            String config_file = fsys.combine( Utility.getConfigPath(), CONFIG_FILE_NAME );
#if DEBUG
            sout.println( "AppManager#loadConfig; config_file=" + config_file );
#endif
            EditorConfig ret = null;
            if ( fsys.isFileExists( config_file ) ) {
                // このバージョン用の設定ファイルがあればそれを利用
                try {
                    ret = deserializeEditorConfig( config_file );
                } catch ( Exception ex ) {
                    serr.println( "AppManager#loadConfig; ex=" + ex );
                    ret = null;
                    Logger.write( typeof( AppManager ) + ".loadConfig; ex=" + ex + "\n" );
                }
            } else {
                // このバージョン用の設定ファイルがなかった場合
                // まず，古いバージョン用の設定ファイルがないかどうか順に調べる
                String[] dirs0 = PortUtil.listDirectories( appdata );
                // 数字と，2個以下のピリオドからなるディレクトリ名のみを抽出
                Vector<VersionString> dirs = new Vector<VersionString>();
                foreach ( String s0 in dirs0 ) {
                    String s = PortUtil.getFileName( s0 );
                    int length = PortUtil.getStringLength( s );
                    boolean register = true;
                    int num_period = 0;
                    for ( int i = 0; i < length; i++ ) {
                        char c = PortUtil.charAt( s, i );
                        if ( c == '.' ) {
                            num_period++;
                        } else {
#if JAVA
                            if ( Character.isDigit( c ) ) {
#else
                            if ( !char.IsNumber( c ) ) {
#endif
                                register = false;
                                break;
                            }
                        }
                    }
                    if ( register && num_period <= 2 ) {
                        try {
                            VersionString vs = new VersionString( s );
                            dirs.add( vs );
                        } catch ( Exception ex ) {
                        }
                    }
                }

                // 並べ替える
                boolean changed = true;
                int size = dirs.size();
                while ( changed ) {
                    changed = false;
                    for ( int i = 0; i < size - 1; i++ ) {
                        VersionString item1 = dirs.get( i );
                        VersionString item2 = dirs.get( i + 1 );
                        if ( item1.compareTo( item2 ) > 0 ) {
                            dirs.set( i, item2 );
                            dirs.set( i + 1, item1 );
                            changed = true;
                        }
                    }
                }

                // バージョン番号付きの設定ファイルを新しい順に読み込みを試みる
                VersionString vs_this = new VersionString( BAssemblyInfo.fileVersionMeasure + "." + BAssemblyInfo.fileVersionMinor );
                for ( int i = size - 1; i >= 0; i-- ) {
                    VersionString vs = dirs.get( i );
                    if ( vs_this.compareTo( vs ) < 0 ) {
                        // 自分自身のバージョンより新しいものは
                        // 読み込んではいけない
                        continue;
                    }
                    config_file = fsys.combine( fsys.combine( appdata, vs.getRawString() ), CONFIG_FILE_NAME );
                    if ( fsys.isFileExists( config_file ) ) {
                        try {
                            ret = deserializeEditorConfig( config_file );
                        } catch ( Exception ex ) {
                            Logger.write( typeof( AppManager ) + ".loadConfig; ex=" + ex + "\n" );
                            ret = null;
                        }
                        if ( ret != null ) {
                            break;
                        }
                    }
                }

                // それでも読み込めなかった場合，旧来のデフォルトの位置にある
                // 設定ファイルを読みに行く
                if ( ret == null ) {
                    config_file = fsys.combine( appdata, CONFIG_FILE_NAME );
                    if ( fsys.isFileExists( config_file ) ) {
                        try {
                            ret = deserializeEditorConfig( config_file );
                        } catch ( Exception ex ) {
                            serr.println( "AppManager#locdConfig; ex=" + ex );
                            ret = null;
                            Logger.write( typeof( AppManager ) + ".loadConfig; ex=" + ex + "\n" );
                        }
                    }
                }
            }

            // 設定ファイルの読み込みが悉く失敗した場合，
            // デフォルトの設定とする．
            if ( ret == null ) {
                ret = new EditorConfig();
            }
            editorConfig = ret;

            keyWidth = editorConfig.KeyWidth;
        }

        public static VsqID getSingerIDUtau( int language, int program )
        {
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

        public static SingerConfig getSingerInfoUtau( int language, int program )
        {
            int index = language << 7 | program;
            if ( 0 <= index && index < editorConfig.UtauSingers.size() ) {
                return editorConfig.UtauSingers.get( index );
            } else {
                return null;
            }
        }

        /// <summary>
        /// TODO: 廃止する。AquesToneDriver から取得するようにする
        /// </summary>
        /// <param name="program_change"></param>
        /// <returns></returns>
        public static SingerConfig getSingerInfoAquesTone( int program_change )
        {
#if ENABLE_AQUESTONE
            return AquesToneDriver.getSingerConfig( program_change );
#else
            return null;
#endif
        }

        private static VsqID createAquesToneSingerID( int program, Func<int, SingerConfig> get_singer_config )
        {
            VsqID ret = new VsqID( 0 );
            ret.type = VsqIDType.Singer;
            SingerConfig config = null;
            if ( get_singer_config != null ) {
                config = get_singer_config( program );
            }
            if ( config != null ) {
                int lang = 0;
                ret.IconHandle = new IconHandle();
                ret.IconHandle.IconID = "$0701" + PortUtil.toHexString( lang, 2 ) + PortUtil.toHexString( program, 2 );
                ret.IconHandle.IDS = config.VOICENAME;
                ret.IconHandle.Index = 0;
                ret.IconHandle.Language = lang;
                ret.IconHandle.setLength( 1 );
                ret.IconHandle.Original = lang << 8 | program;
                ret.IconHandle.Program = program;
                ret.IconHandle.Caption = "";
            } else {
                ret.IconHandle = new IconHandle();
                ret.IconHandle.Program = 0;
                ret.IconHandle.Language = 0;
                ret.IconHandle.IconID = "$0701" + PortUtil.toHexString( 0, 2 ) + PortUtil.toHexString( 0, 2 );
                ret.IconHandle.IDS = "Unknown";
                ret.type = VsqIDType.Singer;
            }
            return ret;
        }

        public static VsqID getSingerIDAquesTone( int program )
        {
#if ENABLE_AQUESTONE
            return createAquesToneSingerID(program, AquesToneDriver.getSingerConfig);
#else
            return createAquesToneSingerID( program, null );
#endif
        }

        public static VsqID getSingerIDAquesTone2( int program )
        {
#if ENABLE_AQUESTONE
            return createAquesToneSingerID( program, AquesTone2Driver.getSingerConfig );
#else
            return createAquesToneSingerID( program, null );
#endif
        }

        public static Color getHilightColor()
        {
            return mHilightBrush;
        }

        public static void setHilightColor( Color value )
        {
            mHilightBrush = value;
        }

        /// <summary>
        /// ピアノロール上の音符の警告色を取得します．
        /// 音抜けの可能性がある音符の背景色として利用されます
        /// </summary>
        /// <returns></returns>
        public static Color getAlertColor()
        {
            return PortUtil.HotPink;
        }

        /// <summary>
        /// ピアノロール上の音符の警告色を取得します．
        /// 音抜けの可能性のある音符であって，かつ現在選択されている音符の背景色として利用されます．
        /// </summary>
        /// <returns></returns>
        public static Color getAlertHilightColor()
        {
            return PortUtil.DeepPink;
        }
    }

#if !JAVA
}
#endif
