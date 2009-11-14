import java.awt.BorderLayout;
import javax.swing.JPanel;
import javax.swing.JFrame;
import java.awt.GridBagLayout;
import java.awt.Dimension;
import javax.swing.JLabel;
import java.awt.GridBagConstraints;
import javax.swing.JComboBox;
import javax.swing.BorderFactory;
import javax.swing.border.TitledBorder;
import java.awt.Font;
import java.awt.Color;
import javax.swing.JSlider;
import javax.swing.JTextField;
import java.awt.Insets;
import javax.swing.JCheckBox;
import javax.swing.JButton;

public class FormNoteExpressionConfig extends JFrame {

	private static final long serialVersionUID = 1L;
	private JPanel jContentPane = null;
	private JPanel panelVocaloid2Template = null;
	private JLabel lblTemplate = null;
	private JComboBox comboTemplate = null;
	private JLabel jLabel1 = null;
	private JPanel groupPitchControl = null;
	private JLabel lblBendDepth = null;
	private JSlider trackBendDepth = null;
	private JTextField txtBendDepth = null;
	private JLabel jLabel3 = null;
	private JLabel lblBendLength = null;
	private JSlider trackBendLength = null;
	private JTextField txtBendLength = null;
	private JLabel jLabel5 = null;
	private JCheckBox chkUpPortamento = null;
	private JCheckBox chkDownPortamento = null;
	private JLabel jLabel6 = null;
	private JPanel groupDynamicsControl = null;
	private JLabel lblDecay = null;
	private JSlider trackDecay = null;
	private JTextField txtDecay = null;
	private JLabel jLabel31 = null;
	private JLabel lblAccent = null;
	private JSlider trackAccent = null;
	private JTextField txtAccent = null;
	private JLabel jLabel51 = null;
	private JLabel jLabel61 = null;
	private JPanel groupAttack = null;
	private JLabel lblDuration = null;
	private JSlider trackDuration = null;
	private JTextField txtDuration = null;
	private JLabel jLabel311 = null;
	private JLabel lblDepth = null;
	private JSlider trackDepth = null;
	private JTextField txtDepth = null;
	private JLabel jLabel511 = null;
	private JLabel jLabel611 = null;
	private JLabel lblAttackTemplate = null;
	private JComboBox comboAttackTemplate = null;
	private JPanel jPanel2 = null;
	private JButton btnOK = null;
	private JButton btnCancel = null;

