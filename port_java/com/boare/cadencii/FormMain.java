/*
 * FormMain.java
 * Copyright (c) 2009 kbinani
 *
 * This file is part of com.boare.cadencii.
 *
 * com.boare.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * com.boare.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
package com.boare.cadencii;

import java.awt.*;
import javax.swing.*;
import java.awt.event.*;
import com.boare.windows.forms.*;
import com.boare.*;

public class FormMain extends BForm implements ActionListener, MouseListener{
    private JMenuBar menuStripMain;
    private JMenu menuFile;
    private JMenu menuEdit;
    private JMenu menuVisual;
    private JMenu menuJob;
    private JMenu menuTrack;
    private JMenu menuLyric;
    private JMenu menuSetting;
    private JMenu menuHelp;
    private JCheckBoxMenuItem menuVisualControlTrack;
    private JCheckBoxMenuItem menuVisualMixer;
    private JCheckBoxMenuItem menuVisualGridline;
    private JCheckBoxMenuItem menuVisualStartMarker;
    private JCheckBoxMenuItem menuVisualEndMarker;
    private JCheckBoxMenuItem menuVisualLyrics;
    private JCheckBoxMenuItem menuVisualNoteProperty;
    private BMenuItem menuSettingPreference;
    private BMenuItem menuSettingDefaultSingerStyle;
    private JMenu menuSettingPositionQuantize;
    private JCheckBoxMenuItem menuSettingPositionQuantize04;
    private JCheckBoxMenuItem menuSettingPositionQuantize08;
    private JCheckBoxMenuItem menuSettingPositionQuantize16;
    private JCheckBoxMenuItem menuSettingPositionQuantize32;
    private JCheckBoxMenuItem menuSettingPositionQuantize64;
    private JCheckBoxMenuItem menuSettingPositionQuantizeOff;
    private BMenuItem menuSettingSingerProperty;
    private JCheckBoxMenuItem menuSettingPositionQuantizeTriplet;
    private JMenu menuSettingLengthQuantize;
    private JCheckBoxMenuItem menuSettingLengthQuantize04;
    private JCheckBoxMenuItem menuSettingLengthQuantize08;
    private JCheckBoxMenuItem menuSettingLengthQuantize16;
    private JCheckBoxMenuItem menuSettingLengthQuantize32;
    private JCheckBoxMenuItem menuSettingLengthQuantize64;
    private JCheckBoxMenuItem menuSettingLengthQuantizeOff;
    private JCheckBoxMenuItem menuSettingLengthQuantizeTriplet;
    private BMenuItem menuFileNew;
    private BMenuItem menuFileOpen;
    private BMenuItem menuFileSave;
    private BMenuItem menuFileSaveNamed;
    private JMenu menuFileImport;
    private JMenu menuFileExport;
    private BMenuItem menuFileQuit;
    private BMenuItem menuEditUndo;
    private BMenuItem menuEditRedo;
    private JPanel pictureBox2;
    private JPanel pictureBox3;
    private JPanel picturePositionIndicator;
    /*private System.Windows.Forms.SaveFileDialog saveXmlVsqDialog;
    private System.Windows.Forms.ContextMenuStrip cMenuPiano;
    private JMenu cMenuPianoPointer;
    private JMenu cMenuPianoPencil;
    private JMenu cMenuPianoEraser;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem13;
    private JMenu cMenuPianoFixed;
    private JMenu cMenuPianoQuantize;
    private JMenu cMenuPianoLength;
    private JMenu cMenuPianoGrid;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem14;
    private JMenu cMenuPianoUndo;
    private JMenu cMenuPianoRedo;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem15;
    private JMenu cMenuPianoCut;
    private JMenu cMenuPianoFixed01;
    private JMenu cMenuPianoFixed02;
    private JMenu cMenuPianoFixed04;
    private JMenu cMenuPianoFixed08;
    private JMenu cMenuPianoFixed16;
    private JMenu cMenuPianoFixed32;
    private JMenu cMenuPianoFixed64;
    private JMenu cMenuPianoFixedOff;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem18;
    private JMenu cMenuPianoFixedTriplet;
    private JMenu cMenuPianoFixedDotted;
    private JMenu cMenuPianoCopy;
    private JMenu cMenuPianoPaste;
    private JMenu cMenuPianoDelete;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem16;
    private JMenu cMenuPianoSelectAll;
    private JMenu cMenuPianoSelectAllEvents;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem17;
    private JMenu cMenuPianoImportLyric;
    private JMenu cMenuPianoExpressionProperty;
    private JMenu cMenuPianoQuantize04;
    private JMenu cMenuPianoQuantize08;
    private JMenu cMenuPianoQuantize16;
    private JMenu cMenuPianoQuantize32;
    private JMenu cMenuPianoQuantize64;
    private JMenu cMenuPianoQuantizeOff;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem26;
    private JMenu cMenuPianoQuantizeTriplet;
    private JMenu cMenuPianoLength04;
    private JMenu cMenuPianoLength08;
    private JMenu cMenuPianoLength16;
    private JMenu cMenuPianoLength32;
    private JMenu cMenuPianoLength64;
    private JMenu cMenuPianoLengthOff;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem32;
    private JMenu cMenuPianoLengthTriplet;*/
    private JMenu menuFileRecent;
    //private System.Windows.Forms.ToolTip toolTip;
    //private System.Windows.Forms.OpenFileDialog openXmlVsqDialog;
    private BMenuItem menuEditCut;
    private BMenuItem menuEditCopy;
    private BMenuItem menuEditPaste;
    private BMenuItem menuEditDelete;
    private BMenuItem menuEditAutoNormalizeMode;
    private BMenuItem menuEditSelectAll;
    private BMenuItem menuEditSelectAllEvents;
    public PictPianoRoll pictPianoRoll;
    private JCheckBoxMenuItem menuTrackOn;
    private BMenuItem menuTrackAdd;
    private BMenuItem menuTrackCopy;
    private BMenuItem menuTrackChangeName;
    private BMenuItem menuTrackDelete;
    private BMenuItem menuTrackRenderCurrent;
    private BMenuItem menuTrackRenderAll;
    private JCheckBoxMenuItem menuTrackOverlay;
    /*private JMenu cMenuTrackTabTrackOn;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem24;
    private JMenu cMenuTrackTabAdd;
    private JMenu cMenuTrackTabCopy;
    private JMenu cMenuTrackTabChangeName;
    private JMenu cMenuTrackTabDelete;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem25;
    private JMenu cMenuTrackTabRenderCurrent;
    private JMenu cMenuTrackTabRenderAll;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem27;
    private JMenu cMenuTrackTabOverlay;
    private System.Windows.Forms.ContextMenuStrip cMenuTrackSelector;
    private JMenu cMenuTrackSelectorPointer;
    private JMenu cMenuTrackSelectorPencil;
    private JMenu cMenuTrackSelectorLine;
    private JMenu cMenuTrackSelectorEraser;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem28;
    private JMenu cMenuTrackSelectorUndo;
    private JMenu cMenuTrackSelectorRedo;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem29;
    private JMenu cMenuTrackSelectorCut;
    private JMenu cMenuTrackSelectorCopy;
    private JMenu cMenuTrackSelectorPaste;
    private JMenu cMenuTrackSelectorDelete;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem31;
    private JMenu cMenuTrackSelectorSelectAll;*/
    private BMenuItem menuJobNormalize;
    private BMenuItem menuJobInsertBar;
    private BMenuItem menuJobDeleteBar;
    private BMenuItem menuJobRandomize;
    private BMenuItem menuJobConnect;
    private BMenuItem menuJobLyric;
    private BMenuItem menuJobRewire;
    private BMenuItem menuLyricExpressionProperty;
    private BMenuItem menuLyricSymbol;
    private BMenuItem menuLyricDictionary;
    private BMenuItem menuHelpAbout;
    private BMenuItem menuHelpDebug;
    private BMenuItem menuFileExportWave;
    private BMenuItem menuFileExportMidi;
    private JMenu menuScript;
    private JMenu menuHidden;
    /*private JMenu menuHiddenEditLyric;
    private JMenu menuHiddenEditFlipToolPointerPencil;
    private JMenu menuHiddenEditFlipToolPointerEraser;
    private JMenu menuHiddenVisualForwardParameter;
    private JMenu menuHiddenVisualBackwardParameter;
    private JMenu menuHiddenTrackNext;
    private JMenu menuHiddenTrackBack;*/
    private BMenuItem menuJobReloadVsti;
    //private JMenu cMenuPianoCurve;
    //private JMenu cMenuTrackSelectorCurve;
    private JSplitPane splitContainer1;
    private JSlider trackBar;
    //private System.ComponentModel.BackgroundWorker bgWorkScreen;
    private JPanel panel1;
    private JToolBar toolStripTool;
    private JToggleButton stripBtnPointer;
    private BorderLayout toolStripContainer;
    private JToggleButton stripBtnLine;
    private JToggleButton stripBtnPencil;
    private JToggleButton stripBtnEraser;
    private JToggleButton stripBtnGrid;
    private JToggleButton stripBtnCurve;
    private JToolBar toolStripPosition;
    private JButton stripBtnMoveTop;
    private JButton stripBtnRewind;
    private JButton stripBtnForward;
    private JButton stripBtnMoveEnd;
    private JButton stripBtnPlay;
    private JButton stripBtnStop;
    private JToggleButton stripBtnScroll;
    private JToggleButton stripBtnLoop;
    /*private System.Windows.Forms.ToolStrip toolStripMeasure;
    private System.Windows.Forms.ToolStripLabel stripLblMeasure;
    private System.Windows.Forms.ToolStripSeparator toolStripButton1;
    private System.Windows.Forms.ToolStripDropDownButton stripDDBtnLength;
    private JMenu stripDDBtnLength04;
    private JMenu stripDDBtnLength08;
    private JMenu stripDDBtnLength16;
    private JMenu stripDDBtnLength32;
    private JMenu stripDDBtnLength64;
    private JMenu stripDDBtnLengthOff;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private JMenu stripDDBtnLengthTriplet;
    private System.Windows.Forms.ToolStripDropDownButton stripDDBtnQuantize;
    private JMenu stripDDBtnQuantize04;
    private JMenu stripDDBtnQuantize08;
    private JMenu stripDDBtnQuantize16;
    private JMenu stripDDBtnQuantize32;
    private JMenu stripDDBtnQuantize64;
    private JMenu stripDDBtnQuantizeOff;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private JMenu stripDDBtnQuantizeTriplet;
    private System.Windows.Forms.ToolStripLabel toolStripLabel5;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    private System.Windows.Forms.ToolStripButton stripBtnStartMarker;
    private System.Windows.Forms.ToolStripButton stripBtnEndMarker;*/
    private JScrollBar hScroll;
    private JScrollBar vScroll;
    private BMenuItem menuLyricVibratoProperty;
    //private JMenu cMenuPianoVibratoProperty;
    /*private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripLabel toolStripLabel6;
    private System.Windows.Forms.ToolStripLabel stripLblCursor;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
    private System.Windows.Forms.ToolStripLabel toolStripLabel8;
    private System.Windows.Forms.ToolStripLabel stripLblTempo;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
    private System.Windows.Forms.ToolStripLabel toolStripLabel10;
    private System.Windows.Forms.ToolStripLabel stripLblBeat;*/
    private BMenuItem menuScriptUpdate;
    private JMenu menuSettingGameControler;
    /*private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripStatusLabel stripLblGameCtrlMode;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
    private System.Windows.Forms.ToolStripDropDownButton stripDDBtnSpeed;*/
    private BMenuItem menuSettingGameControlerSetting;
    private BMenuItem menuSettingGameControlerLoad;
    //private JMenu stripDDBtnLength128;
    //private JMenu stripDDBtnQuantize128;
    private JCheckBoxMenuItem menuSettingPositionQuantize128;
    private JCheckBoxMenuItem menuSettingLengthQuantize128;
    //private JMenu cMenuPianoQuantize128;
    //private JMenu cMenuPianoLength128;
    //private JMenu cMenuPianoFixed128;
    /*private System.Windows.Forms.Timer timer;
    private WaveView waveView;*/
    private JCheckBoxMenuItem menuVisualWaveform;
    private JSplitPane splitContainer2;
   /* private System.Windows.Forms.Panel panel2;
    private JMenu cMenuTrackSelectorDeleteBezier;
    private System.Windows.Forms.OpenFileDialog openUstDialog;
    private System.Windows.Forms.ToolStripStatusLabel stripLblMidiIn;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;*/
    private BMenuItem menuJobRealTime;
    /*private JMenu cMenuTrackTabRenderer;
    private JMenu cMenuTrackTabRendererVOCALOID1;
    private JMenu cMenuTrackTabRendererVOCALOID2;
    private JMenu cMenuTrackTabRendererUtau;*/
    private JCheckBoxMenuItem menuVisualPitchLine;
    /*private System.Windows.Forms.OpenFileDialog openMidiDialog;
    private System.Windows.Forms.SaveFileDialog saveMidiDialog;*/
    private BMenuItem menuFileImportMidi;
    /*private System.Windows.Forms.ToolStrip toolStripFile;
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
    private System.Windows.Forms.ToolStrip toolStripPaletteTools;
    private JMenu cMenuTrackSelectorPaletteTool;
    private JMenu cMenuPianoPaletteTool;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;*/
    private BMenuItem menuSettingPaletteTool;
    private BMenuItem menuTrackMasterTuning;
    //private JMenu cMenuTrackTabMasterTuning;
    private JMenu menuTrackRenderer;
    private JCheckBoxMenuItem menuTrackRendererVOCALOID1;
    private JCheckBoxMenuItem menuTrackRendererVOCALOID2;
    private JCheckBoxMenuItem menuTrackRendererUtau;
    private BMenuItem menuFileImportVsq;
    private BMenuItem menuSettingShortcut;
    /*private System.Windows.Forms.ToolStripTextBox stripDDBtnSpeedTextbox;
    private JMenu stripDDBtnSpeed033;
    private JMenu stripDDBtnSpeed050;
    private JMenu stripDDBtnSpeed100;*/
    private BMenuItem menuSettingMidi;
    private JCheckBoxMenuItem menuVisualProperty;
    private BMenuItem menuFileOpenVsq;
    private BMenuItem menuFileOpenUst;
    private BMenuItem menuSettingGameControlerRemove;
    /*private JMenu menuHiddenCopy;
    private JMenu menuHiddenPaste;
    private JMenu menuHiddenCut;*/
    private BMenuItem menuSettingUtauVoiceDB;
    //private System.Windows.Forms.ToolStrip toolStripBottom;
    private JLabel statusLabel;

    public FormMain(){
        super( "Cadencii - Untitled" );
        initializeComponent();
    }

    private void initializeComponent() {
        menuStripMain = new JMenuBar();
        menuFile = new JMenu();
        menuFileNew = new BMenuItem();
        menuFileOpen = new BMenuItem();
        menuFileSave = new BMenuItem();
        menuFileSaveNamed = new BMenuItem();
        menuFileOpenVsq = new BMenuItem();
        menuFileOpenUst = new BMenuItem();
        menuFileImport = new JMenu();
        menuFileImportVsq = new BMenuItem();
        menuFileImportMidi = new BMenuItem();
        menuFileExport = new JMenu();
        menuFileExportWave = new BMenuItem();
        menuFileExportMidi = new BMenuItem();
        menuFileRecent = new JMenu();
        menuFileQuit = new BMenuItem();
        menuEdit = new JMenu();
        menuEditUndo = new BMenuItem();
        menuEditRedo = new BMenuItem();
        menuEditCut = new BMenuItem();
        menuEditCopy = new BMenuItem();
        menuEditPaste = new BMenuItem();
        menuEditDelete = new BMenuItem();
        menuEditAutoNormalizeMode = new BMenuItem();
        menuEditSelectAll = new BMenuItem();
        menuEditSelectAllEvents = new BMenuItem();
        menuVisual = new JMenu();
        menuVisualControlTrack = new JCheckBoxMenuItem();
        menuVisualMixer = new JCheckBoxMenuItem();
        menuVisualWaveform = new JCheckBoxMenuItem();
        menuVisualProperty = new JCheckBoxMenuItem();
        menuVisualGridline = new JCheckBoxMenuItem();
        menuVisualStartMarker = new JCheckBoxMenuItem();
        menuVisualEndMarker = new JCheckBoxMenuItem();
        menuVisualLyrics = new JCheckBoxMenuItem();
        menuVisualNoteProperty = new JCheckBoxMenuItem();
        menuVisualPitchLine = new JCheckBoxMenuItem();
        menuJob = new JMenu();
        menuJobNormalize = new BMenuItem();
        menuJobInsertBar = new BMenuItem();
        menuJobDeleteBar = new BMenuItem();
        menuJobRandomize = new BMenuItem();
        menuJobConnect = new BMenuItem();
        menuJobLyric = new BMenuItem();
        menuJobRewire = new BMenuItem();
        menuJobRealTime = new BMenuItem();
        menuJobReloadVsti = new BMenuItem();
        menuTrack = new JMenu();
        menuTrackOn = new JCheckBoxMenuItem();
        menuTrackAdd = new BMenuItem();
        menuTrackCopy = new BMenuItem();
        menuTrackChangeName = new BMenuItem();
        menuTrackDelete = new BMenuItem();
        menuTrackRenderCurrent = new BMenuItem();
        menuTrackRenderAll = new BMenuItem();
        menuTrackOverlay = new JCheckBoxMenuItem();
        menuTrackRenderer = new JMenu();
        menuTrackRendererVOCALOID1 = new JCheckBoxMenuItem();
        menuTrackRendererVOCALOID2 = new JCheckBoxMenuItem();
        menuTrackRendererUtau = new JCheckBoxMenuItem();
        menuTrackMasterTuning = new BMenuItem();
        menuLyric = new JMenu();
        menuLyricExpressionProperty = new BMenuItem();
        menuLyricVibratoProperty = new BMenuItem();
        menuLyricSymbol = new BMenuItem();
        menuLyricDictionary = new BMenuItem();
        menuScript = new JMenu();
        menuScriptUpdate = new BMenuItem();
        menuSetting = new JMenu();
        menuSettingPreference = new BMenuItem();
        menuSettingGameControler = new JMenu();
        menuSettingGameControlerSetting = new BMenuItem();
        menuSettingGameControlerLoad = new BMenuItem();
        menuSettingGameControlerRemove = new BMenuItem();
        menuSettingPaletteTool = new BMenuItem();
        menuSettingShortcut = new BMenuItem();
        menuSettingMidi = new BMenuItem();
        menuSettingUtauVoiceDB = new BMenuItem();
        menuSettingDefaultSingerStyle = new BMenuItem();
        menuSettingPositionQuantize = new JMenu();
        menuSettingPositionQuantize04 = new JCheckBoxMenuItem();
        menuSettingPositionQuantize08 = new JCheckBoxMenuItem();
        menuSettingPositionQuantize16 = new JCheckBoxMenuItem();
        menuSettingPositionQuantize32 = new JCheckBoxMenuItem();
        menuSettingPositionQuantize64 = new JCheckBoxMenuItem();
        menuSettingPositionQuantize128 = new JCheckBoxMenuItem();
        menuSettingPositionQuantizeOff = new JCheckBoxMenuItem();
        menuSettingPositionQuantizeTriplet = new JCheckBoxMenuItem();
        menuSettingLengthQuantize = new JMenu();
        menuSettingLengthQuantize04 = new JCheckBoxMenuItem();
        menuSettingLengthQuantize08 = new JCheckBoxMenuItem();
        menuSettingLengthQuantize16 = new JCheckBoxMenuItem();
        menuSettingLengthQuantize32 = new JCheckBoxMenuItem();
        menuSettingLengthQuantize64 = new JCheckBoxMenuItem();
        menuSettingLengthQuantize128 = new JCheckBoxMenuItem();
        menuSettingLengthQuantizeOff = new JCheckBoxMenuItem();
        menuSettingLengthQuantizeTriplet = new JCheckBoxMenuItem();
        menuSettingSingerProperty = new BMenuItem();
        menuHelp = new JMenu();
        menuHelpAbout = new BMenuItem();
        menuHelpDebug = new BMenuItem();
        menuHidden = new JMenu();
        /*menuHiddenEditLyric = new JMenu();
        menuHiddenEditFlipToolPointerPencil = new JMenu();
        menuHiddenEditFlipToolPointerEraser = new JMenu();
        menuHiddenVisualForwardParameter = new JMenu();
        menuHiddenVisualBackwardParameter = new JMenu();
        menuHiddenTrackNext = new JMenu();
        menuHiddenTrackBack = new JMenu();
        menuHiddenCopy = new JMenu();
        menuHiddenPaste = new JMenu();
        menuHiddenCut = new JMenu();
        saveXmlVsqDialog = new System.Windows.Forms.SaveFileDialog();
        cMenuPiano = new System.Windows.Forms.ContextMenuStrip( components );
        cMenuPianoPointer = new JMenu();
        cMenuPianoPencil = new JMenu();
        cMenuPianoEraser = new JMenu();
        cMenuPianoPaletteTool = new JMenu();
        toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
        cMenuPianoCurve = new JMenu();
        toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
        cMenuPianoFixed = new JMenu();
        cMenuPianoFixed01 = new JMenu();
        cMenuPianoFixed02 = new JMenu();
        cMenuPianoFixed04 = new JMenu();
        cMenuPianoFixed08 = new JMenu();
        cMenuPianoFixed16 = new JMenu();
        cMenuPianoFixed32 = new JMenu();
        cMenuPianoFixed64 = new JMenu();
        cMenuPianoFixed128 = new JMenu();
        cMenuPianoFixedOff = new JMenu();
        toolStripMenuItem18 = new System.Windows.Forms.ToolStripSeparator();
        cMenuPianoFixedTriplet = new JMenu();
        cMenuPianoFixedDotted = new JMenu();
        cMenuPianoQuantize = new JMenu();
        cMenuPianoQuantize04 = new JMenu();
        cMenuPianoQuantize08 = new JMenu();
        cMenuPianoQuantize16 = new JMenu();
        cMenuPianoQuantize32 = new JMenu();
        cMenuPianoQuantize64 = new JMenu();
        cMenuPianoQuantize128 = new JMenu();
        cMenuPianoQuantizeOff = new JMenu();
        toolStripMenuItem26 = new System.Windows.Forms.ToolStripSeparator();
        cMenuPianoQuantizeTriplet = new JMenu();
        cMenuPianoLength = new JMenu();
        cMenuPianoLength04 = new JMenu();
        cMenuPianoLength08 = new JMenu();
        cMenuPianoLength16 = new JMenu();
        cMenuPianoLength32 = new JMenu();
        cMenuPianoLength64 = new JMenu();
        cMenuPianoLength128 = new JMenu();
        cMenuPianoLengthOff = new JMenu();
        toolStripMenuItem32 = new System.Windows.Forms.ToolStripSeparator();
        cMenuPianoLengthTriplet = new JMenu();
        cMenuPianoGrid = new JMenu();
        toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
        cMenuPianoUndo = new JMenu();
        cMenuPianoRedo = new JMenu();
        toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
        cMenuPianoCut = new JMenu();
        cMenuPianoCopy = new JMenu();
        cMenuPianoPaste = new JMenu();
        cMenuPianoDelete = new JMenu();
        toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
        cMenuPianoSelectAll = new JMenu();
        cMenuPianoSelectAllEvents = new JMenu();
        toolStripMenuItem17 = new System.Windows.Forms.ToolStripSeparator();
        cMenuPianoImportLyric = new JMenu();
        cMenuPianoExpressionProperty = new JMenu();
        cMenuPianoVibratoProperty = new JMenu();
        toolTip = new System.Windows.Forms.ToolTip( components );
        openXmlVsqDialog = new System.Windows.Forms.OpenFileDialog();
        cMenuTrackTab = new System.Windows.Forms.ContextMenuStrip( components );
        cMenuTrackTabTrackOn = new JMenu();
        toolStripMenuItem24 = new System.Windows.Forms.ToolStripSeparator();
        cMenuTrackTabAdd = new JMenu();
        cMenuTrackTabCopy = new JMenu();
        cMenuTrackTabChangeName = new JMenu();
        cMenuTrackTabDelete = new JMenu();
        toolStripMenuItem25 = new System.Windows.Forms.ToolStripSeparator();
        cMenuTrackTabRenderCurrent = new JMenu();
        cMenuTrackTabRenderAll = new JMenu();
        toolStripMenuItem27 = new System.Windows.Forms.ToolStripSeparator();
        cMenuTrackTabOverlay = new JMenu();
        cMenuTrackTabRenderer = new JMenu();
        cMenuTrackTabRendererVOCALOID1 = new JMenu();
        cMenuTrackTabRendererVOCALOID2 = new JMenu();
        cMenuTrackTabRendererUtau = new JMenu();
        cMenuTrackTabMasterTuning = new JMenu();
        cMenuTrackSelector = new System.Windows.Forms.ContextMenuStrip( components );
        cMenuTrackSelectorPointer = new JMenu();
        cMenuTrackSelectorPencil = new JMenu();
        cMenuTrackSelectorLine = new JMenu();
        cMenuTrackSelectorEraser = new JMenu();
        cMenuTrackSelectorPaletteTool = new JMenu();
        toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
        cMenuTrackSelectorCurve = new JMenu();
        toolStripMenuItem28 = new System.Windows.Forms.ToolStripSeparator();
        cMenuTrackSelectorUndo = new JMenu();
        cMenuTrackSelectorRedo = new JMenu();
        toolStripMenuItem29 = new System.Windows.Forms.ToolStripSeparator();
        cMenuTrackSelectorCut = new JMenu();
        cMenuTrackSelectorCopy = new JMenu();
        cMenuTrackSelectorPaste = new JMenu();
        cMenuTrackSelectorDelete = new JMenu();
        cMenuTrackSelectorDeleteBezier = new JMenu();
        toolStripMenuItem31 = new System.Windows.Forms.ToolStripSeparator();
        cMenuTrackSelectorSelectAll = new JMenu();*/
        trackBar = new JSlider();
        /*bgWorkScreen = new System.ComponentModel.BackgroundWorker();
        timer = new System.Windows.Forms.Timer( components );*/
        panel1 = new JPanel();
        vScroll = new JScrollBar();
        hScroll = new JScrollBar();
        picturePositionIndicator = new JPanel();
        pictPianoRoll = new PictPianoRoll();
        pictureBox3 = new JPanel();
        pictureBox2 = new JPanel();
        toolStripTool = new JToolBar();
        stripBtnPointer = new JToggleButton();
        stripBtnPencil = new JToggleButton();
        stripBtnLine = new JToggleButton();
        stripBtnEraser = new JToggleButton();
        stripBtnGrid = new JToggleButton();
        stripBtnCurve = new JToggleButton();
        toolStripContainer = new BorderLayout();
        /*toolStripBottom = new System.Windows.Forms.ToolStrip();
        toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
        stripLblCursor = new System.Windows.Forms.ToolStripLabel();
        toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
        toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
        stripLblTempo = new System.Windows.Forms.ToolStripLabel();
        toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
        toolStripLabel10 = new System.Windows.Forms.ToolStripLabel();
        stripLblBeat = new System.Windows.Forms.ToolStripLabel();
        toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
        toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
        stripLblGameCtrlMode = new System.Windows.Forms.ToolStripStatusLabel();
        toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
        toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
        stripLblMidiIn = new System.Windows.Forms.ToolStripStatusLabel();
        toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
        stripDDBtnSpeed = new System.Windows.Forms.ToolStripDropDownButton();
        stripDDBtnSpeedTextbox = new System.Windows.Forms.ToolStripTextBox();
        stripDDBtnSpeed033 = new JMenu();
        stripDDBtnSpeed050 = new JMenu();
        stripDDBtnSpeed100 = new JMenu();
        statusStrip1 = new System.Windows.Forms.StatusStrip();*/
        statusLabel = new JLabel();
        //panel2 = new System.Windows.Forms.Panel();
        //waveView = new Boare.Cadencii.WaveView();
        splitContainer2 = new JSplitPane( JSplitPane.VERTICAL_SPLIT );
        splitContainer1 = new JSplitPane( JSplitPane.VERTICAL_SPLIT );
        toolStripPosition = new JToolBar();
        stripBtnMoveTop = new JButton();
        stripBtnRewind = new JButton();
        stripBtnForward = new JButton();
        stripBtnMoveEnd = new JButton();
        stripBtnPlay = new JButton();
        stripBtnStop = new JButton();
        //toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
        stripBtnScroll = new JToggleButton();
        stripBtnLoop = new JToggleButton();
        /*toolStripMeasure = new System.Windows.Forms.ToolStrip();
        toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
        stripLblMeasure = new System.Windows.Forms.ToolStripLabel();
        toolStripButton1 = new System.Windows.Forms.ToolStripSeparator();
        stripDDBtnLength = new System.Windows.Forms.ToolStripDropDownButton();
        stripDDBtnLength04 = new JMenu();
        stripDDBtnLength08 = new JMenu();
        stripDDBtnLength16 = new JMenu();
        stripDDBtnLength32 = new JMenu();
        stripDDBtnLength64 = new JMenu();
        stripDDBtnLength128 = new JMenu();
        stripDDBtnLengthOff = new JMenu();
        toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        stripDDBtnLengthTriplet = new JMenu();
        stripDDBtnQuantize = new System.Windows.Forms.ToolStripDropDownButton();
        stripDDBtnQuantize04 = new JMenu();
        stripDDBtnQuantize08 = new JMenu();
        stripDDBtnQuantize16 = new JMenu();
        stripDDBtnQuantize32 = new JMenu();
        stripDDBtnQuantize64 = new JMenu();
        stripDDBtnQuantize128 = new JMenu();
        stripDDBtnQuantizeOff = new JMenu();
        toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
        stripDDBtnQuantizeTriplet = new JMenu();
        toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
        stripBtnStartMarker = new System.Windows.Forms.ToolStripButton();
        stripBtnEndMarker = new System.Windows.Forms.ToolStripButton();
        toolStripFile = new System.Windows.Forms.ToolStrip();
        stripBtnFileNew = new System.Windows.Forms.ToolStripButton();
        stripBtnFileOpen = new System.Windows.Forms.ToolStripButton();
        stripBtnFileSave = new System.Windows.Forms.ToolStripButton();
        toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
        stripBtnCut = new System.Windows.Forms.ToolStripButton();
        stripBtnCopy = new System.Windows.Forms.ToolStripButton();
        stripBtnPaste = new System.Windows.Forms.ToolStripButton();
        toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
        stripBtnUndo = new System.Windows.Forms.ToolStripButton();
        stripBtnRedo = new System.Windows.Forms.ToolStripButton();
        toolStripPaletteTools = new System.Windows.Forms.ToolStrip();
        openUstDialog = new System.Windows.Forms.OpenFileDialog();
        openMidiDialog = new System.Windows.Forms.OpenFileDialog();
        saveMidiDialog = new System.Windows.Forms.SaveFileDialog();
        menuStripMain.SuspendLayout();
        cMenuPiano.SuspendLayout();
        cMenuTrackTab.SuspendLayout();
        cMenuTrackSelector.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(trackBar)).BeginInit();
        panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(picturePositionIndicator)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(pictPianoRoll)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(pictureBox3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(pictureBox2)).BeginInit();
        toolStripTool.SuspendLayout();
        toolStripContainer.BottomToolStripPanel.SuspendLayout();
        toolStripContainer.ContentPanel.SuspendLayout();
        toolStripContainer.TopToolStripPanel.SuspendLayout();
        toolStripContainer.SuspendLayout();
        toolStripBottom.SuspendLayout();
        statusStrip1.SuspendLayout();
        panel2.SuspendLayout();
        toolStripPosition.SuspendLayout();
        toolStripMeasure.SuspendLayout();
        toolStripFile.SuspendLayout();
        SuspendLayout();*/
        // 
        // menuStripMain
        // 
        menuStripMain.add( menuFile );
        menuStripMain.add( menuEdit );
        menuStripMain.add( menuVisual );
        menuStripMain.add( menuJob );
        menuStripMain.add( menuTrack );
        menuStripMain.add( menuLyric );
        menuStripMain.add( menuScript );
        menuStripMain.add( menuSetting );
        menuStripMain.add( menuHelp );
        menuStripMain.add( menuHidden );
        //menuStripMain.Location = new System.Drawing.Point( 0, 0 );
        menuStripMain.setName( "menuStripMain" );
        //menuStripMain.setSize( 962, 26 );
        //menuStripMain.TabIndex = 0;
        //menuStripMain.MouseDown += new System.Windows.Forms.MouseEventHandler( menuStrip1_MouseDown );*/
        // 
        // menuFile
        // 
        menuFile.add( menuFileNew );
        menuFile.add( menuFileOpen );
        menuFile.add( menuFileSave );
        menuFile.add( menuFileSaveNamed );
        menuFile.addSeparator();
        menuFile.add( menuFileOpenVsq );
        menuFile.add( menuFileOpenUst );
        menuFile.add( menuFileImport );
        menuFile.add( menuFileExport );
        menuFile.addSeparator();
        menuFile.add( menuFileRecent );
        menuFile.addSeparator();
        menuFile.add( menuFileQuit );
        menuFile.setName( "menuFile" );
        menuFile.setText( "File(F)" );
        // 
        // menuFileNew
        // 
        menuFileNew.setName( "menuFileNew" );
        menuFileNew.setText( "New(N)" );
        menuFileNew.setActionCommand( "commonFileNew_Click" );
        menuFileNew.addMouseListener( new MenuDescriptionActivator( statusLabel, "Create new project." ) );
        menuFileNew.addActionListener( this );
        // 
        // menuFileOpen
        // 
        menuFileOpen.setName( "menuFileOpen" );
        menuFileOpen.setText( "Open(O)" );
        menuFileOpen.addMouseListener( new MenuDescriptionActivator( statusLabel, "Open Cadencii project." ) );
        menuFileOpen.clickEvent.add( new BEventHandler( this, "commonFileOpen_Click" ) );
      //menuFileOpen.Click += new EventHandler( this.commonFileOpen_Click );
        // 
        // menuFileSave
        // 
        menuFileSave.setName( "menuFileSave" );
        menuFileSave.setText( "Save(S)" );
        menuFileSave.addMouseListener( new MenuDescriptionActivator( statusLabel, "Save current project." ) );
        menuFileSave.setActionCommand( "commonFileSave_Click" );
        menuFileSave.addActionListener( this );
        // 
        // menuFileSaveNamed
        // 
        menuFileSaveNamed.setName( "menuFileSaveNamed" );
        menuFileSaveNamed.setText( "Save As(A)" );
        menuFileSaveNamed.addMouseListener( new MenuDescriptionActivator( statusLabel, "Save current project with new name." ) );
        menuFileSaveNamed.setActionCommand( "menuFileSaveNamed_Click" );
        menuFileSaveNamed.addActionListener( this );
        // 
        // menuFileOpenVsq
        // 
        menuFileOpenVsq.setName( "menuFileOpenVsq" );
        menuFileOpenVsq.setText( "Open VSQ/Vocaloid Midi(V)" );
        menuFileOpenVsq.addMouseListener( new MenuDescriptionActivator( statusLabel, "Open VSQ / VOCALOID MIDI and create new project." ) );
        menuFileOpenVsq.setActionCommand( "menuFileOpenVsq_Click" );
        menuFileOpenVsq.addActionListener( this );
        // 
        // menuFileOpenUst
        // 
        menuFileOpenUst.setName( "menuFileOpenUst" );
        menuFileOpenUst.setText( "Open UTAU Project File(U)" );
        menuFileOpenUst.addMouseListener( new MenuDescriptionActivator( statusLabel, "Open UTAU project and create new project." ) );
        menuFileOpenUst.setActionCommand( "menuFileOpenUst_Click" );
        menuFileOpenUst.addActionListener( this );
        // 
        // menuFileImportVsq
        // 
        menuFileImportVsq.setName( "menuFileImportVsq" );
        menuFileImportVsq.setText( "VSQ File" );
        menuFileImportVsq.addMouseListener( new MenuDescriptionActivator( statusLabel, "Import VSQ / VOCALOID MIDI." ) );
        menuFileImportVsq.setActionCommand( "menuFileImportVsq_Click" );
        menuFileImportVsq.addActionListener( this );
        // 
        // menuFileImportMidi
        // 
        menuFileImportMidi.setName( "menuFileImportMidi" );
        menuFileImportMidi.setText( "Standard MIDI" );
        menuFileImportMidi.addMouseListener( new MenuDescriptionActivator( statusLabel, "Import Standard MIDI." ) );
        menuFileImportMidi.setActionCommand( "menuFileImportMidi_Click" );
        menuFileImportMidi.addActionListener( this );
        // 
        // menuFileImport
        // 
        menuFileImport.add( menuFileImportVsq );
        menuFileImport.add( menuFileImportMidi );
        menuFileImport.setName( "menuFileImport" );
        menuFileImport.setText( "Import(I)" );
        menuFileImport.addMouseListener( new MenuDescriptionActivator( statusLabel, "Import." ) );
        // 
        // menuFileExportWave
        // 
        menuFileExportWave.setName( "menuFileExportWave" );
        menuFileExportWave.setText( "Wave" );
        menuFileExportWave.addMouseListener( new MenuDescriptionActivator( statusLabel, "Export to WAVE file." ) );
        menuFileExportWave.setActionCommand( "menuFileExportWave_Click" );
        menuFileExportWave.addActionListener( this );
        // 
        // menuFileExportMidi
        // 
        menuFileExportMidi.setName( "menuFileExportMidi" );
        menuFileExportMidi.setText( "MIDI" );
        menuFileExportMidi.addMouseListener( new MenuDescriptionActivator( statusLabel, "Export to Standard MIDI." ) );
        menuFileExportMidi.setActionCommand( "menuFileExportMidi_Click" );
        menuFileExportMidi.addActionListener( this );
        // 
        // menuFileExport
        // 
        menuFileExport.add( menuFileExportWave );
        menuFileExport.add( menuFileExportMidi );
        menuFileExport.setName( "menuFileExport" );
        menuFileExport.setText( "Export(E)" );
        menuFileExport.addMouseListener( new MenuDescriptionActivator( statusLabel, "Export." ) );
        // 
        // menuFileRecent
        // 
        menuFileRecent.setName( "menuFileRecent" );
        menuFileRecent.setText( "Recent Files(R)" );
        menuFileRecent.addMouseListener( new MenuDescriptionActivator( statusLabel, "Recent projects." ) );
        // 
        // menuFileQuit
        // 
        menuFileQuit.setName( "menuFileQuit" );
        menuFileQuit.setText( "Quit(Q)" );
        menuFileQuit.addMouseListener( new MenuDescriptionActivator( statusLabel, "Close this window." ) );
        menuFileQuit.setActionCommand( "menuFileQuit_Click" );
        menuFileQuit.addActionListener( this );
        // 
        // menuEdit
        // 
        menuEdit.add( menuEditUndo );
        menuEdit.add( menuEditRedo );
        menuEdit.addSeparator();
        menuEdit.add( menuEditCut );
        menuEdit.add( menuEditCopy );
        menuEdit.add( menuEditPaste );
        menuEdit.add( menuEditDelete );
        menuEdit.addSeparator();
        menuEdit.add( menuEditAutoNormalizeMode );
        menuEdit.addSeparator();
        menuEdit.add( menuEditSelectAll );
        menuEdit.add( menuEditSelectAllEvents );
        menuEdit.setName( "menuEdit" );
        menuEdit.setText( "Edit(E)" );
        //menuEdit.DropDownOpening += new System.EventHandler( menuEdit_DropDownOpening );
        // 
        // menuEditUndo
        // 
        menuEditUndo.setName( "menuEditUndo" );
        //menuEditUndo.setSize( 220, 22 );
        menuEditUndo.setText( "Undo(U)" );
        //menuEditUndo.Click += new System.EventHandler( commonEditUndo_Click );
        // 
        // menuEditRedo
        // 
        menuEditRedo.setName( "menuEditRedo" );
        //menuEditRedo.setSize( 220, 22 );
        menuEditRedo.setText( "Redo(R)" );
        //menuEditRedo.Click += new System.EventHandler( commonEditRedo_Click );
        // 
        // menuEditCut
        // 
        menuEditCut.setName( "menuEditCut" );
        //menuEditCut.setSize( 220, 22 );
        menuEditCut.setText( "Cut(T)" );
        //menuEditCut.Click += new System.EventHandler( commonEditCut_Click );
        // 
        // menuEditCopy
        // 
        menuEditCopy.setName( "menuEditCopy" );
        //menuEditCopy.setSize( 220, 22 );
        menuEditCopy.setText( "Copy(C)" );
        //menuEditCopy.Click += new System.EventHandler( commonEditCopy_Click );
        // 
        // menuEditPaste
        // 
        menuEditPaste.setName( "menuEditPaste" );
        //menuEditPaste.ShortcutKeyDisplayString = "";
        //menuEditPaste.setSize( 220, 22 );
        menuEditPaste.setText( "Paste(P)" );
        //menuEditPaste.Click += new System.EventHandler( commonEditPaste_Click );
        // 
        // menuEditDelete
        // 
        menuEditDelete.setName( "menuEditDelete" );
        //menuEditDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
        //menuEditDelete.ShowShortcutKeys = false;
        //menuEditDelete.setSize( 220, 22 );
        menuEditDelete.setText( "Delete(D)" );
        //menuEditDelete.Click += new System.EventHandler( menuEditDelete_Click );
        // 
        // menuEditAutoNormalizeMode
        // 
        menuEditAutoNormalizeMode.setName( "menuEditAutoNormalizeMode" );
        //menuEditAutoNormalizeMode.setSize( 220, 22 );
        menuEditAutoNormalizeMode.setText( "Auto Normalize Mode(N)" );
        //menuEditAutoNormalizeMode.Click += new System.EventHandler( menuEditAutoNormalizeMode_Click );
        // 
        // menuEditSelectAll
        // 
        menuEditSelectAll.setName( "menuEditSelectAll" );
        //menuEditSelectAll.setSize( 220, 22 );
        menuEditSelectAll.setText( "Select All(A)" );
        //menuEditSelectAll.Click += new System.EventHandler( menuEditSelectAll_Click );
        // 
        // menuEditSelectAllEvents
        // 
        menuEditSelectAllEvents.setName( "menuEditSelectAllEvents" );
        //menuEditSelectAllEvents.setSize( 220, 22 );
        menuEditSelectAllEvents.setText( "Select All Events(E)" );
        //menuEditSelectAllEvents.Click += new System.EventHandler( menuEditSelectAllEvents_Click );*/
        // 
        // menuVisual
        // 
        //menuVisual.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        menuVisual.add( menuVisualControlTrack );
        menuVisual.add( menuVisualMixer );
        menuVisual.add( menuVisualWaveform );
        menuVisual.add( menuVisualProperty );
        menuVisual.addSeparator();
        menuVisual.add( menuVisualGridline );
        menuVisual.addSeparator();
        menuVisual.add( menuVisualStartMarker );
        menuVisual.add( menuVisualEndMarker );
        menuVisual.addSeparator();
        menuVisual.add( menuVisualLyrics );
        menuVisual.add( menuVisualNoteProperty );
        menuVisual.add( menuVisualPitchLine );
        menuVisual.setName( "menuVisual" );
        //menuVisual.setSize( 66, 22 );
        menuVisual.setText( "View(V)" );
        // 
        // menuVisualControlTrack
        // 
        menuVisualControlTrack.setState( true );
        //menuVisualControlTrack.CheckOnClick = true;
        //menuVisualControlTrack.CheckState = System.Windows.Forms.CheckState.Checked;
        //menuVisualControlTrack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        menuVisualControlTrack.setName( "menuVisualControlTrack" );
        //menuVisualControlTrack.Size = new System.Drawing.Size( 237, 22 );
        menuVisualControlTrack.setText( "Control Track(C)" );
        //menuVisualControlTrack.CheckedChanged += new System.EventHandler( menuVisualControlTrack_CheckedChanged );
        // 
        // menuVisualMixer
        // 
        //menuVisualMixer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        menuVisualMixer.setName( "menuVisualMixer" );
        //menuVisualMixer.Size = new System.Drawing.Size( 237, 22 );
        menuVisualMixer.setText( "Mixer(X)" );
        //menuVisualMixer.Click += new System.EventHandler( menuVisualMixer_Click );
        // 
        // menuVisualWaveform
        // 
        //menuVisualWaveform.CheckOnClick = true;
        menuVisualWaveform.setState( false );
        menuVisualWaveform.setName( "menuVisualWaveform" );
        //menuVisualWaveform.Size = new System.Drawing.Size( 237, 22 );
        menuVisualWaveform.setText( "Waveform(W)" );
        //menuVisualWaveform.CheckedChanged += new System.EventHandler( menuVisualWaveform_CheckedChanged );
        // 
        // menuVisualProperty
        // 
        //menuVisualProperty.CheckOnClick = true;
        menuVisualProperty.setState( false );
        menuVisualProperty.setName( "menuVisualProperty" );
        //menuVisualProperty.Size = new System.Drawing.Size( 237, 22 );
        menuVisualProperty.setText( "Property Window(C)" );
        //menuVisualProperty.CheckedChanged += new System.EventHandler( menuVisualProperty_CheckedChanged );
        // 
        // menuVisualGridline
        // 
        menuVisualGridline.setState( false );
        //menuVisualGridline.CheckOnClick = true;
        //menuVisualGridline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        menuVisualGridline.setName( "menuVisualGridline" );
        //menuVisualGridline.Size = new System.Drawing.Size( 237, 22 );
        menuVisualGridline.setText( "Grid Line(G)" );
        //menuVisualGridline.CheckedChanged += new System.EventHandler( menuVisualGridline_CheckedChanged );
        // 
        // menuVisualStartMarker
        // 
        menuVisualStartMarker.setState( false );
        //menuVisualStartMarker.CheckOnClick = true;
        //menuVisualStartMarker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        menuVisualStartMarker.setEnabled( false );
        menuVisualStartMarker.setName( "menuVisualStartMarker" );
        //menuVisualStartMarker.Size = new System.Drawing.Size( 237, 22 );
        menuVisualStartMarker.setText( "Start Marker(S)" );
        // 
        // menuVisualEndMarker
        // 
        menuVisualEndMarker.setState( false );
        //menuVisualEndMarker.CheckOnClick = true;
        //menuVisualEndMarker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        menuVisualEndMarker.setEnabled( false );
        menuVisualEndMarker.setName( "menuVisualEndMarker" );
        //menuVisualEndMarker.Size = new System.Drawing.Size( 237, 22 );
        menuVisualEndMarker.setText( "End Marker(E)" );
        // 
        // menuVisualLyrics
        // 
        menuVisualLyrics.setState( true );
        //menuVisualLyrics.CheckOnClick = true;
        //menuVisualLyrics.CheckState = System.Windows.Forms.CheckState.Checked;
        //menuVisualLyrics.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        menuVisualLyrics.setName( "menuVisualLyrics" );
        //menuVisualLyrics.Size = new System.Drawing.Size( 237, 22 );
        menuVisualLyrics.setText( "Lyric/Phoneme(L)" );
        //menuVisualLyrics.CheckedChanged += new System.EventHandler( menuVisualLyrics_CheckedChanged );
        // 
        // menuVisualNoteProperty
        // 
        menuVisualNoteProperty.setState( true );
        //menuVisualNoteProperty.CheckOnClick = true;
        //menuVisualNoteProperty.CheckState = System.Windows.Forms.CheckState.Checked;
        //menuVisualNoteProperty.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        menuVisualNoteProperty.setName( "menuVisualNoteProperty" );
        //menuVisualNoteProperty.Size = new System.Drawing.Size( 237, 22 );
        menuVisualNoteProperty.setText( "Note Expression/Vibrato(N)" );
        //menuVisualNoteProperty.CheckedChanged += new System.EventHandler( menuVisualNoteProperty_CheckedChanged );
        // 
        // menuVisualPitchLine
        // 
        menuVisualPitchLine.setState( false );
        //menuVisualPitchLine.CheckOnClick = true;
        menuVisualPitchLine.setName( "menuVisualPitchLine" );
        //menuVisualPitchLine.Size = new System.Drawing.Size( 237, 22 );
        menuVisualPitchLine.setText( "Pitch Line(P)" );
        //menuVisualPitchLine.CheckedChanged += new System.EventHandler( menuVisualPitchLine_CheckedChanged );*/
        // 
        // menuJob
        // 
        menuJob.add( menuJobNormalize );
        menuJob.add( menuJobInsertBar );
        menuJob.add( menuJobDeleteBar );
        menuJob.add( menuJobRandomize );
        menuJob.add( menuJobConnect );
        menuJob.add( menuJobLyric );
        menuJob.add( menuJobRewire );
        menuJob.add( menuJobRealTime );
        menuJob.add( menuJobReloadVsti );
        menuJob.setName( "menuJob" );
        //menuJob.setSize( 54, 22 );
        menuJob.setText( "Job(J)" );
        //menuJob.DropDownOpening += new System.EventHandler( menuJob_DropDownOpening );
        // 
        // menuJobNormalize
        // 
        menuJobNormalize.setName( "menuJobNormalize" );
        //menuJobNormalize.Size = new System.Drawing.Size( 256, 22 );
        menuJobNormalize.setText( "Normalize Notes(N)" );
        //menuJobNormalize.Click += new System.EventHandler( menuJobNormalize_Click );
        // 
        // menuJobInsertBar
        // 
        menuJobInsertBar.setName( "menuJobInsertBar" );
        //menuJobInsertBar.Size = new System.Drawing.Size( 256, 22 );
        menuJobInsertBar.setText( "Insert Bars(I)" );
        //menuJobInsertBar.Click += new System.EventHandler( menuJobInsertBar_Click );
        // 
        // menuJobDeleteBar
        // 
        menuJobDeleteBar.setName( "menuJobDeleteBar" );
        //menuJobDeleteBar.Size = new System.Drawing.Size( 256, 22 );
        menuJobDeleteBar.setText( "Delete Bars(D)" );
        //menuJobDeleteBar.Click += new System.EventHandler( menuJobDeleteBar_Click );
        // 
        // menuJobRandomize
        // 
        menuJobRandomize.setEnabled( false );
        menuJobRandomize.setName( "menuJobRandomize" );
        //menuJobRandomize.Size = new System.Drawing.Size( 256, 22 );
        menuJobRandomize.setText( "Randomize(R)" );
        // 
        // menuJobConnect
        // 
        menuJobConnect.setName( "menuJobConnect" );
        //menuJobConnect.Size = new System.Drawing.Size( 256, 22 );
        menuJobConnect.setText( "Connect Notes(C)" );
        //menuJobConnect.Click += new System.EventHandler( menuJobConnect_Click );
        // 
        // menuJobLyric
        // 
        menuJobLyric.setName( "menuJobLyric" );
        //menuJobLyric.Size = new System.Drawing.Size( 256, 22 );
        menuJobLyric.setText( "Insert Lyrics(L)" );
        //menuJobLyric.Click += new System.EventHandler( menuJobLyric_Click );
        // 
        // menuJobRewire
        // 
        menuJobRewire.setEnabled( false );
        menuJobRewire.setName( "menuJobRewire" );
        //menuJobRewire.Size = new System.Drawing.Size( 256, 22 );
        menuJobRewire.setText( "Import ReWire Host Tempo(T)" );
        // 
        // menuJobRealTime
        // 
        menuJobRealTime.setName( "menuJobRealTime" );
        //menuJobRealTime.Size = new System.Drawing.Size( 256, 22 );
        menuJobRealTime.setText( "Start Realtime Input" );
        //menuJobRealTime.Click += new System.EventHandler( menuJobRealTime_Click );
        // 
        // menuJobReloadVsti
        // 
        menuJobReloadVsti.setName( "menuJobReloadVsti" );
        //menuJobReloadVsti.Size = new System.Drawing.Size( 256, 22 );
        menuJobReloadVsti.setText( "Reload VSTi(R)" );
        menuJobReloadVsti.setVisible( false );
        //menuJobReloadVsti.Click += new System.EventHandler( menuJobReloadVsti_Click );*/
        // 
        // menuTrack
        // 
        menuTrack.add( menuTrackOn );
        menuTrack.addSeparator();
        menuTrack.add( menuTrackAdd );
        menuTrack.add( menuTrackCopy );
        menuTrack.add( menuTrackChangeName );
        menuTrack.add( menuTrackDelete );
        menuTrack.addSeparator();
        menuTrack.add( menuTrackRenderCurrent );
        menuTrack.add( menuTrackRenderAll );
        menuTrack.addSeparator();
        menuTrack.add( menuTrackOverlay );
        menuTrack.add( menuTrackRenderer );
        menuTrack.add( menuTrackMasterTuning );
        menuTrack.setName( "menuTrack" );
        //menuTrack.setSize( 70, 22 );
        menuTrack.setText( "Track(T)" );
        //menuTrack.DropDownOpening += new System.EventHandler( menuTrack_DropDownOpening );
        // 
        // menuTrackOn
        // 
        menuTrackOn.setName( "menuTrackOn" );
        //menuTrackOn.Size = new System.Drawing.Size( 219, 22 );
        menuTrackOn.setText( "Track On(K)" );
        //menuTrackOn.Click += new System.EventHandler( menuTrackOn_Click );
        // 
        // menuTrackAdd
        // 
        menuTrackAdd.setName( "menuTrackAdd" );
        //menuTrackAdd.Size = new System.Drawing.Size( 219, 22 );
        menuTrackAdd.setText( "Add Track(A)" );
        //menuTrackAdd.Click += new System.EventHandler( menuTrackAdd_Click );
        // 
        // menuTrackCopy
        // 
        menuTrackCopy.setName( "menuTrackCopy" );
        //menuTrackCopy.Size = new System.Drawing.Size( 219, 22 );
        menuTrackCopy.setText( "Copy Track(C)" );
        //menuTrackCopy.Click += new System.EventHandler( menuTrackCopy_Click );
        // 
        // menuTrackChangeName
        // 
        menuTrackChangeName.setName( "menuTrackChangeName" );
        //menuTrackChangeName.Size = new System.Drawing.Size( 219, 22 );
        menuTrackChangeName.setText( "Rename Track(R)" );
        //menuTrackChangeName.Click += new System.EventHandler( menuTrackChangeName_Click );
        // 
        // menuTrackDelete
        // 
        menuTrackDelete.setName( "menuTrackDelete" );
        //menuTrackDelete.Size = new System.Drawing.Size( 219, 22 );
        menuTrackDelete.setText( "Delete Track(D)" );
        //menuTrackDelete.Click += new System.EventHandler( menuTrackDelete_Click );
        // 
        // menuTrackRenderCurrent
        // 
        menuTrackRenderCurrent.setName( "menuTrackRenderCurrent" );
        //menuTrackRenderCurrent.Size = new System.Drawing.Size( 219, 22 );
        menuTrackRenderCurrent.setText( "Render Current Track(T)" );
        //menuTrackRenderCurrent.Click += new System.EventHandler( menuTrackRenderCurrent_Click );
        // 
        // menuTrackRenderAll
        // 
        menuTrackRenderAll.setEnabled( false );
        menuTrackRenderAll.setName( "menuTrackRenderAll" );
        //menuTrackRenderAll.Size = new System.Drawing.Size( 219, 22 );
        menuTrackRenderAll.setText( "Render All Tracks(S)" );
        // 
        // menuTrackOverlay
        // 
        menuTrackOverlay.setName( "menuTrackOverlay" );
        //menuTrackOverlay.Size = new System.Drawing.Size( 219, 22 );
        menuTrackOverlay.setText( "Overlay(O)" );
        //menuTrackOverlay.Click += new System.EventHandler( menuTrackOverlay_Click );
        // 
        // menuTrackRenderer
        // 
        menuTrackRenderer.add( menuTrackRendererVOCALOID1 );
        menuTrackRenderer.add( menuTrackRendererVOCALOID2 );
        menuTrackRenderer.add( menuTrackRendererUtau );
        menuTrackRenderer.setName( "menuTrackRenderer" );
        //menuTrackRenderer.Size = new System.Drawing.Size( 219, 22 );
        menuTrackRenderer.setText( "Renderer" );
        //menuTrackRenderer.DropDownOpening += new System.EventHandler( menuTrackRenderer_DropDownOpening );
        // 
        // menuTrackRendererVOCALOID1
        // 
        menuTrackRendererVOCALOID1.setName( "menuTrackRendererVOCALOID1" );
        //menuTrackRendererVOCALOID1.Size = new System.Drawing.Size( 146, 22 );
        menuTrackRendererVOCALOID1.setText( "VOCALOID1" );
        //menuTrackRendererVOCALOID1.Click += new System.EventHandler( commonRendererVOCALOID1_Click );
        // 
        // menuTrackRendererVOCALOID2
        // 
        menuTrackRendererVOCALOID2.setName( "menuTrackRendererVOCALOID2" );
        //menuTrackRendererVOCALOID2.Size = new System.Drawing.Size( 146, 22 );
        menuTrackRendererVOCALOID2.setText( "VOCALOID2" );
        //menuTrackRendererVOCALOID2.Click += new System.EventHandler( commonRendererVOCALOID2_Click );
        // 
        // menuTrackRendererUtau
        // 
        menuTrackRendererUtau.setName( "menuTrackRendererUtau" );
        //menuTrackRendererUtau.Size = new System.Drawing.Size( 146, 22 );
        menuTrackRendererUtau.setText( "UTAU" );
        //menuTrackRendererUtau.Click += new System.EventHandler( commonRendererUtau_Click );
        // 
        // menuTrackMasterTuning
        // 
        menuTrackMasterTuning.setName( "menuTrackMasterTuning" );
        //menuTrackMasterTuning.Size = new System.Drawing.Size( 219, 22 );
        menuTrackMasterTuning.setText( "Master Tuning(M)" );
        //menuTrackMasterTuning.Click += new System.EventHandler( commonMasterTuning_Click );*/
        // 
        // menuLyric
        // 
        menuLyric.add( menuLyricExpressionProperty );
        menuLyric.add( menuLyricVibratoProperty );
        menuLyric.add( menuLyricSymbol );
        menuLyric.add( menuLyricDictionary );
        menuLyric.setName( "menuLyric" );
        //menuLyric.setSize( 70, 22 );
        menuLyric.setText( "Lyrics(L)" );
        //menuLyric.DropDownOpening += new System.EventHandler( menuLyric_DropDownOpening );
        // 
        // menuLyricExpressionProperty
        // 
        menuLyricExpressionProperty.setName( "menuLyricExpressionProperty" );
        //menuLyricExpressionProperty.Size = new System.Drawing.Size( 241, 22 );
        menuLyricExpressionProperty.setText( "Note Expression Property(E)" );
        //menuLyricExpressionProperty.Click += new System.EventHandler( menuLyricExpressionProperty_Click );
        // 
        // menuLyricVibratoProperty
        // 
        menuLyricVibratoProperty.setName( "menuLyricVibratoProperty" );
        //menuLyricVibratoProperty.Size = new System.Drawing.Size( 241, 22 );
        menuLyricVibratoProperty.setText( "Note Vibrato Property(V)" );
        //menuLyricVibratoProperty.Click += new System.EventHandler( menuLyricVibratoProperty_Click );
        // 
        // menuLyricSymbol
        // 
        menuLyricSymbol.setEnabled( false );
        menuLyricSymbol.setName( "menuLyricSymbol" );
        //menuLyricSymbol.Size = new System.Drawing.Size( 241, 22 );
        menuLyricSymbol.setText( "Phoneme Transformation(T)" );
        // 
        // menuLyricDictionary
        // 
        menuLyricDictionary.setName( "menuLyricDictionary" );
        //menuLyricDictionary.Size = new System.Drawing.Size( 241, 22 );
        menuLyricDictionary.setText( "User Word Dictionary(C)" );
        //menuLyricDictionary.Click += new System.EventHandler( menuLyricDictionary_Click );
        // 
        // menuScript
        // 
        menuScript.add( menuScriptUpdate );
        menuScript.setName( "menuScript" );
        //menuScript.setSize( 72, 22 );
        menuScript.setText( "Script(C)" );
        // 
        // menuScriptUpdate
        // 
        menuScriptUpdate.setName( "menuScriptUpdate" );
        //menuScriptUpdate.Size = new System.Drawing.Size( 200, 22 );
        menuScriptUpdate.setText( "Update Script List(U)" );
        //menuScriptUpdate.Click += new System.EventHandler( menuScriptUpdate_Click );
        // 
        // menuSetting
        // 
        menuSetting.add( menuSettingPreference );
        menuSetting.add( menuSettingGameControler );
        menuSetting.add( menuSettingPaletteTool );
        menuSetting.add( menuSettingShortcut );
        menuSetting.add( menuSettingMidi );
        menuSetting.add( menuSettingUtauVoiceDB );
        menuSetting.addSeparator();
        menuSetting.add( menuSettingDefaultSingerStyle );
        menuSetting.addSeparator();
        menuSetting.add( menuSettingPositionQuantize );
        menuSetting.add( menuSettingLengthQuantize );
        menuSetting.addSeparator();
        menuSetting.add( menuSettingSingerProperty );
        menuSetting.setName( "menuSetting" );
        //menuSetting.setSize( 80, 22 );
        menuSetting.setText( "Setting(S)" );
        //menuSetting.DropDownOpening += new System.EventHandler( menuSetting_DropDownOpening );
        // 
        // menuSettingPreference
        // 
        menuSettingPreference.setName( "menuSettingPreference" );
        //menuSettingPreference.Size = new System.Drawing.Size( 223, 22 );
        menuSettingPreference.setText( "Preference(P)" );
        //menuSettingPreference.Click += new System.EventHandler( menuSettingPreference_Click );
        // 
        // menuSettingGameControler
        // 
        menuSettingGameControler.add( menuSettingGameControlerSetting );
        menuSettingGameControler.add( menuSettingGameControlerLoad );
        menuSettingGameControler.add( menuSettingGameControlerRemove );
        menuSettingGameControler.setName( "menuSettingGameControler" );
        //menuSettingGameControler.Size = new System.Drawing.Size( 223, 22 );
        menuSettingGameControler.setText( "Game Controler(G)" );
        // 
        // menuSettingGameControlerSetting
        // 
        menuSettingGameControlerSetting.setName( "menuSettingGameControlerSetting" );
        //menuSettingGameControlerSetting.Size = new System.Drawing.Size( 142, 22 );
        menuSettingGameControlerSetting.setText( "Setting(S)" );
        //menuSettingGameControlerSetting.Click += new System.EventHandler( menuSettingGameControlerSetting_Click );
        // 
        // menuSettingGameControlerLoad
        // 
        menuSettingGameControlerLoad.setName( "menuSettingGameControlerLoad" );
        //menuSettingGameControlerLoad.Size = new System.Drawing.Size( 142, 22 );
        menuSettingGameControlerLoad.setText( "Load(L)" );
        //menuSettingGameControlerLoad.Click += new System.EventHandler( menuSettingGameControlerLoad_Click );
        // 
        // menuSettingGameControlerRemove
        // 
        menuSettingGameControlerRemove.setName( "menuSettingGameControlerRemove" );
        //menuSettingGameControlerRemove.Size = new System.Drawing.Size( 142, 22 );
        menuSettingGameControlerRemove.setText( "Remove(R)" );
        //menuSettingGameControlerRemove.Click += new System.EventHandler( menuSettingGameControlerRemove_Click );
        // 
        // menuSettingPaletteTool
        // 
        menuSettingPaletteTool.setName( "menuSettingPaletteTool" );
        //menuSettingPaletteTool.Size = new System.Drawing.Size( 223, 22 );
        menuSettingPaletteTool.setText( "Palette Tool(T)" );
        // 
        // menuSettingShortcut
        // 
        menuSettingShortcut.setName( "menuSettingShortcut" );
        //menuSettingShortcut.Size = new System.Drawing.Size( 223, 22 );
        menuSettingShortcut.setText( "Shortcut Key(S)" );
        //menuSettingShortcut.Click += new System.EventHandler( menuSettingShortcut_Click );
        // 
        // menuSettingMidi
        // 
        menuSettingMidi.setName( "menuSettingMidi" );
        //menuSettingMidi.Size = new System.Drawing.Size( 223, 22 );
        menuSettingMidi.setText( "MIDI(M)" );
        //menuSettingMidi.Click += new System.EventHandler( menuSettingMidi_Click );
        // 
        // menuSettingUtauVoiceDB
        // 
        menuSettingUtauVoiceDB.setName( "menuSettingUtauVoiceDB" );
        //menuSettingUtauVoiceDB.Size = new System.Drawing.Size( 223, 22 );
        menuSettingUtauVoiceDB.setText( "UTAU Voice DB(U)" );
        //menuSettingUtauVoiceDB.Click += new System.EventHandler( menuSettingUtauVoiceDB_Click );
        // 
        // menuSettingDefaultSingerStyle
        // 
        menuSettingDefaultSingerStyle.setName( "menuSettingDefaultSingerStyle" );
        //menuSettingDefaultSingerStyle.Size = new System.Drawing.Size( 223, 22 );
        menuSettingDefaultSingerStyle.setText( "Singing Style Defaults(D)" );
        //menuSettingDefaultSingerStyle.Click += new System.EventHandler( menuSettingDefaultSingerStyle_Click );
        // 
        // menuSettingPositionQuantize
        // 
        menuSettingPositionQuantize.add( menuSettingPositionQuantize04 );
        menuSettingPositionQuantize.add( menuSettingPositionQuantize08 );
        menuSettingPositionQuantize.add( menuSettingPositionQuantize16 );
        menuSettingPositionQuantize.add( menuSettingPositionQuantize32 );
        menuSettingPositionQuantize.add( menuSettingPositionQuantize64 );
        menuSettingPositionQuantize.add( menuSettingPositionQuantize128 );
        menuSettingPositionQuantize.add( menuSettingPositionQuantizeOff );
        menuSettingPositionQuantize.addSeparator();
        menuSettingPositionQuantize.add( menuSettingPositionQuantizeTriplet );
        menuSettingPositionQuantize.setName( "menuSettingPositionQuantize" );
        //menuSettingPositionQuantize.Size = new System.Drawing.Size( 223, 22 );
        menuSettingPositionQuantize.setText( "Quantize(Q)" );
        // 
        // menuSettingPositionQuantize04
        // 
        menuSettingPositionQuantize04.setName( "menuSettingPositionQuantize04" );
        //menuSettingPositionQuantize04.Size = new System.Drawing.Size( 113, 22 );
        menuSettingPositionQuantize04.setText( "1/4" );
        //menuSettingPositionQuantize04.Click += new System.EventHandler( h_positionQuantize04 );
        // 
        // menuSettingPositionQuantize08
        // 
        menuSettingPositionQuantize08.setName( "menuSettingPositionQuantize08" );
        //menuSettingPositionQuantize08.Size = new System.Drawing.Size( 113, 22 );
        menuSettingPositionQuantize08.setText( "1/8" );
        //menuSettingPositionQuantize08.Click += new System.EventHandler( h_positionQuantize08 );
        // 
        // menuSettingPositionQuantize16
        // 
        menuSettingPositionQuantize16.setName( "menuSettingPositionQuantize16" );
        //menuSettingPositionQuantize16.Size = new System.Drawing.Size( 113, 22 );
        menuSettingPositionQuantize16.setText( "1/16" );
        //menuSettingPositionQuantize16.Click += new System.EventHandler( h_positionQuantize16 );
        // 
        // menuSettingPositionQuantize32
        // 
        menuSettingPositionQuantize32.setName( "menuSettingPositionQuantize32" );
        //menuSettingPositionQuantize32.Size = new System.Drawing.Size( 113, 22 );
        menuSettingPositionQuantize32.setText( "1/32" );
        //menuSettingPositionQuantize32.Click += new System.EventHandler( h_positionQuantize32 );
        // 
        // menuSettingPositionQuantize64
        // 
        menuSettingPositionQuantize64.setName( "menuSettingPositionQuantize64" );
        //menuSettingPositionQuantize64.Size = new System.Drawing.Size( 113, 22 );
        menuSettingPositionQuantize64.setText( "1/64" );
        //menuSettingPositionQuantize64.Click += new System.EventHandler( h_positionQuantize64 );
        // 
        // menuSettingPositionQuantize128
        // 
        menuSettingPositionQuantize128.setName( "menuSettingPositionQuantize128" );
        //menuSettingPositionQuantize128.Size = new System.Drawing.Size( 113, 22 );
        menuSettingPositionQuantize128.setText( "1/128" );
        //menuSettingPositionQuantize128.Click += new System.EventHandler( h_positionQuantize128 );
        // 
        // menuSettingPositionQuantizeOff
        // 
        menuSettingPositionQuantizeOff.setName( "menuSettingPositionQuantizeOff" );
        //menuSettingPositionQuantizeOff.Size = new System.Drawing.Size( 113, 22 );
        menuSettingPositionQuantizeOff.setText( "Off" );
        //menuSettingPositionQuantizeOff.Click += new System.EventHandler( h_positionQuantizeOff );
        // 
        // menuSettingPositionQuantizeTriplet
        // 
        menuSettingPositionQuantizeTriplet.setName( "menuSettingPositionQuantizeTriplet" );
        //menuSettingPositionQuantizeTriplet.Size = new System.Drawing.Size( 113, 22 );
        menuSettingPositionQuantizeTriplet.setText( "Triplet" );
        //menuSettingPositionQuantizeTriplet.Click += new System.EventHandler( h_positionQuantizeTriplet );
        // 
        // menuSettingLengthQuantize
        // 
        menuSettingLengthQuantize.add( menuSettingLengthQuantize04 );
        menuSettingLengthQuantize.add( menuSettingLengthQuantize08 );
        menuSettingLengthQuantize.add( menuSettingLengthQuantize16 );
        menuSettingLengthQuantize.add( menuSettingLengthQuantize32 );
        menuSettingLengthQuantize.add( menuSettingLengthQuantize64 );
        menuSettingLengthQuantize.add( menuSettingLengthQuantize128 );
        menuSettingLengthQuantize.add( menuSettingLengthQuantizeOff );
        menuSettingLengthQuantize.addSeparator();
        menuSettingLengthQuantize.add( menuSettingLengthQuantizeTriplet );
        menuSettingLengthQuantize.setName( "menuSettingLengthQuantize" );
        //menuSettingLengthQuantize.Size = new System.Drawing.Size( 223, 22 );
        menuSettingLengthQuantize.setText( "Length(L)" );
        // 
        // menuSettingLengthQuantize04
        // 
        menuSettingLengthQuantize04.setName( "menuSettingLengthQuantize04" );
        //menuSettingLengthQuantize04.Size = new System.Drawing.Size( 113, 22 );
        menuSettingLengthQuantize04.setText( "1/4" );
        //menuSettingLengthQuantize04.Click += new System.EventHandler( h_lengthQuantize04 );
        // 
        // menuSettingLengthQuantize08
        // 
        menuSettingLengthQuantize08.setName( "menuSettingLengthQuantize08" );
        //menuSettingLengthQuantize08.Size = new System.Drawing.Size( 113, 22 );
        menuSettingLengthQuantize08.setText( "1/8" );
        //menuSettingLengthQuantize08.Click += new System.EventHandler( h_lengthQuantize08 );
        // 
        // menuSettingLengthQuantize16
        // 
        menuSettingLengthQuantize16.setName( "menuSettingLengthQuantize16" );
        //menuSettingLengthQuantize16.Size = new System.Drawing.Size( 113, 22 );
        menuSettingLengthQuantize16.setText( "1/16" );
        //menuSettingLengthQuantize16.Click += new System.EventHandler( h_lengthQuantize16 );
        // 
        // menuSettingLengthQuantize32
        // 
        menuSettingLengthQuantize32.setName( "menuSettingLengthQuantize32" );
        //menuSettingLengthQuantize32.Size = new System.Drawing.Size( 113, 22 );
        menuSettingLengthQuantize32.setText( "1/32" );
        //menuSettingLengthQuantize32.Click += new System.EventHandler( h_lengthQuantize32 );
        // 
        // menuSettingLengthQuantize64
        // 
        menuSettingLengthQuantize64.setName( "menuSettingLengthQuantize64" );
        //menuSettingLengthQuantize64.Size = new System.Drawing.Size( 113, 22 );
        menuSettingLengthQuantize64.setText( "1/64" );
        //menuSettingLengthQuantize64.Click += new System.EventHandler( h_lengthQuantize64 );
        // 
        // menuSettingLengthQuantize128
        // 
        menuSettingLengthQuantize128.setName( "menuSettingLengthQuantize128" );
        //menuSettingLengthQuantize128.Size = new System.Drawing.Size( 113, 22 );
        menuSettingLengthQuantize128.setText( "1/128" );
        //menuSettingLengthQuantize128.Click += new System.EventHandler( h_lengthQuantize128 );
        // 
        // menuSettingLengthQuantizeOff
        // 
        menuSettingLengthQuantizeOff.setName( "menuSettingLengthQuantizeOff" );
        //menuSettingLengthQuantizeOff.Size = new System.Drawing.Size( 113, 22 );
        menuSettingLengthQuantizeOff.setText( "Off" );
        //menuSettingLengthQuantizeOff.Click += new System.EventHandler( h_lengthQuantizeOff );
        // 
        // menuSettingLengthQuantizeTriplet
        // 
        menuSettingLengthQuantizeTriplet.setName( "menuSettingLengthQuantizeTriplet" );
        //menuSettingLengthQuantizeTriplet.Size = new System.Drawing.Size( 113, 22 );
        menuSettingLengthQuantizeTriplet.setText( "Triplet" );
        //menuSettingLengthQuantizeTriplet.Click += new System.EventHandler( h_lengthQuantizeTriplet );
        // 
        // menuSettingSingerProperty
        // 
        menuSettingSingerProperty.setEnabled( false );
        menuSettingSingerProperty.setName( "menuSettingSingerProperty" );
        //menuSettingSingerProperty.Size = new System.Drawing.Size( 223, 22 );
        menuSettingSingerProperty.setText( "Singer Properties(S)" );
        // 
        // menuHelp
        // 
        menuHelp.add( menuHelpAbout );
        menuHelp.add( menuHelpDebug );
        menuHelp.setName( "menuHelp" );
        //menuHelp.setSize( 65, 22 );
        menuHelp.setText( "Help(H)" );
        // 
        // menuHelpAbout
        // 
        menuHelpAbout.setName( "menuHelpAbout" );
        //menuHelpAbout.Size = new System.Drawing.Size( 180, 22 );
        menuHelpAbout.setText( "About Cadencii(A)" );
        //menuHelpAbout.Click += new System.EventHandler( menuHelpAbout_Click );
        // 
        // menuHelpDebug
        // 
        menuHelpDebug.setName( "menuHelpDebug" );
        //menuHelpDebug.Size = new System.Drawing.Size( 180, 22 );
        menuHelpDebug.setText( "Debug" );
        menuHelpDebug.setVisible( false );
        //menuHelpDebug.Click += new System.EventHandler( menuHelpDebug_Click );
        // 
        // menuHidden
        // 
        /*menuHidden.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
        menuHiddenEditLyric,
        menuHiddenEditFlipToolPointerPencil,
        menuHiddenEditFlipToolPointerEraser,
        menuHiddenVisualForwardParameter,
        menuHiddenVisualBackwardParameter,
        menuHiddenTrackNext,
        menuHiddenTrackBack,
        menuHiddenCopy,
        menuHiddenPaste,
        menuHiddenCut} );*/
        menuHidden.setName( "menuHidden" );
        //menuHidden.setSize( 91, 22 );
        menuHidden.setText( "MenuHidden" );
        menuHidden.setVisible( false );
        /*// 
        // menuHiddenEditLyric
        // 
        menuHiddenEditLyric.setName( "menuHiddenEditLyric";
        menuHiddenEditLyric.ShortcutKeys = System.Windows.Forms.Keys.F2;
        menuHiddenEditLyric.Size = new System.Drawing.Size( 304, 22 );
        menuHiddenEditLyric.setText( "Start Lyric Input";
        menuHiddenEditLyric.Visible = false;
        menuHiddenEditLyric.Click += new System.EventHandler( menuHiddenEditLyric_Click );
        // 
        // menuHiddenEditFlipToolPointerPencil
        // 
        menuHiddenEditFlipToolPointerPencil.setName( "menuHiddenEditFlipToolPointerPencil";
        menuHiddenEditFlipToolPointerPencil.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
        menuHiddenEditFlipToolPointerPencil.Size = new System.Drawing.Size( 304, 22 );
        menuHiddenEditFlipToolPointerPencil.setText( "Change Tool Pointer / Pencil";
        menuHiddenEditFlipToolPointerPencil.Visible = false;
        menuHiddenEditFlipToolPointerPencil.Click += new System.EventHandler( menuHiddenEditFlipToolPointerPencil_Click );
        // 
        // menuHiddenEditFlipToolPointerEraser
        // 
        menuHiddenEditFlipToolPointerEraser.setName( "menuHiddenEditFlipToolPointerEraser";
        menuHiddenEditFlipToolPointerEraser.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
        menuHiddenEditFlipToolPointerEraser.Size = new System.Drawing.Size( 304, 22 );
        menuHiddenEditFlipToolPointerEraser.setText( "Change Tool Pointer/ Eraser";
        menuHiddenEditFlipToolPointerEraser.Visible = false;
        menuHiddenEditFlipToolPointerEraser.Click += new System.EventHandler( menuHiddenEditFlipToolPointerEraser_Click );
        // 
        // menuHiddenVisualForwardParameter
        // 
        menuHiddenVisualForwardParameter.setName( "menuHiddenVisualForwardParameter";
        menuHiddenVisualForwardParameter.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                    | System.Windows.Forms.Keys.Next)));
        menuHiddenVisualForwardParameter.Size = new System.Drawing.Size( 304, 22 );
        menuHiddenVisualForwardParameter.setText( "Next Control Curve";
        menuHiddenVisualForwardParameter.Visible = false;
        menuHiddenVisualForwardParameter.Click += new System.EventHandler( menuHiddenVisualForwardParameter_Click );
        // 
        // menuHiddenVisualBackwardParameter
        // 
        menuHiddenVisualBackwardParameter.setName( "menuHiddenVisualBackwardParameter";
        menuHiddenVisualBackwardParameter.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                    | System.Windows.Forms.Keys.PageUp)));
        menuHiddenVisualBackwardParameter.Size = new System.Drawing.Size( 304, 22 );
        menuHiddenVisualBackwardParameter.setText( "Previous Control Curve";
        menuHiddenVisualBackwardParameter.Visible = false;
        menuHiddenVisualBackwardParameter.Click += new System.EventHandler( menuHiddenVisualBackwardParameter_Click );
        // 
        // menuHiddenTrackNext
        // 
        menuHiddenTrackNext.setName( "menuHiddenTrackNext";
        menuHiddenTrackNext.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Next)));
        menuHiddenTrackNext.Size = new System.Drawing.Size( 304, 22 );
        menuHiddenTrackNext.setText( "Next Track";
        menuHiddenTrackNext.Visible = false;
        menuHiddenTrackNext.Click += new System.EventHandler( menuHiddenTrackNext_Click );
        // 
        // menuHiddenTrackBack
        // 
        menuHiddenTrackBack.setName( "menuHiddenTrackBack";
        menuHiddenTrackBack.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.PageUp)));
        menuHiddenTrackBack.Size = new System.Drawing.Size( 304, 22 );
        menuHiddenTrackBack.setText( "Previous Track";
        menuHiddenTrackBack.Visible = false;
        menuHiddenTrackBack.Click += new System.EventHandler( menuHiddenTrackBack_Click );
        // 
        // menuHiddenCopy
        // 
        menuHiddenCopy.setName( "menuHiddenCopy";
        menuHiddenCopy.Size = new System.Drawing.Size( 304, 22 );
        menuHiddenCopy.setText( "Copy";
        menuHiddenCopy.Click += new System.EventHandler( commonEditCopy_Click );
        // 
        // menuHiddenPaste
        // 
        menuHiddenPaste.setName( "menuHiddenPaste";
        menuHiddenPaste.Size = new System.Drawing.Size( 304, 22 );
        menuHiddenPaste.setText( "Paste";
        menuHiddenPaste.Click += new System.EventHandler( commonEditPaste_Click );
        // 
        // menuHiddenCut
        // 
        menuHiddenCut.setName( "menuHiddenCut";
        menuHiddenCut.Size = new System.Drawing.Size( 304, 22 );
        menuHiddenCut.setText( "Cut";
        menuHiddenCut.Click += new System.EventHandler( commonEditCut_Click );
        // 
        // saveXmlVsqDialog
        // 
        saveXmlVsqDialog.Filter = "VSQ Format(*.vsq)|*.vsq|Original Format(*.evsq)|*.evsq|All files(*.*)|*.*";
        // 
        // cMenuPiano
        // 
        cMenuPiano.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
        cMenuPianoPointer,
        cMenuPianoPencil,
        cMenuPianoEraser,
        cMenuPianoPaletteTool,
        toolStripSeparator15,
        cMenuPianoCurve,
        toolStripMenuItem13,
        cMenuPianoFixed,
        cMenuPianoQuantize,
        cMenuPianoLength,
        cMenuPianoGrid,
        toolStripMenuItem14,
        cMenuPianoUndo,
        cMenuPianoRedo,
        toolStripMenuItem15,
        cMenuPianoCut,
        cMenuPianoCopy,
        cMenuPianoPaste,
        cMenuPianoDelete,
        toolStripMenuItem16,
        cMenuPianoSelectAll,
        cMenuPianoSelectAllEvents,
        toolStripMenuItem17,
        cMenuPianoImportLyric,
        cMenuPianoExpressionProperty,
        cMenuPianoVibratoProperty} );
        cMenuPiano.setName( "cMenuPiano";
        cMenuPiano.ShowCheckMargin = true;
        cMenuPiano.ShowImageMargin = false;
        cMenuPiano.Size = new System.Drawing.Size( 242, 480 );
        cMenuPiano.Opening += new System.ComponentModel.CancelEventHandler( cMenuPiano_Opening );
        // 
        // cMenuPianoPointer
        // 
        cMenuPianoPointer.setName( "cMenuPianoPointer";
        cMenuPianoPointer.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoPointer.setText( "Arrow(&A)";
        cMenuPianoPointer.Click += new System.EventHandler( cMenuPianoPointer_Click );
        // 
        // cMenuPianoPencil
        // 
        cMenuPianoPencil.setName( "cMenuPianoPencil";
        cMenuPianoPencil.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoPencil.setText( "Pencil(&W)";
        cMenuPianoPencil.Click += new System.EventHandler( cMenuPianoPencil_Click );
        // 
        // cMenuPianoEraser
        // 
        cMenuPianoEraser.setName( "cMenuPianoEraser";
        cMenuPianoEraser.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoEraser.setText( "Eraser(&E)";
        cMenuPianoEraser.Click += new System.EventHandler( cMenuPianoEraser_Click );
        // 
        // cMenuPianoPaletteTool
        // 
        cMenuPianoPaletteTool.setName( "cMenuPianoPaletteTool";
        cMenuPianoPaletteTool.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoPaletteTool.setText( "Palette Tool";
        // 
        // toolStripSeparator15
        // 
        toolStripSeparator15.setName( "toolStripSeparator15";
        toolStripSeparator15.Size = new System.Drawing.Size( 238, 6 );
        // 
        // cMenuPianoCurve
        // 
        cMenuPianoCurve.setName( "cMenuPianoCurve";
        cMenuPianoCurve.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoCurve.setText( "Curve(&V)";
        cMenuPianoCurve.Click += new System.EventHandler( cMenuPianoCurve_Click );
        // 
        // toolStripMenuItem13
        // 
        toolStripMenuItem13.setName( "toolStripMenuItem13";
        toolStripMenuItem13.Size = new System.Drawing.Size( 238, 6 );
        // 
        // cMenuPianoFixed
        // 
        cMenuPianoFixed.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
        cMenuPianoFixed01,
        cMenuPianoFixed02,
        cMenuPianoFixed04,
        cMenuPianoFixed08,
        cMenuPianoFixed16,
        cMenuPianoFixed32,
        cMenuPianoFixed64,
        cMenuPianoFixed128,
        cMenuPianoFixedOff,
        toolStripMenuItem18,
        cMenuPianoFixedTriplet,
        cMenuPianoFixedDotted} );
        cMenuPianoFixed.setName( "cMenuPianoFixed";
        cMenuPianoFixed.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoFixed.setText( "Note Fixed Length(&N)";
        // 
        // cMenuPianoFixed01
        // 
        cMenuPianoFixed01.setName( "cMenuPianoFixed01";
        cMenuPianoFixed01.Size = new System.Drawing.Size( 141, 22 );
        cMenuPianoFixed01.setText( "1/ 1 [1920]";
        cMenuPianoFixed01.Click += new System.EventHandler( cMenuPianoFixed01_Click );
        // 
        // cMenuPianoFixed02
        // 
        cMenuPianoFixed02.setName( "cMenuPianoFixed02";
        cMenuPianoFixed02.Size = new System.Drawing.Size( 141, 22 );
        cMenuPianoFixed02.setText( "1/ 2 [960]";
        cMenuPianoFixed02.Click += new System.EventHandler( cMenuPianoFixed02_Click );
        // 
        // cMenuPianoFixed04
        // 
        cMenuPianoFixed04.setName( "cMenuPianoFixed04";
        cMenuPianoFixed04.Size = new System.Drawing.Size( 141, 22 );
        cMenuPianoFixed04.setText( "1/ 4 [480]";
        cMenuPianoFixed04.Click += new System.EventHandler( cMenuPianoFixed04_Click );
        // 
        // cMenuPianoFixed08
        // 
        cMenuPianoFixed08.setName( "cMenuPianoFixed08";
        cMenuPianoFixed08.Size = new System.Drawing.Size( 141, 22 );
        cMenuPianoFixed08.setText( "1/ 8 [240]";
        cMenuPianoFixed08.Click += new System.EventHandler( cMenuPianoFixed08_Click );
        // 
        // cMenuPianoFixed16
        // 
        cMenuPianoFixed16.setName( "cMenuPianoFixed16";
        cMenuPianoFixed16.Size = new System.Drawing.Size( 141, 22 );
        cMenuPianoFixed16.setText( "1/16 [120]";
        cMenuPianoFixed16.Click += new System.EventHandler( cMenuPianoFixed16_Click );
        // 
        // cMenuPianoFixed32
        // 
        cMenuPianoFixed32.setName( "cMenuPianoFixed32";
        cMenuPianoFixed32.Size = new System.Drawing.Size( 141, 22 );
        cMenuPianoFixed32.setText( "1/32 [60]";
        cMenuPianoFixed32.Click += new System.EventHandler( cMenuPianoFixed32_Click );
        // 
        // cMenuPianoFixed64
        // 
        cMenuPianoFixed64.setName( "cMenuPianoFixed64";
        cMenuPianoFixed64.Size = new System.Drawing.Size( 141, 22 );
        cMenuPianoFixed64.setText( "1/64 [30]";
        cMenuPianoFixed64.Click += new System.EventHandler( cMenuPianoFixed64_Click );
        // 
        // cMenuPianoFixed128
        // 
        cMenuPianoFixed128.setName( "cMenuPianoFixed128";
        cMenuPianoFixed128.Size = new System.Drawing.Size( 141, 22 );
        cMenuPianoFixed128.setText( "1/128[15]";
        cMenuPianoFixed128.Click += new System.EventHandler( cMenuPianoFixed128_Click );
        // 
        // cMenuPianoFixedOff
        // 
        cMenuPianoFixedOff.setName( "cMenuPianoFixedOff";
        cMenuPianoFixedOff.Size = new System.Drawing.Size( 141, 22 );
        cMenuPianoFixedOff.setText( "オフ";
        cMenuPianoFixedOff.Click += new System.EventHandler( cMenuPianoFixedOff_Click );
        // 
        // toolStripMenuItem18
        // 
        toolStripMenuItem18.setName( "toolStripMenuItem18";
        toolStripMenuItem18.Size = new System.Drawing.Size( 138, 6 );
        // 
        // cMenuPianoFixedTriplet
        // 
        cMenuPianoFixedTriplet.setName( "cMenuPianoFixedTriplet";
        cMenuPianoFixedTriplet.Size = new System.Drawing.Size( 141, 22 );
        cMenuPianoFixedTriplet.setText( "3連符";
        cMenuPianoFixedTriplet.Click += new System.EventHandler( cMenuPianoFixedTriplet_Click );
        // 
        // cMenuPianoFixedDotted
        // 
        cMenuPianoFixedDotted.setName( "cMenuPianoFixedDotted";
        cMenuPianoFixedDotted.Size = new System.Drawing.Size( 141, 22 );
        cMenuPianoFixedDotted.setText( "付点";
        cMenuPianoFixedDotted.Click += new System.EventHandler( cMenuPianoFixedDotted_Click );
        // 
        // cMenuPianoQuantize
        // 
        cMenuPianoQuantize.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
        cMenuPianoQuantize04,
        cMenuPianoQuantize08,
        cMenuPianoQuantize16,
        cMenuPianoQuantize32,
        cMenuPianoQuantize64,
        cMenuPianoQuantize128,
        cMenuPianoQuantizeOff,
        toolStripMenuItem26,
        cMenuPianoQuantizeTriplet} );
        cMenuPianoQuantize.setName( "cMenuPianoQuantize";
        cMenuPianoQuantize.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoQuantize.setText( "Quantize(&Q)";
        // 
        // cMenuPianoQuantize04
        // 
        cMenuPianoQuantize04.setName( "cMenuPianoQuantize04";
        cMenuPianoQuantize04.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoQuantize04.setText( "1/4";
        cMenuPianoQuantize04.Click += new System.EventHandler( h_positionQuantize04 );
        // 
        // cMenuPianoQuantize08
        // 
        cMenuPianoQuantize08.setName( "cMenuPianoQuantize08";
        cMenuPianoQuantize08.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoQuantize08.setText( "1/8";
        cMenuPianoQuantize08.Click += new System.EventHandler( h_positionQuantize08 );
        // 
        // cMenuPianoQuantize16
        // 
        cMenuPianoQuantize16.setName( "cMenuPianoQuantize16";
        cMenuPianoQuantize16.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoQuantize16.setText( "1/16";
        cMenuPianoQuantize16.Click += new System.EventHandler( h_positionQuantize16 );
        // 
        // cMenuPianoQuantize32
        // 
        cMenuPianoQuantize32.setName( "cMenuPianoQuantize32";
        cMenuPianoQuantize32.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoQuantize32.setText( "1/32";
        cMenuPianoQuantize32.Click += new System.EventHandler( h_positionQuantize32 );
        // 
        // cMenuPianoQuantize64
        // 
        cMenuPianoQuantize64.setName( "cMenuPianoQuantize64";
        cMenuPianoQuantize64.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoQuantize64.setText( "1/64";
        cMenuPianoQuantize64.Click += new System.EventHandler( h_positionQuantize64 );
        // 
        // cMenuPianoQuantize128
        // 
        cMenuPianoQuantize128.setName( "cMenuPianoQuantize128";
        cMenuPianoQuantize128.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoQuantize128.setText( "1/128";
        cMenuPianoQuantize128.Click += new System.EventHandler( h_positionQuantize128 );
        // 
        // cMenuPianoQuantizeOff
        // 
        cMenuPianoQuantizeOff.setName( "cMenuPianoQuantizeOff";
        cMenuPianoQuantizeOff.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoQuantizeOff.setText( "オフ";
        cMenuPianoQuantizeOff.Click += new System.EventHandler( h_positionQuantizeOff );
        // 
        // toolStripMenuItem26
        // 
        toolStripMenuItem26.setName( "toolStripMenuItem26";
        toolStripMenuItem26.Size = new System.Drawing.Size( 106, 6 );
        // 
        // cMenuPianoQuantizeTriplet
        // 
        cMenuPianoQuantizeTriplet.setName( "cMenuPianoQuantizeTriplet";
        cMenuPianoQuantizeTriplet.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoQuantizeTriplet.setText( "3連符";
        cMenuPianoQuantizeTriplet.Click += new System.EventHandler( h_positionQuantizeTriplet );
        // 
        // cMenuPianoLength
        // 
        cMenuPianoLength.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
        cMenuPianoLength04,
        cMenuPianoLength08,
        cMenuPianoLength16,
        cMenuPianoLength32,
        cMenuPianoLength64,
        cMenuPianoLength128,
        cMenuPianoLengthOff,
        toolStripMenuItem32,
        cMenuPianoLengthTriplet} );
        cMenuPianoLength.setName( "cMenuPianoLength";
        cMenuPianoLength.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoLength.setText( "Length(&L)";
        // 
        // cMenuPianoLength04
        // 
        cMenuPianoLength04.setName( "cMenuPianoLength04";
        cMenuPianoLength04.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoLength04.setText( "1/4";
        cMenuPianoLength04.Click += new System.EventHandler( h_lengthQuantize04 );
        // 
        // cMenuPianoLength08
        // 
        cMenuPianoLength08.setName( "cMenuPianoLength08";
        cMenuPianoLength08.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoLength08.setText( "1/8";
        cMenuPianoLength08.Click += new System.EventHandler( h_lengthQuantize08 );
        // 
        // cMenuPianoLength16
        // 
        cMenuPianoLength16.setName( "cMenuPianoLength16";
        cMenuPianoLength16.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoLength16.setText( "1/16";
        cMenuPianoLength16.Click += new System.EventHandler( h_lengthQuantize16 );
        // 
        // cMenuPianoLength32
        // 
        cMenuPianoLength32.setName( "cMenuPianoLength32";
        cMenuPianoLength32.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoLength32.setText( "1/32";
        cMenuPianoLength32.Click += new System.EventHandler( h_lengthQuantize32 );
        // 
        // cMenuPianoLength64
        // 
        cMenuPianoLength64.setName( "cMenuPianoLength64";
        cMenuPianoLength64.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoLength64.setText( "1/64";
        cMenuPianoLength64.Click += new System.EventHandler( h_lengthQuantize64 );
        // 
        // cMenuPianoLength128
        // 
        cMenuPianoLength128.setName( "cMenuPianoLength128";
        cMenuPianoLength128.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoLength128.setText( "1/128";
        cMenuPianoLength128.Click += new System.EventHandler( h_lengthQuantize128 );
        // 
        // cMenuPianoLengthOff
        // 
        cMenuPianoLengthOff.setName( "cMenuPianoLengthOff";
        cMenuPianoLengthOff.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoLengthOff.setText( "オフ";
        cMenuPianoLengthOff.Click += new System.EventHandler( h_lengthQuantizeOff );
        // 
        // toolStripMenuItem32
        // 
        toolStripMenuItem32.setName( "toolStripMenuItem32";
        toolStripMenuItem32.Size = new System.Drawing.Size( 106, 6 );
        // 
        // cMenuPianoLengthTriplet
        // 
        cMenuPianoLengthTriplet.setName( "cMenuPianoLengthTriplet";
        cMenuPianoLengthTriplet.Size = new System.Drawing.Size( 109, 22 );
        cMenuPianoLengthTriplet.setText( "3連符";
        cMenuPianoLengthTriplet.Click += new System.EventHandler( h_lengthQuantizeTriplet );
        // 
        // cMenuPianoGrid
        // 
        cMenuPianoGrid.setName( "cMenuPianoGrid";
        cMenuPianoGrid.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoGrid.setText( "Show/Hide Grid Line(&S)";
        cMenuPianoGrid.Click += new System.EventHandler( cMenuPianoGrid_Click );
        // 
        // toolStripMenuItem14
        // 
        toolStripMenuItem14.setName( "toolStripMenuItem14";
        toolStripMenuItem14.Size = new System.Drawing.Size( 238, 6 );
        // 
        // cMenuPianoUndo
        // 
        cMenuPianoUndo.setName( "cMenuPianoUndo";
        cMenuPianoUndo.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoUndo.setText( "Undo(&U)";
        cMenuPianoUndo.Click += new System.EventHandler( cMenuPianoUndo_Click );
        // 
        // cMenuPianoRedo
        // 
        cMenuPianoRedo.setName( "cMenuPianoRedo";
        cMenuPianoRedo.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoRedo.setText( "Redo(&R)";
        cMenuPianoRedo.Click += new System.EventHandler( cMenuPianoRedo_Click );
        // 
        // toolStripMenuItem15
        // 
        toolStripMenuItem15.setName( "toolStripMenuItem15";
        toolStripMenuItem15.Size = new System.Drawing.Size( 238, 6 );
        // 
        // cMenuPianoCut
        // 
        cMenuPianoCut.setName( "cMenuPianoCut";
        cMenuPianoCut.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoCut.setText( "Cut(&T)";
        cMenuPianoCut.Click += new System.EventHandler( cMenuPianoCut_Click );
        // 
        // cMenuPianoCopy
        // 
        cMenuPianoCopy.setName( "cMenuPianoCopy";
        cMenuPianoCopy.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoCopy.setText( "Copy(&C)";
        cMenuPianoCopy.Click += new System.EventHandler( cMenuPianoCopy_Click );
        // 
        // cMenuPianoPaste
        // 
        cMenuPianoPaste.setName( "cMenuPianoPaste";
        cMenuPianoPaste.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoPaste.setText( "Paste(&P)";
        cMenuPianoPaste.Click += new System.EventHandler( cMenuPianoPaste_Click );
        // 
        // cMenuPianoDelete
        // 
        cMenuPianoDelete.setName( "cMenuPianoDelete";
        cMenuPianoDelete.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoDelete.setText( "Delete(&D)";
        cMenuPianoDelete.Click += new System.EventHandler( cMenuPianoDelete_Click );
        // 
        // toolStripMenuItem16
        // 
        toolStripMenuItem16.setName( "toolStripMenuItem16";
        toolStripMenuItem16.Size = new System.Drawing.Size( 238, 6 );
        // 
        // cMenuPianoSelectAll
        // 
        cMenuPianoSelectAll.setName( "cMenuPianoSelectAll";
        cMenuPianoSelectAll.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoSelectAll.setText( "Select All(&A)";
        cMenuPianoSelectAll.Click += new System.EventHandler( cMenuPianoSelectAll_Click );
        // 
        // cMenuPianoSelectAllEvents
        // 
        cMenuPianoSelectAllEvents.setName( "cMenuPianoSelectAllEvents";
        cMenuPianoSelectAllEvents.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoSelectAllEvents.setText( "Select All Events(&E)";
        cMenuPianoSelectAllEvents.Click += new System.EventHandler( cMenuPianoSelectAllEvents_Click );
        // 
        // toolStripMenuItem17
        // 
        toolStripMenuItem17.setName( "toolStripMenuItem17";
        toolStripMenuItem17.Size = new System.Drawing.Size( 238, 6 );
        // 
        // cMenuPianoImportLyric
        // 
        cMenuPianoImportLyric.setName( "cMenuPianoImportLyric";
        cMenuPianoImportLyric.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoImportLyric.setText( "Insert Lyrics(&L)";
        cMenuPianoImportLyric.Click += new System.EventHandler( cMenuPianoImportLyric_Click );
        // 
        // cMenuPianoExpressionProperty
        // 
        cMenuPianoExpressionProperty.setName( "cMenuPianoExpressionProperty";
        cMenuPianoExpressionProperty.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoExpressionProperty.setText( "Note Expression Property(&P)";
        cMenuPianoExpressionProperty.Click += new System.EventHandler( cMenuPianoProperty_Click );
        // 
        // cMenuPianoVibratoProperty
        // 
        cMenuPianoVibratoProperty.setName( "cMenuPianoVibratoProperty";
        cMenuPianoVibratoProperty.Size = new System.Drawing.Size( 241, 22 );
        cMenuPianoVibratoProperty.setText( "Note Vibrato Property";
        cMenuPianoVibratoProperty.Click += new System.EventHandler( cMenuPianoVibratoProperty_Click );
        // 
        // openXmlVsqDialog
        // 
        openXmlVsqDialog.Filter = "VSQ Format(*.vsq)|*.vsq|Original Format(*.evsq)|*.evsq";
        // 
        // cMenuTrackTab
        // 
        cMenuTrackTab.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
        cMenuTrackTabTrackOn,
        toolStripMenuItem24,
        cMenuTrackTabAdd,
        cMenuTrackTabCopy,
        cMenuTrackTabChangeName,
        cMenuTrackTabDelete,
        toolStripMenuItem25,
        cMenuTrackTabRenderCurrent,
        cMenuTrackTabRenderAll,
        toolStripMenuItem27,
        cMenuTrackTabOverlay,
        cMenuTrackTabRenderer,
        cMenuTrackTabMasterTuning} );
        cMenuTrackTab.setName( "cMenuTrackTab";
        cMenuTrackTab.Size = new System.Drawing.Size( 220, 242 );
        cMenuTrackTab.Opening += new System.ComponentModel.CancelEventHandler( cMenuTrackTab_Opening );
        // 
        // cMenuTrackTabTrackOn
        // 
        cMenuTrackTabTrackOn.setName( "cMenuTrackTabTrackOn";
        cMenuTrackTabTrackOn.Size = new System.Drawing.Size( 219, 22 );
        cMenuTrackTabTrackOn.setText( "Track On(&K)";
        cMenuTrackTabTrackOn.Click += new System.EventHandler( cMenuTrackTabTrackOn_Click );
        // 
        // toolStripMenuItem24
        // 
        toolStripMenuItem24.setName( "toolStripMenuItem24";
        toolStripMenuItem24.Size = new System.Drawing.Size( 216, 6 );
        // 
        // cMenuTrackTabAdd
        // 
        cMenuTrackTabAdd.setName( "cMenuTrackTabAdd";
        cMenuTrackTabAdd.Size = new System.Drawing.Size( 219, 22 );
        cMenuTrackTabAdd.setText( "Add Track(&A)";
        cMenuTrackTabAdd.Click += new System.EventHandler( cMenuTrackTabAdd_Click );
        // 
        // cMenuTrackTabCopy
        // 
        cMenuTrackTabCopy.setName( "cMenuTrackTabCopy";
        cMenuTrackTabCopy.Size = new System.Drawing.Size( 219, 22 );
        cMenuTrackTabCopy.setText( "Copy Track(&C)";
        cMenuTrackTabCopy.Click += new System.EventHandler( cMenuTrackTabCopy_Click );
        // 
        // cMenuTrackTabChangeName
        // 
        cMenuTrackTabChangeName.setName( "cMenuTrackTabChangeName";
        cMenuTrackTabChangeName.Size = new System.Drawing.Size( 219, 22 );
        cMenuTrackTabChangeName.setText( "Rename Track(&R)";
        cMenuTrackTabChangeName.Click += new System.EventHandler( cMenuTrackTabChangeName_Click );
        // 
        // cMenuTrackTabDelete
        // 
        cMenuTrackTabDelete.setName( "cMenuTrackTabDelete";
        cMenuTrackTabDelete.Size = new System.Drawing.Size( 219, 22 );
        cMenuTrackTabDelete.setText( "Delete Track(&D)";
        cMenuTrackTabDelete.Click += new System.EventHandler( cMenuTrackTabDelete_Click );
        // 
        // toolStripMenuItem25
        // 
        toolStripMenuItem25.setName( "toolStripMenuItem25";
        toolStripMenuItem25.Size = new System.Drawing.Size( 216, 6 );
        // 
        // cMenuTrackTabRenderCurrent
        // 
        cMenuTrackTabRenderCurrent.setName( "cMenuTrackTabRenderCurrent";
        cMenuTrackTabRenderCurrent.Size = new System.Drawing.Size( 219, 22 );
        cMenuTrackTabRenderCurrent.setText( "Render Current Track(&T)";
        cMenuTrackTabRenderCurrent.Click += new System.EventHandler( cMenuTrackTabRenderCurrent_Click );
        // 
        // cMenuTrackTabRenderAll
        // 
        cMenuTrackTabRenderAll.setName( "cMenuTrackTabRenderAll";
        cMenuTrackTabRenderAll.Size = new System.Drawing.Size( 219, 22 );
        cMenuTrackTabRenderAll.setText( "Render All Tracks(&S)";
        // 
        // toolStripMenuItem27
        // 
        toolStripMenuItem27.setName( "toolStripMenuItem27";
        toolStripMenuItem27.Size = new System.Drawing.Size( 216, 6 );
        // 
        // cMenuTrackTabOverlay
        // 
        cMenuTrackTabOverlay.setName( "cMenuTrackTabOverlay";
        cMenuTrackTabOverlay.Size = new System.Drawing.Size( 219, 22 );
        cMenuTrackTabOverlay.setText( "Overlay(&O)";
        cMenuTrackTabOverlay.Click += new System.EventHandler( cMenuTrackTabOverlay_Click );
        // 
        // cMenuTrackTabRenderer
        // 
        cMenuTrackTabRenderer.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
        cMenuTrackTabRendererVOCALOID1,
        cMenuTrackTabRendererVOCALOID2,
        cMenuTrackTabRendererUtau} );
        cMenuTrackTabRenderer.setName( "cMenuTrackTabRenderer";
        cMenuTrackTabRenderer.Size = new System.Drawing.Size( 219, 22 );
        cMenuTrackTabRenderer.setText( "Renderer";
        cMenuTrackTabRenderer.DropDownOpening += new System.EventHandler( cMenuTrackTabRenderer_DropDownOpening );
        // 
        // cMenuTrackTabRendererVOCALOID1
        // 
        cMenuTrackTabRendererVOCALOID1.setName( "cMenuTrackTabRendererVOCALOID1";
        cMenuTrackTabRendererVOCALOID1.Size = new System.Drawing.Size( 146, 22 );
        cMenuTrackTabRendererVOCALOID1.setText( "VOCALOID1";
        cMenuTrackTabRendererVOCALOID1.Click += new System.EventHandler( commonRendererVOCALOID1_Click );
        // 
        // cMenuTrackTabRendererVOCALOID2
        // 
        cMenuTrackTabRendererVOCALOID2.setName( "cMenuTrackTabRendererVOCALOID2";
        cMenuTrackTabRendererVOCALOID2.Size = new System.Drawing.Size( 146, 22 );
        cMenuTrackTabRendererVOCALOID2.setText( "VOCALOID2";
        cMenuTrackTabRendererVOCALOID2.Click += new System.EventHandler( commonRendererVOCALOID2_Click );
        // 
        // cMenuTrackTabRendererUtau
        // 
        cMenuTrackTabRendererUtau.setName( "cMenuTrackTabRendererUtau";
        cMenuTrackTabRendererUtau.Size = new System.Drawing.Size( 146, 22 );
        cMenuTrackTabRendererUtau.setText( "UTAU";
        cMenuTrackTabRendererUtau.Click += new System.EventHandler( commonRendererUtau_Click );
        // 
        // cMenuTrackTabMasterTuning
        // 
        cMenuTrackTabMasterTuning.setName( "cMenuTrackTabMasterTuning";
        cMenuTrackTabMasterTuning.Size = new System.Drawing.Size( 219, 22 );
        cMenuTrackTabMasterTuning.setText( "Master Tuning(&M)";
        cMenuTrackTabMasterTuning.Click += new System.EventHandler( commonMasterTuning_Click );
        // 
        // cMenuTrackSelector
        // 
        cMenuTrackSelector.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
        cMenuTrackSelectorPointer,
        cMenuTrackSelectorPencil,
        cMenuTrackSelectorLine,
        cMenuTrackSelectorEraser,
        cMenuTrackSelectorPaletteTool,
        toolStripSeparator14,
        cMenuTrackSelectorCurve,
        toolStripMenuItem28,
        cMenuTrackSelectorUndo,
        cMenuTrackSelectorRedo,
        toolStripMenuItem29,
        cMenuTrackSelectorCut,
        cMenuTrackSelectorCopy,
        cMenuTrackSelectorPaste,
        cMenuTrackSelectorDelete,
        cMenuTrackSelectorDeleteBezier,
        toolStripMenuItem31,
        cMenuTrackSelectorSelectAll} );
        cMenuTrackSelector.setName( "cMenuTrackSelector";
        cMenuTrackSelector.Size = new System.Drawing.Size( 206, 336 );
        cMenuTrackSelector.Opening += new System.ComponentModel.CancelEventHandler( cMenuTrackSelector_Opening );
        // 
        // cMenuTrackSelectorPointer
        // 
        cMenuTrackSelectorPointer.setName( "cMenuTrackSelectorPointer";
        cMenuTrackSelectorPointer.Size = new System.Drawing.Size( 205, 22 );
        cMenuTrackSelectorPointer.setText( "Arrow(&A)";
        cMenuTrackSelectorPointer.Click += new System.EventHandler( cMenuTrackSelectorPointer_Click );
        // 
        // cMenuTrackSelectorPencil
        // 
        cMenuTrackSelectorPencil.setName( "cMenuTrackSelectorPencil";
        cMenuTrackSelectorPencil.Size = new System.Drawing.Size( 205, 22 );
        cMenuTrackSelectorPencil.setText( "Pencil(&W)";
        cMenuTrackSelectorPencil.Click += new System.EventHandler( cMenuTrackSelectorPencil_Click );
        // 
        // cMenuTrackSelectorLine
        // 
        cMenuTrackSelectorLine.setName( "cMenuTrackSelectorLine";
        cMenuTrackSelectorLine.Size = new System.Drawing.Size( 205, 22 );
        cMenuTrackSelectorLine.setText( "Line(&L)";
        cMenuTrackSelectorLine.Click += new System.EventHandler( cMenuTrackSelectorLine_Click );
        // 
        // cMenuTrackSelectorEraser
        // 
        cMenuTrackSelectorEraser.setName( "cMenuTrackSelectorEraser";
        cMenuTrackSelectorEraser.Size = new System.Drawing.Size( 205, 22 );
        cMenuTrackSelectorEraser.setText( "Eraser(&E)";
        cMenuTrackSelectorEraser.Click += new System.EventHandler( cMenuTrackSelectorEraser_Click );
        // 
        // cMenuTrackSelectorPaletteTool
        // 
        cMenuTrackSelectorPaletteTool.setName( "cMenuTrackSelectorPaletteTool";
        cMenuTrackSelectorPaletteTool.Size = new System.Drawing.Size( 205, 22 );
        cMenuTrackSelectorPaletteTool.setText( "Palette Tool";
        // 
        // toolStripSeparator14
        // 
        toolStripSeparator14.setName( "toolStripSeparator14";
        toolStripSeparator14.Size = new System.Drawing.Size( 202, 6 );
        // 
        // cMenuTrackSelectorCurve
        // 
        cMenuTrackSelectorCurve.setName( "cMenuTrackSelectorCurve";
        cMenuTrackSelectorCurve.Size = new System.Drawing.Size( 205, 22 );
        cMenuTrackSelectorCurve.setText( "Curve(&V)";
        cMenuTrackSelectorCurve.Click += new System.EventHandler( cMenuTrackSelectorCurve_Click );
        // 
        // toolStripMenuItem28
        // 
        toolStripMenuItem28.setName( "toolStripMenuItem28";
        toolStripMenuItem28.Size = new System.Drawing.Size( 202, 6 );
        // 
        // cMenuTrackSelectorUndo
        // 
        cMenuTrackSelectorUndo.setName( "cMenuTrackSelectorUndo";
        cMenuTrackSelectorUndo.Size = new System.Drawing.Size( 205, 22 );
        cMenuTrackSelectorUndo.setText( "Undo(&U)";
        cMenuTrackSelectorUndo.Click += new System.EventHandler( cMenuTrackSelectorUndo_Click );
        // 
        // cMenuTrackSelectorRedo
        // 
        cMenuTrackSelectorRedo.setName( "cMenuTrackSelectorRedo";
        cMenuTrackSelectorRedo.Size = new System.Drawing.Size( 205, 22 );
        cMenuTrackSelectorRedo.setText( "Redo(&R)";
        cMenuTrackSelectorRedo.Click += new System.EventHandler( cMenuTrackSelectorRedo_Click );
        // 
        // toolStripMenuItem29
        // 
        toolStripMenuItem29.setName( "toolStripMenuItem29";
        toolStripMenuItem29.Size = new System.Drawing.Size( 202, 6 );
        // 
        // cMenuTrackSelectorCut
        // 
        cMenuTrackSelectorCut.setName( "cMenuTrackSelectorCut";
        cMenuTrackSelectorCut.Size = new System.Drawing.Size( 205, 22 );
        cMenuTrackSelectorCut.setText( "Cut(&T)";
        cMenuTrackSelectorCut.Click += new System.EventHandler( cMenuTrackSelectorCut_Click );
        // 
        // cMenuTrackSelectorCopy
        // 
        cMenuTrackSelectorCopy.setName( "cMenuTrackSelectorCopy";
        cMenuTrackSelectorCopy.Size = new System.Drawing.Size( 205, 22 );
        cMenuTrackSelectorCopy.setText( "Copy(&C)";
        cMenuTrackSelectorCopy.Click += new System.EventHandler( cMenuTrackSelectorCopy_Click );
        // 
        // cMenuTrackSelectorPaste
        // 
        cMenuTrackSelectorPaste.setName( "cMenuTrackSelectorPaste";
        cMenuTrackSelectorPaste.Size = new System.Drawing.Size( 205, 22 );
        cMenuTrackSelectorPaste.setText( "Paste(&P)";
        cMenuTrackSelectorPaste.Click += new System.EventHandler( cMenuTrackSelectorPaste_Click );
        // 
        // cMenuTrackSelectorDelete
        // 
        cMenuTrackSelectorDelete.setName( "cMenuTrackSelectorDelete";
        cMenuTrackSelectorDelete.Size = new System.Drawing.Size( 205, 22 );
        cMenuTrackSelectorDelete.setText( "Delete(&D)";
        cMenuTrackSelectorDelete.Click += new System.EventHandler( cMenuTrackSelectorDelete_Click );
        // 
        // cMenuTrackSelectorDeleteBezier
        // 
        cMenuTrackSelectorDeleteBezier.setName( "cMenuTrackSelectorDeleteBezier";
        cMenuTrackSelectorDeleteBezier.Size = new System.Drawing.Size( 205, 22 );
        cMenuTrackSelectorDeleteBezier.setText( "Delete Bezier Point(&B)";
        cMenuTrackSelectorDeleteBezier.Click += new System.EventHandler( cMenuTrackSelectorDeleteBezier_Click );
        // 
        // toolStripMenuItem31
        // 
        toolStripMenuItem31.setName( "toolStripMenuItem31";
        toolStripMenuItem31.Size = new System.Drawing.Size( 202, 6 );
        // 
        // cMenuTrackSelectorSelectAll
        // 
        cMenuTrackSelectorSelectAll.setName( "cMenuTrackSelectorSelectAll";
        cMenuTrackSelectorSelectAll.Size = new System.Drawing.Size( 205, 22 );
        cMenuTrackSelectorSelectAll.setText( "Select All Events(&E)";
        cMenuTrackSelectorSelectAll.Click += new System.EventHandler( cMenuTrackSelectorSelectAll_Click );*/
        // 
        // trackBar
        // 
        //trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        //trackBar.AutoSize = false;
        //trackBar.Location = new System.Drawing.Point( 322, 266 );
        //trackBar.Margin = new System.Windows.Forms.Padding( 0 );
        trackBar.setMaximum( 609 );
        trackBar.setMinimum( 17 );
        trackBar.setName( "trackBar" );
        trackBar.setPreferredSize( new Dimension( 83, 16 ) );
        //trackBar.TabIndex = 15;
        //trackBar.TabStop = false;
        //trackBar.TickFrequency = 100;
        //trackBar.TickStyle = System.Windows.Forms.TickStyle.None;
        trackBar.setValue( 17 );
        //trackBar.ValueChanged += new System.EventHandler( trackBar_ValueChanged );
        //trackBar.MouseDown += new System.Windows.Forms.MouseEventHandler( trackBar_MouseDown );
        //trackBar.Enter += new System.EventHandler( trackBar_Enter );
        /*// 
        // bgWorkScreen
        // 
        bgWorkScreen.DoWork += new System.ComponentModel.DoWorkEventHandler( bgWorkScreen_DoWork );
        // 
        // timer
        // 
        timer.Interval = 200;
        timer.Tick += new System.EventHandler( timer_Tick );*/
        // 
        // panel1
        // 
        GridBagLayout gbl = new GridBagLayout();
        panel1.setLayout( gbl );
        GridBagConstraints gbc = new GridBagConstraints();
        // pitcutrePositionIndicatorをpanel1に追加
        gbc.fill = GridBagConstraints.HORIZONTAL;
        gbc.gridx = 0;
        gbc.gridy = 0;
        gbc.gridwidth = 4;
        gbc.gridheight = 1;
        gbc.weightx = 0.0d;
        gbc.weighty = 0.0d;
        gbl.setConstraints( picturePositionIndicator, gbc );
        // vScrollをpanel1に追加
        gbc.fill = GridBagConstraints.VERTICAL;
        gbc.gridx = 3;
        gbc.gridy = 1;
        gbc.gridwidth = 1;
        gbc.gridheight = 1;
        gbc.weightx = 0.0d;
        gbc.weighty = 1.0d;
        gbl.setConstraints( vScroll, gbc );
        // hScrollをpanel1に追加
        gbc.fill = GridBagConstraints.HORIZONTAL;
        gbc.gridx = 1;
        gbc.gridy = 2;
        gbc.gridwidth = 1;
        gbc.gridheight = 1;
        gbc.weightx = 1.0d;
        gbc.weighty = 0.0d;
        gbl.setConstraints( hScroll, gbc );
        // pictPianoRollをpanel1に追加
        gbc.fill = GridBagConstraints.BOTH;
        gbc.gridx = 0;
        gbc.gridy = 1;
        gbc.gridwidth = 3;
        gbc.gridheight = 1;
        gbc.weightx = 1.0d;
        gbc.weighty = 1.0d;
        gbl.setConstraints( pictPianoRoll, gbc );
        // pictureBox3をpanel1に追加
        gbc.gridx = 0;
        gbc.gridy = 2;
        gbc.gridwidth = 1;
        gbc.gridheight = 1;
        gbc.weightx = 0.0d;
        gbc.weighty = 0.0d;
        gbl.setConstraints( pictureBox3, gbc );
        // trackBarをpanel1に追加
        gbc.gridx = 2;
        gbc.gridy = 2;
        gbc.gridwidth = 1;
        gbc.gridheight = 1;
        gbc.weightx = 0.0d;
        gbc.weighty = 0.0d;
        gbl.setConstraints( trackBar, gbc );
        // pictureBox2をpanel1に追加
        gbc.gridx = 3;
        gbc.gridy = 2;
        gbc.gridwidth = 1;
        gbc.gridheight = 1;
        gbc.weightx = 0.0d;
        gbc.weighty = 0.0d;
        gbl.setConstraints( pictureBox2, gbc );
        panel1.add( picturePositionIndicator );
        panel1.add( vScroll );
        panel1.add( hScroll );
        panel1.add( pictPianoRoll );
        panel1.add( pictureBox3 );
        panel1.add( trackBar );
        panel1.add( pictureBox2 );
        panel1.setName( "panel1" );
        // 
        // vScroll
        // 
        //vScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        //            | System.Windows.Forms.AnchorStyles.Right)));
        //vScroll.LargeChange = 10;
        vScroll.setLocation( 405, 48 );
        ///vScroll.Margin = new System.Windows.Forms.Padding( 0 );
        vScroll.setMaximum( 100 );
        vScroll.setMinimum( 0 );
        vScroll.setName( "vScroll" );
        //vScroll.setSize( 16, 218 );
        //vScroll.SmallChange = 1;
        //vScroll.TabIndex = 17;
        vScroll.setValue( 0 );
        vScroll.setOrientation( JScrollBar.VERTICAL );
        //vScroll.ValueChanged += new System.EventHandler( vScroll_ValueChanged );
        //vScroll.Resize += new System.EventHandler( vScroll_Resize );
        //vScroll.Enter += new System.EventHandler( vScroll_Enter );
        // 
        // hScroll
        // 
        //hScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
        //            | System.Windows.Forms.AnchorStyles.Right)));
        //hScroll.LargeChange = 10;
        //hScroll.setLocation( 66, 266 );
        //hScroll.Margin = new System.Windows.Forms.Padding( 0 );
        hScroll.setMaximum( 100 );
        hScroll.setMinimum( 0 );
        hScroll.setName( "hScroll" );
        //hScroll.SmallChange = 1;
        //hScroll.TabIndex = 16;
        hScroll.setValue( 0 );
        hScroll.setOrientation( JScrollBar.HORIZONTAL );
        //hScroll.ValueChanged += new System.EventHandler( hScroll_ValueChanged );
        //hScroll.Resize += new System.EventHandler( hScroll_Resize );
        //hScroll.Enter += new System.EventHandler( hScroll_Enter );
        // 
        // picturePositionIndicator
        // 
        //picturePositionIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        //            | System.Windows.Forms.AnchorStyles.Right)));
        //picturePositionIndicator.BackColor = System.Drawing.Color.DarkGray;
        //picturePositionIndicator.Location = new System.Drawing.Point( 0, 0 );
        //picturePositionIndicator.Margin = new System.Windows.Forms.Padding( 0 );
        picturePositionIndicator.setName( "picturePositionIndicator" );
        picturePositionIndicator.setPreferredSize( new Dimension( 421, 48 ) );
        //picturePositionIndicator.TabIndex = 10;
        //picturePositionIndicator.TabStop = false;
        //picturePositionIndicator.MouseLeave += new System.EventHandler( picturePositionIndicator_MouseLeave );
        //picturePositionIndicator.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( picturePositionIndicator_PreviewKeyDown );
        //picturePositionIndicator.MouseMove += new System.Windows.Forms.MouseEventHandler( picturePositionIndicator_MouseMove );
        //picturePositionIndicator.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( picturePositionIndicator_MouseDoubleClick );
        //picturePositionIndicator.MouseClick += new System.Windows.Forms.MouseEventHandler( picturePositionIndicator_MouseClick );
        //picturePositionIndicator.MouseDown += new System.Windows.Forms.MouseEventHandler( picturePositionIndicator_MouseDown );
        //picturePositionIndicator.Paint += new System.Windows.Forms.PaintEventHandler( picturePositionIndicator_Paint );
        // 
        // pictPianoRoll
        // 
        //pictPianoRoll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        //            | System.Windows.Forms.AnchorStyles.Left)
        //            | System.Windows.Forms.AnchorStyles.Right)));
        //pictPianoRoll.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))) );
        //pictPianoRoll.Location = new System.Drawing.Point( 0, 48 );
        //pictPianoRoll.Margin = new System.Windows.Forms.Padding( 0 );
        //pictPianoRoll.MinimumSize = new System.Drawing.Size( 0, 100 );
        pictPianoRoll.setName( "pictPianoRoll" );
        //pictPianoRoll.Size = new System.Drawing.Size( 405, 218 );
        //pictPianoRoll.TabIndex = 12;
        //pictPianoRoll.TabStop = false;
        //pictPianoRoll.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( pictPianoRoll_PreviewKeyDown );
        //pictPianoRoll.BKeyUp += new System.Windows.Forms.KeyEventHandler( commonCaptureSpaceKeyUp );
        //pictPianoRoll.MouseMove += new System.Windows.Forms.MouseEventHandler( pictPianoRoll_MouseMove );
        //pictPianoRoll.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( pictPianoRoll_MouseDoubleClick );
        //pictPianoRoll.MouseClick += new System.Windows.Forms.MouseEventHandler( pictPianoRoll_MouseClick );
        //pictPianoRoll.MouseDown += new System.Windows.Forms.MouseEventHandler( pictPianoRoll_MouseDown );
        //pictPianoRoll.Paint += new System.Windows.Forms.PaintEventHandler( pictPianoRoll_Paint );
        //pictPianoRoll.MouseUp += new System.Windows.Forms.MouseEventHandler( pictPianoRoll_MouseUp );
        //pictPianoRoll.BKeyDown += new System.Windows.Forms.KeyEventHandler( commonCaptureSpaceKeyDown );
        // 
        // pictureBox3
        // 
        //pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        //pictureBox3.BackColor = System.Drawing.SystemColors.Control;
        //pictureBox3.Location = new System.Drawing.Point( 0, 266 );
        //pictureBox3.Margin = new System.Windows.Forms.Padding( 0 );
        pictureBox3.setName( "pictureBox3" );
        pictureBox3.setPreferredSize( new Dimension( 66, 16 ) );
        //pictureBox3.TabIndex = 8;
        //pictureBox3.TabStop = false;
        //pictureBox3.MouseDown += new System.Windows.Forms.MouseEventHandler( pictureBox3_MouseDown );
        // 
        // pictureBox2
        // 
        //pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        //pictureBox2.BackColor = System.Drawing.SystemColors.Control;
        //pictureBox2.Location = new System.Drawing.Point( 405, 266 );
        //pictureBox2.Margin = new System.Windows.Forms.Padding( 0 );
        pictureBox2.setName( "pictureBox2" );
        pictureBox2.setPreferredSize( new Dimension( 16, 16 ) );
        //pictureBox2.TabIndex = 5;
        //pictureBox2.TabStop = false;
        //pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler( pictureBox2_MouseDown );
        // 
        // toolStripTool
        // 
        //toolStripTool.Dock = System.Windows.Forms.DockStyle.None;
        toolStripTool.add( stripBtnPointer );
        toolStripTool.add( stripBtnPencil );
        toolStripTool.add( stripBtnLine );
        toolStripTool.add( stripBtnEraser );
        toolStripTool.addSeparator();
        toolStripTool.add( stripBtnGrid );
        toolStripTool.add( stripBtnCurve );
        //toolStripTool.Location = new System.Drawing.Point( 3, 50 );
        toolStripTool.setName( "toolStripTool" );
        //toolStripTool.Size = new System.Drawing.Size( 379, 25 );
        //toolStripTool.TabIndex = 17;
        //toolStripTool.setText( "toolStrip1" );
        toolStripTool.setFloatable( true );
        // 
        // stripBtnPointer
        // 
        //stripBtnPointer.Checked = true;
        //stripBtnPointer.CheckState = System.Windows.Forms.CheckState.Checked;
        //stripBtnPointer.Image = global::Boare.Cadencii.Properties.Resources.arrow_135;
        //stripBtnPointer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
        //stripBtnPointer.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnPointer.setName( "stripBtnPointer" );
        //stripBtnPointer.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
        //stripBtnPointer.Size = new System.Drawing.Size( 69, 22 );
        stripBtnPointer.setText( "Pointer" );
        //stripBtnPointer.Click += new System.EventHandler( stripBtnArrow_Click );
        // 
        // stripBtnPencil
        // 
        //stripBtnPencil.Image = global::Boare.Cadencii.Properties.Resources.pencil;
        //stripBtnPencil.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnPencil.setName( "stripBtnPencil" );
        //stripBtnPencil.Size = new System.Drawing.Size( 61, 22 );
        stripBtnPencil.setText( "Pencil" );
        //stripBtnPencil.Click += new System.EventHandler( stripBtnPencil_Click );
        // 
        // stripBtnLine
        // 
        //stripBtnLine.Image = global::Boare.Cadencii.Properties.Resources.layer_shape_line;
        //stripBtnLine.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnLine.setName( "stripBtnLine" );
        //stripBtnLine.Size = new System.Drawing.Size( 52, 22 );
        stripBtnLine.setText( "Line" );
        //stripBtnLine.Click += new System.EventHandler( stripBtnLine_Click );
        // 
        // stripBtnEraser
        // 
        //stripBtnEraser.Image = global::Boare.Cadencii.Properties.Resources.eraser;
        //stripBtnEraser.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnEraser.setName( "stripBtnEraser" );
        //stripBtnEraser.Size = new System.Drawing.Size( 65, 22 );
        stripBtnEraser.setText( "Eraser" );
        //stripBtnEraser.Click += new System.EventHandler( stripBtnEraser_Click );
        // 
        // stripBtnGrid
        // 
        //stripBtnGrid.CheckOnClick = true;
        //stripBtnGrid.Image = global::Boare.Cadencii.Properties.Resources.ruler_crop;
        //stripBtnGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnGrid.setName( "stripBtnGrid" );
        //stripBtnGrid.Size = new System.Drawing.Size( 52, 22 );
        stripBtnGrid.setText( "Grid" );
        //stripBtnGrid.CheckedChanged += new System.EventHandler( stripBtnGrid_CheckedChanged );
        // 
        // stripBtnCurve
        // 
        //stripBtnCurve.CheckOnClick = true;
        //stripBtnCurve.Image = global::Boare.Cadencii.Properties.Resources.layer_shape_curve;
        //stripBtnCurve.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnCurve.setName( "stripBtnCurve" );
        //stripBtnCurve.Size = new System.Drawing.Size( 62, 22 );
        stripBtnCurve.setText( "Curve" );
        //stripBtnCurve.CheckedChanged += new System.EventHandler( stripBtnCurve_CheckedChanged );
        /*// 
        // toolStripContainer
        // 
        // 
        // toolStripContainer.BottomToolStripPanel
        // 
        toolStripContainer.BottomToolStripPanel.Controls.Add( toolStripBottom );
        toolStripContainer.BottomToolStripPanel.Controls.Add( statusStrip1 );*/
        // 
        // toolStripContainer.ContentPanel
        // 
        //toolStripContainer.ContentPanel.AutoScroll = true;
        //toolStripContainer.ContentPanel.Controls.Add( panel2 );
        getContentPane().add( splitContainer1, BorderLayout.CENTER );
        splitContainer2.setTopComponent( new JButton( "button1" ) );
        splitContainer2.setBottomComponent( new JButton( "button2" ) );
        splitContainer1.setTopComponent( panel1 );
        splitContainer1.setBottomComponent( splitContainer2 );
        //toolStripContainer.ContentPanel.Controls.Add( splitContainer2 );
        //toolStripContainer.ContentPanel.Controls.Add( splitContainer1 );
        /*toolStripContainer.ContentPanel.Size = new System.Drawing.Size( 962, 562 );
        toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
        toolStripContainer.LeftToolStripPanelVisible = false;
        toolStripContainer.Location = new System.Drawing.Point( 0, 26 );
        toolStripContainer.setName( "toolStripContainer";
        toolStripContainer.RightToolStripPanelVisible = false;
        toolStripContainer.Size = new System.Drawing.Size( 962, 734 );
        toolStripContainer.TabIndex = 18;
        toolStripContainer.setText( "toolStripContainer1";*/
        // 
        // toolStripContainer.TopToolStripPanel
        // 
        JPanel p = new JPanel();
        p.setLayout( new FlowLayout( FlowLayout.LEFT ) );
        p.add( toolStripPosition );
        p.add( toolStripTool );
        getContentPane().add( p, BorderLayout.NORTH );
        //getContentPane().add( toolStripPosition, BorderLayout.NORTH );
        //toolStripContainer.TopToolStripPanel.Controls.Add( toolStripMeasure );
        //getContentPane().add( toolStripTool, BorderLayout.NORTH );
        //toolStripContainer.TopToolStripPanel.Controls.Add( toolStripFile );
        //toolStripContainer.TopToolStripPanel.Controls.Add( toolStripPaletteTools );
        /*toolStripContainer.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
        toolStripContainer.TopToolStripPanel.SizeChanged += new System.EventHandler( toolStripContainer_TopToolStripPanel_SizeChanged );
        // 
        // toolStripBottom
        // 
        toolStripBottom.Dock = System.Windows.Forms.DockStyle.None;
        toolStripBottom.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
        toolStripLabel6,
        stripLblCursor,
        toolStripSeparator8,
        toolStripLabel8,
        stripLblTempo,
        toolStripSeparator9,
        toolStripLabel10,
        stripLblBeat,
        toolStripSeparator4,
        toolStripStatusLabel1,
        stripLblGameCtrlMode,
        toolStripSeparator10,
        toolStripStatusLabel2,
        stripLblMidiIn,
        toolStripSeparator11,
        stripDDBtnSpeed} );
        toolStripBottom.Location = new System.Drawing.Point( 5, 0 );
        toolStripBottom.setName( "toolStripBottom";
        toolStripBottom.Size = new System.Drawing.Size( 768, 25 );
        toolStripBottom.TabIndex = 22;
        // 
        // toolStripLabel6
        // 
        toolStripLabel6.setName( "toolStripLabel6";
        toolStripLabel6.Size = new System.Drawing.Size( 58, 22 );
        toolStripLabel6.setText( "CURSOR";
        // 
        // stripLblCursor
        // 
        stripLblCursor.AutoSize = false;
        stripLblCursor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        stripLblCursor.Font = new System.Drawing.Font( "MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)) );
        stripLblCursor.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripLblCursor.setName( "stripLblCursor";
        stripLblCursor.Size = new System.Drawing.Size( 90, 22 );
        stripLblCursor.setText( "0 : 0 : 000";
        // 
        // toolStripSeparator8
        // 
        toolStripSeparator8.setName( "toolStripSeparator8";
        toolStripSeparator8.Size = new System.Drawing.Size( 6, 25 );
        // 
        // toolStripLabel8
        // 
        toolStripLabel8.setName( "toolStripLabel8";
        toolStripLabel8.Size = new System.Drawing.Size( 49, 22 );
        toolStripLabel8.setText( "TEMPO";
        // 
        // stripLblTempo
        // 
        stripLblTempo.AutoSize = false;
        stripLblTempo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        stripLblTempo.Font = new System.Drawing.Font( "MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)) );
        stripLblTempo.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripLblTempo.setName( "stripLblTempo";
        stripLblTempo.Size = new System.Drawing.Size( 60, 22 );
        stripLblTempo.setText( "120.00";
        // 
        // toolStripSeparator9
        // 
        toolStripSeparator9.setName( "toolStripSeparator9";
        toolStripSeparator9.Size = new System.Drawing.Size( 6, 25 );
        // 
        // toolStripLabel10
        // 
        toolStripLabel10.setName( "toolStripLabel10";
        toolStripLabel10.Size = new System.Drawing.Size( 38, 22 );
        toolStripLabel10.setText( "BEAT";
        // 
        // stripLblBeat
        // 
        stripLblBeat.AutoSize = false;
        stripLblBeat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        stripLblBeat.Font = new System.Drawing.Font( "MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)) );
        stripLblBeat.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripLblBeat.setName( "stripLblBeat";
        stripLblBeat.Size = new System.Drawing.Size( 45, 22 );
        stripLblBeat.setText( "4/4";
        // 
        // toolStripSeparator4
        // 
        toolStripSeparator4.setName( "toolStripSeparator4";
        toolStripSeparator4.Size = new System.Drawing.Size( 6, 25 );
        // 
        // toolStripStatusLabel1
        // 
        toolStripStatusLabel1.setName( "toolStripStatusLabel1";
        toolStripStatusLabel1.Size = new System.Drawing.Size( 101, 20 );
        toolStripStatusLabel1.setText( "Game Controler";
        // 
        // stripLblGameCtrlMode
        // 
        stripLblGameCtrlMode.Image = global::Boare.Cadencii.Properties.Resources.slash;
        stripLblGameCtrlMode.setName( "stripLblGameCtrlMode";
        stripLblGameCtrlMode.Size = new System.Drawing.Size( 73, 20 );
        stripLblGameCtrlMode.setText( "Disabled";
        stripLblGameCtrlMode.ToolTipText = "Game Controler";
        // 
        // toolStripSeparator10
        // 
        toolStripSeparator10.setName( "toolStripSeparator10";
        toolStripSeparator10.Size = new System.Drawing.Size( 6, 25 );
        // 
        // toolStripStatusLabel2
        // 
        toolStripStatusLabel2.setName( "toolStripStatusLabel2";
        toolStripStatusLabel2.Size = new System.Drawing.Size( 53, 20 );
        toolStripStatusLabel2.setText( "MIDI In";
        // 
        // stripLblMidiIn
        // 
        stripLblMidiIn.Image = global::Boare.Cadencii.Properties.Resources.slash;
        stripLblMidiIn.setName( "stripLblMidiIn";
        stripLblMidiIn.Size = new System.Drawing.Size( 73, 20 );
        stripLblMidiIn.setText( "Disabled";
        stripLblMidiIn.ToolTipText = "Midi In Device";
        // 
        // toolStripSeparator11
        // 
        toolStripSeparator11.setName( "toolStripSeparator11";
        toolStripSeparator11.Size = new System.Drawing.Size( 6, 25 );
        // 
        // stripDDBtnSpeed
        // 
        stripDDBtnSpeed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        stripDDBtnSpeed.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
        stripDDBtnSpeedTextbox,
        stripDDBtnSpeed033,
        stripDDBtnSpeed050,
        stripDDBtnSpeed100} );
        stripDDBtnSpeed.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripDDBtnSpeed.setName( "stripDDBtnSpeed";
        stripDDBtnSpeed.Size = new System.Drawing.Size( 86, 22 );
        stripDDBtnSpeed.setText( "Speed 1.0x";
        stripDDBtnSpeed.DropDownOpening += new System.EventHandler( stripDDBtnSpeed_DropDownOpening );
        // 
        // stripDDBtnSpeedTextbox
        // 
        stripDDBtnSpeedTextbox.setName( "stripDDBtnSpeedTextbox";
        stripDDBtnSpeedTextbox.Size = new System.Drawing.Size( 100, 25 );
        stripDDBtnSpeedTextbox.setText( "100";
        stripDDBtnSpeedTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler( stripDDBtnSpeedTextbox_KeyDown );
        // 
        // stripDDBtnSpeed033
        // 
        stripDDBtnSpeed033.setName( "stripDDBtnSpeed033";
        stripDDBtnSpeed033.Size = new System.Drawing.Size( 160, 22 );
        stripDDBtnSpeed033.setText( "33.3%";
        stripDDBtnSpeed033.Click += new System.EventHandler( stripDDBtnSpeed033_Click );
        // 
        // stripDDBtnSpeed050
        // 
        stripDDBtnSpeed050.setName( "stripDDBtnSpeed050";
        stripDDBtnSpeed050.Size = new System.Drawing.Size( 160, 22 );
        stripDDBtnSpeed050.setText( "50%";
        stripDDBtnSpeed050.Click += new System.EventHandler( stripDDBtnSpeed050_Click );
        // 
        // stripDDBtnSpeed100
        // 
        stripDDBtnSpeed100.setName( "stripDDBtnSpeed100";
        stripDDBtnSpeed100.Size = new System.Drawing.Size( 160, 22 );
        stripDDBtnSpeed100.setText( "100%";
        stripDDBtnSpeed100.Click += new System.EventHandler( stripDDBtnSpeed100_Click );
        // 
        // statusStrip1
        // 
        statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
        statusStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
        statusLabel} );
        statusStrip1.Location = new System.Drawing.Point( 0, 25 );
        statusStrip1.setName( "statusStrip1";
        statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
        statusStrip1.Size = new System.Drawing.Size( 962, 22 );
        statusStrip1.TabIndex = 17;
        statusStrip1.setText( "statusStrip1";*/
        // 
        // statusLabel
        // 
        statusLabel.setName( "statusLabel" );
        statusLabel.setText( " " );
        Panel panelForStatusLabel = new Panel();
        panelForStatusLabel.setLayout( new BoxLayout( panelForStatusLabel, BoxLayout.X_AXIS ) );
        panelForStatusLabel.add( statusLabel );
        getContentPane().add( panelForStatusLabel, BorderLayout.SOUTH );
        /* // 
        // panel2
        // 
        panel2.BackColor = System.Drawing.Color.DarkGray;
        panel2.Controls.Add( waveView );
        panel2.Location = new System.Drawing.Point( 3, 291 );
        panel2.setName( "panel2";
        panel2.Size = new System.Drawing.Size( 421, 59 );
        panel2.TabIndex = 19;
        // 
        // waveView
        // 
        waveView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        waveView.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))) );
        waveView.Location = new System.Drawing.Point( 66, 0 );
        waveView.Margin = new System.Windows.Forms.Padding( 0 );
        waveView.setName( "waveView";
        waveView.Size = new System.Drawing.Size( 355, 59 );
        waveView.TabIndex = 17;
        // 
        // splitContainer2
        // 
        splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
        splitContainer2.IsSplitterFixed = false;
        splitContainer2.Location = new System.Drawing.Point( 606, 17 );
        splitContainer2.setName( "splitContainer2";
        splitContainer2.Orientation = System.Windows.Forms.Orientation.Vertical;
        // 
        // 
        // 
        splitContainer2.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        splitContainer2.Panel1.BorderColor = System.Drawing.Color.Black;
        splitContainer2.Panel1.Location = new System.Drawing.Point( 0, 0 );
        splitContainer2.Panel1.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 4 );
        splitContainer2.Panel1.setName( "m_panel1";
        splitContainer2.Panel1.Size = new System.Drawing.Size( 115, 53 );
        splitContainer2.Panel1.TabIndex = 0;
        splitContainer2.Panel1MinSize = 25;
        // 
        // 
        // 
        splitContainer2.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        splitContainer2.Panel2.BorderColor = System.Drawing.Color.Black;
        splitContainer2.Panel2.Location = new System.Drawing.Point( 0, 57 );
        splitContainer2.Panel2.Margin = new System.Windows.Forms.Padding( 0 );
        splitContainer2.Panel2.setName( "m_panel2";
        splitContainer2.Panel2.Size = new System.Drawing.Size( 115, 185 );
        splitContainer2.Panel2.TabIndex = 1;
        splitContainer2.Panel2MinSize = 25;
        splitContainer2.Size = new System.Drawing.Size( 115, 242 );
        splitContainer2.SplitterDistance = 53;
        splitContainer2.SplitterWidth = 4;
        splitContainer2.TabIndex = 18;
        splitContainer2.setText( "bSplitContainer1";
        // 
        // splitContainer1
        // 
        splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
        splitContainer1.IsSplitterFixed = false;
        splitContainer1.Location = new System.Drawing.Point( 440, 17 );
        splitContainer1.MinimumSize = new System.Drawing.Size( 0, 54 );
        splitContainer1.setName( "splitContainer1";
        splitContainer1.Orientation = System.Windows.Forms.Orientation.Vertical;
        // 
        // 
        // 
        splitContainer1.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        splitContainer1.Panel1.BorderColor = System.Drawing.Color.Black;
        splitContainer1.Panel1.Location = new System.Drawing.Point( 0, 0 );
        splitContainer1.Panel1.Margin = new System.Windows.Forms.Padding( 0, 0, 0, 4 );
        splitContainer1.Panel1.setName( "m_panel1";
        splitContainer1.Panel1.Size = new System.Drawing.Size( 138, 27 );
        splitContainer1.Panel1.TabIndex = 0;
        splitContainer1.Panel1MinSize = 25;
        // 
        // 
        // 
        splitContainer1.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        splitContainer1.Panel2.BorderColor = System.Drawing.Color.Black;
        splitContainer1.Panel2.Location = new System.Drawing.Point( 0, 31 );
        splitContainer1.Panel2.Margin = new System.Windows.Forms.Padding( 0 );
        splitContainer1.Panel2.setName( "m_panel2";
        splitContainer1.Panel2.Size = new System.Drawing.Size( 138, 211 );
        splitContainer1.Panel2.TabIndex = 1;
        splitContainer1.Panel2MinSize = 25;
        splitContainer1.Size = new System.Drawing.Size( 138, 242 );
        splitContainer1.SplitterDistance = 27;
        splitContainer1.SplitterWidth = 4;
        splitContainer1.TabIndex = 4;
        splitContainer1.setText( "splitContainerEx1";*/
        // 
        // toolStripPosition
        // 
        toolStripPosition.add( stripBtnMoveTop );
        toolStripPosition.add( stripBtnRewind );
        toolStripPosition.add( stripBtnForward );
        toolStripPosition.add( stripBtnMoveEnd );
        toolStripPosition.add( stripBtnPlay );
        toolStripPosition.add( stripBtnStop );
        toolStripPosition.addSeparator();
        toolStripPosition.add( stripBtnScroll );
        toolStripPosition.add( stripBtnLoop );
        //toolStripPosition.Location = new System.Drawing.Point( 3, 0 );
        toolStripPosition.setName( "toolStripPosition" );
        //toolStripPosition.Size = new System.Drawing.Size( 202, 25 );
        //toolStripPosition.TabIndex = 18;
        // 
        // stripBtnMoveTop
        // 
        //stripBtnMoveTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        //stripBtnMoveTop.Image = global::Boare.Cadencii.Properties.Resources.control_stop_180;
        //stripBtnMoveTop.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnMoveTop.setName( "stripBtnMoveTop" );
        //stripBtnMoveTop.Size = new System.Drawing.Size( 23, 22 );
        stripBtnMoveTop.setText( "  <=|  " );
        //stripBtnMoveTop.ToolTipText = "MoveTop";
        //stripBtnMoveTop.Click += new System.EventHandler( stripBtnMoveTop_Click );
        // 
        // stripBtnRewind
        // 
        //stripBtnRewind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        //stripBtnRewind.Image = global::Boare.Cadencii.Properties.Resources.control_double_180;
        //stripBtnRewind.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnRewind.setName( "stripBtnRewind" );
        //stripBtnRewind.Size = new System.Drawing.Size( 23, 22 );
        stripBtnRewind.setText( "  <<  " );
        //stripBtnRewind.ToolTipText = "Rewind";
        //stripBtnRewind.Click += new System.EventHandler( stripBtnRewind_Click );
        // 
        // stripBtnForward
        // 
        //stripBtnForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        //stripBtnForward.Image = global::Boare.Cadencii.Properties.Resources.control_double;
        //stripBtnForward.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnForward.setName( "stripBtnForward" );
        //stripBtnForward.Size = new System.Drawing.Size( 23, 22 );
        stripBtnForward.setText( "  >>  " );
        //stripBtnForward.ToolTipText = "Forward";
        //stripBtnForward.Click += new System.EventHandler( stripBtnForward_Click );
        // 
        // stripBtnMoveEnd
        // 
        //stripBtnMoveEnd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        //stripBtnMoveEnd.Image = global::Boare.Cadencii.Properties.Resources.control_stop;
        //stripBtnMoveEnd.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnMoveEnd.setName( "stripBtnMoveEnd" );
        //stripBtnMoveEnd.Size = new System.Drawing.Size( 23, 22 );
        stripBtnMoveEnd.setText( "  |=>  " );
        //stripBtnMoveEnd.ToolTipText = "MoveEnd";
        //stripBtnMoveEnd.Click += new System.EventHandler( stripBtnMoveEnd_Click );
        // 
        // stripBtnPlay
        // 
        //stripBtnPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        //stripBtnPlay.Image = global::Boare.Cadencii.Properties.Resources.control;
        //stripBtnPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnPlay.setName( "stripBtnPlay" );
        //stripBtnPlay.Size = new System.Drawing.Size( 23, 22 );
        stripBtnPlay.setText( "  =>  " );
        //stripBtnPlay.ToolTipText = "Play";
        //stripBtnPlay.Click += new System.EventHandler( stripBtnPlay_Click );
        // 
        // stripBtnStop
        // 
        //stripBtnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        //stripBtnStop.Image = global::Boare.Cadencii.Properties.Resources.control_pause;
        //stripBtnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnStop.setName( "stripBtnStop" );
        //stripBtnStop.Size = new System.Drawing.Size( 23, 22 );
        stripBtnStop.setText( "   ||   " );
        //stripBtnStop.ToolTipText = "Stop";
        //stripBtnStop.Click += new System.EventHandler( stripBtnStop_Click );
        // 
        // stripBtnScroll
        // 
        //stripBtnScroll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        //stripBtnScroll.Image = global::Boare.Cadencii.Properties.Resources.arrow_circle_double;
        //stripBtnScroll.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnScroll.setName( "stripBtnScroll" );
        //stripBtnScroll.Size = new System.Drawing.Size( 23, 22 );
        stripBtnScroll.setText( "Scroll" );
        //stripBtnScroll.Click += new System.EventHandler( stripBtnScroll_Click );
        // 
        // stripBtnLoop
        // 
        //stripBtnLoop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        //stripBtnLoop.Image = global::Boare.Cadencii.Properties.Resources.arrow_return;
        //stripBtnLoop.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnLoop.setName( "stripBtnLoop" );
        //stripBtnLoop.Size = new System.Drawing.Size( 23, 22 );
        stripBtnLoop.setText( "Loop" );
        //stripBtnLoop.Click += new System.EventHandler( stripBtnLoop_Click );
        /* // 
        // toolStripMeasure
        // 
        toolStripMeasure.Dock = System.Windows.Forms.DockStyle.None;
        toolStripMeasure.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
        toolStripLabel5,
        stripLblMeasure,
        toolStripButton1,
        stripDDBtnLength,
        stripDDBtnQuantize,
        toolStripSeparator6,
        stripBtnStartMarker,
        stripBtnEndMarker} );
        toolStripMeasure.Location = new System.Drawing.Point( 3, 25 );
        toolStripMeasure.setName( "toolStripMeasure";
        toolStripMeasure.Size = new System.Drawing.Size( 430, 25 );
        toolStripMeasure.TabIndex = 19;
        // 
        // toolStripLabel5
        // 
        toolStripLabel5.setName( "toolStripLabel5";
        toolStripLabel5.Size = new System.Drawing.Size( 65, 22 );
        toolStripLabel5.setText( "MEASURE";
        // 
        // stripLblMeasure
        // 
        stripLblMeasure.AutoSize = false;
        stripLblMeasure.Font = new System.Drawing.Font( "MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)) );
        stripLblMeasure.setName( "stripLblMeasure";
        stripLblMeasure.Size = new System.Drawing.Size( 90, 22 );
        stripLblMeasure.setText( "0 : 0 : 000";
        stripLblMeasure.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // toolStripButton1
        // 
        toolStripButton1.setName( "toolStripButton1";
        toolStripButton1.Size = new System.Drawing.Size( 6, 25 );
        // 
        // stripDDBtnLength
        // 
        stripDDBtnLength.AutoSize = false;
        stripDDBtnLength.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        stripDDBtnLength.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
        stripDDBtnLength04,
        stripDDBtnLength08,
        stripDDBtnLength16,
        stripDDBtnLength32,
        stripDDBtnLength64,
        stripDDBtnLength128,
        stripDDBtnLengthOff,
        toolStripSeparator2,
        stripDDBtnLengthTriplet} );
        stripDDBtnLength.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripDDBtnLength.setName( "stripDDBtnLength";
        stripDDBtnLength.Size = new System.Drawing.Size( 95, 22 );
        stripDDBtnLength.setText( "LENGTH  1/64";
        stripDDBtnLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // stripDDBtnLength04
        // 
        stripDDBtnLength04.setName( "stripDDBtnLength04";
        stripDDBtnLength04.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnLength04.setText( "1/4";
        stripDDBtnLength04.Click += new System.EventHandler( h_lengthQuantize04 );
        // 
        // stripDDBtnLength08
        // 
        stripDDBtnLength08.setName( "stripDDBtnLength08";
        stripDDBtnLength08.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnLength08.setText( "1/8";
        stripDDBtnLength08.Click += new System.EventHandler( h_lengthQuantize08 );
        // 
        // stripDDBtnLength16
        // 
        stripDDBtnLength16.setName( "stripDDBtnLength16";
        stripDDBtnLength16.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnLength16.setText( "1/16";
        stripDDBtnLength16.Click += new System.EventHandler( h_lengthQuantize16 );
        // 
        // stripDDBtnLength32
        // 
        stripDDBtnLength32.setName( "stripDDBtnLength32";
        stripDDBtnLength32.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnLength32.setText( "1/32";
        stripDDBtnLength32.Click += new System.EventHandler( h_lengthQuantize32 );
        // 
        // stripDDBtnLength64
        // 
        stripDDBtnLength64.setName( "stripDDBtnLength64";
        stripDDBtnLength64.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnLength64.setText( "1/64";
        stripDDBtnLength64.Click += new System.EventHandler( h_lengthQuantize64 );
        // 
        // stripDDBtnLength128
        // 
        stripDDBtnLength128.setName( "stripDDBtnLength128";
        stripDDBtnLength128.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnLength128.setText( "1/128";
        stripDDBtnLength128.Click += new System.EventHandler( h_lengthQuantize128 );
        // 
        // stripDDBtnLengthOff
        // 
        stripDDBtnLengthOff.setName( "stripDDBtnLengthOff";
        stripDDBtnLengthOff.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnLengthOff.setText( "Off";
        stripDDBtnLengthOff.Click += new System.EventHandler( h_lengthQuantizeOff );
        // 
        // toolStripSeparator2
        // 
        toolStripSeparator2.setName( "toolStripSeparator2";
        toolStripSeparator2.Size = new System.Drawing.Size( 110, 6 );
        // 
        // stripDDBtnLengthTriplet
        // 
        stripDDBtnLengthTriplet.setName( "stripDDBtnLengthTriplet";
        stripDDBtnLengthTriplet.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnLengthTriplet.setText( "Triplet";
        stripDDBtnLengthTriplet.Click += new System.EventHandler( h_lengthQuantizeTriplet );
        // 
        // stripDDBtnQuantize
        // 
        stripDDBtnQuantize.AutoSize = false;
        stripDDBtnQuantize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        stripDDBtnQuantize.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
        stripDDBtnQuantize04,
        stripDDBtnQuantize08,
        stripDDBtnQuantize16,
        stripDDBtnQuantize32,
        stripDDBtnQuantize64,
        stripDDBtnQuantize128,
        stripDDBtnQuantizeOff,
        toolStripSeparator3,
        stripDDBtnQuantizeTriplet} );
        stripDDBtnQuantize.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripDDBtnQuantize.setName( "stripDDBtnQuantize";
        stripDDBtnQuantize.Size = new System.Drawing.Size( 110, 22 );
        stripDDBtnQuantize.setText( "QUANTIZE  1/64";
        stripDDBtnQuantize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // stripDDBtnQuantize04
        // 
        stripDDBtnQuantize04.setName( "stripDDBtnQuantize04";
        stripDDBtnQuantize04.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnQuantize04.setText( "1/4";
        stripDDBtnQuantize04.Click += new System.EventHandler( h_positionQuantize04 );
        // 
        // stripDDBtnQuantize08
        // 
        stripDDBtnQuantize08.setName( "stripDDBtnQuantize08";
        stripDDBtnQuantize08.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnQuantize08.setText( "1/8";
        stripDDBtnQuantize08.Click += new System.EventHandler( h_positionQuantize08 );
        // 
        // stripDDBtnQuantize16
        // 
        stripDDBtnQuantize16.setName( "stripDDBtnQuantize16";
        stripDDBtnQuantize16.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnQuantize16.setText( "1/16";
        stripDDBtnQuantize16.Click += new System.EventHandler( h_positionQuantize16 );
        // 
        // stripDDBtnQuantize32
        // 
        stripDDBtnQuantize32.setName( "stripDDBtnQuantize32";
        stripDDBtnQuantize32.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnQuantize32.setText( "1/32";
        stripDDBtnQuantize32.Click += new System.EventHandler( h_positionQuantize32 );
        // 
        // stripDDBtnQuantize64
        // 
        stripDDBtnQuantize64.setName( "stripDDBtnQuantize64";
        stripDDBtnQuantize64.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnQuantize64.setText( "1/64";
        stripDDBtnQuantize64.Click += new System.EventHandler( h_positionQuantize64 );
        // 
        // stripDDBtnQuantize128
        // 
        stripDDBtnQuantize128.setName( "stripDDBtnQuantize128";
        stripDDBtnQuantize128.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnQuantize128.setText( "1/128";
        stripDDBtnQuantize128.Click += new System.EventHandler( h_positionQuantize128 );
        // 
        // stripDDBtnQuantizeOff
        // 
        stripDDBtnQuantizeOff.setName( "stripDDBtnQuantizeOff";
        stripDDBtnQuantizeOff.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnQuantizeOff.setText( "Off";
        stripDDBtnQuantizeOff.Click += new System.EventHandler( h_positionQuantizeOff );
        // 
        // toolStripSeparator3
        // 
        toolStripSeparator3.setName( "toolStripSeparator3";
        toolStripSeparator3.Size = new System.Drawing.Size( 110, 6 );
        // 
        // stripDDBtnQuantizeTriplet
        // 
        stripDDBtnQuantizeTriplet.setName( "stripDDBtnQuantizeTriplet";
        stripDDBtnQuantizeTriplet.Size = new System.Drawing.Size( 113, 22 );
        stripDDBtnQuantizeTriplet.setText( "Triplet";
        stripDDBtnQuantizeTriplet.Click += new System.EventHandler( h_positionQuantizeTriplet );
        // 
        // toolStripSeparator6
        // 
        toolStripSeparator6.setName( "toolStripSeparator6";
        toolStripSeparator6.Size = new System.Drawing.Size( 6, 25 );
        // 
        // stripBtnStartMarker
        // 
        stripBtnStartMarker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        stripBtnStartMarker.Image = global::Boare.Cadencii.Properties.Resources.pin__arrow;
        stripBtnStartMarker.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnStartMarker.setName( "stripBtnStartMarker";
        stripBtnStartMarker.Size = new System.Drawing.Size( 23, 22 );
        stripBtnStartMarker.setText( "StartMarker";
        stripBtnStartMarker.Click += new System.EventHandler( stripBtnStartMarker_Click );
        // 
        // stripBtnEndMarker
        // 
        stripBtnEndMarker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        stripBtnEndMarker.Image = global::Boare.Cadencii.Properties.Resources.pin__arrow_inv;
        stripBtnEndMarker.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnEndMarker.setName( "stripBtnEndMarker";
        stripBtnEndMarker.Size = new System.Drawing.Size( 23, 22 );
        stripBtnEndMarker.setText( "EndMarker";
        stripBtnEndMarker.Click += new System.EventHandler( stripBtnEndMarker_Click );
        // 
        // toolStripFile
        // 
        toolStripFile.Dock = System.Windows.Forms.DockStyle.None;
        toolStripFile.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
        stripBtnFileNew,
        stripBtnFileOpen,
        stripBtnFileSave,
        toolStripSeparator12,
        stripBtnCut,
        stripBtnCopy,
        stripBtnPaste,
        toolStripSeparator13,
        stripBtnUndo,
        stripBtnRedo} );
        toolStripFile.Location = new System.Drawing.Point( 3, 75 );
        toolStripFile.setName( "toolStripFile";
        toolStripFile.Size = new System.Drawing.Size( 208, 25 );
        toolStripFile.TabIndex = 20;
        // 
        // stripBtnFileNew
        // 
        stripBtnFileNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        stripBtnFileNew.Image = global::Boare.Cadencii.Properties.Resources.disk__plus;
        stripBtnFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnFileNew.setName( "stripBtnFileNew";
        stripBtnFileNew.Size = new System.Drawing.Size( 23, 22 );
        stripBtnFileNew.setText( "toolStripButton6";
        stripBtnFileNew.ToolTipText = "New";
        stripBtnFileNew.Click += new System.EventHandler( commonFileNew_Click );
        // 
        // stripBtnFileOpen
        // 
        stripBtnFileOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        stripBtnFileOpen.Image = global::Boare.Cadencii.Properties.Resources.folder_horizontal_open;
        stripBtnFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnFileOpen.setName( "stripBtnFileOpen";
        stripBtnFileOpen.Size = new System.Drawing.Size( 23, 22 );
        stripBtnFileOpen.setText( "toolStripButton3";
        stripBtnFileOpen.ToolTipText = "Open";
        stripBtnFileOpen.Click += new System.EventHandler( commonFileOpen_Click );
        // 
        // stripBtnFileSave
        // 
        stripBtnFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        stripBtnFileSave.Image = global::Boare.Cadencii.Properties.Resources.disk;
        stripBtnFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnFileSave.setName( "stripBtnFileSave";
        stripBtnFileSave.Size = new System.Drawing.Size( 23, 22 );
        stripBtnFileSave.setText( "toolStripButton2";
        stripBtnFileSave.ToolTipText = "Save";
        stripBtnFileSave.Click += new System.EventHandler( commonFileSave_Click );
        // 
        // toolStripSeparator12
        // 
        toolStripSeparator12.setName( "toolStripSeparator12";
        toolStripSeparator12.Size = new System.Drawing.Size( 6, 25 );
        // 
        // stripBtnCut
        // 
        stripBtnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        stripBtnCut.Image = global::Boare.Cadencii.Properties.Resources.scissors;
        stripBtnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnCut.setName( "stripBtnCut";
        stripBtnCut.Size = new System.Drawing.Size( 23, 22 );
        stripBtnCut.setText( "toolStripButton4";
        stripBtnCut.ToolTipText = "Cut";
        stripBtnCut.Click += new System.EventHandler( commonEditCut_Click );
        // 
        // stripBtnCopy
        // 
        stripBtnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        stripBtnCopy.Image = global::Boare.Cadencii.Properties.Resources.documents;
        stripBtnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnCopy.setName( "stripBtnCopy";
        stripBtnCopy.Size = new System.Drawing.Size( 23, 22 );
        stripBtnCopy.setText( "toolStripButton5";
        stripBtnCopy.ToolTipText = "Copy";
        stripBtnCopy.Click += new System.EventHandler( commonEditCopy_Click );
        // 
        // stripBtnPaste
        // 
        stripBtnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        stripBtnPaste.Image = global::Boare.Cadencii.Properties.Resources.clipboard_paste;
        stripBtnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnPaste.setName( "stripBtnPaste";
        stripBtnPaste.Size = new System.Drawing.Size( 23, 22 );
        stripBtnPaste.setText( "toolStripLabel1";
        stripBtnPaste.ToolTipText = "Paste";
        stripBtnPaste.Click += new System.EventHandler( commonEditPaste_Click );
        // 
        // toolStripSeparator13
        // 
        toolStripSeparator13.setName( "toolStripSeparator13";
        toolStripSeparator13.Size = new System.Drawing.Size( 6, 25 );
        // 
        // stripBtnUndo
        // 
        stripBtnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        stripBtnUndo.Image = global::Boare.Cadencii.Properties.Resources.arrow_skip_180;
        stripBtnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnUndo.setName( "stripBtnUndo";
        stripBtnUndo.Size = new System.Drawing.Size( 23, 22 );
        stripBtnUndo.setText( "toolStripButton7";
        stripBtnUndo.ToolTipText = "Undo";
        stripBtnUndo.Click += new System.EventHandler( commonEditUndo_Click );
        // 
        // stripBtnRedo
        // 
        stripBtnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        stripBtnRedo.Image = global::Boare.Cadencii.Properties.Resources.arrow_skip;
        stripBtnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
        stripBtnRedo.setName( "stripBtnRedo";
        stripBtnRedo.Size = new System.Drawing.Size( 23, 22 );
        stripBtnRedo.setText( "toolStripButton8";
        stripBtnRedo.ToolTipText = "Redo";
        stripBtnRedo.Click += new System.EventHandler( commonEditRedo_Click );
        // 
        // toolStripPaletteTools
        // 
        toolStripPaletteTools.Dock = System.Windows.Forms.DockStyle.None;
        toolStripPaletteTools.Location = new System.Drawing.Point( 3, 100 );
        toolStripPaletteTools.setName( "toolStripPaletteTools";
        toolStripPaletteTools.Size = new System.Drawing.Size( 111, 25 );
        toolStripPaletteTools.TabIndex = 21;
        // 
        // openUstDialog
        // 
        openUstDialog.Filter = "UTAU Project File(*.ust)|*.ust|All Files(*.*)|*.*";
        // 
        // FormMain
        // 
        AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.SystemColors.Control;
        ClientSize = new System.Drawing.Size( 962, 760 );
        Controls.Add( toolStripContainer );*/
        setJMenuBar( menuStripMain );
        /*Icon = ((System.Drawing.Icon)(resources.GetObject( "$Icon" )));
        KeyPreview = true;
        MainMenuStrip = menuStripMain;
        setName( "FormMain";
        setText( "Cadencii";
        Deactivate += new System.EventHandler( FormMain_Deactivate );
        Load += new System.EventHandler( FormMain_Load );
        Activated += new System.EventHandler( FormMain_Activated );
        FormClosed += new System.Windows.Forms.FormClosedEventHandler( FormMain_FormClosed );
        FormClosing += new System.Windows.Forms.FormClosingEventHandler( FormMain_FormClosing );
        PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( FormMain_PreviewKeyDown );
        menuStripMain.ResumeLayout( false );
        menuStripMain.PerformLayout();
        cMenuPiano.ResumeLayout( false );
        cMenuTrackTab.ResumeLayout( false );
        cMenuTrackSelector.ResumeLayout( false );
        ((System.ComponentModel.ISupportInitialize)(trackBar)).EndInit();
        panel1.ResumeLayout( false );
        ((System.ComponentModel.ISupportInitialize)(picturePositionIndicator)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(pictPianoRoll)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(pictureBox3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(pictureBox2)).EndInit();
        toolStripTool.ResumeLayout( false );
        toolStripTool.PerformLayout();
        toolStripContainer.BottomToolStripPanel.ResumeLayout( false );
        toolStripContainer.BottomToolStripPanel.PerformLayout();
        toolStripContainer.ContentPanel.ResumeLayout( false );
        toolStripContainer.TopToolStripPanel.ResumeLayout( false );
        toolStripContainer.TopToolStripPanel.PerformLayout();
        toolStripContainer.ResumeLayout( false );
        toolStripContainer.PerformLayout();
        toolStripBottom.ResumeLayout( false );
        toolStripBottom.PerformLayout();
        statusStrip1.ResumeLayout( false );
        statusStrip1.PerformLayout();
        panel2.ResumeLayout( false );
        toolStripPosition.ResumeLayout( false );
        toolStripPosition.PerformLayout();
        toolStripMeasure.ResumeLayout( false );
        toolStripMeasure.PerformLayout();
        toolStripFile.ResumeLayout( false );
        toolStripFile.PerformLayout();
        ResumeLayout( false );
        PerformLayout();*/

        try{
            //UIManager.setLookAndFeel( "com.sun.java.swing.plaf.mac.MacLookAndFeel" );
            //UIManager.setLookAndFeel( "com.sun.java.swing.plaf.windows.WindowsLookAndFeel" );
    		//SwingUtilities.updateComponentTreeUI( this );
        }catch( Exception ex ){
        }
    }
    
    public void commonFileOpen_Click( Object sender, BEventArgs e ){
        System.out.println( "FormMain#commonFileOpen_Click" );
    }

    public void actionPerformed( ActionEvent e ){
        String cmd = e.getActionCommand();
        if( cmd.equals( "commonFileNew_Click" ) ){
            System.out.println( "commonFileNew_Click; actionPerformed" );
        }
    }

    public void mouseClicked( MouseEvent e ){
    }
    
    public void mouseEntered( MouseEvent e ){
        System.out.println( "mouseEntered" );
    }
    
    public void mouseExited( MouseEvent e ){
        
    }
    
    public void mousePressed( MouseEvent e ){

    }
    
    public void mouseReleased( MouseEvent e ){
    }
}
