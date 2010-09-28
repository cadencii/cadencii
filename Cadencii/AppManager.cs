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
#if JAVA
package org.kbinani.cadencii;

import java.awt.*;
import java.io.*;
import java.lang.reflect.*;
import java.util.*;
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
import org.kbinani.xml.*;
import org.kbinani.media.*;
#else
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.CSharp;
using org.kbinani.apputil;
using org.kbinani.java.awt;
using org.kbinani.java.io;
using org.kbinani.java.util;
using org.kbinani.media;
using org.kbinani.vsq;
using org.kbinani.windows.forms;
using org.kbinani.xml;

namespace org.kbinani.cadencii {
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;
    using Integer = System.Int32;
    using Long = System.Int64;
    using Float = System.Single;
#endif

    public class AppManager {
        public const int MIN_KEY_WIDTH = 68;
        public const int MAX_KEY_WIDTH = MIN_KEY_WIDTH * 5;
        private const String CONFIG_FILE_NAME = "config.xml";
#if CLIPBOARD_AS_TEXT
        /// <summary>
        /// OSのクリップボードに貼り付ける文字列の接頭辞．これがついていた場合，クリップボードの文字列をCadenciiが使用できると判断する．
        /// </summary>
        private const String CLIP_PREFIX = "CADENCIIOBJ";
#endif
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
        public static FormNoteProperty propertyWindow;
#endif
        /// <summary>
        /// アイコンパレット・ウィンドウのインスタンス
        /// </summary>
        public static FormIconPalette iconPalette = null;

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
        /// <summary>
        /// ショートカットキーとして受付可能なキーのリスト
        /// </summary>
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
        private static int s_base_tempo = 480000;
        private static Color s_hilight_brush = PortUtil.CornflowerBlue;
        private static Object s_locker;
        private static BTimer s_auto_backup_timer;
        #endregion

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
        /// <summary>
        /// 選択されているイベントのリスト
        /// </summary>
        private static Vector<SelectedEventEntry> mSelectedEvents = new Vector<SelectedEventEntry>();
        /// <summary>
        /// 選択されているテンポ変更イベントのリスト
        /// </summary>
        private static TreeMap<Integer, SelectedTempoEntry> mSelectedTempo = new TreeMap<Integer, SelectedTempoEntry>();
        private static int mLastSelectedTempo = -1;
        /// <summary>
        /// 選択されている拍子変更イベントのリスト
        /// </summary>
        private static TreeMap<Integer, SelectedTimesigEntry> mSelectedTimesig = new TreeMap<Integer, SelectedTimesigEntry>();
        private static int mLastSelectedTimesig = -1;
        private static EditTool mSelectedTool = EditTool.PENCIL;
#if !TREECOM
        private static Vector<ICommand> mCommands = new Vector<ICommand>();
        private static int mCommandIndex = -1;
#endif
        /// <summary>
        /// 選択されているベジエ点のリスト
        /// </summary>
        private static Vector<SelectedBezierPoint> mSelectedBezier = new Vector<SelectedBezierPoint>();
        /// <summary>
        /// 最後に選択されたベジエ点
        /// </summary>
        private static SelectedBezierPoint mLastSelectedBezier = new SelectedBezierPoint();
        /// <summary>
        /// 現在の演奏カーソルの位置(m_current_clockと意味は同じ。CurrentClockが変更されると、自動で更新される)
        /// </summary>
        private static PlayPositionSpecifier mCurrentPlayPosition = new PlayPositionSpecifier();

        /// <summary>
        /// selectedPointIDsに格納されているデータ点の，CurveType
        /// </summary>
        private static CurveType mSelectedPointCurveType = CurveType.Empty;
        private static Vector<Long> mSelectedPointIDs = new Vector<Long>();
        /// <summary>
        /// Playingプロパティにロックをかけるためのオブジェクト
        /// </summary>
        private static Object mPlayingPropertyLocker = new Object();

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
        /// エンドマーカーの位置(clock)
        /// </summary>
        public static int mEndMarker = 0;
        /// <summary>
        /// エンドマーカーが有効かどうか
        /// </summary>
        public static boolean mEndMarkerEnabled = false;
        /// <summary>
        /// x方向の表示倍率(pixel/clock)
        /// </summary>
        private static float mScaleX = 0.1f;
        /// <summary>
        /// _scaleXの逆数
        /// </summary>
        private static float mInvScaleX = 1.0f / mScaleX;
        /// <summary>
        /// スタートマーカーの位置(clock)
        /// </summary>
        public static int mStartMarker = 0;
        /// <summary>
        /// Bezierカーブ編集モードが有効かどうかを表す
        /// </summary>
        private static boolean mIsCurveMode = false;
        /// <summary>
        /// スタートマーカーが有効かどうか
        /// </summary>
        public static boolean mStartMarkerEnabled = false;
        /// <summary>
        /// 再生時に自動スクロールするかどうか
        /// </summary>
        public static boolean mAutoScroll = true;
        /// <summary>
        /// 最初のバッファが書き込まれたかどうか
        /// </summary>
        public static boolean mFirstBufferWritten = false;
        /// <summary>
        /// リアルタイム再生で使用しようとしたVSTiが有効だったかどうか。
        /// </summary>
        public static boolean mRendererAvailable = false;
        /// <summary>
        /// プレビュー再生が開始された時刻
        /// </summary>
        public static double mPreviewStartedTime;
        /// <summary>
        /// 画面左端位置での、仮想画面上の画面左端から測ったピクセル数．
        /// FormMain.hScroll.ValueとFormMain.trackBar.Valueで決まる．
        /// </summary>
        private static int mStartToDrawX;
        /// <summary>
        /// 画面上端位置での、仮想画面上の画面上端から図ったピクセル数．
        /// FormMain.vScroll.Value，FormMain.vScroll.Height，FormMain.vScroll.Maximum,AppManager.editorConfig.PxTrackHeightによって決まる
        /// </summary>
        private static int mStartToDrawY;
        /// <summary>
        /// 現在選択中のパレットアイテムの名前
        /// </summary>
        public static String mSelectedPaletteTool = "";
        /// <summary>
        /// このCadenciiのID。起動ごとにユニークな値が設定され、一時フォルダのフォルダ名等に使用する
        /// </summary>
        private static String mID = "";
        /// <summary>
        /// メインの編集画面のインスタンス
        /// </summary>
        public static FormMain mMainWindow = null;
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
        public static VsqEvent mAddingEvent;
        /// <summary>
        /// AppManager.m_draw_objectsを描く際の，最初に検索されるインデクス．
        /// </summary>
        public static int[] mDrawStartIndex = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
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
        /// TrackSelectorで表示させているカーブの一覧
        /// </summary>
        private static Vector<CurveType> mViewingCurves = new Vector<CurveType>();
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
        public static BEvent<BEventHandler> gridVisibleChangedEvent = new BEvent<BEventHandler>();
        /// <summary>
        /// プレビュー再生が開始された時発生するイベント
        /// </summary>
        public static BEvent<BEventHandler> previewStartedEvent = new BEvent<BEventHandler>();
        /// <summary>
        /// プレビュー再生が終了した時発生するイベント
        /// </summary>
        public static BEvent<BEventHandler> previewAbortedEvent = new BEvent<BEventHandler>();
        /// <summary>
        /// 選択状態のアイテムが変化した時発生するイベント
        /// </summary>
        public static BEvent<SelectedEventChangedEventHandler> selectedEventChangedEvent = new BEvent<SelectedEventChangedEventHandler>();
        /// <summary>
        /// 編集ツールが変化した時発生するイベント
        /// </summary>
        public static BEvent<BEventHandler> selectedToolChangedEvent = new BEvent<BEventHandler>();

