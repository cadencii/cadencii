package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.BorderFactory;
import javax.swing.JCheckBox;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTabbedPane;
import javax.swing.border.TitledBorder;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BCheckBox;
import org.kbinani.windows.forms.BComboBox;
import org.kbinani.windows.forms.BDialog;
import org.kbinani.windows.forms.BGroupBox;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BListView;
import org.kbinani.windows.forms.BNumericUpDown;
import org.kbinani.windows.forms.BPanel;
import org.kbinani.windows.forms.BRadioButton;
import org.kbinani.windows.forms.BTextBox;
import org.kbinani.windows.forms.RadioButtonManager;

//SECTION-END-IMPORT
public class Preference extends BDialog {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
	private BPanel tabSequence = null;  //  @jve:decl-index=0:visual-constraint="-96,1204"
	private BLabel lblResolution = null;
	private BPanel BPanel = null;
	private BLabel lblResolControlCurve = null;
	private BComboBox comboResolControlCurve = null;
	private BLabel jLabel1 = null;
	private BPanel jPanel1 = null;
	private BLabel lblVibratoLength = null;
	private BComboBox comboVibratoLength = null;
	private BLabel jLabel13 = null;
	private BGroupBox groupUserDefined = null;
	private BPanel jPanel3 = null;
	private BLabel lblAutoVibratoThresholdLength = null;
	private BLabel jLabel4 = null;
	private BPanel jPanel4 = null;
	private BLabel lblAutoVibratoType1 = null;
	private BComboBox comboAutoVibratoType1 = null;
	private BLabel lblAutoVibratoType2 = null;
	private BComboBox comboAutoVibratoType2 = null;
	private BPanel tabAnother = null;  //  @jve:decl-index=0:visual-constraint="384,989"
	private BLabel lblDefaultSinger = null;
	private BComboBox comboDefualtSinger = null;
	private BLabel lblPreSendTime = null;
	private BNumericUpDown numPreSendTime = null;
	private BLabel jLabel8 = null;
	private BLabel lblWait = null;
	private BNumericUpDown numWait = null;
	private BLabel jLabel81 = null;
	private BLabel jLabel9 = null;
	private BPanel tabAppearance = null;
	private BGroupBox groupFont = null;
	private BLabel labelMenu = null;
	private BLabel labelMenuFontName = null;
	private BButton btnChangeMenuFont = null;
	private BLabel labelScreen = null;
	private BLabel labelScreenFontName = null;
	private BButton btnChangeScreenFont = null;
	private BPanel jPanel7 = null;
	private BLabel lblLanguage = null;
	private BComboBox comboLanguage = null;
	private BPanel jPanel71 = null;
	private BLabel lblTrackHeight = null;
	private BNumericUpDown numTrackHeight = null;
	private BGroupBox groupVisibleCurve = null;
	private BCheckBox chkAccent = null;
	private BCheckBox chkDecay = null;
	private BCheckBox chkVibratoRate = null;
	private BCheckBox chkVibratoDepth = null;
	private BCheckBox chkVel = null;
	private BCheckBox chkDyn = null;
	private BCheckBox chkBre = null;
	private BCheckBox chkBri = null;
	private BCheckBox chkCle = null;
	private BCheckBox chkOpe = null;
	private BCheckBox chkGen = null;
	private BCheckBox chkPor = null;
	private BCheckBox chkPit = null;
	private BCheckBox chkPbs = null;
	private BCheckBox chkHarmonics = null;
	private BCheckBox chkFx2Depth = null;
	private BCheckBox chkReso1 = null;
	private BCheckBox chkReso2 = null;
	private BCheckBox chkReso3 = null;
	private BCheckBox chkReso4 = null;
	private BCheckBox chkEnvelope = null;
	private BPanel tabOperation = null;  //  @jve:decl-index=0:visual-constraint="466,50"
	private BGroupBox groupPianoroll = null;
	private BLabel labelWheelOrder = null;
	private BNumericUpDown numericUpDownEx1 = null;
	private BCheckBox chkCursorFix = null;
	private BCheckBox chkScrollHorizontal = null;
	private BCheckBox chkKeepLyricInputMode = null;
	private BCheckBox chkPlayPreviewWhenRightClick = null;
	private BCheckBox chkCurveSelectingQuantized = null;
	private BCheckBox chkUseSpaceKeyAsMiddleButtonModifier = null;
	private BGroupBox groupMisc = null;
	private BLabel lblMaximumFrameRate = null;
	private BNumericUpDown numMaximumFrameRate = null;
	private BLabel lblMilliSecond = null;
	private BLabel lblMouseHoverTime = null;
	private BNumericUpDown numMouseHoverTime = null;
	private BLabel lblMidiInPort = null;
	private BComboBox comboMidiInPortNumber = null;
	private BPanel tabPlatform = null;  //  @jve:decl-index=0:visual-constraint="-96,607"
	private BCheckBox chkTranslateRoman = null;
	private BGroupBox groupVsti = null;
	private BLabel lblVOCALOID1 = null;
	private BTextBox txtVOCALOID1 = null;
	private BLabel lblVOCALOID2 = null;
	private BTextBox txtVOCALOID2 = null;
	private BGroupBox groupUtauCores = null;
	private BLabel lblResampler = null;
	private BLabel lblWavtool = null;
	private BTextBox txtWavtool = null;
	private BButton btnWavtool = null;
	private BCheckBox chkWavtoolWithWine = null;
	private BPanel tabUtausingers = null;  //  @jve:decl-index=0:visual-constraint="-95,1685"
	private BListView listSingers = null;
	private BButton btnAdd = null;
	private BButton btnRemove = null;
	private BButton btnUp = null;
	private BButton btnDown = null;
	private BPanel jPanel17 = null;
	private BPanel jPanel18 = null;
	private BPanel tabFile = null;
	private BCheckBox chkAutoBackup = null;
	private BLabel lblAutoBackupInterval = null;
	private BLabel lblAutoBackupMinutes = null;
	private BPanel jPanel20 = null;
	private BPanel panelMiddle = null;
	private BButton btnOK = null;
	private BButton btnCancel = null;
	private BPanel jPanel5 = null;
	private BPanel panelTop = null;
    private JTabbedPane tabPane = null;
	private BNumericUpDown numAutoBackupInterval = null;
    private JCheckBox chkChasePastEvent = null;
    private BLabel lblAquesTone = null;
    private BTextBox txtAquesTone = null;
    private BButton btnAquesTone = null;
    private BLabel lblBuffer = null;
    private BNumericUpDown numBuffer = null;
    private BLabel lblBufferSize = null;
    private BLabel labelMtcMidiInPort = null;
    private BComboBox comboMtcMidiInPortNumber = null;
    private BPanel tabSingingSynth = null;  //  @jve:decl-index=0:visual-constraint="385,596"
    private BGroupBox groupSynthesizerDll = null;
    private BCheckBox chkLoadVocaloid1 = null;
    private BCheckBox chkLoadVocaloid2 = null;
    private BCheckBox chkLoadAquesTone = null;
    private BCheckBox chkKeepProjectCache = null;
    private BCheckBox chkEnableAutoVibrato = null;
    private BLabel lblAutoVibratoType = null;
    private BPanel jPanel11 = null;
    private BGroupBox groupVocaloidEditorCompatible = null;
    private BPanel jPanel31 = null;
    private BPanel jPanel41 = null;
    private BLabel lblAutoVibratoTypeCustom = null;
    private JPanel jPanel21 = null;
    private BRadioButton radioVocaloidEditorCompatible = null;
    private BRadioButton radioUserDefined = null;
    private BGroupBox groupDefaultSynthesizer = null;
    private BComboBox comboDefaultSynthesizer = null;
    private BComboBox comboAutoVibratoTypeCustom = null;
    private JPanel jPanel = null;
    private BListView listResampler = null;
    private BButton buttonResamplerAdd = null;
    private BButton buttonResamplerRemove = null;
    private BButton buttonResamplerUp = null;
    private BButton buttonResamplerDown = null;
    private BLabel jLabel91 = null;
    private BLabel jLabel911 = null;
    private BTextBox txtAutoVibratoThresholdLength = null;
    private JScrollPane jScrollPane = null;
    private RadioButtonManager mManager = null;
    private JScrollPane jScrollPane1 = null;
    private BLabel lblPlatform1 = null;
    private JPanel jPanel2 = null;
    private BLabel labelWavtoolPath = null;
    private BLabel lblResampler1 = null;
    private BLabel labelResamplerWithWine = null;
    private BPanel panelBottom = null;
    private BLabel labelStatus = null;
    private BGroupBox groupWine = null;
    private BLabel labelWinePrefix = null;
    private BTextBox textWinePrefix = null;
    private BButton buttonWinePrefix = null;
    private BLabel labelWineTop = null;
    private BTextBox textWineTop = null;
    private BButton buttonWineTop = null;
    private BRadioButton radioWineCustom = null;
    private BPanel BPanel1 = null;
    private BRadioButton radioWineBuiltin = null;
    //SECTION-END-FIELD
	/**
	 * This is the default constructor
	 */
	public Preference() {
	    //SECTION-BEGIN-CTOR
		super();
		initialize();
		tabPane = new JTabbedPane();
		tabPane.addTab( "Sequence", getTabSequence() );
		tabPane.addTab( "Other Settings", getTabAnother() );
		tabPane.addTab( "Appearance", getTabAppearance() );
		tabPane.addTab( "Operation", getTabOperation() );
		tabPane.addTab( "Platform", getTabPlatform() );
		tabPane.addTab( "UTAU Singers", getTabUtausingers() );
		tabPane.addTab( "File", getTabFile() );
		tabPane.addTab( "Synthesizer", getTabSingingSynth() );
		GridBagLayout layout = new GridBagLayout();
		getPanelTop().setLayout( layout );
		GridBagConstraints gb = new GridBagConstraints();
		gb.weighty = 1;
		gb.weightx = 1;
		gb.insets = new Insets( 12, 12, 0, 12 );
		gb.fill = GridBagConstraints.BOTH;
		layout.setConstraints( tabPane, gb );
	    this.getPanelTop().add( tabPane );
        mManager = new RadioButtonManager();
        mManager.add( radioUserDefined );
        mManager.add( radioVocaloidEditorCompatible );
        RadioButtonManager rbm2 = new RadioButtonManager();
        rbm2.add( radioWineBuiltin );
        rbm2.add( radioWineCustom );
        setCancelButton( btnCancel );
		//SECTION-END-CTOR
	}

