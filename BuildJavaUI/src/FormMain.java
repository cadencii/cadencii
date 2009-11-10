import java.awt.BorderLayout;
import javax.swing.JPanel;
import javax.swing.JFrame;
import java.awt.Dimension;
import javax.swing.JMenuBar;
import javax.swing.JMenu;
import javax.swing.JMenuItem;
import javax.swing.JSeparator;

public class FormMain extends JFrame {

	private static final long serialVersionUID = 1L;
	private JPanel jContentPane = null;
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
		this.setSize(379, 269);
		this.setJMenuBar(getJJMenuBar());
		this.setContentPane(getJContentPane());
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
		}
		return menuHelpAbout;
	}

}  //  @jve:decl-index=0:visual-constraint="10,10"
