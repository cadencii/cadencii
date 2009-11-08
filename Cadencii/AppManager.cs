/*
 * AppManager.cs
 * Copyright (c) 2009 kbinani
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
#if JAVA
package org.kbinani.Cadencii;

import java.awt.*;
import java.io.*;
import java.util.*;
import javax.swing.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
import org.kbinani.xml.*;
#else
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows.Forms;
using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using bocoree;
using bocoree.awt;
using bocoree.io;
using bocoree.util;
using bocoree.windows.forms;
using bocoree.xml;
using Microsoft.CSharp;

namespace Boare.Cadencii {
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;
    using Integer = System.Int32;
    using java = bocoree;
    using Long = System.Int64;
#endif

    public class AppManager {
        public const int MIN_KEY_WIDTH = 68;
        public const int MAX_KEY_WIDTH = MIN_KEY_WIDTH * 5;
        private const String CONFIG_FILE_NAME = "config.xml";
        private const String CONFIG_DIR_NAME = "Cadencii";
        /// <summary>
        /// OSのクリップボードに貼り付ける文字列の接頭辞．これがついていた場合，クリップボードの文字列をCadenciiが使用できると判断する．
        /// </summary>
        private const String CLIP_PREFIX = "CADENCIIOBJ";
        public const int MSGBOX_DEFAULT_OPTION = -1;
        public const int MSGBOX_YES_NO_OPTION = 0;
        public const int MSGBOX_YES_NO_CANCEL_OPTION = 1;
        public const int MSGBOX_OK_CANCEL_OPTION = 2;

        public const int MSGBOX_ERROR_MESSAGE = 0;
        public const int MSGBOX_INFORMATION_MESSAGE = 1;
        public const int MSGBOX_WARNING_MESSAGE = 2;
        public const int MSGBOX_QUESTION_MESSAGE = 3;
        public const int MSGBOX_PLAIN_MESSAGE = -1;

        public const int YES_OPTION = 0;
        public const int NO_OPTION = 1;
        public const int CANCEL_OPTION = 2;
        public const int OK_OPTION = 0;
        public const int CLOSED_OPTION = -1;

        /// <summary>
        /// 鍵盤の表示幅(pixel)
        /// </summary>
        public static int keyWidth = MIN_KEY_WIDTH;
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
        public static Font baseFont8 = new Font( Font.DIALOG, Font.PLAIN, 8 );
        public static Font baseFont9 = new Font( Font.DIALOG, Font.PLAIN, 9 );
        /// <summary>
        /// ピアノロールの歌詞の描画に使用されるフォント。
        /// </summary>
        public static Font baseFont10 = new Font( Font.DIALOG, Font.PLAIN, 10 );
        /// <summary>
        /// ピアノロールの歌詞の描画に使用されるフォント。（発音記号固定の物の場合）
        /// </summary>
        public static Font baseFont10Bold = new Font( Font.DIALOG, Font.BOLD, 10 );
        /// <summary>
        /// 歌詞を音符の（高さ方向の）真ん中に描画するためのオフセット
        /// </summary>
        public static int baseFont10OffsetHeight = 0;
        public static int baseFont8OffsetHeight = 0;
        public static int baseFont9OffsetHeight = 0;
#if ENABLE_PROPERTY
        public static PropertyPanel propertyPanel;
        public static FormNoteProperty propertyWindow;
#endif

        #region Static Readonly Fields
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
                                             "using Boare.Lib.Vsq;",
                                             "using Boare.Cadencii;",
                                             "using bocoree;",
                                             "using bocoree.io;",
                                             "using bocoree.util;",
                                             "using bocoree.awt;",
                                             "using Boare.Lib.Media;",
                                             "using Boare.Lib.AppUtil;",
                                             "using System.Windows.Forms;",
                                             "using System.Collections.Generic;",
                                             "using System.Drawing;",
                                             "using System.Text;",
                                             "using System.Xml.Serialization;" };
        public static readonly Vector<BKeys> SHORTCUT_ACCEPTABLE = new Vector<BKeys>( new BKeys[]{
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
        } );
        public static readonly CurveType[] CURVE_USAGE = new CurveType[]{ CurveType.DYN,
                                                                          CurveType.BRE,
                                                                          CurveType.BRI,
                                                                          CurveType.CLE,
                                                                          CurveType.OPE,
                                                                          CurveType.GEN,
                                                                          CurveType.POR,
                                                                          CurveType.PIT,
                                                                          CurveType.PBS,
                                                                          CurveType.VibratoRate,
                                                                          CurveType.VibratoDepth,
                                                                          CurveType.harmonics,
                                                                          CurveType.fx2depth,
                                                                          CurveType.reso1amp,
                                                                          CurveType.reso1bw,
                                                                          CurveType.reso1freq,
                                                                          CurveType.reso2amp,
                                                                          CurveType.reso2bw,
                                                                          CurveType.reso2freq,
                                                                          CurveType.reso3amp,
                                                                          CurveType.reso3bw,
                                                                          CurveType.reso3freq,
                                                                          CurveType.reso4amp,
                                                                          CurveType.reso4bw,
                                                                          CurveType.reso4freq };
        #endregion

        #region Private Static Fields
        private static int s_base_tempo = 480000;
        private static Color s_hilight_brush = PortUtil.CornflowerBlue;
        private static Object s_locker;
        private static BTimer s_auto_backup_timer;
        #endregion

        /// <summary>
        /// 現在表示されているピアノロール画面の右上の、仮想スクリーン上座標で見たときのy座標(pixel)
        /// </summary>
        public static int startToDrawY = 0;
#if !TREECOM
        private static VsqFileEx s_vsq;
#endif
        private static String s_file = "";
        private static int s_selected = 1;
        private static int s_current_clock = 0;
        private static boolean s_playing = false;
        private static boolean s_repeat_mode = false;
        private static boolean s_grid_visible = false;
        private static EditMode s_edit_mode = EditMode.NONE;
        /// <summary>
        /// トラックのオーバーレイ表示
        /// </summary>
        private static boolean s_overlay = true;
        //private static SelectedEventList s_selected_event = new SelectedEventList();
        /// <summary>
        /// 選択されているイベントのリスト
        /// </summary>
        private static Vector<SelectedEventEntry> s_selected_events = new Vector<SelectedEventEntry>();
        /// <summary>
        /// 選択されているテンポ変更イベントのリスト
        /// </summary>
        private static TreeMap<Integer, SelectedTempoEntry> s_selected_tempo = new TreeMap<Integer, SelectedTempoEntry>();
        private static int s_last_selected_tempo = -1;
        /// <summary>
        /// 選択されている拍子変更イベントのリスト
        /// </summary>
        private static TreeMap<Integer, SelectedTimesigEntry> s_selected_timesig = new TreeMap<Integer, SelectedTimesigEntry>();
        private static int s_last_selected_timesig = -1;
        private static EditTool s_selected_tool = EditTool.PENCIL;
#if !TREECOM
        private static Vector<ICommand> s_commands = new Vector<ICommand>();
        private static int s_command_position = -1;
#endif
        /// <summary>
        /// 選択されているベジエ点のリスト
        /// </summary>
        private static Vector<SelectedBezierPoint> s_selected_bezier = new Vector<SelectedBezierPoint>();
        /// <summary>
        /// 最後に選択されたベジエ点
        /// </summary>
        private static SelectedBezierPoint s_last_selected_bezier = new SelectedBezierPoint();
        /// <summary>
        /// 現在の演奏カーソルの位置(m_current_clockと意味は同じ。CurrentClockが変更されると、自動で更新される)
        /// </summary>
        private static PlayPositionSpecifier s_current_play_position;

        /// <summary>
        /// selectedPointIDsに格納されているデータ点の，CurveType
        /// </summary>
        private static CurveType selectedPointCurveType = CurveType.Empty;
        private static Vector<Long> selectedPointIDs = new Vector<Long>();

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
        public static java.awt.Rectangle curveSelectingRectangle;
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
        #endregion

        /// <summary>
        /// 自動ノーマライズモードかどうかを表す値を取得、または設定します。
        /// </summary> 
        public static boolean autoNormalize = false;
        /// <summary>
        /// エンドマーカーの位置(clock)
        /// </summary>
        public static int endMarker = 0;
        public static boolean endMarkerEnabled = false;
        /// <summary>
        /// x方向の表示倍率(pixel/clock)
        /// </summary>
        public static float scaleX = 0.1f;
        /// <summary>
        /// スタートマーカーの位置(clock)
        /// </summary>
        public static int startMarker = 0;
        /// <summary>
        /// Bezierカーブ編集モードが有効かどうかを表す
        /// </summary>
        private static boolean s_is_curve_mode = false;
        public static boolean startMarkerEnabled = false;
        /// <summary>
        /// 再生時に自動スクロールするかどうか
        /// </summary>
        public static boolean autoScroll = true;
        /// <summary>
        /// 最初のバッファが書き込まれたかどうか
        /// </summary>
        public static boolean firstBufferWritten = false;
        /// <summary>
        /// リアルタイム再生で使用しようとしたVSTiが有効だったかどうか。
        /// </summary>
        public static boolean rendererAvailable = false;
        /// <summary>
        /// プレビュー再生が開始された時刻
        /// </summary>
        public static double previewStartedTime;
        /// <summary>
        /// 画面左端位置での、仮想画面上の仮想画面左端から測ったピクセル数．FormMain.hScroll.ValueとFormMain.trackBar.Valueで決まる．
        /// </summary>
        public static int startToDrawX;
        public static String selectedPaletteTool = "";
        public static ScreenStatus lastScreenStatus = new ScreenStatus();
        private static String s_id = "";
        public static FormMain mainWindow = null;
        /// <summary>
        /// ミキサーダイアログ
        /// </summary>
        public static FormMixer mixerWindow;
        /// <summary>
        /// 画面に描かれるエントリのリスト．trackBar.Valueの変更やエントリの編集などのたびに更新される
        /// </summary>
        public static Vector<Vector<DrawObject>> drawObjects;
        public static TextBoxEx inputTextBox = null;
        public static int addingEventLength;
        public static VsqEvent addingEvent;
        /// <summary>
        /// AppManager.m_draw_objectsを描く際の，最初に検索されるインデクス．
        /// </summary>
        public static int[] drawStartIndex = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// マウスが降りていて，かつ範囲選択をしているときに立つフラグ
        /// </summary>
        public static boolean isPointerDowned = false;
        /// <summary>
        /// マウスが降りた仮想スクリーン上の座標(ピクセル)
        /// </summary>
        public static Point mouseDownLocation;
        public static int lastTrackSelectorHeight;

#if JAVA
        public static BEvent<BEventHandler> gridVisibleChangedEvent = new BEvent<BEventHandler>();
        public static BEvent<BEventHandler> previewStartedEvent = new BEvent<BEventHandler>();
        public static BEvent<BEventHandler> previewAbortedEvent = new BEvent<BEventHandler>();
        public static BEvent<SelectedEventChangedEventHandler> electedEventChangedEvent = new BEvent<SelectedEventChangedEventHandler>();
        public static BEvent<BEventHandler> selectedToolChangedEvent = new BEvent<BEventHandler>();
        /// <summary>
        /// CurrentClockプロパティの値が変更された時発生します
        /// </summary>
        public static BEvent<BEventHandler> currentClockChangedEvent= new BEvent<BEventHandler>();
#else
        /// <summary>
        /// グリッド線を表示するか否かを表す値が変更された時発生します
        /// </summary>
        public static event EventHandler GridVisibleChanged;
        public static event EventHandler PreviewStarted;
        public static event EventHandler PreviewAborted;
        public static event SelectedEventChangedEventHandler SelectedEventChanged;
        public static event EventHandler SelectedToolChanged;
        /// <summary>
        /// CurrentClockプロパティの値が変更された時発生します
        /// </summary>
        public static event EventHandler CurrentClockChanged;
#endif

        private const String TEMPDIR_NAME = "cadencii";

#if JAVA
        static{
#else
        static AppManager() {
#endif
            s_auto_backup_timer = new BTimer();
#if JAVA
            s_auto_backup_timer.tickEvent.add( new BEventHandler( this, "handleAutoBackupTimerTick" ) );
#else
            s_auto_backup_timer.Tick += handleAutoBackupTimerTick;
#endif
        }

        /// <summary>
        /// クロック数から、画面に描くべきx座標の値を取得します。
        /// </summary>
        /// <param name="clocks"></param>
        /// <returns></returns>
        public static int xCoordFromClocks( double clocks ) {
            return (int)(keyWidth + clocks * scaleX - startToDrawX) + 6;
        }

        /// <summary>
        /// 画面のx座標からクロック数を取得します
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int clockFromXCoord( int x ) {
            return (int)((x + startToDrawX - 6 - keyWidth) / scaleX);
        }

        #region 選択範囲の管理
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
            return showMessageBox( text, "", MSGBOX_DEFAULT_OPTION, MSGBOX_PLAIN_MESSAGE );
        }

        public static BDialogResult showMessageBox( String text, String caption ) {
            return showMessageBox( text, caption, MSGBOX_DEFAULT_OPTION, MSGBOX_PLAIN_MESSAGE );
        }

        public static BDialogResult showMessageBox( String text, String caption, int optionType ) {
            return showMessageBox( text, caption, optionType, MSGBOX_PLAIN_MESSAGE );
        }

        public static BDialogResult showMessageBox( String text, String caption, int optionType, int messageType ) {
#if ENABLE_PROPERTY
            bool property = (propertyWindow != null) ? propertyWindow.TopMost : false;
            if ( property ) {
                propertyWindow.TopMost = false;
            }
#endif
            bool mixer = (mixerWindow != null) ? mixerWindow.TopMost : false;
            if ( mixer ) {
                mixerWindow.TopMost = false;
            }

            BDialogResult ret = BDialogResult.CANCEL;
#if JAVA
            int r = JOptionPane.showConfirmDialog( null, text, caption, optionType, messageType );
            if ( r == JOptionPane.YES_OPTION ){
                ret = BDialogResult.YES;
            } else if ( r == JOptionPane.NO_OPTION ){
                ret = BDialogResult.NO;
            } else if ( r == JOptionPane.CANCEL_OPTION ){
                ret = BDialogResult.CANCEL;
            } else if ( r == JOptionPane.OK_OPTION ){
                ret = BDialogResult.OK;
            } else if ( r == JOptionPane.CLOSED_OPTION ){
                ret = BDialogResult.CANCEL;
            }
#else
            MessageBoxButtons btn = MessageBoxButtons.OK;
            if ( optionType == MSGBOX_YES_NO_CANCEL_OPTION ) {
                btn = MessageBoxButtons.YesNoCancel;
            } else if ( optionType == MSGBOX_YES_NO_OPTION ) {
                btn = MessageBoxButtons.YesNo;
            } else if ( optionType == MSGBOX_OK_CANCEL_OPTION ) {
                btn = MessageBoxButtons.OKCancel;
            } else {
                btn = MessageBoxButtons.OK;
            }

            MessageBoxIcon icon = MessageBoxIcon.None;
            if ( messageType == MSGBOX_ERROR_MESSAGE ) {
                icon = MessageBoxIcon.Error;
            } else if ( messageType == MSGBOX_INFORMATION_MESSAGE ) {
                icon = MessageBoxIcon.Information;
            } else if ( messageType == MSGBOX_PLAIN_MESSAGE ) {
                icon = MessageBoxIcon.None;
            } else if ( messageType == MSGBOX_QUESTION_MESSAGE ) {
                icon = MessageBoxIcon.Question;
            } else if ( messageType == MSGBOX_WARNING_MESSAGE ) {
                icon = MessageBoxIcon.Warning;
            }

            DialogResult dr = MessageBox.Show( text, caption, btn, icon );
            if ( dr == DialogResult.OK ) {
                ret = BDialogResult.OK;
            } else if ( dr == DialogResult.Cancel ) {
                ret = BDialogResult.CANCEL;
            } else if ( dr == DialogResult.Yes ) {
                ret = BDialogResult.YES;
            } else if ( dr == DialogResult.No ) {
                ret = BDialogResult.NO;
            }
#endif

#if ENABLE_PROPERTY
            if ( property ) {
                propertyWindow.TopMost = true;
            }
#endif
            if ( mixer ) {
                mixerWindow.TopMost = true;
            }
            if ( mainWindow != null ) {
                mainWindow.Focus();
            }

            return ret;
        }

        /*public static BDialogResult showMessageBox( String text, String caption, MessageBoxButtons buttons, MessageBoxIcon icon ) {
#if ENABLE_PROPERTY
            bool property = (propertyWindow != null) ? propertyWindow.TopMost : false;
            if ( property ) {
                propertyWindow.TopMost = false;
            }
#endif
            bool mixer = (mixerWindow != null) ? mixerWindow.TopMost : false;
            if ( mixer ) {
                mixerWindow.TopMost = false;
            }
            DialogResult dr = MessageBox.Show( text, caption, buttons, icon );
#if ENABLE_PROPERTY
            if ( property ) {
                propertyWindow.TopMost = true;
            }
#endif
            if ( mixer ) {
                mixerWindow.TopMost = true;
            }

            if ( mainWindow != null ) {
                mainWindow.Focus();
            }
            if ( dr == DialogResult.OK ) {
                return BDialogResult.OK;
            } else if ( dr == DialogResult.Cancel ) {
                return BDialogResult.CANCEL;
            } else if ( dr == DialogResult.Yes ) {
                return BDialogResult.YES;
            } else if ( dr == DialogResult.No ) {
                return BDialogResult.NO;
            }
            return BDialogResult.CANCEL;
        }*/
        #endregion

        #region BGM 関連
        public static int getBgmCount() {
            if ( s_vsq == null ) {
                return 0;
            } else {
                return s_vsq.BgmFiles.size();
            }
        }

        public static BgmFile getBgm( int index ) {
            if ( s_vsq == null ) {
                return null;
            }
            return s_vsq.BgmFiles.get( index );
        }

        public static void removeBgm( int index ) {
            if ( s_vsq == null ) {
                return;
            }
            Vector<BgmFile> list = new Vector<BgmFile>();
            int count = s_vsq.BgmFiles.size();
            for ( int i = 0; i < count; i++ ) {
                if ( i != index ) {
                    list.add( (BgmFile)s_vsq.BgmFiles.get( i ).clone() );
                }
            }
            CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate( list );
            register( s_vsq.executeCommand( run ) );
            mainWindow.setEdited( true );
            mixerWindow.updateStatus();
        }

        public static void clearBgm() {
            if ( s_vsq == null ) {
                return;
            }
            Vector<BgmFile> list = new Vector<BgmFile>();
            CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate( list );
            register( s_vsq.executeCommand( run ) );
            mainWindow.setEdited( true );
            mixerWindow.updateStatus();
        }

        public static void addBgm( String file ) {
            if ( s_vsq == null ) {
                return;
            }
            Vector<BgmFile> list = new Vector<BgmFile>();
            int count = s_vsq.BgmFiles.size();
            for ( int i = 0; i < count; i++ ) {
                list.add( (BgmFile)s_vsq.BgmFiles.get( i ).clone() );
            }
            BgmFile item = new BgmFile();
            item.file = file;
            item.feder = 0;
            item.panpot = 0;
            list.add( item );
            CadenciiCommand run = VsqFileEx.generateCommandBgmUpdate( list );
            register( s_vsq.executeCommand( run ) );
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
            if ( s_vsq != null && 1 <= track && track < s_vsq.Track.size() ) {
                double amp_master = VocaloSysUtil.getAmplifyCoeffFromFeder( s_vsq.Mixer.MasterFeder );
                double pan_left_master = VocaloSysUtil.getAmplifyCoeffFromPanLeft( s_vsq.Mixer.MasterPanpot );
                double pan_right_master = VocaloSysUtil.getAmplifyCoeffFromPanRight( s_vsq.Mixer.MasterPanpot );
                double amp_track = VocaloSysUtil.getAmplifyCoeffFromFeder( s_vsq.Mixer.Slave.get( track - 1 ).Feder );
                double pan_left_track = VocaloSysUtil.getAmplifyCoeffFromPanLeft( s_vsq.Mixer.Slave.get( track - 1 ).Panpot );
                double pan_right_track = VocaloSysUtil.getAmplifyCoeffFromPanRight( s_vsq.Mixer.Slave.get( track - 1 ).Panpot );
                ret.left = amp_master * amp_track * pan_left_master * pan_left_track;
                ret.right = amp_master * amp_track * pan_right_master * pan_right_track;
            }
            return ret;
        }

        public static AmplifyCoefficient getAmplifyCoeffBgm( int index ) {
            AmplifyCoefficient ret = new AmplifyCoefficient();
            ret.left = 1.0;
            ret.right = 1.0;
            if ( s_vsq != null && 0 <= index && index < s_vsq.BgmFiles.size() ) {
                BgmFile item = s_vsq.BgmFiles.get( index );
                if ( item.mute == 1 ) {
                    ret.left = 0.0;
                    ret.right = 0.0;
                } else {
                    double amp_master = VocaloSysUtil.getAmplifyCoeffFromFeder( s_vsq.Mixer.MasterFeder );
                    double pan_left_master = VocaloSysUtil.getAmplifyCoeffFromPanLeft( s_vsq.Mixer.MasterPanpot );
                    double pan_right_master = VocaloSysUtil.getAmplifyCoeffFromPanRight( s_vsq.Mixer.MasterPanpot );
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
            if ( !s_file.Equals( "" ) && editorConfig.AutoBackupIntervalMinutes > 0 ) {
                double millisec = editorConfig.AutoBackupIntervalMinutes * 60.0 * 1000.0;
                int draft = (int)millisec;
                if ( millisec > int.MaxValue ) {
                    draft = int.MaxValue;
                }
                s_auto_backup_timer.Interval = draft;
                s_auto_backup_timer.Start();
            } else {
                s_auto_backup_timer.Stop();
            }
        }

        private static void handleAutoBackupTimerTick( Object sender, BEventArgs e ) {
            if ( !s_file.Equals( "" ) && PortUtil.isFileExists( s_file ) ) {
                String path = PortUtil.getDirectoryName( s_file );
                String backup = PortUtil.combinePath( path, "~$" + PortUtil.getFileName( s_file ) );
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

        /// <summary>
        /// pがrcの中にあるかどうかを判定します
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rc"></param>
        /// <returns></returns>
        public static boolean isInRect( java.awt.Point p, java.awt.Rectangle rc ) {
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

        /// <summary>
        /// 文字列itemをfontを用いて描画したとき、幅widthピクセルに収まるようにitemを調節したものを返します。
        /// 例えば"1 Voice"→"1 Voi..."ナド。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="font"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static String trimString( String item, Font font, int width ) {
            String edited = item;
            int delete_count = item.Length;
            while ( true ) {
                Dimension measured = Util.measureString( edited, font );
                if ( measured.width <= width ) {
                    return edited;
                }
                delete_count -= 1;
                if ( delete_count > 0 ) {
                    edited = item.Substring( 0, delete_count ) + "...";
                } else {
                    return edited;
                }
            }
        }


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

#if ENABLE_SCRIPT
        public static ScriptInvoker loadScript( String file ) {
#if JAVA
            ScriptInvoker ret = new ScriptInvoker();
            return ret;
#else
#if DEBUG
            AppManager.debugWriteLine( "AppManager#loadScript(String)" );
            AppManager.debugWriteLine( "    File.GetLastWriteTimeUtc( file )=" + System.IO.File.GetLastWriteTimeUtc( file ) );
#endif
            ScriptInvoker ret = new ScriptInvoker();
            ret.ScriptFile = file;
            ret.FileTimestamp = System.IO.File.GetLastWriteTimeUtc( file );
            // スクリプトの記述のうち、以下のリストに当てはまる部分は空文字に置換される
            String config_file = configFileNameFromScriptFileName( file );
            String script = "";
            BufferedReader sr = null;
            try {
                sr = new BufferedReader( new FileReader( file ) );
                String line = "";
                while ( (line = sr.readLine()) != null ) {
                    script += line + "\n";
                }
            } catch ( Exception ex ) {
#if DEBUG
                PortUtil.println( "AppManager#loadScript; ex=" + ex );
#endif
            } finally {
                if ( sr != null ) {
                    try {
                        sr.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
            foreach ( String s in AppManager.USINGS ) {
                script = script.Replace( s, "" );
            }

            String code = "";
            foreach ( String s in AppManager.USINGS ) {
                code += s;
            }
            code += "namespace Boare.CadenciiScript{";
            code += script;
            code += "}";
            ret.ErrorMessage = "";

            CompilerResults results = AppManager.compileScript( code );
            Assembly testAssembly = null;
            if ( results != null ) {
                try {
                    testAssembly = results.CompiledAssembly;
                } catch ( Exception ex ) {
                    testAssembly = null;
                }
            }
#if DEBUG
            PortUtil.println( "AppManager#loadScript; (results==null)=" + (results == null) );
            PortUtil.println( "AppManager#loadScript; (testAssembly==null)=" + (testAssembly == null) );
#endif

            if ( testAssembly == null ) {
                ret.scriptDelegate = null;
                if ( results == null ) {
                    ret.ErrorMessage = "failed compiling";
                    return ret;
                } else {
                    if ( results.Errors.Count != 0 ) {
                        for ( int i = 0; i < results.Errors.Count; i++ ) {
                            ret.ErrorMessage += _( "line" ) + ":" + results.Errors[i].Line + " " + results.Errors[i].ErrorText + Environment.NewLine;
                        }
                    }
                    return ret;
                }
            } else {
                foreach ( Type implemented in testAssembly.GetTypes() ) {
                    Object scriptDelegate = null;
                    MethodInfo tmi = implemented.GetMethod( "Edit", new Type[] { typeof( VsqFile ) } );
                    if ( tmi != null && tmi.IsStatic && tmi.IsPublic ) {
                        if ( tmi.ReturnType.Equals( typeof( boolean ) ) ) {
                            scriptDelegate = (EditVsqScriptDelegate)Delegate.CreateDelegate( typeof( EditVsqScriptDelegate ), tmi );
                        } else if ( tmi.ReturnType.Equals( typeof( ScriptReturnStatus ) ) ) {
                            scriptDelegate = (EditVsqScriptDelegateWithStatus)Delegate.CreateDelegate( typeof( EditVsqScriptDelegateWithStatus ), tmi );
                        }
                    }
                    tmi = implemented.GetMethod( "Edit", new Type[] { typeof( VsqFileEx ) } );
                    if ( tmi != null && tmi.IsStatic && tmi.IsPublic ) {
                        if ( tmi.ReturnType.Equals( typeof( boolean ) ) ) {
                            scriptDelegate = (EditVsqScriptDelegateEx)Delegate.CreateDelegate( typeof( EditVsqScriptDelegateEx ), tmi );
                        } else if ( tmi.ReturnType.Equals( typeof( ScriptReturnStatus ) ) ) {
                            scriptDelegate = (EditVsqScriptDelegateExWithStatus)Delegate.CreateDelegate( typeof( EditVsqScriptDelegateExWithStatus ), tmi );
                        }
                    }
                    if ( scriptDelegate != null ) {
                        ret.ScriptType = implemented;
                        ret.scriptDelegate = scriptDelegate;
                        ret.Serializer = new XmlSerializer( implemented, true );

                        if ( !PortUtil.isFileExists( config_file ) ) {
                            continue;
                        }

                        // 設定ファイルからDeserialize
                        using ( System.IO.FileStream fs = new System.IO.FileStream( config_file, System.IO.FileMode.Open ) ) {
                            ret.Serializer.deserialize( fs );
                        }
                    }
                }
            }
            return ret;
#endif
        }
#endif

#if ENABLE_SCRIPT
        /// <summary>
        /// スクリプトを実行します
        /// </summary>
        /// <param name="evsd"></param>
        public static boolean invokeScript( ScriptInvoker script_invoker ) {
            if ( script_invoker != null && script_invoker.scriptDelegate != null ) {
                try {
                    VsqFileEx work = (VsqFileEx)s_vsq.clone();
                    ScriptReturnStatus ret = ScriptReturnStatus.ERROR;
                    if ( script_invoker.scriptDelegate is EditVsqScriptDelegate ) {
                        boolean b_ret = ((EditVsqScriptDelegate)script_invoker.scriptDelegate).Invoke( work );
                        if ( b_ret ) {
                            ret = ScriptReturnStatus.EDITED;
                        } else {
                            ret = ScriptReturnStatus.ERROR;
                        }
                    } else if ( script_invoker.scriptDelegate is EditVsqScriptDelegateEx ) {
                        boolean b_ret = ((EditVsqScriptDelegateEx)script_invoker.scriptDelegate).Invoke( work );
                        if ( b_ret ) {
                            ret = ScriptReturnStatus.EDITED;
                        } else {
                            ret = ScriptReturnStatus.ERROR;
                        }
                    } else if ( script_invoker.scriptDelegate is EditVsqScriptDelegateWithStatus ) {
                        ret = ((EditVsqScriptDelegateWithStatus)script_invoker.scriptDelegate).Invoke( work );
                    } else if ( script_invoker.scriptDelegate is EditVsqScriptDelegateExWithStatus ) {
                        ret = ((EditVsqScriptDelegateExWithStatus)script_invoker.scriptDelegate).Invoke( work );
                    } else {
                        ret = ScriptReturnStatus.ERROR;
                    }
                    if ( ret == ScriptReturnStatus.ERROR ) {
                        AppManager.showMessageBox( _( "Script aborted" ), "Cadencii", MSGBOX_DEFAULT_OPTION, MSGBOX_INFORMATION_MESSAGE );
                    } else if ( ret == ScriptReturnStatus.EDITED ) {
#if DEBUG
                        PortUtil.println( "AppManager#invokeScript; BEFORE: work.Track[1].getCurve( \"reso1amp\" ).getCount()=" + work.Track[1].getCurve( "reso1amp" ).size() );
                        PortUtil.println( "AppManager#invokeScript; BEFORE: work.Track[1].getCurve( \"reso2amp\" ).getCount()=" + work.Track[1].getCurve( "reso2amp" ).size() );
                        PortUtil.println( "AppManager#invokeScript; BEFORE: work.Track[1].getCurve( \"reso3amp\" ).getCount()=" + work.Track[1].getCurve( "reso3amp" ).size() );
                        PortUtil.println( "AppManager#invokeScript; BEFORE: work.Track[1].getCurve( \"reso4amp\" ).getCount()=" + work.Track[1].getCurve( "reso4amp" ).size() );
                        PortUtil.println( "AppManager#invokeScript; BEFORE: s_vsq.Track[1].getCurve( \"reso1amp\" ).getCount()=" + s_vsq.Track[1].getCurve( "reso1amp" ).size() );
                        PortUtil.println( "AppManager#invokeScript; BEFORE: s_vsq.Track[1].getCurve( \"reso2amp\" ).getCount()=" + s_vsq.Track[1].getCurve( "reso2amp" ).size() );
                        PortUtil.println( "AppManager#invokeScript; BEFORE: s_vsq.Track[1].getCurve( \"reso3amp\" ).getCount()=" + s_vsq.Track[1].getCurve( "reso3amp" ).size() );
                        PortUtil.println( "AppManager#invokeScript; BEFORE: s_vsq.Track[1].getCurve( \"reso4amp\" ).getCount()=" + s_vsq.Track[1].getCurve( "reso4amp" ).size() );
#endif
                        CadenciiCommand run = VsqFileEx.generateCommandReplace( work );
                        register( s_vsq.executeCommand( run ) );
#if DEBUG
                        PortUtil.println( "AppManager#invokeScript; AFTER:  s_vsq.Track[1].getCurve( \"reso1amp\" ).getCount()=" + s_vsq.Track[1].getCurve( "reso1amp" ).size() );
                        PortUtil.println( "AppManager#invokeScript; AFTER:  s_vsq.Track[1].getCurve( \"reso2amp\" ).getCount()=" + s_vsq.Track[1].getCurve( "reso2amp" ).size() );
                        PortUtil.println( "AppManager#invokeScript; AFTER:  s_vsq.Track[1].getCurve( \"reso3amp\" ).getCount()=" + s_vsq.Track[1].getCurve( "reso3amp" ).size() );
                        PortUtil.println( "AppManager#invokeScript; AFTER:  s_vsq.Track[1].getCurve( \"reso4amp\" ).getCount()=" + s_vsq.Track[1].getCurve( "reso4amp" ).size() );
#endif
                    }
                    String config_file = configFileNameFromScriptFileName( script_invoker.ScriptFile );
                    FileOutputStream fs = null;
                    try {
                        fs = new FileOutputStream( config_file );
                        script_invoker.Serializer.serialize( fs, null );
                    } catch ( Exception ex ) {
                    } finally {
                        if ( fs != null ) {
                            try {
                                fs.close();
                            } catch ( Exception ex2 ) {
                            }
                        }
                    }
                    return (ret == ScriptReturnStatus.EDITED);
                } catch ( Exception ex ) {
                    AppManager.showMessageBox( _( "Script runtime error:" ) + " " + ex, _( "Error" ), MSGBOX_DEFAULT_OPTION, MSGBOX_INFORMATION_MESSAGE );
#if DEBUG
                    AppManager.debugWriteLine( "    " + ex );
#endif
                }
            } else {
                AppManager.showMessageBox( _( "Script compilation failed." ), _( "Error" ), MSGBOX_DEFAULT_OPTION, MSGBOX_WARNING_MESSAGE );
            }
            return false;
        }
#endif

        private static String configFileNameFromScriptFileName( String script_file ) {
            String dir = PortUtil.combinePath( AppManager.getApplicationDataPath(), "script" );
            if ( !PortUtil.isDirectoryExists( dir ) ) {
                PortUtil.createDirectory( dir );
            }
            return PortUtil.combinePath( dir, PortUtil.getFileNameWithoutExtension( script_file ) + ".config" );
        }

        public static String getTempWaveDir() {
            String temp = PortUtil.combinePath( PortUtil.getTempPath(), TEMPDIR_NAME );
            String dir = PortUtil.combinePath( temp, getID() );
            if ( !PortUtil.isDirectoryExists( temp ) ) {
                PortUtil.createDirectory( temp );
            }
            if ( !PortUtil.isDirectoryExists( dir ) ) {
                PortUtil.createDirectory( dir );
            }
            return dir;
        }

        public static boolean isCurveMode() {
            return s_is_curve_mode;
        }

        public static void setCurveMode( boolean value ) {
            boolean old = s_is_curve_mode;
            s_is_curve_mode = value;
            if ( old != s_is_curve_mode && SelectedToolChanged != null ) {
                SelectedToolChanged( typeof( AppManager ), new BEventArgs() );
            }
        }

#if !TREECOM
        /// <summary>
        /// コマンドの履歴を削除します
        /// </summary>
        public static void clearCommandBuffer() {
            s_commands.clear();
            s_command_position = -1;
        }

        /// <summary>
        /// アンドゥ処理を行います
        /// </summary>
        public static void undo() {
#if DEBUG
            AppManager.debugWriteLine( "CommonConfig.Undo()" );
#endif
            if ( isUndoAvailable() ) {
                ICommand run_src = s_commands.get( s_command_position );
                CadenciiCommand run = (CadenciiCommand)run_src;
#if DEBUG
                AppManager.debugWriteLine( "    command type=" + run.type );
#endif
                if ( run.vsqCommand != null ) {
                    if ( run.vsqCommand.Type == VsqCommandType.TRACK_DELETE ) {
                        int track = (int)run.vsqCommand.Args[0];
                        if ( track == getSelected() && track >= 2 ) {
                            setSelected( track - 1 );
                        }
                    }
                }
                ICommand inv = s_vsq.executeCommand( run );
                if ( run.type == CadenciiCommandType.BGM_UPDATE ) {
                    if ( mainWindow != null ) {
                        mainWindow.updateBgmMenuState();
                    }
                }
                s_commands.set( s_command_position, inv );
                s_command_position--;
            }
        }

        /// <summary>
        /// リドゥ処理を行います
        /// </summary>
        public static void redo() {
#if DEBUG
            AppManager.debugWriteLine( "CommonConfig.Redo()" );
#endif
            if ( isRedoAvailable() ) {
                ICommand run_src = s_commands.get( s_command_position + 1 );
                CadenciiCommand run = (CadenciiCommand)run_src;
                if ( run.vsqCommand != null ) {
                    if ( run.vsqCommand.Type == VsqCommandType.TRACK_DELETE ) {
                        int track = (int)run.args[0];
                        if ( track == getSelected() && track >= 2 ) {
                            setSelected( track - 1 );
                        }
                    }
                }
                ICommand inv = s_vsq.executeCommand( run );
                if ( run.type == CadenciiCommandType.BGM_UPDATE ) {
                    if ( mainWindow != null ) {
                        mainWindow.updateBgmMenuState();
                    }
                }
                s_commands.set( s_command_position + 1, inv );
                s_command_position++;
            }
        }

        /// <summary>
        /// コマンドバッファに指定されたコマンドを登録します
        /// </summary>
        /// <param name="command"></param>
        public static void register( ICommand command ) {
#if DEBUG
            AppManager.debugWriteLine( "EditorManager.Register; command=" + command );
            AppManager.debugWriteLine( "    m_commands.Count=" + s_commands.size() );
#endif
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
        /// リドゥ操作が可能かどうかを表すプロパティ
        /// </summary>
        public static boolean isRedoAvailable() {
            if ( s_commands.size() > 0 && 0 <= s_command_position + 1 && s_command_position + 1 < s_commands.size() ) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// アンドゥ操作が可能かどうかを表すプロパティ
        /// </summary>
        public static boolean isUndoAvailable() {
            if ( s_commands.size() > 0 && 0 <= s_command_position && s_command_position < s_commands.size() ) {
                return true;
            } else {
                return false;
            }
        }
#endif

        public static EditTool getSelectedTool() {
            return s_selected_tool;
        }

        public static void setSelectedTool( EditTool value ) {
            EditTool old = s_selected_tool;
            s_selected_tool = value;
            if ( old != s_selected_tool && SelectedToolChanged != null ) {
                SelectedToolChanged( typeof( AppManager ), new BEventArgs() );
            }
        }

        #region SelectedBezier
        public static Iterator getSelectedBezierEnumerator() {
#if JAVA
            return s_selected_bezier.iterator();
#else
            return new ListIterator<SelectedBezierPoint>( s_selected_bezier );
#endif
        }

        /// <summary>
        /// ベジエ点のリストに最後に追加された点の情報を取得します
        /// </summary>
        public static SelectedBezierPoint getLastSelectedBezier() {
            if ( s_last_selected_bezier.chainID < 0 || s_last_selected_bezier.pointID < 0 ) {
                return null;
            } else {
                return s_last_selected_bezier;
            }
        }

        /// <summary>
        /// 選択されているベジエ点のリストに、アイテムを追加します
        /// </summary>
        /// <param name="selected">追加する点</param>
        public static void addSelectedBezier( SelectedBezierPoint selected ) {
            s_last_selected_bezier = selected;
            int index = -1;
            for ( int i = 0; i < s_selected_bezier.size(); i++ ) {
                if ( s_selected_bezier.get( i ).chainID == selected.chainID &&
                    s_selected_bezier.get( i ).pointID == selected.pointID ) {
                    index = i;
                    break;
                }
            }
            if ( index >= 0 ) {
                s_selected_bezier.set( index, selected );
            } else {
                s_selected_bezier.add( selected );
            }
            checkSelectedItemExistence();
        }

        /// <summary>
        /// 選択されているベジエ点のリストを初期化します
        /// </summary>
        public static void clearSelectedBezier() {
            s_selected_bezier.clear();
            s_last_selected_bezier.chainID = -1;
            s_last_selected_bezier.pointID = -1;
            checkSelectedItemExistence();
        }
        #endregion

        #region SelectedTimesig
        public static SelectedTimesigEntry getLastSelectedTimesig() {
            if ( s_selected_timesig.containsKey( s_last_selected_timesig ) ) {
                return s_selected_timesig.get( s_last_selected_timesig );
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
            if ( !s_selected_timesig.containsKey( barcount ) ) {
                for ( Iterator itr = s_vsq.TimesigTable.iterator(); itr.hasNext(); ) {
                    TimeSigTableEntry tte = (TimeSigTableEntry)itr.next();
                    if ( tte.BarCount == barcount ) {
                        s_selected_timesig.put( barcount, new SelectedTimesigEntry( tte, (TimeSigTableEntry)tte.clone() ) );
                        break;
                    }
                }
            }
            checkSelectedItemExistence();
        }

        public static void clearSelectedTimesig() {
            s_selected_timesig.clear();
            s_last_selected_timesig = -1;
            checkSelectedItemExistence();
        }

        public static int getSelectedTimesigCount() {
            return s_selected_timesig.size();
        }

        public static Iterator getSelectedTimesigIterator() {
            Vector<ValuePair<Integer, SelectedTimesigEntry>> list = new Vector<ValuePair<int, SelectedTimesigEntry>>();
            for ( Iterator itr = s_selected_timesig.keySet().iterator(); itr.hasNext(); ) {
                int clock = (Integer)itr.next();
                list.add( new ValuePair<Integer, SelectedTimesigEntry>( clock, s_selected_timesig.get( clock ) ) );
            }
            return list.iterator();
        }

        public static boolean isSelectedTimesigContains( int barcount ) {
            return s_selected_timesig.containsKey( barcount );
        }

        public static SelectedTimesigEntry getSelectedTimesig( int barcount ) {
            if ( s_selected_timesig.containsKey( barcount ) ) {
                return s_selected_timesig.get( barcount );
            } else {
                return null;
            }
        }

        public static void removeSelectedTimesig( int barcount ) {
            if ( s_selected_timesig.containsKey( barcount ) ) {
                s_selected_timesig.remove( barcount );
                checkSelectedItemExistence();
            }
        }
        #endregion

        #region SelectedTempo
        public static SelectedTempoEntry getLastSelectedTempo() {
            if ( s_selected_tempo.containsKey( s_last_selected_tempo ) ) {
                return s_selected_tempo.get( s_last_selected_tempo );
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
            if ( !s_selected_tempo.containsKey( clock ) ) {
                for ( Iterator itr = s_vsq.TempoTable.iterator(); itr.hasNext(); ) {
                    TempoTableEntry tte = (TempoTableEntry)itr.next();
                    if ( tte.Clock == clock ) {
                        s_selected_tempo.put( clock, new SelectedTempoEntry( tte, (TempoTableEntry)tte.clone() ) );
                        break;
                    }
                }
            }
            checkSelectedItemExistence();
        }

        public static void clearSelectedTempo() {
            s_selected_tempo.clear();
            s_last_selected_tempo = -1;
            checkSelectedItemExistence();
        }

        public static int getSelectedTempoCount() {
            return s_selected_tempo.size();
        }

        public static Iterator getSelectedTempoIterator() {
            Vector<ValuePair<Integer, SelectedTempoEntry>> list = new Vector<ValuePair<int, SelectedTempoEntry>>();
            for ( Iterator itr = s_selected_tempo.keySet().iterator(); itr.hasNext(); ) {
                int clock = (Integer)itr.next();
                list.add( new ValuePair<Integer, SelectedTempoEntry>( clock, s_selected_tempo.get( clock ) ) );
            }
            return list.iterator();
        }

        public static boolean isSelectedTempoContains( int clock ) {
            return s_selected_tempo.containsKey( clock );
        }

        public static SelectedTempoEntry getSelectedTempo( int clock ) {
            if ( s_selected_tempo.containsKey( clock ) ) {
                return s_selected_tempo.get( clock );
            } else {
                return null;
            }
        }

        public static void removeSelectedTempo( int clock ) {
            if ( s_selected_tempo.containsKey( clock ) ) {
                s_selected_tempo.remove( clock );
                checkSelectedItemExistence();
            }
        }
        #endregion

        #region SelectedEvent
        /*public static SelectedEventList getSelectedEvent() {
            return s_selected_event;
        }

        [Obsolete]
        public static SelectedEventList SelectedEvent {
            get {
                return getSelectedEvent();
            }
        }*/

        public static void removeSelectedEvent( int id ) {
            removeSelectedEventCor( id, false );
            checkSelectedItemExistence();
        }

        public static void removeSelectedEventSilent( int id ) {
            removeSelectedEventCor( id, true );
            checkSelectedItemExistence();
        }

        private static void removeSelectedEventCor( int id, boolean silent ) {
            int count = s_selected_events.size();
            for ( int i = 0; i < count; i++ ) {
                if ( s_selected_events.get( i ).original.InternalID == id ) {
                    s_selected_events.removeElementAt( i );
                    break;
                }
            }
            if ( !silent ) {
#if ENABLE_PROPERTY
                propertyPanel.UpdateValue( s_selected );
#endif
            }
        }

        public static void removeSelectedEventRange( int[] ids ) {
            Vector<Integer> v_ids = new Vector<Integer>( ids );
            Vector<Integer> index = new Vector<Integer>();
            int count = s_selected_events.size();
            for ( int i = 0; i < count; i++ ) {
                if ( v_ids.contains( s_selected_events.get( i ).original.InternalID ) ) {
                    index.add( i );
                    if ( index.size() == ids.Length ) {
                        break;
                    }
                }
            }
            count = index.size();
            for ( int i = count - 1; i >= 0; i-- ) {
                s_selected_events.removeElementAt( i );
            }
#if ENABLE_PROPERTY
            propertyPanel.UpdateValue( s_selected );
#endif
            checkSelectedItemExistence();
        }

        public static void addSelectedEventAll( int[] ids ) {
            clearSelectedTempo();
            clearSelectedTimesig();
            Vector<Integer> list = new Vector<Integer>( ids );
            VsqEvent[] index = new VsqEvent[ids.Length];
            int count = 0;
            int c = list.size();
            for ( Iterator itr = s_vsq.Track.get( s_selected ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent ev = (VsqEvent)itr.next();
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
                if ( count == ids.Length ) {
                    break;
                }
            }
            for ( int i = 0; i < index.Length; i++ ) {
                if ( !isSelectedEventContains( s_selected, index[i].InternalID ) ) {
                    s_selected_events.add( new SelectedEventEntry( s_selected, index[i], (VsqEvent)index[i].clone() ) );
                }
            }
#if ENABLE_PROPERTY
            propertyPanel.UpdateValue( s_selected );
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
            for ( Iterator itr = s_vsq.Track.get( s_selected ).getEventIterator(); itr.hasNext(); ) {
                VsqEvent ev = (VsqEvent)itr.next();
                if ( ev.InternalID == id ) {
                    if ( !isSelectedEventContains( s_selected, id ) ) {
                        // まだ選択されていなかった場合
                        s_selected_events.add( new SelectedEventEntry( s_selected, ev, (VsqEvent)ev.clone() ) );
                        if ( !silent && SelectedEventChanged != null ) {
                            SelectedEventChanged( typeof( AppManager ), false );
                        }
                    } else {
                        // すでに選択されているアイテムの再選択
                        int count = s_selected_events.size();
                        for ( int i = 0; i < count; i++ ) {
                            SelectedEventEntry item = s_selected_events.get( i );
                            if ( item.original.InternalID == id ) {
                                s_selected_events.removeElementAt( i );
                                s_selected_events.add( item );
                                break;
                            }
                        }
                    }
                    break;
                }
            }
            if ( !silent ) {
#if ENABLE_PROPERTY
                propertyPanel.UpdateValue( s_selected );
#endif
            }
        }

        public static void clearSelectedEvent() {
            s_selected_events.clear();
#if ENABLE_PROPERTY
            propertyPanel.UpdateValue( s_selected );
#endif
            checkSelectedItemExistence();
        }

        public static boolean isSelectedEventContains( int track, int id ) {
            int count = s_selected_events.size();
            for ( int i = 0; i < count; i++ ) {
                SelectedEventEntry item = s_selected_events.get( i );
                if ( item.original.InternalID == id && item.track == track ) {
                    return true;
                }
            }
            return false;
        }

        public static Iterator getSelectedEventIterator() {
            return s_selected_events.iterator();
        }

        public static SelectedEventEntry getLastSelectedEvent() {
            if ( s_selected_events.size() <= 0 ) {
                return null;
            } else {
                return s_selected_events.get( s_selected_events.size() - 1 );
            }
        }

        public static int getSelectedEventCount() {
            return s_selected_events.size();
        }
        #endregion

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

        public static Iterator getSelectedPointIDIterator() {
            return new ListIterator<Long>( selectedPointIDs );
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
            if ( SelectedEventChanged != null ) {
                boolean ret = s_selected_bezier.size() == 0 &&
                              s_selected_events.size() == 0 &&
                              s_selected_tempo.size() == 0 &&
                              s_selected_timesig.size() == 0 &&
                              selectedPointIDs.size() == 0;
                SelectedEventChanged( typeof( AppManager ), ret );
            }
        }

        public static boolean isOverlay() {
            return s_overlay;
        }

        public static void setOverlay( boolean value ) {
            s_overlay = value;
        }

        public static boolean getRenderRequired( int track ) {
            if ( s_vsq == null ) {
                return false;
            }
            return s_vsq.editorStatus.renderRequired[track - 1];
        }

        public static void setRenderRequired( int track, boolean value ) {
            if ( s_vsq == null ) {
                return;
            }
            s_vsq.editorStatus.renderRequired[track - 1] = value;
        }

        /// <summary>
        /// 現在の編集モード
        /// </summary>
        public static EditMode getEditMode() {
            return s_edit_mode;
        }

        public static void setEditMode( EditMode value ) {
            s_edit_mode = value;
        }

        /// <summary>
        /// グリッドを表示するか否かを表すフラグを取得または設定します
        /// </summary>
        public static boolean isGridVisible() {
            return s_grid_visible;
        }

        public static void setGridVisible( boolean value ) {
            if ( value != s_grid_visible ) {
                s_grid_visible = value;
                if ( GridVisibleChanged != null ) {
                    GridVisibleChanged( typeof( AppManager ), new BEventArgs() );
                }
            }
        }

        /// <summary>
        /// 現在のプレビューがリピートモードであるかどうかを表す値を取得または設定します
        /// </summary>
        public static boolean isRepeatMode() {
            return s_repeat_mode;
        }

        public static void setRepeatMode( boolean value ) {
            s_repeat_mode = value;
        }

        /// <summary>
        /// 現在プレビュー中かどうかを示す値を取得または設定します
        /// </summary>
        public static boolean isPlaying() {
            return s_playing;
        }

        public static void setPlaying( boolean value ) {
            boolean previous = s_playing;
            s_playing = value;
            if ( previous != s_playing ) {
                if ( s_playing && PreviewStarted != null ) {
                    int clock = getCurrentClock();
                    PreviewStarted( typeof( AppManager ), new BEventArgs() );
                } else if ( !s_playing && PreviewAborted != null ) {
                    PreviewAborted( typeof( AppManager ), new BEventArgs() );
                }
            }
        }

        /// <summary>
        /// _vsq_fileにセットされたvsqファイルの名前を取得します。
        /// </summary>
        public static String getFileName() {
            return s_file;
        }

        private static void saveToCor( String file ) {
            boolean hide = false;
            if ( s_vsq != null ) {
                String path = PortUtil.getDirectoryName( file );
                String file2 = PortUtil.combinePath( path, PortUtil.getFileNameWithoutExtension( file ) + ".vsq" );
                s_vsq.writeAsXml( file );
                s_vsq.write( file2 );
#if !JAVA
                if ( hide ) {
                    try {
                        System.IO.File.SetAttributes( file, System.IO.FileAttributes.Hidden );
                        System.IO.File.SetAttributes( file2, System.IO.FileAttributes.Hidden );
                    } catch ( Exception ex ) {
                    }
                }
#endif
            }
        }

        public static void saveTo( String file ) {
            saveToCor( file );
            if ( s_vsq != null ) {
                s_file = file;
                editorConfig.pushRecentFiles( s_file );
                if ( !s_auto_backup_timer.Enabled && editorConfig.AutoBackupIntervalMinutes > 0 ) {
                    double millisec = editorConfig.AutoBackupIntervalMinutes * 60.0 * 1000.0;
                    int draft = (int)millisec;
                    if ( millisec > int.MaxValue ) {
                        draft = int.MaxValue;
                    }
                    s_auto_backup_timer.Interval = draft;
                    s_auto_backup_timer.Start();
                }
            }
        }

        /// <summary>
        /// 現在の演奏マーカーの位置を取得または設定します。
        /// </summary>
        public static int getCurrentClock() {
            return s_current_clock;
        }

        public static void setCurrentClock( int value ) {
            int old = s_current_clock;
            s_current_clock = value;
            int barcount = s_vsq.getBarCountFromClock( s_current_clock );
            int bar_top_clock = s_vsq.getClockFromBarCount( barcount );
            //int numerator = 4;
            //int denominator = 4;
            Timesig timesig = s_vsq.getTimesigAt( s_current_clock );
            int clock_per_beat = 480 / 4 * timesig.denominator;
            int beat = (s_current_clock - bar_top_clock) / clock_per_beat;
            s_current_play_position.barCount = barcount - s_vsq.getPreMeasure() + 1;
            s_current_play_position.beat = beat + 1;
            s_current_play_position.clock = s_current_clock - bar_top_clock - clock_per_beat * beat;
            s_current_play_position.denominator = timesig.denominator;
            s_current_play_position.numerator = timesig.numerator;
            s_current_play_position.tempo = s_vsq.getTempoAt( s_current_clock );
            if ( old != s_current_clock && CurrentClockChanged != null ) {
                CurrentClockChanged( typeof( AppManager ), new BEventArgs() );
            }
        }

        /// <summary>
        /// 現在の演奏カーソルの位置(m_current_clockと意味は同じ。CurrentClockが変更されると、自動で更新される)
        /// </summary>
        public static PlayPositionSpecifier getPlayPosition() {
            return s_current_play_position;
        }

        /// <summary>
        /// 現在選択されているトラックを取得または設定します
        /// </summary>
        public static int getSelected() {
            int tracks = s_vsq.Track.size();
            if ( tracks <= s_selected ) {
                s_selected = tracks - 1;
            }
            return s_selected;
        }

        public static void setSelected( int value ) {
            s_selected = value;
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
            s_selected = 1;
            s_file = file;
            VsqFileEx newvsq = null;
            try {
                newvsq = VsqFileEx.readFromXml( file );
            } catch ( Exception ex ) {
#if DEBUG
                AppManager.debugWriteLine( "EditorManager.ReadVsq; ex=" + ex );
#endif
                return;
            }
            if ( newvsq == null ) {
                return;
            }
            s_vsq = newvsq;
            for ( int i = 0; i < s_vsq.editorStatus.renderRequired.Length; i++ ) {
                if ( i < s_vsq.Track.size() - 1 ) {
                    s_vsq.editorStatus.renderRequired[i] = true;
                } else {
                    s_vsq.editorStatus.renderRequired[i] = false;
                }
            }
            startMarker = s_vsq.getPreMeasureClocks();
            int bar = s_vsq.getPreMeasure() + 1;
            endMarker = s_vsq.getClockFromBarCount( bar );
            if ( s_vsq.Track.size() >= 1 ) {
                s_selected = 1;
            } else {
                s_selected = -1;
            }
            mainWindow.updateBgmMenuState();
        }

#if !TREECOM
        /// <summary>
        /// vsqファイル。
        /// </summary>
        public static VsqFileEx getVsqFile() {
            return s_vsq;
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
            s_vsq = vsq;
            for ( int i = 0; i < s_vsq.editorStatus.renderRequired.Length; i++ ) {
                if ( i < s_vsq.Track.size() - 1 ) {
                    s_vsq.editorStatus.renderRequired[i] = true;
                } else {
                    s_vsq.editorStatus.renderRequired[i] = false;
                }
            }
            s_file = "";
            startMarker = s_vsq.getPreMeasureClocks();
            int bar = s_vsq.getPreMeasure() + 1;
            endMarker = s_vsq.getClockFromBarCount( bar );
            s_auto_backup_timer.Stop();
            if ( mainWindow != null ) {
                mainWindow.updateBgmMenuState();
            }
        }

        public static void init() {
            loadConfig();
            VSTiProxy.init();
            s_locker = new Object();
            SymbolTable.loadDictionary();
            VSTiProxy.CurrentUser = "";

            #region Apply User Dictionary Configuration
            Vector<ValuePair<String, boolean>> current = new Vector<ValuePair<String, boolean>>();
            for ( int i = 0; i < SymbolTable.getCount(); i++ ) {
                current.add( new ValuePair<String, boolean>( SymbolTable.getSymbolTable( i ).getName(), false ) );
            }
            Vector<ValuePair<String, boolean>> config_data = new Vector<ValuePair<String, boolean>>();
            int count = editorConfig.UserDictionaries.size();
            for ( int i = 0; i < count; i++ ) {
                String[] spl = PortUtil.splitString( editorConfig.UserDictionaries.get( i ), new char[] { '\t' }, 2 );
                config_data.add( new ValuePair<String, boolean>( spl[0], (spl[1].Equals( "T" ) ? true : false) ) );
#if DEBUG
                AppManager.debugWriteLine( "    " + spl[0] + "," + spl[1] );
#endif
            }
            Vector<ValuePair<String, Boolean>> common = new Vector<ValuePair<String, Boolean>>();
            for ( int i = 0; i < config_data.size(); i++ ) {
                for ( int j = 0; j < current.size(); j++ ) {
                    if ( config_data.get( i ).Key.Equals( current.get( j ).Key ) ) {
                        current.get( j ).Value = true; //こっちのbooleanは、AppManager.EditorConfigのUserDictionariesにもKeyが含まれているかどうかを表すので注意
                        common.add( new ValuePair<String, Boolean>( config_data.get( i ).Key, config_data.get( i ).Value ) );
                        break;
                    }
                }
            }
            for ( int i = 0; i < current.size(); i++ ) {
                if ( !current.get( i ).Value ) {
                    common.add( new ValuePair<String, Boolean>( current.get( i ).Key, false ) );
                }
            }
            SymbolTable.changeOrder( common.toArray( new ValuePair<String, Boolean>[] { } ) );
            #endregion

            Messaging.loadMessages();
            Messaging.setLanguage( editorConfig.Language );

            KeySoundPlayer.Init();
#if ENABLE_SCRIPT
            PaletteToolServer.Init();
#endif

#if !TREECOM
            s_id = Misc.getmd5( DateTime.Now.ToBinary().ToString() ).Replace( "_", "" );
            String log = PortUtil.combinePath( getTempWaveDir(), "run.log" );
#endif

#if ENABLE_PROPERTY
            propertyPanel = new PropertyPanel();
            propertyWindow = new FormNoteProperty();
            propertyWindow.Controls.Add( propertyPanel );
            propertyPanel.Dock = DockStyle.Fill;
#endif
        }

        public static String getShortcutDisplayString( BKeys[] keys ) {
            String ret = "";
            Vector<BKeys> list = new Vector<BKeys>( keys );
            if ( list.contains( BKeys.Menu ) ) {
#if JAVA
                ret = new String( '\u2318' );
#else
                ret = new String( '\x2318', 1 );
#endif
            }
            if ( list.contains( BKeys.Control ) ) {
                ret += (ret.Equals( "" ) ? "" : "+") + "Ctrl";
            }
            if ( list.contains( BKeys.Shift ) ) {
                ret += (ret.Equals( "" ) ? "" : "+") + "Shift";
            }
            if ( list.contains( BKeys.Alt ) ) {
                ret += (ret.Equals( "" ) ? "" : "+") + "Alt";
            }
            Vector<BKeys> list2 = new Vector<BKeys>();
            foreach ( BKeys key in keys ) {
#if DEBUG
                AppManager.debugWriteLine( "    " + key );
#endif
                if ( key != BKeys.Control && key != BKeys.Shift && key != BKeys.Alt ) {
                    list2.add( key );
                }
            }
            Collections.sort( list2 );
            for ( int i = 0; i < list2.size(); i++ ) {
                ret += (ret.Equals( "" ) ? "" : "+") + getKeyDisplayString( list2.get( i ) );
            }
            return ret;
        }

        private static String getKeyDisplayString( BKeys key ) {
            if ( key.Equals( BKeys.PageDown ) ) {
                return "PgDn";
            } else if ( key.Equals( BKeys.PageUp ) ) {
                return "PgUp";
            } else if ( key.Equals( BKeys.D0 ) ) {
                return "0";
            } else if ( key.Equals( BKeys.D1 ) ) {
                return "1";
            } else if ( key.Equals( BKeys.D2 ) ) {
                return "2";
            } else if ( key.Equals( BKeys.D3 ) ) {
                return "3";
            } else if ( key.Equals( BKeys.D4 ) ) {
                return "4";
            } else if ( key.Equals( BKeys.D5 ) ) {
                return "5";
            } else if ( key.Equals( BKeys.D6 ) ) {
                return "6";
            } else if ( key.Equals( BKeys.D7 ) ) {
                return "7";
            } else if ( key.Equals( BKeys.D8 ) ) {
                return "8";
            } else if ( key.Equals( BKeys.D9 ) ) {
                return "9";
            } else if ( key.Equals( BKeys.Menu ) ) {
#if JAVA
                    return new String( '\u2318' );
#else
                return new String( '\x2318', 1 );
#endif
            } else {
                return key.ToString();
            }
        }


        #region クリップボードの管理
        /// <summary>
        /// オブジェクトをシリアライズし，クリップボードに格納するための文字列を作成します
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static String getSerializedText( Object obj ) {
            String str = "";
            ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
            ObjectOutputStream objectOutputStream = new ObjectOutputStream( outputStream );
            objectOutputStream.writeObject( obj );

            byte[] arr = outputStream.toByteArray();
            str = CLIP_PREFIX + ":" + obj.GetType().FullName + ":" + Base64.encode( arr );
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

        public static void clearClipBoard() {
            if ( Clipboard.ContainsText() ) {
                if ( Clipboard.GetText().StartsWith( CLIP_PREFIX ) ) {
                    Clipboard.Clear();
                }
            }
        }

        public static void setClipboard( ClipboardEntry item ) {
            String clip = getSerializedText( item );
            Clipboard.Clear();
            Clipboard.SetText( clip );
        }

        public static ClipboardEntry getCopiedItems() {
            ClipboardEntry ce = null;
            if ( Clipboard.ContainsText() ) {
                String clip = Clipboard.GetText();
                if ( clip.StartsWith( CLIP_PREFIX ) ) {
                    int index1 = clip.IndexOf( ":" );
                    int index2 = clip.IndexOf( ":", index1 + 1 );
                    String typename = clip.Substring( index1 + 1, index2 - index1 - 1 );
                    if ( typename.Equals( typeof( ClipboardEntry ).FullName ) ) {
                        try {
                            ce = (ClipboardEntry)getDeserializedObjectFromText( clip );
                        } catch ( Exception ex ) {
                        }
                    }
                }
            }
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

        public static void setCopiedEvent( Vector<VsqEvent> item, int copy_started_clock ) {
            ClipboardEntry ce = new ClipboardEntry();
            ce.events = item;
            ce.copyStartedClock = copy_started_clock;
            String clip = getSerializedText( ce );
            Clipboard.Clear();
            Clipboard.SetText( clip );
        }

        public static void setCopiedTempo( Vector<TempoTableEntry> item, int copy_started_clock ) {
            ClipboardEntry ce = new ClipboardEntry();
            ce.tempo = item;
            String clip = getSerializedText( ce );
            Clipboard.Clear();
            Clipboard.SetText( clip );
        }

        public static void setCopiedTimesig( Vector<TimeSigTableEntry> item, int copy_started_clock ) {
            ClipboardEntry ce = new ClipboardEntry();
            ce.timesig = item;
            String clip = getSerializedText( ce );
            Clipboard.Clear();
            Clipboard.SetText( clip );
        }

        public static void setCopiedCurve( TreeMap<CurveType, VsqBPList> item, int copy_started_clock ) {
            ClipboardEntry ce = new ClipboardEntry();
            ce.points = item;
            String clip = getSerializedText( ce );
            Clipboard.Clear();
            Clipboard.SetText( clip );
        }

        public static void setCopiedBezier( TreeMap<CurveType, Vector<BezierChain>> item, int copy_started_clock ) {
            ClipboardEntry ce = new ClipboardEntry();
            ce.beziers = item;
            String clip = getSerializedText( ce );
            Clipboard.Clear();
            Clipboard.SetText( clip );
        }
        #endregion

#if ENABLE_SCRIPT
        public static CompilerResults compileScript( String code ) {
#if DEBUG
            PortUtil.println( "AppManager#compileScript" );
#endif
            CompilerResults ret = null;
            CSharpCodeProvider provider = new CSharpCodeProvider();
            String path = Application.StartupPath;
            CompilerParameters parameters = new CompilerParameters( new String[] {
                PortUtil.combinePath( path, "Boare.Lib.Vsq.dll" ),
                PortUtil.combinePath( path, "Cadencii.exe" ),
                PortUtil.combinePath( path, "Boare.Lib.Media.dll" ),
                PortUtil.combinePath( path, "Boare.Lib.AppUtil.dll" ),
                PortUtil.combinePath( path, "bocoree.dll" ) } );
            parameters.ReferencedAssemblies.Add( "System.Windows.Forms.dll" );
            parameters.ReferencedAssemblies.Add( "System.dll" );
            parameters.ReferencedAssemblies.Add( "System.Drawing.dll" );
            parameters.ReferencedAssemblies.Add( "System.Xml.dll" );
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;
            parameters.IncludeDebugInformation = true;
            try {
                ret = provider.CompileAssemblyFromSource( parameters, code );
            } catch ( Exception ex ) {
#if DEBUG
                AppManager.debugWriteLine( "AppManager#compileScript; ex=" + ex );
#endif
            }
            return ret;
        }
#endif

        /// <summary>
        /// アプリケーションデータの保存位置を取得します
        /// Gets the path for application data
        /// </summary>
        public static String getApplicationDataPath() {
            String dir = PortUtil.combinePath( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Boare" );
            if ( !PortUtil.isDirectoryExists( dir ) ) {
                PortUtil.createDirectory( dir );
            }
            String dir2 = PortUtil.combinePath( dir, CONFIG_DIR_NAME );
            if ( !PortUtil.isDirectoryExists( dir2 ) ) {
                PortUtil.createDirectory( dir2 );
            }
            return dir2;
        }

        /// <summary>
        /// 位置クオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
        /// </summary>
        /// <returns></returns>
        public static int getPositionQuantizeClock() {
            return QuantizeModeUtil.getQuantizeClock( editorConfig.PositionQuantize, editorConfig.PositionQuantizeTriplet );
        }

        /// <summary>
        /// 音符長さクオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
        /// </summary>
        /// <returns></returns>
        public static int getLengthQuantizeClock() {
            return QuantizeModeUtil.getQuantizeClock( editorConfig.LengthQuantize, editorConfig.LengthQuantizeTriplet );
        }

        public static void saveConfig() {
            // ユーザー辞書の情報を取り込む
            editorConfig.UserDictionaries.clear();
            int count = SymbolTable.getCount();
            for ( int i = 0; i < count; i++ ) {
                SymbolTable table = SymbolTable.getSymbolTable( i );
                editorConfig.UserDictionaries.add( table.getName() + "\t" + (table.isEnabled() ? "T" : "F") );
            }
            editorConfig.KeyWidth = keyWidth;

            String file = PortUtil.combinePath( getApplicationDataPath(), CONFIG_FILE_NAME );
            try {
                EditorConfig.serialize( editorConfig, file );
            } catch ( Exception ex ) {
            }
        }

        public static void loadConfig() {
            String config_file = PortUtil.combinePath( getApplicationDataPath(), CONFIG_FILE_NAME );
            EditorConfig ret = null;
            if ( PortUtil.isFileExists( config_file ) ) {
                try {
                    ret = EditorConfig.deserialize( editorConfig, config_file );
                } catch ( Exception ex ) {
                    ret = null;
                }
            } else {
                config_file = PortUtil.combinePath( Application.StartupPath, CONFIG_FILE_NAME );
                if ( PortUtil.isFileExists( config_file ) ) {
                    try {
                        ret = EditorConfig.deserialize( editorConfig, config_file );
                    } catch ( Exception ex ) {
                        ret = null;
                    }
                }
            }
            if ( ret == null ) {
                ret = new EditorConfig();
            }
            editorConfig = ret;
            for ( int i = 0; i < SymbolTable.getCount(); i++ ) {
                SymbolTable st = SymbolTable.getSymbolTable( i );
                boolean found = false;
                for ( Iterator itr = editorConfig.UserDictionaries.iterator(); itr.hasNext(); ) {
                    String s = (String)itr.next();
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
            MidiPlayer.DeviceGeneral = (uint)editorConfig.MidiDeviceGeneral.PortNumber;
            MidiPlayer.DeviceMetronome = (uint)editorConfig.MidiDeviceMetronome.PortNumber;
            MidiPlayer.NoteBell = editorConfig.MidiNoteBell;
            MidiPlayer.NoteNormal = editorConfig.MidiNoteNormal;
            MidiPlayer.PreUtterance = editorConfig.MidiPreUtterance;
            MidiPlayer.ProgramBell = editorConfig.MidiProgramBell;
            MidiPlayer.ProgramNormal = editorConfig.MidiProgramNormal;
            MidiPlayer.RingBell = editorConfig.MidiRingBell;

            int draft_key_width = editorConfig.KeyWidth;
            if ( draft_key_width < MIN_KEY_WIDTH ) {
                draft_key_width = MIN_KEY_WIDTH;
            } else if ( MAX_KEY_WIDTH < draft_key_width ) {
                draft_key_width = MAX_KEY_WIDTH;
            }
            keyWidth = draft_key_width;
        }

        public static VsqID getSingerIDUtau( String name ) {
            VsqID ret = new VsqID( 0 );
            ret.type = VsqIDType.Singer;
            int index = -1;
            int c = editorConfig.UtauSingers.size();
            for ( int i = 0; i < c; i++ ) {
                if ( editorConfig.UtauSingers.get( i ).VOICENAME.Equals( name ) ) {
                    index = i;
                    break;
                }
            }
            if ( index >= 0 ) {
                SingerConfig sc = editorConfig.UtauSingers.get( index );
                int lang = 0;//utauは今のところ全部日本語
                ret.IconHandle = new IconHandle();
                ret.IconHandle.IconID = "$0701" + string.Format( "{0:x4}", index );
                ret.IconHandle.IDS = sc.VOICENAME;
                ret.IconHandle.Index = 0;
                ret.IconHandle.Language = lang;
                ret.IconHandle.setLength( 1 );
                ret.IconHandle.Original = sc.Original;
                ret.IconHandle.Program = sc.Program;
                ret.IconHandle.Caption = "";
                return ret;
            } else {
                ret.IconHandle = new IconHandle();
                ret.IconHandle.Program = 0;
                ret.IconHandle.Language = 0;
                ret.IconHandle.IconID = "$0701" + string.Format( "{0:x4}", (int)0 );
                ret.IconHandle.IDS = "Unknown";
                ret.type = VsqIDType.Singer;
                return ret;
            }
        }

        public static SingerConfig getSingerInfoUtau( int program_change ) {
            if ( 0 <= program_change && program_change < editorConfig.UtauSingers.size() ) {
                return editorConfig.UtauSingers.get( program_change );
            } else {
                return null;
            }
        }

        public static String getVersion() {
            String prefix = "";
            String rev = "";
            // $Id: AppManager.cs 474 2009-09-23 11:31:07Z kbinani $
            String id = getAssemblyConfigurationAttribute();
            String[] spl0 = PortUtil.splitString( id, new String[] { " " }, true );
            if ( spl0.Length >= 3 ) {
                String s = spl0[2];
#if DEBUG
                AppManager.debugWriteLine( "AppManager.get__VERSION; s=" + s );
#endif
                String[] spl = PortUtil.splitString( s, new String[] { " " }, true );
                if ( spl.Length > 0 ) {
                    rev = spl[0];
                }
            }
            if ( rev.Equals( "" ) ) {
                rev = "?";
            }
#if DEBUG
            prefix = "\n(rev: " + rev + "; build: debug)";
#else
            prefix = "\n(rev: " + rev + "; build: release)";
#endif
            return getAssemblyFileVersion( typeof( AppManager ) ) + " " + prefix;
        }

        public static String getAssemblyConfigurationAttribute() {
            Assembly a = Assembly.GetAssembly( typeof( AppManager ) );
            AssemblyConfigurationAttribute attr = (AssemblyConfigurationAttribute)Attribute.GetCustomAttribute( a, typeof( AssemblyConfigurationAttribute ) );
#if DEBUG
            AppManager.debugWriteLine( "GetAssemblyConfigurationAttribute; attr.Configuration=" + attr.Configuration );
#endif
            return attr.Configuration;
        }

#if !JAVA
        public static String getAssemblyFileVersion( Type t ) {
            Assembly a = Assembly.GetAssembly( t );
            AssemblyFileVersionAttribute afva = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute( a, typeof( AssemblyFileVersionAttribute ) );
            return afva.Version;
        }
#endif

#if !JAVA
        public static String getAssemblyNameAndFileVersion( Type t ) {
            Assembly a = Assembly.GetAssembly( t );
            AssemblyFileVersionAttribute afva = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute( a, typeof( AssemblyFileVersionAttribute ) );
            return a.GetName().Name + " v" + afva.Version;
        }
#endif

        public static java.awt.Color getHilightColor() {
            return s_hilight_brush;
        }

        public static void setHilightColor( java.awt.Color value ) {
            s_hilight_brush = value;
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
