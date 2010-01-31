/*
 * EditorConfig.cs
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

import java.awt.*;
import java.util.*;
import java.io.*;
import org.kbinani.*;
import org.kbinani.vsq.*;
import org.kbinani.xml.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani;
using org.kbinani.java.awt;
using org.kbinani.java.io;
using org.kbinani.java.util;
using org.kbinani.windows.forms;
using org.kbinani.xml;
using org.kbinani.vsq;

namespace org.kbinani.cadencii {
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;
#endif

    /// <summary>
    /// Cadenciiの環境設定
    /// </summary>
    public class EditorConfig {
        public int DefaultPreMeasure = 4;
        public String DefaultSingerName = "Miku";
        public int DefaultXScale = 65;
        public String BaseFontName = "MS UI Gothic";
        public float BaseFontSize = 9.0f;
        public String ScreenFontName = "MS UI Gothic";
        public int WheelOrder = 20;
        public boolean CursorFixed = false;
        /// <summary>
        /// RecentFilesに登録することの出来る最大のファイル数
        /// </summary>
        public int NumRecentFiles = 5;
        /// <summary>
        /// 最近使用したファイルのリスト
        /// </summary>
        public Vector<String> RecentFiles = new Vector<String>();
        public int DefaultPMBendDepth = 8;
        public int DefaultPMBendLength = 0;
        public int DefaultPMbPortamentoUse = 3;
        public int DefaultDEMdecGainRate = 50;
        public int DefaultDEMaccent = 50;
        public boolean ShowLyric = true;
        public boolean ShowExpLine = true;
        public DefaultVibratoLengthEnum DefaultVibratoLength = DefaultVibratoLengthEnum.L66;
        public AutoVibratoMinLengthEnum AutoVibratoMinimumLength = AutoVibratoMinLengthEnum.L1;
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
        public int PxTolerance = 10;
        /// <summary>
        /// マウスホイールでピアノロールを水平方向にスクロールするかどうか。
        /// </summary>
        public boolean ScrollHorizontalOnWheel = true;
        /// <summary>
        /// 画面描画の最大フレームレート
        /// </summary>
        public int MaximumFrameRate = 15;
        /// <summary>
        /// ユーザー辞書のOn/Offと順序
        /// </summary>
        public Vector<String> UserDictionaries = new Vector<String>();
        /// <summary>
        /// 実行環境
        /// </summary>
        public PlatformEnum Platform = PlatformEnum.Windows;
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
        /// <summary>
        /// Button index of "△"
        /// </summary>
        public int GameControlerTriangle = 0;
        /// <summary>
        /// Button index of "○"
        /// </summary>
        public int GameControlerCircle = 1;
        /// <summary>
        /// Button index of "×"
        /// </summary>
        public int GameControlerCross = 2;
        /// <summary>
        /// Button index of "□"
        /// </summary>
        public int GameControlerRectangle = 3;
        /// <summary>
        /// Button index of "L1"
        /// </summary>
        public int GameControlL1 = 4;
        /// <summary>
        /// Button index of "R1"
        /// </summary>
        public int GameControlR1 = 5;
        /// <summary>
        /// Button index of "L2"
        /// </summary>
        public int GameControlL2 = 6;
        /// <summary>
        /// Button index of "R2"
        /// </summary>
        public int GameControlR2 = 7;
        /// <summary>
        /// Button index of "SELECT"
        /// </summary>
        public int GameControlSelect = 8;
        /// <summary>
        /// Button index of "START"
        /// </summary>
        public int GameControlStart = 9;
        /// <summary>
        /// Button index of Left Stick
        /// </summary>
        public int GameControlStirckL = 10;
        /// <summary>
        /// Button index of Right Stick
        /// </summary>
        public int GameControlStirckR = 11;
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

        public RgbColor PianorollColorVocalo1Bar = new RgbColor( 161, 157, 136 );
        public RgbColor PianorollColorVocalo1Beat = new RgbColor( 209, 204, 172 );
        public RgbColor PianorollColorVocalo2Bar = new RgbColor( 161, 157, 136 );
        public RgbColor PianorollColorVocalo2Beat = new RgbColor( 209, 204, 172 );
        
        public RgbColor PianorollColorUtauBlack = new RgbColor( 212, 212, 212 );
        public RgbColor PianorollColorUtauWhite = new RgbColor( 240, 240, 240 );
        public RgbColor PianorollColorUtauBar = new RgbColor( 255, 64, 255 );
        public RgbColor PianorollColorUtauBeat = new RgbColor( 128, 128, 255 );
        
        public RgbColor PianorollColorStraightBlack = new RgbColor( 212, 212, 212 );
        public RgbColor PianorollColorStraightWhite = new RgbColor( 240, 240, 240 );
        public RgbColor PianorollColorStraightBar = new RgbColor( 255, 153, 0 );
        public RgbColor PianorollColorStraightBeat = new RgbColor( 128, 128, 255 );

        public RgbColor PianorollColorAquesToneBlack = new RgbColor( 212, 212, 212 );
        public RgbColor PianorollColorAquesToneWhite = new RgbColor( 240, 240, 240 );
        public RgbColor PianorollColorAquesToneBar = new RgbColor( 7, 107, 175 );
        public RgbColor PianorollColorAquesToneBeat = new RgbColor( 234, 190, 62 );

        public boolean ViewAtcualPitch = false;
        public boolean InvokeUtauCoreWithWine = false;
        public Vector<SingerConfig> UtauSingers = new Vector<SingerConfig>();
        public String PathResampler = "";
        public String PathWavtool = "";
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
        /// <summary>
        /// 鍵盤の表示幅、ピクセル,AppManager.keyWidthに代入。
        /// </summary>
        public int KeyWidth = 68;
        /// <summary>
        /// スペースキーを押しながら左クリックで、中ボタンクリックとみなす動作をさせるかどうか。
        /// </summary>
        public boolean UseSpaceKeyAsMiddleButtonModifier = false;
        /// <summary>
        /// AquesToneのVSTi dllへのパス
        /// </summary>
        public String PathAquesTone = "";
        /// <summary>
        /// アイコンパレット・ウィンドウの位置
        /// </summary>
        public XmlPoint FormIconPaletteLocation = new XmlPoint( 0, 0 );
        /// <summary>
        /// アイコンパレット・ウィンドウを常に手前に表示するかどうか
        /// </summary>
        public boolean FormIconTopMost = true;
        /// <summary>
        /// 前回エクスポートしたMusicXmlのパス
        /// </summary>
        public String LastMusicXmlPath = "";
        /// <summary>
        /// 最初に戻る、のショートカットキー
        /// </summary>
        public BKeys[] SpecialShortcutGoToFirst = new BKeys[] { BKeys.Home };
        /// <summary>
        /// waveファイル出力時のチャンネル数（1または2）
        /// </summary>
        public int WaveFileOutputChannel = 2;
        /// <summary>
        /// waveファイル出力時に、全トラックをmixして出力するかどうか
        /// </summary>
        public boolean WaveFileOutputFromMasterTrack = false;
        /// <summary>
        /// MTCスレーブ動作を行う際使用するMIDI INポートの設定
        /// </summary>
        public MidiPortConfig MidiInPortMtc = new MidiPortConfig();
        /// <summary>
        /// プロジェクトごとのキャッシュディレクトリを使うかどうか
        /// </summary>
        public boolean UseProjectCache = true;

        #region Static Fields
        public static readonly Vector<ValuePairOfStringArrayOfKeys> DEFAULT_SHORTCUT_KEYS = new Vector<ValuePairOfStringArrayOfKeys>( Arrays.asList(
            new ValuePairOfStringArrayOfKeys[]{
            new ValuePairOfStringArrayOfKeys( "menuFileNew", new BKeys[]{ BKeys.Control, BKeys.N } ),
            new ValuePairOfStringArrayOfKeys( "menuFileOpen", new BKeys[]{ BKeys.Control, BKeys.O } ),
            new ValuePairOfStringArrayOfKeys( "menuFileOpenVsq", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuFileSave", new BKeys[]{ BKeys.Control, BKeys.S } ),
            new ValuePairOfStringArrayOfKeys( "menuFileQuit", new BKeys[]{ BKeys.Control, BKeys.Q } ),
            new ValuePairOfStringArrayOfKeys( "menuEditUndo", new BKeys[]{ BKeys.Control, BKeys.Z } ),
            new ValuePairOfStringArrayOfKeys( "menuEditRedo", new BKeys[]{ BKeys.Control, BKeys.Shift, BKeys.Z } ),
            new ValuePairOfStringArrayOfKeys( "menuEditCut", new BKeys[]{ BKeys.Control, BKeys.X } ),
            new ValuePairOfStringArrayOfKeys( "menuEditCopy", new BKeys[]{ BKeys.Control, BKeys.C } ),
            new ValuePairOfStringArrayOfKeys( "menuEditPaste", new BKeys[]{ BKeys.Control, BKeys.V } ),
            new ValuePairOfStringArrayOfKeys( "menuEditSelectAll", new BKeys[]{ BKeys.Control, BKeys.A } ),
            new ValuePairOfStringArrayOfKeys( "menuEditSelectAllEvents", new BKeys[]{ BKeys.Control, BKeys.Shift, BKeys.A } ),
            new ValuePairOfStringArrayOfKeys( "menuEditDelete", new BKeys[]{ BKeys.Delete } ),
            new ValuePairOfStringArrayOfKeys( "menuVisualMixer", new BKeys[]{ BKeys.F3 } ),
            new ValuePairOfStringArrayOfKeys( "menuJobRealTime", new BKeys[]{ BKeys.F5 } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenEditLyric", new BKeys[]{ BKeys.F2 } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenEditFlipToolPointerPencil", new BKeys[]{ BKeys.Control, BKeys.W } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenEditFlipToolPointerEraser", new BKeys[]{ BKeys.Control, BKeys.E } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenVisualForwardParameter", new BKeys[]{ BKeys.Control, BKeys.Alt, BKeys.PageDown } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenVisualBackwardParameter", new BKeys[]{ BKeys.Control, BKeys.Alt, BKeys.PageUp } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenTrackNext", new BKeys[]{ BKeys.Control, BKeys.PageDown } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenTrackBack", new BKeys[]{ BKeys.Control, BKeys.PageUp } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenBackToTheFirst", new BKeys[]{ BKeys.Home } ),
            new ValuePairOfStringArrayOfKeys( "menuFileSaveNamed", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuFileImportVsq", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuFileOpenUst", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuFileImportMidi", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuFileExportWave", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuFileExportMidi", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuFileDelete", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualWaveform", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualProperty", new BKeys[]{ BKeys.F6 } ),
            new ValuePairOfStringArrayOfKeys( "menuVisualGridline", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualStartMarker", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualEndMarker", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualLyrics", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualNoteProperty", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualPitchLine", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuVisualIconPalette", new BKeys[]{ BKeys.F4 } ),
            new ValuePairOfStringArrayOfKeys( "menuJobNormalize", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuJobInsertBar", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuJobDeleteBar", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuJobRandomize", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuJobConnect", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuJobLyric", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackOn", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackAdd", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackCopy", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackChangeName", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackDelete", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackRenderCurrent", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackRenderAll", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackOverlay", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackRendererVOCALOID1", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackRendererVOCALOID2", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackRendererUtau", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuTrackMasterTuning", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuLyricExpressionProperty", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuLyricVibratoProperty", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuLyricSymbol", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuLyricDictionary", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuScriptUpdate", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuSettingPreference", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuSettingGameControlerSetting", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuSettingGameControlerReload", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuSettingPaletteTool", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuSettingShortcut", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuSettingSingerProperty", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuHelpAbout", new BKeys[]{} ) } ) );
#if JAVA
        private static XmlSerializer s_serializer = new XmlSerializer( EditorConfig.class );
#else
        private static XmlSerializer s_serializer = new XmlSerializer( typeof( EditorConfig ) );
#endif
        #endregion

        /// <summary>
        /// PositionQuantize, PositionQuantizeTriplet, LengthQuantize, LengthQuantizeTripletの描くプロパティのいずれかが
        /// 変更された時発生します
        /// </summary>
        public static BEvent<BEventHandler> quantizeModeChangedEvent = new BEvent<BEventHandler>();

        public int getControlCurveResolutionValue() {
            return ClockResolutionUtility.getValue( ControlCurveResolution );
        }

        public BKeys[] getShortcutKeyFor( BMenuItem menu_item ) {
            String name = menu_item.getName();
            Vector<BKeys> ret = new Vector<BKeys>();
            for ( Iterator itr = ShortcutKeys.iterator(); itr.hasNext(); ) {
                ValuePairOfStringArrayOfKeys item = (ValuePairOfStringArrayOfKeys)itr.next();
                if ( name.Equals( item.Key ) ) {
                    for ( int i = 0; i < item.Value.Length; i++ ) {
                        BKeys k = item.Value[i];
                        ret.add( k );
                    }
                    return ret.toArray( new BKeys[] { } );
                }
            }
            return ret.toArray( new BKeys[] { } );
        }

        public float getRealtimeInputSpeed() {
            if ( m_realtime_input_speed <= 0.0f ) {
                m_realtime_input_speed = 1.0f;
            }
            return m_realtime_input_speed;
        }

        public void setRealtimeInputSpeed( float value ) {
            m_realtime_input_speed = value;
            if ( m_realtime_input_speed <= 0.0f ) {
                m_realtime_input_speed = 1.0f;
            }
        }

#if !JAVA
        // XMLシリアライズ用
        /// <summary>
        /// リアルタイム再生時の再生速度
        /// </summary>
        public float RealtimeInputSpeed {
            get {
                return getRealtimeInputSpeed();
            }
            set {
                setRealtimeInputSpeed( value );
            }
        }
#endif

        public TreeMap<String, BKeys[]> getShortcutKeysDictionary() {
            TreeMap<String, BKeys[]> ret = new TreeMap<String, BKeys[]>();
            for ( int i = 0; i < ShortcutKeys.size(); i++ ) {
                ret.put( ShortcutKeys.get( i ).Key, ShortcutKeys.get( i ).Value );
            }
            for ( Iterator itr = DEFAULT_SHORTCUT_KEYS.iterator(); itr.hasNext(); ) {
                ValuePairOfStringArrayOfKeys item = (ValuePairOfStringArrayOfKeys)itr.next();
                if ( !ret.containsKey( item.Key ) ) {
                    ret.put( item.Key, item.Value );
                }
            }
            return ret;
        }

        public static void serialize( EditorConfig instance, String file ) {
            FileOutputStream fs = null;
            try {
                fs = new FileOutputStream( file );
                s_serializer.serialize( fs, instance );
            } catch ( Exception ex ) {
            } finally {
                if ( fs != null ) {
                    try {
                        fs.close();
                    } catch ( Exception ex2 ) {
                    }
                }
            }
        }

        public static EditorConfig deserialize( EditorConfig old_instance, String file ) {
            EditorConfig ret = null;
            FileInputStream fs = null;
            try {
                fs = new FileInputStream( file );
                ret = (EditorConfig)s_serializer.deserialize( fs );
            } catch ( Exception ex ) {
            } finally {
                if ( fs != null ) {
                    try {
                        fs.close();
                    } catch ( Exception ex2 ) {
                    }
                }
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

        public Font getBaseFont() {
            return new Font( BaseFontName, Font.PLAIN, (int)BaseFontSize );
        }

        public int getMouseHoverTime() {
            return m_mouse_hover_time;
        }

        public void setMouseHoverTime( int value ) {
            if ( value < 0 ) {
                m_mouse_hover_time = 0;
            } else if ( 2000 < m_mouse_hover_time ) {
                m_mouse_hover_time = 2000;
            } else {
                m_mouse_hover_time = value;
            }
        }

#if !JAVA
        // XMLシリアライズ用
        /// <summary>
        /// ピアノロール上でマウスホバーイベントが発生するまでの時間(millisec)
        /// </summary>
        public int MouseHoverTime {
            get {
                return getMouseHoverTime();
            }
            set {
                setMouseHoverTime( value );
            }
        }
#endif

        public QuantizeMode getPositionQuantize() {
            return m_position_quantize;
        }

        public void setPositionQuantize( QuantizeMode value ) {
            if ( m_position_quantize != value ) {
                m_position_quantize = value;
                try{
                    quantizeModeChangedEvent.raise( typeof( EditorConfig ), new BEventArgs() );
                }catch( Exception ex ){
                    PortUtil.stderr.println( "EditorConfig#setPositionQuantize; ex=" + ex );
                }
            }
        }

#if !JAVA
        // XMLシリアライズ用
        public QuantizeMode PositionQuantize {
            get {
                return getPositionQuantize();
            }
            set {
                setPositionQuantize( value );
            }
        }
#endif

        public boolean isPositionQuantizeTriplet() {
            return m_position_quantize_triplet;
        }

        public void setPositionQuantizeTriplet( boolean value ) {
            if ( m_position_quantize_triplet != value ) {
                m_position_quantize_triplet = value;
                try{
                    quantizeModeChangedEvent.raise( typeof( EditorConfig ), new BEventArgs() );
                }catch( Exception ex ){
                    PortUtil.stderr.println( "EditorConfig#setPositionQuantizeTriplet; ex=" + ex );
                }
            }
        }

#if !JAVA
        // XMLシリアライズ用
        public boolean PositionQuantizeTriplet {
            get {
                return isPositionQuantizeTriplet();
            }
            set {
                setPositionQuantizeTriplet( value );
            }
        }
#endif

        public QuantizeMode getLengthQuantize() {
            return m_length_quantize;
        }

        public void setLengthQuantize( QuantizeMode value ) {
            if ( m_length_quantize != value ) {
                m_length_quantize = value;
                try{
                    quantizeModeChangedEvent.raise( typeof( EditorConfig ), new BEventArgs() );
                }catch( Exception ex ){
                    PortUtil.stderr.println( "EditorConfig#setLengthQuantize; ex=" + ex );
                }
            }
        }

#if !JAVA
        public QuantizeMode LengthQuantize {
            get {
                return getLengthQuantize();
            }
            set {
                setLengthQuantize( value );
            }
        }
#endif

        public boolean isLengthQuantizeTriplet() {
            return m_length_quantize_triplet;
        }

        public void setLengthQuantizeTriplet( boolean value ) {
            if ( m_length_quantize_triplet != value ) {
                m_length_quantize_triplet = value;
                try{
                    quantizeModeChangedEvent.raise( typeof( EditorConfig ), new BEventArgs() );
                }catch( Exception ex ){
                    PortUtil.stderr.println( "EditorConfig#setLengthQuantizeTriplet; ex=" + ex );
                }
            }
        }

#if !JAVA
        // XMLシリアライズ用
        public boolean LengthQuantizeTriplet {
            get {
                return isLengthQuantizeTriplet();
            }
            set {
                setLengthQuantizeTriplet( value );
            }
        }
#endif

        /// <summary>
        /// 「最近使用したファイル」のリストに、アイテムを追加します
        /// </summary>
        /// <param name="new_file"></param>
        public void pushRecentFiles( String new_file ) {
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
            for ( Iterator itr = RecentFiles.iterator(); itr.hasNext(); ) {
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
            for ( Iterator itr = dict.iterator(); itr.hasNext(); ) {
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

#if !JAVA
}
#endif
