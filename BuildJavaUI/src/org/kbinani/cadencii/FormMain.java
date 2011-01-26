package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.GridLayout;
import java.awt.Insets;
import javax.swing.BorderFactory;
import javax.swing.BoxLayout;
import javax.swing.JPanel;
import javax.swing.JSeparator;
import javax.swing.WindowConstants;
import javax.swing.border.EmptyBorder;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BHScrollBar;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BMenu;
import org.kbinani.windows.forms.BMenuBar;
import org.kbinani.windows.forms.BMenuItem;
import org.kbinani.windows.forms.BPanel;
import org.kbinani.windows.forms.BPopupMenu;
import org.kbinani.windows.forms.BSlider;
import org.kbinani.windows.forms.BSplitPane;
import org.kbinani.windows.forms.BToggleButton;
import org.kbinani.windows.forms.BToolBar;
import org.kbinani.windows.forms.BToolBarButton;
import org.kbinani.windows.forms.BVScrollBar;

//SECTION-END-IMPORT
public class FormMain extends BForm {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
    private JPanel jContentPane = null;
    private BMenuBar menuStripMain = null;
    private BMenu menuFile = null;
    private BMenuItem menuFileNew = null;
    private BMenuItem menuFileOpen = null;
    private BMenuItem menuFileSave = null;
    private BMenuItem menuFileSaveNamed = null;
    private JSeparator toolStripMenuItem10 = null;
    private BMenuItem menuFileOpenVsq = null;
    private BMenuItem menuFileOpenUst = null;
    private BMenu menuFileImport = null;
    private BMenu menuFileExport = null;
    private JSeparator toolStripMenuItem101 = null;
    private BMenu menuFileRecent = null;
    private JSeparator toolStripMenuItem102 = null;
    private BMenuItem menuFileQuit = null;
    private BMenuItem menuFileImportVsq = null;
    private BMenuItem menuFileImportMidi = null;
    private BMenuItem menuFileExportWave = null;
    private BMenuItem menuFileExportMidi = null;
    private BMenu menuEdit = null;
    private BMenuItem menuEditUndo = null;
    private BMenuItem menuEditRedo = null;
    private JSeparator toolStripMenuItem103 = null;
    private BMenuItem menuEditCut = null;
    private BMenuItem menuEditCopy = null;
    private BMenuItem menuEditPaste = null;
    private BMenuItem menuEditDelete = null;
    private JSeparator toolStripMenuItem104 = null;
    private BMenuItem menuEditAutoNormalizeMode = null;
    private JSeparator toolStripMenuItem1041 = null;
    private BMenuItem menuEditSelectAll = null;
    private BMenuItem menuEditSelectAllEvents = null;
    private BMenu menuVisual = null;
    private BMenuItem menuVisualControlTrack = null;
    private BMenuItem menuVisualMixer = null;
    private BMenuItem menuVisualWaveform = null;
    private BMenuItem menuVisualProperty = null;
    private BMenuItem menuVisualOverview = null;
    private JSeparator toolStripMenuItem1031 = null;
    private BMenuItem menuVisualGridline = null;
    private JSeparator toolStripMenuItem1032 = null;
    private BMenuItem menuVisualStartMarker = null;
    private BMenuItem menuVisualEndMarker = null;
    private JSeparator toolStripMenuItem1033 = null;
    private BMenuItem menuVisualNoteProperty = null;
    private BMenuItem menuVisualLyrics = null;
    private BMenuItem menuVisualPitchLine = null;
    private BMenu menuJob = null;
    private BMenuItem menuJobNormalize = null;
    private BMenuItem menuJobInsertBar = null;
    private BMenuItem menuJobDeleteBar = null;
    private BMenuItem menuJobRandomize = null;
    private BMenuItem menuJobConnect = null;
    private BMenuItem menuJobLyric = null;
    private BMenu menuTrack = null;
    private BMenuItem menuTrackOn = null;
    private JSeparator toolStripMenuItem10321 = null;
    private BMenuItem menuTrackAdd = null;
    private BMenuItem menuTrackCopy = null;
    private BMenuItem menuTrackChangeName = null;
    private BMenuItem menuTrackDelete = null;
    private JSeparator toolStripMenuItem10322 = null;
    private BMenuItem menuTrackRenderCurrent = null;
    private BMenuItem menuTrackRenderAll = null;
    private JSeparator toolStripMenuItem10323 = null;
    private BMenuItem menuTrackOverlay = null;
    private BMenu menuTrackRenderer = null;
    private JSeparator toolStripMenuItem10324 = null;
    private BMenu menuTrackBgm = null;
    private BMenu menuLyric = null;
    private BMenuItem menuLyricExpressionProperty = null;
    private BMenuItem menuLyricVibratoProperty = null;
    private BMenuItem menuLyricPhonemeTransformation = null;
    private BMenuItem menuLyricDictionary = null;
    private BMenu menuScript = null;
    private BMenuItem menuScriptUpdate = null;
    private BMenu menuSetting = null;
    private BMenuItem menuSettingPreference = null;
    private BMenu menuSettingGameControler = null;
    private BMenuItem menuSettingShortcut = null;
    private BMenuItem menuSettingVibratoPreset = null;
    private JSeparator toolStripMenuItem103211 = null;
    private BMenuItem menuSettingDefaultSingerStyle = null;
    private JSeparator toolStripMenuItem103212 = null;
    private BMenu menuSettingPositionQuantize = null;
    private BMenuItem menuSettingSingerProperty = null;
    private BMenu menuSettingPaletteTool = null;
    private BMenuItem menuSettingPositionQuantize04 = null;
    private BMenuItem menuSettingPositionQuantize08 = null;
    private BMenuItem menuSettingPositionQuantize16 = null;
    private BMenuItem menuSettingPositionQuantize32 = null;
    private BMenuItem menuSettingPositionQuantize64 = null;
    private BMenuItem menuSettingPositionQuantize128 = null;
    private BMenuItem menuSettingPositionQuantizeOff = null;
    private JSeparator toolStripMenuItem1032121 = null;
    private BMenuItem menuSettingPositionQuantizeTriplet = null;
    private BMenu menuHelp = null;
    private BMenuItem menuHelpAbout = null;
    private BSplitPane splitContainer2 = null;
    private BPanel panel1 = null;
    private BPanel panel2 = null;
    private BSplitPane splitContainer1 = null;
    private TrackSelector trackSelector = null;
    private BSplitPane splitContainerProperty = null;
    private BPanel m_property_panel_container = null;
    private BToolBar toolStripFile = null;
    private BToolBarButton stripBtnFileNew = null;
    private BToolBarButton stripBtnFileOpen = null;
    private BToolBarButton stripBtnFileSave = null;
    private BToolBarButton stripBtnCut = null;
    private BToolBarButton stripBtnCopy = null;
    private BToolBarButton stripBtnPaste = null;
    private BToolBarButton stripBtnUndo = null;
    private BToolBarButton stripBtnRedo = null;
    private BToolBar toolStripPosition = null;
    private BToolBarButton stripBtnMoveTop = null;
    private BPanel BPanel = null;
    private BToolBarButton stripBtnRewind = null;
    private BToolBarButton stripBtnForward = null;
    private BToolBarButton stripBtnMoveEnd = null;
    private BToolBarButton stripBtnPlay = null;
    private BToolBarButton stripBtnScroll = null;
    private BToolBarButton stripBtnLoop = null;
    private BToolBar toolStripTool = null;
    private BToolBarButton stripBtnPointer = null;
    private BToolBarButton stripBtnPencil = null;
    private BToolBarButton stripBtnLine = null;
    private BToolBarButton stripBtnEraser = null;
    private BToolBarButton stripBtnGrid = null;
    private BToolBarButton stripBtnCurve = null;
    private BPopupMenu cMenuTrackSelector = null;  //  @jve:decl-index=0:visual-constraint="985,100"
    private BPopupMenu cMenuPiano = null;  //  @jve:decl-index=0:visual-constraint="984,231"
    private BPopupMenu cMenuTrackTab = null;  //  @jve:decl-index=0:visual-constraint="985,358"
    private BMenuItem cMenuTrackSelectorPointer = null;
    private BMenuItem cMenuTrackSelectorPencil = null;
    private BMenuItem cMenuTrackSelectorLine = null;
    private BMenuItem cMenuTrackSelectorEraser = null;
    private BMenuItem cMenuTrackSelectorPaletteTool = null;
    private BMenuItem cMenuTrackSelectorCurve = null;
    private BMenuItem cMenuTrackSelectorUndo = null;
    private BMenuItem cMenuTrackSelectorRedo = null;
    private BMenuItem cMenuTrackSelectorCut = null;
    private BMenuItem cMenuTrackSelectorCopy = null;
    private BMenuItem cMenuTrackSelectorPaste = null;
    private BMenuItem cMenuTrackSelectorDelete = null;
    private BMenuItem cMenuTrackSelectorDeleteBezier = null;
    private BMenuItem cMenuTrackSelectorSelectAll = null;
    private BMenuItem cMenuPianoPointer = null;
    private BMenuItem cMenuPianoPencil = null;
    private BMenuItem cMenuPianoEraser = null;
    private BMenuItem cMenuPianoPaletteTool = null;
    private BMenuItem cMenuPianoCurve = null;
    private BMenu cMenuPianoFixed = null;
    private BMenu cMenuPianoQuantize = null;
    private BMenuItem cMenuPianoGrid = null;
    private BMenuItem cMenuPianoUndo = null;
    private BMenuItem cMenuPianoRedo = null;
    private BMenuItem cMenuPianoCut = null;
    private BMenuItem cMenuPianoCopy = null;
    private BMenuItem cMenuPianoPaste = null;
    private BMenuItem cMenuPianoDelete = null;
    private BMenuItem cMenuPianoSelectAll = null;
    private BMenuItem cMenuPianoSelectAllEvents = null;
    private BMenuItem cMenuPianoImportLyric = null;
    private BMenuItem cMenuPianoExpressionProperty = null;
    private BMenuItem cMenuPianoVibratoProperty = null;
    private BMenuItem cMenuPianoFixed02 = null;
    private BMenuItem cMenuPianoFixed04 = null;
    private BMenuItem cMenuPianoFixed08 = null;
    private BMenuItem cMenuPianoFixed01 = null;
    private BMenuItem cMenuPianoFixed16 = null;
    private BMenuItem cMenuPianoFixed32 = null;
    private BMenuItem cMenuPianoFixed64 = null;
    private BMenuItem cMenuPianoFixed128 = null;
    private BMenuItem cMenuPianoFixedOff = null;
    private BMenuItem cMenuPianoFixedTriplet = null;
    private BMenuItem cMenuPianoFixedDotted = null;
    private BMenuItem cMenuPianoQuantize04 = null;
    private BMenuItem cMenuPianoQuantize08 = null;
    private BMenuItem cMenuPianoQuantize16 = null;
    private BMenuItem cMenuPianoQuantize32 = null;
    private BMenuItem cMenuPianoQuantize64 = null;
    private BMenuItem cMenuPianoQuantize128 = null;
    private BMenuItem cMenuPianoQuantizeTriplet = null;
    private BMenuItem cMenuTrackTabTrackOn = null;
    private BMenuItem cMenuTrackTabAdd = null;
    private BMenuItem cMenuTrackTabCopy = null;
    private BMenuItem cMenuTrackTabChangeName = null;
    private BMenuItem cMenuTrackTabDelete = null;
    private BMenuItem cMenuTrackTabRenderCurrent = null;
    private BMenuItem cMenuTrackTabRenderAll = null;
    private BMenuItem cMenuTrackTabOverlay = null;
    private BMenu cMenuTrackTabRenderer = null;
    private BMenuItem cMenuTrackTabRendererVOCALOID2 = null;
    private BMenuItem cMenuTrackTabRendererVOCALOID100 = null;
    private BMenu cMenuTrackTabRendererUtau = null;
    private BMenuItem cMenuTrackTabRendererStraight = null;
    private BMenuItem cMenuPianoQuantizeOff = null;
    private BPanel panel3 = null;
    private PictOverview panelOverview = null;
    public PictPianoRoll pictPianoRoll = null;
    private BVScrollBar vScroll = null;
    public BHScrollBar hScroll = null;
    private BPanel pictureBox3 = null;
    private BSlider trackBar = null;
    private BButton pictKeyLengthSplitter = null;
    private BPanel picturePositionIndicator = null;
    private BPanel jPanel1 = null;
    private BPanel jPanel3 = null;
    private BLabel statusLabel = null;
    private BMenu menuHidden = null;
    private BMenuItem menuHiddenEditLyric = null;
    private BMenuItem menuHiddenEditFlipToolPointerPencil = null;
    private BMenuItem menuHiddenEditFlipToolPointerEraser = null;
    private BMenuItem menuHiddenVisualForwardParameter = null;
    private BMenuItem menuHiddenVisualBackwardParameter = null;
    private BMenuItem menuHiddenTrackNext = null;
    private BMenuItem menuHiddenTrackBack = null;
    private BMenuItem menuHiddenCopy = null;
    private BMenuItem menuHiddenPaste = null;
    private BMenuItem menuHiddenCut = null;
    private BMenuItem menuTrackRendererVOCALOID100 = null;
    private BMenuItem menuTrackRendererVOCALOID2 = null;
    private BMenu menuTrackRendererUtau = null;
    private BMenuItem menuTrackRendererVCNT = null;
    private BMenuItem menuSettingGameControlerSetting = null;
    private BMenuItem menuSettingGameControlerLoad = null;
    private BMenuItem menuSettingGameControlerRemove = null;
    private BMenuItem menuHelpDebug = null;
    private BPanel pictureBox2 = null;
    private BMenu menuVisualPluginUi = null;
    private BMenuItem menuVisualPluginUiVocaloid100 = null;
    private BMenuItem menuVisualPluginUiVocaloid101 = null;
    private BMenuItem menuVisualPluginUiVocaloid2 = null;
    private BMenuItem menuVisualPluginUiAquesTone = null;
    private BMenuItem menuHiddenSelectForward = null;
    private BMenuItem menuHiddenSelectBackward = null;
    private BMenuItem menuHiddenMoveUp = null;
    private BMenuItem menuHiddenMoveDown = null;
    private BMenuItem menuHiddenMoveLeft = null;
    private BMenuItem menuHiddenMoveRight = null;
    private BMenuItem menuHiddenLengthen = null;
    private BMenuItem menuHiddenShorten = null;
    private BMenuItem menuHiddenGoToStartMarker = null;
    private BMenuItem menuHiddenGoToEndMarker = null;
    private BMenuItem menuFileExportMusicXml = null;
    private BMenuItem cMenuTrackTabRendererVOCALOID101 = null;
    private BMenuItem menuTrackRendererVOCALOID101 = null;
    private BMenuItem menuTrackRendererAquesTone = null;
    private BMenuItem cMenuTrackTabRendererAquesTone = null;
    private BMenuItem menuVisualIconPalette = null;
    private BMenuItem menuHiddenPlayFromStartMarker = null;
    private BMenuItem menuHiddenFlipCurveOnPianorollMode = null;
    private BMenuItem menuFileImportUst = null;
    private BMenuItem menuFileExportParaWave = null;
    private BMenuItem menuFileExportVsq = null;
    private BMenuItem menuFileExportUst = null;
    private BMenuItem menuFileExportVxt = null;
    private BMenu menuLyricCopyVibratoToPreset = null;
    private BMenuItem menuSettingSequence = null;
    private BMenu menuHelpLog = null;
    private BMenuItem menuHelpLogSwitch = null;
    private BMenuItem menuHelpLogOpen = null;
    private BMenuItem menuHiddenPrintPoToCSV = null;
    private BToggleButton stripBtnStepSequencer = null;
    private JPanel jPanel2 = null;
    private BPopupMenu cMenuPositionIndicator = null;  //  @jve:decl-index=0:visual-constraint="991,295"
    private BMenuItem cMenuPositionIndicatorStartMarker = null;
    private BMenuItem cMenuPositionIndicatorEndMarker = null;
    //SECTION-END-FIELD
    public FormMain( String vsq_file ) {
        super();
        initialize();
    }
    //SECTION-BEGIN-METHOD