        private const String TEMPDIR_NAME = "cadencii";

        public static int getViewingCurveCount() {
            return mViewingCurves.size();
        }

        public static CurveType getViewingCurveElement( int index ) {
            return mViewingCurves.get( index );
        }

        /// <summary>
        /// このコントロールに担当させるカーブを追加します
        /// </summary>
        /// <param name="curve"></param>
        public static void addViewingCurveRange( CurveType[] curve ) {
            for ( int j = 0; j < curve.Length; j++ ) {
                boolean found = false;
                for ( int i = 0; i < mViewingCurves.size(); i++ ) {
                    if ( mViewingCurves.get( i ).equals( curve[j] ) ) {
                        found = true;
                        break;
                    }
                }
                if ( !found ) {
                    mViewingCurves.add( curve[j] );
                }
            }
            if ( mViewingCurves.size() >= 2 ) {
                boolean changed = true;
                while ( changed ) {
                    changed = false;
                    for ( int i = 0; i < mViewingCurves.size() - 1; i++ ) {
                        if ( mViewingCurves.get( i ).getIndex() > mViewingCurves.get( i + 1 ).getIndex() ) {
                            CurveType b = (CurveType)mViewingCurves.get( i ).clone();
                            mViewingCurves.set( i, (CurveType)mViewingCurves.get( i + 1 ).clone() );
                            mViewingCurves.set( i + 1, b );
                            changed = true;
                        }
                    }
                }
            }
        }

        public static void addViewingCurve( CurveType curve ) {
            addViewingCurveRange( new CurveType[] { curve } );
        }

        public static void clearViewingCurve() {
            mViewingCurves.clear();
        }

        /// <summary>
        /// このコントロールに担当させるカーブを削除します
        /// </summary>
        /// <param name="curve"></param>
        public void removeViewingCurve( CurveType curve ) {
            for ( int i = 0; i < mViewingCurves.size(); i++ ) {
                if ( mViewingCurves.get( i ).equals( curve ) ) {
                    mViewingCurves.removeElementAt( i );
                    break;
                }
            }
        }

        /// <summary>
        /// ピアノロールの，X方向のスケールを取得します(pixel/clock)
        /// </summary>
        /// <returns></returns>
        public static float getScaleX() {
            return mScaleX;
        }

        /// <summary>
        /// ピアノロールの，X方向のスケールの逆数を取得します(clock/pixel)
        /// </summary>
        /// <returns></returns>
        public static float getScaleXInv() {
            return mInvScaleX;
        }

        /// <summary>
        /// ピアノロールの，X方向のスケールを設定します
        /// </summary>
        /// <param name="scale_x"></param>
        public static void setScaleX( float scale_x ) {
            mScaleX = scale_x;
            mInvScaleX = 1.0f / mScaleX;
        }

        public static int getStartToDrawX() {
            return mStartToDrawX;
        }

        public static void setStartToDrawX( int value ) {
            mStartToDrawX = value;
        }

        public static int getStartToDrawY() {
            return mStartToDrawY;
        }

        public static void setStartToDrawY( int value ) {
            mStartToDrawY = value;
        }

        /// <summary>
        /// 音の高さを表すnoteから、画面に描くべきy座標を計算します
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public static int yCoordFromNote( float note ) {
            return yCoordFromNote( note, mStartToDrawY );
        }

        public static int yCoordFromNote( float note, int start_to_draw_y ) {
            return (int)(-1 * (note - 127.0f) * editorConfig.PxTrackHeight) - start_to_draw_y;
        }

        /// <summary>
        /// ピアノロール画面のy座標から、その位置における音の高さを取得します
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int noteFromYCoord( int y ) {
            return 127 - (int)noteFromYCoordCore( y );
        }

        public static double noteFromYCoordDoublePrecision( int y ) {
            return 127.0 - noteFromYCoordCore( y );
        }

