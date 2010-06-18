/*
 * EditorConfig.js
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
        this.DefaultPreMeasure = 4;
        this.DefaultSingerName = "Miku";
        this.DefaultXScale = 65;
        this.BaseFontName = "MS UI Gothic";
        this.BaseFontSize = 9.0;
        this.ScreenFontName = "MS UI Gothic";
        this.WheelOrder = 20;
        this.CursorFixed = false;
        /// <summary>
        /// RecentFilesに登録することの出来る最大のファイル数
        /// </summary>
        this.NumRecentFiles = 5;
        /// <summary>
        /// 最近使用したファイルのリスト
        /// </summary>
        this.RecentFiles = new Array();
        this.DefaultPMBendDepth = 8;
        this.DefaultPMBendLength = 0;
        this.DefaultPMbPortamentoUse = 3;
        this.DefaultDEMdecGainRate = 50;
        this.DefaultDEMaccent = 50;
        this.ShowLyric = true;
        this.ShowExpLine = true;
        /*public DefaultVibratoLengthEnum DefaultVibratoLength = DefaultVibratoLengthEnum.L66;*/
        this.DefaultVibratoRate = 64;
        this.DefaultVibratoDepth = 64;
        /*public AutoVibratoMinLengthEnum AutoVibratoMinimumLength = AutoVibratoMinLengthEnum.L1;*/
        this.AutoVibratoType1 = "$04040001";
        this.AutoVibratoType2 = "$04040001";
        this.EnableAutoVibrato = true;
        this.PxTrackHeight = 14;
        this.MouseDragIncrement = 50;
        this.MouseDragMaximumRate = 600;
        this.MixerVisible = false;
        this.PreSendTime = 500;
        /*public ClockResolution ControlCurveResolution = ClockResolution.L30;*/
        this.Language = "";
        /// <summary>
        /// マウスの操作などの許容範囲。プリメジャーにPxToleranceピクセルめり込んだ入力を行っても、エラーにならない。(補正はされる)
        /// </summary>
        this.PxTolerance = 10;
        /// <summary>
        /// マウスホイールでピアノロールを水平方向にスクロールするかどうか。
        /// </summary>
        this.ScrollHorizontalOnWheel = true;
        /// <summary>
        /// 画面描画の最大フレームレート
        /// </summary>
        this.MaximumFrameRate = 15;
        /// <summary>
        /// ユーザー辞書のOn/Offと順序
        /// </summary>
        this.UserDictionaries = new Array();
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
        this.WindowMaximized = false;
        /*public Rectangle WindowRect = new Rectangle( 0, 0, 970, 718 );*/
        /// <summary>
        /// hScrollのスクロールボックスの最小幅(px)
        /// </summary>
        this.MinimumScrollHandleWidth = 20;
        /// <summary>
        /// 発音記号入力モードを，維持するかどうか
        /// </summary>
        this.KeepLyricInputMode = false;
        /// <summary>
        /// 最後に使用したVSQファイルへのパス
        /// </summary>
        this.LastVsqPath = "";
        /// <summary>
        /// ピアノロールの何もないところをクリックした場合、右クリックでもプレビュー音を再生するかどうか
        /// </summary>
        this.PlayPreviewWhenRightClick = false;
        /// <summary>
        /// ゲームコントローラで、異なるイベントと識別する最小の時間間隔(millisec)
        /// </summary>
        this.GameControlerMinimumEventInterval = 100;
        /// <summary>
        /// カーブの選択範囲もクオンタイズするかどうか
        /// </summary>
        this.CurveSelectingQuantized = true;

        /**
         * [QuantizeMode]
         */
        if( org.kbinani.cadencii.QuantizeMode == undefined ){
            alert( "EditorConfig; QuantizeMode.js should be load before EditorConfig.js" );
        }
        this._m_position_quantize = org.kbinani.cadencii.QuantizeMode.p32;
        this._m_position_quantize_triplet = false;
        /**
         * [QuantizeMode]
         */
        this._m_length_quantize = org.kbinani.cadencii.QuantizeMode.p32;
        this._m_length_quantize_triplet = false;
        this._m_mouse_hover_time = 500;
        /// <summary>
        /// Button index of "△"
        /// </summary>
        this.GameControlerTriangle = 0;
        /// <summary>
        /// Button index of "○"
        /// </summary>
        this.GameControlerCircle = 1;
        /// <summary>
        /// Button index of "×"
        /// </summary>
        this.GameControlerCross = 2;
        /// <summary>
        /// Button index of "□"
        /// </summary>
        this.GameControlerRectangle = 3;
        /// <summary>
        /// Button index of "L1"
        /// </summary>
        this.GameControlL1 = 4;
        /// <summary>
        /// Button index of "R1"
        /// </summary>
        this.GameControlR1 = 5;
        /// <summary>
        /// Button index of "L2"
        /// </summary>
        this.GameControlL2 = 6;
        /// <summary>
        /// Button index of "R2"
        /// </summary>
        this.GameControlR2 = 7;
        /// <summary>
        /// Button index of "SELECT"
        /// </summary>
        this.GameControlSelect = 8;
        /// <summary>
        /// Button index of "START"
        /// </summary>
        this.GameControlStart = 9;
        /// <summary>
        /// Button index of Left Stick
        /// </summary>
        this.GameControlStirckL = 10;
        /// <summary>
        /// Button index of Right Stick
        /// </summary>
        this.GameControlStirckR = 11;
        this.CurveVisibleVelocity = true;
        this.CurveVisibleAccent = true;
        this.CurveVisibleDecay = true;
        this.CurveVisibleVibratoRate = true;
        this.CurveVisibleVibratoDepth = true;
        this.CurveVisibleDynamics = true;
        this.CurveVisibleBreathiness = true;
        this.CurveVisibleBrightness = true;
        this.CurveVisibleClearness = true;
        this.CurveVisibleOpening = true;
        this.CurveVisibleGendorfactor = true;
        this.CurveVisiblePortamento = true;
        this.CurveVisiblePit = true;
        this.CurveVisiblePbs = true;
        this.CurveVisibleHarmonics = false;
        this.CurveVisibleFx2Depth = false;
        this.CurveVisibleReso1 = false;
        this.CurveVisibleReso2 = false;
        this.CurveVisibleReso3 = false;
        this.CurveVisibleReso4 = false;
        this.CurveVisibleEnvelope = false;
        this.GameControlPovRight = 9000;
        this.GameControlPovLeft = 27000;
        this.GameControlPovUp = 0;
        this.GameControlPovDown = 18000;
        /// <summary>
        /// wave波形を表示するかどうか
        /// </summary>
        this.ViewWaveform = false;
        /*
        /// <summary>
        /// キーボードからの入力に使用するデバイス
        /// </summary>
        public MidiPortConfig MidiInPort = new MidiPortConfig();*/
        this.PianorollColorVocalo2Black = new org.kbinani.cadencii.RgbColor( 212, 212, 212 );
        this.PianorollColorVocalo2White = new org.kbinani.cadencii.RgbColor( 240, 240, 240 );
        this.PianorollColorVocalo1Black = new org.kbinani.cadencii.RgbColor( 210, 205, 172 );
        this.PianorollColorVocalo1White = new org.kbinani.cadencii.RgbColor( 240, 235, 214 );

        this.PianorollColorVocalo1Bar = new org.kbinani.cadencii.RgbColor( 161, 157, 136 );
        this.PianorollColorVocalo1Beat = new org.kbinani.cadencii.RgbColor( 209, 204, 172 );
        this.PianorollColorVocalo2Bar = new org.kbinani.cadencii.RgbColor( 161, 157, 136 );
        this.PianorollColorVocalo2Beat = new org.kbinani.cadencii.RgbColor( 209, 204, 172 );

        this.PianorollColorUtauBlack = new org.kbinani.cadencii.RgbColor( 212, 212, 212 );
        this.PianorollColorUtauWhite = new org.kbinani.cadencii.RgbColor( 240, 240, 240 );
        this.PianorollColorUtauBar = new org.kbinani.cadencii.RgbColor( 255, 64, 255 );
        this.PianorollColorUtauBeat = new org.kbinani.cadencii.RgbColor( 128, 128, 255 );

        this.PianorollColorStraightBlack = new org.kbinani.cadencii.RgbColor( 212, 212, 212 );
        this.PianorollColorStraightWhite = new org.kbinani.cadencii.RgbColor( 240, 240, 240 );
        this.PianorollColorStraightBar = new org.kbinani.cadencii.RgbColor( 255, 153, 0 );
        this.PianorollColorStraightBeat = new org.kbinani.cadencii.RgbColor( 128, 128, 255 );

        this.PianorollColorAquesToneBlack = new org.kbinani.cadencii.RgbColor( 212, 212, 212 );
        this.PianorollColorAquesToneWhite = new org.kbinani.cadencii.RgbColor( 240, 240, 240 );
        this.PianorollColorAquesToneBar = new org.kbinani.cadencii.RgbColor( 7, 107, 175 );
        this.PianorollColorAquesToneBeat = new org.kbinani.cadencii.RgbColor( 234, 190, 62 );

        this.ViewAtcualPitch = false;
        this.InvokeUtauCoreWithWine = false;
        /*public Vector<SingerConfig> UtauSingers = new Vector<SingerConfig>();*/
        this.PathResampler = "";
        this.PathWavtool = "";
        /// <summary>
        /// ベジエ制御点を掴む時の，掴んだと判定する際の誤差．制御点の外輪からPxToleranceBezierピクセルずれていても，掴んだと判定する．
        /// </summary>
        this.PxToleranceBezier = 10;
        /// <summary>
        /// 歌詞入力においてローマ字が入力されたとき，Cadencii側でひらがなに変換するかどうか
        /// </summary>
        this.SelfDeRomanization = false;
        /// <summary>
        /// openMidiDialogで最後に選択された拡張子
        /// </summary>
        this.LastUsedExtension = ".vsq";
        /// <summary>
        /// ミキサーダイアログを常に手前に表示するかどうか
        /// </summary>
        this.MixerTopMost = false;
        /*public Vector<ValuePairOfStringArrayOfKeys> ShortcutKeys = new Vector<ValuePairOfStringArrayOfKeys>();
        /// <summary>
        /// リアルタイム再生時の再生速度
        /// </summary>
        private float m_realtime_input_speed = 1.0f;
        public byte MidiProgramNormal = 115;
        public byte MidiProgramBell = 9;
        public byte MidiNoteNormal = 65;
        public byte MidiNoteBell = 65;*/
        this.MidiRingBell = true;
        this.MidiPreUtterance = 0;
        /*public MidiPortConfig MidiDeviceMetronome = new MidiPortConfig();
        public MidiPortConfig MidiDeviceGeneral = new MidiPortConfig();*/
        this.MetronomeEnabled = true;
        /*public PropertyPanelState PropertyWindowStatus = new PropertyPanelState();*/
        /// <summary>
        /// 概観ペインが表示されているかどうか
        /// </summary>
        this.OverviewEnabled = false;
        this.OverviewScaleCount = 5;
        /*public FormMidiImExportConfig MidiImExportConfigExport = new FormMidiImExportConfig();
        public FormMidiImExportConfig MidiImExportConfigImport = new FormMidiImExportConfig();
        public FormMidiImExportConfig MidiImExportConfigImportVsq = new FormMidiImExportConfig();*/
        this.AutoBackupIntervalMinutes = 10;
        /// <summary>
        /// 鍵盤の表示幅、ピクセル,AppManager.keyWidthに代入。
        /// </summary>
        this.KeyWidth = 68;
        /// <summary>
        /// スペースキーを押しながら左クリックで、中ボタンクリックとみなす動作をさせるかどうか。
        /// </summary>
        this.UseSpaceKeyAsMiddleButtonModifier = false;
        /// <summary>
        /// AquesToneのVSTi dllへのパス
        /// </summary>
        this.PathAquesTone = "";
        /*
        /// <summary>
        /// アイコンパレット・ウィンドウの位置
        /// </summary>
        public XmlPoint FormIconPaletteLocation = new XmlPoint( 0, 0 );*/
        /// <summary>
        /// アイコンパレット・ウィンドウを常に手前に表示するかどうか
        /// </summary>
        this.FormIconTopMost = true;
        /// <summary>
        /// 前回エクスポートしたMusicXmlのパス
        /// </summary>
        this.LastMusicXmlPath = "";
        /*
        /// <summary>
        /// 最初に戻る、のショートカットキー
        /// </summary>
        public BKeys[] SpecialShortcutGoToFirst = new BKeys[] { BKeys.Home };*/
        /// <summary>
        /// waveファイル出力時のチャンネル数（1または2）
        /// </summary>
        this.WaveFileOutputChannel = 2;
        /// <summary>
        /// waveファイル出力時に、全トラックをmixして出力するかどうか
        /// </summary>
        this.WaveFileOutputFromMasterTrack = false;
        /*
        /// <summary>
        /// MTCスレーブ動作を行う際使用するMIDI INポートの設定
        /// </summary>
        public MidiPortConfig MidiInPortMtc = new MidiPortConfig();*/
        /// <summary>
        /// プロジェクトごとのキャッシュディレクトリを使うかどうか
        /// </summary>
        this.UseProjectCache = true;
        /// <summary>
        /// 鍵盤用のキャッシュが無いとき、FormGenerateKeySoundを表示しないかどうか。
        /// trueなら表示しない、falseなら表示する（デフォルト）
        /// </summary>
        this.DoNotAskKeySoundGeneration = false;
        /// <summary>
        /// VOCALOID1 (1.0)のDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        this.DoNotUseVocaloid100 = false;
        /// <summary>
        /// VOCALOID1 (1.1)のDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        this.DoNotUseVocaloid101 = false;
        /// <summary>
        /// VOCALOID2のDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        this.DoNotUseVocaloid2 = false;
        /// <summary>
        /// AquesToneのDLLを読み込まない場合true。既定ではfalse
        /// </summary>
        this.DoNotUseAquesTone = false;
        /// <summary>
        /// 2個目のVOCALOID1 DLLを読み込むかどうか。既定ではfalse
        /// </summary>
        this.LoadSecondaryVocaloid1Dll = false;
        /// <summary>
        /// WAVE再生時のバッファーサイズ。既定では1000ms。
        /// </summary>
        this.BufferSizeMilliSeconds = 1000;
        /*
        /// <summary>
        /// トラックを新規作成するときのデフォルトの音声合成システム
        /// </summary>
        public RendererKind DefaultSynthesizer = RendererKind.VOCALOID2;*/
        /// <summary>
        /// 自動ビブラートを作成するとき，ユーザー定義タイプのビブラートを利用するかどうか．デフォルトではfalse
        /// </summary>
        this.UseUserDefinedAutoVibratoType = false;
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
*/

    org.kbinani.cadencii.EditorConfig.prototype = {
/*
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
        }*/

        /**
         * @return [QuantizeMode]
         */
        getPositionQuantize : function() {
            return this._m_position_quantize;
        },

        /**
         * @param value [QuantizeMode]
         */
        setPositionQuantize : function( value ) {
            if ( this._m_position_quantize != value ) {
                this._m_position_quantize = value;
                /*try {
                    quantizeModeChangedEvent.raise( typeof( EditorConfig ), new BEventArgs() );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "EditorConfig#setPositionQuantize; ex=" + ex );
                }*/
            }
        },

        /**
         * @return [bool]
         */
        isPositionQuantizeTriplet : function() {
            return this._m_position_quantize_triplet;
        },

        /**
         * @param value [bool]
         */
        setPositionQuantizeTriplet : function( value ) {
            if ( this._m_position_quantize_triplet != value ) {
                this._m_position_quantize_triplet = value;
                /*try {
                    quantizeModeChangedEvent.raise( typeof( EditorConfig ), new BEventArgs() );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "EditorConfig#setPositionQuantizeTriplet; ex=" + ex );
                }*/
            }
        },

        /**
         * @return [QuantizeMode]
         */
        getLengthQuantize : function() {
            return this._m_length_quantize;
        },

        /**
         * @param value [QuantizeMode]
         */
        setLengthQuantize : function( value ) {
            if ( this._m_length_quantize != value ) {
                this._m_length_quantize = value;
                /*try {
                    quantizeModeChangedEvent.raise( typeof( EditorConfig ), new BEventArgs() );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "EditorConfig#setLengthQuantize; ex=" + ex );
                }*/
            }
        },

        /**
         * @return [bool]
         */
        isLengthQuantizeTriplet : function() {
            return this._m_length_quantize_triplet;
        },

        /**
         * @param value [bool]
         */
        setLengthQuantizeTriplet : function( value ) {
            if ( this._m_length_quantize_triplet != value ) {
                this._m_length_quantize_triplet = value;
                /*try {
                    quantizeModeChangedEvent.raise( typeof( EditorConfig ), new BEventArgs() );
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "EditorConfig#setLengthQuantizeTriplet; ex=" + ex );
                }*/
            }
        },

        /*
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
        }*/
    };

}