    /**
     * This method initializes menuFileExportMusicXml	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuFileExportMusicXml() {
        if (menuFileExportMusicXml == null) {
            menuFileExportMusicXml = new BMenuItem();
            menuFileExportMusicXml.setText("MusicXML");
        }
        return menuFileExportMusicXml;
    }

    /**
     * This method initializes cMenuTrackTabRendererVOCALOID101	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getCMenuTrackTabRendererVOCALOID101() {
        if (cMenuTrackTabRendererVOCALOID101 == null) {
            cMenuTrackTabRendererVOCALOID101 = new BMenuItem();
            cMenuTrackTabRendererVOCALOID101.setText("VOCALOID1 [1.1]");
        }
        return cMenuTrackTabRendererVOCALOID101;
    }

    /**
     * This method initializes menuTrackRendererVOCALOID101	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuTrackRendererVOCALOID101() {
        if (menuTrackRendererVOCALOID101 == null) {
            menuTrackRendererVOCALOID101 = new BMenuItem();
            menuTrackRendererVOCALOID101.setText("VOCALOID1 [1.1]");
        }
        return menuTrackRendererVOCALOID101;
    }

    /**
     * This method initializes menuTrackRendererAquesTone	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuTrackRendererAquesTone() {
        if (menuTrackRendererAquesTone == null) {
            menuTrackRendererAquesTone = new BMenuItem();
            menuTrackRendererAquesTone.setText("AquesTone");
        }
        return menuTrackRendererAquesTone;
    }

    /**
     * This method initializes cMenuTrackTabRendererAquesTone	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getCMenuTrackTabRendererAquesTone() {
        if (cMenuTrackTabRendererAquesTone == null) {
            cMenuTrackTabRendererAquesTone = new BMenuItem();
            cMenuTrackTabRendererAquesTone.setText("AquesTone");
        }
        return cMenuTrackTabRendererAquesTone;
    }

    /**
     * This method initializes menuVisualIconPalette	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuVisualIconPalette() {
        if (menuVisualIconPalette == null) {
            menuVisualIconPalette = new BMenuItem();
            menuVisualIconPalette.setText("Icon Palette");
            menuVisualIconPalette.setCheckOnClick(true);
        }
        return menuVisualIconPalette;
    }

    /**
     * This method initializes menuHiddenPlayFromStartMarker	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHiddenPlayFromStartMarker() {
        if (menuHiddenPlayFromStartMarker == null) {
            menuHiddenPlayFromStartMarker = new BMenuItem();
            menuHiddenPlayFromStartMarker.setText("Play From Start Marker");
        }
        return menuHiddenPlayFromStartMarker;
    }

    /**
     * This method initializes menuHiddenFlipCurveOnPianorollMode	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHiddenFlipCurveOnPianorollMode() {
        if (menuHiddenFlipCurveOnPianorollMode == null) {
            menuHiddenFlipCurveOnPianorollMode = new BMenuItem();
            menuHiddenFlipCurveOnPianorollMode.setText("Change pitch drawing mode");
        }
        return menuHiddenFlipCurveOnPianorollMode;
    }

    /**
     * This method initializes menuFileImportUst	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuFileImportUst() {
        if (menuFileImportUst == null) {
            menuFileImportUst = new BMenuItem();
            menuFileImportUst.setText("UTAU project file");
        }
        return menuFileImportUst;
    }

    /**
     * This method initializes menuFileExportParaWave	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuFileExportParaWave() {
        if (menuFileExportParaWave == null) {
            menuFileExportParaWave = new BMenuItem();
            menuFileExportParaWave.setText("Serial numbered Wave");
        }
        return menuFileExportParaWave;
    }

    /**
     * This method initializes menuFileExportVsq	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuFileExportVsq() {
        if (menuFileExportVsq == null) {
            menuFileExportVsq = new BMenuItem();
            menuFileExportVsq.setText("VSQ File");
        }
        return menuFileExportVsq;
    }

    /**
     * This method initializes menuFileExportUst	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuFileExportUst() {
        if (menuFileExportUst == null) {
            menuFileExportUst = new BMenuItem();
            menuFileExportUst.setText("UTAU Project File (current track)");
        }
        return menuFileExportUst;
    }

    /**
     * This method initializes menuFileExportVxt	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuFileExportVxt() {
        if (menuFileExportVxt == null) {
            menuFileExportVxt = new BMenuItem();
            menuFileExportVxt.setText("Metatext for vConnect");
        }
        return menuFileExportVxt;
    }

    /**
     * This method initializes menuLyricCopyVibratoToPreset	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenu getMenuLyricCopyVibratoToPreset() {
        if (menuLyricCopyVibratoToPreset == null) {
            menuLyricCopyVibratoToPreset = new BMenu();
            menuLyricCopyVibratoToPreset.setText("Copy vibrato config to preset");
        }
        return menuLyricCopyVibratoToPreset;
    }

    /**
     * This method initializes menuSettingSequence	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuSettingSequence() {
        if (menuSettingSequence == null) {
            menuSettingSequence = new BMenuItem();
            menuSettingSequence.setText("Sequence config");
        }
        return menuSettingSequence;
    }

    /**
     * This method initializes menuHelpLog	
     * 	
     * @return org.kbinani.windows.forms.BMenu	
     */
    private BMenu getMenuHelpLog() {
        if (menuHelpLog == null) {
            menuHelpLog = new BMenu();
            menuHelpLog.setText("Log");
            menuHelpLog.add(getMenuHelpLogSwitch());
            menuHelpLog.add(getMenuHelpLogOpen());
        }
        return menuHelpLog;
    }

    /**
     * This method initializes menuHelpLogSwitch	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHelpLogSwitch() {
        if (menuHelpLogSwitch == null) {
            menuHelpLogSwitch = new BMenuItem();
            menuHelpLogSwitch.setText("Enable log");
            menuHelpLogSwitch.setCheckOnClick(true);
        }
        return menuHelpLogSwitch;
    }

    /**
     * This method initializes menuHelpLogOpen	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHelpLogOpen() {
        if (menuHelpLogOpen == null) {
            menuHelpLogOpen = new BMenuItem();
            menuHelpLogOpen.setText("Open");
        }
        return menuHelpLogOpen;
    }

    /**
     * This method initializes menuHiddenPrintPoToCSV	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHiddenPrintPoToCSV() {
        if (menuHiddenPrintPoToCSV == null) {
            menuHiddenPrintPoToCSV = new BMenuItem();
            menuHiddenPrintPoToCSV.setText("Print language configs to CSV");
        }
        return menuHiddenPrintPoToCSV;
    }

    /**
     * This method initializes stripBtnStepSequencer	
     * 	
     * @return org.kbinani.windows.forms.BToggleButton	
     */
    private BToggleButton getStripBtnStepSequencer() {
        if (stripBtnStepSequencer == null) {
            stripBtnStepSequencer = new BToggleButton();
            stripBtnStepSequencer.setText("Step");
        }
        return stripBtnStepSequencer;
    }


    /**
     * This method initializes this
     * 
     * @return void
     */
    private void initialize() {
        this.setSize(960, 636);
        this.setDefaultCloseOperation(WindowConstants.DISPOSE_ON_CLOSE);
        this.setJMenuBar(getMenuStripMain());
        this.setContentPane(this.getJContentPane());
        this.setTitle("JFrame");
        this.getCMenuPiano();
        this.getCMenuTrackSelector();
        this.getCMenuTrackTab();
        this.getCMenuPositionIndicator();
    }

    /**
     * This method initializes jContentPane
     * 
     * @return javax.swing.BPanel
     */
    private JPanel getJContentPane() {
        if (jContentPane == null) {
            jContentPane = new JPanel();
            jContentPane.setLayout(new BorderLayout());
            jContentPane.add(getJPanel(), BorderLayout.NORTH);
            jContentPane.add(getSplitContainerProperty(), BorderLayout.CENTER);
            jContentPane.add(getJPanel3(), BorderLayout.SOUTH);
        }
        return jContentPane;
    }

    /**
     * This method initializes menuStripMain    
     *  
     * @return javax.swing.BMenuBar 
     */
    private BMenuBar getMenuStripMain() {
        if (menuStripMain == null) {
            menuStripMain = new BMenuBar();
            menuStripMain.add(getMenuFile());
            menuStripMain.add(getMenuEdit());
            menuStripMain.add(getMenuVisual());
            menuStripMain.add(getMenuJob());
            menuStripMain.add(getMenuTrack());
            menuStripMain.add(getMenuLyric());
            menuStripMain.add(getMenuScript());
            menuStripMain.add(getMenuSetting());
            menuStripMain.add(getMenuHelp());
            menuStripMain.add(getMenuHidden());
        }
        return menuStripMain;
    }