        private static double noteFromYCoordCore( int y ) {
            return (double)(mStartToDrawY + y) / (double)editorConfig.PxTrackHeight;
        }

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
                if ( sc != null && mUtauVoiceDB.containsKey( sc.VOICEIDSTR ) ) {
                    UtauVoiceDB db = mUtauVoiceDB.get( sc.VOICEIDSTR );
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
                mRenderingStatusSerializer.serialize( fs, mLastRenderedStatus[track - 1] );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "FormMain#patchWorkToFreeze; ex=" + ex );
                Logger.write( typeof( AppManager ) + ".serializeRenderingStauts; ex=" + ex + "\n" );
            } finally {
                if ( fs != null ) {
                    try {
                        fs.close();
                    } catch ( Exception ex2 ) {
                        PortUtil.stderr.println( "FormMain#patchWorkToFreeze; ex2=" + ex2 );
                        Logger.write( typeof( AppManager ) + ".serializeRenderingStatus; ex=" + ex2 + "\n" ); 
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
                    Logger.write( typeof( AppManager ) + ".reportError; ex=" + ex + "\n" );
                } finally {
                    if ( dialog != null ) {
                        try {
                            dialog.close();
                        } catch ( Exception ex3 ) {
                            Logger.write( typeof( AppManager ) + ".reportError; ex=" + ex3 + "\n" );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// クロック数から、画面に描くべきx座標の値を取得します。
        /// </summary>
        /// <param name="clocks"></param>
        /// <returns></returns>
        public static int xCoordFromClocks( double clocks ) {
            return (int)(keyWidth + clocks * mScaleX - mStartToDrawX) + keyOffset;
        }

        /// <summary>
        /// 画面のx座標からクロック数を取得します
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int clockFromXCoord( int x ) {
            return (int)((x + mStartToDrawX - keyOffset - keyWidth) * mInvScaleX);
        }

        #region 選択範囲の管理
        public static boolean isWholeSelectedIntervalEnabled() {
            return mWholeSelectedIntervalEnabled;
        }

        public static boolean isCurveSelectedIntervalEnabled() {
            return mCurveSelectedIntervalEnabled;
        }

        public static void setWholeSelectedIntervalEnabled( boolean value ) {
            mWholeSelectedIntervalEnabled = value;
            if ( value ) {
                mCurveSelectedIntervalEnabled = false;
            }
        }

        public static void setCurveSelectedIntervalEnabled( boolean value ) {
            mCurveSelectedIntervalEnabled = value;
            if ( value ) {
                mWholeSelectedIntervalEnabled = false;
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
            boolean mixer = (mMixerWindow != null) ? mMixerWindow.isAlwaysOnTop() : false;
            if ( mixer ) {
                mMixerWindow.setAlwaysOnTop( false );
            }
        }

        public static void endShowDialog() {
#if ENABLE_PROPERTY
            boolean property = (propertyWindow != null) ? propertyWindow.isAlwaysOnTop() : false;
            if ( property ) {
                propertyWindow.setAlwaysOnTop( true );
            }
#endif
            boolean mixer = (mMixerWindow != null) ? mMixerWindow.isAlwaysOnTop() : false;
            if ( mixer ) {
                mMixerWindow.setAlwaysOnTop( true );
            }
            if ( mMainWindow != null ) {
                mMainWindow.requestFocus();
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
            if ( mVsq == null ) {
                return 0;
            } else {
                return mVsq.BgmFiles.size();
            }
        }

        public static BgmFile getBgm( int index ) {
            if ( mVsq == null ) {
                return null;
            }
            return mVsq.BgmFiles.get( index );
        }

        public static void removeBgm( int index ) {
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
            register( mVsq.executeCommand( run ) );
            mMainWindow.setEdited( true );
            mMixerWindow.updateStatus();
        }

        public static void clearBgm() {
            if ( mVsq == null ) {
                return;
            }
            Vector<BgmFile> list = new Vector<BgmFile>();
            CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate( list );
            register( mVsq.executeCommand( run ) );
            mMainWindow.setEdited( true );
            mMixerWindow.updateStatus();
        }

        public static void addBgm( String file ) {
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
            register( mVsq.executeCommand( run ) );
            mMainWindow.setEdited( true );
            mMixerWindow.updateStatus();
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
            ret.mLeft = 1.0;
            ret.mRight = 1.0;
            if ( mVsq != null && 1 <= track && track < mVsq.Track.size() ) {
                double amp_master = VocaloSysUtil.getAmplifyCoeffFromFeder( mVsq.Mixer.MasterFeder );
                double pan_left_master = VocaloSysUtil.getAmplifyCoeffFromPanLeft( mVsq.Mixer.MasterPanpot );
                double pan_right_master = VocaloSysUtil.getAmplifyCoeffFromPanRight( mVsq.Mixer.MasterPanpot );
                double amp_track = VocaloSysUtil.getAmplifyCoeffFromFeder( mVsq.Mixer.Slave.get( track - 1 ).Feder );
                double pan_left_track = VocaloSysUtil.getAmplifyCoeffFromPanLeft( mVsq.Mixer.Slave.get( track - 1 ).Panpot );
                double pan_right_track = VocaloSysUtil.getAmplifyCoeffFromPanRight( mVsq.Mixer.Slave.get( track - 1 ).Panpot );
                ret.mLeft = amp_master * amp_track * pan_left_master * pan_left_track;
                ret.mRight = amp_master * amp_track * pan_right_master * pan_right_track;
            }
            return ret;
        }

        public static AmplifyCoefficient getAmplifyCoeffBgm( int index ) {
            AmplifyCoefficient ret = new AmplifyCoefficient();
            ret.mLeft = 1.0;
            ret.mRight = 1.0;
            if ( mVsq != null && 0 <= index && index < mVsq.BgmFiles.size() ) {
                BgmFile item = mVsq.BgmFiles.get( index );
                if ( item.mute == 1 ) {
                    ret.mLeft = 0.0;
                    ret.mRight = 0.0;
                } else {
                    double amp_master = VocaloSysUtil.getAmplifyCoeffFromFeder( mVsq.Mixer.MasterFeder );
                    double pan_left_master = VocaloSysUtil.getAmplifyCoeffFromPanLeft( mVsq.Mixer.MasterPanpot );
                    double pan_right_master = VocaloSysUtil.getAmplifyCoeffFromPanRight( mVsq.Mixer.MasterPanpot );
                    double amp_track = VocaloSysUtil.getAmplifyCoeffFromFeder( item.feder );
                    double pan_left_track = VocaloSysUtil.getAmplifyCoeffFromPanLeft( item.panpot );
                    double pan_right_track = VocaloSysUtil.getAmplifyCoeffFromPanRight( item.panpot );
                    ret.mLeft = amp_master * amp_track * pan_left_master * pan_left_track;
                    ret.mRight = amp_master * amp_track * pan_right_master * pan_right_track;
                }
            }
            return ret;
        }
        #endregion

        #region 自動保存
        public static void updateAutoBackupTimerStatus() {
            if ( !mFile.Equals( "" ) && editorConfig.AutoBackupIntervalMinutes > 0 ) {
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

        public static void handleAutoBackupTimerTick( Object sender, BEventArgs e ) {
#if DEBUG
            PortUtil.println( "AppManager::handleAutoBackupTimerTick" );
#endif
            if ( !mFile.Equals( "" ) && PortUtil.isFileExists( mFile ) ) {
                String path = PortUtil.getDirectoryName( mFile );
                String backup = PortUtil.combinePath( path, "~$" + PortUtil.getFileName( mFile ) );
                String file2 = PortUtil.combinePath( path, PortUtil.getFileNameWithoutExtension( backup ) + ".vsq" );
                if ( PortUtil.isFileExists( backup ) ) {
                    try {
                        PortUtil.deleteFile( backup );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "AppManager::handleAutoBackupTimerTick; ex=" + ex );
                        Logger.write( typeof( AppManager ) + ".handleAutoBackupTimerTick; ex=" + ex + "\n" );
                    }
                }
                if ( PortUtil.isFileExists( file2 ) ) {
                    try {
                        PortUtil.deleteFile( file2 );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "AppManager::handleAutoBackupTimerTick; ex=" + ex );
                        Logger.write( typeof( AppManager ) + ".handleAutoBackupTimerTick; ex=" + ex + "\n" );
                    }
                }
                saveToCor( backup );
            }
        }
        #endregion

        public static void debugWriteLine( String message ) {
#if DEBUG
            try {
                if ( mDebugLog == null ) {
                    String log_file = PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "log.txt" );
                    mDebugLog = new BufferedWriter( new FileWriter( log_file ) );
                }
                mDebugLog.write( message );
                mDebugLog.newLine();
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "AppManager#debugWriteLine; ex=" + ex );
                Logger.write( typeof( AppManager ) + ".debugWriteLine; ex=" + ex + "\n" );
            }
            PortUtil.println( message );
#endif
        }

        /// <summary>
        /// FormMainを識別するID
        /// </summary>
        public static String getID() {
            return mID;
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
            PortUtil.println( "AppManager#setTempWaveDir; before: \"" + mTempWaveDir + "\"" );
            PortUtil.println( "                           after:  \"" + value + "\"" );
#endif
            mTempWaveDir = value;
        }

        /// <summary>
        /// 音声ファイルのキャッシュディレクトリのパスを取得します。
        /// </summary>
        /// <returns></returns>
        public static String getTempWaveDir() {
            return mTempWaveDir;
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
            return mIsCurveMode;
        }

        /// <summary>
        /// ベジエ曲線を編集するモードかどうかを設定します。
        /// </summary>
        /// <param name="value"></param>
        public static void setCurveMode( boolean value ) {
            boolean old = mIsCurveMode;
            mIsCurveMode = value;
            if ( old != mIsCurveMode ) {
                try {
                    selectedToolChangedEvent.raise( typeof( AppManager ), new BEventArgs() );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "AppManager#setCurveMode; ex=" + ex );
                    Logger.write( typeof( AppManager ) + ".setCurveMode; ex=" + ex + "\n" );
                }
            }
        }

#if !TREECOM
        /// <summary>
        /// アンドゥ・リドゥ用のコマンド履歴を削除します。
        /// </summary>
        public static void clearCommandBuffer() {
            mCommands.clear();
            mCommandIndex = -1;
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

                ICommand run_src = mCommands.get( mCommandIndex );
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
                    if ( mMainWindow != null ) {
                        mMainWindow.updateBgmMenuState();
                    }
                }
                mCommands.set( mCommandIndex, inv );
                mCommandIndex--;

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

                ICommand run_src = mCommands.get( mCommandIndex + 1 );
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
                    if ( mMainWindow != null ) {
                        mMainWindow.updateBgmMenuState();
                    }
                }
                mCommands.set( mCommandIndex + 1, inv );
                mCommandIndex++;

                cleanupDeadSelection( before_ids );
                updateSelectedEventInstance();
            }
        }

        /// <summary>
        /// 選択中のアイテムが編集された場合、編集にあわせてオブジェクトを更新する。
        /// </summary>
        public static void updateSelectedEventInstance() {
            if ( mVsq == null ) {
                return;
            }
            VsqTrack vsq_track = mVsq.Track.get( mSelected );

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
                for ( Iterator<VsqEvent> itr2 = mVsq.Track.get( specif.getKey() ).getNoteEventIterator(); itr2.hasNext(); ) {
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
            if ( mCommandIndex == mCommands.size() - 1 ) {
                // 新しいコマンドバッファを追加する場合
                mCommands.add( command );
                mCommandIndex = mCommands.size() - 1;
            } else {
                // 既にあるコマンドバッファを上書きする場合
                mCommands.set( mCommandIndex + 1, command );
                for ( int i = mCommands.size() - 1; i >= mCommandIndex + 2; i-- ) {
                    mCommands.removeElementAt( i );
                }
                mCommandIndex++;
            }
        }

        /// <summary>
        /// リドゥ操作が可能かどうかを取得します。
        /// </summary>
        /// <returns>リドゥ操作が可能ならtrue、そうでなければfalseを返します。</returns>
        public static boolean isRedoAvailable() {
            if ( mCommands.size() > 0 && 0 <= mCommandIndex + 1 && mCommandIndex + 1 < mCommands.size() ) {
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
            if ( mCommands.size() > 0 && 0 <= mCommandIndex && mCommandIndex < mCommands.size() ) {
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
            return mSelectedTool;
        }

        /// <summary>
        /// 現在選択されているツールを設定します。
        /// </summary>
        /// <param name="value"></param>
        public static void setSelectedTool( EditTool value ) {
            EditTool old = mSelectedTool;
            mSelectedTool = value;
            if ( old != mSelectedTool ) {
                try {
                    selectedToolChangedEvent.raise( typeof( AppManager ), new EventArgs() );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "AppManager#setSelectedTool; ex=" + ex );
                    Logger.write( typeof( AppManager ) + ".setSelectedTool; ex=" + ex + "\n" );
                }
            }
        }

        #region SelectedBezier
        /// <summary>
        /// 選択されているベジエ曲線のデータ点を順に返す反復子を取得します。
        /// </summary>
        /// <returns></returns>
        public static Iterator<SelectedBezierPoint> getSelectedBezierIterator() {
            return mSelectedBezier.iterator();
        }

        /// <summary>
        /// 最後に選択状態となったベジエ曲線のデータ点を取得します。
        /// </summary>
        /// <returns>最後に選択状態となったベジエ曲線のデータ点を返します。選択状態となっているベジエ曲線がなければnullを返します。</returns>
        public static SelectedBezierPoint getLastSelectedBezier() {
            if ( mLastSelectedBezier.chainID < 0 || mLastSelectedBezier.pointID < 0 ) {
                return null;
            } else {
                return mLastSelectedBezier;
            }
        }

        /// <summary>
        /// 指定されたベジエ曲線のデータ点を選択状態にします。
        /// </summary>
        /// <param name="selected">選択状態にするデータ点。</param>
        public static void addSelectedBezier( SelectedBezierPoint selected ) {
            mLastSelectedBezier = selected;
            int index = -1;
            for ( int i = 0; i < mSelectedBezier.size(); i++ ) {
                if ( mSelectedBezier.get( i ).chainID == selected.chainID &&
                    mSelectedBezier.get( i ).pointID == selected.pointID ) {
                    index = i;
                    break;
                }
            }
            if ( index >= 0 ) {
                mSelectedBezier.set( index, selected );
            } else {
                mSelectedBezier.add( selected );
            }
            checkSelectedItemExistence();
        }

        /// <summary>
        /// すべてのベジエ曲線のデータ点の選択状態を解除します。
        /// </summary>
        public static void clearSelectedBezier() {
            mSelectedBezier.clear();
            mLastSelectedBezier.chainID = -1;
            mLastSelectedBezier.pointID = -1;
            checkSelectedItemExistence();
        }
        #endregion

        #region SelectedTimesig
        /// <summary>
        /// 最後に選択状態となった拍子変更設定を取得します。
        /// </summary>
        /// <returns>最後に選択状態となった拍子変更設定を返します。選択状態となっている拍子変更設定が無ければnullを返します。</returns>
        public static SelectedTimesigEntry getLastSelectedTimesig() {
            if ( mSelectedTimesig.containsKey( mLastSelectedTimesig ) ) {
                return mSelectedTimesig.get( mLastSelectedTimesig );
            } else {
                return null;
            }
        }

        public static int getLastSelectedTimesigBarcount() {
            return mLastSelectedTimesig;
        }

        public static void addSelectedTimesig( int barcount ) {
            clearSelectedEvent(); //ここ注意！
            clearSelectedTempo();
            mLastSelectedTimesig = barcount;
            if ( !mSelectedTimesig.containsKey( barcount ) ) {
                for ( Iterator<TimeSigTableEntry> itr = mVsq.TimesigTable.iterator(); itr.hasNext(); ) {
                    TimeSigTableEntry tte = itr.next();
                    if ( tte.BarCount == barcount ) {
                        mSelectedTimesig.put( barcount, new SelectedTimesigEntry( tte, (TimeSigTableEntry)tte.clone() ) );
                        break;
                    }
                }
            }
            checkSelectedItemExistence();
        }

        public static void clearSelectedTimesig() {
            mSelectedTimesig.clear();
            mLastSelectedTimesig = -1;
            checkSelectedItemExistence();
        }

        public static int getSelectedTimesigCount() {
            return mSelectedTimesig.size();
        }

        public static Iterator<ValuePair<Integer, SelectedTimesigEntry>> getSelectedTimesigIterator() {
            Vector<ValuePair<Integer, SelectedTimesigEntry>> list = new Vector<ValuePair<Integer, SelectedTimesigEntry>>();
            for ( Iterator<Integer> itr = mSelectedTimesig.keySet().iterator(); itr.hasNext(); ) {
                int clock = itr.next();
                list.add( new ValuePair<Integer, SelectedTimesigEntry>( clock, mSelectedTimesig.get( clock ) ) );
            }
            return list.iterator();
        }

        public static boolean isSelectedTimesigContains( int barcount ) {
            return mSelectedTimesig.containsKey( barcount );
        }

        public static SelectedTimesigEntry getSelectedTimesig( int barcount ) {
            if ( mSelectedTimesig.containsKey( barcount ) ) {
                return mSelectedTimesig.get( barcount );
            } else {
                return null;
            }
        }

        public static void removeSelectedTimesig( int barcount ) {
            if ( mSelectedTimesig.containsKey( barcount ) ) {
                mSelectedTimesig.remove( barcount );
                checkSelectedItemExistence();
            }
        }
        #endregion

        #region SelectedTempo
        public static SelectedTempoEntry getLastSelectedTempo() {
            if ( mSelectedTempo.containsKey( mLastSelectedTempo ) ) {
                return mSelectedTempo.get( mLastSelectedTempo );
            } else {
                return null;
            }
        }

        public static int getLastSelectedTempoClock() {
            return mLastSelectedTempo;
        }

        public static void addSelectedTempo( int clock ) {
            clearSelectedEvent(); //ここ注意！
            clearSelectedTimesig();
            mLastSelectedTempo = clock;
            if ( !mSelectedTempo.containsKey( clock ) ) {
                for ( Iterator<TempoTableEntry> itr = mVsq.TempoTable.iterator(); itr.hasNext(); ) {
                    TempoTableEntry tte = itr.next();
                    if ( tte.Clock == clock ) {
                        mSelectedTempo.put( clock, new SelectedTempoEntry( tte, (TempoTableEntry)tte.clone() ) );
                        break;
                    }
                }
            }
            checkSelectedItemExistence();
        }

        public static void clearSelectedTempo() {
            mSelectedTempo.clear();
            mLastSelectedTempo = -1;
            checkSelectedItemExistence();
        }

        public static int getSelectedTempoCount() {
            return mSelectedTempo.size();
        }

        public static Iterator<ValuePair<Integer, SelectedTempoEntry>> getSelectedTempoIterator() {
            Vector<ValuePair<Integer, SelectedTempoEntry>> list = new Vector<ValuePair<Integer, SelectedTempoEntry>>();
            for ( Iterator<Integer> itr = mSelectedTempo.keySet().iterator(); itr.hasNext(); ) {
                int clock = itr.next();
                list.add( new ValuePair<Integer, SelectedTempoEntry>( clock, mSelectedTempo.get( clock ) ) );
            }
            return list.iterator();
        }

        public static boolean isSelectedTempoContains( int clock ) {
            return mSelectedTempo.containsKey( clock );
        }

        public static SelectedTempoEntry getSelectedTempo( int clock ) {
            if ( mSelectedTempo.containsKey( clock ) ) {
                return mSelectedTempo.get( clock );
            } else {
                return null;
            }
        }

        public static void removeSelectedTempo( int clock ) {
            if ( mSelectedTempo.containsKey( clock ) ) {
                mSelectedTempo.remove( clock );
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
            int count = mSelectedEvents.size();
            for ( int i = 0; i < count; i++ ) {
                if ( mSelectedEvents.get( i ).original.InternalID == id ) {
                    mSelectedEvents.removeElementAt( i );
                    break;
                }
            }
            if ( !silent ) {
#if ENABLE_PROPERTY
                propertyPanel.updateValue( mSelected );
#endif
            }
        }

        public static void removeSelectedEventRange( int[] ids ) {
            Vector<Integer> v_ids = new Vector<Integer>( Arrays.asList( PortUtil.convertIntArray( ids ) ) );
            Vector<Integer> index = new Vector<Integer>();
            int count = mSelectedEvents.size();
            for ( int i = 0; i < count; i++ ) {
                if ( v_ids.contains( mSelectedEvents.get( i ).original.InternalID ) ) {
                    index.add( i );
                    if ( index.size() == ids.Length ) {
                        break;
                    }
                }
            }
            count = index.size();
            for ( int i = count - 1; i >= 0; i-- ) {
                mSelectedEvents.removeElementAt( i );
            }
#if ENABLE_PROPERTY
            propertyPanel.updateValue( mSelected );
#endif
            checkSelectedItemExistence();
        }

        public static void addSelectedEventAll( Vector<Integer> list ) {
            clearSelectedTempo();
            clearSelectedTimesig();
            VsqEvent[] index = new VsqEvent[list.size()];
            int count = 0;
            int c = list.size();
            for ( Iterator<VsqEvent> itr = mVsq.Track.get( mSelected ).getEventIterator(); itr.hasNext(); ) {
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
                if ( !isSelectedEventContains( mSelected, index[i].InternalID ) ) {
                    mSelectedEvents.add( new SelectedEventEntry( mSelected, index[i], (VsqEvent)index[i].clone() ) );
                }
            }
#if ENABLE_PROPERTY
            propertyPanel.updateValue( mSelected );
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
            for ( Iterator<VsqEvent> itr = mVsq.Track.get( mSelected ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent ev = itr.next();
                if ( ev.InternalID == id ) {
                    if ( isSelectedEventContains( mSelected, id ) ) {
                        // すでに選択されていた場合
                        int count = mSelectedEvents.size();
                        for ( int i = 0; i < count; i++ ) {
                            SelectedEventEntry item = mSelectedEvents.get( i );
                            if ( item.original.InternalID == id ) {
                                mSelectedEvents.removeElementAt( i );
                                break;
                            }
                        }
                    }

                    mSelectedEvents.add( new SelectedEventEntry( mSelected, ev, (VsqEvent)ev.clone() ) );
                    if ( !silent ) {
                        try {
                            selectedEventChangedEvent.raise( typeof( AppManager ), false );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "AppManager#addSelectedEventCor; ex=" + ex );
                            Logger.write( typeof( AppManager ) + ".addSelectedCurveCor; ex=" + ex + "\n" );
                        }
                    }
                    break;
                }
            }
            if ( !silent ) {
#if ENABLE_PROPERTY
                propertyPanel.updateValue( mSelected );
#endif
            }
        }

        public static void clearSelectedEvent() {
            mSelectedEvents.clear();
#if ENABLE_PROPERTY
            propertyPanel.updateValue( mSelected );
#endif
            checkSelectedItemExistence();
        }

        public static boolean isSelectedEventContains( int track, int id ) {
            int count = mSelectedEvents.size();
            for ( int i = 0; i < count; i++ ) {
                SelectedEventEntry item = mSelectedEvents.get( i );
                if ( item.original.InternalID == id && item.track == track ) {
                    return true;
                }
            }
            return false;
        }

        public static Iterator<SelectedEventEntry> getSelectedEventIterator() {
            return mSelectedEvents.iterator();
        }

        public static SelectedEventEntry getLastSelectedEvent() {
            if ( mSelectedEvents.size() <= 0 ) {
                return null;
            } else {
                return mSelectedEvents.get( mSelectedEvents.size() - 1 );
            }
        }

        public static int getSelectedEventCount() {
            return mSelectedEvents.size();
        }
        #endregion

        #region SelectedPoint
        public static void clearSelectedPoint() {
            mSelectedPointIDs.clear();
            mSelectedPointCurveType = CurveType.Empty;
            checkSelectedItemExistence();
        }

        public static void addSelectedPoint( CurveType curve, long id ) {
            addSelectedPointAll( curve, new long[] { id } );
            checkSelectedItemExistence();
        }

        public static void addSelectedPointAll( CurveType curve, long[] ids ) {
            if ( !curve.equals( mSelectedPointCurveType ) ) {
                mSelectedPointIDs.clear();
                mSelectedPointCurveType = curve;
            }
            for ( int i = 0; i < ids.Length; i++ ) {
                if ( !mSelectedPointIDs.contains( ids[i] ) ) {
                    mSelectedPointIDs.add( ids[i] );
                }
            }
            checkSelectedItemExistence();
        }

        public static boolean isSelectedPointContains( long id ) {
            return mSelectedPointIDs.contains( id );
        }

        public static CurveType getSelectedPointCurveType() {
            return mSelectedPointCurveType;
        }

        public static Iterator<Long> getSelectedPointIDIterator() {
            return mSelectedPointIDs.iterator();
        }

        public static int getSelectedPointIDCount() {
            return mSelectedPointIDs.size();
        }

        public static void removeSelectedPoint( long id ) {
            mSelectedPointIDs.removeElement( id );
            checkSelectedItemExistence();
        }
        #endregion

        /// <summary>
        /// 現在選択されたアイテムが存在するかどうかを調べ，必要であればSelectedEventChangedイベントを発生させます
        /// </summary>
        private static void checkSelectedItemExistence() {
            boolean ret = mSelectedBezier.size() == 0 &&
                          mSelectedEvents.size() == 0 &&
                          mSelectedTempo.size() == 0 &&
                          mSelectedTimesig.size() == 0 &&
                          mSelectedPointIDs.size() == 0;
            try {
                selectedEventChangedEvent.raise( typeof( AppManager ), ret );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "AppManager#checkSelectedItemExistence; ex=" + ex );
                Logger.write( typeof( AppManager ) + ".checkSelectedItemExistence; ex=" + ex + "\n" );
            }
        }

        public static boolean isOverlay() {
            return mOverlay;
        }

        public static void setOverlay( boolean value ) {
            mOverlay = value;
        }

        public static boolean getRenderRequired( int track ) {
            if ( mVsq == null ) {
                return false;
            }
            return mVsq.editorStatus.renderRequired[track - 1];
        }

        public static void setRenderRequired( int track, boolean value ) {
            if ( mVsq == null ) {
                return;
            }
            mVsq.editorStatus.renderRequired[track - 1] = value;
        }

        /// <summary>
        /// 現在の編集モード
        /// </summary>
        public static EditMode getEditMode() {
            return mEditMode;
        }

        public static void setEditMode( EditMode value ) {
            mEditMode = value;
        }

        /// <summary>
        /// グリッドを表示するか否かを表すフラグを取得または設定します
        /// </summary>
        public static boolean isGridVisible() {
            return mGridVisible;
        }

        public static void setGridVisible( boolean value ) {
            if ( value != mGridVisible ) {
                mGridVisible = value;
                try {
                    gridVisibleChangedEvent.raise( typeof( AppManager ), new BEventArgs() );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "AppManager#setGridVisible; ex=" + ex );
                    Logger.write( typeof( AppManager ) + ".setGridVisible; ex=" + ex + "\n" );
                }
            }
        }

        /// <summary>
        /// 現在のプレビューがリピートモードであるかどうかを表す値を取得または設定します
        /// </summary>
        public static boolean isRepeatMode() {
            return mRepeatMode;
        }

        public static void setRepeatMode( boolean value ) {
            mRepeatMode = value;
        }

        /// <summary>
        /// 現在プレビュー中かどうかを示す値を取得または設定します
        /// </summary>
        public static boolean isPlaying() {
            return mPlaying;
        }

        public static void setPlaying( boolean value ) {
            lock ( mPlayingPropertyLocker ) {
                boolean previous = mPlaying;
                mPlaying = value;
                if ( previous != mPlaying ) {
                    if ( mPlaying ) {
                        try {
                            previewStartedEvent.raise( typeof( AppManager ), new BEventArgs() );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "AppManager#setPlaying; ex=" + ex );
                            Logger.write( typeof( AppManager ) + ".setPlaying; ex=" + ex + "\n" );
                        }
                    } else if ( !mPlaying ) {
                        try {
                            previewAbortedEvent.raise( typeof( AppManager ), new BEventArgs() );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "AppManager#setPlaying; ex=" + ex );
                            Logger.write( typeof( AppManager ) + ".setPlaying; ex=" + ex + "\n" );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// _vsq_fileにセットされたvsqファイルの名前を取得します。
        /// </summary>
        public static String getFileName() {
            return mFile;
        }

        private static void saveToCor( String file ) {
            boolean hide = false;
            if ( mVsq != null ) {
                String path = PortUtil.getDirectoryName( file );
                String file2 = PortUtil.combinePath( path, PortUtil.getFileNameWithoutExtension( file ) + ".vsq" );
                mVsq.writeAsXml( file );
                mVsq.write( file2 );
#if !JAVA
                if ( hide ) {
                    try {
                        System.IO.File.SetAttributes( file, System.IO.FileAttributes.Hidden );
                        System.IO.File.SetAttributes( file2, System.IO.FileAttributes.Hidden );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "AppManager#saveToCor; ex=" + ex );
                        Logger.write( typeof( AppManager ) + ".saveToCor; ex=" + ex + "\n" );
                    }
                }
#endif
            }
        }

        public static void saveTo( String file ) {
            if ( mVsq != null ) {
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
                            Logger.write( typeof( AppManager ) + ".saveTo; ex=" + ex + "\n" );
                            return;
                        }
                    }

                    String currentCacheDir = getTempWaveDir();
                    if ( !currentCacheDir.Equals( cacheDir ) ) {
                        for ( int i = 1; i < mVsq.Track.size(); i++ ) {
                            String wavFrom = PortUtil.combinePath( currentCacheDir, i + ".wav" );
                            String wavTo = PortUtil.combinePath( cacheDir, i + ".wav" );
                            if ( PortUtil.isFileExists( wavFrom ) ) {
                                if ( PortUtil.isFileExists( wavTo ) ) {
                                    try {
                                        PortUtil.deleteFile( wavTo );
                                    } catch ( Exception ex ) {
                                        PortUtil.stderr.println( "AppManager#saveTo; ex=" + ex );
                                        Logger.write( typeof( AppManager ) + ".saveTo; ex=" + ex + "\n" );
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
                                    Logger.write( typeof( AppManager ) + ".saveTo; ex=" + ex + "\n" );
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
                                        Logger.write( typeof( AppManager ) + ".saveTo; ex=" + ex + "\n" );
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
        }

        /// <summary>
        /// 現在の演奏マーカーの位置を取得します。
        /// </summary>
        public static int getCurrentClock() {
            return mCurrentClock;
        }

        /// <summary>
        /// 現在の演奏マーカーの位置を設定します。
        /// </summary>
        public static void setCurrentClock( int value ) {
            int old = mCurrentClock;
            mCurrentClock = value;
            int barcount = mVsq.getBarCountFromClock( mCurrentClock );
            int bar_top_clock = mVsq.getClockFromBarCount( barcount );
            Timesig timesig = mVsq.getTimesigAt( mCurrentClock );
            int clock_per_beat = 480 / 4 * timesig.denominator;
            int beat = (mCurrentClock - bar_top_clock) / clock_per_beat;
            mCurrentPlayPosition.barCount = barcount - mVsq.getPreMeasure() + 1;
            mCurrentPlayPosition.beat = beat + 1;
            mCurrentPlayPosition.clock = mCurrentClock - bar_top_clock - clock_per_beat * beat;
            mCurrentPlayPosition.denominator = timesig.denominator;
            mCurrentPlayPosition.numerator = timesig.numerator;
            mCurrentPlayPosition.tempo = mVsq.getTempoAt( mCurrentClock );
        }

        /// <summary>
        /// 現在の演奏カーソルの位置(m_current_clockと意味は同じ。CurrentClockが変更されると、自動で更新される)
        /// </summary>
        public static PlayPositionSpecifier getPlayPosition() {
            return mCurrentPlayPosition;
        }

        /// <summary>
        /// 現在選択されているトラックを取得または設定します
        /// </summary>
        public static int getSelected() {
            int tracks = mVsq.Track.size();
            if ( tracks <= mSelected ) {
                mSelected = tracks - 1;
            }
            return mSelected;
        }

        public static void setSelected( int value ) {
            mSelected = value;
        }

#if !JAVA
        [Obsolete]
        public static int Selected {
            get {
                return getSelected();
            }
        }
#endif

        /// <summary>
        /// vsqファイルを読込みます
        /// </summary>
        /// <param name="file"></param>
        public static void readVsq( String file ) {
            mSelected = 1;
            mFile = file;
            VsqFileEx newvsq = null;
            try {
                newvsq = VsqFileEx.readFromXml( file );
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "AppManager#readVsq; ex=" + ex );
#if JAVA
                ex.printStackTrace();
#endif
                Logger.write( typeof( AppManager ) + ".readVsq; ex=" + ex + "\n" );
                return;
            }
            if ( newvsq == null ) {
                return;
            }
            mVsq = newvsq;
            for ( int i = 0; i < mVsq.editorStatus.renderRequired.Length; i++ ) {
                if ( i < mVsq.Track.size() - 1 ) {
                    mVsq.editorStatus.renderRequired[i] = true;
                } else {
                    mVsq.editorStatus.renderRequired[i] = false;
                }
            }
            mStartMarker = mVsq.getPreMeasureClocks();
            int bar = mVsq.getPreMeasure() + 1;
            mEndMarker = mVsq.getClockFromBarCount( bar );
            if ( mVsq.Track.size() >= 1 ) {
                mSelected = 1;
            } else {
                mSelected = -1;
            }
            if ( mMainWindow != null ) {
                mMainWindow.updateBgmMenuState();
            }
        }

#if !TREECOM
        /// <summary>
        /// vsqファイル。
        /// </summary>
        public static VsqFileEx getVsqFile() {
            return mVsq;
        }

#if !JAVA
        [Obsolete]
        public static VsqFileEx VsqFile {
            get {
                return getVsqFile();
            }
        }
#endif
#endif

        public static void setVsqFile( VsqFileEx vsq ) {
            mVsq = vsq;
            for ( int i = 0; i < mVsq.editorStatus.renderRequired.Length; i++ ) {
                if ( i < mVsq.Track.size() - 1 ) {
                    mVsq.editorStatus.renderRequired[i] = true;
                } else {
                    mVsq.editorStatus.renderRequired[i] = false;
                }
            }
            mFile = "";
            mStartMarker = mVsq.getPreMeasureClocks();
            int bar = mVsq.getPreMeasure() + 1;
            mEndMarker = mVsq.getClockFromBarCount( bar );
            s_auto_backup_timer.stop();
            if ( mMainWindow != null ) {
                mMainWindow.updateBgmMenuState();
            }
        }

        public static void init() {
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
                    Logger.write( typeof( AppManager ) + ".init; ex=" + ex + "\n" );
                } finally {
                    if ( br != null ) {
                        try {
                            br.close();
                        } catch ( Exception ex2 ) {
                            PortUtil.stderr.println( "AppManager#init; ex2=" + ex2 );
                            Logger.write( typeof( AppManager ) + ".init; ex=" + ex2 + "\n" );
                        }
                    }
                }

#if DEBUG
                PortUtil.println( "AppManager#init; path_image=" + path_image );
#endif
                if ( Cadencii.splash != null ){
                    try{
                        Cadencii.splash.addIconThreadSafe( path_image, sc.VOICENAME );
                    }catch( Exception ex ){
                        PortUtil.stderr.println( "AppManager#init; ex=" + ex );
                        Logger.write( typeof( AppManager ) + ".init; ex=" + ex + "\n" );
                    }
                }
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
                    if ( Cadencii.splash != null ) {
                        try {
                            Cadencii.splash.addIconThreadSafe( path_image, sc.VOICENAME );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "AppManager#init; ex=" + ex );
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
                    String path_image = PortUtil.combinePath(
                                            PortUtil.combinePath(
                                                PortUtil.getApplicationStartupPath(), "resources" ),
                                            name + ".png" );
#if DEBUG
                    PortUtil.println( "AppManager#init; path_image=" + path_image );
#endif
                    if ( Cadencii.splash != null ) {
                        try {
                            Cadencii.splash.addIconThreadSafe( path_image, sc.VOICENAME );
                        } catch ( Exception ex ) {
                            PortUtil.stderr.println( "AppManager#init; ex=" + ex );
                            Logger.write( typeof( AppManager ) + ".init; ex=" + ex + "\n" );
                        }
                    }
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
                Logger.write( typeof( AppManager ) + ".init; ex=" + ex + "\n" );
            }
            #endregion

#if !TREECOM
            mID = PortUtil.getMD5FromString( (long)PortUtil.getCurrentTime() + "" ).Replace( "_", "" );
            mTempWaveDir = PortUtil.combinePath( getCadenciiTempDir(), mID );
            if ( !PortUtil.isDirectoryExists( mTempWaveDir ) ) {
                PortUtil.createDirectory( mTempWaveDir );
            }
            String log = PortUtil.combinePath( getTempWaveDir(), "run.log" );
#endif

            reloadUtauVoiceDB();

            s_auto_backup_timer = new BTimer();
            s_auto_backup_timer.tickEvent.add( new BEventHandler( typeof( AppManager ), "handleAutoBackupTimerTick" ) );
        }

        /// <summary>
        /// utauVoiceDBフィールドのリストを一度クリアし，
        /// editorConfig.Utausingersの情報を元に最新の情報に更新します
        /// </summary>
        public static void reloadUtauVoiceDB() {
            mUtauVoiceDB.clear();
            for ( Iterator<SingerConfig> itr = editorConfig.UtauSingers.iterator(); itr.hasNext(); ) {
                SingerConfig config = itr.next();

                // 通常のUTAU音源
                UtauVoiceDB db = null;
                try {
                    db = new UtauVoiceDB( config );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "AppManager#reloadUtauVoiceDB; ex=" + ex );
                    db = null;
                    Logger.write( typeof( AppManager ) + ".reloadUtauVoiceDB; ex=" + ex + "\n" );
                }
                if ( db != null ) {
                    mUtauVoiceDB.put( config.VOICEIDSTR, db );
                }

                // Straight用に解析されたUTAU音源
                SingerConfig sc = (SingerConfig)config.clone();
                sc.VOICEIDSTR = PortUtil.combinePath( sc.VOICEIDSTR, "analyzed" );
                if ( PortUtil.isDirectoryExists( sc.VOICEIDSTR ) ) {
                    UtauVoiceDB db2 = null;
                    try {
                        db2 = new UtauVoiceDB( sc );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "AppManager#reloadUtauVoiceDB; ex=" + ex );
                        db2 = null;
                        Logger.write( typeof( AppManager ) + ".reloadUtauVoiceDB; ex=" + ex + "\n" );
                    }
                    if ( db2 != null ) {
                        mUtauVoiceDB.put( sc.VOICEIDSTR, db2 );
                    }
                } else {
#if DEBUG
                    PortUtil.println( "ApManager#reloadUtauVoiceDB; directory not found; sc.VOICDIDSTR=" + sc.VOICEIDSTR );
#endif
                }
            }
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
                    Logger.write( typeof( AppManager ) + ".getDeserializedObjectFromText; ex=" + ex + "\n" );
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
                Logger.write( typeof( AppManager ) + ".setClipboard; ex=" + ex + "\n" );
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
                            Logger.write( typeof( AppManager ) + ".getCopiedItems; ex=" + ex + "\n" );
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
                Logger.write( typeof( AppManager ) + ".setClipboard; ex=" + ex + "\n" );
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
        #endregion

        /// <summary>
        /// 位置クオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
        /// </summary>
        /// <returns></returns>
        public static int getPositionQuantizeClock() {
            return QuantizeModeUtil.getQuantizeClock( editorConfig.getPositionQuantize(), editorConfig.isPositionQuantizeTriplet() );
        }

        /// <summary>
        /// 音符長さクオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
        /// </summary>
        /// <returns></returns>
        public static int getLengthQuantizeClock() {
            return QuantizeModeUtil.getQuantizeClock( editorConfig.getLengthQuantize(), editorConfig.isLengthQuantizeTriplet() );
        }

        /// <summary>
        /// 現在の設定を設定ファイルに書き込みます。
        /// </summary>
        public static void saveConfig() {
#if JAVA
            //TODO: AppManager#saveConfig
            PortUtil.println( "AppManager#saveConfig; FIXME" );
#else
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
                Logger.write( typeof( AppManager ) + ".saveConfig; ex=" + ex + "\n" );
            }
#endif
        }

        /// <summary>
        /// 設定ファイルを読み込みます。
        /// 設定ファイルが壊れていたり存在しない場合、デフォルトの設定が使われます。
        /// </summary>
        public static void loadConfig() {
            String appdata = Utility.getApplicationDataPath();
#if DEBUG
            PortUtil.println( "AppManager#loadConfig; appdata=" + appdata );
#endif
            if ( appdata.Equals( "" ) ) {
                editorConfig = new EditorConfig();
                return;
            }
            String config_file = PortUtil.combinePath( appdata, CONFIG_FILE_NAME );
#if DEBUG
            PortUtil.println( "AppManager#loadConfig; config_file=" + config_file + "; isFileExists(config_file)=" + PortUtil.isFileExists( config_file ) );
#endif
            EditorConfig ret = null;
            if ( PortUtil.isFileExists( config_file ) ) {
                try {
                    ret = EditorConfig.deserialize( config_file );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "AppManager#loadConfig; ex=" + ex );
                    ret = null;
                    Logger.write( typeof( AppManager ) + ".loadConfig; ex=" + ex + "\n" );
                }
            } else {
                config_file = PortUtil.combinePath( appdata, CONFIG_FILE_NAME );
                if ( PortUtil.isFileExists( config_file ) ) {
                    try {
                        ret = EditorConfig.deserialize( config_file );
                    } catch ( Exception ex ) {
                        PortUtil.stderr.println( "AppManager#locdConfig; ex=" + ex );
                        ret = null;
                        Logger.write( typeof( AppManager ) + ".loadConfig; ex=" + ex + "\n" );
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
        }

        public static Color getHilightColor() {
            return s_hilight_brush;
        }

        public static void setHilightColor( Color value ) {
            s_hilight_brush = value;
        }

        /// <summary>
        /// ピアノロール上の音符の警告色を取得します．
        /// 音抜けの可能性がある音符の背景色として利用されます
        /// </summary>
        /// <returns></returns>
        public static Color getAlertColor() {
            return PortUtil.HotPink;
        }

        /// <summary>
        /// ピアノロール上の音符の警告色を取得します．
        /// 音抜けの可能性のある音符であって，かつ現在選択されている音符の背景色として利用されます．
        /// </summary>
        /// <returns></returns>
        public static Color getAlertHilightColor() {
            return PortUtil.DeepPink;
        }

        /// <summary>
        /// ベースとなるテンポ。
        /// </summary>
        public static int getBaseTempo() {
            return s_base_tempo;
        }

        public static void setBaseTempo( int value ) {
            s_base_tempo = value;
        }
    }

#if !JAVA
}
#endif
