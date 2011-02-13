package org.kbinani.cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.Dimension;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BCheckBox;
import org.kbinani.windows.forms.BComboBox;
import org.kbinani.windows.forms.BDialog;
import org.kbinani.windows.forms.BGroupBox;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BPanel;
import org.kbinani.windows.forms.BSlider;
import org.kbinani.windows.forms.BTextBox;

//SECTION-END-IMPORT
public class FormNoteExpressionConfig extends BDialog {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
	private JPanel jContentPane = null;
	private BPanel panelVocaloid2Template = null;
	private BLabel lblTemplate = null;
	private BComboBox comboTemplate = null;
	private BLabel jLabel1 = null;
	private BGroupBox groupPitchControl = null;
	private BLabel lblBendDepth = null;
	private BSlider trackBendDepth = null;
	private BTextBox txtBendDepth = null;
	private BLabel jLabel3 = null;
	private BLabel lblBendLength = null;
	private BSlider trackBendLength = null;
	private BTextBox txtBendLength = null;
	private BLabel jLabel5 = null;
	private BCheckBox chkUpPortamento = null;
	private BCheckBox chkDownPortamento = null;
	private BGroupBox groupDynamicsControl = null;
	private BLabel lblDecay = null;
	private BSlider trackDecay = null;
	private BTextBox txtDecay = null;
	private BLabel jLabel31 = null;
	private BLabel lblAccent = null;
	private BSlider trackAccent = null;
	private BTextBox txtAccent = null;
	private BLabel jLabel51 = null;
	private BGroupBox groupAttack = null;
	private BLabel lblDuration = null;
	private BSlider trackDuration = null;
	private BTextBox txtDuration = null;
	private BLabel jLabel311 = null;
	private BLabel lblDepth = null;
	private BSlider trackDepth = null;
	private BTextBox txtDepth = null;
	private BLabel jLabel511 = null;
	private BLabel lblAttackTemplate = null;
	private BComboBox comboAttackTemplate = null;
	private BPanel jPanel2 = null;
	private BButton btnOK = null;
	private BButton btnCancel = null;
    private BLabel lblRightValue = null;

