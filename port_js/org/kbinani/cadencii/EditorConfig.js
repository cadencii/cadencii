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
if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.cadencii == undefined ) org.kbinani.cadencii = {};
if( org.kbinani.cadencii.EditorConfig == undefined ){

    /// <summary>
    /// Cadenciiの環境設定
    /// </summary>
    org.kbinani.cadencii.EditorConfig = function(){
        org.kbinani.cadencii.EditorConfig.DefaultPreMeasure = 4;
        org.kbinani.cadencii.EditorConfig.DefaultSingerName = "Miku";
        org.kbinani.cadencii.EditorConfig.DefaultXScale = 65;
        org.kbinani.cadencii.EditorConfig.BaseFontName = "MS UI Gothic";
        org.kbinani.cadencii.EditorConfig.BaseFontSize = 9.0;
        org.kbinani.cadencii.EditorConfig.ScreenFontName = "MS UI Gothic";
        org.kbinani.cadencii.EditorConfig.WheelOrder = 20;
        org.kbinani.cadencii.EditorConfig.CursorFixed = false;
        /// <summary>
        /// RecentFilesに登録することの出来る最大のファイル数
        /// </summary>
        org.kbinani.cadencii.EditorConfig.NumRecentFiles = 5;
        /// <summary>
        /// 最近使用したファイルのリスト
        /// </summary>
        org.kbinani.cadencii.EditorConfig.RecentFiles = new Array();
        org.kbinani.cadencii.EditorConfig.DefaultPMBendDepth = 8;
        org.kbinani.cadencii.EditorConfig.DefaultPMBendLength = 0;
        org.kbinani.cadencii.EditorConfig.DefaultPMbPortamentoUse = 3;
        org.kbinani.cadencii.EditorConfig.DefaultDEMdecGainRate = 50;
        org.kbinani.cadencii.EditorConfig.DefaultDEMaccent = 50;
        org.kbinani.cadencii.EditorConfig.ShowLyric = true;
        org.kbinani.cadencii.EditorConfig.ShowExpLine = true;
        /*public DefaultVibratoLengthEnum DefaultVibratoLength = DefaultVibratoLengthEnum.L66;*/
        org.kbinani.cadencii.EditorConfig.DefaultVibratoRate = 64;
        org.kbinani.cadencii.EditorConfig.DefaultVibratoDepth = 64;
        /*public AutoVibratoMinLengthEnum AutoVibratoMinimumLength = AutoVibratoMinLengthEnum.L1;*/
        org.kbinani.cadencii.EditorConfig.AutoVibratoType1 = "$04040001";
        org.kbinani.cadencii.EditorConfig.AutoVibratoType2 = "$04040001";
        org.kbinani.cadencii.EditorConfig.EnableAutoVibrato = true;
        org.kbinani.cadencii.EditorConfig.PxTrackHeight = 14;
        org.kbinani.cadencii.EditorConfig.MouseDragIncrement = 50;
        org.kbinani.cadencii.EditorConfig.MouseDragMaximumRate = 600;
        org.kbinani.cadencii.EditorConfig.MixerVisible = false;
        org.kbinani.cadencii.EditorConfig.PreSendTime = 500;
        /*public ClockResolution ControlCurveResolution = ClockResolution.L30;*/
        org.kbinani.cadencii.EditorConfig.Language = "";
        /// <summary>
        /// マウスの操作などの許容範囲。プリメジャーにPxToleranceピクセルめり込んだ入力を行っても、エラーにならない。(補正はされる)
        /// </summary>
        org.kbinani.cadencii.EditorConfig.PxTolerance = 10;
        /// <summary>
        /// マウスホイールでピアノロールを水平方向にスクロールするかどうか。
        /// </summary>
        org.kbinani.cadencii.EditorConfig.ScrollHorizontalOnWheel = true;
        /// <summary>
        /// 画面描画の最大フレームレート
        /// </summary>
        org.kbinani.cadencii.EditorConfig.MaximumFrameRate = 15;
        /// <summary>
        /// ユーザー辞書のOn/Offと順序
        /// </summary>
        org.kbinani.cadencii.EditorConfig.UserDictionaries = new Array();
        /// <summary>
        /// 実行環境
        /// </summary>
        /*public PlatformEnum Platform = PlatformEnum.Windows;*/
        /// <summary>
        /// toolStripToolの表示位置
        /// </summary>
        /*public ToolStripLocation ToolEditTool = new ToolStripLocation( new Point( 3, 0 ), ToolStripLocation.ParentPanel.Top );*/
        /*
        /// <summary>
        /// toolStripPositionの表示位置
        /// </summary>
        public ToolStripLocation ToolPositionLocation = new ToolStripLocation( new Point( 3, 25 ), ToolStripLocation.ParentPanel.Top );*/
        /*
        /// <summary>
        /// toolStripMeasureの表示位置
        /// </summary>
        public ToolStripLocation ToolMeasureLocation = new ToolStripLocation( new Point( 212, 25 ), ToolStripLocation.ParentPanel.Top );*/
        /*public ToolStripLocation ToolFileLocation = new ToolStripLocation( new Point( 461, 0 ), ToolStripLocation.ParentPanel.Top );*/
        org.kbinani.cadencii.EditorConfig.WindowMaximized = false;
        /*public Rectangle WindowRect = new Rectangle( 0, 0, 970, 718 );*/
        /// <summary>
        /// hScrollのスクロールボックスの最小幅(px)
        /// </summary>
        org.kbinani.cadencii.EditorConfig.MinimumScrollHandleWidth = 20;
        /// <summary>
        /// 発音記号入力モードを，維持するかどうか
        /// </summary>
        org.kbinani.cadencii.EditorConfig.KeepLyricInputMode = false;
        /// <summary>
        /// 最後に使用したVSQファイルへのパス
        /// </summary>
        org.kbinani.cadencii.EditorConfig.LastVsqPath = "";
        /// <summary>
        /// ピアノロールの何もないところをクリックした場合、右クリックでもプレビュー音を再生するかどうか
        /// </summary>
        org.kbinani.cadencii.EditorConfig.PlayPreviewWhenRightClick = false;
        /// <summary>
        /// ゲームコントローラで、異なるイベントと識別する最小の時間間隔(millisec)
        /// </summary>
        org.kbinani.cadencii.EditorConfig.GameControlerMinimumEventInterval = 100;
        /// <summary>
        /// カーブの選択範囲もクオンタイズするかどうか
        /// </summary>
        org.kbinani.cadencii.EditorConfig.CurveSelectingQuantized = true;

        /*private QuantizeMode m_position_quantize = QuantizeMode.p32;
        private boolean m_position_quantize_triplet = false;
        private QuantizeMode m_length_quantize = QuantizeMode.p32;
        private boolean m_length_quantize_triplet = false;
        private int m_mouse_hover_time = 500;*/
        /// <summary>
        /// Button index of "△"
        /// </summary>
        org.kbinani.cadencii.EditorConfig.GameControlerTriangle = 0;
        /// <summary>
        /// Button index of "○"
        /// </summary>
        org.kbinani.cadencii.EditorConfig.GameControlerCircle = 1;
        /// <summary>
        /// Button index of "×"
        /// </summary>
        org.kbinani.cadencii.EditorConfig.GameControlerCross = 2;
        /// <summary>
        /// Button index of "□"
        /// </summary>
        org.kbinani.cadencii.EditorConfig.GameControlerRectangle = 3;
        /// <summary>
        /// Button index of "L1"
        /// </summary>
        org.kbinani.cadencii.EditorConfig.GameControlL1 = 4;
        /// <summary>
        /// Button index of "R1"
        /// </summary>
        org.kbinani.cadencii.EditorConfig.GameControlR1 = 5;
        /// <summary>
        /// Button index of "L2"
        /// </summary>
        org.kbinani.cadencii.EditorConfig.GameControlL2 = 6;
        /// <summary>
        /// Button index of "R2"
        /// </summary>
        org.kbinani.cadencii.EditorConfig.GameControlR2 = 7;
        /// <summary>
        /// Button index of "SELECT"
        /// </summary>
        org.kbinani.cadencii.EditorConfig.GameControlSelect = 8;
        /// <summary>
        /// Button index of "START"
        /// </summary>
        org.kbinani.cadencii.EditorConfig.GameControlStart = 9;
        /// <summary>
        /// Button index of Left Stick
        /// </summary>
        org.kbinani.cadencii.EditorConfig.GameControlStirckL = 10;
        /// <summary>
        /// Button index of Right Stick
        /// </summary>
        org.kbinani.cadencii.EditorConfig.GameControlStirckR = 11;
        org.kbinani.cadencii.EditorConfig.CurveVisibleVelocity = true;
        org.kbinani.cadencii.EditorConfig.CurveVisibleAccent = true;
        org.kbinani.cadencii.EditorConfig.CurveVisibleDecay = true;
        org.kbinani.cadencii.EditorConfig.CurveVisibleVibratoRate = true;
        org.kbinani.cadencii.EditorConfig.CurveVisibleVibratoDepth = true;
        org.kbinani.cadencii.EditorConfig.CurveVisibleDynamics = true;
        org.kbinani.cadencii.EditorConfig.CurveVisibleBreathiness = true;
        org.kbinani.cadencii.EditorConfig.CurveVisibleBrightness = true;
        org.kbinani.cadencii.EditorConfig.CurveVisibleClearness = true;
        org.kbinani.cadencii.EditorConfig.CurveVisibleOpening = true;
        org.kbinani.cadencii.EditorConfig.CurveVisibleGendorfactor = true;
        org.kbinani.cadencii.EditorConfig.CurveVisiblePortamento = true;
        org.kbinani.cadencii.EditorConfig.CurveVisiblePit = true;
        org.kbinani.cadencii.EditorConfig.CurveVisiblePbs = true;
        org.kbinani.cadencii.EditorConfig.CurveVisibleHarmonics = false;
        org.kbinani.cadencii.EditorConfig.CurveVisibleFx2Depth = false;
        org.kbinani.cadencii.EditorConfig.CurveVisibleReso1 = false;
        org.kbinani.cadencii.EditorConfig.CurveVisibleReso2 = false;
        org.kbinani.cadencii.EditorConfig.CurveVisibleReso3 = false;
        org.kbinani.cadencii.EditorConfig.CurveVisibleReso4 = false;
        org.kbinani.cadencii.EditorConfig.CurveVisibleEnvelope = false;
        org.kbinani.cadencii.EditorConfig.GameControlPovRight = 9000;
        org.kbinani.cadencii.EditorConfig.GameControlPovLeft = 27000;
        org.kbinani.cadencii.EditorConfig.GameControlPovUp = 0;
        org.kbinani.cadencii.EditorConfig.GameControlPovDown = 18000;
        /// <summary>
        /// wave波形を表示するかどうか
        /// </summary>
        org.kbinani.cadencii.EditorConfig.ViewWaveform = false;
        /*
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
        public RgbColor PianorollColorAquesToneBeat = new RgbColor( 234, 190, 62 );*/

        org.kbinani.cadencii.EditorConfig.ViewAtcualPitch = false;
        org.kbinani.cadencii.EditorConfig.InvokeUtauCoreWithWine = false;
        /*public Vector<SingerConfig> UtauSingers = new Vector<SingerConfig>();*/
        org.kbinani.cadencii.EditorConfig.PathResampler = "";
        org.kbinani.cadencii.EditorConfig.PathWavtool = "";
        /// <summary>
        /// ベジエ制御点を掴む時の，掴んだと判定する際の誤差．制御点の外輪からPxToleranceBezierピクセルずれていても，掴んだと判定する．
        /// </summary>
        org.kbinani.cadencii.EditorConfig.PxToleranceBezier = 10;
        /// <summary>
        /// 歌詞入力においてローマ字が入力されたとき，Cadencii側でひらがなに変換するかどうか
        /// </summary>
        org.kbinani.cadencii.EditorConfig.SelfDeRomanization = false;
        /// <summary>
        /// openMidiDialogで最後に選択された拡張子
        /// </summary>
        org.kbinani.cadencii.EditorConfig.LastUsedExtension = ".vsq";
        /// <summary>
        /// ミキサーダイアログを常に手前に表示するかどうか
        /// </summary>
        org.kbinani.cadencii.EditorConfig.MixerTopMost = false;
        /*public Vector<ValuePairOfStringArrayOfKeys> ShortcutKeys = new Vector<ValuePairOfStringArrayOfKeys>();
        /// <summary>
        /// リアルタイム再生時の再生速度
        /// </summary>
        private float m_realtime_input_speed = 1.0f;
        public byte MidiProgramNormal = 115;
        public byte MidiProgramBell = 9;
        public byte MidiNoteNormal = 65;
        public byte MidiNoteBell = 65;*/
        org.kbinani.cadencii.EditorConfig.MidiRingBell = true;
        org.kbinani.cadencii.EditorConfig.MidiPreUtterance = 0;
        /*public MidiPortConfig MidiDeviceMetronome = new MidiPortConfig();
        public MidiPortConfig MidiDeviceGeneral = new MidiPortConfig();*/
        org.kbinani.cadencii.EditorConfig.MetronomeEnabled = true;
        /*public PropertyPanelState PropertyWindowStatus = new PropertyPanelState();*/
        /// <summary>
        /// 概観ペインが表示されているかどうか
        /// </summary>
        org.kbinani.cadencii.EditorConfig.OverviewEnabled = false;
        org.kbinani.cadencii.EditorConfig.OverviewScaleCount = 5;
        /*public FormMidiImExportConfig MidiImExportConfigExport = new FormMidiImExportConfig();
        public FormMidiImExportConfig MidiImExportConfigImport = new FormMidiImExportConfig();
        public FormMidiImExportConfig MidiImExportConfigImportVsq = new FormMidiImExportConfig();*/
        org.kbinani.cadencii.EditorConfig.AutoBackupIntervalMinutes = 10;
        /// <summary>
        /// 鍵盤の表示幅、ピクセル,AppManager.keyWidthに代入。
        /// </summary>
        org.kbinani.cadencii.EditorConfig.KeyWidth = 68;
        /// <summary>
        /// スペースキーを押しながら左クリックで、中ボタンクリックとみなす動作をさせるかどうか。
        /// </summary>
        org.kbinani.cadencii.EditorConfig.UseSpaceKeyAsMiddleButtonModifier = false;
        /// <summary>
        /// AquesToneのVSTi dllへのパス
        /// </summary>
        org.kbinani.cadencii.EditorConfig.PathAquesTone = "";
        /*
        /// <summary>
        /// アイコンパレット・ウィンドウの位置
        /// </summary>
        public XmlPoint FormIconPaletteLocation = new XmlPoint( 0, 0 );*/
        /// <summary>
        /// アイコンパレット・ウィンドウを常に手前に表示するかどうか
        /// </summary>
        org.kbinani.cadencii.EditorConfig.FormIconTopMost = true;
        /// <summary>
        /// 前回エクスポートしたMusicXmlのパス
        /// </summary>
        org.kbinani.cadencii.EditorConfig.LastMusicXmlPath = "";
        /*
        /// <summary>
        /// 最初に戻る、のショートカットキー
        /// </summary>
        public BKeys[] SpecialShortcutGoToFirst = new BKeys[] { BKeys.Home };*/
        /// <summary>
        /// waveファイル出力時のチャンネル数（1または2）
        /// </summary>
        org.kbinani.cadencii.EditorConfig.WaveFileOutputChannel = 2;
        /// <summary>
        /// waveファイル出力時に、全トラックをmixして出力するかどうか
        /// </summary>
        org.kbinani.cadencii.EditorConfig.WaveFileOutputFromMasterTrack = false;
        /*
        /// <summary>
        /// MTCスレーブ動作を行う際使用するMIDI INポートの設定
        /// </summary>
        public MidiPortConfig MidiInPortMtc = new MidiPortConfig();*/
        /// <summary>
        /// プロジェクトごとのキャッシュディレクトリを使うかどうか
        /// </summary>
        org.kbinani.cadencii.EditorConfig.UseProjectCache = true;
        /// <summary>
        /// 鍵盤用のキャッシュが無いとき、FormGenerateKeySoundを表示しないかどうか。
        /// trueなら表示しない、falseなら表示する（デフォルト）
        /// </summary>
        org.kbinani.cadencii.EditorConfig.DoNotAskKeySoundGeneration = false;
        /// <summary>
        /// VOCALOID1 (1.0)のDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        org.kbinani.cadencii.EditorConfig.DoNotUseVocaloid100 = false;
        /// <summary>
        /// VOCALOID1 (1.1)のDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        org.kbinani.cadencii.EditorConfig.DoNotUseVocaloid101 = false;
        /// <summary>
        /// VOCALOID2のDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        org.kbinani.cadencii.EditorConfig.DoNotUseVocaloid2 = false;
        /// <summary>
        /// AquesToneのDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        org.kbinani.cadencii.EditorConfig.DoNotUseAquesTone = false;
        /// <summary>
        /// 2個目のVOCALOID1 DLLを読み込むかどうか。既定ではfalse
        /// </summary>
        org.kbinani.cadencii.EditorConfig.LoadSecondaryVocaloid1Dll = false;
        /// <summary>
        /// WAVE再生時のバッファーサイズ。既定では1000ms。
        /// </summary>
        org.kbinani.cadencii.EditorConfig.BufferSizeMilliSeconds = 1000;
        /*
        /// <summary>
        /// トラックを新規作成するときのデフォルトの音声合成システム
        /// </summary>
        public RendererKind DefaultSynthesizer = RendererKind.VOCALOID2;*/
        /// <summary>
        /// 自動ビブラートを作成するとき，ユーザー定義タイプのビブラートを利用するかどうか．デフォルトではfalse
        /// </summary>
        org.kbinani.cadencii.EditorConfig.UseUserDefinedAutoVibratoType = false;
    };

    /*
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
            new ValuePairOfStringArrayOfKeys( "menuHiddenFlipCurveOnPianorollMode", new BKeys[]{ BKeys.Tab } ) } ) );
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
            for ( Iterator<ValuePairOfStringArrayOfKeys> itr = DEFAULT_SHORTCUT_KEYS.iterator(); itr.hasNext(); ) {
                ValuePairOfStringArrayOfKeys item = itr.next();
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

        public static EditorConfig deserialize( String file ) {
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
    }

#if !JAVA
}
#endif*/
}
