package org.kbinani.Cadencii;

//SECTION-BEGIN-IMPORT
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Insets;
import javax.swing.ImageIcon;
import javax.swing.JPanel;
import org.kbinani.windows.forms.BButton;
import org.kbinani.windows.forms.BCheckBox;
import org.kbinani.windows.forms.BForm;
import org.kbinani.windows.forms.BGroupBox;
import org.kbinani.windows.forms.BLabel;
import org.kbinani.windows.forms.BTextBox;

//SECTION-END-IMPORT
public class FormBezierPointEdit extends BForm {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
	private JPanel jContentPane = null;
	private BButton btnBackward = null;
	private BCheckBox chkEnableSmooth = null;
	private BButton btnForward = null;
	private BGroupBox groupLeft = null;
	private BLabel lblLeftClock = null;
	private BTextBox txtLeftClock = null;
	private BLabel lblLeftValue = null;
	private BTextBox txtLeftValue = null;
	private BButton btnLeft = null;
	private BGroupBox groupDataPoint = null;
	private BLabel lblDataPointClock = null;
	private BTextBox txtDataPointClock = null;
	private BLabel lblDataPointValue = null;
	private BTextBox txtDataPointValue = null;
	private BButton btnDataPoint = null;
	private BGroupBox groupRight = null;
	private BLabel lblRightClock = null;
	private BTextBox txtRightClock = null;
	private BLabel lblRightValue = null;
	private BTextBox txtRightValue = null;
	private BButton btnRight = null;
	private BButton btnOK = null;
	private BButton btnCancel = null;
	private BLabel jLabel4 = null;
	private BLabel jLabel5 = null;
	private JPanel jPanel3 = null;

	//SECTION-END-FIELD
	/**
	 * This is the default constructor
	 */
	public FormBezierPointEdit() {
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
		this.setSize(469, 266);
		this.setContentPane(getJContentPane());
		this.setTitle("Edit Bezier Data Point");
	}

	/**
	 * This method initializes jContentPane
	 * 
	 * @return javax.swing.JPanel
	 */
	private JPanel getJContentPane() {
		if (jContentPane == null) {
			GridBagConstraints gridBagConstraints91 = new GridBagConstraints();
			gridBagConstraints91.gridx = 0;
			gridBagConstraints91.gridwidth = 3;
			gridBagConstraints91.anchor = GridBagConstraints.EAST;
			gridBagConstraints91.gridy = 4;
			GridBagConstraints gridBagConstraints81 = new GridBagConstraints();
			gridBagConstraints81.gridx = 0;
			gridBagConstraints81.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints81.gridwidth = 3;
			gridBagConstraints81.gridy = 3;
			jLabel5 = new BLabel();
			jLabel5.setText("    ");
			GridBagConstraints gridBagConstraints73 = new GridBagConstraints();
			gridBagConstraints73.gridx = 0;
			gridBagConstraints73.gridwidth = 3;
			gridBagConstraints73.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints73.gridy = 1;
			jLabel4 = new BLabel();
			jLabel4.setText("     ");
			GridBagConstraints gridBagConstraints13 = new GridBagConstraints();
			gridBagConstraints13.gridx = 2;
			gridBagConstraints13.gridy = 1;
			GridBagConstraints gridBagConstraints10 = new GridBagConstraints();
			gridBagConstraints10.gridx = 2;
			gridBagConstraints10.weightx = 1.0D;
			gridBagConstraints10.fill = GridBagConstraints.BOTH;
			gridBagConstraints10.insets = new Insets(5, 5, 5, 5);
			gridBagConstraints10.gridy = 2;
			GridBagConstraints gridBagConstraints9 = new GridBagConstraints();
			gridBagConstraints9.gridx = 1;
			gridBagConstraints9.weightx = 1.0D;
			gridBagConstraints9.fill = GridBagConstraints.BOTH;
			gridBagConstraints9.insets = new Insets(5, 5, 5, 5);
			gridBagConstraints9.gridy = 2;
			GridBagConstraints gridBagConstraints8 = new GridBagConstraints();
			gridBagConstraints8.gridx = 0;
			gridBagConstraints8.weightx = 1.0D;
			gridBagConstraints8.fill = GridBagConstraints.BOTH;
			gridBagConstraints8.insets = new Insets(5, 5, 5, 5);
			gridBagConstraints8.gridy = 2;
			GridBagConstraints gridBagConstraints2 = new GridBagConstraints();
			gridBagConstraints2.gridx = 2;
			gridBagConstraints2.anchor = GridBagConstraints.WEST;
			gridBagConstraints2.gridy = 0;
			GridBagConstraints gridBagConstraints1 = new GridBagConstraints();
			gridBagConstraints1.gridx = 1;
			gridBagConstraints1.gridy = 0;
			GridBagConstraints gridBagConstraints = new GridBagConstraints();
			gridBagConstraints.gridx = 0;
			gridBagConstraints.anchor = GridBagConstraints.EAST;
			gridBagConstraints.gridy = 0;
			jContentPane = new JPanel();
			jContentPane.setLayout(new GridBagLayout());
			jContentPane.add(getBtnBackward(), gridBagConstraints);
			jContentPane.add(getChkEnableSmooth(), gridBagConstraints1);
			jContentPane.add(getBtnForward(), gridBagConstraints2);
			jContentPane.add(getGroupLeft(), gridBagConstraints8);
			jContentPane.add(getGroupDataPoint(), gridBagConstraints9);
			jContentPane.add(getGroupRight(), gridBagConstraints10);
			jContentPane.add(jLabel4, gridBagConstraints73);
			jContentPane.add(jLabel5, gridBagConstraints81);
			jContentPane.add(getJPanel3(), gridBagConstraints91);
			jContentPane.add(jLabel4, gridBagConstraints13);
		}
		return jContentPane;
	}

