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
        /// <summary>
        /// デフォルトのプリメジャー．単位は小節
        /// </summary>
        public int DefaultPreMeasure = 4;
        /// <summary>
        /// デフォルトで使用する歌手の名前
        /// </summary>
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
        public int DefaultVibratoRate = 64;
        public int DefaultVibratoDepth = 64;
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
        /// ウィンドウが最大化された状態かどうか
        /// </summary>
        public boolean WindowMaximized = false;
        /// <summary>
        /// ウィンドウの位置とサイズ．
        /// 最小化された状態での値は，この変数に代入されない(ことになっている)
        /// </summary>
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
        public boolean MixerTopMost = true;
        public Vector<ValuePairOfStringArrayOfKeys> ShortcutKeys = new Vector<ValuePairOfStringArrayOfKeys>();
        public PropertyPanelState PropertyWindowStatus = new PropertyPanelState();
        /// <summary>
        /// 概観ペインが表示されているかどうか
        /// </summary>
        public boolean OverviewEnabled = false;
        public int OverviewScaleCount = 5;
        public FormMidiImExportConfig MidiImExportConfigExport = new FormMidiImExportConfig();
        public FormMidiImExportConfig MidiImExportConfigImport = new FormMidiImExportConfig();
        public FormMidiImExportConfig MidiImExportConfigImportVsq = new FormMidiImExportConfig();
        /// <summary>
        /// 自動バックアップする間隔．単位は分
        /// </summary>
        public int AutoBackupIntervalMinutes = 10;
        /// <summary>
        /// 鍵盤の表示幅、ピクセル,AppManager.keyWidthに代入。
        /// </summary>
        public int KeyWidth = 136;
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
        /// <summary>
        /// 鍵盤用のキャッシュが無いとき、FormGenerateKeySoundを表示しないかどうか。
        /// trueなら表示しない、falseなら表示する（デフォルト）
        /// </summary>
        public boolean DoNotAskKeySoundGeneration = false;
        /// <summary>
        /// VOCALOID1 (1.0)のDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        public boolean DoNotUseVocaloid100 = false;
        /// <summary>
        /// VOCALOID1 (1.1)のDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        public boolean DoNotUseVocaloid101 = false;
        /// <summary>
        /// VOCALOID2のDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        public boolean DoNotUseVocaloid2 = false;
        /// <summary>
        /// AquesToneのDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        public boolean DoNotUseAquesTone = false;
        /// <summary>
        /// 2個目のVOCALOID1 DLLを読み込むかどうか。既定ではfalse
        /// </summary>
        public boolean LoadSecondaryVocaloid1Dll = false;
        /// <summary>
        /// WAVE再生時のバッファーサイズ。既定では1000ms。
        /// </summary>
        public int BufferSizeMilliSeconds = 1000;
        /// <summary>
        /// トラックを新規作成するときのデフォルトの音声合成システム
        /// </summary>
        public RendererKind DefaultSynthesizer = RendererKind.VOCALOID2;
        /// <summary>
        /// 自動ビブラートを作成するとき，ユーザー定義タイプのビブラートを利用するかどうか．デフォルトではfalse
        /// </summary>
        public boolean UseUserDefinedAutoVibratoType = false;
        /// <summary>
        /// 再生中に画面を描画するかどうか。デフォルトはfalse
        /// <version>3.3+</version>
        /// </summary>
        public boolean SkipDrawWhilePlaying = false;
        /// <summary>
        /// ピアノロール画面の縦方向のスケール.
        /// <verssion>3.3+</verssion>
        /// </summary>
        public int PianoRollScaleY = 0;
        /// <summary>
        /// ファイル・ツールバーのサイズ
        /// <version>3.3+</version>
        /// </summary>
        public int BandSizeFile = 236;
        /// <summary>
        /// ツール・ツールバーのサイズ
        /// <version>3.3+</version>
        /// </summary>
        public int BandSizeTool = 712;
        /// <summary>
        /// メジャー・ツールバーのサイズ
        /// <version>3.3+</version>
        /// </summary>
        public int BandSizeMeasure = 714;
        /// <summary>
        /// ポジション・ツールバーのサイズ
        /// <version>3.3+</version>
        /// </summary>
        public int BandSizePosition = 234;
        /// <summary>
        /// ファイル・ツールバーを新しい行に追加するかどうか
        /// <version>3.3+</version>
        /// </summary>
        public boolean BandNewRowFile = false;
        /// <summary>
        /// ツール・ツールバーを新しい行に追加するかどうか
        /// <version>3.3+</version>
        /// </summary>
        public boolean BandNewRowTool = false;
        /// <summary>
        /// メジャー・ツールバーを新しい行に追加するかどうか
        /// <version>3.3+</version>
        /// </summary>
        public boolean BandNewRowMeasure = false;
        /// <summary>
        /// ポジション・ツールバーを新しい行に追加するかどうか
        /// <version>3.3+</version>
        /// </summary>
        public boolean BandNewRowPosition = true;
        /// <summary>
        /// ファイル・ツールバーの順番
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int BandOrderFile = 0;
        /// <summary>
        /// ツール・ツールバーの順番
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int BandOrderTool = 1;
        /// <summary>
        /// メジャー・ツールバーの順番
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int BandOrderMeasure = 3;
        /// <summary>
        /// ポジション・ツールバーの順番
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int BandOrderPosition = 2;
        /// <summary>
        /// ツールバーのChevronの幅．
        /// Winodws 7(Aero): 17px
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public int ChevronWidth = 17;
        /// <summary>
        /// 最後に入力したファイルパスのリスト
        /// リストに入る文字列は，拡張子+タブ文字+パスの形式にする
        /// 拡張子はピリオドを含めない
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public Vector<String> LastUsedPathIn = new Vector<String>();
        /// <summary>
        /// 最後に出力したファイルパスのリスト
        /// リストに入る文字列は，拡張子+タブ文字+パスの形式にする
        /// 拡張子はピリオドを含めない
        /// <remarks>version 3.3+</remarks>
        /// </summary>
        public Vector<String> LastUsedPathOut = new Vector<String>();

        /// <summary>
        /// バッファーサイズに設定できる最大値
        /// </summary>
        public const int MAX_BUFFER_MILLISEC = 1000;
        /// <summary>
        /// バッファーサイズに設定できる最小値
        /// </summary>
        public const int MIN_BUFFER_MILLIXEC = 100;
        public const int MAX_PIANOROLL_SCALEY = 10;
        public const int MIN_PIANOROLL_SCALEY = -4;

        #region static fields
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
            new ValuePairOfStringArrayOfKeys( "menuHiddenSelectBackward", new BKeys[]{ BKeys.Alt, BKeys.Left } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenSelectForward", new BKeys[]{ BKeys.Alt, BKeys.Right } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenMoveUp", new BKeys[]{ BKeys.Shift, BKeys.Up } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenMoveDown", new BKeys[]{ BKeys.Shift, BKeys.Down } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenMoveLeft", new BKeys[]{ BKeys.Shift, BKeys.Left } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenMoveRight", new BKeys[]{ BKeys.Shift, BKeys.Right } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenLengthen", new BKeys[]{ BKeys.Control, BKeys.Right } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenShorten", new BKeys[]{ BKeys.Control, BKeys.Left } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenGoToEndMarker", new BKeys[]{ BKeys.Control, BKeys.End } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenGoToStartMarker", new BKeys[]{ BKeys.Control, BKeys.Home } ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenPlayFromStartMarker", new BKeys[]{ BKeys.Control, BKeys.Enter } ),
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
            new ValuePairOfStringArrayOfKeys( "menuHelpAbout", new BKeys[]{} ),
            new ValuePairOfStringArrayOfKeys( "menuHiddenFlipCurveOnPianorollMode", new BKeys[]{ BKeys.Tab } ),
            new ValuePairOfStringArrayOfKeys( "menuVisualCircuitDiagram", new BKeys[]{} ),
        } ) );

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

        /// <summary>
        /// コンストラクタ．起動したOSによって動作を変える場合がある
        /// </summary>
        public EditorConfig() {
#if !JAVA
            // デフォルトのフォントを，システムのメニューフォントと同じにする
            System.Drawing.Font f = System.Windows.Forms.SystemInformation.MenuFont;
            if ( f != null ) {
                this.BaseFontName = f.Name;
                this.ScreenFontName = f.Name;
            }

            // 言語設定を，システムのデフォルトの言語を用いる
            String name = System.Windows.Forms.Application.CurrentCulture.Name;
            String lang = "";
            if ( name.Equals( "ja" ) ||
                 name.StartsWith( "ja-" ) ) {
                lang = "ja";
            } else {
                lang = name;
            }
            this.Language = lang;
#endif
        }

        #region public static method
        public static void serialize( EditorConfig instance, String file ) {
            FileOutputStream fs = null;
            try {
                fs = new FileOutputStream( file );
                s_serializer.serialize( fs, instance );
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

        public static EditorConfig deserialize( String file ) {
            EditorConfig ret = null;
            FileInputStream fs = null;
            try {
                fs = new FileInputStream( file );
                ret = (EditorConfig)s_serializer.deserialize( fs );
            } catch ( Exception ex ) {
#if JAVA
                PortUtil.stderr.println( "EditorConfig#deserialize; ex=" + ex );
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

            // バッファーサイズを正規化
            if ( ret.BufferSizeMilliSeconds < MIN_BUFFER_MILLIXEC ) {
                ret.BufferSizeMilliSeconds = MIN_BUFFER_MILLIXEC;
            } else if ( MAX_BUFFER_MILLISEC < ret.BufferSizeMilliSeconds ) {
                ret.BufferSizeMilliSeconds = MAX_BUFFER_MILLISEC;
            }
            return ret;
        }

        /// <summary>
        /// このクラスの指定した名前のプロパティが総称型引数を用いる型である場合に，
        /// その型の限定名を返します．それ以外の場合は空文字を返します．
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String getGenericTypeName( String name ) {
            if ( name != null ) {
                if ( name.Equals( "RecentFiles" ) ) {
                    return "java.lang.String";
                } else if ( name.Equals( "UserDictionaries" ) ) {
                    return "java.lang.String";
                } else if ( name.Equals( "Utausingers" ) ) {
                    return "org.kbinani.vsq.SingerConfig";
                } else if ( name.Equals( "ShortcutKeys" ) ) {
                    return "org.kbinani.cadencii.ValuePairOfStringArrayOfKeys";
                }
            }
            return "";
        }
        #endregion

        #region private static method
        private static String getLastUsedPathCore( Vector<String> list, String extension ) {
            if ( extension == null ) return "";
            if ( PortUtil.getStringLength( extension ) <= 0 ) return "";
            if ( extension.Equals( "." ) ) return "";

            if ( extension.StartsWith( "." ) ) {
                extension = extension.Substring( 1 );
            }

            int c = list.size();
            for ( int i = 0; i < c; i++ ) {
                String s = list.get( i );
                if ( s.StartsWith( extension ) ) {
                    String[] spl = PortUtil.splitString( s, '\t' );
                    if ( spl.Length >= 2 ) {
                        return spl[1];
                    }
                    break;
                }
            }
            return "";
        }

        private static void setLastUsedPathCore( Vector<String> list, String path ) {
            String extension = PortUtil.getExtension( path );
            if ( extension == null ) return;
            if ( extension.Equals( "." ) ) return;
            if ( extension.StartsWith( "." ) ) {
                extension = extension.Substring( 1 );
            }

            int c = list.size();
            String entry = extension + "\t" + path;
            for ( int i = 0; i < c; i++ ) {
                String s = list.get( i );
                if ( s.StartsWith( extension ) ) {
                    list.set( i, entry );
                    return;
                }
            }
            list.add( entry );
        }
        #endregion

        #region public method
        /// <summary>
        /// 最後に出力したファイルのパスのうち，拡張子が指定したものと同じであるものを取得します
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public String getLastUsedPathIn( String extension ) {
            String ret = getLastUsedPathCore( LastUsedPathIn, extension );
            if ( ret.Equals( "" ) ) {
                return getLastUsedPathCore( LastUsedPathOut, extension );
            }
            return ret;
        }

        /// <summary>
        /// 最後に出力したファイルのパスを設定します
        /// </summary>
        /// <param name="path"></param>
        public void setLastUsedPathIn( String path ) {
            setLastUsedPathCore( LastUsedPathIn, path );
        }

        /// <summary>
        /// 最後に入力したファイルのパスのうち，拡張子が指定したものと同じであるものを取得します．
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public String getLastUsedPathOut( String extension ) {
            String ret = getLastUsedPathCore( LastUsedPathOut, extension );
            if ( ret.Equals( "" ) ) {
                return getLastUsedPathCore( LastUsedPathIn, extension );
            }
            return ret;
        }

        /// <summary>
        /// 最後に入力したファイルのパスを設定します
        /// </summary>
        /// <param name="path"></param>
        public void setLastUsedPathOut( String path ) {
            setLastUsedPathCore( LastUsedPathOut, path );
        }

        /// <summary>
        /// 自動ビブラートを作成します
        /// </summary>
        /// <param name="type"></param>
        /// <param name="vibrato_clocks"></param>
        /// <returns></returns>
        public VibratoHandle createAutoVibrato( SynthesizerType type, int vibrato_clocks ) {
            if ( UseUserDefinedAutoVibratoType ) {
                VibratoHandle ret = new VibratoHandle();
                ret.IconID = "$04040001";
                ret.setStartDepth( DefaultVibratoDepth );
                ret.setStartRate( DefaultVibratoRate );
                ret.setLength( vibrato_clocks );
                return ret;
            } else {
                String iconid = type == SynthesizerType.VOCALOID1 ? AutoVibratoType1 : AutoVibratoType2;
                VibratoHandle ret = VocaloSysUtil.getDefaultVibratoHandle( iconid,
                                                                           vibrato_clocks,
                                                                           type );
                if ( ret == null ) {
                    ret = new VibratoHandle();
                    ret.IconID = "$04040001";
                    ret.setLength( vibrato_clocks );
                }
                return ret;
            }
        }

        public int getControlCurveResolutionValue() {
            return ClockResolutionUtility.getValue( ControlCurveResolution );
        }

        public BKeys[] getShortcutKeyFor( BMenuItem menu_item ) {
            String name = menu_item.getName();
            Vector<BKeys> ret = new Vector<BKeys>();
            for ( Iterator<ValuePairOfStringArrayOfKeys> itr = ShortcutKeys.iterator(); itr.hasNext(); ) {
                ValuePairOfStringArrayOfKeys item = itr.next();
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

        public TreeMap<String, BKeys[]> getShortcutKeysDictionary() {
            TreeMap<String, BKeys[]> ret = new TreeMap<String, BKeys[]>();
            for ( int i = 0; i < ShortcutKeys.size(); i++ ) {
                ret.put( ShortcutKeys.get( i ).Key, ShortcutKeys.get( i ).Value );
            }
            for ( Iterator<ValuePairOfStringArrayOfKeys> itr = DEFAULT_SHORTCUT_KEYS.iterator(); itr.hasNext(); ) {
                ValuePairOfStringArrayOfKeys item = itr.next();
                if ( !ret.containsKey( item.Key ) ) {
                    ret.put( item.Key, item.Value );
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
                try {
                    quantizeModeChangedEvent.raise( typeof( EditorConfig ), new BEventArgs() );
                } catch ( Exception ex ) {
                    Logger.write( typeof( EditorConfig ) + ".getPositionQuantize; ex=" + ex + "\n" );
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
                try {
                    quantizeModeChangedEvent.raise( typeof( EditorConfig ), new BEventArgs() );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "EditorConfig#setPositionQuantizeTriplet; ex=" + ex );
                    Logger.write( typeof( EditorConfig ) + ".setPositionQuantizeTriplet; ex=" + ex + "\n" );
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
                try {
                    quantizeModeChangedEvent.raise( typeof( EditorConfig ), new BEventArgs() );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "EditorConfig#setLengthQuantize; ex=" + ex );
                    Logger.write( typeof( EditorConfig ) + ".setLengthQuantize; ex=" + ex + "\n" );
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
                try {
                    quantizeModeChangedEvent.raise( typeof( EditorConfig ), new BEventArgs() );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "EditorConfig#setLengthQuantizeTriplet; ex=" + ex );
                    Logger.write( typeof( EditorConfig ) + ".setLengthQuantizeTriplet; ex=" + ex + "\n" );
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
            for ( Iterator<String> itr = RecentFiles.iterator(); itr.hasNext(); ) {
                String s = itr.next();
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
            for ( Iterator<String> itr = dict.iterator(); itr.hasNext(); ) {
                String s = itr.next();
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
        #endregion
    }

#if !JAVA
}
#endif