	//SECTION-END-FIELD
	/**
	 * This is the default constructor
	 */
	public FormNoteExpressionConfig() {
		super();
		initialize();
	}
	//SECTION-BEGIN-METHOD

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		this.setSize(403, 540);
		this.setContentPane(getJContentPane());
		this.setTitle("Default Singer Style");
		setCancelButton( btnCancel );
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
			gridBagConstraints20.fill = GridBagConstraints.BOTH;
			gridBagConstraints20.gridy = 4;
			GridBagConstraints gridBagConstraints19 = new GridBagConstraints();
			gridBagConstraints19.gridx = 0;
			gridBagConstraints19.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints19.insets = new Insets(12, 12, 0, 12);
			gridBagConstraints19.weightx = 1.0D;
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
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BPanel getPanelVocaloid2Template() {
		if (panelVocaloid2Template == null) {
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 0;
			gridBagConstraints2.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints2.weightx = 1.0D;
			gridBagConstraints2.gridy = 0;
			jLabel1 = new BLabel();
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
			lblTemplate = new BLabel();
			lblTemplate.setText("Template");
			panelVocaloid2Template = new BPanel();
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
	 * @return org.kbinani.windows.forms.BComboBox	
	 */
	private BComboBox getComboTemplate() {
		if (comboTemplate == null) {
			comboTemplate = new BComboBox();
			comboTemplate.setPreferredSize(new Dimension(140, 27));
		}
		return comboTemplate;
	}

	/**
	 * This method initializes groupPitchControl	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BGroupBox getGroupPitchControl() {
		if (groupPitchControl == null) {
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
			jLabel5 = new BLabel();
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
			lblBendLength = new BLabel();
			lblBendLength.setText("Bend Length");
			GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			gridBagConstraints7.gridx = 3;
			gridBagConstraints7.anchor = GridBagConstraints.WEST;
			gridBagConstraints7.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints7.weightx = 1.0D;
			gridBagConstraints7.gridy = 0;
			jLabel3 = new BLabel();
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
			lblBendDepth = new BLabel();
			lblBendDepth.setText("Bend Depth");
			groupPitchControl = new BGroupBox();
			groupPitchControl.setLayout(new GridBagLayout());
			groupPitchControl.setTitle("Pitch Control (VOCALOID2)");
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
		}
		return groupPitchControl;
	}

	/**
	 * This method initializes trackBendDepth	
	 * 	
	 * @return org.kbinani.windows.forms.BSlider	
	 */
	private BSlider getTrackBendDepth() {
		if (trackBendDepth == null) {
			trackBendDepth = new BSlider();
			trackBendDepth.setPreferredSize(new Dimension(156, 29));
			trackBendDepth.setValue(8);
		}
		return trackBendDepth;
	}

	/**
	 * This method initializes txtBendDepth	
	 * 	
	 * @return org.kbinani.windows.forms.BTextBox	
	 */
	private BTextBox getTxtBendDepth() {
		if (txtBendDepth == null) {
			txtBendDepth = new BTextBox();
			txtBendDepth.setText("8");
			txtBendDepth.setHorizontalAlignment(BTextBox.RIGHT);
			txtBendDepth.setPreferredSize(new Dimension(39, 19));
		}
		return txtBendDepth;
	}

	/**
	 * This method initializes trackBendLength	
	 * 	
	 * @return org.kbinani.windows.forms.BSlider	
	 */
	private BSlider getTrackBendLength() {
		if (trackBendLength == null) {
			trackBendLength = new BSlider();
			trackBendLength.setPreferredSize(new Dimension(156, 29));
			trackBendLength.setValue(0);
		}
		return trackBendLength;
	}

	/**
	 * This method initializes txtBendLength	
	 * 	
	 * @return org.kbinani.windows.forms.BTextBox	
	 */
	private BTextBox getTxtBendLength() {
		if (txtBendLength == null) {
			txtBendLength = new BTextBox();
			txtBendLength.setText("0");
			txtBendLength.setPreferredSize(new Dimension(39, 19));
			txtBendLength.setHorizontalAlignment(BTextBox.RIGHT);
		}
		return txtBendLength;
	}

	/**
	 * This method initializes chkUpPortamento	
	 * 	
	 * @return org.kbinani.windows.forms.BCheckBox	
	 */
	private BCheckBox getChkUpPortamento() {
		if (chkUpPortamento == null) {
			chkUpPortamento = new BCheckBox();
			chkUpPortamento.setText("Add portamento in rising movement");
		}
		return chkUpPortamento;
	}

	/**
	 * This method initializes chkDownPortamento	
	 * 	
	 * @return org.kbinani.windows.forms.BCheckBox	
	 */
	private BCheckBox getChkDownPortamento() {
		if (chkDownPortamento == null) {
			chkDownPortamento = new BCheckBox();
			chkDownPortamento.setText("Add portamento in falling movement");
		}
		return chkDownPortamento;
	}

	/**
	 * This method initializes groupDynamicsControl	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BGroupBox getGroupDynamicsControl() {
		if (groupDynamicsControl == null) {
			GridBagConstraints gridBagConstraints111 = new GridBagConstraints();
			gridBagConstraints111.gridx = 3;
			gridBagConstraints111.anchor = GridBagConstraints.WEST;
			gridBagConstraints111.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints111.weightx = 1.0D;
			gridBagConstraints111.gridy = 1;
			jLabel51 = new BLabel();
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
			lblAccent = new BLabel();
			lblAccent.setText("Accent");
			GridBagConstraints gridBagConstraints71 = new GridBagConstraints();
			gridBagConstraints71.gridx = 3;
			gridBagConstraints71.weightx = 1.0D;
			gridBagConstraints71.anchor = GridBagConstraints.WEST;
			gridBagConstraints71.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints71.gridy = 0;
			jLabel31 = new BLabel();
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
			lblDecay = new BLabel();
			lblDecay.setText("Decay");
			groupDynamicsControl = new BGroupBox();
			groupDynamicsControl.setLayout(new GridBagLayout());
			groupDynamicsControl.setTitle("Dynamics Control (VOCALOID2)");
			groupDynamicsControl.add(lblDecay, gridBagConstraints41);
			groupDynamicsControl.add(getTrackDecay(), gridBagConstraints51);
			groupDynamicsControl.add(getTxtDecay(), gridBagConstraints61);
			groupDynamicsControl.add(jLabel31, gridBagConstraints71);
			groupDynamicsControl.add(lblAccent, gridBagConstraints81);
			groupDynamicsControl.add(getTrackAccent(), gridBagConstraints91);
			groupDynamicsControl.add(getTxtAccent(), gridBagConstraints101);
			groupDynamicsControl.add(jLabel51, gridBagConstraints111);
		}
		return groupDynamicsControl;
	}

	/**
	 * This method initializes trackDecay	
	 * 	
	 * @return org.kbinani.windows.forms.BSlider	
	 */
	private BSlider getTrackDecay() {
		if (trackDecay == null) {
			trackDecay = new BSlider();
			trackDecay.setPreferredSize(new Dimension(156, 29));
		}
		return trackDecay;
	}

	/**
	 * This method initializes txtDecay	
	 * 	
	 * @return org.kbinani.windows.forms.BTextBox	
	 */
	private BTextBox getTxtDecay() {
		if (txtDecay == null) {
			txtDecay = new BTextBox();
			txtDecay.setPreferredSize(new Dimension(39, 19));
			txtDecay.setHorizontalAlignment(BTextBox.RIGHT);
			txtDecay.setText("50");
		}
		return txtDecay;
	}

	/**
	 * This method initializes trackAccent	
	 * 	
	 * @return org.kbinani.windows.forms.BSlider	
	 */
	private BSlider getTrackAccent() {
		if (trackAccent == null) {
			trackAccent = new BSlider();
			trackAccent.setPreferredSize(new Dimension(156, 29));
		}
		return trackAccent;
	}

	/**
	 * This method initializes txtAccent	
	 * 	
	 * @return org.kbinani.windows.forms.BTextBox	
	 */
	private BTextBox getTxtAccent() {
		if (txtAccent == null) {
			txtAccent = new BTextBox();
			txtAccent.setPreferredSize(new Dimension(39, 19));
			txtAccent.setHorizontalAlignment(BTextBox.RIGHT);
			txtAccent.setText("50");
		}
		return txtAccent;
	}

	/**
	 * This method initializes groupAttack	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BGroupBox getGroupAttack() {
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
			lblAttackTemplate = new BLabel();
			lblAttackTemplate.setText("Attack Variation");
			GridBagConstraints gridBagConstraints1111 = new GridBagConstraints();
			gridBagConstraints1111.gridx = 3;
			gridBagConstraints1111.anchor = GridBagConstraints.WEST;
			gridBagConstraints1111.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints1111.weightx = 1.0D;
			gridBagConstraints1111.gridy = 2;
			jLabel511 = new BLabel();
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
			lblDepth = new BLabel();
			lblDepth.setText("Depth");
			GridBagConstraints gridBagConstraints711 = new GridBagConstraints();
			gridBagConstraints711.gridx = 3;
			gridBagConstraints711.weightx = 1.0D;
			gridBagConstraints711.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints711.anchor = GridBagConstraints.WEST;
			gridBagConstraints711.gridy = 1;
			jLabel311 = new BLabel();
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
			lblDuration = new BLabel();
			lblDuration.setText("Duration");
			groupAttack = new BGroupBox();
			groupAttack.setLayout(new GridBagLayout());
			groupAttack.setTitle("Attack (VOCALOID1)");
			groupAttack.add(lblDuration, gridBagConstraints411);
			groupAttack.add(getTrackDuration(), gridBagConstraints511);
			groupAttack.add(getTxtDuration(), gridBagConstraints611);
			groupAttack.add(jLabel311, gridBagConstraints711);
			groupAttack.add(lblDepth, gridBagConstraints811);
			groupAttack.add(getTrackDepth(), gridBagConstraints911);
			groupAttack.add(getTxtDepth(), gridBagConstraints1011);
			groupAttack.add(jLabel511, gridBagConstraints1111);
			groupAttack.add(lblAttackTemplate, gridBagConstraints17);
			groupAttack.add(getComboAttackTemplate(), gridBagConstraints18);
		}
		return groupAttack;
	}

	/**
	 * This method initializes trackDuration	
	 * 	
	 * @return org.kbinani.windows.forms.BSlider	
	 */
	private BSlider getTrackDuration() {
		if (trackDuration == null) {
			trackDuration = new BSlider();
			trackDuration.setPreferredSize(new Dimension(156, 29));
			trackDuration.setValue(64);
			trackDuration.setMaximum(127);
		}
		return trackDuration;
	}

	/**
	 * This method initializes txtDuration	
	 * 	
	 * @return org.kbinani.windows.forms.BTextBox	
	 */
	private BTextBox getTxtDuration() {
		if (txtDuration == null) {
			txtDuration = new BTextBox();
			txtDuration.setPreferredSize(new Dimension(39, 19));
			txtDuration.setHorizontalAlignment(BTextBox.RIGHT);
			txtDuration.setText("64");
		}
		return txtDuration;
	}

	/**
	 * This method initializes trackDepth	
	 * 	
	 * @return org.kbinani.windows.forms.BSlider	
	 */
	private BSlider getTrackDepth() {
		if (trackDepth == null) {
			trackDepth = new BSlider();
			trackDepth.setPreferredSize(new Dimension(156, 29));
			trackDepth.setMaximum(127);
			trackDepth.setValue(64);
		}
		return trackDepth;
	}

	/**
	 * This method initializes txtDepth	
	 * 	
	 * @return org.kbinani.windows.forms.BTextBox	
	 */
	private BTextBox getTxtDepth() {
		if (txtDepth == null) {
			txtDepth = new BTextBox();
			txtDepth.setPreferredSize(new Dimension(39, 19));
			txtDepth.setHorizontalAlignment(BTextBox.RIGHT);
			txtDepth.setText("64");
		}
		return txtDepth;
	}

	/**
	 * This method initializes comboAttackTemplate	
	 * 	
	 * @return org.kbinani.windows.forms.BComboBox	
	 */
	private BComboBox getComboAttackTemplate() {
		if (comboAttackTemplate == null) {
			comboAttackTemplate = new BComboBox();
			comboAttackTemplate.setPreferredSize(new Dimension(143, 27));
		}
		return comboAttackTemplate;
	}

	/**
	 * This method initializes jPanel2	
	 * 	
	 * @return org.kbinani.windows.forms.BPanel	
	 */
	private BPanel getJPanel2() {
		if (jPanel2 == null) {
			GridBagConstraints gridBagConstraints14 = new GridBagConstraints();
			gridBagConstraints14.gridx = 0;
			gridBagConstraints14.weightx = 1.0D;
			gridBagConstraints14.gridy = 0;
			lblRightValue = new BLabel();
			lblRightValue.setPreferredSize(new Dimension(4, 4));
			lblRightValue.setText("");
			GridBagConstraints gridBagConstraints52 = new GridBagConstraints();
			gridBagConstraints52.anchor = GridBagConstraints.SOUTHWEST;
			gridBagConstraints52.gridx = 1;
			gridBagConstraints52.gridy = 0;
			gridBagConstraints52.insets = new Insets(0, 0, 0, 0);
			GridBagConstraints gridBagConstraints42 = new GridBagConstraints();
			gridBagConstraints42.anchor = GridBagConstraints.WEST;
			gridBagConstraints42.gridx = 2;
			gridBagConstraints42.gridy = 0;
			gridBagConstraints42.insets = new Insets(0, 0, 0, 12);
			jPanel2 = new BPanel();
			jPanel2.setLayout(new GridBagLayout());
			jPanel2.add(getBtnOK(), gridBagConstraints42);
			jPanel2.add(getBtnCancel(), gridBagConstraints52);
			jPanel2.add(lblRightValue, gridBagConstraints14);
		}
		return jPanel2;
	}

	/**
	 * This method initializes btnOK	
	 * 	
	 * @return org.kbinani.windows.forms.BButton	
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
	 * @return org.kbinani.windows.forms.BButton	
	 */
	private BButton getBtnCancel() {
		if (btnCancel == null) {
			btnCancel = new BButton();
			btnCancel.setText("Cancel");
			btnCancel.setPreferredSize(new Dimension(100, 29));
		}
		return btnCancel;
	}

	//SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