    /**
     * This method initializes menuFile 
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuFile() {
        if (menuFile == null) {
            menuFile = new BMenu();
            menuFile.setText("File");
            menuFile.add(getMenuFileNew());
            menuFile.add(getMenuFileOpen());
            menuFile.add(getMenuFileSave());
            menuFile.add(getMenuFileSaveNamed());
            menuFile.add(getBMenuItem());
            menuFile.add(getBMenuItem2());
            menuFile.add(getBMenuItem3());
            menuFile.add(getMenuFileImport());
            menuFile.add(getMenuFileExport());
            menuFile.add(getToolStripMenuItem101());
            menuFile.add(getBMenuItem4());
            menuFile.add(getToolStripMenuItem102());
            menuFile.add(getBMenuItem5());
        }
        return menuFile;
    }

    /**
     * This method initializes menuFileNew  
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getMenuFileNew() {
        if (menuFileNew == null) {
            menuFileNew = new BMenuItem();
            menuFileNew.setText("New");
        }
        return menuFileNew;
    }

    /**
     * This method initializes menuFileOpen 
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getMenuFileOpen() {
        if (menuFileOpen == null) {
            menuFileOpen = new BMenuItem();
            menuFileOpen.setText("Open");
        }
        return menuFileOpen;
    }

    /**
     * This method initializes menuFileSave 
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getMenuFileSave() {
        if (menuFileSave == null) {
            menuFileSave = new BMenuItem();
            menuFileSave.setText("Save");
        }
        return menuFileSave;
    }

    /**
     * This method initializes menuFileSaveNamed    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getMenuFileSaveNamed() {
        if (menuFileSaveNamed == null) {
            menuFileSaveNamed = new BMenuItem();
            menuFileSaveNamed.setText("Save As");
        }
        return menuFileSaveNamed;
    }

    /**
     * This method initializes toolStripMenuItem10  
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getBMenuItem() {
        if (toolStripMenuItem10 == null) {
            toolStripMenuItem10 = new JSeparator();
        }
        return toolStripMenuItem10;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem2() {
        if (menuFileOpenVsq == null) {
            menuFileOpenVsq = new BMenuItem();
            menuFileOpenVsq.setText("Open VSQ/Vocaloid Midi");
        }
        return menuFileOpenVsq;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem3() {
        if (menuFileOpenUst == null) {
            menuFileOpenUst = new BMenuItem();
            menuFileOpenUst.setText("Open UTAU Project File");
        }
        return menuFileOpenUst;
    }

    /**
     * This method initializes menuFileImport   
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuFileImport() {
        if (menuFileImport == null) {
            menuFileImport = new BMenu();
            menuFileImport.setText("Import");
            menuFileImport.add(getBMenuItem6());
            menuFileImport.add(getBMenuItem7());
            menuFileImport.add(getMenuFileImportUst());
        }
        return menuFileImport;
    }

    /**
     * This method initializes menuFileExport   
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuFileExport() {
        if (menuFileExport == null) {
            menuFileExport = new BMenu();
            menuFileExport.setText("Export");
            menuFileExport.add(getBMenuItem8());
            menuFileExport.add(getMenuFileExportParaWave());
            menuFileExport.add(getMenuFileExportVsq());
            menuFileExport.add(getBMenuItem9());
            menuFileExport.add(getMenuFileExportMusicXml());
            menuFileExport.add(getMenuFileExportUst());
            menuFileExport.add(getMenuFileExportVxt());
        }
        return menuFileExport;
    }

    /**
     * This method initializes toolStripMenuItem101 
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem101() {
        if (toolStripMenuItem101 == null) {
            toolStripMenuItem101 = new JSeparator();
        }
        return toolStripMenuItem101;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenu getBMenuItem4() {
        if (menuFileRecent == null) {
            menuFileRecent = new BMenu();
            menuFileRecent.setText("Recent Files");
        }
        return menuFileRecent;
    }

    /**
     * This method initializes toolStripMenuItem102 
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem102() {
        if (toolStripMenuItem102 == null) {
            toolStripMenuItem102 = new JSeparator();
        }
        return toolStripMenuItem102;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem5() {
        if (menuFileQuit == null) {
            menuFileQuit = new BMenuItem();
            menuFileQuit.setText("Quit");
        }
        return menuFileQuit;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem6() {
        if (menuFileImportVsq == null) {
            menuFileImportVsq = new BMenuItem();
            menuFileImportVsq.setText("VSQ / Vocaloid MIDI");
        }
        return menuFileImportVsq;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem7() {
        if (menuFileImportMidi == null) {
            menuFileImportMidi = new BMenuItem();
            menuFileImportMidi.setText("Standard MIDI");
        }
        return menuFileImportMidi;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem8() {
        if (menuFileExportWave == null) {
            menuFileExportWave = new BMenuItem();
            menuFileExportWave.setText("Wave");
        }
        return menuFileExportWave;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem9() {
        if (menuFileExportMidi == null) {
            menuFileExportMidi = new BMenuItem();
            menuFileExportMidi.setText("MIDI");
        }
        return menuFileExportMidi;
    }

    /**
     * This method initializes menuEdit 
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuEdit() {
        if (menuEdit == null) {
            menuEdit = new BMenu();
            menuEdit.setText("Edit");
            menuEdit.add(getBMenuItem10());
            menuEdit.add(getBMenuItem11());
            menuEdit.add(getToolStripMenuItem103());
            menuEdit.add(getBMenuItem12());
            menuEdit.add(getMenuEditCopy());
            menuEdit.add(getBMenuItem22());
            menuEdit.add(getBMenuItem13());
            menuEdit.add(getToolStripMenuItem104());
            menuEdit.add(getBMenuItem14());
            menuEdit.add(getToolStripMenuItem1041());
            menuEdit.add(getBMenuItem15());
            menuEdit.add(getMenuEditSelectAllEvents());
        }
        return menuEdit;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem10() {
        if (menuEditUndo == null) {
            menuEditUndo = new BMenuItem();
            menuEditUndo.setText("Undo");
        }
        return menuEditUndo;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem11() {
        if (menuEditRedo == null) {
            menuEditRedo = new BMenuItem();
            menuEditRedo.setText("Redo");
        }
        return menuEditRedo;
    }

    /**
     * This method initializes toolStripMenuItem103 
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem103() {
        if (toolStripMenuItem103 == null) {
            toolStripMenuItem103 = new JSeparator();
        }
        return toolStripMenuItem103;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem12() {
        if (menuEditCut == null) {
            menuEditCut = new BMenuItem();
            menuEditCut.setText("Cut");
        }
        return menuEditCut;
    }

    /**
     * This method initializes menuEditCopy 
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getMenuEditCopy() {
        if (menuEditCopy == null) {
            menuEditCopy = new BMenuItem();
            menuEditCopy.setText("Copy");
        }
        return menuEditCopy;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem22() {
        if (menuEditPaste == null) {
            menuEditPaste = new BMenuItem();
            menuEditPaste.setText("Paste");
        }
        return menuEditPaste;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem13() {
        if (menuEditDelete == null) {
            menuEditDelete = new BMenuItem();
            menuEditDelete.setText("Delete");
        }
        return menuEditDelete;
    }

    /**
     * This method initializes toolStripMenuItem104 
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem104() {
        if (toolStripMenuItem104 == null) {
            toolStripMenuItem104 = new JSeparator();
        }
        return toolStripMenuItem104;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem14() {
        if (menuEditAutoNormalizeMode == null) {
            menuEditAutoNormalizeMode = new BMenuItem();
            menuEditAutoNormalizeMode.setText("Auto Normalize Mode");
        }
        return menuEditAutoNormalizeMode;
    }

    /**
     * This method initializes toolStripMenuItem1041    
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem1041() {
        if (toolStripMenuItem1041 == null) {
            toolStripMenuItem1041 = new JSeparator();
        }
        return toolStripMenuItem1041;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem15() {
        if (menuEditSelectAll == null) {
            menuEditSelectAll = new BMenuItem();
            menuEditSelectAll.setText("Select All");
        }
        return menuEditSelectAll;
    }

    /**
     * This method initializes menuEditSelectAllEvents  
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getMenuEditSelectAllEvents() {
        if (menuEditSelectAllEvents == null) {
            menuEditSelectAllEvents = new BMenuItem();
            menuEditSelectAllEvents.setText("Select All Events");
        }
        return menuEditSelectAllEvents;
    }

    /**
     * This method initializes menuVisual   
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuVisual() {
        if (menuVisual == null) {
            menuVisual = new BMenu();
            menuVisual.setText("Visual");
            menuVisual.add(getBMenuItem16());
            menuVisual.add(getBMenuItem17());
            menuVisual.add(getMenuVisualWaveform());
            menuVisual.add(getMenuVisualIconPalette());
            menuVisual.add(getBMenuItem23());
            menuVisual.add(getBMenuItem32());
            menuVisual.add(getMenuVisualPluginUi());
            menuVisual.add(getToolStripMenuItem1031());
            menuVisual.add(getBMenuItem18());
            menuVisual.add(getToolStripMenuItem1032());
            menuVisual.add(getBMenuItem19());
            menuVisual.add(getMenuVisualEndMarker());
            menuVisual.add(getToolStripMenuItem1033());
            menuVisual.add(getMenuVisualLyrics());
            menuVisual.add(getBMenuItem20());
            menuVisual.add(getBMenuItem24());
        }
        return menuVisual;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem16() {
        if (menuVisualControlTrack == null) {
            menuVisualControlTrack = new BMenuItem();
            menuVisualControlTrack.setText("Control Track");
            menuVisualControlTrack.setSelected(true);
            menuVisualControlTrack.setCheckOnClick(true);
        }
        return menuVisualControlTrack;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem17() {
        if (menuVisualMixer == null) {
            menuVisualMixer = new BMenuItem();
            menuVisualMixer.setText("Mixer");
            menuVisualMixer.setCheckOnClick(true);
        }
        return menuVisualMixer;
    }

    /**
     * This method initializes menuVisualWaveform   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getMenuVisualWaveform() {
        if (menuVisualWaveform == null) {
            menuVisualWaveform = new BMenuItem();
            menuVisualWaveform.setText("Waveform");
            menuVisualWaveform.setCheckOnClick(true);
        }
        return menuVisualWaveform;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem23() {
        if (menuVisualProperty == null) {
            menuVisualProperty = new BMenuItem();
            menuVisualProperty.setText("Property Window");
            menuVisualProperty.setCheckOnClick(true);
        }
        return menuVisualProperty;
    }

    /**
     * This method initializes BMenuItem3   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem32() {
        if (menuVisualOverview == null) {
            menuVisualOverview = new BMenuItem();
            menuVisualOverview.setText("Navigation");
            menuVisualOverview.setCheckOnClick(true);
        }
        return menuVisualOverview;
    }

    /**
     * This method initializes toolStripMenuItem1031    
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem1031() {
        if (toolStripMenuItem1031 == null) {
            toolStripMenuItem1031 = new JSeparator();
        }
        return toolStripMenuItem1031;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem18() {
        if (menuVisualGridline == null) {
            menuVisualGridline = new BMenuItem();
            menuVisualGridline.setText("Grid Line");
            menuVisualGridline.setCheckOnClick(true);
        }
        return menuVisualGridline;
    }

    /**
     * This method initializes toolStripMenuItem1032    
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem1032() {
        if (toolStripMenuItem1032 == null) {
            toolStripMenuItem1032 = new JSeparator();
        }
        return toolStripMenuItem1032;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem19() {
        if (menuVisualStartMarker == null) {
            menuVisualStartMarker = new BMenuItem();
            menuVisualStartMarker.setText("Start Marker");
            menuVisualStartMarker.setCheckOnClick(false);
        }
        return menuVisualStartMarker;
    }

    /**
     * This method initializes menuVisualEndMarker  
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getMenuVisualEndMarker() {
        if (menuVisualEndMarker == null) {
            menuVisualEndMarker = new BMenuItem();
            menuVisualEndMarker.setText("End Marker");
            menuVisualEndMarker.setCheckOnClick(false);
        }
        return menuVisualEndMarker;
    }

    /**
     * This method initializes toolStripMenuItem1033    
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem1033() {
        if (toolStripMenuItem1033 == null) {
            toolStripMenuItem1033 = new JSeparator();
        }
        return toolStripMenuItem1033;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem20() {
        if (menuVisualNoteProperty == null) {
            menuVisualNoteProperty = new BMenuItem();
            menuVisualNoteProperty.setText("Note Expression/Vibrato");
            menuVisualNoteProperty.setCheckOnClick(true);
        }
        return menuVisualNoteProperty;
    }

    /**
     * This method initializes menuVisualLyrics 
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getMenuVisualLyrics() {
        if (menuVisualLyrics == null) {
            menuVisualLyrics = new BMenuItem();
            menuVisualLyrics.setText("Lyrics/Phoneme");
            menuVisualLyrics.setCheckOnClick(true);
        }
        return menuVisualLyrics;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem24() {
        if (menuVisualPitchLine == null) {
            menuVisualPitchLine = new BMenuItem();
            menuVisualPitchLine.setText("Pitch Line");
            menuVisualPitchLine.setCheckOnClick(true);
        }
        return menuVisualPitchLine;
    }

    /**
     * This method initializes menuJob  
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuJob() {
        if (menuJob == null) {
            menuJob = new BMenu();
            menuJob.setText("Job");
            menuJob.add(getBMenuItem21());
            menuJob.add(getMenuJobInsertBar());
            menuJob.add(getBMenuItem25());
            menuJob.add(getBMenuItem33());
            menuJob.add(getBMenuItem42());
            menuJob.add(getBMenuItem52());
        }
        return menuJob;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem21() {
        if (menuJobNormalize == null) {
            menuJobNormalize = new BMenuItem();
            menuJobNormalize.setText("Normalize Notes");
        }
        return menuJobNormalize;
    }

    /**
     * This method initializes menuJobInsertBar 
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getMenuJobInsertBar() {
        if (menuJobInsertBar == null) {
            menuJobInsertBar = new BMenuItem();
            menuJobInsertBar.setText("Insert Bars");
        }
        return menuJobInsertBar;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem25() {
        if (menuJobDeleteBar == null) {
            menuJobDeleteBar = new BMenuItem();
            menuJobDeleteBar.setText("Delete Bars");
        }
        return menuJobDeleteBar;
    }

    /**
     * This method initializes BMenuItem3   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem33() {
        if (menuJobRandomize == null) {
            menuJobRandomize = new BMenuItem();
            menuJobRandomize.setText("Randomize");
        }
        return menuJobRandomize;
    }

    /**
     * This method initializes BMenuItem4   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem42() {
        if (menuJobConnect == null) {
            menuJobConnect = new BMenuItem();
            menuJobConnect.setText("Connect Notes");
        }
        return menuJobConnect;
    }

    /**
     * This method initializes BMenuItem5   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem52() {
        if (menuJobLyric == null) {
            menuJobLyric = new BMenuItem();
            menuJobLyric.setText("Insert Lyrics");
        }
        return menuJobLyric;
    }

    /**
     * This method initializes menuTrack    
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuTrack() {
        if (menuTrack == null) {
            menuTrack = new BMenu();
            menuTrack.setText("Track");
            menuTrack.add(getBMenuItem27());
            menuTrack.add(getToolStripMenuItem10321());
            menuTrack.add(getMenuTrackAdd());
            menuTrack.add(getBMenuItem28());
            menuTrack.add(getBMenuItem34());
            menuTrack.add(getBMenuItem43());
            menuTrack.add(getToolStripMenuItem10322());
            menuTrack.add(getBMenuItem53());
            menuTrack.add(getBMenuItem63());
            menuTrack.add(getToolStripMenuItem10323());
            menuTrack.add(getBMenuItem73());
            menuTrack.add(getMenuTrackRenderer());
            menuTrack.add(getToolStripMenuItem10324());
            menuTrack.add(getMenuTrackBgm());
        }
        return menuTrack;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem27() {
        if (menuTrackOn == null) {
            menuTrackOn = new BMenuItem();
            menuTrackOn.setText("Track On");
        }
        return menuTrackOn;
    }

    /**
     * This method initializes toolStripMenuItem10321   
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem10321() {
        if (toolStripMenuItem10321 == null) {
            toolStripMenuItem10321 = new JSeparator();
        }
        return toolStripMenuItem10321;
    }

    /**
     * This method initializes menuTrackAdd 
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getMenuTrackAdd() {
        if (menuTrackAdd == null) {
            menuTrackAdd = new BMenuItem();
            menuTrackAdd.setText("Add Track");
        }
        return menuTrackAdd;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem28() {
        if (menuTrackCopy == null) {
            menuTrackCopy = new BMenuItem();
            menuTrackCopy.setText("Copy Track");
        }
        return menuTrackCopy;
    }

    /**
     * This method initializes BMenuItem3   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem34() {
        if (menuTrackChangeName == null) {
            menuTrackChangeName = new BMenuItem();
            menuTrackChangeName.setText("Rename Track");
        }
        return menuTrackChangeName;
    }

    /**
     * This method initializes BMenuItem4   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem43() {
        if (menuTrackDelete == null) {
            menuTrackDelete = new BMenuItem();
            menuTrackDelete.setText("Delete Track");
        }
        return menuTrackDelete;
    }

    /**
     * This method initializes toolStripMenuItem10322   
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem10322() {
        if (toolStripMenuItem10322 == null) {
            toolStripMenuItem10322 = new JSeparator();
        }
        return toolStripMenuItem10322;
    }

    /**
     * This method initializes BMenuItem5   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem53() {
        if (menuTrackRenderCurrent == null) {
            menuTrackRenderCurrent = new BMenuItem();
            menuTrackRenderCurrent.setText("Render Current Track");
        }
        return menuTrackRenderCurrent;
    }

    /**
     * This method initializes BMenuItem6   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem63() {
        if (menuTrackRenderAll == null) {
            menuTrackRenderAll = new BMenuItem();
            menuTrackRenderAll.setText("Render All Tracks");
        }
        return menuTrackRenderAll;
    }

    /**
     * This method initializes toolStripMenuItem10323   
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem10323() {
        if (toolStripMenuItem10323 == null) {
            toolStripMenuItem10323 = new JSeparator();
        }
        return toolStripMenuItem10323;
    }

    /**
     * This method initializes BMenuItem7   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem73() {
        if (menuTrackOverlay == null) {
            menuTrackOverlay = new BMenuItem();
            menuTrackOverlay.setText("Overlay");
            menuTrackOverlay.setCheckOnClick(true);
        }
        return menuTrackOverlay;
    }

    /**
     * This method initializes menuTrackRenderer    
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuTrackRenderer() {
        if (menuTrackRenderer == null) {
            menuTrackRenderer = new BMenu();
            menuTrackRenderer.setText("Renderer");
            menuTrackRenderer.add(getMenuTrackRendererVOCALOID100());
            menuTrackRenderer.add(getMenuTrackRendererVOCALOID101());
            menuTrackRenderer.add(getMenuTrackRendererVOCALOID2());
            menuTrackRenderer.add(getMenuTrackRendererUtau());
            menuTrackRenderer.add(getMenuTrackRendererVCNT());
            menuTrackRenderer.add(getMenuTrackRendererAquesTone());
        }
        return menuTrackRenderer;
    }

    /**
     * This method initializes toolStripMenuItem10324   
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem10324() {
        if (toolStripMenuItem10324 == null) {
            toolStripMenuItem10324 = new JSeparator();
        }
        return toolStripMenuItem10324;
    }

    /**
     * This method initializes menuTrackBgm 
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuTrackBgm() {
        if (menuTrackBgm == null) {
            menuTrackBgm = new BMenu();
            menuTrackBgm.setText("BGM");
        }
        return menuTrackBgm;
    }

    /**
     * This method initializes menuLyric    
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuLyric() {
        if (menuLyric == null) {
            menuLyric = new BMenu();
            menuLyric.setText("Lyrics");
            menuLyric.add(getBMenuItem29());
            menuLyric.add(getMenuLyricVibratoProperty());
            menuLyric.add(getBMenuItem210());
            menuLyric.add(getBMenuItem35());
            menuLyric.add(getMenuLyricCopyVibratoToPreset());
        }
        return menuLyric;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem29() {
        if (menuLyricExpressionProperty == null) {
            menuLyricExpressionProperty = new BMenuItem();
            menuLyricExpressionProperty.setText("Note Expression Propertry");
        }
        return menuLyricExpressionProperty;
    }

    /**
     * This method initializes menuLyricVibratoProperty 
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getMenuLyricVibratoProperty() {
        if (menuLyricVibratoProperty == null) {
            menuLyricVibratoProperty = new BMenuItem();
            menuLyricVibratoProperty.setText("Note Vibrato Property");
        }
        return menuLyricVibratoProperty;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem210() {
        if (menuLyricPhonemeTransformation == null) {
            menuLyricPhonemeTransformation = new BMenuItem();
            menuLyricPhonemeTransformation.setText("Phoneme Transformation");
        }
        return menuLyricPhonemeTransformation;
    }

    /**
     * This method initializes BMenuItem3   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem35() {
        if (menuLyricDictionary == null) {
            menuLyricDictionary = new BMenuItem();
            menuLyricDictionary.setText("User Word Dictionary");
        }
        return menuLyricDictionary;
    }

    /**
     * This method initializes menuScript   
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuScript() {
        if (menuScript == null) {
            menuScript = new BMenu();
            menuScript.setText("Script");
            menuScript.add(getBMenuItem30());
        }
        return menuScript;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem30() {
        if (menuScriptUpdate == null) {
            menuScriptUpdate = new BMenuItem();
            menuScriptUpdate.setText("Update Script List");
        }
        return menuScriptUpdate;
    }

    /**
     * This method initializes menuSetting  
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuSetting() {
        if (menuSetting == null) {
            menuSetting = new BMenu();
            menuSetting.setText("Setting");
            menuSetting.add(getBMenuItem31());
            menuSetting.add(getMenuSettingSequence());
            menuSetting.add(getMenuSettingPositionQuantize());
            menuSetting.add(getToolStripMenuItem103211());
            menuSetting.add(getMenuSettingGameControler());
            menuSetting.add(getMenuSettingPaletteTool());
            menuSetting.add(getBMenuItem211());
            menuSetting.add(getBMenuItem44());
            menuSetting.add(getToolStripMenuItem103212());
            menuSetting.add(getBMenuItem54());
            menuSetting.add(getBMenuItem64());
        }
        return menuSetting;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem31() {
        if (menuSettingPreference == null) {
            menuSettingPreference = new BMenuItem();
            menuSettingPreference.setText("Preference");
        }
        return menuSettingPreference;
    }

    /**
     * This method initializes menuSettingGameControler 
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuSettingGameControler() {
        if (menuSettingGameControler == null) {
            menuSettingGameControler = new BMenu();
            menuSettingGameControler.setText("Game Controler");
            menuSettingGameControler.add(getMenuSettingGameControlerSetting());
            menuSettingGameControler.add(getMenuSettingGameControlerLoad());
            menuSettingGameControler.add(getMenuSettingGameControlerRemove());
        }
        return menuSettingGameControler;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem211() {
        if (menuSettingShortcut == null) {
            menuSettingShortcut = new BMenuItem();
            menuSettingShortcut.setText("Shortcut Key");
        }
        return menuSettingShortcut;
    }

    /**
     * This method initializes BMenuItem4   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem44() {
        if (menuSettingVibratoPreset == null) {
            menuSettingVibratoPreset = new BMenuItem();
            menuSettingVibratoPreset.setText("Vibrato preset");
        }
        return menuSettingVibratoPreset;
    }

    /**
     * This method initializes toolStripMenuItem103211  
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem103211() {
        if (toolStripMenuItem103211 == null) {
            toolStripMenuItem103211 = new JSeparator();
        }
        return toolStripMenuItem103211;
    }

    /**
     * This method initializes BMenuItem5   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem54() {
        if (menuSettingDefaultSingerStyle == null) {
            menuSettingDefaultSingerStyle = new BMenuItem();
            menuSettingDefaultSingerStyle.setText("Singing Style Defaults");
        }
        return menuSettingDefaultSingerStyle;
    }

    /**
     * This method initializes toolStripMenuItem103212  
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem103212() {
        if (toolStripMenuItem103212 == null) {
            toolStripMenuItem103212 = new JSeparator();
        }
        return toolStripMenuItem103212;
    }

    /**
     * This method initializes menuSettingPositionQuantize  
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuSettingPositionQuantize() {
        if (menuSettingPositionQuantize == null) {
            menuSettingPositionQuantize = new BMenu();
            menuSettingPositionQuantize.setText("Quantize");
            menuSettingPositionQuantize.add(getBMenuItem37());
            menuSettingPositionQuantize.add(getMenuSettingPositionQuantize08());
            menuSettingPositionQuantize.add(getBMenuItem212());
            menuSettingPositionQuantize.add(getBMenuItem38());
            menuSettingPositionQuantize.add(getBMenuItem45());
            menuSettingPositionQuantize.add(getBMenuItem55());
            menuSettingPositionQuantize.add(getBMenuItem65());
            menuSettingPositionQuantize.add(getToolStripMenuItem1032121());
            menuSettingPositionQuantize.add(getBMenuItem74());
        }
        return menuSettingPositionQuantize;
    }

    /**
     * This method initializes BMenuItem6   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem64() {
        if (menuSettingSingerProperty == null) {
            menuSettingSingerProperty = new BMenuItem();
        }
        return menuSettingSingerProperty;
    }

    /**
     * This method initializes menuSettingPaletteTool   
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuSettingPaletteTool() {
        if (menuSettingPaletteTool == null) {
            menuSettingPaletteTool = new BMenu();
            menuSettingPaletteTool.setText("Palette Tool");
        }
        return menuSettingPaletteTool;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem37() {
        if (menuSettingPositionQuantize04 == null) {
            menuSettingPositionQuantize04 = new BMenuItem();
            menuSettingPositionQuantize04.setText("1/4");
        }
        return menuSettingPositionQuantize04;
    }

    /**
     * This method initializes menuSettingPositionQuantize08    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getMenuSettingPositionQuantize08() {
        if (menuSettingPositionQuantize08 == null) {
            menuSettingPositionQuantize08 = new BMenuItem();
            menuSettingPositionQuantize08.setText("1/8");
        }
        return menuSettingPositionQuantize08;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem212() {
        if (menuSettingPositionQuantize16 == null) {
            menuSettingPositionQuantize16 = new BMenuItem();
            menuSettingPositionQuantize16.setText("1/16");
        }
        return menuSettingPositionQuantize16;
    }

    /**
     * This method initializes BMenuItem3   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem38() {
        if (menuSettingPositionQuantize32 == null) {
            menuSettingPositionQuantize32 = new BMenuItem();
            menuSettingPositionQuantize32.setText("1/32");
        }
        return menuSettingPositionQuantize32;
    }

    /**
     * This method initializes BMenuItem4   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem45() {
        if (menuSettingPositionQuantize64 == null) {
            menuSettingPositionQuantize64 = new BMenuItem();
            menuSettingPositionQuantize64.setText("1/64");
        }
        return menuSettingPositionQuantize64;
    }

    /**
     * This method initializes BMenuItem5   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem55() {
        if (menuSettingPositionQuantize128 == null) {
            menuSettingPositionQuantize128 = new BMenuItem();
            menuSettingPositionQuantize128.setText("1/128");
        }
        return menuSettingPositionQuantize128;
    }

    /**
     * This method initializes BMenuItem6   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem65() {
        if (menuSettingPositionQuantizeOff == null) {
            menuSettingPositionQuantizeOff = new BMenuItem();
            menuSettingPositionQuantizeOff.setText("Off");
        }
        return menuSettingPositionQuantizeOff;
    }

    /**
     * This method initializes toolStripMenuItem1032121 
     *  
     * @return javax.swing.JSeparator   
     */
    private JSeparator getToolStripMenuItem1032121() {
        if (toolStripMenuItem1032121 == null) {
            toolStripMenuItem1032121 = new JSeparator();
        }
        return toolStripMenuItem1032121;
    }