	/**
	 * This method initializes btnBackward	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnBackward() {
		if (btnBackward == null) {
			btnBackward = new BButton();
			btnBackward.setText("<<");
		}
		return btnBackward;
	}

	/**
	 * This method initializes chkEnableSmooth	
	 * 	
	 * @return javax.swing.BCheckBox	
	 */
	private BCheckBox getChkEnableSmooth() {
		if (chkEnableSmooth == null) {
			chkEnableSmooth = new BCheckBox();
			chkEnableSmooth.setText("Smooth");
		}
		return chkEnableSmooth;
	}

	/**
	 * This method initializes btnForward	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnForward() {
		if (btnForward == null) {
			btnForward = new BButton();
			btnForward.setText(">>");
		}
		return btnForward;
	}

	/**
	 * This method initializes groupLeft	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupLeft() {
		if (groupLeft == null) {
			GridBagConstraints gridBagConstraints7 = new GridBagConstraints();
			gridBagConstraints7.gridx = 0;
			gridBagConstraints7.gridwidth = 2;
			gridBagConstraints7.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints7.weightx = 1.0D;
			gridBagConstraints7.ipadx = 0;
			gridBagConstraints7.ipady = 0;
			gridBagConstraints7.insets = new Insets(5, 20, 5, 20);
			gridBagConstraints7.gridy = 3;
			GridBagConstraints gridBagConstraints6 = new GridBagConstraints();
			gridBagConstraints6.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints6.gridy = 1;
			gridBagConstraints6.weightx = 1.0;
			gridBagConstraints6.insets = new Insets(5, 5, 5, 15);
			gridBagConstraints6.gridx = 1;
			GridBagConstraints gridBagConstraints5 = new GridBagConstraints();
			gridBagConstraints5.gridx = 0;
			gridBagConstraints5.insets = new Insets(0, 15, 0, 0);
			gridBagConstraints5.gridy = 1;
			lblLeftValue = new BLabel();
			lblLeftValue.setText("Value");
			GridBagConstraints gridBagConstraints4 = new GridBagConstraints();
			gridBagConstraints4.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints4.gridy = 0;
			gridBagConstraints4.weightx = 1.0D;
			gridBagConstraints4.insets = new Insets(5, 5, 5, 15);
			gridBagConstraints4.gridx = 1;
			GridBagConstraints gridBagConstraints3 = new GridBagConstraints();
			gridBagConstraints3.gridx = 0;
			gridBagConstraints3.insets = new Insets(0, 15, 0, 0);
			gridBagConstraints3.gridy = 0;
			lblLeftClock = new BLabel();
			lblLeftClock.setText("Clock");
			groupLeft = new BGroupBox();
			groupLeft.setLayout(new GridBagLayout());
			groupLeft.setTitle("Left Control Point");
			groupLeft.add(lblLeftClock, gridBagConstraints3);
			groupLeft.add(getJTextField(), gridBagConstraints4);
			groupLeft.add(lblLeftValue, gridBagConstraints5);
			groupLeft.add(getTxtLeftValue(), gridBagConstraints6);
			groupLeft.add(getBtnLeft(), gridBagConstraints7);
		}
		return groupLeft;
	}

	/**
	 * This method initializes BTextBox	
	 * 	
	 * @return javax.swing.BTextBox	
	 */
	private BTextBox getJTextField() {
		if (txtLeftClock == null) {
			txtLeftClock = new BTextBox();
		}
		return txtLeftClock;
	}