	/**
	 * This is the default constructor
	 */
	public FormNoteExpressionConfig() {
		super();
		initialize();
	}

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		this.setSize(403, 461);
		this.setContentPane(getJContentPane());
		this.setTitle("Default Singer Style");
	}

	/**
	 * This method initializes jContentPane
	 * 
	 * @return javax.swing.JPanel
	 */
	private JPanel getJContentPane() {
		if (jContentPane == null) {
			GridBagConstraints gridBagConstraints20 = new GridBagConstraints();
			gridBagConstraints20.gridx = 0;
			gridBagConstraints20.insets = new Insets(16, 0, 16, 0);
			gridBagConstraints20.anchor = GridBagConstraints.SOUTHEAST;
			gridBagConstraints20.weighty = 1.0D;
			gridBagConstraints20.fill = GridBagConstraints.VERTICAL;
			gridBagConstraints20.gridy = 4;
			GridBagConstraints gridBagConstraints19 = new GridBagConstraints();
			gridBagConstraints19.gridx = 0;
			gridBagConstraints19.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints19.insets = new Insets(12, 12, 0, 12);
			gridBagConstraints19.gridy = 3;
			GridBagConstraints gridBagConstraints16 = new GridBagConstraints();
			gridBagConstraints16.gridx = 0;
			gridBagConstraints16.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints16.weightx = 1.0D;
			gridBagConstraints16.insets = new Insets(12, 12, 0, 12);
			gridBagConstraints16.gridy = 2;
			GridBagConstraints gridBagConstraints15 = new GridBagConstraints();
			gridBagConstraints15.gridx = 0;
			gridBagConstraints15.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints15.anchor = GridBagConstraints.NORTH;
			gridBagConstraints15.insets = new Insets(12, 12, 0, 12);
			gridBagConstraints15.gridy = 1;
			GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			gridBagConstraints3.gridx = 0;
			gridBagConstraints3.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints3.weightx = 1.0D;
			gridBagConstraints3.anchor = GridBagConstraints.NORTH;
			gridBagConstraints3.insets = new Insets(12, 12, 0, 12);
			gridBagConstraints3.gridy = 0;
			jContentPane = new JPanel();
			jContentPane.setLayout(new GridBagLayout());
			jContentPane.add(getPanelVocaloid2Template(), gridBagConstraints3);
			jContentPane.add(getGroupPitchControl(), gridBagConstraints15);
			jContentPane.add(getGroupDynamicsControl(), gridBagConstraints16);
			jContentPane.add(getGroupAttack(), gridBagConstraints19);
			jContentPane.add(getJPanel2(), gridBagConstraints20);
		}
		return jContentPane;
	}

	/**
	 * This method initializes panelVocaloid2Template	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getPanelVocaloid2Template() {
		if (panelVocaloid2Template == null) {
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 0;
			gridBagConstraints2.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints2.weightx = 1.0D;
			gridBagConstraints2.gridy = 0;
			jLabel1 = new JLabel();
			jLabel1.setText("");
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.fill = GridBagConstraints.NONE;
			gridBagConstraints1.gridy = 0;
			gridBagConstraints1.weightx = 0.0D;
			gridBagConstraints1.anchor = GridBagConstraints.EAST;
			gridBagConstraints1.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints1.gridx = 2;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 1;
			gridBagConstraints.anchor = GridBagConstraints.EAST;
			gridBagConstraints.gridy = 0;
			lblTemplate = new JLabel();
			lblTemplate.setText("Template");
			panelVocaloid2Template = new JPanel();
			panelVocaloid2Template.setLayout(new GridBagLayout());
			panelVocaloid2Template.add(lblTemplate, gridBagConstraints);
			panelVocaloid2Template.add(getComboTemplate(), gridBagConstraints1);
			panelVocaloid2Template.add(jLabel1, gridBagConstraints2);
		}
		return panelVocaloid2Template;
	}

	/**
	 * This method initializes comboTemplate	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboTemplate() {
		if (comboTemplate == null) {
			comboTemplate = new JComboBox();
			comboTemplate.setPreferredSize(new Dimension(121, 22));
		}
		return comboTemplate;
	}

	/**
	 * This method initializes groupPitchControl	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupPitchControl() {
		if (groupPitchControl == null) {
			GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
			gridBagConstraints14.gridx = 0;
			gridBagConstraints14.fill = GridBagConstraints.BOTH;
			gridBagConstraints14.weighty = 1.0D;
			gridBagConstraints14.weightx = 1.0D;
			gridBagConstraints14.gridwidth = 4;
			gridBagConstraints14.gridy = 4;
			jLabel6 = new JLabel();
			jLabel6.setText("");
			GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			gridBagConstraints13.gridx = 0;
			gridBagConstraints13.gridwidth = 4;
			gridBagConstraints13.anchor = GridBagConstraints.WEST;
			gridBagConstraints13.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints13.gridy = 3;
			GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			gridBagConstraints12.gridx = 0;
			gridBagConstraints12.gridwidth = 4;
			gridBagConstraints12.anchor = GridBagConstraints.WEST;
			gridBagConstraints12.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints12.gridy = 2;
			GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
			gridBagConstraints11.gridx = 3;
			gridBagConstraints11.weightx = 1.0D;
			gridBagConstraints11.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints11.anchor = GridBagConstraints.WEST;
			gridBagConstraints11.gridy = 1;
			jLabel5 = new JLabel();
			jLabel5.setText("%");
			GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
			gridBagConstraints10.fill = GridBagConstraints.NONE;
			gridBagConstraints10.gridy = 1;
			gridBagConstraints10.weightx = 1.0;
			gridBagConstraints10.anchor = GridBagConstraints.WEST;
			gridBagConstraints10.insets = new Insets(3, 3, 3, 3);
			gridBagConstraints10.gridx = 2;
			GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
			gridBagConstraints9.fill = GridBagConstraints.NONE;
			gridBagConstraints9.gridy = 1;
			gridBagConstraints9.weightx = 0.0D;
			gridBagConstraints9.anchor = GridBagConstraints.WEST;
			gridBagConstraints9.insets = new Insets(3, 3, 3, 3);
			gridBagConstraints9.gridx = 1;
			GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
			gridBagConstraints8.gridx = 0;
			gridBagConstraints8.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints8.anchor = GridBagConstraints.WEST;
			gridBagConstraints8.gridy = 1;
			lblBendLength = new JLabel();
			lblBendLength.setText("Bend Length");
			GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			gridBagConstraints7.gridx = 3;
			gridBagConstraints7.anchor = GridBagConstraints.WEST;
			gridBagConstraints7.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints7.weightx = 1.0D;
			gridBagConstraints7.gridy = 0;
			jLabel3 = new JLabel();
			jLabel3.setText("%");
			GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			gridBagConstraints6.fill = GridBagConstraints.NONE;
			gridBagConstraints6.gridy = 0;
			gridBagConstraints6.weightx = 1.0;
			gridBagConstraints6.anchor = GridBagConstraints.WEST;
			gridBagConstraints6.insets = new Insets(3, 3, 3, 3);
			gridBagConstraints6.gridx = 2;
			GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			gridBagConstraints5.fill = GridBagConstraints.NONE;
			gridBagConstraints5.gridy = 0;
			gridBagConstraints5.weightx = 0.0D;
			gridBagConstraints5.anchor = GridBagConstraints.WEST;
			gridBagConstraints5.insets = new Insets(3, 3, 3, 3);
			gridBagConstraints5.gridx = 1;
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.gridx = 0;
			gridBagConstraints4.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints4.anchor = GridBagConstraints.WEST;
			gridBagConstraints4.gridy = 0;
			lblBendDepth = new JLabel();
			lblBendDepth.setText("Bend Depth");
			groupPitchControl = new JPanel();
			groupPitchControl.setLayout(new GridBagLayout());
			groupPitchControl.setBorder(BorderFactory.createTitledBorder(null, "Pitch Control (VOCALOID2)", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupPitchControl.add(lblBendDepth, gridBagConstraints4);
			groupPitchControl.add(getTrackBendDepth(), gridBagConstraints5);
			groupPitchControl.add(getTxtBendDepth(), gridBagConstraints6);
			groupPitchControl.add(jLabel3, gridBagConstraints7);
			groupPitchControl.add(lblBendLength, gridBagConstraints8);
			groupPitchControl.add(getTrackBendLength(), gridBagConstraints9);
			groupPitchControl.add(getTxtBendLength(), gridBagConstraints10);
			groupPitchControl.add(jLabel5, gridBagConstraints11);
			groupPitchControl.add(getChkUpPortamento(), gridBagConstraints12);
			groupPitchControl.add(getChkDownPortamento(), gridBagConstraints13);
			groupPitchControl.add(jLabel6, gridBagConstraints14);
		}
		return groupPitchControl;
	}

	/**
	 * This method initializes trackBendDepth	
	 * 	
	 * @return javax.swing.JSlider	
	 */
	private JSlider getTrackBendDepth() {
		if (trackBendDepth == null) {
			trackBendDepth = new JSlider();
			trackBendDepth.setPreferredSize(new Dimension(156, 18));
			trackBendDepth.setValue(8);
		}
		return trackBendDepth;
	}

	/**
	 * This method initializes txtBendDepth	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtBendDepth() {
		if (txtBendDepth == null) {
			txtBendDepth = new JTextField();
			txtBendDepth.setText("8");
			txtBendDepth.setHorizontalAlignment(JTextField.RIGHT);
			txtBendDepth.setPreferredSize(new Dimension(39, 19));
		}
		return txtBendDepth;
	}

	/**
	 * This method initializes trackBendLength	
	 * 	
	 * @return javax.swing.JSlider	
	 */
	private JSlider getTrackBendLength() {
		if (trackBendLength == null) {
			trackBendLength = new JSlider();
			trackBendLength.setPreferredSize(new Dimension(156, 18));
			trackBendLength.setValue(0);
		}
		return trackBendLength;
	}

	/**
	 * This method initializes txtBendLength	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtBendLength() {
		if (txtBendLength == null) {
			txtBendLength = new JTextField();
			txtBendLength.setText("0");
			txtBendLength.setPreferredSize(new Dimension(39, 19));
			txtBendLength.setHorizontalAlignment(JTextField.RIGHT);
		}
		return txtBendLength;
	}

	/**
	 * This method initializes chkUpPortamento	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkUpPortamento() {
		if (chkUpPortamento == null) {
			chkUpPortamento = new JCheckBox();
			chkUpPortamento.setText("Add portamento in rising movement");
		}
		return chkUpPortamento;
	}

	/**
	 * This method initializes chkDownPortamento	
	 * 	
	 * @return javax.swing.JCheckBox	
	 */
	private JCheckBox getChkDownPortamento() {
		if (chkDownPortamento == null) {
			chkDownPortamento = new JCheckBox();
			chkDownPortamento.setText("Add portamento in falling movement");
		}
		return chkDownPortamento;
	}

	/**
	 * This method initializes groupDynamicsControl	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupDynamicsControl() {
		if (groupDynamicsControl == null) {
			GridBagConstraints gridBagConstraints141 = new GridBagConstraints();
			gridBagConstraints141.fill = GridBagConstraints.BOTH;
			gridBagConstraints141.gridx = 0;
			gridBagConstraints141.gridy = 4;
			gridBagConstraints141.weightx = 1.0D;
			gridBagConstraints141.weighty = 1.0D;
			gridBagConstraints141.gridwidth = 4;
			jLabel61 = new JLabel();
			jLabel61.setText("");
			GridBagConstraints gridBagConstraints111 = new GridBagConstraints();
			gridBagConstraints111.gridx = 3;
			gridBagConstraints111.anchor = GridBagConstraints.WEST;
			gridBagConstraints111.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints111.weightx = 1.0D;
			gridBagConstraints111.gridy = 1;
			jLabel51 = new JLabel();
			jLabel51.setText("%");
			GridBagConstraints gridBagConstraints101 = new GridBagConstraints();
			gridBagConstraints101.fill = GridBagConstraints.NONE;
			gridBagConstraints101.gridy = 1;
			gridBagConstraints101.weightx = 0.0D;
			gridBagConstraints101.anchor = GridBagConstraints.WEST;
			gridBagConstraints101.gridx = 2;
			GridBagConstraints gridBagConstraints91 = new GridBagConstraints();
			gridBagConstraints91.fill = GridBagConstraints.NONE;
			gridBagConstraints91.gridy = 1;
			gridBagConstraints91.weightx = 0.0D;
			gridBagConstraints91.insets = new Insets(3, 3, 3, 3);
			gridBagConstraints91.anchor = GridBagConstraints.WEST;
			gridBagConstraints91.gridx = 1;
			GridBagConstraints gridBagConstraints81 = new GridBagConstraints();
			gridBagConstraints81.anchor = GridBagConstraints.WEST;
			gridBagConstraints81.gridx = 0;
			gridBagConstraints81.gridy = 1;
			gridBagConstraints81.insets = new Insets(0, 12, 0, 0);
			lblAccent = new JLabel();
			lblAccent.setText("Accent");
			GridBagConstraints gridBagConstraints71 = new GridBagConstraints();
			gridBagConstraints71.gridx = 3;
			gridBagConstraints71.weightx = 1.0D;
			gridBagConstraints71.anchor = GridBagConstraints.WEST;
			gridBagConstraints71.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints71.gridy = 0;
			jLabel31 = new JLabel();
			jLabel31.setText("%");
			GridBagConstraints gridBagConstraints61 = new GridBagConstraints();
			gridBagConstraints61.fill = GridBagConstraints.NONE;
			gridBagConstraints61.gridy = 0;
			gridBagConstraints61.weightx = 0.0D;
			gridBagConstraints61.anchor = GridBagConstraints.WEST;
			gridBagConstraints61.gridx = 2;
			GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
			gridBagConstraints51.fill = GridBagConstraints.NONE;
			gridBagConstraints51.gridy = 0;
			gridBagConstraints51.weightx = 0.0D;
			gridBagConstraints51.insets = new Insets(3, 3, 3, 3);
			gridBagConstraints51.anchor = GridBagConstraints.WEST;
			gridBagConstraints51.gridx = 1;
			GridBagConstraints gridBagConstraints41 = new GridBagConstraints();
			gridBagConstraints41.anchor = GridBagConstraints.WEST;
			gridBagConstraints41.gridx = 0;
			gridBagConstraints41.gridy = 0;
			gridBagConstraints41.insets = new Insets(0, 12, 0, 0);
			lblDecay = new JLabel();
			lblDecay.setText("Decay");
			groupDynamicsControl = new JPanel();
			groupDynamicsControl.setLayout(new GridBagLayout());
			groupDynamicsControl.setBorder(BorderFactory.createTitledBorder(null, "Dynamics Control (VOCALOID2)", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupDynamicsControl.add(lblDecay, gridBagConstraints41);
			groupDynamicsControl.add(getTrackDecay(), gridBagConstraints51);
			groupDynamicsControl.add(getTxtDecay(), gridBagConstraints61);
			groupDynamicsControl.add(jLabel31, gridBagConstraints71);
			groupDynamicsControl.add(lblAccent, gridBagConstraints81);
			groupDynamicsControl.add(getTrackAccent(), gridBagConstraints91);
			groupDynamicsControl.add(getTxtAccent(), gridBagConstraints101);
			groupDynamicsControl.add(jLabel51, gridBagConstraints111);
			groupDynamicsControl.add(jLabel61, gridBagConstraints141);
		}
		return groupDynamicsControl;
	}

	/**
	 * This method initializes trackDecay	
	 * 	
	 * @return javax.swing.JSlider	
	 */
	private JSlider getTrackDecay() {
		if (trackDecay == null) {
			trackDecay = new JSlider();
			trackDecay.setPreferredSize(new Dimension(156, 18));
		}
		return trackDecay;
	}

	/**
	 * This method initializes txtDecay	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtDecay() {
		if (txtDecay == null) {
			txtDecay = new JTextField();
			txtDecay.setPreferredSize(new Dimension(39, 19));
			txtDecay.setHorizontalAlignment(JTextField.RIGHT);
			txtDecay.setText("50");
		}
		return txtDecay;
	}

	/**
	 * This method initializes trackAccent	
	 * 	
	 * @return javax.swing.JSlider	
	 */
	private JSlider getTrackAccent() {
		if (trackAccent == null) {
			trackAccent = new JSlider();
			trackAccent.setPreferredSize(new Dimension(156, 18));
		}
		return trackAccent;
	}

	/**
	 * This method initializes txtAccent	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtAccent() {
		if (txtAccent == null) {
			txtAccent = new JTextField();
			txtAccent.setPreferredSize(new Dimension(39, 19));
			txtAccent.setHorizontalAlignment(JTextField.RIGHT);
			txtAccent.setText("50");
		}
		return txtAccent;
	}

	/**
	 * This method initializes groupAttack	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupAttack() {
		if (groupAttack == null) {
			GridBagConstraints gridBagConstraints18 = new GridBagConstraints();
			gridBagConstraints18.fill = GridBagConstraints.NONE;
			gridBagConstraints18.gridy = 0;
			gridBagConstraints18.weightx = 1.0;
			gridBagConstraints18.gridwidth = 3;
			gridBagConstraints18.anchor = GridBagConstraints.WEST;
			gridBagConstraints18.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints18.gridx = 1;
			GridBagConstraints gridBagConstraints17 = new GridBagConstraints();
			gridBagConstraints17.gridx = 0;
			gridBagConstraints17.insets = new Insets(0, 12, 0, 0);
			gridBagConstraints17.gridy = 0;
			lblAttackTemplate = new JLabel();
			lblAttackTemplate.setText("Attack Variation");
			GridBagConstraints gridBagConstraints1411 = new GridBagConstraints();
			gridBagConstraints1411.fill = GridBagConstraints.BOTH;
			gridBagConstraints1411.gridx = 0;
			gridBagConstraints1411.gridy = 5;
			gridBagConstraints1411.weightx = 1.0D;
			gridBagConstraints1411.weighty = 1.0D;
			gridBagConstraints1411.gridwidth = 4;
			jLabel611 = new JLabel();
			jLabel611.setText("");
			GridBagConstraints gridBagConstraints1111 = new GridBagConstraints();
			gridBagConstraints1111.gridx = 3;
			gridBagConstraints1111.anchor = GridBagConstraints.WEST;
			gridBagConstraints1111.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints1111.weightx = 1.0D;
			gridBagConstraints1111.gridy = 2;
			jLabel511 = new JLabel();
			jLabel511.setText("%");
			GridBagConstraints gridBagConstraints1011 = new GridBagConstraints();
			gridBagConstraints1011.fill = GridBagConstraints.NONE;
			gridBagConstraints1011.gridy = 2;
			gridBagConstraints1011.weightx = 0.0D;
			gridBagConstraints1011.anchor = GridBagConstraints.WEST;
			gridBagConstraints1011.gridx = 2;
			GridBagConstraints gridBagConstraints911 = new GridBagConstraints();
			gridBagConstraints911.fill = GridBagConstraints.NONE;
			gridBagConstraints911.gridy = 2;
			gridBagConstraints911.weightx = 0.0D;
			gridBagConstraints911.anchor = GridBagConstraints.WEST;
			gridBagConstraints911.insets = new Insets(3, 3, 3, 3);
			gridBagConstraints911.gridx = 1;
			GridBagConstraints gridBagConstraints811 = new GridBagConstraints();
			gridBagConstraints811.anchor = GridBagConstraints.WEST;
			gridBagConstraints811.gridx = 0;
			gridBagConstraints811.gridy = 2;
			gridBagConstraints811.insets = new Insets(0, 12, 0, 0);
			lblDepth = new JLabel();
			lblDepth.setText("Depth");
			GridBagConstraints gridBagConstraints711 = new GridBagConstraints();
			gridBagConstraints711.gridx = 3;
			gridBagConstraints711.weightx = 1.0D;
			gridBagConstraints711.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints711.anchor = GridBagConstraints.WEST;
			gridBagConstraints711.gridy = 1;
			jLabel311 = new JLabel();
			jLabel311.setText("%");
			GridBagConstraints gridBagConstraints611 = new GridBagConstraints();
			gridBagConstraints611.fill = GridBagConstraints.NONE;
			gridBagConstraints611.gridy = 1;
			gridBagConstraints611.weightx = 0.0D;
			gridBagConstraints611.anchor = GridBagConstraints.WEST;
			gridBagConstraints611.gridx = 2;
			GridBagConstraints gridBagConstraints511 = new GridBagConstraints();
			gridBagConstraints511.fill = GridBagConstraints.NONE;
			gridBagConstraints511.gridy = 1;
			gridBagConstraints511.weightx = 0.0D;
			gridBagConstraints511.anchor = GridBagConstraints.WEST;
			gridBagConstraints511.insets = new Insets(3, 3, 3, 3);
			gridBagConstraints511.gridx = 1;
			GridBagConstraints gridBagConstraints411 = new GridBagConstraints();
			gridBagConstraints411.anchor = GridBagConstraints.WEST;
			gridBagConstraints411.gridx = 0;
			gridBagConstraints411.gridy = 1;
			gridBagConstraints411.insets = new Insets(0, 12, 0, 0);
			lblDuration = new JLabel();
			lblDuration.setText("Duration");
			groupAttack = new JPanel();
			groupAttack.setLayout(new GridBagLayout());
			groupAttack.setBorder(BorderFactory.createTitledBorder(null, "Attack (VOCALOID1)", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font("Dialog", Font.BOLD, 12), new Color(51, 51, 51)));
			groupAttack.add(lblDuration, gridBagConstraints411);
			groupAttack.add(getTrackDuration(), gridBagConstraints511);
			groupAttack.add(getTxtDuration(), gridBagConstraints611);
			groupAttack.add(jLabel311, gridBagConstraints711);
			groupAttack.add(lblDepth, gridBagConstraints811);
			groupAttack.add(getTrackDepth(), gridBagConstraints911);
			groupAttack.add(getTxtDepth(), gridBagConstraints1011);
			groupAttack.add(jLabel511, gridBagConstraints1111);
			groupAttack.add(jLabel611, gridBagConstraints1411);
			groupAttack.add(lblAttackTemplate, gridBagConstraints17);
			groupAttack.add(getComboAttackTemplate(), gridBagConstraints18);
		}
		return groupAttack;
	}

	/**
	 * This method initializes trackDuration	
	 * 	
	 * @return javax.swing.JSlider	
	 */
	private JSlider getTrackDuration() {
		if (trackDuration == null) {
			trackDuration = new JSlider();
			trackDuration.setPreferredSize(new Dimension(156, 18));
			trackDuration.setValue(64);
			trackDuration.setMaximum(127);
		}
		return trackDuration;
	}

	/**
	 * This method initializes txtDuration	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtDuration() {
		if (txtDuration == null) {
			txtDuration = new JTextField();
			txtDuration.setPreferredSize(new Dimension(39, 19));
			txtDuration.setHorizontalAlignment(JTextField.RIGHT);
			txtDuration.setText("64");
		}
		return txtDuration;
	}

	/**
	 * This method initializes trackDepth	
	 * 	
	 * @return javax.swing.JSlider	
	 */
	private JSlider getTrackDepth() {
		if (trackDepth == null) {
			trackDepth = new JSlider();
			trackDepth.setPreferredSize(new Dimension(156, 18));
			trackDepth.setMaximum(127);
			trackDepth.setValue(64);
		}
		return trackDepth;
	}

	/**
	 * This method initializes txtDepth	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getTxtDepth() {
		if (txtDepth == null) {
			txtDepth = new JTextField();
			txtDepth.setPreferredSize(new Dimension(39, 19));
			txtDepth.setHorizontalAlignment(JTextField.RIGHT);
			txtDepth.setText("64");
		}
		return txtDepth;
	}

	/**
	 * This method initializes comboAttackTemplate	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getComboAttackTemplate() {
		if (comboAttackTemplate == null) {
			comboAttackTemplate = new JComboBox();
			comboAttackTemplate.setPreferredSize(new Dimension(143, 20));
		}
		return comboAttackTemplate;
	}

	/**
	 * This method initializes jPanel2	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel2() {
		if (jPanel2 == null) {
			GridBagConstraints gridBagConstraints52 = new GridBagConstraints();
			gridBagConstraints52.anchor = GridBagConstraints.SOUTHWEST;
			gridBagConstraints52.gridx = 1;
			gridBagConstraints52.gridy = 0;
			gridBagConstraints52.insets = new Insets(0, 0, 0, 16);
			GridBagConstraints gridBagConstraints42 = new GridBagConstraints();
			gridBagConstraints42.anchor = GridBagConstraints.WEST;
			gridBagConstraints42.gridx = 0;
			gridBagConstraints42.gridy = 0;
			gridBagConstraints42.insets = new Insets(0, 0, 0, 16);
			jPanel2 = new JPanel();
			jPanel2.setLayout(new GridBagLayout());
			jPanel2.add(getBtnOK(), gridBagConstraints42);
			jPanel2.add(getBtnCancel(), gridBagConstraints52);
		}
		return jPanel2;
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

}  //  @jve:decl-index=0:visual-constraint="10,10"