    /**
     * This method initializes BMenuItem7   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem74() {
        if (menuSettingPositionQuantizeTriplet == null) {
            menuSettingPositionQuantizeTriplet = new BMenuItem();
            menuSettingPositionQuantizeTriplet.setText("Triplet");
        }
        return menuSettingPositionQuantizeTriplet;
    }

    /**
     * This method initializes menuHelp 
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getMenuHelp() {
        if (menuHelp == null) {
            menuHelp = new BMenu();
            menuHelp.setText("Help");
            menuHelp.add(getBMenuItem39());
            menuHelp.add(getMenuHelpLog());
            menuHelp.add(getMenuHelpDebug());
        }
        return menuHelp;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getBMenuItem39() {
        if (menuHelpAbout == null) {
            menuHelpAbout = new BMenuItem();
            menuHelpAbout.setText("About Cadencii");
        }
        return menuHelpAbout;
    }

    /**
     * This method initializes splitContainer2  
     *  
     * @return javax.swing.BSplitPane   
     */
    private BSplitPane getSplitContainer2() {
        if (splitContainer2 == null) {
            splitContainer2 = new BSplitPane();
            splitContainer2.setDividerSize(9);
            splitContainer2.setDividerLocation(300);
            splitContainer2.setEnabled(false);
            splitContainer2.setResizeWeight(1.0D);
            splitContainer2.setPanel2Hidden(true);
            splitContainer2.setBottomComponent(getPanel2());
            splitContainer2.setTopComponent(getJPanel1());
            splitContainer2.setOrientation(BSplitPane.VERTICAL_SPLIT);
            splitContainer2.setBorder( new EmptyBorder( 0, 0, 0, 0 ) );
        }
        return splitContainer2;
    }

    /**
     * This method initializes panel1   
     *  
     * @return javax.swing.BPanel   
     */
    private BPanel getPanel1() {
        if (panel1 == null) {
            GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
            gridBagConstraints2.gridx = 3;
            gridBagConstraints2.gridheight = 2;
            gridBagConstraints2.fill = GridBagConstraints.VERTICAL;
            gridBagConstraints2.gridy = 0;
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.fill = GridBagConstraints.NONE;
            gridBagConstraints11.gridy = 1;
            gridBagConstraints11.weightx = 0.0D;
            gridBagConstraints11.anchor = GridBagConstraints.EAST;
            gridBagConstraints11.gridx = 2;
            GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
            gridBagConstraints10.gridx = 0;
            gridBagConstraints10.fill = GridBagConstraints.BOTH;
            gridBagConstraints10.gridy = 1;
            GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
            gridBagConstraints9.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints9.gridy = 1;
            gridBagConstraints9.weighty = 0.0D;
            gridBagConstraints9.weightx = 1.0D;
            gridBagConstraints9.gridx = 1;
            GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
            gridBagConstraints7.gridx = 0;
            gridBagConstraints7.fill = GridBagConstraints.BOTH;
            gridBagConstraints7.weightx = 1.0D;
            gridBagConstraints7.weighty = 1.0D;
            gridBagConstraints7.gridwidth = 3;
            gridBagConstraints7.gridheight = 1;
            gridBagConstraints7.gridy = 0;
            panel1 = new BPanel();
            panel1.setLayout(new GridBagLayout());
            panel1.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            panel1.add(getPictPianoRoll(), gridBagConstraints7);
            panel1.add(getPictureBox3(), gridBagConstraints10);
            panel1.add(getHScroll(), gridBagConstraints9);
            panel1.add(getTrackBar(), gridBagConstraints11);
            panel1.add(getJPanel22(), gridBagConstraints2);
        }
        return panel1;
    }

    /**
     * This method initializes panel2   
     *  
     * @return javax.swing.BPanel   
     */
    private BPanel getPanel2() {
        if (panel2 == null) {
            panel2 = new BPanel();
            panel2.setLayout(new GridBagLayout());
            panel2.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
        }
        return panel2;
    }

    /**
     * This method initializes splitContainer1  
     *  
     * @return javax.swing.BSplitPane   
     */
    private BSplitPane getSplitContainer1() {
        if (splitContainer1 == null) {
            splitContainer1 = new BSplitPane();
            splitContainer1.setDividerLocation(300);
            splitContainer1.setResizeWeight(1.0D);
            splitContainer1.setDividerSize(9);
            splitContainer1.setTopComponent(getSplitContainer2());
            splitContainer1.setBottomComponent(getTrackSelector());
            splitContainer1.setOrientation(BSplitPane.VERTICAL_SPLIT);
            splitContainer1.setBorder( new EmptyBorder( 0, 0, 0, 0 ) );
        }
        return splitContainer1;
    }

