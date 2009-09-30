/*
 * TrackSelector.cs
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using Boare.Lib.Vsq;
using Boare.Lib.AppUtil;
using bocoree;

namespace Boare.Cadencii {

    using boolean = System.Boolean;
    using Integer = System.Int32;
    using Long = System.Int64;

    partial class TrackSelector : UserControl {
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

        private static readonly Color DOT_COLOR_MASTER = Color.Red;
        private static readonly Color CURVE_COLOR = Color.Navy;
        private static readonly Color CONROL_LINE = Color.Orange;
        private static readonly Color DOT_COLOR_NORMAL = Color.FromArgb( 237, 107, 158 );
        private static readonly Color DOT_COLOR_BASE = Color.FromArgb( 125, 198, 34 );
        private static readonly Color DOT_COLOR_NORMAL_AROUND = Color.FromArgb( 231, 50, 122 );
        private static readonly Color DOT_COLOR_BASE_AROUND = Color.FromArgb( 90, 143, 24 );
        private static readonly Color CURVE_COLOR_DOT = Color.Coral;
        private static readonly SolidBrush BRS_A244_255_023_012 = new SolidBrush( Color.FromArgb( 244, 255, 23, 12 ) );
        private static readonly SolidBrush BRS_A144_255_255_255 = new SolidBrush( Color.FromArgb( 144, 255, 255, 255 ) );
        private static readonly SolidBrush s_brs_a072_255_255_255 = new SolidBrush( Color.FromArgb( 72, 255, 255, 255 ) );
        private static readonly SolidBrush s_brs_a127_008_166_172 = new SolidBrush( Color.FromArgb( 127, 8, 166, 172 ) );
        private static readonly SolidBrush s_brs_a098_000_000_000 = new SolidBrush( Color.FromArgb( 98, 0, 0, 0 ) );
        private static readonly SolidBrush s_brs_dot_master = new SolidBrush( DOT_COLOR_MASTER );
        private static readonly SolidBrush s_brs_dot_normal = new SolidBrush( DOT_COLOR_NORMAL );
        private static readonly SolidBrush s_brs_dot_base = new SolidBrush( DOT_COLOR_BASE );
        private static readonly SolidBrush BRS_CURVE_COLOR_DOT = new SolidBrush( CURVE_COLOR_DOT );
        private static readonly Pen s_pen_dot_normal = new Pen( DOT_COLOR_NORMAL_AROUND );
        private static readonly Pen s_pen_dot_base = new Pen( DOT_COLOR_BASE_AROUND );
        private static readonly Pen s_pen_050_140_150 = new Pen( Color.FromArgb( 50, 140, 150 ) );
        private static readonly Pen s_pen_128_128_128 = new Pen( Color.FromArgb( 128, 128, 128 ) );
        private static readonly Pen s_pen_246_251_010 = new Pen( Color.FromArgb( 246, 251, 10 ) );
        private static readonly Pen s_pen_curve = new Pen( CURVE_COLOR );
        private static readonly Pen s_pen_control_line = new Pen( CONROL_LINE );
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
        #endregion

        private CurveType m_selected_curve = CurveType.VEL;
        private CurveType m_last_selected_curve = CurveType.DYN;
        private boolean m_curve_visible = true;
        /// <summary>
        /// 現在のマウス位置におけるカーブの値
        /// </summary>
        private int m_mouse_value;
        private Point[] m_points = new Point[BUF_LEN];
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
        private SortedList<int, int> m_mouse_trace;
        /// <summary>
        /// マウスのトレース時、前回リストに追加されたx座標の値
        /// </summary>
        private int m_mouse_trace_last_x;
        private int m_mouse_trace_last_y;
        /// <summary>
        /// 選択範囲。Xはクロック単位、Yは各コントロールカーブの値と同じ単位
        /// </summary>
        private Rectangle m_selecting_region;
        private Range m_selected_region;
        private boolean m_selected_region_enabled = false;
        private boolean m_pencil_moved = false;
        private Thread m_mouse_hover_thread = null;
        /// <summary>
        /// cmenuSingerのメニューアイテムを初期化するのに使用したRenderer。
        /// </summary>
        private String m_cmenu_singer_prepared = "";
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
        private Keys m_modifier_key = Keys.Control;
        /// <summary>
        /// このコントロールが表示を担当しているカーブのリスト
        /// </summary>
        private Vector<CurveType> m_viewing_curves = new Vector<CurveType>();
        private Pen m_generic_line = new Pen( Color.FromArgb( 118, 123, 138 ) );
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
        private Keys m_modifier_on_mouse_down = Keys.None;
        /// <summary>
        /// 移動しているデータ点のリスト
        /// </summary>
        private Vector<BPPair> m_moving_points = new Vector<BPPair>();
        /* /// <summary>
        /// 移動開始時の、マウス位置とデータ点の座標のズレ
        /// </summary>
        private Point m_moving_points_shift;*/

        public delegate void CommandExecutedEventHandler();

        public event BSimpleDelegate<CurveType> SelectedCurveChanged;
        public event BSimpleDelegate<int> SelectedTrackChanged;
        public event CommandExecutedEventHandler CommandExecuted;
        public event BSimpleDelegate<int[]> RenderRequired;

        public TrackSelector() {
            this.SetStyle( ControlStyles.DoubleBuffer, true );
            this.SetStyle( ControlStyles.UserPaint, true );
            InitializeComponent();
            m_modifier_key = (AppManager.editorConfig.Platform == Platform.Macintosh) ? Keys.Menu : Keys.Control;
            cmenuCurveVelocity.Tag = CurveType.VEL;
            cmenuCurveAccent.Tag = CurveType.Accent;
            cmenuCurveDecay.Tag = CurveType.Decay;

            cmenuCurveDynamics.Tag = CurveType.DYN;
            cmenuCurveVibratoRate.Tag = CurveType.VibratoRate;
            cmenuCurveVibratoDepth.Tag = CurveType.VibratoDepth;

            cmenuCurveReso1Amp.Tag = CurveType.reso1amp;
            cmenuCurveReso1BW.Tag = CurveType.reso1bw;
            cmenuCurveReso1Freq.Tag = CurveType.reso1freq;
            cmenuCurveReso2Amp.Tag = CurveType.reso2amp;
            cmenuCurveReso2BW.Tag = CurveType.reso2bw;
            cmenuCurveReso2Freq.Tag = CurveType.reso2freq;
            cmenuCurveReso3Amp.Tag = CurveType.reso3amp;
            cmenuCurveReso3BW.Tag = CurveType.reso3bw;
            cmenuCurveReso3Freq.Tag = CurveType.reso3freq;
            cmenuCurveReso4Amp.Tag = CurveType.reso4amp;
            cmenuCurveReso4BW.Tag = CurveType.reso4bw;
            cmenuCurveReso4Freq.Tag = CurveType.reso4freq;

            cmenuCurveHarmonics.Tag = CurveType.harmonics;
            cmenuCurveBreathiness.Tag = CurveType.BRE;
            cmenuCurveBrightness.Tag = CurveType.BRI;
            cmenuCurveClearness.Tag = CurveType.CLE;
            cmenuCurveOpening.Tag = CurveType.OPE;
            cmenuCurveGenderFactor.Tag = CurveType.GEN;

            cmenuCurvePortamentoTiming.Tag = CurveType.POR;
            cmenuCurvePitchBend.Tag = CurveType.PIT;
            cmenuCurvePitchBendSensitivity.Tag = CurveType.PBS;

            cmenuCurveEffect2Depth.Tag = CurveType.fx2depth;
            cmenuCurveEnvelope.Tag = CurveType.Env;
        }

        protected override void OnResize( EventArgs e ) {
            base.OnResize( e );
            vScroll.Width = VSCROLL_WIDTH;
            vScroll.Height = this.Height - ZOOMPANEL_HEIGHT - 2;
            vScroll.Left = this.Width - VSCROLL_WIDTH;
            vScroll.Top = 0;

            panelZoomButton.Width = VSCROLL_WIDTH;
            panelZoomButton.Height = ZOOMPANEL_HEIGHT;
            panelZoomButton.Left = this.Width - VSCROLL_WIDTH;
            panelZoomButton.Top = this.Height - ZOOMPANEL_HEIGHT - 2;
        }

        public void applyLanguage() {
        }

        public void applyFont( Font font ) {
            foreach ( Control c in Controls ) {
                Boare.Lib.AppUtil.Misc.ApplyFontRecurse( c, font );
            }
            Misc.ApplyContextMenuFontRecurse( cmenuSinger, font );
            Misc.ApplyContextMenuFontRecurse( cmenuCurve, font );
        }

        /// <summary>
        /// このコントロールの推奨最小表示高さを取得します
        /// </summary>
        public int PreferredMinSize {
            get {
                return 240 + 18 * (m_viewing_curves.size() - 10) + 16;
            }
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
                        if ( m_viewing_curves.get( i ).Index > m_viewing_curves.get( i + 1 ).Index ) {
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
            if ( CommandExecuted != null ) {
                CommandExecuted();
            }
        }

        public KeyValuePair<Integer, Integer> SelectedRegion {
            get {
                int x0 = m_selected_region.getStart();
                int x1 = m_selected_region.getEnd();
                int min = Math.Min( x0, x1 );
                int max = Math.Max( x0, x1 );
                return new KeyValuePair<Integer, Integer>( min, max );
            }
        }

        /*public void DisableSelectedRegion() {
            m_selected_region_enabled = false;
        }*/

        /*public boolean SelectedRegionEnabled {
            get {
                if ( m_selected_curve.equals( CurveType.VEL ) || m_selected_curve.equals( CurveType.Accent ) || m_selected_curve.equals( CurveType.Decay ) ) {
                    return false;
                } else {
                    return m_selected_region_enabled;
                }
            }
        }*/

        /// <summary>
        /// 現在最前面に表示され，編集可能となっているカーブの種類を取得または設定します
        /// </summary>
        public CurveType SelectedCurve {
            get {
                return m_selected_curve;
            }
            set {
                CurveType old = m_selected_curve;
                m_selected_curve = value;
                if ( !old.equals( m_selected_curve ) ) {
                    m_last_selected_curve = old;
                    if ( SelectedCurveChanged != null ) {
                        SelectedCurveChanged( m_selected_curve );
                    }
                }
            }
        }

        /// <summary>
        /// エディタのx方向の位置からクロック数を求めます
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int clockFromXCoord( int x ) {
            return (int)((x - AppManager.KEY_LENGTH - 6 + AppManager.startToDrawX) / AppManager.scaleX);
        }

        /// <summary>
        /// クロック数から、画面に描くべきx座標の値を取得します。
        /// </summary>
        /// <param name="clocks"></param>
        /// <returns></returns>
        public int xCoordFromClocks( int clocks ) {
            return (int)(clocks * AppManager.scaleX - AppManager.startToDrawX) + AppManager.KEY_LENGTH + 6;
        }

        /// <summary>
        /// エディタのy方向の位置から，カーブの値を求めます
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public int valueFromYCoord( int y ) {
            int max = m_selected_curve.Maximum;
            int min = m_selected_curve.Minimum;
            return valueFromYCoord( y, max, min );
        }

        public int valueFromYCoord( int y, int max, int min ) {
            int oy = this.Height - 42;
            float order = GraphHeight / (float)(max - min);
            return (int)((oy - y) / order) + min;
        }

        public int yCoordFromValue( int value ) {
            int max = m_selected_curve.Maximum;
            int min = m_selected_curve.Minimum;
            return yCoordFromValue( value, max, min );
        }

        public int yCoordFromValue( int value, int max, int min ) {
            int oy = this.Height - 42;
            float order = GraphHeight / (float)(max - min);
            return oy - (int)((value - min) * order);
        }

        /// <summary>
        /// カーブエディタを表示するかどうかを取得または設定します
        /// </summary>
        public boolean CurveVisible {
            get {
                return m_curve_visible;
            }
            set {
                m_curve_visible = value;
            }
        }

        protected override void OnPaint( PaintEventArgs e ) {
            drawTo( e.Graphics, new Size( this.Width - vScroll.Width + 2, this.Height ) );
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
            e.Graphics.DrawString(
                m_fps.ToString( "000.000" ),
                new Font( "Verdana", 40, FontStyle.Bold ),
                Brushes.Red,
                new PointF( 0, 0 ) );
#endif
        }

        /// <summary>
        /// x軸方向の表示倍率。pixel/clock
        /// </summary>
        public float ScaleY {
            get {
                int max = m_selected_curve.Maximum;
                int min = m_selected_curve.Minimum;
                int oy = this.Height - 42;
                return GraphHeight / (float)(max - min);
            }
        }

        public Rectangle getRectFromCurveType( CurveType curve ) {
            int centre = 17 + GraphHeight / 2 + 8;
            int index = 100;
            for ( int i = 0; i < m_viewing_curves.size(); i++ ) {
                if ( m_viewing_curves.get( i ).equals( curve ) ) {
                    index = i;
                    break;
                }
            }
            int y = centre - m_viewing_curves.size() * 9 + 2 + 18 * index;
            return new Rectangle( 7, y, 56, 14 );
        }

        public void drawTo( Graphics g, Size size ) {
#if DEBUG
            try{
#endif
            SolidBrush brs_string = new SolidBrush( Color.Black );
            using ( Pen rect_curve = new Pen( Color.FromArgb( 41, 46, 55 ) ) ) {
                int centre = 8 + GraphHeight / 2;
                g.FillRectangle( Brushes.DarkGray,
                                 new Rectangle( 0, size.Height - 2 * OFFSET_TRACK_TAB, size.Width, 2 * OFFSET_TRACK_TAB ) );
                int numeric_view = m_mouse_value;
                Point mouse = this.PointToClient( Control.MousePosition );

                #region SINGER
                Region last = g.Clip;
                g.DrawLine( m_generic_line,
                            new Point( 2, size.Height - 2 * OFFSET_TRACK_TAB ),
                            new Point( size.Width - 3, size.Height - 2 * OFFSET_TRACK_TAB ) );
                g.DrawLine( m_generic_line,
                            new Point( AppManager.KEY_LENGTH, size.Height - 2 * OFFSET_TRACK_TAB + 1 ),
                            new Point( AppManager.KEY_LENGTH, size.Height - 2 * OFFSET_TRACK_TAB + 15 ) );
                g.DrawString( "SINGER",
                              AppManager.baseFont8,
                              brs_string,
                              new PointF( 9, size.Height - 2 * OFFSET_TRACK_TAB + OFFSET_TRACK_TAB / 2 - AppManager.baseFont8OffsetHeight ) );
                g.SetClip( new Rectangle( AppManager.KEY_LENGTH,
                                          size.Height - 2 * OFFSET_TRACK_TAB,
                                          size.Width - AppManager.KEY_LENGTH,
                                          OFFSET_TRACK_TAB ),
                                          CombineMode.Replace );
                VsqTrack draw_target = null;
                if ( AppManager.getVsqFile() != null ) {
                    draw_target = AppManager.getVsqFile().Track.get( AppManager.getSelected() );
                }
                if ( draw_target != null ) {
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Near;
                    int event_count = draw_target.getEventCount();
                    for ( int i = 0; i < event_count; i++ ) {
                        VsqEvent ve = draw_target.getEvent( i );
                        if ( ve.ID.type == VsqIDType.Singer ) {
                            int clock = ve.Clock;
                            int x = xCoordFromClocks( clock );
                            Rectangle rc = new Rectangle( x, size.Height - 2 * OFFSET_TRACK_TAB + 1, SINGER_ITEM_WIDTH, OFFSET_TRACK_TAB - 5 );
                            g.FillRectangle( Brushes.White, rc );
                            if ( AppManager.isSelectedEventContains( AppManager.getSelected(), ve.InternalID ) ) {
                                g.DrawRectangle( new Pen( AppManager.getHilightBrush() ), rc );
                                g.DrawString( ve.ID.IconHandle.IDS,
                                              AppManager.baseFont8,
                                              brs_string,
                                              rc, 
                                              sf );
                            } else {
                                g.DrawRectangle( new Pen( Color.FromArgb( 182, 182, 182 ) ), rc );
                                g.DrawString( ve.ID.IconHandle.IDS,
                                              AppManager.baseFont8,
                                              brs_string,
                                              rc,
                                              sf );
                            }
                        }
                    }
                }
                g.SetClip( last, CombineMode.Replace );
                #endregion

                #region トラック選択欄
                int selecter_width = SelectorWidth;
                g.DrawLine( m_generic_line,
                            new Point( 1, size.Height - OFFSET_TRACK_TAB ),
                            new Point( size.Width - 2, size.Height - OFFSET_TRACK_TAB ) );
                g.DrawString( "TRACK",
                              AppManager.baseFont8,
                              brs_string,
                              new PointF( 9, size.Height - OFFSET_TRACK_TAB + OFFSET_TRACK_TAB / 2 - AppManager.baseFont8OffsetHeight ) );
                /*g.DrawString( "RENDER ALL",
                              new Font( "MS UI Gothic", 7 ),
                              brs_string,
                              new PointF( size.Width - 59, size.Height - _OFFSET_TRACK_TAB + 3 ) );*/
                /*g.DrawLine( m_generic_line,
                            new Point( size.Width - _PX_WIDTH_RENDERALL, size.Height - _OFFSET_TRACK_TAB ),
                            new Point( size.Width - _PX_WIDTH_RENDERALL, size.Height - 1 ) );*/
                if ( AppManager.getVsqFile() != null ) {
                    for ( int i = 0; i < 16; i++ ) {
                        int x = AppManager.KEY_LENGTH + i * selecter_width;
#if DEBUG
                        try {
#endif
                            drawTrackTab( g,
                                          new Rectangle( x, size.Height - OFFSET_TRACK_TAB + 1, selecter_width, OFFSET_TRACK_TAB - 1 ),
                                          (i + 1 < AppManager.getVsqFile().Track.size()) ? (i + 1) + " " + AppManager.getVsqFile().Track.get( i + 1 ).getName() : "",
                                          (i == AppManager.getSelected() - 1),
                                          draw_target.getCommon().PlayMode >= 0,
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

                int clock_at_mouse = clockFromXCoord( mouse.X );
                int pbs_at_mouse = 0;
                if ( CurveVisible ) {
                    #region カーブエディタ
                    // カーブエディタの下の線
                    g.DrawLine( new Pen( Color.FromArgb( 156, 161, 169 ) ),
                                new Point( AppManager.KEY_LENGTH, size.Height - 42 ),
                                new Point( size.Width - 3, size.Height - 42 ) );

                    // カーブエディタの上の線
                    g.DrawLine( new Pen( Color.FromArgb( 46, 47, 50 ) ),
                                new Point( AppManager.KEY_LENGTH, 8 ),
                                new Point( size.Width - 3, 8 ) );

                    g.DrawLine( new Pen( Color.FromArgb( 125, 123, 124 ) ),
                                new Point( AppManager.KEY_LENGTH, 0 ),
                                new Point( AppManager.KEY_LENGTH, size.Height - 35 ) );

                    if ( m_selected_region_enabled ) {
                        int x0 = xCoordFromClocks( m_selected_region.getStart() );
                        int x1 = xCoordFromClocks( m_selected_region.getEnd() );
                        g.FillRectangle( s_brs_a072_255_255_255, new Rectangle( x0, HEADER, x1 - x0, GraphHeight ) );
                    }

                    #region 小節ごとのライン
                    if ( AppManager.getVsqFile() != null ) {
                        int dashed_line_step = AppManager.getPositionQuantizeClock();
                        g.SetClip( new Rectangle( AppManager.KEY_LENGTH, HEADER, size.Width - AppManager.KEY_LENGTH, size.Height - 2 * OFFSET_TRACK_TAB ) );
                        using ( Pen white100 = new Pen( Color.FromArgb( 100, Color.Black ) ) ) {
                            for ( Iterator itr = AppManager.getVsqFile().getBarLineIterator( clockFromXCoord( Width ) ); itr.hasNext(); ) {
                                VsqBarLineType blt = (VsqBarLineType)itr.next();
                                int x = xCoordFromClocks( blt.clock() );
                                int local_clock_step = 480 * 4 / blt.getLocalDenominator();
                                if ( blt.isSeparator() ) {
                                    g.DrawLine( white100,
                                                new Point( x, size.Height - 42 - 1 ),
                                                new Point( x, 8 + 1 ) );
                                } else {
                                    g.DrawLine( white100,
                                                new Point( x, centre - 5 ),
                                                new Point( x, centre + 6 ) );
                                    using ( Pen pen = new Pen( Color.FromArgb( 12, 12, 12 ) ) ) {
                                        g.DrawLine( pen,
                                                    new Point( x, 8 ),
                                                    new Point( x, 14 ) );
                                        g.DrawLine( pen,
                                                    new Point( x, size.Height - 43 ),
                                                    new Point( x, size.Height - 42 - 6 ) );
                                    }
                                }
                                if ( dashed_line_step > 1 && AppManager.isGridVisible() ) {
                                    int numDashedLine = local_clock_step / dashed_line_step;
                                    using ( Pen pen = new Pen( Color.FromArgb( 65, 65, 65 ) ) ) {
                                        for ( int i = 1; i < numDashedLine; i++ ) {
                                            int x2 = xCoordFromClocks( blt.clock() + i * dashed_line_step );
                                            g.DrawLine( white100,
                                                        new Point( x2, centre - 2 ),
                                                        new Point( x2, centre + 3 ) );
                                            g.DrawLine( white100,
                                                        new Point( x2, 8 ),
                                                        new Point( x2, 12 ) );
                                            g.DrawLine( white100,
                                                        new Point( x2, size.Height - 43 ),
                                                        new Point( x2, size.Height - 43 - 4 ) );
                                        }
                                    }
                                }
                            }
                        }
                        g.ResetClip();
                    }
                    #endregion

                    if ( draw_target != null ) {
                        Color front = Color.FromArgb( 150, AppManager.getHilightColor() );
                        Color back = Color.FromArgb( 44, 255, 249, 255 );
                        Color vel_color = Color.FromArgb( 64, 78, 30 );

                        // 後ろに描くカーブ
                        if ( m_last_selected_curve.equals( CurveType.VEL ) || m_last_selected_curve.equals( CurveType.Accent ) || m_last_selected_curve.equals( CurveType.Decay ) ) {
                            drawVEL( g, draw_target, back, false, m_last_selected_curve );
                        } else if ( m_last_selected_curve.equals( CurveType.VibratoRate ) || m_last_selected_curve.equals( CurveType.VibratoDepth ) ) {
                            drawVibratoControlCurve( g, draw_target, m_last_selected_curve, back, false );
                        } else {
                            VsqBPList list_back = draw_target.getCurve( m_last_selected_curve.Name );
                            if ( list_back != null ) {
                                drawVsqBPList( g, list_back, back, false );
                            }
                        }

                        // 手前に描くカーブ
                        if ( m_selected_curve.equals( CurveType.VEL ) || m_selected_curve.equals( CurveType.Accent ) || m_selected_curve.equals( CurveType.Decay ) ) {
                            drawVEL( g, draw_target, vel_color, true, m_selected_curve );
                        } else if ( m_selected_curve.equals( CurveType.VibratoRate ) || m_selected_curve.equals( CurveType.VibratoDepth ) ) {
                            drawVibratoControlCurve( g, draw_target, m_selected_curve, front, true );
                        } else if ( m_selected_curve.equals( CurveType.Env ) ){
                            drawEnvelope( g, draw_target, front );
                        } else {
                            VsqBPList list_front = draw_target.getCurve( m_selected_curve.Name );
                            if ( list_front != null ) {
                                drawVsqBPList( g, list_front, front, true );
                            }
                            if ( m_selected_curve.equals( CurveType.PIT ) ) {
                                #region PBSの値に応じて，メモリを記入する
                                SmoothingMode old = g.SmoothingMode;
                                g.SmoothingMode = SmoothingMode.AntiAlias;
                                using ( Pen nrml = new Pen( Color.FromArgb( 190, Color.Black ) ) )
                                using ( Pen dash = new Pen( Color.FromArgb( 128, Color.Black ) ) ) {
                                    dash.DashStyle = DashStyle.Dash;
                                    VsqBPList pbs = draw_target.getCurve( CurveType.PBS.Name );
                                    pbs_at_mouse = pbs.getValue( clock_at_mouse );
                                    int c = pbs.size();
                                    int premeasure = AppManager.getVsqFile().getPreMeasureClocks();
                                    int clock_start = clockFromXCoord( AppManager.KEY_LENGTH );
                                    int clock_end = clockFromXCoord( this.Width );
                                    if ( clock_start < premeasure && premeasure < clock_end ) {
                                        clock_start = premeasure;
                                    }
                                    int last_pbs = pbs.getValue( clock_start );
                                    int last_clock = clock_start;
                                    int ycenter = yCoordFromValue( 0 );
                                    g.DrawLine( nrml, new Point( AppManager.KEY_LENGTH, ycenter ), new Point( Width, ycenter ) );
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
                                        int x1 = xCoordFromClocks( last_clock );
                                        int x2 = xCoordFromClocks( cl );
                                        for ( int j = min + 1; j <= max - 1; j++ ) {
                                            if ( j == 0 ) {
                                                continue;
                                            }
                                            int y = yCoordFromValue( (int)(j * 8192 / (double)last_pbs) );
                                            Pen pen = null;
                                            if ( j % 2 == 0 ) {
                                                pen = nrml;
                                            } else {
                                                pen = dash;
                                            }
                                            g.DrawLine( pen, new Point( x1, y ), new Point( x2, y ) );
                                        }
                                        last_clock = cl;
                                        last_pbs = thispbs;
                                    }
                                    int max0 = last_pbs;
                                    int min0 = -last_pbs;
                                    int x10 = xCoordFromClocks( last_clock );
                                    int x20 = xCoordFromClocks( clock_end );
                                    for ( int j = min0 + 1; j <= max0 - 1; j++ ) {
                                        if ( j == 0 ) {
                                            continue;
                                        }
                                        int y = yCoordFromValue( (int)(j * 8192 / (double)last_pbs) );
                                        Pen pen = null;
                                        if ( j % 2 == 0 ) {
                                            pen = nrml;
                                        } else {
                                            pen = dash;
                                        }
                                        g.DrawLine( pen, new Point( x10, y ), new Point( x20, y ) );
                                    }
                                }
                                g.SmoothingMode = old;
                                #endregion
                            }
                            drawAttachedCurve( g, AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).get( m_selected_curve ) );
                        }
                    }

                    if ( AppManager.selectedRegionEnabled ) {
                        int stdx = AppManager.startToDrawX;
                        int start = AppManager.selectedRegion.Start - stdx;
                        int end = AppManager.selectedRegion.End - stdx;
                        g.FillRectangle( s_brs_a098_000_000_000,
                                         new Rectangle( start, HEADER, end - start, GraphHeight ) );
                    }

                    if ( m_mouse_downed ) {
                        #region 選択されたツールに応じて描画
                        int value = valueFromYCoord( mouse.Y );
                        if ( clock_at_mouse < AppManager.getVsqFile().getPreMeasure() ) {
                            clock_at_mouse = AppManager.getVsqFile().getPreMeasure();
                        }
                        int max = m_selected_curve.Maximum;
                        int min = m_selected_curve.Minimum;
                        if ( value < min ) {
                            value = min;
                        } else if ( max < value ) {
                            value = max;
                        }
                        switch ( AppManager.getSelectedTool() ) {
                            case EditTool.LINE:
                                int xini = xCoordFromClocks( m_line_start.X );
                                int yini = yCoordFromValue( m_line_start.Y );
                                g.DrawLine( s_pen_050_140_150,
                                            new Point( xini, yini ),
                                            new Point( xCoordFromClocks( clock_at_mouse ), yCoordFromValue( value ) ) );
                                break;
                            case EditTool.PENCIL:
                                if ( m_mouse_trace != null && !AppManager.isCurveMode() ) {
                                    Vector<Point> pt = new Vector<Point>();
                                    int stdx = AppManager.startToDrawX;
                                    int lastx = m_mouse_trace.Keys[0] - stdx;
                                    int lasty = m_mouse_trace[m_mouse_trace.Keys[0]];
                                    int height = this.Height - 42;
                                    if ( lasty < 8 ) {
                                        lasty = 8;
                                    } else if ( height < lasty ) {
                                        lasty = height;
                                    }
                                    pt.add( new Point( lastx, height ) );
                                    pt.add( new Point( lastx, lasty ) );
                                    IList<int> keylist = m_mouse_trace.Keys;
                                    int keys_count = keylist.Count;
                                    for ( int i = 1; i < keys_count; i++ ) {
                                        int key = keylist[i];
                                        int new_x = key - stdx;
                                        int new_y = m_mouse_trace[key];
                                        if ( new_y < 8 ) {
                                            new_y = 8;
                                        } else if ( height < new_y ) {
                                            new_y = height;
                                        }
                                        pt.add( new Point( new_x, lasty ) );
                                        pt.add( new Point( new_x, new_y ) );
                                        lastx = new_x;
                                        lasty = new_y;
                                    }
                                    pt.add( new Point( lastx, height ) );
                                    g.FillPolygon( new SolidBrush( Color.FromArgb( 127, 8, 166, 172 ) ),
                                                   pt.toArray( new Point[]{} ) );
                                }
                                break;
                            case EditTool.ERASER:
                            case EditTool.ARROW:
                                if ( m_mouse_down_mode == MouseDownMode.CURVE_EDIT && m_mouse_moved && m_selecting_region.Width != 0 ) {
                                    xini = xCoordFromClocks( m_selecting_region.X );
                                    int xend = xCoordFromClocks( m_selecting_region.X + m_selecting_region.Width );
                                    int x_start = Math.Min( xini, xend );
                                    if ( x_start < AppManager.KEY_LENGTH ) {
                                        x_start = AppManager.KEY_LENGTH;
                                    }
                                    int x_end = Math.Max( xini, xend );
                                    yini = yCoordFromValue( m_selecting_region.Y );
                                    int yend = yCoordFromValue( m_selecting_region.Y + m_selecting_region.Height );
                                    int y_start = Math.Min( yini, yend );
                                    int y_end = Math.Max( yini, yend );
                                    if ( y_start < 8 ) y_start = 8;
                                    if ( y_end > this.Height - 42 - 8 ) y_end = this.Height - 42;
                                    if ( x_start < x_end ) {
                                        g.FillRectangle( BRS_A144_255_255_255,
                                                         new Rectangle( x_start, y_start, x_end - x_start, y_end - y_start ) );
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
                                break;
                        }
                        if ( m_mouse_down_mode == MouseDownMode.SINGER_LIST && AppManager.getSelectedTool() != EditTool.ERASER ) {
                            for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                                SelectedEventEntry item = (SelectedEventEntry)itr.next();
                                int x = xCoordFromClocks( item.editing.Clock );
                                g.DrawLines( s_pen_128_128_128,
                                             new Point[] { new Point( x, size.Height - OFFSET_TRACK_TAB ),
                                                           new Point( x, size.Height - 2 * OFFSET_TRACK_TAB + 1 ),
                                                           new Point( x + SINGER_ITEM_WIDTH, size.Height - 2 * OFFSET_TRACK_TAB + 1 ),
                                                           new Point( x + SINGER_ITEM_WIDTH, size.Height - OFFSET_TRACK_TAB ) } );
                                g.DrawLine( s_pen_246_251_010,
                                            new Point( x, size.Height - OFFSET_TRACK_TAB ),
                                            new Point( x + SINGER_ITEM_WIDTH, size.Height - OFFSET_TRACK_TAB ) );
                            }
                        }
                        #endregion
                    }
                    #endregion
                }

                if ( CurveVisible ) {
                    #region カーブの種類一覧
                    Color font_color_normal = Color.Black;
                    g.FillRectangle( new SolidBrush( Color.FromArgb( 212, 212, 212 ) ),
                                     new Rectangle( 0, 0, AppManager.KEY_LENGTH, size.Height - 2 * OFFSET_TRACK_TAB ) );

                    // 数値ビュー
                    Rectangle num_view = new Rectangle( 13, 4, 38, 16 );
                    g.DrawRectangle( new Pen( Color.FromArgb( 125, 123, 124 ) ),
                                     num_view );
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Far;
                    g.DrawString( numeric_view.ToString(),
                                  AppManager.baseFont9,
                                  brs_string,
                                  num_view,
                                  sf );

                    // 現在表示されているカーブの名前
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Near;
                    g.DrawString( m_selected_curve.Name, AppManager.baseFont9, brs_string, new Rectangle( 7, 24, 56, 14 ), sf );

                    for ( Iterator itr = m_viewing_curves.iterator(); itr.hasNext(); ){
                        CurveType curve = (CurveType)itr.next();
                        Rectangle rc = getRectFromCurveType( curve );
                        if ( curve.equals( m_selected_curve ) || curve.equals( m_last_selected_curve ) ) {
                            g.FillRectangle( new SolidBrush( Color.FromArgb( 108, 108, 108 ) ), rc );
                        }
                        g.DrawRectangle( rect_curve, rc );
                        Rectangle rc_str = new Rectangle( rc.X, rc.Y + rc.Height / 2 - AppManager.baseFont9OffsetHeight, rc.Width, rc.Height );
                        rc_str.Y += 2;
                        if ( curve.equals( m_selected_curve ) ) {
                            g.DrawString( curve.Name,
                                          AppManager.baseFont9,
                                          Brushes.White,
                                          rc_str,
                                          sf );
                        } else {
                            g.DrawString( curve.Name,
                                          AppManager.baseFont9,
                                          new SolidBrush( font_color_normal ),
                                          rc_str,
                                          sf );
                        }
                    }
                    #endregion
                }

                #region 現在のマーカー
                int marker_x = xCoordFromClocks( AppManager.getCurrentClock() );
                if ( AppManager.KEY_LENGTH <= marker_x && marker_x <= size.Width ) {
                    g.DrawLine( new Pen( Color.White, 2f ),
                                new Point( marker_x, 0 ),
                                new Point( marker_x, size.Height - 18 ) );
                }
                #endregion

                // マウス位置での値
                if ( isInRect( mouse, new Rectangle( AppManager.KEY_LENGTH, HEADER, this.Width, this.GraphHeight ) ) &&
                     m_mouse_down_mode != MouseDownMode.PRE_UTTERANCE_MOVE && m_mouse_down_mode != MouseDownMode.OVERLAP_MOVE ) {
                    StringFormat sf_value = new StringFormat();
                    sf_value.Alignment = StringAlignment.Far;
                    sf_value.LineAlignment = StringAlignment.Center;
                    int shift = 50;
                    if ( m_selected_curve.equals( CurveType.PIT ) ) {
                        sf_value.LineAlignment = StringAlignment.Far;
                        shift = 100;
                    }
                    g.DrawString( m_mouse_value.ToString(),
                                  AppManager.baseFont10Bold,
                                  Brushes.White,
                                  new RectangleF( mouse.X - 100, mouse.Y - shift, 100, 100 ),
                                  sf_value );
                    if ( m_selected_curve.equals( CurveType.PIT ) ) {
                        float delta_note = m_mouse_value * pbs_at_mouse / 8192.0f;
                        StringFormat fs_pitch = new StringFormat();
                        fs_pitch.Alignment = StringAlignment.Far;
                        fs_pitch.LineAlignment = StringAlignment.Near;
                        g.DrawString( String.Format( "{0:#0.00}", delta_note ),
                                      AppManager.baseFont10Bold,
                                      Brushes.White,
                                      new RectangleF( mouse.X - 100, mouse.Y, 100, 100 ),
                                      fs_pitch );
                    }
                }

                #region 外枠
                // 左外側
                g.DrawLine( new Pen( Color.FromArgb( 160, 160, 160 ) ),
                            new Point( 0, 0 ),
                            new Point( 0, size.Height - 2 ) );
                // 左内側
                g.DrawLine( new Pen( Color.FromArgb( 105, 105, 105 ) ),
                            new Point( 1, 0 ),
                            new Point( 1, size.Height - 1 ) );
                // 下内側
                g.DrawLine( new Pen( Color.FromArgb( 192, 192, 192 ) ),
                            new Point( 1, size.Height - 2 ),
                            new Point( size.Width + 20, size.Height - 2 ) );
                // 下外側
                g.DrawLine(
                    Pens.White,
                    new Point( 0, size.Height - 1 ),
                    new Point( size.Width + 20, size.Height - 1 ) );
                /*/ 右外側
                g.DrawLine( Pens.White,
                            new Point( size.Width - 1, 0 ),
                            new Point( size.Width - 1, size.Height - 1 ) );
                // 右内側
                g.DrawLine( new Pen( Color.FromArgb( 227, 227, 227 ) ),
                            new Point( size.Width - 2, 0 ),
                            new Point( size.Width - 2, size.Height - 2 ) );*/
                #endregion
            }
#if DEBUG
            } catch( Exception ex ){
                AppManager.debugWriteLine( "    ex=" + ex );
            }
#endif
        }

        private void drawEnvelope( Graphics g, VsqTrack track, Color fill_color ) {
            int clock_start = clockFromXCoord( AppManager.KEY_LENGTH );
            int clock_end = clockFromXCoord( this.Width );

            VsqFileEx vsq = AppManager.getVsqFile();
            VsqEvent last = null;
            Point[] highlight = null;
            //int highlight_len = 0;
            //Vector<Point> highlight2 = new Vector<Point>();
            Point mouse = this.PointToClient( Control.MousePosition );
            int px_preutterance = int.MinValue;
            int px_overlap = int.MinValue;
            int drawn_id = -1;
            int distance = int.MaxValue;
            int drawn_preutterance = 0;
            int drawn_overlap = 0;

            using ( SolidBrush brs = new SolidBrush( fill_color ) ) {
                Point selected_point = Point.Empty;
                boolean selected_found = false;
                boolean search_mouse = (0 <= mouse.Y && mouse.Y <= this.Height);
                for ( Iterator itr = track.getNoteEventIterator(); itr.hasNext(); ) {
                    VsqEvent item = (VsqEvent)itr.next();
                    if ( last == null ) {
                        last = item;
                        continue;
                    }
                    if ( item.Clock + item.ID.Length < clock_start ) {
                        last = item;
                        continue;
                    }
                    if ( clock_end < item.Clock ) {
                        break;
                    }
                    int preutterance, overlap;
                    Point[] points = getEnvelopePoints( vsq, last, item, out preutterance, out overlap );
                    if ( m_mouse_down_mode == MouseDownMode.ENVELOPE_MOVE || 
                         m_mouse_down_mode == MouseDownMode.PRE_UTTERANCE_MOVE || 
                         m_mouse_down_mode == MouseDownMode.OVERLAP_MOVE ) {
                        if ( m_mouse_down_mode == MouseDownMode.ENVELOPE_MOVE && last.InternalID == m_envelope_move_id ) {
                            selected_point = points[m_envelope_point_kind];
                            selected_found = true;
                            highlight = points;
                            px_overlap = overlap;
                            px_preutterance = preutterance;
                            drawn_preutterance = last.UstEvent.PreUtterance;
                            drawn_overlap = last.UstEvent.VoiceOverlap;
                            drawn_id = last.InternalID;
                        } else if ( (m_mouse_down_mode == MouseDownMode.PRE_UTTERANCE_MOVE && last.InternalID == m_preutterance_moving_id) ||
                                    (m_mouse_down_mode == MouseDownMode.OVERLAP_MOVE && last.InternalID == m_overlap_moving_id) ) {
                            highlight = points;
                            px_overlap = overlap;
                            px_preutterance = preutterance;
                            drawn_preutterance = last.UstEvent.PreUtterance;
                            drawn_overlap = last.UstEvent.VoiceOverlap;
                            drawn_id = last.InternalID;
                        }
                    } else if ( search_mouse ){
                        int draft_distance = int.MaxValue;
                        if ( points[0].X <= mouse.X && mouse.X <= points[6].X ) {
                            draft_distance = 0;
                        } else if( mouse.X < points[0].X ) {
                            draft_distance = points[0].X - mouse.X;
                        } else {
                            draft_distance = mouse.X - points[6].X;
                        }
                        if ( distance > draft_distance ) {
                            distance = draft_distance;
                            highlight = points;
                            px_overlap = overlap;
                            px_preutterance = preutterance;
                            drawn_preutterance = last.UstEvent.PreUtterance;
                            drawn_overlap = last.UstEvent.VoiceOverlap;
                            drawn_id = last.InternalID;
                        }
                    }
                    g.FillPolygon( brs, points );
                    g.DrawLines( Pens.White, points );
                    last = item;
                }
                int dotwid = DOT_WID * 2 + 1;
                if ( vsq != null && last != null ) {
                    int overlap, preutterance;
                    Point[] points = getEnvelopePoints( vsq, last, null, out preutterance, out overlap );
                    if ( m_mouse_down_mode == MouseDownMode.ENVELOPE_MOVE ||
                         m_mouse_down_mode == MouseDownMode.PRE_UTTERANCE_MOVE ||
                         m_mouse_down_mode == MouseDownMode.OVERLAP_MOVE ) {
                        if ( m_mouse_down_mode == MouseDownMode.ENVELOPE_MOVE && last.InternalID == m_envelope_move_id){
                            selected_point = points[m_envelope_point_kind];
                            selected_found = true;
                            highlight = points;
                            px_overlap = overlap;
                            px_preutterance = preutterance;
                            drawn_preutterance = last.UstEvent.PreUtterance;
                            drawn_overlap = last.UstEvent.VoiceOverlap;
                            drawn_id = last.InternalID;
                        } else if ( (m_mouse_down_mode == MouseDownMode.PRE_UTTERANCE_MOVE && last.InternalID == m_preutterance_moving_id) ||
                                    (m_mouse_down_mode == MouseDownMode.OVERLAP_MOVE && last.InternalID == m_overlap_moving_id) ) {
                            highlight = points;
                            px_overlap = overlap;
                            px_preutterance = preutterance;
                            drawn_preutterance = last.UstEvent.PreUtterance;
                            drawn_overlap = last.UstEvent.VoiceOverlap;
                            drawn_id = last.InternalID;
                        }
                    } else if ( search_mouse ) {
                        int draft_distance = int.MaxValue;
                        if ( points[0].X - dotwid <= mouse.X && mouse.X <= points[6].X + dotwid ) {
                            for ( int i = 1; i < 6; i++ ) {
                                Point p = points[i];
                                Rectangle rc = new Rectangle( p.X - DOT_WID, p.Y - DOT_WID, dotwid, dotwid );
                                g.FillRectangle( s_brs_dot_normal, rc );
                                g.DrawRectangle( s_pen_dot_normal, rc );
                            }
                            draft_distance = 0;
                        } else if ( mouse.X < points[0].X ) {
                            draft_distance = points[0].X - mouse.X;
                        } else {
                            draft_distance = mouse.X - points[6].X;
                        }
                        if ( distance > draft_distance ) {
                            distance = draft_distance;
                            highlight = points;
                            px_overlap = overlap;
                            px_preutterance = preutterance;
                            drawn_preutterance = last.UstEvent.PreUtterance;
                            drawn_overlap = last.UstEvent.VoiceOverlap;
                            drawn_id = last.InternalID;
                        }
                    }
                    g.FillPolygon( brs, points );
                    g.DrawLines( Pens.White, points );
                }
                if ( highlight != null ) {
                    for ( int i = 1; i < 6; i++ ) {
                        Point p = highlight[i];
                        Rectangle rc = new Rectangle( p.X - DOT_WID, p.Y - DOT_WID, dotwid, dotwid );
                        g.FillRectangle( s_brs_dot_normal, rc );
                        g.DrawRectangle( s_pen_dot_normal, rc );
                    }
                }
                if ( selected_found ) {
                    Rectangle rc = new Rectangle( selected_point.X - DOT_WID, selected_point.Y - DOT_WID, dotwid, dotwid );
                    g.FillRectangle( AppManager.getHilightBrush(), rc );
                    g.DrawRectangle( s_pen_dot_normal, rc );
                }

                if ( px_preutterance != int.MinValue && px_overlap != int.MinValue ) {
                    drawPreutteranceAndOverlap( g, px_preutterance, px_overlap, drawn_preutterance, drawn_overlap );
                }
                m_overlap_viewing = drawn_id;
                m_preutterance_viewing = drawn_id;
            }
        }

        private void drawPreutteranceAndOverlap( Graphics g, int px_preutterance, int px_overlap, int preutterance, int overlap ) {
            const int OFFSET_PRE = 15;
            const int OFFSET_OVL = 40;
            g.DrawLine( Pens.Orange, new Point( px_preutterance, HEADER + 1 ), new Point( px_preutterance, GraphHeight + HEADER ) );
            g.DrawLine( Pens.LawnGreen, new Point( px_overlap, HEADER + 1 ), new Point( px_overlap, GraphHeight + HEADER ) );

            String s_pre = "Pre Utterance: " + preutterance;
            SizeF size = AppManager.measureString( s_pre, AppManager.baseFont10 );
            m_preutterance_bounds = new Rectangle( px_preutterance + 1, OFFSET_PRE, (int)size.Width, (int)size.Height );
            String s_ovl = "Overlap: " + overlap;
            size = AppManager.measureString( s_ovl, AppManager.baseFont10 );
            m_overlap_bounds = new Rectangle( px_overlap + 1, OFFSET_OVL, (int)size.Width, (int)size.Height );

            using ( Pen pen = new Pen( Color.FromArgb( 50, Color.Black ) ) ) {
                using ( SolidBrush transp = new SolidBrush( Color.FromArgb( 50, Color.Orange ) ) ) {
                    g.FillRectangle( transp, m_preutterance_bounds );
                    g.DrawRectangle( pen, m_preutterance_bounds );
                }
                using ( SolidBrush transp = new SolidBrush( Color.FromArgb( 50, Color.LawnGreen ) ) ) {
                    g.FillRectangle( transp, m_overlap_bounds );
                    g.DrawRectangle( pen, m_overlap_bounds );
                }
            }

            g.DrawString( s_pre, AppManager.baseFont10, Brushes.Black, new PointF( px_preutterance + 1, OFFSET_PRE ) );
            g.DrawString( s_ovl, AppManager.baseFont10, Brushes.Black, new PointF( px_overlap + 1, OFFSET_OVL ) );
        }

        /// <summary>
        /// 画面上の指定した点に、コントロールカーブのデータ点があるかどうかを調べます
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private long findDataPointAt( Point location ) {
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
                VsqBPList list = AppManager.getVsqFile().Track[AppManager.getSelected()].getCurve( m_selected_curve.Name );
                int count = list.size();
                int w = DOT_WID * 2 + 1;
                for ( int i = 0; i < count; i++ ) {
                    int clock = list.getKeyClock( i );
                    VsqBPPair item = list.getElementB( i );
                    int x = xCoordFromClocks( clock );
                    if ( x + DOT_WID < AppManager.KEY_LENGTH ) {
                        continue;
                    }
                    if ( this.Width < x - DOT_WID ) {
                        break;
                    }
                    int y = yCoordFromValue( item.value );
                    Rectangle rc = new Rectangle( x - DOT_WID, y - DOT_WID, w, w );
                    if ( isInRect( location, rc ) ) {
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
        private boolean findEnvelopePointAt( Point location, out int internal_id, out int point_kind ) {
            int clock_start = clockFromXCoord( AppManager.KEY_LENGTH );
            int clock_end = clockFromXCoord( this.Width );
            VsqEvent last = null;
            int dotwid = DOT_WID * 2 + 1;
            VsqFileEx vsq = AppManager.getVsqFile();
            for ( Iterator itr = vsq.Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = (VsqEvent)itr.next();
                if ( last == null ) {
                    last = item;
                    continue;
                }
                if ( item.Clock + item.ID.Length < clock_start ) {
                    last = item;
                    continue;
                }
                if ( clock_end < last.Clock ) {
                    last = item;
                    break;
                }
                Point[] points = getEnvelopePoints( vsq, last, item );
                for ( int i = 5; i >= 1; i-- ) {
                    Point p = points[i];
                    Rectangle rc = new Rectangle( p.X - DOT_WID, p.Y - DOT_WID, dotwid, dotwid );
                    if ( isInRect( location, rc ) ) {
                        internal_id = last.InternalID;
                        point_kind = i;
                        return true;
                    }
                }
                last = item;
            }
            if ( last != null ) {
                Point[] points = getEnvelopePoints( vsq, last, null );
                for ( int i = 5; i >= 1; i-- ) {
                    Point p = points[i];
                    Rectangle rc = new Rectangle( p.X - DOT_WID, p.Y - DOT_WID, dotwid, dotwid );
                    if ( isInRect( location, rc ) ) {
                        internal_id = last.InternalID;
                        point_kind = i;
                        return true;
                    }
                }
            }
            internal_id = -1;
            point_kind = -1;
            return false;
        }

        private Point[] getEnvelopePoints( VsqFileEx vsq, VsqEvent item, VsqEvent next_item ) {
            int i, j;
            return getEnvelopePoints( vsq, item, next_item, out i, out j );
        }

        private Point[] getEnvelopePoints( VsqFileEx vsq, VsqEvent item, VsqEvent next_item, out int px_pre_utteramce, out int px_overlap ) {
            double sec_start = vsq.getSecFromClock( item.Clock );
            double sec_end = vsq.getSecFromClock( item.Clock + item.ID.Length );
            UstEvent ust_event = item.UstEvent;
            if ( ust_event == null ){
                ust_event = new UstEvent();
            }
            UstEnvelope draw_target = ust_event.Envelope;
            if ( draw_target == null ) {
                draw_target = new UstEnvelope();
            }
            int pre_utterance = ust_event.PreUtterance;
            int overlap = ust_event.VoiceOverlap;

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
            int p_start = xCoordFromClocks( (int)vsq.getClockFromSec( sec_start ) );
            int p_end = xCoordFromClocks( (int)vsq.getClockFromSec( sec_end ) );
            px_pre_utteramce = p_start;
            px_overlap = xCoordFromClocks( (int)vsq.getClockFromSec( sec_start + overlap / 1000.0 ) );
            int p1 = xCoordFromClocks( (int)vsq.getClockFromSec( sec_start + draw_target.p1 / 1000.0 ) );
            int p2 = xCoordFromClocks( (int)vsq.getClockFromSec( sec_start + (draw_target.p1 + draw_target.p2) / 1000.0 ) );
            int p5 = xCoordFromClocks( (int)vsq.getClockFromSec( sec_start + (draw_target.p1 + draw_target.p2 + draw_target.p5) / 1000.0 ) );
            int p3 = xCoordFromClocks( (int)vsq.getClockFromSec( sec_end - (draw_target.p4 + draw_target.p3) / 1000.0 ) );
            int p4 = xCoordFromClocks( (int)vsq.getClockFromSec( sec_end - draw_target.p4 / 1000.0 ) );
            int v1 = yCoordFromValue( draw_target.v1 );
            int v2 = yCoordFromValue( draw_target.v2 );
            int v3 = yCoordFromValue( draw_target.v3 );
            int v4 = yCoordFromValue( draw_target.v4 );
            int v5 = yCoordFromValue( draw_target.v5 );
            int y = yCoordFromValue( 0 );
            Point[] points = new Point[] { new Point( p_start, y ),
                                               new Point( p1, v1 ),
                                               new Point( p2, v2 ),
                                               new Point( p5, v5 ),
                                               new Point( p3, v3 ),
                                               new Point( p4, v4 ),
                                               new Point( p_end, y ) };
            return points;
        }

        private void drawTrackTab( Graphics g, Rectangle destRect, String name, boolean selected, boolean enabled, boolean render_required, Color hilight, Color render_button_hilight ) {
            int x = destRect.X;
            int panel_width = render_required ? destRect.Width - 10 : destRect.Width;
            Color panel_color = enabled ? hilight : Color.FromArgb( 125, 123, 124 );
            Color button_color = enabled ? render_button_hilight : Color.FromArgb( 125, 123, 124 );
            Color panel_title = Color.Black;
            Color button_title = selected ? Color.White : Color.Black;
            Color border = selected ? Color.White : Color.FromArgb( 118, 123, 138 );

            // 背景(選択されている場合)
            if ( selected ) {
                g.FillRectangle( new SolidBrush( panel_color ),
                                 destRect );
                if ( render_required && enabled ) {
                    g.FillRectangle( new SolidBrush( render_button_hilight ),
                                     new Rectangle( destRect.X + destRect.Width - 10, destRect.Y, 10, destRect.Height ) );
                }
            }

            // 左縦線
            g.DrawLine( new Pen( border ),
                        new Point( destRect.X, destRect.Y ),
                        new Point( destRect.X, destRect.Y + destRect.Height ) );
            if ( name.Length > 0 ) {
                // 上横線
                g.DrawLine( new Pen( border ),
                            new Point( destRect.X + 1, destRect.Y ),
                            new Point( destRect.X + destRect.Width, destRect.Y ) );
            }
            if ( render_required ) {
                g.DrawLine(
                    new Pen( border ),
                    new Point( destRect.X + destRect.Width - 10, destRect.Y ),
                    new Point( destRect.X + destRect.Width - 10, destRect.Y + destRect.Height ) );
            }
            g.SetClip( destRect );
            String title = AppManager.trimString( name, AppManager.baseFont8, panel_width );
            g.DrawString( title,
                          AppManager.baseFont8,
                          new SolidBrush( panel_title ),
                          new PointF( destRect.X + 2, destRect.Y + destRect.Height / 2 - AppManager.baseFont8OffsetHeight ) );
            if ( render_required ) {
                g.DrawString( "R",
                              AppManager.baseFont8,
                              new SolidBrush( button_title ),
                              new PointF( destRect.X + destRect.Width - _PX_WIDTH_RENDER, destRect.Y + destRect.Height / 2 - AppManager.baseFont8OffsetHeight ) );
            }
            if ( selected ) {
                g.DrawLine( new Pen( border ),
                            new Point( destRect.X + destRect.Width - 1, destRect.Y ),
                            new Point( destRect.X + destRect.Width - 1, destRect.Y + destRect.Height ) );
                g.DrawLine( new Pen( border ),
                            new Point( destRect.X, destRect.Y + destRect.Height - 1 ),
                            new Point( destRect.X + destRect.Width, destRect.Y + destRect.Height - 1 ) );
            }
            g.ResetClip();
            g.DrawLine( m_generic_line,
                        new Point( destRect.X + destRect.Width, destRect.Y ),
                        new Point( destRect.X + destRect.Width, destRect.Y + destRect.Height ) );
        }

        /// <summary>
        /// トラック選択部分の、トラック1個分の幅を調べます。pixel
        /// </summary>
        public int SelectorWidth {
            get {
                return (int)((this.Width - vScroll.Width) / 16.0f);
            }
        }

        /// <summary>
        /// ベロシティを、与えられたグラフィックgを用いて描画します
        /// </summary>
        /// <param name="g"></param>
        /// <param name="track"></param>
        /// <param name="color"></param>
        public void drawVEL( Graphics g, VsqTrack track, Color color, boolean is_front, CurveType type ) {
            int HEADER = 8;
            int height = GraphHeight;
            float order = (type.equals( CurveType.VEL )) ? height / 127f : height / 100f;
            int oy = this.Height - 42;
            Region last_clip = g.Clip;
            int xoffset = 6 + AppManager.KEY_LENGTH - AppManager.startToDrawX;
            g.SetClip( new Rectangle( AppManager.KEY_LENGTH, HEADER, this.Width - AppManager.KEY_LENGTH - vScroll.Width, height ) );
            float scale = AppManager.scaleX;
            int count = track.getEventCount();
            for ( int i = 0; i < count; i++ ) {
                VsqEvent ve = track.getEvent( i );
                if ( ve.ID.type != VsqIDType.Anote ) {
                    continue;
                }
                int clock = ve.Clock;
                int x = (int)(clock * scale) + xoffset;
                if ( x + VEL_BAR_WIDTH < 0 ) {
                    continue;
                } else if ( this.Width - vScroll.Width < x ) {
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
                        g.FillRectangle( s_brs_a127_008_166_172,
                                         new Rectangle( x, y, VEL_BAR_WIDTH, oy - y ) );
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
                                g.FillRectangle( BRS_A244_255_023_012,
                                                 new Rectangle( x, edit_y, VEL_BAR_WIDTH, oy - edit_y ) );
                            }
                        }
                    } else {
                        g.FillRectangle( new SolidBrush( color ),
                                         new Rectangle( x, y, VEL_BAR_WIDTH, oy - y ) );
                    }
                }
            }
            g.SetClip( last_clip, CombineMode.Replace );
        }

        private void drawAttachedCurve( Graphics g, Vector<BezierChain> chains ) {
#if DEBUG
            try {
                BezierCurves t;
#endif
                int chains_count = chains.size();
                for( int i = 0; i < chains_count; i++ ){
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
                        int next_x = xCoordFromClocks( (int)next.getBase().X );
                        pxNext = new Point( next_x, yCoordFromValue( (int)next.getBase().Y ) );
                        Point pxControlCurrent = getScreenCoord( current.getControlRight() );
                        Point pxControlNext = getScreenCoord( next.getControlLeft() );

                        // ベジエ曲線本体を描く
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        if ( current.getControlRightType() == BezierControlType.None &&
                             next.getControlLeftType() == BezierControlType.None ) {
                            g.DrawLine( s_pen_curve, pxCurrent, pxNext );
                        } else {
                            g.DrawBezier( s_pen_curve,
                                          pxCurrent,
                                          (current.getControlRightType() == BezierControlType.None) ? pxCurrent : pxControlCurrent,
                                          (next.getControlLeftType() == BezierControlType.None) ? pxNext : pxControlNext,
                                          pxNext );
                        }

                        if ( current.getControlRightType() != BezierControlType.None ) {
                            g.DrawLine( s_pen_control_line, pxCurrent, pxControlCurrent );
                        }
                        if ( next.getControlLeftType() != BezierControlType.None ) {
                            g.DrawLine( s_pen_control_line, pxNext, pxControlNext );
                        }
                        g.SmoothingMode = SmoothingMode.Default;

                        // コントロール点
                        if ( current.getControlRightType() == BezierControlType.Normal ) {
                            Rectangle rc = new Rectangle( pxControlCurrent.X - DOT_WID,
                                                          pxControlCurrent.Y - DOT_WID,
                                                          DOT_WID * 2 + 1,
                                                          DOT_WID * 2 + 1 );
                            if ( chain_id == EditingChainID && current.ID == EditingPointID ) {
                                g.FillEllipse( AppManager.getHilightBrush(), rc );
                            } else {
                                g.FillEllipse( s_brs_dot_normal, rc );
                            }
                            g.DrawEllipse( s_pen_dot_normal, rc );
                        }

                        // コントロール点
                        if ( next.getControlLeftType() == BezierControlType.Normal ) {
                            Rectangle rc = new Rectangle( pxControlNext.X - DOT_WID,
                                                          pxControlNext.Y - DOT_WID,
                                                          DOT_WID * 2 + 1,
                                                          DOT_WID * 2 + 1 );
                            if ( chain_id == EditingChainID && next.ID == EditingPointID ) {
                                g.FillEllipse( AppManager.getHilightBrush(), rc );
                            } else {
                                g.FillEllipse( s_brs_dot_normal, rc );
                            }
                            g.DrawEllipse( s_pen_dot_normal, rc );
                        }

                        // データ点
                        Rectangle rc2 = new Rectangle( pxCurrent.X - DOT_WID,
                                                        pxCurrent.Y - DOT_WID,
                                                        DOT_WID * 2 + 1,
                                                        DOT_WID * 2 + 1 );
                        if ( chain_id == EditingChainID && current.ID == EditingPointID ) {
                            g.FillRectangle( AppManager.getHilightBrush(), rc2 );
                        } else {
                            g.FillRectangle( s_brs_dot_base, rc2 );
                        }
                        g.DrawRectangle( s_pen_dot_base, rc2 );
                        pxCurrent = pxNext;
                        current = next;
                    }
                    next = target_chain.points.get( target_chain.points.size() - 1 );
                    pxNext = getScreenCoord( next.getBase() );
                    Rectangle rc_last = new Rectangle( pxNext.X - DOT_WID,
                                                       pxNext.Y - DOT_WID,
                                                       DOT_WID * 2 + 1,
                                                       DOT_WID * 2 + 1 );
                    if ( chain_id == EditingChainID && next.ID == EditingPointID ) {
                        g.FillRectangle( AppManager.getHilightBrush(), rc_last );
                    } else {
                        g.FillRectangle( s_brs_dot_base, rc_last );
                    }
                    g.DrawRectangle( s_pen_dot_base, rc_last );
                }
#if DEBUG
            } catch ( Exception ex ) {
                AppManager.debugWriteLine( "TrackSelector+DrawAttatchedCurve" );
                AppManager.debugWriteLine( "    ex=" + ex );
            }
#endif
        }

        private Point getScreenCoord( PointD pt ) {
            return new Point( xCoordFromClocks( (int)pt.X ), yCoordFromValue( (int)pt.Y ) );
        }

        // TODO: TrackSelector+DrawVibratoControlCurve; かきかけ
        public void drawVibratoControlCurve( Graphics g, VsqTrack draw_target, CurveType type, Color color, boolean is_front ) {
#if DEBUG
            //AppManager.DebugWriteLine( "TrackSelector+DrawVibratoControlCurve" );
#endif
            Region last_clip = g.Clip;
            int graph_height = GraphHeight;
            g.SetClip( new Rectangle( AppManager.KEY_LENGTH,
                                      HEADER,
                                      this.Width - AppManager.KEY_LENGTH - vScroll.Width,
                                      graph_height ),
                       CombineMode.Replace );

            //int track = AppManager.Selected;
            int cl_start = clockFromXCoord( AppManager.KEY_LENGTH );
            int cl_end = clockFromXCoord( this.Width - vScroll.Width );
            if ( is_front ) {
                // draw shadow of non-note area
                Region last_clip2 = g.Clip;
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
                        g.SetClip( new Rectangle( x1, HEADER, x2 - x1, graph_height ), CombineMode.Exclude );
                    }
                }
                g.FillRectangle( new SolidBrush( Color.FromArgb( 127, Color.Black ) ),
                                 new Rectangle( AppManager.KEY_LENGTH, HEADER, this.Width - AppManager.KEY_LENGTH - vScroll.Width, graph_height ) );

                g.SetClip( last_clip, CombineMode.Replace );

                // draw curve
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
                    if ( x1 < x2 ) {
                        g.SetClip( new Rectangle( x1, HEADER, x2 - x1, graph_height ), CombineMode.Replace );
                        Vector<Point> poly = new Vector<Point>();
                        int draw_width = x2 - x1;
                        poly.add( new Point( x1, yCoordFromValue( 0, type.Maximum, type.Minimum ) ) );
                        if ( type.equals( CurveType.VibratoRate ) ) {
                            int last_y = yCoordFromValue( ve.ID.VibratoHandle.StartRate, type.Maximum, type.Minimum );
                            poly.add( new Point( x1, last_y ) );
                            int c = ve.ID.VibratoHandle.RateBP.getCount();
                            for ( int i = 0; i < c; i++ ) {
                                VibratoBPPair item = ve.ID.VibratoHandle.RateBP.getElement( i );
                                int x = x1 + (int)(item.X * draw_width);
                                int y = yCoordFromValue( item.Y, type.Maximum, type.Minimum );
                                poly.add( new Point( x, last_y ) );
                                poly.add( new Point( x, y ) );
                                last_y = y;
                            }
                            poly.add( new Point( x2, last_y ) );
                        } else {
                            int last_y = yCoordFromValue( ve.ID.VibratoHandle.StartDepth, type.Maximum, type.Minimum );
                            poly.add( new Point( x1, last_y ) );
                            int c = ve.ID.VibratoHandle.DepthBP.getCount();
                            for ( int i = 0; i < c; i++ ) {
                                VibratoBPPair item = ve.ID.VibratoHandle.DepthBP.getElement( i );
                                int x = x1 + (int)(item.X * draw_width);
                                int y = yCoordFromValue( item.Y, type.Maximum, type.Minimum );
                                poly.add( new Point( x, last_y ) );
                                poly.add( new Point( x, y ) );
                                last_y = y;
                            }
                            poly.add( new Point( x2, last_y ) );
                        }
                        poly.add( new Point( x2, yCoordFromValue( 0, type.Maximum, type.Minimum ) ) );
                        g.FillPolygon( new SolidBrush( color ), poly.toArray( new Point[]{} ) );
                    }
                }
            }
            g.SetClip( last_clip, CombineMode.Replace );
        }

        /// <summary>
        /// Draws VsqBPList using specified Graphics "g", toward rectangular region "rect".
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="list"></param>
        /// <param name="color"></param>
        public void drawVsqBPList( Graphics g, VsqBPList list, Color color, boolean is_front ) {
            int max = list.getMaximum();
            int min = list.getMinimum();
            int height = GraphHeight;
            float order = height / (float)(max - min);
            int oy = this.Height - 42;
            Region last_clip = g.Clip.Clone();
            g.SetClip( new Rectangle( AppManager.KEY_LENGTH, 
                                      HEADER, 
                                      this.Width - AppManager.KEY_LENGTH - vScroll.Width, 
                                      this.Height - 2 * OFFSET_TRACK_TAB ), 
                       CombineMode.Replace );

            // 選択範囲。この四角の中に入っていたら、選択されているとみなす
            Rectangle select_window = new Rectangle( Math.Min( m_selecting_region.X, m_selecting_region.X + m_selecting_region.Width ),
                                                     Math.Min( m_selecting_region.Y, m_selecting_region.Y + m_selecting_region.Height ),
                                                     Math.Abs( m_selecting_region.Width ),
                                                     Math.Abs( m_selecting_region.Height ) );
            EditTool selected_tool = AppManager.getSelectedTool();
            boolean select_enabled = !m_selected_curve.IsScalar && ((selected_tool == EditTool.ARROW) || (selected_tool == EditTool.ERASER));

            int start = AppManager.KEY_LENGTH;
            int start_clock = clockFromXCoord( start );
            int end = this.Width - vScroll.Width;
            int end_clock = clockFromXCoord( end );
            int hilight_start = m_selected_region.getStart();
            int hilight_end = m_selected_region.getEnd();
            int hilight_start_x = xCoordFromClocks( hilight_start );
            int hilight_end_x = xCoordFromClocks( hilight_end );
            boolean hilight_enabled = false;
            Vector<Point> hilight = new Vector<Point>();
            Rectangle clip_hilight = Rectangle.Empty;
            if ( is_front && AppManager.getSelectedPointIDCount() > 0 ) {
                if ( start < hilight_end_x && hilight_start_x < end ) {
                    hilight_enabled = true;
                }
            }

            SolidBrush brush = new SolidBrush( color );
            Vector<Point> points = new Vector<Point>();
            Vector<Integer> index_selected_in_points = new Vector<Integer>(); // pointsのうち、選択された状態のものが格納されたインデックス
            points.add( new Point( this.Width - vScroll.Width, oy ) );
            points.add( new Point( AppManager.KEY_LENGTH, oy ) );
            int first_y = list.getValue( start_clock );
            int last_y = oy - (int)((first_y - min) * order);

            boolean first = true;
            if ( list.size() > 0 ) {
                int first_clock = list.getKeyClock( 0 );
                int last_x = xCoordFromClocks( first_clock );
                first_y = list.getValue( first_clock );
                last_y = oy - (int)((first_y - min) * order);

                boolean hilight_first = true;
                boolean hilight_entered = false;
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
                        last_y = yCoordFromValue( list.getValue( clockFromXCoord( AppManager.KEY_LENGTH ) ),
                                                  max,
                                                  min );
                        points.add( new Point( AppManager.KEY_LENGTH, last_y ) );
                        first = false;
                    }
                    
                    int x = xCoordFromClocks( clock );
                    VsqBPPair v = list.getElementB( i );
                    int y = oy - (int)((v.value - min) * order);

                    if ( hilight_enabled ) {
                        if ( hilight_start_x <= x && x <= hilight_end_x ) {
                            if ( hilight_first ) {
                                hilight.add( new Point( hilight_start_x, oy ) );
                                hilight.add( new Point( hilight_start_x, last_y ) );
                                hilight_first = false;
                                hilight_entered = true;
                            }
                            hilight.add( new Point( x, last_y ) );
                            hilight.add( new Point( x, y ) );
                            //index_selected_in_hilight.add( hilight.size() - 1 );
                        } else if ( hilight_entered && hilight_end_x < x ) {
                            hilight.add( new Point( hilight_end_x, last_y ) );
                            hilight.add( new Point( hilight_end_x, oy ) );
                        }
                    }
                    points.add( new Point( x, last_y ) );
                    points.add( new Point( x, y ) );
                    if ( AppManager.isSelectedPointContains( v.id ) ) {
                        index_selected_in_points.add( points.size() - 1 );
                    } else if( select_enabled && isInRect( new Point( clock, v.value ), select_window ) ) {
                        index_selected_in_points.add( points.size() - 1 );
                    }
                    last_y = y;
                }
            }
            if ( first ) {
                last_y = yCoordFromValue( list.getValue( clockFromXCoord( AppManager.KEY_LENGTH ) ),
                                          max,
                                          min );
                points.add( new Point( AppManager.KEY_LENGTH, last_y ) );
            }
            last_y = oy - (int)((list.getValue( end_clock ) - min) * order);
            points.add( new Point( this.Width - vScroll.Width, last_y ) );
            g.FillPolygon( brush, points.toArray( new Point[]{} ) );
            brush.Dispose();
            brush = null;

            if ( is_front ) {
                // データ点を描画
                int c_points = points.size();
                int w = DOT_WID * 2 + 1;
                using ( Pen pen = new Pen( Color.White ) ) {
                    Point b1 = points[0];
                    Point b2 = points[1];
                    points.removeElementAt( 0 );
                    points.removeElementAt( 0 );
                    g.DrawLines( pen, points.toArray( new Point[] { } ) );
                    points.insertElementAt( b2, 0 );
                    points.insertElementAt( b1, 0 );
                }

                boolean draw_dot_near_mouse = true; // マウスの近くのデータ点だけ描画するモード
                int threshold_near_px = 200;  // マウスに「近い」と判定する距離（ピクセル単位）。
                Point mouse = this.PointToClient( Control.MousePosition );
                using ( SolidBrush white5 = new SolidBrush( Color.FromArgb( 200, Color.White ) ) ) {
                    for ( int i = 4; i < c_points; i += 2 ) {
                        Point p = points.get( i );
                        if ( index_selected_in_points.contains( i ) ) {
                            g.FillRectangle( BRS_CURVE_COLOR_DOT, new Rectangle( p.X - DOT_WID, p.Y - DOT_WID, w, w ) );
                        } else {
                            if ( draw_dot_near_mouse ) {
                                if ( mouse.Y < 0 || this.Height < mouse.Y ) {
                                    continue;
                                }
                                double x = Math.Abs( p.X - mouse.X ) / (double)threshold_near_px;
                                double sigma = 0.3;
                                int alpha = (int)(255.0 * Math.Exp( -(x * x) / (2.0 * sigma * sigma) ));
                                if ( alpha <= 0 ) {
                                    continue;
                                }
                                using ( SolidBrush brs = new SolidBrush( Color.FromArgb( alpha, Color.White ) ) ) {
                                    g.FillRectangle( brs, new Rectangle( p.X - DOT_WID, p.Y - DOT_WID, w, w ) );
                                }
                            } else {
                                g.FillRectangle( white5, new Rectangle( p.X - DOT_WID, p.Y - DOT_WID, w, w ) );
                            }
                        }
                    }
                }
            }

            // m_mouse_down_modeごとの描画
            if ( is_front ) {
                if ( m_mouse_down_mode == MouseDownMode.POINT_MOVE ) {
                    Point mouse = this.PointToClient( Control.MousePosition );
                    int dx = mouse.X + AppManager.startToDrawX - m_mouse_down_location.X;
                    int dy = mouse.Y - m_mouse_down_location.Y;
                    int w = DOT_WID * 2 + 1;
                    for ( Iterator itr = m_moving_points.iterator(); itr.hasNext(); ) {
                        BPPair item = (BPPair)itr.next();
                        int x = xCoordFromClocks( item.Clock ) + dx;
                        int y = yCoordFromValue( item.Value ) + dy;
                        g.FillRectangle( BRS_CURVE_COLOR_DOT, new Rectangle( x - DOT_WID, y - DOT_WID, w, w ) );
                    }
                }
            }

            g.SetClip( last_clip, CombineMode.Replace );
        }

        /// <summary>
        /// VsqBPListを与えられたグラフィックgを用いて、領域rectに描画します。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="list"></param>
        /// <param name="color"></param>
        public void NEW_DrawVsqBPList( Graphics g, VsqBPList list, Color color, int start_x, int end_x, Point position ) {
            int max = list.getMaximum();
            int min = list.getMinimum();
            int height = GraphHeight;
            float order = height / (float)(max - min);
            int oy = this.Height - 42;
            Region last_clip = g.Clip;
            g.SetClip( new Rectangle( position.X + AppManager.KEY_LENGTH, position.Y + HEADER, this.Width - AppManager.KEY_LENGTH - vScroll.Width, this.Height - 2 * OFFSET_TRACK_TAB ) );
            SolidBrush brush = new SolidBrush( color );
            int count = -1;
            count++;
            m_points[count].X = position.X + AppManager.KEY_LENGTH;
            m_points[count].Y = position.Y + oy;
            int first_y = list.getValue( 0 );
            int last_y = oy - (int)((first_y - min) * order) + position.Y;
            count++;
            m_points[count].X = AppManager.KEY_LENGTH;
            m_points[count].Y = last_y;
            count--;

            int xoffset = -AppManager.startToDrawX + 73 + position.X;

            if ( list.size() > 0 ) {
                int[] keys = list.getKeys();
                int first_clock = keys[0];
                first_y = list.getValue( first_clock );
                last_y = oy - (int)((first_y - min) * order) + position.Y;
                int last_x = xCoordFromClocks( first_clock ) + position.X;
                boolean first = true;
                for ( int i = 1; i < keys.Length; i++ ) {
                    int clock = keys[i];
                    int x = (int)(clock * AppManager.scaleX) + xoffset;
                    int y = oy - (int)((list.getValue( clock ) - min) * order) + position.Y;
                    if ( x < start_x ) {
                        last_y = y;
                        continue;
                    }
                    if ( first ) {
                        count++;
                        m_points[count].X = position.X + AppManager.KEY_LENGTH;
                        m_points[count].Y = last_y;
                        first = false;
                    }
                    if ( count == BUF_LEN - 1 ) { //このif文の位置，BUF_LENが偶数であることを利用
                        m_points[BUF_LEN - 1].Y = position.Y + oy;
                        g.FillPolygon( brush, m_points );
                        m_points[0].X = m_points[BUF_LEN - 1].X;
                        m_points[0].Y = position.Y + oy;
                        m_points[1].X = m_points[BUF_LEN - 1].X;
                        m_points[1].Y = m_points[BUF_LEN - 1].Y;
                        count = 1;
                    }
                    count++;
                    m_points[count].X = x;
                    m_points[count].Y = last_y;
                    count++;
                    m_points[count].X = x;
                    m_points[count].Y = y;
                    if ( end_x < last_x ) {
                        break;
                    }
                    last_y = y;
                    last_x = x;
                }
            }
            count++;
            m_points[count].X = position.X + this.Width - vScroll.Width;
            m_points[count].Y = position.Y + last_y;
            count++;
            m_points[count].X = position.X + this.Width - vScroll.Width;
            m_points[count].Y = position.Y + oy;
            Point[] pbuf = new Point[count];
            Array.Copy( m_points, pbuf, count );
            g.FillPolygon( brush, pbuf );
            brush.Dispose();
            g.SetClip( last_clip, CombineMode.Replace );
        }

        /// <summary>
        /// カーブエディタのグラフ部分の高さを取得します(pixel)
        /// </summary>
        public int GraphHeight {
            get {
                return this.Height - 42 - 8;
            }
        }

        /// <summary>
        /// カーブエディタのグラフ部分の幅を取得します。(pixel)
        /// </summary>
        public int GraphWidth {
            get {
                return this.Width - AppManager.KEY_LENGTH - vScroll.Width;
            }
        }

        private void TrackSelector_Load( object sender, EventArgs e ) {
            this.SetStyle( ControlStyles.DoubleBuffer, true );
            this.SetStyle( ControlStyles.UserPaint, true );
            this.SetStyle( ControlStyles.AllPaintingInWmPaint, true );
        }

        private void TrackSelector_MouseClick( object sender, MouseEventArgs e ) {
            if ( CurveVisible ) {
                if ( e.Button == MouseButtons.Left ) {
                    // カーブの種類一覧上で発生したイベントかどうかを検査
                    for ( Iterator itr = m_viewing_curves.iterator(); itr.hasNext(); ){
                        CurveType curve = (CurveType)itr.next();
                        Rectangle r = getRectFromCurveType( curve );
                        if ( isInRect( e.Location, r ) ) {
                            changeCurve( curve );
                            return;
                        }
                    }
                } else if ( e.Button == MouseButtons.Right ) {
                    if ( 0 <= e.X && e.X <= AppManager.KEY_LENGTH &&
                         0 <= e.Y && e.Y <= this.Height - 2 * OFFSET_TRACK_TAB ) {
                        foreach ( ToolStripItem tsi in cmenuCurve.Items ) {
                            if ( tsi is ToolStripMenuItem ) {
                                ToolStripMenuItem tsmi = (ToolStripMenuItem)tsi;
                                tsmi.Checked = false;
                                foreach ( ToolStripItem tsi2 in tsmi.DropDownItems ) {
                                    if ( tsi2 is ToolStripMenuItem ) {
                                        ToolStripMenuItem tsmi2 = (ToolStripMenuItem)tsi2;
                                        tsmi2.Checked = false;
                                    }
                                }
                            }
                        }
                        String version = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version;
                        if ( version.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
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
                        } else if ( version.StartsWith( VSTiProxy.RENDERER_UTU0 ) || version.StartsWith( VSTiProxy.RENDERER_STR0 ) ) {
                            cmenuCurveVelocity.Visible = false;
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
                        }
                        foreach ( ToolStripItem tsi in cmenuCurve.Items ) {
                            if ( tsi is ToolStripMenuItem ) {
                                ToolStripMenuItem tsmi = (ToolStripMenuItem)tsi;
                                if ( tsmi.Tag is CurveType ) {
                                    CurveType ct = (CurveType)tsmi.Tag;
                                    if ( ct.equals( m_selected_curve ) ) {
                                        tsmi.Checked = true;
                                        break;
                                    }
                                }
                                foreach ( ToolStripItem tsi2 in tsmi.DropDownItems ) {
                                    if ( tsi2 is ToolStripMenuItem ) {
                                        ToolStripMenuItem tsmi2 = (ToolStripMenuItem)tsi2;
                                        if ( tsmi2.Tag is CurveType ) {
                                            CurveType ct2 = (CurveType)tsmi2.Tag;
                                            if ( ct2.equals( m_selected_curve ) ) {
                                                tsmi2.Checked = true;
                                                if ( ct2.equals( CurveType.reso1amp ) || ct2.equals( CurveType.reso1bw ) || ct2.equals( CurveType.reso1freq ) ||
                                                     ct2.equals( CurveType.reso2amp ) || ct2.equals( CurveType.reso2bw ) || ct2.equals( CurveType.reso2freq ) ||
                                                     ct2.equals( CurveType.reso3amp ) || ct2.equals( CurveType.reso3bw ) || ct2.equals( CurveType.reso3freq ) ||
                                                     ct2.equals( CurveType.reso4amp ) || ct2.equals( CurveType.reso4bw ) || ct2.equals( CurveType.reso4freq ) ){
                                                    tsmi.Checked = true;//親アイテムもチェック。Resonance*用
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        cmenuCurve.Show( this, e.Location );
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
                if ( target.points.get( i ).ID == point_id ) {
                    index = i;
                    break;
                }
            }
            float scale_x = AppManager.scaleX;
            float scale_y = ScaleY;
            BezierPoint ret = new BezierPoint( 0, 0 );
            if ( index >= 0 ) {
                BezierPoint item = target.points.get( index );
                if ( picked == BezierPickedSide.BASE ) {
                    Point old = target.points.get( index ).getBase().ToPoint();
                    item.setBase( new PointD( clock, value ) );
                    if ( !BezierChain.isBezierImplicit( target ) ) {
                        item.setBase( new PointD( old.X, old.Y ) );
                    }
                    ret = (BezierPoint)target.points.get( index ).Clone();
                } else if ( picked == BezierPickedSide.LEFT ) {
                    if ( item.getControlLeftType() != BezierControlType.Master ) {
                        Point old1 = item.getControlLeft().ToPoint();
                        Point old2 = item.getControlRight().ToPoint();
                        Point b = item.getBase().ToPoint();
                        item.setControlLeft( new PointD( clock, value_raw ) );
                        double dx = (b.X - item.getControlLeft().X) * scale_x;
                        double dy = (b.Y - item.getControlLeft().Y) * scale_y;
                        BezierPoint original = AppManager.getLastSelectedBezier().original;
                        double theta = Math.Atan2( dy, dx );
                        double dx2 = (original.getControlRight().X - b.X) * scale_x;
                        double dy2 = (original.getControlRight().Y - b.Y) * scale_y;
                        double l = Math.Sqrt( dx2 * dx2 + dy2 * dy2 );
                        dx2 = l * Math.Cos( theta ) / scale_x;
                        dy2 = l * Math.Sin( theta ) / scale_y;
                        item.setControlRight( new PointD( b.X + (int)dx2, b.Y + (int)dy2 ) );
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            item.setControlLeft( new PointD( old1.X, old1.Y ) );
                            item.setControlRight( new PointD( old2.X, old2.Y ) );
                        }
                    } else {
                        Point old = item.getControlLeft().ToPoint();
                        item.setControlLeft( new PointD( clock, value_raw ) );
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            item.setControlLeft( new PointD( old.X, old.Y ) );
                        }
                    }
                    ret = (BezierPoint)item.Clone();
                } else if ( picked == BezierPickedSide.RIGHT ) {
                    if ( item.getControlRightType() != BezierControlType.Master ) {
                        Point old1 = item.getControlLeft().ToPoint();
                        Point old2 = item.getControlRight().ToPoint();
                        Point b = item.getBase().ToPoint();
                        item.setControlRight( new PointD( clock, value_raw ) );
                        double dx = (item.getControlRight().X - b.X) * scale_x;
                        double dy = (item.getControlRight().Y - b.Y) * scale_y;
                        BezierPoint original = AppManager.getLastSelectedBezier().original;
                        double theta = Math.Atan2( dy, dx );
                        double dx2 = (b.X - original.getControlLeft().X) * scale_x;
                        double dy2 = (b.Y - original.getControlLeft().Y) * scale_y;
                        double l = Math.Sqrt( dx2 * dx2 + dy2 * dy2 );
                        dx2 = l * Math.Cos( theta ) / scale_x;
                        dy2 = l * Math.Sin( theta ) / scale_y;
                        item.setControlLeft( new PointD( b.X - (int)dx2, b.Y - (int)dy2 ) );
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            item.setControlLeft( new PointD( old1.X, old1.Y ) );
                            item.setControlRight( new PointD( old2.X, old2.Y ) );
                        }
                    } else {
                        Point old = item.getControlRight().ToPoint();
                        item.setControlRight( new PointD( clock, value ) );
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            item.setControlRight( new PointD( old.X, old.Y ) );
                        }
                    }
                    ret = (BezierPoint)item.Clone();
                }
            }
            return ret;
        }

        public BezierPoint HandleMouseMoveForBezierMove( MouseEventArgs e, BezierPickedSide picked ) {
            int clock = clockFromXCoord( e.X );
            int value = valueFromYCoord( e.Y );
            int value_raw = value;

            if ( clock < AppManager.getVsqFile().getPreMeasure() ) {
                clock = AppManager.getVsqFile().getPreMeasure();
            }
            int max = m_selected_curve.Maximum;
            int min = m_selected_curve.Minimum;
            if ( value < min ) {
                value = min;
            } else if ( max < value ) {
                value = max;
            }
            return HandleMouseMoveForBezierMove( clock, value, value_raw, picked );
        }

        private void TrackSelector_MouseMove( object sender, MouseEventArgs e ) {
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

            if ( e.Button == MouseButtons.None ) {
                return;
            }
            if ( m_mouse_hover_thread != null ) {
                if ( m_mouse_hover_thread.IsAlive ) {
                    m_mouse_hover_thread.Abort();
                }
            }
            if ( AppManager.isPlaying() ) {
                return;
            }
            m_mouse_moved = true;
            int clock = clockFromXCoord( e.X );

            if ( clock < AppManager.getVsqFile().getPreMeasure() ) {
                clock = AppManager.getVsqFile().getPreMeasure();
            }

            if ( e.Button == MouseButtons.Left &&
                 0 <= e.Y && e.Y <= Height - 2 * OFFSET_TRACK_TAB &&
                 m_mouse_down_mode == MouseDownMode.CURVE_EDIT ) {
                switch ( AppManager.getSelectedTool() ) {
                    case EditTool.PENCIL:
                        m_pencil_moved = true;
                        if ( m_mouse_trace != null ) {
                            Vector<Integer> removelist = new Vector<Integer>();
                            int x = e.X + AppManager.startToDrawX;
                            if ( x < m_mouse_trace_last_x ) {
                                foreach ( int key in m_mouse_trace.Keys ) {
                                    if ( x <= key && key < m_mouse_trace_last_x ) {
                                        removelist.add( key );
                                    }
                                }
                            } else if ( m_mouse_trace_last_x < x ) {
                                foreach ( int key in m_mouse_trace.Keys ) {
                                    if ( m_mouse_trace_last_x < key && key <= x ) {
                                        removelist.add( key );
                                    }
                                }
                            }
                            for ( int i = 0; i < removelist.size(); i++ ) {
                                m_mouse_trace.Remove( removelist.get( i ) );
                            }
                            if ( x == m_mouse_trace_last_x ) {
                                m_mouse_trace[x] = e.Y;
                                m_mouse_trace_last_y = e.Y;
                            } else {
                                float a = (e.Y - m_mouse_trace_last_y) / (float)(x - m_mouse_trace_last_x);
                                float b = m_mouse_trace_last_y - a * m_mouse_trace_last_x;
                                int start = Math.Min( x, m_mouse_trace_last_x );
                                int end = Math.Max( x, m_mouse_trace_last_x );
                                for ( int xx = start; xx <= end; xx++ ) {
                                    int yy = (int)(a * xx + b);
                                    if ( m_mouse_trace.ContainsKey( xx ) ) {
                                        m_mouse_trace[xx] = yy;
                                    } else {
                                        m_mouse_trace.Add( xx, yy );
                                    }
                                }
                                m_mouse_trace_last_x = x;
                                m_mouse_trace_last_y = e.Y;
                            }
                        }
                        break;
                    case EditTool.ARROW:
                    case EditTool.ERASER:
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
                        m_selecting_region.Width = draft_clock - m_selecting_region.X;
                        m_selecting_region.Height = value - m_selecting_region.Y;
                        break;
                }
            } else if ( m_mouse_down_mode == MouseDownMode.SINGER_LIST ) {
                int dclock = clock - m_singer_move_started_clock;
                for( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                    SelectedEventEntry item = (SelectedEventEntry)itr.next();
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
                for ( Iterator itr = m_veledit_selected.keySet().iterator(); itr.hasNext(); ){
                    int id = (Integer)itr.next();
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
                    if ( target.points.get( i ).ID == point_id ) {
                        index = i;
                        break;
                    }
                }
                if ( index >= 0 ) {
                    BezierPoint item = target.points.get( index );
                    Point old_right = item.getControlRight().ToPoint();
                    Point old_left = item.getControlLeft().ToPoint();
                    BezierControlType old_right_type = item.getControlRightType();
                    BezierControlType old_left_type = item.getControlLeftType();
                    int cl = clock;
                    int va = value_raw;
                    int dx = (int)item.getBase().X - cl;
                    int dy = (int)item.getBase().Y - va;
                    if ( item.getBase().X + dx >= 0 ) {
                        item.setControlRight( new PointD( clock, value_raw ) );
                        item.setControlLeft( new PointD( clock + 2 * dx, value_raw + 2 * dy ) );
                        item.setControlRightType( BezierControlType.Normal );
                        item.setControlLeftType( BezierControlType.Normal );
                        if ( !BezierChain.isBezierImplicit( target ) ) {
                            item.setControlLeft( new PointD( old_left.X, old_left.Y ) );
                            item.setControlRight( new PointD( old_right.X, old_right.Y ) );
                            item.setControlLeftType( old_left_type );
                            item.setControlRightType( old_right_type );
                        }
                    }
                }
            } else if ( m_mouse_down_mode == MouseDownMode.ENVELOPE_MOVE ) {
                double sec = AppManager.getVsqFile().getSecFromClock( clockFromXCoord( e.X ) );
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
                int clock_at_downed = clockFromXCoord( m_mouse_down_location.X - AppManager.startToDrawX );
                VsqFileEx vsq = AppManager.getVsqFile();
                double dsec = vsq.getSecFromClock( clock ) - vsq.getSecFromClock( clock_at_downed );
                int draft_preutterance = m_pre_ovl_original.UstEvent.PreUtterance - (int)(dsec * 1000);
                m_pre_ovl_editing.UstEvent.PreUtterance = draft_preutterance;
            } else if ( m_mouse_down_mode == MouseDownMode.OVERLAP_MOVE ) {
                int clock_at_downed = clockFromXCoord( m_mouse_down_location.X - AppManager.startToDrawX );
                VsqFileEx vsq = AppManager.getVsqFile();
                double dsec = vsq.getSecFromClock( clock ) - vsq.getSecFromClock( clock_at_downed );
                int draft_overlap = m_pre_ovl_original.UstEvent.VoiceOverlap + (int)(dsec * 1000);
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
        private void findBezierPointAt( Point location,
                                        Vector<BezierChain> list,
                                        out BezierChain found_chain,
                                        out BezierPoint found_point,
                                        out BezierPickedSide found_side,
                                        int dot_width,
                                        int px_tolerance ) {
            found_chain = null;
            found_point = null;
            found_side = BezierPickedSide.BASE;
            int shift = dot_width + px_tolerance;
            int width = (dot_width + px_tolerance) * 2;
            int c = list.size();
            for ( int i = 0; i < c; i++ ) {
                BezierChain bc = list.get( i );
                for ( Iterator itr = bc.points.iterator(); itr.hasNext(); ){
                    BezierPoint bp = (BezierPoint)itr.next();
                    Point p = getScreenCoord( bp.getBase() );
                    Rectangle r = new Rectangle( p.X - shift, p.Y - shift, width, width );
                    if ( isInRect( location, r ) ) {
                        found_chain = bc;
                        found_point = bp;
                        found_side = BezierPickedSide.BASE;
                        return;
                    }

                    if ( bp.getControlLeftType() != BezierControlType.None ) {
                        p = getScreenCoord( bp.getControlLeft() );
                        r = new Rectangle( p.X - shift, p.Y - shift, width, width );
                        if ( isInRect( location, r ) ) {
                            found_chain = bc;
                            found_point = bp;
                            found_side = BezierPickedSide.LEFT;
                            return;
                        }
                    }

                    if ( bp.getControlRightType() != BezierControlType.None ) {
                        p = getScreenCoord( bp.getControlRight() );
                        r = new Rectangle( p.X - shift, p.Y - shift, width, width );
                        if ( isInRect( location, r ) ) {
                            found_chain = bc;
                            found_point = bp;
                            found_side = BezierPickedSide.RIGHT;
                            return;
                        }
                    }
                }
            }
        }

        private void processMouseDownSelectRegion( MouseEventArgs e ) {
            if ( (Control.ModifierKeys & Keys.Control) != Keys.Control ) {
                AppManager.clearSelectedPoint();
            }

            int clock = clockFromXCoord( e.X );
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
                m_selecting_region = new Rectangle( quantized_clock, value, 0, 0 );
            } else {
                m_selecting_region = new Rectangle( clock, value, 0, 0 );
            }
        }

        public void TrackSelector_MouseDown( object sender, MouseEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "TrackSelector_MouseDown" );
#endif
            VsqFileEx vsq = AppManager.getVsqFile();
            m_mouse_down_location = new Point( e.X + AppManager.startToDrawX, e.Y );
            int clock = clockFromXCoord( e.X );
            m_mouse_moved = false;
            m_mouse_downed = true;
            if ( AppManager.KEY_LENGTH < e.X && clock < vsq.getPreMeasure() ) {
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }
            m_modifier_on_mouse_down = Control.ModifierKeys;
            int max = m_selected_curve.getMaximum();
            int min = m_selected_curve.getMinimum();
            int value = valueFromYCoord( e.Y );
            if ( value < min ) {
                value = min;
            } else if ( max < value ) {
                value = max;
            }

            if ( Height - OFFSET_TRACK_TAB <= e.Y && e.Y < Height ) {
                if ( e.Button == MouseButtons.Left ) {
                    #region MouseDown occured on track list
                    m_mouse_down_mode = MouseDownMode.TRACK_LIST;
                    //m_selected_region_enabled = false;
                    m_mouse_trace = null;
                    int selecter_width = SelectorWidth;
                    if ( AppManager.getVsqFile() != null ) {
                        for ( int i = 0; i < 16; i++ ) {
                            int x = AppManager.KEY_LENGTH + i * selecter_width;
                            if ( AppManager.getVsqFile().Track.size() > i + 1 ) {
                                if ( x <= e.X && e.X < x + selecter_width ) {
                                    int new_selected = i + 1;
                                    if ( AppManager.getSelected() != new_selected ) {
                                        AppManager.setSelected( i + 1 );
                                        if ( SelectedTrackChanged != null ) {
                                            SelectedTrackChanged( i + 1 );
                                        }
                                        Invalidate();
                                        return;
                                    } else if ( x + selecter_width - _PX_WIDTH_RENDER <= e.X && e.X < e.X + selecter_width ) {
                                        if ( AppManager.getRenderRequired( AppManager.getSelected() ) && !AppManager.isPlaying() ) {
                                            if ( RenderRequired != null ) {
                                                RenderRequired( new int[] { AppManager.getSelected() } );
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            } else if ( Height - 2 * OFFSET_TRACK_TAB <= e.Y && e.Y < Height - OFFSET_TRACK_TAB ) {
                #region MouseDown occured on singer tab
                m_mouse_down_mode = MouseDownMode.SINGER_LIST;
                AppManager.clearSelectedPoint();
                m_mouse_trace = null;
                int x;
                VsqEvent ve = findItemAt( e.Location, out x );
                if ( AppManager.getSelectedTool() == EditTool.ERASER ) {
                    #region EditTool.Eraser
                    if ( ve != null && ve.Clock > 0 ) {
                        CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventDelete( AppManager.getSelected(), ve.InternalID ) );
                        executeCommand( run, true );
                    }
                    #endregion
                } else {
                    if ( ve != null ) {
                        if ( (Control.ModifierKeys & m_modifier_key) == m_modifier_key ) {
                            if ( AppManager.isSelectedEventContains( AppManager.getSelected(), ve.InternalID ) ) {
                                Vector<Integer> old = new Vector<Integer>();
                                for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                                    SelectedEventEntry item = (SelectedEventEntry)itr.next();
                                    int id = item.original.InternalID;
                                    if ( id != ve.InternalID ) {
                                        old.add( id );
                                    }
                                }
                                AppManager.clearSelectedEvent();
                                AppManager.addSelectedEventRange( old.toArray( new Integer[]{} ) );
                            } else {
                                AppManager.addSelectedEvent( ve.InternalID );
                            }
                        } else if ( (Control.ModifierKeys & Keys.Shift) == Keys.Shift ) {
                            int last_clock = AppManager.getLastSelectedEvent().original.Clock;
                            int tmin = Math.Min( ve.Clock, last_clock );
                            int tmax = Math.Max( ve.Clock, last_clock );
                            Vector<Integer> add_required = new Vector<Integer>();
                            for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getEventIterator(); itr.hasNext(); ) {
                                VsqEvent item = (VsqEvent)itr.next();
                                if ( item.ID.type == VsqIDType.Singer && tmin <= item.Clock && item.Clock <= tmax ) {
                                    add_required.add( item.InternalID );
                                    //AppManager.AddSelectedEvent( item.InternalID );
                                }
                            }
                            add_required.add( ve.InternalID );
                            AppManager.addSelectedEventRange( add_required.toArray( new Integer[]{} ) );
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
                int left_clock = clockFromXCoord( AppManager.KEY_LENGTH );
                int right_clock = clockFromXCoord( this.Width - vScroll.Width );
                for ( Iterator itr = vsq.Track.get( AppManager.getSelected() ).getEventIterator(); itr.hasNext(); ) {
                    VsqEvent ve = (VsqEvent)itr.next();
                    if ( ve.ID.type == VsqIDType.Anote ) {
                        int start = ve.Clock;
                        if ( right_clock < start ) {
                            break;
                        }
                        int end = ve.Clock + ve.ID.Length;
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
                if ( AppManager.KEY_LENGTH <= e.X ) {
                    if ( e.Button == MouseButtons.Left && !m_spacekey_downed ) {
                        //m_selected_region_enabled = false;
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
                                        goto last;
                                    }
                                    if ( processMouseDownPreutteranceAndOverlap( e ) ) {
                                        goto last;
                                    }
                                } else if ( !m_selected_curve.equals( CurveType.VEL ) &&
                                            !m_selected_curve.equals( CurveType.Accent ) &&
                                            !m_selected_curve.equals( CurveType.Decay ) &&
                                            !m_selected_curve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownBezier( e ) ) {
                                        goto last;
                                    }
                                }
                            } else {
                                if ( m_selected_curve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownEnvelope( e ) ) {
                                        goto last;
                                    }
                                    if ( processMouseDownPreutteranceAndOverlap( e ) ) {
                                        goto last;
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
                                        goto last;
                                    }
                                    if ( processMouseDownPreutteranceAndOverlap( e ) ) {
                                        goto last;
                                    }
                                } else if ( !m_selected_curve.equals( CurveType.VEL ) &&
                                            !m_selected_curve.equals( CurveType.Accent ) &&
                                            !m_selected_curve.equals( CurveType.Decay ) &&
                                            !m_selected_curve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownBezier( e ) ) {
                                        goto last;
                                    }
                                } else {
                                    m_mouse_down_mode = MouseDownMode.NONE;
                                }
                                #endregion
                            } else {
                                #region NOT CurveMode
                                if ( m_selected_curve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownEnvelope( e ) ) {
                                        goto last;
                                    }
                                    if ( processMouseDownPreutteranceAndOverlap( e ) ) {
                                        goto last;
                                    }
                                }
                                m_mouse_trace = null;
                                m_mouse_trace = new SortedList<int, int>();
                                int x = e.X + AppManager.startToDrawX;
                                m_mouse_trace.Add( x, e.Y );
                                m_mouse_trace_last_x = x;
                                m_mouse_trace_last_y = e.Y;
                                m_pencil_moved = false;

                                m_mouse_hover_thread = new Thread( new ThreadStart( MouseHoverEventGenerator ) );
                                m_mouse_hover_thread.Start();
                                #endregion
                            }
                            #endregion
                        } else if ( AppManager.getSelectedTool() == EditTool.ARROW ) {
                            #region Arrow
                            boolean found = false;
                            if ( m_selected_curve.IsScalar || m_selected_curve.IsAttachNote ) {
                                if ( m_selected_curve.equals( CurveType.Env ) ) {
                                    if ( processMouseDownEnvelope( e ) ) {
                                        goto last;
                                    }
                                    if ( processMouseDownPreutteranceAndOverlap( e ) ) {
                                        goto last;
                                    }
                                }
                                m_mouse_down_mode = MouseDownMode.NONE;
                            } else {
                                // まずベジエ曲線の点にヒットしてないかどうかを検査
                                Vector<BezierChain> dict = AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).get( m_selected_curve );
                                AppManager.clearSelectedBezier();
                                for ( int i = 0; i < dict.size(); i++ ) {
                                    BezierChain bc = dict.get( i );
                                    for ( Iterator itr = bc.points.iterator(); itr.hasNext(); ) {
                                        BezierPoint bp = (BezierPoint)itr.next();
                                        Point pt = getScreenCoord( bp.getBase() );
                                        Rectangle rc = new Rectangle( pt.X - px_shift, pt.Y - px_shift, px_width, px_width );
                                        if ( isInRect( e.Location, rc ) ) {
                                            AppManager.addSelectedBezier( new SelectedBezierPoint( bc.id, bp.ID, BezierPickedSide.BASE, bp ) );
                                            m_editing_bezier_original = (BezierChain)bc.Clone();
                                            found = true;
                                            break;
                                        }

                                        if ( bp.getControlLeftType() != BezierControlType.None ) {
                                            pt = getScreenCoord( bp.getControlLeft() );
                                            rc = new Rectangle( pt.X - px_shift, pt.Y - px_shift, px_width, px_width );
                                            if ( isInRect( e.Location, rc ) ) {
                                                AppManager.addSelectedBezier( new SelectedBezierPoint( bc.id, bp.ID, BezierPickedSide.LEFT, bp ) );
                                                m_editing_bezier_original = (BezierChain)bc.Clone();
                                                found = true;
                                                break;
                                            }
                                        }

                                        if ( bp.getControlRightType() != BezierControlType.None ) {
                                            pt = getScreenCoord( bp.getControlRight() );
                                            rc = new Rectangle( pt.X - px_shift, pt.Y - px_shift, px_width, px_width );
                                            if ( isInRect( e.Location, rc ) ) {
                                                AppManager.addSelectedBezier( new SelectedBezierPoint( bc.id, bp.ID, BezierPickedSide.RIGHT, bp ) );
                                                m_editing_bezier_original = (BezierChain)bc.Clone();
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
                                VsqEvent ve = findItemAt( e.Location );
                                // マウス位置の音符アイテムを検索
                                if ( ve != null ) {
                                    boolean found2 = false;
                                    if ( (Control.ModifierKeys & m_modifier_key) == m_modifier_key ) {
                                        // clicked with CTRL key
                                        Vector<Integer> list = new Vector<Integer>();
                                        for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                                            SelectedEventEntry item = (SelectedEventEntry)itr.next();
                                            VsqEvent ve2 = item.original;
                                            if ( ve.InternalID == ve2.InternalID ) {
                                                found2 = true;
                                            } else {
                                                list.add( ve2.InternalID );
                                            }
                                        }
                                        AppManager.clearSelectedEvent();
                                        AppManager.addSelectedEventRange( list.toArray( new Integer[] { } ) );
                                    } else if ( (Control.ModifierKeys & Keys.Shift) == Keys.Shift ) {
                                        // clicked with Shift key
                                        SelectedEventEntry last_selected = AppManager.getLastSelectedEvent();
                                        if ( last_selected != null ) {
                                            int last_clock = last_selected.original.Clock;
                                            int tmin = Math.Min( ve.Clock, last_clock );
                                            int tmax = Math.Max( ve.Clock, last_clock );
                                            Vector<Integer> add_required = new Vector<Integer>();
                                            for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                                                VsqEvent item = (VsqEvent)itr.next();
                                                if ( tmin <= item.Clock && item.Clock <= tmax ) {
                                                    add_required.add( item.InternalID );
                                                }
                                            }
                                            AppManager.addSelectedEventRange( add_required.toArray( new Integer[] { } ) );
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
                                        for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                                            SelectedEventEntry item = (SelectedEventEntry)itr.next();
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
                                    m_mouse_hover_thread = new Thread( new ThreadStart( MouseHoverEventGenerator ) );
                                    m_mouse_hover_thread.Start();
                                    goto last;
                                }

                                // マウス位置のデータポイントを検索
                                long id = findDataPointAt( e.Location );
                                if ( id > 0 ) {
                                    if ( AppManager.isSelectedPointContains( id ) ) {
                                        if ( (m_modifier_on_mouse_down & m_modifier_key) == m_modifier_key ) {
                                            AppManager.removeSelectedPoint( id );
                                            m_mouse_down_mode = MouseDownMode.NONE;
                                            goto last;
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
                                            /*if ( id == item.id ) {
                                                int x = xCoordFromClocks( list.getKeyClock( i ) );
                                                int y = yCoordFromValue( item.value );
                                                m_moving_points_shift = new Point( x - e.X, y - e.Y );
#if DEBUG
                                                Console.WriteLine( "FormMain#TrackSelector_MouseDown; m_moving_points_shift=" + m_moving_points_shift );
#endif
                                            }*/
                                        }
                                        goto last;
                                    }
                                } else {
                                    if ( (m_modifier_on_mouse_down & Keys.Control) != Keys.Control ) {
                                        AppManager.clearSelectedPoint();
                                    }
                                    if ( (m_modifier_on_mouse_down & Keys.Shift) != Keys.Shift && (m_modifier_on_mouse_down & m_modifier_key) != m_modifier_key ) {
                                        AppManager.clearSelectedPoint();
                                    }
                                }

                                if ( (m_modifier_on_mouse_down & m_modifier_key) != m_modifier_key ) {
                                    m_selected_region_enabled = false;
                                }
                                if ( AppManager.editorConfig.CurveSelectingQuantized ) {
                                    m_selecting_region = new Rectangle( quantized_clock, value, 0, 0 );
                                } else {
                                    m_selecting_region = new Rectangle( clock, value, 0, 0 );
                                }
                                #endregion
                            }
                            #endregion
                        } else if ( AppManager.getSelectedTool() == EditTool.ERASER ) {
                            #region Eraser
                            VsqEvent ve3 = findItemAt( e.Location );
                            if ( ve3 != null ) {
                                AppManager.clearSelectedEvent();
                                CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventDelete( AppManager.getSelected(),
                                                                                                                  ve3.InternalID ) );
                                executeCommand( run, true );
                            } else {
                                if ( AppManager.isCurveMode() ) {
                                    Vector<BezierChain> list = vsq.AttachedCurves.get( AppManager.getSelected() - 1 ).get( m_selected_curve );
                                    if ( list != null ){
                                        BezierChain chain;
                                        BezierPoint point;
                                        BezierPickedSide side;
                                        findBezierPointAt( e.Location, list, out chain, out point, out side, DOT_WID, AppManager.editorConfig.PxToleranceBezier );
                                        if ( point != null ) {
                                            if ( side == BezierPickedSide.BASE ) {
                                                // データ点自体を削除
                                                BezierChain work = (BezierChain)chain.clone();
                                                int count = work.points.size();
                                                if ( count > 1 ) {
                                                    // 2個以上のデータ点があるので、BezierChainを置換
                                                    for ( int i = 0; i < count; i++ ) {
                                                        BezierPoint bp = work.points.get( i );
                                                        if ( bp.ID == point.ID ) {
                                                            work.points.removeElementAt( i );
                                                            break;
                                                        }
                                                    }
                                                    CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( AppManager.getSelected(),
                                                                                                                       m_selected_curve,
                                                                                                                       chain.id,
                                                                                                                       work,
                                                                                                                       AppManager.editorConfig.ControlCurveResolution.Value );
                                                    executeCommand( run, true );
                                                    m_mouse_down_mode = MouseDownMode.NONE;
                                                    goto last;
                                                } else {
                                                    // 1個しかデータ点がないので、BezierChainを削除
                                                    CadenciiCommand run = VsqFileEx.generateCommandDeleteBezierChain( AppManager.getSelected(),
                                                                                                                      m_selected_curve,
                                                                                                                      chain.id,
                                                                                                                      AppManager.editorConfig.ControlCurveResolution.Value );
                                                    executeCommand( run, true );
                                                    m_mouse_down_mode = MouseDownMode.NONE;
                                                    goto last;
                                                }
                                            } else {
                                                // 滑らかにするオプションを解除する
                                                BezierChain work = (BezierChain)chain.clone();
                                                int count = work.points.size();
                                                for ( int i = 0; i < count; i++ ) {
                                                    BezierPoint bp = work.points.get( i );
                                                    if ( bp.ID == point.ID ) {
                                                        bp.setControlLeftType( BezierControlType.None );
                                                        bp.setControlRightType( BezierControlType.None );
                                                        break;
                                                    }
                                                }
                                                CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( AppManager.getSelected(),
                                                                                                                   m_selected_curve,
                                                                                                                   chain.id,
                                                                                                                   work,
                                                                                                                   AppManager.editorConfig.ControlCurveResolution.Value );
                                                executeCommand( run, true );
                                                m_mouse_down_mode = MouseDownMode.NONE;
                                                goto last;
                                            }
                                        }
                                    }
                                } else {
                                    long id = findDataPointAt( e.Location );
                                    if ( id > 0 ) {
                                        VsqBPList item = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( m_selected_curve.getName() );
                                        if ( item != null ) {
                                            VsqBPList work = (VsqBPList)item.clone();
                                            VsqBPPairSearchContext context = work.findElement( id );
                                            if ( context.point.id == id ) {
                                                work.remove( context.clock );
                                                CadenciiCommand run = new CadenciiCommand(
                                                    VsqCommand.generateCommandTrackCurveReplace( AppManager.getSelected(),
                                                                                                 m_selected_curve.getName(),
                                                                                                 work ) );
                                                executeCommand( run, true );
                                                m_mouse_down_mode = MouseDownMode.NONE;
                                                goto last;
                                            }
                                        }
                                    }
                                }

                                if ( (m_modifier_on_mouse_down & Keys.Shift) != Keys.Shift && (m_modifier_on_mouse_down & m_modifier_key) != m_modifier_key ) {
                                    AppManager.clearSelectedPoint();
                                }
                                if ( AppManager.editorConfig.CurveSelectingQuantized ) {
                                    m_selecting_region = new Rectangle( quantized_clock, value, 0, 0 );
                                } else {
                                    m_selecting_region = new Rectangle( clock, value, 0, 0 );
                                }
                            }
                            #endregion
                        }
                    } else if ( e.Button == MouseButtons.Right ) {
                        if ( AppManager.isCurveMode() ) {
                            if ( !m_selected_curve.equals( CurveType.VEL ) && !m_selected_curve.equals( CurveType.Env ) ) {
                                Vector<BezierChain> dict = AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).get( m_selected_curve );
                                AppManager.clearSelectedBezier();
                                boolean found = false;
                                for ( int i = 0; i < dict.size(); i++ ) {
                                    BezierChain bc = dict.get( i );
                                    for ( Iterator itr = bc.points.iterator(); itr.hasNext(); ){
                                        BezierPoint bp = (BezierPoint)itr.next();
                                        Point pt = getScreenCoord( bp.getBase() );
                                        Rectangle rc = new Rectangle( pt.X - DOT_WID, pt.Y - DOT_WID, 2 * DOT_WID + 1, 2 * DOT_WID + 1 );
                                        if ( isInRect( e.Location, rc ) ) {
                                            AppManager.addSelectedBezier( new SelectedBezierPoint( bc.id, bp.ID, BezierPickedSide.BASE, bp ) );
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
                    m_selected_region_enabled = false;
                }
                #endregion
            }
        last: ;
            Invalidate();
        }


        private boolean processMouseDownBezier( MouseEventArgs e ){
            //, int clock, int value, int px_shift, int px_width, Keys modifier ) {
            int clock = clockFromXCoord( e.X );
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
            Keys modifier = Control.ModifierKeys;

            int track = AppManager.getSelected();
            boolean too_near = false; // clicked position is too near to existing bezier points
            boolean is_middle = false;

            // check whether bezier point exists on clicked position
            Vector<BezierChain> dict = AppManager.getVsqFile().AttachedCurves.get( track - 1 ).get( m_selected_curve );
            BezierChain found_chain;
            BezierPoint found_point;
            BezierPickedSide found_side;
            findBezierPointAt( e.Location, dict, out found_chain, out found_point, out found_side, DOT_WID, AppManager.editorConfig.PxToleranceBezier );

            if ( found_chain != null ) {
                AppManager.addSelectedBezier( new SelectedBezierPoint( found_chain.id, found_point.ID, found_side, found_point ) );
                m_editing_bezier_original = (BezierChain)found_chain.Clone();
                m_mouse_down_mode = MouseDownMode.BEZIER_MODE;
            } else {
                if ( AppManager.getSelectedTool() != EditTool.PENCIL ) {
                    return false;
                }
                BezierChain target_chain = null;
                for ( int j = 0; j < dict.size(); j++ ) {
                    BezierChain bc = dict.get( j );
                    for ( int i = 1; i < bc.size(); i++ ) {
                        if ( !is_middle && bc.points.get( i - 1 ).getBase().X <= clock && clock <= bc.points.get( i ).getBase().X ) {
                            target_chain = (BezierChain)bc.Clone();
                            is_middle = true;
                        }
                        if ( !too_near ) {
                            for ( Iterator itr = bc.points.iterator(); itr.hasNext(); ){
                                BezierPoint bp = (BezierPoint)itr.next();
                                Point pt = getScreenCoord( bp.getBase() );
                                Rectangle rc = new Rectangle( pt.X - px_shift, pt.Y - px_shift, px_width, px_width );
                                if ( isInRect( e.Location, rc ) ) {
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
                            int last = (int)bc.points.get( bc.points.size() - 1 ).getBase().X;
                            if ( tmax < last && last < clock ) {
                                tmax = last;
                                target_chain = (BezierChain)bc.Clone();
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
                        bp.ID = point_id;
                        adding.add( bp );
                        chain_id = AppManager.getVsqFile().AttachedCurves.get( track - 1 ).getNextId( m_selected_curve );
#if DEBUG
                        AppManager.debugWriteLine( "    new chain_id=" + chain_id );
#endif
                        CadenciiCommand run = VsqFileEx.generateCommandAddBezierChain( track,
                                                                                m_selected_curve,
                                                                                chain_id,
                                                                                AppManager.editorConfig.ControlCurveResolution.Value,
                                                                                adding );
                        executeCommand( run, false );
                        m_mouse_down_mode = MouseDownMode.BEZIER_ADD_NEW;
                    } else {
                        m_editing_bezier_original = (BezierChain)target_chain.Clone();
                        bp = new BezierPoint( pt, pt, pt );
                        point_id = target_chain.getNextId();
                        bp.ID = point_id;
                        target_chain.add( bp );
                        Collections.sort( target_chain.points );
                        chain_id = target_chain.id;
                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( track,
                                                                                    m_selected_curve,
                                                                                    target_chain.id,
                                                                                    target_chain,
                                                                                    AppManager.editorConfig.ControlCurveResolution.Value );
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

        private boolean processMouseDownPreutteranceAndOverlap( MouseEventArgs e ) {
            if ( m_preutterance_viewing >= 0 && isInRect( e.Location, m_preutterance_bounds ) ) {
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
            if ( m_overlap_viewing >= 0 && isInRect( e.Location, m_overlap_bounds ) ) {
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

        private boolean processMouseDownEnvelope( MouseEventArgs e ) {
            int internal_id = -1;
            int point_kind = -1;
            if ( !findEnvelopePointAt( e.Location, out internal_id, out point_kind ) ) {
#if DEBUG
                Console.WriteLine( "processTrackSelectorMouseDownForEnvelope; not found" );
#endif
                return false;
            }
            m_envelope_original = null;
            VsqTrack target = AppManager.getVsqFile().Track.get( AppManager.getSelected() );
            VsqEvent item = target.findEventFromID( internal_id );
            if ( item == null ) {
                return false;
            }
            if ( item != null && item.UstEvent != null && item.UstEvent.Envelope != null ) {
                m_envelope_original = (UstEnvelope)item.UstEvent.Envelope.Clone();
                m_envelope_editing = item.UstEvent.Envelope;
            }
            if ( m_envelope_original == null ) {
                item.UstEvent.Envelope = new UstEnvelope();
                m_envelope_editing = item.UstEvent.Envelope;
                m_envelope_original = (UstEnvelope)item.UstEvent.Envelope.Clone();
            }
            m_mouse_down_mode = MouseDownMode.ENVELOPE_MOVE;
            m_envelope_move_id = internal_id;
            m_envelope_point_kind = point_kind;

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
            double sec = AppManager.getVsqFile().getSecFromClock( clockFromXCoord( e.X ) );
            m_envelope_range_begin = AppManager.getVsqFile().getSecFromClock( item.Clock ) - item.UstEvent.PreUtterance / 1000.0;
            m_envelope_range_end = AppManager.getVsqFile().getSecFromClock( item.Clock + item.ID.Length );
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
                if ( SelectedCurveChanged != null ) {
                    SelectedCurveChanged( curve );
                }
            }
        }

        private static boolean isInRect( Point pt, Rectangle rc ) {
            if ( rc.X <= pt.X && pt.X <= rc.X + rc.Width ) {
                if ( rc.Y <= pt.Y && pt.Y <= rc.Y + rc.Height ) {
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
        private VsqEvent findItemAt( Point location, out int position_x ) {
            position_x = 0;
            if ( AppManager.getVsqFile() == null ) {
                return null;
            }
            VsqTrack target = AppManager.getVsqFile().Track.get( AppManager.getSelected() );
            int count = target.getEventCount();
            for ( int i = 0; i < count; i++ ) {
                VsqEvent ve = target.getEvent( i );
                if ( ve.ID.type == VsqIDType.Singer ) {
                    int x = xCoordFromClocks( ve.Clock );
                    if ( Height - 2 * OFFSET_TRACK_TAB <= location.Y &&
                         location.Y <= Height - OFFSET_TRACK_TAB &&
                         x <= location.X && location.X <= x + SINGER_ITEM_WIDTH ) {
                        position_x = x;
                        return ve;
                    } else if ( Width < x ) {
                        //return null;
                    }
                } else if ( ve.ID.type == VsqIDType.Anote ) {
                    int x = xCoordFromClocks( ve.Clock );
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
                    if ( 0 <= location.Y && location.Y <= Height - 2 * OFFSET_TRACK_TAB &&
                         AppManager.KEY_LENGTH <= location.X && location.X <= Width ) {
                        if ( y <= location.Y && location.Y <= Height - FOOTER && x <= location.X && location.X <= x + VEL_BAR_WIDTH ) {
                            position_x = x;
                            return ve;
                        }
                    }
                }
            }
            return null;
        }
        
        /// <summary>
        /// クリックされた位置にある歌手変更イベントを取得します
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private VsqEvent findItemAt( Point location ) {
            int x;
            return findItemAt( location, out x );
        }

        public void TrackSelector_MouseUp( object sender, MouseEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "TrackSelector_MouseUp" );
#endif
            m_mouse_downed = false;
            if ( m_mouse_hover_thread != null ) {
                if ( m_mouse_hover_thread.IsAlive ) {
                    m_mouse_hover_thread.Abort();
                }
            }
            if ( CurveVisible ) {
                int max = m_selected_curve.getMaximum();
                int min = m_selected_curve.getMinimum();
                VsqFileEx vsq = AppManager.getVsqFile();
#if DEBUG
                AppManager.debugWriteLine( "    max,min=" + max + "," + min );
#endif
                if ( m_mouse_down_mode == MouseDownMode.BEZIER_ADD_NEW ||
                     m_mouse_down_mode == MouseDownMode.BEZIER_MODE ||
                     m_mouse_down_mode == MouseDownMode.BEZIER_EDIT ) {
                    if ( e.Button == MouseButtons.Left ) {
                        if ( sender is TrackSelector ) {
                            int chain_id = AppManager.getLastSelectedBezier().chainID;
                            BezierChain edited = (BezierChain)AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).getBezierChain( m_selected_curve, chain_id ).Clone();
                            if ( m_mouse_down_mode == MouseDownMode.BEZIER_ADD_NEW ) {
                                edited.id = chain_id;
                                CadenciiCommand pre = VsqFileEx.generateCommandDeleteBezierChain( AppManager.getSelected(),
                                                                                                  m_selected_curve,
                                                                                                  chain_id,
                                                                                                  AppManager.editorConfig.ControlCurveResolution.Value );
                                executeCommand( pre, false );
                                CadenciiCommand run = VsqFileEx.generateCommandAddBezierChain( AppManager.getSelected(),
                                                                                               m_selected_curve,
                                                                                               chain_id,
                                                                                               AppManager.editorConfig.ControlCurveResolution.Value,
                                                                                               edited );
                                executeCommand( run, true );
                            } else if ( m_mouse_down_mode == MouseDownMode.BEZIER_EDIT ) {
                                CadenciiCommand pre = VsqFileEx.generateCommandReplaceBezierChain( AppManager.getSelected(),
                                                                                            m_selected_curve,
                                                                                            chain_id,
                                                                                            m_editing_bezier_original,
                                                                                            AppManager.editorConfig.ControlCurveResolution.Value );
                                executeCommand( pre, false );
                                CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( AppManager.getSelected(),
                                                                                            m_selected_curve,
                                                                                            chain_id,
                                                                                            edited,
                                                                                            AppManager.editorConfig.ControlCurveResolution.Value );
                                executeCommand( run, true );
                            } else if ( m_mouse_down_mode == MouseDownMode.BEZIER_MODE ) {
                                AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).setBezierChain( m_selected_curve, chain_id, m_editing_bezier_original );
                                CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( AppManager.getSelected(),
                                                                                            m_selected_curve,
                                                                                            chain_id,
                                                                                            edited,
                                                                                            AppManager.editorConfig.ControlCurveResolution.Value );
                                executeCommand( run, true );
#if DEBUG
                                AppManager.debugWriteLine( "    m_mouse_down_mode=" + m_mouse_down_mode );
                                AppManager.debugWriteLine( "    chain_id=" + chain_id );
#endif

                            }
                        }
                    }
                } else if ( m_mouse_down_mode == MouseDownMode.CURVE_EDIT ||
                            m_mouse_down_mode == MouseDownMode.VEL_WAIT_HOVER ) {
                    if ( e.Button == MouseButtons.Left ) {
                        if ( AppManager.getSelectedTool() == EditTool.ARROW ) {
                            #region Arrow
                            if ( m_selected_curve.equals( CurveType.Env ) ) {

                            } else if ( !m_selected_curve.equals( CurveType.VEL ) && !m_selected_curve.equals( CurveType.Accent ) && !m_selected_curve.equals( CurveType.Decay ) ) {
                                if ( m_selecting_region.Width == 0 ) {
                                    m_selected_region_enabled = false;
                                } else {
                                    if ( !m_selected_region_enabled ) {
                                        int start = Math.Min( m_selecting_region.X, m_selecting_region.X + m_selecting_region.Width );
                                        int end = Math.Max( m_selecting_region.X, m_selecting_region.X + m_selecting_region.Width );
                                        m_selected_region = new Range( start, end );
#if DEBUG
                                        AppManager.debugWriteLine( "    selected_region ENABLED" );
#endif
                                        m_selected_region_enabled = true;
                                    } else {
                                        int start = Math.Min( m_selecting_region.X, m_selecting_region.X + m_selecting_region.Width );
                                        int end = Math.Max( m_selecting_region.X, m_selecting_region.X + m_selecting_region.Width );
                                        int old_start = m_selected_region.getStart();
                                        int old_end = m_selected_region.getEnd();
                                        m_selected_region = new Range( Math.Min( start, old_start ), Math.Max( end, old_end ) );
                                    }

                                    if ( (m_modifier_on_mouse_down & Keys.Control) != Keys.Control ) {
                                        AppManager.clearSelectedPoint();
                                    }
                                    if ( !m_selected_curve.equals( CurveType.Accent ) &&
                                         !m_selected_curve.equals( CurveType.Decay ) &&
                                         !m_selected_curve.equals( CurveType.Env ) &&
                                         !m_selected_curve.equals( CurveType.VEL ) &&
                                         !m_selected_curve.equals( CurveType.VibratoDepth ) &&
                                         !m_selected_curve.equals( CurveType.VibratoRate ) ) {
                                        VsqBPList list = AppManager.getVsqFile().Track[AppManager.getSelected()].getCurve( m_selected_curve.Name );
                                        int count = list.size();
                                        Rectangle rc = new Rectangle( Math.Min( m_selecting_region.X, m_selecting_region.X + m_selecting_region.Width ),
                                                                      Math.Min( m_selecting_region.Y, m_selecting_region.Y + m_selecting_region.Height ),
                                                                      Math.Abs( m_selecting_region.Width ),
                                                                      Math.Abs( m_selecting_region.Height ) );
                                        for ( int i = 0; i < count; i++ ) {
                                            int clock = list.getKeyClock( i );
                                            VsqBPPair item = list.getElementB( i );
                                            if ( isInRect( new Point( clock, item.value ), rc ) ) {
                                                AppManager.addSelectedPoint( m_selected_curve, item.id );
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        } else if ( AppManager.getSelectedTool() == EditTool.ERASER ){
                            #region Eraser
                            if ( AppManager.isCurveMode() ) {
                                Vector<BezierChain> list = vsq.AttachedCurves.get( AppManager.getSelected() - 1 ).get( m_selected_curve );
                                if ( list != null ) {
                                    int x = Math.Min( m_selecting_region.X, m_selecting_region.X + m_selecting_region.Width );
                                    int y = Math.Min( m_selecting_region.Y, m_selecting_region.Y + m_selecting_region.Height );
                                    Rectangle rc = new Rectangle( x, y, Math.Abs( m_selecting_region.Width ), Math.Abs( m_selecting_region.Height ) );

                                    boolean changed = false; //1箇所でも削除が実行されたらtrue

                                    int count = list.size();
                                    Vector<BezierChain> work = new Vector<BezierChain>();
                                    for ( int i = 0; i < count; i++ ) {
                                        BezierChain chain = list.get( i );
                                        BezierChain chain_copy = new BezierChain();
                                        chain_copy.Color = chain.Color;
                                        chain_copy.Default = chain.Default;
                                        chain_copy.id = chain.id;
                                        int point_count = chain.points.size();
                                        for ( int j = 0; j < point_count; j++ ) {
                                            BezierPoint point = chain.points.get( j );
                                            if ( isInRect( point.getBase().ToPoint(), rc ) ) {
                                                // データ点が選択範囲に入っているので、追加しない
                                                changed = true;
                                                continue;
                                            } else {
                                                if ( (point.getControlLeftType() != BezierControlType.None && isInRect( point.getControlLeft().ToPoint(), rc )) ||
                                                     (point.getControlRightType() != BezierControlType.None && isInRect( point.getControlRight().ToPoint(), rc )) ) {
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
                                        CadenciiCommand run = VsqFileEx.generateCommandReplaceAttachedCurveRange( AppManager.getSelected(), comm );
                                        executeCommand( run, true );
                                    }
                                }
                            } else {
                                if ( m_selected_curve.equals( CurveType.VEL ) || m_selected_curve.equals( CurveType.Accent ) || m_selected_curve.equals( CurveType.Decay ) ) {
                                    #region VEL Accent Delay
                                    int start = Math.Min( m_selecting_region.X, m_selecting_region.X + m_selecting_region.Width );
                                    int end = Math.Max( m_selecting_region.X, m_selecting_region.X + m_selecting_region.Width );
                                    int old_start = m_selected_region.getStart();
                                    int old_end = m_selected_region.getEnd();
                                    m_selected_region.setStart( Math.Min( start, old_start ) );
                                    m_selected_region.setEnd( Math.Max( end, old_end ) );
                                    AppManager.clearSelectedEvent();
                                    Vector<Integer> deleting = new Vector<Integer>();
                                    for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                                        VsqEvent ev = (VsqEvent)itr.next();
                                        if ( start <= ev.Clock && ev.Clock <= end ) {
                                            deleting.add( ev.InternalID );
                                        }
                                    }
                                    if ( deleting.size() > 0 ) {
                                        CadenciiCommand er_run = new CadenciiCommand(
                                            VsqCommand.generateCommandEventDeleteRange( AppManager.getSelected(), deleting.toArray( new Integer[] { } ) ) );
                                        executeCommand( er_run, true );
                                    }
                                    #endregion
                                } else if ( m_selected_curve.equals( CurveType.VibratoRate ) || m_selected_curve.equals( CurveType.VibratoDepth ) ) {
                                    #region VibratoRate ViratoDepth
                                    int er_start = Math.Min( m_selecting_region.X, m_selecting_region.X + m_selecting_region.Width );
                                    int er_end = Math.Max( m_selecting_region.X, m_selecting_region.X + m_selecting_region.Width );
                                    Vector<Integer> internal_ids = new Vector<Integer>();
                                    Vector<VsqID> items = new Vector<VsqID>();
                                    for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                                        VsqEvent ve = (VsqEvent)itr.next();
                                        if ( ve.ID.VibratoHandle == null ) {
                                            continue;
                                        }
                                        int cl_vib_start = ve.Clock + ve.ID.VibratoDelay;
                                        int cl_vib_length = ve.ID.Length - ve.ID.VibratoDelay;
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
                                                target = item.VibratoHandle.DepthBP;
                                            } else {
                                                target = item.VibratoHandle.RateBP;
                                            }
                                            Vector<float> bpx = new Vector<float>();
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
                                                item.VibratoHandle.DepthBP = new VibratoBPList( bpx.toArray( new float[] { } ), bpy.toArray( new int[] { } ) );
                                            } else {
                                                item.VibratoHandle.RateBP = new VibratoBPList( bpx.toArray( new float[] { } ), bpy.toArray( new int[] { } ) );
                                            }
                                            internal_ids.add( ve.InternalID );
                                            items.add( item );
                                        }
                                    }
                                    CadenciiCommand run = new CadenciiCommand(
                                        VsqCommand.generateCommandEventChangeIDContaintsRange( AppManager.getSelected(), internal_ids.toArray( new Integer[] { } ), items.toArray( new VsqID[] { } ) ) );
                                    executeCommand( run, true );
                                    #endregion
                                } else if ( m_selected_curve.equals( CurveType.Env ) ) {

                                } else {
                                    #region Other Curves
                                    Vector<Integer> remove_clock_list = new Vector<Integer>();
                                    VsqBPList work = (VsqBPList)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( m_selected_curve.getName() ).clone();

                                    // 削除するべきデータ点のリストを作成
                                    int x = Math.Min( m_selecting_region.X, m_selecting_region.X + m_selecting_region.Width );
                                    int y = Math.Min( m_selecting_region.Y, m_selecting_region.Y + m_selecting_region.Height );
                                    Rectangle rc = new Rectangle( x, y, Math.Abs( m_selecting_region.Width ), Math.Abs( m_selecting_region.Height ) );
                                    int count = work.size();
                                    for ( int i = 0; i < count; i++ ) {
                                        int clock = work.getKeyClock( i );
                                        int value = work.getElementA( i );
                                        if ( isInRect( new Point( clock, value ), rc ) ) {
                                            remove_clock_list.add( clock );
                                        }
                                    }

                                    if ( remove_clock_list.size() > 0 ) {
                                        // 削除を実行
                                        int c = remove_clock_list.size();
                                        for ( int i = 0; i < c; i++ ) {
                                            work.remove( remove_clock_list.get( i ) );
                                        }

                                        CadenciiCommand run_eraser = new CadenciiCommand(
                                            VsqCommand.generateCommandTrackCurveReplace( AppManager.getSelected(),
                                                                                         m_selected_curve.getName(),
                                                                                         work ) );
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
#endif
                            if ( m_pencil_moved ) {
                                int stdx = AppManager.startToDrawX;
                                if ( m_selected_curve.equals( CurveType.VEL ) || m_selected_curve.equals( CurveType.Accent ) || m_selected_curve.equals( CurveType.Decay ) ) {
                                    #region VEL Accent Decay
                                    int start = m_mouse_trace.Keys[0];
                                    int end = m_mouse_trace.Keys[m_mouse_trace.Count - 1];
                                    start = clockFromXCoord( start - stdx );
                                    end = clockFromXCoord( end - stdx );
#if DEBUG
                                    AppManager.debugWriteLine( "        start=" + start );
                                    AppManager.debugWriteLine( "        end=" + end );
#endif
                                    TreeMap<Integer, int> velocity = new TreeMap<Integer, int>();
                                    for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                                        VsqEvent ve = (VsqEvent)itr.next();
                                        if ( start <= ve.Clock && ve.Clock < end ) {
                                            for ( int i = 0; i < m_mouse_trace.Keys.Count - 1; i++ ) {
                                                int key0 = m_mouse_trace.Keys[i];
                                                int key1 = m_mouse_trace.Keys[i + 1];
                                                int key0_clock = clockFromXCoord( key0 - stdx );
                                                int key1_clock = clockFromXCoord( key1 - stdx );
#if DEBUG
                                                AppManager.debugWriteLine( "        key0,key1=" + key0 + "," + key1 );
#endif
                                                if ( key0_clock < ve.Clock && ve.Clock < key1_clock ) {
                                                    int key0_value = valueFromYCoord( m_mouse_trace[key0] );
                                                    int key1_value = valueFromYCoord( m_mouse_trace[key1] );
                                                    float a = (key1_value - key0_value) / (float)(key1_clock - key0_clock);
                                                    float b = key0_value - a * key0_clock;
                                                    int new_value = (int)(a * ve.Clock + b);
                                                    velocity.put( ve.InternalID, new_value );
                                                } else if ( key0_clock == ve.Clock ) {
                                                    velocity.put( ve.InternalID, valueFromYCoord( m_mouse_trace[key0] ) );
                                                } else if ( key1_clock == ve.Clock ) {
                                                    velocity.put( ve.InternalID, valueFromYCoord( m_mouse_trace[key1] ) );
                                                }
                                            }
                                        }
                                    }
                                    if ( velocity.size() > 0 ) {
                                        Vector<KeyValuePair<Integer, Integer>> cpy = new Vector<KeyValuePair<Integer, Integer>>();
                                        for ( Iterator itr = velocity.keySet().iterator(); itr.hasNext(); ) {
                                            int key = (Integer)itr.next();
                                            int value = (Integer)velocity.get( key );
                                            cpy.add( new KeyValuePair<int, int>( key, value ) );
                                        }
                                        CadenciiCommand run = null;
                                        if ( m_selected_curve.equals( CurveType.VEL ) ) {
                                            run = new CadenciiCommand( VsqCommand.generateCommandEventChangeVelocity( AppManager.getSelected(), cpy ) );
                                        } else if ( m_selected_curve.equals( CurveType.Accent ) ) {
                                            run = new CadenciiCommand( VsqCommand.generateCommandEventChangeAccent( AppManager.getSelected(), cpy ) );
                                        } else if ( m_selected_curve.equals( CurveType.Decay ) ) {
                                            run = new CadenciiCommand( VsqCommand.generateCommandEventChangeDecay( AppManager.getSelected(), cpy ) );
                                        }
                                        executeCommand( run, true );
                                    }
                                    #endregion
                                } else if ( m_selected_curve.equals( CurveType.VibratoRate ) || m_selected_curve.equals( CurveType.VibratoDepth ) ) {
                                    #region VibratoRate || VibratoDepth
                                    int step_clock = AppManager.editorConfig.ControlCurveResolution.Value;
                                    int step_px = (int)(step_clock * AppManager.scaleX);
                                    if ( step_px <= 0 ) {
                                        step_px = 1;
                                    }
                                    int start = m_mouse_trace.Keys[0];
                                    int end = m_mouse_trace.Keys[m_mouse_trace.Count - 1];
#if DEBUG
                                    AppManager.debugWriteLine( "    start,end=" + start + " " + end );
#endif
                                    Vector<Integer> internal_ids = new Vector<Integer>();
                                    Vector<VsqID> items = new Vector<VsqID>();
                                    for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                                        VsqEvent ve = (VsqEvent)itr.next();
                                        int cl_vib_start = ve.Clock + ve.ID.VibratoDelay;
                                        float cl_vib_length = ve.ID.Length - ve.ID.VibratoDelay;
                                        int vib_start = xCoordFromClocks( cl_vib_start ) + stdx;          // 仮想スクリーン上の、ビブラートの描画開始位置
                                        int vib_end = xCoordFromClocks( ve.Clock + ve.ID.Length ) + stdx; // 仮想スクリーン上の、ビブラートの描画終了位置
                                        int chk_start = Math.Max( vib_start, start );       // マウスのトレースと、ビブラートの描画範囲がオーバーラップしている部分を検出
                                        int chk_end = Math.Min( vib_end, end );
                                        float add_min = (clockFromXCoord( chk_start - stdx ) - cl_vib_start) / cl_vib_length;
                                        float add_max = (clockFromXCoord( chk_end - stdx ) - cl_vib_start) / cl_vib_length;
#if DEBUG
                                        AppManager.debugWriteLine( "    vib_start,vib_end=" + vib_start + " " + vib_end );
                                        AppManager.debugWriteLine( "    chk_start,chk_end=" + chk_start + " " + chk_end );
                                        AppManager.debugWriteLine( "    add_min,add_max=" + add_min + " " + add_max );
#endif
                                        if ( chk_start < chk_end ) {    // オーバーラップしている。
                                            Vector<ValuePair<float, int>> edit = new Vector<ValuePair<float, int>>();
                                            for ( int i = chk_start; i < chk_end; i += step_px ) {
                                                if ( m_mouse_trace.ContainsKey( i ) ) {
                                                    edit.add( new ValuePair<float, int>( (clockFromXCoord( i - stdx ) - cl_vib_start) / cl_vib_length,
                                                                                            valueFromYCoord( m_mouse_trace[i] ) ) );
                                                } else {
                                                    for ( int j = 0; j < m_mouse_trace.Keys.Count - 1; j++ ) {
                                                        if ( m_mouse_trace.Keys[j] <= i && i < m_mouse_trace.Keys[j + 1] ) {
                                                            int key0 = m_mouse_trace.Keys[j];
                                                            int key1 = m_mouse_trace.Keys[j + 1];
                                                            float a = (m_mouse_trace[key1] - m_mouse_trace[key0]) / (float)(key1 - key0);
                                                            int newy = (int)(m_mouse_trace[key0] + a * (i - key0));
                                                            int clock = clockFromXCoord( i - stdx );
                                                            int value = valueFromYCoord( newy );
                                                            if ( value < min ) {
                                                                value = min;
                                                            } else if ( max < value ) {
                                                                value = max;
                                                            }
                                                            edit.add( new ValuePair<float, int>( (clock - cl_vib_start) / cl_vib_length, value ) );
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            int c = clockFromXCoord( end - stdx );
                                            float lastx = (c - cl_vib_start) / cl_vib_length;
                                            if ( 0 <= lastx && lastx <= 1 ) {
                                                int v = valueFromYCoord( m_mouse_trace[end] );
                                                if ( v < min ) {
                                                    v = min;
                                                } else if ( max < v ) {
                                                    v = max;
                                                }
                                                edit.add( new ValuePair<float, int>( lastx, v ) );
                                            }
                                            if ( ve.ID.VibratoHandle != null ) {
                                                VibratoBPList target = null;
                                                if ( m_selected_curve.equals( CurveType.VibratoRate ) ) {
                                                    target = ve.ID.VibratoHandle.RateBP;
                                                } else {
                                                    target = ve.ID.VibratoHandle.DepthBP;
                                                }
                                                if ( target.getCount() > 0 ) {
                                                    for ( int i = 0; i < target.getCount(); i++ ) {
                                                        if ( target.getElement( i ).X < add_min || add_max < target.getElement( i ).X ) {
                                                            edit.add( new ValuePair<float, int>( target.getElement( i ).X,
                                                                                                     target.getElement( i ).Y ) );
                                                        }
                                                    }
                                                }
                                                Collections.sort( edit );
                                                VsqID id = (VsqID)ve.ID.clone();
                                                float[] bpx = new float[edit.size()];
                                                int[] bpy = new int[edit.size()];
                                                for ( int i = 0; i < edit.size(); i++ ) {
                                                    bpx[i] = edit.get( i ).Key;
                                                    bpy[i] = edit.get( i ).Value;
                                                }
                                                if ( m_selected_curve.equals( CurveType.VibratoDepth ) ) {
                                                    id.VibratoHandle.DepthBP = new VibratoBPList( bpx, bpy );
                                                } else {
                                                    id.VibratoHandle.RateBP = new VibratoBPList( bpx, bpy );
                                                }
                                                internal_ids.add( ve.InternalID );
                                                items.add( id );
                                            }
                                        }
                                    }
                                    if ( internal_ids.size() > 0 ) {
                                        CadenciiCommand run = new CadenciiCommand(
                                            VsqCommand.generateCommandEventChangeIDContaintsRange( AppManager.getSelected(),
                                                                                                   internal_ids.toArray( new Integer[] { } ),
                                                                                                   items.toArray( new VsqID[] { } ) ) );
                                        executeCommand( run, true );
                                    }
                                    #endregion
                                } else if ( m_selected_curve.equals( CurveType.Env ) ) {
                                    #region Env

                                    #endregion
                                } else {
                                    #region Other Curves
                                    Vector<BPPair> edit = new Vector<BPPair>();
                                    int step_clock = AppManager.editorConfig.ControlCurveResolution.Value;
                                    int step_px = (int)(step_clock * AppManager.scaleX);
                                    if ( step_px <= 0 ) {
                                        step_px = 1;
                                    }
                                    int start = m_mouse_trace.Keys[0];
                                    int end = m_mouse_trace.Keys[m_mouse_trace.Count - 1];
                                    int last = start;
#if DEBUG
                                    AppManager.debugWriteLine( "    start=" + start );
                                    AppManager.debugWriteLine( "    end=" + end );
#endif
                                    int last_value = int.MinValue;
                                    for ( int i = start; i <= end; i += step_px ) {
                                        if ( m_mouse_trace.ContainsKey( i ) ) {
                                            int clock = clockFromXCoord( i - stdx );
                                            int value = valueFromYCoord( m_mouse_trace[i] );
                                            if ( value < min ) {
                                                value = min;
                                            } else if ( max < value ) {
                                                value = max;
                                            }
                                            if ( value != last_value ) {
                                                edit.add( new BPPair( clock, value ) );
                                                last_value = value;
                                            }
                                        } else {
                                            for ( int j = 0; j < m_mouse_trace.Keys.Count - 1; j++ ) {
                                                if ( m_mouse_trace.Keys[j] <= i && i < m_mouse_trace.Keys[j + 1] ) {
                                                    int key0 = m_mouse_trace.Keys[j];
                                                    int key1 = m_mouse_trace.Keys[j + 1];
                                                    float a = (m_mouse_trace[key1] - m_mouse_trace[key0]) / (float)(key1 - key0);
                                                    int newy = (int)(m_mouse_trace[key0] + a * (i - key0));
                                                    int clock = clockFromXCoord( i - stdx );
                                                    int value = valueFromYCoord( newy, max, min );
                                                    if ( value < min ) {
                                                        value = min;
                                                    } else if ( max < value ) {
                                                        value = max;
                                                    }
                                                    if ( value != last_value ) {
                                                        edit.add( new BPPair( clock, value ) );
                                                        last_value = value;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if ( (end - start) % step_px != 0 ) {
                                        int clock = clockFromXCoord( end - stdx );
                                        int value = valueFromYCoord( m_mouse_trace[end] );
                                        if ( value < min ) {
                                            value = min;
                                        } else if ( max < value ) {
                                            value = max;
                                        }
                                        edit.add( new BPPair( clock, value ) );
                                    }
                                    int end_clock = clockFromXCoord( end - stdx );
                                    int last_v = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( m_selected_curve.getName() ).getValue( end_clock );
                                    edit.get( edit.size() - 1 ).Value = last_v;
                                    CadenciiCommand pen_run = new CadenciiCommand( VsqCommand.generateCommandTrackCurveEdit( AppManager.getSelected(), m_selected_curve.getName(), edit ) );
                                    executeCommand( pen_run, true );
                                    #endregion
                                }
                            }
                            m_mouse_trace = null;
                            #endregion
                        } else if ( !AppManager.isCurveMode() && AppManager.getSelectedTool() == EditTool.LINE ) {
                            #region Line
                            int x0 = m_line_start.X;
                            int y0 = m_line_start.Y;
                            int x1 = clockFromXCoord( e.X );
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
                            float a0 = (y1 - y0) / (float)(x1 - x0);
                            float b0 = y0 - a0 * x0;
                            if ( m_selected_curve.equals( CurveType.VEL ) || m_selected_curve.equals( CurveType.Accent ) || m_selected_curve.equals( CurveType.Decay ) ) {
                                Vector<KeyValuePair<Integer, Integer>> velocity = new Vector<KeyValuePair<Integer, Integer>>();
                                for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ve = (VsqEvent)itr.next();
                                    if ( x0 <= ve.Clock && ve.Clock < x1 ) {
                                        int new_value = (int)(a0 * ve.Clock + b0);
                                        velocity.add( new KeyValuePair<Integer, Integer>( ve.InternalID, new_value ) );
                                    }
                                }
                                CadenciiCommand run = null;
                                if ( velocity.size() > 0 ) {
                                    if ( m_selected_curve.equals( CurveType.VEL ) ) {
                                        run = new CadenciiCommand( VsqCommand.generateCommandEventChangeVelocity( AppManager.getSelected(), velocity ) );
                                    } else if ( m_selected_curve.equals( CurveType.Accent ) ) {
                                        run = new CadenciiCommand( VsqCommand.generateCommandEventChangeAccent( AppManager.getSelected(), velocity ) );
                                    } else if ( m_selected_curve.equals( CurveType.Decay ) ) {
                                        run = new CadenciiCommand( VsqCommand.generateCommandEventChangeDecay( AppManager.getSelected(), velocity ) );
                                    }
                                }
                                if ( run != null ) {
                                    executeCommand( run, true );
                                }
                            } else if ( m_selected_curve.equals( CurveType.VibratoDepth ) || m_selected_curve.equals( CurveType.VibratoRate ) ) {
                                int stdx = AppManager.startToDrawX;
                                int step_clock = AppManager.editorConfig.ControlCurveResolution.Value;
                                int cl_start = x0;
                                int cl_end = x1;
#if DEBUG
                                AppManager.debugWriteLine( "    cl_start,cl_end=" + cl_start + " " + cl_end );
#endif
                                Vector<Integer> internal_ids = new Vector<Integer>();
                                Vector<VsqID> items = new Vector<VsqID>();
                                for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                                    VsqEvent ve = (VsqEvent)itr.next();
                                    int cl_vib_start = ve.Clock + ve.ID.VibratoDelay;
                                    int cl_vib_length = ve.ID.Length - ve.ID.VibratoDelay;
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
                                        Vector<ValuePair<float, int>> edit = new Vector<ValuePair<float, int>>();
                                        for ( int i = chk_start; i < chk_end; i += step_clock ) {
                                            float x = (i - cl_vib_start) / (float)cl_vib_length;
                                            if ( 0 <= x && x <= 1 ) {
                                                int y = (int)(a0 * i + b0);
                                                edit.add( new ValuePair<float, int>( x, y ) );
                                            }
                                        }
                                        edit.add( new ValuePair<float, int>( (chk_end - cl_vib_start) / (float)cl_vib_length, (int)(a0 * chk_end + b0) ) );
                                        if ( ve.ID.VibratoHandle != null ) {
                                            VibratoBPList target = null;
                                            if ( m_selected_curve.equals( CurveType.VibratoRate ) ) {
                                                target = ve.ID.VibratoHandle.RateBP;
                                            } else {
                                                target = ve.ID.VibratoHandle.DepthBP;
                                            }
                                            if ( target.getCount() > 0 ) {
                                                for ( int i = 0; i < target.getCount(); i++ ) {
                                                    if ( target.getElement( i ).X < add_min || add_max < target.getElement( i ).X ) {
                                                        edit.add( new ValuePair<float, int>( target.getElement( i ).X,
                                                                                                 target.getElement( i ).Y ) );
                                                    }
                                                }
                                            }
                                            Collections.sort( edit );
                                            VsqID id = (VsqID)ve.ID.clone();
                                            float[] bpx = new float[edit.size()];
                                            int[] bpy = new int[edit.size()];
                                            for ( int i = 0; i < edit.size(); i++ ) {
                                                bpx[i] = edit.get( i ).Key;
                                                bpy[i] = edit.get( i ).Value;
                                            }
                                            if ( m_selected_curve.equals( CurveType.VibratoDepth ) ) {
                                                id.VibratoHandle.DepthBP = new VibratoBPList( bpx, bpy );
                                            } else {
                                                id.VibratoHandle.RateBP = new VibratoBPList( bpx, bpy );
                                            }
                                            internal_ids.add( ve.InternalID );
                                            items.add( id );
                                        }
                                    }
                                }
                                if ( internal_ids.size() > 0 ) {
                                    CadenciiCommand run = new CadenciiCommand(
                                        VsqCommand.generateCommandEventChangeIDContaintsRange( AppManager.getSelected(),
                                                                                        internal_ids.toArray( new Integer[] { } ),
                                                                                        items.toArray( new VsqID[] { } ) ) );
                                    executeCommand( run, true );
                                }
                            } else if ( m_selected_curve.equals( CurveType.Env ) ) {
                                // todo:
                            } else {
                                Vector<BPPair> edit = new Vector<BPPair>();
                                int step_clock = AppManager.editorConfig.ControlCurveResolution.Value;
                                for ( int i = x0; i <= x1; i += step_clock ) {
                                    int y = (int)(a0 * i + b0);
                                    edit.add( new BPPair( i, y ) );
                                }
                                int lasty = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCurve( m_selected_curve.Name ).getValue( x1 );
                                if ( x1 == edit.get( edit.size() - 1 ).Clock ) {
                                    edit.get( edit.size() - 1 ).Value = lasty;
                                } else {
                                    edit.add( new BPPair( x1, lasty ) );
                                }
                                CadenciiCommand pen_run = new CadenciiCommand( VsqCommand.generateCommandTrackCurveEdit( AppManager.getSelected(), m_selected_curve.Name, edit ) );
                                executeCommand( pen_run, true );
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
                            int premeasure = AppManager.getVsqFile().getPreMeasureClocks();
                            for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                                SelectedEventEntry item = (SelectedEventEntry)itr.next();
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
                                System.Media.SystemSounds.Asterisk.Play();
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
                                    System.Media.SystemSounds.Asterisk.Play();
                                }
                                boolean changed = false;
                                for ( int j = 0; j < ids.Length; j++ ) {
                                    for ( Iterator itr = AppManager.getSelectedEventIterator(); itr.hasNext(); ){
                                        SelectedEventEntry item = (SelectedEventEntry)itr.next();
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
                                        VsqCommand.generateCommandEventChangeClockAndIDContaintsRange( AppManager.getSelected(), ids, clocks, values ) );
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
                    for ( Iterator itr = m_veledit_selected.keySet().iterator(); itr.hasNext(); ) {
                        int id = (Integer)itr.next();
                        i++;
                        ids[i] = id;
                        values[i] = (VsqID)m_veledit_selected.get( id ).editing.ID.clone();
                    }
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventChangeIDContaintsRange( AppManager.getSelected(), ids, values ) );
                    executeCommand( run, true );
                    if ( m_veledit_selected.size() == 1 ) {
                        AppManager.clearSelectedEvent();
                        AppManager.addSelectedEvent( m_veledit_last_selectedid );
                    }
                } else if ( m_mouse_down_mode == MouseDownMode.ENVELOPE_MOVE ) {
                    m_mouse_down_mode = MouseDownMode.NONE;
                    if ( m_mouse_moved ) {
                        VsqTrack target = AppManager.getVsqFile().Track.get( AppManager.getSelected() );
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
                        CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventReplace( AppManager.getSelected(),
                                                                                                           edited ) );
                        executeCommand( run, true );
                    }
                } else if ( m_mouse_down_mode == MouseDownMode.PRE_UTTERANCE_MOVE ) {
                    m_mouse_down_mode = MouseDownMode.NONE;
                    if ( m_mouse_moved ) {
                        VsqTrack target = AppManager.getVsqFile().Track.get( AppManager.getSelected() );
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
                        CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventReplace( AppManager.getSelected(),
                                                                                                           edited ) );
                        executeCommand( run, true );
                    }
                } else if ( m_mouse_down_mode == MouseDownMode.OVERLAP_MOVE ) {
                    m_mouse_down_mode = MouseDownMode.NONE;
                    if ( m_mouse_moved ) {
                        VsqTrack target = AppManager.getVsqFile().Track.get( AppManager.getSelected() );
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
                        CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventReplace( AppManager.getSelected(),
                                                                                                           edited ) );
                        executeCommand( run, true );
                    }
                } else if ( m_mouse_down_mode == MouseDownMode.POINT_MOVE ) {
                    if ( m_mouse_moved ) {
                        Point mouse = this.PointToClient( Control.MousePosition );
                        int dx = mouse.X + AppManager.startToDrawX - m_mouse_down_location.X;
                        int dy = mouse.Y - m_mouse_down_location.Y;

                        String curve = m_selected_curve.getName();
                        VsqTrack work = (VsqTrack)AppManager.getVsqFile().Track.get( AppManager.getSelected() ).clone();
                        VsqBPList list = work.getCurve( curve );
                        VsqBPList work_list = (VsqBPList)list.clone();
                        int min0 = list.getMinimum();
                        int max0 = list.getMaximum();
                        int count = list.size();
                        for ( int i = 0; i < count; i++ ) {
                            int clock = list.getKeyClock( i );
                            VsqBPPair item = list.getElementB( i );
                            if ( AppManager.isSelectedPointContains( item.id ) ) {
                                int x = xCoordFromClocks( clock ) + dx + 1;
                                int y = yCoordFromValue( item.value ) + dy - 1;

                                int nclock = clockFromXCoord( x );
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
                        BezierCurves beziers = AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 );
                        CadenciiCommand run = VsqFileEx.generateCommandTrackReplace( AppManager.getSelected(), work, beziers );
                        executeCommand( run, true );
                    }
                    m_moving_points.clear();
                }
            }
            m_mouse_down_mode = MouseDownMode.NONE;
            this.Invalidate();
        }

        private void TrackSelector_MouseHover( object sender, EventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "TrackSelector_MouseHover" );
            AppManager.debugWriteLine( "    m_mouse_downed=" + m_mouse_downed );
#endif
            if ( m_selected_curve.equals( CurveType.Accent ) || m_selected_curve.equals( CurveType.Decay ) || m_selected_curve.equals( CurveType.Env ) ) {
                return;
            }
            if ( m_mouse_downed && !m_pencil_moved && AppManager.getSelectedTool() == EditTool.PENCIL && 
                 !m_selected_curve.equals( CurveType.VEL ) ) {
                Point mouse = this.PointToClient( Control.MousePosition );
                int clock = clockFromXCoord( mouse.X );
                int value = valueFromYCoord( mouse.Y );
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
                    for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getNoteEventIterator(); itr.hasNext(); ) {
                        VsqEvent ve = (VsqEvent)itr.next();
                        if ( ve.ID.VibratoHandle == null ){
                            continue;
                        }
                        int cl_vib_start = ve.Clock + ve.ID.VibratoDelay;
                        int cl_vib_end = ve.Clock + ve.ID.Length;
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
                                edited.VibratoHandle.StartRate = value;
                            } else {
                                if ( edited.VibratoHandle.RateBP.getCount() <= 0 ) {
                                    edited.VibratoHandle.RateBP = new VibratoBPList( new float[] { x },
                                                                                     new int[] { value } );
                                } else {
                                    Vector<float> xs = new Vector<float>();
                                    Vector<Integer> vals = new Vector<Integer>();
                                    boolean first = true;
                                    for ( int i = 0; i < edited.VibratoHandle.RateBP.getCount(); i++ ) {
                                        if ( edited.VibratoHandle.RateBP.getElement( i ).X < x ) {
                                            xs.add( edited.VibratoHandle.RateBP.getElement( i ).X );
                                            vals.add( edited.VibratoHandle.RateBP.getElement( i ).Y );
                                        } else if ( edited.VibratoHandle.RateBP.getElement( i ).X == x ) {
                                            xs.add( x );
                                            vals.add( value );
                                            first = false;
                                        } else {
                                            if ( first ) {
                                                xs.add( x );
                                                vals.add( value );
                                                first = false;
                                            }
                                            xs.add( edited.VibratoHandle.RateBP.getElement( i ).X );
                                            vals.add( edited.VibratoHandle.RateBP.getElement( i ).Y );
                                        }
                                    }
                                    if ( first ) {
                                        xs.add( x );
                                        vals.add( value );
                                    }
                                    edited.VibratoHandle.RateBP = new VibratoBPList( xs.toArray( new float[]{} ), vals.toArray( new int[]{} ) );
                                }
                            }
                        } else {
                            if ( x == 0f ) {
                                edited.VibratoHandle.StartDepth = value;
                            } else {
                                if ( edited.VibratoHandle.DepthBP.getCount() <= 0 ) {
                                    edited.VibratoHandle.DepthBP = new VibratoBPList( new float[] { x },
                                                                                      new int[] { value } );
                                } else {
                                    Vector<float> xs = new Vector<float>();
                                    Vector<Integer> vals = new Vector<Integer>();
                                    boolean first = true;
                                    for ( int i = 0; i < edited.VibratoHandle.DepthBP.getCount(); i++ ) {
                                        if ( edited.VibratoHandle.DepthBP.getElement( i ).X < x ) {
                                            xs.add( edited.VibratoHandle.DepthBP.getElement( i ).X );
                                            vals.add( edited.VibratoHandle.DepthBP.getElement( i ).Y );
                                        } else if ( edited.VibratoHandle.DepthBP.getElement( i ).X == x ) {
                                            xs.add( x );
                                            vals.add( value );
                                            first = false;
                                        } else {
                                            if ( first ) {
                                                xs.add( x );
                                                vals.add( value );
                                                first = false;
                                            }
                                            xs.add( edited.VibratoHandle.DepthBP.getElement( i ).X );
                                            vals.add( edited.VibratoHandle.DepthBP.getElement( i ).Y );
                                        }
                                    }
                                    if ( first ) {
                                        xs.add( x );
                                        vals.add( value );
                                    }
                                    edited.VibratoHandle.DepthBP = new VibratoBPList( xs.toArray( new float[]{} ), vals.toArray( new int[]{} ) );
                                }
                            }
                        }
                        CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(),
                                                                                                              event_id,
                                                                                                              edited ) );
                        executeCommand( run, true );
                    }
                } else {
                    Vector<BPPair> edit = new Vector<BPPair>();
                    edit.add( new BPPair( clock, value ) );
                    CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandTrackCurveEdit( AppManager.getSelected(), 
                                                                                                         m_selected_curve.Name, 
                                                                                                         edit ) );
                    executeCommand( run, true );
                }
            } else if ( m_mouse_down_mode == MouseDownMode.VEL_WAIT_HOVER ) {
#if DEBUG
                AppManager.debugWriteLine( "    entered VelEdit" );
                AppManager.debugWriteLine( "    m_veledit_selected.Count=" + m_veledit_selected.size() );
                AppManager.debugWriteLine( "    m_veledit_last_selectedid=" + m_veledit_last_selectedid );
                AppManager.debugWriteLine( "    m_veledit_selected.ContainsKey(m_veledit_last_selectedid" + m_veledit_selected.containsKey( m_veledit_last_selectedid ) );
#endif
                m_mouse_down_mode = MouseDownMode.VEL_EDIT;
                this.Invalidate();
            }
        }

        private void MouseHoverEventGenerator() {
#if DEBUG
            AppManager.debugWriteLine( "MouseHoverEventGenerator" );
#endif
            System.Threading.Thread.Sleep( (int)(SystemInformation.MouseHoverTime * 0.8) );
#if DEBUG
            AppManager.debugWriteLine( "   firing" );
#endif
            Invoke( new EventHandler( TrackSelector_MouseHover ) );
        }

        private void TrackSelector_MouseDoubleClick( object sender, MouseEventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "TrackSelector_MouseDoubleClick" );
#endif
            Keys modifier = Control.ModifierKeys;
            int x;
            if ( 0 <= e.Y && e.Y <= Height - 2 * OFFSET_TRACK_TAB ) {
                #region MouseDown occured on curve-pane
                if ( AppManager.KEY_LENGTH <= e.X && e.X <= Width ) {
                    if ( !m_selected_curve.equals( CurveType.VEL ) && !m_selected_curve.equals( CurveType.Accent ) && !m_selected_curve.equals( CurveType.Decay ) && !m_selected_curve.equals( CurveType.Env ) ) {
                        // check whether bezier point exists on clicked position
                        int track = AppManager.getSelected();
                        int clock = clockFromXCoord( e.X );
                        Vector<BezierChain> dict = AppManager.getVsqFile().AttachedCurves.get( track - 1 ).get( m_selected_curve );
                        BezierChain target_chain = null;
                        BezierPoint target_point = null;
                        boolean found = false;
                        //foreach ( BezierChain bc in dict.Values ) {
                        for ( int i = 0; i < dict.size(); i++ ) {
                            BezierChain bc = dict.get( i );
                            for ( Iterator itr = bc.points.iterator(); itr.hasNext(); ){
                                BezierPoint bp = (BezierPoint)itr.next();
                                Point pt = getScreenCoord( bp.getBase() );
                                Rectangle rc = new Rectangle( pt.X - DOT_WID, pt.Y - DOT_WID, 2 * DOT_WID + 1, 2 * DOT_WID + 1 );
                                if ( isInRect( e.Location, rc ) ) {
                                    found = true;
                                    target_point = (BezierPoint)bp.Clone();
                                    target_chain = (BezierChain)bc.Clone();
                                    break;
                                }

                                if ( bp.getControlLeftType() != BezierControlType.None ) {
                                    pt = getScreenCoord( bp.getControlLeft() );
                                    rc = new Rectangle( pt.X - DOT_WID, pt.Y - DOT_WID, 2 * DOT_WID + 1, 2 * DOT_WID + 1 );
                                    if ( isInRect( e.Location, rc ) ) {
                                        found = true;
                                        target_point = (BezierPoint)bp.Clone();
                                        target_chain = (BezierChain)bc.Clone();
                                        break;
                                    }
                                }
                                if ( bp.getControlRightType() != BezierControlType.None ) {
                                    pt = getScreenCoord( bp.getControlRight() );
                                    rc = new Rectangle( pt.X - DOT_WID, pt.Y - DOT_WID, 2 * DOT_WID + 1, 2 * DOT_WID + 1 );
                                    if ( isInRect( e.Location, rc ) ) {
                                        found = true;
                                        target_point = (BezierPoint)bp.Clone();
                                        target_chain = (BezierChain)bc.Clone();
                                        break;
                                    }
                                }
                            }
                            if ( found ) {
                                break;
                            }
                        }
                        if ( found ) {
                            int chain_id = target_chain.id;
                            BezierChain before = (BezierChain)target_chain.Clone();
                            using ( FormBezierPointEdit fbpe = new FormBezierPointEdit( this,
                                                                                        m_selected_curve,
                                                                                        chain_id,
                                                                                        target_point.ID ) ) {
                                EditingChainID = chain_id;
                                EditingPointID = target_point.ID;
                                DialogResult ret = fbpe.ShowDialog();
                                EditingChainID = -1;
                                EditingPointID = -1;
                                BezierChain after = AppManager.getVsqFile().AttachedCurves.get( AppManager.getSelected() - 1 ).getBezierChain( m_selected_curve, chain_id );
                                // 編集前の状態に戻す
                                CadenciiCommand revert = VsqFileEx.generateCommandReplaceBezierChain( track,
                                                                                               m_selected_curve,
                                                                                               chain_id,
                                                                                               before,
                                                                                               AppManager.editorConfig.ControlCurveResolution.Value );
                                executeCommand( revert, false );
                                if ( ret == DialogResult.OK ) {
                                    // ダイアログの結果がOKで、かつベジエ曲線が単調増加なら編集を適用
                                    if ( BezierChain.isBezierImplicit( target_chain ) ) {
                                        CadenciiCommand run = VsqFileEx.generateCommandReplaceBezierChain( track,
                                                                                                    m_selected_curve,
                                                                                                    chain_id,
                                                                                                    after,
                                                                                                    AppManager.editorConfig.ControlCurveResolution.Value );
                                        executeCommand( run, true );
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            } else if ( Height - 2 * OFFSET_TRACK_TAB <= e.Y && e.Y <= Height - OFFSET_TRACK_TAB ) {
                #region MouseDown occured on singer list
                if ( AppManager.getSelectedTool() != EditTool.ERASER ) {
                    VsqEvent ve = findItemAt( e.Location, out x );
                    String renderer = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getCommon().Version;
                    if ( ve != null ) {
                        if ( ve.ID.type != VsqIDType.Singer ) {
                            return;
                        }
                        if ( m_cmenu_singer_prepared != renderer ) {
                            InitCmenuSinger( renderer );
                        }
                        TagForCMenuSinger tag = new TagForCMenuSinger();
                        tag.SingerChangeExists = true;
                        tag.InternalID = ve.InternalID;
                        cmenuSinger.Tag = tag;//                        new KeyValuePair<boolean, int>( true, ve.InternalID );
                        foreach ( ToolStripMenuItem tsmi in cmenuSinger.Items ) {
                            TagForCMenuSingerDropDown tag2 = (TagForCMenuSingerDropDown)tsmi.Tag;
                            if ( tag2.SingerName.Equals( ve.ID.IconHandle.IDS ) ) {
                                tsmi.Checked = true;
                            } else {
                                tsmi.Checked = false;
                            }
                        }
                        cmenuSinger.Show( this, e.Location );
                    } else {
                        if ( m_cmenu_singer_prepared != renderer ) {
                            InitCmenuSinger( renderer );
                        }
                        String singer = AppManager.editorConfig.DefaultSingerName;
                        int clock = clockFromXCoord( e.X );
                        int last_clock = 0;
                        for ( Iterator itr = AppManager.getVsqFile().Track.get( AppManager.getSelected() ).getSingerEventIterator(); itr.hasNext(); ) {
                            VsqEvent ve2 = (VsqEvent)itr.next();
                            if ( last_clock <= clock && clock < ve2.Clock ) {
                                singer = ve2.ID.IconHandle.IDS;
                                break;
                            }
                            last_clock = ve2.Clock;
                        }
                        TagForCMenuSinger tag = new TagForCMenuSinger();
                        tag.SingerChangeExists = false;
                        tag.Clock = clock;
                        cmenuSinger.Tag = tag;//                        new KeyValuePair<boolean, int>( false, clock );
                        foreach ( ToolStripMenuItem tsmi in cmenuSinger.Items ) {
                            tsmi.Checked = false;
                        }
                        cmenuSinger.Show( this, e.Location );
                    }
                }
                #endregion
            }
        }

        private void InitCmenuSinger( String renderer ) {
            cmenuSinger.Items.Clear();
            //Vector<Integer> list = new Vector<Integer>();
            Vector<SingerConfig> items = null;
            if ( renderer.StartsWith( VSTiProxy.RENDERER_UTU0 ) || renderer.StartsWith( VSTiProxy.RENDERER_STR0 ) ) {
                items = AppManager.editorConfig.UtauSingers;
            } else if ( renderer.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                items = new Vector<SingerConfig>( VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID1 ) );
            } else if ( renderer.StartsWith( VSTiProxy.RENDERER_DSB3 ) ) {
                items = new Vector<SingerConfig>( VocaloSysUtil.getSingerConfigs( SynthesizerType.VOCALOID2 ) );
            } else {
                return;
            }
            int count = 0;
            for( Iterator itr = items.iterator(); itr.hasNext(); ){
                SingerConfig sc = (SingerConfig)itr.next();
                String tip = "";
                if ( renderer.StartsWith( VSTiProxy.RENDERER_UTU0 ) || renderer.StartsWith( VSTiProxy.RENDERER_STR0 ) ){
                    //sc = AppManager.getSingerInfoUtau( i );
                    if ( sc != null ) {
                        tip = "Name: " + sc.VOICENAME +
                              "\nDirectory: " + sc.VOICEIDSTR;
                    }
                } else if ( renderer.StartsWith( VSTiProxy.RENDERER_DSB2 ) ){
                    //sc = VocaloSysUtil.getSingerInfo1( i );
                    if ( sc != null ) {
                        tip = "Original: " + VocaloSysUtil.getOriginalSinger( sc.VOICENAME, SynthesizerType.VOCALOID1 ) +
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
                } else if ( renderer.StartsWith( VSTiProxy.RENDERER_DSB3 ) ) {
                    //sc = VocaloSysUtil.getSingerInfo2( i );
                    if ( sc != null ) {
                        tip = "Original: " + VocaloSysUtil.getOriginalSinger( sc.VOICENAME, SynthesizerType.VOCALOID2 ) +
                              "\nBreathiness: " + sc.Breathiness +
                              "\nBrightness: " + sc.Brightness +
                              "\nClearness: " + sc.Clearness +
                              "\nGender Factor: " + sc.GenderFactor +
                              "\nOpening: " + sc.Opening;
                    }
                }
                if ( sc != null ) {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem( sc.VOICENAME );
                    TagForCMenuSingerDropDown tag = new TagForCMenuSingerDropDown();
                    tag.SingerName = sc.VOICENAME;
                    tag.ToolTipText = tip;
                    tag.ToolTipPxWidth = 0;
                    tsmi.Tag = tag;
                    tsmi.Click += new EventHandler( tsmi_Click );
                    if ( AppManager.editorConfig.Platform == Platform.Windows ) {
                        // TODO: cmenuSinger.ItemsのToolTip。monoで実行するとMouseHoverで落ちる
                        tsmi.MouseEnter += new EventHandler( tsmi_MouseEnter );
                    }
                    cmenuSinger.Items.Add( tsmi );
                    //list.Add( i );
                    count++;
                }
            }
            cmenuSinger.VisibleChanged += new EventHandler( cmenuSinger_VisibleChanged );
            m_cmenusinger_tooltip_width = new int[count];
            //m_cmenusinger_map = list.ToArray();
            for ( int i = 0; i < count; i++ ) {
                m_cmenusinger_tooltip_width[i] = 0;
            }
            m_cmenu_singer_prepared = renderer;
        }

        private void cmenuSinger_VisibleChanged( object sender, EventArgs e ) {
            toolTip.Hide( cmenuSinger );
        }
        
        private void tsmi_MouseEnter( object sender, EventArgs e ) {
            toolTip.Hide( cmenuSinger );
        }
        
        private void tsmi_MouseHover( object sender, EventArgs e ) {
#if DEBUG
            try {
#endif
                TagForCMenuSingerDropDown tag = (TagForCMenuSingerDropDown)((ToolStripMenuItem)sender).Tag;
                String tip = tag.ToolTipText;
                String singer = tag.SingerName;

                // tooltipを表示するy座標を決める
                int y = 0;
                for ( int i = 0; i < cmenuSinger.Items.Count; i++ ) {
                    TagForCMenuSingerDropDown tag2 = (TagForCMenuSingerDropDown)cmenuSinger.Items[i].Tag;
                    String singer2 = tag2.SingerName;
                    if ( singer.Equals( singer2 ) ) {
                        break;
                    }
                    y += cmenuSinger.Items[i].Height;
                }

                int tip_width = tag.ToolTipPxWidth;
                Point pts = cmenuSinger.PointToScreen( new Point( 0, 0 ) );
                Rectangle rc = Screen.GetBounds( this );
                toolTip.Tag = singer;
                if ( pts.X + cmenuSinger.Width + tip_width > rc.Width ) {
                    toolTip.Show( tip, cmenuSinger, new Point( -tip_width, y ), 5000 );
                } else {
                    toolTip.Show( tip, cmenuSinger, new Point( cmenuSinger.Width, y ), 5000 );
                }
#if DEBUG
            } catch ( Exception ex ) {
                Console.WriteLine( "TarckSelectro.tsmi_MouseHover; ex=" + ex );
                AppManager.debugWriteLine( "TarckSelectro.tsmi_MouseHover; ex=" + ex );
            }
#endif
        }

        private struct TagForCMenuSinger {
            public boolean SingerChangeExists;
            public int Clock;
            public int InternalID;
            //public String Renderer;
        }

        private struct TagForCMenuSingerDropDown {
            public String SingerName;
            public int ToolTipPxWidth;
            public String ToolTipText;
        }

        private void tsmi_Click( object sender, EventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "CmenuSingerClick" );
            AppManager.debugWriteLine( "    sender.GetType()=" + sender.GetType() );
            AppManager.debugWriteLine( "    ((ToolStripMenuItem)sender).Tag.GetType()=" + ((ToolStripMenuItem)sender).Tag.GetType() );
#endif
            if ( sender is ToolStripMenuItem ) {
                TagForCMenuSinger tag = (TagForCMenuSinger)cmenuSinger.Tag;
                TagForCMenuSingerDropDown tag_dditem = (TagForCMenuSingerDropDown)((ToolStripMenuItem)sender).Tag;
                String singer = tag_dditem.SingerName;
                VsqID item = null;
                if ( m_cmenu_singer_prepared.StartsWith( VSTiProxy.RENDERER_DSB2 ) ) {
                    item = VocaloSysUtil.getSingerID( singer, SynthesizerType.VOCALOID1 );
                } else if ( m_cmenu_singer_prepared.StartsWith( VSTiProxy.RENDERER_DSB3 ) ) {
                    item = VocaloSysUtil.getSingerID( singer, SynthesizerType.VOCALOID2 );
                } else if ( m_cmenu_singer_prepared.StartsWith( VSTiProxy.RENDERER_UTU0 ) || m_cmenu_singer_prepared.StartsWith( VSTiProxy.RENDERER_STR0 ) ) {
                    item = AppManager.getSingerIDUtau( singer );
                }
                if ( item != null ) {
                    if ( tag.SingerChangeExists ) {
                        int id = tag.InternalID;
                        CadenciiCommand run = new CadenciiCommand(
                            VsqCommand.generateCommandEventChangeIDContaints( AppManager.getSelected(), id, item ) );
#if DEBUG
                        AppManager.debugWriteLine( "tsmi_Click; item.IconHandle.Program" + item.IconHandle.Program );
#endif
                        executeCommand( run, true );
                    } else {
                        int clock = tag.Clock;
                        VsqEvent ve = new VsqEvent( clock, item );
                        CadenciiCommand run = new CadenciiCommand( VsqCommand.generateCommandEventAdd( AppManager.getSelected(), ve ) );
                        executeCommand( run, true );
                    }
                }
            }
        }
        
        private void toolTip_Draw( object sender, DrawToolTipEventArgs e ) {
            Rectangle rc = e.Bounds;
#if DEBUG
            AppManager.debugWriteLine( "toolTip_Draw" );
            AppManager.debugWriteLine( "    sender.GetType()=" + sender.GetType() );
#endif
            String singer = (String)((ToolTip)sender).Tag;
            foreach ( ToolStripItem tsi in cmenuSinger.Items ) {
                if ( tsi is ToolStripMenuItem ) {
                    TagForCMenuSingerDropDown tag = (TagForCMenuSingerDropDown)((ToolStripMenuItem)tsi).Tag;
                    if ( tag.SingerName.Equals( singer ) ) {
                        tag.ToolTipPxWidth = rc.Width;
                        ((ToolStripMenuItem)tsi).Tag = tag;
                        break;
                    }
                }
            }
            e.DrawBackground();
            e.DrawBorder();
            e.DrawText( TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoFullWidthCharacterBreak );
        }

        private void TrackSelector_KeyDown( object sender, KeyEventArgs e ) {
            if ( (e.KeyCode & Keys.Space) == Keys.Space ) {
                m_spacekey_downed = true;
            }
        }

        private void TrackSelector_KeyUp( object sender, KeyEventArgs e ) {
            if ( (e.KeyCode & Keys.Space) == Keys.Space ) {
                m_spacekey_downed = false;
            }
        }

        private void cmenuCurveCommon_Click( object sender, EventArgs e ) {
            if ( sender is ToolStripMenuItem ) {
                ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
                if ( tsmi.Tag is CurveType ) {
                    CurveType curve = (CurveType)tsmi.Tag;
                    changeCurve( curve );
                }
            }
        }

        private void panelZoomButton_Paint( object sender, PaintEventArgs e ) {
            using ( Pen p = new Pen( Color.FromArgb( 118, 123, 138 ) ) ) {
                // 外枠
                int halfheight = panelZoomButton.Height / 2;
                e.Graphics.DrawRectangle( p, new Rectangle( 0, 0, panelZoomButton.Width - 1, panelZoomButton.Height - 1 ) );
                e.Graphics.DrawLine( p, new Point( 0, halfheight ), new Point( panelZoomButton.Width, halfheight ) );
                /*if ( m_selected_curve == CurveType.PIT ) {
                    int halfwidth = panelZoomButton.Width / 2;
                    int quoterheight = panelZoomButton.Height / 4;
                    // +の文字
                    e.Graphics.DrawLine( Pens.Black, new Point( halfwidth - 4, quoterheight ), new Point( halfwidth + 4, quoterheight ) );
                    e.Graphics.DrawLine( Pens.Black, new Point( halfwidth, quoterheight - 4 ), new Point( halfwidth, quoterheight + 4 ) );

                    // -の文字
                    e.Graphics.DrawLine( Pens.Black, new Point( halfwidth - 4, quoterheight + halfheight ), new Point( halfwidth + 4, quoterheight + halfheight ) );
                }*/
            }
        }

        private void panelZoomButton_MouseDown( object sender, MouseEventArgs e ) {
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
    }

}
