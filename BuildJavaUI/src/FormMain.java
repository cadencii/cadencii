import java.awt.BorderLayout;
import java.awt.FlowLayout;
import java.awt.GridBagLayout;
import javax.swing.JButton;
import javax.swing.JComboBox;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.JPanel;
import javax.swing.JSeparator;
import javax.swing.JSplitPane;
import javax.swing.JToggleButton;
import javax.swing.JToolBar;
import java.awt.GridLayout;
import javax.swing.BoxLayout;
import javax.swing.JPopupMenu;
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import javax.swing.JScrollBar;
import javax.swing.JSlider;

public class FormMain extends JFrame {

	private static final long serialVersionUID = 1L;
	private JPanel jContentPane = null;  //  @jve:decl-index=0:visual-constraint="10,55"
	private JMenuBar jJMenuBar = null;
	private JMenu menuFile = null;
	private JMenuItem menuFileNew = null;
	private JMenuItem menuFileOpen = null;
	private JMenuItem menuFileSave = null;
	private JMenuItem menuFileSaveNamed = null;
	private JSeparator toolStripMenuItem10 = null;
	private JMenuItem menuFileOpenVsq = null;
	private JMenuItem menuFileOpenUst = null;
	private JMenu menuFileImport = null;
	private JMenu menuFileExport = null;
	private JSeparator toolStripMenuItem101 = null;
	private JMenuItem menuFileRecent = null;
	private JSeparator toolStripMenuItem102 = null;
	private JMenuItem menuFileQuit = null;
	private JMenuItem menuFileImportVsq = null;
	private JMenuItem menuFileImportMidi = null;
	private JMenuItem menuFileExportWav = null;
	private JMenuItem menuFileExportMidi = null;
	private JMenu menuEdit = null;
	private JMenuItem menuEditUndo = null;
	private JMenuItem menuEditRedo = null;
	private JSeparator toolStripMenuItem103 = null;
	private JMenuItem menuEditCut = null;
	private JMenuItem menuEditCopy = null;
	private JMenuItem menuEditPaste = null;
	private JMenuItem menuEditDelete = null;
	private JSeparator toolStripMenuItem104 = null;
	private JMenuItem menuEditAutoNormalizeMode = null;
	private JSeparator toolStripMenuItem1041 = null;
	private JMenuItem menuEditSelectAll = null;
	private JMenuItem menuEditSelectAllEvents = null;
	private JMenu menuVisual = null;
	private JMenuItem menuVisualControlTrack = null;
	private JMenuItem menuVisualMixer = null;
	private JMenuItem menuVisualWaveform = null;
	private JMenuItem menuVisualProperty = null;
	private JMenuItem menuVisualOverview = null;
	private JSeparator toolStripMenuItem1031 = null;
	private JMenuItem menuVisualGridline = null;
	private JSeparator toolStripMenuItem1032 = null;
	private JMenuItem menuVisualStartMarker = null;
	private JMenuItem menuVisualEndMarker = null;
	private JSeparator toolStripMenuItem1033 = null;
	private JMenuItem menuVisualNoteProperty = null;
	private JMenuItem menuVisualLyrics = null;
	private JMenuItem menuVisualPitchLine = null;
	private JMenu menuJob = null;
	private JMenuItem menuJobNormalize = null;
	private JMenuItem menuJobInsertBar = null;
	private JMenuItem menuJobDeleteBar = null;
	private JMenuItem menuJobRandomize = null;
	private JMenuItem menuJobConnect = null;
	private JMenuItem menuJobLyric = null;
	private JMenuItem menuJobRewire = null;
	private JMenuItem menuJobRealTime = null;
	private JMenuItem menuJobReloadVsti = null;
	private JMenu menuTrack = null;
	private JMenuItem menuTrackOn = null;
	private JSeparator toolStripMenuItem10321 = null;
	private JMenuItem menuTrackAdd = null;
	private JMenuItem menuTrackCopy = null;
	private JMenuItem menuTrackChangeName = null;
	private JMenuItem menuTrackDelete = null;
	private JSeparator toolStripMenuItem10322 = null;
	private JMenuItem menuTrackRenderCurrent = null;
	private JMenuItem menuTrackRenderAll = null;
	private JSeparator toolStripMenuItem10323 = null;
	private JMenuItem menuTrackOverlay = null;
	private JMenu menuTrackRenderer = null;
	private JSeparator toolStripMenuItem10324 = null;
	private JMenu menuTrackBgm = null;
	private JMenuItem menuTrackManager = null;
	private JMenu menuLyric = null;
	private JMenuItem menuLyricExpressionProperty = null;
	private JMenuItem menuLyricVibratoProperty = null;
	private JMenuItem menuLyricSymbol = null;
	private JMenuItem menuLyricDictionary = null;
	private JMenu menuScript = null;
	private JMenuItem menuScriptUpdate = null;
	private JMenu menuSetting = null;
	private JMenuItem menuSettingPreference = null;
	private JMenu menuSettingGameControler = null;
	private JMenuItem menuSettingShortcut = null;
	private JMenuItem menuSettingMidi = null;
	private JMenuItem menuSettingUtauVoiceDB = null;
	private JSeparator toolStripMenuItem103211 = null;
	private JMenuItem menuSettingDefaultSingerStyle = null;
	private JSeparator toolStripMenuItem103212 = null;
	private JMenu menuSettingPositionQuantize = null;
	private JMenuItem menuSettingSingerProperty = null;
	private JMenu menuSettingPaletteTool = null;
	private JMenuItem menuSettingPositionQuantize04 = null;
	private JMenuItem menuSettingPositionQuantize08 = null;
	private JMenuItem menuSettingPositionQuantize16 = null;
	private JMenuItem menuSettingPositionQuantize32 = null;
	private JMenuItem menuSettingPositionQuantize64 = null;
	private JMenuItem menuSettingPositionQuantize128 = null;
	private JMenuItem menuSettingPositionQuantizeOff = null;
	private JSeparator toolStripMenuItem1032121 = null;
	private JMenuItem menuSettingPositionQuantizeTriplet = null;
	private JMenu menuSettingLengthQuantize = null;
	private JMenuItem menuSettingLengthQuantize04 = null;
	private JMenuItem menuSettingLengthQuantize08 = null;
	private JMenuItem menuSettingLengthQuantize16 = null;
	private JMenuItem menuSettingLengthQuantize32 = null;
	private JMenuItem menuSettingLengthQuantize64 = null;
	private JMenuItem menuSettingLengthQuantize128 = null;
	private JMenuItem menuSettingLengthQuantizeOff = null;
	private JSeparator toolStripMenuItem10321211 = null;
	private JMenuItem menuSettingLengthQuantizeTriplet = null;
	private JMenu menuHelp = null;
	private JMenuItem menuHelpAbout = null;
	private JSplitPane splitContainer2 = null;
	private JPanel panel1 = null;
	private JPanel panel2 = null;
	private JSplitPane splitContainer1 = null;
	private TrackSelector trackSelector = null;
	private JSplitPane splitContainerProperty = null;
	private JPanel m_property_panel_container = null;
	private JToolBar toolStripFile = null;
	private JToolBar toolStripBottom = null;
	private JButton stripBtnFileNew = null;
	private JButton stripBtnFileOpen = null;
	private JButton stripBtnFileSave = null;
	private JButton stripBtnCut = null;
	private JButton stripBtnCopy = null;
	private JButton stripBtnPaste = null;
	private JButton stripBtnUndo = null;
	private JButton stripBtnRedo = null;
	private JToolBar toolStripPosition = null;
	private JButton stripBtnMoveTop = null;
	private JPanel jPanel = null;
	private JButton stripBtnRewind = null;
	private JButton stripBtnForward = null;
	private JButton stripBtnMoveEnd = null;
	private JButton stripBtnPlay = null;
	private JButton stripBtnStop = null;
	private JToggleButton stripBtnScroll = null;
	private JToggleButton stripBtnLoop = null;
	private JToolBar toolStripMeasure = null;
	private JLabel toolStripLabel5 = null;
	private JLabel stripLblMeasure = null;
	private JComboBox stripDDBtnLength = null;
	private JLabel jLabel = null;
	private JLabel jLabel1 = null;
	private JComboBox stripDDBtnQuantize = null;
	private JToggleButton stripBtnStartMarker = null;
	private JToggleButton stripBtnEndMarker = null;
	private JToolBar toolStripTool = null;
	private JToggleButton stripBtnPointer = null;
	private JToggleButton stripBtnPencil = null;
	private JToggleButton stripBtnLine = null;
	private JToggleButton stripBtnEraser = null;
	private JToggleButton stripBtnGrid = null;
	private JToggleButton stripBtnCurve = null;
	private JLabel toolStripLabel6 = null;
	private JLabel stripLblCursor = null;
	private JLabel toolStripLabel8 = null;
	private JLabel stripLblTempo = null;
	private JLabel jLabel2 = null;
	private JLabel stripLblBeat = null;
	private JLabel jLabel3 = null;
	private JLabel stripLblGameCtrlMode = null;
	private JLabel jLabel4 = null;
	private JLabel stripLblMidiIn = null;
	private JLabel jLabel5 = null;
	private JComboBox stripDDBtnSpeed = null;
	private JPopupMenu cMenuTrackSelector = null;  //  @jve:decl-index=0:visual-constraint="749,73"
	private JPopupMenu cMenuPiano = null;  //  @jve:decl-index=0:visual-constraint="764,258"
	private JPopupMenu cMenuTrackTab = null;  //  @jve:decl-index=0:visual-constraint="752,356"
	private JMenuItem cMenuTrackSelectorPointer = null;
	private JMenuItem cMenuTrackSelectorPencil = null;
	private JMenuItem cMenuTrackSelectorLine = null;
	private JMenuItem cMenuTrackSelectorEraser = null;
	private JMenuItem cMenuTrackSelectorPaletteTool = null;
	private JMenuItem cMenuTrackSelectorCurve = null;
	private JMenuItem cMenuTrackSelectorUndo = null;
	private JMenuItem cMenuTrackSelectorRedo = null;
	private JMenuItem cMenuTrackSelectorCut = null;
	private JMenuItem cMenuTrackSelectorCopy = null;
	private JMenuItem cMenuTrackSelectorPaste = null;
	private JMenuItem cMenuTrackSelectorDelete = null;
	private JMenuItem cMenuTrackSelectorDeleteBezier = null;
	private JMenuItem cMenuTrackSelectorSelectAll = null;
	private JMenuItem cMenuPianoPointer = null;
	private JMenuItem cMenuPianoPencil = null;
	private JMenuItem cMenuPianoEraser = null;
	private JMenuItem cMenuPianoPaletteTool = null;
	private JMenuItem cMenuPianoCurve = null;
	private JMenu cMenuPianoFixed = null;
	private JMenu cMenuPianoQuantize = null;
	private JMenuItem cMenuPianoGrid = null;
	private JMenuItem cMenuPianoUndo = null;
	private JMenuItem cMenuPianoRedo = null;
	private JMenuItem cMenuPianoCut = null;
	private JMenuItem cMenuPianoCopy = null;
	private JMenuItem cMenuPianoPaste = null;
	private JMenuItem cMenuPianoDelete = null;
	private JMenuItem cMenuPianoSelectAll = null;
	private JMenuItem cMenuPianoSelectAllEvents = null;
	private JMenuItem cMenuPianoImportLyric = null;
	private JMenuItem cMenuPianoExpressionProperty = null;
	private JMenuItem cMenuPianoVibratoProperty = null;
	private JMenuItem cMenuPianoFixed02 = null;
	private JMenuItem cMenuPianoFixed04 = null;
	private JMenuItem cMenuPianoFixed08 = null;
	private JMenuItem cMenuPianoFixed01 = null;
	private JMenuItem cMenuPianoFixed16 = null;
	private JMenuItem cMenuPianoFixed32 = null;
	private JMenuItem cMenuPianoFixed64 = null;
	private JMenuItem cMenuPianoFixed128 = null;
	private JMenuItem cMenuPianoFixedOff = null;
	private JMenuItem cMenuPianoFixedTriplet = null;
	private JMenuItem cMenuPianoFixedDotted = null;
	private JMenuItem cMenuPianoQuantize04 = null;
	private JMenuItem cMenuPianoQuantize08 = null;
	private JMenuItem cMenuPianoQuantize16 = null;
	private JMenuItem cMenuPianoQuantize32 = null;
	private JMenuItem cMenuPianoQuantize64 = null;
	private JMenuItem cMenuPianoQuantize128 = null;
	private JMenuItem cMenuPianoQuantizeTriplet = null;
	private JMenu cMenuPianoLength = null;
	private JMenuItem cMenuPianoLength04 = null;
	private JMenuItem cMenuPianoLength08 = null;
	private JMenuItem cMenuPianoLength16 = null;
	private JMenuItem cMenuPianoLength32 = null;
	private JMenuItem cMenuPianoLength64 = null;
	private JMenuItem cMenuPianoLength128 = null;
	private JMenuItem cMenuPianoLengthTriplet = null;
	private JMenuItem cMenuTrackTabTrackOn = null;
	private JMenuItem cMenuTrackTabAdd = null;
	private JMenuItem cMenuTrackTabCopy = null;
	private JMenuItem cMenuTrackTabChangeName = null;
	private JMenuItem cMenuTrackTabDelete = null;
	private JMenuItem cMenuTrackTabRenderCurrent = null;
	private JMenuItem cMenuTrackTabRenderAll = null;
	private JMenuItem cMenuTrackTabOverlay = null;
	private JMenu cMenuTrackTabRenderer = null;
	private JMenuItem cMenuTrackTabRendererVOCALOID2 = null;
	private JMenuItem cMenuTrackTabRendererVOCALOID1 = null;
	private JMenuItem cMenuTrackTabRendererUtau = null;
	private JMenuItem cMenuTrackTabRendererStraight = null;
	private JMenuItem cMenuPianoQuantizeOff = null;
	private JMenuItem cMenuPianoLengthOff = null;
	private JPanel panel3 = null;
	private JPanel jPanel2 = null;
	private JButton jButton = null;
	private JButton jButton1 = null;
	private JButton jButton2 = null;
	private JButton jButton3 = null;
	private JButton jButton4 = null;
	private JButton jButton5 = null;
	private PictPianoRoll pictPianoRoll = null;
	private JScrollBar vScroll = null;
	private JScrollBar hScroll = null;
	private JPanel pictureBox3 = null;
	private JSlider trackBar = null;
	private JButton jButton6 = null;
	private JPanel picturePositionIndicator = null;
	private JPanel jPanel1 = null;
	private JPanel jPanel3 = null;
	private JLabel statusLabel = null;
	/**
	 * This is the default constructor
	 */
	public FormMain() {
		super();
		initialize();
	}

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		this.setSize(711, 591);
		this.setJMenuBar(getJJMenuBar());
		this.setContentPane( this.getJContentPane());
		this.setTitle("JFrame");
	}

	/**
	 * This method initializes jContentPane
	 * 
	 * @return javax.swing.JPanel
	 */
	private JPanel getJContentPane() {
		if (jContentPane == null) {
			jContentPane = new JPanel();
			jContentPane.setLayout(new BorderLayout());
			jContentPane.setSize(new Dimension(582, 431));
			jContentPane.add(getJPanel(), BorderLayout.NORTH);
			jContentPane.add(getSplitContainerProperty(), BorderLayout.CENTER);
			jContentPane.add(getJPanel3(), BorderLayout.SOUTH);
		}
		return jContentPane;
	}

	/**
	 * This method initializes jJMenuBar	
	 * 	
	 * @return javax.swing.JMenuBar	
	 */
	private JMenuBar getJJMenuBar() {
		if (jJMenuBar == null) {
			jJMenuBar = new JMenuBar();
			jJMenuBar.add(getMenuFile());
			jJMenuBar.add(getMenuEdit());
			jJMenuBar.add(getMenuVisual());
			jJMenuBar.add(getMenuJob());
			jJMenuBar.add(getMenuTrack());
			jJMenuBar.add(getMenuLyric());
			jJMenuBar.add(getMenuScript());
			jJMenuBar.add(getMenuSetting());
			jJMenuBar.add(getMenuHelp());
		}
		return jJMenuBar;
	}

	/**
	 * This method initializes menuFile	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuFile() {
		if (menuFile == null) {
			menuFile = new JMenu();
			menuFile.setText("File");
			menuFile.add(getMenuFileNew());
			menuFile.add(getMenuFileOpen());
			menuFile.add(getMenuFileSave());
			menuFile.add(getMenuFileSaveNamed());
			menuFile.add(getJMenuItem());
			menuFile.add(getJMenuItem2());
			menuFile.add(getJMenuItem3());
			menuFile.add(getMenuFileImport());
			menuFile.add(getMenuFileExport());
			menuFile.add(getToolStripMenuItem101());
			menuFile.add(getJMenuItem4());
			menuFile.add(getToolStripMenuItem102());
			menuFile.add(getJMenuItem5());
		}
		return menuFile;
	}

	/**
	 * This method initializes menuFileNew	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuFileNew() {
		if (menuFileNew == null) {
			menuFileNew = new JMenuItem();
			menuFileNew.setText("New");
		}
		return menuFileNew;
	}

	/**
	 * This method initializes menuFileOpen	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuFileOpen() {
		if (menuFileOpen == null) {
			menuFileOpen = new JMenuItem();
			menuFileOpen.setText("Open");
		}
		return menuFileOpen;
	}

	/**
	 * This method initializes menuFileSave	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuFileSave() {
		if (menuFileSave == null) {
			menuFileSave = new JMenuItem();
			menuFileSave.setText("Save");
		}
		return menuFileSave;
	}

	/**
	 * This method initializes menuFileSaveNamed	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuFileSaveNamed() {
		if (menuFileSaveNamed == null) {
			menuFileSaveNamed = new JMenuItem();
			menuFileSaveNamed.setText("Save As");
		}
		return menuFileSaveNamed;
	}

	/**
	 * This method initializes toolStripMenuItem10	
	 * 	
	 * @return javax.swing.JSeparator	
	 */
	private JSeparator getJMenuItem() {
		if (toolStripMenuItem10 == null) {
			toolStripMenuItem10 = new JSeparator();
		}
		return toolStripMenuItem10;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem2() {
		if (menuFileOpenVsq == null) {
			menuFileOpenVsq = new JMenuItem();
			menuFileOpenVsq.setText("Open VSQ/Vocaloid Midi");
		}
		return menuFileOpenVsq;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem3() {
		if (menuFileOpenUst == null) {
			menuFileOpenUst = new JMenuItem();
			menuFileOpenUst.setText("Open UTAU Project File");
		}
		return menuFileOpenUst;
	}

	/**
	 * This method initializes menuFileImport	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuFileImport() {
		if (menuFileImport == null) {
			menuFileImport = new JMenu();
			menuFileImport.setText("Import");
			menuFileImport.add(getJMenuItem6());
			menuFileImport.add(getJMenuItem7());
		}
		return menuFileImport;
	}

	/**
	 * This method initializes menuFileExport	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuFileExport() {
		if (menuFileExport == null) {
			menuFileExport = new JMenu();
			menuFileExport.setText("Export");
			menuFileExport.add(getJMenuItem8());
			menuFileExport.add(getJMenuItem9());
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
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem4() {
		if (menuFileRecent == null) {
			menuFileRecent = new JMenuItem();
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
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem5() {
		if (menuFileQuit == null) {
			menuFileQuit = new JMenuItem();
			menuFileQuit.setText("Quit");
		}
		return menuFileQuit;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem6() {
		if (menuFileImportVsq == null) {
			menuFileImportVsq = new JMenuItem();
			menuFileImportVsq.setText("VSQ / Vocaloid MIDI");
		}
		return menuFileImportVsq;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem7() {
		if (menuFileImportMidi == null) {
			menuFileImportMidi = new JMenuItem();
			menuFileImportMidi.setText("Standard MIDI");
		}
		return menuFileImportMidi;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem8() {
		if (menuFileExportWav == null) {
			menuFileExportWav = new JMenuItem();
			menuFileExportWav.setText("Wave");
		}
		return menuFileExportWav;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem9() {
		if (menuFileExportMidi == null) {
			menuFileExportMidi = new JMenuItem();
			menuFileExportMidi.setText("MIDI");
		}
		return menuFileExportMidi;
	}

	/**
	 * This method initializes menuEdit	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuEdit() {
		if (menuEdit == null) {
			menuEdit = new JMenu();
			menuEdit.setText("Edit");
			menuEdit.add(getJMenuItem10());
			menuEdit.add(getJMenuItem11());
			menuEdit.add(getToolStripMenuItem103());
			menuEdit.add(getJMenuItem12());
			menuEdit.add(getMenuEditCopy());
			menuEdit.add(getJMenuItem22());
			menuEdit.add(getJMenuItem13());
			menuEdit.add(getToolStripMenuItem104());
			menuEdit.add(getJMenuItem14());
			menuEdit.add(getToolStripMenuItem1041());
			menuEdit.add(getJMenuItem15());
			menuEdit.add(getMenuEditSelectAllEvents());
		}
		return menuEdit;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem10() {
		if (menuEditUndo == null) {
			menuEditUndo = new JMenuItem();
			menuEditUndo.setText("Undo");
		}
		return menuEditUndo;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem11() {
		if (menuEditRedo == null) {
			menuEditRedo = new JMenuItem();
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
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem12() {
		if (menuEditCut == null) {
			menuEditCut = new JMenuItem();
			menuEditCut.setText("Cut");
		}
		return menuEditCut;
	}

	/**
	 * This method initializes menuEditCopy	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuEditCopy() {
		if (menuEditCopy == null) {
			menuEditCopy = new JMenuItem();
			menuEditCopy.setText("Copy");
		}
		return menuEditCopy;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem22() {
		if (menuEditPaste == null) {
			menuEditPaste = new JMenuItem();
			menuEditPaste.setText("Paste");
		}
		return menuEditPaste;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem13() {
		if (menuEditDelete == null) {
			menuEditDelete = new JMenuItem();
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
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem14() {
		if (menuEditAutoNormalizeMode == null) {
			menuEditAutoNormalizeMode = new JMenuItem();
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
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem15() {
		if (menuEditSelectAll == null) {
			menuEditSelectAll = new JMenuItem();
			menuEditSelectAll.setText("Select All");
		}
		return menuEditSelectAll;
	}

	/**
	 * This method initializes menuEditSelectAllEvents	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuEditSelectAllEvents() {
		if (menuEditSelectAllEvents == null) {
			menuEditSelectAllEvents = new JMenuItem();
			menuEditSelectAllEvents.setText("Select All Events");
		}
		return menuEditSelectAllEvents;
	}

	/**
	 * This method initializes menuVisual	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuVisual() {
		if (menuVisual == null) {
			menuVisual = new JMenu();
			menuVisual.setText("Visual");
			menuVisual.add(getJMenuItem16());
			menuVisual.add(getJMenuItem17());
			menuVisual.add(getMenuVisualWaveform());
			menuVisual.add(getJMenuItem23());
			menuVisual.add(getJMenuItem32());
			menuVisual.add(getToolStripMenuItem1031());
			menuVisual.add(getJMenuItem18());
			menuVisual.add(getToolStripMenuItem1032());
			menuVisual.add(getJMenuItem19());
			menuVisual.add(getMenuVisualEndMarker());
			menuVisual.add(getToolStripMenuItem1033());
			menuVisual.add(getMenuVisualLyrics());
			menuVisual.add(getJMenuItem20());
			menuVisual.add(getJMenuItem24());
		}
		return menuVisual;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem16() {
		if (menuVisualControlTrack == null) {
			menuVisualControlTrack = new JMenuItem();
			menuVisualControlTrack.setText("Control Track");
		}
		return menuVisualControlTrack;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem17() {
		if (menuVisualMixer == null) {
			menuVisualMixer = new JMenuItem();
			menuVisualMixer.setText("Mixer");
		}
		return menuVisualMixer;
	}

	/**
	 * This method initializes menuVisualWaveform	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuVisualWaveform() {
		if (menuVisualWaveform == null) {
			menuVisualWaveform = new JMenuItem();
			menuVisualWaveform.setText("Waveform");
		}
		return menuVisualWaveform;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem23() {
		if (menuVisualProperty == null) {
			menuVisualProperty = new JMenuItem();
			menuVisualProperty.setText("Property Window");
		}
		return menuVisualProperty;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem32() {
		if (menuVisualOverview == null) {
			menuVisualOverview = new JMenuItem();
			menuVisualOverview.setText("Navigation");
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
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem18() {
		if (menuVisualGridline == null) {
			menuVisualGridline = new JMenuItem();
			menuVisualGridline.setText("Grid Line");
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
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem19() {
		if (menuVisualStartMarker == null) {
			menuVisualStartMarker = new JMenuItem();
			menuVisualStartMarker.setText("Start Marker");
		}
		return menuVisualStartMarker;
	}

	/**
	 * This method initializes menuVisualEndMarker	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuVisualEndMarker() {
		if (menuVisualEndMarker == null) {
			menuVisualEndMarker = new JMenuItem();
			menuVisualEndMarker.setText("End Marker");
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
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem20() {
		if (menuVisualNoteProperty == null) {
			menuVisualNoteProperty = new JMenuItem();
			menuVisualNoteProperty.setText("Note Expression/Vibrato");
		}
		return menuVisualNoteProperty;
	}

	/**
	 * This method initializes menuVisualLyrics	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuVisualLyrics() {
		if (menuVisualLyrics == null) {
			menuVisualLyrics = new JMenuItem();
			menuVisualLyrics.setText("Lyrics/Phoneme");
		}
		return menuVisualLyrics;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem24() {
		if (menuVisualPitchLine == null) {
			menuVisualPitchLine = new JMenuItem();
			menuVisualPitchLine.setText("Pitch Line");
		}
		return menuVisualPitchLine;
	}

	/**
	 * This method initializes menuJob	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuJob() {
		if (menuJob == null) {
			menuJob = new JMenu();
			menuJob.setText("Job");
			menuJob.add(getJMenuItem21());
			menuJob.add(getMenuJobInsertBar());
			menuJob.add(getJMenuItem25());
			menuJob.add(getJMenuItem33());
			menuJob.add(getJMenuItem42());
			menuJob.add(getJMenuItem52());
			menuJob.add(getJMenuItem62());
			menuJob.add(getJMenuItem72());
			menuJob.add(getJMenuItem26());
		}
		return menuJob;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem21() {
		if (menuJobNormalize == null) {
			menuJobNormalize = new JMenuItem();
			menuJobNormalize.setText("Normalize Notes");
		}
		return menuJobNormalize;
	}

	/**
	 * This method initializes menuJobInsertBar	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuJobInsertBar() {
		if (menuJobInsertBar == null) {
			menuJobInsertBar = new JMenuItem();
			menuJobInsertBar.setText("Insert Bars");
		}
		return menuJobInsertBar;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem25() {
		if (menuJobDeleteBar == null) {
			menuJobDeleteBar = new JMenuItem();
			menuJobDeleteBar.setText("Delete Bars");
		}
		return menuJobDeleteBar;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem33() {
		if (menuJobRandomize == null) {
			menuJobRandomize = new JMenuItem();
			menuJobRandomize.setText("Randomize");
		}
		return menuJobRandomize;
	}

	/**
	 * This method initializes jMenuItem4	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem42() {
		if (menuJobConnect == null) {
			menuJobConnect = new JMenuItem();
			menuJobConnect.setText("Connect Notes");
		}
		return menuJobConnect;
	}

	/**
	 * This method initializes jMenuItem5	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem52() {
		if (menuJobLyric == null) {
			menuJobLyric = new JMenuItem();
			menuJobLyric.setText("Insert Lyrics");
		}
		return menuJobLyric;
	}

	/**
	 * This method initializes jMenuItem6	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem62() {
		if (menuJobRewire == null) {
			menuJobRewire = new JMenuItem();
			menuJobRewire.setText("Import ReWire Host Tempo");
		}
		return menuJobRewire;
	}

	/**
	 * This method initializes jMenuItem7	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem72() {
		if (menuJobRealTime == null) {
			menuJobRealTime = new JMenuItem();
			menuJobRealTime.setText("Start Realtime Input");
		}
		return menuJobRealTime;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem26() {
		if (menuJobReloadVsti == null) {
			menuJobReloadVsti = new JMenuItem();
			menuJobReloadVsti.setText("Reload VSTi");
		}
		return menuJobReloadVsti;
	}

	/**
	 * This method initializes menuTrack	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuTrack() {
		if (menuTrack == null) {
			menuTrack = new JMenu();
			menuTrack.setText("Track");
			menuTrack.add(getJMenuItem27());
			menuTrack.add(getToolStripMenuItem10321());
			menuTrack.add(getMenuTrackAdd());
			menuTrack.add(getJMenuItem28());
			menuTrack.add(getJMenuItem34());
			menuTrack.add(getJMenuItem43());
			menuTrack.add(getToolStripMenuItem10322());
			menuTrack.add(getJMenuItem53());
			menuTrack.add(getJMenuItem63());
			menuTrack.add(getToolStripMenuItem10323());
			menuTrack.add(getJMenuItem73());
			menuTrack.add(getMenuTrackRenderer());
			menuTrack.add(getToolStripMenuItem10324());
			menuTrack.add(getMenuTrackBgm());
			menuTrack.add(getJMenuItem82());
		}
		return menuTrack;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem27() {
		if (menuTrackOn == null) {
			menuTrackOn = new JMenuItem();
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
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuTrackAdd() {
		if (menuTrackAdd == null) {
			menuTrackAdd = new JMenuItem();
			menuTrackAdd.setText("Add Track");
		}
		return menuTrackAdd;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem28() {
		if (menuTrackCopy == null) {
			menuTrackCopy = new JMenuItem();
			menuTrackCopy.setText("Copy Track");
		}
		return menuTrackCopy;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem34() {
		if (menuTrackChangeName == null) {
			menuTrackChangeName = new JMenuItem();
			menuTrackChangeName.setText("Rename Track");
		}
		return menuTrackChangeName;
	}

	/**
	 * This method initializes jMenuItem4	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem43() {
		if (menuTrackDelete == null) {
			menuTrackDelete = new JMenuItem();
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
	 * This method initializes jMenuItem5	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem53() {
		if (menuTrackRenderCurrent == null) {
			menuTrackRenderCurrent = new JMenuItem();
			menuTrackRenderCurrent.setText("Render Current Track");
		}
		return menuTrackRenderCurrent;
	}

	/**
	 * This method initializes jMenuItem6	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem63() {
		if (menuTrackRenderAll == null) {
			menuTrackRenderAll = new JMenuItem();
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
	 * This method initializes jMenuItem7	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem73() {
		if (menuTrackOverlay == null) {
			menuTrackOverlay = new JMenuItem();
			menuTrackOverlay.setText("Overlay");
		}
		return menuTrackOverlay;
	}

	/**
	 * This method initializes menuTrackRenderer	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuTrackRenderer() {
		if (menuTrackRenderer == null) {
			menuTrackRenderer = new JMenu();
			menuTrackRenderer.setText("Renderer");
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
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuTrackBgm() {
		if (menuTrackBgm == null) {
			menuTrackBgm = new JMenu();
			menuTrackBgm.setText("BGM");
		}
		return menuTrackBgm;
	}

	/**
	 * This method initializes jMenuItem8	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem82() {
		if (menuTrackManager == null) {
			menuTrackManager = new JMenuItem();
		}
		return menuTrackManager;
	}

	/**
	 * This method initializes menuLyric	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuLyric() {
		if (menuLyric == null) {
			menuLyric = new JMenu();
			menuLyric.setText("Lyrics");
			menuLyric.add(getJMenuItem29());
			menuLyric.add(getMenuLyricVibratoProperty());
			menuLyric.add(getJMenuItem210());
			menuLyric.add(getJMenuItem35());
		}
		return menuLyric;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem29() {
		if (menuLyricExpressionProperty == null) {
			menuLyricExpressionProperty = new JMenuItem();
			menuLyricExpressionProperty.setText("Note Expression Propertry");
		}
		return menuLyricExpressionProperty;
	}

	/**
	 * This method initializes menuLyricVibratoProperty	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuLyricVibratoProperty() {
		if (menuLyricVibratoProperty == null) {
			menuLyricVibratoProperty = new JMenuItem();
			menuLyricVibratoProperty.setText("Note Vibrato Property");
		}
		return menuLyricVibratoProperty;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem210() {
		if (menuLyricSymbol == null) {
			menuLyricSymbol = new JMenuItem();
			menuLyricSymbol.setText("Phoneme Transformation");
		}
		return menuLyricSymbol;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem35() {
		if (menuLyricDictionary == null) {
			menuLyricDictionary = new JMenuItem();
			menuLyricDictionary.setText("User Word Dictionary");
		}
		return menuLyricDictionary;
	}

	/**
	 * This method initializes menuScript	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuScript() {
		if (menuScript == null) {
			menuScript = new JMenu();
			menuScript.setText("Script");
			menuScript.add(getJMenuItem30());
		}
		return menuScript;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem30() {
		if (menuScriptUpdate == null) {
			menuScriptUpdate = new JMenuItem();
			menuScriptUpdate.setText("Update Script List");
		}
		return menuScriptUpdate;
	}

	/**
	 * This method initializes menuSetting	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuSetting() {
		if (menuSetting == null) {
			menuSetting = new JMenu();
			menuSetting.setText("Setting");
			menuSetting.add(getJMenuItem31());
			menuSetting.add(getMenuSettingGameControler());
			menuSetting.add(getMenuSettingPaletteTool());
			menuSetting.add(getJMenuItem211());
			menuSetting.add(getJMenuItem36());
			menuSetting.add(getJMenuItem44());
			menuSetting.add(getToolStripMenuItem103211());
			menuSetting.add(getJMenuItem54());
			menuSetting.add(getToolStripMenuItem103212());
			menuSetting.add(getMenuSettingPositionQuantize());
			menuSetting.add(getMenuSettingLengthQuantize());
			menuSetting.add(getJMenuItem64());
		}
		return menuSetting;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem31() {
		if (menuSettingPreference == null) {
			menuSettingPreference = new JMenuItem();
			menuSettingPreference.setText("Preference");
		}
		return menuSettingPreference;
	}

	/**
	 * This method initializes menuSettingGameControler	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuSettingGameControler() {
		if (menuSettingGameControler == null) {
			menuSettingGameControler = new JMenu();
			menuSettingGameControler.setText("Game Controler");
		}
		return menuSettingGameControler;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem211() {
		if (menuSettingShortcut == null) {
			menuSettingShortcut = new JMenuItem();
			menuSettingShortcut.setText("Shortcut Key");
		}
		return menuSettingShortcut;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem36() {
		if (menuSettingMidi == null) {
			menuSettingMidi = new JMenuItem();
		}
		return menuSettingMidi;
	}

	/**
	 * This method initializes jMenuItem4	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem44() {
		if (menuSettingUtauVoiceDB == null) {
			menuSettingUtauVoiceDB = new JMenuItem();
			menuSettingUtauVoiceDB.setText("UTAU Voice DB");
		}
		return menuSettingUtauVoiceDB;
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
	 * This method initializes jMenuItem5	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem54() {
		if (menuSettingDefaultSingerStyle == null) {
			menuSettingDefaultSingerStyle = new JMenuItem();
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
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuSettingPositionQuantize() {
		if (menuSettingPositionQuantize == null) {
			menuSettingPositionQuantize = new JMenu();
			menuSettingPositionQuantize.setText("Quantize");
			menuSettingPositionQuantize.add(getJMenuItem37());
			menuSettingPositionQuantize.add(getMenuSettingPositionQuantize08());
			menuSettingPositionQuantize.add(getJMenuItem212());
			menuSettingPositionQuantize.add(getJMenuItem38());
			menuSettingPositionQuantize.add(getJMenuItem45());
			menuSettingPositionQuantize.add(getJMenuItem55());
			menuSettingPositionQuantize.add(getJMenuItem65());
			menuSettingPositionQuantize.add(getToolStripMenuItem1032121());
			menuSettingPositionQuantize.add(getJMenuItem74());
		}
		return menuSettingPositionQuantize;
	}

	/**
	 * This method initializes jMenuItem6	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem64() {
		if (menuSettingSingerProperty == null) {
			menuSettingSingerProperty = new JMenuItem();
		}
		return menuSettingSingerProperty;
	}

	/**
	 * This method initializes menuSettingPaletteTool	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuSettingPaletteTool() {
		if (menuSettingPaletteTool == null) {
			menuSettingPaletteTool = new JMenu();
			menuSettingPaletteTool.setText("Palette Tool");
		}
		return menuSettingPaletteTool;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem37() {
		if (menuSettingPositionQuantize04 == null) {
			menuSettingPositionQuantize04 = new JMenuItem();
			menuSettingPositionQuantize04.setText("1/4");
		}
		return menuSettingPositionQuantize04;
	}

	/**
	 * This method initializes menuSettingPositionQuantize08	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuSettingPositionQuantize08() {
		if (menuSettingPositionQuantize08 == null) {
			menuSettingPositionQuantize08 = new JMenuItem();
			menuSettingPositionQuantize08.setText("1/8");
		}
		return menuSettingPositionQuantize08;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem212() {
		if (menuSettingPositionQuantize16 == null) {
			menuSettingPositionQuantize16 = new JMenuItem();
			menuSettingPositionQuantize16.setText("1/16");
		}
		return menuSettingPositionQuantize16;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem38() {
		if (menuSettingPositionQuantize32 == null) {
			menuSettingPositionQuantize32 = new JMenuItem();
			menuSettingPositionQuantize32.setText("1/32");
		}
		return menuSettingPositionQuantize32;
	}

	/**
	 * This method initializes jMenuItem4	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem45() {
		if (menuSettingPositionQuantize64 == null) {
			menuSettingPositionQuantize64 = new JMenuItem();
			menuSettingPositionQuantize64.setText("1/64");
		}
		return menuSettingPositionQuantize64;
	}

	/**
	 * This method initializes jMenuItem5	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem55() {
		if (menuSettingPositionQuantize128 == null) {
			menuSettingPositionQuantize128 = new JMenuItem();
			menuSettingPositionQuantize128.setText("1/128");
		}
		return menuSettingPositionQuantize128;
	}

	/**
	 * This method initializes jMenuItem6	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem65() {
		if (menuSettingPositionQuantizeOff == null) {
			menuSettingPositionQuantizeOff = new JMenuItem();
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
	 * This method initializes jMenuItem7	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem74() {
		if (menuSettingPositionQuantizeTriplet == null) {
			menuSettingPositionQuantizeTriplet = new JMenuItem();
			menuSettingPositionQuantizeTriplet.setText("Triplet");
		}
		return menuSettingPositionQuantizeTriplet;
	}

	/**
	 * This method initializes menuSettingLengthQuantize	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuSettingLengthQuantize() {
		if (menuSettingLengthQuantize == null) {
			menuSettingLengthQuantize = new JMenu();
			menuSettingLengthQuantize.setText("Length");
			menuSettingLengthQuantize.add(getMenuSettingLengthQuantize04());
			menuSettingLengthQuantize.add(getMenuSettingLengthQuantize08());
			menuSettingLengthQuantize.add(getMenuSettingLengthQuantize16());
			menuSettingLengthQuantize.add(getMenuSettingLengthQuantize32());
			menuSettingLengthQuantize.add(getMenuSettingLengthQuantize64());
			menuSettingLengthQuantize.add(getMenuSettingLengthQuantize128());
			menuSettingLengthQuantize.add(getMenuSettingLengthQuantizeOff());
			menuSettingLengthQuantize.add(getToolStripMenuItem10321211());
			menuSettingLengthQuantize.add(getMenuSettingLengthQuantizeTriplet());
		}
		return menuSettingLengthQuantize;
	}

	/**
	 * This method initializes menuSettingLengthQuantize04	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuSettingLengthQuantize04() {
		if (menuSettingLengthQuantize04 == null) {
			menuSettingLengthQuantize04 = new JMenuItem();
			menuSettingLengthQuantize04.setText("1/4");
		}
		return menuSettingLengthQuantize04;
	}

	/**
	 * This method initializes menuSettingLengthQuantize08	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuSettingLengthQuantize08() {
		if (menuSettingLengthQuantize08 == null) {
			menuSettingLengthQuantize08 = new JMenuItem();
			menuSettingLengthQuantize08.setText("1/8");
		}
		return menuSettingLengthQuantize08;
	}

	/**
	 * This method initializes menuSettingLengthQuantize16	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuSettingLengthQuantize16() {
		if (menuSettingLengthQuantize16 == null) {
			menuSettingLengthQuantize16 = new JMenuItem();
			menuSettingLengthQuantize16.setText("1/16");
		}
		return menuSettingLengthQuantize16;
	}

	/**
	 * This method initializes menuSettingLengthQuantize32	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuSettingLengthQuantize32() {
		if (menuSettingLengthQuantize32 == null) {
			menuSettingLengthQuantize32 = new JMenuItem();
			menuSettingLengthQuantize32.setText("1/32");
		}
		return menuSettingLengthQuantize32;
	}

	/**
	 * This method initializes menuSettingLengthQuantize64	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuSettingLengthQuantize64() {
		if (menuSettingLengthQuantize64 == null) {
			menuSettingLengthQuantize64 = new JMenuItem();
			menuSettingLengthQuantize64.setText("1/64");
		}
		return menuSettingLengthQuantize64;
	}

	/**
	 * This method initializes menuSettingLengthQuantize128	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuSettingLengthQuantize128() {
		if (menuSettingLengthQuantize128 == null) {
			menuSettingLengthQuantize128 = new JMenuItem();
			menuSettingLengthQuantize128.setText("1/128");
		}
		return menuSettingLengthQuantize128;
	}

	/**
	 * This method initializes menuSettingLengthQuantizeOff	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuSettingLengthQuantizeOff() {
		if (menuSettingLengthQuantizeOff == null) {
			menuSettingLengthQuantizeOff = new JMenuItem();
			menuSettingLengthQuantizeOff.setText("Off");
		}
		return menuSettingLengthQuantizeOff;
	}

	/**
	 * This method initializes toolStripMenuItem10321211	
	 * 	
	 * @return javax.swing.JSeparator	
	 */
	private JSeparator getToolStripMenuItem10321211() {
		if (toolStripMenuItem10321211 == null) {
			toolStripMenuItem10321211 = new JSeparator();
		}
		return toolStripMenuItem10321211;
	}

	/**
	 * This method initializes menuSettingLengthQuantizeTriplet	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getMenuSettingLengthQuantizeTriplet() {
		if (menuSettingLengthQuantizeTriplet == null) {
			menuSettingLengthQuantizeTriplet = new JMenuItem();
			menuSettingLengthQuantizeTriplet.setText("Triplet");
		}
		return menuSettingLengthQuantizeTriplet;
	}

	/**
	 * This method initializes menuHelp	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getMenuHelp() {
		if (menuHelp == null) {
			menuHelp = new JMenu();
			menuHelp.setText("Help");
			menuHelp.add(getJMenuItem39());
		}
		return menuHelp;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getJMenuItem39() {
		if (menuHelpAbout == null) {
			menuHelpAbout = new JMenuItem();
			menuHelpAbout.setText("About Cadencii");
		}
		return menuHelpAbout;
	}

	/**
	 * This method initializes splitContainer2	
	 * 	
	 * @return javax.swing.JSplitPane	
	 */
	private JSplitPane getSplitContainer2() {
		if (splitContainer2 == null) {
			splitContainer2 = new JSplitPane();
			splitContainer2.setDividerSize(0);
			splitContainer2.setDividerLocation(70);
			splitContainer2.setEnabled(false);
			splitContainer2.setResizeWeight(1.0D);
			splitContainer2.setBottomComponent(getPanel2());
			splitContainer2.setTopComponent(getJPanel1());
			splitContainer2.setOrientation(JSplitPane.VERTICAL_SPLIT);
		}
		return splitContainer2;
	}

	/**
	 * This method initializes panel1	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getPanel1() {
		if (panel1 == null) {
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
			GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
			gridBagConstraints8.fill = GridBagConstraints.VERTICAL;
			gridBagConstraints8.gridy = 0;
			gridBagConstraints8.weighty = 1.0D;
			gridBagConstraints8.gridx = 3;
			GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			gridBagConstraints7.gridx = 0;
			gridBagConstraints7.fill = GridBagConstraints.BOTH;
			gridBagConstraints7.weightx = 1.0D;
			gridBagConstraints7.weighty = 1.0D;
			gridBagConstraints7.gridwidth = 3;
			gridBagConstraints7.gridy = 0;
			panel1 = new JPanel();
			panel1.setLayout(new GridBagLayout());
			panel1.add(getPictPianoRoll(), gridBagConstraints7);
			panel1.add(getVScroll(), gridBagConstraints8);
			panel1.add(getHScroll(), gridBagConstraints9);
			panel1.add(getPictureBox3(), gridBagConstraints10);
			panel1.add(getTrackBar(), gridBagConstraints11);
		}
		return panel1;
	}

	/**
	 * This method initializes panel2	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getPanel2() {
		if (panel2 == null) {
			panel2 = new JPanel();
			panel2.setLayout(new GridBagLayout());
		}
		return panel2;
	}

	/**
	 * This method initializes splitContainer1	
	 * 	
	 * @return javax.swing.JSplitPane	
	 */
	private JSplitPane getSplitContainer1() {
		if (splitContainer1 == null) {
			splitContainer1 = new JSplitPane();
			splitContainer1.setDividerLocation(200);
			splitContainer1.setResizeWeight(1.0D);
			splitContainer1.setTopComponent(getSplitContainer2());
			splitContainer1.setBottomComponent(getTrackSelector());
			splitContainer1.setOrientation(JSplitPane.VERTICAL_SPLIT);
		}
		return splitContainer1;
	}

	/**
	 * This method initializes trackSelector	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private TrackSelector getTrackSelector() {
		if (trackSelector == null) {
			trackSelector = new TrackSelector();
			trackSelector.setLayout(new GridBagLayout());
		}
		return trackSelector;
	}

	/**
	 * This method initializes splitContainerProperty	
	 * 	
	 * @return javax.swing.JSplitPane	
	 */
	private JSplitPane getSplitContainerProperty() {
		if (splitContainerProperty == null) {
			splitContainerProperty = new JSplitPane();
			splitContainerProperty.setDividerLocation(0);
			splitContainerProperty.setEnabled(false);
			splitContainerProperty.setDividerSize(0);
			splitContainerProperty.setRightComponent(getSplitContainer1());
			splitContainerProperty.setLeftComponent(getM_property_panel_container());
		}
		return splitContainerProperty;
	}

	/**
	 * This method initializes m_property_panel_container	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getM_property_panel_container() {
		if (m_property_panel_container == null) {
			m_property_panel_container = new JPanel();
			m_property_panel_container.setLayout(new GridBagLayout());
		}
		return m_property_panel_container;
	}

	/**
	 * This method initializes toolStripFile	
	 * 	
	 * @return javax.swing.JToolBar	
	 */
	private JToolBar getToolStripFile() {
		if (toolStripFile == null) {
			toolStripFile = new JToolBar();
			toolStripFile.setName("toolStripFile");
			toolStripFile.add(getStripBtnFileNew());
			toolStripFile.add(getStripBtnFileOpen());
			toolStripFile.add(getStripBtnFileSave());
			toolStripFile.addSeparator();
			toolStripFile.add(getStripBtnCut());
			toolStripFile.add(getStripBtnCopy());
			toolStripFile.add(getStripBtnPaste());
			toolStripFile.add(getStripBtnUndo());
			toolStripFile.add(getStripBtnRedo());
		}
		return toolStripFile;
	}

	/**
	 * This method initializes toolStripBottom	
	 * 	
	 * @return javax.swing.JToolBar	
	 */
	private JToolBar getToolStripBottom() {
		if (toolStripBottom == null) {
			jLabel5 = new JLabel();
			jLabel5.setText("Speed 1.0x");
			stripLblMidiIn = new JLabel();
			stripLblMidiIn.setText("Disabled");
			jLabel4 = new JLabel();
			jLabel4.setText("MIDI In");
			stripLblGameCtrlMode = new JLabel();
			stripLblGameCtrlMode.setText("Disabled");
			jLabel3 = new JLabel();
			jLabel3.setText("Game Controler");
			stripLblBeat = new JLabel();
			stripLblBeat.setText("4/4");
			jLabel2 = new JLabel();
			jLabel2.setText("BEAT");
			stripLblTempo = new JLabel();
			stripLblTempo.setText("120.00");
			toolStripLabel8 = new JLabel();
			toolStripLabel8.setText("TEMPO");
			stripLblCursor = new JLabel();
			stripLblCursor.setText("0 : 0 : 000");
			toolStripLabel6 = new JLabel();
			toolStripLabel6.setText("CURSOR");
			toolStripBottom = new JToolBar();
			toolStripBottom.add(toolStripLabel6);
			toolStripBottom.add(stripLblCursor);
			toolStripBottom.addSeparator();
			toolStripBottom.add(toolStripLabel8);
			toolStripBottom.add(stripLblTempo);
			toolStripBottom.addSeparator();
			toolStripBottom.add(jLabel2);
			toolStripBottom.add(stripLblBeat);
			toolStripBottom.addSeparator();
			toolStripBottom.add(jLabel3);
			toolStripBottom.add(stripLblGameCtrlMode);
			toolStripBottom.addSeparator();
			toolStripBottom.add(jLabel4);
			toolStripBottom.add(stripLblMidiIn);
			toolStripBottom.add(jLabel5);
			toolStripBottom.add(getStripDDBtnSpeed());
			toolStripBottom.addSeparator();
			
		}
		return toolStripBottom;
	}

	/**
	 * This method initializes stripBtnFileNew	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getStripBtnFileNew() {
		if (stripBtnFileNew == null) {
			stripBtnFileNew = new JButton();
			stripBtnFileNew.setText("New");
		}
		return stripBtnFileNew;
	}

	/**
	 * This method initializes stripBtnFileOpen	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getStripBtnFileOpen() {
		if (stripBtnFileOpen == null) {
			stripBtnFileOpen = new JButton();
			stripBtnFileOpen.setText("Open");
		}
		return stripBtnFileOpen;
	}

	/**
	 * This method initializes stripBtnFileSave	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getStripBtnFileSave() {
		if (stripBtnFileSave == null) {
			stripBtnFileSave = new JButton();
			stripBtnFileSave.setText("Save");
		}
		return stripBtnFileSave;
	}

	/**
	 * This method initializes stripBtnCut	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getStripBtnCut() {
		if (stripBtnCut == null) {
			stripBtnCut = new JButton();
			stripBtnCut.setText("Cut");
		}
		return stripBtnCut;
	}

	/**
	 * This method initializes stripBtnCopy	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getStripBtnCopy() {
		if (stripBtnCopy == null) {
			stripBtnCopy = new JButton();
			stripBtnCopy.setText("Copy");
		}
		return stripBtnCopy;
	}

	/**
	 * This method initializes stripBtnPaste	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getStripBtnPaste() {
		if (stripBtnPaste == null) {
			stripBtnPaste = new JButton();
			stripBtnPaste.setText("Paste");
		}
		return stripBtnPaste;
	}

	/**
	 * This method initializes stripBtnUndo	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getStripBtnUndo() {
		if (stripBtnUndo == null) {
			stripBtnUndo = new JButton();
			stripBtnUndo.setText("Undo");
		}
		return stripBtnUndo;
	}

	/**
	 * This method initializes stripBtnRedo	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getStripBtnRedo() {
		if (stripBtnRedo == null) {
			stripBtnRedo = new JButton();
			stripBtnRedo.setText("Redo");
		}
		return stripBtnRedo;
	}

	/**
	 * This method initializes toolStripPosition	
	 * 	
	 * @return javax.swing.JToolBar	
	 */
	private JToolBar getToolStripPosition() {
		if (toolStripPosition == null) {
			toolStripPosition = new JToolBar();
			toolStripPosition.setName("toolStripPosition");
			toolStripPosition.add(getStripBtnMoveTop());
			toolStripPosition.add(getStripBtnRewind());
			toolStripPosition.add(getStripBtnForward());
			toolStripPosition.add(getStripBtnMoveEnd());
			toolStripPosition.add(getStripBtnPlay());
			toolStripPosition.add(getStripBtnStop());
			toolStripPosition.add(getStripBtnScroll());
			toolStripPosition.add(getStripBtnLoop());
			toolStripPosition.addSeparator();
		}
		return toolStripPosition;
	}

	/**
	 * This method initializes stripBtnMoveTop	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getStripBtnMoveTop() {
		if (stripBtnMoveTop == null) {
			stripBtnMoveTop = new JButton();
			stripBtnMoveTop.setText("|<");
		}
		return stripBtnMoveTop;
	}

	/**
	 * This method initializes jPanel	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel() {
		if (jPanel == null) {
			GridLayout gridLayout4 = new GridLayout();
			gridLayout4.setRows(2);
			GridLayout gridLayout3 = new GridLayout();
			gridLayout3.setRows(2);
			GridLayout gridLayout2 = new GridLayout();
			gridLayout2.setRows(2);
			GridLayout gridLayout = new GridLayout();
			gridLayout.setRows(2);
			jPanel = new JPanel();
			jPanel.setLayout(new BoxLayout(getJPanel(), BoxLayout.X_AXIS));
			jPanel.add(getToolStripFile(), null);
			jPanel.add(getToolStripPosition(), null);
			jPanel.add(getToolStripTool(), null);
			jPanel.add(getToolStripMeasure(), null);
		}
		return jPanel;
	}

	/**
	 * This method initializes stripBtnRewind	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getStripBtnRewind() {
		if (stripBtnRewind == null) {
			stripBtnRewind = new JButton();
			stripBtnRewind.setText("<<");
		}
		return stripBtnRewind;
	}

	/**
	 * This method initializes stripBtnForward	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getStripBtnForward() {
		if (stripBtnForward == null) {
			stripBtnForward = new JButton();
			stripBtnForward.setText(">>");
		}
		return stripBtnForward;
	}

	/**
	 * This method initializes stripBtnMoveEnd	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getStripBtnMoveEnd() {
		if (stripBtnMoveEnd == null) {
			stripBtnMoveEnd = new JButton();
			stripBtnMoveEnd.setText(">|");
		}
		return stripBtnMoveEnd;
	}

	/**
	 * This method initializes stripBtnPlay	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getStripBtnPlay() {
		if (stripBtnPlay == null) {
			stripBtnPlay = new JButton();
			stripBtnPlay.setText(">");
		}
		return stripBtnPlay;
	}

	/**
	 * This method initializes stripBtnStop	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getStripBtnStop() {
		if (stripBtnStop == null) {
			stripBtnStop = new JButton();
			stripBtnStop.setText("[  ]");
		}
		return stripBtnStop;
	}

	/**
	 * This method initializes stripBtnScroll	
	 * 	
	 * @return javax.swing.JToggleButton	
	 */
	private JToggleButton getStripBtnScroll() {
		if (stripBtnScroll == null) {
			stripBtnScroll = new JToggleButton();
			stripBtnScroll.setText("Scroll");
		}
		return stripBtnScroll;
	}

	/**
	 * This method initializes stripBtnLoop	
	 * 	
	 * @return javax.swing.JToggleButton	
	 */
	private JToggleButton getStripBtnLoop() {
		if (stripBtnLoop == null) {
			stripBtnLoop = new JToggleButton();
			stripBtnLoop.setText("Loop");
		}
		return stripBtnLoop;
	}

	/**
	 * This method initializes toolStripMeasure	
	 * 	
	 * @return javax.swing.JToolBar	
	 */
	private JToolBar getToolStripMeasure() {
		if (toolStripMeasure == null) {
			jLabel1 = new JLabel();
			jLabel1.setText("QUANTIZE");
			jLabel = new JLabel();
			jLabel.setText("LENGTH");
			stripLblMeasure = new JLabel();
			stripLblMeasure.setText("0 : 0 : 000");
			toolStripLabel5 = new JLabel();
			toolStripLabel5.setText("MEASURE");
			toolStripMeasure = new JToolBar();
			toolStripMeasure.setName("toolStripMeasure");
			toolStripMeasure.add(toolStripLabel5);
			toolStripMeasure.add(stripLblMeasure);
			toolStripMeasure.add(jLabel);
			toolStripMeasure.add(getStripDDBtnLength());
			toolStripMeasure.add(jLabel1);
			toolStripMeasure.add(getStripDDBtnQuantize());
			toolStripMeasure.add(getStripBtnStartMarker());
			toolStripMeasure.add(getStripBtnEndMarker());
			toolStripMeasure.addSeparator();
		}
		return toolStripMeasure;
	}

	/**
	 * This method initializes stripDDBtnLength	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getStripDDBtnLength() {
		if (stripDDBtnLength == null) {
			stripDDBtnLength = new JComboBox();
		}
		return stripDDBtnLength;
	}

	/**
	 * This method initializes stripDDBtnQuantize	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getStripDDBtnQuantize() {
		if (stripDDBtnQuantize == null) {
			stripDDBtnQuantize = new JComboBox();
		}
		return stripDDBtnQuantize;
	}

	/**
	 * This method initializes stripBtnStartMarker	
	 * 	
	 * @return javax.swing.JToggleButton	
	 */
	private JToggleButton getStripBtnStartMarker() {
		if (stripBtnStartMarker == null) {
			stripBtnStartMarker = new JToggleButton();
		}
		return stripBtnStartMarker;
	}

	/**
	 * This method initializes stripBtnEndMarker	
	 * 	
	 * @return javax.swing.JToggleButton	
	 */
	private JToggleButton getStripBtnEndMarker() {
		if (stripBtnEndMarker == null) {
			stripBtnEndMarker = new JToggleButton();
		}
		return stripBtnEndMarker;
	}

	/**
	 * This method initializes toolStripTool	
	 * 	
	 * @return javax.swing.JToolBar	
	 */
	private JToolBar getToolStripTool() {
		if (toolStripTool == null) {
			toolStripTool = new JToolBar();
			toolStripTool.add(getStripBtnPointer());
			toolStripTool.add(getStripBtnPencil());
			toolStripTool.add(getStripBtnLine());
			toolStripTool.add(getStripBtnEraser());
			toolStripTool.add(getStripBtnGrid());
			toolStripTool.add(getStripBtnCurve());
			toolStripTool.addSeparator();
		}
		return toolStripTool;
	}

	/**
	 * This method initializes stripBtnPointer	
	 * 	
	 * @return javax.swing.JToggleButton	
	 */
	private JToggleButton getStripBtnPointer() {
		if (stripBtnPointer == null) {
			stripBtnPointer = new JToggleButton();
			stripBtnPointer.setText("Pointer");
		}
		return stripBtnPointer;
	}

	/**
	 * This method initializes stripBtnPencil	
	 * 	
	 * @return javax.swing.JToggleButton	
	 */
	private JToggleButton getStripBtnPencil() {
		if (stripBtnPencil == null) {
			stripBtnPencil = new JToggleButton();
			stripBtnPencil.setText("Pencil");
		}
		return stripBtnPencil;
	}

	/**
	 * This method initializes stripBtnLine	
	 * 	
	 * @return javax.swing.JToggleButton	
	 */
	private JToggleButton getStripBtnLine() {
		if (stripBtnLine == null) {
			stripBtnLine = new JToggleButton();
			stripBtnLine.setText("Line");
		}
		return stripBtnLine;
	}

	/**
	 * This method initializes stripBtnEraser	
	 * 	
	 * @return javax.swing.JToggleButton	
	 */
	private JToggleButton getStripBtnEraser() {
		if (stripBtnEraser == null) {
			stripBtnEraser = new JToggleButton();
			stripBtnEraser.setToolTipText("");
			stripBtnEraser.setText("Eraser");
		}
		return stripBtnEraser;
	}

	/**
	 * This method initializes stripBtnGrid	
	 * 	
	 * @return javax.swing.JToggleButton	
	 */
	private JToggleButton getStripBtnGrid() {
		if (stripBtnGrid == null) {
			stripBtnGrid = new JToggleButton();
			stripBtnGrid.setText("Grid");
		}
		return stripBtnGrid;
	}

	/**
	 * This method initializes stripBtnCurve	
	 * 	
	 * @return javax.swing.JToggleButton	
	 */
	private JToggleButton getStripBtnCurve() {
		if (stripBtnCurve == null) {
			stripBtnCurve = new JToggleButton();
			stripBtnCurve.setText("Curve");
		}
		return stripBtnCurve;
	}

	/**
	 * This method initializes stripDDBtnSpeed	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getStripDDBtnSpeed() {
		if (stripDDBtnSpeed == null) {
			stripDDBtnSpeed = new JComboBox();
		}
		return stripDDBtnSpeed;
	}

	/**
	 * This method initializes cMenuTrackSelector	
	 * 	
	 * @return javax.swing.JPopupMenu	
	 */
	private JPopupMenu getCMenuTrackSelector() {
		if (cMenuTrackSelector == null) {
			cMenuTrackSelector = new JPopupMenu();
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
	 * @return javax.swing.JPopupMenu	
	 */
	private JPopupMenu getCMenuPiano() {
		if (cMenuPiano == null) {
			cMenuPiano = new JPopupMenu();
			cMenuPiano.add(getCMenuPianoPointer());
			cMenuPiano.add(getCMenuPianoPencil());
			cMenuPiano.add(getCMenuPianoEraser());
			cMenuPiano.add(getCMenuPianoPaletteTool());
			cMenuPiano.addSeparator();
			cMenuPiano.add(getCMenuPianoCurve());
			cMenuPiano.addSeparator();
			cMenuPiano.add(getCMenuPianoFixed());
			cMenuPiano.add(getCMenuPianoQuantize());
			cMenuPiano.add(getCMenuPianoLength());
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
	 * @return javax.swing.JPopupMenu	
	 */
	private JPopupMenu getCMenuTrackTab() {
		if (cMenuTrackTab == null) {
			cMenuTrackTab = new JPopupMenu();
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
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackSelectorPointer() {
		if (cMenuTrackSelectorPointer == null) {
			cMenuTrackSelectorPointer = new JMenuItem();
		}
		return cMenuTrackSelectorPointer;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackSelectorPencil() {
		if (cMenuTrackSelectorPencil == null) {
			cMenuTrackSelectorPencil = new JMenuItem();
		}
		return cMenuTrackSelectorPencil;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackSelectorLine() {
		if (cMenuTrackSelectorLine == null) {
			cMenuTrackSelectorLine = new JMenuItem();
		}
		return cMenuTrackSelectorLine;
	}

	/**
	 * This method initializes cMenuTrackSelectorEraser	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackSelectorEraser() {
		if (cMenuTrackSelectorEraser == null) {
			cMenuTrackSelectorEraser = new JMenuItem();
		}
		return cMenuTrackSelectorEraser;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackSelectorPaletteTool() {
		if (cMenuTrackSelectorPaletteTool == null) {
			cMenuTrackSelectorPaletteTool = new JMenuItem();
		}
		return cMenuTrackSelectorPaletteTool;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackSelectorCurve() {
		if (cMenuTrackSelectorCurve == null) {
			cMenuTrackSelectorCurve = new JMenuItem();
		}
		return cMenuTrackSelectorCurve;
	}

	/**
	 * This method initializes cMenuTrackSelectorUndo	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackSelectorUndo() {
		if (cMenuTrackSelectorUndo == null) {
			cMenuTrackSelectorUndo = new JMenuItem();
		}
		return cMenuTrackSelectorUndo;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackSelectorRedo() {
		if (cMenuTrackSelectorRedo == null) {
			cMenuTrackSelectorRedo = new JMenuItem();
		}
		return cMenuTrackSelectorRedo;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackSelectorCut() {
		if (cMenuTrackSelectorCut == null) {
			cMenuTrackSelectorCut = new JMenuItem();
		}
		return cMenuTrackSelectorCut;
	}

	/**
	 * This method initializes cMenuTrackSelectorCopy	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackSelectorCopy() {
		if (cMenuTrackSelectorCopy == null) {
			cMenuTrackSelectorCopy = new JMenuItem();
		}
		return cMenuTrackSelectorCopy;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackSelectorPaste() {
		if (cMenuTrackSelectorPaste == null) {
			cMenuTrackSelectorPaste = new JMenuItem();
		}
		return cMenuTrackSelectorPaste;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackSelectorDelete() {
		if (cMenuTrackSelectorDelete == null) {
			cMenuTrackSelectorDelete = new JMenuItem();
		}
		return cMenuTrackSelectorDelete;
	}

	/**
	 * This method initializes jMenuItem4	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackSelectorDeleteBezier() {
		if (cMenuTrackSelectorDeleteBezier == null) {
			cMenuTrackSelectorDeleteBezier = new JMenuItem();
		}
		return cMenuTrackSelectorDeleteBezier;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackSelectorSelectAll() {
		if (cMenuTrackSelectorSelectAll == null) {
			cMenuTrackSelectorSelectAll = new JMenuItem();
		}
		return cMenuTrackSelectorSelectAll;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoPointer() {
		if (cMenuPianoPointer == null) {
			cMenuPianoPointer = new JMenuItem();
		}
		return cMenuPianoPointer;
	}

	/**
	 * This method initializes cMenuPianoPencil	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoPencil() {
		if (cMenuPianoPencil == null) {
			cMenuPianoPencil = new JMenuItem();
		}
		return cMenuPianoPencil;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoEraser() {
		if (cMenuPianoEraser == null) {
			cMenuPianoEraser = new JMenuItem();
		}
		return cMenuPianoEraser;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoPaletteTool() {
		if (cMenuPianoPaletteTool == null) {
			cMenuPianoPaletteTool = new JMenuItem();
		}
		return cMenuPianoPaletteTool;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoCurve() {
		if (cMenuPianoCurve == null) {
			cMenuPianoCurve = new JMenuItem();
		}
		return cMenuPianoCurve;
	}

	/**
	 * This method initializes cMenuPianoFixed	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getCMenuPianoFixed() {
		if (cMenuPianoFixed == null) {
			cMenuPianoFixed = new JMenu();
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
	 * @return javax.swing.JMenu	
	 */
	private JMenu getCMenuPianoQuantize() {
		if (cMenuPianoQuantize == null) {
			cMenuPianoQuantize = new JMenu();
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
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoGrid() {
		if (cMenuPianoGrid == null) {
			cMenuPianoGrid = new JMenuItem();
		}
		return cMenuPianoGrid;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoUndo() {
		if (cMenuPianoUndo == null) {
			cMenuPianoUndo = new JMenuItem();
		}
		return cMenuPianoUndo;
	}

	/**
	 * This method initializes cMenuPianoRedo	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoRedo() {
		if (cMenuPianoRedo == null) {
			cMenuPianoRedo = new JMenuItem();
		}
		return cMenuPianoRedo;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoCut() {
		if (cMenuPianoCut == null) {
			cMenuPianoCut = new JMenuItem();
		}
		return cMenuPianoCut;
	}

	/**
	 * This method initializes cMenuPianoCopy	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoCopy() {
		if (cMenuPianoCopy == null) {
			cMenuPianoCopy = new JMenuItem();
		}
		return cMenuPianoCopy;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoPaste() {
		if (cMenuPianoPaste == null) {
			cMenuPianoPaste = new JMenuItem();
		}
		return cMenuPianoPaste;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoDelete() {
		if (cMenuPianoDelete == null) {
			cMenuPianoDelete = new JMenuItem();
		}
		return cMenuPianoDelete;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoSelectAll() {
		if (cMenuPianoSelectAll == null) {
			cMenuPianoSelectAll = new JMenuItem();
		}
		return cMenuPianoSelectAll;
	}

	/**
	 * This method initializes cMenuPianoSelectAllEvents	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoSelectAllEvents() {
		if (cMenuPianoSelectAllEvents == null) {
			cMenuPianoSelectAllEvents = new JMenuItem();
		}
		return cMenuPianoSelectAllEvents;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoImportLyric() {
		if (cMenuPianoImportLyric == null) {
			cMenuPianoImportLyric = new JMenuItem();
		}
		return cMenuPianoImportLyric;
	}

	/**
	 * This method initializes cMenuPianoExpressionProperty	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoExpressionProperty() {
		if (cMenuPianoExpressionProperty == null) {
			cMenuPianoExpressionProperty = new JMenuItem();
		}
		return cMenuPianoExpressionProperty;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoVibratoProperty() {
		if (cMenuPianoVibratoProperty == null) {
			cMenuPianoVibratoProperty = new JMenuItem();
		}
		return cMenuPianoVibratoProperty;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoFixed02() {
		if (cMenuPianoFixed02 == null) {
			cMenuPianoFixed02 = new JMenuItem();
		}
		return cMenuPianoFixed02;
	}

	/**
	 * This method initializes cMenuPianoFixed04	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoFixed04() {
		if (cMenuPianoFixed04 == null) {
			cMenuPianoFixed04 = new JMenuItem();
		}
		return cMenuPianoFixed04;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoFixed08() {
		if (cMenuPianoFixed08 == null) {
			cMenuPianoFixed08 = new JMenuItem();
		}
		return cMenuPianoFixed08;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoFixed01() {
		if (cMenuPianoFixed01 == null) {
			cMenuPianoFixed01 = new JMenuItem();
		}
		return cMenuPianoFixed01;
	}

	/**
	 * This method initializes jMenuItem4	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoFixed16() {
		if (cMenuPianoFixed16 == null) {
			cMenuPianoFixed16 = new JMenuItem();
		}
		return cMenuPianoFixed16;
	}

	/**
	 * This method initializes jMenuItem5	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoFixed32() {
		if (cMenuPianoFixed32 == null) {
			cMenuPianoFixed32 = new JMenuItem();
		}
		return cMenuPianoFixed32;
	}

	/**
	 * This method initializes jMenuItem6	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoFixed64() {
		if (cMenuPianoFixed64 == null) {
			cMenuPianoFixed64 = new JMenuItem();
		}
		return cMenuPianoFixed64;
	}

	/**
	 * This method initializes jMenuItem7	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoFixed128() {
		if (cMenuPianoFixed128 == null) {
			cMenuPianoFixed128 = new JMenuItem();
		}
		return cMenuPianoFixed128;
	}

	/**
	 * This method initializes jMenuItem8	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoFixedOff() {
		if (cMenuPianoFixedOff == null) {
			cMenuPianoFixedOff = new JMenuItem();
		}
		return cMenuPianoFixedOff;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoFixedTriplet() {
		if (cMenuPianoFixedTriplet == null) {
			cMenuPianoFixedTriplet = new JMenuItem();
		}
		return cMenuPianoFixedTriplet;
	}

	/**
	 * This method initializes cMenuPianoFixedDotted	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoFixedDotted() {
		if (cMenuPianoFixedDotted == null) {
			cMenuPianoFixedDotted = new JMenuItem();
		}
		return cMenuPianoFixedDotted;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoQuantize04() {
		if (cMenuPianoQuantize04 == null) {
			cMenuPianoQuantize04 = new JMenuItem();
		}
		return cMenuPianoQuantize04;
	}

	/**
	 * This method initializes cMenuPianoQuantize08	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoQuantize08() {
		if (cMenuPianoQuantize08 == null) {
			cMenuPianoQuantize08 = new JMenuItem();
		}
		return cMenuPianoQuantize08;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoQuantize16() {
		if (cMenuPianoQuantize16 == null) {
			cMenuPianoQuantize16 = new JMenuItem();
		}
		return cMenuPianoQuantize16;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoQuantize32() {
		if (cMenuPianoQuantize32 == null) {
			cMenuPianoQuantize32 = new JMenuItem();
		}
		return cMenuPianoQuantize32;
	}

	/**
	 * This method initializes jMenuItem4	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoQuantize64() {
		if (cMenuPianoQuantize64 == null) {
			cMenuPianoQuantize64 = new JMenuItem();
		}
		return cMenuPianoQuantize64;
	}

	/**
	 * This method initializes jMenuItem5	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoQuantize128() {
		if (cMenuPianoQuantize128 == null) {
			cMenuPianoQuantize128 = new JMenuItem();
		}
		return cMenuPianoQuantize128;
	}

	/**
	 * This method initializes jMenuItem6	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoQuantizeTriplet() {
		if (cMenuPianoQuantizeTriplet == null) {
			cMenuPianoQuantizeTriplet = new JMenuItem();
		}
		return cMenuPianoQuantizeTriplet;
	}

	/**
	 * This method initializes cMenuPianoLength	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getCMenuPianoLength() {
		if (cMenuPianoLength == null) {
			cMenuPianoLength = new JMenu();
			cMenuPianoLength.add(getCMenuPianoLength04());
			cMenuPianoLength.add(getCMenuPianoLength08());
			cMenuPianoLength.add(getCMenuPianoLength16());
			cMenuPianoLength.add(getCMenuPianoLength32());
			cMenuPianoLength.add(getCMenuPianoLength64());
			cMenuPianoLength.add(getCMenuPianoLength128());
			cMenuPianoLength.add(getCMenuPianoLengthOff());
			cMenuPianoLength.addSeparator();
			cMenuPianoLength.add(getCMenuPianoLengthTriplet());
		}
		return cMenuPianoLength;
	}

	/**
	 * This method initializes cMenuPianoLength04	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoLength04() {
		if (cMenuPianoLength04 == null) {
			cMenuPianoLength04 = new JMenuItem();
		}
		return cMenuPianoLength04;
	}

	/**
	 * This method initializes cMenuPianoLength08	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoLength08() {
		if (cMenuPianoLength08 == null) {
			cMenuPianoLength08 = new JMenuItem();
		}
		return cMenuPianoLength08;
	}

	/**
	 * This method initializes cMenuPianoLength16	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoLength16() {
		if (cMenuPianoLength16 == null) {
			cMenuPianoLength16 = new JMenuItem();
		}
		return cMenuPianoLength16;
	}

	/**
	 * This method initializes cMenuPianoLength32	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoLength32() {
		if (cMenuPianoLength32 == null) {
			cMenuPianoLength32 = new JMenuItem();
		}
		return cMenuPianoLength32;
	}

	/**
	 * This method initializes cMenuPianoLength64	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoLength64() {
		if (cMenuPianoLength64 == null) {
			cMenuPianoLength64 = new JMenuItem();
		}
		return cMenuPianoLength64;
	}

	/**
	 * This method initializes cMenuPianoLength128	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoLength128() {
		if (cMenuPianoLength128 == null) {
			cMenuPianoLength128 = new JMenuItem();
		}
		return cMenuPianoLength128;
	}

	/**
	 * This method initializes cMenuPianoLengthTriplet	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoLengthTriplet() {
		if (cMenuPianoLengthTriplet == null) {
			cMenuPianoLengthTriplet = new JMenuItem();
		}
		return cMenuPianoLengthTriplet;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackTabTrackOn() {
		if (cMenuTrackTabTrackOn == null) {
			cMenuTrackTabTrackOn = new JMenuItem();
		}
		return cMenuTrackTabTrackOn;
	}

	/**
	 * This method initializes cMenuTrackTabAdd	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackTabAdd() {
		if (cMenuTrackTabAdd == null) {
			cMenuTrackTabAdd = new JMenuItem();
		}
		return cMenuTrackTabAdd;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackTabCopy() {
		if (cMenuTrackTabCopy == null) {
			cMenuTrackTabCopy = new JMenuItem();
		}
		return cMenuTrackTabCopy;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackTabChangeName() {
		if (cMenuTrackTabChangeName == null) {
			cMenuTrackTabChangeName = new JMenuItem();
		}
		return cMenuTrackTabChangeName;
	}

	/**
	 * This method initializes jMenuItem4	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackTabDelete() {
		if (cMenuTrackTabDelete == null) {
			cMenuTrackTabDelete = new JMenuItem();
		}
		return cMenuTrackTabDelete;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackTabRenderCurrent() {
		if (cMenuTrackTabRenderCurrent == null) {
			cMenuTrackTabRenderCurrent = new JMenuItem();
		}
		return cMenuTrackTabRenderCurrent;
	}

	/**
	 * This method initializes cMenuTrackTabRenderAll	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackTabRenderAll() {
		if (cMenuTrackTabRenderAll == null) {
			cMenuTrackTabRenderAll = new JMenuItem();
		}
		return cMenuTrackTabRenderAll;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackTabOverlay() {
		if (cMenuTrackTabOverlay == null) {
			cMenuTrackTabOverlay = new JMenuItem();
		}
		return cMenuTrackTabOverlay;
	}

	/**
	 * This method initializes cMenuTrackTabRenderer	
	 * 	
	 * @return javax.swing.JMenu	
	 */
	private JMenu getCMenuTrackTabRenderer() {
		if (cMenuTrackTabRenderer == null) {
			cMenuTrackTabRenderer = new JMenu();
			cMenuTrackTabRenderer.add(getCMenuTrackTabRendererVOCALOID1());
			cMenuTrackTabRenderer.add(getCMenuTrackTabRendererVOCALOID2());
			cMenuTrackTabRenderer.add(getCMenuTrackTabRendererUtau());
			cMenuTrackTabRenderer.add(getCMenuTrackTabRendererStraight());
		}
		return cMenuTrackTabRenderer;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackTabRendererVOCALOID2() {
		if (cMenuTrackTabRendererVOCALOID2 == null) {
			cMenuTrackTabRendererVOCALOID2 = new JMenuItem();
		}
		return cMenuTrackTabRendererVOCALOID2;
	}

	/**
	 * This method initializes cMenuTrackTabRendererVOCALOID1	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackTabRendererVOCALOID1() {
		if (cMenuTrackTabRendererVOCALOID1 == null) {
			cMenuTrackTabRendererVOCALOID1 = new JMenuItem();
		}
		return cMenuTrackTabRendererVOCALOID1;
	}

	/**
	 * This method initializes jMenuItem2	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackTabRendererUtau() {
		if (cMenuTrackTabRendererUtau == null) {
			cMenuTrackTabRendererUtau = new JMenuItem();
		}
		return cMenuTrackTabRendererUtau;
	}

	/**
	 * This method initializes jMenuItem3	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuTrackTabRendererStraight() {
		if (cMenuTrackTabRendererStraight == null) {
			cMenuTrackTabRendererStraight = new JMenuItem();
		}
		return cMenuTrackTabRendererStraight;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoQuantizeOff() {
		if (cMenuPianoQuantizeOff == null) {
			cMenuPianoQuantizeOff = new JMenuItem();
		}
		return cMenuPianoQuantizeOff;
	}

	/**
	 * This method initializes jMenuItem	
	 * 	
	 * @return javax.swing.JMenuItem	
	 */
	private JMenuItem getCMenuPianoLengthOff() {
		if (cMenuPianoLengthOff == null) {
			cMenuPianoLengthOff = new JMenuItem();
		}
		return cMenuPianoLengthOff;
	}

	/**
	 * This method initializes panel3	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getPanel3() {
		if (panel3 == null) {
			GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			gridBagConstraints6.gridx = 3;
			gridBagConstraints6.weightx = 1.0D;
			gridBagConstraints6.gridheight = 2;
			gridBagConstraints6.weighty = 1.0D;
			gridBagConstraints6.fill = GridBagConstraints.BOTH;
			gridBagConstraints6.gridy = 0;
			GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			gridBagConstraints5.gridx = 4;
			gridBagConstraints5.gridy = 1;
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.gridx = 4;
			gridBagConstraints4.gridy = 0;
			GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			gridBagConstraints3.gridx = 1;
			gridBagConstraints3.gridheight = 2;
			gridBagConstraints3.gridy = 0;
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 2;
			gridBagConstraints2.weighty = 1.0D;
			gridBagConstraints2.gridy = 1;
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.gridx = 2;
			gridBagConstraints1.weighty = 1.0D;
			gridBagConstraints1.gridy = 0;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 0;
			gridBagConstraints.gridheight = 2;
			gridBagConstraints.weighty = 1.0D;
			gridBagConstraints.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints.gridy = 0;
			panel3 = new JPanel();
			panel3.setLayout(new GridBagLayout());
			panel3.add(getJButton(), gridBagConstraints);
			panel3.add(getJButton1(), gridBagConstraints1);
			panel3.add(getJButton2(), gridBagConstraints2);
			panel3.add(getJButton3(), gridBagConstraints3);
			panel3.add(getJButton4(), gridBagConstraints4);
			panel3.add(getJButton5(), gridBagConstraints5);
			panel3.add(getJPanel2(), gridBagConstraints6);
		}
		return panel3;
	}

	/**
	 * This method initializes jPanel2	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel2() {
		if (jPanel2 == null) {
			jPanel2 = new JPanel();
			jPanel2.setLayout(new GridBagLayout());
		}
		return jPanel2;
	}

	/**
	 * This method initializes jButton	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getJButton() {
		if (jButton == null) {
			jButton = new JButton();
			jButton.setText("-");
		}
		return jButton;
	}

	/**
	 * This method initializes jButton1	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getJButton1() {
		if (jButton1 == null) {
			jButton1 = new JButton();
			jButton1.setText("<");
		}
		return jButton1;
	}

	/**
	 * This method initializes jButton2	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getJButton2() {
		if (jButton2 == null) {
			jButton2 = new JButton();
			jButton2.setText(">");
		}
		return jButton2;
	}

	/**
	 * This method initializes jButton3	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getJButton3() {
		if (jButton3 == null) {
			jButton3 = new JButton();
			jButton3.setText("+");
		}
		return jButton3;
	}

	/**
	 * This method initializes jButton4	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getJButton4() {
		if (jButton4 == null) {
			jButton4 = new JButton();
			jButton4.setText("<");
		}
		return jButton4;
	}

	/**
	 * This method initializes jButton5	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getJButton5() {
		if (jButton5 == null) {
			jButton5 = new JButton();
			jButton5.setText(">");
		}
		return jButton5;
	}

	/**
	 * This method initializes pictPianoRoll	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private PictPianoRoll getPictPianoRoll() {
		if (pictPianoRoll == null) {
			pictPianoRoll = new PictPianoRoll();
			pictPianoRoll.setLayout(new GridBagLayout());
		}
		return pictPianoRoll;
	}

	/**
	 * This method initializes vScroll	
	 * 	
	 * @return javax.swing.JScrollBar	
	 */
	private JScrollBar getVScroll() {
		if (vScroll == null) {
			vScroll = new JScrollBar();
		}
		return vScroll;
	}

	/**
	 * This method initializes hScroll	
	 * 	
	 * @return javax.swing.JScrollBar	
	 */
	private JScrollBar getHScroll() {
		if (hScroll == null) {
			hScroll = new JScrollBar();
			hScroll.setOrientation(JScrollBar.HORIZONTAL);
		}
		return hScroll;
	}

	/**
	 * This method initializes pictureBox3	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getPictureBox3() {
		if (pictureBox3 == null) {
			GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			gridBagConstraints12.anchor = GridBagConstraints.EAST;
			pictureBox3 = new JPanel();
			pictureBox3.setLayout(new GridBagLayout());
			pictureBox3.setPreferredSize(new Dimension(68, 0));
			pictureBox3.add(getJButton6(), gridBagConstraints12);
		}
		return pictureBox3;
	}

	/**
	 * This method initializes trackBar	
	 * 	
	 * @return javax.swing.JSlider	
	 */
	private JSlider getTrackBar() {
		if (trackBar == null) {
			trackBar = new JSlider();
			trackBar.setPreferredSize(new Dimension(83, 16));
		}
		return trackBar;
	}

	/**
	 * This method initializes jButton6	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getJButton6() {
		if (jButton6 == null) {
			jButton6 = new JButton();
		}
		return jButton6;
	}

	/**
	 * This method initializes picturePositionIndicator	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getPicturePositionIndicator() {
		if (picturePositionIndicator == null) {
			picturePositionIndicator = new JPanel();
			picturePositionIndicator.setLayout(new GridBagLayout());
			picturePositionIndicator.setPreferredSize(new Dimension(421, 48));
		}
		return picturePositionIndicator;
	}

	/**
	 * This method initializes jPanel1	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel1() {
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
			jPanel1 = new JPanel();
			jPanel1.setLayout(new GridBagLayout());
			jPanel1.add(getPanel3(), gridBagConstraints13);
			jPanel1.add(getPicturePositionIndicator(), gridBagConstraints14);
			jPanel1.add(getPanel1(), gridBagConstraints15);
		}
		return jPanel1;
	}

	/**
	 * This method initializes jPanel3	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel3() {
		if (jPanel3 == null) {
			GridLayout gridLayout1 = new GridLayout();
			gridLayout1.setRows(2);
			gridLayout1.setColumns(1);
			statusLabel = new JLabel();
			statusLabel.setText("");
			jPanel3 = new JPanel();
			jPanel3.setLayout(gridLayout1);
			jPanel3.add(getToolStripBottom(), null);
			jPanel3.add(statusLabel, null);
		}
		return jPanel3;
	}

}  //  @jve:decl-index=0:visual-constraint="18,34"
