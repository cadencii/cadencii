/*
 * TrackSelector.cs
 * Copyright (C) 2008-2010 kbinani
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

//INCLUDE-SECTION IMPORT ..\BuildJavaUI\src\org\kbinani\Cadencii\TrackSelector.java

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
using System;
using System.Threading;
using org.kbinani.apputil;
using org.kbinani.vsq;
using org.kbinani;
using org.kbinani.java.awt;
using org.kbinani.java.awt.event_;
using org.kbinani.java.util;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using BEventArgs = System.EventArgs;
    using BKeyEventArgs = System.Windows.Forms.KeyEventArgs;
    using BMouseEventArgs = System.Windows.Forms.MouseEventArgs;
    using boolean = System.Boolean;
    using Graphics = org.kbinani.java.awt.Graphics2D;
    using Integer = System.Int32;
    using java = org.kbinani.java;
    using Long = System.Int64;
    using BMouseButtons = System.Windows.Forms.MouseButtons;
    using Float = System.Single;
#endif

#if JAVA
    public class TrackSelector extends BPanel {
#else
    public class TrackSelector : BPanel {
#endif
        #region Constants and internal enums
        private enum MouseDownMode {
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

        private static readonly Color DOT_COLOR_MASTER = Color.red;
        private static readonly Color CURVE_COLOR = PortUtil.Navy;
        private static readonly Color CONROL_LINE = Color.orange;
        private static readonly Color DOT_COLOR_NORMAL = new Color( 237, 107, 158 );
        private static readonly Color DOT_COLOR_BASE = new Color( 125, 198, 34 );
        private static readonly Color DOT_COLOR_NORMAL_AROUND = new Color( 231, 50, 122 );
        private static readonly Color DOT_COLOR_BASE_AROUND = new Color( 90, 143, 24 );
        private static readonly Color CURVE_COLOR_DOT = PortUtil.Coral;
        private static readonly Color BRS_A244_255_023_012 = new Color( 255, 23, 12, 244 );
        private static readonly Color BRS_A144_255_255_255 = new Color( 255, 255, 255, 144 );
        private static readonly Color s_brs_a072_255_255_255 = new Color( 255, 255, 255, 72 );
        private static readonly Color s_brs_a127_008_166_172 = new Color( 8, 166, 172, 127 );
        private static readonly Color s_brs_a098_000_000_000 = new Color( 0, 0, 0, 98 );
        //private static readonly Color s_brs_dot_master = new SolidBrush( DOT_COLOR_MASTER );
        //private static readonly Color s_brs_dot_normal = new SolidBrush( DOT_COLOR_NORMAL );
        //private static readonly Color s_brs_dot_base = new SolidBrush( DOT_COLOR_BASE );
        //private static readonly Color BRS_CURVE_COLOR_DOT = new SolidBrush( CURVE_COLOR_DOT );
        //private static readonly Pen s_pen_dot_normal = new Pen( DOT_COLOR_NORMAL_AROUND );
        //private static readonly Pen s_pen_dot_base = new Pen( DOT_COLOR_BASE_AROUND );
        private static readonly Color s_pen_050_140_150 = new Color( 50, 140, 150 );
        private static readonly Color s_pen_128_128_128 = new Color( 128, 128, 128 );
        private static readonly Color s_pen_246_251_010 = new Color( 246, 251, 10 );
        //private static readonly Pen s_pen_curve = new Pen( CURVE_COLOR );
        //private static readonly Pen s_pen_control_line = new Pen( CONROL_LINE );
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
        public const int HEADER = 8;
        const int BUF_LEN = 512;
        /// <summary>
        /// 歌手変更イベントの表示矩形の幅
        /// </summary>
        const int SINGER_ITEM_WIDTH = 66;
        /// <summary>
        /// RENDERボタンの幅(px)
        /// </summary>
        const int _PX_WIDTH_RENDER = 10;
        /// <summary>
        /// カーブ制御点の幅（実際は_DOT_WID * 2 + 1ピクセルで描画される）
        /// </summary>
        const int DOT_WID = 3;
        static readonly Rectangle PLUS_MARK = new Rectangle( 7, 38, 23, 21 );
        static readonly Rectangle MINUS_MARK = new Rectangle( 40, 38, 23, 21 );
        const int VSCROLL_WIDTH = 16;
        const int ZOOMPANEL_HEIGHT = 33;
        /// <summary>
        /// カーブの種類を表す部分の，1個あたりの高さ（ピクセル，余白を含む）
        /// TrackSelectorの推奨表示高さは，HEIGHT_WITHOUT_CURVE + UNIT_HEIGHT_PER_CURVE * (カーブの個数)となる
        /// </summary>
        const int UNIT_HEIGHT_PER_CURVE = 18;
        /// <summary>
        /// カーブの種類を除いた部分の高さ（ピクセル）．
        /// TrackSelectorの推奨表示高さは，HEIGHT_WITHOUT_CURVE + UNIT_HEIGHT_PER_CURVE * (カーブの個数)となる
        /// </summary>
        const int HEIGHT_WITHOUT_CURVE = 76;
        /// <summary>
        /// トラックの名前表示部分の最大表示幅（ピクセル）
        /// </summary>
        const int TRACK_SELECTOR_MAX_WIDTH = 80;
        #endregion

        private CurveType m_selected_curve = CurveType.VEL;
        private CurveType m_last_selected_curve = CurveType.DYN;
        private boolean m_curve_visible = true;
        /// <summary>
        /// 現在のマウス位置におけるカーブの値
        /// </summary>
        private int m_mouse_value;
        private int[] m_pointsx = new int[BUF_LEN];
        private int[] m_pointsy = new int[BUF_LEN];
        /// <summary>
        /// 編集しているBezierChainのID
        /// </summary>
        public int EditingChainID = -1;
        /// <summary>
        /// 編集しているBezierPointのID
        /// </summary>
        public int EditingPointID = -1;
#if MONITOR_FPS
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
#endif
        /// <summary>
        /// ラインツールが降りた位置。x座標はクロック、y座標はカーブのコントロール値
        /// </summary>
        private Point m_line_start;
        /// <summary>
        /// マウスがカーブ部分に下ろされている最中かどうかを表すフラグ
        /// </summary>
        private boolean m_mouse_downed = false;
        /// <summary>
        /// マウスのトレース。
        /// </summary>
        private TreeMap<Integer, Integer> m_mouse_trace;
        /// <summary>
        /// マウスのトレース時、前回リストに追加されたx座標の値
        /// </summary>
        private int m_mouse_trace_last_x;
        private int m_mouse_trace_last_y;
        private boolean m_pencil_moved = false;
        private Thread m_mouse_hover_thread = null;
        /// <summary>
        /// cmenuSingerのメニューアイテムを初期化するのに使用したRenderer。
        /// </summary>
        private RendererKind m_cmenu_singer_prepared = RendererKind.NULL;
        /// <summary>
        /// マウスがDownしてからUpするまでのモード
        /// </summary>
        private MouseDownMode m_mouse_down_mode = MouseDownMode.NONE;
        /// <summary>
        /// マウスがDownしてからマウスが移動したかどうかを表す。
        /// </summary>
        private boolean m_mouse_moved = false;
        /// <summary>
        /// マウスドラッグで歌手変更イベントの矩形を移動開始した時の、マウス位置におけるクロック
        /// </summary>
        private int m_singer_move_started_clock;
        /// <summary>
        /// cmenuSinger用のツールチップの幅を記録しておく。
        /// </summary>
        private int[] m_cmenusinger_tooltip_width;
        /// <summary>
        /// マウス長押しによるVELの編集。選択されている音符のInternalID
        /// </summary>
        private int m_veledit_last_selectedid = -1;
        /// <summary>
        /// マウス長押しによるVELの編集。棒グラフのてっぺんの座標と、マウスが降りた座標の差分。プラスなら、マウスの方が下になる。
        /// </summary>
        private int m_veledit_shifty = 0;
        /// <summary>
        /// マウス長押しによるVELの編集。編集対象の音符のリスト。
        /// </summary>
        private TreeMap<Integer, SelectedEventEntry> m_veledit_selected = new TreeMap<Integer, SelectedEventEntry>();
        /// <summary>
        /// 現在編集操作が行われているBezierChainの、編集直前のオリジナル
        /// </summary>
        private BezierChain m_editing_bezier_original = null;
        /// <summary>
        /// CTRLキー。MacOSXの場合はMenu
        /// </summary>
        private int m_modifier_key = InputEvent.CTRL_MASK;
        /// <summary>
        /// このコントロールが表示を担当しているカーブのリスト
        /// </summary>
        private Vector<CurveType> m_viewing_curves = new Vector<CurveType>();
        private Color m_generic_line = new Color( 118, 123, 138 );
        /// <summary>
        /// スペースキーが押されているかどうか。
        /// MouseDown時に範囲選択モードをスキップする必要があるので、FormMainでの処理に加えてこのクラス内部でも処理する必要がある
        /// </summary>
        private boolean m_spacekey_downed = false;
        /// <summary>
        /// マウスがDownした位置の座標．xは仮想スクリーン座標．yは通常のe.Location.Y
        /// </summary>
        private Point m_mouse_down_location;
        /// <summary>
        /// エンベロープ点を動かすモードで，選択されているInternalID．
        /// </summary>
        private int m_envelope_move_id = -1;
        /// <summary>
        /// エンベロープ点を動かすモードで，選択されている点のタイプ
        /// </summary>
        private int m_envelope_point_kind = -1;
        /// <summary>
        /// エンベロープ点を動かすモードで，編集される前のオリジナルのエンベロープ
        /// </summary>
        private UstEnvelope m_envelope_original = null;
        /// <summary>
        /// エンベロープ点を動かすモードで、点が移動可能な範囲の始点(秒)
        /// </summary>
        private double m_envelope_dot_begin;
        /// <summary>
        /// エンベロープ点を動かすモードで、点が移動可能な範囲の終点(秒)
        /// </summary>
        private double m_envelope_dot_end;
        /// <summary>
        /// 編集中のエンベロープ
        /// </summary>
        private UstEnvelope m_envelope_editing = null;
        /// <summary>
        /// 編集中のエンベロープの範囲の始点（秒）
        /// </summary>
        private double m_envelope_range_begin;
        /// <summary>
        /// 編集中のエンベロープの範囲の終点（秒）
        /// </summary>
        private double m_envelope_range_end;

        /// <summary>
        /// 現在表示されているPreUtterance編集用の旗のID．負なら表示されていない
        /// </summary>
        private int m_preutterance_viewing = -1;
        /// <summary>
        /// PreUtterance編集用の旗の現在位置
        /// </summary>
        private Rectangle m_preutterance_bounds;
        /// <summary>
        /// 現在PreUtteranceを編集中のVsqEventのID
        /// </summary>
        private int m_preutterance_moving_id;

        /// <summary>
        /// 現在表意されているオーバーラップ編集用の旗のID．負なら表示されていない
        /// </summary>
        private int m_overlap_viewing = -1;
        /// <summary>
        /// オーバーラップ編集用の旗の現在位置
        /// </summary>
        private Rectangle m_overlap_bounds;
        /// <summary>
        /// 現在Overlapを編集中のVsqEventのID
        /// </summary>
        private int m_overlap_moving_id;
        private VsqEvent m_pre_ovl_original = null;
        private VsqEvent m_pre_ovl_editing = null;

        /// <summary>
        /// MouseDown時のControl.Modifiersの状態。
        /// </summary>
        private int m_modifier_on_mouse_down = 0;
        /// <summary>
        /// 移動しているデータ点のリスト
        /// </summary>
        private Vector<BPPair> m_moving_points = new Vector<BPPair>();
        private int m_last_preferred_min_height;
        private PolylineDrawer drawer = new PolylineDrawer( null, 1024 );
#if JAVA
        public BEvent<SelectedCurveChangedEventHandler> selectedCurveChangedEvent = new BEvent<SelectedCurveChangedEventHandler>();
        public BEvent<SelectedTrackChangedEventHandler> selectedTrackChangedEvent = new BEvent<SelectedTrackChangedEventHandler>();
        public BEvent<BEventHandler> commandExecutedEvent = new BEvent<BEventHandler>();
        public BEvent<RenderRequiredEventHandler> renderRequiredEvent = new BEvent<RenderRequiredEventHandler>();
        public BEvent<BEventHandler> preferredMinHeightChangedEvent = new BEvent<BEventHandler>();
#else
        public event SelectedCurveChangedEventHandler SelectedCurveChanged;
        public event SelectedTrackChangedEventHandler SelectedTrackChanged;
        public event EventHandler CommandExecuted;
        public event RenderRequiredEventHandler RenderRequired;
        /// <summary>
        /// このコントロールの推奨表示高さが変わったとき発生します
        /// </summary>
        public event EventHandler PreferredMinHeightChanged;
#endif

        public TrackSelector() {
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
            registerEventHandlers();
            setResources();
            m_modifier_key = (AppManager.editorConfig.Platform == PlatformEnum.Macintosh) ? InputEvent.META_MASK : InputEvent.CTRL_MASK;
            cmenuCurveVelocity.setTag( CurveType.VEL );
            cmenuCurveAccent.setTag( CurveType.Accent );
            cmenuCurveDecay.setTag( CurveType.Decay );

            cmenuCurveDynamics.setTag( CurveType.DYN );
            cmenuCurveVibratoRate.setTag( CurveType.VibratoRate );
            cmenuCurveVibratoDepth.setTag( CurveType.VibratoDepth );

            cmenuCurveReso1Amp.setTag( CurveType.reso1amp );
            cmenuCurveReso1BW.setTag( CurveType.reso1bw );
            cmenuCurveReso1Freq.setTag( CurveType.reso1freq );
            cmenuCurveReso2Amp.setTag( CurveType.reso2amp );
            cmenuCurveReso2BW.setTag( CurveType.reso2bw );
            cmenuCurveReso2Freq.setTag( CurveType.reso2freq );
            cmenuCurveReso3Amp.setTag( CurveType.reso3amp );
            cmenuCurveReso3BW.setTag( CurveType.reso3bw );
            cmenuCurveReso3Freq.setTag( CurveType.reso3freq );
            cmenuCurveReso4Amp.setTag( CurveType.reso4amp );
            cmenuCurveReso4BW.setTag( CurveType.reso4bw );
            cmenuCurveReso4Freq.setTag( CurveType.reso4freq );

            cmenuCurveHarmonics.setTag( CurveType.harmonics );
            cmenuCurveBreathiness.setTag( CurveType.BRE );
            cmenuCurveBrightness.setTag( CurveType.BRI );
            cmenuCurveClearness.setTag( CurveType.CLE );
            cmenuCurveOpening.setTag( CurveType.OPE );
            cmenuCurveGenderFactor.setTag( CurveType.GEN );

            cmenuCurvePortamentoTiming.setTag( CurveType.POR );
            cmenuCurvePitchBend.setTag( CurveType.PIT );
            cmenuCurvePitchBendSensitivity.setTag( CurveType.PBS );

            cmenuCurveEffect2Depth.setTag( CurveType.fx2depth );
            cmenuCurveEnvelope.setTag( CurveType.Env );
        }

#if !JAVA
        #region java.awt.Component
        // root implementation of java.awt.Component is in BForm.cs
        public void invalidate() {
            base.Invalidate();
        }

        public void repaint() {
            base.Refresh();
        }

        public void setBounds( int x, int y, int width, int height ) {
            base.Bounds = new System.Drawing.Rectangle( x, y, width, height );
        }

        public void setBounds( org.kbinani.java.awt.Rectangle rc ) {
            base.Bounds = new System.Drawing.Rectangle( rc.x, rc.y, rc.width, rc.height );
        }

        public org.kbinani.java.awt.Cursor getCursor() {
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

        public void setCursor( org.kbinani.java.awt.Cursor value ) {
            base.Cursor = value.cursor;
        }

        public bool isVisible() {
            return base.Visible;
        }

        public void setVisible( bool value ) {
            base.Visible = value;
        }

#if COMPONENT_ENABLE_TOOL_TIP_TEXT
        public void setToolTipText( string value )
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
        public object getParent() {
            return base.Parent;
        }
#endif

        public string getName() {
            return base.Name;
        }

        public void setName( string value ) {
            base.Name = value;
        }

#if COMPONENT_ENABLE_LOCATION
        public org.kbinani.java.awt.Point getLocationOnScreen() {
            System.Drawing.Point p = base.PointToScreen( base.Location );
            return new org.kbinani.java.awt.Point( p.X, p.Y );
        }

        public org.kbinani.java.awt.Point getLocation() {
            System.Drawing.Point loc = this.Location;
            return new org.kbinani.java.awt.Point( loc.X, loc.Y );
        }

        public void setLocation( int x, int y ) {
            base.Location = new System.Drawing.Point( x, y );
        }

        public void setLocation( org.kbinani.java.awt.Point p ) {
            base.Location = new System.Drawing.Point( p.x, p.y );
        }
#endif

        public org.kbinani.java.awt.Rectangle getBounds() {
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

        public int getWidth() {
            return base.Width;
        }

        public int getHeight() {
            return base.Height;
        }

        public org.kbinani.java.awt.Dimension getSize() {
            return new org.kbinani.java.awt.Dimension( base.Size.Width, base.Size.Height );
        }

        public void setSize( int width, int height ) {
            base.Size = new System.Drawing.Size( width, height );
        }

        public void setSize( org.kbinani.java.awt.Dimension d ) {
            setSize( d.width, d.height );
        }

        public void setBackground( org.kbinani.java.awt.Color color ) {
            base.BackColor = System.Drawing.Color.FromArgb( color.getRed(), color.getGreen(), color.getBlue() );
        }

        public org.kbinani.java.awt.Color getBackground() {
            return new org.kbinani.java.awt.Color( base.BackColor.R, base.BackColor.G, base.BackColor.B );
        }

        public void setForeground( org.kbinani.java.awt.Color color ) {
            base.ForeColor = color.color;
        }

        public org.kbinani.java.awt.Color getForeground() {
            return new org.kbinani.java.awt.Color( base.ForeColor.R, base.ForeColor.G, base.ForeColor.B );
        }

        public bool isEnabled() {
            return base.Enabled;
        }

        public void setEnabled( bool value ) {
            base.Enabled = value;
        }

        public void requestFocus() {
            base.Focus();
        }

        public bool isFocusOwner() {
            return base.Focused;
        }

        public void setPreferredSize( org.kbinani.java.awt.Dimension size ) {
            base.Size = new System.Drawing.Size( size.width, size.height );
        }

        public org.kbinani.java.awt.Font getFont() {
            return new org.kbinani.java.awt.Font( base.Font );
        }

        public void setFont( org.kbinani.java.awt.Font font ) {
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
        public java.awt.Point pointToScreen( java.awt.Point point_on_client ) {
            java.awt.Point p = getLocationOnScreen();
            return new java.awt.Point( p.x + point_on_client.x, p.y + point_on_client.y );
        }

        public java.awt.Point pointToClient( java.awt.Point point_on_screen ) {
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
        public Object getTag() {
            return base.Tag;
        }

        public void setTag( Object value ) {
            base.Tag = value;
        }
#endif
        #endregion

#if !JAVA
        protected override void OnResize( BEventArgs e ) {
            base.OnResize( e );
            vScroll.Width = VSCROLL_WIDTH;
            vScroll.Height = getHeight() - ZOOMPANEL_HEIGHT - 2;
            vScroll.Left = getWidth() - VSCROLL_WIDTH;
            vScroll.Top = 0;

            panelZoomButton.Width = VSCROLL_WIDTH;
            panelZoomButton.Height = ZOOMPANEL_HEIGHT;
            panelZoomButton.Left = getWidth() - VSCROLL_WIDTH;
            panelZoomButton.Top = getHeight() - ZOOMPANEL_HEIGHT - 2;
        }
#endif

        public void applyLanguage() {
        }

        public void applyFont( java.awt.Font font ) {
            Util.applyFontRecurse( this, font );
            Util.applyContextMenuFontRecurse( cmenuSinger, font );
            Util.applyContextMenuFontRecurse( cmenuCurve, font );
        }

        private int getMaxColumns() {
            int max_columns = AppManager.keyWidth / AppManager.MIN_KEY_WIDTH;
            if ( max_columns < 1 ) {
                max_columns = 1;
            }
            return max_columns;
        }

        public int getRowsPerColumn() {
            int max_columns = getMaxColumns();
            int row_per_column = m_viewing_curves.size() / max_columns;
            if ( row_per_column * max_columns < m_viewing_curves.size() ) {
                row_per_column++;
            }
            return row_per_column;
        }

        /// <summary>
        /// このコントロールの推奨最小表示高さを取得します
        /// </summary>
        public int getPreferredMinSize() {
            return HEIGHT_WITHOUT_CURVE + UNIT_HEIGHT_PER_CURVE * getRowsPerColumn();
        }

        /// <summary>
        /// このコントロールに担当させるカーブを追加します
        /// </summary>
        /// <param name="curve"></param>
        public void addViewingCurve( CurveType curve ) {
            addViewingCurve( new CurveType[] { curve } );
        }

        public void addViewingCurve( CurveType[] curve ) {
            for ( int j = 0; j < curve.Length; j++ ) {
                boolean found = false;
                for ( int i = 0; i < m_viewing_curves.size(); i++ ) {
                    if ( m_viewing_curves.get( i ).equals( curve[j] ) ) {
                        found = true;
                        break;
                    }
                }
                if ( !found ) {
                    m_viewing_curves.add( curve[j] );
                }
            }
            if ( m_viewing_curves.size() >= 2 ) {
                boolean changed = true;
                while ( changed ) {
                    changed = false;
                    for ( int i = 0; i < m_viewing_curves.size() - 1; i++ ) {
                        if ( m_viewing_curves.get( i ).getIndex() > m_viewing_curves.get( i + 1 ).getIndex() ) {
                            CurveType b = m_viewing_curves.get( i );
                            m_viewing_curves.set( i, m_viewing_curves.get( i + 1 ) );
                            m_viewing_curves.set( i + 1, b );
                            changed = true;
                        }
                    }
                }
            }
        }

        public void clearViewingCurve() {
            m_viewing_curves.clear();
        }

        /// <summary>
        /// このコントロールに担当させるカーブを削除します
        /// </summary>
        /// <param name="curve"></param>
        public void removeViewingCurve( CurveType curve ) {
            for ( int i = 0; i < m_viewing_curves.size(); i++ ) {
                if ( m_viewing_curves.get( i ).equals( curve ) ) {
                    m_viewing_curves.removeElementAt( i );
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="register">Undo/Redo用バッファにExecuteの結果を格納するかどうかを指定するフラグ</param>
        private void executeCommand( CadenciiCommand command, boolean register ) {
            if ( register ) {
                AppManager.register( AppManager.getVsqFile().executeCommand( command ) );
            } else {
                AppManager.getVsqFile().executeCommand( command );
            }
#if JAVA
            try{
                commandExecutedEvent.raise( this, new BEventArgs() );
            }catch( Exception ex ){
                System.err.println( "TrackSelector#executeCommand; ex=" + ex );
            }
#else
            if ( CommandExecuted != null ) {
                CommandExecuted( this, new BEventArgs() );
            }
#endif
        }

        public ValuePair<Integer, Integer> getSelectedRegion() {
            int x0 = AppManager.curveSelectedInterval.getStart();
            int x1 = AppManager.curveSelectedInterval.getEnd();
            int min = Math.Min( x0, x1 );
            int max = Math.Max( x0, x1 );
            return new ValuePair<Integer, Integer>( min, max );
        }

        /// <summary>
        /// 現在最前面に表示され，編集可能となっているカーブの種類を取得または設定します
        /// </summary>
        public CurveType getSelectedCurve() {
            return m_selected_curve;
        }

        public void setSelectedCurve( CurveType value ) {
            CurveType old = m_selected_curve;
            m_selected_curve = value;
            if ( !old.equals( m_selected_curve ) ) {
                m_last_selected_curve = old;
#if JAVA
                try{
                    selectedCurveChangedEvent.raise( this, m_selected_curve );
                }catch( Exception ex ){
                    System.err.println( "TrackSelector#setSelectedCurve; ex=" + ex );
                }
#else
                if ( SelectedCurveChanged != null ) {
                    SelectedCurveChanged( this, m_selected_curve );
                }
#endif
            }
        }

        /// <summary>
        /// エディタのy方向の位置から，カーブの値を求めます
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public int valueFromYCoord( int y ) {
            int max = m_selected_curve.getMaximum();
            int min = m_selected_curve.getMinimum();
            return valueFromYCoord( y, max, min );
        }

        public int valueFromYCoord( int y, int max, int min ) {
            int oy = getHeight() - 42;
            float order = getGraphHeight() / (float)(max - min);
            return (int)((oy - y) / order) + min;
        }

        public int yCoordFromValue( int value ) {
            int max = m_selected_curve.getMaximum();
            int min = m_selected_curve.getMinimum();
            return yCoordFromValue( value, max, min );
        }

        public int yCoordFromValue( int value, int max, int min ) {
            int oy = getHeight() - 42;
            float order = getGraphHeight() / (float)(max - min);
            return oy - (int)((value - min) * order);
        }

        /// <summary>
        /// カーブエディタを表示するかどうかを取得または設定します
        /// </summary>
        public boolean isCurveVisible() {
            return m_curve_visible;
        }

        public void setCurveVisible( boolean value ) {
            m_curve_visible = value;
        }

#if !JAVA
        protected override void OnPaint( System.Windows.Forms.PaintEventArgs e ) {
            paint( new Graphics2D( e.Graphics ) );
#if MONITOR_FPS
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
            e.Graphics.drawString(
                m_fps.ToString( "000.000" ),
                new Font( "Verdana", 40, FontStyle.Bold ),
                Brushes.Red,
                new PointF( 0, 0 ) );
#endif
        }
#endif

        /// <summary>
        /// x軸方向の表示倍率。pixel/clock
        /// </summary>
        public float getScaleY() {
            int max = m_selected_curve.getMaximum();
            int min = m_selected_curve.getMinimum();
            int oy = getHeight() - 42;
            return getGraphHeight() / (float)(max - min);
        }

        public Rectangle getRectFromCurveType( CurveType curve ) {
            int row_per_column = getRowsPerColumn();

            int centre = 17 + getGraphHeight() / 2 + 8;
            int index = 100;
            for ( int i = 0; i < m_viewing_curves.size(); i++ ) {
                if ( m_viewing_curves.get( i ).equals( curve ) ) {
                    index = i;
                    break;
                }
            }
            int ix = index / row_per_column;
            int iy = index - ix * row_per_column;
            int x = 7 + ix * AppManager.MIN_KEY_WIDTH;
            int y = centre - row_per_column * UNIT_HEIGHT_PER_CURVE / 2 + 2 + UNIT_HEIGHT_PER_CURVE * iy;
            int min_size = getPreferredMinSize();
            if ( m_last_preferred_min_height != min_size ){
#if JAVA
                try{
                    preferredMinHeightChangedEvent.raise( this, new BEventArgs() );
                }catch( Exception ex ){
                    System.err.println( "TrackSelector#getRectFromCurveType; ex=" + ex );
                }
#else
                if ( PreferredMinHeightChanged != null ) {
                    PreferredMinHeightChanged( this, new BEventArgs() );
                }
#endif
                m_last_preferred_min_height = min_size;
            }
            return new Rectangle( x, y, 56, 14 );
        }

        public void paint( Graphics graphics ) {
            Dimension size = new Dimension( getWidth() - vScroll.getWidth() + 2, getHeight() );
            Graphics2D g = (Graphics2D)graphics;
            Color brs_string = Color.black;
            Color rect_curve = new Color( 41, 46, 55 );
            int centre = 8 + getGraphHeight() / 2;
            g.setColor( Color.darkGray );
            g.fillRect( 0, size.height - 2 * OFFSET_TRACK_TAB, size.width, 2 * OFFSET_TRACK_TAB );
            int numeric_view = m_mouse_value;
            Point p = pointToClient( PortUtil.getMousePosition() );
            Point mouse = new Point( p.x, p.y );
            VsqFileEx vsq = AppManager.getVsqFile();
            int selected = AppManager.getSelected();
            int key_width = AppManager.keyWidth;
            int stdx = AppManager.startToDrawX;
            int graphHeight = getGraphHeight();
            int width = getWidth();

            try {
                #region SINGER
                Shape last = g.getClip();
                g.setColor( m_generic_line );
                g.drawLine( 2, size.height - 2 * OFFSET_TRACK_TAB,
                            size.width - 3, size.height - 2 * OFFSET_TRACK_TAB );
                g.drawLine( key_width, size.height - 2 * OFFSET_TRACK_TAB + 1,
                            key_width, size.height - 2 * OFFSET_TRACK_TAB + 15 );
                g.setFont( AppManager.baseFont8 );
                g.setColor( brs_string );
                g.drawString( "SINGER", 9, size.height - 2 * OFFSET_TRACK_TAB + OFFSET_TRACK_TAB / 2 - AppManager.baseFont8OffsetHeight );
                g.clipRect( key_width, size.height - 2 * OFFSET_TRACK_TAB,
                            size.width - key_width, OFFSET_TRACK_TAB );
                VsqTrack vsq_track = null;
                if ( vsq != null ) {
                    vsq_track = vsq.Track.get( selected );
                }
                if ( vsq_track != null ) {
                    int event_count = vsq_track.getEventCount();
                    for ( int i = 0; i < event_count; i++ ) {
                        VsqEvent ve = vsq_track.getEvent( i );
                        if ( ve.ID.type == VsqIDType.Singer ) {
                            int clock = ve.Clock;
                            IconHandle singer_handle = (IconHandle)ve.ID.IconHandle;
                            int x = AppManager.xCoordFromClocks( clock );
                            Rectangle rc = new Rectangle( x, size.height - 2 * OFFSET_TRACK_TAB + 1, SINGER_ITEM_WIDTH, OFFSET_TRACK_TAB - 5 );
                            g.setColor( Color.white );
                            g.fillRect( rc.x, rc.y, rc.width, rc.height );
                            if ( AppManager.isSelectedEventContains( selected, ve.InternalID ) ) {
                                g.setColor( AppManager.getHilightColor() );
                                g.drawRect( rc.x, rc.y, rc.width, rc.height );
                                g.setColor( brs_string );
                                g.drawString( singer_handle.IDS, rc.x, rc.y ); // sf );
                            } else {
                                g.setColor( new Color( 182, 182, 182 ) );
                                g.drawRect( rc.x, rc.y, rc.width, rc.height );
                                g.setColor( brs_string );
                                g.drawString( singer_handle.IDS, rc.x, rc.y ); // sf );
                            }
                        }
                    }
                }
                g.setClip( last );
                #endregion

                #region トラック選択欄
                int selecter_width = getSelectorWidth();
                g.setColor( m_generic_line );
                g.drawLine( 1, size.height - OFFSET_TRACK_TAB,
                            size.width - 2, size.height - OFFSET_TRACK_TAB );
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
                if ( m_curve_visible ) {
                    #region カーブエディタ
                    // カーブエディタの下の線
                    g.setColor( new Color( 156, 161, 169 ) );
                    g.drawLine( key_width, size.height - 42,
                                size.width - 3, size.height - 42 );

                    // カーブエディタの上の線
                    g.setColor( new Color( 46, 47, 50 ) );
                    g.drawLine( key_width, 8,
                                size.width - 3, 8 );

                    g.setColor( new Color( 125, 123, 124 ) );
                    g.drawLine( key_width, 0,
                                key_width, size.height );

                    if ( AppManager.isCurveSelectedIntervalEnabled() ) {
                        int x0 = AppManager.xCoordFromClocks( AppManager.curveSelectedInterval.getStart() );
                        int x1 = AppManager.xCoordFromClocks( AppManager.curveSelectedInterval.getEnd() );
                        g.setColor( s_brs_a072_255_255_255 );
                        g.fillRect( x0, HEADER, x1 - x0, getGraphHeight() );
                    }

                    #region 音符の境界
                    if ( AppManager.drawObjects != null && selected - 1 < AppManager.drawObjects.size() ) {
                        if ( AppManager.drawItemBorderInControlCurveView && 
                             !m_selected_curve.equals( CurveType.VibratoDepth ) &&
                             !m_selected_curve.equals( CurveType.VibratoRate ) ) {
                            lock ( AppManager.drawObjects ) {
                                Vector<DrawObject> objs = AppManager.drawObjects.get( selected - 1 );
                                int start = AppManager.drawStartIndex[selected - 1];
                                int count = objs.size();
                                Color line = new Color( 0, 0, 0, 128 );
                                Color fill = new Color( 0, 0, 0, 32 );
                                for ( int i = start; i < count; i++ ) {
                                    DrawObject obj = objs.get( i );
                                    int x0 = obj.pxRectangle.x + key_width - stdx;
                                    int w = obj.pxRectangle.width;
                                    int x1 = x0 + w;
                                    if ( width < x0 ) {
                                        break;
                                    }
                                    g.setColor( fill );
                                    g.fillRect( x0, HEADER, w, graphHeight );
                                    g.setColor( line );
                                    g.drawLine( x0, HEADER, x0, HEADER + graphHeight );
                                    g.drawLine( x1, HEADER, x1, HEADER + graphHeight );
                                }
                            }
                        }
                    }
                    #endregion

                    #region 小節ごとのライン
                    if ( vsq != null ) {
                        int dashed_line_step = AppManager.getPositionQuantizeClock();
                        g.clipRect( key_width, HEADER, size.width - key_width, size.height - 2 * OFFSET_TRACK_TAB );
                        Color white100 = new Color( 0, 0, 0, 100 );
                        for ( Iterator<VsqBarLineType> itr = vsq.getBarLineIterator( AppManager.clockFromXCoord( getWidth() ) ); itr.hasNext(); ) {
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
                        if ( m_last_selected_curve.equals( CurveType.VEL ) || m_last_selected_curve.equals( CurveType.Accent ) || m_last_selected_curve.equals( CurveType.Decay ) ) {
                            drawVEL( g, vsq_track, back, false, m_last_selected_curve );
                        } else if ( m_last_selected_curve.equals( CurveType.VibratoRate ) || m_last_selected_curve.equals( CurveType.VibratoDepth ) ) {
                            drawVibratoControlCurve( g, vsq_track, m_last_selected_curve, back, false );
                        } else {
                            VsqBPList list_back = vsq_track.getCurve( m_last_selected_curve.getName() );
                            if ( list_back != null ) {
                                drawVsqBPList( g, list_back, back, false );
                            }
                        }

                        // 手前に描くカーブ
                        if ( m_selected_curve.equals( CurveType.VEL ) || m_selected_curve.equals( CurveType.Accent ) || m_selected_curve.equals( CurveType.Decay ) ) {
                            drawVEL( g, vsq_track, vel_color, true, m_selected_curve );
                        } else if ( m_selected_curve.equals( CurveType.VibratoRate ) || m_selected_curve.equals( CurveType.VibratoDepth ) ) {
                            drawVibratoControlCurve( g, vsq_track, m_selected_curve, front, true );
                        } else if ( m_selected_curve.equals( CurveType.Env ) ) {
                            drawEnvelope( g, vsq_track, front );
                        } else {
                            VsqBPList list_front = vsq_track.getCurve( m_selected_curve.getName() );
                            if ( list_front != null ) {
                                drawVsqBPList( g, list_front, front, true );
                            }
                            if ( m_selected_curve.equals( CurveType.PIT ) ) {
                                #region PBSの値に応じて，メモリを記入する
#if !JAVA
                                System.Drawing.Drawing2D.SmoothingMode old = g.nativeGraphics.SmoothingMode;
                                g.nativeGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
#endif
                                Color nrml = new Color( 0, 0, 0, 190 );
                                Color dash = new Color( 0, 0, 0, 128 );
                                Stroke nrml_stroke = new BasicStroke();
                                Stroke dash_stroke = new BasicStroke( 1.0f, 0, 0, 10.0f, new float[] { 2.0f, 2.0f }, 0.0f );
                                VsqBPList pbs = vsq_track.getCurve( CurveType.PBS.getName() );
                                pbs_at_mouse = pbs.getValue( clock_at_mouse );
                                int c = pbs.size();
                                int premeasure = vsq.getPreMeasureClocks();
                                int clock_start = AppManager.clockFromXCoord( key_width );
                                int clock_end = AppManager.clockFromXCoord( getWidth() );
                                if ( clock_start < premeasure && premeasure < clock_end ) {
                                    clock_start = premeasure;
                                }
                                int last_pbs = pbs.getValue( clock_start );
                                int last_clock = clock_start;
                                int ycenter = yCoordFromValue( 0 );
                                g.setColor( nrml );
                                g.drawLine( key_width, ycenter, getWidth(), ycenter );
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
                            drawAttachedCurve( g, vsq.AttachedCurves.get( AppManager.getSelected() - 1 ).get( m_selected_curve ) );
                        }
                    }

                    if ( AppManager.isWholeSelectedIntervalEnabled() ) {
                        //int stdx = AppManager.startToDrawX;
                        int start = AppManager.xCoordFromClocks( AppManager.wholeSelectedInterval.getStart() ) + 2;
                        int end = AppManager.xCoordFromClocks( AppManager.wholeSelectedInterval.getEnd() ) + 2;
                        g.setColor( s_brs_a098_000_000_000 );
                        g.fillRect( start, HEADER, end - start, getGraphHeight() );
                    }

                    if ( m_mouse_downed ) {
                        #region 選択されたツールに応じて描画
                        int value = valueFromYCoord( mouse.y );
                        if ( clock_at_mouse < vsq.getPreMeasure() ) {
                            clock_at_mouse = vsq.getPreMeasure();
                        }
                        int max = m_selected_curve.getMaximum();
                        int min = m_selected_curve.getMinimum();
                        if ( value < min ) {
                            value = min;
                        } else if ( max < value ) {
                            value = max;
                        }
                        EditTool tool = AppManager.getSelectedTool();
                        if ( tool == EditTool.LINE ) {
                            int xini = AppManager.xCoordFromClocks( m_line_start.x );
                            int yini = yCoordFromValue( m_line_start.y );
                            g.setColor( s_pen_050_140_150 );
                            g.drawLine( xini, yini, AppManager.xCoordFromClocks( clock_at_mouse ), yCoordFromValue( value ) );
                        } else if ( tool == EditTool.PENCIL ) {
                            if ( m_mouse_trace != null && !AppManager.isCurveMode() ) {
                                Vector<Integer> ptx = new Vector<Integer>();
                                Vector<Integer> pty = new Vector<Integer>();
                                //int stdx = AppManager.startToDrawX;
                                int height = getHeight() - 42;

                                int count = 0;
                                int lastx = 0;
                                int lasty = 0;
                                for ( Iterator<Integer> itr = m_mouse_trace.keySet().iterator(); itr.hasNext(); ) {
                                    int key = itr.next();
                                    int x = key - stdx;
                                    int y = m_mouse_trace.get( key );
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
                                               nPoints );
                            }
                        } else if ( tool == EditTool.ERASER || tool == EditTool.ARROW ) {
                            if ( m_mouse_down_mode == MouseDownMode.CURVE_EDIT && m_mouse_moved && AppManager.curveSelectingRectangle.width != 0 ) {
                                int xini = AppManager.xCoordFromClocks( AppManager.curveSelectingRectangle.x );
                                int xend = AppManager.xCoordFromClocks( AppManager.curveSelectingRectangle.x + AppManager.curveSelectingRectangle.width );
                                int x_start = Math.Min( xini, xend );
                                if ( x_start < key_width ) {
                                    x_start = key_width;
                                }
                                int x_end = Math.Max( xini, xend );
                                int yini = yCoordFromValue( AppManager.curveSelectingRectangle.y );
                                int yend = yCoordFromValue( AppManager.curveSelectingRectangle.y + AppManager.curveSelectingRectangle.height );
                                int y_start = Math.Min( yini, yend );
                                int y_end = Math.Max( yini, yend );
                                if ( y_start < 8 ) y_start = 8;
                                if ( y_end > getHeight() - 42 - 8 ) y_end = getHeight() - 42;
                                if ( x_start < x_end ) {
                                    g.setColor( BRS_A144_255_255_255 );
                                    g.fillRect( x_start, y_start, x_end - x_start, y_end - y_start );
                                }
                            } else if ( m_mouse_down_mode == MouseDownMode.VEL_EDIT && m_veledit_selected.containsKey( m_veledit_last_selectedid ) ) {
                                if ( m_selected_curve.equals( CurveType.VEL ) ) {
                                    numeric_view = m_veledit_selected.get( m_veledit_last_selectedid ).editing.ID.Dynamics;
                                } else if ( m_selected_curve.equals( CurveType.Accent ) ) {
                                    numeric_view = m_veledit_selected.get( m_veledit_last_selectedid ).editing.ID.DEMaccent;
                                } else if ( m_selected_curve.equals( CurveType.Decay ) ) {
                                    numeric_view = m_veledit_selected.get( m_veledit_last_selectedid ).editing.ID.DEMdecGainRate;
                                }
                            }
                        }
                        if ( m_mouse_down_mode == MouseDownMode.SINGER_LIST && AppManager.getSelectedTool() != EditTool.ERASER ) {
                            for ( Iterator<SelectedEventEntry> itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                                SelectedEventEntry item = itr.next();
                                int x = AppManager.xCoordFromClocks( item.editing.Clock );
                                g.setColor( s_pen_128_128_128 );
                                g.drawPolygon( new int[] { x, x, x + SINGER_ITEM_WIDTH, x + SINGER_ITEM_WIDTH },
                                               new int[] { size.height - OFFSET_TRACK_TAB, size.height - 2 * OFFSET_TRACK_TAB + 1, size.height - 2 * OFFSET_TRACK_TAB + 1, size.height - OFFSET_TRACK_TAB },
                                               4 );
                                g.setColor( s_pen_246_251_010 );
                                g.drawLine( x, size.height - OFFSET_TRACK_TAB,
                                            x + SINGER_ITEM_WIDTH, size.height - OFFSET_TRACK_TAB );
                            }
                        }
                        #endregion
                    }
                    #endregion
                }

                if ( m_curve_visible ) {
                    #region カーブの種類一覧
                    Color font_color_normal = Color.black;
                    g.setColor( new Color( 212, 212, 212 ) );
                    g.fillRect( 0, 0, key_width, size.height - 2 * OFFSET_TRACK_TAB );

                    // 数値ビュー
                    Rectangle num_view = new Rectangle( 13, 4, 38, 16 );
                    g.setColor( new Color( 125, 123, 124 ) );
                    g.drawRect( num_view.x, num_view.y, num_view.width, num_view.height );
                    //StringFormat sf = new StringFormat();
                    //sf.Alignment = StringAlignment.Far;
                    //sf.LineAlignment = StringAlignment.Far;
                    g.setFont( AppManager.baseFont9 );
                    g.setColor( brs_string );
                    g.drawString( numeric_view + "", num_view.x, num_view.y ); // sf );

                    // 現在表示されているカーブの名前
                    //sf.Alignment = StringAlignment.Center;
                    //sf.LineAlignment = StringAlignment.Near;
                    g.drawString( m_selected_curve.getName(), 7, 24 ); // new Rectangle( 7, 24, 56, 14 ), sf

                    for ( Iterator<CurveType> itr = m_viewing_curves.iterator(); itr.hasNext(); ) {
                        CurveType curve = itr.next();
                        Rectangle rc = getRectFromCurveType( curve );
                        if ( curve.equals( m_selected_curve ) || curve.equals( m_last_selected_curve ) ) {
                            g.setColor( new Color( 108, 108, 108 ) );
                            g.fillRect( rc.x, rc.y, rc.width, rc.height );
                        }
                        g.setColor( rect_curve );
                        g.drawRect( rc.x, rc.y, rc.width, rc.height );
                        Rectangle rc_str = new Rectangle( rc.x, rc.y + rc.height / 2 - AppManager.baseFont9OffsetHeight, rc.width, rc.height );
                        rc_str.y += 2;
                        if ( curve.equals( m_selected_curve ) ) {
                            g.setColor( Color.white );
                            g.drawString( curve.getName(), rc_str.x, rc_str.y ); // sf );
                        } else {
                            g.setColor( font_color_normal );
                            g.drawString( curve.getName(), rc_str.x, rc_str.y ); // sf );
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
                if ( isInRect( mouse.x, mouse.y, new Rectangle( key_width, HEADER, getWidth(), this.getGraphHeight() ) ) &&
                     m_mouse_down_mode != MouseDownMode.PRE_UTTERANCE_MOVE &&
                     m_mouse_down_mode != MouseDownMode.OVERLAP_MOVE &&
                     m_mouse_down_mode != MouseDownMode.VEL_EDIT ) {
                    int align = 1;
                    int valign = 0;
                    int shift = 50;
                    if ( m_selected_curve.equals( CurveType.PIT ) ) {
                        valign = 1;
                        shift = 100;
                    }
                    g.setFont( AppManager.baseFont10Bold );
                    g.setColor( Color.white );
                    PortUtil.drawStringEx( g,
                                           m_mouse_value + "",
                                           AppManager.baseFont10Bold,
                                           new Rectangle( mouse.x - 100, mouse.y - shift, 100, 100 ),
                                           align,
                                           valign );
                    if ( m_selected_curve.equals( CurveType.PIT ) ) {
                        float delta_note = m_mouse_value * pbs_at_mouse / 8192.0f;
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

                #region 外枠
                // 左外側
                g.setColor( new Color( 160, 160, 160 ) );
                g.drawLine( 0, 0, 0, size.height - 2 );
                // 左内側
                g.setColor( new Color( 105, 105, 105 ) );
                g.drawLine( 1, 0, 1, size.height - 1 );
                // 下内側
                g.setColor( new Color( 192, 192, 192 ) );
                g.drawLine( 1, size.height - 2,
                            size.width + 20, size.height - 2 );
                // 下外側
                g.setColor( Color.white );
                g.drawLine( 0, size.height - 1,
                            size.width + 20, size.height - 1 );
                /*/ 右外側
                g.drawLine( Pens.White,
                            new Point( size.Width - 1, 0 ),
                            new Point( size.Width - 1, size.Height - 1 ) );
                // 右内側
                g.drawLine( new Pen( Color.FromArgb( 227, 227, 227 ) ),
                            new Point( size.Width - 2, 0 ),
                            new Point( size.Width - 2, size.Height - 2 ) );*/
                #endregion
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "TrackSelector#paint; ex= "+ ex );
            }
        }

        private void drawEnvelope( Graphics2D g, VsqTrack track, Color fill_color ) {
            int clock_start = AppManager.clockFromXCoord( AppManager.keyWidth );
            int clock_end = AppManager.clockFromXCoord( getWidth() );

            VsqFileEx vsq = AppManager.getVsqFile();
            VsqEvent last = null;
            Polygon highlight = null;
            Point pmouse = pointToClient( PortUtil.getMousePosition() );
            Point mouse = new Point( pmouse.x, pmouse.y );
            int px_preutterance = int.MinValue;
            int px_overlap = int.MinValue;
            int drawn_id = -1;
            int distance = int.MaxValue;
            float drawn_preutterance = 0;
            float drawn_overlap = 0;

            Color brs = fill_color;
            Point selected_point = new Point();
            boolean selected_found = false;
            boolean search_mouse = (0 <= mouse.y && mouse.y <= getHeight());
            for ( Iterator<VsqEvent> itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                if ( last == null ) {
                    last = item;
                    continue;
                }
                if ( item.Clock + item.ID.getLength() < clock_start ) {
                    last = item;
                    continue;
                }
                if ( clock_end < item.Clock ) {
                    break;
                }
                ByRef<Integer> preutterance = new ByRef<Integer>();
                ByRef<Integer> overlap = new ByRef<Integer>();
                Polygon points = getEnvelopePoints( vsq, last, item, preutterance, overlap );
                if ( m_mouse_down_mode == MouseDownMode.ENVELOPE_MOVE ||
                     m_mouse_down_mode == MouseDownMode.PRE_UTTERANCE_MOVE ||
                     m_mouse_down_mode == MouseDownMode.OVERLAP_MOVE ) {
                    if ( m_mouse_down_mode == MouseDownMode.ENVELOPE_MOVE && last.InternalID == m_envelope_move_id ) {
                        selected_point = new Point( points.xpoints[m_envelope_point_kind], points.ypoints[m_envelope_point_kind] );
                        selected_found = true;
                        highlight = points;
                        px_overlap = overlap.value;
                        px_preutterance = preutterance.value;
                        drawn_preutterance = last.UstEvent.PreUtterance;
                        drawn_overlap = last.UstEvent.VoiceOverlap;
                        drawn_id = last.InternalID;
                    } else if ( (m_mouse_down_mode == MouseDownMode.PRE_UTTERANCE_MOVE && last.InternalID == m_preutterance_moving_id) ||
                                (m_mouse_down_mode == MouseDownMode.OVERLAP_MOVE && last.InternalID == m_overlap_moving_id) ) {
                        highlight = points;
                        px_overlap = overlap.value;
                        px_preutterance = preutterance.value;
                        drawn_preutterance = last.UstEvent.PreUtterance;
                        drawn_overlap = last.UstEvent.VoiceOverlap;
                        drawn_id = last.InternalID;
                    }
                } else if ( search_mouse ) {
                    int draft_distance = int.MaxValue;
                    if ( points.xpoints[0] <= mouse.x && mouse.x <= points.xpoints[6] ) {
                        draft_distance = 0;
                    } else if ( mouse.x < points.xpoints[0] ) {
                        draft_distance = points.xpoints[0] - mouse.x;
                    } else {
                        draft_distance = mouse.x - points.xpoints[6];
                    }
                    if ( distance > draft_distance ) {
                        distance = draft_distance;
                        highlight = points;
                        px_overlap = overlap.value;
                        px_preutterance = preutterance.value;
                        drawn_preutterance = last.UstEvent.PreUtterance;
                        drawn_overlap = last.UstEvent.VoiceOverlap;
                        drawn_id = last.InternalID;
                    }
                }
                g.setColor( brs );
                g.fillPolygon( points );
                g.setColor( Color.white );
                g.drawPolygon( points );
                last = item;
            }
            int dotwid = DOT_WID * 2 + 1;
            if ( vsq != null && last != null ) {
                ByRef<Integer> preutterance = new ByRef<Integer>();
                ByRef<Integer> overlap = new ByRef<Integer>();
                Polygon points = getEnvelopePoints( vsq, last, null, preutterance, overlap );
                if ( m_mouse_down_mode == MouseDownMode.ENVELOPE_MOVE ||
                     m_mouse_down_mode == MouseDownMode.PRE_UTTERANCE_MOVE ||
                     m_mouse_down_mode == MouseDownMode.OVERLAP_MOVE ) {
                    if ( m_mouse_down_mode == MouseDownMode.ENVELOPE_MOVE && last.InternalID == m_envelope_move_id ) {
                        selected_point = new Point( points.xpoints[m_envelope_point_kind], points.ypoints[m_envelope_point_kind] );
                        selected_found = true;
                        highlight = points;
                        px_overlap = overlap.value;
                        px_preutterance = preutterance.value;
                        drawn_preutterance = last.UstEvent.PreUtterance;
                        drawn_overlap = last.UstEvent.VoiceOverlap;
                        drawn_id = last.InternalID;
                    } else if ( (m_mouse_down_mode == MouseDownMode.PRE_UTTERANCE_MOVE && last.InternalID == m_preutterance_moving_id) ||
                                (m_mouse_down_mode == MouseDownMode.OVERLAP_MOVE && last.InternalID == m_overlap_moving_id) ) {
                        highlight = points;
                        px_overlap = overlap.value;
                        px_preutterance = preutterance.value;
                        drawn_preutterance = last.UstEvent.PreUtterance;
                        drawn_overlap = last.UstEvent.VoiceOverlap;
                        drawn_id = last.InternalID;
                    }
                } else if ( search_mouse ) {
                    int draft_distance = int.MaxValue;
                    if ( points.xpoints[0] - dotwid <= mouse.x && mouse.x <= points.xpoints[6] + dotwid ) {
                        for ( int i = 1; i < 6; i++ ) {
                            Point p = new Point( points.xpoints[i], points.ypoints[i] );
                            Rectangle rc = new Rectangle( p.x - DOT_WID, p.y - DOT_WID, dotwid, dotwid );
                            g.setColor( DOT_COLOR_NORMAL );
                            g.fillRect( rc.x, rc.y, rc.width, rc.height );
                            g.setColor( DOT_COLOR_NORMAL );
                            g.drawRect( rc.x, rc.y, rc.width, rc.height );
                        }
                        draft_distance = 0;
                    } else if ( mouse.x < points.xpoints[0] ) {
                        draft_distance = points.xpoints[0] - mouse.x;
                    } else {
                        draft_distance = mouse.x - points.xpoints[6];
                    }
                    if ( distance > draft_distance ) {
                        distance = draft_distance;
                        highlight = points;
                        px_overlap = overlap.value;
                        px_preutterance = preutterance.value;
                        drawn_preutterance = last.UstEvent.PreUtterance;
                        drawn_overlap = last.UstEvent.VoiceOverlap;
                        drawn_id = last.InternalID;
                    }
                }
                g.setColor( brs );
                g.fillPolygon( points );
                g.setColor( Color.white );
                g.drawPolygon( points );
            }
            if ( highlight != null ) {
                for ( int i = 1; i < 6; i++ ) {
                    Point p = new Point( highlight.xpoints[i], highlight.ypoints[i] );
                    Rectangle rc = new Rectangle( p.x - DOT_WID, p.y - DOT_WID, dotwid, dotwid );
                    g.setColor( DOT_COLOR_NORMAL );
                    g.fillRect( rc.x, rc.y, rc.width, rc.height );
                    g.setColor( DOT_COLOR_NORMAL );
                    g.drawRect( rc.x, rc.y, rc.width, rc.height );
                }
            }
            if ( selected_found ) {
                Rectangle rc = new Rectangle( selected_point.x - DOT_WID, selected_point.y - DOT_WID, dotwid, dotwid );
                g.setColor( AppManager.getHilightColor() );
                g.fillRect( rc.x, rc.y, rc.width, rc.height );
                g.setColor( DOT_COLOR_NORMAL );
                g.drawRect( rc.x, rc.y, rc.width, rc.height );
            }

            if ( px_preutterance != int.MinValue && px_overlap != int.MinValue ) {
                drawPreutteranceAndOverlap( g, px_preutterance, px_overlap, drawn_preutterance, drawn_overlap );
            }
            m_overlap_viewing = drawn_id;
            m_preutterance_viewing = drawn_id;
        }

        private void drawPreutteranceAndOverlap( Graphics2D g, int px_preutterance, int px_overlap, float preutterance, float overlap ) {
            int OFFSET_PRE = 15;
            int OFFSET_OVL = 40;
            g.setColor( PortUtil.Orange );
            g.drawLine( px_preutterance, HEADER + 1, px_preutterance, getGraphHeight() + HEADER );
            g.setColor( PortUtil.LawnGreen );
            g.drawLine( px_overlap, HEADER + 1, px_overlap, getGraphHeight() + HEADER );

            String s_pre = "Pre Utterance: " + preutterance;
            java.awt.Dimension size = Util.measureString( s_pre, AppManager.baseFont10 );
            m_preutterance_bounds = new Rectangle( px_preutterance + 1, OFFSET_PRE, (int)size.width, (int)size.height );
            String s_ovl = "Overlap: " + overlap;
            size = Util.measureString( s_ovl, AppManager.baseFont10 );
            m_overlap_bounds = new Rectangle( px_overlap + 1, OFFSET_OVL, (int)size.width, (int)size.height );

            Color pen = new Color( 0, 0, 0, 50 );
            Color transp = new Color( PortUtil.Orange.getRed(), PortUtil.Orange.getGreen(), PortUtil.Orange.getBlue(), 50 );
            g.setColor( transp );
            g.fillRect( m_preutterance_bounds.x, m_preutterance_bounds.y, m_preutterance_bounds.width, m_preutterance_bounds.height );
            g.setColor( pen );
            g.drawRect( m_preutterance_bounds.x, m_preutterance_bounds.y, m_preutterance_bounds.width, m_preutterance_bounds.height );
            transp = new Color( PortUtil.LawnGreen.getRed(), PortUtil.LawnGreen.getGreen(), PortUtil.LawnGreen.getBlue(), 50 );
            g.setColor( transp );
            g.fillRect( m_overlap_bounds.x, m_overlap_bounds.y, m_overlap_bounds.width, m_overlap_bounds.height );
            g.setColor( pen );
            g.drawRect( m_overlap_bounds.x, m_overlap_bounds.y, m_overlap_bounds.width, m_overlap_bounds.height );

            g.setFont( AppManager.baseFont10 );
            g.setColor( Color.black );
            g.drawString( s_pre, px_preutterance + 1, OFFSET_PRE );
            g.drawString( s_ovl, px_overlap + 1, OFFSET_OVL );
        }

        /// <summary>
        /// 画面上の指定した点に、コントロールカーブのデータ点があるかどうかを調べます
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private long findDataPointAt( int locx, int locy ) {
            if ( m_selected_curve.equals( CurveType.Accent ) ||
                 m_selected_curve.equals( CurveType.Decay ) ||
                 m_selected_curve.equals( CurveType.Env ) ||
                 m_selected_curve.equals( CurveType.VEL ) ) {
                return -1;
            }
            if ( m_selected_curve.equals( CurveType.VibratoDepth ) ||
                 m_selected_curve.equals( CurveType.VibratoRate ) ) {
                //TODO: この辺
            } else {
                VsqBPList list = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( m_selected_curve.getName() );
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
        /// <param name="location">画面上の点</param>
        /// <param name="internal_id">見つかったエンベロープ・ポイントを保持しているVsqEventのID</param>
        /// <param name="point_kind">見つかったエンベロープ・ポイントのタイプ。(p1,v1)なら1、(p2,v2)なら2，(p5,v5)なら3，(p3,v3)なら4，(p4,v4)なら5</param>
        /// <returns>見つかった場合は真を、そうでなければ偽を返します</returns>
        private boolean findEnvelopePointAt( int locx, int locy, ByRef<Integer> internal_id, ByRef<Integer> point_kind ) {
            int clock_start = AppManager.clockFromXCoord( AppManager.keyWidth );
            int clock_end = AppManager.clockFromXCoord( getWidth() );
            VsqEvent last = null;
            int dotwid = DOT_WID * 2 + 1;
            VsqFileEx vsq = AppManager.getVsqFile();
            for ( Iterator<VsqEvent> itr = vsq.Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                if ( last == null ) {
                    last = item;
                    continue;
                }
                if ( item.Clock + item.ID.getLength() < clock_start ) {
                    last = item;
                    continue;
                }
                if ( clock_end < last.Clock ) {
                    last = item;
                    break;
                }
                Polygon points = getEnvelopePoints( vsq, last, item );
                for ( int i = 5; i >= 1; i-- ) {
                    Point p = new Point( points.xpoints[i], points.ypoints[i] );
                    Rectangle rc = new Rectangle( p.x - DOT_WID, p.y - DOT_WID, dotwid, dotwid );
                    if ( isInRect( locx, locy, rc ) ) {
                        internal_id.value = last.InternalID;
                        point_kind.value = i;
                        return true;
                    }
                }
                last = item;
            }
            if ( last != null ) {
                Polygon points = getEnvelopePoints( vsq, last, null );
                for ( int i = 5; i >= 1; i-- ) {
                    Point p = new Point( points.xpoints[i], points.ypoints[i] );
                    Rectangle rc = new Rectangle( p.x - DOT_WID, p.y - DOT_WID, dotwid, dotwid );
                    if ( isInRect( locx, locy, rc ) ) {
                        internal_id.value = last.InternalID;
                        point_kind.value = i;
                        return true;
                    }
                }
            }
            internal_id.value = -1;
            point_kind.value = -1;
            return false;
        }

        private Polygon getEnvelopePoints( VsqFileEx vsq, VsqEvent item, VsqEvent next_item ) {
            ByRef<Integer> i = new ByRef<Integer>();
            ByRef<Integer> j = new ByRef<Integer>();
            return getEnvelopePoints( vsq, item, next_item, i, j );
        }

        private Polygon getEnvelopePoints( VsqFileEx vsq, VsqEvent item, VsqEvent next_item, ByRef<Integer> px_pre_utteramce, ByRef<Integer> px_overlap ) {
            double sec_start = vsq.getSecFromClock( item.Clock );
            double sec_end = vsq.getSecFromClock( item.Clock + item.ID.getLength() );
            UstEvent ust_event = item.UstEvent;
            if ( ust_event == null ) {
                ust_event = new UstEvent();
            }
            UstEnvelope draw_target = ust_event.Envelope;
            if ( draw_target == null ) {
                draw_target = new UstEnvelope();
            }
            float pre_utterance = ust_event.PreUtterance;
            float overlap = ust_event.VoiceOverlap;

            UstEvent ust_event_next = null;
            double sec_start_next = double.MaxValue;
            if ( next_item != null ) {
                ust_event_next = next_item.UstEvent;
                sec_start_next = vsq.getSecFromClock( next_item.Clock );
            }
            if ( ust_event_next == null ) {
                ust_event_next = new UstEvent();
            }
            sec_start_next = sec_start_next - (ust_event_next.PreUtterance - ust_event_next.VoiceOverlap) / 1000.0;

            sec_start = sec_start - pre_utterance / 1000.0;
            if ( sec_start_next < sec_end ) {
                sec_end = sec_start_next;
            }
            int p_start = AppManager.xCoordFromClocks( (int)vsq.getClockFromSec( sec_start ) );
            int p_end = AppManager.xCoordFromClocks( (int)vsq.getClockFromSec( sec_end ) );
            px_pre_utteramce.value = p_start;
            px_overlap.value = AppManager.xCoordFromClocks( (int)vsq.getClockFromSec( sec_start + overlap / 1000.0 ) );
            int p1 = AppManager.xCoordFromClocks( (int)vsq.getClockFromSec( sec_start + draw_target.p1 / 1000.0 ) );
            int p2 = AppManager.xCoordFromClocks( (int)vsq.getClockFromSec( sec_start + (draw_target.p1 + draw_target.p2) / 1000.0 ) );
            int p5 = AppManager.xCoordFromClocks( (int)vsq.getClockFromSec( sec_start + (draw_target.p1 + draw_target.p2 + draw_target.p5) / 1000.0 ) );
            int p3 = AppManager.xCoordFromClocks( (int)vsq.getClockFromSec( sec_end - (draw_target.p4 + draw_target.p3) / 1000.0 ) );
            int p4 = AppManager.xCoordFromClocks( (int)vsq.getClockFromSec( sec_end - draw_target.p4 / 1000.0 ) );
            int v1 = yCoordFromValue( draw_target.v1 );
            int v2 = yCoordFromValue( draw_target.v2 );
            int v3 = yCoordFromValue( draw_target.v3 );
            int v4 = yCoordFromValue( draw_target.v4 );
            int v5 = yCoordFromValue( draw_target.v5 );
            int y = yCoordFromValue( 0 );
            return new Polygon( new int[] { p_start, p1, p2, p5, p3, p4, p_end },
                                new int[] { y, v1, v2, v5, v3, v4, y },
                                7 );
        }

        private void drawTrackTab( Graphics2D g, Rectangle destRect, String name, boolean selected, boolean enabled, boolean render_required, Color hilight, Color render_button_hilight ) {
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
                        destRect.x, destRect.y + destRect.height );
            if ( PortUtil.getStringLength( name ) > 0 ) {
                // 上横線
                g.setColor( border );
                g.drawLine( destRect.x + 1, destRect.y,
                            destRect.x + destRect.width, destRect.y );
            }
            if ( render_required ) {
                g.setColor( border );
                g.drawLine( destRect.x + destRect.width - 10, destRect.y,
                            destRect.x + destRect.width - 10, destRect.y + destRect.height );
            }
            g.clipRect( destRect.x, destRect.y, destRect.width, destRect.height );
            String title = Utility.trimString( name, AppManager.baseFont8, panel_width );
            g.setFont( AppManager.baseFont8 );
            g.setColor( panel_title );
            g.drawString( title, destRect.x + 2, destRect.y + destRect.height / 2 - AppManager.baseFont8OffsetHeight );
            if ( render_required ) {
                g.setColor( button_title );
                g.drawString( "R", destRect.x + destRect.width - _PX_WIDTH_RENDER, destRect.y + destRect.height / 2 - AppManager.baseFont8OffsetHeight );
            }
            if ( selected ) {
                g.setColor( border );
                g.drawLine( destRect.x + destRect.width - 1, destRect.y,
                            destRect.x + destRect.width - 1, destRect.y + destRect.height );
                g.setColor( border );
                g.drawLine( destRect.x, destRect.y + destRect.height - 1,
                            destRect.x + destRect.width, destRect.y + destRect.height - 1 );
            }
            g.setClip( null );
            g.setColor( m_generic_line );
            g.drawLine( destRect.x + destRect.width, destRect.y,
                        destRect.x + destRect.width, destRect.y + destRect.height );
        }

        /// <summary>
        /// トラック選択部分の、トラック1個分の幅を調べます。pixel
        /// </summary>
        public int getSelectorWidth() {
            int draft = TRACK_SELECTOR_MAX_WIDTH;
            int maxTotalWidth = getWidth() - vScroll.getWidth() - AppManager.keyWidth; // トラックの一覧を表示するのに利用できる最大の描画幅
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
        public void drawVEL( Graphics2D g, VsqTrack track, Color color, boolean is_front, CurveType type ) {
#if JAVA
            System.out.println( "TrackSelector#drawVEL" );
#endif
            Point pmouse = pointToClient( PortUtil.getMousePosition() );
            Point mouse = new Point( pmouse.x, pmouse.y );

            int HEADER = 8;
            int height = getGraphHeight();
            float order = (type.equals( CurveType.VEL )) ? height / 127f : height / 100f;
            int oy = getHeight() - 42;
            Shape last_clip = g.getClip();
            int xoffset = AppManager.keyOffset + AppManager.keyWidth - AppManager.startToDrawX;
            g.clipRect( AppManager.keyWidth, HEADER, getWidth() - AppManager.keyWidth - vScroll.getWidth(), height );
            float scale = AppManager.scaleX;
            int count = track.getEventCount();

            g.setFont( AppManager.baseFont10Bold );
            boolean cursor_should_be_hand = false;
            for ( int i = 0; i < count; i++ ) {
                VsqEvent ve = track.getEvent( i );
                if ( ve.ID.type != VsqIDType.Anote ) {
                    continue;
                }
                int clock = ve.Clock;
                int x = (int)(clock * scale) + xoffset;
                if ( x + VEL_BAR_WIDTH < 0 ) {
                    continue;
                } else if ( getWidth() - vScroll.getWidth() < x ) {
                    break;
                } else {
                    int value = 0;
                    if ( type.equals( CurveType.VEL ) ) {
                        value = ve.ID.Dynamics;
                    } else if ( type.equals( CurveType.Accent ) ) {
                        value = ve.ID.DEMaccent;
                    } else if ( type.equals( CurveType.Decay ) ) {
                        value = ve.ID.DEMdecGainRate;
                    }
                    int y = oy - (int)(value * order);
                    if ( is_front && AppManager.isSelectedEventContains( AppManager.getSelected(), ve.InternalID ) ) {
                        g.setColor( s_brs_a127_008_166_172 );
                        g.fillRect( x, y, VEL_BAR_WIDTH, oy - y );
                        if ( m_mouse_down_mode == MouseDownMode.VEL_EDIT ) {
                            int editing = 0;
                            if ( m_veledit_selected.containsKey( ve.InternalID ) ) {
                                if ( m_selected_curve.equals( CurveType.VEL ) ) {
                                    editing = m_veledit_selected.get( ve.InternalID ).editing.ID.Dynamics;
                                } else if ( m_selected_curve.equals( CurveType.Accent ) ) {
                                    editing = m_veledit_selected.get( ve.InternalID ).editing.ID.DEMaccent;
                                } else if ( m_selected_curve.equals( CurveType.Decay ) ) {
                                    editing = m_veledit_selected.get( ve.InternalID ).editing.ID.DEMdecGainRate;
                                }
                                int edit_y = oy - (int)(editing * order);
                                g.setColor( BRS_A244_255_023_012 );
                                g.fillRect( x, edit_y, VEL_BAR_WIDTH, oy - edit_y );
                                g.setColor( Color.white );
                                g.drawString( editing + "", x + VEL_BAR_WIDTH, (edit_y > oy - 20) ? oy - 20 : edit_y );
                            }
                        }
                    } else {
                        g.setColor( color );
                        g.fillRect( x, y, VEL_BAR_WIDTH, oy - y );
                    }
                    if ( m_mouse_down_mode == MouseDownMode.VEL_EDIT ) {
                        cursor_should_be_hand = true;
                    } else {
                        if ( AppManager.getSelectedTool() == EditTool.ARROW && is_front && isInRect( mouse.x, mouse.y, new Rectangle( x, y, VEL_BAR_WIDTH, oy - y ) ) ) {
                            cursor_should_be_hand = true;
                        }
                    }
                }
            }
            if ( cursor_should_be_hand ) {
                if ( getCursor().getType() != java.awt.Cursor.HAND_CURSOR ){
                    setCursor( new Cursor( java.awt.Cursor.HAND_CURSOR ) );
                }
            } else {
                if ( getCursor().getType() != java.awt.Cursor.DEFAULT_CURSOR ) {
                    setCursor( new Cursor( java.awt.Cursor.DEFAULT_CURSOR ) );
                }
            }
            g.setClip( last_clip );
        }

        private void drawAttachedCurve( Graphics2D g, Vector<BezierChain> chains ) {
#if JAVA
            System.out.println( "TrackSelector#paint; drawAttachedCurve" );
#endif
#if DEBUG
            try {
                BezierCurves t;
#endif
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
                            g.setColor( CURVE_COLOR );
                            g.drawLine( pxCurrent.x, pxCurrent.y, pxNext.x, pxNext.y );
                        } else {
                            Point ctrl1 = (current.getControlRightType() == BezierControlType.None) ? pxCurrent : pxControlCurrent;
                            Point ctrl2 = (next.getControlLeftType() == BezierControlType.None) ? pxNext : pxControlNext;
                            g.setColor( CURVE_COLOR );
                            PortUtil.drawBezier( g, pxCurrent.x, pxCurrent.y,
                                                    ctrl1.x, ctrl1.y,
                                                    ctrl2.x, ctrl2.y,
                                                    pxNext.x, pxNext.y );
                        }

                        if ( current.getControlRightType() != BezierControlType.None ) {
                            g.setColor( CONROL_LINE );
                            g.drawLine( pxCurrent.x, pxCurrent.y, pxControlCurrent.x, pxControlCurrent.y );
                        }
                        if ( next.getControlLeftType() != BezierControlType.None ) {
                            g.setColor( CONROL_LINE );
                            g.drawLine( pxNext.x, pxNext.y, pxControlNext.x, pxControlNext.y );
                        }
                        //g.SmoothingMode = SmoothingMode.Default;

                        // コントロール点
                        if ( current.getControlRightType() == BezierControlType.Normal ) {
                            Rectangle rc = new Rectangle( pxControlCurrent.x - DOT_WID,
                                                          pxControlCurrent.y - DOT_WID,
                                                          DOT_WID * 2 + 1,
                                                          DOT_WID * 2 + 1 );
                            if ( chain_id == EditingChainID && current.getID() == EditingPointID ) {
                                g.setColor( AppManager.getHilightColor() );
                                g.fillOval( rc.x, rc.y, rc.width, rc.height );
                            } else {
                                g.setColor( DOT_COLOR_NORMAL );
                                g.fillOval( rc.x, rc.y, rc.width, rc.height );
                            }
                            g.setColor( DOT_COLOR_NORMAL );
                            g.drawOval( rc.x, rc.y, rc.width, rc.height );
                        }

                        // コントロール点
                        if ( next.getControlLeftType() == BezierControlType.Normal ) {
                            Rectangle rc = new Rectangle( pxControlNext.x - DOT_WID,
                                                          pxControlNext.y - DOT_WID,
                                                          DOT_WID * 2 + 1,
                                                          DOT_WID * 2 + 1 );
                            if ( chain_id == EditingChainID && next.getID() == EditingPointID ) {
                                g.setColor( AppManager.getHilightColor() );
                                g.fillOval( rc.x, rc.y, rc.width, rc.height );
                            } else {
                                g.setColor( DOT_COLOR_NORMAL );
                                g.fillOval( rc.x, rc.y, rc.width, rc.height );
                            }
                            g.setColor( DOT_COLOR_NORMAL );
                            g.drawOval( rc.x, rc.y, rc.width, rc.height );
                        }

                        // データ点
                        Rectangle rc2 = new Rectangle( pxCurrent.x - DOT_WID,
                                                        pxCurrent.y - DOT_WID,
                                                        DOT_WID * 2 + 1,
                                                        DOT_WID * 2 + 1 );
                        if ( chain_id == EditingChainID && current.getID() == EditingPointID ) {
                            g.setColor( AppManager.getHilightColor() );
                            g.fillRect( rc2.x, rc2.y, rc2.width, rc2.height );
                        } else {
                            g.setColor( DOT_COLOR_BASE );
                            g.fillRect( rc2.x, rc2.y, rc2.width, rc2.height );
                        }
                        g.setColor( DOT_COLOR_BASE );
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
                    if ( chain_id == EditingChainID && next.getID() == EditingPointID ) {
                        g.setColor( AppManager.getHilightColor() );
                        g.fillRect( rc_last.x, rc_last.y, rc_last.width, rc_last.height );
                    } else {
                        g.setColor( DOT_COLOR_BASE );
                        g.fillRect( rc_last.x, rc_last.y, rc_last.width, rc_last.height );
                    }
                    g.setColor( DOT_COLOR_BASE );
                    g.drawRect( rc_last.x, rc_last.y, rc_last.width, rc_last.height );
                }
#if DEBUG
            } catch ( Exception ex ) {
                AppManager.debugWriteLine( "TrackSelector+DrawAttatchedCurve" );
                AppManager.debugWriteLine( "    ex=" + ex );
            }
#endif
        }

        private Point getScreenCoord( PointD pt ) {
            return new Point( AppManager.xCoordFromClocks( (int)pt.getX() ), yCoordFromValue( (int)pt.getY() ) );
        }

        // TODO: TrackSelector+DrawVibratoControlCurve; かきかけ
        public void drawVibratoControlCurve( Graphics2D g, VsqTrack draw_target, CurveType type, Color color, boolean is_front ) {
#if JAVA
            System.out.println( "TrackSelector#paint; drawVibratoControlCurve" );
#endif
#if DEBUG
            //AppManager.DebugWriteLine( "TrackSelector+DrawVibratoControlCurve" );
#endif
            Shape last_clip = g.getClip();
            int graph_height = getGraphHeight();
            g.clipRect( AppManager.keyWidth, HEADER,
                        getWidth() - AppManager.keyWidth - vScroll.getWidth(), graph_height );

            //int track = AppManager.Selected;
            int cl_start = AppManager.clockFromXCoord( AppManager.keyWidth );
            int cl_end = AppManager.clockFromXCoord( getWidth() - vScroll.getWidth() );
            if ( is_front ) {
                /* // draw shadow of non-note area
                Shape last_clip2 = g.getClip();
                for ( Iterator itr = draw_target.getNoteEventIterator(); itr.hasNext(); ) {
                    VsqEvent ve = (VsqEvent)itr.next();
                    int start = ve.Clock + ve.ID.VibratoDelay;
                    int end = ve.Clock + ve.ID.Length;
                    if ( end < cl_start ) {
                        continue;
                    }
                    if ( cl_end < start ) {
                        break;
                    }
                    if ( ve.ID.VibratoHandle == null ) {
                        continue;
                    }
                    int x1 = xCoordFromClocks( start );
                    int x2 = xCoordFromClocks( end );
                    if ( x1 < AppManager.KEY_LENGTH ) {
                        x1 = AppManager.KEY_LENGTH;
                    }
                    if ( this.Width - vScroll.Width < x2 ) {
                        x2 = this.Width;
                    }
                    if ( x1 < x2 ) {
                        //g.SetClip( new Rectangle( x1, HEADER, x2 - x1, graph_height ), CombineMode.Exclude );
                        g.clipRect( x1, HEADER, x2 - x1, graph_height );
                    }
                }
                g.setColor( new Color( 0, 0, 0, 127 ) );
                g.fillRect( AppManager.KEY_LENGTH, HEADER, this.Width - AppManager.KEY_LENGTH - vScroll.Width, graph_height );

                g.setClip( last_clip );*/

                // draw curve
                Color shadow = new Color( 0, 0, 0, 127 );
                int last_shadow_x = AppManager.keyWidth;
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
                    int x1 = AppManager.xCoordFromClocks( start );
                    g.setColor( shadow );
                    g.fillRect( last_shadow_x, HEADER, x1 - last_shadow_x, graph_height );
                    int x2 = AppManager.xCoordFromClocks( end );
                    last_shadow_x = x2;
                    if ( x1 < x2 ) {
                        //g.clipRect( x1, HEADER, x2 - x1, graph_height );
                        Vector<Integer> polyx = new Vector<Integer>();
                        Vector<Integer> polyy = new Vector<Integer>();
                        int draw_width = x2 - x1;
                        polyx.add( x1 ); polyy.add( yCoordFromValue( 0, type.getMaximum(), type.getMinimum() ) );
                        if ( type.equals( CurveType.VibratoRate ) ) {
                            int last_y = yCoordFromValue( ve.ID.VibratoHandle.getStartRate(), type.getMaximum(), type.getMinimum() );
                            polyx.add( x1 ); polyy.add( last_y );
                            VibratoBPList ratebp = ve.ID.VibratoHandle.getRateBP();
                            int c = ratebp.getCount();
                            for ( int i = 0; i < c; i++ ) {
                                VibratoBPPair item = ratebp.getElement( i );
                                int x = x1 + (int)(item.X * draw_width);
                                int y = yCoordFromValue( item.Y, type.getMaximum(), type.getMinimum() );
                                polyx.add( x ); polyy.add( last_y );
                                polyx.add( x ); polyy.add( y );
                                last_y = y;
                            }
                            polyx.add( x2 );
                            polyy.add( last_y );
                        } else {
                            int last_y = yCoordFromValue( ve.ID.VibratoHandle.getStartDepth(), type.getMaximum(), type.getMinimum() );
                            polyx.add( x1 );
                            polyy.add( last_y );
                            VibratoBPList depthbp = ve.ID.VibratoHandle.getDepthBP();
                            int c = depthbp.getCount();
                            for ( int i = 0; i < c; i++ ) {
                                VibratoBPPair item = depthbp.getElement( i );
                                int x = x1 + (int)(item.X * draw_width);
                                int y = yCoordFromValue( item.Y, type.getMaximum(), type.getMinimum() );
                                polyx.add( x ); polyy.add( last_y );
                                polyx.add( x ); polyy.add( y );
                                last_y = y;
                            }
                            polyx.add( x2 ); polyy.add( last_y );
                        }
                        polyx.add( x2 ); polyy.add( yCoordFromValue( 0, type.getMaximum(), type.getMinimum() ) );
                        g.setColor( color );
                        g.fillPolygon( PortUtil.convertIntArray( polyx.toArray( new Integer[] { } ) ),
                                       PortUtil.convertIntArray( polyy.toArray( new Integer[] { } ) ),
                                       polyx.size() );
                    }
                }
                g.setColor( shadow );
                g.fillRect( last_shadow_x, HEADER, getWidth() - AppManager.keyWidth - vScroll.getWidth(), graph_height );
            }
            g.setClip( last_clip );
        }

        /// <summary>
        /// Draws VsqBPList using specified Graphics "g", toward rectangular region "rect".
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="list"></param>
        /// <param name="color"></param>
        public void drawVsqBPList_impl( Graphics2D g, VsqBPList list, Color color, boolean is_front ) {
#if JAVA
            System.out.println( "TrackSelector#paint; drawVsqBPList" );
            System.out.println( "TrackSelector#paint; (list==null)=" + (list == null) );
#endif
            int max = list.getMaximum();
            int min = list.getMinimum();
            int height = getGraphHeight();
            float order = height / (float)(max - min);
            int oy = getHeight() - 42;
            Shape last_clip = g.getClip();
            g.clipRect( AppManager.keyWidth, HEADER,
                        getWidth() - AppManager.keyWidth - vScroll.getWidth(), getHeight() - 2 * OFFSET_TRACK_TAB );

            // 選択範囲。この四角の中に入っていたら、選択されているとみなす
            Rectangle select_window = new Rectangle( Math.Min( AppManager.curveSelectingRectangle.x, AppManager.curveSelectingRectangle.x + AppManager.curveSelectingRectangle.width ),
                                                     Math.Min( AppManager.curveSelectingRectangle.y, AppManager.curveSelectingRectangle.y + AppManager.curveSelectingRectangle.height ),
                                                     Math.Abs( AppManager.curveSelectingRectangle.width ),
                                                     Math.Abs( AppManager.curveSelectingRectangle.height ) );
            EditTool selected_tool = AppManager.getSelectedTool();
            boolean select_enabled = !m_selected_curve.isScalar() && ((selected_tool == EditTool.ARROW) || (selected_tool == EditTool.ERASER)) && m_mouse_downed;

            int start = AppManager.keyWidth;
            int start_clock = AppManager.clockFromXCoord( start );
            int end = getWidth() - vScroll.getWidth();
            int end_clock = AppManager.clockFromXCoord( end );
            int hilight_start = AppManager.curveSelectedInterval.getStart();
            int hilight_end = AppManager.curveSelectedInterval.getEnd();
            int hilight_start_x = AppManager.xCoordFromClocks( hilight_start );
            int hilight_end_x = AppManager.xCoordFromClocks( hilight_end );

            Color brush = color;
            Vector<Integer> pointsx = new Vector<Integer>();
            Vector<Integer> pointsy = new Vector<Integer>();
            Vector<Integer> index_selected_in_points = new Vector<Integer>(); // pointsのうち、選択された状態のものが格納されたインデックス
            pointsx.add( getWidth() - vScroll.getWidth() ); pointsy.add( oy );
            pointsx.add( AppManager.keyWidth ); pointsy.add( oy );
            int first_y = list.getValue( start_clock );
            int last_y = oy - (int)((first_y - min) * order);

            boolean first = true;
            if ( list.size() > 0 ) {
                int first_clock = list.getKeyClock( 0 );
                int last_x = AppManager.xCoordFromClocks( first_clock );
                first_y = list.getValue( first_clock );
                last_y = oy - (int)((first_y - min) * order);

                int c = list.size();
                for ( int i = 0; i < c; i++ ) {
                    int clock = list.getKeyClock( i );
                    if ( clock < start_clock ) {
                        continue;
                    }
                    if ( end_clock < clock ) {
                        break;
                    }
                    if ( first ) {
                        last_y = yCoordFromValue( list.getValue( AppManager.clockFromXCoord( AppManager.keyWidth ) ),
                                                  max,
                                                  min );
                        pointsx.add( AppManager.keyWidth ); pointsy.add( last_y );
                        first = false;
                    }

                    int x = AppManager.xCoordFromClocks( clock );
                    VsqBPPair v = list.getElementB( i );
                    int y = oy - (int)((v.value - min) * order);

                    pointsx.add( x ); pointsy.add( last_y );
                    pointsx.add( x ); pointsy.add( y );
                    if ( AppManager.isSelectedPointContains( v.id ) ) {
                        index_selected_in_points.add( pointsx.size() - 1 );
                    } else if ( select_enabled && isInRect( clock, v.value, select_window ) ) {
                        index_selected_in_points.add( pointsx.size() - 1 );
                    }
                    last_y = y;
                }
            }
            if ( first ) {
                last_y = yCoordFromValue( list.getValue( AppManager.clockFromXCoord( AppManager.keyWidth ) ),
                                          max,
                                          min );
                pointsx.add( AppManager.keyWidth ); pointsy.add( last_y );
            }
            last_y = oy - (int)((list.getValue( end_clock ) - min) * order);
            pointsx.add( getWidth() - vScroll.getWidth() ); pointsy.add( last_y );
            g.setColor( brush );
            g.fillPolygon( PortUtil.convertIntArray( pointsx.toArray( new Integer[] { } ) ),
                           PortUtil.convertIntArray( pointsy.toArray( new Integer[] { } ) ),
                           pointsx.size() );

            if ( is_front ) {
                // データ点を描画
                int c_points = pointsx.size();
                int w = DOT_WID * 2 + 1;
                Color pen = Color.white;
                int b1x = pointsx.get( 0 );
                int b1y = pointsy.get( 0 );
                int b2x = pointsx.get( 1 );
                int b2y = pointsy.get( 1 );
                pointsx.removeElementAt( 0 ); pointsy.removeElementAt( 0 );
                pointsx.removeElementAt( 0 ); pointsy.removeElementAt( 0 );
                g.setColor( pen );
                g.drawPolygon( PortUtil.convertIntArray( pointsx.toArray( new Integer[] { } ) ),
                               PortUtil.convertIntArray( pointsy.toArray( new Integer[] { } ) ),
                               pointsx.size() );
                pointsx.insertElementAt( b2x, 0 ); pointsy.insertElementAt( b2y, 0 );
                pointsx.insertElementAt( b1x, 0 ); pointsy.insertElementAt( b1y, 0 );

                boolean draw_dot_near_mouse = true; // マウスの近くのデータ点だけ描画するモード
                int threshold_near_px = 200;  // マウスに「近い」と判定する距離（ピクセル単位）。
                Point pmouse = pointToClient( PortUtil.getMousePosition() );
                Point mouse = new Point( pmouse.x, pmouse.y );
                Color white5 = new Color( 255, 255, 255, 200 );
                for ( int i = 4; i < c_points; i += 2 ) {
                    Point p = new Point( pointsx.get( i ), pointsy.get( i ) );
                    if ( index_selected_in_points.contains( i ) ) {
                        g.setColor( CURVE_COLOR_DOT );
                        g.fillRect( p.x - DOT_WID, p.y - DOT_WID, w, w );
                    } else {
                        if ( draw_dot_near_mouse ) {
                            if ( mouse.y < 0 || getHeight() < mouse.y ) {
                                continue;
                            }
                            double x = Math.Abs( p.x - mouse.x ) / (double)threshold_near_px;
                            double sigma = 0.3;
                            int alpha = (int)(255.0 * Math.Exp( -(x * x) / (2.0 * sigma * sigma) ));
                            if ( alpha <= 0 ) {
                                continue;
                            }
                            Color brs = new Color( 255, 255, 255, alpha );
                            g.setColor( brs );
                            g.fillRect( p.x - DOT_WID, p.y - DOT_WID, w, w );
                        } else {
                            g.setColor( white5 );
                            g.fillRect( p.x - DOT_WID, p.y - DOT_WID, w, w );
                        }
                    }
                }
            }

            // m_mouse_down_modeごとの描画
            if ( is_front ) {
                if ( m_mouse_down_mode == MouseDownMode.POINT_MOVE ) {
                    Point pmouse = pointToClient( PortUtil.getMousePosition() );
                    Point mouse = new Point( pmouse.x, pmouse.y );
                    int dx = mouse.x + AppManager.startToDrawX - m_mouse_down_location.x;
                    int dy = mouse.y - m_mouse_down_location.y;
                    int w = DOT_WID * 2 + 1;
                    for ( Iterator<BPPair> itr = m_moving_points.iterator(); itr.hasNext(); ) {
                        BPPair item = itr.next();
                        int x = AppManager.xCoordFromClocks( item.Clock ) + dx;
                        int y = yCoordFromValue( item.Value ) + dy;
                        g.setColor( CURVE_COLOR_DOT );
                        g.fillRect( x - DOT_WID, y - DOT_WID, w, w );
                    }
                }
            }

            g.setClip( last_clip );
        }

        /// <summary>
        /// Draws VsqBPList using specified Graphics "g", toward rectangular region "rect".
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="list"></param>
        /// <param name="color"></param>
        public void drawVsqBPList( Graphics2D g, VsqBPList list, Color color, boolean is_front ) {
#if JAVA
            System.out.println( "TrackSelector#paint; drawVsqBPList" );
            System.out.println( "TrackSelector#paint; (list==null)=" + (list == null) );
#endif
            int max = list.getMaximum();
            int min = list.getMinimum();
            int graphHeight = getGraphHeight();
            int width = getWidth();
            float order = graphHeight / (float)(max - min);
            int oy = getHeight() - 42;
            Shape last_clip = g.getClip();
            g.clipRect( AppManager.keyWidth, HEADER,
                        width - AppManager.keyWidth - vScroll.getWidth(), getHeight() - 2 * OFFSET_TRACK_TAB );

            // 選択範囲。この四角の中に入っていたら、選択されているとみなす
            Rectangle select_window = new Rectangle( Math.Min( AppManager.curveSelectingRectangle.x, AppManager.curveSelectingRectangle.x + AppManager.curveSelectingRectangle.width ),
                                                     Math.Min( AppManager.curveSelectingRectangle.y, AppManager.curveSelectingRectangle.y + AppManager.curveSelectingRectangle.height ),
                                                     Math.Abs( AppManager.curveSelectingRectangle.width ),
                                                     Math.Abs( AppManager.curveSelectingRectangle.height ) );
            EditTool selected_tool = AppManager.getSelectedTool();
            boolean select_enabled = !m_selected_curve.isScalar() && ((selected_tool == EditTool.ARROW) || (selected_tool == EditTool.ERASER)) && m_mouse_downed;

            int start = AppManager.keyWidth;
            int start_clock = AppManager.clockFromXCoord( start );
            int end = width - vScroll.getWidth();
            int end_clock = AppManager.clockFromXCoord( end );
            int hilight_start = AppManager.curveSelectedInterval.getStart();
            int hilight_end = AppManager.curveSelectedInterval.getEnd();
            int hilight_start_x = AppManager.xCoordFromClocks( hilight_start );
            int hilight_end_x = AppManager.xCoordFromClocks( hilight_end );

            Color brush = color;
            Vector<Integer> pointsx = new Vector<Integer>();
            Vector<Integer> pointsy = new Vector<Integer>();
            Vector<Integer> index_selected_in_points = new Vector<Integer>(); // pointsのうち、選択された状態のものが格納されたインデックス
            pointsx.add( width - vScroll.getWidth() ); pointsy.add( oy );
            pointsx.add( AppManager.keyWidth ); pointsy.add( oy );
            int first_y = list.getValue( start_clock );
            int last_y = oy - (int)((first_y - min) * order);

            boolean first = true;
            if ( list.size() > 0 ) {
                int first_clock = list.getKeyClock( 0 );
                int last_x = AppManager.xCoordFromClocks( first_clock );
                first_y = list.getValue( first_clock );
                last_y = oy - (int)((first_y - min) * order);

                int c = list.size();
                for ( int i = 0; i < c; i++ ) {
                    int clock = list.getKeyClock( i );
                    if ( clock < start_clock ) {
                        continue;
                    }
                    if ( end_clock < clock ) {
                        break;
                    }
                    if ( first ) {
                        last_y = yCoordFromValue( list.getValue( AppManager.clockFromXCoord( AppManager.keyWidth ) ),
                                                  max,
                                                  min );
                        pointsx.add( AppManager.keyWidth ); pointsy.add( last_y );
                        first = false;
                    }

                    int x = AppManager.xCoordFromClocks( clock );
                    VsqBPPair v = list.getElementB( i );
                    int y = oy - (int)((v.value - min) * order);

                    pointsx.add( x ); pointsy.add( last_y );
                    pointsx.add( x ); pointsy.add( y );
                    if ( AppManager.isSelectedPointContains( v.id ) ) {
                        index_selected_in_points.add( pointsx.size() - 1 );
                    } else if ( select_enabled && isInRect( clock, v.value, select_window ) ) {
                        index_selected_in_points.add( pointsx.size() - 1 );
                    }
                    last_y = y;
                }
            }
            if ( first ) {
                last_y = yCoordFromValue( list.getValue( AppManager.clockFromXCoord( AppManager.keyWidth ) ),
                                          max,
                                          min );
                pointsx.add( AppManager.keyWidth ); pointsy.add( last_y );
            }
            last_y = oy - (int)((list.getValue( end_clock ) - min) * order);
            pointsx.add( width - vScroll.getWidth() ); pointsy.add( last_y );
            g.setColor( brush );
            g.fillPolygon( PortUtil.convertIntArray( pointsx.toArray( new Integer[] { } ) ),
                           PortUtil.convertIntArray( pointsy.toArray( new Integer[] { } ) ),
                           pointsx.size() );

            if ( is_front ) {
                // データ点を描画
                int c_points = pointsx.size();
                int w = DOT_WID * 2 + 1;
                Color pen = Color.white;
                int b1x = pointsx.get( 0 );
                int b1y = pointsy.get( 0 );
                int b2x = pointsx.get( 1 );
                int b2y = pointsy.get( 1 );
                pointsx.removeElementAt( 0 ); pointsy.removeElementAt( 0 );
                pointsx.removeElementAt( 0 ); pointsy.removeElementAt( 0 );
                g.setColor( pen );
                g.drawPolygon( PortUtil.convertIntArray( pointsx.toArray( new Integer[] { } ) ),
                               PortUtil.convertIntArray( pointsy.toArray( new Integer[] { } ) ),
                               pointsx.size() );
                pointsx.insertElementAt( b2x, 0 ); pointsy.insertElementAt( b2y, 0 );
                pointsx.insertElementAt( b1x, 0 ); pointsy.insertElementAt( b1y, 0 );

                boolean draw_dot_near_mouse = AppManager.drawCurveDotInControlCurveView; // マウスの近くのデータ点だけ描画するモード
                int threshold_near_px = 200;  // マウスに「近い」と判定する距離（ピクセル単位）。
                Point pmouse = pointToClient( PortUtil.getMousePosition() );
                Point mouse = new Point( pmouse.x, pmouse.y );
                Color white5 = new Color( 255, 255, 255, 200 );
                for ( int i = 4; i < c_points; i += 2 ) {
                    Point p = new Point( pointsx.get( i ), pointsy.get( i ) );
                    if ( index_selected_in_points.contains( i ) ) {
                        g.setColor( CURVE_COLOR_DOT );
                        g.fillRect( p.x - DOT_WID, p.y - DOT_WID, w, w );
                    } else {
                        if ( draw_dot_near_mouse ) {
                            if ( mouse.y < 0 || getHeight() < mouse.y ) {
                                continue;
                            }
                            double x = Math.Abs( p.x - mouse.x ) / (double)threshold_near_px;
                            double sigma = 0.3;
                            int alpha = (int)(255.0 * Math.Exp( -(x * x) / (2.0 * sigma * sigma) ));
                            if ( alpha <= 0 ) {
                                continue;
                            }
                            Color brs = new Color( 255, 255, 255, alpha );
                            g.setColor( brs );
                            g.fillRect( p.x - DOT_WID, p.y - DOT_WID, w, w );
                        } else {
                            g.setColor( white5 );
                            g.fillRect( p.x - DOT_WID, p.y - DOT_WID, w, w );
                        }
                    }
                }
            }

            // m_mouse_down_modeごとの描画
            if ( is_front ) {
                if ( m_mouse_down_mode == MouseDownMode.POINT_MOVE ) {
                    Point pmouse = pointToClient( PortUtil.getMousePosition() );
                    Point mouse = new Point( pmouse.x, pmouse.y );
                    int dx = mouse.x + AppManager.startToDrawX - m_mouse_down_location.x;
                    int dy = mouse.y - m_mouse_down_location.y;
                    int w = DOT_WID * 2 + 1;
                    for ( Iterator<BPPair> itr = m_moving_points.iterator(); itr.hasNext(); ) {
                        BPPair item = itr.next();
                        int x = AppManager.xCoordFromClocks( item.Clock ) + dx;
                        int y = yCoordFromValue( item.Value ) + dy;
                        g.setColor( CURVE_COLOR_DOT );
                        g.fillRect( x - DOT_WID, y - DOT_WID, w, w );
                    }
                }
            }

            g.setClip( last_clip );
        }

        /// <summary>
        /// カーブエディタのグラフ部分の高さを取得します(pixel)
        /// </summary>
        public int getGraphHeight() {
            return getHeight() - 42 - 8;
        }

        /// <summary>
        /// カーブエディタのグラフ部分の幅を取得します。(pixel)
        /// </summary>
        public int getGraphWidth() {
            return getWidth() - AppManager.keyWidth - vScroll.getWidth();
        }

        private void TrackSelector_Load( Object sender, BEventArgs e ) {
#if !JAVA
            this.SetStyle( System.Windows.Forms.ControlStyles.DoubleBuffer, true );
            this.SetStyle( System.Windows.Forms.ControlStyles.UserPaint, true );
            this.SetStyle( System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true );
#endif
        }

        private void TrackSelector_MouseClick( Object sender, BMouseEventArgs e ) {
            if ( m_curve_visible ) {
                if ( e.Button == BMouseButtons.Left ) {
                    // カーブの種類一覧上で発生したイベントかどうかを検査
                    for ( Iterator<CurveType> itr = m_viewing_curves.iterator(); itr.hasNext(); ) {
                        CurveType curve = itr.next();
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
                        if ( kind == RendererKind.VOCALOID1_100 || kind == RendererKind.VOCALOID1_101 ) {
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
                        } else if ( kind == RendererKind.UTAU || kind == RendererKind.STRAIGHT_UTAU ) {
                            cmenuCurveVelocity.setVisible( false );
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
                        }
                        for ( int i = 0; i < sub_cmenu_curve.Length; i++ ) {
                            MenuElement tsi = sub_cmenu_curve[i];
                            if ( tsi is BMenuItem ) {
                                BMenuItem tsmi = (BMenuItem)tsi;
                                if ( tsmi.getTag() is CurveType ) {
                                    CurveType ct = (CurveType)tsmi.getTag();
                                    if ( ct.equals( m_selected_curve ) ) {
                                        tsmi.setSelected( true );
                                        break;
                                    }
                                }
                                MenuElement[] sub_tsmi = tsmi.getSubElements();
                                for ( int j = 0; j < sub_tsmi.Length; j++ ) {
                                    MenuElement tsi2 = sub_tsmi[j];
                                    if ( tsi2 is BMenuItem ) {
                                        BMenuItem tsmi2 = (BMenuItem)tsi2;
                                        if ( tsmi2.getTag() is CurveType ) {
                                            CurveType ct2 = (CurveType)tsmi2.getTag();
                                            if ( ct2.equals( m_selected_curve ) ) {
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
                        }

                        cmenuCurve.show( this, e.X, e.Y );
                    }
                }
            }
        }

        public void SelectNextCurve() {
            int index = 0;
            if ( m_viewing_curves.size() >= 2 ) {
                for ( int i = 0; i < m_viewing_curves.size(); i++ ) {
                    if ( m_viewing_curves.get( i ).equals( m_selected_curve ) ) {
                        index = i;
                        break;
                    }
                }
                index++;
                if ( m_viewing_curves.size() <= index ) {
                    index = 0;
                }
                changeCurve( m_viewing_curves.get( index ) );
            }
        }

        public void SelectPreviousCurve() {
            int index = 0;
            if ( m_viewing_curves.size() >= 2 ) {
                for ( int i = 0; i < m_viewing_curves.size(); i++ ) {
                    if ( m_viewing_curves.get( i ).equals( m_selected_curve ) ) {
                        index = i;
                        break;
                    }
                }
                index--;
                if ( index < 0 ) {
                    index = m_viewing_curves.size() - 1;
                }
                changeCurve( m_viewing_curves.get( index ) );
            }
        }

        private BezierPoint HandleMouseMoveForBezierMove( int clock, int value, int value_raw, BezierPickedSide picked ) {
            BezierChain target = AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).getBezierChain( m_selected_curve, AppManager.getLastSelectedBezier().chainID );
            int point_id = AppManager.getLastSelectedBezier().pointID;
            /*BezierChain target = null;
            Vector<BezierChain> list = AppManager.VsqFile.AttachedCurves[AppManager.Selected - 1][m_selected_curve];
            for ( int i = 0; i < list.Count; i++ ) {
                if ( list[i].ID == AppManager.LastSelectedBezier.ChainID ) {
                    target = list[i];
                }
            }*/
            int index = -1;
            for ( int i = 0; i < target.points.size(); i++ ) {
                if ( target.points.get( i ).getID() == point_id ) {
                    index = i;
                    break;
                }
            }
            float scale_x = AppManager.scaleX;
            float scale_y = getScaleY();
            BezierPoint ret = new BezierPoint( 0, 0 );
            if ( index >= 0 ) {
                BezierPoint item = target.points.get( index );
                if ( picked == BezierPickedSide.BASE ) {
                    Point old = target.points.get( index ).getBase().toPoint();
                    item.setBase( new PointD( clock, value ) );
                    if ( !BezierChain.isBezierImplicit( target ) ) {
                        item.setBase( new PointD( old.x, old.y ) );
                    }
                    ret = (BezierPoint)target.points.get( index ).clone();
                } else if ( picked == BezierPickedSide.LEFT ) {
                    if ( item.getControlLeftType() != BezierControlType.Master ) {
                        Point old1 = item.getControlLeft().toPoint();
                        Point old2 = item.getControlRight().toPoint();
                        Point b = item.getBase().toPoint();
                        item.setControlLeft( new PointD( clock, value_raw ) );
                        double dx = (b.x - item.getControlLeft().getX()) * scale_x;
                        double dy = (b.y - item.getControlLeft().getY()) * scale_y;
                        BezierPoint original = AppManager.getLastSelectedBezier().original;
                        double theta = Math.Atan2( dy, dx );
                        double dx2 = (original.getControlRight().getX() - b.x) * scale_x;
                        double dy2 = (original.getControlRight().getY() - b.y) * scale_y;
                        double l = Math.Sqrt( dx2 * dx2 + dy2 * dy2 );
                        dx2 = l * Math.Cos( theta ) / scale_x;
                        dy2 = l * Math.Sin( theta ) / scale_y;
                        item.setControlRight( new PointD( b.x + (int)dx2, b.y + (int)dy2 ) );
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            item.setControlLeft( new PointD( old1.x, old1.y ) );
                            item.setControlRight( new PointD( old2.x, old2.y ) );
                        }
                    } else {
                        Point old = item.getControlLeft().toPoint();
                        item.setControlLeft( new PointD( clock, value_raw ) );
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            item.setControlLeft( new PointD( old.x, old.y ) );
                        }
                    }
                    ret = (BezierPoint)item.clone();
                } else if ( picked == BezierPickedSide.RIGHT ) {
                    if ( item.getControlRightType() != BezierControlType.Master ) {
                        Point old1 = item.getControlLeft().toPoint();
                        Point old2 = item.getControlRight().toPoint();
                        Point b = item.getBase().toPoint();
                        item.setControlRight( new PointD( clock, value_raw ) );
                        double dx = (item.getControlRight().getX() - b.x) * scale_x;
                        double dy = (item.getControlRight().getY() - b.y) * scale_y;
                        BezierPoint original = AppManager.getLastSelectedBezier().original;
                        double theta = Math.Atan2( dy, dx );
                        double dx2 = (b.x - original.getControlLeft().getX()) * scale_x;
                        double dy2 = (b.y - original.getControlLeft().getY()) * scale_y;
                        double l = Math.Sqrt( dx2 * dx2 + dy2 * dy2 );
                        dx2 = l * Math.Cos( theta ) / scale_x;
                        dy2 = l * Math.Sin( theta ) / scale_y;
                        item.setControlLeft( new PointD( b.x - (int)dx2, b.y - (int)dy2 ) );
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            item.setControlLeft( new PointD( old1.x, old1.y ) );
                            item.setControlRight( new PointD( old2.x, old2.y ) );
                        }
                    } else {
                        Point old = item.getControlRight().toPoint();
                        item.setControlRight( new PointD( clock, value ) );
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            item.setControlRight( new PointD( old.x, old.y ) );
                        }
                    }
                    ret = (BezierPoint)item.clone();
                }
            }
            return ret;
        }

        public BezierPoint HandleMouseMoveForBezierMove( BMouseEventArgs e, BezierPickedSide picked ) {
            int clock = AppManager.clockFromXCoord( e.X );
            int value = valueFromYCoord( e.Y );
            int value_raw = value;

            if ( clock < AppManager.getVsqFile().getPreMeasure() ) {
                clock = AppManager.getVsqFile().getPreMeasure();
            }
            int max = m_selected_curve.getMaximum();
            int min = m_selected_curve.getMinimum();
            if ( value < min ) {
                value = min;
            } else if ( max < value ) {
                value = max;
            }
            return HandleMouseMoveForBezierMove( clock, value, value_raw, picked );
        }

        private void TrackSelector_MouseMove( Object sender, BMouseEventArgs e ) {
#if DEBUG
            //AppManager.DebugWriteLine( "TrackSelectro_MouseMove" );
#endif
            int value = valueFromYCoord( e.Y );
            int value_raw = value;
            int max = m_selected_curve.getMaximum();
            int min = m_selected_curve.getMinimum();
            if ( value < min ) {
                value = min;
            } else if ( max < value ) {
                value = max;
            }
            m_mouse_value = value;

            if ( e.Button == BMouseButtons.None ) {
                return;
            }
            if ( (e.X + AppManager.startToDrawX != m_mouse_down_location.x || e.Y != m_mouse_down_location.y) ) {
#if JAVA
                if( m_mouse_hover_thread != null && m_mouse_hover_thread.isAlive() ){
                    m_mouse_hover_thread.stop();
                }
#else
                if ( m_mouse_hover_thread != null && m_mouse_hover_thread.IsAlive ) {
                    m_mouse_hover_thread.Abort();
                }
#endif
                if ( m_mouse_down_mode == MouseDownMode.VEL_WAIT_HOVER ) {
                    m_mouse_down_mode = MouseDownMode.VEL_EDIT;
                }
                m_mouse_moved = true;
            }
            if ( AppManager.isPlaying() ) {
                return;
            }
            int clock = AppManager.clockFromXCoord( e.X );

            if ( clock < AppManager.getVsqFile().getPreMeasure() ) {
                clock = AppManager.getVsqFile().getPreMeasure();
            }

            if ( e.Button == BMouseButtons.Left &&
                 0 <= e.Y && e.Y <= getHeight() - 2 * OFFSET_TRACK_TAB &&
                 m_mouse_down_mode == MouseDownMode.CURVE_EDIT ) {
                EditTool selected = AppManager.getSelectedTool();
                if ( selected == EditTool.PENCIL ) {
                    if ( e.X + AppManager.startToDrawX != m_mouse_down_location.x || e.Y != m_mouse_down_location.y ) {
                        m_pencil_moved = true;
                    } else {
                        m_pencil_moved = false;
                    }

                    if ( m_mouse_trace != null ) {
                        Vector<Integer> removelist = new Vector<Integer>();
                        int x = e.X + AppManager.startToDrawX;
                        if ( x < m_mouse_trace_last_x ) {
                            for ( Iterator<Integer> itr = m_mouse_trace.keySet().iterator(); itr.hasNext(); ) {
                                int key = itr.next();
                                if ( x <= key && key < m_mouse_trace_last_x ) {
                                    removelist.add( key );
                                }
                            }
                        } else if ( m_mouse_trace_last_x < x ) {
                            for ( Iterator<Integer> itr = m_mouse_trace.keySet().iterator(); itr.hasNext(); ) {
                                int key = itr.next();
                                if ( m_mouse_trace_last_x < key && key <= x ) {
                                    removelist.add( key );
                                }
                            }
                        }
                        for ( int i = 0; i < removelist.size(); i++ ) {
                            m_mouse_trace.remove( removelist.get( i ) );
                        }
                        if ( x == m_mouse_trace_last_x ) {
                            m_mouse_trace.put( x, e.Y );
                            m_mouse_trace_last_y = e.Y;
                        } else {
                            float a = (e.Y - m_mouse_trace_last_y) / (float)(x - m_mouse_trace_last_x);
                            float b = m_mouse_trace_last_y - a * m_mouse_trace_last_x;
                            int start = Math.Min( x, m_mouse_trace_last_x );
                            int end = Math.Max( x, m_mouse_trace_last_x );
                            for ( int xx = start; xx <= end; xx++ ) {
                                int yy = (int)(a * xx + b);
                                m_mouse_trace.put( xx, yy );
                            }
                            m_mouse_trace_last_x = x;
                            m_mouse_trace_last_y = e.Y;
                        }
                    }
                } else if ( selected == EditTool.ARROW ||
                            selected == EditTool.ERASER ) {
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
                    AppManager.curveSelectingRectangle.width = draft_clock - AppManager.curveSelectingRectangle.x;
                    AppManager.curveSelectingRectangle.height = value - AppManager.curveSelectingRectangle.y;
                }
            } else if ( m_mouse_down_mode == MouseDownMode.SINGER_LIST ) {
                int dclock = clock - m_singer_move_started_clock;
                for ( Iterator<SelectedEventEntry> itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                    SelectedEventEntry item = itr.next();
                    item.editing.Clock = item.original.Clock + dclock;
                }
            } else if ( m_mouse_down_mode == MouseDownMode.VEL_EDIT ) {
                int t_value = valueFromYCoord( e.Y - m_veledit_shifty );
                int d_vel = 0;
                if ( m_selected_curve.equals( CurveType.VEL ) ) {
                    d_vel = t_value - m_veledit_selected.get( m_veledit_last_selectedid ).original.ID.Dynamics;
                } else if ( m_selected_curve.equals( CurveType.Accent ) ) {
                    d_vel = t_value - m_veledit_selected.get( m_veledit_last_selectedid ).original.ID.DEMaccent;
                } else if ( m_selected_curve.equals( CurveType.Decay ) ) {
                    d_vel = t_value - m_veledit_selected.get( m_veledit_last_selectedid ).original.ID.DEMdecGainRate;
                }
                for ( Iterator<Integer> itr = m_veledit_selected.keySet().iterator(); itr.hasNext(); ) {
                    int id = itr.next();
                    if ( m_selected_curve.equals( CurveType.VEL ) ) {
                        int new_vel = m_veledit_selected.get( id ).original.ID.Dynamics + d_vel;
                        if ( new_vel < m_selected_curve.getMinimum() ) {
                            new_vel = m_selected_curve.getMinimum();
                        } else if ( m_selected_curve.getMaximum() < new_vel ) {
                            new_vel = m_selected_curve.getMaximum();
                        }
                        m_veledit_selected.get( id ).editing.ID.Dynamics = new_vel;
                    } else if ( m_selected_curve.equals( CurveType.Accent ) ) {
                        int new_vel = m_veledit_selected.get( id ).original.ID.DEMaccent + d_vel;
                        if ( new_vel < m_selected_curve.getMinimum() ) {
                            new_vel = m_selected_curve.getMinimum();
                        } else if ( m_selected_curve.getMaximum() < new_vel ) {
                            new_vel = m_selected_curve.getMaximum();
                        }
                        m_veledit_selected.get( id ).editing.ID.DEMaccent = new_vel;
                    } else if ( m_selected_curve.equals( CurveType.Decay ) ) {
                        int new_vel = m_veledit_selected.get( id ).original.ID.DEMdecGainRate + d_vel;
                        if ( new_vel < m_selected_curve.getMinimum() ) {
                            new_vel = m_selected_curve.getMinimum();
                        } else if ( m_selected_curve.getMaximum() < new_vel ) {
                            new_vel = m_selected_curve.getMaximum();
                        }
                        m_veledit_selected.get( id ).editing.ID.DEMdecGainRate = new_vel;
                    }
                }
            } else if ( m_mouse_down_mode == MouseDownMode.BEZIER_MODE ) {
                HandleMouseMoveForBezierMove( clock, value, value_raw, AppManager.getLastSelectedBezier().picked );
            } else if ( m_mouse_down_mode == MouseDownMode.BEZIER_ADD_NEW || m_mouse_down_mode == MouseDownMode.BEZIER_EDIT ) {
                BezierChain target = AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).getBezierChain( m_selected_curve, AppManager.getLastSelectedBezier().chainID );
                int point_id = AppManager.getLastSelectedBezier().pointID;
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
            } else if ( m_mouse_down_mode == MouseDownMode.ENVELOPE_MOVE ) {
                double sec = AppManager.getVsqFile().getSecFromClock( AppManager.clockFromXCoord( e.X ) );
                int v = valueFromYCoord( e.Y );
                if ( v < 0 ) {
                    v = 0;
                } else if ( 200 < v ) {
                    v = 200;
                }
                if ( sec < m_envelope_dot_begin ) {
                    sec = m_envelope_dot_begin;
                } else if ( m_envelope_dot_end < sec ) {
                    sec = m_envelope_dot_end;
                }
                if ( m_envelope_point_kind == 1 ) {
                    m_envelope_editing.p1 = (int)((sec - m_envelope_range_begin) * 1000.0);
                    m_envelope_editing.v1 = v;
                } else if ( m_envelope_point_kind == 2 ) {
                    m_envelope_editing.p2 = (int)((sec - m_envelope_range_begin) * 1000.0) - m_envelope_editing.p1;
                    m_envelope_editing.v2 = v;
                } else if ( m_envelope_point_kind == 3 ) {
                    m_envelope_editing.p5 = (int)((sec - m_envelope_range_begin) * 1000.0) - m_envelope_editing.p1 - m_envelope_editing.p2;
                    m_envelope_editing.v5 = v;
                } else if ( m_envelope_point_kind == 4 ) {
                    m_envelope_editing.p3 = (int)((m_envelope_range_end - sec) * 1000.0) - m_envelope_editing.p4;
                    m_envelope_editing.v3 = v;
                } else if ( m_envelope_point_kind == 5 ) {
                    m_envelope_editing.p4 = (int)((m_envelope_range_end - sec) * 1000.0);
                    m_envelope_editing.v4 = v;
                }
            } else if ( m_mouse_down_mode == MouseDownMode.PRE_UTTERANCE_MOVE ) {
                int clock_at_downed = AppManager.clockFromXCoord( m_mouse_down_location.x - AppManager.startToDrawX );
                VsqFileEx vsq = AppManager.getVsqFile();
                double dsec = vsq.getSecFromClock( clock ) - vsq.getSecFromClock( clock_at_downed );
                float draft_preutterance = m_pre_ovl_original.UstEvent.PreUtterance - (float)(dsec * 1000);
                m_pre_ovl_editing.UstEvent.PreUtterance = draft_preutterance;
            } else if ( m_mouse_down_mode == MouseDownMode.OVERLAP_MOVE ) {
                int clock_at_downed = AppManager.clockFromXCoord( m_mouse_down_location.x - AppManager.startToDrawX );
                VsqFileEx vsq = AppManager.getVsqFile();
                double dsec = vsq.getSecFromClock( clock ) - vsq.getSecFromClock( clock_at_downed );
                float draft_overlap = m_pre_ovl_original.UstEvent.VoiceOverlap + (float)(dsec * 1000);
                m_pre_ovl_editing.UstEvent.VoiceOverlap = draft_overlap;
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
        private void findBezierPointAt( int locx,
                                        int locy,
                                        Vector<BezierChain> list,
                                        ByRef<BezierChain> found_chain,
                                        ByRef<BezierPoint> found_point,
                                        ByRef<BezierPickedSide> found_side,
                                        int dot_width,
                                        int px_tolerance ) {
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

        private void processMouseDownSelectRegion( BMouseEventArgs e ) {
            if ( (PortUtil.getCurrentModifierKey() & InputEvent.CTRL_MASK) != InputEvent.CTRL_MASK ) {
                AppManager.clearSelectedPoint();
            }

            int clock = AppManager.clockFromXCoord( e.X );
            int quantized_clock = clock;
            int unit = AppManager.getPositionQuantizeClock();
            int odd = clock % unit;
            quantized_clock -= odd;
            if ( odd > unit / 2 ) {
                quantized_clock += unit;
            }

            int max = m_selected_curve.getMaximum();
            int min = m_selected_curve.getMinimum();
            int value = valueFromYCoord( e.Y );
            if ( value < min ) {
                value = min;
            } else if ( max < value ) {
                value = max;
            }

            if ( AppManager.editorConfig.CurveSelectingQuantized ) {
                AppManager.curveSelectingRectangle = new Rectangle( quantized_clock, value, 0, 0 );
            } else {
                AppManager.curveSelectingRectangle = new Rectangle( clock, value, 0, 0 );
            }
        }

        public void TrackSelector_MouseDown( Object sender, BMouseEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "TrackSelector_MouseDown" );
#endif
            VsqFileEx vsq = AppManager.getVsqFile();
            m_mouse_down_location = new Point( e.X + AppManager.startToDrawX, e.Y );
            int clock = AppManager.clockFromXCoord( e.X );
            int selected = AppManager.getSelected();
            m_mouse_moved = false;
            m_mouse_downed = true;
            if ( AppManager.keyWidth < e.X && clock < vsq.getPreMeasure() ) {
#if !JAVA
                System.Media.SystemSounds.Asterisk.Play();
#endif
                return;
            }
            m_modifier_on_mouse_down = PortUtil.getCurrentModifierKey();
            int max = m_selected_curve.getMaximum();
            int min = m_selected_curve.getMinimum();
            int value = valueFromYCoord( e.Y );
            if ( value < min ) {
                value = min;
            } else if ( max < value ) {
                value = max;
            }

            if ( getHeight() - OFFSET_TRACK_TAB <= e.Y && e.Y < getHeight() ) {
                if ( e.Button == BMouseButtons.Left ) {
                    #region MouseDown occured on track list
                    m_mouse_down_mode = MouseDownMode.TRACK_LIST;
                    //AppManager.isCurveSelectedIntervalEnabled() = false;
                    m_mouse_trace = null;
                    int selecter_width = getSelectorWidth();
                    if ( AppManager.getVsqFile() != null ) {
                        for ( int i = 0; i < 16; i++ ) {
                            int x = AppManager.keyWidth + i * selecter_width;
                            if ( AppManager.getVsqFile().Track.size() > i + 1 ) {
                                if ( x <= e.X && e.X < x + selecter_width ) {
                                    int new_selected = i + 1;
                                    if ( AppManager.getSelected() != new_selected ) {
                                        AppManager.setSelected( i + 1 );
#if JAVA
                                        try{
                                            selectedTrackChangedEvent.raise( this, i + 1 );
                                        }catch( Exception ex ){
                                            System.err.println( "TrackSelector#TrackSelector_MouseDown; ex=" + ex );
                                        }
#else
                                        if ( SelectedTrackChanged != null ) {
                                            SelectedTrackChanged( this, i + 1 );
                                        }
#endif
                                        invalidate();
                                        return;
                                    } else if ( x + selecter_width - _PX_WIDTH_RENDER <= e.X && e.X < e.X + selecter_width ) {
                                        if ( AppManager.getRenderRequired( AppManager.getSelected() ) && !AppManager.isPlaying() ) {
#if JAVA
                                            try{
                                                renderRequiredEvent.raise( this, new int[]{ AppManager.getSelected() } );
                                            }catch( Exception ex ){
                                                System.err.println( "TrackSelector#TrackSelector_MouseDown; ex=" + ex );
                                            }
#else
                                            if ( RenderRequired != null ) {
                                                RenderRequired( this, new int[] { AppManager.getSelected() } );
                                            }
#endif
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            } else if ( getHeight() - 2 * OFFSET_TRACK_TAB <= e.Y && e.Y < getHeight() - OFFSET_TRACK_TAB ) {
                #region MouseDown occured on singer tab
                m_mouse_down_mode = MouseDownMode.SINGER_LIST;
                AppManager.clearSelectedPoint();
                m_mouse_trace = null;
                VsqEvent ve = findItemAt( e.X, e.Y );
                if ( AppManager.getSelectedTool() == EditTool.ERASER ) {
                    #region EditTool.Eraser
                    if ( ve != null && ve.Clock > 0 ) {
                        CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventDelete( selected, ve.InternalID ) );
                        executeCommand( run, true );
                    }
                    #endregion
                } else {
                    if ( ve != null ) {
                        if ( (m_modifier_on_mouse_down & m_modifier_key) == m_modifier_key ) {
                            if ( AppManager.isSelectedEventContains( AppManager.getSelected(), ve.InternalID ) ) {
                                Vector<Integer> old = new Vector<Integer>();
                                for ( Iterator<SelectedEventEntry> itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                                    SelectedEventEntry item = itr.next();
                                    int id = item.original.InternalID;
                                    if ( id != ve.InternalID ) {
                                        old.add( id );
                                    }
                                }
                                AppManager.clearSelectedEvent();
                                AppManager.addSelectedEventAll( old );
                            } else {
                                AppManager.addSelectedEvent( ve.InternalID );
                            }
                        } else if ( (PortUtil.getCurrentModifierKey() & InputEvent.SHIFT_MASK) == InputEvent.SHIFT_MASK ) {
                            int last_clock = AppManager.getLastSelectedEvent().original.Clock;
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
                            AppManager.addSelectedEventAll( add_required );
                        } else {
                            if ( !AppManager.isSelectedEventContains( AppManager.getSelected(), ve.InternalID ) ) {
                                AppManager.clearSelectedEvent();
                            }
                            AppManager.addSelectedEvent( ve.InternalID );
                        }
                        m_singer_move_started_clock = clock;
                    } else {
                        AppManager.clearSelectedEvent();
                    }
                }
                #endregion
            } else {
                #region MouseDown occred on other position
                boolean clock_inner_note = false; //マウスの降りたクロックが，ノートの範囲内かどうかをチェック
                int left_clock = AppManager.clockFromXCoord( AppManager.keyWidth );
                int right_clock = AppManager.clockFromXCoord( getWidth() - vScroll.getWidth() );
                for ( Iterator<VsqEvent> itr = vsq.Track.get( AppManager.getSelected() ).getEventIterator(); itr.hasNext(); ) {
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
                    if ( e.Button == BMouseButtons.Left && !m_spacekey_downed ) {
                        //AppManager.isCurveSelectedIntervalEnabled() = false;
                        m_mouse_down_mode = MouseDownMode.CURVE_EDIT;
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
                                if ( m_selected_curve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownEnvelope( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                    if ( processMouseDownPreutteranceAndOverlap( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                } else if ( !m_selected_curve.equals( CurveType.VEL ) &&
                                            !m_selected_curve.equals( CurveType.Accent ) &&
                                            !m_selected_curve.equals( CurveType.Decay ) &&
                                            !m_selected_curve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownBezier( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                }
                            } else {
                                if ( m_selected_curve.equals( CurveType.Env ) ) {
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
                            m_line_start = new Point( clock, value );
                            #endregion
                        } else if ( AppManager.getSelectedTool() == EditTool.PENCIL ) {
                            #region Pencil
                            if ( AppManager.isCurveMode() ) {
                                #region CurveMode
                                if ( m_selected_curve.equals( CurveType.VibratoRate ) || m_selected_curve.equals( CurveType.VibratoDepth ) ) {
                                    // todo: TrackSelector_MouseDownのベジエ曲線
                                } else if ( m_selected_curve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownEnvelope( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                    if ( processMouseDownPreutteranceAndOverlap( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                } else if ( !m_selected_curve.equals( CurveType.VEL ) &&
                                            !m_selected_curve.equals( CurveType.Accent ) &&
                                            !m_selected_curve.equals( CurveType.Decay ) &&
                                            !m_selected_curve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownBezier( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                } else {
                                    m_mouse_down_mode = MouseDownMode.NONE;
                                }
                                #endregion
                            } else {
                                #region NOT CurveMode
                                if ( m_selected_curve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownEnvelope( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                    if ( processMouseDownPreutteranceAndOverlap( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                }
                                m_mouse_trace = null;
                                m_mouse_trace = new TreeMap<Integer, Integer>();
                                int x = e.X + AppManager.startToDrawX;
                                m_mouse_trace.put( x, e.Y );
                                m_mouse_trace_last_x = x;
                                m_mouse_trace_last_y = e.Y;
                                m_pencil_moved = false;

#if JAVA
                                m_mouse_hover_thread = new MouseHoverEventGeneratorProc();
                                m_mouse_hover_thread.start();
#else
                                m_mouse_hover_thread = new Thread( new ThreadStart( MouseHoverEventGenerator ) );
                                m_mouse_hover_thread.Start();
#endif
                                #endregion
                            }
                            #endregion
                        } else if ( AppManager.getSelectedTool() == EditTool.ARROW ) {
                            #region Arrow
                            boolean found = false;
                            if ( m_selected_curve.isScalar() || m_selected_curve.isAttachNote() ) {
                                if ( m_selected_curve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownEnvelope( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                    if ( processMouseDownPreutteranceAndOverlap( e ) ) {
                                        invalidate();
                                        return;
                                    }
                                }
                                m_mouse_down_mode = MouseDownMode.NONE;
                            } else {
                                // まずベジエ曲線の点にヒットしてないかどうかを検査
                                Vector<BezierChain> dict = AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).get( m_selected_curve );
                                AppManager.clearSelectedBezier();
                                for ( int i = 0; i < dict.size(); i++ ) {
                                    BezierChain bc = dict.get( i );
                                    for ( Iterator<BezierPoint> itr = bc.points.iterator(); itr.hasNext(); ) {
                                        BezierPoint bp = itr.next();
                                        Point pt = getScreenCoord( bp.getBase() );
                                        Rectangle rc = new Rectangle( pt.x - px_shift, pt.y - px_shift, px_width, px_width );
                                        if ( isInRect( e.X, e.Y, rc ) ) {
                                            AppManager.addSelectedBezier( new SelectedBezierPoint( bc.id, bp.getID(), BezierPickedSide.BASE, bp ) );
                                            m_editing_bezier_original = (BezierChain)bc.clone();
                                            found = true;
                                            break;
                                        }

                                        if ( bp.getControlLeftType() != BezierControlType.None ) {
                                            pt = getScreenCoord( bp.getControlLeft() );
                                            rc = new Rectangle( pt.x - px_shift, pt.y - px_shift, px_width, px_width );
                                            if ( isInRect( e.X, e.Y, rc ) ) {
                                                AppManager.addSelectedBezier( new SelectedBezierPoint( bc.id, bp.getID(), BezierPickedSide.LEFT, bp ) );
                                                m_editing_bezier_original = (BezierChain)bc.clone();
                                                found = true;
                                                break;
                                            }
                                        }

                                        if ( bp.getControlRightType() != BezierControlType.None ) {
                                            pt = getScreenCoord( bp.getControlRight() );
                                            rc = new Rectangle( pt.x - px_shift, pt.y - px_shift, px_width, px_width );
                                            if ( isInRect( e.X, e.Y, rc ) ) {
                                                AppManager.addSelectedBezier( new SelectedBezierPoint( bc.id, bp.getID(), BezierPickedSide.RIGHT, bp ) );
                                                m_editing_bezier_original = (BezierChain)bc.clone();
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
                                    m_mouse_down_mode = MouseDownMode.BEZIER_MODE;
                                }
                            }

                            // ベジエ曲線の点にヒットしなかった場合
                            if ( !found ) {
                                #region NOT CurveMode
                                VsqEvent ve = findItemAt( e.X, e.Y );
                                // マウス位置の音符アイテムを検索
                                if ( ve != null ) {
                                    boolean found2 = false;
                                    if ( (m_modifier_on_mouse_down & m_modifier_key) == m_modifier_key ) {
                                        // clicked with CTRL key
                                        Vector<Integer> list = new Vector<Integer>();
                                        for ( Iterator<SelectedEventEntry> itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                                            SelectedEventEntry item = itr.next();
                                            VsqEvent ve2 = item.original;
                                            if ( ve.InternalID == ve2.InternalID ) {
                                                found2 = true;
                                            } else {
                                                list.add( ve2.InternalID );
                                            }
                                        }
                                        AppManager.clearSelectedEvent();
                                        AppManager.addSelectedEventAll( list );
                                    } else if ( (PortUtil.getCurrentModifierKey() & InputEvent.SHIFT_MASK) == InputEvent.SHIFT_MASK ) {
                                        // clicked with Shift key
                                        SelectedEventEntry last_selected = AppManager.getLastSelectedEvent();
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
                                            AppManager.addSelectedEventAll( add_required );
                                        }
                                    } else {
                                        // no modefier key
                                        if ( !AppManager.isSelectedEventContains( AppManager.getSelected(), ve.InternalID ) ) {
                                            AppManager.clearSelectedEvent();
                                        }
                                    }
                                    if ( !found2 ) {
                                        AppManager.addSelectedEvent( ve.InternalID );
                                    }

                                    m_mouse_down_mode = MouseDownMode.VEL_WAIT_HOVER;
                                    m_veledit_last_selectedid = ve.InternalID;
                                    if ( m_selected_curve.equals( CurveType.VEL ) ) {
                                        m_veledit_shifty = e.Y - yCoordFromValue( ve.ID.Dynamics );
                                    } else if ( m_selected_curve.equals( CurveType.Accent ) ) {
                                        m_veledit_shifty = e.Y - yCoordFromValue( ve.ID.DEMaccent );
                                    } else if ( m_selected_curve.equals( CurveType.Decay ) ) {
                                        m_veledit_shifty = e.Y - yCoordFromValue( ve.ID.DEMdecGainRate );
                                    }
                                    m_veledit_selected.clear();
                                    if ( AppManager.isSelectedEventContains( AppManager.getSelected(), m_veledit_last_selectedid ) ) {
                                        for ( Iterator<SelectedEventEntry> itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
                                            SelectedEventEntry item = itr.next();
                                            m_veledit_selected.put( item.original.InternalID,
                                                                    new SelectedEventEntry( AppManager.getSelected(),
                                                                                            item.original,
                                                                                            item.editing ) );
                                        }
                                    } else {
                                        m_veledit_selected.put( m_veledit_last_selectedid,
                                                                new SelectedEventEntry( AppManager.getSelected(),
                                                                                        (VsqEvent)ve.clone(),
                                                                                        (VsqEvent)ve.clone() ) );
                                    }
#if JAVA
                                    m_mouse_hover_thread = new MouseHoverEventGeneratorProc();
                                    m_mouse_hover_thread.start();
#else
                                    m_mouse_hover_thread = new Thread( new ThreadStart( MouseHoverEventGenerator ) );
                                    m_mouse_hover_thread.Start();
#endif
                                    invalidate();
                                    return;
                                }

                                // マウス位置のデータポイントを検索
                                long id = findDataPointAt( e.X, e.Y );
                                if ( id > 0 ) {
                                    if ( AppManager.isSelectedPointContains( id ) ) {
                                        if ( (m_modifier_on_mouse_down & m_modifier_key) == m_modifier_key ) {
                                            AppManager.removeSelectedPoint( id );
                                            m_mouse_down_mode = MouseDownMode.NONE;
                                            invalidate();
                                            return;
                                        }
                                    } else {
                                        if ( (m_modifier_on_mouse_down & m_modifier_key) != m_modifier_key ) {
                                            AppManager.clearSelectedPoint();
                                        }
                                        AppManager.addSelectedPoint( m_selected_curve, id );
                                    }

                                    m_mouse_down_mode = MouseDownMode.POINT_MOVE;
                                    m_moving_points.clear();
                                    VsqBPList list = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( m_selected_curve.getName() );
                                    if ( list != null ) {
                                        int count = list.size();
                                        for ( int i = 0; i < count; i++ ) {
                                            VsqBPPair item = list.getElementB( i );
                                            if ( AppManager.isSelectedPointContains( item.id ) ) {
                                                m_moving_points.add( new BPPair( list.getKeyClock( i ), item.value ) );
                                            }
                                        }
                                        invalidate();
                                        return;
                                    }
                                } else {
                                    if ( (m_modifier_on_mouse_down & InputEvent.CTRL_MASK) != InputEvent.CTRL_MASK ) {
                                        AppManager.clearSelectedPoint();
                                    }
                                    if ( (m_modifier_on_mouse_down & InputEvent.SHIFT_MASK) != InputEvent.SHIFT_MASK && (m_modifier_on_mouse_down & m_modifier_key) != m_modifier_key ) {
                                        AppManager.clearSelectedPoint();
                                    }
                                }

                                if ( (m_modifier_on_mouse_down & m_modifier_key) != m_modifier_key ) {
                                    AppManager.setCurveSelectedIntervalEnabled( false );
                                }
                                if ( AppManager.editorConfig.CurveSelectingQuantized ) {
                                    AppManager.curveSelectingRectangle = new Rectangle( quantized_clock, value, 0, 0 );
                                } else {
                                    AppManager.curveSelectingRectangle = new Rectangle( clock, value, 0, 0 );
                                }
                                #endregion
                            }
                            #endregion
                        } else if ( AppManager.getSelectedTool() == EditTool.ERASER ) {
                            #region Eraser
                            VsqEvent ve3 = findItemAt( e.X, e.Y );
                            if ( ve3 != null ) {
                                AppManager.clearSelectedEvent();
                                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventDelete( selected,
                                                                                                                  ve3.InternalID ) );
                                executeCommand( run, true );
                            } else {
                                if ( AppManager.isCurveMode() ) {
                                    Vector<BezierChain> list = vsq.AttachedCurves.get( AppManager.getSelected() - 1 ).get( m_selected_curve );
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
                                                                                                                       m_selected_curve,
                                                                                                                       chain.value.id,
                                                                                                                       work,
                                                                                                                       AppManager.editorConfig.getControlCurveResolutionValue() );
                                                    executeCommand( run, true );
                                                    m_mouse_down_mode = MouseDownMode.NONE;
                                                    invalidate();
                                                    return;
                                                } else {
                                                    // 1個しかデータ点がないので、BezierChainを削除
                                                    CadenciiCommand run = VsqFileEx.generateCommandDeleteBezierChain( AppManager.getSelected(),
                                                                                                                      m_selected_curve,
                                                                                                                      chain.value.id,
                                                                                                                      AppManager.editorConfig.getControlCurveResolutionValue() );
                                                    executeCommand( run, true );
                                                    m_mouse_down_mode = MouseDownMode.NONE;
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
                                                                                                                   m_selected_curve,
                                                                                                                   chain.value.id,
                                                                                                                   work,
                                                                                                                   AppManager.editorConfig.getControlCurveResolutionValue() );
                                                executeCommand( run, true );
                                                m_mouse_down_mode = MouseDownMode.NONE;
                                                invalidate();
                                                return;
                                            }
                                        }
                                    }
                                } else {
                                    long id = findDataPointAt( e.X, e.Y );
                                    if ( id > 0 ) {
                                        VsqBPList item = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( m_selected_curve.getName() );
                                        if ( item != null ) {
                                            VsqBPList work = (VsqBPList)item.clone();
                                            VsqBPPairSearchContext context = work.findElement( id );
                                            if ( context.point.id == id ) {
                                                work.remove( context.clock );
                                                CadenciiCommand run = new CadenciiCommand(
                                                    VsqCommand.generateCommandTrackCurveReplace( selected,
                                                                                                 m_selected_curve.getName(),
                                                                                                 work ) );
                                                executeCommand( run, true );
                                                m_mouse_down_mode = MouseDownMode.NONE;
                                                invalidate();
                                                return;
                                            }
                                        }
                                    }
                                }

                                if ( (m_modifier_on_mouse_down & InputEvent.SHIFT_MASK) != InputEvent.SHIFT_MASK && (m_modifier_on_mouse_down & m_modifier_key) != m_modifier_key ) {
                                    AppManager.clearSelectedPoint();
                                }
                                if ( AppManager.editorConfig.CurveSelectingQuantized ) {
                                    AppManager.curveSelectingRectangle = new Rectangle( quantized_clock, value, 0, 0 );
                                } else {
                                    AppManager.curveSelectingRectangle = new Rectangle( clock, value, 0, 0 );
                                }
                            }
                            #endregion
                        }
                    } else if ( e.Button == BMouseButtons.Right ) {
                        if ( AppManager.isCurveMode() ) {
                            if ( !m_selected_curve.equals( CurveType.VEL ) && !m_selected_curve.equals( CurveType.Env ) ) {
                                Vector<BezierChain> dict = AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).get( m_selected_curve );
                                AppManager.clearSelectedBezier();
                                boolean found = false;
                                for ( int i = 0; i < dict.size(); i++ ) {
                                    BezierChain bc = dict.get( i );
                                    for ( Iterator<BezierPoint> itr = bc.points.iterator(); itr.hasNext(); ) {
                                        BezierPoint bp = itr.next();
                                        Point pt = getScreenCoord( bp.getBase() );
                                        Rectangle rc = new Rectangle( pt.x - DOT_WID, pt.y - DOT_WID, 2 * DOT_WID + 1, 2 * DOT_WID + 1 );
                                        if ( isInRect( e.X, e.Y, rc ) ) {
                                            AppManager.addSelectedBezier( new SelectedBezierPoint( bc.id, bp.getID(), BezierPickedSide.BASE, bp ) );
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


        private boolean processMouseDownBezier( BMouseEventArgs e ) {
            //, int clock, int value, int px_shift, int px_width, Keys modifier ) {
            int clock = AppManager.clockFromXCoord( e.X );
            int max = m_selected_curve.getMaximum();
            int min = m_selected_curve.getMinimum();
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
            Vector<BezierChain> dict = AppManager.getVsqFile().AttachedCurves.get( track - 1 ).get( m_selected_curve );
            ByRef<BezierChain> found_chain = new ByRef<BezierChain>();
            ByRef<BezierPoint> found_point = new ByRef<BezierPoint>();
            ByRef<BezierPickedSide> found_side = new ByRef<BezierPickedSide>();
            findBezierPointAt( e.X, e.Y, dict, found_chain, found_point, found_side, DOT_WID, AppManager.editorConfig.PxToleranceBezier );

            if ( found_chain.value != null ) {
                AppManager.addSelectedBezier( new SelectedBezierPoint( found_chain.value.id, found_point.value.getID(), found_side.value, found_point.value ) );
                m_editing_bezier_original = (BezierChain)found_chain.value.clone();
                m_mouse_down_mode = MouseDownMode.BEZIER_MODE;
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
                    if ( (modifier & m_modifier_key) != m_modifier_key && target_chain == null ) {
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
                        chain_id = AppManager.getVsqFile().AttachedCurves.get( track - 1 ).getNextId( m_selected_curve );
#if DEBUG
                        AppManager.debugWriteLine( "    new chain_id=" + chain_id );
#endif
                        CadenciiCommand run = VsqFileEx.generateCommandAddBezierChain( track,
                                                                                m_selected_curve,
                                                                                chain_id,
                                                                                AppManager.editorConfig.getControlCurveResolutionValue(),
                                                                                adding );
                        executeCommand( run, false );
                        m_mouse_down_mode = MouseDownMode.BEZIER_ADD_NEW;
                    } else {
                        m_editing_bezier_original = (BezierChain)target_chain.clone();
                        bp = new BezierPoint( pt, pt, pt );
                        point_id = target_chain.getNextId();
                        bp.setID( point_id );
                        target_chain.add( bp );
                        Collections.sort( target_chain.points );
                        chain_id = target_chain.id;
                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( track,
                                                                                    m_selected_curve,
                                                                                    target_chain.id,
                                                                                    target_chain,
                                                                                    AppManager.editorConfig.getControlCurveResolutionValue() );
                        executeCommand( run, false );
                        m_mouse_down_mode = MouseDownMode.BEZIER_EDIT;
                    }
                    AppManager.clearSelectedBezier();
                    AppManager.addSelectedBezier( new SelectedBezierPoint( chain_id, point_id, BezierPickedSide.BASE, bp ) );
                } else {
                    m_mouse_down_mode = MouseDownMode.NONE;
                }
            }
            return true;
        }

        private boolean processMouseDownPreutteranceAndOverlap( BMouseEventArgs e ) {
            if ( m_preutterance_viewing >= 0 && isInRect( e.X, e.Y, m_preutterance_bounds ) ) {
                m_preutterance_moving_id = m_preutterance_viewing;
                m_pre_ovl_editing = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).findEventFromID( m_preutterance_moving_id );
                if ( m_pre_ovl_editing == null ) {
                    m_mouse_down_mode = MouseDownMode.NONE;
                    return false;
                }
                m_pre_ovl_original = (VsqEvent)m_pre_ovl_editing.clone();
                m_mouse_down_mode = MouseDownMode.PRE_UTTERANCE_MOVE;
                return true;
            }
            if ( m_overlap_viewing >= 0 && isInRect( e.X, e.Y, m_overlap_bounds ) ) {
                m_overlap_moving_id = m_overlap_viewing;
                m_pre_ovl_editing = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).findEventFromID( m_overlap_moving_id );
                if ( m_pre_ovl_editing == null ) {
                    m_mouse_down_mode = MouseDownMode.NONE;
                    return false;
                }
                m_pre_ovl_original = (VsqEvent)m_pre_ovl_editing.clone();
                m_mouse_down_mode = MouseDownMode.OVERLAP_MOVE;
                return true;
            }
            return false;
        }

        private boolean processMouseDownEnvelope( BMouseEventArgs e ) {
            ByRef<Integer> internal_id = new ByRef<Integer>( -1 );
            ByRef<Integer> point_kind = new ByRef<Integer>( -1 );
            if ( !findEnvelopePointAt( e.X, e.Y, internal_id, point_kind ) ) {
#if DEBUG
                PortUtil.println( "processTrackSelectorMouseDownForEnvelope; not found" );
#endif
                return false;
            }
            m_envelope_original = null;
            VsqTrack target = AppManager.getVsqFile().Track.get( AppManager.getSelected() );
            VsqEvent item = target.findEventFromID( internal_id.value );
            if ( item == null ) {
                return false;
            }
            if ( item != null && item.UstEvent != null && item.UstEvent.Envelope != null ) {
                m_envelope_original = (UstEnvelope)item.UstEvent.Envelope.clone();
                m_envelope_editing = item.UstEvent.Envelope;
            }
            if ( m_envelope_original == null ) {
                item.UstEvent.Envelope = new UstEnvelope();
                m_envelope_editing = item.UstEvent.Envelope;
                m_envelope_original = (UstEnvelope)item.UstEvent.Envelope.clone();
            }
            m_mouse_down_mode = MouseDownMode.ENVELOPE_MOVE;
            m_envelope_move_id = internal_id.value;
            m_envelope_point_kind = point_kind.value;

            // エンベロープ点が移動可能な範囲を、あらかじめ取得
            VsqEvent next = null;
            int count = target.getEventCount();
            for ( int i = 0; i < count; i++ ) {
                if ( target.getEvent( i ).InternalID == m_envelope_move_id ) {
                    for ( int j = i + 1; j < count; j++ ) {
                        if ( target.getEvent( j ).ID.type == VsqIDType.Anote ) {
                            next = target.getEvent( j );
                            break;
                        }
                    }
                    break;
                }
            }
            double sec = AppManager.getVsqFile().getSecFromClock( AppManager.clockFromXCoord( e.X ) );
            m_envelope_range_begin = AppManager.getVsqFile().getSecFromClock( item.Clock ) - item.UstEvent.PreUtterance / 1000.0;
            m_envelope_range_end = AppManager.getVsqFile().getSecFromClock( item.Clock + item.ID.getLength() );
            if ( next != null ) {
                double sec_start_next = AppManager.getVsqFile().getSecFromClock( next.Clock ) - next.UstEvent.PreUtterance / 1000.0 + next.UstEvent.VoiceOverlap / 1000.0;
                if ( sec_start_next < m_envelope_range_end ) {
                    m_envelope_range_end = sec_start_next;
                }
            }
            if ( m_envelope_point_kind == 1 ) {
                m_envelope_dot_begin = m_envelope_range_begin;
                m_envelope_dot_end = m_envelope_range_end - (m_envelope_original.p4 + m_envelope_original.p3 + m_envelope_original.p5 + m_envelope_original.p2) / 1000.0;
            } else if ( m_envelope_point_kind == 2 ) {
                m_envelope_dot_begin = m_envelope_range_begin + m_envelope_original.p1 / 1000.0;
                m_envelope_dot_end = m_envelope_range_end - (m_envelope_original.p4 + m_envelope_original.p3 + m_envelope_original.p5) / 1000.0;
            } else if ( m_envelope_point_kind == 3 ) {
                m_envelope_dot_begin = m_envelope_range_begin + (m_envelope_original.p1 + m_envelope_original.p2) / 1000.0;
                m_envelope_dot_end = m_envelope_range_end - (m_envelope_original.p4 + m_envelope_original.p3) / 1000.0;
            } else if ( m_envelope_point_kind == 4 ) {
                m_envelope_dot_begin = m_envelope_range_begin + (m_envelope_original.p1 + m_envelope_original.p2 + m_envelope_original.p5) / 1000.0;
                m_envelope_dot_end = m_envelope_range_end - m_envelope_original.p4 / 1000.0;
            } else if ( m_envelope_point_kind == 5 ) {
                m_envelope_dot_begin = m_envelope_range_begin + (m_envelope_original.p1 + m_envelope_original.p2 + m_envelope_original.p5 + m_envelope_original.p3) / 1000.0;
                m_envelope_dot_end = m_envelope_range_end;
            }
            return true;
        }

        private void changeCurve( CurveType curve ) {
            if ( !m_selected_curve.equals( curve ) ) {
                m_last_selected_curve = m_selected_curve;
                m_selected_curve = curve;
#if JAVA
                try{
                    selectedCurveChangedEvent.raise( this, curve );
                }catch( Exception ex ){
                    System.err.println( "TrackSelector#changeCurve; ex=" + ex );
                }
#else
                if ( SelectedCurveChanged != null ) {
                    SelectedCurveChanged( this, curve );
                }
#endif
            }
        }

        private static boolean isInRect( int x, int y, Rectangle rc ) {
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
        private VsqEvent findItemAt( int locx, int locy ) {
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
                    if ( m_selected_curve.equals( CurveType.VEL ) ) {
                        y = yCoordFromValue( ve.ID.Dynamics );
                    } else if ( m_selected_curve.equals( CurveType.Accent ) ) {
                        y = yCoordFromValue( ve.ID.DEMaccent );
                    } else if ( m_selected_curve.equals( CurveType.Decay ) ) {
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

        public void TrackSelector_MouseUp( Object sender, BMouseEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "TrackSelector_MouseUp" );
#endif
            m_mouse_downed = false;
            if ( m_mouse_hover_thread != null ) {
#if JAVA
                if( m_mouse_hover_thread.isAlive() ){
                    m_mouse_hover_thread.stop();
                }
#else
                if ( m_mouse_hover_thread.IsAlive ) {
                    m_mouse_hover_thread.Abort();
                }
#endif
            }

            if ( !m_curve_visible ) {
                m_mouse_down_mode = MouseDownMode.NONE;
                invalidate();
                return;
            }

            int selected = AppManager.getSelected();

            int max = m_selected_curve.getMaximum();
            int min = m_selected_curve.getMinimum();
            VsqFileEx vsq = AppManager.getVsqFile();
            VsqTrack vsq_track = vsq.Track.get( selected );
#if DEBUG
            AppManager.debugWriteLine( "    max,min=" + max + "," + min );
#endif
            if ( m_mouse_down_mode == MouseDownMode.BEZIER_ADD_NEW ||
                 m_mouse_down_mode == MouseDownMode.BEZIER_MODE ||
                 m_mouse_down_mode == MouseDownMode.BEZIER_EDIT ) {
                if ( e.Button == BMouseButtons.Left && sender is TrackSelector ) {
                    int chain_id = AppManager.getLastSelectedBezier().chainID;
                    BezierChain edited = (BezierChain)vsq.AttachedCurves.get( selected - 1 ).getBezierChain( m_selected_curve, chain_id ).clone();
                    if ( m_mouse_down_mode == MouseDownMode.BEZIER_ADD_NEW ) {
                        edited.id = chain_id;
                        CadenciiCommand pre = VsqFileEx.generateCommandDeleteBezierChain( selected,
                                                                                          m_selected_curve,
                                                                                          chain_id,
                                                                                          AppManager.editorConfig.getControlCurveResolutionValue() );
                        executeCommand( pre, false );
                        CadenciiCommand run = VsqFileEx.generateCommandAddBezierChain( selected,
                                                                                       m_selected_curve,
                                                                                       chain_id,
                                                                                       AppManager.editorConfig.getControlCurveResolutionValue(),
                                                                                       edited );
                        executeCommand( run, true );
                    } else if ( m_mouse_down_mode == MouseDownMode.BEZIER_EDIT ) {
                        CadenciiCommand pre = VsqFileEx.generateCommandReplaceBezierChain( selected,
                                                                                           m_selected_curve,
                                                                                           chain_id,
                                                                                           m_editing_bezier_original,
                                                                                           AppManager.editorConfig.getControlCurveResolutionValue() );
                        executeCommand( pre, false );
                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( selected,
                                                                                           m_selected_curve,
                                                                                           chain_id,
                                                                                           edited,
                                                                                           AppManager.editorConfig.getControlCurveResolutionValue() );
                        executeCommand( run, true );
                    } else if ( m_mouse_down_mode == MouseDownMode.BEZIER_MODE && m_mouse_moved ) {
                        vsq.AttachedCurves.get( selected - 1 ).setBezierChain( m_selected_curve, chain_id, m_editing_bezier_original );
                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( selected,
                                                                                           m_selected_curve,
                                                                                           chain_id,
                                                                                           edited,
                                                                                           AppManager.editorConfig.getControlCurveResolutionValue() );
                        executeCommand( run, true );
#if DEBUG
                        AppManager.debugWriteLine( "    m_mouse_down_mode=" + m_mouse_down_mode );
                        AppManager.debugWriteLine( "    chain_id=" + chain_id );
#endif

                    }
                }
            } else if ( m_mouse_down_mode == MouseDownMode.CURVE_EDIT ||
                      m_mouse_down_mode == MouseDownMode.VEL_WAIT_HOVER ) {
                if ( e.Button == BMouseButtons.Left ) {
                    if ( AppManager.getSelectedTool() == EditTool.ARROW ) {
                        #region Arrow
                        if ( m_selected_curve.equals( CurveType.Env ) ) {

                        } else if ( !m_selected_curve.equals( CurveType.VEL ) && !m_selected_curve.equals( CurveType.Accent ) && !m_selected_curve.equals( CurveType.Decay ) ) {
                            if ( AppManager.curveSelectingRectangle.width == 0 ) {
                                AppManager.setCurveSelectedIntervalEnabled( false );
                            } else {
                                if ( !AppManager.isCurveSelectedIntervalEnabled() ) {
                                    int start = Math.Min( AppManager.curveSelectingRectangle.x, AppManager.curveSelectingRectangle.x + AppManager.curveSelectingRectangle.width );
                                    int end = Math.Max( AppManager.curveSelectingRectangle.x, AppManager.curveSelectingRectangle.x + AppManager.curveSelectingRectangle.width );
                                    AppManager.curveSelectedInterval = new SelectedRegion( start );
                                    AppManager.curveSelectedInterval.setEnd( end );
#if DEBUG
                                    AppManager.debugWriteLine( "TrackSelector#TrackSelector_MouseUp; selected_region is set to TRUE" );
#endif
                                    AppManager.setCurveSelectedIntervalEnabled( true );
                                } else {
                                    int start = Math.Min( AppManager.curveSelectingRectangle.x, AppManager.curveSelectingRectangle.x + AppManager.curveSelectingRectangle.width );
                                    int end = Math.Max( AppManager.curveSelectingRectangle.x, AppManager.curveSelectingRectangle.x + AppManager.curveSelectingRectangle.width );
                                    int old_start = AppManager.curveSelectedInterval.getStart();
                                    int old_end = AppManager.curveSelectedInterval.getEnd();
                                    AppManager.curveSelectedInterval = new SelectedRegion( Math.Min( start, old_start ) );
                                    AppManager.curveSelectedInterval.setEnd( Math.Max( end, old_end ) );
                                }

                                if ( (m_modifier_on_mouse_down & InputEvent.CTRL_MASK) != InputEvent.CTRL_MASK ) {
#if DEBUG
                                    PortUtil.println( "TrackSelector#TrackSelector_MouseUp; CTRL was not pressed" );
#endif
                                    AppManager.clearSelectedPoint();
                                }
                                if ( !m_selected_curve.equals( CurveType.Accent ) &&
                                     !m_selected_curve.equals( CurveType.Decay ) &&
                                     !m_selected_curve.equals( CurveType.Env ) &&
                                     !m_selected_curve.equals( CurveType.VEL ) &&
                                     !m_selected_curve.equals( CurveType.VibratoDepth ) &&
                                     !m_selected_curve.equals( CurveType.VibratoRate ) ) {
                                    VsqBPList list = vsq_track.getCurve( m_selected_curve.getName() );
                                    int count = list.size();
                                    Rectangle rc = new Rectangle( Math.Min( AppManager.curveSelectingRectangle.x, AppManager.curveSelectingRectangle.x + AppManager.curveSelectingRectangle.width ),
                                                                  Math.Min( AppManager.curveSelectingRectangle.y, AppManager.curveSelectingRectangle.y + AppManager.curveSelectingRectangle.height ),
                                                                  Math.Abs( AppManager.curveSelectingRectangle.width ),
                                                                  Math.Abs( AppManager.curveSelectingRectangle.height ) );
#if DEBUG
                                    PortUtil.println( "TrackSelectro#TrackSelectro_MouseUp; rc={x=" + rc.x + ", y=" + rc.y + ", width=" + rc.width + ", height=" + rc.height + "}" );
#endif
                                    for ( int i = 0; i < count; i++ ) {
                                        int clock = list.getKeyClock( i );
                                        VsqBPPair item = list.getElementB( i );
                                        if ( isInRect( clock, item.value, rc ) ) {
#if DEBUG
                                            PortUtil.println( "TrackSelector#TrackSelectro_MosueUp; selected; clock=" + clock + "; id=" + item.id );
#endif
                                            AppManager.addSelectedPoint( m_selected_curve, item.id );
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    } else if ( AppManager.getSelectedTool() == EditTool.ERASER ) {
                        #region Eraser
                        if ( AppManager.isCurveMode() ) {
                            Vector<BezierChain> list = vsq.AttachedCurves.get( selected - 1 ).get( m_selected_curve );
                            if ( list != null ) {
                                int x = Math.Min( AppManager.curveSelectingRectangle.x, AppManager.curveSelectingRectangle.x + AppManager.curveSelectingRectangle.width );
                                int y = Math.Min( AppManager.curveSelectingRectangle.y, AppManager.curveSelectingRectangle.y + AppManager.curveSelectingRectangle.height );
                                Rectangle rc = new Rectangle( x, y, Math.Abs( AppManager.curveSelectingRectangle.width ), Math.Abs( AppManager.curveSelectingRectangle.height ) );

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
                                    comm.put( m_selected_curve, work );
                                    CadenciiCommand run = VsqFileEx.generateCommandReplaceAttachedCurveRange( selected, comm );
                                    executeCommand( run, true );
                                }
                            }
                        } else {
                            if ( m_selected_curve.equals( CurveType.VEL ) || m_selected_curve.equals( CurveType.Accent ) || m_selected_curve.equals( CurveType.Decay ) ) {
                                #region VEL Accent Delay
                                int start = Math.Min( AppManager.curveSelectingRectangle.x, AppManager.curveSelectingRectangle.x + AppManager.curveSelectingRectangle.width );
                                int end = Math.Max( AppManager.curveSelectingRectangle.x, AppManager.curveSelectingRectangle.x + AppManager.curveSelectingRectangle.width );
                                int old_start = AppManager.curveSelectedInterval.getStart();
                                int old_end = AppManager.curveSelectedInterval.getEnd();
                                AppManager.curveSelectedInterval = new SelectedRegion( Math.Min( start, old_start ) );
                                AppManager.curveSelectedInterval.setEnd( Math.Max( end, old_end ) );
                                AppManager.clearSelectedEvent();
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
                            } else if ( m_selected_curve.equals( CurveType.VibratoRate ) || m_selected_curve.equals( CurveType.VibratoDepth ) ) {
                                #region VibratoRate ViratoDepth
                                int er_start = Math.Min( AppManager.curveSelectingRectangle.x, AppManager.curveSelectingRectangle.x + AppManager.curveSelectingRectangle.width );
                                int er_end = Math.Max( AppManager.curveSelectingRectangle.x, AppManager.curveSelectingRectangle.x + AppManager.curveSelectingRectangle.width );
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
                                        if ( m_selected_curve.equals( CurveType.VibratoDepth ) ) {
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
                                        if ( m_selected_curve.equals( CurveType.VibratoDepth ) ) {
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
                            } else if ( m_selected_curve.equals( CurveType.Env ) ) {

                            } else {
                                #region Other Curves
                                VsqBPList work = vsq_track.getCurve( m_selected_curve.getName() );

                                // 削除するべきデータ点のリストを作成
                                int x = Math.Min( AppManager.curveSelectingRectangle.x, AppManager.curveSelectingRectangle.x + AppManager.curveSelectingRectangle.width );
                                int y = Math.Min( AppManager.curveSelectingRectangle.y, AppManager.curveSelectingRectangle.y + AppManager.curveSelectingRectangle.height );
                                Rectangle rc = new Rectangle( x, y, Math.Abs( AppManager.curveSelectingRectangle.width ), Math.Abs( AppManager.curveSelectingRectangle.height ) );
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
                                        VsqCommand.generateCommandTrackCurveEdit2( selected, m_selected_curve.getName(), delete, new TreeMap<Integer, VsqBPPair>() ) );
                                    executeCommand( run_eraser, true );
                                }
                                #endregion
                            }
                        }
                        #endregion
                    } else if ( !AppManager.isCurveMode() && AppManager.getSelectedTool() == EditTool.PENCIL ) {
                        #region Pencil
#if DEBUG
                        AppManager.debugWriteLine( "    Pencil" );
                        AppManager.debugWriteLine( "    m_selected_curve=" + m_selected_curve );
                        PortUtil.println( "TrackSelector_MouseUp; m_pencil_moved=" + m_pencil_moved );
#endif
                        if ( m_pencil_moved ) {
                            int stdx = AppManager.startToDrawX;
                            if ( m_selected_curve.equals( CurveType.VEL ) || m_selected_curve.equals( CurveType.Accent ) || m_selected_curve.equals( CurveType.Decay ) ) {
                                #region VEL Accent Decay
                                int start = m_mouse_trace.firstKey();
                                int end = m_mouse_trace.lastKey();
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
                                        int count = m_mouse_trace.size();
                                        for ( Iterator<Integer> itr2 = m_mouse_trace.keySet().iterator(); itr2.hasNext(); ) {
                                            i++;
                                            int key = itr2.next();
                                            if ( i == 0 ) {
                                                lkey = key;
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
                                                int key0_value = valueFromYCoord( m_mouse_trace.get( key0 ) );
                                                int key1_value = valueFromYCoord( m_mouse_trace.get( key1 ) );
                                                float a = (key1_value - key0_value) / (float)(key1_clock - key0_clock);
                                                float b = key0_value - a * key0_clock;
                                                int new_value = (int)(a * ve.Clock + b);
                                                velocity.put( ve.InternalID, new_value );
                                            } else if ( key0_clock == ve.Clock ) {
                                                velocity.put( ve.InternalID, valueFromYCoord( m_mouse_trace.get( key0 ) ) );
                                            } else if ( key1_clock == ve.Clock ) {
                                                velocity.put( ve.InternalID, valueFromYCoord( m_mouse_trace.get( key1 ) ) );
                                            }
                                            lkey = key;
                                        }
                                    }
                                }
                                if ( velocity.size() > 0 ) {
                                    Vector<ValuePair<Integer, Integer>> cpy = new Vector<ValuePair<Integer, Integer>>();
                                    for ( Iterator<Integer> itr = velocity.keySet().iterator(); itr.hasNext(); ) {
                                        int key = itr.next();
                                        int value = (Integer)velocity.get( key );
                                        cpy.add( new ValuePair<Integer, Integer>( key, value ) );
                                    }
                                    CadenciiCommand run = null;
                                    if ( m_selected_curve.equals( CurveType.VEL ) ) {
                                        run = new CadenciiCommand( VsqCommand.generateCommandEventChangeVelocity( selected, cpy ) );
                                    } else if ( m_selected_curve.equals( CurveType.Accent ) ) {
                                        run = new CadenciiCommand( VsqCommand.generateCommandEventChangeAccent( selected, cpy ) );
                                    } else if ( m_selected_curve.equals( CurveType.Decay ) ) {
                                        run = new CadenciiCommand( VsqCommand.generateCommandEventChangeDecay( selected, cpy ) );
                                    }
                                    executeCommand( run, true );
                                }
                                #endregion
                            } else if ( m_selected_curve.equals( CurveType.VibratoRate ) || m_selected_curve.equals( CurveType.VibratoDepth ) ) {
                                #region VibratoRate || VibratoDepth
                                int step_clock = AppManager.editorConfig.getControlCurveResolutionValue();
                                int step_px = (int)(step_clock * AppManager.scaleX);
                                if ( step_px <= 0 ) {
                                    step_px = 1;
                                }
                                int start = m_mouse_trace.firstKey();
                                int end = m_mouse_trace.lastKey();
#if DEBUG
                                AppManager.debugWriteLine( "    start,end=" + start + " " + end );
#endif
                                Vector<Integer> internal_ids = new Vector<Integer>();
                                Vector<VsqID> items = new Vector<VsqID>();
                                for ( Iterator<VsqEvent> itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ve = itr.next();
                                    int cl_vib_start = ve.Clock + ve.ID.VibratoDelay;
                                    float cl_vib_length = ve.ID.getLength() - ve.ID.VibratoDelay;
                                    int vib_start = AppManager.xCoordFromClocks( cl_vib_start ) + stdx;          // 仮想スクリーン上の、ビブラートの描画開始位置
                                    int vib_end = AppManager.xCoordFromClocks( ve.Clock + ve.ID.getLength() ) + stdx; // 仮想スクリーン上の、ビブラートの描画終了位置
                                    int chk_start = Math.Max( vib_start, start );       // マウスのトレースと、ビブラートの描画範囲がオーバーラップしている部分を検出
                                    int chk_end = Math.Min( vib_end, end );
                                    float add_min = (AppManager.clockFromXCoord( chk_start - stdx ) - cl_vib_start) / cl_vib_length;
                                    float add_max = (AppManager.clockFromXCoord( chk_end - stdx ) - cl_vib_start) / cl_vib_length;
#if DEBUG
                                    AppManager.debugWriteLine( "    vib_start,vib_end=" + vib_start + " " + vib_end );
                                    AppManager.debugWriteLine( "    chk_start,chk_end=" + chk_start + " " + chk_end );
                                    AppManager.debugWriteLine( "    add_min,add_max=" + add_min + " " + add_max );
#endif
                                    if ( chk_start < chk_end ) {    // オーバーラップしている。
                                        Vector<ValuePair<Float, Integer>> edit = new Vector<ValuePair<Float, Integer>>();
                                        for ( int i = chk_start; i < chk_end; i += step_px ) {
                                            if ( m_mouse_trace.containsKey( i ) ) {
                                                edit.add( new ValuePair<Float, Integer>( (AppManager.clockFromXCoord( i - stdx ) - cl_vib_start) / cl_vib_length,
                                                                                        valueFromYCoord( m_mouse_trace.get( i ) ) ) );
                                            } else {
                                                //for ( int j = 0; j < m_mouse_trace.Keys.Count - 1; j++ ) {
                                                int j = -1;
                                                int lkey = 0;
                                                int lvalue = 0;
                                                for ( Iterator<Integer> itr2 = m_mouse_trace.keySet().iterator(); itr2.hasNext(); ) {
                                                    j++;
                                                    int key = itr2.next();
                                                    int value = m_mouse_trace.get( key );
                                                    if ( j == 0 ) {
                                                        lkey = key;
                                                        lvalue = value;
                                                        continue;
                                                    }
                                                    int key0 = lkey;
                                                    int key1 = key;
                                                    int value0 = lvalue;
                                                    int value1 = value;
                                                    if ( key0 <= i && i < key1 ) {
                                                        float a = (value1 - value0) / (float)(key1 - key0);
                                                        int newy = (int)(value0 + a * (i - key0));
                                                        int clock = AppManager.clockFromXCoord( i - stdx );
                                                        int val = valueFromYCoord( newy );
                                                        if ( val < min ) {
                                                            val = min;
                                                        } else if ( max < value ) {
                                                            val = max;
                                                        }
                                                        edit.add( new ValuePair<Float, Integer>( (clock - cl_vib_start) / cl_vib_length, val ) );
                                                        break;
                                                    }
                                                    lkey = key;
                                                    lvalue = value;
                                                }
                                            }
                                        }
                                        int c = AppManager.clockFromXCoord( end - stdx );
                                        float lastx = (c - cl_vib_start) / cl_vib_length;
                                        if ( 0 <= lastx && lastx <= 1 ) {
                                            int v = valueFromYCoord( m_mouse_trace.get( end ) );
                                            if ( v < min ) {
                                                v = min;
                                            } else if ( max < v ) {
                                                v = max;
                                            }
                                            edit.add( new ValuePair<Float, Integer>( lastx, v ) );
                                        }
                                        if ( ve.ID.VibratoHandle != null ) {
                                            VibratoBPList target = null;
                                            if ( m_selected_curve.equals( CurveType.VibratoRate ) ) {
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
                                            if ( m_selected_curve.equals( CurveType.VibratoDepth ) ) {
                                                id.VibratoHandle.setDepthBP( new VibratoBPList( bpx, bpy ) );
                                            } else {
                                                id.VibratoHandle.setRateBP( new VibratoBPList( bpx, bpy ) );
                                            }
                                            internal_ids.add( ve.InternalID );
                                            items.add( id );
                                        }
                                    }
                                }
                                if ( internal_ids.size() > 0 ) {
                                    CadenciiCommand run = new CadenciiCommand(
                                        VsqCommand.generateCommandEventChangeIDContaintsRange( selected,
                                                                                               PortUtil.convertIntArray( internal_ids.toArray( new Integer[] { } ) ),
                                                                                               items.toArray( new VsqID[] { } ) ) );
                                    executeCommand( run, true );
                                }
                                #endregion
                            } else if ( m_selected_curve.equals( CurveType.Env ) ) {
                                #region Env

                                #endregion
                            } else {
                                #region Other Curves
                                int track = selected;
                                int step_clock = AppManager.editorConfig.getControlCurveResolutionValue();
                                int step_px = (int)(step_clock * AppManager.scaleX);
                                if ( step_px <= 0 ) {
                                    step_px = 1;
                                }
                                int start = m_mouse_trace.firstKey();
                                int end = m_mouse_trace.lastKey();
                                int clock_start = AppManager.clockFromXCoord( start - stdx );
                                int clock_end = AppManager.clockFromXCoord( end - stdx );
                                int last = start;

#if DEBUG
                                PortUtil.println( "TrackSelector#TrackSelector_MouseUp; start, end=" + start + ", " + end );
#endif
                                VsqBPList list = vsq.Track.get( track ).getCurve( m_selected_curve.getName() );
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
#if DEBUG
                                AppManager.debugWriteLine( "    start=" + start );
                                AppManager.debugWriteLine( "    end=" + end );
#endif
                                int last_value = int.MinValue;
                                int index = 0;
                                for ( int i = start; i <= end; i += step_px ) {
                                    if ( m_mouse_trace.containsKey( i ) ) {
                                        int clock = AppManager.clockFromXCoord( i - stdx );
                                        int value = valueFromYCoord( m_mouse_trace.get( i ) );
                                        if ( value < min ) {
                                            value = min;
                                        } else if ( max < value ) {
                                            value = max;
                                        }
                                        if ( value != last_value ) {
                                            index++;
                                            add.put( clock, new VsqBPPair( value, maxid + index ) );
                                            last_value = value;
                                        }
                                    } else {
                                        int j = -1;
                                        int lkey = 0;
                                        int lvalue = 0;
                                        for ( Iterator<Integer> itr = m_mouse_trace.keySet().iterator(); itr.hasNext(); ) {
                                            j++;
                                            int key = itr.next();
                                            int value = m_mouse_trace.get( key );
                                            if ( j == 0 ) {
                                                lkey = key;
                                                lvalue = value;
                                                continue;
                                            }
                                            int key0 = lkey;
                                            int key1 = key;
                                            int value0 = lvalue;
                                            int value1 = value;
                                            if ( key0 <= i && i < key1 ) {
                                                float a = (m_mouse_trace.get( key1 ) - m_mouse_trace.get( key0 )) / (float)(key1 - key0);
                                                int newy = (int)(m_mouse_trace.get( key0 ) + a * (i - key0));
                                                int clock = AppManager.clockFromXCoord( i - stdx );
                                                int val = valueFromYCoord( newy, max, min );
                                                if ( val < min ) {
                                                    val = min;
                                                } else if ( max < val ) {
                                                    val = max;
                                                }
                                                if ( val != last_value ) {
                                                    index++;
                                                    add.put( clock, new VsqBPPair( value, maxid + index ) );
                                                    last_value = value;
                                                }
                                                break;
                                            }
                                            lkey = key;
                                            lvalue = value;
                                        }
                                    }
                                }
                                if ( (end - start) % step_px != 0 ) {
                                    int clock = AppManager.clockFromXCoord( end - stdx );
                                    int value = valueFromYCoord( m_mouse_trace.get( end ) );
                                    if ( value < min ) {
                                        value = min;
                                    } else if ( max < value ) {
                                        value = max;
                                    }
                                    index++;
                                    add.put( clock, new VsqBPPair( value, maxid + index ) );
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
                                    VsqCommand.generateCommandTrackCurveEdit2( track, m_selected_curve.getName(), delete, add ) );
                                executeCommand( pen_run, true );
                                #endregion
                            }
                        }
                        m_mouse_trace = null;
                        #endregion
                    } else if ( !AppManager.isCurveMode() && AppManager.getSelectedTool() == EditTool.LINE ) {
                        #region Line
                        int x0 = m_line_start.x;
                        int y0 = m_line_start.y;
                        int x1 = AppManager.clockFromXCoord( e.X );
                        int y1 = valueFromYCoord( e.Y );
                        if ( y1 < min ) {
                            y1 = min;
                        } else if ( max < y1 ) {
                            y1 = max;
                        }
                        if ( x1 < x0 ) {
                            int b = x0;
                            x0 = x1;
                            x1 = b;
                            b = y0;
                            y0 = y1;
                            y1 = b;
                        }
                        if ( x1 != x0 ) {
                            float a0 = (y1 - y0) / (float)(x1 - x0);
                            float b0 = y0 - a0 * x0;
                            if ( m_selected_curve.equals( CurveType.VEL ) || m_selected_curve.equals( CurveType.Accent ) || m_selected_curve.equals( CurveType.Decay ) ) {
                                Vector<ValuePair<Integer, Integer>> velocity = new Vector<ValuePair<Integer, Integer>>();
                                for ( Iterator<VsqEvent> itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ve = itr.next();
                                    if ( x0 <= ve.Clock && ve.Clock < x1 ) {
                                        int new_value = (int)(a0 * ve.Clock + b0);
                                        velocity.add( new ValuePair<Integer, Integer>( ve.InternalID, new_value ) );
                                    }
                                }
                                CadenciiCommand run = null;
                                if ( velocity.size() > 0 ) {
                                    if ( m_selected_curve.equals( CurveType.VEL ) ) {
                                        run = new CadenciiCommand( VsqCommand.generateCommandEventChangeVelocity( selected, velocity ) );
                                    } else if ( m_selected_curve.equals( CurveType.Accent ) ) {
                                        run = new CadenciiCommand( VsqCommand.generateCommandEventChangeAccent( selected, velocity ) );
                                    } else if ( m_selected_curve.equals( CurveType.Decay ) ) {
                                        run = new CadenciiCommand( VsqCommand.generateCommandEventChangeDecay( selected, velocity ) );
                                    }
                                }
                                if ( run != null ) {
                                    executeCommand( run, true );
                                }
                            } else if ( m_selected_curve.equals( CurveType.VibratoDepth ) || m_selected_curve.equals( CurveType.VibratoRate ) ) {
                                int stdx = AppManager.startToDrawX;
                                int step_clock = AppManager.editorConfig.getControlCurveResolutionValue();
                                int cl_start = x0;
                                int cl_end = x1;
#if DEBUG
                                AppManager.debugWriteLine( "    cl_start,cl_end=" + cl_start + " " + cl_end );
#endif
                                Vector<Integer> internal_ids = new Vector<Integer>();
                                Vector<VsqID> items = new Vector<VsqID>();
                                for ( Iterator<VsqEvent> itr = vsq_track.getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ve = itr.next();
                                    int cl_vib_start = ve.Clock + ve.ID.VibratoDelay;
                                    int cl_vib_length = ve.ID.getLength() - ve.ID.VibratoDelay;
                                    int cl_vib_end = cl_vib_start + cl_vib_length;
                                    //int vib_start = XCoordFromClocks( cl_vib_start ) + stdx;          // 仮想スクリーン上の、ビブラートの描画開始位置
                                    //int vib_end = XCoordFromClocks( ve.Clock + ve.ID.Length ) + stdx; // 仮想スクリーン上の、ビブラートの描画終了位置
                                    int chk_start = Math.Max( cl_vib_start, cl_start );       // マウスのトレースと、ビブラートの描画範囲がオーバーラップしている部分を検出
                                    int chk_end = Math.Min( cl_vib_end, cl_end );
                                    float add_min = (chk_start - cl_vib_start) / (float)cl_vib_length;
                                    float add_max = (chk_end - cl_vib_start) / (float)cl_vib_length;
#if DEBUG
                                    AppManager.debugWriteLine( "    cl_vib_start,cl_vib_end=" + cl_vib_start + " " + cl_vib_end );
                                    AppManager.debugWriteLine( "    chk_start,chk_end=" + chk_start + " " + chk_end );
                                    AppManager.debugWriteLine( "    add_min,add_max=" + add_min + " " + add_max );
#endif
                                    if ( chk_start < chk_end ) {    // オーバーラップしている。
                                        Vector<ValuePair<Float, Integer>> edit = new Vector<ValuePair<Float, Integer>>();
                                        for ( int i = chk_start; i < chk_end; i += step_clock ) {
                                            float x = (i - cl_vib_start) / (float)cl_vib_length;
                                            if ( 0 <= x && x <= 1 ) {
                                                int y = (int)(a0 * i + b0);
                                                edit.add( new ValuePair<Float, Integer>( x, y ) );
                                            }
                                        }
                                        edit.add( new ValuePair<Float, Integer>( (chk_end - cl_vib_start) / (float)cl_vib_length, (int)(a0 * chk_end + b0) ) );
                                        if ( ve.ID.VibratoHandle != null ) {
                                            VibratoBPList target = null;
                                            if ( m_selected_curve.equals( CurveType.VibratoRate ) ) {
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
                                            if ( m_selected_curve.equals( CurveType.VibratoDepth ) ) {
                                                id.VibratoHandle.setDepthBP( new VibratoBPList( bpx, bpy ) );
                                            } else {
                                                id.VibratoHandle.setRateBP( new VibratoBPList( bpx, bpy ) );
                                            }
                                            internal_ids.add( ve.InternalID );
                                            items.add( id );
                                        }
                                    }
                                }
                                if ( internal_ids.size() > 0 ) {
                                    CadenciiCommand run = new CadenciiCommand(
                                        VsqCommand.generateCommandEventChangeIDContaintsRange( selected,
                                                                                               PortUtil.convertIntArray( internal_ids.toArray( new Integer[] { } ) ),
                                                                                               items.toArray( new VsqID[] { } ) ) );
                                    executeCommand( run, true );
                                }
                            } else if ( m_selected_curve.equals( CurveType.Env ) ) {
                                // todo:
                            } else {
                                VsqBPList list = vsq_track.getCurve( m_selected_curve.getName() );
                                if ( list != null ) {
                                    int step_clock = AppManager.editorConfig.getControlCurveResolutionValue();
                                    Vector<Long> delete = new Vector<Long>();
                                    TreeMap<Integer, VsqBPPair> add = new TreeMap<Integer, VsqBPPair>();
                                    int c = list.size();
                                    for ( int i = 0; i < c; i++ ) {
                                        int cl = list.getKeyClock( i );
                                        if ( x0 <= cl && cl <= x1 ) {
                                            delete.add( list.getElementB( i ).id );
                                        } else if ( x1 < cl ) {
                                            break;
                                        }
                                    }
                                    int index = 0;
                                    long maxid = list.getMaxID();
                                    int lasty = int.MinValue;
                                    for ( int i = x0; i <= x1; i += step_clock ) {
                                        int y = (int)(a0 * i + b0);
                                        if ( lasty != y ) {
                                            index++;
                                            add.put( i, new VsqBPPair( y, maxid + index ) );
                                            lasty = y;
                                        }
                                    }
                                    CadenciiCommand run = new CadenciiCommand(
                                        VsqCommand.generateCommandTrackCurveEdit2( selected,
                                                                                   m_selected_curve.getName(),
                                                                                   delete,
                                                                                   add ) );
                                    executeCommand( run, true );
                                }
                            }
                        }
                        #endregion
                    }
                }
                m_mouse_downed = false;
            } else if ( m_mouse_down_mode == MouseDownMode.SINGER_LIST ) {
                if ( m_mouse_moved ) {
                    int count = AppManager.getSelectedEventCount();
                    if ( count > 0 ) {
                        int[] ids = new int[count];
                        int[] clocks = new int[count];
                        VsqID[] values = new VsqID[count];
                        int i = -1;
                        boolean is_valid = true;
                        boolean contains_first_singer = false;
                        int premeasure = vsq.getPreMeasureClocks();
                        for ( Iterator<SelectedEventEntry> itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
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
                                for ( Iterator<SelectedEventEntry> itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ) {
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
            } else if ( m_mouse_down_mode == MouseDownMode.VEL_EDIT ) {
                int count = m_veledit_selected.size();
                int[] ids = new int[count];
                VsqID[] values = new VsqID[count];
                int i = -1;
                for ( Iterator<Integer> itr = m_veledit_selected.keySet().iterator(); itr.hasNext(); ) {
                    int id = itr.next();
                    i++;
                    ids[i] = id;
                    values[i] = (VsqID)m_veledit_selected.get( id ).editing.ID.clone();
                }
                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventChangeIDContaintsRange( selected, ids, values ) );
                executeCommand( run, true );
                if ( m_veledit_selected.size() == 1 ) {
                    AppManager.clearSelectedEvent();
                    AppManager.addSelectedEvent( m_veledit_last_selectedid );
                }
            } else if ( m_mouse_down_mode == MouseDownMode.ENVELOPE_MOVE ) {
                m_mouse_down_mode = MouseDownMode.NONE;
                if ( m_mouse_moved ) {
                    VsqTrack target = vsq_track;
                    VsqEvent edited = (VsqEvent)target.findEventFromID( m_envelope_move_id ).clone();

                    // m_envelope_originalに，編集前のが入っているので，いったん置き換え
                    int count = target.getEventCount();
                    for ( int i = 0; i < count; i++ ) {
                        VsqEvent item = target.getEvent( i );
                        if ( item.ID.type == VsqIDType.Anote && item.InternalID == m_envelope_move_id ) {
                            item.UstEvent.Envelope = m_envelope_original;
                            target.setEvent( i, item );
                            break;
                        }
                    }

                    // コマンドを発行
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventReplace( selected,
                                                                                                       edited ) );
                    executeCommand( run, true );
                }
            } else if ( m_mouse_down_mode == MouseDownMode.PRE_UTTERANCE_MOVE ) {
                m_mouse_down_mode = MouseDownMode.NONE;
                if ( m_mouse_moved ) {
                    VsqTrack target = vsq_track;
                    VsqEvent edited = (VsqEvent)target.findEventFromID( m_preutterance_moving_id ).clone();

                    // m_envelope_originalに，編集前のが入っているので，いったん置き換え
                    int count = target.getEventCount();
                    for ( int i = 0; i < count; i++ ) {
                        VsqEvent item = target.getEvent( i );
                        if ( item.ID.type == VsqIDType.Anote && item.InternalID == m_preutterance_moving_id ) {
                            target.setEvent( i, m_pre_ovl_original );
                            break;
                        }
                    }

                    // コマンドを発行
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventReplace( selected,
                                                                                                       edited ) );
                    executeCommand( run, true );
                }
            } else if ( m_mouse_down_mode == MouseDownMode.OVERLAP_MOVE ) {
                m_mouse_down_mode = MouseDownMode.NONE;
                if ( m_mouse_moved ) {
                    VsqTrack target = vsq_track;
                    VsqEvent edited = (VsqEvent)target.findEventFromID( m_overlap_moving_id ).clone();

                    // m_envelope_originalに，編集前のが入っているので，いったん置き換え
                    int count = target.getEventCount();
                    for ( int i = 0; i < count; i++ ) {
                        VsqEvent item = target.getEvent( i );
                        if ( item.ID.type == VsqIDType.Anote && item.InternalID == m_overlap_moving_id ) {
                            target.setEvent( i, m_pre_ovl_original );
                            break;
                        }
                    }

                    // コマンドを発行
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventReplace( selected,
                                                                                                       edited ) );
                    executeCommand( run, true );
                }
            } else if ( m_mouse_down_mode == MouseDownMode.POINT_MOVE ) {
                if ( m_mouse_moved ) {
                    Point pmouse = pointToClient( PortUtil.getMousePosition() );
                    Point mouse = new Point( pmouse.x, pmouse.y );
                    int dx = mouse.x + AppManager.startToDrawX - m_mouse_down_location.x;
                    int dy = mouse.y - m_mouse_down_location.y;

                    String curve = m_selected_curve.getName();
                    VsqTrack work = (VsqTrack)vsq_track.clone();
                    VsqBPList list = work.getCurve( curve );
                    VsqBPList work_list = (VsqBPList)list.clone();
                    int min0 = list.getMinimum();
                    int max0 = list.getMaximum();
                    int count = list.size();
                    for ( int i = 0; i < count; i++ ) {
                        int clock = list.getKeyClock( i );
                        VsqBPPair item = list.getElementB( i );
                        if ( AppManager.isSelectedPointContains( item.id ) ) {
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
                m_moving_points.clear();
            }
            m_mouse_down_mode = MouseDownMode.NONE;
            invalidate();
        }

        private void TrackSelector_MouseHover( Object sender, BEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "TrackSelector_MouseHover" );
            AppManager.debugWriteLine( "    m_mouse_downed=" + m_mouse_downed );
#endif
            //if ( m_selected_curve.equals( CurveType.Accent ) || m_selected_curve.equals( CurveType.Decay ) || m_selected_curve.equals( CurveType.Env ) ) {
            if ( m_selected_curve.equals( CurveType.Env ) ) {
                return;
            }
            if ( m_mouse_downed && !m_pencil_moved && AppManager.getSelectedTool() == EditTool.PENCIL &&
                 !m_selected_curve.equals( CurveType.VEL ) ) {
                Point pmouse = pointToClient( PortUtil.getMousePosition() );
                Point mouse = new Point( pmouse.x, pmouse.y );
                int clock = AppManager.clockFromXCoord( mouse.x );
                int value = valueFromYCoord( mouse.y );
                int min = m_selected_curve.getMinimum();
                int max = m_selected_curve.getMaximum();
                if ( value < min ) {
                    value = min;
                } else if ( max < value ) {
                    value = max;
                }
                if ( m_selected_curve.equals( CurveType.VibratoRate ) || m_selected_curve.equals( CurveType.VibratoDepth ) ) {
                    // マウスの位置がビブラートの範囲かどうかを調べる
                    float x = -1f;
                    VsqID edited = null;
                    int event_id = -1;
                    for ( Iterator<VsqEvent> itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
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
                        if ( m_selected_curve.equals( CurveType.VibratoRate ) ) {
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
                                AppManager.getSelected(),
                                event_id,
                                edited ) );
                        executeCommand( run, true );
                    }
                } else {
                    VsqBPList list = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( m_selected_curve.getName() );
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
                            VsqCommand.generateCommandTrackCurveEdit2( AppManager.getSelected(),
                                                                       m_selected_curve.getName(),
                                                                       delete,
                                                                       add ) );
                        executeCommand( run, true );
                    }
                }
            } else if ( m_mouse_down_mode == MouseDownMode.VEL_WAIT_HOVER ) {
#if DEBUG
                AppManager.debugWriteLine( "    entered VelEdit" );
                AppManager.debugWriteLine( "    m_veledit_selected.Count=" + m_veledit_selected.size() );
                AppManager.debugWriteLine( "    m_veledit_last_selectedid=" + m_veledit_last_selectedid );
                AppManager.debugWriteLine( "    m_veledit_selected.ContainsKey(m_veledit_last_selectedid" + m_veledit_selected.containsKey( m_veledit_last_selectedid ) );
#endif
                m_mouse_down_mode = MouseDownMode.VEL_EDIT;
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
        private void MouseHoverEventGenerator() {
            Thread.Sleep( (int)(System.Windows.Forms.SystemInformation.MouseHoverTime * 0.8) );
            Invoke( new EventHandler( TrackSelector_MouseHover ) );
        }
#endif

        private void TrackSelector_MouseDoubleClick( Object sender, BMouseEventArgs e ) {
#if JAVA
            if( m_mouse_hover_thread != null && m_mouse_hover_thread.isAlive() ){
                m_mouse_hover_thread.stop();
            }
#else
            if ( m_mouse_hover_thread != null && m_mouse_hover_thread.IsAlive ) {
                m_mouse_hover_thread.Abort();
            }
#endif

            //int modifier = PortUtil.getCurrentModifierKey();
            if ( 0 <= e.Y && e.Y <= getHeight() - 2 * OFFSET_TRACK_TAB ) {
                #region MouseDown occured on curve-pane
                if ( AppManager.keyWidth <= e.X && e.X <= getWidth() ) {
                    if ( !m_selected_curve.equals( CurveType.VEL ) &&
                         !m_selected_curve.equals( CurveType.Accent ) &&
                         !m_selected_curve.equals( CurveType.Decay ) &&
                         !m_selected_curve.equals( CurveType.Env ) ) {
                        // ベジエデータ点にヒットしているかどうかを検査
                        int track = AppManager.getSelected();
                        int clock = AppManager.clockFromXCoord( e.X );
                        Vector<BezierChain> dict = AppManager.getVsqFile().AttachedCurves.get( track - 1 ).get( m_selected_curve );
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
                                                                m_selected_curve,
                                                                chain_id,
                                                                target_point.getID() );
                                EditingChainID = chain_id;
                                EditingPointID = target_point.getID();
                                fbpe.setModal( true );
                                fbpe.setVisible( true );
                                BDialogResult ret = fbpe.getDialogResult();
                                EditingChainID = -1;
                                EditingPointID = -1;
                                BezierChain after = AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).getBezierChain( m_selected_curve, chain_id );
                                // 編集前の状態に戻す
                                CadenciiCommand revert = VsqFileEx.generateCommandReplaceBezierChain( track,
                                                                                               m_selected_curve,
                                                                                               chain_id,
                                                                                               before,
                                                                                               AppManager.editorConfig.getControlCurveResolutionValue() );
                                executeCommand( revert, false );
                                if ( ret == BDialogResult.OK ) {
                                    // ダイアログの結果がOKで、かつベジエ曲線が単調増加なら編集を適用
                                    if ( BezierChain.isBezierImplicit( target_chain ) ) {
                                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( track,
                                                                                                    m_selected_curve,
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
                            VsqBPList list = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( m_selected_curve.getName() );
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
                                AppManager.clearSelectedPoint();
                                AppManager.addSelectedPoint( m_selected_curve, bp_id );
                                FormCurvePointEdit dialog = new FormCurvePointEdit( bp_id, m_selected_curve );
                                int tx = AppManager.xCoordFromClocks( tclock );
                                Point pt = pointToScreen( new Point( tx, 0 ) );
                                invalidate();
                                dialog.setLocation( new Point( pt.x - dialog.getWidth() / 2, pt.y - dialog.getHeight() ) );
                                dialog.setModal( true );
                                dialog.setVisible( true );
                            }
                            #endregion
                        }
                    }
                }
                #endregion
            } else if ( getHeight() - 2 * OFFSET_TRACK_TAB <= e.Y && e.Y <= getHeight() - OFFSET_TRACK_TAB ) {
                #region MouseDown occured on singer list
                if ( AppManager.getSelectedTool() != EditTool.ERASER ) {
                    VsqEvent ve = findItemAt( e.X, e.Y );
                    RendererKind renderer = VsqFileEx.getTrackRendererKind( AppManager.getVsqFile().Track.get( AppManager.getSelected() ) );
                    if ( ve != null ) {
                        if ( ve.ID.type != VsqIDType.Singer ) {
                            return;
                        }
                        if ( !m_cmenu_singer_prepared.Equals( renderer ) ) {
                            prepareSingerMenu( renderer );
                        }
                        TagForCMenuSinger tag = new TagForCMenuSinger();
                        tag.SingerChangeExists = true;
                        tag.InternalID = ve.InternalID;
                        cmenuSinger.setTag( tag );//                        new KeyValuePair<boolean, int>( true, ve.InternalID );
                        MenuElement[] sub_cmenu_singer = cmenuSinger.getSubElements();
                        for ( int i = 0; i < sub_cmenu_singer.Length; i++ ) {
                            BMenuItem tsmi = (BMenuItem)sub_cmenu_singer[i];
                            TagForCMenuSingerDropDown tag2 = (TagForCMenuSingerDropDown)tsmi.getTag();
                            if ( tag2.Language == ve.ID.IconHandle.Language && 
                                 tag2.Program == ve.ID.IconHandle.Program ) {
                                tsmi.setSelected( true );
                            } else {
                                tsmi.setSelected( false );
                            }
                        }
                        cmenuSinger.show( this, e.X, e.Y );
                    } else {
                        if ( !m_cmenu_singer_prepared.Equals( renderer ) ) {
                            prepareSingerMenu( renderer );
                        }
                        String singer = AppManager.editorConfig.DefaultSingerName;
                        int clock = AppManager.clockFromXCoord( e.X );
                        int last_clock = 0;
                        for ( Iterator<VsqEvent> itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getSingerEventIterator(); itr.hasNext(); ) {
                            VsqEvent ve2 = itr.next();
                            if ( last_clock <= clock && clock < ve2.Clock ) {
                                singer = ((IconHandle)ve2.ID.IconHandle).IDS;
                                break;
                            }
                            last_clock = ve2.Clock;
                        }
                        TagForCMenuSinger tag = new TagForCMenuSinger();
                        tag.SingerChangeExists = false;
                        tag.Clock = clock;
                        cmenuSinger.setTag( tag );//                        new KeyValuePair<boolean, int>( false, clock );
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

        /// <summary>
        /// 指定した歌声合成システムの歌手のリストを作成し，コンテキストメニューを準備します．
        /// </summary>
        /// <param name="renderer"></param>
        public void prepareSingerMenu( RendererKind renderer ) {
            cmenuSinger.removeAll();
            Vector<SingerConfig> items = null;
            if ( renderer == RendererKind.UTAU || renderer == RendererKind.STRAIGHT_UTAU ) {
                items = AppManager.editorConfig.UtauSingers;
            } else if ( renderer == RendererKind.VOCALOID1_100 || renderer == RendererKind.VOCALOID1_101 ) {
                items = new Vector<SingerConfig>( Arrays.asList( VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID1 ) ) );
            } else if ( renderer == RendererKind.VOCALOID2 ) {
                items = new Vector<SingerConfig>( Arrays.asList( VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID2 ) ) );
#if ENABLE_AQUESTONE
            } else if ( renderer == RendererKind.AQUES_TONE ){
                items = new Vector<SingerConfig>();
                int c = AquesToneDriver.SINGERS.Length;
                for( int i = 0; i < c; i++ ){
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
                if ( renderer == RendererKind.UTAU || renderer == RendererKind.STRAIGHT_UTAU ) {
                    if ( sc != null ) {
                        tip = "Name: " + sc.VOICENAME +
                              "\nDirectory: " + sc.VOICEIDSTR;
                    }
                } else if ( renderer == RendererKind.VOCALOID1_100 || renderer == RendererKind.VOCALOID1_101 ) {
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
                    BMenuItem tsmi = new BMenuItem();
                    tsmi.setText( sc.VOICENAME );
                    TagForCMenuSingerDropDown tag = new TagForCMenuSingerDropDown();
                    tag.ToolTipText = tip;
                    tag.ToolTipPxWidth = 0;
                    tag.Language = sc.Language;
                    tag.Program = sc.Program;
                    tsmi.setTag( tag );
#if JAVA
                    tsmi.clickEvent.add( new BEventHandler( this, "tsmi_Click" ) );
#else
                    tsmi.Click += new EventHandler( tsmi_Click );
#endif
                    if ( AppManager.editorConfig.Platform == PlatformEnum.Windows ) {
                        // TODO: cmenuSinger.ItemsのToolTip。monoで実行するとMouseHoverで落ちる
#if JAVA
#else
                        tsmi.MouseHover += new EventHandler( tsmi_MouseHover );
#endif
                    }
                    cmenuSinger.add( tsmi );
                    //list.Add( i );
                    count++;
                }
            }
#if JAVA
            cmenuSinger.visibleChangedEvent.add( new BEventHandler( this, "cmenuSinger_VisibleChanged" ) );
#else
            cmenuSinger.VisibleChanged += new EventHandler( cmenuSinger_VisibleChanged );
#endif
            m_cmenusinger_tooltip_width = new int[count];
            //m_cmenusinger_map = list.ToArray();
            for ( int i = 0; i < count; i++ ) {
                m_cmenusinger_tooltip_width[i] = 0;
            }
            m_cmenu_singer_prepared = renderer;
        }

        private void cmenuSinger_VisibleChanged( Object sender, BEventArgs e ) {
#if JAVA
#else
            toolTip.Hide( cmenuSinger );
#endif
        }

        private void tsmi_MouseEnter( Object sender, BEventArgs e ) {
            tsmi_MouseHover( sender, e );
        }

        private void tsmi_MouseHover( Object sender, BEventArgs e ) {
#if !JAVA
            try {
                TagForCMenuSingerDropDown tag = (TagForCMenuSingerDropDown)((BMenuItem)sender).getTag();
                String tip = tag.ToolTipText;
                int language = tag.Language;
                int program = tag.Program;

                // tooltipを表示するy座標を決める
                int y = 0;
                MenuElement[] sub = cmenuSinger.getSubElements();
                for ( int i = 0; i < sub.Length; i++ ) {
                    BMenuItem item = (BMenuItem)sub[i];
                    TagForCMenuSingerDropDown tag2 = (TagForCMenuSingerDropDown)item.getTag();
                    if ( language == tag2.Language &&
                         program == tag2.Program ) {
                        break;
                    }
                    y += item.getHeight();
                }

                int tip_width = tag.ToolTipPxWidth;
                Point ppts = cmenuSinger.pointToScreen( new Point( 0, 0 ) );
                Point pts = new Point( ppts.x, ppts.y );
                Rectangle rrc = PortUtil.getScreenBounds( this );
                Rectangle rc = new Rectangle( rrc.x, rrc.y, rrc.width, rrc.height );
                TagForCMenuSingerDropDown tag3 = new TagForCMenuSingerDropDown();
                tag3.Program = program;
                tag3.Language = language;
                toolTip.Tag = tag3;
                if ( pts.x + cmenuSinger.getWidth() + tip_width > rc.width ) {
                    toolTip.Show( tip, cmenuSinger, new System.Drawing.Point( -tip_width, y ), 5000 );
                } else {
                    toolTip.Show( tip, cmenuSinger, new System.Drawing.Point( cmenuSinger.Width, y ), 5000 );
                }
            } catch ( Exception ex ) {
                PortUtil.println( "TarckSelectro.tsmi_MouseHover; ex=" + ex );
                AppManager.debugWriteLine( "TarckSelectro.tsmi_MouseHover; ex=" + ex );
            }
#endif
        }

        private struct TagForCMenuSinger {
            public boolean SingerChangeExists;
            public int Clock;
            public int InternalID;
        }

        private struct TagForCMenuSingerDropDown {
            public int ToolTipPxWidth;
            public String ToolTipText;
            public int Language;
            public int Program;
        }

        private void tsmi_Click( Object sender, BEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "CmenuSingerClick" );
            AppManager.debugWriteLine( "    sender.GetType()=" + sender.GetType() );
#endif
            if ( sender is BMenuItem ) {
                TagForCMenuSinger tag = (TagForCMenuSinger)cmenuSinger.getTag();
                TagForCMenuSingerDropDown tag_dditem = (TagForCMenuSingerDropDown)((BMenuItem)sender).getTag();
                int language = tag_dditem.Language;
                int program = tag_dditem.Program;
                VsqID item = null;
                if ( m_cmenu_singer_prepared == RendererKind.VOCALOID1_100 || m_cmenu_singer_prepared == RendererKind.VOCALOID1_101 ) {
                    item = VocaloSysUtil.getSingerID( language, program, SynthesizerType.VOCALOID1 );
                } else if ( m_cmenu_singer_prepared == RendererKind.VOCALOID2 ) {
                    item = VocaloSysUtil.getSingerID( language, program, SynthesizerType.VOCALOID2 );
                } else if ( m_cmenu_singer_prepared == RendererKind.UTAU || m_cmenu_singer_prepared == RendererKind.STRAIGHT_UTAU ) {
                    item = AppManager.getSingerIDUtau( language, program );
                } else if ( m_cmenu_singer_prepared == RendererKind.AQUES_TONE ) {
                    item = AppManager.getSingerIDAquesTone( program );
                }
                if ( item != null ) {
                    int selected = AppManager.getSelected();
                    if ( tag.SingerChangeExists ) {
                        int id = tag.InternalID;
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandEventChangeIDContaints( selected, id, item ) );
#if DEBUG
                        AppManager.debugWriteLine( "tsmi_Click; item.IconHandle.Program" + item.IconHandle.Program );
#endif
                        executeCommand( run, true );
                    } else {
                        int clock = tag.Clock;
                        VsqEvent ve = new VsqEvent( clock, item );
                        CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventAdd( selected, ve ) );
                        executeCommand( run, true );
                    }
                }
            }
        }

#if !JAVA
        private void toolTip_Draw( Object sender, System.Windows.Forms.DrawToolTipEventArgs e ) {
            if ( !(sender is System.Windows.Forms.ToolTip) ) {
                return;
            }

            System.Drawing.Rectangle rrc = e.Bounds;
            Rectangle rc = new Rectangle( rrc.X, rrc.Y, rrc.Width, rrc.Height );
#if DEBUG
            PortUtil.println( "FormMain#toolTip_Draw; sender.GetType()=" + sender.GetType() );
#endif

            System.Windows.Forms.ToolTip tooltip = (System.Windows.Forms.ToolTip)sender;
            if ( tooltip.Tag == null ) {
                return;
            }
            if ( !(tooltip.Tag is TagForCMenuSingerDropDown) ) {
                return;
            }
            TagForCMenuSingerDropDown tag_tooltip = (TagForCMenuSingerDropDown)tooltip.Tag;
            MenuElement[] sub_cmenu_singer = cmenuSinger.getSubElements();
            for ( int i = 0; i < sub_cmenu_singer.Length; i++ ) {
                MenuElement tsi = sub_cmenu_singer[i];
                if ( !(tsi is BMenuItem) ) {
                    continue;
                }
                BMenuItem menu = (BMenuItem)tsi;
                Object obj = menu.getTag();
                if ( obj == null ) {
                    continue;
                }
                if ( !(obj is TagForCMenuSingerDropDown) ) {
                    continue;
                }
                TagForCMenuSingerDropDown tag = (TagForCMenuSingerDropDown)obj;
                if ( tag.Language == tag_tooltip.Language &&
                     tag.Program == tag_tooltip.Program ) {
                    tag.ToolTipPxWidth = rc.width;
                    ((BMenuItem)tsi).setTag( tag );
                    break;
                }
            }
            e.DrawBackground();
            e.DrawBorder();
            e.DrawText( System.Windows.Forms.TextFormatFlags.VerticalCenter | System.Windows.Forms.TextFormatFlags.Left | System.Windows.Forms.TextFormatFlags.NoFullWidthCharacterBreak );
        }
#endif

        private void TrackSelector_KeyDown( Object sender, BKeyEventArgs e ) {
#if JAVA
            if( (e.KeyCode & KeyEvent.VK_SPACE) == KeyEvent.VK_SPACE )
#else
            if ( (e.KeyCode & System.Windows.Forms.Keys.Space) == System.Windows.Forms.Keys.Space ) 
#endif
            {
                m_spacekey_downed = true;
            }
        }

        private void TrackSelector_KeyUp( Object sender, BKeyEventArgs e ) {
#if JAVA
            if( (e.KeyCode & KeyEvent.VK_SPACE) == KeyEvent.VK_SPACE )
#else
            if ( (e.KeyCode & System.Windows.Forms.Keys.Space) == System.Windows.Forms.Keys.Space )
#endif
            {
                m_spacekey_downed = false;
            }
        }

        private void cmenuCurveCommon_Click( Object sender, BEventArgs e ) {
            if ( sender is BMenuItem ) {
                BMenuItem tsmi = (BMenuItem)sender;
                if ( tsmi.getTag() is CurveType ) {
                    CurveType curve = (CurveType)tsmi.getTag();
                    changeCurve( curve );
                }
            }
        }

#if !JAVA
        private void panelZoomButton_Paint( Object sender, System.Windows.Forms.PaintEventArgs e ) {
            Graphics2D g = new Graphics2D( e.Graphics );
            // 外枠
            int halfheight = panelZoomButton.Height / 2;
            g.setColor( new Color( 118, 123, 138 ) );
            g.drawRect( 0, 0, panelZoomButton.Width - 1, panelZoomButton.Height - 1 );
            g.drawLine( 0, halfheight, panelZoomButton.Width, halfheight );
            /*if ( m_selected_curve == CurveType.PIT ) {
                int halfwidth = panelZoomButton.Width / 2;
                int quoterheight = panelZoomButton.Height / 4;
                // +の文字
                e.Graphics.drawLine( Pens.Black, new Point( halfwidth - 4, quoterheight ), new Point( halfwidth + 4, quoterheight ) );
                e.Graphics.drawLine( Pens.Black, new Point( halfwidth, quoterheight - 4 ), new Point( halfwidth, quoterheight + 4 ) );

                // -の文字
                e.Graphics.drawLine( Pens.Black, new Point( halfwidth - 4, quoterheight + halfheight ), new Point( halfwidth + 4, quoterheight + halfheight ) );
            }*/
        }
#endif

        private void panelZoomButton_MouseDown( Object sender, BMouseEventArgs e ) {
            return;
            /* // Pitch表示のとき、縦方向倍率変更ボタン上のMouseDownかどうかを検査
            if ( m_selected_curve == CurveType.PIT && e.Button == MouseButtons.Left ) {
                if ( 0 <= e.X && e.X < panelZoomButton.Width ) {
                    int halfheight = panelZoomButton.Height / 2;
                    if ( 0 <= e.Y && e.Y < halfheight ) {
                        if ( m_internal_pbs - 1 >= 1 ) {
                            m_internal_pbs--;
                            this.Invalidate();
                        }
                    } else if ( halfheight < e.Y && e.Y < panelZoomButton.Height ) {
                        if ( m_internal_pbs + 1 <= 24 ) {
                            m_internal_pbs++;
                            this.Invalidate();
                        }
                    }
                }
            }*/
        }

        private void registerEventHandlers() {
#if JAVA
#else
            this.toolTip.Draw += new System.Windows.Forms.DrawToolTipEventHandler( this.toolTip_Draw );
            this.cmenuCurveVelocity.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveAccent.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveDecay.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveDynamics.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveVibratoRate.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveVibratoDepth.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveReso1Freq.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveReso1BW.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveReso1Amp.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveReso2Freq.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveReso2BW.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveReso2Amp.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveReso3Freq.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveReso3BW.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveReso3Amp.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveReso4Freq.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveReso4BW.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveReso4Amp.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveHarmonics.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveBreathiness.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveBrightness.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveClearness.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveOpening.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveGenderFactor.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurvePortamentoTiming.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurvePitchBend.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurvePitchBendSensitivity.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.cmenuCurveEffect2Depth.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.panelZoomButton.Paint += new System.Windows.Forms.PaintEventHandler( this.panelZoomButton_Paint );
            this.panelZoomButton.MouseDown += new System.Windows.Forms.MouseEventHandler( this.panelZoomButton_MouseDown );
            this.cmenuCurveEnvelope.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            this.Load += new System.EventHandler( this.TrackSelector_Load );
            this.MouseMove += new System.Windows.Forms.MouseEventHandler( this.TrackSelector_MouseMove );
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( this.TrackSelector_MouseDoubleClick );
            this.KeyUp += new System.Windows.Forms.KeyEventHandler( this.TrackSelector_KeyUp );
            this.MouseClick += new System.Windows.Forms.MouseEventHandler( this.TrackSelector_MouseClick );
            this.MouseDown += new System.Windows.Forms.MouseEventHandler( this.TrackSelector_MouseDown );
            this.MouseUp += new System.Windows.Forms.MouseEventHandler( this.TrackSelector_MouseUp );
            this.KeyDown += new System.Windows.Forms.KeyEventHandler( this.TrackSelector_KeyDown );
#endif
        }

        private void setResources() {
        }

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ..\BuildJavaUI\src\org\kbinani\Cadencii\TrackSelector.java
        //INCLUDE-SECTION METHOD ..\BuildJavaUI\src\org\kbinani\Cadencii\TrackSelector.java
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

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.cmenuSinger = new org.kbinani.windows.forms.BPopupMenu( this.components );
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
            this.vScroll = new org.kbinani.windows.forms.BVScrollBar();
            this.panelZoomButton = new System.Windows.Forms.Panel();
            this.cmenuCurve.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmenuSinger
            // 
            this.cmenuSinger.Name = "cmenuSinger";
            this.cmenuSinger.Size = new System.Drawing.Size( 61, 4 );
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
            this.cmenuCurve.Size = new System.Drawing.Size( 185, 496 );
            // 
            // cmenuCurveVelocity
            // 
            this.cmenuCurveVelocity.Name = "cmenuCurveVelocity";
            this.cmenuCurveVelocity.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveVelocity.Text = "Velocity(&V)";
            // 
            // cmenuCurveAccent
            // 
            this.cmenuCurveAccent.Name = "cmenuCurveAccent";
            this.cmenuCurveAccent.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveAccent.Text = "Accent";
            // 
            // cmenuCurveDecay
            // 
            this.cmenuCurveDecay.Name = "cmenuCurveDecay";
            this.cmenuCurveDecay.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveDecay.Text = "Decay";
            // 
            // cmenuCurveSeparator1
            // 
            this.cmenuCurveSeparator1.Name = "cmenuCurveSeparator1";
            this.cmenuCurveSeparator1.Size = new System.Drawing.Size( 181, 6 );
            // 
            // cmenuCurveDynamics
            // 
            this.cmenuCurveDynamics.Name = "cmenuCurveDynamics";
            this.cmenuCurveDynamics.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveDynamics.Text = "Dynamics";
            // 
            // cmenuCurveVibratoRate
            // 
            this.cmenuCurveVibratoRate.Name = "cmenuCurveVibratoRate";
            this.cmenuCurveVibratoRate.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveVibratoRate.Text = "Vibrato Rate";
            // 
            // cmenuCurveVibratoDepth
            // 
            this.cmenuCurveVibratoDepth.Name = "cmenuCurveVibratoDepth";
            this.cmenuCurveVibratoDepth.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveVibratoDepth.Text = "Vibrato Depth";
            // 
            // cmenuCurveSeparator2
            // 
            this.cmenuCurveSeparator2.Name = "cmenuCurveSeparator2";
            this.cmenuCurveSeparator2.Size = new System.Drawing.Size( 181, 6 );
            // 
            // cmenuCurveReso1
            // 
            this.cmenuCurveReso1.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso1Freq,
            this.cmenuCurveReso1BW,
            this.cmenuCurveReso1Amp} );
            this.cmenuCurveReso1.Name = "cmenuCurveReso1";
            this.cmenuCurveReso1.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveReso1.Text = "Resonance 1";
            // 
            // cmenuCurveReso1Freq
            // 
            this.cmenuCurveReso1Freq.Name = "cmenuCurveReso1Freq";
            this.cmenuCurveReso1Freq.Size = new System.Drawing.Size( 152, 22 );
            this.cmenuCurveReso1Freq.Text = "Frequency";
            // 
            // cmenuCurveReso1BW
            // 
            this.cmenuCurveReso1BW.Name = "cmenuCurveReso1BW";
            this.cmenuCurveReso1BW.Size = new System.Drawing.Size( 152, 22 );
            this.cmenuCurveReso1BW.Text = "Band Width";
            // 
            // cmenuCurveReso1Amp
            // 
            this.cmenuCurveReso1Amp.Name = "cmenuCurveReso1Amp";
            this.cmenuCurveReso1Amp.Size = new System.Drawing.Size( 152, 22 );
            this.cmenuCurveReso1Amp.Text = "Amplitude";
            // 
            // cmenuCurveReso2
            // 
            this.cmenuCurveReso2.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso2Freq,
            this.cmenuCurveReso2BW,
            this.cmenuCurveReso2Amp} );
            this.cmenuCurveReso2.Name = "cmenuCurveReso2";
            this.cmenuCurveReso2.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveReso2.Text = "Resonance 2";
            // 
            // cmenuCurveReso2Freq
            // 
            this.cmenuCurveReso2Freq.Name = "cmenuCurveReso2Freq";
            this.cmenuCurveReso2Freq.Size = new System.Drawing.Size( 152, 22 );
            this.cmenuCurveReso2Freq.Text = "Frequency";
            // 
            // cmenuCurveReso2BW
            // 
            this.cmenuCurveReso2BW.Name = "cmenuCurveReso2BW";
            this.cmenuCurveReso2BW.Size = new System.Drawing.Size( 152, 22 );
            this.cmenuCurveReso2BW.Text = "Band Width";
            // 
            // cmenuCurveReso2Amp
            // 
            this.cmenuCurveReso2Amp.Name = "cmenuCurveReso2Amp";
            this.cmenuCurveReso2Amp.Size = new System.Drawing.Size( 152, 22 );
            this.cmenuCurveReso2Amp.Text = "Amplitude";
            // 
            // cmenuCurveReso3
            // 
            this.cmenuCurveReso3.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso3Freq,
            this.cmenuCurveReso3BW,
            this.cmenuCurveReso3Amp} );
            this.cmenuCurveReso3.Name = "cmenuCurveReso3";
            this.cmenuCurveReso3.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveReso3.Text = "Resonance 3";
            // 
            // cmenuCurveReso3Freq
            // 
            this.cmenuCurveReso3Freq.Name = "cmenuCurveReso3Freq";
            this.cmenuCurveReso3Freq.Size = new System.Drawing.Size( 152, 22 );
            this.cmenuCurveReso3Freq.Text = "Frequency";
            // 
            // cmenuCurveReso3BW
            // 
            this.cmenuCurveReso3BW.Name = "cmenuCurveReso3BW";
            this.cmenuCurveReso3BW.Size = new System.Drawing.Size( 152, 22 );
            this.cmenuCurveReso3BW.Text = "Band Width";
            // 
            // cmenuCurveReso3Amp
            // 
            this.cmenuCurveReso3Amp.Name = "cmenuCurveReso3Amp";
            this.cmenuCurveReso3Amp.Size = new System.Drawing.Size( 152, 22 );
            this.cmenuCurveReso3Amp.Text = "Amplitude";
            // 
            // cmenuCurveReso4
            // 
            this.cmenuCurveReso4.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso4Freq,
            this.cmenuCurveReso4BW,
            this.cmenuCurveReso4Amp} );
            this.cmenuCurveReso4.Name = "cmenuCurveReso4";
            this.cmenuCurveReso4.Size = new System.Drawing.Size( 184, 22 );
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
            this.cmenuCurveSeparator3.Size = new System.Drawing.Size( 181, 6 );
            // 
            // cmenuCurveHarmonics
            // 
            this.cmenuCurveHarmonics.Name = "cmenuCurveHarmonics";
            this.cmenuCurveHarmonics.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveHarmonics.Text = "Harmonics";
            // 
            // cmenuCurveBreathiness
            // 
            this.cmenuCurveBreathiness.Name = "cmenuCurveBreathiness";
            this.cmenuCurveBreathiness.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveBreathiness.Text = "Noise";
            // 
            // cmenuCurveBrightness
            // 
            this.cmenuCurveBrightness.Name = "cmenuCurveBrightness";
            this.cmenuCurveBrightness.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveBrightness.Text = "Brightness";
            // 
            // cmenuCurveClearness
            // 
            this.cmenuCurveClearness.Name = "cmenuCurveClearness";
            this.cmenuCurveClearness.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveClearness.Text = "Clearness";
            // 
            // cmenuCurveOpening
            // 
            this.cmenuCurveOpening.Name = "cmenuCurveOpening";
            this.cmenuCurveOpening.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveOpening.Text = "Opening";
            // 
            // cmenuCurveGenderFactor
            // 
            this.cmenuCurveGenderFactor.Name = "cmenuCurveGenderFactor";
            this.cmenuCurveGenderFactor.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveGenderFactor.Text = "Gender Factor";
            // 
            // cmenuCurveSeparator4
            // 
            this.cmenuCurveSeparator4.Name = "cmenuCurveSeparator4";
            this.cmenuCurveSeparator4.Size = new System.Drawing.Size( 181, 6 );
            // 
            // cmenuCurvePortamentoTiming
            // 
            this.cmenuCurvePortamentoTiming.Name = "cmenuCurvePortamentoTiming";
            this.cmenuCurvePortamentoTiming.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurvePortamentoTiming.Text = "Portamento Timing";
            // 
            // cmenuCurvePitchBend
            // 
            this.cmenuCurvePitchBend.Name = "cmenuCurvePitchBend";
            this.cmenuCurvePitchBend.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurvePitchBend.Text = "Pitch Bend";
            // 
            // cmenuCurvePitchBendSensitivity
            // 
            this.cmenuCurvePitchBendSensitivity.Name = "cmenuCurvePitchBendSensitivity";
            this.cmenuCurvePitchBendSensitivity.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurvePitchBendSensitivity.Text = "Pitch Bend Sensitivity";
            // 
            // cmenuCurveSeparator5
            // 
            this.cmenuCurveSeparator5.Name = "cmenuCurveSeparator5";
            this.cmenuCurveSeparator5.Size = new System.Drawing.Size( 181, 6 );
            // 
            // cmenuCurveEffect2Depth
            // 
            this.cmenuCurveEffect2Depth.Name = "cmenuCurveEffect2Depth";
            this.cmenuCurveEffect2Depth.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveEffect2Depth.Text = "Effect2 Depth";
            // 
            // cmenuCurveEnvelope
            // 
            this.cmenuCurveEnvelope.Name = "cmenuCurveEnvelope";
            this.cmenuCurveEnvelope.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveEnvelope.Text = "Envelope";
            // 
            // vScroll
            // 
            this.vScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScroll.Enabled = false;
            this.vScroll.Location = new System.Drawing.Point( 414, 0 );
            this.vScroll.Name = "vScroll";
            this.vScroll.Size = new System.Drawing.Size( 16, 193 );
            this.vScroll.TabIndex = 2;
            // 
            // panelZoomButton
            // 
            this.panelZoomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelZoomButton.BackColor = System.Drawing.Color.DarkGray;
            this.panelZoomButton.Location = new System.Drawing.Point( 414, 193 );
            this.panelZoomButton.Margin = new System.Windows.Forms.Padding( 0 );
            this.panelZoomButton.Name = "panelZoomButton";
            this.panelZoomButton.Size = new System.Drawing.Size( 16, 33 );
            this.panelZoomButton.TabIndex = 3;
            // 
            // TrackSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.Controls.Add( this.vScroll );
            this.Controls.Add( this.panelZoomButton );
            this.DoubleBuffered = true;
            this.Name = "TrackSelector";
            this.Size = new System.Drawing.Size( 430, 228 );
            this.cmenuCurve.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private BPopupMenu cmenuSinger;
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
        private BVScrollBar vScroll;
        private System.Windows.Forms.Panel panelZoomButton;
        private BMenuItem cmenuCurvePitchBend;
        private BMenuItem cmenuCurvePitchBendSensitivity;
        private BMenuItem cmenuCurveEnvelope;

        #endregion
#endif
    }

#if !JAVA
}
#endif