	/**
	 * This method initializes txtLeftValue	
	 * 	
	 * @return javax.swing.BTextBox	
	 */
	private BTextBox getTxtLeftValue() {
		if (txtLeftValue == null) {
			txtLeftValue = new BTextBox();
		}
		return txtLeftValue;
	}

	/**
	 * This method initializes btnLeft	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnLeft() {
		if (btnLeft == null) {
			btnLeft = new BButton();
			btnLeft.setText("");
			btnLeft.setIcon(new ImageIcon(getClass().getResource("/target--pencil.png")));
		}
		return btnLeft;
	}

	/**
	 * This method initializes groupDataPoint	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupDataPoint() {
		if (groupDataPoint == null) {
			GridBagConstraints gridBagConstraints71 = new GridBagConstraints();
			gridBagConstraints71.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints71.gridx = 0;
			gridBagConstraints71.gridy = 2;
			gridBagConstraints71.weightx = 1.0D;
			gridBagConstraints71.insets = new Insets(5, 20, 5, 20);
			gridBagConstraints71.gridwidth = 2;
			GridBagConstraints gridBagConstraints61 = new GridBagConstraints();
			gridBagConstraints61.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints61.gridy = 1;
			gridBagConstraints61.weightx = 1.0;
			gridBagConstraints61.insets = new Insets(5, 5, 5, 15);
			gridBagConstraints61.gridx = 1;
			GridBagConstraints gridBagConstraints51 = new GridBagConstraints();
			gridBagConstraints51.gridx = 0;
			gridBagConstraints51.anchor = GridBagConstraints.WEST;
			gridBagConstraints51.insets = new Insets(0, 15, 0, 0);
			gridBagConstraints51.gridy = 1;
			lblDataPointValue = new BLabel();
			lblDataPointValue.setText("Value");
			GridBagConstraints gridBagConstraints41 = new GridBagConstraints();
			gridBagConstraints41.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints41.gridy = 0;
			gridBagConstraints41.weightx = 1.0D;
			gridBagConstraints41.insets = new Insets(5, 5, 5, 15);
			gridBagConstraints41.gridx = 1;
			GridBagConstraints gridBagConstraints31 = new GridBagConstraints();
			gridBagConstraints31.gridx = 0;
			gridBagConstraints31.insets = new Insets(0, 15, 0, 0);
			gridBagConstraints31.anchor = GridBagConstraints.WEST;
			gridBagConstraints31.gridy = 0;
			lblDataPointClock = new BLabel();
			lblDataPointClock.setText("Clock");
			groupDataPoint = new BGroupBox();
			groupDataPoint.setLayout(new GridBagLayout());
			groupDataPoint.setTitle("Data Point");
			groupDataPoint.add(lblDataPointClock, gridBagConstraints31);
			groupDataPoint.add(getTxtDataPointClock(), gridBagConstraints41);
			groupDataPoint.add(lblDataPointValue, gridBagConstraints51);
			groupDataPoint.add(getTxtDataPointValue(), gridBagConstraints61);
			groupDataPoint.add(getBtnDataPoint(), gridBagConstraints71);
		}
		return groupDataPoint;
	}

	/**
	 * This method initializes txtDataPointClock	
	 * 	
	 * @return javax.swing.BTextBox	
	 */
	private BTextBox getTxtDataPointClock() {
		if (txtDataPointClock == null) {
			txtDataPointClock = new BTextBox();
		}
		return txtDataPointClock;
	}

	/**
	 * This method initializes txtDataPointValue	
	 * 	
	 * @return javax.swing.BTextBox	
	 */
	private BTextBox getTxtDataPointValue() {
		if (txtDataPointValue == null) {
			txtDataPointValue = new BTextBox();
		}
		return txtDataPointValue;
	}