	//SECTION-BEGIN-METHOD
	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		this.setSize(520, 591);
		this.setResizable(true);
		this.setContentPane(getJPanel5());
		this.setTitle("Preference");
	}

	/**
	 * This method initializes tabSequence
	 * 
	 * @return javax.swing.BPanel
	 */
	private BPanel getTabSequence() {
		if (tabSequence == null) {
			GridBagConstraints gridBagConstraints146 = new GridBagConstraints();
			gridBagConstraints146.gridx = 0;
			gridBagConstraints146.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints146.anchor = GridBagConstraints.NORTHWEST;
			gridBagConstraints146.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints146.gridy = 3;
			GridBagConstraints gridBagConstraints410 = new GridBagConstraints();
			gridBagConstraints410.gridx = 0;
			gridBagConstraints410.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints410.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints410.weighty = 1.0D;
			gridBagConstraints410.anchor = GridBagConstraints.NORTHWEST;
			gridBagConstraints410.gridy = 5;
			GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			gridBagConstraints7.gridx = 0;
			gridBagConstraints7.anchor = GridBagConstraints.WEST;
			gridBagConstraints7.insets = new Insets(12, 12, 3, 0);
			gridBagConstraints7.gridy = 4;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 0;
			gridBagConstraints.anchor = GridBagConstraints.WEST;
			gridBagConstraints.insets = new Insets(12, 12, 3, 0);
			gridBagConstraints.gridy = 2;
			GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
			gridBagConstraints21.anchor = GridBagConstraints.WEST;
			gridBagConstraints21.gridy = 1;
			gridBagConstraints21.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints21.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints21.gridx = 0;
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.anchor = GridBagConstraints.WEST;
			gridBagConstraints1.gridx = 0;
			gridBagConstraints1.gridy = 0;
			gridBagConstraints1.weightx = 1.0D;
			gridBagConstraints1.insets = new Insets(12, 12, 3, 0);
			lblResolution = new BLabel();
			lblResolution.setText("Resolution(VSTi)");
            lblAutoVibratoType = new BLabel();
            lblAutoVibratoType.setText("Auto Vibrato Type");
			tabSequence = new BPanel();
			tabSequence.setLayout(new GridBagLayout());
			tabSequence.setSize(new Dimension(465, 459));
			tabSequence.add(lblResolution, gridBagConstraints1);
			tabSequence.add(getJPanel(), gridBagConstraints21);
			tabSequence.add(getChkEnableAutoVibrato(), gridBagConstraints);
			tabSequence.add(lblAutoVibratoType, gridBagConstraints7);
			tabSequence.add(getJPanel11(), gridBagConstraints410);
			tabSequence.add(getJPanel1(), gridBagConstraints146);
		}
		return tabSequence;
	}

	/**
	 * This method initializes BPanel	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getJPanel() {
		if (BPanel == null) {
			GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
			gridBagConstraints8.gridx = 4;
			gridBagConstraints8.weightx = 1.0D;
			gridBagConstraints8.anchor = GridBagConstraints.WEST;
			gridBagConstraints8.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints8.gridy = 0;
			jLabel1 = new BLabel();
			jLabel1.setText("clocks");
			GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			gridBagConstraints3.fill = GridBagConstraints.NONE;
			gridBagConstraints3.gridy = 0;
			gridBagConstraints3.weightx = 0.0D;
			gridBagConstraints3.anchor = GridBagConstraints.WEST;
			gridBagConstraints3.insets = new Insets(3, 24, 3, 0);
			gridBagConstraints3.gridx = 3;
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 0;
			gridBagConstraints2.anchor = GridBagConstraints.WEST;
			gridBagConstraints2.insets = new Insets(0, 24, 0, 0);
			gridBagConstraints2.gridy = 0;
			lblResolControlCurve = new BLabel();
			lblResolControlCurve.setText("Control Curve");
			BPanel = new BPanel();
			BPanel.setLayout(new GridBagLayout());
			BPanel.add(lblResolControlCurve, gridBagConstraints2);
			BPanel.add(getComboResolControlCurve(), gridBagConstraints3);
			BPanel.add(jLabel1, gridBagConstraints8);
		}
		return BPanel;
	}

	/**
	 * This method initializes comboResolControlCurve	
	 * 	
	 * @return javax.swing.BComboBox	
	 */
	private BComboBox getComboResolControlCurve() {
		if (comboResolControlCurve == null) {
			comboResolControlCurve = new BComboBox();
			comboResolControlCurve.setPreferredSize(new Dimension(101, 27));
		}
		return comboResolControlCurve;
	}

	/**
	 * This method initializes jPanel1	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getJPanel1() {
		if (jPanel1 == null) {
			GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			gridBagConstraints5.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints5.gridy = 1;
			gridBagConstraints5.weightx = 1.0;
			gridBagConstraints5.insets = new Insets(3, 24, 3, 0);
			gridBagConstraints5.gridx = 3;
			GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			gridBagConstraints6.anchor = GridBagConstraints.WEST;
			gridBagConstraints6.gridy = 1;
			gridBagConstraints6.weightx = 1.0D;
			gridBagConstraints6.gridx = 4;
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.gridx = 0;
			gridBagConstraints4.anchor = GridBagConstraints.WEST;
			gridBagConstraints4.insets = new Insets(0, 24, 0, 0);
			gridBagConstraints4.gridy = 1;
			GridBagConstraints gridBagConstraints81 = new GridBagConstraints();
			gridBagConstraints81.anchor = GridBagConstraints.WEST;
			gridBagConstraints81.gridx = 4;
			gridBagConstraints81.gridy = 0;
			gridBagConstraints81.weightx = 1.0D;
			gridBagConstraints81.insets = new Insets(0, 12, 0, 0);
			jLabel13 = new BLabel();
			jLabel13.setText("%");
			GridBagConstraints gridBagConstraints32 = new GridBagConstraints();
			gridBagConstraints32.anchor = GridBagConstraints.WEST;
			gridBagConstraints32.insets = new Insets(3, 24, 3, 0);
			gridBagConstraints32.gridx = 3;
			gridBagConstraints32.gridy = 0;
			gridBagConstraints32.weightx = 0.0D;
			gridBagConstraints32.fill = GridBagConstraints.NONE;
			GridBagConstraints gridBagConstraints22 = new GridBagConstraints();
			gridBagConstraints22.anchor = GridBagConstraints.WEST;
			gridBagConstraints22.gridx = 0;
			gridBagConstraints22.gridy = 0;
			gridBagConstraints22.insets = new Insets(0, 24, 0, 0);
			lblVibratoLength = new BLabel();
			lblVibratoLength.setText("Default Vibrato Length");
			jPanel1 = new BPanel();
			jPanel1.setLayout(new GridBagLayout());
			jPanel1.add(lblVibratoLength, gridBagConstraints22);
			jPanel1.add(getComboVibratoLength(), gridBagConstraints32);
			jPanel1.add(jLabel13, gridBagConstraints81);
			jPanel1.add(lblAutoVibratoThresholdLength, gridBagConstraints4);
			jPanel1.add(jLabel4, gridBagConstraints6);
			jPanel1.add(getTxtAutoVibratoThresholdLength(), gridBagConstraints5);
		}
		return jPanel1;
	}

	/**
	 * This method initializes comboVibratoLength	
	 * 	
	 * @return javax.swing.BComboBox	
	 */
	private BComboBox getComboVibratoLength() {
		if (comboVibratoLength == null) {
			comboVibratoLength = new BComboBox();
			comboVibratoLength.setPreferredSize(new Dimension(101, 27));
		}
		return comboVibratoLength;
	}

	/**
	 * This method initializes groupUserDefined	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BGroupBox getGroupUserDefined() {
		if (groupUserDefined == null) {
			GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
			gridBagConstraints10.gridx = 0;
			gridBagConstraints10.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints10.weightx = 1.0D;
			gridBagConstraints10.gridy = 0;
			GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
			gridBagConstraints14.gridx = 0;
			gridBagConstraints14.anchor = GridBagConstraints.WEST;
			gridBagConstraints14.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints14.gridy = 1;
			groupUserDefined = new BGroupBox();
			groupUserDefined.setLayout(new GridBagLayout());
			groupUserDefined.setBorder(BorderFactory.createTitledBorder(null, "Auto Vibrato Settings", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupUserDefined.setTitle("User Defined");
			groupUserDefined.add(getJPanel3(), gridBagConstraints14);
			groupUserDefined.add(getJPanel41(), gridBagConstraints10);
		}
		return groupUserDefined;
	}

	/**
	 * This method initializes jPanel3	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getJPanel3() {
		if (jPanel3 == null) {
			jLabel4 = new BLabel();
			jLabel4.setText("clock");
			lblAutoVibratoThresholdLength = new BLabel();
			lblAutoVibratoThresholdLength.setText("Minimum note length for Automatic Vibrato");
			jPanel3 = new BPanel();
			jPanel3.setLayout(new GridBagLayout());
		}
		return jPanel3;
	}

	/**
	 * This method initializes jPanel4	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getJPanel4() {
		if (jPanel4 == null) {
			GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
			gridBagConstraints18.fill = GridBagConstraints.NONE;
			gridBagConstraints18.gridy = 1;
			gridBagConstraints18.weightx = 1.0;
			gridBagConstraints18.anchor = GridBagConstraints.WEST;
			gridBagConstraints18.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints18.gridx = 1;
			GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
			gridBagConstraints17.gridx = 0;
			gridBagConstraints17.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints17.gridy = 1;
			lblAutoVibratoType2 = new BLabel();
			lblAutoVibratoType2.setText("Vibrato Type VOCALOID2");
			GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
			gridBagConstraints16.fill = GridBagConstraints.NONE;
			gridBagConstraints16.gridy = 0;
			gridBagConstraints16.weightx = 1.0;
			gridBagConstraints16.anchor = GridBagConstraints.WEST;
			gridBagConstraints16.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints16.gridx = 1;
			GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
			gridBagConstraints15.gridx = 0;
			gridBagConstraints15.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints15.gridy = 0;
			lblAutoVibratoType1 = new BLabel();
			lblAutoVibratoType1.setText("Vibrato Type VOCALOID1");
			jPanel4 = new BPanel();
			jPanel4.setLayout(new GridBagLayout());
			jPanel4.add(lblAutoVibratoType1, gridBagConstraints15);
			jPanel4.add(getComboAutoVibratoType1(), gridBagConstraints16);
			jPanel4.add(lblAutoVibratoType2, gridBagConstraints17);
			jPanel4.add(getComboAutoVibratoType2(), gridBagConstraints18);
		}
		return jPanel4;
	}

	/**
	 * This method initializes comboAutoVibratoType1	
	 * 	
	 * @return javax.swing.BComboBox	
	 */
	private BComboBox getComboAutoVibratoType1() {
		if (comboAutoVibratoType1 == null) {
			comboAutoVibratoType1 = new BComboBox();
			comboAutoVibratoType1.setPreferredSize(new Dimension(150, 27));
		}
		return comboAutoVibratoType1;
	}

	/**
	 * This method initializes comboAutoVibratoType2	
	 * 	
	 * @return javax.swing.BComboBox	
	 */
	private BComboBox getComboAutoVibratoType2() {
		if (comboAutoVibratoType2 == null) {
			comboAutoVibratoType2 = new BComboBox();
			comboAutoVibratoType2.setPreferredSize(new Dimension(150, 27));
		}
		return comboAutoVibratoType2;
	}

	/**
	 * This method initializes tabAnother	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getTabAnother() {
		if (tabAnother == null) {
			GridBagConstraints gridBagConstraints124 = new GridBagConstraints();
			gridBagConstraints124.gridx = 2;
			gridBagConstraints124.anchor = GridBagConstraints.WEST;
			gridBagConstraints124.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints124.gridy = 4;
			lblBufferSize = new BLabel();
			lblBufferSize.setText("msec(100-1000)");
			GridBagConstraints gridBagConstraints123 = new GridBagConstraints();
			gridBagConstraints123.gridx = 1;
			gridBagConstraints123.anchor = GridBagConstraints.WEST;
			gridBagConstraints123.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints123.gridy = 4;
			GridBagConstraints gridBagConstraints122 = new GridBagConstraints();
			gridBagConstraints122.gridx = 0;
			gridBagConstraints122.anchor = GridBagConstraints.WEST;
			gridBagConstraints122.insets = new Insets(3, 24, 3, 0);
			gridBagConstraints122.gridy = 4;
			lblBuffer = new BLabel();
			lblBuffer.setText("Buffer Size");
			GridBagConstraints gridBagConstraints90 = new GridBagConstraints();
			gridBagConstraints90.gridx = 0;
			gridBagConstraints90.anchor = GridBagConstraints.WEST;
			gridBagConstraints90.insets = new Insets(0, 24, 0, 0);
			gridBagConstraints90.gridwidth = 3;
			gridBagConstraints90.gridy = 3;
			GridBagConstraints gridBagConstraints35 = new GridBagConstraints();
			gridBagConstraints35.gridx = 0;
			gridBagConstraints35.weighty = 1.0D;
			gridBagConstraints35.gridy = 9;
			jLabel9 = new BLabel();
			jLabel9.setText("   ");
			GridBagConstraints gridBagConstraints30 = new GridBagConstraints();
			gridBagConstraints30.gridx = 2;
			gridBagConstraints30.anchor = GridBagConstraints.WEST;
			gridBagConstraints30.weightx = 1.0D;
			gridBagConstraints30.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints30.gridy = 2;
			jLabel81 = new BLabel();
			jLabel81.setText("msec(200-2000)");
			GridBagConstraints gridBagConstraints29 = new GridBagConstraints();
			gridBagConstraints29.fill = GridBagConstraints.NONE;
			gridBagConstraints29.gridy = 2;
			gridBagConstraints29.weightx = 0.0D;
			gridBagConstraints29.anchor = GridBagConstraints.WEST;
			gridBagConstraints29.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints29.gridx = 1;
			GridBagConstraints gridBagConstraints28 = new GridBagConstraints();
			gridBagConstraints28.gridx = 0;
			gridBagConstraints28.anchor = GridBagConstraints.WEST;
			gridBagConstraints28.insets = new Insets(0, 24, 0, 0);
			gridBagConstraints28.gridy = 2;
			lblWait = new BLabel();
			lblWait.setText("Waiting Time");
			GridBagConstraints gridBagConstraints27 = new GridBagConstraints();
			gridBagConstraints27.gridx = 2;
			gridBagConstraints27.anchor = GridBagConstraints.WEST;
			gridBagConstraints27.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints27.weightx = 1.0D;
			gridBagConstraints27.gridy = 1;
			jLabel8 = new BLabel();
			jLabel8.setText("msec(500-5000)");
			GridBagConstraints gridBagConstraints26 = new GridBagConstraints();
			gridBagConstraints26.fill = GridBagConstraints.NONE;
			gridBagConstraints26.gridy = 1;
			gridBagConstraints26.weightx = 0.0D;
			gridBagConstraints26.anchor = GridBagConstraints.WEST;
			gridBagConstraints26.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints26.gridx = 1;
			GridBagConstraints gridBagConstraints25 = new GridBagConstraints();
			gridBagConstraints25.gridx = 0;
			gridBagConstraints25.anchor = GridBagConstraints.WEST;
			gridBagConstraints25.insets = new Insets(0, 24, 0, 0);
			gridBagConstraints25.gridy = 1;
			lblPreSendTime = new BLabel();
			lblPreSendTime.setText("Pre-Send time");
			GridBagConstraints gridBagConstraints24 = new GridBagConstraints();
			gridBagConstraints24.fill = GridBagConstraints.NONE;
			gridBagConstraints24.gridy = 0;
			gridBagConstraints24.weightx = 1.0;
			gridBagConstraints24.anchor = GridBagConstraints.WEST;
			gridBagConstraints24.insets = new Insets(12, 12, 3, 0);
			gridBagConstraints24.gridwidth = 2;
			gridBagConstraints24.gridx = 1;
			GridBagConstraints gridBagConstraints23 = new GridBagConstraints();
			gridBagConstraints23.gridx = 0;
			gridBagConstraints23.anchor = GridBagConstraints.WEST;
			gridBagConstraints23.insets = new Insets(12, 24, 0, 0);
			gridBagConstraints23.gridy = 0;
			lblDefaultSinger = new BLabel();
			lblDefaultSinger.setText("Default Singer");
			tabAnother = new BPanel();
			tabAnother.setLayout(new GridBagLayout());
			tabAnother.setSize(new Dimension(420, 283));
			tabAnother.add(lblDefaultSinger, gridBagConstraints23);
			tabAnother.add(getComboDefualtSinger(), gridBagConstraints24);
			tabAnother.add(lblPreSendTime, gridBagConstraints25);
			tabAnother.add(getNumPreSendTime(), gridBagConstraints26);
			tabAnother.add(jLabel8, gridBagConstraints27);
			tabAnother.add(lblWait, gridBagConstraints28);
			tabAnother.add(getNumWait(), gridBagConstraints29);
			tabAnother.add(jLabel81, gridBagConstraints30);
			tabAnother.add(jLabel9, gridBagConstraints35);
			tabAnother.add(getChkChasePastEvent(), gridBagConstraints90);
			tabAnother.add(lblBuffer, gridBagConstraints122);
			tabAnother.add(getNumBuffer(), gridBagConstraints123);
			tabAnother.add(lblBufferSize, gridBagConstraints124);
		}
		return tabAnother;
	}

	/**
	 * This method initializes comboDefualtSinger	
	 * 	
	 * @return javax.swing.BComboBox	
	 */
	private BComboBox getComboDefualtSinger() {
		if (comboDefualtSinger == null) {
			comboDefualtSinger = new BComboBox();
			comboDefualtSinger.setPreferredSize(new Dimension(222, 27));
		}
		return comboDefualtSinger;
	}

	/**
	 * This method initializes numPreSendTime	
	 * 	
	 * @return javax.swing.BComboBox	
	 */
	private BNumericUpDown getNumPreSendTime() {
		if (numPreSendTime == null) {
			numPreSendTime = new BNumericUpDown();
			numPreSendTime.setPreferredSize(new Dimension(68, 28));
			numPreSendTime.setMinimum(500.0F);
			numPreSendTime.setFloatValue(500.0F);
			numPreSendTime.setDecimalPlaces(0);
			numPreSendTime.setMaximum(5000.0F);
		}
		return numPreSendTime;
	}

	/**
	 * This method initializes numWait	
	 * 	
	 * @return javax.swing.BComboBox	
	 */
	private BNumericUpDown getNumWait() {
		if (numWait == null) {
			numWait = new BNumericUpDown();
			numWait.setPreferredSize(new Dimension(68, 28));
			numWait.setMinimum(200.0F);
			numWait.setFloatValue(200.0F);
			numWait.setMaximum(2000.0F);
		}
		return numWait;
	}

	/**
	 * This method initializes tabAppearance	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getTabAppearance() {
		if (tabAppearance == null) {
			GridBagConstraints gridBagConstraints69 = new GridBagConstraints();
			gridBagConstraints69.gridx = 0;
			gridBagConstraints69.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints69.weighty = 1.0D;
			gridBagConstraints69.anchor = GridBagConstraints.NORTH;
			gridBagConstraints69.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints69.gridy = 3;
			GridBagConstraints gridBagConstraints46 = new GridBagConstraints();
			gridBagConstraints46.gridx = 0;
			gridBagConstraints46.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints46.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints46.gridy = 2;
			GridBagConstraints gridBagConstraints45 = new GridBagConstraints();
			gridBagConstraints45.gridx = 0;
			gridBagConstraints45.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints45.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints45.gridy = 1;
			GridBagConstraints gridBagConstraints42 = new GridBagConstraints();
			gridBagConstraints42.gridx = 0;
			gridBagConstraints42.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints42.weightx = 1.0D;
			gridBagConstraints42.insets = new Insets(12, 12, 3, 12);
			gridBagConstraints42.gridy = 0;
			tabAppearance = new BPanel();
			tabAppearance.setLayout(new GridBagLayout());
			tabAppearance.add(getGroupFont(), gridBagConstraints42);
			tabAppearance.add(getJPanel7(), gridBagConstraints45);
			tabAppearance.add(getJPanel71(), gridBagConstraints46);
			tabAppearance.add(getGroupVisibleCurve(), gridBagConstraints69);
		}
		return tabAppearance;
	}

	/**
	 * This method initializes groupFont	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BGroupBox getGroupFont() {
		if (groupFont == null) {
			GridBagConstraints gridBagConstraints41 = new GridBagConstraints();
			gridBagConstraints41.gridx = 2;
			gridBagConstraints41.weightx = 1.0D;
			gridBagConstraints41.anchor = GridBagConstraints.WEST;
			gridBagConstraints41.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints41.gridy = 1;
			GridBagConstraints gridBagConstraints40 = new GridBagConstraints();
			gridBagConstraints40.gridx = 1;
			gridBagConstraints40.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints40.anchor = GridBagConstraints.WEST;
			gridBagConstraints40.gridy = 1;
			labelScreenFontName = new BLabel();
			labelScreenFontName.setText("MS UI Gothic");
			GridBagConstraints gridBagConstraints39 = new GridBagConstraints();
			gridBagConstraints39.gridx = 0;
			gridBagConstraints39.anchor = GridBagConstraints.WEST;
			gridBagConstraints39.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints39.gridy = 1;
			labelScreen = new BLabel();
			labelScreen.setText("Screen");
			GridBagConstraints gridBagConstraints38 = new GridBagConstraints();
			gridBagConstraints38.gridx = 2;
			gridBagConstraints38.weightx = 1.0D;
			gridBagConstraints38.anchor = GridBagConstraints.WEST;
			gridBagConstraints38.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints38.gridy = 0;
			GridBagConstraints gridBagConstraints37 = new GridBagConstraints();
			gridBagConstraints37.gridx = 1;
			gridBagConstraints37.anchor = GridBagConstraints.WEST;
			gridBagConstraints37.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints37.gridy = 0;
			labelMenuFontName = new BLabel();
			labelMenuFontName.setText("MS UI Gothic");
			GridBagConstraints gridBagConstraints36 = new GridBagConstraints();
			gridBagConstraints36.gridx = 0;
			gridBagConstraints36.anchor = GridBagConstraints.WEST;
			gridBagConstraints36.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints36.gridy = 0;
			labelMenu = new BLabel();
			labelMenu.setText("Menu/Lyrics");
			groupFont = new BGroupBox();
			groupFont.setLayout(new GridBagLayout());
			groupFont.setBorder(BorderFactory.createTitledBorder(null, "Font", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupFont.add(labelMenu, gridBagConstraints36);
			groupFont.add(labelMenuFontName, gridBagConstraints37);
			groupFont.add(getBtnChangeMenuFont(), gridBagConstraints38);
			groupFont.add(labelScreen, gridBagConstraints39);
			groupFont.add(labelScreenFontName, gridBagConstraints40);
			groupFont.add(getBtnChangeScreenFont(), gridBagConstraints41);
		}
		return groupFont;
	}

	/**
	 * This method initializes btnChangeMenuFont	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnChangeMenuFont() {
		if (btnChangeMenuFont == null) {
			btnChangeMenuFont = new BButton();
			btnChangeMenuFont.setText("Change");
			btnChangeMenuFont.setPreferredSize(new Dimension(85, 29));
		}
		return btnChangeMenuFont;
	}

	/**
	 * This method initializes btnChangeScreenFont	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnChangeScreenFont() {
		if (btnChangeScreenFont == null) {
			btnChangeScreenFont = new BButton();
			btnChangeScreenFont.setPreferredSize(new Dimension(85, 29));
			btnChangeScreenFont.setText("Change");
		}
		return btnChangeScreenFont;
	}

	/**
	 * This method initializes jPanel7	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getJPanel7() {
		if (jPanel7 == null) {
			GridBagConstraints gridBagConstraints44 = new GridBagConstraints();
			gridBagConstraints44.fill = GridBagConstraints.NONE;
			gridBagConstraints44.gridy = 0;
			gridBagConstraints44.weightx = 1.0;
			gridBagConstraints44.anchor = GridBagConstraints.WEST;
			gridBagConstraints44.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints44.gridx = 1;
			GridBagConstraints gridBagConstraints43 = new GridBagConstraints();
			gridBagConstraints43.gridx = 0;
			gridBagConstraints43.anchor = GridBagConstraints.WEST;
			gridBagConstraints43.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints43.gridy = 0;
			lblLanguage = new BLabel();
			lblLanguage.setText("UI Language");
			jPanel7 = new BPanel();
			jPanel7.setLayout(new GridBagLayout());
			jPanel7.add(lblLanguage, gridBagConstraints43);
			jPanel7.add(getComboLanguage(), gridBagConstraints44);
		}
		return jPanel7;
	}

	/**
	 * This method initializes comboLanguage	
	 * 	
	 * @return javax.swing.BComboBox	
	 */
	private BComboBox getComboLanguage() {
		if (comboLanguage == null) {
			comboLanguage = new BComboBox();
			comboLanguage.setPreferredSize(new Dimension(121, 27));
		}
		return comboLanguage;
	}

	/**
	 * This method initializes jPanel71	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getJPanel71() {
		if (jPanel71 == null) {
			GridBagConstraints gridBagConstraints441 = new GridBagConstraints();
			gridBagConstraints441.anchor = GridBagConstraints.WEST;
			gridBagConstraints441.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints441.gridx = 1;
			gridBagConstraints441.gridy = 0;
			gridBagConstraints441.weightx = 1.0;
			gridBagConstraints441.fill = GridBagConstraints.NONE;
			GridBagConstraints gridBagConstraints431 = new GridBagConstraints();
			gridBagConstraints431.anchor = GridBagConstraints.WEST;
			gridBagConstraints431.gridx = 0;
			gridBagConstraints431.gridy = 0;
			gridBagConstraints431.insets = new Insets(3, 12, 3, 0);
			lblTrackHeight = new BLabel();
			lblTrackHeight.setText("Track Height(pixel)");
			jPanel71 = new BPanel();
			jPanel71.setLayout(new GridBagLayout());
			jPanel71.add(lblTrackHeight, gridBagConstraints431);
			jPanel71.add(getNumTrackHeight(), gridBagConstraints441);
		}
		return jPanel71;
	}

	/**
	 * This method initializes numTrackHeight	
	 * 	
	 * @return javax.swing.BComboBox	
	 */
	private BNumericUpDown getNumTrackHeight() {
		if (numTrackHeight == null) {
			numTrackHeight = new BNumericUpDown();
			numTrackHeight.setPreferredSize(new Dimension(121, 28));
		}
		return numTrackHeight;
	}

	/**
	 * This method initializes groupVisibleCurve	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BGroupBox getGroupVisibleCurve() {
		if (groupVisibleCurve == null) {
			GridBagConstraints gridBagConstraints68 = new GridBagConstraints();
			gridBagConstraints68.gridx = 0;
			gridBagConstraints68.anchor = GridBagConstraints.WEST;
			gridBagConstraints68.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints68.gridy = 5;
			GridBagConstraints gridBagConstraints67 = new GridBagConstraints();
			gridBagConstraints67.gridx = 3;
			gridBagConstraints67.anchor = GridBagConstraints.WEST;
			gridBagConstraints67.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints67.gridy = 4;
			GridBagConstraints gridBagConstraints66 = new GridBagConstraints();
			gridBagConstraints66.gridx = 2;
			gridBagConstraints66.anchor = GridBagConstraints.WEST;
			gridBagConstraints66.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints66.gridy = 4;
			GridBagConstraints gridBagConstraints65 = new GridBagConstraints();
			gridBagConstraints65.gridx = 1;
			gridBagConstraints65.anchor = GridBagConstraints.WEST;
			gridBagConstraints65.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints65.gridy = 4;
			GridBagConstraints gridBagConstraints64 = new GridBagConstraints();
			gridBagConstraints64.gridx = 0;
			gridBagConstraints64.anchor = GridBagConstraints.WEST;
			gridBagConstraints64.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints64.gridy = 4;
			GridBagConstraints gridBagConstraints63 = new GridBagConstraints();
			gridBagConstraints63.gridx = 3;
			gridBagConstraints63.anchor = GridBagConstraints.WEST;
			gridBagConstraints63.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints63.gridy = 3;
			GridBagConstraints gridBagConstraints62 = new GridBagConstraints();
			gridBagConstraints62.gridx = 2;
			gridBagConstraints62.anchor = GridBagConstraints.WEST;
			gridBagConstraints62.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints62.gridy = 3;
			GridBagConstraints gridBagConstraints61 = new GridBagConstraints();
			gridBagConstraints61.gridx = 1;
			gridBagConstraints61.anchor = GridBagConstraints.WEST;
			gridBagConstraints61.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints61.gridy = 3;
			GridBagConstraints gridBagConstraints60 = new GridBagConstraints();
			gridBagConstraints60.gridx = 0;
			gridBagConstraints60.anchor = GridBagConstraints.WEST;
			gridBagConstraints60.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints60.gridy = 3;
			GridBagConstraints gridBagConstraints59 = new GridBagConstraints();
			gridBagConstraints59.gridx = 3;
			gridBagConstraints59.anchor = GridBagConstraints.WEST;
			gridBagConstraints59.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints59.gridy = 2;
			GridBagConstraints gridBagConstraints58 = new GridBagConstraints();
			gridBagConstraints58.gridx = 2;
			gridBagConstraints58.anchor = GridBagConstraints.WEST;
			gridBagConstraints58.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints58.gridy = 2;
			GridBagConstraints gridBagConstraints57 = new GridBagConstraints();
			gridBagConstraints57.gridx = 1;
			gridBagConstraints57.anchor = GridBagConstraints.WEST;
			gridBagConstraints57.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints57.gridy = 2;
			GridBagConstraints gridBagConstraints56 = new GridBagConstraints();
			gridBagConstraints56.gridx = 0;
			gridBagConstraints56.anchor = GridBagConstraints.WEST;
			gridBagConstraints56.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints56.gridy = 2;
			GridBagConstraints gridBagConstraints55 = new GridBagConstraints();
			gridBagConstraints55.gridx = 3;
			gridBagConstraints55.anchor = GridBagConstraints.WEST;
			gridBagConstraints55.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints55.gridy = 1;
			GridBagConstraints gridBagConstraints54 = new GridBagConstraints();
			gridBagConstraints54.gridx = 2;
			gridBagConstraints54.anchor = GridBagConstraints.WEST;
			gridBagConstraints54.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints54.gridy = 1;
			GridBagConstraints gridBagConstraints53 = new GridBagConstraints();
			gridBagConstraints53.gridx = 1;
			gridBagConstraints53.anchor = GridBagConstraints.WEST;
			gridBagConstraints53.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints53.gridy = 1;
			GridBagConstraints gridBagConstraints52 = new GridBagConstraints();
			gridBagConstraints52.gridx = 0;
			gridBagConstraints52.anchor = GridBagConstraints.WEST;
			gridBagConstraints52.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints52.gridy = 1;
			GridBagConstraints gridBagConstraints50 = new GridBagConstraints();
			gridBagConstraints50.gridx = 3;
			gridBagConstraints50.anchor = GridBagConstraints.WEST;
			gridBagConstraints50.weightx = 0.25D;
			gridBagConstraints50.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints50.gridy = 0;
			GridBagConstraints gridBagConstraints49 = new GridBagConstraints();
			gridBagConstraints49.gridx = 2;
			gridBagConstraints49.anchor = GridBagConstraints.WEST;
			gridBagConstraints49.weightx = 0.25D;
			gridBagConstraints49.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints49.gridy = 0;
			GridBagConstraints gridBagConstraints48 = new GridBagConstraints();
			gridBagConstraints48.gridx = 1;
			gridBagConstraints48.anchor = GridBagConstraints.WEST;
			gridBagConstraints48.weightx = 0.25D;
			gridBagConstraints48.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints48.gridy = 0;
			GridBagConstraints gridBagConstraints47 = new GridBagConstraints();
			gridBagConstraints47.gridx = 0;
			gridBagConstraints47.anchor = GridBagConstraints.WEST;
			gridBagConstraints47.weightx = 0.25D;
			gridBagConstraints47.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints47.gridy = 0;
			groupVisibleCurve = new BGroupBox();
			groupVisibleCurve.setLayout(new GridBagLayout());
			groupVisibleCurve.setBorder(BorderFactory.createTitledBorder(null, "Visible Control Curve", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupVisibleCurve.add(getChkAccent(), gridBagConstraints47);
			groupVisibleCurve.add(getChkDecay(), gridBagConstraints48);
			groupVisibleCurve.add(getChkVibratoRate(), gridBagConstraints49);
			groupVisibleCurve.add(getChkVibratoDepth(), gridBagConstraints50);
			groupVisibleCurve.add(getChkVel(), gridBagConstraints52);
			groupVisibleCurve.add(getChkDyn(), gridBagConstraints53);
			groupVisibleCurve.add(getChkBre(), gridBagConstraints54);
			groupVisibleCurve.add(getChkBri(), gridBagConstraints55);
			groupVisibleCurve.add(getChkCle(), gridBagConstraints56);
			groupVisibleCurve.add(getChkOpe(), gridBagConstraints57);
			groupVisibleCurve.add(getChkGen(), gridBagConstraints58);
			groupVisibleCurve.add(getChkPor(), gridBagConstraints59);
			groupVisibleCurve.add(getChkPit(), gridBagConstraints60);
			groupVisibleCurve.add(getChkPbs(), gridBagConstraints61);
			groupVisibleCurve.add(getChkHarmonics(), gridBagConstraints62);
			groupVisibleCurve.add(getChkFx2Depth(), gridBagConstraints63);
			groupVisibleCurve.add(getChkReso1(), gridBagConstraints64);
			groupVisibleCurve.add(getChkReso2(), gridBagConstraints65);
			groupVisibleCurve.add(getChkReso3(), gridBagConstraints66);
			groupVisibleCurve.add(getChkReso4(), gridBagConstraints67);
			groupVisibleCurve.add(getChkEnvelope(), gridBagConstraints68);
		}
		return groupVisibleCurve;
	}

	/**
	 * This method initializes chkAccent	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkAccent() {
		if (chkAccent == null) {
			chkAccent = new BCheckBox();
			chkAccent.setText("Accent");
		}
		return chkAccent;
	}

	/**
	 * This method initializes chkDecay	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkDecay() {
		if (chkDecay == null) {
			chkDecay = new BCheckBox();
			chkDecay.setText("Decay");
		}
		return chkDecay;
	}

	/**
	 * This method initializes chkVibratoRate	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkVibratoRate() {
		if (chkVibratoRate == null) {
			chkVibratoRate = new BCheckBox();
			chkVibratoRate.setText("VibratoRate");
		}
		return chkVibratoRate;
	}

	/**
	 * This method initializes chkVibratoDepth	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkVibratoDepth() {
		if (chkVibratoDepth == null) {
			chkVibratoDepth = new BCheckBox();
			chkVibratoDepth.setText("VibratoDepth");
		}
		return chkVibratoDepth;
	}

	/**
	 * This method initializes chkVel	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkVel() {
		if (chkVel == null) {
			chkVel = new BCheckBox();
			chkVel.setText("VEL");
		}
		return chkVel;
	}

	/**
	 * This method initializes chkDyn	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkDyn() {
		if (chkDyn == null) {
			chkDyn = new BCheckBox();
			chkDyn.setText("DYN");
		}
		return chkDyn;
	}

	/**
	 * This method initializes chkBre	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkBre() {
		if (chkBre == null) {
			chkBre = new BCheckBox();
			chkBre.setText("BRE");
		}
		return chkBre;
	}

	/**
	 * This method initializes chkBri	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkBri() {
		if (chkBri == null) {
			chkBri = new BCheckBox();
			chkBri.setText("BRI");
		}
		return chkBri;
	}

	/**
	 * This method initializes chkCle	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkCle() {
		if (chkCle == null) {
			chkCle = new BCheckBox();
			chkCle.setText("CLE");
		}
		return chkCle;
	}

	/**
	 * This method initializes chkOpe	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkOpe() {
		if (chkOpe == null) {
			chkOpe = new BCheckBox();
			chkOpe.setText("OPE");
		}
		return chkOpe;
	}

	/**
	 * This method initializes chkGen	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkGen() {
		if (chkGen == null) {
			chkGen = new BCheckBox();
			chkGen.setText("GEN");
		}
		return chkGen;
	}

	/**
	 * This method initializes chkPor	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkPor() {
		if (chkPor == null) {
			chkPor = new BCheckBox();
			chkPor.setText("POR");
		}
		return chkPor;
	}

	/**
	 * This method initializes chkPit	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkPit() {
		if (chkPit == null) {
			chkPit = new BCheckBox();
			chkPit.setText("PIT");
		}
		return chkPit;
	}

	/**
	 * This method initializes chkPbs	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkPbs() {
		if (chkPbs == null) {
			chkPbs = new BCheckBox();
			chkPbs.setText("PBS");
		}
		return chkPbs;
	}

	/**
	 * This method initializes chkHarmonics	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkHarmonics() {
		if (chkHarmonics == null) {
			chkHarmonics = new BCheckBox();
			chkHarmonics.setText("Harmonics");
		}
		return chkHarmonics;
	}

	/**
	 * This method initializes chkFx2Depth	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkFx2Depth() {
		if (chkFx2Depth == null) {
			chkFx2Depth = new BCheckBox();
			chkFx2Depth.setText("FX2Depth");
		}
		return chkFx2Depth;
	}

	/**
	 * This method initializes chkReso1	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkReso1() {
		if (chkReso1 == null) {
			chkReso1 = new BCheckBox();
			chkReso1.setText("Reso1");
		}
		return chkReso1;
	}

	/**
	 * This method initializes chkReso2	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkReso2() {
		if (chkReso2 == null) {
			chkReso2 = new BCheckBox();
			chkReso2.setText("Reso2");
		}
		return chkReso2;
	}

	/**
	 * This method initializes chkReso3	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkReso3() {
		if (chkReso3 == null) {
			chkReso3 = new BCheckBox();
			chkReso3.setText("Reso3");
		}
		return chkReso3;
	}

	/**
	 * This method initializes chkReso4	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkReso4() {
		if (chkReso4 == null) {
			chkReso4 = new BCheckBox();
			chkReso4.setText("Reso4");
		}
		return chkReso4;
	}

	/**
	 * This method initializes chkEnvelope	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkEnvelope() {
		if (chkEnvelope == null) {
			chkEnvelope = new BCheckBox();
			chkEnvelope.setText("Envelope");
		}
		return chkEnvelope;
	}

	/**
	 * This method initializes tabOperation	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getTabOperation() {
		if (tabOperation == null) {
			GridBagConstraints gridBagConstraints87 = new GridBagConstraints();
			gridBagConstraints87.gridx = 0;
			gridBagConstraints87.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints87.weighty = 1.0D;
			gridBagConstraints87.weightx = 1.0D;
			gridBagConstraints87.anchor = GridBagConstraints.NORTH;
			gridBagConstraints87.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints87.gridy = 1;
			GridBagConstraints gridBagConstraints78 = new GridBagConstraints();
			gridBagConstraints78.gridx = 0;
			gridBagConstraints78.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints78.anchor = GridBagConstraints.NORTH;
			gridBagConstraints78.weightx = 1.0D;
			gridBagConstraints78.insets = new Insets(12, 12, 3, 12);
			gridBagConstraints78.gridy = 0;
			tabOperation = new BPanel();
			tabOperation.setLayout(new GridBagLayout());
			tabOperation.setSize(new Dimension(462, 460));
			tabOperation.add(getGroupPianoroll(), gridBagConstraints78);
			tabOperation.add(getGroupMisc(), gridBagConstraints87);
		}
		return tabOperation;
	}

	/**
	 * This method initializes groupPianoroll	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BGroupBox getGroupPianoroll() {
		if (groupPianoroll == null) {
			GridBagConstraints gridBagConstraints77 = new GridBagConstraints();
			gridBagConstraints77.gridx = 0;
			gridBagConstraints77.anchor = GridBagConstraints.WEST;
			gridBagConstraints77.gridwidth = 2;
			gridBagConstraints77.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints77.gridy = 6;
			GridBagConstraints gridBagConstraints76 = new GridBagConstraints();
			gridBagConstraints76.gridx = 0;
			gridBagConstraints76.anchor = GridBagConstraints.WEST;
			gridBagConstraints76.gridwidth = 2;
			gridBagConstraints76.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints76.gridy = 5;
			GridBagConstraints gridBagConstraints75 = new GridBagConstraints();
			gridBagConstraints75.gridx = 0;
			gridBagConstraints75.anchor = GridBagConstraints.WEST;
			gridBagConstraints75.gridwidth = 2;
			gridBagConstraints75.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints75.gridy = 4;
			GridBagConstraints gridBagConstraints74 = new GridBagConstraints();
			gridBagConstraints74.gridx = 0;
			gridBagConstraints74.gridwidth = 2;
			gridBagConstraints74.anchor = GridBagConstraints.WEST;
			gridBagConstraints74.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints74.gridy = 3;
			GridBagConstraints gridBagConstraints73 = new GridBagConstraints();
			gridBagConstraints73.gridx = 0;
			gridBagConstraints73.gridwidth = 2;
			gridBagConstraints73.anchor = GridBagConstraints.WEST;
			gridBagConstraints73.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints73.gridy = 2;
			GridBagConstraints gridBagConstraints72 = new GridBagConstraints();
			gridBagConstraints72.gridx = 0;
			gridBagConstraints72.weightx = 0.0D;
			gridBagConstraints72.gridwidth = 2;
			gridBagConstraints72.anchor = GridBagConstraints.WEST;
			gridBagConstraints72.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints72.gridy = 1;
			GridBagConstraints gridBagConstraints71 = new GridBagConstraints();
			gridBagConstraints71.fill = GridBagConstraints.NONE;
			gridBagConstraints71.gridy = 0;
			gridBagConstraints71.weightx = 1.0;
			gridBagConstraints71.anchor = GridBagConstraints.WEST;
			gridBagConstraints71.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints71.gridx = 1;
			GridBagConstraints gridBagConstraints70 = new GridBagConstraints();
			gridBagConstraints70.gridx = 0;
			gridBagConstraints70.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints70.anchor = GridBagConstraints.WEST;
			gridBagConstraints70.gridy = 0;
			labelWheelOrder = new BLabel();
			labelWheelOrder.setText("Mouse wheel Rate");
			groupPianoroll = new BGroupBox();
			groupPianoroll.setLayout(new GridBagLayout());
			groupPianoroll.setBorder(BorderFactory.createTitledBorder(null, "Piano Roll", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupPianoroll.add(labelWheelOrder, gridBagConstraints70);
			groupPianoroll.add(getNumericUpDownEx1(), gridBagConstraints71);
			groupPianoroll.add(getChkCursorFix(), gridBagConstraints72);
			groupPianoroll.add(getChkScrollHorizontal(), gridBagConstraints73);
			groupPianoroll.add(getChkKeepLyricInputMode(), gridBagConstraints74);
			groupPianoroll.add(getChkPlayPreviewWhenRightClick(), gridBagConstraints75);
			groupPianoroll.add(getChkCurveSelectingQuantized(), gridBagConstraints76);
			groupPianoroll.add(getChkUseSpaceKeyAsMiddleButtonModifier(), gridBagConstraints77);
		}
		return groupPianoroll;
	}

	/**
	 * This method initializes numericUpDownEx1	
	 * 	
	 * @return javax.swing.BComboBox	
	 */
	private BNumericUpDown getNumericUpDownEx1() {
		if (numericUpDownEx1 == null) {
			numericUpDownEx1 = new BNumericUpDown();
			numericUpDownEx1.setPreferredSize(new Dimension(120, 28));
		}
		return numericUpDownEx1;
	}

	/**
	 * This method initializes chkCursorFix	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkCursorFix() {
		if (chkCursorFix == null) {
			chkCursorFix = new BCheckBox();
			chkCursorFix.setText("Fix Play Cursor to Center");
		}
		return chkCursorFix;
	}

	/**
	 * This method initializes chkScrollHorizontal	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkScrollHorizontal() {
		if (chkScrollHorizontal == null) {
			chkScrollHorizontal = new BCheckBox();
			chkScrollHorizontal.setText("Horizontal Scroll when Mouse wheel");
		}
		return chkScrollHorizontal;
	}

	/**
	 * This method initializes chkKeepLyricInputMode	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkKeepLyricInputMode() {
		if (chkKeepLyricInputMode == null) {
			chkKeepLyricInputMode = new BCheckBox();
			chkKeepLyricInputMode.setText("Keep Lyric Input Mode");
		}
		return chkKeepLyricInputMode;
	}

	/**
	 * This method initializes chkPlayPreviewWhenRightClick	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkPlayPreviewWhenRightClick() {
		if (chkPlayPreviewWhenRightClick == null) {
			chkPlayPreviewWhenRightClick = new BCheckBox();
			chkPlayPreviewWhenRightClick.setText("Play Preview On Right Click");
		}
		return chkPlayPreviewWhenRightClick;
	}

	/**
	 * This method initializes chkCurveSelectingQuantized	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkCurveSelectingQuantized() {
		if (chkCurveSelectingQuantized == null) {
			chkCurveSelectingQuantized = new BCheckBox();
			chkCurveSelectingQuantized.setText("Enable Quantize for Curve Selecting");
		}
		return chkCurveSelectingQuantized;
	}

	/**
	 * This method initializes chkUseSpaceKeyAsMiddleButtonModifier	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkUseSpaceKeyAsMiddleButtonModifier() {
		if (chkUseSpaceKeyAsMiddleButtonModifier == null) {
			chkUseSpaceKeyAsMiddleButtonModifier = new BCheckBox();
			chkUseSpaceKeyAsMiddleButtonModifier.setText("Use space key as Middle button modifier");
		}
		return chkUseSpaceKeyAsMiddleButtonModifier;
	}

	/**
	 * This method initializes groupMisc	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BGroupBox getGroupMisc() {
		if (groupMisc == null) {
			GridBagConstraints gridBagConstraints88 = new GridBagConstraints();
			gridBagConstraints88.gridx = 0;
			gridBagConstraints88.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints88.gridy = 4;
			GridBagConstraints gridBagConstraints136 = new GridBagConstraints();
			gridBagConstraints136.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints136.gridx = 1;
			gridBagConstraints136.gridy = 3;
			gridBagConstraints136.anchor = GridBagConstraints.WEST;
			gridBagConstraints136.gridwidth = 2;
			gridBagConstraints136.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints136.weightx = 1.0;
			GridBagConstraints gridBagConstraints135 = new GridBagConstraints();
			gridBagConstraints135.gridx = 0;
			gridBagConstraints135.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints135.anchor = GridBagConstraints.WEST;
			gridBagConstraints135.gridy = 3;
			labelMtcMidiInPort = new BLabel();
			labelMtcMidiInPort.setText("MTC MIDI In Port Number");
			GridBagConstraints gridBagConstraints86 = new GridBagConstraints();
			gridBagConstraints86.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints86.gridy = 2;
			gridBagConstraints86.weightx = 0.0D;
			gridBagConstraints86.anchor = GridBagConstraints.WEST;
			gridBagConstraints86.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints86.gridwidth = 2;
			gridBagConstraints86.gridx = 1;
			GridBagConstraints gridBagConstraints85 = new GridBagConstraints();
			gridBagConstraints85.gridx = 0;
			gridBagConstraints85.anchor = GridBagConstraints.WEST;
			gridBagConstraints85.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints85.gridy = 2;
			lblMidiInPort = new BLabel();
			lblMidiInPort.setText("MIDI In Port Number");
			GridBagConstraints gridBagConstraints84 = new GridBagConstraints();
			gridBagConstraints84.fill = GridBagConstraints.NONE;
			gridBagConstraints84.gridy = 1;
			gridBagConstraints84.weightx = 1.0D;
			gridBagConstraints84.gridwidth = 2;
			gridBagConstraints84.anchor = GridBagConstraints.WEST;
			gridBagConstraints84.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints84.gridx = 1;
			GridBagConstraints gridBagConstraints83 = new GridBagConstraints();
			gridBagConstraints83.gridx = 0;
			gridBagConstraints83.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints83.anchor = GridBagConstraints.WEST;
			gridBagConstraints83.gridy = 1;
			lblMouseHoverTime = new BLabel();
			lblMouseHoverTime.setText("Waiting Time for Preview");
			GridBagConstraints gridBagConstraints82 = new GridBagConstraints();
			gridBagConstraints82.gridx = 2;
			gridBagConstraints82.anchor = GridBagConstraints.WEST;
			gridBagConstraints82.insets = new Insets(3, 3, 3, 0);
			gridBagConstraints82.weightx = 1.0D;
			gridBagConstraints82.gridy = 0;
			lblMilliSecond = new BLabel();
			lblMilliSecond.setText("milli second");
			GridBagConstraints gridBagConstraints80 = new GridBagConstraints();
			gridBagConstraints80.fill = GridBagConstraints.NONE;
			gridBagConstraints80.gridy = 0;
			gridBagConstraints80.weightx = 1.0;
			gridBagConstraints80.anchor = GridBagConstraints.WEST;
			gridBagConstraints80.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints80.gridx = 1;
			GridBagConstraints gridBagConstraints79 = new GridBagConstraints();
			gridBagConstraints79.gridx = 0;
			gridBagConstraints79.anchor = GridBagConstraints.WEST;
			gridBagConstraints79.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints79.gridy = 0;
			lblMaximumFrameRate = new BLabel();
			lblMaximumFrameRate.setText("Maximum Frame Rate");
			groupMisc = new BGroupBox();
			groupMisc.setLayout(new GridBagLayout());
			groupMisc.setBorder(BorderFactory.createTitledBorder(null, "Misc", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupMisc.add(lblMaximumFrameRate, gridBagConstraints79);
			groupMisc.add(getNumMaximumFrameRate(), gridBagConstraints80);
			groupMisc.add(lblMilliSecond, gridBagConstraints82);
			groupMisc.add(lblMouseHoverTime, gridBagConstraints83);
			groupMisc.add(getNumMouseHoverTime(), gridBagConstraints84);
			groupMisc.add(lblMidiInPort, gridBagConstraints85);
			groupMisc.add(getComboMidiInPortNumber(), gridBagConstraints86);
			groupMisc.add(labelMtcMidiInPort, gridBagConstraints135);
			groupMisc.add(getComboMtcMidiInPortNumber(), gridBagConstraints136);
			groupMisc.add(getChkTranslateRoman(), gridBagConstraints88);
		}
		return groupMisc;
	}

	/**
	 * This method initializes numMaximumFrameRate	
	 * 	
	 * @return javax.swing.BComboBox	
	 */
	private BNumericUpDown getNumMaximumFrameRate() {
		if (numMaximumFrameRate == null) {
			numMaximumFrameRate = new BNumericUpDown();
			numMaximumFrameRate.setPreferredSize(new Dimension(120, 28));
		}
		return numMaximumFrameRate;
	}

	/**
	 * This method initializes numMouseHoverTime	
	 * 	
	 * @return javax.swing.BComboBox	
	 */
	private BNumericUpDown getNumMouseHoverTime() {
		if (numMouseHoverTime == null) {
			numMouseHoverTime = new BNumericUpDown();
			numMouseHoverTime.setPreferredSize(new Dimension(120, 28));
		}
		return numMouseHoverTime;
	}

	/**
	 * This method initializes comboMidiInPortNumber	
	 * 	
	 * @return javax.swing.BComboBox	
	 */
	private BComboBox getComboMidiInPortNumber() {
		if (comboMidiInPortNumber == null) {
			comboMidiInPortNumber = new BComboBox();
			comboMidiInPortNumber.setPreferredSize(new Dimension(200, 27));
		}
		return comboMidiInPortNumber;
	}

	/**
	 * This method initializes tabPlatform	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getTabPlatform() {
		if (tabPlatform == null) {
			GridBagConstraints gridBagConstraints89 = new GridBagConstraints();
			gridBagConstraints89.gridx = 0;
			gridBagConstraints89.weightx = 1.0D;
			gridBagConstraints89.fill = GridBagConstraints.BOTH;
			gridBagConstraints89.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints89.gridy = 3;
			GridBagConstraints gridBagConstraints125 = new GridBagConstraints();
			gridBagConstraints125.gridx = 0;
			gridBagConstraints125.fill = GridBagConstraints.NONE;
			gridBagConstraints125.weighty = 1.0D;
			gridBagConstraints125.gridy = 4;
			jLabel91 = new BLabel();
			jLabel91.setText(" ");
			jLabel91.setPreferredSize(new Dimension(4, 4));
			GridBagConstraints gridBagConstraints106 = new GridBagConstraints();
			gridBagConstraints106.gridx = 0;
			gridBagConstraints106.anchor = GridBagConstraints.NORTH;
			gridBagConstraints106.fill = GridBagConstraints.BOTH;
			gridBagConstraints106.weighty = 0.0D;
			gridBagConstraints106.insets = new Insets(12, 12, 3, 12);
			gridBagConstraints106.weightx = 1.0D;
			gridBagConstraints106.gridy = 2;
			tabPlatform = new BPanel();
			tabPlatform.setLayout(new GridBagLayout());
			tabPlatform.setSize(new Dimension(455, 538));
			tabPlatform.add(getGroupUtauCores(), gridBagConstraints106);
			tabPlatform.add(jLabel91, gridBagConstraints125);
			tabPlatform.add(getGroupWine(), gridBagConstraints89);
		}
		return tabPlatform;
	}

	/**
	 * This method initializes chkTranslateRoman	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkTranslateRoman() {
		if (chkTranslateRoman == null) {
			chkTranslateRoman = new BCheckBox();
			chkTranslateRoman.setText("Translate Roman letters into Kana");
		}
		return chkTranslateRoman;
	}

	/**
	 * This method initializes groupVsti	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BGroupBox getGroupVsti() {
		if (groupVsti == null) {
			GridBagConstraints gridBagConstraints133 = new GridBagConstraints();
			gridBagConstraints133.gridx = 2;
			gridBagConstraints133.insets = new Insets(3, 3, 3, 3);
			gridBagConstraints133.gridy = 2;
			GridBagConstraints gridBagConstraints132 = new GridBagConstraints();
			gridBagConstraints132.gridx = 0;
			gridBagConstraints132.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints132.anchor = GridBagConstraints.WEST;
			gridBagConstraints132.gridy = 2;
			GridBagConstraints gridBagConstraints131 = new GridBagConstraints();
			gridBagConstraints131.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints131.gridx = 1;
			gridBagConstraints131.gridy = 2;
			gridBagConstraints131.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints131.weightx = 1.0;
			lblAquesTone = new BLabel();
			lblAquesTone.setText("AquesTone");
			lblAquesTone.setPreferredSize(new Dimension(72, 29));
			GridBagConstraints gridBagConstraints97 = new GridBagConstraints();
			gridBagConstraints97.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints97.gridy = 1;
			gridBagConstraints97.weightx = 1.0;
			gridBagConstraints97.anchor = GridBagConstraints.WEST;
			gridBagConstraints97.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints97.gridx = 1;
			GridBagConstraints gridBagConstraints96 = new GridBagConstraints();
			gridBagConstraints96.gridx = 0;
			gridBagConstraints96.anchor = GridBagConstraints.WEST;
			gridBagConstraints96.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints96.gridy = 1;
			lblVOCALOID2 = new BLabel();
			lblVOCALOID2.setText("VOCALOID2");
			lblVOCALOID2.setPreferredSize(new Dimension(76, 29));
			GridBagConstraints gridBagConstraints95 = new GridBagConstraints();
			gridBagConstraints95.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints95.gridy = 0;
			gridBagConstraints95.weightx = 1.0;
			gridBagConstraints95.anchor = GridBagConstraints.WEST;
			gridBagConstraints95.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints95.gridx = 1;
			GridBagConstraints gridBagConstraints94 = new GridBagConstraints();
			gridBagConstraints94.gridx = 0;
			gridBagConstraints94.anchor = GridBagConstraints.WEST;
			gridBagConstraints94.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints94.gridy = 0;
			lblVOCALOID1 = new BLabel();
			lblVOCALOID1.setText("VOCALOID1");
			lblVOCALOID1.setPreferredSize(new Dimension(76, 29));
			groupVsti = new BGroupBox();
			groupVsti.setLayout(new GridBagLayout());
			groupVsti.setBorder(BorderFactory.createTitledBorder(null, "VST Instruments", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupVsti.add(lblVOCALOID1, gridBagConstraints94);
			groupVsti.add(getTxtVOCALOID1(), gridBagConstraints95);
			groupVsti.add(lblVOCALOID2, gridBagConstraints96);
			groupVsti.add(getTxtVOCALOID2(), gridBagConstraints97);
			groupVsti.add(lblAquesTone, gridBagConstraints132);
			groupVsti.add(getTxtAquesTone(), gridBagConstraints131);
			groupVsti.add(getBtnAquesTone(), gridBagConstraints133);
		}
		return groupVsti;
	}

	/**
	 * This method initializes txtVOCALOID1	
	 * 	
	 * @return javax.swing.BTextBox	
	 */
	private BTextBox getTxtVOCALOID1() {
		if (txtVOCALOID1 == null) {
			txtVOCALOID1 = new BTextBox();
			txtVOCALOID1.setEditable(false);
		}
		return txtVOCALOID1;
	}

	/**
	 * This method initializes txtVOCALOID2	
	 * 	
	 * @return javax.swing.BTextBox	
	 */
	private BTextBox getTxtVOCALOID2() {
		if (txtVOCALOID2 == null) {
			txtVOCALOID2 = new BTextBox();
			txtVOCALOID2.setEditable(false);
		}
		return txtVOCALOID2;
	}

	/**
	 * This method initializes groupUtauCores	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BGroupBox getGroupUtauCores() {
		if (groupUtauCores == null) {
			GridBagConstraints gridBagConstraints134 = new GridBagConstraints();
			gridBagConstraints134.gridx = 0;
			gridBagConstraints134.anchor = GridBagConstraints.WEST;
			gridBagConstraints134.gridwidth = 3;
			gridBagConstraints134.insets = new Insets(3, 36, 3, 12);
			gridBagConstraints134.gridy = 5;
			labelResamplerWithWine = new BLabel();
			labelResamplerWithWine.setText("Check the box to use Wine");
			GridBagConstraints gridBagConstraints130 = new GridBagConstraints();
			gridBagConstraints130.gridx = 0;
			gridBagConstraints130.gridwidth = 3;
			gridBagConstraints130.anchor = GridBagConstraints.WEST;
			gridBagConstraints130.insets = new Insets(3, 36, 3, 12);
			gridBagConstraints130.gridy = 5;
			lblResampler1 = new BLabel();
			lblResampler1.setText("resampler");
			GridBagConstraints gridBagConstraints129 = new GridBagConstraints();
			gridBagConstraints129.gridx = 0;
			gridBagConstraints129.insets = new Insets(0, 36, 0, 0);
			gridBagConstraints129.gridy = 1;
			labelWavtoolPath = new BLabel();
			labelWavtoolPath.setPreferredSize(new Dimension(49, 29));
			labelWavtoolPath.setText("Path:");
			GridBagConstraints gridBagConstraints128 = new GridBagConstraints();
			gridBagConstraints128.gridx = 3;
			gridBagConstraints128.insets = new Insets(3, 0, 3, 0);
			gridBagConstraints128.fill = GridBagConstraints.BOTH;
			gridBagConstraints128.gridy = 1;
			GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			gridBagConstraints12.fill = GridBagConstraints.BOTH;
			gridBagConstraints12.gridy = 4;
			gridBagConstraints12.weightx = 1.0D;
			gridBagConstraints12.weighty = 1.0;
			gridBagConstraints12.insets = new Insets(3, 36, 3, 3);
			gridBagConstraints12.gridwidth = 2;
			gridBagConstraints12.gridx = 0;
			GridBagConstraints gridBagConstraints101 = new GridBagConstraints();
			gridBagConstraints101.gridx = 3;
			gridBagConstraints101.fill = GridBagConstraints.VERTICAL;
			gridBagConstraints101.insets = new Insets(3, 0, 3, 0);
			gridBagConstraints101.gridy = 4;
			GridBagConstraints gridBagConstraints105 = new GridBagConstraints();
			gridBagConstraints105.gridx = 0;
			gridBagConstraints105.gridwidth = 3;
			gridBagConstraints105.anchor = GridBagConstraints.WEST;
			gridBagConstraints105.insets = new Insets(0, 36, 0, 0);
			gridBagConstraints105.gridy = 2;
			GridBagConstraints gridBagConstraints103 = new GridBagConstraints();
			gridBagConstraints103.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints103.gridy = 1;
			gridBagConstraints103.weightx = 1.0D;
			gridBagConstraints103.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints103.gridx = 1;
			GridBagConstraints gridBagConstraints102 = new GridBagConstraints();
			gridBagConstraints102.gridx = 0;
			gridBagConstraints102.gridwidth = 3;
			gridBagConstraints102.anchor = GridBagConstraints.WEST;
			gridBagConstraints102.insets = new Insets(3, 12, 0, 0);
			gridBagConstraints102.gridy = 0;
			lblWavtool = new BLabel();
			lblWavtool.setText("wavtool");
			lblWavtool.setPreferredSize(new Dimension(49, 29));
			GridBagConstraints gridBagConstraints99 = new GridBagConstraints();
			gridBagConstraints99.gridx = 0;
			gridBagConstraints99.anchor = GridBagConstraints.NORTHWEST;
			gridBagConstraints99.insets = new Insets(12, 12, 0, 0);
			gridBagConstraints99.gridwidth = 3;
			gridBagConstraints99.gridy = 3;
			lblResampler = new BLabel();
			lblResampler.setText("resampler");
			groupUtauCores = new BGroupBox();
			groupUtauCores.setLayout(new GridBagLayout());
			groupUtauCores.setBorder(BorderFactory.createTitledBorder(null, "UTAU Cores", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupUtauCores.add(lblWavtool, gridBagConstraints102);
			groupUtauCores.add(getTxtWavtool(), gridBagConstraints103);
			groupUtauCores.add(getJPanel22(), gridBagConstraints128);
			groupUtauCores.add(lblResampler, gridBagConstraints99);
			groupUtauCores.add(getJScrollPane1(), gridBagConstraints12);
			groupUtauCores.add(getJPanel2(), gridBagConstraints101);
			groupUtauCores.add(getChkWavtoolWithWine(), gridBagConstraints105);
			groupUtauCores.add(labelWavtoolPath, gridBagConstraints129);
			groupUtauCores.add(labelResamplerWithWine, gridBagConstraints134);
		}
		return groupUtauCores;
	}

	/**
	 * This method initializes txtWavtool	
	 * 	
	 * @return javax.swing.BTextBox	
	 */
	private BTextBox getTxtWavtool() {
		if (txtWavtool == null) {
			txtWavtool = new BTextBox();
		}
		return txtWavtool;
	}

	/**
	 * This method initializes btnWavtool	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnWavtool() {
		if (btnWavtool == null) {
			btnWavtool = new BButton();
			btnWavtool.setText("Browse");
			btnWavtool.setPreferredSize(new Dimension(90, 29));
		}
		return btnWavtool;
	}

	/**
	 * This method initializes chkWavtoolWithWine	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkWavtoolWithWine() {
		if (chkWavtoolWithWine == null) {
			chkWavtoolWithWine = new BCheckBox();
			chkWavtoolWithWine.setText("Invoke wavtool with Wine");
		}
		return chkWavtoolWithWine;
	}

	/**
	 * This method initializes tabUtausingers	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getTabUtausingers() {
		if (tabUtausingers == null) {
			GridBagConstraints gridBagConstraints107 = new GridBagConstraints();
			gridBagConstraints107.fill = GridBagConstraints.BOTH;
			gridBagConstraints107.weighty = 1.0;
			gridBagConstraints107.gridx = 0;
			gridBagConstraints107.gridy = 0;
			gridBagConstraints107.gridwidth = 2;
			gridBagConstraints107.insets = new Insets(12, 12, 12, 12);
			gridBagConstraints107.anchor = GridBagConstraints.NORTH;
			gridBagConstraints107.weightx = 1.0;
			GridBagConstraints gridBagConstraints113 = new GridBagConstraints();
			gridBagConstraints113.gridx = 1;
			gridBagConstraints113.anchor = GridBagConstraints.EAST;
			gridBagConstraints113.insets = new Insets(0, 12, 12, 12);
			gridBagConstraints113.gridy = 1;
			GridBagConstraints gridBagConstraints112 = new GridBagConstraints();
			gridBagConstraints112.gridx = 0;
			gridBagConstraints112.anchor = GridBagConstraints.WEST;
			gridBagConstraints112.insets = new Insets(0, 12, 12, 12);
			gridBagConstraints112.gridy = 1;
			tabUtausingers = new BPanel();
			tabUtausingers.setLayout(new GridBagLayout());
			tabUtausingers.setSize(new Dimension(392, 182));
			tabUtausingers.add(getJPanel17(), gridBagConstraints112);
			tabUtausingers.add(getJPanel18(), gridBagConstraints113);
			tabUtausingers.add(getJScrollPane(), gridBagConstraints107);
		}
		return tabUtausingers;
	}

	/**
	 * This method initializes listSingers	
	 * 	
	 * @return javax.swing.JTable	
	 */
	private BListView getListSingers() {
		if (listSingers == null) {
			listSingers = new BListView();
			listSingers.setCheckBoxes(false);
		}
		return listSingers;
	}

	/**
	 * This method initializes btnAdd	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnAdd() {
		if (btnAdd == null) {
			btnAdd = new BButton();
			btnAdd.setText("Add");
			btnAdd.setPreferredSize(new Dimension(85, 29));
		}
		return btnAdd;
	}

	/**
	 * This method initializes btnRemove	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnRemove() {
		if (btnRemove == null) {
			btnRemove = new BButton();
			btnRemove.setText("Remove");
			btnRemove.setPreferredSize(new Dimension(85, 29));
		}
		return btnRemove;
	}

	/**
	 * This method initializes btnUp	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnUp() {
		if (btnUp == null) {
			btnUp = new BButton();
			btnUp.setText("Up");
			btnUp.setPreferredSize(new Dimension(75, 29));
		}
		return btnUp;
	}

	/**
	 * This method initializes btnDown	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnDown() {
		if (btnDown == null) {
			btnDown = new BButton();
			btnDown.setText("Down");
			btnDown.setPreferredSize(new Dimension(75, 29));
		}
		return btnDown;
	}

	/**
	 * This method initializes jPanel17	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getJPanel17() {
		if (jPanel17 == null) {
			GridBagConstraints gridBagConstraints109 = new GridBagConstraints();
			gridBagConstraints109.anchor = GridBagConstraints.WEST;
			gridBagConstraints109.gridy = 0;
			gridBagConstraints109.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints109.gridx = 1;
			GridBagConstraints gridBagConstraints108 = new GridBagConstraints();
			gridBagConstraints108.anchor = GridBagConstraints.WEST;
			gridBagConstraints108.gridy = 0;
			gridBagConstraints108.gridx = 0;
			jPanel17 = new BPanel();
			jPanel17.setLayout(new GridBagLayout());
			jPanel17.add(getBtnAdd(), gridBagConstraints108);
			jPanel17.add(getBtnRemove(), gridBagConstraints109);
		}
		return jPanel17;
	}

	/**
	 * This method initializes jPanel18	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getJPanel18() {
		if (jPanel18 == null) {
			GridBagConstraints gridBagConstraints111 = new GridBagConstraints();
			gridBagConstraints111.anchor = GridBagConstraints.EAST;
			gridBagConstraints111.gridy = 0;
			gridBagConstraints111.gridx = 1;
			GridBagConstraints gridBagConstraints110 = new GridBagConstraints();
			gridBagConstraints110.anchor = GridBagConstraints.EAST;
			gridBagConstraints110.gridy = 0;
			gridBagConstraints110.insets = new Insets(0, 0, 0, 12);
			gridBagConstraints110.gridx = 0;
			jPanel18 = new BPanel();
			jPanel18.setLayout(new GridBagLayout());
			jPanel18.add(getBtnUp(), gridBagConstraints110);
			jPanel18.add(getBtnDown(), gridBagConstraints111);
		}
		return jPanel18;
	}

	/**
	 * This method initializes tabFile	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getTabFile() {
		if (tabFile == null) {
			GridBagConstraints gridBagConstraints118 = new GridBagConstraints();
			gridBagConstraints118.gridx = 0;
			gridBagConstraints118.anchor = GridBagConstraints.NORTH;
			gridBagConstraints118.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints118.weighty = 1.0D;
			gridBagConstraints118.weightx = 1.0D;
			gridBagConstraints118.gridheight = 2;
			gridBagConstraints118.gridy = 0;
			lblAutoBackupInterval = new BLabel();
			lblAutoBackupInterval.setText("interval");
			tabFile = new BPanel();
			tabFile.setLayout(new GridBagLayout());
			tabFile.add(getJPanel20(), gridBagConstraints118);
		}
		return tabFile;
	}

	/**
	 * This method initializes chkAutoBackup	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkAutoBackup() {
		if (chkAutoBackup == null) {
			chkAutoBackup = new BCheckBox();
			chkAutoBackup.setText("Automatical Backup");
		}
		return chkAutoBackup;
	}

	/**
	 * This method initializes lblAutoBackupMinutes	
	 * 	
	 * @return javax.swing.BLabel	
	 */
	private BLabel getLblAutoBackupMinutes() {
		if (lblAutoBackupMinutes == null) {
			lblAutoBackupMinutes = new BLabel();
			lblAutoBackupMinutes.setText("minutes");
		}
		return lblAutoBackupMinutes;
	}

	/**
	 * This method initializes jPanel20	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getJPanel20() {
		if (jPanel20 == null) {
			GridBagConstraints gridBagConstraints143 = new GridBagConstraints();
			gridBagConstraints143.gridx = 0;
			gridBagConstraints143.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints143.gridwidth = 4;
			gridBagConstraints143.anchor = GridBagConstraints.WEST;
			gridBagConstraints143.gridy = 1;
			GridBagConstraints gridBagConstraints121 = new GridBagConstraints();
			gridBagConstraints121.fill = GridBagConstraints.NONE;
			gridBagConstraints121.gridy = 0;
			gridBagConstraints121.weightx = 1.0;
			gridBagConstraints121.insets = new Insets(3, 6, 3, 12);
			gridBagConstraints121.gridx = 2;
			GridBagConstraints gridBagConstraints117 = new GridBagConstraints();
			gridBagConstraints117.anchor = GridBagConstraints.WEST;
			gridBagConstraints117.gridy = 0;
			gridBagConstraints117.weightx = 1.0D;
			gridBagConstraints117.gridx = 3;
			GridBagConstraints gridBagConstraints116 = new GridBagConstraints();
			gridBagConstraints116.anchor = GridBagConstraints.WEST;
			gridBagConstraints116.insets = new Insets(3, 6, 3, 6);
			gridBagConstraints116.gridx = 2;
			gridBagConstraints116.gridy = 0;
			gridBagConstraints116.weightx = 0.0D;
			gridBagConstraints116.weighty = 1.0;
			gridBagConstraints116.fill = GridBagConstraints.NONE;
			GridBagConstraints gridBagConstraints115 = new GridBagConstraints();
			gridBagConstraints115.anchor = GridBagConstraints.WEST;
			gridBagConstraints115.gridx = 1;
			gridBagConstraints115.gridy = 0;
			gridBagConstraints115.insets = new Insets(0, 24, 0, 0);
			GridBagConstraints gridBagConstraints114 = new GridBagConstraints();
			gridBagConstraints114.anchor = GridBagConstraints.WEST;
			gridBagConstraints114.gridx = 0;
			gridBagConstraints114.gridy = 0;
			gridBagConstraints114.insets = new Insets(3, 12, 3, 0);
			jPanel20 = new BPanel();
			jPanel20.setLayout(new GridBagLayout());
			jPanel20.add(getChkAutoBackup(), gridBagConstraints114);
			jPanel20.add(lblAutoBackupInterval, gridBagConstraints115);
			jPanel20.add(getLblAutoBackupMinutes(), gridBagConstraints117);
			jPanel20.add(getNumAutoBackupInterval(), gridBagConstraints121);
			jPanel20.add(getChkKeepProjectCache(), gridBagConstraints143);
		}
		return jPanel20;
	}

	/**
	 * This method initializes panelMiddle	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getPanelMiddle() {
		if (panelMiddle == null) {
			GridBagConstraints gridBagConstraints127 = new GridBagConstraints();
			gridBagConstraints127.gridx = 0;
			gridBagConstraints127.weightx = 1.0D;
			gridBagConstraints127.fill = GridBagConstraints.NONE;
			gridBagConstraints127.gridy = 0;
			lblPlatform1 = new BLabel();
			lblPlatform1.setText("");
			lblPlatform1.setPreferredSize(new Dimension(4, 4));
			GridBagConstraints gridBagConstraints311 = new GridBagConstraints();
			gridBagConstraints311.insets = new Insets(0, 0, 0, 0);
			gridBagConstraints311.gridy = 0;
			gridBagConstraints311.gridx = 1;
			GridBagConstraints gridBagConstraints211 = new GridBagConstraints();
			gridBagConstraints211.insets = new Insets(0, 0, 0, 12);
			gridBagConstraints211.gridy = 0;
			gridBagConstraints211.gridx = 2;
			panelMiddle = new BPanel();
			panelMiddle.setLayout(new GridBagLayout());
			panelMiddle.add(getBtnOK(), gridBagConstraints211);
			panelMiddle.add(getBtnCancel(), gridBagConstraints311);
			panelMiddle.add(lblPlatform1, gridBagConstraints127);
		}
		return panelMiddle;
	}

	/**
	 * This method initializes btnOK	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnOK() {
		if (btnOK == null) {
			btnOK = new BButton();
			btnOK.setText("OK");
			btnOK.setPreferredSize(new Dimension(100, 29));
		}
		return btnOK;
	}

	/**
	 * This method initializes btnCancel	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnCancel() {
		if (btnCancel == null) {
			btnCancel = new BButton();
			btnCancel.setText("Cancel");
			btnCancel.setPreferredSize(new Dimension(100, 29));
		}
		return btnCancel;
	}

	/**
	 * This method initializes jPanel5	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getJPanel5() {
		if (jPanel5 == null) {
			GridBagConstraints gridBagConstraints147 = new GridBagConstraints();
			gridBagConstraints147.gridx = 0;
			gridBagConstraints147.weightx = 1.0D;
			gridBagConstraints147.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints147.gridy = 2;
			GridBagConstraints gridBagConstraints120 = new GridBagConstraints();
			gridBagConstraints120.gridx = 0;
			gridBagConstraints120.fill = GridBagConstraints.BOTH;
			gridBagConstraints120.weightx = 1.0D;
			gridBagConstraints120.weighty = 1.0D;
			gridBagConstraints120.gridy = 0;
			GridBagConstraints gridBagConstraints119 = new GridBagConstraints();
			gridBagConstraints119.gridx = 0;
			gridBagConstraints119.anchor = GridBagConstraints.EAST;
			gridBagConstraints119.insets = new Insets(12, 0, 12, 0);
			gridBagConstraints119.weightx = 1.0D;
			gridBagConstraints119.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints119.gridy = 1;
			jPanel5 = new BPanel();
			jPanel5.setLayout(new GridBagLayout());
			jPanel5.add(getPanelTop(), gridBagConstraints120);
			jPanel5.add(getPanelMiddle(), gridBagConstraints119);
			jPanel5.add(getPanelBottom(), gridBagConstraints147);
		}
		return jPanel5;
	}

	/**
	 * This method initializes panelTop	
	 * 	
	 * @return javax.swing.BPanel	
	 */
	private BPanel getPanelTop() {
		if (panelTop == null) {
			panelTop = new BPanel();
			panelTop.setLayout(new GridBagLayout());
		}
		return panelTop;
	}

	/**
	 * This method initializes numAutoBackupInterval	
	 * 	
	 * @return javax.swing.BTextBox	
	 */
	private BNumericUpDown getNumAutoBackupInterval() {
		if (numAutoBackupInterval == null) {
			numAutoBackupInterval = new BNumericUpDown();
            numAutoBackupInterval.setDecimalPlaces(0);
            numAutoBackupInterval.setFloatValue(1.0F);
            numAutoBackupInterval.setMaximum(100.0F);
			numAutoBackupInterval.setMinimum(1.0F);
            numAutoBackupInterval.setPreferredSize(new Dimension(69, 28));
		}
		return numAutoBackupInterval;
	}

    /**
     * This method initializes chkChasePastEvent	
     * 	
     * @return javax.swing.JCheckBox	
     */
    private JCheckBox getChkChasePastEvent() {
        if (chkChasePastEvent == null) {
            chkChasePastEvent = new JCheckBox();
            chkChasePastEvent.setText("Chase Event");
        }
        return chkChasePastEvent;
    }

    /**
     * This method initializes txtAquesTone	
     * 	
     * @return org.kbinani.windows.forms.BTextBox	
     */
    private BTextBox getTxtAquesTone() {
        if (txtAquesTone == null) {
            txtAquesTone = new BTextBox();
        }
        return txtAquesTone;
    }

    /**
     * This method initializes btnAquesTone	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getBtnAquesTone() {
        if (btnAquesTone == null) {
            btnAquesTone = new BButton();
            btnAquesTone.setPreferredSize(new Dimension(41, 29));
            btnAquesTone.setText("...");
        }
        return btnAquesTone;
    }

    /**
     * This method initializes numBuffer	
     * 	
     * @return org.kbinani.windows.forms.BNumericUpDown	
     */
    private BNumericUpDown getNumBuffer() {
        if (numBuffer == null) {
            numBuffer = new BNumericUpDown();
            numBuffer.setPreferredSize(new Dimension(68, 28));
            numBuffer.setDecimalPlaces(0);
            numBuffer.setMinimum(100.0F);
            numBuffer.setFloatValue(100.0F);
            numBuffer.setMaximum(1000.0F);
        }
        return numBuffer;
    }

    /**
     * This method initializes comboMtcMidiInPortNumber	
     * 	
     * @return org.kbinani.windows.forms.BComboBox	
     */
    private BComboBox getComboMtcMidiInPortNumber() {
        if (comboMtcMidiInPortNumber == null) {
            comboMtcMidiInPortNumber = new BComboBox();
        }
        return comboMtcMidiInPortNumber;
    }

    /**
     * This method initializes tabSingingSynth	
     * 	
     * @return org.kbinani.windows.forms.BPanel	
     */
    private BPanel getTabSingingSynth() {
        if (tabSingingSynth == null) {
            GridBagConstraints gridBagConstraints145 = new GridBagConstraints();
            gridBagConstraints145.gridx = 0;
            gridBagConstraints145.weighty = 1.0D;
            gridBagConstraints145.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints145.insets = new Insets(3, 12, 3, 12);
            gridBagConstraints145.anchor = GridBagConstraints.NORTHWEST;
            gridBagConstraints145.gridy = 2;
            GridBagConstraints gridBagConstraints137 = new GridBagConstraints();
            gridBagConstraints137.gridx = 0;
            gridBagConstraints137.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints137.insets = new Insets(3, 12, 3, 12);
            gridBagConstraints137.anchor = GridBagConstraints.NORTH;
            gridBagConstraints137.weighty = 0.0D;
            gridBagConstraints137.gridy = 1;
            GridBagConstraints gridBagConstraints98 = new GridBagConstraints();
            gridBagConstraints98.anchor = GridBagConstraints.NORTH;
            gridBagConstraints98.insets = new Insets(3, 12, 3, 12);
            gridBagConstraints98.gridx = 0;
            gridBagConstraints98.gridy = 0;
            gridBagConstraints98.weightx = 1.0D;
            gridBagConstraints98.fill = GridBagConstraints.HORIZONTAL;
            tabSingingSynth = new BPanel();
            tabSingingSynth.setLayout(new GridBagLayout());
            tabSingingSynth.setSize(new Dimension(422, 379));
            tabSingingSynth.add(getGroupVsti(), gridBagConstraints98);
            tabSingingSynth.add(getGroupSynthesizerDll(), gridBagConstraints137);
            tabSingingSynth.add(getGroupDefaultSynthesizer(), gridBagConstraints145);
        }
        return tabSingingSynth;
    }

    /**
     * This method initializes groupSynthesizerDll	
     * 	
     * @return org.kbinani.windows.forms.BGroupBox	
     */
    private BGroupBox getGroupSynthesizerDll() {
        if (groupSynthesizerDll == null) {
            GridBagConstraints gridBagConstraints142 = new GridBagConstraints();
            gridBagConstraints142.gridy = 4;
            gridBagConstraints142.anchor = GridBagConstraints.WEST;
            gridBagConstraints142.insets = new Insets(0, 12, 0, 0);
            gridBagConstraints142.gridx = 0;
            GridBagConstraints gridBagConstraints141 = new GridBagConstraints();
            gridBagConstraints141.gridy = 3;
            gridBagConstraints141.anchor = GridBagConstraints.WEST;
            gridBagConstraints141.insets = new Insets(0, 12, 0, 0);
            gridBagConstraints141.gridx = 0;
            GridBagConstraints gridBagConstraints139 = new GridBagConstraints();
            gridBagConstraints139.gridx = 0;
            gridBagConstraints139.anchor = GridBagConstraints.WEST;
            gridBagConstraints139.weightx = 1.0D;
            gridBagConstraints139.insets = new Insets(0, 12, 0, 0);
            gridBagConstraints139.gridy = 1;
            groupSynthesizerDll = new BGroupBox();
            groupSynthesizerDll.setLayout(new GridBagLayout());
            groupSynthesizerDll.setBorder(BorderFactory.createTitledBorder(null, "Synthesizer DLL Usage", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
            groupSynthesizerDll.setTitle("Synthesizer DLL Usage");
            groupSynthesizerDll.add(getChkLoadVocaloid1(), gridBagConstraints139);
            groupSynthesizerDll.add(getChkLoadVocaloid2(), gridBagConstraints141);
            groupSynthesizerDll.add(getChkLoadAquesTone(), gridBagConstraints142);
        }
        return groupSynthesizerDll;
    }

    /**
     * This method initializes chkLoadVocaloid1	
     * 	
     * @return org.kbinani.windows.forms.BCheckBox	
     */
    private BCheckBox getChkLoadVocaloid1() {
        if (chkLoadVocaloid1 == null) {
            chkLoadVocaloid1 = new BCheckBox();
            chkLoadVocaloid1.setText("VOCALOID1");
            chkLoadVocaloid1.setActionCommand("VOCALOID1");
        }
        return chkLoadVocaloid1;
    }

    /**
     * This method initializes chkLoadVocaloid2	
     * 	
     * @return org.kbinani.windows.forms.BCheckBox	
     */
    private BCheckBox getChkLoadVocaloid2() {
        if (chkLoadVocaloid2 == null) {
            chkLoadVocaloid2 = new BCheckBox();
            chkLoadVocaloid2.setText("VOCALOID2");
        }
        return chkLoadVocaloid2;
    }

    /**
     * This method initializes chkLoadAquesTone	
     * 	
     * @return org.kbinani.windows.forms.BCheckBox	
     */
    private BCheckBox getChkLoadAquesTone() {
        if (chkLoadAquesTone == null) {
            chkLoadAquesTone = new BCheckBox();
            chkLoadAquesTone.setText("AquesTone");
        }
        return chkLoadAquesTone;
    }

    /**
     * This method initializes chkKeepProjectCache	
     * 	
     * @return org.kbinani.windows.forms.BCheckBox	
     */
    private BCheckBox getChkKeepProjectCache() {
        if (chkKeepProjectCache == null) {
            chkKeepProjectCache = new BCheckBox();
            chkKeepProjectCache.setText("Keep Project Cache");
        }
        return chkKeepProjectCache;
    }

    /**
     * This method initializes chkEnableAutoVibrato	
     * 	
     * @return org.kbinani.windows.forms.BCheckBox	
     */
    private BCheckBox getChkEnableAutoVibrato() {
        if (chkEnableAutoVibrato == null) {
            chkEnableAutoVibrato = new BCheckBox();
            chkEnableAutoVibrato.setText("Enable Automatic Vibrato");
        }
        return chkEnableAutoVibrato;
    }

    /**
     * This method initializes jPanel11	
     * 	
     * @return org.kbinani.windows.forms.BPanel	
     */
    private BPanel getJPanel11() {
        if (jPanel11 == null) {
            GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
            gridBagConstraints31.gridx = 0;
            gridBagConstraints31.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints31.weighty = 1.0D;
            gridBagConstraints31.anchor = GridBagConstraints.NORTHWEST;
            gridBagConstraints31.insets = new Insets(3, 48, 3, 12);
            gridBagConstraints31.gridy = 2;
            GridBagConstraints gridBagConstraints19 = new GridBagConstraints();
            gridBagConstraints19.gridx = 0;
            gridBagConstraints19.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints19.insets = new Insets(3, 48, 3, 12);
            gridBagConstraints19.gridy = 1;
            GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
            gridBagConstraints13.gridx = 0;
            gridBagConstraints13.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints13.weightx = 1.0D;
            gridBagConstraints13.gridy = 0;
            jPanel11 = new BPanel();
            jPanel11.setLayout(new GridBagLayout());
            jPanel11.add(getJPanel21(), gridBagConstraints13);
            jPanel11.add(getGroupVocaloidEditorCompatible(), gridBagConstraints19);
            jPanel11.add(getGroupUserDefined(), gridBagConstraints31);
        }
        return jPanel11;
    }

    /**
     * This method initializes groupVocaloidEditorCompatible	
     * 	
     * @return org.kbinani.windows.forms.BGroupBox	
     */
    private BGroupBox getGroupVocaloidEditorCompatible() {
        if (groupVocaloidEditorCompatible == null) {
            GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
            gridBagConstraints9.gridx = 1;
            gridBagConstraints9.weightx = 1.0D;
            gridBagConstraints9.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints9.gridy = 1;
            GridBagConstraints gridBagConstraints144 = new GridBagConstraints();
            gridBagConstraints144.anchor = GridBagConstraints.WEST;
            gridBagConstraints144.gridx = 1;
            gridBagConstraints144.gridy = 1;
            gridBagConstraints144.insets = new Insets(3, 12, 3, 0);
            groupVocaloidEditorCompatible = new BGroupBox();
            groupVocaloidEditorCompatible.setLayout(new GridBagLayout());
            groupVocaloidEditorCompatible.setBorder(BorderFactory.createTitledBorder(null, "Auto Vibrato Settings", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
            groupVocaloidEditorCompatible.setTitle("VOCALOID Editor Compatible");
            groupVocaloidEditorCompatible.add(getJPanel31(), gridBagConstraints144);
            groupVocaloidEditorCompatible.add(getJPanel4(), gridBagConstraints9);
        }
        return groupVocaloidEditorCompatible;
    }

    /**
     * This method initializes jPanel31	
     * 	
     * @return org.kbinani.windows.forms.BPanel	
     */
    private BPanel getJPanel31() {
        if (jPanel31 == null) {
            jPanel31 = new BPanel();
            jPanel31.setLayout(new GridBagLayout());
        }
        return jPanel31;
    }

    /**
     * This method initializes jPanel41	
     * 	
     * @return org.kbinani.windows.forms.BPanel	
     */
    private BPanel getJPanel41() {
        if (jPanel41 == null) {
            GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
            gridBagConstraints11.fill = GridBagConstraints.NONE;
            gridBagConstraints11.gridy = 0;
            gridBagConstraints11.weightx = 1.0;
            gridBagConstraints11.anchor = GridBagConstraints.WEST;
            gridBagConstraints11.insets = new Insets(3, 12, 3, 0);
            gridBagConstraints11.gridx = 1;
            GridBagConstraints gridBagConstraints151 = new GridBagConstraints();
            gridBagConstraints151.gridx = 0;
            gridBagConstraints151.insets = new Insets(0, 12, 0, 0);
            gridBagConstraints151.gridy = 0;
            lblAutoVibratoTypeCustom = new BLabel();
            lblAutoVibratoTypeCustom.setText("Vibrato Type: Custom");
            lblAutoVibratoTypeCustom.setEnabled(false);
            jPanel41 = new BPanel();
            jPanel41.setLayout(new GridBagLayout());
            jPanel41.add(lblAutoVibratoTypeCustom, gridBagConstraints151);
            jPanel41.add(getComboAutoVibratoTypeCustom(), gridBagConstraints11);
        }
        return jPanel41;
    }

    /**
     * This method initializes jPanel21	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel21() {
        if (jPanel21 == null) {
            GridBagConstraints gridBagConstraints1291 = new GridBagConstraints();
            gridBagConstraints1291.anchor = GridBagConstraints.WEST;
            gridBagConstraints1291.gridx = 1;
            gridBagConstraints1291.gridy = 0;
            gridBagConstraints1291.weightx = 1.0D;
            gridBagConstraints1291.insets = new Insets(3, 12, 3, 0);
            GridBagConstraints gridBagConstraints1281 = new GridBagConstraints();
            gridBagConstraints1281.anchor = GridBagConstraints.WEST;
            gridBagConstraints1281.gridx = 0;
            gridBagConstraints1281.gridy = 0;
            gridBagConstraints1281.insets = new Insets(3, 24, 3, 0);
            jPanel21 = new JPanel();
            jPanel21.setLayout(new GridBagLayout());
            jPanel21.add(getRadioVocaloidEditorCompatible(), gridBagConstraints1281);
            jPanel21.add(getRadioUserDefined(), gridBagConstraints1291);
        }
        return jPanel21;
    }

    /**
     * This method initializes radioVocaloidEditorCompatible	
     * 	
     * @return org.kbinani.windows.forms.BRadioButton	
     */
    private BRadioButton getRadioVocaloidEditorCompatible() {
        if (radioVocaloidEditorCompatible == null) {
            radioVocaloidEditorCompatible = new BRadioButton();
            radioVocaloidEditorCompatible.setText("VOCALOID Editor Compatible");
            radioVocaloidEditorCompatible.setSelected(true);
        }
        return radioVocaloidEditorCompatible;
    }

    /**
     * This method initializes radioUserDefined	
     * 	
     * @return org.kbinani.windows.forms.BRadioButton	
     */
    private BRadioButton getRadioUserDefined() {
        if (radioUserDefined == null) {
            radioUserDefined = new BRadioButton();
            radioUserDefined.setText("User Defined");
        }
        return radioUserDefined;
    }

    /**
     * This method initializes groupDefaultSynthesizer	
     * 	
     * @return org.kbinani.windows.forms.BGroupBox	
     */
    private BGroupBox getGroupDefaultSynthesizer() {
        if (groupDefaultSynthesizer == null) {
            GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
            gridBagConstraints51.fill = GridBagConstraints.NONE;
            gridBagConstraints51.gridy = 0;
            gridBagConstraints51.weightx = 1.0;
            gridBagConstraints51.insets = new Insets(3, 12, 3, 12);
            gridBagConstraints51.anchor = GridBagConstraints.NORTHWEST;
            gridBagConstraints51.gridx = 0;
            groupDefaultSynthesizer = new BGroupBox();
            groupDefaultSynthesizer.setLayout(new GridBagLayout());
            groupDefaultSynthesizer.setBorder(BorderFactory.createTitledBorder(null, "VST Instruments", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
            groupDefaultSynthesizer.setTitle("Default Synthesizer");
            groupDefaultSynthesizer.add(getComboDefaultSynthesizer(), gridBagConstraints51);
        }
        return groupDefaultSynthesizer;
    }

    /**
     * This method initializes comboDefaultSynthesizer	
     * 	
     * @return org.kbinani.windows.forms.BComboBox	
     */
    private BComboBox getComboDefaultSynthesizer() {
        if (comboDefaultSynthesizer == null) {
            comboDefaultSynthesizer = new BComboBox();
            comboDefaultSynthesizer.setPreferredSize(new Dimension(222, 27));
        }
        return comboDefaultSynthesizer;
    }

    /**
     * This method initializes comboAutoVibratoTypeCustom	
     * 	
     * @return org.kbinani.windows.forms.BComboBox	
     */
    private BComboBox getComboAutoVibratoTypeCustom() {
        if (comboAutoVibratoTypeCustom == null) {
            comboAutoVibratoTypeCustom = new BComboBox();
            comboAutoVibratoTypeCustom.setPreferredSize(new Dimension(101, 27));
            comboAutoVibratoTypeCustom.setEnabled(false);
        }
        return comboAutoVibratoTypeCustom;
    }

    /**
     * This method initializes jPanel	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel2() {
        if (jPanel == null) {
            GridBagConstraints gridBagConstraints126 = new GridBagConstraints();
            gridBagConstraints126.gridx = 0;
            gridBagConstraints126.weighty = 1.0D;
            gridBagConstraints126.gridy = 2;
            jLabel911 = new BLabel();
            jLabel911.setText(" ");
            jLabel911.setPreferredSize(new Dimension(4, 4));
            GridBagConstraints gridBagConstraints100 = new GridBagConstraints();
            gridBagConstraints100.gridx = 0;
            gridBagConstraints100.insets = new Insets(3, 3, 3, 3);
            gridBagConstraints100.gridy = 4;
            GridBagConstraints gridBagConstraints34 = new GridBagConstraints();
            gridBagConstraints34.gridx = 0;
            gridBagConstraints34.insets = new Insets(3, 3, 3, 3);
            gridBagConstraints34.gridy = 3;
            GridBagConstraints gridBagConstraints33 = new GridBagConstraints();
            gridBagConstraints33.gridx = 0;
            gridBagConstraints33.insets = new Insets(3, 3, 3, 3);
            gridBagConstraints33.gridy = 1;
            GridBagConstraints gridBagConstraints20 = new GridBagConstraints();
            gridBagConstraints20.gridx = 0;
            gridBagConstraints20.insets = new Insets(3, 3, 3, 3);
            gridBagConstraints20.fill = GridBagConstraints.NONE;
            gridBagConstraints20.gridy = 0;
            jPanel = new JPanel();
            jPanel.setLayout(new GridBagLayout());
            jPanel.setPreferredSize(new Dimension(120, 156));
            jPanel.add(getButtonResamplerAdd(), gridBagConstraints20);
            jPanel.add(getButtonResamplerRemove(), gridBagConstraints33);
            jPanel.add(getButtonResamplerUp(), gridBagConstraints34);
            jPanel.add(getButtonResamplerDown(), gridBagConstraints100);
            jPanel.add(jLabel911, gridBagConstraints126);
        }
        return jPanel;
    }

    /**
     * This method initializes listResampler	
     * 	
     * @return javax.swing.JList	
     */
    private BListView getListResampler() {
        if (listResampler == null) {
            listResampler = new BListView();
        }
        return listResampler;
    }

    /**
     * This method initializes buttonResamplerAdd	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getButtonResamplerAdd() {
        if (buttonResamplerAdd == null) {
            buttonResamplerAdd = new BButton();
            buttonResamplerAdd.setPreferredSize(new Dimension(90, 29));
            buttonResamplerAdd.setText("Add");
        }
        return buttonResamplerAdd;
    }

    /**
     * This method initializes buttonResamplerRemove	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getButtonResamplerRemove() {
        if (buttonResamplerRemove == null) {
            buttonResamplerRemove = new BButton();
            buttonResamplerRemove.setText("Remove");
            buttonResamplerRemove.setPreferredSize(new Dimension(90, 29));
        }
        return buttonResamplerRemove;
    }

    /**
     * This method initializes buttonResamplerUp	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getButtonResamplerUp() {
        if (buttonResamplerUp == null) {
            buttonResamplerUp = new BButton();
            buttonResamplerUp.setPreferredSize(new Dimension(90, 29));
            buttonResamplerUp.setText("Up");
        }
        return buttonResamplerUp;
    }

    /**
     * This method initializes buttonResamplerDown	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getButtonResamplerDown() {
        if (buttonResamplerDown == null) {
            buttonResamplerDown = new BButton();
            buttonResamplerDown.setPreferredSize(new Dimension(90, 29));
            buttonResamplerDown.setText("Down");
        }
        return buttonResamplerDown;
    }

    /**
     * This method initializes txtAutoVibratoThresholdLength	
     * 	
     * @return org.kbinani.windows.forms.BTextBox	
     */
    private BTextBox getTxtAutoVibratoThresholdLength() {
        if (txtAutoVibratoThresholdLength == null) {
            txtAutoVibratoThresholdLength = new BTextBox();
        }
        return txtAutoVibratoThresholdLength;
    }

    /**
     * This method initializes jScrollPane	
     * 	
     * @return javax.swing.JScrollPane	
     */
    private JScrollPane getJScrollPane() {
        if (jScrollPane == null) {
            jScrollPane = new JScrollPane();
            jScrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
            jScrollPane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
            jScrollPane.setViewportView(getListSingers());
        }
        return jScrollPane;
    }

    /**
     * This method initializes jScrollPane1	
     * 	
     * @return javax.swing.JScrollPane	
     */
    private JScrollPane getJScrollPane1() {
        if (jScrollPane1 == null) {
            jScrollPane1 = new JScrollPane();
            jScrollPane1.setBorder(BorderFactory.createLineBorder(Color.gray, 1));
            jScrollPane1.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
            jScrollPane1.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
            jScrollPane1.setViewportView(getListResampler());
        }
        return jScrollPane1;
    }

    /**
     * This method initializes jPanel2	
     * 	
     * @return javax.swing.JPanel	
     */
    private JPanel getJPanel22() {
        if (jPanel2 == null) {
            GridBagConstraints gridBagConstraints104 = new GridBagConstraints();
            gridBagConstraints104.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints104.gridx = 0;
            gridBagConstraints104.gridy = 0;
            gridBagConstraints104.weightx = 0.0D;
            gridBagConstraints104.insets = new Insets(3, 3, 3, 3);
            jPanel2 = new JPanel();
            jPanel2.setLayout(new GridBagLayout());
            jPanel2.setPreferredSize(new Dimension(103, 35));
            jPanel2.setBorder(BorderFactory.createEmptyBorder(0, 0, 0, 0));
            jPanel2.add(getBtnWavtool(), gridBagConstraints104);
        }
        return jPanel2;
    }

    /**
     * This method initializes panelBottom	
     * 	
     * @return org.kbinani.windows.forms.BPanel	
     */
    private BPanel getPanelBottom() {
        if (panelBottom == null) {
            GridBagConstraints gridBagConstraints1271 = new GridBagConstraints();
            gridBagConstraints1271.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints1271.gridy = 0;
            gridBagConstraints1271.weightx = 1.0D;
            gridBagConstraints1271.insets = new Insets(0, 5, 0, 20);
            gridBagConstraints1271.gridx = 0;
            labelStatus = new BLabel();
            labelStatus.setPreferredSize(new Dimension(10, 18));
            labelStatus.setText("");
            panelBottom = new BPanel();
            panelBottom.setLayout(new GridBagLayout());
            panelBottom.setPreferredSize(new Dimension(10, 18));
            panelBottom.add(labelStatus, gridBagConstraints1271);
        }
        return panelBottom;
    }

    /**
     * This method initializes groupWine	
     * 	
     * @return org.kbinani.windows.forms.BGroupBox	
     */
    private BGroupBox getGroupWine() {
        if (groupWine == null) {
            GridBagConstraints gridBagConstraints154 = new GridBagConstraints();
            gridBagConstraints154.anchor = GridBagConstraints.WEST;
            gridBagConstraints154.gridy = 2;
            gridBagConstraints154.gridwidth = 2;
            gridBagConstraints154.insets = new Insets(0, 36, 0, 0);
            gridBagConstraints154.gridx = 0;
            GridBagConstraints gridBagConstraints156 = new GridBagConstraints();
            gridBagConstraints156.gridx = 3;
            gridBagConstraints156.weighty = 0.0D;
            gridBagConstraints156.gridy = 4;
            GridBagConstraints gridBagConstraints153 = new GridBagConstraints();
            gridBagConstraints153.gridx = 0;
            gridBagConstraints153.gridwidth = 2;
            gridBagConstraints153.fill = GridBagConstraints.BOTH;
            gridBagConstraints153.insets = new Insets(0, 36, 0, 0);
            gridBagConstraints153.gridheight = 2;
            gridBagConstraints153.gridy = 3;
            GridBagConstraints gridBagConstraints148 = new GridBagConstraints();
            gridBagConstraints148.gridx = 0;
            gridBagConstraints148.anchor = GridBagConstraints.WEST;
            gridBagConstraints148.insets = new Insets(0, 12, 0, 0);
            gridBagConstraints148.gridy = 1;
            labelWineTop = new BLabel();
            labelWineTop.setText("WINETOP");
            GridBagConstraints gridBagConstraints93 = new GridBagConstraints();
            gridBagConstraints93.gridx = 3;
            gridBagConstraints93.gridy = 0;
            GridBagConstraints gridBagConstraints92 = new GridBagConstraints();
            gridBagConstraints92.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints92.gridy = 0;
            gridBagConstraints92.weightx = 1.0;
            gridBagConstraints92.insets = new Insets(0, 6, 0, 0);
            gridBagConstraints92.gridx = 1;
            GridBagConstraints gridBagConstraints91 = new GridBagConstraints();
            gridBagConstraints91.gridx = 0;
            gridBagConstraints91.insets = new Insets(0, 12, 0, 0);
            gridBagConstraints91.anchor = GridBagConstraints.WEST;
            gridBagConstraints91.gridy = 0;
            labelWinePrefix = new BLabel();
            labelWinePrefix.setText("WINEPREFIX");
            groupWine = new BGroupBox();
            groupWine.setLayout(new GridBagLayout());
            groupWine.setBorder(BorderFactory.createTitledBorder(null, "Platform", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
            groupWine.setTitle("Wine");
            groupWine.add(labelWinePrefix, gridBagConstraints91);
            groupWine.add(getTextWinePrefix(), gridBagConstraints92);
            groupWine.add(getButtonWinePrefix(), gridBagConstraints93);
            groupWine.add(labelWineTop, gridBagConstraints148);
            groupWine.add(getBPanel1(), gridBagConstraints153);
            groupWine.add(getButtonWineTop(), gridBagConstraints156);
            groupWine.add(getRadioWineBuiltin(), gridBagConstraints154);
        }
        return groupWine;
    }

    /**
     * This method initializes textWinePrefix	
     * 	
     * @return org.kbinani.windows.forms.BTextBox	
     */
    private BTextBox getTextWinePrefix() {
        if (textWinePrefix == null) {
            textWinePrefix = new BTextBox();
        }
        return textWinePrefix;
    }

    /**
     * This method initializes buttonWinePrefix	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getButtonWinePrefix() {
        if (buttonWinePrefix == null) {
            buttonWinePrefix = new BButton();
            buttonWinePrefix.setPreferredSize(new Dimension(90, 29));
            buttonWinePrefix.setText("Browse");
        }
        return buttonWinePrefix;
    }

    /**
     * This method initializes textWineTop	
     * 	
     * @return org.kbinani.windows.forms.BTextBox	
     */
    private BTextBox getTextWineTop() {
        if (textWineTop == null) {
            textWineTop = new BTextBox();
        }
        return textWineTop;
    }

    /**
     * This method initializes buttonWineTop	
     * 	
     * @return org.kbinani.windows.forms.BButton	
     */
    private BButton getButtonWineTop() {
        if (buttonWineTop == null) {
            buttonWineTop = new BButton();
            buttonWineTop.setPreferredSize(new Dimension(90, 29));
            buttonWineTop.setText("Browse");
        }
        return buttonWineTop;
    }

    /**
     * This method initializes radioWineCustom	
     * 	
     * @return org.kbinani.windows.forms.BRadioButton	
     */
    private BRadioButton getRadioWineCustom() {
        if (radioWineCustom == null) {
            radioWineCustom = new BRadioButton();
            radioWineCustom.setText("custom");
        }
        return radioWineCustom;
    }

    /**
     * This method initializes BPanel1	
     * 	
     * @return org.kbinani.windows.forms.BPanel	
     */
    private BPanel getBPanel1() {
        if (BPanel1 == null) {
            GridBagConstraints gridBagConstraints150 = new GridBagConstraints();
            gridBagConstraints150.gridx = 3;
            gridBagConstraints150.gridy = 3;
            GridBagConstraints gridBagConstraints149 = new GridBagConstraints();
            gridBagConstraints149.fill = GridBagConstraints.HORIZONTAL;
            gridBagConstraints149.gridx = 1;
            gridBagConstraints149.gridy = 1;
            gridBagConstraints149.weightx = 1.0;
            gridBagConstraints149.insets = new Insets(0, 6, 0, 0);
            GridBagConstraints gridBagConstraints152 = new GridBagConstraints();
            gridBagConstraints152.gridx = 0;
            gridBagConstraints152.weightx = 0.0D;
            gridBagConstraints152.anchor = GridBagConstraints.WEST;
            gridBagConstraints152.gridy = 1;
            BPanel1 = new BPanel();
            BPanel1.setLayout(new GridBagLayout());
            BPanel1.add(getRadioWineCustom(), gridBagConstraints152);
            BPanel1.add(getTextWineTop(), gridBagConstraints149);
        }
        return BPanel1;
    }

    /**
     * This method initializes radioWineBuiltin	
     * 	
     * @return org.kbinani.windows.forms.BRadioButton	
     */
    private BRadioButton getRadioWineBuiltin() {
        if (radioWineBuiltin == null) {
            radioWineBuiltin = new BRadioButton();
            radioWineBuiltin.setText("built-in");
            radioWineBuiltin.setPreferredSize(new Dimension(4, 29));
        }
        return radioWineBuiltin;
    }

	//SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="-49,12"
