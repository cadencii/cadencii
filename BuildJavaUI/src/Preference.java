import java.awt.BorderLayout;
import javax.swing.JPanel;
import javax.swing.JFrame;
import java.awt.Dimension;
import javax.swing.JTabbedPane;
import java.awt.GridBagLayout;
import javax.swing.JButton;
import java.awt.GridBagConstraints;
import javax.swing.JLabel;
import java.awt.Insets;
import java.awt.Color;
import javax.swing.JComboBox;
import javax.swing.BorderFactory;
import javax.swing.border.TitledBorder;
import java.awt.Font;
import javax.swing.JCheckBox;
import javax.swing.JTextField;
import javax.swing.JTable;
import javax.swing.JTextArea;
import javax.swing.SwingUtilities;
import javax.swing.UIManager;

public class Preference extends JFrame {

	private static final long serialVersionUID = 1L;
	private JPanel tabSequence = null;
	private JLabel lblResolution = null;
	private JPanel jPanel = null;
	private JLabel lblDynamics = null;
	private JComboBox comboDynamics = null;
	private JLabel lblAmplitude = null;
	private JComboBox comboAmplitude = null;
	private JLabel lblPeriod = null;
	private JComboBox comboPeriod = null;
	private JLabel jLabel1 = null;
	private JLabel jLabel11 = null;
	private JLabel jLabel12 = null;
	private JLabel lblVibratoConfig = null;
	private JPanel jPanel1 = null;
	private JLabel lblVibratoLength = null;
	private JComboBox comboVibratoLength = null;
	private JLabel jLabel13 = null;
	private JPanel groupAutoVibratoConfig = null;
	private JPanel jPanel3 = null;
	private JCheckBox chkEnableAutoVibrato = null;
	private JLabel lblAutoVibratoMinLength = null;
	private JComboBox comboAutoVibratoMinLength = null;
	private JLabel jLabel4 = null;
	private JPanel jPanel4 = null;
	private JLabel lblAutoVibratoType1 = null;
	private JComboBox comboAutoVibratoType1 = null;
	private JLabel lblAutoVibratoType2 = null;
	private JComboBox comboAutoVibratoType2 = null;
	private JPanel tabAnother = null;
	private JLabel lblDefaultSinger = null;
	private JComboBox comboDefualtSinger = null;
	private JLabel lblPreSendTime = null;
	private JComboBox numPreSendTime = null;
	private JLabel jLabel8 = null;
	private JLabel lblWait = null;
	private JComboBox numWait = null;
	private JLabel jLabel81 = null;
	private JLabel lblDefaultPremeasure = null;
	private JComboBox comboDefaultPremeasure = null;
	private JLabel jLabel9 = null;
	private JPanel tabAppearance = null;
	private JPanel groupFont = null;
	private JLabel labelMenu = null;
	private JLabel labelMenuFontName = null;
	private JButton btnChangeMenuFont = null;
	private JLabel labelScreen = null;
	private JLabel labelScreenFontName = null;
	private JButton btnChangeScreenFont = null;
	private JPanel jPanel7 = null;
	private JLabel lblLanguage = null;
	private JComboBox comboLanguage = null;
	private JPanel jPanel71 = null;
	private JLabel lblTrackHeight = null;
	private JComboBox numTrackHeight = null;
	private JPanel groupVisibleCurve = null;
	private JCheckBox chkAccent = null;
	private JCheckBox chkDecay = null;
	private JCheckBox chkVibratoRate = null;
	private JCheckBox chkVibratoDepth = null;
	private JCheckBox chkVel = null;
	private JCheckBox chkDyn = null;
	private JCheckBox chkBre = null;
	private JCheckBox chkBri = null;
	private JCheckBox chkCle = null;
	private JCheckBox chkOpe = null;
	private JCheckBox chkGen = null;
	private JCheckBox chkPor = null;
	private JCheckBox chkPit = null;
	private JCheckBox chkPbs = null;
	private JCheckBox chkHarmonics = null;
	private JCheckBox chkFx2Depth = null;
	private JCheckBox chkReso1 = null;
	private JCheckBox chkReso2 = null;
	private JCheckBox chkReso3 = null;
	private JCheckBox chkReso4 = null;
	private JCheckBox chkEnvelope = null;
	private JPanel tabOperation = null;
	private JPanel groupPianoroll = null;
	private JLabel labelWheelOrder = null;
	private JComboBox numericUpDownEx1 = null;
	private JCheckBox chkCursorFix = null;
	private JCheckBox chkScrollHorizontal = null;
	private JCheckBox chkKeepLyricInputMode = null;
	private JCheckBox chkPlayPreviewWhenRightClick = null;
	private JCheckBox chkCurveSelectingQuantized = null;
	private JCheckBox chkUseSpaceKeyAsMiddleButtonModifier = null;
	private JPanel groupMisc = null;
	private JLabel lblMaximumFrameRate = null;
	private JComboBox numMaximumFrameRate = null;
	private JLabel lblMilliSecond = null;
	private JLabel lblMouseHoverTime = null;
	private JComboBox numMouseHoverTime = null;
	private JLabel lblMidiInPort = null;
	private JComboBox comboMidiInPortNumber = null;
	private JPanel tabPlatform = null;
	private JPanel groupPlatform = null;
	private JLabel lblPlatform = null;
	private JComboBox comboPlatform = null;
	private JCheckBox chkUseCustomFileDialog = null;
	private JCheckBox chkCommandKeyAsControl = null;
	private JCheckBox chkTranslateRoman = null;
	private JPanel groupVsti = null;
	private JLabel lblVOCALOID1 = null;
	private JTextField txtVOCALOID1 = null;
	private JLabel lblVOCALOID2 = null;
	private JTextField txtVOCALOID2 = null;
	private JPanel groupUtauCores = null;
	private JLabel lblResampler = null;
	private JTextField txtResampler = null;
	private JButton btnResampler = null;
	private JLabel lblWavtool = null;
	private JTextField txtWavtool = null;
	private JButton btnWavtool = null;
	private JCheckBox chkInvokeWithWine = null;
	private JPanel tabUtauSingers = null;
	private JTable listSingers = null;
	private JButton btnAdd = null;
	private JButton btnRemove = null;
	private JButton btnUp = null;
	private JButton btnDown = null;
	private JPanel jPanel17 = null;
	private JPanel jPanel18 = null;
	private JPanel tabFile = null;
	private JCheckBox chkAutoBackup = null;
	private JLabel lblAutoBackupInterval = null;
	private JLabel lblAutoBackupMinutes = null;
	private JPanel jPanel20 = null;
	private JPanel panelLower = null;
	private JButton btnOK = null;
	private JButton btnCancel = null;
	private JPanel jPanel5 = null;
	private JPanel panelUpper = null;
    private JTabbedPane tabPane = null;
	private JTextField numAutoBackupInterval = null;
	/**
	 * This is the default constructor
	 */
	public Preference() {
		super();
		initialize();
		tabPane = new JTabbedPane();
		tabPane.addTab( "Sequence", getTabSequence() );
		tabPane.addTab( "Other Settings", getTabAnother() );
		tabPane.addTab( "Appearance", getTabAppearance() );
		tabPane.addTab( "Operation", getTabOperation() );
		tabPane.addTab( "Platform", getTabPlatform() );
		tabPane.addTab( "UTAU Singers", getTabUtauSingers() );
		tabPane.addTab( "File", getTabFile() );
		GridBagLayout layout = new GridBagLayout();
		getPanelUpper().setLayout( layout );
		GridBagConstraints gb = new GridBagConstraints();
		gb.weighty = 1;
		gb.weightx = 1;
		gb.insets = new Insets( 12, 12, 0, 12 );
		gb.fill = GridBagConstraints.BOTH;
		layout.setConstraints( tabPane, gb );
	    this.getPanelUpper().add( tabPane );
		try{
	        //UIManager.setLookAndFeel( "com.sun.java.swing.plaf.mac.MacLookAndFeel" );
	        UIManager.setLookAndFeel( "com.sun.java.swing.plaf.windows.WindowsLookAndFeel" );
			SwingUtilities.updateComponentTreeUI( this );
		}catch( Exception ex ){
            System.err.println( "Preference#.ctor; ex=" + ex );
		}
	}

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		this.setSize(504, 496);
		this.setContentPane(getJPanel5());
		this.setTitle("Preference");
	}

	/**
	 * This method initializes tabSequence
	 * 
	 * @return javax.swing.JPanel
	 */
	private JPanel getTabSequence() {
		if (tabSequence == null) {
			GridBagConstraints gridBagConstraints20 = new GridBagConstraints();
			gridBagConstraints20.gridx = 0;
			gridBagConstraints20.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints20.anchor = GridBagConstraints.NORTHWEST;
			gridBagConstraints20.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints20.weighty = 1.0D;
			gridBagConstraints20.gridy = 3;
			GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
			gridBagConstraints31.gridx = 0;
			gridBagConstraints31.anchor = GridBagConstraints.WEST;
			gridBagConstraints31.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints31.weightx = 1.0D;
			gridBagConstraints31.gridy = 2;
			lblVibratoConfig = new JLabel();
			lblVibratoConfig.setText("Vibrato Setting");
			GridBagConstraints gridBagConstraints21 = new GridBagConstraints();
			gridBagConstraints21.anchor = GridBagConstraints.WEST;
			gridBagConstraints21.gridy = 1;
			gridBagConstraints21.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints21.gridx = 0;
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.anchor = GridBagConstraints.WEST;
			gridBagConstraints1.gridx = 0;
			gridBagConstraints1.gridy = 0;
			gridBagConstraints1.weightx = 1.0D;
			gridBagConstraints1.insets = new Insets(12, 12, 3, 0);
			lblResolution = new JLabel();
			lblResolution.setText("Resolution(VSTi)");
			tabSequence = new JPanel();
			tabSequence.setLayout(new GridBagLayout());
			tabSequence.add(lblResolution, gridBagConstraints1);
			tabSequence.add(getJPanel(), gridBagConstraints21);
			tabSequence.add(lblVibratoConfig, gridBagConstraints31);
			tabSequence.add(getJPanel1(), gridBagConstraints20);
		}
		return tabSequence;
	}

	/**
	 * This method initializes jPanel	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel() {
		if (jPanel == null) {
			GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
			gridBagConstraints10.gridx = 4;
			gridBagConstraints10.anchor = GridBagConstraints.WEST;
			gridBagConstraints10.weightx = 1.0D;
			gridBagConstraints10.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints10.gridy = 3;
			jLabel12 = new JLabel();
			jLabel12.setText("clocks");
			GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
			gridBagConstraints9.gridx = 4;
			gridBagConstraints9.anchor = GridBagConstraints.WEST;
			gridBagConstraints9.weightx = 1.0D;
			gridBagConstraints9.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints9.gridy = 1;
			jLabel11 = new JLabel();
			jLabel11.setText("clocks");
			GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
			gridBagConstraints8.gridx = 4;
			gridBagConstraints8.weightx = 1.0D;
			gridBagConstraints8.anchor = GridBagConstraints.WEST;
			gridBagConstraints8.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints8.gridy = 0;
			jLabel1 = new JLabel();
			jLabel1.setText("clocks");
			GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			gridBagConstraints7.fill = GridBagConstraints.NONE;
			gridBagConstraints7.gridy = 3;
			gridBagConstraints7.weightx = 0.0D;
			gridBagConstraints7.anchor = GridBagConstraints.WEST;
			gridBagConstraints7.insets = new Insets(3, 24, 3, 0);
			gridBagConstraints7.gridx = 3;
			GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			gridBagConstraints6.gridx = 0;
			gridBagConstraints6.anchor = GridBagConstraints.WEST;
			gridBagConstraints6.insets = new Insets(0, 24, 0, 0);
			gridBagConstraints6.gridy = 3;
			lblPeriod = new JLabel();
			lblPeriod.setText("Vibrato Rate");
			GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			gridBagConstraints5.fill = GridBagConstraints.NONE;
			gridBagConstraints5.gridy = 1;
			gridBagConstraints5.weightx = 0.0D;
			gridBagConstraints5.anchor = GridBagConstraints.WEST;
			gridBagConstraints5.insets = new Insets(3, 24, 3, 0);
			gridBagConstraints5.gridx = 3;
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.gridx = 0;
			gridBagConstraints4.anchor = GridBagConstraints.WEST;
			gridBagConstraints4.insets = new Insets(0, 24, 0, 0);
			gridBagConstraints4.gridy = 1;
			lblAmplitude = new JLabel();
			lblAmplitude.setText("Vibrato Depth");
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
			lblDynamics = new JLabel();
			lblDynamics.setText("Dynamics");
			jPanel = new JPanel();
			jPanel.setLayout(new GridBagLayout());
			jPanel.add(lblDynamics, gridBagConstraints2);
			jPanel.add(getComboDynamics(), gridBagConstraints3);
			jPanel.add(lblAmplitude, gridBagConstraints4);
			jPanel.add(getComboAmplitude(), gridBagConstraints5);
			jPanel.add(lblPeriod, gridBagConstraints6);
			jPanel.add(getComboPeriod(), gridBagConstraints7);
			jPanel.add(jLabel1, gridBagConstraints8);
			jPanel.add(jLabel11, gridBagConstraints9);
			jPanel.add(jLabel12, gridBagConstraints10);
		}
		return jPanel;
	}

	/**
	 * This method initializes comboDynamics	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboDynamics() {
		if (comboDynamics == null) {
			comboDynamics = new JComboBox();
			comboDynamics.setPreferredSize(new Dimension(101, 20));
		}
		return comboDynamics;
	}

	/**
	 * This method initializes comboAmplitude	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboAmplitude() {
		if (comboAmplitude == null) {
			comboAmplitude = new JComboBox();
			comboAmplitude.setPreferredSize(new Dimension(101, 20));
		}
		return comboAmplitude;
	}

	/**
	 * This method initializes comboPeriod	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboPeriod() {
		if (comboPeriod == null) {
			comboPeriod = new JComboBox();
			comboPeriod.setPreferredSize(new Dimension(101, 20));
		}
		return comboPeriod;
	}

	/**
	 * This method initializes jPanel1	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel1() {
		if (jPanel1 == null) {
			GridBagConstraints gridBagConstraints19 = new GridBagConstraints();
			gridBagConstraints19.gridx = 0;
			gridBagConstraints19.gridwidth = 5;
			gridBagConstraints19.anchor = GridBagConstraints.WEST;
			gridBagConstraints19.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints19.insets = new Insets(3, 24, 3, 12);
			gridBagConstraints19.gridy = 1;
			GridBagConstraints gridBagConstraints81 = new GridBagConstraints();
			gridBagConstraints81.anchor = GridBagConstraints.WEST;
			gridBagConstraints81.gridx = 4;
			gridBagConstraints81.gridy = 0;
			gridBagConstraints81.weightx = 1.0D;
			gridBagConstraints81.insets = new Insets(0, 12, 0, 0);
			jLabel13 = new JLabel();
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
			lblVibratoLength = new JLabel();
			lblVibratoLength.setText("Default Vibrato Length");
			jPanel1 = new JPanel();
			jPanel1.setLayout(new GridBagLayout());
			jPanel1.add(lblVibratoLength, gridBagConstraints22);
			jPanel1.add(getComboVibratoLength(), gridBagConstraints32);
			jPanel1.add(jLabel13, gridBagConstraints81);
			jPanel1.add(getGroupAutoVibratoConfig(), gridBagConstraints19);
		}
		return jPanel1;
	}

	/**
	 * This method initializes comboVibratoLength	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboVibratoLength() {
		if (comboVibratoLength == null) {
			comboVibratoLength = new JComboBox();
			comboVibratoLength.setPreferredSize(new Dimension(101, 20));
		}
		return comboVibratoLength;
	}

	/**
	 * This method initializes groupAutoVibratoConfig	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupAutoVibratoConfig() {
		if (groupAutoVibratoConfig == null) {
			GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
			gridBagConstraints51.gridx = 0;
			gridBagConstraints51.anchor = GridBagConstraints.WEST;
			gridBagConstraints51.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints51.gridy = 2;
			GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
			gridBagConstraints14.gridx = 0;
			gridBagConstraints14.anchor = GridBagConstraints.WEST;
			gridBagConstraints14.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints14.gridy = 1;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 0;
			gridBagConstraints.anchor = GridBagConstraints.WEST;
			gridBagConstraints.weightx = 1.0D;
			gridBagConstraints.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints.gridy = 0;
			groupAutoVibratoConfig = new JPanel();
			groupAutoVibratoConfig.setLayout(new GridBagLayout());
			groupAutoVibratoConfig.setBorder(BorderFactory.createTitledBorder(null, "Auto Vibrato Settings", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupAutoVibratoConfig.add(getChkEnableAutoVibrato(), gridBagConstraints);
			groupAutoVibratoConfig.add(getJPanel3(), gridBagConstraints14);
			groupAutoVibratoConfig.add(getJPanel4(), gridBagConstraints51);
		}
		return groupAutoVibratoConfig;
	}

	/**
	 * This method initializes jPanel3	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel3() {
		if (jPanel3 == null) {
			GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			gridBagConstraints13.gridx = 2;
			gridBagConstraints13.weightx = 1.0D;
			gridBagConstraints13.anchor = GridBagConstraints.WEST;
			gridBagConstraints13.gridy = 0;
			jLabel4 = new JLabel();
			jLabel4.setText("beat");
			GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			gridBagConstraints12.fill = GridBagConstraints.NONE;
			gridBagConstraints12.gridy = 0;
			gridBagConstraints12.weightx = 0.0D;
			gridBagConstraints12.insets = new Insets(0, 12, 0, 12);
			gridBagConstraints12.gridx = 1;
			GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
			gridBagConstraints11.gridx = 0;
			gridBagConstraints11.gridy = 0;
			lblAutoVibratoMinLength = new JLabel();
			lblAutoVibratoMinLength.setText("Minimum note length for Automatic Vibrato");
			jPanel3 = new JPanel();
			jPanel3.setLayout(new GridBagLayout());
			jPanel3.add(lblAutoVibratoMinLength, gridBagConstraints11);
			jPanel3.add(getComboAutoVibratoMinLength(), gridBagConstraints12);
			jPanel3.add(jLabel4, gridBagConstraints13);
		}
		return jPanel3;
	}

	/**
	 * This method initializes chkEnableAutoVibrato	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkEnableAutoVibrato() {
		if (chkEnableAutoVibrato == null) {
			chkEnableAutoVibrato = new JCheckBox();
			chkEnableAutoVibrato.setText("Enable Automatic Vibrato");
		}
		return chkEnableAutoVibrato;
	}

	/**
	 * This method initializes comboAutoVibratoMinLength	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboAutoVibratoMinLength() {
		if (comboAutoVibratoMinLength == null) {
			comboAutoVibratoMinLength = new JComboBox();
			comboAutoVibratoMinLength.setPreferredSize(new Dimension(66, 20));
		}
		return comboAutoVibratoMinLength;
	}

	/**
	 * This method initializes jPanel4	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel4() {
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
			gridBagConstraints17.gridy = 1;
			lblAutoVibratoType2 = new JLabel();
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
			gridBagConstraints15.gridy = 0;
			lblAutoVibratoType1 = new JLabel();
			lblAutoVibratoType1.setText("Vibrato Type VOCALOID1");
			jPanel4 = new JPanel();
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
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboAutoVibratoType1() {
		if (comboAutoVibratoType1 == null) {
			comboAutoVibratoType1 = new JComboBox();
			comboAutoVibratoType1.setPreferredSize(new Dimension(101, 20));
		}
		return comboAutoVibratoType1;
	}

	/**
	 * This method initializes comboAutoVibratoType2	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboAutoVibratoType2() {
		if (comboAutoVibratoType2 == null) {
			comboAutoVibratoType2 = new JComboBox();
			comboAutoVibratoType2.setPreferredSize(new Dimension(101, 20));
		}
		return comboAutoVibratoType2;
	}

	/**
	 * This method initializes tabAnother	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getTabAnother() {
		if (tabAnother == null) {
			GridBagConstraints gridBagConstraints35 = new GridBagConstraints();
			gridBagConstraints35.gridx = 0;
			gridBagConstraints35.weighty = 1.0D;
			gridBagConstraints35.gridy = 4;
			jLabel9 = new JLabel();
			jLabel9.setText("   ");
			GridBagConstraints gridBagConstraints34 = new GridBagConstraints();
			gridBagConstraints34.fill = GridBagConstraints.NONE;
			gridBagConstraints34.gridy = 3;
			gridBagConstraints34.weightx = 1.0;
			gridBagConstraints34.gridwidth = 2;
			gridBagConstraints34.anchor = GridBagConstraints.WEST;
			gridBagConstraints34.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints34.gridx = 1;
			GridBagConstraints gridBagConstraints33 = new GridBagConstraints();
			gridBagConstraints33.gridx = 0;
			gridBagConstraints33.anchor = GridBagConstraints.WEST;
			gridBagConstraints33.insets = new Insets(3, 24, 3, 0);
			gridBagConstraints33.gridy = 3;
			lblDefaultPremeasure = new JLabel();
			lblDefaultPremeasure.setText("Default Pre-measure");
			GridBagConstraints gridBagConstraints30 = new GridBagConstraints();
			gridBagConstraints30.gridx = 2;
			gridBagConstraints30.anchor = GridBagConstraints.WEST;
			gridBagConstraints30.weightx = 1.0D;
			gridBagConstraints30.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints30.gridy = 2;
			jLabel81 = new JLabel();
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
			lblWait = new JLabel();
			lblWait.setText("Waiting Time");
			GridBagConstraints gridBagConstraints27 = new GridBagConstraints();
			gridBagConstraints27.gridx = 2;
			gridBagConstraints27.anchor = GridBagConstraints.WEST;
			gridBagConstraints27.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints27.weightx = 1.0D;
			gridBagConstraints27.gridy = 1;
			jLabel8 = new JLabel();
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
			lblPreSendTime = new JLabel();
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
			lblDefaultSinger = new JLabel();
			lblDefaultSinger.setText("Default Singer");
			tabAnother = new JPanel();
			tabAnother.setLayout(new GridBagLayout());
			tabAnother.add(lblDefaultSinger, gridBagConstraints23);
			tabAnother.add(getComboDefualtSinger(), gridBagConstraints24);
			tabAnother.add(lblPreSendTime, gridBagConstraints25);
			tabAnother.add(getNumPreSendTime(), gridBagConstraints26);
			tabAnother.add(jLabel8, gridBagConstraints27);
			tabAnother.add(lblWait, gridBagConstraints28);
			tabAnother.add(getNumWait(), gridBagConstraints29);
			tabAnother.add(jLabel81, gridBagConstraints30);
			tabAnother.add(lblDefaultPremeasure, gridBagConstraints33);
			tabAnother.add(getComboDefaultPremeasure(), gridBagConstraints34);
			tabAnother.add(jLabel9, gridBagConstraints35);
		}
		return tabAnother;
	}

	/**
	 * This method initializes comboDefualtSinger	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboDefualtSinger() {
		if (comboDefualtSinger == null) {
			comboDefualtSinger = new JComboBox();
			comboDefualtSinger.setPreferredSize(new Dimension(222, 20));
		}
		return comboDefualtSinger;
	}

	/**
	 * This method initializes numPreSendTime	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getNumPreSendTime() {
		if (numPreSendTime == null) {
			numPreSendTime = new JComboBox();
			numPreSendTime.setPreferredSize(new Dimension(68, 20));
		}
		return numPreSendTime;
	}

	/**
	 * This method initializes numWait	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getNumWait() {
		if (numWait == null) {
			numWait = new JComboBox();
			numWait.setPreferredSize(new Dimension(68, 20));
		}
		return numWait;
	}

	/**
	 * This method initializes comboDefaultPremeasure	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboDefaultPremeasure() {
		if (comboDefaultPremeasure == null) {
			comboDefaultPremeasure = new JComboBox();
			comboDefaultPremeasure.setPreferredSize(new Dimension(68, 20));
		}
		return comboDefaultPremeasure;
	}

	/**
	 * This method initializes tabAppearance	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getTabAppearance() {
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
			tabAppearance = new JPanel();
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
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupFont() {
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
			labelScreenFontName = new JLabel();
			labelScreenFontName.setText("MS UI Gothic");
			GridBagConstraints gridBagConstraints39 = new GridBagConstraints();
			gridBagConstraints39.gridx = 0;
			gridBagConstraints39.anchor = GridBagConstraints.WEST;
			gridBagConstraints39.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints39.gridy = 1;
			labelScreen = new JLabel();
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
			labelMenuFontName = new JLabel();
			labelMenuFontName.setText("MS UI Gothic");
			GridBagConstraints gridBagConstraints36 = new GridBagConstraints();
			gridBagConstraints36.gridx = 0;
			gridBagConstraints36.anchor = GridBagConstraints.WEST;
			gridBagConstraints36.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints36.gridy = 0;
			labelMenu = new JLabel();
			labelMenu.setText("Menu/Lyrics");
			groupFont = new JPanel();
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
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnChangeMenuFont() {
		if (btnChangeMenuFont == null) {
			btnChangeMenuFont = new JButton();
			btnChangeMenuFont.setText("Change");
			btnChangeMenuFont.setPreferredSize(new Dimension(85, 23));
		}
		return btnChangeMenuFont;
	}

	/**
	 * This method initializes btnChangeScreenFont	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnChangeScreenFont() {
		if (btnChangeScreenFont == null) {
			btnChangeScreenFont = new JButton();
			btnChangeScreenFont.setPreferredSize(new Dimension(85, 23));
			btnChangeScreenFont.setText("Change");
		}
		return btnChangeScreenFont;
	}

	/**
	 * This method initializes jPanel7	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel7() {
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
			lblLanguage = new JLabel();
			lblLanguage.setText("UI Language");
			jPanel7 = new JPanel();
			jPanel7.setLayout(new GridBagLayout());
			jPanel7.add(lblLanguage, gridBagConstraints43);
			jPanel7.add(getComboLanguage(), gridBagConstraints44);
		}
		return jPanel7;
	}

	/**
	 * This method initializes comboLanguage	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboLanguage() {
		if (comboLanguage == null) {
			comboLanguage = new JComboBox();
			comboLanguage.setPreferredSize(new Dimension(121, 20));
		}
		return comboLanguage;
	}

	/**
	 * This method initializes jPanel71	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel71() {
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
			lblTrackHeight = new JLabel();
			lblTrackHeight.setText("Track Height(pixel)");
			jPanel71 = new JPanel();
			jPanel71.setLayout(new GridBagLayout());
			jPanel71.add(lblTrackHeight, gridBagConstraints431);
			jPanel71.add(getNumTrackHeight(), gridBagConstraints441);
		}
		return jPanel71;
	}

	/**
	 * This method initializes numTrackHeight	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getNumTrackHeight() {
		if (numTrackHeight == null) {
			numTrackHeight = new JComboBox();
			numTrackHeight.setPreferredSize(new Dimension(121, 20));
		}
		return numTrackHeight;
	}

	/**
	 * This method initializes groupVisibleCurve	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupVisibleCurve() {
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
			groupVisibleCurve = new JPanel();
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
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkAccent() {
		if (chkAccent == null) {
			chkAccent = new JCheckBox();
			chkAccent.setText("Accent");
		}
		return chkAccent;
	}

	/**
	 * This method initializes chkDecay	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkDecay() {
		if (chkDecay == null) {
			chkDecay = new JCheckBox();
			chkDecay.setText("Decay");
		}
		return chkDecay;
	}

	/**
	 * This method initializes chkVibratoRate	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkVibratoRate() {
		if (chkVibratoRate == null) {
			chkVibratoRate = new JCheckBox();
			chkVibratoRate.setText("VibratoRate");
		}
		return chkVibratoRate;
	}

	/**
	 * This method initializes chkVibratoDepth	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkVibratoDepth() {
		if (chkVibratoDepth == null) {
			chkVibratoDepth = new JCheckBox();
			chkVibratoDepth.setText("VibratoDepth");
		}
		return chkVibratoDepth;
	}

	/**
	 * This method initializes chkVel	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkVel() {
		if (chkVel == null) {
			chkVel = new JCheckBox();
			chkVel.setText("VEL");
		}
		return chkVel;
	}

	/**
	 * This method initializes chkDyn	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkDyn() {
		if (chkDyn == null) {
			chkDyn = new JCheckBox();
			chkDyn.setText("DYN");
		}
		return chkDyn;
	}

	/**
	 * This method initializes chkBre	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkBre() {
		if (chkBre == null) {
			chkBre = new JCheckBox();
			chkBre.setText("BRE");
		}
		return chkBre;
	}

	/**
	 * This method initializes chkBri	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkBri() {
		if (chkBri == null) {
			chkBri = new JCheckBox();
			chkBri.setText("BRI");
		}
		return chkBri;
	}

	/**
	 * This method initializes chkCle	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkCle() {
		if (chkCle == null) {
			chkCle = new JCheckBox();
			chkCle.setText("CLE");
		}
		return chkCle;
	}

	/**
	 * This method initializes chkOpe	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkOpe() {
		if (chkOpe == null) {
			chkOpe = new JCheckBox();
			chkOpe.setText("OPE");
		}
		return chkOpe;
	}

	/**
	 * This method initializes chkGen	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkGen() {
		if (chkGen == null) {
			chkGen = new JCheckBox();
			chkGen.setText("GEN");
		}
		return chkGen;
	}

	/**
	 * This method initializes chkPor	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkPor() {
		if (chkPor == null) {
			chkPor = new JCheckBox();
			chkPor.setText("POR");
		}
		return chkPor;
	}

	/**
	 * This method initializes chkPit	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkPit() {
		if (chkPit == null) {
			chkPit = new JCheckBox();
			chkPit.setText("PIT");
		}
		return chkPit;
	}

	/**
	 * This method initializes chkPbs	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkPbs() {
		if (chkPbs == null) {
			chkPbs = new JCheckBox();
			chkPbs.setText("PBS");
		}
		return chkPbs;
	}

	/**
	 * This method initializes chkHarmonics	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkHarmonics() {
		if (chkHarmonics == null) {
			chkHarmonics = new JCheckBox();
			chkHarmonics.setText("Harmonics");
		}
		return chkHarmonics;
	}

	/**
	 * This method initializes chkFx2Depth	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkFx2Depth() {
		if (chkFx2Depth == null) {
			chkFx2Depth = new JCheckBox();
			chkFx2Depth.setText("FX2Depth");
		}
		return chkFx2Depth;
	}

	/**
	 * This method initializes chkReso1	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkReso1() {
		if (chkReso1 == null) {
			chkReso1 = new JCheckBox();
			chkReso1.setText("Reso1");
		}
		return chkReso1;
	}

	/**
	 * This method initializes chkReso2	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkReso2() {
		if (chkReso2 == null) {
			chkReso2 = new JCheckBox();
			chkReso2.setText("Reso2");
		}
		return chkReso2;
	}

	/**
	 * This method initializes chkReso3	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkReso3() {
		if (chkReso3 == null) {
			chkReso3 = new JCheckBox();
			chkReso3.setText("Reso3");
		}
		return chkReso3;
	}

	/**
	 * This method initializes chkReso4	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkReso4() {
		if (chkReso4 == null) {
			chkReso4 = new JCheckBox();
			chkReso4.setText("Reso4");
		}
		return chkReso4;
	}

	/**
	 * This method initializes chkEnvelope	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkEnvelope() {
		if (chkEnvelope == null) {
			chkEnvelope = new JCheckBox();
			chkEnvelope.setText("Envelope");
		}
		return chkEnvelope;
	}

	/**
	 * This method initializes tabOperation	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getTabOperation() {
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
			tabOperation = new JPanel();
			tabOperation.setLayout(new GridBagLayout());
			tabOperation.add(getGroupPianoroll(), gridBagConstraints78);
			tabOperation.add(getGroupMisc(), gridBagConstraints87);
		}
		return tabOperation;
	}

	/**
	 * This method initializes groupPianoroll	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupPianoroll() {
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
			labelWheelOrder = new JLabel();
			labelWheelOrder.setText("Mouse wheel Rate");
			groupPianoroll = new JPanel();
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
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getNumericUpDownEx1() {
		if (numericUpDownEx1 == null) {
			numericUpDownEx1 = new JComboBox();
			numericUpDownEx1.setPreferredSize(new Dimension(120, 20));
		}
		return numericUpDownEx1;
	}

	/**
	 * This method initializes chkCursorFix	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkCursorFix() {
		if (chkCursorFix == null) {
			chkCursorFix = new JCheckBox();
			chkCursorFix.setText("Fix Play Cursor to Center");
		}
		return chkCursorFix;
	}

	/**
	 * This method initializes chkScrollHorizontal	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkScrollHorizontal() {
		if (chkScrollHorizontal == null) {
			chkScrollHorizontal = new JCheckBox();
			chkScrollHorizontal.setText("Horizontal Scroll when Mouse wheel");
		}
		return chkScrollHorizontal;
	}

	/**
	 * This method initializes chkKeepLyricInputMode	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkKeepLyricInputMode() {
		if (chkKeepLyricInputMode == null) {
			chkKeepLyricInputMode = new JCheckBox();
			chkKeepLyricInputMode.setText("Keep Lyric Input Mode");
		}
		return chkKeepLyricInputMode;
	}

	/**
	 * This method initializes chkPlayPreviewWhenRightClick	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkPlayPreviewWhenRightClick() {
		if (chkPlayPreviewWhenRightClick == null) {
			chkPlayPreviewWhenRightClick = new JCheckBox();
			chkPlayPreviewWhenRightClick.setText("Play Preview On Right Click");
		}
		return chkPlayPreviewWhenRightClick;
	}

	/**
	 * This method initializes chkCurveSelectingQuantized	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkCurveSelectingQuantized() {
		if (chkCurveSelectingQuantized == null) {
			chkCurveSelectingQuantized = new JCheckBox();
			chkCurveSelectingQuantized.setText("Enable Quantize for Curve Selecting");
		}
		return chkCurveSelectingQuantized;
	}

	/**
	 * This method initializes chkUseSpaceKeyAsMiddleButtonModifier	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkUseSpaceKeyAsMiddleButtonModifier() {
		if (chkUseSpaceKeyAsMiddleButtonModifier == null) {
			chkUseSpaceKeyAsMiddleButtonModifier = new JCheckBox();
			chkUseSpaceKeyAsMiddleButtonModifier.setText("Use space key as Middle button modifier");
		}
		return chkUseSpaceKeyAsMiddleButtonModifier;
	}

	/**
	 * This method initializes groupMisc	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupMisc() {
		if (groupMisc == null) {
			GridBagConstraints gridBagConstraints86 = new GridBagConstraints();
			gridBagConstraints86.fill = GridBagConstraints.NONE;
			gridBagConstraints86.gridy = 2;
			gridBagConstraints86.weightx = 1.0D;
			gridBagConstraints86.anchor = GridBagConstraints.WEST;
			gridBagConstraints86.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints86.gridwidth = 2;
			gridBagConstraints86.gridx = 1;
			GridBagConstraints gridBagConstraints85 = new GridBagConstraints();
			gridBagConstraints85.gridx = 0;
			gridBagConstraints85.anchor = GridBagConstraints.WEST;
			gridBagConstraints85.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints85.gridy = 2;
			lblMidiInPort = new JLabel();
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
			gridBagConstraints83.gridy = 1;
			lblMouseHoverTime = new JLabel();
			lblMouseHoverTime.setText("Waiting Time for Preview");
			GridBagConstraints gridBagConstraints82 = new GridBagConstraints();
			gridBagConstraints82.gridx = 2;
			gridBagConstraints82.anchor = GridBagConstraints.WEST;
			gridBagConstraints82.insets = new Insets(3, 3, 3, 0);
			gridBagConstraints82.weightx = 1.0D;
			gridBagConstraints82.gridy = 0;
			lblMilliSecond = new JLabel();
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
			lblMaximumFrameRate = new JLabel();
			lblMaximumFrameRate.setText("Maximum Frame Rate");
			groupMisc = new JPanel();
			groupMisc.setLayout(new GridBagLayout());
			groupMisc.setBorder(BorderFactory.createTitledBorder(null, "Misc", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupMisc.add(lblMaximumFrameRate, gridBagConstraints79);
			groupMisc.add(getNumMaximumFrameRate(), gridBagConstraints80);
			groupMisc.add(lblMilliSecond, gridBagConstraints82);
			groupMisc.add(lblMouseHoverTime, gridBagConstraints83);
			groupMisc.add(getNumMouseHoverTime(), gridBagConstraints84);
			groupMisc.add(lblMidiInPort, gridBagConstraints85);
			groupMisc.add(getComboMidiInPortNumber(), gridBagConstraints86);
		}
		return groupMisc;
	}

	/**
	 * This method initializes numMaximumFrameRate	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getNumMaximumFrameRate() {
		if (numMaximumFrameRate == null) {
			numMaximumFrameRate = new JComboBox();
			numMaximumFrameRate.setPreferredSize(new Dimension(120, 20));
		}
		return numMaximumFrameRate;
	}

	/**
	 * This method initializes numMouseHoverTime	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getNumMouseHoverTime() {
		if (numMouseHoverTime == null) {
			numMouseHoverTime = new JComboBox();
			numMouseHoverTime.setPreferredSize(new Dimension(120, 20));
		}
		return numMouseHoverTime;
	}

	/**
	 * This method initializes comboMidiInPortNumber	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboMidiInPortNumber() {
		if (comboMidiInPortNumber == null) {
			comboMidiInPortNumber = new JComboBox();
			comboMidiInPortNumber.setPreferredSize(new Dimension(239, 20));
		}
		return comboMidiInPortNumber;
	}

	/**
	 * This method initializes tabPlatform	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getTabPlatform() {
		if (tabPlatform == null) {
			GridBagConstraints gridBagConstraints106 = new GridBagConstraints();
			gridBagConstraints106.gridx = 0;
			gridBagConstraints106.anchor = GridBagConstraints.NORTH;
			gridBagConstraints106.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints106.weighty = 1.0D;
			gridBagConstraints106.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints106.gridy = 2;
			GridBagConstraints gridBagConstraints98 = new GridBagConstraints();
			gridBagConstraints98.gridx = 0;
			gridBagConstraints98.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints98.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints98.anchor = GridBagConstraints.NORTH;
			gridBagConstraints98.gridy = 1;
			GridBagConstraints gridBagConstraints93 = new GridBagConstraints();
			gridBagConstraints93.gridx = 0;
			gridBagConstraints93.anchor = GridBagConstraints.NORTH;
			gridBagConstraints93.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints93.weightx = 1.0D;
			gridBagConstraints93.insets = new Insets(12, 12, 3, 12);
			gridBagConstraints93.gridy = 0;
			tabPlatform = new JPanel();
			tabPlatform.setLayout(new GridBagLayout());
			tabPlatform.add(getGroupPlatform(), gridBagConstraints93);
			tabPlatform.add(getGroupVsti(), gridBagConstraints98);
			tabPlatform.add(getGroupUtauCores(), gridBagConstraints106);
		}
		return tabPlatform;
	}

	/**
	 * This method initializes groupPlatform	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupPlatform() {
		if (groupPlatform == null) {
			GridBagConstraints gridBagConstraints92 = new GridBagConstraints();
			gridBagConstraints92.gridx = 0;
			gridBagConstraints92.gridwidth = 2;
			gridBagConstraints92.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints92.anchor = GridBagConstraints.WEST;
			gridBagConstraints92.gridy = 3;
			GridBagConstraints gridBagConstraints91 = new GridBagConstraints();
			gridBagConstraints91.gridx = 0;
			gridBagConstraints91.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints91.anchor = GridBagConstraints.WEST;
			gridBagConstraints91.gridwidth = 2;
			gridBagConstraints91.gridy = 2;
			GridBagConstraints gridBagConstraints90 = new GridBagConstraints();
			gridBagConstraints90.gridx = 0;
			gridBagConstraints90.anchor = GridBagConstraints.WEST;
			gridBagConstraints90.gridwidth = 2;
			gridBagConstraints90.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints90.gridy = 1;
			GridBagConstraints gridBagConstraints89 = new GridBagConstraints();
			gridBagConstraints89.fill = GridBagConstraints.NONE;
			gridBagConstraints89.gridy = 0;
			gridBagConstraints89.weightx = 1.0;
			gridBagConstraints89.anchor = GridBagConstraints.WEST;
			gridBagConstraints89.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints89.gridx = 1;
			GridBagConstraints gridBagConstraints88 = new GridBagConstraints();
			gridBagConstraints88.gridx = 0;
			gridBagConstraints88.anchor = GridBagConstraints.WEST;
			gridBagConstraints88.insets = new Insets(3, 12, 3, 0);
			gridBagConstraints88.gridy = 0;
			lblPlatform = new JLabel();
			lblPlatform.setText("Current Platform");
			groupPlatform = new JPanel();
			groupPlatform.setLayout(new GridBagLayout());
			groupPlatform.setBorder(BorderFactory.createTitledBorder(null, "Platform", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupPlatform.add(lblPlatform, gridBagConstraints88);
			groupPlatform.add(getComboPlatform(), gridBagConstraints89);
			groupPlatform.add(getChkUseCustomFileDialog(), gridBagConstraints90);
			groupPlatform.add(getChkCommandKeyAsControl(), gridBagConstraints91);
			groupPlatform.add(getChkTranslateRoman(), gridBagConstraints92);
		}
		return groupPlatform;
	}

	/**
	 * This method initializes comboPlatform	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboPlatform() {
		if (comboPlatform == null) {
			comboPlatform = new JComboBox();
			comboPlatform.setPreferredSize(new Dimension(121, 20));
		}
		return comboPlatform;
	}

	/**
	 * This method initializes chkUseCustomFileDialog	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkUseCustomFileDialog() {
		if (chkUseCustomFileDialog == null) {
			chkUseCustomFileDialog = new JCheckBox();
			chkUseCustomFileDialog.setText("Use Custom File Dialog");
		}
		return chkUseCustomFileDialog;
	}

	/**
	 * This method initializes chkCommandKeyAsControl	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkCommandKeyAsControl() {
		if (chkCommandKeyAsControl == null) {
			chkCommandKeyAsControl = new JCheckBox();
			chkCommandKeyAsControl.setText("Use Command key as Control key");
		}
		return chkCommandKeyAsControl;
	}

	/**
	 * This method initializes chkTranslateRoman	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkTranslateRoman() {
		if (chkTranslateRoman == null) {
			chkTranslateRoman = new JCheckBox();
			chkTranslateRoman.setText("Translate Roman letters into Kana");
		}
		return chkTranslateRoman;
	}

	/**
	 * This method initializes groupVsti	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupVsti() {
		if (groupVsti == null) {
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
			lblVOCALOID2 = new JLabel();
			lblVOCALOID2.setText("VOCALOID2");
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
			lblVOCALOID1 = new JLabel();
			lblVOCALOID1.setText("VOCALOID1");
			groupVsti = new JPanel();
			groupVsti.setLayout(new GridBagLayout());
			groupVsti.setBorder(BorderFactory.createTitledBorder(null, "VST Instruments", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupVsti.add(lblVOCALOID1, gridBagConstraints94);
			groupVsti.add(getTxtVOCALOID1(), gridBagConstraints95);
			groupVsti.add(lblVOCALOID2, gridBagConstraints96);
			groupVsti.add(getTxtVOCALOID2(), gridBagConstraints97);
		}
		return groupVsti;
	}

	/**
	 * This method initializes txtVOCALOID1	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtVOCALOID1() {
		if (txtVOCALOID1 == null) {
			txtVOCALOID1 = new JTextField();
		}
		return txtVOCALOID1;
	}

	/**
	 * This method initializes txtVOCALOID2	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtVOCALOID2() {
		if (txtVOCALOID2 == null) {
			txtVOCALOID2 = new JTextField();
		}
		return txtVOCALOID2;
	}

	/**
	 * This method initializes groupUtauCores	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupUtauCores() {
		if (groupUtauCores == null) {
			GridBagConstraints gridBagConstraints105 = new GridBagConstraints();
			gridBagConstraints105.gridx = 0;
			gridBagConstraints105.gridwidth = 3;
			gridBagConstraints105.anchor = GridBagConstraints.WEST;
			gridBagConstraints105.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints105.gridy = 2;
			GridBagConstraints gridBagConstraints104 = new GridBagConstraints();
			gridBagConstraints104.gridx = 2;
			gridBagConstraints104.insets = new Insets(3, 3, 3, 3);
			gridBagConstraints104.gridy = 1;
			GridBagConstraints gridBagConstraints103 = new GridBagConstraints();
			gridBagConstraints103.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints103.gridy = 1;
			gridBagConstraints103.weightx = 1.0;
			gridBagConstraints103.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints103.gridx = 1;
			GridBagConstraints gridBagConstraints102 = new GridBagConstraints();
			gridBagConstraints102.gridx = 0;
			gridBagConstraints102.gridy = 1;
			lblWavtool = new JLabel();
			lblWavtool.setText("wavtool");
			GridBagConstraints gridBagConstraints101 = new GridBagConstraints();
			gridBagConstraints101.gridx = 2;
			gridBagConstraints101.insets = new Insets(3, 3, 3, 3);
			gridBagConstraints101.gridy = 0;
			GridBagConstraints gridBagConstraints100 = new GridBagConstraints();
			gridBagConstraints100.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints100.gridy = 0;
			gridBagConstraints100.weightx = 1.0;
			gridBagConstraints100.insets = new Insets(3, 12, 3, 12);
			gridBagConstraints100.anchor = GridBagConstraints.WEST;
			gridBagConstraints100.gridx = 1;
			GridBagConstraints gridBagConstraints99 = new GridBagConstraints();
			gridBagConstraints99.gridx = 0;
			gridBagConstraints99.anchor = GridBagConstraints.WEST;
			gridBagConstraints99.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints99.gridy = 0;
			lblResampler = new JLabel();
			lblResampler.setText("resampler");
			groupUtauCores = new JPanel();
			groupUtauCores.setLayout(new GridBagLayout());
			groupUtauCores.setBorder(BorderFactory.createTitledBorder(null, "UTAU Cores", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupUtauCores.add(lblResampler, gridBagConstraints99);
			groupUtauCores.add(getTxtResampler(), gridBagConstraints100);
			groupUtauCores.add(getBtnResampler(), gridBagConstraints101);
			groupUtauCores.add(lblWavtool, gridBagConstraints102);
			groupUtauCores.add(getTxtWavtool(), gridBagConstraints103);
			groupUtauCores.add(getBtnWavtool(), gridBagConstraints104);
			groupUtauCores.add(getChkInvokeWithWine(), gridBagConstraints105);
		}
		return groupUtauCores;
	}

	/**
	 * This method initializes txtResampler	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtResampler() {
		if (txtResampler == null) {
			txtResampler = new JTextField();
		}
		return txtResampler;
	}

	/**
	 * This method initializes btnResampler	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnResampler() {
		if (btnResampler == null) {
			btnResampler = new JButton();
			btnResampler.setText("...");
			btnResampler.setPreferredSize(new Dimension(41, 23));
		}
		return btnResampler;
	}

	/**
	 * This method initializes txtWavtool	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtWavtool() {
		if (txtWavtool == null) {
			txtWavtool = new JTextField();
		}
		return txtWavtool;
	}

	/**
	 * This method initializes btnWavtool	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnWavtool() {
		if (btnWavtool == null) {
			btnWavtool = new JButton();
			btnWavtool.setPreferredSize(new Dimension(41, 23));
			btnWavtool.setText("...");
		}
		return btnWavtool;
	}

	/**
	 * This method initializes chkInvokeWithWine	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkInvokeWithWine() {
		if (chkInvokeWithWine == null) {
			chkInvokeWithWine = new JCheckBox();
			chkInvokeWithWine.setText("Invoke UTAU cores with Wine");
		}
		return chkInvokeWithWine;
	}

	/**
	 * This method initializes tabUtauSingers	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getTabUtauSingers() {
		if (tabUtauSingers == null) {
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
			GridBagConstraints gridBagConstraints107 = new GridBagConstraints();
			gridBagConstraints107.fill = GridBagConstraints.BOTH;
			gridBagConstraints107.gridy = 0;
			gridBagConstraints107.weightx = 1.0;
			gridBagConstraints107.weighty = 1.0D;
			gridBagConstraints107.anchor = GridBagConstraints.NORTH;
			gridBagConstraints107.insets = new Insets(12, 12, 12, 12);
			gridBagConstraints107.gridwidth = 2;
			gridBagConstraints107.gridx = 0;
			tabUtauSingers = new JPanel();
			tabUtauSingers.setLayout(new GridBagLayout());
			tabUtauSingers.add(getListSingers(), gridBagConstraints107);
			tabUtauSingers.add(getJPanel17(), gridBagConstraints112);
			tabUtauSingers.add(getJPanel18(), gridBagConstraints113);
		}
		return tabUtauSingers;
	}

	/**
	 * This method initializes listSingers	
	 * 	
	 * @return javax.swing.JTable	
	 */
	private JTable getListSingers() {
		if (listSingers == null) {
			listSingers = new JTable();
		}
		return listSingers;
	}

	/**
	 * This method initializes btnAdd	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnAdd() {
		if (btnAdd == null) {
			btnAdd = new JButton();
			btnAdd.setText("Add");
			btnAdd.setPreferredSize(new Dimension(85, 23));
		}
		return btnAdd;
	}

	/**
	 * This method initializes btnRemove	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnRemove() {
		if (btnRemove == null) {
			btnRemove = new JButton();
			btnRemove.setText("Remove");
			btnRemove.setPreferredSize(new Dimension(85, 23));
		}
		return btnRemove;
	}

	/**
	 * This method initializes btnUp	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnUp() {
		if (btnUp == null) {
			btnUp = new JButton();
			btnUp.setText("Up");
			btnUp.setPreferredSize(new Dimension(75, 23));
		}
		return btnUp;
	}

	/**
	 * This method initializes btnDown	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnDown() {
		if (btnDown == null) {
			btnDown = new JButton();
			btnDown.setText("Down");
			btnDown.setPreferredSize(new Dimension(75, 23));
		}
		return btnDown;
	}

	/**
	 * This method initializes jPanel17	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel17() {
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
			jPanel17 = new JPanel();
			jPanel17.setLayout(new GridBagLayout());
			jPanel17.add(getBtnAdd(), gridBagConstraints108);
			jPanel17.add(getBtnRemove(), gridBagConstraints109);
		}
		return jPanel17;
	}

	/**
	 * This method initializes jPanel18	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel18() {
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
			jPanel18 = new JPanel();
			jPanel18.setLayout(new GridBagLayout());
			jPanel18.add(getBtnUp(), gridBagConstraints110);
			jPanel18.add(getBtnDown(), gridBagConstraints111);
		}
		return jPanel18;
	}

	/**
	 * This method initializes tabFile	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getTabFile() {
		if (tabFile == null) {
			GridBagConstraints gridBagConstraints118 = new GridBagConstraints();
			gridBagConstraints118.gridx = 0;
			gridBagConstraints118.anchor = GridBagConstraints.NORTH;
			gridBagConstraints118.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints118.weighty = 1.0D;
			gridBagConstraints118.weightx = 1.0D;
			gridBagConstraints118.gridy = 0;
			lblAutoBackupInterval = new JLabel();
			lblAutoBackupInterval.setText("interval");
			tabFile = new JPanel();
			tabFile.setLayout(new GridBagLayout());
			tabFile.add(getJPanel20(), gridBagConstraints118);
		}
		return tabFile;
	}

	/**
	 * This method initializes chkAutoBackup	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkAutoBackup() {
		if (chkAutoBackup == null) {
			chkAutoBackup = new JCheckBox();
			chkAutoBackup.setText("Automatical Backup");
		}
		return chkAutoBackup;
	}

	/**
	 * This method initializes lblAutoBackupMinutes	
	 * 	
	 * @return javax.swing.JLabel	
	 */
	private JLabel getLblAutoBackupMinutes() {
		if (lblAutoBackupMinutes == null) {
			lblAutoBackupMinutes = new JLabel();
			lblAutoBackupMinutes.setText("minutes");
		}
		return lblAutoBackupMinutes;
	}

	/**
	 * This method initializes jPanel20	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel20() {
		if (jPanel20 == null) {
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
			jPanel20 = new JPanel();
			jPanel20.setLayout(new GridBagLayout());
			jPanel20.add(getChkAutoBackup(), gridBagConstraints114);
			jPanel20.add(lblAutoBackupInterval, gridBagConstraints115);
			jPanel20.add(getLblAutoBackupMinutes(), gridBagConstraints117);
			jPanel20.add(getNumAutoBackupInterval(), gridBagConstraints121);
		}
		return jPanel20;
	}

	/**
	 * This method initializes panelLower	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getPanelLower() {
		if (panelLower == null) {
			GridBagConstraints gridBagConstraints311 = new GridBagConstraints();
			gridBagConstraints311.insets = new Insets(0, 0, 0, 12);
			gridBagConstraints311.gridy = 1;
			gridBagConstraints311.gridx = 1;
			GridBagConstraints gridBagConstraints211 = new GridBagConstraints();
			gridBagConstraints211.insets = new Insets(0, 0, 0, 16);
			gridBagConstraints211.gridy = 1;
			gridBagConstraints211.gridx = 0;
			panelLower = new JPanel();
			panelLower.setLayout(new GridBagLayout());
			panelLower.add(getBtnOK(), gridBagConstraints211);
			panelLower.add(getBtnCancel(), gridBagConstraints311);
		}
		return panelLower;
	}

	/**
	 * This method initializes btnOK	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnOK() {
		if (btnOK == null) {
			btnOK = new JButton();
			btnOK.setText("OK");
		}
		return btnOK;
	}

	/**
	 * This method initializes btnCancel	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getBtnCancel() {
		if (btnCancel == null) {
			btnCancel = new JButton();
			btnCancel.setText("Cancel");
		}
		return btnCancel;
	}

	/**
	 * This method initializes jPanel5	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel5() {
		if (jPanel5 == null) {
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
			gridBagConstraints119.gridy = 1;
			jPanel5 = new JPanel();
			jPanel5.setLayout(new GridBagLayout());
			jPanel5.add(getPanelUpper(), gridBagConstraints120);
			jPanel5.add(getPanelLower(), gridBagConstraints119);
		}
		return jPanel5;
	}

	/**
	 * This method initializes panelUpper	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getPanelUpper() {
		if (panelUpper == null) {
			panelUpper = new JPanel();
			panelUpper.setLayout(new GridBagLayout());
		}
		return panelUpper;
	}

	/**
	 * This method initializes numAutoBackupInterval	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getNumAutoBackupInterval() {
		if (numAutoBackupInterval == null) {
			numAutoBackupInterval = new JTextField();
			numAutoBackupInterval.setPreferredSize(new Dimension(69, 20));
		}
		return numAutoBackupInterval;
	}

}  //  @jve:decl-index=0:visual-constraint="-49,12"
