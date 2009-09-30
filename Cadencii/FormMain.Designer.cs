/*
 * Form1.Designer.cs
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
namespace Boare.Cadencii {
    using boolean = System.Boolean;
    partial class FormMain {
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

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( FormMain ) );
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSaveNamed = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileOpenVsq = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileOpenUst = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileImport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileImportVsq = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileImportMidi = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileExportWave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileExportMidi = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileRecent = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEditAutoNormalizeMode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEditSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditSelectAllEvents = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVisual = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVisualControlTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVisualMixer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVisualWaveform = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVisualProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVisualOverview = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuVisualGridline = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuVisualStartMarker = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVisualEndMarker = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuVisualLyrics = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVisualNoteProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVisualPitchLine = new System.Windows.Forms.ToolStripMenuItem();
            this.menuJob = new System.Windows.Forms.ToolStripMenuItem();
            this.menuJobNormalize = new System.Windows.Forms.ToolStripMenuItem();
            this.menuJobInsertBar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuJobDeleteBar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuJobRandomize = new System.Windows.Forms.ToolStripMenuItem();
            this.menuJobConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuJobLyric = new System.Windows.Forms.ToolStripMenuItem();
            this.menuJobRewire = new System.Windows.Forms.ToolStripMenuItem();
            this.menuJobRealTime = new System.Windows.Forms.ToolStripMenuItem();
            this.menuJobReloadVsti = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTrackOn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripSeparator();
            this.menuTrackAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTrackCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTrackChangeName = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTrackDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem22 = new System.Windows.Forms.ToolStripSeparator();
            this.menuTrackRenderCurrent = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTrackRenderAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripSeparator();
            this.menuTrackOverlay = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTrackRenderer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTrackRendererVOCALOID1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTrackRendererVOCALOID2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTrackRendererUtau = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTrackRendererStraight = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuTrackBgm = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLyric = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLyricExpressionProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLyricVibratoProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLyricSymbol = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLyricDictionary = new System.Windows.Forms.ToolStripMenuItem();
            this.menuScript = new System.Windows.Forms.ToolStripMenuItem();
            this.menuScriptUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingPreference = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingGameControler = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingGameControlerSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingGameControlerLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingGameControlerRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingPaletteTool = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingShortcut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingMidi = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingUtauVoiceDB = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSettingDefaultSingerStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSettingPositionQuantize = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingPositionQuantize04 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingPositionQuantize08 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingPositionQuantize16 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingPositionQuantize32 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingPositionQuantize64 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingPositionQuantize128 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingPositionQuantizeOff = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSettingPositionQuantizeTriplet = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingLengthQuantize = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingLengthQuantize04 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingLengthQuantize08 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingLengthQuantize16 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingLengthQuantize32 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingLengthQuantize64 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingLengthQuantize128 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingLengthQuantizeOff = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSettingLengthQuantizeTriplet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSettingSingerProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHidden = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHiddenEditLyric = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHiddenEditFlipToolPointerPencil = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHiddenEditFlipToolPointerEraser = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHiddenVisualForwardParameter = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHiddenVisualBackwardParameter = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHiddenTrackNext = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHiddenTrackBack = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHiddenCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHiddenPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHiddenCut = new System.Windows.Forms.ToolStripMenuItem();
            this.saveXmlVsqDialog = new System.Windows.Forms.SaveFileDialog();
            this.cMenuPiano = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.cMenuPianoPointer = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoPencil = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoEraser = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoPaletteTool = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoCurve = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoFixed = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoFixed01 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoFixed02 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoFixed04 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoFixed08 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoFixed16 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoFixed32 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoFixed64 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoFixed128 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoFixedOff = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoFixedTriplet = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoFixedDotted = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoQuantize = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoQuantize04 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoQuantize08 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoQuantize16 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoQuantize32 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoQuantize64 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoQuantize128 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoQuantizeOff = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem26 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoQuantizeTriplet = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoLength = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoLength04 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoLength08 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoLength16 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoLength32 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoLength64 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoLength128 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoLengthOff = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem32 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoLengthTriplet = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoCut = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoSelectAllEvents = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuPianoImportLyric = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoExpressionProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuPianoVibratoProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip( this.components );
            this.openXmlVsqDialog = new System.Windows.Forms.OpenFileDialog();
            this.cMenuTrackTab = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.cMenuTrackTabTrackOn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem24 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuTrackTabAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackTabCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackTabChangeName = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackTabDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem25 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuTrackTabRenderCurrent = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackTabRenderAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem27 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuTrackTabOverlay = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackTabRenderer = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackTabRendererVOCALOID1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackTabRendererVOCALOID2 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackTabRendererUtau = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackTabRendererStraight = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackSelector = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.cMenuTrackSelectorPointer = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackSelectorPencil = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackSelectorLine = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackSelectorEraser = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackSelectorPaletteTool = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuTrackSelectorCurve = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem28 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuTrackSelectorUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackSelectorRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem29 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuTrackSelectorCut = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackSelectorCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackSelectorPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackSelectorDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuTrackSelectorDeleteBezier = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem31 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuTrackSelectorSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.bgWorkScreen = new System.ComponentModel.BackgroundWorker();
            this.timer = new System.Windows.Forms.Timer( this.components );
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnRight1 = new System.Windows.Forms.Button();
            this.btnLeft2 = new System.Windows.Forms.Button();
            this.btnZoom = new System.Windows.Forms.Button();
            this.btnMooz = new System.Windows.Forms.Button();
            this.btnLeft1 = new System.Windows.Forms.Button();
            this.btnRight2 = new System.Windows.Forms.Button();
            this.pictOverview = new System.Windows.Forms.PictureBox();
            this.vScroll = new Boare.Lib.AppUtil.BVScrollBar();
            this.hScroll = new Boare.Lib.AppUtil.BHScrollBar();
            this.picturePositionIndicator = new System.Windows.Forms.PictureBox();
            this.pictPianoRoll = new Boare.Cadencii.BPictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.toolStripTool = new System.Windows.Forms.ToolStrip();
            this.stripBtnPointer = new System.Windows.Forms.ToolStripButton();
            this.stripBtnPencil = new System.Windows.Forms.ToolStripButton();
            this.stripBtnLine = new System.Windows.Forms.ToolStripButton();
            this.stripBtnEraser = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.stripBtnGrid = new System.Windows.Forms.ToolStripButton();
            this.stripBtnCurve = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.toolStripBottom = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.stripLblCursor = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
            this.stripLblTempo = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel10 = new System.Windows.Forms.ToolStripLabel();
            this.stripLblBeat = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.stripLblGameCtrlMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.stripLblMidiIn = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.stripDDBtnSpeed = new System.Windows.Forms.ToolStripDropDownButton();
            this.stripDDBtnSpeedTextbox = new System.Windows.Forms.ToolStripTextBox();
            this.stripDDBtnSpeed033 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnSpeed050 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnSpeed100 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerProperty = new Boare.Lib.AppUtil.BSplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.waveView = new Boare.Cadencii.WaveView();
            this.splitContainer2 = new Boare.Lib.AppUtil.BSplitContainer();
            this.splitContainer1 = new Boare.Lib.AppUtil.BSplitContainer();
            this.toolStripFile = new System.Windows.Forms.ToolStrip();
            this.stripBtnFileNew = new System.Windows.Forms.ToolStripButton();
            this.stripBtnFileOpen = new System.Windows.Forms.ToolStripButton();
            this.stripBtnFileSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.stripBtnCut = new System.Windows.Forms.ToolStripButton();
            this.stripBtnCopy = new System.Windows.Forms.ToolStripButton();
            this.stripBtnPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.stripBtnUndo = new System.Windows.Forms.ToolStripButton();
            this.stripBtnRedo = new System.Windows.Forms.ToolStripButton();
            this.toolStripPosition = new System.Windows.Forms.ToolStrip();
            this.stripBtnMoveTop = new System.Windows.Forms.ToolStripButton();
            this.stripBtnRewind = new System.Windows.Forms.ToolStripButton();
            this.stripBtnForward = new System.Windows.Forms.ToolStripButton();
            this.stripBtnMoveEnd = new System.Windows.Forms.ToolStripButton();
            this.stripBtnPlay = new System.Windows.Forms.ToolStripButton();
            this.stripBtnStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.stripBtnScroll = new System.Windows.Forms.ToolStripButton();
            this.stripBtnLoop = new System.Windows.Forms.ToolStripButton();
            this.toolStripMeasure = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.stripLblMeasure = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripSeparator();
            this.stripDDBtnLength = new System.Windows.Forms.ToolStripDropDownButton();
            this.stripDDBtnLength04 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnLength08 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnLength16 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnLength32 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnLength64 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnLength128 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnLengthOff = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.stripDDBtnLengthTriplet = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnQuantize = new System.Windows.Forms.ToolStripDropDownButton();
            this.stripDDBtnQuantize04 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnQuantize08 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnQuantize16 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnQuantize32 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnQuantize64 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnQuantize128 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripDDBtnQuantizeOff = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.stripDDBtnQuantizeTriplet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.stripBtnStartMarker = new System.Windows.Forms.ToolStripButton();
            this.stripBtnEndMarker = new System.Windows.Forms.ToolStripButton();
            this.openUstDialog = new System.Windows.Forms.OpenFileDialog();
            this.openMidiDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveMidiDialog = new System.Windows.Forms.SaveFileDialog();
            this.openWaveDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStripMain.SuspendLayout();
            this.cMenuPiano.SuspendLayout();
            this.cMenuTrackTab.SuspendLayout();
            this.cMenuTrackSelector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictOverview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePositionIndicator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictPianoRoll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.toolStripTool.SuspendLayout();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.toolStripBottom.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStripFile.SuspendLayout();
            this.toolStripPosition.SuspendLayout();
            this.toolStripMeasure.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuVisual,
            this.menuJob,
            this.menuTrack,
            this.menuLyric,
            this.menuScript,
            this.menuSetting,
            this.menuHelp,
            this.menuHidden} );
            this.menuStripMain.Location = new System.Drawing.Point( 0, 0 );
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size( 960, 24 );
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            this.menuStripMain.MouseDown += new System.Windows.Forms.MouseEventHandler( this.menuStrip1_MouseDown );
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuFileNew,
            this.menuFileOpen,
            this.menuFileSave,
            this.menuFileSaveNamed,
            this.toolStripMenuItem10,
            this.menuFileOpenVsq,
            this.menuFileOpenUst,
            this.menuFileImport,
            this.menuFileExport,
            this.toolStripMenuItem11,
            this.menuFileRecent,
            this.toolStripMenuItem12,
            this.menuFileQuit} );
            this.menuFile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size( 51, 20 );
            this.menuFile.Text = "File(&F)";
            // 
            // menuFileNew
            // 
            this.menuFileNew.Name = "menuFileNew";
            this.menuFileNew.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileNew.Text = "New(N)";
            this.menuFileNew.MouseEnter += new System.EventHandler( this.menuFileNew_MouseEnter );
            this.menuFileNew.Click += new System.EventHandler( this.commonFileNew_Click );
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.Name = "menuFileOpen";
            this.menuFileOpen.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileOpen.Text = "Open(&O)";
            this.menuFileOpen.MouseEnter += new System.EventHandler( this.menuFileOpen_MouseEnter );
            this.menuFileOpen.Click += new System.EventHandler( this.commonFileOpen_Click );
            // 
            // menuFileSave
            // 
            this.menuFileSave.Name = "menuFileSave";
            this.menuFileSave.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileSave.Text = "Save(&S)";
            this.menuFileSave.MouseEnter += new System.EventHandler( this.menuFileSave_MouseEnter );
            this.menuFileSave.Click += new System.EventHandler( this.commonFileSave_Click );
            // 
            // menuFileSaveNamed
            // 
            this.menuFileSaveNamed.Name = "menuFileSaveNamed";
            this.menuFileSaveNamed.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileSaveNamed.Text = "Save As(&A)";
            this.menuFileSaveNamed.MouseEnter += new System.EventHandler( this.menuFileSaveNamed_MouseEnter );
            this.menuFileSaveNamed.Click += new System.EventHandler( this.menuFileSaveNamed_Click );
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size( 211, 6 );
            // 
            // menuFileOpenVsq
            // 
            this.menuFileOpenVsq.Name = "menuFileOpenVsq";
            this.menuFileOpenVsq.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileOpenVsq.Text = "Open VSQ/Vocaloid Midi(&V)";
            this.menuFileOpenVsq.MouseEnter += new System.EventHandler( this.menuFileOpenVsq_MouseEnter );
            this.menuFileOpenVsq.Click += new System.EventHandler( this.menuFileOpenVsq_Click );
            // 
            // menuFileOpenUst
            // 
            this.menuFileOpenUst.Name = "menuFileOpenUst";
            this.menuFileOpenUst.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileOpenUst.Text = "Open UTAU Project File(&U)";
            this.menuFileOpenUst.MouseEnter += new System.EventHandler( this.menuFileOpenUst_MouseEnter );
            this.menuFileOpenUst.Click += new System.EventHandler( this.menuFileOpenUst_Click );
            // 
            // menuFileImport
            // 
            this.menuFileImport.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuFileImportVsq,
            this.menuFileImportMidi} );
            this.menuFileImport.Name = "menuFileImport";
            this.menuFileImport.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileImport.Text = "Import(&I)";
            this.menuFileImport.MouseEnter += new System.EventHandler( this.menuFileImport_MouseEnter );
            // 
            // menuFileImportVsq
            // 
            this.menuFileImportVsq.Name = "menuFileImportVsq";
            this.menuFileImportVsq.Size = new System.Drawing.Size( 142, 22 );
            this.menuFileImportVsq.Text = "VSQ File";
            this.menuFileImportVsq.MouseEnter += new System.EventHandler( this.menuFileImportVsq_MouseEnter );
            this.menuFileImportVsq.Click += new System.EventHandler( this.menuFileImportVsq_Click );
            // 
            // menuFileImportMidi
            // 
            this.menuFileImportMidi.Name = "menuFileImportMidi";
            this.menuFileImportMidi.Size = new System.Drawing.Size( 142, 22 );
            this.menuFileImportMidi.Text = "Standard MIDI";
            this.menuFileImportMidi.MouseEnter += new System.EventHandler( this.menuFileImportMidi_MouseEnter );
            this.menuFileImportMidi.Click += new System.EventHandler( this.menuFileImportMidi_Click );
            // 
            // menuFileExport
            // 
            this.menuFileExport.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuFileExportWave,
            this.menuFileExportMidi} );
            this.menuFileExport.Name = "menuFileExport";
            this.menuFileExport.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileExport.Text = "Export(&E)";
            this.menuFileExport.DropDownOpening += new System.EventHandler( this.menuFileExport_DropDownOpening );
            // 
            // menuFileExportWave
            // 
            this.menuFileExportWave.Name = "menuFileExportWave";
            this.menuFileExportWave.Size = new System.Drawing.Size( 97, 22 );
            this.menuFileExportWave.Text = "Wave";
            this.menuFileExportWave.MouseEnter += new System.EventHandler( this.menuFileExportWave_MouseEnter );
            this.menuFileExportWave.Click += new System.EventHandler( this.menuFileExportWave_Click );
            // 
            // menuFileExportMidi
            // 
            this.menuFileExportMidi.Name = "menuFileExportMidi";
            this.menuFileExportMidi.Size = new System.Drawing.Size( 97, 22 );
            this.menuFileExportMidi.Text = "MIDI";
            this.menuFileExportMidi.MouseEnter += new System.EventHandler( this.menuFileExportMidi_MouseEnter );
            this.menuFileExportMidi.Click += new System.EventHandler( this.menuFileExportMidi_Click );
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size( 211, 6 );
            // 
            // menuFileRecent
            // 
            this.menuFileRecent.Name = "menuFileRecent";
            this.menuFileRecent.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileRecent.Text = "Recent Files(&R)";
            this.menuFileRecent.MouseEnter += new System.EventHandler( this.menuFileRecent_MouseEnter );
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size( 211, 6 );
            // 
            // menuFileQuit
            // 
            this.menuFileQuit.Name = "menuFileQuit";
            this.menuFileQuit.Size = new System.Drawing.Size( 214, 22 );
            this.menuFileQuit.Text = "Quit(&Q)";
            this.menuFileQuit.MouseEnter += new System.EventHandler( this.menuFileQuit_MouseEnter );
            this.menuFileQuit.Click += new System.EventHandler( this.menuFileQuit_Click );
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuEditUndo,
            this.menuEditRedo,
            this.toolStripMenuItem5,
            this.menuEditCut,
            this.menuEditCopy,
            this.menuEditPaste,
            this.menuEditDelete,
            this.toolStripMenuItem19,
            this.menuEditAutoNormalizeMode,
            this.toolStripMenuItem20,
            this.menuEditSelectAll,
            this.menuEditSelectAllEvents} );
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size( 52, 20 );
            this.menuEdit.Text = "Edit(&E)";
            this.menuEdit.DropDownOpening += new System.EventHandler( this.menuEdit_DropDownOpening );
            // 
            // menuEditUndo
            // 
            this.menuEditUndo.Name = "menuEditUndo";
            this.menuEditUndo.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditUndo.Text = "Undo(&U)";
            this.menuEditUndo.MouseEnter += new System.EventHandler( this.menuEditUndo_MouseEnter );
            this.menuEditUndo.Click += new System.EventHandler( this.commonEditUndo_Click );
            // 
            // menuEditRedo
            // 
            this.menuEditRedo.Name = "menuEditRedo";
            this.menuEditRedo.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditRedo.Text = "Redo(&R)";
            this.menuEditRedo.MouseEnter += new System.EventHandler( this.menuEditRedo_MouseEnter );
            this.menuEditRedo.Click += new System.EventHandler( this.commonEditRedo_Click );
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size( 192, 6 );
            // 
            // menuEditCut
            // 
            this.menuEditCut.Name = "menuEditCut";
            this.menuEditCut.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditCut.Text = "Cut(&T)";
            this.menuEditCut.MouseEnter += new System.EventHandler( this.menuEditCut_MouseEnter );
            this.menuEditCut.Click += new System.EventHandler( this.commonEditCut_Click );
            // 
            // menuEditCopy
            // 
            this.menuEditCopy.Name = "menuEditCopy";
            this.menuEditCopy.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditCopy.Text = "Copy(&C)";
            this.menuEditCopy.MouseEnter += new System.EventHandler( this.menuEditCopy_MouseEnter );
            this.menuEditCopy.Click += new System.EventHandler( this.commonEditCopy_Click );
            // 
            // menuEditPaste
            // 
            this.menuEditPaste.Name = "menuEditPaste";
            this.menuEditPaste.ShortcutKeyDisplayString = "";
            this.menuEditPaste.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditPaste.Text = "Paste(&P)";
            this.menuEditPaste.MouseEnter += new System.EventHandler( this.menuEditPaste_MouseEnter );
            this.menuEditPaste.Click += new System.EventHandler( this.commonEditPaste_Click );
            // 
            // menuEditDelete
            // 
            this.menuEditDelete.Name = "menuEditDelete";
            this.menuEditDelete.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditDelete.Text = "Delete(&D)";
            this.menuEditDelete.MouseEnter += new System.EventHandler( this.menuEditDelete_MouseEnter );
            this.menuEditDelete.Click += new System.EventHandler( this.menuEditDelete_Click );
            // 
            // toolStripMenuItem19
            // 
            this.toolStripMenuItem19.Name = "toolStripMenuItem19";
            this.toolStripMenuItem19.Size = new System.Drawing.Size( 192, 6 );
            // 
            // menuEditAutoNormalizeMode
            // 
            this.menuEditAutoNormalizeMode.Name = "menuEditAutoNormalizeMode";
            this.menuEditAutoNormalizeMode.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditAutoNormalizeMode.Text = "Auto Normalize Mode(&N)";
            this.menuEditAutoNormalizeMode.MouseEnter += new System.EventHandler( this.menuEditAutoNormalizeMode_MouseEnter );
            this.menuEditAutoNormalizeMode.Click += new System.EventHandler( this.menuEditAutoNormalizeMode_Click );
            // 
            // toolStripMenuItem20
            // 
            this.toolStripMenuItem20.Name = "toolStripMenuItem20";
            this.toolStripMenuItem20.Size = new System.Drawing.Size( 192, 6 );
            // 
            // menuEditSelectAll
            // 
            this.menuEditSelectAll.Name = "menuEditSelectAll";
            this.menuEditSelectAll.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditSelectAll.Text = "Select All(&A)";
            this.menuEditSelectAll.MouseEnter += new System.EventHandler( this.menuEditSelectAll_MouseEnter );
            this.menuEditSelectAll.Click += new System.EventHandler( this.menuEditSelectAll_Click );
            // 
            // menuEditSelectAllEvents
            // 
            this.menuEditSelectAllEvents.Name = "menuEditSelectAllEvents";
            this.menuEditSelectAllEvents.Size = new System.Drawing.Size( 195, 22 );
            this.menuEditSelectAllEvents.Text = "Select All Events(&E)";
            this.menuEditSelectAllEvents.MouseEnter += new System.EventHandler( this.menuEditSelectAllEvents_MouseEnter );
            this.menuEditSelectAllEvents.Click += new System.EventHandler( this.menuEditSelectAllEvents_Click );
            // 
            // menuVisual
            // 
            this.menuVisual.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisual.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuVisualControlTrack,
            this.menuVisualMixer,
            this.menuVisualWaveform,
            this.menuVisualProperty,
            this.menuVisualOverview,
            this.toolStripMenuItem1,
            this.menuVisualGridline,
            this.toolStripMenuItem2,
            this.menuVisualStartMarker,
            this.menuVisualEndMarker,
            this.toolStripMenuItem3,
            this.menuVisualLyrics,
            this.menuVisualNoteProperty,
            this.menuVisualPitchLine} );
            this.menuVisual.Name = "menuVisual";
            this.menuVisual.Size = new System.Drawing.Size( 58, 20 );
            this.menuVisual.Text = "View(&V)";
            // 
            // menuVisualControlTrack
            // 
            this.menuVisualControlTrack.Checked = true;
            this.menuVisualControlTrack.CheckOnClick = true;
            this.menuVisualControlTrack.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuVisualControlTrack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisualControlTrack.Name = "menuVisualControlTrack";
            this.menuVisualControlTrack.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualControlTrack.Text = "Control Track(&C)";
            this.menuVisualControlTrack.CheckedChanged += new System.EventHandler( this.menuVisualControlTrack_CheckedChanged );
            this.menuVisualControlTrack.MouseEnter += new System.EventHandler( this.menuVisualControlTrack_MouseEnter );
            // 
            // menuVisualMixer
            // 
            this.menuVisualMixer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisualMixer.Name = "menuVisualMixer";
            this.menuVisualMixer.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualMixer.Text = "Mixer(&X)";
            this.menuVisualMixer.MouseEnter += new System.EventHandler( this.menuVisualMixer_MouseEnter );
            this.menuVisualMixer.Click += new System.EventHandler( this.menuVisualMixer_Click );
            // 
            // menuVisualWaveform
            // 
            this.menuVisualWaveform.CheckOnClick = true;
            this.menuVisualWaveform.Name = "menuVisualWaveform";
            this.menuVisualWaveform.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualWaveform.Text = "Waveform(&W)";
            this.menuVisualWaveform.CheckedChanged += new System.EventHandler( this.menuVisualWaveform_CheckedChanged );
            this.menuVisualWaveform.MouseEnter += new System.EventHandler( this.menuVisualWaveform_MouseEnter );
            // 
            // menuVisualProperty
            // 
            this.menuVisualProperty.CheckOnClick = true;
            this.menuVisualProperty.Name = "menuVisualProperty";
            this.menuVisualProperty.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualProperty.Text = "Property Window(&C)";
            this.menuVisualProperty.MouseEnter += new System.EventHandler( this.menuVisualProperty_MouseEnter );
            this.menuVisualProperty.Click += new System.EventHandler( this.menuVisualProperty_Click );
            // 
            // menuVisualOverview
            // 
            this.menuVisualOverview.CheckOnClick = true;
            this.menuVisualOverview.Name = "menuVisualOverview";
            this.menuVisualOverview.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualOverview.Text = "Overview(&O)";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size( 210, 6 );
            // 
            // menuVisualGridline
            // 
            this.menuVisualGridline.CheckOnClick = true;
            this.menuVisualGridline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisualGridline.Name = "menuVisualGridline";
            this.menuVisualGridline.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualGridline.Text = "Grid Line(&G)";
            this.menuVisualGridline.CheckedChanged += new System.EventHandler( this.menuVisualGridline_CheckedChanged );
            this.menuVisualGridline.MouseEnter += new System.EventHandler( this.menuVisualGridline_MouseEnter );
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size( 210, 6 );
            // 
            // menuVisualStartMarker
            // 
            this.menuVisualStartMarker.CheckOnClick = true;
            this.menuVisualStartMarker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisualStartMarker.Enabled = false;
            this.menuVisualStartMarker.Name = "menuVisualStartMarker";
            this.menuVisualStartMarker.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualStartMarker.Text = "Start Marker(&S)";
            this.menuVisualStartMarker.MouseEnter += new System.EventHandler( this.menuVisualStartMarker_MouseEnter );
            // 
            // menuVisualEndMarker
            // 
            this.menuVisualEndMarker.CheckOnClick = true;
            this.menuVisualEndMarker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisualEndMarker.Enabled = false;
            this.menuVisualEndMarker.Name = "menuVisualEndMarker";
            this.menuVisualEndMarker.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualEndMarker.Text = "End Marker(&E)";
            this.menuVisualEndMarker.MouseEnter += new System.EventHandler( this.menuVisualEndMarker_MouseEnter );
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size( 210, 6 );
            // 
            // menuVisualLyrics
            // 
            this.menuVisualLyrics.Checked = true;
            this.menuVisualLyrics.CheckOnClick = true;
            this.menuVisualLyrics.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuVisualLyrics.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisualLyrics.Name = "menuVisualLyrics";
            this.menuVisualLyrics.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualLyrics.Text = "Lyric/Phoneme(&L)";
            this.menuVisualLyrics.CheckedChanged += new System.EventHandler( this.menuVisualLyrics_CheckedChanged );
            this.menuVisualLyrics.MouseEnter += new System.EventHandler( this.menuVisualLyrics_MouseEnter );
            // 
            // menuVisualNoteProperty
            // 
            this.menuVisualNoteProperty.Checked = true;
            this.menuVisualNoteProperty.CheckOnClick = true;
            this.menuVisualNoteProperty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuVisualNoteProperty.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuVisualNoteProperty.Name = "menuVisualNoteProperty";
            this.menuVisualNoteProperty.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualNoteProperty.Text = "Note Expression/Vibrato(&N)";
            this.menuVisualNoteProperty.CheckedChanged += new System.EventHandler( this.menuVisualNoteProperty_CheckedChanged );
            this.menuVisualNoteProperty.MouseEnter += new System.EventHandler( this.menuVisualNoteProperty_MouseEnter );
            // 
            // menuVisualPitchLine
            // 
            this.menuVisualPitchLine.CheckOnClick = true;
            this.menuVisualPitchLine.Name = "menuVisualPitchLine";
            this.menuVisualPitchLine.Size = new System.Drawing.Size( 213, 22 );
            this.menuVisualPitchLine.Text = "Pitch Line(&P)";
            this.menuVisualPitchLine.CheckedChanged += new System.EventHandler( this.menuVisualPitchLine_CheckedChanged );
            this.menuVisualPitchLine.MouseEnter += new System.EventHandler( this.menuVisualPitchLine_MouseEnter );
            // 
            // menuJob
            // 
            this.menuJob.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuJobNormalize,
            this.menuJobInsertBar,
            this.menuJobDeleteBar,
            this.menuJobRandomize,
            this.menuJobConnect,
            this.menuJobLyric,
            this.menuJobRewire,
            this.menuJobRealTime,
            this.menuJobReloadVsti} );
            this.menuJob.Name = "menuJob";
            this.menuJob.Size = new System.Drawing.Size( 51, 20 );
            this.menuJob.Text = "Job(&J)";
            this.menuJob.DropDownOpening += new System.EventHandler( this.menuJob_DropDownOpening );
            // 
            // menuJobNormalize
            // 
            this.menuJobNormalize.Name = "menuJobNormalize";
            this.menuJobNormalize.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobNormalize.Text = "Normalize Notes(&N)";
            this.menuJobNormalize.MouseEnter += new System.EventHandler( this.menuJobNormalize_MouseEnter );
            this.menuJobNormalize.Click += new System.EventHandler( this.menuJobNormalize_Click );
            // 
            // menuJobInsertBar
            // 
            this.menuJobInsertBar.Name = "menuJobInsertBar";
            this.menuJobInsertBar.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobInsertBar.Text = "Insert Bars(&I)";
            this.menuJobInsertBar.MouseEnter += new System.EventHandler( this.menuJobInsertBar_MouseEnter );
            this.menuJobInsertBar.Click += new System.EventHandler( this.menuJobInsertBar_Click );
            // 
            // menuJobDeleteBar
            // 
            this.menuJobDeleteBar.Name = "menuJobDeleteBar";
            this.menuJobDeleteBar.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobDeleteBar.Text = "Delete Bars(&D)";
            this.menuJobDeleteBar.MouseEnter += new System.EventHandler( this.menuJobDeleteBar_MouseEnter );
            this.menuJobDeleteBar.Click += new System.EventHandler( this.menuJobDeleteBar_Click );
            // 
            // menuJobRandomize
            // 
            this.menuJobRandomize.Enabled = false;
            this.menuJobRandomize.Name = "menuJobRandomize";
            this.menuJobRandomize.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobRandomize.Text = "Randomize(&R)";
            this.menuJobRandomize.MouseEnter += new System.EventHandler( this.menuJobRandomize_MouseEnter );
            // 
            // menuJobConnect
            // 
            this.menuJobConnect.Name = "menuJobConnect";
            this.menuJobConnect.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobConnect.Text = "Connect Notes(&C)";
            this.menuJobConnect.MouseEnter += new System.EventHandler( this.menuJobConnect_MouseEnter );
            this.menuJobConnect.Click += new System.EventHandler( this.menuJobConnect_Click );
            // 
            // menuJobLyric
            // 
            this.menuJobLyric.Name = "menuJobLyric";
            this.menuJobLyric.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobLyric.Text = "Insert Lyrics(&L)";
            this.menuJobLyric.MouseEnter += new System.EventHandler( this.menuJobLyric_MouseEnter );
            this.menuJobLyric.Click += new System.EventHandler( this.menuJobLyric_Click );
            // 
            // menuJobRewire
            // 
            this.menuJobRewire.Enabled = false;
            this.menuJobRewire.Name = "menuJobRewire";
            this.menuJobRewire.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobRewire.Text = "Import ReWire Host Tempo(&T)";
            this.menuJobRewire.MouseEnter += new System.EventHandler( this.menuJobRewire_MouseEnter );
            // 
            // menuJobRealTime
            // 
            this.menuJobRealTime.Name = "menuJobRealTime";
            this.menuJobRealTime.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobRealTime.Text = "Start Realtime Input";
            this.menuJobRealTime.MouseEnter += new System.EventHandler( this.menuJobRealTime_MouseEnter );
            this.menuJobRealTime.Click += new System.EventHandler( this.menuJobRealTime_Click );
            // 
            // menuJobReloadVsti
            // 
            this.menuJobReloadVsti.Name = "menuJobReloadVsti";
            this.menuJobReloadVsti.Size = new System.Drawing.Size( 223, 22 );
            this.menuJobReloadVsti.Text = "Reload VSTi(&R)";
            this.menuJobReloadVsti.Visible = false;
            this.menuJobReloadVsti.MouseEnter += new System.EventHandler( this.menuJobReloadVsti_MouseEnter );
            this.menuJobReloadVsti.Click += new System.EventHandler( this.menuJobReloadVsti_Click );
            // 
            // menuTrack
            // 
            this.menuTrack.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuTrackOn,
            this.toolStripMenuItem21,
            this.menuTrackAdd,
            this.menuTrackCopy,
            this.menuTrackChangeName,
            this.menuTrackDelete,
            this.toolStripMenuItem22,
            this.menuTrackRenderCurrent,
            this.menuTrackRenderAll,
            this.toolStripMenuItem23,
            this.menuTrackOverlay,
            this.menuTrackRenderer,
            this.toolStripMenuItem4,
            this.menuTrackBgm} );
            this.menuTrack.Name = "menuTrack";
            this.menuTrack.Size = new System.Drawing.Size( 61, 20 );
            this.menuTrack.Text = "Track(&T)";
            this.menuTrack.DropDownOpening += new System.EventHandler( this.menuTrack_DropDownOpening );
            // 
            // menuTrackOn
            // 
            this.menuTrackOn.Name = "menuTrackOn";
            this.menuTrackOn.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackOn.Text = "Track On(&K)";
            this.menuTrackOn.MouseEnter += new System.EventHandler( this.menuTrackOn_MouseEnter );
            this.menuTrackOn.Click += new System.EventHandler( this.menuTrackOn_Click );
            // 
            // toolStripMenuItem21
            // 
            this.toolStripMenuItem21.Name = "toolStripMenuItem21";
            this.toolStripMenuItem21.Size = new System.Drawing.Size( 193, 6 );
            // 
            // menuTrackAdd
            // 
            this.menuTrackAdd.Name = "menuTrackAdd";
            this.menuTrackAdd.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackAdd.Text = "Add Track(&A)";
            this.menuTrackAdd.MouseEnter += new System.EventHandler( this.menuTrackAdd_MouseEnter );
            this.menuTrackAdd.Click += new System.EventHandler( this.menuTrackAdd_Click );
            // 
            // menuTrackCopy
            // 
            this.menuTrackCopy.Name = "menuTrackCopy";
            this.menuTrackCopy.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackCopy.Text = "Copy Track(&C)";
            this.menuTrackCopy.MouseEnter += new System.EventHandler( this.menuTrackCopy_MouseEnter );
            this.menuTrackCopy.Click += new System.EventHandler( this.menuTrackCopy_Click );
            // 
            // menuTrackChangeName
            // 
            this.menuTrackChangeName.Name = "menuTrackChangeName";
            this.menuTrackChangeName.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackChangeName.Text = "Rename Track(&R)";
            this.menuTrackChangeName.MouseEnter += new System.EventHandler( this.menuTrackChangeName_MouseEnter );
            this.menuTrackChangeName.Click += new System.EventHandler( this.menuTrackChangeName_Click );
            // 
            // menuTrackDelete
            // 
            this.menuTrackDelete.Name = "menuTrackDelete";
            this.menuTrackDelete.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackDelete.Text = "Delete Track(&D)";
            this.menuTrackDelete.MouseEnter += new System.EventHandler( this.menuTrackDelete_MouseEnter );
            this.menuTrackDelete.Click += new System.EventHandler( this.menuTrackDelete_Click );
            // 
            // toolStripMenuItem22
            // 
            this.toolStripMenuItem22.Name = "toolStripMenuItem22";
            this.toolStripMenuItem22.Size = new System.Drawing.Size( 193, 6 );
            // 
            // menuTrackRenderCurrent
            // 
            this.menuTrackRenderCurrent.Name = "menuTrackRenderCurrent";
            this.menuTrackRenderCurrent.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackRenderCurrent.Text = "Render Current Track(&T)";
            this.menuTrackRenderCurrent.MouseEnter += new System.EventHandler( this.menuTrackRenderCurrent_MouseEnter );
            this.menuTrackRenderCurrent.Click += new System.EventHandler( this.menuTrackRenderCurrent_Click );
            // 
            // menuTrackRenderAll
            // 
            this.menuTrackRenderAll.Enabled = false;
            this.menuTrackRenderAll.Name = "menuTrackRenderAll";
            this.menuTrackRenderAll.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackRenderAll.Text = "Render All Tracks(&S)";
            this.menuTrackRenderAll.MouseEnter += new System.EventHandler( this.menuTrackRenderAll_MouseEnter );
            this.menuTrackRenderAll.Click += new System.EventHandler( this.commonTrackRenderAll_Click );
            // 
            // toolStripMenuItem23
            // 
            this.toolStripMenuItem23.Name = "toolStripMenuItem23";
            this.toolStripMenuItem23.Size = new System.Drawing.Size( 193, 6 );
            // 
            // menuTrackOverlay
            // 
            this.menuTrackOverlay.Name = "menuTrackOverlay";
            this.menuTrackOverlay.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackOverlay.Text = "Overlay(&O)";
            this.menuTrackOverlay.MouseEnter += new System.EventHandler( this.menuTrackOverlay_MouseEnter );
            this.menuTrackOverlay.Click += new System.EventHandler( this.menuTrackOverlay_Click );
            // 
            // menuTrackRenderer
            // 
            this.menuTrackRenderer.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuTrackRendererVOCALOID1,
            this.menuTrackRendererVOCALOID2,
            this.menuTrackRendererUtau,
            this.menuTrackRendererStraight} );
            this.menuTrackRenderer.Name = "menuTrackRenderer";
            this.menuTrackRenderer.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackRenderer.Text = "Renderer";
            this.menuTrackRenderer.MouseEnter += new System.EventHandler( this.menuTrackRenderer_MouseEnter );
            this.menuTrackRenderer.DropDownOpening += new System.EventHandler( this.menuTrackRenderer_DropDownOpening );
            // 
            // menuTrackRendererVOCALOID1
            // 
            this.menuTrackRendererVOCALOID1.Name = "menuTrackRendererVOCALOID1";
            this.menuTrackRendererVOCALOID1.Size = new System.Drawing.Size( 156, 22 );
            this.menuTrackRendererVOCALOID1.Text = "VOCALOID1";
            this.menuTrackRendererVOCALOID1.MouseEnter += new System.EventHandler( this.menuTrackRendererVOCALOID1_MouseEnter );
            this.menuTrackRendererVOCALOID1.Click += new System.EventHandler( this.commonRendererVOCALOID1_Click );
            // 
            // menuTrackRendererVOCALOID2
            // 
            this.menuTrackRendererVOCALOID2.Name = "menuTrackRendererVOCALOID2";
            this.menuTrackRendererVOCALOID2.Size = new System.Drawing.Size( 156, 22 );
            this.menuTrackRendererVOCALOID2.Text = "VOCALOID2";
            this.menuTrackRendererVOCALOID2.MouseEnter += new System.EventHandler( this.menuTrackRendererVOCALOID2_MouseEnter );
            this.menuTrackRendererVOCALOID2.Click += new System.EventHandler( this.commonRendererVOCALOID2_Click );
            // 
            // menuTrackRendererUtau
            // 
            this.menuTrackRendererUtau.Name = "menuTrackRendererUtau";
            this.menuTrackRendererUtau.Size = new System.Drawing.Size( 156, 22 );
            this.menuTrackRendererUtau.Text = "UTAU";
            this.menuTrackRendererUtau.MouseEnter += new System.EventHandler( this.menuTrackRendererUtau_MouseEnter );
            this.menuTrackRendererUtau.Click += new System.EventHandler( this.commonRendererUtau_Click );
            // 
            // menuTrackRendererStraight
            // 
            this.menuTrackRendererStraight.Name = "menuTrackRendererStraight";
            this.menuTrackRendererStraight.Size = new System.Drawing.Size( 156, 22 );
            this.menuTrackRendererStraight.Text = "Straight X UTAU";
            this.menuTrackRendererStraight.Click += new System.EventHandler( this.commonRendererStraight_Click );
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size( 193, 6 );
            // 
            // menuTrackBgm
            // 
            this.menuTrackBgm.Name = "menuTrackBgm";
            this.menuTrackBgm.Size = new System.Drawing.Size( 196, 22 );
            this.menuTrackBgm.Text = "BGM(&B)";
            // 
            // menuLyric
            // 
            this.menuLyric.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuLyricExpressionProperty,
            this.menuLyricVibratoProperty,
            this.menuLyricSymbol,
            this.menuLyricDictionary} );
            this.menuLyric.Name = "menuLyric";
            this.menuLyric.Size = new System.Drawing.Size( 62, 20 );
            this.menuLyric.Text = "Lyrics(&L)";
            this.menuLyric.DropDownOpening += new System.EventHandler( this.menuLyric_DropDownOpening );
            // 
            // menuLyricExpressionProperty
            // 
            this.menuLyricExpressionProperty.Name = "menuLyricExpressionProperty";
            this.menuLyricExpressionProperty.Size = new System.Drawing.Size( 216, 22 );
            this.menuLyricExpressionProperty.Text = "Note Expression Property(&E)";
            this.menuLyricExpressionProperty.Click += new System.EventHandler( this.menuLyricExpressionProperty_Click );
            // 
            // menuLyricVibratoProperty
            // 
            this.menuLyricVibratoProperty.Name = "menuLyricVibratoProperty";
            this.menuLyricVibratoProperty.Size = new System.Drawing.Size( 216, 22 );
            this.menuLyricVibratoProperty.Text = "Note Vibrato Property(&V)";
            this.menuLyricVibratoProperty.Click += new System.EventHandler( this.menuLyricVibratoProperty_Click );
            // 
            // menuLyricSymbol
            // 
            this.menuLyricSymbol.Enabled = false;
            this.menuLyricSymbol.Name = "menuLyricSymbol";
            this.menuLyricSymbol.Size = new System.Drawing.Size( 216, 22 );
            this.menuLyricSymbol.Text = "Phoneme Transformation(&T)";
            // 
            // menuLyricDictionary
            // 
            this.menuLyricDictionary.Name = "menuLyricDictionary";
            this.menuLyricDictionary.Size = new System.Drawing.Size( 216, 22 );
            this.menuLyricDictionary.Text = "User Word Dictionary(&C)";
            this.menuLyricDictionary.Click += new System.EventHandler( this.menuLyricDictionary_Click );
            // 
            // menuScript
            // 
            this.menuScript.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuScriptUpdate} );
            this.menuScript.Name = "menuScript";
            this.menuScript.Size = new System.Drawing.Size( 63, 20 );
            this.menuScript.Text = "Script(&C)";
            // 
            // menuScriptUpdate
            // 
            this.menuScriptUpdate.Name = "menuScriptUpdate";
            this.menuScriptUpdate.Size = new System.Drawing.Size( 179, 22 );
            this.menuScriptUpdate.Text = "Update Script List(&U)";
            this.menuScriptUpdate.Click += new System.EventHandler( this.menuScriptUpdate_Click );
            // 
            // menuSetting
            // 
            this.menuSetting.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuSettingPreference,
            this.menuSettingGameControler,
            this.menuSettingPaletteTool,
            this.menuSettingShortcut,
            this.menuSettingMidi,
            this.menuSettingUtauVoiceDB,
            this.toolStripMenuItem6,
            this.menuSettingDefaultSingerStyle,
            this.toolStripMenuItem7,
            this.menuSettingPositionQuantize,
            this.menuSettingLengthQuantize,
            this.toolStripMenuItem8,
            this.menuSettingSingerProperty} );
            this.menuSetting.Name = "menuSetting";
            this.menuSetting.Size = new System.Drawing.Size( 68, 20 );
            this.menuSetting.Text = "Setting(&S)";
            this.menuSetting.DropDownOpening += new System.EventHandler( this.menuSetting_DropDownOpening );
            // 
            // menuSettingPreference
            // 
            this.menuSettingPreference.Name = "menuSettingPreference";
            this.menuSettingPreference.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingPreference.Text = "Preference(&P)";
            this.menuSettingPreference.Click += new System.EventHandler( this.menuSettingPreference_Click );
            // 
            // menuSettingGameControler
            // 
            this.menuSettingGameControler.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuSettingGameControlerSetting,
            this.menuSettingGameControlerLoad,
            this.menuSettingGameControlerRemove} );
            this.menuSettingGameControler.Name = "menuSettingGameControler";
            this.menuSettingGameControler.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingGameControler.Text = "Game Controler(&G)";
            // 
            // menuSettingGameControlerSetting
            // 
            this.menuSettingGameControlerSetting.Name = "menuSettingGameControlerSetting";
            this.menuSettingGameControlerSetting.Size = new System.Drawing.Size( 127, 22 );
            this.menuSettingGameControlerSetting.Text = "Setting(&S)";
            this.menuSettingGameControlerSetting.Click += new System.EventHandler( this.menuSettingGameControlerSetting_Click );
            // 
            // menuSettingGameControlerLoad
            // 
            this.menuSettingGameControlerLoad.Name = "menuSettingGameControlerLoad";
            this.menuSettingGameControlerLoad.Size = new System.Drawing.Size( 127, 22 );
            this.menuSettingGameControlerLoad.Text = "Load(&L)";
            this.menuSettingGameControlerLoad.Click += new System.EventHandler( this.menuSettingGameControlerLoad_Click );
            // 
            // menuSettingGameControlerRemove
            // 
            this.menuSettingGameControlerRemove.Name = "menuSettingGameControlerRemove";
            this.menuSettingGameControlerRemove.Size = new System.Drawing.Size( 127, 22 );
            this.menuSettingGameControlerRemove.Text = "Remove(&R)";
            this.menuSettingGameControlerRemove.Click += new System.EventHandler( this.menuSettingGameControlerRemove_Click );
            // 
            // menuSettingPaletteTool
            // 
            this.menuSettingPaletteTool.Name = "menuSettingPaletteTool";
            this.menuSettingPaletteTool.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingPaletteTool.Text = "Palette Tool(&T)";
            // 
            // menuSettingShortcut
            // 
            this.menuSettingShortcut.Name = "menuSettingShortcut";
            this.menuSettingShortcut.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingShortcut.Text = "Shortcut Key(&S)";
            this.menuSettingShortcut.Click += new System.EventHandler( this.menuSettingShortcut_Click );
            // 
            // menuSettingMidi
            // 
            this.menuSettingMidi.Name = "menuSettingMidi";
            this.menuSettingMidi.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingMidi.Text = "MIDI(&M)";
            this.menuSettingMidi.Click += new System.EventHandler( this.menuSettingMidi_Click );
            // 
            // menuSettingUtauVoiceDB
            // 
            this.menuSettingUtauVoiceDB.Name = "menuSettingUtauVoiceDB";
            this.menuSettingUtauVoiceDB.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingUtauVoiceDB.Text = "UTAU Voice DB(&U)";
            this.menuSettingUtauVoiceDB.Click += new System.EventHandler( this.menuSettingUtauVoiceDB_Click );
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size( 197, 6 );
            // 
            // menuSettingDefaultSingerStyle
            // 
            this.menuSettingDefaultSingerStyle.Name = "menuSettingDefaultSingerStyle";
            this.menuSettingDefaultSingerStyle.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingDefaultSingerStyle.Text = "Singing Style Defaults(&D)";
            this.menuSettingDefaultSingerStyle.Click += new System.EventHandler( this.menuSettingDefaultSingerStyle_Click );
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size( 197, 6 );
            // 
            // menuSettingPositionQuantize
            // 
            this.menuSettingPositionQuantize.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuSettingPositionQuantize04,
            this.menuSettingPositionQuantize08,
            this.menuSettingPositionQuantize16,
            this.menuSettingPositionQuantize32,
            this.menuSettingPositionQuantize64,
            this.menuSettingPositionQuantize128,
            this.menuSettingPositionQuantizeOff,
            this.toolStripMenuItem9,
            this.menuSettingPositionQuantizeTriplet} );
            this.menuSettingPositionQuantize.Name = "menuSettingPositionQuantize";
            this.menuSettingPositionQuantize.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingPositionQuantize.Text = "Quantize(&Q)";
            // 
            // menuSettingPositionQuantize04
            // 
            this.menuSettingPositionQuantize04.Name = "menuSettingPositionQuantize04";
            this.menuSettingPositionQuantize04.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantize04.Text = "1/4";
            this.menuSettingPositionQuantize04.Click += new System.EventHandler( this.h_positionQuantize04 );
            // 
            // menuSettingPositionQuantize08
            // 
            this.menuSettingPositionQuantize08.Name = "menuSettingPositionQuantize08";
            this.menuSettingPositionQuantize08.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantize08.Text = "1/8";
            this.menuSettingPositionQuantize08.Click += new System.EventHandler( this.h_positionQuantize08 );
            // 
            // menuSettingPositionQuantize16
            // 
            this.menuSettingPositionQuantize16.Name = "menuSettingPositionQuantize16";
            this.menuSettingPositionQuantize16.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantize16.Text = "1/16";
            this.menuSettingPositionQuantize16.Click += new System.EventHandler( this.h_positionQuantize16 );
            // 
            // menuSettingPositionQuantize32
            // 
            this.menuSettingPositionQuantize32.Name = "menuSettingPositionQuantize32";
            this.menuSettingPositionQuantize32.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantize32.Text = "1/32";
            this.menuSettingPositionQuantize32.Click += new System.EventHandler( this.h_positionQuantize32 );
            // 
            // menuSettingPositionQuantize64
            // 
            this.menuSettingPositionQuantize64.Name = "menuSettingPositionQuantize64";
            this.menuSettingPositionQuantize64.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantize64.Text = "1/64";
            this.menuSettingPositionQuantize64.Click += new System.EventHandler( this.h_positionQuantize64 );
            // 
            // menuSettingPositionQuantize128
            // 
            this.menuSettingPositionQuantize128.Name = "menuSettingPositionQuantize128";
            this.menuSettingPositionQuantize128.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantize128.Text = "1/128";
            this.menuSettingPositionQuantize128.Click += new System.EventHandler( this.h_positionQuantize128 );
            // 
            // menuSettingPositionQuantizeOff
            // 
            this.menuSettingPositionQuantizeOff.Name = "menuSettingPositionQuantizeOff";
            this.menuSettingPositionQuantizeOff.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantizeOff.Text = "Off";
            this.menuSettingPositionQuantizeOff.Click += new System.EventHandler( this.h_positionQuantizeOff );
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size( 100, 6 );
            // 
            // menuSettingPositionQuantizeTriplet
            // 
            this.menuSettingPositionQuantizeTriplet.Name = "menuSettingPositionQuantizeTriplet";
            this.menuSettingPositionQuantizeTriplet.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingPositionQuantizeTriplet.Text = "Triplet";
            this.menuSettingPositionQuantizeTriplet.Click += new System.EventHandler( this.h_positionQuantizeTriplet );
            // 
            // menuSettingLengthQuantize
            // 
            this.menuSettingLengthQuantize.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuSettingLengthQuantize04,
            this.menuSettingLengthQuantize08,
            this.menuSettingLengthQuantize16,
            this.menuSettingLengthQuantize32,
            this.menuSettingLengthQuantize64,
            this.menuSettingLengthQuantize128,
            this.menuSettingLengthQuantizeOff,
            this.toolStripSeparator1,
            this.menuSettingLengthQuantizeTriplet} );
            this.menuSettingLengthQuantize.Name = "menuSettingLengthQuantize";
            this.menuSettingLengthQuantize.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingLengthQuantize.Text = "Length(&L)";
            // 
            // menuSettingLengthQuantize04
            // 
            this.menuSettingLengthQuantize04.Name = "menuSettingLengthQuantize04";
            this.menuSettingLengthQuantize04.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantize04.Text = "1/4";
            this.menuSettingLengthQuantize04.Click += new System.EventHandler( this.h_lengthQuantize04 );
            // 
            // menuSettingLengthQuantize08
            // 
            this.menuSettingLengthQuantize08.Name = "menuSettingLengthQuantize08";
            this.menuSettingLengthQuantize08.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantize08.Text = "1/8";
            this.menuSettingLengthQuantize08.Click += new System.EventHandler( this.h_lengthQuantize08 );
            // 
            // menuSettingLengthQuantize16
            // 
            this.menuSettingLengthQuantize16.Name = "menuSettingLengthQuantize16";
            this.menuSettingLengthQuantize16.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantize16.Text = "1/16";
            this.menuSettingLengthQuantize16.Click += new System.EventHandler( this.h_lengthQuantize16 );
            // 
            // menuSettingLengthQuantize32
            // 
            this.menuSettingLengthQuantize32.Name = "menuSettingLengthQuantize32";
            this.menuSettingLengthQuantize32.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantize32.Text = "1/32";
            this.menuSettingLengthQuantize32.Click += new System.EventHandler( this.h_lengthQuantize32 );
            // 
            // menuSettingLengthQuantize64
            // 
            this.menuSettingLengthQuantize64.Name = "menuSettingLengthQuantize64";
            this.menuSettingLengthQuantize64.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantize64.Text = "1/64";
            this.menuSettingLengthQuantize64.Click += new System.EventHandler( this.h_lengthQuantize64 );
            // 
            // menuSettingLengthQuantize128
            // 
            this.menuSettingLengthQuantize128.Name = "menuSettingLengthQuantize128";
            this.menuSettingLengthQuantize128.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantize128.Text = "1/128";
            this.menuSettingLengthQuantize128.Click += new System.EventHandler( this.h_lengthQuantize128 );
            // 
            // menuSettingLengthQuantizeOff
            // 
            this.menuSettingLengthQuantizeOff.Name = "menuSettingLengthQuantizeOff";
            this.menuSettingLengthQuantizeOff.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantizeOff.Text = "Off";
            this.menuSettingLengthQuantizeOff.Click += new System.EventHandler( this.h_lengthQuantizeOff );
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size( 100, 6 );
            // 
            // menuSettingLengthQuantizeTriplet
            // 
            this.menuSettingLengthQuantizeTriplet.Name = "menuSettingLengthQuantizeTriplet";
            this.menuSettingLengthQuantizeTriplet.Size = new System.Drawing.Size( 103, 22 );
            this.menuSettingLengthQuantizeTriplet.Text = "Triplet";
            this.menuSettingLengthQuantizeTriplet.Click += new System.EventHandler( this.h_lengthQuantizeTriplet );
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size( 197, 6 );
            // 
            // menuSettingSingerProperty
            // 
            this.menuSettingSingerProperty.Enabled = false;
            this.menuSettingSingerProperty.Name = "menuSettingSingerProperty";
            this.menuSettingSingerProperty.Size = new System.Drawing.Size( 200, 22 );
            this.menuSettingSingerProperty.Text = "Singer Properties(&S)";
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpAbout,
            this.menuHelpDebug} );
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size( 56, 20 );
            this.menuHelp.Text = "Help(&H)";
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.Size = new System.Drawing.Size( 164, 22 );
            this.menuHelpAbout.Text = "About Cadencii(&A)";
            this.menuHelpAbout.Click += new System.EventHandler( this.menuHelpAbout_Click );
            // 
            // menuHelpDebug
            // 
            this.menuHelpDebug.Name = "menuHelpDebug";
            this.menuHelpDebug.Size = new System.Drawing.Size( 164, 22 );
            this.menuHelpDebug.Text = "Debug";
            this.menuHelpDebug.Visible = false;
            this.menuHelpDebug.Click += new System.EventHandler( this.menuHelpDebug_Click );
            // 
            // menuHidden
            // 
            this.menuHidden.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.menuHiddenEditLyric,
            this.menuHiddenEditFlipToolPointerPencil,
            this.menuHiddenEditFlipToolPointerEraser,
            this.menuHiddenVisualForwardParameter,
            this.menuHiddenVisualBackwardParameter,
            this.menuHiddenTrackNext,
            this.menuHiddenTrackBack,
            this.menuHiddenCopy,
            this.menuHiddenPaste,
            this.menuHiddenCut} );
            this.menuHidden.Name = "menuHidden";
            this.menuHidden.Size = new System.Drawing.Size( 79, 20 );
            this.menuHidden.Text = "MenuHidden";
            this.menuHidden.Visible = false;
            // 
            // menuHiddenEditLyric
            // 
            this.menuHiddenEditLyric.Name = "menuHiddenEditLyric";
            this.menuHiddenEditLyric.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.menuHiddenEditLyric.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenEditLyric.Text = "Start Lyric Input";
            this.menuHiddenEditLyric.Visible = false;
            this.menuHiddenEditLyric.Click += new System.EventHandler( this.menuHiddenEditLyric_Click );
            // 
            // menuHiddenEditFlipToolPointerPencil
            // 
            this.menuHiddenEditFlipToolPointerPencil.Name = "menuHiddenEditFlipToolPointerPencil";
            this.menuHiddenEditFlipToolPointerPencil.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.menuHiddenEditFlipToolPointerPencil.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenEditFlipToolPointerPencil.Text = "Change Tool Pointer / Pencil";
            this.menuHiddenEditFlipToolPointerPencil.Visible = false;
            this.menuHiddenEditFlipToolPointerPencil.Click += new System.EventHandler( this.menuHiddenEditFlipToolPointerPencil_Click );
            // 
            // menuHiddenEditFlipToolPointerEraser
            // 
            this.menuHiddenEditFlipToolPointerEraser.Name = "menuHiddenEditFlipToolPointerEraser";
            this.menuHiddenEditFlipToolPointerEraser.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.menuHiddenEditFlipToolPointerEraser.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenEditFlipToolPointerEraser.Text = "Change Tool Pointer/ Eraser";
            this.menuHiddenEditFlipToolPointerEraser.Visible = false;
            this.menuHiddenEditFlipToolPointerEraser.Click += new System.EventHandler( this.menuHiddenEditFlipToolPointerEraser_Click );
            // 
            // menuHiddenVisualForwardParameter
            // 
            this.menuHiddenVisualForwardParameter.Name = "menuHiddenVisualForwardParameter";
            this.menuHiddenVisualForwardParameter.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.Next)));
            this.menuHiddenVisualForwardParameter.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenVisualForwardParameter.Text = "Next Control Curve";
            this.menuHiddenVisualForwardParameter.Visible = false;
            this.menuHiddenVisualForwardParameter.Click += new System.EventHandler( this.menuHiddenVisualForwardParameter_Click );
            // 
            // menuHiddenVisualBackwardParameter
            // 
            this.menuHiddenVisualBackwardParameter.Name = "menuHiddenVisualBackwardParameter";
            this.menuHiddenVisualBackwardParameter.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.PageUp)));
            this.menuHiddenVisualBackwardParameter.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenVisualBackwardParameter.Text = "Previous Control Curve";
            this.menuHiddenVisualBackwardParameter.Visible = false;
            this.menuHiddenVisualBackwardParameter.Click += new System.EventHandler( this.menuHiddenVisualBackwardParameter_Click );
            // 
            // menuHiddenTrackNext
            // 
            this.menuHiddenTrackNext.Name = "menuHiddenTrackNext";
            this.menuHiddenTrackNext.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Next)));
            this.menuHiddenTrackNext.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenTrackNext.Text = "Next Track";
            this.menuHiddenTrackNext.Visible = false;
            this.menuHiddenTrackNext.Click += new System.EventHandler( this.menuHiddenTrackNext_Click );
            // 
            // menuHiddenTrackBack
            // 
            this.menuHiddenTrackBack.Name = "menuHiddenTrackBack";
            this.menuHiddenTrackBack.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.PageUp)));
            this.menuHiddenTrackBack.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenTrackBack.Text = "Previous Track";
            this.menuHiddenTrackBack.Visible = false;
            this.menuHiddenTrackBack.Click += new System.EventHandler( this.menuHiddenTrackBack_Click );
            // 
            // menuHiddenCopy
            // 
            this.menuHiddenCopy.Name = "menuHiddenCopy";
            this.menuHiddenCopy.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenCopy.Text = "Copy";
            this.menuHiddenCopy.Click += new System.EventHandler( this.commonEditCopy_Click );
            // 
            // menuHiddenPaste
            // 
            this.menuHiddenPaste.Name = "menuHiddenPaste";
            this.menuHiddenPaste.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenPaste.Text = "Paste";
            this.menuHiddenPaste.Click += new System.EventHandler( this.commonEditPaste_Click );
            // 
            // menuHiddenCut
            // 
            this.menuHiddenCut.Name = "menuHiddenCut";
            this.menuHiddenCut.Size = new System.Drawing.Size( 267, 22 );
            this.menuHiddenCut.Text = "Cut";
            this.menuHiddenCut.Click += new System.EventHandler( this.commonEditCut_Click );
            // 
            // saveXmlVsqDialog
            // 
            this.saveXmlVsqDialog.Filter = "VSQ Format(*.vsq)|*.vsq|Original Format(*.evsq)|*.evsq|All files(*.*)|*.*";
            // 
            // cMenuPiano
            // 
            this.cMenuPiano.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cMenuPianoPointer,
            this.cMenuPianoPencil,
            this.cMenuPianoEraser,
            this.cMenuPianoPaletteTool,
            this.toolStripSeparator15,
            this.cMenuPianoCurve,
            this.toolStripMenuItem13,
            this.cMenuPianoFixed,
            this.cMenuPianoQuantize,
            this.cMenuPianoLength,
            this.cMenuPianoGrid,
            this.toolStripMenuItem14,
            this.cMenuPianoUndo,
            this.cMenuPianoRedo,
            this.toolStripMenuItem15,
            this.cMenuPianoCut,
            this.cMenuPianoCopy,
            this.cMenuPianoPaste,
            this.cMenuPianoDelete,
            this.toolStripMenuItem16,
            this.cMenuPianoSelectAll,
            this.cMenuPianoSelectAllEvents,
            this.toolStripMenuItem17,
            this.cMenuPianoImportLyric,
            this.cMenuPianoExpressionProperty,
            this.cMenuPianoVibratoProperty} );
            this.cMenuPiano.Name = "cMenuPiano";
            this.cMenuPiano.ShowCheckMargin = true;
            this.cMenuPiano.ShowImageMargin = false;
            this.cMenuPiano.Size = new System.Drawing.Size( 217, 480 );
            this.cMenuPiano.Opening += new System.ComponentModel.CancelEventHandler( this.cMenuPiano_Opening );
            // 
            // cMenuPianoPointer
            // 
            this.cMenuPianoPointer.Name = "cMenuPianoPointer";
            this.cMenuPianoPointer.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoPointer.Text = "Arrow(&A)";
            this.cMenuPianoPointer.Click += new System.EventHandler( this.cMenuPianoPointer_Click );
            // 
            // cMenuPianoPencil
            // 
            this.cMenuPianoPencil.Name = "cMenuPianoPencil";
            this.cMenuPianoPencil.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoPencil.Text = "Pencil(&W)";
            this.cMenuPianoPencil.Click += new System.EventHandler( this.cMenuPianoPencil_Click );
            // 
            // cMenuPianoEraser
            // 
            this.cMenuPianoEraser.Name = "cMenuPianoEraser";
            this.cMenuPianoEraser.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoEraser.Text = "Eraser(&E)";
            this.cMenuPianoEraser.Click += new System.EventHandler( this.cMenuPianoEraser_Click );
            // 
            // cMenuPianoPaletteTool
            // 
            this.cMenuPianoPaletteTool.Name = "cMenuPianoPaletteTool";
            this.cMenuPianoPaletteTool.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoPaletteTool.Text = "Palette Tool";
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size( 213, 6 );
            // 
            // cMenuPianoCurve
            // 
            this.cMenuPianoCurve.Name = "cMenuPianoCurve";
            this.cMenuPianoCurve.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoCurve.Text = "Curve(&V)";
            this.cMenuPianoCurve.Click += new System.EventHandler( this.cMenuPianoCurve_Click );
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size( 213, 6 );
            // 
            // cMenuPianoFixed
            // 
            this.cMenuPianoFixed.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cMenuPianoFixed01,
            this.cMenuPianoFixed02,
            this.cMenuPianoFixed04,
            this.cMenuPianoFixed08,
            this.cMenuPianoFixed16,
            this.cMenuPianoFixed32,
            this.cMenuPianoFixed64,
            this.cMenuPianoFixed128,
            this.cMenuPianoFixedOff,
            this.toolStripMenuItem18,
            this.cMenuPianoFixedTriplet,
            this.cMenuPianoFixedDotted} );
            this.cMenuPianoFixed.Name = "cMenuPianoFixed";
            this.cMenuPianoFixed.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoFixed.Text = "Note Fixed Length(&N)";
            // 
            // cMenuPianoFixed01
            // 
            this.cMenuPianoFixed01.Name = "cMenuPianoFixed01";
            this.cMenuPianoFixed01.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed01.Text = "1/ 1 [1920]";
            this.cMenuPianoFixed01.Click += new System.EventHandler( this.cMenuPianoFixed01_Click );
            // 
            // cMenuPianoFixed02
            // 
            this.cMenuPianoFixed02.Name = "cMenuPianoFixed02";
            this.cMenuPianoFixed02.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed02.Text = "1/ 2 [960]";
            this.cMenuPianoFixed02.Click += new System.EventHandler( this.cMenuPianoFixed02_Click );
            // 
            // cMenuPianoFixed04
            // 
            this.cMenuPianoFixed04.Name = "cMenuPianoFixed04";
            this.cMenuPianoFixed04.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed04.Text = "1/ 4 [480]";
            this.cMenuPianoFixed04.Click += new System.EventHandler( this.cMenuPianoFixed04_Click );
            // 
            // cMenuPianoFixed08
            // 
            this.cMenuPianoFixed08.Name = "cMenuPianoFixed08";
            this.cMenuPianoFixed08.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed08.Text = "1/ 8 [240]";
            this.cMenuPianoFixed08.Click += new System.EventHandler( this.cMenuPianoFixed08_Click );
            // 
            // cMenuPianoFixed16
            // 
            this.cMenuPianoFixed16.Name = "cMenuPianoFixed16";
            this.cMenuPianoFixed16.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed16.Text = "1/16 [120]";
            this.cMenuPianoFixed16.Click += new System.EventHandler( this.cMenuPianoFixed16_Click );
            // 
            // cMenuPianoFixed32
            // 
            this.cMenuPianoFixed32.Name = "cMenuPianoFixed32";
            this.cMenuPianoFixed32.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed32.Text = "1/32 [60]";
            this.cMenuPianoFixed32.Click += new System.EventHandler( this.cMenuPianoFixed32_Click );
            // 
            // cMenuPianoFixed64
            // 
            this.cMenuPianoFixed64.Name = "cMenuPianoFixed64";
            this.cMenuPianoFixed64.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed64.Text = "1/64 [30]";
            this.cMenuPianoFixed64.Click += new System.EventHandler( this.cMenuPianoFixed64_Click );
            // 
            // cMenuPianoFixed128
            // 
            this.cMenuPianoFixed128.Name = "cMenuPianoFixed128";
            this.cMenuPianoFixed128.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixed128.Text = "1/128[15]";
            this.cMenuPianoFixed128.Click += new System.EventHandler( this.cMenuPianoFixed128_Click );
            // 
            // cMenuPianoFixedOff
            // 
            this.cMenuPianoFixedOff.Name = "cMenuPianoFixedOff";
            this.cMenuPianoFixedOff.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixedOff.Text = "オフ";
            this.cMenuPianoFixedOff.Click += new System.EventHandler( this.cMenuPianoFixedOff_Click );
            // 
            // toolStripMenuItem18
            // 
            this.toolStripMenuItem18.Name = "toolStripMenuItem18";
            this.toolStripMenuItem18.Size = new System.Drawing.Size( 125, 6 );
            // 
            // cMenuPianoFixedTriplet
            // 
            this.cMenuPianoFixedTriplet.Name = "cMenuPianoFixedTriplet";
            this.cMenuPianoFixedTriplet.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixedTriplet.Text = "3連符";
            this.cMenuPianoFixedTriplet.Click += new System.EventHandler( this.cMenuPianoFixedTriplet_Click );
            // 
            // cMenuPianoFixedDotted
            // 
            this.cMenuPianoFixedDotted.Name = "cMenuPianoFixedDotted";
            this.cMenuPianoFixedDotted.Size = new System.Drawing.Size( 128, 22 );
            this.cMenuPianoFixedDotted.Text = "付点";
            this.cMenuPianoFixedDotted.Click += new System.EventHandler( this.cMenuPianoFixedDotted_Click );
            // 
            // cMenuPianoQuantize
            // 
            this.cMenuPianoQuantize.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cMenuPianoQuantize04,
            this.cMenuPianoQuantize08,
            this.cMenuPianoQuantize16,
            this.cMenuPianoQuantize32,
            this.cMenuPianoQuantize64,
            this.cMenuPianoQuantize128,
            this.cMenuPianoQuantizeOff,
            this.toolStripMenuItem26,
            this.cMenuPianoQuantizeTriplet} );
            this.cMenuPianoQuantize.Name = "cMenuPianoQuantize";
            this.cMenuPianoQuantize.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoQuantize.Text = "Quantize(&Q)";
            // 
            // cMenuPianoQuantize04
            // 
            this.cMenuPianoQuantize04.Name = "cMenuPianoQuantize04";
            this.cMenuPianoQuantize04.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantize04.Text = "1/4";
            this.cMenuPianoQuantize04.Click += new System.EventHandler( this.h_positionQuantize04 );
            // 
            // cMenuPianoQuantize08
            // 
            this.cMenuPianoQuantize08.Name = "cMenuPianoQuantize08";
            this.cMenuPianoQuantize08.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantize08.Text = "1/8";
            this.cMenuPianoQuantize08.Click += new System.EventHandler( this.h_positionQuantize08 );
            // 
            // cMenuPianoQuantize16
            // 
            this.cMenuPianoQuantize16.Name = "cMenuPianoQuantize16";
            this.cMenuPianoQuantize16.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantize16.Text = "1/16";
            this.cMenuPianoQuantize16.Click += new System.EventHandler( this.h_positionQuantize16 );
            // 
            // cMenuPianoQuantize32
            // 
            this.cMenuPianoQuantize32.Name = "cMenuPianoQuantize32";
            this.cMenuPianoQuantize32.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantize32.Text = "1/32";
            this.cMenuPianoQuantize32.Click += new System.EventHandler( this.h_positionQuantize32 );
            // 
            // cMenuPianoQuantize64
            // 
            this.cMenuPianoQuantize64.Name = "cMenuPianoQuantize64";
            this.cMenuPianoQuantize64.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantize64.Text = "1/64";
            this.cMenuPianoQuantize64.Click += new System.EventHandler( this.h_positionQuantize64 );
            // 
            // cMenuPianoQuantize128
            // 
            this.cMenuPianoQuantize128.Name = "cMenuPianoQuantize128";
            this.cMenuPianoQuantize128.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantize128.Text = "1/128";
            this.cMenuPianoQuantize128.Click += new System.EventHandler( this.h_positionQuantize128 );
            // 
            // cMenuPianoQuantizeOff
            // 
            this.cMenuPianoQuantizeOff.Name = "cMenuPianoQuantizeOff";
            this.cMenuPianoQuantizeOff.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantizeOff.Text = "オフ";
            this.cMenuPianoQuantizeOff.Click += new System.EventHandler( this.h_positionQuantizeOff );
            // 
            // toolStripMenuItem26
            // 
            this.toolStripMenuItem26.Name = "toolStripMenuItem26";
            this.toolStripMenuItem26.Size = new System.Drawing.Size( 97, 6 );
            // 
            // cMenuPianoQuantizeTriplet
            // 
            this.cMenuPianoQuantizeTriplet.Name = "cMenuPianoQuantizeTriplet";
            this.cMenuPianoQuantizeTriplet.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoQuantizeTriplet.Text = "3連符";
            this.cMenuPianoQuantizeTriplet.Click += new System.EventHandler( this.h_positionQuantizeTriplet );
            // 
            // cMenuPianoLength
            // 
            this.cMenuPianoLength.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cMenuPianoLength04,
            this.cMenuPianoLength08,
            this.cMenuPianoLength16,
            this.cMenuPianoLength32,
            this.cMenuPianoLength64,
            this.cMenuPianoLength128,
            this.cMenuPianoLengthOff,
            this.toolStripMenuItem32,
            this.cMenuPianoLengthTriplet} );
            this.cMenuPianoLength.Name = "cMenuPianoLength";
            this.cMenuPianoLength.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoLength.Text = "Length(&L)";
            // 
            // cMenuPianoLength04
            // 
            this.cMenuPianoLength04.Name = "cMenuPianoLength04";
            this.cMenuPianoLength04.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLength04.Text = "1/4";
            this.cMenuPianoLength04.Click += new System.EventHandler( this.h_lengthQuantize04 );
            // 
            // cMenuPianoLength08
            // 
            this.cMenuPianoLength08.Name = "cMenuPianoLength08";
            this.cMenuPianoLength08.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLength08.Text = "1/8";
            this.cMenuPianoLength08.Click += new System.EventHandler( this.h_lengthQuantize08 );
            // 
            // cMenuPianoLength16
            // 
            this.cMenuPianoLength16.Name = "cMenuPianoLength16";
            this.cMenuPianoLength16.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLength16.Text = "1/16";
            this.cMenuPianoLength16.Click += new System.EventHandler( this.h_lengthQuantize16 );
            // 
            // cMenuPianoLength32
            // 
            this.cMenuPianoLength32.Name = "cMenuPianoLength32";
            this.cMenuPianoLength32.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLength32.Text = "1/32";
            this.cMenuPianoLength32.Click += new System.EventHandler( this.h_lengthQuantize32 );
            // 
            // cMenuPianoLength64
            // 
            this.cMenuPianoLength64.Name = "cMenuPianoLength64";
            this.cMenuPianoLength64.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLength64.Text = "1/64";
            this.cMenuPianoLength64.Click += new System.EventHandler( this.h_lengthQuantize64 );
            // 
            // cMenuPianoLength128
            // 
            this.cMenuPianoLength128.Name = "cMenuPianoLength128";
            this.cMenuPianoLength128.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLength128.Text = "1/128";
            this.cMenuPianoLength128.Click += new System.EventHandler( this.h_lengthQuantize128 );
            // 
            // cMenuPianoLengthOff
            // 
            this.cMenuPianoLengthOff.Name = "cMenuPianoLengthOff";
            this.cMenuPianoLengthOff.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLengthOff.Text = "オフ";
            this.cMenuPianoLengthOff.Click += new System.EventHandler( this.h_lengthQuantizeOff );
            // 
            // toolStripMenuItem32
            // 
            this.toolStripMenuItem32.Name = "toolStripMenuItem32";
            this.toolStripMenuItem32.Size = new System.Drawing.Size( 97, 6 );
            // 
            // cMenuPianoLengthTriplet
            // 
            this.cMenuPianoLengthTriplet.Name = "cMenuPianoLengthTriplet";
            this.cMenuPianoLengthTriplet.Size = new System.Drawing.Size( 100, 22 );
            this.cMenuPianoLengthTriplet.Text = "3連符";
            this.cMenuPianoLengthTriplet.Click += new System.EventHandler( this.h_lengthQuantizeTriplet );
            // 
            // cMenuPianoGrid
            // 
            this.cMenuPianoGrid.Name = "cMenuPianoGrid";
            this.cMenuPianoGrid.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoGrid.Text = "Show/Hide Grid Line(&S)";
            this.cMenuPianoGrid.Click += new System.EventHandler( this.cMenuPianoGrid_Click );
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size( 213, 6 );
            // 
            // cMenuPianoUndo
            // 
            this.cMenuPianoUndo.Name = "cMenuPianoUndo";
            this.cMenuPianoUndo.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoUndo.Text = "Undo(&U)";
            this.cMenuPianoUndo.Click += new System.EventHandler( this.cMenuPianoUndo_Click );
            // 
            // cMenuPianoRedo
            // 
            this.cMenuPianoRedo.Name = "cMenuPianoRedo";
            this.cMenuPianoRedo.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoRedo.Text = "Redo(&R)";
            this.cMenuPianoRedo.Click += new System.EventHandler( this.cMenuPianoRedo_Click );
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size( 213, 6 );
            // 
            // cMenuPianoCut
            // 
            this.cMenuPianoCut.Name = "cMenuPianoCut";
            this.cMenuPianoCut.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoCut.Text = "Cut(&T)";
            this.cMenuPianoCut.Click += new System.EventHandler( this.cMenuPianoCut_Click );
            // 
            // cMenuPianoCopy
            // 
            this.cMenuPianoCopy.Name = "cMenuPianoCopy";
            this.cMenuPianoCopy.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoCopy.Text = "Copy(&C)";
            this.cMenuPianoCopy.Click += new System.EventHandler( this.cMenuPianoCopy_Click );
            // 
            // cMenuPianoPaste
            // 
            this.cMenuPianoPaste.Name = "cMenuPianoPaste";
            this.cMenuPianoPaste.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoPaste.Text = "Paste(&P)";
            this.cMenuPianoPaste.Click += new System.EventHandler( this.cMenuPianoPaste_Click );
            // 
            // cMenuPianoDelete
            // 
            this.cMenuPianoDelete.Name = "cMenuPianoDelete";
            this.cMenuPianoDelete.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoDelete.Text = "Delete(&D)";
            this.cMenuPianoDelete.Click += new System.EventHandler( this.cMenuPianoDelete_Click );
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Size = new System.Drawing.Size( 213, 6 );
            // 
            // cMenuPianoSelectAll
            // 
            this.cMenuPianoSelectAll.Name = "cMenuPianoSelectAll";
            this.cMenuPianoSelectAll.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoSelectAll.Text = "Select All(&A)";
            this.cMenuPianoSelectAll.Click += new System.EventHandler( this.cMenuPianoSelectAll_Click );
            // 
            // cMenuPianoSelectAllEvents
            // 
            this.cMenuPianoSelectAllEvents.Name = "cMenuPianoSelectAllEvents";
            this.cMenuPianoSelectAllEvents.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoSelectAllEvents.Text = "Select All Events(&E)";
            this.cMenuPianoSelectAllEvents.Click += new System.EventHandler( this.cMenuPianoSelectAllEvents_Click );
            // 
            // toolStripMenuItem17
            // 
            this.toolStripMenuItem17.Name = "toolStripMenuItem17";
            this.toolStripMenuItem17.Size = new System.Drawing.Size( 213, 6 );
            // 
            // cMenuPianoImportLyric
            // 
            this.cMenuPianoImportLyric.Name = "cMenuPianoImportLyric";
            this.cMenuPianoImportLyric.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoImportLyric.Text = "Insert Lyrics(&L)";
            this.cMenuPianoImportLyric.Click += new System.EventHandler( this.cMenuPianoImportLyric_Click );
            // 
            // cMenuPianoExpressionProperty
            // 
            this.cMenuPianoExpressionProperty.Name = "cMenuPianoExpressionProperty";
            this.cMenuPianoExpressionProperty.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoExpressionProperty.Text = "Note Expression Property(&P)";
            this.cMenuPianoExpressionProperty.Click += new System.EventHandler( this.cMenuPianoProperty_Click );
            // 
            // cMenuPianoVibratoProperty
            // 
            this.cMenuPianoVibratoProperty.Name = "cMenuPianoVibratoProperty";
            this.cMenuPianoVibratoProperty.Size = new System.Drawing.Size( 216, 22 );
            this.cMenuPianoVibratoProperty.Text = "Note Vibrato Property";
            this.cMenuPianoVibratoProperty.Click += new System.EventHandler( this.cMenuPianoVibratoProperty_Click );
            // 
            // openXmlVsqDialog
            // 
            this.openXmlVsqDialog.Filter = "VSQ Format(*.vsq)|*.vsq|Original Format(*.evsq)|*.evsq";
            // 
            // cMenuTrackTab
            // 
            this.cMenuTrackTab.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cMenuTrackTabTrackOn,
            this.toolStripMenuItem24,
            this.cMenuTrackTabAdd,
            this.cMenuTrackTabCopy,
            this.cMenuTrackTabChangeName,
            this.cMenuTrackTabDelete,
            this.toolStripMenuItem25,
            this.cMenuTrackTabRenderCurrent,
            this.cMenuTrackTabRenderAll,
            this.toolStripMenuItem27,
            this.cMenuTrackTabOverlay,
            this.cMenuTrackTabRenderer} );
            this.cMenuTrackTab.Name = "cMenuTrackTab";
            this.cMenuTrackTab.Size = new System.Drawing.Size( 197, 220 );
            this.cMenuTrackTab.Opening += new System.ComponentModel.CancelEventHandler( this.cMenuTrackTab_Opening );
            // 
            // cMenuTrackTabTrackOn
            // 
            this.cMenuTrackTabTrackOn.Name = "cMenuTrackTabTrackOn";
            this.cMenuTrackTabTrackOn.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabTrackOn.Text = "Track On(&K)";
            this.cMenuTrackTabTrackOn.Click += new System.EventHandler( this.cMenuTrackTabTrackOn_Click );
            // 
            // toolStripMenuItem24
            // 
            this.toolStripMenuItem24.Name = "toolStripMenuItem24";
            this.toolStripMenuItem24.Size = new System.Drawing.Size( 193, 6 );
            // 
            // cMenuTrackTabAdd
            // 
            this.cMenuTrackTabAdd.Name = "cMenuTrackTabAdd";
            this.cMenuTrackTabAdd.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabAdd.Text = "Add Track(&A)";
            this.cMenuTrackTabAdd.Click += new System.EventHandler( this.cMenuTrackTabAdd_Click );
            // 
            // cMenuTrackTabCopy
            // 
            this.cMenuTrackTabCopy.Name = "cMenuTrackTabCopy";
            this.cMenuTrackTabCopy.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabCopy.Text = "Copy Track(&C)";
            this.cMenuTrackTabCopy.Click += new System.EventHandler( this.cMenuTrackTabCopy_Click );
            // 
            // cMenuTrackTabChangeName
            // 
            this.cMenuTrackTabChangeName.Name = "cMenuTrackTabChangeName";
            this.cMenuTrackTabChangeName.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabChangeName.Text = "Rename Track(&R)";
            this.cMenuTrackTabChangeName.Click += new System.EventHandler( this.cMenuTrackTabChangeName_Click );
            // 
            // cMenuTrackTabDelete
            // 
            this.cMenuTrackTabDelete.Name = "cMenuTrackTabDelete";
            this.cMenuTrackTabDelete.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabDelete.Text = "Delete Track(&D)";
            this.cMenuTrackTabDelete.Click += new System.EventHandler( this.cMenuTrackTabDelete_Click );
            // 
            // toolStripMenuItem25
            // 
            this.toolStripMenuItem25.Name = "toolStripMenuItem25";
            this.toolStripMenuItem25.Size = new System.Drawing.Size( 193, 6 );
            // 
            // cMenuTrackTabRenderCurrent
            // 
            this.cMenuTrackTabRenderCurrent.Name = "cMenuTrackTabRenderCurrent";
            this.cMenuTrackTabRenderCurrent.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabRenderCurrent.Text = "Render Current Track(&T)";
            this.cMenuTrackTabRenderCurrent.Click += new System.EventHandler( this.cMenuTrackTabRenderCurrent_Click );
            // 
            // cMenuTrackTabRenderAll
            // 
            this.cMenuTrackTabRenderAll.Name = "cMenuTrackTabRenderAll";
            this.cMenuTrackTabRenderAll.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabRenderAll.Text = "Render All Tracks(&S)";
            this.cMenuTrackTabRenderAll.Click += new System.EventHandler( this.commonTrackRenderAll_Click );
            // 
            // toolStripMenuItem27
            // 
            this.toolStripMenuItem27.Name = "toolStripMenuItem27";
            this.toolStripMenuItem27.Size = new System.Drawing.Size( 193, 6 );
            // 
            // cMenuTrackTabOverlay
            // 
            this.cMenuTrackTabOverlay.Name = "cMenuTrackTabOverlay";
            this.cMenuTrackTabOverlay.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabOverlay.Text = "Overlay(&O)";
            this.cMenuTrackTabOverlay.Click += new System.EventHandler( this.cMenuTrackTabOverlay_Click );
            // 
            // cMenuTrackTabRenderer
            // 
            this.cMenuTrackTabRenderer.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cMenuTrackTabRendererVOCALOID1,
            this.cMenuTrackTabRendererVOCALOID2,
            this.cMenuTrackTabRendererUtau,
            this.cMenuTrackTabRendererStraight} );
            this.cMenuTrackTabRenderer.Name = "cMenuTrackTabRenderer";
            this.cMenuTrackTabRenderer.Size = new System.Drawing.Size( 196, 22 );
            this.cMenuTrackTabRenderer.Text = "Renderer";
            this.cMenuTrackTabRenderer.DropDownOpening += new System.EventHandler( this.cMenuTrackTabRenderer_DropDownOpening );
            // 
            // cMenuTrackTabRendererVOCALOID1
            // 
            this.cMenuTrackTabRendererVOCALOID1.Name = "cMenuTrackTabRendererVOCALOID1";
            this.cMenuTrackTabRendererVOCALOID1.Size = new System.Drawing.Size( 160, 22 );
            this.cMenuTrackTabRendererVOCALOID1.Text = "VOCALOID1";
            this.cMenuTrackTabRendererVOCALOID1.Click += new System.EventHandler( this.commonRendererVOCALOID1_Click );
            // 
            // cMenuTrackTabRendererVOCALOID2
            // 
            this.cMenuTrackTabRendererVOCALOID2.Name = "cMenuTrackTabRendererVOCALOID2";
            this.cMenuTrackTabRendererVOCALOID2.Size = new System.Drawing.Size( 160, 22 );
            this.cMenuTrackTabRendererVOCALOID2.Text = "VOCALOID2";
            this.cMenuTrackTabRendererVOCALOID2.Click += new System.EventHandler( this.commonRendererVOCALOID2_Click );
            // 
            // cMenuTrackTabRendererUtau
            // 
            this.cMenuTrackTabRendererUtau.Name = "cMenuTrackTabRendererUtau";
            this.cMenuTrackTabRendererUtau.Size = new System.Drawing.Size( 160, 22 );
            this.cMenuTrackTabRendererUtau.Text = "UTAU";
            this.cMenuTrackTabRendererUtau.Click += new System.EventHandler( this.commonRendererUtau_Click );
            // 
            // cMenuTrackTabRendererStraight
            // 
            this.cMenuTrackTabRendererStraight.Name = "cMenuTrackTabRendererStraight";
            this.cMenuTrackTabRendererStraight.Size = new System.Drawing.Size( 160, 22 );
            this.cMenuTrackTabRendererStraight.Text = "Straight X UTAU ";
            this.cMenuTrackTabRendererStraight.Click += new System.EventHandler( this.commonRendererStraight_Click );
            // 
            // cMenuTrackSelector
            // 
            this.cMenuTrackSelector.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cMenuTrackSelectorPointer,
            this.cMenuTrackSelectorPencil,
            this.cMenuTrackSelectorLine,
            this.cMenuTrackSelectorEraser,
            this.cMenuTrackSelectorPaletteTool,
            this.toolStripSeparator14,
            this.cMenuTrackSelectorCurve,
            this.toolStripMenuItem28,
            this.cMenuTrackSelectorUndo,
            this.cMenuTrackSelectorRedo,
            this.toolStripMenuItem29,
            this.cMenuTrackSelectorCut,
            this.cMenuTrackSelectorCopy,
            this.cMenuTrackSelectorPaste,
            this.cMenuTrackSelectorDelete,
            this.cMenuTrackSelectorDeleteBezier,
            this.toolStripMenuItem31,
            this.cMenuTrackSelectorSelectAll} );
            this.cMenuTrackSelector.Name = "cMenuTrackSelector";
            this.cMenuTrackSelector.Size = new System.Drawing.Size( 186, 336 );
            this.cMenuTrackSelector.Opening += new System.ComponentModel.CancelEventHandler( this.cMenuTrackSelector_Opening );
            // 
            // cMenuTrackSelectorPointer
            // 
            this.cMenuTrackSelectorPointer.Name = "cMenuTrackSelectorPointer";
            this.cMenuTrackSelectorPointer.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorPointer.Text = "Arrow(&A)";
            this.cMenuTrackSelectorPointer.Click += new System.EventHandler( this.cMenuTrackSelectorPointer_Click );
            // 
            // cMenuTrackSelectorPencil
            // 
            this.cMenuTrackSelectorPencil.Name = "cMenuTrackSelectorPencil";
            this.cMenuTrackSelectorPencil.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorPencil.Text = "Pencil(&W)";
            this.cMenuTrackSelectorPencil.Click += new System.EventHandler( this.cMenuTrackSelectorPencil_Click );
            // 
            // cMenuTrackSelectorLine
            // 
            this.cMenuTrackSelectorLine.Name = "cMenuTrackSelectorLine";
            this.cMenuTrackSelectorLine.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorLine.Text = "Line(&L)";
            this.cMenuTrackSelectorLine.Click += new System.EventHandler( this.cMenuTrackSelectorLine_Click );
            // 
            // cMenuTrackSelectorEraser
            // 
            this.cMenuTrackSelectorEraser.Name = "cMenuTrackSelectorEraser";
            this.cMenuTrackSelectorEraser.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorEraser.Text = "Eraser(&E)";
            this.cMenuTrackSelectorEraser.Click += new System.EventHandler( this.cMenuTrackSelectorEraser_Click );
            // 
            // cMenuTrackSelectorPaletteTool
            // 
            this.cMenuTrackSelectorPaletteTool.Name = "cMenuTrackSelectorPaletteTool";
            this.cMenuTrackSelectorPaletteTool.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorPaletteTool.Text = "Palette Tool";
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size( 182, 6 );
            // 
            // cMenuTrackSelectorCurve
            // 
            this.cMenuTrackSelectorCurve.Name = "cMenuTrackSelectorCurve";
            this.cMenuTrackSelectorCurve.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorCurve.Text = "Curve(&V)";
            this.cMenuTrackSelectorCurve.Click += new System.EventHandler( this.cMenuTrackSelectorCurve_Click );
            // 
            // toolStripMenuItem28
            // 
            this.toolStripMenuItem28.Name = "toolStripMenuItem28";
            this.toolStripMenuItem28.Size = new System.Drawing.Size( 182, 6 );
            // 
            // cMenuTrackSelectorUndo
            // 
            this.cMenuTrackSelectorUndo.Name = "cMenuTrackSelectorUndo";
            this.cMenuTrackSelectorUndo.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorUndo.Text = "Undo(&U)";
            this.cMenuTrackSelectorUndo.Click += new System.EventHandler( this.cMenuTrackSelectorUndo_Click );
            // 
            // cMenuTrackSelectorRedo
            // 
            this.cMenuTrackSelectorRedo.Name = "cMenuTrackSelectorRedo";
            this.cMenuTrackSelectorRedo.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorRedo.Text = "Redo(&R)";
            this.cMenuTrackSelectorRedo.Click += new System.EventHandler( this.cMenuTrackSelectorRedo_Click );
            // 
            // toolStripMenuItem29
            // 
            this.toolStripMenuItem29.Name = "toolStripMenuItem29";
            this.toolStripMenuItem29.Size = new System.Drawing.Size( 182, 6 );
            // 
            // cMenuTrackSelectorCut
            // 
            this.cMenuTrackSelectorCut.Name = "cMenuTrackSelectorCut";
            this.cMenuTrackSelectorCut.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorCut.Text = "Cut(&T)";
            this.cMenuTrackSelectorCut.Click += new System.EventHandler( this.cMenuTrackSelectorCut_Click );
            // 
            // cMenuTrackSelectorCopy
            // 
            this.cMenuTrackSelectorCopy.Name = "cMenuTrackSelectorCopy";
            this.cMenuTrackSelectorCopy.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorCopy.Text = "Copy(&C)";
            this.cMenuTrackSelectorCopy.Click += new System.EventHandler( this.cMenuTrackSelectorCopy_Click );
            // 
            // cMenuTrackSelectorPaste
            // 
            this.cMenuTrackSelectorPaste.Name = "cMenuTrackSelectorPaste";
            this.cMenuTrackSelectorPaste.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorPaste.Text = "Paste(&P)";
            this.cMenuTrackSelectorPaste.Click += new System.EventHandler( this.cMenuTrackSelectorPaste_Click );
            // 
            // cMenuTrackSelectorDelete
            // 
            this.cMenuTrackSelectorDelete.Name = "cMenuTrackSelectorDelete";
            this.cMenuTrackSelectorDelete.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorDelete.Text = "Delete(&D)";
            this.cMenuTrackSelectorDelete.Click += new System.EventHandler( this.cMenuTrackSelectorDelete_Click );
            // 
            // cMenuTrackSelectorDeleteBezier
            // 
            this.cMenuTrackSelectorDeleteBezier.Name = "cMenuTrackSelectorDeleteBezier";
            this.cMenuTrackSelectorDeleteBezier.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorDeleteBezier.Text = "Delete Bezier Point(&B)";
            this.cMenuTrackSelectorDeleteBezier.Click += new System.EventHandler( this.cMenuTrackSelectorDeleteBezier_Click );
            // 
            // toolStripMenuItem31
            // 
            this.toolStripMenuItem31.Name = "toolStripMenuItem31";
            this.toolStripMenuItem31.Size = new System.Drawing.Size( 182, 6 );
            // 
            // cMenuTrackSelectorSelectAll
            // 
            this.cMenuTrackSelectorSelectAll.Name = "cMenuTrackSelectorSelectAll";
            this.cMenuTrackSelectorSelectAll.Size = new System.Drawing.Size( 185, 22 );
            this.cMenuTrackSelectorSelectAll.Text = "Select All Events(&E)";
            this.cMenuTrackSelectorSelectAll.Click += new System.EventHandler( this.cMenuTrackSelectorSelectAll_Click );
            // 
            // trackBar
            // 
            this.trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar.AutoSize = false;
            this.trackBar.Location = new System.Drawing.Point( 322, 266 );
            this.trackBar.Margin = new System.Windows.Forms.Padding( 0 );
            this.trackBar.Maximum = 609;
            this.trackBar.Minimum = 17;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size( 83, 16 );
            this.trackBar.TabIndex = 15;
            this.trackBar.TabStop = false;
            this.trackBar.TickFrequency = 100;
            this.trackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar.Value = 17;
            this.trackBar.ValueChanged += new System.EventHandler( this.trackBar_ValueChanged );
            this.trackBar.MouseDown += new System.Windows.Forms.MouseEventHandler( this.trackBar_MouseDown );
            this.trackBar.Enter += new System.EventHandler( this.trackBar_Enter );
            // 
            // bgWorkScreen
            // 
            this.bgWorkScreen.DoWork += new System.ComponentModel.DoWorkEventHandler( this.bgWorkScreen_DoWork );
            // 
            // timer
            // 
            this.timer.Interval = 200;
            this.timer.Tick += new System.EventHandler( this.timer_Tick );
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.panel3 );
            this.panel1.Controls.Add( this.vScroll );
            this.panel1.Controls.Add( this.hScroll );
            this.panel1.Controls.Add( this.picturePositionIndicator );
            this.panel1.Controls.Add( this.pictPianoRoll );
            this.panel1.Controls.Add( this.pictureBox3 );
            this.panel1.Controls.Add( this.trackBar );
            this.panel1.Controls.Add( this.pictureBox2 );
            this.panel1.Location = new System.Drawing.Point( 3, 3 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 421, 282 );
            this.panel1.TabIndex = 16;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add( this.btnRight1 );
            this.panel3.Controls.Add( this.btnLeft2 );
            this.panel3.Controls.Add( this.btnZoom );
            this.panel3.Controls.Add( this.btnMooz );
            this.panel3.Controls.Add( this.btnLeft1 );
            this.panel3.Controls.Add( this.btnRight2 );
            this.panel3.Controls.Add( this.pictOverview );
            this.panel3.Location = new System.Drawing.Point( 0, 0 );
            this.panel3.Margin = new System.Windows.Forms.Padding( 0 );
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size( 421, 45 );
            this.panel3.TabIndex = 19;
            // 
            // btnRight1
            // 
            this.btnRight1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRight1.Location = new System.Drawing.Point( 52, 23 );
            this.btnRight1.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnRight1.Name = "btnRight1";
            this.btnRight1.Size = new System.Drawing.Size( 16, 22 );
            this.btnRight1.TabIndex = 24;
            this.btnRight1.Text = ">";
            this.btnRight1.UseVisualStyleBackColor = true;
            this.btnRight1.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnRight_MouseDown );
            this.btnRight1.MouseUp += new System.Windows.Forms.MouseEventHandler( this.btnRight_MouseUp );
            // 
            // btnLeft2
            // 
            this.btnLeft2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLeft2.Location = new System.Drawing.Point( 405, 0 );
            this.btnLeft2.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnLeft2.Name = "btnLeft2";
            this.btnLeft2.Size = new System.Drawing.Size( 16, 22 );
            this.btnLeft2.TabIndex = 23;
            this.btnLeft2.Text = "<";
            this.btnLeft2.UseVisualStyleBackColor = true;
            this.btnLeft2.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnLeft_MouseDown );
            this.btnLeft2.MouseUp += new System.Windows.Forms.MouseEventHandler( this.btnLeft_MouseUp );
            // 
            // btnZoom
            // 
            this.btnZoom.Location = new System.Drawing.Point( 26, 12 );
            this.btnZoom.Name = "btnZoom";
            this.btnZoom.Size = new System.Drawing.Size( 23, 23 );
            this.btnZoom.TabIndex = 22;
            this.btnZoom.Text = "+";
            this.btnZoom.UseVisualStyleBackColor = true;
            this.btnZoom.Click += new System.EventHandler( this.btnZoom_Click );
            // 
            // btnMooz
            // 
            this.btnMooz.Location = new System.Drawing.Point( 3, 12 );
            this.btnMooz.Name = "btnMooz";
            this.btnMooz.Size = new System.Drawing.Size( 23, 23 );
            this.btnMooz.TabIndex = 21;
            this.btnMooz.Text = "-";
            this.btnMooz.UseVisualStyleBackColor = true;
            this.btnMooz.Click += new System.EventHandler( this.btnMooz_Click );
            // 
            // btnLeft1
            // 
            this.btnLeft1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLeft1.Location = new System.Drawing.Point( 52, 0 );
            this.btnLeft1.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnLeft1.Name = "btnLeft1";
            this.btnLeft1.Size = new System.Drawing.Size( 16, 23 );
            this.btnLeft1.TabIndex = 20;
            this.btnLeft1.Text = "<";
            this.btnLeft1.UseVisualStyleBackColor = true;
            this.btnLeft1.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnLeft_MouseDown );
            this.btnLeft1.MouseUp += new System.Windows.Forms.MouseEventHandler( this.btnLeft_MouseUp );
            // 
            // btnRight2
            // 
            this.btnRight2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRight2.Location = new System.Drawing.Point( 405, 22 );
            this.btnRight2.Margin = new System.Windows.Forms.Padding( 0 );
            this.btnRight2.Name = "btnRight2";
            this.btnRight2.Size = new System.Drawing.Size( 16, 23 );
            this.btnRight2.TabIndex = 19;
            this.btnRight2.Text = ">";
            this.btnRight2.UseVisualStyleBackColor = true;
            this.btnRight2.MouseDown += new System.Windows.Forms.MouseEventHandler( this.btnRight_MouseDown );
            this.btnRight2.MouseUp += new System.Windows.Forms.MouseEventHandler( this.btnRight_MouseUp );
            // 
            // pictOverview
            // 
            this.pictOverview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictOverview.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(106)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))) );
            this.pictOverview.Location = new System.Drawing.Point( 68, 0 );
            this.pictOverview.Margin = new System.Windows.Forms.Padding( 0 );
            this.pictOverview.Name = "pictOverview";
            this.pictOverview.Size = new System.Drawing.Size( 337, 45 );
            this.pictOverview.TabIndex = 18;
            this.pictOverview.TabStop = false;
            this.pictOverview.MouseMove += new System.Windows.Forms.MouseEventHandler( this.pictOverview_MouseMove );
            this.pictOverview.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( this.pictOverview_MouseDoubleClick );
            this.pictOverview.MouseDown += new System.Windows.Forms.MouseEventHandler( this.pictOverview_MouseDown );
            this.pictOverview.Paint += new System.Windows.Forms.PaintEventHandler( this.pictOverview_Paint );
            this.pictOverview.MouseUp += new System.Windows.Forms.MouseEventHandler( this.pictOverview_MouseUp );
            // 
            // vScroll
            // 
            this.vScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScroll.LargeChange = 10;
            this.vScroll.Location = new System.Drawing.Point( 405, 93 );
            this.vScroll.Margin = new System.Windows.Forms.Padding( 0 );
            this.vScroll.Maximum = 100;
            this.vScroll.Minimum = 0;
            this.vScroll.Name = "vScroll";
            this.vScroll.Size = new System.Drawing.Size( 16, 173 );
            this.vScroll.SmallChange = 1;
            this.vScroll.TabIndex = 17;
            this.vScroll.Value = 0;
            this.vScroll.ValueChanged += new System.EventHandler( this.vScroll_ValueChanged );
            this.vScroll.Resize += new System.EventHandler( this.vScroll_Resize );
            this.vScroll.Enter += new System.EventHandler( this.vScroll_Enter );
            // 
            // hScroll
            // 
            this.hScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hScroll.LargeChange = 10;
            this.hScroll.Location = new System.Drawing.Point( 66, 266 );
            this.hScroll.Margin = new System.Windows.Forms.Padding( 0 );
            this.hScroll.Maximum = 100;
            this.hScroll.Minimum = 0;
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new System.Drawing.Size( 256, 16 );
            this.hScroll.SmallChange = 1;
            this.hScroll.TabIndex = 16;
            this.hScroll.Value = 0;
            this.hScroll.ValueChanged += new System.EventHandler( this.hScroll_ValueChanged );
            this.hScroll.Resize += new System.EventHandler( this.hScroll_Resize );
            this.hScroll.Enter += new System.EventHandler( this.hScroll_Enter );
            // 
            // picturePositionIndicator
            // 
            this.picturePositionIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picturePositionIndicator.BackColor = System.Drawing.Color.DarkGray;
            this.picturePositionIndicator.Location = new System.Drawing.Point( 0, 45 );
            this.picturePositionIndicator.Margin = new System.Windows.Forms.Padding( 0 );
            this.picturePositionIndicator.Name = "picturePositionIndicator";
            this.picturePositionIndicator.Size = new System.Drawing.Size( 421, 48 );
            this.picturePositionIndicator.TabIndex = 10;
            this.picturePositionIndicator.TabStop = false;
            this.picturePositionIndicator.MouseLeave += new System.EventHandler( this.picturePositionIndicator_MouseLeave );
            this.picturePositionIndicator.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.picturePositionIndicator_PreviewKeyDown );
            this.picturePositionIndicator.MouseMove += new System.Windows.Forms.MouseEventHandler( this.picturePositionIndicator_MouseMove );
            this.picturePositionIndicator.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( this.picturePositionIndicator_MouseDoubleClick );
            this.picturePositionIndicator.MouseClick += new System.Windows.Forms.MouseEventHandler( this.picturePositionIndicator_MouseClick );
            this.picturePositionIndicator.MouseDown += new System.Windows.Forms.MouseEventHandler( this.picturePositionIndicator_MouseDown );
            this.picturePositionIndicator.Paint += new System.Windows.Forms.PaintEventHandler( this.picturePositionIndicator_Paint );
            // 
            // pictPianoRoll
            // 
            this.pictPianoRoll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictPianoRoll.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))) );
            this.pictPianoRoll.Location = new System.Drawing.Point( 0, 93 );
            this.pictPianoRoll.Margin = new System.Windows.Forms.Padding( 0 );
            this.pictPianoRoll.MinimumSize = new System.Drawing.Size( 0, 100 );
            this.pictPianoRoll.Name = "pictPianoRoll";
            this.pictPianoRoll.Size = new System.Drawing.Size( 405, 173 );
            this.pictPianoRoll.TabIndex = 12;
            this.pictPianoRoll.TabStop = false;
            this.pictPianoRoll.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.pictPianoRoll_PreviewKeyDown );
            this.pictPianoRoll.BKeyUp += new System.Windows.Forms.KeyEventHandler( this.commonCaptureSpaceKeyUp );
            this.pictPianoRoll.MouseMove += new System.Windows.Forms.MouseEventHandler( this.pictPianoRoll_MouseMove );
            this.pictPianoRoll.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( this.pictPianoRoll_MouseDoubleClick );
            this.pictPianoRoll.MouseClick += new System.Windows.Forms.MouseEventHandler( this.pictPianoRoll_MouseClick );
            this.pictPianoRoll.MouseDown += new System.Windows.Forms.MouseEventHandler( this.pictPianoRoll_MouseDown );
            this.pictPianoRoll.Paint += new System.Windows.Forms.PaintEventHandler( this.pictPianoRoll_Paint );
            this.pictPianoRoll.MouseUp += new System.Windows.Forms.MouseEventHandler( this.pictPianoRoll_MouseUp );
            this.pictPianoRoll.BKeyDown += new System.Windows.Forms.KeyEventHandler( this.commonCaptureSpaceKeyDown );
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox3.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox3.Location = new System.Drawing.Point( 0, 266 );
            this.pictureBox3.Margin = new System.Windows.Forms.Padding( 0 );
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size( 66, 16 );
            this.pictureBox3.TabIndex = 8;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.MouseDown += new System.Windows.Forms.MouseEventHandler( this.pictureBox3_MouseDown );
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox2.Location = new System.Drawing.Point( 405, 266 );
            this.pictureBox2.Margin = new System.Windows.Forms.Padding( 0 );
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size( 16, 16 );
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler( this.pictureBox2_MouseDown );
            // 
            // toolStripTool
            // 
            this.toolStripTool.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripTool.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.stripBtnPointer,
            this.stripBtnPencil,
            this.stripBtnLine,
            this.stripBtnEraser,
            this.toolStripSeparator5,
            this.stripBtnGrid,
            this.stripBtnCurve} );
            this.toolStripTool.Location = new System.Drawing.Point( 3, 75 );
            this.toolStripTool.Name = "toolStripTool";
            this.toolStripTool.Size = new System.Drawing.Size( 340, 25 );
            this.toolStripTool.TabIndex = 17;
            this.toolStripTool.Text = "toolStrip1";
            // 
            // stripBtnPointer
            // 
            this.stripBtnPointer.Checked = true;
            this.stripBtnPointer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stripBtnPointer.Image = global::Boare.Cadencii.Properties.Resources.arrow_135;
            this.stripBtnPointer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.stripBtnPointer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnPointer.Name = "stripBtnPointer";
            this.stripBtnPointer.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.stripBtnPointer.Size = new System.Drawing.Size( 61, 22 );
            this.stripBtnPointer.Text = "Pointer";
            this.stripBtnPointer.Click += new System.EventHandler( this.stripBtnArrow_Click );
            // 
            // stripBtnPencil
            // 
            this.stripBtnPencil.Image = global::Boare.Cadencii.Properties.Resources.pencil;
            this.stripBtnPencil.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnPencil.Name = "stripBtnPencil";
            this.stripBtnPencil.Size = new System.Drawing.Size( 56, 22 );
            this.stripBtnPencil.Text = "Pencil";
            this.stripBtnPencil.Click += new System.EventHandler( this.stripBtnPencil_Click );
            // 
            // stripBtnLine
            // 
            this.stripBtnLine.Image = global::Boare.Cadencii.Properties.Resources.layer_shape_line;
            this.stripBtnLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnLine.Name = "stripBtnLine";
            this.stripBtnLine.Size = new System.Drawing.Size( 46, 22 );
            this.stripBtnLine.Text = "Line";
            this.stripBtnLine.Click += new System.EventHandler( this.stripBtnLine_Click );
            // 
            // stripBtnEraser
            // 
            this.stripBtnEraser.Image = global::Boare.Cadencii.Properties.Resources.eraser;
            this.stripBtnEraser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnEraser.Name = "stripBtnEraser";
            this.stripBtnEraser.Size = new System.Drawing.Size( 58, 22 );
            this.stripBtnEraser.Text = "Eraser";
            this.stripBtnEraser.Click += new System.EventHandler( this.stripBtnEraser_Click );
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size( 6, 25 );
            // 
            // stripBtnGrid
            // 
            this.stripBtnGrid.CheckOnClick = true;
            this.stripBtnGrid.Image = global::Boare.Cadencii.Properties.Resources.ruler_crop;
            this.stripBtnGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnGrid.Name = "stripBtnGrid";
            this.stripBtnGrid.Size = new System.Drawing.Size( 46, 22 );
            this.stripBtnGrid.Text = "Grid";
            this.stripBtnGrid.CheckedChanged += new System.EventHandler( this.stripBtnGrid_CheckedChanged );
            // 
            // stripBtnCurve
            // 
            this.stripBtnCurve.Image = global::Boare.Cadencii.Properties.Resources.layer_shape_curve;
            this.stripBtnCurve.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnCurve.Name = "stripBtnCurve";
            this.stripBtnCurve.Size = new System.Drawing.Size( 55, 22 );
            this.stripBtnCurve.Text = "Curve";
            this.stripBtnCurve.Click += new System.EventHandler( this.stripBtnCurve_Click );
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            this.toolStripContainer.BottomToolStripPanel.Controls.Add( this.toolStripBottom );
            this.toolStripContainer.BottomToolStripPanel.Controls.Add( this.statusStrip1 );
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.AutoScroll = true;
            this.toolStripContainer.ContentPanel.Controls.Add( this.splitContainerProperty );
            this.toolStripContainer.ContentPanel.Controls.Add( this.panel2 );
            this.toolStripContainer.ContentPanel.Controls.Add( this.splitContainer2 );
            this.toolStripContainer.ContentPanel.Controls.Add( this.splitContainer1 );
            this.toolStripContainer.ContentPanel.Controls.Add( this.panel1 );
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size( 960, 518 );
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.LeftToolStripPanelVisible = false;
            this.toolStripContainer.Location = new System.Drawing.Point( 0, 24 );
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.RightToolStripPanelVisible = false;
            this.toolStripContainer.Size = new System.Drawing.Size( 960, 665 );
            this.toolStripContainer.TabIndex = 18;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add( this.toolStripFile );
            this.toolStripContainer.TopToolStripPanel.Controls.Add( this.toolStripPosition );
            this.toolStripContainer.TopToolStripPanel.Controls.Add( this.toolStripMeasure );
            this.toolStripContainer.TopToolStripPanel.Controls.Add( this.toolStripTool );
            this.toolStripContainer.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripContainer.TopToolStripPanel.SizeChanged += new System.EventHandler( this.toolStripContainer_TopToolStripPanel_SizeChanged );
            // 
            // toolStripBottom
            // 
            this.toolStripBottom.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripBottom.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel6,
            this.stripLblCursor,
            this.toolStripSeparator8,
            this.toolStripLabel8,
            this.stripLblTempo,
            this.toolStripSeparator9,
            this.toolStripLabel10,
            this.stripLblBeat,
            this.toolStripSeparator4,
            this.toolStripStatusLabel1,
            this.stripLblGameCtrlMode,
            this.toolStripSeparator10,
            this.toolStripStatusLabel2,
            this.stripLblMidiIn,
            this.toolStripSeparator11,
            this.stripDDBtnSpeed} );
            this.toolStripBottom.Location = new System.Drawing.Point( 5, 0 );
            this.toolStripBottom.Name = "toolStripBottom";
            this.toolStripBottom.Size = new System.Drawing.Size( 696, 25 );
            this.toolStripBottom.TabIndex = 22;
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size( 52, 22 );
            this.toolStripLabel6.Text = "CURSOR";
            // 
            // stripLblCursor
            // 
            this.stripLblCursor.AutoSize = false;
            this.stripLblCursor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripLblCursor.Font = new System.Drawing.Font( "MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)) );
            this.stripLblCursor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripLblCursor.Name = "stripLblCursor";
            this.stripLblCursor.Size = new System.Drawing.Size( 90, 22 );
            this.stripLblCursor.Text = "0 : 0 : 000";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size( 6, 25 );
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            this.toolStripLabel8.Size = new System.Drawing.Size( 43, 22 );
            this.toolStripLabel8.Text = "TEMPO";
            // 
            // stripLblTempo
            // 
            this.stripLblTempo.AutoSize = false;
            this.stripLblTempo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripLblTempo.Font = new System.Drawing.Font( "MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)) );
            this.stripLblTempo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripLblTempo.Name = "stripLblTempo";
            this.stripLblTempo.Size = new System.Drawing.Size( 60, 22 );
            this.stripLblTempo.Text = "120.00";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size( 6, 25 );
            // 
            // toolStripLabel10
            // 
            this.toolStripLabel10.Name = "toolStripLabel10";
            this.toolStripLabel10.Size = new System.Drawing.Size( 35, 22 );
            this.toolStripLabel10.Text = "BEAT";
            // 
            // stripLblBeat
            // 
            this.stripLblBeat.AutoSize = false;
            this.stripLblBeat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripLblBeat.Font = new System.Drawing.Font( "MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)) );
            this.stripLblBeat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripLblBeat.Name = "stripLblBeat";
            this.stripLblBeat.Size = new System.Drawing.Size( 45, 22 );
            this.stripLblBeat.Text = "4/4";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size( 6, 25 );
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size( 85, 20 );
            this.toolStripStatusLabel1.Text = "Game Controler";
            // 
            // stripLblGameCtrlMode
            // 
            this.stripLblGameCtrlMode.Image = global::Boare.Cadencii.Properties.Resources.slash;
            this.stripLblGameCtrlMode.Name = "stripLblGameCtrlMode";
            this.stripLblGameCtrlMode.Size = new System.Drawing.Size( 65, 20 );
            this.stripLblGameCtrlMode.Text = "Disabled";
            this.stripLblGameCtrlMode.ToolTipText = "Game Controler";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size( 6, 25 );
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size( 41, 20 );
            this.toolStripStatusLabel2.Text = "MIDI In";
            // 
            // stripLblMidiIn
            // 
            this.stripLblMidiIn.Image = global::Boare.Cadencii.Properties.Resources.slash;
            this.stripLblMidiIn.Name = "stripLblMidiIn";
            this.stripLblMidiIn.Size = new System.Drawing.Size( 65, 20 );
            this.stripLblMidiIn.Text = "Disabled";
            this.stripLblMidiIn.ToolTipText = "Midi In Device";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size( 6, 25 );
            // 
            // stripDDBtnSpeed
            // 
            this.stripDDBtnSpeed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripDDBtnSpeed.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.stripDDBtnSpeedTextbox,
            this.stripDDBtnSpeed033,
            this.stripDDBtnSpeed050,
            this.stripDDBtnSpeed100} );
            this.stripDDBtnSpeed.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripDDBtnSpeed.Name = "stripDDBtnSpeed";
            this.stripDDBtnSpeed.Size = new System.Drawing.Size( 73, 22 );
            this.stripDDBtnSpeed.Text = "Speed 1.0x";
            this.stripDDBtnSpeed.DropDownOpening += new System.EventHandler( this.stripDDBtnSpeed_DropDownOpening );
            // 
            // stripDDBtnSpeedTextbox
            // 
            this.stripDDBtnSpeedTextbox.Name = "stripDDBtnSpeedTextbox";
            this.stripDDBtnSpeedTextbox.Size = new System.Drawing.Size( 100, 19 );
            this.stripDDBtnSpeedTextbox.Text = "100";
            this.stripDDBtnSpeedTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler( this.stripDDBtnSpeedTextbox_KeyDown );
            // 
            // stripDDBtnSpeed033
            // 
            this.stripDDBtnSpeed033.Name = "stripDDBtnSpeed033";
            this.stripDDBtnSpeed033.Size = new System.Drawing.Size( 160, 22 );
            this.stripDDBtnSpeed033.Text = "33.3%";
            this.stripDDBtnSpeed033.Click += new System.EventHandler( this.stripDDBtnSpeed033_Click );
            // 
            // stripDDBtnSpeed050
            // 
            this.stripDDBtnSpeed050.Name = "stripDDBtnSpeed050";
            this.stripDDBtnSpeed050.Size = new System.Drawing.Size( 160, 22 );
            this.stripDDBtnSpeed050.Text = "50%";
            this.stripDDBtnSpeed050.Click += new System.EventHandler( this.stripDDBtnSpeed050_Click );
            // 
            // stripDDBtnSpeed100
            // 
            this.stripDDBtnSpeed100.Name = "stripDDBtnSpeed100";
            this.stripDDBtnSpeed100.Size = new System.Drawing.Size( 160, 22 );
            this.stripDDBtnSpeed100.Text = "100%";
            this.stripDDBtnSpeed100.Click += new System.EventHandler( this.stripDDBtnSpeed100_Click );
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel} );
            this.statusStrip1.Location = new System.Drawing.Point( 0, 25 );
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size( 960, 22 );
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size( 0, 17 );
            // 
            // splitContainerProperty
            // 
            this.splitContainerProperty.FixedPanel = System.Windows.Forms.FixedPanel.None;
            this.splitContainerProperty.IsSplitterFixed = false;
            this.splitContainerProperty.Location = new System.Drawing.Point( 714, 17 );
            this.splitContainerProperty.Name = "splitContainerProperty";
            this.splitContainerProperty.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // 
            // 
            this.splitContainerProperty.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerProperty.Panel1.BorderColor = System.Drawing.Color.Black;
            this.splitContainerProperty.Panel1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainerProperty.Panel1.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 4 );
            this.splitContainerProperty.Panel1.Name = "m_panel1";
            this.splitContainerProperty.Panel1.Size = new System.Drawing.Size( 220, 348 );
            this.splitContainerProperty.Panel1.TabIndex = 0;
            this.splitContainerProperty.Panel1MinSize = 25;
            // 
            // 
            // 
            this.splitContainerProperty.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerProperty.Panel2.BorderColor = System.Drawing.Color.Black;
            this.splitContainerProperty.Panel2.Location = new System.Drawing.Point( 224, 0 );
            this.splitContainerProperty.Panel2.Margin = new System.Windows.Forms.Padding( 0 );
            this.splitContainerProperty.Panel2.Name = "m_panel2";
            this.splitContainerProperty.Panel2.Size = new System.Drawing.Size( 217, 348 );
            this.splitContainerProperty.Panel2.TabIndex = 1;
            this.splitContainerProperty.Panel2MinSize = 25;
            this.splitContainerProperty.Size = new System.Drawing.Size( 441, 348 );
            this.splitContainerProperty.SplitterDistance = 220;
            this.splitContainerProperty.SplitterWidth = 4;
            this.splitContainerProperty.TabIndex = 20;
            this.splitContainerProperty.Text = "bSplitContainer1";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DarkGray;
            this.panel2.Controls.Add( this.waveView );
            this.panel2.Location = new System.Drawing.Point( 3, 291 );
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size( 421, 59 );
            this.panel2.TabIndex = 19;
            // 
            // waveView
            // 
            this.waveView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.waveView.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))) );
            this.waveView.Location = new System.Drawing.Point( 66, 0 );
            this.waveView.Margin = new System.Windows.Forms.Padding( 0 );
            this.waveView.Name = "waveView";
            this.waveView.Size = new System.Drawing.Size( 355, 59 );
            this.waveView.TabIndex = 17;
            // 
            // splitContainer2
            // 
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = false;
            this.splitContainer2.Location = new System.Drawing.Point( 593, 17 );
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // 
            // 
            this.splitContainer2.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Panel1.BorderColor = System.Drawing.Color.Black;
            this.splitContainer2.Panel1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer2.Panel1.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 4 );
            this.splitContainer2.Panel1.Name = "m_panel1";
            this.splitContainer2.Panel1.Size = new System.Drawing.Size( 115, 53 );
            this.splitContainer2.Panel1.TabIndex = 0;
            this.splitContainer2.Panel1MinSize = 25;
            // 
            // 
            // 
            this.splitContainer2.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Panel2.BorderColor = System.Drawing.Color.Black;
            this.splitContainer2.Panel2.Location = new System.Drawing.Point( 0, 57 );
            this.splitContainer2.Panel2.Margin = new System.Windows.Forms.Padding( 0 );
            this.splitContainer2.Panel2.Name = "m_panel2";
            this.splitContainer2.Panel2.Size = new System.Drawing.Size( 115, 185 );
            this.splitContainer2.Panel2.TabIndex = 1;
            this.splitContainer2.Panel2MinSize = 25;
            this.splitContainer2.Size = new System.Drawing.Size( 115, 242 );
            this.splitContainer2.SplitterDistance = 53;
            this.splitContainer2.SplitterWidth = 4;
            this.splitContainer2.TabIndex = 18;
            this.splitContainer2.Text = "bSplitContainer1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = false;
            this.splitContainer1.Location = new System.Drawing.Point( 440, 17 );
            this.splitContainer1.MinimumSize = new System.Drawing.Size( 0, 54 );
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // 
            // 
            this.splitContainer1.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Panel1.BorderColor = System.Drawing.Color.Black;
            this.splitContainer1.Panel1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Panel1.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 4 );
            this.splitContainer1.Panel1.Name = "m_panel1";
            this.splitContainer1.Panel1.Size = new System.Drawing.Size( 138, 27 );
            this.splitContainer1.Panel1.TabIndex = 0;
            this.splitContainer1.Panel1MinSize = 25;
            // 
            // 
            // 
            this.splitContainer1.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Panel2.BorderColor = System.Drawing.Color.Black;
            this.splitContainer1.Panel2.Location = new System.Drawing.Point( 0, 31 );
            this.splitContainer1.Panel2.Margin = new System.Windows.Forms.Padding( 0 );
            this.splitContainer1.Panel2.Name = "m_panel2";
            this.splitContainer1.Panel2.Size = new System.Drawing.Size( 138, 211 );
            this.splitContainer1.Panel2.TabIndex = 1;
            this.splitContainer1.Panel2MinSize = 25;
            this.splitContainer1.Size = new System.Drawing.Size( 138, 242 );
            this.splitContainer1.SplitterDistance = 27;
            this.splitContainer1.SplitterWidth = 4;
            this.splitContainer1.TabIndex = 4;
            this.splitContainer1.Text = "splitContainerEx1";
            // 
            // toolStripFile
            // 
            this.toolStripFile.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripFile.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.stripBtnFileNew,
            this.stripBtnFileOpen,
            this.stripBtnFileSave,
            this.toolStripSeparator12,
            this.stripBtnCut,
            this.stripBtnCopy,
            this.stripBtnPaste,
            this.toolStripSeparator13,
            this.stripBtnUndo,
            this.stripBtnRedo} );
            this.toolStripFile.Location = new System.Drawing.Point( 3, 0 );
            this.toolStripFile.Name = "toolStripFile";
            this.toolStripFile.Size = new System.Drawing.Size( 208, 25 );
            this.toolStripFile.TabIndex = 20;
            // 
            // stripBtnFileNew
            // 
            this.stripBtnFileNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnFileNew.Image = global::Boare.Cadencii.Properties.Resources.disk__plus;
            this.stripBtnFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnFileNew.Name = "stripBtnFileNew";
            this.stripBtnFileNew.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnFileNew.Text = "toolStripButton6";
            this.stripBtnFileNew.ToolTipText = "New";
            this.stripBtnFileNew.Click += new System.EventHandler( this.commonFileNew_Click );
            // 
            // stripBtnFileOpen
            // 
            this.stripBtnFileOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnFileOpen.Image = global::Boare.Cadencii.Properties.Resources.folder_horizontal_open;
            this.stripBtnFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnFileOpen.Name = "stripBtnFileOpen";
            this.stripBtnFileOpen.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnFileOpen.Text = "toolStripButton3";
            this.stripBtnFileOpen.ToolTipText = "Open";
            this.stripBtnFileOpen.Click += new System.EventHandler( this.commonFileOpen_Click );
            // 
            // stripBtnFileSave
            // 
            this.stripBtnFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnFileSave.Image = global::Boare.Cadencii.Properties.Resources.disk;
            this.stripBtnFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnFileSave.Name = "stripBtnFileSave";
            this.stripBtnFileSave.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnFileSave.Text = "toolStripButton2";
            this.stripBtnFileSave.ToolTipText = "Save";
            this.stripBtnFileSave.Click += new System.EventHandler( this.commonFileSave_Click );
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size( 6, 25 );
            // 
            // stripBtnCut
            // 
            this.stripBtnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnCut.Image = global::Boare.Cadencii.Properties.Resources.scissors;
            this.stripBtnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnCut.Name = "stripBtnCut";
            this.stripBtnCut.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnCut.Text = "toolStripButton4";
            this.stripBtnCut.ToolTipText = "Cut";
            this.stripBtnCut.Click += new System.EventHandler( this.commonEditCut_Click );
            // 
            // stripBtnCopy
            // 
            this.stripBtnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnCopy.Image = global::Boare.Cadencii.Properties.Resources.documents;
            this.stripBtnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnCopy.Name = "stripBtnCopy";
            this.stripBtnCopy.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnCopy.Text = "toolStripButton5";
            this.stripBtnCopy.ToolTipText = "Copy";
            this.stripBtnCopy.Click += new System.EventHandler( this.commonEditCopy_Click );
            // 
            // stripBtnPaste
            // 
            this.stripBtnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnPaste.Image = global::Boare.Cadencii.Properties.Resources.clipboard_paste;
            this.stripBtnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnPaste.Name = "stripBtnPaste";
            this.stripBtnPaste.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnPaste.Text = "toolStripLabel1";
            this.stripBtnPaste.ToolTipText = "Paste";
            this.stripBtnPaste.Click += new System.EventHandler( this.commonEditPaste_Click );
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size( 6, 25 );
            // 
            // stripBtnUndo
            // 
            this.stripBtnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnUndo.Image = global::Boare.Cadencii.Properties.Resources.arrow_skip_180;
            this.stripBtnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnUndo.Name = "stripBtnUndo";
            this.stripBtnUndo.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnUndo.Text = "toolStripButton7";
            this.stripBtnUndo.ToolTipText = "Undo";
            this.stripBtnUndo.Click += new System.EventHandler( this.commonEditUndo_Click );
            // 
            // stripBtnRedo
            // 
            this.stripBtnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnRedo.Image = global::Boare.Cadencii.Properties.Resources.arrow_skip;
            this.stripBtnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnRedo.Name = "stripBtnRedo";
            this.stripBtnRedo.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnRedo.Text = "toolStripButton8";
            this.stripBtnRedo.ToolTipText = "Redo";
            this.stripBtnRedo.Click += new System.EventHandler( this.commonEditRedo_Click );
            // 
            // toolStripPosition
            // 
            this.toolStripPosition.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripPosition.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.stripBtnMoveTop,
            this.stripBtnRewind,
            this.stripBtnForward,
            this.stripBtnMoveEnd,
            this.stripBtnPlay,
            this.stripBtnStop,
            this.toolStripSeparator7,
            this.stripBtnScroll,
            this.stripBtnLoop} );
            this.toolStripPosition.Location = new System.Drawing.Point( 3, 25 );
            this.toolStripPosition.Name = "toolStripPosition";
            this.toolStripPosition.Size = new System.Drawing.Size( 202, 25 );
            this.toolStripPosition.TabIndex = 18;
            // 
            // stripBtnMoveTop
            // 
            this.stripBtnMoveTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnMoveTop.Image = global::Boare.Cadencii.Properties.Resources.control_stop_180;
            this.stripBtnMoveTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnMoveTop.Name = "stripBtnMoveTop";
            this.stripBtnMoveTop.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnMoveTop.Text = "  <=|  ";
            this.stripBtnMoveTop.ToolTipText = "MoveTop";
            this.stripBtnMoveTop.Click += new System.EventHandler( this.stripBtnMoveTop_Click );
            // 
            // stripBtnRewind
            // 
            this.stripBtnRewind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnRewind.Image = global::Boare.Cadencii.Properties.Resources.control_double_180;
            this.stripBtnRewind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnRewind.Name = "stripBtnRewind";
            this.stripBtnRewind.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnRewind.Text = "  <<  ";
            this.stripBtnRewind.ToolTipText = "Rewind";
            this.stripBtnRewind.Click += new System.EventHandler( this.stripBtnRewind_Click );
            // 
            // stripBtnForward
            // 
            this.stripBtnForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnForward.Image = global::Boare.Cadencii.Properties.Resources.control_double;
            this.stripBtnForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnForward.Name = "stripBtnForward";
            this.stripBtnForward.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnForward.Text = "  >>  ";
            this.stripBtnForward.ToolTipText = "Forward";
            this.stripBtnForward.Click += new System.EventHandler( this.stripBtnForward_Click );
            // 
            // stripBtnMoveEnd
            // 
            this.stripBtnMoveEnd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnMoveEnd.Image = global::Boare.Cadencii.Properties.Resources.control_stop;
            this.stripBtnMoveEnd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnMoveEnd.Name = "stripBtnMoveEnd";
            this.stripBtnMoveEnd.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnMoveEnd.Text = "  |=>  ";
            this.stripBtnMoveEnd.ToolTipText = "MoveEnd";
            this.stripBtnMoveEnd.Click += new System.EventHandler( this.stripBtnMoveEnd_Click );
            // 
            // stripBtnPlay
            // 
            this.stripBtnPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnPlay.Image = global::Boare.Cadencii.Properties.Resources.control;
            this.stripBtnPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnPlay.Name = "stripBtnPlay";
            this.stripBtnPlay.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnPlay.Text = "  =>  ";
            this.stripBtnPlay.ToolTipText = "Play";
            this.stripBtnPlay.Click += new System.EventHandler( this.stripBtnPlay_Click );
            // 
            // stripBtnStop
            // 
            this.stripBtnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnStop.Image = global::Boare.Cadencii.Properties.Resources.control_pause;
            this.stripBtnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnStop.Name = "stripBtnStop";
            this.stripBtnStop.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnStop.Text = "   ||   ";
            this.stripBtnStop.ToolTipText = "Stop";
            this.stripBtnStop.Click += new System.EventHandler( this.stripBtnStop_Click );
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size( 6, 25 );
            // 
            // stripBtnScroll
            // 
            this.stripBtnScroll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnScroll.Image = global::Boare.Cadencii.Properties.Resources.arrow_circle_double;
            this.stripBtnScroll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnScroll.Name = "stripBtnScroll";
            this.stripBtnScroll.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnScroll.Text = "Scroll";
            this.stripBtnScroll.Click += new System.EventHandler( this.stripBtnScroll_Click );
            // 
            // stripBtnLoop
            // 
            this.stripBtnLoop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnLoop.Image = global::Boare.Cadencii.Properties.Resources.arrow_return;
            this.stripBtnLoop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnLoop.Name = "stripBtnLoop";
            this.stripBtnLoop.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnLoop.Text = "Loop";
            this.stripBtnLoop.Click += new System.EventHandler( this.stripBtnLoop_Click );
            // 
            // toolStripMeasure
            // 
            this.toolStripMeasure.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripMeasure.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel5,
            this.stripLblMeasure,
            this.toolStripButton1,
            this.stripDDBtnLength,
            this.stripDDBtnQuantize,
            this.toolStripSeparator6,
            this.stripBtnStartMarker,
            this.stripBtnEndMarker} );
            this.toolStripMeasure.Location = new System.Drawing.Point( 3, 50 );
            this.toolStripMeasure.Name = "toolStripMeasure";
            this.toolStripMeasure.Size = new System.Drawing.Size( 409, 25 );
            this.toolStripMeasure.TabIndex = 19;
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size( 59, 22 );
            this.toolStripLabel5.Text = "MEASURE";
            // 
            // stripLblMeasure
            // 
            this.stripLblMeasure.Font = new System.Drawing.Font( "MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)) );
            this.stripLblMeasure.Name = "stripLblMeasure";
            this.stripLblMeasure.Size = new System.Drawing.Size( 75, 22 );
            this.stripLblMeasure.Text = "0 : 0 : 000";
            this.stripLblMeasure.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size( 6, 25 );
            // 
            // stripDDBtnLength
            // 
            this.stripDDBtnLength.AutoSize = false;
            this.stripDDBtnLength.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripDDBtnLength.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.stripDDBtnLength04,
            this.stripDDBtnLength08,
            this.stripDDBtnLength16,
            this.stripDDBtnLength32,
            this.stripDDBtnLength64,
            this.stripDDBtnLength128,
            this.stripDDBtnLengthOff,
            this.toolStripSeparator2,
            this.stripDDBtnLengthTriplet} );
            this.stripDDBtnLength.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripDDBtnLength.Name = "stripDDBtnLength";
            this.stripDDBtnLength.Size = new System.Drawing.Size( 95, 22 );
            this.stripDDBtnLength.Text = "LENGTH  1/64";
            this.stripDDBtnLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stripDDBtnLength04
            // 
            this.stripDDBtnLength04.Name = "stripDDBtnLength04";
            this.stripDDBtnLength04.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLength04.Text = "1/4";
            this.stripDDBtnLength04.Click += new System.EventHandler( this.h_lengthQuantize04 );
            // 
            // stripDDBtnLength08
            // 
            this.stripDDBtnLength08.Name = "stripDDBtnLength08";
            this.stripDDBtnLength08.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLength08.Text = "1/8";
            this.stripDDBtnLength08.Click += new System.EventHandler( this.h_lengthQuantize08 );
            // 
            // stripDDBtnLength16
            // 
            this.stripDDBtnLength16.Name = "stripDDBtnLength16";
            this.stripDDBtnLength16.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLength16.Text = "1/16";
            this.stripDDBtnLength16.Click += new System.EventHandler( this.h_lengthQuantize16 );
            // 
            // stripDDBtnLength32
            // 
            this.stripDDBtnLength32.Name = "stripDDBtnLength32";
            this.stripDDBtnLength32.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLength32.Text = "1/32";
            this.stripDDBtnLength32.Click += new System.EventHandler( this.h_lengthQuantize32 );
            // 
            // stripDDBtnLength64
            // 
            this.stripDDBtnLength64.Name = "stripDDBtnLength64";
            this.stripDDBtnLength64.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLength64.Text = "1/64";
            this.stripDDBtnLength64.Click += new System.EventHandler( this.h_lengthQuantize64 );
            // 
            // stripDDBtnLength128
            // 
            this.stripDDBtnLength128.Name = "stripDDBtnLength128";
            this.stripDDBtnLength128.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLength128.Text = "1/128";
            this.stripDDBtnLength128.Click += new System.EventHandler( this.h_lengthQuantize128 );
            // 
            // stripDDBtnLengthOff
            // 
            this.stripDDBtnLengthOff.Name = "stripDDBtnLengthOff";
            this.stripDDBtnLengthOff.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLengthOff.Text = "Off";
            this.stripDDBtnLengthOff.Click += new System.EventHandler( this.h_lengthQuantizeOff );
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size( 100, 6 );
            // 
            // stripDDBtnLengthTriplet
            // 
            this.stripDDBtnLengthTriplet.Name = "stripDDBtnLengthTriplet";
            this.stripDDBtnLengthTriplet.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnLengthTriplet.Text = "Triplet";
            this.stripDDBtnLengthTriplet.Click += new System.EventHandler( this.h_lengthQuantizeTriplet );
            // 
            // stripDDBtnQuantize
            // 
            this.stripDDBtnQuantize.AutoSize = false;
            this.stripDDBtnQuantize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripDDBtnQuantize.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.stripDDBtnQuantize04,
            this.stripDDBtnQuantize08,
            this.stripDDBtnQuantize16,
            this.stripDDBtnQuantize32,
            this.stripDDBtnQuantize64,
            this.stripDDBtnQuantize128,
            this.stripDDBtnQuantizeOff,
            this.toolStripSeparator3,
            this.stripDDBtnQuantizeTriplet} );
            this.stripDDBtnQuantize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripDDBtnQuantize.Name = "stripDDBtnQuantize";
            this.stripDDBtnQuantize.Size = new System.Drawing.Size( 110, 22 );
            this.stripDDBtnQuantize.Text = "QUANTIZE  1/64";
            this.stripDDBtnQuantize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stripDDBtnQuantize04
            // 
            this.stripDDBtnQuantize04.Name = "stripDDBtnQuantize04";
            this.stripDDBtnQuantize04.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantize04.Text = "1/4";
            this.stripDDBtnQuantize04.Click += new System.EventHandler( this.h_positionQuantize04 );
            // 
            // stripDDBtnQuantize08
            // 
            this.stripDDBtnQuantize08.Name = "stripDDBtnQuantize08";
            this.stripDDBtnQuantize08.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantize08.Text = "1/8";
            this.stripDDBtnQuantize08.Click += new System.EventHandler( this.h_positionQuantize08 );
            // 
            // stripDDBtnQuantize16
            // 
            this.stripDDBtnQuantize16.Name = "stripDDBtnQuantize16";
            this.stripDDBtnQuantize16.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantize16.Text = "1/16";
            this.stripDDBtnQuantize16.Click += new System.EventHandler( this.h_positionQuantize16 );
            // 
            // stripDDBtnQuantize32
            // 
            this.stripDDBtnQuantize32.Name = "stripDDBtnQuantize32";
            this.stripDDBtnQuantize32.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantize32.Text = "1/32";
            this.stripDDBtnQuantize32.Click += new System.EventHandler( this.h_positionQuantize32 );
            // 
            // stripDDBtnQuantize64
            // 
            this.stripDDBtnQuantize64.Name = "stripDDBtnQuantize64";
            this.stripDDBtnQuantize64.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantize64.Text = "1/64";
            this.stripDDBtnQuantize64.Click += new System.EventHandler( this.h_positionQuantize64 );
            // 
            // stripDDBtnQuantize128
            // 
            this.stripDDBtnQuantize128.Name = "stripDDBtnQuantize128";
            this.stripDDBtnQuantize128.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantize128.Text = "1/128";
            this.stripDDBtnQuantize128.Click += new System.EventHandler( this.h_positionQuantize128 );
            // 
            // stripDDBtnQuantizeOff
            // 
            this.stripDDBtnQuantizeOff.Name = "stripDDBtnQuantizeOff";
            this.stripDDBtnQuantizeOff.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantizeOff.Text = "Off";
            this.stripDDBtnQuantizeOff.Click += new System.EventHandler( this.h_positionQuantizeOff );
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size( 100, 6 );
            // 
            // stripDDBtnQuantizeTriplet
            // 
            this.stripDDBtnQuantizeTriplet.Name = "stripDDBtnQuantizeTriplet";
            this.stripDDBtnQuantizeTriplet.Size = new System.Drawing.Size( 103, 22 );
            this.stripDDBtnQuantizeTriplet.Text = "Triplet";
            this.stripDDBtnQuantizeTriplet.Click += new System.EventHandler( this.h_positionQuantizeTriplet );
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size( 6, 25 );
            // 
            // stripBtnStartMarker
            // 
            this.stripBtnStartMarker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnStartMarker.Image = global::Boare.Cadencii.Properties.Resources.pin__arrow;
            this.stripBtnStartMarker.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnStartMarker.Name = "stripBtnStartMarker";
            this.stripBtnStartMarker.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnStartMarker.Text = "StartMarker";
            this.stripBtnStartMarker.Click += new System.EventHandler( this.stripBtnStartMarker_Click );
            // 
            // stripBtnEndMarker
            // 
            this.stripBtnEndMarker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripBtnEndMarker.Image = global::Boare.Cadencii.Properties.Resources.pin__arrow_inv;
            this.stripBtnEndMarker.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripBtnEndMarker.Name = "stripBtnEndMarker";
            this.stripBtnEndMarker.Size = new System.Drawing.Size( 23, 22 );
            this.stripBtnEndMarker.Text = "EndMarker";
            this.stripBtnEndMarker.Click += new System.EventHandler( this.stripBtnEndMarker_Click );
            // 
            // openUstDialog
            // 
            this.openUstDialog.Filter = "UTAU Project File(*.ust)|*.ust|All Files(*.*)|*.*";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size( 960, 689 );
            this.Controls.Add( this.toolStripContainer );
            this.Controls.Add( this.menuStripMain );
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "FormMain";
            this.Text = "Cadencii";
            this.Deactivate += new System.EventHandler( this.FormMain_Deactivate );
            this.Load += new System.EventHandler( this.FormMain_Load );
            this.Activated += new System.EventHandler( this.FormMain_Activated );
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler( this.FormMain_FormClosed );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.FormMain_FormClosing );
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.FormMain_PreviewKeyDown );
            this.menuStripMain.ResumeLayout( false );
            this.menuStripMain.PerformLayout();
            this.cMenuPiano.ResumeLayout( false );
            this.cMenuTrackTab.ResumeLayout( false );
            this.cMenuTrackSelector.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.panel1.ResumeLayout( false );
            this.panel3.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.pictOverview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePositionIndicator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictPianoRoll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.toolStripTool.ResumeLayout( false );
            this.toolStripTool.PerformLayout();
            this.toolStripContainer.BottomToolStripPanel.ResumeLayout( false );
            this.toolStripContainer.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout( false );
            this.toolStripContainer.TopToolStripPanel.ResumeLayout( false );
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout( false );
            this.toolStripContainer.PerformLayout();
            this.toolStripBottom.ResumeLayout( false );
            this.toolStripBottom.PerformLayout();
            this.statusStrip1.ResumeLayout( false );
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout( false );
            this.toolStripFile.ResumeLayout( false );
            this.toolStripFile.PerformLayout();
            this.toolStripPosition.ResumeLayout( false );
            this.toolStripPosition.PerformLayout();
            this.toolStripMeasure.ResumeLayout( false );
            this.toolStripMeasure.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuVisual;
        private System.Windows.Forms.ToolStripMenuItem menuJob;
        private System.Windows.Forms.ToolStripMenuItem menuTrack;
        private System.Windows.Forms.ToolStripMenuItem menuLyric;
        private System.Windows.Forms.ToolStripMenuItem menuSetting;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuVisualControlTrack;
        private System.Windows.Forms.ToolStripMenuItem menuVisualMixer;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuVisualGridline;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuVisualStartMarker;
        private System.Windows.Forms.ToolStripMenuItem menuVisualEndMarker;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem menuVisualLyrics;
        private System.Windows.Forms.ToolStripMenuItem menuVisualNoteProperty;
        private System.Windows.Forms.ToolStripMenuItem menuSettingPreference;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem menuSettingDefaultSingerStyle;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem menuSettingPositionQuantize;
        private System.Windows.Forms.ToolStripMenuItem menuSettingPositionQuantize04;
        private System.Windows.Forms.ToolStripMenuItem menuSettingPositionQuantize08;
        private System.Windows.Forms.ToolStripMenuItem menuSettingPositionQuantize16;
        private System.Windows.Forms.ToolStripMenuItem menuSettingPositionQuantize32;
        private System.Windows.Forms.ToolStripMenuItem menuSettingPositionQuantize64;
        private System.Windows.Forms.ToolStripMenuItem menuSettingPositionQuantizeOff;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem menuSettingSingerProperty;
        private System.Windows.Forms.ToolStripMenuItem menuSettingPositionQuantizeTriplet;
        private System.Windows.Forms.ToolStripMenuItem menuSettingLengthQuantize;
        private System.Windows.Forms.ToolStripMenuItem menuSettingLengthQuantize04;
        private System.Windows.Forms.ToolStripMenuItem menuSettingLengthQuantize08;
        private System.Windows.Forms.ToolStripMenuItem menuSettingLengthQuantize16;
        private System.Windows.Forms.ToolStripMenuItem menuSettingLengthQuantize32;
        private System.Windows.Forms.ToolStripMenuItem menuSettingLengthQuantize64;
        private System.Windows.Forms.ToolStripMenuItem menuSettingLengthQuantizeOff;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuSettingLengthQuantizeTriplet;
        private System.Windows.Forms.ToolStripMenuItem menuFileNew;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem menuFileSave;
        private System.Windows.Forms.ToolStripMenuItem menuFileSaveNamed;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem menuFileImport;
        private System.Windows.Forms.ToolStripMenuItem menuFileExport;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem11;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem menuFileQuit;
        private System.Windows.Forms.ToolStripMenuItem menuEditUndo;
        private System.Windows.Forms.ToolStripMenuItem menuEditRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox picturePositionIndicator;
        private System.Windows.Forms.SaveFileDialog saveXmlVsqDialog;
        private System.Windows.Forms.ContextMenuStrip cMenuPiano;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoPointer;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoPencil;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoEraser;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoFixed;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoQuantize;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoLength;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoGrid;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem14;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoUndo;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem15;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoCut;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoFixed01;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoFixed02;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoFixed04;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoFixed08;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoFixed16;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoFixed32;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoFixed64;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoFixedOff;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem18;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoFixedTriplet;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoFixedDotted;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoCopy;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoPaste;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem16;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoSelectAll;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoSelectAllEvents;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem17;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoImportLyric;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoExpressionProperty;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoQuantize04;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoQuantize08;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoQuantize16;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoQuantize32;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoQuantize64;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoQuantizeOff;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem26;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoQuantizeTriplet;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoLength04;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoLength08;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoLength16;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoLength32;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoLength64;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoLengthOff;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem32;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoLengthTriplet;
        private System.Windows.Forms.ToolStripMenuItem menuFileRecent;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.OpenFileDialog openXmlVsqDialog;
        private System.Windows.Forms.ToolStripMenuItem menuEditCut;
        private System.Windows.Forms.ToolStripMenuItem menuEditCopy;
        private System.Windows.Forms.ToolStripMenuItem menuEditPaste;
        private System.Windows.Forms.ToolStripMenuItem menuEditDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem19;
        private System.Windows.Forms.ToolStripMenuItem menuEditAutoNormalizeMode;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem20;
        private System.Windows.Forms.ToolStripMenuItem menuEditSelectAll;
        private System.Windows.Forms.ToolStripMenuItem menuEditSelectAllEvents;
        public Boare.Cadencii.BPictureBox pictPianoRoll;
        private System.Windows.Forms.ToolStripMenuItem menuTrackOn;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem21;
        private System.Windows.Forms.ToolStripMenuItem menuTrackAdd;
        private System.Windows.Forms.ToolStripMenuItem menuTrackCopy;
        private System.Windows.Forms.ToolStripMenuItem menuTrackChangeName;
        private System.Windows.Forms.ToolStripMenuItem menuTrackDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem22;
        private System.Windows.Forms.ToolStripMenuItem menuTrackRenderCurrent;
        private System.Windows.Forms.ToolStripMenuItem menuTrackRenderAll;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem23;
        private System.Windows.Forms.ToolStripMenuItem menuTrackOverlay;
        private System.Windows.Forms.ContextMenuStrip cMenuTrackTab;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackTabTrackOn;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem24;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackTabAdd;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackTabCopy;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackTabChangeName;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackTabDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem25;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackTabRenderCurrent;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackTabRenderAll;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem27;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackTabOverlay;
        private System.Windows.Forms.ContextMenuStrip cMenuTrackSelector;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackSelectorPointer;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackSelectorPencil;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackSelectorLine;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackSelectorEraser;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem28;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackSelectorUndo;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackSelectorRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem29;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackSelectorCut;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackSelectorCopy;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackSelectorPaste;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackSelectorDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem31;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackSelectorSelectAll;
        private System.Windows.Forms.ToolStripMenuItem menuJobNormalize;
        private System.Windows.Forms.ToolStripMenuItem menuJobInsertBar;
        private System.Windows.Forms.ToolStripMenuItem menuJobDeleteBar;
        private System.Windows.Forms.ToolStripMenuItem menuJobRandomize;
        private System.Windows.Forms.ToolStripMenuItem menuJobConnect;
        private System.Windows.Forms.ToolStripMenuItem menuJobLyric;
        private System.Windows.Forms.ToolStripMenuItem menuJobRewire;
        private System.Windows.Forms.ToolStripMenuItem menuLyricExpressionProperty;
        private System.Windows.Forms.ToolStripMenuItem menuLyricSymbol;
        private System.Windows.Forms.ToolStripMenuItem menuLyricDictionary;
        private System.Windows.Forms.ToolStripMenuItem menuHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem menuHelpDebug;
        private System.Windows.Forms.ToolStripMenuItem menuFileExportWave;
        private System.Windows.Forms.ToolStripMenuItem menuFileExportMidi;
        private System.Windows.Forms.ToolStripMenuItem menuScript;
        private System.Windows.Forms.ToolStripMenuItem menuHidden;
        private System.Windows.Forms.ToolStripMenuItem menuHiddenEditLyric;
        private System.Windows.Forms.ToolStripMenuItem menuHiddenEditFlipToolPointerPencil;
        private System.Windows.Forms.ToolStripMenuItem menuHiddenEditFlipToolPointerEraser;
        private System.Windows.Forms.ToolStripMenuItem menuHiddenVisualForwardParameter;
        private System.Windows.Forms.ToolStripMenuItem menuHiddenVisualBackwardParameter;
        private System.Windows.Forms.ToolStripMenuItem menuHiddenTrackNext;
        private System.Windows.Forms.ToolStripMenuItem menuHiddenTrackBack;
        private System.Windows.Forms.ToolStripMenuItem menuJobReloadVsti;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoCurve;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackSelectorCurve;
        private System.Windows.Forms.TrackBar trackBar;
        private System.ComponentModel.BackgroundWorker bgWorkScreen;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStripTool;
        private System.Windows.Forms.ToolStripButton stripBtnPointer;
        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.ToolStripButton stripBtnLine;
        private System.Windows.Forms.ToolStripButton stripBtnPencil;
        private System.Windows.Forms.ToolStripButton stripBtnEraser;
        private System.Windows.Forms.ToolStripButton stripBtnGrid;
        private System.Windows.Forms.ToolStrip toolStripPosition;
        private System.Windows.Forms.ToolStripButton stripBtnMoveTop;
        private System.Windows.Forms.ToolStripButton stripBtnRewind;
        private System.Windows.Forms.ToolStripButton stripBtnForward;
        private System.Windows.Forms.ToolStripButton stripBtnMoveEnd;
        private System.Windows.Forms.ToolStripButton stripBtnPlay;
        private System.Windows.Forms.ToolStripButton stripBtnStop;
        private System.Windows.Forms.ToolStripButton stripBtnScroll;
        private System.Windows.Forms.ToolStripButton stripBtnLoop;
        private System.Windows.Forms.ToolStripButton stripBtnCurve;
        private System.Windows.Forms.ToolStrip toolStripMeasure;
        private System.Windows.Forms.ToolStripLabel stripLblMeasure;
        private System.Windows.Forms.ToolStripSeparator toolStripButton1;
        private System.Windows.Forms.ToolStripDropDownButton stripDDBtnLength;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnLength04;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnLength08;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnLength16;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnLength32;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnLength64;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnLengthOff;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnLengthTriplet;
        private System.Windows.Forms.ToolStripDropDownButton stripDDBtnQuantize;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnQuantize04;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnQuantize08;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnQuantize16;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnQuantize32;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnQuantize64;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnQuantizeOff;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnQuantizeTriplet;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton stripBtnStartMarker;
        private System.Windows.Forms.ToolStripButton stripBtnEndMarker;
        private Boare.Lib.AppUtil.BHScrollBar hScroll;
        private Boare.Lib.AppUtil.BVScrollBar vScroll;
        private System.Windows.Forms.ToolStripMenuItem menuLyricVibratoProperty;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoVibratoProperty;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripLabel stripLblCursor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripLabel toolStripLabel8;
        private System.Windows.Forms.ToolStripLabel stripLblTempo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripLabel toolStripLabel10;
        private System.Windows.Forms.ToolStripLabel stripLblBeat;
        private System.Windows.Forms.ToolStripMenuItem menuScriptUpdate;
        private System.Windows.Forms.ToolStripMenuItem menuSettingGameControler;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripStatusLabel stripLblGameCtrlMode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripDropDownButton stripDDBtnSpeed;
        private System.Windows.Forms.ToolStripMenuItem menuSettingGameControlerSetting;
        private System.Windows.Forms.ToolStripMenuItem menuSettingGameControlerLoad;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnLength128;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnQuantize128;
        private System.Windows.Forms.ToolStripMenuItem menuSettingPositionQuantize128;
        private System.Windows.Forms.ToolStripMenuItem menuSettingLengthQuantize128;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoQuantize128;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoLength128;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoFixed128;
        private System.Windows.Forms.Timer timer;
        private WaveView waveView;
        private System.Windows.Forms.ToolStripMenuItem menuVisualWaveform;
        private Boare.Lib.AppUtil.BSplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackSelectorDeleteBezier;
        private System.Windows.Forms.OpenFileDialog openUstDialog;
        private System.Windows.Forms.ToolStripStatusLabel stripLblMidiIn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem menuJobRealTime;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackTabRenderer;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackTabRendererVOCALOID1;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackTabRendererVOCALOID2;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackTabRendererUtau;
        private System.Windows.Forms.ToolStripMenuItem menuVisualPitchLine;
        private System.Windows.Forms.OpenFileDialog openMidiDialog;
        private System.Windows.Forms.SaveFileDialog saveMidiDialog;
        private System.Windows.Forms.ToolStripMenuItem menuFileImportMidi;
        private System.Windows.Forms.ToolStrip toolStripFile;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripButton stripBtnFileSave;
        private System.Windows.Forms.ToolStripButton stripBtnFileOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripButton stripBtnCut;
        private System.Windows.Forms.ToolStripButton stripBtnCopy;
        private System.Windows.Forms.ToolStripButton stripBtnPaste;
        private System.Windows.Forms.ToolStripButton stripBtnFileNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripButton stripBtnUndo;
        private System.Windows.Forms.ToolStripButton stripBtnRedo;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackSelectorPaletteTool;
        private System.Windows.Forms.ToolStripMenuItem cMenuPianoPaletteTool;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem menuSettingPaletteTool;
        private System.Windows.Forms.ToolStripMenuItem menuTrackRenderer;
        private System.Windows.Forms.ToolStripMenuItem menuTrackRendererVOCALOID1;
        private System.Windows.Forms.ToolStripMenuItem menuTrackRendererVOCALOID2;
        private System.Windows.Forms.ToolStripMenuItem menuTrackRendererUtau;
        private System.Windows.Forms.ToolStripMenuItem menuFileImportVsq;
        private System.Windows.Forms.ToolStripMenuItem menuSettingShortcut;
        private System.Windows.Forms.ToolStripTextBox stripDDBtnSpeedTextbox;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnSpeed033;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnSpeed050;
        private System.Windows.Forms.ToolStripMenuItem stripDDBtnSpeed100;
        private System.Windows.Forms.ToolStripMenuItem menuSettingMidi;
        private System.Windows.Forms.ToolStripMenuItem menuVisualProperty;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpenVsq;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpenUst;
        private System.Windows.Forms.ToolStripMenuItem menuSettingGameControlerRemove;
        private System.Windows.Forms.ToolStripMenuItem menuHiddenCopy;
        private System.Windows.Forms.ToolStripMenuItem menuHiddenPaste;
        private System.Windows.Forms.ToolStripMenuItem menuHiddenCut;
        private System.Windows.Forms.ToolStripMenuItem menuSettingUtauVoiceDB;
        private System.Windows.Forms.ToolStrip toolStripBottom;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private Boare.Lib.AppUtil.BSplitContainer splitContainerProperty;
        private System.Windows.Forms.PictureBox pictOverview;
        private System.Windows.Forms.ToolStripMenuItem menuVisualOverview;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnLeft1;
        private System.Windows.Forms.Button btnRight2;
        private System.Windows.Forms.Button btnZoom;
        private System.Windows.Forms.Button btnMooz;
        private System.Windows.Forms.Button btnLeft2;
        private System.Windows.Forms.Button btnRight1;
        private Boare.Lib.AppUtil.BSplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem menuTrackBgm;
        private System.Windows.Forms.OpenFileDialog openWaveDialog;
        private System.Windows.Forms.ToolStripMenuItem menuTrackRendererStraight;
        private System.Windows.Forms.ToolStripMenuItem cMenuTrackTabRendererStraight;
    }
}