	/**
	 * This method initializes btnDataPoint	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnDataPoint() {
		if (btnDataPoint == null) {
			btnDataPoint = new BButton();
			btnDataPoint.setText("");
			btnDataPoint.setIcon(new ImageIcon(getClass().getResource("/target--pencil.png")));
		}
		return btnDataPoint;
	}

	/**
	 * This method initializes groupRight	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getGroupRight() {
		if (groupRight == null) {
			GridBagConstraints gridBagConstraints72 = new GridBagConstraints();
			gridBagConstraints72.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints72.gridx = 0;
			gridBagConstraints72.gridy = 2;
			gridBagConstraints72.weightx = 1.0D;
			gridBagConstraints72.insets = new Insets(5, 20, 5, 20);
			gridBagConstraints72.gridwidth = 2;
			GridBagConstraints gridBagConstraints62 = new GridBagConstraints();
			gridBagConstraints62.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints62.gridy = 1;
			gridBagConstraints62.weightx = 1.0;
			gridBagConstraints62.insets = new Insets(5, 5, 5, 15);
			gridBagConstraints62.gridx = 1;
			GridBagConstraints gridBagConstraints52 = new GridBagConstraints();
			gridBagConstraints52.gridx = 0;
			gridBagConstraints52.insets = new Insets(0, 15, 0, 0);
			gridBagConstraints52.gridy = 1;
			lblRightValue = new BLabel();
			lblRightValue.setText("Value");
			GridBagConstraints gridBagConstraints42 = new GridBagConstraints();
			gridBagConstraints42.fill = GridBagConstraints.HORIZONTAL;
			gridBagConstraints42.gridy = 0;
			gridBagConstraints42.weightx = 1.0D;
			gridBagConstraints42.insets = new Insets(5, 5, 5, 15);
			gridBagConstraints42.gridx = 1;
			GridBagConstraints gridBagConstraints32 = new GridBagConstraints();
			gridBagConstraints32.gridx = 0;
			gridBagConstraints32.insets = new Insets(0, 15, 0, 0);
			gridBagConstraints32.gridy = 0;
			lblRightClock = new BLabel();
			lblRightClock.setText("Clock");
			groupRight = new BGroupBox();
			groupRight.setLayout(new GridBagLayout());
			groupRight.setTitle("Right Control Point");
			groupRight.add(lblRightClock, gridBagConstraints32);
			groupRight.add(getTxtRightClock(), gridBagConstraints42);
			groupRight.add(lblRightValue, gridBagConstraints52);
			groupRight.add(getTxtRightValue(), gridBagConstraints62);
			groupRight.add(getBtnRight(), gridBagConstraints72);
		}
		return groupRight;
	}

	/**
	 * This method initializes txtRightClock	
	 * 	
	 * @return javax.swing.BTextBox	
	 */
	private BTextBox getTxtRightClock() {
		if (txtRightClock == null) {
			txtRightClock = new BTextBox();
		}
		return txtRightClock;
	}

	/**
	 * This method initializes txtRightValue	
	 * 	
	 * @return javax.swing.BTextBox	
	 */
	private BTextBox getTxtRightValue() {
		if (txtRightValue == null) {
			txtRightValue = new BTextBox();
		}
		return txtRightValue;
	}

	/**
	 * This method initializes btnRight	
	 * 	
	 * @return javax.swing.BButton	
	 */
	private BButton getBtnRight() {
		if (btnRight == null) {
			btnRight = new BButton();
			btnRight.setText("");
			btnRight.setIcon(new ImageIcon(getClass().getResource("/target--pencil.png")));
		}
		return btnRight;
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
		}
		return btnCancel;
	}

	/**
	 * This method initializes jPanel3	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel3() {
		if (jPanel3 == null) {
			GridBagConstraints gridBagConstraints11 = new GridBagConstraints();
			gridBagConstraints11.gridx = 0;
			gridBagConstraints11.insets = new Insets(0, 0, 0, 16);
			gridBagConstraints11.gridy = 0;
			GridBagConstraints gridBagConstraints12 = new GridBagConstraints();
			gridBagConstraints12.gridx = 2;
			gridBagConstraints12.insets = new Insets(0, 0, 0, 16);
			gridBagConstraints12.gridy = 0;
			jPanel3 = new JPanel();
			jPanel3.setLayout(new GridBagLayout());
			jPanel3.add(getBtnCancel(), gridBagConstraints12);
			jPanel3.add(getBtnOK(), gridBagConstraints11);
		}
		return jPanel3;
	}

	//SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
