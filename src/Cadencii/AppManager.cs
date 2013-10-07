/*
 * AppManager.cs
 * Copyright © 2009-2011 kbinani
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
//#define ENABLE_OBSOLUTE_COMMAND
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.CSharp;
using cadencii.apputil;
using cadencii.java.awt;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.media;
using cadencii.vsq;
using cadencii.windows.forms;
using cadencii.xml;
using cadencii.utau;

namespace cadencii
{

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
        private const string CONFIG_FILE_NAME = "config.xml";
        /// <summary>
        /// 強弱記号の，ピアノロール画面上の表示幅（ピクセル）
        /// </summary>
        public const int DYNAFF_ITEM_WIDTH = 40;
        public const int FONT_SIZE8 = 8;
        public const int FONT_SIZE9 = FONT_SIZE8 + 1;
        public const int FONT_SIZE10 = FONT_SIZE8 + 2;
        public const int FONT_SIZE50 = FONT_SIZE8 + 42;
        public const int MAX_NUM_TRACK = 16;

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
        public static XmlSerializer xmlSerializerListBezierCurves = new XmlSerializer(typeof(AttachedCurve));
        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont8 = new Font("Dialog", Font.PLAIN, FONT_SIZE8);
        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont9 = new Font("Dialog", Font.PLAIN, FONT_SIZE9);
        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont10 = new Font("Dialog", Font.PLAIN, FONT_SIZE10);
        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont10Bold = new Font("Dialog", Font.BOLD, FONT_SIZE10);
        /// <summary>
        /// 画面描画に使用する共用のフォントオブジェクト
        /// </summary>
        public static Font baseFont50Bold = new Font("Dialog", Font.BOLD, FONT_SIZE50);
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
        public static readonly string[] usingS = new string[] { "using System;",
                                             "using System.IO;",
                                             "using cadencii.vsq;",
                                             "using cadencii;",
                                             "using cadencii.java.io;",
                                             "using cadencii.java.util;",
                                             "using cadencii.java.awt;",
                                             "using cadencii.media;",
                                             "using cadencii.apputil;",
                                             "using System.Windows.Forms;",
                                             "using System.Collections.Generic;",
                                             "using System.Drawing;",
                                             "using System.Text;",
                                             "using System.Xml.Serialization;" };
        /// <summary>
        /// ショートカットキーとして受付可能なキーのリスト
        /// </summary>
        public static readonly List<Keys> SHORTCUT_ACCEPTABLE = new List<Keys>(new Keys[] {
            Keys.A,
            Keys.B,
            Keys.Back,
            Keys.C,
            Keys.D,
            Keys.D0,
            Keys.D1,
            Keys.D2,
            Keys.D3,
            Keys.D4,
            Keys.D5,
            Keys.D6,
            Keys.D7,
            Keys.D8,
            Keys.D9,
            Keys.Down,
            Keys.E,
            Keys.F,
            Keys.F1,
            Keys.F2,
            Keys.F3,
            Keys.F4,
            Keys.F5,
            Keys.F6,
            Keys.F7,
            Keys.F8,
            Keys.F9,
            Keys.F10,
            Keys.F11,
            Keys.F12,
            Keys.F13,
            Keys.F14,
            Keys.F15,
            Keys.F16,
            Keys.F17,
            Keys.F18,
            Keys.F19,
            Keys.F20,
            Keys.F21,
            Keys.F22,
            Keys.F23,
            Keys.F24,
            Keys.G,
            Keys.H,
            Keys.I,
            Keys.J,
            Keys.K,
            Keys.L,
            Keys.Left,
            Keys.M,
            Keys.N,
            Keys.NumPad0,
            Keys.NumPad1,
            Keys.NumPad2,
            Keys.NumPad3,
            Keys.NumPad4,
            Keys.NumPad5,
            Keys.NumPad6,
            Keys.NumPad7,
            Keys.NumPad8,
            Keys.NumPad9,
            Keys.O,
            Keys.P,
            Keys.PageDown,
            Keys.PageUp,
            Keys.Q,
            Keys.R,
            Keys.Right,
            Keys.S,
            Keys.Space,
            Keys.T,
            Keys.U,
            Keys.Up,
            Keys.V,
            Keys.W,
            Keys.X,
            Keys.Y,
            Keys.Z,
            Keys.Delete,
            Keys.Home,
            Keys.End,
        });
        /// <summary>
        /// UTAU関連のテキストファイルで受け付けるエンコーディングの種類
        /// </summary>
        public static readonly string[] TEXT_ENCODINGS_IN_UTAU = new string[] { "Shift_JIS", "UTF-16", "us-ascii" };
        /// <summary>
        /// よく使うボーダー線の色
        /// </summary>
        public static readonly Color COLOR_BORDER = new Color(118, 123, 138);
        #endregion

        #region Private Static Fields
        private static Color mHilightBrush = PortUtil.CornflowerBlue;
        private static Object mLocker;
        private static System.Windows.Forms.Timer mAutoBackupTimer;
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
        private static string mFile = "";
        private static int mSelected = 1;
        private static int mCurrentClock = 0;
        private static bool mPlaying = false;
        private static bool mRepeatMode = false;
        private static bool mGridVisible = false;
        private static EditMode mEditMode = EditMode.NONE;
        /// <summary>
        /// トラックのオーバーレイ表示
        /// </summary>
        private static bool mOverlay = true;
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
        private static bool mWholeSelectedIntervalEnabled = false;
        /// <summary>
        /// Ctrlキーを押しながらのマウスドラッグ操作による選択が行われた範囲(単位：クロック)
        /// </summary>
        public static SelectedRegion mWholeSelectedInterval = new SelectedRegion(0);
        /// <summary>
        /// コントロールカーブ上で現在選択されている範囲（x:クロック、y:各コントロールカーブの単位に同じ）。マウスが動いているときのみ使用
        /// </summary>
        public static Rectangle mCurveSelectingRectangle = new Rectangle();
        /// <summary>
        /// コントロールカーブ上で選択された範囲（単位：クロック）
        /// </summary>
        public static SelectedRegion mCurveSelectedInterval = new SelectedRegion(0);
        /// <summary>
        /// 選択範囲が有効かどうか。
        /// </summary>
        private static bool mCurveSelectedIntervalEnabled = false;
        /// <summary>
        /// 範囲選択モードで音符を動かしている最中の、選択範囲の開始位置（クロック）。マウスが動いているときのみ使用
        /// </summary>
        public static int mWholeSelectedIntervalStartForMoving = 0;
        #endregion

        /// <summary>
        /// 自動ノーマライズモードかどうかを表す値を取得、または設定します。
        /// </summary>
        public static bool mAutoNormalize = false;
        /// <summary>
        /// Bezierカーブ編集モードが有効かどうかを表す
        /// </summary>
        private static bool mIsCurveMode = false;
        /// <summary>
        /// 再生時に自動スクロールするかどうか
        /// </summary>
        public static bool mAutoScroll = true;
        /// <summary>
        /// プレビュー再生が開始された時刻
        /// </summary>
        public static double mPreviewStartedTime;
        /// <summary>
        /// 現在選択中のパレットアイテムの名前
        /// </summary>
        public static string mSelectedPaletteTool = "";
        /// <summary>
        /// このCadenciiのID。起動ごとにユニークな値が設定され、一時フォルダのフォルダ名等に使用する
        /// </summary>
        private static string mID = "";
        /// <summary>
        /// ダイアログを表示中かどうか
        /// </summary>
        private static bool mShowingDialog = false;
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
        public static List<DrawObject>[] mDrawObjects = new List<DrawObject>[MAX_NUM_TRACK];
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
        public static bool[] mDrawIsUtau = new bool[MAX_NUM_TRACK];
        /// <summary>
        /// マウスが降りていて，かつ範囲選択をしているときに立つフラグ
        /// </summary>
        public static bool mIsPointerDowned = false;
        /// <summary>
        /// マウスが降りた仮想スクリーン上の座標(ピクセル)
        /// </summary>
        public static Point mMouseDownLocation = new Point();
        public static int mLastTrackSelectorHeight;
        /// <summary>
        /// UTAUの原音設定のリスト。TreeMapのキーは、oto.iniのあるディレクトリ名になっている。
        /// </summary>
        public static SortedDictionary<string, UtauVoiceDB> mUtauVoiceDB = new SortedDictionary<string, UtauVoiceDB>();
        /// <summary>
        /// 最後にレンダリングが行われた時の、トラックの情報が格納されている。
        /// </summary>
        public static RenderedStatus[] mLastRenderedStatus = new RenderedStatus[MAX_NUM_TRACK];
        /// <summary>
        /// RenderingStatusをXMLシリアライズするためのシリアライザ
        /// </summary>
        public static XmlSerializer mRenderingStatusSerializer = new XmlSerializer(typeof(RenderedStatus));
        /// <summary>
        /// wavを出力するための一時ディレクトリのパス。
        /// </summary>
        private static string mTempWaveDir = "";
        /// <summary>
        /// 再生開始からの経過時刻がこの秒数以下の場合、再生を止めることが禁止される。
        /// </summary>
        public static double mForbidFlipPlayingThresholdSeconds = 0.2;
        /// <summary>
        /// ピアノロール画面に，コントロールカーブをオーバーレイしているモード
        /// </summary>
        public static bool mCurveOnPianoroll = false;
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
        private static StreamWriter mDebugLog = null;
#endif

        #region 裏設定項目
        /// <summary>
        /// 再生中にWAVE波形の描画をスキップするかどうか（デフォルトはtrue）
        /// </summary>
        public static bool skipDrawingWaveformWhenPlaying = true;
        /// <summary>
        /// コントロールカーブに、音符の境界線を重ね描きするかどうか（デフォルトはtrue）
        /// </summary>
        public static bool drawItemBorderInControlCurveView = true;
        /// <summary>
        /// コントロールカーブに、データ点を表す四角を描くかどうか（デフォルトはtrue）
        /// </summary>
        public static bool drawCurveDotInControlCurveView = true;
        /// <summary>
        /// ピアノロール画面に、現在選択中の歌声合成エンジンの種類を描くかどうか
        /// </summary>
        public static bool drawOverSynthNameOnPianoroll = true;
        /// <summary>
        /// 音符の長さが変更されたとき、ビブラートの長さがどう影響を受けるかを決める因子
        /// </summary>
        public static VibratoLengthEditingRule vibratoLengthEditingRule = VibratoLengthEditingRule.PERCENTAGE;
        /// <summary>
        /// ピアノロール上で右クリックでコンテキストメニューを表示するかどうか
        /// </summary>
        public static bool showContextMenuWhenRightClickedOnPianoroll = true;
        #endregion // 裏設定項目

        /// <summary>
        /// メイン画面で、グリッド表示のOn/Offが切り替わった時発生するイベント
        /// </summary>
        public static event EventHandler GridVisibleChanged;

        /// <summary>
        /// プレビュー再生が開始された時発生するイベント
        /// </summary>
        public static event EventHandler PreviewStarted;

        /// <summary>
        /// プレビュー再生が終了した時発生するイベント
        /// </summary>
        public static event EventHandler PreviewAborted;

        /// <summary>
        /// 編集ツールが変化した時発生するイベント
        /// </summary>
        public static event EventHandler SelectedToolChanged;

        /// <summary>
        /// BGMに何らかの変更があった時発生するイベント
        /// </summary>
        public static event EventHandler UpdateBgmStatusRequired;

        /// <summary>
        /// メインウィンドウにフォーカスを当てる要求があった時発生するイベント
        /// </summary>
        public static event EventHandler MainWindowFocusRequired;

        /// <summary>
        /// 編集されたかどうかを表す値に変更が要求されたときに発生するイベント
        /// </summary>
        public static event EditedStateChangedEventHandler EditedStateChanged;

        /// <summary>
        /// 波形ビューのリロードが要求されたとき発生するイベント．
        /// GeneralEventArgsの引数は，トラック番号,waveファイル名,開始時刻(秒),終了時刻(秒)が格納されたObject[]配列
        /// 開始時刻＞終了時刻の場合は，partialではなく全体のリロード要求
        /// </summary>
        public static event WaveViewRealoadRequiredEventHandler WaveViewReloadRequired;

        private const string TEMPDIR_NAME = "cadencii";

        static AppManager()
        {
            for (int i = 0; i < MAX_NUM_TRACK; i++) {
                mDrawObjects[i] = new List<DrawObject>();
            }
        }

        /// <summary>
        /// プレビュー再生を開始します．
        /// 合成処理などが途中でキャンセルされた場合にtrue，それ以外の場合にfalseを返します
        /// </summary>
        private static bool previewStart(FormMain form)
        {
            int selected = mSelected;
            RendererKind renderer = VsqFileEx.getTrackRendererKind(mVsq.Track[selected]);
            int clock = mCurrentClock;
            mDirectPlayShift = (float)mVsq.getSecFromClock(clock);
            // リアルタイム再生で無い場合
            string tmppath = getTempWaveDir();

            int track_count = mVsq.Track.Count;

            List<int> tracks = new List<int>();
            for (int track = 1; track < track_count; track++) {
                tracks.Add(track);
            }

            if (patchWorkToFreeze(form, tracks)) {
                // キャンセルされた
#if DEBUG
                sout.println("AppManager#previewStart; patchWorkToFreeze returns true");
#endif
                return true;
            }

            WaveSenderDriver driver = new WaveSenderDriver();
            List<Amplifier> waves = new List<Amplifier>();
            for (int i = 0; i < tracks.Count; i++) {
                int track = tracks[i];
                string file = Path.Combine(tmppath, track + ".wav");
                WaveReader wr = null;
                try {
                    wr = new WaveReader(file);
                    wr.setOffsetSeconds(mDirectPlayShift);
                    Amplifier a = new Amplifier();
                    FileWaveSender f = new FileWaveSender(wr);
                    a.setSender(f);
                    a.setAmplifierView(mMixerWindow.getVolumeTracker(track));
                    waves.Add(a);
                    a.setRoot(driver);
                    f.setRoot(driver);
                } catch (Exception ex) {
                    Logger.write(typeof(AppManager) + ".previewStart; ex=" + ex + "\n");
                    serr.println("AppManager.previewStart; ex=" + ex);
                }
            }

            // clock以降に音符があるかどうかを調べる
            int count = 0;
            foreach (var ve in mVsq.Track[selected].getNoteEventIterator()) {
                if (ve.Clock >= clock) {
                    count++;
                    break;
                }
            }

            int bgm_count = getBgmCount();
            double pre_measure_sec = mVsq.getSecFromClock(mVsq.getPreMeasureClocks());
            for (int i = 0; i < bgm_count; i++) {
                BgmFile bgm = getBgm(i);
                WaveReader wr = null;
                try {
                    wr = new WaveReader(bgm.file);
                    double offset = bgm.readOffsetSeconds + mDirectPlayShift;
                    if (bgm.startAfterPremeasure) {
                        offset -= pre_measure_sec;
                    }
                    wr.setOffsetSeconds(offset);
#if DEBUG
                    sout.println("AppManager.previewStart; bgm.file=" + bgm.file + "; offset=" + offset);

#endif
                    Amplifier a = new Amplifier();
                    FileWaveSender f = new FileWaveSender(wr);
                    a.setSender(f);
                    a.setAmplifierView(AppManager.mMixerWindow.getVolumeTrackerBgm(i));
                    waves.Add(a);
                    a.setRoot(driver);
                    f.setRoot(driver);
                } catch (Exception ex) {
                    Logger.write(typeof(AppManager) + ".previewStart; ex=" + ex + "\n");
                    serr.println("AppManager.previewStart; ex=" + ex);
                }
            }

            // 最初のsenderをドライバにする
            driver.setSender(waves[0]);
            Mixer m = new Mixer();
            m.setRoot(driver);
            driver.setReceiver(m);
            stopGenerator();
            setGenerator(driver);
            Amplifier amp = new Amplifier();
            amp.setRoot(driver);
            amp.setAmplifierView(mMixerWindow.getVolumeTrackerMaster());
            m.setReceiver(amp);
            MonitorWaveReceiver monitor = MonitorWaveReceiver.prepareInstance();
            monitor.setRoot(driver);
            amp.setReceiver(monitor);
            for (int i = 1; i < waves.Count; i++) {
                m.addSender(waves[i]);
            }

            int end_clock = mVsq.TotalClocks;
            if (mVsq.config.EndMarkerEnabled) {
                end_clock = mVsq.config.EndMarker;
            }
            mPreviewEndingClock = end_clock;
            double end_sec = mVsq.getSecFromClock(end_clock);
            int sample_rate = mVsq.config.SamplingRate;
            long samples = (long)((end_sec - mDirectPlayShift) * sample_rate);
            driver.init(mVsq, mSelected, 0, end_clock, sample_rate);
#if DEBUG
            sout.println("AppManager.previewStart; calling runGenerator...");
#endif
            runGenerator(samples);
#if DEBUG
            sout.println("AppManager.previewStart; calling runGenerator... done");
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
        public static bool patchWorkToFreeze(FormMain main_window, List<int> tracks)
        {
            mVsq.updateTotalClocks();
            List<PatchWorkQueue> queue = patchWorkCreateQueue(tracks);
#if DEBUG
            sout.println("AppManager#patchWorkToFreeze; queue.size()=" + queue.Count);
#endif

            FormWorker fw = new FormWorker();
            fw.setupUi(new FormWorkerUi(fw));
            fw.getUi().setTitle(_("Synthesize"));
            fw.getUi().setText(_("now synthesizing..."));

            double total = 0;
            SynthesizeWorker worker = new SynthesizeWorker(main_window);
            foreach (PatchWorkQueue q in queue) {
                // ジョブを追加
                double job_amount = q.getJobAmount();
                fw.addJob(worker, "processQueue", q.getMessage(), job_amount, q);
                total += job_amount;
            }

            // パッチワークをするジョブを追加
            fw.addJob(worker, "patchWork", _("patchwork"), total, new Object[] { queue, tracks });

            // ジョブを開始
            fw.startJob();

            // ダイアログを表示する
            beginShowDialog();
            bool ret = fw.getUi().showDialogTo(main_window);
#if DEBUG
            sout.println("AppManager#patchWorkToFreeze; showDialog returns " + ret);
#endif
            endShowDialog();
            return ret;
        }

        public static void invokeWaveViewReloadRequiredEvent(int track, string wavePath, double secStart, double secEnd)
        {
            try {
                WaveViewRealoadRequiredEventArgs arg = new WaveViewRealoadRequiredEventArgs();
                arg.track = track;
                arg.file = wavePath;
                arg.secStart = secStart;
                arg.secEnd = secEnd;
                if (WaveViewReloadRequired != null) {
                    WaveViewReloadRequired.Invoke(typeof(AppManager), arg);
                }
            } catch (Exception ex) {
                Logger.write(typeof(AppManager) + ".invokeWaveViewReloadRequiredEvent; ex=" + ex + "\n");
                sout.println(typeof(AppManager) + ".invokeWaveViewReloadRequiredEvent; ex=" + ex);
            }
        }

        /// <summary>
        /// 指定したトラックについて，再合成が必要な範囲を抽出し，それらのリストを作成します
        /// </summary>
        /// <param name="tracks">リストを作成するトラック番号の一覧</param>
        /// <returns></returns>
        public static List<PatchWorkQueue> patchWorkCreateQueue(List<int> tracks)
        {
            mVsq.updateTotalClocks();
            string temppath = getTempWaveDir();
            int presend = editorConfig.PreSendTime;
            int totalClocks = mVsq.TotalClocks;

            List<PatchWorkQueue> queue = new List<PatchWorkQueue>();
            int[] startIndex = new int[tracks.Count + 1]; // startList, endList, trackList, filesの内，第startIndex[j]からが，第tracks[j]トラックについてのレンダリング要求かを表す.

            for (int k = 0; k < tracks.Count; k++) {
                startIndex[k] = queue.Count;
                int track = tracks[k];
                VsqTrack vsq_track = mVsq.Track[track];
                string wavePath = Path.Combine(temppath, track + ".wav");

                if (mLastRenderedStatus[track - 1] == null) {
                    // この場合は全部レンダリングする必要がある
                    PatchWorkQueue q = new PatchWorkQueue();
                    q.track = track;
                    q.clockStart = 0;
                    q.clockEnd = totalClocks + 240;
                    q.file = wavePath;
                    q.vsq = mVsq;
                    queue.Add(q);
                    continue;
                }

                // 部分レンダリング
                EditedZoneUnit[] areas =
                    Utility.detectRenderedStatusDifference(mLastRenderedStatus[track - 1],
                                                            new RenderedStatus(
                                                                (VsqTrack)vsq_track.clone(),
                                                                mVsq.TempoTable,
                                                                (SequenceConfig)mVsq.config.clone()));

                // areasとかぶっている音符がどれかを判定する
                EditedZone zone = new EditedZone();
                zone.add(areas);
                checkSerializedEvents(zone, vsq_track, mVsq.TempoTable, areas);
                checkSerializedEvents(zone, mLastRenderedStatus[track - 1].track, mLastRenderedStatus[track - 1].tempo, areas);

                // レンダリング済みのwaveがあれば、zoneに格納された編集範囲に隣接する前後が無音でない場合、
                // 編集範囲を無音部分まで延長する。
                if (System.IO.File.Exists(wavePath)) {
                    WaveReader wr = null;
                    try {
                        wr = new WaveReader(wavePath);
                        int sampleRate = wr.getSampleRate();
                        int buflen = 1024;
                        double[] left = new double[buflen];
                        double[] right = new double[buflen];

                        // まずzoneから編集範囲を抽出
                        List<EditedZoneUnit> areasList = new List<EditedZoneUnit>();
                        foreach (var e in zone.iterator()) {
                            areasList.Add((EditedZoneUnit)e.clone());
                        }

                        foreach (var e in areasList) {
                            int exStart = e.mStart;
                            int exEnd = e.mEnd;

                            // 前方に1クロックずつ検索する。
                            int end = e.mStart;
                            int start = end - 1;
                            double secEnd = mVsq.getSecFromClock(end);
                            long saEnd = (long)(secEnd * sampleRate);
                            double secStart = 0.0;
                            long saStart = 0;
                            while (true) {
                                start = end - 1;
                                if (start < 0) {
                                    start = 0;
                                    break;
                                }
                                secStart = mVsq.getSecFromClock(start);
                                saStart = (long)(secStart * sampleRate);
                                int samples = (int)(saEnd - saStart);
                                long pos = saStart;
                                bool allzero = true;
                                while (samples > 0) {
                                    int delta = samples > buflen ? buflen : samples;
                                    wr.read(pos, delta, left, right);
                                    for (int i = 0; i < delta; i++) {
                                        if (left[i] != 0.0 || right[i] != 0.0) {
                                            allzero = false;
                                            break;
                                        }
                                    }
                                    pos += delta;
                                    samples -= delta;
                                    if (!allzero) {
                                        break;
                                    }
                                }
                                if (allzero) {
                                    break;
                                }
                                secEnd = secStart;
                                end = start;
                                saEnd = saStart;
                            }
                            // endクロックより先は無音であるようだ。
                            exStart = end;

                            // 後方に1クロックずつ検索する
                            if (e.mEnd < int.MaxValue) {
                                start = e.mEnd;
                                secStart = mVsq.getSecFromClock(start);
                                while (true) {
                                    end = start + 1;
                                    secEnd = mVsq.getSecFromClock(end);
                                    saEnd = (long)(secEnd * sampleRate);
                                    int samples = (int)(saEnd - saStart);
                                    long pos = saStart;
                                    bool allzero = true;
                                    while (samples > 0) {
                                        int delta = samples > buflen ? buflen : samples;
                                        wr.read(pos, delta, left, right);
                                        for (int i = 0; i < delta; i++) {
                                            if (left[i] != 0.0 || right[i] != 0.0) {
                                                allzero = false;
                                                break;
                                            }
                                        }
                                        pos += delta;
                                        samples -= delta;
                                        if (!allzero) {
                                            break;
                                        }
                                    }
                                    if (allzero) {
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
                            if (e.mStart != exStart) {
                                sout.println("FormMain#patchWorkToFreeze; start extended; " + e.mStart + " => " + exStart);
                            }
                            if (e.mEnd != exEnd) {
                                sout.println("FormMain#patchWorkToFreeze; end extended; " + e.mEnd + " => " + exEnd);
                            }
#endif

                            zone.add(exStart, exEnd);
                        }
                    } catch (Exception ex) {
                        Logger.write(typeof(FormMain) + ".patchWorkToFreeze; ex=" + ex + "\n");
                        serr.println("FormMain#patchWorkToFreeze; ex=" + ex);
                    } finally {
                        if (wr != null) {
                            try {
                                wr.close();
                            } catch (Exception ex2) {
                                Logger.write(typeof(FormMain) + ".patchWorkToFreeze; ex=" + ex2 + "\n");
                                serr.println("FormMain#patchWorkToFreeze; ex2=" + ex2);
                            }
                        }
                    }
                }

                // zoneに、レンダリングが必要なアイテムの範囲が格納されているので。
                int j = -1;
#if DEBUG
                sout.println("AppManager#patchWorkCreateQueue; track#" + track);
#endif
                foreach (var unit in zone.iterator()) {
                    j++;
                    PatchWorkQueue q = new PatchWorkQueue();
                    q.track = track;
                    q.clockStart = unit.mStart;
                    q.clockEnd = unit.mEnd;
#if DEBUG
                    sout.println("    start=" + unit.mStart + "; end=" + unit.mEnd);
#endif
                    q.file = Path.Combine(temppath, track + "_" + j + ".wav");
                    q.vsq = mVsq;
                    queue.Add(q);
                }
            }
            startIndex[tracks.Count] = queue.Count;

            if (queue.Count <= 0) {
                // パッチワークする必要なし
                for (int i = 0; i < tracks.Count; i++) {
                    setRenderRequired(tracks[i], false);
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
        private static void checkSerializedEvents(EditedZone zone, VsqTrack vsq_track, TempoVector tempo_vector, EditedZoneUnit[] areas)
        {
            if (vsq_track == null || zone == null || areas == null) {
                return;
            }
            if (areas.Length == 0) {
                return;
            }

            // まず，先行発音も考慮した音符の範囲を列挙する
            List<int> clockStartList = new List<int>();
            List<int> clockEndList = new List<int>();
            List<int> internalIdList = new List<int>();
            int size = vsq_track.getEventCount();
            RendererKind kind = VsqFileEx.getTrackRendererKind(vsq_track);
            for (int i = 0; i < size; i++) {
                VsqEvent item = vsq_track.getEvent(i);
                int clock_start = item.Clock;
                int clock_end = item.Clock + item.ID.getLength();
                int internal_id = item.InternalID;
                if (item.ID.type == VsqIDType.Anote) {
                    if (kind == RendererKind.UTAU) {
                        // 秒単位の先行発音
                        double sec_pre_utterance = item.UstEvent.getPreUtterance() / 1000.0;
                        // 先行発音を考慮した，音符の開始秒
                        double sec_at_clock_start_act = tempo_vector.getSecFromClock(clock_start) - sec_pre_utterance;
                        // 上記をクロック数に変換した物
                        int clock_start_draft = (int)tempo_vector.getClockFromSec(sec_at_clock_start_act);
                        // くり上がりがあるかもしれないので検査
                        while (sec_at_clock_start_act < tempo_vector.getSecFromClock(clock_start_draft) && 0 < clock_start_draft) {
                            clock_start_draft--;
                        }
                        clock_start = clock_start_draft;
                    }
                } else {
                    internal_id = -1;
                }

                // リストに追加
                clockStartList.Add(clock_start);
                clockEndList.Add(clock_end);
                internalIdList.Add(internal_id);
            }

            SortedDictionary<int, int> ids = new SortedDictionary<int, int>();
            //for ( Iterator<Integer> itr = vsq_track.indexIterator( IndexIteratorKind.NOTE ); itr.hasNext(); ) {
            for (int indx = 0; indx < size; indx++) {
                int internal_id = internalIdList[indx];
                if (internal_id == -1) {
                    continue;
                }
                int clockStart = clockStartList[indx];// item.Clock;
                int clockEnd = clockEndList[indx];// clockStart + item.ID.getLength();
                for (int i = 0; i < areas.Length; i++) {
                    EditedZoneUnit area = areas[i];
                    if (clockStart < area.mEnd && area.mEnd <= clockEnd) {
                        if (!ids.ContainsKey(internal_id)) {
                            ids[internal_id] = indx;
                            zone.add(clockStart, clockEnd);
                        }
                    } else if (clockStart <= area.mStart && area.mStart < clockEnd) {
                        if (!ids.ContainsKey(internal_id)) {
                            ids[internal_id] = indx;
                            zone.add(clockStart, clockEnd);
                        }
                    } else if (area.mStart <= clockStart && clockEnd < area.mEnd) {
                        if (!ids.ContainsKey(internal_id)) {
                            ids[internal_id] = indx;
                            zone.add(clockStart, clockEnd);
                        }
                    } else if (clockStart <= area.mStart && area.mEnd < clockEnd) {
                        if (!ids.ContainsKey(internal_id)) {
                            ids[internal_id] = indx;
                            zone.add(clockStart, clockEnd);
                        }
                    }
                }
            }

            // idsに登録された音符のうち、前後がつながっているものを列挙する。
            bool changed = true;
            int numEvents = vsq_track.getEventCount();
            while (changed) {
                changed = false;
                foreach (var id in ids.Keys) {
                    int indx = ids[id]; // InternalIDがidのアイテムの禁書目録
                    //VsqEvent item = vsq_track.getEvent( indx );

                    // アイテムを遡り、連続していれば追加する
                    int clock = clockStartList[indx];// item.Clock;
                    for (int i = indx - 1; i >= 0; i--) {
                        //VsqEvent search = vsq_track.getEvent( i );
                        int internal_id = internalIdList[i];
                        if (internal_id == -1) {
                            continue;
                        }
                        int searchClock = clockStartList[i];// search.Clock;
                        //int searchLength = search.ID.getLength();
                        int searchClockEnd = clockEndList[i];//
                        // 一個前の音符の終了位置が，この音符の開始位置と同じが後ろにある場合 -> 重なり有りと判定
                        if (clock <= searchClockEnd) {
                            if (!ids.ContainsKey(internal_id)) {
                                ids[internal_id] = i;
                                zone.add(searchClock, searchClockEnd);
                                changed = true;
                            }
                            clock = searchClock;
                        } else {
                            break;
                        }
                    }

                    // アイテムを辿り、連続していれば追加する
                    clock = clockEndList[indx];// item.Clock + item.ID.getLength();
                    for (int i = indx + 1; i < numEvents; i++) {
                        //VsqEvent search = vsq_track.getEvent( i );
                        int internal_id = internalIdList[i];
                        if (internal_id == -1) {
                            continue;
                        }
                        int searchClock = clockStartList[i];// search.Clock;
                        int searchClockEnd = clockEndList[i];// search.ID.getLength();
                        // 一行後ろの音符の開始位置が，この音符の終了位置と同じが後ろにある場合 -> 重なり有りと判定
                        if (searchClock <= clock) {
                            if (!ids.ContainsKey(internal_id)) {
                                ids[internal_id] = i;
                                zone.add(searchClock, searchClockEnd);
                                changed = true;
                            }
                            clock = searchClockEnd;
                        } else {
                            break;
                        }
                    }

                    if (changed) {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 波形生成器が実行中かどうかを取得します
        /// </summary>
        /// <returns></returns>
        public static bool isGeneratorRunning()
        {
            bool ret = false;
            lock (mLocker) {
                WaveGenerator g = mWaveGenerator;
                if (g != null) {
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
            lock (mLocker) {
                WaveGenerator g = mWaveGenerator;
                if (g != null) {
                    mWaveGeneratorState.requestCancel();
                    while (mWaveGenerator.isRunning()) {
                        Thread.Sleep(100);
                    }
                }
                mWaveGenerator = null;
            }
        }

        /// <summary>
        /// 波形生成器を設定します
        /// </summary>
        /// <param name="generator"></param>
        public static void setGenerator(WaveGenerator generator)
        {
            lock (mLocker) {
                WaveGenerator g = mWaveGenerator;
                if (g != null) {
                    mWaveGeneratorState.requestCancel();
                    while (g.isRunning()) {
                        Thread.Sleep(100);
                    }
                }
                mWaveGenerator = generator;
            }
        }

        /// <summary>
        /// 波形生成器を別スレッドで実行します
        /// </summary>
        /// <param name="samples">合成するサンプル数．波形合成器のbeginメソッドに渡される</param>
        public static void runGenerator(long samples)
        {
            lock (mLocker) {
#if DEBUG
                sout.println("AppManager#runGenerator; (mPreviewThread==null)=" + (mPreviewThread == null));
#endif
                Thread t = mPreviewThread;
                if (t != null) {
#if DEBUG
                    sout.println("AppManager#runGenerator; mPreviewThread.ThreadState=" + t.ThreadState);
#endif
                    if (t.ThreadState != ThreadState.Stopped) {
                        WaveGenerator g = mWaveGenerator;
                        if (g != null) {
                            mWaveGeneratorState.requestCancel();
                            while (mWaveGenerator.isRunning()) {
                                Thread.Sleep(100);
                            }
                        }
#if DEBUG
                        sout.println("AppManager#runGenerator; waiting stop...");
#endif
                        while (t.ThreadState != ThreadState.Stopped) {
                            Thread.Sleep(100);
                        }
#if DEBUG
                        sout.println("AppManager#runGenerator; waiting stop... done");
#endif
                    }
                }

                mWaveGeneratorState.reset();
                RunGeneratorQueue q = new RunGeneratorQueue();
                q.generator = mWaveGenerator;
                q.samples = samples;
                mPreviewThread = new Thread(
                    new ParameterizedThreadStart(runGeneratorCore));
                mPreviewThread.Start(q);
            }
        }

        private static void runGeneratorCore(Object argument)
        {
            RunGeneratorQueue q = (RunGeneratorQueue)argument;
            WaveGenerator g = q.generator;
            long samples = q.samples;
            try {
                g.begin(samples, mWaveGeneratorState);
            } catch (Exception ex) {
                Logger.write(typeof(AppManager) + ".runGeneratorCore; ex=" + ex + "\n");
                sout.println("AppManager#runGeneratorCore; ex=" + ex);
            }
        }

        /// <summary>
        /// 音の高さを表すnoteから、画面に描くべきy座標を計算します
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public static int yCoordFromNote(float note)
        {
            return yCoordFromNote(note, mMainWindowController.getStartToDrawY());
        }

        /// <summary>
        /// 音の高さを表すnoteから、画面に描くべきy座標を計算します
        /// </summary>
        /// <param name="note"></param>
        /// <param name="start_to_draw_y"></param>
        /// <returns></returns>
        public static int yCoordFromNote(float note, int start_to_draw_y)
        {
            return (int)(-1 * (note - 127.0f) * (int)(mMainWindowController.getScaleY() * 100)) - start_to_draw_y;
        }

        /// <summary>
        /// ピアノロール画面のy座標から、その位置における音の高さを取得します
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int noteFromYCoord(int y)
        {
            return 127 - (int)noteFromYCoordCore(y);
        }

        /// <summary>
        /// ピアノロール画面のy座標から、その位置における音の高さを取得します
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double noteFromYCoordDoublePrecision(int y)
        {
            return 127.0 - noteFromYCoordCore(y);
        }

        private static double noteFromYCoordCore(int y)
        {
            return (double)(mMainWindowController.getStartToDrawY() + y) / (double)((int)(mMainWindowController.getScaleY() * 100));
        }

        /// <summary>
        /// 指定した音声合成システムが使用する歌手のリストを取得します
        /// </summary>
        /// <param name="kind">音声合成システムの種類</param>
        /// <returns>歌手のリスト</returns>
        public static List<VsqID> getSingerListFromRendererKind(RendererKind kind)
        {
            List<VsqID> singers = null;
            if (kind == RendererKind.AQUES_TONE) {
                singers = new List<VsqID>();
#if ENABLE_AQUESTONE
                singers.AddRange(AquesToneDriver.Singers.Select((config) => getSingerIDAquesTone(config.Program)));
#endif
            } else if (kind == RendererKind.AQUES_TONE2) {
                singers = new List<VsqID>();
#if ENABLE_AQUESTONE
                singers.AddRange(AquesTone2Driver.Singers.Select((config) => getSingerIDAquesTone2(config.Program)));
#endif
            } else if (kind == RendererKind.VCNT || kind == RendererKind.UTAU) {
                List<SingerConfig> list = editorConfig.UtauSingers;
                singers = new List<VsqID>();
                foreach (var sc in list) {
                    singers.Add(getSingerIDUtau(sc.Language, sc.Program));
                }
            } else if (kind == RendererKind.VOCALOID1) {
                SingerConfig[] configs = VocaloSysUtil.getSingerConfigs(SynthesizerType.VOCALOID1);
                singers = new List<VsqID>();
                for (int i = 0; i < configs.Length; i++) {
                    SingerConfig sc = configs[i];
                    singers.Add(VocaloSysUtil.getSingerID(sc.Language, sc.Program, SynthesizerType.VOCALOID1));
                }
            } else if (kind == RendererKind.VOCALOID2) {
                singers = new List<VsqID>();
                SingerConfig[] configs = VocaloSysUtil.getSingerConfigs(SynthesizerType.VOCALOID2);
                for (int i = 0; i < configs.Length; i++) {
                    SingerConfig sc = configs[i];
                    singers.Add(VocaloSysUtil.getSingerID(sc.Language, sc.Program, SynthesizerType.VOCALOID2));
                }
            }
            return singers;
        }

        /// <summary>
        /// 指定したトラックの，指定した音符イベントについて，UTAUのパラメータを適用します
        /// </summary>
        /// <param name="vsq_track"></param>
        /// <param name="item"></param>
        public static void applyUtauParameter(VsqTrack vsq_track, VsqEvent item)
        {
            VsqEvent singer = vsq_track.getSingerEventAt(item.Clock);
            if (singer == null) {
                return;
            }
            SingerConfig sc = getSingerInfoUtau(singer.ID.IconHandle.Language, singer.ID.IconHandle.Program);
            if (sc != null && mUtauVoiceDB.ContainsKey(sc.VOICEIDSTR)) {
                UtauVoiceDB db = mUtauVoiceDB[sc.VOICEIDSTR];
                OtoArgs oa = db.attachFileNameFromLyric(item.ID.LyricHandle.L0.Phrase, item.ID.Note);
                if (item.UstEvent == null) {
                    item.UstEvent = new UstEvent();
                }
                item.UstEvent.setVoiceOverlap(oa.msOverlap);
                item.UstEvent.setPreUtterance(oa.msPreUtterance);
            }
        }

        /// <summary>
        /// 指定したディレクトリにある合成ステータスのxmlデータを読み込みます
        /// </summary>
        /// <param name="directory">読み込むxmlが保存されたディレクトリ</param>
        /// <param name="track">読み込みを行うトラックの番号</param>
        public static void deserializeRenderingStatus(string directory, int track)
        {
            string xml = Path.Combine(directory, track + ".xml");
            RenderedStatus status = null;
            if (System.IO.File.Exists(xml)) {
                FileStream fs = null;
                try {
                    fs = new FileStream(xml, FileMode.Open, FileAccess.Read);
                    Object obj = mRenderingStatusSerializer.deserialize(fs);
                    if (obj != null && obj is RenderedStatus) {
                        status = (RenderedStatus)obj;
                    }
                } catch (Exception ex) {
                    Logger.write(typeof(AppManager) + ".deserializeRederingStatus; ex=" + ex + "\n");
                    status = null;
                    serr.println("AppManager#deserializeRederingStatus; ex=" + ex);
                } finally {
                    if (fs != null) {
                        try {
                            fs.Close();
                        } catch (Exception ex2) {
                            Logger.write(typeof(AppManager) + ".deserializeRederingStatus; ex=" + ex2 + "\n");
                            serr.println("AppManager#deserializeRederingStatus; ex2=" + ex2);
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
        public static void serializeRenderingStatus(string temppath, int track)
        {
            FileStream fs = null;
            bool failed = true;
            string xml = Path.Combine(temppath, track + ".xml");
            try {
                fs = new FileStream(xml, FileMode.Create, FileAccess.Write);
                mRenderingStatusSerializer.serialize(fs, mLastRenderedStatus[track - 1]);
                failed = false;
            } catch (Exception ex) {
                serr.println("FormMain#patchWorkToFreeze; ex=" + ex);
                Logger.write(typeof(AppManager) + ".serializeRenderingStauts; ex=" + ex + "\n");
            } finally {
                if (fs != null) {
                    try {
                        fs.Close();
                    } catch (Exception ex2) {
                        serr.println("FormMain#patchWorkToFreeze; ex2=" + ex2);
                        Logger.write(typeof(AppManager) + ".serializeRenderingStatus; ex=" + ex2 + "\n");
                    }
                }
            }

            // シリアライズに失敗した場合，該当するxmlを削除する
            if (failed) {
                if (System.IO.File.Exists(xml)) {
                    try {
                        PortUtil.deleteFile(xml);
                    } catch (Exception ex) {
                        Logger.write(typeof(AppManager) + ".serializeRendererStatus; ex=" + ex + "\n");
                    }
                }
            }
        }

        /// <summary>
        /// クロック数から、画面に描くべきx座標の値を取得します。
        /// </summary>
        /// <param name="clocks"></param>
        /// <returns></returns>
        public static int xCoordFromClocks(double clocks)
        {
            return xCoordFromClocks(clocks, mMainWindowController.getScaleX(), mMainWindowController.getStartToDrawX());
        }

        /// <summary>
        /// クロック数から、画面に描くべきx座標の値を取得します。
        /// </summary>
        /// <param name="clocks"></param>
        /// <returns></returns>
        public static int xCoordFromClocks(double clocks, float scalex, int start_to_draw_x)
        {
            return (int)(keyWidth + clocks * scalex - start_to_draw_x) + keyOffset;
        }

        /// <summary>
        /// 画面のx座標からクロック数を取得します
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int clockFromXCoord(int x)
        {
            return (int)((x + mMainWindowController.getStartToDrawX() - keyOffset - keyWidth) * mMainWindowController.getScaleXInv());
        }

        #region 選択範囲の管理
        public static bool isWholeSelectedIntervalEnabled()
        {
            return mWholeSelectedIntervalEnabled;
        }

        public static bool isCurveSelectedIntervalEnabled()
        {
            return mCurveSelectedIntervalEnabled;
        }

        public static void setWholeSelectedIntervalEnabled(bool value)
        {
            mWholeSelectedIntervalEnabled = value;
            if (value) {
                mCurveSelectedIntervalEnabled = false;
            }
        }

        public static void setCurveSelectedIntervalEnabled(bool value)
        {
            mCurveSelectedIntervalEnabled = value;
            if (value) {
                mWholeSelectedIntervalEnabled = false;
            }
        }
        #endregion

        #region MessageBoxのラッパー
        public static DialogResult showMessageBox(string text)
        {
            return showMessageBox(text, "", cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION, cadencii.windows.forms.Utility.MSGBOX_PLAIN_MESSAGE);
        }

        public static DialogResult showMessageBox(string text, string caption)
        {
            return showMessageBox(text, caption, cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION, cadencii.windows.forms.Utility.MSGBOX_PLAIN_MESSAGE);
        }

        public static DialogResult showMessageBox(string text, string caption, int optionType)
        {
            return showMessageBox(text, caption, optionType, cadencii.windows.forms.Utility.MSGBOX_PLAIN_MESSAGE);
        }

        /// <summary>
        /// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="main_form"></param>
        /// <returns></returns>
        public static DialogResult showModalDialog(Form dialog, Form parent_form)
        {
            beginShowDialog();
            DialogResult ret = dialog.ShowDialog(parent_form);
            endShowDialog();
            return ret;
        }

        /// <summary>
        /// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="main_form"></param>
        /// <returns></returns>
        public static int showModalDialog(UiBase dialog, System.Windows.Forms.Form parent_form)
        {
            beginShowDialog();
            int ret = dialog.showDialog(parent_form);
            endShowDialog();
            return ret;
        }

        /// <summary>
        /// ダイアログを，メインウィンドウに対してモーダルに表示し，ダイアログの結果を取得します
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="main_form"></param>
        /// <returns></returns>
        public static DialogResult showModalDialog(FolderBrowserDialog dialog, Form main_form)
        {
            beginShowDialog();
            DialogResult ret = dialog.ShowDialog(main_form);
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
        public static DialogResult showModalDialog(FileDialog dialog, bool open_mode, Form main_form)
        {
            beginShowDialog();
            DialogResult ret = dialog.ShowDialog(main_form);
            endShowDialog();
            return ret;
        }

        /// <summary>
        /// beginShowDialogが呼ばれた後，endShowDialogがまだ呼ばれていないときにtrue
        /// </summary>
        /// <returns></returns>
        public static bool isShowingDialog()
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
            if (propertyWindow != null) {
                bool previous = propertyWindow.getUi().isAlwaysOnTop();
                propertyWindow.setPreviousAlwaysOnTop(previous);
                if (previous) {
                    propertyWindow.getUi().setAlwaysOnTop(false);
                }
            }
#endif
            if (mMixerWindow != null) {
                bool previous = mMixerWindow.TopMost;
                mMixerWindow.setPreviousAlwaysOnTop(previous);
                if (previous) {
                    mMixerWindow.TopMost = false;
                }
            }

            if (iconPalette != null) {
                bool previous = iconPalette.TopMost;
                iconPalette.setPreviousAlwaysOnTop(previous);
                if (previous) {
                    iconPalette.TopMost = false;
                }
            }
        }

        /// <summary>
        /// beginShowDialogで一時的にOFFにした「最前面に表示」設定を元に戻します
        /// </summary>
        private static void endShowDialog()
        {
#if ENABLE_PROPERTY
            if (propertyWindow != null) {
                propertyWindow.getUi().setAlwaysOnTop(propertyWindow.getPreviousAlwaysOnTop());
            }
#endif
            if (mMixerWindow != null) {
                mMixerWindow.TopMost = mMixerWindow.getPreviousAlwaysOnTop();
            }

            if (iconPalette != null) {
                iconPalette.TopMost = iconPalette.getPreviousAlwaysOnTop();
            }

            try {
                if (MainWindowFocusRequired != null) {
                    MainWindowFocusRequired.Invoke(typeof(AppManager), new EventArgs());
                }
            } catch (Exception ex) {
                Logger.write(typeof(AppManager) + ".endShowDialog; ex=" + ex + "\n");
                sout.println(typeof(AppManager) + ".endShowDialog; ex=" + ex);
            }
            mShowingDialog = false;
        }

        public static DialogResult showMessageBox(string text, string caption, int optionType, int messageType)
        {
            beginShowDialog();
            DialogResult ret = cadencii.windows.forms.Utility.showMessageBox(text, caption, optionType, messageType);
            endShowDialog();
            return ret;
        }
        #endregion

        #region BGM 関連
        public static int getBgmCount()
        {
            if (mVsq == null) {
                return 0;
            } else {
                return mVsq.BgmFiles.Count;
            }
        }

        public static BgmFile getBgm(int index)
        {
            if (mVsq == null) {
                return null;
            }
            return mVsq.BgmFiles[index];
        }

        public static void removeBgm(int index)
        {
            if (mVsq == null) {
                return;
            }
            List<BgmFile> list = new List<BgmFile>();
            int count = mVsq.BgmFiles.Count;
            for (int i = 0; i < count; i++) {
                if (i != index) {
                    list.Add((BgmFile)mVsq.BgmFiles[i].clone());
                }
            }
            CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate(list);
            editHistory.register(mVsq.executeCommand(run));
            try {
                if (EditedStateChanged != null) {
                    EditedStateChanged.Invoke(typeof(AppManager), true);
                }
            } catch (Exception ex) {
                Logger.write(typeof(AppManager) + ".removeBgm; ex=" + ex + "\n");
                serr.println(typeof(AppManager) + ".removeBgm; ex=" + ex);
            }
            mMixerWindow.updateStatus();
        }

        public static void clearBgm()
        {
            if (mVsq == null) {
                return;
            }
            List<BgmFile> list = new List<BgmFile>();
            CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate(list);
            editHistory.register(mVsq.executeCommand(run));
            try {
                if (EditedStateChanged != null) {
                    EditedStateChanged.Invoke(typeof(AppManager), true);
                }
            } catch (Exception ex) {
                Logger.write(typeof(AppManager) + ".removeBgm; ex=" + ex + "\n");
                serr.println(typeof(AppManager) + ".removeBgm; ex=" + ex);
            }
            mMixerWindow.updateStatus();
        }

        public static void addBgm(string file)
        {
            if (mVsq == null) {
                return;
            }
            List<BgmFile> list = new List<BgmFile>();
            int count = mVsq.BgmFiles.Count;
            for (int i = 0; i < count; i++) {
                list.Add((BgmFile)mVsq.BgmFiles[i].clone());
            }
            BgmFile item = new BgmFile();
            item.file = file;
            item.feder = 0;
            item.panpot = 0;
            list.Add(item);
            CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate(list);
            editHistory.register(mVsq.executeCommand(run));
            try {
                if (EditedStateChanged != null) {
                    EditedStateChanged.Invoke(typeof(AppManager), true);
                }
            } catch (Exception ex) {
                Logger.write(typeof(AppManager) + ".removeBgm; ex=" + ex + "\n");
                serr.println(typeof(AppManager) + ".removeBgm; ex=" + ex);
            }
            mMixerWindow.updateStatus();
        }
        #endregion

        #region 自動保存
        public static void updateAutoBackupTimerStatus()
        {
            if (!mFile.Equals("") && editorConfig.AutoBackupIntervalMinutes > 0) {
                double millisec = editorConfig.AutoBackupIntervalMinutes * 60.0 * 1000.0;
                int draft = (int)millisec;
                if (millisec > int.MaxValue) {
                    draft = int.MaxValue;
                }
                mAutoBackupTimer.Interval = draft;
                mAutoBackupTimer.Start();
            } else {
                mAutoBackupTimer.Stop();
            }
        }

        public static void handleAutoBackupTimerTick(Object sender, EventArgs e)
        {
#if DEBUG
            sout.println("AppManager::handleAutoBackupTimerTick");
#endif
            if (!mFile.Equals("") && System.IO.File.Exists(mFile)) {
                string path = PortUtil.getDirectoryName(mFile);
                string backup = Path.Combine(path, "~$" + PortUtil.getFileName(mFile));
                string file2 = Path.Combine(path, PortUtil.getFileNameWithoutExtension(backup) + ".vsq");
                if (System.IO.File.Exists(backup)) {
                    try {
                        PortUtil.deleteFile(backup);
                    } catch (Exception ex) {
                        serr.println("AppManager::handleAutoBackupTimerTick; ex=" + ex);
                        Logger.write(typeof(AppManager) + ".handleAutoBackupTimerTick; ex=" + ex + "\n");
                    }
                }
                if (System.IO.File.Exists(file2)) {
                    try {
                        PortUtil.deleteFile(file2);
                    } catch (Exception ex) {
                        serr.println("AppManager::handleAutoBackupTimerTick; ex=" + ex);
                        Logger.write(typeof(AppManager) + ".handleAutoBackupTimerTick; ex=" + ex + "\n");
                    }
                }
                saveToCor(backup);
            }
        }
        #endregion

        public static void debugWriteLine(string message)
        {
#if DEBUG
            try {
                if (mDebugLog == null) {
                    string log_file = Path.Combine(PortUtil.getApplicationStartupPath(), "log.txt");
                    mDebugLog = new StreamWriter(log_file);
                }
                mDebugLog.WriteLine(message);
            } catch (Exception ex) {
                serr.println("AppManager#debugWriteLine; ex=" + ex);
                Logger.write(typeof(AppManager) + ".debugWriteLine; ex=" + ex + "\n");
            }
            sout.println(message);
#endif
        }

        /// <summary>
        /// FormMainを識別するID
        /// </summary>
        public static string getID()
        {
            return mID;
        }

        /// <summary>
        /// gettext
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        /// <summary>
        /// 音声ファイルのキャッシュディレクトリのパスを設定します。
        /// このメソッドでは、キャッシュディレクトリの変更に伴う他の処理は実行されません。
        /// </summary>
        /// <param name="value"></param>
        public static void setTempWaveDir(string value)
        {
#if DEBUG
            sout.println("AppManager#setTempWaveDir; before: \"" + mTempWaveDir + "\"");
            sout.println("                           after:  \"" + value + "\"");
#endif
            mTempWaveDir = value;
        }

        /// <summary>
        /// 音声ファイルのキャッシュディレクトリのパスを取得します。
        /// </summary>
        /// <returns></returns>
        public static string getTempWaveDir()
        {
            return mTempWaveDir;
        }

        /// <summary>
        /// Cadenciiが使用する一時ディレクトリのパスを取得します。
        /// </summary>
        /// <returns></returns>
        public static string getCadenciiTempDir()
        {
            string temp = Path.Combine(PortUtil.getTempPath(), TEMPDIR_NAME);
            if (!Directory.Exists(temp)) {
                PortUtil.createDirectory(temp);
            }
            return temp;
        }

        /// <summary>
        /// ベジエ曲線を編集するモードかどうかを取得します。
        /// </summary>
        /// <returns></returns>
        public static bool isCurveMode()
        {
            return mIsCurveMode;
        }

        /// <summary>
        /// ベジエ曲線を編集するモードかどうかを設定します。
        /// </summary>
        /// <param name="value"></param>
        public static void setCurveMode(bool value)
        {
            bool old = mIsCurveMode;
            mIsCurveMode = value;
            if (old != mIsCurveMode) {
                try {
                    if (SelectedToolChanged != null) {
                        SelectedToolChanged.Invoke(typeof(AppManager), new EventArgs());
                    }
                } catch (Exception ex) {
                    serr.println("AppManager#setCurveMode; ex=" + ex);
                    Logger.write(typeof(AppManager) + ".setCurveMode; ex=" + ex + "\n");
                }
            }
        }

#if !TREECOM
        /// <summary>
        /// アンドゥ処理を行います。
        /// </summary>
        public static void undo()
        {
            if (editHistory.hasUndoHistory()) {
                List<ValuePair<int, int>> before_ids = new List<ValuePair<int, int>>();
                foreach (var item in itemSelection.getEventIterator()) {
                    before_ids.Add(new ValuePair<int, int>(item.track, item.original.InternalID));
                }

                ICommand run_src = editHistory.getUndo();
                CadenciiCommand run = (CadenciiCommand)run_src;
                if (run.vsqCommand != null) {
                    if (run.vsqCommand.Type == VsqCommandType.TRACK_DELETE) {
                        int track = (int)run.vsqCommand.Args[0];
                        if (track == getSelected() && track >= 2) {
                            setSelected(track - 1);
                        }
                    }
                }
                ICommand inv = mVsq.executeCommand(run);
                if (run.type == CadenciiCommandType.BGM_UPDATE) {
                    try {
                        if (UpdateBgmStatusRequired != null) {
                            UpdateBgmStatusRequired.Invoke(typeof(AppManager), new EventArgs());
                        }
                    } catch (Exception ex) {
                        Logger.write(typeof(AppManager) + ".undo; ex=" + ex + "\n");
                        serr.println(typeof(AppManager) + ".undo; ex=" + ex);
                    }
                }
                editHistory.registerAfterUndo(inv);

                cleanupDeadSelection(before_ids);
                itemSelection.updateSelectedEventInstance();
            }
        }

        /// <summary>
        /// リドゥ処理を行います。
        /// </summary>
        public static void redo()
        {
            if (editHistory.hasRedoHistory()) {
                List<ValuePair<int, int>> before_ids = new List<ValuePair<int, int>>();
                foreach (var item in itemSelection.getEventIterator()) {
                    before_ids.Add(new ValuePair<int, int>(item.track, item.original.InternalID));
                }

                ICommand run_src = editHistory.getRedo();
                CadenciiCommand run = (CadenciiCommand)run_src;
                if (run.vsqCommand != null) {
                    if (run.vsqCommand.Type == VsqCommandType.TRACK_DELETE) {
                        int track = (int)run.args[0];
                        if (track == getSelected() && track >= 2) {
                            setSelected(track - 1);
                        }
                    }
                }
                ICommand inv = mVsq.executeCommand(run);
                if (run.type == CadenciiCommandType.BGM_UPDATE) {
                    try {
                        if (UpdateBgmStatusRequired != null) {
                            UpdateBgmStatusRequired.Invoke(typeof(AppManager), new EventArgs());
                        }
                    } catch (Exception ex) {
                        Logger.write(typeof(AppManager) + ".redo; ex=" + ex + "\n");
                        serr.println(typeof(AppManager) + ".redo; ex=" + ex);
                    }
                }
                editHistory.registerAfterRedo(inv);

                cleanupDeadSelection(before_ids);
                itemSelection.updateSelectedEventInstance();
            }
        }

        /// <summary>
        /// 「選択されている」と登録されているオブジェクトのうち、Undo, Redoなどによって存在しなくなったものを登録解除する
        /// </summary>
        public static void cleanupDeadSelection(List<ValuePair<int, int>> before_ids)
        {
            int size = mVsq.Track.Count;
            foreach (var specif in before_ids) {
                bool found = false;
                int track = specif.getKey();
                int internal_id = specif.getValue();
                if (1 <= track && track < size) {
                    foreach (var item in mVsq.Track[track].getNoteEventIterator()) {
                        if (item.InternalID == internal_id) {
                            found = true;
                            break;
                        }
                    }
                }
                if (!found) {
                    AppManager.itemSelection.removeEvent(internal_id);
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
        public static void setSelectedTool(EditTool value)
        {
            EditTool old = mSelectedTool;
            mSelectedTool = value;
            if (old != mSelectedTool) {
                try {
                    if (SelectedToolChanged != null) {
                        SelectedToolChanged.Invoke(typeof(AppManager), new EventArgs());
                    }
                } catch (Exception ex) {
                    serr.println("AppManager#setSelectedTool; ex=" + ex);
                    Logger.write(typeof(AppManager) + ".setSelectedTool; ex=" + ex + "\n");
                }
            }
        }

        public static bool isOverlay()
        {
            return mOverlay;
        }

        public static void setOverlay(bool value)
        {
            mOverlay = value;
        }

        public static bool getRenderRequired(int track)
        {
            if (mVsq == null) {
                return false;
            }
            return mVsq.editorStatus.renderRequired[track - 1];
        }

        public static void setRenderRequired(int track, bool value)
        {
            if (mVsq == null) {
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
        public static void setEditMode(EditMode value)
        {
            mEditMode = value;
        }

        /// <summary>
        /// グリッドを表示するか否かを表す値を取得します
        /// </summary>
        public static bool isGridVisible()
        {
            return mGridVisible;
        }

        /// <summary>
        /// グリッドを表示するか否かを設定します
        /// </summary>
        /// <param name="value"></param>
        public static void setGridVisible(bool value)
        {
            if (value != mGridVisible) {
                mGridVisible = value;
                try {
                    if (GridVisibleChanged != null) {
                        GridVisibleChanged.Invoke(typeof(AppManager), new EventArgs());
                    }
                } catch (Exception ex) {
                    serr.println("AppManager#setGridVisible; ex=" + ex);
                    Logger.write(typeof(AppManager) + ".setGridVisible; ex=" + ex + "\n");
                }
            }
        }

        /// <summary>
        /// 現在のプレビューがリピートモードであるかどうかを表す値を取得します
        /// </summary>
        public static bool isRepeatMode()
        {
            return mRepeatMode;
        }

        /// <summary>
        /// 現在のプレビューがリピートモードかどうかを設定します
        /// </summary>
        /// <param name="value"></param>
        public static void setRepeatMode(bool value)
        {
            mRepeatMode = value;
        }

        /// <summary>
        /// 現在プレビュー中かどうかを示す値を取得します
        /// </summary>
        public static bool isPlaying()
        {
            return mPlaying;
        }

        /// <summary>
        /// プレビュー再生中かどうかを設定します．このプロパティーを切り替えることで，再生の開始と停止を行います．
        /// </summary>
        /// <param name="value"></param>
        /// <param name="form"></param>
        public static void setPlaying(bool value, FormMain form)
        {
#if DEBUG
            sout.println("AppManager#setPlaying; value=" + value);
#endif
            lock (mLockerPlayingProperty) {
                bool previous = mPlaying;
                mPlaying = value;
                if (previous != mPlaying) {
                    if (mPlaying) {
                        try {
                            if (previewStart(form)) {
#if DEBUG
                                sout.println("AppManager#setPlaying; previewStart returns true");
#endif
                                mPlaying = false;
                                return;
                            }
                            if (PreviewStarted != null) {
                                PreviewStarted.Invoke(typeof(AppManager), new EventArgs());
                            }
                        } catch (Exception ex) {
                            serr.println("AppManager#setPlaying; ex=" + ex);
                            Logger.write(typeof(AppManager) + ".setPlaying; ex=" + ex + "\n");
                        }
                    } else if (!mPlaying) {
                        try {
                            previewStop();
#if DEBUG
                            sout.println("AppManager#setPlaying; raise previewAbortedEvent");
#endif
                            if (PreviewAborted != null) {
                                PreviewAborted.Invoke(typeof(AppManager), new EventArgs());
                            }
                        } catch (Exception ex) {
                            serr.println("AppManager#setPlaying; ex=" + ex);
                            Logger.write(typeof(AppManager) + ".setPlaying; ex=" + ex + "\n");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// _vsq_fileにセットされたvsqファイルの名前を取得します。
        /// </summary>
        public static string getFileName()
        {
            return mFile;
        }

        private static void saveToCor(string file)
        {
            bool hide = false;
            if (mVsq != null) {
                string path = PortUtil.getDirectoryName(file);
                //String file2 = fsys.combine( path, PortUtil.getFileNameWithoutExtension( file ) + ".vsq" );
                mVsq.writeAsXml(file);
                //mVsq.write( file2 );
                if (hide) {
                    try {
                        System.IO.File.SetAttributes(file, System.IO.FileAttributes.Hidden);
                        //System.IO.File.SetAttributes( file2, System.IO.FileAttributes.Hidden );
                    } catch (Exception ex) {
                        serr.println("AppManager#saveToCor; ex=" + ex);
                        Logger.write(typeof(AppManager) + ".saveToCor; ex=" + ex + "\n");
                    }
                }
            }
        }

        public static void saveTo(string file)
        {
            if (mVsq != null) {
                if (editorConfig.UseProjectCache) {
                    // キャッシュディレクトリの処理
                    string dir = PortUtil.getDirectoryName(file);
                    string name = PortUtil.getFileNameWithoutExtension(file);
                    string cacheDir = Path.Combine(dir, name + ".cadencii");

                    if (!Directory.Exists(cacheDir)) {
                        try {
                            PortUtil.createDirectory(cacheDir);
                        } catch (Exception ex) {
                            serr.println("AppManager#saveTo; ex=" + ex);
                            showMessageBox(PortUtil.formatMessage(_("failed creating cache directory, '{0}'."), cacheDir),
                                            _("Info."),
                                            PortUtil.OK_OPTION,
                                            cadencii.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE);
                            Logger.write(typeof(AppManager) + ".saveTo; ex=" + ex + "\n");
                            return;
                        }
                    }

                    string currentCacheDir = getTempWaveDir();
                    if (!currentCacheDir.Equals(cacheDir)) {
                        for (int i = 1; i < mVsq.Track.Count; i++) {
                            string wavFrom = Path.Combine(currentCacheDir, i + ".wav");
                            string wavTo = Path.Combine(cacheDir, i + ".wav");
                            if (System.IO.File.Exists(wavFrom)) {
                                if (System.IO.File.Exists(wavTo)) {
                                    try {
                                        PortUtil.deleteFile(wavTo);
                                    } catch (Exception ex) {
                                        serr.println("AppManager#saveTo; ex=" + ex);
                                        Logger.write(typeof(AppManager) + ".saveTo; ex=" + ex + "\n");
                                    }
                                }
                                try {
                                    PortUtil.moveFile(wavFrom, wavTo);
                                } catch (Exception ex) {
                                    serr.println("AppManager#saveTo; ex=" + ex);
                                    showMessageBox(PortUtil.formatMessage(_("failed copying WAVE cache file, '{0}'."), wavFrom),
                                                    _("Error"),
                                                    PortUtil.OK_OPTION,
                                                    cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE);
                                    Logger.write(typeof(AppManager) + ".saveTo; ex=" + ex + "\n");
                                    break;
                                }
                            }

                            string xmlFrom = Path.Combine(currentCacheDir, i + ".xml");
                            string xmlTo = Path.Combine(cacheDir, i + ".xml");
                            if (System.IO.File.Exists(xmlFrom)) {
                                if (System.IO.File.Exists(xmlTo)) {
                                    try {
                                        PortUtil.deleteFile(xmlTo);
                                    } catch (Exception ex) {
                                        serr.println("AppManager#saveTo; ex=" + ex);
                                        Logger.write(typeof(AppManager) + ".saveTo; ex=" + ex + "\n");
                                    }
                                }
                                try {
                                    PortUtil.moveFile(xmlFrom, xmlTo);
                                } catch (Exception ex) {
                                    serr.println("AppManager#saveTo; ex=" + ex);
                                    showMessageBox(PortUtil.formatMessage(_("failed copying XML cache file, '{0}'."), xmlFrom),
                                                    _("Error"),
                                                    PortUtil.OK_OPTION,
                                                    cadencii.windows.forms.Utility.MSGBOX_WARNING_MESSAGE);
                                    Logger.write(typeof(AppManager) + ".saveTo; ex=" + ex + "\n");
                                    break;
                                }
                            }
                        }

                        setTempWaveDir(cacheDir);
                    }
                    mVsq.cacheDir = cacheDir;
                }
            }

            saveToCor(file);

            if (mVsq != null) {
                mFile = file;
                editorConfig.pushRecentFiles(mFile);
                if (!mAutoBackupTimer.Enabled && editorConfig.AutoBackupIntervalMinutes > 0) {
                    double millisec = editorConfig.AutoBackupIntervalMinutes * 60.0 * 1000.0;
                    int draft = (int)millisec;
                    if (millisec > int.MaxValue) {
                        draft = int.MaxValue;
                    }
                    mAutoBackupTimer.Interval = draft;
                    mAutoBackupTimer.Start();
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
        public static void setCurrentClock(int value)
        {
            mCurrentClock = value;
        }

        /// <summary>
        /// 現在選択されているトラックを取得または設定します
        /// </summary>
        public static int getSelected()
        {
            int tracks = mVsq.Track.Count;
            if (tracks <= mSelected) {
                mSelected = tracks - 1;
            }
            return mSelected;
        }

        public static void setSelected(int value)
        {
            mSelected = value;
        }

        [Obsolete]
        public static int Selected
        {
            get
            {
                return getSelected();
            }
        }

        /// <summary>
        /// xvsqファイルを読込みます．キャッシュディレクトリの更新は行われません
        /// </summary>
        /// <param name="file"></param>
        /// <returns>ファイルの読み込みに成功した場合にtrueを，それ以外の場合はfalseを返します</returns>
        public static bool readVsq(string file)
        {
            mSelected = 1;
            mFile = file;
            VsqFileEx newvsq = null;
            try {
                newvsq = VsqFileEx.readFromXml(file);
            } catch (Exception ex) {
                serr.println("AppManager#readVsq; ex=" + ex);
                Logger.write(typeof(AppManager) + ".readVsq; ex=" + ex + "\n");
                return true;
            }
            if (newvsq == null) {
                return true;
            }
            mVsq = newvsq;
            for (int i = 0; i < mVsq.editorStatus.renderRequired.Length; i++) {
                if (i < mVsq.Track.Count - 1) {
                    mVsq.editorStatus.renderRequired[i] = true;
                } else {
                    mVsq.editorStatus.renderRequired[i] = false;
                }
            }
            //mStartMarker = mVsq.getPreMeasureClocks();
            //int bar = mVsq.getPreMeasure() + 1;
            //mEndMarker = mVsq.getClockFromBarCount( bar );
            if (mVsq.Track.Count >= 1) {
                mSelected = 1;
            } else {
                mSelected = -1;
            }
            try {
                if (UpdateBgmStatusRequired != null) {
                    UpdateBgmStatusRequired.Invoke(typeof(AppManager), new EventArgs());
                }
            } catch (Exception ex) {
                Logger.write(typeof(AppManager) + ".readVsq; ex=" + ex + "\n");
                serr.println(typeof(AppManager) + ".readVsq; ex=" + ex);
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

        [Obsolete]
        public static VsqFileEx VsqFile
        {
            get
            {
                return getVsqFile();
            }
        }
#endif

        public static void setVsqFile(VsqFileEx vsq)
        {
            mVsq = vsq;
            for (int i = 0; i < mVsq.editorStatus.renderRequired.Length; i++) {
                if (i < mVsq.Track.Count - 1) {
                    mVsq.editorStatus.renderRequired[i] = true;
                } else {
                    mVsq.editorStatus.renderRequired[i] = false;
                }
            }
            mFile = "";
            //mStartMarker = mVsq.getPreMeasureClocks();
            //int bar = mVsq.getPreMeasure() + 1;
            //mEndMarker = mVsq.getClockFromBarCount( bar );
            mAutoBackupTimer.Stop();
            setCurrentClock(mVsq.getPreMeasureClocks());
            try {
                if (UpdateBgmStatusRequired != null) {
                    UpdateBgmStatusRequired.Invoke(typeof(AppManager), new EventArgs());
                }
            } catch (Exception ex) {
                Logger.write(typeof(AppManager) + ".setVsqFile; ex=" + ex + "\n");
                serr.println(typeof(AppManager) + ".setVsqFile; ex=" + ex);
            }
        }

        public static void init()
        {
            loadConfig();
            clipboard = new ClipboardModel();
            itemSelection = new ItemSelectionModel();
            editHistory = new EditHistoryModel();
            // UTAU歌手のアイコンを読み込み、起動画面に表示を要求する
            int c = editorConfig.UtauSingers.Count;
            for (int i = 0; i < c; i++) {
                SingerConfig sc = editorConfig.UtauSingers[i];
                if (sc == null) {
                    continue;
                }
                string dir = sc.VOICEIDSTR;
                SingerConfig sc_temp = new SingerConfig();
                string path_image = Utility.readUtauSingerConfig(dir, sc_temp);

#if DEBUG
                sout.println("AppManager#init; path_image=" + path_image);
#endif
                if (Cadencii.splash != null) {
                    try {
                        Cadencii.splash.addIconThreadSafe(path_image, sc.VOICENAME);
                    } catch (Exception ex) {
                        serr.println("AppManager#init; ex=" + ex);
                        Logger.write(typeof(AppManager) + ".init; ex=" + ex + "\n");
                    }
                }
            }

            VocaloSysUtil.init();

            editorConfig.check();
            keyWidth = editorConfig.KeyWidth;
            VSTiDllManager.init();
            // アイコンパレード, VOCALOID1
            SingerConfigSys singer_config_sys1 = VocaloSysUtil.getSingerConfigSys(SynthesizerType.VOCALOID1);
            if (singer_config_sys1 != null) {
                foreach (SingerConfig sc in singer_config_sys1.getInstalledSingers()) {
                    if (sc == null) {
                        continue;
                    }
                    string name = sc.VOICENAME.ToLower();
                    string path_image = Path.Combine(
                                            Path.Combine(
                                                PortUtil.getApplicationStartupPath(), "resources"),
                                            name + ".png");
#if DEBUG
                    sout.println("AppManager#init; path_image=" + path_image);
#endif
                    if (Cadencii.splash != null) {
                        try {
                            Cadencii.splash.addIconThreadSafe(path_image, sc.VOICENAME);
                        } catch (Exception ex) {
                            serr.println("AppManager#init; ex=" + ex);
                            Logger.write(typeof(AppManager) + ".init; ex=" + ex + "\n");
                        }
                    }
                }
            }

            // アイコンパレード、VOCALOID2
            SingerConfigSys singer_config_sys2 = VocaloSysUtil.getSingerConfigSys(SynthesizerType.VOCALOID2);
            if (singer_config_sys2 != null) {
                foreach (SingerConfig sc in singer_config_sys2.getInstalledSingers()) {
                    if (sc == null) {
                        continue;
                    }
                    string name = sc.VOICENAME.ToLower();
                    string path_image = Path.Combine(
                                            Path.Combine(
                                                PortUtil.getApplicationStartupPath(), "resources"),
                                            name + ".png");
#if DEBUG
                    sout.println("AppManager#init; path_image=" + path_image);
#endif
                    if (Cadencii.splash != null) {
                        try {
                            Cadencii.splash.addIconThreadSafe(path_image, sc.VOICENAME);
                        } catch (Exception ex) {
                            serr.println("AppManager#init; ex=" + ex);
                            Logger.write(typeof(AppManager) + ".init; ex=" + ex + "\n");
                        }
                    }
                }
            }

            PlaySound.init();
            mLocker = new Object();
            // VOCALOID2の辞書を読み込み
            SymbolTable.loadSystemDictionaries();
            // 日本語辞書
            SymbolTable.loadDictionary(
                Path.Combine(Path.Combine(PortUtil.getApplicationStartupPath(), "resources"), "dict_ja.txt"),
                "DEFAULT_JP");
            // 英語辞書
            SymbolTable.loadDictionary(
                Path.Combine(Path.Combine(PortUtil.getApplicationStartupPath(), "resources"), "dict_en.txt"),
                "DEFAULT_EN");
            // 拡張辞書
            SymbolTable.loadAllDictionaries(Path.Combine(PortUtil.getApplicationStartupPath(), "udic"));
            //VSTiProxy.CurrentUser = "";

            // 辞書の設定を適用
            try {
                // 現在辞書リストに読込済みの辞書を列挙
                List<ValuePair<string, Boolean>> current = new List<ValuePair<string, Boolean>>();
                for (int i = 0; i < SymbolTable.getCount(); i++) {
                    current.Add(new ValuePair<string, Boolean>(SymbolTable.getSymbolTable(i).getName(), false));
                }
                // editorConfig.UserDictionariesの設定値をコピー
                List<ValuePair<string, Boolean>> config_data = new List<ValuePair<string, Boolean>>();
                int count = editorConfig.UserDictionaries.Count;
                for (int i = 0; i < count; i++) {
                    string[] spl = PortUtil.splitString(editorConfig.UserDictionaries[i], new char[] { '\t' }, 2);
                    config_data.Add(new ValuePair<string, Boolean>(spl[0], (spl[1].Equals("T") ? true : false)));
#if DEBUG
                    AppManager.debugWriteLine("    " + spl[0] + "," + spl[1]);
#endif
                }
                // 辞書リストとeditorConfigの設定を比較する
                // currentの方には、editorConfigと共通するものについてのみsetValue(true)とする
                List<ValuePair<string, Boolean>> common = new List<ValuePair<string, Boolean>>();
                for (int i = 0; i < config_data.Count; i++) {
                    for (int j = 0; j < current.Count; j++) {
                        if (config_data[i].getKey().Equals(current[j].getKey())) {
                            // editorConfig.UserDictionariesにもKeyが含まれていたらtrue
                            current[j].setValue(true);
                            common.Add(new ValuePair<string, Boolean>(config_data[i].getKey(), config_data[i].getValue()));
                            break;
                        }
                    }
                }
                // editorConfig.UserDictionariesに登録されていないが、辞書リストには読み込まれている場合。
                // この場合は、デフォルトでENABLEとし、優先順位を最後尾とする。
                for (int i = 0; i < current.Count; i++) {
                    if (!current[i].getValue()) {
                        common.Add(new ValuePair<string, Boolean>(current[i].getKey(), true));
                    }
                }
                SymbolTable.changeOrder(common);
            } catch (Exception ex) {
                serr.println("AppManager#init; ex=" + ex);
            }

#if !TREECOM
            mID = PortUtil.getMD5FromString((long)PortUtil.getCurrentTime() + "").Replace("_", "");
            mTempWaveDir = Path.Combine(getCadenciiTempDir(), mID);
            if (!Directory.Exists(mTempWaveDir)) {
                PortUtil.createDirectory(mTempWaveDir);
            }
            string log = Path.Combine(getTempWaveDir(), "run.log");
#endif

            reloadUtauVoiceDB();

            mAutoBackupTimer = new System.Windows.Forms.Timer();
            mAutoBackupTimer.Tick += new EventHandler(handleAutoBackupTimerTick);
        }

        /// <summary>
        /// utauVoiceDBフィールドのリストを一度クリアし，
        /// editorConfig.Utausingersの情報を元に最新の情報に更新します
        /// </summary>
        public static void reloadUtauVoiceDB()
        {
            mUtauVoiceDB.Clear();
            foreach (var config in editorConfig.UtauSingers) {
                // 通常のUTAU音源
                UtauVoiceDB db = null;
                try {
                    db = new UtauVoiceDB(config);
                } catch (Exception ex) {
                    serr.println("AppManager#reloadUtauVoiceDB; ex=" + ex);
                    db = null;
                    Logger.write(typeof(AppManager) + ".reloadUtauVoiceDB; ex=" + ex + "\n");
                }
                if (db != null) {
                    mUtauVoiceDB[config.VOICEIDSTR] = db;
                }
            }
        }

        /// <summary>
        /// 位置クオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
        /// </summary>
        /// <returns></returns>
        public static int getPositionQuantizeClock()
        {
            return QuantizeModeUtil.getQuantizeClock(editorConfig.getPositionQuantize(), editorConfig.isPositionQuantizeTriplet());
        }

        /// <summary>
        /// 音符長さクオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
        /// </summary>
        /// <returns></returns>
        public static int getLengthQuantizeClock()
        {
            return QuantizeModeUtil.getQuantizeClock(editorConfig.getLengthQuantize(), editorConfig.isLengthQuantizeTriplet());
        }

        public static void serializeEditorConfig(EditorConfig instance, string file)
        {
            FileStream fs = null;
            try {
                fs = new FileStream(file, FileMode.Create, FileAccess.Write);
                EditorConfig.getSerializer().serialize(fs, instance);
            } catch (Exception ex) {
                Logger.write(typeof(EditorConfig) + ".serialize; ex=" + ex + "\n");
            } finally {
                if (fs != null) {
                    try {
                        fs.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(EditorConfig) + ".serialize; ex=" + ex2 + "\n");
                    }
                }
            }
        }

        public static EditorConfig deserializeEditorConfig(string file)
        {
            EditorConfig ret = null;
            FileStream fs = null;
            try {
                fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                ret = (EditorConfig)EditorConfig.getSerializer().deserialize(fs);
            } catch (Exception ex) {
                Logger.write(typeof(EditorConfig) + ".deserialize; ex=" + ex + "\n");
            } finally {
                if (fs != null) {
                    try {
                        fs.Close();
                    } catch (Exception ex2) {
                        Logger.write(typeof(EditorConfig) + ".deserialize; ex=" + ex2 + "\n");
                    }
                }
            }

            if (ret == null) {
                return null;
            }

            if (mMainWindow != null) {
                List<ValuePairOfStringArrayOfKeys> defs = mMainWindow.getDefaultShortcutKeys();
                for (int j = 0; j < defs.Count; j++) {
                    bool found = false;
                    for (int i = 0; i < ret.ShortcutKeys.Count; i++) {
                        if (defs[j].Key.Equals(ret.ShortcutKeys[i].Key)) {
                            found = true;
                            break;
                        }
                    }
                    if (!found) {
                        ret.ShortcutKeys.Add(defs[j]);
                    }
                }
            }

            // バッファーサイズを正規化
            if (ret.BufferSizeMilliSeconds < EditorConfig.MIN_BUFFER_MILLIXEC) {
                ret.BufferSizeMilliSeconds = EditorConfig.MIN_BUFFER_MILLIXEC;
            } else if (EditorConfig.MAX_BUFFER_MILLISEC < ret.BufferSizeMilliSeconds) {
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
            editorConfig.UserDictionaries.Clear();
            int count = SymbolTable.getCount();
            for (int i = 0; i < count; i++) {
                SymbolTable table = SymbolTable.getSymbolTable(i);
                editorConfig.UserDictionaries.Add(table.getName() + "\t" + (table.isEnabled() ? "T" : "F"));
            }
            editorConfig.KeyWidth = keyWidth;

            // chevronの幅を保存
            if (Rebar.CHEVRON_WIDTH > 0) {
                editorConfig.ChevronWidth = Rebar.CHEVRON_WIDTH;
            }

            // シリアライズして保存
            string file = Path.Combine(Utility.getConfigPath(), CONFIG_FILE_NAME);
#if DEBUG
            sout.println("AppManager#saveConfig; file=" + file);
#endif
            try {
                serializeEditorConfig(editorConfig, file);
            } catch (Exception ex) {
                serr.println("AppManager#saveConfig; ex=" + ex);
                Logger.write(typeof(AppManager) + ".saveConfig; ex=" + ex + "\n");
            }
        }

        /// <summary>
        /// 設定ファイルを読み込みます。
        /// 設定ファイルが壊れていたり存在しない場合、デフォルトの設定が使われます。
        /// </summary>
        public static void loadConfig()
        {
            string appdata = Utility.getApplicationDataPath();
#if DEBUG
            sout.println("AppManager#loadConfig; appdata=" + appdata);
#endif
            if (appdata.Equals("")) {
                editorConfig = new EditorConfig();
                return;
            }

            // バージョン番号付きのファイル
            string config_file = Path.Combine(Utility.getConfigPath(), CONFIG_FILE_NAME);
#if DEBUG
            sout.println("AppManager#loadConfig; config_file=" + config_file);
#endif
            EditorConfig ret = null;
            if (System.IO.File.Exists(config_file)) {
                // このバージョン用の設定ファイルがあればそれを利用
                try {
                    ret = deserializeEditorConfig(config_file);
                } catch (Exception ex) {
                    serr.println("AppManager#loadConfig; ex=" + ex);
                    ret = null;
                    Logger.write(typeof(AppManager) + ".loadConfig; ex=" + ex + "\n");
                }
            } else {
                // このバージョン用の設定ファイルがなかった場合
                // まず，古いバージョン用の設定ファイルがないかどうか順に調べる
                string[] dirs0 = PortUtil.listDirectories(appdata);
                // 数字と，2個以下のピリオドからなるディレクトリ名のみを抽出
                List<VersionString> dirs = new List<VersionString>();
                foreach (string s0 in dirs0) {
                    string s = PortUtil.getFileName(s0);
                    int length = PortUtil.getStringLength(s);
                    bool register = true;
                    int num_period = 0;
                    for (int i = 0; i < length; i++) {
                        char c = PortUtil.charAt(s, i);
                        if (c == '.') {
                            num_period++;
                        } else {
                            if (!char.IsNumber(c)) {
                                register = false;
                                break;
                            }
                        }
                    }
                    if (register && num_period <= 2) {
                        try {
                            VersionString vs = new VersionString(s);
                            dirs.Add(vs);
                        } catch (Exception ex) {
                        }
                    }
                }

                // 並べ替える
                bool changed = true;
                int size = dirs.Count;
                while (changed) {
                    changed = false;
                    for (int i = 0; i < size - 1; i++) {
                        VersionString item1 = dirs[i];
                        VersionString item2 = dirs[i + 1];
                        if (item1.compareTo(item2) > 0) {
                            dirs[i] = item2;
                            dirs[i + 1] = item1;
                            changed = true;
                        }
                    }
                }

                // バージョン番号付きの設定ファイルを新しい順に読み込みを試みる
                VersionString vs_this = new VersionString(BAssemblyInfo.fileVersionMeasure + "." + BAssemblyInfo.fileVersionMinor);
                for (int i = size - 1; i >= 0; i--) {
                    VersionString vs = dirs[i];
                    if (vs_this.compareTo(vs) < 0) {
                        // 自分自身のバージョンより新しいものは
                        // 読み込んではいけない
                        continue;
                    }
                    config_file = Path.Combine(Path.Combine(appdata, vs.getRawString()), CONFIG_FILE_NAME);
                    if (System.IO.File.Exists(config_file)) {
                        try {
                            ret = deserializeEditorConfig(config_file);
                        } catch (Exception ex) {
                            Logger.write(typeof(AppManager) + ".loadConfig; ex=" + ex + "\n");
                            ret = null;
                        }
                        if (ret != null) {
                            break;
                        }
                    }
                }

                // それでも読み込めなかった場合，旧来のデフォルトの位置にある
                // 設定ファイルを読みに行く
                if (ret == null) {
                    config_file = Path.Combine(appdata, CONFIG_FILE_NAME);
                    if (System.IO.File.Exists(config_file)) {
                        try {
                            ret = deserializeEditorConfig(config_file);
                        } catch (Exception ex) {
                            serr.println("AppManager#locdConfig; ex=" + ex);
                            ret = null;
                            Logger.write(typeof(AppManager) + ".loadConfig; ex=" + ex + "\n");
                        }
                    }
                }
            }

            // 設定ファイルの読み込みが悉く失敗した場合，
            // デフォルトの設定とする．
            if (ret == null) {
                ret = new EditorConfig();
            }
            editorConfig = ret;

            keyWidth = editorConfig.KeyWidth;
        }

        public static VsqID getSingerIDUtau(int language, int program)
        {
            VsqID ret = new VsqID(0);
            ret.type = VsqIDType.Singer;
            int index = language << 7 | program;
            if (0 <= index && index < editorConfig.UtauSingers.Count) {
                SingerConfig sc = editorConfig.UtauSingers[index];
                ret.IconHandle = new IconHandle();
                ret.IconHandle.IconID = "$0701" + PortUtil.toHexString(language, 2) + PortUtil.toHexString(program, 2);
                ret.IconHandle.IDS = sc.VOICENAME;
                ret.IconHandle.Index = 0;
                ret.IconHandle.Language = language;
                ret.IconHandle.setLength(1);
                ret.IconHandle.Original = language << 8 | program;
                ret.IconHandle.Program = program;
                ret.IconHandle.Caption = "";
                return ret;
            } else {
                ret.IconHandle = new IconHandle();
                ret.IconHandle.Program = 0;
                ret.IconHandle.Language = 0;
                ret.IconHandle.IconID = "$0701" + PortUtil.toHexString(0, 4);
                ret.IconHandle.IDS = "Unknown";
                ret.type = VsqIDType.Singer;
                return ret;
            }
        }

        public static SingerConfig getSingerInfoUtau(int language, int program)
        {
            int index = language << 7 | program;
            if (0 <= index && index < editorConfig.UtauSingers.Count) {
                return editorConfig.UtauSingers[index];
            } else {
                return null;
            }
        }

        /// <summary>
        /// TODO: 廃止する。AquesToneDriver から取得するようにする
        /// </summary>
        /// <param name="program_change"></param>
        /// <returns></returns>
        public static SingerConfig getSingerInfoAquesTone(int program_change)
        {
#if ENABLE_AQUESTONE
            return AquesToneDriver.getSingerConfig(program_change);
#else
            return null;
#endif
        }

        private static VsqID createAquesToneSingerID(int program, Func<int, SingerConfig> get_singer_config)
        {
            VsqID ret = new VsqID(0);
            ret.type = VsqIDType.Singer;
            SingerConfig config = null;
            if (get_singer_config != null) {
                config = get_singer_config(program);
            }
            if (config != null) {
                int lang = 0;
                ret.IconHandle = new IconHandle();
                ret.IconHandle.IconID = "$0701" + PortUtil.toHexString(lang, 2) + PortUtil.toHexString(program, 2);
                ret.IconHandle.IDS = config.VOICENAME;
                ret.IconHandle.Index = 0;
                ret.IconHandle.Language = lang;
                ret.IconHandle.setLength(1);
                ret.IconHandle.Original = lang << 8 | program;
                ret.IconHandle.Program = program;
                ret.IconHandle.Caption = "";
            } else {
                ret.IconHandle = new IconHandle();
                ret.IconHandle.Program = 0;
                ret.IconHandle.Language = 0;
                ret.IconHandle.IconID = "$0701" + PortUtil.toHexString(0, 2) + PortUtil.toHexString(0, 2);
                ret.IconHandle.IDS = "Unknown";
                ret.type = VsqIDType.Singer;
            }
            return ret;
        }

        public static VsqID getSingerIDAquesTone(int program)
        {
#if ENABLE_AQUESTONE
            return createAquesToneSingerID(program, AquesToneDriver.getSingerConfig);
#else
            return createAquesToneSingerID( program, null );
#endif
        }

        public static VsqID getSingerIDAquesTone2(int program)
        {
#if ENABLE_AQUESTONE
            return createAquesToneSingerID(program, AquesTone2Driver.getSingerConfig);
#else
            return createAquesToneSingerID( program, null );
#endif
        }

        public static Color getHilightColor()
        {
            return mHilightBrush;
        }

        public static void setHilightColor(Color value)
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

}
