/*
 * EditorConfig.cs
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
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

using bocoree;
using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;

namespace Boare.Cadencii {

    using boolean = System.Boolean;

    /// <summary>
    /// Cadenciiの環境設定
    /// </summary>
    public class EditorConfig {
        [XmlItemDescription( "Default Pre-measure" )]
        public int DefaultPreMeasure = 4;
        //[Obsolete][XmlItemDescription( "Program change of default singer")]
        //public int DefaultSinger = 0;
        public String DefaultSingerName = "Miku";
        [XmlItemDescription( "Default scale for width direction in pixel/clock" )]
        public int DefaultXScale = 65;
        public String BaseFontName = "MS UI Gothic";
        [XmlItemDescription( "Font size in GraphicsUnit.Point unit" )]
        public float BaseFontSize = 9.0f;
        public String ScreenFontName = "MS UI Gothic";
        //public String CounterFontName = "Arial";
        [XmlItemDescription( "Mouse wheel speed" )]
        public int WheelOrder = 20;
        [XmlItemDescription( "Fix Play Cursor to Center" )]
        public boolean CursorFixed = false;
        /// <summary>
        /// RecentFilesに登録することの出来る最大のファイル数
        /// </summary>
        [XmlItemDescription( "Capacity of RecentFiles" )]
        public int NumRecentFiles = 5;
        /// <summary>
        /// 最近使用したファイルのリスト
        /// </summary>
        [XmlItemDescription( "List of files recentry used" )]
        public Vector<String> RecentFiles = new Vector<String>();
        public int DefaultPMBendDepth = 8;
        public int DefaultPMBendLength = 0;
        public int DefaultPMbPortamentoUse = 3;
        public int DefaultDEMdecGainRate = 50;
        public int DefaultDEMaccent = 50;
        [XmlItemDescription( "Drawing lyrics to screen or not" )]
        public boolean ShowLyric = true;
        public boolean ShowExpLine = true;
        public DefaultVibratoLength DefaultVibratoLength = DefaultVibratoLength.L66;
        public AutoVibratoMinLength AutoVibratoMinimumLength = AutoVibratoMinLength.L1;
        //public VibratoType AutoVibratoType = VibratoType.NormalType1;
        public String AutoVibratoType1 = "$04040001";
        public String AutoVibratoType2 = "$04040001";
        public boolean EnableAutoVibrato = true;
        public int PxTrackHeight = 14;
        public int MouseDragIncrement = 50;
        public int MouseDragMaximumRate = 600;
        public boolean MixerVisible = false;
        public int PreSendTime = 500;
        public ClockResolution ControlCurveResolution = ClockResolution.L30;
        public String Language = "";
        /// <summary>
        /// マウスの操作などの許容範囲。プリメジャーにPxToleranceピクセルめり込んだ入力を行っても、エラーにならない。(補正はされる)
        /// </summary>
        [XmlItemDescription( "Tolerance of mouse operation (in pixel)" )]
        public int PxTolerance = 10;
        /// <summary>
        /// マウスホイールでピアノロールを水平方向にスクロールするかどうか。
        /// </summary>
        [XmlItemDescription( "Whether scroll horizontally with mouse-wheel" )]
        public boolean ScrollHorizontalOnWheel = true;
        /// <summary>
        /// 画面描画の最大フレームレート
        /// </summary>
        [XmlItemDescription( "Maximum Frame Rate" )]
        public int MaximumFrameRate = 15;
        /// <summary>
        /// ユーザー辞書のOn/Offと順序
        /// </summary>
        public Vector<String> UserDictionaries = new Vector<String>();
        /// <summary>
        /// 実行環境
        /// </summary>
        public Platform Platform = Platform.Windows;
        /// <summary>
        /// toolStripToolの表示位置
        /// </summary>
        public ToolStripLocation ToolEditTool = new ToolStripLocation( new Point( 3, 0 ), ToolStripLocation.ParentPanel.Top );
        /// <summary>
        /// toolStripPositionの表示位置
        /// </summary>
        public ToolStripLocation ToolPositionLocation = new ToolStripLocation( new Point( 3, 25 ), ToolStripLocation.ParentPanel.Top );
        /// <summary>
        /// toolStripMeasureの表示位置
        /// </summary>
        public ToolStripLocation ToolMeasureLocation = new ToolStripLocation( new Point( 212, 25 ), ToolStripLocation.ParentPanel.Top );
        public ToolStripLocation ToolFileLocation = new ToolStripLocation( new Point( 461, 0 ), ToolStripLocation.ParentPanel.Top );
        //public ToolStripLocation ToolPaletteLocation = new ToolStripLocation( new Point( 3, 100 ), ToolStripLocation.ParentPanel.Top );
        public boolean WindowMaximized = false;
        public Rectangle WindowRect = new Rectangle( 0, 0, 970, 718 );
        /// <summary>
        /// hScrollのスクロールボックスの最小幅(px)
        /// </summary>
        public int MinimumScrollHandleWidth = 20;
        /// <summary>
        /// 発音記号入力モードを，維持するかどうか
        /// </summary>
        public boolean KeepLyricInputMode = false;
        /// <summary>
        /// 最後に使用したVSQファイルへのパス
        /// </summary>
        public String LastVsqPath = "";
        /// <summary>
        /// ピアノロールの何もないところをクリックした場合、右クリックでもプレビュー音を再生するかどうか
        /// </summary>
        public boolean PlayPreviewWhenRightClick = false;
        /// <summary>
        /// ゲームコントローラで、異なるイベントと識別する最小の時間間隔(millisec)
        /// </summary>
        public int GameControlerMinimumEventInterval = 100;
        /// <summary>
        /// カーブの選択範囲もクオンタイズするかどうか
        /// </summary>
        public boolean CurveSelectingQuantized = true;

        private QuantizeMode m_position_quantize = QuantizeMode.p32;
        private boolean m_position_quantize_triplet = false;
        private QuantizeMode m_length_quantize = QuantizeMode.p32;
        private boolean m_length_quantize_triplet = false;
        private int m_mouse_hover_time = 500;
        private int m_gamectrl_tr = 0;
        private int m_gamectrl_o = 1;
        private int m_gamectrl_x = 2;
        private int m_gamectrl_re = 3;
        private int m_gamectrl_L1 = 4;
        private int m_gamectrl_R1 = 5;
        private int m_gamectrl_L2 = 6;
        private int m_gamectrl_R2 = 7;
        private int m_gamectrl_select = 8;
        private int m_gamectrl_start = 9;
        private int m_gamectrl_stickL = 10;
        private int m_gamectrl_stickR = 11;
        public boolean CurveVisibleVelocity = true;
        public boolean CurveVisibleAccent = true;
        public boolean CurveVisibleDecay = true;
        public boolean CurveVisibleVibratoRate = true;
        public boolean CurveVisibleVibratoDepth = true;
        public boolean CurveVisibleDynamics = true;
        public boolean CurveVisibleBreathiness = true;
        public boolean CurveVisibleBrightness = true;
        public boolean CurveVisibleClearness = true;
        public boolean CurveVisibleOpening = true;
        public boolean CurveVisibleGendorfactor = true;
        public boolean CurveVisiblePortamento = true;
        public boolean CurveVisiblePit = true;
        public boolean CurveVisiblePbs = true;
        public boolean CurveVisibleHarmonics = false;
        public boolean CurveVisibleFx2Depth = false;
        public boolean CurveVisibleReso1 = false;
        public boolean CurveVisibleReso2 = false;
        public boolean CurveVisibleReso3 = false;
        public boolean CurveVisibleReso4 = false;
        public boolean CurveVisibleEnvelope = false;
        public int GameControlPovRight = 9000;
        public int GameControlPovLeft = 27000;
        public int GameControlPovUp = 0;
        public int GameControlPovDown = 18000;
        /// <summary>
        /// wave波形を表示するかどうか
        /// </summary>
        public boolean ViewWaveform = false;
        /// <summary>
        /// キーボードからの入力に使用するデバイス
        /// </summary>
        public MidiPortConfig MidiInPort = new MidiPortConfig();
        public RgbColor PianorollColorVocalo2Black = new RgbColor( 212, 212, 212 );
        public RgbColor PianorollColorVocalo2White = new RgbColor( 240, 240, 240 );
        public RgbColor PianorollColorVocalo1Black = new RgbColor( 210, 205, 172 );
        public RgbColor PianorollColorVocalo1White = new RgbColor( 240, 235, 214 );
        public RgbColor PianorollColorUtauBlack = new RgbColor( 212, 212, 212 );
        public RgbColor PianorollColorUtauWhite = new RgbColor( 240, 240, 240 );
        public RgbColor PianorollColorVocalo1Bar = new RgbColor( 161, 157, 136 );
        public RgbColor PianorollColorVocalo1Beat = new RgbColor( 209, 204, 172 );
        public RgbColor PianorollColorVocalo2Bar = new RgbColor( 161, 157, 136 );
        public RgbColor PianorollColorVocalo2Beat = new RgbColor( 209, 204, 172 );
        public RgbColor PianorollColorUtauBar = new RgbColor( 255, 64, 255 );
        public RgbColor PianorollColorUtauBeat = new RgbColor( 128, 128, 255 );
        public RgbColor PianorollColorStraightBlack = new RgbColor( 212, 212, 212 );
        public RgbColor PianorollColorStraightWhite = new RgbColor( 240, 240, 240 );
        public RgbColor PianorollColorStraightBar = new RgbColor( 255, 153, 0 );
        public RgbColor PianorollColorStraightBeat = new RgbColor( 128, 128, 255 );
        [XmlItemDescription( "Show actual pitch line or not" )]
        public boolean ViewAtcualPitch = false;
        [XmlItemDescription( "Invoke resampler with Wine" )]
        public boolean InvokeUtauCoreWithWine = false;
        public Vector<SingerConfig> UtauSingers = new Vector<SingerConfig>();
        public String PathResampler = "";
        public String PathWavtool = "";
        public boolean UseCustomFileDialog = false;
        /// <summary>
        /// ベジエ制御点を掴む時の，掴んだと判定する際の誤差．制御点の外輪からPxToleranceBezierピクセルずれていても，掴んだと判定する．
        /// </summary>
        public int PxToleranceBezier = 10;
        /// <summary>
        /// 歌詞入力においてローマ字が入力されたとき，Cadencii側でひらがなに変換するかどうか
        /// </summary>
        public boolean SelfDeRomanization = false;
        /// <summary>
        /// openMidiDialogで最後に選択された拡張子
        /// </summary>
        public String LastUsedExtension = ".vsq";
        /// <summary>
        /// ミキサーダイアログを常に手前に表示するかどうか
        /// </summary>
        public boolean MixerTopMost = false;
        public Vector<ValuePairOfStringArrayOfKeys> ShortcutKeys = new Vector<ValuePairOfStringArrayOfKeys>();
        /// <summary>
        /// リアルタイム再生時の再生速度
        /// </summary>
        private float m_realtime_input_speed = 1.0f;
        public byte MidiProgramNormal = 115;
        public byte MidiProgramBell = 9;
        public byte MidiNoteNormal = 65;
        public byte MidiNoteBell = 65;
        public boolean MidiRingBell = true;
        public int MidiPreUtterance = 0;
        public MidiPortConfig MidiDeviceMetronome = new MidiPortConfig();
        public MidiPortConfig MidiDeviceGeneral = new MidiPortConfig();
        public boolean MetronomeEnabled = true;
        public PropertyPanelState PropertyWindowStatus = new PropertyPanelState();
        //public FormConfigUtauVoiceConfig FormConfigUtauVoiceConfig = new FormConfigUtauVoiceConfig( new Size( 714, 533 ), 70.0f, 60.0f, 20 );
        /// <summary>
        /// 概観ペインが表示されているかどうか
        /// </summary>
        public boolean OverviewEnabled = false;
        public int OverviewScaleCount = 5;
        public FormMidiImExportConfig MidiImExportConfigExport = new FormMidiImExportConfig();
        public FormMidiImExportConfig MidiImExportConfigImport = new FormMidiImExportConfig();
        public FormMidiImExportConfig MidiImExportConfigImportVsq = new FormMidiImExportConfig();
        public int AutoBackupIntervalMinutes = 10;

        #region Static Fields
        public static readonly Vector<ValuePairOfStringArrayOfKeys> DEFAULT_SHORTCUT_KEYS = new Vector<ValuePairOfStringArrayOfKeys>(
            new ValuePairOfStringArrayOfKeys[]{
            new ValuePairOfStringArrayOfKeys( "menuFileNew", new Keys[]{ Keys.Control, Keys.N } ),
            new ValuePairOfStringArrayOfKeys( "menuFileOpen", new Keys[]{ Keys.Control, Keys.O } ),
            new ValuePairOfStringArrayOfKeys( "menuFileOpenVsq", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuFileSave", new Keys[]{ Keys.Control, Keys.S } ),
            new ValuePairOfStringArrayOfKeys( "menuFileQuit", new Keys[]{ Keys.Control, Keys.Q } ),
            new ValuePairOfStringArrayOfKeys( "menuEditUndo", new Keys[]{ Keys.Control, Keys.Z } ),
            new ValuePairOfStringArrayOfKeys( "menuEditRedo", new Keys[]{ Keys.Control, Keys.Shift, Keys.Z } ),
            new ValuePairOfStringArrayOfKeys( "menuEditCut", new Keys[]{ Keys.Control, Keys.X } ),
            new ValuePairOfStringArrayOfKeys( "menuEditCopy", new Keys[]{ Keys.Control, Keys.C } ),
            new ValuePairOfStringArrayOfKeys( "menuEditPaste", new Keys[]{ Keys.Control, Keys.V } ),
            new ValuePairOfStringArrayOfKeys( "menuEditSelectAll", new Keys[]{ Keys.Control, Keys.A } ),
            new ValuePairOfStringArrayOfKeys( "menuEditSelectAllEvents", new Keys[]{ Keys.Control, Keys.Shift, Keys.A } ),
            new ValuePairOfStringArrayOfKeys( "menuEditDelete", new Keys[]{ Keys.Delete } ),
            new ValuePairOfStringArrayOfKeys( "menuVisualMixer", new Keys[]{ Keys.F3 } ),
            new ValuePairOfStringArrayOfKeys( "menuJobRealTime", new Keys[]{ Keys.F5 } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenEditLyric", new Keys[]{ Keys.F2 } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenEditFlipToolPointerPencil", new Keys[]{ Keys.Control, Keys.W } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenEditFlipToolPointerEraser", new Keys[]{ Keys.Control, Keys.E } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenVisualForwardParameter", new Keys[]{ Keys.Control, Keys.Alt, Keys.PageDown } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenVisualBackwardParameter", new Keys[]{ Keys.Control, Keys.Alt, Keys.PageUp } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenTrackNext", new Keys[]{ Keys.Control, Keys.PageDown } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenTrackBack", new Keys[]{ Keys.Control, Keys.PageUp } ),
            new ValuePairOfStringArrayOfKeys( "menuFileSaveNamed", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuFileImportVsq", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuFileOpenUst", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuFileImportMidi", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuFileExportWave", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuFileExportMidi", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuFileDelete", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualWaveform", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualProperty", new Keys[]{ Keys.F6 } ),
            new ValuePairOfStringArrayOfKeys( "menuVisualGridline", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualStartMarker", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualEndMarker", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualLyrics", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualNoteProperty", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualPitchLine", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuJobNormalize", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuJobInsertBar", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuJobDeleteBar", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuJobRandomize", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuJobConnect", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuJobLyric", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackOn", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackAdd", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackCopy", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackChangeName", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackDelete", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackRenderCurrent", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackRenderAll", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackOverlay", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackRendererVOCALOID1", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackRendererVOCALOID2", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackRendererUtau", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackMasterTuning", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuLyricExpressionProperty", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuLyricVibratoProperty", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuLyricSymbol", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuLyricDictionary", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuScriptUpdate", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuSettingPreference", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuSettingGameControlerSetting", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuSettingGameControlerReload", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuSettingPaletteTool", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuSettingShortcut", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuSettingSingerProperty", new Keys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuHelpAbout", new Keys[]{} ) } );
        private static XmlSerializer s_serializer = new XmlSerializer( typeof( EditorConfig ) );
        #endregion

        /// <summary>
        /// PositionQuantize, PositionQuantizeTriplet, LengthQuantize, LengthQuantizeTripletの描くプロパティのいずれかが
        /// 変更された時発生します
        /// </summary>
        public static event EventHandler QuantizeModeChanged;

        public Keys GetShortcutKeyFor( ToolStripMenuItem menu_item ) {
            String name = menu_item.Name;
            for( Iterator itr = ShortcutKeys.iterator(); itr.hasNext(); ){
                ValuePairOfStringArrayOfKeys item = (ValuePairOfStringArrayOfKeys)itr.next();
                if ( name.Equals( item.Key ) ) {
                    Keys ret = Keys.None;
                    foreach ( Keys k in item.Value ) {
                        ret = ret | k;
                    }
                    return ret;
                }
            }
            return Keys.None;
        }

        /// <summary>
        /// リアルタイム再生時の再生速度
        /// </summary>
        public float RealtimeInputSpeed {
            get {
                if ( m_realtime_input_speed <= 0.0f ) {
                    m_realtime_input_speed = 1.0f;
                }
                return m_realtime_input_speed;
            }
            set {
                m_realtime_input_speed = value;
                if ( m_realtime_input_speed <= 0.0f ) {
                    m_realtime_input_speed = 1.0f;
                }
            }
        }

        public TreeMap<String, Keys[]> GetShortcutKeysDictionary() {
            TreeMap<String, Keys[]> ret = new TreeMap<String, Keys[]>();
            for ( int i = 0; i < ShortcutKeys.size(); i++ ) {
                ret.put( ShortcutKeys.get( i ).Key, ShortcutKeys.get( i ).Value );
            }
            return ret;
        }

        public static void Serialize( EditorConfig instance, String file ) {
            using ( FileStream fs = new FileStream( file, FileMode.Create, FileAccess.Write ) ) {
                s_serializer.Serialize( fs, instance );
            }
        }

        public static EditorConfig Deserialize( EditorConfig old_instance, String file ) {
            EditorConfig ret = null;
            using ( FileStream fs = new FileStream( file, FileMode.Open, FileAccess.Read ) ) {
                ret = (EditorConfig)s_serializer.Deserialize( fs );
            }
            for ( int j = 0; j < DEFAULT_SHORTCUT_KEYS.size(); j++ ) {
                boolean found = false;
                for ( int i = 0; i < ret.ShortcutKeys.size(); i++ ) {
                    if ( DEFAULT_SHORTCUT_KEYS.get( j ).Key.Equals( ret.ShortcutKeys.get( i ).Key ) ) {
                        found = true;
                        break;
                    }
                }
                if ( !found ) {
                    ret.ShortcutKeys.add( DEFAULT_SHORTCUT_KEYS.get( j ) );
                }
            }
            return ret;
        }

        public Font BaseFont {
            get {
                return new Font( BaseFontName, BaseFontSize, GraphicsUnit.Point );
            }
        }

        /// <summary>
        /// Button index of Left Stick
        /// </summary>
        public int GameControlStirckL {
            get {
                return m_gamectrl_stickL;
            }
            set {
                m_gamectrl_stickL = value;
            }
        }

        /// <summary>
        /// Button index of Right Stick
        /// </summary>
        public int GameControlStirckR {
            get {
                return m_gamectrl_stickR;
            }
            set {
                m_gamectrl_stickR = value;
            }
        }

        /// <summary>
        /// Button index of "START"
        /// </summary>
        public int GameControlStart {
            get {
                return m_gamectrl_start;
            }
            set {
                m_gamectrl_start = value;
            }
        }

        /// <summary>
        /// Button index of "R2"
        /// </summary>
        public int GameControlR2 {
            get {
                return m_gamectrl_R2;
            }
            set {
                m_gamectrl_R2 = value;
            }
        }

        /// <summary>
        /// Button index of "R1"
        /// </summary>
        public int GameControlR1 {
            get {
                return m_gamectrl_R1;
            }
            set {
                m_gamectrl_R1 = value;
            }
        }

        /// <summary>
        /// Button index of "L2"
        /// </summary>
        public int GameControlL2 {
            get {
                return m_gamectrl_L2;
            }
            set {
                m_gamectrl_L2 = value;
            }
        }

        /// <summary>
        /// Button index of "L1"
        /// </summary>
        public int GameControlL1 {
            get {
                return m_gamectrl_L1;
            }
            set {
                m_gamectrl_L1 = value;
            }
        }

        /// <summary>
        /// Button index of "SELECT"
        /// </summary>
        public int GameControlSelect {
            get {
                return m_gamectrl_select;
            }
            set {
                m_gamectrl_select = value;
            }
        }

        /// <summary>
        /// Button index of "□"
        /// </summary>
        public int GameControlerRectangle {
            get {
                return m_gamectrl_re;
            }
            set {
                m_gamectrl_re = value;
            }
        }

        /// <summary>
        /// Button index of "×"
        /// </summary>
        public int GameControlerCross {
            get {
                return m_gamectrl_x;
            }
            set {
                m_gamectrl_x = value;
            }
        }

        /// <summary>
        /// Button index of "○"
        /// </summary>
        public int GameControlerCircle {
            get {
                return m_gamectrl_o;
            }
            set {
                m_gamectrl_o = value;
            }
        }

        /// <summary>
        /// Button index of "△"
        /// </summary>
        public int GameControlerTriangle {
            get {
                return m_gamectrl_tr;
            }
            set {
                m_gamectrl_tr = value;
            }
        }

        /// <summary>
        /// ピアノロール上でマウスホバーイベントが発生するまでの時間(millisec)
        /// </summary>
        public int MouseHoverTime {
            get {
                return m_mouse_hover_time;
            }
            set {
                if ( value < 0 ) {
                    m_mouse_hover_time = 0;
                } else if ( 2000 < m_mouse_hover_time ) {
                    m_mouse_hover_time = 2000;
                } else {
                    m_mouse_hover_time = value;
                }
            }
        }

        public QuantizeMode PositionQuantize {
            get {
                return m_position_quantize;
            }
            set {
                if ( m_position_quantize != value ) {
                    m_position_quantize = value;
                    if ( QuantizeModeChanged != null ) {
                        QuantizeModeChanged( typeof( EditorConfig ), new EventArgs() );
                    }
                }
            }
        }

        public boolean PositionQuantizeTriplet {
            get {
                return m_position_quantize_triplet;
            }
            set {
                if ( m_position_quantize_triplet != value ) {
                    m_position_quantize_triplet = value;
                    if ( QuantizeModeChanged != null ) {
                        QuantizeModeChanged( typeof( EditorConfig ), new EventArgs() );
                    }
                }
            }
        }

        public QuantizeMode LengthQuantize {
            get {
                return m_length_quantize;
            }
            set {
                if ( m_length_quantize != value ) {
                    m_length_quantize = value;
                    if ( QuantizeModeChanged != null ) {
                        QuantizeModeChanged( typeof( EditorConfig ), new EventArgs() );
                    }
                }
            }
        }

        public boolean LengthQuantizeTriplet {
            get {
                return m_length_quantize_triplet;
            }
            set {
                if ( m_length_quantize_triplet != value ) {
                    m_length_quantize_triplet = value;
                    if ( QuantizeModeChanged != null ) {
                        QuantizeModeChanged( typeof( EditorConfig ), new EventArgs() );
                    }
                }
            }
        }

        /// <summary>
        /// 「最近使用したファイル」のリストに、アイテムを追加します
        /// </summary>
        /// <param name="new_file"></param>
        public void PushRecentFiles( String new_file ) {
#if DEBUG
            System.Diagnostics.Debug.WriteLine( "PushRecentFiles" );
#endif
            // NumRecentFilesは0以下かも知れない
            if ( NumRecentFiles <= 0 ) {
                NumRecentFiles = 5;
            }

            // RecentFilesはnullかもしれない．
            if ( RecentFiles == null ) {
                RecentFiles = new Vector<String>();
            }

            // 重複があれば消す
            Vector<String> dict = new Vector<String>();
            for( Iterator itr = RecentFiles.iterator(); itr.hasNext(); ){
                String s = (String)itr.next();
                boolean found = false;
                for ( int i = 0; i < dict.size(); i++ ) {
                    if ( s.Equals( dict.get( i ) ) ) {
                        found = true;
                    }
                }
                if ( !found ) {
                    dict.add( s );
                }
            }
            RecentFiles.clear();
            for ( Iterator itr = dict.iterator(); itr.hasNext(); ){
                String s = (String)itr.next();
                RecentFiles.add( s );
            }

            // 現在登録されているRecentFilesのサイズが規定より大きければ，下の方から消す
            if ( RecentFiles.size() > NumRecentFiles ) {
                for ( int i = RecentFiles.size() - 1; i > NumRecentFiles; i-- ) {
                    RecentFiles.removeElementAt( i );
                }
            }

            // 登録しようとしているファイルは，RecentFilesの中に既に登録されているかs？
            int index = -1;
            for ( int i = 0; i < RecentFiles.size(); i++ ) {
                if ( RecentFiles.get( i ).Equals( new_file ) ) {
                    index = i;
                    break;
                }
            }

            if ( index >= 0 ) {  // 登録されてる場合
                RecentFiles.removeElementAt( index );
            }
            RecentFiles.insertElementAt( new_file, 0 );
        }
    }

}