    /**
     * This method initializes trackSelector    
     *  
     * @return javax.swing.BPanel   
     */
    private TrackSelector getTrackSelector() {
        if (trackSelector == null) {
            trackSelector = new TrackSelector();
            trackSelector.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
        }
        return trackSelector;
    }

    /**
     * This method initializes splitContainerProperty   
     *  
     * @return javax.swing.BSplitPane   
     */
    private BSplitPane getSplitContainerProperty() {
        if (splitContainerProperty == null) {
            splitContainerProperty = new BSplitPane();
            splitContainerProperty.setDividerLocation(0);
            splitContainerProperty.setEnabled(false);
            splitContainerProperty.setDividerSize(0);
            splitContainerProperty.setResizeWeight(1.0D);
            splitContainerProperty.setRightComponent(getSplitContainer1());
            splitContainerProperty.setLeftComponent(getM_property_panel_container());
            splitContainerProperty.setBorder( new EmptyBorder( 0, 0, 0, 0 ) );
        }
        return splitContainerProperty;
    }

    /**
     * This method initializes m_property_panel_container   
     *  
     * @return javax.swing.BPanel   
     */
    private BPanel getM_property_panel_container() {
        if (m_property_panel_container == null) {
            m_property_panel_container = new BPanel();
            m_property_panel_container.setLayout(new GridBagLayout());
            m_property_panel_container.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
        }
        return m_property_panel_container;
    }

    /**
     * This method initializes toolStripFile    
     *  
     * @return javax.swing.BToolBar 
     */
    private BToolBar getToolStripFile() {
        if (toolStripFile == null) {
            toolStripFile = new BToolBar();
            toolStripFile.setName("toolStripFile");
            toolStripFile.add(getStripBtnFileNew());
            toolStripFile.add(getStripBtnFileOpen());
            toolStripFile.add(getStripBtnFileSave());
            toolStripFile.add(getStripBtnCut());
            toolStripFile.add(getStripBtnCopy());
            toolStripFile.add(getStripBtnPaste());
            toolStripFile.add(getStripBtnUndo());
            toolStripFile.add(getStripBtnRedo());
        }
        return toolStripFile;
    }

    /**
     * This method initializes stripBtnFileNew  
     *  
     * @return javax.swing.BButton  
     */
    private BToolBarButton getStripBtnFileNew() {
        if (stripBtnFileNew == null) {
            stripBtnFileNew = new BToolBarButton();
            stripBtnFileNew.setText("");
            stripBtnFileNew.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnFileNew;
    }

    /**
     * This method initializes stripBtnFileOpen 
     *  
     * @return javax.swing.BButton  
     */
    private BToolBarButton getStripBtnFileOpen() {
        if (stripBtnFileOpen == null) {
            stripBtnFileOpen = new BToolBarButton();
            stripBtnFileOpen.setText("");
            stripBtnFileOpen.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnFileOpen;
    }

    /**
     * This method initializes stripBtnFileSave 
     *  
     * @return javax.swing.BButton  
     */
    private BToolBarButton getStripBtnFileSave() {
        if (stripBtnFileSave == null) {
            stripBtnFileSave = new BToolBarButton();
            stripBtnFileSave.setText("");
            stripBtnFileSave.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnFileSave;
    }

    /**
     * This method initializes stripBtnCut  
     *  
     * @return javax.swing.BButton  
     */
    private BToolBarButton getStripBtnCut() {
        if (stripBtnCut == null) {
            stripBtnCut = new BToolBarButton();
            stripBtnCut.setText("");
            stripBtnCut.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnCut;
    }

    /**
     * This method initializes stripBtnCopy 
     *  
     * @return javax.swing.BButton  
     */
    private BToolBarButton getStripBtnCopy() {
        if (stripBtnCopy == null) {
            stripBtnCopy = new BToolBarButton();
            stripBtnCopy.setText("");
            stripBtnCopy.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnCopy;
    }

    /**
     * This method initializes stripBtnPaste    
     *  
     * @return javax.swing.BButton  
     */
    private BToolBarButton getStripBtnPaste() {
        if (stripBtnPaste == null) {
            stripBtnPaste = new BToolBarButton();
            stripBtnPaste.setText("");
            stripBtnPaste.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnPaste;
    }

    /**
     * This method initializes stripBtnUndo 
     *  
     * @return javax.swing.BButton  
     */
    private BToolBarButton getStripBtnUndo() {
        if (stripBtnUndo == null) {
            stripBtnUndo = new BToolBarButton();
            stripBtnUndo.setText("");
            stripBtnUndo.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnUndo;
    }

    /**
     * This method initializes stripBtnRedo 
     *  
     * @return javax.swing.BButton  
     */
    private BToolBarButton getStripBtnRedo() {
        if (stripBtnRedo == null) {
            stripBtnRedo = new BToolBarButton();
            stripBtnRedo.setText("");
            stripBtnRedo.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnRedo;
    }

    /**
     * This method initializes toolStripPosition    
     *  
     * @return javax.swing.BToolBar 
     */
    private BToolBar getToolStripPosition() {
        if (toolStripPosition == null) {
            toolStripPosition = new BToolBar();
            toolStripPosition.setName("toolStripPosition");
            toolStripPosition.add(getStripBtnMoveTop());
            toolStripPosition.add(getStripBtnRewind());
            toolStripPosition.add(getStripBtnForward());
            toolStripPosition.add(getStripBtnMoveEnd());
            toolStripPosition.add(getStripBtnPlay());
            toolStripPosition.add(getStripBtnScroll());
            toolStripPosition.add(getStripBtnLoop());
        }
        return toolStripPosition;
    }

    /**
     * This method initializes stripBtnMoveTop  
     *  
     * @return javax.swing.BButton  
     */
    private BToolBarButton getStripBtnMoveTop() {
        if (stripBtnMoveTop == null) {
            stripBtnMoveTop = new BToolBarButton();
            stripBtnMoveTop.setText("");
            stripBtnMoveTop.setSize(new Dimension(16, 16));
            stripBtnMoveTop.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnMoveTop;
    }

    /**
     * This method initializes BPanel   
     *  
     * @return javax.swing.BPanel   
     */
    private BPanel getJPanel() {
        if (BPanel == null) {
            GridLayout gridLayout4 = new GridLayout();
            gridLayout4.setRows(2);
            GridLayout gridLayout3 = new GridLayout();
            gridLayout3.setRows(2);
            GridLayout gridLayout2 = new GridLayout();
            gridLayout2.setRows(2);
            GridLayout gridLayout = new GridLayout();
            gridLayout.setRows(2);
            BPanel = new BPanel();
            BPanel.setLayout(new BoxLayout(getJPanel(), BoxLayout.X_AXIS));
            BPanel.add(getToolStripFile(), null);
            BPanel.add(getToolStripPosition(), null);
            BPanel.add(getToolStripTool(), null);
        }
        return BPanel;
    }

    /**
     * This method initializes stripBtnRewind   
     *  
     * @return javax.swing.BButton  
     */
    private BToolBarButton getStripBtnRewind() {
        if (stripBtnRewind == null) {
            stripBtnRewind = new BToolBarButton();
            stripBtnRewind.setText("");
            stripBtnRewind.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnRewind;
    }

    /**
     * This method initializes stripBtnForward  
     *  
     * @return javax.swing.BButton  
     */
    private BToolBarButton getStripBtnForward() {
        if (stripBtnForward == null) {
            stripBtnForward = new BToolBarButton();
            stripBtnForward.setText("");
            stripBtnForward.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnForward;
    }

    /**
     * This method initializes stripBtnMoveEnd  
     *  
     * @return javax.swing.BButton  
     */
    private BToolBarButton getStripBtnMoveEnd() {
        if (stripBtnMoveEnd == null) {
            stripBtnMoveEnd = new BToolBarButton();
            stripBtnMoveEnd.setText("");
            stripBtnMoveEnd.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnMoveEnd;
    }

    /**
     * This method initializes stripBtnPlay 
     *  
     * @return javax.swing.BButton  
     */
    private BToolBarButton getStripBtnPlay() {
        if (stripBtnPlay == null) {
            stripBtnPlay = new BToolBarButton();
            stripBtnPlay.setText("Play");
            stripBtnPlay.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnPlay;
    }

    /**
     * This method initializes stripBtnScroll   
     *  
     * @return javax.swing.BToggleButton    
     */
    private BToolBarButton getStripBtnScroll() {
        if (stripBtnScroll == null) {
            stripBtnScroll = new BToolBarButton();
            stripBtnScroll.setText("");
            stripBtnScroll.setCheckOnClick(true);
            stripBtnScroll.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnScroll;
    }

    /**
     * This method initializes stripBtnLoop 
     *  
     * @return javax.swing.BToggleButton    
     */
    private BToolBarButton getStripBtnLoop() {
        if (stripBtnLoop == null) {
            stripBtnLoop = new BToolBarButton();
            stripBtnLoop.setText("");
            stripBtnLoop.setCheckOnClick(true);
            stripBtnLoop.setPreferredSize(new Dimension(23, 22));
        }
        return stripBtnLoop;
    }

    /**
     * This method initializes toolStripTool    
     *  
     * @return javax.swing.BToolBar 
     */
    private BToolBar getToolStripTool() {
        if (toolStripTool == null) {
            toolStripTool = new BToolBar();
            toolStripTool.add(getStripBtnPointer());
            toolStripTool.add(getStripBtnPencil());
            toolStripTool.add(getStripBtnLine());
            toolStripTool.add(getStripBtnEraser());
            toolStripTool.add(getStripBtnGrid());
            toolStripTool.add(getStripBtnCurve());
            toolStripTool.add(getStripBtnStepSequencer());
        }
        return toolStripTool;
    }

    /**
     * This method initializes stripBtnPointer  
     *  
     * @return javax.swing.BToggleButton    
     */
    private BToolBarButton getStripBtnPointer() {
        if (stripBtnPointer == null) {
            stripBtnPointer = new BToolBarButton();
            stripBtnPointer.setText("Pointer");
            stripBtnPointer.setCheckOnClick(true);
        }
        return stripBtnPointer;
    }

    /**
     * This method initializes stripBtnPencil   
     *  
     * @return javax.swing.BToggleButton    
     */
    private BToolBarButton getStripBtnPencil() {
        if (stripBtnPencil == null) {
            stripBtnPencil = new BToolBarButton();
            stripBtnPencil.setText("Pencil");
            stripBtnPencil.setCheckOnClick(true);
        }
        return stripBtnPencil;
    }

    /**
     * This method initializes stripBtnLine 
     *  
     * @return javax.swing.BToggleButton    
     */
    private BToolBarButton getStripBtnLine() {
        if (stripBtnLine == null) {
            stripBtnLine = new BToolBarButton();
            stripBtnLine.setText("Line");
            stripBtnLine.setCheckOnClick(true);
        }
        return stripBtnLine;
    }

    /**
     * This method initializes stripBtnEraser   
     *  
     * @return javax.swing.BToggleButton    
     */
    private BToolBarButton getStripBtnEraser() {
        if (stripBtnEraser == null) {
            stripBtnEraser = new BToolBarButton();
            stripBtnEraser.setToolTipText("");
            stripBtnEraser.setCheckOnClick(true);
            stripBtnEraser.setText("Eraser");
        }
        return stripBtnEraser;
    }

    /**
     * This method initializes stripBtnGrid 
     *  
     * @return javax.swing.BToggleButton    
     */
    private BToolBarButton getStripBtnGrid() {
        if (stripBtnGrid == null) {
            stripBtnGrid = new BToolBarButton();
            stripBtnGrid.setText("Grid");
            stripBtnGrid.setCheckOnClick(true);
        }
        return stripBtnGrid;
    }

    /**
     * This method initializes stripBtnCurve    
     *  
     * @return javax.swing.BToggleButton    
     */
    private BToolBarButton getStripBtnCurve() {
        if (stripBtnCurve == null) {
            stripBtnCurve = new BToolBarButton();
            stripBtnCurve.setText("Curve");
            stripBtnCurve.setCheckOnClick(true);
        }
        return stripBtnCurve;
    }

    /**
     * This method initializes cMenuTrackSelector   
     *  
     * @return javax.swing.BPopupMenu   
     */
    private BPopupMenu getCMenuTrackSelector() {
        if (cMenuTrackSelector == null) {
            cMenuTrackSelector = new BPopupMenu();
            cMenuTrackSelector.add(getCMenuTrackSelectorPointer());
            cMenuTrackSelector.add(getCMenuTrackSelectorPencil());
            cMenuTrackSelector.add(getCMenuTrackSelectorLine());
            cMenuTrackSelector.add(getCMenuTrackSelectorEraser());
            cMenuTrackSelector.add(getCMenuTrackSelectorPaletteTool());
            cMenuTrackSelector.addSeparator();
            cMenuTrackSelector.add(getCMenuTrackSelectorCurve());
            cMenuTrackSelector.addSeparator();
            cMenuTrackSelector.add(getCMenuTrackSelectorUndo());
            cMenuTrackSelector.add(getCMenuTrackSelectorRedo());
            cMenuTrackSelector.addSeparator();
            cMenuTrackSelector.add(getCMenuTrackSelectorCut());
            cMenuTrackSelector.add(getCMenuTrackSelectorCopy());
            cMenuTrackSelector.add(getCMenuTrackSelectorPaste());
            cMenuTrackSelector.add(getCMenuTrackSelectorDelete());
            cMenuTrackSelector.add(getCMenuTrackSelectorDeleteBezier());
            cMenuTrackSelector.addSeparator();
            cMenuTrackSelector.add(getCMenuTrackSelectorSelectAll());
        }
        return cMenuTrackSelector;
    }

    /**
     * This method initializes cMenuPiano   
     *  
     * @return javax.swing.BPopupMenu   
     */
    private BPopupMenu getCMenuPiano() {
        if (cMenuPiano == null) {
            cMenuPiano = new BPopupMenu();
            cMenuPiano.add(getCMenuPianoPointer());
            cMenuPiano.add(getCMenuPianoPencil());
            cMenuPiano.add(getCMenuPianoEraser());
            cMenuPiano.add(getCMenuPianoPaletteTool());
            cMenuPiano.addSeparator();
            cMenuPiano.add(getCMenuPianoCurve());
            cMenuPiano.addSeparator();
            cMenuPiano.add(getCMenuPianoFixed());
            cMenuPiano.add(getCMenuPianoQuantize());
            cMenuPiano.add(getCMenuPianoGrid());
            cMenuPiano.addSeparator();
            cMenuPiano.add(getCMenuPianoUndo());
            cMenuPiano.add(getCMenuPianoRedo());
            cMenuPiano.addSeparator();
            cMenuPiano.add(getCMenuPianoCut());
            cMenuPiano.add(getCMenuPianoCopy());
            cMenuPiano.add(getCMenuPianoPaste());
            cMenuPiano.add(getCMenuPianoDelete());
            cMenuPiano.addSeparator();
            cMenuPiano.add(getCMenuPianoSelectAll());
            cMenuPiano.add(getCMenuPianoSelectAllEvents());
            cMenuPiano.addSeparator();
            cMenuPiano.add(getCMenuPianoImportLyric());
            cMenuPiano.add(getCMenuPianoExpressionProperty());
            cMenuPiano.add(getCMenuPianoVibratoProperty());
        }
        return cMenuPiano;
    }

    /**
     * This method initializes cMenuTrackTab    
     *  
     * @return javax.swing.BPopupMenu   
     */
    private BPopupMenu getCMenuTrackTab() {
        if (cMenuTrackTab == null) {
            cMenuTrackTab = new BPopupMenu();
            cMenuTrackTab.add(getCMenuTrackTabTrackOn());
            cMenuTrackTab.addSeparator();
            cMenuTrackTab.add(getCMenuTrackTabAdd());
            cMenuTrackTab.add(getCMenuTrackTabCopy());
            cMenuTrackTab.add(getCMenuTrackTabChangeName());
            cMenuTrackTab.add(getCMenuTrackTabDelete());
            cMenuTrackTab.addSeparator();
            cMenuTrackTab.add(getCMenuTrackTabRenderCurrent());
            cMenuTrackTab.add(getCMenuTrackTabRenderAll());
            cMenuTrackTab.addSeparator();
            cMenuTrackTab.add(getCMenuTrackTabOverlay());
            cMenuTrackTab.add(getCMenuTrackTabRenderer());
        }
        return cMenuTrackTab;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackSelectorPointer() {
        if (cMenuTrackSelectorPointer == null) {
            cMenuTrackSelectorPointer = new BMenuItem();
        }
        return cMenuTrackSelectorPointer;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackSelectorPencil() {
        if (cMenuTrackSelectorPencil == null) {
            cMenuTrackSelectorPencil = new BMenuItem();
        }
        return cMenuTrackSelectorPencil;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackSelectorLine() {
        if (cMenuTrackSelectorLine == null) {
            cMenuTrackSelectorLine = new BMenuItem();
        }
        return cMenuTrackSelectorLine;
    }

    /**
     * This method initializes cMenuTrackSelectorEraser 
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackSelectorEraser() {
        if (cMenuTrackSelectorEraser == null) {
            cMenuTrackSelectorEraser = new BMenuItem();
        }
        return cMenuTrackSelectorEraser;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackSelectorPaletteTool() {
        if (cMenuTrackSelectorPaletteTool == null) {
            cMenuTrackSelectorPaletteTool = new BMenuItem();
        }
        return cMenuTrackSelectorPaletteTool;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackSelectorCurve() {
        if (cMenuTrackSelectorCurve == null) {
            cMenuTrackSelectorCurve = new BMenuItem();
        }
        return cMenuTrackSelectorCurve;
    }

    /**
     * This method initializes cMenuTrackSelectorUndo   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackSelectorUndo() {
        if (cMenuTrackSelectorUndo == null) {
            cMenuTrackSelectorUndo = new BMenuItem();
        }
        return cMenuTrackSelectorUndo;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackSelectorRedo() {
        if (cMenuTrackSelectorRedo == null) {
            cMenuTrackSelectorRedo = new BMenuItem();
        }
        return cMenuTrackSelectorRedo;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackSelectorCut() {
        if (cMenuTrackSelectorCut == null) {
            cMenuTrackSelectorCut = new BMenuItem();
        }
        return cMenuTrackSelectorCut;
    }

    /**
     * This method initializes cMenuTrackSelectorCopy   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackSelectorCopy() {
        if (cMenuTrackSelectorCopy == null) {
            cMenuTrackSelectorCopy = new BMenuItem();
        }
        return cMenuTrackSelectorCopy;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackSelectorPaste() {
        if (cMenuTrackSelectorPaste == null) {
            cMenuTrackSelectorPaste = new BMenuItem();
        }
        return cMenuTrackSelectorPaste;
    }

    /**
     * This method initializes BMenuItem3   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackSelectorDelete() {
        if (cMenuTrackSelectorDelete == null) {
            cMenuTrackSelectorDelete = new BMenuItem();
        }
        return cMenuTrackSelectorDelete;
    }

    /**
     * This method initializes BMenuItem4   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackSelectorDeleteBezier() {
        if (cMenuTrackSelectorDeleteBezier == null) {
            cMenuTrackSelectorDeleteBezier = new BMenuItem();
        }
        return cMenuTrackSelectorDeleteBezier;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackSelectorSelectAll() {
        if (cMenuTrackSelectorSelectAll == null) {
            cMenuTrackSelectorSelectAll = new BMenuItem();
        }
        return cMenuTrackSelectorSelectAll;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoPointer() {
        if (cMenuPianoPointer == null) {
            cMenuPianoPointer = new BMenuItem();
        }
        return cMenuPianoPointer;
    }

    /**
     * This method initializes cMenuPianoPencil 
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoPencil() {
        if (cMenuPianoPencil == null) {
            cMenuPianoPencil = new BMenuItem();
        }
        return cMenuPianoPencil;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoEraser() {
        if (cMenuPianoEraser == null) {
            cMenuPianoEraser = new BMenuItem();
        }
        return cMenuPianoEraser;
    }

    /**
     * This method initializes BMenuItem3   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoPaletteTool() {
        if (cMenuPianoPaletteTool == null) {
            cMenuPianoPaletteTool = new BMenuItem();
        }
        return cMenuPianoPaletteTool;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoCurve() {
        if (cMenuPianoCurve == null) {
            cMenuPianoCurve = new BMenuItem();
        }
        return cMenuPianoCurve;
    }

    /**
     * This method initializes cMenuPianoFixed  
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getCMenuPianoFixed() {
        if (cMenuPianoFixed == null) {
            cMenuPianoFixed = new BMenu();
            cMenuPianoFixed.add(getCMenuPianoFixed01());
            cMenuPianoFixed.add(getCMenuPianoFixed02());
            cMenuPianoFixed.add(getCMenuPianoFixed04());
            cMenuPianoFixed.add(getCMenuPianoFixed08());
            cMenuPianoFixed.add(getCMenuPianoFixed16());
            cMenuPianoFixed.add(getCMenuPianoFixed32());
            cMenuPianoFixed.add(getCMenuPianoFixed64());
            cMenuPianoFixed.add(getCMenuPianoFixed128());
            cMenuPianoFixed.add(getCMenuPianoFixedOff());
            cMenuPianoFixed.addSeparator();
            cMenuPianoFixed.add(getCMenuPianoFixedTriplet());
            cMenuPianoFixed.add(getCMenuPianoFixedDotted());
        }
        return cMenuPianoFixed;
    }

    /**
     * This method initializes cMenuPianoQuantize   
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getCMenuPianoQuantize() {
        if (cMenuPianoQuantize == null) {
            cMenuPianoQuantize = new BMenu();
            cMenuPianoQuantize.add(getCMenuPianoQuantize04());
            cMenuPianoQuantize.add(getCMenuPianoQuantize08());
            cMenuPianoQuantize.add(getCMenuPianoQuantize16());
            cMenuPianoQuantize.add(getCMenuPianoQuantize32());
            cMenuPianoQuantize.add(getCMenuPianoQuantize64());
            cMenuPianoQuantize.add(getCMenuPianoQuantize128());
            cMenuPianoQuantize.add(getCMenuPianoQuantizeOff());
            cMenuPianoQuantize.addSeparator();
            cMenuPianoQuantize.add(getCMenuPianoQuantizeTriplet());
        }
        return cMenuPianoQuantize;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoGrid() {
        if (cMenuPianoGrid == null) {
            cMenuPianoGrid = new BMenuItem();
        }
        return cMenuPianoGrid;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoUndo() {
        if (cMenuPianoUndo == null) {
            cMenuPianoUndo = new BMenuItem();
        }
        return cMenuPianoUndo;
    }

    /**
     * This method initializes cMenuPianoRedo   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoRedo() {
        if (cMenuPianoRedo == null) {
            cMenuPianoRedo = new BMenuItem();
        }
        return cMenuPianoRedo;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoCut() {
        if (cMenuPianoCut == null) {
            cMenuPianoCut = new BMenuItem();
        }
        return cMenuPianoCut;
    }

    /**
     * This method initializes cMenuPianoCopy   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoCopy() {
        if (cMenuPianoCopy == null) {
            cMenuPianoCopy = new BMenuItem();
        }
        return cMenuPianoCopy;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoPaste() {
        if (cMenuPianoPaste == null) {
            cMenuPianoPaste = new BMenuItem();
        }
        return cMenuPianoPaste;
    }

    /**
     * This method initializes BMenuItem3   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoDelete() {
        if (cMenuPianoDelete == null) {
            cMenuPianoDelete = new BMenuItem();
        }
        return cMenuPianoDelete;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoSelectAll() {
        if (cMenuPianoSelectAll == null) {
            cMenuPianoSelectAll = new BMenuItem();
        }
        return cMenuPianoSelectAll;
    }

    /**
     * This method initializes cMenuPianoSelectAllEvents    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoSelectAllEvents() {
        if (cMenuPianoSelectAllEvents == null) {
            cMenuPianoSelectAllEvents = new BMenuItem();
        }
        return cMenuPianoSelectAllEvents;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoImportLyric() {
        if (cMenuPianoImportLyric == null) {
            cMenuPianoImportLyric = new BMenuItem();
        }
        return cMenuPianoImportLyric;
    }

    /**
     * This method initializes cMenuPianoExpressionProperty 
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoExpressionProperty() {
        if (cMenuPianoExpressionProperty == null) {
            cMenuPianoExpressionProperty = new BMenuItem();
        }
        return cMenuPianoExpressionProperty;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoVibratoProperty() {
        if (cMenuPianoVibratoProperty == null) {
            cMenuPianoVibratoProperty = new BMenuItem();
        }
        return cMenuPianoVibratoProperty;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoFixed02() {
        if (cMenuPianoFixed02 == null) {
            cMenuPianoFixed02 = new BMenuItem();
            cMenuPianoFixed02.setText("1/2");
        }
        return cMenuPianoFixed02;
    }

    /**
     * This method initializes cMenuPianoFixed04    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoFixed04() {
        if (cMenuPianoFixed04 == null) {
            cMenuPianoFixed04 = new BMenuItem();
            cMenuPianoFixed04.setText("1/4");
        }
        return cMenuPianoFixed04;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoFixed08() {
        if (cMenuPianoFixed08 == null) {
            cMenuPianoFixed08 = new BMenuItem();
            cMenuPianoFixed08.setText("1/8");
        }
        return cMenuPianoFixed08;
    }

    /**
     * This method initializes BMenuItem3   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoFixed01() {
        if (cMenuPianoFixed01 == null) {
            cMenuPianoFixed01 = new BMenuItem();
            cMenuPianoFixed01.setText("1/1");
        }
        return cMenuPianoFixed01;
    }

    /**
     * This method initializes BMenuItem4   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoFixed16() {
        if (cMenuPianoFixed16 == null) {
            cMenuPianoFixed16 = new BMenuItem();
            cMenuPianoFixed16.setText("1/16");
        }
        return cMenuPianoFixed16;
    }

    /**
     * This method initializes BMenuItem5   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoFixed32() {
        if (cMenuPianoFixed32 == null) {
            cMenuPianoFixed32 = new BMenuItem();
            cMenuPianoFixed32.setText("1/32");
        }
        return cMenuPianoFixed32;
    }

    /**
     * This method initializes BMenuItem6   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoFixed64() {
        if (cMenuPianoFixed64 == null) {
            cMenuPianoFixed64 = new BMenuItem();
            cMenuPianoFixed64.setText("1/64");
        }
        return cMenuPianoFixed64;
    }

    /**
     * This method initializes BMenuItem7   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoFixed128() {
        if (cMenuPianoFixed128 == null) {
            cMenuPianoFixed128 = new BMenuItem();
            cMenuPianoFixed128.setText("1/128");
        }
        return cMenuPianoFixed128;
    }

    /**
     * This method initializes BMenuItem8   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoFixedOff() {
        if (cMenuPianoFixedOff == null) {
            cMenuPianoFixedOff = new BMenuItem();
            cMenuPianoFixedOff.setText("Off");
        }
        return cMenuPianoFixedOff;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoFixedTriplet() {
        if (cMenuPianoFixedTriplet == null) {
            cMenuPianoFixedTriplet = new BMenuItem();
            cMenuPianoFixedTriplet.setText("Triplet");
        }
        return cMenuPianoFixedTriplet;
    }

    /**
     * This method initializes cMenuPianoFixedDotted    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoFixedDotted() {
        if (cMenuPianoFixedDotted == null) {
            cMenuPianoFixedDotted = new BMenuItem();
            cMenuPianoFixedDotted.setText("Dot");
        }
        return cMenuPianoFixedDotted;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoQuantize04() {
        if (cMenuPianoQuantize04 == null) {
            cMenuPianoQuantize04 = new BMenuItem();
            cMenuPianoQuantize04.setText("1/4");
        }
        return cMenuPianoQuantize04;
    }

    /**
     * This method initializes cMenuPianoQuantize08 
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoQuantize08() {
        if (cMenuPianoQuantize08 == null) {
            cMenuPianoQuantize08 = new BMenuItem();
            cMenuPianoQuantize08.setText("1/8");
        }
        return cMenuPianoQuantize08;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoQuantize16() {
        if (cMenuPianoQuantize16 == null) {
            cMenuPianoQuantize16 = new BMenuItem();
            cMenuPianoQuantize16.setText("1/16");
        }
        return cMenuPianoQuantize16;
    }

    /**
     * This method initializes BMenuItem3   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoQuantize32() {
        if (cMenuPianoQuantize32 == null) {
            cMenuPianoQuantize32 = new BMenuItem();
            cMenuPianoQuantize32.setToolTipText("");
            cMenuPianoQuantize32.setText("1/32");
        }
        return cMenuPianoQuantize32;
    }

    /**
     * This method initializes BMenuItem4   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoQuantize64() {
        if (cMenuPianoQuantize64 == null) {
            cMenuPianoQuantize64 = new BMenuItem();
            cMenuPianoQuantize64.setText("1/64");
        }
        return cMenuPianoQuantize64;
    }

    /**
     * This method initializes BMenuItem5   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoQuantize128() {
        if (cMenuPianoQuantize128 == null) {
            cMenuPianoQuantize128 = new BMenuItem();
            cMenuPianoQuantize128.setText("1/128");
        }
        return cMenuPianoQuantize128;
    }

    /**
     * This method initializes BMenuItem6   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoQuantizeTriplet() {
        if (cMenuPianoQuantizeTriplet == null) {
            cMenuPianoQuantizeTriplet = new BMenuItem();
            cMenuPianoQuantizeTriplet.setText("Triplet");
        }
        return cMenuPianoQuantizeTriplet;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackTabTrackOn() {
        if (cMenuTrackTabTrackOn == null) {
            cMenuTrackTabTrackOn = new BMenuItem();
            cMenuTrackTabTrackOn.setText("Track On");
        }
        return cMenuTrackTabTrackOn;
    }

    /**
     * This method initializes cMenuTrackTabAdd 
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackTabAdd() {
        if (cMenuTrackTabAdd == null) {
            cMenuTrackTabAdd = new BMenuItem();
        }
        return cMenuTrackTabAdd;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackTabCopy() {
        if (cMenuTrackTabCopy == null) {
            cMenuTrackTabCopy = new BMenuItem();
        }
        return cMenuTrackTabCopy;
    }

    /**
     * This method initializes BMenuItem3   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackTabChangeName() {
        if (cMenuTrackTabChangeName == null) {
            cMenuTrackTabChangeName = new BMenuItem();
        }
        return cMenuTrackTabChangeName;
    }

    /**
     * This method initializes BMenuItem4   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackTabDelete() {
        if (cMenuTrackTabDelete == null) {
            cMenuTrackTabDelete = new BMenuItem();
        }
        return cMenuTrackTabDelete;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackTabRenderCurrent() {
        if (cMenuTrackTabRenderCurrent == null) {
            cMenuTrackTabRenderCurrent = new BMenuItem();
        }
        return cMenuTrackTabRenderCurrent;
    }

    /**
     * This method initializes cMenuTrackTabRenderAll   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackTabRenderAll() {
        if (cMenuTrackTabRenderAll == null) {
            cMenuTrackTabRenderAll = new BMenuItem();
        }
        return cMenuTrackTabRenderAll;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackTabOverlay() {
        if (cMenuTrackTabOverlay == null) {
            cMenuTrackTabOverlay = new BMenuItem();
        }
        return cMenuTrackTabOverlay;
    }

    /**
     * This method initializes cMenuTrackTabRenderer    
     *  
     * @return javax.swing.BMenu    
     */
    private BMenu getCMenuTrackTabRenderer() {
        if (cMenuTrackTabRenderer == null) {
            cMenuTrackTabRenderer = new BMenu();
            cMenuTrackTabRenderer.add(getCMenuTrackTabRendererVOCALOID100());
            cMenuTrackTabRenderer.add(getCMenuTrackTabRendererVOCALOID101());
            cMenuTrackTabRenderer.add(getCMenuTrackTabRendererVOCALOID2());
            cMenuTrackTabRenderer.add(getCMenuTrackTabRendererUtau());
            cMenuTrackTabRenderer.add(getCMenuTrackTabRendererStraight());
            cMenuTrackTabRenderer.add(getCMenuTrackTabRendererAquesTone());
        }
        return cMenuTrackTabRenderer;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackTabRendererVOCALOID2() {
        if (cMenuTrackTabRendererVOCALOID2 == null) {
            cMenuTrackTabRendererVOCALOID2 = new BMenuItem();
            cMenuTrackTabRendererVOCALOID2.setText("VOCALOID2");
        }
        return cMenuTrackTabRendererVOCALOID2;
    }

    /**
     * This method initializes cMenuTrackTabRendererVOCALOID100   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackTabRendererVOCALOID100() {
        if (cMenuTrackTabRendererVOCALOID100 == null) {
            cMenuTrackTabRendererVOCALOID100 = new BMenuItem();
            cMenuTrackTabRendererVOCALOID100.setText("VOCALOID1 [1.0]");
        }
        return cMenuTrackTabRendererVOCALOID100;
    }

    /**
     * This method initializes BMenuItem2   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenu getCMenuTrackTabRendererUtau() {
        if (cMenuTrackTabRendererUtau == null) {
            cMenuTrackTabRendererUtau = new BMenu();
            cMenuTrackTabRendererUtau.setText("UTAU");
        }
        return cMenuTrackTabRendererUtau;
    }

    /**
     * This method initializes BMenuItem3   
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuTrackTabRendererStraight() {
        if (cMenuTrackTabRendererStraight == null) {
            cMenuTrackTabRendererStraight = new BMenuItem();
            cMenuTrackTabRendererStraight.setText("vConnect-STAND");
        }
        return cMenuTrackTabRendererStraight;
    }

    /**
     * This method initializes BMenuItem    
     *  
     * @return javax.swing.BMenuItem    
     */
    private BMenuItem getCMenuPianoQuantizeOff() {
        if (cMenuPianoQuantizeOff == null) {
            cMenuPianoQuantizeOff = new BMenuItem();
            cMenuPianoQuantizeOff.setText("Off");
        }
        return cMenuPianoQuantizeOff;
    }

    /**
     * This method initializes panel3   
     *  
     * @return javax.swing.BPanel   
     */
    private BPanel getPanel3() {
        if (panel3 == null) {
            GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
            gridBagConstraints6.gridx = 0;
            gridBagConstraints6.weightx = 1.0D;
            gridBagConstraints6.gridheight = 1;
            gridBagConstraints6.weighty = 1.0D;
            gridBagConstraints6.fill = GridBagConstraints.BOTH;
            gridBagConstraints6.gridy = 0;
            panel3 = new BPanel();
            panel3.setLayout(new GridBagLayout());
            panel3.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            panel3.add(getPanelOverview(), gridBagConstraints6);
        }
        return panel3;
    }

    /**
     * This method initializes panelOverview  
     *  
     * @return javax.swing.BPanel   
     */
    private PictOverview getPanelOverview() {
        if (panelOverview == null) {
            panelOverview = new PictOverview();
            panelOverview.setLayout(new GridBagLayout());
            panelOverview.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            panelOverview.setBackground(new Color(106, 108, 108));
            panelOverview.setPreferredSize(new Dimension(421, 50));
        }
        return panelOverview;
    }

    /**
     * This method initializes pictPianoRoll    
     *  
     * @return javax.swing.BPanel   
     */
    private PictPianoRoll getPictPianoRoll() {
        if (pictPianoRoll == null) {
            pictPianoRoll = new PictPianoRoll();
            pictPianoRoll.setLayout(new GridBagLayout());
            pictPianoRoll.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
        }
        return pictPianoRoll;
    }

    /**
     * This method initializes vScroll  
     *  
     * @return javax.swing.JScrollBar   
     */
    private BVScrollBar getVScroll() {
        if (vScroll == null) {
            vScroll = new BVScrollBar();
            vScroll.setPreferredSize(new Dimension(17, 96));
        }
        return vScroll;
    }

    /**
     * This method initializes hScroll  
     *  
     * @return javax.swing.JScrollBar   
     */
    private BHScrollBar getHScroll() {
        if (hScroll == null) {
            hScroll = new BHScrollBar();
            hScroll.setPreferredSize(new Dimension(48, 17));
        }
        return hScroll;
    }

    /**
     * This method initializes pictureBox3  
     *  
     * @return javax.swing.BPanel   
     */
    private BPanel getPictureBox3() {
        if (pictureBox3 == null) {
            GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
            gridBagConstraints12.anchor = GridBagConstraints.EAST;
            gridBagConstraints12.gridy = 0;
            gridBagConstraints12.weightx = 1.0D;
            gridBagConstraints12.fill = GridBagConstraints.VERTICAL;
            gridBagConstraints12.insets = new Insets(0, 0, 0, 2);
            gridBagConstraints12.gridx = 0;
            pictureBox3 = new BPanel();
            pictureBox3.setLayout(new GridBagLayout());
            pictureBox3.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            pictureBox3.setPreferredSize(new Dimension(68, 16));
            pictureBox3.setPreferredSize(new Dimension(68, 0));
            pictureBox3.setBackground(Color.lightGray);
            pictureBox3.add(getPictKeyLengthSplitter(), gridBagConstraints12);
        }
        return pictureBox3;
    }

    /**
     * This method initializes trackBar 
     *  
     * @return javax.swing.JSlider  
     */
    private BSlider getTrackBar() {
        if (trackBar == null) {
            trackBar = new BSlider();
            trackBar.setPreferredSize(new Dimension(83, 17));
            trackBar.setMinimum(17);
            trackBar.setMaximum(609);
        }
        return trackBar;
    }

    /**
     * This method initializes pictKeyLengthSplitter 
     *  
     * @return javax.swing.BButton  
     */
    private BButton getPictKeyLengthSplitter() {
        if (pictKeyLengthSplitter == null) {
            pictKeyLengthSplitter = new BButton();
            pictKeyLengthSplitter.setPreferredSize(new Dimension(16, 16));
            pictKeyLengthSplitter.setPreferredSize(new Dimension(16, 16));
        }
        return pictKeyLengthSplitter;
    }

    /**
     * This method initializes picturePositionIndicator 
     *  
     * @return javax.swing.BPanel   
     */
    private BPanel getPicturePositionIndicator() {
        if (picturePositionIndicator == null) {
            picturePositionIndicator = new BPanel();
            picturePositionIndicator.setLayout(new GridBagLayout());
            picturePositionIndicator.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            picturePositionIndicator.setPreferredSize(new Dimension(421, 48));
            picturePositionIndicator.setBackground(new Color(169, 169, 169));
        }
        return picturePositionIndicator;
    }

    /**
     * This method initializes jPanel1  
     *  
     * @return javax.swing.BPanel   
     */
    private BPanel getJPanel1() {
        if (jPanel1 == null) {
            GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
            gridBagConstraints15.gridx = 0;
            gridBagConstraints15.fill = GridBagConstraints.BOTH;
            gridBagConstraints15.weightx = 1.0D;
            gridBagConstraints15.weighty = 1.0D;
            gridBagConstraints15.gridy = 2;
            GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
            gridBagConstraints14.gridx = 0;
            gridBagConstraints14.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints14.weightx = 1.0D;
            gridBagConstraints14.gridy = 1;
            GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
            gridBagConstraints13.gridx = 0;
            gridBagConstraints13.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints13.anchor = GridBagConstraints.NORTH;
            gridBagConstraints13.weightx = 1.0D;
            gridBagConstraints13.gridy = 0;
            jPanel1 = new BPanel();
            jPanel1.setLayout(new GridBagLayout());
            jPanel1.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            jPanel1.add(getPanel3(), gridBagConstraints13);
            jPanel1.add(getPicturePositionIndicator(), gridBagConstraints14);
            jPanel1.add(getPanel1(), gridBagConstraints15);
        }
        return jPanel1;
    }

    /**
     * This method initializes jPanel3  
     *  
     * @return javax.swing.BPanel   
     */
    private BPanel getJPanel3() {
        if (jPanel3 == null) {
            GridBagConstraints gridBagConstraints = new GridBagConstraints();
            gridBagConstraints.insets = new Insets(0, 0, 0, 0);
            gridBagConstraints.gridy = 0;
            gridBagConstraints.ipadx = 0;
            gridBagConstraints.ipady = 0;
            gridBagConstraints.fill = GridBagConstraints.BOTH;
            gridBagConstraints.anchor = GridBagConstraints.WEST;
            gridBagConstraints.weightx = 1.0D;
            gridBagConstraints.gridx = 0;
            statusLabel = new BLabel();
            statusLabel.setText("");
            statusLabel.setPreferredSize(new Dimension(10, 27));
            jPanel3 = new BPanel();
            jPanel3.setPreferredSize(new Dimension(10, 27));
            jPanel3.setLayout(new GridBagLayout());
            jPanel3.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            jPanel3.add(statusLabel, gridBagConstraints);
        }
        return jPanel3;
    }

    /**
     * This method initializes menuHidden	
     * 	
     * @return javax.swing.JMenu	
     */
    private BMenu getMenuHidden() {
        if (menuHidden == null) {
            menuHidden = new BMenu();
            menuHidden.setText("Hidden");
            menuHidden.add(getMenuHiddenEditLyric());
            menuHidden.add(getMenuHiddenEditFlipToolPointerPencil());
            menuHidden.add(getMenuHiddenEditFlipToolPointerEraser());
            menuHidden.add(getMenuHiddenVisualForwardParameter());
            menuHidden.add(getMenuHiddenVisualBackwardParameter());
            menuHidden.add(getMenuHiddenTrackNext());
            menuHidden.add(getMenuHiddenTrackBack());
            menuHidden.add(getMenuHiddenCopy());
            menuHidden.add(getMenuHiddenPaste());
            menuHidden.add(getMenuHiddenCut());
            menuHidden.add(getMenuHiddenSelectForward());
            menuHidden.add(getMenuHiddenSelectBackward());
            menuHidden.add(getMenuHiddenMoveUp());
            menuHidden.add(getMenuHiddenMoveDown());
            menuHidden.add(getMenuHiddenMoveLeft());
            menuHidden.add(getMenuHiddenMoveRight());
            menuHidden.add(getMenuHiddenLengthen());
            menuHidden.add(getMenuHiddenShorten());
            menuHidden.add(getMenuHiddenGoToStartMarker());
            menuHidden.add(getMenuHiddenGoToEndMarker());
            menuHidden.add(getMenuHiddenPlayFromStartMarker());
            menuHidden.add(getMenuHiddenFlipCurveOnPianorollMode());
            menuHidden.add(getMenuHiddenPrintPoToCSV());
        }
        return menuHidden;
    }

    /**
     * This method initializes menuHiddenEditLyric	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuHiddenEditLyric() {
        if (menuHiddenEditLyric == null) {
            menuHiddenEditLyric = new BMenuItem();
            menuHiddenEditLyric.setText("Start Lyric Input");
        }
        return menuHiddenEditLyric;
    }

    /**
     * This method initializes menuHiddenEditFlipToolPointerPencil	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuHiddenEditFlipToolPointerPencil() {
        if (menuHiddenEditFlipToolPointerPencil == null) {
            menuHiddenEditFlipToolPointerPencil = new BMenuItem();
            menuHiddenEditFlipToolPointerPencil.setText("Change Tool Pointer / Pencil");
        }
        return menuHiddenEditFlipToolPointerPencil;
    }

    /**
     * This method initializes menuHiddenEditFlipToolPointerEraser	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuHiddenEditFlipToolPointerEraser() {
        if (menuHiddenEditFlipToolPointerEraser == null) {
            menuHiddenEditFlipToolPointerEraser = new BMenuItem();
            menuHiddenEditFlipToolPointerEraser.setText("Change Tool Pointer/ Eraser");
        }
        return menuHiddenEditFlipToolPointerEraser;
    }

    /**
     * This method initializes menuHiddenVisualForwardParameter	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuHiddenVisualForwardParameter() {
        if (menuHiddenVisualForwardParameter == null) {
            menuHiddenVisualForwardParameter = new BMenuItem();
            menuHiddenVisualForwardParameter.setText("Next Control Curve");
        }
        return menuHiddenVisualForwardParameter;
    }

    /**
     * This method initializes menuHiddenVisualBackwardParameter	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuHiddenVisualBackwardParameter() {
        if (menuHiddenVisualBackwardParameter == null) {
            menuHiddenVisualBackwardParameter = new BMenuItem();
            menuHiddenVisualBackwardParameter.setText("Previous Control Curve");
        }
        return menuHiddenVisualBackwardParameter;
    }

    /**
     * This method initializes menuHiddenTrackNext	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuHiddenTrackNext() {
        if (menuHiddenTrackNext == null) {
            menuHiddenTrackNext = new BMenuItem();
            menuHiddenTrackNext.setText("Next Track");
        }
        return menuHiddenTrackNext;
    }

    /**
     * This method initializes menuHiddenTrackBack	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuHiddenTrackBack() {
        if (menuHiddenTrackBack == null) {
            menuHiddenTrackBack = new BMenuItem();
            menuHiddenTrackBack.setText("Previous Track");
        }
        return menuHiddenTrackBack;
    }

    /**
     * This method initializes menuHiddenCopy	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuHiddenCopy() {
        if (menuHiddenCopy == null) {
            menuHiddenCopy = new BMenuItem();
            menuHiddenCopy.setText("Copy");
        }
        return menuHiddenCopy;
    }

    /**
     * This method initializes menuHiddenPaste	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuHiddenPaste() {
        if (menuHiddenPaste == null) {
            menuHiddenPaste = new BMenuItem();
            menuHiddenPaste.setText("Paste");
        }
        return menuHiddenPaste;
    }

    /**
     * This method initializes menuHiddenCut	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuHiddenCut() {
        if (menuHiddenCut == null) {
            menuHiddenCut = new BMenuItem();
            menuHiddenCut.setText("Cut");
        }
        return menuHiddenCut;
    }

    /**
     * This method initializes menuTrackRendererVOCALOID100	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuTrackRendererVOCALOID100() {
        if (menuTrackRendererVOCALOID100 == null) {
            menuTrackRendererVOCALOID100 = new BMenuItem();
            menuTrackRendererVOCALOID100.setText("VOCALOID1 [1.0]");
        }
        return menuTrackRendererVOCALOID100;
    }

    /**
     * This method initializes menuTrackRendererVOCALOID2	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuTrackRendererVOCALOID2() {
        if (menuTrackRendererVOCALOID2 == null) {
            menuTrackRendererVOCALOID2 = new BMenuItem();
            menuTrackRendererVOCALOID2.setText("VOCALOID2");
        }
        return menuTrackRendererVOCALOID2;
    }

    /**
     * This method initializes menuTrackRendererUtau	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenu getMenuTrackRendererUtau() {
        if (menuTrackRendererUtau == null) {
            menuTrackRendererUtau = new BMenu();
            menuTrackRendererUtau.setText("UTAU");
        }
        return menuTrackRendererUtau;
    }

    /**
     * This method initializes menuTrackRendererVCNT	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuTrackRendererVCNT() {
        if (menuTrackRendererVCNT == null) {
            menuTrackRendererVCNT = new BMenuItem();
            menuTrackRendererVCNT.setText("vConnect-STAND");
        }
        return menuTrackRendererVCNT;
    }

    /**
     * This method initializes menuSettingGameControlerSetting	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuSettingGameControlerSetting() {
        if (menuSettingGameControlerSetting == null) {
            menuSettingGameControlerSetting = new BMenuItem();
            menuSettingGameControlerSetting.setText("Setting(&S)");
        }
        return menuSettingGameControlerSetting;
    }

    /**
     * This method initializes menuSettingGameControlerLoad	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuSettingGameControlerLoad() {
        if (menuSettingGameControlerLoad == null) {
            menuSettingGameControlerLoad = new BMenuItem();
            menuSettingGameControlerLoad.setText("Load(&L)");
        }
        return menuSettingGameControlerLoad;
    }

    /**
     * This method initializes menuSettingGameControlerRemove	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuSettingGameControlerRemove() {
        if (menuSettingGameControlerRemove == null) {
            menuSettingGameControlerRemove = new BMenuItem();
            menuSettingGameControlerRemove.setText("Remove(&R)");
        }
        return menuSettingGameControlerRemove;
    }

    /**
     * This method initializes menuHelpDebug	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getMenuHelpDebug() {
        if (menuHelpDebug == null) {
            menuHelpDebug = new BMenuItem();
        }
        return menuHelpDebug;
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private BPanel getJPanel2() {
        if (pictureBox2 == null) {
            pictureBox2 = new BPanel();
            pictureBox2.setLayout(new GridBagLayout());
            pictureBox2.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            pictureBox2.setPreferredSize(new Dimension(16, 64));
            pictureBox2.setPreferredSize(new Dimension(16, 48));
        }
        return pictureBox2;
    }

    /**
     * This method initializes menuVisualPluginUi	
     * 	
     * @return javax.swing.JMenu	
     */
    private BMenu getMenuVisualPluginUi() {
        if (menuVisualPluginUi == null) {
            menuVisualPluginUi = new BMenu();
            menuVisualPluginUi.setText("VSTi Plugin UI");
            menuVisualPluginUi.add(getMenuVisualPluginUiVocaloid100());
            menuVisualPluginUi.add(getMenuVisualPluginUiVocaloid101());
            menuVisualPluginUi.add(getMenuVisualPluginUiVocaloid2());
            menuVisualPluginUi.add(getMenuVisualPluginUiAquesTone());
        }
        return menuVisualPluginUi;
    }

    /**
     * This method initializes menuVisualPluginUiVocaloid100	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuVisualPluginUiVocaloid100() {
        if (menuVisualPluginUiVocaloid100 == null) {
            menuVisualPluginUiVocaloid100 = new BMenuItem();
            menuVisualPluginUiVocaloid100.setText("VOCALOID1 [1.0]");
        }
        return menuVisualPluginUiVocaloid100;
    }

    /**
     * This method initializes menuVisualPluginUiVocaloid101	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuVisualPluginUiVocaloid101() {
        if (menuVisualPluginUiVocaloid101 == null) {
            menuVisualPluginUiVocaloid101 = new BMenuItem();
            menuVisualPluginUiVocaloid101.setText("VOCALOID1 [1.1]");
        }
        return menuVisualPluginUiVocaloid101;
    }

    /**
     * This method initializes menuVisualPluginUiVocaloid2	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuVisualPluginUiVocaloid2() {
        if (menuVisualPluginUiVocaloid2 == null) {
            menuVisualPluginUiVocaloid2 = new BMenuItem();
            menuVisualPluginUiVocaloid2.setText("VOCALOID2");
            menuVisualPluginUiVocaloid2.setActionCommand("VOCALOID2");
        }
        return menuVisualPluginUiVocaloid2;
    }

    /**
     * This method initializes menuVisualPluginUiAquesTone	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuVisualPluginUiAquesTone() {
        if (menuVisualPluginUiAquesTone == null) {
            menuVisualPluginUiAquesTone = new BMenuItem();
            menuVisualPluginUiAquesTone.setText("AquesTone");
        }
        return menuVisualPluginUiAquesTone;
    }

    /**
     * This method initializes menuHiddenSelectForward	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHiddenSelectForward() {
        if (menuHiddenSelectForward == null) {
            menuHiddenSelectForward = new BMenuItem();
            menuHiddenSelectForward.setText("Select Forward");
        }
        return menuHiddenSelectForward;
    }

    /**
     * This method initializes menuHiddenSelectBackward	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHiddenSelectBackward() {
        if (menuHiddenSelectBackward == null) {
            menuHiddenSelectBackward = new BMenuItem();
            menuHiddenSelectBackward.setText("Select Backward");
        }
        return menuHiddenSelectBackward;
    }

    /**
     * This method initializes menuHiddenMoveUp	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHiddenMoveUp() {
        if (menuHiddenMoveUp == null) {
            menuHiddenMoveUp = new BMenuItem();
            menuHiddenMoveUp.setText("Move Up");
        }
        return menuHiddenMoveUp;
    }

    /**
     * This method initializes menuHiddenMoveDown	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHiddenMoveDown() {
        if (menuHiddenMoveDown == null) {
            menuHiddenMoveDown = new BMenuItem();
            menuHiddenMoveDown.setText("Move Down");
        }
        return menuHiddenMoveDown;
    }

    /**
     * This method initializes menuHiddenMoveLeft	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHiddenMoveLeft() {
        if (menuHiddenMoveLeft == null) {
            menuHiddenMoveLeft = new BMenuItem();
            menuHiddenMoveLeft.setText("Move Left");
        }
        return menuHiddenMoveLeft;
    }

    /**
     * This method initializes menuHiddenMoveRight	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHiddenMoveRight() {
        if (menuHiddenMoveRight == null) {
            menuHiddenMoveRight = new BMenuItem();
            menuHiddenMoveRight.setText("Move Right");
        }
        return menuHiddenMoveRight;
    }

    /**
     * This method initializes menuHiddenLengthen	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHiddenLengthen() {
        if (menuHiddenLengthen == null) {
            menuHiddenLengthen = new BMenuItem();
            menuHiddenLengthen.setText("Lengthen");
        }
        return menuHiddenLengthen;
    }

    /**
     * This method initializes menuHiddenShorten	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHiddenShorten() {
        if (menuHiddenShorten == null) {
            menuHiddenShorten = new BMenuItem();
            menuHiddenShorten.setText("Shorten");
        }
        return menuHiddenShorten;
    }

    /**
     * This method initializes menuHiddenGoToStartMarker	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHiddenGoToStartMarker() {
        if (menuHiddenGoToStartMarker == null) {
            menuHiddenGoToStartMarker = new BMenuItem();
            menuHiddenGoToStartMarker.setText("GoTo Start Marker");
        }
        return menuHiddenGoToStartMarker;
    }

    /**
     * This method initializes menuHiddenGoToEndMarker	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getMenuHiddenGoToEndMarker() {
        if (menuHiddenGoToEndMarker == null) {
            menuHiddenGoToEndMarker = new BMenuItem();
            menuHiddenGoToEndMarker.setText("GoTo End Marker");
        }
        return menuHiddenGoToEndMarker;
    }

    /**
     * This method initializes jPanel2	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel22() {
        if (jPanel2 == null) {
            GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
            gridBagConstraints3.anchor = GridBagConstraints.NORTH;
            gridBagConstraints3.gridheight = 1;
            gridBagConstraints3.gridx = 0;
            gridBagConstraints3.gridy = 1;
            gridBagConstraints3.weighty = 0.0D;
            gridBagConstraints3.fill = GridBagConstraints.BOTH;
            GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
            gridBagConstraints1.fill = GridBagConstraints.VERTICAL;
            gridBagConstraints1.gridx = 0;
            gridBagConstraints1.gridy = 0;
            gridBagConstraints1.weighty = 1.0D;
            gridBagConstraints1.gridheight = 1;
            jPanel2 = new JPanel();
            jPanel2.setLayout(new GridBagLayout());
            jPanel2.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            jPanel2.add(getVScroll(), gridBagConstraints1);
            jPanel2.add(getJPanel2(), gridBagConstraints3);
        }
        return jPanel2;
    }

    /**
     * This method initializes cMenuPositionIndicator	
     * 	
     * @return org.kbinani.windows.forms.BPopupMenu	
     */
    private BPopupMenu getCMenuPositionIndicator() {
        if (cMenuPositionIndicator == null) {
            cMenuPositionIndicator = new BPopupMenu();
            cMenuPositionIndicator.add(getCMenuPositionIndicatorStartMarker());
            cMenuPositionIndicator.add(getCMenuPositionIndicatorEndMarker());
        }
        return cMenuPositionIndicator;
    }

    /**
     * This method initializes cMenuPositionIndicatorStartMarker	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getCMenuPositionIndicatorStartMarker() {
        if (cMenuPositionIndicatorStartMarker == null) {
            cMenuPositionIndicatorStartMarker = new BMenuItem();
            cMenuPositionIndicatorStartMarker.setText("Set start marker");
        }
        return cMenuPositionIndicatorStartMarker;
    }

    /**
     * This method initializes cMenuPositionIndicatorEndMarker	
     * 	
     * @return org.kbinani.windows.forms.BMenuItem	
     */
    private BMenuItem getCMenuPositionIndicatorEndMarker() {
        if (cMenuPositionIndicatorEndMarker == null) {
            cMenuPositionIndicatorEndMarker = new BMenuItem();
            cMenuPositionIndicatorEndMarker.setText("Set end marker");
        }
        return cMenuPositionIndicatorEndMarker;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="18,34"
